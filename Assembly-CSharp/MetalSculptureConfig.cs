using System;
using TUNING;
using UnityEngine;

// Token: 0x02000298 RID: 664
public class MetalSculptureConfig : IBuildingConfig
{
	// Token: 0x06000DBA RID: 3514 RVA: 0x0004E9EC File Offset: 0x0004CBEC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MetalSculpture";
		int width = 1;
		int height = 3;
		string anim = "sculpture_metal_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, new EffectorValues
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

	// Token: 0x06000DBB RID: 3515 RVA: 0x0004EA88 File Offset: 0x0004CC88
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x0004EAA7 File Offset: 0x0004CCA7
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x04000899 RID: 2201
	public const string ID = "MetalSculpture";
}
