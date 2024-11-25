using System;

namespace Rendering.World
{
	// Token: 0x02000E1C RID: 3612
	public struct Tile
	{
		// Token: 0x0600731E RID: 29470 RVA: 0x002C2171 File Offset: 0x002C0371
		public Tile(int idx, int tile_x, int tile_y, int mask_count)
		{
			this.Idx = idx;
			this.TileCells = new TileCells(tile_x, tile_y);
			this.MaskCount = mask_count;
		}

		// Token: 0x04004F54 RID: 20308
		public int Idx;

		// Token: 0x04004F55 RID: 20309
		public TileCells TileCells;

		// Token: 0x04004F56 RID: 20310
		public int MaskCount;
	}
}
