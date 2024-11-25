using System;
using UnityEngine;

// Token: 0x02000B32 RID: 2866
public class Thought : Resource
{
	// Token: 0x0600557D RID: 21885 RVA: 0x001E8C50 File Offset: 0x001E6E50
	public Thought(string id, ResourceSet parent, Sprite icon, string mode_icon, string sound_name, string bubble, string speech_prefix, LocString hover_text, bool show_immediately = false, float show_time = 4f) : base(id, parent, null)
	{
		this.sprite = icon;
		if (mode_icon != null)
		{
			this.modeSprite = Assets.GetSprite(mode_icon);
		}
		this.bubbleSprite = Assets.GetSprite(bubble);
		this.sound = sound_name;
		this.speechPrefix = speech_prefix;
		this.hoverText = hover_text;
		this.showImmediately = show_immediately;
		this.showTime = show_time;
	}

	// Token: 0x0600557E RID: 21886 RVA: 0x001E8CC0 File Offset: 0x001E6EC0
	public Thought(string id, ResourceSet parent, string icon, string mode_icon, string sound_name, string bubble, string speech_prefix, LocString hover_text, bool show_immediately = false, float show_time = 4f) : base(id, parent, null)
	{
		this.sprite = Assets.GetSprite(icon);
		if (mode_icon != null)
		{
			this.modeSprite = Assets.GetSprite(mode_icon);
		}
		this.bubbleSprite = Assets.GetSprite(bubble);
		this.sound = sound_name;
		this.speechPrefix = speech_prefix;
		this.hoverText = hover_text;
		this.showImmediately = show_immediately;
		this.showTime = show_time;
	}

	// Token: 0x040037FA RID: 14330
	public int priority;

	// Token: 0x040037FB RID: 14331
	public Sprite sprite;

	// Token: 0x040037FC RID: 14332
	public Sprite modeSprite;

	// Token: 0x040037FD RID: 14333
	public string sound;

	// Token: 0x040037FE RID: 14334
	public Sprite bubbleSprite;

	// Token: 0x040037FF RID: 14335
	public string speechPrefix;

	// Token: 0x04003800 RID: 14336
	public LocString hoverText;

	// Token: 0x04003801 RID: 14337
	public bool showImmediately;

	// Token: 0x04003802 RID: 14338
	public float showTime;
}
