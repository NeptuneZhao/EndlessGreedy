using System;
using TUNING;
using UnityEngine;

// Token: 0x02000734 RID: 1844
public class MissionControlClusterWorkable : Workable
{
	// Token: 0x1700031F RID: 799
	// (get) Token: 0x060030F3 RID: 12531 RVA: 0x0010E4D5 File Offset: 0x0010C6D5
	// (set) Token: 0x060030F4 RID: 12532 RVA: 0x0010E4DD File Offset: 0x0010C6DD
	public Clustercraft TargetClustercraft
	{
		get
		{
			return this.targetClustercraft;
		}
		set
		{
			base.WorkTimeRemaining = this.GetWorkTime();
			this.targetClustercraft = value;
		}
	}

	// Token: 0x060030F5 RID: 12533 RVA: 0x0010E4F4 File Offset: 0x0010C6F4
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

	// Token: 0x060030F6 RID: 12534 RVA: 0x0010E5B2 File Offset: 0x0010C7B2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MissionControlClusterWorkables.Add(this);
	}

	// Token: 0x060030F7 RID: 12535 RVA: 0x0010E5C5 File Offset: 0x0010C7C5
	protected override void OnCleanUp()
	{
		Components.MissionControlClusterWorkables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060030F8 RID: 12536 RVA: 0x0010E5D8 File Offset: 0x0010C7D8
	public static bool IsRocketInRange(AxialI worldLocation, AxialI rocketLocation)
	{
		return AxialUtil.GetDistance(worldLocation, rocketLocation) <= 2;
	}

	// Token: 0x060030F9 RID: 12537 RVA: 0x0010E5E8 File Offset: 0x0010C7E8
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.workStatusItem = base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.MissionControlAssistingRocket, this.TargetClustercraft);
		this.operational.SetActive(true, false);
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x0010E634 File Offset: 0x0010C834
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.GetSMI<SkyVisibilityMonitor.Instance>().PercentClearSky);
	}

	// Token: 0x060030FB RID: 12539 RVA: 0x0010E64E File Offset: 0x0010C84E
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.TargetClustercraft == null || !MissionControlClusterWorkable.IsRocketInRange(base.gameObject.GetMyWorldLocation(), this.TargetClustercraft.Location))
		{
			worker.StopWork();
			return true;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x0010E68B File Offset: 0x0010C88B
	protected override void OnCompleteWork(WorkerBase worker)
	{
		global::Debug.Assert(this.TargetClustercraft != null);
		base.gameObject.GetSMI<MissionControlCluster.Instance>().ApplyEffect(this.TargetClustercraft);
		base.OnCompleteWork(worker);
	}

	// Token: 0x060030FD RID: 12541 RVA: 0x0010E6BB File Offset: 0x0010C8BB
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.workStatusItem, false);
		this.TargetClustercraft = null;
		this.operational.SetActive(false, false);
	}

	// Token: 0x04001CBD RID: 7357
	private Clustercraft targetClustercraft;

	// Token: 0x04001CBE RID: 7358
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001CBF RID: 7359
	private Guid workStatusItem = Guid.Empty;
}
