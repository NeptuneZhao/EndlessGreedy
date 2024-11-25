using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class WormBasicFoodConfig : IEntityConfig
{
	// Token: 0x06000973 RID: 2419 RVA: 0x000389A7 File Offset: 0x00036BA7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x000389B0 File Offset: 0x00036BB0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormBasicFood", STRINGS.ITEMS.FOOD.WORMBASICFOOD.NAME, STRINGS.ITEMS.FOOD.WORMBASICFOOD.DESC, 1f, false, Assets.GetAnim("wormwood_roast_nuts_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMBASICFOOD);
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x00038A14 File Offset: 0x00036C14
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x00038A16 File Offset: 0x00036C16
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000645 RID: 1605
	public const string ID = "WormBasicFood";

	// Token: 0x04000646 RID: 1606
	public static ComplexRecipe recipe;
}
