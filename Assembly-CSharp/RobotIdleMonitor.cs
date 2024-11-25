using System;

// Token: 0x0200081D RID: 2077
public class RobotIdleMonitor : GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance>
{
	// Token: 0x06003977 RID: 14711 RVA: 0x00139594 File Offset: 0x00137794
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Transition(this.working, (RobotIdleMonitor.Instance smi) => !RobotIdleMonitor.CheckShouldIdle(smi), UpdateRate.SIM_200ms);
		this.working.Transition(this.idle, (RobotIdleMonitor.Instance smi) => RobotIdleMonitor.CheckShouldIdle(smi), UpdateRate.SIM_200ms);
	}

	// Token: 0x06003978 RID: 14712 RVA: 0x00139618 File Offset: 0x00137818
	private static bool CheckShouldIdle(RobotIdleMonitor.Instance smi)
	{
		FallMonitor.Instance smi2 = smi.master.gameObject.GetSMI<FallMonitor.Instance>();
		return smi2 == null || (!smi.master.gameObject.GetComponent<ChoreConsumer>().choreDriver.HasChore() && smi2.GetCurrentState() == smi2.sm.standing);
	}

	// Token: 0x04002295 RID: 8853
	public GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04002296 RID: 8854
	public GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.State working;

	// Token: 0x0200172D RID: 5933
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200172E RID: 5934
	public new class Instance : GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060094E4 RID: 38116 RVA: 0x0035E47E File Offset: 0x0035C67E
		public Instance(IStateMachineTarget master, RobotIdleMonitor.Def def) : base(master)
		{
		}

		// Token: 0x040071FD RID: 29181
		public KBatchedAnimController eyes;
	}
}
