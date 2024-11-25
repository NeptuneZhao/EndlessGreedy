using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using FMOD.Studio;
using ProcGen;
using ProcGenGame;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D28 RID: 3368
public class RetiredColonyInfoScreen : KModalScreen
{
	// Token: 0x06006989 RID: 27017 RVA: 0x0027A34C File Offset: 0x0027854C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		RetiredColonyInfoScreen.Instance = this;
		this.ConfigButtons();
		this.LoadExplorer();
		this.PopulateAchievements();
		base.ConsumeMouseScroll = true;
		this.explorerSearch.text = "";
		this.explorerSearch.onValueChanged.AddListener(delegate(string value)
		{
			if (this.colonyDataRoot.activeSelf)
			{
				this.FilterColonyData(this.explorerSearch.text);
				return;
			}
			this.FilterExplorer(this.explorerSearch.text);
		});
		this.clearExplorerSearchButton.onClick += delegate()
		{
			this.explorerSearch.text = "";
		};
		this.achievementSearch.text = "";
		this.achievementSearch.onValueChanged.AddListener(delegate(string value)
		{
			this.FilterAchievements(this.achievementSearch.text);
		});
		this.clearAchievementSearchButton.onClick += delegate()
		{
			this.achievementSearch.text = "";
		};
		this.RefreshUIScale(null);
		base.Subscribe(-810220474, new Action<object>(this.RefreshUIScale));
	}

	// Token: 0x0600698A RID: 27018 RVA: 0x0027A423 File Offset: 0x00278623
	private void RefreshUIScale(object data = null)
	{
		base.StartCoroutine(this.DelayedRefreshScale());
	}

	// Token: 0x0600698B RID: 27019 RVA: 0x0027A432 File Offset: 0x00278632
	private IEnumerator DelayedRefreshScale()
	{
		int num;
		for (int i = 0; i < 3; i = num + 1)
		{
			yield return 0;
			num = i;
		}
		float num2 = 36f;
		if (GameObject.Find("ScreenSpaceOverlayCanvas") != null)
		{
			this.explorerRoot.transform.parent.localScale = Vector3.one * ((this.colonyScroll.rectTransform().rect.width - num2) / this.explorerRoot.transform.parent.rectTransform().rect.width);
		}
		else
		{
			this.explorerRoot.transform.parent.localScale = Vector3.one * ((this.colonyScroll.rectTransform().rect.width - num2) / this.explorerRoot.transform.parent.rectTransform().rect.width);
		}
		yield break;
	}

	// Token: 0x0600698C RID: 27020 RVA: 0x0027A444 File Offset: 0x00278644
	private void ConfigButtons()
	{
		this.closeButton.ClearOnClick();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.viewOtherColoniesButton.ClearOnClick();
		this.viewOtherColoniesButton.onClick += delegate()
		{
			this.ToggleExplorer(true);
		};
		this.quitToMainMenuButton.ClearOnClick();
		this.quitToMainMenuButton.onClick += delegate()
		{
			this.ConfirmDecision(UI.FRONTEND.MAINMENU.QUITCONFIRM, new System.Action(this.OnQuitConfirm));
		};
		this.closeScreenButton.ClearOnClick();
		this.closeScreenButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.viewOtherColoniesButton.gameObject.SetActive(false);
		if (Game.Instance != null)
		{
			this.closeScreenButton.gameObject.SetActive(true);
			this.closeScreenButton.GetComponentInChildren<LocText>().SetText(UI.RETIRED_COLONY_INFO_SCREEN.BUTTONS.RETURN_TO_GAME);
			this.quitToMainMenuButton.gameObject.SetActive(true);
			return;
		}
		this.closeScreenButton.gameObject.SetActive(true);
		this.closeScreenButton.GetComponentInChildren<LocText>().SetText(UI.RETIRED_COLONY_INFO_SCREEN.BUTTONS.CLOSE);
		this.quitToMainMenuButton.gameObject.SetActive(false);
	}

	// Token: 0x0600698D RID: 27021 RVA: 0x0027A570 File Offset: 0x00278770
	private void ConfirmDecision(string text, System.Action onConfirm)
	{
		base.gameObject.SetActive(false);
		((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(text, onConfirm, new System.Action(this.OnCancelPopup), null, null, null, null, null, null);
	}

	// Token: 0x0600698E RID: 27022 RVA: 0x0027A5D1 File Offset: 0x002787D1
	private void OnCancelPopup()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600698F RID: 27023 RVA: 0x0027A5DF File Offset: 0x002787DF
	private void OnQuitConfirm()
	{
		LoadingOverlay.Load(delegate
		{
			this.Deactivate();
			PauseScreen.TriggerQuitGame();
		});
	}

	// Token: 0x06006990 RID: 27024 RVA: 0x0027A5F2 File Offset: 0x002787F2
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.GetCanvasRef();
		this.wasPixelPerfect = this.canvasRef.pixelPerfect;
		this.canvasRef.pixelPerfect = false;
	}

	// Token: 0x06006991 RID: 27025 RVA: 0x0027A61D File Offset: 0x0027881D
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			this.Show(false);
		}
		else if (e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006992 RID: 27026 RVA: 0x0027A654 File Offset: 0x00278854
	private void GetCanvasRef()
	{
		if (base.transform.parent.GetComponent<Canvas>() != null)
		{
			this.canvasRef = base.transform.parent.GetComponent<Canvas>();
			return;
		}
		this.canvasRef = base.transform.parent.parent.GetComponent<Canvas>();
	}

	// Token: 0x06006993 RID: 27027 RVA: 0x0027A6AB File Offset: 0x002788AB
	protected override void OnCmpDisable()
	{
		this.canvasRef.pixelPerfect = this.wasPixelPerfect;
		base.OnCmpDisable();
	}

	// Token: 0x06006994 RID: 27028 RVA: 0x0027A6C4 File Offset: 0x002788C4
	public RetiredColonyData GetColonyDataByBaseName(string name)
	{
		name = RetireColonyUtility.StripInvalidCharacters(name);
		for (int i = 0; i < this.retiredColonyData.Length; i++)
		{
			if (RetireColonyUtility.StripInvalidCharacters(this.retiredColonyData[i].colonyName) == name)
			{
				return this.retiredColonyData[i];
			}
		}
		return null;
	}

	// Token: 0x06006995 RID: 27029 RVA: 0x0027A710 File Offset: 0x00278910
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.explorerSearch.text = "";
			this.achievementSearch.text = "";
			this.RefreshUIScale(null);
		}
		else
		{
			this.InstantClearAchievementVeils();
		}
		if (Game.Instance != null)
		{
			if (!show)
			{
				if (MusicManager.instance.SongIsPlaying("Music_Victory_03_StoryAndSummary"))
				{
					MusicManager.instance.StopSong("Music_Victory_03_StoryAndSummary", true, STOP_MODE.ALLOWFADEOUT);
				}
			}
			else
			{
				this.retiredColonyData = RetireColonyUtility.LoadRetiredColonies(true);
				if (MusicManager.instance.SongIsPlaying("Music_Victory_03_StoryAndSummary"))
				{
					MusicManager.instance.SetSongParameter("Music_Victory_03_StoryAndSummary", "songSection", 2f, true);
				}
			}
		}
		else if (Game.Instance == null)
		{
			this.ToggleExplorer(true);
		}
		this.disabledPlatformUnlocks.SetActive(SaveGame.Instance != null);
		if (SaveGame.Instance != null)
		{
			this.disabledPlatformUnlocks.GetComponent<HierarchyReferences>().GetReference("enabled").gameObject.SetActive(!DebugHandler.InstantBuildMode && !SaveGame.Instance.sandboxEnabled && !Game.Instance.debugWasUsed);
			this.disabledPlatformUnlocks.GetComponent<HierarchyReferences>().GetReference("disabled").gameObject.SetActive(DebugHandler.InstantBuildMode || SaveGame.Instance.sandboxEnabled || Game.Instance.debugWasUsed);
		}
	}

	// Token: 0x06006996 RID: 27030 RVA: 0x0027A880 File Offset: 0x00278A80
	public void LoadColony(RetiredColonyData data)
	{
		this.colonyName.text = data.colonyName.ToUpper();
		this.cycleCount.text = string.Format(UI.RETIRED_COLONY_INFO_SCREEN.CYCLE_COUNT, data.cycleCount.ToString());
		this.focusedWorld = data.startWorld;
		this.ToggleExplorer(false);
		this.RefreshUIScale(null);
		if (Game.Instance == null)
		{
			this.viewOtherColoniesButton.gameObject.SetActive(true);
		}
		this.ClearColony();
		if (SaveGame.Instance != null)
		{
			ColonyAchievementTracker component = SaveGame.Instance.GetComponent<ColonyAchievementTracker>();
			this.UpdateAchievementData(data, component.achievementsToDisplay.ToArray());
			component.ClearDisplayAchievements();
			this.PopulateAchievementProgress(component);
		}
		else
		{
			this.UpdateAchievementData(data, null);
		}
		this.DisplayStatistics(data);
		this.colonyDataRoot.transform.parent.rectTransform().SetPosition(new Vector3(this.colonyDataRoot.transform.parent.rectTransform().position.x, 0f, 0f));
	}

	// Token: 0x06006997 RID: 27031 RVA: 0x0027A99C File Offset: 0x00278B9C
	private void PopulateAchievementProgress(ColonyAchievementTracker tracker)
	{
		if (tracker != null)
		{
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.achievementEntries)
			{
				ColonyAchievementStatus colonyAchievementStatus;
				tracker.achievements.TryGetValue(keyValuePair.Key, out colonyAchievementStatus);
				if (colonyAchievementStatus != null)
				{
					AchievementWidget component = keyValuePair.Value.GetComponent<AchievementWidget>();
					if (component != null)
					{
						component.ShowProgress(colonyAchievementStatus);
						if (colonyAchievementStatus.failed)
						{
							component.SetFailed();
						}
					}
				}
			}
		}
	}

	// Token: 0x06006998 RID: 27032 RVA: 0x0027AA34 File Offset: 0x00278C34
	private bool LoadSlideshow(RetiredColonyData data)
	{
		this.clearCurrentSlideshow();
		this.currentSlideshowFiles = RetireColonyUtility.LoadColonySlideshowFiles(data.colonyName, this.focusedWorld);
		this.slideshow.SetFiles(this.currentSlideshowFiles, -1);
		return this.currentSlideshowFiles != null && this.currentSlideshowFiles.Length != 0;
	}

	// Token: 0x06006999 RID: 27033 RVA: 0x0027AA84 File Offset: 0x00278C84
	private void clearCurrentSlideshow()
	{
		this.currentSlideshowFiles = new string[0];
	}

	// Token: 0x0600699A RID: 27034 RVA: 0x0027AA94 File Offset: 0x00278C94
	private bool LoadScreenshot(RetiredColonyData data, string world)
	{
		this.clearCurrentSlideshow();
		Sprite sprite = RetireColonyUtility.LoadRetiredColonyPreview(data.colonyName, world);
		if (sprite != null)
		{
			this.slideshow.setSlide(sprite);
			this.CorrectTimelapseImageSize(sprite);
		}
		return sprite != null;
	}

	// Token: 0x0600699B RID: 27035 RVA: 0x0027AAD8 File Offset: 0x00278CD8
	private void ClearColony()
	{
		foreach (GameObject obj in this.activeColonyWidgetContainers)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.activeColonyWidgetContainers.Clear();
		this.activeColonyWidgets.Clear();
		this.UpdateAchievementData(null, null);
	}

	// Token: 0x0600699C RID: 27036 RVA: 0x0027AB48 File Offset: 0x00278D48
	private bool IsAchievementValidForDLCContext(string[] dlcid, string clusterTag)
	{
		return DlcManager.IsAnyContentSubscribed(dlcid) && (!(SaveLoader.Instance != null) || ((clusterTag == null || CustomGameSettings.Instance.GetCurrentClusterLayout().clusterTags.Contains(clusterTag)) && SaveLoader.Instance.IsDlcListActiveForCurrentSave(dlcid)));
	}

	// Token: 0x0600699D RID: 27037 RVA: 0x0027AB98 File Offset: 0x00278D98
	private void PopulateAchievements()
	{
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (this.IsAchievementValidForDLCContext(colonyAchievement.dlcIds, null))
			{
				GameObject gameObject = global::Util.KInstantiateUI(colonyAchievement.isVictoryCondition ? this.victoryAchievementsPrefab : this.achievementsPrefab, this.achievementsContainer, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("nameLabel").SetText(colonyAchievement.Name);
				component.GetReference<LocText>("descriptionLabel").SetText(colonyAchievement.description);
				if (string.IsNullOrEmpty(colonyAchievement.icon) || Assets.GetSprite(colonyAchievement.icon) == null)
				{
					if (Assets.GetSprite(colonyAchievement.Name) != null)
					{
						component.GetReference<Image>("icon").sprite = Assets.GetSprite(colonyAchievement.Name);
					}
					else
					{
						component.GetReference<Image>("icon").sprite = Assets.GetSprite("check");
					}
				}
				else
				{
					component.GetReference<Image>("icon").sprite = Assets.GetSprite(colonyAchievement.icon);
				}
				if (colonyAchievement.isVictoryCondition)
				{
					gameObject.transform.SetAsFirstSibling();
				}
				KImage reference = component.GetReference<KImage>("dlc_overlay");
				if (DlcManager.IsDlcId(colonyAchievement.dlcIdFrom))
				{
					reference.gameObject.SetActive(true);
					reference.sprite = Assets.GetSprite(DlcManager.GetDlcBanner(colonyAchievement.dlcIdFrom));
					reference.color = DlcManager.GetDlcBannerColor(colonyAchievement.dlcIdFrom);
				}
				else
				{
					reference.gameObject.SetActive(false);
				}
				gameObject.GetComponent<MultiToggle>().ChangeState(2);
				gameObject.GetComponent<AchievementWidget>().dlcIdFrom = colonyAchievement.dlcIdFrom;
				this.achievementEntries.Add(colonyAchievement.Id, gameObject);
			}
		}
		this.UpdateAchievementData(null, null);
	}

	// Token: 0x0600699E RID: 27038 RVA: 0x0027ADB8 File Offset: 0x00278FB8
	private void InstantClearAchievementVeils()
	{
		GameObject[] array = this.achievementVeils;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
		}
		array = this.achievementVeils;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.achievementEntries)
		{
			AchievementWidget component = keyValuePair.Value.GetComponent<AchievementWidget>();
			component.StopAllCoroutines();
			component.CompleteFlourish();
		}
	}

	// Token: 0x0600699F RID: 27039 RVA: 0x0027AE74 File Offset: 0x00279074
	private IEnumerator ClearAchievementVeil(float delay = 0f)
	{
		yield return new WaitForSecondsRealtime(delay);
		for (float i = 0.7f; i >= 0f; i -= Time.unscaledDeltaTime)
		{
			GameObject[] array = this.achievementVeils;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].GetComponent<Image>().color = new Color(0f, 0f, 0f, i);
			}
			yield return 0;
		}
		this.InstantClearAchievementVeils();
		yield break;
	}

	// Token: 0x060069A0 RID: 27040 RVA: 0x0027AE8A File Offset: 0x0027908A
	private IEnumerator ShowAchievementVeil()
	{
		float targetAlpha = 0.7f;
		GameObject[] array = this.achievementVeils;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].SetActive(true);
		}
		for (float i = 0f; i <= targetAlpha; i += Time.unscaledDeltaTime)
		{
			array = this.achievementVeils;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].GetComponent<Image>().color = new Color(0f, 0f, 0f, i);
			}
			yield return 0;
		}
		for (float num = 0f; num <= targetAlpha; num += Time.unscaledDeltaTime)
		{
			array = this.achievementVeils;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].GetComponent<Image>().color = new Color(0f, 0f, 0f, targetAlpha);
			}
		}
		yield break;
	}

	// Token: 0x060069A1 RID: 27041 RVA: 0x0027AE9C File Offset: 0x0027909C
	private void UpdateAchievementData(RetiredColonyData data, string[] newlyAchieved = null)
	{
		int num = 0;
		float num2 = 2f;
		float num3 = 1f;
		if (newlyAchieved != null && newlyAchieved.Length != 0)
		{
			this.retiredColonyData = RetireColonyUtility.LoadRetiredColonies(true);
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.achievementEntries)
		{
			bool flag = false;
			bool flag2 = false;
			if (data != null)
			{
				string[] achievements = data.achievements;
				for (int i = 0; i < achievements.Length; i++)
				{
					if (achievements[i] == keyValuePair.Key)
					{
						flag = true;
						break;
					}
				}
			}
			ColonyAchievement colonyAchievement = Db.Get().ColonyAchievements.TryGet(keyValuePair.Key);
			if (colonyAchievement != null && !this.IsAchievementValidForDLCContext(colonyAchievement.dlcIds, colonyAchievement.clusterTag))
			{
				keyValuePair.Value.SetActive(false);
			}
			else
			{
				keyValuePair.Value.SetActive(true);
			}
			if (!flag && data == null && this.retiredColonyData != null)
			{
				RetiredColonyData[] array = this.retiredColonyData;
				for (int i = 0; i < array.Length; i++)
				{
					string[] achievements = array[i].achievements;
					for (int j = 0; j < achievements.Length; j++)
					{
						if (achievements[j] == keyValuePair.Key)
						{
							flag2 = true;
						}
					}
				}
			}
			bool flag3 = false;
			if (newlyAchieved != null)
			{
				for (int k = 0; k < newlyAchieved.Length; k++)
				{
					if (newlyAchieved[k] == keyValuePair.Key)
					{
						flag3 = true;
					}
				}
			}
			if (flag || flag3)
			{
				if (flag3)
				{
					keyValuePair.Value.GetComponent<AchievementWidget>().ActivateNewlyAchievedFlourish(num3 + (float)num * num2);
					num++;
				}
				else
				{
					keyValuePair.Value.GetComponent<AchievementWidget>().SetAchievedNow();
				}
			}
			else if (flag2)
			{
				keyValuePair.Value.GetComponent<AchievementWidget>().SetAchievedBefore();
			}
			else if (data == null)
			{
				keyValuePair.Value.GetComponent<AchievementWidget>().SetNeverAchieved();
			}
			else
			{
				keyValuePair.Value.GetComponent<AchievementWidget>().SetNotAchieved();
			}
		}
		if (newlyAchieved != null && newlyAchieved.Length != 0)
		{
			base.StartCoroutine(this.ShowAchievementVeil());
			base.StartCoroutine(this.ClearAchievementVeil(num3 + (float)num * num2));
			return;
		}
		this.InstantClearAchievementVeils();
	}

	// Token: 0x060069A2 RID: 27042 RVA: 0x0027B0DC File Offset: 0x002792DC
	private void DisplayInfoBlock(RetiredColonyData data, GameObject container)
	{
		container.GetComponent<HierarchyReferences>().GetReference<LocText>("ColonyNameLabel").SetText(data.colonyName);
		container.GetComponent<HierarchyReferences>().GetReference<LocText>("CycleCountLabel").SetText(string.Format(UI.RETIRED_COLONY_INFO_SCREEN.CYCLE_COUNT, data.cycleCount.ToString()));
	}

	// Token: 0x060069A3 RID: 27043 RVA: 0x0027B138 File Offset: 0x00279338
	private void CorrectTimelapseImageSize(Sprite sprite)
	{
		Vector2 sizeDelta = this.slideshow.transform.parent.GetComponent<RectTransform>().sizeDelta;
		Vector2 fittedSize = this.slideshow.GetFittedSize(sprite, sizeDelta.x, sizeDelta.y);
		LayoutElement component = this.slideshow.GetComponent<LayoutElement>();
		if (fittedSize.y > component.preferredHeight)
		{
			component.minHeight = component.preferredHeight / (fittedSize.y / fittedSize.x);
			component.minHeight = component.preferredHeight;
			return;
		}
		component.minWidth = (component.preferredWidth = fittedSize.x);
		component.minHeight = (component.preferredHeight = fittedSize.y);
	}

	// Token: 0x060069A4 RID: 27044 RVA: 0x0027B1E4 File Offset: 0x002793E4
	private void DisplayTimelapse(RetiredColonyData data, GameObject container)
	{
		container.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.RETIRED_COLONY_INFO_SCREEN.TITLES.TIMELAPSE);
		RectTransform reference = container.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Worlds");
		this.DisplayWorlds(data, reference.gameObject);
		RectTransform reference2 = container.GetComponent<HierarchyReferences>().GetReference<RectTransform>("PlayIcon");
		this.slideshow = container.GetComponent<HierarchyReferences>().GetReference<Slideshow>("Slideshow");
		this.slideshow.updateType = SlideshowUpdateType.loadOnDemand;
		this.slideshow.SetPaused(true);
		this.slideshow.onBeforePlay = delegate()
		{
			this.LoadSlideshow(data);
		};
		this.slideshow.onEndingPlay = delegate()
		{
			this.LoadScreenshot(data, this.focusedWorld);
		};
		if (!this.LoadScreenshot(data, this.focusedWorld))
		{
			this.slideshow.gameObject.SetActive(false);
			reference2.gameObject.SetActive(false);
			return;
		}
		this.slideshow.gameObject.SetActive(true);
		reference2.gameObject.SetActive(true);
	}

	// Token: 0x060069A5 RID: 27045 RVA: 0x0027B304 File Offset: 0x00279504
	private void DisplayDuplicants(RetiredColonyData data, GameObject container, int range_min = -1, int range_max = -1)
	{
		for (int i = container.transform.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.DestroyImmediate(container.transform.GetChild(i).gameObject);
		}
		for (int j = 0; j < data.Duplicants.Length; j++)
		{
			if (j < range_min || (j > range_max && range_max != -1))
			{
				new GameObject().transform.SetParent(container.transform);
			}
			else
			{
				RetiredColonyData.RetiredDuplicantData retiredDuplicantData = data.Duplicants[j];
				GameObject gameObject = global::Util.KInstantiateUI(this.duplicantPrefab, container, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("NameLabel").SetText(retiredDuplicantData.name);
				component.GetReference<LocText>("AgeLabel").SetText(string.Format(UI.RETIRED_COLONY_INFO_SCREEN.DUPLICANT_AGE, retiredDuplicantData.age.ToString()));
				component.GetReference<LocText>("SkillLabel").SetText(string.Format(UI.RETIRED_COLONY_INFO_SCREEN.SKILL_LEVEL, retiredDuplicantData.skillPointsGained.ToString()));
				SymbolOverrideController reference = component.GetReference<SymbolOverrideController>("SymbolOverrideController");
				reference.RemoveAllSymbolOverrides(0);
				KBatchedAnimController componentInChildren = gameObject.GetComponentInChildren<KBatchedAnimController>();
				componentInChildren.SetSymbolVisiblity("snapTo_neck", false);
				componentInChildren.SetSymbolVisiblity("snapTo_goggles", false);
				componentInChildren.SetSymbolVisiblity("snapTo_hat", false);
				componentInChildren.SetSymbolVisiblity("snapTo_headfx", false);
				componentInChildren.SetSymbolVisiblity("snapTo_hat_hair", false);
				foreach (KeyValuePair<string, string> keyValuePair in retiredDuplicantData.accessories)
				{
					if (Db.Get().Accessories.Exists(keyValuePair.Value))
					{
						KAnim.Build.Symbol symbol = Db.Get().Accessories.Get(keyValuePair.Value).symbol;
						AccessorySlot accessorySlot = Db.Get().AccessorySlots.Get(keyValuePair.Key);
						reference.AddSymbolOverride(accessorySlot.targetSymbolId, symbol, 0);
						gameObject.GetComponentInChildren<KBatchedAnimController>().SetSymbolVisiblity(keyValuePair.Key, true);
					}
				}
				reference.ApplyOverrides();
			}
		}
		base.StartCoroutine(this.ActivatePortraitsWhenReady(container));
	}

	// Token: 0x060069A6 RID: 27046 RVA: 0x0027B550 File Offset: 0x00279750
	private IEnumerator ActivatePortraitsWhenReady(GameObject container)
	{
		yield return 0;
		if (container == null)
		{
			global::Debug.LogError("RetiredColonyInfoScreen minion container is null");
		}
		else
		{
			for (int i = 0; i < container.transform.childCount; i++)
			{
				KBatchedAnimController componentInChildren = container.transform.GetChild(i).GetComponentInChildren<KBatchedAnimController>();
				if (componentInChildren != null)
				{
					componentInChildren.transform.localScale = Vector3.one;
				}
			}
		}
		yield break;
	}

	// Token: 0x060069A7 RID: 27047 RVA: 0x0027B560 File Offset: 0x00279760
	private void DisplayBuildings(RetiredColonyData data, GameObject container)
	{
		for (int i = container.transform.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(container.transform.GetChild(i).gameObject);
		}
		data.buildings.Sort(delegate(global::Tuple<string, int> a, global::Tuple<string, int> b)
		{
			if (a.second > b.second)
			{
				return 1;
			}
			if (a.second == b.second)
			{
				return 0;
			}
			return -1;
		});
		data.buildings.Reverse();
		foreach (global::Tuple<string, int> tuple in data.buildings)
		{
			GameObject prefab = Assets.GetPrefab(tuple.first);
			if (!(prefab == null))
			{
				HierarchyReferences component = global::Util.KInstantiateUI(this.buildingPrefab, container, true).GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("NameLabel").SetText(GameUtil.ApplyBoldString(prefab.GetProperName()));
				component.GetReference<LocText>("CountLabel").SetText(string.Format(UI.RETIRED_COLONY_INFO_SCREEN.BUILDING_COUNT, tuple.second.ToString()));
				global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(prefab, "ui", false);
				component.GetReference<Image>("Portrait").sprite = uisprite.first;
			}
		}
	}

	// Token: 0x060069A8 RID: 27048 RVA: 0x0027B6AC File Offset: 0x002798AC
	private void DisplayWorlds(RetiredColonyData data, GameObject container)
	{
		container.SetActive(data.worldIdentities.Count > 0);
		for (int i = container.transform.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(container.transform.GetChild(i).gameObject);
		}
		if (data.worldIdentities.Count <= 0)
		{
			return;
		}
		using (Dictionary<string, string>.Enumerator enumerator = data.worldIdentities.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> worldPair = enumerator.Current;
				GameObject gameObject = global::Util.KInstantiateUI(this.worldPrefab, container, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				ProcGen.World worldData = SettingsCache.worlds.GetWorldData(worldPair.Value);
				Sprite sprite = (worldData != null) ? ColonyDestinationAsteroidBeltData.GetUISprite(worldData.asteroidIcon) : null;
				if (sprite != null)
				{
					component.GetReference<Image>("Portrait").sprite = sprite;
				}
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.focusedWorld = worldPair.Key;
					this.LoadScreenshot(data, this.focusedWorld);
				};
			}
		}
	}

	// Token: 0x060069A9 RID: 27049 RVA: 0x0027B7F8 File Offset: 0x002799F8
	private IEnumerator ComputeSizeStatGrid()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		GridLayoutGroup component = this.statsContainer.GetComponent<GridLayoutGroup>();
		component.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		component.constraintCount = ((Screen.width < 1920) ? 2 : 3);
		yield return SequenceUtil.WaitForEndOfFrame;
		float num = base.gameObject.rectTransform().rect.width - this.explorerRoot.transform.parent.rectTransform().rect.width - 50f;
		num = Mathf.Min(830f, num);
		this.achievementsSection.GetComponent<LayoutElement>().preferredWidth = num;
		yield break;
	}

	// Token: 0x060069AA RID: 27050 RVA: 0x0027B807 File Offset: 0x00279A07
	private IEnumerator ComputeSizeExplorerGrid()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		GridLayoutGroup component = this.explorerGrid.GetComponent<GridLayoutGroup>();
		component.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		component.constraintCount = ((Screen.width < 1920) ? 2 : 3);
		yield return SequenceUtil.WaitForEndOfFrame;
		float num = base.gameObject.rectTransform().rect.width - this.explorerRoot.transform.parent.rectTransform().rect.width - 50f;
		num = Mathf.Min(830f, num);
		this.achievementsSection.GetComponent<LayoutElement>().preferredWidth = num;
		yield break;
	}

	// Token: 0x060069AB RID: 27051 RVA: 0x0027B818 File Offset: 0x00279A18
	private void DisplayStatistics(RetiredColonyData data)
	{
		GameObject gameObject = global::Util.KInstantiateUI(this.specialMediaBlock, this.statsContainer, true);
		this.activeColonyWidgetContainers.Add(gameObject);
		this.activeColonyWidgets.Add("timelapse", gameObject);
		this.DisplayTimelapse(data, gameObject);
		GameObject duplicantBlock = global::Util.KInstantiateUI(this.tallFeatureBlock, this.statsContainer, true);
		this.activeColonyWidgetContainers.Add(duplicantBlock);
		this.activeColonyWidgets.Add("duplicants", duplicantBlock);
		duplicantBlock.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.RETIRED_COLONY_INFO_SCREEN.TITLES.DUPLICANTS);
		PageView pageView = duplicantBlock.GetComponentInChildren<PageView>();
		pageView.OnChangePage = delegate(int page)
		{
			this.DisplayDuplicants(data, duplicantBlock.GetComponent<HierarchyReferences>().GetReference("Content").gameObject, page * pageView.ChildrenPerPage, (page + 1) * pageView.ChildrenPerPage);
		};
		this.DisplayDuplicants(data, duplicantBlock.GetComponent<HierarchyReferences>().GetReference("Content").gameObject, -1, -1);
		GameObject gameObject2 = global::Util.KInstantiateUI(this.tallFeatureBlock, this.statsContainer, true);
		this.activeColonyWidgetContainers.Add(gameObject2);
		this.activeColonyWidgets.Add("buildings", gameObject2);
		gameObject2.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.RETIRED_COLONY_INFO_SCREEN.TITLES.BUILDINGS);
		this.DisplayBuildings(data, gameObject2.GetComponent<HierarchyReferences>().GetReference("Content").gameObject);
		int num = 2;
		for (int i = 0; i < data.Stats.Length; i += num)
		{
			GameObject gameObject3 = global::Util.KInstantiateUI(this.standardStatBlock, this.statsContainer, true);
			this.activeColonyWidgetContainers.Add(gameObject3);
			for (int j = 0; j < num; j++)
			{
				if (i + j <= data.Stats.Length - 1)
				{
					RetiredColonyData.RetiredColonyStatistic retiredColonyStatistic = data.Stats[i + j];
					this.ConfigureGraph(this.GetStatistic(retiredColonyStatistic.id, data), gameObject3);
				}
			}
		}
		base.StartCoroutine(this.ComputeSizeStatGrid());
	}

	// Token: 0x060069AC RID: 27052 RVA: 0x0027BA3C File Offset: 0x00279C3C
	private void ConfigureGraph(RetiredColonyData.RetiredColonyStatistic statistic, GameObject layoutBlockGameObject)
	{
		GameObject gameObject = global::Util.KInstantiateUI(this.lineGraphPrefab, layoutBlockGameObject, true);
		this.activeColonyWidgets.Add(statistic.name, gameObject);
		GraphBase componentInChildren = gameObject.GetComponentInChildren<GraphBase>();
		componentInChildren.graphName = statistic.name;
		componentInChildren.label_title.SetText(componentInChildren.graphName);
		componentInChildren.axis_x.name = statistic.nameX;
		componentInChildren.axis_y.name = statistic.nameY;
		componentInChildren.label_x.SetText(componentInChildren.axis_x.name);
		componentInChildren.label_y.SetText(componentInChildren.axis_y.name);
		LineLayer componentInChildren2 = gameObject.GetComponentInChildren<LineLayer>();
		componentInChildren.axis_y.min_value = 0f;
		componentInChildren.axis_y.max_value = statistic.GetByMaxValue().second * 1.2f;
		if (float.IsNaN(componentInChildren.axis_y.max_value))
		{
			componentInChildren.axis_y.max_value = 1f;
		}
		componentInChildren.axis_x.min_value = 0f;
		componentInChildren.axis_x.max_value = statistic.GetByMaxKey().first;
		componentInChildren.axis_x.guide_frequency = (componentInChildren.axis_x.max_value - componentInChildren.axis_x.min_value) / 10f;
		componentInChildren.axis_y.guide_frequency = (componentInChildren.axis_y.max_value - componentInChildren.axis_y.min_value) / 10f;
		componentInChildren.RefreshGuides();
		global::Tuple<float, float>[] value = statistic.value;
		GraphedLine graphedLine = componentInChildren2.NewLine(value, statistic.id);
		if (this.statColors.ContainsKey(statistic.id))
		{
			componentInChildren2.line_formatting[componentInChildren2.line_formatting.Length - 1].color = this.statColors[statistic.id];
		}
		graphedLine.line_renderer.color = componentInChildren2.line_formatting[componentInChildren2.line_formatting.Length - 1].color;
	}

	// Token: 0x060069AD RID: 27053 RVA: 0x0027BC24 File Offset: 0x00279E24
	private RetiredColonyData.RetiredColonyStatistic GetStatistic(string id, RetiredColonyData data)
	{
		foreach (RetiredColonyData.RetiredColonyStatistic retiredColonyStatistic in data.Stats)
		{
			if (retiredColonyStatistic.id == id)
			{
				return retiredColonyStatistic;
			}
		}
		return null;
	}

	// Token: 0x060069AE RID: 27054 RVA: 0x0027BC5C File Offset: 0x00279E5C
	private void ToggleExplorer(bool active)
	{
		if (active && Game.Instance == null)
		{
			WorldGen.LoadSettings(false);
		}
		this.ConfigButtons();
		this.explorerRoot.SetActive(active);
		this.colonyDataRoot.SetActive(!active);
		if (!this.explorerGridConfigured)
		{
			this.explorerGridConfigured = true;
			base.StartCoroutine(this.ComputeSizeExplorerGrid());
		}
		this.explorerHeaderContainer.SetActive(active);
		this.colonyHeaderContainer.SetActive(!active);
		if (active)
		{
			this.colonyDataRoot.transform.parent.rectTransform().SetPosition(new Vector3(this.colonyDataRoot.transform.parent.rectTransform().position.x, 0f, 0f));
		}
		this.UpdateAchievementData(null, null);
		this.explorerSearch.text = "";
	}

	// Token: 0x060069AF RID: 27055 RVA: 0x0027BD3C File Offset: 0x00279F3C
	private void LoadExplorer()
	{
		if (SaveGame.Instance != null)
		{
			return;
		}
		this.ToggleExplorer(true);
		this.retiredColonyData = RetireColonyUtility.LoadRetiredColonies(false);
		RetiredColonyData[] array = this.retiredColonyData;
		for (int i = 0; i < array.Length; i++)
		{
			RetiredColonyData retiredColonyData = array[i];
			RetiredColonyData data = retiredColonyData;
			GameObject gameObject = global::Util.KInstantiateUI(this.colonyButtonPrefab, this.explorerGrid, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			Sprite sprite = RetireColonyUtility.LoadRetiredColonyPreview(RetireColonyUtility.StripInvalidCharacters(data.colonyName), data.startWorld);
			Image reference = component.GetReference<Image>("ColonyImage");
			RectTransform reference2 = component.GetReference<RectTransform>("PreviewUnavailableText");
			if (sprite != null)
			{
				reference.enabled = true;
				reference.sprite = sprite;
				reference2.gameObject.SetActive(false);
			}
			else
			{
				reference.enabled = false;
				reference2.gameObject.SetActive(true);
			}
			component.GetReference<LocText>("ColonyNameLabel").SetText(retiredColonyData.colonyName);
			component.GetReference<LocText>("CycleCountLabel").SetText(string.Format(UI.RETIRED_COLONY_INFO_SCREEN.CYCLE_COUNT, retiredColonyData.cycleCount.ToString()));
			component.GetReference<LocText>("DateLabel").SetText(retiredColonyData.date);
			gameObject.GetComponent<KButton>().onClick += delegate()
			{
				this.LoadColony(data);
			};
			string key = retiredColonyData.colonyName;
			int num = 0;
			while (this.explorerColonyWidgets.ContainsKey(key))
			{
				num++;
				key = retiredColonyData.colonyName + "_" + num.ToString();
			}
			this.explorerColonyWidgets.Add(key, gameObject);
		}
	}

	// Token: 0x060069B0 RID: 27056 RVA: 0x0027BEF0 File Offset: 0x0027A0F0
	private void FilterExplorer(string search)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.explorerColonyWidgets)
		{
			if (string.IsNullOrEmpty(search) || keyValuePair.Key.ToUpper().Contains(search.ToUpper()))
			{
				keyValuePair.Value.SetActive(true);
			}
			else
			{
				keyValuePair.Value.SetActive(false);
			}
		}
	}

	// Token: 0x060069B1 RID: 27057 RVA: 0x0027BF7C File Offset: 0x0027A17C
	private void FilterColonyData(string search)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.activeColonyWidgets)
		{
			if (string.IsNullOrEmpty(search) || keyValuePair.Key.ToUpper().Contains(search.ToUpper()))
			{
				keyValuePair.Value.SetActive(true);
			}
			else
			{
				keyValuePair.Value.SetActive(false);
			}
		}
	}

	// Token: 0x060069B2 RID: 27058 RVA: 0x0027C008 File Offset: 0x0027A208
	private void FilterAchievements(string search)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.achievementEntries)
		{
			if (string.IsNullOrEmpty(search) || Db.Get().ColonyAchievements.Get(keyValuePair.Key).Name.ToUpper().Contains(search.ToUpper()))
			{
				keyValuePair.Value.SetActive(true);
			}
			else
			{
				keyValuePair.Value.SetActive(false);
			}
		}
	}

	// Token: 0x040047D4 RID: 18388
	public static RetiredColonyInfoScreen Instance;

	// Token: 0x040047D5 RID: 18389
	private bool wasPixelPerfect;

	// Token: 0x040047D6 RID: 18390
	[Header("Screen")]
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040047D7 RID: 18391
	[Header("Header References")]
	[SerializeField]
	private GameObject explorerHeaderContainer;

	// Token: 0x040047D8 RID: 18392
	[SerializeField]
	private GameObject colonyHeaderContainer;

	// Token: 0x040047D9 RID: 18393
	[SerializeField]
	private LocText colonyName;

	// Token: 0x040047DA RID: 18394
	[SerializeField]
	private LocText cycleCount;

	// Token: 0x040047DB RID: 18395
	[Header("Timelapse References")]
	[SerializeField]
	private Slideshow slideshow;

	// Token: 0x040047DC RID: 18396
	[SerializeField]
	private GameObject worldPrefab;

	// Token: 0x040047DD RID: 18397
	private string focusedWorld;

	// Token: 0x040047DE RID: 18398
	private string[] currentSlideshowFiles = new string[0];

	// Token: 0x040047DF RID: 18399
	[Header("Main Layout")]
	[SerializeField]
	private GameObject coloniesSection;

	// Token: 0x040047E0 RID: 18400
	[SerializeField]
	private GameObject achievementsSection;

	// Token: 0x040047E1 RID: 18401
	[Header("Achievement References")]
	[SerializeField]
	private GameObject achievementsContainer;

	// Token: 0x040047E2 RID: 18402
	[SerializeField]
	private GameObject achievementsPrefab;

	// Token: 0x040047E3 RID: 18403
	[SerializeField]
	private GameObject victoryAchievementsPrefab;

	// Token: 0x040047E4 RID: 18404
	[SerializeField]
	private KInputTextField achievementSearch;

	// Token: 0x040047E5 RID: 18405
	[SerializeField]
	private KButton clearAchievementSearchButton;

	// Token: 0x040047E6 RID: 18406
	[SerializeField]
	private GameObject[] achievementVeils;

	// Token: 0x040047E7 RID: 18407
	[Header("Duplicant References")]
	[SerializeField]
	private GameObject duplicantPrefab;

	// Token: 0x040047E8 RID: 18408
	[Header("Building References")]
	[SerializeField]
	private GameObject buildingPrefab;

	// Token: 0x040047E9 RID: 18409
	[Header("Colony Stat References")]
	[SerializeField]
	private GameObject statsContainer;

	// Token: 0x040047EA RID: 18410
	[SerializeField]
	private GameObject specialMediaBlock;

	// Token: 0x040047EB RID: 18411
	[SerializeField]
	private GameObject tallFeatureBlock;

	// Token: 0x040047EC RID: 18412
	[SerializeField]
	private GameObject standardStatBlock;

	// Token: 0x040047ED RID: 18413
	[SerializeField]
	private GameObject lineGraphPrefab;

	// Token: 0x040047EE RID: 18414
	public RetiredColonyData[] retiredColonyData;

	// Token: 0x040047EF RID: 18415
	[Header("Explorer References")]
	[SerializeField]
	private GameObject colonyScroll;

	// Token: 0x040047F0 RID: 18416
	[SerializeField]
	private GameObject explorerRoot;

	// Token: 0x040047F1 RID: 18417
	[SerializeField]
	private GameObject explorerGrid;

	// Token: 0x040047F2 RID: 18418
	[SerializeField]
	private GameObject colonyDataRoot;

	// Token: 0x040047F3 RID: 18419
	[SerializeField]
	private GameObject colonyButtonPrefab;

	// Token: 0x040047F4 RID: 18420
	[SerializeField]
	private KInputTextField explorerSearch;

	// Token: 0x040047F5 RID: 18421
	[SerializeField]
	private KButton clearExplorerSearchButton;

	// Token: 0x040047F6 RID: 18422
	[Header("Navigation Buttons")]
	[SerializeField]
	private KButton closeScreenButton;

	// Token: 0x040047F7 RID: 18423
	[SerializeField]
	private KButton viewOtherColoniesButton;

	// Token: 0x040047F8 RID: 18424
	[SerializeField]
	private KButton quitToMainMenuButton;

	// Token: 0x040047F9 RID: 18425
	[SerializeField]
	private GameObject disabledPlatformUnlocks;

	// Token: 0x040047FA RID: 18426
	private bool explorerGridConfigured;

	// Token: 0x040047FB RID: 18427
	private Dictionary<string, GameObject> achievementEntries = new Dictionary<string, GameObject>();

	// Token: 0x040047FC RID: 18428
	private List<GameObject> activeColonyWidgetContainers = new List<GameObject>();

	// Token: 0x040047FD RID: 18429
	private Dictionary<string, GameObject> activeColonyWidgets = new Dictionary<string, GameObject>();

	// Token: 0x040047FE RID: 18430
	private const float maxAchievementWidth = 830f;

	// Token: 0x040047FF RID: 18431
	private Canvas canvasRef;

	// Token: 0x04004800 RID: 18432
	private Dictionary<string, Color> statColors = new Dictionary<string, Color>
	{
		{
			RetiredColonyData.DataIDs.OxygenProduced,
			new Color(0.17f, 0.91f, 0.91f, 1f)
		},
		{
			RetiredColonyData.DataIDs.OxygenConsumed,
			new Color(0.17f, 0.91f, 0.91f, 1f)
		},
		{
			RetiredColonyData.DataIDs.CaloriesProduced,
			new Color(0.24f, 0.49f, 0.32f, 1f)
		},
		{
			RetiredColonyData.DataIDs.CaloriesRemoved,
			new Color(0.24f, 0.49f, 0.32f, 1f)
		},
		{
			RetiredColonyData.DataIDs.PowerProduced,
			new Color(0.98f, 0.69f, 0.23f, 1f)
		},
		{
			RetiredColonyData.DataIDs.PowerWasted,
			new Color(0.82f, 0.3f, 0.35f, 1f)
		},
		{
			RetiredColonyData.DataIDs.WorkTime,
			new Color(0.99f, 0.51f, 0.28f, 1f)
		},
		{
			RetiredColonyData.DataIDs.TravelTime,
			new Color(0.55f, 0.55f, 0.75f, 1f)
		},
		{
			RetiredColonyData.DataIDs.AverageWorkTime,
			new Color(0.99f, 0.51f, 0.28f, 1f)
		},
		{
			RetiredColonyData.DataIDs.AverageTravelTime,
			new Color(0.55f, 0.55f, 0.75f, 1f)
		},
		{
			RetiredColonyData.DataIDs.LiveDuplicants,
			new Color(0.98f, 0.69f, 0.23f, 1f)
		},
		{
			RetiredColonyData.DataIDs.RocketsInFlight,
			new Color(0.9f, 0.9f, 0.16f, 1f)
		},
		{
			RetiredColonyData.DataIDs.AverageStressCreated,
			new Color(0.8f, 0.32f, 0.33f, 1f)
		},
		{
			RetiredColonyData.DataIDs.AverageStressRemoved,
			new Color(0.8f, 0.32f, 0.33f, 1f)
		},
		{
			RetiredColonyData.DataIDs.AverageGerms,
			new Color(0.68f, 0.79f, 0.18f, 1f)
		},
		{
			RetiredColonyData.DataIDs.DomesticatedCritters,
			new Color(0.62f, 0.31f, 0.47f, 1f)
		},
		{
			RetiredColonyData.DataIDs.WildCritters,
			new Color(0.62f, 0.31f, 0.47f, 1f)
		}
	};

	// Token: 0x04004801 RID: 18433
	private Dictionary<string, GameObject> explorerColonyWidgets = new Dictionary<string, GameObject>();
}
