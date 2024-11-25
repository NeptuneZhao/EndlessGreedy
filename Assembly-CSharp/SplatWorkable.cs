using System;
using TUNING;

// Token: 0x02000B17 RID: 2839
public class SplatWorkable : Workable
{
	// Token: 0x0600548D RID: 21645 RVA: 0x001E3DB4 File Offset: 0x001E1FB4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Mopping;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "disinfect";
		this.multitoolHitEffectTag = "fx_disinfect_splash";
		this.synchronizeAnims = false;
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x0600548E RID: 21646 RVA: 0x001E3E5E File Offset: 0x001E205E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(5f);
	}
}
