﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B15 RID: 2837
public class SpaceTreeSeededComet : Comet
{
	// Token: 0x06005487 RID: 21639 RVA: 0x001E3964 File Offset: 0x001E1B64
	protected override void DepositTiles(int cell, Element element, int world, int prev_cell, float temperature)
	{
		float depthOfElement = (float)base.GetDepthOfElement(cell2, element, world);
		float num = 1f;
		float num2 = (depthOfElement - (float)this.addTilesMinHeight) / (float)(this.addTilesMaxHeight - this.addTilesMinHeight);
		if (!float.IsNaN(num2))
		{
			num -= num2;
		}
		int num3 = Mathf.Min(this.addTiles, Mathf.Clamp(Mathf.RoundToInt((float)this.addTiles * num), 1, this.addTiles));
		HashSetPool<int, Comet>.PooledHashSet pooledHashSet = HashSetPool<int, Comet>.Allocate();
		HashSetPool<int, Comet>.PooledHashSet pooledHashSet2 = HashSetPool<int, Comet>.Allocate();
		QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
		int num4 = -1;
		int num5 = 1;
		if (this.velocity.x < 0f)
		{
			num4 *= -1;
			num5 *= -1;
		}
		pooledQueue.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = prev_cell,
			depth = 0
		});
		pooledQueue.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = Grid.OffsetCell(prev_cell, new CellOffset(num4, 0)),
			depth = 0
		});
		pooledQueue.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = Grid.OffsetCell(prev_cell, new CellOffset(num5, 0)),
			depth = 0
		});
		Func<int, bool> condition = (int cell) => Grid.IsValidCellInWorld(cell, world) && !Grid.Solid[cell];
		GameUtil.FloodFillConditional(pooledQueue, condition, pooledHashSet2, pooledHashSet, 10);
		float mass = (num3 > 0) ? (this.addTileMass / (float)this.addTiles) : 1f;
		int disease_count = this.addDiseaseCount / num3;
		float value = UnityEngine.Random.value;
		float num6 = (num3 == 0) ? -1f : (1f / (float)num3);
		float num7 = 0f;
		bool flag = false;
		using (HashSet<int>.Enumerator enumerator = pooledHashSet.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int viable_cell = enumerator.Current;
				if (num3 <= 0)
				{
					break;
				}
				num7 += num6;
				bool flag2 = !flag && num6 >= 0f && value <= num7;
				int callbackIdx = flag2 ? Game.Instance.callbackManager.Add(new Game.CallbackInfo(delegate()
				{
					SpaceTreeSeededComet.PlantTreeOnSolidTileCreated(viable_cell, this.addTilesMaxHeight);
				}, false)).index : -1;
				SimMessages.AddRemoveSubstance(viable_cell, element.id, CellEventLogger.Instance.ElementEmitted, mass, temperature, this.diseaseIdx, disease_count, true, callbackIdx);
				num3--;
				flag = (flag || flag2);
			}
		}
		pooledHashSet.Recycle();
		pooledHashSet2.Recycle();
		pooledQueue.Recycle();
	}

	// Token: 0x06005488 RID: 21640 RVA: 0x001E3C04 File Offset: 0x001E1E04
	private static void PlantTreeOnSolidTileCreated(int cell, int tileMaxHeight)
	{
		byte worldIdx = Grid.WorldIdx[cell];
		int num = 2;
		int num2 = Grid.OffsetCell(cell, new CellOffset(0, tileMaxHeight));
		int num3 = num2;
		bool flag = false;
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		for (;;)
		{
			num2 = num3;
			num3 = Grid.OffsetCell(num2, 0, -1);
			if (!Grid.IsValidCell(num3))
			{
				break;
			}
			if (Grid.Solid[num3] && SpaceTreeSeededComet.CanGrowOnCell(num2, worldIdx))
			{
				flag = true;
			}
			num--;
			if (flag || num <= 0)
			{
				goto IL_5F;
			}
		}
		return;
		IL_5F:
		if (flag)
		{
			GameObject prefab = Assets.GetPrefab("SpaceTree");
			KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
			Vector3 position = Grid.CellToPosCBC(num2, component.sceneLayer);
			Util.KInstantiate(prefab, position).SetActive(true);
		}
	}

	// Token: 0x06005489 RID: 21641 RVA: 0x001E3CA8 File Offset: 0x001E1EA8
	public static bool CanGrowOnCell(int spawnCell, byte worldIdx)
	{
		CellOffset[] occupiedCellsOffsets = Assets.GetPrefab("SpaceTree").GetComponent<OccupyArea>().OccupiedCellsOffsets;
		bool flag = true;
		int num = 0;
		while (flag && num < occupiedCellsOffsets.Length)
		{
			int num2 = Grid.OffsetCell(spawnCell, occupiedCellsOffsets[num]);
			flag = (flag && Grid.IsValidCellInWorld(num2, (int)worldIdx));
			flag = (flag && (!Grid.IsSolidCell(num2) || Grid.Element[num2].HasTag(GameTags.Unstable)));
			flag = (flag && Grid.Objects[num2, 1] == null);
			flag = (flag && Grid.Objects[num2, 5] == null);
			flag = (flag && !Grid.Foundation[num2]);
			num++;
		}
		return flag;
	}
}
