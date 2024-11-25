using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200076F RID: 1903
[SerializationConfig(MemberSerialization.OptIn)]
public class SpaceHeater : StateMachineComponent<SpaceHeater.StatesInstance>, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
{
	// Token: 0x17000360 RID: 864
	// (get) Token: 0x06003346 RID: 13126 RVA: 0x0011961E File Offset: 0x0011781E
	public float TargetTemperature
	{
		get
		{
			return this.targetTemperature;
		}
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x06003347 RID: 13127 RVA: 0x00119626 File Offset: 0x00117826
	public float MaxPower
	{
		get
		{
			return 240f;
		}
	}

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x06003348 RID: 13128 RVA: 0x0011962D File Offset: 0x0011782D
	public float MinPower
	{
		get
		{
			return 120f;
		}
	}

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x06003349 RID: 13129 RVA: 0x00119634 File Offset: 0x00117834
	public float MaxSelfHeatKWs
	{
		get
		{
			return 32f;
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x0600334A RID: 13130 RVA: 0x0011963B File Offset: 0x0011783B
	public float MinSelfHeatKWs
	{
		get
		{
			return 16f;
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x0600334B RID: 13131 RVA: 0x00119642 File Offset: 0x00117842
	public float MaxExhaustedKWs
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x0600334C RID: 13132 RVA: 0x00119649 File Offset: 0x00117849
	public float MinExhaustedKWs
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x0600334D RID: 13133 RVA: 0x00119650 File Offset: 0x00117850
	public float CurrentSelfHeatKW
	{
		get
		{
			return Mathf.Lerp(this.MinSelfHeatKWs, this.MaxSelfHeatKWs, this.UserSliderSetting);
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x0600334E RID: 13134 RVA: 0x00119669 File Offset: 0x00117869
	public float CurrentExhaustedKW
	{
		get
		{
			return Mathf.Lerp(this.MinExhaustedKWs, this.MaxExhaustedKWs, this.UserSliderSetting);
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x0600334F RID: 13135 RVA: 0x00119682 File Offset: 0x00117882
	public float CurrentPowerConsumption
	{
		get
		{
			return Mathf.Lerp(this.MinPower, this.MaxPower, this.UserSliderSetting);
		}
	}

	// Token: 0x06003350 RID: 13136 RVA: 0x0011969B File Offset: 0x0011789B
	public static void GenerateHeat(SpaceHeater.StatesInstance smi, float dt)
	{
		if (smi.master.produceHeat)
		{
			SpaceHeater.AddExhaustHeat(smi, dt);
			SpaceHeater.AddSelfHeat(smi, dt);
		}
	}

	// Token: 0x06003351 RID: 13137 RVA: 0x001196BC File Offset: 0x001178BC
	private static float AddExhaustHeat(SpaceHeater.StatesInstance smi, float dt)
	{
		float currentExhaustedKW = smi.master.CurrentExhaustedKW;
		StructureTemperatureComponents.ExhaustHeat(smi.master.extents, currentExhaustedKW, smi.master.overheatTemperature, dt);
		return currentExhaustedKW;
	}

	// Token: 0x06003352 RID: 13138 RVA: 0x001196F4 File Offset: 0x001178F4
	public static void RefreshHeatEffect(SpaceHeater.StatesInstance smi)
	{
		if (smi.master.heatEffect != null && smi.master.produceHeat)
		{
			float heatBeingProducedValue = smi.IsInsideState(smi.sm.online.heating) ? (smi.master.CurrentExhaustedKW + smi.master.CurrentSelfHeatKW) : 0f;
			smi.master.heatEffect.SetHeatBeingProducedValue(heatBeingProducedValue);
		}
	}

	// Token: 0x06003353 RID: 13139 RVA: 0x0011976C File Offset: 0x0011796C
	private static float AddSelfHeat(SpaceHeater.StatesInstance smi, float dt)
	{
		float currentSelfHeatKW = smi.master.CurrentSelfHeatKW;
		GameComps.StructureTemperatures.ProduceEnergy(smi.master.structureTemperature, currentSelfHeatKW * dt, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, dt);
		return currentSelfHeatKW;
	}

	// Token: 0x06003354 RID: 13140 RVA: 0x001197AC File Offset: 0x001179AC
	public void SetUserSpecifiedPowerConsumptionValue(float value)
	{
		if (this.produceHeat)
		{
			this.UserSliderSetting = (value - this.MinPower) / (this.MaxPower - this.MinPower);
			SpaceHeater.RefreshHeatEffect(base.smi);
			this.energyConsumer.BaseWattageRating = this.CurrentPowerConsumption;
		}
	}

	// Token: 0x06003355 RID: 13141 RVA: 0x001197FC File Offset: 0x001179FC
	protected override void OnPrefabInit()
	{
		if (this.produceHeat)
		{
			this.heatStatusItem = new StatusItem("OperatingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.heatStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance)data;
				float num = statesInstance.master.CurrentSelfHeatKW + statesInstance.master.CurrentExhaustedKW;
				str = string.Format(str, GameUtil.GetFormattedHeatEnergy(num * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
				return str;
			};
			this.heatStatusItem.resolveTooltipCallback = delegate(string str, object data)
			{
				SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance)data;
				float num = statesInstance.master.CurrentSelfHeatKW + statesInstance.master.CurrentExhaustedKW;
				str = str.Replace("{0}", GameUtil.GetFormattedHeatEnergy(num * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
				string text = string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, BUILDING.STATUSITEMS.OPERATINGENERGY.OPERATING, GameUtil.GetFormattedHeatEnergy(statesInstance.master.CurrentSelfHeatKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
				text += string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, BUILDING.STATUSITEMS.OPERATINGENERGY.EXHAUSTING, GameUtil.GetFormattedHeatEnergy(statesInstance.master.CurrentExhaustedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
				str = str.Replace("{1}", text);
				return str;
			};
		}
		base.OnPrefabInit();
	}

	// Token: 0x06003356 RID: 13142 RVA: 0x00119894 File Offset: 0x00117A94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		this.extents = base.GetComponent<OccupyArea>().GetExtents();
		this.overheatTemperature = base.GetComponent<BuildingComplete>().Def.OverheatTemperature;
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.smi.StartSM();
		this.SetUserSpecifiedPowerConsumptionValue(this.CurrentPowerConsumption);
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x00119931 File Offset: 0x00117B31
	public void SetLiquidHeater()
	{
		this.heatLiquid = true;
	}

	// Token: 0x06003358 RID: 13144 RVA: 0x0011993C File Offset: 0x00117B3C
	private SpaceHeater.MonitorState MonitorHeating(float dt)
	{
		this.monitorCells.Clear();
		GameUtil.GetNonSolidCells(Grid.PosToCell(base.transform.GetPosition()), this.radius, this.monitorCells);
		int num = 0;
		float num2 = 0f;
		for (int i = 0; i < this.monitorCells.Count; i++)
		{
			if (Grid.Mass[this.monitorCells[i]] > this.minimumCellMass && ((Grid.Element[this.monitorCells[i]].IsGas && !this.heatLiquid) || (Grid.Element[this.monitorCells[i]].IsLiquid && this.heatLiquid)))
			{
				num++;
				num2 += Grid.Temperature[this.monitorCells[i]];
			}
		}
		if (num == 0)
		{
			if (!this.heatLiquid)
			{
				return SpaceHeater.MonitorState.NotEnoughGas;
			}
			return SpaceHeater.MonitorState.NotEnoughLiquid;
		}
		else
		{
			if (num2 / (float)num >= this.targetTemperature)
			{
				return SpaceHeater.MonitorState.TooHot;
			}
			return SpaceHeater.MonitorState.ReadyToHeat;
		}
	}

	// Token: 0x06003359 RID: 13145 RVA: 0x00119A3C File Offset: 0x00117C3C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATER_TARGETTEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATER_TARGETTEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x0600335A RID: 13146 RVA: 0x00119AA1 File Offset: 0x00117CA1
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TITLE";
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x0600335B RID: 13147 RVA: 0x00119AA8 File Offset: 0x00117CA8
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.ELECTRICAL.WATT;
		}
	}

	// Token: 0x0600335C RID: 13148 RVA: 0x00119AB4 File Offset: 0x00117CB4
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x0600335D RID: 13149 RVA: 0x00119AB7 File Offset: 0x00117CB7
	public float GetSliderMin(int index)
	{
		if (!this.produceHeat)
		{
			return 0f;
		}
		return this.MinPower;
	}

	// Token: 0x0600335E RID: 13150 RVA: 0x00119ACD File Offset: 0x00117CCD
	public float GetSliderMax(int index)
	{
		if (!this.produceHeat)
		{
			return 0f;
		}
		return this.MaxPower;
	}

	// Token: 0x0600335F RID: 13151 RVA: 0x00119AE3 File Offset: 0x00117CE3
	public float GetSliderValue(int index)
	{
		return this.CurrentPowerConsumption;
	}

	// Token: 0x06003360 RID: 13152 RVA: 0x00119AEB File Offset: 0x00117CEB
	public void SetSliderValue(float value, int index)
	{
		this.SetUserSpecifiedPowerConsumptionValue(value);
	}

	// Token: 0x06003361 RID: 13153 RVA: 0x00119AF4 File Offset: 0x00117CF4
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TOOLTIP";
	}

	// Token: 0x06003362 RID: 13154 RVA: 0x00119AFB File Offset: 0x00117CFB
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TOOLTIP"), GameUtil.GetFormattedHeatEnergyRate((this.CurrentSelfHeatKW + this.CurrentExhaustedKW) * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
	}

	// Token: 0x04001E50 RID: 7760
	public float targetTemperature = 308.15f;

	// Token: 0x04001E51 RID: 7761
	public float minimumCellMass;

	// Token: 0x04001E52 RID: 7762
	public int radius = 2;

	// Token: 0x04001E53 RID: 7763
	[SerializeField]
	private bool heatLiquid;

	// Token: 0x04001E54 RID: 7764
	[Serialize]
	public float UserSliderSetting;

	// Token: 0x04001E55 RID: 7765
	public bool produceHeat;

	// Token: 0x04001E56 RID: 7766
	private StatusItem heatStatusItem;

	// Token: 0x04001E57 RID: 7767
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001E58 RID: 7768
	private Extents extents;

	// Token: 0x04001E59 RID: 7769
	private float overheatTemperature;

	// Token: 0x04001E5A RID: 7770
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001E5B RID: 7771
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04001E5C RID: 7772
	[MyCmpGet]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x04001E5D RID: 7773
	[MyCmpGet]
	private EnergyConsumer energyConsumer;

	// Token: 0x04001E5E RID: 7774
	private List<int> monitorCells = new List<int>();

	// Token: 0x02001607 RID: 5639
	public class StatesInstance : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.GameInstance
	{
		// Token: 0x060090A5 RID: 37029 RVA: 0x0034C591 File Offset: 0x0034A791
		public StatesInstance(SpaceHeater master) : base(master)
		{
		}
	}

	// Token: 0x02001608 RID: 5640
	public class States : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater>
	{
		// Token: 0x060090A6 RID: 37030 RVA: 0x0034C59C File Offset: 0x0034A79C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.offline;
			base.serializable = StateMachine.SerializeType.Never;
			this.statusItemUnderMassLiquid = new StatusItem("statusItemUnderMassLiquid", BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItemUnderMassGas = new StatusItem("statusItemUnderMassGas", BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItemOverTemp = new StatusItem("statusItemOverTemp", BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItemOverTemp.resolveStringCallback = delegate(string str, object obj)
			{
				SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance)obj;
				return string.Format(str, GameUtil.GetFormattedTemperature(statesInstance.master.TargetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
			this.offline.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect)).EventTransition(GameHashes.OperationalChanged, this.online, (SpaceHeater.StatesInstance smi) => smi.master.operational.IsOperational);
			this.online.EventTransition(GameHashes.OperationalChanged, this.offline, (SpaceHeater.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.online.heating).Update("spaceheater_online", delegate(SpaceHeater.StatesInstance smi, float dt)
			{
				switch (smi.master.MonitorHeating(dt))
				{
				case SpaceHeater.MonitorState.ReadyToHeat:
					smi.GoTo(this.online.heating);
					return;
				case SpaceHeater.MonitorState.TooHot:
					smi.GoTo(this.online.overtemp);
					return;
				case SpaceHeater.MonitorState.NotEnoughLiquid:
					smi.GoTo(this.online.undermassliquid);
					return;
				case SpaceHeater.MonitorState.NotEnoughGas:
					smi.GoTo(this.online.undermassgas);
					return;
				default:
					return;
				}
			}, UpdateRate.SIM_4000ms, false);
			this.online.heating.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect)).Enter(delegate(SpaceHeater.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).ToggleStatusItem((SpaceHeater.StatesInstance smi) => smi.master.heatStatusItem, (SpaceHeater.StatesInstance smi) => smi).Update(new Action<SpaceHeater.StatesInstance, float>(SpaceHeater.GenerateHeat), UpdateRate.SIM_200ms, false).Exit(delegate(SpaceHeater.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Exit(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect));
			this.online.undermassliquid.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassLiquid, null);
			this.online.undermassgas.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassGas, null);
			this.online.overtemp.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemOverTemp, null);
		}

		// Token: 0x04006E68 RID: 28264
		public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State offline;

		// Token: 0x04006E69 RID: 28265
		public SpaceHeater.States.OnlineStates online;

		// Token: 0x04006E6A RID: 28266
		private StatusItem statusItemUnderMassLiquid;

		// Token: 0x04006E6B RID: 28267
		private StatusItem statusItemUnderMassGas;

		// Token: 0x04006E6C RID: 28268
		private StatusItem statusItemOverTemp;

		// Token: 0x02002538 RID: 9528
		public class OnlineStates : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State
		{
			// Token: 0x0400A5BF RID: 42431
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State heating;

			// Token: 0x0400A5C0 RID: 42432
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State overtemp;

			// Token: 0x0400A5C1 RID: 42433
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State undermassliquid;

			// Token: 0x0400A5C2 RID: 42434
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State undermassgas;
		}
	}

	// Token: 0x02001609 RID: 5641
	private enum MonitorState
	{
		// Token: 0x04006E6E RID: 28270
		ReadyToHeat,
		// Token: 0x04006E6F RID: 28271
		TooHot,
		// Token: 0x04006E70 RID: 28272
		NotEnoughLiquid,
		// Token: 0x04006E71 RID: 28273
		NotEnoughGas
	}
}
