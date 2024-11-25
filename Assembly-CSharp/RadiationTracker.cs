using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x020005E0 RID: 1504
public class RadiationTracker : WorldTracker
{
	// Token: 0x06002484 RID: 9348 RVA: 0x000CB860 File Offset: 0x000C9A60
	public RadiationTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x000CB86C File Offset: 0x000C9A6C
	public override void UpdateData()
	{
		float num = 0f;
		List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(base.WorldID, false);
		if (worldItems.Count == 0)
		{
			base.AddPoint(0f);
			return;
		}
		foreach (MinionIdentity cmp in worldItems)
		{
			num += cmp.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).value;
		}
		float value = num / (float)worldItems.Count;
		base.AddPoint(value);
	}

	// Token: 0x06002486 RID: 9350 RVA: 0x000CB91C File Offset: 0x000C9B1C
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedRads(value, GameUtil.TimeSlice.None);
	}
}
