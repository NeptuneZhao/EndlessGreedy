using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C45 RID: 3141
public class FacadeSelectionPanel : KMonoBehaviour
{
	// Token: 0x1700072F RID: 1839
	// (get) Token: 0x06006081 RID: 24705 RVA: 0x0023E836 File Offset: 0x0023CA36
	private int GridLayoutConstraintCount
	{
		get
		{
			if (this.gridLayout != null)
			{
				return this.gridLayout.constraintCount;
			}
			return 3;
		}
	}

	// Token: 0x17000730 RID: 1840
	// (get) Token: 0x06006083 RID: 24707 RVA: 0x0023E862 File Offset: 0x0023CA62
	// (set) Token: 0x06006082 RID: 24706 RVA: 0x0023E853 File Offset: 0x0023CA53
	public ClothingOutfitUtility.OutfitType SelectedOutfitCategory
	{
		get
		{
			return this.selectedOutfitCategory;
		}
		set
		{
			this.selectedOutfitCategory = value;
			this.Refresh();
		}
	}

	// Token: 0x17000731 RID: 1841
	// (get) Token: 0x06006084 RID: 24708 RVA: 0x0023E86A File Offset: 0x0023CA6A
	public string SelectedBuildingDefID
	{
		get
		{
			return this.selectedBuildingDefID;
		}
	}

	// Token: 0x17000732 RID: 1842
	// (get) Token: 0x06006085 RID: 24709 RVA: 0x0023E872 File Offset: 0x0023CA72
	// (set) Token: 0x06006086 RID: 24710 RVA: 0x0023E87C File Offset: 0x0023CA7C
	public string SelectedFacade
	{
		get
		{
			return this._selectedFacade;
		}
		set
		{
			if (this._selectedFacade != value)
			{
				this._selectedFacade = value;
				FacadeSelectionPanel.ConfigType configType = this.currentConfigType;
				if (configType != FacadeSelectionPanel.ConfigType.BuildingFacade)
				{
					if (configType == FacadeSelectionPanel.ConfigType.MinionOutfit)
					{
						this.RefreshTogglesForOutfit(this.selectedOutfitCategory);
					}
				}
				else
				{
					this.RefreshTogglesForBuilding();
				}
				if (this.OnFacadeSelectionChanged != null)
				{
					this.OnFacadeSelectionChanged();
				}
			}
		}
	}

	// Token: 0x06006087 RID: 24711 RVA: 0x0023E8D5 File Offset: 0x0023CAD5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.gridLayout = this.toggleContainer.GetComponent<GridLayoutGroup>();
	}

	// Token: 0x06006088 RID: 24712 RVA: 0x0023E8EE File Offset: 0x0023CAEE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.getMoreButton.ClearOnClick();
		this.getMoreButton.onClick += LockerMenuScreen.Instance.ShowInventoryScreen;
	}

	// Token: 0x06006089 RID: 24713 RVA: 0x0023E91C File Offset: 0x0023CB1C
	public void SetBuildingDef(string defID, string currentFacadeID = null)
	{
		this.currentConfigType = FacadeSelectionPanel.ConfigType.BuildingFacade;
		this.ClearToggles();
		this.selectedBuildingDefID = defID;
		this.SelectedFacade = ((currentFacadeID == null) ? "DEFAULT_FACADE" : currentFacadeID);
		this.RefreshTogglesForBuilding();
		if (this.hideWhenEmpty)
		{
			base.gameObject.SetActive(Assets.GetBuildingDef(defID).AvailableFacades.Count != 0);
		}
	}

	// Token: 0x0600608A RID: 24714 RVA: 0x0023E97A File Offset: 0x0023CB7A
	public void SetOutfitTarget(ClothingOutfitTarget outfitTarget, ClothingOutfitUtility.OutfitType outfitType)
	{
		this.currentConfigType = FacadeSelectionPanel.ConfigType.MinionOutfit;
		this.ClearToggles();
		this.SelectedFacade = outfitTarget.OutfitId;
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600608B RID: 24715 RVA: 0x0023E9A4 File Offset: 0x0023CBA4
	private void ClearToggles()
	{
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			this.pooledFacadeToggles.Add(keyValuePair.Value.gameObject);
			keyValuePair.Value.gameObject.SetActive(false);
		}
		this.activeFacadeToggles.Clear();
	}

	// Token: 0x0600608C RID: 24716 RVA: 0x0023EA2C File Offset: 0x0023CC2C
	public void Refresh()
	{
		FacadeSelectionPanel.ConfigType configType = this.currentConfigType;
		if (configType != FacadeSelectionPanel.ConfigType.BuildingFacade)
		{
			if (configType == FacadeSelectionPanel.ConfigType.MinionOutfit)
			{
				this.RefreshTogglesForOutfit(this.selectedOutfitCategory);
			}
		}
		else
		{
			this.RefreshTogglesForBuilding();
		}
		this.getMoreButton.gameObject.SetActive(this.showGetMoreButton);
		if (this.useDummyPlaceholder)
		{
			for (int i = 0; i < this.dummyGridPlaceholders.Count; i++)
			{
				this.dummyGridPlaceholders[i].SetActive(false);
			}
			int num = 0;
			for (int j = 0; j < this.toggleContainer.transform.childCount; j++)
			{
				if (this.toggleContainer.GetChild(j).gameObject.activeInHierarchy)
				{
					num++;
				}
			}
			this.getMoreButton.transform.SetAsLastSibling();
			if (num % this.GridLayoutConstraintCount != 0)
			{
				for (int k = 0; k < this.GridLayoutConstraintCount - 1; k++)
				{
					this.dummyGridPlaceholders[k].SetActive(k < this.GridLayoutConstraintCount - num % this.GridLayoutConstraintCount);
					this.dummyGridPlaceholders[k].transform.SetAsLastSibling();
				}
				return;
			}
		}
		else
		{
			this.getMoreButton.transform.SetAsLastSibling();
		}
	}

	// Token: 0x0600608D RID: 24717 RVA: 0x0023EB5C File Offset: 0x0023CD5C
	private void RefreshTogglesForOutfit(ClothingOutfitUtility.OutfitType outfitType)
	{
		IEnumerable<ClothingOutfitTarget> enumerable = from outfit in ClothingOutfitTarget.GetAllTemplates()
		where outfit.OutfitType == outfitType
		select outfit;
		List<string> list = new List<string>();
		using (Dictionary<string, FacadeSelectionPanel.FacadeToggle>.Enumerator enumerator = this.activeFacadeToggles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> toggle = enumerator.Current;
				if (!enumerable.Any((ClothingOutfitTarget match) => match.OutfitId == toggle.Key))
				{
					list.Add(toggle.Key);
				}
			}
		}
		foreach (string key in list)
		{
			this.pooledFacadeToggles.Add(this.activeFacadeToggles[key].gameObject);
			this.activeFacadeToggles[key].gameObject.SetActive(false);
			this.activeFacadeToggles.Remove(key);
		}
		list.Clear();
		this.AddDefaultOutfitToggle();
		enumerable = enumerable.StableSort((ClothingOutfitTarget a, ClothingOutfitTarget b) => a.OutfitId.CompareTo(b.OutfitId));
		foreach (ClothingOutfitTarget clothingOutfitTarget in enumerable)
		{
			if (!clothingOutfitTarget.DoesContainLockedItems())
			{
				this.AddNewOutfitToggle(clothingOutfitTarget.OutfitId, false);
			}
		}
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			keyValuePair.Value.multiToggle.ChangeState((this.SelectedFacade != null && this.SelectedFacade == keyValuePair.Key) ? 1 : 0);
		}
		this.RefreshHeight();
	}

	// Token: 0x0600608E RID: 24718 RVA: 0x0023ED80 File Offset: 0x0023CF80
	private void RefreshTogglesForBuilding()
	{
		BuildingDef buildingDef = Assets.GetBuildingDef(this.selectedBuildingDefID);
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			if (!buildingDef.AvailableFacades.Contains(keyValuePair.Key))
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (string key in list)
		{
			this.pooledFacadeToggles.Add(this.activeFacadeToggles[key].gameObject);
			this.activeFacadeToggles[key].gameObject.SetActive(false);
			this.activeFacadeToggles.Remove(key);
		}
		list.Clear();
		this.AddDefaultBuildingFacadeToggle();
		foreach (string text in buildingDef.AvailableFacades)
		{
			PermitResource permitResource = Db.Get().Permits.TryGet(text);
			if (permitResource != null && permitResource.IsUnlocked())
			{
				this.AddNewBuildingToggle(text);
			}
		}
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair2 in this.activeFacadeToggles)
		{
			keyValuePair2.Value.multiToggle.ChangeState((this.SelectedFacade == keyValuePair2.Key) ? 1 : 0);
		}
		this.activeFacadeToggles["DEFAULT_FACADE"].gameObject.transform.SetAsFirstSibling();
		this.RefreshHeight();
	}

	// Token: 0x0600608F RID: 24719 RVA: 0x0023EF88 File Offset: 0x0023D188
	private void RefreshHeight()
	{
		if (this.usesScrollRect)
		{
			LayoutElement component = this.scrollRect.GetComponent<LayoutElement>();
			component.minHeight = (float)(58 * ((this.activeFacadeToggles.Count <= 5) ? 1 : 2));
			component.preferredHeight = component.minHeight;
		}
	}

	// Token: 0x06006090 RID: 24720 RVA: 0x0023EFC4 File Offset: 0x0023D1C4
	private void AddDefaultBuildingFacadeToggle()
	{
		this.AddNewBuildingToggle("DEFAULT_FACADE");
	}

	// Token: 0x06006091 RID: 24721 RVA: 0x0023EFD1 File Offset: 0x0023D1D1
	private void AddDefaultOutfitToggle()
	{
		this.AddNewOutfitToggle("DEFAULT_FACADE", true);
	}

	// Token: 0x06006092 RID: 24722 RVA: 0x0023EFE0 File Offset: 0x0023D1E0
	private void AddNewBuildingToggle(string facadeID)
	{
		if (this.activeFacadeToggles.ContainsKey(facadeID))
		{
			return;
		}
		GameObject gameObject;
		if (this.pooledFacadeToggles.Count > 0)
		{
			gameObject = this.pooledFacadeToggles[0];
			this.pooledFacadeToggles.RemoveAt(0);
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.togglePrefab, this.toggleContainer.gameObject, false);
		}
		FacadeSelectionPanel.FacadeToggle newToggle = new FacadeSelectionPanel.FacadeToggle(facadeID, this.selectedBuildingDefID, gameObject);
		MultiToggle multiToggle = newToggle.multiToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SelectFacade(newToggle.id);
		}));
		this.activeFacadeToggles.Add(newToggle.id, newToggle);
	}

	// Token: 0x06006093 RID: 24723 RVA: 0x0023F0A8 File Offset: 0x0023D2A8
	private void AddNewOutfitToggle(string outfitID, bool setAsFirstSibling = false)
	{
		if (this.activeFacadeToggles.ContainsKey(outfitID))
		{
			if (setAsFirstSibling)
			{
				this.activeFacadeToggles[outfitID].gameObject.transform.SetAsFirstSibling();
			}
			return;
		}
		GameObject gameObject;
		if (this.pooledFacadeToggles.Count > 0)
		{
			gameObject = this.pooledFacadeToggles[0];
			this.pooledFacadeToggles.RemoveAt(0);
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.togglePrefab, this.toggleContainer.gameObject, false);
		}
		FacadeSelectionPanel.FacadeToggle newToggle = new FacadeSelectionPanel.FacadeToggle(outfitID, gameObject, this.selectedOutfitCategory);
		MultiToggle multiToggle = newToggle.multiToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SelectFacade(newToggle.id);
		}));
		this.activeFacadeToggles.Add(newToggle.id, newToggle);
		if (setAsFirstSibling)
		{
			this.activeFacadeToggles[outfitID].gameObject.transform.SetAsFirstSibling();
		}
	}

	// Token: 0x06006094 RID: 24724 RVA: 0x0023F1B1 File Offset: 0x0023D3B1
	private void SelectFacade(string id)
	{
		this.SelectedFacade = id;
	}

	// Token: 0x04004138 RID: 16696
	[SerializeField]
	private GameObject togglePrefab;

	// Token: 0x04004139 RID: 16697
	[SerializeField]
	private RectTransform toggleContainer;

	// Token: 0x0400413A RID: 16698
	[SerializeField]
	private bool usesScrollRect;

	// Token: 0x0400413B RID: 16699
	[SerializeField]
	private LayoutElement scrollRect;

	// Token: 0x0400413C RID: 16700
	private Dictionary<string, FacadeSelectionPanel.FacadeToggle> activeFacadeToggles = new Dictionary<string, FacadeSelectionPanel.FacadeToggle>();

	// Token: 0x0400413D RID: 16701
	private List<GameObject> pooledFacadeToggles = new List<GameObject>();

	// Token: 0x0400413E RID: 16702
	[SerializeField]
	private KButton getMoreButton;

	// Token: 0x0400413F RID: 16703
	[SerializeField]
	private bool showGetMoreButton;

	// Token: 0x04004140 RID: 16704
	[SerializeField]
	private bool hideWhenEmpty = true;

	// Token: 0x04004141 RID: 16705
	[SerializeField]
	private bool useDummyPlaceholder;

	// Token: 0x04004142 RID: 16706
	private GridLayoutGroup gridLayout;

	// Token: 0x04004143 RID: 16707
	[SerializeField]
	private List<GameObject> dummyGridPlaceholders;

	// Token: 0x04004144 RID: 16708
	public System.Action OnFacadeSelectionChanged;

	// Token: 0x04004145 RID: 16709
	private ClothingOutfitUtility.OutfitType selectedOutfitCategory;

	// Token: 0x04004146 RID: 16710
	private string selectedBuildingDefID;

	// Token: 0x04004147 RID: 16711
	private FacadeSelectionPanel.ConfigType currentConfigType;

	// Token: 0x04004148 RID: 16712
	private string _selectedFacade;

	// Token: 0x04004149 RID: 16713
	public const string DEFAULT_FACADE_ID = "DEFAULT_FACADE";

	// Token: 0x02001D2A RID: 7466
	private struct FacadeToggle
	{
		// Token: 0x0600A7E6 RID: 42982 RVA: 0x0039B6E0 File Offset: 0x003998E0
		public FacadeToggle(string buildingFacadeID, string buildingPrefabID, GameObject gameObject)
		{
			this.id = buildingFacadeID;
			this.gameObject = gameObject;
			gameObject.SetActive(true);
			this.multiToggle = gameObject.GetComponent<MultiToggle>();
			this.multiToggle.onClick = null;
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<UIMannequin>("Mannequin").gameObject.SetActive(false);
			component.GetReference<Image>("FGImage").SetAlpha(1f);
			Sprite sprite;
			string simpleTooltip;
			string dlcId;
			if (buildingFacadeID != "DEFAULT_FACADE")
			{
				BuildingFacadeResource buildingFacadeResource = Db.GetBuildingFacades().Get(buildingFacadeID);
				sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(buildingFacadeResource.AnimFile), "ui", false, "");
				simpleTooltip = KleiItemsUI.GetTooltipStringFor(buildingFacadeResource);
				dlcId = buildingFacadeResource.GetDlcIdFrom();
			}
			else
			{
				GameObject prefab = Assets.GetPrefab(buildingPrefabID);
				Building component2 = prefab.GetComponent<Building>();
				StringEntry entry;
				string text;
				if (Strings.TryGet(string.Concat(new string[]
				{
					"STRINGS.BUILDINGS.PREFABS.",
					buildingPrefabID.ToUpperInvariant(),
					".FACADES.DEFAULT_",
					buildingPrefabID.ToUpperInvariant(),
					".NAME"
				}), out entry))
				{
					text = entry;
				}
				else if (component2 != null)
				{
					text = component2.Def.Name;
				}
				else
				{
					text = prefab.GetProperName();
				}
				StringEntry entry2;
				string str;
				if (Strings.TryGet(string.Concat(new string[]
				{
					"STRINGS.BUILDINGS.PREFABS.",
					buildingPrefabID.ToUpperInvariant(),
					".FACADES.DEFAULT_",
					buildingPrefabID.ToUpperInvariant(),
					".DESC"
				}), out entry2))
				{
					str = entry2;
				}
				else if (component2 != null)
				{
					str = component2.Def.Desc;
				}
				else
				{
					str = "";
				}
				sprite = Def.GetUISprite(buildingPrefabID, "ui", false).first;
				simpleTooltip = KleiItemsUI.WrapAsToolTipTitle(text) + "\n" + str;
				dlcId = null;
			}
			component.GetReference<Image>("FGImage").sprite = sprite;
			this.gameObject.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
			Image reference = component.GetReference<Image>("DlcBanner");
			if (DlcManager.IsDlcId(dlcId))
			{
				reference.gameObject.SetActive(true);
				reference.color = DlcManager.GetDlcBannerColor(dlcId);
				return;
			}
			reference.gameObject.SetActive(false);
		}

		// Token: 0x0600A7E7 RID: 42983 RVA: 0x0039B90C File Offset: 0x00399B0C
		public FacadeToggle(string outfitID, GameObject gameObject, ClothingOutfitUtility.OutfitType outfitType)
		{
			this.id = outfitID;
			this.gameObject = gameObject;
			gameObject.SetActive(true);
			this.multiToggle = gameObject.GetComponent<MultiToggle>();
			this.multiToggle.onClick = null;
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			UIMannequin reference = component.GetReference<UIMannequin>("Mannequin");
			reference.gameObject.SetActive(true);
			component.GetReference<Image>("FGImage").SetAlpha(0f);
			ToolTip component2 = this.gameObject.GetComponent<ToolTip>();
			component2.SetSimpleTooltip("");
			if (outfitID != "DEFAULT_FACADE")
			{
				ClothingOutfitTarget outfit = ClothingOutfitTarget.FromTemplateId(outfitID);
				component.GetReference<UIMannequin>("Mannequin").SetOutfit(outfit);
				component2.SetSimpleTooltip(GameUtil.ApplyBoldString(outfit.ReadName()));
			}
			else
			{
				component.GetReference<UIMannequin>("Mannequin").ClearOutfit(outfitType);
				component2.SetSimpleTooltip(GameUtil.ApplyBoldString(UI.OUTFIT_NAME.NONE));
			}
			string dlcId = null;
			if (outfitID != "DEFAULT_FACADE")
			{
				ClothingOutfitTarget.Implementation impl = ClothingOutfitTarget.FromTemplateId(outfitID).impl;
				if (impl is ClothingOutfitTarget.DatabaseAuthoredTemplate)
				{
					ClothingOutfitTarget.DatabaseAuthoredTemplate databaseAuthoredTemplate = (ClothingOutfitTarget.DatabaseAuthoredTemplate)impl;
					dlcId = databaseAuthoredTemplate.resource.GetDlcIdFrom();
				}
			}
			Image reference2 = component.GetReference<Image>("DlcBanner");
			if (DlcManager.IsDlcId(dlcId))
			{
				reference2.gameObject.SetActive(true);
				reference2.color = DlcManager.GetDlcBannerColor(dlcId);
			}
			else
			{
				reference2.gameObject.SetActive(false);
			}
			Vector2 sizeDelta = new Vector2(0f, 0f);
			if (outfitType == ClothingOutfitUtility.OutfitType.AtmoSuit)
			{
				sizeDelta = new Vector2(-16f, -16f);
			}
			reference.rectTransform().sizeDelta = sizeDelta;
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x0600A7E8 RID: 42984 RVA: 0x0039BA9E File Offset: 0x00399C9E
		// (set) Token: 0x0600A7E9 RID: 42985 RVA: 0x0039BAA6 File Offset: 0x00399CA6
		public string id { readonly get; set; }

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x0600A7EA RID: 42986 RVA: 0x0039BAAF File Offset: 0x00399CAF
		// (set) Token: 0x0600A7EB RID: 42987 RVA: 0x0039BAB7 File Offset: 0x00399CB7
		public GameObject gameObject { readonly get; set; }

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x0600A7EC RID: 42988 RVA: 0x0039BAC0 File Offset: 0x00399CC0
		// (set) Token: 0x0600A7ED RID: 42989 RVA: 0x0039BAC8 File Offset: 0x00399CC8
		public MultiToggle multiToggle { readonly get; set; }
	}

	// Token: 0x02001D2B RID: 7467
	private enum ConfigType
	{
		// Token: 0x04008642 RID: 34370
		BuildingFacade,
		// Token: 0x04008643 RID: 34371
		MinionOutfit
	}
}
