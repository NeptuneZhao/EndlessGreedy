using System;
using Klei.AI;
using TUNING;

// Token: 0x02000448 RID: 1096
public class PartyChore : Chore<PartyChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x06001741 RID: 5953 RVA: 0x0007DE18 File Offset: 0x0007C018
	public PartyChore(IStateMachineTarget master, Workable chat_workable, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null) : base(Db.Get().ChoreTypes.Party, master, master.GetComponent<ChoreProvider>(), true, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new PartyChore.StatesInstance(this);
		base.smi.sm.chitchatlocator.Set(chat_workable, base.smi);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, chat_workable);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x06001742 RID: 5954 RVA: 0x0007DEA4 File Offset: 0x0007C0A4
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.partyer.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
		base.smi.sm.partyer.Get(base.smi).gameObject.AddTag(GameTags.Partying);
	}

	// Token: 0x06001743 RID: 5955 RVA: 0x0007DF0C File Offset: 0x0007C10C
	protected override void End(string reason)
	{
		if (base.smi.sm.partyer.Get(base.smi) != null)
		{
			base.smi.sm.partyer.Get(base.smi).gameObject.RemoveTag(GameTags.Partying);
		}
		base.End(reason);
	}

	// Token: 0x06001744 RID: 5956 RVA: 0x0007DF6D File Offset: 0x0007C16D
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000D0F RID: 3343
	public int basePriority = RELAXATION.PRIORITY.SPECIAL_EVENT;

	// Token: 0x04000D10 RID: 3344
	public const string specificEffect = "Socialized";

	// Token: 0x04000D11 RID: 3345
	public const string trackingEffect = "RecentlySocialized";

	// Token: 0x020011DA RID: 4570
	public class States : GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore>
	{
		// Token: 0x0600814F RID: 33103 RVA: 0x00316A08 File Offset: 0x00314C08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.stand;
			base.Target(this.partyer);
			this.stand.InitializeStates(this.partyer, this.masterTarget, this.chat, null, null, null);
			this.chat_move.InitializeStates(this.partyer, this.chitchatlocator, this.chat, null, null, null);
			this.chat.ToggleWork<Workable>(this.chitchatlocator, this.success, null, null);
			this.success.Enter(delegate(PartyChore.StatesInstance smi)
			{
				this.partyer.Get(smi).gameObject.GetComponent<Effects>().Add("RecentlyPartied", true);
			}).ReturnSuccess();
		}

		// Token: 0x0400619C RID: 24988
		public StateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.TargetParameter partyer;

		// Token: 0x0400619D RID: 24989
		public StateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.TargetParameter chitchatlocator;

		// Token: 0x0400619E RID: 24990
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.ApproachSubState<IApproachable> stand;

		// Token: 0x0400619F RID: 24991
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.ApproachSubState<IApproachable> chat_move;

		// Token: 0x040061A0 RID: 24992
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.State chat;

		// Token: 0x040061A1 RID: 24993
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.State success;
	}

	// Token: 0x020011DB RID: 4571
	public class StatesInstance : GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.GameInstance
	{
		// Token: 0x06008152 RID: 33106 RVA: 0x00316ACE File Offset: 0x00314CCE
		public StatesInstance(PartyChore master) : base(master)
		{
		}
	}
}
