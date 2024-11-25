using System;
using TUNING;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class CanvasTallConfig : IBuildingConfig
{
	// Token: 0x06000102 RID: 258 RVA: 0x00007ECC File Offset: 0x000060CC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CanvasTall";
		int width = 2;
		int height = 3;
		string anim = "painting_tall_off_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] construction_mass = new float[]
		{
			400f,
			1f
		};
		string[] construction_materials = new string[]
		{
			"Metal",
			"BuildingFiber"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, new EffectorValues
		{
			amount = 15,
			radius = 6
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.SceneLayer = Grid.SceneLayer.InteriorWall;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "off";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00007F92 File Offset: 0x00006192
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00007FB1 File Offset: 0x000061B1
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddComponent<Painting>().defaultAnimName = "off";
	}

	// Token: 0x040000A2 RID: 162
	public const string ID = "CanvasTall";
}
