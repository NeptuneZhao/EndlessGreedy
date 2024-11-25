﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x0200038B RID: 907
public class RocketEnvelopeWindowTileConfig : IBuildingConfig
{
	// Token: 0x060012D9 RID: 4825 RVA: 0x00068D80 File Offset: 0x00066F80
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketEnvelopeWindowTile";
		int width = 1;
		int height = 1;
		string anim = "floor_glass_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] transparents = MATERIALS.TRANSPARENTS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, transparents, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.Replaceable = false;
		buildingDef.Invincible = true;
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.GlassTile;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileIsTransparent = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_glass");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_glass_place");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_place_info");
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		return buildingDef;
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x00068E9C File Offset: 0x0006709C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.setTransparent = true;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = RocketEnvelopeWindowTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Window, false);
	}

	// Token: 0x060012DB RID: 4827 RVA: 0x00068F0B File Offset: 0x0006710B
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Bunker, false);
		component.AddTag(GameTags.FloorTiles, false);
		component.AddTag(GameTags.NoRocketRefund, false);
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
	}

	// Token: 0x060012DC RID: 4828 RVA: 0x00068F48 File Offset: 0x00067148
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x04000B02 RID: 2818
	public const string ID = "RocketEnvelopeWindowTile";

	// Token: 0x04000B03 RID: 2819
	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_glass_tops");
}
