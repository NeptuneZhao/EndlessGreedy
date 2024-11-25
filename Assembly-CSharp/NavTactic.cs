using System;
using UnityEngine;

// Token: 0x02000593 RID: 1427
public class NavTactic
{
	// Token: 0x06002167 RID: 8551 RVA: 0x000BAF8A File Offset: 0x000B918A
	public NavTactic(int preferredRange, int rangePenalty = 1, int overlapPenalty = 1, int pathCostPenalty = 1)
	{
		this._overlapPenalty = overlapPenalty;
		this._preferredRange = preferredRange;
		this._rangePenalty = rangePenalty;
		this._pathCostPenalty = pathCostPenalty;
	}

	// Token: 0x06002168 RID: 8552 RVA: 0x000BAFC4 File Offset: 0x000B91C4
	public int GetCellPreferences(int root, CellOffset[] offsets, Navigator navigator)
	{
		int result = NavigationReservations.InvalidReservation;
		int num = int.MaxValue;
		for (int i = 0; i < offsets.Length; i++)
		{
			int num2 = Grid.OffsetCell(root, offsets[i]);
			int num3 = 0;
			num3 += this._overlapPenalty * NavigationReservations.Instance.GetOccupancyCount(num2);
			num3 += this._rangePenalty * Mathf.Abs(this._preferredRange - Grid.GetCellDistance(root, num2));
			num3 += this._pathCostPenalty * Mathf.Max(navigator.GetNavigationCost(num2), 0);
			if (num3 < num && navigator.CanReach(num2))
			{
				num = num3;
				result = num2;
			}
		}
		return result;
	}

	// Token: 0x040012B9 RID: 4793
	private int _overlapPenalty = 3;

	// Token: 0x040012BA RID: 4794
	private int _preferredRange;

	// Token: 0x040012BB RID: 4795
	private int _rangePenalty = 2;

	// Token: 0x040012BC RID: 4796
	private int _pathCostPenalty = 1;
}
