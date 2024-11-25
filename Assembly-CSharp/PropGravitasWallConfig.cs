using System;
using TUNING;
using UnityEngine;

// Token: 0x02000368 RID: 872
public class PropGravitasWallConfig : IBuildingConfig
{
	// Token: 0x06001215 RID: 4629 RVA: 0x000637C0 File Offset: 0x000619C0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PropGravitasWall";
		int width = 1;
		int height = 1;
		string anim = "gravitas_walls_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.Entombable = false;
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

	// Token: 0x06001216 RID: 4630 RVA: 0x00063858 File Offset: 0x00061A58
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		go.AddComponent<ZoneTile>();
		go.GetComponent<PrimaryElement>().SetElement(SimHashes.Granite, true);
		go.GetComponent<PrimaryElement>().Temperature = 273f;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x000638BF File Offset: 0x00061ABF
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A89 RID: 2697
	public const string ID = "PropGravitasWall";
}
