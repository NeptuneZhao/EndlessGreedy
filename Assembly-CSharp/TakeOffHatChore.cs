using System;
using UnityEngine;

// Token: 0x0200045D RID: 1117
public class TakeOffHatChore : Chore<TakeOffHatChore.StatesInstance>
{
	// Token: 0x06001782 RID: 6018 RVA: 0x0007F77C File Offset: 0x0007D97C
	public TakeOffHatChore(IStateMachineTarget target, ChoreType chore_type) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new TakeOffHatChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x0200120D RID: 4621
	public class StatesInstance : GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.GameInstance
	{
		// Token: 0x06008208 RID: 33288 RVA: 0x0031B258 File Offset: 0x00319458
		public StatesInstance(TakeOffHatChore master, GameObject duplicant) : base(master)
		{
			base.sm.duplicant.Set(duplicant, base.smi, false);
		}
	}

	// Token: 0x0200120E RID: 4622
	public class States : GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore>
	{
		// Token: 0x06008209 RID: 33289 RVA: 0x0031B27C File Offset: 0x0031947C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.remove_hat_pre;
			base.Target(this.duplicant);
			this.remove_hat_pre.Enter(delegate(TakeOffHatChore.StatesInstance smi)
			{
				if (this.duplicant.Get(smi).GetComponent<MinionResume>().CurrentHat != null)
				{
					smi.GoTo(this.remove_hat);
					return;
				}
				smi.GoTo(this.complete);
			});
			this.remove_hat.ToggleAnims("anim_hat_kanim", 0f).PlayAnim("hat_off").OnAnimQueueComplete(this.complete);
			this.complete.Enter(delegate(TakeOffHatChore.StatesInstance smi)
			{
				smi.master.GetComponent<MinionResume>().RemoveHat();
			}).ReturnSuccess();
		}

		// Token: 0x0400623B RID: 25147
		public StateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.TargetParameter duplicant;

		// Token: 0x0400623C RID: 25148
		public GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.State remove_hat_pre;

		// Token: 0x0400623D RID: 25149
		public GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.State remove_hat;

		// Token: 0x0400623E RID: 25150
		public GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.State complete;
	}
}
