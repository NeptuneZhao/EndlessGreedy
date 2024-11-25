using System;

// Token: 0x02000566 RID: 1382
public class FallWhenDeadMonitor : GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance>
{
	// Token: 0x06002008 RID: 8200 RVA: 0x000B4758 File Offset: 0x000B2958
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.standing;
		this.standing.Transition(this.entombed, (FallWhenDeadMonitor.Instance smi) => smi.IsEntombed(), UpdateRate.SIM_200ms).Transition(this.falling, (FallWhenDeadMonitor.Instance smi) => smi.IsFalling(), UpdateRate.SIM_200ms);
		this.falling.ToggleGravity(this.standing);
		this.entombed.Transition(this.standing, (FallWhenDeadMonitor.Instance smi) => !smi.IsEntombed(), UpdateRate.SIM_200ms);
	}

	// Token: 0x04001219 RID: 4633
	public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State standing;

	// Token: 0x0400121A RID: 4634
	public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State falling;

	// Token: 0x0400121B RID: 4635
	public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State entombed;

	// Token: 0x02001366 RID: 4966
	public new class Instance : GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060086FD RID: 34557 RVA: 0x0032A823 File Offset: 0x00328A23
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060086FE RID: 34558 RVA: 0x0032A82C File Offset: 0x00328A2C
		public bool IsEntombed()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			return component != null && component.IsEntombed;
		}

		// Token: 0x060086FF RID: 34559 RVA: 0x0032A854 File Offset: 0x00328A54
		public bool IsFalling()
		{
			int num = Grid.CellBelow(Grid.PosToCell(base.master.transform.GetPosition()));
			return Grid.IsValidCell(num) && !Grid.Solid[num];
		}
	}
}
