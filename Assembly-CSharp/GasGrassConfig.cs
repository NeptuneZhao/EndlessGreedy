﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class GasGrassConfig : IEntityConfig
{
	// Token: 0x0600071F RID: 1823 RVA: 0x0002F4BE File Offset: 0x0002D6BE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x0002F4C8 File Offset: 0x0002D6C8
	public GameObject CreatePrefab()
	{
		string id = "GasGrass";
		string name = STRINGS.CREATURES.SPECIES.GASGRASS.NAME;
		string desc = STRINGS.CREATURES.SPECIES.GASGRASS.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER3;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gassygrass_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 3, tier, default(EffectorValues), SimHashes.Creature, null, 255f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 218.15f, 0f, 348.15f, 373.15f, null, false, 0f, 0.15f, "GasGrassHarvested", true, true, true, true, 2400f, 0f, 12200f, "GasGrassOriginal", STRINGS.CREATURES.SPECIES.GASGRASS.NAME);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Chlorine,
				massConsumptionRate = 0.00083333335f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Dirt.CreateTag(),
				massConsumptionRate = 0.041666668f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.AddOrGet<HarvestDesignatable>().defaultHarvestStateWhenPlanted = false;
		Modifiers component = gameObject.GetComponent<Modifiers>();
		Db.Get().traits.Get(component.initialTraits[0]).Add(new AttributeModifier(Db.Get().PlantAttributes.MinLightLux.Id, 10000f, STRINGS.CREATURES.SPECIES.GASGRASS.NAME, false, false, true));
		component.initialAttributes.Add(Db.Get().PlantAttributes.MinLightLux.Id);
		gameObject.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(false);
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = DlcManager.FeaturePlantMutationsEnabled() ? SeedProducer.ProductionType.Harvest : SeedProducer.ProductionType.Hidden;
		string id2 = "GasGrassSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.GASGRASS.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.GASGRASS.DESC;
		KAnimFile anim = Assets.GetAnim("seed_gassygrass_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.GASGRASS.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 22, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.2f, 0.2f, null, "", false, null), "GasGrass_preview", Assets.GetAnim("gassygrass_kanim"), "place", 1, 1);
		SoundEventVolumeCache.instance.AddVolume("gassygrass_kanim", "GasGrass_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("gassygrass_kanim", "GasGrass_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x0002F753 File Offset: 0x0002D953
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x0002F755 File Offset: 0x0002D955
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000506 RID: 1286
	public const string ID = "GasGrass";

	// Token: 0x04000507 RID: 1287
	public const string SEED_ID = "GasGrassSeed";

	// Token: 0x04000508 RID: 1288
	public const float CHLORINE_FERTILIZATION_RATE = 0.00083333335f;

	// Token: 0x04000509 RID: 1289
	public const float DIRT_FERTILIZATION_RATE = 0.041666668f;
}
