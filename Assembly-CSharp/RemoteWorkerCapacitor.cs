using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A22 RID: 2594
public class RemoteWorkerCapacitor : StateMachineComponent<RemoteWorkerCapacitor.StatesInstance>
{
	// Token: 0x06004B32 RID: 19250 RVA: 0x001AD53E File Offset: 0x001AB73E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004B33 RID: 19251 RVA: 0x001AD554 File Offset: 0x001AB754
	public float ApplyDeltaEnergy(float delta)
	{
		float num = this.charge;
		this.charge = Mathf.Clamp(this.charge + delta, 0f, 60f);
		return this.charge - num;
	}

	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x06004B34 RID: 19252 RVA: 0x001AD58D File Offset: 0x001AB78D
	public float ChargeRatio
	{
		get
		{
			return this.charge / 60f;
		}
	}

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x06004B35 RID: 19253 RVA: 0x001AD59B File Offset: 0x001AB79B
	public float Charge
	{
		get
		{
			return this.charge;
		}
	}

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x06004B36 RID: 19254 RVA: 0x001AD5A3 File Offset: 0x001AB7A3
	public bool IsLowPower
	{
		get
		{
			return this.charge < 15f;
		}
	}

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x06004B37 RID: 19255 RVA: 0x001AD5B2 File Offset: 0x001AB7B2
	public bool IsOutOfPower
	{
		get
		{
			return this.charge < float.Epsilon;
		}
	}

	// Token: 0x04003141 RID: 12609
	[Serialize]
	private float charge;

	// Token: 0x04003142 RID: 12610
	public const float LOW_LEVEL = 15f;

	// Token: 0x04003143 RID: 12611
	public const float POWER_USE_RATE_J_PER_S = -0.1f;

	// Token: 0x04003144 RID: 12612
	public const float POWER_CHARGE_RATE_J_PER_S = 4f;

	// Token: 0x04003145 RID: 12613
	public const float CAPACITY = 60f;

	// Token: 0x02001A3C RID: 6716
	public class StatesInstance : GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.GameInstance
	{
		// Token: 0x06009F6F RID: 40815 RVA: 0x0037C5F2 File Offset: 0x0037A7F2
		public StatesInstance(RemoteWorkerCapacitor master) : base(master)
		{
		}
	}

	// Token: 0x02001A3D RID: 6717
	public class States : GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor>
	{
		// Token: 0x06009F70 RID: 40816 RVA: 0x0037C5FC File Offset: 0x0037A7FC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = this.ok;
			this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerCapacitorStatus, (RemoteWorkerCapacitor.StatesInstance smi) => smi.master);
			this.ok.Transition(this.out_of_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOutOfPower), UpdateRate.SIM_200ms).Transition(this.low_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsLowPower), UpdateRate.SIM_200ms);
			this.low_power.Transition(this.out_of_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOutOfPower), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOkForPower), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerLowPower, null);
			this.out_of_power.Transition(this.low_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsLowPower), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOkForPower), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerOutOfPower, null);
		}

		// Token: 0x06009F71 RID: 40817 RVA: 0x0037C721 File Offset: 0x0037A921
		public static bool IsOkForPower(RemoteWorkerCapacitor.StatesInstance smi)
		{
			return !smi.master.IsLowPower;
		}

		// Token: 0x06009F72 RID: 40818 RVA: 0x0037C731 File Offset: 0x0037A931
		public static bool IsLowPower(RemoteWorkerCapacitor.StatesInstance smi)
		{
			return smi.master.IsLowPower && !smi.master.IsOutOfPower;
		}

		// Token: 0x06009F73 RID: 40819 RVA: 0x0037C750 File Offset: 0x0037A950
		public static bool IsOutOfPower(RemoteWorkerCapacitor.StatesInstance smi)
		{
			return smi.master.IsOutOfPower;
		}

		// Token: 0x04007BC5 RID: 31685
		private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State ok;

		// Token: 0x04007BC6 RID: 31686
		private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State low_power;

		// Token: 0x04007BC7 RID: 31687
		private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State out_of_power;
	}
}
