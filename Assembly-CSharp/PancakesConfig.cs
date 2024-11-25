using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B6 RID: 438
public class PancakesConfig : IEntityConfig
{
	// Token: 0x060008ED RID: 2285 RVA: 0x00037B0A File Offset: 0x00035D0A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x00037B14 File Offset: 0x00035D14
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Pancakes", STRINGS.ITEMS.FOOD.PANCAKES.NAME, STRINGS.ITEMS.FOOD.PANCAKES.DESC, 1f, false, Assets.GetAnim("stackedpancakes_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.8f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PANCAKES);
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00037B78 File Offset: 0x00035D78
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x00037B7A File Offset: 0x00035D7A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400060C RID: 1548
	public const string ID = "Pancakes";

	// Token: 0x0400060D RID: 1549
	public static ComplexRecipe recipe;
}
