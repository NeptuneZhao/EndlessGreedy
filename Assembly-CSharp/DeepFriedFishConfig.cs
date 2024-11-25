using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public class DeepFriedFishConfig : IEntityConfig
{
	// Token: 0x0600088A RID: 2186 RVA: 0x0003700A File Offset: 0x0003520A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00037014 File Offset: 0x00035214
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedFish", STRINGS.ITEMS.FOOD.DEEPFRIEDFISH.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDFISH.DESC, 1f, false, Assets.GetAnim("deepfried_fish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_FISH);
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00037078 File Offset: 0x00035278
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0003707A File Offset: 0x0003527A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005E8 RID: 1512
	public const string ID = "DeepFriedFish";

	// Token: 0x040005E9 RID: 1513
	public static ComplexRecipe recipe;
}
