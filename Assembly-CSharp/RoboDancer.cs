using System;
using TUNING;

// Token: 0x0200047D RID: 1149
public class RoboDancer : GameStateMachine<RoboDancer, RoboDancer.Instance>
{
	// Token: 0x060018D1 RID: 6353 RVA: 0x00084404 File Offset: 0x00082604
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<float>(this.timeSpentDancing, this.overjoyed.exitEarly, (RoboDancer.Instance smi, float p) => p >= TRAITS.JOY_REACTIONS.ROBO_DANCER.DANCE_DURATION).Exit(delegate(RoboDancer.Instance smi)
		{
			this.timeSpentDancing.Set(0f, smi, false);
		});
		this.overjoyed.idle.Enter(delegate(RoboDancer.Instance smi)
		{
			if (smi.IsRecTime())
			{
				smi.GoTo(this.overjoyed.dancing);
			}
		}).ToggleStatusItem(Db.Get().DuplicantStatusItems.RoboDancerPlanning, null).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.dancing, (RoboDancer.Instance smi) => smi.IsRecTime());
		this.overjoyed.dancing.ToggleStatusItem(Db.Get().DuplicantStatusItems.RoboDancerDancing, null).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.idle, (RoboDancer.Instance smi) => !smi.IsRecTime()).ToggleChore((RoboDancer.Instance smi) => new RoboDancerChore(smi.master), this.overjoyed.idle);
		this.overjoyed.exitEarly.Enter(delegate(RoboDancer.Instance smi)
		{
			smi.ExitJoyReactionEarly();
		});
	}

	// Token: 0x04000DC8 RID: 3528
	public StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.FloatParameter timeSpentDancing;

	// Token: 0x04000DC9 RID: 3529
	public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000DCA RID: 3530
	public RoboDancer.OverjoyedStates overjoyed;

	// Token: 0x0200124E RID: 4686
	public class OverjoyedStates : GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040062EC RID: 25324
		public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040062ED RID: 25325
		public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State dancing;

		// Token: 0x040062EE RID: 25326
		public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State exitEarly;
	}

	// Token: 0x0200124F RID: 4687
	public new class Instance : GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060082C5 RID: 33477 RVA: 0x0031D665 File Offset: 0x0031B865
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060082C6 RID: 33478 RVA: 0x0031D66E File Offset: 0x0031B86E
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x060082C7 RID: 33479 RVA: 0x0031D690 File Offset: 0x0031B890
		public void ExitJoyReactionEarly()
		{
			JoyBehaviourMonitor.Instance smi = base.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			smi.sm.exitEarly.Trigger(smi);
		}
	}
}
