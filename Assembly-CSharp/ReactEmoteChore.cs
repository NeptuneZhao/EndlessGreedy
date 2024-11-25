using System;
using UnityEngine;

// Token: 0x0200044C RID: 1100
public class ReactEmoteChore : Chore<ReactEmoteChore.StatesInstance>
{
	// Token: 0x0600174A RID: 5962 RVA: 0x0007E1A4 File Offset: 0x0007C3A4
	public ReactEmoteChore(IStateMachineTarget target, ChoreType chore_type, EmoteReactable reactable, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode play_mode, Func<StatusItem> get_status_item) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.AddPrecondition(ChorePreconditions.instance.IsMoving, null);
		this.AddPrecondition(ChorePreconditions.instance.IsOffLadder, null);
		this.AddPrecondition(ChorePreconditions.instance.NotInTube, null);
		this.AddPrecondition(ChorePreconditions.instance.IsAwake, null);
		this.getStatusItem = get_status_item;
		base.smi = new ReactEmoteChore.StatesInstance(this, target.gameObject, reactable, emote_kanim, emote_anims, play_mode);
	}

	// Token: 0x0600174B RID: 5963 RVA: 0x0007E230 File Offset: 0x0007C430
	protected override StatusItem GetStatusItem()
	{
		if (this.getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return this.getStatusItem();
	}

	// Token: 0x0600174C RID: 5964 RVA: 0x0007E24C File Offset: 0x0007C44C
	public override string ToString()
	{
		HashedString hashedString;
		if (base.smi.emoteKAnim.IsValid)
		{
			string str = "ReactEmoteChore<";
			hashedString = base.smi.emoteKAnim;
			return str + hashedString.ToString() + ">";
		}
		string str2 = "ReactEmoteChore<";
		hashedString = base.smi.emoteAnims[0];
		return str2 + hashedString.ToString() + ">";
	}

	// Token: 0x04000D13 RID: 3347
	private Func<StatusItem> getStatusItem;

	// Token: 0x020011E3 RID: 4579
	public class StatesInstance : GameStateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.GameInstance
	{
		// Token: 0x0600816B RID: 33131 RVA: 0x0031739C File Offset: 0x0031559C
		public StatesInstance(ReactEmoteChore master, GameObject emoter, EmoteReactable reactable, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode mode) : base(master)
		{
			this.emoteKAnim = emote_kanim;
			this.emoteAnims = emote_anims;
			this.mode = mode;
			base.sm.reactable.Set(reactable, base.smi, false);
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x040061B3 RID: 25011
		public HashedString[] emoteAnims;

		// Token: 0x040061B4 RID: 25012
		public HashedString emoteKAnim;

		// Token: 0x040061B5 RID: 25013
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}

	// Token: 0x020011E4 RID: 4580
	public class States : GameStateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore>
	{
		// Token: 0x0600816C RID: 33132 RVA: 0x00317404 File Offset: 0x00315604
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.emoter);
			this.root.ToggleThought((ReactEmoteChore.StatesInstance smi) => this.reactable.Get(smi).thought).ToggleExpression((ReactEmoteChore.StatesInstance smi) => this.reactable.Get(smi).expression).ToggleAnims((ReactEmoteChore.StatesInstance smi) => smi.emoteKAnim).ToggleThought(Db.Get().Thoughts.Unhappy, null).PlayAnims((ReactEmoteChore.StatesInstance smi) => smi.emoteAnims, (ReactEmoteChore.StatesInstance smi) => smi.mode).OnAnimQueueComplete(null).Enter(delegate(ReactEmoteChore.StatesInstance smi)
			{
				smi.master.GetComponent<Facing>().Face(Grid.CellToPos(this.reactable.Get(smi).sourceCell));
			});
		}

		// Token: 0x040061B6 RID: 25014
		public StateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.TargetParameter emoter;

		// Token: 0x040061B7 RID: 25015
		public StateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.ObjectParameter<EmoteReactable> reactable;
	}
}
