using System;
using UnityEngine;

// Token: 0x02000A7D RID: 2685
public struct Extents
{
	// Token: 0x06004EA2 RID: 20130 RVA: 0x001C4D48 File Offset: 0x001C2F48
	public static Extents OneCell(int cell)
	{
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		return new Extents(num, num2, 1, 1);
	}

	// Token: 0x06004EA3 RID: 20131 RVA: 0x001C4D68 File Offset: 0x001C2F68
	public Extents(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	// Token: 0x06004EA4 RID: 20132 RVA: 0x001C4D88 File Offset: 0x001C2F88
	public Extents(int cell, int radius)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		this.x = num - radius;
		this.y = num2 - radius;
		this.width = radius * 2 + 1;
		this.height = radius * 2 + 1;
	}

	// Token: 0x06004EA5 RID: 20133 RVA: 0x001C4DCB File Offset: 0x001C2FCB
	public Extents(int center_x, int center_y, int radius)
	{
		this.x = center_x - radius;
		this.y = center_y - radius;
		this.width = radius * 2 + 1;
		this.height = radius * 2 + 1;
	}

	// Token: 0x06004EA6 RID: 20134 RVA: 0x001C4DF8 File Offset: 0x001C2FF8
	public Extents(int cell, CellOffset[] offsets)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset offset in offsets)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, offset), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x06004EA7 RID: 20135 RVA: 0x001C4E94 File Offset: 0x001C3094
	public Extents(int cell, CellOffset[] offsets, Extents.BoundExtendsToGridFlag _)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset offset in offsets)
		{
			int val = 0;
			int val2 = 0;
			int cell2 = Grid.OffsetCell(cell, offset);
			if (Grid.IsValidCell(cell2))
			{
				Grid.CellToXY(cell2, out val, out val2);
				num = Math.Min(num, val);
				num2 = Math.Min(num2, val2);
				num3 = Math.Max(num3, val);
				num4 = Math.Max(num4, val2);
			}
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x06004EA8 RID: 20136 RVA: 0x001C4F3C File Offset: 0x001C313C
	public Extents(int cell, CellOffset[] offsets, Orientation orientation)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		for (int i = 0; i < offsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(offsets[i], orientation);
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, rotatedCellOffset), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x06004EA9 RID: 20137 RVA: 0x001C4FDC File Offset: 0x001C31DC
	public Extents(int cell, CellOffset[][] offset_table)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset[] array in offset_table)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, array[0]), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x06004EAA RID: 20138 RVA: 0x001C5078 File Offset: 0x001C3278
	public bool Contains(Vector2I pos)
	{
		return this.x <= pos.x && pos.x < this.x + this.width && this.y <= pos.y && pos.y < this.y + this.height;
	}

	// Token: 0x06004EAB RID: 20139 RVA: 0x001C50D0 File Offset: 0x001C32D0
	public bool Contains(Vector3 pos)
	{
		return (float)this.x <= pos.x && pos.x < (float)(this.x + this.width) && (float)this.y <= pos.y && pos.y < (float)(this.y + this.height);
	}

	// Token: 0x04003440 RID: 13376
	public int x;

	// Token: 0x04003441 RID: 13377
	public int y;

	// Token: 0x04003442 RID: 13378
	public int width;

	// Token: 0x04003443 RID: 13379
	public int height;

	// Token: 0x04003444 RID: 13380
	public static Extents.BoundExtendsToGridFlag BoundsCheckCoords;

	// Token: 0x02001AB1 RID: 6833
	public struct BoundExtendsToGridFlag
	{
	}
}
