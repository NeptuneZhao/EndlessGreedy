﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class SwampHarvestPlantConfig : IEntityConfig
{
	// Token: 0x060007FC RID: 2044 RVA: 0x00035110 File Offset: 0x00033310
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00035118 File Offset: 0x00033318
	public GameObject CreatePrefab()
	{
		string id = "SwampHarvestPlant";
		string name = STRINGS.CREATURES.SPECIES.SWAMPHARVESTPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SWAMPHARVESTPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("swampcrop_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		GameObject template = gameObject;
		float temperature_lethal_low = 218.15f;
		float temperature_warning_low = 283.15f;
		float temperature_warning_high = 303.15f;
		float temperature_lethal_high = 398.15f;
		string text = SwampFruitConfig.ID;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, text, true, true, true, true, 2400f, 0f, 4600f, "SwampHarvestPlantOriginal", gameObject.PrefabID().Name);
		gameObject.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(true);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.DirtyWater,
				massConsumptionRate = 0.06666667f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<LoopingSounds>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "SwampHarvestPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.SWAMPHARVESTPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.SWAMPHARVESTPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_swampcrop_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		text = STRINGS.CREATURES.SPECIES.SWAMPHARVESTPLANT.DOMESTICATEDDESC;
		string[] dlcIds = this.GetDlcIds();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 2, text, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false, dlcIds), "SwampHarvestPlant_preview", Assets.GetAnim("swampcrop_kanim"), "place", 1, 2);
		return gameObject;
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x000352CC File Offset: 0x000334CC
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x000352CE File Offset: 0x000334CE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005A1 RID: 1441
	public const string ID = "SwampHarvestPlant";

	// Token: 0x040005A2 RID: 1442
	public const string SEED_ID = "SwampHarvestPlantSeed";

	// Token: 0x040005A3 RID: 1443
	public const float WATER_RATE = 0.06666667f;
}
