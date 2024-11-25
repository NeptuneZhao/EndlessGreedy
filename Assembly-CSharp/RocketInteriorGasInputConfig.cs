using System;
using TUNING;
using UnityEngine;

// Token: 0x0200038C RID: 908
public class RocketInteriorGasInputConfig : IBuildingConfig
{
	// Token: 0x060012DF RID: 4831 RVA: 0x00068F71 File Offset: 0x00067171
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x00068F78 File Offset: 0x00067178
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorGasInput";
		int width = 1;
		int height = 1;
		string anim = "rocket_floor_plug_gas_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnRocketEnvelope;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
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
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "RocketInteriorGasInput");
		return buildingDef;
	}

	// Token: 0x060012E1 RID: 4833 RVA: 0x00069040 File Offset: 0x00067240
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x00069064 File Offset: 0x00067264
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<ActiveController.Def>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1f;
		RocketConduitStorageAccess rocketConduitStorageAccess = go.AddOrGet<RocketConduitStorageAccess>();
		rocketConduitStorageAccess.storage = storage;
		rocketConduitStorageAccess.cargoType = CargoBay.CargoType.Gasses;
		rocketConduitStorageAccess.targetLevel = 0f;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = storage.capacityKg;
	}

	// Token: 0x04000B04 RID: 2820
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x04000B05 RID: 2821
	private const CargoBay.CargoType CARGO_TYPE = CargoBay.CargoType.Gasses;

	// Token: 0x04000B06 RID: 2822
	public const string ID = "RocketInteriorGasInput";
}
