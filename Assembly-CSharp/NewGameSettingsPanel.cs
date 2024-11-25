using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02000CF0 RID: 3312
[AddComponentMenu("KMonoBehaviour/scripts/NewGameSettingsPanel")]
public class NewGameSettingsPanel : CustomGameSettingsPanelBase
{
	// Token: 0x060066B6 RID: 26294 RVA: 0x0026609D File Offset: 0x0026429D
	public void SetCloseAction(System.Action onClose)
	{
		if (this.closeButton != null)
		{
			this.closeButton.onClick += onClose;
		}
		if (this.background != null)
		{
			this.background.onClick += onClose;
		}
	}

	// Token: 0x060066B7 RID: 26295 RVA: 0x002660D4 File Offset: 0x002642D4
	public override void Init()
	{
		CustomGameSettings.Instance.LoadClusters();
		Global.Instance.modManager.Report(base.gameObject);
		this.settings = CustomGameSettings.Instance;
		this.widgets = new List<CustomGameSettingWidget>();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.settings.QualitySettings)
		{
			if (keyValuePair.Value.ShowInUI())
			{
				ListSettingConfig listSettingConfig = keyValuePair.Value as ListSettingConfig;
				if (listSettingConfig != null)
				{
					CustomGameSettingListWidget customGameSettingListWidget = Util.KInstantiateUI<CustomGameSettingListWidget>(this.prefab_cycle_setting, this.content.gameObject, false);
					customGameSettingListWidget.Initialize(listSettingConfig, new Func<SettingConfig, SettingLevel>(CustomGameSettings.Instance.GetCurrentQualitySetting), new Func<ListSettingConfig, int, SettingLevel>(CustomGameSettings.Instance.CycleQualitySettingLevel));
					customGameSettingListWidget.gameObject.SetActive(true);
					base.AddWidget(customGameSettingListWidget);
				}
				else
				{
					ToggleSettingConfig toggleSettingConfig = keyValuePair.Value as ToggleSettingConfig;
					if (toggleSettingConfig != null)
					{
						CustomGameSettingToggleWidget customGameSettingToggleWidget = Util.KInstantiateUI<CustomGameSettingToggleWidget>(this.prefab_checkbox_setting, this.content.gameObject, false);
						customGameSettingToggleWidget.Initialize(toggleSettingConfig, new Func<SettingConfig, SettingLevel>(CustomGameSettings.Instance.GetCurrentQualitySetting), new Func<ToggleSettingConfig, SettingLevel>(CustomGameSettings.Instance.ToggleQualitySettingLevel));
						customGameSettingToggleWidget.gameObject.SetActive(true);
						base.AddWidget(customGameSettingToggleWidget);
					}
					else
					{
						SeedSettingConfig seedSettingConfig = keyValuePair.Value as SeedSettingConfig;
						if (seedSettingConfig != null)
						{
							CustomGameSettingSeed customGameSettingSeed = Util.KInstantiateUI<CustomGameSettingSeed>(this.prefab_seed_input_setting, this.content.gameObject, false);
							customGameSettingSeed.Initialize(seedSettingConfig);
							customGameSettingSeed.gameObject.SetActive(true);
							base.AddWidget(customGameSettingSeed);
						}
					}
				}
			}
		}
		this.Refresh();
	}

	// Token: 0x060066B8 RID: 26296 RVA: 0x002662A0 File Offset: 0x002644A0
	public void ConsumeSettingsCode(string code)
	{
		this.settings.ParseAndApplySettingsCode(code);
	}

	// Token: 0x060066B9 RID: 26297 RVA: 0x002662AE File Offset: 0x002644AE
	public void ConsumeStoryTraitsCode(string code)
	{
		this.settings.ParseAndApplyStoryTraitSettingsCode(code);
	}

	// Token: 0x060066BA RID: 26298 RVA: 0x002662BC File Offset: 0x002644BC
	public void ConsumeMixingSettingsCode(string code)
	{
		this.settings.ParseAndApplyMixingSettingsCode(code);
	}

	// Token: 0x060066BB RID: 26299 RVA: 0x002662CA File Offset: 0x002644CA
	public void SetSetting(SettingConfig setting, string level, bool notify = true)
	{
		this.settings.SetQualitySetting(setting, level, notify);
	}

	// Token: 0x060066BC RID: 26300 RVA: 0x002662DA File Offset: 0x002644DA
	public string GetSetting(SettingConfig setting)
	{
		return this.settings.GetCurrentQualitySetting(setting).id;
	}

	// Token: 0x060066BD RID: 26301 RVA: 0x002662ED File Offset: 0x002644ED
	public string GetSetting(string setting)
	{
		return this.settings.GetCurrentQualitySetting(setting).id;
	}

	// Token: 0x060066BE RID: 26302 RVA: 0x00266300 File Offset: 0x00264500
	public void Cancel()
	{
	}

	// Token: 0x04004541 RID: 17729
	[SerializeField]
	private Transform content;

	// Token: 0x04004542 RID: 17730
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004543 RID: 17731
	[SerializeField]
	private KButton background;

	// Token: 0x04004544 RID: 17732
	[Header("Prefab UI Refs")]
	[SerializeField]
	private GameObject prefab_cycle_setting;

	// Token: 0x04004545 RID: 17733
	[SerializeField]
	private GameObject prefab_slider_setting;

	// Token: 0x04004546 RID: 17734
	[SerializeField]
	private GameObject prefab_checkbox_setting;

	// Token: 0x04004547 RID: 17735
	[SerializeField]
	private GameObject prefab_seed_input_setting;

	// Token: 0x04004548 RID: 17736
	private CustomGameSettings settings;
}
