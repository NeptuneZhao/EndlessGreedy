using System;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C17 RID: 3095
public class ColonyDestinationSelectScreen : NewGameFlowScreen
{
	// Token: 0x06005EE5 RID: 24293 RVA: 0x00233FAC File Offset: 0x002321AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.backButton.onClick += this.BackClicked;
		this.customizeButton.onClick += this.CustomizeClicked;
		this.launchButton.onClick += this.LaunchClicked;
		this.shuffleButton.onClick += this.ShuffleClicked;
		this.storyTraitShuffleButton.onClick += this.StoryTraitShuffleClicked;
		this.storyTraitShuffleButton.gameObject.SetActive(Db.Get().Stories.Count > 5);
		this.destinationMapPanel.OnAsteroidClicked += this.OnAsteroidClicked;
		KInputTextField kinputTextField = this.coordinate;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(this.CoordinateEditStarted));
		this.coordinate.onEndEdit.AddListener(new UnityAction<string>(this.CoordinateEditFinished));
		if (this.locationIcons != null)
		{
			bool cloudSavesAvailable = SaveLoader.GetCloudSavesAvailable();
			this.locationIcons.gameObject.SetActive(cloudSavesAvailable);
		}
		this.random = new KRandom();
	}

	// Token: 0x06005EE6 RID: 24294 RVA: 0x002340E0 File Offset: 0x002322E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshCloudSavePref();
		this.RefreshCloudLocalIcon();
		this.newGameSettingsPanel.Init();
		this.newGameSettingsPanel.SetCloseAction(new System.Action(this.CustomizeClose));
		this.destinationMapPanel.Init();
		this.mixingPanel.Init();
		this.ShuffleClicked();
		this.RefreshMenuTabs();
		for (int i = 0; i < this.menuTabs.Length; i++)
		{
			int target = i;
			this.menuTabs[i].onClick = delegate()
			{
				this.selectedMenuTabIdx = target;
				this.RefreshMenuTabs();
			};
		}
		this.ResizeLayout();
		this.storyContentPanel.Init();
		this.storyContentPanel.SelectRandomStories(5, 5, true);
		this.storyContentPanel.SelectDefault();
		this.RefreshStoryLabel();
		this.RefreshRowsAndDescriptions();
		CustomGameSettings.Instance.OnQualitySettingChanged += this.QualitySettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged += this.QualitySettingChanged;
		CustomGameSettings.Instance.OnMixingSettingChanged += this.QualitySettingChanged;
		this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
	}

	// Token: 0x06005EE7 RID: 24295 RVA: 0x00234210 File Offset: 0x00232410
	private void ResizeLayout()
	{
		Vector2 sizeDelta = this.destinationProperties.clusterDetailsButton.rectTransform().sizeDelta;
		this.destinationProperties.clusterDetailsButton.rectTransform().sizeDelta = new Vector2(sizeDelta.x, (float)(DlcManager.FeatureClusterSpaceEnabled() ? 164 : 76));
		Vector2 sizeDelta2 = this.worldsScrollPanel.rectTransform().sizeDelta;
		Vector2 anchoredPosition = this.worldsScrollPanel.rectTransform().anchoredPosition;
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.worldsScrollPanel.rectTransform().anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + 88f);
		}
		float num = (float)(DlcManager.FeatureClusterSpaceEnabled() ? 436 : 524);
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
		num = Mathf.Min(num, this.destinationInfoPanel.sizeDelta.y - (float)(DlcManager.FeatureClusterSpaceEnabled() ? 164 : 76) - 22f);
		this.worldsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.storyScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.mixingScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.gameSettingsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
	}

	// Token: 0x06005EE8 RID: 24296 RVA: 0x00234378 File Offset: 0x00232578
	protected override void OnCleanUp()
	{
		CustomGameSettings.Instance.OnQualitySettingChanged -= this.QualitySettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged -= this.QualitySettingChanged;
		this.newGameSettingsPanel.Uninit();
		this.destinationMapPanel.Uninit();
		this.mixingPanel.Uninit();
		this.storyContentPanel.Cleanup();
		base.OnCleanUp();
	}

	// Token: 0x06005EE9 RID: 24297 RVA: 0x002343E4 File Offset: 0x002325E4
	private void RefreshCloudLocalIcon()
	{
		if (this.locationIcons == null)
		{
			return;
		}
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		HierarchyReferences component = this.locationIcons.GetComponent<HierarchyReferences>();
		LocText component2 = component.GetReference<RectTransform>("LocationText").GetComponent<LocText>();
		KButton component3 = component.GetReference<RectTransform>("CloudButton").GetComponent<KButton>();
		KButton component4 = component.GetReference<RectTransform>("LocalButton").GetComponent<KButton>();
		ToolTip component5 = component3.GetComponent<ToolTip>();
		ToolTip component6 = component4.GetComponent<ToolTip>();
		component5.toolTip = string.Format("{0}\n{1}", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA);
		component6.toolTip = string.Format("{0}\n{1}", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_LOCAL, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA);
		bool flag = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
		component2.text = (flag ? UI.FRONTEND.LOADSCREEN.CLOUD_SAVE : UI.FRONTEND.LOADSCREEN.LOCAL_SAVE);
		component3.gameObject.SetActive(flag);
		component3.ClearOnClick();
		if (flag)
		{
			component3.onClick += delegate()
			{
				CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Disabled");
				this.RefreshCloudLocalIcon();
			};
		}
		component4.gameObject.SetActive(!flag);
		component4.ClearOnClick();
		if (!flag)
		{
			component4.onClick += delegate()
			{
				CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Enabled");
				this.RefreshCloudLocalIcon();
			};
		}
	}

	// Token: 0x06005EEA RID: 24298 RVA: 0x00234518 File Offset: 0x00232718
	private void RefreshCloudSavePref()
	{
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		string cloudSavesDefaultPref = SaveLoader.GetCloudSavesDefaultPref();
		CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, cloudSavesDefaultPref);
	}

	// Token: 0x06005EEB RID: 24299 RVA: 0x00234543 File Offset: 0x00232743
	private void BackClicked()
	{
		this.newGameSettingsPanel.Cancel();
		base.NavigateBackward();
	}

	// Token: 0x06005EEC RID: 24300 RVA: 0x00234556 File Offset: 0x00232756
	private void CustomizeClicked()
	{
		this.newGameSettingsPanel.Refresh();
		this.customSettings.SetActive(true);
	}

	// Token: 0x06005EED RID: 24301 RVA: 0x0023456F File Offset: 0x0023276F
	private void CustomizeClose()
	{
		this.customSettings.SetActive(false);
	}

	// Token: 0x06005EEE RID: 24302 RVA: 0x0023457D File Offset: 0x0023277D
	private void LaunchClicked()
	{
		CustomGameSettings.Instance.RemoveInvalidMixingSettings();
		base.NavigateForward();
	}

	// Token: 0x06005EEF RID: 24303 RVA: 0x00234590 File Offset: 0x00232790
	private void RefreshMenuTabs()
	{
		for (int i = 0; i < this.menuTabs.Length; i++)
		{
			this.menuTabs[i].ChangeState((i == this.selectedMenuTabIdx) ? 1 : 0);
			LocText componentInChildren = this.menuTabs[i].GetComponentInChildren<LocText>();
			HierarchyReferences component = this.menuTabs[i].GetComponent<HierarchyReferences>();
			if (componentInChildren != null)
			{
				componentInChildren.color = ((i == this.selectedMenuTabIdx) ? Color.white : Color.grey);
			}
			if (component != null)
			{
				Image reference = component.GetReference<Image>("Icon");
				if (reference != null)
				{
					reference.color = ((i == this.selectedMenuTabIdx) ? Color.white : Color.grey);
				}
			}
		}
		this.destinationInfoPanel.gameObject.SetActive(this.selectedMenuTabIdx == 1);
		this.storyInfoPanel.gameObject.SetActive(this.selectedMenuTabIdx == 2);
		this.mixingSettingsPanel.gameObject.SetActive(this.selectedMenuTabIdx == 3);
		this.gameSettingsPanel.gameObject.SetActive(this.selectedMenuTabIdx == 4);
		int num = this.selectedMenuTabIdx;
		if (num != 1)
		{
			if (num == 2)
			{
				this.destinationDetailsHeader.SetParent(this.destinationDetailsParent_Story);
			}
		}
		else
		{
			this.destinationDetailsHeader.SetParent(this.destinationDetailsParent_Asteroid);
		}
		this.destinationDetailsHeader.SetAsFirstSibling();
	}

	// Token: 0x06005EF0 RID: 24304 RVA: 0x002346F0 File Offset: 0x002328F0
	private void ShuffleClicked()
	{
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		int num = this.random.Next();
		if (currentClusterLayout != null && currentClusterLayout.fixedCoordinate != -1)
		{
			num = currentClusterLayout.fixedCoordinate;
		}
		this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, num.ToString(), true);
	}

	// Token: 0x06005EF1 RID: 24305 RVA: 0x0023473F File Offset: 0x0023293F
	private void StoryTraitShuffleClicked()
	{
		this.storyContentPanel.SelectRandomStories(5, 5, false);
	}

	// Token: 0x06005EF2 RID: 24306 RVA: 0x00234750 File Offset: 0x00232950
	private void CoordinateChanged(string text)
	{
		string[] array = CustomGameSettings.ParseSettingCoordinate(text);
		if (array.Length < 4 || array.Length > 6)
		{
			return;
		}
		int num;
		if (!int.TryParse(array[2], out num))
		{
			return;
		}
		ClusterLayout clusterLayout = null;
		foreach (string name in SettingsCache.GetClusterNames())
		{
			ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(name);
			if (clusterData.coordinatePrefix == array[1])
			{
				clusterLayout = clusterData;
			}
		}
		if (clusterLayout != null)
		{
			this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, clusterLayout.filePath, true);
		}
		this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, array[2], true);
		this.newGameSettingsPanel.ConsumeSettingsCode(array[3]);
		string code = (array.Length >= 5) ? array[4] : "0";
		this.newGameSettingsPanel.ConsumeStoryTraitsCode(code);
		string code2 = (array.Length >= 6) ? array[5] : "0";
		this.newGameSettingsPanel.ConsumeMixingSettingsCode(code2);
	}

	// Token: 0x06005EF3 RID: 24307 RVA: 0x00234858 File Offset: 0x00232A58
	private void CoordinateEditStarted()
	{
		this.isEditingCoordinate = true;
	}

	// Token: 0x06005EF4 RID: 24308 RVA: 0x00234861 File Offset: 0x00232A61
	private void CoordinateEditFinished(string text)
	{
		this.CoordinateChanged(text);
		this.isEditingCoordinate = false;
		this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
	}

	// Token: 0x06005EF5 RID: 24309 RVA: 0x00234888 File Offset: 0x00232A88
	private void QualitySettingChanged(SettingConfig config, SettingLevel level)
	{
		if (config == CustomGameSettingConfigs.SaveToCloud)
		{
			this.RefreshCloudLocalIcon();
		}
		if (!this.destinationDetailsHeader.IsNullOrDestroyed())
		{
			if (!this.isEditingCoordinate && !this.coordinate.IsNullOrDestroyed())
			{
				this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
			}
			this.RefreshRowsAndDescriptions();
		}
	}

	// Token: 0x06005EF6 RID: 24310 RVA: 0x002348E0 File Offset: 0x00232AE0
	public void RefreshRowsAndDescriptions()
	{
		string setting = this.newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.ClusterLayout);
		int seed;
		int.TryParse(this.newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.WorldgenSeed), out seed);
		int fixedCoordinate = CustomGameSettings.Instance.GetCurrentClusterLayout().fixedCoordinate;
		if (fixedCoordinate != -1)
		{
			this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, fixedCoordinate.ToString(), false);
			seed = fixedCoordinate;
			this.shuffleButton.isInteractable = false;
			this.shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP_DISABLED);
		}
		else
		{
			this.coordinate.interactable = true;
			this.shuffleButton.isInteractable = true;
			this.shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP);
		}
		ColonyDestinationAsteroidBeltData cluster;
		try
		{
			cluster = this.destinationMapPanel.SelectCluster(setting, seed);
		}
		catch
		{
			string defaultAsteroid = this.destinationMapPanel.GetDefaultAsteroid();
			this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, defaultAsteroid, true);
			cluster = this.destinationMapPanel.SelectCluster(defaultAsteroid, seed);
		}
		if (DlcManager.IsContentSubscribed("EXPANSION1_ID"))
		{
			this.destinationProperties.EnableClusterLocationLabels(true);
			this.destinationProperties.RefreshAsteroidLines(cluster, this.selectedLocationProperties, this.storyContentPanel.GetActiveStories());
			this.destinationProperties.EnableClusterDetails(true);
			this.destinationProperties.SetClusterDetailLabels(cluster);
			this.selectedLocationProperties.headerLabel.SetText(UI.FRONTEND.COLONYDESTINATIONSCREEN.SELECTED_CLUSTER_TRAITS_HEADER);
			this.destinationProperties.clusterDetailsButton.onClick = delegate()
			{
				this.destinationProperties.SelectWholeClusterDetails(cluster, this.selectedLocationProperties, this.storyContentPanel.GetActiveStories());
			};
		}
		else
		{
			this.destinationProperties.EnableClusterDetails(false);
			this.destinationProperties.EnableClusterLocationLabels(false);
			this.destinationProperties.SetParameterDescriptors(cluster.GetParamDescriptors());
			this.selectedLocationProperties.SetTraitDescriptors(cluster.GetTraitDescriptors(), this.storyContentPanel.GetActiveStories(), true);
		}
		this.RefreshStoryLabel();
	}

	// Token: 0x06005EF7 RID: 24311 RVA: 0x00234AF0 File Offset: 0x00232CF0
	public void RefreshStoryLabel()
	{
		this.storyTraitsDestinationDetailsLabel.SetText(this.storyContentPanel.GetTraitsString(false));
		this.storyTraitsDestinationDetailsLabel.GetComponent<ToolTip>().SetSimpleTooltip(this.storyContentPanel.GetTraitsString(true));
	}

	// Token: 0x06005EF8 RID: 24312 RVA: 0x00234B25 File Offset: 0x00232D25
	private void OnAsteroidClicked(ColonyDestinationAsteroidBeltData cluster)
	{
		this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, cluster.beltPath, true);
		this.ShuffleClicked();
	}

	// Token: 0x06005EF9 RID: 24313 RVA: 0x00234B44 File Offset: 0x00232D44
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.isEditingCoordinate)
		{
			return;
		}
		if (!e.Consumed && e.TryConsume(global::Action.PanLeft))
		{
			this.destinationMapPanel.ScrollLeft();
		}
		else if (!e.Consumed && e.TryConsume(global::Action.PanRight))
		{
			this.destinationMapPanel.ScrollRight();
		}
		else if (this.customSettings.activeSelf && !e.Consumed && (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight)))
		{
			this.CustomizeClose();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04003F66 RID: 16230
	[SerializeField]
	private GameObject destinationMap;

	// Token: 0x04003F67 RID: 16231
	[SerializeField]
	private GameObject customSettings;

	// Token: 0x04003F68 RID: 16232
	[Header("Menu")]
	[SerializeField]
	private MultiToggle[] menuTabs;

	// Token: 0x04003F69 RID: 16233
	private int selectedMenuTabIdx = 1;

	// Token: 0x04003F6A RID: 16234
	[Header("Buttons")]
	[SerializeField]
	private KButton backButton;

	// Token: 0x04003F6B RID: 16235
	[SerializeField]
	private KButton customizeButton;

	// Token: 0x04003F6C RID: 16236
	[SerializeField]
	private KButton launchButton;

	// Token: 0x04003F6D RID: 16237
	[SerializeField]
	private KButton shuffleButton;

	// Token: 0x04003F6E RID: 16238
	[SerializeField]
	private KButton storyTraitShuffleButton;

	// Token: 0x04003F6F RID: 16239
	[Header("Scroll Panels")]
	[SerializeField]
	private RectTransform worldsScrollPanel;

	// Token: 0x04003F70 RID: 16240
	[SerializeField]
	private RectTransform storyScrollPanel;

	// Token: 0x04003F71 RID: 16241
	[SerializeField]
	private RectTransform mixingScrollPanel;

	// Token: 0x04003F72 RID: 16242
	[SerializeField]
	private RectTransform gameSettingsScrollPanel;

	// Token: 0x04003F73 RID: 16243
	[Header("Panels")]
	[SerializeField]
	private RectTransform destinationDetailsHeader;

	// Token: 0x04003F74 RID: 16244
	[SerializeField]
	private RectTransform destinationInfoPanel;

	// Token: 0x04003F75 RID: 16245
	[SerializeField]
	private RectTransform storyInfoPanel;

	// Token: 0x04003F76 RID: 16246
	[SerializeField]
	private RectTransform mixingSettingsPanel;

	// Token: 0x04003F77 RID: 16247
	[SerializeField]
	private RectTransform gameSettingsPanel;

	// Token: 0x04003F78 RID: 16248
	[Header("References")]
	[SerializeField]
	private RectTransform destinationDetailsParent_Asteroid;

	// Token: 0x04003F79 RID: 16249
	[SerializeField]
	private RectTransform destinationDetailsParent_Story;

	// Token: 0x04003F7A RID: 16250
	[SerializeField]
	private LocText storyTraitsDestinationDetailsLabel;

	// Token: 0x04003F7B RID: 16251
	[SerializeField]
	private HierarchyReferences locationIcons;

	// Token: 0x04003F7C RID: 16252
	[SerializeField]
	private KInputTextField coordinate;

	// Token: 0x04003F7D RID: 16253
	[SerializeField]
	private StoryContentPanel storyContentPanel;

	// Token: 0x04003F7E RID: 16254
	[SerializeField]
	private AsteroidDescriptorPanel destinationProperties;

	// Token: 0x04003F7F RID: 16255
	[SerializeField]
	private AsteroidDescriptorPanel selectedLocationProperties;

	// Token: 0x04003F80 RID: 16256
	private const int DESTINATION_HEADER_BUTTON_HEIGHT_CLUSTER = 164;

	// Token: 0x04003F81 RID: 16257
	private const int DESTINATION_HEADER_BUTTON_HEIGHT_BASE = 76;

	// Token: 0x04003F82 RID: 16258
	private const int WORLDS_SCROLL_PANEL_HEIGHT_CLUSTER = 436;

	// Token: 0x04003F83 RID: 16259
	private const int WORLDS_SCROLL_PANEL_HEIGHT_BASE = 524;

	// Token: 0x04003F84 RID: 16260
	[SerializeField]
	private NewGameSettingsPanel newGameSettingsPanel;

	// Token: 0x04003F85 RID: 16261
	[MyCmpReq]
	private DestinationSelectPanel destinationMapPanel;

	// Token: 0x04003F86 RID: 16262
	[SerializeField]
	private MixingContentPanel mixingPanel;

	// Token: 0x04003F87 RID: 16263
	private KRandom random;

	// Token: 0x04003F88 RID: 16264
	private bool isEditingCoordinate;
}
