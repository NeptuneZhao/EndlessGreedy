using System;

// Token: 0x0200069A RID: 1690
public class ComplexFabricatorSM : StateMachineComponent<ComplexFabricatorSM.StatesInstance>
{
	// Token: 0x06002A7A RID: 10874 RVA: 0x000F02D2 File Offset: 0x000EE4D2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001877 RID: 6263
	[MyCmpGet]
	private ComplexFabricator fabricator;

	// Token: 0x04001878 RID: 6264
	public StatusItem idleQueue_StatusItem = Db.Get().BuildingStatusItems.FabricatorIdle;

	// Token: 0x04001879 RID: 6265
	public StatusItem waitingForMaterial_StatusItem = Db.Get().BuildingStatusItems.FabricatorEmpty;

	// Token: 0x0400187A RID: 6266
	public StatusItem waitingForWorker_StatusItem = Db.Get().BuildingStatusItems.PendingWork;

	// Token: 0x0400187B RID: 6267
	public string idleAnimationName = "off";

	// Token: 0x02001495 RID: 5269
	public class StatesInstance : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.GameInstance
	{
		// Token: 0x06008B67 RID: 35687 RVA: 0x003365CD File Offset: 0x003347CD
		public StatesInstance(ComplexFabricatorSM master) : base(master)
		{
		}
	}

	// Token: 0x02001496 RID: 5270
	public class States : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM>
	{
		// Token: 0x06008B68 RID: 35688 RVA: 0x003365D8 File Offset: 0x003347D8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.idle, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.idle.DefaultState(this.idle.idleQueue).PlayAnim(new Func<ComplexFabricatorSM.StatesInstance, string>(ComplexFabricatorSM.States.GetIdleAnimName), KAnim.PlayMode.Once).EventTransition(GameHashes.OperationalChanged, this.off, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.idle.idleQueue.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.idleQueue_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForMaterial, (ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.HasAnyOrder);
			this.idle.waitingForMaterial.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.waitingForMaterial_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.HasAnyOrder).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForWorker, (ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.WaitingForWorker).EventHandler(GameHashes.FabricatorOrdersUpdated, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).EventHandler(GameHashes.OnParticleStorageChanged, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).Enter(delegate(ComplexFabricatorSM.StatesInstance smi)
			{
				this.RefreshHEPStatus(smi);
			});
			this.idle.waitingForWorker.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.waitingForWorker_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.WaitingForWorker).EnterTransition(this.operating, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.duplicantOperated).EventHandler(GameHashes.FabricatorOrdersUpdated, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).EventHandler(GameHashes.OnParticleStorageChanged, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).Enter(delegate(ComplexFabricatorSM.StatesInstance smi)
			{
				this.RefreshHEPStatus(smi);
			});
			this.operating.DefaultState(this.operating.working_pre).ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.workingStatusItem, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<ComplexFabricator>());
			this.operating.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operating.working_loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.operating.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.operating.working_pst.PlayAnim("working_pst").WorkableCompleteTransition((ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.Workable, this.operating.working_pst_complete).OnAnimQueueComplete(this.idle);
			this.operating.working_pst_complete.PlayAnim("working_pst_complete").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x06008B69 RID: 35689 RVA: 0x00336A94 File Offset: 0x00334C94
		public void RefreshHEPStatus(ComplexFabricatorSM.StatesInstance smi)
		{
			if (smi.master.GetComponent<HighEnergyParticleStorage>() != null && smi.master.fabricator.NeedsMoreHEPForQueuedRecipe())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP, smi.master.fabricator);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP, false);
		}

		// Token: 0x06008B6A RID: 35690 RVA: 0x00336B13 File Offset: 0x00334D13
		public static string GetIdleAnimName(ComplexFabricatorSM.StatesInstance smi)
		{
			return smi.master.idleAnimationName;
		}

		// Token: 0x04006A6C RID: 27244
		public ComplexFabricatorSM.States.IdleStates off;

		// Token: 0x04006A6D RID: 27245
		public ComplexFabricatorSM.States.IdleStates idle;

		// Token: 0x04006A6E RID: 27246
		public ComplexFabricatorSM.States.OperatingStates operating;

		// Token: 0x020024C4 RID: 9412
		public class IdleStates : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
		{
			// Token: 0x0400A30B RID: 41739
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State idleQueue;

			// Token: 0x0400A30C RID: 41740
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForMaterial;

			// Token: 0x0400A30D RID: 41741
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForWorker;
		}

		// Token: 0x020024C5 RID: 9413
		public class OperatingStates : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
		{
			// Token: 0x0400A30E RID: 41742
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pre;

			// Token: 0x0400A30F RID: 41743
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_loop;

			// Token: 0x0400A310 RID: 41744
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst;

			// Token: 0x0400A311 RID: 41745
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst_complete;
		}
	}
}
