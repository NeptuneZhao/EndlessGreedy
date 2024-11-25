using System;
using System.Collections.Generic;

// Token: 0x020005D3 RID: 1491
public class ChoreCountTracker : WorldTracker
{
	// Token: 0x0600245B RID: 9307 RVA: 0x000CAFBD File Offset: 0x000C91BD
	public ChoreCountTracker(int worldID, ChoreGroup group) : base(worldID)
	{
		this.choreGroup = group;
	}

	// Token: 0x0600245C RID: 9308 RVA: 0x000CAFD0 File Offset: 0x000C91D0
	public override void UpdateData()
	{
		float num = 0f;
		List<Chore> list;
		GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(base.WorldID, out list);
		int num2 = 0;
		while (list != null && num2 < list.Count)
		{
			Chore chore = list[num2];
			if (chore != null && !chore.target.Equals(null) && !(chore.gameObject == null))
			{
				ChoreGroup[] groups = chore.choreType.groups;
				for (int i = 0; i < groups.Length; i++)
				{
					if (groups[i] == this.choreGroup)
					{
						num += 1f;
						break;
					}
				}
			}
			num2++;
		}
		List<FetchChore> list2;
		GlobalChoreProvider.Instance.fetchMap.TryGetValue(base.WorldID, out list2);
		int num3 = 0;
		while (list2 != null && num3 < list2.Count)
		{
			Chore chore2 = list2[num3];
			if (chore2 != null && !chore2.target.Equals(null) && !(chore2.gameObject == null))
			{
				ChoreGroup[] groups2 = chore2.choreType.groups;
				for (int j = 0; j < groups2.Length; j++)
				{
					if (groups2[j] == this.choreGroup)
					{
						num += 1f;
						break;
					}
				}
			}
			num3++;
		}
		base.AddPoint(num);
	}

	// Token: 0x0600245D RID: 9309 RVA: 0x000CB10D File Offset: 0x000C930D
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}

	// Token: 0x040014B3 RID: 5299
	public ChoreGroup choreGroup;
}
