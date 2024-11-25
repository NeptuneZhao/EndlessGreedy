using System;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
public class MingleCellSensor : Sensor
{
	// Token: 0x06001A09 RID: 6665 RVA: 0x0008A83E File Offset: 0x00088A3E
	public MingleCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
	}

	// Token: 0x06001A0A RID: 6666 RVA: 0x0008A860 File Offset: 0x00088A60
	public override void Update()
	{
		this.cell = Grid.InvalidCell;
		int num = int.MaxValue;
		ListPool<int, MingleCellSensor>.PooledList pooledList = ListPool<int, MingleCellSensor>.Allocate();
		int num2 = 50;
		foreach (int num3 in Game.Instance.mingleCellTracker.mingleCells)
		{
			if (this.brain.IsCellClear(num3))
			{
				int navigationCost = this.navigator.GetNavigationCost(num3);
				if (navigationCost != -1)
				{
					if (num3 == Grid.InvalidCell || navigationCost < num)
					{
						this.cell = num3;
						num = navigationCost;
					}
					if (navigationCost < num2)
					{
						pooledList.Add(num3);
					}
				}
			}
		}
		if (pooledList.Count > 0)
		{
			this.cell = pooledList[UnityEngine.Random.Range(0, pooledList.Count)];
		}
		pooledList.Recycle();
	}

	// Token: 0x06001A0B RID: 6667 RVA: 0x0008A940 File Offset: 0x00088B40
	public int GetCell()
	{
		return this.cell;
	}

	// Token: 0x04000ED4 RID: 3796
	private MinionBrain brain;

	// Token: 0x04000ED5 RID: 3797
	private Navigator navigator;

	// Token: 0x04000ED6 RID: 3798
	private int cell;
}
