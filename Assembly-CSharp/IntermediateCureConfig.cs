using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000292 RID: 658
public class IntermediateCureConfig : IEntityConfig
{
	// Token: 0x06000D99 RID: 3481 RVA: 0x0004D79A File Offset: 0x0004B99A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x0004D7A4 File Offset: 0x0004B9A4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IntermediateCure", STRINGS.ITEMS.PILLS.INTERMEDIATECURE.NAME, STRINGS.ITEMS.PILLS.INTERMEDIATECURE.DESC, 1f, true, Assets.GetAnim("iv_slimelung_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.INTERMEDIATECURE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SwampLilyFlowerConfig.ID, 1f),
			new ComplexRecipe.RecipeElement(SimHashes.Phosphorite.CreateTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("IntermediateCure", 1f)
		};
		string text = "Apothecary";
		IntermediateCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(text, array, array2), array, array2)
		{
			time = 100f,
			description = STRINGS.ITEMS.PILLS.INTERMEDIATECURE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				text
			},
			sortOrder = 10,
			requiredTech = "MedicineII"
		};
		return gameObject;
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x0004D8C5 File Offset: 0x0004BAC5
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x0004D8C7 File Offset: 0x0004BAC7
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400086C RID: 2156
	public const string ID = "IntermediateCure";

	// Token: 0x0400086D RID: 2157
	public static ComplexRecipe recipe;
}
