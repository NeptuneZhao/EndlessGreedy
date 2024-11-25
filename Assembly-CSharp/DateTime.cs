using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C2A RID: 3114
public class DateTime : KScreen
{
	// Token: 0x06005F6E RID: 24430 RVA: 0x00236C65 File Offset: 0x00234E65
	public static void DestroyInstance()
	{
		global::DateTime.Instance = null;
	}

	// Token: 0x06005F6F RID: 24431 RVA: 0x00236C6D File Offset: 0x00234E6D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::DateTime.Instance = this;
		this.milestoneEffect.gameObject.SetActive(false);
	}

	// Token: 0x06005F70 RID: 24432 RVA: 0x00236C8C File Offset: 0x00234E8C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnComplexToolTip = new ToolTip.ComplexTooltipDelegate(this.BuildTooltip);
		Game.Instance.Subscribe(2070437606, new Action<object>(this.OnMilestoneDayReached));
		Game.Instance.Subscribe(-720092972, new Action<object>(this.OnMilestoneDayApproaching));
	}

	// Token: 0x06005F71 RID: 24433 RVA: 0x00236CF0 File Offset: 0x00234EF0
	private List<global::Tuple<string, TextStyleSetting>> BuildTooltip()
	{
		List<global::Tuple<string, TextStyleSetting>> colonyToolTip = SaveGame.Instance.GetColonyToolTip();
		if (TimeOfDay.IsMilestoneApproaching)
		{
			colonyToolTip.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
			colonyToolTip.Add(new global::Tuple<string, TextStyleSetting>(UI.ASTEROIDCLOCK.MILESTONE_TITLE.text, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			colonyToolTip.Add(new global::Tuple<string, TextStyleSetting>(UI.ASTEROIDCLOCK.MILESTONE_DESCRIPTION.text.Replace("{0}", (GameClock.Instance.GetCycle() + 2).ToString()), ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		return colonyToolTip;
	}

	// Token: 0x06005F72 RID: 24434 RVA: 0x00236D7D File Offset: 0x00234F7D
	private void Update()
	{
		if (GameClock.Instance != null && this.displayedDayCount != GameUtil.GetCurrentCycle())
		{
			this.text.text = this.Days();
			this.displayedDayCount = GameUtil.GetCurrentCycle();
		}
	}

	// Token: 0x06005F73 RID: 24435 RVA: 0x00236DB5 File Offset: 0x00234FB5
	private void OnMilestoneDayApproaching(object data)
	{
		int num = (int)data;
		this.milestoneEffect.gameObject.SetActive(true);
		this.milestoneEffect.Play("100fx_pre", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06005F74 RID: 24436 RVA: 0x00236DEF File Offset: 0x00234FEF
	private void OnMilestoneDayReached(object data)
	{
		int num = (int)data;
		this.milestoneEffect.gameObject.SetActive(true);
		this.milestoneEffect.Play("100fx", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06005F75 RID: 24437 RVA: 0x00236E2C File Offset: 0x0023502C
	private string Days()
	{
		return GameUtil.GetCurrentCycle().ToString();
	}

	// Token: 0x0400403D RID: 16445
	public static global::DateTime Instance;

	// Token: 0x0400403E RID: 16446
	private const string MILESTONE_ANTICIPATION_ANIMATION_NAME = "100fx_pre";

	// Token: 0x0400403F RID: 16447
	private const string MILESTONE_ANIMATION_NAME = "100fx";

	// Token: 0x04004040 RID: 16448
	public LocText day;

	// Token: 0x04004041 RID: 16449
	private int displayedDayCount = -1;

	// Token: 0x04004042 RID: 16450
	[SerializeField]
	private KBatchedAnimController milestoneEffect;

	// Token: 0x04004043 RID: 16451
	[SerializeField]
	private LocText text;

	// Token: 0x04004044 RID: 16452
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x04004045 RID: 16453
	[SerializeField]
	private TextStyleSetting tooltipstyle_Days;

	// Token: 0x04004046 RID: 16454
	[SerializeField]
	private TextStyleSetting tooltipstyle_Playtime;

	// Token: 0x04004047 RID: 16455
	[SerializeField]
	public KToggle scheduleToggle;
}
