using System;

// Token: 0x0200049B RID: 1179
public class IdleSuitMarkerCellQuery : PathFinderQuery
{
	// Token: 0x06001974 RID: 6516 RVA: 0x000880CD File Offset: 0x000862CD
	public IdleSuitMarkerCellQuery(bool is_rotated, int marker_x)
	{
		this.targetCell = Grid.InvalidCell;
		this.isRotated = is_rotated;
		this.markerX = marker_x;
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x000880F0 File Offset: 0x000862F0
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!Grid.PreventIdleTraversal[cell] && Grid.CellToXY(cell).x < this.markerX != this.isRotated)
		{
			this.targetCell = cell;
		}
		return this.targetCell != Grid.InvalidCell;
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x0008813C File Offset: 0x0008633C
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04000E60 RID: 3680
	private int targetCell;

	// Token: 0x04000E61 RID: 3681
	private bool isRotated;

	// Token: 0x04000E62 RID: 3682
	private int markerX;
}
