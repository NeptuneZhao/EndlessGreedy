using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D5 RID: 1493
public class WorkTimeTracker : WorldTracker
{
	// Token: 0x06002461 RID: 9313 RVA: 0x000CB18A File Offset: 0x000C938A
	public WorkTimeTracker(int worldID, ChoreGroup group) : base(worldID)
	{
		this.choreGroup = group;
	}

	// Token: 0x06002462 RID: 9314 RVA: 0x000CB19C File Offset: 0x000C939C
	public override void UpdateData()
	{
		float num = 0f;
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false);
		Chore chore;
		Predicate<ChoreType> <>9__0;
		foreach (MinionIdentity minionIdentity in worldItems)
		{
			chore = minionIdentity.GetComponent<ChoreConsumer>().choreDriver.GetCurrentChore();
			if (chore != null)
			{
				List<ChoreType> choreTypes = this.choreGroup.choreTypes;
				Predicate<ChoreType> match2;
				if ((match2 = <>9__0) == null)
				{
					match2 = (<>9__0 = ((ChoreType match) => match == chore.choreType));
				}
				if (choreTypes.Find(match2) != null)
				{
					num += 1f;
				}
			}
		}
		base.AddPoint(num / (float)worldItems.Count * 100f);
	}

	// Token: 0x06002463 RID: 9315 RVA: 0x000CB274 File Offset: 0x000C9474
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedPercent(Mathf.Round(value), GameUtil.TimeSlice.None).ToString();
	}

	// Token: 0x040014B4 RID: 5300
	public ChoreGroup choreGroup;
}
