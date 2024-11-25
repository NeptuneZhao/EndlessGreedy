using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000493 RID: 1171
public class BuildingPlacementQuery : PathFinderQuery
{
	// Token: 0x06001958 RID: 6488 RVA: 0x00087C80 File Offset: 0x00085E80
	public BuildingPlacementQuery Reset(int max_results, GameObject toPlace)
	{
		this.max_results = max_results;
		this.toPlace = toPlace;
		this.cellOffsets = toPlace.GetComponent<OccupyArea>().OccupiedCellsOffsets;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x00087CAD File Offset: 0x00085EAD
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidPlaceCell(cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x00087CE8 File Offset: 0x00085EE8
	private bool CheckValidPlaceCell(int testCell)
	{
		if (!Grid.IsValidCell(testCell) || Grid.IsSolidCell(testCell) || Grid.ObjectLayers[1].ContainsKey(testCell))
		{
			return false;
		}
		bool flag = true;
		int widthInCells = this.toPlace.GetComponent<OccupyArea>().GetWidthInCells();
		int cell = testCell;
		for (int i = 0; i < widthInCells; i++)
		{
			int cellInDirection = Grid.GetCellInDirection(cell, Direction.Down);
			if (!Grid.IsValidCell(cellInDirection) || !Grid.IsSolidCell(cellInDirection))
			{
				flag = false;
				break;
			}
			cell = Grid.GetCellInDirection(cell, Direction.Right);
		}
		if (flag)
		{
			for (int j = 0; j < this.cellOffsets.Length; j++)
			{
				CellOffset offset = this.cellOffsets[j];
				int num = Grid.OffsetCell(testCell, offset);
				if (!Grid.IsValidCell(num) || Grid.IsSolidCell(num) || !Grid.IsValidBuildingCell(num) || Grid.ObjectLayers[1].ContainsKey(num))
				{
					flag = false;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x04000E51 RID: 3665
	public List<int> result_cells = new List<int>();

	// Token: 0x04000E52 RID: 3666
	private int max_results;

	// Token: 0x04000E53 RID: 3667
	private GameObject toPlace;

	// Token: 0x04000E54 RID: 3668
	private CellOffset[] cellOffsets;
}
