using System;
using TUNING;
using UnityEngine;

// Token: 0x02000214 RID: 532
public class HotTubConfig : IBuildingConfig
{
	// Token: 0x06000AFD RID: 2813 RVA: 0x00041AB4 File Offset: 0x0003FCB4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HotTub";
		int width = 5;
		int height = 2;
		string anim = "hottub_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] construction_mass = new float[]
		{
			200f,
			200f
		};
		string[] construction_materials = new string[]
		{
			"Metal",
			"BuildingWood"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER3, none, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.OverheatTemperature = this.MINIMUM_WATER_TEMPERATURE;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(2, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(-2, 0);
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		return buildingDef;
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x00041BB4 File Offset: 0x0003FDB4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = this.WATER_AMOUNT;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
		conduitConsumer.SetOnState(false);
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.storage = storage;
		conduitDispenser.SetOnState(false);
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = new Tag("BleachStone");
		manualDeliveryKG.capacity = 100f;
		manualDeliveryKG.refillMass = 10f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		HotTub hotTub = go.AddOrGet<HotTub>();
		hotTub.waterStorage = storage;
		hotTub.hotTubCapacity = this.WATER_AMOUNT;
		hotTub.waterCoolingRate = 15f;
		hotTub.minimumWaterTemperature = this.MINIMUM_WATER_TEMPERATURE;
		hotTub.bleachStoneConsumption = this.BLEACH_STONE_CONSUMPTION_RATE;
		hotTub.maxOperatingTemperature = this.MAXIMUM_TUB_TEMPERATURE;
		hotTub.specificEffect = "HotTub";
		hotTub.trackingEffect = "RecentlyHotTub";
		hotTub.basePriority = RELAXATION.PRIORITY.TIER4;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x00041D2C File Offset: 0x0003FF2C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<RequireInputs>().requireConduitHasMass = false;
	}

	// Token: 0x04000740 RID: 1856
	public const string ID = "HotTub";

	// Token: 0x04000741 RID: 1857
	public const string COLD_IMMUNITY_EFFECT_NAME = "WarmTouch";

	// Token: 0x04000742 RID: 1858
	public const float COLD_IMMUNITY_DURATION = 1800f;

	// Token: 0x04000743 RID: 1859
	private float WATER_AMOUNT = 100f;

	// Token: 0x04000744 RID: 1860
	private const float KDTU_TRANSFER_RATE = 15f;

	// Token: 0x04000745 RID: 1861
	private float MINIMUM_WATER_TEMPERATURE = 310.85f;

	// Token: 0x04000746 RID: 1862
	private float MAXIMUM_TUB_TEMPERATURE = 310.85f;

	// Token: 0x04000747 RID: 1863
	private float BLEACH_STONE_CONSUMPTION_RATE = 0.11666667f;
}
