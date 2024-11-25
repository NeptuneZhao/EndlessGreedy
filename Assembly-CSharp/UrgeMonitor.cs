using System;
using Klei.AI;

// Token: 0x020009AD RID: 2477
public class UrgeMonitor : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance>
{
	// Token: 0x06004808 RID: 18440 RVA: 0x0019C994 File Offset: 0x0019AB94
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.hasurge, (UrgeMonitor.Instance smi) => smi.HasUrge(), UpdateRate.SIM_200ms);
		this.hasurge.Transition(this.satisfied, (UrgeMonitor.Instance smi) => !smi.HasUrge(), UpdateRate.SIM_200ms).ToggleUrge((UrgeMonitor.Instance smi) => smi.GetUrge());
	}

	// Token: 0x04002F27 RID: 12071
	public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002F28 RID: 12072
	public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State hasurge;

	// Token: 0x020019A5 RID: 6565
	public new class Instance : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009D89 RID: 40329 RVA: 0x00375720 File Offset: 0x00373920
		public Instance(IStateMachineTarget master, Urge urge, Amount amount, ScheduleBlockType schedule_block, float in_schedule_threshold, float out_of_schedule_threshold, bool is_threshold_minimum) : base(master)
		{
			this.urge = urge;
			this.scheduleBlock = schedule_block;
			this.schedulable = base.GetComponent<Schedulable>();
			this.amountInstance = base.gameObject.GetAmounts().Get(amount);
			this.isThresholdMinimum = is_threshold_minimum;
			this.inScheduleThreshold = in_schedule_threshold;
			this.outOfScheduleThreshold = out_of_schedule_threshold;
		}

		// Token: 0x06009D8A RID: 40330 RVA: 0x0037577E File Offset: 0x0037397E
		private float GetThreshold()
		{
			if (this.schedulable.IsAllowed(this.scheduleBlock))
			{
				return this.inScheduleThreshold;
			}
			return this.outOfScheduleThreshold;
		}

		// Token: 0x06009D8B RID: 40331 RVA: 0x003757A0 File Offset: 0x003739A0
		public Urge GetUrge()
		{
			return this.urge;
		}

		// Token: 0x06009D8C RID: 40332 RVA: 0x003757A8 File Offset: 0x003739A8
		public bool HasUrge()
		{
			if (this.isThresholdMinimum)
			{
				return this.amountInstance.value >= this.GetThreshold();
			}
			return this.amountInstance.value <= this.GetThreshold();
		}

		// Token: 0x04007A2E RID: 31278
		private AmountInstance amountInstance;

		// Token: 0x04007A2F RID: 31279
		private Urge urge;

		// Token: 0x04007A30 RID: 31280
		private ScheduleBlockType scheduleBlock;

		// Token: 0x04007A31 RID: 31281
		private Schedulable schedulable;

		// Token: 0x04007A32 RID: 31282
		private float inScheduleThreshold;

		// Token: 0x04007A33 RID: 31283
		private float outOfScheduleThreshold;

		// Token: 0x04007A34 RID: 31284
		private bool isThresholdMinimum;
	}
}
