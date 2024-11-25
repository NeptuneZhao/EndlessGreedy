using System;
using TUNING;
using UnityEngine;

// Token: 0x0200006B RID: 107
public class DevHeaterConfig : IBuildingConfig
{
	// Token: 0x060001FB RID: 507 RVA: 0x0000E100 File Offset: 0x0000C300
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevHeater";
		int width = 1;
		int height = 1;
		string anim = "dev_generator_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.DebugOnly = true;
		buildingDef.Overheatable = false;
		SoundEventVolumeCache.instance.AddVolume("dev_lightgenerator_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("dev_lightgenerator_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		return buildingDef;
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0000E1B5 File Offset: 0x0000C3B5
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x060001FD RID: 509 RVA: 0x0000E1B7 File Offset: 0x0000C3B7
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0000E1C4 File Offset: 0x0000C3C4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<DirectVolumeHeater>();
	}

	// Token: 0x04000143 RID: 323
	public const string ID = "DevHeater";
}
