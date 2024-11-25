using System;
using System.Collections.Generic;

// Token: 0x02000CFB RID: 3323
public class NotificationManager : KMonoBehaviour
{
	// Token: 0x1700075E RID: 1886
	// (get) Token: 0x06006713 RID: 26387 RVA: 0x00267D8F File Offset: 0x00265F8F
	// (set) Token: 0x06006714 RID: 26388 RVA: 0x00267D96 File Offset: 0x00265F96
	public static NotificationManager Instance { get; private set; }

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06006715 RID: 26389 RVA: 0x00267DA0 File Offset: 0x00265FA0
	// (remove) Token: 0x06006716 RID: 26390 RVA: 0x00267DD8 File Offset: 0x00265FD8
	public event Action<Notification> notificationAdded;

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x06006717 RID: 26391 RVA: 0x00267E10 File Offset: 0x00266010
	// (remove) Token: 0x06006718 RID: 26392 RVA: 0x00267E48 File Offset: 0x00266048
	public event Action<Notification> notificationRemoved;

	// Token: 0x06006719 RID: 26393 RVA: 0x00267E7D File Offset: 0x0026607D
	protected override void OnPrefabInit()
	{
		Debug.Assert(NotificationManager.Instance == null);
		NotificationManager.Instance = this;
	}

	// Token: 0x0600671A RID: 26394 RVA: 0x00267E95 File Offset: 0x00266095
	protected override void OnForcedCleanUp()
	{
		NotificationManager.Instance = null;
	}

	// Token: 0x0600671B RID: 26395 RVA: 0x00267E9D File Offset: 0x0026609D
	public void AddNotification(Notification notification)
	{
		this.pendingNotifications.Add(notification);
		if (NotificationScreen.Instance != null)
		{
			NotificationScreen.Instance.AddPendingNotification(notification);
		}
	}

	// Token: 0x0600671C RID: 26396 RVA: 0x00267EC4 File Offset: 0x002660C4
	public void RemoveNotification(Notification notification)
	{
		this.pendingNotifications.Remove(notification);
		if (NotificationScreen.Instance != null)
		{
			NotificationScreen.Instance.RemovePendingNotification(notification);
		}
		if (this.notifications.Remove(notification))
		{
			this.notificationRemoved(notification);
		}
	}

	// Token: 0x0600671D RID: 26397 RVA: 0x00267F10 File Offset: 0x00266110
	private void Update()
	{
		int i = 0;
		while (i < this.pendingNotifications.Count)
		{
			if (this.pendingNotifications[i].IsReady())
			{
				this.DoAddNotification(this.pendingNotifications[i]);
				this.pendingNotifications.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600671E RID: 26398 RVA: 0x00267F66 File Offset: 0x00266166
	private void DoAddNotification(Notification notification)
	{
		this.notifications.Add(notification);
		if (this.notificationAdded != null)
		{
			this.notificationAdded(notification);
		}
	}

	// Token: 0x0400458F RID: 17807
	private List<Notification> pendingNotifications = new List<Notification>();

	// Token: 0x04004590 RID: 17808
	private List<Notification> notifications = new List<Notification>();
}
