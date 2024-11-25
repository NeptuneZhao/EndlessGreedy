using System;

// Token: 0x02000D42 RID: 3394
public interface IActivationRangeTarget
{
	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x06006AB1 RID: 27313
	// (set) Token: 0x06006AB2 RID: 27314
	float ActivateValue { get; set; }

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x06006AB3 RID: 27315
	// (set) Token: 0x06006AB4 RID: 27316
	float DeactivateValue { get; set; }

	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x06006AB5 RID: 27317
	float MinValue { get; }

	// Token: 0x1700077A RID: 1914
	// (get) Token: 0x06006AB6 RID: 27318
	float MaxValue { get; }

	// Token: 0x1700077B RID: 1915
	// (get) Token: 0x06006AB7 RID: 27319
	bool UseWholeNumbers { get; }

	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x06006AB8 RID: 27320
	string ActivationRangeTitleText { get; }

	// Token: 0x1700077D RID: 1917
	// (get) Token: 0x06006AB9 RID: 27321
	string ActivateSliderLabelText { get; }

	// Token: 0x1700077E RID: 1918
	// (get) Token: 0x06006ABA RID: 27322
	string DeactivateSliderLabelText { get; }

	// Token: 0x1700077F RID: 1919
	// (get) Token: 0x06006ABB RID: 27323
	string ActivateTooltip { get; }

	// Token: 0x17000780 RID: 1920
	// (get) Token: 0x06006ABC RID: 27324
	string DeactivateTooltip { get; }
}
