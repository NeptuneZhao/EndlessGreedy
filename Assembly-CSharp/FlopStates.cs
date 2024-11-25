using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class FlopStates : GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>
{
	// Token: 0x060003ED RID: 1005 RVA: 0x0002004C File Offset: 0x0001E24C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.flop_pre;
		GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.FLOPPING.NAME;
		string tooltip = CREATURES.STATUSITEMS.FLOPPING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.flop_pre.Enter(new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State.Callback(FlopStates.ChooseDirection)).Transition(this.flop_cycle, new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.ShouldFlop), UpdateRate.SIM_200ms).Transition(this.pst, GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Not(new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.ShouldFlop)), UpdateRate.SIM_200ms);
		this.flop_cycle.PlayAnim("flop_loop", KAnim.PlayMode.Once).Transition(this.pst, new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.IsSubstantialLiquid), UpdateRate.SIM_200ms).Update("Flop", new Action<FlopStates.Instance, float>(FlopStates.FlopForward), UpdateRate.SIM_33ms, false).OnAnimQueueComplete(this.flop_pre);
		this.pst.QueueAnim("flop_loop", true, null).BehaviourComplete(GameTags.Creatures.Flopping, false);
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x00020164 File Offset: 0x0001E364
	public static bool ShouldFlop(FlopStates.Instance smi)
	{
		int num = Grid.CellBelow(Grid.PosToCell(smi.transform.GetPosition()));
		return Grid.IsValidCell(num) && Grid.Solid[num];
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0002019C File Offset: 0x0001E39C
	public static void ChooseDirection(FlopStates.Instance smi)
	{
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		if (FlopStates.SearchForLiquid(cell, 1))
		{
			smi.currentDir = 1f;
			return;
		}
		if (FlopStates.SearchForLiquid(cell, -1))
		{
			smi.currentDir = -1f;
			return;
		}
		if (UnityEngine.Random.value > 0.5f)
		{
			smi.currentDir = 1f;
			return;
		}
		smi.currentDir = -1f;
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x00020208 File Offset: 0x0001E408
	private static bool SearchForLiquid(int cell, int delta_x)
	{
		while (Grid.IsValidCell(cell))
		{
			if (Grid.IsSubstantialLiquid(cell, 0.35f))
			{
				return true;
			}
			if (Grid.Solid[cell])
			{
				return false;
			}
			if (Grid.CritterImpassable[cell])
			{
				return false;
			}
			int num = Grid.CellBelow(cell);
			if (Grid.IsValidCell(num) && Grid.Solid[num])
			{
				cell += delta_x;
			}
			else
			{
				cell = num;
			}
		}
		return false;
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x00020274 File Offset: 0x0001E474
	public static void FlopForward(FlopStates.Instance smi, float dt)
	{
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		int currentFrame = component.currentFrame;
		if (component.IsVisible() && (currentFrame < 23 || currentFrame > 36))
		{
			return;
		}
		Vector3 position = smi.transform.GetPosition();
		Vector3 vector = position;
		vector.x = position.x + smi.currentDir * dt * 1f;
		int num = Grid.PosToCell(vector);
		if (Grid.IsValidCell(num) && !Grid.Solid[num] && !Grid.CritterImpassable[num])
		{
			smi.transform.SetPosition(vector);
			return;
		}
		smi.currentDir = -smi.currentDir;
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0002030E File Offset: 0x0001E50E
	public static bool IsSubstantialLiquid(FlopStates.Instance smi)
	{
		return Grid.IsSubstantialLiquid(Grid.PosToCell(smi.transform.GetPosition()), 0.35f);
	}

	// Token: 0x040002B4 RID: 692
	private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State flop_pre;

	// Token: 0x040002B5 RID: 693
	private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State flop_cycle;

	// Token: 0x040002B6 RID: 694
	private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State pst;

	// Token: 0x0200104C RID: 4172
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200104D RID: 4173
	public new class Instance : GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.GameInstance
	{
		// Token: 0x06007BAB RID: 31659 RVA: 0x00303F06 File Offset: 0x00302106
		public Instance(Chore<FlopStates.Instance> chore, FlopStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Flopping);
		}

		// Token: 0x04005C8B RID: 23691
		public float currentDir = 1f;
	}
}
