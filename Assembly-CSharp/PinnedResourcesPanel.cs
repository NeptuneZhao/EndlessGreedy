using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D0B RID: 3339
public class PinnedResourcesPanel : KScreen, IRender1000ms
{
	// Token: 0x060067F0 RID: 26608 RVA: 0x0026D258 File Offset: 0x0026B458
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rowContainerLayout = this.rowContainer.GetComponent<QuickLayout>();
	}

	// Token: 0x060067F1 RID: 26609 RVA: 0x0026D274 File Offset: 0x0026B474
	protected override void OnSpawn()
	{
		base.OnSpawn();
		PinnedResourcesPanel.Instance = this;
		this.Populate(null);
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Populate));
		MultiToggle component = this.headerButton.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.Refresh();
		}));
		MultiToggle component2 = this.seeAllButton.GetComponent<MultiToggle>();
		component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
		{
			bool flag = !AllResourcesScreen.Instance.isHiddenButActive;
			AllResourcesScreen.Instance.Show(!flag);
		}));
		this.seeAllLabel = this.seeAllButton.GetComponentInChildren<LocText>();
		MultiToggle component3 = this.clearNewButton.GetComponent<MultiToggle>();
		component3.onClick = (System.Action)Delegate.Combine(component3.onClick, new System.Action(delegate()
		{
			this.ClearAllNew();
		}));
		this.clearAllButton.onClick += delegate()
		{
			this.ClearAllNew();
			this.UnPinAll();
			this.Refresh();
		};
		AllResourcesScreen.Instance.Init();
		this.Refresh();
	}

	// Token: 0x060067F2 RID: 26610 RVA: 0x0026D37F File Offset: 0x0026B57F
	protected override void OnForcedCleanUp()
	{
		PinnedResourcesPanel.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060067F3 RID: 26611 RVA: 0x0026D38D File Offset: 0x0026B58D
	public void ClearExcessiveNewItems()
	{
		if (DiscoveredResources.Instance.CheckAllDiscoveredAreNew())
		{
			DiscoveredResources.Instance.newDiscoveries.Clear();
		}
	}

	// Token: 0x060067F4 RID: 26612 RVA: 0x0026D3AC File Offset: 0x0026B5AC
	private void ClearAllNew()
	{
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair in this.rows)
		{
			if (keyValuePair.Value.gameObject.activeSelf && DiscoveredResources.Instance.newDiscoveries.ContainsKey(keyValuePair.Key))
			{
				DiscoveredResources.Instance.newDiscoveries.Remove(keyValuePair.Key);
			}
		}
	}

	// Token: 0x060067F5 RID: 26613 RVA: 0x0026D43C File Offset: 0x0026B63C
	private void UnPinAll()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair in this.rows)
		{
			worldInventory.pinnedResources.Remove(keyValuePair.Key);
		}
	}

	// Token: 0x060067F6 RID: 26614 RVA: 0x0026D4B8 File Offset: 0x0026B6B8
	private PinnedResourcesPanel.PinnedResourceRow CreateRow(Tag tag)
	{
		PinnedResourcesPanel.PinnedResourceRow pinnedResourceRow = new PinnedResourcesPanel.PinnedResourceRow(tag);
		GameObject gameObject = Util.KInstantiateUI(this.linePrefab, this.rowContainer, false);
		pinnedResourceRow.gameObject = gameObject;
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		pinnedResourceRow.icon = component.GetReference<Image>("Icon");
		pinnedResourceRow.nameLabel = component.GetReference<LocText>("NameLabel");
		pinnedResourceRow.valueLabel = component.GetReference<LocText>("ValueLabel");
		pinnedResourceRow.pinToggle = component.GetReference<MultiToggle>("PinToggle");
		pinnedResourceRow.notifyToggle = component.GetReference<MultiToggle>("NotifyToggle");
		pinnedResourceRow.newLabel = component.GetReference<MultiToggle>("NewLabel");
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
		pinnedResourceRow.icon.sprite = uisprite.first;
		pinnedResourceRow.icon.color = uisprite.second;
		pinnedResourceRow.nameLabel.SetText(tag.ProperNameStripLink());
		MultiToggle component2 = pinnedResourceRow.gameObject.GetComponent<MultiToggle>();
		component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
		{
			List<Pickupable> list = ClusterManager.Instance.activeWorld.worldInventory.CreatePickupablesList(tag);
			if (list != null && list.Count > 0)
			{
				SelectTool.Instance.SelectAndFocus(list[this.clickIdx % list.Count].transform.position, list[this.clickIdx % list.Count].GetComponent<KSelectable>());
				this.clickIdx++;
				return;
			}
			this.clickIdx = 0;
		}));
		return pinnedResourceRow;
	}

	// Token: 0x060067F7 RID: 26615 RVA: 0x0026D5E8 File Offset: 0x0026B7E8
	public void Populate(object data = null)
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		foreach (KeyValuePair<Tag, float> keyValuePair in DiscoveredResources.Instance.newDiscoveries)
		{
			if (!this.rows.ContainsKey(keyValuePair.Key) && this.IsDisplayedTag(keyValuePair.Key))
			{
				this.rows.Add(keyValuePair.Key, this.CreateRow(keyValuePair.Key));
			}
		}
		foreach (Tag tag in worldInventory.pinnedResources)
		{
			if (!this.rows.ContainsKey(tag))
			{
				this.rows.Add(tag, this.CreateRow(tag));
			}
		}
		foreach (Tag tag2 in worldInventory.notifyResources)
		{
			if (!this.rows.ContainsKey(tag2))
			{
				this.rows.Add(tag2, this.CreateRow(tag2));
			}
		}
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair2 in this.rows)
		{
			if (false || worldInventory.pinnedResources.Contains(keyValuePair2.Key) || worldInventory.notifyResources.Contains(keyValuePair2.Key) || (DiscoveredResources.Instance.newDiscoveries.ContainsKey(keyValuePair2.Key) && worldInventory.GetAmount(keyValuePair2.Key, false) > 0f))
			{
				if (!keyValuePair2.Value.gameObject.activeSelf)
				{
					keyValuePair2.Value.gameObject.SetActive(true);
				}
			}
			else if (keyValuePair2.Value.gameObject.activeSelf)
			{
				keyValuePair2.Value.gameObject.SetActive(false);
			}
		}
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair3 in this.rows)
		{
			keyValuePair3.Value.pinToggle.gameObject.SetActive(worldInventory.pinnedResources.Contains(keyValuePair3.Key));
		}
		this.SortRows();
		this.rowContainerLayout.ForceUpdate();
	}

	// Token: 0x060067F8 RID: 26616 RVA: 0x0026D8C0 File Offset: 0x0026BAC0
	private void SortRows()
	{
		List<PinnedResourcesPanel.PinnedResourceRow> list = new List<PinnedResourcesPanel.PinnedResourceRow>();
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair in this.rows)
		{
			list.Add(keyValuePair.Value);
		}
		list.Sort((PinnedResourcesPanel.PinnedResourceRow a, PinnedResourcesPanel.PinnedResourceRow b) => a.SortableNameWithoutLink.CompareTo(b.SortableNameWithoutLink));
		foreach (PinnedResourcesPanel.PinnedResourceRow pinnedResourceRow in list)
		{
			this.rows[pinnedResourceRow.Tag].gameObject.transform.SetAsLastSibling();
		}
		this.clearNewButton.transform.SetAsLastSibling();
		this.seeAllButton.transform.SetAsLastSibling();
	}

	// Token: 0x060067F9 RID: 26617 RVA: 0x0026D9BC File Offset: 0x0026BBBC
	private bool IsDisplayedTag(Tag tag)
	{
		foreach (TagSet tagSet in AllResourcesScreen.Instance.allowDisplayCategories)
		{
			foreach (KeyValuePair<Tag, HashSet<Tag>> keyValuePair in DiscoveredResources.Instance.GetDiscoveredResourcesFromTagSet(tagSet))
			{
				if (keyValuePair.Value.Contains(tag))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060067FA RID: 26618 RVA: 0x0026DA64 File Offset: 0x0026BC64
	private void SyncRows()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		bool flag = false;
		foreach (Tag key in worldInventory.pinnedResources)
		{
			if (!this.rows.ContainsKey(key))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			foreach (KeyValuePair<Tag, float> keyValuePair in DiscoveredResources.Instance.newDiscoveries)
			{
				if (!this.rows.ContainsKey(keyValuePair.Key) && this.IsDisplayedTag(keyValuePair.Key))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			foreach (Tag key2 in worldInventory.notifyResources)
			{
				if (!this.rows.ContainsKey(key2))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair2 in this.rows)
			{
				if ((worldInventory.pinnedResources.Contains(keyValuePair2.Key) || worldInventory.notifyResources.Contains(keyValuePair2.Key) || (DiscoveredResources.Instance.newDiscoveries.ContainsKey(keyValuePair2.Key) && worldInventory.GetAmount(keyValuePair2.Key, false) > 0f)) != keyValuePair2.Value.gameObject.activeSelf)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			this.Populate(null);
		}
	}

	// Token: 0x060067FB RID: 26619 RVA: 0x0026DC60 File Offset: 0x0026BE60
	public void Refresh()
	{
		this.SyncRows();
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		bool flag = false;
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair in this.rows)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				this.RefreshLine(keyValuePair.Key, worldInventory, false);
				flag = (flag || DiscoveredResources.Instance.newDiscoveries.ContainsKey(keyValuePair.Key));
			}
		}
		this.clearNewButton.gameObject.SetActive(flag);
		this.seeAllLabel.SetText(string.Format(UI.RESOURCESCREEN.SEE_ALL, AllResourcesScreen.Instance.UniqueResourceRowCount()));
	}

	// Token: 0x060067FC RID: 26620 RVA: 0x0026DD48 File Offset: 0x0026BF48
	private void RefreshLine(Tag tag, WorldInventory inventory, bool initialConfig = false)
	{
		Tag tag2 = tag;
		if (!AllResourcesScreen.Instance.units.ContainsKey(tag))
		{
			AllResourcesScreen.Instance.units.Add(tag, GameUtil.MeasureUnit.quantity);
		}
		if (!inventory.HasValidCount)
		{
			this.rows[tag].valueLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
		}
		else
		{
			switch (AllResourcesScreen.Instance.units[tag])
			{
			case GameUtil.MeasureUnit.mass:
			{
				float amount = inventory.GetAmount(tag2, false);
				if (this.rows[tag].CheckAmountChanged(amount, true))
				{
					this.rows[tag].valueLabel.SetText(GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				}
				break;
			}
			case GameUtil.MeasureUnit.kcal:
			{
				float num = WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(tag.Name, ClusterManager.Instance.activeWorld.worldInventory, true);
				if (this.rows[tag].CheckAmountChanged(num, true))
				{
					this.rows[tag].valueLabel.SetText(GameUtil.GetFormattedCalories(num, GameUtil.TimeSlice.None, true));
				}
				break;
			}
			case GameUtil.MeasureUnit.quantity:
			{
				float amount2 = inventory.GetAmount(tag2, false);
				if (this.rows[tag].CheckAmountChanged(amount2, true))
				{
					this.rows[tag].valueLabel.SetText(GameUtil.GetFormattedUnits(amount2, GameUtil.TimeSlice.None, true, ""));
				}
				break;
			}
			}
		}
		this.rows[tag].pinToggle.onClick = delegate()
		{
			inventory.pinnedResources.Remove(tag);
			this.SyncRows();
		};
		this.rows[tag].notifyToggle.onClick = delegate()
		{
			inventory.notifyResources.Remove(tag);
			this.SyncRows();
		};
		this.rows[tag].newLabel.gameObject.SetActive(DiscoveredResources.Instance.newDiscoveries.ContainsKey(tag));
		this.rows[tag].newLabel.onClick = delegate()
		{
			AllResourcesScreen.Instance.Show(!AllResourcesScreen.Instance.gameObject.activeSelf);
		};
	}

	// Token: 0x060067FD RID: 26621 RVA: 0x0026DFDC File Offset: 0x0026C1DC
	public void Render1000ms(float dt)
	{
		if (this.headerButton != null && this.headerButton.CurrentState == 0)
		{
			return;
		}
		this.Refresh();
	}

	// Token: 0x04004628 RID: 17960
	public GameObject linePrefab;

	// Token: 0x04004629 RID: 17961
	public GameObject rowContainer;

	// Token: 0x0400462A RID: 17962
	public MultiToggle headerButton;

	// Token: 0x0400462B RID: 17963
	public MultiToggle clearNewButton;

	// Token: 0x0400462C RID: 17964
	public KButton clearAllButton;

	// Token: 0x0400462D RID: 17965
	public MultiToggle seeAllButton;

	// Token: 0x0400462E RID: 17966
	private LocText seeAllLabel;

	// Token: 0x0400462F RID: 17967
	private QuickLayout rowContainerLayout;

	// Token: 0x04004630 RID: 17968
	private Dictionary<Tag, PinnedResourcesPanel.PinnedResourceRow> rows = new Dictionary<Tag, PinnedResourcesPanel.PinnedResourceRow>();

	// Token: 0x04004631 RID: 17969
	public static PinnedResourcesPanel Instance;

	// Token: 0x04004632 RID: 17970
	private int clickIdx;

	// Token: 0x02001E2D RID: 7725
	public class PinnedResourceRow
	{
		// Token: 0x0600AAB9 RID: 43705 RVA: 0x003A3002 File Offset: 0x003A1202
		public PinnedResourceRow(Tag tag)
		{
			this.Tag = tag;
			this.SortableNameWithoutLink = tag.ProperNameStripLink();
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x0600AABA RID: 43706 RVA: 0x003A3028 File Offset: 0x003A1228
		// (set) Token: 0x0600AABB RID: 43707 RVA: 0x003A3030 File Offset: 0x003A1230
		public Tag Tag { get; private set; }

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x0600AABC RID: 43708 RVA: 0x003A3039 File Offset: 0x003A1239
		// (set) Token: 0x0600AABD RID: 43709 RVA: 0x003A3041 File Offset: 0x003A1241
		public string SortableNameWithoutLink { get; private set; }

		// Token: 0x0600AABE RID: 43710 RVA: 0x003A304A File Offset: 0x003A124A
		public bool CheckAmountChanged(float newResourceAmount, bool updateIfTrue)
		{
			bool flag = newResourceAmount != this.oldResourceAmount;
			if (flag && updateIfTrue)
			{
				this.oldResourceAmount = newResourceAmount;
			}
			return flag;
		}

		// Token: 0x04008993 RID: 35219
		public GameObject gameObject;

		// Token: 0x04008994 RID: 35220
		public Image icon;

		// Token: 0x04008995 RID: 35221
		public LocText nameLabel;

		// Token: 0x04008996 RID: 35222
		public LocText valueLabel;

		// Token: 0x04008997 RID: 35223
		public MultiToggle pinToggle;

		// Token: 0x04008998 RID: 35224
		public MultiToggle notifyToggle;

		// Token: 0x04008999 RID: 35225
		public MultiToggle newLabel;

		// Token: 0x0400899A RID: 35226
		private float oldResourceAmount = -1f;
	}
}
