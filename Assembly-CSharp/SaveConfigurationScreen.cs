using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000D2E RID: 3374
[Serializable]
public class SaveConfigurationScreen
{
	// Token: 0x06006A10 RID: 27152 RVA: 0x0027F7BC File Offset: 0x0027D9BC
	public void ToggleDisabledContent(bool enable)
	{
		if (enable)
		{
			this.disabledContentPanel.SetActive(true);
			this.disabledContentWarning.SetActive(false);
			this.perSaveWarning.SetActive(true);
			return;
		}
		this.disabledContentPanel.SetActive(false);
		this.disabledContentWarning.SetActive(true);
		this.perSaveWarning.SetActive(false);
	}

	// Token: 0x06006A11 RID: 27153 RVA: 0x0027F818 File Offset: 0x0027DA18
	public void Init()
	{
		this.autosaveFrequencySlider.minValue = 0f;
		this.autosaveFrequencySlider.maxValue = (float)(this.sliderValueToCycleCount.Length - 1);
		this.autosaveFrequencySlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnAutosaveValueChanged(Mathf.FloorToInt(val));
		});
		this.autosaveFrequencySlider.value = (float)this.CycleCountToSlider(SaveGame.Instance.AutoSaveCycleInterval);
		this.timelapseResolutionSlider.minValue = 0f;
		this.timelapseResolutionSlider.maxValue = (float)(this.sliderValueToResolution.Length - 1);
		this.timelapseResolutionSlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnTimelapseValueChanged(Mathf.FloorToInt(val));
		});
		this.timelapseResolutionSlider.value = (float)this.ResolutionToSliderValue(SaveGame.Instance.TimelapseResolution);
		this.OnTimelapseValueChanged(Mathf.FloorToInt(this.timelapseResolutionSlider.value));
	}

	// Token: 0x06006A12 RID: 27154 RVA: 0x0027F8F8 File Offset: 0x0027DAF8
	public void Show(bool show)
	{
		if (show)
		{
			this.autosaveFrequencySlider.value = (float)this.CycleCountToSlider(SaveGame.Instance.AutoSaveCycleInterval);
			this.timelapseResolutionSlider.value = (float)this.ResolutionToSliderValue(SaveGame.Instance.TimelapseResolution);
			this.OnAutosaveValueChanged(Mathf.FloorToInt(this.autosaveFrequencySlider.value));
			this.OnTimelapseValueChanged(Mathf.FloorToInt(this.timelapseResolutionSlider.value));
		}
	}

	// Token: 0x06006A13 RID: 27155 RVA: 0x0027F96C File Offset: 0x0027DB6C
	private void OnTimelapseValueChanged(int sliderValue)
	{
		Vector2I vector2I = this.SliderValueToResolution(sliderValue);
		if (vector2I.x <= 0)
		{
			this.timelapseDescriptionLabel.SetText(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_DISABLED_DESCRIPTION);
		}
		else
		{
			this.timelapseDescriptionLabel.SetText(string.Format(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_RESOLUTION_DESCRIPTION, vector2I.x, vector2I.y));
		}
		SaveGame.Instance.TimelapseResolution = vector2I;
		Game.Instance.Trigger(75424175, null);
	}

	// Token: 0x06006A14 RID: 27156 RVA: 0x0027F9EC File Offset: 0x0027DBEC
	private void OnAutosaveValueChanged(int sliderValue)
	{
		int num = this.SliderValueToCycleCount(sliderValue);
		if (sliderValue == 0)
		{
			this.autosaveDescriptionLabel.SetText(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_NEVER);
		}
		else
		{
			this.autosaveDescriptionLabel.SetText(string.Format(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_FREQUENCY_DESCRIPTION, num));
		}
		SaveGame.Instance.AutoSaveCycleInterval = num;
	}

	// Token: 0x06006A15 RID: 27157 RVA: 0x0027FA46 File Offset: 0x0027DC46
	private int SliderValueToCycleCount(int sliderValue)
	{
		return this.sliderValueToCycleCount[sliderValue];
	}

	// Token: 0x06006A16 RID: 27158 RVA: 0x0027FA50 File Offset: 0x0027DC50
	private int CycleCountToSlider(int count)
	{
		for (int i = 0; i < this.sliderValueToCycleCount.Length; i++)
		{
			if (this.sliderValueToCycleCount[i] == count)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x06006A17 RID: 27159 RVA: 0x0027FA7E File Offset: 0x0027DC7E
	private Vector2I SliderValueToResolution(int sliderValue)
	{
		return this.sliderValueToResolution[sliderValue];
	}

	// Token: 0x06006A18 RID: 27160 RVA: 0x0027FA8C File Offset: 0x0027DC8C
	private int ResolutionToSliderValue(Vector2I resolution)
	{
		for (int i = 0; i < this.sliderValueToResolution.Length; i++)
		{
			if (this.sliderValueToResolution[i] == resolution)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x04004843 RID: 18499
	[SerializeField]
	private KSlider autosaveFrequencySlider;

	// Token: 0x04004844 RID: 18500
	[SerializeField]
	private LocText timelapseDescriptionLabel;

	// Token: 0x04004845 RID: 18501
	[SerializeField]
	private KSlider timelapseResolutionSlider;

	// Token: 0x04004846 RID: 18502
	[SerializeField]
	private LocText autosaveDescriptionLabel;

	// Token: 0x04004847 RID: 18503
	private int[] sliderValueToCycleCount = new int[]
	{
		-1,
		50,
		20,
		10,
		5,
		2,
		1
	};

	// Token: 0x04004848 RID: 18504
	private Vector2I[] sliderValueToResolution = new Vector2I[]
	{
		new Vector2I(-1, -1),
		new Vector2I(256, 384),
		new Vector2I(512, 768),
		new Vector2I(1024, 1536),
		new Vector2I(2048, 3072),
		new Vector2I(4096, 6144),
		new Vector2I(8192, 12288)
	};

	// Token: 0x04004849 RID: 18505
	[SerializeField]
	private GameObject disabledContentPanel;

	// Token: 0x0400484A RID: 18506
	[SerializeField]
	private GameObject disabledContentWarning;

	// Token: 0x0400484B RID: 18507
	[SerializeField]
	private GameObject perSaveWarning;
}
