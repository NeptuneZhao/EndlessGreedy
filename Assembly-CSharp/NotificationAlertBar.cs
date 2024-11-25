using System;
using System.Collections.Generic;

// Token: 0x02000CF7 RID: 3319
public class NotificationAlertBar : KMonoBehaviour
{
	// Token: 0x060066F9 RID: 26361 RVA: 0x0026780C File Offset: 0x00265A0C
	public void Init(ManagementMenuNotification notification)
	{
		this.notification = notification;
		this.thisButton.onClick += this.OnThisButtonClicked;
		this.background.colorStyleSetting = this.alertColorStyle[(int)notification.valence];
		this.background.ApplyColorStyleSetting();
		this.text.text = notification.titleText;
		this.tooltip.SetSimpleTooltip(notification.ToolTip(null, notification.tooltipData));
		this.muteButton.onClick += this.OnMuteButtonClicked;
	}

	// Token: 0x060066FA RID: 26362 RVA: 0x002678A4 File Offset: 0x00265AA4
	private void OnThisButtonClicked()
	{
		NotificationHighlightController componentInParent = base.GetComponentInParent<NotificationHighlightController>();
		if (componentInParent != null)
		{
			componentInParent.SetActiveTarget(this.notification);
			return;
		}
		this.notification.View();
	}

	// Token: 0x060066FB RID: 26363 RVA: 0x002678D9 File Offset: 0x00265AD9
	private void OnMuteButtonClicked()
	{
	}

	// Token: 0x0400457E RID: 17790
	public ManagementMenuNotification notification;

	// Token: 0x0400457F RID: 17791
	public KButton thisButton;

	// Token: 0x04004580 RID: 17792
	public KImage background;

	// Token: 0x04004581 RID: 17793
	public LocText text;

	// Token: 0x04004582 RID: 17794
	public ToolTip tooltip;

	// Token: 0x04004583 RID: 17795
	public KButton muteButton;

	// Token: 0x04004584 RID: 17796
	public List<ColorStyleSetting> alertColorStyle;
}
