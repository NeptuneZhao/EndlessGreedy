﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000309 RID: 777
public class NoseconeHarvestConfig : IBuildingConfig
{
	// Token: 0x06001053 RID: 4179 RVA: 0x0005C3BB File Offset: 0x0005A5BB
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001054 RID: 4180 RVA: 0x0005C3C4 File Offset: 0x0005A5C4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "NoseconeHarvest";
		int width = 5;
		int height = 4;
		string anim = "rocket_nosecone_gathering_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] nose_CONE_TIER = BUILDINGS.ROCKETRY_MASS_KG.NOSE_CONE_TIER2;
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, nose_CONE_TIER, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x0005C484 File Offset: 0x0005A684
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.NoseRocketModule, false);
		go.AddOrGetDef<ResourceHarvestModule.Def>().harvestSpeed = this.solidCapacity / this.timeToFill;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.useWideOffsets = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = SimHashes.Diamond.CreateTag();
		manualDeliveryKG.MinimumMass = storage.capacityKg;
		manualDeliveryKG.capacity = storage.capacityKg;
		manualDeliveryKG.refillMass = storage.capacityKg;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x0005C55A File Offset: 0x0005A75A
	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MINOR, 0f, 0f);
		go.GetComponent<ReorderableBuilding>().buildConditions.Add(new TopOnly());
	}

	// Token: 0x040009F4 RID: 2548
	public const string ID = "NoseconeHarvest";

	// Token: 0x040009F5 RID: 2549
	private float timeToFill = 3600f;

	// Token: 0x040009F6 RID: 2550
	private float solidCapacity = ROCKETRY.SOLID_CARGO_BAY_CLUSTER_CAPACITY * ROCKETRY.CARGO_CAPACITY_SCALE;

	// Token: 0x040009F7 RID: 2551
	public const float DIAMOND_CONSUMED_PER_HARVEST_KG = 0.05f;

	// Token: 0x040009F8 RID: 2552
	public const float DIAMOND_STORAGE_CAPACITY_KG = 1000f;
}
