using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CF6 RID: 3318
public class ManagementScreenNotificationOverlay : KMonoBehaviour
{
	// Token: 0x060066F4 RID: 26356 RVA: 0x002677C0 File Offset: 0x002659C0
	protected void OnEnable()
	{
	}

	// Token: 0x060066F5 RID: 26357 RVA: 0x002677C2 File Offset: 0x002659C2
	protected override void OnDisable()
	{
	}

	// Token: 0x060066F6 RID: 26358 RVA: 0x002677C4 File Offset: 0x002659C4
	private NotificationAlertBar CreateAlertBar(ManagementMenuNotification notification)
	{
		NotificationAlertBar notificationAlertBar = Util.KInstantiateUI<NotificationAlertBar>(this.alertBarPrefab.gameObject, this.alertContainer.gameObject, false);
		notificationAlertBar.Init(notification);
		notificationAlertBar.gameObject.SetActive(true);
		return notificationAlertBar;
	}

	// Token: 0x060066F7 RID: 26359 RVA: 0x002677F5 File Offset: 0x002659F5
	private void NotificationsChanged()
	{
	}

	// Token: 0x0400457A RID: 17786
	public global::Action currentMenu;

	// Token: 0x0400457B RID: 17787
	public NotificationAlertBar alertBarPrefab;

	// Token: 0x0400457C RID: 17788
	public RectTransform alertContainer;

	// Token: 0x0400457D RID: 17789
	private List<NotificationAlertBar> alertBars = new List<NotificationAlertBar>();
}
