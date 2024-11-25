using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000A16 RID: 2582
[DebuggerDisplay("{Name}")]
public class Recipe : IHasSortOrder
{
	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x06004AE6 RID: 19174 RVA: 0x001AC415 File Offset: 0x001AA615
	// (set) Token: 0x06004AE7 RID: 19175 RVA: 0x001AC41D File Offset: 0x001AA61D
	public int sortOrder { get; set; }

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x06004AE9 RID: 19177 RVA: 0x001AC42F File Offset: 0x001AA62F
	// (set) Token: 0x06004AE8 RID: 19176 RVA: 0x001AC426 File Offset: 0x001AA626
	public string Name
	{
		get
		{
			if (this.nameOverride != null)
			{
				return this.nameOverride;
			}
			return this.Result.ProperName();
		}
		set
		{
			this.nameOverride = value;
		}
	}

	// Token: 0x06004AEA RID: 19178 RVA: 0x001AC44B File Offset: 0x001AA64B
	public Recipe()
	{
	}

	// Token: 0x06004AEB RID: 19179 RVA: 0x001AC460 File Offset: 0x001AA660
	public Recipe(string prefabId, float outputUnits = 1f, SimHashes elementOverride = (SimHashes)0, string nameOverride = null, string recipeDescription = null, int sortOrder = 0)
	{
		global::Debug.Assert(prefabId != null);
		this.Result = TagManager.Create(prefabId);
		this.ResultElementOverride = elementOverride;
		this.nameOverride = nameOverride;
		this.OutputUnits = outputUnits;
		this.Ingredients = new List<Recipe.Ingredient>();
		this.recipeDescription = recipeDescription;
		this.sortOrder = sortOrder;
		this.FabricationVisualizer = null;
	}

	// Token: 0x06004AEC RID: 19180 RVA: 0x001AC4CB File Offset: 0x001AA6CB
	public Recipe SetFabricator(string fabricator, float fabricationTime)
	{
		this.fabricators = new string[]
		{
			fabricator
		};
		this.FabricationTime = fabricationTime;
		RecipeManager.Get().Add(this);
		return this;
	}

	// Token: 0x06004AED RID: 19181 RVA: 0x001AC4F0 File Offset: 0x001AA6F0
	public Recipe SetFabricators(string[] fabricators, float fabricationTime)
	{
		this.fabricators = fabricators;
		this.FabricationTime = fabricationTime;
		RecipeManager.Get().Add(this);
		return this;
	}

	// Token: 0x06004AEE RID: 19182 RVA: 0x001AC50C File Offset: 0x001AA70C
	public Recipe SetIcon(Sprite Icon)
	{
		this.Icon = Icon;
		this.IconColor = Color.white;
		return this;
	}

	// Token: 0x06004AEF RID: 19183 RVA: 0x001AC521 File Offset: 0x001AA721
	public Recipe SetIcon(Sprite Icon, Color IconColor)
	{
		this.Icon = Icon;
		this.IconColor = IconColor;
		return this;
	}

	// Token: 0x06004AF0 RID: 19184 RVA: 0x001AC532 File Offset: 0x001AA732
	public Recipe AddIngredient(Recipe.Ingredient ingredient)
	{
		this.Ingredients.Add(ingredient);
		return this;
	}

	// Token: 0x06004AF1 RID: 19185 RVA: 0x001AC544 File Offset: 0x001AA744
	public Recipe.Ingredient[] GetAllIngredients(IList<Tag> selectedTags)
	{
		List<Recipe.Ingredient> list = new List<Recipe.Ingredient>();
		for (int i = 0; i < this.Ingredients.Count; i++)
		{
			float amount = this.Ingredients[i].amount;
			if (i < selectedTags.Count)
			{
				list.Add(new Recipe.Ingredient(selectedTags[i], amount));
			}
			else
			{
				list.Add(new Recipe.Ingredient(this.Ingredients[i].tag, amount));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06004AF2 RID: 19186 RVA: 0x001AC5C0 File Offset: 0x001AA7C0
	public Recipe.Ingredient[] GetAllIngredients(IList<Element> selected_elements)
	{
		List<Recipe.Ingredient> list = new List<Recipe.Ingredient>();
		for (int i = 0; i < this.Ingredients.Count; i++)
		{
			int num = (int)this.Ingredients[i].amount;
			bool flag = false;
			if (i < selected_elements.Count)
			{
				Element element = selected_elements[i];
				if (element != null && element.HasTag(this.Ingredients[i].tag))
				{
					list.Add(new Recipe.Ingredient(GameTagExtensions.Create(element.id), (float)num));
					flag = true;
				}
			}
			if (!flag)
			{
				list.Add(new Recipe.Ingredient(this.Ingredients[i].tag, (float)num));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06004AF3 RID: 19187 RVA: 0x001AC678 File Offset: 0x001AA878
	public GameObject Craft(Storage resource_storage, IList<Tag> selectedTags)
	{
		Recipe.Ingredient[] allIngredients = this.GetAllIngredients(selectedTags);
		return this.CraftRecipe(resource_storage, allIngredients);
	}

	// Token: 0x06004AF4 RID: 19188 RVA: 0x001AC698 File Offset: 0x001AA898
	private GameObject CraftRecipe(Storage resource_storage, Recipe.Ingredient[] ingredientTags)
	{
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		float num = 0f;
		float num2 = 0f;
		foreach (Recipe.Ingredient ingredient in ingredientTags)
		{
			GameObject gameObject = resource_storage.FindFirst(ingredient.tag);
			if (gameObject != null)
			{
				Edible component = gameObject.GetComponent<Edible>();
				if (component)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED, "{0}", component.GetProperName()), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
				}
			}
			SimUtil.DiseaseInfo b;
			float temp;
			resource_storage.ConsumeAndGetDisease(ingredient, out b, out temp);
			diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo, b);
			num = SimUtil.CalculateFinalTemperature(num2, num, ingredient.amount, temp);
			num2 += ingredient.amount;
		}
		GameObject prefab = Assets.GetPrefab(this.Result);
		GameObject gameObject2 = null;
		if (prefab != null)
		{
			gameObject2 = GameUtil.KInstantiate(prefab, Grid.SceneLayer.Ore, null, 0);
			PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
			gameObject2.GetComponent<KSelectable>().entityName = this.Name;
			if (component2 != null)
			{
				gameObject2.GetComponent<KPrefabID>().RemoveTag(TagManager.Create("Vacuum"));
				if (this.ResultElementOverride != (SimHashes)0)
				{
					if (component2.GetComponent<ElementChunk>() != null)
					{
						component2.SetElement(this.ResultElementOverride, true);
					}
					else
					{
						component2.ElementID = this.ResultElementOverride;
					}
				}
				component2.Temperature = num;
				component2.Units = this.OutputUnits;
			}
			Edible component3 = gameObject2.GetComponent<Edible>();
			if (component3)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component3.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED, "{0}", component3.GetProperName()), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
			}
			gameObject2.SetActive(true);
			if (component2 != null)
			{
				component2.AddDisease(diseaseInfo.idx, diseaseInfo.count, "Recipe.CraftRecipe");
			}
			gameObject2.GetComponent<KMonoBehaviour>().Trigger(748399584, null);
		}
		return gameObject2;
	}

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x06004AF5 RID: 19189 RVA: 0x001AC8A0 File Offset: 0x001AAAA0
	public string[] MaterialOptionNames
	{
		get
		{
			List<string> list = new List<string>();
			foreach (Element element in ElementLoader.elements)
			{
				if (Array.IndexOf<Tag>(element.oreTags, this.Ingredients[0].tag) >= 0)
				{
					list.Add(element.id.ToString());
				}
			}
			return list.ToArray();
		}
	}

	// Token: 0x06004AF6 RID: 19190 RVA: 0x001AC930 File Offset: 0x001AAB30
	public Element[] MaterialOptions()
	{
		List<Element> list = new List<Element>();
		foreach (Element element in ElementLoader.elements)
		{
			if (Array.IndexOf<Tag>(element.oreTags, this.Ingredients[0].tag) >= 0)
			{
				list.Add(element);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06004AF7 RID: 19191 RVA: 0x001AC9B0 File Offset: 0x001AABB0
	public BuildingDef GetBuildingDef()
	{
		BuildingComplete component = Assets.GetPrefab(this.Result).GetComponent<BuildingComplete>();
		if (component != null)
		{
			return component.Def;
		}
		return null;
	}

	// Token: 0x06004AF8 RID: 19192 RVA: 0x001AC9E0 File Offset: 0x001AABE0
	public Sprite GetUIIcon()
	{
		Sprite result = null;
		if (this.Icon != null)
		{
			result = this.Icon;
		}
		else
		{
			KBatchedAnimController component = Assets.GetPrefab(this.Result).GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
			}
		}
		return result;
	}

	// Token: 0x06004AF9 RID: 19193 RVA: 0x001ACA3A File Offset: 0x001AAC3A
	public Color GetUIColor()
	{
		if (!(this.Icon != null))
		{
			return Color.white;
		}
		return this.IconColor;
	}

	// Token: 0x04003113 RID: 12563
	private string nameOverride;

	// Token: 0x04003114 RID: 12564
	public string HotKey;

	// Token: 0x04003115 RID: 12565
	public string Type;

	// Token: 0x04003116 RID: 12566
	public List<Recipe.Ingredient> Ingredients;

	// Token: 0x04003117 RID: 12567
	public string recipeDescription;

	// Token: 0x04003118 RID: 12568
	public Tag Result;

	// Token: 0x04003119 RID: 12569
	public GameObject FabricationVisualizer;

	// Token: 0x0400311A RID: 12570
	public SimHashes ResultElementOverride;

	// Token: 0x0400311B RID: 12571
	public Sprite Icon;

	// Token: 0x0400311C RID: 12572
	public Color IconColor = Color.white;

	// Token: 0x0400311D RID: 12573
	public string[] fabricators;

	// Token: 0x0400311E RID: 12574
	public float OutputUnits;

	// Token: 0x0400311F RID: 12575
	public float FabricationTime;

	// Token: 0x04003120 RID: 12576
	public string TechUnlock;

	// Token: 0x02001A33 RID: 6707
	[DebuggerDisplay("{tag} {amount}")]
	[Serializable]
	public class Ingredient
	{
		// Token: 0x06009F50 RID: 40784 RVA: 0x0037BFDF File Offset: 0x0037A1DF
		public Ingredient(string tag, float amount)
		{
			this.tag = TagManager.Create(tag);
			this.amount = amount;
		}

		// Token: 0x06009F51 RID: 40785 RVA: 0x0037BFFA File Offset: 0x0037A1FA
		public Ingredient(Tag tag, float amount)
		{
			this.tag = tag;
			this.amount = amount;
		}

		// Token: 0x06009F52 RID: 40786 RVA: 0x0037C010 File Offset: 0x0037A210
		public List<Element> GetElementOptions()
		{
			List<Element> list = new List<Element>(ElementLoader.elements);
			list.RemoveAll((Element e) => !e.IsSolid);
			list.RemoveAll((Element e) => !e.HasTag(this.tag));
			return list;
		}

		// Token: 0x04007BB8 RID: 31672
		public Tag tag;

		// Token: 0x04007BB9 RID: 31673
		public float amount;
	}
}
