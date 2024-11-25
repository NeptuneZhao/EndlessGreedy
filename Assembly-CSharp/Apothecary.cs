using System;
using TUNING;

// Token: 0x0200067F RID: 1663
public class Apothecary : ComplexFabricator
{
	// Token: 0x06002926 RID: 10534 RVA: 0x000E8CD5 File Offset: 0x000E6ED5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.Compound;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
		this.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
	}

	// Token: 0x06002927 RID: 10535 RVA: 0x000E8D14 File Offset: 0x000E6F14
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
			Assets.GetAnim("anim_interacts_apothecary_kanim")
		};
	}
}
