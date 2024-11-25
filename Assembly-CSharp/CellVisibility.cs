using System;

// Token: 0x020007AA RID: 1962
public class CellVisibility
{
	// Token: 0x060035B8 RID: 13752 RVA: 0x00124530 File Offset: 0x00122730
	public CellVisibility()
	{
		Grid.GetVisibleExtents(out this.MinX, out this.MinY, out this.MaxX, out this.MaxY);
	}

	// Token: 0x060035B9 RID: 13753 RVA: 0x00124558 File Offset: 0x00122758
	public bool IsVisible(int cell)
	{
		int num = Grid.CellColumn(cell);
		if (num < this.MinX || num > this.MaxX)
		{
			return false;
		}
		int num2 = Grid.CellRow(cell);
		return num2 >= this.MinY && num2 <= this.MaxY;
	}

	// Token: 0x04001FFD RID: 8189
	private int MinX;

	// Token: 0x04001FFE RID: 8190
	private int MinY;

	// Token: 0x04001FFF RID: 8191
	private int MaxX;

	// Token: 0x04002000 RID: 8192
	private int MaxY;
}
