using System;

// Token: 0x020009B1 RID: 2481
public class YellowAlertMonitor : GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance>
{
	// Token: 0x06004814 RID: 18452 RVA: 0x0019D0E0 File Offset: 0x0019B2E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.off.EventTransition(GameHashes.EnteredYellowAlert, (YellowAlertMonitor.Instance smi) => Game.Instance, this.on, (YellowAlertMonitor.Instance smi) => YellowAlertManager.Instance.Get().IsOn());
		this.on.EventTransition(GameHashes.ExitedYellowAlert, (YellowAlertMonitor.Instance smi) => Game.Instance, this.off, (YellowAlertMonitor.Instance smi) => !YellowAlertManager.Instance.Get().IsOn()).Enter("EnableYellowAlert", delegate(YellowAlertMonitor.Instance smi)
		{
			smi.EnableYellowAlert();
		});
	}

	// Token: 0x04002F34 RID: 12084
	public GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04002F35 RID: 12085
	public GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x020019AF RID: 6575
	public new class Instance : GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009DC9 RID: 40393 RVA: 0x00376070 File Offset: 0x00374270
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009DCA RID: 40394 RVA: 0x00376079 File Offset: 0x00374279
		public void EnableYellowAlert()
		{
		}
	}
}
