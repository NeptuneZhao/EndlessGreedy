using System;

// Token: 0x020005D9 RID: 1497
public class ResourceTracker : WorldTracker
{
	// Token: 0x170001AD RID: 429
	// (get) Token: 0x0600246D RID: 9325 RVA: 0x000CB440 File Offset: 0x000C9640
	// (set) Token: 0x0600246E RID: 9326 RVA: 0x000CB448 File Offset: 0x000C9648
	public Tag tag { get; private set; }

	// Token: 0x0600246F RID: 9327 RVA: 0x000CB451 File Offset: 0x000C9651
	public ResourceTracker(int worldID, Tag materialCategoryTag) : base(worldID)
	{
		this.tag = materialCategoryTag;
	}

	// Token: 0x06002470 RID: 9328 RVA: 0x000CB464 File Offset: 0x000C9664
	public override void UpdateData()
	{
		if (ClusterManager.Instance.GetWorld(base.WorldID).worldInventory == null)
		{
			return;
		}
		base.AddPoint(ClusterManager.Instance.GetWorld(base.WorldID).worldInventory.GetAmount(this.tag, false));
	}

	// Token: 0x06002471 RID: 9329 RVA: 0x000CB4B6 File Offset: 0x000C96B6
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
