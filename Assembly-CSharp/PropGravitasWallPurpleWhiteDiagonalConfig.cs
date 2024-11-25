using System;
using TUNING;
using UnityEngine;

// Token: 0x0200036A RID: 874
public class PropGravitasWallPurpleWhiteDiagonalConfig : IBuildingConfig
{
	// Token: 0x0600121D RID: 4637 RVA: 0x000639D8 File Offset: 0x00061BD8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PropGravitasWallPurpleWhiteDiagonal";
		int width = 1;
		int height = 1;
		string anim = "walls_diagonal_gravitas_purple_white_kanim";
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

	// Token: 0x0600121E RID: 4638 RVA: 0x00063A70 File Offset: 0x00061C70
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		go.AddComponent<ZoneTile>();
		go.GetComponent<PrimaryElement>().SetElement(SimHashes.Granite, true);
		go.GetComponent<PrimaryElement>().Temperature = 273f;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x0600121F RID: 4639 RVA: 0x00063AD7 File Offset: 0x00061CD7
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A8B RID: 2699
	public const string ID = "PropGravitasWallPurpleWhiteDiagonal";
}
