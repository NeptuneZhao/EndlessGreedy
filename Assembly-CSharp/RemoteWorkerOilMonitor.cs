using System;

// Token: 0x02000A20 RID: 2592
public class RemoteWorkerOilMonitor : StateMachineComponent<RemoteWorkerOilMonitor.StatesInstance>
{
	// Token: 0x06004B2A RID: 19242 RVA: 0x001AD4C8 File Offset: 0x001AB6C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x06004B2B RID: 19243 RVA: 0x001AD4DB File Offset: 0x001AB6DB
	public float Oil
	{
		get
		{
			return this.storage.GetMassAvailable(GameTags.LubricatingOil);
		}
	}

	// Token: 0x06004B2C RID: 19244 RVA: 0x001AD4ED File Offset: 0x001AB6ED
	public float OilLevel()
	{
		return this.Oil / 60f;
	}

	// Token: 0x04003138 RID: 12600
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003139 RID: 12601
	public const float CAPACITY_KG = 60f;

	// Token: 0x0400313A RID: 12602
	public const float LOW_LEVEL = 12f;

	// Token: 0x0400313B RID: 12603
	public const float FILL_RATE_KG_PER_S = 1f;

	// Token: 0x0400313C RID: 12604
	public const float CONSUMPTION_RATE_KG_PER_S = 0.1f;

	// Token: 0x02001A38 RID: 6712
	public class StatesInstance : GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.GameInstance
	{
		// Token: 0x06009F63 RID: 40803 RVA: 0x0037C357 File Offset: 0x0037A557
		public StatesInstance(RemoteWorkerOilMonitor master) : base(master)
		{
		}
	}

	// Token: 0x02001A39 RID: 6713
	public class States : GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor>
	{
		// Token: 0x06009F64 RID: 40804 RVA: 0x0037C360 File Offset: 0x0037A560
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = this.ok;
			this.ok.Transition(this.out_of_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOutOfOil), UpdateRate.SIM_200ms).Transition(this.low_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsLowOnOil), UpdateRate.SIM_200ms);
			this.low_oil.Transition(this.out_of_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOutOfOil), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOkForOil), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerLowOil, null);
			this.out_of_oil.Transition(this.low_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsLowOnOil), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOkForOil), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerOutOfOil, null);
		}

		// Token: 0x06009F65 RID: 40805 RVA: 0x0037C44B File Offset: 0x0037A64B
		public static bool IsOkForOil(RemoteWorkerOilMonitor.StatesInstance smi)
		{
			return smi.master.Oil > 12f;
		}

		// Token: 0x06009F66 RID: 40806 RVA: 0x0037C45F File Offset: 0x0037A65F
		public static bool IsLowOnOil(RemoteWorkerOilMonitor.StatesInstance smi)
		{
			return smi.master.Oil >= float.Epsilon && smi.master.Oil < 12f;
		}

		// Token: 0x06009F67 RID: 40807 RVA: 0x0037C487 File Offset: 0x0037A687
		public static bool IsOutOfOil(RemoteWorkerOilMonitor.StatesInstance smi)
		{
			return smi.master.Oil < float.Epsilon;
		}

		// Token: 0x04007BBF RID: 31679
		private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State ok;

		// Token: 0x04007BC0 RID: 31680
		private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State low_oil;

		// Token: 0x04007BC1 RID: 31681
		private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State out_of_oil;
	}
}
