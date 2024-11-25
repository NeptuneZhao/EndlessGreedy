using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class LogicHEPSensorConfig : IBuildingConfig
{
	// Token: 0x06000CC5 RID: 3269 RVA: 0x00049A55 File Offset: 0x00047C55
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x00049A5C File Offset: 0x00047C5C
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicHEPSensorConfig.ID;
		int width = 1;
		int height = 1;
		string anim = LogicHEPSensorConfig.kanim;
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
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICHEPSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICHEPSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICHEPSENSOR.LOGIC_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume(LogicHEPSensorConfig.kanim, "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume(LogicHEPSensorConfig.kanim, "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicHEPSensorConfig.ID);
		return buildingDef;
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x00049B5F File Offset: 0x00047D5F
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicHEPSensor logicHEPSensor = go.AddOrGet<LogicHEPSensor>();
		logicHEPSensor.manuallyControlled = false;
		logicHEPSensor.activateOnHigherThan = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000807 RID: 2055
	public static string ID = "LogicHEPSensor";

	// Token: 0x04000808 RID: 2056
	private static readonly string kanim = "radbolt_sensor_kanim";
}
