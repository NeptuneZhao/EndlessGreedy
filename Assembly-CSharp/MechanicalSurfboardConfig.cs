using System;
using TUNING;
using UnityEngine;

// Token: 0x0200028A RID: 650
public class MechanicalSurfboardConfig : IBuildingConfig
{
	// Token: 0x06000D73 RID: 3443 RVA: 0x0004CD48 File Offset: 0x0004AF48
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MechanicalSurfboard";
		int width = 2;
		int height = 3;
		string anim = "mechanical_surfboard_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(1, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		return buildingDef;
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x0004CDF0 File Offset: 0x0004AFF0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = 20f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		go.AddOrGet<MechanicalSurfboardWorkable>().basePriority = RELAXATION.PRIORITY.TIER3;
		MechanicalSurfboard mechanicalSurfboard = go.AddOrGet<MechanicalSurfboard>();
		mechanicalSurfboard.waterSpillRateKG = 0.05f;
		mechanicalSurfboard.minOperationalWaterKG = 2f;
		mechanicalSurfboard.specificEffect = "MechanicalSurfboard";
		mechanicalSurfboard.trackingEffect = "RecentlyMechanicalSurfboard";
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x0004CEBF File Offset: 0x0004B0BF
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400085C RID: 2140
	public const string ID = "MechanicalSurfboard";

	// Token: 0x0400085D RID: 2141
	private const float TANK_SIZE_KG = 20f;

	// Token: 0x0400085E RID: 2142
	private const float SPILL_RATE_KG = 0.05f;
}
