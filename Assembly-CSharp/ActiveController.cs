using System;

// Token: 0x0200004E RID: 78
public class ActiveController : GameStateMachine<ActiveController, ActiveController.Instance>
{
	// Token: 0x06000183 RID: 387 RVA: 0x0000A3FC File Offset: 0x000085FC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.ActiveChanged, this.working_pre, (ActiveController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.ActiveChanged, this.working_pst, (ActiveController.Instance smi) => !smi.GetComponent<Operational>().IsActive);
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x040000E7 RID: 231
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040000E8 RID: 232
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x040000E9 RID: 233
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x040000EA RID: 234
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x02000F9B RID: 3995
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F9C RID: 3996
	public new class Instance : GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007A0F RID: 31247 RVA: 0x00301AB4 File Offset: 0x002FFCB4
		public Instance(IStateMachineTarget master, ActiveController.Def def) : base(master, def)
		{
		}
	}
}
