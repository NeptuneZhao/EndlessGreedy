using System;

// Token: 0x02000052 RID: 82
public class PoweredActiveController : GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>
{
	// Token: 0x0600018D RID: 397 RVA: 0x0000A854 File Offset: 0x00008A54
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (PoweredActiveController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (PoweredActiveController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working.pre, (PoweredActiveController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.working.Enter(delegate(PoweredActiveController.Instance smi)
		{
			if (smi.def.showWorkingStatus)
			{
				smi.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.Working, null);
			}
		}).Exit(delegate(PoweredActiveController.Instance smi)
		{
			if (smi.def.showWorkingStatus)
			{
				smi.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.Working, false);
			}
		});
		this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
		this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.working.pst, (PoweredActiveController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working.pst, (PoweredActiveController.Instance smi) => !smi.GetComponent<Operational>().IsActive);
		this.working.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on);
	}

	// Token: 0x040000F4 RID: 244
	public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State off;

	// Token: 0x040000F5 RID: 245
	public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State on;

	// Token: 0x040000F6 RID: 246
	public PoweredActiveController.WorkingStates working;

	// Token: 0x02000FA8 RID: 4008
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005B3E RID: 23358
		public bool showWorkingStatus;
	}

	// Token: 0x02000FA9 RID: 4009
	public class WorkingStates : GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State
	{
		// Token: 0x04005B3F RID: 23359
		public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State pre;

		// Token: 0x04005B40 RID: 23360
		public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State loop;

		// Token: 0x04005B41 RID: 23361
		public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State pst;
	}

	// Token: 0x02000FAA RID: 4010
	public new class Instance : GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.GameInstance
	{
		// Token: 0x06007A2F RID: 31279 RVA: 0x00301CA0 File Offset: 0x002FFEA0
		public Instance(IStateMachineTarget master, PoweredActiveController.Def def) : base(master, def)
		{
		}
	}
}
