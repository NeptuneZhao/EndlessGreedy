using System;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02000C22 RID: 3106
public class CustomGameSettingListWidget : CustomGameSettingWidget
{
	// Token: 0x06005F3A RID: 24378 RVA: 0x00236247 File Offset: 0x00234447
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.CycleLeft.onClick += this.DoCycleLeft;
		this.CycleRight.onClick += this.DoCycleRight;
	}

	// Token: 0x06005F3B RID: 24379 RVA: 0x0023627D File Offset: 0x0023447D
	public void Initialize(ListSettingConfig config, Func<SettingConfig, SettingLevel> getCallback, Func<ListSettingConfig, int, SettingLevel> cycleCallback)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.getCallback = getCallback;
		this.cycleCallback = cycleCallback;
	}

	// Token: 0x06005F3C RID: 24380 RVA: 0x002362B8 File Offset: 0x002344B8
	public override void Refresh()
	{
		base.Refresh();
		SettingLevel settingLevel = this.getCallback(this.config);
		this.ValueLabel.text = settingLevel.label;
		this.ValueToolTip.toolTip = settingLevel.tooltip;
		this.CycleLeft.isInteractable = !this.config.IsFirstLevel(settingLevel.id);
		this.CycleRight.isInteractable = !this.config.IsLastLevel(settingLevel.id);
	}

	// Token: 0x06005F3D RID: 24381 RVA: 0x0023633D File Offset: 0x0023453D
	private void DoCycleLeft()
	{
		this.cycleCallback(this.config, -1);
		base.Notify();
	}

	// Token: 0x06005F3E RID: 24382 RVA: 0x00236358 File Offset: 0x00234558
	private void DoCycleRight()
	{
		this.cycleCallback(this.config, 1);
		base.Notify();
	}

	// Token: 0x04004010 RID: 16400
	[SerializeField]
	private LocText Label;

	// Token: 0x04004011 RID: 16401
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x04004012 RID: 16402
	[SerializeField]
	private LocText ValueLabel;

	// Token: 0x04004013 RID: 16403
	[SerializeField]
	private ToolTip ValueToolTip;

	// Token: 0x04004014 RID: 16404
	[SerializeField]
	private KButton CycleLeft;

	// Token: 0x04004015 RID: 16405
	[SerializeField]
	private KButton CycleRight;

	// Token: 0x04004016 RID: 16406
	private ListSettingConfig config;

	// Token: 0x04004017 RID: 16407
	protected Func<ListSettingConfig, int, SettingLevel> cycleCallback;

	// Token: 0x04004018 RID: 16408
	protected Func<SettingConfig, SettingLevel> getCallback;
}
