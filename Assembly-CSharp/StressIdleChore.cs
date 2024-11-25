using System;
using UnityEngine;

// Token: 0x02000459 RID: 1113
public class StressIdleChore : Chore<StressIdleChore.StatesInstance>
{
	// Token: 0x06001779 RID: 6009 RVA: 0x0007F470 File Offset: 0x0007D670
	public StressIdleChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.StressIdle, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StressIdleChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02001204 RID: 4612
	public class StatesInstance : GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.GameInstance
	{
		// Token: 0x060081EC RID: 33260 RVA: 0x0031A406 File Offset: 0x00318606
		public StatesInstance(StressIdleChore master, GameObject idler) : base(master)
		{
			base.sm.idler.Set(idler, base.smi, false);
		}
	}

	// Token: 0x02001205 RID: 4613
	public class States : GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore>
	{
		// Token: 0x060081ED RID: 33261 RVA: 0x0031A428 File Offset: 0x00318628
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.Target(this.idler);
			this.idle.PlayAnim("idle_default", KAnim.PlayMode.Loop);
		}

		// Token: 0x0400621B RID: 25115
		public StateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.TargetParameter idler;

		// Token: 0x0400621C RID: 25116
		public GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.State idle;
	}
}
