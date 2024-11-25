﻿using System;
using STRINGS;

// Token: 0x020000D0 RID: 208
public class DropElementStates : GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>
{
	// Token: 0x060003D2 RID: 978 RVA: 0x0001F354 File Offset: 0x0001D554
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.dropping;
		GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.EXPELLING_GAS.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXPELLING_GAS.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.dropping.PlayAnim("dirty").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.Enter("DropElement", delegate(DropElementStates.Instance smi)
		{
			smi.GetSMI<ElementDropperMonitor.Instance>().DropPeriodicElement();
		}).QueueAnim("idle_loop", true, null).BehaviourComplete(GameTags.Creatures.WantsToDropElements, false);
	}

	// Token: 0x0400029D RID: 669
	public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State dropping;

	// Token: 0x0400029E RID: 670
	public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State behaviourcomplete;

	// Token: 0x02001035 RID: 4149
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001036 RID: 4150
	public new class Instance : GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.GameInstance
	{
		// Token: 0x06007B7A RID: 31610 RVA: 0x00303AC8 File Offset: 0x00301CC8
		public Instance(Chore<DropElementStates.Instance> chore, DropElementStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToDropElements);
		}
	}
}
