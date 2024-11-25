using System;

// Token: 0x0200097C RID: 2428
public class DropUnusedInventoryMonitor : GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance>
{
	// Token: 0x06004703 RID: 18179 RVA: 0x001962B0 File Offset: 0x001944B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.EventTransition(GameHashes.OnStorageChange, this.hasinventory, (DropUnusedInventoryMonitor.Instance smi) => smi.GetComponent<Storage>().Count > 0);
		this.hasinventory.EventTransition(GameHashes.OnStorageChange, this.hasinventory, (DropUnusedInventoryMonitor.Instance smi) => smi.GetComponent<Storage>().Count == 0).ToggleChore((DropUnusedInventoryMonitor.Instance smi) => new DropUnusedInventoryChore(Db.Get().ChoreTypes.DropUnusedInventory, smi.master), this.satisfied);
	}

	// Token: 0x04002E46 RID: 11846
	public GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002E47 RID: 11847
	public GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance, IStateMachineTarget, object>.State hasinventory;

	// Token: 0x02001925 RID: 6437
	public new class Instance : GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009B5D RID: 39773 RVA: 0x0036F6E7 File Offset: 0x0036D8E7
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
