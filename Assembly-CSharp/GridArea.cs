using System;
using UnityEngine;

// Token: 0x020008DF RID: 2271
public struct GridArea
{
	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06004114 RID: 16660 RVA: 0x00171F1D File Offset: 0x0017011D
	public Vector2I Min
	{
		get
		{
			return this.min;
		}
	}

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06004115 RID: 16661 RVA: 0x00171F25 File Offset: 0x00170125
	public Vector2I Max
	{
		get
		{
			return this.max;
		}
	}

	// Token: 0x06004116 RID: 16662 RVA: 0x00171F30 File Offset: 0x00170130
	public void SetArea(int cell, int width, int height)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = new Vector2I(vector2I.x + width, vector2I.y + height);
		this.SetExtents(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y);
	}

	// Token: 0x06004117 RID: 16663 RVA: 0x00171F7C File Offset: 0x0017017C
	public void SetExtents(int min_x, int min_y, int max_x, int max_y)
	{
		this.min.x = Math.Max(min_x, 0);
		this.min.y = Math.Max(min_y, 0);
		this.max.x = Math.Min(max_x, Grid.WidthInCells);
		this.max.y = Math.Min(max_y, Grid.HeightInCells);
		this.MinCell = Grid.XYToCell(this.min.x, this.min.y);
		this.MaxCell = Grid.XYToCell(this.max.x, this.max.y);
	}

	// Token: 0x06004118 RID: 16664 RVA: 0x0017201C File Offset: 0x0017021C
	public bool Contains(int cell)
	{
		if (cell >= this.MinCell && cell < this.MaxCell)
		{
			int num = cell % Grid.WidthInCells;
			return num >= this.Min.x && num < this.Max.x;
		}
		return false;
	}

	// Token: 0x06004119 RID: 16665 RVA: 0x00172063 File Offset: 0x00170263
	public bool Contains(int x, int y)
	{
		return x >= this.min.x && x < this.max.x && y >= this.min.y && y < this.max.y;
	}

	// Token: 0x0600411A RID: 16666 RVA: 0x001720A0 File Offset: 0x001702A0
	public bool Contains(Vector3 pos)
	{
		return (float)this.min.x <= pos.x && pos.x < (float)this.max.x && (float)this.min.y <= pos.y && pos.y <= (float)this.max.y;
	}

	// Token: 0x0600411B RID: 16667 RVA: 0x00172102 File Offset: 0x00170302
	public void RunIfInside(int cell, Action<int> action)
	{
		if (this.Contains(cell))
		{
			action(cell);
		}
	}

	// Token: 0x0600411C RID: 16668 RVA: 0x00172114 File Offset: 0x00170314
	public void Run(Action<int> action)
	{
		for (int i = this.min.y; i < this.max.y; i++)
		{
			for (int j = this.min.x; j < this.max.x; j++)
			{
				int obj = Grid.XYToCell(j, i);
				action(obj);
			}
		}
	}

	// Token: 0x0600411D RID: 16669 RVA: 0x00172170 File Offset: 0x00170370
	public void RunOnDifference(GridArea subtract_area, Action<int> action)
	{
		for (int i = this.min.y; i < this.max.y; i++)
		{
			for (int j = this.min.x; j < this.max.x; j++)
			{
				if (!subtract_area.Contains(j, i))
				{
					int obj = Grid.XYToCell(j, i);
					action(obj);
				}
			}
		}
	}

	// Token: 0x0600411E RID: 16670 RVA: 0x001721D7 File Offset: 0x001703D7
	public int GetCellCount()
	{
		return (this.max.x - this.min.x) * (this.max.y - this.min.y);
	}

	// Token: 0x04002B33 RID: 11059
	private Vector2I min;

	// Token: 0x04002B34 RID: 11060
	private Vector2I max;

	// Token: 0x04002B35 RID: 11061
	private int MinCell;

	// Token: 0x04002B36 RID: 11062
	private int MaxCell;
}
