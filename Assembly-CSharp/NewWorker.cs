using System;

// Token: 0x02000A25 RID: 2597
public class NewWorker : Workable
{
	// Token: 0x06004B50 RID: 19280 RVA: 0x001AD818 File Offset: 0x001ABA18
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.EnteringDock;
		this.workAnims = NewWorker.WORK_ANIMS;
		this.workAnimPlayMode = KAnim.PlayMode.Once;
		this.synchronizeAnims = true;
		this.workTime = 0.8f;
		this.triggerWorkReactions = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x06004B51 RID: 19281 RVA: 0x001AD873 File Offset: 0x001ABA73
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		worker.GetComponent<RemoteWorkerSM>().Docked = true;
	}

	// Token: 0x04003158 RID: 12632
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"new_worker"
	};
}
