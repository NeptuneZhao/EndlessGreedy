using System;
using UnityEngine;

// Token: 0x020009D1 RID: 2513
public class OffsetTracker
{
	// Token: 0x06004908 RID: 18696 RVA: 0x001A2954 File Offset: 0x001A0B54
	public virtual CellOffset[] GetOffsets(int current_cell)
	{
		if (current_cell != this.previousCell)
		{
			global::Debug.Assert(!OffsetTracker.isExecutingWithinJob, "OffsetTracker.GetOffsets() is making a mutating call but is currently executing within a job");
			this.UpdateCell(this.previousCell, current_cell);
			this.previousCell = current_cell;
		}
		if (this.offsets == null)
		{
			global::Debug.Assert(!OffsetTracker.isExecutingWithinJob, "OffsetTracker.GetOffsets() is making a mutating call but is currently executing within a job");
			this.UpdateOffsets(this.previousCell);
		}
		return this.offsets;
	}

	// Token: 0x06004909 RID: 18697 RVA: 0x001A29BC File Offset: 0x001A0BBC
	public virtual bool ValidateOffsets(int current_cell)
	{
		return current_cell == this.previousCell && this.offsets != null;
	}

	// Token: 0x0600490A RID: 18698 RVA: 0x001A29D4 File Offset: 0x001A0BD4
	public void ForceRefresh()
	{
		int cell = this.previousCell;
		this.previousCell = Grid.InvalidCell;
		this.Refresh(cell);
	}

	// Token: 0x0600490B RID: 18699 RVA: 0x001A29FA File Offset: 0x001A0BFA
	public void Refresh(int cell)
	{
		this.GetOffsets(cell);
	}

	// Token: 0x0600490C RID: 18700 RVA: 0x001A2A04 File Offset: 0x001A0C04
	protected virtual void UpdateCell(int previous_cell, int current_cell)
	{
	}

	// Token: 0x0600490D RID: 18701 RVA: 0x001A2A06 File Offset: 0x001A0C06
	protected virtual void UpdateOffsets(int current_cell)
	{
	}

	// Token: 0x0600490E RID: 18702 RVA: 0x001A2A08 File Offset: 0x001A0C08
	public virtual void Clear()
	{
	}

	// Token: 0x0600490F RID: 18703 RVA: 0x001A2A0A File Offset: 0x001A0C0A
	public virtual void DebugDrawExtents()
	{
	}

	// Token: 0x06004910 RID: 18704 RVA: 0x001A2A0C File Offset: 0x001A0C0C
	public virtual void DebugDrawEditor()
	{
	}

	// Token: 0x06004911 RID: 18705 RVA: 0x001A2A10 File Offset: 0x001A0C10
	public virtual void DebugDrawOffsets(int cell)
	{
		foreach (CellOffset offset in this.GetOffsets(cell))
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
			Gizmos.DrawWireCube(Grid.CellToPosCCC(cell2, Grid.SceneLayer.Move), new Vector3(0.95f, 0.95f, 0.95f));
		}
	}

	// Token: 0x04002FCD RID: 12237
	public static bool isExecutingWithinJob;

	// Token: 0x04002FCE RID: 12238
	protected CellOffset[] offsets;

	// Token: 0x04002FCF RID: 12239
	protected int previousCell = Grid.InvalidCell;
}
