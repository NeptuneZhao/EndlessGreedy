using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020001EB RID: 491
public class GasConduitRadiantConfig : IBuildingConfig
{
	// Token: 0x06000A11 RID: 2577 RVA: 0x0003B320 File Offset: 0x00039520
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasConduitRadiant";
		int width = 1;
		int height = 1;
		string anim = "utilities_gas_radiant_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.ThermalConductivity = 2f;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.ObjectLayer = ObjectLayer.GasConduit;
		buildingDef.TileLayer = ObjectLayer.GasConduitTile;
		buildingDef.ReplacementLayer = ObjectLayer.ReplacementGasConduit;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = 0f;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.SceneLayer = Grid.SceneLayer.GasConduits;
		buildingDef.isKAnimTile = true;
		buildingDef.isUtility = true;
		buildingDef.DragBuild = true;
		buildingDef.ReplacementTags = new List<Tag>();
		buildingDef.ReplacementTags.Add(GameTags.Vents);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasConduitRadiant");
		return buildingDef;
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0003B42A File Offset: 0x0003962A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		go.AddOrGet<Conduit>().type = ConduitType.Gas;
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0003B440 File Offset: 0x00039640
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired = false;
		go.AddComponent<EmptyConduitWorkable>();
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Gas;
		kanimGraphTileVisualizer.isPhysicalBuilding = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Vents, false);
		LiquidConduitConfig.CommonConduitPostConfigureComplete(go);
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x0003B499 File Offset: 0x00039699
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Gas;
		kanimGraphTileVisualizer.isPhysicalBuilding = false;
	}

	// Token: 0x04000694 RID: 1684
	public const string ID = "GasConduitRadiant";
}
