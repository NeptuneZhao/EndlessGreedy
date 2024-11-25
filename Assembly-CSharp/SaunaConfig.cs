using System;
using TUNING;
using UnityEngine;

// Token: 0x0200039A RID: 922
public class SaunaConfig : IBuildingConfig
{
	// Token: 0x06001331 RID: 4913 RVA: 0x0006A668 File Offset: 0x00068868
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Sauna";
		int width = 3;
		int height = 3;
		string anim = "sauna_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			100f,
			100f
		};
		string[] construction_materials = new string[]
		{
			"Metal",
			"BuildingWood"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER2, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 2);
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		return buildingDef;
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x0006A748 File Offset: 0x00068948
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
		conduitConsumer.capacityKG = 50f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.alwaysConsume = true;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.Water
		};
		go.AddOrGet<SaunaWorkable>().basePriority = RELAXATION.PRIORITY.TIER3;
		Sauna sauna = go.AddOrGet<Sauna>();
		sauna.steamPerUseKG = 25f;
		sauna.waterOutputTemp = 353.15f;
		sauna.specificEffect = "Sauna";
		sauna.trackingEffect = "RecentlySauna";
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x0006A83E File Offset: 0x00068A3E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<RequireInputs>().requireConduitHasMass = false;
	}

	// Token: 0x04000B2C RID: 2860
	public const string ID = "Sauna";

	// Token: 0x04000B2D RID: 2861
	public const string COLD_IMMUNITY_EFFECT_NAME = "WarmTouch";

	// Token: 0x04000B2E RID: 2862
	public const float COLD_IMMUNITY_DURATION = 1800f;

	// Token: 0x04000B2F RID: 2863
	private const float STEAM_PER_USE_KG = 25f;

	// Token: 0x04000B30 RID: 2864
	private const float WATER_OUTPUT_TEMP = 353.15f;
}
