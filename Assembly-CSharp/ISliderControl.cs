using System;

// Token: 0x02000DAA RID: 3498
public interface ISliderControl
{
	// Token: 0x170007C4 RID: 1988
	// (get) Token: 0x06006E83 RID: 28291
	string SliderTitleKey { get; }

	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x06006E84 RID: 28292
	string SliderUnits { get; }

	// Token: 0x06006E85 RID: 28293
	int SliderDecimalPlaces(int index);

	// Token: 0x06006E86 RID: 28294
	float GetSliderMin(int index);

	// Token: 0x06006E87 RID: 28295
	float GetSliderMax(int index);

	// Token: 0x06006E88 RID: 28296
	float GetSliderValue(int index);

	// Token: 0x06006E89 RID: 28297
	void SetSliderValue(float percent, int index);

	// Token: 0x06006E8A RID: 28298
	string GetSliderTooltipKey(int index);

	// Token: 0x06006E8B RID: 28299
	string GetSliderTooltip(int index);
}
