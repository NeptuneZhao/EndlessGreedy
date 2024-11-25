using System;
using STRINGS;

// Token: 0x02000990 RID: 2448
public class MoveToLocationMonitor : GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance>
{
	// Token: 0x0600475F RID: 18271 RVA: 0x0019870C File Offset: 0x0019690C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DoNothing();
		this.moving.ToggleChore((MoveToLocationMonitor.Instance smi) => new MoveChore(smi.master, Db.Get().ChoreTypes.MoveTo, (MoveChore.StatesInstance smii) => smi.targetCell, false), this.satisfied);
	}

	// Token: 0x04002E9B RID: 11931
	public GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002E9C RID: 11932
	public GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, object>.State moving;

	// Token: 0x02001959 RID: 6489
	public new class Instance : GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009C3A RID: 39994 RVA: 0x00371E8C File Offset: 0x0037008C
		public Instance(IStateMachineTarget master) : base(master)
		{
			master.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		}

		// Token: 0x06009C3B RID: 39995 RVA: 0x00371EB0 File Offset: 0x003700B0
		private void OnRefreshUserMenu(object data)
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.MOVETOLOCATION.NAME, new System.Action(this.OnClickMoveToLocation), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MOVETOLOCATION.TOOLTIP, true), 0.2f);
		}

		// Token: 0x06009C3C RID: 39996 RVA: 0x00371F0A File Offset: 0x0037010A
		private void OnClickMoveToLocation()
		{
			MoveToLocationTool.Instance.Activate(base.GetComponent<Navigator>());
		}

		// Token: 0x06009C3D RID: 39997 RVA: 0x00371F1C File Offset: 0x0037011C
		public void MoveToLocation(int cell)
		{
			this.targetCell = cell;
			base.smi.GoTo(base.smi.sm.satisfied);
			base.smi.GoTo(base.smi.sm.moving);
		}

		// Token: 0x06009C3E RID: 39998 RVA: 0x00371F5B File Offset: 0x0037015B
		public override void StopSM(string reason)
		{
			base.master.Unsubscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
			base.StopSM(reason);
		}

		// Token: 0x04007937 RID: 31031
		public int targetCell;
	}
}
