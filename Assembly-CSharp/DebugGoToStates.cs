using System;
using STRINGS;

// Token: 0x020000CB RID: 203
public class DebugGoToStates : GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>
{
	// Token: 0x060003B7 RID: 951 RVA: 0x0001EAD4 File Offset: 0x0001CCD4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State state = this.moving.MoveTo(new Func<DebugGoToStates.Instance, int>(DebugGoToStates.GetTargetCell), this.behaviourcomplete, this.behaviourcomplete, true);
		string name = CREATURES.STATUSITEMS.DEBUGGOTO.NAME;
		string tooltip = CREATURES.STATUSITEMS.DEBUGGOTO.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.behaviourcomplete.BehaviourComplete(GameTags.HasDebugDestination, false);
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x0001EB62 File Offset: 0x0001CD62
	private static int GetTargetCell(DebugGoToStates.Instance smi)
	{
		return smi.GetSMI<CreatureDebugGoToMonitor.Instance>().targetCell;
	}

	// Token: 0x0400028E RID: 654
	public GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State moving;

	// Token: 0x0400028F RID: 655
	public GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State behaviourcomplete;

	// Token: 0x02001025 RID: 4133
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001026 RID: 4134
	public new class Instance : GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.GameInstance
	{
		// Token: 0x06007B5C RID: 31580 RVA: 0x00303743 File Offset: 0x00301943
		public Instance(Chore<DebugGoToStates.Instance> chore, DebugGoToStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.HasDebugDestination);
		}
	}
}
