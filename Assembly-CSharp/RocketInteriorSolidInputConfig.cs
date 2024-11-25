using System;
using TUNING;
using UnityEngine;

// Token: 0x02000395 RID: 917
public class RocketInteriorSolidInputConfig : IBuildingConfig
{
	// Token: 0x06001318 RID: 4888 RVA: 0x00069E2D File Offset: 0x0006802D
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x00069E34 File Offset: 0x00068034
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorSolidInput";
		int width = 1;
		int height = 1;
		string anim = "rocket_floor_plug_solid_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnRocketEnvelope;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
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
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "RocketInteriorSolidInput");
		return buildingDef;
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x00069EF5 File Offset: 0x000680F5
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x00069F18 File Offset: 0x00068118
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<ActiveController.Def>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 20f;
		RocketConduitStorageAccess rocketConduitStorageAccess = go.AddOrGet<RocketConduitStorageAccess>();
		rocketConduitStorageAccess.storage = storage;
		rocketConduitStorageAccess.cargoType = CargoBay.CargoType.Solids;
		rocketConduitStorageAccess.targetLevel = 0f;
		SolidConduitConsumer solidConduitConsumer = go.AddOrGet<SolidConduitConsumer>();
		solidConduitConsumer.alwaysConsume = true;
		solidConduitConsumer.capacityKG = storage.capacityKg;
	}

	// Token: 0x04000B19 RID: 2841
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;

	// Token: 0x04000B1A RID: 2842
	private const CargoBay.CargoType CARGO_TYPE = CargoBay.CargoType.Solids;

	// Token: 0x04000B1B RID: 2843
	public const string ID = "RocketInteriorSolidInput";
}
