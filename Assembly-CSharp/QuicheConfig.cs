using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001BB RID: 443
public class QuicheConfig : IEntityConfig
{
	// Token: 0x06000908 RID: 2312 RVA: 0x00037EB3 File Offset: 0x000360B3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x00037EBC File Offset: 0x000360BC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Quiche", STRINGS.ITEMS.FOOD.QUICHE.NAME, STRINGS.ITEMS.FOOD.QUICHE.DESC, 1f, false, Assets.GetAnim("quiche_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.QUICHE);
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x00037F20 File Offset: 0x00036120
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x00037F22 File Offset: 0x00036122
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000616 RID: 1558
	public const string ID = "Quiche";

	// Token: 0x04000617 RID: 1559
	public static ComplexRecipe recipe;
}
