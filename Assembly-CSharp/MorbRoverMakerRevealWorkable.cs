using System;

// Token: 0x02000305 RID: 773
public class MorbRoverMakerRevealWorkable : Workable
{
	// Token: 0x0600103E RID: 4158 RVA: 0x0005BE3C File Offset: 0x0005A03C
	protected override void OnPrefabInit()
	{
		this.workAnims = new HashedString[]
		{
			"reveal_working_pre",
			"reveal_working_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"reveal_working_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"reveal_working_pst"
		};
		base.OnPrefabInit();
		this.workingStatusItem = Db.Get().BuildingStatusItems.MorbRoverMakerBuildingRevealed;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.MorbRoverMakerWorkingOnRevealing);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gravitas_morb_tank_kanim")
		};
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x0600103F RID: 4159 RVA: 0x0005BF18 File Offset: 0x0005A118
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x040009E1 RID: 2529
	public const string WORKABLE_PRE_ANIM_NAME = "reveal_working_pre";

	// Token: 0x040009E2 RID: 2530
	public const string WORKABLE_LOOP_ANIM_NAME = "reveal_working_loop";

	// Token: 0x040009E3 RID: 2531
	public const string WORKABLE_PST_ANIM_NAME = "reveal_working_pst";
}
