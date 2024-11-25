using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200020A RID: 522
public class HEPBatteryConfig : IBuildingConfig
{
	// Token: 0x06000AC3 RID: 2755 RVA: 0x00040323 File Offset: 0x0003E523
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x0004032C File Offset: 0x0003E52C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HEPBattery";
		int width = 3;
		int height = 3;
		string anim = "radbolt_battery_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.UseHighEnergyParticleInputPort = true;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 1);
		buildingDef.UseHighEnergyParticleOutputPort = true;
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 2);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.AddLogicPowerPort = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "HEPBattery");
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("HEP_STORAGE", new CellOffset(1, 1), STRINGS.BUILDINGS.PREFABS.HEPBATTERY.LOGIC_PORT_STORAGE, STRINGS.BUILDINGS.PREFABS.HEPBATTERY.LOGIC_PORT_STORAGE_ACTIVE, STRINGS.BUILDINGS.PREFABS.HEPBATTERY.LOGIC_PORT_STORAGE_INACTIVE, false, false)
		};
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(HEPBattery.FIRE_PORT_ID, new CellOffset(0, 2), STRINGS.BUILDINGS.PREFABS.HEPBATTERY.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.HEPBATTERY.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.HEPBATTERY.LOGIC_PORT_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x000404A0 File Offset: 0x0003E6A0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
		highEnergyParticleStorage.capacity = 1000f;
		highEnergyParticleStorage.autoStore = true;
		highEnergyParticleStorage.PORT_ID = "HEP_STORAGE";
		highEnergyParticleStorage.showCapacityStatusItem = true;
		highEnergyParticleStorage.showCapacityAsMainStatus = true;
		go.AddOrGet<LoopingSounds>();
		HEPBattery.Def def = go.AddOrGetDef<HEPBattery.Def>();
		def.minLaunchInterval = 1f;
		def.minSlider = 0f;
		def.maxSlider = 100f;
		def.particleDecayRate = 0.5f;
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x0004051B File Offset: 0x0003E71B
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000715 RID: 1813
	public const string ID = "HEPBattery";

	// Token: 0x04000716 RID: 1814
	public const float MIN_LAUNCH_INTERVAL = 1f;

	// Token: 0x04000717 RID: 1815
	public const int MIN_SLIDER = 0;

	// Token: 0x04000718 RID: 1816
	public const int MAX_SLIDER = 100;

	// Token: 0x04000719 RID: 1817
	public const float HEP_CAPACITY = 1000f;

	// Token: 0x0400071A RID: 1818
	public const float DISABLED_DECAY_RATE = 0.5f;

	// Token: 0x0400071B RID: 1819
	public const string STORAGE_PORT_ID = "HEP_STORAGE";

	// Token: 0x0400071C RID: 1820
	public const string FIRE_PORT_ID = "HEP_FIRE";
}
