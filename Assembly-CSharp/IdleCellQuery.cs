using System;

// Token: 0x0200049A RID: 1178
public class IdleCellQuery : PathFinderQuery
{
	// Token: 0x06001970 RID: 6512 RVA: 0x00088057 File Offset: 0x00086257
	public IdleCellQuery Reset(MinionBrain brain, int max_cost)
	{
		this.brain = brain;
		this.maxCost = max_cost;
		this.targetCell = Grid.InvalidCell;
		return this;
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x00088074 File Offset: 0x00086274
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, false);
		if ((flags & SafeCellQuery.SafeFlags.IsClear) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotLadder) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotTube) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsBreathable) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotLiquid) != (SafeCellQuery.SafeFlags)0)
		{
			this.targetCell = cell;
		}
		return cost > this.maxCost;
	}

	// Token: 0x06001972 RID: 6514 RVA: 0x000880BD File Offset: 0x000862BD
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04000E5D RID: 3677
	private MinionBrain brain;

	// Token: 0x04000E5E RID: 3678
	private int targetCell;

	// Token: 0x04000E5F RID: 3679
	private int maxCost;
}
