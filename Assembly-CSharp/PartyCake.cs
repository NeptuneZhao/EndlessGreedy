using System;
using System.Collections.Generic;

// Token: 0x02000748 RID: 1864
public class PartyCake : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>
{
	// Token: 0x060031A2 RID: 12706 RVA: 0x00111054 File Offset: 0x0010F254
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.creating.ready;
		this.creating.ready.PlayAnim("base").GoTo(this.creating.tier1);
		this.creating.tier1.InitializeStates(this.masterTarget, "tier_1", this.creating.tier2);
		this.creating.tier2.InitializeStates(this.masterTarget, "tier_2", this.creating.tier3);
		this.creating.tier3.InitializeStates(this.masterTarget, "tier_3", this.ready_to_party);
		this.ready_to_party.PlayAnim("unlit");
		this.party.PlayAnim("lit");
		this.complete.PlayAnim("finished");
	}

	// Token: 0x060031A3 RID: 12707 RVA: 0x00111138 File Offset: 0x0010F338
	private static Chore CreateFetchChore(PartyCake.StatesInstance smi)
	{
		return new FetchChore(Db.Get().ChoreTypes.FarmFetch, smi.GetComponent<Storage>(), 10f, new HashSet<Tag>
		{
			"MushBar".ToTag()
		}, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.Functional, 0);
	}

	// Token: 0x060031A4 RID: 12708 RVA: 0x00111188 File Offset: 0x0010F388
	private static Chore CreateWorkChore(PartyCake.StatesInstance smi)
	{
		Workable component = smi.master.GetComponent<PartyCakeWorkable>();
		return new WorkChore<PartyCakeWorkable>(Db.Get().ChoreTypes.Cook, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Work, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x04001D2E RID: 7470
	public PartyCake.CreatingStates creating;

	// Token: 0x04001D2F RID: 7471
	public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State ready_to_party;

	// Token: 0x04001D30 RID: 7472
	public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State party;

	// Token: 0x04001D31 RID: 7473
	public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State complete;

	// Token: 0x020015B5 RID: 5557
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020015B6 RID: 5558
	public class CreatingStates : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State
	{
		// Token: 0x04006D97 RID: 28055
		public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State ready;

		// Token: 0x04006D98 RID: 28056
		public PartyCake.CreatingStates.Tier tier1;

		// Token: 0x04006D99 RID: 28057
		public PartyCake.CreatingStates.Tier tier2;

		// Token: 0x04006D9A RID: 28058
		public PartyCake.CreatingStates.Tier tier3;

		// Token: 0x04006D9B RID: 28059
		public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State finish;

		// Token: 0x02002517 RID: 9495
		public class Tier : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State
		{
			// Token: 0x0600BD1F RID: 48415 RVA: 0x003D6F78 File Offset: 0x003D5178
			public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State InitializeStates(StateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.TargetParameter targ, string animPrefix, GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State success)
			{
				base.root.Target(targ).DefaultState(this.fetch);
				this.fetch.PlayAnim(animPrefix + "_ready").ToggleChore(new Func<PartyCake.StatesInstance, Chore>(PartyCake.CreateFetchChore), this.work);
				this.work.Enter(delegate(PartyCake.StatesInstance smi)
				{
					KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
					component.Play(animPrefix + "_working", KAnim.PlayMode.Once, 1f, 0f);
					component.SetPositionPercent(0f);
				}).ToggleChore(new Func<PartyCake.StatesInstance, Chore>(PartyCake.CreateWorkChore), success, this.work);
				return this;
			}

			// Token: 0x0400A51A RID: 42266
			public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State fetch;

			// Token: 0x0400A51B RID: 42267
			public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State work;
		}
	}

	// Token: 0x020015B7 RID: 5559
	public class StatesInstance : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.GameInstance
	{
		// Token: 0x06008F92 RID: 36754 RVA: 0x003481AB File Offset: 0x003463AB
		public StatesInstance(IStateMachineTarget smi, PartyCake.Def def) : base(smi, def)
		{
		}
	}
}
