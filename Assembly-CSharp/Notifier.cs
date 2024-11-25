using System;
using UnityEngine;

// Token: 0x0200059A RID: 1434
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Notifier")]
public class Notifier : KMonoBehaviour
{
	// Token: 0x060021C3 RID: 8643 RVA: 0x000BC234 File Offset: 0x000BA434
	protected override void OnPrefabInit()
	{
		Components.Notifiers.Add(this);
	}

	// Token: 0x060021C4 RID: 8644 RVA: 0x000BC241 File Offset: 0x000BA441
	protected override void OnCleanUp()
	{
		Components.Notifiers.Remove(this);
	}

	// Token: 0x060021C5 RID: 8645 RVA: 0x000BC250 File Offset: 0x000BA450
	public void Add(Notification notification, string suffix = "")
	{
		if (KScreenManager.Instance == null)
		{
			return;
		}
		if (this.DisableNotifications)
		{
			return;
		}
		if (DebugHandler.NotificationsDisabled)
		{
			return;
		}
		DebugUtil.DevAssert(notification != null, "Trying to add null notification. It's safe to continue playing, the notification won't be displayed.", null);
		if (notification == null)
		{
			return;
		}
		if (notification.Notifier == null)
		{
			if (this.Selectable != null)
			{
				notification.NotifierName = "• " + this.Selectable.GetName() + suffix;
			}
			else
			{
				notification.NotifierName = "• " + base.name + suffix;
			}
			notification.Notifier = this;
			if (this.AutoClickFocus && notification.clickFocus == null)
			{
				notification.clickFocus = base.transform;
			}
			NotificationManager.Instance.AddNotification(notification);
			notification.GameTime = Time.time;
		}
		else
		{
			DebugUtil.Assert(notification.Notifier == this);
		}
		notification.Time = KTime.Instance.UnscaledGameTime;
	}

	// Token: 0x060021C6 RID: 8646 RVA: 0x000BC345 File Offset: 0x000BA545
	public void Remove(Notification notification)
	{
		if (notification == null)
		{
			return;
		}
		if (notification.Notifier != null)
		{
			notification.Notifier = null;
		}
		if (NotificationManager.Instance != null)
		{
			NotificationManager.Instance.RemoveNotification(notification);
		}
	}

	// Token: 0x04001301 RID: 4865
	[MyCmpGet]
	private KSelectable Selectable;

	// Token: 0x04001302 RID: 4866
	public bool DisableNotifications;

	// Token: 0x04001303 RID: 4867
	public bool AutoClickFocus = true;
}
