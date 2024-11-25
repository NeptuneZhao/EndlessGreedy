using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200028F RID: 655
public class BasicCureConfig : IEntityConfig
{
	// Token: 0x06000D8A RID: 3466 RVA: 0x0004D43A File Offset: 0x0004B63A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D8B RID: 3467 RVA: 0x0004D444 File Offset: 0x0004B644
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicCure", STRINGS.ITEMS.PILLS.BASICCURE.NAME, STRINGS.ITEMS.PILLS.BASICCURE.DESC, 1f, true, Assets.GetAnim("pill_foodpoisoning_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.BASICCURE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Carbon.CreateTag(), 1f),
			new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BasicCure", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		BasicCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 50f,
			description = STRINGS.ITEMS.PILLS.BASICCURE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 10
		};
		return gameObject;
	}

	// Token: 0x06000D8C RID: 3468 RVA: 0x0004D55E File Offset: 0x0004B75E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x0004D560 File Offset: 0x0004B760
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000866 RID: 2150
	public const string ID = "BasicCure";

	// Token: 0x04000867 RID: 2151
	public static ComplexRecipe recipe;
}
