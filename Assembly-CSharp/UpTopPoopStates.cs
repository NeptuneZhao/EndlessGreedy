using System;
using STRINGS;

// Token: 0x020000F6 RID: 246
public class UpTopPoopStates : GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>
{
	// Token: 0x06000483 RID: 1155 RVA: 0x000242A4 File Offset: 0x000224A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtopoop;
		this.root.Enter("SetTarget", delegate(UpTopPoopStates.Instance smi)
		{
			this.targetCell.Set(smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().targetCell, smi, false);
		});
		this.goingtopoop.MoveTo((UpTopPoopStates.Instance smi) => smi.GetPoopCell(), this.pooping, this.pooping, false);
		GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State state = this.pooping.PlayAnim("poop");
		string name = CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Poop, false);
	}

	// Token: 0x04000303 RID: 771
	public GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State goingtopoop;

	// Token: 0x04000304 RID: 772
	public GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State pooping;

	// Token: 0x04000305 RID: 773
	public GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State behaviourcomplete;

	// Token: 0x04000306 RID: 774
	public StateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.IntParameter targetCell;

	// Token: 0x020010AD RID: 4269
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020010AE RID: 4270
	public new class Instance : GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.GameInstance
	{
		// Token: 0x06007CA1 RID: 31905 RVA: 0x0030640C File Offset: 0x0030460C
		public Instance(Chore<UpTopPoopStates.Instance> chore, UpTopPoopStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Poop);
		}

		// Token: 0x06007CA2 RID: 31906 RVA: 0x00306430 File Offset: 0x00304630
		public int GetPoopCell()
		{
			int num = base.master.gameObject.GetComponent<Navigator>().maxProbingRadius - 1;
			int num2 = Grid.PosToCell(base.gameObject);
			int num3 = Grid.OffsetCell(num2, 0, 1);
			while (num > 0 && Grid.IsValidCell(num3) && !Grid.Solid[num3] && !this.IsClosedDoor(num3))
			{
				num--;
				num2 = num3;
				num3 = Grid.OffsetCell(num2, 0, 1);
			}
			return num2;
		}

		// Token: 0x06007CA3 RID: 31907 RVA: 0x003064A0 File Offset: 0x003046A0
		public bool IsClosedDoor(int cellAbove)
		{
			if (Grid.HasDoor[cellAbove])
			{
				Door component = Grid.Objects[cellAbove, 1].GetComponent<Door>();
				return component != null && component.CurrentState != Door.ControlState.Opened;
			}
			return false;
		}
	}
}
