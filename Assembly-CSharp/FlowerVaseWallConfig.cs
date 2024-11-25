using System;
using TUNING;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class FlowerVaseWallConfig : IBuildingConfig
{
	// Token: 0x06000831 RID: 2097 RVA: 0x00036358 File Offset: 0x00034558
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FlowerVaseWall";
		int width = 1;
		int height = 1;
		string anim = "flowervase_wall_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnWall;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x000363D4 File Offset: 0x000345D4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.AddDepositTag(GameTags.DecorSeed);
		plantablePlot.occupyingObjectVisualOffset = new Vector3(0f, -0.25f, 0f);
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x00036429 File Offset: 0x00034629
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040005C3 RID: 1475
	public const string ID = "FlowerVaseWall";
}
