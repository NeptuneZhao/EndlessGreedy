using System;
using UnityEngine;

// Token: 0x0200045B RID: 1115
public class SwitchRoleHatChore : Chore<SwitchRoleHatChore.StatesInstance>
{
	// Token: 0x0600177E RID: 6014 RVA: 0x0007F584 File Offset: 0x0007D784
	public SwitchRoleHatChore(IStateMachineTarget target, ChoreType chore_type) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new SwitchRoleHatChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02001208 RID: 4616
	public class StatesInstance : GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.GameInstance
	{
		// Token: 0x060081FA RID: 33274 RVA: 0x0031AF36 File Offset: 0x00319136
		public StatesInstance(SwitchRoleHatChore master, GameObject duplicant) : base(master)
		{
			base.sm.duplicant.Set(duplicant, base.smi, false);
		}
	}

	// Token: 0x02001209 RID: 4617
	public class States : GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore>
	{
		// Token: 0x060081FB RID: 33275 RVA: 0x0031AF58 File Offset: 0x00319158
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.start;
			base.Target(this.duplicant);
			this.start.Enter(delegate(SwitchRoleHatChore.StatesInstance smi)
			{
				if (this.duplicant.Get(smi).GetComponent<MinionResume>().CurrentHat == null)
				{
					smi.GoTo(this.delay);
					return;
				}
				smi.GoTo(this.remove_hat);
			});
			this.remove_hat.ToggleAnims("anim_hat_kanim", 0f).PlayAnim("hat_off").OnAnimQueueComplete(this.delay);
			this.delay.ToggleThought(Db.Get().Thoughts.NewRole, null).ToggleExpression(Db.Get().Expressions.Happy, null).ToggleAnims("anim_selfish_kanim", 0f).QueueAnim("working_pre", false, null).QueueAnim("working_loop", false, null).QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.applyHat_pre);
			this.applyHat_pre.ToggleAnims("anim_hat_kanim", 0f).Enter(delegate(SwitchRoleHatChore.StatesInstance smi)
			{
				this.duplicant.Get(smi).GetComponent<MinionResume>().ApplyTargetHat();
			}).PlayAnim("hat_first").OnAnimQueueComplete(this.applyHat);
			this.applyHat.ToggleAnims("anim_hat_kanim", 0f).PlayAnim("working_pst").OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x0400622B RID: 25131
		public StateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.TargetParameter duplicant;

		// Token: 0x0400622C RID: 25132
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State remove_hat;

		// Token: 0x0400622D RID: 25133
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State start;

		// Token: 0x0400622E RID: 25134
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State delay;

		// Token: 0x0400622F RID: 25135
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State delay_pst;

		// Token: 0x04006230 RID: 25136
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State applyHat_pre;

		// Token: 0x04006231 RID: 25137
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State applyHat;

		// Token: 0x04006232 RID: 25138
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State complete;
	}
}
