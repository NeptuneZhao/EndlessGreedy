using System;
using TUNING;
using UnityEngine;

// Token: 0x02000396 RID: 918
public class RocketInteriorSolidOutputConfig : IBuildingConfig
{
	// Token: 0x0600131D RID: 4893 RVA: 0x00069F7C File Offset: 0x0006817C
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x00069F84 File Offset: 0x00068184
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorSolidOutput";
		int width = 1;
		int height = 1;
		string anim = "rocket_floor_plug_solid_out_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnRocketEnvelope;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.OutputConduitType = ConduitType.Solid;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "RocketInteriorSolidOutput");
		return buildingDef;
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x0006A06D File Offset: 0x0006826D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x0006A090 File Offset: 0x00068290
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 20f;
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Solid;
		RocketConduitStorageAccess rocketConduitStorageAccess = go.AddOrGet<RocketConduitStorageAccess>();
		rocketConduitStorageAccess.storage = storage;
		rocketConduitStorageAccess.cargoType = CargoBay.CargoType.Solids;
		rocketConduitStorageAccess.targetLevel = 20f;
		SolidConduitDispenser solidConduitDispenser = go.AddOrGet<SolidConduitDispenser>();
		solidConduitDispenser.alwaysDispense = true;
		solidConduitDispenser.elementFilter = null;
	}

	// Token: 0x04000B1C RID: 2844
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;

	// Token: 0x04000B1D RID: 2845
	private const CargoBay.CargoType CARGO_TYPE = CargoBay.CargoType.Solids;

	// Token: 0x04000B1E RID: 2846
	public const string ID = "RocketInteriorSolidOutput";
}
