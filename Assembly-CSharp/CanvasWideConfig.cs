using System;
using TUNING;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class CanvasWideConfig : IBuildingConfig
{
	// Token: 0x06000106 RID: 262 RVA: 0x00007FD4 File Offset: 0x000061D4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CanvasWide";
		int width = 3;
		int height = 2;
		string anim = "painting_wide_off_kanim";
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

	// Token: 0x06000107 RID: 263 RVA: 0x0000809A File Offset: 0x0000629A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000108 RID: 264 RVA: 0x000080B9 File Offset: 0x000062B9
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddComponent<Painting>().defaultAnimName = "off";
	}

	// Token: 0x040000A3 RID: 163
	public const string ID = "CanvasWide";
}
