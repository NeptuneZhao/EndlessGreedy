using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B92 RID: 2962
public class DisinfectThresholdDiagram : MonoBehaviour
{
	// Token: 0x06005960 RID: 22880 RVA: 0x00205634 File Offset: 0x00203834
	private void Start()
	{
		this.inputField.minValue = 0f;
		this.inputField.maxValue = (float)DisinfectThresholdDiagram.MAX_VALUE;
		this.inputField.currentValue = (float)SaveGame.Instance.minGermCountForDisinfect;
		this.inputField.SetDisplayValue(SaveGame.Instance.minGermCountForDisinfect.ToString());
		this.inputField.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.inputField.currentValue);
		};
		this.inputField.decimalPlaces = 1;
		this.inputField.Activate();
		this.slider.minValue = 0f;
		this.slider.maxValue = (float)(DisinfectThresholdDiagram.MAX_VALUE / DisinfectThresholdDiagram.SLIDER_CONVERSION);
		this.slider.wholeNumbers = true;
		this.slider.value = (float)(SaveGame.Instance.minGermCountForDisinfect / DisinfectThresholdDiagram.SLIDER_CONVERSION);
		this.slider.onReleaseHandle += this.OnReleaseHandle;
		this.slider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
			this.OnReleaseHandle();
		};
		this.unitsLabel.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.UNITS);
		this.minLabel.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.MIN_LABEL);
		this.maxLabel.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.MAX_LABEL);
		this.thresholdPrefix.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.THRESHOLD_PREFIX);
		this.toolTip.OnToolTip = delegate()
		{
			this.toolTip.ClearMultiStringTooltip();
			if (SaveGame.Instance.enableAutoDisinfect)
			{
				this.toolTip.AddMultiStringTooltip(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.TOOLTIP.ToString().Replace("{NumberOfGerms}", SaveGame.Instance.minGermCountForDisinfect.ToString()), null);
			}
			else
			{
				this.toolTip.AddMultiStringTooltip(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.TOOLTIP_DISABLED.ToString(), null);
			}
			return "";
		};
		this.disabledImage.gameObject.SetActive(!SaveGame.Instance.enableAutoDisinfect);
		this.toggle.isOn = SaveGame.Instance.enableAutoDisinfect;
		this.toggle.onValueChanged += this.OnClickToggle;
	}

	// Token: 0x06005961 RID: 22881 RVA: 0x00205820 File Offset: 0x00203A20
	private void OnReleaseHandle()
	{
		float num = (float)((int)this.slider.value * DisinfectThresholdDiagram.SLIDER_CONVERSION);
		SaveGame.Instance.minGermCountForDisinfect = (int)num;
		this.inputField.SetDisplayValue(num.ToString());
	}

	// Token: 0x06005962 RID: 22882 RVA: 0x00205860 File Offset: 0x00203A60
	private void ReceiveValueFromSlider(float new_value)
	{
		SaveGame.Instance.minGermCountForDisinfect = (int)new_value * DisinfectThresholdDiagram.SLIDER_CONVERSION;
		this.inputField.SetDisplayValue((new_value * (float)DisinfectThresholdDiagram.SLIDER_CONVERSION).ToString());
	}

	// Token: 0x06005963 RID: 22883 RVA: 0x0020589A File Offset: 0x00203A9A
	private void ReceiveValueFromInput(float new_value)
	{
		this.slider.value = new_value / (float)DisinfectThresholdDiagram.SLIDER_CONVERSION;
		SaveGame.Instance.minGermCountForDisinfect = (int)new_value;
	}

	// Token: 0x06005964 RID: 22884 RVA: 0x002058BB File Offset: 0x00203ABB
	private void OnClickToggle(bool new_value)
	{
		SaveGame.Instance.enableAutoDisinfect = new_value;
		this.disabledImage.gameObject.SetActive(!SaveGame.Instance.enableAutoDisinfect);
	}

	// Token: 0x04003ABD RID: 15037
	[SerializeField]
	private KNumberInputField inputField;

	// Token: 0x04003ABE RID: 15038
	[SerializeField]
	private KSlider slider;

	// Token: 0x04003ABF RID: 15039
	[SerializeField]
	private LocText minLabel;

	// Token: 0x04003AC0 RID: 15040
	[SerializeField]
	private LocText maxLabel;

	// Token: 0x04003AC1 RID: 15041
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04003AC2 RID: 15042
	[SerializeField]
	private LocText thresholdPrefix;

	// Token: 0x04003AC3 RID: 15043
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x04003AC4 RID: 15044
	[SerializeField]
	private KToggle toggle;

	// Token: 0x04003AC5 RID: 15045
	[SerializeField]
	private Image disabledImage;

	// Token: 0x04003AC6 RID: 15046
	private static int MAX_VALUE = 1000000;

	// Token: 0x04003AC7 RID: 15047
	private static int SLIDER_CONVERSION = 1000;
}
