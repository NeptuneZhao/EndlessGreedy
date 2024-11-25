using System;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B58 RID: 2904
public class WarmBlooded : StateMachineComponent<WarmBlooded.StatesInstance>
{
	// Token: 0x060056BE RID: 22206 RVA: 0x001EFF9B File Offset: 0x001EE19B
	public static bool IsCold(WarmBlooded.StatesInstance smi)
	{
		return !smi.IsSimpleHeatProducer() && smi.IsCold();
	}

	// Token: 0x060056BF RID: 22207 RVA: 0x001EFFAD File Offset: 0x001EE1AD
	public static bool IsHot(WarmBlooded.StatesInstance smi)
	{
		return !smi.IsSimpleHeatProducer() && smi.IsHot();
	}

	// Token: 0x060056C0 RID: 22208 RVA: 0x001EFFC0 File Offset: 0x001EE1C0
	public static void WarmingRegulator(WarmBlooded.StatesInstance smi, float dt)
	{
		PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
		float num = SimUtil.EnergyFlowToTemperatureDelta(smi.master.CoolingKW, component.Element.specificHeatCapacity, component.Mass);
		float num2 = smi.IdealTemperature - smi.BodyTemperature;
		float num3 = 1f;
		if ((num - smi.baseTemperatureModification.Value) * dt < num2)
		{
			num3 = Mathf.Clamp(num2 / ((num - smi.baseTemperatureModification.Value) * dt), 0f, 1f);
		}
		smi.bodyRegulator.SetValue(-num * num3);
		if (smi.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
		{
			smi.burningCalories.SetValue(-smi.master.CoolingKW * num3 / smi.master.KCal2Joules);
		}
	}

	// Token: 0x060056C1 RID: 22209 RVA: 0x001F0084 File Offset: 0x001EE284
	public static void CoolingRegulator(WarmBlooded.StatesInstance smi, float dt)
	{
		PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
		float num = SimUtil.EnergyFlowToTemperatureDelta(smi.master.BaseGenerationKW, component.Element.specificHeatCapacity, component.Mass);
		float num2 = SimUtil.EnergyFlowToTemperatureDelta(smi.master.WarmingKW, component.Element.specificHeatCapacity, component.Mass);
		float num3 = smi.IdealTemperature - smi.BodyTemperature;
		float num4 = 1f;
		if (num2 + num > num3)
		{
			num4 = Mathf.Max(0f, num3 - num) / num2;
		}
		smi.bodyRegulator.SetValue(num2 * num4);
		if (smi.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
		{
			smi.burningCalories.SetValue(-smi.master.WarmingKW * num4 * 1000f / smi.master.KCal2Joules);
		}
	}

	// Token: 0x060056C2 RID: 22210 RVA: 0x001F0156 File Offset: 0x001EE356
	protected override void OnPrefabInit()
	{
		this.temperature = Db.Get().Amounts.Get(this.TemperatureAmountName).Lookup(base.gameObject);
		this.primaryElement = base.GetComponent<PrimaryElement>();
	}

	// Token: 0x060056C3 RID: 22211 RVA: 0x001F018A File Offset: 0x001EE38A
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x060056C4 RID: 22212 RVA: 0x001F0197 File Offset: 0x001EE397
	public void SetTemperatureImmediate(float t)
	{
		this.temperature.value = t;
	}

	// Token: 0x040038CE RID: 14542
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x040038CF RID: 14543
	public AmountInstance temperature;

	// Token: 0x040038D0 RID: 14544
	private PrimaryElement primaryElement;

	// Token: 0x040038D1 RID: 14545
	public WarmBlooded.ComplexityType complexity = WarmBlooded.ComplexityType.FullHomeostasis;

	// Token: 0x040038D2 RID: 14546
	public string TemperatureAmountName = "Temperature";

	// Token: 0x040038D3 RID: 14547
	public float IdealTemperature = DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;

	// Token: 0x040038D4 RID: 14548
	public float BaseGenerationKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_BASE_GENERATION_KILOWATTS;

	// Token: 0x040038D5 RID: 14549
	public string BaseTemperatureModifierDescription = DUPLICANTS.MODEL.STANDARD.NAME;

	// Token: 0x040038D6 RID: 14550
	public float KCal2Joules = DUPLICANTSTATS.STANDARD.BaseStats.KCAL2JOULES;

	// Token: 0x040038D7 RID: 14551
	public float WarmingKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_WARMING_KILOWATTS;

	// Token: 0x040038D8 RID: 14552
	public float CoolingKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_COOLING_KILOWATTS;

	// Token: 0x040038D9 RID: 14553
	public string CaloriesModifierDescription = DUPLICANTS.MODIFIERS.BURNINGCALORIES.NAME;

	// Token: 0x040038DA RID: 14554
	public string BodyRegulatorModifierDescription = DUPLICANTS.MODIFIERS.HOMEOSTASIS.NAME;

	// Token: 0x040038DB RID: 14555
	public const float TRANSITION_DELAY_HOT = 3f;

	// Token: 0x040038DC RID: 14556
	public const float TRANSITION_DELAY_COLD = 3f;

	// Token: 0x02001BAA RID: 7082
	public enum ComplexityType
	{
		// Token: 0x0400805D RID: 32861
		SimpleHeatProduction,
		// Token: 0x0400805E RID: 32862
		HomeostasisWithoutCaloriesImpact,
		// Token: 0x0400805F RID: 32863
		FullHomeostasis
	}

	// Token: 0x02001BAB RID: 7083
	public class StatesInstance : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.GameInstance
	{
		// Token: 0x0600A418 RID: 42008 RVA: 0x0038B4FC File Offset: 0x003896FC
		public StatesInstance(WarmBlooded smi) : base(smi)
		{
			this.baseTemperatureModification = new AttributeModifier(base.master.TemperatureAmountName + "Delta", 0f, base.master.BaseTemperatureModifierDescription, false, true, false);
			base.master.GetAttributes().Add(this.baseTemperatureModification);
			if (base.master.complexity != WarmBlooded.ComplexityType.SimpleHeatProduction)
			{
				this.bodyRegulator = new AttributeModifier(base.master.TemperatureAmountName + "Delta", 0f, base.master.BodyRegulatorModifierDescription, false, true, false);
				base.master.GetAttributes().Add(this.bodyRegulator);
			}
			if (base.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
			{
				this.burningCalories = new AttributeModifier("CaloriesDelta", 0f, base.master.CaloriesModifierDescription, false, false, false);
				base.master.GetAttributes().Add(this.burningCalories);
			}
			base.master.SetTemperatureImmediate(this.IdealTemperature);
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x0600A419 RID: 42009 RVA: 0x0038B607 File Offset: 0x00389807
		public float IdealTemperature
		{
			get
			{
				return base.master.IdealTemperature;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x0600A41A RID: 42010 RVA: 0x0038B614 File Offset: 0x00389814
		public float TemperatureDelta
		{
			get
			{
				return this.bodyRegulator.Value;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x0600A41B RID: 42011 RVA: 0x0038B621 File Offset: 0x00389821
		public float BodyTemperature
		{
			get
			{
				return base.master.primaryElement.Temperature;
			}
		}

		// Token: 0x0600A41C RID: 42012 RVA: 0x0038B633 File Offset: 0x00389833
		public bool IsSimpleHeatProducer()
		{
			return base.master.complexity == WarmBlooded.ComplexityType.SimpleHeatProduction;
		}

		// Token: 0x0600A41D RID: 42013 RVA: 0x0038B643 File Offset: 0x00389843
		public bool IsHot()
		{
			return this.BodyTemperature > this.IdealTemperature;
		}

		// Token: 0x0600A41E RID: 42014 RVA: 0x0038B653 File Offset: 0x00389853
		public bool IsCold()
		{
			return this.BodyTemperature < this.IdealTemperature;
		}

		// Token: 0x04008060 RID: 32864
		public AttributeModifier baseTemperatureModification;

		// Token: 0x04008061 RID: 32865
		public AttributeModifier bodyRegulator;

		// Token: 0x04008062 RID: 32866
		public AttributeModifier burningCalories;
	}

	// Token: 0x02001BAC RID: 7084
	public class States : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded>
	{
		// Token: 0x0600A41F RID: 42015 RVA: 0x0038B664 File Offset: 0x00389864
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.alive.normal;
			this.root.TagTransition(GameTags.Dead, this.dead, false).Enter(delegate(WarmBlooded.StatesInstance smi)
			{
				PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
				float value = SimUtil.EnergyFlowToTemperatureDelta(smi.master.BaseGenerationKW, component.Element.specificHeatCapacity, component.Mass);
				smi.baseTemperatureModification.SetValue(value);
				CreatureSimTemperatureTransfer component2 = smi.master.GetComponent<CreatureSimTemperatureTransfer>();
				component2.NonSimTemperatureModifiers.Add(smi.baseTemperatureModification);
				if (!smi.IsSimpleHeatProducer())
				{
					component2.NonSimTemperatureModifiers.Add(smi.bodyRegulator);
				}
			});
			this.alive.normal.Transition(this.alive.cold.transition, new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold), UpdateRate.SIM_200ms).Transition(this.alive.hot.transition, new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot), UpdateRate.SIM_200ms);
			this.alive.cold.transition.ScheduleGoTo(3f, this.alive.cold.regulating).Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold)), UpdateRate.SIM_200ms);
			this.alive.cold.regulating.Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold)), UpdateRate.SIM_200ms).Update("ColdRegulating", new Action<WarmBlooded.StatesInstance, float>(WarmBlooded.CoolingRegulator), UpdateRate.SIM_200ms, false).Exit(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.bodyRegulator.SetValue(0f);
				if (smi.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
				{
					smi.burningCalories.SetValue(0f);
				}
			});
			this.alive.hot.transition.ScheduleGoTo(3f, this.alive.hot.regulating).Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot)), UpdateRate.SIM_200ms);
			this.alive.hot.regulating.Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot)), UpdateRate.SIM_200ms).Update("WarmRegulating", new Action<WarmBlooded.StatesInstance, float>(WarmBlooded.WarmingRegulator), UpdateRate.SIM_200ms, false).Exit(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.bodyRegulator.SetValue(0f);
			});
			this.dead.Enter(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.master.enabled = false;
			});
		}

		// Token: 0x04008063 RID: 32867
		public WarmBlooded.States.AliveState alive;

		// Token: 0x04008064 RID: 32868
		public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State dead;

		// Token: 0x0200262B RID: 9771
		public class RegulatingState : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State
		{
			// Token: 0x0400A9D9 RID: 43481
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State transition;

			// Token: 0x0400A9DA RID: 43482
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State regulating;
		}

		// Token: 0x0200262C RID: 9772
		public class AliveState : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State
		{
			// Token: 0x0400A9DB RID: 43483
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State normal;

			// Token: 0x0400A9DC RID: 43484
			public WarmBlooded.States.RegulatingState cold;

			// Token: 0x0400A9DD RID: 43485
			public WarmBlooded.States.RegulatingState hot;
		}
	}
}
