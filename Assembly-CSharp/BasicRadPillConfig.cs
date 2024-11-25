using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000290 RID: 656
public class BasicRadPillConfig : IEntityConfig
{
	// Token: 0x06000D8F RID: 3471 RVA: 0x0004D56A File Offset: 0x0004B76A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x0004D574 File Offset: 0x0004B774
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicRadPill", STRINGS.ITEMS.PILLS.BASICRADPILL.NAME, STRINGS.ITEMS.PILLS.BASICRADPILL.DESC, 1f, true, Assets.GetAnim("pill_radiation_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.BASICRADPILL);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Carbon", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BasicRadPill".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		BasicRadPillConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 50f,
			description = STRINGS.ITEMS.PILLS.BASICRADPILL.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 10
		};
		return gameObject;
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x0004D677 File Offset: 0x0004B877
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x0004D679 File Offset: 0x0004B879
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000868 RID: 2152
	public const string ID = "BasicRadPill";

	// Token: 0x04000869 RID: 2153
	public static ComplexRecipe recipe;
}
