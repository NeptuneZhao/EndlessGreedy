﻿using System;

// Token: 0x02000A29 RID: 2601
public class WorkerGunkRemover : Workable
{
	// Token: 0x06004B60 RID: 19296 RVA: 0x001ADACC File Offset: 0x001ABCCC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_remote_work_dock_kanim")
		};
		this.workAnims = WorkerGunkRemover.WORK_ANIMS;
		this.workingPstComplete = WorkerGunkRemover.WORK_PST_ANIM;
		this.workingPstFailed = WorkerGunkRemover.WORK_PST_ANIM;
		this.synchronizeAnims = true;
		this.triggerWorkReactions = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerDraining;
		this.workTime = (this.workTimeRemaining = float.PositiveInfinity);
	}

	// Token: 0x06004B61 RID: 19297 RVA: 0x001ADB60 File Offset: 0x001ABD60
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		Storage component = worker.GetComponent<Storage>();
		if (component != null)
		{
			float massAvailable = component.GetMassAvailable(SimHashes.LiquidGunk);
			float num = Math.Min(massAvailable, 1f * dt);
			this.progressBar.PercentFull = 1f - massAvailable / 600f;
			if (num > 0f)
			{
				component.TransferMass(this.storage, SimHashes.LiquidGunk.CreateTag(), num, false, false, true);
				return false;
			}
		}
		return true;
	}

	// Token: 0x0400315D RID: 12637
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"drain_gunk_pre",
		"drain_gunk_loop"
	};

	// Token: 0x0400315E RID: 12638
	private static readonly HashedString[] WORK_PST_ANIM = new HashedString[]
	{
		"drain_gunk_pst"
	};

	// Token: 0x0400315F RID: 12639
	[MyCmpGet]
	private Storage storage;
}
