using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class CookedMeatConfig : IEntityConfig
{
	// Token: 0x06000875 RID: 2165 RVA: 0x00036E10 File Offset: 0x00035010
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x00036E18 File Offset: 0x00035018
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedMeat", STRINGS.ITEMS.FOOD.COOKEDMEAT.NAME, STRINGS.ITEMS.FOOD.COOKEDMEAT.DESC, 1f, false, Assets.GetAnim("barbeque_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_MEAT);
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x00036E7C File Offset: 0x0003507C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x00036E7E File Offset: 0x0003507E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005DF RID: 1503
	public const string ID = "CookedMeat";

	// Token: 0x040005E0 RID: 1504
	public static ComplexRecipe recipe;
}
