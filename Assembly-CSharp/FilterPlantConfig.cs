﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class FilterPlantConfig : IEntityConfig
{
	// Token: 0x060006FD RID: 1789 RVA: 0x0002E857 File Offset: 0x0002CA57
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0002E860 File Offset: 0x0002CA60
	public GameObject CreatePrefab()
	{
		string id = "FilterPlant";
		string name = STRINGS.CREATURES.SPECIES.FILTERPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.FILTERPLANT.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("cactus_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 348.15f);
		GameObject template = gameObject;
		float temperature_lethal_low = 253.15f;
		float temperature_warning_low = 293.15f;
		float temperature_warning_high = 383.15f;
		float temperature_lethal_high = 443.15f;
		string crop_id = SimHashes.Water.ToString();
		string text = STRINGS.CREATURES.SPECIES.FILTERPLANT.NAME;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.Oxygen
		}, true, 0f, 0.025f, crop_id, true, true, true, true, 2400f, 0f, 2200f, "FilterPlantOriginal", text);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Sand.CreateTag(),
				massConsumptionRate = 0.008333334f
			}
		});
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.DirtyWater,
				massConsumptionRate = 0.108333334f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<SaltPlant>();
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.showInStatusPanel = true;
		elementConsumer.showDescriptor = true;
		elementConsumer.storeOnConsume = false;
		elementConsumer.elementToConsume = SimHashes.Oxygen;
		elementConsumer.configuration = ElementConsumer.Configuration.Element;
		elementConsumer.consumptionRadius = 4;
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
		elementConsumer.consumptionRate = 0.008333334f;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "FilterPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.FILTERPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.FILTERPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_cactus_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		text = STRINGS.CREATURES.SPECIES.FILTERPLANT.DOMESTICATEDDESC;
		string[] dlcIds = this.GetDlcIds();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 21, text, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, null, "", false, dlcIds), "FilterPlant_preview", Assets.GetAnim("cactus_kanim"), "place", 1, 2);
		gameObject.AddTag(GameTags.DeprecatedContent);
		return gameObject;
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0002EAAF File Offset: 0x0002CCAF
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0002EAB1 File Offset: 0x0002CCB1
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F4 RID: 1268
	public const string ID = "FilterPlant";

	// Token: 0x040004F5 RID: 1269
	public const string SEED_ID = "FilterPlantSeed";

	// Token: 0x040004F6 RID: 1270
	public const float SAND_CONSUMPTION_RATE = 0.008333334f;

	// Token: 0x040004F7 RID: 1271
	public const float WATER_CONSUMPTION_RATE = 0.108333334f;

	// Token: 0x040004F8 RID: 1272
	public const float OXYGEN_CONSUMPTION_RATE = 0.008333334f;
}
