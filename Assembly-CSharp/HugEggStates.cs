using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class HugEggStates : GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>
{
	// Token: 0x06000406 RID: 1030 RVA: 0x0002090C File Offset: 0x0001EB0C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State state = this.root.Enter(new StateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State.Callback(HugEggStates.SetTarget)).Enter(delegate(HugEggStates.Instance smi)
		{
			if (!HugEggStates.Reserve(smi))
			{
				smi.GoTo(this.behaviourcomplete);
			}
		}).Exit(new StateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State.Callback(HugEggStates.Unreserve));
		string name = CREATURES.STATUSITEMS.HUGEGG.NAME;
		string tooltip = CREATURES.STATUSITEMS.HUGEGG.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).OnTargetLost(this.target, this.behaviourcomplete);
		this.moving.MoveTo(new Func<HugEggStates.Instance, int>(HugEggStates.GetClimbableCell), this.hug, this.behaviourcomplete, false);
		this.hug.DefaultState(this.hug.pre).Enter(delegate(HugEggStates.Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Front);
		}).Exit(delegate(HugEggStates.Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
		});
		this.hug.pre.Face(this.target, 0.5f).Enter(delegate(HugEggStates.Instance smi)
		{
			Navigator component = smi.GetComponent<Navigator>();
			if (component.IsValidNavType(NavType.Floor))
			{
				component.SetCurrentNavType(NavType.Floor);
			}
		}).PlayAnim((HugEggStates.Instance smi) => HugEggStates.GetAnims(smi).pre, KAnim.PlayMode.Once).OnAnimQueueComplete(this.hug.loop);
		this.hug.loop.QueueAnim((HugEggStates.Instance smi) => HugEggStates.GetAnims(smi).loop, true, null).ScheduleGoTo((HugEggStates.Instance smi) => smi.def.hugTime, this.hug.pst);
		this.hug.pst.QueueAnim((HugEggStates.Instance smi) => HugEggStates.GetAnims(smi).pst, false, null).Enter(new StateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State.Callback(HugEggStates.ApplyEffect)).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete((HugEggStates.Instance smi) => smi.def.behaviourTag, false);
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x00020B82 File Offset: 0x0001ED82
	private static void SetTarget(HugEggStates.Instance smi)
	{
		smi.sm.target.Set(smi.GetSMI<HugMonitor.Instance>().hugTarget.gameObject, smi, false);
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x00020BA7 File Offset: 0x0001EDA7
	private static HugEggStates.AnimSet GetAnims(HugEggStates.Instance smi)
	{
		if (!(smi.sm.target.Get(smi).GetComponent<EggIncubator>() != null))
		{
			return smi.def.hugAnims;
		}
		return smi.def.incubatorHugAnims;
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00020BE0 File Offset: 0x0001EDE0
	private static bool Reserve(HugEggStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null && !gameObject.HasTag(GameTags.Creatures.ReservedByCreature))
		{
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
			return true;
		}
		return false;
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x00020C24 File Offset: 0x0001EE24
	private static void Unreserve(HugEggStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x00020C57 File Offset: 0x0001EE57
	private static int GetClimbableCell(HugEggStates.Instance smi)
	{
		return Grid.PosToCell(smi.sm.target.Get(smi));
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x00020C70 File Offset: 0x0001EE70
	private static void ApplyEffect(HugEggStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			EggIncubator component = gameObject.GetComponent<EggIncubator>();
			if (component != null && component.Occupant != null)
			{
				component.Occupant.GetComponent<Effects>().Add("EggHug", true);
				return;
			}
			if (gameObject.HasTag(GameTags.Egg))
			{
				gameObject.GetComponent<Effects>().Add("EggHug", true);
			}
		}
	}

	// Token: 0x040002C1 RID: 705
	public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.ApproachSubState<EggIncubator> moving;

	// Token: 0x040002C2 RID: 706
	public HugEggStates.HugState hug;

	// Token: 0x040002C3 RID: 707
	public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State behaviourcomplete;

	// Token: 0x040002C4 RID: 708
	public StateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.TargetParameter target;

	// Token: 0x02001063 RID: 4195
	public class AnimSet
	{
		// Token: 0x04005CAE RID: 23726
		public string pre;

		// Token: 0x04005CAF RID: 23727
		public string loop;

		// Token: 0x04005CB0 RID: 23728
		public string pst;
	}

	// Token: 0x02001064 RID: 4196
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007BDD RID: 31709 RVA: 0x003044CC File Offset: 0x003026CC
		public Def(Tag behaviourTag)
		{
			this.behaviourTag = behaviourTag;
		}

		// Token: 0x04005CB1 RID: 23729
		public float hugTime = 15f;

		// Token: 0x04005CB2 RID: 23730
		public Tag behaviourTag;

		// Token: 0x04005CB3 RID: 23731
		public HugEggStates.AnimSet hugAnims = new HugEggStates.AnimSet
		{
			pre = "hug_egg_pre",
			loop = "hug_egg_loop",
			pst = "hug_egg_pst"
		};

		// Token: 0x04005CB4 RID: 23732
		public HugEggStates.AnimSet incubatorHugAnims = new HugEggStates.AnimSet
		{
			pre = "hug_incubator_pre",
			loop = "hug_incubator_loop",
			pst = "hug_incubator_pst"
		};
	}

	// Token: 0x02001065 RID: 4197
	public new class Instance : GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.GameInstance
	{
		// Token: 0x06007BDE RID: 31710 RVA: 0x00304549 File Offset: 0x00302749
		public Instance(Chore<HugEggStates.Instance> chore, HugEggStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.behaviourTag);
		}
	}

	// Token: 0x02001066 RID: 4198
	public class HugState : GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State
	{
		// Token: 0x04005CB5 RID: 23733
		public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State pre;

		// Token: 0x04005CB6 RID: 23734
		public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State loop;

		// Token: 0x04005CB7 RID: 23735
		public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State pst;
	}
}
