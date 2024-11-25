using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000788 RID: 1928
[AddComponentMenu("KMonoBehaviour/Workable/ToiletWorkableClean")]
public class ToiletWorkableClean : Workable
{
	// Token: 0x06003488 RID: 13448 RVA: 0x0011E728 File Offset: 0x0011C928
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.workAnims = ToiletWorkableClean.CLEAN_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			ToiletWorkableClean.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			ToiletWorkableClean.PST_ANIM
		};
	}

	// Token: 0x06003489 RID: 13449 RVA: 0x0011E7E5 File Offset: 0x0011C9E5
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.timesCleaned++;
		base.OnCompleteWork(worker);
	}

	// Token: 0x04001F0A RID: 7946
	[Serialize]
	public int timesCleaned;

	// Token: 0x04001F0B RID: 7947
	private static readonly HashedString[] CLEAN_ANIMS = new HashedString[]
	{
		"unclog_pre",
		"unclog_loop"
	};

	// Token: 0x04001F0C RID: 7948
	private static readonly HashedString PST_ANIM = new HashedString("unclog_pst");
}
