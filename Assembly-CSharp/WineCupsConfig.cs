using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class WineCupsConfig : IEntityConfig
{
	// Token: 0x06000818 RID: 2072 RVA: 0x00035C4E File Offset: 0x00033E4E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x00035C58 File Offset: 0x00033E58
	public GameObject CreatePrefab()
	{
		string id = "WineCups";
		string name = STRINGS.CREATURES.SPECIES.WINECUPS.NAME;
		string desc = STRINGS.CREATURES.SPECIES.WINECUPS.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = WineCupsConfig.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_cups_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 218.15f, 283.15f, 303.15f, 398.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 900f, "WineCupsOriginal", STRINGS.CREATURES.SPECIES.WINECUPS.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = WineCupsConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = WineCupsConfig.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "WineCupsSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.WINECUPS.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.WINECUPS.DESC;
		KAnimFile anim = Assets.GetAnim("seed_potted_cups_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.WINECUPS.DOMESTICATEDDESC;
		string[] dlcIds = this.GetDlcIds();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 11, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false, dlcIds), "WineCups_preview", Assets.GetAnim("potted_cups_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x00035DCD File Offset: 0x00033FCD
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00035DCF File Offset: 0x00033FCF
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005B6 RID: 1462
	public const string ID = "WineCups";

	// Token: 0x040005B7 RID: 1463
	public const string SEED_ID = "WineCupsSeed";

	// Token: 0x040005B8 RID: 1464
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x040005B9 RID: 1465
	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
