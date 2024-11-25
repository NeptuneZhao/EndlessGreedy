using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000E5 RID: 229
public class IncubatingStates : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>
{
	// Token: 0x06000424 RID: 1060 RVA: 0x00021590 File Offset: 0x0001F790
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.incubator;
		GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.IN_INCUBATOR.NAME;
		string tooltip = CREATURES.STATUSITEMS.IN_INCUBATOR.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.incubator.DefaultState(this.incubator.idle).ToggleTag(GameTags.Creatures.Deliverable).TagTransition(GameTags.Creatures.InIncubator, null, true);
		this.incubator.idle.Enter("VariantUpdate", new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State.Callback(IncubatingStates.VariantUpdate)).PlayAnim("incubator_idle_loop").OnAnimQueueComplete(this.incubator.choose);
		this.incubator.choose.Transition(this.incubator.variant, new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback(IncubatingStates.DoVariant), UpdateRate.SIM_200ms).Transition(this.incubator.idle, GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Not(new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback(IncubatingStates.DoVariant)), UpdateRate.SIM_200ms);
		this.incubator.variant.PlayAnim("incubator_variant").OnAnimQueueComplete(this.incubator.idle);
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x000216C8 File Offset: 0x0001F8C8
	public static bool DoVariant(IncubatingStates.Instance smi)
	{
		return smi.variant_time == 0;
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x000216D3 File Offset: 0x0001F8D3
	public static void VariantUpdate(IncubatingStates.Instance smi)
	{
		if (smi.variant_time <= 0)
		{
			smi.variant_time = UnityEngine.Random.Range(3, 7);
			return;
		}
		smi.variant_time--;
	}

	// Token: 0x040002CF RID: 719
	public IncubatingStates.IncubatorStates incubator;

	// Token: 0x02001073 RID: 4211
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001074 RID: 4212
	public new class Instance : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.GameInstance
	{
		// Token: 0x06007C05 RID: 31749 RVA: 0x0030486E File Offset: 0x00302A6E
		public Instance(Chore<IncubatingStates.Instance> chore, IncubatingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(IncubatingStates.Instance.IsInIncubator, null);
		}

		// Token: 0x04005CD1 RID: 23761
		public int variant_time = 3;

		// Token: 0x04005CD2 RID: 23762
		public static readonly Chore.Precondition IsInIncubator = new Chore.Precondition
		{
			id = "IsInIncubator",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Creatures.InIncubator);
			}
		};
	}

	// Token: 0x02001075 RID: 4213
	public class IncubatorStates : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State
	{
		// Token: 0x04005CD3 RID: 23763
		public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State idle;

		// Token: 0x04005CD4 RID: 23764
		public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State choose;

		// Token: 0x04005CD5 RID: 23765
		public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State variant;
	}
}
