using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200018E RID: 398
public class WormPlantConfig : IEntityConfig
{
	// Token: 0x0600081E RID: 2078 RVA: 0x00035DEF File Offset: 0x00033FEF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x00035DF8 File Offset: 0x00033FF8
	public static GameObject BaseWormPlant(string id, string name, string desc, string animFile, EffectorValues decor, string cropID)
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, 1f, Assets.GetAnim(animFile), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, default(EffectorValues), SimHashes.Creature, null, 307.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 273.15f, 288.15f, 323.15f, 373.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, cropID, true, true, true, true, 2400f, 0f, 9800f, id + "Original", name);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Sulfur.CreateTag(),
				massConsumptionRate = 0.016666668f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x00035EE4 File Offset: 0x000340E4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = WormPlantConfig.BaseWormPlant("WormPlant", STRINGS.CREATURES.SPECIES.WORMPLANT.NAME, STRINGS.CREATURES.SPECIES.WORMPLANT.DESC, "wormwood_kanim", WormPlantConfig.BASIC_DECOR, "WormBasicFruit");
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id = "WormPlantSeed";
		string name = STRINGS.CREATURES.SPECIES.SEEDS.WORMPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SEEDS.WORMPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_wormwood_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.WORMPLANT.DOMESTICATEDDESC;
		string[] dlcIds = this.GetDlcIds();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, productionType, id, name, desc, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 3, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false, dlcIds), "WormPlant_preview", Assets.GetAnim("wormwood_kanim"), "place", 1, 2);
		return gameObject;
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00035FB8 File Offset: 0x000341B8
	public void OnPrefabInit(GameObject prefab)
	{
		TransformingPlant transformingPlant = prefab.AddOrGet<TransformingPlant>();
		transformingPlant.transformPlantId = "SuperWormPlant";
		transformingPlant.SubscribeToTransformEvent(GameHashes.CropTended);
		transformingPlant.useGrowthTimeRatio = true;
		transformingPlant.eventDataCondition = delegate(object data)
		{
			CropTendingStates.CropTendingEventData cropTendingEventData = (CropTendingStates.CropTendingEventData)data;
			if (cropTendingEventData != null)
			{
				CreatureBrain component = cropTendingEventData.source.GetComponent<CreatureBrain>();
				if (component != null && component.species == GameTags.Creatures.Species.DivergentSpecies)
				{
					return true;
				}
			}
			return false;
		};
		transformingPlant.fxKAnim = "plant_transform_fx_kanim";
		transformingPlant.fxAnim = "plant_transform";
		prefab.AddOrGet<StandardCropPlant>().anims = WormPlantConfig.animSet;
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00036032 File Offset: 0x00034232
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005BA RID: 1466
	public const string ID = "WormPlant";

	// Token: 0x040005BB RID: 1467
	public const string SEED_ID = "WormPlantSeed";

	// Token: 0x040005BC RID: 1468
	public const float SULFUR_CONSUMPTION_RATE = 0.016666668f;

	// Token: 0x040005BD RID: 1469
	public static readonly EffectorValues BASIC_DECOR = DECOR.PENALTY.TIER0;

	// Token: 0x040005BE RID: 1470
	public const string BASIC_CROP_ID = "WormBasicFruit";

	// Token: 0x040005BF RID: 1471
	private static StandardCropPlant.AnimSet animSet = new StandardCropPlant.AnimSet
	{
		grow = "basic_grow",
		grow_pst = "basic_grow_pst",
		idle_full = "basic_idle_full",
		wilt_base = "basic_wilt",
		harvest = "basic_harvest"
	};
}
