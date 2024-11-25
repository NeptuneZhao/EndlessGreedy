using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000159 RID: 345
public class BasicSingleHarvestPlantConfig : IEntityConfig
{
	// Token: 0x060006BF RID: 1727 RVA: 0x0002D0AF File Offset: 0x0002B2AF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0002D0B8 File Offset: 0x0002B2B8
	public GameObject CreatePrefab()
	{
		string id = "BasicSingleHarvestPlant";
		string name = STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("meallice_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 218.15f, 283.15f, 303.15f, 398.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, "BasicPlantFood", true, false, true, true, 2400f, 0f, 4600f, "BasicSingleHarvestPlantOriginal", STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.NAME);
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "BasicSingleHarvestPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.BASICSINGLEHARVESTPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.BASICSINGLEHARVESTPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_meallice_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.DOMESTICATEDDESC;
		GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 1, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false, null);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Dirt,
				massConsumptionRate = 0.016666668f
			}
		});
		EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "BasicSingleHarvestPlant_preview", Assets.GetAnim("meallice_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
		return gameObject;
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0002D28D File Offset: 0x0002B48D
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x0002D28F File Offset: 0x0002B48F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004BF RID: 1215
	public const string ID = "BasicSingleHarvestPlant";

	// Token: 0x040004C0 RID: 1216
	public const string SEED_ID = "BasicSingleHarvestPlantSeed";

	// Token: 0x040004C1 RID: 1217
	public const float DIRT_RATE = 0.016666668f;
}
