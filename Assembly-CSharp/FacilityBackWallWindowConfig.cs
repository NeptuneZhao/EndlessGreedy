using System;
using TUNING;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class FacilityBackWallWindowConfig : IBuildingConfig
{
	// Token: 0x060002C2 RID: 706 RVA: 0x00013DA0 File Offset: 0x00011FA0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FacilityBackWallWindow";
		int width = 1;
		int height = 6;
		string anim = "gravitas_window_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] glasses = MATERIALS.GLASSES;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, glasses, melting_point, build_location_rule, DECOR.BONUS.TIER3, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.R90;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.DefaultAnimState = "off";
		buildingDef.ObjectLayer = ObjectLayer.Backwall;
		buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00013E34 File Offset: 0x00012034
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		go.AddComponent<ZoneTile>();
		go.GetComponent<PrimaryElement>().SetElement(SimHashes.Glass, true);
		go.GetComponent<PrimaryElement>().Temperature = 273f;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00013E9B File Offset: 0x0001209B
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000199 RID: 409
	public const string ID = "FacilityBackWallWindow";
}
