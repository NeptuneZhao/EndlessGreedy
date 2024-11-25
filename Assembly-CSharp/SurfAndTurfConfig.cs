using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class SurfAndTurfConfig : IEntityConfig
{
	// Token: 0x0600094A RID: 2378 RVA: 0x0003858E File Offset: 0x0003678E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x00038598 File Offset: 0x00036798
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SurfAndTurf", STRINGS.ITEMS.FOOD.SURFANDTURF.NAME, STRINGS.ITEMS.FOOD.SURFANDTURF.DESC, 1f, false, Assets.GetAnim("surfnturf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SURF_AND_TURF);
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x000385FC File Offset: 0x000367FC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x000385FE File Offset: 0x000367FE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000633 RID: 1587
	public const string ID = "SurfAndTurf";

	// Token: 0x04000634 RID: 1588
	public static ComplexRecipe recipe;
}
