using System;
using UnityEngine;

// Token: 0x02000A50 RID: 2640
public class ResearchType
{
	// Token: 0x06004C95 RID: 19605 RVA: 0x001B5D19 File Offset: 0x001B3F19
	public ResearchType(string id, string name, string description, Sprite sprite, Color color, Recipe.Ingredient[] fabricationIngredients, float fabricationTime, HashedString kAnim_ID, string[] fabricators, string recipeDescription)
	{
		this._id = id;
		this._name = name;
		this._description = description;
		this._sprite = sprite;
		this._color = color;
		this.CreatePrefab(fabricationIngredients, fabricationTime, kAnim_ID, fabricators, recipeDescription, color);
	}

	// Token: 0x06004C96 RID: 19606 RVA: 0x001B5D59 File Offset: 0x001B3F59
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06004C97 RID: 19607 RVA: 0x001B5D60 File Offset: 0x001B3F60
	public GameObject CreatePrefab(Recipe.Ingredient[] fabricationIngredients, float fabricationTime, HashedString kAnim_ID, string[] fabricators, string recipeDescription, Color color)
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity(this.id, this.name, this.description, 1f, true, Assets.GetAnim(kAnim_ID), "ui", Grid.SceneLayer.BuildingFront, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<ResearchPointObject>().TypeID = this.id;
		this._recipe = new Recipe(this.id, 1f, (SimHashes)0, this.name, recipeDescription, 0);
		this._recipe.SetFabricators(fabricators, fabricationTime);
		this._recipe.SetIcon(Assets.GetSprite("research_type_icon"), color);
		if (fabricationIngredients != null)
		{
			foreach (Recipe.Ingredient ingredient in fabricationIngredients)
			{
				this._recipe.AddIngredient(ingredient);
			}
		}
		return gameObject;
	}

	// Token: 0x06004C98 RID: 19608 RVA: 0x001B5E25 File Offset: 0x001B4025
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06004C99 RID: 19609 RVA: 0x001B5E27 File Offset: 0x001B4027
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06004C9A RID: 19610 RVA: 0x001B5E29 File Offset: 0x001B4029
	public string id
	{
		get
		{
			return this._id;
		}
	}

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06004C9B RID: 19611 RVA: 0x001B5E31 File Offset: 0x001B4031
	public string name
	{
		get
		{
			return this._name;
		}
	}

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06004C9C RID: 19612 RVA: 0x001B5E39 File Offset: 0x001B4039
	public string description
	{
		get
		{
			return this._description;
		}
	}

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x06004C9D RID: 19613 RVA: 0x001B5E41 File Offset: 0x001B4041
	public string recipe
	{
		get
		{
			return this.recipe;
		}
	}

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x06004C9E RID: 19614 RVA: 0x001B5E49 File Offset: 0x001B4049
	public Color color
	{
		get
		{
			return this._color;
		}
	}

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06004C9F RID: 19615 RVA: 0x001B5E51 File Offset: 0x001B4051
	public Sprite sprite
	{
		get
		{
			return this._sprite;
		}
	}

	// Token: 0x040032ED RID: 13037
	private string _id;

	// Token: 0x040032EE RID: 13038
	private string _name;

	// Token: 0x040032EF RID: 13039
	private string _description;

	// Token: 0x040032F0 RID: 13040
	private Recipe _recipe;

	// Token: 0x040032F1 RID: 13041
	private Sprite _sprite;

	// Token: 0x040032F2 RID: 13042
	private Color _color;
}
