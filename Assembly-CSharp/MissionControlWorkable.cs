using System;
using TUNING;
using UnityEngine;

// Token: 0x02000735 RID: 1845
public class MissionControlWorkable : Workable
{
	// Token: 0x17000320 RID: 800
	// (get) Token: 0x060030FF RID: 12543 RVA: 0x0010E703 File Offset: 0x0010C903
	// (set) Token: 0x06003100 RID: 12544 RVA: 0x0010E70B File Offset: 0x0010C90B
	public Spacecraft TargetSpacecraft
	{
		get
		{
			return this.targetSpacecraft;
		}
		set
		{
			base.WorkTimeRemaining = this.GetWorkTime();
			this.targetSpacecraft = value;
		}
	}

	// Token: 0x06003101 RID: 12545 RVA: 0x0010E720 File Offset: 0x0010C920
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.MissionControlling;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_mission_control_station_kanim")
		};
		base.SetWorkTime(90f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06003102 RID: 12546 RVA: 0x0010E7DE File Offset: 0x0010C9DE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MissionControlWorkables.Add(this);
	}

	// Token: 0x06003103 RID: 12547 RVA: 0x0010E7F1 File Offset: 0x0010C9F1
	protected override void OnCleanUp()
	{
		Components.MissionControlWorkables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003104 RID: 12548 RVA: 0x0010E804 File Offset: 0x0010CA04
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.workStatusItem = base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.MissionControlAssistingRocket, this.TargetSpacecraft);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06003105 RID: 12549 RVA: 0x0010E850 File Offset: 0x0010CA50
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.GetSMI<SkyVisibilityMonitor.Instance>().PercentClearSky);
	}

	// Token: 0x06003106 RID: 12550 RVA: 0x0010E86A File Offset: 0x0010CA6A
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.TargetSpacecraft == null)
		{
			worker.StopWork();
			return true;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06003107 RID: 12551 RVA: 0x0010E884 File Offset: 0x0010CA84
	protected override void OnCompleteWork(WorkerBase worker)
	{
		global::Debug.Assert(this.TargetSpacecraft != null);
		base.gameObject.GetSMI<MissionControl.Instance>().ApplyEffect(this.TargetSpacecraft);
		base.OnCompleteWork(worker);
	}

	// Token: 0x06003108 RID: 12552 RVA: 0x0010E8B1 File Offset: 0x0010CAB1
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.workStatusItem, false);
		this.TargetSpacecraft = null;
		this.operational.SetActive(false, false);
	}

	// Token: 0x04001CC0 RID: 7360
	private Spacecraft targetSpacecraft;

	// Token: 0x04001CC1 RID: 7361
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001CC2 RID: 7362
	private Guid workStatusItem = Guid.Empty;
}
