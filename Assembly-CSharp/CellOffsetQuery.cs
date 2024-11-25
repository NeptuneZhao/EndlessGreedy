using System;

// Token: 0x02000496 RID: 1174
public class CellOffsetQuery : CellArrayQuery
{
	// Token: 0x06001964 RID: 6500 RVA: 0x00087E6C File Offset: 0x0008606C
	public CellArrayQuery Reset(int cell, CellOffset[] offsets)
	{
		int[] array = new int[offsets.Length];
		for (int i = 0; i < offsets.Length; i++)
		{
			array[i] = Grid.OffsetCell(cell, offsets[i]);
		}
		base.Reset(array);
		return this;
	}
}
