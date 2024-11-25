using System;
using TUNING;
using UnityEngine;

// Token: 0x020003B1 RID: 945
public class SmallSculptureConfig : IBuildingConfig
{
	// Token: 0x060013A4 RID: 5028 RVA: 0x0006C594 File Offset: 0x0006A794
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SmallSculpture";
		int width = 1;
		int height = 2;
		string anim = "sculpture_1x2_kanim";
		int hitpoints = 10;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 5,
			radius = 4
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

	// Token: 0x060013A5 RID: 5029 RVA: 0x0006C62F File Offset: 0x0006A82F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x0006C64E File Offset: 0x0006A84E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x04000B45 RID: 2885
	public const string ID = "SmallSculpture";
}
