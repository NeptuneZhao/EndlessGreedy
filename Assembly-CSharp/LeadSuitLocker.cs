using System;
using TUNING;
using UnityEngine;

// Token: 0x020006F9 RID: 1785
public class LeadSuitLocker : StateMachineComponent<LeadSuitLocker.StatesInstance>
{
	// Token: 0x06002D97 RID: 11671 RVA: 0x000FFE20 File Offset: 0x000FE020
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.o2_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_top", "meter_oxygen", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_top"
		});
		this.battery_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_side", "meter_petrol", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_side"
		});
		base.smi.StartSM();
	}

	// Token: 0x06002D98 RID: 11672 RVA: 0x000FFEA0 File Offset: 0x000FE0A0
	public bool IsSuitFullyCharged()
	{
		return this.suit_locker.IsSuitFullyCharged();
	}

	// Token: 0x06002D99 RID: 11673 RVA: 0x000FFEAD File Offset: 0x000FE0AD
	public KPrefabID GetStoredOutfit()
	{
		return this.suit_locker.GetStoredOutfit();
	}

	// Token: 0x06002D9A RID: 11674 RVA: 0x000FFEBC File Offset: 0x000FE0BC
	private void FillBattery(float dt)
	{
		KPrefabID storedOutfit = this.suit_locker.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		LeadSuitTank component = storedOutfit.GetComponent<LeadSuitTank>();
		if (!component.IsFull())
		{
			component.batteryCharge += dt / this.batteryChargeTime;
		}
	}

	// Token: 0x06002D9B RID: 11675 RVA: 0x000FFF04 File Offset: 0x000FE104
	private void RefreshMeter()
	{
		this.o2_meter.SetPositionPercent(this.suit_locker.OxygenAvailable);
		this.battery_meter.SetPositionPercent(this.suit_locker.BatteryAvailable);
		this.anim_controller.SetSymbolVisiblity("oxygen_yes_bloom", this.IsOxygenTankAboveMinimumLevel());
		this.anim_controller.SetSymbolVisiblity("petrol_yes_bloom", this.IsBatteryAboveMinimumLevel());
	}

	// Token: 0x06002D9C RID: 11676 RVA: 0x000FFF74 File Offset: 0x000FE174
	public bool IsOxygenTankAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			SuitTank component = storedOutfit.GetComponent<SuitTank>();
			return component == null || component.PercentFull() >= EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x06002D9D RID: 11677 RVA: 0x000FFFB8 File Offset: 0x000FE1B8
	public bool IsBatteryAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			LeadSuitTank component = storedOutfit.GetComponent<LeadSuitTank>();
			return component == null || component.PercentFull() >= EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x04001A7C RID: 6780
	[MyCmpReq]
	private Building building;

	// Token: 0x04001A7D RID: 6781
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001A7E RID: 6782
	[MyCmpReq]
	private SuitLocker suit_locker;

	// Token: 0x04001A7F RID: 6783
	[MyCmpReq]
	private KBatchedAnimController anim_controller;

	// Token: 0x04001A80 RID: 6784
	private MeterController o2_meter;

	// Token: 0x04001A81 RID: 6785
	private MeterController battery_meter;

	// Token: 0x04001A82 RID: 6786
	private float batteryChargeTime = 60f;

	// Token: 0x02001532 RID: 5426
	public class States : GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker>
	{
		// Token: 0x06008D89 RID: 36233 RVA: 0x0033FD08 File Offset: 0x0033DF08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Update("RefreshMeter", delegate(LeadSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.RefreshMeter();
			}, UpdateRate.RENDER_200ms, false);
			this.empty.EventTransition(GameHashes.OnStorageChange, this.charging, (LeadSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null);
			this.charging.DefaultState(this.charging.notoperational).EventTransition(GameHashes.OnStorageChange, this.empty, (LeadSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).Transition(this.charged, (LeadSuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms);
			this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational, false);
			this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Update("FillBattery", delegate(LeadSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.FillBattery(dt);
			}, UpdateRate.SIM_1000ms, false);
			this.charged.EventTransition(GameHashes.OnStorageChange, this.empty, (LeadSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null);
		}

		// Token: 0x04006C35 RID: 27701
		public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State empty;

		// Token: 0x04006C36 RID: 27702
		public LeadSuitLocker.States.ChargingStates charging;

		// Token: 0x04006C37 RID: 27703
		public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State charged;

		// Token: 0x020024F8 RID: 9464
		public class ChargingStates : GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State
		{
			// Token: 0x0400A472 RID: 42098
			public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State notoperational;

			// Token: 0x0400A473 RID: 42099
			public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State operational;
		}
	}

	// Token: 0x02001533 RID: 5427
	public class StatesInstance : GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.GameInstance
	{
		// Token: 0x06008D8B RID: 36235 RVA: 0x0033FEAA File Offset: 0x0033E0AA
		public StatesInstance(LeadSuitLocker lead_suit_locker) : base(lead_suit_locker)
		{
		}
	}
}
