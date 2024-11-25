using System;

// Token: 0x02000436 RID: 1078
public class DieChore : Chore<DieChore.StatesInstance>
{
	// Token: 0x060016EA RID: 5866 RVA: 0x0007BBE0 File Offset: 0x00079DE0
	public DieChore(IStateMachineTarget master, Death death) : base(Db.Get().ChoreTypes.Die, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new DieChore.StatesInstance(this, death);
	}

	// Token: 0x020011AE RID: 4526
	public class StatesInstance : GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.GameInstance
	{
		// Token: 0x060080BC RID: 32956 RVA: 0x003127D8 File Offset: 0x003109D8
		public StatesInstance(DieChore master, Death death) : base(master)
		{
			base.sm.death.Set(death, base.smi, false);
		}

		// Token: 0x060080BD RID: 32957 RVA: 0x003127FC File Offset: 0x003109FC
		public void PlayPreAnim()
		{
			string preAnim = base.sm.death.Get(base.smi).preAnim;
			base.GetComponent<KAnimControllerBase>().Play(preAnim, KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x020011AF RID: 4527
	public class States : GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore>
	{
		// Token: 0x060080BE RID: 32958 RVA: 0x00312844 File Offset: 0x00310A44
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.dying;
			this.dying.OnAnimQueueComplete(this.dead).Enter("PlayAnim", delegate(DieChore.StatesInstance smi)
			{
				smi.PlayPreAnim();
			});
			this.dead.ReturnSuccess();
		}

		// Token: 0x040060FF RID: 24831
		public GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State dying;

		// Token: 0x04006100 RID: 24832
		public GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State dead;

		// Token: 0x04006101 RID: 24833
		public StateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.ResourceParameter<Death> death;
	}
}
