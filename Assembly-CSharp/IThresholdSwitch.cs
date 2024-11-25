using System;

// Token: 0x0200077C RID: 1916
public interface IThresholdSwitch
{
	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06003404 RID: 13316
	// (set) Token: 0x06003405 RID: 13317
	float Threshold { get; set; }

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06003406 RID: 13318
	// (set) Token: 0x06003407 RID: 13319
	bool ActivateAboveThreshold { get; set; }

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06003408 RID: 13320
	float CurrentValue { get; }

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06003409 RID: 13321
	float RangeMin { get; }

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x0600340A RID: 13322
	float RangeMax { get; }

	// Token: 0x0600340B RID: 13323
	float GetRangeMinInputField();

	// Token: 0x0600340C RID: 13324
	float GetRangeMaxInputField();

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x0600340D RID: 13325
	LocString Title { get; }

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x0600340E RID: 13326
	LocString ThresholdValueName { get; }

	// Token: 0x0600340F RID: 13327
	LocString ThresholdValueUnits();

	// Token: 0x06003410 RID: 13328
	string Format(float value, bool units);

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06003411 RID: 13329
	string AboveToolTip { get; }

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06003412 RID: 13330
	string BelowToolTip { get; }

	// Token: 0x06003413 RID: 13331
	float ProcessedSliderValue(float input);

	// Token: 0x06003414 RID: 13332
	float ProcessedInputValue(float input);

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x06003415 RID: 13333
	ThresholdScreenLayoutType LayoutType { get; }

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x06003416 RID: 13334
	int IncrementScale { get; }

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x06003417 RID: 13335
	NonLinearSlider.Range[] GetRanges { get; }
}
