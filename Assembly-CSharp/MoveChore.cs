using System;
using UnityEngine;

// Token: 0x02000444 RID: 1092
public class MoveChore : Chore<MoveChore.StatesInstance>
{
	// Token: 0x06001739 RID: 5945 RVA: 0x0007D9C4 File Offset: 0x0007BBC4
	public MoveChore(IStateMachineTarget target, ChoreType chore_type, Func<MoveChore.StatesInstance, int> get_cell_callback, bool update_cell = false) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveChore.StatesInstance(this, target.gameObject, get_cell_callback, update_cell);
	}

	// Token: 0x020011D1 RID: 4561
	public class StatesInstance : GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.GameInstance
	{
		// Token: 0x06008135 RID: 33077 RVA: 0x00316214 File Offset: 0x00314414
		public StatesInstance(MoveChore master, GameObject mover, Func<MoveChore.StatesInstance, int> get_cell_callback, bool update_cell = false) : base(master)
		{
			this.getCellCallback = get_cell_callback;
			base.sm.mover.Set(mover, base.smi, false);
		}

		// Token: 0x04006182 RID: 24962
		public Func<MoveChore.StatesInstance, int> getCellCallback;
	}

	// Token: 0x020011D2 RID: 4562
	public class States : GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore>
	{
		// Token: 0x06008136 RID: 33078 RVA: 0x00316240 File Offset: 0x00314440
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.mover);
			this.root.MoveTo((MoveChore.StatesInstance smi) => smi.getCellCallback(smi), null, null, false);
		}

		// Token: 0x04006183 RID: 24963
		public GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x04006184 RID: 24964
		public StateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.TargetParameter mover;

		// Token: 0x04006185 RID: 24965
		public StateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.TargetParameter locator;
	}
}
