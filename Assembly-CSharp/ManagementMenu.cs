using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000BA0 RID: 2976
public class ManagementMenu : KIconToggleMenu
{
	// Token: 0x06005A03 RID: 23043 RVA: 0x00209C55 File Offset: 0x00207E55
	public static void DestroyInstance()
	{
		ManagementMenu.Instance = null;
	}

	// Token: 0x06005A04 RID: 23044 RVA: 0x00209C5D File Offset: 0x00207E5D
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x06005A05 RID: 23045 RVA: 0x00209C64 File Offset: 0x00207E64
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ManagementMenu.Instance = this;
		this.notificationDisplayer.onNotificationsChanged += this.OnNotificationsChanged;
		CodexCache.CodexCacheInit();
		ScheduledUIInstantiation component = GameScreenManager.Instance.GetComponent<ScheduledUIInstantiation>();
		this.starmapScreen = component.GetInstantiatedObject<StarmapScreen>();
		this.clusterMapScreen = component.GetInstantiatedObject<ClusterMapScreen>();
		this.skillsScreen = component.GetInstantiatedObject<SkillsScreen>();
		this.researchScreen = component.GetInstantiatedObject<ResearchScreen>();
		this.fullscreenUIs = new ManagementMenu.ManagementMenuToggleInfo[]
		{
			this.researchInfo,
			this.skillsInfo,
			this.starmapInfo,
			this.clusterMapInfo
		};
		base.Subscribe(Game.Instance.gameObject, 288942073, new Action<object>(this.OnUIClear));
		this.consumablesInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.CONSUMABLES, "OverviewUI_consumables_icon", null, global::Action.ManageConsumables, UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES, "");
		this.AddToggleTooltip(this.consumablesInfo, null);
		this.vitalsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.VITALS, "OverviewUI_vitals_icon", null, global::Action.ManageVitals, UI.TOOLTIPS.MANAGEMENTMENU_VITALS, "");
		this.AddToggleTooltip(this.vitalsInfo, null);
		this.researchInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.RESEARCH, "OverviewUI_research_nav_icon", null, global::Action.ManageResearch, UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, "");
		this.AddToggleTooltipForResearch(this.researchInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_RESEARCH);
		this.researchInfo.prefabOverride = this.researchButtonPrefab;
		this.jobsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.JOBS, "OverviewUI_priority_icon", null, global::Action.ManagePriorities, UI.TOOLTIPS.MANAGEMENTMENU_JOBS, "");
		this.AddToggleTooltip(this.jobsInfo, null);
		this.skillsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.SKILLS, "OverviewUI_jobs_icon", null, global::Action.ManageSkills, UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, "");
		this.AddToggleTooltip(this.skillsInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_SKILL_STATION);
		this.starmapInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", null, global::Action.ManageStarmap, UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, "");
		this.AddToggleTooltip(this.starmapInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_TELESCOPE);
		this.clusterMapInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", null, global::Action.ManageStarmap, UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, "");
		this.AddToggleTooltip(this.clusterMapInfo, null);
		this.scheduleInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.SCHEDULE, "OverviewUI_schedule2_icon", null, global::Action.ManageSchedule, UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE, "");
		this.AddToggleTooltip(this.scheduleInfo, null);
		this.reportsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.REPORT, "OverviewUI_reports_icon", null, global::Action.ManageReport, UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT, "");
		this.AddToggleTooltip(this.reportsInfo, null);
		this.reportsInfo.prefabOverride = this.smallPrefab;
		this.codexInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.CODEX.MANAGEMENT_BUTTON, "OverviewUI_database_icon", null, global::Action.ManageDatabase, UI.TOOLTIPS.MANAGEMENTMENU_CODEX, "");
		this.AddToggleTooltip(this.codexInfo, null);
		this.codexInfo.prefabOverride = this.smallPrefab;
		this.ScreenInfoMatch.Add(this.consumablesInfo, new ManagementMenu.ScreenData
		{
			screen = this.consumablesScreen,
			tabIdx = 3,
			toggleInfo = this.consumablesInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.vitalsInfo, new ManagementMenu.ScreenData
		{
			screen = this.vitalsScreen,
			tabIdx = 2,
			toggleInfo = this.vitalsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.reportsInfo, new ManagementMenu.ScreenData
		{
			screen = this.reportsScreen,
			tabIdx = 4,
			toggleInfo = this.reportsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.jobsInfo, new ManagementMenu.ScreenData
		{
			screen = this.jobsScreen,
			tabIdx = 1,
			toggleInfo = this.jobsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.skillsInfo, new ManagementMenu.ScreenData
		{
			screen = this.skillsScreen,
			tabIdx = 0,
			toggleInfo = this.skillsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.codexInfo, new ManagementMenu.ScreenData
		{
			screen = this.codexScreen,
			tabIdx = 6,
			toggleInfo = this.codexInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.scheduleInfo, new ManagementMenu.ScreenData
		{
			screen = this.scheduleScreen,
			tabIdx = 7,
			toggleInfo = this.scheduleInfo,
			cancelHandler = null
		});
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			this.ScreenInfoMatch.Add(this.clusterMapInfo, new ManagementMenu.ScreenData
			{
				screen = this.clusterMapScreen,
				tabIdx = 7,
				toggleInfo = this.clusterMapInfo,
				cancelHandler = new Func<bool>(this.clusterMapScreen.TryHandleCancel)
			});
		}
		else
		{
			this.ScreenInfoMatch.Add(this.starmapInfo, new ManagementMenu.ScreenData
			{
				screen = this.starmapScreen,
				tabIdx = 7,
				toggleInfo = this.starmapInfo,
				cancelHandler = null
			});
		}
		this.ScreenInfoMatch.Add(this.researchInfo, new ManagementMenu.ScreenData
		{
			screen = this.researchScreen,
			tabIdx = 5,
			toggleInfo = this.researchInfo,
			cancelHandler = null
		});
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		list.Add(this.vitalsInfo);
		list.Add(this.consumablesInfo);
		list.Add(this.scheduleInfo);
		list.Add(this.jobsInfo);
		list.Add(this.skillsInfo);
		list.Add(this.researchInfo);
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(this.clusterMapInfo);
		}
		else
		{
			list.Add(this.starmapInfo);
		}
		list.Add(this.reportsInfo);
		list.Add(this.codexInfo);
		base.Setup(list);
		base.onSelect += this.OnButtonClick;
		this.PauseMenuButton.onClick += this.OnPauseMenuClicked;
		this.PauseMenuButton.transform.SetAsLastSibling();
		this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, global::Action.Escape);
		KInputManager.InputChange.AddListener(new UnityAction(this.OnInputChanged));
		Components.ResearchCenters.OnAdd += new Action<IResearchCenter>(this.CheckResearch);
		Components.ResearchCenters.OnRemove += new Action<IResearchCenter>(this.CheckResearch);
		Components.RoleStations.OnAdd += new Action<RoleStation>(this.CheckSkills);
		Components.RoleStations.OnRemove += new Action<RoleStation>(this.CheckSkills);
		Game.Instance.Subscribe(-809948329, new Action<object>(this.CheckResearch));
		Game.Instance.Subscribe(-809948329, new Action<object>(this.CheckSkills));
		Game.Instance.Subscribe(445618876, new Action<object>(this.OnResolutionChanged));
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			Components.Telescopes.OnAdd += new Action<Telescope>(this.CheckStarmap);
			Components.Telescopes.OnRemove += new Action<Telescope>(this.CheckStarmap);
		}
		this.CheckResearch(null);
		this.CheckSkills(null);
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.CheckStarmap(null);
		}
		this.researchInfo.toggle.soundPlayer.AcceptClickCondition = (() => this.ResearchAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.soundPlayer.toggle_widget_sound_events[0].PlaySound = false;
			ktoggle.soundPlayer.toggle_widget_sound_events[1].PlaySound = false;
		}
		this.OnResolutionChanged(null);
	}

	// Token: 0x06005A06 RID: 23046 RVA: 0x0020A4AC File Offset: 0x002086AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.mutuallyExclusiveScreens.Add(AllResourcesScreen.Instance);
		this.mutuallyExclusiveScreens.Add(AllDiagnosticsScreen.Instance);
		this.OnNotificationsChanged();
	}

	// Token: 0x06005A07 RID: 23047 RVA: 0x0020A4DA File Offset: 0x002086DA
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.OnInputChanged));
		base.OnForcedCleanUp();
	}

	// Token: 0x06005A08 RID: 23048 RVA: 0x0020A4F8 File Offset: 0x002086F8
	private void OnInputChanged()
	{
		this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, global::Action.Escape);
		this.consumablesInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES, this.consumablesInfo.hotKey);
		this.vitalsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_VITALS, this.vitalsInfo.hotKey);
		this.researchInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, this.researchInfo.hotKey);
		this.jobsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_JOBS, this.jobsInfo.hotKey);
		this.skillsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, this.skillsInfo.hotKey);
		this.starmapInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, this.starmapInfo.hotKey);
		this.clusterMapInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, this.clusterMapInfo.hotKey);
		this.scheduleInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE, this.scheduleInfo.hotKey);
		this.reportsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT, this.reportsInfo.hotKey);
		this.codexInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_CODEX, this.codexInfo.hotKey);
	}

	// Token: 0x06005A09 RID: 23049 RVA: 0x0020A698 File Offset: 0x00208898
	private void OnResolutionChanged(object data = null)
	{
		bool flag = (float)Screen.width < 1300f;
		foreach (KToggle ktoggle in this.toggles)
		{
			HierarchyReferences component = ktoggle.GetComponent<HierarchyReferences>();
			if (!(component == null))
			{
				RectTransform reference = component.GetReference<RectTransform>("TextContainer");
				if (!(reference == null))
				{
					reference.gameObject.SetActive(!flag);
				}
			}
		}
	}

	// Token: 0x06005A0A RID: 23050 RVA: 0x0020A724 File Offset: 0x00208924
	private void OnNotificationsChanged()
	{
		foreach (KeyValuePair<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData> keyValuePair in this.ScreenInfoMatch)
		{
			keyValuePair.Key.SetNotificationDisplay(false, false, null, this.noAlertColorStyle);
		}
	}

	// Token: 0x06005A0B RID: 23051 RVA: 0x0020A788 File Offset: 0x00208988
	private ToolTip.ComplexTooltipDelegate CreateToggleTooltip(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		return delegate()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			if (disabledTooltip != null && !toggleInfo.toggle.interactable)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(disabledTooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				return list;
			}
			if (toggleInfo.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		};
	}

	// Token: 0x06005A0C RID: 23052 RVA: 0x0020A7A8 File Offset: 0x002089A8
	private void AddToggleTooltip(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		toggleInfo.getTooltipText = this.CreateToggleTooltip(toggleInfo, disabledTooltip);
	}

	// Token: 0x06005A0D RID: 23053 RVA: 0x0020A7B8 File Offset: 0x002089B8
	private void AddToggleTooltipForResearch(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		toggleInfo.getTooltipText = delegate()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			TechInstance activeResearch = Research.Instance.GetActiveResearch();
			string a = (activeResearch == null) ? UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_NO_RESEARCH : string.Format(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_CARD_NAME, activeResearch.tech.Name);
			list.Add(new global::Tuple<string, TextStyleSetting>(a, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			if (activeResearch != null)
			{
				string text = "";
				for (int i = 0; i < activeResearch.tech.unlockedItems.Count; i++)
				{
					TechItem techItem = activeResearch.tech.unlockedItems[i];
					text = text + "\n" + string.Format(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_ITEM_LINE, techItem.Name);
				}
				list.Add(new global::Tuple<string, TextStyleSetting>(text, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			}
			if (disabledTooltip != null && !toggleInfo.toggle.interactable)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(disabledTooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				return list;
			}
			if (toggleInfo.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>("\n" + toggleInfo.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		};
	}

	// Token: 0x06005A0E RID: 23054 RVA: 0x0020A7F0 File Offset: 0x002089F0
	public bool IsFullscreenUIActive()
	{
		if (this.activeScreen == null)
		{
			return false;
		}
		foreach (ManagementMenu.ManagementMenuToggleInfo managementMenuToggleInfo in this.fullscreenUIs)
		{
			if (this.activeScreen.toggleInfo == managementMenuToggleInfo)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005A0F RID: 23055 RVA: 0x0020A831 File Offset: 0x00208A31
	private void OnPauseMenuClicked()
	{
		PauseScreen.Instance.Show(true);
		this.PauseMenuButton.isOn = false;
	}

	// Token: 0x06005A10 RID: 23056 RVA: 0x0020A84A File Offset: 0x00208A4A
	public void Refresh()
	{
		this.CheckResearch(null);
		this.CheckSkills(null);
		this.CheckStarmap(null);
	}

	// Token: 0x06005A11 RID: 23057 RVA: 0x0020A864 File Offset: 0x00208A64
	public void CheckResearch(object o)
	{
		if (this.researchInfo.toggle == null)
		{
			return;
		}
		bool flag = Components.ResearchCenters.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive;
		bool active = !flag && this.activeScreen != null && this.activeScreen.toggleInfo == this.researchInfo;
		this.ConfigureToggle(this.researchInfo.toggle, flag, active);
	}

	// Token: 0x06005A12 RID: 23058 RVA: 0x0020A8E0 File Offset: 0x00208AE0
	public void CheckSkills(object o = null)
	{
		if (this.skillsInfo.toggle == null)
		{
			return;
		}
		bool disabled = Components.RoleStations.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive;
		bool active = this.activeScreen != null && this.activeScreen.toggleInfo == this.skillsInfo;
		this.ConfigureToggle(this.skillsInfo.toggle, disabled, active);
	}

	// Token: 0x06005A13 RID: 23059 RVA: 0x0020A958 File Offset: 0x00208B58
	public void CheckStarmap(object o = null)
	{
		if (this.starmapInfo.toggle == null)
		{
			return;
		}
		bool disabled = Components.Telescopes.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive;
		bool active = this.activeScreen != null && this.activeScreen.toggleInfo == this.starmapInfo;
		this.ConfigureToggle(this.starmapInfo.toggle, disabled, active);
	}

	// Token: 0x06005A14 RID: 23060 RVA: 0x0020A9D0 File Offset: 0x00208BD0
	private void ConfigureToggle(KToggle toggle, bool disabled, bool active)
	{
		toggle.interactable = !disabled;
		if (disabled)
		{
			toggle.GetComponentInChildren<ImageToggleState>().SetDisabled();
			return;
		}
		toggle.GetComponentInChildren<ImageToggleState>().SetActiveState(active);
	}

	// Token: 0x06005A15 RID: 23061 RVA: 0x0020A9F7 File Offset: 0x00208BF7
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.activeScreen != null && e.TryConsume(global::Action.Escape))
		{
			this.ToggleIfCancelUnhandled(this.activeScreen);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06005A16 RID: 23062 RVA: 0x0020AA25 File Offset: 0x00208C25
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.activeScreen != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.ToggleIfCancelUnhandled(this.activeScreen);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06005A17 RID: 23063 RVA: 0x0020AA58 File Offset: 0x00208C58
	private void ToggleIfCancelUnhandled(ManagementMenu.ScreenData screenData)
	{
		if (screenData.cancelHandler == null || !screenData.cancelHandler())
		{
			this.ToggleScreen(screenData);
		}
	}

	// Token: 0x06005A18 RID: 23064 RVA: 0x0020AA76 File Offset: 0x00208C76
	private bool ResearchAvailable()
	{
		return Components.ResearchCenters.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
	}

	// Token: 0x06005A19 RID: 23065 RVA: 0x0020AA98 File Offset: 0x00208C98
	private bool SkillsAvailable()
	{
		return Components.RoleStations.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
	}

	// Token: 0x06005A1A RID: 23066 RVA: 0x0020AABA File Offset: 0x00208CBA
	public static bool StarmapAvailable()
	{
		return Components.Telescopes.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
	}

	// Token: 0x06005A1B RID: 23067 RVA: 0x0020AADC File Offset: 0x00208CDC
	public void CloseAll()
	{
		if (this.activeScreen == null)
		{
			return;
		}
		if (this.activeScreen.toggleInfo != null)
		{
			this.ToggleScreen(this.activeScreen);
		}
		this.CloseActive();
		this.ClearSelection();
	}

	// Token: 0x06005A1C RID: 23068 RVA: 0x0020AB0C File Offset: 0x00208D0C
	private void OnUIClear(object data)
	{
		this.CloseAll();
	}

	// Token: 0x06005A1D RID: 23069 RVA: 0x0020AB14 File Offset: 0x00208D14
	public void ToggleScreen(ManagementMenu.ScreenData screenData)
	{
		if (screenData == null)
		{
			return;
		}
		if (screenData.toggleInfo == this.researchInfo && !this.ResearchAvailable())
		{
			this.CheckResearch(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo == this.skillsInfo && !this.SkillsAvailable())
		{
			this.CheckSkills(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo == this.starmapInfo && !ManagementMenu.StarmapAvailable())
		{
			this.CheckStarmap(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().IsDisabled)
		{
			return;
		}
		if (this.activeScreen != null)
		{
			this.activeScreen.toggleInfo.toggle.isOn = false;
			this.activeScreen.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
		}
		if (this.activeScreen != screenData)
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			if (this.activeScreen != null)
			{
				this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenMigrated);
			screenData.toggleInfo.toggle.ActivateFlourish(true);
			screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetActive();
			this.CloseActive();
			this.activeScreen = screenData;
			if (!this.activeScreen.screen.IsActive())
			{
				this.activeScreen.screen.Activate();
			}
			this.activeScreen.screen.Show(true);
			foreach (ManagementMenuNotification managementMenuNotification in this.notificationDisplayer.GetNotificationsForAction(screenData.toggleInfo.hotKey))
			{
				if (managementMenuNotification.customClickCallback != null)
				{
					managementMenuNotification.customClickCallback(managementMenuNotification.customClickData);
					break;
				}
			}
			using (List<KScreen>.Enumerator enumerator2 = this.mutuallyExclusiveScreens.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KScreen kscreen = enumerator2.Current;
					kscreen.Show(false);
				}
				return;
			}
		}
		this.activeScreen.screen.Show(false);
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenMigrated, STOP_MODE.ALLOWFADEOUT);
		this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
		this.activeScreen = null;
		screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
	}

	// Token: 0x06005A1E RID: 23070 RVA: 0x0020ADCC File Offset: 0x00208FCC
	public void OnButtonClick(KIconToggleMenu.ToggleInfo toggle_info)
	{
		this.ToggleScreen(this.ScreenInfoMatch[(ManagementMenu.ManagementMenuToggleInfo)toggle_info]);
	}

	// Token: 0x06005A1F RID: 23071 RVA: 0x0020ADE5 File Offset: 0x00208FE5
	private void CloseActive()
	{
		if (this.activeScreen != null)
		{
			this.activeScreen.toggleInfo.toggle.isOn = false;
			this.activeScreen.screen.Show(false);
			this.activeScreen = null;
		}
	}

	// Token: 0x06005A20 RID: 23072 RVA: 0x0020AE20 File Offset: 0x00209020
	public void ToggleResearch()
	{
		if ((this.ResearchAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]) && this.researchInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		}
	}

	// Token: 0x06005A21 RID: 23073 RVA: 0x0020AE75 File Offset: 0x00209075
	public void ToggleCodex()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.codexInfo]);
	}

	// Token: 0x06005A22 RID: 23074 RVA: 0x0020AE94 File Offset: 0x00209094
	public void OpenCodexToLockId(string lockId, bool focusContent = false)
	{
		string entryForLock = CodexCache.GetEntryForLock(lockId);
		if (entryForLock == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Could not open codex to lockId \"" + lockId + "\", couldn't find an entry that contained that lockId"
			});
			return;
		}
		ContentContainer contentContainer = null;
		if (focusContent)
		{
			CodexEntry codexEntry = CodexCache.FindEntry(entryForLock);
			int num = 0;
			while (contentContainer == null && num < codexEntry.contentContainers.Count)
			{
				if (!(codexEntry.contentContainers[num].lockID != lockId))
				{
					contentContainer = codexEntry.contentContainers[num];
				}
				num++;
			}
		}
		this.OpenCodexToEntry(entryForLock, contentContainer);
	}

	// Token: 0x06005A23 RID: 23075 RVA: 0x0020AF20 File Offset: 0x00209120
	public void OpenCodexToEntry(string id, ContentContainer targetContainer = null)
	{
		if (!this.codexScreen.gameObject.activeInHierarchy)
		{
			this.ToggleCodex();
		}
		this.codexScreen.ChangeArticle(id, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		this.codexScreen.FocusContainer(targetContainer);
	}

	// Token: 0x06005A24 RID: 23076 RVA: 0x0020AF68 File Offset: 0x00209168
	public void ToggleSkills()
	{
		if ((this.SkillsAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]) && this.skillsInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
		}
	}

	// Token: 0x06005A25 RID: 23077 RVA: 0x0020AFBD File Offset: 0x002091BD
	public void ToggleStarmap()
	{
		if (this.starmapInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
		}
	}

	// Token: 0x06005A26 RID: 23078 RVA: 0x0020AFE2 File Offset: 0x002091E2
	public void ToggleClusterMap()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
	}

	// Token: 0x06005A27 RID: 23079 RVA: 0x0020AFFF File Offset: 0x002091FF
	public void TogglePriorities()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.jobsInfo]);
	}

	// Token: 0x06005A28 RID: 23080 RVA: 0x0020B01C File Offset: 0x0020921C
	public void OpenReports(int day)
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo]);
		}
		ReportScreen.Instance.ShowReport(day);
	}

	// Token: 0x06005A29 RID: 23081 RVA: 0x0020B06C File Offset: 0x0020926C
	public void OpenResearch(string zoomToTech = null)
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		}
		if (zoomToTech != null)
		{
			this.researchScreen.ZoomToTech(zoomToTech);
		}
	}

	// Token: 0x06005A2A RID: 23082 RVA: 0x0020B0C0 File Offset: 0x002092C0
	public void OpenStarmap()
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
		}
	}

	// Token: 0x06005A2B RID: 23083 RVA: 0x0020B0FA File Offset: 0x002092FA
	public void OpenClusterMap()
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
		}
	}

	// Token: 0x06005A2C RID: 23084 RVA: 0x0020B134 File Offset: 0x00209334
	public void CloseClusterMap()
	{
		if (this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
		}
	}

	// Token: 0x06005A2D RID: 23085 RVA: 0x0020B170 File Offset: 0x00209370
	public void OpenSkills(MinionIdentity minionIdentity)
	{
		this.skillsScreen.CurrentlySelectedMinion = minionIdentity;
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
		}
	}

	// Token: 0x06005A2E RID: 23086 RVA: 0x0020B1C1 File Offset: 0x002093C1
	public bool IsScreenOpen(KScreen screen)
	{
		return this.activeScreen != null && this.activeScreen.screen == screen;
	}

	// Token: 0x04003B22 RID: 15138
	private const float UI_WIDTH_COMPRESS_THRESHOLD = 1300f;

	// Token: 0x04003B23 RID: 15139
	[MyCmpReq]
	public ManagementMenuNotificationDisplayer notificationDisplayer;

	// Token: 0x04003B24 RID: 15140
	public static ManagementMenu Instance;

	// Token: 0x04003B25 RID: 15141
	[Header("Management Menu Specific")]
	[SerializeField]
	private KToggle smallPrefab;

	// Token: 0x04003B26 RID: 15142
	[SerializeField]
	private KToggle researchButtonPrefab;

	// Token: 0x04003B27 RID: 15143
	public KToggle PauseMenuButton;

	// Token: 0x04003B28 RID: 15144
	[Header("Top Right Screen References")]
	public JobsTableScreen jobsScreen;

	// Token: 0x04003B29 RID: 15145
	public VitalsTableScreen vitalsScreen;

	// Token: 0x04003B2A RID: 15146
	public ScheduleScreen scheduleScreen;

	// Token: 0x04003B2B RID: 15147
	public ReportScreen reportsScreen;

	// Token: 0x04003B2C RID: 15148
	public CodexScreen codexScreen;

	// Token: 0x04003B2D RID: 15149
	public ConsumablesTableScreen consumablesScreen;

	// Token: 0x04003B2E RID: 15150
	private StarmapScreen starmapScreen;

	// Token: 0x04003B2F RID: 15151
	private ClusterMapScreen clusterMapScreen;

	// Token: 0x04003B30 RID: 15152
	private SkillsScreen skillsScreen;

	// Token: 0x04003B31 RID: 15153
	private ResearchScreen researchScreen;

	// Token: 0x04003B32 RID: 15154
	[Header("Notification Styles")]
	public ColorStyleSetting noAlertColorStyle;

	// Token: 0x04003B33 RID: 15155
	public List<ColorStyleSetting> alertColorStyle;

	// Token: 0x04003B34 RID: 15156
	public List<TextStyleSetting> alertTextStyle;

	// Token: 0x04003B35 RID: 15157
	private ManagementMenu.ManagementMenuToggleInfo jobsInfo;

	// Token: 0x04003B36 RID: 15158
	private ManagementMenu.ManagementMenuToggleInfo consumablesInfo;

	// Token: 0x04003B37 RID: 15159
	private ManagementMenu.ManagementMenuToggleInfo scheduleInfo;

	// Token: 0x04003B38 RID: 15160
	private ManagementMenu.ManagementMenuToggleInfo vitalsInfo;

	// Token: 0x04003B39 RID: 15161
	private ManagementMenu.ManagementMenuToggleInfo reportsInfo;

	// Token: 0x04003B3A RID: 15162
	private ManagementMenu.ManagementMenuToggleInfo researchInfo;

	// Token: 0x04003B3B RID: 15163
	private ManagementMenu.ManagementMenuToggleInfo codexInfo;

	// Token: 0x04003B3C RID: 15164
	private ManagementMenu.ManagementMenuToggleInfo starmapInfo;

	// Token: 0x04003B3D RID: 15165
	private ManagementMenu.ManagementMenuToggleInfo clusterMapInfo;

	// Token: 0x04003B3E RID: 15166
	private ManagementMenu.ManagementMenuToggleInfo skillsInfo;

	// Token: 0x04003B3F RID: 15167
	private ManagementMenu.ManagementMenuToggleInfo[] fullscreenUIs;

	// Token: 0x04003B40 RID: 15168
	private Dictionary<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData> ScreenInfoMatch = new Dictionary<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData>();

	// Token: 0x04003B41 RID: 15169
	private ManagementMenu.ScreenData activeScreen;

	// Token: 0x04003B42 RID: 15170
	private KButton activeButton;

	// Token: 0x04003B43 RID: 15171
	private string skillsTooltip;

	// Token: 0x04003B44 RID: 15172
	private string skillsTooltipDisabled;

	// Token: 0x04003B45 RID: 15173
	private string researchTooltip;

	// Token: 0x04003B46 RID: 15174
	private string researchTooltipDisabled;

	// Token: 0x04003B47 RID: 15175
	private string starmapTooltip;

	// Token: 0x04003B48 RID: 15176
	private string starmapTooltipDisabled;

	// Token: 0x04003B49 RID: 15177
	private string clusterMapTooltip;

	// Token: 0x04003B4A RID: 15178
	private string clusterMapTooltipDisabled;

	// Token: 0x04003B4B RID: 15179
	private List<KScreen> mutuallyExclusiveScreens = new List<KScreen>();

	// Token: 0x02001C0D RID: 7181
	public class ScreenData
	{
		// Token: 0x040081B4 RID: 33204
		public KScreen screen;

		// Token: 0x040081B5 RID: 33205
		public ManagementMenu.ManagementMenuToggleInfo toggleInfo;

		// Token: 0x040081B6 RID: 33206
		public Func<bool> cancelHandler;

		// Token: 0x040081B7 RID: 33207
		public int tabIdx;
	}

	// Token: 0x02001C0E RID: 7182
	public class ManagementMenuToggleInfo : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x0600A53B RID: 42299 RVA: 0x0038F34B File Offset: 0x0038D54B
		public ManagementMenuToggleInfo(string text, string icon, object user_data = null, global::Action hotkey = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon, user_data, hotkey, tooltip, tooltip_header)
		{
			this.tooltip = GameUtil.ReplaceHotkeyString(this.tooltip, this.hotKey);
		}

		// Token: 0x0600A53C RID: 42300 RVA: 0x0038F374 File Offset: 0x0038D574
		public void SetNotificationDisplay(bool showAlertImage, bool showGlow, ColorStyleSetting buttonColorStyle, ColorStyleSetting alertColorStyle)
		{
			ImageToggleState component = this.toggle.GetComponent<ImageToggleState>();
			if (component != null)
			{
				if (buttonColorStyle != null)
				{
					component.SetColorStyle(buttonColorStyle);
				}
				else
				{
					component.SetColorStyle(this.originalButtonSetting);
				}
			}
			if (this.alertImage != null)
			{
				this.alertImage.gameObject.SetActive(showAlertImage);
				this.alertImage.SetColorStyle(alertColorStyle);
			}
			if (this.glowImage != null)
			{
				this.glowImage.gameObject.SetActive(showGlow);
				if (buttonColorStyle != null)
				{
					this.glowImage.SetColorStyle(buttonColorStyle);
				}
			}
		}

		// Token: 0x0600A53D RID: 42301 RVA: 0x0038F414 File Offset: 0x0038D614
		public override void SetToggle(KToggle toggle)
		{
			base.SetToggle(toggle);
			ImageToggleState component = toggle.GetComponent<ImageToggleState>();
			if (component != null)
			{
				this.originalButtonSetting = component.colorStyleSetting;
			}
			HierarchyReferences component2 = toggle.GetComponent<HierarchyReferences>();
			if (component2 != null)
			{
				this.alertImage = component2.GetReference<ImageToggleState>("AlertImage");
				this.glowImage = component2.GetReference<ImageToggleState>("GlowImage");
			}
		}

		// Token: 0x040081B8 RID: 33208
		public ImageToggleState alertImage;

		// Token: 0x040081B9 RID: 33209
		public ImageToggleState glowImage;

		// Token: 0x040081BA RID: 33210
		private ColorStyleSetting originalButtonSetting;
	}
}
