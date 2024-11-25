using System;

// Token: 0x02000978 RID: 2424
public class DebugGoToMonitor : GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>
{
	// Token: 0x060046F6 RID: 18166 RVA: 0x00195D24 File Offset: 0x00193F24
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DoNothing();
		this.hastarget.ToggleChore((DebugGoToMonitor.Instance smi) => new MoveChore(smi.master, Db.Get().ChoreTypes.DebugGoTo, (MoveChore.StatesInstance smii) => smi.targetCellIndex, false), this.satisfied);
	}

	// Token: 0x04002E3F RID: 11839
	public GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.State satisfied;

	// Token: 0x04002E40 RID: 11840
	public GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.State hastarget;

	// Token: 0x02001919 RID: 6425
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200191A RID: 6426
	public new class Instance : GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.GameInstance
	{
		// Token: 0x06009B32 RID: 39730 RVA: 0x0036EEFD File Offset: 0x0036D0FD
		public Instance(IStateMachineTarget target, DebugGoToMonitor.Def def) : base(target, def)
		{
		}

		// Token: 0x06009B33 RID: 39731 RVA: 0x0036EF14 File Offset: 0x0036D114
		public void GoToCursor()
		{
			this.targetCellIndex = DebugHandler.GetMouseCell();
			if (base.smi.GetCurrentState() == base.smi.sm.satisfied)
			{
				base.smi.GoTo(base.smi.sm.hastarget);
			}
		}

		// Token: 0x06009B34 RID: 39732 RVA: 0x0036EF64 File Offset: 0x0036D164
		public void GoToCell(int cellIndex)
		{
			this.targetCellIndex = cellIndex;
			if (base.smi.GetCurrentState() == base.smi.sm.satisfied)
			{
				base.smi.GoTo(base.smi.sm.hastarget);
			}
		}

		// Token: 0x04007858 RID: 30808
		public int targetCellIndex = Grid.InvalidCell;
	}
}
