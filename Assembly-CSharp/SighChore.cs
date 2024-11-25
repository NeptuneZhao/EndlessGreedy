using System;
using UnityEngine;

// Token: 0x02000456 RID: 1110
public class SighChore : Chore<SighChore.StatesInstance>
{
	// Token: 0x0600176E RID: 5998 RVA: 0x0007F120 File Offset: 0x0007D320
	public SighChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Sigh, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new SighChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x020011FD RID: 4605
	public class StatesInstance : GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.GameInstance
	{
		// Token: 0x060081CE RID: 33230 RVA: 0x0031967E File Offset: 0x0031787E
		public StatesInstance(SighChore master, GameObject sigher) : base(master)
		{
			base.sm.sigher.Set(sigher, base.smi, false);
		}
	}

	// Token: 0x020011FE RID: 4606
	public class States : GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore>
	{
		// Token: 0x060081CF RID: 33231 RVA: 0x003196A0 File Offset: 0x003178A0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.sigher);
			this.root.PlayAnim("emote_depressed").OnAnimQueueComplete(null);
		}

		// Token: 0x04006200 RID: 25088
		public StateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.TargetParameter sigher;
	}
}
