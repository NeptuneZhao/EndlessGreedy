using System;
using TUNING;
using UnityEngine;

// Token: 0x02000693 RID: 1683
[AddComponentMenu("KMonoBehaviour/Workable/DoctorChoreWorkable")]
public class DoctorChoreWorkable : Workable
{
	// Token: 0x06002A04 RID: 10756 RVA: 0x000ECC3A File Offset: 0x000EAE3A
	private DoctorChoreWorkable()
	{
		this.synchronizeAnims = false;
	}

	// Token: 0x06002A05 RID: 10757 RVA: 0x000ECC4C File Offset: 0x000EAE4C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}
}
