using System;
using TUNING;
using UnityEngine;

// Token: 0x020001E7 RID: 487
public class GasConduitBridgeConfig : IBuildingConfig
{
	// Token: 0x060009FA RID: 2554 RVA: 0x0003AD58 File Offset: 0x00038F58
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasConduitBridge";
		int width = 3;
		int height = 1;
		string anim = "utilitygasbridge_kanim";
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Conduit;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ObjectLayer = ObjectLayer.GasConduitConnection;
		buildingDef.SceneLayer = Grid.SceneLayer.GasConduitBridges;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, buildingDef.PrefabID);
		return buildingDef;
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x0003AE2F File Offset: 0x0003902F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<ConduitBridge>().type = ConduitType.Gas;
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x0003AE58 File Offset: 0x00039058
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
	}

	// Token: 0x0400068B RID: 1675
	public const string ID = "GasConduitBridge";

	// Token: 0x0400068C RID: 1676
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;
}
