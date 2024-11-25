using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009B0 RID: 2480
public class YellowAlertManager : GameStateMachine<YellowAlertManager, YellowAlertManager.Instance>
{
	// Token: 0x06004812 RID: 18450 RVA: 0x0019CF40 File Offset: 0x0019B140
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.off.ParamTransition<bool>(this.isOn, this.on, GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.IsTrue);
		this.on.Enter("EnterEvent", delegate(YellowAlertManager.Instance smi)
		{
			Game.Instance.Trigger(-741654735, null);
		}).Exit("ExitEvent", delegate(YellowAlertManager.Instance smi)
		{
			Game.Instance.Trigger(-2062778933, null);
		}).Enter("EnableVignette", delegate(YellowAlertManager.Instance smi)
		{
			Vignette.Instance.SetColor(new Color(1f, 1f, 0f, 0.1f));
		}).Exit("DisableVignette", delegate(YellowAlertManager.Instance smi)
		{
			Vignette.Instance.Reset();
		}).Enter("Sounds", delegate(YellowAlertManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON", false));
		}).ToggleLoopingSound(GlobalAssets.GetSound("RedAlert_LP", false), null, true, true, true).ToggleNotification((YellowAlertManager.Instance smi) => smi.notification).ParamTransition<bool>(this.isOn, this.off, GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.IsFalse);
		this.on_pst.Enter("Sounds", delegate(YellowAlertManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF", false));
		});
	}

	// Token: 0x04002F30 RID: 12080
	public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04002F31 RID: 12081
	public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x04002F32 RID: 12082
	public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State on_pst;

	// Token: 0x04002F33 RID: 12083
	public StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.BoolParameter isOn = new StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x020019AD RID: 6573
	public new class Instance : GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009DBA RID: 40378 RVA: 0x00375F27 File Offset: 0x00374127
		public static void DestroyInstance()
		{
			YellowAlertManager.Instance.instance = null;
		}

		// Token: 0x06009DBB RID: 40379 RVA: 0x00375F2F File Offset: 0x0037412F
		public static YellowAlertManager.Instance Get()
		{
			return YellowAlertManager.Instance.instance;
		}

		// Token: 0x06009DBC RID: 40380 RVA: 0x00375F38 File Offset: 0x00374138
		public Instance(IStateMachineTarget master) : base(master)
		{
			YellowAlertManager.Instance.instance = this;
		}

		// Token: 0x06009DBD RID: 40381 RVA: 0x00375F94 File Offset: 0x00374194
		public bool IsOn()
		{
			return base.sm.isOn.Get(base.smi);
		}

		// Token: 0x06009DBE RID: 40382 RVA: 0x00375FAC File Offset: 0x003741AC
		public void HasTopPriorityChore(bool on)
		{
			this.hasTopPriorityChore = on;
			this.Refresh();
		}

		// Token: 0x06009DBF RID: 40383 RVA: 0x00375FBB File Offset: 0x003741BB
		private void Refresh()
		{
			base.sm.isOn.Set(this.hasTopPriorityChore, base.smi, false);
		}

		// Token: 0x04007A56 RID: 31318
		private static YellowAlertManager.Instance instance;

		// Token: 0x04007A57 RID: 31319
		private bool hasTopPriorityChore;

		// Token: 0x04007A58 RID: 31320
		public Notification notification = new Notification(MISC.NOTIFICATIONS.YELLOWALERT.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.YELLOWALERT.TOOLTIP, null, false, 0f, null, null, null, true, false, false);
	}
}
