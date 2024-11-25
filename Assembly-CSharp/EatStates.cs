using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000D2 RID: 210
public class EatStates : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>
{
	// Token: 0x060003D7 RID: 983 RVA: 0x0001F558 File Offset: 0x0001D758
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtoeat;
		this.root.Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.SetTarget)).Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.SetOffset)).Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.ReserveEdible)).Exit(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.UnreserveEdible));
		GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state = this.goingtoeat.MoveTo(new Func<EatStates.Instance, int>(EatStates.GetEdibleCell), this.eating, null, false);
		string name = CREATURES.STATUSITEMS.HUNGRY.NAME;
		string tooltip = CREATURES.STATUSITEMS.HUNGRY.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state2 = this.eating.Face(this.target, 0f).DefaultState(this.eating.pre);
		string name2 = CREATURES.STATUSITEMS.EATING.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.EATING.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
		this.eating.pre.QueueAnim((EatStates.Instance smi) => smi.eatAnims[0], false, null).OnAnimQueueComplete(this.eating.loop);
		this.eating.loop.Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.EatComplete)).QueueAnim((EatStates.Instance smi) => smi.eatAnims[1], true, null).ScheduleGoTo(3f, this.eating.pst);
		this.eating.pst.QueueAnim((EatStates.Instance smi) => smi.eatAnims[2], false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.WantsToEat, false);
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0001F777 File Offset: 0x0001D977
	private static void SetTarget(EatStates.Instance smi)
	{
		smi.sm.target.Set(smi.GetSMI<SolidConsumerMonitor.Instance>().targetEdible, smi, false);
		smi.OverrideEatAnims(smi, smi.GetSMI<SolidConsumerMonitor.Instance>().GetTargetEdibleEatAnims());
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0001F7A9 File Offset: 0x0001D9A9
	private static void SetOffset(EatStates.Instance smi)
	{
		smi.sm.offset.Set(smi.GetSMI<SolidConsumerMonitor.Instance>().targetEdibleOffset, smi, false);
	}

	// Token: 0x060003DA RID: 986 RVA: 0x0001F7CC File Offset: 0x0001D9CC
	private static void ReserveEdible(EatStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x060003DB RID: 987 RVA: 0x0001F814 File Offset: 0x0001DA14
	private static void UnreserveEdible(EatStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			if (gameObject.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(smi.gameObject, "{0} UnreserveEdible but it wasn't reserved: {1}", new object[]
			{
				smi.gameObject,
				gameObject
			});
		}
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0001F878 File Offset: 0x0001DA78
	private static void EatComplete(EatStates.Instance smi)
	{
		PrimaryElement primaryElement = smi.sm.target.Get<PrimaryElement>(smi);
		if (primaryElement != null)
		{
			smi.lastMealElement = primaryElement.Element;
		}
		smi.Trigger(1386391852, smi.sm.target.Get<KPrefabID>(smi));
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0001F8C8 File Offset: 0x0001DAC8
	private static int GetEdibleCell(EatStates.Instance smi)
	{
		return Grid.PosToCell(smi.sm.target.Get(smi).transform.GetPosition() + smi.sm.offset.Get(smi));
	}

	// Token: 0x040002A2 RID: 674
	public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.ApproachSubState<Pickupable> goingtoeat;

	// Token: 0x040002A3 RID: 675
	public EatStates.EatingState eating;

	// Token: 0x040002A4 RID: 676
	public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State behaviourcomplete;

	// Token: 0x040002A5 RID: 677
	public StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.Vector3Parameter offset;

	// Token: 0x040002A6 RID: 678
	public StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.TargetParameter target;

	// Token: 0x0200103C RID: 4156
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200103D RID: 4157
	public new class Instance : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.GameInstance
	{
		// Token: 0x06007B86 RID: 31622 RVA: 0x00303B85 File Offset: 0x00301D85
		public void OverrideEatAnims(EatStates.Instance smi, string[] preLoopPstAnims)
		{
			global::Debug.Assert(preLoopPstAnims != null && preLoopPstAnims.Length == 3);
			smi.eatAnims = preLoopPstAnims;
		}

		// Token: 0x06007B87 RID: 31623 RVA: 0x00303BA0 File Offset: 0x00301DA0
		public Instance(Chore<EatStates.Instance> chore, EatStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToEat);
		}

		// Token: 0x06007B88 RID: 31624 RVA: 0x00303BF3 File Offset: 0x00301DF3
		public Element GetLatestMealElement()
		{
			return this.lastMealElement;
		}

		// Token: 0x04005C70 RID: 23664
		public Element lastMealElement;

		// Token: 0x04005C71 RID: 23665
		public string[] eatAnims = new string[]
		{
			"eat_pre",
			"eat_loop",
			"eat_pst"
		};
	}

	// Token: 0x0200103E RID: 4158
	public class EatingState : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State
	{
		// Token: 0x04005C72 RID: 23666
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pre;

		// Token: 0x04005C73 RID: 23667
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State loop;

		// Token: 0x04005C74 RID: 23668
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pst;
	}
}
