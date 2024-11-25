using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DB5 RID: 3509
public class TimeRangeSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x06006EE4 RID: 28388 RVA: 0x0029A2A4 File Offset: 0x002984A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.labelHeaderStart.text = UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.ON;
		this.labelHeaderDuration.text = UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.DURATION;
	}

	// Token: 0x06006EE5 RID: 28389 RVA: 0x0029A2D6 File Offset: 0x002984D6
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicTimeOfDaySensor>() != null;
	}

	// Token: 0x06006EE6 RID: 28390 RVA: 0x0029A2E4 File Offset: 0x002984E4
	public override void SetTarget(GameObject target)
	{
		this.imageActiveZone.color = GlobalAssets.Instance.colorSet.logicOnSidescreen;
		this.imageInactiveZone.color = GlobalAssets.Instance.colorSet.logicOffSidescreen;
		base.SetTarget(target);
		this.targetTimedSwitch = target.GetComponent<LogicTimeOfDaySensor>();
		this.duration.onValueChanged.RemoveAllListeners();
		this.startTime.onValueChanged.RemoveAllListeners();
		this.startTime.value = this.targetTimedSwitch.startTime;
		this.duration.value = this.targetTimedSwitch.duration;
		this.ChangeSetting();
		this.startTime.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
		this.duration.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
	}

	// Token: 0x06006EE7 RID: 28391 RVA: 0x0029A3CC File Offset: 0x002985CC
	private void ChangeSetting()
	{
		this.targetTimedSwitch.startTime = this.startTime.value;
		this.targetTimedSwitch.duration = this.duration.value;
		this.imageActiveZone.rectTransform.rotation = Quaternion.identity;
		this.imageActiveZone.rectTransform.Rotate(0f, 0f, this.NormalizedValueToDegrees(this.startTime.value));
		this.imageActiveZone.fillAmount = this.duration.value;
		this.labelValueStart.text = GameUtil.GetFormattedPercent(this.targetTimedSwitch.startTime * 100f, GameUtil.TimeSlice.None);
		this.labelValueDuration.text = GameUtil.GetFormattedPercent(this.targetTimedSwitch.duration * 100f, GameUtil.TimeSlice.None);
		this.endIndicator.rotation = Quaternion.identity;
		this.endIndicator.Rotate(0f, 0f, this.NormalizedValueToDegrees(this.startTime.value + this.duration.value));
		this.startTime.SetTooltipText(string.Format(UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.ON_TOOLTIP, GameUtil.GetFormattedPercent(this.targetTimedSwitch.startTime * 100f, GameUtil.TimeSlice.None)));
		this.duration.SetTooltipText(string.Format(UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.DURATION_TOOLTIP, GameUtil.GetFormattedPercent(this.targetTimedSwitch.duration * 100f, GameUtil.TimeSlice.None)));
	}

	// Token: 0x06006EE8 RID: 28392 RVA: 0x0029A543 File Offset: 0x00298743
	public void Render200ms(float dt)
	{
		this.currentTimeMarker.rotation = Quaternion.identity;
		this.currentTimeMarker.Rotate(0f, 0f, this.NormalizedValueToDegrees(GameClock.Instance.GetCurrentCycleAsPercentage()));
	}

	// Token: 0x06006EE9 RID: 28393 RVA: 0x0029A57A File Offset: 0x0029877A
	private float NormalizedValueToDegrees(float value)
	{
		return 360f * value;
	}

	// Token: 0x06006EEA RID: 28394 RVA: 0x0029A583 File Offset: 0x00298783
	private float SecondsToDegrees(float seconds)
	{
		return 360f * (seconds / 600f);
	}

	// Token: 0x06006EEB RID: 28395 RVA: 0x0029A592 File Offset: 0x00298792
	private float DegreesToNormalizedValue(float degrees)
	{
		return degrees / 360f;
	}

	// Token: 0x04004B9D RID: 19357
	public Image imageInactiveZone;

	// Token: 0x04004B9E RID: 19358
	public Image imageActiveZone;

	// Token: 0x04004B9F RID: 19359
	private LogicTimeOfDaySensor targetTimedSwitch;

	// Token: 0x04004BA0 RID: 19360
	public KSlider startTime;

	// Token: 0x04004BA1 RID: 19361
	public KSlider duration;

	// Token: 0x04004BA2 RID: 19362
	public RectTransform endIndicator;

	// Token: 0x04004BA3 RID: 19363
	public LocText labelHeaderStart;

	// Token: 0x04004BA4 RID: 19364
	public LocText labelHeaderDuration;

	// Token: 0x04004BA5 RID: 19365
	public LocText labelValueStart;

	// Token: 0x04004BA6 RID: 19366
	public LocText labelValueDuration;

	// Token: 0x04004BA7 RID: 19367
	public RectTransform currentTimeMarker;
}
