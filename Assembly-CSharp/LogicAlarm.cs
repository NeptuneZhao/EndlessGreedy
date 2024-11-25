using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006FF RID: 1791
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicAlarm")]
public class LogicAlarm : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06002DE0 RID: 11744 RVA: 0x00101C48 File Offset: 0x000FFE48
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicAlarm>(-905833192, LogicAlarm.OnCopySettingsDelegate);
	}

	// Token: 0x06002DE1 RID: 11745 RVA: 0x00101C64 File Offset: 0x000FFE64
	private void OnCopySettings(object data)
	{
		LogicAlarm component = ((GameObject)data).GetComponent<LogicAlarm>();
		if (component != null)
		{
			this.notificationName = component.notificationName;
			this.notificationType = component.notificationType;
			this.pauseOnNotify = component.pauseOnNotify;
			this.zoomOnNotify = component.zoomOnNotify;
			this.cooldown = component.cooldown;
			this.notificationTooltip = component.notificationTooltip;
		}
	}

	// Token: 0x06002DE2 RID: 11746 RVA: 0x00101CD0 File Offset: 0x000FFED0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.notifier = base.gameObject.AddComponent<Notifier>();
		base.Subscribe<LogicAlarm>(-801688580, LogicAlarm.OnLogicValueChangedDelegate);
		if (string.IsNullOrEmpty(this.notificationName))
		{
			this.notificationName = UI.UISIDESCREENS.LOGICALARMSIDESCREEN.NAME_DEFAULT;
		}
		if (string.IsNullOrEmpty(this.notificationTooltip))
		{
			this.notificationTooltip = UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIP_DEFAULT;
		}
		this.UpdateVisualState();
		this.UpdateNotification(false);
	}

	// Token: 0x06002DE3 RID: 11747 RVA: 0x00101D4C File Offset: 0x000FFF4C
	private void UpdateVisualState()
	{
		base.GetComponent<KBatchedAnimController>().Play(this.wasOn ? LogicAlarm.ON_ANIMS : LogicAlarm.OFF_ANIMS, KAnim.PlayMode.Once);
	}

	// Token: 0x06002DE4 RID: 11748 RVA: 0x00101D70 File Offset: 0x000FFF70
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicAlarm.INPUT_PORT_ID)
		{
			return;
		}
		int newValue = logicValueChanged.newValue;
		if (LogicCircuitNetwork.IsBitActive(0, newValue))
		{
			if (!this.wasOn)
			{
				this.PushNotification();
				this.wasOn = true;
				if (this.pauseOnNotify && !SpeedControlScreen.Instance.IsPaused)
				{
					SpeedControlScreen.Instance.Pause(false, false);
				}
				if (this.zoomOnNotify)
				{
					CameraController.Instance.ActiveWorldStarWipe(base.gameObject.GetMyWorldId(), base.transform.GetPosition(), 8f, null);
				}
				this.UpdateVisualState();
				return;
			}
		}
		else if (this.wasOn)
		{
			this.wasOn = false;
			this.UpdateVisualState();
		}
	}

	// Token: 0x06002DE5 RID: 11749 RVA: 0x00101E26 File Offset: 0x00100026
	private void PushNotification()
	{
		this.notification.Clear();
		this.notifier.Add(this.notification, "");
	}

	// Token: 0x06002DE6 RID: 11750 RVA: 0x00101E4C File Offset: 0x0010004C
	public void UpdateNotification(bool clear)
	{
		if (this.notification != null && clear)
		{
			this.notification.Clear();
			this.lastNotificationCreated = null;
		}
		if (this.notification != this.lastNotificationCreated || this.lastNotificationCreated == null)
		{
			this.notification = this.CreateNotification();
		}
	}

	// Token: 0x06002DE7 RID: 11751 RVA: 0x00101E9C File Offset: 0x0010009C
	public Notification CreateNotification()
	{
		base.GetComponent<KSelectable>();
		Notification result = new Notification(this.notificationName, this.notificationType, (List<Notification> n, object d) => this.notificationTooltip, null, true, 0f, null, null, null, false, false, false);
		this.lastNotificationCreated = result;
		return result;
	}

	// Token: 0x04001AB8 RID: 6840
	[Serialize]
	public string notificationName;

	// Token: 0x04001AB9 RID: 6841
	[Serialize]
	public string notificationTooltip;

	// Token: 0x04001ABA RID: 6842
	[Serialize]
	public NotificationType notificationType;

	// Token: 0x04001ABB RID: 6843
	[Serialize]
	public bool pauseOnNotify;

	// Token: 0x04001ABC RID: 6844
	[Serialize]
	public bool zoomOnNotify;

	// Token: 0x04001ABD RID: 6845
	[Serialize]
	public float cooldown;

	// Token: 0x04001ABE RID: 6846
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001ABF RID: 6847
	private bool wasOn;

	// Token: 0x04001AC0 RID: 6848
	private Notifier notifier;

	// Token: 0x04001AC1 RID: 6849
	private Notification notification;

	// Token: 0x04001AC2 RID: 6850
	private Notification lastNotificationCreated;

	// Token: 0x04001AC3 RID: 6851
	private static readonly EventSystem.IntraObjectHandler<LogicAlarm> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicAlarm>(delegate(LogicAlarm component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001AC4 RID: 6852
	private static readonly EventSystem.IntraObjectHandler<LogicAlarm> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicAlarm>(delegate(LogicAlarm component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001AC5 RID: 6853
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicAlarmInput");

	// Token: 0x04001AC6 RID: 6854
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on_loop"
	};

	// Token: 0x04001AC7 RID: 6855
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};
}
