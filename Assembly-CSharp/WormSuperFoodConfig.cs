using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class WormSuperFoodConfig : IEntityConfig
{
	// Token: 0x0600097D RID: 2429 RVA: 0x00038A98 File Offset: 0x00036C98
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x00038AA0 File Offset: 0x00036CA0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormSuperFood", STRINGS.ITEMS.FOOD.WORMSUPERFOOD.NAME, STRINGS.ITEMS.FOOD.WORMSUPERFOOD.DESC, 1f, false, Assets.GetAnim("wormwood_preserved_berries_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMSUPERFOOD);
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00038B04 File Offset: 0x00036D04
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00038B06 File Offset: 0x00036D06
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000648 RID: 1608
	public const string ID = "WormSuperFood";

	// Token: 0x04000649 RID: 1609
	public static ComplexRecipe recipe;
}
