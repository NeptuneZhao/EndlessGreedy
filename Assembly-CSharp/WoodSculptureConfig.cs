using System;
using TUNING;
using UnityEngine;

// Token: 0x02000405 RID: 1029
public class WoodSculptureConfig : IBuildingConfig
{
	// Token: 0x060015AB RID: 5547 RVA: 0x00076A68 File Offset: 0x00074C68
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x00076A70 File Offset: 0x00074C70
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WoodSculpture";
		int width = 1;
		int height = 1;
		string anim = "sculpture_wood_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] woods = MATERIALS.WOODS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, woods, melting_point, build_location_rule, new EffectorValues
		{
			amount = 4,
			radius = 4
		}, none, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.InteriorWall;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x00076B13 File Offset: 0x00074D13
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x00076B32 File Offset: 0x00074D32
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<LongRangeSculpture>().defaultAnimName = "slab";
	}

	// Token: 0x04000C42 RID: 3138
	public const string ID = "WoodSculpture";
}
