using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000B80 RID: 2944
public class ComplexRecipe
{
	// Token: 0x06005871 RID: 22641 RVA: 0x001FE69A File Offset: 0x001FC89A
	public string[] GetDlcIds()
	{
		return this.dlcIds;
	}

	// Token: 0x170006AD RID: 1709
	// (get) Token: 0x06005872 RID: 22642 RVA: 0x001FE6A2 File Offset: 0x001FC8A2
	// (set) Token: 0x06005873 RID: 22643 RVA: 0x001FE6AA File Offset: 0x001FC8AA
	public bool ProductHasFacade { get; set; }

	// Token: 0x170006AE RID: 1710
	// (get) Token: 0x06005874 RID: 22644 RVA: 0x001FE6B3 File Offset: 0x001FC8B3
	// (set) Token: 0x06005875 RID: 22645 RVA: 0x001FE6BB File Offset: 0x001FC8BB
	public bool RequiresAllIngredientsDiscovered { get; set; }

	// Token: 0x170006AF RID: 1711
	// (get) Token: 0x06005876 RID: 22646 RVA: 0x001FE6C4 File Offset: 0x001FC8C4
	public Tag FirstResult
	{
		get
		{
			return this.results[0].material;
		}
	}

	// Token: 0x06005877 RID: 22647 RVA: 0x001FE6D3 File Offset: 0x001FC8D3
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results)
	{
		this.id = id;
		this.ingredients = ingredients;
		this.results = results;
		ComplexRecipeManager.Get().Add(this);
	}

	// Token: 0x06005878 RID: 22648 RVA: 0x001FE711 File Offset: 0x001FC911
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP, int producedHEP) : this(id, ingredients, results)
	{
		this.consumedHEP = consumedHEP;
		this.producedHEP = producedHEP;
	}

	// Token: 0x06005879 RID: 22649 RVA: 0x001FE72C File Offset: 0x001FC92C
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP) : this(id, ingredients, results, consumedHEP, 0)
	{
	}

	// Token: 0x0600587A RID: 22650 RVA: 0x001FE73A File Offset: 0x001FC93A
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, string[] dlcIds) : this(id, ingredients, results)
	{
		this.dlcIds = dlcIds;
	}

	// Token: 0x0600587B RID: 22651 RVA: 0x001FE74D File Offset: 0x001FC94D
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP, int producedHEP, string[] dlcIds) : this(id, ingredients, results, consumedHEP, producedHEP)
	{
		this.dlcIds = dlcIds;
	}

	// Token: 0x0600587C RID: 22652 RVA: 0x001FE764 File Offset: 0x001FC964
	public float TotalResultUnits()
	{
		float num = 0f;
		foreach (ComplexRecipe.RecipeElement recipeElement in this.results)
		{
			num += recipeElement.amount;
		}
		return num;
	}

	// Token: 0x0600587D RID: 22653 RVA: 0x001FE79A File Offset: 0x001FC99A
	public bool RequiresTechUnlock()
	{
		return !string.IsNullOrEmpty(this.requiredTech);
	}

	// Token: 0x0600587E RID: 22654 RVA: 0x001FE7AA File Offset: 0x001FC9AA
	public bool IsRequiredTechUnlocked()
	{
		return string.IsNullOrEmpty(this.requiredTech) || Db.Get().Techs.Get(this.requiredTech).IsComplete();
	}

	// Token: 0x0600587F RID: 22655 RVA: 0x001FE7D8 File Offset: 0x001FC9D8
	public Sprite GetUIIcon()
	{
		Sprite result = null;
		KBatchedAnimController component = Assets.GetPrefab((this.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient) ? this.ingredients[0].material : this.results[0].material).GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
		}
		return result;
	}

	// Token: 0x06005880 RID: 22656 RVA: 0x001FE839 File Offset: 0x001FCA39
	public Color GetUIColor()
	{
		return Color.white;
	}

	// Token: 0x06005881 RID: 22657 RVA: 0x001FE840 File Offset: 0x001FCA40
	public string GetUIName(bool includeAmounts)
	{
		string text = this.results[0].facadeID.IsNullOrWhiteSpace() ? this.results[0].material.ProperName() : this.results[0].facadeID.ProperName();
		switch (this.nameDisplay)
		{
		case ComplexRecipe.RecipeNameDisplay.Result:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, text, this.results[0].amount);
			}
			return text;
		case ComplexRecipe.RecipeNameDisplay.IngredientToResult:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.ingredients[0].amount,
					this.results[0].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.ResultWithIngredient:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.ingredients[0].amount,
					this.results[0].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.Composite:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_COMPOSITE_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.results[1].material.ProperName(),
					this.ingredients[0].amount,
					this.results[0].amount,
					this.results[1].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_COMPOSITE, this.ingredients[0].material.ProperName(), text, this.results[1].material.ProperName());
		case ComplexRecipe.RecipeNameDisplay.HEP:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_HEP_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					this.results[1].material.ProperName(),
					this.ingredients[0].amount,
					this.producedHEP,
					this.results[1].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_HEP, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.Custom:
			return this.customName;
		}
		if (includeAmounts)
		{
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, this.ingredients[0].material.ProperName(), this.ingredients[0].amount);
		}
		return this.ingredients[0].material.ProperName();
	}

	// Token: 0x04003A17 RID: 14871
	public string id;

	// Token: 0x04003A18 RID: 14872
	public ComplexRecipe.RecipeElement[] ingredients;

	// Token: 0x04003A19 RID: 14873
	public ComplexRecipe.RecipeElement[] results;

	// Token: 0x04003A1A RID: 14874
	public float time;

	// Token: 0x04003A1B RID: 14875
	public GameObject FabricationVisualizer;

	// Token: 0x04003A1C RID: 14876
	public int consumedHEP;

	// Token: 0x04003A1D RID: 14877
	public int producedHEP;

	// Token: 0x04003A1E RID: 14878
	public string recipeCategoryID = "";

	// Token: 0x04003A1F RID: 14879
	private string[] dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;

	// Token: 0x04003A21 RID: 14881
	public ComplexRecipe.RecipeNameDisplay nameDisplay;

	// Token: 0x04003A22 RID: 14882
	public string customName;

	// Token: 0x04003A23 RID: 14883
	public string description;

	// Token: 0x04003A24 RID: 14884
	public List<Tag> fabricators;

	// Token: 0x04003A25 RID: 14885
	public int sortOrder;

	// Token: 0x04003A26 RID: 14886
	public string requiredTech;

	// Token: 0x02001BE0 RID: 7136
	public enum RecipeNameDisplay
	{
		// Token: 0x040080F5 RID: 33013
		Ingredient,
		// Token: 0x040080F6 RID: 33014
		Result,
		// Token: 0x040080F7 RID: 33015
		IngredientToResult,
		// Token: 0x040080F8 RID: 33016
		ResultWithIngredient,
		// Token: 0x040080F9 RID: 33017
		Composite,
		// Token: 0x040080FA RID: 33018
		HEP,
		// Token: 0x040080FB RID: 33019
		Custom
	}

	// Token: 0x02001BE1 RID: 7137
	public class RecipeElement
	{
		// Token: 0x0600A4BB RID: 42171 RVA: 0x0038D810 File Offset: 0x0038BA10
		public RecipeElement(Tag material, float amount, bool inheritElement)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
			this.inheritElement = inheritElement;
		}

		// Token: 0x0600A4BC RID: 42172 RVA: 0x0038D834 File Offset: 0x0038BA34
		public RecipeElement(Tag material, float amount)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
		}

		// Token: 0x0600A4BD RID: 42173 RVA: 0x0038D851 File Offset: 0x0038BA51
		public RecipeElement(Tag material, float amount, ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation, bool storeElement = false)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = temperatureOperation;
			this.storeElement = storeElement;
		}

		// Token: 0x0600A4BE RID: 42174 RVA: 0x0038D876 File Offset: 0x0038BA76
		public RecipeElement(Tag material, float amount, ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation, string facadeID, bool storeElement = false)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = temperatureOperation;
			this.storeElement = storeElement;
			this.facadeID = facadeID;
		}

		// Token: 0x0600A4BF RID: 42175 RVA: 0x0038D8A3 File Offset: 0x0038BAA3
		public RecipeElement(EdiblesManager.FoodInfo foodInfo, float amount)
		{
			this.material = foodInfo.Id;
			this.amount = amount;
			this.Edible = true;
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x0600A4C0 RID: 42176 RVA: 0x0038D8CA File Offset: 0x0038BACA
		// (set) Token: 0x0600A4C1 RID: 42177 RVA: 0x0038D8D2 File Offset: 0x0038BAD2
		public float amount { get; private set; }

		// Token: 0x040080FC RID: 33020
		public Tag material;

		// Token: 0x040080FE RID: 33022
		public ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation;

		// Token: 0x040080FF RID: 33023
		public bool storeElement;

		// Token: 0x04008100 RID: 33024
		public bool inheritElement;

		// Token: 0x04008101 RID: 33025
		public string facadeID;

		// Token: 0x04008102 RID: 33026
		public bool Edible;

		// Token: 0x02002631 RID: 9777
		public enum TemperatureOperation
		{
			// Token: 0x0400A9F3 RID: 43507
			AverageTemperature,
			// Token: 0x0400A9F4 RID: 43508
			Heated,
			// Token: 0x0400A9F5 RID: 43509
			Melted,
			// Token: 0x0400A9F6 RID: 43510
			Dehydrated
		}
	}
}
