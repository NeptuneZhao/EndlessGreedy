using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DB7 RID: 3511
public class TreeFilterableSideScreen : SideScreenContent
{
	// Token: 0x170007C7 RID: 1991
	// (get) Token: 0x06006F00 RID: 28416 RVA: 0x0029B06C File Offset: 0x0029926C
	private bool InputFieldEmpty
	{
		get
		{
			return this.inputField.text == "";
		}
	}

	// Token: 0x170007C8 RID: 1992
	// (get) Token: 0x06006F01 RID: 28417 RVA: 0x0029B083 File Offset: 0x00299283
	public bool IsStorage
	{
		get
		{
			return this.storage != null;
		}
	}

	// Token: 0x06006F02 RID: 28418 RVA: 0x0029B091 File Offset: 0x00299291
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Initialize();
	}

	// Token: 0x06006F03 RID: 28419 RVA: 0x0029B0A0 File Offset: 0x002992A0
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.rowPool = new UIPool<TreeFilterableSideScreenRow>(this.rowPrefab);
		this.elementPool = new UIPool<TreeFilterableSideScreenElement>(this.elementPrefab);
		MultiToggle multiToggle = this.allCheckBox;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			TreeFilterableSideScreenRow.State allCheckboxState = this.GetAllCheckboxState();
			if (allCheckboxState > TreeFilterableSideScreenRow.State.Mixed)
			{
				if (allCheckboxState == TreeFilterableSideScreenRow.State.On)
				{
					this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.Off);
					return;
				}
			}
			else
			{
				this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.On);
			}
		}));
		this.onlyAllowTransportItemsCheckBox.onClick = new System.Action(this.OnlyAllowTransportItemsClicked);
		this.onlyAllowSpicedItemsCheckBox.onClick = new System.Action(this.OnlyAllowSpicedItemsClicked);
		this.initialized = true;
	}

	// Token: 0x06006F04 RID: 28420 RVA: 0x0029B134 File Offset: 0x00299334
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.allCheckBox.transform.parent.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTONTOOLTIP);
		this.onlyAllowTransportItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP);
		this.onlyAllowSpicedItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWSPICEDITEMSBUTTONTOOLTIP);
		this.inputField.ActivateInputField();
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
		this.InitSearch();
	}

	// Token: 0x06006F05 RID: 28421 RVA: 0x0029B1E8 File Offset: 0x002993E8
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return base.GetSortKey();
	}

	// Token: 0x06006F06 RID: 28422 RVA: 0x0029B1FE File Offset: 0x002993FE
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

	// Token: 0x06006F07 RID: 28423 RVA: 0x0029B218 File Offset: 0x00299418
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

	// Token: 0x06006F08 RID: 28424 RVA: 0x0029B232 File Offset: 0x00299432
	public override int GetSideScreenSortOrder()
	{
		return 1;
	}

	// Token: 0x06006F09 RID: 28425 RVA: 0x0029B238 File Offset: 0x00299438
	private void UpdateAllCheckBoxVisualState()
	{
		switch (this.GetAllCheckboxState())
		{
		case TreeFilterableSideScreenRow.State.Off:
			this.allCheckBox.ChangeState(0);
			return;
		case TreeFilterableSideScreenRow.State.Mixed:
			this.allCheckBox.ChangeState(1);
			return;
		case TreeFilterableSideScreenRow.State.On:
			this.allCheckBox.ChangeState(2);
			return;
		default:
			return;
		}
	}

	// Token: 0x06006F0A RID: 28426 RVA: 0x0029B288 File Offset: 0x00299488
	public void Update()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (keyValuePair.Value.visualDirty)
			{
				this.visualDirty = true;
				break;
			}
		}
		if (this.visualDirty)
		{
			foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair2 in this.tagRowMap)
			{
				keyValuePair2.Value.RefreshRowElements();
				keyValuePair2.Value.UpdateCheckBoxVisualState();
			}
			this.UpdateAllCheckBoxVisualState();
			this.visualDirty = false;
		}
	}

	// Token: 0x06006F0B RID: 28427 RVA: 0x0029B354 File Offset: 0x00299554
	private void OnlyAllowTransportItemsClicked()
	{
		this.storage.SetOnlyFetchMarkedItems(!this.storage.GetOnlyFetchMarkedItems());
	}

	// Token: 0x06006F0C RID: 28428 RVA: 0x0029B36F File Offset: 0x0029956F
	private void OnlyAllowSpicedItemsClicked()
	{
		FoodStorage component = this.storage.GetComponent<FoodStorage>();
		component.SpicedFoodOnly = !component.SpicedFoodOnly;
	}

	// Token: 0x06006F0D RID: 28429 RVA: 0x0029B38C File Offset: 0x0029958C
	private TreeFilterableSideScreenRow.State GetAllCheckboxState()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (keyValuePair.Value.standardCommodity)
			{
				switch (keyValuePair.Value.GetState())
				{
				case TreeFilterableSideScreenRow.State.Off:
					flag2 = true;
					break;
				case TreeFilterableSideScreenRow.State.Mixed:
					flag3 = true;
					break;
				case TreeFilterableSideScreenRow.State.On:
					flag = true;
					break;
				}
			}
		}
		if (flag3)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		if (flag && !flag2)
		{
			return TreeFilterableSideScreenRow.State.On;
		}
		if (!flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		if (flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		return TreeFilterableSideScreenRow.State.Off;
	}

	// Token: 0x06006F0E RID: 28430 RVA: 0x0029B43C File Offset: 0x0029963C
	private void SetAllCheckboxState(TreeFilterableSideScreenRow.State newState)
	{
		switch (newState)
		{
		case TreeFilterableSideScreenRow.State.Off:
			using (Dictionary<Tag, TreeFilterableSideScreenRow>.Enumerator enumerator = this.tagRowMap.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair = enumerator.Current;
					if (keyValuePair.Value.standardCommodity)
					{
						keyValuePair.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
					}
				}
				goto IL_AB;
			}
			break;
		case TreeFilterableSideScreenRow.State.Mixed:
			goto IL_AB;
		case TreeFilterableSideScreenRow.State.On:
			break;
		default:
			goto IL_AB;
		}
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair2 in this.tagRowMap)
		{
			if (keyValuePair2.Value.standardCommodity)
			{
				keyValuePair2.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
			}
		}
		IL_AB:
		this.visualDirty = true;
	}

	// Token: 0x06006F0F RID: 28431 RVA: 0x0029B518 File Offset: 0x00299718
	public bool GetElementTagAcceptedState(Tag t)
	{
		return this.targetFilterable.ContainsTag(t);
	}

	// Token: 0x06006F10 RID: 28432 RVA: 0x0029B528 File Offset: 0x00299728
	public override bool IsValidForTarget(GameObject target)
	{
		TreeFilterable component = target.GetComponent<TreeFilterable>();
		Storage component2 = target.GetComponent<Storage>();
		return component != null && target.GetComponent<FlatTagFilterable>() == null && component.showUserMenu && (component2 == null || component2.showInUI) && target.GetSMI<StorageTile.Instance>() == null;
	}

	// Token: 0x06006F11 RID: 28433 RVA: 0x0029B57E File Offset: 0x0029977E
	private void ReconfigureForPreviousTarget()
	{
		global::Debug.Assert(this.target != null, "TreeFilterableSideScreen trying to restore null target.");
		this.SetTarget(this.target);
	}

	// Token: 0x06006F12 RID: 28434 RVA: 0x0029B5A4 File Offset: 0x002997A4
	public override void SetTarget(GameObject target)
	{
		this.Initialize();
		this.target = target;
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetFilterable = target.GetComponent<TreeFilterable>();
		if (this.targetFilterable == null)
		{
			global::Debug.LogError("The target provided does not have a Tree Filterable component");
			return;
		}
		this.contentMask.GetComponent<LayoutElement>().minHeight = (float)((this.targetFilterable.uiHeight == TreeFilterable.UISideScreenHeight.Tall) ? 380 : 256);
		this.storage = this.targetFilterable.GetComponent<Storage>();
		this.storage.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.storage.Subscribe(1163645216, new Action<object>(this.OnOnlySpicedItemsSettingChanged));
		this.OnOnlyFetchMarkedItemsSettingChanged(null);
		this.OnOnlySpicedItemsSettingChanged(null);
		this.allCheckBoxLabel.SetText(this.targetFilterable.allResourceFilterLabelString);
		this.CreateCategories();
		this.CreateSpecialItemRows();
		this.titlebar.SetActive(false);
		if (this.storage.showSideScreenTitleBar)
		{
			this.titlebar.SetActive(true);
			this.titlebar.GetComponentInChildren<LocText>().SetText(this.storage.GetProperName());
		}
		if (!this.InputFieldEmpty)
		{
			this.ClearSearch();
		}
		this.ToggleSearchConfiguration(!this.InputFieldEmpty);
	}

	// Token: 0x06006F13 RID: 28435 RVA: 0x0029B6FC File Offset: 0x002998FC
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		this.onlyAllowTransportItemsCheckBox.ChangeState(this.storage.GetOnlyFetchMarkedItems() ? 1 : 0);
		if (this.storage.allowSettingOnlyFetchMarkedItems)
		{
			this.onlyallowTransportItemsRow.SetActive(true);
			return;
		}
		this.onlyallowTransportItemsRow.SetActive(false);
	}

	// Token: 0x06006F14 RID: 28436 RVA: 0x0029B74C File Offset: 0x0029994C
	private void OnOnlySpicedItemsSettingChanged(object data)
	{
		FoodStorage component = this.storage.GetComponent<FoodStorage>();
		if (component != null)
		{
			this.onlyallowSpicedItemsRow.SetActive(true);
			this.onlyAllowSpicedItemsCheckBox.ChangeState(component.SpicedFoodOnly ? 1 : 0);
			return;
		}
		this.onlyallowSpicedItemsRow.SetActive(false);
	}

	// Token: 0x06006F15 RID: 28437 RVA: 0x0029B79E File Offset: 0x0029999E
	public bool IsTagAllowed(Tag tag)
	{
		return this.targetFilterable.AcceptedTags.Contains(tag);
	}

	// Token: 0x06006F16 RID: 28438 RVA: 0x0029B7B1 File Offset: 0x002999B1
	public void AddTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		this.targetFilterable.AddTagToFilter(tag);
	}

	// Token: 0x06006F17 RID: 28439 RVA: 0x0029B7CE File Offset: 0x002999CE
	public void RemoveTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		this.targetFilterable.RemoveTagFromFilter(tag);
	}

	// Token: 0x06006F18 RID: 28440 RVA: 0x0029B7EC File Offset: 0x002999EC
	private List<TreeFilterableSideScreen.TagOrderInfo> GetTagsSortedAlphabetically(ICollection<Tag> tags)
	{
		List<TreeFilterableSideScreen.TagOrderInfo> list = new List<TreeFilterableSideScreen.TagOrderInfo>();
		foreach (Tag tag in tags)
		{
			list.Add(new TreeFilterableSideScreen.TagOrderInfo
			{
				tag = tag,
				strippedName = tag.ProperNameStripLink()
			});
		}
		list.Sort((TreeFilterableSideScreen.TagOrderInfo a, TreeFilterableSideScreen.TagOrderInfo b) => a.strippedName.CompareTo(b.strippedName));
		return list;
	}

	// Token: 0x06006F19 RID: 28441 RVA: 0x0029B880 File Offset: 0x00299A80
	private TreeFilterableSideScreenRow AddRow(Tag rowTag)
	{
		if (this.tagRowMap.ContainsKey(rowTag))
		{
			return this.tagRowMap[rowTag];
		}
		TreeFilterableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
		freeElement.Parent = this;
		freeElement.standardCommodity = !STORAGEFILTERS.SPECIAL_STORAGE.Contains(rowTag);
		this.tagRowMap.Add(rowTag, freeElement);
		Dictionary<Tag, bool> dictionary = new Dictionary<Tag, bool>();
		foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically(DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(rowTag)))
		{
			dictionary.Add(tagOrderInfo.tag, this.targetFilterable.ContainsTag(tagOrderInfo.tag) || this.targetFilterable.ContainsTag(rowTag));
		}
		freeElement.SetElement(rowTag, this.targetFilterable.ContainsTag(rowTag), dictionary);
		freeElement.transform.SetAsLastSibling();
		return freeElement;
	}

	// Token: 0x06006F1A RID: 28442 RVA: 0x0029B984 File Offset: 0x00299B84
	public float GetAmountInStorage(Tag tag)
	{
		if (!this.IsStorage)
		{
			return 0f;
		}
		return this.storage.GetMassAvailable(tag);
	}

	// Token: 0x06006F1B RID: 28443 RVA: 0x0029B9A0 File Offset: 0x00299BA0
	private void CreateCategories()
	{
		if (this.storage.storageFilters != null && this.storage.storageFilters.Count >= 1)
		{
			bool flag = this.target.GetComponent<CreatureDeliveryPoint>() != null;
			foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically(this.storage.storageFilters))
			{
				Tag tag = tagOrderInfo.tag;
				if (flag || DiscoveredResources.Instance.IsDiscovered(tag))
				{
					this.AddRow(tag);
				}
			}
			this.visualDirty = true;
			return;
		}
		global::Debug.LogError("If you're filtering, your storage filter should have the filters set on it");
	}

	// Token: 0x06006F1C RID: 28444 RVA: 0x0029BA60 File Offset: 0x00299C60
	private void CreateSpecialItemRows()
	{
		this.specialItemsHeader.transform.SetAsLastSibling();
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (!keyValuePair.Value.standardCommodity)
			{
				keyValuePair.Value.transform.transform.SetAsLastSibling();
			}
		}
		this.RefreshSpecialItemsHeader();
	}

	// Token: 0x06006F1D RID: 28445 RVA: 0x0029BAE8 File Offset: 0x00299CE8
	private void RefreshSpecialItemsHeader()
	{
		bool active = false;
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (!keyValuePair.Value.standardCommodity)
			{
				active = true;
				break;
			}
		}
		this.specialItemsHeader.gameObject.SetActive(active);
	}

	// Token: 0x06006F1E RID: 28446 RVA: 0x0029BB5C File Offset: 0x00299D5C
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (this.target != null && (this.tagRowMap == null || this.tagRowMap.Count == 0))
		{
			this.ReconfigureForPreviousTarget();
		}
	}

	// Token: 0x06006F1F RID: 28447 RVA: 0x0029BB90 File Offset: 0x00299D90
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.storage != null)
		{
			this.storage.Unsubscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
			this.storage.Unsubscribe(1163645216, new Action<object>(this.OnOnlySpicedItemsSettingChanged));
		}
		this.rowPool.ClearAll();
		this.elementPool.ClearAll();
		this.tagRowMap.Clear();
	}

	// Token: 0x06006F20 RID: 28448 RVA: 0x0029BC0C File Offset: 0x00299E0C
	private void RecordRowExpandedStatus()
	{
		this.rowExpandedStatusMemory.Clear();
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			this.rowExpandedStatusMemory.Add(keyValuePair.Key, keyValuePair.Value.ArrowExpanded);
		}
	}

	// Token: 0x06006F21 RID: 28449 RVA: 0x0029BC84 File Offset: 0x00299E84
	private void RestoreRowExpandedStatus()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (this.rowExpandedStatusMemory.ContainsKey(keyValuePair.Key))
			{
				keyValuePair.Value.SetArrowToggleState(this.rowExpandedStatusMemory[keyValuePair.Key]);
			}
		}
	}

	// Token: 0x06006F22 RID: 28450 RVA: 0x0029BD04 File Offset: 0x00299F04
	private void InitSearch()
	{
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
			KScreenManager.Instance.RefreshStack();
			UISounds.PlaySound(UISounds.Sound.ClickHUD);
			this.RecordRowExpandedStatus();
		}));
		this.inputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
			KScreenManager.Instance.RefreshStack();
		});
		this.inputField.onValueChanged.AddListener(delegate(string value)
		{
			if (this.InputFieldEmpty)
			{
				this.RestoreRowExpandedStatus();
			}
			this.ToggleSearchConfiguration(!this.InputFieldEmpty);
			this.UpdateSearchFilter();
		});
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
		this.clearButton.onClick += delegate()
		{
			if (!this.InputFieldEmpty)
			{
				this.ClearSearch();
			}
		};
	}

	// Token: 0x06006F23 RID: 28451 RVA: 0x0029BDA8 File Offset: 0x00299FA8
	private void ToggleSearchConfiguration(bool searching)
	{
		this.configurationRowsContainer.gameObject.SetActive(!searching);
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			keyValuePair.Value.ShowToggleBox(!searching);
		}
		if (searching)
		{
			this.specialItemsHeader.gameObject.SetActive(false);
			return;
		}
		this.RefreshSpecialItemsHeader();
	}

	// Token: 0x06006F24 RID: 28452 RVA: 0x0029BE34 File Offset: 0x0029A034
	private void ClearSearch()
	{
		this.inputField.text = "";
		this.RestoreRowExpandedStatus();
		this.ToggleSearchConfiguration(false);
	}

	// Token: 0x170007C9 RID: 1993
	// (get) Token: 0x06006F25 RID: 28453 RVA: 0x0029BE53 File Offset: 0x0029A053
	public string CurrentSearchValue
	{
		get
		{
			if (this.inputField.text == null)
			{
				return "";
			}
			return this.inputField.text;
		}
	}

	// Token: 0x06006F26 RID: 28454 RVA: 0x0029BE74 File Offset: 0x0029A074
	private void UpdateSearchFilter()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			keyValuePair.Value.FilterAgainstSearch(keyValuePair.Key, this.CurrentSearchValue);
		}
	}

	// Token: 0x04004BC0 RID: 19392
	[SerializeField]
	private MultiToggle allCheckBox;

	// Token: 0x04004BC1 RID: 19393
	[SerializeField]
	private LocText allCheckBoxLabel;

	// Token: 0x04004BC2 RID: 19394
	[SerializeField]
	private GameObject specialItemsHeader;

	// Token: 0x04004BC3 RID: 19395
	[SerializeField]
	private MultiToggle onlyAllowTransportItemsCheckBox;

	// Token: 0x04004BC4 RID: 19396
	[SerializeField]
	private GameObject onlyallowTransportItemsRow;

	// Token: 0x04004BC5 RID: 19397
	[SerializeField]
	private MultiToggle onlyAllowSpicedItemsCheckBox;

	// Token: 0x04004BC6 RID: 19398
	[SerializeField]
	private GameObject onlyallowSpicedItemsRow;

	// Token: 0x04004BC7 RID: 19399
	[SerializeField]
	private TreeFilterableSideScreenRow rowPrefab;

	// Token: 0x04004BC8 RID: 19400
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x04004BC9 RID: 19401
	[SerializeField]
	private TreeFilterableSideScreenElement elementPrefab;

	// Token: 0x04004BCA RID: 19402
	[SerializeField]
	private GameObject titlebar;

	// Token: 0x04004BCB RID: 19403
	[SerializeField]
	private GameObject contentMask;

	// Token: 0x04004BCC RID: 19404
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x04004BCD RID: 19405
	[SerializeField]
	private KButton clearButton;

	// Token: 0x04004BCE RID: 19406
	[SerializeField]
	private GameObject configurationRowsContainer;

	// Token: 0x04004BCF RID: 19407
	private GameObject target;

	// Token: 0x04004BD0 RID: 19408
	private bool visualDirty;

	// Token: 0x04004BD1 RID: 19409
	private bool initialized;

	// Token: 0x04004BD2 RID: 19410
	private KImage onlyAllowTransportItemsImg;

	// Token: 0x04004BD3 RID: 19411
	public UIPool<TreeFilterableSideScreenElement> elementPool;

	// Token: 0x04004BD4 RID: 19412
	private UIPool<TreeFilterableSideScreenRow> rowPool;

	// Token: 0x04004BD5 RID: 19413
	private TreeFilterable targetFilterable;

	// Token: 0x04004BD6 RID: 19414
	private Dictionary<Tag, TreeFilterableSideScreenRow> tagRowMap = new Dictionary<Tag, TreeFilterableSideScreenRow>();

	// Token: 0x04004BD7 RID: 19415
	private Dictionary<Tag, bool> rowExpandedStatusMemory = new Dictionary<Tag, bool>();

	// Token: 0x04004BD8 RID: 19416
	private Storage storage;

	// Token: 0x02001EC7 RID: 7879
	private struct TagOrderInfo
	{
		// Token: 0x04008B7C RID: 35708
		public Tag tag;

		// Token: 0x04008B7D RID: 35709
		public string strippedName;
	}
}
