using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class MushroomWrapConfig : IEntityConfig
{
	// Token: 0x060008DD RID: 2269 RVA: 0x00037983 File Offset: 0x00035B83
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0003798C File Offset: 0x00035B8C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("MushroomWrap", STRINGS.ITEMS.FOOD.MUSHROOMWRAP.NAME, STRINGS.ITEMS.FOOD.MUSHROOMWRAP.DESC, 1f, false, Assets.GetAnim("mushroom_wrap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.MUSHROOM_WRAP);
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x000379F0 File Offset: 0x00035BF0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x000379F2 File Offset: 0x00035BF2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000605 RID: 1541
	public const string ID = "MushroomWrap";

	// Token: 0x04000606 RID: 1542
	public static ComplexRecipe recipe;
}
