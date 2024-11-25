using System;

// Token: 0x0200093F RID: 2367
public class LiquidFetchMask
{
	// Token: 0x060044CD RID: 17613 RVA: 0x001877AC File Offset: 0x001859AC
	public LiquidFetchMask(CellOffset[][] offset_table)
	{
		for (int i = 0; i < offset_table.Length; i++)
		{
			for (int j = 0; j < offset_table[i].Length; j++)
			{
				this.maxOffset.x = Math.Max(this.maxOffset.x, Math.Abs(offset_table[i][j].x));
				this.maxOffset.y = Math.Max(this.maxOffset.y, Math.Abs(offset_table[i][j].y));
			}
		}
		this.isLiquidAvailable = new bool[Grid.CellCount];
		for (int k = 0; k < Grid.CellCount; k++)
		{
			this.RefreshCell(k);
		}
	}

	// Token: 0x060044CE RID: 17614 RVA: 0x00187860 File Offset: 0x00185A60
	private void RefreshCell(int cell)
	{
		CellOffset offset = Grid.GetOffset(cell);
		int num = Math.Max(0, offset.y - this.maxOffset.y);
		while (num < Grid.HeightInCells && num < offset.y + this.maxOffset.y)
		{
			int num2 = Math.Max(0, offset.x - this.maxOffset.x);
			while (num2 < Grid.WidthInCells && num2 < offset.x + this.maxOffset.x)
			{
				if (Grid.Element[Grid.XYToCell(num2, num)].IsLiquid)
				{
					this.isLiquidAvailable[cell] = true;
					return;
				}
				num2++;
			}
			num++;
		}
		this.isLiquidAvailable[cell] = false;
	}

	// Token: 0x060044CF RID: 17615 RVA: 0x00187913 File Offset: 0x00185B13
	public void MarkDirty(int cell)
	{
		this.RefreshCell(cell);
	}

	// Token: 0x060044D0 RID: 17616 RVA: 0x0018791C File Offset: 0x00185B1C
	public bool IsLiquidAvailable(int cell)
	{
		return this.isLiquidAvailable[cell];
	}

	// Token: 0x060044D1 RID: 17617 RVA: 0x00187926 File Offset: 0x00185B26
	public void Destroy()
	{
		this.isLiquidAvailable = null;
	}

	// Token: 0x04002D02 RID: 11522
	private bool[] isLiquidAvailable;

	// Token: 0x04002D03 RID: 11523
	private CellOffset maxOffset;
}
