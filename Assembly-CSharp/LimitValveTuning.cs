using System;

// Token: 0x0200024E RID: 590
public class LimitValveTuning
{
	// Token: 0x06000C39 RID: 3129 RVA: 0x00047932 File Offset: 0x00045B32
	public static NonLinearSlider.Range[] GetDefaultSlider()
	{
		return new NonLinearSlider.Range[]
		{
			new NonLinearSlider.Range(70f, 100f),
			new NonLinearSlider.Range(30f, 500f)
		};
	}

	// Token: 0x040007E3 RID: 2019
	public const float MAX_LIMIT = 500f;

	// Token: 0x040007E4 RID: 2020
	public const float DEFAULT_LIMIT = 100f;
}
