using System;
using System.Collections.Generic;

// Token: 0x0200049C RID: 1180
public class MineableCellQuery : PathFinderQuery
{
	// Token: 0x06001977 RID: 6519 RVA: 0x00088144 File Offset: 0x00086344
	public MineableCellQuery Reset(Tag element, int max_results)
	{
		this.element = element;
		this.max_results = max_results;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x00088160 File Offset: 0x00086360
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidMineCell(this.element, cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x06001979 RID: 6521 RVA: 0x000881AC File Offset: 0x000863AC
	private bool CheckValidMineCell(Tag element, int testCell)
	{
		if (!Grid.IsValidCell(testCell))
		{
			return false;
		}
		foreach (Direction d in MineableCellQuery.DIRECTION_CHECKS)
		{
			int cellInDirection = Grid.GetCellInDirection(testCell, d);
			if (Grid.IsValidCell(cellInDirection) && Grid.IsSolidCell(cellInDirection) && !Grid.Foundation[cellInDirection] && Grid.Element[cellInDirection].tag == element)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000E63 RID: 3683
	public List<int> result_cells = new List<int>();

	// Token: 0x04000E64 RID: 3684
	private Tag element;

	// Token: 0x04000E65 RID: 3685
	private int max_results;

	// Token: 0x04000E66 RID: 3686
	public static List<Direction> DIRECTION_CHECKS = new List<Direction>
	{
		Direction.Down,
		Direction.Right,
		Direction.Left,
		Direction.Up
	};
}
