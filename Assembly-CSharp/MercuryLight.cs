using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000729 RID: 1833
public class MercuryLight : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>
{
	// Token: 0x060030A3 RID: 12451 RVA: 0x0010C3B0 File Offset: 0x0010A5B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.noOperational.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOff)).ParamTransition<float>(this.Charge, this.noOperational.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero).ParamTransition<float>(this.Charge, this.noOperational.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero);
		this.noOperational.depleating.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("depleating", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleating, null).ParamTransition<float>(this.Charge, this.noOperational.depleated, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero).Update(new Action<MercuryLight.Instance, float>(MercuryLight.DepleteUpdate), UpdateRate.SIM_200ms, false);
		this.noOperational.depleated.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("on_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.noOperational.idle);
		this.noOperational.idle.TagTransition(GameTags.Operational, this.noOperational.exit, false).PlayAnim("off", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleated, null);
		this.noOperational.exit.PlayAnim("on_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational);
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.darkness).Update(new Action<MercuryLight.Instance, float>(MercuryLight.ConsumeFuelUpdate), UpdateRate.SIM_200ms, false);
		this.operational.darkness.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOff)).ParamTransition<bool>(this.HasEnoughFuel, this.operational.light, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsTrue).ParamTransition<float>(this.Charge, this.operational.darkness.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero).ParamTransition<float>(this.Charge, this.operational.darkness.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero);
		this.operational.darkness.depleating.PlayAnim("depleating", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleating, null).ParamTransition<float>(this.Charge, this.operational.darkness.depleated, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero).Update(new Action<MercuryLight.Instance, float>(MercuryLight.DepleteUpdate), UpdateRate.SIM_200ms, false);
		this.operational.darkness.depleated.PlayAnim("on_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.darkness.idle);
		this.operational.darkness.idle.PlayAnim("off", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleated, null).ParamTransition<float>(this.Charge, this.operational.darkness.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero);
		this.operational.light.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOn)).PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<bool>(this.HasEnoughFuel, this.operational.darkness, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).DefaultState(this.operational.light.charging);
		this.operational.light.charging.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Charging, null).ParamTransition<float>(this.Charge, this.operational.light.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTEOne).Update(new Action<MercuryLight.Instance, float>(MercuryLight.ChargeUpdate), UpdateRate.SIM_200ms, false);
		this.operational.light.idle.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Charged, null).ParamTransition<float>(this.Charge, this.operational.light.charging, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTOne);
	}

	// Token: 0x060030A4 RID: 12452 RVA: 0x0010C7F0 File Offset: 0x0010A9F0
	public static void SetOperationalActiveFlagOn(MercuryLight.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x060030A5 RID: 12453 RVA: 0x0010C7FF File Offset: 0x0010A9FF
	public static void SetOperationalActiveFlagOff(MercuryLight.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x060030A6 RID: 12454 RVA: 0x0010C80E File Offset: 0x0010AA0E
	public static void DepleteUpdate(MercuryLight.Instance smi, float dt)
	{
		smi.DepleteUpdate(dt);
	}

	// Token: 0x060030A7 RID: 12455 RVA: 0x0010C817 File Offset: 0x0010AA17
	public static void ChargeUpdate(MercuryLight.Instance smi, float dt)
	{
		smi.ChargeUpdate(dt);
	}

	// Token: 0x060030A8 RID: 12456 RVA: 0x0010C820 File Offset: 0x0010AA20
	public static void ConsumeFuelUpdate(MercuryLight.Instance smi, float dt)
	{
		smi.ConsumeFuelUpdate(dt);
	}

	// Token: 0x04001C84 RID: 7300
	private static Tag ELEMENT_TAG = SimHashes.Mercury.CreateTag();

	// Token: 0x04001C85 RID: 7301
	private const string ON_ANIM_NAME = "on";

	// Token: 0x04001C86 RID: 7302
	private const string ON_PRE_ANIM_NAME = "on_pre";

	// Token: 0x04001C87 RID: 7303
	private const string TRANSITION_TO_OFF_ANIM_NAME = "on_pst";

	// Token: 0x04001C88 RID: 7304
	private const string DEPLEATING_ANIM_NAME = "depleating";

	// Token: 0x04001C89 RID: 7305
	private const string OFF_ANIM_NAME = "off";

	// Token: 0x04001C8A RID: 7306
	private const string LIGHT_LEVEL_METER_TARGET_NAME = "meter_target";

	// Token: 0x04001C8B RID: 7307
	private const string LIGHT_LEVEL_METER_ANIM_NAME = "meter";

	// Token: 0x04001C8C RID: 7308
	public MercuryLight.Darknesstates noOperational;

	// Token: 0x04001C8D RID: 7309
	public MercuryLight.OperationalStates operational;

	// Token: 0x04001C8E RID: 7310
	public StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.FloatParameter Charge;

	// Token: 0x04001C8F RID: 7311
	public StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.BoolParameter HasEnoughFuel;

	// Token: 0x02001574 RID: 5492
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06008E96 RID: 36502 RVA: 0x00344780 File Offset: 0x00342980
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			string arg = MercuryLight.ELEMENT_TAG.ProperName();
			List<Descriptor> list = new List<Descriptor>();
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.FUEL_MASS_PER_SECOND, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.FUEL_MASS_PER_SECOND, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false);
			list.Add(item);
			return list;
		}

		// Token: 0x04006CD3 RID: 27859
		public float MAX_LUX;

		// Token: 0x04006CD4 RID: 27860
		public float TURN_ON_DELAY;

		// Token: 0x04006CD5 RID: 27861
		public float FUEL_MASS_PER_SECOND;
	}

	// Token: 0x02001575 RID: 5493
	public class LightStates : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
	{
		// Token: 0x04006CD6 RID: 27862
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State charging;

		// Token: 0x04006CD7 RID: 27863
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State idle;
	}

	// Token: 0x02001576 RID: 5494
	public class Darknesstates : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
	{
		// Token: 0x04006CD8 RID: 27864
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State depleating;

		// Token: 0x04006CD9 RID: 27865
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State depleated;

		// Token: 0x04006CDA RID: 27866
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State idle;

		// Token: 0x04006CDB RID: 27867
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State exit;
	}

	// Token: 0x02001577 RID: 5495
	public class OperationalStates : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
	{
		// Token: 0x04006CDC RID: 27868
		public MercuryLight.LightStates light;

		// Token: 0x04006CDD RID: 27869
		public MercuryLight.Darknesstates darkness;
	}

	// Token: 0x02001578 RID: 5496
	public new class Instance : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.GameInstance
	{
		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06008E9B RID: 36507 RVA: 0x00344813 File Offset: 0x00342A13
		public bool HasEnoughFuel
		{
			get
			{
				return base.sm.HasEnoughFuel.Get(this);
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06008E9C RID: 36508 RVA: 0x00344826 File Offset: 0x00342A26
		public int LuxLevel
		{
			get
			{
				return Mathf.FloorToInt(base.smi.ChargeLevel * base.def.MAX_LUX);
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06008E9D RID: 36509 RVA: 0x00344845 File Offset: 0x00342A45
		public float ChargeLevel
		{
			get
			{
				return base.smi.sm.Charge.Get(this);
			}
		}

		// Token: 0x06008E9E RID: 36510 RVA: 0x00344860 File Offset: 0x00342A60
		public Instance(IStateMachineTarget master, MercuryLight.Def def) : base(master, def)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			this.lightIntensityMeterController = new MeterController(component, "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
		}

		// Token: 0x06008E9F RID: 36511 RVA: 0x0034489A File Offset: 0x00342A9A
		public override void StartSM()
		{
			base.StartSM();
			this.SetChargeLevel(this.ChargeLevel);
		}

		// Token: 0x06008EA0 RID: 36512 RVA: 0x003448B0 File Offset: 0x00342AB0
		public void DepleteUpdate(float dt)
		{
			float chargeLevel = Mathf.Clamp(this.ChargeLevel - dt / base.def.TURN_ON_DELAY, 0f, 1f);
			this.SetChargeLevel(chargeLevel);
		}

		// Token: 0x06008EA1 RID: 36513 RVA: 0x003448E8 File Offset: 0x00342AE8
		public void ChargeUpdate(float dt)
		{
			float chargeLevel = Mathf.Clamp(this.ChargeLevel + dt / base.def.TURN_ON_DELAY, 0f, 1f);
			this.SetChargeLevel(chargeLevel);
		}

		// Token: 0x06008EA2 RID: 36514 RVA: 0x00344920 File Offset: 0x00342B20
		public void SetChargeLevel(float value)
		{
			base.sm.Charge.Set(value, this, false);
			this.light.Lux = this.LuxLevel;
			this.light.FullRefresh();
			bool flag = this.ChargeLevel > 0f;
			if (this.light.enabled != flag)
			{
				this.light.enabled = flag;
			}
			this.lightIntensityMeterController.SetPositionPercent(value);
		}

		// Token: 0x06008EA3 RID: 36515 RVA: 0x00344994 File Offset: 0x00342B94
		public void ConsumeFuelUpdate(float dt)
		{
			float num = base.def.FUEL_MASS_PER_SECOND * dt;
			if (this.storage.MassStored() < num)
			{
				base.sm.HasEnoughFuel.Set(false, this, false);
				return;
			}
			float num2;
			SimUtil.DiseaseInfo diseaseInfo;
			float num3;
			this.storage.ConsumeAndGetDisease(MercuryLight.ELEMENT_TAG, num, out num2, out diseaseInfo, out num3);
			base.sm.HasEnoughFuel.Set(true, this, false);
		}

		// Token: 0x06008EA4 RID: 36516 RVA: 0x003449FD File Offset: 0x00342BFD
		public bool CanRun()
		{
			return true;
		}

		// Token: 0x04006CDE RID: 27870
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04006CDF RID: 27871
		[MyCmpGet]
		private Light2D light;

		// Token: 0x04006CE0 RID: 27872
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04006CE1 RID: 27873
		[MyCmpGet]
		private ConduitConsumer conduitConsumer;

		// Token: 0x04006CE2 RID: 27874
		private MeterController lightIntensityMeterController;
	}
}
