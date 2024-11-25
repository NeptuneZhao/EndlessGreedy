using System;
using UnityEngine;

// Token: 0x02000DAB RID: 3499
[Serializable]
public class SliderSet
{
	// Token: 0x06006E8C RID: 28300 RVA: 0x002986D0 File Offset: 0x002968D0
	public void SetupSlider(int index)
	{
		this.index = index;
		this.valueSlider.onReleaseHandle += delegate()
		{
			this.valueSlider.value = Mathf.Round(this.valueSlider.value * 10f) / 10f;
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput();
		};
	}

	// Token: 0x06006E8D RID: 28301 RVA: 0x00298758 File Offset: 0x00296958
	public void SetTarget(ISliderControl target, int index)
	{
		this.index = index;
		this.target = target;
		ToolTip component = this.valueSlider.handleRect.GetComponent<ToolTip>();
		if (component != null)
		{
			component.SetSimpleTooltip(target.GetSliderTooltip(index));
		}
		if (this.targetLabel != null)
		{
			this.targetLabel.text = ((target.SliderTitleKey != null) ? Strings.Get(target.SliderTitleKey) : "");
		}
		this.unitsLabel.text = target.SliderUnits;
		this.minLabel.text = target.GetSliderMin(index).ToString() + target.SliderUnits;
		this.maxLabel.text = target.GetSliderMax(index).ToString() + target.SliderUnits;
		this.numberInput.minValue = target.GetSliderMin(index);
		this.numberInput.maxValue = target.GetSliderMax(index);
		this.numberInput.decimalPlaces = target.SliderDecimalPlaces(index);
		this.numberInput.field.characterLimit = Mathf.FloorToInt(1f + Mathf.Log10(this.numberInput.maxValue + (float)this.numberInput.decimalPlaces));
		Vector2 sizeDelta = this.numberInput.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.x = (float)((this.numberInput.field.characterLimit + 1) * 10);
		this.numberInput.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		this.valueSlider.minValue = target.GetSliderMin(index);
		this.valueSlider.maxValue = target.GetSliderMax(index);
		this.valueSlider.value = target.GetSliderValue(index);
		this.SetValue(target.GetSliderValue(index));
		if (index == 0)
		{
			this.numberInput.Activate();
		}
	}

	// Token: 0x06006E8E RID: 28302 RVA: 0x0029892C File Offset: 0x00296B2C
	private void ReceiveValueFromSlider()
	{
		float num = this.valueSlider.value;
		if (this.numberInput.decimalPlaces != -1)
		{
			float num2 = Mathf.Pow(10f, (float)this.numberInput.decimalPlaces);
			num = Mathf.Round(num * num2) / num2;
		}
		this.SetValue(num);
	}

	// Token: 0x06006E8F RID: 28303 RVA: 0x0029897C File Offset: 0x00296B7C
	private void ReceiveValueFromInput()
	{
		float num = this.numberInput.currentValue;
		if (this.numberInput.decimalPlaces != -1)
		{
			float num2 = Mathf.Pow(10f, (float)this.numberInput.decimalPlaces);
			num = Mathf.Round(num * num2) / num2;
		}
		this.valueSlider.value = num;
		this.SetValue(num);
	}

	// Token: 0x06006E90 RID: 28304 RVA: 0x002989D8 File Offset: 0x00296BD8
	private void SetValue(float value)
	{
		float num = value;
		if (num > this.target.GetSliderMax(this.index))
		{
			num = this.target.GetSliderMax(this.index);
		}
		else if (num < this.target.GetSliderMin(this.index))
		{
			num = this.target.GetSliderMin(this.index);
		}
		this.UpdateLabel(num);
		this.target.SetSliderValue(num, this.index);
		ToolTip component = this.valueSlider.handleRect.GetComponent<ToolTip>();
		if (component != null)
		{
			component.SetSimpleTooltip(this.target.GetSliderTooltip(this.index));
		}
	}

	// Token: 0x06006E91 RID: 28305 RVA: 0x00298A80 File Offset: 0x00296C80
	private void UpdateLabel(float value)
	{
		float num = Mathf.Round(value * 10f) / 10f;
		this.numberInput.SetDisplayValue(num.ToString());
	}

	// Token: 0x04004B62 RID: 19298
	public KSlider valueSlider;

	// Token: 0x04004B63 RID: 19299
	public KNumberInputField numberInput;

	// Token: 0x04004B64 RID: 19300
	public LocText targetLabel;

	// Token: 0x04004B65 RID: 19301
	public LocText unitsLabel;

	// Token: 0x04004B66 RID: 19302
	public LocText minLabel;

	// Token: 0x04004B67 RID: 19303
	public LocText maxLabel;

	// Token: 0x04004B68 RID: 19304
	[NonSerialized]
	public int index;

	// Token: 0x04004B69 RID: 19305
	private ISliderControl target;
}
