using System;
using UnityEngine;

// Token: 0x020000C6 RID: 198
public class ConduitSleepMonitor : GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>
{
	// Token: 0x0600038E RID: 910 RVA: 0x0001D938 File Offset: 0x0001BB38
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.idle.Enter(delegate(ConduitSleepMonitor.Instance smi)
		{
			this.targetSleepCell.Set(Grid.InvalidCell, smi, false);
			smi.GetComponent<Staterpillar>().DestroyOrphanedConnectorBuilding();
		}).EventTransition(GameHashes.NewBlock, (ConduitSleepMonitor.Instance smi) => GameClock.Instance, this.searching.looking, new StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.Transition.ConditionCallback(ConduitSleepMonitor.IsSleepyTime));
		this.searching.Enter(new StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State.Callback(this.TryRecoverSave)).EventTransition(GameHashes.NewBlock, (ConduitSleepMonitor.Instance smi) => GameClock.Instance, this.idle, GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.Not(new StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.Transition.ConditionCallback(ConduitSleepMonitor.IsSleepyTime))).Exit(delegate(ConduitSleepMonitor.Instance smi)
		{
			this.targetSleepCell.Set(Grid.InvalidCell, smi, false);
			smi.GetComponent<Staterpillar>().DestroyOrphanedConnectorBuilding();
		});
		this.searching.looking.Update(delegate(ConduitSleepMonitor.Instance smi, float dt)
		{
			this.FindSleepLocation(smi);
		}, UpdateRate.SIM_1000ms, false).ToggleStatusItem(Db.Get().CreatureStatusItems.NoSleepSpot, null).ParamTransition<int>(this.targetSleepCell, this.searching.found, (ConduitSleepMonitor.Instance smi, int sleepCell) => sleepCell != Grid.InvalidCell);
		this.searching.found.Enter(delegate(ConduitSleepMonitor.Instance smi)
		{
			smi.GetComponent<Staterpillar>().SpawnConnectorBuilding(this.targetSleepCell.Get(smi));
		}).ParamTransition<int>(this.targetSleepCell, this.searching.looking, (ConduitSleepMonitor.Instance smi, int sleepCell) => sleepCell == Grid.InvalidCell).ToggleBehaviour(GameTags.Creatures.WantsConduitConnection, (ConduitSleepMonitor.Instance smi) => this.targetSleepCell.Get(smi) != Grid.InvalidCell && ConduitSleepMonitor.IsSleepyTime(smi), null);
	}

	// Token: 0x0600038F RID: 911 RVA: 0x0001DAE7 File Offset: 0x0001BCE7
	public static bool IsSleepyTime(ConduitSleepMonitor.Instance smi)
	{
		return GameClock.Instance.GetTimeSinceStartOfCycle() >= 500f;
	}

	// Token: 0x06000390 RID: 912 RVA: 0x0001DB00 File Offset: 0x0001BD00
	private void TryRecoverSave(ConduitSleepMonitor.Instance smi)
	{
		Staterpillar component = smi.GetComponent<Staterpillar>();
		if (this.targetSleepCell.Get(smi) == Grid.InvalidCell && component.IsConnectorBuildingSpawned())
		{
			int value = Grid.PosToCell(component.GetConnectorBuilding());
			this.targetSleepCell.Set(value, smi, false);
		}
	}

	// Token: 0x06000391 RID: 913 RVA: 0x0001DB4C File Offset: 0x0001BD4C
	private void FindSleepLocation(ConduitSleepMonitor.Instance smi)
	{
		StaterpillarCellQuery staterpillarCellQuery = PathFinderQueries.staterpillarCellQuery.Reset(10, smi.gameObject, smi.def.conduitLayer);
		smi.GetComponent<Navigator>().RunQuery(staterpillarCellQuery);
		if (staterpillarCellQuery.result_cells.Count > 0)
		{
			foreach (int num in staterpillarCellQuery.result_cells)
			{
				int cellInDirection = Grid.GetCellInDirection(num, Direction.Down);
				if (Grid.Objects[cellInDirection, (int)smi.def.conduitLayer] != null)
				{
					this.targetSleepCell.Set(num, smi, false);
					break;
				}
			}
			if (this.targetSleepCell.Get(smi) == Grid.InvalidCell)
			{
				this.targetSleepCell.Set(staterpillarCellQuery.result_cells[UnityEngine.Random.Range(0, staterpillarCellQuery.result_cells.Count)], smi, false);
			}
		}
	}

	// Token: 0x04000279 RID: 633
	private GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State idle;

	// Token: 0x0400027A RID: 634
	private ConduitSleepMonitor.SleepSearchStates searching;

	// Token: 0x0400027B RID: 635
	public StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.IntParameter targetSleepCell = new StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.IntParameter(Grid.InvalidCell);

	// Token: 0x02001012 RID: 4114
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005C24 RID: 23588
		public ObjectLayer conduitLayer;
	}

	// Token: 0x02001013 RID: 4115
	private class SleepSearchStates : GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State
	{
		// Token: 0x04005C25 RID: 23589
		public GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State looking;

		// Token: 0x04005C26 RID: 23590
		public GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State found;
	}

	// Token: 0x02001014 RID: 4116
	public new class Instance : GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.GameInstance
	{
		// Token: 0x06007B2E RID: 31534 RVA: 0x00303323 File Offset: 0x00301523
		public Instance(IStateMachineTarget master, ConduitSleepMonitor.Def def) : base(master, def)
		{
		}
	}
}
