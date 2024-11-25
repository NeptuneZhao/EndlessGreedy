using System;
using TUNING;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class CrownMouldingConfig : IBuildingConfig
{
	// Token: 0x060001D2 RID: 466 RVA: 0x0000D1FC File Offset: 0x0000B3FC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CrownMoulding";
		int width = 1;
		int height = 1;
		string anim = "crown_moulding_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 5,
			radius = 3
		}, none, 0.2f);
		buildingDef.DefaultAnimState = "S_U";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		return buildingDef;
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0000D290 File Offset: 0x0000B490
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
		go.AddOrGet<AnimTileable>();
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000D2AA File Offset: 0x0000B4AA
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000120 RID: 288
	public const string ID = "CrownMoulding";
}
