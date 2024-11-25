using System;

// Token: 0x020006D5 RID: 1749
public class FoodDehydratorWorkableEmpty : Workable
{
	// Token: 0x06002C4E RID: 11342 RVA: 0x000F8E05 File Offset: 0x000F7005
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.workAnims = FoodDehydratorWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = FoodDehydratorWorkableEmpty.WORK_ANIMS_PST;
		this.workingPstFailed = FoodDehydratorWorkableEmpty.WORK_ANIMS_FAIL_PST;
	}

	// Token: 0x04001994 RID: 6548
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"empty_pre",
		"empty_loop"
	};

	// Token: 0x04001995 RID: 6549
	private static readonly HashedString[] WORK_ANIMS_PST = new HashedString[]
	{
		"empty_pst"
	};

	// Token: 0x04001996 RID: 6550
	private static readonly HashedString[] WORK_ANIMS_FAIL_PST = new HashedString[]
	{
		""
	};
}
