using System;

// Token: 0x020009D0 RID: 2512
public class OffsetTableTracker : OffsetTracker
{
	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x060048FF RID: 18687 RVA: 0x001A26B1 File Offset: 0x001A08B1
	private static NavGrid navGrid
	{
		get
		{
			if (OffsetTableTracker.navGridImpl == null)
			{
				OffsetTableTracker.navGridImpl = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
			}
			return OffsetTableTracker.navGridImpl;
		}
	}

	// Token: 0x06004900 RID: 18688 RVA: 0x001A26D3 File Offset: 0x001A08D3
	public OffsetTableTracker(CellOffset[][] table, KMonoBehaviour cmp)
	{
		this.table = table;
		this.cmp = cmp;
	}

	// Token: 0x06004901 RID: 18689 RVA: 0x001A26EC File Offset: 0x001A08EC
	protected override void UpdateCell(int previous_cell, int current_cell)
	{
		if (previous_cell == current_cell)
		{
			return;
		}
		base.UpdateCell(previous_cell, current_cell);
		Extents extents = new Extents(current_cell, this.table);
		extents.height += 2;
		extents.y--;
		if (!this.solidPartitionerEntry.IsValid())
		{
			this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", this.cmp.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnCellChanged));
			this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", this.cmp.gameObject, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
		}
		else
		{
			GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, extents);
			GameScenePartitioner.Instance.UpdatePosition(this.validNavCellChangedPartitionerEntry, extents);
		}
		this.offsets = null;
	}

	// Token: 0x06004902 RID: 18690 RVA: 0x001A27D4 File Offset: 0x001A09D4
	private static bool IsValidRow(int current_cell, CellOffset[] row, int rowIdx, int[] debugIdxs)
	{
		for (int i = 1; i < row.Length; i++)
		{
			int num = Grid.OffsetCell(current_cell, row[i]);
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if (Grid.Solid[num])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004903 RID: 18691 RVA: 0x001A2818 File Offset: 0x001A0A18
	private void UpdateOffsets(int cell, CellOffset[][] table)
	{
		HashSetPool<CellOffset, OffsetTableTracker>.PooledHashSet pooledHashSet = HashSetPool<CellOffset, OffsetTableTracker>.Allocate();
		if (Grid.IsValidCell(cell))
		{
			for (int i = 0; i < table.Length; i++)
			{
				CellOffset[] array = table[i];
				if (!pooledHashSet.Contains(array[0]))
				{
					int cell2 = Grid.OffsetCell(cell, array[0]);
					for (int j = 0; j < OffsetTableTracker.navGrid.ValidNavTypes.Length; j++)
					{
						NavType navType = OffsetTableTracker.navGrid.ValidNavTypes[j];
						if (navType != NavType.Tube && OffsetTableTracker.navGrid.NavTable.IsValid(cell2, navType) && OffsetTableTracker.IsValidRow(cell, array, i, this.DEBUG_rowValidIdx))
						{
							pooledHashSet.Add(array[0]);
							break;
						}
					}
				}
			}
		}
		if (this.offsets == null || this.offsets.Length != pooledHashSet.Count)
		{
			this.offsets = new CellOffset[pooledHashSet.Count];
		}
		pooledHashSet.CopyTo(this.offsets);
		pooledHashSet.Recycle();
	}

	// Token: 0x06004904 RID: 18692 RVA: 0x001A2909 File Offset: 0x001A0B09
	protected override void UpdateOffsets(int current_cell)
	{
		base.UpdateOffsets(current_cell);
		this.UpdateOffsets(current_cell, this.table);
	}

	// Token: 0x06004905 RID: 18693 RVA: 0x001A291F File Offset: 0x001A0B1F
	private void OnCellChanged(object data)
	{
		this.offsets = null;
	}

	// Token: 0x06004906 RID: 18694 RVA: 0x001A2928 File Offset: 0x001A0B28
	public override void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
	}

	// Token: 0x06004907 RID: 18695 RVA: 0x001A294A File Offset: 0x001A0B4A
	public static void OnPathfindingInvalidated()
	{
		OffsetTableTracker.navGridImpl = null;
	}

	// Token: 0x04002FC7 RID: 12231
	private readonly CellOffset[][] table;

	// Token: 0x04002FC8 RID: 12232
	public HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x04002FC9 RID: 12233
	public HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x04002FCA RID: 12234
	private static NavGrid navGridImpl;

	// Token: 0x04002FCB RID: 12235
	private KMonoBehaviour cmp;

	// Token: 0x04002FCC RID: 12236
	private int[] DEBUG_rowValidIdx;
}
