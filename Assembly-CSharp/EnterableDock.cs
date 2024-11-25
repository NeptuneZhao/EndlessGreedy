using System;

// Token: 0x02000A26 RID: 2598
public class EnterableDock : Workable
{
	// Token: 0x06004B54 RID: 19284 RVA: 0x001AD8B0 File Offset: 0x001ABAB0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.EnteringDock;
		this.workAnims = EnterableDock.WORK_ANIMS;
		this.workAnimPlayMode = KAnim.PlayMode.Once;
		this.synchronizeAnims = true;
		this.triggerWorkReactions = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x06004B55 RID: 19285 RVA: 0x001AD900 File Offset: 0x001ABB00
	protected override void OnCompleteWork(WorkerBase worker)
	{
		worker.GetComponent<RemoteWorkerSM>().Docked = true;
		base.OnCompleteWork(worker);
	}

	// Token: 0x04003159 RID: 12633
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"enter_dock"
	};
}
