using System;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000D0E RID: 3342
public class PlanScreen : KIconToggleMenu
{
	// Token: 0x17000767 RID: 1895
	// (get) Token: 0x06006811 RID: 26641 RVA: 0x0026E78C File Offset: 0x0026C98C
	// (set) Token: 0x06006812 RID: 26642 RVA: 0x0026E793 File Offset: 0x0026C993
	public static PlanScreen Instance { get; private set; }

	// Token: 0x06006813 RID: 26643 RVA: 0x0026E79B File Offset: 0x0026C99B
	public static void DestroyInstance()
	{
		PlanScreen.Instance = null;
	}

	// Token: 0x17000768 RID: 1896
	// (get) Token: 0x06006814 RID: 26644 RVA: 0x0026E7A3 File Offset: 0x0026C9A3
	public static Dictionary<HashedString, string> IconNameMap
	{
		get
		{
			return PlanScreen.iconNameMap;
		}
	}

	// Token: 0x06006815 RID: 26645 RVA: 0x0026E7AA File Offset: 0x0026C9AA
	private static HashedString CacheHashedString(string str)
	{
		return HashCache.Get().Add(str);
	}

	// Token: 0x17000769 RID: 1897
	// (get) Token: 0x06006816 RID: 26646 RVA: 0x0026E7B7 File Offset: 0x0026C9B7
	// (set) Token: 0x06006817 RID: 26647 RVA: 0x0026E7BF File Offset: 0x0026C9BF
	public ProductInfoScreen ProductInfoScreen { get; private set; }

	// Token: 0x1700076A RID: 1898
	// (get) Token: 0x06006818 RID: 26648 RVA: 0x0026E7C8 File Offset: 0x0026C9C8
	public KIconToggleMenu.ToggleInfo ActiveCategoryToggleInfo
	{
		get
		{
			return this.activeCategoryInfo;
		}
	}

	// Token: 0x1700076B RID: 1899
	// (get) Token: 0x06006819 RID: 26649 RVA: 0x0026E7D0 File Offset: 0x0026C9D0
	// (set) Token: 0x0600681A RID: 26650 RVA: 0x0026E7D8 File Offset: 0x0026C9D8
	public GameObject SelectedBuildingGameObject { get; private set; }

	// Token: 0x0600681B RID: 26651 RVA: 0x0026E7E1 File Offset: 0x0026C9E1
	public override float GetSortKey()
	{
		return 2f;
	}

	// Token: 0x0600681C RID: 26652 RVA: 0x0026E7E8 File Offset: 0x0026C9E8
	public PlanScreen.RequirementsState GetBuildableState(BuildingDef def)
	{
		if (def == null)
		{
			return PlanScreen.RequirementsState.Materials;
		}
		return this._buildableStatesByID[def.PrefabID];
	}

	// Token: 0x0600681D RID: 26653 RVA: 0x0026E808 File Offset: 0x0026CA08
	private bool IsDefResearched(BuildingDef def)
	{
		bool result = false;
		if (!this._researchedDefs.TryGetValue(def, out result))
		{
			result = this.UpdateDefResearched(def);
		}
		return result;
	}

	// Token: 0x0600681E RID: 26654 RVA: 0x0026E830 File Offset: 0x0026CA30
	private bool UpdateDefResearched(BuildingDef def)
	{
		return this._researchedDefs[def] = Db.Get().TechItems.IsTechItemComplete(def.PrefabID);
	}

	// Token: 0x0600681F RID: 26655 RVA: 0x0026E864 File Offset: 0x0026CA64
	protected override void OnPrefabInit()
	{
		if (BuildMenu.UseHotkeyBuildMenu())
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.OnPrefabInit();
			PlanScreen.Instance = this;
			this.ProductInfoScreen = global::Util.KInstantiateUI<ProductInfoScreen>(this.productInfoScreenPrefab, this.recipeInfoScreenParent, false);
			this.ProductInfoScreen.rectTransform().pivot = new Vector2(0f, 0f);
			this.ProductInfoScreen.rectTransform().SetLocalPosition(new Vector3(326f, 0f, 0f));
			this.ProductInfoScreen.onElementsFullySelected = new System.Action(this.OnRecipeElementsFullySelected);
			KInputManager.InputChange.AddListener(new UnityAction(this.RefreshToolTip));
			this.planScreenScrollRect = base.transform.parent.GetComponentInParent<KScrollRect>();
			Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
			Game.Instance.Subscribe(1174281782, new Action<object>(this.OnActiveToolChanged));
			Game.Instance.Subscribe(1557339983, new Action<object>(this.ForceUpdateAllCategoryToggles));
		}
		this.buildingGroupsRoot.gameObject.SetActive(false);
	}

	// Token: 0x06006820 RID: 26656 RVA: 0x0026E99C File Offset: 0x0026CB9C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		this.useSubCategoryLayout = (KPlayerPrefs.GetInt("usePlanScreenListView") == 1);
		this.initTime = KTime.Instance.UnscaledGameTime;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			this._buildableStatesByID.Add(buildingDef.PrefabID, PlanScreen.RequirementsState.Materials);
		}
		if (BuildMenu.UseHotkeyBuildMenu())
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.onSelect += this.OnClickCategory;
			this.Refresh();
			foreach (KToggle ktoggle in this.toggles)
			{
				ktoggle.group = base.GetComponent<ToggleGroup>();
			}
			this.RefreshBuildableStates(true);
			Game.Instance.Subscribe(288942073, new Action<object>(this.OnUIClear));
		}
		this.copyBuildingButton.GetComponent<MultiToggle>().onClick = delegate()
		{
			this.OnClickCopyBuilding();
		};
		this.RefreshCopyBuildingButton(null);
		Game.Instance.Subscribe(-1503271301, new Action<object>(this.RefreshCopyBuildingButton));
		Game.Instance.Subscribe(1983128072, delegate(object data)
		{
			this.CloseRecipe(false);
		});
		this.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(this.pointerEnterActions, new KScreen.PointerEnterActions(this.PointerEnter));
		this.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(this.pointerExitActions, new KScreen.PointerExitActions(this.PointerExit));
		this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.COPY_BUILDING_TOOLTIP, global::Action.CopyBuilding));
		this.RefreshScale(null);
		this.refreshScaleHandle = Game.Instance.Subscribe(-442024484, new Action<object>(this.RefreshScale));
		this.BuildButtonList();
		this.gridViewButton.onClick += this.OnClickGridView;
		this.listViewButton.onClick += this.OnClickListView;
	}

	// Token: 0x06006821 RID: 26657 RVA: 0x0026EBE0 File Offset: 0x0026CDE0
	private void RefreshScale(object data = null)
	{
		base.GetComponent<GridLayoutGroup>().cellSize = (ScreenResolutionMonitor.UsingGamepadUIMode() ? new Vector2(54f, 50f) : new Vector2(45f, 45f));
		this.toggles.ForEach(delegate(KToggle to)
		{
			to.GetComponentInChildren<LocText>().fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
		});
		LayoutElement component = this.copyBuildingButton.GetComponent<LayoutElement>();
		component.minWidth = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? 58 : 54);
		component.minHeight = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? 58 : 54);
		base.gameObject.rectTransform().anchoredPosition = new Vector2(0f, (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? -68 : -74));
		this.adjacentPinnedButtons.GetComponent<HorizontalLayoutGroup>().padding.bottom = (ScreenResolutionMonitor.UsingGamepadUIMode() ? 14 : 6);
		Vector2 sizeDelta = this.buildingGroupsRoot.rectTransform().sizeDelta;
		Vector2 vector = ScreenResolutionMonitor.UsingGamepadUIMode() ? new Vector2(320f, sizeDelta.y) : new Vector2(264f, sizeDelta.y);
		this.buildingGroupsRoot.rectTransform().sizeDelta = vector;
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
			GridLayoutGroup componentInChildren = keyValuePair.Value.GetComponentInChildren<GridLayoutGroup>(true);
			if (this.useSubCategoryLayout)
			{
				componentInChildren.constraintCount = 1;
				componentInChildren.cellSize = new Vector2(vector.x - 24f, 36f);
			}
			else
			{
				componentInChildren.constraintCount = 3;
				componentInChildren.cellSize = (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.bigBuildingButtonSize : PlanScreen.standarduildingButtonSize);
			}
		}
		this.ProductInfoScreen.rectTransform().anchoredPosition = new Vector2(vector.x + 8f, this.ProductInfoScreen.rectTransform().anchoredPosition.y);
	}

	// Token: 0x06006822 RID: 26658 RVA: 0x0026EDE8 File Offset: 0x0026CFE8
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.RefreshToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x06006823 RID: 26659 RVA: 0x0026EE06 File Offset: 0x0026D006
	protected override void OnCleanUp()
	{
		if (Game.Instance != null)
		{
			Game.Instance.Unsubscribe(this.refreshScaleHandle);
		}
		base.OnCleanUp();
	}

	// Token: 0x06006824 RID: 26660 RVA: 0x0026EE2C File Offset: 0x0026D02C
	private void OnClickCopyBuilding()
	{
		if (!this.LastSelectedBuilding.IsNullOrDestroyed() && this.LastSelectedBuilding.gameObject.activeInHierarchy && (!this.lastSelectedBuilding.Def.DebugOnly || DebugHandler.InstantBuildMode))
		{
			PlanScreen.Instance.CopyBuildingOrder(this.LastSelectedBuilding);
			return;
		}
		if (this.lastSelectedBuildingDef != null && (!this.lastSelectedBuildingDef.DebugOnly || DebugHandler.InstantBuildMode))
		{
			PlanScreen.Instance.CopyBuildingOrder(this.lastSelectedBuildingDef, this.LastSelectedBuildingFacade);
		}
	}

	// Token: 0x06006825 RID: 26661 RVA: 0x0026EEBA File Offset: 0x0026D0BA
	private void OnClickListView()
	{
		this.useSubCategoryLayout = true;
		this.ForceRefreshAllBuildingToggles();
		this.BuildButtonList();
		this.ConfigurePanelSize(null);
		this.RefreshScale(null);
		KPlayerPrefs.SetInt("usePlanScreenListView", 1);
	}

	// Token: 0x06006826 RID: 26662 RVA: 0x0026EEE8 File Offset: 0x0026D0E8
	private void OnClickGridView()
	{
		this.useSubCategoryLayout = false;
		this.ForceRefreshAllBuildingToggles();
		this.BuildButtonList();
		this.ConfigurePanelSize(null);
		this.RefreshScale(null);
		KPlayerPrefs.SetInt("usePlanScreenListView", 0);
	}

	// Token: 0x1700076C RID: 1900
	// (get) Token: 0x06006827 RID: 26663 RVA: 0x0026EF16 File Offset: 0x0026D116
	// (set) Token: 0x06006828 RID: 26664 RVA: 0x0026EF20 File Offset: 0x0026D120
	private Building LastSelectedBuilding
	{
		get
		{
			return this.lastSelectedBuilding;
		}
		set
		{
			this.lastSelectedBuilding = value;
			if (this.lastSelectedBuilding != null)
			{
				this.lastSelectedBuildingDef = this.lastSelectedBuilding.Def;
				if (this.lastSelectedBuilding.gameObject.activeInHierarchy)
				{
					this.LastSelectedBuildingFacade = this.lastSelectedBuilding.GetComponent<BuildingFacade>().CurrentFacade;
				}
			}
		}
	}

	// Token: 0x1700076D RID: 1901
	// (get) Token: 0x06006829 RID: 26665 RVA: 0x0026EF7B File Offset: 0x0026D17B
	// (set) Token: 0x0600682A RID: 26666 RVA: 0x0026EF83 File Offset: 0x0026D183
	public string LastSelectedBuildingFacade
	{
		get
		{
			return this.lastSelectedBuildingFacade;
		}
		set
		{
			this.lastSelectedBuildingFacade = value;
		}
	}

	// Token: 0x0600682B RID: 26667 RVA: 0x0026EF8C File Offset: 0x0026D18C
	public void RefreshCopyBuildingButton(object data = null)
	{
		this.adjacentPinnedButtons.rectTransform().anchoredPosition = new Vector2(Mathf.Min(base.gameObject.rectTransform().sizeDelta.x, base.transform.parent.rectTransform().rect.width), 0f);
		MultiToggle component = this.copyBuildingButton.GetComponent<MultiToggle>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null)
		{
			Building component2 = SelectTool.Instance.selected.GetComponent<Building>();
			if (component2 != null && component2.Def.ShouldShowInBuildMenu() && component2.Def.IsAvailable())
			{
				this.LastSelectedBuilding = component2;
			}
		}
		if (this.lastSelectedBuildingDef != null)
		{
			component.gameObject.SetActive(PlanScreen.Instance.gameObject.activeInHierarchy);
			Sprite sprite = this.lastSelectedBuildingDef.GetUISprite("ui", false);
			if (this.LastSelectedBuildingFacade != null && this.LastSelectedBuildingFacade != "DEFAULT_FACADE" && Db.Get().Permits.BuildingFacades.TryGet(this.LastSelectedBuildingFacade) != null)
			{
				sprite = Def.GetFacadeUISprite(this.LastSelectedBuildingFacade);
			}
			component.transform.Find("FG").GetComponent<Image>().sprite = sprite;
			component.transform.Find("FG").GetComponent<Image>().color = Color.white;
			component.ChangeState(1);
			return;
		}
		component.gameObject.SetActive(false);
		component.ChangeState(0);
	}

	// Token: 0x0600682C RID: 26668 RVA: 0x0026F124 File Offset: 0x0026D324
	public void RefreshToolTip()
	{
		for (int i = 0; i < TUNING.BUILDINGS.PLANORDER.Count; i++)
		{
			PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER[i];
			if (SaveLoader.Instance.IsDLCActiveForCurrentSave(planInfo.RequiredDlcId))
			{
				global::Action action = (i < 14) ? (global::Action.Plan1 + i) : global::Action.NumActions;
				string str = HashCache.Get().Get(planInfo.category).ToUpper();
				this.toggleInfo[i].tooltip = GameUtil.ReplaceHotkeyString(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".TOOLTIP"), action);
			}
		}
		this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.COPY_BUILDING_TOOLTIP, global::Action.CopyBuilding));
	}

	// Token: 0x0600682D RID: 26669 RVA: 0x0026F1E0 File Offset: 0x0026D3E0
	public void Refresh()
	{
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		if (this.tagCategoryMap == null)
		{
			int num = 0;
			this.tagCategoryMap = new Dictionary<Tag, HashedString>();
			this.tagOrderMap = new Dictionary<Tag, int>();
			if (TUNING.BUILDINGS.PLANORDER.Count > 15)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Insufficient keys to cover root plan menu",
					"Max of 14 keys supported but TUNING.BUILDINGS.PLANORDER has " + TUNING.BUILDINGS.PLANORDER.Count.ToString()
				});
			}
			this.toggleEntries.Clear();
			for (int i = 0; i < TUNING.BUILDINGS.PLANORDER.Count; i++)
			{
				PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER[i];
				if (SaveLoader.Instance.IsDLCActiveForCurrentSave(planInfo.RequiredDlcId))
				{
					global::Action action = (i < 15) ? (global::Action.Plan1 + i) : global::Action.NumActions;
					string icon = PlanScreen.iconNameMap[planInfo.category];
					string str = HashCache.Get().Get(planInfo.category).ToUpper();
					KIconToggleMenu.ToggleInfo toggleInfo = new KIconToggleMenu.ToggleInfo(UI.StripLinkFormatting(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".NAME")), icon, planInfo.category, action, GameUtil.ReplaceHotkeyString(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".TOOLTIP"), action), "");
					list.Add(toggleInfo);
					PlanScreen.PopulateOrderInfo(planInfo.category, planInfo.buildingAndSubcategoryData, this.tagCategoryMap, this.tagOrderMap, ref num);
					List<BuildingDef> list2 = new List<BuildingDef>();
					foreach (BuildingDef buildingDef in Assets.BuildingDefs)
					{
						HashedString x;
						if (buildingDef.IsAvailable() && this.tagCategoryMap.TryGetValue(buildingDef.Tag, out x) && !(x != planInfo.category))
						{
							list2.Add(buildingDef);
						}
					}
					this.toggleEntries.Add(new PlanScreen.ToggleEntry(toggleInfo, planInfo.category, list2, planInfo.hideIfNotResearched));
				}
			}
			base.Setup(list);
			this.toggleBouncers.Clear();
			this.toggles.ForEach(delegate(KToggle to)
			{
				foreach (ImageToggleState imageToggleState in to.GetComponents<ImageToggleState>())
				{
					if (imageToggleState.TargetImage.sprite != null && imageToggleState.TargetImage.name == "FG" && !imageToggleState.useSprites)
					{
						imageToggleState.SetSprites(Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"), imageToggleState.TargetImage.sprite, imageToggleState.TargetImage.sprite, Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"));
					}
				}
				to.GetComponent<KToggle>().soundPlayer.Enabled = false;
				to.GetComponentInChildren<LocText>().fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
				this.toggleBouncers.Add(to, to.GetComponent<Bouncer>());
			});
			for (int j = 0; j < this.toggleEntries.Count; j++)
			{
				PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[j];
				toggleEntry.CollectToggleImages();
				this.toggleEntries[j] = toggleEntry;
			}
			this.ForceUpdateAllCategoryToggles(null);
		}
	}

	// Token: 0x0600682E RID: 26670 RVA: 0x0026F474 File Offset: 0x0026D674
	private void ForceUpdateAllCategoryToggles(object data = null)
	{
		this.forceUpdateAllCategoryToggles = true;
	}

	// Token: 0x0600682F RID: 26671 RVA: 0x0026F47D File Offset: 0x0026D67D
	public void ForceRefreshAllBuildingToggles()
	{
		this.forceRefreshAllBuildings = true;
	}

	// Token: 0x06006830 RID: 26672 RVA: 0x0026F488 File Offset: 0x0026D688
	public void CopyBuildingOrder(BuildingDef buildingDef, string facadeID)
	{
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				if (buildingDef.PrefabID == keyValuePair.Key)
				{
					this.OpenCategoryByName(HashCache.Get().Get(planInfo.category));
					this.OnSelectBuilding(this.activeCategoryBuildingToggles[buildingDef].gameObject, buildingDef, facadeID);
					this.ProductInfoScreen.ToggleExpandedInfo(true);
					break;
				}
			}
		}
	}

	// Token: 0x06006831 RID: 26673 RVA: 0x0026F568 File Offset: 0x0026D768
	public void CopyBuildingOrder(Building building)
	{
		this.CopyBuildingOrder(building.Def, building.GetComponent<BuildingFacade>().CurrentFacade);
		if (this.ProductInfoScreen.materialSelectionPanel == null)
		{
			DebugUtil.DevLogError(building.Def.name + " def likely needs to be marked def.ShowInBuildMenu = false");
			return;
		}
		this.ProductInfoScreen.materialSelectionPanel.SelectSourcesMaterials(building);
		Rotatable component = building.GetComponent<Rotatable>();
		if (component != null)
		{
			BuildTool.Instance.SetToolOrientation(component.GetOrientation());
		}
	}

	// Token: 0x06006832 RID: 26674 RVA: 0x0026F5EC File Offset: 0x0026D7EC
	private static void PopulateOrderInfo(HashedString category, object data, Dictionary<Tag, HashedString> category_map, Dictionary<Tag, int> order_map, ref int building_index)
	{
		if (data.GetType() == typeof(PlanScreen.PlanInfo))
		{
			PlanScreen.PlanInfo planInfo = (PlanScreen.PlanInfo)data;
			PlanScreen.PopulateOrderInfo(planInfo.category, planInfo.buildingAndSubcategoryData, category_map, order_map, ref building_index);
			return;
		}
		foreach (KeyValuePair<string, string> keyValuePair in ((List<KeyValuePair<string, string>>)data))
		{
			Tag key = new Tag(keyValuePair.Key);
			category_map[key] = category;
			order_map[key] = building_index;
			building_index++;
		}
	}

	// Token: 0x06006833 RID: 26675 RVA: 0x0026F694 File Offset: 0x0026D894
	protected override void OnCmpEnable()
	{
		this.Refresh();
		this.RefreshCopyBuildingButton(null);
	}

	// Token: 0x06006834 RID: 26676 RVA: 0x0026F6A3 File Offset: 0x0026D8A3
	protected override void OnCmpDisable()
	{
		this.ClearButtons();
	}

	// Token: 0x06006835 RID: 26677 RVA: 0x0026F6AC File Offset: 0x0026D8AC
	private void ClearButtons()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
		}
		foreach (KeyValuePair<string, PlanBuildingToggle> keyValuePair2 in this.allBuildingToggles)
		{
			keyValuePair2.Value.gameObject.SetActive(false);
		}
		this.activeCategoryBuildingToggles.Clear();
		this.copyBuildingButton.gameObject.SetActive(false);
		this.copyBuildingButton.GetComponent<MultiToggle>().ChangeState(0);
	}

	// Token: 0x06006836 RID: 26678 RVA: 0x0026F774 File Offset: 0x0026D974
	public void OnSelectBuilding(GameObject button_go, BuildingDef def, string facadeID = null)
	{
		if (button_go == null)
		{
			global::Debug.Log("Button gameObject is null", base.gameObject);
			return;
		}
		if (button_go == this.SelectedBuildingGameObject)
		{
			this.CloseRecipe(true);
			return;
		}
		this.ignoreToolChangeMessages++;
		PlanBuildingToggle planBuildingToggle = null;
		if (this.currentlySelectedToggle != null)
		{
			planBuildingToggle = this.currentlySelectedToggle.GetComponent<PlanBuildingToggle>();
		}
		this.SelectedBuildingGameObject = button_go;
		this.currentlySelectedToggle = button_go.GetComponent<KToggle>();
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		HashedString category = this.tagCategoryMap[def.Tag];
		PlanScreen.ToggleEntry toggleEntry;
		if (this.GetToggleEntryForCategory(category, out toggleEntry) && toggleEntry.pendingResearchAttentions.Contains(def.Tag))
		{
			toggleEntry.pendingResearchAttentions.Remove(def.Tag);
			button_go.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
			if (toggleEntry.pendingResearchAttentions.Count == 0)
			{
				toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
			}
		}
		this.ProductInfoScreen.ClearProduct(false);
		if (planBuildingToggle != null)
		{
			planBuildingToggle.Refresh();
		}
		ToolMenu.Instance.ClearSelection();
		PrebuildTool.Instance.Activate(def, this.GetTooltipForBuildable(def));
		this.LastSelectedBuilding = def.BuildingComplete.GetComponent<Building>();
		this.RefreshCopyBuildingButton(null);
		this.ProductInfoScreen.Show(true);
		this.ProductInfoScreen.ConfigureScreen(def, facadeID);
		this.ignoreToolChangeMessages--;
	}

	// Token: 0x06006837 RID: 26679 RVA: 0x0026F8E8 File Offset: 0x0026DAE8
	private void RefreshBuildableStates(bool force_update)
	{
		if (Assets.BuildingDefs == null || Assets.BuildingDefs.Count == 0)
		{
			return;
		}
		if (this.timeSinceNotificationPing < this.specialNotificationEmbellishDelay)
		{
			this.timeSinceNotificationPing += Time.unscaledDeltaTime;
		}
		if (this.timeSinceNotificationPing >= this.notificationPingExpire)
		{
			this.notificationPingCount = 0;
		}
		int num = 10;
		if (force_update)
		{
			num = Assets.BuildingDefs.Count;
			this.buildable_state_update_idx = 0;
		}
		ListPool<HashedString, PlanScreen>.PooledList pooledList = ListPool<HashedString, PlanScreen>.Allocate();
		for (int i = 0; i < num; i++)
		{
			this.buildable_state_update_idx = (this.buildable_state_update_idx + 1) % Assets.BuildingDefs.Count;
			BuildingDef buildingDef = Assets.BuildingDefs[this.buildable_state_update_idx];
			PlanScreen.RequirementsState buildableStateForDef = this.GetBuildableStateForDef(buildingDef);
			HashedString hashedString;
			if (this.tagCategoryMap.TryGetValue(buildingDef.Tag, out hashedString) && this._buildableStatesByID[buildingDef.PrefabID] != buildableStateForDef)
			{
				this._buildableStatesByID[buildingDef.PrefabID] = buildableStateForDef;
				if (this.ProductInfoScreen.currentDef == buildingDef)
				{
					this.ignoreToolChangeMessages++;
					this.ProductInfoScreen.ClearProduct(false);
					this.ProductInfoScreen.Show(true);
					this.ProductInfoScreen.ConfigureScreen(buildingDef);
					this.ignoreToolChangeMessages--;
				}
				if (buildableStateForDef == PlanScreen.RequirementsState.Complete)
				{
					foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
					{
						if ((HashedString)toggleInfo.userData == hashedString)
						{
							Bouncer bouncer = this.toggleBouncers[toggleInfo.toggle];
							if (bouncer != null && !bouncer.IsBouncing() && !pooledList.Contains(hashedString))
							{
								pooledList.Add(hashedString);
								bouncer.Bounce();
								if (KTime.Instance.UnscaledGameTime - this.initTime > 1.5f)
								{
									if (this.timeSinceNotificationPing >= this.specialNotificationEmbellishDelay)
									{
										string sound = GlobalAssets.GetSound("NewBuildable_Embellishment", false);
										if (sound != null)
										{
											SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition(), 1f, false));
										}
									}
									string sound2 = GlobalAssets.GetSound("NewBuildable", false);
									if (sound2 != null)
									{
										EventInstance instance = SoundEvent.BeginOneShot(sound2, SoundListenerController.Instance.transform.GetPosition(), 1f, false);
										instance.setParameterByName("playCount", (float)this.notificationPingCount, false);
										SoundEvent.EndOneShot(instance);
									}
								}
								this.timeSinceNotificationPing = 0f;
								this.notificationPingCount++;
							}
						}
					}
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06006838 RID: 26680 RVA: 0x0026FBB8 File Offset: 0x0026DDB8
	private PlanScreen.RequirementsState GetBuildableStateForDef(BuildingDef def)
	{
		if (!def.IsAvailable())
		{
			return PlanScreen.RequirementsState.Invalid;
		}
		PlanScreen.RequirementsState result = PlanScreen.RequirementsState.Complete;
		KPrefabID component = def.BuildingComplete.GetComponent<KPrefabID>();
		if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && !this.IsDefResearched(def))
		{
			result = PlanScreen.RequirementsState.Tech;
		}
		else if (component.HasTag(GameTags.Telepad) && ClusterUtil.ActiveWorldHasPrinter())
		{
			result = PlanScreen.RequirementsState.TelepadBuilt;
		}
		else if (component.HasTag(GameTags.RocketInteriorBuilding) && !ClusterUtil.ActiveWorldIsRocketInterior())
		{
			result = PlanScreen.RequirementsState.RocketInteriorOnly;
		}
		else if (component.HasTag(GameTags.NotRocketInteriorBuilding) && ClusterUtil.ActiveWorldIsRocketInterior())
		{
			result = PlanScreen.RequirementsState.RocketInteriorForbidden;
		}
		else if (component.HasTag(GameTags.UniquePerWorld) && BuildingInventory.Instance.BuildingCountForWorld_BAD_PERF(def.Tag, ClusterManager.Instance.activeWorldId) > 0)
		{
			result = PlanScreen.RequirementsState.UniquePerWorld;
		}
		else if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && !ProductInfoScreen.MaterialsMet(def.CraftRecipe))
		{
			result = PlanScreen.RequirementsState.Materials;
		}
		return result;
	}

	// Token: 0x06006839 RID: 26681 RVA: 0x0026FC9C File Offset: 0x0026DE9C
	private void SetCategoryButtonState()
	{
		this.nextCategoryToUpdateIDX = (this.nextCategoryToUpdateIDX + 1) % this.toggleEntries.Count;
		for (int i = 0; i < this.toggleEntries.Count; i++)
		{
			if (this.forceUpdateAllCategoryToggles || i == this.nextCategoryToUpdateIDX)
			{
				PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[i];
				KIconToggleMenu.ToggleInfo toggleInfo = toggleEntry.toggleInfo;
				toggleInfo.toggle.ActivateFlourish(this.activeCategoryInfo != null && toggleInfo.userData == this.activeCategoryInfo.userData);
				bool flag = false;
				bool flag2 = true;
				if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
				{
					flag = true;
					flag2 = false;
				}
				else
				{
					foreach (BuildingDef def in toggleEntry.buildingDefs)
					{
						if (this.GetBuildableState(def) == PlanScreen.RequirementsState.Complete)
						{
							flag = true;
							flag2 = false;
							break;
						}
					}
					if (flag2 && toggleEntry.AreAnyRequiredTechItemsAvailable())
					{
						flag2 = false;
					}
				}
				this.CategoryInteractive[toggleInfo] = !flag2;
				GameObject gameObject = toggleInfo.toggle.fgImage.transform.Find("ResearchIcon").gameObject;
				if (!flag)
				{
					if (flag2 && toggleEntry.hideIfNotResearched)
					{
						toggleInfo.toggle.gameObject.SetActive(false);
					}
					else if (flag2)
					{
						toggleInfo.toggle.gameObject.SetActive(true);
						gameObject.gameObject.SetActive(true);
					}
					else
					{
						toggleInfo.toggle.gameObject.SetActive(true);
						gameObject.gameObject.SetActive(false);
					}
					ImageToggleState.State state = (this.activeCategoryInfo != null && toggleInfo.userData == this.activeCategoryInfo.userData) ? ImageToggleState.State.DisabledActive : ImageToggleState.State.Disabled;
					ImageToggleState[] toggleImages = toggleEntry.toggleImages;
					for (int j = 0; j < toggleImages.Length; j++)
					{
						toggleImages[j].SetState(state);
					}
				}
				else
				{
					toggleInfo.toggle.gameObject.SetActive(true);
					gameObject.gameObject.SetActive(false);
					ImageToggleState.State state2 = (this.activeCategoryInfo == null || toggleInfo.userData != this.activeCategoryInfo.userData) ? ImageToggleState.State.Inactive : ImageToggleState.State.Active;
					ImageToggleState[] toggleImages = toggleEntry.toggleImages;
					for (int j = 0; j < toggleImages.Length; j++)
					{
						toggleImages[j].SetState(state2);
					}
				}
			}
		}
		this.RefreshCopyBuildingButton(null);
		this.forceUpdateAllCategoryToggles = false;
	}

	// Token: 0x0600683A RID: 26682 RVA: 0x0026FF08 File Offset: 0x0026E108
	private void DeactivateBuildTools()
	{
		InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
		if (activeTool != null)
		{
			Type type = activeTool.GetType();
			if (type == typeof(BuildTool) || typeof(BaseUtilityBuildTool).IsAssignableFrom(type) || type == typeof(PrebuildTool))
			{
				activeTool.DeactivateTool(null);
				PlayerController.Instance.ActivateTool(SelectTool.Instance);
			}
		}
	}

	// Token: 0x0600683B RID: 26683 RVA: 0x0026FF7C File Offset: 0x0026E17C
	public void CloseRecipe(bool playSound = false)
	{
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
		}
		if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.DeactivateBuildTools();
		if (this.ProductInfoScreen != null)
		{
			this.ProductInfoScreen.ClearProduct(true);
		}
		if (this.activeCategoryInfo != null)
		{
			this.UpdateBuildingButtonList(this.activeCategoryInfo);
		}
		this.SelectedBuildingGameObject = null;
	}

	// Token: 0x0600683C RID: 26684 RVA: 0x00270004 File Offset: 0x0026E204
	public void SoftCloseRecipe()
	{
		this.ignoreToolChangeMessages++;
		if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.DeactivateBuildTools();
		if (this.ProductInfoScreen != null)
		{
			this.ProductInfoScreen.ClearProduct(true);
		}
		this.currentlySelectedToggle = null;
		this.SelectedBuildingGameObject = null;
		this.ignoreToolChangeMessages--;
	}

	// Token: 0x0600683D RID: 26685 RVA: 0x00270088 File Offset: 0x0026E288
	public void CloseCategoryPanel(bool playSound = true)
	{
		this.activeCategoryInfo = null;
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Collapse(delegate(object s)
		{
			this.ClearButtons();
			this.buildingGroupsRoot.gameObject.SetActive(false);
			this.ForceUpdateAllCategoryToggles(null);
		});
		this.PlanCategoryLabel.text = "";
		this.ForceUpdateAllCategoryToggles(null);
	}

	// Token: 0x0600683E RID: 26686 RVA: 0x002700E4 File Offset: 0x0026E2E4
	private void OnClickCategory(KIconToggleMenu.ToggleInfo toggle_info)
	{
		this.CloseRecipe(false);
		if (!this.CategoryInteractive.ContainsKey(toggle_info) || !this.CategoryInteractive[toggle_info])
		{
			this.CloseCategoryPanel(false);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			return;
		}
		if (this.activeCategoryInfo == toggle_info)
		{
			this.CloseCategoryPanel(true);
		}
		else
		{
			this.OpenCategoryPanel(toggle_info, true);
		}
		this.ConfigurePanelSize(null);
		this.SetScrollPoint(0f);
	}

	// Token: 0x0600683F RID: 26687 RVA: 0x00270158 File Offset: 0x0026E358
	private void OpenCategoryPanel(KIconToggleMenu.ToggleInfo toggle_info, bool play_sound = true)
	{
		HashedString hashedString = (HashedString)toggle_info.userData;
		if (BuildingGroupScreen.Instance != null)
		{
			BuildingGroupScreen.Instance.ClearSearch();
		}
		this.ClearButtons();
		this.buildingGroupsRoot.gameObject.SetActive(true);
		this.activeCategoryInfo = toggle_info;
		if (play_sound)
		{
			UISounds.PlaySound(UISounds.Sound.ClickObject);
		}
		this.BuildButtonList();
		this.ForceRefreshAllBuildingToggles();
		this.UpdateBuildingButtonList(this.activeCategoryInfo);
		this.RefreshCategoryPanelTitle();
		this.ForceUpdateAllCategoryToggles(null);
		this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Expand(null);
	}

	// Token: 0x06006840 RID: 26688 RVA: 0x002701E8 File Offset: 0x0026E3E8
	public void RefreshCategoryPanelTitle()
	{
		if (this.activeCategoryInfo != null)
		{
			this.PlanCategoryLabel.text = this.activeCategoryInfo.text.ToUpper();
		}
		if (!BuildingGroupScreen.SearchIsEmpty)
		{
			this.PlanCategoryLabel.text = UI.BUILDMENU.SEARCH_RESULTS_HEADER;
		}
	}

	// Token: 0x06006841 RID: 26689 RVA: 0x00270234 File Offset: 0x0026E434
	public void OpenCategoryByName(string category)
	{
		PlanScreen.ToggleEntry toggleEntry;
		if (this.GetToggleEntryForCategory(category, out toggleEntry))
		{
			this.OpenCategoryPanel(toggleEntry.toggleInfo, false);
			this.ConfigurePanelSize(null);
		}
	}

	// Token: 0x06006842 RID: 26690 RVA: 0x00270268 File Offset: 0x0026E468
	private void UpdateBuildingButtonList(KIconToggleMenu.ToggleInfo toggle_info)
	{
		KToggle toggle = toggle_info.toggle;
		if (toggle == null)
		{
			foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
			{
				if (toggleInfo.userData == toggle_info.userData)
				{
					toggle = toggleInfo.toggle;
					break;
				}
			}
		}
		if (toggle != null && this.allBuildingToggles.Count != 0)
		{
			if (this.forceRefreshAllBuildings)
			{
				for (int i = 0; i < this.allBuildingToggles.Count; i++)
				{
					PlanBuildingToggle value = this.allBuildingToggles.ElementAt(i).Value;
					this.categoryPanelSizeNeedsRefresh = (value.Refresh() || this.categoryPanelSizeNeedsRefresh);
					this.forceRefreshAllBuildings = false;
					value.SwitchViewMode(this.useSubCategoryLayout);
				}
			}
			else
			{
				for (int j = 0; j < this.maxToggleRefreshPerFrame; j++)
				{
					if (this.building_button_refresh_idx >= this.allBuildingToggles.Count)
					{
						this.building_button_refresh_idx = 0;
					}
					PlanBuildingToggle value2 = this.allBuildingToggles.ElementAt(this.building_button_refresh_idx).Value;
					this.categoryPanelSizeNeedsRefresh = (value2.Refresh() || this.categoryPanelSizeNeedsRefresh);
					value2.SwitchViewMode(this.useSubCategoryLayout);
					this.building_button_refresh_idx++;
				}
			}
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
			GridLayoutGroup componentInChildren = keyValuePair.Value.GetComponentInChildren<GridLayoutGroup>(true);
			if (!(componentInChildren == null))
			{
				int num = 0;
				for (int k = 0; k < componentInChildren.transform.childCount; k++)
				{
					if (componentInChildren.transform.GetChild(k).gameObject.activeSelf)
					{
						num++;
					}
				}
				if (keyValuePair.Value.gameObject.activeSelf != num > 0)
				{
					keyValuePair.Value.gameObject.SetActive(num > 0);
				}
			}
		}
		if (this.categoryPanelSizeNeedsRefresh && this.building_button_refresh_idx >= this.activeCategoryBuildingToggles.Count)
		{
			this.categoryPanelSizeNeedsRefresh = false;
			this.ConfigurePanelSize(null);
		}
	}

	// Token: 0x06006843 RID: 26691 RVA: 0x002704C4 File Offset: 0x0026E6C4
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		this.RefreshBuildableStates(false);
		this.SetCategoryButtonState();
		if (this.activeCategoryInfo != null)
		{
			this.UpdateBuildingButtonList(this.activeCategoryInfo);
		}
	}

	// Token: 0x06006844 RID: 26692 RVA: 0x002704F0 File Offset: 0x0026E6F0
	private void BuildButtonList()
	{
		this.activeCategoryBuildingToggles.Clear();
		Dictionary<string, HashedString> dictionary = new Dictionary<string, HashedString>();
		Dictionary<string, List<BuildingDef>> dictionary2 = new Dictionary<string, List<BuildingDef>>();
		if (!dictionary2.ContainsKey("default"))
		{
			dictionary2.Add("default", new List<BuildingDef>());
		}
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				if (buildingDef.IsAvailable() && buildingDef.ShouldShowInBuildMenu() && buildingDef.IsValidDLC())
				{
					dictionary.Add(buildingDef.PrefabID, planInfo.category);
					if (!dictionary2.ContainsKey(keyValuePair.Value))
					{
						dictionary2.Add(keyValuePair.Value, new List<BuildingDef>());
					}
					dictionary2[keyValuePair.Value].Add(buildingDef);
				}
			}
		}
		if (!this.allSubCategoryObjects.ContainsKey("default"))
		{
			this.allSubCategoryObjects.Add("default", global::Util.KInstantiateUI(this.subgroupPrefab, this.GroupsTransform.gameObject, true));
		}
		GameObject gameObject = this.allSubCategoryObjects["default"].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").gameObject;
		foreach (KeyValuePair<string, List<BuildingDef>> keyValuePair2 in dictionary2)
		{
			if (!this.allSubCategoryObjects.ContainsKey(keyValuePair2.Key))
			{
				this.allSubCategoryObjects.Add(keyValuePair2.Key, global::Util.KInstantiateUI(this.subgroupPrefab, this.GroupsTransform.gameObject, true));
			}
			if (keyValuePair2.Key == "default")
			{
				this.allSubCategoryObjects[keyValuePair2.Key].SetActive(this.useSubCategoryLayout);
			}
			HierarchyReferences component = this.allSubCategoryObjects[keyValuePair2.Key].GetComponent<HierarchyReferences>();
			GameObject parent;
			if (this.useSubCategoryLayout)
			{
				component.GetReference<RectTransform>("Header").gameObject.SetActive(true);
				parent = this.allSubCategoryObjects[keyValuePair2.Key].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").gameObject;
				StringEntry entry;
				if (Strings.TryGet("STRINGS.UI.NEWBUILDCATEGORIES." + keyValuePair2.Key.ToUpper() + ".BUILDMENUTITLE", out entry))
				{
					component.GetReference<LocText>("HeaderLabel").SetText(entry);
				}
			}
			else
			{
				component.GetReference<RectTransform>("Header").gameObject.SetActive(false);
				parent = gameObject;
			}
			foreach (BuildingDef buildingDef2 in keyValuePair2.Value)
			{
				HashedString hashedString = dictionary[buildingDef2.PrefabID];
				GameObject gameObject2 = this.CreateButton(buildingDef2, parent, hashedString);
				PlanScreen.ToggleEntry toggleEntry = null;
				this.GetToggleEntryForCategory(hashedString, out toggleEntry);
				if (toggleEntry != null && toggleEntry.pendingResearchAttentions.Contains(buildingDef2.PrefabID))
				{
					gameObject2.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
				}
			}
		}
		this.RefreshScale(null);
	}

	// Token: 0x06006845 RID: 26693 RVA: 0x002708B8 File Offset: 0x0026EAB8
	public void ConfigurePanelSize(object data = null)
	{
		if (this.useSubCategoryLayout)
		{
			this.buildGrid_bg_rowHeight = 48f;
		}
		else
		{
			this.buildGrid_bg_rowHeight = (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.bigBuildingButtonSize.y : PlanScreen.standarduildingButtonSize.y);
		}
		GridLayoutGroup reference = this.subgroupPrefab.GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid");
		this.buildGrid_bg_rowHeight += reference.spacing.y;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.GroupsTransform.childCount; i++)
		{
			int num3 = 0;
			HierarchyReferences component = this.GroupsTransform.GetChild(i).GetComponent<HierarchyReferences>();
			if (!(component == null))
			{
				GridLayoutGroup reference2 = component.GetReference<GridLayoutGroup>("Grid");
				if (!(reference2 == null))
				{
					bool flag = false;
					for (int j = 0; j < reference2.transform.childCount; j++)
					{
						if (reference2.transform.GetChild(j).gameObject.activeSelf)
						{
							flag = true;
							num3++;
						}
					}
					if (flag)
					{
						num2 += 24;
					}
					num += num3 / reference2.constraintCount;
					if (num3 % reference2.constraintCount != 0)
					{
						num++;
					}
				}
			}
		}
		num2 = Math.Min(72, num2);
		this.noResultMessage.SetActive(num == 0);
		int num4 = num;
		int num5 = Math.Max(1, Screen.height / (int)this.buildGrid_bg_rowHeight - 3);
		num5 = Math.Min(num5, this.useSubCategoryLayout ? 12 : 6);
		if (BuildingGroupScreen.IsEditing || !BuildingGroupScreen.SearchIsEmpty)
		{
			num4 = Mathf.Min(num5, this.useSubCategoryLayout ? 8 : 4);
		}
		this.BuildingGroupContentsRect.GetComponent<ScrollRect>().verticalScrollbar.gameObject.SetActive(num4 >= num5 - 1);
		float num6 = this.buildGrid_bg_borderHeight + (float)num2 + 36f + (float)Mathf.Clamp(num4, 0, num5) * this.buildGrid_bg_rowHeight;
		if (BuildingGroupScreen.IsEditing || !BuildingGroupScreen.SearchIsEmpty)
		{
			num6 = Mathf.Max(num6, this.buildingGroupsRoot.sizeDelta.y);
		}
		this.buildingGroupsRoot.sizeDelta = new Vector2(this.buildGrid_bg_width, num6);
		this.RefreshScale(null);
	}

	// Token: 0x06006846 RID: 26694 RVA: 0x00270AE5 File Offset: 0x0026ECE5
	private void SetScrollPoint(float targetY)
	{
		this.BuildingGroupContentsRect.anchoredPosition = new Vector2(this.BuildingGroupContentsRect.anchoredPosition.x, targetY);
	}

	// Token: 0x06006847 RID: 26695 RVA: 0x00270B08 File Offset: 0x0026ED08
	private GameObject CreateButton(BuildingDef def, GameObject parent, HashedString plan_category)
	{
		GameObject gameObject;
		PlanBuildingToggle planBuildingToggle;
		if (this.allBuildingToggles.ContainsKey(def.PrefabID))
		{
			gameObject = this.allBuildingToggles[def.PrefabID].gameObject;
			planBuildingToggle = this.allBuildingToggles[def.PrefabID];
			planBuildingToggle.Refresh();
		}
		else
		{
			gameObject = global::Util.KInstantiateUI(this.planButtonPrefab, parent, false);
			gameObject.name = UI.StripLinkFormatting(def.name) + " Group:" + plan_category.ToString();
			planBuildingToggle = gameObject.GetComponentInChildren<PlanBuildingToggle>();
			planBuildingToggle.Config(def, this, plan_category);
			planBuildingToggle.soundPlayer.Enabled = false;
			planBuildingToggle.SwitchViewMode(this.useSubCategoryLayout);
			this.allBuildingToggles.Add(def.PrefabID, planBuildingToggle);
		}
		if (gameObject.transform.parent != parent)
		{
			gameObject.transform.SetParent(parent.transform);
		}
		this.activeCategoryBuildingToggles.Add(def, planBuildingToggle);
		return gameObject;
	}

	// Token: 0x06006848 RID: 26696 RVA: 0x00270C00 File Offset: 0x0026EE00
	public static bool TechRequirementsMet(TechItem techItem)
	{
		return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem == null || techItem.IsComplete();
	}

	// Token: 0x06006849 RID: 26697 RVA: 0x00270C20 File Offset: 0x0026EE20
	private static bool TechRequirementsUpcoming(TechItem techItem)
	{
		return PlanScreen.TechRequirementsMet(techItem);
	}

	// Token: 0x0600684A RID: 26698 RVA: 0x00270C28 File Offset: 0x0026EE28
	private bool GetToggleEntryForCategory(HashedString category, out PlanScreen.ToggleEntry toggleEntry)
	{
		toggleEntry = null;
		foreach (PlanScreen.ToggleEntry toggleEntry2 in this.toggleEntries)
		{
			if (toggleEntry2.planCategory == category)
			{
				toggleEntry = toggleEntry2;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600684B RID: 26699 RVA: 0x00270C90 File Offset: 0x0026EE90
	public bool IsDefBuildable(BuildingDef def)
	{
		return this.GetBuildableState(def) == PlanScreen.RequirementsState.Complete;
	}

	// Token: 0x0600684C RID: 26700 RVA: 0x00270C9C File Offset: 0x0026EE9C
	public string GetTooltipForBuildable(BuildingDef def)
	{
		PlanScreen.RequirementsState buildableState = this.GetBuildableState(def);
		return PlanScreen.GetTooltipForRequirementsState(def, buildableState);
	}

	// Token: 0x0600684D RID: 26701 RVA: 0x00270CB8 File Offset: 0x0026EEB8
	public static string GetTooltipForRequirementsState(BuildingDef def, PlanScreen.RequirementsState state)
	{
		TechItem techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		string text = null;
		if (Game.Instance.SandboxModeActive)
		{
			text = UIConstants.ColorPrefixYellow + UI.SANDBOXTOOLS.SETTINGS.INSTANT_BUILD.NAME + UIConstants.ColorSuffix;
		}
		else if (DebugHandler.InstantBuildMode)
		{
			text = UIConstants.ColorPrefixYellow + UI.DEBUG_TOOLS.DEBUG_ACTIVE + UIConstants.ColorSuffix;
		}
		else
		{
			switch (state)
			{
			case PlanScreen.RequirementsState.Tech:
				text = string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, techItem.ParentTech.Name);
				break;
			case PlanScreen.RequirementsState.Materials:
				text = UI.PRODUCTINFO_MISSINGRESOURCES_HOVER;
				foreach (Recipe.Ingredient ingredient in def.CraftRecipe.Ingredients)
				{
					string str = string.Format("{0}{1}: {2}", "• ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
					text = text + "\n" + str;
				}
				break;
			case PlanScreen.RequirementsState.TelepadBuilt:
				text = UI.PRODUCTINFO_UNIQUE_PER_WORLD;
				break;
			case PlanScreen.RequirementsState.UniquePerWorld:
				text = UI.PRODUCTINFO_UNIQUE_PER_WORLD;
				break;
			case PlanScreen.RequirementsState.RocketInteriorOnly:
				text = UI.PRODUCTINFO_ROCKET_INTERIOR;
				break;
			case PlanScreen.RequirementsState.RocketInteriorForbidden:
				text = UI.PRODUCTINFO_ROCKET_NOT_INTERIOR;
				break;
			}
		}
		return text;
	}

	// Token: 0x0600684E RID: 26702 RVA: 0x00270E44 File Offset: 0x0026F044
	private void PointerEnter(PointerEventData data)
	{
		this.planScreenScrollRect.mouseIsOver = true;
	}

	// Token: 0x0600684F RID: 26703 RVA: 0x00270E52 File Offset: 0x0026F052
	private void PointerExit(PointerEventData data)
	{
		this.planScreenScrollRect.mouseIsOver = false;
	}

	// Token: 0x06006850 RID: 26704 RVA: 0x00270E60 File Offset: 0x0026F060
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (this.mouseOver && base.ConsumeMouseScroll)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut))
				{
					this.planScreenScrollRect.OnKeyDown(e);
				}
			}
			else if (!e.TryConsume(global::Action.ZoomIn))
			{
				e.TryConsume(global::Action.ZoomOut);
			}
		}
		if (e.IsAction(global::Action.CopyBuilding) && e.TryConsume(global::Action.CopyBuilding))
		{
			this.OnClickCopyBuilding();
		}
		if (this.toggles == null)
		{
			return;
		}
		if (!e.Consumed && this.activeCategoryInfo != null && e.TryConsume(global::Action.Escape))
		{
			this.OnClickCategory(this.activeCategoryInfo);
			SelectTool.Instance.Activate();
			this.ClearSelection();
			return;
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06006851 RID: 26705 RVA: 0x00270F28 File Offset: 0x0026F128
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.mouseOver && base.ConsumeMouseScroll)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut))
				{
					this.planScreenScrollRect.OnKeyUp(e);
				}
			}
			else if (!e.TryConsume(global::Action.ZoomIn))
			{
				e.TryConsume(global::Action.ZoomOut);
			}
		}
		if (e.Consumed)
		{
			return;
		}
		if (this.SelectedBuildingGameObject != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.CloseRecipe(false);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		else if (this.activeCategoryInfo != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.OnUIClear(null);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06006852 RID: 26706 RVA: 0x00270FE8 File Offset: 0x0026F1E8
	private void OnRecipeElementsFullySelected()
	{
		BuildingDef buildingDef = null;
		foreach (KeyValuePair<string, PlanBuildingToggle> keyValuePair in this.allBuildingToggles)
		{
			if (keyValuePair.Value == this.currentlySelectedToggle)
			{
				buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				break;
			}
		}
		DebugUtil.DevAssert(buildingDef, "def is null", null);
		if (buildingDef)
		{
			if (buildingDef.isKAnimTile && buildingDef.isUtility)
			{
				IList<Tag> getSelectedElementAsList = this.ProductInfoScreen.materialSelectionPanel.GetSelectedElementAsList;
				((buildingDef.BuildingComplete.GetComponent<Wire>() != null) ? WireBuildTool.Instance : UtilityBuildTool.Instance).Activate(buildingDef, getSelectedElementAsList, this.ProductInfoScreen.FacadeSelectionPanel.SelectedFacade);
				return;
			}
			BuildTool.Instance.Activate(buildingDef, this.ProductInfoScreen.materialSelectionPanel.GetSelectedElementAsList, this.ProductInfoScreen.FacadeSelectionPanel.SelectedFacade);
		}
	}

	// Token: 0x06006853 RID: 26707 RVA: 0x002710F8 File Offset: 0x0026F2F8
	public void OnResearchComplete(object tech)
	{
		if (tech is Tech)
		{
			using (List<TechItem>.Enumerator enumerator = ((Tech)tech).unlockedItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TechItem techItem = enumerator.Current;
					BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
					this.AddResearchedBuildingCategory(buildingDef);
				}
				return;
			}
		}
		if (tech is BuildingDef)
		{
			BuildingDef def = tech as BuildingDef;
			this.AddResearchedBuildingCategory(def);
		}
	}

	// Token: 0x06006854 RID: 26708 RVA: 0x00271178 File Offset: 0x0026F378
	private void AddResearchedBuildingCategory(BuildingDef def)
	{
		if (def != null)
		{
			this.UpdateDefResearched(def);
			if (this.tagCategoryMap.ContainsKey(def.Tag))
			{
				HashedString category = this.tagCategoryMap[def.Tag];
				PlanScreen.ToggleEntry toggleEntry;
				if (this.GetToggleEntryForCategory(category, out toggleEntry))
				{
					toggleEntry.pendingResearchAttentions.Add(def.Tag);
					toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
					toggleEntry.Refresh();
				}
			}
		}
	}

	// Token: 0x06006855 RID: 26709 RVA: 0x002711F4 File Offset: 0x0026F3F4
	private void OnUIClear(object data)
	{
		if (this.activeCategoryInfo != null)
		{
			this.selected = -1;
			this.OnClickCategory(this.activeCategoryInfo);
			SelectTool.Instance.Activate();
			PlayerController.Instance.ActivateTool(SelectTool.Instance);
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x06006856 RID: 26710 RVA: 0x00271244 File Offset: 0x0026F444
	private void OnActiveToolChanged(object data)
	{
		if (data == null)
		{
			return;
		}
		if (this.ignoreToolChangeMessages > 0)
		{
			return;
		}
		Type type = data.GetType();
		if (!typeof(BuildTool).IsAssignableFrom(type) && !typeof(PrebuildTool).IsAssignableFrom(type) && !typeof(BaseUtilityBuildTool).IsAssignableFrom(type))
		{
			this.CloseRecipe(false);
			this.CloseCategoryPanel(false);
		}
	}

	// Token: 0x06006857 RID: 26711 RVA: 0x002712AA File Offset: 0x0026F4AA
	public PrioritySetting GetBuildingPriority()
	{
		return this.ProductInfoScreen.materialSelectionPanel.PriorityScreen.GetLastSelectedPriority();
	}

	// Token: 0x04004644 RID: 17988
	[SerializeField]
	private GameObject planButtonPrefab;

	// Token: 0x04004645 RID: 17989
	[SerializeField]
	private GameObject recipeInfoScreenParent;

	// Token: 0x04004646 RID: 17990
	[SerializeField]
	private GameObject productInfoScreenPrefab;

	// Token: 0x04004647 RID: 17991
	[SerializeField]
	private GameObject copyBuildingButton;

	// Token: 0x04004648 RID: 17992
	[SerializeField]
	private KButton gridViewButton;

	// Token: 0x04004649 RID: 17993
	[SerializeField]
	private KButton listViewButton;

	// Token: 0x0400464A RID: 17994
	private bool useSubCategoryLayout;

	// Token: 0x0400464B RID: 17995
	private int refreshScaleHandle = -1;

	// Token: 0x0400464C RID: 17996
	[SerializeField]
	private GameObject adjacentPinnedButtons;

	// Token: 0x0400464D RID: 17997
	private static Dictionary<HashedString, string> iconNameMap = new Dictionary<HashedString, string>
	{
		{
			PlanScreen.CacheHashedString("Base"),
			"icon_category_base"
		},
		{
			PlanScreen.CacheHashedString("Oxygen"),
			"icon_category_oxygen"
		},
		{
			PlanScreen.CacheHashedString("Power"),
			"icon_category_electrical"
		},
		{
			PlanScreen.CacheHashedString("Food"),
			"icon_category_food"
		},
		{
			PlanScreen.CacheHashedString("Plumbing"),
			"icon_category_plumbing"
		},
		{
			PlanScreen.CacheHashedString("HVAC"),
			"icon_category_ventilation"
		},
		{
			PlanScreen.CacheHashedString("Refining"),
			"icon_category_refinery"
		},
		{
			PlanScreen.CacheHashedString("Medical"),
			"icon_category_medical"
		},
		{
			PlanScreen.CacheHashedString("Furniture"),
			"icon_category_furniture"
		},
		{
			PlanScreen.CacheHashedString("Equipment"),
			"icon_category_misc"
		},
		{
			PlanScreen.CacheHashedString("Utilities"),
			"icon_category_utilities"
		},
		{
			PlanScreen.CacheHashedString("Automation"),
			"icon_category_automation"
		},
		{
			PlanScreen.CacheHashedString("Conveyance"),
			"icon_category_shipping"
		},
		{
			PlanScreen.CacheHashedString("Rocketry"),
			"icon_category_rocketry"
		},
		{
			PlanScreen.CacheHashedString("HEP"),
			"icon_category_radiation"
		}
	};

	// Token: 0x0400464E RID: 17998
	private Dictionary<KIconToggleMenu.ToggleInfo, bool> CategoryInteractive = new Dictionary<KIconToggleMenu.ToggleInfo, bool>();

	// Token: 0x04004650 RID: 18000
	[SerializeField]
	public PlanScreen.BuildingToolTipSettings buildingToolTipSettings;

	// Token: 0x04004651 RID: 18001
	public PlanScreen.BuildingNameTextSetting buildingNameTextSettings;

	// Token: 0x04004652 RID: 18002
	private KIconToggleMenu.ToggleInfo activeCategoryInfo;

	// Token: 0x04004653 RID: 18003
	public Dictionary<BuildingDef, PlanBuildingToggle> activeCategoryBuildingToggles = new Dictionary<BuildingDef, PlanBuildingToggle>();

	// Token: 0x04004654 RID: 18004
	private float timeSinceNotificationPing;

	// Token: 0x04004655 RID: 18005
	private float notificationPingExpire = 0.5f;

	// Token: 0x04004656 RID: 18006
	private float specialNotificationEmbellishDelay = 8f;

	// Token: 0x04004657 RID: 18007
	private int notificationPingCount;

	// Token: 0x04004658 RID: 18008
	private Dictionary<KToggle, Bouncer> toggleBouncers = new Dictionary<KToggle, Bouncer>();

	// Token: 0x04004659 RID: 18009
	public const string DEFAULT_SUBCATEGORY_KEY = "default";

	// Token: 0x0400465A RID: 18010
	private Dictionary<string, GameObject> allSubCategoryObjects = new Dictionary<string, GameObject>();

	// Token: 0x0400465B RID: 18011
	private Dictionary<string, PlanBuildingToggle> allBuildingToggles = new Dictionary<string, PlanBuildingToggle>();

	// Token: 0x0400465C RID: 18012
	private static Vector2 bigBuildingButtonSize = new Vector2(98f, 123f);

	// Token: 0x0400465D RID: 18013
	private static Vector2 standarduildingButtonSize = PlanScreen.bigBuildingButtonSize * 0.8f;

	// Token: 0x0400465E RID: 18014
	public static int fontSizeBigMode = 16;

	// Token: 0x0400465F RID: 18015
	public static int fontSizeStandardMode = 14;

	// Token: 0x04004661 RID: 18017
	[SerializeField]
	private GameObject subgroupPrefab;

	// Token: 0x04004662 RID: 18018
	public Transform GroupsTransform;

	// Token: 0x04004663 RID: 18019
	public Sprite Overlay_NeedTech;

	// Token: 0x04004664 RID: 18020
	public RectTransform buildingGroupsRoot;

	// Token: 0x04004665 RID: 18021
	public RectTransform BuildButtonBGPanel;

	// Token: 0x04004666 RID: 18022
	public RectTransform BuildingGroupContentsRect;

	// Token: 0x04004667 RID: 18023
	public Sprite defaultBuildingIconSprite;

	// Token: 0x04004668 RID: 18024
	private KScrollRect planScreenScrollRect;

	// Token: 0x04004669 RID: 18025
	public Material defaultUIMaterial;

	// Token: 0x0400466A RID: 18026
	public Material desaturatedUIMaterial;

	// Token: 0x0400466B RID: 18027
	public LocText PlanCategoryLabel;

	// Token: 0x0400466C RID: 18028
	public GameObject noResultMessage;

	// Token: 0x0400466D RID: 18029
	private int nextCategoryToUpdateIDX = -1;

	// Token: 0x0400466E RID: 18030
	private bool forceUpdateAllCategoryToggles;

	// Token: 0x0400466F RID: 18031
	private bool forceRefreshAllBuildings = true;

	// Token: 0x04004670 RID: 18032
	private List<PlanScreen.ToggleEntry> toggleEntries = new List<PlanScreen.ToggleEntry>();

	// Token: 0x04004671 RID: 18033
	private int ignoreToolChangeMessages;

	// Token: 0x04004672 RID: 18034
	private Dictionary<string, PlanScreen.RequirementsState> _buildableStatesByID = new Dictionary<string, PlanScreen.RequirementsState>();

	// Token: 0x04004673 RID: 18035
	private Dictionary<Def, bool> _researchedDefs = new Dictionary<Def, bool>();

	// Token: 0x04004674 RID: 18036
	[SerializeField]
	private TextStyleSetting[] CategoryLabelTextStyles;

	// Token: 0x04004675 RID: 18037
	private float initTime;

	// Token: 0x04004676 RID: 18038
	private Dictionary<Tag, HashedString> tagCategoryMap;

	// Token: 0x04004677 RID: 18039
	private Dictionary<Tag, int> tagOrderMap;

	// Token: 0x04004678 RID: 18040
	private BuildingDef lastSelectedBuildingDef;

	// Token: 0x04004679 RID: 18041
	private Building lastSelectedBuilding;

	// Token: 0x0400467A RID: 18042
	private string lastSelectedBuildingFacade = "DEFAULT_FACADE";

	// Token: 0x0400467B RID: 18043
	private int buildable_state_update_idx;

	// Token: 0x0400467C RID: 18044
	private int building_button_refresh_idx;

	// Token: 0x0400467D RID: 18045
	private int maxToggleRefreshPerFrame = 10;

	// Token: 0x0400467E RID: 18046
	private bool categoryPanelSizeNeedsRefresh;

	// Token: 0x0400467F RID: 18047
	private float buildGrid_bg_width = 320f;

	// Token: 0x04004680 RID: 18048
	private float buildGrid_bg_borderHeight = 48f;

	// Token: 0x04004681 RID: 18049
	private const float BUILDGRID_SEARCHBAR_HEIGHT = 36f;

	// Token: 0x04004682 RID: 18050
	private const int SUBCATEGORY_HEADER_HEIGHT = 24;

	// Token: 0x04004683 RID: 18051
	private float buildGrid_bg_rowHeight;

	// Token: 0x02001E32 RID: 7730
	public struct PlanInfo
	{
		// Token: 0x0600AACB RID: 43723 RVA: 0x003A3200 File Offset: 0x003A1400
		public PlanInfo(HashedString category, bool hideIfNotResearched, List<string> listData, string RequiredDlcId = "")
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (string key in listData)
			{
				list.Add(new KeyValuePair<string, string>(key, TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(key) ? TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[key] : "uncategorized"));
			}
			this.category = category;
			this.hideIfNotResearched = hideIfNotResearched;
			this.data = listData;
			this.buildingAndSubcategoryData = list;
			this.RequiredDlcId = RequiredDlcId;
		}

		// Token: 0x040089A8 RID: 35240
		public HashedString category;

		// Token: 0x040089A9 RID: 35241
		public bool hideIfNotResearched;

		// Token: 0x040089AA RID: 35242
		[Obsolete("Modders: Use ModUtil.AddBuildingToPlanScreen")]
		public List<string> data;

		// Token: 0x040089AB RID: 35243
		public List<KeyValuePair<string, string>> buildingAndSubcategoryData;

		// Token: 0x040089AC RID: 35244
		public string RequiredDlcId;
	}

	// Token: 0x02001E33 RID: 7731
	[Serializable]
	public struct BuildingToolTipSettings
	{
		// Token: 0x040089AD RID: 35245
		public TextStyleSetting BuildButtonName;

		// Token: 0x040089AE RID: 35246
		public TextStyleSetting BuildButtonDescription;

		// Token: 0x040089AF RID: 35247
		public TextStyleSetting MaterialRequirement;

		// Token: 0x040089B0 RID: 35248
		public TextStyleSetting ResearchRequirement;
	}

	// Token: 0x02001E34 RID: 7732
	[Serializable]
	public struct BuildingNameTextSetting
	{
		// Token: 0x040089B1 RID: 35249
		public TextStyleSetting ActiveSelected;

		// Token: 0x040089B2 RID: 35250
		public TextStyleSetting ActiveDeselected;

		// Token: 0x040089B3 RID: 35251
		public TextStyleSetting InactiveSelected;

		// Token: 0x040089B4 RID: 35252
		public TextStyleSetting InactiveDeselected;
	}

	// Token: 0x02001E35 RID: 7733
	private class ToggleEntry
	{
		// Token: 0x0600AACC RID: 43724 RVA: 0x003A329C File Offset: 0x003A149C
		public ToggleEntry(KIconToggleMenu.ToggleInfo toggle_info, HashedString plan_category, List<BuildingDef> building_defs, bool hideIfNotResearched)
		{
			this.toggleInfo = toggle_info;
			this.planCategory = plan_category;
			building_defs.RemoveAll((BuildingDef def) => !def.IsValidDLC());
			this.buildingDefs = building_defs;
			this.hideIfNotResearched = hideIfNotResearched;
			this.pendingResearchAttentions = new List<Tag>();
			this.requiredTechItems = new List<TechItem>();
			this.toggleImages = null;
			foreach (BuildingDef buildingDef in building_defs)
			{
				TechItem techItem = Db.Get().TechItems.TryGet(buildingDef.PrefabID);
				if (techItem == null)
				{
					this.requiredTechItems.Clear();
					break;
				}
				if (!this.requiredTechItems.Contains(techItem))
				{
					this.requiredTechItems.Add(techItem);
				}
			}
			this._areAnyRequiredTechItemsAvailable = false;
			this.Refresh();
		}

		// Token: 0x0600AACD RID: 43725 RVA: 0x003A3398 File Offset: 0x003A1598
		public bool AreAnyRequiredTechItemsAvailable()
		{
			return this._areAnyRequiredTechItemsAvailable;
		}

		// Token: 0x0600AACE RID: 43726 RVA: 0x003A33A0 File Offset: 0x003A15A0
		public void Refresh()
		{
			if (this._areAnyRequiredTechItemsAvailable)
			{
				return;
			}
			if (this.requiredTechItems.Count == 0)
			{
				this._areAnyRequiredTechItemsAvailable = true;
				return;
			}
			using (List<TechItem>.Enumerator enumerator = this.requiredTechItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (PlanScreen.TechRequirementsUpcoming(enumerator.Current))
					{
						this._areAnyRequiredTechItemsAvailable = true;
						break;
					}
				}
			}
		}

		// Token: 0x0600AACF RID: 43727 RVA: 0x003A341C File Offset: 0x003A161C
		public void CollectToggleImages()
		{
			this.toggleImages = this.toggleInfo.toggle.gameObject.GetComponents<ImageToggleState>();
		}

		// Token: 0x040089B5 RID: 35253
		public KIconToggleMenu.ToggleInfo toggleInfo;

		// Token: 0x040089B6 RID: 35254
		public HashedString planCategory;

		// Token: 0x040089B7 RID: 35255
		public List<BuildingDef> buildingDefs;

		// Token: 0x040089B8 RID: 35256
		public List<Tag> pendingResearchAttentions;

		// Token: 0x040089B9 RID: 35257
		private List<TechItem> requiredTechItems;

		// Token: 0x040089BA RID: 35258
		public ImageToggleState[] toggleImages;

		// Token: 0x040089BB RID: 35259
		public bool hideIfNotResearched;

		// Token: 0x040089BC RID: 35260
		private bool _areAnyRequiredTechItemsAvailable;
	}

	// Token: 0x02001E36 RID: 7734
	public enum RequirementsState
	{
		// Token: 0x040089BE RID: 35262
		Invalid,
		// Token: 0x040089BF RID: 35263
		Tech,
		// Token: 0x040089C0 RID: 35264
		Materials,
		// Token: 0x040089C1 RID: 35265
		Complete,
		// Token: 0x040089C2 RID: 35266
		TelepadBuilt,
		// Token: 0x040089C3 RID: 35267
		UniquePerWorld,
		// Token: 0x040089C4 RID: 35268
		RocketInteriorOnly,
		// Token: 0x040089C5 RID: 35269
		RocketInteriorForbidden
	}
}
