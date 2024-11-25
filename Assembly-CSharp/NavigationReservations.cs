using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000591 RID: 1425
[AddComponentMenu("KMonoBehaviour/scripts/NavigationReservations")]
public class NavigationReservations : KMonoBehaviour
{
	// Token: 0x0600215F RID: 8543 RVA: 0x000BAE86 File Offset: 0x000B9086
	public static void DestroyInstance()
	{
		NavigationReservations.Instance = null;
	}

	// Token: 0x06002160 RID: 8544 RVA: 0x000BAE8E File Offset: 0x000B908E
	public int GetOccupancyCount(int cell)
	{
		if (this.cellOccupancyDensity.ContainsKey(cell))
		{
			return this.cellOccupancyDensity[cell];
		}
		return 0;
	}

	// Token: 0x06002161 RID: 8545 RVA: 0x000BAEAC File Offset: 0x000B90AC
	public void AddOccupancy(int cell)
	{
		if (!this.cellOccupancyDensity.ContainsKey(cell))
		{
			this.cellOccupancyDensity.Add(cell, 1);
			return;
		}
		Dictionary<int, int> dictionary = this.cellOccupancyDensity;
		dictionary[cell]++;
	}

	// Token: 0x06002162 RID: 8546 RVA: 0x000BAEF0 File Offset: 0x000B90F0
	public void RemoveOccupancy(int cell)
	{
		int num = 0;
		if (this.cellOccupancyDensity.TryGetValue(cell, out num))
		{
			if (num == 1)
			{
				this.cellOccupancyDensity.Remove(cell);
				return;
			}
			this.cellOccupancyDensity[cell] = num - 1;
		}
	}

	// Token: 0x06002163 RID: 8547 RVA: 0x000BAF30 File Offset: 0x000B9130
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NavigationReservations.Instance = this;
	}

	// Token: 0x040012B3 RID: 4787
	public static NavigationReservations Instance;

	// Token: 0x040012B4 RID: 4788
	public static int InvalidReservation = -1;

	// Token: 0x040012B5 RID: 4789
	private Dictionary<int, int> cellOccupancyDensity = new Dictionary<int, int>();
}
