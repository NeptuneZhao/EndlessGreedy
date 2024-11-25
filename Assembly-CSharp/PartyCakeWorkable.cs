using System;
using TUNING;

// Token: 0x02000749 RID: 1865
public class PartyCakeWorkable : Workable
{
	// Token: 0x060031A6 RID: 12710 RVA: 0x001111E0 File Offset: 0x0010F3E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
		this.alwaysShowProgressBar = true;
		this.resetProgressOnStop = false;
		this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_desalinator_kanim")
		};
		this.workAnims = PartyCakeWorkable.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			PartyCakeWorkable.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			PartyCakeWorkable.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x00111296 File Offset: 0x0010F496
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		base.GetComponent<KBatchedAnimController>().SetPositionPercent(this.GetPercentComplete());
		return false;
	}

	// Token: 0x04001D32 RID: 7474
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x04001D33 RID: 7475
	private static readonly HashedString PST_ANIM = new HashedString("salt_pst");
}
