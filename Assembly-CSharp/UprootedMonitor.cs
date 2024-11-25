using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000827 RID: 2087
[AddComponentMenu("KMonoBehaviour/scripts/UprootedMonitor")]
public class UprootedMonitor : KMonoBehaviour
{
	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x060039BC RID: 14780 RVA: 0x0013A7F7 File Offset: 0x001389F7
	public bool IsUprooted
	{
		get
		{
			return this.uprooted || base.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
		}
	}

	// Token: 0x060039BD RID: 14781 RVA: 0x0013A814 File Offset: 0x00138A14
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<UprootedMonitor>(-216549700, UprootedMonitor.OnUprootedDelegate);
		this.position = Grid.PosToCell(base.gameObject);
		foreach (CellOffset offset in this.monitorCells)
		{
			int cell = Grid.OffsetCell(this.position, offset);
			if (Grid.IsValidCell(this.position) && Grid.IsValidCell(cell))
			{
				this.partitionerEntries.Add(GameScenePartitioner.Instance.Add("UprootedMonitor.OnSpawn", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnGroundChanged)));
			}
			this.OnGroundChanged(null);
		}
	}

	// Token: 0x060039BE RID: 14782 RVA: 0x0013A8C8 File Offset: 0x00138AC8
	protected override void OnCleanUp()
	{
		foreach (HandleVector<int>.Handle handle in this.partitionerEntries)
		{
			GameScenePartitioner.Instance.Free(ref handle);
		}
		base.OnCleanUp();
	}

	// Token: 0x060039BF RID: 14783 RVA: 0x0013A928 File Offset: 0x00138B28
	public bool CheckTileGrowable()
	{
		return !this.canBeUprooted || (!this.uprooted && this.IsSuitableFoundation(this.position));
	}

	// Token: 0x060039C0 RID: 14784 RVA: 0x0013A950 File Offset: 0x00138B50
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

	// Token: 0x060039C1 RID: 14785 RVA: 0x0013A9A1 File Offset: 0x00138BA1
	public void OnGroundChanged(object callbackData)
	{
		if (!this.CheckTileGrowable())
		{
			this.uprooted = true;
		}
		if (this.uprooted)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			base.Trigger(-216549700, null);
		}
	}

	// Token: 0x040022BB RID: 8891
	private int position;

	// Token: 0x040022BC RID: 8892
	[Serialize]
	public bool canBeUprooted = true;

	// Token: 0x040022BD RID: 8893
	[Serialize]
	private bool uprooted;

	// Token: 0x040022BE RID: 8894
	public CellOffset[] monitorCells = new CellOffset[]
	{
		new CellOffset(0, -1)
	};

	// Token: 0x040022BF RID: 8895
	private List<HandleVector<int>.Handle> partitionerEntries = new List<HandleVector<int>.Handle>();

	// Token: 0x040022C0 RID: 8896
	private static readonly EventSystem.IntraObjectHandler<UprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<UprootedMonitor>(delegate(UprootedMonitor component, object data)
	{
		if (!component.uprooted)
		{
			component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			component.uprooted = true;
			component.Trigger(-216549700, null);
		}
	});
}
