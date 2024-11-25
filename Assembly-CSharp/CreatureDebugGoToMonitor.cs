using System;

// Token: 0x02000975 RID: 2421
public class CreatureDebugGoToMonitor : GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>
{
	// Token: 0x060046E9 RID: 18153 RVA: 0x00195896 File Offset: 0x00193A96
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.HasDebugDestination, new StateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.Transition.ConditionCallback(CreatureDebugGoToMonitor.HasTargetCell), new Action<CreatureDebugGoToMonitor.Instance>(CreatureDebugGoToMonitor.ClearTargetCell));
	}

	// Token: 0x060046EA RID: 18154 RVA: 0x001958C9 File Offset: 0x00193AC9
	private static bool HasTargetCell(CreatureDebugGoToMonitor.Instance smi)
	{
		return smi.targetCell != Grid.InvalidCell;
	}

	// Token: 0x060046EB RID: 18155 RVA: 0x001958DB File Offset: 0x00193ADB
	private static void ClearTargetCell(CreatureDebugGoToMonitor.Instance smi)
	{
		smi.targetCell = Grid.InvalidCell;
	}

	// Token: 0x02001911 RID: 6417
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001912 RID: 6418
	public new class Instance : GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.GameInstance
	{
		// Token: 0x06009B15 RID: 39701 RVA: 0x0036EBD2 File Offset: 0x0036CDD2
		public Instance(IStateMachineTarget target, CreatureDebugGoToMonitor.Def def) : base(target, def)
		{
		}

		// Token: 0x06009B16 RID: 39702 RVA: 0x0036EBE7 File Offset: 0x0036CDE7
		public void GoToCursor()
		{
			this.targetCell = DebugHandler.GetMouseCell();
		}

		// Token: 0x06009B17 RID: 39703 RVA: 0x0036EBF4 File Offset: 0x0036CDF4
		public void GoToCell(int cellIndex)
		{
			this.targetCell = cellIndex;
		}

		// Token: 0x04007848 RID: 30792
		public int targetCell = Grid.InvalidCell;
	}
}
