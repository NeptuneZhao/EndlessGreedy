using System;

// Token: 0x02000053 RID: 83
public class PoweredActiveStoppableController : GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance>
{
	// Token: 0x0600018F RID: 399 RVA: 0x0000AA3C File Offset: 0x00008C3C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.ActiveChanged, this.working_pre, (PoweredActiveStoppableController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.stop, (PoweredActiveStoppableController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working_pst, (PoweredActiveStoppableController.Instance smi) => !smi.GetComponent<Operational>().IsActive);
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
		this.stop.PlayAnim("stop").OnAnimQueueComplete(this.off);
	}

	// Token: 0x040000F7 RID: 247
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040000F8 RID: 248
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x040000F9 RID: 249
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x040000FA RID: 250
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x040000FB RID: 251
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State stop;

	// Token: 0x02000FAC RID: 4012
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000FAD RID: 4013
	public new class Instance : GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007A3A RID: 31290 RVA: 0x00301D66 File Offset: 0x002FFF66
		public Instance(IStateMachineTarget master, PoweredActiveStoppableController.Def def) : base(master, def)
		{
		}
	}
}
