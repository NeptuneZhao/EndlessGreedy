using System;
using TUNING;
using UnityEngine;

// Token: 0x0200069C RID: 1692
[AddComponentMenu("KMonoBehaviour/Workable/CompostWorkable")]
public class CompostWorkable : Workable
{
	// Token: 0x06002A83 RID: 10883 RVA: 0x000F0428 File Offset: 0x000EE628
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x06002A84 RID: 10884 RVA: 0x000F0480 File Offset: 0x000EE680
	protected override void OnStartWork(WorkerBase worker)
	{
	}

	// Token: 0x06002A85 RID: 10885 RVA: 0x000F0482 File Offset: 0x000EE682
	protected override void OnStopWork(WorkerBase worker)
	{
	}
}
