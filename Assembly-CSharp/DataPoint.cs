using System;

// Token: 0x020005E7 RID: 1511
public struct DataPoint
{
	// Token: 0x060024A1 RID: 9377 RVA: 0x000CBFEA File Offset: 0x000CA1EA
	public DataPoint(float start, float end, float value)
	{
		this.periodStart = start;
		this.periodEnd = end;
		this.periodValue = value;
	}

	// Token: 0x040014BD RID: 5309
	public float periodStart;

	// Token: 0x040014BE RID: 5310
	public float periodEnd;

	// Token: 0x040014BF RID: 5311
	public float periodValue;
}
