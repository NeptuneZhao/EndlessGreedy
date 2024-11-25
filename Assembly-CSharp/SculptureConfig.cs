using System;
using TUNING;
using UnityEngine;

// Token: 0x0200039E RID: 926
public class SculptureConfig : IBuildingConfig
{
	// Token: 0x06001344 RID: 4932 RVA: 0x0006AD10 File Offset: 0x00068F10
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Sculpture";
		int width = 1;
		int height = 3;
		string anim = "sculpture_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 10,
			radius = 8
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x06001345 RID: 4933 RVA: 0x0006ADAC File Offset: 0x00068FAC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x0006ADCB File Offset: 0x00068FCB
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x04000B36 RID: 2870
	public const string ID = "Sculpture";
}
