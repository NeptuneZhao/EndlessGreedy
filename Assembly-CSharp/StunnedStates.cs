using System;
using STRINGS;

// Token: 0x020000F3 RID: 243
public class StunnedStates : GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>
{
	// Token: 0x06000476 RID: 1142 RVA: 0x00023DA8 File Offset: 0x00021FA8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.stunned;
		GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
		string tooltip = CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.stunned.PlayAnim("idle_loop", KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.Stunned, null, true);
	}

	// Token: 0x040002FC RID: 764
	public GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State stunned;

	// Token: 0x020010A4 RID: 4260
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020010A5 RID: 4261
	public new class Instance : GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.GameInstance
	{
		// Token: 0x06007C91 RID: 31889 RVA: 0x0030616A File Offset: 0x0030436A
		public Instance(Chore<StunnedStates.Instance> chore, StunnedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(StunnedStates.Instance.IsStunned, null);
		}

		// Token: 0x04005D6F RID: 23919
		public static readonly Chore.Precondition IsStunned = new Chore.Precondition
		{
			id = "IsStunned",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Creatures.Stunned);
			}
		};
	}
}
