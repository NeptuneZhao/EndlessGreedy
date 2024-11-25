using System;

// Token: 0x02000551 RID: 1361
public class IncubatorMonitor : GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>
{
	// Token: 0x06001F41 RID: 8001 RVA: 0x000AF370 File Offset: 0x000AD570
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.not;
		this.not.EventTransition(GameHashes.OnStore, this.in_incubator, new StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback(IncubatorMonitor.InIncubator));
		this.in_incubator.ToggleTag(GameTags.Creatures.InIncubator).EventTransition(GameHashes.OnStore, this.not, GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Not(new StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback(IncubatorMonitor.InIncubator)));
	}

	// Token: 0x06001F42 RID: 8002 RVA: 0x000AF3DA File Offset: 0x000AD5DA
	public static bool InIncubator(IncubatorMonitor.Instance smi)
	{
		return smi.gameObject.transform.parent && smi.gameObject.transform.parent.GetComponent<EggIncubator>() != null;
	}

	// Token: 0x0400119D RID: 4509
	public GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State not;

	// Token: 0x0400119E RID: 4510
	public GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State in_incubator;

	// Token: 0x02001341 RID: 4929
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001342 RID: 4930
	public new class Instance : GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.GameInstance
	{
		// Token: 0x06008662 RID: 34402 RVA: 0x003290F4 File Offset: 0x003272F4
		public Instance(IStateMachineTarget master, IncubatorMonitor.Def def) : base(master, def)
		{
		}
	}
}
