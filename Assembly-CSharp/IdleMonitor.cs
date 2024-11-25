using System;

// Token: 0x02000988 RID: 2440
public class IdleMonitor : GameStateMachine<IdleMonitor, IdleMonitor.Instance>
{
	// Token: 0x06004740 RID: 18240 RVA: 0x00197AD8 File Offset: 0x00195CD8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.TagTransition(GameTags.Dying, this.stopped, false).ToggleRecurringChore(new Func<IdleMonitor.Instance, Chore>(this.CreateIdleChore), null);
		this.stopped.DoNothing();
	}

	// Token: 0x06004741 RID: 18241 RVA: 0x00197B18 File Offset: 0x00195D18
	private Chore CreateIdleChore(IdleMonitor.Instance smi)
	{
		return new IdleChore(smi.master);
	}

	// Token: 0x04002E80 RID: 11904
	public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04002E81 RID: 11905
	public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State stopped;

	// Token: 0x02001946 RID: 6470
	public new class Instance : GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009BF3 RID: 39923 RVA: 0x00371576 File Offset: 0x0036F776
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
