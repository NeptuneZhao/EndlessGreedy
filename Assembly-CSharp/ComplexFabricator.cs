using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000699 RID: 1689
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ComplexFabricator")]
public class ComplexFabricator : RemoteDockWorkTargetComponent, ISim200ms, ISim1000ms
{
	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06002A2C RID: 10796 RVA: 0x000EDF9E File Offset: 0x000EC19E
	public ComplexFabricatorWorkable Workable
	{
		get
		{
			return this.workable;
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06002A2D RID: 10797 RVA: 0x000EDFA6 File Offset: 0x000EC1A6
	// (set) Token: 0x06002A2E RID: 10798 RVA: 0x000EDFAE File Offset: 0x000EC1AE
	public bool ForbidMutantSeeds
	{
		get
		{
			return this.forbidMutantSeeds;
		}
		set
		{
			this.forbidMutantSeeds = value;
			this.ToggleMutantSeedFetches();
			this.UpdateMutantSeedStatusItem();
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06002A2F RID: 10799 RVA: 0x000EDFC3 File Offset: 0x000EC1C3
	public Tag[] ForbiddenTags
	{
		get
		{
			if (!this.forbidMutantSeeds)
			{
				return null;
			}
			return this.forbiddenMutantTags;
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06002A30 RID: 10800 RVA: 0x000EDFD5 File Offset: 0x000EC1D5
	public int CurrentOrderIdx
	{
		get
		{
			return this.nextOrderIdx;
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06002A31 RID: 10801 RVA: 0x000EDFDD File Offset: 0x000EC1DD
	public ComplexRecipe CurrentWorkingOrder
	{
		get
		{
			if (!this.HasWorkingOrder)
			{
				return null;
			}
			return this.recipe_list[this.workingOrderIdx];
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06002A32 RID: 10802 RVA: 0x000EDFF6 File Offset: 0x000EC1F6
	public ComplexRecipe NextOrder
	{
		get
		{
			if (!this.nextOrderIsWorkable)
			{
				return null;
			}
			return this.recipe_list[this.nextOrderIdx];
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06002A33 RID: 10803 RVA: 0x000EE00F File Offset: 0x000EC20F
	// (set) Token: 0x06002A34 RID: 10804 RVA: 0x000EE017 File Offset: 0x000EC217
	public float OrderProgress
	{
		get
		{
			return this.orderProgress;
		}
		set
		{
			this.orderProgress = value;
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06002A35 RID: 10805 RVA: 0x000EE020 File Offset: 0x000EC220
	public bool HasAnyOrder
	{
		get
		{
			return this.HasWorkingOrder || this.hasOpenOrders;
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06002A36 RID: 10806 RVA: 0x000EE032 File Offset: 0x000EC232
	public bool HasWorker
	{
		get
		{
			return !this.duplicantOperated || this.workable.worker != null;
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06002A37 RID: 10807 RVA: 0x000EE04F File Offset: 0x000EC24F
	public bool WaitingForWorker
	{
		get
		{
			return this.HasWorkingOrder && !this.HasWorker;
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06002A38 RID: 10808 RVA: 0x000EE064 File Offset: 0x000EC264
	private bool HasWorkingOrder
	{
		get
		{
			return this.workingOrderIdx > -1;
		}
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06002A39 RID: 10809 RVA: 0x000EE06F File Offset: 0x000EC26F
	public List<FetchList2> DebugFetchLists
	{
		get
		{
			return this.fetchListList;
		}
	}

	// Token: 0x06002A3A RID: 10810 RVA: 0x000EE078 File Offset: 0x000EC278
	[OnDeserialized]
	protected virtual void OnDeserializedMethod()
	{
		List<string> list = new List<string>();
		foreach (string text in this.recipeQueueCounts.Keys)
		{
			if (ComplexRecipeManager.Get().GetRecipe(text) == null)
			{
				list.Add(text);
			}
		}
		foreach (string text2 in list)
		{
			global::Debug.LogWarningFormat("{1} removing missing recipe from queue: {0}", new object[]
			{
				text2,
				base.name
			});
			this.recipeQueueCounts.Remove(text2);
		}
	}

	// Token: 0x06002A3B RID: 10811 RVA: 0x000EE148 File Offset: 0x000EC348
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.GetRecipes();
		this.simRenderLoadBalance = true;
		this.choreType = Db.Get().ChoreTypes.Fabricate;
		base.Subscribe<ComplexFabricator>(-1957399615, ComplexFabricator.OnDroppedAllDelegate);
		base.Subscribe<ComplexFabricator>(-592767678, ComplexFabricator.OnOperationalChangedDelegate);
		base.Subscribe<ComplexFabricator>(-905833192, ComplexFabricator.OnCopySettingsDelegate);
		base.Subscribe<ComplexFabricator>(-1697596308, ComplexFabricator.OnStorageChangeDelegate);
		base.Subscribe<ComplexFabricator>(-1837862626, ComplexFabricator.OnParticleStorageChangedDelegate);
		this.workable = base.GetComponent<ComplexFabricatorWorkable>();
		Components.ComplexFabricators.Add(this);
		base.Subscribe<ComplexFabricator>(493375141, ComplexFabricator.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06002A3C RID: 10812 RVA: 0x000EE1FC File Offset: 0x000EC3FC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.InitRecipeQueueCount();
		foreach (string key in this.recipeQueueCounts.Keys)
		{
			if (this.recipeQueueCounts[key] == 100)
			{
				this.recipeQueueCounts[key] = ComplexFabricator.QUEUE_INFINITE;
			}
		}
		this.buildStorage.Transfer(this.inStorage, true, true);
		this.DropExcessIngredients(this.inStorage);
		int num = this.FindRecipeIndex(this.lastWorkingRecipe);
		if (num > -1)
		{
			this.nextOrderIdx = num;
		}
		this.UpdateMutantSeedStatusItem();
	}

	// Token: 0x06002A3D RID: 10813 RVA: 0x000EE2B8 File Offset: 0x000EC4B8
	protected override void OnCleanUp()
	{
		this.CancelAllOpenOrders();
		this.CancelChore();
		Components.ComplexFabricators.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002A3E RID: 10814 RVA: 0x000EE2D8 File Offset: 0x000EC4D8
	private void OnRefreshUserMenu(object data)
	{
		if (SaveLoader.Instance.IsDLCActiveForCurrentSave("EXPANSION1_ID") && this.HasRecipiesWithSeeds())
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", this.ForbidMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT, delegate()
			{
				this.ForbidMutantSeeds = !this.ForbidMutantSeeds;
				this.OnRefreshUserMenu(null);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.TOOLTIP, true), 1f);
		}
	}

	// Token: 0x06002A3F RID: 10815 RVA: 0x000EE35C File Offset: 0x000EC55C
	private bool HasRecipiesWithSeeds()
	{
		bool result = false;
		ComplexRecipe[] array = this.recipe_list;
		for (int i = 0; i < array.Length; i++)
		{
			ComplexRecipe.RecipeElement[] ingredients = array[i].ingredients;
			for (int j = 0; j < ingredients.Length; j++)
			{
				GameObject prefab = Assets.GetPrefab(ingredients[j].material);
				if (prefab != null && prefab.GetComponent<PlantableSeed>() != null)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06002A40 RID: 10816 RVA: 0x000EE3CC File Offset: 0x000EC5CC
	private void UpdateMutantSeedStatusItem()
	{
		base.gameObject.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.FabricatorAcceptsMutantSeeds, SaveLoader.Instance.IsDLCActiveForCurrentSave("EXPANSION1_ID") && this.HasRecipiesWithSeeds() && !this.forbidMutantSeeds, null);
	}

	// Token: 0x06002A41 RID: 10817 RVA: 0x000EE422 File Offset: 0x000EC622
	private void OnOperationalChanged(object data)
	{
		if ((bool)data)
		{
			this.queueDirty = true;
		}
		else
		{
			this.CancelAllOpenOrders();
		}
		this.UpdateChore();
	}

	// Token: 0x06002A42 RID: 10818 RVA: 0x000EE444 File Offset: 0x000EC644
	public virtual void Sim1000ms(float dt)
	{
		this.RefreshAndStartNextOrder();
		if (this.materialNeedCache.Count > 0 && this.fetchListList.Count == 0)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} has material needs cached, but no open fetches. materialNeedCache={1}, fetchListList={2}", new object[]
			{
				base.gameObject,
				this.materialNeedCache.Count,
				this.fetchListList.Count
			});
			this.queueDirty = true;
		}
	}

	// Token: 0x06002A43 RID: 10819 RVA: 0x000EE4BE File Offset: 0x000EC6BE
	protected virtual float ComputeWorkProgress(float dt, ComplexRecipe recipe)
	{
		return dt / recipe.time;
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x000EE4C8 File Offset: 0x000EC6C8
	public void Sim200ms(float dt)
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		this.operational.SetActive(this.HasWorkingOrder && this.HasWorker, false);
		if (!this.duplicantOperated && this.HasWorkingOrder)
		{
			this.orderProgress += this.ComputeWorkProgress(dt, this.recipe_list[this.workingOrderIdx]);
			if (this.orderProgress >= 1f)
			{
				this.ShowProgressBar(false);
				this.CompleteWorkingOrder();
			}
		}
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x000EE54C File Offset: 0x000EC74C
	private void RefreshAndStartNextOrder()
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		if (this.queueDirty)
		{
			this.RefreshQueue();
		}
		if (!this.HasWorkingOrder && this.nextOrderIsWorkable)
		{
			this.ShowProgressBar(true);
			this.StartWorkingOrder(this.nextOrderIdx);
		}
	}

	// Token: 0x06002A46 RID: 10822 RVA: 0x000EE598 File Offset: 0x000EC798
	public virtual float GetPercentComplete()
	{
		return this.orderProgress;
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x000EE5A0 File Offset: 0x000EC7A0
	private void ShowProgressBar(bool show)
	{
		if (show && this.showProgressBar && !this.duplicantOperated)
		{
			if (this.progressBar == null)
			{
				this.progressBar = ProgressBar.CreateProgressBar(base.gameObject, new Func<float>(this.GetPercentComplete));
			}
			this.progressBar.enabled = true;
			this.progressBar.SetVisibility(true);
			return;
		}
		if (this.progressBar != null)
		{
			this.progressBar.gameObject.DeleteObject();
			this.progressBar = null;
		}
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x000EE62A File Offset: 0x000EC82A
	public void SetQueueDirty()
	{
		this.queueDirty = true;
	}

	// Token: 0x06002A49 RID: 10825 RVA: 0x000EE633 File Offset: 0x000EC833
	private void RefreshQueue()
	{
		this.queueDirty = false;
		this.ValidateWorkingOrder();
		this.ValidateNextOrder();
		this.UpdateOpenOrders();
		this.DropExcessIngredients(this.inStorage);
		base.Trigger(1721324763, this);
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x000EE668 File Offset: 0x000EC868
	private void StartWorkingOrder(int index)
	{
		global::Debug.Assert(!this.HasWorkingOrder, "machineOrderIdx already set");
		this.workingOrderIdx = index;
		if (this.recipe_list[this.workingOrderIdx].id != this.lastWorkingRecipe)
		{
			this.orderProgress = 0f;
			this.lastWorkingRecipe = this.recipe_list[this.workingOrderIdx].id;
		}
		this.TransferCurrentRecipeIngredientsForBuild();
		global::Debug.Assert(this.openOrderCounts[this.workingOrderIdx] > 0, "openOrderCount invalid");
		List<int> list = this.openOrderCounts;
		int index2 = this.workingOrderIdx;
		int num = list[index2];
		list[index2] = num - 1;
		this.UpdateChore();
		base.Trigger(2023536846, this.recipe_list[this.workingOrderIdx]);
		this.AdvanceNextOrder();
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x000EE737 File Offset: 0x000EC937
	private void CancelWorkingOrder()
	{
		global::Debug.Assert(this.HasWorkingOrder, "machineOrderIdx not set");
		this.buildStorage.Transfer(this.inStorage, true, true);
		this.workingOrderIdx = -1;
		this.orderProgress = 0f;
		this.UpdateChore();
	}

	// Token: 0x06002A4C RID: 10828 RVA: 0x000EE774 File Offset: 0x000EC974
	public void CompleteWorkingOrder()
	{
		if (!this.HasWorkingOrder)
		{
			global::Debug.LogWarning("CompleteWorkingOrder called with no working order.", base.gameObject);
			return;
		}
		ComplexRecipe complexRecipe = this.recipe_list[this.workingOrderIdx];
		this.SpawnOrderProduct(complexRecipe);
		float num = this.buildStorage.MassStored();
		if (num != 0f)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} build storage contains mass {1} after order completion.", new object[]
			{
				base.gameObject,
				num
			});
			this.buildStorage.Transfer(this.inStorage, true, true);
		}
		this.DecrementRecipeQueueCountInternal(complexRecipe, true);
		this.workingOrderIdx = -1;
		this.orderProgress = 0f;
		this.CancelChore();
		base.Trigger(1355439576, complexRecipe);
		if (!this.cancelling)
		{
			this.RefreshAndStartNextOrder();
		}
	}

	// Token: 0x06002A4D RID: 10829 RVA: 0x000EE83C File Offset: 0x000ECA3C
	private void ValidateWorkingOrder()
	{
		if (!this.HasWorkingOrder)
		{
			return;
		}
		ComplexRecipe recipe = this.recipe_list[this.workingOrderIdx];
		if (!this.IsRecipeQueued(recipe))
		{
			this.CancelWorkingOrder();
		}
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x000EE870 File Offset: 0x000ECA70
	private void UpdateChore()
	{
		if (!this.duplicantOperated)
		{
			return;
		}
		bool flag = this.operational.IsOperational && this.HasWorkingOrder;
		if (flag && this.chore == null)
		{
			this.CreateChore();
			return;
		}
		if (!flag && this.chore != null)
		{
			this.CancelChore();
		}
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x000EE8C0 File Offset: 0x000ECAC0
	private void AdvanceNextOrder()
	{
		for (int i = 0; i < this.recipe_list.Length; i++)
		{
			this.nextOrderIdx = (this.nextOrderIdx + 1) % this.recipe_list.Length;
			ComplexRecipe recipe = this.recipe_list[this.nextOrderIdx];
			this.nextOrderIsWorkable = (this.GetRemainingQueueCount(recipe) > 0 && this.HasIngredients(recipe, this.inStorage));
			if (this.nextOrderIsWorkable)
			{
				break;
			}
		}
	}

	// Token: 0x06002A50 RID: 10832 RVA: 0x000EE930 File Offset: 0x000ECB30
	private void ValidateNextOrder()
	{
		ComplexRecipe recipe = this.recipe_list[this.nextOrderIdx];
		this.nextOrderIsWorkable = (this.GetRemainingQueueCount(recipe) > 0 && this.HasIngredients(recipe, this.inStorage));
		if (!this.nextOrderIsWorkable)
		{
			this.AdvanceNextOrder();
		}
	}

	// Token: 0x06002A51 RID: 10833 RVA: 0x000EE97C File Offset: 0x000ECB7C
	private void CancelAllOpenOrders()
	{
		for (int i = 0; i < this.openOrderCounts.Count; i++)
		{
			this.openOrderCounts[i] = 0;
		}
		this.ClearMaterialNeeds();
		this.CancelFetches();
	}

	// Token: 0x06002A52 RID: 10834 RVA: 0x000EE9B8 File Offset: 0x000ECBB8
	private void UpdateOpenOrders()
	{
		ComplexRecipe[] recipes = this.GetRecipes();
		if (recipes.Length != this.openOrderCounts.Count)
		{
			global::Debug.LogErrorFormat(base.gameObject, "Recipe count {0} doesn't match open order count {1}", new object[]
			{
				recipes.Length,
				this.openOrderCounts.Count
			});
		}
		bool flag = false;
		this.hasOpenOrders = false;
		for (int i = 0; i < recipes.Length; i++)
		{
			ComplexRecipe recipe = recipes[i];
			int recipePrefetchCount = this.GetRecipePrefetchCount(recipe);
			if (recipePrefetchCount > 0)
			{
				this.hasOpenOrders = true;
			}
			int num = this.openOrderCounts[i];
			if (num != recipePrefetchCount)
			{
				if (recipePrefetchCount < num)
				{
					flag = true;
				}
				this.openOrderCounts[i] = recipePrefetchCount;
			}
		}
		DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
		DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary2 = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
		for (int j = 0; j < this.openOrderCounts.Count; j++)
		{
			if (this.openOrderCounts[j] > 0)
			{
				foreach (ComplexRecipe.RecipeElement recipeElement in this.recipe_list[j].ingredients)
				{
					pooledDictionary[recipeElement.material] = this.inStorage.GetAmountAvailable(recipeElement.material);
				}
			}
		}
		for (int l = 0; l < this.recipe_list.Length; l++)
		{
			int num2 = this.openOrderCounts[l];
			if (num2 > 0)
			{
				foreach (ComplexRecipe.RecipeElement recipeElement2 in this.recipe_list[l].ingredients)
				{
					float num3 = recipeElement2.amount * (float)num2;
					float num4 = num3 - pooledDictionary[recipeElement2.material];
					if (num4 > 0f)
					{
						float num5;
						pooledDictionary2.TryGetValue(recipeElement2.material, out num5);
						pooledDictionary2[recipeElement2.material] = num5 + num4;
						pooledDictionary[recipeElement2.material] = 0f;
					}
					else
					{
						DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary3 = pooledDictionary;
						Tag material = recipeElement2.material;
						pooledDictionary3[material] -= num3;
					}
				}
			}
		}
		if (flag)
		{
			this.CancelFetches();
		}
		if (pooledDictionary2.Count > 0)
		{
			this.UpdateFetches(pooledDictionary2);
		}
		this.UpdateMaterialNeeds(pooledDictionary2);
		pooledDictionary2.Recycle();
		pooledDictionary.Recycle();
	}

	// Token: 0x06002A53 RID: 10835 RVA: 0x000EEC04 File Offset: 0x000ECE04
	private void UpdateMaterialNeeds(Dictionary<Tag, float> missingAmounts)
	{
		this.ClearMaterialNeeds();
		foreach (KeyValuePair<Tag, float> keyValuePair in missingAmounts)
		{
			MaterialNeeds.UpdateNeed(keyValuePair.Key, keyValuePair.Value, base.gameObject.GetMyWorldId());
			this.materialNeedCache.Add(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06002A54 RID: 10836 RVA: 0x000EEC88 File Offset: 0x000ECE88
	private void ClearMaterialNeeds()
	{
		foreach (KeyValuePair<Tag, float> keyValuePair in this.materialNeedCache)
		{
			MaterialNeeds.UpdateNeed(keyValuePair.Key, -keyValuePair.Value, base.gameObject.GetMyWorldId());
		}
		this.materialNeedCache.Clear();
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x000EED00 File Offset: 0x000ECF00
	public int HighestHEPQueued()
	{
		int num = 0;
		foreach (KeyValuePair<string, int> keyValuePair in this.recipeQueueCounts)
		{
			if (keyValuePair.Value > 0)
			{
				num = Math.Max(this.recipe_list[this.FindRecipeIndex(keyValuePair.Key)].consumedHEP, num);
			}
		}
		return num;
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x000EED7C File Offset: 0x000ECF7C
	private void OnFetchComplete()
	{
		for (int i = this.fetchListList.Count - 1; i >= 0; i--)
		{
			if (this.fetchListList[i].IsComplete)
			{
				this.fetchListList.RemoveAt(i);
				this.queueDirty = true;
			}
		}
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x000EEDC7 File Offset: 0x000ECFC7
	private void OnStorageChange(object data)
	{
		this.queueDirty = true;
	}

	// Token: 0x06002A58 RID: 10840 RVA: 0x000EEDD0 File Offset: 0x000ECFD0
	private void OnDroppedAll(object data)
	{
		if (this.HasWorkingOrder)
		{
			this.CancelWorkingOrder();
		}
		this.CancelAllOpenOrders();
		this.RefreshQueue();
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x000EEDEC File Offset: 0x000ECFEC
	private void DropExcessIngredients(Storage storage)
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		if (this.keepAdditionalTag != Tag.Invalid)
		{
			hashSet.Add(this.keepAdditionalTag);
		}
		for (int i = 0; i < this.recipe_list.Length; i++)
		{
			ComplexRecipe complexRecipe = this.recipe_list[i];
			if (this.IsRecipeQueued(complexRecipe))
			{
				foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
				{
					hashSet.Add(recipeElement.material);
				}
			}
		}
		for (int k = storage.items.Count - 1; k >= 0; k--)
		{
			GameObject gameObject = storage.items[k];
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && (!this.keepExcessLiquids || !component.Element.IsLiquid))
				{
					KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
					if (component2 && !hashSet.Contains(component2.PrefabID()))
					{
						storage.Drop(gameObject, true);
					}
				}
			}
		}
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x000EEEFC File Offset: 0x000ED0FC
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		ComplexFabricator component = gameObject.GetComponent<ComplexFabricator>();
		if (component == null)
		{
			return;
		}
		this.ForbidMutantSeeds = component.ForbidMutantSeeds;
		foreach (ComplexRecipe complexRecipe in this.recipe_list)
		{
			int count;
			if (!component.recipeQueueCounts.TryGetValue(complexRecipe.id, out count))
			{
				count = 0;
			}
			this.SetRecipeQueueCountInternal(complexRecipe, count);
		}
		this.RefreshQueue();
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x000EEF7A File Offset: 0x000ED17A
	private int CompareRecipe(ComplexRecipe a, ComplexRecipe b)
	{
		if (a.sortOrder != b.sortOrder)
		{
			return a.sortOrder - b.sortOrder;
		}
		return StringComparer.InvariantCulture.Compare(a.id, b.id);
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x000EEFB0 File Offset: 0x000ED1B0
	public ComplexRecipe[] GetRecipes()
	{
		if (this.recipe_list == null)
		{
			Tag prefabTag = base.GetComponent<KPrefabID>().PrefabTag;
			List<ComplexRecipe> recipes = ComplexRecipeManager.Get().recipes;
			List<ComplexRecipe> list = new List<ComplexRecipe>();
			foreach (ComplexRecipe complexRecipe in recipes)
			{
				using (List<Tag>.Enumerator enumerator2 = complexRecipe.fabricators.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current == prefabTag && SaveLoader.Instance.IsDlcListActiveForCurrentSave(complexRecipe.GetDlcIds()))
						{
							list.Add(complexRecipe);
						}
					}
				}
			}
			this.recipe_list = list.ToArray();
			Array.Sort<ComplexRecipe>(this.recipe_list, new Comparison<ComplexRecipe>(this.CompareRecipe));
		}
		return this.recipe_list;
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x000EF0A4 File Offset: 0x000ED2A4
	private void InitRecipeQueueCount()
	{
		foreach (ComplexRecipe complexRecipe in this.GetRecipes())
		{
			bool flag = false;
			using (Dictionary<string, int>.KeyCollection.Enumerator enumerator = this.recipeQueueCounts.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == complexRecipe.id)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.recipeQueueCounts.Add(complexRecipe.id, 0);
			}
			this.openOrderCounts.Add(0);
		}
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x000EF144 File Offset: 0x000ED344
	private int FindRecipeIndex(string id)
	{
		for (int i = 0; i < this.recipe_list.Length; i++)
		{
			if (this.recipe_list[i].id == id)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002A5F RID: 10847 RVA: 0x000EF17C File Offset: 0x000ED37C
	public int GetRecipeQueueCount(ComplexRecipe recipe)
	{
		return this.recipeQueueCounts[recipe.id];
	}

	// Token: 0x06002A60 RID: 10848 RVA: 0x000EF190 File Offset: 0x000ED390
	public bool IsRecipeQueued(ComplexRecipe recipe)
	{
		int num = this.recipeQueueCounts[recipe.id];
		global::Debug.Assert(num >= 0 || num == ComplexFabricator.QUEUE_INFINITE);
		return num != 0;
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x000EF1C8 File Offset: 0x000ED3C8
	public int GetRecipePrefetchCount(ComplexRecipe recipe)
	{
		int remainingQueueCount = this.GetRemainingQueueCount(recipe);
		global::Debug.Assert(remainingQueueCount >= 0);
		return Mathf.Min(2, remainingQueueCount);
	}

	// Token: 0x06002A62 RID: 10850 RVA: 0x000EF1F0 File Offset: 0x000ED3F0
	private int GetRemainingQueueCount(ComplexRecipe recipe)
	{
		int num = this.recipeQueueCounts[recipe.id];
		global::Debug.Assert(num >= 0 || num == ComplexFabricator.QUEUE_INFINITE);
		if (num == ComplexFabricator.QUEUE_INFINITE)
		{
			return ComplexFabricator.MAX_QUEUE_SIZE;
		}
		if (num > 0)
		{
			if (this.IsCurrentRecipe(recipe))
			{
				num--;
			}
			return num;
		}
		return 0;
	}

	// Token: 0x06002A63 RID: 10851 RVA: 0x000EF245 File Offset: 0x000ED445
	private bool IsCurrentRecipe(ComplexRecipe recipe)
	{
		return this.workingOrderIdx >= 0 && this.recipe_list[this.workingOrderIdx].id == recipe.id;
	}

	// Token: 0x06002A64 RID: 10852 RVA: 0x000EF26F File Offset: 0x000ED46F
	public void SetRecipeQueueCount(ComplexRecipe recipe, int count)
	{
		this.SetRecipeQueueCountInternal(recipe, count);
		this.RefreshQueue();
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x000EF27F File Offset: 0x000ED47F
	private void SetRecipeQueueCountInternal(ComplexRecipe recipe, int count)
	{
		this.recipeQueueCounts[recipe.id] = count;
	}

	// Token: 0x06002A66 RID: 10854 RVA: 0x000EF294 File Offset: 0x000ED494
	public void IncrementRecipeQueueCount(ComplexRecipe recipe)
	{
		if (this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
		{
			this.recipeQueueCounts[recipe.id] = 0;
		}
		else if (this.recipeQueueCounts[recipe.id] >= ComplexFabricator.MAX_QUEUE_SIZE)
		{
			this.recipeQueueCounts[recipe.id] = ComplexFabricator.QUEUE_INFINITE;
		}
		else
		{
			Dictionary<string, int> dictionary = this.recipeQueueCounts;
			string id = recipe.id;
			int num = dictionary[id];
			dictionary[id] = num + 1;
		}
		this.RefreshQueue();
	}

	// Token: 0x06002A67 RID: 10855 RVA: 0x000EF321 File Offset: 0x000ED521
	public void DecrementRecipeQueueCount(ComplexRecipe recipe, bool respectInfinite = true)
	{
		this.DecrementRecipeQueueCountInternal(recipe, respectInfinite);
		this.RefreshQueue();
	}

	// Token: 0x06002A68 RID: 10856 RVA: 0x000EF334 File Offset: 0x000ED534
	private void DecrementRecipeQueueCountInternal(ComplexRecipe recipe, bool respectInfinite = true)
	{
		if (!respectInfinite || this.recipeQueueCounts[recipe.id] != ComplexFabricator.QUEUE_INFINITE)
		{
			if (this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
			{
				this.recipeQueueCounts[recipe.id] = ComplexFabricator.MAX_QUEUE_SIZE;
				return;
			}
			if (this.recipeQueueCounts[recipe.id] == 0)
			{
				this.recipeQueueCounts[recipe.id] = ComplexFabricator.QUEUE_INFINITE;
				return;
			}
			Dictionary<string, int> dictionary = this.recipeQueueCounts;
			string id = recipe.id;
			int num = dictionary[id];
			dictionary[id] = num - 1;
		}
	}

	// Token: 0x06002A69 RID: 10857 RVA: 0x000EF3D3 File Offset: 0x000ED5D3
	private void CreateChore()
	{
		global::Debug.Assert(this.chore == null, "chore should be null");
		this.chore = this.workable.CreateWorkChore(this.choreType, this.orderProgress);
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06002A6A RID: 10858 RVA: 0x000EF405 File Offset: 0x000ED605
	public override Chore RemoteDockChore
	{
		get
		{
			if (!this.duplicantOperated)
			{
				return null;
			}
			return this.chore;
		}
	}

	// Token: 0x06002A6B RID: 10859 RVA: 0x000EF417 File Offset: 0x000ED617
	private void CancelChore()
	{
		if (this.cancelling)
		{
			return;
		}
		this.cancelling = true;
		if (this.chore != null)
		{
			this.chore.Cancel("order cancelled");
			this.chore = null;
		}
		this.cancelling = false;
	}

	// Token: 0x06002A6C RID: 10860 RVA: 0x000EF450 File Offset: 0x000ED650
	private void UpdateFetches(DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary missingAmounts)
	{
		ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreTypeIdHash);
		foreach (KeyValuePair<Tag, float> keyValuePair in missingAmounts)
		{
			if (!this.allowManualFluidDelivery)
			{
				Element element = ElementLoader.GetElement(keyValuePair.Key);
				if (element != null && (element.IsLiquid || element.IsGas))
				{
					continue;
				}
			}
			if (keyValuePair.Value >= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.HasPendingFetch(keyValuePair.Key))
			{
				FetchList2 fetchList = new FetchList2(this.inStorage, byHash);
				FetchList2 fetchList2 = fetchList;
				Tag key = keyValuePair.Key;
				float value = keyValuePair.Value;
				fetchList2.Add(key, this.ForbiddenTags, value, Operational.State.None);
				fetchList.ShowStatusItem = false;
				fetchList.Submit(new System.Action(this.OnFetchComplete), false);
				this.fetchListList.Add(fetchList);
			}
		}
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x000EF550 File Offset: 0x000ED750
	private bool HasPendingFetch(Tag tag)
	{
		foreach (FetchList2 fetchList in this.fetchListList)
		{
			float num;
			fetchList.MinimumAmount.TryGetValue(tag, out num);
			if (num > 0f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002A6E RID: 10862 RVA: 0x000EF5B8 File Offset: 0x000ED7B8
	private void CancelFetches()
	{
		foreach (FetchList2 fetchList in this.fetchListList)
		{
			fetchList.Cancel("cancel all orders");
		}
		this.fetchListList.Clear();
	}

	// Token: 0x06002A6F RID: 10863 RVA: 0x000EF618 File Offset: 0x000ED818
	protected virtual void TransferCurrentRecipeIngredientsForBuild()
	{
		ComplexRecipe.RecipeElement[] ingredients = this.recipe_list[this.workingOrderIdx].ingredients;
		int i = 0;
		while (i < ingredients.Length)
		{
			ComplexRecipe.RecipeElement recipeElement = ingredients[i];
			float num;
			for (;;)
			{
				num = recipeElement.amount - this.buildStorage.GetAmountAvailable(recipeElement.material);
				if (num <= 0f)
				{
					break;
				}
				if (this.inStorage.GetAmountAvailable(recipeElement.material) <= 0f)
				{
					goto Block_2;
				}
				this.inStorage.Transfer(this.buildStorage, recipeElement.material, num, false, true);
			}
			IL_9D:
			i++;
			continue;
			Block_2:
			global::Debug.LogWarningFormat("TransferCurrentRecipeIngredientsForBuild ran out of {0} but still needed {1} more.", new object[]
			{
				recipeElement.material,
				num
			});
			goto IL_9D;
		}
	}

	// Token: 0x06002A70 RID: 10864 RVA: 0x000EF6D0 File Offset: 0x000ED8D0
	protected virtual bool HasIngredients(ComplexRecipe recipe, Storage storage)
	{
		ComplexRecipe.RecipeElement[] ingredients = recipe.ingredients;
		if (recipe.consumedHEP > 0)
		{
			HighEnergyParticleStorage component = base.GetComponent<HighEnergyParticleStorage>();
			if (component == null || component.Particles < (float)recipe.consumedHEP)
			{
				return false;
			}
		}
		foreach (ComplexRecipe.RecipeElement recipeElement in ingredients)
		{
			float amountAvailable = storage.GetAmountAvailable(recipeElement.material);
			if (recipeElement.amount - amountAvailable >= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x000EF748 File Offset: 0x000ED948
	private void ToggleMutantSeedFetches()
	{
		if (this.HasAnyOrder)
		{
			ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreTypeIdHash);
			List<FetchList2> list = new List<FetchList2>();
			foreach (FetchList2 fetchList in this.fetchListList)
			{
				foreach (FetchOrder2 fetchOrder in fetchList.FetchOrders)
				{
					foreach (Tag tag in fetchOrder.Tags)
					{
						GameObject prefab = Assets.GetPrefab(tag);
						if (prefab != null && prefab.GetComponent<PlantableSeed>() != null)
						{
							fetchList.Cancel("MutantSeedTagChanged");
							list.Add(fetchList);
						}
					}
				}
			}
			foreach (FetchList2 fetchList2 in list)
			{
				this.fetchListList.Remove(fetchList2);
				foreach (FetchOrder2 fetchOrder2 in fetchList2.FetchOrders)
				{
					foreach (Tag tag2 in fetchOrder2.Tags)
					{
						FetchList2 fetchList3 = new FetchList2(this.inStorage, byHash);
						FetchList2 fetchList4 = fetchList3;
						Tag tag3 = tag2;
						float totalAmount = fetchOrder2.TotalAmount;
						fetchList4.Add(tag3, this.ForbiddenTags, totalAmount, Operational.State.None);
						fetchList3.ShowStatusItem = false;
						fetchList3.Submit(new System.Action(this.OnFetchComplete), false);
						this.fetchListList.Add(fetchList3);
					}
				}
			}
		}
	}

	// Token: 0x06002A72 RID: 10866 RVA: 0x000EF988 File Offset: 0x000EDB88
	protected virtual List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = new List<GameObject>();
		SimUtil.DiseaseInfo diseaseInfo;
		diseaseInfo.count = 0;
		diseaseInfo.idx = 0;
		float num = 0f;
		float num2 = 0f;
		string text = null;
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			num2 += recipeElement.amount;
		}
		ComplexRecipe.RecipeElement recipeElement2 = null;
		foreach (ComplexRecipe.RecipeElement recipeElement3 in recipe.ingredients)
		{
			float num3 = recipeElement3.amount / num2;
			if (recipe.ProductHasFacade && text.IsNullOrWhiteSpace())
			{
				RepairableEquipment component = this.buildStorage.FindFirst(recipeElement3.material).GetComponent<RepairableEquipment>();
				if (component != null)
				{
					text = component.facadeID;
				}
			}
			if (recipeElement3.inheritElement || recipeElement3.Edible)
			{
				recipeElement2 = recipeElement3;
			}
			if (recipeElement3.Edible)
			{
				this.buildStorage.TransferMass(this.outStorage, recipeElement3.material, recipeElement3.amount, true, true, true);
			}
			else
			{
				float num4;
				SimUtil.DiseaseInfo diseaseInfo2;
				float num5;
				this.buildStorage.ConsumeAndGetDisease(recipeElement3.material, recipeElement3.amount, out num4, out diseaseInfo2, out num5);
				if (diseaseInfo2.count > diseaseInfo.count)
				{
					diseaseInfo = diseaseInfo2;
				}
				num += num5 * num3;
			}
		}
		if (recipe.consumedHEP > 0)
		{
			base.GetComponent<HighEnergyParticleStorage>().ConsumeAndGet((float)recipe.consumedHEP);
		}
		foreach (ComplexRecipe.RecipeElement recipeElement4 in recipe.results)
		{
			GameObject gameObject = this.buildStorage.FindFirst(recipeElement4.material);
			if (gameObject != null)
			{
				Edible component2 = gameObject.GetComponent<Edible>();
				if (component2)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component2.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED, "{0}", component2.GetProperName()), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
				}
			}
			switch (recipeElement4.temperatureOperation)
			{
			case ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature:
			case ComplexRecipe.RecipeElement.TemperatureOperation.Heated:
			{
				GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab(recipeElement4.material), Grid.SceneLayer.Ore, null, 0);
				int cell = Grid.PosToCell(this);
				gameObject2.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore) + this.outputOffset);
				PrimaryElement component3 = gameObject2.GetComponent<PrimaryElement>();
				component3.Units = recipeElement4.amount;
				component3.Temperature = ((recipeElement4.temperatureOperation == ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature) ? num : this.heatedTemperature);
				if (recipeElement2 != null)
				{
					Element element = ElementLoader.GetElement(recipeElement2.material);
					if (element != null)
					{
						component3.SetElement(element.id, false);
					}
				}
				if (recipe.ProductHasFacade && !text.IsNullOrWhiteSpace())
				{
					Equippable component4 = gameObject2.GetComponent<Equippable>();
					if (component4 != null)
					{
						EquippableFacade.AddFacadeToEquippable(component4, text);
					}
				}
				gameObject2.SetActive(true);
				float num6 = recipeElement4.amount / recipe.TotalResultUnits();
				component3.AddDisease(diseaseInfo.idx, Mathf.RoundToInt((float)diseaseInfo.count * num6), "ComplexFabricator.CompleteOrder");
				if (!recipeElement4.facadeID.IsNullOrWhiteSpace())
				{
					Equippable component5 = gameObject2.GetComponent<Equippable>();
					if (component5 != null)
					{
						EquippableFacade.AddFacadeToEquippable(component5, recipeElement4.facadeID);
					}
				}
				gameObject2.GetComponent<KMonoBehaviour>().Trigger(748399584, null);
				list.Add(gameObject2);
				if (this.storeProduced || recipeElement4.storeElement)
				{
					this.outStorage.Store(gameObject2, false, false, true, false);
				}
				break;
			}
			case ComplexRecipe.RecipeElement.TemperatureOperation.Melted:
				if (this.storeProduced || recipeElement4.storeElement)
				{
					float temperature = ElementLoader.GetElement(recipeElement4.material).defaultValues.temperature;
					this.outStorage.AddLiquid(ElementLoader.GetElementID(recipeElement4.material), recipeElement4.amount, temperature, 0, 0, false, true);
				}
				break;
			case ComplexRecipe.RecipeElement.TemperatureOperation.Dehydrated:
				for (int j = 0; j < (int)recipeElement4.amount; j++)
				{
					GameObject gameObject3 = GameUtil.KInstantiate(Assets.GetPrefab(recipeElement4.material), Grid.SceneLayer.Ore, null, 0);
					int cell2 = Grid.PosToCell(this);
					gameObject3.transform.SetPosition(Grid.CellToPosCCC(cell2, Grid.SceneLayer.Ore) + this.outputOffset);
					float amount = recipeElement2.amount / recipeElement4.amount;
					gameObject3.GetComponent<PrimaryElement>().Temperature = ((recipeElement4.temperatureOperation == ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature) ? num : this.heatedTemperature);
					DehydratedFoodPackage component6 = gameObject3.GetComponent<DehydratedFoodPackage>();
					if (component6 != null)
					{
						Storage component7 = component6.GetComponent<Storage>();
						this.outStorage.TransferMass(component7, recipeElement2.material, amount, true, false, false);
					}
					gameObject3.SetActive(true);
					gameObject3.GetComponent<KMonoBehaviour>().Trigger(748399584, null);
					list.Add(gameObject3);
					if (this.storeProduced || recipeElement4.storeElement)
					{
						this.outStorage.Store(gameObject3, false, false, true, false);
					}
				}
				break;
			}
			if (list.Count > 0)
			{
				SymbolOverrideController component8 = base.GetComponent<SymbolOverrideController>();
				if (component8 != null)
				{
					KAnim.Build build = list[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
					KAnim.Build.Symbol symbol = build.GetSymbol(build.name);
					if (symbol != null)
					{
						component8.TryRemoveSymbolOverride("output_tracker", 0);
						component8.AddSymbolOverride("output_tracker", symbol, 0);
					}
					else
					{
						global::Debug.LogWarning(component8.name + " is missing symbol " + build.name);
					}
				}
			}
		}
		if (recipe.producedHEP > 0)
		{
			base.GetComponent<HighEnergyParticleStorage>().Store((float)recipe.producedHEP);
		}
		return list;
	}

	// Token: 0x06002A73 RID: 10867 RVA: 0x000EFF28 File Offset: 0x000EE128
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		ComplexRecipe[] recipes = this.GetRecipes();
		if (recipes.Length != 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.PROCESSES, UI.BUILDINGEFFECTS.TOOLTIPS.PROCESSES, Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		foreach (ComplexRecipe complexRecipe in recipes)
		{
			string text = "";
			string uiname = complexRecipe.GetUIName(false);
			foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
			{
				text = text + "• " + string.Format(UI.BUILDINGEFFECTS.PROCESSEDITEM, recipeElement.material.ProperName(), recipeElement.amount) + "\n";
			}
			Descriptor item2 = new Descriptor(uiname, string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.FABRICATOR_INGREDIENTS, text), Descriptor.DescriptorType.Effect, false);
			item2.IncreaseIndent();
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x000F0020 File Offset: 0x000EE220
	public virtual List<Descriptor> AdditionalEffectsForRecipe(ComplexRecipe recipe)
	{
		return new List<Descriptor>();
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x000F0028 File Offset: 0x000EE228
	public string GetConversationTopic()
	{
		if (this.HasWorkingOrder)
		{
			ComplexRecipe complexRecipe = this.recipe_list[this.workingOrderIdx];
			if (complexRecipe != null)
			{
				return complexRecipe.results[0].material.Name;
			}
		}
		return null;
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x000F0064 File Offset: 0x000EE264
	public bool NeedsMoreHEPForQueuedRecipe()
	{
		if (this.hasOpenOrders)
		{
			HighEnergyParticleStorage component = base.GetComponent<HighEnergyParticleStorage>();
			foreach (KeyValuePair<string, int> keyValuePair in this.recipeQueueCounts)
			{
				if (keyValuePair.Value > 0)
				{
					foreach (ComplexRecipe complexRecipe in this.GetRecipes())
					{
						if (complexRecipe.id == keyValuePair.Key && (float)complexRecipe.consumedHEP > component.Particles)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x04001847 RID: 6215
	private const int MaxPrefetchCount = 2;

	// Token: 0x04001848 RID: 6216
	public bool duplicantOperated = true;

	// Token: 0x04001849 RID: 6217
	protected ComplexFabricatorWorkable workable;

	// Token: 0x0400184A RID: 6218
	public string SideScreenSubtitleLabel = UI.UISIDESCREENS.FABRICATORSIDESCREEN.SUBTITLE;

	// Token: 0x0400184B RID: 6219
	public string SideScreenRecipeScreenTitle = UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_DETAILS;

	// Token: 0x0400184C RID: 6220
	[SerializeField]
	public HashedString fetchChoreTypeIdHash = Db.Get().ChoreTypes.FabricateFetch.IdHash;

	// Token: 0x0400184D RID: 6221
	[SerializeField]
	public float heatedTemperature;

	// Token: 0x0400184E RID: 6222
	[SerializeField]
	public bool storeProduced;

	// Token: 0x0400184F RID: 6223
	[SerializeField]
	public bool allowManualFluidDelivery = true;

	// Token: 0x04001850 RID: 6224
	public ComplexFabricatorSideScreen.StyleSetting sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;

	// Token: 0x04001851 RID: 6225
	public bool labelByResult = true;

	// Token: 0x04001852 RID: 6226
	public Vector3 outputOffset = Vector3.zero;

	// Token: 0x04001853 RID: 6227
	public ChoreType choreType;

	// Token: 0x04001854 RID: 6228
	public bool keepExcessLiquids;

	// Token: 0x04001855 RID: 6229
	public Tag keepAdditionalTag = Tag.Invalid;

	// Token: 0x04001856 RID: 6230
	public StatusItem workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorProducing;

	// Token: 0x04001857 RID: 6231
	public static int MAX_QUEUE_SIZE = 99;

	// Token: 0x04001858 RID: 6232
	public static int QUEUE_INFINITE = -1;

	// Token: 0x04001859 RID: 6233
	[Serialize]
	private Dictionary<string, int> recipeQueueCounts = new Dictionary<string, int>();

	// Token: 0x0400185A RID: 6234
	private int nextOrderIdx;

	// Token: 0x0400185B RID: 6235
	private bool nextOrderIsWorkable;

	// Token: 0x0400185C RID: 6236
	private int workingOrderIdx = -1;

	// Token: 0x0400185D RID: 6237
	[Serialize]
	private string lastWorkingRecipe;

	// Token: 0x0400185E RID: 6238
	[Serialize]
	private float orderProgress;

	// Token: 0x0400185F RID: 6239
	private List<int> openOrderCounts = new List<int>();

	// Token: 0x04001860 RID: 6240
	[Serialize]
	private bool forbidMutantSeeds;

	// Token: 0x04001861 RID: 6241
	private Tag[] forbiddenMutantTags = new Tag[]
	{
		GameTags.MutatedSeed
	};

	// Token: 0x04001862 RID: 6242
	private bool queueDirty = true;

	// Token: 0x04001863 RID: 6243
	private bool hasOpenOrders;

	// Token: 0x04001864 RID: 6244
	private List<FetchList2> fetchListList = new List<FetchList2>();

	// Token: 0x04001865 RID: 6245
	private Chore chore;

	// Token: 0x04001866 RID: 6246
	private bool cancelling;

	// Token: 0x04001867 RID: 6247
	private ComplexRecipe[] recipe_list;

	// Token: 0x04001868 RID: 6248
	private Dictionary<Tag, float> materialNeedCache = new Dictionary<Tag, float>();

	// Token: 0x04001869 RID: 6249
	[SerializeField]
	public Storage inStorage;

	// Token: 0x0400186A RID: 6250
	[SerializeField]
	public Storage buildStorage;

	// Token: 0x0400186B RID: 6251
	[SerializeField]
	public Storage outStorage;

	// Token: 0x0400186C RID: 6252
	[MyCmpAdd]
	private LoopingSounds loopingSounds;

	// Token: 0x0400186D RID: 6253
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x0400186E RID: 6254
	[MyCmpAdd]
	protected ComplexFabricatorSM fabricatorSM;

	// Token: 0x0400186F RID: 6255
	private ProgressBar progressBar;

	// Token: 0x04001870 RID: 6256
	public bool showProgressBar;

	// Token: 0x04001871 RID: 6257
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04001872 RID: 6258
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnParticleStorageChangedDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04001873 RID: 6259
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnDroppedAllDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnDroppedAll(data);
	});

	// Token: 0x04001874 RID: 6260
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001875 RID: 6261
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001876 RID: 6262
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
