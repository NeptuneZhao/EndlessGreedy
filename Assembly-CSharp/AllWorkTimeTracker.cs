using System;

// Token: 0x020005D4 RID: 1492
public class AllWorkTimeTracker : WorldTracker
{
	// Token: 0x0600245E RID: 9310 RVA: 0x000CB116 File Offset: 0x000C9316
	public AllWorkTimeTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600245F RID: 9311 RVA: 0x000CB120 File Offset: 0x000C9320
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			num += TrackerTool.Instance.GetWorkTimeTracker(base.WorldID, Db.Get().ChoreGroups[i]).GetCurrentValue();
		}
		base.AddPoint(num);
	}

	// Token: 0x06002460 RID: 9312 RVA: 0x000CB17C File Offset: 0x000C937C
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedPercent(value, GameUtil.TimeSlice.None).ToString();
	}
}
