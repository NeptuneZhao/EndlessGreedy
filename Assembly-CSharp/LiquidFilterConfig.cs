using System;
using TUNING;
using UnityEngine;

// Token: 0x0200024A RID: 586
public class LiquidFilterConfig : IBuildingConfig
{
	// Token: 0x06000C24 RID: 3108 RVA: 0x00047220 File Offset: 0x00045420
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidFilter";
		int width = 3;
		int height = 1;
		string anim = "filter_liquid_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidFilter");
		return buildingDef;
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x000472E9 File Offset: 0x000454E9
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x000472FC File Offset: 0x000454FC
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x0004730D File Offset: 0x0004550D
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x0004731D File Offset: 0x0004551D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<ElementFilter>().portInfo = this.secondaryPort;
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Liquid;
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x0004734D File Offset: 0x0004554D
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040007DA RID: 2010
	public const string ID = "LiquidFilter";

	// Token: 0x040007DB RID: 2011
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x040007DC RID: 2012
	private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 0));
}
