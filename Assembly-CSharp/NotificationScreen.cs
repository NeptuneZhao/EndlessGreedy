using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CF4 RID: 3316
public class NotificationScreen : KScreen
{
	// Token: 0x1700075C RID: 1884
	// (get) Token: 0x060066D1 RID: 26321 RVA: 0x002666B6 File Offset: 0x002648B6
	// (set) Token: 0x060066D2 RID: 26322 RVA: 0x002666BD File Offset: 0x002648BD
	public static NotificationScreen Instance { get; private set; }

	// Token: 0x060066D3 RID: 26323 RVA: 0x002666C5 File Offset: 0x002648C5
	public static void DestroyInstance()
	{
		NotificationScreen.Instance = null;
	}

	// Token: 0x060066D4 RID: 26324 RVA: 0x002666CD File Offset: 0x002648CD
	public void AddPendingNotification(Notification notification)
	{
		this.pendingNotifications.Add(notification);
	}

	// Token: 0x060066D5 RID: 26325 RVA: 0x002666DB File Offset: 0x002648DB
	public void RemovePendingNotification(Notification notification)
	{
		this.dirty = true;
		this.pendingNotifications.Remove(notification);
		this.RemoveNotification(notification);
	}

	// Token: 0x060066D6 RID: 26326 RVA: 0x002666F8 File Offset: 0x002648F8
	public void RemoveNotification(Notification notification)
	{
		NotificationScreen.Entry entry = null;
		this.entriesByMessage.TryGetValue(notification.titleText, out entry);
		if (entry == null)
		{
			return;
		}
		this.notifications.Remove(notification);
		entry.Remove(notification);
		if (entry.notifications.Count == 0)
		{
			UnityEngine.Object.Destroy(entry.label);
			this.entriesByMessage[notification.titleText] = null;
			this.entries.Remove(entry);
		}
	}

	// Token: 0x060066D7 RID: 26327 RVA: 0x0026676A File Offset: 0x0026496A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NotificationScreen.Instance = this;
		this.MessagesPrefab.gameObject.SetActive(false);
		this.LabelPrefab.gameObject.SetActive(false);
		this.InitNotificationSounds();
	}

	// Token: 0x060066D8 RID: 26328 RVA: 0x002667A0 File Offset: 0x002649A0
	private void OnNewMessage(object data)
	{
		Message m = (Message)data;
		this.notifier.Add(new MessageNotification(m), "");
	}

	// Token: 0x060066D9 RID: 26329 RVA: 0x002667CC File Offset: 0x002649CC
	private void ShowMessage(MessageNotification mn)
	{
		mn.message.OnClick();
		if (mn.message.ShowDialog())
		{
			for (int i = 0; i < this.dialogPrefabs.Count; i++)
			{
				if (this.dialogPrefabs[i].CanDisplay(mn.message))
				{
					if (this.messageDialog != null)
					{
						UnityEngine.Object.Destroy(this.messageDialog.gameObject);
						this.messageDialog = null;
					}
					this.messageDialog = global::Util.KInstantiateUI<MessageDialogFrame>(ScreenPrefabs.Instance.MessageDialogFrame.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
					MessageDialog dialog = global::Util.KInstantiateUI<MessageDialog>(this.dialogPrefabs[i].gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
					this.messageDialog.SetMessage(dialog, mn.message);
					this.messageDialog.Show(true);
					break;
				}
			}
		}
		Messenger.Instance.RemoveMessage(mn.message);
		mn.Clear();
	}

	// Token: 0x060066DA RID: 26330 RVA: 0x002668D8 File Offset: 0x00264AD8
	public void OnClickNextMessage()
	{
		Notification notification2 = this.notifications.Find((Notification notification) => notification.Type == NotificationType.Messages);
		this.ShowMessage((MessageNotification)notification2);
	}

	// Token: 0x060066DB RID: 26331 RVA: 0x0026691C File Offset: 0x00264B1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.initTime = KTime.Instance.UnscaledGameTime;
		LocText[] componentsInChildren = this.LabelPrefab.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = GlobalAssets.Instance.colorSet.NotificationNormal;
		}
		componentsInChildren = this.MessagesPrefab.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = GlobalAssets.Instance.colorSet.NotificationNormal;
		}
		base.Subscribe(Messenger.Instance.gameObject, 1558809273, new Action<object>(this.OnNewMessage));
		foreach (Message m in Messenger.Instance.Messages)
		{
			Notification notification = new MessageNotification(m);
			notification.playSound = false;
			this.notifier.Add(notification, "");
		}
	}

	// Token: 0x060066DC RID: 26332 RVA: 0x00266A28 File Offset: 0x00264C28
	protected override void OnActivate()
	{
		base.OnActivate();
		this.dirty = true;
	}

	// Token: 0x060066DD RID: 26333 RVA: 0x00266A38 File Offset: 0x00264C38
	public void AddNotification(Notification notification)
	{
		if (DebugHandler.NotificationsDisabled)
		{
			return;
		}
		this.notifications.Add(notification);
		NotificationScreen.Entry entry;
		this.entriesByMessage.TryGetValue(notification.titleText, out entry);
		if (entry == null)
		{
			HierarchyReferences hierarchyReferences;
			if (notification.Type == NotificationType.Messages)
			{
				hierarchyReferences = global::Util.KInstantiateUI<HierarchyReferences>(this.MessagesPrefab, this.MessagesFolder, false);
			}
			else
			{
				hierarchyReferences = global::Util.KInstantiateUI<HierarchyReferences>(this.LabelPrefab, this.LabelsFolder, false);
			}
			Button reference = hierarchyReferences.GetReference<Button>("DismissButton");
			reference.gameObject.SetActive(notification.showDismissButton);
			if (notification.showDismissButton)
			{
				reference.onClick.AddListener(delegate()
				{
					NotificationScreen.Entry entry;
					if (!this.entriesByMessage.TryGetValue(notification.titleText, out entry))
					{
						return;
					}
					for (int i = entry.notifications.Count - 1; i >= 0; i--)
					{
						Notification notification2 = entry.notifications[i];
						MessageNotification messageNotification2 = notification2 as MessageNotification;
						if (messageNotification2 != null)
						{
							Messenger.Instance.RemoveMessage(messageNotification2.message);
						}
						notification2.Clear();
					}
				});
			}
			hierarchyReferences.GetReference<NotificationAnimator>("Animator").Begin(true);
			hierarchyReferences.gameObject.SetActive(true);
			if (notification.ToolTip != null)
			{
				ToolTip tooltip = hierarchyReferences.GetReference<ToolTip>("ToolTip");
				tooltip.OnToolTip = delegate()
				{
					tooltip.ClearMultiStringTooltip();
					tooltip.AddMultiStringTooltip(notification.ToolTip(entry.notifications, notification.tooltipData), this.TooltipTextStyle);
					return "";
				};
			}
			KImage reference2 = hierarchyReferences.GetReference<KImage>("Icon");
			LocText reference3 = hierarchyReferences.GetReference<LocText>("Text");
			Button reference4 = hierarchyReferences.GetReference<Button>("MainButton");
			ColorBlock colors = reference4.colors;
			switch (notification.Type)
			{
			case NotificationType.Bad:
			case NotificationType.DuplicantThreatening:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationBadBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationBad;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationBad;
				reference2.sprite = ((notification.Type == NotificationType.Bad) ? this.icon_bad : this.icon_threatening);
				goto IL_43D;
			case NotificationType.Tutorial:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationTutorialBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationTutorial;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationTutorial;
				reference2.sprite = this.icon_warning;
				goto IL_43D;
			case NotificationType.Messages:
			{
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationMessageBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationMessage;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationMessage;
				reference2.sprite = this.icon_message;
				MessageNotification messageNotification = notification as MessageNotification;
				if (messageNotification == null)
				{
					goto IL_43D;
				}
				TutorialMessage tutorialMessage = messageNotification.message as TutorialMessage;
				if (tutorialMessage != null && !string.IsNullOrEmpty(tutorialMessage.videoClipId))
				{
					reference2.sprite = this.icon_video;
					goto IL_43D;
				}
				goto IL_43D;
			}
			case NotificationType.Event:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationEventBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationEvent;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationEvent;
				reference2.sprite = this.icon_event;
				goto IL_43D;
			case NotificationType.MessageImportant:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationMessageImportantBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationMessageImportant;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationMessageImportant;
				reference2.sprite = this.icon_message_important;
				goto IL_43D;
			}
			colors.normalColor = GlobalAssets.Instance.colorSet.NotificationNormalBG;
			reference3.color = GlobalAssets.Instance.colorSet.NotificationNormal;
			reference2.color = GlobalAssets.Instance.colorSet.NotificationNormal;
			reference2.sprite = this.icon_normal;
			IL_43D:
			reference4.colors = colors;
			reference4.onClick.AddListener(delegate()
			{
				this.OnClick(entry);
			});
			string str = "";
			if (KTime.Instance.UnscaledGameTime - this.initTime > 5f && notification.playSound)
			{
				this.PlayDingSound(notification, 0);
			}
			else
			{
				str = "too early";
			}
			if (AudioDebug.Get().debugNotificationSounds)
			{
				global::Debug.Log("Notification(" + notification.titleText + "):" + str);
			}
			entry = new NotificationScreen.Entry(hierarchyReferences.gameObject);
			this.entriesByMessage[notification.titleText] = entry;
			this.entries.Add(entry);
		}
		entry.Add(notification);
		this.dirty = true;
		this.SortNotifications();
	}

	// Token: 0x060066DE RID: 26334 RVA: 0x00266F70 File Offset: 0x00265170
	private void SortNotifications()
	{
		this.notifications.Sort(delegate(Notification n1, Notification n2)
		{
			if (n1.Type == n2.Type)
			{
				return n1.Idx - n2.Idx;
			}
			return n1.Type - n2.Type;
		});
		foreach (Notification notification in this.notifications)
		{
			NotificationScreen.Entry entry = null;
			this.entriesByMessage.TryGetValue(notification.titleText, out entry);
			if (entry != null)
			{
				entry.label.GetComponent<RectTransform>().SetAsLastSibling();
			}
		}
	}

	// Token: 0x060066DF RID: 26335 RVA: 0x00267010 File Offset: 0x00265210
	private void PlayDingSound(Notification notification, int count)
	{
		string text;
		if (!this.notificationSounds.TryGetValue(notification.Type, out text))
		{
			text = "Notification";
		}
		float num;
		if (!this.timeOfLastNotification.TryGetValue(text, out num))
		{
			num = 0f;
		}
		float value = notification.volume_attenuation ? ((Time.time - num) / this.soundDecayTime) : 1f;
		this.timeOfLastNotification[text] = Time.time;
		string sound;
		if (count > 1)
		{
			sound = GlobalAssets.GetSound(text + "_AddCount", true);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound(text, false);
			}
		}
		else
		{
			sound = GlobalAssets.GetSound(text, false);
		}
		if (notification.playSound)
		{
			EventInstance instance = KFMOD.BeginOneShot(sound, Vector3.zero, 1f);
			instance.setParameterByName("timeSinceLast", value, false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x060066E0 RID: 26336 RVA: 0x002670DC File Offset: 0x002652DC
	private void Update()
	{
		int i = 0;
		while (i < this.pendingNotifications.Count)
		{
			if (this.pendingNotifications[i].IsReady())
			{
				this.AddNotification(this.pendingNotifications[i]);
				this.pendingNotifications.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
		int num = 0;
		int num2 = 0;
		for (int j = 0; j < this.notifications.Count; j++)
		{
			Notification notification = this.notifications[j];
			if (notification.Type == NotificationType.Messages)
			{
				num2++;
			}
			else
			{
				num++;
			}
			if (notification.expires && KTime.Instance.UnscaledGameTime - notification.Time > this.lifetime)
			{
				this.dirty = true;
				if (notification.Notifier == null)
				{
					this.RemovePendingNotification(notification);
				}
				else
				{
					notification.Clear();
				}
			}
		}
	}

	// Token: 0x060066E1 RID: 26337 RVA: 0x002671B8 File Offset: 0x002653B8
	private void OnClick(NotificationScreen.Entry entry)
	{
		Notification nextClickedNotification = entry.NextClickedNotification;
		base.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Open", false));
		if (nextClickedNotification.customClickCallback != null)
		{
			nextClickedNotification.customClickCallback(nextClickedNotification.customClickData);
		}
		else
		{
			if (nextClickedNotification.clickFocus != null)
			{
				Vector3 position = nextClickedNotification.clickFocus.GetPosition();
				position.z = -40f;
				ClusterGridEntity component = nextClickedNotification.clickFocus.GetComponent<ClusterGridEntity>();
				KSelectable component2 = nextClickedNotification.clickFocus.GetComponent<KSelectable>();
				int myWorldId = nextClickedNotification.clickFocus.gameObject.GetMyWorldId();
				if (myWorldId != -1)
				{
					CameraController.Instance.ActiveWorldStarWipe(myWorldId, position, 10f, null);
				}
				else if (DlcManager.FeatureClusterSpaceEnabled() && component != null && component.IsVisible)
				{
					ManagementMenu.Instance.OpenClusterMap();
					ClusterMapScreen.Instance.SetTargetFocusPosition(component.Location, 0.5f);
				}
				if (component2 != null)
				{
					if (DlcManager.FeatureClusterSpaceEnabled() && component != null && component.IsVisible)
					{
						ClusterMapSelectTool.Instance.Select(component2, false);
					}
					else
					{
						SelectTool.Instance.Select(component2, false);
					}
				}
			}
			else if (nextClickedNotification.Notifier != null)
			{
				SelectTool.Instance.Select(nextClickedNotification.Notifier.GetComponent<KSelectable>(), false);
			}
			if (nextClickedNotification.Type == NotificationType.Messages)
			{
				this.ShowMessage((MessageNotification)nextClickedNotification);
			}
		}
		if (nextClickedNotification.clearOnClick)
		{
			nextClickedNotification.Clear();
		}
	}

	// Token: 0x060066E2 RID: 26338 RVA: 0x00267323 File Offset: 0x00265523
	private void PositionLocatorIcon()
	{
	}

	// Token: 0x060066E3 RID: 26339 RVA: 0x00267328 File Offset: 0x00265528
	private void InitNotificationSounds()
	{
		this.notificationSounds[NotificationType.Good] = "Notification";
		this.notificationSounds[NotificationType.BadMinor] = "Notification";
		this.notificationSounds[NotificationType.Bad] = "Warning";
		this.notificationSounds[NotificationType.Neutral] = "Notification";
		this.notificationSounds[NotificationType.Tutorial] = "Notification";
		this.notificationSounds[NotificationType.Messages] = "Message";
		this.notificationSounds[NotificationType.DuplicantThreatening] = "Warning_DupeThreatening";
		this.notificationSounds[NotificationType.Event] = "Message";
		this.notificationSounds[NotificationType.MessageImportant] = "Message_Important";
	}

	// Token: 0x060066E4 RID: 26340 RVA: 0x002673D0 File Offset: 0x002655D0
	public Sprite GetNotificationIcon(NotificationType type)
	{
		switch (type)
		{
		case NotificationType.Bad:
			return this.icon_bad;
		case NotificationType.Tutorial:
			return this.icon_warning;
		case NotificationType.Messages:
			return this.icon_message;
		case NotificationType.DuplicantThreatening:
			return this.icon_threatening;
		case NotificationType.Event:
			return this.icon_event;
		case NotificationType.MessageImportant:
			return this.icon_message_important;
		}
		return this.icon_normal;
	}

	// Token: 0x060066E5 RID: 26341 RVA: 0x0026743C File Offset: 0x0026563C
	public Color GetNotificationColour(NotificationType type)
	{
		switch (type)
		{
		case NotificationType.Bad:
			return GlobalAssets.Instance.colorSet.NotificationBad;
		case NotificationType.Tutorial:
			return GlobalAssets.Instance.colorSet.NotificationTutorial;
		case NotificationType.Messages:
			return GlobalAssets.Instance.colorSet.NotificationMessage;
		case NotificationType.DuplicantThreatening:
			return GlobalAssets.Instance.colorSet.NotificationBad;
		case NotificationType.Event:
			return GlobalAssets.Instance.colorSet.NotificationEvent;
		case NotificationType.MessageImportant:
			return GlobalAssets.Instance.colorSet.NotificationMessageImportant;
		}
		return GlobalAssets.Instance.colorSet.NotificationNormal;
	}

	// Token: 0x060066E6 RID: 26342 RVA: 0x0026750C File Offset: 0x0026570C
	public Color GetNotificationBGColour(NotificationType type)
	{
		switch (type)
		{
		case NotificationType.Bad:
			return GlobalAssets.Instance.colorSet.NotificationBadBG;
		case NotificationType.Tutorial:
			return GlobalAssets.Instance.colorSet.NotificationTutorialBG;
		case NotificationType.Messages:
			return GlobalAssets.Instance.colorSet.NotificationMessageBG;
		case NotificationType.DuplicantThreatening:
			return GlobalAssets.Instance.colorSet.NotificationBadBG;
		case NotificationType.Event:
			return GlobalAssets.Instance.colorSet.NotificationEventBG;
		case NotificationType.MessageImportant:
			return GlobalAssets.Instance.colorSet.NotificationMessageImportantBG;
		}
		return GlobalAssets.Instance.colorSet.NotificationNormalBG;
	}

	// Token: 0x060066E7 RID: 26343 RVA: 0x002675D9 File Offset: 0x002657D9
	public string GetNotificationSound(NotificationType type)
	{
		return this.notificationSounds[type];
	}

	// Token: 0x04004552 RID: 17746
	public float lifetime;

	// Token: 0x04004553 RID: 17747
	public bool dirty;

	// Token: 0x04004554 RID: 17748
	public GameObject LabelPrefab;

	// Token: 0x04004555 RID: 17749
	public GameObject LabelsFolder;

	// Token: 0x04004556 RID: 17750
	public GameObject MessagesPrefab;

	// Token: 0x04004557 RID: 17751
	public GameObject MessagesFolder;

	// Token: 0x04004558 RID: 17752
	private MessageDialogFrame messageDialog;

	// Token: 0x04004559 RID: 17753
	private float initTime;

	// Token: 0x0400455A RID: 17754
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x0400455B RID: 17755
	[SerializeField]
	private List<MessageDialog> dialogPrefabs = new List<MessageDialog>();

	// Token: 0x0400455C RID: 17756
	[SerializeField]
	private Color badColorBG;

	// Token: 0x0400455D RID: 17757
	[SerializeField]
	private Color badColor = Color.red;

	// Token: 0x0400455E RID: 17758
	[SerializeField]
	private Color normalColorBG;

	// Token: 0x0400455F RID: 17759
	[SerializeField]
	private Color normalColor = Color.white;

	// Token: 0x04004560 RID: 17760
	[SerializeField]
	private Color warningColorBG;

	// Token: 0x04004561 RID: 17761
	[SerializeField]
	private Color warningColor;

	// Token: 0x04004562 RID: 17762
	[SerializeField]
	private Color messageColorBG;

	// Token: 0x04004563 RID: 17763
	[SerializeField]
	private Color messageColor;

	// Token: 0x04004564 RID: 17764
	[SerializeField]
	private Color messageImportantColorBG;

	// Token: 0x04004565 RID: 17765
	[SerializeField]
	private Color messageImportantColor;

	// Token: 0x04004566 RID: 17766
	[SerializeField]
	private Color eventColorBG;

	// Token: 0x04004567 RID: 17767
	[SerializeField]
	private Color eventColor;

	// Token: 0x04004568 RID: 17768
	public Sprite icon_normal;

	// Token: 0x04004569 RID: 17769
	public Sprite icon_warning;

	// Token: 0x0400456A RID: 17770
	public Sprite icon_bad;

	// Token: 0x0400456B RID: 17771
	public Sprite icon_threatening;

	// Token: 0x0400456C RID: 17772
	public Sprite icon_message;

	// Token: 0x0400456D RID: 17773
	public Sprite icon_message_important;

	// Token: 0x0400456E RID: 17774
	public Sprite icon_video;

	// Token: 0x0400456F RID: 17775
	public Sprite icon_event;

	// Token: 0x04004570 RID: 17776
	private List<Notification> pendingNotifications = new List<Notification>();

	// Token: 0x04004571 RID: 17777
	private List<Notification> notifications = new List<Notification>();

	// Token: 0x04004572 RID: 17778
	public TextStyleSetting TooltipTextStyle;

	// Token: 0x04004573 RID: 17779
	private Dictionary<NotificationType, string> notificationSounds = new Dictionary<NotificationType, string>();

	// Token: 0x04004574 RID: 17780
	private Dictionary<string, float> timeOfLastNotification = new Dictionary<string, float>();

	// Token: 0x04004575 RID: 17781
	private float soundDecayTime = 10f;

	// Token: 0x04004576 RID: 17782
	private List<NotificationScreen.Entry> entries = new List<NotificationScreen.Entry>();

	// Token: 0x04004577 RID: 17783
	private Dictionary<string, NotificationScreen.Entry> entriesByMessage = new Dictionary<string, NotificationScreen.Entry>();

	// Token: 0x02001DFE RID: 7678
	private class Entry
	{
		// Token: 0x0600AA39 RID: 43577 RVA: 0x003A119A File Offset: 0x0039F39A
		public Entry(GameObject label)
		{
			this.label = label;
		}

		// Token: 0x0600AA3A RID: 43578 RVA: 0x003A11B4 File Offset: 0x0039F3B4
		public void Add(Notification notification)
		{
			this.notifications.Add(notification);
			this.UpdateMessage(notification, true);
		}

		// Token: 0x0600AA3B RID: 43579 RVA: 0x003A11CA File Offset: 0x0039F3CA
		public void Remove(Notification notification)
		{
			this.notifications.Remove(notification);
			this.UpdateMessage(notification, false);
		}

		// Token: 0x0600AA3C RID: 43580 RVA: 0x003A11E4 File Offset: 0x0039F3E4
		public void UpdateMessage(Notification notification, bool playSound = true)
		{
			if (Game.IsQuitting())
			{
				return;
			}
			this.message = notification.titleText;
			if (this.notifications.Count > 1)
			{
				if (playSound && (notification.Type == NotificationType.Bad || notification.Type == NotificationType.DuplicantThreatening))
				{
					NotificationScreen.Instance.PlayDingSound(notification, this.notifications.Count);
				}
				this.message = this.message + " (" + this.notifications.Count.ToString() + ")";
			}
			if (this.label != null)
			{
				this.label.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").text = this.message;
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x0600AA3D RID: 43581 RVA: 0x003A129C File Offset: 0x0039F49C
		public Notification NextClickedNotification
		{
			get
			{
				List<Notification> list = this.notifications;
				int num = this.clickIdx;
				this.clickIdx = num + 1;
				return list[num % this.notifications.Count];
			}
		}

		// Token: 0x040088EE RID: 35054
		public string message;

		// Token: 0x040088EF RID: 35055
		public int clickIdx;

		// Token: 0x040088F0 RID: 35056
		public GameObject label;

		// Token: 0x040088F1 RID: 35057
		public List<Notification> notifications = new List<Notification>();
	}
}
