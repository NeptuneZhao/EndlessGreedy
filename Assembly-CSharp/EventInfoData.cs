using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000472 RID: 1138
public class EventInfoData
{
	// Token: 0x0600187E RID: 6270 RVA: 0x00082CF8 File Offset: 0x00080EF8
	public EventInfoData(string title, string description, HashedString animFileName)
	{
		this.title = title;
		this.description = description;
		this.animFileName = animFileName;
	}

	// Token: 0x0600187F RID: 6271 RVA: 0x00082D46 File Offset: 0x00080F46
	public List<EventInfoData.Option> GetOptions()
	{
		this.FinalizeText();
		return this.options;
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x00082D54 File Offset: 0x00080F54
	public EventInfoData.Option AddOption(string mainText, string description = null)
	{
		EventInfoData.Option option = new EventInfoData.Option
		{
			mainText = mainText,
			description = description
		};
		this.options.Add(option);
		this.dirty = true;
		return option;
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x00082D8C File Offset: 0x00080F8C
	public EventInfoData.Option SimpleOption(string mainText, System.Action callback)
	{
		EventInfoData.Option option = new EventInfoData.Option
		{
			mainText = mainText,
			callback = callback
		};
		this.options.Add(option);
		this.dirty = true;
		return option;
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x00082DC1 File Offset: 0x00080FC1
	public EventInfoData.Option AddDefaultOption(System.Action callback = null)
	{
		return this.SimpleOption(GAMEPLAY_EVENTS.DEFAULT_OPTION_NAME, callback);
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x00082DD4 File Offset: 0x00080FD4
	public EventInfoData.Option AddDefaultConsiderLaterOption(System.Action callback = null)
	{
		return this.SimpleOption(GAMEPLAY_EVENTS.DEFAULT_OPTION_CONSIDER_NAME, callback);
	}

	// Token: 0x06001884 RID: 6276 RVA: 0x00082DE7 File Offset: 0x00080FE7
	public void SetTextParameter(string key, string value)
	{
		this.textParameters[key] = value;
		this.dirty = true;
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x00082E00 File Offset: 0x00081000
	public void FinalizeText()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		foreach (KeyValuePair<string, string> keyValuePair in this.textParameters)
		{
			string oldValue = "{" + keyValuePair.Key + "}";
			if (this.title != null)
			{
				this.title = this.title.Replace(oldValue, keyValuePair.Value);
			}
			if (this.description != null)
			{
				this.description = this.description.Replace(oldValue, keyValuePair.Value);
			}
			if (this.location != null)
			{
				this.location = this.location.Replace(oldValue, keyValuePair.Value);
			}
			if (this.whenDescription != null)
			{
				this.whenDescription = this.whenDescription.Replace(oldValue, keyValuePair.Value);
			}
			foreach (EventInfoData.Option option in this.options)
			{
				if (option.mainText != null)
				{
					option.mainText = option.mainText.Replace(oldValue, keyValuePair.Value);
				}
				if (option.description != null)
				{
					option.description = option.description.Replace(oldValue, keyValuePair.Value);
				}
				if (option.tooltip != null)
				{
					option.tooltip = option.tooltip.Replace(oldValue, keyValuePair.Value);
				}
				foreach (EventInfoData.OptionIcon optionIcon in option.informationIcons)
				{
					if (optionIcon.tooltip != null)
					{
						optionIcon.tooltip = optionIcon.tooltip.Replace(oldValue, keyValuePair.Value);
					}
				}
				foreach (EventInfoData.OptionIcon optionIcon2 in option.consequenceIcons)
				{
					if (optionIcon2.tooltip != null)
					{
						optionIcon2.tooltip = optionIcon2.tooltip.Replace(oldValue, keyValuePair.Value);
					}
				}
			}
		}
	}

	// Token: 0x04000D91 RID: 3473
	public string title;

	// Token: 0x04000D92 RID: 3474
	public string description;

	// Token: 0x04000D93 RID: 3475
	public string location;

	// Token: 0x04000D94 RID: 3476
	public string whenDescription;

	// Token: 0x04000D95 RID: 3477
	public Transform clickFocus;

	// Token: 0x04000D96 RID: 3478
	public GameObject[] minions;

	// Token: 0x04000D97 RID: 3479
	public GameObject artifact;

	// Token: 0x04000D98 RID: 3480
	public HashedString animFileName;

	// Token: 0x04000D99 RID: 3481
	public HashedString mainAnim = "event";

	// Token: 0x04000D9A RID: 3482
	public Dictionary<string, string> textParameters = new Dictionary<string, string>();

	// Token: 0x04000D9B RID: 3483
	public List<EventInfoData.Option> options = new List<EventInfoData.Option>();

	// Token: 0x04000D9C RID: 3484
	public System.Action showCallback;

	// Token: 0x04000D9D RID: 3485
	private bool dirty;

	// Token: 0x0200122B RID: 4651
	public class OptionIcon
	{
		// Token: 0x0600825B RID: 33371 RVA: 0x0031CC53 File Offset: 0x0031AE53
		public OptionIcon(Sprite sprite, EventInfoData.OptionIcon.ContainerType containerType, string tooltip, float scale = 1f)
		{
			this.sprite = sprite;
			this.containerType = containerType;
			this.tooltip = tooltip;
			this.scale = scale;
		}

		// Token: 0x0400629C RID: 25244
		public EventInfoData.OptionIcon.ContainerType containerType;

		// Token: 0x0400629D RID: 25245
		public Sprite sprite;

		// Token: 0x0400629E RID: 25246
		public string tooltip;

		// Token: 0x0400629F RID: 25247
		public float scale;

		// Token: 0x020023FD RID: 9213
		public enum ContainerType
		{
			// Token: 0x0400A0AA RID: 41130
			Neutral,
			// Token: 0x0400A0AB RID: 41131
			Positive,
			// Token: 0x0400A0AC RID: 41132
			Negative,
			// Token: 0x0400A0AD RID: 41133
			Information
		}
	}

	// Token: 0x0200122C RID: 4652
	public class Option
	{
		// Token: 0x0600825C RID: 33372 RVA: 0x0031CC78 File Offset: 0x0031AE78
		public void AddInformationIcon(string tooltip, float scale = 1f)
		{
			this.informationIcons.Add(new EventInfoData.OptionIcon(null, EventInfoData.OptionIcon.ContainerType.Information, tooltip, scale));
		}

		// Token: 0x0600825D RID: 33373 RVA: 0x0031CC8E File Offset: 0x0031AE8E
		public void AddPositiveIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Positive, tooltip, scale));
		}

		// Token: 0x0600825E RID: 33374 RVA: 0x0031CCA4 File Offset: 0x0031AEA4
		public void AddNeutralIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Neutral, tooltip, scale));
		}

		// Token: 0x0600825F RID: 33375 RVA: 0x0031CCBA File Offset: 0x0031AEBA
		public void AddNegativeIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Negative, tooltip, scale));
		}

		// Token: 0x040062A0 RID: 25248
		public string mainText;

		// Token: 0x040062A1 RID: 25249
		public string description;

		// Token: 0x040062A2 RID: 25250
		public string tooltip;

		// Token: 0x040062A3 RID: 25251
		public System.Action callback;

		// Token: 0x040062A4 RID: 25252
		public List<EventInfoData.OptionIcon> informationIcons = new List<EventInfoData.OptionIcon>();

		// Token: 0x040062A5 RID: 25253
		public List<EventInfoData.OptionIcon> consequenceIcons = new List<EventInfoData.OptionIcon>();

		// Token: 0x040062A6 RID: 25254
		public bool allowed = true;
	}
}
