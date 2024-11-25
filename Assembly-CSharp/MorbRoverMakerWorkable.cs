using System;
using TUNING;

// Token: 0x02000304 RID: 772
public class MorbRoverMakerWorkable : Workable
{
	// Token: 0x0600103A RID: 4154 RVA: 0x0005BD5C File Offset: 0x00059F5C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workingStatusItem = Db.Get().BuildingStatusItems.MorbRoverMakerDoctorWorking;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.MorbRoverMakerDoctorWorking);
		this.requiredSkillPerk = Db.Get().SkillPerks.CanAdvancedMedicine.Id;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gravitas_morb_tank_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = true;
		this.shouldShowSkillPerkStatusItem = true;
		base.SetWorkTime(90f);
		this.resetProgressOnStop = true;
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x0005BE23 File Offset: 0x0005A023
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x0005BE2B File Offset: 0x0005A02B
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x040009E0 RID: 2528
	public const float DOCTOR_WORKING_TIME = 90f;
}
