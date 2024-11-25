using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000176 RID: 374
public class MushroomPlantConfig : IEntityConfig
{
	// Token: 0x06000758 RID: 1880 RVA: 0x00030EE3 File Offset: 0x0002F0E3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x00030EEC File Offset: 0x0002F0EC
	public GameObject CreatePrefab()
	{
		string id = "MushroomPlant";
		string name = STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("fungusplant_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 228.15f, 278.15f, 308.15f, 398.15f, new SimHashes[]
		{
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, MushroomConfig.ID, true, true, true, true, 2400f, 0f, 4600f, "MushroomPlantOriginal", STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.NAME);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.SlimeMold,
				massConsumptionRate = 0.006666667f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(true);
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "MushroomSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.MUSHROOMPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.MUSHROOMPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_fungusplant_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 3, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.33f, 0.33f, null, "", false, null), "MushroomPlant_preview", Assets.GetAnim("fungusplant_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x000310B9 File Offset: 0x0002F2B9
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x000310BB File Offset: 0x0002F2BB
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400053A RID: 1338
	public const float FERTILIZATION_RATE = 0.006666667f;

	// Token: 0x0400053B RID: 1339
	public const string ID = "MushroomPlant";

	// Token: 0x0400053C RID: 1340
	public const string SEED_ID = "MushroomSeed";
}
