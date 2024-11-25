using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020003CB RID: 971
public class SpiceGrinderConfig : IBuildingConfig
{
	// Token: 0x06001445 RID: 5189 RVA: 0x0006F5B8 File Offset: 0x0006D7B8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SpiceGrinder";
		int width = 2;
		int height = 3;
		string anim = "spice_grinder_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x0006F638 File Offset: 0x0006D838
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.SpiceStation, false);
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x0006F654 File Offset: 0x0006D854
	public override void DoPostConfigureComplete(GameObject go)
	{
		SpiceGrinder.InitializeSpices();
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGetDef<SpiceGrinder.Def>();
		go.AddOrGet<SpiceGrinderWorkable>();
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<TreeFilterable>().uiHeight = TreeFilterable.UISideScreenHeight.Short;
		go.AddOrGet<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, SpiceGrinderConfig.STORAGE_PRIORITY));
		Storage storage = go.AddComponent<Storage>();
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.storageFilters = new List<Tag>
		{
			GameTags.Edible
		};
		storage.allowItemRemoval = false;
		storage.capacityKg = 1f;
		storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage.fetchCategory = Storage.FetchCategory.Building;
		storage.showCapacityStatusItem = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.showSideScreenTitleBar = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		Storage storage2 = go.AddComponent<Storage>();
		storage2.showInUI = true;
		storage2.showDescriptor = true;
		storage2.storageFilters = new List<Tag>
		{
			GameTags.Seed
		};
		storage2.allowItemRemoval = false;
		storage2.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage2.fetchCategory = Storage.FetchCategory.Building;
		storage2.showCapacityStatusItem = true;
		storage2.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Kitchen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
	}

	// Token: 0x04000B8F RID: 2959
	public const string ID = "SpiceGrinder";

	// Token: 0x04000B90 RID: 2960
	public static Tag MATERIAL_FOR_TINKER = GameTags.CropSeed;

	// Token: 0x04000B91 RID: 2961
	public static Tag TINKER_TOOLS = FarmStationToolsConfig.tag;

	// Token: 0x04000B92 RID: 2962
	public const float MASS_PER_TINKER = 5f;

	// Token: 0x04000B93 RID: 2963
	public const float OUTPUT_TEMPERATURE = 313.15f;

	// Token: 0x04000B94 RID: 2964
	public const float WORK_TIME_PER_1000KCAL = 5f;

	// Token: 0x04000B95 RID: 2965
	public const short SPICE_CAPACITY_PER_INGREDIENT = 10;

	// Token: 0x04000B96 RID: 2966
	public const string PrimaryColorSymbol = "stripe_anim2";

	// Token: 0x04000B97 RID: 2967
	public const string SecondaryColorSymbol = "stripe_anim1";

	// Token: 0x04000B98 RID: 2968
	public const string GrinderColorSymbol = "grinder";

	// Token: 0x04000B99 RID: 2969
	public static StatusItem SpicedStatus = Db.Get().MiscStatusItems.SpicedFood;

	// Token: 0x04000B9A RID: 2970
	private static int STORAGE_PRIORITY = Chore.DefaultPrioritySetting.priority_value - 1;
}
