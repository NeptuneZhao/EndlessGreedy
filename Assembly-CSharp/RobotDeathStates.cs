using System;
using STRINGS;

// Token: 0x020000EE RID: 238
public class RobotDeathStates : GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>
{
	// Token: 0x0600044B RID: 1099 RVA: 0x000227B8 File Offset: 0x000209B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State state = this.loop;
		string name = CREATURES.STATUSITEMS.DEAD.NAME;
		string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).PlayAnim((RobotDeathStates.Instance smi) => smi.def.deathAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pst);
		this.pst.TriggerOnEnter(GameHashes.DeathAnimComplete, null).TriggerOnEnter(GameHashes.Died, (RobotDeathStates.Instance smi) => smi.gameObject).BehaviourComplete(GameTags.Creatures.Die, false);
	}

	// Token: 0x040002E8 RID: 744
	private GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State loop;

	// Token: 0x040002E9 RID: 745
	private GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State pst;

	// Token: 0x02001091 RID: 4241
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005D2A RID: 23850
		public string deathAnim = "death";
	}

	// Token: 0x02001092 RID: 4242
	public new class Instance : GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.GameInstance
	{
		// Token: 0x06007C5C RID: 31836 RVA: 0x00305640 File Offset: 0x00303840
		public Instance(Chore<RobotDeathStates.Instance> chore, RobotDeathStates.Def def) : base(chore, def)
		{
			chore.choreType.interruptPriority = Db.Get().ChoreTypes.Die.interruptPriority;
			chore.masterPriority.priority_class = PriorityScreen.PriorityClass.compulsory;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Die);
		}
	}
}
