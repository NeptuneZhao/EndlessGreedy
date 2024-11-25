using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032F RID: 815
public class PixelPackConfig : IBuildingConfig
{
	// Token: 0x060010FF RID: 4351 RVA: 0x0005FD4C File Offset: 0x0005DF4C
	public override BuildingDef CreateBuildingDef()
	{
		string id = PixelPackConfig.ID;
		int width = 4;
		int height = 1;
		string anim = "pixel_pack_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0]
		};
		string[] construction_materials = new string[]
		{
			"Glass",
			"RefinedMetal"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER3, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Replaceable = false;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.EnergyConsumptionWhenActive = 10f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.RibbonInputPort(PixelPack.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.PIXELPACK.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.PIXELPACK.INPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.PIXELPACK.INPUT_PORT_INACTIVE, false, false)
		};
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ObjectLayer = ObjectLayer.Backwall;
		buildingDef.SceneLayer = Grid.SceneLayer.InteriorWall;
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, PixelPackConfig.ID);
		return buildingDef;
	}

	// Token: 0x06001100 RID: 4352 RVA: 0x0005FE86 File Offset: 0x0005E086
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddComponent<ZoneTile>();
	}

	// Token: 0x06001101 RID: 4353 RVA: 0x0005FEB0 File Offset: 0x0005E0B0
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Backwall, false);
		go.AddOrGet<PixelPack>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000A68 RID: 2664
	public static string ID = "PixelPack";
}
