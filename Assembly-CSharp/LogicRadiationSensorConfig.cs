using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000271 RID: 625
public class LogicRadiationSensorConfig : IBuildingConfig
{
	// Token: 0x06000CF1 RID: 3313 RVA: 0x0004A6F5 File Offset: 0x000488F5
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x0004A6FC File Offset: 0x000488FC
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicRadiationSensorConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "radiation_sensor_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>();
		buildingDef.LogicOutputPorts.Add(LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICRADIATIONSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICRADIATIONSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICRADIATIONSENSOR.LOGIC_PORT_INACTIVE, true, false));
		SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicRadiationSensorConfig.ID);
		return buildingDef;
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x0004A804 File Offset: 0x00048A04
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicRadiationSensor>().manuallyControlled = false;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000813 RID: 2067
	public static string ID = "LogicRadiationSensor";
}
