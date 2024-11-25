using System;
using TUNING;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class CanvasConfig : IBuildingConfig
{
	// Token: 0x060000FE RID: 254 RVA: 0x00007DC4 File Offset: 0x00005FC4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Canvas";
		int width = 2;
		int height = 2;
		string anim = "painting_off_kanim";
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
			amount = 10,
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

	// Token: 0x060000FF RID: 255 RVA: 0x00007E8A File Offset: 0x0000608A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00007EA9 File Offset: 0x000060A9
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddComponent<Painting>().defaultAnimName = "off";
	}

	// Token: 0x040000A1 RID: 161
	public const string ID = "Canvas";
}
