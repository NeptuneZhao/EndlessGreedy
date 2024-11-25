using System;
using STRINGS;

// Token: 0x020000C7 RID: 199
public class CreatureSleepStates : GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>
{
	// Token: 0x06000398 RID: 920 RVA: 0x0001DCE0 File Offset: 0x0001BEE0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.SLEEPING.NAME;
		string tooltip = CREATURES.STATUSITEMS.SLEEPING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.pre.QueueAnim("sleep_pre", false, null).OnAnimQueueComplete(this.loop);
		this.loop.QueueAnim("sleep_loop", true, null).Transition(this.pst, new StateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.Transition.ConditionCallback(CreatureSleepStates.ShouldWakeUp), UpdateRate.SIM_1000ms);
		this.pst.QueueAnim("sleep_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.SleepBehaviour, false);
	}

	// Token: 0x06000399 RID: 921 RVA: 0x0001DDB7 File Offset: 0x0001BFB7
	public static bool ShouldWakeUp(CreatureSleepStates.Instance smi)
	{
		return !GameClock.Instance.IsNighttime();
	}

	// Token: 0x0400027C RID: 636
	public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State pre;

	// Token: 0x0400027D RID: 637
	public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State loop;

	// Token: 0x0400027E RID: 638
	public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State pst;

	// Token: 0x0400027F RID: 639
	public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State behaviourcomplete;

	// Token: 0x02001016 RID: 4118
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001017 RID: 4119
	public new class Instance : GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.GameInstance
	{
		// Token: 0x06007B36 RID: 31542 RVA: 0x0030336E File Offset: 0x0030156E
		public Instance(Chore<CreatureSleepStates.Instance> chore, CreatureSleepStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.SleepBehaviour);
		}
	}
}
