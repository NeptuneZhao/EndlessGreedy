using System;
using TUNING;
using UnityEngine;

// Token: 0x020006C4 RID: 1732
[AddComponentMenu("KMonoBehaviour/Workable/EggIncubatorWorkable")]
public class EggIncubatorWorkable : Workable
{
	// Token: 0x06002BD2 RID: 11218 RVA: 0x000F6268 File Offset: 0x000F4468
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_incubator_kanim")
		};
		base.SetWorkTime(15f);
		this.showProgressBar = true;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.attributeConverter = Db.Get().AttributeConverters.RanchingEffectDuration;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}

	// Token: 0x06002BD3 RID: 11219 RVA: 0x000F6314 File Offset: 0x000F4514
	protected override void OnCompleteWork(WorkerBase worker)
	{
		EggIncubator component = base.GetComponent<EggIncubator>();
		if (component && component.Occupant)
		{
			IncubationMonitor.Instance smi = component.Occupant.GetSMI<IncubationMonitor.Instance>();
			if (smi != null)
			{
				smi.ApplySongBuff();
			}
		}
	}
}
