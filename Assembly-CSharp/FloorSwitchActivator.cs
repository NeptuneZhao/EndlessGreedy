using System;
using UnityEngine;

// Token: 0x020008BA RID: 2234
[AddComponentMenu("KMonoBehaviour/scripts/FloorSwitchActivator")]
public class FloorSwitchActivator : KMonoBehaviour
{
	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06003E97 RID: 16023 RVA: 0x0015AAC3 File Offset: 0x00158CC3
	public PrimaryElement PrimaryElement
	{
		get
		{
			return this.primaryElement;
		}
	}

	// Token: 0x06003E98 RID: 16024 RVA: 0x0015AACB File Offset: 0x00158CCB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
		this.OnCellChange();
	}

	// Token: 0x06003E99 RID: 16025 RVA: 0x0015AADF File Offset: 0x00158CDF
	protected override void OnCleanUp()
	{
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x06003E9A RID: 16026 RVA: 0x0015AAF0 File Offset: 0x00158CF0
	private void OnCellChange()
	{
		int num = Grid.PosToCell(this);
		GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, num);
		if (Grid.IsValidCell(this.last_cell_occupied) && num != this.last_cell_occupied)
		{
			this.NotifyChanged(this.last_cell_occupied);
		}
		this.NotifyChanged(num);
		this.last_cell_occupied = num;
	}

	// Token: 0x06003E9B RID: 16027 RVA: 0x0015AB45 File Offset: 0x00158D45
	private void NotifyChanged(int cell)
	{
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, this);
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x0015AB5D File Offset: 0x00158D5D
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.Register();
	}

	// Token: 0x06003E9D RID: 16029 RVA: 0x0015AB6B File Offset: 0x00158D6B
	protected override void OnCmpDisable()
	{
		this.Unregister();
		base.OnCmpDisable();
	}

	// Token: 0x06003E9E RID: 16030 RVA: 0x0015AB7C File Offset: 0x00158D7C
	private void Register()
	{
		if (this.registered)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("FloorSwitchActivator.Register", this, cell, GameScenePartitioner.Instance.floorSwitchActivatorLayer, null);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "FloorSwitchActivator.Register");
		this.registered = true;
	}

	// Token: 0x06003E9F RID: 16031 RVA: 0x0015ABE4 File Offset: 0x00158DE4
	private void Unregister()
	{
		if (!this.registered)
		{
			return;
		}
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		if (this.last_cell_occupied > -1)
		{
			this.NotifyChanged(this.last_cell_occupied);
		}
		this.registered = false;
	}

	// Token: 0x0400267D RID: 9853
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400267E RID: 9854
	private bool registered;

	// Token: 0x0400267F RID: 9855
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002680 RID: 9856
	private int last_cell_occupied = -1;
}
