using System;

// Token: 0x02000778 RID: 1912
public class StorageTileSwitchItemWorkable : Workable
{
	// Token: 0x17000376 RID: 886
	// (get) Token: 0x060033B7 RID: 13239 RVA: 0x0011B22A File Offset: 0x0011942A
	// (set) Token: 0x060033B6 RID: 13238 RVA: 0x0011B221 File Offset: 0x00119421
	public int LastCellWorkerUsed { get; private set; } = -1;

	// Token: 0x060033B8 RID: 13240 RVA: 0x0011B232 File Offset: 0x00119432
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
	}

	// Token: 0x060033B9 RID: 13241 RVA: 0x0011B271 File Offset: 0x00119471
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
	}

	// Token: 0x060033BA RID: 13242 RVA: 0x0011B284 File Offset: 0x00119484
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (worker != null)
		{
			this.LastCellWorkerUsed = Grid.PosToCell(worker.transform.GetPosition());
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x04001EAB RID: 7851
	private const string animName = "anim_use_remote_kanim";
}
