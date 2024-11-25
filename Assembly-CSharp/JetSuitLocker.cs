using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020006F5 RID: 1781
public class JetSuitLocker : StateMachineComponent<JetSuitLocker.StatesInstance>, ISecondaryInput
{
	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06002D7D RID: 11645 RVA: 0x000FF580 File Offset: 0x000FD780
	public float FuelAvailable
	{
		get
		{
			GameObject fuel = this.GetFuel();
			float num = 0f;
			if (fuel != null)
			{
				num = fuel.GetComponent<PrimaryElement>().Mass / 100f;
				num = Math.Min(num, 1f);
			}
			return num;
		}
	}

	// Token: 0x06002D7E RID: 11646 RVA: 0x000FF5C4 File Offset: 0x000FD7C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.fuel_tag = SimHashes.Petroleum.CreateTag();
		this.fuel_consumer = base.gameObject.AddComponent<ConduitConsumer>();
		this.fuel_consumer.conduitType = this.portInfo.conduitType;
		this.fuel_consumer.consumptionRate = 10f;
		this.fuel_consumer.capacityTag = this.fuel_tag;
		this.fuel_consumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		this.fuel_consumer.forceAlwaysSatisfied = true;
		this.fuel_consumer.capacityKG = 100f;
		this.fuel_consumer.useSecondaryInput = true;
		RequireInputs requireInputs = base.gameObject.AddComponent<RequireInputs>();
		requireInputs.conduitConsumer = this.fuel_consumer;
		requireInputs.SetRequirements(false, true);
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = this.building.GetRotatedOffset(this.portInfo.offset);
		this.secondaryInputCell = Grid.OffsetCell(cell, rotatedOffset);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.flowNetworkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.secondaryInputCell, base.gameObject);
		networkManager.AddToNetworks(this.secondaryInputCell, this.flowNetworkItem, true);
		this.fuel_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_1", "meter_petrol", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_1"
		});
		this.o2_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_2", "meter_oxygen", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_2"
		});
		base.smi.StartSM();
	}

	// Token: 0x06002D7F RID: 11647 RVA: 0x000FF768 File Offset: 0x000FD968
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryInputCell, this.flowNetworkItem, true);
		base.OnCleanUp();
	}

	// Token: 0x06002D80 RID: 11648 RVA: 0x000FF792 File Offset: 0x000FD992
	private GameObject GetFuel()
	{
		return this.storage.FindFirst(this.fuel_tag);
	}

	// Token: 0x06002D81 RID: 11649 RVA: 0x000FF7A5 File Offset: 0x000FD9A5
	public bool IsSuitFullyCharged()
	{
		return this.suit_locker.IsSuitFullyCharged();
	}

	// Token: 0x06002D82 RID: 11650 RVA: 0x000FF7B2 File Offset: 0x000FD9B2
	public KPrefabID GetStoredOutfit()
	{
		return this.suit_locker.GetStoredOutfit();
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x000FF7C0 File Offset: 0x000FD9C0
	private void FuelSuit(float dt)
	{
		KPrefabID storedOutfit = this.suit_locker.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		GameObject fuel = this.GetFuel();
		if (fuel == null)
		{
			return;
		}
		PrimaryElement component = fuel.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		JetSuitTank component2 = storedOutfit.GetComponent<JetSuitTank>();
		float num = 375f * dt / 600f;
		num = Mathf.Min(num, 25f - component2.amount);
		num = Mathf.Min(component.Mass, num);
		component.Mass -= num;
		component2.amount += num;
	}

	// Token: 0x06002D84 RID: 11652 RVA: 0x000FF85D File Offset: 0x000FDA5D
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002D85 RID: 11653 RVA: 0x000FF86D File Offset: 0x000FDA6D
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.portInfo.conduitType == type)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x06002D86 RID: 11654 RVA: 0x000FF890 File Offset: 0x000FDA90
	public bool HasFuel()
	{
		GameObject fuel = this.GetFuel();
		return fuel != null && fuel.GetComponent<PrimaryElement>().Mass > 0f;
	}

	// Token: 0x06002D87 RID: 11655 RVA: 0x000FF8C4 File Offset: 0x000FDAC4
	private void RefreshMeter()
	{
		this.o2_meter.SetPositionPercent(this.suit_locker.OxygenAvailable);
		this.fuel_meter.SetPositionPercent(this.FuelAvailable);
		this.anim_controller.SetSymbolVisiblity("oxygen_yes_bloom", this.IsOxygenTankAboveMinimumLevel());
		this.anim_controller.SetSymbolVisiblity("petrol_yes_bloom", this.IsFuelTankAboveMinimumLevel());
	}

	// Token: 0x06002D88 RID: 11656 RVA: 0x000FF930 File Offset: 0x000FDB30
	public bool IsOxygenTankAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			SuitTank component = storedOutfit.GetComponent<SuitTank>();
			return component == null || component.PercentFull() >= TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x06002D89 RID: 11657 RVA: 0x000FF974 File Offset: 0x000FDB74
	public bool IsFuelTankAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
			return component == null || component.PercentFull() >= TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x04001A66 RID: 6758
	[MyCmpReq]
	private Building building;

	// Token: 0x04001A67 RID: 6759
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001A68 RID: 6760
	[MyCmpReq]
	private SuitLocker suit_locker;

	// Token: 0x04001A69 RID: 6761
	[MyCmpReq]
	private KBatchedAnimController anim_controller;

	// Token: 0x04001A6A RID: 6762
	public const float FUEL_CAPACITY = 100f;

	// Token: 0x04001A6B RID: 6763
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x04001A6C RID: 6764
	private int secondaryInputCell = -1;

	// Token: 0x04001A6D RID: 6765
	private FlowUtilityNetwork.NetworkItem flowNetworkItem;

	// Token: 0x04001A6E RID: 6766
	private ConduitConsumer fuel_consumer;

	// Token: 0x04001A6F RID: 6767
	private Tag fuel_tag;

	// Token: 0x04001A70 RID: 6768
	private MeterController o2_meter;

	// Token: 0x04001A71 RID: 6769
	private MeterController fuel_meter;

	// Token: 0x0200152A RID: 5418
	public class States : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker>
	{
		// Token: 0x06008D74 RID: 36212 RVA: 0x0033F6B8 File Offset: 0x0033D8B8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Update("RefreshMeter", delegate(JetSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.RefreshMeter();
			}, UpdateRate.RENDER_200ms, false);
			this.empty.EventTransition(GameHashes.OnStorageChange, this.charging, (JetSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null);
			this.charging.DefaultState(this.charging.notoperational).EventTransition(GameHashes.OnStorageChange, this.empty, (JetSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).Transition(this.charged, (JetSuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms);
			this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational, false);
			this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.nofuel, (JetSuitLocker.StatesInstance smi) => !smi.master.HasFuel(), UpdateRate.SIM_200ms).Update("FuelSuit", delegate(JetSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.FuelSuit(dt);
			}, UpdateRate.SIM_1000ms, false);
			this.charging.nofuel.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.operational, (JetSuitLocker.StatesInstance smi) => smi.master.HasFuel(), UpdateRate.SIM_200ms).ToggleStatusItem(BUILDING.STATUSITEMS.SUIT_LOCKER.NO_FUEL.NAME, BUILDING.STATUSITEMS.SUIT_LOCKER.NO_FUEL.TOOLTIP, "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, default(HashedString), 129022, null, null, null);
			this.charged.EventTransition(GameHashes.OnStorageChange, this.empty, (JetSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null);
		}

		// Token: 0x04006C20 RID: 27680
		public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State empty;

		// Token: 0x04006C21 RID: 27681
		public JetSuitLocker.States.ChargingStates charging;

		// Token: 0x04006C22 RID: 27682
		public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State charged;

		// Token: 0x020024F6 RID: 9462
		public class ChargingStates : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State
		{
			// Token: 0x0400A466 RID: 42086
			public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State notoperational;

			// Token: 0x0400A467 RID: 42087
			public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State operational;

			// Token: 0x0400A468 RID: 42088
			public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State nofuel;
		}
	}

	// Token: 0x0200152B RID: 5419
	public class StatesInstance : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.GameInstance
	{
		// Token: 0x06008D76 RID: 36214 RVA: 0x0033F90E File Offset: 0x0033DB0E
		public StatesInstance(JetSuitLocker jet_suit_locker) : base(jet_suit_locker)
		{
		}
	}
}
