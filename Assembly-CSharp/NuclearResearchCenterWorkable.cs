using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020009C9 RID: 2505
public class NuclearResearchCenterWorkable : Workable
{
	// Token: 0x060048C1 RID: 18625 RVA: 0x001A0024 File Offset: 0x0019E224
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Researching;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
		this.radiationStorage = base.GetComponent<HighEnergyParticleStorage>();
		this.nrc = base.GetComponent<NuclearResearchCenter>();
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x060048C2 RID: 18626 RVA: 0x001A00B0 File Offset: 0x0019E2B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x060048C3 RID: 18627 RVA: 0x001A00C4 File Offset: 0x0019E2C4
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float num = dt / this.nrc.timePerPoint;
		if (Game.Instance.FastWorkersModeActive)
		{
			num *= 2f;
		}
		this.radiationStorage.ConsumeAndGet(num * this.nrc.materialPerPoint);
		this.pointsProduced += num;
		if (this.pointsProduced >= 1f)
		{
			int num2 = Mathf.FloorToInt(this.pointsProduced);
			this.pointsProduced -= (float)num2;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, Research.Instance.GetResearchType("nuclear").name, base.transform, 1.5f, false);
			Research.Instance.AddResearchPoints("nuclear", (float)num2);
		}
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		return this.radiationStorage.IsEmpty() || activeResearch == null || activeResearch.PercentageCompleteResearchType("nuclear") >= 1f;
	}

	// Token: 0x060048C4 RID: 18628 RVA: 0x001A01BA File Offset: 0x0019E3BA
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.nrc);
	}

	// Token: 0x060048C5 RID: 18629 RVA: 0x001A01E4 File Offset: 0x0019E3E4
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
	}

	// Token: 0x060048C6 RID: 18630 RVA: 0x001A01ED File Offset: 0x0019E3ED
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.nrc);
	}

	// Token: 0x060048C7 RID: 18631 RVA: 0x001A021C File Offset: 0x0019E41C
	public override float GetPercentComplete()
	{
		if (Research.Instance.GetActiveResearch() == null)
		{
			return 0f;
		}
		float num = Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID["nuclear"];
		float num2 = 0f;
		if (!Research.Instance.GetActiveResearch().tech.costsByResearchTypeID.TryGetValue("nuclear", out num2))
		{
			return 1f;
		}
		return num / num2;
	}

	// Token: 0x060048C8 RID: 18632 RVA: 0x001A028B File Offset: 0x0019E48B
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x04002FA1 RID: 12193
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002FA2 RID: 12194
	[Serialize]
	private float pointsProduced;

	// Token: 0x04002FA3 RID: 12195
	private NuclearResearchCenter nrc;

	// Token: 0x04002FA4 RID: 12196
	private HighEnergyParticleStorage radiationStorage;
}
