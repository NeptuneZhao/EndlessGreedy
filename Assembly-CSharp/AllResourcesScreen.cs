﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BC8 RID: 3016
public class AllResourcesScreen : ShowOptimizedKScreen, ISim4000ms, ISim1000ms
{
	// Token: 0x06005BFA RID: 23546 RVA: 0x0021A45F File Offset: 0x0021865F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		AllResourcesScreen.Instance = this;
	}

	// Token: 0x06005BFB RID: 23547 RVA: 0x0021A46D File Offset: 0x0021866D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		this.Init();
	}

	// Token: 0x06005BFC RID: 23548 RVA: 0x0021A482 File Offset: 0x00218682
	protected override void OnForcedCleanUp()
	{
		AllResourcesScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06005BFD RID: 23549 RVA: 0x0021A490 File Offset: 0x00218690
	public void SetFilter(string filter)
	{
		if (string.IsNullOrEmpty(filter))
		{
			filter = "";
		}
		this.searchInputField.text = filter;
	}

	// Token: 0x06005BFE RID: 23550 RVA: 0x0021A4B0 File Offset: 0x002186B0
	public void Init()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		this.Populate(null);
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Populate));
		DiscoveredResources.Instance.OnDiscover += delegate(Tag a, Tag b)
		{
			this.Populate(null);
		};
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.clearSearchButton.onClick += delegate()
		{
			this.searchInputField.text = "";
		};
		this.searchInputField.OnValueChangesPaused = delegate()
		{
			this.SearchFilter(this.searchInputField.text);
		};
		KInputTextField kinputTextField = this.searchInputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
		}));
		this.searchInputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.Show(false);
	}

	// Token: 0x06005BFF RID: 23551 RVA: 0x0021A595 File Offset: 0x00218795
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			ManagementMenu.Instance.CloseAll();
			AllDiagnosticsScreen.Instance.Show(false);
			this.RefreshRows();
			return;
		}
		this.SetFilter(null);
	}

	// Token: 0x06005C00 RID: 23552 RVA: 0x0021A5C4 File Offset: 0x002187C4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			this.Show(false);
			e.Consumed = true;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005C01 RID: 23553 RVA: 0x0021A618 File Offset: 0x00218818
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		if (PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			this.Show(false);
			e.Consumed = true;
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06005C02 RID: 23554 RVA: 0x0021A669 File Offset: 0x00218869
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x06005C03 RID: 23555 RVA: 0x0021A670 File Offset: 0x00218870
	public void Populate(object data = null)
	{
		this.SpawnRows();
	}

	// Token: 0x06005C04 RID: 23556 RVA: 0x0021A678 File Offset: 0x00218878
	private void SpawnRows()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		this.allowDisplayCategories.Add(GameTags.MaterialCategories);
		this.allowDisplayCategories.Add(GameTags.CalorieCategories);
		this.allowDisplayCategories.Add(GameTags.UnitCategories);
		foreach (Tag categoryTag in GameTags.MaterialCategories)
		{
			this.SpawnCategoryRow(categoryTag, GameUtil.MeasureUnit.mass);
		}
		foreach (Tag categoryTag2 in GameTags.CalorieCategories)
		{
			this.SpawnCategoryRow(categoryTag2, GameUtil.MeasureUnit.kcal);
		}
		foreach (Tag categoryTag3 in GameTags.UnitCategories)
		{
			this.SpawnCategoryRow(categoryTag3, GameUtil.MeasureUnit.quantity);
		}
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
		{
			list.Add(keyValuePair.Key);
		}
		list.Sort((Tag a, Tag b) => a.ProperNameStripLink().CompareTo(b.ProperNameStripLink()));
		foreach (Tag key in list)
		{
			this.categoryRows[key].GameObject.transform.SetAsLastSibling();
		}
	}

	// Token: 0x06005C05 RID: 23557 RVA: 0x0021A854 File Offset: 0x00218A54
	private void SpawnCategoryRow(Tag categoryTag, GameUtil.MeasureUnit unit)
	{
		if (!this.categoryRows.ContainsKey(categoryTag))
		{
			GameObject gameObject = Util.KInstantiateUI(this.categoryLinePrefab, this.rootListContainer, true);
			AllResourcesScreen.CategoryRow categoryRow = new AllResourcesScreen.CategoryRow(categoryTag, gameObject);
			gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel").SetText(categoryTag.ProperNameStripLink());
			this.categoryRows.Add(categoryTag, categoryRow);
			this.currentlyDisplayedRows.Add(categoryTag, true);
			this.units.Add(categoryTag, unit);
			GraphBase component = categoryRow.sparkLayer.GetComponent<GraphBase>();
			component.axis_x.min_value = 0f;
			component.axis_x.max_value = 600f;
			component.axis_x.guide_frequency = 120f;
			component.RefreshGuides();
		}
		GameObject container = this.categoryRows[categoryTag].FoldOutPanel.container;
		foreach (Tag tag in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(categoryTag))
		{
			if (!this.resourceRows.ContainsKey(tag))
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.resourceLinePrefab, container, true);
				HierarchyReferences component2 = gameObject2.GetComponent<HierarchyReferences>();
				global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
				component2.GetReference<Image>("Icon").sprite = uisprite.first;
				component2.GetReference<Image>("Icon").color = uisprite.second;
				component2.GetReference<LocText>("NameLabel").SetText(tag.ProperNameStripLink());
				Tag targetTag = tag;
				MultiToggle pinToggle = component2.GetReference<MultiToggle>("PinToggle");
				MultiToggle pinToggle2 = pinToggle;
				pinToggle2.onClick = (System.Action)Delegate.Combine(pinToggle2.onClick, new System.Action(delegate()
				{
					if (ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(targetTag))
					{
						ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Remove(targetTag);
					}
					else
					{
						ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Add(targetTag);
						if (DiscoveredResources.Instance.newDiscoveries.ContainsKey(targetTag))
						{
							DiscoveredResources.Instance.newDiscoveries.Remove(targetTag);
						}
					}
					this.RefreshPinnedState(targetTag);
					pinToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(targetTag) ? 1 : 0);
				}));
				gameObject2.GetComponent<MultiToggle>().onClick = pinToggle.onClick;
				MultiToggle notifyToggle = component2.GetReference<MultiToggle>("NotificationToggle");
				MultiToggle notifyToggle2 = notifyToggle;
				notifyToggle2.onClick = (System.Action)Delegate.Combine(notifyToggle2.onClick, new System.Action(delegate()
				{
					if (ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Contains(targetTag))
					{
						ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Remove(targetTag);
					}
					else
					{
						ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Add(targetTag);
					}
					this.RefreshPinnedState(targetTag);
					notifyToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Contains(targetTag) ? 1 : 0);
				}));
				component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.min_value = 0f;
				component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.max_value = 600f;
				component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.guide_frequency = 120f;
				component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().RefreshGuides();
				AllResourcesScreen.ResourceRow value = new AllResourcesScreen.ResourceRow(tag, gameObject2);
				this.resourceRows.Add(tag, value);
				this.currentlyDisplayedRows.Add(tag, true);
				if (this.units.ContainsKey(tag))
				{
					global::Debug.LogError(string.Concat(new string[]
					{
						"Trying to add ",
						tag.ToString(),
						":UnitType ",
						this.units[tag].ToString(),
						" but units dictionary already has key ",
						tag.ToString(),
						" with unit type:",
						unit.ToString()
					}));
				}
				else
				{
					this.units.Add(tag, unit);
				}
			}
		}
	}

	// Token: 0x06005C06 RID: 23558 RVA: 0x0021ABEC File Offset: 0x00218DEC
	private void FilterRowBySearch(Tag tag, string filter)
	{
		this.currentlyDisplayedRows[tag] = this.PassesSearchFilter(tag, filter);
	}

	// Token: 0x06005C07 RID: 23559 RVA: 0x0021AC04 File Offset: 0x00218E04
	private void SearchFilter(string search)
	{
		foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair in this.resourceRows)
		{
			this.FilterRowBySearch(keyValuePair.Key, search);
		}
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair2 in this.categoryRows)
		{
			if (this.PassesSearchFilter(keyValuePair2.Key, search))
			{
				this.currentlyDisplayedRows[keyValuePair2.Key] = true;
				using (HashSet<Tag>.Enumerator enumerator3 = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(keyValuePair2.Key).GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Tag key = enumerator3.Current;
						if (this.currentlyDisplayedRows.ContainsKey(key))
						{
							this.currentlyDisplayedRows[key] = true;
						}
					}
					continue;
				}
			}
			this.currentlyDisplayedRows[keyValuePair2.Key] = false;
		}
		this.EnableCategoriesByActiveChildren();
		this.SetRowsActive();
	}

	// Token: 0x06005C08 RID: 23560 RVA: 0x0021AD48 File Offset: 0x00218F48
	private bool PassesSearchFilter(Tag tag, string filter)
	{
		filter = filter.ToUpper();
		string text = tag.ProperNameStripLink().ToUpper();
		return !(filter != "") || text.Contains(filter);
	}

	// Token: 0x06005C09 RID: 23561 RVA: 0x0021AD84 File Offset: 0x00218F84
	private void EnableCategoriesByActiveChildren()
	{
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(keyValuePair.Key).Count == 0)
			{
				this.currentlyDisplayedRows[keyValuePair.Key] = false;
			}
			else
			{
				GameObject container = keyValuePair.Value.GameObject.GetComponent<FoldOutPanel>().container;
				foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair2 in this.resourceRows)
				{
					if (!(keyValuePair2.Value.GameObject.transform.parent.gameObject != container))
					{
						this.currentlyDisplayedRows[keyValuePair.Key] = (this.currentlyDisplayedRows[keyValuePair.Key] || this.currentlyDisplayedRows[keyValuePair2.Key]);
					}
				}
			}
		}
	}

	// Token: 0x06005C0A RID: 23562 RVA: 0x0021AEB8 File Offset: 0x002190B8
	private void RefreshPinnedState(Tag tag)
	{
		this.resourceRows[tag].notificiationToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Contains(tag) ? 1 : 0);
		this.resourceRows[tag].pinToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(tag) ? 1 : 0);
	}

	// Token: 0x06005C0B RID: 23563 RVA: 0x0021AF34 File Offset: 0x00219134
	public void RefreshRows()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		this.EnableCategoriesByActiveChildren();
		this.SetRowsActive();
		if (this.allowRefresh)
		{
			foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
			{
				if (keyValuePair.Value.GameObject.activeInHierarchy)
				{
					float amount = worldInventory.GetAmount(keyValuePair.Key, false);
					float totalAmount = worldInventory.GetTotalAmount(keyValuePair.Key, false);
					if (!worldInventory.HasValidCount)
					{
						keyValuePair.Value.availableLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
						keyValuePair.Value.totalLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
						keyValuePair.Value.reservedLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
					}
					else
					{
						switch (this.units[keyValuePair.Key])
						{
						case GameUtil.MeasureUnit.mass:
							if (keyValuePair.Value.CheckAvailableAmountChanged(amount, true))
							{
								keyValuePair.Value.availableLabel.SetText(GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							if (keyValuePair.Value.CheckTotalResourceAmountChanged(totalAmount, true))
							{
								keyValuePair.Value.totalLabel.SetText(GameUtil.GetFormattedMass(totalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							if (keyValuePair.Value.CheckReservedResourceAmountChanged(totalAmount - amount, true))
							{
								keyValuePair.Value.reservedLabel.SetText(GameUtil.GetFormattedMass(totalAmount - amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							break;
						case GameUtil.MeasureUnit.kcal:
						{
							float calories = WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, ClusterManager.Instance.activeWorld.worldInventory, true);
							if (keyValuePair.Value.CheckAvailableAmountChanged(amount, true))
							{
								keyValuePair.Value.availableLabel.SetText(GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true));
							}
							if (keyValuePair.Value.CheckTotalResourceAmountChanged(totalAmount, true))
							{
								keyValuePair.Value.totalLabel.SetText(GameUtil.GetFormattedCalories(totalAmount, GameUtil.TimeSlice.None, true));
							}
							if (keyValuePair.Value.CheckReservedResourceAmountChanged(totalAmount - amount, true))
							{
								keyValuePair.Value.reservedLabel.SetText(GameUtil.GetFormattedCalories(totalAmount - amount, GameUtil.TimeSlice.None, true));
							}
							break;
						}
						case GameUtil.MeasureUnit.quantity:
							if (keyValuePair.Value.CheckAvailableAmountChanged(amount, true))
							{
								keyValuePair.Value.availableLabel.SetText(GameUtil.GetFormattedUnits(amount, GameUtil.TimeSlice.None, true, ""));
							}
							if (keyValuePair.Value.CheckTotalResourceAmountChanged(totalAmount, true))
							{
								keyValuePair.Value.totalLabel.SetText(GameUtil.GetFormattedUnits(totalAmount, GameUtil.TimeSlice.None, true, ""));
							}
							if (keyValuePair.Value.CheckReservedResourceAmountChanged(totalAmount - amount, true))
							{
								keyValuePair.Value.reservedLabel.SetText(GameUtil.GetFormattedUnits(totalAmount - amount, GameUtil.TimeSlice.None, true, ""));
							}
							break;
						}
					}
				}
			}
			foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair2 in this.resourceRows)
			{
				if (keyValuePair2.Value.GameObject.activeInHierarchy)
				{
					float amount2 = worldInventory.GetAmount(keyValuePair2.Key, false);
					float totalAmount2 = worldInventory.GetTotalAmount(keyValuePair2.Key, false);
					if (!worldInventory.HasValidCount)
					{
						keyValuePair2.Value.availableLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
						keyValuePair2.Value.totalLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
						keyValuePair2.Value.reservedLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
					}
					else
					{
						switch (this.units[keyValuePair2.Key])
						{
						case GameUtil.MeasureUnit.mass:
							if (keyValuePair2.Value.CheckAvailableAmountChanged(amount2, true))
							{
								keyValuePair2.Value.availableLabel.SetText(GameUtil.GetFormattedMass(amount2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							if (keyValuePair2.Value.CheckTotalResourceAmountChanged(totalAmount2, true))
							{
								keyValuePair2.Value.totalLabel.SetText(GameUtil.GetFormattedMass(totalAmount2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							if (keyValuePair2.Value.CheckReservedResourceAmountChanged(totalAmount2 - amount2, true))
							{
								keyValuePair2.Value.reservedLabel.SetText(GameUtil.GetFormattedMass(totalAmount2 - amount2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							break;
						case GameUtil.MeasureUnit.kcal:
						{
							float num = WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(keyValuePair2.Key.Name, ClusterManager.Instance.activeWorld.worldInventory, true);
							if (keyValuePair2.Value.CheckAvailableAmountChanged(num, true))
							{
								keyValuePair2.Value.availableLabel.SetText(GameUtil.GetFormattedCalories(num, GameUtil.TimeSlice.None, true));
							}
							if (keyValuePair2.Value.CheckTotalResourceAmountChanged(totalAmount2, true))
							{
								keyValuePair2.Value.totalLabel.SetText(GameUtil.GetFormattedCalories(totalAmount2, GameUtil.TimeSlice.None, true));
							}
							if (keyValuePair2.Value.CheckReservedResourceAmountChanged(totalAmount2 - amount2, true))
							{
								keyValuePair2.Value.reservedLabel.SetText(GameUtil.GetFormattedCalories(totalAmount2 - amount2, GameUtil.TimeSlice.None, true));
							}
							break;
						}
						case GameUtil.MeasureUnit.quantity:
							if (keyValuePair2.Value.CheckAvailableAmountChanged(amount2, true))
							{
								keyValuePair2.Value.availableLabel.SetText(GameUtil.GetFormattedUnits(amount2, GameUtil.TimeSlice.None, true, ""));
							}
							if (keyValuePair2.Value.CheckTotalResourceAmountChanged(totalAmount2, true))
							{
								keyValuePair2.Value.totalLabel.SetText(GameUtil.GetFormattedUnits(totalAmount2, GameUtil.TimeSlice.None, true, ""));
							}
							if (keyValuePair2.Value.CheckReservedResourceAmountChanged(totalAmount2 - amount2, true))
							{
								keyValuePair2.Value.reservedLabel.SetText(GameUtil.GetFormattedUnits(totalAmount2 - amount2, GameUtil.TimeSlice.None, true, ""));
							}
							break;
						}
					}
					this.RefreshPinnedState(keyValuePair2.Key);
				}
			}
		}
	}

	// Token: 0x06005C0C RID: 23564 RVA: 0x0021B568 File Offset: 0x00219768
	public int UniqueResourceRowCount()
	{
		return this.resourceRows.Count;
	}

	// Token: 0x06005C0D RID: 23565 RVA: 0x0021B578 File Offset: 0x00219778
	private void RefreshCharts()
	{
		float time = GameClock.Instance.GetTime();
		float num = 3000f;
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
		{
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, keyValuePair.Key);
			if (resourceStatistic != null)
			{
				SparkLayer sparkLayer = keyValuePair.Value.sparkLayer;
				global::Tuple<float, float>[] array = resourceStatistic.ChartableData(num);
				if (array.Length != 0)
				{
					sparkLayer.graph.axis_x.max_value = array[array.Length - 1].first;
				}
				else
				{
					sparkLayer.graph.axis_x.max_value = 0f;
				}
				sparkLayer.graph.axis_x.min_value = time - num;
				sparkLayer.RefreshLine(array, "resourceAmount");
			}
			else
			{
				DebugUtil.DevLogError("DevError: No tracker found for resource category " + keyValuePair.Key.ToString());
			}
		}
		foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair2 in this.resourceRows)
		{
			if (keyValuePair2.Value.GameObject.activeInHierarchy)
			{
				ResourceTracker resourceStatistic2 = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, keyValuePair2.Key);
				if (resourceStatistic2 != null)
				{
					SparkLayer sparkLayer2 = keyValuePair2.Value.sparkLayer;
					global::Tuple<float, float>[] array2 = resourceStatistic2.ChartableData(num);
					if (array2.Length != 0)
					{
						sparkLayer2.graph.axis_x.max_value = array2[array2.Length - 1].first;
					}
					else
					{
						sparkLayer2.graph.axis_x.max_value = 0f;
					}
					sparkLayer2.graph.axis_x.min_value = time - num;
					sparkLayer2.RefreshLine(array2, "resourceAmount");
				}
				else
				{
					DebugUtil.DevLogError("DevError: No tracker found for resource " + keyValuePair2.Key.ToString());
				}
			}
		}
	}

	// Token: 0x06005C0E RID: 23566 RVA: 0x0021B7B0 File Offset: 0x002199B0
	private void SetRowsActive()
	{
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
		{
			if (keyValuePair.Value.GameObject.activeSelf != this.currentlyDisplayedRows[keyValuePair.Key])
			{
				keyValuePair.Value.GameObject.SetActive(this.currentlyDisplayedRows[keyValuePair.Key]);
			}
		}
		foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair2 in this.resourceRows)
		{
			if (keyValuePair2.Value.GameObject.activeSelf != this.currentlyDisplayedRows[keyValuePair2.Key])
			{
				keyValuePair2.Value.GameObject.SetActive(this.currentlyDisplayedRows[keyValuePair2.Key]);
				if (!this.currentlyDisplayedRows[keyValuePair2.Key] && keyValuePair2.Value.horizontalLayoutGroup.enabled)
				{
					keyValuePair2.Value.horizontalLayoutGroup.enabled = false;
				}
			}
		}
	}

	// Token: 0x06005C0F RID: 23567 RVA: 0x0021B908 File Offset: 0x00219B08
	public void Sim4000ms(float dt)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		this.RefreshCharts();
	}

	// Token: 0x06005C10 RID: 23568 RVA: 0x0021B919 File Offset: 0x00219B19
	public void Sim1000ms(float dt)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		this.RefreshRows();
	}

	// Token: 0x04003D8C RID: 15756
	private Dictionary<Tag, AllResourcesScreen.ResourceRow> resourceRows = new Dictionary<Tag, AllResourcesScreen.ResourceRow>();

	// Token: 0x04003D8D RID: 15757
	private Dictionary<Tag, AllResourcesScreen.CategoryRow> categoryRows = new Dictionary<Tag, AllResourcesScreen.CategoryRow>();

	// Token: 0x04003D8E RID: 15758
	public Dictionary<Tag, GameUtil.MeasureUnit> units = new Dictionary<Tag, GameUtil.MeasureUnit>();

	// Token: 0x04003D8F RID: 15759
	public GameObject rootListContainer;

	// Token: 0x04003D90 RID: 15760
	public GameObject resourceLinePrefab;

	// Token: 0x04003D91 RID: 15761
	public GameObject categoryLinePrefab;

	// Token: 0x04003D92 RID: 15762
	public KButton closeButton;

	// Token: 0x04003D93 RID: 15763
	public bool allowRefresh = true;

	// Token: 0x04003D94 RID: 15764
	[SerializeField]
	private KInputTextField searchInputField;

	// Token: 0x04003D95 RID: 15765
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x04003D96 RID: 15766
	public static AllResourcesScreen Instance;

	// Token: 0x04003D97 RID: 15767
	public Dictionary<Tag, bool> currentlyDisplayedRows = new Dictionary<Tag, bool>();

	// Token: 0x04003D98 RID: 15768
	public List<TagSet> allowDisplayCategories = new List<TagSet>();

	// Token: 0x04003D99 RID: 15769
	private bool initialized;

	// Token: 0x02001CB8 RID: 7352
	private class ScreenRowBase
	{
		// Token: 0x0600A6C5 RID: 42693 RVA: 0x00398B0C File Offset: 0x00396D0C
		public ScreenRowBase(Tag tag, GameObject gameObject)
		{
			this.Tag = tag;
			this.GameObject = gameObject;
			HierarchyReferences component = this.GameObject.GetComponent<HierarchyReferences>();
			this.availableLabel = component.GetReference<LocText>("AvailableLabel");
			this.totalLabel = component.GetReference<LocText>("TotalLabel");
			this.reservedLabel = component.GetReference<LocText>("ReservedLabel");
			this.sparkLayer = component.GetReference<SparkLayer>("Chart");
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x0600A6C6 RID: 42694 RVA: 0x00398B9E File Offset: 0x00396D9E
		// (set) Token: 0x0600A6C7 RID: 42695 RVA: 0x00398BA6 File Offset: 0x00396DA6
		public Tag Tag { get; private set; }

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x0600A6C8 RID: 42696 RVA: 0x00398BAF File Offset: 0x00396DAF
		// (set) Token: 0x0600A6C9 RID: 42697 RVA: 0x00398BB7 File Offset: 0x00396DB7
		public GameObject GameObject { get; private set; }

		// Token: 0x0600A6CA RID: 42698 RVA: 0x00398BC0 File Offset: 0x00396DC0
		public bool CheckAvailableAmountChanged(float newAvailableResourceAmount, bool updateIfTrue)
		{
			bool flag = newAvailableResourceAmount != this.oldAvailableResourceAmount;
			if (flag && updateIfTrue)
			{
				this.oldAvailableResourceAmount = newAvailableResourceAmount;
			}
			return flag;
		}

		// Token: 0x0600A6CB RID: 42699 RVA: 0x00398BDA File Offset: 0x00396DDA
		public bool CheckTotalResourceAmountChanged(float newTotalResourceAmount, bool updateIfTrue)
		{
			bool flag = newTotalResourceAmount != this.oldTotalResourceAmount;
			if (flag && updateIfTrue)
			{
				this.oldTotalResourceAmount = newTotalResourceAmount;
			}
			return flag;
		}

		// Token: 0x0600A6CC RID: 42700 RVA: 0x00398BF4 File Offset: 0x00396DF4
		public bool CheckReservedResourceAmountChanged(float newReservedResourceAmount, bool updateIfTrue)
		{
			bool flag = newReservedResourceAmount != this.oldReserverResourceAmount;
			if (flag && updateIfTrue)
			{
				this.oldReserverResourceAmount = newReservedResourceAmount;
			}
			return flag;
		}

		// Token: 0x040084E4 RID: 34020
		public LocText availableLabel;

		// Token: 0x040084E5 RID: 34021
		public LocText totalLabel;

		// Token: 0x040084E6 RID: 34022
		public LocText reservedLabel;

		// Token: 0x040084E7 RID: 34023
		public SparkLayer sparkLayer;

		// Token: 0x040084E8 RID: 34024
		private float oldAvailableResourceAmount = -1f;

		// Token: 0x040084E9 RID: 34025
		private float oldTotalResourceAmount = -1f;

		// Token: 0x040084EA RID: 34026
		private float oldReserverResourceAmount = -1f;
	}

	// Token: 0x02001CB9 RID: 7353
	private class CategoryRow : AllResourcesScreen.ScreenRowBase
	{
		// Token: 0x0600A6CD RID: 42701 RVA: 0x00398C0E File Offset: 0x00396E0E
		public CategoryRow(Tag tag, GameObject gameObject) : base(tag, gameObject)
		{
			this.FoldOutPanel = base.GameObject.GetComponent<FoldOutPanel>();
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x0600A6CE RID: 42702 RVA: 0x00398C29 File Offset: 0x00396E29
		// (set) Token: 0x0600A6CF RID: 42703 RVA: 0x00398C31 File Offset: 0x00396E31
		public FoldOutPanel FoldOutPanel { get; private set; }
	}

	// Token: 0x02001CBA RID: 7354
	private class ResourceRow : AllResourcesScreen.ScreenRowBase
	{
		// Token: 0x0600A6D0 RID: 42704 RVA: 0x00398C3C File Offset: 0x00396E3C
		public ResourceRow(Tag tag, GameObject gameObject) : base(tag, gameObject)
		{
			HierarchyReferences component = base.GameObject.GetComponent<HierarchyReferences>();
			this.notificiationToggle = component.GetReference<MultiToggle>("NotificationToggle");
			this.pinToggle = component.GetReference<MultiToggle>("PinToggle");
			this.horizontalLayoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();
		}

		// Token: 0x040084EC RID: 34028
		public MultiToggle notificiationToggle;

		// Token: 0x040084ED RID: 34029
		public MultiToggle pinToggle;

		// Token: 0x040084EE RID: 34030
		public HorizontalLayoutGroup horizontalLayoutGroup;
	}
}
