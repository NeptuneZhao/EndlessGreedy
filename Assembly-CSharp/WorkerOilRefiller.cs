using System;

// Token: 0x02000A2A RID: 2602
public class WorkerOilRefiller : Workable
{
	// Token: 0x06004B64 RID: 19300 RVA: 0x001ADC40 File Offset: 0x001ABE40
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_remote_work_dock_kanim")
		};
		this.workAnims = WorkerOilRefiller.WORK_ANIMS;
		this.workingPstComplete = WorkerOilRefiller.WORK_PST_ANIM;
		this.workingPstFailed = WorkerOilRefiller.WORK_PST_ANIM;
		this.synchronizeAnims = true;
		this.triggerWorkReactions = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerOiling;
		this.workTime = (this.workTimeRemaining = float.PositiveInfinity);
	}

	// Token: 0x06004B65 RID: 19301 RVA: 0x001ADCD4 File Offset: 0x001ABED4
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		Storage component = worker.GetComponent<Storage>();
		if (component != null)
		{
			float massAvailable = component.GetMassAvailable(GameTags.LubricatingOil);
			float num = Math.Min(60f - massAvailable, 1f * dt);
			this.progressBar.PercentFull = massAvailable / 60f;
			if (num > 0f)
			{
				this.storage.TransferMass(component, GameTags.LubricatingOil, num, false, false, true);
				return false;
			}
		}
		return true;
	}

	// Token: 0x04003160 RID: 12640
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"oil_pre",
		"oil_loop"
	};

	// Token: 0x04003161 RID: 12641
	private static readonly HashedString[] WORK_PST_ANIM = new HashedString[]
	{
		"oil_pst"
	};

	// Token: 0x04003162 RID: 12642
	[MyCmpGet]
	private Storage storage;
}
