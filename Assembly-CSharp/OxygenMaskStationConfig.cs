﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000323 RID: 803
public class OxygenMaskStationConfig : IBuildingConfig
{
	// Token: 0x060010CC RID: 4300 RVA: 0x0005E9E4 File Offset: 0x0005CBE4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OxygenMaskStation";
		int width = 2;
		int height = 3;
		string anim = "oxygen_mask_station_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] construction_materials = raw_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x0005EA6C File Offset: 0x0005CC6C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddComponent<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		storage.storageFilters = new List<Tag>
		{
			GameTags.Metal
		};
		storage.capacityKg = 45f;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage2.showInUI = true;
		storage2.storageFilters = new List<Tag>
		{
			GameTags.Breathable
		};
		MaskStation maskStation = go.AddOrGet<MaskStation>();
		maskStation.materialConsumedPerMask = 15f;
		maskStation.oxygenConsumedPerMask = 20f;
		maskStation.maxUses = 3;
		maskStation.materialTag = GameTags.Metal;
		maskStation.oxygenTag = GameTags.Breathable;
		maskStation.choreTypeID = this.fetchChoreType.Id;
		maskStation.PathFlag = PathFinder.PotentialPath.Flags.HasOxygenMask;
		maskStation.materialStorage = storage;
		maskStation.oxygenStorage = storage2;
		ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
		elementConsumer.elementToConsume = SimHashes.Oxygen;
		elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
		elementConsumer.consumptionRate = 0.5f;
		elementConsumer.storeOnConsume = true;
		elementConsumer.showInStatusPanel = false;
		elementConsumer.consumptionRadius = 2;
		elementConsumer.storage = storage2;
		ElementConsumer elementConsumer2 = go.AddComponent<ElementConsumer>();
		elementConsumer2.elementToConsume = SimHashes.ContaminatedOxygen;
		elementConsumer2.configuration = ElementConsumer.Configuration.AllGas;
		elementConsumer2.consumptionRate = 0.5f;
		elementConsumer2.storeOnConsume = true;
		elementConsumer2.showInStatusPanel = false;
		elementConsumer2.consumptionRadius = 2;
		elementConsumer2.storage = storage2;
		Prioritizable.AddRef(go);
		go.AddOrGet<LoopingSounds>();
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x0005EBC9 File Offset: 0x0005CDC9
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A42 RID: 2626
	public const string ID = "OxygenMaskStation";

	// Token: 0x04000A43 RID: 2627
	public const float MATERIAL_PER_MASK = 15f;

	// Token: 0x04000A44 RID: 2628
	public const float OXYGEN_PER_MASK = 20f;

	// Token: 0x04000A45 RID: 2629
	public const int MASKS_PER_REFILL = 3;

	// Token: 0x04000A46 RID: 2630
	public const float WORK_TIME = 5f;

	// Token: 0x04000A47 RID: 2631
	public ChoreType fetchChoreType = Db.Get().ChoreTypes.Fetch;
}
