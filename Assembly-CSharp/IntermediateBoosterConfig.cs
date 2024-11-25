using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000291 RID: 657
public class IntermediateBoosterConfig : IEntityConfig
{
	// Token: 0x06000D94 RID: 3476 RVA: 0x0004D683 File Offset: 0x0004B883
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x0004D68C File Offset: 0x0004B88C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IntermediateBooster", STRINGS.ITEMS.PILLS.INTERMEDIATEBOOSTER.NAME, STRINGS.ITEMS.PILLS.INTERMEDIATEBOOSTER.DESC, 1f, true, Assets.GetAnim("pill_3_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.INTERMEDIATEBOOSTER);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SpiceNutConfig.ID, 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("IntermediateBooster", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		IntermediateBoosterConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 100f,
			description = STRINGS.ITEMS.PILLS.INTERMEDIATEBOOSTER.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 5
		};
		return gameObject;
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x0004D78E File Offset: 0x0004B98E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x0004D790 File Offset: 0x0004B990
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400086A RID: 2154
	public const string ID = "IntermediateBooster";

	// Token: 0x0400086B RID: 2155
	public static ComplexRecipe recipe;
}
