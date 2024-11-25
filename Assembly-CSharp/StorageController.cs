using System;

// Token: 0x02000056 RID: 86
public class StorageController : GameStateMachine<StorageController, StorageController.Instance>
{
	// Token: 0x06000195 RID: 405 RVA: 0x0000ADBC File Offset: 0x00008FBC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.EventTransition(GameHashes.OnStorageInteracted, this.working, null);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (StorageController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (StorageController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.working.PlayAnim("working").OnAnimQueueComplete(this.off);
	}

	// Token: 0x04000103 RID: 259
	public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000104 RID: 260
	public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x04000105 RID: 261
	public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State working;

	// Token: 0x02000FB5 RID: 4021
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000FB6 RID: 4022
	public new class Instance : GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007A52 RID: 31314 RVA: 0x00301EC2 File Offset: 0x003000C2
		public Instance(IStateMachineTarget master, StorageController.Def def) : base(master)
		{
		}
	}
}
