using System;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DB2 RID: 3506
public class TemperatureSwitchSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x06006EBF RID: 28351 RVA: 0x00299830 File Offset: 0x00297A30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.coolerToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(false);
		};
		this.warmerToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(true);
		};
		LocText component = this.coolerToggle.transform.GetChild(0).GetComponent<LocText>();
		TMP_Text component2 = this.warmerToggle.transform.GetChild(0).GetComponent<LocText>();
		component.SetText(UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.COLDER_BUTTON);
		component2.SetText(UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.WARMER_BUTTON);
		Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
		sliderEvent.AddListener(new UnityAction<float>(this.OnTargetTemperatureChanged));
		this.targetTemperatureSlider.onValueChanged = sliderEvent;
	}

	// Token: 0x06006EC0 RID: 28352 RVA: 0x002998E1 File Offset: 0x00297AE1
	public void Render200ms(float dt)
	{
		if (this.targetTemperatureSwitch == null)
		{
			return;
		}
		this.UpdateLabels();
	}

	// Token: 0x06006EC1 RID: 28353 RVA: 0x002998F8 File Offset: 0x00297AF8
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<TemperatureControlledSwitch>() != null;
	}

	// Token: 0x06006EC2 RID: 28354 RVA: 0x00299908 File Offset: 0x00297B08
	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targetTemperatureSwitch = target.GetComponent<TemperatureControlledSwitch>();
		if (this.targetTemperatureSwitch == null)
		{
			global::Debug.LogError("The gameObject received does not contain a TimedSwitch component");
			return;
		}
		this.UpdateLabels();
		this.UpdateTargetTemperatureLabel();
		this.OnConditionButtonClicked(this.targetTemperatureSwitch.activateOnWarmerThan);
	}

	// Token: 0x06006EC3 RID: 28355 RVA: 0x0029996B File Offset: 0x00297B6B
	private void OnTargetTemperatureChanged(float new_value)
	{
		this.targetTemperatureSwitch.thresholdTemperature = new_value;
		this.UpdateTargetTemperatureLabel();
	}

	// Token: 0x06006EC4 RID: 28356 RVA: 0x00299980 File Offset: 0x00297B80
	private void OnConditionButtonClicked(bool isWarmer)
	{
		this.targetTemperatureSwitch.activateOnWarmerThan = isWarmer;
		if (isWarmer)
		{
			this.coolerToggle.isOn = false;
			this.warmerToggle.isOn = true;
			this.coolerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
			this.warmerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
			return;
		}
		this.coolerToggle.isOn = true;
		this.warmerToggle.isOn = false;
		this.coolerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
		this.warmerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
	}

	// Token: 0x06006EC5 RID: 28357 RVA: 0x00299A11 File Offset: 0x00297C11
	private void UpdateTargetTemperatureLabel()
	{
		this.targetTemperature.text = GameUtil.GetFormattedTemperature(this.targetTemperatureSwitch.thresholdTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
	}

	// Token: 0x06006EC6 RID: 28358 RVA: 0x00299A32 File Offset: 0x00297C32
	private void UpdateLabels()
	{
		this.currentTemperature.text = string.Format(UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.CURRENT_TEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperatureSwitch.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	}

	// Token: 0x04004B89 RID: 19337
	private TemperatureControlledSwitch targetTemperatureSwitch;

	// Token: 0x04004B8A RID: 19338
	[SerializeField]
	private LocText currentTemperature;

	// Token: 0x04004B8B RID: 19339
	[SerializeField]
	private LocText targetTemperature;

	// Token: 0x04004B8C RID: 19340
	[SerializeField]
	private KToggle coolerToggle;

	// Token: 0x04004B8D RID: 19341
	[SerializeField]
	private KToggle warmerToggle;

	// Token: 0x04004B8E RID: 19342
	[SerializeField]
	private KSlider targetTemperatureSlider;
}
