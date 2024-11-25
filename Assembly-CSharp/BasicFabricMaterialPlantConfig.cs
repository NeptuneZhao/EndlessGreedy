﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class BasicFabricMaterialPlantConfig : IEntityConfig
{
	// Token: 0x060006AF RID: 1711 RVA: 0x0002CD38 File Offset: 0x0002AF38
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x0002CD40 File Offset: 0x0002AF40
	public GameObject CreatePrefab()
	{
		string id = BasicFabricMaterialPlantConfig.ID;
		string name = STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("swampreed_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 3, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		GameObject template = gameObject;
		float temperature_lethal_low = 248.15f;
		float temperature_warning_low = 295.15f;
		float temperature_warning_high = 310.15f;
		float temperature_lethal_high = 398.15f;
		string text = BasicFabricConfig.ID;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide,
			SimHashes.DirtyWater,
			SimHashes.Water
		}, false, 0f, 0.15f, text, false, true, true, true, 2400f, 0f, 4600f, BasicFabricMaterialPlantConfig.ID + "Original", STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.NAME);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.DirtyWater,
				massConsumptionRate = 0.26666668f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.AddOrGet<LoopingSounds>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string seed_ID = BasicFabricMaterialPlantConfig.SEED_ID;
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_swampreed_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.WaterSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		text = STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, seed_ID, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 20, text, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false, null), BasicFabricMaterialPlantConfig.ID + "_preview", Assets.GetAnim("swampreed_kanim"), "place", 1, 3);
		SoundEventVolumeCache.instance.AddVolume("swampreed_kanim", "FabricPlant_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("swampreed_kanim", "FabricPlant_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x0002CF28 File Offset: 0x0002B128
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x0002CF2A File Offset: 0x0002B12A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004BA RID: 1210
	public static string ID = "BasicFabricPlant";

	// Token: 0x040004BB RID: 1211
	public static string SEED_ID = "BasicFabricMaterialPlantSeed";

	// Token: 0x040004BC RID: 1212
	public const float WATER_RATE = 0.26666668f;
}
