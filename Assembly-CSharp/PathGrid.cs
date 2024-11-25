using System;
using System.Collections.Generic;

// Token: 0x02000490 RID: 1168
public class PathGrid
{
	// Token: 0x0600193F RID: 6463 RVA: 0x00087596 File Offset: 0x00085796
	public void SetGroupProber(IGroupProber group_prober)
	{
		this.groupProber = group_prober;
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x000875A0 File Offset: 0x000857A0
	public PathGrid(int width_in_cells, int height_in_cells, bool apply_offset, NavType[] valid_nav_types)
	{
		this.applyOffset = apply_offset;
		this.widthInCells = width_in_cells;
		this.heightInCells = height_in_cells;
		this.ValidNavTypes = valid_nav_types;
		int num = 0;
		this.NavTypeTable = new int[11];
		for (int i = 0; i < this.NavTypeTable.Length; i++)
		{
			this.NavTypeTable[i] = -1;
			for (int j = 0; j < this.ValidNavTypes.Length; j++)
			{
				if (this.ValidNavTypes[j] == (NavType)i)
				{
					this.NavTypeTable[i] = num++;
					break;
				}
			}
		}
		DebugUtil.DevAssert(true, "Cell packs nav type into 4 bits!", null);
		this.Cells = new PathFinder.Cell[width_in_cells * height_in_cells * this.ValidNavTypes.Length];
		this.ProberCells = new PathGrid.ProberCell[width_in_cells * height_in_cells];
		this.serialNo = 0;
		this.previousSerialNo = -1;
		this.isUpdating = false;
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x0008767A File Offset: 0x0008587A
	public void OnCleanUp()
	{
		if (this.groupProber != null)
		{
			this.groupProber.ReleaseProber(this);
		}
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x00087691 File Offset: 0x00085891
	public void ResetUpdate()
	{
		this.previousSerialNo = -1;
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x0008769C File Offset: 0x0008589C
	public void BeginUpdate(int root_cell, bool isContinuation)
	{
		this.isUpdating = true;
		this.freshlyOccupiedCells.Clear();
		if (isContinuation)
		{
			return;
		}
		if (this.applyOffset)
		{
			Grid.CellToXY(root_cell, out this.rootX, out this.rootY);
			this.rootX -= this.widthInCells / 2;
			this.rootY -= this.heightInCells / 2;
		}
		this.serialNo += 1;
		if (this.groupProber != null)
		{
			this.groupProber.SetValidSerialNos(this, this.previousSerialNo, this.serialNo);
		}
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x00087734 File Offset: 0x00085934
	public void EndUpdate(bool isComplete)
	{
		this.isUpdating = false;
		if (this.groupProber != null)
		{
			this.groupProber.Occupy(this, this.serialNo, this.freshlyOccupiedCells);
		}
		if (!isComplete)
		{
			return;
		}
		if (this.groupProber != null)
		{
			this.groupProber.SetValidSerialNos(this, this.serialNo, this.serialNo);
		}
		this.previousSerialNo = this.serialNo;
	}

	// Token: 0x06001945 RID: 6469 RVA: 0x00087798 File Offset: 0x00085998
	private bool IsValidSerialNo(short serialNo)
	{
		return serialNo == this.serialNo || (!this.isUpdating && this.previousSerialNo != -1 && serialNo == this.previousSerialNo);
	}

	// Token: 0x06001946 RID: 6470 RVA: 0x000877C1 File Offset: 0x000859C1
	public PathFinder.Cell GetCell(PathFinder.PotentialPath potential_path, out bool is_cell_in_range)
	{
		return this.GetCell(potential_path.cell, potential_path.navType, out is_cell_in_range);
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x000877D8 File Offset: 0x000859D8
	public PathFinder.Cell GetCell(int cell, NavType nav_type, out bool is_cell_in_range)
	{
		int num = this.OffsetCell(cell);
		is_cell_in_range = (-1 != num);
		if (!is_cell_in_range)
		{
			return PathGrid.InvalidCell;
		}
		PathFinder.Cell cell2 = this.Cells[num * this.ValidNavTypes.Length + this.NavTypeTable[(int)nav_type]];
		if (!this.IsValidSerialNo(cell2.queryId))
		{
			return PathGrid.InvalidCell;
		}
		return cell2;
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x00087834 File Offset: 0x00085A34
	public void SetCell(PathFinder.PotentialPath potential_path, ref PathFinder.Cell cell_data)
	{
		int num = this.OffsetCell(potential_path.cell);
		if (-1 == num)
		{
			return;
		}
		cell_data.queryId = this.serialNo;
		int num2 = this.NavTypeTable[(int)potential_path.navType];
		int num3 = num * this.ValidNavTypes.Length + num2;
		this.Cells[num3] = cell_data;
		if (potential_path.navType != NavType.Tube)
		{
			PathGrid.ProberCell proberCell = this.ProberCells[num];
			if (cell_data.queryId != proberCell.queryId || cell_data.cost < proberCell.cost)
			{
				proberCell.queryId = cell_data.queryId;
				proberCell.cost = cell_data.cost;
				this.ProberCells[num] = proberCell;
				this.freshlyOccupiedCells.Add(potential_path.cell);
			}
		}
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x000878F8 File Offset: 0x00085AF8
	public int GetCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		int num = -1;
		foreach (CellOffset offset in offsets)
		{
			int num2 = Grid.OffsetCell(cell, offset);
			if (Grid.IsValidCell(num2))
			{
				PathGrid.ProberCell proberCell = this.ProberCells[num2];
				if (this.IsValidSerialNo(proberCell.queryId) && (num == -1 || proberCell.cost < num))
				{
					num = proberCell.cost;
				}
			}
		}
		return num;
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x00087968 File Offset: 0x00085B68
	public int GetCost(int cell)
	{
		int num = this.OffsetCell(cell);
		if (-1 == num)
		{
			return -1;
		}
		PathGrid.ProberCell proberCell = this.ProberCells[num];
		if (!this.IsValidSerialNo(proberCell.queryId))
		{
			return -1;
		}
		return proberCell.cost;
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x000879A8 File Offset: 0x00085BA8
	private int OffsetCell(int cell)
	{
		if (!this.applyOffset)
		{
			return cell;
		}
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		if (num < this.rootX || num >= this.rootX + this.widthInCells || num2 < this.rootY || num2 >= this.rootY + this.heightInCells)
		{
			return -1;
		}
		int num3 = num - this.rootX;
		return (num2 - this.rootY) * this.widthInCells + num3;
	}

	// Token: 0x04000E37 RID: 3639
	private PathFinder.Cell[] Cells;

	// Token: 0x04000E38 RID: 3640
	private PathGrid.ProberCell[] ProberCells;

	// Token: 0x04000E39 RID: 3641
	private List<int> freshlyOccupiedCells = new List<int>();

	// Token: 0x04000E3A RID: 3642
	private NavType[] ValidNavTypes;

	// Token: 0x04000E3B RID: 3643
	private int[] NavTypeTable;

	// Token: 0x04000E3C RID: 3644
	private int widthInCells;

	// Token: 0x04000E3D RID: 3645
	private int heightInCells;

	// Token: 0x04000E3E RID: 3646
	private bool applyOffset;

	// Token: 0x04000E3F RID: 3647
	private int rootX;

	// Token: 0x04000E40 RID: 3648
	private int rootY;

	// Token: 0x04000E41 RID: 3649
	private short serialNo;

	// Token: 0x04000E42 RID: 3650
	private short previousSerialNo;

	// Token: 0x04000E43 RID: 3651
	private bool isUpdating;

	// Token: 0x04000E44 RID: 3652
	private IGroupProber groupProber;

	// Token: 0x04000E45 RID: 3653
	public static readonly PathFinder.Cell InvalidCell = new PathFinder.Cell
	{
		cost = -1
	};

	// Token: 0x0200126B RID: 4715
	private struct ProberCell
	{
		// Token: 0x04006355 RID: 25429
		public int cost;

		// Token: 0x04006356 RID: 25430
		public short queryId;
	}
}
