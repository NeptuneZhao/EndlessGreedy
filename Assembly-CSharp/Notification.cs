using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000598 RID: 1432
public class Notification
{
	// Token: 0x1700016A RID: 362
	// (get) Token: 0x060021A9 RID: 8617 RVA: 0x000BBE97 File Offset: 0x000BA097
	// (set) Token: 0x060021AA RID: 8618 RVA: 0x000BBE9F File Offset: 0x000BA09F
	public NotificationType Type { get; set; }

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x060021AB RID: 8619 RVA: 0x000BBEA8 File Offset: 0x000BA0A8
	// (set) Token: 0x060021AC RID: 8620 RVA: 0x000BBEB0 File Offset: 0x000BA0B0
	public Notifier Notifier { get; set; }

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x060021AD RID: 8621 RVA: 0x000BBEB9 File Offset: 0x000BA0B9
	// (set) Token: 0x060021AE RID: 8622 RVA: 0x000BBEC1 File Offset: 0x000BA0C1
	public Transform clickFocus { get; set; }

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x060021AF RID: 8623 RVA: 0x000BBECA File Offset: 0x000BA0CA
	// (set) Token: 0x060021B0 RID: 8624 RVA: 0x000BBED2 File Offset: 0x000BA0D2
	public float Time { get; set; }

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x060021B1 RID: 8625 RVA: 0x000BBEDB File Offset: 0x000BA0DB
	// (set) Token: 0x060021B2 RID: 8626 RVA: 0x000BBEE3 File Offset: 0x000BA0E3
	public float GameTime { get; set; }

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x060021B3 RID: 8627 RVA: 0x000BBEEC File Offset: 0x000BA0EC
	// (set) Token: 0x060021B4 RID: 8628 RVA: 0x000BBEF4 File Offset: 0x000BA0F4
	public float Delay { get; set; }

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x060021B5 RID: 8629 RVA: 0x000BBEFD File Offset: 0x000BA0FD
	// (set) Token: 0x060021B6 RID: 8630 RVA: 0x000BBF05 File Offset: 0x000BA105
	public int Idx { get; set; }

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x060021B7 RID: 8631 RVA: 0x000BBF0E File Offset: 0x000BA10E
	// (set) Token: 0x060021B8 RID: 8632 RVA: 0x000BBF16 File Offset: 0x000BA116
	public Func<List<Notification>, object, string> ToolTip { get; set; }

	// Token: 0x060021B9 RID: 8633 RVA: 0x000BBF1F File Offset: 0x000BA11F
	public bool IsReady()
	{
		return UnityEngine.Time.time >= this.GameTime + this.Delay;
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x060021BA RID: 8634 RVA: 0x000BBF38 File Offset: 0x000BA138
	// (set) Token: 0x060021BB RID: 8635 RVA: 0x000BBF40 File Offset: 0x000BA140
	public string titleText { get; private set; }

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x060021BC RID: 8636 RVA: 0x000BBF49 File Offset: 0x000BA149
	// (set) Token: 0x060021BD RID: 8637 RVA: 0x000BBF51 File Offset: 0x000BA151
	public string NotifierName
	{
		get
		{
			return this.notifierName;
		}
		set
		{
			this.notifierName = value;
			this.titleText = this.ReplaceTags(this.titleText);
		}
	}

	// Token: 0x060021BE RID: 8638 RVA: 0x000BBF6C File Offset: 0x000BA16C
	public Notification(string title, NotificationType type, Func<List<Notification>, object, string> tooltip = null, object tooltip_data = null, bool expires = true, float delay = 0f, Notification.ClickCallback custom_click_callback = null, object custom_click_data = null, Transform click_focus = null, bool volume_attenuation = true, bool clear_on_click = false, bool show_dismiss_button = false)
	{
		this.titleText = title;
		this.Type = type;
		this.ToolTip = tooltip;
		this.tooltipData = tooltip_data;
		this.expires = expires;
		this.Delay = delay;
		this.customClickCallback = custom_click_callback;
		this.customClickData = custom_click_data;
		this.clickFocus = click_focus;
		this.volume_attenuation = volume_attenuation;
		this.clearOnClick = clear_on_click;
		this.showDismissButton = show_dismiss_button;
		int num = this.notificationIncrement;
		this.notificationIncrement = num + 1;
		this.Idx = num;
	}

	// Token: 0x060021BF RID: 8639 RVA: 0x000BC008 File Offset: 0x000BA208
	public void Clear()
	{
		if (this.Notifier != null)
		{
			this.Notifier.Remove(this);
			return;
		}
		NotificationManager.Instance.RemoveNotification(this);
	}

	// Token: 0x060021C0 RID: 8640 RVA: 0x000BC030 File Offset: 0x000BA230
	private string ReplaceTags(string text)
	{
		DebugUtil.Assert(text != null);
		int num = text.IndexOf('{');
		int num2 = text.IndexOf('}');
		if (0 <= num && num < num2)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num3 = 0;
			while (0 <= num)
			{
				string value = text.Substring(num3, num - num3);
				stringBuilder.Append(value);
				num2 = text.IndexOf('}', num);
				if (num >= num2)
				{
					break;
				}
				string tag = text.Substring(num + 1, num2 - num - 1);
				string tagDescription = this.GetTagDescription(tag);
				stringBuilder.Append(tagDescription);
				num3 = num2 + 1;
				num = text.IndexOf('{', num2);
			}
			stringBuilder.Append(text.Substring(num3, text.Length - num3));
			return stringBuilder.ToString();
		}
		return text;
	}

	// Token: 0x060021C1 RID: 8641 RVA: 0x000BC0E4 File Offset: 0x000BA2E4
	private string GetTagDescription(string tag)
	{
		string result;
		if (tag == "NotifierName")
		{
			result = this.notifierName;
		}
		else
		{
			result = "UNKNOWN TAG: " + tag;
		}
		return result;
	}

	// Token: 0x040012F6 RID: 4854
	public object tooltipData;

	// Token: 0x040012F7 RID: 4855
	public bool expires = true;

	// Token: 0x040012F8 RID: 4856
	public bool playSound = true;

	// Token: 0x040012F9 RID: 4857
	public bool volume_attenuation = true;

	// Token: 0x040012FA RID: 4858
	public Notification.ClickCallback customClickCallback;

	// Token: 0x040012FB RID: 4859
	public bool clearOnClick;

	// Token: 0x040012FC RID: 4860
	public bool showDismissButton;

	// Token: 0x040012FD RID: 4861
	public object customClickData;

	// Token: 0x040012FE RID: 4862
	private int notificationIncrement;

	// Token: 0x04001300 RID: 4864
	private string notifierName;

	// Token: 0x02001387 RID: 4999
	// (Invoke) Token: 0x06008775 RID: 34677
	public delegate void ClickCallback(object data);
}
