using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009AE RID: 2478
public class VignetteManager : GameStateMachine<VignetteManager, VignetteManager.Instance>
{
	// Token: 0x0600480A RID: 18442 RVA: 0x0019CA3C File Offset: 0x0019AC3C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.ParamTransition<bool>(this.isOn, this.on, GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.IsTrue);
		this.on.Exit("VignetteOff", delegate(VignetteManager.Instance smi)
		{
			Vignette.Instance.Reset();
		}).ParamTransition<bool>(this.isRedAlert, this.on.red, (VignetteManager.Instance smi, bool p) => this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isRedAlert, this.on.yellow, (VignetteManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isYellowAlert, this.on.yellow, (VignetteManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isOn, this.off, GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.IsFalse);
		this.on.red.Enter("EnterEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(1585324898, null);
		}).Exit("ExitEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(-1393151672, null);
		}).Enter("EnableVignette", delegate(VignetteManager.Instance smi)
		{
			Vignette.Instance.SetColor(new Color(1f, 0f, 0f, 0.3f));
		}).Enter("SoundsOnRedAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF", false));
		}).ToggleLoopingSound(GlobalAssets.GetSound("RedAlert_LP", false), null, true, false, true).ToggleNotification((VignetteManager.Instance smi) => smi.redAlertNotification);
		this.on.yellow.Enter("EnterEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(-741654735, null);
		}).Exit("ExitEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(-2062778933, null);
		}).Enter("EnableVignette", delegate(VignetteManager.Instance smi)
		{
			Vignette.Instance.SetColor(new Color(1f, 1f, 0f, 0.3f));
		}).Enter("SoundsOnYellowAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_OFF", false));
		}).ToggleLoopingSound(GlobalAssets.GetSound("YellowAlert_LP", false), null, true, false, true);
	}

	// Token: 0x04002F29 RID: 12073
	public GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04002F2A RID: 12074
	public VignetteManager.OnStates on;

	// Token: 0x04002F2B RID: 12075
	public StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter isRedAlert = new StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x04002F2C RID: 12076
	public StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter isYellowAlert = new StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x04002F2D RID: 12077
	public StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter isOn = new StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x020019A7 RID: 6567
	public class OnStates : GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007A39 RID: 31289
		public GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State yellow;

		// Token: 0x04007A3A RID: 31290
		public GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State red;
	}

	// Token: 0x020019A8 RID: 6568
	public new class Instance : GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009D93 RID: 40339 RVA: 0x00375816 File Offset: 0x00373A16
		public static void DestroyInstance()
		{
			VignetteManager.Instance.instance = null;
		}

		// Token: 0x06009D94 RID: 40340 RVA: 0x0037581E File Offset: 0x00373A1E
		public static VignetteManager.Instance Get()
		{
			return VignetteManager.Instance.instance;
		}

		// Token: 0x06009D95 RID: 40341 RVA: 0x00375828 File Offset: 0x00373A28
		public Instance(IStateMachineTarget master) : base(master)
		{
			VignetteManager.Instance.instance = this;
		}

		// Token: 0x06009D96 RID: 40342 RVA: 0x00375884 File Offset: 0x00373A84
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

		// Token: 0x06009D97 RID: 40343 RVA: 0x003758F7 File Offset: 0x00373AF7
		public bool IsOn()
		{
			return base.sm.isYellowAlert.Get(base.smi) || base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x06009D98 RID: 40344 RVA: 0x00375929 File Offset: 0x00373B29
		public bool IsRedAlert()
		{
			return base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x06009D99 RID: 40345 RVA: 0x00375941 File Offset: 0x00373B41
		public bool IsYellowAlert()
		{
			return base.sm.isYellowAlert.Get(base.smi);
		}

		// Token: 0x06009D9A RID: 40346 RVA: 0x00375959 File Offset: 0x00373B59
		public bool IsRedAlertToggledOn()
		{
			return this.isToggled;
		}

		// Token: 0x06009D9B RID: 40347 RVA: 0x00375961 File Offset: 0x00373B61
		public void ToggleRedAlert(bool on)
		{
			this.isToggled = on;
			this.Refresh();
		}

		// Token: 0x06009D9C RID: 40348 RVA: 0x00375970 File Offset: 0x00373B70
		public void HasTopPriorityChore(bool on)
		{
			this.hasTopPriorityChore = on;
			this.Refresh();
		}

		// Token: 0x06009D9D RID: 40349 RVA: 0x00375980 File Offset: 0x00373B80
		private void Refresh()
		{
			base.sm.isYellowAlert.Set(this.hasTopPriorityChore, base.smi, false);
			base.sm.isRedAlert.Set(this.isToggled, base.smi, false);
			base.sm.isOn.Set(this.hasTopPriorityChore || this.isToggled, base.smi, false);
		}

		// Token: 0x04007A3B RID: 31291
		private static VignetteManager.Instance instance;

		// Token: 0x04007A3C RID: 31292
		private bool isToggled;

		// Token: 0x04007A3D RID: 31293
		private bool hasTopPriorityChore;

		// Token: 0x04007A3E RID: 31294
		public Notification redAlertNotification = new Notification(MISC.NOTIFICATIONS.REDALERT.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.REDALERT.TOOLTIP, null, false, 0f, null, null, null, true, false, false);
	}
}
