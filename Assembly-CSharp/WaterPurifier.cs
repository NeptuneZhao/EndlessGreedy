using System;
using KSerialization;

// Token: 0x0200079A RID: 1946
[SerializationConfig(MemberSerialization.OptIn)]
public class WaterPurifier : StateMachineComponent<WaterPurifier.StatesInstance>
{
	// Token: 0x06003547 RID: 13639 RVA: 0x001222F0 File Offset: 0x001204F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.deliveryComponents = base.GetComponents<ManualDeliveryKG>();
		this.OnConduitConnectionChanged(base.GetComponent<ConduitConsumer>().IsConnected);
		base.Subscribe<WaterPurifier>(-2094018600, WaterPurifier.OnConduitConnectionChangedDelegate);
		base.smi.StartSM();
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x00122344 File Offset: 0x00120544
	private void OnConduitConnectionChanged(object data)
	{
		bool pause = (bool)data;
		foreach (ManualDeliveryKG manualDeliveryKG in this.deliveryComponents)
		{
			Element element = ElementLoader.GetElement(manualDeliveryKG.RequestedItemTag);
			if (element != null && element.IsLiquid)
			{
				manualDeliveryKG.Pause(pause, "pipe connected");
			}
		}
	}

	// Token: 0x04001FA8 RID: 8104
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001FA9 RID: 8105
	private ManualDeliveryKG[] deliveryComponents;

	// Token: 0x04001FAA RID: 8106
	private static readonly EventSystem.IntraObjectHandler<WaterPurifier> OnConduitConnectionChangedDelegate = new EventSystem.IntraObjectHandler<WaterPurifier>(delegate(WaterPurifier component, object data)
	{
		component.OnConduitConnectionChanged(data);
	});

	// Token: 0x02001654 RID: 5716
	public class StatesInstance : GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.GameInstance
	{
		// Token: 0x060091E0 RID: 37344 RVA: 0x003524A0 File Offset: 0x003506A0
		public StatesInstance(WaterPurifier smi) : base(smi)
		{
		}
	}

	// Token: 0x02001655 RID: 5717
	public class States : GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier>
	{
		// Token: 0x060091E1 RID: 37345 RVA: 0x003524AC File Offset: 0x003506AC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (WaterPurifier.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (WaterPurifier.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (WaterPurifier.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.on.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.Enter(delegate(WaterPurifier.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.on.working_pst, (WaterPurifier.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll()).Exit(delegate(WaterPurifier.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.on.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.waiting);
		}

		// Token: 0x04006F60 RID: 28512
		public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State off;

		// Token: 0x04006F61 RID: 28513
		public WaterPurifier.States.OnStates on;

		// Token: 0x0200255B RID: 9563
		public class OnStates : GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State
		{
			// Token: 0x0400A677 RID: 42615
			public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State waiting;

			// Token: 0x0400A678 RID: 42616
			public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State working_pre;

			// Token: 0x0400A679 RID: 42617
			public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State working;

			// Token: 0x0400A67A RID: 42618
			public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State working_pst;
		}
	}
}
