using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200059F RID: 1439
[AddComponentMenu("KMonoBehaviour/scripts/Pathfinding")]
public class Pathfinding : KMonoBehaviour
{
	// Token: 0x060021F0 RID: 8688 RVA: 0x000BCCE0 File Offset: 0x000BAEE0
	public static void DestroyInstance()
	{
		Pathfinding.Instance = null;
		OffsetTableTracker.OnPathfindingInvalidated();
	}

	// Token: 0x060021F1 RID: 8689 RVA: 0x000BCCED File Offset: 0x000BAEED
	protected override void OnPrefabInit()
	{
		Pathfinding.Instance = this;
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x000BCCF5 File Offset: 0x000BAEF5
	public void AddNavGrid(NavGrid nav_grid)
	{
		this.NavGrids.Add(nav_grid);
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x000BCD04 File Offset: 0x000BAF04
	public NavGrid GetNavGrid(string id)
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			if (navGrid.id == id)
			{
				return navGrid;
			}
		}
		global::Debug.LogError("Could not find nav grid: " + id);
		return null;
	}

	// Token: 0x060021F4 RID: 8692 RVA: 0x000BCD78 File Offset: 0x000BAF78
	public List<NavGrid> GetNavGrids()
	{
		return this.NavGrids;
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000BCD80 File Offset: 0x000BAF80
	public void ResetNavGrids()
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.InitializeGraph();
		}
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x000BCDD0 File Offset: 0x000BAFD0
	public void FlushNavGridsOnLoad()
	{
		if (this.navGridsHaveBeenFlushedOnLoad)
		{
			return;
		}
		this.navGridsHaveBeenFlushedOnLoad = true;
		this.UpdateNavGrids(true);
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x000BCDEC File Offset: 0x000BAFEC
	public void UpdateNavGrids(bool update_all = false)
	{
		update_all = true;
		if (update_all)
		{
			using (List<NavGrid>.Enumerator enumerator = this.NavGrids.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NavGrid navGrid = enumerator.Current;
					navGrid.UpdateGraph();
				}
				return;
			}
		}
		foreach (NavGrid navGrid2 in this.NavGrids)
		{
			if (navGrid2.updateEveryFrame)
			{
				navGrid2.UpdateGraph();
			}
		}
		this.NavGrids[this.UpdateIdx].UpdateGraph();
		this.UpdateIdx = (this.UpdateIdx + 1) % this.NavGrids.Count;
	}

	// Token: 0x060021F8 RID: 8696 RVA: 0x000BCEBC File Offset: 0x000BB0BC
	public void RenderEveryTick()
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.DebugUpdate();
		}
	}

	// Token: 0x060021F9 RID: 8697 RVA: 0x000BCF0C File Offset: 0x000BB10C
	public void AddDirtyNavGridCell(int cell)
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.AddDirtyCell(cell);
		}
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x000BCF60 File Offset: 0x000BB160
	public void RefreshNavCell(int cell)
	{
		HashSet<int> hashSet = new HashSet<int>();
		hashSet.Add(cell);
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.UpdateGraph(hashSet);
		}
	}

	// Token: 0x060021FB RID: 8699 RVA: 0x000BCFC0 File Offset: 0x000BB1C0
	protected override void OnCleanUp()
	{
		this.NavGrids.Clear();
		OffsetTableTracker.OnPathfindingInvalidated();
	}

	// Token: 0x04001315 RID: 4885
	private List<NavGrid> NavGrids = new List<NavGrid>();

	// Token: 0x04001316 RID: 4886
	private int UpdateIdx;

	// Token: 0x04001317 RID: 4887
	private bool navGridsHaveBeenFlushedOnLoad;

	// Token: 0x04001318 RID: 4888
	public static Pathfinding Instance;
}
