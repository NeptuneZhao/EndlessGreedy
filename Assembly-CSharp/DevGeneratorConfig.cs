﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class DevGeneratorConfig : IBuildingConfig
{
	// Token: 0x060001F2 RID: 498 RVA: 0x0000DEEC File Offset: 0x0000C0EC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevGenerator";
		int width = 1;
		int height = 1;
		string anim = "dev_generator_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 100000f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000DF8C File Offset: 0x0000C18C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		DevGenerator devGenerator = go.AddOrGet<DevGenerator>();
		devGenerator.powerDistributionOrder = 9;
		devGenerator.wattageRating = 100000f;
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000DFB1 File Offset: 0x0000C1B1
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000141 RID: 321
	public const string ID = "DevGenerator";
}
