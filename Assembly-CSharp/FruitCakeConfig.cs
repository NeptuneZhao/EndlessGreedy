using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001AC RID: 428
public class FruitCakeConfig : IEntityConfig
{
	// Token: 0x060008B7 RID: 2231 RVA: 0x00037448 File Offset: 0x00035648
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x00037450 File Offset: 0x00035650
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FruitCake", STRINGS.ITEMS.FOOD.FRUITCAKE.NAME, STRINGS.ITEMS.FOOD.FRUITCAKE.DESC, 1f, false, Assets.GetAnim("fruitcake_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.FRUITCAKE);
		ComplexRecipeManager.Get().GetRecipe(FruitCakeConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x000374D7 File Offset: 0x000356D7
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x000374D9 File Offset: 0x000356D9
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F8 RID: 1528
	public const string ID = "FruitCake";

	// Token: 0x040005F9 RID: 1529
	public static ComplexRecipe recipe;
}
