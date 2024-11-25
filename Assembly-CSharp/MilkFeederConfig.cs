using System;
using TUNING;
using UnityEngine;

// Token: 0x0200029D RID: 669
public class MilkFeederConfig : IBuildingConfig
{
	// Token: 0x06000DD3 RID: 3539 RVA: 0x0004F620 File Offset: 0x0004D820
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MilkFeeder";
		int width = 3;
		int height = 3;
		string anim = "critter_milk_feeder_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x0004F69E File Offset: 0x0004D89E
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x0004F6A0 File Offset: 0x0004D8A0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<LogicOperationalController>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 80f;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.allowItemRemoval = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Milk);
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType, false);
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x0004F769 File Offset: 0x0004D969
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<MilkFeeder.Def>();
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x0004F772 File Offset: 0x0004D972
	public override void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x040008AD RID: 2221
	public const string ID = "MilkFeeder";

	// Token: 0x040008AE RID: 2222
	public const string HAD_CONSUMED_MILK_RECENTLY_EFFECT_ID = "HadMilk";

	// Token: 0x040008AF RID: 2223
	public const float EFFECT_DURATION_IN_SECONDS = 600f;

	// Token: 0x040008B0 RID: 2224
	public static readonly CellOffset DRINK_FROM_OFFSET = new CellOffset(1, 0);

	// Token: 0x040008B1 RID: 2225
	public static readonly Tag MILK_TAG = SimHashes.Milk.CreateTag();

	// Token: 0x040008B2 RID: 2226
	public const float UNITS_OF_MILK_CONSUMED_PER_FEEDING = 5f;
}
