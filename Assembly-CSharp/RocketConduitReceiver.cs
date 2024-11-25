using System;
using UnityEngine;

// Token: 0x0200075E RID: 1886
public class RocketConduitReceiver : StateMachineComponent<RocketConduitReceiver.StatesInstance>, ISecondaryOutput
{
	// Token: 0x060032A2 RID: 12962 RVA: 0x001167D4 File Offset: 0x001149D4
	public void AddConduitPortToNetwork()
	{
		if (this.conduitPort.conduitDispenser == null)
		{
			return;
		}
		int num = Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.conduitPortInfo.offset);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.conduitPortInfo.conduitType);
		this.conduitPort.outputCell = num;
		this.conduitPort.networkItem = new FlowUtilityNetwork.NetworkItem(this.conduitPortInfo.conduitType, Endpoint.Source, num, base.gameObject);
		networkManager.AddToNetworks(num, this.conduitPort.networkItem, true);
	}

	// Token: 0x060032A3 RID: 12963 RVA: 0x00116864 File Offset: 0x00114A64
	public void RemoveConduitPortFromNetwork()
	{
		if (this.conduitPort.conduitDispenser == null)
		{
			return;
		}
		Conduit.GetNetworkManager(this.conduitPortInfo.conduitType).RemoveFromNetworks(this.conduitPort.outputCell, this.conduitPort.networkItem, true);
	}

	// Token: 0x060032A4 RID: 12964 RVA: 0x001168B4 File Offset: 0x00114AB4
	private bool CanTransferFromSender()
	{
		bool result = false;
		if ((base.smi.master.senderConduitStorage.MassStored() > 0f || base.smi.master.senderConduitStorage.items.Count > 0) && base.smi.master.conduitPort.conduitDispenser.GetConduitManager().GetPermittedFlow(base.smi.master.conduitPort.outputCell) != ConduitFlow.FlowDirections.None)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x060032A5 RID: 12965 RVA: 0x00116938 File Offset: 0x00114B38
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FindPartner();
		base.Subscribe<RocketConduitReceiver>(-1118736034, RocketConduitReceiver.TryFindPartner);
		base.Subscribe<RocketConduitReceiver>(546421097, RocketConduitReceiver.OnLaunchedDelegate);
		base.Subscribe<RocketConduitReceiver>(-735346771, RocketConduitReceiver.OnLandedDelegate);
		base.smi.StartSM();
		Components.RocketConduitReceivers.Add(this);
	}

	// Token: 0x060032A6 RID: 12966 RVA: 0x0011699A File Offset: 0x00114B9A
	protected override void OnCleanUp()
	{
		this.RemoveConduitPortFromNetwork();
		base.OnCleanUp();
		Components.RocketConduitReceivers.Remove(this);
	}

	// Token: 0x060032A7 RID: 12967 RVA: 0x001169B4 File Offset: 0x00114BB4
	private void FindPartner()
	{
		if (this.senderConduitStorage != null)
		{
			return;
		}
		RocketConduitSender rocketConduitSender = null;
		WorldContainer world = ClusterManager.Instance.GetWorld(base.gameObject.GetMyWorldId());
		if (world != null && world.IsModuleInterior)
		{
			foreach (RocketConduitSender rocketConduitSender2 in world.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().GetComponents<RocketConduitSender>())
			{
				if (rocketConduitSender2.conduitPortInfo.conduitType == this.conduitPortInfo.conduitType)
				{
					rocketConduitSender = rocketConduitSender2;
					break;
				}
			}
		}
		else
		{
			ClustercraftExteriorDoor component = base.gameObject.GetComponent<ClustercraftExteriorDoor>();
			if (component.HasTargetWorld())
			{
				WorldContainer targetWorld = component.GetTargetWorld();
				foreach (RocketConduitSender rocketConduitSender3 in Components.RocketConduitSenders.GetWorldItems(targetWorld.id, false))
				{
					if (rocketConduitSender3.conduitPortInfo.conduitType == this.conduitPortInfo.conduitType)
					{
						rocketConduitSender = rocketConduitSender3;
						break;
					}
				}
			}
		}
		if (rocketConduitSender == null)
		{
			global::Debug.LogWarning("No warp conduit sender found?");
			return;
		}
		this.SetStorage(rocketConduitSender.conduitStorage);
	}

	// Token: 0x060032A8 RID: 12968 RVA: 0x00116AF0 File Offset: 0x00114CF0
	public void SetStorage(Storage conduitStorage)
	{
		this.senderConduitStorage = conduitStorage;
		this.conduitPort.SetPortInfo(base.gameObject, this.conduitPortInfo, conduitStorage);
		if (base.gameObject.GetMyWorld() != null)
		{
			this.AddConduitPortToNetwork();
		}
	}

	// Token: 0x060032A9 RID: 12969 RVA: 0x00116B2A File Offset: 0x00114D2A
	bool ISecondaryOutput.HasSecondaryConduitType(ConduitType type)
	{
		return type == this.conduitPortInfo.conduitType;
	}

	// Token: 0x060032AA RID: 12970 RVA: 0x00116B3A File Offset: 0x00114D3A
	CellOffset ISecondaryOutput.GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.conduitPortInfo.conduitType)
		{
			return this.conduitPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04001DE6 RID: 7654
	[SerializeField]
	public ConduitPortInfo conduitPortInfo;

	// Token: 0x04001DE7 RID: 7655
	public RocketConduitReceiver.ConduitPort conduitPort;

	// Token: 0x04001DE8 RID: 7656
	public Storage senderConduitStorage;

	// Token: 0x04001DE9 RID: 7657
	private static readonly EventSystem.IntraObjectHandler<RocketConduitReceiver> TryFindPartner = new EventSystem.IntraObjectHandler<RocketConduitReceiver>(delegate(RocketConduitReceiver component, object data)
	{
		component.FindPartner();
	});

	// Token: 0x04001DEA RID: 7658
	private static readonly EventSystem.IntraObjectHandler<RocketConduitReceiver> OnLandedDelegate = new EventSystem.IntraObjectHandler<RocketConduitReceiver>(delegate(RocketConduitReceiver component, object data)
	{
		component.AddConduitPortToNetwork();
	});

	// Token: 0x04001DEB RID: 7659
	private static readonly EventSystem.IntraObjectHandler<RocketConduitReceiver> OnLaunchedDelegate = new EventSystem.IntraObjectHandler<RocketConduitReceiver>(delegate(RocketConduitReceiver component, object data)
	{
		component.RemoveConduitPortFromNetwork();
	});

	// Token: 0x020015E0 RID: 5600
	public struct ConduitPort
	{
		// Token: 0x06009033 RID: 36915 RVA: 0x0034A8E4 File Offset: 0x00348AE4
		public void SetPortInfo(GameObject parent, ConduitPortInfo info, Storage senderStorage)
		{
			this.portInfo = info;
			ConduitDispenser conduitDispenser = parent.AddComponent<ConduitDispenser>();
			conduitDispenser.conduitType = this.portInfo.conduitType;
			conduitDispenser.useSecondaryOutput = true;
			conduitDispenser.alwaysDispense = true;
			conduitDispenser.storage = senderStorage;
			this.conduitDispenser = conduitDispenser;
		}

		// Token: 0x04006E17 RID: 28183
		public ConduitPortInfo portInfo;

		// Token: 0x04006E18 RID: 28184
		public int outputCell;

		// Token: 0x04006E19 RID: 28185
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x04006E1A RID: 28186
		public ConduitDispenser conduitDispenser;
	}

	// Token: 0x020015E1 RID: 5601
	public class StatesInstance : GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.GameInstance
	{
		// Token: 0x06009034 RID: 36916 RVA: 0x0034A92C File Offset: 0x00348B2C
		public StatesInstance(RocketConduitReceiver master) : base(master)
		{
		}
	}

	// Token: 0x020015E2 RID: 5602
	public class States : GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver>
	{
		// Token: 0x06009035 RID: 36917 RVA: 0x0034A938 File Offset: 0x00348B38
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.EventTransition(GameHashes.OperationalFlagChanged, this.on, (RocketConduitReceiver.StatesInstance smi) => smi.GetComponent<Operational>().GetFlag(WarpConduitStatus.warpConnectedFlag));
			this.on.DefaultState(this.on.empty);
			this.on.empty.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).Update(delegate(RocketConduitReceiver.StatesInstance smi, float dt)
			{
				if (smi.master.CanTransferFromSender())
				{
					smi.GoTo(this.on.hasResources);
				}
			}, UpdateRate.SIM_200ms, false);
			this.on.hasResources.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).Update(delegate(RocketConduitReceiver.StatesInstance smi, float dt)
			{
				if (!smi.master.CanTransferFromSender())
				{
					smi.GoTo(this.on.empty);
				}
			}, UpdateRate.SIM_200ms, false);
		}

		// Token: 0x04006E1B RID: 28187
		public GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.State off;

		// Token: 0x04006E1C RID: 28188
		public RocketConduitReceiver.States.onStates on;

		// Token: 0x02002524 RID: 9508
		public class onStates : GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.State
		{
			// Token: 0x0400A570 RID: 42352
			public GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.State hasResources;

			// Token: 0x0400A571 RID: 42353
			public GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.State empty;
		}
	}
}
