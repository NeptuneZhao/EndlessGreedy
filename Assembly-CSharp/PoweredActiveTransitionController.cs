using System;

// Token: 0x02000054 RID: 84
public class PoweredActiveTransitionController : GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>
{
	// Token: 0x06000191 RID: 401 RVA: 0x0000AB60 File Offset: 0x00008D60
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on_pre, (PoweredActiveTransitionController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.on_pst, (PoweredActiveTransitionController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working, (PoweredActiveTransitionController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.on_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
		this.working.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on_pst, (PoweredActiveTransitionController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.on, (PoweredActiveTransitionController.Instance smi) => !smi.GetComponent<Operational>().IsActive).Enter(delegate(PoweredActiveTransitionController.Instance smi)
		{
			if (smi.def.showWorkingStatus)
			{
				smi.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.Working, null);
			}
		}).Exit(delegate(PoweredActiveTransitionController.Instance smi)
		{
			if (smi.def.showWorkingStatus)
			{
				smi.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.Working, false);
			}
		});
	}

	// Token: 0x040000FC RID: 252
	public GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>.State off;

	// Token: 0x040000FD RID: 253
	public GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>.State on;

	// Token: 0x040000FE RID: 254
	public GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>.State on_pre;

	// Token: 0x040000FF RID: 255
	public GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>.State on_pst;

	// Token: 0x04000100 RID: 256
	public GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>.State working;

	// Token: 0x02000FAF RID: 4015
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005B4E RID: 23374
		public bool showWorkingStatus;
	}

	// Token: 0x02000FB0 RID: 4016
	public new class Instance : GameStateMachine<PoweredActiveTransitionController, PoweredActiveTransitionController.Instance, IStateMachineTarget, PoweredActiveTransitionController.Def>.GameInstance
	{
		// Token: 0x06007A41 RID: 31297 RVA: 0x00301DB9 File Offset: 0x002FFFB9
		public Instance(IStateMachineTarget master, PoweredActiveTransitionController.Def def) : base(master, def)
		{
		}
	}
}
