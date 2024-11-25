using System;

// Token: 0x020006E1 RID: 1761
public class ToggleGeothermalVentConnection : Toggleable
{
	// Token: 0x06002CBD RID: 11453 RVA: 0x000FB638 File Offset: 0x000F9838
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(10f);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim(GeothermalVentConfig.TOGGLE_ANIM_OVERRIDE)
		};
		this.workAnims = new HashedString[]
		{
			GeothermalVentConfig.TOGGLE_ANIMATION
		};
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.workLayer = Grid.SceneLayer.Front;
		this.synchronizeAnims = false;
		this.workAnimPlayMode = KAnim.PlayMode.Once;
		base.SetOffsets(new CellOffset[]
		{
			CellOffset.none
		});
	}

	// Token: 0x06002CBE RID: 11454 RVA: 0x000FB6C8 File Offset: 0x000F98C8
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.buildingAnimController.Play(GeothermalVentConfig.TOGGLE_ANIMATION, KAnim.PlayMode.Once, 1f, 0f);
		if (this.workerFacing == null || this.workerFacing.gameObject != worker.gameObject)
		{
			this.workerFacing = worker.GetComponent<Facing>();
		}
	}

	// Token: 0x06002CBF RID: 11455 RVA: 0x000FB72E File Offset: 0x000F992E
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.workerFacing != null)
		{
			this.workerFacing.Face(this.workerFacing.transform.GetLocalPosition().x + 0.5f);
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x040019D0 RID: 6608
	[MyCmpGet]
	private KBatchedAnimController buildingAnimController;

	// Token: 0x040019D1 RID: 6609
	private Facing workerFacing;
}
