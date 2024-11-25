using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000597 RID: 1431
public class ManagementMenuNotification : Notification
{
	// Token: 0x17000168 RID: 360
	// (get) Token: 0x060021A3 RID: 8611 RVA: 0x000BBE1D File Offset: 0x000BA01D
	// (set) Token: 0x060021A4 RID: 8612 RVA: 0x000BBE25 File Offset: 0x000BA025
	public bool hasBeenViewed { get; private set; }

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x060021A5 RID: 8613 RVA: 0x000BBE2E File Offset: 0x000BA02E
	// (set) Token: 0x060021A6 RID: 8614 RVA: 0x000BBE36 File Offset: 0x000BA036
	public string highlightTarget { get; set; }

	// Token: 0x060021A7 RID: 8615 RVA: 0x000BBE40 File Offset: 0x000BA040
	public ManagementMenuNotification(global::Action targetMenu, NotificationValence valence, string highlightTarget, string title, NotificationType type, Func<List<Notification>, object, string> tooltip = null, object tooltip_data = null, bool expires = true, float delay = 0f, Notification.ClickCallback custom_click_callback = null, object custom_click_data = null, Transform click_focus = null, bool volume_attenuation = true) : base(title, type, tooltip, tooltip_data, expires, delay, custom_click_callback, custom_click_data, click_focus, volume_attenuation, false, false)
	{
		this.targetMenu = targetMenu;
		this.valence = valence;
		this.highlightTarget = highlightTarget;
	}

	// Token: 0x060021A8 RID: 8616 RVA: 0x000BBE7E File Offset: 0x000BA07E
	public void View()
	{
		this.hasBeenViewed = true;
		ManagementMenu.Instance.notificationDisplayer.NotificationWasViewed(this);
	}

	// Token: 0x040012EA RID: 4842
	public global::Action targetMenu;

	// Token: 0x040012EB RID: 4843
	public NotificationValence valence;
}
