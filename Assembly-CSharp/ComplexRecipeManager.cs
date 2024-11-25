using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000B7F RID: 2943
public class ComplexRecipeManager
{
	// Token: 0x06005867 RID: 22631 RVA: 0x001FE335 File Offset: 0x001FC535
	public static ComplexRecipeManager Get()
	{
		if (ComplexRecipeManager._Instance == null)
		{
			ComplexRecipeManager._Instance = new ComplexRecipeManager();
		}
		return ComplexRecipeManager._Instance;
	}

	// Token: 0x06005868 RID: 22632 RVA: 0x001FE34D File Offset: 0x001FC54D
	public static void DestroyInstance()
	{
		ComplexRecipeManager._Instance = null;
	}

	// Token: 0x06005869 RID: 22633 RVA: 0x001FE358 File Offset: 0x001FC558
	public static string MakeObsoleteRecipeID(string fabricator, Tag signatureElement)
	{
		string str = "_";
		Tag tag = signatureElement;
		return fabricator + str + tag.ToString();
	}

	// Token: 0x0600586A RID: 22634 RVA: 0x001FE380 File Offset: 0x001FC580
	public static string MakeRecipeID(string fabricator, IList<ComplexRecipe.RecipeElement> inputs, IList<ComplexRecipe.RecipeElement> outputs)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(fabricator);
		stringBuilder.Append("_I");
		foreach (ComplexRecipe.RecipeElement recipeElement in inputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement.material.ToString());
		}
		stringBuilder.Append("_O");
		foreach (ComplexRecipe.RecipeElement recipeElement2 in outputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement2.material.ToString());
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600586B RID: 22635 RVA: 0x001FE468 File Offset: 0x001FC668
	public static string MakeRecipeID(string fabricator, IList<ComplexRecipe.RecipeElement> inputs, IList<ComplexRecipe.RecipeElement> outputs, string facadeID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(fabricator);
		stringBuilder.Append("_I");
		foreach (ComplexRecipe.RecipeElement recipeElement in inputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement.material.ToString());
		}
		stringBuilder.Append("_O");
		foreach (ComplexRecipe.RecipeElement recipeElement2 in outputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement2.material.ToString());
		}
		stringBuilder.Append("_" + facadeID);
		return stringBuilder.ToString();
	}

	// Token: 0x0600586C RID: 22636 RVA: 0x001FE560 File Offset: 0x001FC760
	public void Add(ComplexRecipe recipe)
	{
		using (List<ComplexRecipe>.Enumerator enumerator = this.recipes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.id == recipe.id)
				{
					global::Debug.LogError(string.Format("DUPLICATE RECIPE ID! '{0}' is being added to the recipe manager multiple times. This will result in the failure to save/load certain queued recipes at fabricators.", recipe.id));
				}
			}
		}
		this.recipes.Add(recipe);
		if (recipe.FabricationVisualizer != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(recipe.FabricationVisualizer);
		}
	}

	// Token: 0x0600586D RID: 22637 RVA: 0x001FE5F8 File Offset: 0x001FC7F8
	public ComplexRecipe GetRecipe(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		return this.recipes.Find((ComplexRecipe r) => r.id == id);
	}

	// Token: 0x0600586E RID: 22638 RVA: 0x001FE638 File Offset: 0x001FC838
	public void AddObsoleteIDMapping(string obsolete_id, string new_id)
	{
		this.obsoleteIDMapping[obsolete_id] = new_id;
	}

	// Token: 0x0600586F RID: 22639 RVA: 0x001FE648 File Offset: 0x001FC848
	public ComplexRecipe GetObsoleteRecipe(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		ComplexRecipe result = null;
		string id2 = null;
		if (this.obsoleteIDMapping.TryGetValue(id, out id2))
		{
			result = this.GetRecipe(id2);
		}
		return result;
	}

	// Token: 0x04003A14 RID: 14868
	private static ComplexRecipeManager _Instance;

	// Token: 0x04003A15 RID: 14869
	public List<ComplexRecipe> recipes = new List<ComplexRecipe>();

	// Token: 0x04003A16 RID: 14870
	private Dictionary<string, string> obsoleteIDMapping = new Dictionary<string, string>();
}
