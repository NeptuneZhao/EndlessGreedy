using System;

// Token: 0x020005E1 RID: 1505
public class RocketFuelTracker : WorldTracker
{
	// Token: 0x06002487 RID: 9351 RVA: 0x000CB925 File Offset: 0x000C9B25
	public RocketFuelTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002488 RID: 9352 RVA: 0x000CB930 File Offset: 0x000C9B30
	public override void UpdateData()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.WorldID).GetComponent<Clustercraft>();
		base.AddPoint((component != null) ? component.ModuleInterface.FuelRemaining : 0f);
	}

	// Token: 0x06002489 RID: 9353 RVA: 0x000CB974 File Offset: 0x000C9B74
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
