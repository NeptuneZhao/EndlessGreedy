using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008B8 RID: 2232
[AddComponentMenu("KMonoBehaviour/scripts/FishOvercrowingManager")]
public class FishOvercrowingManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06003E8D RID: 16013 RVA: 0x0015A522 File Offset: 0x00158722
	public static void DestroyInstance()
	{
		FishOvercrowingManager.Instance = null;
	}

	// Token: 0x06003E8E RID: 16014 RVA: 0x0015A52A File Offset: 0x0015872A
	protected override void OnPrefabInit()
	{
		FishOvercrowingManager.Instance = this;
		this.cells = new FishOvercrowingManager.Cell[Grid.CellCount];
	}

	// Token: 0x06003E8F RID: 16015 RVA: 0x0015A542 File Offset: 0x00158742
	public void Add(FishOvercrowdingMonitor.Instance fish)
	{
		this.fishes.Add(fish);
	}

	// Token: 0x06003E90 RID: 16016 RVA: 0x0015A550 File Offset: 0x00158750
	public void Remove(FishOvercrowdingMonitor.Instance fish)
	{
		this.fishes.Remove(fish);
	}

	// Token: 0x06003E91 RID: 16017 RVA: 0x0015A560 File Offset: 0x00158760
	public void Sim1000ms(float dt)
	{
		int num = this.versionCounter;
		this.versionCounter = num + 1;
		int num2 = num;
		int num3 = 1;
		this.cavityIdToCavityInfo.Clear();
		this.cellToFishCount.Clear();
		ListPool<FishOvercrowingManager.FishInfo, FishOvercrowingManager>.PooledList pooledList = ListPool<FishOvercrowingManager.FishInfo, FishOvercrowingManager>.Allocate();
		foreach (FishOvercrowdingMonitor.Instance instance in this.fishes)
		{
			int num4 = Grid.PosToCell(instance);
			if (Grid.IsValidCell(num4))
			{
				FishOvercrowingManager.FishInfo item = new FishOvercrowingManager.FishInfo
				{
					cell = num4,
					fish = instance
				};
				pooledList.Add(item);
				int num5 = 0;
				this.cellToFishCount.TryGetValue(num4, out num5);
				num5++;
				this.cellToFishCount[num4] = num5;
			}
		}
		foreach (FishOvercrowingManager.FishInfo fishInfo in pooledList)
		{
			ListPool<int, FishOvercrowingManager>.PooledList pooledList2 = ListPool<int, FishOvercrowingManager>.Allocate();
			pooledList2.Add(fishInfo.cell);
			int i = 0;
			int num6 = num3++;
			while (i < pooledList2.Count)
			{
				int num7 = pooledList2[i++];
				if (Grid.IsValidCell(num7))
				{
					FishOvercrowingManager.Cell cell = this.cells[num7];
					if (cell.version != num2 && Grid.IsLiquid(num7))
					{
						cell.cavityId = num6;
						cell.version = num2;
						int num8 = 0;
						this.cellToFishCount.TryGetValue(num7, out num8);
						FishOvercrowingManager.CavityInfo value = default(FishOvercrowingManager.CavityInfo);
						if (!this.cavityIdToCavityInfo.TryGetValue(num6, out value))
						{
							value = default(FishOvercrowingManager.CavityInfo);
						}
						value.fishCount += num8;
						value.cellCount++;
						this.cavityIdToCavityInfo[num6] = value;
						pooledList2.Add(Grid.CellLeft(num7));
						pooledList2.Add(Grid.CellRight(num7));
						pooledList2.Add(Grid.CellAbove(num7));
						pooledList2.Add(Grid.CellBelow(num7));
						this.cells[num7] = cell;
					}
				}
			}
			pooledList2.Recycle();
		}
		foreach (FishOvercrowingManager.FishInfo fishInfo2 in pooledList)
		{
			FishOvercrowingManager.Cell cell2 = this.cells[fishInfo2.cell];
			FishOvercrowingManager.CavityInfo cavityInfo = default(FishOvercrowingManager.CavityInfo);
			this.cavityIdToCavityInfo.TryGetValue(cell2.cavityId, out cavityInfo);
			fishInfo2.fish.SetOvercrowdingInfo(cavityInfo.cellCount, cavityInfo.fishCount);
		}
		pooledList.Recycle();
	}

	// Token: 0x04002672 RID: 9842
	public static FishOvercrowingManager Instance;

	// Token: 0x04002673 RID: 9843
	private List<FishOvercrowdingMonitor.Instance> fishes = new List<FishOvercrowdingMonitor.Instance>();

	// Token: 0x04002674 RID: 9844
	private Dictionary<int, FishOvercrowingManager.CavityInfo> cavityIdToCavityInfo = new Dictionary<int, FishOvercrowingManager.CavityInfo>();

	// Token: 0x04002675 RID: 9845
	private Dictionary<int, int> cellToFishCount = new Dictionary<int, int>();

	// Token: 0x04002676 RID: 9846
	private FishOvercrowingManager.Cell[] cells;

	// Token: 0x04002677 RID: 9847
	private int versionCounter = 1;

	// Token: 0x020017AE RID: 6062
	private struct Cell
	{
		// Token: 0x04007361 RID: 29537
		public int version;

		// Token: 0x04007362 RID: 29538
		public int cavityId;
	}

	// Token: 0x020017AF RID: 6063
	private struct FishInfo
	{
		// Token: 0x04007363 RID: 29539
		public int cell;

		// Token: 0x04007364 RID: 29540
		public FishOvercrowdingMonitor.Instance fish;
	}

	// Token: 0x020017B0 RID: 6064
	private struct CavityInfo
	{
		// Token: 0x04007365 RID: 29541
		public int fishCount;

		// Token: 0x04007366 RID: 29542
		public int cellCount;
	}
}
