using System;
using UnityEngine;

// Token: 0x02000B49 RID: 2889
public abstract class UtilityNetworkLink : KMonoBehaviour
{
	// Token: 0x06005636 RID: 22070 RVA: 0x001ED512 File Offset: 0x001EB712
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<UtilityNetworkLink>(774203113, UtilityNetworkLink.OnBuildingBrokenDelegate);
		base.Subscribe<UtilityNetworkLink>(-1735440190, UtilityNetworkLink.OnBuildingFullyRepairedDelegate);
		this.Connect();
	}

	// Token: 0x06005637 RID: 22071 RVA: 0x001ED542 File Offset: 0x001EB742
	protected override void OnCleanUp()
	{
		base.Unsubscribe<UtilityNetworkLink>(774203113, UtilityNetworkLink.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<UtilityNetworkLink>(-1735440190, UtilityNetworkLink.OnBuildingFullyRepairedDelegate, false);
		this.Disconnect();
		base.OnCleanUp();
	}

	// Token: 0x06005638 RID: 22072 RVA: 0x001ED574 File Offset: 0x001EB774
	protected void Connect()
	{
		if (!this.visualizeOnly && !this.connected)
		{
			this.connected = true;
			int cell;
			int cell2;
			this.GetCells(out cell, out cell2);
			this.OnConnect(cell, cell2);
		}
	}

	// Token: 0x06005639 RID: 22073 RVA: 0x001ED5AA File Offset: 0x001EB7AA
	protected virtual void OnConnect(int cell1, int cell2)
	{
	}

	// Token: 0x0600563A RID: 22074 RVA: 0x001ED5AC File Offset: 0x001EB7AC
	protected void Disconnect()
	{
		if (!this.visualizeOnly && this.connected)
		{
			this.connected = false;
			int cell;
			int cell2;
			this.GetCells(out cell, out cell2);
			this.OnDisconnect(cell, cell2);
		}
	}

	// Token: 0x0600563B RID: 22075 RVA: 0x001ED5E2 File Offset: 0x001EB7E2
	protected virtual void OnDisconnect(int cell1, int cell2)
	{
	}

	// Token: 0x0600563C RID: 22076 RVA: 0x001ED5E4 File Offset: 0x001EB7E4
	public void GetCells(out int linked_cell1, out int linked_cell2)
	{
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			Orientation orientation = component.Orientation;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.GetCells(cell, orientation, out linked_cell1, out linked_cell2);
			return;
		}
		linked_cell1 = -1;
		linked_cell2 = -1;
	}

	// Token: 0x0600563D RID: 22077 RVA: 0x001ED62C File Offset: 0x001EB82C
	public void GetCells(int cell, Orientation orientation, out int linked_cell1, out int linked_cell2)
	{
		CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.link1, orientation);
		CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.link2, orientation);
		linked_cell1 = Grid.OffsetCell(cell, rotatedCellOffset);
		linked_cell2 = Grid.OffsetCell(cell, rotatedCellOffset2);
	}

	// Token: 0x0600563E RID: 22078 RVA: 0x001ED668 File Offset: 0x001EB868
	public bool AreCellsValid(int cell, Orientation orientation)
	{
		CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.link1, orientation);
		CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.link2, orientation);
		return Grid.IsCellOffsetValid(cell, rotatedCellOffset) && Grid.IsCellOffsetValid(cell, rotatedCellOffset2);
	}

	// Token: 0x0600563F RID: 22079 RVA: 0x001ED6A1 File Offset: 0x001EB8A1
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06005640 RID: 22080 RVA: 0x001ED6A9 File Offset: 0x001EB8A9
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06005641 RID: 22081 RVA: 0x001ED6B4 File Offset: 0x001EB8B4
	public int GetNetworkCell()
	{
		int result;
		int num;
		this.GetCells(out result, out num);
		return result;
	}

	// Token: 0x04003877 RID: 14455
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04003878 RID: 14456
	[SerializeField]
	public CellOffset link1;

	// Token: 0x04003879 RID: 14457
	[SerializeField]
	public CellOffset link2;

	// Token: 0x0400387A RID: 14458
	[SerializeField]
	public bool visualizeOnly;

	// Token: 0x0400387B RID: 14459
	private bool connected;

	// Token: 0x0400387C RID: 14460
	private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<UtilityNetworkLink>(delegate(UtilityNetworkLink component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x0400387D RID: 14461
	private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<UtilityNetworkLink>(delegate(UtilityNetworkLink component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
