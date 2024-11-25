using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020008E8 RID: 2280
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Harvestable")]
public class Harvestable : Workable
{
	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x0600416C RID: 16748 RVA: 0x00173815 File Offset: 0x00171A15
	// (set) Token: 0x0600416D RID: 16749 RVA: 0x0017381D File Offset: 0x00171A1D
	public WorkerBase completed_by { get; protected set; }

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x0600416E RID: 16750 RVA: 0x00173826 File Offset: 0x00171A26
	public bool CanBeHarvested
	{
		get
		{
			return this.canBeHarvested;
		}
	}

	// Token: 0x0600416F RID: 16751 RVA: 0x0017382E File Offset: 0x00171A2E
	protected Harvestable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06004170 RID: 16752 RVA: 0x00173856 File Offset: 0x00171A56
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Harvesting;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
	}

	// Token: 0x06004171 RID: 16753 RVA: 0x00173894 File Offset: 0x00171A94
	protected override void OnSpawn()
	{
		this.harvestDesignatable = base.GetComponent<HarvestDesignatable>();
		base.Subscribe<Harvestable>(2127324410, Harvestable.ForceCancelHarvestDelegate);
		base.SetWorkTime(10f);
		base.Subscribe<Harvestable>(2127324410, Harvestable.OnCancelDelegate);
		this.faceTargetWhenWorking = true;
		Components.Harvestables.Add(this);
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x06004172 RID: 16754 RVA: 0x00173931 File Offset: 0x00171B31
	public void OnUprooted(object data)
	{
		if (this.canBeHarvested)
		{
			this.Harvest();
		}
	}

	// Token: 0x06004173 RID: 16755 RVA: 0x00173944 File Offset: 0x00171B44
	public void Harvest()
	{
		this.harvestDesignatable.MarkedForHarvest = false;
		this.chore = null;
		base.Trigger(1272413801, this);
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
		component.RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06004174 RID: 16756 RVA: 0x001739B8 File Offset: 0x00171BB8
	public void OnMarkedForHarvest()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.chore == null)
		{
			this.chore = new WorkChore<Harvestable>(Db.Get().ChoreTypes.Harvest, this, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			component.AddStatusItem(Db.Get().MiscStatusItems.PendingHarvest, this);
		}
		component.RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, false);
	}

	// Token: 0x06004175 RID: 16757 RVA: 0x00173A30 File Offset: 0x00171C30
	public void SetCanBeHarvested(bool state)
	{
		this.canBeHarvested = state;
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.canBeHarvested)
		{
			component.AddStatusItem(this.readyForHarvestStatusItem, null);
			if (this.harvestDesignatable.HarvestWhenReady)
			{
				this.harvestDesignatable.MarkForHarvest();
			}
			else if (this.harvestDesignatable.InPlanterBox)
			{
				component.AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, this);
			}
		}
		else
		{
			component.RemoveStatusItem(this.readyForHarvestStatusItem, false);
			component.RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, false);
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06004176 RID: 16758 RVA: 0x00173ADB File Offset: 0x00171CDB
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.completed_by = worker;
		this.Harvest();
	}

	// Token: 0x06004177 RID: 16759 RVA: 0x00173AEC File Offset: 0x00171CEC
	protected virtual void OnCancel(object data)
	{
		bool flag = data == null || (data is bool && !(bool)data);
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel harvest");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
			if (flag)
			{
				this.harvestDesignatable.SetHarvestWhenReady(false);
			}
		}
		if (flag)
		{
			this.harvestDesignatable.MarkedForHarvest = false;
		}
	}

	// Token: 0x06004178 RID: 16760 RVA: 0x00173B6D File Offset: 0x00171D6D
	public bool HasChore()
	{
		return this.chore != null;
	}

	// Token: 0x06004179 RID: 16761 RVA: 0x00173B7A File Offset: 0x00171D7A
	public virtual void ForceCancelHarvest(object data = null)
	{
		this.OnCancel(data);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x0600417A RID: 16762 RVA: 0x00173BB4 File Offset: 0x00171DB4
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Harvestables.Remove(this);
	}

	// Token: 0x0600417B RID: 16763 RVA: 0x00173BC7 File Offset: 0x00171DC7
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
	}

	// Token: 0x04002B5C RID: 11100
	public StatusItem readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest;

	// Token: 0x04002B5D RID: 11101
	public HarvestDesignatable harvestDesignatable;

	// Token: 0x04002B5E RID: 11102
	[Serialize]
	protected bool canBeHarvested;

	// Token: 0x04002B60 RID: 11104
	protected Chore chore;

	// Token: 0x04002B61 RID: 11105
	private static readonly EventSystem.IntraObjectHandler<Harvestable> ForceCancelHarvestDelegate = new EventSystem.IntraObjectHandler<Harvestable>(delegate(Harvestable component, object data)
	{
		component.ForceCancelHarvest(data);
	});

	// Token: 0x04002B62 RID: 11106
	private static readonly EventSystem.IntraObjectHandler<Harvestable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Harvestable>(delegate(Harvestable component, object data)
	{
		component.OnCancel(data);
	});
}
