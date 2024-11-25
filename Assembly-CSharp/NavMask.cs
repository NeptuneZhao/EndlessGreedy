using System;

// Token: 0x02000487 RID: 1159
public class NavMask
{
	// Token: 0x06001915 RID: 6421 RVA: 0x0008699E File Offset: 0x00084B9E
	public virtual bool IsTraversable(PathFinder.PotentialPath path, int from_cell, int cost, int transition_id, PathFinderAbilities abilities)
	{
		return true;
	}

	// Token: 0x06001916 RID: 6422 RVA: 0x000869A1 File Offset: 0x00084BA1
	public virtual void ApplyTraversalToPath(ref PathFinder.PotentialPath path, int from_cell)
	{
	}
}
