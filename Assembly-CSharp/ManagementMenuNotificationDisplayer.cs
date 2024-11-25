using System;
using System.Collections.Generic;

// Token: 0x02000CF5 RID: 3317
public class ManagementMenuNotificationDisplayer : NotificationDisplayer
{
	// Token: 0x1700075D RID: 1885
	// (get) Token: 0x060066E9 RID: 26345 RVA: 0x00267669 File Offset: 0x00265869
	// (set) Token: 0x060066EA RID: 26346 RVA: 0x00267671 File Offset: 0x00265871
	public List<ManagementMenuNotification> displayedManagementMenuNotifications { get; private set; }

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x060066EB RID: 26347 RVA: 0x0026767C File Offset: 0x0026587C
	// (remove) Token: 0x060066EC RID: 26348 RVA: 0x002676B4 File Offset: 0x002658B4
	public event System.Action onNotificationsChanged;

	// Token: 0x060066ED RID: 26349 RVA: 0x002676E9 File Offset: 0x002658E9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.displayedManagementMenuNotifications = new List<ManagementMenuNotification>();
	}

	// Token: 0x060066EE RID: 26350 RVA: 0x002676FC File Offset: 0x002658FC
	public void NotificationWasViewed(ManagementMenuNotification notification)
	{
		this.onNotificationsChanged();
	}

	// Token: 0x060066EF RID: 26351 RVA: 0x00267709 File Offset: 0x00265909
	protected override void OnNotificationAdded(Notification notification)
	{
		this.displayedManagementMenuNotifications.Add(notification as ManagementMenuNotification);
		this.onNotificationsChanged();
	}

	// Token: 0x060066F0 RID: 26352 RVA: 0x00267727 File Offset: 0x00265927
	protected override void OnNotificationRemoved(Notification notification)
	{
		this.displayedManagementMenuNotifications.Remove(notification as ManagementMenuNotification);
		this.onNotificationsChanged();
	}

	// Token: 0x060066F1 RID: 26353 RVA: 0x00267746 File Offset: 0x00265946
	protected override bool ShouldDisplayNotification(Notification notification)
	{
		return notification is ManagementMenuNotification;
	}

	// Token: 0x060066F2 RID: 26354 RVA: 0x00267754 File Offset: 0x00265954
	public List<ManagementMenuNotification> GetNotificationsForAction(global::Action hotKey)
	{
		List<ManagementMenuNotification> list = new List<ManagementMenuNotification>();
		foreach (ManagementMenuNotification managementMenuNotification in this.displayedManagementMenuNotifications)
		{
			if (managementMenuNotification.targetMenu == hotKey)
			{
				list.Add(managementMenuNotification);
			}
		}
		return list;
	}
}
