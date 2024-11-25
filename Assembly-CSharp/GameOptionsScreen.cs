using System;
using System.IO;
using Steamworks;
using STRINGS;
using UnityEngine;

// Token: 0x02000C4C RID: 3148
public class GameOptionsScreen : KModalButtonMenu
{
	// Token: 0x060060BA RID: 24762 RVA: 0x0023FDEB File Offset: 0x0023DFEB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060060BB RID: 24763 RVA: 0x0023FDF4 File Offset: 0x0023DFF4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitConfiguration.Init();
		if (SaveGame.Instance != null)
		{
			this.saveConfiguration.ToggleDisabledContent(true);
			this.saveConfiguration.Init();
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
		}
		else
		{
			this.saveConfiguration.ToggleDisabledContent(false);
		}
		this.resetTutorialButton.onClick += this.OnTutorialReset;
		if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.controlsButton.gameObject.SetActive(false);
		}
		else
		{
			this.controlsButton.onClick += this.OnKeyBindings;
		}
		this.sandboxButton.onClick += this.OnUnlockSandboxMode;
		this.doneButton.onClick += this.Deactivate;
		this.closeButton.onClick += this.Deactivate;
		if (this.defaultToCloudSaveToggle != null)
		{
			this.RefreshCloudSaveToggle();
			this.defaultToCloudSaveToggle.GetComponentInChildren<KButton>().onClick += this.OnDefaultToCloudSaveToggle;
		}
		if (this.cloudSavesPanel != null)
		{
			this.cloudSavesPanel.SetActive(SaveLoader.GetCloudSavesAvailable());
		}
		this.cameraSpeedSlider.minValue = 1f;
		this.cameraSpeedSlider.maxValue = 20f;
		this.cameraSpeedSlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnCameraSpeedValueChanged(Mathf.FloorToInt(val));
		});
		this.cameraSpeedSlider.value = this.CameraSpeedToSlider(KPlayerPrefs.GetFloat("CameraSpeed"));
		this.RefreshCameraSliderLabel();
	}

	// Token: 0x060060BC RID: 24764 RVA: 0x0023FF98 File Offset: 0x0023E198
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (SaveGame.Instance != null)
		{
			this.savePanel.SetActive(true);
			this.saveConfiguration.Show(show);
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
		}
		else
		{
			this.savePanel.SetActive(false);
		}
		if (!KPlayerPrefs.HasKey("CameraSpeed"))
		{
			CameraController.SetDefaultCameraSpeed();
		}
	}

	// Token: 0x060060BD RID: 24765 RVA: 0x00240000 File Offset: 0x0023E200
	private float CameraSpeedToSlider(float prefsValue)
	{
		return prefsValue * 10f;
	}

	// Token: 0x060060BE RID: 24766 RVA: 0x00240009 File Offset: 0x0023E209
	private void OnCameraSpeedValueChanged(int sliderValue)
	{
		KPlayerPrefs.SetFloat("CameraSpeed", (float)sliderValue / 10f);
		this.RefreshCameraSliderLabel();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(75424175, null);
		}
	}

	// Token: 0x060060BF RID: 24767 RVA: 0x00240040 File Offset: 0x0023E240
	private void RefreshCameraSliderLabel()
	{
		this.cameraSpeedSliderLabel.text = string.Format(UI.FRONTEND.GAME_OPTIONS_SCREEN.CAMERA_SPEED_LABEL, (KPlayerPrefs.GetFloat("CameraSpeed") * 10f * 10f).ToString());
	}

	// Token: 0x060060C0 RID: 24768 RVA: 0x00240085 File Offset: 0x0023E285
	private void OnDefaultToCloudSaveToggle()
	{
		SaveLoader.SetCloudSavesDefault(!SaveLoader.GetCloudSavesDefault());
		this.RefreshCloudSaveToggle();
	}

	// Token: 0x060060C1 RID: 24769 RVA: 0x0024009C File Offset: 0x0023E29C
	private void RefreshCloudSaveToggle()
	{
		bool cloudSavesDefault = SaveLoader.GetCloudSavesDefault();
		this.defaultToCloudSaveToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(cloudSavesDefault);
	}

	// Token: 0x060060C2 RID: 24770 RVA: 0x002400CF File Offset: 0x0023E2CF
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060060C3 RID: 24771 RVA: 0x002400F4 File Offset: 0x0023E2F4
	private void OnTutorialReset()
	{
		ConfirmDialogScreen component = base.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
		component.PopupConfirmDialog(UI.FRONTEND.OPTIONS_SCREEN.RESET_TUTORIAL_WARNING, delegate
		{
			Tutorial.ResetHiddenTutorialMessages();
		}, delegate
		{
		}, null, null, null, null, null, null);
		component.Activate();
	}

	// Token: 0x060060C4 RID: 24772 RVA: 0x00240174 File Offset: 0x0023E374
	private void OnUnlockSandboxMode()
	{
		ConfirmDialogScreen component = base.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
		string text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.UNLOCK_SANDBOX_WARNING;
		System.Action on_confirm = delegate()
		{
			SaveGame.Instance.sandboxEnabled = true;
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
			TopLeftControlScreen.Instance.UpdateSandboxToggleState();
			this.Deactivate();
		};
		System.Action on_cancel = delegate()
		{
			string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
			string path = SaveGame.Instance.BaseName + UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND + ".sav";
			SaveLoader.Instance.Save(Path.Combine(savePrefixAndCreateFolder, path), false, false);
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
			TopLeftControlScreen.Instance.UpdateSandboxToggleState();
			this.Deactivate();
		};
		string confirm_text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM;
		string cancel_text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM_SAVE_BACKUP;
		component.PopupConfirmDialog(text, on_confirm, on_cancel, UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CANCEL, delegate
		{
		}, null, confirm_text, cancel_text, null);
		component.Activate();
	}

	// Token: 0x060060C5 RID: 24773 RVA: 0x0024020B File Offset: 0x0023E40B
	private void OnKeyBindings()
	{
		base.ActivateChildScreen(this.inputBindingsScreenPrefab.gameObject);
	}

	// Token: 0x060060C6 RID: 24774 RVA: 0x00240220 File Offset: 0x0023E420
	private void SetSandboxModeActive(bool active)
	{
		this.sandboxButton.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(active);
		this.sandboxButton.isInteractable = !active;
		this.sandboxButton.gameObject.GetComponentInParent<CanvasGroup>().alpha = (active ? 0.5f : 1f);
	}

	// Token: 0x0400415E RID: 16734
	[SerializeField]
	private SaveConfigurationScreen saveConfiguration;

	// Token: 0x0400415F RID: 16735
	[SerializeField]
	private UnitConfigurationScreen unitConfiguration;

	// Token: 0x04004160 RID: 16736
	[SerializeField]
	private KButton resetTutorialButton;

	// Token: 0x04004161 RID: 16737
	[SerializeField]
	private KButton controlsButton;

	// Token: 0x04004162 RID: 16738
	[SerializeField]
	private KButton sandboxButton;

	// Token: 0x04004163 RID: 16739
	[SerializeField]
	private ConfirmDialogScreen confirmPrefab;

	// Token: 0x04004164 RID: 16740
	[SerializeField]
	private KButton doneButton;

	// Token: 0x04004165 RID: 16741
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004166 RID: 16742
	[SerializeField]
	private GameObject cloudSavesPanel;

	// Token: 0x04004167 RID: 16743
	[SerializeField]
	private GameObject defaultToCloudSaveToggle;

	// Token: 0x04004168 RID: 16744
	[SerializeField]
	private GameObject savePanel;

	// Token: 0x04004169 RID: 16745
	[SerializeField]
	private InputBindingsScreen inputBindingsScreenPrefab;

	// Token: 0x0400416A RID: 16746
	[SerializeField]
	private KSlider cameraSpeedSlider;

	// Token: 0x0400416B RID: 16747
	[SerializeField]
	private LocText cameraSpeedSliderLabel;

	// Token: 0x0400416C RID: 16748
	private const int cameraSliderNotchScale = 10;

	// Token: 0x0400416D RID: 16749
	public const string PREFS_KEY_CAMERA_SPEED = "CameraSpeed";
}
