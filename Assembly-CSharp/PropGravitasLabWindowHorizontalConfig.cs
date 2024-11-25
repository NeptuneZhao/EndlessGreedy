using System;
using TUNING;
using UnityEngine;

// Token: 0x02000363 RID: 867
public class PropGravitasLabWindowHorizontalConfig : IBuildingConfig
{
	// Token: 0x060011FC RID: 4604 RVA: 0x000633BD File Offset: 0x000615BD
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x000633C4 File Offset: 0x000615C4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PropGravitasLabWindowHorizontal";
		int width = 3;
		int height = 2;
		string anim = "gravitas_lab_window_horizontal_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier_TINY = BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY;
		string[] glasses = MATERIALS.GLASSES;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier_TINY, glasses, melting_point, build_location_rule, DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.DefaultAnimState = "on";
		buildingDef.ObjectLayer = ObjectLayer.Backwall;
		buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x060011FE RID: 4606 RVA: 0x00063458 File Offset: 0x00061658
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		go.AddComponent<ZoneTile>();
		go.GetComponent<PrimaryElement>().SetElement(SimHashes.Glass, true);
		go.GetComponent<PrimaryElement>().Temperature = 273f;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x000634BF File Offset: 0x000616BF
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A88 RID: 2696
	public const string ID = "PropGravitasLabWindowHorizontal";
}
