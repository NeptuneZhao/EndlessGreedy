using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200049D RID: 1181
public class PlantableCellQuery : PathFinderQuery
{
	// Token: 0x0600197C RID: 6524 RVA: 0x0008827F File Offset: 0x0008647F
	public PlantableCellQuery Reset(PlantableSeed seed, int max_results)
	{
		this.seed = seed;
		this.max_results = max_results;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x0008829C File Offset: 0x0008649C
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidPlotCell(this.seed, cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x000882E8 File Offset: 0x000864E8
	private bool CheckValidPlotCell(PlantableSeed seed, int plant_cell)
	{
		if (!Grid.IsValidCell(plant_cell))
		{
			return false;
		}
		int num;
		if (seed.Direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			num = Grid.CellAbove(plant_cell);
		}
		else
		{
			num = Grid.CellBelow(plant_cell);
		}
		if (!Grid.IsValidCell(num))
		{
			return false;
		}
		if (!Grid.Solid[num])
		{
			return false;
		}
		if (Grid.Objects[plant_cell, 5])
		{
			return false;
		}
		if (Grid.Objects[plant_cell, 1])
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[num, 1];
		if (gameObject)
		{
			PlantablePlot component = gameObject.GetComponent<PlantablePlot>();
			if (component == null)
			{
				return false;
			}
			if (component.Direction != seed.Direction)
			{
				return false;
			}
			if (component.Occupant != null)
			{
				return false;
			}
		}
		else
		{
			if (!seed.TestSuitableGround(plant_cell))
			{
				return false;
			}
			if (PlantableCellQuery.CountNearbyPlants(num, this.plantDetectionRadius) > this.maxPlantsInRadius)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x000883C4 File Offset: 0x000865C4
	private static int CountNearbyPlants(int cell, int radius)
	{
		int num = 0;
		int num2 = 0;
		Grid.PosToXY(Grid.CellToPos(cell), out num, out num2);
		int num3 = radius * 2;
		num -= radius;
		num2 -= radius;
		ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(num, num2, num3, num3, GameScenePartitioner.Instance.plants, pooledList);
		int num4 = 0;
		using (List<ScenePartitionerEntry>.Enumerator enumerator = pooledList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!((KPrefabID)enumerator.Current.obj).GetComponent<TreeBud>())
				{
					num4++;
				}
			}
		}
		pooledList.Recycle();
		return num4;
	}

	// Token: 0x04000E67 RID: 3687
	public List<int> result_cells = new List<int>();

	// Token: 0x04000E68 RID: 3688
	private PlantableSeed seed;

	// Token: 0x04000E69 RID: 3689
	private int max_results;

	// Token: 0x04000E6A RID: 3690
	private int plantDetectionRadius = 6;

	// Token: 0x04000E6B RID: 3691
	private int maxPlantsInRadius = 2;
}
