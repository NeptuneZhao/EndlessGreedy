using System;
using TUNING;

// Token: 0x0200067A RID: 1658
public class AdvancedApothecary : ComplexFabricator
{
	// Token: 0x0600290A RID: 10506 RVA: 0x000E81D9 File Offset: 0x000E63D9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.Compound;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
		this.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
	}

	// Token: 0x0600290B RID: 10507 RVA: 0x000E8218 File Offset: 0x000E6418
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CompoundingSpeed;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanCompound.Id;
		this.workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_medicine_nuclear_kanim")
		};
	}
}
