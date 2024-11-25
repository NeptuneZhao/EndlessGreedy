using System;

// Token: 0x0200048F RID: 1167
public class PathFinderQuery
{
	// Token: 0x06001939 RID: 6457 RVA: 0x00087562 File Offset: 0x00085762
	public virtual bool IsMatch(int cell, int parent_cell, int cost)
	{
		return true;
	}

	// Token: 0x0600193A RID: 6458 RVA: 0x00087565 File Offset: 0x00085765
	public void SetResult(int cell, int cost, NavType nav_type)
	{
		this.resultCell = cell;
		this.resultNavType = nav_type;
	}

	// Token: 0x0600193B RID: 6459 RVA: 0x00087575 File Offset: 0x00085775
	public void ClearResult()
	{
		this.resultCell = -1;
	}

	// Token: 0x0600193C RID: 6460 RVA: 0x0008757E File Offset: 0x0008577E
	public virtual int GetResultCell()
	{
		return this.resultCell;
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x00087586 File Offset: 0x00085786
	public NavType GetResultNavType()
	{
		return this.resultNavType;
	}

	// Token: 0x04000E35 RID: 3637
	protected int resultCell;

	// Token: 0x04000E36 RID: 3638
	private NavType resultNavType;
}
