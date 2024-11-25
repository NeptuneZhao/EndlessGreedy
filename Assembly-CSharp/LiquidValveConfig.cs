using System;
using TUNING;
using UnityEngine;

// Token: 0x02000255 RID: 597
public class LiquidValveConfig : IBuildingConfig
{
	// Token: 0x06000C52 RID: 3154 RVA: 0x000483FC File Offset: 0x000465FC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidValve";
		int width = 1;
		int height = 2;
		string anim = "valveliquid_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidValve");
		return buildingDef;
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x000484A0 File Offset: 0x000466A0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		ValveBase valveBase = go.AddOrGet<ValveBase>();
		valveBase.conduitType = ConduitType.Liquid;
		valveBase.maxFlow = 10f;
		valveBase.animFlowRanges = new ValveBase.AnimRangeInfo[]
		{
			new ValveBase.AnimRangeInfo(3f, "lo"),
			new ValveBase.AnimRangeInfo(7f, "med"),
			new ValveBase.AnimRangeInfo(10f, "hi")
		};
		go.AddOrGet<Valve>();
		go.AddOrGet<Workable>().workTime = 5f;
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x00048544 File Offset: 0x00046744
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040007F1 RID: 2033
	public const string ID = "LiquidValve";

	// Token: 0x040007F2 RID: 2034
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
}
