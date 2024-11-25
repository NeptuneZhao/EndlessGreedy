using System;

// Token: 0x02000488 RID: 1160
public class NavTable
{
	// Token: 0x06001918 RID: 6424 RVA: 0x000869AC File Offset: 0x00084BAC
	public NavTable(int cell_count)
	{
		this.ValidCells = new short[cell_count];
		this.NavTypeMasks = new short[11];
		for (short num = 0; num < 11; num += 1)
		{
			this.NavTypeMasks[(int)num] = (short)(1 << (int)num);
		}
	}

	// Token: 0x06001919 RID: 6425 RVA: 0x000869F5 File Offset: 0x00084BF5
	public bool IsValid(int cell, NavType nav_type = NavType.Floor)
	{
		return Grid.IsValidCell(cell) && (this.NavTypeMasks[(int)nav_type] & this.ValidCells[cell]) != 0;
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x00086A18 File Offset: 0x00084C18
	public void SetValid(int cell, NavType nav_type, bool is_valid)
	{
		short num = this.NavTypeMasks[(int)nav_type];
		short num2 = this.ValidCells[cell];
		if ((num2 & num) != 0 != is_valid)
		{
			if (is_valid)
			{
				this.ValidCells[cell] = (num | num2);
			}
			else
			{
				this.ValidCells[cell] = (~num & num2);
			}
			if (this.OnValidCellChanged != null)
			{
				this.OnValidCellChanged(cell, nav_type);
			}
		}
	}

	// Token: 0x04000E10 RID: 3600
	public Action<int, NavType> OnValidCellChanged;

	// Token: 0x04000E11 RID: 3601
	private short[] NavTypeMasks;

	// Token: 0x04000E12 RID: 3602
	private short[] ValidCells;
}
