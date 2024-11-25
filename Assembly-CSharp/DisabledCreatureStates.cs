using System;
using STRINGS;

// Token: 0x020000CE RID: 206
public class DisabledCreatureStates : GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>
{
	// Token: 0x060003C1 RID: 961 RVA: 0x0001ED90 File Offset: 0x0001CF90
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.disableCreature;
		GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.DISABLED.NAME;
		string tooltip = CREATURES.STATUSITEMS.DISABLED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).TagTransition(GameTags.Creatures.Behaviours.DisableCreature, this.behaviourcomplete, true);
		this.disableCreature.PlayAnim((DisabledCreatureStates.Instance smi) => smi.def.disabledAnim, KAnim.PlayMode.Once);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.DisableCreature, false);
	}

	// Token: 0x04000296 RID: 662
	public GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State disableCreature;

	// Token: 0x04000297 RID: 663
	public GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State behaviourcomplete;

	// Token: 0x0200102E RID: 4142
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007B68 RID: 31592 RVA: 0x00303838 File Offset: 0x00301A38
		public Def(string anim)
		{
			this.disabledAnim = anim;
		}

		// Token: 0x04005C5D RID: 23645
		public string disabledAnim = "off";
	}

	// Token: 0x0200102F RID: 4143
	public new class Instance : GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.GameInstance
	{
		// Token: 0x06007B69 RID: 31593 RVA: 0x00303852 File Offset: 0x00301A52
		public Instance(Chore<DisabledCreatureStates.Instance> chore, DisabledCreatureStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.HasTag, GameTags.Creatures.Behaviours.DisableCreature);
		}
	}
}
