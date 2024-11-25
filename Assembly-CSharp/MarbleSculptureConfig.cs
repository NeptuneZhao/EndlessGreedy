using System;
using TUNING;
using UnityEngine;

// Token: 0x02000287 RID: 647
public class MarbleSculptureConfig : IBuildingConfig
{
	// Token: 0x06000D68 RID: 3432 RVA: 0x0004C97C File Offset: 0x0004AB7C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MarbleSculpture";
		int width = 2;
		int height = 3;
		string anim = "sculpture_marble_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] precious_ROCKS = MATERIALS.PRECIOUS_ROCKS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, precious_ROCKS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 20,
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

	// Token: 0x06000D69 RID: 3433 RVA: 0x0004CA18 File Offset: 0x0004AC18
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x0004CA37 File Offset: 0x0004AC37
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x04000857 RID: 2135
	public const string ID = "MarbleSculpture";
}
