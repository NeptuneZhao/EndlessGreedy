using System;

// Token: 0x020002FF RID: 767
public static class MathfExtensions
{
	// Token: 0x06001027 RID: 4135 RVA: 0x0005B749 File Offset: 0x00059949
	public static long Max(this long a, long b)
	{
		if (a < b)
		{
			return b;
		}
		return a;
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x0005B752 File Offset: 0x00059952
	public static long Min(this long a, long b)
	{
		if (a > b)
		{
			return b;
		}
		return a;
	}
}
