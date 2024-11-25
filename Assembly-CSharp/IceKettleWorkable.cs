using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006F3 RID: 1779
public class IceKettleWorkable : Workable
{
	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06002D6A RID: 11626 RVA: 0x000FEF3B File Offset: 0x000FD13B
	// (set) Token: 0x06002D6B RID: 11627 RVA: 0x000FEF43 File Offset: 0x000FD143
	public MeterController meter { get; private set; }

	// Token: 0x06002D6C RID: 11628 RVA: 0x000FEF4C File Offset: 0x000FD14C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_icemelter_kettle_kanim")
		};
		this.synchronizeAnims = true;
		base.SetOffsets(new CellOffset[]
		{
			this.workCellOffset
		});
		base.SetWorkTime(5f);
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
		this.storage.onDestroyItemsDropped = new Action<List<GameObject>>(this.RestoreStoredItemsInteractions);
		this.handler = base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
	}

	// Token: 0x06002D6D RID: 11629 RVA: 0x000FF026 File Offset: 0x000FD226
	protected override void OnSpawn()
	{
		this.AdjustStoredItemsPositionsAndWorkable();
	}

	// Token: 0x06002D6E RID: 11630 RVA: 0x000FF030 File Offset: 0x000FD230
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		this.meter.gameObject.SetActive(true);
		PrimaryElement component = pickupableStartWorkInfo.originalPickupable.GetComponent<PrimaryElement>();
		this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), component.Element.substance.colour);
		this.meter.SetSymbolTint(new KAnimHashedString("water1"), component.Element.substance.colour);
	}

	// Token: 0x06002D6F RID: 11631 RVA: 0x000FF0B8 File Offset: 0x000FD2B8
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float value = (this.workTime - base.WorkTimeRemaining) / this.workTime;
		this.meter.SetPositionPercent(Mathf.Clamp01(value));
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06002D70 RID: 11632 RVA: 0x000FF0F4 File Offset: 0x000FD2F4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		if (pickupableStartWorkInfo.amount > 0f)
		{
			this.storage.TransferMass(component, pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID(), pickupableStartWorkInfo.amount, false, false, false);
		}
		GameObject gameObject = component.FindFirst(pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID());
		if (gameObject != null)
		{
			pickupableStartWorkInfo.setResultCb(gameObject);
		}
		else
		{
			pickupableStartWorkInfo.setResultCb(null);
		}
		base.OnCompleteWork(worker);
		foreach (GameObject gameObject2 in component.items)
		{
			if (gameObject2.HasTag(GameTags.Liquid))
			{
				Pickupable component2 = gameObject2.GetComponent<Pickupable>();
				this.RestorePickupableInteractions(component2);
			}
		}
	}

	// Token: 0x06002D71 RID: 11633 RVA: 0x000FF1E8 File Offset: 0x000FD3E8
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.meter.gameObject.SetActive(false);
	}

	// Token: 0x06002D72 RID: 11634 RVA: 0x000FF202 File Offset: 0x000FD402
	private void OnStorageChanged(object obj)
	{
		this.AdjustStoredItemsPositionsAndWorkable();
	}

	// Token: 0x06002D73 RID: 11635 RVA: 0x000FF20C File Offset: 0x000FD40C
	private void AdjustStoredItemsPositionsAndWorkable()
	{
		int cell = Grid.PosToCell(this);
		Vector3 position = Grid.CellToPosCCC(Grid.OffsetCell(cell, new CellOffset(0, 0)), Grid.SceneLayer.Ore);
		foreach (GameObject gameObject in this.storage.items)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			component.transform.SetPosition(position);
			component.UpdateCachedCell(cell);
			this.OverridePickupableInteractions(component);
		}
	}

	// Token: 0x06002D74 RID: 11636 RVA: 0x000FF29C File Offset: 0x000FD49C
	private void OverridePickupableInteractions(Pickupable pickupable)
	{
		pickupable.AddTag(GameTags.LiquidSource);
		pickupable.targetWorkable = this;
		pickupable.SetOffsets(new CellOffset[]
		{
			this.workCellOffset
		});
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x000FF2C9 File Offset: 0x000FD4C9
	private void RestorePickupableInteractions(Pickupable pickupable)
	{
		pickupable.RemoveTag(GameTags.LiquidSource);
		pickupable.targetWorkable = pickupable;
		pickupable.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06002D76 RID: 11638 RVA: 0x000FF2E8 File Offset: 0x000FD4E8
	private void RestoreStoredItemsInteractions(List<GameObject> specificItems = null)
	{
		specificItems = ((specificItems == null) ? this.storage.items : specificItems);
		foreach (GameObject gameObject in specificItems)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			this.RestorePickupableInteractions(component);
		}
	}

	// Token: 0x06002D77 RID: 11639 RVA: 0x000FF350 File Offset: 0x000FD550
	protected override void OnCleanUp()
	{
		if (base.worker != null)
		{
			ChoreDriver component = base.worker.GetComponent<ChoreDriver>();
			base.worker.StopWork();
			component.StopChore();
		}
		this.RestoreStoredItemsInteractions(null);
		base.Unsubscribe(this.handler);
		base.OnCleanUp();
	}

	// Token: 0x04001A5A RID: 6746
	public Storage storage;

	// Token: 0x04001A5B RID: 6747
	private int handler;

	// Token: 0x04001A5D RID: 6749
	public CellOffset workCellOffset = new CellOffset(0, 0);
}
