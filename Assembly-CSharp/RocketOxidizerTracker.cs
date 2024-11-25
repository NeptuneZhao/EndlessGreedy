using System;

// Token: 0x020005E2 RID: 1506
public class RocketOxidizerTracker : WorldTracker
{
	// Token: 0x0600248A RID: 9354 RVA: 0x000CB984 File Offset: 0x000C9B84
	public RocketOxidizerTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600248B RID: 9355 RVA: 0x000CB990 File Offset: 0x000C9B90
	public override void UpdateData()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.WorldID).GetComponent<Clustercraft>();
		base.AddPoint((component != null) ? component.ModuleInterface.OxidizerPowerRemaining : 0f);
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x000CB9D4 File Offset: 0x000C9BD4
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
