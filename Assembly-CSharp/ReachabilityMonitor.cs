using System;

// Token: 0x020005A8 RID: 1448
public class ReachabilityMonitor : GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable>
{
	// Token: 0x06002270 RID: 8816 RVA: 0x000BFBDC File Offset: 0x000BDDDC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.unreachable;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.FastUpdate("UpdateReachability", ReachabilityMonitor.updateReachabilityCB, UpdateRate.SIM_1000ms, true);
		this.reachable.ToggleTag(GameTags.Reachable).Enter("TriggerEvent", delegate(ReachabilityMonitor.Instance smi)
		{
			smi.TriggerEvent();
		}).ParamTransition<bool>(this.isReachable, this.unreachable, GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.IsFalse);
		this.unreachable.Enter("TriggerEvent", delegate(ReachabilityMonitor.Instance smi)
		{
			smi.TriggerEvent();
		}).ParamTransition<bool>(this.isReachable, this.reachable, GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.IsTrue);
	}

	// Token: 0x0400136D RID: 4973
	public GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State reachable;

	// Token: 0x0400136E RID: 4974
	public GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State unreachable;

	// Token: 0x0400136F RID: 4975
	public StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.BoolParameter isReachable = new StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.BoolParameter(false);

	// Token: 0x04001370 RID: 4976
	private static ReachabilityMonitor.UpdateReachabilityCB updateReachabilityCB = new ReachabilityMonitor.UpdateReachabilityCB();

	// Token: 0x02001396 RID: 5014
	private class UpdateReachabilityCB : UpdateBucketWithUpdater<ReachabilityMonitor.Instance>.IUpdater
	{
		// Token: 0x0600879C RID: 34716 RVA: 0x0032C04F File Offset: 0x0032A24F
		public void Update(ReachabilityMonitor.Instance smi, float dt)
		{
			smi.UpdateReachability();
		}
	}

	// Token: 0x02001397 RID: 5015
	public new class Instance : GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.GameInstance
	{
		// Token: 0x0600879E RID: 34718 RVA: 0x0032C05F File Offset: 0x0032A25F
		public Instance(Workable workable) : base(workable)
		{
			this.UpdateReachability();
		}

		// Token: 0x0600879F RID: 34719 RVA: 0x0032C070 File Offset: 0x0032A270
		public void TriggerEvent()
		{
			bool flag = base.sm.isReachable.Get(base.smi);
			base.Trigger(-1432940121, flag);
		}

		// Token: 0x060087A0 RID: 34720 RVA: 0x0032C0A8 File Offset: 0x0032A2A8
		public void UpdateReachability()
		{
			if (base.master == null)
			{
				return;
			}
			int cell = base.master.GetCell();
			base.sm.isReachable.Set(MinionGroupProber.Get().IsAllReachable(cell, base.master.GetOffsets(cell)), base.smi, false);
		}
	}
}
