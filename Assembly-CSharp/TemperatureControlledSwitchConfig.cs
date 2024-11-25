using System;
using TUNING;
using UnityEngine;

// Token: 0x020003E5 RID: 997
public class TemperatureControlledSwitchConfig : IBuildingConfig
{
	// Token: 0x060014D8 RID: 5336 RVA: 0x000730B8 File Offset: 0x000712B8
	public override BuildingDef CreateBuildingDef()
	{
		string id = TemperatureControlledSwitchConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "switchthermal_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Deprecated = true;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		return buildingDef;
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x00073163 File Offset: 0x00071363
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		TemperatureControlledSwitch temperatureControlledSwitch = go.AddOrGet<TemperatureControlledSwitch>();
		temperatureControlledSwitch.objectLayer = ObjectLayer.Wire;
		temperatureControlledSwitch.manuallyControlled = false;
		temperatureControlledSwitch.minTemp = 0f;
		temperatureControlledSwitch.maxTemp = 573.15f;
	}

	// Token: 0x060014DA RID: 5338 RVA: 0x00073195 File Offset: 0x00071395
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<BuildingCellVisualizer>();
	}

	// Token: 0x04000BE4 RID: 3044
	public static string ID = "TemperatureControlledSwitch";
}
