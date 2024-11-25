﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200015A RID: 346
public class BeanPlantConfig : IEntityConfig
{
	// Token: 0x060006C4 RID: 1732 RVA: 0x0002D299 File Offset: 0x0002B499
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0002D2A0 File Offset: 0x0002B4A0
	public GameObject CreatePrefab()
	{
		string id = "BeanPlant";
		string name = STRINGS.CREATURES.SPECIES.BEAN_PLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BEAN_PLANT.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("beanplant_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 258.15f);
		GameObject template = gameObject;
		float temperature_lethal_low = 198.15f;
		float temperature_warning_low = 248.15f;
		float temperature_warning_high = 273.15f;
		float temperature_lethal_high = 323.15f;
		string text = STRINGS.CREATURES.SPECIES.BEAN_PLANT.NAME;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.CarbonDioxide
		}, true, 0f, 0.025f, "BeanPlantSeed", true, true, true, true, 2400f, 0f, 9800f, "BeanPlantOriginal", text);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Ethanol.CreateTag(),
				massConsumptionRate = 0.033333335f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Dirt.CreateTag(),
				massConsumptionRate = 0.008333334f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Crop;
		string id2 = "BeanPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.BEAN_PLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.BEAN_PLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_beanplant_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		text = STRINGS.CREATURES.SPECIES.BEAN_PLANT.DOMESTICATEDDESC;
		GameObject gameObject2 = EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 3, text, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.3f, null, "", true, null);
		EntityTemplates.ExtendEntityToFood(gameObject2, FOOD.FOOD_TYPES.BEAN);
		EntityTemplates.CreateAndRegisterPreviewForPlant(gameObject2, "BeanPlant_preview", Assets.GetAnim("beanplant_kanim"), "place", 1, 2);
		return gameObject;
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x0002D483 File Offset: 0x0002B683
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x0002D485 File Offset: 0x0002B685
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004C2 RID: 1218
	public const string ID = "BeanPlant";

	// Token: 0x040004C3 RID: 1219
	public const string SEED_ID = "BeanPlantSeed";

	// Token: 0x040004C4 RID: 1220
	public const float FERTILIZATION_RATE = 0.008333334f;

	// Token: 0x040004C5 RID: 1221
	public const float WATER_RATE = 0.033333335f;
}
