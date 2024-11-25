using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200028D RID: 653
public class AntihistamineConfig : IEntityConfig
{
	// Token: 0x06000D80 RID: 3456 RVA: 0x0004D1F3 File Offset: 0x0004B3F3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x0004D1FC File Offset: 0x0004B3FC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Antihistamine", STRINGS.ITEMS.PILLS.ANTIHISTAMINE.NAME, STRINGS.ITEMS.PILLS.ANTIHISTAMINE.DESC, 1f, true, Assets.GetAnim("pill_allergies_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.ANTIHISTAMINE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("PrickleFlowerSeed", 1f),
			new ComplexRecipe.RecipeElement(SimHashes.Dirt.CreateTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Antihistamine", 10f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		AntihistamineConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 100f,
			description = STRINGS.ITEMS.PILLS.ANTIHISTAMINE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 10
		};
		return gameObject;
	}

	// Token: 0x06000D82 RID: 3458 RVA: 0x0004D316 File Offset: 0x0004B516
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D83 RID: 3459 RVA: 0x0004D318 File Offset: 0x0004B518
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000862 RID: 2146
	public const string ID = "Antihistamine";

	// Token: 0x04000863 RID: 2147
	public static ComplexRecipe recipe;
}
