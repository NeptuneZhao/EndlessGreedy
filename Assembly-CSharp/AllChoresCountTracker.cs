using System;

// Token: 0x020005D2 RID: 1490
public class AllChoresCountTracker : WorldTracker
{
	// Token: 0x06002458 RID: 9304 RVA: 0x000CAF42 File Offset: 0x000C9142
	public AllChoresCountTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002459 RID: 9305 RVA: 0x000CAF4C File Offset: 0x000C914C
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			Tracker choreGroupTracker = TrackerTool.Instance.GetChoreGroupTracker(base.WorldID, Db.Get().ChoreGroups[i]);
			num += ((choreGroupTracker == null) ? 0f : choreGroupTracker.GetCurrentValue());
		}
		base.AddPoint(num);
	}

	// Token: 0x0600245A RID: 9306 RVA: 0x000CAFB4 File Offset: 0x000C91B4
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
