using System;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02000F86 RID: 3974
	[ActionType("InterfaceTool", "Dig", true)]
	public abstract class DigAction
	{
		// Token: 0x060079D0 RID: 31184 RVA: 0x003014F8 File Offset: 0x002FF6F8
		public void Uproot(int cell)
		{
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			int x_bottomLeft;
			int y_bottomLeft;
			Grid.CellToXY(cell, out x_bottomLeft, out y_bottomLeft);
			GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, GameScenePartitioner.Instance.plants, pooledList);
			if (pooledList.Count > 0)
			{
				this.EntityDig((pooledList[0].obj as Component).GetComponent<IDigActionEntity>());
			}
			pooledList.Recycle();
		}

		// Token: 0x060079D1 RID: 31185
		public abstract void Dig(int cell, int distFromOrigin);

		// Token: 0x060079D2 RID: 31186
		protected abstract void EntityDig(IDigActionEntity digAction);
	}
}
