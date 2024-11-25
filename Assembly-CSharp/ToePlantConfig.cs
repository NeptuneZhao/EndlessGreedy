using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200018A RID: 394
public class ToePlantConfig : IEntityConfig
{
	// Token: 0x06000807 RID: 2055 RVA: 0x000354DF File Offset: 0x000336DF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x000354E8 File Offset: 0x000336E8
	public GameObject CreatePrefab()
	{
		string id = "ToePlant";
		string name = STRINGS.CREATURES.SPECIES.TOEPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.TOEPLANT.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = ToePlantConfig.POSITIVE_DECOR_EFFECT;
		KAnimFile anim = Assets.GetAnim("potted_toes_kanim");
		string initialAnim = "grow_seed";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 1;
		EffectorValues decor = positive_DECOR_EFFECT;
		float freezing_ = TUNING.CREATURES.TEMPERATURE.FREEZING_3;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, null, freezing_);
		GameObject template = gameObject;
		SimHashes[] safe_elements = new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		};
		EntityTemplates.ExtendEntityToBasicPlant(template, TUNING.CREATURES.TEMPERATURE.FREEZING_10, TUNING.CREATURES.TEMPERATURE.FREEZING_9, TUNING.CREATURES.TEMPERATURE.FREEZING, TUNING.CREATURES.TEMPERATURE.COOL, safe_elements, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "ToePlantOriginal", STRINGS.CREATURES.SPECIES.TOEPLANT.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = ToePlantConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = ToePlantConfig.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "ToePlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.TOEPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.TOEPLANT.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_potted_toes_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.TOEPLANT.DOMESTICATEDDESC;
		string[] dlcIds = this.GetDlcIds();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 12, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false, dlcIds), "ToePlant_preview", Assets.GetAnim("potted_toes_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x00035665 File Offset: 0x00033865
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00035667 File Offset: 0x00033867
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005A6 RID: 1446
	public const string ID = "ToePlant";

	// Token: 0x040005A7 RID: 1447
	public const string SEED_ID = "ToePlantSeed";

	// Token: 0x040005A8 RID: 1448
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x040005A9 RID: 1449
	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
