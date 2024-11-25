using System;
using UnityEngine;

// Token: 0x02000446 RID: 1094
public class MoveToQuarantineChore : Chore<MoveToQuarantineChore.StatesInstance>
{
	// Token: 0x0600173F RID: 5951 RVA: 0x0007DD64 File Offset: 0x0007BF64
	public MoveToQuarantineChore(IStateMachineTarget target, KMonoBehaviour quarantine_area) : base(Db.Get().ChoreTypes.MoveToQuarantine, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveToQuarantineChore.StatesInstance(this, target.gameObject);
		base.smi.sm.locator.Set(quarantine_area.gameObject, base.smi, false);
	}

	// Token: 0x020011D6 RID: 4566
	public class StatesInstance : GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.GameInstance
	{
		// Token: 0x06008148 RID: 33096 RVA: 0x00316876 File Offset: 0x00314A76
		public StatesInstance(MoveToQuarantineChore master, GameObject quarantined) : base(master)
		{
			base.sm.quarantined.Set(quarantined, base.smi, false);
		}
	}

	// Token: 0x020011D7 RID: 4567
	public class States : GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore>
	{
		// Token: 0x06008149 RID: 33097 RVA: 0x00316898 File Offset: 0x00314A98
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			this.approach.InitializeStates(this.quarantined, this.locator, this.success, null, null, null);
			this.success.ReturnSuccess();
		}

		// Token: 0x04006194 RID: 24980
		public StateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.TargetParameter locator;

		// Token: 0x04006195 RID: 24981
		public StateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.TargetParameter quarantined;

		// Token: 0x04006196 RID: 24982
		public GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x04006197 RID: 24983
		public GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.State success;
	}
}
