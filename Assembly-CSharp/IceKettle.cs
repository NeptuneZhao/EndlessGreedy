using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x020006F2 RID: 1778
public class IceKettle : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>
{
	// Token: 0x06002D5C RID: 11612 RVA: 0x000FEA48 File Offset: 0x000FCC48
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.root.EventHandlerTransition(GameHashes.WorkableStartWork, this.inUse, (IceKettle.Instance smi, object obj) => true).EventHandler(GameHashes.OnStorageChange, delegate(IceKettle.Instance smi)
		{
			smi.UpdateMeter();
		});
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.idle);
		this.operational.idle.PlayAnim(IceKettle.IDEL_ANIM_STATE).DefaultState(this.operational.idle.waitingForSolids);
		this.operational.idle.waitingForSolids.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientSolids, null).EventTransition(GameHashes.OnStorageChange, this.operational.idle.waitingForSpaceInLiquidTank, (IceKettle.Instance smi) => IceKettle.HasEnoughSolidsToMelt(smi));
		this.operational.idle.waitingForSpaceInLiquidTank.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientLiquidSpace, null).EventTransition(GameHashes.OnStorageChange, this.operational.idle.notEnoughFuel, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.LiquidTankHasCapacityForNextBatch));
		this.operational.idle.notEnoughFuel.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientFuel, null).EventTransition(GameHashes.OnStorageChange, this.operational.melting, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch));
		this.operational.melting.Toggle("Operational Active State", new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.SetOperationalActiveStatesTrue), new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.SetOperationalActiveStatesFalse)).DefaultState(this.operational.melting.entering);
		this.operational.melting.entering.PlayAnim(IceKettle.BOILING_PRE_ANIM_NAME, KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.melting.working);
		this.operational.melting.working.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleMelting, null).DefaultState(this.operational.melting.working.idle).PlayAnim(IceKettle.BOILING_LOOP_ANIM_NAME, KAnim.PlayMode.Loop);
		this.operational.melting.working.idle.ParamTransition<float>(this.MeltingTimer, this.operational.melting.working.complete, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Parameter<float>.Callback(IceKettle.IsDoneMelting)).Update(new Action<IceKettle.Instance, float>(IceKettle.MeltingTimerUpdate), UpdateRate.SIM_200ms, false);
		this.operational.melting.working.complete.Enter(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.ResetMeltingTimer)).Enter(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.MeltNextBatch)).EnterTransition(this.operational.melting.working.idle, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch)).EnterTransition(this.operational.melting.exit, GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Not(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch)));
		this.operational.melting.exit.PlayAnim(IceKettle.BOILING_PST_ANIM_NAME, KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.idle);
		this.inUse.EventHandlerTransition(GameHashes.WorkableStopWork, this.noOperational, (IceKettle.Instance smi, object obj) => true).ScheduleGoTo(new Func<IceKettle.Instance, float>(IceKettle.GetInUseTimeout), this.noOperational);
	}

	// Token: 0x06002D5D RID: 11613 RVA: 0x000FEE31 File Offset: 0x000FD031
	public static void SetOperationalActiveStatesTrue(IceKettle.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x06002D5E RID: 11614 RVA: 0x000FEE40 File Offset: 0x000FD040
	public static void SetOperationalActiveStatesFalse(IceKettle.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x06002D5F RID: 11615 RVA: 0x000FEE4F File Offset: 0x000FD04F
	public static float GetInUseTimeout(IceKettle.Instance smi)
	{
		return smi.InUseWorkableDuration + 1f;
	}

	// Token: 0x06002D60 RID: 11616 RVA: 0x000FEE5D File Offset: 0x000FD05D
	public static void ResetMeltingTimer(IceKettle.Instance smi)
	{
		smi.sm.MeltingTimer.Set(0f, smi, false);
	}

	// Token: 0x06002D61 RID: 11617 RVA: 0x000FEE77 File Offset: 0x000FD077
	public static bool HasEnoughSolidsToMelt(IceKettle.Instance smi)
	{
		return smi.HasAtLeastOneBatchOfSolidsWaitingToMelt;
	}

	// Token: 0x06002D62 RID: 11618 RVA: 0x000FEE7F File Offset: 0x000FD07F
	public static bool LiquidTankHasCapacityForNextBatch(IceKettle.Instance smi)
	{
		return smi.LiquidTankHasCapacityForNextBatch;
	}

	// Token: 0x06002D63 RID: 11619 RVA: 0x000FEE87 File Offset: 0x000FD087
	public static bool HasEnoughFuelForNextBacth(IceKettle.Instance smi)
	{
		return smi.HasEnoughFuelUnitsToMeltNextBatch;
	}

	// Token: 0x06002D64 RID: 11620 RVA: 0x000FEE8F File Offset: 0x000FD08F
	public static bool CanMeltNextBatch(IceKettle.Instance smi)
	{
		return smi.HasAtLeastOneBatchOfSolidsWaitingToMelt && IceKettle.LiquidTankHasCapacityForNextBatch(smi) && IceKettle.HasEnoughFuelForNextBacth(smi);
	}

	// Token: 0x06002D65 RID: 11621 RVA: 0x000FEEA9 File Offset: 0x000FD0A9
	public static bool IsDoneMelting(IceKettle.Instance smi, float timePassed)
	{
		return timePassed >= smi.MeltDurationPerBatch;
	}

	// Token: 0x06002D66 RID: 11622 RVA: 0x000FEEB8 File Offset: 0x000FD0B8
	public static void MeltingTimerUpdate(IceKettle.Instance smi, float dt)
	{
		float num = smi.sm.MeltingTimer.Get(smi);
		smi.sm.MeltingTimer.Set(num + dt, smi, false);
	}

	// Token: 0x06002D67 RID: 11623 RVA: 0x000FEEED File Offset: 0x000FD0ED
	public static void MeltNextBatch(IceKettle.Instance smi)
	{
		smi.MeltNextBatch();
	}

	// Token: 0x04001A4F RID: 6735
	public static string LIQUID_METER_TARGET_NAME = "kettle_meter_target";

	// Token: 0x04001A50 RID: 6736
	public static string LIQUID_METER_ANIM_NAME = "meter_kettle";

	// Token: 0x04001A51 RID: 6737
	public static string IDEL_ANIM_STATE = "on";

	// Token: 0x04001A52 RID: 6738
	public static string BOILING_PRE_ANIM_NAME = "boiling_pre";

	// Token: 0x04001A53 RID: 6739
	public static string BOILING_LOOP_ANIM_NAME = "boiling_loop";

	// Token: 0x04001A54 RID: 6740
	public static string BOILING_PST_ANIM_NAME = "boiling_pst";

	// Token: 0x04001A55 RID: 6741
	private const float InUseTimeout = 5f;

	// Token: 0x04001A56 RID: 6742
	public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State noOperational;

	// Token: 0x04001A57 RID: 6743
	public IceKettle.OperationalStates operational;

	// Token: 0x04001A58 RID: 6744
	public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State inUse;

	// Token: 0x04001A59 RID: 6745
	public StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.FloatParameter MeltingTimer;

	// Token: 0x02001522 RID: 5410
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06008D56 RID: 36182 RVA: 0x0033F298 File Offset: 0x0033D498
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			string txt = string.Format(UI.BUILDINGEFFECTS.KETTLE_MELT_RATE, GameUtil.GetFormattedMass(this.KGMeltedPerSecond, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			string tooltip = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.KETTLE_MELT_RATE, GameUtil.GetFormattedMass(this.KGToMeltPerBatch, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.TargetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			Descriptor item = new Descriptor(txt, tooltip, Descriptor.DescriptorType.Effect, false);
			list.Add(item);
			return list;
		}

		// Token: 0x04006C00 RID: 27648
		public SimHashes exhaust_tag;

		// Token: 0x04006C01 RID: 27649
		public Tag targetElementTag;

		// Token: 0x04006C02 RID: 27650
		public Tag fuelElementTag;

		// Token: 0x04006C03 RID: 27651
		public float KGToMeltPerBatch;

		// Token: 0x04006C04 RID: 27652
		public float KGMeltedPerSecond;

		// Token: 0x04006C05 RID: 27653
		public float TargetTemperature;

		// Token: 0x04006C06 RID: 27654
		public float EnergyPerUnitOfLumber;

		// Token: 0x04006C07 RID: 27655
		public float ExhaustMassPerUnitOfLumber;
	}

	// Token: 0x02001523 RID: 5411
	public class WorkingStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x04006C08 RID: 27656
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State idle;

		// Token: 0x04006C09 RID: 27657
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State complete;
	}

	// Token: 0x02001524 RID: 5412
	public class MeltingStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x04006C0A RID: 27658
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State entering;

		// Token: 0x04006C0B RID: 27659
		public IceKettle.WorkingStates working;

		// Token: 0x04006C0C RID: 27660
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State exit;
	}

	// Token: 0x02001525 RID: 5413
	public class IdleStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x04006C0D RID: 27661
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State notEnoughFuel;

		// Token: 0x04006C0E RID: 27662
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State waitingForSolids;

		// Token: 0x04006C0F RID: 27663
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State waitingForSpaceInLiquidTank;
	}

	// Token: 0x02001526 RID: 5414
	public class OperationalStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x04006C10 RID: 27664
		public IceKettle.MeltingStates melting;

		// Token: 0x04006C11 RID: 27665
		public IceKettle.IdleStates idle;
	}

	// Token: 0x02001527 RID: 5415
	public new class Instance : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.GameInstance
	{
		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06008D5C RID: 36188 RVA: 0x0033F339 File Offset: 0x0033D539
		public float CurrentTemperatureOfSolidsStored
		{
			get
			{
				if (this.kettleStorage.MassStored() <= 0f)
				{
					return 0f;
				}
				return this.kettleStorage.items[0].GetComponent<PrimaryElement>().Temperature;
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06008D5D RID: 36189 RVA: 0x0033F36E File Offset: 0x0033D56E
		public float MeltDurationPerBatch
		{
			get
			{
				return base.def.KGToMeltPerBatch / base.def.KGMeltedPerSecond;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06008D5E RID: 36190 RVA: 0x0033F387 File Offset: 0x0033D587
		public float FuelUnitsAvailable
		{
			get
			{
				return this.fuelStorage.MassStored();
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06008D5F RID: 36191 RVA: 0x0033F394 File Offset: 0x0033D594
		public bool HasAtLeastOneBatchOfSolidsWaitingToMelt
		{
			get
			{
				return this.kettleStorage.MassStored() >= base.def.KGToMeltPerBatch;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06008D60 RID: 36192 RVA: 0x0033F3B1 File Offset: 0x0033D5B1
		public bool HasEnoughFuelUnitsToMeltNextBatch
		{
			get
			{
				return this.kettleStorage.MassStored() <= 0f || this.FuelUnitsAvailable >= this.FuelRequiredForNextBratch;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06008D61 RID: 36193 RVA: 0x0033F3D8 File Offset: 0x0033D5D8
		public bool LiquidTankHasCapacityForNextBatch
		{
			get
			{
				return this.outputStorage.RemainingCapacity() >= base.def.KGToMeltPerBatch;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06008D62 RID: 36194 RVA: 0x0033F3F5 File Offset: 0x0033D5F5
		public float LiquidTankCapacity
		{
			get
			{
				return this.outputStorage.capacityKg;
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06008D63 RID: 36195 RVA: 0x0033F402 File Offset: 0x0033D602
		public float LiquidStored
		{
			get
			{
				return this.outputStorage.MassStored();
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06008D64 RID: 36196 RVA: 0x0033F40F File Offset: 0x0033D60F
		public float FuelRequiredForNextBratch
		{
			get
			{
				return this.GetUnitsOfFuelRequiredToMelt(this.elementToMelt, base.def.KGToMeltPerBatch, this.CurrentTemperatureOfSolidsStored);
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06008D65 RID: 36197 RVA: 0x0033F42E File Offset: 0x0033D62E
		public float InUseWorkableDuration
		{
			get
			{
				return this.dupeWorkable.workTime;
			}
		}

		// Token: 0x06008D66 RID: 36198 RVA: 0x0033F43C File Offset: 0x0033D63C
		public Instance(IStateMachineTarget master, IceKettle.Def def) : base(master, def)
		{
			this.elementToMelt = ElementLoader.GetElement(def.targetElementTag);
			this.LiquidMeter = new MeterController(this.animController, IceKettle.LIQUID_METER_TARGET_NAME, IceKettle.LIQUID_METER_ANIM_NAME, Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.fuelStorage = components[0];
			this.kettleStorage = components[1];
			this.outputStorage = components[2];
		}

		// Token: 0x06008D67 RID: 36199 RVA: 0x0033F4AC File Offset: 0x0033D6AC
		public override void StartSM()
		{
			base.StartSM();
			this.UpdateMeter();
		}

		// Token: 0x06008D68 RID: 36200 RVA: 0x0033F4BA File Offset: 0x0033D6BA
		public void UpdateMeter()
		{
			this.LiquidMeter.SetPositionPercent(this.outputStorage.MassStored() / this.outputStorage.capacityKg);
		}

		// Token: 0x06008D69 RID: 36201 RVA: 0x0033F4E0 File Offset: 0x0033D6E0
		public void MeltNextBatch()
		{
			if (!this.HasAtLeastOneBatchOfSolidsWaitingToMelt)
			{
				return;
			}
			PrimaryElement component = this.kettleStorage.FindFirst(base.def.targetElementTag).GetComponent<PrimaryElement>();
			float num = Mathf.Min(this.GetUnitsOfFuelRequiredToMelt(this.elementToMelt, base.def.KGToMeltPerBatch, component.Temperature), this.FuelUnitsAvailable);
			float mass = 0f;
			float num2 = 0f;
			SimUtil.DiseaseInfo diseaseInfo;
			this.kettleStorage.ConsumeAndGetDisease(this.elementToMelt.id.CreateTag(), base.def.KGToMeltPerBatch, out mass, out diseaseInfo, out num2);
			this.outputStorage.AddElement(this.elementToMelt.highTempTransitionTarget, mass, base.def.TargetTemperature, diseaseInfo.idx, diseaseInfo.count, false, true);
			float temperature = this.fuelStorage.FindFirst(base.def.fuelElementTag).GetComponent<PrimaryElement>().Temperature;
			this.fuelStorage.ConsumeIgnoringDisease(base.def.fuelElementTag, num);
			float mass2 = num * base.def.ExhaustMassPerUnitOfLumber;
			Element element = ElementLoader.FindElementByHash(base.def.exhaust_tag);
			SimMessages.AddRemoveSubstance(Grid.PosToCell(base.gameObject), element.id, null, mass2, temperature, byte.MaxValue, 0, true, -1);
		}

		// Token: 0x06008D6A RID: 36202 RVA: 0x0033F624 File Offset: 0x0033D824
		public float GetUnitsOfFuelRequiredToMelt(Element elementToMelt, float massToMelt_KG, float elementToMelt_initialTemperature)
		{
			if (!elementToMelt.IsSolid)
			{
				return -1f;
			}
			float num = massToMelt_KG * elementToMelt.specificHeatCapacity * elementToMelt_initialTemperature;
			float targetTemperature = base.def.TargetTemperature;
			return (massToMelt_KG * elementToMelt.specificHeatCapacity * targetTemperature - num) / base.def.EnergyPerUnitOfLumber;
		}

		// Token: 0x04006C12 RID: 27666
		private Storage fuelStorage;

		// Token: 0x04006C13 RID: 27667
		private Storage kettleStorage;

		// Token: 0x04006C14 RID: 27668
		private Storage outputStorage;

		// Token: 0x04006C15 RID: 27669
		private Element elementToMelt;

		// Token: 0x04006C16 RID: 27670
		private MeterController LiquidMeter;

		// Token: 0x04006C17 RID: 27671
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04006C18 RID: 27672
		[MyCmpGet]
		private IceKettleWorkable dupeWorkable;

		// Token: 0x04006C19 RID: 27673
		[MyCmpGet]
		private KBatchedAnimController animController;
	}
}
