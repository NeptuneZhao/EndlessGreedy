using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200019D RID: 413
public class CookedEggConfig : IEntityConfig
{
	// Token: 0x0600086B RID: 2155 RVA: 0x00036D20 File Offset: 0x00034F20
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00036D28 File Offset: 0x00034F28
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedEgg", STRINGS.ITEMS.FOOD.COOKEDEGG.NAME, STRINGS.ITEMS.FOOD.COOKEDEGG.DESC, 1f, false, Assets.GetAnim("cookedegg_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_EGG);
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x00036D8C File Offset: 0x00034F8C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00036D8E File Offset: 0x00034F8E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005DB RID: 1499
	public const string ID = "CookedEgg";

	// Token: 0x040005DC RID: 1500
	public static ComplexRecipe recipe;
}
