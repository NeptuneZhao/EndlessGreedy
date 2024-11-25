using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class SpiceNutConfig : IEntityConfig
{
	// Token: 0x06000939 RID: 2361 RVA: 0x000383BE File Offset: 0x000365BE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x000383C8 File Offset: 0x000365C8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(SpiceNutConfig.ID, STRINGS.ITEMS.FOOD.SPICENUT.NAME, STRINGS.ITEMS.FOOD.SPICENUT.DESC, 1f, false, Assets.GetAnim("spicenut_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.SPICENUT);
		SoundEventVolumeCache.instance.AddVolume("vinespicenut_kanim", "VineSpiceNut_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("vinespicenut_kanim", "VineSpiceNut_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00038460 File Offset: 0x00036660
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x00038462 File Offset: 0x00036662
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400062B RID: 1579
	public static float SEEDS_PER_FRUIT = 1f;

	// Token: 0x0400062C RID: 1580
	public static string ID = "SpiceNut";
}
