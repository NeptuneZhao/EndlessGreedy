using System;

// Token: 0x020000CD RID: 205
public class DiggerStates : GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>
{
	// Token: 0x060003BE RID: 958 RVA: 0x0001ECF2 File Offset: 0x0001CEF2
	private static bool ShouldStopHiding(DiggerStates.Instance smi)
	{
		return !GameplayEventManager.Instance.IsGameplayEventRunningWithTag(GameTags.SpaceDanger);
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0001ED08 File Offset: 0x0001CF08
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.move;
		this.move.MoveTo((DiggerStates.Instance smi) => smi.GetTunnelCell(), this.hide, this.behaviourcomplete, false);
		this.hide.Transition(this.behaviourcomplete, new StateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.Transition.ConditionCallback(DiggerStates.ShouldStopHiding), UpdateRate.SIM_4000ms);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Tunnel, false);
	}

	// Token: 0x04000293 RID: 659
	public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State move;

	// Token: 0x04000294 RID: 660
	public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State hide;

	// Token: 0x04000295 RID: 661
	public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State behaviourcomplete;

	// Token: 0x0200102B RID: 4139
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200102C RID: 4140
	public new class Instance : GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.GameInstance
	{
		// Token: 0x06007B63 RID: 31587 RVA: 0x003037D3 File Offset: 0x003019D3
		public Instance(Chore<DiggerStates.Instance> chore, DiggerStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Tunnel);
		}

		// Token: 0x06007B64 RID: 31588 RVA: 0x003037F8 File Offset: 0x003019F8
		public int GetTunnelCell()
		{
			DiggerMonitor.Instance smi = base.smi.GetSMI<DiggerMonitor.Instance>();
			if (smi != null)
			{
				return smi.lastDigCell;
			}
			return -1;
		}
	}
}
