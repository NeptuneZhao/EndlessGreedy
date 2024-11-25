using System;

// Token: 0x02000998 RID: 2456
public class RedAlertMonitor : GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance>
{
	// Token: 0x0600479C RID: 18332 RVA: 0x00199FA4 File Offset: 0x001981A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.off.EventTransition(GameHashes.EnteredRedAlert, (RedAlertMonitor.Instance smi) => Game.Instance, this.on, delegate(RedAlertMonitor.Instance smi)
		{
			WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
			return !(myWorld == null) && myWorld.AlertManager.IsRedAlert();
		});
		this.on.EventTransition(GameHashes.ExitedRedAlert, (RedAlertMonitor.Instance smi) => Game.Instance, this.off, delegate(RedAlertMonitor.Instance smi)
		{
			WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
			return !(myWorld == null) && !myWorld.AlertManager.IsRedAlert();
		}).Enter("EnableRedAlert", delegate(RedAlertMonitor.Instance smi)
		{
			smi.EnableRedAlert();
		}).ToggleEffect("RedAlert").ToggleExpression(Db.Get().Expressions.RedAlert, null);
	}

	// Token: 0x04002ECC RID: 11980
	public GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04002ECD RID: 11981
	public GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x0200196F RID: 6511
	public new class Instance : GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009C8F RID: 40079 RVA: 0x00372A01 File Offset: 0x00370C01
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009C90 RID: 40080 RVA: 0x00372A0C File Offset: 0x00370C0C
		public void EnableRedAlert()
		{
			ChoreDriver component = base.GetComponent<ChoreDriver>();
			if (component != null)
			{
				Chore currentChore = component.GetCurrentChore();
				if (currentChore != null)
				{
					bool flag = false;
					for (int i = 0; i < currentChore.GetPreconditions().Count; i++)
					{
						if (currentChore.GetPreconditions()[i].condition.id == ChorePreconditions.instance.IsNotRedAlert.id)
						{
							flag = true;
						}
					}
					if (flag)
					{
						component.StopChore();
					}
				}
			}
		}
	}
}
