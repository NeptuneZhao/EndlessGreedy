using System;

// Token: 0x02000D81 RID: 3457
public interface IMultiSliderControl
{
	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x06006CCC RID: 27852
	string SidescreenTitleKey { get; }

	// Token: 0x06006CCD RID: 27853
	bool SidescreenEnabled();

	// Token: 0x1700079F RID: 1951
	// (get) Token: 0x06006CCE RID: 27854
	ISliderControl[] sliderControls { get; }
}
