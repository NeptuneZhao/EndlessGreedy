using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200080B RID: 2059
[AddComponentMenu("KMonoBehaviour/scripts/FoundationMonitor")]
public class FoundationMonitor : KMonoBehaviour
{
	// Token: 0x060038E1 RID: 14561 RVA: 0x001366FC File Offset: 0x001348FC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.position = Grid.PosToCell(base.gameObject);
		foreach (CellOffset offset in this.monitorCells)
		{
			int cell = Grid.OffsetCell(this.position, offset);
			if (Grid.IsValidCell(this.position) && Grid.IsValidCell(cell))
			{
				this.partitionerEntries.Add(GameScenePartitioner.Instance.Add("FoundationMonitor.OnSpawn", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnGroundChanged)));
			}
			this.OnGroundChanged(null);
		}
	}

	// Token: 0x060038E2 RID: 14562 RVA: 0x001367A0 File Offset: 0x001349A0
	protected override void OnCleanUp()
	{
		foreach (HandleVector<int>.Handle handle in this.partitionerEntries)
		{
			GameScenePartitioner.Instance.Free(ref handle);
		}
		base.OnCleanUp();
	}

	// Token: 0x060038E3 RID: 14563 RVA: 0x00136800 File Offset: 0x00134A00
	public bool CheckFoundationValid()
	{
		return !this.needsFoundation || this.IsSuitableFoundation(this.position);
	}

	// Token: 0x060038E4 RID: 14564 RVA: 0x00136818 File Offset: 0x00134A18
	public bool IsSuitableFoundation(int cell)
	{
		bool flag = true;
		foreach (CellOffset offset in this.monitorCells)
		{
			if (!Grid.IsCellOffsetValid(cell, offset))
			{
				return false;
			}
			int i2 = Grid.OffsetCell(cell, offset);
			flag = Grid.Solid[i2];
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x060038E5 RID: 14565 RVA: 0x0013686C File Offset: 0x00134A6C
	public void OnGroundChanged(object callbackData)
	{
		if (!this.hasFoundation && this.CheckFoundationValid())
		{
			this.hasFoundation = true;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.HasNoFoundation);
			base.Trigger(-1960061727, null);
		}
		if (this.hasFoundation && !this.CheckFoundationValid())
		{
			this.hasFoundation = false;
			base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.HasNoFoundation, false);
			base.Trigger(-1960061727, null);
		}
	}

	// Token: 0x0400223A RID: 8762
	private int position;

	// Token: 0x0400223B RID: 8763
	[Serialize]
	public bool needsFoundation = true;

	// Token: 0x0400223C RID: 8764
	[Serialize]
	private bool hasFoundation = true;

	// Token: 0x0400223D RID: 8765
	public CellOffset[] monitorCells = new CellOffset[]
	{
		new CellOffset(0, -1)
	};

	// Token: 0x0400223E RID: 8766
	private List<HandleVector<int>.Handle> partitionerEntries = new List<HandleVector<int>.Handle>();
}
