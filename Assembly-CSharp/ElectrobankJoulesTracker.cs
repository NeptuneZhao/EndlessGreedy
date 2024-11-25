using System;

// Token: 0x020005DE RID: 1502
public class ElectrobankJoulesTracker : WorldTracker
{
	// Token: 0x0600247E RID: 9342 RVA: 0x000CB798 File Offset: 0x000C9998
	public ElectrobankJoulesTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600247F RID: 9343 RVA: 0x000CB7A1 File Offset: 0x000C99A1
	public override void UpdateData()
	{
		base.AddPoint(WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(null, ClusterManager.Instance.GetWorld(base.WorldID).worldInventory, true));
	}

	// Token: 0x06002480 RID: 9344 RVA: 0x000CB7CA File Offset: 0x000C99CA
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedJoules(value, "F1", GameUtil.TimeSlice.None);
	}
}
