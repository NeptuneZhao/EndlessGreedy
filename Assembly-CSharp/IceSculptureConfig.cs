using System;
using TUNING;
using UnityEngine;

// Token: 0x0200021C RID: 540
public class IceSculptureConfig : IBuildingConfig
{
	// Token: 0x06000B26 RID: 2854 RVA: 0x00042B74 File Offset: 0x00040D74
	public override BuildingDef CreateBuildingDef()
	{
		string id = "IceSculpture";
		int width = 2;
		int height = 2;
		string anim = "icesculpture_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] construction_materials = new string[]
		{
			"Ice"
		};
		float melting_point = 273.15f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, new EffectorValues
		{
			amount = 35,
			radius = 8
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.Temperature = 253.15f;
		return buildingDef;
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x00042C24 File Offset: 0x00040E24
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x00042C43 File Offset: 0x00040E43
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x0400076B RID: 1899
	public const string ID = "IceSculpture";
}
