using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DB6 RID: 3510
public class TimerSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x06006EEF RID: 28399 RVA: 0x0029A5B3 File Offset: 0x002987B3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.labelHeaderOnDuration.text = UI.UISIDESCREENS.TIMER_SIDE_SCREEN.ON;
		this.labelHeaderOffDuration.text = UI.UISIDESCREENS.TIMER_SIDE_SCREEN.OFF;
	}

	// Token: 0x06006EF0 RID: 28400 RVA: 0x0029A5E8 File Offset: 0x002987E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.modeButton.onClick += delegate()
		{
			this.ToggleMode();
		};
		this.resetButton.onClick += this.ResetTimer;
		this.onDurationNumberInput.onEndEdit += delegate()
		{
			this.UpdateDurationValueFromTextInput(this.onDurationNumberInput.currentValue, this.onDurationSlider);
		};
		this.offDurationNumberInput.onEndEdit += delegate()
		{
			this.UpdateDurationValueFromTextInput(this.offDurationNumberInput.currentValue, this.offDurationSlider);
		};
		this.onDurationSlider.wholeNumbers = false;
		this.offDurationSlider.wholeNumbers = false;
	}

	// Token: 0x06006EF1 RID: 28401 RVA: 0x0029A66F File Offset: 0x0029886F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicTimerSensor>() != null;
	}

	// Token: 0x06006EF2 RID: 28402 RVA: 0x0029A680 File Offset: 0x00298880
	public override void SetTarget(GameObject target)
	{
		this.greenActiveZone.color = GlobalAssets.Instance.colorSet.logicOnSidescreen;
		this.redActiveZone.color = GlobalAssets.Instance.colorSet.logicOffSidescreen;
		base.SetTarget(target);
		this.targetTimedSwitch = target.GetComponent<LogicTimerSensor>();
		this.onDurationSlider.onValueChanged.RemoveAllListeners();
		this.offDurationSlider.onValueChanged.RemoveAllListeners();
		this.cyclesMode = this.targetTimedSwitch.displayCyclesMode;
		this.UpdateVisualsForNewTarget();
		this.ReconfigureRingVisuals();
		this.onDurationSlider.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
		this.offDurationSlider.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
	}

	// Token: 0x06006EF3 RID: 28403 RVA: 0x0029A754 File Offset: 0x00298954
	private void UpdateVisualsForNewTarget()
	{
		float onDuration = this.targetTimedSwitch.onDuration;
		float offDuration = this.targetTimedSwitch.offDuration;
		bool displayCyclesMode = this.targetTimedSwitch.displayCyclesMode;
		if (displayCyclesMode)
		{
			this.onDurationSlider.minValue = this.minCycles;
			this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
			this.onDurationSlider.maxValue = this.maxCycles;
			this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
			this.onDurationNumberInput.decimalPlaces = 2;
			this.offDurationSlider.minValue = this.minCycles;
			this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
			this.offDurationSlider.maxValue = this.maxCycles;
			this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
			this.offDurationNumberInput.decimalPlaces = 2;
			this.onDurationSlider.value = onDuration / 600f;
			this.offDurationSlider.value = offDuration / 600f;
			this.onDurationNumberInput.SetAmount(onDuration / 600f);
			this.offDurationNumberInput.SetAmount(offDuration / 600f);
		}
		else
		{
			this.onDurationSlider.minValue = this.minSeconds;
			this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
			this.onDurationSlider.maxValue = this.maxSeconds;
			this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
			this.onDurationNumberInput.decimalPlaces = 1;
			this.offDurationSlider.minValue = this.minSeconds;
			this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
			this.offDurationSlider.maxValue = this.maxSeconds;
			this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
			this.offDurationNumberInput.decimalPlaces = 1;
			this.onDurationSlider.value = onDuration;
			this.offDurationSlider.value = offDuration;
			this.onDurationNumberInput.SetAmount(onDuration);
			this.offDurationNumberInput.SetAmount(offDuration);
		}
		this.modeButton.GetComponentInChildren<LocText>().text = (displayCyclesMode ? UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_CYCLES : UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_SECONDS);
	}

	// Token: 0x06006EF4 RID: 28404 RVA: 0x0029A994 File Offset: 0x00298B94
	private void ToggleMode()
	{
		this.cyclesMode = !this.cyclesMode;
		this.targetTimedSwitch.displayCyclesMode = this.cyclesMode;
		float num = this.onDurationSlider.value;
		float num2 = this.offDurationSlider.value;
		if (this.cyclesMode)
		{
			num = this.onDurationSlider.value / 600f;
			num2 = this.offDurationSlider.value / 600f;
		}
		else
		{
			num = this.onDurationSlider.value * 600f;
			num2 = this.offDurationSlider.value * 600f;
		}
		this.onDurationSlider.minValue = (this.cyclesMode ? this.minCycles : this.minSeconds);
		this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
		this.onDurationSlider.maxValue = (this.cyclesMode ? this.maxCycles : this.maxSeconds);
		this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
		this.onDurationNumberInput.decimalPlaces = (this.cyclesMode ? 2 : 1);
		this.offDurationSlider.minValue = (this.cyclesMode ? this.minCycles : this.minSeconds);
		this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
		this.offDurationSlider.maxValue = (this.cyclesMode ? this.maxCycles : this.maxSeconds);
		this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
		this.offDurationNumberInput.decimalPlaces = (this.cyclesMode ? 2 : 1);
		this.onDurationSlider.value = num;
		this.offDurationSlider.value = num2;
		this.onDurationNumberInput.SetAmount(num);
		this.offDurationNumberInput.SetAmount(num2);
		this.modeButton.GetComponentInChildren<LocText>().text = (this.cyclesMode ? UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_CYCLES : UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_SECONDS);
	}

	// Token: 0x06006EF5 RID: 28405 RVA: 0x0029AB90 File Offset: 0x00298D90
	private void ChangeSetting()
	{
		this.targetTimedSwitch.onDuration = (this.cyclesMode ? (this.onDurationSlider.value * 600f) : this.onDurationSlider.value);
		this.targetTimedSwitch.offDuration = (this.cyclesMode ? (this.offDurationSlider.value * 600f) : this.offDurationSlider.value);
		this.ReconfigureRingVisuals();
		this.onDurationNumberInput.SetDisplayValue(this.cyclesMode ? (this.targetTimedSwitch.onDuration / 600f).ToString("F2") : this.targetTimedSwitch.onDuration.ToString());
		this.offDurationNumberInput.SetDisplayValue(this.cyclesMode ? (this.targetTimedSwitch.offDuration / 600f).ToString("F2") : this.targetTimedSwitch.offDuration.ToString());
		this.onDurationSlider.SetTooltipText(string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.GREEN_DURATION_TOOLTIP, this.cyclesMode ? GameUtil.GetFormattedCycles(this.targetTimedSwitch.onDuration, "F2", false) : GameUtil.GetFormattedTime(this.targetTimedSwitch.onDuration, "F0")));
		this.offDurationSlider.SetTooltipText(string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.RED_DURATION_TOOLTIP, this.cyclesMode ? GameUtil.GetFormattedCycles(this.targetTimedSwitch.offDuration, "F2", false) : GameUtil.GetFormattedTime(this.targetTimedSwitch.offDuration, "F0")));
		if (this.phaseLength == 0f)
		{
			this.timeLeft.text = UI.UISIDESCREENS.TIMER_SIDE_SCREEN.DISABLED;
			if (this.targetTimedSwitch.IsSwitchedOn)
			{
				this.greenActiveZone.fillAmount = 1f;
				this.redActiveZone.fillAmount = 0f;
			}
			else
			{
				this.greenActiveZone.fillAmount = 0f;
				this.redActiveZone.fillAmount = 1f;
			}
			this.targetTimedSwitch.timeElapsedInCurrentState = 0f;
			this.currentTimeMarker.rotation = Quaternion.identity;
			this.currentTimeMarker.Rotate(0f, 0f, 0f);
		}
	}

	// Token: 0x06006EF6 RID: 28406 RVA: 0x0029ADD8 File Offset: 0x00298FD8
	private void ReconfigureRingVisuals()
	{
		this.phaseLength = this.targetTimedSwitch.onDuration + this.targetTimedSwitch.offDuration;
		this.greenActiveZone.fillAmount = this.targetTimedSwitch.onDuration / this.phaseLength;
		this.redActiveZone.fillAmount = this.targetTimedSwitch.offDuration / this.phaseLength;
	}

	// Token: 0x06006EF7 RID: 28407 RVA: 0x0029AE3C File Offset: 0x0029903C
	public void RenderEveryTick(float dt)
	{
		if (this.phaseLength == 0f)
		{
			return;
		}
		float timeElapsedInCurrentState = this.targetTimedSwitch.timeElapsedInCurrentState;
		if (this.cyclesMode)
		{
			this.timeLeft.text = string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.CURRENT_TIME, GameUtil.GetFormattedCycles(timeElapsedInCurrentState, "F2", false), GameUtil.GetFormattedCycles(this.targetTimedSwitch.IsSwitchedOn ? this.targetTimedSwitch.onDuration : this.targetTimedSwitch.offDuration, "F2", false));
		}
		else
		{
			this.timeLeft.text = string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.CURRENT_TIME, GameUtil.GetFormattedTime(timeElapsedInCurrentState, "F1"), GameUtil.GetFormattedTime(this.targetTimedSwitch.IsSwitchedOn ? this.targetTimedSwitch.onDuration : this.targetTimedSwitch.offDuration, "F1"));
		}
		this.currentTimeMarker.rotation = Quaternion.identity;
		if (this.targetTimedSwitch.IsSwitchedOn)
		{
			this.currentTimeMarker.Rotate(0f, 0f, this.targetTimedSwitch.timeElapsedInCurrentState / this.phaseLength * -360f);
			return;
		}
		this.currentTimeMarker.Rotate(0f, 0f, (this.targetTimedSwitch.onDuration + this.targetTimedSwitch.timeElapsedInCurrentState) / this.phaseLength * -360f);
	}

	// Token: 0x06006EF8 RID: 28408 RVA: 0x0029AF9C File Offset: 0x0029919C
	private void UpdateDurationValueFromTextInput(float newValue, KSlider slider)
	{
		if (newValue < slider.minValue)
		{
			newValue = slider.minValue;
		}
		if (newValue > slider.maxValue)
		{
			newValue = slider.maxValue;
		}
		slider.value = newValue;
		NonLinearSlider nonLinearSlider = slider as NonLinearSlider;
		if (nonLinearSlider != null)
		{
			slider.value = nonLinearSlider.GetPercentageFromValue(newValue);
			return;
		}
		slider.value = newValue;
	}

	// Token: 0x06006EF9 RID: 28409 RVA: 0x0029AFF7 File Offset: 0x002991F7
	private void ResetTimer()
	{
		this.targetTimedSwitch.ResetTimer();
	}

	// Token: 0x04004BA8 RID: 19368
	public Image greenActiveZone;

	// Token: 0x04004BA9 RID: 19369
	public Image redActiveZone;

	// Token: 0x04004BAA RID: 19370
	private LogicTimerSensor targetTimedSwitch;

	// Token: 0x04004BAB RID: 19371
	public KToggle modeButton;

	// Token: 0x04004BAC RID: 19372
	public KButton resetButton;

	// Token: 0x04004BAD RID: 19373
	public KSlider onDurationSlider;

	// Token: 0x04004BAE RID: 19374
	[SerializeField]
	private KNumberInputField onDurationNumberInput;

	// Token: 0x04004BAF RID: 19375
	public KSlider offDurationSlider;

	// Token: 0x04004BB0 RID: 19376
	[SerializeField]
	private KNumberInputField offDurationNumberInput;

	// Token: 0x04004BB1 RID: 19377
	public RectTransform endIndicator;

	// Token: 0x04004BB2 RID: 19378
	public RectTransform currentTimeMarker;

	// Token: 0x04004BB3 RID: 19379
	public LocText labelHeaderOnDuration;

	// Token: 0x04004BB4 RID: 19380
	public LocText labelHeaderOffDuration;

	// Token: 0x04004BB5 RID: 19381
	public LocText labelValueOnDuration;

	// Token: 0x04004BB6 RID: 19382
	public LocText labelValueOffDuration;

	// Token: 0x04004BB7 RID: 19383
	public LocText timeLeft;

	// Token: 0x04004BB8 RID: 19384
	public float phaseLength;

	// Token: 0x04004BB9 RID: 19385
	private bool cyclesMode;

	// Token: 0x04004BBA RID: 19386
	[SerializeField]
	private float minSeconds;

	// Token: 0x04004BBB RID: 19387
	[SerializeField]
	private float maxSeconds = 600f;

	// Token: 0x04004BBC RID: 19388
	[SerializeField]
	private float minCycles;

	// Token: 0x04004BBD RID: 19389
	[SerializeField]
	private float maxCycles = 10f;

	// Token: 0x04004BBE RID: 19390
	private const int CYCLEMODE_DECIMALS = 2;

	// Token: 0x04004BBF RID: 19391
	private const int SECONDSMODE_DECIMALS = 1;
}
