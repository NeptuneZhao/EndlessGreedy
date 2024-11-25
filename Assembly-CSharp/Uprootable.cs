using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B40 RID: 2880
[AddComponentMenu("KMonoBehaviour/Workable/Uprootable")]
public class Uprootable : Workable, IDigActionEntity
{
	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x060055FC RID: 22012 RVA: 0x001EC329 File Offset: 0x001EA529
	public bool IsMarkedForUproot
	{
		get
		{
			return this.isMarkedForUproot;
		}
	}

	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x060055FD RID: 22013 RVA: 0x001EC331 File Offset: 0x001EA531
	public Storage GetPlanterStorage
	{
		get
		{
			return this.planterStorage;
		}
	}

	// Token: 0x060055FE RID: 22014 RVA: 0x001EC33C File Offset: 0x001EA53C
	protected Uprootable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.buttonLabel = UI.USERMENUACTIONS.UPROOT.NAME;
		this.buttonTooltip = UI.USERMENUACTIONS.UPROOT.TOOLTIP;
		this.cancelButtonLabel = UI.USERMENUACTIONS.CANCELUPROOT.NAME;
		this.cancelButtonTooltip = UI.USERMENUACTIONS.CANCELUPROOT.TOOLTIP;
		this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
	}

	// Token: 0x060055FF RID: 22015 RVA: 0x001EC3DC File Offset: 0x001EA5DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
		base.Subscribe<Uprootable>(1309017699, Uprootable.OnPlanterStorageDelegate);
	}

	// Token: 0x06005600 RID: 22016 RVA: 0x001EC490 File Offset: 0x001EA690
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Uprootable>(2127324410, Uprootable.ForceCancelUprootDelegate);
		base.SetWorkTime(12.5f);
		base.Subscribe<Uprootable>(2127324410, Uprootable.OnCancelDelegate);
		base.Subscribe<Uprootable>(493375141, Uprootable.OnRefreshUserMenuDelegate);
		this.faceTargetWhenWorking = true;
		Components.Uprootables.Add(this);
		this.area = base.GetComponent<OccupyArea>();
		Prioritizable.AddRef(base.gameObject);
		base.gameObject.AddTag(GameTags.Plant);
		Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets);
		this.partitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.gameObject.GetComponent<KPrefabID>(), extents, GameScenePartitioner.Instance.plants, null);
		if (this.isMarkedForUproot)
		{
			this.MarkForUproot(true);
		}
	}

	// Token: 0x06005601 RID: 22017 RVA: 0x001EC580 File Offset: 0x001EA780
	private void OnPlanterStorage(object data)
	{
		this.planterStorage = (Storage)data;
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.showIcon = (this.planterStorage == null);
		}
	}

	// Token: 0x06005602 RID: 22018 RVA: 0x001EC5BB File Offset: 0x001EA7BB
	public bool IsInPlanterBox()
	{
		return this.planterStorage != null;
	}

	// Token: 0x06005603 RID: 22019 RVA: 0x001EC5CC File Offset: 0x001EA7CC
	public void Uproot()
	{
		this.isMarkedForUproot = false;
		this.chore = null;
		this.uprootComplete = true;
		base.Trigger(-216549700, this);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06005604 RID: 22020 RVA: 0x001EC647 File Offset: 0x001EA847
	public void SetCanBeUprooted(bool state)
	{
		this.canBeUprooted = state;
		if (this.canBeUprooted)
		{
			this.SetUprootedComplete(false);
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06005605 RID: 22021 RVA: 0x001EC674 File Offset: 0x001EA874
	public void SetUprootedComplete(bool state)
	{
		this.uprootComplete = state;
	}

	// Token: 0x06005606 RID: 22022 RVA: 0x001EC680 File Offset: 0x001EA880
	public void MarkForUproot(bool instantOnDebug = true)
	{
		if (!this.canBeUprooted)
		{
			return;
		}
		if (DebugHandler.InstantBuildMode && instantOnDebug)
		{
			this.Uproot();
		}
		else if (this.chore == null)
		{
			this.chore = new WorkChore<Uprootable>(Db.Get().ChoreTypes.Uproot, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			base.GetComponent<KSelectable>().AddStatusItem(this.pendingStatusItem, this);
		}
		this.isMarkedForUproot = true;
	}

	// Token: 0x06005607 RID: 22023 RVA: 0x001EC6F5 File Offset: 0x001EA8F5
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.Uproot();
	}

	// Token: 0x06005608 RID: 22024 RVA: 0x001EC700 File Offset: 0x001EA900
	private void OnCancel(object data)
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel uproot");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
		}
		this.isMarkedForUproot = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06005609 RID: 22025 RVA: 0x001EC764 File Offset: 0x001EA964
	public bool HasChore()
	{
		return this.chore != null;
	}

	// Token: 0x0600560A RID: 22026 RVA: 0x001EC771 File Offset: 0x001EA971
	private void OnClickUproot()
	{
		this.MarkForUproot(true);
	}

	// Token: 0x0600560B RID: 22027 RVA: 0x001EC77A File Offset: 0x001EA97A
	protected void OnClickCancelUproot()
	{
		this.OnCancel(null);
	}

	// Token: 0x0600560C RID: 22028 RVA: 0x001EC783 File Offset: 0x001EA983
	public virtual void ForceCancelUproot(object data = null)
	{
		this.OnCancel(null);
	}

	// Token: 0x0600560D RID: 22029 RVA: 0x001EC78C File Offset: 0x001EA98C
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showUserMenuButtons)
		{
			return;
		}
		if (this.uprootComplete)
		{
			if (this.deselectOnUproot)
			{
				KSelectable component = base.GetComponent<KSelectable>();
				if (component != null && SelectTool.Instance.selected == component)
				{
					SelectTool.Instance.Select(null, false);
				}
			}
			return;
		}
		if (!this.canBeUprooted)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_uproot", this.cancelButtonLabel, new System.Action(this.OnClickCancelUproot), global::Action.NumActions, null, null, null, this.cancelButtonTooltip, true) : new KIconButtonMenu.ButtonInfo("action_uproot", this.buttonLabel, new System.Action(this.OnClickUproot), global::Action.NumActions, null, null, null, this.buttonTooltip, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x0600560E RID: 22030 RVA: 0x001EC866 File Offset: 0x001EAA66
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		Components.Uprootables.Remove(this);
	}

	// Token: 0x0600560F RID: 22031 RVA: 0x001EC889 File Offset: 0x001EAA89
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
	}

	// Token: 0x06005610 RID: 22032 RVA: 0x001EC8AE File Offset: 0x001EAAAE
	public void Dig()
	{
		this.Uproot();
	}

	// Token: 0x06005611 RID: 22033 RVA: 0x001EC8B6 File Offset: 0x001EAAB6
	public void MarkForDig(bool instantOnDebug = true)
	{
		this.MarkForUproot(instantOnDebug);
	}

	// Token: 0x0400384F RID: 14415
	[Serialize]
	protected bool isMarkedForUproot;

	// Token: 0x04003850 RID: 14416
	protected bool uprootComplete;

	// Token: 0x04003851 RID: 14417
	[MyCmpReq]
	private Prioritizable prioritizable;

	// Token: 0x04003852 RID: 14418
	[Serialize]
	protected bool canBeUprooted = true;

	// Token: 0x04003853 RID: 14419
	public bool deselectOnUproot = true;

	// Token: 0x04003854 RID: 14420
	protected Chore chore;

	// Token: 0x04003855 RID: 14421
	private string buttonLabel;

	// Token: 0x04003856 RID: 14422
	private string buttonTooltip;

	// Token: 0x04003857 RID: 14423
	private string cancelButtonLabel;

	// Token: 0x04003858 RID: 14424
	private string cancelButtonTooltip;

	// Token: 0x04003859 RID: 14425
	private StatusItem pendingStatusItem;

	// Token: 0x0400385A RID: 14426
	public OccupyArea area;

	// Token: 0x0400385B RID: 14427
	private Storage planterStorage;

	// Token: 0x0400385C RID: 14428
	public bool showUserMenuButtons = true;

	// Token: 0x0400385D RID: 14429
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400385E RID: 14430
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnPlanterStorageDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnPlanterStorage(data);
	});

	// Token: 0x0400385F RID: 14431
	private static readonly EventSystem.IntraObjectHandler<Uprootable> ForceCancelUprootDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.ForceCancelUproot(data);
	});

	// Token: 0x04003860 RID: 14432
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04003861 RID: 14433
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
