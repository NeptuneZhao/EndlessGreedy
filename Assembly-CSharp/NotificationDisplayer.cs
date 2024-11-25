using System;
using System.Collections.Generic;

// Token: 0x02000CF8 RID: 3320
public abstract class NotificationDisplayer : KMonoBehaviour
{
	// Token: 0x060066FD RID: 26365 RVA: 0x002678E3 File Offset: 0x00265AE3
	protected override void OnSpawn()
	{
		this.displayedNotifications = new List<Notification>();
		NotificationManager.Instance.notificationAdded += this.NotificationAdded;
		NotificationManager.Instance.notificationRemoved += this.NotificationRemoved;
	}

	// Token: 0x060066FE RID: 26366 RVA: 0x0026791C File Offset: 0x00265B1C
	public void NotificationAdded(Notification notification)
	{
		if (this.ShouldDisplayNotification(notification))
		{
			this.displayedNotifications.Add(notification);
			this.OnNotificationAdded(notification);
		}
	}

	// Token: 0x060066FF RID: 26367
	protected abstract void OnNotificationAdded(Notification notification);

	// Token: 0x06006700 RID: 26368 RVA: 0x0026793A File Offset: 0x00265B3A
	public void NotificationRemoved(Notification notification)
	{
		if (this.displayedNotifications.Contains(notification))
		{
			this.displayedNotifications.Remove(notification);
			this.OnNotificationRemoved(notification);
		}
	}

	// Token: 0x06006701 RID: 26369
	protected abstract void OnNotificationRemoved(Notification notification);

	// Token: 0x06006702 RID: 26370
	protected abstract bool ShouldDisplayNotification(Notification notification);

	// Token: 0x04004585 RID: 17797
	protected List<Notification> displayedNotifications;
}
