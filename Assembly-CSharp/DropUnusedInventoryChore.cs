using System;
using UnityEngine;

// Token: 0x020004D4 RID: 1236
public class DropUnusedInventoryChore : Chore<DropUnusedInventoryChore.StatesInstance>
{
	// Token: 0x06001AAD RID: 6829 RVA: 0x0008C59C File Offset: 0x0008A79C
	public DropUnusedInventoryChore(ChoreType chore_type, IStateMachineTarget target) : base(chore_type, target, target.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new DropUnusedInventoryChore.StatesInstance(this);
	}

	// Token: 0x020012AE RID: 4782
	public class StatesInstance : GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.GameInstance
	{
		// Token: 0x060084B4 RID: 33972 RVA: 0x0032435F File Offset: 0x0032255F
		public StatesInstance(DropUnusedInventoryChore master) : base(master)
		{
		}
	}

	// Token: 0x020012AF RID: 4783
	public class States : GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore>
	{
		// Token: 0x060084B5 RID: 33973 RVA: 0x00324368 File Offset: 0x00322568
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.dropping;
			this.dropping.Enter(delegate(DropUnusedInventoryChore.StatesInstance smi)
			{
				smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
			}).GoTo(this.success);
			this.success.ReturnSuccess();
		}

		// Token: 0x04006403 RID: 25603
		public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State dropping;

		// Token: 0x04006404 RID: 25604
		public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State success;
	}
}
