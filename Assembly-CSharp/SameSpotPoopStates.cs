using System;
using KSerialization;
using STRINGS;

// Token: 0x020000EF RID: 239
public class SameSpotPoopStates : GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>
{
	// Token: 0x0600044D RID: 1101 RVA: 0x00022894 File Offset: 0x00020A94
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtopoop;
		this.root.Enter("SetTarget", delegate(SameSpotPoopStates.Instance smi)
		{
			this.targetCell.Set(smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().targetCell, smi, false);
		});
		this.goingtopoop.MoveTo((SameSpotPoopStates.Instance smi) => smi.GetLastPoopCell(), this.pooping, this.updatepoopcell, false);
		GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State state = this.pooping.PlayAnim("poop");
		string name = CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).OnAnimQueueComplete(this.behaviourcomplete);
		this.updatepoopcell.Enter(delegate(SameSpotPoopStates.Instance smi)
		{
			smi.SetLastPoopCell();
		}).GoTo(this.pooping);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Poop, false);
	}

	// Token: 0x040002EA RID: 746
	public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State goingtopoop;

	// Token: 0x040002EB RID: 747
	public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State pooping;

	// Token: 0x040002EC RID: 748
	public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State behaviourcomplete;

	// Token: 0x040002ED RID: 749
	public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State updatepoopcell;

	// Token: 0x040002EE RID: 750
	public StateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.IntParameter targetCell;

	// Token: 0x02001094 RID: 4244
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001095 RID: 4245
	public new class Instance : GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.GameInstance
	{
		// Token: 0x06007C62 RID: 31842 RVA: 0x003056CB File Offset: 0x003038CB
		public Instance(Chore<SameSpotPoopStates.Instance> chore, SameSpotPoopStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Poop);
		}

		// Token: 0x06007C63 RID: 31843 RVA: 0x003056F6 File Offset: 0x003038F6
		public int GetLastPoopCell()
		{
			if (this.lastPoopCell == -1)
			{
				this.SetLastPoopCell();
			}
			return this.lastPoopCell;
		}

		// Token: 0x06007C64 RID: 31844 RVA: 0x0030570D File Offset: 0x0030390D
		public void SetLastPoopCell()
		{
			this.lastPoopCell = Grid.PosToCell(this);
		}

		// Token: 0x04005D2E RID: 23854
		[Serialize]
		private int lastPoopCell = -1;
	}
}
