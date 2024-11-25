using System;

// Token: 0x02000A27 RID: 2599
public class ExitableDock : Workable
{
	// Token: 0x06004B58 RID: 19288 RVA: 0x001AD93B File Offset: 0x001ABB3B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workAnims = ExitableDock.WORK_ANIMS;
		this.workAnimPlayMode = KAnim.PlayMode.Once;
		this.synchronizeAnims = true;
		this.triggerWorkReactions = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x06004B59 RID: 19289 RVA: 0x001AD96B File Offset: 0x001ABB6B
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		worker.GetComponent<RemoteWorkerSM>().Docked = false;
	}

	// Token: 0x0400315A RID: 12634
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"exit_dock"
	};
}
