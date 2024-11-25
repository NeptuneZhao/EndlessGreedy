using System;

// Token: 0x02000A1B RID: 2587
public class RemoteWorkTerminalSM : StateMachineComponent<RemoteWorkTerminalSM.StatesInstance>
{
	// Token: 0x06004B1A RID: 19226 RVA: 0x001AD40B File Offset: 0x001AB60B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04003136 RID: 12598
	[MyCmpGet]
	private RemoteWorkTerminal terminal;

	// Token: 0x04003137 RID: 12599
	[MyCmpGet]
	private Operational operational;

	// Token: 0x02001A36 RID: 6710
	public class StatesInstance : GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.GameInstance
	{
		// Token: 0x06009F5D RID: 40797 RVA: 0x0037C220 File Offset: 0x0037A420
		public StatesInstance(RemoteWorkTerminalSM master) : base(master)
		{
		}
	}

	// Token: 0x02001A37 RID: 6711
	public class States : GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM>
	{
		// Token: 0x06009F5E RID: 40798 RVA: 0x0037C22C File Offset: 0x0037A42C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.offline;
			this.offline.Transition(this.online, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.And(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.IsOperational)), UpdateRate.SIM_200ms).Transition(this.offline.no_dock, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Not(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock)), UpdateRate.SIM_200ms);
			this.offline.no_dock.Transition(this.offline, new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().BuildingStatusItems.RemoteWorkTerminalNoDock, null);
			this.online.ToggleRecurringChore(new Func<RemoteWorkTerminalSM.StatesInstance, Chore>(RemoteWorkTerminalSM.States.CreateChore), null).Transition(this.offline, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Not(GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.And(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.IsOperational))), UpdateRate.SIM_200ms);
		}

		// Token: 0x06009F5F RID: 40799 RVA: 0x0037C313 File Offset: 0x0037A513
		public static bool IsOperational(RemoteWorkTerminalSM.StatesInstance smi)
		{
			return smi.master.operational.IsOperational;
		}

		// Token: 0x06009F60 RID: 40800 RVA: 0x0037C325 File Offset: 0x0037A525
		public static bool HasAssignedDock(RemoteWorkTerminalSM.StatesInstance smi)
		{
			return smi.master.terminal.CurrentDock != null;
		}

		// Token: 0x06009F61 RID: 40801 RVA: 0x0037C33D File Offset: 0x0037A53D
		public static Chore CreateChore(RemoteWorkTerminalSM.StatesInstance smi)
		{
			return new RemoteChore(smi.master.terminal);
		}

		// Token: 0x04007BBD RID: 31677
		public GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State online;

		// Token: 0x04007BBE RID: 31678
		public RemoteWorkTerminalSM.States.OfflineStates offline;

		// Token: 0x020025F2 RID: 9714
		public class OfflineStates : GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State
		{
			// Token: 0x0400A8F0 RID: 43248
			public GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State no_dock;
		}
	}
}
