using System;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000DB4 RID: 3508
public class ThresholdSwitchSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x06006ED1 RID: 28369 RVA: 0x00299BC0 File Offset: 0x00297DC0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.aboveToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(true);
		};
		this.belowToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(false);
		};
		LocText component = this.aboveToggle.transform.GetChild(0).GetComponent<LocText>();
		TMP_Text component2 = this.belowToggle.transform.GetChild(0).GetComponent<LocText>();
		component.SetText(UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.ABOVE_BUTTON);
		component2.SetText(UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BELOW_BUTTON);
		this.thresholdSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.thresholdSlider.GetValueForPercentage(GameUtil.GetRoundedTemperatureInKelvin(this.thresholdSlider.value)));
		};
		this.thresholdSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.thresholdSlider.GetValueForPercentage(GameUtil.GetRoundedTemperatureInKelvin(this.thresholdSlider.value)));
		};
		this.thresholdSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.thresholdSlider.GetValueForPercentage(GameUtil.GetRoundedTemperatureInKelvin(this.thresholdSlider.value)));
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x06006ED2 RID: 28370 RVA: 0x00299CB5 File Offset: 0x00297EB5
	public void Render200ms(float dt)
	{
		if (this.target == null)
		{
			this.target = null;
			return;
		}
		this.UpdateLabels();
	}

	// Token: 0x06006ED3 RID: 28371 RVA: 0x00299CD3 File Offset: 0x00297ED3
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IThresholdSwitch>() != null;
	}

	// Token: 0x06006ED4 RID: 28372 RVA: 0x00299CE0 File Offset: 0x00297EE0
	public override void SetTarget(GameObject new_target)
	{
		this.target = null;
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target;
		this.thresholdSwitch = this.target.GetComponent<IThresholdSwitch>();
		if (this.thresholdSwitch == null)
		{
			this.target = null;
			global::Debug.LogError("The gameObject received does not contain a IThresholdSwitch component");
			return;
		}
		this.UpdateLabels();
		if (this.target.GetComponent<IThresholdSwitch>().LayoutType == ThresholdScreenLayoutType.SliderBar)
		{
			this.thresholdSlider.gameObject.SetActive(true);
			this.thresholdSlider.minValue = 0f;
			this.thresholdSlider.maxValue = 100f;
			this.thresholdSlider.SetRanges(this.thresholdSwitch.GetRanges);
			this.thresholdSlider.value = this.thresholdSlider.GetPercentageFromValue(this.thresholdSwitch.Threshold);
			this.thresholdSlider.GetComponentInChildren<ToolTip>();
		}
		else
		{
			this.thresholdSlider.gameObject.SetActive(false);
		}
		MultiToggle incrementMinorToggle = this.incrementMinor.GetComponent<MultiToggle>();
		incrementMinorToggle.onClick = delegate()
		{
			this.UpdateThresholdValue(this.thresholdSwitch.Threshold + (float)this.thresholdSwitch.IncrementScale);
			incrementMinorToggle.ChangeState(1);
		};
		incrementMinorToggle.onStopHold = delegate()
		{
			incrementMinorToggle.ChangeState(0);
		};
		MultiToggle incrementMajorToggle = this.incrementMajor.GetComponent<MultiToggle>();
		incrementMajorToggle.onClick = delegate()
		{
			this.UpdateThresholdValue(this.thresholdSwitch.Threshold + 10f * (float)this.thresholdSwitch.IncrementScale);
			incrementMajorToggle.ChangeState(1);
		};
		incrementMajorToggle.onStopHold = delegate()
		{
			incrementMajorToggle.ChangeState(0);
		};
		MultiToggle decrementMinorToggle = this.decrementMinor.GetComponent<MultiToggle>();
		decrementMinorToggle.onClick = delegate()
		{
			this.UpdateThresholdValue(this.thresholdSwitch.Threshold - (float)this.thresholdSwitch.IncrementScale);
			decrementMinorToggle.ChangeState(1);
		};
		decrementMinorToggle.onStopHold = delegate()
		{
			decrementMinorToggle.ChangeState(0);
		};
		MultiToggle decrementMajorToggle = this.decrementMajor.GetComponent<MultiToggle>();
		decrementMajorToggle.onClick = delegate()
		{
			this.UpdateThresholdValue(this.thresholdSwitch.Threshold - 10f * (float)this.thresholdSwitch.IncrementScale);
			decrementMajorToggle.ChangeState(1);
		};
		decrementMajorToggle.onStopHold = delegate()
		{
			decrementMajorToggle.ChangeState(0);
		};
		this.unitsLabel.text = this.thresholdSwitch.ThresholdValueUnits();
		this.numberInput.minValue = this.thresholdSwitch.GetRangeMinInputField();
		this.numberInput.maxValue = this.thresholdSwitch.GetRangeMaxInputField();
		this.numberInput.Activate();
		this.UpdateTargetThresholdLabel();
		this.OnConditionButtonClicked(this.thresholdSwitch.ActivateAboveThreshold);
	}

	// Token: 0x06006ED5 RID: 28373 RVA: 0x00299F4B File Offset: 0x0029814B
	private void OnThresholdValueChanged(float new_value)
	{
		this.thresholdSwitch.Threshold = new_value;
		this.UpdateTargetThresholdLabel();
	}

	// Token: 0x06006ED6 RID: 28374 RVA: 0x00299F60 File Offset: 0x00298160
	private void OnConditionButtonClicked(bool activate_above_threshold)
	{
		this.thresholdSwitch.ActivateAboveThreshold = activate_above_threshold;
		if (activate_above_threshold)
		{
			this.belowToggle.isOn = true;
			this.aboveToggle.isOn = false;
			this.belowToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
			this.aboveToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
		}
		else
		{
			this.belowToggle.isOn = false;
			this.aboveToggle.isOn = true;
			this.belowToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
			this.aboveToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
		}
		this.UpdateTargetThresholdLabel();
	}

	// Token: 0x06006ED7 RID: 28375 RVA: 0x00299FF8 File Offset: 0x002981F8
	private void UpdateTargetThresholdLabel()
	{
		this.numberInput.SetDisplayValue(this.thresholdSwitch.Format(this.thresholdSwitch.Threshold, false) + this.thresholdSwitch.ThresholdValueUnits());
		if (this.thresholdSwitch.ActivateAboveThreshold)
		{
			this.thresholdSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.thresholdSwitch.AboveToolTip, this.thresholdSwitch.Format(this.thresholdSwitch.Threshold, true)));
			this.thresholdSlider.GetComponentInChildren<ToolTip>().tooltipPositionOffset = new Vector2(0f, 25f);
			return;
		}
		this.thresholdSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.thresholdSwitch.BelowToolTip, this.thresholdSwitch.Format(this.thresholdSwitch.Threshold, true)));
		this.thresholdSlider.GetComponentInChildren<ToolTip>().tooltipPositionOffset = new Vector2(0f, 25f);
	}

	// Token: 0x06006ED8 RID: 28376 RVA: 0x0029A0F6 File Offset: 0x002982F6
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateThresholdValue(this.thresholdSwitch.ProcessedSliderValue(newValue));
	}

	// Token: 0x06006ED9 RID: 28377 RVA: 0x0029A10A File Offset: 0x0029830A
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateThresholdValue(this.thresholdSwitch.ProcessedInputValue(newValue));
	}

	// Token: 0x06006EDA RID: 28378 RVA: 0x0029A120 File Offset: 0x00298320
	private void UpdateThresholdValue(float newValue)
	{
		if (newValue < this.thresholdSwitch.RangeMin)
		{
			newValue = this.thresholdSwitch.RangeMin;
		}
		if (newValue > this.thresholdSwitch.RangeMax)
		{
			newValue = this.thresholdSwitch.RangeMax;
		}
		this.thresholdSwitch.Threshold = newValue;
		NonLinearSlider nonLinearSlider = this.thresholdSlider;
		if (nonLinearSlider != null)
		{
			this.thresholdSlider.value = nonLinearSlider.GetPercentageFromValue(newValue);
		}
		else
		{
			this.thresholdSlider.value = newValue;
		}
		this.UpdateTargetThresholdLabel();
	}

	// Token: 0x06006EDB RID: 28379 RVA: 0x0029A1A5 File Offset: 0x002983A5
	private void UpdateLabels()
	{
		this.currentValue.text = string.Format(UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CURRENT_VALUE, this.thresholdSwitch.ThresholdValueName, this.thresholdSwitch.Format(this.thresholdSwitch.CurrentValue, true));
	}

	// Token: 0x06006EDC RID: 28380 RVA: 0x0029A1E3 File Offset: 0x002983E3
	public override string GetTitle()
	{
		if (this.target != null)
		{
			return this.thresholdSwitch.Title;
		}
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
	}

	// Token: 0x04004B90 RID: 19344
	private GameObject target;

	// Token: 0x04004B91 RID: 19345
	private IThresholdSwitch thresholdSwitch;

	// Token: 0x04004B92 RID: 19346
	[SerializeField]
	private LocText currentValue;

	// Token: 0x04004B93 RID: 19347
	[SerializeField]
	private LocText thresholdValue;

	// Token: 0x04004B94 RID: 19348
	[SerializeField]
	private KToggle aboveToggle;

	// Token: 0x04004B95 RID: 19349
	[SerializeField]
	private KToggle belowToggle;

	// Token: 0x04004B96 RID: 19350
	[Header("Slider")]
	[SerializeField]
	private NonLinearSlider thresholdSlider;

	// Token: 0x04004B97 RID: 19351
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x04004B98 RID: 19352
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04004B99 RID: 19353
	[Header("Increment Buttons")]
	[SerializeField]
	private GameObject incrementMinor;

	// Token: 0x04004B9A RID: 19354
	[SerializeField]
	private GameObject incrementMajor;

	// Token: 0x04004B9B RID: 19355
	[SerializeField]
	private GameObject decrementMinor;

	// Token: 0x04004B9C RID: 19356
	[SerializeField]
	private GameObject decrementMajor;
}
