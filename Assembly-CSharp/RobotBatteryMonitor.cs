using System;
using Klei.AI;

// Token: 0x02000A61 RID: 2657
public class RobotBatteryMonitor : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>
{
	// Token: 0x06004D2D RID: 19757 RVA: 0x001BA208 File Offset: 0x001B8408
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.drainingStates;
		this.drainingStates.DefaultState(this.drainingStates.highBattery).Transition(this.deadBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.BatteryDead), UpdateRate.SIM_200ms).Transition(this.needsRechargeStates, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.NeedsRecharge), UpdateRate.SIM_200ms);
		this.drainingStates.highBattery.Transition(this.drainingStates.lowBattery, GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Not(new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent)), UpdateRate.SIM_200ms);
		this.drainingStates.lowBattery.Transition(this.drainingStates.highBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent), UpdateRate.SIM_200ms).ToggleStatusItem(delegate(RobotBatteryMonitor.Instance smi)
		{
			if (!smi.def.canCharge)
			{
				return Db.Get().RobotStatusItems.LowBatteryNoCharge;
			}
			return Db.Get().RobotStatusItems.LowBattery;
		}, (RobotBatteryMonitor.Instance smi) => smi.gameObject);
		this.needsRechargeStates.DefaultState(this.needsRechargeStates.lowBattery).Transition(this.deadBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.BatteryDead), UpdateRate.SIM_200ms).Transition(this.drainingStates, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeComplete), UpdateRate.SIM_200ms).ToggleBehaviour(GameTags.Robots.Behaviours.RechargeBehaviour, (RobotBatteryMonitor.Instance smi) => smi.def.canCharge, null);
		this.needsRechargeStates.lowBattery.ToggleStatusItem(delegate(RobotBatteryMonitor.Instance smi)
		{
			if (!smi.def.canCharge)
			{
				return Db.Get().RobotStatusItems.LowBatteryNoCharge;
			}
			return Db.Get().RobotStatusItems.LowBattery;
		}, (RobotBatteryMonitor.Instance smi) => smi.gameObject).Transition(this.needsRechargeStates.mediumBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent), UpdateRate.SIM_200ms);
		this.needsRechargeStates.mediumBattery.Transition(this.needsRechargeStates.lowBattery, GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Not(new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent)), UpdateRate.SIM_200ms).Transition(this.needsRechargeStates.trickleCharge, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeFull), UpdateRate.SIM_200ms);
		this.needsRechargeStates.trickleCharge.Transition(this.needsRechargeStates.mediumBattery, GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Not(new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeFull)), UpdateRate.SIM_200ms);
		this.deadBattery.ToggleStatusItem(Db.Get().RobotStatusItems.DeadBattery, (RobotBatteryMonitor.Instance smi) => smi.gameObject).Enter(delegate(RobotBatteryMonitor.Instance smi)
		{
			if (smi.GetSMI<DeathMonitor.Instance>() != null)
			{
				smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.DeadBattery);
			}
		});
	}

	// Token: 0x06004D2E RID: 19758 RVA: 0x001BA4B2 File Offset: 0x001B86B2
	public static bool NeedsRecharge(RobotBatteryMonitor.Instance smi)
	{
		return smi.amountInstance.value <= 0f || GameClock.Instance.IsNighttime();
	}

	// Token: 0x06004D2F RID: 19759 RVA: 0x001BA4D2 File Offset: 0x001B86D2
	public static bool ChargeDecent(RobotBatteryMonitor.Instance smi)
	{
		return smi.amountInstance.value >= smi.amountInstance.GetMax() * smi.def.lowBatteryWarningPercent;
	}

	// Token: 0x06004D30 RID: 19760 RVA: 0x001BA4FB File Offset: 0x001B86FB
	public static bool ChargeFull(RobotBatteryMonitor.Instance smi)
	{
		return smi.amountInstance.value >= smi.amountInstance.GetMax();
	}

	// Token: 0x06004D31 RID: 19761 RVA: 0x001BA518 File Offset: 0x001B8718
	public static bool ChargeComplete(RobotBatteryMonitor.Instance smi)
	{
		return smi.amountInstance.value >= smi.amountInstance.GetMax() && !GameClock.Instance.IsNighttime();
	}

	// Token: 0x06004D32 RID: 19762 RVA: 0x001BA541 File Offset: 0x001B8741
	public static bool BatteryDead(RobotBatteryMonitor.Instance smi)
	{
		return !smi.def.canCharge && smi.amountInstance.value == 0f;
	}

	// Token: 0x04003342 RID: 13122
	public StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.ObjectParameter<Storage> internalStorage = new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.ObjectParameter<Storage>();

	// Token: 0x04003343 RID: 13123
	public RobotBatteryMonitor.NeedsRechargeStates needsRechargeStates;

	// Token: 0x04003344 RID: 13124
	public RobotBatteryMonitor.DrainingStates drainingStates;

	// Token: 0x04003345 RID: 13125
	public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State deadBattery;

	// Token: 0x02001A66 RID: 6758
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007C5F RID: 31839
		public string batteryAmountId;

		// Token: 0x04007C60 RID: 31840
		public float lowBatteryWarningPercent;

		// Token: 0x04007C61 RID: 31841
		public bool canCharge;
	}

	// Token: 0x02001A67 RID: 6759
	public class DrainingStates : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State
	{
		// Token: 0x04007C62 RID: 31842
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State highBattery;

		// Token: 0x04007C63 RID: 31843
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State lowBattery;
	}

	// Token: 0x02001A68 RID: 6760
	public class NeedsRechargeStates : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State
	{
		// Token: 0x04007C64 RID: 31844
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State lowBattery;

		// Token: 0x04007C65 RID: 31845
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State mediumBattery;

		// Token: 0x04007C66 RID: 31846
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State trickleCharge;
	}

	// Token: 0x02001A69 RID: 6761
	public new class Instance : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.GameInstance
	{
		// Token: 0x06009FE0 RID: 40928 RVA: 0x0037E154 File Offset: 0x0037C354
		public Instance(IStateMachineTarget master, RobotBatteryMonitor.Def def) : base(master, def)
		{
			this.amountInstance = Db.Get().Amounts.Get(def.batteryAmountId).Lookup(base.gameObject);
			this.amountInstance.SetValue(this.amountInstance.GetMax());
		}

		// Token: 0x04007C67 RID: 31847
		public AmountInstance amountInstance;
	}
}
