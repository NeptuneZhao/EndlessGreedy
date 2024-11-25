using System;

// Token: 0x02000C52 RID: 3154
[Serializable]
public struct GraphAxis
{
	// Token: 0x17000734 RID: 1844
	// (get) Token: 0x06006101 RID: 24833 RVA: 0x00241C78 File Offset: 0x0023FE78
	public float range
	{
		get
		{
			return this.max_value - this.min_value;
		}
	}

	// Token: 0x040041AB RID: 16811
	public string name;

	// Token: 0x040041AC RID: 16812
	public float min_value;

	// Token: 0x040041AD RID: 16813
	public float max_value;

	// Token: 0x040041AE RID: 16814
	public float guide_frequency;
}
