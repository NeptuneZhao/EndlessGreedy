﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020003B3 RID: 947
public class SodaFountainConfig : IBuildingConfig
{
	// Token: 0x060013B1 RID: 5041 RVA: 0x0006C914 File Offset: 0x0006AB14
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SodaFountain";
		int width = 2;
		int height = 2;
		string anim = "sodamaker_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(1, 1);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		return buildingDef;
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x0006C9BC File Offset: 0x0006ABBC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = 20f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = SimHashes.CarbonDioxide.CreateTag();
		manualDeliveryKG.capacity = 4f;
		manualDeliveryKG.refillMass = 1f;
		manualDeliveryKG.MinimumMass = 0.5f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		go.AddOrGet<SodaFountainWorkable>().basePriority = RELAXATION.PRIORITY.TIER5;
		SodaFountain sodaFountain = go.AddOrGet<SodaFountain>();
		sodaFountain.specificEffect = "SodaFountain";
		sodaFountain.trackingEffect = "RecentlyRecDrink";
		sodaFountain.ingredientTag = SimHashes.CarbonDioxide.CreateTag();
		sodaFountain.ingredientMassPerUse = 1f;
		sodaFountain.waterMassPerUse = 5f;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
		component.prefabInitFn += this.OnInit;
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x0006CB08 File Offset: 0x0006AD08
	private void OnInit(GameObject go)
	{
		SodaFountainWorkable component = go.GetComponent<SodaFountainWorkable>();
		KAnimFile[] value = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sodamaker_kanim")
		};
		component.workerTypeOverrideAnims.Add(MinionConfig.ID, value);
		component.workerTypeOverrideAnims.Add(BionicMinionConfig.ID, new KAnimFile[]
		{
			Assets.GetAnim("anim_bionic_interacts_sodamaker_kanim")
		});
	}

	// Token: 0x060013B4 RID: 5044 RVA: 0x0006CB78 File Offset: 0x0006AD78
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000B4A RID: 2890
	public const string ID = "SodaFountain";
}
