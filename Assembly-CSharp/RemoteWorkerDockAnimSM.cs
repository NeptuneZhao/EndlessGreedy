using System;

// Token: 0x02000A2C RID: 2604
internal class RemoteWorkerDockAnimSM : StateMachineComponent<RemoteWorkerDockAnimSM.StatesInstance>
{
	// Token: 0x06004B7D RID: 19325 RVA: 0x001AE34F File Offset: 0x001AC54F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04003174 RID: 12660
	[MyCmpAdd]
	private RemoteWorkerDock dock;

	// Token: 0x04003175 RID: 12661
	[MyCmpGet]
	private Operational operational;

	// Token: 0x02001A41 RID: 6721
	public class StatesInstance : GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.GameInstance
	{
		// Token: 0x06009F8A RID: 40842 RVA: 0x0037D2C0 File Offset: 0x0037B4C0
		public StatesInstance(RemoteWorkerDockAnimSM master) : base(master)
		{
		}
	}

	// Token: 0x02001A42 RID: 6722
	public class States : GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM>
	{
		// Token: 0x06009F8B RID: 40843 RVA: 0x0037D2CC File Offset: 0x0037B4CC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.EnterTransition(this.off.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)).EnterTransition(this.off.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored))).Transition(this.on, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.IsOnline), UpdateRate.SIM_200ms);
			this.off.full.QueueAnim("off_full", false, null).Transition(this.off.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)), UpdateRate.SIM_200ms);
			this.off.empty.QueueAnim("off_empty", false, null).Transition(this.off.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored), UpdateRate.SIM_200ms);
			this.on.EnterTransition(this.on.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)).EnterTransition(this.on.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored))).Transition(this.off, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.IsOnline)), UpdateRate.SIM_200ms);
			this.on.full.QueueAnim("on_full", false, null).Transition(this.off.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)), UpdateRate.SIM_200ms);
			this.on.empty.QueueAnim("on_empty", false, null).Transition(this.on.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored), UpdateRate.SIM_200ms);
		}

		// Token: 0x06009F8C RID: 40844 RVA: 0x0037D47C File Offset: 0x0037B67C
		public static bool IsOnline(RemoteWorkerDockAnimSM.StatesInstance smi)
		{
			return smi.master.operational.IsOperational && smi.master.dock.RemoteWorker != null;
		}

		// Token: 0x06009F8D RID: 40845 RVA: 0x0037D4A8 File Offset: 0x0037B6A8
		public static bool HasWorkerStored(RemoteWorkerDockAnimSM.StatesInstance smi)
		{
			return smi.master.dock.RemoteWorker != null && smi.master.dock.RemoteWorker.Docked;
		}

		// Token: 0x04007BD0 RID: 31696
		public RemoteWorkerDockAnimSM.States.FullOrEmptyState on;

		// Token: 0x04007BD1 RID: 31697
		public RemoteWorkerDockAnimSM.States.FullOrEmptyState off;

		// Token: 0x020025F8 RID: 9720
		public class FullOrEmptyState : GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State
		{
			// Token: 0x0400A90A RID: 43274
			public GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State full;

			// Token: 0x0400A90B RID: 43275
			public GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State empty;
		}
	}
}
