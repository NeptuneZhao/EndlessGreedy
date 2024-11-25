using System;
using TUNING;
using UnityEngine;

// Token: 0x02000334 RID: 820
public class PlanterBoxConfig : IBuildingConfig
{
	// Token: 0x06001116 RID: 4374 RVA: 0x0006027C File Offset: 0x0005E47C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PlanterBox";
		int width = 1;
		int height = 1;
		string anim = "planterbox_kanim";
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] farmable = MATERIALS.FARMABLE;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, farmable, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x000602F0 File Offset: 0x0005E4F0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.tagOnPlanted = GameTags.PlantedOnFloorVessel;
		plantablePlot.AddDepositTag(GameTags.CropSeed);
		plantablePlot.SetFertilizationFlags(true, false);
		go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
		BuildingTemplates.CreateDefaultStorage(go, false);
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<PlanterBox>();
		go.AddOrGet<AnimTileable>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06001118 RID: 4376 RVA: 0x00060363 File Offset: 0x0005E563
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A6C RID: 2668
	public const string ID = "PlanterBox";
}
