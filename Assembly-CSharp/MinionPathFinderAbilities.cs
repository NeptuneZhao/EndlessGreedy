using System;
using System.Diagnostics;

// Token: 0x02000423 RID: 1059
public class MinionPathFinderAbilities : PathFinderAbilities
{
	// Token: 0x06001695 RID: 5781 RVA: 0x0007903C File Offset: 0x0007723C
	public MinionPathFinderAbilities(Navigator navigator) : base(navigator)
	{
		this.transitionVoidOffsets = new CellOffset[navigator.NavGrid.transitions.Length][];
		for (int i = 0; i < this.transitionVoidOffsets.Length; i++)
		{
			this.transitionVoidOffsets[i] = navigator.NavGrid.transitions[i].voidOffsets;
		}
	}

	// Token: 0x06001696 RID: 5782 RVA: 0x00079099 File Offset: 0x00077299
	protected override void Refresh(Navigator navigator)
	{
		this.proxyID = navigator.GetComponent<MinionIdentity>().assignableProxy.Get().GetComponent<KPrefabID>().InstanceID;
		this.out_of_fuel = navigator.HasTag(GameTags.JetSuitOutOfFuel);
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x000790CC File Offset: 0x000772CC
	public void SetIdleNavMaskEnabled(bool enabled)
	{
		this.idleNavMaskEnabled = enabled;
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x000790D5 File Offset: 0x000772D5
	private static bool IsAccessPermitted(int proxyID, int cell, int from_cell, NavType from_nav_type)
	{
		return Grid.HasPermission(cell, proxyID, from_cell, from_nav_type);
	}

	// Token: 0x06001699 RID: 5785 RVA: 0x000790E0 File Offset: 0x000772E0
	public override int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
	{
		if (!path.HasAnyFlag(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasLeadSuit))
		{
			return (int)(link.cost * 2);
		}
		return 0;
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x000790F8 File Offset: 0x000772F8
	public override bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged)
	{
		if (!MinionPathFinderAbilities.IsAccessPermitted(this.proxyID, path.cell, from_cell, from_nav_type))
		{
			return false;
		}
		foreach (CellOffset offset in this.transitionVoidOffsets[transition_id])
		{
			int cell = Grid.OffsetCell(from_cell, offset);
			if (!MinionPathFinderAbilities.IsAccessPermitted(this.proxyID, cell, from_cell, from_nav_type))
			{
				return false;
			}
		}
		if (path.navType == NavType.Tube && from_nav_type == NavType.Floor && !Grid.HasUsableTubeEntrance(from_cell, this.prefabInstanceID))
		{
			return false;
		}
		if (path.navType == NavType.Hover && (this.out_of_fuel || !path.HasFlag(PathFinder.PotentialPath.Flags.HasJetPack)))
		{
			return false;
		}
		Grid.SuitMarker.Flags flags = (Grid.SuitMarker.Flags)0;
		PathFinder.PotentialPath.Flags flags2 = PathFinder.PotentialPath.Flags.None;
		bool flag = path.HasFlag(PathFinder.PotentialPath.Flags.PerformSuitChecks) && Grid.TryGetSuitMarkerFlags(from_cell, out flags, out flags2) && (flags & Grid.SuitMarker.Flags.Operational) > (Grid.SuitMarker.Flags)0;
		bool flag2 = SuitMarker.DoesTraversalDirectionRequireSuit(from_cell, path.cell, flags);
		bool flag3 = path.HasAnyFlag(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasOxygenMask | PathFinder.PotentialPath.Flags.HasLeadSuit);
		if (flag)
		{
			bool flag4 = path.HasFlag(flags2);
			if (flag2)
			{
				if (!flag3 && !Grid.HasSuit(from_cell, this.prefabInstanceID))
				{
					return false;
				}
			}
			else if (flag3 && (flags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) != (Grid.SuitMarker.Flags)0 && (!flag4 || !Grid.HasEmptyLocker(from_cell, this.prefabInstanceID)))
			{
				return false;
			}
		}
		if (this.idleNavMaskEnabled && (Grid.PreventIdleTraversal[path.cell] || Grid.PreventIdleTraversal[from_cell]))
		{
			return false;
		}
		if (flag)
		{
			if (flag2)
			{
				if (!flag3)
				{
					path.SetFlags(flags2);
				}
			}
			else
			{
				path.ClearFlags(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasOxygenMask | PathFinder.PotentialPath.Flags.HasLeadSuit);
			}
		}
		return true;
	}

	// Token: 0x0600169B RID: 5787 RVA: 0x0007925A File Offset: 0x0007745A
	[Conditional("ENABLE_NAVIGATION_MASK_PROFILING")]
	private void BeginSample(string region_name)
	{
	}

	// Token: 0x0600169C RID: 5788 RVA: 0x0007925C File Offset: 0x0007745C
	[Conditional("ENABLE_NAVIGATION_MASK_PROFILING")]
	private void EndSample(string region_name)
	{
	}

	// Token: 0x04000C9F RID: 3231
	private CellOffset[][] transitionVoidOffsets;

	// Token: 0x04000CA0 RID: 3232
	private int proxyID;

	// Token: 0x04000CA1 RID: 3233
	private bool out_of_fuel;

	// Token: 0x04000CA2 RID: 3234
	private bool idleNavMaskEnabled;
}
