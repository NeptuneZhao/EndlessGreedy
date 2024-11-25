using System;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02000C24 RID: 3108
public class CustomGameSettingToggleWidget : CustomGameSettingWidget
{
	// Token: 0x06005F49 RID: 24393 RVA: 0x00236603 File Offset: 0x00234803
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle toggle = this.Toggle;
		toggle.onClick = (System.Action)Delegate.Combine(toggle.onClick, new System.Action(this.ToggleSetting));
	}

	// Token: 0x06005F4A RID: 24394 RVA: 0x00236632 File Offset: 0x00234832
	public void Initialize(ToggleSettingConfig config, Func<SettingConfig, SettingLevel> getCurrentSettingCallback, Func<ToggleSettingConfig, SettingLevel> toggleCallback)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.getCurrentSettingCallback = getCurrentSettingCallback;
		this.toggleCallback = toggleCallback;
	}

	// Token: 0x06005F4B RID: 24395 RVA: 0x0023666C File Offset: 0x0023486C
	public override void Refresh()
	{
		base.Refresh();
		SettingLevel settingLevel = this.getCurrentSettingCallback(this.config);
		this.Toggle.ChangeState(this.config.IsOnLevel(settingLevel.id) ? 1 : 0);
		this.ToggleToolTip.toolTip = settingLevel.tooltip;
	}

	// Token: 0x06005F4C RID: 24396 RVA: 0x002366C4 File Offset: 0x002348C4
	public void ToggleSetting()
	{
		this.toggleCallback(this.config);
		base.Notify();
	}

	// Token: 0x04004022 RID: 16418
	[SerializeField]
	private LocText Label;

	// Token: 0x04004023 RID: 16419
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x04004024 RID: 16420
	[SerializeField]
	private MultiToggle Toggle;

	// Token: 0x04004025 RID: 16421
	[SerializeField]
	private ToolTip ToggleToolTip;

	// Token: 0x04004026 RID: 16422
	private ToggleSettingConfig config;

	// Token: 0x04004027 RID: 16423
	protected Func<SettingConfig, SettingLevel> getCurrentSettingCallback;

	// Token: 0x04004028 RID: 16424
	protected Func<ToggleSettingConfig, SettingLevel> toggleCallback;
}
