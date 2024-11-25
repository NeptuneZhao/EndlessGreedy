using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B8 RID: 440
public class PickledMealConfig : IEntityConfig
{
	// Token: 0x060008F7 RID: 2295 RVA: 0x00037C1F File Offset: 0x00035E1F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x00037C28 File Offset: 0x00035E28
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("PickledMeal", STRINGS.ITEMS.FOOD.PICKLEDMEAL.NAME, STRINGS.ITEMS.FOOD.PICKLEDMEAL.DESC, 1f, false, Assets.GetAnim("pickledmeal_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PICKLEDMEAL);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Pickled, false);
		return gameObject;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x00037C9D File Offset: 0x00035E9D
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x00037C9F File Offset: 0x00035E9F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000610 RID: 1552
	public const string ID = "PickledMeal";

	// Token: 0x04000611 RID: 1553
	public static ComplexRecipe recipe;
}
