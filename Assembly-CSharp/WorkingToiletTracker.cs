using System;
using System.Collections.Generic;

// Token: 0x020005E3 RID: 1507
public class WorkingToiletTracker : WorldTracker
{
	// Token: 0x0600248D RID: 9357 RVA: 0x000CB9E4 File Offset: 0x000C9BE4
	public WorkingToiletTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600248E RID: 9358 RVA: 0x000CB9F0 File Offset: 0x000C9BF0
	public override void UpdateData()
	{
		int num = 0;
		using (IEnumerator<IUsable> enumerator = Components.Toilets.WorldItemsEnumerate(base.WorldID, true).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsUsable())
				{
					num++;
				}
			}
		}
		base.AddPoint((float)num);
	}

	// Token: 0x0600248F RID: 9359 RVA: 0x000CBA58 File Offset: 0x000C9C58
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
