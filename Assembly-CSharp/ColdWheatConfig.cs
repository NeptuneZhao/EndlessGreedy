﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000161 RID: 353
public class ColdWheatConfig : IEntityConfig
{
	// Token: 0x060006E8 RID: 1768 RVA: 0x0002E084 File Offset: 0x0002C284
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x0002E08C File Offset: 0x0002C28C
	public GameObject CreatePrefab()
	{
		string id = "ColdWheat";
		string name = STRINGS.CREATURES.SPECIES.COLDWHEAT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.COLDWHEAT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("coldwheat_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, 255f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 118.149994f, 218.15f, 278.15f, 358.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, "ColdWheatSeed", true, true, true, true, 2400f, 0f, 12200f, "ColdWheatOriginal", STRINGS.CREATURES.SPECIES.COLDWHEAT.NAME);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Dirt,
				massConsumptionRate = 0.008333334f
			}
		});
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Water,
				massConsumptionRate = 0.033333335f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Crop;
		string id2 = "ColdWheatSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.COLDWHEAT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.COLDWHEAT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_coldwheat_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.COLDWHEAT.DOMESTICATEDDESC;
		GameObject gameObject2 = EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 3, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.2f, 0.2f, null, "", true, null);
		EntityTemplates.ExtendEntityToFood(gameObject2, FOOD.FOOD_TYPES.COLD_WHEAT_SEED);
		EntityTemplates.CreateAndRegisterPreviewForPlant(gameObject2, "ColdWheat_preview", Assets.GetAnim("coldwheat_kanim"), "place", 1, 1);
		SoundEventVolumeCache.instance.AddVolume("coldwheat_kanim", "ColdWheat_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("coldwheat_kanim", "ColdWheat_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x0002E291 File Offset: 0x0002C491
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x0002E293 File Offset: 0x0002C493
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004E3 RID: 1251
	public const string ID = "ColdWheat";

	// Token: 0x040004E4 RID: 1252
	public const string SEED_ID = "ColdWheatSeed";

	// Token: 0x040004E5 RID: 1253
	public const float FERTILIZATION_RATE = 0.008333334f;

	// Token: 0x040004E6 RID: 1254
	public const float WATER_RATE = 0.033333335f;
}
