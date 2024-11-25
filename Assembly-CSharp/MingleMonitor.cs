using System;

// Token: 0x0200098E RID: 2446
public class MingleMonitor : GameStateMachine<MingleMonitor, MingleMonitor.Instance>
{
	// Token: 0x06004757 RID: 18263 RVA: 0x001985ED File Offset: 0x001967ED
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.mingle;
		base.serializable = StateMachine.SerializeType.Never;
		this.mingle.ToggleRecurringChore(new Func<MingleMonitor.Instance, Chore>(this.CreateMingleChore), null);
	}

	// Token: 0x06004758 RID: 18264 RVA: 0x00198617 File Offset: 0x00196817
	private Chore CreateMingleChore(MingleMonitor.Instance smi)
	{
		return new MingleChore(smi.master);
	}

	// Token: 0x04002E98 RID: 11928
	public GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.State mingle;

	// Token: 0x02001956 RID: 6486
	public new class Instance : GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009C35 RID: 39989 RVA: 0x00371E59 File Offset: 0x00370059
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
