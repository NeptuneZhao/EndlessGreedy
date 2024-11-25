using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D01 RID: 3329
[Serializable]
public class OutfitBrowserScreen_CategoriesAndSearchBar
{
	// Token: 0x0600675B RID: 26459 RVA: 0x002694E0 File Offset: 0x002676E0
	public void InitializeWith(OutfitBrowserScreen outfitBrowserScreen)
	{
		this.outfitBrowserScreen = outfitBrowserScreen;
		this.clothingOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.selectOutfitType_Prefab.transform.parent.gameObject, true));
		this.clothingOutfitTypeButton.button.onClick += delegate()
		{
			this.SetOutfitType(ClothingOutfitUtility.OutfitType.Clothing);
		};
		this.clothingOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_equipment");
		KleiItemsUI.ConfigureTooltipOn(this.clothingOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_CLOTHING);
		this.atmosuitOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.selectOutfitType_Prefab.transform.parent.gameObject, true));
		this.atmosuitOutfitTypeButton.button.onClick += delegate()
		{
			this.SetOutfitType(ClothingOutfitUtility.OutfitType.AtmoSuit);
		};
		this.atmosuitOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_atmosuits");
		KleiItemsUI.ConfigureTooltipOn(this.atmosuitOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_ATMO_SUITS);
		this.searchTextField.onValueChanged.AddListener(delegate(string newFilter)
		{
			outfitBrowserScreen.state.Filter = newFilter;
		});
		this.searchTextField.transform.SetAsLastSibling();
		outfitBrowserScreen.state.OnCurrentOutfitTypeChanged += delegate()
		{
			if (outfitBrowserScreen.Config.onlyShowOutfitType.IsSome())
			{
				this.clothingOutfitTypeButton.root.gameObject.SetActive(false);
				this.atmosuitOutfitTypeButton.root.gameObject.SetActive(false);
				return;
			}
			this.clothingOutfitTypeButton.root.gameObject.SetActive(true);
			this.atmosuitOutfitTypeButton.root.gameObject.SetActive(true);
			this.clothingOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
			this.atmosuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
			ClothingOutfitUtility.OutfitType currentOutfitType = outfitBrowserScreen.state.CurrentOutfitType;
			if (currentOutfitType == ClothingOutfitUtility.OutfitType.Clothing)
			{
				this.clothingOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
				return;
			}
			if (currentOutfitType != ClothingOutfitUtility.OutfitType.AtmoSuit)
			{
				throw new NotImplementedException();
			}
			this.atmosuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
		};
	}

	// Token: 0x0600675C RID: 26460 RVA: 0x00269677 File Offset: 0x00267877
	public void SetOutfitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		this.outfitBrowserScreen.state.CurrentOutfitType = outfitType;
	}

	// Token: 0x040045C3 RID: 17859
	[NonSerialized]
	public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton clothingOutfitTypeButton;

	// Token: 0x040045C4 RID: 17860
	[NonSerialized]
	public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton atmosuitOutfitTypeButton;

	// Token: 0x040045C5 RID: 17861
	[NonSerialized]
	public OutfitBrowserScreen outfitBrowserScreen;

	// Token: 0x040045C6 RID: 17862
	public KButton selectOutfitType_Prefab;

	// Token: 0x040045C7 RID: 17863
	public KInputTextField searchTextField;

	// Token: 0x02001E11 RID: 7697
	public enum SelectOutfitTypeButtonState
	{
		// Token: 0x04008931 RID: 35121
		Disabled,
		// Token: 0x04008932 RID: 35122
		Unselected,
		// Token: 0x04008933 RID: 35123
		Selected
	}

	// Token: 0x02001E12 RID: 7698
	public readonly struct SelectOutfitTypeButton
	{
		// Token: 0x0600AA77 RID: 43639 RVA: 0x003A1CD0 File Offset: 0x0039FED0
		public SelectOutfitTypeButton(OutfitBrowserScreen outfitBrowserScreen, GameObject rootGameObject)
		{
			this.outfitBrowserScreen = outfitBrowserScreen;
			this.root = rootGameObject.GetComponent<RectTransform>();
			this.button = rootGameObject.GetComponent<KButton>();
			this.buttonImage = rootGameObject.GetComponent<KImage>();
			this.icon = this.root.GetChild(0).GetComponent<Image>();
			global::Debug.Assert(this.root != null);
			global::Debug.Assert(this.button != null);
			global::Debug.Assert(this.buttonImage != null);
			global::Debug.Assert(this.icon != null);
		}

		// Token: 0x0600AA78 RID: 43640 RVA: 0x003A1D64 File Offset: 0x0039FF64
		public void SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState state)
		{
			switch (state)
			{
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Disabled:
				this.button.isInteractable = false;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.notSelectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected:
				this.button.isInteractable = true;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.notSelectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected:
				this.button.isInteractable = true;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.selectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x04008934 RID: 35124
		public readonly OutfitBrowserScreen outfitBrowserScreen;

		// Token: 0x04008935 RID: 35125
		public readonly RectTransform root;

		// Token: 0x04008936 RID: 35126
		public readonly KButton button;

		// Token: 0x04008937 RID: 35127
		public readonly KImage buttonImage;

		// Token: 0x04008938 RID: 35128
		public readonly Image icon;
	}
}
