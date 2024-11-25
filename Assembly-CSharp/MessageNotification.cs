using System;
using System.Collections.Generic;

// Token: 0x02000588 RID: 1416
public class MessageNotification : Notification
{
	// Token: 0x060020F1 RID: 8433 RVA: 0x000B874D File Offset: 0x000B694D
	private string OnToolTip(List<Notification> notifications, string tooltipText)
	{
		return tooltipText;
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x000B8750 File Offset: 0x000B6950
	public MessageNotification(Message m) : base(m.GetTitle(), NotificationType.Messages, null, null, false, 0f, null, null, null, true, false, true)
	{
		MessageNotification <>4__this = this;
		this.message = m;
		base.Type = m.GetMessageType();
		this.showDismissButton = m.ShowDismissButton();
		if (!this.message.PlayNotificationSound())
		{
			this.playSound = false;
		}
		base.ToolTip = ((List<Notification> notifications, object data) => <>4__this.OnToolTip(notifications, m.GetTooltip()));
		base.clickFocus = null;
	}

	// Token: 0x0400126F RID: 4719
	public Message message;
}
