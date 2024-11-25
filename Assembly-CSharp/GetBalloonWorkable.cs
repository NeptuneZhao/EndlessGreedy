using System;
using Database;
using UnityEngine;

// Token: 0x020006E4 RID: 1764
[AddComponentMenu("KMonoBehaviour/Workable/GetBalloonWorkable")]
public class GetBalloonWorkable : Workable
{
	// Token: 0x06002CE7 RID: 11495 RVA: 0x000FC570 File Offset: 0x000FA770
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = null;
		this.workingStatusItem = null;
		this.workAnims = GetBalloonWorkable.GET_BALLOON_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			GetBalloonWorkable.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			GetBalloonWorkable.PST_ANIM
		};
	}

	// Token: 0x06002CE8 RID: 11496 RVA: 0x000FC5D4 File Offset: 0x000FA7D4
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		BalloonOverrideSymbol balloonOverride = this.balloonArtist.GetBalloonOverride();
		if (balloonOverride.animFile.IsNone())
		{
			worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", Assets.GetAnim("balloon_anim_kanim").GetData().build.GetSymbol("body"), 0);
			return;
		}
		worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", balloonOverride.symbol.Unwrap(), 0);
	}

	// Token: 0x06002CE9 RID: 11497 RVA: 0x000FC670 File Offset: 0x000FA870
	protected override void OnCompleteWork(WorkerBase worker)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EquippableBalloon"), worker.transform.GetPosition());
		gameObject.GetComponent<Equippable>().Assign(worker.GetComponent<MinionIdentity>());
		gameObject.GetComponent<Equippable>().isEquipped = true;
		gameObject.SetActive(true);
		base.OnCompleteWork(worker);
		BalloonOverrideSymbol balloonOverride = this.balloonArtist.GetBalloonOverride();
		this.balloonArtist.GiveBalloon(balloonOverride);
		gameObject.GetComponent<EquippableBalloon>().SetBalloonOverride(balloonOverride);
	}

	// Token: 0x06002CEA RID: 11498 RVA: 0x000FC6EA File Offset: 0x000FA8EA
	public override Vector3 GetFacingTarget()
	{
		return this.balloonArtist.master.transform.GetPosition();
	}

	// Token: 0x06002CEB RID: 11499 RVA: 0x000FC701 File Offset: 0x000FA901
	public void SetBalloonArtist(BalloonArtistChore.StatesInstance chore)
	{
		this.balloonArtist = chore;
	}

	// Token: 0x06002CEC RID: 11500 RVA: 0x000FC70A File Offset: 0x000FA90A
	public BalloonArtistChore.StatesInstance GetBalloonArtist()
	{
		return this.balloonArtist;
	}

	// Token: 0x040019E7 RID: 6631
	private static readonly HashedString[] GET_BALLOON_ANIMS = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x040019E8 RID: 6632
	private static readonly HashedString PST_ANIM = new HashedString("working_pst");

	// Token: 0x040019E9 RID: 6633
	private BalloonArtistChore.StatesInstance balloonArtist;

	// Token: 0x040019EA RID: 6634
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x040019EB RID: 6635
	private const int TARGET_OVERRIDE_PRIORITY = 0;
}
