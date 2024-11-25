using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A62 RID: 2658
public class RobotElectroBankMonitor : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>
{
	// Token: 0x06004D34 RID: 19764 RVA: 0x001BA578 File Offset: 0x001B8778
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.powered;
		this.root.Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			smi.ElectroBankStorageChange(null);
		});
		this.powered.DefaultState(this.powered.highBattery).ParamTransition<bool>(this.hasElectrobank, this.powerdown.pre, GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.IsFalse).Update(delegate(RobotElectroBankMonitor.Instance smi, float dt)
		{
			RobotElectroBankMonitor.ConsumePower(smi, dt);
		}, UpdateRate.SIM_200ms, false);
		this.powered.highBattery.Transition(this.powered.lowBattery, GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Not(new StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Transition.ConditionCallback(RobotElectroBankMonitor.ChargeDecent)), UpdateRate.SIM_200ms).Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			smi.gameObject.RemoveTag(GameTags.Dead);
		});
		this.powered.lowBattery.Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			RobotElectroBankMonitor.RequestBattery(smi);
		}).Transition(this.powered.highBattery, new StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Transition.ConditionCallback(RobotElectroBankMonitor.ChargeDecent), UpdateRate.SIM_200ms).ToggleStatusItem((RobotElectroBankMonitor.Instance smi) => Db.Get().RobotStatusItems.LowBatteryNoCharge, null).EventHandlerTransition(GameHashes.OnStorageChange, this.powerup.flying, new Func<RobotElectroBankMonitor.Instance, object, bool>(RobotElectroBankMonitor.BatteryDelivered));
		this.powerdown.Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			RobotElectroBankMonitor.RequestBattery(smi);
			smi.Get<Brain>().Stop("dead battery");
		}).ToggleStatusItem(Db.Get().RobotStatusItems.DeadBatteryFlydo, (RobotElectroBankMonitor.Instance smi) => smi.gameObject).Exit(delegate(RobotElectroBankMonitor.Instance smi)
		{
			if (GameComps.Fallers.Has(smi.gameObject))
			{
				GameComps.Fallers.Remove(smi.gameObject);
			}
		});
		this.powerdown.pre.PlayAnim("power_down_pre").OnAnimQueueComplete(this.powerdown.fall);
		this.powerdown.fall.PlayAnim("power_down_loop", KAnim.PlayMode.Loop).Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			if (!GameComps.Fallers.Has(smi.gameObject))
			{
				GameComps.Fallers.Add(smi.gameObject, Vector2.zero);
			}
		}).Update(delegate(RobotElectroBankMonitor.Instance smi, float dt)
		{
			if (!GameComps.Gravities.Has(smi.gameObject))
			{
				smi.GoTo(this.powerdown.landed);
			}
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.Landed, this.powerdown.landed, null);
		this.powerdown.landed.PlayAnim("power_down_pst").OnAnimQueueComplete(this.powerdown.dead);
		this.powerdown.dead.PlayAnim("dead_battery").ParamTransition<bool>(this.hasElectrobank, this.powerup.grounded, GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.IsTrue);
		this.powerup.Exit(delegate(RobotElectroBankMonitor.Instance smi)
		{
			smi.Get<Brain>().Reset("power up");
		});
		this.powerup.flying.PlayAnim("battery_change_fly").OnAnimQueueComplete(this.powered);
		this.powerup.grounded.PlayAnim("battery_change_dead").OnAnimQueueComplete(this.powerup.takeoff);
		this.powerup.takeoff.PlayAnim("power_up").OnAnimQueueComplete(this.powered);
	}

	// Token: 0x06004D35 RID: 19765 RVA: 0x001BA8EC File Offset: 0x001B8AEC
	public static bool ChargeDecent(RobotElectroBankMonitor.Instance smi)
	{
		float num = 0f;
		foreach (GameObject gameObject in smi.electroBankStorage.items)
		{
			num += gameObject.GetComponent<Electrobank>().Charge;
		}
		return num >= smi.def.lowBatteryWarningPercent * 120000f;
	}

	// Token: 0x06004D36 RID: 19766 RVA: 0x001BA968 File Offset: 0x001B8B68
	public static void ConsumePower(RobotElectroBankMonitor.Instance smi, float dt)
	{
		if (smi.electrobank == null)
		{
			RobotElectroBankMonitor.RequestBattery(smi);
			return;
		}
		float joules = Mathf.Min(dt * smi.def.wattage, smi.electrobank.Charge);
		smi.electrobank.RemovePower(joules, true);
		smi.bankAmount.value = smi.electrobank.Charge;
	}

	// Token: 0x06004D37 RID: 19767 RVA: 0x001BA9CC File Offset: 0x001B8BCC
	public static void RequestBattery(RobotElectroBankMonitor.Instance smi)
	{
		if (smi.fetchBatteryChore.IsPaused)
		{
			smi.fetchBatteryChore.Pause(smi.electrobank != null && RobotElectroBankMonitor.ChargeDecent(smi), "FlydoBattery");
		}
	}

	// Token: 0x06004D38 RID: 19768 RVA: 0x001BAA02 File Offset: 0x001B8C02
	public static bool BatteryDelivered(RobotElectroBankMonitor.Instance smi, object data)
	{
		return data as GameObject != null;
	}

	// Token: 0x04003346 RID: 13126
	public RobotElectroBankMonitor.PoweredState powered;

	// Token: 0x04003347 RID: 13127
	public RobotElectroBankMonitor.PowerDown powerdown;

	// Token: 0x04003348 RID: 13128
	public RobotElectroBankMonitor.PowerUp powerup;

	// Token: 0x04003349 RID: 13129
	public StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.BoolParameter hasElectrobank;

	// Token: 0x02001A6B RID: 6763
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007C70 RID: 31856
		public float lowBatteryWarningPercent;

		// Token: 0x04007C71 RID: 31857
		public float wattage;
	}

	// Token: 0x02001A6C RID: 6764
	public class PoweredState : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State
	{
		// Token: 0x04007C72 RID: 31858
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State highBattery;

		// Token: 0x04007C73 RID: 31859
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State lowBattery;
	}

	// Token: 0x02001A6D RID: 6765
	public class PowerDown : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State
	{
		// Token: 0x04007C74 RID: 31860
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State pre;

		// Token: 0x04007C75 RID: 31861
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State fall;

		// Token: 0x04007C76 RID: 31862
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State landed;

		// Token: 0x04007C77 RID: 31863
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State dead;
	}

	// Token: 0x02001A6E RID: 6766
	public class PowerUp : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State
	{
		// Token: 0x04007C78 RID: 31864
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State flying;

		// Token: 0x04007C79 RID: 31865
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State grounded;

		// Token: 0x04007C7A RID: 31866
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State takeoff;
	}

	// Token: 0x02001A6F RID: 6767
	public new class Instance : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.GameInstance
	{
		// Token: 0x06009FEE RID: 40942 RVA: 0x0037E280 File Offset: 0x0037C480
		public Instance(IStateMachineTarget master, RobotElectroBankMonitor.Def def) : base(master, def)
		{
			this.fetchBatteryChore = base.GetComponent<ManualDeliveryKG>();
			foreach (Storage storage in master.gameObject.GetComponents<Storage>())
			{
				if (storage.storageID == GameTags.ChargedPortableBattery)
				{
					this.electroBankStorage = storage;
					break;
				}
			}
			this.bankAmount = Db.Get().Amounts.InternalElectroBank.Lookup(master.gameObject);
			this.electroBankStorage.Subscribe(-1697596308, new Action<object>(this.ElectroBankStorageChange));
			this.ElectroBankStorageChange(null);
		}

		// Token: 0x06009FEF RID: 40943 RVA: 0x0037E320 File Offset: 0x0037C520
		public void ElectroBankStorageChange(object data = null)
		{
			if (this.electroBankStorage.Count > 0)
			{
				this.electrobank = this.electroBankStorage.items[0].GetComponent<Electrobank>();
				this.bankAmount.value = this.electrobank.Charge;
			}
			else
			{
				this.electrobank = null;
			}
			this.fetchBatteryChore.Pause(this.electrobank != null && RobotElectroBankMonitor.ChargeDecent(this), "Robot has sufficienct electrobank");
			base.sm.hasElectrobank.Set(this.electrobank != null, this, false);
		}

		// Token: 0x04007C7B RID: 31867
		public Storage electroBankStorage;

		// Token: 0x04007C7C RID: 31868
		public Electrobank electrobank;

		// Token: 0x04007C7D RID: 31869
		public ManualDeliveryKG fetchBatteryChore;

		// Token: 0x04007C7E RID: 31870
		public AmountInstance bankAmount;
	}
}
