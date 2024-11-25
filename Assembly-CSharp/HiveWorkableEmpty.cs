using System;
using TUNING;
using UnityEngine;

// Token: 0x020006EF RID: 1775
[AddComponentMenu("KMonoBehaviour/Workable/HiveWorkableEmpty")]
public class HiveWorkableEmpty : Workable
{
	// Token: 0x06002D4B RID: 11595 RVA: 0x000FE420 File Offset: 0x000FC620
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.workAnims = HiveWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			HiveWorkableEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			HiveWorkableEmpty.PST_ANIM
		};
	}

	// Token: 0x06002D4C RID: 11596 RVA: 0x000FE4C8 File Offset: 0x000FC6C8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		if (!this.wasStung)
		{
			SaveGame.Instance.ColonyAchievementTracker.harvestAHiveWithoutGettingStung = true;
		}
	}

	// Token: 0x04001A3E RID: 6718
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04001A3F RID: 6719
	private static readonly HashedString PST_ANIM = new HashedString("working_pst");

	// Token: 0x04001A40 RID: 6720
	public bool wasStung;
}
