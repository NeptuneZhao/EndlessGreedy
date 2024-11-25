using System;
using TUNING;
using UnityEngine;

// Token: 0x02000299 RID: 665
public class MetalTileConfig : IBuildingConfig
{
	// Token: 0x06000DBE RID: 3518 RVA: 0x0004EAC4 File Offset: 0x0004CCC4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MetalTile";
		int width = 1;
		int height = 1;
		string anim = "floor_metal_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER2, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_metal");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_metal_place");
		buildingDef.BlockTileShineAtlas = Assets.GetTextureAtlas("tiles_metal_spec");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_metal_tops_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_metal_tops_decor_place_info");
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		return buildingDef;
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x0004EBC8 File Offset: 0x0004CDC8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.BONUS_3;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MetalTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x0004EC2A File Offset: 0x0004CE2A
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x0004EC43 File Offset: 0x0004CE43
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x0400089A RID: 2202
	public const string ID = "MetalTile";

	// Token: 0x0400089B RID: 2203
	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_metal_tops");
}
