using System;

// Token: 0x02000492 RID: 1170
public class BreathableCellQuery : PathFinderQuery
{
	// Token: 0x06001955 RID: 6485 RVA: 0x00087C55 File Offset: 0x00085E55
	public BreathableCellQuery Reset(Brain brain)
	{
		this.breather = brain.GetComponent<OxygenBreather>();
		return this;
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x00087C64 File Offset: 0x00085E64
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		return this.breather.IsBreathableElementAtCell(cell, null);
	}

	// Token: 0x04000E50 RID: 3664
	private OxygenBreather breather;
}
