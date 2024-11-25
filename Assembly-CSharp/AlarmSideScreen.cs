using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D44 RID: 3396
public class AlarmSideScreen : SideScreenContent
{
	// Token: 0x06006AC8 RID: 27336 RVA: 0x0028340C File Offset: 0x0028160C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.nameInputField.onEndEdit += this.OnEndEditName;
		this.nameInputField.field.characterLimit = 30;
		this.tooltipInputField.onEndEdit += this.OnEndEditTooltip;
		this.tooltipInputField.field.characterLimit = 90;
		this.pauseToggle.onClick += this.TogglePause;
		this.zoomToggle.onClick += this.ToggleZoom;
		this.InitializeToggles();
	}

	// Token: 0x06006AC9 RID: 27337 RVA: 0x002834A5 File Offset: 0x002816A5
	private void OnEndEditName()
	{
		this.targetAlarm.notificationName = this.nameInputField.field.text;
		this.UpdateNotification(true);
	}

	// Token: 0x06006ACA RID: 27338 RVA: 0x002834C9 File Offset: 0x002816C9
	private void OnEndEditTooltip()
	{
		this.targetAlarm.notificationTooltip = this.tooltipInputField.field.text;
		this.UpdateNotification(true);
	}

	// Token: 0x06006ACB RID: 27339 RVA: 0x002834ED File Offset: 0x002816ED
	private void TogglePause()
	{
		this.targetAlarm.pauseOnNotify = !this.targetAlarm.pauseOnNotify;
		this.pauseCheckmark.enabled = this.targetAlarm.pauseOnNotify;
		this.UpdateNotification(true);
	}

	// Token: 0x06006ACC RID: 27340 RVA: 0x00283525 File Offset: 0x00281725
	private void ToggleZoom()
	{
		this.targetAlarm.zoomOnNotify = !this.targetAlarm.zoomOnNotify;
		this.zoomCheckmark.enabled = this.targetAlarm.zoomOnNotify;
		this.UpdateNotification(true);
	}

	// Token: 0x06006ACD RID: 27341 RVA: 0x0028355D File Offset: 0x0028175D
	private void SelectType(NotificationType type)
	{
		this.targetAlarm.notificationType = type;
		this.UpdateNotification(true);
		this.RefreshToggles();
	}

	// Token: 0x06006ACE RID: 27342 RVA: 0x00283578 File Offset: 0x00281778
	private void InitializeToggles()
	{
		if (this.toggles_by_type.Count == 0)
		{
			using (List<NotificationType>.Enumerator enumerator = this.validTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NotificationType type = enumerator.Current;
					GameObject gameObject = Util.KInstantiateUI(this.typeButtonPrefab, this.typeButtonPrefab.transform.parent.gameObject, true);
					gameObject.name = "TypeButton: " + type.ToString();
					HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
					Color notificationBGColour = NotificationScreen.Instance.GetNotificationBGColour(type);
					Color notificationColour = NotificationScreen.Instance.GetNotificationColour(type);
					notificationBGColour.a = 1f;
					notificationColour.a = 1f;
					component.GetReference<KImage>("bg").color = notificationBGColour;
					component.GetReference<KImage>("icon").color = notificationColour;
					component.GetReference<KImage>("icon").sprite = NotificationScreen.Instance.GetNotificationIcon(type);
					ToolTip component2 = gameObject.GetComponent<ToolTip>();
					NotificationType type2 = type;
					if (type2 != NotificationType.Bad)
					{
						if (type2 != NotificationType.Neutral)
						{
							if (type2 == NotificationType.DuplicantThreatening)
							{
								component2.SetSimpleTooltip(UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.DUPLICANT_THREATENING);
							}
						}
						else
						{
							component2.SetSimpleTooltip(UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.NEUTRAL);
						}
					}
					else
					{
						component2.SetSimpleTooltip(UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.BAD);
					}
					if (!this.toggles_by_type.ContainsKey(type))
					{
						this.toggles_by_type.Add(type, gameObject.GetComponent<MultiToggle>());
					}
					this.toggles_by_type[type].onClick = delegate()
					{
						this.SelectType(type);
					};
					for (int i = 0; i < this.toggles_by_type[type].states.Length; i++)
					{
						this.toggles_by_type[type].states[i].on_click_override_sound_path = NotificationScreen.Instance.GetNotificationSound(type);
					}
				}
			}
		}
	}

	// Token: 0x06006ACF RID: 27343 RVA: 0x002837BC File Offset: 0x002819BC
	private void RefreshToggles()
	{
		this.InitializeToggles();
		foreach (KeyValuePair<NotificationType, MultiToggle> keyValuePair in this.toggles_by_type)
		{
			if (this.targetAlarm.notificationType == keyValuePair.Key)
			{
				keyValuePair.Value.ChangeState(0);
			}
			else
			{
				keyValuePair.Value.ChangeState(1);
			}
		}
	}

	// Token: 0x06006AD0 RID: 27344 RVA: 0x00283840 File Offset: 0x00281A40
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicAlarm>() != null;
	}

	// Token: 0x06006AD1 RID: 27345 RVA: 0x0028384E File Offset: 0x00281A4E
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetAlarm = target.GetComponent<LogicAlarm>();
		this.RefreshToggles();
		this.UpdateVisuals();
	}

	// Token: 0x06006AD2 RID: 27346 RVA: 0x0028386F File Offset: 0x00281A6F
	private void UpdateNotification(bool clear)
	{
		this.targetAlarm.UpdateNotification(clear);
	}

	// Token: 0x06006AD3 RID: 27347 RVA: 0x00283880 File Offset: 0x00281A80
	private void UpdateVisuals()
	{
		this.nameInputField.SetDisplayValue(this.targetAlarm.notificationName);
		this.tooltipInputField.SetDisplayValue(this.targetAlarm.notificationTooltip);
		this.pauseCheckmark.enabled = this.targetAlarm.pauseOnNotify;
		this.zoomCheckmark.enabled = this.targetAlarm.zoomOnNotify;
	}

	// Token: 0x040048C7 RID: 18631
	public LogicAlarm targetAlarm;

	// Token: 0x040048C8 RID: 18632
	[SerializeField]
	private KInputField nameInputField;

	// Token: 0x040048C9 RID: 18633
	[SerializeField]
	private KInputField tooltipInputField;

	// Token: 0x040048CA RID: 18634
	[SerializeField]
	private KToggle pauseToggle;

	// Token: 0x040048CB RID: 18635
	[SerializeField]
	private Image pauseCheckmark;

	// Token: 0x040048CC RID: 18636
	[SerializeField]
	private KToggle zoomToggle;

	// Token: 0x040048CD RID: 18637
	[SerializeField]
	private Image zoomCheckmark;

	// Token: 0x040048CE RID: 18638
	[SerializeField]
	private GameObject typeButtonPrefab;

	// Token: 0x040048CF RID: 18639
	private List<NotificationType> validTypes = new List<NotificationType>
	{
		NotificationType.Bad,
		NotificationType.Neutral,
		NotificationType.DuplicantThreatening
	};

	// Token: 0x040048D0 RID: 18640
	private Dictionary<NotificationType, MultiToggle> toggles_by_type = new Dictionary<NotificationType, MultiToggle>();
}
