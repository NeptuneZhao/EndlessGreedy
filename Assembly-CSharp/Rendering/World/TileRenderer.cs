using System;
using System.Collections.Generic;

namespace Rendering.World
{
	// Token: 0x02000E1D RID: 3613
	public abstract class TileRenderer : KMonoBehaviour
	{
		// Token: 0x0600731F RID: 29471 RVA: 0x002C2190 File Offset: 0x002C0390
		protected override void OnSpawn()
		{
			this.Masks = this.GetMasks();
			this.TileGridWidth = Grid.WidthInCells + 1;
			this.TileGridHeight = Grid.HeightInCells + 1;
			this.BrushGrid = new int[this.TileGridWidth * this.TileGridHeight * 4];
			for (int i = 0; i < this.BrushGrid.Length; i++)
			{
				this.BrushGrid[i] = -1;
			}
			this.TileGrid = new Tile[this.TileGridWidth * this.TileGridHeight];
			for (int j = 0; j < this.TileGrid.Length; j++)
			{
				int tile_x = j % this.TileGridWidth;
				int tile_y = j / this.TileGridWidth;
				this.TileGrid[j] = new Tile(j, tile_x, tile_y, this.Masks.Length);
			}
			this.LoadBrushes();
			this.VisibleAreaUpdater = new VisibleAreaUpdater(new Action<int>(this.UpdateOutsideView), new Action<int>(this.UpdateInsideView), "TileRenderer");
		}

		// Token: 0x06007320 RID: 29472 RVA: 0x002C2280 File Offset: 0x002C0480
		protected virtual Mask[] GetMasks()
		{
			return new Mask[]
			{
				new Mask(this.Atlas, 0, false, false, false, false),
				new Mask(this.Atlas, 2, false, false, true, false),
				new Mask(this.Atlas, 2, false, true, true, false),
				new Mask(this.Atlas, 1, false, false, true, false),
				new Mask(this.Atlas, 2, false, false, false, false),
				new Mask(this.Atlas, 1, true, false, false, false),
				new Mask(this.Atlas, 3, false, false, false, false),
				new Mask(this.Atlas, 4, false, false, true, false),
				new Mask(this.Atlas, 2, false, true, false, false),
				new Mask(this.Atlas, 3, true, false, false, false),
				new Mask(this.Atlas, 1, true, false, true, false),
				new Mask(this.Atlas, 4, false, true, true, false),
				new Mask(this.Atlas, 1, false, false, false, false),
				new Mask(this.Atlas, 4, false, false, false, false),
				new Mask(this.Atlas, 4, false, true, false, false),
				new Mask(this.Atlas, 0, false, false, false, true)
			};
		}

		// Token: 0x06007321 RID: 29473 RVA: 0x002C240C File Offset: 0x002C060C
		private void UpdateInsideView(int cell)
		{
			foreach (int item in this.GetCellTiles(cell))
			{
				this.ClearTiles.Add(item);
				this.DirtyTiles.Add(item);
			}
		}

		// Token: 0x06007322 RID: 29474 RVA: 0x002C2450 File Offset: 0x002C0650
		private void UpdateOutsideView(int cell)
		{
			foreach (int item in this.GetCellTiles(cell))
			{
				this.ClearTiles.Add(item);
			}
		}

		// Token: 0x06007323 RID: 29475 RVA: 0x002C2484 File Offset: 0x002C0684
		private int[] GetCellTiles(int cell)
		{
			int num = 0;
			int num2 = 0;
			Grid.CellToXY(cell, out num, out num2);
			this.CellTiles[0] = num2 * this.TileGridWidth + num;
			this.CellTiles[1] = num2 * this.TileGridWidth + (num + 1);
			this.CellTiles[2] = (num2 + 1) * this.TileGridWidth + num;
			this.CellTiles[3] = (num2 + 1) * this.TileGridWidth + (num + 1);
			return this.CellTiles;
		}

		// Token: 0x06007324 RID: 29476
		public abstract void LoadBrushes();

		// Token: 0x06007325 RID: 29477 RVA: 0x002C24F5 File Offset: 0x002C06F5
		public void MarkDirty(int cell)
		{
			this.VisibleAreaUpdater.UpdateCell(cell);
		}

		// Token: 0x06007326 RID: 29478 RVA: 0x002C2504 File Offset: 0x002C0704
		private void LateUpdate()
		{
			foreach (int num in this.ClearTiles)
			{
				this.Clear(ref this.TileGrid[num], this.Brushes, this.BrushGrid);
			}
			this.ClearTiles.Clear();
			foreach (int num2 in this.DirtyTiles)
			{
				this.MarkDirty(ref this.TileGrid[num2], this.Brushes, this.BrushGrid);
			}
			this.DirtyTiles.Clear();
			this.VisibleAreaUpdater.Update();
			foreach (Brush brush in this.DirtyBrushes)
			{
				brush.Refresh();
			}
			this.DirtyBrushes.Clear();
			foreach (Brush brush2 in this.ActiveBrushes)
			{
				brush2.Render();
			}
		}

		// Token: 0x06007327 RID: 29479
		public abstract void MarkDirty(ref Tile tile, Brush[] brush_array, int[] brush_grid);

		// Token: 0x06007328 RID: 29480 RVA: 0x002C2674 File Offset: 0x002C0874
		public void Clear(ref Tile tile, Brush[] brush_array, int[] brush_grid)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = tile.Idx * 4 + i;
				if (brush_grid[num] != -1)
				{
					brush_array[brush_grid[num]].Remove(tile.Idx);
				}
			}
		}

		// Token: 0x04004F57 RID: 20311
		private Tile[] TileGrid;

		// Token: 0x04004F58 RID: 20312
		private int[] BrushGrid;

		// Token: 0x04004F59 RID: 20313
		protected int TileGridWidth;

		// Token: 0x04004F5A RID: 20314
		protected int TileGridHeight;

		// Token: 0x04004F5B RID: 20315
		private int[] CellTiles = new int[4];

		// Token: 0x04004F5C RID: 20316
		protected Brush[] Brushes;

		// Token: 0x04004F5D RID: 20317
		protected Mask[] Masks;

		// Token: 0x04004F5E RID: 20318
		protected List<Brush> DirtyBrushes = new List<Brush>();

		// Token: 0x04004F5F RID: 20319
		protected List<Brush> ActiveBrushes = new List<Brush>();

		// Token: 0x04004F60 RID: 20320
		private VisibleAreaUpdater VisibleAreaUpdater;

		// Token: 0x04004F61 RID: 20321
		private HashSet<int> ClearTiles = new HashSet<int>();

		// Token: 0x04004F62 RID: 20322
		private HashSet<int> DirtyTiles = new HashSet<int>();

		// Token: 0x04004F63 RID: 20323
		public TextureAtlas Atlas;
	}
}
