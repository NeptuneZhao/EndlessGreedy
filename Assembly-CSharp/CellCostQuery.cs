using System;

// Token: 0x02000495 RID: 1173
public class CellCostQuery : PathFinderQuery
{
	// Token: 0x1700009E RID: 158
	// (get) Token: 0x0600195F RID: 6495 RVA: 0x00087E1A File Offset: 0x0008601A
	// (set) Token: 0x06001960 RID: 6496 RVA: 0x00087E22 File Offset: 0x00086022
	public int resultCost { get; private set; }

	// Token: 0x06001961 RID: 6497 RVA: 0x00087E2B File Offset: 0x0008602B
	public void Reset(int target_cell, int max_cost)
	{
		this.targetCell = target_cell;
		this.maxCost = max_cost;
		this.resultCost = -1;
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x00087E42 File Offset: 0x00086042
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (cost > this.maxCost)
		{
			return true;
		}
		if (cell == this.targetCell)
		{
			this.resultCost = cost;
			return true;
		}
		return false;
	}

	// Token: 0x04000E56 RID: 3670
	private int targetCell;

	// Token: 0x04000E57 RID: 3671
	private int maxCost;
}
