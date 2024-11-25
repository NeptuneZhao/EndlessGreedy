using System;
using UnityEngine;

// Token: 0x02000754 RID: 1876
public class RailGunPayloadOpener : StateMachineComponent<RailGunPayloadOpener.StatesInstance>, ISecondaryOutput
{
	// Token: 0x06003221 RID: 12833 RVA: 0x001138B8 File Offset: 0x00111AB8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.gasOutputCell = Grid.OffsetCell(Grid.PosToCell(this), this.gasPortInfo.offset);
		this.gasDispenser = this.CreateConduitDispenser(ConduitType.Gas, this.gasOutputCell, out this.gasNetworkItem);
		this.liquidOutputCell = Grid.OffsetCell(Grid.PosToCell(this), this.liquidPortInfo.offset);
		this.liquidDispenser = this.CreateConduitDispenser(ConduitType.Liquid, this.liquidOutputCell, out this.liquidNetworkItem);
		this.solidOutputCell = Grid.OffsetCell(Grid.PosToCell(this), this.solidPortInfo.offset);
		this.solidDispenser = this.CreateSolidConduitDispenser(this.solidOutputCell, out this.solidNetworkItem);
		this.deliveryComponents = base.GetComponents<ManualDeliveryKG>();
		this.payloadStorage.gunTargetOffset = new Vector2(-1f, 1.5f);
		this.payloadMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_storage_target", "meter_storage", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.smi.StartSM();
	}

	// Token: 0x06003222 RID: 12834 RVA: 0x001139C0 File Offset: 0x00111BC0
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidOutputCell, this.liquidNetworkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasOutputCell, this.gasNetworkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidOutputCell, this.solidDispenser, true);
		base.OnCleanUp();
	}

	// Token: 0x06003223 RID: 12835 RVA: 0x00113A34 File Offset: 0x00111C34
	private ConduitDispenser CreateConduitDispenser(ConduitType outputType, int outputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		ConduitDispenser conduitDispenser = base.gameObject.AddComponent<ConduitDispenser>();
		conduitDispenser.conduitType = outputType;
		conduitDispenser.useSecondaryOutput = true;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.storage = this.resourceStorage;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(outputType);
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(outputType, Endpoint.Source, outputCell, base.gameObject);
		networkManager.AddToNetworks(outputCell, flowNetworkItem, true);
		return conduitDispenser;
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x00113A8C File Offset: 0x00111C8C
	private SolidConduitDispenser CreateSolidConduitDispenser(int outputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		SolidConduitDispenser solidConduitDispenser = base.gameObject.AddComponent<SolidConduitDispenser>();
		solidConduitDispenser.storage = this.resourceStorage;
		solidConduitDispenser.alwaysDispense = true;
		solidConduitDispenser.useSecondaryOutput = true;
		solidConduitDispenser.solidOnly = true;
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Source, outputCell, base.gameObject);
		Game.Instance.solidConduitSystem.AddToNetworks(outputCell, flowNetworkItem, true);
		return solidConduitDispenser;
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x00113AE8 File Offset: 0x00111CE8
	public void EmptyPayload()
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null && component.items.Count > 0)
		{
			GameObject gameObject = this.payloadStorage.items[0];
			gameObject.GetComponent<Storage>().Transfer(this.resourceStorage, false, false);
			Util.KDestroyGameObject(gameObject);
			component.ConsumeIgnoringDisease(this.payloadStorage.items[0]);
		}
	}

	// Token: 0x06003226 RID: 12838 RVA: 0x00113B54 File Offset: 0x00111D54
	public bool PowerOperationalChanged()
	{
		EnergyConsumer component = base.GetComponent<EnergyConsumer>();
		return component != null && component.IsPowered;
	}

	// Token: 0x06003227 RID: 12839 RVA: 0x00113B79 File Offset: 0x00111D79
	bool ISecondaryOutput.HasSecondaryConduitType(ConduitType type)
	{
		return type == this.gasPortInfo.conduitType || type == this.liquidPortInfo.conduitType || type == this.solidPortInfo.conduitType;
	}

	// Token: 0x06003228 RID: 12840 RVA: 0x00113BA8 File Offset: 0x00111DA8
	CellOffset ISecondaryOutput.GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.gasPortInfo.conduitType)
		{
			return this.gasPortInfo.offset;
		}
		if (type == this.liquidPortInfo.conduitType)
		{
			return this.liquidPortInfo.offset;
		}
		if (type != this.solidPortInfo.conduitType)
		{
			return CellOffset.none;
		}
		return this.solidPortInfo.offset;
	}

	// Token: 0x04001D9F RID: 7583
	public static float delivery_time = 10f;

	// Token: 0x04001DA0 RID: 7584
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x04001DA1 RID: 7585
	private int liquidOutputCell = -1;

	// Token: 0x04001DA2 RID: 7586
	private FlowUtilityNetwork.NetworkItem liquidNetworkItem;

	// Token: 0x04001DA3 RID: 7587
	private ConduitDispenser liquidDispenser;

	// Token: 0x04001DA4 RID: 7588
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x04001DA5 RID: 7589
	private int gasOutputCell = -1;

	// Token: 0x04001DA6 RID: 7590
	private FlowUtilityNetwork.NetworkItem gasNetworkItem;

	// Token: 0x04001DA7 RID: 7591
	private ConduitDispenser gasDispenser;

	// Token: 0x04001DA8 RID: 7592
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x04001DA9 RID: 7593
	private int solidOutputCell = -1;

	// Token: 0x04001DAA RID: 7594
	private FlowUtilityNetwork.NetworkItem solidNetworkItem;

	// Token: 0x04001DAB RID: 7595
	private SolidConduitDispenser solidDispenser;

	// Token: 0x04001DAC RID: 7596
	public Storage payloadStorage;

	// Token: 0x04001DAD RID: 7597
	public Storage resourceStorage;

	// Token: 0x04001DAE RID: 7598
	private ManualDeliveryKG[] deliveryComponents;

	// Token: 0x04001DAF RID: 7599
	private MeterController payloadMeter;

	// Token: 0x020015CC RID: 5580
	public class StatesInstance : GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.GameInstance
	{
		// Token: 0x06008FD8 RID: 36824 RVA: 0x003492F6 File Offset: 0x003474F6
		public StatesInstance(RailGunPayloadOpener master) : base(master)
		{
		}

		// Token: 0x06008FD9 RID: 36825 RVA: 0x003492FF File Offset: 0x003474FF
		public bool HasPayload()
		{
			return base.smi.master.payloadStorage.items.Count > 0;
		}

		// Token: 0x06008FDA RID: 36826 RVA: 0x0034931E File Offset: 0x0034751E
		public bool HasResources()
		{
			return base.smi.master.resourceStorage.MassStored() > 0f;
		}
	}

	// Token: 0x020015CD RID: 5581
	public class States : GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener>
	{
		// Token: 0x06008FDB RID: 36827 RVA: 0x0034933C File Offset: 0x0034753C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.unoperational.PlayAnim("off").EventTransition(GameHashes.OperationalFlagChanged, this.operational, (RailGunPayloadOpener.StatesInstance smi) => smi.master.PowerOperationalChanged()).Enter(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, true);
				smi.GetComponent<ManualDeliveryKG>().Pause(true, "no_power");
			});
			this.operational.Enter(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.GetComponent<ManualDeliveryKG>().Pause(false, "power");
			}).EventTransition(GameHashes.OperationalFlagChanged, this.unoperational, (RailGunPayloadOpener.StatesInstance smi) => !smi.master.PowerOperationalChanged()).DefaultState(this.operational.idle).EventHandler(GameHashes.OnStorageChange, delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.master.payloadMeter.SetPositionPercent(Mathf.Clamp01((float)smi.master.payloadStorage.items.Count / smi.master.payloadStorage.capacityKg));
			});
			this.operational.idle.PlayAnim("on").EventTransition(GameHashes.OnStorageChange, this.operational.pre, (RailGunPayloadOpener.StatesInstance smi) => smi.HasPayload());
			this.operational.pre.Enter(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, true);
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.operational.loop);
			this.operational.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(10f, this.operational.pst);
			this.operational.pst.PlayAnim("working_pst").Exit(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.master.EmptyPayload();
				smi.GetComponent<Operational>().SetActive(false, true);
			}).OnAnimQueueComplete(this.operational.idle);
		}

		// Token: 0x04006DCD RID: 28109
		public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State unoperational;

		// Token: 0x04006DCE RID: 28110
		public RailGunPayloadOpener.States.OperationalStates operational;

		// Token: 0x0200251E RID: 9502
		public class OperationalStates : GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State
		{
			// Token: 0x0400A54C RID: 42316
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State idle;

			// Token: 0x0400A54D RID: 42317
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State pre;

			// Token: 0x0400A54E RID: 42318
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State loop;

			// Token: 0x0400A54F RID: 42319
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State pst;
		}
	}
}
