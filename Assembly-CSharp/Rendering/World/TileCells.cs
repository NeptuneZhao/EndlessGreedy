using System;

namespace Rendering.World
{
	// Token: 0x02000E1B RID: 3611
	public struct TileCells
	{
		// Token: 0x0600731D RID: 29469 RVA: 0x002C20D0 File Offset: 0x002C02D0
		public TileCells(int tile_x, int tile_y)
		{
			int val = Grid.WidthInCells - 1;
			int val2 = Grid.HeightInCells - 1;
			this.Cell0 = Grid.XYToCell(Math.Min(Math.Max(tile_x - 1, 0), val), Math.Min(Math.Max(tile_y - 1, 0), val2));
			this.Cell1 = Grid.XYToCell(Math.Min(tile_x, val), Math.Min(Math.Max(tile_y - 1, 0), val2));
			this.Cell2 = Grid.XYToCell(Math.Min(Math.Max(tile_x - 1, 0), val), Math.Min(tile_y, val2));
			this.Cell3 = Grid.XYToCell(Math.Min(tile_x, val), Math.Min(tile_y, val2));
		}

		// Token: 0x04004F50 RID: 20304
		public int Cell0;

		// Token: 0x04004F51 RID: 20305
		public int Cell1;

		// Token: 0x04004F52 RID: 20306
		public int Cell2;

		// Token: 0x04004F53 RID: 20307
		public int Cell3;
	}
}
