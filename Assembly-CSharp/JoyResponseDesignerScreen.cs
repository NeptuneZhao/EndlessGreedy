using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C6F RID: 3183
public class JoyResponseDesignerScreen : KMonoBehaviour
{
	// Token: 0x1700073C RID: 1852
	// (get) Token: 0x060061A5 RID: 24997 RVA: 0x0024766F File Offset: 0x0024586F
	// (set) Token: 0x060061A6 RID: 24998 RVA: 0x00247677 File Offset: 0x00245877
	public JoyResponseScreenConfig Config { get; private set; }

	// Token: 0x060061A7 RID: 24999 RVA: 0x00247680 File Offset: 0x00245880
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(this.categoryRowPrefab.transform.parent == this.categoryListContent.transform);
		global::Debug.Assert(this.galleryItemPrefab.transform.parent == this.galleryGridContent.transform);
		this.categoryRowPrefab.SetActive(false);
		this.galleryItemPrefab.SetActive(false);
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.categoryRowPool = new UIPrefabLocalPool(this.categoryRowPrefab, this.categoryListContent.gameObject);
		this.galleryGridItemPool = new UIPrefabLocalPool(this.galleryItemPrefab, this.galleryGridContent.gameObject);
		JoyResponseDesignerScreen.JoyResponseCategory[] array = new JoyResponseDesignerScreen.JoyResponseCategory[1];
		int num = 0;
		JoyResponseDesignerScreen.JoyResponseCategory joyResponseCategory = new JoyResponseDesignerScreen.JoyResponseCategory();
		joyResponseCategory.displayName = UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSES.BALLOON_ARTIST;
		joyResponseCategory.icon = Assets.GetSprite("icon_inventory_balloonartist");
		JoyResponseDesignerScreen.GalleryItem[] items = (from r in Db.Get().Permits.BalloonArtistFacades.resources
		select JoyResponseDesignerScreen.GalleryItem.Of(r)).Prepend(JoyResponseDesignerScreen.GalleryItem.Of(Option.None)).ToArray<JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget>();
		joyResponseCategory.items = items;
		array[num] = joyResponseCategory;
		this.joyResponseCategories = array;
		this.dioramaVis.ConfigureSetup();
	}

	// Token: 0x060061A8 RID: 25000 RVA: 0x00247801 File Offset: 0x00245A01
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x060061A9 RID: 25001 RVA: 0x0024780E File Offset: 0x00245A0E
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		if (this.Config.isValid)
		{
			this.Configure(this.Config);
			return;
		}
		throw new InvalidOperationException("Cannot open up JoyResponseDesignerScreen without a target personality or minion instance");
	}

	// Token: 0x060061AA RID: 25002 RVA: 0x0024783B File Offset: 0x00245A3B
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.Configure(this.Config);
		});
	}

	// Token: 0x060061AB RID: 25003 RVA: 0x0024785C File Offset: 0x00245A5C
	public void Configure(JoyResponseScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.RegisterPreventScreenPop();
		this.primaryButton.ClearOnClick();
		TMP_Text componentInChildren = this.primaryButton.GetComponentInChildren<LocText>();
		LocString button_APPLY_TO_MINION = UI.JOY_RESPONSE_DESIGNER_SCREEN.BUTTON_APPLY_TO_MINION;
		string search = "{MinionName}";
		JoyResponseScreenConfig config2 = this.Config;
		componentInChildren.SetText(button_APPLY_TO_MINION.Replace(search, config2.target.GetMinionName()));
		this.primaryButton.onClick += delegate()
		{
			Option<PermitResource> permitResource = this.selectedGalleryItem.GetPermitResource();
			if (permitResource.IsSome())
			{
				string str = "Save selected balloon ";
				string name = this.selectedGalleryItem.GetName();
				string str2 = " for ";
				JoyResponseScreenConfig config3 = this.Config;
				global::Debug.Log(str + name + str2 + config3.target.GetMinionName());
				if (this.CanSaveSelection())
				{
					config3 = this.Config;
					config3.target.WriteFacadeId(permitResource.Unwrap().Id);
				}
			}
			else
			{
				string str3 = "Save selected balloon ";
				string name2 = this.selectedGalleryItem.GetName();
				string str4 = " for ";
				JoyResponseScreenConfig config3 = this.Config;
				global::Debug.Log(str3 + name2 + str4 + config3.target.GetMinionName());
				config3 = this.Config;
				config3.target.WriteFacadeId(Option.None);
			}
			LockerNavigator.Instance.PopScreen();
		};
		this.PopulateCategories();
		this.PopulateGallery();
		this.PopulatePreview();
		config2 = this.Config;
		if (config2.initalSelectedItem.IsSome())
		{
			config2 = this.Config;
			this.SelectGalleryItem(config2.initalSelectedItem.Unwrap());
		}
	}

	// Token: 0x060061AC RID: 25004 RVA: 0x00247914 File Offset: 0x00245B14
	private bool CanSaveSelection()
	{
		return this.GetSaveSelectionError().IsNone();
	}

	// Token: 0x060061AD RID: 25005 RVA: 0x00247930 File Offset: 0x00245B30
	private Option<string> GetSaveSelectionError()
	{
		if (!this.selectedGalleryItem.IsUnlocked())
		{
			return Option.Some<string>(UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_PICK_JOY_RESPONSE_ERROR_LOCKED.Replace("{MinionName}", this.Config.target.GetMinionName()));
		}
		return Option.None;
	}

	// Token: 0x060061AE RID: 25006 RVA: 0x0024797C File Offset: 0x00245B7C
	private void RefreshCategories()
	{
		if (this.RefreshCategoriesFn != null)
		{
			this.RefreshCategoriesFn();
		}
	}

	// Token: 0x060061AF RID: 25007 RVA: 0x00247994 File Offset: 0x00245B94
	public void PopulateCategories()
	{
		this.RefreshCategoriesFn = null;
		this.categoryRowPool.ReturnAll();
		JoyResponseDesignerScreen.JoyResponseCategory[] array = this.joyResponseCategories;
		for (int i = 0; i < array.Length; i++)
		{
			JoyResponseDesignerScreen.<>c__DisplayClass28_0 CS$<>8__locals1 = new JoyResponseDesignerScreen.<>c__DisplayClass28_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.category = array[i];
			GameObject gameObject = this.categoryRowPool.Borrow();
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(CS$<>8__locals1.category.displayName);
			component.GetReference<Image>("Icon").sprite = CS$<>8__locals1.category.icon;
			MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
			MultiToggle toggle2 = toggle;
			toggle2.onEnter = (System.Action)Delegate.Combine(toggle2.onEnter, new System.Action(this.OnMouseOverToggle));
			toggle.onClick = delegate()
			{
				CS$<>8__locals1.<>4__this.SelectCategory(CS$<>8__locals1.category);
			};
			this.RefreshCategoriesFn = (System.Action)Delegate.Combine(this.RefreshCategoriesFn, new System.Action(delegate()
			{
				toggle.ChangeState((CS$<>8__locals1.category == CS$<>8__locals1.<>4__this.selectedCategoryOpt) ? 1 : 0);
			}));
			this.SetCatogoryClickUISound(CS$<>8__locals1.category, toggle);
		}
		this.SelectCategory(this.joyResponseCategories[0]);
	}

	// Token: 0x060061B0 RID: 25008 RVA: 0x00247ADB File Offset: 0x00245CDB
	public void SelectCategory(JoyResponseDesignerScreen.JoyResponseCategory category)
	{
		this.selectedCategoryOpt = category;
		this.galleryHeaderLabel.text = category.displayName;
		this.RefreshCategories();
		this.PopulateGallery();
		this.RefreshPreview();
	}

	// Token: 0x060061B1 RID: 25009 RVA: 0x00247B0C File Offset: 0x00245D0C
	private void SetCatogoryClickUISound(JoyResponseDesignerScreen.JoyResponseCategory category, MultiToggle toggle)
	{
	}

	// Token: 0x060061B2 RID: 25010 RVA: 0x00247B0E File Offset: 0x00245D0E
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x060061B3 RID: 25011 RVA: 0x00247B24 File Offset: 0x00245D24
	public void PopulateGallery()
	{
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		if (this.selectedCategoryOpt.IsNone())
		{
			return;
		}
		JoyResponseDesignerScreen.JoyResponseCategory joyResponseCategory = this.selectedCategoryOpt.Unwrap();
		foreach (JoyResponseDesignerScreen.GalleryItem item in joyResponseCategory.items)
		{
			this.<PopulateGallery>g__AddGridIcon|36_0(item);
		}
		this.SelectGalleryItem(joyResponseCategory.items[0]);
	}

	// Token: 0x060061B4 RID: 25012 RVA: 0x00247B8B File Offset: 0x00245D8B
	public void SelectGalleryItem(JoyResponseDesignerScreen.GalleryItem item)
	{
		this.selectedGalleryItem = item;
		this.RefreshGallery();
		this.RefreshPreview();
	}

	// Token: 0x060061B5 RID: 25013 RVA: 0x00247BA0 File Offset: 0x00245DA0
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x060061B6 RID: 25014 RVA: 0x00247BB2 File Offset: 0x00245DB2
	public void RefreshPreview()
	{
		if (this.RefreshPreviewFn != null)
		{
			this.RefreshPreviewFn();
		}
	}

	// Token: 0x060061B7 RID: 25015 RVA: 0x00247BC7 File Offset: 0x00245DC7
	public void PopulatePreview()
	{
		this.RefreshPreviewFn = (System.Action)Delegate.Combine(this.RefreshPreviewFn, new System.Action(delegate()
		{
			JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget balloonArtistFacadeTarget = this.selectedGalleryItem as JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget;
			if (balloonArtistFacadeTarget == null)
			{
				throw new NotImplementedException();
			}
			Option<PermitResource> permitResource = balloonArtistFacadeTarget.GetPermitResource();
			this.selectionHeaderLabel.SetText(balloonArtistFacadeTarget.GetName());
			KleiPermitDioramaVis_JoyResponseBalloon kleiPermitDioramaVis_JoyResponseBalloon = this.dioramaVis;
			JoyResponseScreenConfig config = this.Config;
			kleiPermitDioramaVis_JoyResponseBalloon.SetMinion(config.target.GetPersonality());
			this.dioramaVis.ConfigureWith(balloonArtistFacadeTarget.permit);
			OutfitDescriptionPanel outfitDescriptionPanel = this.outfitDescriptionPanel;
			PermitResource permitResource2 = permitResource.UnwrapOr(null, null);
			ClothingOutfitUtility.OutfitType outfitType = ClothingOutfitUtility.OutfitType.JoyResponse;
			config = this.Config;
			outfitDescriptionPanel.Refresh(permitResource2, outfitType, config.target.GetPersonality());
			Option<string> saveSelectionError = this.GetSaveSelectionError();
			if (saveSelectionError.IsSome())
			{
				this.primaryButton.isInteractable = false;
				this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(saveSelectionError.Unwrap());
				return;
			}
			this.primaryButton.isInteractable = true;
			this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
		}));
		this.RefreshPreview();
	}

	// Token: 0x060061B8 RID: 25016 RVA: 0x00247BF1 File Offset: 0x00245DF1
	private void RegisterPreventScreenPop()
	{
		this.UnregisterPreventScreenPop();
		this.preventScreenPopFn = delegate()
		{
			if (this.Config.target.ReadFacadeId() != this.selectedGalleryItem.GetPermitResource().AndThen<string>((PermitResource r) => r.Id))
			{
				this.RegisterPreventScreenPop();
				JoyResponseDesignerScreen.MakeSaveWarningPopup(this.Config.target, delegate
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

	// Token: 0x060061B9 RID: 25017 RVA: 0x00247C20 File Offset: 0x00245E20
	private void UnregisterPreventScreenPop()
	{
		if (this.preventScreenPopFn != null)
		{
			LockerNavigator.Instance.preventScreenPop.Remove(this.preventScreenPopFn);
			this.preventScreenPopFn = null;
		}
	}

	// Token: 0x060061BA RID: 25018 RVA: 0x00247C48 File Offset: 0x00245E48
	public static void MakeSaveWarningPopup(JoyResponseOutfitTarget target, System.Action discardChangesFn)
	{
		Action<InfoDialogScreen> <>9__1;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.JOY_RESPONSE_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.HEADER.Replace("{MinionName}", target.GetMinionName())).AddPlainText(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BODY);
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

	// Token: 0x060061BE RID: 25022 RVA: 0x00247D78 File Offset: 0x00245F78
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIcon|36_0(JoyResponseDesignerScreen.GalleryItem item)
	{
		GameObject gameObject = this.galleryGridItemPool.Borrow();
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = item.GetIcon();
		component.GetReference<Image>("IsUnownedOverlay").gameObject.SetActive(!item.IsUnlocked());
		Option<PermitResource> permitResource = item.GetPermitResource();
		if (permitResource.IsSome())
		{
			KleiItemsUI.ConfigureTooltipOn(gameObject, KleiItemsUI.GetTooltipStringFor(permitResource.Unwrap()));
		}
		else
		{
			KleiItemsUI.ConfigureTooltipOn(gameObject, KleiItemsUI.GetNoneTooltipStringFor(PermitCategory.JoyResponse));
		}
		MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
		MultiToggle toggle3 = toggle;
		toggle3.onEnter = (System.Action)Delegate.Combine(toggle3.onEnter, new System.Action(this.OnMouseOverToggle));
		MultiToggle toggle2 = toggle;
		toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
		{
			this.SelectGalleryItem(item);
		}));
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			toggle.ChangeState((item == this.selectedGalleryItem) ? 1 : 0);
		}));
	}

	// Token: 0x04004234 RID: 16948
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x04004235 RID: 16949
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x04004236 RID: 16950
	[Header("GalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x04004237 RID: 16951
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04004238 RID: 16952
	[SerializeField]
	private GameObject galleryItemPrefab;

	// Token: 0x04004239 RID: 16953
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x0400423A RID: 16954
	[SerializeField]
	private KleiPermitDioramaVis_JoyResponseBalloon dioramaVis;

	// Token: 0x0400423B RID: 16955
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x0400423C RID: 16956
	[SerializeField]
	private KButton primaryButton;

	// Token: 0x0400423E RID: 16958
	public JoyResponseDesignerScreen.JoyResponseCategory[] joyResponseCategories;

	// Token: 0x0400423F RID: 16959
	private bool postponeConfiguration = true;

	// Token: 0x04004240 RID: 16960
	private Option<JoyResponseDesignerScreen.JoyResponseCategory> selectedCategoryOpt;

	// Token: 0x04004241 RID: 16961
	private UIPrefabLocalPool categoryRowPool;

	// Token: 0x04004242 RID: 16962
	private System.Action RefreshCategoriesFn;

	// Token: 0x04004243 RID: 16963
	private JoyResponseDesignerScreen.GalleryItem selectedGalleryItem;

	// Token: 0x04004244 RID: 16964
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x04004245 RID: 16965
	private GridLayouter galleryGridLayouter;

	// Token: 0x04004246 RID: 16966
	private System.Action RefreshGalleryFn;

	// Token: 0x04004247 RID: 16967
	public System.Action RefreshPreviewFn;

	// Token: 0x04004248 RID: 16968
	private Func<bool> preventScreenPopFn;

	// Token: 0x02001D47 RID: 7495
	public class JoyResponseCategory
	{
		// Token: 0x040086CD RID: 34509
		public string displayName;

		// Token: 0x040086CE RID: 34510
		public Sprite icon;

		// Token: 0x040086CF RID: 34511
		public JoyResponseDesignerScreen.GalleryItem[] items;
	}

	// Token: 0x02001D48 RID: 7496
	private enum MultiToggleState
	{
		// Token: 0x040086D1 RID: 34513
		Default,
		// Token: 0x040086D2 RID: 34514
		Selected
	}

	// Token: 0x02001D49 RID: 7497
	public abstract class GalleryItem : IEquatable<JoyResponseDesignerScreen.GalleryItem>
	{
		// Token: 0x0600A82C RID: 43052
		public abstract string GetName();

		// Token: 0x0600A82D RID: 43053
		public abstract Sprite GetIcon();

		// Token: 0x0600A82E RID: 43054
		public abstract string GetUniqueId();

		// Token: 0x0600A82F RID: 43055
		public abstract bool IsUnlocked();

		// Token: 0x0600A830 RID: 43056
		public abstract Option<PermitResource> GetPermitResource();

		// Token: 0x0600A831 RID: 43057 RVA: 0x0039C190 File Offset: 0x0039A390
		public override bool Equals(object obj)
		{
			JoyResponseDesignerScreen.GalleryItem galleryItem = obj as JoyResponseDesignerScreen.GalleryItem;
			return galleryItem != null && this.Equals(galleryItem);
		}

		// Token: 0x0600A832 RID: 43058 RVA: 0x0039C1B0 File Offset: 0x0039A3B0
		public bool Equals(JoyResponseDesignerScreen.GalleryItem other)
		{
			return this.GetHashCode() == other.GetHashCode();
		}

		// Token: 0x0600A833 RID: 43059 RVA: 0x0039C1C0 File Offset: 0x0039A3C0
		public override int GetHashCode()
		{
			return Hash.SDBMLower(this.GetUniqueId());
		}

		// Token: 0x0600A834 RID: 43060 RVA: 0x0039C1CD File Offset: 0x0039A3CD
		public override string ToString()
		{
			return this.GetUniqueId();
		}

		// Token: 0x0600A835 RID: 43061 RVA: 0x0039C1D5 File Offset: 0x0039A3D5
		public static JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget Of(Option<BalloonArtistFacadeResource> permit)
		{
			return new JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget
			{
				permit = permit
			};
		}

		// Token: 0x02002653 RID: 9811
		public class BalloonArtistFacadeTarget : JoyResponseDesignerScreen.GalleryItem
		{
			// Token: 0x0600C1FD RID: 49661 RVA: 0x003DFCE8 File Offset: 0x003DDEE8
			public override Sprite GetIcon()
			{
				return this.permit.AndThen<Sprite>((BalloonArtistFacadeResource p) => p.GetPermitPresentationInfo().sprite).UnwrapOrElse(() => KleiItemsUI.GetNoneBalloonArtistIcon(), null);
			}

			// Token: 0x0600C1FE RID: 49662 RVA: 0x003DFD48 File Offset: 0x003DDF48
			public override string GetName()
			{
				return this.permit.AndThen<string>((BalloonArtistFacadeResource p) => p.Name).UnwrapOrElse(() => KleiItemsUI.GetNoneClothingItemStrings(PermitCategory.JoyResponse).Item1, null);
			}

			// Token: 0x0600C1FF RID: 49663 RVA: 0x003DFDA8 File Offset: 0x003DDFA8
			public override string GetUniqueId()
			{
				return "balloon_artist_facade::" + this.permit.AndThen<string>((BalloonArtistFacadeResource p) => p.Id).UnwrapOr("<none>", null);
			}

			// Token: 0x0600C200 RID: 49664 RVA: 0x003DFDF7 File Offset: 0x003DDFF7
			public override Option<PermitResource> GetPermitResource()
			{
				return this.permit.AndThen<PermitResource>((BalloonArtistFacadeResource p) => p);
			}

			// Token: 0x0600C201 RID: 49665 RVA: 0x003DFE24 File Offset: 0x003DE024
			public override bool IsUnlocked()
			{
				return this.GetPermitResource().AndThen<bool>((PermitResource p) => p.IsUnlocked()).UnwrapOr(true, null);
			}

			// Token: 0x0400AA65 RID: 43621
			public Option<BalloonArtistFacadeResource> permit;
		}
	}
}
