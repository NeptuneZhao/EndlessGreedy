using System;
using TUNING;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class FlowerVaseConfig : IBuildingConfig
{
	// Token: 0x06000825 RID: 2085 RVA: 0x00036094 File Offset: 0x00034294
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FlowerVase";
		int width = 1;
		int height = 1;
		string anim = "flowervase_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x00036109 File Offset: 0x00034309
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
		go.AddOrGet<PlantablePlot>().AddDepositTag(GameTags.DecorSeed);
		go.AddOrGet<FlowerVase>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x00036140 File Offset: 0x00034340
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040005C0 RID: 1472
	public const string ID = "FlowerVase";
}
