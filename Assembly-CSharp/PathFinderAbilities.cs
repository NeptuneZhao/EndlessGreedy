using System;

// Token: 0x0200048E RID: 1166
public abstract class PathFinderAbilities
{
	// Token: 0x06001934 RID: 6452 RVA: 0x00087527 File Offset: 0x00085727
	public PathFinderAbilities(Navigator navigator)
	{
		this.navigator = navigator;
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x00087536 File Offset: 0x00085736
	public void Refresh()
	{
		this.prefabInstanceID = this.navigator.gameObject.GetComponent<KPrefabID>().InstanceID;
		this.Refresh(this.navigator);
	}

	// Token: 0x06001936 RID: 6454
	protected abstract void Refresh(Navigator navigator);

	// Token: 0x06001937 RID: 6455
	public abstract bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged);

	// Token: 0x06001938 RID: 6456 RVA: 0x0008755F File Offset: 0x0008575F
	public virtual int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
	{
		return 0;
	}

	// Token: 0x04000E33 RID: 3635
	protected Navigator navigator;

	// Token: 0x04000E34 RID: 3636
	protected int prefabInstanceID;
}
