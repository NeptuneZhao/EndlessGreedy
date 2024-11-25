using System;
using TUNING;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class CarpetTileConfig : IBuildingConfig
{
	// Token: 0x06000113 RID: 275 RVA: 0x0000848C File Offset: 0x0000668C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CarpetTile";
		int width = 1;
		int height = 1;
		string anim = "floor_carpet_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] construction_mass = new float[]
		{
			200f,
			2f
		};
		string[] construction_materials = new string[]
		{
			"BuildableRaw",
			"BuildingFiber"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER3, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_carpet");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_carpet_place");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_carpet_tops_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_carpet_tops_decor_place_info");
		return buildingDef;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x000085A4 File Offset: 0x000067A4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.PENALTY_2;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = CarpetTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000085F1 File Offset: 0x000067F1
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.Carpeted, false);
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0000861B File Offset: 0x0000681B
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x040000AC RID: 172
	public const string ID = "CarpetTile";

	// Token: 0x040000AD RID: 173
	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_carpet_tops");
}
