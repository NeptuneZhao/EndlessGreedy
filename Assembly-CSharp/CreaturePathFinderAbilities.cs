using System;
using Klei.AI;

// Token: 0x02000422 RID: 1058
public class CreaturePathFinderAbilities : PathFinderAbilities
{
	// Token: 0x06001692 RID: 5778 RVA: 0x00078FDF File Offset: 0x000771DF
	public CreaturePathFinderAbilities(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06001693 RID: 5779 RVA: 0x00078FE8 File Offset: 0x000771E8
	protected override void Refresh(Navigator navigator)
	{
		if (PathFinder.IsSubmerged(Grid.PosToCell(navigator)))
		{
			this.canTraverseSubmered = true;
			return;
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.MaxUnderwaterTravelCost.Lookup(navigator);
		this.canTraverseSubmered = (attributeInstance == null);
	}

	// Token: 0x06001694 RID: 5780 RVA: 0x0007902A File Offset: 0x0007722A
	public override bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged)
	{
		return !submerged || this.canTraverseSubmered;
	}

	// Token: 0x04000C9E RID: 3230
	public bool canTraverseSubmered;
}
