using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000DC3 RID: 3523
[AddComponentMenu("KMonoBehaviour/scripts/SliderContainer")]
public class SliderContainer : KMonoBehaviour
{
	// Token: 0x06006FCE RID: 28622 RVA: 0x002A2916 File Offset: 0x002A0B16
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.UpdateSliderLabel));
	}

	// Token: 0x06006FCF RID: 28623 RVA: 0x002A293C File Offset: 0x002A0B3C
	public void UpdateSliderLabel(float newValue)
	{
		if (this.isPercentValue)
		{
			this.valueLabel.text = (newValue * 100f).ToString("F0") + "%";
			return;
		}
		this.valueLabel.text = newValue.ToString();
	}

	// Token: 0x04004C91 RID: 19601
	public bool isPercentValue = true;

	// Token: 0x04004C92 RID: 19602
	public KSlider slider;

	// Token: 0x04004C93 RID: 19603
	public LocText nameLabel;

	// Token: 0x04004C94 RID: 19604
	public LocText valueLabel;
}
