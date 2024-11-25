using System;
using System.Linq;
using KSerialization;

// Token: 0x02000798 RID: 1944
public class WarpReceiver : Workable
{
	// Token: 0x06003537 RID: 13623 RVA: 0x00121F2C File Offset: 0x0012012C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003538 RID: 13624 RVA: 0x00121F34 File Offset: 0x00120134
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.warpReceiverSMI = new WarpReceiver.WarpReceiverSM.Instance(this);
		this.warpReceiverSMI.StartSM();
		Components.WarpReceivers.Add(this);
	}

	// Token: 0x06003539 RID: 13625 RVA: 0x00121F60 File Offset: 0x00120160
	public void ReceiveWarpedDuplicant(WorkerBase dupe)
	{
		dupe.transform.SetPosition(Grid.CellToPos(Grid.PosToCell(this), CellAlignment.Bottom, Grid.SceneLayer.Move));
		Debug.Assert(this.chore == null);
		KAnimFile anim = Assets.GetAnim("anim_interacts_warp_portal_receiver_kanim");
		ChoreType migrate = Db.Get().ChoreTypes.Migrate;
		KAnimFile override_anims = anim;
		this.chore = new WorkChore<Workable>(migrate, this, dupe.GetComponent<ChoreProvider>(), true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, true, true, override_anims, false, true, false, PriorityScreen.PriorityClass.compulsory, 5, false, true);
		Workable component = base.GetComponent<Workable>();
		component.workLayer = Grid.SceneLayer.Building;
		component.workAnims = new HashedString[]
		{
			"printing_pre",
			"printing_loop"
		};
		component.workingPstComplete = new HashedString[]
		{
			"printing_pst"
		};
		component.workingPstFailed = new HashedString[]
		{
			"printing_pst"
		};
		component.synchronizeAnims = true;
		float num = 0f;
		KAnimFileData data = anim.GetData();
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim2 = data.GetAnim(i);
			if (component.workAnims.Contains(anim2.hash))
			{
				num += anim2.totalTime;
			}
		}
		component.SetWorkTime(num);
		this.Used = true;
	}

	// Token: 0x0600353A RID: 13626 RVA: 0x001220BB File Offset: 0x001202BB
	private void CompleteChore()
	{
		this.chore.Cleanup();
		this.chore = null;
		this.warpReceiverSMI.GoTo(this.warpReceiverSMI.sm.idle);
	}

	// Token: 0x0600353B RID: 13627 RVA: 0x001220EA File Offset: 0x001202EA
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.WarpReceivers.Remove(this);
	}

	// Token: 0x04001F9D RID: 8093
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04001F9E RID: 8094
	private WarpReceiver.WarpReceiverSM.Instance warpReceiverSMI;

	// Token: 0x04001F9F RID: 8095
	private Notification notification;

	// Token: 0x04001FA0 RID: 8096
	[Serialize]
	public bool IsConsumed;

	// Token: 0x04001FA1 RID: 8097
	private Chore chore;

	// Token: 0x04001FA2 RID: 8098
	[Serialize]
	public bool Used;

	// Token: 0x02001653 RID: 5715
	public class WarpReceiverSM : GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver>
	{
		// Token: 0x060091DE RID: 37342 RVA: 0x0035247D File Offset: 0x0035067D
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("idle");
		}

		// Token: 0x04006F5F RID: 28511
		public GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver, object>.State idle;

		// Token: 0x0200255A RID: 9562
		public new class Instance : GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver, object>.GameInstance
		{
			// Token: 0x0600BE6F RID: 48751 RVA: 0x003D8C56 File Offset: 0x003D6E56
			public Instance(WarpReceiver master) : base(master)
			{
			}
		}
	}
}
