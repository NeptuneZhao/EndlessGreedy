using System;

// Token: 0x02000A64 RID: 2660
public class StorageUnloadMonitor : GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>
{
	// Token: 0x06004D3D RID: 19773 RVA: 0x001BAAC4 File Offset: 0x001B8CC4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notFull;
		this.notFull.Transition(this.full, new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback(StorageUnloadMonitor.WantsToUnload), UpdateRate.SIM_200ms);
		this.full.ToggleStatusItem(Db.Get().RobotStatusItems.DustBinFull, (StorageUnloadMonitor.Instance smi) => smi.gameObject).ToggleBehaviour(GameTags.Robots.Behaviours.UnloadBehaviour, (StorageUnloadMonitor.Instance data) => true, null).Transition(this.notFull, GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Not(new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback(StorageUnloadMonitor.WantsToUnload)), UpdateRate.SIM_1000ms).Enter(delegate(StorageUnloadMonitor.Instance smi)
		{
			if (smi.master.gameObject.GetComponents<Storage>()[1].RemainingCapacity() <= 0f)
			{
				smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("react_full");
			}
		});
	}

	// Token: 0x06004D3E RID: 19774 RVA: 0x001BABA0 File Offset: 0x001B8DA0
	public static bool WantsToUnload(StorageUnloadMonitor.Instance smi)
	{
		Storage storage = smi.sm.sweepLocker.Get(smi);
		return !(storage == null) && !(smi.sm.internalStorage.Get(smi) == null) && !smi.HasTag(GameTags.Robots.Behaviours.RechargeBehaviour) && (smi.sm.internalStorage.Get(smi).IsFull() || (storage != null && !smi.sm.internalStorage.Get(smi).IsEmpty() && Grid.PosToCell(storage) == Grid.PosToCell(smi)));
	}

	// Token: 0x0400334B RID: 13131
	public StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage> internalStorage = new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage>();

	// Token: 0x0400334C RID: 13132
	public StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage> sweepLocker;

	// Token: 0x0400334D RID: 13133
	public GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State notFull;

	// Token: 0x0400334E RID: 13134
	public GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State full;

	// Token: 0x02001A71 RID: 6769
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001A72 RID: 6770
	public new class Instance : GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.GameInstance
	{
		// Token: 0x06009FFD RID: 40957 RVA: 0x0037E495 File Offset: 0x0037C695
		public Instance(IStateMachineTarget master, StorageUnloadMonitor.Def def) : base(master, def)
		{
		}
	}
}
