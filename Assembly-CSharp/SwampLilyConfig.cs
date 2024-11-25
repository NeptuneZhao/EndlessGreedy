using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000189 RID: 393
public class SwampLilyConfig : IEntityConfig
{
	// Token: 0x06000801 RID: 2049 RVA: 0x000352D8 File Offset: 0x000334D8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x000352E0 File Offset: 0x000334E0
	public GameObject CreatePrefab()
	{
		string id = "SwampLily";
		string name = STRINGS.CREATURES.SPECIES.SWAMPLILY.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SWAMPLILY.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("swamplily_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 328.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 258.15f, 308.15f, 358.15f, 448.15f, new SimHashes[]
		{
			SimHashes.ChlorineGas
		}, true, 0f, 0.15f, SwampLilyFlowerConfig.ID, true, true, true, true, 2400f, 0f, 4600f, SwampLilyConfig.ID + "Original", STRINGS.CREATURES.SPECIES.SWAMPLILY.NAME);
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "SwampLilySeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.SWAMPLILY.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.SWAMPLILY.DESC;
		KAnimFile anim = Assets.GetAnim("seed_swampLily_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.SWAMPLILY.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 21, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false, null), SwampLilyConfig.ID + "_preview", Assets.GetAnim("swamplily_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_death", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_death_bloom", NOISE_POLLUTION.CREATURES.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, SwampLilyConfig.ID);
		return gameObject;
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x000354C7 File Offset: 0x000336C7
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x000354C9 File Offset: 0x000336C9
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005A4 RID: 1444
	public static string ID = "SwampLily";

	// Token: 0x040005A5 RID: 1445
	public const string SEED_ID = "SwampLilySeed";
}
