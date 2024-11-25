﻿using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000E1E RID: 3614
	public class LiquidTileOverlayRenderer : TileRenderer
	{
		// Token: 0x0600732A RID: 29482 RVA: 0x002C26EE File Offset: 0x002C08EE
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
		}

		// Token: 0x0600732B RID: 29483 RVA: 0x002C2708 File Offset: 0x002C0908
		protected override Mask[] GetMasks()
		{
			return new Mask[]
			{
				new Mask(this.Atlas, 0, false, false, false, false),
				new Mask(this.Atlas, 0, false, true, false, false),
				new Mask(this.Atlas, 1, false, false, false, false)
			};
		}

		// Token: 0x0600732C RID: 29484 RVA: 0x002C2760 File Offset: 0x002C0960
		public void OnShadersReloaded()
		{
			foreach (Element element in ElementLoader.elements)
			{
				if (element.IsLiquid && element.substance != null && element.substance.material != null)
				{
					Material material = new Material(element.substance.material);
					this.InitAlphaMaterial(material, element);
					int idx = element.substance.idx;
					for (int i = 0; i < this.Masks.Length; i++)
					{
						int num = idx * this.Masks.Length + i;
						element.substance.RefreshPropertyBlock();
						this.Brushes[num].SetMaterial(material, element.substance.propertyBlock);
					}
				}
			}
		}

		// Token: 0x0600732D RID: 29485 RVA: 0x002C284C File Offset: 0x002C0A4C
		public override void LoadBrushes()
		{
			this.Brushes = new Brush[ElementLoader.elements.Count * this.Masks.Length];
			foreach (Element element in ElementLoader.elements)
			{
				if (element.IsLiquid && element.substance != null && element.substance.material != null)
				{
					Material material = new Material(element.substance.material);
					this.InitAlphaMaterial(material, element);
					int idx = element.substance.idx;
					for (int i = 0; i < this.Masks.Length; i++)
					{
						int num = idx * this.Masks.Length + i;
						element.substance.RefreshPropertyBlock();
						this.Brushes[num] = new Brush(num, element.id.ToString(), material, this.Masks[i], this.ActiveBrushes, this.DirtyBrushes, this.TileGridWidth, element.substance.propertyBlock);
					}
				}
			}
		}

		// Token: 0x0600732E RID: 29486 RVA: 0x002C298C File Offset: 0x002C0B8C
		private void InitAlphaMaterial(Material alpha_material, Element element)
		{
			alpha_material.name = element.name;
			alpha_material.renderQueue = RenderQueues.BlockTiles + element.substance.idx;
			alpha_material.EnableKeyword("ALPHA");
			alpha_material.DisableKeyword("OPAQUE");
			alpha_material.SetTexture("_AlphaTestMap", this.Atlas.texture);
			alpha_material.SetInt("_SrcAlpha", 5);
			alpha_material.SetInt("_DstAlpha", 10);
			alpha_material.SetInt("_ZWrite", 0);
			alpha_material.SetColor("_Colour", element.substance.colour);
		}

		// Token: 0x0600732F RID: 29487 RVA: 0x002C2A28 File Offset: 0x002C0C28
		private bool RenderLiquid(int cell, int cell_above)
		{
			bool result = false;
			if (Grid.Element[cell].IsSolid)
			{
				Element element = Grid.Element[cell_above];
				if (element.IsLiquid && element.substance.material != null)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06007330 RID: 29488 RVA: 0x002C2A6C File Offset: 0x002C0C6C
		private void SetBrushIdx(int i, ref Tile tile, int substance_idx, LiquidTileOverlayRenderer.LiquidConnections connections, Brush[] brush_array, int[] brush_grid)
		{
			if (connections == LiquidTileOverlayRenderer.LiquidConnections.Empty)
			{
				brush_grid[tile.Idx * 4 + i] = -1;
				return;
			}
			Brush brush = brush_array[substance_idx * tile.MaskCount + connections - LiquidTileOverlayRenderer.LiquidConnections.Left];
			brush.Add(tile.Idx);
			brush_grid[tile.Idx * 4 + i] = brush.Id;
		}

		// Token: 0x06007331 RID: 29489 RVA: 0x002C2AC4 File Offset: 0x002C0CC4
		public override void MarkDirty(ref Tile tile, Brush[] brush_array, int[] brush_grid)
		{
			if (!this.RenderLiquid(tile.TileCells.Cell0, tile.TileCells.Cell2))
			{
				if (this.RenderLiquid(tile.TileCells.Cell1, tile.TileCells.Cell3))
				{
					this.SetBrushIdx(1, ref tile, Grid.Element[tile.TileCells.Cell3].substance.idx, LiquidTileOverlayRenderer.LiquidConnections.Right, brush_array, brush_grid);
				}
				return;
			}
			if (this.RenderLiquid(tile.TileCells.Cell1, tile.TileCells.Cell3))
			{
				this.SetBrushIdx(0, ref tile, Grid.Element[tile.TileCells.Cell2].substance.idx, LiquidTileOverlayRenderer.LiquidConnections.Both, brush_array, brush_grid);
				return;
			}
			this.SetBrushIdx(0, ref tile, Grid.Element[tile.TileCells.Cell2].substance.idx, LiquidTileOverlayRenderer.LiquidConnections.Left, brush_array, brush_grid);
		}

		// Token: 0x02001F49 RID: 8009
		private enum LiquidConnections
		{
			// Token: 0x04008D2C RID: 36140
			Left = 1,
			// Token: 0x04008D2D RID: 36141
			Right,
			// Token: 0x04008D2E RID: 36142
			Both,
			// Token: 0x04008D2F RID: 36143
			Empty = 128
		}
	}
}
