using System;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using Klei;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000D0A RID: 3338
public class PauseScreen : KModalButtonMenu
{
	// Token: 0x17000766 RID: 1894
	// (get) Token: 0x060067C9 RID: 26569 RVA: 0x0026C665 File Offset: 0x0026A865
	public static PauseScreen Instance
	{
		get
		{
			return PauseScreen.instance;
		}
	}

	// Token: 0x060067CA RID: 26570 RVA: 0x0026C66C File Offset: 0x0026A86C
	public static void DestroyInstance()
	{
		PauseScreen.instance = null;
	}

	// Token: 0x060067CB RID: 26571 RVA: 0x0026C674 File Offset: 0x0026A874
	protected override void OnPrefabInit()
	{
		this.keepMenuOpen = true;
		base.OnPrefabInit();
		this.ConfigureButtonInfos();
		this.closeButton.onClick += this.OnResume;
		PauseScreen.instance = this;
		this.Show(false);
	}

	// Token: 0x060067CC RID: 26572 RVA: 0x0026C6B0 File Offset: 0x0026A8B0
	private void ConfigureButtonInfos()
	{
		if (!GenericGameSettings.instance.demoMode)
		{
			this.buttons = new KButtonMenu.ButtonInfo[]
			{
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.RESUME, global::Action.NumActions, new UnityAction(this.OnResume), null, null),
				new KButtonMenu.ButtonInfo(this.recentlySaved ? UI.FRONTEND.PAUSE_SCREEN.ALREADY_SAVED : UI.FRONTEND.PAUSE_SCREEN.SAVE, global::Action.NumActions, new UnityAction(this.OnSave), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.SAVEAS, global::Action.NumActions, new UnityAction(this.OnSaveAs), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.LOAD, global::Action.NumActions, new UnityAction(this.OnLoad), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.OPTIONS, global::Action.NumActions, new UnityAction(this.OnOptions), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.COLONY_SUMMARY, global::Action.NumActions, new UnityAction(this.OnColonySummary), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.LOCKERMENU, global::Action.NumActions, new UnityAction(this.OnLockerMenu), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.QUIT, global::Action.NumActions, new UnityAction(this.OnQuit), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, global::Action.NumActions, new UnityAction(this.OnDesktopQuit), null, null)
			};
			return;
		}
		this.buttons = new KButtonMenu.ButtonInfo[]
		{
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.RESUME, global::Action.NumActions, new UnityAction(this.OnResume), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.OPTIONS, global::Action.NumActions, new UnityAction(this.OnOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.QUIT, global::Action.NumActions, new UnityAction(this.OnQuit), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, global::Action.NumActions, new UnityAction(this.OnDesktopQuit), null, null)
		};
	}

	// Token: 0x060067CD RID: 26573 RVA: 0x0026C8D8 File Offset: 0x0026AAD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.clipboard.GetText = new Func<string>(this.GetClipboardText);
		this.title.SetText(UI.FRONTEND.PAUSE_SCREEN.TITLE);
		try
		{
			string settingsCoordinate = CustomGameSettings.Instance.GetSettingsCoordinate();
			string[] array = CustomGameSettings.ParseSettingCoordinate(settingsCoordinate);
			this.worldSeed.SetText(string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, settingsCoordinate));
			this.worldSeed.GetComponent<ToolTip>().toolTip = string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED_TOOLTIP, new object[]
			{
				array[1],
				array[2],
				array[3],
				array[4],
				array[5]
			});
		}
		catch (Exception arg)
		{
			global::Debug.LogWarning(string.Format("Failed to load Coordinates on ClusterLayout {0}, please report this error on the forums", arg));
			CustomGameSettings.Instance.Print();
			global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
			this.worldSeed.SetText(string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, "0"));
		}
	}

	// Token: 0x060067CE RID: 26574 RVA: 0x0026CA00 File Offset: 0x0026AC00
	public override float GetSortKey()
	{
		return 30f;
	}

	// Token: 0x060067CF RID: 26575 RVA: 0x0026CA08 File Offset: 0x0026AC08
	private string GetClipboardText()
	{
		string result;
		try
		{
			result = CustomGameSettings.Instance.GetSettingsCoordinate();
		}
		catch
		{
			result = "";
		}
		return result;
	}

	// Token: 0x060067D0 RID: 26576 RVA: 0x0026CA3C File Offset: 0x0026AC3C
	private void OnResume()
	{
		this.Show(false);
	}

	// Token: 0x060067D1 RID: 26577 RVA: 0x0026CA48 File Offset: 0x0026AC48
	protected override void OnShow(bool show)
	{
		this.recentlySaved = false;
		this.ConfigureButtonInfos();
		base.OnShow(show);
		if (show)
		{
			this.RefreshButtons();
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().ESCPauseSnapshot);
			MusicManager.instance.OnEscapeMenu(true);
			MusicManager.instance.PlaySong("Music_ESC_Menu", false);
			this.RefreshDLCActivationButtons();
			return;
		}
		ToolTipScreen.Instance.ClearToolTip(this.closeButton.GetComponent<ToolTip>());
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().ESCPauseSnapshot, STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.OnEscapeMenu(false);
		if (MusicManager.instance.SongIsPlaying("Music_ESC_Menu"))
		{
			MusicManager.instance.StopSong("Music_ESC_Menu", true, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x060067D2 RID: 26578 RVA: 0x0026CB01 File Offset: 0x0026AD01
	private void OnOptions()
	{
		base.ActivateChildScreen(this.optionsScreen.gameObject);
	}

	// Token: 0x060067D3 RID: 26579 RVA: 0x0026CB15 File Offset: 0x0026AD15
	private void OnSaveAs()
	{
		base.ActivateChildScreen(this.saveScreenPrefab.gameObject);
	}

	// Token: 0x060067D4 RID: 26580 RVA: 0x0026CB2C File Offset: 0x0026AD2C
	private void OnSave()
	{
		string filename = SaveLoader.GetActiveSaveFilePath();
		if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
		{
			base.gameObject.SetActive(false);
			((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.OVERWRITEMESSAGE, System.IO.Path.GetFileNameWithoutExtension(filename)), delegate
			{
				this.DoSave(filename);
				this.gameObject.SetActive(true);
			}, new System.Action(this.OnCancelPopup), null, null, null, null, null, null);
			return;
		}
		this.OnSaveAs();
	}

	// Token: 0x060067D5 RID: 26581 RVA: 0x0026CBED File Offset: 0x0026ADED
	public void OnSaveComplete()
	{
		this.recentlySaved = true;
		this.ConfigureButtonInfos();
		this.RefreshButtons();
	}

	// Token: 0x060067D6 RID: 26582 RVA: 0x0026CC04 File Offset: 0x0026AE04
	private void DoSave(string filename)
	{
		try
		{
			SaveLoader.Instance.Save(filename, false, true);
			this.OnSaveComplete();
		}
		catch (IOException ex)
		{
			IOException e2 = ex;
			IOException e = e2;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.IO_ERROR, e.ToString()), delegate
			{
				this.Deactivate();
			}, null, UI.FRONTEND.SAVESCREEN.REPORT_BUG, delegate
			{
				KCrashReporter.ReportError(e.Message, e.StackTrace.ToString(), null, null, null, true, new string[]
				{
					KCrashReporter.CRASH_CATEGORY.FILEIO
				}, null);
			}, null, null, null, null);
		}
	}

	// Token: 0x060067D7 RID: 26583 RVA: 0x0026CCBC File Offset: 0x0026AEBC
	private void ConfirmDecision(string questionText, string primaryButtonText, System.Action primaryButtonAction, string alternateButtonText = null, System.Action alternateButtonAction = null)
	{
		base.gameObject.SetActive(false);
		((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(questionText, primaryButtonAction, new System.Action(this.OnCancelPopup), alternateButtonText, alternateButtonAction, null, primaryButtonText, null, null);
	}

	// Token: 0x060067D8 RID: 26584 RVA: 0x0026CD1F File Offset: 0x0026AF1F
	private void OnLoad()
	{
		base.ActivateChildScreen(this.loadScreenPrefab.gameObject);
	}

	// Token: 0x060067D9 RID: 26585 RVA: 0x0026CD34 File Offset: 0x0026AF34
	private void OnColonySummary()
	{
		RetiredColonyData currentColonyRetiredColonyData = RetireColonyUtility.GetCurrentColonyRetiredColonyData();
		MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, currentColonyRetiredColonyData);
	}

	// Token: 0x060067DA RID: 26586 RVA: 0x0026CD61 File Offset: 0x0026AF61
	private void OnLockerMenu()
	{
		LockerMenuScreen.Instance.Show(true);
	}

	// Token: 0x060067DB RID: 26587 RVA: 0x0026CD6E File Offset: 0x0026AF6E
	private void OnQuit()
	{
		this.ConfirmDecision(UI.FRONTEND.MAINMENU.QUITCONFIRM, UI.FRONTEND.MAINMENU.SAVEANDQUITTITLE, delegate
		{
			this.OnQuitConfirm(true);
		}, UI.FRONTEND.MAINMENU.QUIT, delegate
		{
			this.OnQuitConfirm(false);
		});
	}

	// Token: 0x060067DC RID: 26588 RVA: 0x0026CDAC File Offset: 0x0026AFAC
	private void OnDesktopQuit()
	{
		this.ConfirmDecision(UI.FRONTEND.MAINMENU.DESKTOPQUITCONFIRM, UI.FRONTEND.MAINMENU.SAVEANDQUITDESKTOP, delegate
		{
			this.OnDesktopQuitConfirm(true);
		}, UI.FRONTEND.MAINMENU.QUIT, delegate
		{
			this.OnDesktopQuitConfirm(false);
		});
	}

	// Token: 0x060067DD RID: 26589 RVA: 0x0026CDEA File Offset: 0x0026AFEA
	private void OnCancelPopup()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x060067DE RID: 26590 RVA: 0x0026CDF8 File Offset: 0x0026AFF8
	private void OnLoadConfirm()
	{
		LoadingOverlay.Load(delegate
		{
			LoadScreen.ForceStopGame();
			this.Deactivate();
			App.LoadScene("frontend");
		});
	}

	// Token: 0x060067DF RID: 26591 RVA: 0x0026CE0B File Offset: 0x0026B00B
	private void OnRetireConfirm()
	{
		RetireColonyUtility.SaveColonySummaryData();
	}

	// Token: 0x060067E0 RID: 26592 RVA: 0x0026CE14 File Offset: 0x0026B014
	private void OnQuitConfirm(bool saveFirst)
	{
		if (saveFirst)
		{
			string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
			if (!string.IsNullOrEmpty(activeSaveFilePath) && File.Exists(activeSaveFilePath))
			{
				this.DoSave(activeSaveFilePath);
			}
			else
			{
				this.OnSaveAs();
			}
		}
		LoadingOverlay.Load(delegate
		{
			this.Deactivate();
			PauseScreen.TriggerQuitGame();
		});
	}

	// Token: 0x060067E1 RID: 26593 RVA: 0x0026CE5C File Offset: 0x0026B05C
	private void OnDesktopQuitConfirm(bool saveFirst)
	{
		if (saveFirst)
		{
			string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
			if (!string.IsNullOrEmpty(activeSaveFilePath) && File.Exists(activeSaveFilePath))
			{
				this.DoSave(activeSaveFilePath);
			}
			else
			{
				this.OnSaveAs();
			}
		}
		App.Quit();
	}

	// Token: 0x060067E2 RID: 26594 RVA: 0x0026CE96 File Offset: 0x0026B096
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060067E3 RID: 26595 RVA: 0x0026CEB9 File Offset: 0x0026B0B9
	public static void TriggerQuitGame()
	{
		ThreadedHttps<KleiMetrics>.Instance.EndGame();
		LoadScreen.ForceStopGame();
		App.LoadScene("frontend");
	}

	// Token: 0x060067E4 RID: 26596 RVA: 0x0026CED4 File Offset: 0x0026B0D4
	private void RefreshDLCActivationButtons()
	{
		foreach (KeyValuePair<string, DlcManager.DlcInfo> keyValuePair in DlcManager.DLC_PACKS)
		{
			if (!(keyValuePair.Value.id == "DLC3_ID") && !this.dlcActivationButtons.ContainsKey(keyValuePair.Key))
			{
				GameObject gameObject = global::Util.KInstantiateUI(this.dlcActivationButtonPrefab, this.dlcActivationButtonPrefab.transform.parent.gameObject, true);
				Sprite sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(keyValuePair.Key));
				gameObject.GetComponent<Image>().sprite = sprite;
				gameObject.GetComponent<MultiToggle>().states[0].sprite = sprite;
				gameObject.GetComponent<MultiToggle>().states[1].sprite = sprite;
				this.dlcActivationButtons.Add(keyValuePair.Key, gameObject);
			}
		}
		this.RefreshDLCButton("EXPANSION1_ID", this.dlc1ActivationButton, false);
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.dlcActivationButtons)
		{
			this.RefreshDLCButton(keyValuePair2.Key, keyValuePair2.Value.GetComponent<MultiToggle>(), true);
		}
	}

	// Token: 0x060067E5 RID: 26597 RVA: 0x0026D048 File Offset: 0x0026B248
	private void RefreshDLCButton(string DLCID, MultiToggle button, bool userEditable)
	{
		button.ChangeState(SaveLoader.Instance.IsDLCActiveForCurrentSave(DLCID) ? 1 : 0);
		button.GetComponent<Image>().material = (SaveLoader.Instance.IsDLCActiveForCurrentSave(DLCID) ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		ToolTip component = button.GetComponent<ToolTip>();
		string dlcTitle = DlcManager.GetDlcTitle(DLCID);
		if (!DlcManager.IsContentSubscribed(DLCID))
		{
			component.SetSimpleTooltip(string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_NOT_EDITABLE_TOOLTIP, dlcTitle));
			button.onClick = null;
			return;
		}
		if (userEditable)
		{
			component.SetSimpleTooltip(SaveLoader.Instance.IsDLCActiveForCurrentSave(DLCID) ? string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_ENABLED_TOOLTIP, dlcTitle) : string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_TOOLTIP, dlcTitle));
			button.onClick = delegate()
			{
				this.OnClickAddDLCButton(DLCID);
			};
			return;
		}
		component.SetSimpleTooltip(SaveLoader.Instance.IsDLCActiveForCurrentSave(DLCID) ? string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_ENABLED_TOOLTIP, dlcTitle) : string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_NOT_EDITABLE_TOOLTIP, dlcTitle));
		button.onClick = null;
	}

	// Token: 0x060067E6 RID: 26598 RVA: 0x0026D18C File Offset: 0x0026B38C
	private void OnClickAddDLCButton(string dlcID)
	{
		if (!SaveLoader.Instance.IsDLCActiveForCurrentSave(dlcID))
		{
			this.ConfirmDecision(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.ENABLE_QUESTION, UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.CONFIRM, delegate
			{
				this.OnConfirmAddDLC(dlcID);
			}, null, null);
		}
	}

	// Token: 0x060067E7 RID: 26599 RVA: 0x0026D1E7 File Offset: 0x0026B3E7
	private void OnConfirmAddDLC(string dlcId)
	{
		SaveLoader.Instance.UpgradeActiveSaveDLCInfo(dlcId, true);
	}

	// Token: 0x0400461B RID: 17947
	[SerializeField]
	private OptionsMenuScreen optionsScreen;

	// Token: 0x0400461C RID: 17948
	[SerializeField]
	private SaveScreen saveScreenPrefab;

	// Token: 0x0400461D RID: 17949
	[SerializeField]
	private LoadScreen loadScreenPrefab;

	// Token: 0x0400461E RID: 17950
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400461F RID: 17951
	[SerializeField]
	private LocText title;

	// Token: 0x04004620 RID: 17952
	[SerializeField]
	private LocText worldSeed;

	// Token: 0x04004621 RID: 17953
	[SerializeField]
	private CopyTextFieldToClipboard clipboard;

	// Token: 0x04004622 RID: 17954
	[SerializeField]
	private MultiToggle dlc1ActivationButton;

	// Token: 0x04004623 RID: 17955
	[SerializeField]
	private GameObject dlcActivationButtonPrefab;

	// Token: 0x04004624 RID: 17956
	private Dictionary<string, GameObject> dlcActivationButtons = new Dictionary<string, GameObject>();

	// Token: 0x04004625 RID: 17957
	private float originalTimeScale;

	// Token: 0x04004626 RID: 17958
	private bool recentlySaved;

	// Token: 0x04004627 RID: 17959
	private static PauseScreen instance;
}
