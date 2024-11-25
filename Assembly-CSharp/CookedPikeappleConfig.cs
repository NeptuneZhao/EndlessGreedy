using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class CookedPikeappleConfig : IEntityConfig
{
	// Token: 0x0600087A RID: 2170 RVA: 0x00036E88 File Offset: 0x00035088
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00036E90 File Offset: 0x00035090
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedPikeapple", STRINGS.ITEMS.FOOD.COOKEDPIKEAPPLE.NAME, STRINGS.ITEMS.FOOD.COOKEDPIKEAPPLE.DESC, 1f, false, Assets.GetAnim("iceberry_cooked_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_PIKEAPPLE);
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00036EF4 File Offset: 0x000350F4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00036EF6 File Offset: 0x000350F6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005E1 RID: 1505
	public const string ID = "CookedPikeapple";

	// Token: 0x040005E2 RID: 1506
	public static ComplexRecipe recipe;
}
