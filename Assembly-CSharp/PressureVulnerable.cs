using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000819 RID: 2073
[SkipSaveFileSerialization]
public class PressureVulnerable : StateMachineComponent<PressureVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x06003956 RID: 14678 RVA: 0x00138B6C File Offset: 0x00136D6C
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x06003957 RID: 14679 RVA: 0x00138B8E File Offset: 0x00136D8E
	public bool IsSafeElement(Element element)
	{
		return this.safe_atmospheres == null || this.safe_atmospheres.Count == 0 || this.safe_atmospheres.Contains(element);
	}

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x06003958 RID: 14680 RVA: 0x00138BB6 File Offset: 0x00136DB6
	public PressureVulnerable.PressureState ExternalPressureState
	{
		get
		{
			return this.pressureState;
		}
	}

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x06003959 RID: 14681 RVA: 0x00138BBE File Offset: 0x00136DBE
	public bool IsLethal
	{
		get
		{
			return this.pressureState == PressureVulnerable.PressureState.LethalHigh || this.pressureState == PressureVulnerable.PressureState.LethalLow || !this.testAreaElementSafe;
		}
	}

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x0600395A RID: 14682 RVA: 0x00138BDC File Offset: 0x00136DDC
	public bool IsNormal
	{
		get
		{
			return this.testAreaElementSafe && this.pressureState == PressureVulnerable.PressureState.Normal;
		}
	}

	// Token: 0x0600395B RID: 14683 RVA: 0x00138BF4 File Offset: 0x00136DF4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Amounts amounts = base.gameObject.GetAmounts();
		this.displayPressureAmount = amounts.Add(new AmountInstance(Db.Get().Amounts.AirPressure, base.gameObject));
	}

	// Token: 0x0600395C RID: 14684 RVA: 0x00138C3C File Offset: 0x00136E3C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SlicedUpdaterSim1000ms<PressureVulnerable>.instance.RegisterUpdate1000ms(this);
		this.cell = Grid.PosToCell(this);
		base.smi.sm.pressure.Set(1f, base.smi, false);
		base.smi.sm.safe_element.Set(this.testAreaElementSafe, base.smi, false);
		base.smi.StartSM();
	}

	// Token: 0x0600395D RID: 14685 RVA: 0x00138CB6 File Offset: 0x00136EB6
	protected override void OnCleanUp()
	{
		SlicedUpdaterSim1000ms<PressureVulnerable>.instance.UnregisterUpdate1000ms(this);
		base.OnCleanUp();
	}

	// Token: 0x0600395E RID: 14686 RVA: 0x00138CCC File Offset: 0x00136ECC
	public void Configure(SimHashes[] safeAtmospheres = null)
	{
		this.pressure_sensitive = false;
		this.pressureWarning_Low = float.MinValue;
		this.pressureLethal_Low = float.MinValue;
		this.pressureLethal_High = float.MaxValue;
		this.pressureWarning_High = float.MaxValue;
		this.safe_atmospheres = new HashSet<Element>();
		if (safeAtmospheres != null)
		{
			foreach (SimHashes hash in safeAtmospheres)
			{
				this.safe_atmospheres.Add(ElementLoader.FindElementByHash(hash));
			}
		}
	}

	// Token: 0x0600395F RID: 14687 RVA: 0x00138D40 File Offset: 0x00136F40
	public void Configure(float pressureWarningLow = 0.25f, float pressureLethalLow = 0.01f, float pressureWarningHigh = 10f, float pressureLethalHigh = 30f, SimHashes[] safeAtmospheres = null)
	{
		this.pressure_sensitive = true;
		this.pressureWarning_Low = pressureWarningLow;
		this.pressureLethal_Low = pressureLethalLow;
		this.pressureLethal_High = pressureLethalHigh;
		this.pressureWarning_High = pressureWarningHigh;
		this.safe_atmospheres = new HashSet<Element>();
		if (safeAtmospheres != null)
		{
			foreach (SimHashes hash in safeAtmospheres)
			{
				this.safe_atmospheres.Add(ElementLoader.FindElementByHash(hash));
			}
		}
	}

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x06003960 RID: 14688 RVA: 0x00138DA7 File Offset: 0x00136FA7
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Pressure,
				WiltCondition.Condition.AtmosphereElement
			};
		}
	}

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06003961 RID: 14689 RVA: 0x00138DB8 File Offset: 0x00136FB8
	public string WiltStateString
	{
		get
		{
			string text = "";
			if (base.smi.IsInsideState(base.smi.sm.warningLow) || base.smi.IsInsideState(base.smi.sm.lethalLow))
			{
				text += Db.Get().CreatureStatusItems.AtmosphericPressureTooLow.resolveStringCallback(CREATURES.STATUSITEMS.ATMOSPHERICPRESSURETOOLOW.NAME, this);
			}
			else if (base.smi.IsInsideState(base.smi.sm.warningHigh) || base.smi.IsInsideState(base.smi.sm.lethalHigh))
			{
				text += Db.Get().CreatureStatusItems.AtmosphericPressureTooHigh.resolveStringCallback(CREATURES.STATUSITEMS.ATMOSPHERICPRESSURETOOHIGH.NAME, this);
			}
			else if (base.smi.IsInsideState(base.smi.sm.unsafeElement))
			{
				text += Db.Get().CreatureStatusItems.WrongAtmosphere.resolveStringCallback(CREATURES.STATUSITEMS.WRONGATMOSPHERE.NAME, this);
			}
			return text;
		}
	}

	// Token: 0x06003962 RID: 14690 RVA: 0x00138EE5 File Offset: 0x001370E5
	public bool IsSafePressure(float pressure)
	{
		return !this.pressure_sensitive || (pressure > this.pressureLethal_Low && pressure < this.pressureLethal_High);
	}

	// Token: 0x06003963 RID: 14691 RVA: 0x00138F08 File Offset: 0x00137108
	public void SlicedSim1000ms(float dt)
	{
		float value = base.smi.sm.pressure.Get(base.smi) * 0.7f + this.GetPressureOverArea(this.cell) * 0.3f;
		this.safe_element *= 0.7f;
		if (this.testAreaElementSafe)
		{
			this.safe_element += 0.3f;
		}
		this.displayPressureAmount.value = value;
		bool value2 = this.safe_atmospheres == null || this.safe_atmospheres.Count == 0 || this.safe_element >= 0.06f;
		base.smi.sm.safe_element.Set(value2, base.smi, false);
		base.smi.sm.pressure.Set(value, base.smi, false);
	}

	// Token: 0x06003964 RID: 14692 RVA: 0x00138FE7 File Offset: 0x001371E7
	public float GetExternalPressure()
	{
		return this.GetPressureOverArea(this.cell);
	}

	// Token: 0x06003965 RID: 14693 RVA: 0x00138FF8 File Offset: 0x001371F8
	private float GetPressureOverArea(int cell)
	{
		bool flag = this.testAreaElementSafe;
		PressureVulnerable.testAreaPressure = 0f;
		PressureVulnerable.testAreaCount = 0;
		this.testAreaElementSafe = false;
		this.currentAtmoElement = null;
		this.occupyArea.TestArea(cell, this, PressureVulnerable.testAreaCB);
		PressureVulnerable.testAreaPressure = ((PressureVulnerable.testAreaCount > 0) ? (PressureVulnerable.testAreaPressure / (float)PressureVulnerable.testAreaCount) : 0f);
		if (this.testAreaElementSafe != flag)
		{
			base.Trigger(-2023773544, null);
		}
		return PressureVulnerable.testAreaPressure;
	}

	// Token: 0x06003966 RID: 14694 RVA: 0x00139078 File Offset: 0x00137278
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.pressure_sensitive)
		{
			list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_PRESSURE, GameUtil.GetFormattedMass(this.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_PRESSURE, GameUtil.GetFormattedMass(this.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		}
		if (this.safe_atmospheres != null && this.safe_atmospheres.Count > 0)
		{
			string text = "";
			bool flag = false;
			bool flag2 = false;
			foreach (Element element in this.safe_atmospheres)
			{
				flag |= element.IsGas;
				flag2 |= element.IsLiquid;
				text = text + "\n        • " + element.name;
			}
			if (flag && flag2)
			{
				list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, text), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE_MIXED, text), Descriptor.DescriptorType.Requirement, false));
			}
			if (flag)
			{
				list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, text), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE, text), Descriptor.DescriptorType.Requirement, false));
			}
			else
			{
				list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, text), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE_LIQUID, text), Descriptor.DescriptorType.Requirement, false));
			}
		}
		return list;
	}

	// Token: 0x0400227E RID: 8830
	private const float kTrailingWeight = 0.7f;

	// Token: 0x0400227F RID: 8831
	private const float kLeadingWeight = 0.3f;

	// Token: 0x04002280 RID: 8832
	private const float kSafeElementThreshold = 0.06f;

	// Token: 0x04002281 RID: 8833
	private float safe_element = 1f;

	// Token: 0x04002282 RID: 8834
	private OccupyArea _occupyArea;

	// Token: 0x04002283 RID: 8835
	public float pressureLethal_Low;

	// Token: 0x04002284 RID: 8836
	public float pressureWarning_Low;

	// Token: 0x04002285 RID: 8837
	public float pressureWarning_High;

	// Token: 0x04002286 RID: 8838
	public float pressureLethal_High;

	// Token: 0x04002287 RID: 8839
	private static float testAreaPressure;

	// Token: 0x04002288 RID: 8840
	private static int testAreaCount;

	// Token: 0x04002289 RID: 8841
	public bool testAreaElementSafe = true;

	// Token: 0x0400228A RID: 8842
	public Element currentAtmoElement;

	// Token: 0x0400228B RID: 8843
	private static Func<int, object, bool> testAreaCB = delegate(int test_cell, object data)
	{
		PressureVulnerable pressureVulnerable = (PressureVulnerable)data;
		if (!Grid.IsSolidCell(test_cell))
		{
			Element element = Grid.Element[test_cell];
			if (pressureVulnerable.IsSafeElement(element))
			{
				PressureVulnerable.testAreaPressure += Grid.Mass[test_cell];
				PressureVulnerable.testAreaCount++;
				pressureVulnerable.testAreaElementSafe = true;
				pressureVulnerable.currentAtmoElement = element;
			}
			if (pressureVulnerable.currentAtmoElement == null)
			{
				pressureVulnerable.currentAtmoElement = element;
			}
		}
		return true;
	};

	// Token: 0x0400228C RID: 8844
	private AmountInstance displayPressureAmount;

	// Token: 0x0400228D RID: 8845
	public bool pressure_sensitive = true;

	// Token: 0x0400228E RID: 8846
	public HashSet<Element> safe_atmospheres = new HashSet<Element>();

	// Token: 0x0400228F RID: 8847
	private int cell;

	// Token: 0x04002290 RID: 8848
	private PressureVulnerable.PressureState pressureState = PressureVulnerable.PressureState.Normal;

	// Token: 0x02001724 RID: 5924
	public class StatesInstance : GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.GameInstance
	{
		// Token: 0x060094CE RID: 38094 RVA: 0x0035DD62 File Offset: 0x0035BF62
		public StatesInstance(PressureVulnerable master) : base(master)
		{
			if (Db.Get().Amounts.Maturity.Lookup(base.gameObject) != null)
			{
				this.hasMaturity = true;
			}
		}

		// Token: 0x040071E2 RID: 29154
		public bool hasMaturity;
	}

	// Token: 0x02001725 RID: 5925
	public class States : GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable>
	{
		// Token: 0x060094CF RID: 38095 RVA: 0x0035DD90 File Offset: 0x0035BF90
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal;
			this.lethalLow.ParamTransition<float>(this.pressure, this.warningLow, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureLethal_Low).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.LethalLow;
			}).TriggerOnEnter(GameHashes.LowPressureFatal, null);
			this.lethalHigh.ParamTransition<float>(this.pressure, this.warningHigh, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureLethal_High).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.LethalHigh;
			}).TriggerOnEnter(GameHashes.HighPressureFatal, null);
			this.warningLow.ParamTransition<float>(this.pressure, this.lethalLow, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureLethal_Low).ParamTransition<float>(this.pressure, this.normal, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureWarning_Low).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.WarningLow;
			}).TriggerOnEnter(GameHashes.LowPressureWarning, null);
			this.unsafeElement.ParamTransition<bool>(this.safe_element, this.normal, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsTrue).TriggerOnExit(GameHashes.CorrectAtmosphere, null).TriggerOnEnter(GameHashes.WrongAtmosphere, null);
			this.warningHigh.ParamTransition<float>(this.pressure, this.lethalHigh, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureLethal_High).ParamTransition<float>(this.pressure, this.normal, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureWarning_High).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.WarningHigh;
			}).TriggerOnEnter(GameHashes.HighPressureWarning, null);
			this.normal.ParamTransition<float>(this.pressure, this.warningHigh, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureWarning_High).ParamTransition<float>(this.pressure, this.warningLow, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureWarning_Low).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.Normal;
			}).TriggerOnEnter(GameHashes.OptimalPressureAchieved, null);
		}

		// Token: 0x040071E3 RID: 29155
		public StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.FloatParameter pressure;

		// Token: 0x040071E4 RID: 29156
		public StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.BoolParameter safe_element;

		// Token: 0x040071E5 RID: 29157
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State unsafeElement;

		// Token: 0x040071E6 RID: 29158
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State lethalLow;

		// Token: 0x040071E7 RID: 29159
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State lethalHigh;

		// Token: 0x040071E8 RID: 29160
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State warningLow;

		// Token: 0x040071E9 RID: 29161
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State warningHigh;

		// Token: 0x040071EA RID: 29162
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State normal;
	}

	// Token: 0x02001726 RID: 5926
	public enum PressureState
	{
		// Token: 0x040071EC RID: 29164
		LethalLow,
		// Token: 0x040071ED RID: 29165
		WarningLow,
		// Token: 0x040071EE RID: 29166
		Normal,
		// Token: 0x040071EF RID: 29167
		WarningHigh,
		// Token: 0x040071F0 RID: 29168
		LethalHigh
	}
}
