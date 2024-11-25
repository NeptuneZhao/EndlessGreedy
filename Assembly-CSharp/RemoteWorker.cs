using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A23 RID: 2595
public class RemoteWorker : StandardWorker
{
	// Token: 0x06004B39 RID: 19257 RVA: 0x001AD5CC File Offset: 0x001AB7CC
	public override AttributeConverterInstance GetAttributeConverter(string id)
	{
		RemoteWorkerDock homeDepot = this.remoteWorkerSM.HomeDepot;
		WorkerBase workerBase = ((homeDepot != null) ? homeDepot.GetActiveTerminalWorker() : null) ?? null;
		if (workerBase != null)
		{
			return workerBase.GetAttributeConverter(id);
		}
		return null;
	}

	// Token: 0x06004B3A RID: 19258 RVA: 0x001AD608 File Offset: 0x001AB808
	protected override void TryPlayingIdle()
	{
		if (this.remoteWorkerSM.Docked)
		{
			base.GetComponent<KAnimControllerBase>().Play("in_dock_idle", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		base.TryPlayingIdle();
	}

	// Token: 0x06004B3B RID: 19259 RVA: 0x001AD640 File Offset: 0x001AB840
	protected override void InternalStopWork(Workable target_workable, bool is_aborted)
	{
		base.InternalStopWork(target_workable, is_aborted);
		Vector3 position = base.transform.GetPosition();
		RemoteWorkerSM remoteWorkerSM = this.remoteWorkerSM;
		position.z = Grid.GetLayerZ((remoteWorkerSM != null && remoteWorkerSM.Docked) ? Grid.SceneLayer.BuildingUse : Grid.SceneLayer.Move);
		base.transform.SetPosition(position);
	}

	// Token: 0x04003146 RID: 12614
	[MyCmpGet]
	private RemoteWorkerSM remoteWorkerSM;
}
