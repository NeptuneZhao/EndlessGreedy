using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020007E2 RID: 2018
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConsumerManager")]
public class ConsumerManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060037CB RID: 14283 RVA: 0x001306BE File Offset: 0x0012E8BE
	public static void DestroyInstance()
	{
		ConsumerManager.instance = null;
	}

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x060037CC RID: 14284 RVA: 0x001306C8 File Offset: 0x0012E8C8
	// (remove) Token: 0x060037CD RID: 14285 RVA: 0x00130700 File Offset: 0x0012E900
	public event Action<Tag> OnDiscover;

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x060037CE RID: 14286 RVA: 0x00130735 File Offset: 0x0012E935
	public List<Tag> DefaultForbiddenTagsList
	{
		get
		{
			return this.defaultForbiddenTagsList;
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x060037CF RID: 14287 RVA: 0x00130740 File Offset: 0x0012E940
	public List<Tag> StandardDuplicantDietaryRestrictions
	{
		get
		{
			List<Tag> list = new List<Tag>();
			foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.ChargedPortableBattery))
			{
				list.Add(go.PrefabID());
			}
			return list;
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x060037D0 RID: 14288 RVA: 0x001307A4 File Offset: 0x0012E9A4
	public List<Tag> BionicDuplicantDietaryRestrictions
	{
		get
		{
			List<Tag> list = new List<Tag>();
			foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.Edible))
			{
				list.Add(go.PrefabID());
			}
			return list;
		}
	}

	// Token: 0x060037D1 RID: 14289 RVA: 0x00130808 File Offset: 0x0012EA08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ConsumerManager.instance = this;
		this.RefreshDiscovered(null);
		DiscoveredResources.Instance.OnDiscover += this.OnWorldInventoryDiscover;
		Game.Instance.Subscribe(-107300940, new Action<object>(this.RefreshDiscovered));
	}

	// Token: 0x060037D2 RID: 14290 RVA: 0x0013085A File Offset: 0x0012EA5A
	public bool isDiscovered(Tag id)
	{
		return !this.undiscoveredConsumableTags.Contains(id);
	}

	// Token: 0x060037D3 RID: 14291 RVA: 0x0013086B File Offset: 0x0012EA6B
	private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
	{
		if (this.undiscoveredConsumableTags.Contains(tag))
		{
			this.RefreshDiscovered(null);
		}
	}

	// Token: 0x060037D4 RID: 14292 RVA: 0x00130884 File Offset: 0x0012EA84
	public void RefreshDiscovered(object data = null)
	{
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			if (!this.ShouldBeDiscovered(foodInfo.Id.ToTag()) && !this.undiscoveredConsumableTags.Contains(foodInfo.Id.ToTag()))
			{
				this.undiscoveredConsumableTags.Add(foodInfo.Id.ToTag());
				if (this.OnDiscover != null)
				{
					this.OnDiscover("UndiscoveredSomething".ToTag());
				}
			}
			else if (this.undiscoveredConsumableTags.Contains(foodInfo.Id.ToTag()) && this.ShouldBeDiscovered(foodInfo.Id.ToTag()))
			{
				this.undiscoveredConsumableTags.Remove(foodInfo.Id.ToTag());
				if (this.OnDiscover != null)
				{
					this.OnDiscover(foodInfo.Id.ToTag());
				}
				if (!DiscoveredResources.Instance.IsDiscovered(foodInfo.Id.ToTag()))
				{
					if (foodInfo.CaloriesPerUnit == 0f)
					{
						DiscoveredResources.Instance.Discover(foodInfo.Id.ToTag(), GameTags.CookingIngredient);
					}
					else
					{
						DiscoveredResources.Instance.Discover(foodInfo.Id.ToTag(), GameTags.Edible);
					}
				}
			}
		}
	}

	// Token: 0x060037D5 RID: 14293 RVA: 0x00130A08 File Offset: 0x0012EC08
	private bool ShouldBeDiscovered(Tag food_id)
	{
		if (DiscoveredResources.Instance.IsDiscovered(food_id))
		{
			return true;
		}
		foreach (Recipe recipe in RecipeManager.Get().recipes)
		{
			if (recipe.Result == food_id)
			{
				foreach (string id in recipe.fabricators)
				{
					if (Db.Get().TechItems.IsTechItemComplete(id))
					{
						return true;
					}
				}
			}
		}
		foreach (Crop crop in Components.Crops.Items)
		{
			if (Grid.IsVisible(Grid.PosToCell(crop.gameObject)) && crop.cropId == food_id.Name)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0400219B RID: 8603
	public static ConsumerManager instance;

	// Token: 0x0400219D RID: 8605
	[Serialize]
	private List<Tag> undiscoveredConsumableTags = new List<Tag>();

	// Token: 0x0400219E RID: 8606
	[Serialize]
	private List<Tag> defaultForbiddenTagsList = new List<Tag>();
}
