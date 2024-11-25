﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class ContactConductivePipeBridgeConfig : IBuildingConfig
{
	// Token: 0x0600017F RID: 383 RVA: 0x0000A28C File Offset: 0x0000848C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ContactConductivePipeBridge";
		int width = 3;
		int height = 1;
		string anim = "contactConductivePipeBridge_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.NoLiquidConduitAtOrigin;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.ObjectLayer = ObjectLayer.LiquidConduitConnection;
		buildingDef.SceneLayer = Grid.SceneLayer.LiquidConduitBridges;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.UseStructureTemperature = true;
		buildingDef.ReplacementTags = new List<Tag>();
		buildingDef.ReplacementTags.Add(GameTags.Pipes);
		buildingDef.ThermalConductivity = 2f;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "ContactConductivePipeBridge");
		return buildingDef;
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000A38E File Offset: 0x0000858E
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<StructureToStructureTemperature>();
		ContactConductivePipeBridge.Def def = go.AddOrGetDef<ContactConductivePipeBridge.Def>();
		def.pumpKGRate = 10f;
		def.type = ConduitType.Liquid;
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000A3C3 File Offset: 0x000085C3
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireOutputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
	}

	// Token: 0x040000E0 RID: 224
	public const float LIQUID_CAPACITY_KG = 10f;

	// Token: 0x040000E1 RID: 225
	public const float GAS_CAPACITY_KG = 0.5f;

	// Token: 0x040000E2 RID: 226
	public const float TEMPERATURE_EXCHANGE_WITH_STORAGE_MODIFIER = 50f;

	// Token: 0x040000E3 RID: 227
	public const float BUILDING_TO_BUILDING_TEMPERATURE_SCALE = 0.001f;

	// Token: 0x040000E4 RID: 228
	public const string ID = "ContactConductivePipeBridge";

	// Token: 0x040000E5 RID: 229
	public const float NO_LIQUIDS_COOLDOWN = 1.5f;

	// Token: 0x040000E6 RID: 230
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
}
