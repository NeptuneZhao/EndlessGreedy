using System;

// Token: 0x02000050 RID: 80
public class LightController : GameStateMachine<LightController, LightController.Instance>
{
	// Token: 0x06000189 RID: 393 RVA: 0x0000A66C File Offset: 0x0000886C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (LightController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (LightController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).Enter("SetActive", delegate(LightController.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		});
	}

	// Token: 0x040000EE RID: 238
	public GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040000EF RID: 239
	public GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02000FA2 RID: 4002
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000FA3 RID: 4003
	public new class Instance : GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007A20 RID: 31264 RVA: 0x00301BF3 File Offset: 0x002FFDF3
		public Instance(IStateMachineTarget master, LightController.Def def) : base(master, def)
		{
		}
	}
}
