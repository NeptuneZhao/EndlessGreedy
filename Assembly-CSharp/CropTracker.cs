using System;

// Token: 0x020005D6 RID: 1494
public class CropTracker : WorldTracker
{
	// Token: 0x06002464 RID: 9316 RVA: 0x000CB287 File Offset: 0x000C9487
	public CropTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002465 RID: 9317 RVA: 0x000CB290 File Offset: 0x000C9490
	public override void UpdateData()
	{
		float num = 0f;
		foreach (PlantablePlot plantablePlot in Components.PlantablePlots.GetItems(base.WorldID))
		{
			if (!(plantablePlot.plant == null) && plantablePlot.HasDepositTag(GameTags.CropSeed) && !plantablePlot.plant.HasTag(GameTags.Wilting))
			{
				num += 1f;
			}
		}
		base.AddPoint(num);
	}

	// Token: 0x06002466 RID: 9318 RVA: 0x000CB328 File Offset: 0x000C9528
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
