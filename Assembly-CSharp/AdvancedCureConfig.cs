using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200028C RID: 652
public class AdvancedCureConfig : IEntityConfig
{
	// Token: 0x06000D7B RID: 3451 RVA: 0x0004D0BD File Offset: 0x0004B2BD
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x0004D0C4 File Offset: 0x0004B2C4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("AdvancedCure", STRINGS.ITEMS.PILLS.ADVANCEDCURE.NAME, STRINGS.ITEMS.PILLS.ADVANCEDCURE.DESC, 1f, true, Assets.GetAnim("vial_spore_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.ADVANCEDCURE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Steel.CreateTag(), 1f),
			new ComplexRecipe.RecipeElement("LightBugOrangeEgg", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("AdvancedCure", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		string text = "Apothecary";
		AdvancedCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(text, array, array2), array, array2)
		{
			time = 200f,
			description = STRINGS.ITEMS.PILLS.ADVANCEDCURE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				text
			},
			sortOrder = 20,
			requiredTech = "MedicineIV"
		};
		return gameObject;
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x0004D1E7 File Offset: 0x0004B3E7
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D7E RID: 3454 RVA: 0x0004D1E9 File Offset: 0x0004B3E9
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000860 RID: 2144
	public const string ID = "AdvancedCure";

	// Token: 0x04000861 RID: 2145
	public static ComplexRecipe recipe;
}
