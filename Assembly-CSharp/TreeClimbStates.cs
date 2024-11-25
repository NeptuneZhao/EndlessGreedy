using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class TreeClimbStates : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>
{
	// Token: 0x0600047B RID: 1147 RVA: 0x00023FB8 File Offset: 0x000221B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State state = this.root.Enter(new StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback(TreeClimbStates.SetTarget)).Enter(delegate(TreeClimbStates.Instance smi)
		{
			if (!TreeClimbStates.ReserveClimbable(smi))
			{
				smi.GoTo(this.behaviourcomplete);
			}
		}).Exit(new StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback(TreeClimbStates.UnreserveClimbable));
		string name = CREATURES.STATUSITEMS.RUMMAGINGSEED.NAME;
		string tooltip = CREATURES.STATUSITEMS.RUMMAGINGSEED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.moving.MoveTo(new Func<TreeClimbStates.Instance, int>(TreeClimbStates.GetClimbableCell), this.climbing, this.behaviourcomplete, false);
		this.climbing.DefaultState(this.climbing.pre);
		this.climbing.pre.PlayAnim("rummage_pre").OnAnimQueueComplete(this.climbing.loop);
		this.climbing.loop.QueueAnim("rummage_loop", true, null).ScheduleGoTo(3.5f, this.climbing.pst).Update(new Action<TreeClimbStates.Instance, float>(TreeClimbStates.Rummage), UpdateRate.SIM_1000ms, false);
		this.climbing.pst.QueueAnim("rummage_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToClimbTree, false);
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x00024120 File Offset: 0x00022320
	private static void SetTarget(TreeClimbStates.Instance smi)
	{
		smi.sm.target.Set(smi.GetSMI<ClimbableTreeMonitor.Instance>().climbTarget, smi, false);
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x00024140 File Offset: 0x00022340
	private static bool ReserveClimbable(TreeClimbStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null && !gameObject.HasTag(GameTags.Creatures.ReservedByCreature))
		{
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
			return true;
		}
		return false;
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x00024184 File Offset: 0x00022384
	private static void UnreserveClimbable(TreeClimbStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x000241B8 File Offset: 0x000223B8
	private static void Rummage(TreeClimbStates.Instance smi, float dt)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			ForestTreeSeedMonitor component = gameObject.GetComponent<ForestTreeSeedMonitor>();
			if (component != null)
			{
				component.ExtractExtraSeed();
				return;
			}
			Storage component2 = gameObject.GetComponent<Storage>();
			if (component2 && component2.items.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, component2.items.Count - 1);
				GameObject gameObject2 = component2.items[index];
				Pickupable pickupable = gameObject2 ? gameObject2.GetComponent<Pickupable>() : null;
				if (pickupable && pickupable.UnreservedAmount > 0.01f)
				{
					smi.Toss(pickupable);
				}
			}
		}
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0002426D File Offset: 0x0002246D
	private static int GetClimbableCell(TreeClimbStates.Instance smi)
	{
		return Grid.PosToCell(smi.sm.target.Get(smi));
	}

	// Token: 0x040002FF RID: 767
	public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.ApproachSubState<Uprootable> moving;

	// Token: 0x04000300 RID: 768
	public TreeClimbStates.ClimbState climbing;

	// Token: 0x04000301 RID: 769
	public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State behaviourcomplete;

	// Token: 0x04000302 RID: 770
	public StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.TargetParameter target;

	// Token: 0x020010AA RID: 4266
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020010AB RID: 4267
	public new class Instance : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.GameInstance
	{
		// Token: 0x06007C9B RID: 31899 RVA: 0x0030625C File Offset: 0x0030445C
		public Instance(Chore<TreeClimbStates.Instance> chore, TreeClimbStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToClimbTree);
			this.storage = base.GetComponent<Storage>();
		}

		// Token: 0x06007C9C RID: 31900 RVA: 0x0030628C File Offset: 0x0030448C
		public void Toss(Pickupable pu)
		{
			Pickupable pickupable = pu.Take(Mathf.Min(1f, pu.UnreservedAmount));
			if (pickupable != null)
			{
				this.storage.Store(pickupable.gameObject, true, false, true, false);
				this.storage.Drop(pickupable.gameObject, true);
				this.Throw(pickupable.gameObject);
			}
		}

		// Token: 0x06007C9D RID: 31901 RVA: 0x003062F0 File Offset: 0x003044F0
		private void Throw(GameObject ore_go)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			int num = Grid.PosToCell(position);
			int num2 = Grid.CellAbove(num);
			Vector2 zero;
			if ((Grid.IsValidCell(num) && Grid.Solid[num]) || (Grid.IsValidCell(num2) && Grid.Solid[num2]))
			{
				zero = Vector2.zero;
			}
			else
			{
				position.y += 0.5f;
				zero = new Vector2(UnityEngine.Random.Range(TreeClimbStates.Instance.VEL_MIN.x, TreeClimbStates.Instance.VEL_MAX.x), UnityEngine.Random.Range(TreeClimbStates.Instance.VEL_MIN.y, TreeClimbStates.Instance.VEL_MAX.y));
			}
			ore_go.transform.SetPosition(position);
			if (GameComps.Fallers.Has(ore_go))
			{
				GameComps.Fallers.Remove(ore_go);
			}
			GameComps.Fallers.Add(ore_go, zero);
		}

		// Token: 0x04005D73 RID: 23923
		private Storage storage;

		// Token: 0x04005D74 RID: 23924
		private static readonly Vector2 VEL_MIN = new Vector2(-1f, 2f);

		// Token: 0x04005D75 RID: 23925
		private static readonly Vector2 VEL_MAX = new Vector2(1f, 4f);
	}

	// Token: 0x020010AC RID: 4268
	public class ClimbState : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State
	{
		// Token: 0x04005D76 RID: 23926
		public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State pre;

		// Token: 0x04005D77 RID: 23927
		public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State loop;

		// Token: 0x04005D78 RID: 23928
		public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State pst;
	}
}
