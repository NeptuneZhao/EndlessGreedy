using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200067B RID: 1659
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/AirConditioner")]
public class AirConditioner : KMonoBehaviour, ISaveLoadable, IGameObjectEffectDescriptor, ISim200ms
{
	// Token: 0x17000207 RID: 519
	// (get) Token: 0x0600290D RID: 10509 RVA: 0x000E82D8 File Offset: 0x000E64D8
	// (set) Token: 0x0600290E RID: 10510 RVA: 0x000E82E0 File Offset: 0x000E64E0
	public float lastEnvTemp { get; private set; }

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x0600290F RID: 10511 RVA: 0x000E82E9 File Offset: 0x000E64E9
	// (set) Token: 0x06002910 RID: 10512 RVA: 0x000E82F1 File Offset: 0x000E64F1
	public float lastGasTemp { get; private set; }

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06002911 RID: 10513 RVA: 0x000E82FA File Offset: 0x000E64FA
	public float TargetTemperature
	{
		get
		{
			return this.targetTemperature;
		}
	}

	// Token: 0x06002912 RID: 10514 RVA: 0x000E8302 File Offset: 0x000E6502
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<AirConditioner>(-592767678, AirConditioner.OnOperationalChangedDelegate);
		base.Subscribe<AirConditioner>(824508782, AirConditioner.OnActiveChangedDelegate);
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x000E832C File Offset: 0x000E652C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.gameObject.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
		this.cooledAirOutputCell = this.building.GetUtilityOutputCell();
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x000E83BA File Offset: 0x000E65BA
	public void Sim200ms(float dt)
	{
		if (this.operational != null && !this.operational.IsOperational)
		{
			this.operational.SetActive(false, false);
			return;
		}
		this.UpdateState(dt);
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x000E83EC File Offset: 0x000E65EC
	private static bool UpdateStateCb(int cell, object data)
	{
		AirConditioner airConditioner = data as AirConditioner;
		airConditioner.cellCount++;
		airConditioner.envTemp += Grid.Temperature[cell];
		return true;
	}

	// Token: 0x06002916 RID: 10518 RVA: 0x000E841C File Offset: 0x000E661C
	private void UpdateState(float dt)
	{
		bool value = this.consumer.IsSatisfied;
		this.envTemp = 0f;
		this.cellCount = 0;
		if (this.occupyArea != null && base.gameObject != null)
		{
			this.occupyArea.TestArea(Grid.PosToCell(base.gameObject), this, AirConditioner.UpdateStateCbDelegate);
			this.envTemp /= (float)this.cellCount;
		}
		this.lastEnvTemp = this.envTemp;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < items.Count; i++)
		{
			PrimaryElement component = items[i].GetComponent<PrimaryElement>();
			if (component.Mass > 0f && (!this.isLiquidConditioner || !component.Element.IsGas) && (this.isLiquidConditioner || !component.Element.IsLiquid))
			{
				value = true;
				this.lastGasTemp = component.Temperature;
				float num = component.Temperature + this.temperatureDelta;
				if (num < 1f)
				{
					num = 1f;
					this.lowTempLag = Mathf.Min(this.lowTempLag + dt / 5f, 1f);
				}
				else
				{
					this.lowTempLag = Mathf.Min(this.lowTempLag - dt / 5f, 0f);
				}
				float num2 = (this.isLiquidConditioner ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow).AddElement(this.cooledAirOutputCell, component.ElementID, component.Mass, num, component.DiseaseIdx, component.DiseaseCount);
				component.KeepZeroMassObject = true;
				float num3 = num2 / component.Mass;
				int num4 = (int)((float)component.DiseaseCount * num3);
				component.Mass -= num2;
				component.ModifyDiseaseCount(-num4, "AirConditioner.UpdateState");
				float num5 = (num - component.Temperature) * component.Element.specificHeatCapacity * num2;
				float display_dt = (this.lastSampleTime > 0f) ? (Time.time - this.lastSampleTime) : 1f;
				this.lastSampleTime = Time.time;
				this.heatEffect.SetHeatBeingProducedValue(Mathf.Abs(num5));
				GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, -num5, BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, display_dt);
				break;
			}
		}
		if (Time.time - this.lastSampleTime > 2f)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, 0f, BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, Time.time - this.lastSampleTime);
			this.lastSampleTime = Time.time;
		}
		this.operational.SetActive(value, false);
		this.UpdateStatus();
	}

	// Token: 0x06002917 RID: 10519 RVA: 0x000E86D2 File Offset: 0x000E68D2
	private void OnOperationalChanged(object data)
	{
		if (this.operational.IsOperational)
		{
			this.UpdateState(0f);
		}
	}

	// Token: 0x06002918 RID: 10520 RVA: 0x000E86EC File Offset: 0x000E68EC
	private void OnActiveChanged(object data)
	{
		this.UpdateStatus();
		if (this.operational.IsActive)
		{
			this.heatEffect.enabled = true;
			return;
		}
		this.heatEffect.enabled = false;
	}

	// Token: 0x06002919 RID: 10521 RVA: 0x000E871C File Offset: 0x000E691C
	private void UpdateStatus()
	{
		if (this.operational.IsActive)
		{
			if (this.lowTempLag >= 1f && !this.showingLowTemp)
			{
				this.statusHandle = (this.isLiquidConditioner ? this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CoolingStalledColdLiquid, this) : this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CoolingStalledColdGas, this));
				this.showingLowTemp = true;
				this.showingHotEnv = false;
				return;
			}
			if (this.lowTempLag <= 0f && (this.showingHotEnv || this.showingLowTemp))
			{
				this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Cooling, null);
				this.showingLowTemp = false;
				this.showingHotEnv = false;
				return;
			}
			if (this.statusHandle == Guid.Empty)
			{
				this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Cooling, null);
				this.showingLowTemp = false;
				this.showingHotEnv = false;
				return;
			}
		}
		else
		{
			this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x0600291A RID: 10522 RVA: 0x000E8890 File Offset: 0x000E6A90
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedTemperature = GameUtil.GetFormattedTemperature(this.temperatureDelta, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
		Element element = ElementLoader.FindElementByName(this.isLiquidConditioner ? "Water" : "Oxygen");
		float num;
		if (this.isLiquidConditioner)
		{
			num = Mathf.Abs(this.temperatureDelta * element.specificHeatCapacity * 10000f);
		}
		else
		{
			num = Mathf.Abs(this.temperatureDelta * element.specificHeatCapacity * 1000f);
		}
		float dtu = num * 1f;
		Descriptor item = default(Descriptor);
		string txt = string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.HEATGENERATED_LIQUIDCONDITIONER : UI.BUILDINGEFFECTS.HEATGENERATED_AIRCONDITIONER, GameUtil.GetFormattedHeatEnergy(dtu, GameUtil.HeatEnergyFormatterUnit.Automatic), GameUtil.GetFormattedTemperature(Mathf.Abs(this.temperatureDelta), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
		string tooltip = string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_LIQUIDCONDITIONER : UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_AIRCONDITIONER, GameUtil.GetFormattedHeatEnergy(dtu, GameUtil.HeatEnergyFormatterUnit.Automatic), GameUtil.GetFormattedTemperature(Mathf.Abs(this.temperatureDelta), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
		item.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Descriptor item2 = default(Descriptor);
		item2.SetupDescriptor(string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.LIQUIDCOOLING : UI.BUILDINGEFFECTS.GASCOOLING, formattedTemperature), string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.TOOLTIPS.LIQUIDCOOLING : UI.BUILDINGEFFECTS.TOOLTIPS.GASCOOLING, formattedTemperature), Descriptor.DescriptorType.Effect);
		list.Add(item2);
		return list;
	}

	// Token: 0x0400178E RID: 6030
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x0400178F RID: 6031
	[MyCmpReq]
	protected Storage storage;

	// Token: 0x04001790 RID: 6032
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x04001791 RID: 6033
	[MyCmpReq]
	private ConduitConsumer consumer;

	// Token: 0x04001792 RID: 6034
	[MyCmpReq]
	private BuildingComplete building;

	// Token: 0x04001793 RID: 6035
	[MyCmpGet]
	private OccupyArea occupyArea;

	// Token: 0x04001794 RID: 6036
	[MyCmpGet]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x04001795 RID: 6037
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001796 RID: 6038
	public float temperatureDelta = -14f;

	// Token: 0x04001797 RID: 6039
	public float maxEnvironmentDelta = -50f;

	// Token: 0x04001798 RID: 6040
	private float lowTempLag;

	// Token: 0x04001799 RID: 6041
	private bool showingLowTemp;

	// Token: 0x0400179A RID: 6042
	public bool isLiquidConditioner;

	// Token: 0x0400179B RID: 6043
	private bool showingHotEnv;

	// Token: 0x0400179E RID: 6046
	private Guid statusHandle;

	// Token: 0x0400179F RID: 6047
	[Serialize]
	private float targetTemperature;

	// Token: 0x040017A0 RID: 6048
	private int cooledAirOutputCell = -1;

	// Token: 0x040017A1 RID: 6049
	private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<AirConditioner>(delegate(AirConditioner component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x040017A2 RID: 6050
	private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<AirConditioner>(delegate(AirConditioner component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x040017A3 RID: 6051
	private float lastSampleTime = -1f;

	// Token: 0x040017A4 RID: 6052
	private float envTemp;

	// Token: 0x040017A5 RID: 6053
	private int cellCount;

	// Token: 0x040017A6 RID: 6054
	private static readonly Func<int, object, bool> UpdateStateCbDelegate = (int cell, object data) => AirConditioner.UpdateStateCb(cell, data);
}
