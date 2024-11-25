using System;
using TUNING;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class DevRadiationGeneratorConfig : IBuildingConfig
{
	// Token: 0x06000215 RID: 533 RVA: 0x0000E900 File Offset: 0x0000CB00
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevRadiationGenerator";
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
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000E97C File Offset: 0x0000CB7C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		RadiationEmitter radiationEmitter = go.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 12;
		radiationEmitter.emitRadiusY = 12;
		radiationEmitter.emitRads = 2400f / ((float)radiationEmitter.emitRadiusX / 6f);
		go.AddOrGet<DevRadiationEmitter>();
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000E9D9 File Offset: 0x0000CBD9
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000153 RID: 339
	public const string ID = "DevRadiationGenerator";
}
