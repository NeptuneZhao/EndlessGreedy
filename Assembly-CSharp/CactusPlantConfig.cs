using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class CactusPlantConfig : IEntityConfig
{
	// Token: 0x060006D3 RID: 1747 RVA: 0x0002D898 File Offset: 0x0002BA98
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x0002D8A0 File Offset: 0x0002BAA0
	public GameObject CreatePrefab()
	{
		string id = "CactusPlant";
		string name = STRINGS.CREATURES.SPECIES.CACTUSPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.CACTUSPLANT.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = this.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_cactus_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 200f, 273.15f, 373.15f, 400f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, false, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "CactusPlantOriginal", STRINGS.CREATURES.SPECIES.CACTUSPLANT.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "CactusPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.CACTUSPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.CACTUSPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_potted_cactus_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.CACTUSPLANT.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 13, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false, null), "CactusPlant_preview", Assets.GetAnim("potted_cactus_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x0002DA0F File Offset: 0x0002BC0F
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x0002DA11 File Offset: 0x0002BC11
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004CE RID: 1230
	public const string ID = "CactusPlant";

	// Token: 0x040004CF RID: 1231
	public const string SEED_ID = "CactusPlantSeed";

	// Token: 0x040004D0 RID: 1232
	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x040004D1 RID: 1233
	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
