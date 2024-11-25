using System;

// Token: 0x020004A0 RID: 1184
public class SafetyQuery : PathFinderQuery
{
	// Token: 0x06001986 RID: 6534 RVA: 0x0008884F File Offset: 0x00086A4F
	public SafetyQuery(SafetyChecker checker, KMonoBehaviour cmp, int max_cost)
	{
		this.checker = checker;
		this.cmp = cmp;
		this.maxCost = max_cost;
	}

	// Token: 0x06001987 RID: 6535 RVA: 0x0008886C File Offset: 0x00086A6C
	public void Reset()
	{
		this.targetCell = PathFinder.InvalidCell;
		this.targetCost = int.MaxValue;
		this.targetConditions = 0;
		this.context = new SafetyChecker.Context(this.cmp);
	}

	// Token: 0x06001988 RID: 6536 RVA: 0x0008889C File Offset: 0x00086A9C
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		bool flag = false;
		int safetyConditions = this.checker.GetSafetyConditions(cell, cost, this.context, out flag);
		if (safetyConditions != 0 && (safetyConditions > this.targetConditions || (safetyConditions == this.targetConditions && cost < this.targetCost)))
		{
			this.targetCell = cell;
			this.targetConditions = safetyConditions;
			this.targetCost = cost;
			if (flag)
			{
				return true;
			}
		}
		return cost >= this.maxCost;
	}

	// Token: 0x06001989 RID: 6537 RVA: 0x00088905 File Offset: 0x00086B05
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04000E7E RID: 3710
	private int targetCell;

	// Token: 0x04000E7F RID: 3711
	private int targetCost;

	// Token: 0x04000E80 RID: 3712
	private int targetConditions;

	// Token: 0x04000E81 RID: 3713
	private int maxCost;

	// Token: 0x04000E82 RID: 3714
	private SafetyChecker checker;

	// Token: 0x04000E83 RID: 3715
	private KMonoBehaviour cmp;

	// Token: 0x04000E84 RID: 3716
	private SafetyChecker.Context context;
}
