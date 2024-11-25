using System;
using STRINGS;

// Token: 0x020000D1 RID: 209
public class DrowningStates : GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>
{
	// Token: 0x060003D4 RID: 980 RVA: 0x0001F420 File Offset: 0x0001D620
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.drown;
		GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.DROWNING.NAME;
		string tooltip = CREATURES.STATUSITEMS.DROWNING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).TagTransition(GameTags.Creatures.Drowning, null, true);
		this.drown.PlayAnim("drown_pre").QueueAnim("drown_loop", true, null).Transition(this.drown_pst, new StateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.Transition.ConditionCallback(this.UpdateSafeCell), UpdateRate.SIM_1000ms);
		this.drown_pst.PlayAnim("drown_pst").OnAnimQueueComplete(this.move_to_safe);
		this.move_to_safe.MoveTo((DrowningStates.Instance smi) => smi.safeCell, null, null, false);
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0001F50C File Offset: 0x0001D70C
	public bool UpdateSafeCell(DrowningStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		DrowningStates.EscapeCellQuery escapeCellQuery = new DrowningStates.EscapeCellQuery(smi.GetComponent<DrowningMonitor>());
		component.RunQuery(escapeCellQuery);
		smi.safeCell = escapeCellQuery.GetResultCell();
		return smi.safeCell != Grid.InvalidCell;
	}

	// Token: 0x0400029F RID: 671
	public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State drown;

	// Token: 0x040002A0 RID: 672
	public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State drown_pst;

	// Token: 0x040002A1 RID: 673
	public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State move_to_safe;

	// Token: 0x02001038 RID: 4152
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001039 RID: 4153
	public new class Instance : GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.GameInstance
	{
		// Token: 0x06007B7F RID: 31615 RVA: 0x00303B15 File Offset: 0x00301D15
		public Instance(Chore<DrowningStates.Instance> chore, DrowningStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.HasTag, GameTags.Creatures.Drowning);
		}

		// Token: 0x04005C6C RID: 23660
		public int safeCell = Grid.InvalidCell;
	}

	// Token: 0x0200103A RID: 4154
	public class EscapeCellQuery : PathFinderQuery
	{
		// Token: 0x06007B80 RID: 31616 RVA: 0x00303B44 File Offset: 0x00301D44
		public EscapeCellQuery(DrowningMonitor monitor)
		{
			this.monitor = monitor;
		}

		// Token: 0x06007B81 RID: 31617 RVA: 0x00303B53 File Offset: 0x00301D53
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			return this.monitor.IsCellSafe(cell);
		}

		// Token: 0x04005C6D RID: 23661
		private DrowningMonitor monitor;
	}
}
