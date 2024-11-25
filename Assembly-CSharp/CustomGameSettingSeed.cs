using System;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000C23 RID: 3107
public class CustomGameSettingSeed : CustomGameSettingWidget
{
	// Token: 0x06005F40 RID: 24384 RVA: 0x0023637C File Offset: 0x0023457C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Input.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.Input.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		this.RandomizeButton.onClick += this.GetNewRandomSeed;
	}

	// Token: 0x06005F41 RID: 24385 RVA: 0x002363DE File Offset: 0x002345DE
	public void Initialize(SeedSettingConfig config)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.GetNewRandomSeed();
	}

	// Token: 0x06005F42 RID: 24386 RVA: 0x00236410 File Offset: 0x00234610
	public override void Refresh()
	{
		base.Refresh();
		string currentQualitySettingLevelId = CustomGameSettings.Instance.GetCurrentQualitySettingLevelId(this.config);
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		this.allowChange = (currentClusterLayout.fixedCoordinate == -1);
		this.Input.interactable = this.allowChange;
		this.RandomizeButton.isInteractable = this.allowChange;
		if (this.allowChange)
		{
			this.InputToolTip.enabled = false;
			this.RandomizeButtonToolTip.enabled = false;
		}
		else
		{
			this.InputToolTip.enabled = true;
			this.RandomizeButtonToolTip.enabled = true;
			this.InputToolTip.SetSimpleTooltip(UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
			this.RandomizeButtonToolTip.SetSimpleTooltip(UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
		}
		this.Input.text = currentQualitySettingLevelId;
	}

	// Token: 0x06005F43 RID: 24387 RVA: 0x002364E0 File Offset: 0x002346E0
	private char ValidateInput(string text, int charIndex, char addedChar)
	{
		if ('0' > addedChar || addedChar > '9')
		{
			return '\0';
		}
		return addedChar;
	}

	// Token: 0x06005F44 RID: 24388 RVA: 0x002364F0 File Offset: 0x002346F0
	private void OnEndEdit(string text)
	{
		int seed;
		try
		{
			seed = Convert.ToInt32(text);
		}
		catch
		{
			seed = 0;
		}
		this.SetSeed(seed);
	}

	// Token: 0x06005F45 RID: 24389 RVA: 0x00236524 File Offset: 0x00234724
	public void SetSeed(int seed)
	{
		seed = Mathf.Min(seed, int.MaxValue);
		CustomGameSettings.Instance.SetQualitySetting(this.config, seed.ToString());
		this.Refresh();
	}

	// Token: 0x06005F46 RID: 24390 RVA: 0x00236550 File Offset: 0x00234750
	private void OnValueChanged(string text)
	{
		int num = 0;
		try
		{
			num = Convert.ToInt32(text);
		}
		catch
		{
			if (text.Length > 0)
			{
				this.Input.text = text.Substring(0, text.Length - 1);
			}
			else
			{
				this.Input.text = "";
			}
		}
		if (num > 2147483647)
		{
			this.Input.text = text.Substring(0, text.Length - 1);
		}
	}

	// Token: 0x06005F47 RID: 24391 RVA: 0x002365D4 File Offset: 0x002347D4
	private void GetNewRandomSeed()
	{
		int seed = UnityEngine.Random.Range(0, int.MaxValue);
		this.SetSeed(seed);
	}

	// Token: 0x04004019 RID: 16409
	[SerializeField]
	private LocText Label;

	// Token: 0x0400401A RID: 16410
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x0400401B RID: 16411
	[SerializeField]
	private KInputTextField Input;

	// Token: 0x0400401C RID: 16412
	[SerializeField]
	private KButton RandomizeButton;

	// Token: 0x0400401D RID: 16413
	[SerializeField]
	private ToolTip InputToolTip;

	// Token: 0x0400401E RID: 16414
	[SerializeField]
	private ToolTip RandomizeButtonToolTip;

	// Token: 0x0400401F RID: 16415
	private const int MAX_VALID_SEED = 2147483647;

	// Token: 0x04004020 RID: 16416
	private SeedSettingConfig config;

	// Token: 0x04004021 RID: 16417
	private bool allowChange = true;
}
