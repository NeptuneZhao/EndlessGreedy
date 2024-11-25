using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class SalsaConfig : IEntityConfig
{
	// Token: 0x0600091E RID: 2334 RVA: 0x0003812C File Offset: 0x0003632C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x00038134 File Offset: 0x00036334
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Salsa", STRINGS.ITEMS.FOOD.SALSA.NAME, STRINGS.ITEMS.FOOD.SALSA.DESC, 1f, false, Assets.GetAnim("zestysalsa_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SALSA);
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x00038198 File Offset: 0x00036398
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0003819A File Offset: 0x0003639A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400061E RID: 1566
	public const string ID = "Salsa";

	// Token: 0x0400061F RID: 1567
	public static ComplexRecipe recipe;
}
