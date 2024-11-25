using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200087C RID: 2172
[AddComponentMenu("KMonoBehaviour/Workable/EmptySolidConduitWorkable")]
public class EmptySolidConduitWorkable : Workable, IEmptyConduitWorkable
{
	// Token: 0x06003CBC RID: 15548 RVA: 0x00150E7C File Offset: 0x0014F07C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		base.SetWorkTime(float.PositiveInfinity);
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		base.Subscribe<EmptySolidConduitWorkable>(2127324410, EmptySolidConduitWorkable.OnEmptyConduitCancelledDelegate);
		if (EmptySolidConduitWorkable.emptySolidConduitStatusItem == null)
		{
			EmptySolidConduitWorkable.emptySolidConduitStatusItem = new StatusItem("EmptySolidConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.SolidConveyor.ID, 32770, true, null);
		}
		this.requiredSkillPerk = Db.Get().SkillPerks.CanDoPlumbing.Id;
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x00150F3C File Offset: 0x0014F13C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elapsedTime != -1f)
		{
			this.MarkForEmptying();
		}
	}

	// Token: 0x06003CBE RID: 15550 RVA: 0x00150F58 File Offset: 0x0014F158
	public void MarkForEmptying()
	{
		if (this.chore == null && this.HasContents())
		{
			StatusItem statusItem = this.GetStatusItem();
			base.GetComponent<KSelectable>().ToggleStatusItem(statusItem, true, null);
			this.CreateWorkChore();
		}
	}

	// Token: 0x06003CBF RID: 15551 RVA: 0x00150F94 File Offset: 0x0014F194
	private bool HasContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.GetFlowManager().GetContents(cell).pickupableHandle.IsValid();
	}

	// Token: 0x06003CC0 RID: 15552 RVA: 0x00150FCB File Offset: 0x0014F1CB
	private void CancelEmptying()
	{
		this.CleanUpVisualization();
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel");
			this.chore = null;
			this.shouldShowSkillPerkStatusItem = false;
			this.UpdateStatusItem(null);
		}
	}

	// Token: 0x06003CC1 RID: 15553 RVA: 0x00151000 File Offset: 0x0014F200
	private void CleanUpVisualization()
	{
		StatusItem statusItem = this.GetStatusItem();
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.ToggleStatusItem(statusItem, false, null);
		}
		this.elapsedTime = -1f;
		if (this.chore != null)
		{
			base.GetComponent<Prioritizable>().RemoveRef();
		}
	}

	// Token: 0x06003CC2 RID: 15554 RVA: 0x0015104C File Offset: 0x0014F24C
	protected override void OnCleanUp()
	{
		this.CancelEmptying();
		base.OnCleanUp();
	}

	// Token: 0x06003CC3 RID: 15555 RVA: 0x0015105A File Offset: 0x0014F25A
	private SolidConduitFlow GetFlowManager()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x06003CC4 RID: 15556 RVA: 0x00151066 File Offset: 0x0014F266
	private void OnEmptyConduitCancelled(object data)
	{
		this.CancelEmptying();
	}

	// Token: 0x06003CC5 RID: 15557 RVA: 0x0015106E File Offset: 0x0014F26E
	private StatusItem GetStatusItem()
	{
		return EmptySolidConduitWorkable.emptySolidConduitStatusItem;
	}

	// Token: 0x06003CC6 RID: 15558 RVA: 0x00151078 File Offset: 0x0014F278
	private void CreateWorkChore()
	{
		base.GetComponent<Prioritizable>().AddRef();
		this.chore = new WorkChore<EmptySolidConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDoPlumbing.Id);
		this.elapsedTime = 0f;
		this.emptiedPipe = false;
		this.shouldShowSkillPerkStatusItem = true;
		this.UpdateStatusItem(null);
	}

	// Token: 0x06003CC7 RID: 15559 RVA: 0x00151108 File Offset: 0x0014F308
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.elapsedTime == -1f)
		{
			return true;
		}
		bool result = false;
		this.elapsedTime += dt;
		if (!this.emptiedPipe)
		{
			if (this.elapsedTime > 4f)
			{
				this.EmptyContents();
				this.emptiedPipe = true;
				this.elapsedTime = 0f;
			}
		}
		else if (this.elapsedTime > 2f)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			if (this.GetFlowManager().GetContents(cell).pickupableHandle.IsValid())
			{
				this.elapsedTime = 0f;
				this.emptiedPipe = false;
			}
			else
			{
				this.CleanUpVisualization();
				this.chore = null;
				result = true;
				this.shouldShowSkillPerkStatusItem = false;
				this.UpdateStatusItem(null);
			}
		}
		return result;
	}

	// Token: 0x06003CC8 RID: 15560 RVA: 0x001511D1 File Offset: 0x0014F3D1
	public override bool InstantlyFinish(WorkerBase worker)
	{
		worker.Work(4f);
		return true;
	}

	// Token: 0x06003CC9 RID: 15561 RVA: 0x001511E0 File Offset: 0x0014F3E0
	public void EmptyContents()
	{
		int cell_idx = Grid.PosToCell(base.transform.GetPosition());
		this.GetFlowManager().RemovePickupable(cell_idx);
		this.elapsedTime = 0f;
	}

	// Token: 0x06003CCA RID: 15562 RVA: 0x00151216 File Offset: 0x0014F416
	public override float GetPercentComplete()
	{
		return Mathf.Clamp01(this.elapsedTime / 4f);
	}

	// Token: 0x0400251D RID: 9501
	[MyCmpReq]
	private SolidConduit conduit;

	// Token: 0x0400251E RID: 9502
	private static StatusItem emptySolidConduitStatusItem;

	// Token: 0x0400251F RID: 9503
	private Chore chore;

	// Token: 0x04002520 RID: 9504
	private const float RECHECK_PIPE_INTERVAL = 2f;

	// Token: 0x04002521 RID: 9505
	private const float TIME_TO_EMPTY_PIPE = 4f;

	// Token: 0x04002522 RID: 9506
	private const float NO_EMPTY_SCHEDULED = -1f;

	// Token: 0x04002523 RID: 9507
	[Serialize]
	private float elapsedTime = -1f;

	// Token: 0x04002524 RID: 9508
	private bool emptiedPipe = true;

	// Token: 0x04002525 RID: 9509
	private static readonly EventSystem.IntraObjectHandler<EmptySolidConduitWorkable> OnEmptyConduitCancelledDelegate = new EventSystem.IntraObjectHandler<EmptySolidConduitWorkable>(delegate(EmptySolidConduitWorkable component, object data)
	{
		component.OnEmptyConduitCancelled(data);
	});
}
