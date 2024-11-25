using System;

// Token: 0x02000497 RID: 1175
public class CellQuery : PathFinderQuery
{
	// Token: 0x06001966 RID: 6502 RVA: 0x00087EB1 File Offset: 0x000860B1
	public CellQuery Reset(int target_cell)
	{
		this.targetCell = target_cell;
		return this;
	}

	// Token: 0x06001967 RID: 6503 RVA: 0x00087EBB File Offset: 0x000860BB
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		return cell == this.targetCell;
	}

	// Token: 0x04000E59 RID: 3673
	private int targetCell;
}
