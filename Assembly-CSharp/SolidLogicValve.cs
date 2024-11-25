using System;
using KSerialization;

// Token: 0x0200076D RID: 1901
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidLogicValve : StateMachineComponent<SolidLogicValve.StatesInstance>
{
	// Token: 0x06003326 RID: 13094 RVA: 0x00118B70 File Offset: 0x00116D70
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003327 RID: 13095 RVA: 0x00118B78 File Offset: 0x00116D78
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06003328 RID: 13096 RVA: 0x00118B8B File Offset: 0x00116D8B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x04001E34 RID: 7732
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001E35 RID: 7733
	[MyCmpReq]
	private SolidConduitBridge bridge;

	// Token: 0x020015FE RID: 5630
	public class States : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve>
	{
		// Token: 0x06009095 RID: 37013 RVA: 0x0034C050 File Offset: 0x0034A250
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidLogicValve.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidLogicValve.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidLogicValve.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidLogicValve.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			});
			this.on.idle.PlayAnim("on").Transition(this.on.working, (SolidLogicValve.StatesInstance smi) => smi.IsDispensing(), UpdateRate.SIM_200ms);
			this.on.working.PlayAnim("on_flow", KAnim.PlayMode.Loop).Transition(this.on.idle, (SolidLogicValve.StatesInstance smi) => !smi.IsDispensing(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04006E55 RID: 28245
		public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State off;

		// Token: 0x04006E56 RID: 28246
		public SolidLogicValve.States.ReadyStates on;

		// Token: 0x02002534 RID: 9524
		public class ReadyStates : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State
		{
			// Token: 0x0400A5AE RID: 42414
			public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State idle;

			// Token: 0x0400A5AF RID: 42415
			public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State working;
		}
	}

	// Token: 0x020015FF RID: 5631
	public class StatesInstance : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.GameInstance
	{
		// Token: 0x06009097 RID: 37015 RVA: 0x0034C1D4 File Offset: 0x0034A3D4
		public StatesInstance(SolidLogicValve master) : base(master)
		{
		}

		// Token: 0x06009098 RID: 37016 RVA: 0x0034C1DD File Offset: 0x0034A3DD
		public bool IsDispensing()
		{
			return base.master.bridge.IsDispensing;
		}
	}
}
