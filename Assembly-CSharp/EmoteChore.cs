using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000438 RID: 1080
public class EmoteChore : Chore<EmoteChore.StatesInstance>
{
	// Token: 0x060016EE RID: 5870 RVA: 0x0007BE78 File Offset: 0x0007A078
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, Emote emote, int emoteIterations = 1, Func<StatusItem> get_status_item = null) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, emote, KAnim.PlayMode.Once, emoteIterations, false);
		this.getStatusItem = get_status_item;
	}

	// Token: 0x060016EF RID: 5871 RVA: 0x0007BEC0 File Offset: 0x0007A0C0
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, Emote emote, KAnim.PlayMode play_mode, int emoteIterations = 1, bool flip_x = false) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, emote, play_mode, emoteIterations, flip_x);
	}

	// Token: 0x060016F0 RID: 5872 RVA: 0x0007BF00 File Offset: 0x0007A100
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString animFile, HashedString[] anims, Func<StatusItem> get_status_item = null) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, animFile, anims, KAnim.PlayMode.Once, false);
		this.getStatusItem = get_status_item;
	}

	// Token: 0x060016F1 RID: 5873 RVA: 0x0007BF48 File Offset: 0x0007A148
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString animFile, HashedString[] anims, KAnim.PlayMode play_mode, bool flip_x = false) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, animFile, anims, play_mode, flip_x);
	}

	// Token: 0x060016F2 RID: 5874 RVA: 0x0007BF88 File Offset: 0x0007A188
	protected override StatusItem GetStatusItem()
	{
		if (this.getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return this.getStatusItem();
	}

	// Token: 0x060016F3 RID: 5875 RVA: 0x0007BFA4 File Offset: 0x0007A1A4
	public override string ToString()
	{
		if (base.smi.animFile != null)
		{
			return "EmoteChore<" + base.smi.animFile.GetData().name + ">";
		}
		string str = "EmoteChore<";
		HashedString hashedString = base.smi.emoteAnims[0];
		return str + hashedString.ToString() + ">";
	}

	// Token: 0x060016F4 RID: 5876 RVA: 0x0007C017 File Offset: 0x0007A217
	public void PairReactable(SelfEmoteReactable reactable)
	{
		this.reactable = reactable;
	}

	// Token: 0x060016F5 RID: 5877 RVA: 0x0007C020 File Offset: 0x0007A220
	protected new virtual void End(string reason)
	{
		if (this.reactable != null)
		{
			this.reactable.PairEmote(null);
			this.reactable.Cleanup();
			this.reactable = null;
		}
		base.End(reason);
	}

	// Token: 0x04000CF5 RID: 3317
	private Func<StatusItem> getStatusItem;

	// Token: 0x04000CF6 RID: 3318
	private SelfEmoteReactable reactable;

	// Token: 0x020011B3 RID: 4531
	public class StatesInstance : GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.GameInstance
	{
		// Token: 0x060080D4 RID: 32980 RVA: 0x003131EC File Offset: 0x003113EC
		public StatesInstance(EmoteChore master, GameObject emoter, Emote emote, KAnim.PlayMode mode, int emoteIterations, bool flip_x) : base(master)
		{
			this.mode = mode;
			this.animFile = emote.AnimSet;
			emote.CollectStepAnims(out this.emoteAnims, emoteIterations);
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x060080D5 RID: 32981 RVA: 0x00313244 File Offset: 0x00311444
		public StatesInstance(EmoteChore master, GameObject emoter, HashedString animFile, HashedString[] anims, KAnim.PlayMode mode, bool flip_x) : base(master)
		{
			this.mode = mode;
			this.animFile = Assets.GetAnim(animFile);
			this.emoteAnims = anims;
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x04006110 RID: 24848
		public KAnimFile animFile;

		// Token: 0x04006111 RID: 24849
		public HashedString[] emoteAnims;

		// Token: 0x04006112 RID: 24850
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}

	// Token: 0x020011B4 RID: 4532
	public class States : GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore>
	{
		// Token: 0x060080D6 RID: 32982 RVA: 0x00313294 File Offset: 0x00311494
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.emoter);
			this.root.ToggleAnims((EmoteChore.StatesInstance smi) => smi.animFile).PlayAnims((EmoteChore.StatesInstance smi) => smi.emoteAnims, (EmoteChore.StatesInstance smi) => smi.mode).ScheduleGoTo(10f, this.finish).OnAnimQueueComplete(this.finish);
			this.finish.ReturnSuccess();
		}

		// Token: 0x04006113 RID: 24851
		public StateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.TargetParameter emoter;

		// Token: 0x04006114 RID: 24852
		public GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.State finish;
	}
}
