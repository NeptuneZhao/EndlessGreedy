using System;
using UnityEngine;

// Token: 0x02000458 RID: 1112
public class StressEmoteChore : Chore<StressEmoteChore.StatesInstance>
{
	// Token: 0x06001776 RID: 6006 RVA: 0x0007F350 File Offset: 0x0007D550
	public StressEmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode play_mode, Func<StatusItem> get_status_item) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.AddPrecondition(ChorePreconditions.instance.IsMoving, null);
		this.AddPrecondition(ChorePreconditions.instance.IsOffLadder, null);
		this.AddPrecondition(ChorePreconditions.instance.NotInTube, null);
		this.AddPrecondition(ChorePreconditions.instance.IsAwake, null);
		this.getStatusItem = get_status_item;
		base.smi = new StressEmoteChore.StatesInstance(this, target.gameObject, emote_kanim, emote_anims, play_mode);
	}

	// Token: 0x06001777 RID: 6007 RVA: 0x0007F3DA File Offset: 0x0007D5DA
	protected override StatusItem GetStatusItem()
	{
		if (this.getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return this.getStatusItem();
	}

	// Token: 0x06001778 RID: 6008 RVA: 0x0007F3F8 File Offset: 0x0007D5F8
	public override string ToString()
	{
		HashedString hashedString;
		if (base.smi.emoteKAnim.IsValid)
		{
			string str = "StressEmoteChore<";
			hashedString = base.smi.emoteKAnim;
			return str + hashedString.ToString() + ">";
		}
		string str2 = "StressEmoteChore<";
		hashedString = base.smi.emoteAnims[0];
		return str2 + hashedString.ToString() + ">";
	}

	// Token: 0x04000D1B RID: 3355
	private Func<StatusItem> getStatusItem;

	// Token: 0x02001202 RID: 4610
	public class StatesInstance : GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.GameInstance
	{
		// Token: 0x060081E9 RID: 33257 RVA: 0x0031A313 File Offset: 0x00318513
		public StatesInstance(StressEmoteChore master, GameObject emoter, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode mode) : base(master)
		{
			this.emoteKAnim = emote_kanim;
			this.emoteAnims = emote_anims;
			this.mode = mode;
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x04006217 RID: 25111
		public HashedString[] emoteAnims;

		// Token: 0x04006218 RID: 25112
		public HashedString emoteKAnim;

		// Token: 0x04006219 RID: 25113
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}

	// Token: 0x02001203 RID: 4611
	public class States : GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore>
	{
		// Token: 0x060081EA RID: 33258 RVA: 0x0031A354 File Offset: 0x00318554
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.emoter);
			this.root.ToggleAnims((StressEmoteChore.StatesInstance smi) => smi.emoteKAnim).ToggleThought(Db.Get().Thoughts.Unhappy, null).PlayAnims((StressEmoteChore.StatesInstance smi) => smi.emoteAnims, (StressEmoteChore.StatesInstance smi) => smi.mode).OnAnimQueueComplete(null);
		}

		// Token: 0x0400621A RID: 25114
		public StateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.TargetParameter emoter;
	}
}
