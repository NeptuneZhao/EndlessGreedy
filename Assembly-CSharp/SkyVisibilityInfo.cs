using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000A9E RID: 2718
public readonly struct SkyVisibilityInfo
{
	// Token: 0x06004FF9 RID: 20473 RVA: 0x001CC48C File Offset: 0x001CA68C
	public SkyVisibilityInfo(CellOffset scanLeftOffset, int scanLeftCount, CellOffset scanRightOffset, int scanRightCount, int verticalStep)
	{
		this.scanLeftOffset = scanLeftOffset;
		this.scanLeftCount = scanLeftCount;
		this.scanRightOffset = scanRightOffset;
		this.scanRightCount = scanRightCount;
		this.verticalStep = verticalStep;
		this.totalColumnsCount = scanLeftCount + scanRightCount + (scanRightOffset.x - scanLeftOffset.x + 1);
	}

	// Token: 0x06004FFA RID: 20474 RVA: 0x001CC4D8 File Offset: 0x001CA6D8
	[return: TupleElementNames(new string[]
	{
		"isAnyVisible",
		"percentVisible01"
	})]
	public ValueTuple<bool, float> GetVisibilityOf(GameObject gameObject)
	{
		return this.GetVisibilityOf(Grid.PosToCell(gameObject));
	}

	// Token: 0x06004FFB RID: 20475 RVA: 0x001CC4E8 File Offset: 0x001CA6E8
	[return: TupleElementNames(new string[]
	{
		"isAnyVisible",
		"percentVisible01"
	})]
	public ValueTuple<bool, float> GetVisibilityOf(int buildingCenterCellId)
	{
		int num = 0;
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[buildingCenterCellId]);
		num += SkyVisibilityInfo.ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, this.scanLeftOffset), -1, this.verticalStep, this.scanLeftCount, world);
		num += SkyVisibilityInfo.ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, this.scanRightOffset), 1, this.verticalStep, this.scanRightCount, world);
		if (this.scanLeftOffset.x == this.scanRightOffset.x)
		{
			num = Mathf.Max(0, num - 1);
		}
		return new ValueTuple<bool, float>(num > 0, (float)num / (float)this.totalColumnsCount);
	}

	// Token: 0x06004FFC RID: 20476 RVA: 0x001CC584 File Offset: 0x001CA784
	public void CollectVisibleCellsTo(HashSet<int> visibleCells, int buildingBottomLeftCellId, WorldContainer originWorld)
	{
		SkyVisibilityInfo.ScanAndCollectVisibleCellsTo(visibleCells, Grid.OffsetCell(buildingBottomLeftCellId, this.scanLeftOffset), -1, this.verticalStep, this.scanLeftCount, originWorld);
		SkyVisibilityInfo.ScanAndCollectVisibleCellsTo(visibleCells, Grid.OffsetCell(buildingBottomLeftCellId, this.scanRightOffset), 1, this.verticalStep, this.scanRightCount, originWorld);
	}

	// Token: 0x06004FFD RID: 20477 RVA: 0x001CC5D4 File Offset: 0x001CA7D4
	private static void ScanAndCollectVisibleCellsTo(HashSet<int> visibleCells, int originCellId, int stepX, int stepY, int stepCountInclusive, WorldContainer originWorld)
	{
		for (int i = 0; i <= stepCountInclusive; i++)
		{
			int num = Grid.OffsetCell(originCellId, i * stepX, i * stepY);
			if (!SkyVisibilityInfo.IsVisible(num, originWorld))
			{
				break;
			}
			visibleCells.Add(num);
		}
	}

	// Token: 0x06004FFE RID: 20478 RVA: 0x001CC610 File Offset: 0x001CA810
	private static int ScanAndGetVisibleCellCount(int originCellId, int stepX, int stepY, int stepCountInclusive, WorldContainer originWorld)
	{
		for (int i = 0; i <= stepCountInclusive; i++)
		{
			if (!SkyVisibilityInfo.IsVisible(Grid.OffsetCell(originCellId, i * stepX, i * stepY), originWorld))
			{
				return i;
			}
		}
		return stepCountInclusive + 1;
	}

	// Token: 0x06004FFF RID: 20479 RVA: 0x001CC644 File Offset: 0x001CA844
	public static bool IsVisible(int cellId, WorldContainer originWorld)
	{
		if (!Grid.IsValidCell(cellId))
		{
			return false;
		}
		if (Grid.ExposedToSunlight[cellId] > 0)
		{
			return true;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cellId]);
		if (world != null && world.IsModuleInterior)
		{
			return true;
		}
		originWorld != world;
		return false;
	}

	// Token: 0x0400351E RID: 13598
	public readonly CellOffset scanLeftOffset;

	// Token: 0x0400351F RID: 13599
	public readonly int scanLeftCount;

	// Token: 0x04003520 RID: 13600
	public readonly CellOffset scanRightOffset;

	// Token: 0x04003521 RID: 13601
	public readonly int scanRightCount;

	// Token: 0x04003522 RID: 13602
	public readonly int verticalStep;

	// Token: 0x04003523 RID: 13603
	public readonly int totalColumnsCount;
}
