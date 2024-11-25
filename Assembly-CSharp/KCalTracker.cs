using System;

// Token: 0x020005DD RID: 1501
public class KCalTracker : WorldTracker
{
	// Token: 0x0600247B RID: 9339 RVA: 0x000CB75C File Offset: 0x000C995C
	public KCalTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600247C RID: 9340 RVA: 0x000CB765 File Offset: 0x000C9965
	public override void UpdateData()
	{
		base.AddPoint(WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, ClusterManager.Instance.GetWorld(base.WorldID).worldInventory, true));
	}

	// Token: 0x0600247D RID: 9341 RVA: 0x000CB78E File Offset: 0x000C998E
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedCalories(value, GameUtil.TimeSlice.None, true);
	}
}
