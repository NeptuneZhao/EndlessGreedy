using System;
using STRINGS;

// Token: 0x020000C3 RID: 195
public class CallAdultStates : GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>
{
	// Token: 0x06000381 RID: 897 RVA: 0x0001D0E8 File Offset: 0x0001B2E8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.SLEEPING.NAME;
		string tooltip = CREATURES.STATUSITEMS.SLEEPING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.pre.QueueAnim("call_pre", false, null).OnAnimQueueComplete(this.loop);
		this.loop.QueueAnim("call_loop", false, null).OnAnimQueueComplete(this.pst);
		this.pst.QueueAnim("call_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.CallAdultBehaviour, false);
	}

	// Token: 0x04000270 RID: 624
	public GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State pre;

	// Token: 0x04000271 RID: 625
	public GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State loop;

	// Token: 0x04000272 RID: 626
	public GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State pst;

	// Token: 0x04000273 RID: 627
	public GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State behaviourcomplete;

	// Token: 0x02001007 RID: 4103
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001008 RID: 4104
	public new class Instance : GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.GameInstance
	{
		// Token: 0x06007B0B RID: 31499 RVA: 0x00302FD0 File Offset: 0x003011D0
		public Instance(Chore<CallAdultStates.Instance> chore, CallAdultStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.CallAdultBehaviour);
		}
	}
}
