using System;

// Token: 0x02000494 RID: 1172
public class CellArrayQuery : PathFinderQuery
{
	// Token: 0x0600195C RID: 6492 RVA: 0x00087DD9 File Offset: 0x00085FD9
	public CellArrayQuery Reset(int[] target_cells)
	{
		this.targetCells = target_cells;
		return this;
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x00087DE4 File Offset: 0x00085FE4
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		for (int i = 0; i < this.targetCells.Length; i++)
		{
			if (this.targetCells[i] == cell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000E55 RID: 3669
	private int[] targetCells;
}
