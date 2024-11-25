using System;
using System.Collections.Generic;

// Token: 0x02000499 RID: 1177
public class FloorCellQuery : PathFinderQuery
{
	// Token: 0x0600196C RID: 6508 RVA: 0x00087F3E File Offset: 0x0008613E
	public FloorCellQuery Reset(int max_results, int adjacent_cells_buffer = 0)
	{
		this.max_results = max_results;
		this.adjacent_cells_buffer = adjacent_cells_buffer;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x00087F5A File Offset: 0x0008615A
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidFloorCell(cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x00087F98 File Offset: 0x00086198
	private bool CheckValidFloorCell(int testCell)
	{
		if (!Grid.IsValidCell(testCell) || Grid.IsSolidCell(testCell))
		{
			return false;
		}
		int cellInDirection = Grid.GetCellInDirection(testCell, Direction.Up);
		int cellInDirection2 = Grid.GetCellInDirection(testCell, Direction.Down);
		if (!Grid.ObjectLayers[1].ContainsKey(testCell) && Grid.IsValidCell(cellInDirection2) && Grid.IsSolidCell(cellInDirection2) && Grid.IsValidCell(cellInDirection) && !Grid.IsSolidCell(cellInDirection))
		{
			int cell = testCell;
			int cell2 = testCell;
			for (int i = 0; i < this.adjacent_cells_buffer; i++)
			{
				cell = Grid.CellLeft(cell);
				cell2 = Grid.CellRight(cell2);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell))
				{
					return false;
				}
				if (!Grid.IsValidCell(cell2) || Grid.IsSolidCell(cell2))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000E5A RID: 3674
	public List<int> result_cells = new List<int>();

	// Token: 0x04000E5B RID: 3675
	private int max_results;

	// Token: 0x04000E5C RID: 3676
	private int adjacent_cells_buffer;
}
