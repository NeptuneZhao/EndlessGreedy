using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B7E RID: 2942
public class RecipeManager
{
	// Token: 0x06005863 RID: 22627 RVA: 0x001FE2DB File Offset: 0x001FC4DB
	public static RecipeManager Get()
	{
		if (RecipeManager._Instance == null)
		{
			RecipeManager._Instance = new RecipeManager();
		}
		return RecipeManager._Instance;
	}

	// Token: 0x06005864 RID: 22628 RVA: 0x001FE2F3 File Offset: 0x001FC4F3
	public static void DestroyInstance()
	{
		RecipeManager._Instance = null;
	}

	// Token: 0x06005865 RID: 22629 RVA: 0x001FE2FB File Offset: 0x001FC4FB
	public void Add(Recipe recipe)
	{
		this.recipes.Add(recipe);
		if (recipe.FabricationVisualizer != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(recipe.FabricationVisualizer);
		}
	}

	// Token: 0x04003A12 RID: 14866
	private static RecipeManager _Instance;

	// Token: 0x04003A13 RID: 14867
	public List<Recipe> recipes = new List<Recipe>();
}
