using System;
using TUNING;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class BottleEmptierConfig : IBuildingConfig
{
	// Token: 0x060000CA RID: 202 RVA: 0x000069A4 File Offset: 0x00004BA4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BottleEmptier";
		int width = 1;
		int height = 3;
		string anim = "liquidator_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00006A0C File Offset: 0x00004C0C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.storageFilters = STORAGEFILTERS.LIQUIDS;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.capacityKg = 200f;
		storage.gunTargetOffset = new Vector2(0f, 2f);
		go.AddOrGet<TreeFilterable>();
		go.AddOrGet<BottleEmptier>();
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00006A6B File Offset: 0x00004C6B
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000089 RID: 137
	public const string ID = "BottleEmptier";
}
