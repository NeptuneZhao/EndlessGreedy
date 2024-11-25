using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class BeeSleepStates : GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>
{
	// Token: 0x06000371 RID: 881 RVA: 0x0001CA98 File Offset: 0x0001AC98
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findSleepLocation;
		GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.SLEEPING.NAME;
		string tooltip = CREATURES.STATUSITEMS.SLEEPING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.findSleepLocation.Enter(delegate(BeeSleepStates.Instance smi)
		{
			BeeSleepStates.FindSleepLocation(smi);
			if (smi.targetSleepCell != Grid.InvalidCell)
			{
				smi.GoTo(this.moveToSleepLocation);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.moveToSleepLocation.MoveTo((BeeSleepStates.Instance smi) => smi.targetSleepCell, this.sleep.pre, this.behaviourcomplete, false);
		this.sleep.Enter("EnableGravity", delegate(BeeSleepStates.Instance smi)
		{
			GameComps.Gravities.Add(smi.gameObject, Vector2.zero, delegate()
			{
				if (GameComps.Gravities.Has(smi.gameObject))
				{
					GameComps.Gravities.Remove(smi.gameObject);
				}
			});
		}).TriggerOnEnter(GameHashes.SleepStarted, null).TriggerOnExit(GameHashes.SleepFinished, null).Transition(this.sleep.pst, new StateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.Transition.ConditionCallback(BeeSleepStates.ShouldWakeUp), UpdateRate.SIM_1000ms);
		this.sleep.pre.QueueAnim("sleep_pre", false, null).OnAnimQueueComplete(this.sleep.loop);
		this.sleep.loop.Enter(delegate(BeeSleepStates.Instance smi)
		{
			smi.GetComponent<LoopingSounds>().PauseSound(GlobalAssets.GetSound("Bee_wings_LP", false), true);
		}).QueueAnim("sleep_loop", true, null).Exit(delegate(BeeSleepStates.Instance smi)
		{
			smi.GetComponent<LoopingSounds>().PauseSound(GlobalAssets.GetSound("Bee_wings_LP", false), false);
		});
		this.sleep.pst.QueueAnim("sleep_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.BeeWantsToSleep, false);
	}

	// Token: 0x06000372 RID: 882 RVA: 0x0001CC6C File Offset: 0x0001AE6C
	private static void FindSleepLocation(BeeSleepStates.Instance smi)
	{
		smi.targetSleepCell = Grid.InvalidCell;
		FloorCellQuery floorCellQuery = PathFinderQueries.floorCellQuery.Reset(1, 0);
		smi.GetComponent<Navigator>().RunQuery(floorCellQuery);
		if (floorCellQuery.result_cells.Count > 0)
		{
			smi.targetSleepCell = floorCellQuery.result_cells[UnityEngine.Random.Range(0, floorCellQuery.result_cells.Count)];
		}
	}

	// Token: 0x06000373 RID: 883 RVA: 0x0001CCCD File Offset: 0x0001AECD
	public static bool ShouldWakeUp(BeeSleepStates.Instance smi)
	{
		return smi.GetSMI<BeeSleepMonitor.Instance>().CO2Exposure <= 0f;
	}

	// Token: 0x04000268 RID: 616
	public BeeSleepStates.SleepStates sleep;

	// Token: 0x04000269 RID: 617
	public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State findSleepLocation;

	// Token: 0x0400026A RID: 618
	public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State moveToSleepLocation;

	// Token: 0x0400026B RID: 619
	public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State behaviourcomplete;

	// Token: 0x02000FFB RID: 4091
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000FFC RID: 4092
	public new class Instance : GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.GameInstance
	{
		// Token: 0x06007AF1 RID: 31473 RVA: 0x00302DBE File Offset: 0x00300FBE
		public Instance(Chore<BeeSleepStates.Instance> chore, BeeSleepStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.BeeWantsToSleep);
		}

		// Token: 0x04005BE8 RID: 23528
		public int targetSleepCell;

		// Token: 0x04005BE9 RID: 23529
		public float co2Exposure;
	}

	// Token: 0x02000FFD RID: 4093
	public class SleepStates : GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State
	{
		// Token: 0x04005BEA RID: 23530
		public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State pre;

		// Token: 0x04005BEB RID: 23531
		public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State loop;

		// Token: 0x04005BEC RID: 23532
		public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State pst;
	}
}
