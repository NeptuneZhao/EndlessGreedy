using System;
using UnityEngine;

// Token: 0x0200075F RID: 1887
public class RocketConduitSender : StateMachineComponent<RocketConduitSender.StatesInstance>, ISecondaryInput
{
	// Token: 0x060032AD RID: 12973 RVA: 0x00116BC0 File Offset: 0x00114DC0
	public void AddConduitPortToNetwork()
	{
		if (this.conduitPort == null)
		{
			return;
		}
		int num = Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.conduitPortInfo.offset);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.conduitPortInfo.conduitType);
		this.conduitPort.inputCell = num;
		this.conduitPort.networkItem = new FlowUtilityNetwork.NetworkItem(this.conduitPortInfo.conduitType, Endpoint.Sink, num, base.gameObject);
		networkManager.AddToNetworks(num, this.conduitPort.networkItem, true);
	}

	// Token: 0x060032AE RID: 12974 RVA: 0x00116C43 File Offset: 0x00114E43
	public void RemoveConduitPortFromNetwork()
	{
		if (this.conduitPort == null)
		{
			return;
		}
		Conduit.GetNetworkManager(this.conduitPortInfo.conduitType).RemoveFromNetworks(this.conduitPort.inputCell, this.conduitPort.networkItem, true);
	}

	// Token: 0x060032AF RID: 12975 RVA: 0x00116C7C File Offset: 0x00114E7C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FindPartner();
		base.Subscribe<RocketConduitSender>(-1118736034, RocketConduitSender.TryFindPartnerDelegate);
		base.Subscribe<RocketConduitSender>(546421097, RocketConduitSender.OnLaunchedDelegate);
		base.Subscribe<RocketConduitSender>(-735346771, RocketConduitSender.OnLandedDelegate);
		base.smi.StartSM();
		Components.RocketConduitSenders.Add(this);
	}

	// Token: 0x060032B0 RID: 12976 RVA: 0x00116CDE File Offset: 0x00114EDE
	protected override void OnCleanUp()
	{
		this.RemoveConduitPortFromNetwork();
		base.OnCleanUp();
		Components.RocketConduitSenders.Remove(this);
	}

	// Token: 0x060032B1 RID: 12977 RVA: 0x00116CF8 File Offset: 0x00114EF8
	private void FindPartner()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(base.gameObject.GetMyWorldId());
		if (world != null && world.IsModuleInterior)
		{
			foreach (RocketConduitReceiver rocketConduitReceiver in world.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().GetComponents<RocketConduitReceiver>())
			{
				if (rocketConduitReceiver.conduitPortInfo.conduitType == this.conduitPortInfo.conduitType)
				{
					this.partnerReceiver = rocketConduitReceiver;
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
				foreach (RocketConduitReceiver rocketConduitReceiver2 in Components.RocketConduitReceivers.GetWorldItems(targetWorld.id, false))
				{
					if (rocketConduitReceiver2.conduitPortInfo.conduitType == this.conduitPortInfo.conduitType)
					{
						this.partnerReceiver = rocketConduitReceiver2;
						break;
					}
				}
			}
		}
		if (this.partnerReceiver == null)
		{
			global::Debug.LogWarning("No rocket conduit receiver found?");
			return;
		}
		this.conduitPort = new RocketConduitSender.ConduitPort(base.gameObject, this.conduitPortInfo, this.conduitStorage);
		if (world != null)
		{
			this.AddConduitPortToNetwork();
		}
		this.partnerReceiver.SetStorage(this.conduitStorage);
	}

	// Token: 0x060032B2 RID: 12978 RVA: 0x00116E60 File Offset: 0x00115060
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.conduitPortInfo.conduitType == type;
	}

	// Token: 0x060032B3 RID: 12979 RVA: 0x00116E70 File Offset: 0x00115070
	CellOffset ISecondaryInput.GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.conduitPortInfo.conduitType == type)
		{
			return this.conduitPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04001DEC RID: 7660
	public Storage conduitStorage;

	// Token: 0x04001DED RID: 7661
	[SerializeField]
	public ConduitPortInfo conduitPortInfo;

	// Token: 0x04001DEE RID: 7662
	private RocketConduitSender.ConduitPort conduitPort;

	// Token: 0x04001DEF RID: 7663
	private RocketConduitReceiver partnerReceiver;

	// Token: 0x04001DF0 RID: 7664
	private static readonly EventSystem.IntraObjectHandler<RocketConduitSender> TryFindPartnerDelegate = new EventSystem.IntraObjectHandler<RocketConduitSender>(delegate(RocketConduitSender component, object data)
	{
		component.FindPartner();
	});

	// Token: 0x04001DF1 RID: 7665
	private static readonly EventSystem.IntraObjectHandler<RocketConduitSender> OnLandedDelegate = new EventSystem.IntraObjectHandler<RocketConduitSender>(delegate(RocketConduitSender component, object data)
	{
		component.AddConduitPortToNetwork();
	});

	// Token: 0x04001DF2 RID: 7666
	private static readonly EventSystem.IntraObjectHandler<RocketConduitSender> OnLaunchedDelegate = new EventSystem.IntraObjectHandler<RocketConduitSender>(delegate(RocketConduitSender component, object data)
	{
		component.RemoveConduitPortFromNetwork();
	});

	// Token: 0x020015E4 RID: 5604
	private class ConduitPort
	{
		// Token: 0x0600903E RID: 36926 RVA: 0x0034AA80 File Offset: 0x00348C80
		public ConduitPort(GameObject parent, ConduitPortInfo info, Storage targetStorage)
		{
			this.conduitPortInfo = info;
			ConduitConsumer conduitConsumer = parent.AddComponent<ConduitConsumer>();
			conduitConsumer.conduitType = this.conduitPortInfo.conduitType;
			conduitConsumer.useSecondaryInput = true;
			conduitConsumer.storage = targetStorage;
			conduitConsumer.capacityKG = targetStorage.capacityKg;
			conduitConsumer.alwaysConsume = true;
			conduitConsumer.forceAlwaysSatisfied = true;
			this.conduitConsumer = conduitConsumer;
			this.conduitConsumer.keepZeroMassObject = false;
		}

		// Token: 0x04006E1E RID: 28190
		public ConduitPortInfo conduitPortInfo;

		// Token: 0x04006E1F RID: 28191
		public int inputCell;

		// Token: 0x04006E20 RID: 28192
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x04006E21 RID: 28193
		private ConduitConsumer conduitConsumer;
	}

	// Token: 0x020015E5 RID: 5605
	public class StatesInstance : GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.GameInstance
	{
		// Token: 0x0600903F RID: 36927 RVA: 0x0034AAED File Offset: 0x00348CED
		public StatesInstance(RocketConduitSender smi) : base(smi)
		{
		}
	}

	// Token: 0x020015E6 RID: 5606
	public class States : GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender>
	{
		// Token: 0x06009040 RID: 36928 RVA: 0x0034AAF8 File Offset: 0x00348CF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.on;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.on.DefaultState(this.on.waiting);
			this.on.waiting.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).EventTransition(GameHashes.OnStorageChange, this.on.working, (RocketConduitSender.StatesInstance smi) => smi.GetComponent<Storage>().MassStored() > 0f);
			this.on.working.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).DefaultState(this.on.working.ground);
			this.on.working.notOnGround.Enter(delegate(RocketConduitSender.StatesInstance smi)
			{
				smi.gameObject.GetSMI<AutoStorageDropper.Instance>().SetInvertElementFilter(true);
			}).UpdateTransition(this.on.working.ground, delegate(RocketConduitSender.StatesInstance smi, float f)
			{
				WorldContainer myWorld = smi.master.GetMyWorld();
				return myWorld && myWorld.IsModuleInterior && !myWorld.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().HasTag(GameTags.RocketNotOnGround);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(RocketConduitSender.StatesInstance smi)
			{
				if (smi.gameObject != null)
				{
					AutoStorageDropper.Instance smi2 = smi.gameObject.GetSMI<AutoStorageDropper.Instance>();
					if (smi2 != null)
					{
						smi2.SetInvertElementFilter(false);
					}
				}
			});
			this.on.working.ground.Enter(delegate(RocketConduitSender.StatesInstance smi)
			{
				if (smi.master.partnerReceiver != null)
				{
					smi.master.partnerReceiver.conduitPort.conduitDispenser.alwaysDispense = true;
				}
			}).UpdateTransition(this.on.working.notOnGround, delegate(RocketConduitSender.StatesInstance smi, float f)
			{
				WorldContainer myWorld = smi.master.GetMyWorld();
				return myWorld && myWorld.IsModuleInterior && myWorld.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().HasTag(GameTags.RocketNotOnGround);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(RocketConduitSender.StatesInstance smi)
			{
				if (smi.master.partnerReceiver != null)
				{
					smi.master.partnerReceiver.conduitPort.conduitDispenser.alwaysDispense = false;
				}
			});
		}

		// Token: 0x04006E22 RID: 28194
		public RocketConduitSender.States.onStates on;

		// Token: 0x02002526 RID: 9510
		public class onStates : GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State
		{
			// Token: 0x0400A574 RID: 42356
			public RocketConduitSender.States.workingStates working;

			// Token: 0x0400A575 RID: 42357
			public GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State waiting;
		}

		// Token: 0x02002527 RID: 9511
		public class workingStates : GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State
		{
			// Token: 0x0400A576 RID: 42358
			public GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State notOnGround;

			// Token: 0x0400A577 RID: 42359
			public GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State ground;
		}
	}
}
