using System;
using TUNING;
using UnityEngine;

// Token: 0x020006DC RID: 1756
public class GeneticAnalysisStationWorkable : Workable
{
	// Token: 0x06002C86 RID: 11398 RVA: 0x000F9C18 File Offset: 0x000F7E18
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanIdentifyMutantSeeds.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.AnalyzingGenes;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_genetic_analysisstation_kanim")
		};
		base.SetWorkTime(150f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06002C87 RID: 11399 RVA: 0x000F9CD6 File Offset: 0x000F7ED6
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.storage.FindFirst(GameTags.UnidentifiedSeed));
	}

	// Token: 0x06002C88 RID: 11400 RVA: 0x000F9D0A File Offset: 0x000F7F0A
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, false);
	}

	// Token: 0x06002C89 RID: 11401 RVA: 0x000F9D2F File Offset: 0x000F7F2F
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.IdentifyMutant();
	}

	// Token: 0x06002C8A RID: 11402 RVA: 0x000F9D40 File Offset: 0x000F7F40
	public void IdentifyMutant()
	{
		GameObject gameObject = this.storage.FindFirst(GameTags.UnidentifiedSeed);
		DebugUtil.DevAssertArgs(gameObject != null, new object[]
		{
			"AAACCCCKKK!! GeneticAnalysisStation finished studying a seed but we don't have one in storage??"
		});
		if (gameObject != null)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			Pickupable pickupable;
			if (component.PrimaryElement.Units > 1f)
			{
				pickupable = component.Take(1f);
			}
			else
			{
				pickupable = this.storage.Drop(gameObject, true).GetComponent<Pickupable>();
			}
			pickupable.transform.SetPosition(base.transform.GetPosition() + this.finishedSeedDropOffset);
			MutantPlant component2 = pickupable.GetComponent<MutantPlant>();
			PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(component2.SubSpeciesID);
			component2.Analyze();
			SaveGame.Instance.ColonyAchievementTracker.LogAnalyzedSeed(component2.SpeciesID);
		}
	}

	// Token: 0x040019B0 RID: 6576
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x040019B1 RID: 6577
	[MyCmpReq]
	public Storage storage;

	// Token: 0x040019B2 RID: 6578
	[SerializeField]
	public Vector3 finishedSeedDropOffset;

	// Token: 0x040019B3 RID: 6579
	private Notification notification;

	// Token: 0x040019B4 RID: 6580
	public GeneticAnalysisStation.StatesInstance statesInstance;
}
