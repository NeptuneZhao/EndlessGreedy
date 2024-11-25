using System;
using TUNING;
using UnityEngine;

// Token: 0x0200038E RID: 910
public class RocketInteriorGasOutputConfig : IBuildingConfig
{
	// Token: 0x060012EC RID: 4844 RVA: 0x000692EC File Offset: 0x000674EC
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060012ED RID: 4845 RVA: 0x000692F4 File Offset: 0x000674F4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorGasOutput";
		int width = 1;
		int height = 1;
		string anim = "rocket_floor_plug_gas_out_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnRocketEnvelope;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.ShowInBuildMenu = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "RocketInteriorGasOutput");
		return buildingDef;
	}

	// Token: 0x060012EE RID: 4846 RVA: 0x000693E4 File Offset: 0x000675E4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x00069408 File Offset: 0x00067608
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1f;
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Gas;
		RocketConduitStorageAccess rocketConduitStorageAccess = go.AddOrGet<RocketConduitStorageAccess>();
		rocketConduitStorageAccess.storage = storage;
		rocketConduitStorageAccess.cargoType = CargoBay.CargoType.Gasses;
		rocketConduitStorageAccess.targetLevel = 1f;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
	}

	// Token: 0x04000B09 RID: 2825
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x04000B0A RID: 2826
	private const CargoBay.CargoType CARGO_TYPE = CargoBay.CargoType.Gasses;

	// Token: 0x04000B0B RID: 2827
	public const string ID = "RocketInteriorGasOutput";
}
