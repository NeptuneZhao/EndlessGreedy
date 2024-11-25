using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000814 RID: 2068
public class MoltDropperMonitor : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>
{
	// Token: 0x0600393A RID: 14650 RVA: 0x00138170 File Offset: 0x00136370
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.EventHandler(GameHashes.NewDay, (MoltDropperMonitor.Instance smi) => GameClock.Instance, delegate(MoltDropperMonitor.Instance smi)
		{
			smi.spawnedThisCycle = false;
		});
		this.satisfied.UpdateTransition(this.drop, (MoltDropperMonitor.Instance smi, float dt) => smi.ShouldDropElement(), UpdateRate.SIM_4000ms, false);
		this.drop.DefaultState(this.drop.dropping);
		this.drop.dropping.EnterTransition(this.drop.complete, (MoltDropperMonitor.Instance smi) => !smi.def.synchWithBehaviour).ToggleBehaviour(GameTags.Creatures.ReadyToMolt, (MoltDropperMonitor.Instance smi) => true, delegate(MoltDropperMonitor.Instance smi)
		{
			smi.GoTo(this.drop.complete);
		});
		this.drop.complete.Enter(delegate(MoltDropperMonitor.Instance smi)
		{
			smi.Drop();
		}).TriggerOnEnter(GameHashes.Molt, null).EventTransition(GameHashes.NewDay, (MoltDropperMonitor.Instance smi) => GameClock.Instance, this.satisfied, null);
	}

	// Token: 0x04002272 RID: 8818
	public StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.BoolParameter droppedThisCycle = new StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.BoolParameter(false);

	// Token: 0x04002273 RID: 8819
	public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State satisfied;

	// Token: 0x04002274 RID: 8820
	public MoltDropperMonitor.DropStates drop;

	// Token: 0x0200171C RID: 5916
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040071BF RID: 29119
		public bool synchWithBehaviour;

		// Token: 0x040071C0 RID: 29120
		public string onGrowDropID;

		// Token: 0x040071C1 RID: 29121
		public float massToDrop;

		// Token: 0x040071C2 RID: 29122
		public string amountName;

		// Token: 0x040071C3 RID: 29123
		public Func<MoltDropperMonitor.Instance, bool> isReadyToMolt;
	}

	// Token: 0x0200171D RID: 5917
	public class DropStates : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State
	{
		// Token: 0x040071C4 RID: 29124
		public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State dropping;

		// Token: 0x040071C5 RID: 29125
		public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State complete;
	}

	// Token: 0x0200171E RID: 5918
	public new class Instance : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.GameInstance
	{
		// Token: 0x060094B4 RID: 38068 RVA: 0x0035D778 File Offset: 0x0035B978
		public Instance(IStateMachineTarget master, MoltDropperMonitor.Def def) : base(master, def)
		{
			if (!string.IsNullOrEmpty(def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(def.amountName).Lookup(base.smi.gameObject);
				amountInstance.OnMaxValueReached = (System.Action)Delegate.Combine(amountInstance.OnMaxValueReached, new System.Action(this.OnAmountMaxValueReached));
			}
		}

		// Token: 0x060094B5 RID: 38069 RVA: 0x0035D7E0 File Offset: 0x0035B9E0
		private void OnAmountMaxValueReached()
		{
			this.lastTineAmountReachedMax = GameClock.Instance.GetTime();
		}

		// Token: 0x060094B6 RID: 38070 RVA: 0x0035D7F4 File Offset: 0x0035B9F4
		protected override void OnCleanUp()
		{
			if (!string.IsNullOrEmpty(base.def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(base.def.amountName).Lookup(base.smi.gameObject);
				amountInstance.OnMaxValueReached = (System.Action)Delegate.Remove(amountInstance.OnMaxValueReached, new System.Action(this.OnAmountMaxValueReached));
			}
			base.OnCleanUp();
		}

		// Token: 0x060094B7 RID: 38071 RVA: 0x0035D864 File Offset: 0x0035BA64
		public bool ShouldDropElement()
		{
			return base.def.isReadyToMolt(this);
		}

		// Token: 0x060094B8 RID: 38072 RVA: 0x0035D878 File Offset: 0x0035BA78
		public void Drop()
		{
			GameObject gameObject = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, base.def.onGrowDropID, Grid.SceneLayer.Ore);
			gameObject.SetActive(true);
			gameObject.GetComponent<PrimaryElement>().Mass = base.def.massToDrop;
			this.spawnedThisCycle = true;
			this.timeOfLastDrop = GameClock.Instance.GetTime();
			if (!string.IsNullOrEmpty(base.def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(base.def.amountName).Lookup(base.smi.gameObject);
				amountInstance.value = amountInstance.GetMin();
			}
		}

		// Token: 0x060094B9 RID: 38073 RVA: 0x0035D91C File Offset: 0x0035BB1C
		private int GetDropSpawnLocation()
		{
			int num = Grid.PosToCell(base.gameObject);
			int num2 = Grid.CellAbove(num);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				return num2;
			}
			return num;
		}

		// Token: 0x040071C6 RID: 29126
		[MyCmpGet]
		public KPrefabID prefabID;

		// Token: 0x040071C7 RID: 29127
		[Serialize]
		public bool spawnedThisCycle;

		// Token: 0x040071C8 RID: 29128
		[Serialize]
		public float timeOfLastDrop;

		// Token: 0x040071C9 RID: 29129
		[Serialize]
		public float lastTineAmountReachedMax;
	}
}
