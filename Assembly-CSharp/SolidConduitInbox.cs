using System;
using KSerialization;

// Token: 0x0200076B RID: 1899
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitInbox : StateMachineComponent<SolidConduitInbox.SMInstance>, ISim1000ms
{
	// Token: 0x06003319 RID: 13081 RVA: 0x00118A08 File Offset: 0x00116C08
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.filteredStorage = new FilteredStorage(this, null, null, false, Db.Get().ChoreTypes.StorageFetch);
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x00118A2E File Offset: 0x00116C2E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filteredStorage.FilterChanged();
		base.smi.StartSM();
	}

	// Token: 0x0600331B RID: 13083 RVA: 0x00118A4C File Offset: 0x00116C4C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x00118A54 File Offset: 0x00116C54
	public void Sim1000ms(float dt)
	{
		if (this.operational.IsOperational && this.dispenser.IsDispensing)
		{
			this.operational.SetActive(true, false);
			return;
		}
		this.operational.SetActive(false, false);
	}

	// Token: 0x04001E2B RID: 7723
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001E2C RID: 7724
	[MyCmpReq]
	private SolidConduitDispenser dispenser;

	// Token: 0x04001E2D RID: 7725
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001E2E RID: 7726
	private FilteredStorage filteredStorage;

	// Token: 0x020015F9 RID: 5625
	public class SMInstance : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.GameInstance
	{
		// Token: 0x0600908C RID: 37004 RVA: 0x0034BDE4 File Offset: 0x00349FE4
		public SMInstance(SolidConduitInbox master) : base(master)
		{
		}
	}

	// Token: 0x020015FA RID: 5626
	public class States : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox>
	{
		// Token: 0x0600908D RID: 37005 RVA: 0x0034BDF0 File Offset: 0x00349FF0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidConduitInbox.SMInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidConduitInbox.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (SolidConduitInbox.SMInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).EventTransition(GameHashes.ActiveChanged, this.on.post, (SolidConduitInbox.SMInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.on.post.PlayAnim("working_pst").OnAnimQueueComplete(this.on);
		}

		// Token: 0x04006E4E RID: 28238
		public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State off;

		// Token: 0x04006E4F RID: 28239
		public SolidConduitInbox.States.ReadyStates on;

		// Token: 0x02002531 RID: 9521
		public class ReadyStates : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State
		{
			// Token: 0x0400A5A4 RID: 42404
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State idle;

			// Token: 0x0400A5A5 RID: 42405
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State working;

			// Token: 0x0400A5A6 RID: 42406
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State post;
		}
	}
}
