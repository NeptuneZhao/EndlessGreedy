using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000825 RID: 2085
[SkipSaveFileSerialization]
public class TemperatureVulnerable : StateMachineComponent<TemperatureVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x060039A5 RID: 14757 RVA: 0x0013A348 File Offset: 0x00138548
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

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x060039A6 RID: 14758 RVA: 0x0013A36A File Offset: 0x0013856A
	public float TemperatureLethalLow
	{
		get
		{
			return this.internalTemperatureLethal_Low;
		}
	}

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x060039A7 RID: 14759 RVA: 0x0013A372 File Offset: 0x00138572
	public float TemperatureLethalHigh
	{
		get
		{
			return this.internalTemperatureLethal_High;
		}
	}

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x060039A8 RID: 14760 RVA: 0x0013A37A File Offset: 0x0013857A
	public float TemperatureWarningLow
	{
		get
		{
			if (this.wiltTempRangeModAttribute != null)
			{
				return this.internalTemperatureWarning_Low + (1f - this.wiltTempRangeModAttribute.GetTotalValue()) * this.temperatureRangeModScalar;
			}
			return this.internalTemperatureWarning_Low;
		}
	}

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x060039A9 RID: 14761 RVA: 0x0013A3AA File Offset: 0x001385AA
	public float TemperatureWarningHigh
	{
		get
		{
			if (this.wiltTempRangeModAttribute != null)
			{
				return this.internalTemperatureWarning_High - (1f - this.wiltTempRangeModAttribute.GetTotalValue()) * this.temperatureRangeModScalar;
			}
			return this.internalTemperatureWarning_High;
		}
	}

	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x060039AA RID: 14762 RVA: 0x0013A3DA File Offset: 0x001385DA
	public float InternalTemperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x060039AB RID: 14763 RVA: 0x0013A3E7 File Offset: 0x001385E7
	public TemperatureVulnerable.TemperatureState GetInternalTemperatureState
	{
		get
		{
			return this.internalTemperatureState;
		}
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x060039AC RID: 14764 RVA: 0x0013A3EF File Offset: 0x001385EF
	public bool IsLethal
	{
		get
		{
			return this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalHot || this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalCold;
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x060039AD RID: 14765 RVA: 0x0013A405 File Offset: 0x00138605
	public bool IsNormal
	{
		get
		{
			return this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.Normal;
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x060039AE RID: 14766 RVA: 0x0013A410 File Offset: 0x00138610
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[1];
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x060039AF RID: 14767 RVA: 0x0013A418 File Offset: 0x00138618
	public string WiltStateString
	{
		get
		{
			if (base.smi.IsInsideState(base.smi.sm.warningCold))
			{
				return Db.Get().CreatureStatusItems.Cold_Crop.resolveStringCallback(CREATURES.STATUSITEMS.COLD_CROP.NAME, this);
			}
			if (base.smi.IsInsideState(base.smi.sm.warningHot))
			{
				return Db.Get().CreatureStatusItems.Hot_Crop.resolveStringCallback(CREATURES.STATUSITEMS.HOT_CROP.NAME, this);
			}
			return "";
		}
	}

	// Token: 0x060039B0 RID: 14768 RVA: 0x0013A4B0 File Offset: 0x001386B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Amounts amounts = base.gameObject.GetAmounts();
		this.displayTemperatureAmount = amounts.Add(new AmountInstance(Db.Get().Amounts.Temperature, base.gameObject));
	}

	// Token: 0x060039B1 RID: 14769 RVA: 0x0013A4F8 File Offset: 0x001386F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.wiltTempRangeModAttribute = this.GetAttributes().Get(Db.Get().PlantAttributes.WiltTempRangeMod);
		this.temperatureRangeModScalar = (this.internalTemperatureWarning_High - this.internalTemperatureWarning_Low) / 2f;
		SlicedUpdaterSim1000ms<TemperatureVulnerable>.instance.RegisterUpdate1000ms(this);
		base.smi.sm.internalTemp.Set(this.primaryElement.Temperature, base.smi, false);
		base.smi.StartSM();
	}

	// Token: 0x060039B2 RID: 14770 RVA: 0x0013A582 File Offset: 0x00138782
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SlicedUpdaterSim1000ms<TemperatureVulnerable>.instance.UnregisterUpdate1000ms(this);
	}

	// Token: 0x060039B3 RID: 14771 RVA: 0x0013A595 File Offset: 0x00138795
	public void Configure(float tempWarningLow, float tempLethalLow, float tempWarningHigh, float tempLethalHigh)
	{
		this.internalTemperatureWarning_Low = tempWarningLow;
		this.internalTemperatureLethal_Low = tempLethalLow;
		this.internalTemperatureLethal_High = tempLethalHigh;
		this.internalTemperatureWarning_High = tempWarningHigh;
	}

	// Token: 0x060039B4 RID: 14772 RVA: 0x0013A5B4 File Offset: 0x001387B4
	public bool IsCellSafe(int cell)
	{
		float averageTemperature = this.GetAverageTemperature(cell);
		return averageTemperature > -1f && averageTemperature > this.TemperatureLethalLow && averageTemperature < this.internalTemperatureLethal_High;
	}

	// Token: 0x060039B5 RID: 14773 RVA: 0x0013A5E8 File Offset: 0x001387E8
	public void SlicedSim1000ms(float dt)
	{
		if (!Grid.IsValidCell(Grid.PosToCell(base.gameObject)))
		{
			return;
		}
		base.smi.sm.internalTemp.Set(this.InternalTemperature, base.smi, false);
		this.displayTemperatureAmount.value = this.InternalTemperature;
	}

	// Token: 0x060039B6 RID: 14774 RVA: 0x0013A63C File Offset: 0x0013883C
	private static bool GetAverageTemperatureCb(int cell, object data)
	{
		TemperatureVulnerable temperatureVulnerable = data as TemperatureVulnerable;
		if (Grid.Mass[cell] > 0.1f)
		{
			temperatureVulnerable.averageTemp += Grid.Temperature[cell];
			temperatureVulnerable.cellCount++;
		}
		return true;
	}

	// Token: 0x060039B7 RID: 14775 RVA: 0x0013A68C File Offset: 0x0013888C
	private float GetAverageTemperature(int cell)
	{
		this.averageTemp = 0f;
		this.cellCount = 0;
		this.occupyArea.TestArea(cell, this, TemperatureVulnerable.GetAverageTemperatureCbDelegate);
		if (this.cellCount > 0)
		{
			return this.averageTemp / (float)this.cellCount;
		}
		return -1f;
	}

	// Token: 0x060039B8 RID: 14776 RVA: 0x0013A6DC File Offset: 0x001388DC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		float num = (this.internalTemperatureWarning_High - this.internalTemperatureWarning_Low) / 2f;
		float temp = (this.wiltTempRangeModAttribute != null) ? this.TemperatureWarningLow : (this.internalTemperatureWarning_Low + (1f - base.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.WiltTempRangeMod)) * num);
		float temp2 = (this.wiltTempRangeModAttribute != null) ? this.TemperatureWarningHigh : (this.internalTemperatureWarning_High - (1f - base.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.WiltTempRangeMod)) * num);
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_TEMPERATURE, GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false), GameUtil.GetFormattedTemperature(temp2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_TEMPERATURE, GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false), GameUtil.GetFormattedTemperature(temp2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x040022AC RID: 8876
	private OccupyArea _occupyArea;

	// Token: 0x040022AD RID: 8877
	[SerializeField]
	private float internalTemperatureLethal_Low;

	// Token: 0x040022AE RID: 8878
	[SerializeField]
	private float internalTemperatureWarning_Low;

	// Token: 0x040022AF RID: 8879
	[SerializeField]
	private float internalTemperatureWarning_High;

	// Token: 0x040022B0 RID: 8880
	[SerializeField]
	private float internalTemperatureLethal_High;

	// Token: 0x040022B1 RID: 8881
	private AttributeInstance wiltTempRangeModAttribute;

	// Token: 0x040022B2 RID: 8882
	private float temperatureRangeModScalar;

	// Token: 0x040022B3 RID: 8883
	private const float minimumMassForReading = 0.1f;

	// Token: 0x040022B4 RID: 8884
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x040022B5 RID: 8885
	[MyCmpReq]
	private SimTemperatureTransfer temperatureTransfer;

	// Token: 0x040022B6 RID: 8886
	private AmountInstance displayTemperatureAmount;

	// Token: 0x040022B7 RID: 8887
	private TemperatureVulnerable.TemperatureState internalTemperatureState = TemperatureVulnerable.TemperatureState.Normal;

	// Token: 0x040022B8 RID: 8888
	private float averageTemp;

	// Token: 0x040022B9 RID: 8889
	private int cellCount;

	// Token: 0x040022BA RID: 8890
	private static readonly Func<int, object, bool> GetAverageTemperatureCbDelegate = (int cell, object data) => TemperatureVulnerable.GetAverageTemperatureCb(cell, data);

	// Token: 0x0200173F RID: 5951
	public class StatesInstance : GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.GameInstance
	{
		// Token: 0x0600950C RID: 38156 RVA: 0x0035E981 File Offset: 0x0035CB81
		public StatesInstance(TemperatureVulnerable master) : base(master)
		{
		}
	}

	// Token: 0x02001740 RID: 5952
	public class States : GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable>
	{
		// Token: 0x0600950D RID: 38157 RVA: 0x0035E98C File Offset: 0x0035CB8C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal;
			this.lethalCold.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.LethalCold;
			}).TriggerOnEnter(GameHashes.TooColdFatal, null).ParamTransition<float>(this.internalTemp, this.warningCold, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureLethalLow).Enter(new StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback(TemperatureVulnerable.States.Kill));
			this.lethalHot.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.LethalHot;
			}).TriggerOnEnter(GameHashes.TooHotFatal, null).ParamTransition<float>(this.internalTemp, this.warningHot, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureLethalHigh).Enter(new StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback(TemperatureVulnerable.States.Kill));
			this.warningCold.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.WarningCold;
			}).TriggerOnEnter(GameHashes.TooColdWarning, null).ParamTransition<float>(this.internalTemp, this.lethalCold, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureLethalLow).ParamTransition<float>(this.internalTemp, this.normal, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureWarningLow);
			this.warningHot.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.WarningHot;
			}).TriggerOnEnter(GameHashes.TooHotWarning, null).ParamTransition<float>(this.internalTemp, this.lethalHot, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureLethalHigh).ParamTransition<float>(this.internalTemp, this.normal, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureWarningHigh);
			this.normal.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.Normal;
			}).TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, null).ParamTransition<float>(this.internalTemp, this.warningHot, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureWarningHigh).ParamTransition<float>(this.internalTemp, this.warningCold, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureWarningLow);
		}

		// Token: 0x0600950E RID: 38158 RVA: 0x0035EC54 File Offset: 0x0035CE54
		private static void Kill(StateMachine.Instance smi)
		{
			DeathMonitor.Instance smi2 = smi.GetSMI<DeathMonitor.Instance>();
			if (smi2 != null)
			{
				smi2.Kill(Db.Get().Deaths.Generic);
			}
		}

		// Token: 0x04007226 RID: 29222
		public StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.FloatParameter internalTemp;

		// Token: 0x04007227 RID: 29223
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State lethalCold;

		// Token: 0x04007228 RID: 29224
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State lethalHot;

		// Token: 0x04007229 RID: 29225
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State warningCold;

		// Token: 0x0400722A RID: 29226
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State warningHot;

		// Token: 0x0400722B RID: 29227
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State normal;
	}

	// Token: 0x02001741 RID: 5953
	public enum TemperatureState
	{
		// Token: 0x0400722D RID: 29229
		LethalCold,
		// Token: 0x0400722E RID: 29230
		WarningCold,
		// Token: 0x0400722F RID: 29231
		Normal,
		// Token: 0x04007230 RID: 29232
		WarningHot,
		// Token: 0x04007231 RID: 29233
		LethalHot
	}
}
