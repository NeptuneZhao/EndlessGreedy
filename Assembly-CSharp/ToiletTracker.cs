using System;

// Token: 0x020005D8 RID: 1496
public class ToiletTracker : WorldTracker
{
	// Token: 0x0600246A RID: 9322 RVA: 0x000CB427 File Offset: 0x000C9627
	public ToiletTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600246B RID: 9323 RVA: 0x000CB430 File Offset: 0x000C9630
	public override void UpdateData()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600246C RID: 9324 RVA: 0x000CB437 File Offset: 0x000C9637
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
