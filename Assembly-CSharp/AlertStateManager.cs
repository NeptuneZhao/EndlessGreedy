using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000966 RID: 2406
public class AlertStateManager : GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>
{
	// Token: 0x0600466A RID: 18026 RVA: 0x00192810 File Offset: 0x00190A10
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.off.ParamTransition<bool>(this.isOn, this.on, GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.IsTrue);
		this.on.Exit("VignetteOff", delegate(AlertStateManager.Instance smi)
		{
			Vignette.Instance.Reset();
		}).ParamTransition<bool>(this.isRedAlert, this.on.red, (AlertStateManager.Instance smi, bool p) => this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isRedAlert, this.on.yellow, (AlertStateManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isYellowAlert, this.on.yellow, (AlertStateManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isOn, this.off, GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.IsFalse);
		this.on.red.Enter("EnterEvent", delegate(AlertStateManager.Instance smi)
		{
			Game.Instance.Trigger(1585324898, null);
		}).Exit("ExitEvent", delegate(AlertStateManager.Instance smi)
		{
			Game.Instance.Trigger(-1393151672, null);
		}).Enter("SoundsOnRedAlert", delegate(AlertStateManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(AlertStateManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF", false));
		}).ToggleNotification((AlertStateManager.Instance smi) => smi.redAlertNotification);
		this.on.yellow.Enter("EnterEvent", delegate(AlertStateManager.Instance smi)
		{
			Game.Instance.Trigger(-741654735, null);
		}).Exit("ExitEvent", delegate(AlertStateManager.Instance smi)
		{
			Game.Instance.Trigger(-2062778933, null);
		}).Enter("SoundsOnYellowAlert", delegate(AlertStateManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(AlertStateManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_OFF", false));
		});
	}

	// Token: 0x04002DD2 RID: 11730
	public GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.State off;

	// Token: 0x04002DD3 RID: 11731
	public AlertStateManager.OnStates on;

	// Token: 0x04002DD4 RID: 11732
	public StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter isRedAlert = new StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter();

	// Token: 0x04002DD5 RID: 11733
	public StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter isYellowAlert = new StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter();

	// Token: 0x04002DD6 RID: 11734
	public StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter isOn = new StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter();

	// Token: 0x020018D2 RID: 6354
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018D3 RID: 6355
	public class OnStates : GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.State
	{
		// Token: 0x04007780 RID: 30592
		public GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.State yellow;

		// Token: 0x04007781 RID: 30593
		public GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.State red;
	}

	// Token: 0x020018D4 RID: 6356
	public new class Instance : GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.GameInstance
	{
		// Token: 0x060099E9 RID: 39401 RVA: 0x0036BCC0 File Offset: 0x00369EC0
		public Instance(IStateMachineTarget master, AlertStateManager.Def def) : base(master, def)
		{
		}

		// Token: 0x060099EA RID: 39402 RVA: 0x0036BD18 File Offset: 0x00369F18
		public void UpdateState(float dt)
		{
			if (this.IsRedAlert())
			{
				base.smi.GoTo(base.sm.on.red);
				return;
			}
			if (this.IsYellowAlert())
			{
				base.smi.GoTo(base.sm.on.yellow);
				return;
			}
			if (!this.IsOn())
			{
				base.smi.GoTo(base.sm.off);
			}
		}

		// Token: 0x060099EB RID: 39403 RVA: 0x0036BD8B File Offset: 0x00369F8B
		public bool IsOn()
		{
			return base.sm.isYellowAlert.Get(base.smi) || base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x060099EC RID: 39404 RVA: 0x0036BDBD File Offset: 0x00369FBD
		public bool IsRedAlert()
		{
			return base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x060099ED RID: 39405 RVA: 0x0036BDD5 File Offset: 0x00369FD5
		public bool IsYellowAlert()
		{
			return base.sm.isYellowAlert.Get(base.smi);
		}

		// Token: 0x060099EE RID: 39406 RVA: 0x0036BDED File Offset: 0x00369FED
		public bool IsRedAlertToggledOn()
		{
			return this.isToggled;
		}

		// Token: 0x060099EF RID: 39407 RVA: 0x0036BDF5 File Offset: 0x00369FF5
		public void ToggleRedAlert(bool on)
		{
			this.isToggled = on;
			this.Refresh();
		}

		// Token: 0x060099F0 RID: 39408 RVA: 0x0036BE04 File Offset: 0x0036A004
		public void SetHasTopPriorityChore(bool on)
		{
			this.hasTopPriorityChore = on;
			this.Refresh();
		}

		// Token: 0x060099F1 RID: 39409 RVA: 0x0036BE14 File Offset: 0x0036A014
		private void Refresh()
		{
			base.sm.isYellowAlert.Set(this.hasTopPriorityChore, base.smi, false);
			base.sm.isRedAlert.Set(this.isToggled, base.smi, false);
			base.sm.isOn.Set(this.hasTopPriorityChore || this.isToggled, base.smi, false);
		}

		// Token: 0x04007782 RID: 30594
		private bool isToggled;

		// Token: 0x04007783 RID: 30595
		private bool hasTopPriorityChore;

		// Token: 0x04007784 RID: 30596
		public Notification redAlertNotification = new Notification(MISC.NOTIFICATIONS.REDALERT.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.REDALERT.TOOLTIP, null, false, 0f, null, null, null, true, false, false);
	}
}
