using System;

// Token: 0x0200081E RID: 2078
public class RocketSelfDestructMonitor : GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance>
{
	// Token: 0x0600397A RID: 14714 RVA: 0x00139674 File Offset: 0x00137874
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.RocketSelfDestructRequested, this.exploding, null);
		this.exploding.Update(delegate(RocketSelfDestructMonitor.Instance smi, float dt)
		{
			if (smi.timeinstate >= 3f)
			{
				smi.master.Trigger(-1311384361, null);
				smi.GoTo(this.idle);
			}
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x04002297 RID: 8855
	public GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04002298 RID: 8856
	public GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.State exploding;

	// Token: 0x02001730 RID: 5936
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001731 RID: 5937
	public new class Instance : GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060094EA RID: 38122 RVA: 0x0035E4B6 File Offset: 0x0035C6B6
		public Instance(IStateMachineTarget master, RocketSelfDestructMonitor.Def def) : base(master)
		{
		}

		// Token: 0x04007201 RID: 29185
		public KBatchedAnimController eyes;
	}
}
