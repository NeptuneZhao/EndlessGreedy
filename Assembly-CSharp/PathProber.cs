using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000491 RID: 1169
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/PathProber")]
public class PathProber : KMonoBehaviour
{
	// Token: 0x0600194D RID: 6477 RVA: 0x00087A3B File Offset: 0x00085C3B
	protected override void OnCleanUp()
	{
		if (this.PathGrid != null)
		{
			this.PathGrid.OnCleanUp();
		}
		base.OnCleanUp();
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x00087A56 File Offset: 0x00085C56
	public void SetGroupProber(IGroupProber group_prober)
	{
		this.PathGrid.SetGroupProber(group_prober);
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x00087A64 File Offset: 0x00085C64
	public void SetValidNavTypes(NavType[] nav_types, int max_probing_radius)
	{
		if (max_probing_radius != 0)
		{
			this.PathGrid = new PathGrid(max_probing_radius * 2, max_probing_radius * 2, true, nav_types);
			return;
		}
		this.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, nav_types);
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x00087A94 File Offset: 0x00085C94
	public int GetCost(int cell)
	{
		return this.PathGrid.GetCost(cell);
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x00087AA2 File Offset: 0x00085CA2
	public int GetNavigationCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		return this.PathGrid.GetCostIgnoreProberOffset(cell, offsets);
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x00087AB1 File Offset: 0x00085CB1
	public PathGrid GetPathGrid()
	{
		return this.PathGrid;
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x00087ABC File Offset: 0x00085CBC
	public void UpdateProbe(NavGrid nav_grid, int cell, NavType nav_type, PathFinderAbilities abilities, PathFinder.PotentialPath.Flags flags)
	{
		if (this.scratchPad == null)
		{
			this.scratchPad = new PathFinder.PotentialScratchPad(nav_grid.maxLinksPerCell);
		}
		bool flag = this.updateCount == -1;
		bool flag2 = this.Potentials.Count == 0 || flag;
		this.PathGrid.BeginUpdate(cell, !flag2);
		if (flag2)
		{
			this.updateCount = 0;
			bool flag3;
			PathFinder.Cell cell2 = this.PathGrid.GetCell(cell, nav_type, out flag3);
			PathFinder.AddPotential(new PathFinder.PotentialPath(cell, nav_type, flags), Grid.InvalidCell, NavType.NumNavTypes, 0, 0, this.Potentials, this.PathGrid, ref cell2);
		}
		int num = (this.potentialCellsPerUpdate <= 0 || flag) ? int.MaxValue : this.potentialCellsPerUpdate;
		this.updateCount++;
		while (this.Potentials.Count > 0 && num > 0)
		{
			KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = this.Potentials.Next();
			num--;
			bool flag3;
			PathFinder.Cell cell3 = this.PathGrid.GetCell(keyValuePair.Value, out flag3);
			if (cell3.cost == keyValuePair.Key)
			{
				PathFinder.AddPotentials(this.scratchPad, keyValuePair.Value, cell3.cost, ref abilities, null, nav_grid.maxLinksPerCell, nav_grid.Links, this.Potentials, this.PathGrid, cell3.parent, cell3.parentNavType);
			}
		}
		bool flag4 = this.Potentials.Count == 0;
		this.PathGrid.EndUpdate(flag4);
		if (flag4)
		{
			int num2 = this.updateCount;
		}
	}

	// Token: 0x04000E46 RID: 3654
	public const int InvalidHandle = -1;

	// Token: 0x04000E47 RID: 3655
	public const int InvalidIdx = -1;

	// Token: 0x04000E48 RID: 3656
	public const int InvalidCell = -1;

	// Token: 0x04000E49 RID: 3657
	public const int InvalidCost = -1;

	// Token: 0x04000E4A RID: 3658
	private PathGrid PathGrid;

	// Token: 0x04000E4B RID: 3659
	private PathFinder.PotentialList Potentials = new PathFinder.PotentialList();

	// Token: 0x04000E4C RID: 3660
	public int updateCount = -1;

	// Token: 0x04000E4D RID: 3661
	private const int updateCountThreshold = 25;

	// Token: 0x04000E4E RID: 3662
	private PathFinder.PotentialScratchPad scratchPad;

	// Token: 0x04000E4F RID: 3663
	public int potentialCellsPerUpdate = -1;
}
