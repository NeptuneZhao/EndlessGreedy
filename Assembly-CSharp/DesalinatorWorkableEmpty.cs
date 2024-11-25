using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020006B4 RID: 1716
[AddComponentMenu("KMonoBehaviour/Workable/DesalinatorWorkableEmpty")]
public class DesalinatorWorkableEmpty : Workable
{
	// Token: 0x06002B3A RID: 11066 RVA: 0x000F2CB8 File Offset: 0x000F0EB8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_desalinator_kanim")
		};
		this.workAnims = DesalinatorWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			DesalinatorWorkableEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			DesalinatorWorkableEmpty.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06002B3B RID: 11067 RVA: 0x000F2D75 File Offset: 0x000F0F75
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.timesCleaned++;
		base.OnCompleteWork(worker);
	}

	// Token: 0x040018D3 RID: 6355
	[Serialize]
	public int timesCleaned;

	// Token: 0x040018D4 RID: 6356
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x040018D5 RID: 6357
	private static readonly HashedString PST_ANIM = new HashedString("salt_pst");
}
