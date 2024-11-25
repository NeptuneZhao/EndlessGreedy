using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class BaggedStates : GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>
{
	// Token: 0x06000347 RID: 839 RVA: 0x0001B7E8 File Offset: 0x000199E8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.bagged;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.BAGGED.NAME;
		string tooltip = CREATURES.STATUSITEMS.BAGGED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.bagged.Enter(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.BagStart)).ToggleTag(GameTags.Creatures.Deliverable).PlayAnim(new Func<BaggedStates.Instance, string>(BaggedStates.GetBaggedAnimName), KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.Bagged, null, true).Transition(this.escape, new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.Transition.ConditionCallback(BaggedStates.ShouldEscape), UpdateRate.SIM_4000ms).EventHandler(GameHashes.OnStore, new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.OnStore)).Exit(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.BagEnd));
		this.escape.Enter(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.Unbag)).PlayAnim("escape").OnAnimQueueComplete(null);
	}

	// Token: 0x06000348 RID: 840 RVA: 0x0001B8F4 File Offset: 0x00019AF4
	public static string GetBaggedAnimName(BaggedStates.Instance smi)
	{
		return Baggable.GetBaggedAnimName(smi.gameObject);
	}

	// Token: 0x06000349 RID: 841 RVA: 0x0001B901 File Offset: 0x00019B01
	private static void BagStart(BaggedStates.Instance smi)
	{
		if (smi.baggedTime == 0f)
		{
			smi.baggedTime = GameClock.Instance.GetTime();
		}
		smi.UpdateFaller(true);
	}

	// Token: 0x0600034A RID: 842 RVA: 0x0001B927 File Offset: 0x00019B27
	private static void BagEnd(BaggedStates.Instance smi)
	{
		smi.baggedTime = 0f;
		smi.UpdateFaller(false);
	}

	// Token: 0x0600034B RID: 843 RVA: 0x0001B93C File Offset: 0x00019B3C
	private static void Unbag(BaggedStates.Instance smi)
	{
		Baggable component = smi.gameObject.GetComponent<Baggable>();
		if (component)
		{
			component.Free();
		}
	}

	// Token: 0x0600034C RID: 844 RVA: 0x0001B963 File Offset: 0x00019B63
	private static void OnStore(BaggedStates.Instance smi)
	{
		smi.UpdateFaller(true);
	}

	// Token: 0x0600034D RID: 845 RVA: 0x0001B96C File Offset: 0x00019B6C
	private static bool ShouldEscape(BaggedStates.Instance smi)
	{
		return !smi.gameObject.HasTag(GameTags.Stored) && GameClock.Instance.GetTime() - smi.baggedTime >= smi.def.escapeTime;
	}

	// Token: 0x04000257 RID: 599
	public GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State bagged;

	// Token: 0x04000258 RID: 600
	public GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State escape;

	// Token: 0x02000FE4 RID: 4068
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005BB5 RID: 23477
		public float escapeTime = 300f;
	}

	// Token: 0x02000FE5 RID: 4069
	public new class Instance : GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.GameInstance
	{
		// Token: 0x06007AC8 RID: 31432 RVA: 0x00302947 File Offset: 0x00300B47
		public Instance(Chore<BaggedStates.Instance> chore, BaggedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(BaggedStates.Instance.IsBagged, null);
		}

		// Token: 0x06007AC9 RID: 31433 RVA: 0x00302960 File Offset: 0x00300B60
		public void UpdateFaller(bool bagged)
		{
			bool flag = bagged && !base.gameObject.HasTag(GameTags.Stored);
			bool flag2 = GameComps.Fallers.Has(base.gameObject);
			if (flag != flag2)
			{
				if (flag)
				{
					GameComps.Fallers.Add(base.gameObject, Vector2.zero);
					return;
				}
				GameComps.Fallers.Remove(base.gameObject);
			}
		}

		// Token: 0x04005BB6 RID: 23478
		[Serialize]
		public float baggedTime;

		// Token: 0x04005BB7 RID: 23479
		public static readonly Chore.Precondition IsBagged = new Chore.Precondition
		{
			id = "IsBagged",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Creatures.Bagged);
			}
		};
	}
}
