using System;

// Token: 0x02000051 RID: 81
public class OperationalController : GameStateMachine<OperationalController, OperationalController.Instance>
{
	// Token: 0x0600018B RID: 395 RVA: 0x0000A748 File Offset: 0x00008948
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.EventTransition(GameHashes.OperationalChanged, this.off, (OperationalController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.working_pre, (OperationalController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.working_pst, (OperationalController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x040000F0 RID: 240
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040000F1 RID: 241
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x040000F2 RID: 242
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x040000F3 RID: 243
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x02000FA5 RID: 4005
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000FA6 RID: 4006
	public new class Instance : GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007A27 RID: 31271 RVA: 0x00301C45 File Offset: 0x002FFE45
		public Instance(IStateMachineTarget master, OperationalController.Def def) : base(master, def)
		{
		}
	}
}
