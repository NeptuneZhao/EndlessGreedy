using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C79 RID: 3193
public class KleiInventoryScreen : KModalScreen
{
	// Token: 0x1700073D RID: 1853
	// (get) Token: 0x0600621F RID: 25119 RVA: 0x00249C67 File Offset: 0x00247E67
	// (set) Token: 0x06006220 RID: 25120 RVA: 0x00249C6F File Offset: 0x00247E6F
	private PermitResource SelectedPermit { get; set; }

	// Token: 0x1700073E RID: 1854
	// (get) Token: 0x06006221 RID: 25121 RVA: 0x00249C78 File Offset: 0x00247E78
	// (set) Token: 0x06006222 RID: 25122 RVA: 0x00249C80 File Offset: 0x00247E80
	private string SelectedCategoryId { get; set; }

	// Token: 0x06006223 RID: 25123 RVA: 0x00249C8C File Offset: 0x00247E8C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		base.ConsumeMouseScroll = true;
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = new List<GridLayoutGroup>()
		};
		this.galleryGridLayouter.overrideParentForSizeReference = this.galleryGridContent;
		InventoryOrganization.Initialize();
	}

	// Token: 0x06006224 RID: 25124 RVA: 0x00249CFF File Offset: 0x00247EFF
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006225 RID: 25125 RVA: 0x00249D21 File Offset: 0x00247F21
	public override float GetSortKey()
	{
		return 20f;
	}

	// Token: 0x06006226 RID: 25126 RVA: 0x00249D28 File Offset: 0x00247F28
	protected override void OnActivate()
	{
		this.OnShow(true);
	}

	// Token: 0x06006227 RID: 25127 RVA: 0x00249D31 File Offset: 0x00247F31
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.InitConfig();
			this.ToggleDoublesOnly(0);
			this.ClearSearch();
		}
	}

	// Token: 0x06006228 RID: 25128 RVA: 0x00249D50 File Offset: 0x00247F50
	private void ToggleDoublesOnly(int newState)
	{
		this.showFilterState = newState;
		this.doublesOnlyToggle.ChangeState(this.showFilterState);
		this.doublesOnlyToggle.GetComponentInChildren<LocText>().text = this.showFilterState.ToString() + "+";
		string simpleTooltip = "";
		switch (this.showFilterState)
		{
		case 0:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_ALL_ITEMS;
			break;
		case 1:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_OWNED_ONLY;
			break;
		case 2:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_DOUBLES_ONLY;
			break;
		}
		ToolTip component = this.doublesOnlyToggle.GetComponent<ToolTip>();
		component.SetSimpleTooltip(simpleTooltip);
		component.refreshWhileHovering = true;
		component.forceRefresh = true;
		this.RefreshGallery();
	}

	// Token: 0x06006229 RID: 25129 RVA: 0x00249E08 File Offset: 0x00248008
	private void InitConfig()
	{
		if (this.initConfigComplete)
		{
			return;
		}
		this.initConfigComplete = true;
		this.galleryGridLayouter.RequestGridResize();
		this.categoryListContent.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		this.PopulateCategories();
		this.PopulateGallery();
		this.SelectCategory("BUILDINGS");
		this.searchField.onValueChanged.RemoveAllListeners();
		this.searchField.onValueChanged.AddListener(delegate(string value)
		{
			this.RefreshGallery();
		});
		this.clearSearchButton.ClearOnClick();
		this.clearSearchButton.onClick += this.ClearSearch;
		MultiToggle multiToggle = this.doublesOnlyToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			int newState = (this.showFilterState + 1) % 3;
			this.ToggleDoublesOnly(newState);
		}));
	}

	// Token: 0x0600622A RID: 25130 RVA: 0x00249EDB File Offset: 0x002480DB
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.ToggleDoublesOnly(0);
		this.ClearSearch();
		if (!this.initConfigComplete)
		{
			this.InitConfig();
		}
		this.RefreshUI();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshUI();
		});
	}

	// Token: 0x0600622B RID: 25131 RVA: 0x00249F1B File Offset: 0x0024811B
	private void ClearSearch()
	{
		this.searchField.text = "";
		this.searchField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.KLEI_INVENTORY_SCREEN.SEARCH_PLACEHOLDER;
		this.RefreshGallery();
	}

	// Token: 0x0600622C RID: 25132 RVA: 0x00249F52 File Offset: 0x00248152
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x0600622D RID: 25133 RVA: 0x00249F60 File Offset: 0x00248160
	private void RefreshUI()
	{
		this.IS_ONLINE = ThreadedHttps<KleiAccount>.Instance.HasValidTicket();
		this.RefreshCategories();
		this.RefreshGallery();
		if (this.SelectedCategoryId.IsNullOrWhiteSpace())
		{
			this.SelectCategory("BUILDINGS");
		}
		this.RefreshDetails();
		this.RefreshBarterPanel();
	}

	// Token: 0x0600622E RID: 25134 RVA: 0x00249FAD File Offset: 0x002481AD
	private GameObject GetAvailableGridButton()
	{
		if (this.recycledGalleryGridButtons.Count == 0)
		{
			return Util.KInstantiateUI(this.gridItemPrefab, this.galleryGridContent.gameObject, true);
		}
		GameObject result = this.recycledGalleryGridButtons[0];
		this.recycledGalleryGridButtons.RemoveAt(0);
		return result;
	}

	// Token: 0x0600622F RID: 25135 RVA: 0x00249FEC File Offset: 0x002481EC
	private void RecycleGalleryGridButton(GameObject button)
	{
		button.GetComponent<MultiToggle>().onClick = null;
		this.recycledGalleryGridButtons.Add(button);
	}

	// Token: 0x06006230 RID: 25136 RVA: 0x0024A008 File Offset: 0x00248208
	public void PopulateCategories()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.categoryToggles)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
		}
		this.categoryToggles.Clear();
		foreach (KeyValuePair<string, List<string>> keyValuePair2 in InventoryOrganization.categoryIdToSubcategoryIdsMap)
		{
			string categoryId2;
			List<string> list;
			keyValuePair2.Deconstruct(out categoryId2, out list);
			string categoryId = categoryId2;
			GameObject gameObject = Util.KInstantiateUI(this.categoryRowPrefab, this.categoryListContent.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(InventoryOrganization.GetCategoryName(categoryId));
			component.GetReference<Image>("Icon").sprite = InventoryOrganization.categoryIdToIconMap[categoryId];
			MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
			MultiToggle multiToggle = component2;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnMouseOverToggle));
			component2.onClick = delegate()
			{
				this.SelectCategory(categoryId);
			};
			this.categoryToggles.Add(categoryId, component2);
			this.SetCatogoryClickUISound(categoryId, component2);
		}
	}

	// Token: 0x06006231 RID: 25137 RVA: 0x0024A188 File Offset: 0x00248388
	public void PopulateGallery()
	{
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			this.RecycleGalleryGridButton(keyValuePair.Value.gameObject);
		}
		this.galleryGridButtons.Clear();
		this.galleryGridLayouter.ImmediateSizeGridToScreenResolution();
		foreach (PermitResource permitResource in Db.Get().Permits.resources)
		{
			if (!permitResource.Id.StartsWith("visonly_"))
			{
				this.AddItemToGallery(permitResource);
			}
		}
		this.subcategories.Sort((KleiInventoryUISubcategory a, KleiInventoryUISubcategory b) => InventoryOrganization.subcategoryIdToPresentationDataMap[a.subcategoryID].sortKey.CompareTo(InventoryOrganization.subcategoryIdToPresentationDataMap[b.subcategoryID].sortKey));
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			kleiInventoryUISubcategory.gameObject.transform.SetAsLastSibling();
		}
		this.CollectSubcategoryGridLayouts();
		this.CloseSubcategory("UNCATEGORIZED");
	}

	// Token: 0x06006232 RID: 25138 RVA: 0x0024A2E0 File Offset: 0x002484E0
	private void CloseSubcategory(string subcategoryID)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = this.subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryID);
		if (kleiInventoryUISubcategory != null)
		{
			kleiInventoryUISubcategory.ToggleOpen(false);
		}
	}

	// Token: 0x06006233 RID: 25139 RVA: 0x0024A324 File Offset: 0x00248524
	private void AddItemToSubcategoryUIContainer(GameObject itemButton, string subcategoryId)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = this.subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryId);
		if (kleiInventoryUISubcategory == null)
		{
			kleiInventoryUISubcategory = Util.KInstantiateUI(this.subcategoryPrefab, this.galleryGridContent.gameObject, true).GetComponent<KleiInventoryUISubcategory>();
			kleiInventoryUISubcategory.subcategoryID = subcategoryId;
			this.subcategories.Add(kleiInventoryUISubcategory);
			kleiInventoryUISubcategory.SetIdentity(InventoryOrganization.GetSubcategoryName(subcategoryId), InventoryOrganization.subcategoryIdToPresentationDataMap[subcategoryId].icon);
		}
		itemButton.transform.SetParent(kleiInventoryUISubcategory.gridLayout.transform);
	}

	// Token: 0x06006234 RID: 25140 RVA: 0x0024A3D0 File Offset: 0x002485D0
	private void CollectSubcategoryGridLayouts()
	{
		this.galleryGridLayouter.OnSizeGridComplete = null;
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			this.galleryGridLayouter.targetGridLayouts.Add(kleiInventoryUISubcategory.gridLayout);
			GridLayouter gridLayouter = this.galleryGridLayouter;
			gridLayouter.OnSizeGridComplete = (System.Action)Delegate.Combine(gridLayouter.OnSizeGridComplete, new System.Action(kleiInventoryUISubcategory.RefreshDisplay));
		}
		this.galleryGridLayouter.RequestGridResize();
	}

	// Token: 0x06006235 RID: 25141 RVA: 0x0024A470 File Offset: 0x00248670
	private void AddItemToGallery(PermitResource permit)
	{
		if (this.galleryGridButtons.ContainsKey(permit))
		{
			return;
		}
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		GameObject availableGridButton = this.GetAvailableGridButton();
		this.AddItemToSubcategoryUIContainer(availableGridButton, InventoryOrganization.GetPermitSubcategory(permit));
		HierarchyReferences component = availableGridButton.GetComponent<HierarchyReferences>();
		Image reference = component.GetReference<Image>("Icon");
		LocText reference2 = component.GetReference<LocText>("OwnedCountLabel");
		Image reference3 = component.GetReference<Image>("IsUnownedOverlay");
		Image reference4 = component.GetReference<Image>("DlcBanner");
		MultiToggle component2 = availableGridButton.GetComponent<MultiToggle>();
		reference.sprite = permitPresentationInfo.sprite;
		if (permit.IsOwnableOnServer())
		{
			int ownedCount = PermitItems.GetOwnedCount(permit);
			reference2.text = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
			reference2.gameObject.SetActive(ownedCount > 0);
			reference3.gameObject.SetActive(ownedCount <= 0);
		}
		else
		{
			reference2.gameObject.SetActive(false);
			reference3.gameObject.SetActive(false);
		}
		string dlcIdFrom = permit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			reference4.gameObject.SetActive(true);
			reference4.color = DlcManager.GetDlcBannerColor(dlcIdFrom);
		}
		else
		{
			reference4.gameObject.SetActive(false);
		}
		MultiToggle multiToggle = component2;
		multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnMouseOverToggle));
		component2.onClick = delegate()
		{
			this.SelectItem(permit);
		};
		this.galleryGridButtons.Add(permit, component2);
		this.SetItemClickUISound(permit, component2);
		KleiItemsUI.ConfigureTooltipOn(availableGridButton, KleiItemsUI.GetTooltipStringFor(permit));
	}

	// Token: 0x06006236 RID: 25142 RVA: 0x0024A63B File Offset: 0x0024883B
	public void SelectCategory(string categoryId)
	{
		if (InventoryOrganization.categoryIdToIsEmptyMap[categoryId])
		{
			return;
		}
		this.SelectedCategoryId = categoryId;
		this.galleryHeaderLabel.SetText(InventoryOrganization.GetCategoryName(categoryId));
		this.RefreshCategories();
		this.SelectDefaultCategoryItem();
	}

	// Token: 0x06006237 RID: 25143 RVA: 0x0024A670 File Offset: 0x00248870
	private void SelectDefaultCategoryItem()
	{
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			if (InventoryOrganization.categoryIdToSubcategoryIdsMap[this.SelectedCategoryId].Contains(InventoryOrganization.GetPermitSubcategory(keyValuePair.Key)))
			{
				this.SelectItem(keyValuePair.Key);
				return;
			}
		}
		this.SelectItem(null);
	}

	// Token: 0x06006238 RID: 25144 RVA: 0x0024A6F8 File Offset: 0x002488F8
	public void SelectItem(PermitResource permit)
	{
		this.SelectedPermit = permit;
		this.RefreshGallery();
		this.RefreshDetails();
		this.RefreshBarterPanel();
	}

	// Token: 0x06006239 RID: 25145 RVA: 0x0024A714 File Offset: 0x00248914
	private void RefreshGallery()
	{
		string value = this.searchField.text.ToUpper();
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			PermitResource permitResource;
			MultiToggle multiToggle;
			keyValuePair.Deconstruct(out permitResource, out multiToggle);
			PermitResource permitResource2 = permitResource;
			MultiToggle multiToggle2 = multiToggle;
			string permitSubcategory = InventoryOrganization.GetPermitSubcategory(permitResource2);
			bool flag = permitSubcategory == "UNCATEGORIZED" || InventoryOrganization.categoryIdToSubcategoryIdsMap[this.SelectedCategoryId].Contains(permitSubcategory);
			flag = (flag && (permitResource2.Name.ToUpper().Contains(value) || permitResource2.Id.ToUpper().Contains(value) || permitResource2.Description.ToUpper().Contains(value)));
			multiToggle2.ChangeState((permitResource2 == this.SelectedPermit) ? 1 : 0);
			HierarchyReferences component = multiToggle2.gameObject.GetComponent<HierarchyReferences>();
			LocText reference = component.GetReference<LocText>("OwnedCountLabel");
			Image reference2 = component.GetReference<Image>("IsUnownedOverlay");
			if (permitResource2.IsOwnableOnServer())
			{
				int ownedCount = PermitItems.GetOwnedCount(permitResource2);
				reference.text = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
				reference.gameObject.SetActive(ownedCount > 0);
				reference2.gameObject.SetActive(ownedCount <= 0);
				if (this.showFilterState == 2 && ownedCount < 2)
				{
					flag = false;
				}
				else if (this.showFilterState == 1 && ownedCount == 0)
				{
					flag = false;
				}
			}
			else if (!permitResource2.IsUnlocked())
			{
				reference.gameObject.SetActive(false);
				reference2.gameObject.SetActive(true);
				if (this.showFilterState != 0)
				{
					flag = false;
				}
			}
			else
			{
				reference.gameObject.SetActive(false);
				reference2.gameObject.SetActive(false);
				if (this.showFilterState == 2)
				{
					flag = false;
				}
			}
			if (multiToggle2.gameObject.activeSelf != flag)
			{
				multiToggle2.gameObject.SetActive(flag);
			}
		}
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			kleiInventoryUISubcategory.RefreshDisplay();
		}
	}

	// Token: 0x0600623A RID: 25146 RVA: 0x0024A978 File Offset: 0x00248B78
	private void RefreshCategories()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.categoryToggles)
		{
			keyValuePair.Value.ChangeState((keyValuePair.Key == this.SelectedCategoryId) ? 1 : 0);
			if (InventoryOrganization.categoryIdToIsEmptyMap[keyValuePair.Key])
			{
				keyValuePair.Value.ChangeState(2);
			}
			else
			{
				keyValuePair.Value.ChangeState((keyValuePair.Key == this.SelectedCategoryId) ? 1 : 0);
			}
		}
	}

	// Token: 0x0600623B RID: 25147 RVA: 0x0024AA30 File Offset: 0x00248C30
	private void RefreshDetails()
	{
		PermitResource selectedPermit = this.SelectedPermit;
		PermitPresentationInfo permitPresentationInfo = selectedPermit.GetPermitPresentationInfo();
		this.permitVis.ConfigureWith(selectedPermit);
		this.selectionDetailsScrollRect.rectTransform().anchorMin = new Vector2(0f, 0f);
		this.selectionDetailsScrollRect.rectTransform().anchorMax = new Vector2(1f, 1f);
		this.selectionDetailsScrollRect.rectTransform().sizeDelta = new Vector2(-24f, 0f);
		this.selectionDetailsScrollRect.rectTransform().anchoredPosition = Vector2.zero;
		this.selectionDetailsScrollRect.content.rectTransform().sizeDelta = new Vector2(0f, this.selectionDetailsScrollRect.content.rectTransform().sizeDelta.y);
		this.selectionDetailsScrollRectScrollBarContainer.anchorMin = new Vector2(1f, 0f);
		this.selectionDetailsScrollRectScrollBarContainer.anchorMax = new Vector2(1f, 1f);
		this.selectionDetailsScrollRectScrollBarContainer.sizeDelta = new Vector2(24f, 0f);
		this.selectionDetailsScrollRectScrollBarContainer.anchoredPosition = Vector2.zero;
		this.selectionHeaderLabel.SetText(selectedPermit.Name);
		this.selectionNameLabel.SetText(selectedPermit.Name);
		this.selectionDescriptionLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(selectedPermit.Description));
		this.selectionDescriptionLabel.SetText(selectedPermit.Description);
		this.selectionFacadeForLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(permitPresentationInfo.facadeFor));
		this.selectionFacadeForLabel.SetText(permitPresentationInfo.facadeFor);
		string dlcIdFrom = selectedPermit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			this.selectionRarityDetailsLabel.gameObject.SetActive(false);
			this.selectionOwnedCount.gameObject.SetActive(false);
			this.selectionCollectionLabel.gameObject.SetActive(true);
			if (selectedPermit.Rarity == PermitRarity.UniversalLocked)
			{
				this.selectionCollectionLabel.SetText(UI.KLEI_INVENTORY_SCREEN.COLLECTION_COMING_SOON.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
				return;
			}
			this.selectionCollectionLabel.SetText(UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
			return;
		}
		else
		{
			this.selectionCollectionLabel.gameObject.SetActive(false);
			string text = UI.KLEI_INVENTORY_SCREEN.ITEM_RARITY_DETAILS.Replace("{RarityName}", selectedPermit.Rarity.GetLocStringName());
			this.selectionRarityDetailsLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(text));
			this.selectionRarityDetailsLabel.SetText(text);
			this.selectionOwnedCount.gameObject.SetActive(true);
			if (!selectedPermit.IsOwnableOnServer())
			{
				this.selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE);
				return;
			}
			int ownedCount = PermitItems.GetOwnedCount(selectedPermit);
			if (ownedCount > 0)
			{
				this.selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT.Replace("{OwnedCount}", ownedCount.ToString()));
				return;
			}
			this.selectionOwnedCount.SetText(KleiItemsUI.WrapWithColor(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));
			return;
		}
	}

	// Token: 0x0600623C RID: 25148 RVA: 0x0024AD40 File Offset: 0x00248F40
	private KleiInventoryScreen.PermitPrintabilityState GetPermitPrintabilityState(PermitResource permit)
	{
		if (!this.IS_ONLINE)
		{
			return KleiInventoryScreen.PermitPrintabilityState.UserOffline;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(this.SelectedPermit.Id, out num, out num2);
		if (num == 0UL)
		{
			if (permit.Rarity == PermitRarity.Universal || permit.Rarity == PermitRarity.UniversalLocked || permit.Rarity == PermitRarity.Loyalty || permit.Rarity == PermitRarity.Unknown)
			{
				return KleiInventoryScreen.PermitPrintabilityState.NotForSale;
			}
			return KleiInventoryScreen.PermitPrintabilityState.NotForSaleYet;
		}
		else
		{
			if (PermitItems.GetOwnedCount(permit) > 0)
			{
				return KleiInventoryScreen.PermitPrintabilityState.AlreadyOwned;
			}
			if (KleiItems.GetFilamentAmount() < num)
			{
				return KleiInventoryScreen.PermitPrintabilityState.TooExpensive;
			}
			return KleiInventoryScreen.PermitPrintabilityState.Printable;
		}
	}

	// Token: 0x0600623D RID: 25149 RVA: 0x0024ADB4 File Offset: 0x00248FB4
	private void RefreshBarterPanel()
	{
		this.barterBuyButton.ClearOnClick();
		this.barterSellButton.ClearOnClick();
		this.barterBuyButton.isInteractable = this.IS_ONLINE;
		this.barterSellButton.isInteractable = this.IS_ONLINE;
		HierarchyReferences component = this.barterBuyButton.GetComponent<HierarchyReferences>();
		HierarchyReferences component2 = this.barterSellButton.GetComponent<HierarchyReferences>();
		new Color(1f, 0.69411767f, 0.69411767f);
		Color color = new Color(0.6f, 0.9529412f, 0.5019608f);
		LocText reference = component.GetReference<LocText>("CostLabel");
		LocText reference2 = component2.GetReference<LocText>("CostLabel");
		this.barterPanelBG.color = (this.IS_ONLINE ? Util.ColorFromHex("575D6F") : Util.ColorFromHex("6F6F6F"));
		this.filamentWalletSection.gameObject.SetActive(this.IS_ONLINE);
		this.barterOfflineLabel.gameObject.SetActive(!this.IS_ONLINE);
		ulong filamentAmount = KleiItems.GetFilamentAmount();
		this.filamentWalletSection.GetComponent<ToolTip>().SetSimpleTooltip((filamentAmount > 1UL) ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_PLURAL_TOOLTIP, filamentAmount) : string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_TOOLTIP, filamentAmount));
		KleiInventoryScreen.PermitPrintabilityState permitPrintabilityState = this.GetPermitPrintabilityState(this.SelectedPermit);
		if (!this.IS_ONLINE)
		{
			component.GetReference<LocText>("CostLabel").SetText("");
			reference2.SetText("");
			reference2.color = Color.white;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
			this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
			return;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(this.SelectedPermit.Id, out num, out num2);
		this.filamentWalletSection.GetComponentInChildren<LocText>().SetText(KleiItems.GetFilamentAmount().ToString());
		switch (permitPrintabilityState)
		{
		case KleiInventoryScreen.PermitPrintabilityState.Printable:
			this.barterBuyButton.isInteractable = true;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_ACTIVE, num.ToString()));
			reference.SetText("-" + num.ToString());
			this.barterBuyButton.onClick += delegate()
			{
				GameObject gameObject = Util.KInstantiateUI(this.barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject, false);
				gameObject.rectTransform().sizeDelta = Vector2.zero;
				gameObject.GetComponent<BarterConfirmationScreen>().Present(this.SelectedPermit, true);
			};
			break;
		case KleiInventoryScreen.PermitPrintabilityState.AlreadyOwned:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_ALREADY_OWNED);
			reference.SetText("-" + num.ToString());
			break;
		case KleiInventoryScreen.PermitPrintabilityState.TooExpensive:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_CANT_AFFORD.text);
			reference.SetText("-" + num.ToString());
			break;
		case KleiInventoryScreen.PermitPrintabilityState.NotForSale:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE);
			reference.SetText("");
			break;
		case KleiInventoryScreen.PermitPrintabilityState.NotForSaleYet:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_BETA);
			reference.SetText("");
			break;
		}
		if (num2 == 0UL)
		{
			this.barterSellButton.isInteractable = false;
			this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNSELLABLE);
			reference2.SetText("");
			reference2.color = Color.white;
			return;
		}
		bool flag = PermitItems.GetOwnedCount(this.SelectedPermit) > 0;
		this.barterSellButton.isInteractable = flag;
		this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_SELL_ACTIVE, num2.ToString()) : UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_NONE_TO_SELL.text);
		if (flag)
		{
			reference2.color = color;
			reference2.SetText("+" + num2.ToString());
		}
		else
		{
			reference2.color = Color.white;
			reference2.SetText("+" + num2.ToString());
		}
		this.barterSellButton.onClick += delegate()
		{
			GameObject gameObject = Util.KInstantiateUI(this.barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject, false);
			gameObject.rectTransform().sizeDelta = Vector2.zero;
			gameObject.GetComponent<BarterConfirmationScreen>().Present(this.SelectedPermit, false);
		};
	}

	// Token: 0x0600623E RID: 25150 RVA: 0x0024B20C File Offset: 0x0024940C
	private void SetCatogoryClickUISound(string categoryID, MultiToggle toggle)
	{
		if (!this.categoryToggles.ContainsKey(categoryID))
		{
			toggle.states[1].on_click_override_sound_path = "";
			toggle.states[0].on_click_override_sound_path = "";
			return;
		}
		toggle.states[1].on_click_override_sound_path = "General_Category_Click";
		toggle.states[0].on_click_override_sound_path = "General_Category_Click";
	}

	// Token: 0x0600623F RID: 25151 RVA: 0x0024B280 File Offset: 0x00249480
	private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
	{
		string facadeItemSoundName = KleiInventoryScreen.GetFacadeItemSoundName(permit);
		toggle.states[1].on_click_override_sound_path = facadeItemSoundName + "_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = facadeItemSoundName + "_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x06006240 RID: 25152 RVA: 0x0024B368 File Offset: 0x00249568
	public static string GetFacadeItemSoundName(PermitResource permit)
	{
		if (permit == null)
		{
			return "HUD";
		}
		switch (permit.Category)
		{
		case PermitCategory.DupeTops:
			return "tops";
		case PermitCategory.DupeBottoms:
			return "bottoms";
		case PermitCategory.DupeGloves:
			return "gloves";
		case PermitCategory.DupeShoes:
			return "shoes";
		case PermitCategory.DupeHats:
			return "hats";
		case PermitCategory.AtmoSuitHelmet:
			return "atmosuit_helmet";
		case PermitCategory.AtmoSuitBody:
			return "tops";
		case PermitCategory.AtmoSuitGloves:
			return "gloves";
		case PermitCategory.AtmoSuitBelt:
			return "belt";
		case PermitCategory.AtmoSuitShoes:
			return "shoes";
		}
		if (permit.Category == PermitCategory.Building)
		{
			BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef == null)
			{
				return "HUD";
			}
			string prefabID = buildingDef.PrefabID;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(prefabID);
			if (num <= 2076384603U)
			{
				if (num <= 1633134164U)
				{
					if (num <= 595816591U)
					{
						if (num != 228062815U)
						{
							if (num != 595816591U)
							{
								goto IL_38D;
							}
							if (!(prefabID == "FlowerVase"))
							{
								goto IL_38D;
							}
						}
						else
						{
							if (!(prefabID == "LuxuryBed"))
							{
								goto IL_38D;
							}
							string id = permit.Id;
							if (id == "LuxuryBed_boat")
							{
								return "elegantbed_boat";
							}
							if (!(id == "LuxuryBed_bouncy"))
							{
								return "elegantbed";
							}
							return "elegantbed_bouncy";
						}
					}
					else if (num != 1607642960U)
					{
						if (num != 1633134164U)
						{
							goto IL_38D;
						}
						if (!(prefabID == "CeilingLight"))
						{
							goto IL_38D;
						}
						return "ceilingLight";
					}
					else
					{
						if (!(prefabID == "FlushToilet"))
						{
							goto IL_38D;
						}
						return "flushtoilate";
					}
				}
				else if (num <= 1943253450U)
				{
					if (num != 1734850496U)
					{
						if (num != 1943253450U)
						{
							goto IL_38D;
						}
						if (!(prefabID == "WaterCooler"))
						{
							goto IL_38D;
						}
						return "watercooler";
					}
					else
					{
						if (!(prefabID == "RockCrusher"))
						{
							goto IL_38D;
						}
						return "rockrefinery";
					}
				}
				else if (num != 2028863301U)
				{
					if (num != 2076384603U)
					{
						goto IL_38D;
					}
					if (!(prefabID == "GasReservoir"))
					{
						goto IL_38D;
					}
					return "gasstorage";
				}
				else if (!(prefabID == "FlowerVaseHanging"))
				{
					goto IL_38D;
				}
			}
			else if (num <= 3048425356U)
			{
				if (num <= 2722382738U)
				{
					if (num != 2402859370U)
					{
						if (num != 2722382738U)
						{
							goto IL_38D;
						}
						if (!(prefabID == "PlanterBox"))
						{
							goto IL_38D;
						}
						return "planterbox";
					}
					else
					{
						if (!(prefabID == "StorageLocker"))
						{
							goto IL_38D;
						}
						return "storagelocker";
					}
				}
				else if (num != 2899744071U)
				{
					if (num != 3048425356U)
					{
						goto IL_38D;
					}
					if (!(prefabID == "Bed"))
					{
						goto IL_38D;
					}
					return "bed";
				}
				else
				{
					if (!(prefabID == "ExteriorWall"))
					{
						goto IL_38D;
					}
					return "wall";
				}
			}
			else if (num <= 3534553076U)
			{
				if (num != 3132083755U)
				{
					if (num != 3534553076U)
					{
						goto IL_38D;
					}
					if (!(prefabID == "MassageTable"))
					{
						goto IL_38D;
					}
					return "massagetable";
				}
				else if (!(prefabID == "FlowerVaseWall"))
				{
					goto IL_38D;
				}
			}
			else if (num != 3903452895U)
			{
				if (num != 3958671086U)
				{
					goto IL_38D;
				}
				if (!(prefabID == "FlowerVaseHangingFancy"))
				{
					goto IL_38D;
				}
			}
			else
			{
				if (!(prefabID == "EggCracker"))
				{
					goto IL_38D;
				}
				return "eggcracker";
			}
			return "flowervase";
		}
		IL_38D:
		if (permit.Category == PermitCategory.Artwork)
		{
			BuildingDef buildingDef2 = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef2 == null)
			{
				return "HUD";
			}
			ArtableStage artableStage = (ArtableStage)permit;
			if (KleiInventoryScreen.<GetFacadeItemSoundName>g__Has|76_0<Sculpture>(buildingDef2))
			{
				string prefabID = buildingDef2.PrefabID;
				if (prefabID == "IceSculpture")
				{
					return "icesculpture";
				}
				if (!(prefabID == "WoodSculpture"))
				{
					return "sculpture";
				}
				return "woodsculpture";
			}
			else if (KleiInventoryScreen.<GetFacadeItemSoundName>g__Has|76_0<Painting>(buildingDef2))
			{
				return "painting";
			}
		}
		if (permit.Category == PermitCategory.JoyResponse && permit is BalloonArtistFacadeResource)
		{
			return "balloon";
		}
		return "HUD";
	}

	// Token: 0x06006241 RID: 25153 RVA: 0x0024B796 File Offset: 0x00249996
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06006249 RID: 25161 RVA: 0x0024B889 File Offset: 0x00249A89
	[CompilerGenerated]
	internal static bool <GetFacadeItemSoundName>g__Has|76_0<T>(BuildingDef buildingDef) where T : Component
	{
		return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
	}

	// Token: 0x04004284 RID: 17028
	[Header("Header")]
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004285 RID: 17029
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x04004286 RID: 17030
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x04004287 RID: 17031
	private Dictionary<string, MultiToggle> categoryToggles = new Dictionary<string, MultiToggle>();

	// Token: 0x04004288 RID: 17032
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x04004289 RID: 17033
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x0400428A RID: 17034
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x0400428B RID: 17035
	[SerializeField]
	private GameObject subcategoryPrefab;

	// Token: 0x0400428C RID: 17036
	[SerializeField]
	private GameObject itemDummyPrefab;

	// Token: 0x0400428D RID: 17037
	[Header("GalleryFilters")]
	[SerializeField]
	private KInputTextField searchField;

	// Token: 0x0400428E RID: 17038
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x0400428F RID: 17039
	[SerializeField]
	private MultiToggle doublesOnlyToggle;

	// Token: 0x04004290 RID: 17040
	public const int FILTER_SHOW_ALL = 0;

	// Token: 0x04004291 RID: 17041
	public const int FILTER_SHOW_OWNED_ONLY = 1;

	// Token: 0x04004292 RID: 17042
	public const int FILTER_SHOW_DOUBLES_ONLY = 2;

	// Token: 0x04004293 RID: 17043
	private int showFilterState;

	// Token: 0x04004294 RID: 17044
	[Header("BarterSection")]
	[SerializeField]
	private Image barterPanelBG;

	// Token: 0x04004295 RID: 17045
	[SerializeField]
	private KButton barterBuyButton;

	// Token: 0x04004296 RID: 17046
	[SerializeField]
	private KButton barterSellButton;

	// Token: 0x04004297 RID: 17047
	[SerializeField]
	private GameObject barterConfirmationScreenPrefab;

	// Token: 0x04004298 RID: 17048
	[SerializeField]
	private GameObject filamentWalletSection;

	// Token: 0x04004299 RID: 17049
	[SerializeField]
	private GameObject barterOfflineLabel;

	// Token: 0x0400429A RID: 17050
	private Dictionary<PermitResource, MultiToggle> galleryGridButtons = new Dictionary<PermitResource, MultiToggle>();

	// Token: 0x0400429B RID: 17051
	private List<KleiInventoryUISubcategory> subcategories = new List<KleiInventoryUISubcategory>();

	// Token: 0x0400429C RID: 17052
	private List<GameObject> recycledGalleryGridButtons = new List<GameObject>();

	// Token: 0x0400429D RID: 17053
	private GridLayouter galleryGridLayouter;

	// Token: 0x0400429E RID: 17054
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x0400429F RID: 17055
	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	// Token: 0x040042A0 RID: 17056
	[SerializeField]
	private KScrollRect selectionDetailsScrollRect;

	// Token: 0x040042A1 RID: 17057
	[SerializeField]
	private RectTransform selectionDetailsScrollRectScrollBarContainer;

	// Token: 0x040042A2 RID: 17058
	[SerializeField]
	private LocText selectionNameLabel;

	// Token: 0x040042A3 RID: 17059
	[SerializeField]
	private LocText selectionDescriptionLabel;

	// Token: 0x040042A4 RID: 17060
	[SerializeField]
	private LocText selectionFacadeForLabel;

	// Token: 0x040042A5 RID: 17061
	[SerializeField]
	private LocText selectionCollectionLabel;

	// Token: 0x040042A6 RID: 17062
	[SerializeField]
	private LocText selectionRarityDetailsLabel;

	// Token: 0x040042A7 RID: 17063
	[SerializeField]
	private LocText selectionOwnedCount;

	// Token: 0x040042A9 RID: 17065
	private bool IS_ONLINE;

	// Token: 0x040042AA RID: 17066
	private bool initConfigComplete;

	// Token: 0x02001D60 RID: 7520
	private enum PermitPrintabilityState
	{
		// Token: 0x04008724 RID: 34596
		Printable,
		// Token: 0x04008725 RID: 34597
		AlreadyOwned,
		// Token: 0x04008726 RID: 34598
		TooExpensive,
		// Token: 0x04008727 RID: 34599
		NotForSale,
		// Token: 0x04008728 RID: 34600
		NotForSaleYet,
		// Token: 0x04008729 RID: 34601
		UserOffline
	}

	// Token: 0x02001D61 RID: 7521
	private enum MultiToggleState
	{
		// Token: 0x0400872B RID: 34603
		Default,
		// Token: 0x0400872C RID: 34604
		Selected,
		// Token: 0x0400872D RID: 34605
		NonInteractable
	}
}
