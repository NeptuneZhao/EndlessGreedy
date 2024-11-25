using System;

// Token: 0x02000A21 RID: 2593
public class RemoteWorkerGunkMonitor : StateMachineComponent<RemoteWorkerGunkMonitor.StatesInstance>
{
	// Token: 0x06004B2E RID: 19246 RVA: 0x001AD503 File Offset: 0x001AB703
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x06004B2F RID: 19247 RVA: 0x001AD516 File Offset: 0x001AB716
	public float Gunk
	{
		get
		{
			return this.storage.GetMassAvailable(SimHashes.LiquidGunk);
		}
	}

	// Token: 0x06004B30 RID: 19248 RVA: 0x001AD528 File Offset: 0x001AB728
	public float GunkLevel()
	{
		return this.Gunk / 600f;
	}

	// Token: 0x0400313D RID: 12605
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400313E RID: 12606
	public const float CAPACITY_KG = 600f;

	// Token: 0x0400313F RID: 12607
	public const float HIGH_LEVEL = 480f;

	// Token: 0x04003140 RID: 12608
	public const float DRAIN_AMOUNT_KG_PER_S = 1f;

	// Token: 0x02001A3A RID: 6714
	public class StatesInstance : GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.GameInstance
	{
		// Token: 0x06009F69 RID: 40809 RVA: 0x0037C4A3 File Offset: 0x0037A6A3
		public StatesInstance(RemoteWorkerGunkMonitor master) : base(master)
		{
		}
	}

	// Token: 0x02001A3B RID: 6715
	public class States : GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor>
	{
		// Token: 0x06009F6A RID: 40810 RVA: 0x0037C4AC File Offset: 0x0037A6AC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = this.ok;
			this.ok.Transition(this.full_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsFullOfGunk), UpdateRate.SIM_200ms).Transition(this.high_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkHigh), UpdateRate.SIM_200ms);
			this.high_gunk.Transition(this.full_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsFullOfGunk), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkLevelOk), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerHighGunkLevel, null);
			this.full_gunk.Transition(this.high_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkHigh), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkLevelOk), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerFullGunkLevel, null);
		}

		// Token: 0x06009F6B RID: 40811 RVA: 0x0037C597 File Offset: 0x0037A797
		public static bool IsGunkLevelOk(RemoteWorkerGunkMonitor.StatesInstance smi)
		{
			return smi.master.Gunk < 480f;
		}

		// Token: 0x06009F6C RID: 40812 RVA: 0x0037C5AB File Offset: 0x0037A7AB
		public static bool IsGunkHigh(RemoteWorkerGunkMonitor.StatesInstance smi)
		{
			return smi.master.Gunk >= 480f && smi.master.Gunk < 600f;
		}

		// Token: 0x06009F6D RID: 40813 RVA: 0x0037C5D3 File Offset: 0x0037A7D3
		public static bool IsFullOfGunk(RemoteWorkerGunkMonitor.StatesInstance smi)
		{
			return smi.master.Gunk >= 600f;
		}

		// Token: 0x04007BC2 RID: 31682
		private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State ok;

		// Token: 0x04007BC3 RID: 31683
		private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State high_gunk;

		// Token: 0x04007BC4 RID: 31684
		private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State full_gunk;
	}
}
