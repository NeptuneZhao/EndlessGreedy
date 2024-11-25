using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class DeepFriedMeatConfig : IEntityConfig
{
	// Token: 0x0600088F RID: 2191 RVA: 0x00037084 File Offset: 0x00035284
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0003708C File Offset: 0x0003528C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedMeat", STRINGS.ITEMS.FOOD.DEEPFRIEDMEAT.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDMEAT.DESC, 1f, false, Assets.GetAnim("deepfried_meat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_MEAT);
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x000370F0 File Offset: 0x000352F0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x000370F2 File Offset: 0x000352F2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005EA RID: 1514
	public const string ID = "DeepFriedMeat";

	// Token: 0x040005EB RID: 1515
	public static ComplexRecipe recipe;
}
