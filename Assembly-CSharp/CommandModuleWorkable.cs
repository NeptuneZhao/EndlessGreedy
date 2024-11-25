using System;
using UnityEngine;

// Token: 0x02000698 RID: 1688
[AddComponentMenu("KMonoBehaviour/Workable/CommandModuleWorkable")]
public class CommandModuleWorkable : Workable
{
	// Token: 0x06002A25 RID: 10789 RVA: 0x000EDE70 File Offset: 0x000EC070
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsets(CommandModuleWorkable.entryOffsets);
		this.synchronizeAnims = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_incubator_kanim")
		};
		base.SetWorkTime(float.PositiveInfinity);
		this.showProgressBar = false;
	}

	// Token: 0x06002A26 RID: 10790 RVA: 0x000EDEC5 File Offset: 0x000EC0C5
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x000EDED0 File Offset: 0x000EC0D0
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (!(worker != null))
		{
			return base.OnWorkTick(worker, dt);
		}
		if (DlcManager.IsExpansion1Active())
		{
			GameObject gameObject = worker.gameObject;
			base.CompleteWork(worker);
			base.GetComponent<ClustercraftExteriorDoor>().FerryMinion(gameObject);
			return true;
		}
		GameObject gameObject2 = worker.gameObject;
		base.CompleteWork(worker);
		base.GetComponent<MinionStorage>().SerializeMinion(gameObject2);
		return true;
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x000EDF2D File Offset: 0x000EC12D
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x000EDF36 File Offset: 0x000EC136
	protected override void OnCompleteWork(WorkerBase worker)
	{
	}

	// Token: 0x04001846 RID: 6214
	private static CellOffset[] entryOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(0, 2),
		new CellOffset(0, 3),
		new CellOffset(0, 4)
	};
}
