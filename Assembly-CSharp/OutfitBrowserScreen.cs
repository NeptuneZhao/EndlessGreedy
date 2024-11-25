using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000D00 RID: 3328
public class OutfitBrowserScreen : KMonoBehaviour
{
	// Token: 0x06006742 RID: 26434 RVA: 0x00268718 File Offset: 0x00266918
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
		this.gridLayouter = new GridLayouter
		{
			minCellSize = 112f,
			maxCellSize = 144f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.categoriesAndSearchBar.InitializeWith(this);
		this.pickOutfitButton.onClick += this.OnClickPickOutfit;
		this.editOutfitButton.onClick += delegate()
		{
			if (this.state.SelectedOutfitOpt.IsNone())
			{
				return;
			}
			new OutfitDesignerScreenConfig(this.state.SelectedOutfitOpt.Unwrap(), this.Config.minionPersonality, this.Config.targetMinionInstance, new Action<ClothingOutfitTarget>(this.OnOutfitDesignerWritesToOutfitTarget)).ApplyAndOpenScreen();
		};
		this.renameOutfitButton.onClick += delegate()
		{
			ClothingOutfitTarget selectedOutfit = this.state.SelectedOutfitOpt.Unwrap();
			OutfitBrowserScreen.MakeRenamePopup(this.inputFieldPrefab, selectedOutfit, () => selectedOutfit.ReadName(), delegate(string new_name)
			{
				selectedOutfit.WriteName(new_name);
				this.Configure(this.Config.WithOutfit(selectedOutfit));
			});
		};
		this.deleteOutfitButton.onClick += delegate()
		{
			ClothingOutfitTarget selectedOutfit = this.state.SelectedOutfitOpt.Unwrap();
			OutfitBrowserScreen.MakeDeletePopup(selectedOutfit, delegate
			{
				selectedOutfit.Delete();
				this.Configure(this.Config.WithOutfit(Option.None));
			});
		};
	}

	// Token: 0x1700075F RID: 1887
	// (get) Token: 0x06006743 RID: 26435 RVA: 0x002687E6 File Offset: 0x002669E6
	// (set) Token: 0x06006744 RID: 26436 RVA: 0x002687EE File Offset: 0x002669EE
	public OutfitBrowserScreenConfig Config { get; private set; }

	// Token: 0x06006745 RID: 26437 RVA: 0x002687F8 File Offset: 0x002669F8
	protected override void OnCmpEnable()
	{
		if (this.isFirstDisplay)
		{
			this.isFirstDisplay = false;
			this.dioramaMinionOrMannequin.TrySpawn();
			this.FirstTimeSetup();
			this.postponeConfiguration = false;
			this.Configure(this.Config);
		}
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshGallery();
			this.outfitDescriptionPanel.Refresh(this.state.SelectedOutfitOpt, ClothingOutfitUtility.OutfitType.Clothing, this.Config.minionPersonality);
		});
	}

	// Token: 0x06006746 RID: 26438 RVA: 0x00268850 File Offset: 0x00266A50
	private void FirstTimeSetup()
	{
		this.state.OnCurrentOutfitTypeChanged += delegate()
		{
			this.PopulateGallery();
			OutfitBrowserScreenConfig config = this.Config;
			Option<ClothingOutfitTarget> selectedOutfitOpt;
			if (!config.minionPersonality.HasValue)
			{
				config = this.Config;
				if (!config.selectedTarget.HasValue)
				{
					selectedOutfitOpt = ClothingOutfitTarget.GetRandom(this.state.CurrentOutfitType);
					goto IL_4F;
				}
			}
			selectedOutfitOpt = this.Config.selectedTarget;
			IL_4F:
			if (selectedOutfitOpt.IsSome() && selectedOutfitOpt.Unwrap().DoesExist())
			{
				this.state.SelectedOutfitOpt = selectedOutfitOpt;
				return;
			}
			this.state.SelectedOutfitOpt = Option.None;
		};
		this.state.OnSelectedOutfitOptChanged += delegate()
		{
			if (this.state.SelectedOutfitOpt.IsSome())
			{
				this.selectionHeaderLabel.text = this.state.SelectedOutfitOpt.Unwrap().ReadName();
			}
			else
			{
				this.selectionHeaderLabel.text = UI.OUTFIT_NAME.NONE;
			}
			this.dioramaMinionOrMannequin.current.SetOutfit(this.state.CurrentOutfitType, this.state.SelectedOutfitOpt);
			this.dioramaMinionOrMannequin.current.ReactToFullOutfitChange();
			this.outfitDescriptionPanel.Refresh(this.state.SelectedOutfitOpt, this.state.CurrentOutfitType, this.Config.minionPersonality);
			this.dioramaBG.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.state.CurrentOutfitType);
			this.pickOutfitButton.gameObject.SetActive(this.Config.isPickingOutfitForDupe);
			OutfitBrowserScreenConfig config = this.Config;
			if (config.minionPersonality.IsSome())
			{
				this.pickOutfitButton.isInteractable = (!this.state.SelectedOutfitOpt.IsSome() || !this.state.SelectedOutfitOpt.Unwrap().DoesContainLockedItems());
				GameObject gameObject = this.pickOutfitButton.gameObject;
				Option<string> tooltipText;
				if (!this.pickOutfitButton.isInteractable)
				{
					LocString tooltip_PICK_OUTFIT_ERROR_LOCKED = UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_PICK_OUTFIT_ERROR_LOCKED;
					string search = "{MinionName}";
					config = this.Config;
					tooltipText = Option.Some<string>(tooltip_PICK_OUTFIT_ERROR_LOCKED.Replace(search, config.GetMinionName()));
				}
				else
				{
					tooltipText = Option.None;
				}
				KleiItemsUI.ConfigureTooltipOn(gameObject, tooltipText);
			}
			this.editOutfitButton.isInteractable = this.state.SelectedOutfitOpt.IsSome();
			this.renameOutfitButton.isInteractable = (this.state.SelectedOutfitOpt.IsSome() && this.state.SelectedOutfitOpt.Unwrap().CanWriteName);
			KleiItemsUI.ConfigureTooltipOn(this.renameOutfitButton.gameObject, this.renameOutfitButton.isInteractable ? UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_RENAME_OUTFIT : UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_RENAME_OUTFIT_ERROR_READONLY);
			this.deleteOutfitButton.isInteractable = (this.state.SelectedOutfitOpt.IsSome() && this.state.SelectedOutfitOpt.Unwrap().CanDelete);
			KleiItemsUI.ConfigureTooltipOn(this.deleteOutfitButton.gameObject, this.deleteOutfitButton.isInteractable ? UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_DELETE_OUTFIT : UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_DELETE_OUTFIT_ERROR_READONLY);
			this.state.OnSelectedOutfitOptChanged += this.RefreshGallery;
			this.state.OnFilterChanged += this.RefreshGallery;
			this.state.OnCurrentOutfitTypeChanged += this.RefreshGallery;
			this.RefreshGallery();
		};
	}

	// Token: 0x06006747 RID: 26439 RVA: 0x00268880 File Offset: 0x00266A80
	public void Configure(OutfitBrowserScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.dioramaMinionOrMannequin.SetFrom(config.minionPersonality);
		if (config.targetMinionInstance.HasValue)
		{
			this.galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.MINION_GALLERY_HEADER.Replace("{MinionName}", config.targetMinionInstance.Value.GetProperName());
		}
		else if (config.minionPersonality.HasValue)
		{
			this.galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.MINION_GALLERY_HEADER.Replace("{MinionName}", config.minionPersonality.Value.Name);
		}
		else
		{
			this.galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.GALLERY_HEADER;
		}
		this.state.CurrentOutfitType = config.onlyShowOutfitType.UnwrapOr(this.lastShownOutfitType.UnwrapOr(ClothingOutfitUtility.OutfitType.Clothing, null), null);
		if (base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06006748 RID: 26440 RVA: 0x00268984 File Offset: 0x00266B84
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x06006749 RID: 26441 RVA: 0x0026899C File Offset: 0x00266B9C
	private void PopulateGallery()
	{
		this.outfits.Clear();
		this.galleryGridItemPool.ReturnAll();
		this.RefreshGalleryFn = null;
		if (this.Config.isPickingOutfitForDupe)
		{
			this.<PopulateGallery>g__AddGridIconForTarget|35_0(Option.None);
		}
		OutfitBrowserScreenConfig config = this.Config;
		if (config.targetMinionInstance.HasValue)
		{
			ClothingOutfitUtility.OutfitType currentOutfitType = this.state.CurrentOutfitType;
			config = this.Config;
			this.<PopulateGallery>g__AddGridIconForTarget|35_0(ClothingOutfitTarget.FromMinion(currentOutfitType, config.targetMinionInstance.Value));
		}
		foreach (ClothingOutfitTarget value in from outfit in ClothingOutfitTarget.GetAllTemplates()
		where outfit.OutfitType == this.state.CurrentOutfitType
		select outfit)
		{
			this.<PopulateGallery>g__AddGridIconForTarget|35_0(value);
		}
		this.addButtonGridItem.transform.SetAsLastSibling();
		this.addButtonGridItem.SetActive(true);
		this.addButtonGridItem.GetComponent<MultiToggle>().onClick = delegate()
		{
			new OutfitDesignerScreenConfig(ClothingOutfitTarget.ForNewTemplateOutfit(this.state.CurrentOutfitType), this.Config.minionPersonality, this.Config.targetMinionInstance, new Action<ClothingOutfitTarget>(this.OnOutfitDesignerWritesToOutfitTarget)).ApplyAndOpenScreen();
		};
		this.RefreshGallery();
	}

	// Token: 0x0600674A RID: 26442 RVA: 0x00268ABC File Offset: 0x00266CBC
	private void OnOutfitDesignerWritesToOutfitTarget(ClothingOutfitTarget outfit)
	{
		this.Configure(this.Config.WithOutfit(outfit));
	}

	// Token: 0x0600674B RID: 26443 RVA: 0x00268AE3 File Offset: 0x00266CE3
	private void Update()
	{
		this.gridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x0600674C RID: 26444 RVA: 0x00268AF0 File Offset: 0x00266CF0
	private void OnClickPickOutfit()
	{
		OutfitBrowserScreenConfig config = this.Config;
		if (config.targetMinionInstance.IsSome())
		{
			config = this.Config;
			WearableAccessorizer component = config.targetMinionInstance.Unwrap().GetComponent<WearableAccessorizer>();
			ClothingOutfitUtility.OutfitType currentOutfitType = this.state.CurrentOutfitType;
			Option<ClothingOutfitTarget> selectedOutfitOpt = this.state.SelectedOutfitOpt;
			component.ApplyClothingItems(currentOutfitType, selectedOutfitOpt.AndThen<IEnumerable<ClothingItemResource>>((ClothingOutfitTarget outfit) => outfit.ReadItemValues()).UnwrapOr(ClothingOutfitTarget.NO_ITEM_VALUES, null));
		}
		else
		{
			config = this.Config;
			if (config.minionPersonality.IsSome())
			{
				config = this.Config;
				Personality value = config.minionPersonality.Value;
				ClothingOutfitUtility.OutfitType currentOutfitType2 = this.state.CurrentOutfitType;
				Option<ClothingOutfitTarget> selectedOutfitOpt = this.state.SelectedOutfitOpt;
				value.SetSelectedTemplateOutfitId(currentOutfitType2, selectedOutfitOpt.AndThen<string>((ClothingOutfitTarget o) => o.OutfitId));
			}
		}
		LockerNavigator.Instance.PopScreen();
	}

	// Token: 0x0600674D RID: 26445 RVA: 0x00268BF4 File Offset: 0x00266DF4
	public static void MakeDeletePopup(ClothingOutfitTarget sourceTarget, System.Action deleteFn)
	{
		Action<InfoDialogScreen> <>9__1;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.HEADER.Replace("{OutfitName}", sourceTarget.ReadName())).AddPlainText(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BODY.Replace("{OutfitName}", sourceTarget.ReadName()));
			string text = UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BUTTON_YES_DELETE;
			Action<InfoDialogScreen> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(InfoDialogScreen d)
				{
					deleteFn();
					d.Deactivate();
				});
			}
			infoDialogScreen.AddOption(text, action, true).AddOption(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BUTTON_DONT_DELETE, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
		});
	}

	// Token: 0x0600674E RID: 26446 RVA: 0x00268C2C File Offset: 0x00266E2C
	public static void MakeRenamePopup(KInputTextField inputFieldPrefab, ClothingOutfitTarget sourceTarget, Func<string> readName, Action<string> writeName)
	{
		KInputTextField inputField;
		InfoScreenPlainText errorText;
		KButton okButton;
		LocText okButtonText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_BROWSER_SCREEN.RENAME_POPUP.HEADER).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out errorText).AddOption(true, out okButton, out okButtonText).AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
			inputField.onValueChanged.AddListener(new UnityAction<string>(base.<MakeRenamePopup>g__Refresh|1));
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
				writeName(inputField.text);
				dialog.Deactivate();
			};
			base.<MakeRenamePopup>g__Refresh|1(readName());
		});
	}

	// Token: 0x0600674F RID: 26447 RVA: 0x00268C74 File Offset: 0x00266E74
	private void SetButtonClickUISound(Option<ClothingOutfitTarget> target, MultiToggle toggle)
	{
		if (!target.HasValue)
		{
			toggle.states[1].on_click_override_sound_path = "HUD_Click";
			toggle.states[0].on_click_override_sound_path = "HUD_Click";
			return;
		}
		bool flag = !target.Value.DoesContainLockedItems();
		toggle.states[1].on_click_override_sound_path = "ClothingItem_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (flag ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = "ClothingItem_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (flag ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x06006750 RID: 26448 RVA: 0x00268D86 File Offset: 0x00266F86
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06006758 RID: 26456 RVA: 0x00269280 File Offset: 0x00267480
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIconForTarget|35_0(Option<ClothingOutfitTarget> target)
	{
		GameObject spawn = this.galleryGridItemPool.Borrow();
		GameObject gameObject = spawn.transform.GetChild(1).gameObject;
		GameObject isUnownedOverlayGO = spawn.transform.GetChild(2).gameObject;
		GameObject dlcBannerGO = spawn.transform.GetChild(3).gameObject;
		gameObject.SetActive(true);
		bool shouldShowOutfitWithDefaultItems = target.IsNone() || this.state.CurrentOutfitType == ClothingOutfitUtility.OutfitType.AtmoSuit;
		UIMannequin componentInChildren = gameObject.GetComponentInChildren<UIMannequin>();
		this.dioramaMinionOrMannequin.mannequin.shouldShowOutfitWithDefaultItems = shouldShowOutfitWithDefaultItems;
		componentInChildren.shouldShowOutfitWithDefaultItems = shouldShowOutfitWithDefaultItems;
		componentInChildren.personalityToUseForDefaultClothing = this.Config.minionPersonality;
		componentInChildren.SetOutfit(this.state.CurrentOutfitType, target);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		float x;
		float num;
		float num2;
		float y;
		switch (this.state.CurrentOutfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			x = 8f;
			num = 8f;
			num2 = 8f;
			y = 8f;
			break;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			throw new NotSupportedException();
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			x = 24f;
			num = 16f;
			num2 = 32f;
			y = 8f;
			break;
		default:
			throw new NotImplementedException();
		}
		component.offsetMin = new Vector2(x, y);
		component.offsetMax = new Vector2(-num, -num2);
		MultiToggle button = spawn.GetComponent<MultiToggle>();
		MultiToggle button2 = button;
		button2.onEnter = (System.Action)Delegate.Combine(button2.onEnter, new System.Action(this.OnMouseOverToggle));
		button.onClick = delegate()
		{
			this.state.SelectedOutfitOpt = target;
		};
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			button.ChangeState((target == this.state.SelectedOutfitOpt) ? 1 : 0);
			if (string.IsNullOrWhiteSpace(this.state.Filter) || target.IsNone())
			{
				spawn.SetActive(true);
			}
			else
			{
				spawn.SetActive(target.Unwrap().ReadName().ToLower().Contains(this.state.Filter.ToLower()));
			}
			if (!target.HasValue)
			{
				KleiItemsUI.ConfigureTooltipOn(spawn, KleiItemsUI.WrapAsToolTipTitle(KleiItemsUI.GetNoneOutfitName(this.state.CurrentOutfitType)));
				isUnownedOverlayGO.SetActive(false);
			}
			else
			{
				KleiItemsUI.ConfigureTooltipOn(spawn, KleiItemsUI.WrapAsToolTipTitle(target.Value.ReadName()));
				isUnownedOverlayGO.SetActive(target.Value.DoesContainLockedItems());
			}
			if (target.IsSome())
			{
				ClothingOutfitTarget.Implementation impl = target.Unwrap().impl;
				if (impl is ClothingOutfitTarget.DatabaseAuthoredTemplate)
				{
					ClothingOutfitTarget.DatabaseAuthoredTemplate databaseAuthoredTemplate = (ClothingOutfitTarget.DatabaseAuthoredTemplate)impl;
					string dlcIdFrom = databaseAuthoredTemplate.resource.GetDlcIdFrom();
					if (DlcManager.IsDlcId(dlcIdFrom))
					{
						dlcBannerGO.GetComponent<Image>().color = DlcManager.GetDlcBannerColor(dlcIdFrom);
						dlcBannerGO.SetActive(true);
						return;
					}
					dlcBannerGO.SetActive(false);
					return;
				}
			}
			dlcBannerGO.SetActive(false);
		}));
		this.SetButtonClickUISound(target, button);
	}

	// Token: 0x040045AA RID: 17834
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x040045AB RID: 17835
	[SerializeField]
	private OutfitBrowserScreen_CategoriesAndSearchBar categoriesAndSearchBar;

	// Token: 0x040045AC RID: 17836
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x040045AD RID: 17837
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x040045AE RID: 17838
	[SerializeField]
	private GameObject addButtonGridItem;

	// Token: 0x040045AF RID: 17839
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x040045B0 RID: 17840
	private GridLayouter gridLayouter;

	// Token: 0x040045B1 RID: 17841
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x040045B2 RID: 17842
	[SerializeField]
	private UIMinionOrMannequin dioramaMinionOrMannequin;

	// Token: 0x040045B3 RID: 17843
	[SerializeField]
	private Image dioramaBG;

	// Token: 0x040045B4 RID: 17844
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x040045B5 RID: 17845
	[SerializeField]
	private KButton pickOutfitButton;

	// Token: 0x040045B6 RID: 17846
	[SerializeField]
	private KButton editOutfitButton;

	// Token: 0x040045B7 RID: 17847
	[SerializeField]
	private KButton renameOutfitButton;

	// Token: 0x040045B8 RID: 17848
	[SerializeField]
	private KButton deleteOutfitButton;

	// Token: 0x040045B9 RID: 17849
	[Header("Misc")]
	[SerializeField]
	private KInputTextField inputFieldPrefab;

	// Token: 0x040045BA RID: 17850
	[SerializeField]
	public ColorStyleSetting selectedCategoryStyle;

	// Token: 0x040045BB RID: 17851
	[SerializeField]
	public ColorStyleSetting notSelectedCategoryStyle;

	// Token: 0x040045BC RID: 17852
	public OutfitBrowserScreen.State state = new OutfitBrowserScreen.State();

	// Token: 0x040045BD RID: 17853
	public Option<ClothingOutfitUtility.OutfitType> lastShownOutfitType = Option.None;

	// Token: 0x040045BE RID: 17854
	private Dictionary<string, MultiToggle> outfits = new Dictionary<string, MultiToggle>();

	// Token: 0x040045C0 RID: 17856
	private bool postponeConfiguration = true;

	// Token: 0x040045C1 RID: 17857
	private bool isFirstDisplay = true;

	// Token: 0x040045C2 RID: 17858
	private System.Action RefreshGalleryFn;

	// Token: 0x02001E08 RID: 7688
	public class State
	{
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x0600AA54 RID: 43604 RVA: 0x003A1524 File Offset: 0x0039F724
		// (remove) Token: 0x0600AA55 RID: 43605 RVA: 0x003A155C File Offset: 0x0039F75C
		public event System.Action OnSelectedOutfitOptChanged;

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x0600AA56 RID: 43606 RVA: 0x003A1591 File Offset: 0x0039F791
		// (set) Token: 0x0600AA57 RID: 43607 RVA: 0x003A1599 File Offset: 0x0039F799
		public Option<ClothingOutfitTarget> SelectedOutfitOpt
		{
			get
			{
				return this.m_selectedOutfitOpt;
			}
			set
			{
				this.m_selectedOutfitOpt = value;
				if (this.OnSelectedOutfitOptChanged != null)
				{
					this.OnSelectedOutfitOptChanged();
				}
			}
		}

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x0600AA58 RID: 43608 RVA: 0x003A15B8 File Offset: 0x0039F7B8
		// (remove) Token: 0x0600AA59 RID: 43609 RVA: 0x003A15F0 File Offset: 0x0039F7F0
		public event System.Action OnCurrentOutfitTypeChanged;

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x0600AA5A RID: 43610 RVA: 0x003A1625 File Offset: 0x0039F825
		// (set) Token: 0x0600AA5B RID: 43611 RVA: 0x003A162D File Offset: 0x0039F82D
		public ClothingOutfitUtility.OutfitType CurrentOutfitType
		{
			get
			{
				return this.m_currentOutfitType;
			}
			set
			{
				this.m_currentOutfitType = value;
				if (this.OnCurrentOutfitTypeChanged != null)
				{
					this.OnCurrentOutfitTypeChanged();
				}
			}
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x0600AA5C RID: 43612 RVA: 0x003A164C File Offset: 0x0039F84C
		// (remove) Token: 0x0600AA5D RID: 43613 RVA: 0x003A1684 File Offset: 0x0039F884
		public event System.Action OnFilterChanged;

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x0600AA5E RID: 43614 RVA: 0x003A16B9 File Offset: 0x0039F8B9
		// (set) Token: 0x0600AA5F RID: 43615 RVA: 0x003A16C1 File Offset: 0x0039F8C1
		public string Filter
		{
			get
			{
				return this.m_filter;
			}
			set
			{
				this.m_filter = value;
				if (this.OnFilterChanged != null)
				{
					this.OnFilterChanged();
				}
			}
		}

		// Token: 0x0400890A RID: 35082
		private Option<ClothingOutfitTarget> m_selectedOutfitOpt;

		// Token: 0x0400890B RID: 35083
		private ClothingOutfitUtility.OutfitType m_currentOutfitType;

		// Token: 0x0400890C RID: 35084
		private string m_filter;
	}

	// Token: 0x02001E09 RID: 7689
	private enum MultiToggleState
	{
		// Token: 0x04008911 RID: 35089
		Default,
		// Token: 0x04008912 RID: 35090
		Selected,
		// Token: 0x04008913 RID: 35091
		NonInteractable
	}
}
