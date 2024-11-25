using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000557 RID: 1367
[RequireComponent(typeof(Prioritizable))]
public class Demolishable : Workable
{
	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06001F68 RID: 8040 RVA: 0x000B06F3 File Offset: 0x000AE8F3
	public bool HasBeenDestroyed
	{
		get
		{
			return this.destroyed;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06001F69 RID: 8041 RVA: 0x000B06FC File Offset: 0x000AE8FC
	private CellOffset[] placementOffsets
	{
		get
		{
			Building component = base.GetComponent<Building>();
			if (component != null)
			{
				return component.Def.PlacementOffsets;
			}
			OccupyArea component2 = base.GetComponent<OccupyArea>();
			if (component2 != null)
			{
				return component2.OccupiedCellsOffsets;
			}
			global::Debug.Assert(false, "Ack! We put a Demolishable on something that's neither a Building nor OccupyArea!", this);
			return null;
		}
	}

	// Token: 0x06001F6A RID: 8042 RVA: 0x000B074C File Offset: 0x000AE94C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanDemolish.Id;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Deconstructing;
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.minimumAttributeMultiplier = 0.75f;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.multitoolContext = "demolish";
		this.multitoolHitEffectTag = EffectConfigs.DemolishSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		Building component = base.GetComponent<Building>();
		if (component != null && component.Def.IsTilePiece)
		{
			base.SetWorkTime(component.Def.ConstructionTime * 0.5f);
			return;
		}
		base.SetWorkTime(30f);
	}

	// Token: 0x06001F6B RID: 8043 RVA: 0x000B085C File Offset: 0x000AEA5C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Demolishable>(493375141, Demolishable.OnRefreshUserMenuDelegate);
		base.Subscribe<Demolishable>(-111137758, Demolishable.OnRefreshUserMenuDelegate);
		base.Subscribe<Demolishable>(2127324410, Demolishable.OnCancelDelegate);
		base.Subscribe<Demolishable>(-790448070, Demolishable.OnDeconstructDelegate);
		CellOffset[][] table = OffsetGroups.InvertedStandardTable;
		CellOffset[] filter = null;
		Building component = base.GetComponent<Building>();
		if (component != null && component.Def.IsTilePiece)
		{
			table = OffsetGroups.InvertedStandardTableWithCorners;
			filter = component.Def.ConstructionOffsetFilter;
		}
		CellOffset[][] offsetTable = OffsetGroups.BuildReachabilityTable(this.placementOffsets, table, filter);
		base.SetOffsetTable(offsetTable);
		if (this.isMarkedForDemolition)
		{
			this.QueueDemolition();
		}
	}

	// Token: 0x06001F6C RID: 8044 RVA: 0x000B090D File Offset: 0x000AEB0D
	protected override void OnStartWork(WorkerBase worker)
	{
		this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("DeconstructBar");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDemolition, false);
	}

	// Token: 0x06001F6D RID: 8045 RVA: 0x000B0945 File Offset: 0x000AEB45
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.TriggerDestroy();
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x000B094D File Offset: 0x000AEB4D
	private void TriggerDestroy()
	{
		if (this == null || this.destroyed)
		{
			return;
		}
		this.destroyed = true;
		this.isMarkedForDemolition = false;
		base.gameObject.DeleteObject();
	}

	// Token: 0x06001F6F RID: 8047 RVA: 0x000B097C File Offset: 0x000AEB7C
	private void QueueDemolition()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
			return;
		}
		if (this.chore == null)
		{
			Prioritizable.AddRef(base.gameObject);
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDemolish.Id;
			this.chore = new WorkChore<Demolishable>(Db.Get().ChoreTypes.Demolish, this, null, true, null, null, null, true, null, false, false, Assets.GetAnim("anim_interacts_clothingfactory_kanim"), true, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingDemolition, this);
			this.isMarkedForDemolition = true;
			base.Trigger(2108245096, "Demolish");
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06001F70 RID: 8048 RVA: 0x000B0A3C File Offset: 0x000AEC3C
	private void OnRefreshUserMenu(object data)
	{
		if (!this.allowDemolition)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore == null) ? new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.DEMOLISH.NAME, new System.Action(this.OnDemolish), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DEMOLISH.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.DEMOLISH.NAME_OFF, new System.Action(this.OnDemolish), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DEMOLISH.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 0f);
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x000B0AE0 File Offset: 0x000AECE0
	public void CancelDemolition()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancelled demolition");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDemolition, false);
			base.ShowProgressBar(false);
			this.isMarkedForDemolition = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06001F72 RID: 8050 RVA: 0x000B0B48 File Offset: 0x000AED48
	private void OnCancel(object data)
	{
		this.CancelDemolition();
	}

	// Token: 0x06001F73 RID: 8051 RVA: 0x000B0B50 File Offset: 0x000AED50
	private void OnDemolish(object data)
	{
		if (this.allowDemolition || DebugHandler.InstantBuildMode)
		{
			this.QueueDemolition();
		}
	}

	// Token: 0x06001F74 RID: 8052 RVA: 0x000B0B67 File Offset: 0x000AED67
	private void OnDemolish()
	{
		if (this.chore == null)
		{
			this.QueueDemolition();
			return;
		}
		this.CancelDemolition();
	}

	// Token: 0x06001F75 RID: 8053 RVA: 0x000B0B7E File Offset: 0x000AED7E
	protected override void UpdateStatusItem(object data = null)
	{
		this.shouldShowSkillPerkStatusItem = this.isMarkedForDemolition;
		base.UpdateStatusItem(data);
	}

	// Token: 0x040011B4 RID: 4532
	public Chore chore;

	// Token: 0x040011B5 RID: 4533
	public bool allowDemolition = true;

	// Token: 0x040011B6 RID: 4534
	[Serialize]
	private bool isMarkedForDemolition;

	// Token: 0x040011B7 RID: 4535
	private bool destroyed;

	// Token: 0x040011B8 RID: 4536
	private static readonly EventSystem.IntraObjectHandler<Demolishable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Demolishable>(delegate(Demolishable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040011B9 RID: 4537
	private static readonly EventSystem.IntraObjectHandler<Demolishable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Demolishable>(delegate(Demolishable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x040011BA RID: 4538
	private static readonly EventSystem.IntraObjectHandler<Demolishable> OnDeconstructDelegate = new EventSystem.IntraObjectHandler<Demolishable>(delegate(Demolishable component, object data)
	{
		component.OnDemolish(data);
	});
}
