using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class HardSkinBerryPlantConfig : IEntityConfig
{
	// Token: 0x0600073A RID: 1850 RVA: 0x000306E6 File Offset: 0x0002E8E6
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x000306F0 File Offset: 0x0002E8F0
	public GameObject CreatePrefab()
	{
		string id = "HardSkinBerryPlant";
		string name = STRINGS.CREATURES.SPECIES.HARDSKINBERRYPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.HARDSKINBERRYPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("ice_berry_bush_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 255f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 118.149994f, 218.15f, 259.15f, 269.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, "HardSkinBerry", true, true, true, true, 2400f, 0f, 4600f, "HardSkinBerryPlantOriginal", STRINGS.CREATURES.SPECIES.HARDSKINBERRYPLANT.NAME);
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.AddOrGet<LoopingSounds>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "HardSkinBerryPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.HARDSKINBERRYPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.HARDSKINBERRYPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_ice_berry_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.HARDSKINBERRYPLANT.DOMESTICATEDDESC;
		string[] dlcIds = this.GetDlcIds();
		GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 1, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false, dlcIds);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Phosphorite.CreateTag(),
				massConsumptionRate = 0.008333334f
			}
		});
		EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "HardSkinBerryPlant_preview", Assets.GetAnim("ice_berry_bush_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
		return gameObject;
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x000308D3 File Offset: 0x0002EAD3
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x000308D5 File Offset: 0x0002EAD5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000528 RID: 1320
	public const string ID = "HardSkinBerryPlant";

	// Token: 0x04000529 RID: 1321
	public const string SEED_ID = "HardSkinBerryPlantSeed";

	// Token: 0x0400052A RID: 1322
	public const float Temperature_lethal_low = 118.149994f;

	// Token: 0x0400052B RID: 1323
	public const float Temperature_warning_low = 218.15f;

	// Token: 0x0400052C RID: 1324
	public const float Temperature_lethal_high = 269.15f;

	// Token: 0x0400052D RID: 1325
	public const float Temperature_warning_high = 259.15f;

	// Token: 0x0400052E RID: 1326
	public const float FERTILIZATION_RATE = 0.008333334f;
}
