using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000752 RID: 1874
public class RailGun : StateMachineComponent<RailGun.StatesInstance>, ISim200ms, ISecondaryInput
{
	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06003206 RID: 12806 RVA: 0x00112CAB File Offset: 0x00110EAB
	public float MaxLaunchMass
	{
		get
		{
			return 200f;
		}
	}

	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06003207 RID: 12807 RVA: 0x00112CB2 File Offset: 0x00110EB2
	public float EnergyCost
	{
		get
		{
			return base.smi.EnergyCost();
		}
	}

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06003208 RID: 12808 RVA: 0x00112CBF File Offset: 0x00110EBF
	public float CurrentEnergy
	{
		get
		{
			return this.hepStorage.Particles;
		}
	}

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06003209 RID: 12809 RVA: 0x00112CCC File Offset: 0x00110ECC
	public bool AllowLaunchingFromLogic
	{
		get
		{
			return !this.hasLogicWire || (this.hasLogicWire && this.isLogicActive);
		}
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x0600320A RID: 12810 RVA: 0x00112CE8 File Offset: 0x00110EE8
	public bool HasLogicWire
	{
		get
		{
			return this.hasLogicWire;
		}
	}

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x0600320B RID: 12811 RVA: 0x00112CF0 File Offset: 0x00110EF0
	public bool IsLogicActive
	{
		get
		{
			return this.isLogicActive;
		}
	}

	// Token: 0x0600320C RID: 12812 RVA: 0x00112CF8 File Offset: 0x00110EF8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.destinationSelector = base.GetComponent<ClusterDestinationSelector>();
		this.resourceStorage = base.GetComponent<Storage>();
		this.particleStorage = base.GetComponent<HighEnergyParticleStorage>();
		if (RailGun.noSurfaceSightStatusItem == null)
		{
			RailGun.noSurfaceSightStatusItem = new StatusItem("RAILGUN_PATH_NOT_CLEAR", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		}
		if (RailGun.noDestinationStatusItem == null)
		{
			RailGun.noDestinationStatusItem = new StatusItem("RAILGUN_NO_DESTINATION", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		}
		this.gasInputCell = Grid.OffsetCell(Grid.PosToCell(this), this.gasPortInfo.offset);
		this.gasConsumer = this.CreateConduitConsumer(ConduitType.Gas, this.gasInputCell, out this.gasNetworkItem);
		this.liquidInputCell = Grid.OffsetCell(Grid.PosToCell(this), this.liquidPortInfo.offset);
		this.liquidConsumer = this.CreateConduitConsumer(ConduitType.Liquid, this.liquidInputCell, out this.liquidNetworkItem);
		this.solidInputCell = Grid.OffsetCell(Grid.PosToCell(this), this.solidPortInfo.offset);
		this.solidConsumer = this.CreateSolidConduitConsumer(this.solidInputCell, out this.solidNetworkItem);
		this.CreateMeters();
		base.smi.StartSM();
		if (RailGun.infoStatusItemLogic == null)
		{
			RailGun.infoStatusItemLogic = new StatusItem("LogicOperationalInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			RailGun.infoStatusItemLogic.resolveStringCallback = new Func<string, object, string>(RailGun.ResolveInfoStatusItemString);
		}
		this.CheckLogicWireState();
		base.Subscribe<RailGun>(-801688580, RailGun.OnLogicValueChangedDelegate);
	}

	// Token: 0x0600320D RID: 12813 RVA: 0x00112E98 File Offset: 0x00111098
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidInputCell, this.liquidNetworkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasInputCell, this.gasNetworkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidInputCell, this.solidConsumer, true);
		base.OnCleanUp();
	}

	// Token: 0x0600320E RID: 12814 RVA: 0x00112F0C File Offset: 0x0011110C
	private void CreateMeters()
	{
		this.resourceMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_storage_target", "meter_storage", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_orb_target", "meter_orb", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x0600320F RID: 12815 RVA: 0x00112F5F File Offset: 0x0011115F
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.liquidPortInfo.conduitType == type || this.gasPortInfo.conduitType == type || this.solidPortInfo.conduitType == type;
	}

	// Token: 0x06003210 RID: 12816 RVA: 0x00112F90 File Offset: 0x00111190
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.liquidPortInfo.conduitType == type)
		{
			return this.liquidPortInfo.offset;
		}
		if (this.gasPortInfo.conduitType == type)
		{
			return this.gasPortInfo.offset;
		}
		if (this.solidPortInfo.conduitType == type)
		{
			return this.solidPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x06003211 RID: 12817 RVA: 0x00112FF0 File Offset: 0x001111F0
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(RailGun.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x06003212 RID: 12818 RVA: 0x00113020 File Offset: 0x00111220
	private void CheckLogicWireState()
	{
		LogicCircuitNetwork network = this.GetNetwork();
		this.hasLogicWire = (network != null);
		int value = (network != null) ? network.OutputValue : 1;
		bool flag = LogicCircuitNetwork.IsBitActive(0, value);
		this.isLogicActive = flag;
		base.smi.sm.allowedFromLogic.Set(this.AllowLaunchingFromLogic, base.smi, false);
		base.GetComponent<KSelectable>().ToggleStatusItem(RailGun.infoStatusItemLogic, network != null, this);
	}

	// Token: 0x06003213 RID: 12819 RVA: 0x00113093 File Offset: 0x00111293
	private void OnLogicValueChanged(object data)
	{
		if (((LogicValueChanged)data).portID == RailGun.PORT_ID)
		{
			this.CheckLogicWireState();
		}
	}

	// Token: 0x06003214 RID: 12820 RVA: 0x001130B2 File Offset: 0x001112B2
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		RailGun railGun = (RailGun)data;
		Operational operational = railGun.operational;
		return railGun.AllowLaunchingFromLogic ? BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_ENABLED : BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_DISABLED;
	}

	// Token: 0x06003215 RID: 12821 RVA: 0x001130DC File Offset: 0x001112DC
	public void Sim200ms(float dt)
	{
		WorldContainer myWorld = this.GetMyWorld();
		Extents extents = base.GetComponent<Building>().GetExtents();
		int x = extents.x;
		int x2 = extents.x + extents.width - 2;
		int y = extents.y + extents.height;
		int num = Grid.XYToCell(x, y);
		int num2 = Grid.XYToCell(x2, y);
		bool flag = true;
		int num3 = (int)myWorld.maximumBounds.y;
		for (int i = num; i <= num2; i++)
		{
			int num4 = i;
			while (Grid.CellRow(num4) <= num3)
			{
				if (!Grid.IsValidCell(num4) || Grid.Solid[num4])
				{
					flag = false;
					break;
				}
				num4 = Grid.CellAbove(num4);
			}
		}
		this.operational.SetFlag(RailGun.noSurfaceSight, flag);
		this.operational.SetFlag(RailGun.noDestination, this.destinationSelector.GetDestinationWorld() >= 0);
		KSelectable component = base.GetComponent<KSelectable>();
		component.ToggleStatusItem(RailGun.noSurfaceSightStatusItem, !flag, null);
		component.ToggleStatusItem(RailGun.noDestinationStatusItem, this.destinationSelector.GetDestinationWorld() < 0, null);
		this.UpdateMeters();
	}

	// Token: 0x06003216 RID: 12822 RVA: 0x001131F4 File Offset: 0x001113F4
	private void UpdateMeters()
	{
		this.resourceMeter.SetPositionPercent(Mathf.Clamp01(this.resourceStorage.MassStored() / this.resourceStorage.capacityKg));
		this.particleMeter.SetPositionPercent(Mathf.Clamp01(this.particleStorage.Particles / this.particleStorage.capacity));
	}

	// Token: 0x06003217 RID: 12823 RVA: 0x00113250 File Offset: 0x00111450
	private void LaunchProjectile()
	{
		Extents extents = base.GetComponent<Building>().GetExtents();
		Vector2I vector2I = Grid.PosToXY(base.transform.position);
		vector2I.y += extents.height + 1;
		int cell = Grid.XYToCell(vector2I.x, vector2I.y);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RailGunPayload"), Grid.CellToPosCBC(cell, Grid.SceneLayer.Front));
		Storage component = gameObject.GetComponent<Storage>();
		float num = 0f;
		while (num < this.launchMass && this.resourceStorage.MassStored() > 0f)
		{
			num += this.resourceStorage.Transfer(component, GameTags.Stored, this.launchMass - num, false, true);
		}
		component.SetContentsDeleteOffGrid(false);
		this.particleStorage.ConsumeAndGet(base.smi.EnergyCost());
		gameObject.SetActive(true);
		if (this.destinationSelector.GetDestinationWorld() >= 0)
		{
			RailGunPayload.StatesInstance smi = gameObject.GetSMI<RailGunPayload.StatesInstance>();
			smi.takeoffVelocity = 35f;
			smi.StartSM();
			smi.Launch(base.gameObject.GetMyWorldLocation(), this.destinationSelector.GetDestination());
		}
	}

	// Token: 0x06003218 RID: 12824 RVA: 0x00113371 File Offset: 0x00111571
	private ConduitConsumer CreateConduitConsumer(ConduitType inputType, int inputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		ConduitConsumer conduitConsumer = base.gameObject.AddComponent<ConduitConsumer>();
		conduitConsumer.conduitType = inputType;
		conduitConsumer.useSecondaryInput = true;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(inputType);
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(inputType, Endpoint.Sink, inputCell, base.gameObject);
		networkManager.AddToNetworks(inputCell, flowNetworkItem, true);
		return conduitConsumer;
	}

	// Token: 0x06003219 RID: 12825 RVA: 0x001133AB File Offset: 0x001115AB
	private SolidConduitConsumer CreateSolidConduitConsumer(int inputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		SolidConduitConsumer solidConduitConsumer = base.gameObject.AddComponent<SolidConduitConsumer>();
		solidConduitConsumer.useSecondaryInput = true;
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Sink, inputCell, base.gameObject);
		Game.Instance.solidConduitSystem.AddToNetworks(inputCell, flowNetworkItem, true);
		return solidConduitConsumer;
	}

	// Token: 0x04001D76 RID: 7542
	[Serialize]
	public float launchMass = 200f;

	// Token: 0x04001D77 RID: 7543
	public float MinLaunchMass = 2f;

	// Token: 0x04001D78 RID: 7544
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001D79 RID: 7545
	[MyCmpGet]
	private KAnimControllerBase kac;

	// Token: 0x04001D7A RID: 7546
	[MyCmpGet]
	public HighEnergyParticleStorage hepStorage;

	// Token: 0x04001D7B RID: 7547
	public Storage resourceStorage;

	// Token: 0x04001D7C RID: 7548
	private MeterController resourceMeter;

	// Token: 0x04001D7D RID: 7549
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x04001D7E RID: 7550
	private MeterController particleMeter;

	// Token: 0x04001D7F RID: 7551
	private ClusterDestinationSelector destinationSelector;

	// Token: 0x04001D80 RID: 7552
	public static readonly Operational.Flag noSurfaceSight = new Operational.Flag("noSurfaceSight", Operational.Flag.Type.Requirement);

	// Token: 0x04001D81 RID: 7553
	private static StatusItem noSurfaceSightStatusItem;

	// Token: 0x04001D82 RID: 7554
	public static readonly Operational.Flag noDestination = new Operational.Flag("noDestination", Operational.Flag.Type.Requirement);

	// Token: 0x04001D83 RID: 7555
	private static StatusItem noDestinationStatusItem;

	// Token: 0x04001D84 RID: 7556
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x04001D85 RID: 7557
	private int liquidInputCell = -1;

	// Token: 0x04001D86 RID: 7558
	private FlowUtilityNetwork.NetworkItem liquidNetworkItem;

	// Token: 0x04001D87 RID: 7559
	private ConduitConsumer liquidConsumer;

	// Token: 0x04001D88 RID: 7560
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x04001D89 RID: 7561
	private int gasInputCell = -1;

	// Token: 0x04001D8A RID: 7562
	private FlowUtilityNetwork.NetworkItem gasNetworkItem;

	// Token: 0x04001D8B RID: 7563
	private ConduitConsumer gasConsumer;

	// Token: 0x04001D8C RID: 7564
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x04001D8D RID: 7565
	private int solidInputCell = -1;

	// Token: 0x04001D8E RID: 7566
	private FlowUtilityNetwork.NetworkItem solidNetworkItem;

	// Token: 0x04001D8F RID: 7567
	private SolidConduitConsumer solidConsumer;

	// Token: 0x04001D90 RID: 7568
	public static readonly HashedString PORT_ID = "LogicLaunching";

	// Token: 0x04001D91 RID: 7569
	private bool hasLogicWire;

	// Token: 0x04001D92 RID: 7570
	private bool isLogicActive;

	// Token: 0x04001D93 RID: 7571
	private static StatusItem infoStatusItemLogic;

	// Token: 0x04001D94 RID: 7572
	public bool FreeStartHex;

	// Token: 0x04001D95 RID: 7573
	public bool FreeDestinationHex;

	// Token: 0x04001D96 RID: 7574
	private static readonly EventSystem.IntraObjectHandler<RailGun> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<RailGun>(delegate(RailGun component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x020015C2 RID: 5570
	public class StatesInstance : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.GameInstance
	{
		// Token: 0x06008FAC RID: 36780 RVA: 0x003484AE File Offset: 0x003466AE
		public StatesInstance(RailGun smi) : base(smi)
		{
		}

		// Token: 0x06008FAD RID: 36781 RVA: 0x003484B7 File Offset: 0x003466B7
		public bool HasResources()
		{
			return base.smi.master.resourceStorage.MassStored() >= base.smi.master.launchMass;
		}

		// Token: 0x06008FAE RID: 36782 RVA: 0x003484E3 File Offset: 0x003466E3
		public bool HasEnergy()
		{
			return base.smi.master.particleStorage.Particles > this.EnergyCost();
		}

		// Token: 0x06008FAF RID: 36783 RVA: 0x00348502 File Offset: 0x00346702
		public bool HasDestination()
		{
			return base.smi.master.destinationSelector.GetDestinationWorld() != base.smi.master.GetMyWorldId();
		}

		// Token: 0x06008FB0 RID: 36784 RVA: 0x0034852E File Offset: 0x0034672E
		public bool IsDestinationReachable(bool forceRefresh = false)
		{
			if (forceRefresh)
			{
				this.UpdatePath();
			}
			return base.smi.master.destinationSelector.GetDestinationWorld() != base.smi.master.GetMyWorldId() && this.PathLength() != -1;
		}

		// Token: 0x06008FB1 RID: 36785 RVA: 0x00348570 File Offset: 0x00346770
		public int PathLength()
		{
			if (base.smi.m_cachedPath == null)
			{
				this.UpdatePath();
			}
			if (base.smi.m_cachedPath == null)
			{
				return -1;
			}
			int num = base.smi.m_cachedPath.Count;
			if (base.master.FreeStartHex)
			{
				num--;
			}
			if (base.master.FreeDestinationHex)
			{
				num--;
			}
			return num;
		}

		// Token: 0x06008FB2 RID: 36786 RVA: 0x003485D4 File Offset: 0x003467D4
		public void UpdatePath()
		{
			this.m_cachedPath = ClusterGrid.Instance.GetPath(base.gameObject.GetMyWorldLocation(), base.smi.master.destinationSelector.GetDestination(), base.smi.master.destinationSelector);
		}

		// Token: 0x06008FB3 RID: 36787 RVA: 0x00348621 File Offset: 0x00346821
		public float EnergyCost()
		{
			return Mathf.Max(0f, 0f + (float)this.PathLength() * 10f);
		}

		// Token: 0x06008FB4 RID: 36788 RVA: 0x00348640 File Offset: 0x00346840
		public bool MayTurnOn()
		{
			return this.HasEnergy() && this.IsDestinationReachable(false) && base.master.operational.IsOperational && base.sm.allowedFromLogic.Get(this);
		}

		// Token: 0x04006DA8 RID: 28072
		public const int INVALID_PATH_LENGTH = -1;

		// Token: 0x04006DA9 RID: 28073
		private List<AxialI> m_cachedPath;
	}

	// Token: 0x020015C3 RID: 5571
	public class States : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun>
	{
		// Token: 0x06008FB5 RID: 36789 RVA: 0x00348678 File Offset: 0x00346878
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			this.root.EventHandler(GameHashes.ClusterDestinationChanged, delegate(RailGun.StatesInstance smi)
			{
				smi.UpdatePath();
			});
			this.off.PlayAnim("off").EventTransition(GameHashes.OnParticleStorageChanged, this.on, (RailGun.StatesInstance smi) => smi.MayTurnOn()).EventTransition(GameHashes.ClusterDestinationChanged, this.on, (RailGun.StatesInstance smi) => smi.MayTurnOn()).EventTransition(GameHashes.OperationalChanged, this.on, (RailGun.StatesInstance smi) => smi.MayTurnOn()).ParamTransition<bool>(this.allowedFromLogic, this.on, (RailGun.StatesInstance smi, bool p) => smi.MayTurnOn());
			this.on.DefaultState(this.on.power_on).EventTransition(GameHashes.OperationalChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.master.operational.IsOperational).EventTransition(GameHashes.ClusterDestinationChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.IsDestinationReachable(false)).EventTransition(GameHashes.ClusterFogOfWarRevealed, (RailGun.StatesInstance smi) => Game.Instance, this.on.power_off, (RailGun.StatesInstance smi) => !smi.IsDestinationReachable(true)).EventTransition(GameHashes.OnParticleStorageChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.MayTurnOn()).ParamTransition<bool>(this.allowedFromLogic, this.on.power_off, (RailGun.StatesInstance smi, bool p) => !p).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null);
			this.on.power_on.PlayAnim("power_on").OnAnimQueueComplete(this.on.wait_for_storage);
			this.on.power_off.PlayAnim("power_off").OnAnimQueueComplete(this.off);
			this.on.wait_for_storage.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.ClusterDestinationChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.HasEnergy()).EventTransition(GameHashes.OnStorageChange, this.on.working, (RailGun.StatesInstance smi) => smi.HasResources() && smi.sm.cooldownTimer.Get(smi) <= 0f).EventTransition(GameHashes.OperationalChanged, this.on.working, (RailGun.StatesInstance smi) => smi.HasResources() && smi.sm.cooldownTimer.Get(smi) <= 0f).EventTransition(GameHashes.RailGunLaunchMassChanged, this.on.working, (RailGun.StatesInstance smi) => smi.HasResources() && smi.sm.cooldownTimer.Get(smi) <= 0f).ParamTransition<float>(this.cooldownTimer, this.on.cooldown, (RailGun.StatesInstance smi, float p) => p > 0f);
			this.on.working.DefaultState(this.on.working.pre).Enter(delegate(RailGun.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(RailGun.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.on.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working.loop);
			this.on.working.loop.PlayAnim("working_loop").OnAnimQueueComplete(this.on.working.fire);
			this.on.working.fire.Enter(delegate(RailGun.StatesInstance smi)
			{
				if (smi.IsDestinationReachable(false))
				{
					smi.master.LaunchProjectile();
					smi.sm.payloadsFiredSinceCooldown.Delta(1, smi);
					if (smi.sm.payloadsFiredSinceCooldown.Get(smi) >= 6)
					{
						smi.sm.cooldownTimer.Set(30f, smi, false);
					}
				}
			}).GoTo(this.on.working.bounce);
			this.on.working.bounce.ParamTransition<float>(this.cooldownTimer, this.on.working.pst, (RailGun.StatesInstance smi, float p) => p > 0f || !smi.HasResources()).ParamTransition<int>(this.payloadsFiredSinceCooldown, this.on.working.loop, (RailGun.StatesInstance smi, int p) => p < 6 && smi.HasResources());
			this.on.working.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.wait_for_storage);
			this.on.cooldown.DefaultState(this.on.cooldown.pre).ToggleMainStatusItem(Db.Get().BuildingStatusItems.RailGunCooldown, null);
			this.on.cooldown.pre.PlayAnim("cooldown_pre").OnAnimQueueComplete(this.on.cooldown.loop);
			this.on.cooldown.loop.PlayAnim("cooldown_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.cooldownTimer, this.on.cooldown.pst, (RailGun.StatesInstance smi, float p) => p <= 0f).Update(delegate(RailGun.StatesInstance smi, float dt)
			{
				this.cooldownTimer.Delta(-dt, smi);
			}, UpdateRate.SIM_1000ms, false);
			this.on.cooldown.pst.PlayAnim("cooldown_pst").OnAnimQueueComplete(this.on.wait_for_storage).Exit(delegate(RailGun.StatesInstance smi)
			{
				smi.sm.payloadsFiredSinceCooldown.Set(0, smi, false);
			});
		}

		// Token: 0x04006DAA RID: 28074
		public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State off;

		// Token: 0x04006DAB RID: 28075
		public RailGun.States.OnStates on;

		// Token: 0x04006DAC RID: 28076
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.FloatParameter cooldownTimer;

		// Token: 0x04006DAD RID: 28077
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.IntParameter payloadsFiredSinceCooldown;

		// Token: 0x04006DAE RID: 28078
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.BoolParameter allowedFromLogic;

		// Token: 0x04006DAF RID: 28079
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.BoolParameter updatePath;

		// Token: 0x0200251A RID: 9498
		public class WorkingStates : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State
		{
			// Token: 0x0400A527 RID: 42279
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pre;

			// Token: 0x0400A528 RID: 42280
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State loop;

			// Token: 0x0400A529 RID: 42281
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State fire;

			// Token: 0x0400A52A RID: 42282
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State bounce;

			// Token: 0x0400A52B RID: 42283
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pst;
		}

		// Token: 0x0200251B RID: 9499
		public class CooldownStates : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State
		{
			// Token: 0x0400A52C RID: 42284
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pre;

			// Token: 0x0400A52D RID: 42285
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State loop;

			// Token: 0x0400A52E RID: 42286
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pst;
		}

		// Token: 0x0200251C RID: 9500
		public class OnStates : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State
		{
			// Token: 0x0400A52F RID: 42287
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State power_on;

			// Token: 0x0400A530 RID: 42288
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State wait_for_storage;

			// Token: 0x0400A531 RID: 42289
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State power_off;

			// Token: 0x0400A532 RID: 42290
			public RailGun.States.WorkingStates working;

			// Token: 0x0400A533 RID: 42291
			public RailGun.States.CooldownStates cooldown;
		}
	}
}
