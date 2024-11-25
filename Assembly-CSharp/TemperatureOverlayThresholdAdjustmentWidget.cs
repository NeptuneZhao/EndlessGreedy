using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000BB7 RID: 2999
public class TemperatureOverlayThresholdAdjustmentWidget : KMonoBehaviour
{
	// Token: 0x06005B02 RID: 23298 RVA: 0x002118D5 File Offset: 0x0020FAD5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.scrollbar.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x06005B03 RID: 23299 RVA: 0x002118FC File Offset: 0x0020FAFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.scrollbar.size = TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange;
		this.scrollbar.value = this.KelvinToScrollPercentage(SaveGame.Instance.relativeTemperatureOverlaySliderValue);
		this.defaultButton.onClick += this.OnDefaultPressed;
	}

	// Token: 0x06005B04 RID: 23300 RVA: 0x00211957 File Offset: 0x0020FB57
	private void OnValueChanged(float data)
	{
		this.SetUserConfig(data);
	}

	// Token: 0x06005B05 RID: 23301 RVA: 0x00211960 File Offset: 0x0020FB60
	private float KelvinToScrollPercentage(float kelvin)
	{
		kelvin -= TemperatureOverlayThresholdAdjustmentWidget.minimumSelectionTemperature;
		if (kelvin < 1f)
		{
			kelvin = 1f;
		}
		return Mathf.Clamp01(kelvin / TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange);
	}

	// Token: 0x06005B06 RID: 23302 RVA: 0x00211988 File Offset: 0x0020FB88
	private void SetUserConfig(float scrollPercentage)
	{
		float num = TemperatureOverlayThresholdAdjustmentWidget.minimumSelectionTemperature + TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange * scrollPercentage;
		float num2 = num - TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
		float num3 = num + TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
		SimDebugView.Instance.user_temperatureThresholds[0] = num2;
		SimDebugView.Instance.user_temperatureThresholds[1] = num3;
		this.scrollBarRangeCenterText.SetText(GameUtil.GetFormattedTemperature(num, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true));
		this.scrollBarRangeLowText.SetText(GameUtil.GetFormattedTemperature((float)Mathf.RoundToInt(num2), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true));
		this.scrollBarRangeHighText.SetText(GameUtil.GetFormattedTemperature((float)Mathf.RoundToInt(num3), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true));
		SaveGame.Instance.relativeTemperatureOverlaySliderValue = num;
	}

	// Token: 0x06005B07 RID: 23303 RVA: 0x00211A37 File Offset: 0x0020FC37
	private void OnDefaultPressed()
	{
		this.scrollbar.value = this.KelvinToScrollPercentage(294.15f);
	}

	// Token: 0x04003BEE RID: 15342
	public const float DEFAULT_TEMPERATURE = 294.15f;

	// Token: 0x04003BEF RID: 15343
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x04003BF0 RID: 15344
	[SerializeField]
	private LocText scrollBarRangeLowText;

	// Token: 0x04003BF1 RID: 15345
	[SerializeField]
	private LocText scrollBarRangeCenterText;

	// Token: 0x04003BF2 RID: 15346
	[SerializeField]
	private LocText scrollBarRangeHighText;

	// Token: 0x04003BF3 RID: 15347
	[SerializeField]
	private KButton defaultButton;

	// Token: 0x04003BF4 RID: 15348
	private static float maxTemperatureRange = 700f;

	// Token: 0x04003BF5 RID: 15349
	private static float temperatureWindowSize = 200f;

	// Token: 0x04003BF6 RID: 15350
	private static float minimumSelectionTemperature = TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
}
