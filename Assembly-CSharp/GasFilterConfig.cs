using System;
using TUNING;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class GasFilterConfig : IBuildingConfig
{
	// Token: 0x06000A16 RID: 2582 RVA: 0x0003B4B8 File Offset: 0x000396B8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasFilter";
		int width = 3;
		int height = 1;
		string anim = "filter_gas_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasFilter");
		return buildingDef;
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x0003B581 File Offset: 0x00039781
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0003B594 File Offset: 0x00039794
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x0003B5A5 File Offset: 0x000397A5
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0003B5B5 File Offset: 0x000397B5
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<ElementFilter>().portInfo = this.secondaryPort;
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Gas;
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0003B5E5 File Offset: 0x000397E5
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000695 RID: 1685
	public const string ID = "GasFilter";

	// Token: 0x04000696 RID: 1686
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x04000697 RID: 1687
	private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 0));
}
