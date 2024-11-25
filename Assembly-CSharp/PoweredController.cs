using System;

// Token: 0x02000055 RID: 85
public class PoweredController : GameStateMachine<PoweredController, PoweredController.Instance>
{
	// Token: 0x06000193 RID: 403 RVA: 0x0000AD1C File Offset: 0x00008F1C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (PoweredController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (PoweredController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
	}

	// Token: 0x04000101 RID: 257
	public GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000102 RID: 258
	public GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02000FB2 RID: 4018
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000FB3 RID: 4019
	public new class Instance : GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007A4C RID: 31308 RVA: 0x00301E7F File Offset: 0x0030007F
		public Instance(IStateMachineTarget master, PoweredController.Def def) : base(master, def)
		{
		}

		// Token: 0x04005B57 RID: 23383
		public bool ShowWorkingStatus;
	}
}
