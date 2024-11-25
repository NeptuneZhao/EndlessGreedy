using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class GammaMushConfig : IEntityConfig
{
	// Token: 0x060008BC RID: 2236 RVA: 0x000374E3 File Offset: 0x000356E3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x000374EC File Offset: 0x000356EC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GammaMush", STRINGS.ITEMS.FOOD.GAMMAMUSH.NAME, STRINGS.ITEMS.FOOD.GAMMAMUSH.DESC, 1f, false, Assets.GetAnim("mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.GAMMAMUSH);
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x00037550 File Offset: 0x00035750
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x00037552 File Offset: 0x00035752
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005FA RID: 1530
	public const string ID = "GammaMush";

	// Token: 0x040005FB RID: 1531
	public static ComplexRecipe recipe;
}
