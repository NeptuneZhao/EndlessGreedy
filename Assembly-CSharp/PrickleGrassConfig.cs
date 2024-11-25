using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class PrickleGrassConfig : IEntityConfig
{
	// Token: 0x06000771 RID: 1905 RVA: 0x00031865 File Offset: 0x0002FA65
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x0003186C File Offset: 0x0002FA6C
	public GameObject CreatePrefab()
	{
		string id = "PrickleGrass";
		string name = STRINGS.CREATURES.SPECIES.PRICKLEGRASS.NAME;
		string desc = STRINGS.CREATURES.SPECIES.PRICKLEGRASS.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = PrickleGrassConfig.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("bristlebriar_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 218.15f, 283.15f, 303.15f, 398.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 900f, "PrickleGrassOriginal", STRINGS.CREATURES.SPECIES.PRICKLEGRASS.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = PrickleGrassConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = PrickleGrassConfig.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "PrickleGrassSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEGRASS.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEGRASS.DESC;
		KAnimFile anim = Assets.GetAnim("seed_bristlebriar_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.PRICKLEGRASS.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 10, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false, null), "PrickleGrass_preview", Assets.GetAnim("bristlebriar_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x000319D8 File Offset: 0x0002FBD8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x000319DA File Offset: 0x0002FBDA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000549 RID: 1353
	public const string ID = "PrickleGrass";

	// Token: 0x0400054A RID: 1354
	public const string SEED_ID = "PrickleGrassSeed";

	// Token: 0x0400054B RID: 1355
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x0400054C RID: 1356
	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
