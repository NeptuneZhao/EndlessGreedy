﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020003DA RID: 986
public class SugarEngineConfig : IBuildingConfig
{
	// Token: 0x0600149D RID: 5277 RVA: 0x000710F1 File Offset: 0x0006F2F1
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x000710F8 File Offset: 0x0006F2F8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SugarEngine";
		int width = 3;
		int height = 3;
		string anim = "rocket_sugar_engine_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] dense_TIER = BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, dense_TIER, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.InputConduitType = ConduitType.None;
		buildingDef.GeneratorWattageRating = 60f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		return buildingDef;
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x000711BC File Offset: 0x0006F3BC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 3), GameTags.Rocket, null)
		};
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x00071220 File Offset: 0x0006F420
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x060014A1 RID: 5281 RVA: 0x00071222 File Offset: 0x0006F422
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x00071224 File Offset: 0x0006F424
	public override void DoPostConfigureComplete(GameObject go)
	{
		RocketEngineCluster rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
		rocketEngineCluster.maxModules = 5;
		rocketEngineCluster.maxHeight = ROCKETRY.ROCKET_HEIGHT.SHORT;
		rocketEngineCluster.fuelTag = SimHashes.Sucrose.CreateTag();
		rocketEngineCluster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.STRONG;
		rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		rocketEngineCluster.requireOxidizer = true;
		rocketEngineCluster.exhaustElement = SimHashes.CarbonDioxide;
		go.AddOrGet<ModuleGenerator>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 450f;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		FuelTank fuelTank = go.AddOrGet<FuelTank>();
		fuelTank.consumeFuelOnLand = false;
		fuelTank.storage = storage;
		fuelTank.FuelType = SimHashes.Sucrose.CreateTag();
		fuelTank.targetFillMass = storage.capacityKg;
		fuelTank.physicalFuelCapacity = storage.capacityKg;
		go.AddOrGet<CopyBuildingSettings>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = ElementLoader.FindElementByHash(SimHashes.Sucrose).tag;
		manualDeliveryKG.refillMass = storage.capacityKg;
		manualDeliveryKG.capacity = storage.capacityKg;
		manualDeliveryKG.operationalRequirement = Operational.State.None;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.INSIGNIFICANT, (float)ROCKETRY.ENGINE_POWER.EARLY_WEAK, SugarEngineConfig.FUEL_EFFICIENCY);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
		};
	}

	// Token: 0x04000BC4 RID: 3012
	public const string ID = "SugarEngine";

	// Token: 0x04000BC5 RID: 3013
	public const SimHashes FUEL = SimHashes.Sucrose;

	// Token: 0x04000BC6 RID: 3014
	public const float FUEL_CAPACITY = 450f;

	// Token: 0x04000BC7 RID: 3015
	public static float FUEL_EFFICIENCY = 0.125f;
}
