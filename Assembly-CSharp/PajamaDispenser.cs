using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class PajamaDispenser : Workable, IDispenser
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000A8C RID: 2700 RVA: 0x0003F384 File Offset: 0x0003D584
	// (remove) Token: 0x06000A8D RID: 2701 RVA: 0x0003F3BC File Offset: 0x0003D5BC
	public event System.Action OnStopWorkEvent;

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0003F3F1 File Offset: 0x0003D5F1
	// (set) Token: 0x06000A8F RID: 2703 RVA: 0x0003F3FC File Offset: 0x0003D5FC
	private WorkChore<PajamaDispenser> Chore
	{
		get
		{
			return this.chore;
		}
		set
		{
			this.chore = value;
			if (this.chore != null)
			{
				base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, null);
				return;
			}
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, true);
		}
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x0003F45B File Offset: 0x0003D65B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (PajamaDispenser.pajamaPrefab != null)
		{
			return;
		}
		PajamaDispenser.pajamaPrefab = Assets.GetPrefab(new Tag("SleepClinicPajamas"));
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x0003F488 File Offset: 0x0003D688
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Vector3 targetPoint = this.GetTargetPoint();
		targetPoint.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront);
		Util.KInstantiate(PajamaDispenser.pajamaPrefab, targetPoint, Quaternion.identity, null, null, true, 0).SetActive(true);
		this.hasDispenseChore = false;
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0003F4CC File Offset: 0x0003D6CC
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.Chore != null && this.Chore.smi.IsRunning())
		{
			this.Chore.Cancel("work interrupted");
		}
		this.Chore = null;
		if (this.hasDispenseChore)
		{
			this.FetchPajamas();
		}
		if (this.OnStopWorkEvent != null)
		{
			this.OnStopWorkEvent();
		}
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x0003F534 File Offset: 0x0003D734
	[ContextMenu("fetch")]
	public void FetchPajamas()
	{
		if (this.Chore != null)
		{
			return;
		}
		this.hasDispenseChore = true;
		this.Chore = new WorkChore<PajamaDispenser>(Db.Get().ChoreTypes.EquipmentFetch, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, false);
		this.Chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0003F594 File Offset: 0x0003D794
	public void CancelFetch()
	{
		if (this.Chore == null)
		{
			return;
		}
		this.Chore.Cancel("User Cancelled");
		this.Chore = null;
		this.hasDispenseChore = false;
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, false);
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x0003F5E9 File Offset: 0x0003D7E9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.hasDispenseChore)
		{
			this.FetchPajamas();
		}
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x0003F5FF File Offset: 0x0003D7FF
	public List<Tag> DispensedItems()
	{
		return PajamaDispenser.PajamaList;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0003F606 File Offset: 0x0003D806
	public Tag SelectedItem()
	{
		return PajamaDispenser.PajamaList[0];
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0003F613 File Offset: 0x0003D813
	public void SelectItem(Tag tag)
	{
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0003F615 File Offset: 0x0003D815
	public void OnOrderDispense()
	{
		this.FetchPajamas();
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x0003F61D File Offset: 0x0003D81D
	public void OnCancelDispense()
	{
		this.CancelFetch();
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x0003F625 File Offset: 0x0003D825
	public bool HasOpenChore()
	{
		return this.Chore != null;
	}

	// Token: 0x04000703 RID: 1795
	[Serialize]
	private bool hasDispenseChore;

	// Token: 0x04000704 RID: 1796
	private static GameObject pajamaPrefab = null;

	// Token: 0x04000706 RID: 1798
	private WorkChore<PajamaDispenser> chore;

	// Token: 0x04000707 RID: 1799
	private static List<Tag> PajamaList = new List<Tag>
	{
		"SleepClinicPajamas"
	};
}
