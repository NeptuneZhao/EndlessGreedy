using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000293 RID: 659
public class IntermediateRadPillConfig : IEntityConfig
{
	// Token: 0x06000D9E RID: 3486 RVA: 0x0004D8D1 File Offset: 0x0004BAD1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000D9F RID: 3487 RVA: 0x0004D8D8 File Offset: 0x0004BAD8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IntermediateRadPill", STRINGS.ITEMS.PILLS.INTERMEDIATERADPILL.NAME, STRINGS.ITEMS.PILLS.INTERMEDIATERADPILL.DESC, 1f, true, Assets.GetAnim("vial_radiation_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.INTERMEDIATERADPILL);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Carbon", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("IntermediateRadPill".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		IntermediateRadPillConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("AdvancedApothecary", array, array2), array, array2)
		{
			time = 50f,
			description = STRINGS.ITEMS.PILLS.INTERMEDIATERADPILL.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"AdvancedApothecary"
			},
			sortOrder = 21
		};
		return gameObject;
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x0004D9DB File Offset: 0x0004BBDB
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x0004D9DD File Offset: 0x0004BBDD
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400086E RID: 2158
	public const string ID = "IntermediateRadPill";

	// Token: 0x0400086F RID: 2159
	public static ComplexRecipe recipe;
}
