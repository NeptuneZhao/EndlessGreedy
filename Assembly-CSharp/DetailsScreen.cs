using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C3B RID: 3131
public class DetailsScreen : KTabMenu
{
	// Token: 0x0600600C RID: 24588 RVA: 0x0023AE00 File Offset: 0x00239000
	public static void DestroyInstance()
	{
		DetailsScreen.Instance = null;
	}

	// Token: 0x1700072C RID: 1836
	// (get) Token: 0x0600600D RID: 24589 RVA: 0x0023AE08 File Offset: 0x00239008
	// (set) Token: 0x0600600E RID: 24590 RVA: 0x0023AE10 File Offset: 0x00239010
	public GameObject target { get; private set; }

	// Token: 0x0600600F RID: 24591 RVA: 0x0023AE1C File Offset: 0x0023901C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.SortScreenOrder();
		base.ConsumeMouseScroll = true;
		global::Debug.Assert(DetailsScreen.Instance == null);
		DetailsScreen.Instance = this;
		this.InitiateSidescreenTabs();
		this.DeactivateSideContent();
		this.Show(false);
		base.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnSelectObject));
		this.tabHeader.Init();
	}

	// Token: 0x06006010 RID: 24592 RVA: 0x0023AE94 File Offset: 0x00239094
	public bool CanObjectDisplayTabOfType(GameObject obj, DetailsScreen.SidescreenTabTypes type)
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			if (sidescreenTab.type == type)
			{
				return sidescreenTab.ValidateTarget(obj);
			}
		}
		return false;
	}

	// Token: 0x06006011 RID: 24593 RVA: 0x0023AED0 File Offset: 0x002390D0
	public DetailsScreen.SidescreenTab GetTabOfType(DetailsScreen.SidescreenTabTypes type)
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			if (sidescreenTab.type == type)
			{
				return sidescreenTab;
			}
		}
		return null;
	}

	// Token: 0x06006012 RID: 24594 RVA: 0x0023AF08 File Offset: 0x00239108
	public void InitiateSidescreenTabs()
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			sidescreenTab.Initiate(this.original_tab, this.original_tab_body, delegate(DetailsScreen.SidescreenTab _tab)
			{
				this.SelectSideScreenTab(_tab.type);
			});
			switch (sidescreenTab.type)
			{
			case DetailsScreen.SidescreenTabTypes.Errands:
				sidescreenTab.ValidateTargetCallback = ((GameObject target, DetailsScreen.SidescreenTab _tab) => target.GetComponent<MinionIdentity>() != null);
				break;
			case DetailsScreen.SidescreenTabTypes.Material:
				sidescreenTab.ValidateTargetCallback = delegate(GameObject target, DetailsScreen.SidescreenTab _tab)
				{
					Reconstructable component = target.GetComponent<Reconstructable>();
					return component != null && component.AllowReconstruct;
				};
				break;
			case DetailsScreen.SidescreenTabTypes.Blueprints:
				sidescreenTab.ValidateTargetCallback = delegate(GameObject target, DetailsScreen.SidescreenTab _tab)
				{
					UnityEngine.Object component = target.GetComponent<MinionIdentity>();
					BuildingFacade component2 = target.GetComponent<BuildingFacade>();
					return component != null || component2 != null;
				};
				break;
			}
		}
	}

	// Token: 0x06006013 RID: 24595 RVA: 0x0023AFE8 File Offset: 0x002391E8
	private void OnSelectObject(object data)
	{
		if (data == null)
		{
			this.previouslyActiveTab = -1;
			this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Config);
			return;
		}
		KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
		if (!(component == null) && !(this.previousTargetID != component.PrefabID()))
		{
			this.SelectSideScreenTab(this.selectedSidescreenTabID);
			return;
		}
		if (component != null && component.GetComponent<MinionIdentity>())
		{
			this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Errands);
			return;
		}
		this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Config);
	}

	// Token: 0x06006014 RID: 24596 RVA: 0x0023B064 File Offset: 0x00239264
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CodexEntryButton.onClick += this.CodexEntryButton_OnClick;
		this.PinResourceButton.onClick += this.PinResourceButton_OnClick;
		this.CloseButton.onClick += this.DeselectAndClose;
		this.TabTitle.OnNameChanged += this.OnNameChanged;
		this.TabTitle.OnStartedEditing += this.OnStartedEditing;
		this.sideScreen2.SetActive(false);
		base.Subscribe<DetailsScreen>(-1514841199, DetailsScreen.OnRefreshDataDelegate);
	}

	// Token: 0x06006015 RID: 24597 RVA: 0x0023B107 File Offset: 0x00239307
	private void OnStartedEditing()
	{
		base.isEditing = true;
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x06006016 RID: 24598 RVA: 0x0023B11C File Offset: 0x0023931C
	private void OnNameChanged(string newName)
	{
		base.isEditing = false;
		if (string.IsNullOrEmpty(newName))
		{
			return;
		}
		MinionIdentity component = this.target.GetComponent<MinionIdentity>();
		UserNameable component2 = this.target.GetComponent<UserNameable>();
		ClustercraftExteriorDoor component3 = this.target.GetComponent<ClustercraftExteriorDoor>();
		CommandModule component4 = this.target.GetComponent<CommandModule>();
		if (component != null)
		{
			component.SetName(newName);
		}
		else if (component4 != null)
		{
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(component4.GetComponent<LaunchConditionManager>()).SetRocketName(newName);
		}
		else if (component3 != null)
		{
			component3.GetTargetWorld().GetComponent<UserNameable>().SetName(newName);
		}
		else if (component2 != null)
		{
			component2.SetName(newName);
		}
		this.TabTitle.UpdateRenameTooltip(this.target);
	}

	// Token: 0x06006017 RID: 24599 RVA: 0x0023B1D9 File Offset: 0x002393D9
	protected override void OnDeactivate()
	{
		if (this.target != null && this.setRocketTitleHandle != -1)
		{
			this.target.Unsubscribe(this.setRocketTitleHandle);
		}
		this.setRocketTitleHandle = -1;
		this.DeactivateSideContent();
		base.OnDeactivate();
	}

	// Token: 0x06006018 RID: 24600 RVA: 0x0023B216 File Offset: 0x00239416
	protected override void OnShow(bool show)
	{
		if (!show)
		{
			this.DeactivateSideContent();
		}
		else
		{
			this.MaskSideContent(false);
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenHalfEffect);
		}
		base.OnShow(show);
	}

	// Token: 0x06006019 RID: 24601 RVA: 0x0023B246 File Offset: 0x00239446
	protected override void OnCmpDisable()
	{
		this.DeactivateSideContent();
		base.OnCmpDisable();
	}

	// Token: 0x0600601A RID: 24602 RVA: 0x0023B254 File Offset: 0x00239454
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!base.isEditing && this.target != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.DeselectAndClose();
		}
	}

	// Token: 0x0600601B RID: 24603 RVA: 0x0023B280 File Offset: 0x00239480
	private static Component GetComponent(GameObject go, string name)
	{
		Type type = Type.GetType(name);
		Component component;
		if (type != null)
		{
			component = go.GetComponent(type);
		}
		else
		{
			component = go.GetComponent(name);
		}
		return component;
	}

	// Token: 0x0600601C RID: 24604 RVA: 0x0023B2B4 File Offset: 0x002394B4
	private static bool IsExcludedPrefabTag(GameObject go, Tag[] excluded_tags)
	{
		if (excluded_tags == null || excluded_tags.Length == 0)
		{
			return false;
		}
		bool result = false;
		KPrefabID component = go.GetComponent<KPrefabID>();
		foreach (Tag b in excluded_tags)
		{
			if (component.PrefabTag == b)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600601D RID: 24605 RVA: 0x0023B2FC File Offset: 0x002394FC
	private string CodexEntryButton_GetCodexId()
	{
		string text = "";
		global::Debug.Assert(this.target != null, "Details Screen has no target");
		KSelectable component = this.target.GetComponent<KSelectable>();
		DebugUtil.AssertArgs(component != null, new object[]
		{
			"Details Screen target is not a KSelectable",
			this.target
		});
		CellSelectionObject component2 = component.GetComponent<CellSelectionObject>();
		BuildingUnderConstruction component3 = component.GetComponent<BuildingUnderConstruction>();
		CreatureBrain component4 = component.GetComponent<CreatureBrain>();
		PlantableSeed component5 = component.GetComponent<PlantableSeed>();
		BudUprootedMonitor component6 = component.GetComponent<BudUprootedMonitor>();
		if (component2 != null)
		{
			text = CodexCache.FormatLinkID(component2.element.id.ToString());
		}
		else if (component3 != null)
		{
			text = CodexCache.FormatLinkID(component3.Def.PrefabID);
		}
		else if (component4 != null)
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			text = text.Replace("BABY", "");
		}
		else if (component5 != null)
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			text = text.Replace("SEED", "");
		}
		else if (component6 != null)
		{
			if (component6.parentObject.Get() != null)
			{
				text = CodexCache.FormatLinkID(component6.parentObject.Get().PrefabID().ToString());
			}
			else if (component6.GetComponent<TreeBud>() != null)
			{
				text = CodexCache.FormatLinkID(component6.GetComponent<TreeBud>().buddingTrunk.Get().PrefabID().ToString());
			}
		}
		else
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
		}
		if (CodexCache.entries.ContainsKey(text) || CodexCache.FindSubEntry(text) != null)
		{
			return text;
		}
		return "";
	}

	// Token: 0x0600601E RID: 24606 RVA: 0x0023B4F4 File Offset: 0x002396F4
	private void CodexEntryButton_Refresh()
	{
		string a = this.CodexEntryButton_GetCodexId();
		this.CodexEntryButton.isInteractable = (a != "");
		this.CodexEntryButton.GetComponent<ToolTip>().SetSimpleTooltip(this.CodexEntryButton.isInteractable ? UI.TOOLTIPS.OPEN_CODEX_ENTRY : UI.TOOLTIPS.NO_CODEX_ENTRY);
	}

	// Token: 0x0600601F RID: 24607 RVA: 0x0023B54C File Offset: 0x0023974C
	public void CodexEntryButton_OnClick()
	{
		string text = this.CodexEntryButton_GetCodexId();
		if (text != "")
		{
			ManagementMenu.Instance.OpenCodexToEntry(text, null);
		}
	}

	// Token: 0x06006020 RID: 24608 RVA: 0x0023B57C File Offset: 0x0023977C
	private bool PinResourceButton_TryGetResourceTagAndProperName(out Tag targetTag, out string targetProperName)
	{
		KPrefabID component = this.target.GetComponent<KPrefabID>();
		if (component != null && DetailsScreen.<PinResourceButton_TryGetResourceTagAndProperName>g__ShouldUse|51_0(component.PrefabTag))
		{
			targetTag = component.PrefabTag;
			targetProperName = component.GetProperName();
			return true;
		}
		CellSelectionObject component2 = this.target.GetComponent<CellSelectionObject>();
		if (component2 != null && DetailsScreen.<PinResourceButton_TryGetResourceTagAndProperName>g__ShouldUse|51_0(component2.element.tag))
		{
			targetTag = component2.element.tag;
			targetProperName = component2.GetProperName();
			return true;
		}
		targetTag = null;
		targetProperName = null;
		return false;
	}

	// Token: 0x06006021 RID: 24609 RVA: 0x0023B614 File Offset: 0x00239814
	private void PinResourceButton_Refresh()
	{
		Tag tag;
		string arg;
		if (this.PinResourceButton_TryGetResourceTagAndProperName(out tag, out arg))
		{
			ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(tag);
			GameUtil.MeasureUnit measureUnit;
			if (!AllResourcesScreen.Instance.units.TryGetValue(tag, out measureUnit))
			{
				measureUnit = GameUtil.MeasureUnit.quantity;
			}
			string arg2;
			switch (measureUnit)
			{
			case GameUtil.MeasureUnit.mass:
				arg2 = GameUtil.GetFormattedMass(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, false), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
				break;
			case GameUtil.MeasureUnit.kcal:
				arg2 = GameUtil.GetFormattedCalories(WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(tag.Name, ClusterManager.Instance.activeWorld.worldInventory, true), GameUtil.TimeSlice.None, true);
				break;
			case GameUtil.MeasureUnit.quantity:
				arg2 = GameUtil.GetFormattedUnits(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, false), GameUtil.TimeSlice.None, true, "");
				break;
			default:
				arg2 = "";
				break;
			}
			this.PinResourceButton.gameObject.SetActive(true);
			this.PinResourceButton.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.TOOLTIPS.OPEN_RESOURCE_INFO, arg2, arg));
			return;
		}
		this.PinResourceButton.gameObject.SetActive(false);
	}

	// Token: 0x06006022 RID: 24610 RVA: 0x0023B738 File Offset: 0x00239938
	public void PinResourceButton_OnClick()
	{
		Tag tag;
		string text;
		if (this.PinResourceButton_TryGetResourceTagAndProperName(out tag, out text))
		{
			AllResourcesScreen.Instance.SetFilter(UI.StripLinkFormatting(text));
			AllResourcesScreen.Instance.Show(true);
		}
	}

	// Token: 0x06006023 RID: 24611 RVA: 0x0023B76C File Offset: 0x0023996C
	public void OnRefreshData(object obj)
	{
		this.RefreshTitle();
		for (int i = 0; i < this.tabs.Count; i++)
		{
			if (this.tabs[i].gameObject.activeInHierarchy)
			{
				this.tabs[i].Trigger(-1514841199, obj);
			}
		}
	}

	// Token: 0x06006024 RID: 24612 RVA: 0x0023B7C4 File Offset: 0x002399C4
	public void Refresh(GameObject go)
	{
		if (this.screens == null)
		{
			return;
		}
		if (this.target != go)
		{
			if (this.setRocketTitleHandle != -1)
			{
				this.target.Unsubscribe(this.setRocketTitleHandle);
				this.setRocketTitleHandle = -1;
			}
			if (this.target != null)
			{
				if (this.target.GetComponent<KPrefabID>() != null)
				{
					this.previousTargetID = this.target.GetComponent<KPrefabID>().PrefabID();
				}
				else
				{
					this.previousTargetID = null;
				}
			}
		}
		this.target = go;
		this.sortedSideScreens.Clear();
		CellSelectionObject component = this.target.GetComponent<CellSelectionObject>();
		if (component)
		{
			component.OnObjectSelected(null);
		}
		this.UpdateTitle();
		this.tabHeader.RefreshTabDisplayForTarget(this.target);
		if (this.sideScreens != null && this.sideScreens.Count > 0)
		{
			bool flag = false;
			foreach (DetailsScreen.SideScreenRef sideScreenRef in this.sideScreens)
			{
				if (!sideScreenRef.screenPrefab.IsValidForTarget(this.target))
				{
					if (sideScreenRef.screenInstance != null && sideScreenRef.screenInstance.gameObject.activeSelf)
					{
						sideScreenRef.screenInstance.gameObject.SetActive(false);
					}
				}
				else
				{
					flag = true;
					if (sideScreenRef.screenInstance == null)
					{
						DetailsScreen.SidescreenTab tabOfType = this.GetTabOfType(sideScreenRef.tab);
						sideScreenRef.screenInstance = global::Util.KInstantiateUI<SideScreenContent>(sideScreenRef.screenPrefab.gameObject, tabOfType.bodyInstance, false);
					}
					if (!this.sideScreen.activeSelf)
					{
						this.sideScreen.SetActive(true);
					}
					sideScreenRef.screenInstance.SetTarget(this.target);
					sideScreenRef.screenInstance.Show(true);
					int sideScreenSortOrder = sideScreenRef.screenInstance.GetSideScreenSortOrder();
					this.sortedSideScreens.Add(new KeyValuePair<DetailsScreen.SideScreenRef, int>(sideScreenRef, sideScreenSortOrder));
				}
			}
			if (!flag)
			{
				if (!this.CanObjectDisplayTabOfType(this.target, DetailsScreen.SidescreenTabTypes.Material) && !this.CanObjectDisplayTabOfType(this.target, DetailsScreen.SidescreenTabTypes.Blueprints))
				{
					this.sideScreen.SetActive(false);
				}
				else
				{
					this.sideScreen.SetActive(true);
				}
			}
		}
		this.sortedSideScreens.Sort(delegate(KeyValuePair<DetailsScreen.SideScreenRef, int> x, KeyValuePair<DetailsScreen.SideScreenRef, int> y)
		{
			if (x.Value <= y.Value)
			{
				return 1;
			}
			return -1;
		});
		for (int i = 0; i < this.sortedSideScreens.Count; i++)
		{
			this.sortedSideScreens[i].Key.screenInstance.transform.SetSiblingIndex(i);
		}
		for (int j = 0; j < this.sidescreenTabs.Length; j++)
		{
			DetailsScreen.SidescreenTab tab = this.sidescreenTabs[j];
			tab.RepositionTitle();
			KeyValuePair<DetailsScreen.SideScreenRef, int> keyValuePair = this.sortedSideScreens.Find((KeyValuePair<DetailsScreen.SideScreenRef, int> t) => t.Key.tab == tab.type);
			tab.SetNoConfigMessageVisibility(keyValuePair.Key == null);
		}
		this.RefreshTitle();
	}

	// Token: 0x06006025 RID: 24613 RVA: 0x0023BAF0 File Offset: 0x00239CF0
	public void RefreshTitle()
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab tab = this.sidescreenTabs[i];
			if (tab.IsVisible)
			{
				KeyValuePair<DetailsScreen.SideScreenRef, int> keyValuePair = this.sortedSideScreens.Find((KeyValuePair<DetailsScreen.SideScreenRef, int> match) => match.Key.tab == tab.type);
				if (keyValuePair.Key != null)
				{
					tab.SetTitleVisibility(true);
					tab.SetTitle(keyValuePair.Key.screenInstance.GetTitle());
				}
				else
				{
					tab.SetTitle(UI.UISIDESCREENS.NOCONFIG.TITLE);
					tab.SetTitleVisibility(tab.type == DetailsScreen.SidescreenTabTypes.Config || tab.type == DetailsScreen.SidescreenTabTypes.Errands);
				}
			}
		}
	}

	// Token: 0x06006026 RID: 24614 RVA: 0x0023BBC3 File Offset: 0x00239DC3
	private void SelectSideScreenTab(DetailsScreen.SidescreenTabTypes tabID)
	{
		this.selectedSidescreenTabID = tabID;
		this.RefreshSideScreenTabs();
	}

	// Token: 0x06006027 RID: 24615 RVA: 0x0023BBD4 File Offset: 0x00239DD4
	private void RefreshSideScreenTabs()
	{
		int num = 1;
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			bool flag = sidescreenTab.ValidateTarget(this.target);
			sidescreenTab.SetVisible(flag);
			sidescreenTab.SetSelected(this.selectedSidescreenTabID == sidescreenTab.type);
			num += (flag ? 1 : 0);
		}
		this.RefreshTitle();
		DetailsScreen.SidescreenTabTypes sidescreenTabTypes = this.selectedSidescreenTabID;
		if (sidescreenTabTypes != DetailsScreen.SidescreenTabTypes.Material)
		{
			if (sidescreenTabTypes == DetailsScreen.SidescreenTabTypes.Blueprints)
			{
				CosmeticsPanel reference = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Blueprints).bodyInstance.GetComponent<HierarchyReferences>().GetReference<CosmeticsPanel>("CosmeticsPanel");
				reference.SetTarget(this.target);
				reference.Refresh();
			}
		}
		else
		{
			this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Material).bodyInstance.GetComponentInChildren<DetailsScreenMaterialPanel>().SetTarget(this.target);
		}
		this.sidescreenTabHeader.SetActive(num > 1);
	}

	// Token: 0x06006028 RID: 24616 RVA: 0x0023BCA4 File Offset: 0x00239EA4
	public KScreen SetSecondarySideScreen(KScreen secondaryPrefab, string title)
	{
		this.ClearSecondarySideScreen();
		if (this.instantiatedSecondarySideScreens.ContainsKey(secondaryPrefab))
		{
			this.activeSideScreen2 = this.instantiatedSecondarySideScreens[secondaryPrefab];
			this.activeSideScreen2.gameObject.SetActive(true);
		}
		else
		{
			this.activeSideScreen2 = KScreenManager.Instance.InstantiateScreen(secondaryPrefab.gameObject, this.sideScreen2ContentBody);
			this.activeSideScreen2.Activate();
			this.instantiatedSecondarySideScreens.Add(secondaryPrefab, this.activeSideScreen2);
		}
		this.sideScreen2Title.text = title;
		this.sideScreen2.SetActive(true);
		return this.activeSideScreen2;
	}

	// Token: 0x06006029 RID: 24617 RVA: 0x0023BD41 File Offset: 0x00239F41
	public void ClearSecondarySideScreen()
	{
		if (this.activeSideScreen2 != null)
		{
			this.activeSideScreen2.gameObject.SetActive(false);
			this.activeSideScreen2 = null;
		}
		this.sideScreen2.SetActive(false);
	}

	// Token: 0x0600602A RID: 24618 RVA: 0x0023BD78 File Offset: 0x00239F78
	public void DeactivateSideContent()
	{
		if (SideDetailsScreen.Instance != null && SideDetailsScreen.Instance.gameObject.activeInHierarchy)
		{
			SideDetailsScreen.Instance.Show(false);
		}
		if (this.sideScreens != null && this.sideScreens.Count > 0)
		{
			this.sideScreens.ForEach(delegate(DetailsScreen.SideScreenRef scn)
			{
				if (scn.screenInstance != null)
				{
					scn.screenInstance.ClearTarget();
					scn.screenInstance.Show(false);
				}
			});
		}
		DetailsScreen.SidescreenTab tabOfType = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Material);
		DetailsScreen.SidescreenTab tabOfType2 = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Blueprints);
		tabOfType.bodyInstance.GetComponentInChildren<DetailsScreenMaterialPanel>().SetTarget(null);
		tabOfType2.bodyInstance.GetComponentInChildren<CosmeticsPanel>().SetTarget(null);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenHalfEffect, STOP_MODE.ALLOWFADEOUT);
		this.sideScreen.SetActive(false);
	}

	// Token: 0x0600602B RID: 24619 RVA: 0x0023BE40 File Offset: 0x0023A040
	public void MaskSideContent(bool hide)
	{
		if (hide)
		{
			this.sideScreen.transform.localScale = Vector3.zero;
			return;
		}
		this.sideScreen.transform.localScale = Vector3.one;
	}

	// Token: 0x0600602C RID: 24620 RVA: 0x0023BE70 File Offset: 0x0023A070
	public void DeselectAndClose()
	{
		if (base.gameObject.activeInHierarchy)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back", false));
		}
		if (this.GetActiveTab() != null)
		{
			this.GetActiveTab().SetTarget(null);
		}
		SelectTool.Instance.Select(null, false);
		ClusterMapSelectTool.Instance.Select(null, false);
		if (this.target == null)
		{
			return;
		}
		this.target = null;
		this.previousTargetID = null;
		this.DeactivateSideContent();
		this.Show(false);
	}

	// Token: 0x0600602D RID: 24621 RVA: 0x0023BEFB File Offset: 0x0023A0FB
	private void SortScreenOrder()
	{
		Array.Sort<DetailsScreen.Screens>(this.screens, (DetailsScreen.Screens x, DetailsScreen.Screens y) => x.displayOrderPriority.CompareTo(y.displayOrderPriority));
	}

	// Token: 0x0600602E RID: 24622 RVA: 0x0023BF28 File Offset: 0x0023A128
	public void UpdatePortrait(GameObject target)
	{
		KSelectable component = target.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		this.TabTitle.portrait.ClearPortrait();
		Building component2 = component.GetComponent<Building>();
		if (component2)
		{
			Sprite uisprite = component2.Def.GetUISprite("ui", false);
			if (uisprite != null)
			{
				this.TabTitle.portrait.SetPortrait(uisprite);
				return;
			}
		}
		if (target.GetComponent<MinionIdentity>())
		{
			this.TabTitle.SetPortrait(component.gameObject);
			return;
		}
		Edible component3 = target.GetComponent<Edible>();
		if (component3 != null)
		{
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(component3.GetComponent<KBatchedAnimController>().AnimFiles[0], "ui", false, "");
			this.TabTitle.portrait.SetPortrait(uispriteFromMultiObjectAnim);
			return;
		}
		PrimaryElement component4 = target.GetComponent<PrimaryElement>();
		if (component4 != null)
		{
			this.TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(ElementLoader.FindElementByHash(component4.ElementID).substance.anim, "ui", false, ""));
			return;
		}
		CellSelectionObject component5 = target.GetComponent<CellSelectionObject>();
		if (component5 != null)
		{
			string animName = component5.element.IsSolid ? "ui" : component5.element.substance.name;
			Sprite uispriteFromMultiObjectAnim2 = Def.GetUISpriteFromMultiObjectAnim(component5.element.substance.anim, animName, false, "");
			this.TabTitle.portrait.SetPortrait(uispriteFromMultiObjectAnim2);
			return;
		}
	}

	// Token: 0x0600602F RID: 24623 RVA: 0x0023C0AC File Offset: 0x0023A2AC
	public bool CompareTargetWith(GameObject compare)
	{
		return this.target == compare;
	}

	// Token: 0x06006030 RID: 24624 RVA: 0x0023C0BC File Offset: 0x0023A2BC
	public void UpdateTitle()
	{
		this.CodexEntryButton_Refresh();
		this.PinResourceButton_Refresh();
		this.TabTitle.SetTitle(this.target.GetProperName());
		if (this.TabTitle != null)
		{
			this.TabTitle.SetTitle(this.target.GetProperName());
			MinionIdentity minionIdentity = null;
			UserNameable x = null;
			ClustercraftExteriorDoor clustercraftExteriorDoor = null;
			CommandModule commandModule = null;
			if (this.target != null)
			{
				minionIdentity = this.target.gameObject.GetComponent<MinionIdentity>();
				x = this.target.gameObject.GetComponent<UserNameable>();
				clustercraftExteriorDoor = this.target.gameObject.GetComponent<ClustercraftExteriorDoor>();
				commandModule = this.target.gameObject.GetComponent<CommandModule>();
			}
			if (minionIdentity != null)
			{
				this.TabTitle.SetSubText(minionIdentity.GetComponent<MinionResume>().GetSkillsSubtitle(), "");
				this.TabTitle.SetUserEditable(true);
			}
			else if (x != null)
			{
				this.TabTitle.SetSubText("", "");
				this.TabTitle.SetUserEditable(true);
			}
			else if (commandModule != null)
			{
				this.TrySetRocketTitle(commandModule);
			}
			else if (clustercraftExteriorDoor != null)
			{
				this.TrySetRocketTitle(clustercraftExteriorDoor);
			}
			else
			{
				this.TabTitle.SetSubText("", "");
				this.TabTitle.SetUserEditable(false);
			}
			this.TabTitle.UpdateRenameTooltip(this.target);
		}
	}

	// Token: 0x06006031 RID: 24625 RVA: 0x0023C220 File Offset: 0x0023A420
	private void TrySetRocketTitle(ClustercraftExteriorDoor clusterCraftDoor)
	{
		if (clusterCraftDoor2.HasTargetWorld())
		{
			WorldContainer targetWorld = clusterCraftDoor2.GetTargetWorld();
			this.TabTitle.SetTitle(targetWorld.GetComponent<ClusterGridEntity>().Name);
			this.TabTitle.SetUserEditable(true);
			this.TabTitle.SetSubText(this.target.GetProperName(), "");
			this.setRocketTitleHandle = -1;
			return;
		}
		if (this.setRocketTitleHandle == -1)
		{
			this.setRocketTitleHandle = this.target.Subscribe(-71801987, delegate(object clusterCraftDoor)
			{
				this.OnRefreshData(null);
				this.target.Unsubscribe(this.setRocketTitleHandle);
				this.setRocketTitleHandle = -1;
			});
		}
	}

	// Token: 0x06006032 RID: 24626 RVA: 0x0023C2AC File Offset: 0x0023A4AC
	private void TrySetRocketTitle(CommandModule commandModule)
	{
		if (commandModule != null)
		{
			this.TabTitle.SetTitle(SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(commandModule.GetComponent<LaunchConditionManager>()).GetRocketName());
			this.TabTitle.SetUserEditable(true);
		}
		this.TabTitle.SetSubText(this.target.GetProperName(), "");
	}

	// Token: 0x06006033 RID: 24627 RVA: 0x0023C309 File Offset: 0x0023A509
	public TargetPanel GetActiveTab()
	{
		return this.tabHeader.ActivePanel;
	}

	// Token: 0x06006037 RID: 24631 RVA: 0x0023C374 File Offset: 0x0023A574
	[CompilerGenerated]
	internal static bool <PinResourceButton_TryGetResourceTagAndProperName>g__ShouldUse|51_0(Tag targetTag)
	{
		foreach (Tag tag in GameTags.MaterialCategories)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag).Contains(targetTag))
			{
				return true;
			}
		}
		foreach (Tag tag2 in GameTags.CalorieCategories)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag2).Contains(targetTag))
			{
				return true;
			}
		}
		foreach (Tag tag3 in GameTags.UnitCategories)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag3).Contains(targetTag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040040D2 RID: 16594
	public static DetailsScreen Instance;

	// Token: 0x040040D3 RID: 16595
	[SerializeField]
	private KButton CodexEntryButton;

	// Token: 0x040040D4 RID: 16596
	[SerializeField]
	private KButton PinResourceButton;

	// Token: 0x040040D5 RID: 16597
	[Header("Panels")]
	public Transform UserMenuPanel;

	// Token: 0x040040D6 RID: 16598
	[Header("Name Editing (disabled)")]
	[SerializeField]
	private KButton CloseButton;

	// Token: 0x040040D7 RID: 16599
	[Header("Tabs")]
	[SerializeField]
	private DetailTabHeader tabHeader;

	// Token: 0x040040D8 RID: 16600
	[SerializeField]
	private EditableTitleBar TabTitle;

	// Token: 0x040040D9 RID: 16601
	[SerializeField]
	private DetailsScreen.Screens[] screens;

	// Token: 0x040040DA RID: 16602
	[SerializeField]
	private GameObject tabHeaderContainer;

	// Token: 0x040040DB RID: 16603
	[Header("Side Screen Tabs")]
	[SerializeField]
	private DetailsScreen.SidescreenTab[] sidescreenTabs;

	// Token: 0x040040DC RID: 16604
	[SerializeField]
	private GameObject sidescreenTabHeader;

	// Token: 0x040040DD RID: 16605
	[SerializeField]
	private GameObject original_tab;

	// Token: 0x040040DE RID: 16606
	[SerializeField]
	private GameObject original_tab_body;

	// Token: 0x040040DF RID: 16607
	[Header("Side Screens")]
	[SerializeField]
	private GameObject sideScreen;

	// Token: 0x040040E0 RID: 16608
	[SerializeField]
	private List<DetailsScreen.SideScreenRef> sideScreens;

	// Token: 0x040040E1 RID: 16609
	[SerializeField]
	private LayoutElement tabBodyLayoutElement;

	// Token: 0x040040E2 RID: 16610
	[Header("Secondary Side Screens")]
	[SerializeField]
	private GameObject sideScreen2ContentBody;

	// Token: 0x040040E3 RID: 16611
	[SerializeField]
	private GameObject sideScreen2;

	// Token: 0x040040E4 RID: 16612
	[SerializeField]
	private LocText sideScreen2Title;

	// Token: 0x040040E5 RID: 16613
	private KScreen activeSideScreen2;

	// Token: 0x040040E7 RID: 16615
	private Tag previousTargetID = null;

	// Token: 0x040040E8 RID: 16616
	private bool HasActivated;

	// Token: 0x040040E9 RID: 16617
	private DetailsScreen.SidescreenTabTypes selectedSidescreenTabID;

	// Token: 0x040040EA RID: 16618
	private Dictionary<KScreen, KScreen> instantiatedSecondarySideScreens = new Dictionary<KScreen, KScreen>();

	// Token: 0x040040EB RID: 16619
	private static readonly EventSystem.IntraObjectHandler<DetailsScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<DetailsScreen>(delegate(DetailsScreen component, object data)
	{
		component.OnRefreshData(data);
	});

	// Token: 0x040040EC RID: 16620
	private List<KeyValuePair<DetailsScreen.SideScreenRef, int>> sortedSideScreens = new List<KeyValuePair<DetailsScreen.SideScreenRef, int>>();

	// Token: 0x040040ED RID: 16621
	private int setRocketTitleHandle = -1;

	// Token: 0x02001D18 RID: 7448
	[Serializable]
	private struct Screens
	{
		// Token: 0x040085F4 RID: 34292
		public string name;

		// Token: 0x040085F5 RID: 34293
		public string displayName;

		// Token: 0x040085F6 RID: 34294
		public string tooltip;

		// Token: 0x040085F7 RID: 34295
		public Sprite icon;

		// Token: 0x040085F8 RID: 34296
		public TargetPanel screen;

		// Token: 0x040085F9 RID: 34297
		public int displayOrderPriority;

		// Token: 0x040085FA RID: 34298
		public bool hideWhenDead;

		// Token: 0x040085FB RID: 34299
		public HashedString focusInViewMode;

		// Token: 0x040085FC RID: 34300
		[HideInInspector]
		public int tabIdx;
	}

	// Token: 0x02001D19 RID: 7449
	public enum SidescreenTabTypes
	{
		// Token: 0x040085FE RID: 34302
		Config,
		// Token: 0x040085FF RID: 34303
		Errands,
		// Token: 0x04008600 RID: 34304
		Material,
		// Token: 0x04008601 RID: 34305
		Blueprints
	}

	// Token: 0x02001D1A RID: 7450
	[Serializable]
	public class SidescreenTab
	{
		// Token: 0x0600A7A2 RID: 42914 RVA: 0x0039AF36 File Offset: 0x00399136
		private void OnTabClicked()
		{
			System.Action onClicked = this.OnClicked;
			if (onClicked == null)
			{
				return;
			}
			onClicked();
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x0600A7A4 RID: 42916 RVA: 0x0039AF51 File Offset: 0x00399151
		// (set) Token: 0x0600A7A3 RID: 42915 RVA: 0x0039AF48 File Offset: 0x00399148
		public bool IsVisible { get; private set; }

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x0600A7A6 RID: 42918 RVA: 0x0039AF62 File Offset: 0x00399162
		// (set) Token: 0x0600A7A5 RID: 42917 RVA: 0x0039AF59 File Offset: 0x00399159
		public bool IsSelected { get; private set; }

		// Token: 0x0600A7A7 RID: 42919 RVA: 0x0039AF6C File Offset: 0x0039916C
		public void Initiate(GameObject originalTabInstance, GameObject originalBodyInstance, Action<DetailsScreen.SidescreenTab> on_tab_clicked_callback)
		{
			if (on_tab_clicked_callback != null)
			{
				this.OnClicked = delegate()
				{
					on_tab_clicked_callback(this);
				};
			}
			originalBodyInstance.gameObject.SetActive(false);
			if (this.OverrideBody == null)
			{
				this.bodyInstance = UnityEngine.Object.Instantiate<GameObject>(originalBodyInstance);
				this.bodyInstance.name = this.type.ToString() + " Tab - body instance";
				this.bodyInstance.SetActive(true);
				this.bodyInstance.transform.SetParent(originalBodyInstance.transform.parent, false);
			}
			else
			{
				this.bodyInstance = this.OverrideBody;
			}
			this.bodyReferences = this.bodyInstance.GetComponent<HierarchyReferences>();
			originalTabInstance.gameObject.SetActive(false);
			if (this.tabInstance == null)
			{
				this.tabInstance = UnityEngine.Object.Instantiate<GameObject>(originalTabInstance.gameObject).GetComponent<MultiToggle>();
				this.tabInstance.name = this.type.ToString() + " Tab Instance";
				this.tabInstance.gameObject.SetActive(true);
				this.tabInstance.transform.SetParent(originalTabInstance.transform.parent, false);
				MultiToggle multiToggle = this.tabInstance;
				multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnTabClicked));
				HierarchyReferences component = this.tabInstance.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("label").SetText(Strings.Get(this.Title_Key));
				component.GetReference<Image>("icon").sprite = this.Icon;
				this.tabInstance.GetComponent<ToolTip>().SetSimpleTooltip(Strings.Get(this.Tooltip_Key));
			}
		}

		// Token: 0x0600A7A8 RID: 42920 RVA: 0x0039B147 File Offset: 0x00399347
		public void SetSelected(bool isSelected)
		{
			this.IsSelected = isSelected;
			this.tabInstance.ChangeState(isSelected ? 1 : 0);
			this.bodyInstance.SetActive(isSelected);
		}

		// Token: 0x0600A7A9 RID: 42921 RVA: 0x0039B16E File Offset: 0x0039936E
		public void SetTitle(string title)
		{
			if (this.bodyReferences != null && this.bodyReferences.HasReference("TitleLabel"))
			{
				this.bodyReferences.GetReference<LocText>("TitleLabel").SetText(title);
			}
		}

		// Token: 0x0600A7AA RID: 42922 RVA: 0x0039B1A6 File Offset: 0x003993A6
		public void SetTitleVisibility(bool visible)
		{
			if (this.bodyReferences != null && this.bodyReferences.HasReference("Title"))
			{
				this.bodyReferences.GetReference("Title").gameObject.SetActive(visible);
			}
		}

		// Token: 0x0600A7AB RID: 42923 RVA: 0x0039B1E3 File Offset: 0x003993E3
		public void SetNoConfigMessageVisibility(bool visible)
		{
			if (this.bodyReferences != null && this.bodyReferences.HasReference("NoConfigMessage"))
			{
				this.bodyReferences.GetReference("NoConfigMessage").gameObject.SetActive(visible);
			}
		}

		// Token: 0x0600A7AC RID: 42924 RVA: 0x0039B220 File Offset: 0x00399420
		public void RepositionTitle()
		{
			if (this.bodyReferences != null && this.bodyReferences.GetReference("Title") != null)
			{
				this.bodyReferences.GetReference("Title").transform.SetSiblingIndex(0);
			}
		}

		// Token: 0x0600A7AD RID: 42925 RVA: 0x0039B26E File Offset: 0x0039946E
		public void SetVisible(bool visible)
		{
			this.IsVisible = visible;
			this.tabInstance.gameObject.SetActive(visible);
			this.bodyInstance.SetActive(this.IsSelected && this.IsVisible);
		}

		// Token: 0x0600A7AE RID: 42926 RVA: 0x0039B2A4 File Offset: 0x003994A4
		public bool ValidateTarget(GameObject target)
		{
			return !(target == null) && (this.ValidateTargetCallback == null || this.ValidateTargetCallback(target, this));
		}

		// Token: 0x04008602 RID: 34306
		public DetailsScreen.SidescreenTabTypes type;

		// Token: 0x04008603 RID: 34307
		public string Title_Key;

		// Token: 0x04008604 RID: 34308
		public string Tooltip_Key;

		// Token: 0x04008605 RID: 34309
		public Sprite Icon;

		// Token: 0x04008606 RID: 34310
		public GameObject OverrideBody;

		// Token: 0x04008607 RID: 34311
		public Func<GameObject, DetailsScreen.SidescreenTab, bool> ValidateTargetCallback;

		// Token: 0x04008608 RID: 34312
		public System.Action OnClicked;

		// Token: 0x0400860B RID: 34315
		[NonSerialized]
		public MultiToggle tabInstance;

		// Token: 0x0400860C RID: 34316
		[NonSerialized]
		public GameObject bodyInstance;

		// Token: 0x0400860D RID: 34317
		private HierarchyReferences bodyReferences;

		// Token: 0x0400860E RID: 34318
		private const string bodyRef_Title = "Title";

		// Token: 0x0400860F RID: 34319
		private const string bodyRef_TitleLabel = "TitleLabel";

		// Token: 0x04008610 RID: 34320
		private const string bodyRef_NoConfigMessage = "NoConfigMessage";
	}

	// Token: 0x02001D1B RID: 7451
	[Serializable]
	public class SideScreenRef
	{
		// Token: 0x04008611 RID: 34321
		public string name;

		// Token: 0x04008612 RID: 34322
		public SideScreenContent screenPrefab;

		// Token: 0x04008613 RID: 34323
		public Vector2 offset;

		// Token: 0x04008614 RID: 34324
		public DetailsScreen.SidescreenTabTypes tab;

		// Token: 0x04008615 RID: 34325
		[HideInInspector]
		public SideScreenContent screenInstance;
	}
}
