using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000D04 RID: 3332
public class OutfitDesignerScreen : KMonoBehaviour
{
	// Token: 0x17000760 RID: 1888
	// (get) Token: 0x0600676F RID: 26479 RVA: 0x00269E89 File Offset: 0x00268089
	// (set) Token: 0x06006770 RID: 26480 RVA: 0x00269E91 File Offset: 0x00268091
	public OutfitDesignerScreenConfig Config { get; private set; }

	// Token: 0x17000761 RID: 1889
	// (get) Token: 0x06006771 RID: 26481 RVA: 0x00269E9A File Offset: 0x0026809A
	// (set) Token: 0x06006772 RID: 26482 RVA: 0x00269EA2 File Offset: 0x002680A2
	public PermitResource SelectedPermit { get; private set; }

	// Token: 0x17000762 RID: 1890
	// (get) Token: 0x06006773 RID: 26483 RVA: 0x00269EAB File Offset: 0x002680AB
	// (set) Token: 0x06006774 RID: 26484 RVA: 0x00269EB3 File Offset: 0x002680B3
	public PermitCategory SelectedCategory { get; private set; }

	// Token: 0x17000763 RID: 1891
	// (get) Token: 0x06006775 RID: 26485 RVA: 0x00269EBC File Offset: 0x002680BC
	// (set) Token: 0x06006776 RID: 26486 RVA: 0x00269EC4 File Offset: 0x002680C4
	public OutfitDesignerScreen_OutfitState outfitState { get; private set; }

	// Token: 0x06006777 RID: 26487 RVA: 0x00269ED0 File Offset: 0x002680D0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(this.categoryRowPrefab.transform.parent == this.categoryListContent.transform);
		global::Debug.Assert(this.gridItemPrefab.transform.parent == this.galleryGridContent.transform);
		global::Debug.Assert(this.subcategoryUiPrefab.transform.parent == this.galleryGridContent.transform);
		this.categoryRowPrefab.SetActive(false);
		this.gridItemPrefab.SetActive(false);
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.galleryGridLayouter.overrideParentForSizeReference = this.galleryGridContent;
		this.categoryRowPool = new UIPrefabLocalPool(this.categoryRowPrefab, this.categoryListContent.gameObject);
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
		this.subcategoryUiPool = new UIPrefabLocalPool(this.subcategoryUiPrefab, this.galleryGridContent.gameObject);
		if (OutfitDesignerScreen.outfitTypeToCategoriesDict == null)
		{
			Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]> dictionary = new Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]>();
			dictionary[ClothingOutfitUtility.OutfitType.Clothing] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_CLOTHING;
			dictionary[ClothingOutfitUtility.OutfitType.AtmoSuit] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_ATMO_SUITS;
			OutfitDesignerScreen.outfitTypeToCategoriesDict = dictionary;
		}
		InventoryOrganization.Initialize();
	}

	// Token: 0x06006778 RID: 26488 RVA: 0x0026A034 File Offset: 0x00268234
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x06006779 RID: 26489 RVA: 0x0026A041 File Offset: 0x00268241
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		this.minionOrMannequin.TrySpawn();
		if (!this.Config.isValid)
		{
			throw new NotSupportedException("Cannot open OutfitDesignerScreen without a config. Make sure to call Configure() before enabling the screen");
		}
		this.Configure(this.Config);
	}

	// Token: 0x0600677A RID: 26490 RVA: 0x0026A07A File Offset: 0x0026827A
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshCategories();
			this.RefreshGallery();
			this.RefreshOutfitState();
		});
	}

	// Token: 0x0600677B RID: 26491 RVA: 0x0026A099 File Offset: 0x00268299
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.UnregisterPreventScreenPop();
	}

	// Token: 0x0600677C RID: 26492 RVA: 0x0026A0A7 File Offset: 0x002682A7
	private void UpdateSaveButtons()
	{
		if (this.updateSaveButtonsFn != null)
		{
			this.updateSaveButtonsFn();
		}
	}

	// Token: 0x0600677D RID: 26493 RVA: 0x0026A0BC File Offset: 0x002682BC
	public void Configure(OutfitDesignerScreenConfig config)
	{
		this.Config = config;
		if (config.targetMinionInstance.HasValue)
		{
			this.outfitState = OutfitDesignerScreen_OutfitState.ForMinionInstance(this.Config.sourceTarget, config.targetMinionInstance.Value);
		}
		else
		{
			this.outfitState = OutfitDesignerScreen_OutfitState.ForTemplateOutfit(this.Config.sourceTarget);
		}
		if (this.postponeConfiguration)
		{
			return;
		}
		this.RegisterPreventScreenPop();
		this.minionOrMannequin.SetFrom(config.minionPersonality).SpawnedAvatar.GetComponent<WearableAccessorizer>();
		using (ListPool<ClothingItemResource, OutfitDesignerScreen>.PooledList pooledList = PoolsFor<OutfitDesignerScreen>.AllocateList<ClothingItemResource>())
		{
			this.outfitState.AddItemValuesTo(pooledList);
			this.minionOrMannequin.SetFrom(config.minionPersonality).SetOutfit(config.sourceTarget.OutfitType, pooledList);
		}
		this.PopulateCategories();
		this.SelectCategory(OutfitDesignerScreen.outfitTypeToCategoriesDict[this.outfitState.outfitType][0]);
		this.galleryGridLayouter.RequestGridResize();
		this.RefreshOutfitState();
		OutfitDesignerScreenConfig config2 = this.Config;
		if (config2.targetMinionInstance.HasValue)
		{
			this.updateSaveButtonsFn = null;
			this.primaryButton.ClearOnClick();
			TMP_Text componentInChildren = this.primaryButton.GetComponentInChildren<LocText>();
			LocString button_APPLY_TO_MINION = UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.BUTTON_APPLY_TO_MINION;
			string search = "{MinionName}";
			config2 = this.Config;
			componentInChildren.SetText(button_APPLY_TO_MINION.Replace(search, config2.targetMinionInstance.Value.GetProperName()));
			this.primaryButton.onClick += delegate()
			{
				OutfitDesignerScreenConfig config3 = this.Config;
				ClothingOutfitUtility.OutfitType outfitType = config3.sourceTarget.OutfitType;
				config3 = this.Config;
				ClothingOutfitTarget obj = ClothingOutfitTarget.FromMinion(outfitType, config3.targetMinionInstance.Value);
				config3 = this.Config;
				obj.WriteItems(config3.sourceTarget.OutfitType, this.outfitState.GetItems());
				if (this.Config.onWriteToOutfitTargetFn != null)
				{
					this.Config.onWriteToOutfitTargetFn(obj);
				}
				LockerNavigator.Instance.PopScreen();
			};
			this.secondaryButton.ClearOnClick();
			this.secondaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.BUTTON_APPLY_TO_TEMPLATE);
			this.secondaryButton.onClick += delegate()
			{
				OutfitDesignerScreen.MakeApplyToTemplatePopup(this.inputFieldPrefab, this.outfitState, this.Config.targetMinionInstance.Value, this.Config.outfitTemplate, this.Config.onWriteToOutfitTargetFn);
			};
			this.updateSaveButtonsFn = (System.Action)Delegate.Combine(this.updateSaveButtonsFn, new System.Action(delegate()
			{
				if (this.outfitState.DoesContainLockedItems())
				{
					this.primaryButton.isInteractable = false;
					this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
					this.secondaryButton.isInteractable = false;
					this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
					return;
				}
				this.primaryButton.isInteractable = true;
				this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
				this.secondaryButton.isInteractable = true;
				this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
			}));
		}
		else
		{
			config2 = this.Config;
			if (!config2.outfitTemplate.HasValue)
			{
				throw new NotSupportedException();
			}
			this.updateSaveButtonsFn = null;
			this.primaryButton.ClearOnClick();
			this.primaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.BUTTON_SAVE);
			this.primaryButton.onClick += delegate()
			{
				this.outfitState.destinationTarget.WriteName(this.outfitState.name);
				this.outfitState.destinationTarget.WriteItems(this.outfitState.outfitType, this.outfitState.GetItems());
				OutfitDesignerScreenConfig config3 = this.Config;
				if (config3.minionPersonality.HasValue)
				{
					config3 = this.Config;
					config3.minionPersonality.Value.SetSelectedTemplateOutfitId(this.outfitState.destinationTarget.OutfitType, this.outfitState.destinationTarget.OutfitId);
				}
				if (this.Config.onWriteToOutfitTargetFn != null)
				{
					this.Config.onWriteToOutfitTargetFn(this.outfitState.destinationTarget);
				}
				LockerNavigator.Instance.PopScreen();
			};
			this.secondaryButton.ClearOnClick();
			this.secondaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.BUTTON_COPY);
			this.secondaryButton.onClick += delegate()
			{
				OutfitDesignerScreen.MakeCopyPopup(this, this.inputFieldPrefab, this.outfitState, this.Config.outfitTemplate.Value, this.Config.minionPersonality, this.Config.onWriteToOutfitTargetFn);
			};
			this.updateSaveButtonsFn = (System.Action)Delegate.Combine(this.updateSaveButtonsFn, new System.Action(delegate()
			{
				if (!this.outfitState.destinationTarget.CanWriteItems)
				{
					this.primaryButton.isInteractable = false;
					this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_READONLY);
					if (this.outfitState.DoesContainLockedItems())
					{
						this.secondaryButton.isInteractable = false;
						this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
						return;
					}
					this.secondaryButton.isInteractable = true;
					this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					return;
				}
				else
				{
					if (this.outfitState.DoesContainLockedItems())
					{
						this.primaryButton.isInteractable = false;
						this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
						this.secondaryButton.isInteractable = false;
						this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
						return;
					}
					this.primaryButton.isInteractable = true;
					this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					this.secondaryButton.isInteractable = true;
					this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					return;
				}
			}));
		}
		this.UpdateSaveButtons();
	}

	// Token: 0x0600677E RID: 26494 RVA: 0x0026A36C File Offset: 0x0026856C
	private void RefreshOutfitState()
	{
		this.selectionHeaderLabel.text = this.outfitState.name;
		this.outfitDescriptionPanel.Refresh(this.outfitState, this.Config.minionPersonality);
		this.UpdateSaveButtons();
	}

	// Token: 0x0600677F RID: 26495 RVA: 0x0026A3A6 File Offset: 0x002685A6
	private void RefreshCategories()
	{
		if (this.RefreshCategoriesFn != null)
		{
			this.RefreshCategoriesFn();
		}
	}

	// Token: 0x06006780 RID: 26496 RVA: 0x0026A3BC File Offset: 0x002685BC
	public void PopulateCategories()
	{
		this.RefreshCategoriesFn = null;
		this.categoryRowPool.ReturnAll();
		PermitCategory[] array = OutfitDesignerScreen.outfitTypeToCategoriesDict[this.outfitState.outfitType];
		for (int i = 0; i < array.Length; i++)
		{
			OutfitDesignerScreen.<>c__DisplayClass47_0 CS$<>8__locals1 = new OutfitDesignerScreen.<>c__DisplayClass47_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.permitCategory = array[i];
			GameObject gameObject = this.categoryRowPool.Borrow();
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(PermitCategories.GetUppercaseDisplayName(CS$<>8__locals1.permitCategory));
			component.GetReference<Image>("Icon").sprite = Assets.GetSprite(PermitCategories.GetIconName(CS$<>8__locals1.permitCategory));
			MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
			MultiToggle toggle2 = toggle;
			toggle2.onEnter = (System.Action)Delegate.Combine(toggle2.onEnter, new System.Action(this.OnMouseOverToggle));
			toggle.onClick = delegate()
			{
				CS$<>8__locals1.<>4__this.SelectCategory(CS$<>8__locals1.permitCategory);
			};
			this.RefreshCategoriesFn = (System.Action)Delegate.Combine(this.RefreshCategoriesFn, new System.Action(delegate()
			{
				toggle.ChangeState((CS$<>8__locals1.permitCategory == CS$<>8__locals1.<>4__this.SelectedCategory) ? 1 : 0);
			}));
			this.SetCatogoryClickUISound(CS$<>8__locals1.permitCategory, toggle);
		}
	}

	// Token: 0x06006781 RID: 26497 RVA: 0x0026A510 File Offset: 0x00268710
	public void SelectCategory(PermitCategory permitCategory)
	{
		this.SelectedCategory = permitCategory;
		this.galleryHeaderLabel.text = PermitCategories.GetDisplayName(permitCategory);
		this.RefreshCategories();
		this.PopulateGallery();
		Option<ClothingItemResource> itemForCategory = this.outfitState.GetItemForCategory(permitCategory);
		if (itemForCategory.HasValue)
		{
			this.SelectPermit(itemForCategory.Value);
			return;
		}
		this.SelectPermit(null);
	}

	// Token: 0x06006782 RID: 26498 RVA: 0x0026A56C File Offset: 0x0026876C
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x06006783 RID: 26499 RVA: 0x0026A584 File Offset: 0x00268784
	public void PopulateGallery()
	{
		OutfitDesignerScreen.<>c__DisplayClass51_0 CS$<>8__locals1 = new OutfitDesignerScreen.<>c__DisplayClass51_0();
		CS$<>8__locals1.<>4__this = this;
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		this.subcategoryUiPool.ReturnAll();
		this.galleryGridLayouter.targetGridLayouts.Clear();
		this.galleryGridLayouter.OnSizeGridComplete = null;
		CS$<>8__locals1.onFirstDisplayCategoryDecided = new Promise<KleiInventoryUISubcategory>();
		CS$<>8__locals1.<PopulateGallery>g__AddGridIconForPermit|0(null);
		foreach (ClothingItemResource clothingItemResource in Db.Get().Permits.ClothingItems.resources)
		{
			if (clothingItemResource.Category == this.SelectedCategory && clothingItemResource.outfitType == this.Config.sourceTarget.OutfitType && !clothingItemResource.Id.StartsWith("visonly_"))
			{
				CS$<>8__locals1.<PopulateGallery>g__AddGridIconForPermit|0(clothingItemResource);
			}
		}
		foreach (GameObject gameObject3 in this.subcategoryUiPool.GetBorrowedObjects().StableSort(Comparer<GameObject>.Create(delegate(GameObject a, GameObject b)
		{
			KleiInventoryUISubcategory component = a.GetComponent<KleiInventoryUISubcategory>();
			KleiInventoryUISubcategory component2 = b.GetComponent<KleiInventoryUISubcategory>();
			int sortKey = InventoryOrganization.subcategoryIdToPresentationDataMap[component.subcategoryID].sortKey;
			int sortKey2 = InventoryOrganization.subcategoryIdToPresentationDataMap[component2.subcategoryID].sortKey;
			return sortKey.CompareTo(sortKey2);
		})))
		{
			gameObject3.transform.SetAsLastSibling();
		}
		GameObject gameObject2 = this.subcategoryUiPool.GetBorrowedObjects().FirstOrDefault((GameObject gameObject) => gameObject.GetComponent<KleiInventoryUISubcategory>().IsOpen);
		if (gameObject2 != null)
		{
			CS$<>8__locals1.onFirstDisplayCategoryDecided.Resolve(gameObject2.GetComponent<KleiInventoryUISubcategory>());
		}
		this.galleryGridLayouter.RequestGridResize();
		this.RefreshGallery();
	}

	// Token: 0x06006784 RID: 26500 RVA: 0x0026A748 File Offset: 0x00268948
	public void SelectPermit(PermitResource permit)
	{
		this.SelectedPermit = permit;
		this.RefreshGallery();
		this.UpdateSelectedItemDetails();
		this.UpdateSaveButtons();
	}

	// Token: 0x06006785 RID: 26501 RVA: 0x0026A764 File Offset: 0x00268964
	public void UpdateSelectedItemDetails()
	{
		Option<ClothingItemResource> item = Option.None;
		if (this.SelectedPermit != null)
		{
			ClothingItemResource clothingItemResource = this.SelectedPermit as ClothingItemResource;
			if (clothingItemResource != null)
			{
				item = clothingItemResource;
			}
		}
		this.outfitState.SetItemForCategory(this.SelectedCategory, item);
		this.minionOrMannequin.current.SetOutfit(this.outfitState);
		this.minionOrMannequin.current.ReactToClothingItemChange(this.SelectedCategory);
		this.outfitDescriptionPanel.Refresh(this.outfitState, this.Config.minionPersonality);
		this.dioramaBG.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.SelectedCategory);
	}

	// Token: 0x06006786 RID: 26502 RVA: 0x0026A80A File Offset: 0x00268A0A
	private void RegisterPreventScreenPop()
	{
		this.UnregisterPreventScreenPop();
		this.preventScreenPopFn = delegate()
		{
			if (this.outfitState.IsDirty())
			{
				this.RegisterPreventScreenPop();
				OutfitDesignerScreen.MakeSaveWarningPopup(this.outfitState, delegate
				{
					this.UnregisterPreventScreenPop();
					LockerNavigator.Instance.PopScreen();
				});
				return true;
			}
			return false;
		};
		LockerNavigator.Instance.preventScreenPop.Add(this.preventScreenPopFn);
	}

	// Token: 0x06006787 RID: 26503 RVA: 0x0026A839 File Offset: 0x00268A39
	private void UnregisterPreventScreenPop()
	{
		if (this.preventScreenPopFn != null)
		{
			LockerNavigator.Instance.preventScreenPop.Remove(this.preventScreenPopFn);
			this.preventScreenPopFn = null;
		}
	}

	// Token: 0x06006788 RID: 26504 RVA: 0x0026A860 File Offset: 0x00268A60
	public static void MakeSaveWarningPopup(OutfitDesignerScreen_OutfitState outfitState, System.Action discardChangesFn)
	{
		Action<InfoDialogScreen> <>9__1;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.HEADER.Replace("{OutfitName}", outfitState.name)).AddPlainText(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BODY);
			string text = UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_DISCARD;
			Action<InfoDialogScreen> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(InfoDialogScreen d)
				{
					d.Deactivate();
					discardChangesFn();
				});
			}
			infoDialogScreen.AddOption(text, action, true).AddOption(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_RETURN, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
		});
	}

	// Token: 0x06006789 RID: 26505 RVA: 0x0026A898 File Offset: 0x00268A98
	public static void MakeApplyToTemplatePopup(KInputTextField inputFieldPrefab, OutfitDesignerScreen_OutfitState outfitState, GameObject targetMinionInstance, Option<ClothingOutfitTarget> existingOutfitTemplate, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		ClothingOutfitNameProposal proposal = default(ClothingOutfitNameProposal);
		Color errorTextColor = Util.ColorFromHex("F44A47");
		Color defaultTextColor = Util.ColorFromHex("FFFFFF");
		KInputTextField inputField;
		InfoScreenPlainText descLabel;
		KButton saveButton;
		LocText saveButtonText;
		LocText descLocText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.HEADER.Replace("{OutfitName}", outfitState.name)).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out descLabel).AddOption(true, out saveButton, out saveButtonText).AddDefaultCancel();
			descLocText = descLabel.gameObject.GetComponent<LocText>();
			descLocText.allowOverride = true;
			descLocText.alignment = TextAlignmentOptions.BottomLeft;
			descLocText.color = errorTextColor;
			descLocText.fontSize = 14f;
			descLabel.SetText("");
			inputField.onValueChanged.AddListener(new UnityAction<string>(base.<MakeApplyToTemplatePopup>g__Refresh|1));
			saveButton.onClick += delegate()
			{
				ClothingOutfitTarget clothingOutfitTarget = ClothingOutfitTarget.FromMinion(outfitState.outfitType, targetMinionInstance);
				ClothingOutfitNameProposal.Result result = proposal.result;
				ClothingOutfitTarget obj;
				if (result != ClothingOutfitNameProposal.Result.NewOutfit)
				{
					if (result != ClothingOutfitNameProposal.Result.SameOutfit)
					{
						throw new NotSupportedException(string.Format("Can't save outfit with name \"{0}\", failed with result: {1}", proposal.candidateName, proposal.result));
					}
					obj = existingOutfitTemplate.Value;
				}
				else
				{
					obj = ClothingOutfitTarget.ForNewTemplateOutfit(outfitState.outfitType, proposal.candidateName);
				}
				obj.WriteItems(outfitState.outfitType, outfitState.GetItems());
				clothingOutfitTarget.WriteItems(outfitState.outfitType, outfitState.GetItems());
				if (onWriteToOutfitTargetFn != null)
				{
					onWriteToOutfitTargetFn(obj);
				}
				dialog.Deactivate();
				LockerNavigator.Instance.PopScreen();
			};
			if (!existingOutfitTemplate.HasValue)
			{
				base.<MakeApplyToTemplatePopup>g__Refresh|1(outfitState.name);
				return;
			}
			if (existingOutfitTemplate.Value.CanWriteName && existingOutfitTemplate.Value.CanWriteItems)
			{
				base.<MakeApplyToTemplatePopup>g__Refresh|1(existingOutfitTemplate.Value.OutfitId);
				return;
			}
			base.<MakeApplyToTemplatePopup>g__Refresh|1(ClothingOutfitTarget.ForTemplateCopyOf(existingOutfitTemplate.Value).OutfitId);
		});
	}

	// Token: 0x0600678A RID: 26506 RVA: 0x0026A914 File Offset: 0x00268B14
	public static void MakeCopyPopup(OutfitDesignerScreen screen, KInputTextField inputFieldPrefab, OutfitDesignerScreen_OutfitState outfitState, ClothingOutfitTarget outfitTemplate, Option<Personality> minionPersonality, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		ClothingOutfitNameProposal proposal = default(ClothingOutfitNameProposal);
		KInputTextField inputField;
		InfoScreenPlainText errorText;
		KButton okButton;
		LocText okButtonText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.COPY_POPUP.HEADER).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out errorText).AddOption(true, out okButton, out okButtonText).AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
			inputField.onValueChanged.AddListener(new UnityAction<string>(base.<MakeCopyPopup>g__Refresh|1));
			errorText.gameObject.SetActive(false);
			LocText component = errorText.gameObject.GetComponent<LocText>();
			component.allowOverride = true;
			component.alignment = TextAlignmentOptions.BottomLeft;
			component.color = Util.ColorFromHex("F44A47");
			component.fontSize = 14f;
			errorText.SetText("");
			okButtonText.text = UI.CONFIRMDIALOG.OK;
			okButton.onClick += delegate()
			{
				if (proposal.result == ClothingOutfitNameProposal.Result.NewOutfit)
				{
					ClothingOutfitTarget clothingOutfitTarget = ClothingOutfitTarget.ForNewTemplateOutfit(outfitTemplate.OutfitType, proposal.candidateName);
					clothingOutfitTarget.WriteItems(outfitState.outfitType, outfitState.GetItems());
					if (minionPersonality.HasValue)
					{
						minionPersonality.Value.SetSelectedTemplateOutfitId(clothingOutfitTarget.OutfitType, clothingOutfitTarget.OutfitId);
					}
					if (onWriteToOutfitTargetFn != null)
					{
						onWriteToOutfitTargetFn(clothingOutfitTarget);
					}
					dialog.Deactivate();
					screen.Configure(screen.Config.WithOutfit(clothingOutfitTarget));
					return;
				}
				throw new NotSupportedException(string.Format("Can't save outfit with name \"{0}\", failed with result: {1}", proposal.candidateName, proposal.result));
			};
			base.<MakeCopyPopup>g__Refresh|1(ClothingOutfitTarget.ForTemplateCopyOf(outfitTemplate).OutfitId);
		});
	}

	// Token: 0x0600678B RID: 26507 RVA: 0x0026A978 File Offset: 0x00268B78
	private void SetCatogoryClickUISound(PermitCategory category, MultiToggle toggle)
	{
		toggle.states[1].on_click_override_sound_path = category.ToString() + "_Click";
		toggle.states[0].on_click_override_sound_path = category.ToString() + "_Click";
	}

	// Token: 0x0600678C RID: 26508 RVA: 0x0026A9D8 File Offset: 0x00268BD8
	private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
	{
		if (permit == null)
		{
			toggle.states[1].on_click_override_sound_path = "HUD_Click";
			toggle.states[0].on_click_override_sound_path = "HUD_Click";
			return;
		}
		string clothingItemSoundName = OutfitDesignerScreen.GetClothingItemSoundName(permit);
		toggle.states[1].on_click_override_sound_path = clothingItemSoundName + "_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = clothingItemSoundName + "_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x0600678D RID: 26509 RVA: 0x0026AAF0 File Offset: 0x00268CF0
	public static string GetClothingItemSoundName(PermitResource permit)
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
		default:
			return "HUD";
		}
	}

	// Token: 0x0600678E RID: 26510 RVA: 0x0026AB51 File Offset: 0x00268D51
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x040045D6 RID: 17878
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x040045D7 RID: 17879
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x040045D8 RID: 17880
	private UIPrefabLocalPool categoryRowPool;

	// Token: 0x040045D9 RID: 17881
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x040045DA RID: 17882
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x040045DB RID: 17883
	[SerializeField]
	private GameObject subcategoryUiPrefab;

	// Token: 0x040045DC RID: 17884
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x040045DD RID: 17885
	private UIPrefabLocalPool subcategoryUiPool;

	// Token: 0x040045DE RID: 17886
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x040045DF RID: 17887
	private GridLayouter galleryGridLayouter;

	// Token: 0x040045E0 RID: 17888
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x040045E1 RID: 17889
	[SerializeField]
	private UIMinionOrMannequin minionOrMannequin;

	// Token: 0x040045E2 RID: 17890
	[SerializeField]
	private Image dioramaBG;

	// Token: 0x040045E3 RID: 17891
	[SerializeField]
	private KButton primaryButton;

	// Token: 0x040045E4 RID: 17892
	[SerializeField]
	private KButton secondaryButton;

	// Token: 0x040045E5 RID: 17893
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x040045E6 RID: 17894
	[SerializeField]
	private KInputTextField inputFieldPrefab;

	// Token: 0x040045EB RID: 17899
	public static Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]> outfitTypeToCategoriesDict;

	// Token: 0x040045EC RID: 17900
	private bool postponeConfiguration = true;

	// Token: 0x040045ED RID: 17901
	private System.Action updateSaveButtonsFn;

	// Token: 0x040045EE RID: 17902
	private System.Action RefreshCategoriesFn;

	// Token: 0x040045EF RID: 17903
	private System.Action RefreshGalleryFn;

	// Token: 0x040045F0 RID: 17904
	private Func<bool> preventScreenPopFn;

	// Token: 0x02001E15 RID: 7701
	private enum MultiToggleState
	{
		// Token: 0x04008941 RID: 35137
		Default,
		// Token: 0x04008942 RID: 35138
		Selected,
		// Token: 0x04008943 RID: 35139
		NonInteractable
	}
}
