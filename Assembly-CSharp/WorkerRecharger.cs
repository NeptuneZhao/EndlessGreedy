using System;

// Token: 0x02000A28 RID: 2600
public class WorkerRecharger : Workable
{
	// Token: 0x06004B5C RID: 19292 RVA: 0x001AD9A8 File Offset: 0x001ABBA8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workAnims = WorkerRecharger.WORK_ANIMS;
		this.workingPstComplete = WorkerRecharger.WORK_PST_ANIM;
		this.workingPstFailed = WorkerRecharger.WORK_PST_ANIM;
		this.synchronizeAnims = true;
		this.triggerWorkReactions = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerRecharging;
		this.workTime = (this.workTimeRemaining = float.PositiveInfinity);
	}

	// Token: 0x06004B5D RID: 19293 RVA: 0x001ADA1C File Offset: 0x001ABC1C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		RemoteWorkerCapacitor component = worker.GetComponent<RemoteWorkerCapacitor>();
		if (component != null)
		{
			this.progressBar.PercentFull = component.ChargeRatio;
			return component.ApplyDeltaEnergy(4f * dt) == 0f;
		}
		return true;
	}

	// Token: 0x0400315B RID: 12635
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"recharge_pre",
		"recharge_loop"
	};

	// Token: 0x0400315C RID: 12636
	private static readonly HashedString[] WORK_PST_ANIM = new HashedString[]
	{
		"recharge_pst"
	};
}
