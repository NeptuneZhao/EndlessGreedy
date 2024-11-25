using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000250 RID: 592
public class LiquidLogicValveConfig : IBuildingConfig
{
	// Token: 0x06000C3F RID: 3135 RVA: 0x00047B88 File Offset: 0x00045D88
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidLogicValve";
		int width = 1;
		int height = 2;
		string anim = "valveliquid_logic_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 10f;
		buildingDef.PowerInputOffset = new CellOffset(0, 1);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LIQUIDLOGICVALVE.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LIQUIDLOGICVALVE.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LIQUIDLOGICVALVE.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidLogicValve");
		return buildingDef;
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x00047C8A File Offset: 0x00045E8A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		OperationalValve operationalValve = go.AddOrGet<OperationalValve>();
		operationalValve.conduitType = ConduitType.Liquid;
		operationalValve.maxFlow = 10f;
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x00047CC4 File Offset: 0x00045EC4
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		go.GetComponent<RequireInputs>().SetRequirements(true, false);
		go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040007E7 RID: 2023
	public const string ID = "LiquidLogicValve";

	// Token: 0x040007E8 RID: 2024
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
}
