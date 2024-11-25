using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DA8 RID: 3496
public abstract class SingleItemSelectionSideScreenBase : SideScreenContent
{
	// Token: 0x06006E64 RID: 28260 RVA: 0x00297EEE File Offset: 0x002960EE
	private static bool TagContainsSearchWord(Tag tag, string search)
	{
		return string.IsNullOrEmpty(search) || tag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
	}

	// Token: 0x170007C2 RID: 1986
	// (get) Token: 0x06006E66 RID: 28262 RVA: 0x00297F19 File Offset: 0x00296119
	// (set) Token: 0x06006E65 RID: 28261 RVA: 0x00297F10 File Offset: 0x00296110
	private protected SingleItemSelectionRow CurrentSelectedItem { protected get; private set; }

	// Token: 0x06006E67 RID: 28263 RVA: 0x00297F24 File Offset: 0x00296124
	protected override void OnPrefabInit()
	{
		if (this.searchbar != null)
		{
			this.searchbar.EditingStateChanged = new Action<bool>(this.OnSearchbarEditStateChanged);
			this.searchbar.ValueChanged = new Action<string>(this.OnSearchBarValueChanged);
			this.activateOnSpawn = true;
		}
		base.OnPrefabInit();
	}

	// Token: 0x06006E68 RID: 28264 RVA: 0x00297F7C File Offset: 0x0029617C
	protected virtual void OnSearchbarEditStateChanged(bool isEditing)
	{
		base.isEditing = isEditing;
	}

	// Token: 0x06006E69 RID: 28265 RVA: 0x00297F88 File Offset: 0x00296188
	protected virtual void OnSearchBarValueChanged(string value)
	{
		foreach (Tag tag in this.categories.Keys)
		{
			SingleItemSelectionSideScreenBase.Category category = this.categories[tag];
			bool flag = SingleItemSelectionSideScreenBase.TagContainsSearchWord(tag, value);
			int num = category.FilterItemsBySearch(flag ? null : value);
			category.SetUnfoldedState((num > 0) ? SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded : SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded);
			category.SetVisibilityState(flag || num > 0);
		}
	}

	// Token: 0x06006E6A RID: 28266 RVA: 0x00298018 File Offset: 0x00296218
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return base.GetSortKey();
	}

	// Token: 0x06006E6B RID: 28267 RVA: 0x0029802E File Offset: 0x0029622E
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x06006E6C RID: 28268 RVA: 0x00298048 File Offset: 0x00296248
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x06006E6D RID: 28269 RVA: 0x00298064 File Offset: 0x00296264
	public virtual void SetData(Dictionary<Tag, HashSet<Tag>> data)
	{
		this.ProhibitAllCategories();
		foreach (Tag tag in data.Keys)
		{
			ICollection<Tag> items = data[tag];
			this.CreateCategoryWithItems(tag, items);
		}
		this.SortAll();
		if (this.searchbar != null && !string.IsNullOrEmpty(this.searchbar.CurrentSearchValue))
		{
			this.searchbar.ClearSearch();
		}
	}

	// Token: 0x06006E6E RID: 28270 RVA: 0x002980F8 File Offset: 0x002962F8
	public virtual SingleItemSelectionSideScreenBase.Category CreateCategoryWithItems(Tag categoryTag, ICollection<Tag> items)
	{
		SingleItemSelectionSideScreenBase.Category orCreateEmptyCategory = this.GetOrCreateEmptyCategory(categoryTag);
		if (!orCreateEmptyCategory.InitializeItemList(items.Count))
		{
			orCreateEmptyCategory.RemoveAllItems();
		}
		foreach (Tag itemTag in items)
		{
			SingleItemSelectionRow orCreateItemRow = this.GetOrCreateItemRow(itemTag);
			orCreateEmptyCategory.AddItem(orCreateItemRow);
		}
		return orCreateEmptyCategory;
	}

	// Token: 0x06006E6F RID: 28271 RVA: 0x00298168 File Offset: 0x00296368
	public virtual SingleItemSelectionSideScreenBase.Category GetOrCreateEmptyCategory(Tag categoryTag)
	{
		this.original_CategoryRow.gameObject.SetActive(false);
		SingleItemSelectionSideScreenBase.Category category = null;
		if (!this.categories.TryGetValue(categoryTag, out category))
		{
			HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.original_CategoryRow.gameObject, this.original_CategoryRow.transform.parent.gameObject, false);
			hierarchyReferences.gameObject.SetActive(true);
			category = new SingleItemSelectionSideScreenBase.Category(hierarchyReferences, categoryTag);
			category.ItemRemoved = new Action<SingleItemSelectionRow>(this.RecycleItemRow);
			SingleItemSelectionSideScreenBase.Category category2 = category;
			category2.ToggleClicked = (Action<SingleItemSelectionSideScreenBase.Category>)Delegate.Combine(category2.ToggleClicked, new Action<SingleItemSelectionSideScreenBase.Category>(this.CategoryToggleClicked));
			this.categories.Add(categoryTag, category);
		}
		else
		{
			category.SetProihibedState(false);
			category.SetVisibilityState(true);
		}
		return category;
	}

	// Token: 0x06006E70 RID: 28272 RVA: 0x00298224 File Offset: 0x00296424
	public virtual SingleItemSelectionRow GetOrCreateItemRow(Tag itemTag)
	{
		this.original_ItemRow.gameObject.SetActive(false);
		SingleItemSelectionRow singleItemSelectionRow = null;
		if (!this.pooledRows.TryGetValue(itemTag, out singleItemSelectionRow))
		{
			singleItemSelectionRow = Util.KInstantiateUI<SingleItemSelectionRow>(this.original_ItemRow.gameObject, this.original_ItemRow.transform.parent.gameObject, false);
			UnityEngine.Object @object = singleItemSelectionRow;
			string str = "Item-";
			Tag tag = itemTag;
			@object.name = str + tag.ToString();
		}
		else
		{
			this.pooledRows.Remove(itemTag);
		}
		singleItemSelectionRow.gameObject.SetActive(true);
		singleItemSelectionRow.SetTag(itemTag);
		singleItemSelectionRow.Clicked = new Action<SingleItemSelectionRow>(this.ItemRowClicked);
		singleItemSelectionRow.SetVisibleState(true);
		return singleItemSelectionRow;
	}

	// Token: 0x06006E71 RID: 28273 RVA: 0x002982D8 File Offset: 0x002964D8
	public SingleItemSelectionSideScreenBase.Category GetCategoryWithItem(Tag itemTag, bool includeNotVisibleCategories = false)
	{
		foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
		{
			if ((includeNotVisibleCategories || category.IsVisible) && category.GetItem(itemTag) != null)
			{
				return category;
			}
		}
		return null;
	}

	// Token: 0x06006E72 RID: 28274 RVA: 0x0029834C File Offset: 0x0029654C
	public virtual void SetSelectedItem(SingleItemSelectionRow itemRow)
	{
		if (this.CurrentSelectedItem != null)
		{
			this.CurrentSelectedItem.SetSelected(false);
		}
		this.CurrentSelectedItem = itemRow;
		if (itemRow != null)
		{
			itemRow.SetSelected(true);
		}
	}

	// Token: 0x06006E73 RID: 28275 RVA: 0x00298380 File Offset: 0x00296580
	public virtual bool SetSelectedItem(Tag itemTag)
	{
		foreach (Tag key in this.categories.Keys)
		{
			SingleItemSelectionSideScreenBase.Category category = this.categories[key];
			if (category.IsVisible)
			{
				SingleItemSelectionRow item = category.GetItem(itemTag);
				if (item != null)
				{
					this.SetSelectedItem(item);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06006E74 RID: 28276 RVA: 0x00298408 File Offset: 0x00296608
	public virtual void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		this.SetSelectedItem(rowClicked);
	}

	// Token: 0x06006E75 RID: 28277 RVA: 0x00298411 File Offset: 0x00296611
	public virtual void CategoryToggleClicked(SingleItemSelectionSideScreenBase.Category categoryClicked)
	{
		categoryClicked.ToggleUnfoldedState();
	}

	// Token: 0x06006E76 RID: 28278 RVA: 0x0029841C File Offset: 0x0029661C
	private void RecycleItemRow(SingleItemSelectionRow row)
	{
		if (this.pooledRows.ContainsKey(row.tag))
		{
			global::Debug.LogError(string.Format("Recycling an item row with tag {0} that was already in the recycle pool", row.tag));
		}
		if (this.CurrentSelectedItem == row)
		{
			this.SetSelectedItem(null);
		}
		row.Clicked = null;
		row.SetSelected(false);
		row.transform.SetParent(this.original_ItemRow.transform.parent.parent);
		row.gameObject.SetActive(false);
		this.pooledRows.Add(row.tag, row);
	}

	// Token: 0x06006E77 RID: 28279 RVA: 0x002984B8 File Offset: 0x002966B8
	private void ProhibitAllCategories()
	{
		foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
		{
			category.SetProihibedState(true);
		}
	}

	// Token: 0x06006E78 RID: 28280 RVA: 0x00298510 File Offset: 0x00296710
	public virtual void SortAll()
	{
		foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
		{
			if (category.IsVisible)
			{
				category.Sort();
				category.SendToLastSibiling();
			}
		}
	}

	// Token: 0x04004B56 RID: 19286
	[Space]
	[Header("Settings")]
	[SerializeField]
	private SearchBar searchbar;

	// Token: 0x04004B57 RID: 19287
	[SerializeField]
	protected HierarchyReferences original_CategoryRow;

	// Token: 0x04004B58 RID: 19288
	[SerializeField]
	protected SingleItemSelectionRow original_ItemRow;

	// Token: 0x04004B59 RID: 19289
	protected SortedDictionary<Tag, SingleItemSelectionSideScreenBase.Category> categories = new SortedDictionary<Tag, SingleItemSelectionSideScreenBase.Category>(SingleItemSelectionSideScreenBase.categoryComparer);

	// Token: 0x04004B5A RID: 19290
	private Dictionary<Tag, SingleItemSelectionRow> pooledRows = new Dictionary<Tag, SingleItemSelectionRow>();

	// Token: 0x04004B5B RID: 19291
	private static TagNameComparer categoryComparer = new TagNameComparer(GameTags.Void);

	// Token: 0x04004B5C RID: 19292
	private static SingleItemSelectionSideScreenBase.ItemComparer itemRowComparer = new SingleItemSelectionSideScreenBase.ItemComparer(GameTags.Void);

	// Token: 0x02001EC1 RID: 7873
	public class ItemComparer : IComparer<SingleItemSelectionRow>
	{
		// Token: 0x0600AC57 RID: 44119 RVA: 0x003A6F79 File Offset: 0x003A5179
		public ItemComparer()
		{
		}

		// Token: 0x0600AC58 RID: 44120 RVA: 0x003A6F81 File Offset: 0x003A5181
		public ItemComparer(Tag firstTag)
		{
			this.firstTag = firstTag;
		}

		// Token: 0x0600AC59 RID: 44121 RVA: 0x003A6F90 File Offset: 0x003A5190
		public int Compare(SingleItemSelectionRow x, SingleItemSelectionRow y)
		{
			if (x == y)
			{
				return 0;
			}
			if (this.firstTag.IsValid)
			{
				if (x.tag == this.firstTag && y.tag != this.firstTag)
				{
					return 1;
				}
				if (x.tag != this.firstTag && y.tag == this.firstTag)
				{
					return -1;
				}
			}
			return x.tag.ProperNameStripLink().CompareTo(y.tag.ProperNameStripLink());
		}

		// Token: 0x04008B6A RID: 35690
		private Tag firstTag;
	}

	// Token: 0x02001EC2 RID: 7874
	public class Category
	{
		// Token: 0x0600AC5A RID: 44122 RVA: 0x003A7020 File Offset: 0x003A5220
		public virtual void ToggleUnfoldedState()
		{
			SingleItemSelectionSideScreenBase.Category.UnfoldedStates currentState = (SingleItemSelectionSideScreenBase.Category.UnfoldedStates)this.toggle.CurrentState;
			if (currentState == SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded)
			{
				this.SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
				return;
			}
			if (currentState != SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded)
			{
				return;
			}
			this.SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded);
		}

		// Token: 0x0600AC5B RID: 44123 RVA: 0x003A7050 File Offset: 0x003A5250
		public virtual void SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates new_state)
		{
			this.toggle.ChangeState((int)new_state);
			this.entries.gameObject.SetActive(new_state == SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
		}

		// Token: 0x0600AC5C RID: 44124 RVA: 0x003A7072 File Offset: 0x003A5272
		public virtual void SetTitle(string text)
		{
			this.title.text = text;
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x0600AC5E RID: 44126 RVA: 0x003A7089 File Offset: 0x003A5289
		// (set) Token: 0x0600AC5D RID: 44125 RVA: 0x003A7080 File Offset: 0x003A5280
		public Tag CategoryTag { get; protected set; }

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x0600AC60 RID: 44128 RVA: 0x003A709A File Offset: 0x003A529A
		// (set) Token: 0x0600AC5F RID: 44127 RVA: 0x003A7091 File Offset: 0x003A5291
		public bool IsProhibited { get; protected set; }

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x0600AC61 RID: 44129 RVA: 0x003A70A2 File Offset: 0x003A52A2
		public bool IsVisible
		{
			get
			{
				return this.hierarchyReferences != null && this.hierarchyReferences.gameObject.activeSelf;
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x0600AC62 RID: 44130 RVA: 0x003A70C4 File Offset: 0x003A52C4
		protected RectTransform entries
		{
			get
			{
				return this.hierarchyReferences.GetReference<RectTransform>("Entries");
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x0600AC63 RID: 44131 RVA: 0x003A70D6 File Offset: 0x003A52D6
		protected LocText title
		{
			get
			{
				return this.hierarchyReferences.GetReference<LocText>("Label");
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x0600AC64 RID: 44132 RVA: 0x003A70E8 File Offset: 0x003A52E8
		protected MultiToggle toggle
		{
			get
			{
				return this.hierarchyReferences.GetReference<MultiToggle>("Toggle");
			}
		}

		// Token: 0x0600AC65 RID: 44133 RVA: 0x003A70FA File Offset: 0x003A52FA
		public Category(HierarchyReferences references, Tag categoryTag)
		{
			this.CategoryTag = categoryTag;
			this.hierarchyReferences = references;
			this.toggle.onClick = new System.Action(this.OnToggleClicked);
			this.SetTitle(categoryTag.ProperName());
		}

		// Token: 0x0600AC66 RID: 44134 RVA: 0x003A7134 File Offset: 0x003A5334
		public virtual void OnToggleClicked()
		{
			Action<SingleItemSelectionSideScreenBase.Category> toggleClicked = this.ToggleClicked;
			if (toggleClicked == null)
			{
				return;
			}
			toggleClicked(this);
		}

		// Token: 0x0600AC67 RID: 44135 RVA: 0x003A7148 File Offset: 0x003A5348
		public virtual void AddItems(SingleItemSelectionRow[] _items)
		{
			if (this.items == null)
			{
				this.items = new List<SingleItemSelectionRow>(_items);
				return;
			}
			for (int i = 0; i < _items.Length; i++)
			{
				if (!this.items.Contains(_items[i]))
				{
					_items[i].transform.SetParent(this.entries, false);
					this.items.Add(_items[i]);
				}
			}
		}

		// Token: 0x0600AC68 RID: 44136 RVA: 0x003A71AA File Offset: 0x003A53AA
		public virtual void AddItem(SingleItemSelectionRow item)
		{
			if (this.items == null)
			{
				this.items = new List<SingleItemSelectionRow>();
			}
			item.transform.SetParent(this.entries, false);
			this.items.Add(item);
		}

		// Token: 0x0600AC69 RID: 44137 RVA: 0x003A71DD File Offset: 0x003A53DD
		public virtual bool InitializeItemList(int size)
		{
			if (this.items == null)
			{
				this.items = new List<SingleItemSelectionRow>(size);
				return true;
			}
			return false;
		}

		// Token: 0x0600AC6A RID: 44138 RVA: 0x003A71F6 File Offset: 0x003A53F6
		public virtual void SetVisibilityState(bool isVisible)
		{
			this.hierarchyReferences.gameObject.SetActive(isVisible && !this.IsProhibited);
		}

		// Token: 0x0600AC6B RID: 44139 RVA: 0x003A7218 File Offset: 0x003A5418
		public virtual void RemoveAllItems()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				SingleItemSelectionRow obj = this.items[i];
				Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
				if (itemRemoved != null)
				{
					itemRemoved(obj);
				}
			}
			this.items.Clear();
			this.items = null;
		}

		// Token: 0x0600AC6C RID: 44140 RVA: 0x003A726C File Offset: 0x003A546C
		public virtual SingleItemSelectionRow RemoveItem(Tag itemTag)
		{
			if (this.items != null)
			{
				SingleItemSelectionRow singleItemSelectionRow = this.items.Find((SingleItemSelectionRow row) => row.tag == itemTag);
				if (singleItemSelectionRow != null)
				{
					Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
					if (itemRemoved != null)
					{
						itemRemoved(singleItemSelectionRow);
					}
					return singleItemSelectionRow;
				}
			}
			return null;
		}

		// Token: 0x0600AC6D RID: 44141 RVA: 0x003A72C4 File Offset: 0x003A54C4
		public virtual bool RemoveItem(SingleItemSelectionRow itemRow)
		{
			if (this.items != null && this.items.Remove(itemRow))
			{
				Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
				if (itemRemoved != null)
				{
					itemRemoved(itemRow);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600AC6E RID: 44142 RVA: 0x003A72F4 File Offset: 0x003A54F4
		public SingleItemSelectionRow GetItem(Tag itemTag)
		{
			if (this.items == null)
			{
				return null;
			}
			return this.items.Find((SingleItemSelectionRow row) => row.tag == itemTag);
		}

		// Token: 0x0600AC6F RID: 44143 RVA: 0x003A7330 File Offset: 0x003A5530
		public int FilterItemsBySearch(string searchValue)
		{
			int num = 0;
			if (this.items != null)
			{
				foreach (SingleItemSelectionRow singleItemSelectionRow in this.items)
				{
					bool flag = SingleItemSelectionSideScreenBase.TagContainsSearchWord(singleItemSelectionRow.tag, searchValue);
					singleItemSelectionRow.SetVisibleState(flag);
					if (flag)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600AC70 RID: 44144 RVA: 0x003A73A0 File Offset: 0x003A55A0
		public void Sort()
		{
			if (this.items != null)
			{
				this.items.Sort(SingleItemSelectionSideScreenBase.itemRowComparer);
				foreach (SingleItemSelectionRow singleItemSelectionRow in this.items)
				{
					singleItemSelectionRow.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x0600AC71 RID: 44145 RVA: 0x003A7410 File Offset: 0x003A5610
		public void SendToLastSibiling()
		{
			this.hierarchyReferences.transform.SetAsLastSibling();
		}

		// Token: 0x0600AC72 RID: 44146 RVA: 0x003A7422 File Offset: 0x003A5622
		public void SetProihibedState(bool isPohibited)
		{
			this.IsProhibited = isPohibited;
			if (this.IsVisible && isPohibited)
			{
				this.SetVisibilityState(false);
			}
		}

		// Token: 0x04008B6B RID: 35691
		public Action<SingleItemSelectionRow> ItemRemoved;

		// Token: 0x04008B6C RID: 35692
		public Action<SingleItemSelectionSideScreenBase.Category> ToggleClicked;

		// Token: 0x04008B6F RID: 35695
		protected HierarchyReferences hierarchyReferences;

		// Token: 0x04008B70 RID: 35696
		protected List<SingleItemSelectionRow> items;

		// Token: 0x02002660 RID: 9824
		public enum UnfoldedStates
		{
			// Token: 0x0400AA77 RID: 43639
			Folded,
			// Token: 0x0400AA78 RID: 43640
			Unfolded
		}
	}
}
