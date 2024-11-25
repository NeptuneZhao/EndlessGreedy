using System;
using UnityEngine;

// Token: 0x0200044A RID: 1098
public class PutOnHatChore : Chore<PutOnHatChore.StatesInstance>
{
	// Token: 0x06001746 RID: 5958 RVA: 0x0007DFC0 File Offset: 0x0007C1C0
	public PutOnHatChore(IStateMachineTarget target, ChoreType chore_type) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new PutOnHatChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x020011DE RID: 4574
	public class StatesInstance : GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.GameInstance
	{
		// Token: 0x06008159 RID: 33113 RVA: 0x00316E09 File Offset: 0x00315009
		public StatesInstance(PutOnHatChore master, GameObject duplicant) : base(master)
		{
			base.sm.duplicant.Set(duplicant, base.smi, false);
		}
	}

	// Token: 0x020011DF RID: 4575
	public class States : GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore>
	{
		// Token: 0x0600815A RID: 33114 RVA: 0x00316E2C File Offset: 0x0031502C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.applyHat_pre;
			base.Target(this.duplicant);
			this.applyHat_pre.ToggleAnims("anim_hat_kanim", 0f).Enter(delegate(PutOnHatChore.StatesInstance smi)
			{
				this.duplicant.Get(smi).GetComponent<MinionResume>().ApplyTargetHat();
			}).PlayAnim("hat_first").OnAnimQueueComplete(this.applyHat);
			this.applyHat.ToggleAnims("anim_hat_kanim", 0f).PlayAnim("working_pst").OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x040061A7 RID: 24999
		public StateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.TargetParameter duplicant;

		// Token: 0x040061A8 RID: 25000
		public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State applyHat_pre;

		// Token: 0x040061A9 RID: 25001
		public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State applyHat;

		// Token: 0x040061AA RID: 25002
		public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State complete;
	}
}
