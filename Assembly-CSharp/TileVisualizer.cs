using System;
using UnityEngine;

// Token: 0x020005CE RID: 1486
public class TileVisualizer
{
	// Token: 0x06002439 RID: 9273 RVA: 0x000CA1D0 File Offset: 0x000C83D0
	private static void RefreshCellInternal(int cell, ObjectLayer tile_layer)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		GameObject gameObject = Grid.Objects[cell, (int)tile_layer];
		if (gameObject != null)
		{
			World.Instance.blockTileRenderer.Rebuild(tile_layer, cell);
			KAnimGraphTileVisualizer componentInChildren = gameObject.GetComponentInChildren<KAnimGraphTileVisualizer>();
			if (componentInChildren != null)
			{
				componentInChildren.Refresh();
				return;
			}
		}
	}

	// Token: 0x0600243A RID: 9274 RVA: 0x000CA22C File Offset: 0x000C842C
	private static void RefreshCell(int cell, ObjectLayer tile_layer)
	{
		if (tile_layer == ObjectLayer.NumLayers)
		{
			return;
		}
		TileVisualizer.RefreshCellInternal(cell, tile_layer);
		TileVisualizer.RefreshCellInternal(Grid.CellAbove(cell), tile_layer);
		TileVisualizer.RefreshCellInternal(Grid.CellBelow(cell), tile_layer);
		TileVisualizer.RefreshCellInternal(Grid.CellLeft(cell), tile_layer);
		TileVisualizer.RefreshCellInternal(Grid.CellRight(cell), tile_layer);
	}

	// Token: 0x0600243B RID: 9275 RVA: 0x000CA26B File Offset: 0x000C846B
	public static void RefreshCell(int cell, ObjectLayer tile_layer, ObjectLayer replacement_layer)
	{
		TileVisualizer.RefreshCell(cell, tile_layer);
		TileVisualizer.RefreshCell(cell, replacement_layer);
	}
}
