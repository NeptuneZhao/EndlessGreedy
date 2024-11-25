using System;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200078D RID: 1933
[SerializationConfig(MemberSerialization.OptIn)]
public class TravelTubeEntrance : StateMachineComponent<TravelTubeEntrance.SMInstance>, ISaveLoadable, ISim200ms
{
	// Token: 0x1700039C RID: 924
	// (get) Token: 0x060034A7 RID: 13479 RVA: 0x0011EF7B File Offset: 0x0011D17B
	public float AvailableJoules
	{
		get
		{
			return this.availableJoules;
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x060034A8 RID: 13480 RVA: 0x0011EF83 File Offset: 0x0011D183
	public float TotalCapacity
	{
		get
		{
			return this.jouleCapacity;
		}
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x060034A9 RID: 13481 RVA: 0x0011EF8B File Offset: 0x0011D18B
	public float UsageJoules
	{
		get
		{
			return this.joulesPerLaunch;
		}
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x060034AA RID: 13482 RVA: 0x0011EF93 File Offset: 0x0011D193
	public bool HasLaunchPower
	{
		get
		{
			return this.availableJoules > this.joulesPerLaunch;
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x060034AB RID: 13483 RVA: 0x0011EFA3 File Offset: 0x0011D1A3
	public bool HasWaxForGreasyLaunch
	{
		get
		{
			return this.storage.GetAmountAvailable(SimHashes.MilkFat.CreateTag()) >= this.waxPerLaunch;
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x060034AC RID: 13484 RVA: 0x0011EFC5 File Offset: 0x0011D1C5
	public int WaxLaunchesAvailable
	{
		get
		{
			return Mathf.FloorToInt(this.storage.GetAmountAvailable(SimHashes.MilkFat.CreateTag()) / this.waxPerLaunch);
		}
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x060034AD RID: 13485 RVA: 0x0011EFE8 File Offset: 0x0011D1E8
	private bool ShouldUseWaxLaunchAnimation
	{
		get
		{
			return this.deliverAndUseWax && this.HasWaxForGreasyLaunch;
		}
	}

	// Token: 0x060034AE RID: 13486 RVA: 0x0011EFFC File Offset: 0x0011D1FC
	public static void SetTravelerGleamEffect(TravelTubeEntrance.SMInstance smi)
	{
		TravelTubeEntrance.Work component = smi.GetComponent<TravelTubeEntrance.Work>();
		if (component.worker != null)
		{
			component.worker.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("gleam", smi.master.ShouldUseWaxLaunchAnimation);
		}
	}

	// Token: 0x060034AF RID: 13487 RVA: 0x0011F043 File Offset: 0x0011D243
	public static string GetLaunchAnimName(TravelTubeEntrance.SMInstance smi)
	{
		if (!smi.master.ShouldUseWaxLaunchAnimation)
		{
			return "working_pre";
		}
		return "wax";
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x0011F05D File Offset: 0x0011D25D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.energyConsumer.OnConnectionChanged += this.OnConnectionChanged;
	}

	// Token: 0x060034B1 RID: 13489 RVA: 0x0011F07C File Offset: 0x0011D27C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetWaxUse(this.deliverAndUseWax);
		int x = (int)base.transform.GetPosition().x;
		int y = (int)base.transform.GetPosition().y + 2;
		Extents extents = new Extents(x, y, 1, 1);
		UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(Grid.XYToCell(x, y), true);
		this.TubeConnectionsChanged(connections);
		this.tubeChangedEntry = GameScenePartitioner.Instance.Add("TravelTubeEntrance.TubeListener", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[35], new Action<object>(this.TubeChanged));
		base.Subscribe<TravelTubeEntrance>(-592767678, TravelTubeEntrance.OnOperationalChangedDelegate);
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.waxMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "wax_meter_target", "wax_meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.CreateNewWaitReactable();
		Grid.RegisterTubeEntrance(Grid.PosToCell(this), Mathf.FloorToInt(this.availableJoules / this.joulesPerLaunch));
		base.smi.StartSM();
		this.UpdateWaxCharge();
		this.UpdateCharge();
		base.Subscribe<TravelTubeEntrance>(493375141, TravelTubeEntrance.OnRefreshUserMenuDelegate);
	}

	// Token: 0x060034B2 RID: 13490 RVA: 0x0011F1D0 File Offset: 0x0011D3D0
	private void OnStorageChanged(object obj)
	{
		this.UpdateWaxCharge();
	}

	// Token: 0x060034B3 RID: 13491 RVA: 0x0011F1D8 File Offset: 0x0011D3D8
	protected override void OnCleanUp()
	{
		if (this.travelTube != null)
		{
			this.travelTube.Unsubscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = null;
		}
		Grid.UnregisterTubeEntrance(Grid.PosToCell(this));
		this.ClearWaitReactable();
		GameScenePartitioner.Instance.Free(ref this.tubeChangedEntry);
		base.OnCleanUp();
	}

	// Token: 0x060034B4 RID: 13492 RVA: 0x0011F240 File Offset: 0x0011D440
	private void OnRefreshUserMenu(object data)
	{
		if (!this.deliverAndUseWax)
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_speed_up", UI.USERMENUACTIONS.TRANSITTUBEWAX.NAME, delegate()
			{
				this.SetWaxUse(true);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.TRANSITTUBEWAX.TOOLTIP, true), 1f);
		}
		else
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_speed_up", UI.USERMENUACTIONS.CANCELTRANSITTUBEWAX.NAME, delegate()
			{
				this.SetWaxUse(false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELTRANSITTUBEWAX.TOOLTIP, true), 1f);
		}
		KSelectable component = base.GetComponent<KSelectable>();
		bool flag = this.deliverAndUseWax && this.WaxLaunchesAvailable > 0;
		if (component != null)
		{
			if (flag)
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.TransitTubeEntranceWaxReady, this);
				return;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.TransitTubeEntranceWaxReady, false);
		}
	}

	// Token: 0x060034B5 RID: 13493 RVA: 0x0011F348 File Offset: 0x0011D548
	public void SetWaxUse(bool usingWax)
	{
		this.deliverAndUseWax = usingWax;
		this.manualDelivery.AbortDelivery("Switching to new delivery request");
		this.manualDelivery.capacity = (usingWax ? this.storage.capacityKg : 0f);
		this.manualDelivery.refillMass = (usingWax ? this.waxPerLaunch : 0f);
		this.manualDelivery.MinimumMass = (usingWax ? this.waxPerLaunch : 0f);
		if (!usingWax)
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x060034B6 RID: 13494 RVA: 0x0011F3E4 File Offset: 0x0011D5E4
	private void TubeChanged(object data)
	{
		if (this.travelTube != null)
		{
			this.travelTube.Unsubscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = null;
		}
		GameObject gameObject = data as GameObject;
		if (data == null)
		{
			this.TubeConnectionsChanged(0);
			return;
		}
		TravelTube component = gameObject.GetComponent<TravelTube>();
		if (component != null)
		{
			component.Subscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = component;
			return;
		}
		this.TubeConnectionsChanged(0);
	}

	// Token: 0x060034B7 RID: 13495 RVA: 0x0011F478 File Offset: 0x0011D678
	private void TubeConnectionsChanged(object data)
	{
		bool value = (UtilityConnections)data == UtilityConnections.Up;
		this.operational.SetFlag(TravelTubeEntrance.tubeConnected, value);
	}

	// Token: 0x060034B8 RID: 13496 RVA: 0x0011F4A0 File Offset: 0x0011D6A0
	private bool CanAcceptMorePower()
	{
		return this.operational.IsOperational && (this.button == null || this.button.IsEnabled) && this.energyConsumer.IsExternallyPowered && this.availableJoules < this.jouleCapacity;
	}

	// Token: 0x060034B9 RID: 13497 RVA: 0x0011F4F4 File Offset: 0x0011D6F4
	public void Sim200ms(float dt)
	{
		if (this.CanAcceptMorePower())
		{
			this.availableJoules = Mathf.Min(this.jouleCapacity, this.availableJoules + this.energyConsumer.WattsUsed * dt);
			this.UpdateCharge();
		}
		this.energyConsumer.SetSustained(this.HasLaunchPower);
		this.UpdateActive();
		this.UpdateConnectionStatus();
	}

	// Token: 0x060034BA RID: 13498 RVA: 0x0011F551 File Offset: 0x0011D751
	public void Reserve(TubeTraveller.Instance traveller, int prefabInstanceID)
	{
		Grid.ReserveTubeEntrance(Grid.PosToCell(this), prefabInstanceID, true);
	}

	// Token: 0x060034BB RID: 13499 RVA: 0x0011F561 File Offset: 0x0011D761
	public void Unreserve(TubeTraveller.Instance traveller, int prefabInstanceID)
	{
		Grid.ReserveTubeEntrance(Grid.PosToCell(this), prefabInstanceID, false);
	}

	// Token: 0x060034BC RID: 13500 RVA: 0x0011F571 File Offset: 0x0011D771
	public bool IsTraversable(Navigator agent)
	{
		return Grid.HasUsableTubeEntrance(Grid.PosToCell(this), agent.gameObject.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060034BD RID: 13501 RVA: 0x0011F58E File Offset: 0x0011D78E
	public bool HasChargeSlotReserved(Navigator agent)
	{
		return Grid.HasReservedTubeEntrance(Grid.PosToCell(this), agent.gameObject.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x0011F5AB File Offset: 0x0011D7AB
	public bool HasChargeSlotReserved(TubeTraveller.Instance tube_traveller, int prefabInstanceID)
	{
		return Grid.HasReservedTubeEntrance(Grid.PosToCell(this), prefabInstanceID);
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x0011F5B9 File Offset: 0x0011D7B9
	public bool IsChargedSlotAvailable(TubeTraveller.Instance tube_traveller, int prefabInstanceID)
	{
		return Grid.HasUsableTubeEntrance(Grid.PosToCell(this), prefabInstanceID);
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x0011F5C8 File Offset: 0x0011D7C8
	public bool ShouldWait(GameObject reactor)
	{
		if (!this.operational.IsOperational)
		{
			return false;
		}
		if (!this.HasLaunchPower)
		{
			return false;
		}
		if (this.launch_workable.worker == null)
		{
			return false;
		}
		TubeTraveller.Instance smi = reactor.GetSMI<TubeTraveller.Instance>();
		return this.HasChargeSlotReserved(smi, reactor.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x0011F61C File Offset: 0x0011D81C
	public void ConsumeCharge(GameObject reactor)
	{
		if (this.HasLaunchPower)
		{
			this.availableJoules -= this.joulesPerLaunch;
			if (this.deliverAndUseWax && this.HasWaxForGreasyLaunch)
			{
				TubeTraveller.Instance smi = reactor.GetSMI<TubeTraveller.Instance>();
				if (smi != null)
				{
					Tag tag = SimHashes.MilkFat.CreateTag();
					float num;
					SimUtil.DiseaseInfo diseaseInfo;
					float num2;
					this.storage.ConsumeAndGetDisease(tag, this.waxPerLaunch, out num, out diseaseInfo, out num2);
					GermExposureMonitor.Instance smi2 = reactor.GetSMI<GermExposureMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, tag, Sickness.InfectionVector.Contact);
					}
					smi.SetWaxState(true);
				}
			}
			this.UpdateCharge();
			this.UpdateWaxCharge();
		}
	}

	// Token: 0x060034C2 RID: 13506 RVA: 0x0011F6B8 File Offset: 0x0011D8B8
	private void CreateNewWaitReactable()
	{
		if (this.wait_reactable == null)
		{
			this.wait_reactable = new TravelTubeEntrance.WaitReactable(this);
		}
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x0011F6CE File Offset: 0x0011D8CE
	private void OrphanWaitReactable()
	{
		this.wait_reactable = null;
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x0011F6D7 File Offset: 0x0011D8D7
	private void ClearWaitReactable()
	{
		if (this.wait_reactable != null)
		{
			this.wait_reactable.Cleanup();
			this.wait_reactable = null;
		}
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x0011F6F4 File Offset: 0x0011D8F4
	private void OnOperationalChanged(object data)
	{
		bool flag = (bool)data;
		Grid.SetTubeEntranceOperational(Grid.PosToCell(this), flag);
		this.UpdateActive();
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x0011F71A File Offset: 0x0011D91A
	private void OnConnectionChanged()
	{
		this.UpdateActive();
		this.UpdateConnectionStatus();
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x0011F728 File Offset: 0x0011D928
	private void UpdateActive()
	{
		this.operational.SetActive(this.CanAcceptMorePower(), false);
	}

	// Token: 0x060034C8 RID: 13512 RVA: 0x0011F73C File Offset: 0x0011D93C
	private void UpdateCharge()
	{
		base.smi.sm.hasLaunchCharges.Set(this.HasLaunchPower, base.smi, false);
		float positionPercent = Mathf.Clamp01(this.availableJoules / this.jouleCapacity);
		this.meter.SetPositionPercent(positionPercent);
		this.energyConsumer.UpdatePoweredStatus();
		Grid.SetTubeEntranceReservationCapacity(Grid.PosToCell(this), Mathf.FloorToInt(this.availableJoules / this.joulesPerLaunch));
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x060034C9 RID: 13513 RVA: 0x0011F7BC File Offset: 0x0011D9BC
	private void UpdateWaxCharge()
	{
		float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
		this.waxMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x060034CA RID: 13514 RVA: 0x0011F7F4 File Offset: 0x0011D9F4
	private void UpdateConnectionStatus()
	{
		bool flag = this.button != null && !this.button.IsEnabled;
		bool isConnected = this.energyConsumer.IsConnected;
		bool hasLaunchPower = this.HasLaunchPower;
		if (flag || !isConnected || hasLaunchPower)
		{
			this.connectedStatus = this.selectable.RemoveStatusItem(this.connectedStatus, false);
			return;
		}
		if (this.connectedStatus == Guid.Empty)
		{
			this.connectedStatus = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotEnoughPower, null);
		}
	}

	// Token: 0x04001F1A RID: 7962
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001F1B RID: 7963
	[MyCmpReq]
	private TravelTubeEntrance.Work launch_workable;

	// Token: 0x04001F1C RID: 7964
	[MyCmpReq]
	private EnergyConsumerSelfSustaining energyConsumer;

	// Token: 0x04001F1D RID: 7965
	[MyCmpGet]
	private BuildingEnabledButton button;

	// Token: 0x04001F1E RID: 7966
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001F1F RID: 7967
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001F20 RID: 7968
	[MyCmpReq]
	private ManualDeliveryKG manualDelivery;

	// Token: 0x04001F21 RID: 7969
	public float jouleCapacity = 1f;

	// Token: 0x04001F22 RID: 7970
	public float joulesPerLaunch = 1f;

	// Token: 0x04001F23 RID: 7971
	public float waxPerLaunch;

	// Token: 0x04001F24 RID: 7972
	[Serialize]
	private float availableJoules;

	// Token: 0x04001F25 RID: 7973
	[Serialize]
	private bool deliverAndUseWax;

	// Token: 0x04001F26 RID: 7974
	private TravelTube travelTube;

	// Token: 0x04001F27 RID: 7975
	public const string WAX_LAUNCH_ANIM_NAME = "wax";

	// Token: 0x04001F28 RID: 7976
	private TravelTubeEntrance.WaitReactable wait_reactable;

	// Token: 0x04001F29 RID: 7977
	private MeterController meter;

	// Token: 0x04001F2A RID: 7978
	private MeterController waxMeter;

	// Token: 0x04001F2B RID: 7979
	private const int MAX_CHARGES = 3;

	// Token: 0x04001F2C RID: 7980
	private const float RECHARGE_TIME = 10f;

	// Token: 0x04001F2D RID: 7981
	private static readonly Operational.Flag tubeConnected = new Operational.Flag("tubeConnected", Operational.Flag.Type.Functional);

	// Token: 0x04001F2E RID: 7982
	private HandleVector<int>.Handle tubeChangedEntry;

	// Token: 0x04001F2F RID: 7983
	private static readonly EventSystem.IntraObjectHandler<TravelTubeEntrance> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<TravelTubeEntrance>(delegate(TravelTubeEntrance component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001F30 RID: 7984
	private static readonly EventSystem.IntraObjectHandler<TravelTubeEntrance> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TravelTubeEntrance>(delegate(TravelTubeEntrance component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001F31 RID: 7985
	private Guid connectedStatus;

	// Token: 0x02001637 RID: 5687
	private class LaunchReactable : WorkableReactable
	{
		// Token: 0x0600917B RID: 37243 RVA: 0x0035063C File Offset: 0x0034E83C
		public LaunchReactable(Workable workable, TravelTubeEntrance entrance) : base(workable, "LaunchReactable", Db.Get().ChoreTypes.TravelTubeEntrance, WorkableReactable.AllowedDirection.Any)
		{
			this.entrance = entrance;
		}

		// Token: 0x0600917C RID: 37244 RVA: 0x00350668 File Offset: 0x0034E868
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				Navigator component = new_reactor.GetComponent<Navigator>();
				return component && this.entrance.HasChargeSlotReserved(component);
			}
			return false;
		}

		// Token: 0x04006F06 RID: 28422
		private TravelTubeEntrance entrance;
	}

	// Token: 0x02001638 RID: 5688
	private class WaitReactable : Reactable
	{
		// Token: 0x0600917D RID: 37245 RVA: 0x003506A0 File Offset: 0x0034E8A0
		public WaitReactable(TravelTubeEntrance entrance) : base(entrance.gameObject, "WaitReactable", Db.Get().ChoreTypes.TravelTubeEntrance, 2, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.entrance = entrance;
			this.preventChoreInterruption = false;
		}

		// Token: 0x0600917E RID: 37246 RVA: 0x003506F9 File Offset: 0x0034E8F9
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.entrance == null)
			{
				base.Cleanup();
				return false;
			}
			return this.entrance.ShouldWait(new_reactor);
		}

		// Token: 0x0600917F RID: 37247 RVA: 0x00350730 File Offset: 0x0034E930
		protected override void InternalBegin()
		{
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"), 1f);
			component.Play("idle_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			this.entrance.OrphanWaitReactable();
			this.entrance.CreateNewWaitReactable();
		}

		// Token: 0x06009180 RID: 37248 RVA: 0x003507AD File Offset: 0x0034E9AD
		public override void Update(float dt)
		{
			if (this.entrance == null)
			{
				base.Cleanup();
				return;
			}
			if (!this.entrance.ShouldWait(this.reactor))
			{
				base.Cleanup();
			}
		}

		// Token: 0x06009181 RID: 37249 RVA: 0x003507DD File Offset: 0x0034E9DD
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"));
			}
		}

		// Token: 0x06009182 RID: 37250 RVA: 0x0035080C File Offset: 0x0034EA0C
		protected override void InternalCleanup()
		{
		}

		// Token: 0x04006F07 RID: 28423
		private TravelTubeEntrance entrance;
	}

	// Token: 0x02001639 RID: 5689
	public class SMInstance : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.GameInstance
	{
		// Token: 0x06009183 RID: 37251 RVA: 0x0035080E File Offset: 0x0034EA0E
		public SMInstance(TravelTubeEntrance master) : base(master)
		{
		}
	}

	// Token: 0x0200163A RID: 5690
	public class States : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance>
	{
		// Token: 0x06009184 RID: 37252 RVA: 0x00350818 File Offset: 0x0034EA18
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notoperational;
			this.root.ToggleStatusItem(Db.Get().BuildingStatusItems.StoredCharge, null);
			this.notoperational.DefaultState(this.notoperational.normal).PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.notoperational.normal.EventTransition(GameHashes.OperationalFlagChanged, this.notoperational.notube, (TravelTubeEntrance.SMInstance smi) => !smi.master.operational.GetFlag(TravelTubeEntrance.tubeConnected));
			this.notoperational.notube.EventTransition(GameHashes.OperationalFlagChanged, this.notoperational.normal, (TravelTubeEntrance.SMInstance smi) => smi.master.operational.GetFlag(TravelTubeEntrance.tubeConnected)).ToggleStatusItem(Db.Get().BuildingStatusItems.NoTubeConnected, null);
			this.notready.PlayAnim("off").ParamTransition<bool>(this.hasLaunchCharges, this.ready, (TravelTubeEntrance.SMInstance smi, bool hasLaunchCharges) => hasLaunchCharges).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((TravelTubeEntrance.SMInstance smi) => new TravelTubeEntrance.LaunchReactable(smi.master.GetComponent<TravelTubeEntrance.Work>(), smi.master.GetComponent<TravelTubeEntrance>())).ParamTransition<bool>(this.hasLaunchCharges, this.notready, (TravelTubeEntrance.SMInstance smi, bool hasLaunchCharges) => !hasLaunchCharges).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((TravelTubeEntrance.SMInstance smi) => smi.GetComponent<TravelTubeEntrance.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim(new Func<TravelTubeEntrance.SMInstance, string>(TravelTubeEntrance.GetLaunchAnimName), KAnim.PlayMode.Once).QueueAnim("working_loop", true, null).Enter(new StateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State.Callback(TravelTubeEntrance.SetTravelerGleamEffect)).WorkableStopTransition((TravelTubeEntrance.SMInstance smi) => smi.GetComponent<TravelTubeEntrance.Work>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x04006F08 RID: 28424
		public StateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.BoolParameter hasLaunchCharges;

		// Token: 0x04006F09 RID: 28425
		public TravelTubeEntrance.States.NotOperationalStates notoperational;

		// Token: 0x04006F0A RID: 28426
		public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State notready;

		// Token: 0x04006F0B RID: 28427
		public TravelTubeEntrance.States.ReadyStates ready;

		// Token: 0x0200254E RID: 9550
		public class NotOperationalStates : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State
		{
			// Token: 0x0400A63E RID: 42558
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State normal;

			// Token: 0x0400A63F RID: 42559
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State notube;
		}

		// Token: 0x0200254F RID: 9551
		public class ReadyStates : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State
		{
			// Token: 0x0400A640 RID: 42560
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State free;

			// Token: 0x0400A641 RID: 42561
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State occupied;

			// Token: 0x0400A642 RID: 42562
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State post;
		}
	}

	// Token: 0x0200163B RID: 5691
	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		// Token: 0x06009186 RID: 37254 RVA: 0x00350AB6 File Offset: 0x0034ECB6
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.showProgressBar = false;
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_tube_launcher_kanim")
			};
			this.workLayer = Grid.SceneLayer.BuildingUse;
		}

		// Token: 0x06009187 RID: 37255 RVA: 0x00350AF2 File Offset: 0x0034ECF2
		protected override void OnStartWork(WorkerBase worker)
		{
			base.SetWorkTime(1f);
		}

		// Token: 0x04006F0C RID: 28428
		public const string DEFAULT_LAUNCH_ANIM_NAME = "anim_interacts_tube_launcher_kanim";
	}
}
