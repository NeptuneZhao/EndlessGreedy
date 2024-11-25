using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200028E RID: 654
public class BasicBoosterConfig : IEntityConfig
{
	// Token: 0x06000D85 RID: 3461 RVA: 0x0004D322 File Offset: 0x0004B522
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x0004D32C File Offset: 0x0004B52C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicBooster", STRINGS.ITEMS.PILLS.BASICBOOSTER.NAME, STRINGS.ITEMS.PILLS.BASICBOOSTER.DESC, 1f, true, Assets.GetAnim("pill_2_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.BASICBOOSTER);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Carbon", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BasicBooster".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		BasicBoosterConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 50f,
			description = STRINGS.ITEMS.PILLS.BASICBOOSTER.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 1
		};
		return gameObject;
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x0004D42E File Offset: 0x0004B62E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x0004D430 File Offset: 0x0004B630
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000864 RID: 2148
	public const string ID = "BasicBooster";

	// Token: 0x04000865 RID: 2149
	public static ComplexRecipe recipe;
}
