using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006E0 RID: 1760
public class GeothermalController : StateMachineComponent<GeothermalController.StatesInstance>
{
	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06002CA8 RID: 11432 RVA: 0x000FADBD File Offset: 0x000F8FBD
	// (set) Token: 0x06002CA9 RID: 11433 RVA: 0x000FADC5 File Offset: 0x000F8FC5
	public GeothermalController.ProgressState State
	{
		get
		{
			return this.state;
		}
		protected set
		{
			this.state = value;
		}
	}

	// Token: 0x06002CAA RID: 11434 RVA: 0x000FADD0 File Offset: 0x000F8FD0
	public List<GeothermalVent> FindVents(bool requireEnabled)
	{
		if (!requireEnabled)
		{
			return Components.GeothermalVents.GetItems(base.gameObject.GetMyWorldId());
		}
		List<GeothermalVent> list = new List<GeothermalVent>();
		foreach (GeothermalVent geothermalVent in this.FindVents(false))
		{
			if (geothermalVent.IsVentConnected())
			{
				list.Add(geothermalVent);
			}
		}
		return list;
	}

	// Token: 0x06002CAB RID: 11435 RVA: 0x000FAE4C File Offset: 0x000F904C
	public void PushToVents(GeothermalVent.ElementInfo info)
	{
		List<GeothermalVent> list = this.FindVents(true);
		if (list.Count == 0)
		{
			return;
		}
		float[] array = new float[list.Count];
		float num = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			array[i] = GeothermalControllerConfig.OUTPUT_VENT_WEIGHT_RANGE.Get();
			num += array[i];
		}
		GeothermalVent.ElementInfo info2 = info;
		for (int j = 0; j < list.Count; j++)
		{
			info2.mass = array[j] * info.mass / num;
			info2.diseaseCount = (int)(array[j] * (float)info.diseaseCount / num);
			list[j].addMaterial(info2);
		}
	}

	// Token: 0x06002CAC RID: 11436 RVA: 0x000FAEF5 File Offset: 0x000F90F5
	public bool IsFull()
	{
		return this.storage.MassStored() > 11999.9f;
	}

	// Token: 0x06002CAD RID: 11437 RVA: 0x000FAF0C File Offset: 0x000F910C
	public float ComputeContentTemperature()
	{
		float num = 0f;
		float num2 = 0f;
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			float num3 = component.Mass * component.Element.specificHeatCapacity;
			num += num3 * component.Temperature;
			num2 += num3;
		}
		float result = 0f;
		if (num2 != 0f)
		{
			result = num / num2;
		}
		return result;
	}

	// Token: 0x06002CAE RID: 11438 RVA: 0x000FAFAC File Offset: 0x000F91AC
	public List<GeothermalVent.ElementInfo> ComputeOutputs()
	{
		float num = this.ComputeContentTemperature();
		float temperature = GeothermalControllerConfig.CalculateOutputTemperature(num);
		GeothermalController.ImpuritiesHelper impuritiesHelper = new GeothermalController.ImpuritiesHelper();
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			impuritiesHelper.AddMaterial(component.Element.idx, component.Mass * 0.92f, temperature, component.DiseaseIdx, component.DiseaseCount);
		}
		foreach (GeothermalControllerConfig.Impurity impurity in GeothermalControllerConfig.GetImpurities())
		{
			MathUtil.MinMax required_temp_range = impurity.required_temp_range;
			if (required_temp_range.Contains(num))
			{
				impuritiesHelper.AddMaterial(impurity.elementIdx, impurity.mass_kg, temperature, byte.MaxValue, 0);
			}
		}
		return impuritiesHelper.results;
	}

	// Token: 0x06002CAF RID: 11439 RVA: 0x000FB0B8 File Offset: 0x000F92B8
	public void PushToVents()
	{
		SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerHasVented = true;
		List<GeothermalVent.ElementInfo> list = this.ComputeOutputs();
		if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent && list[0].temperature >= 602f)
		{
			GeothermalPlantComponent.OnVentingHotMaterial(this.GetMyWorldId());
		}
		foreach (GeothermalVent.ElementInfo info in list)
		{
			this.PushToVents(info);
		}
		this.storage.ConsumeAllIgnoringDisease();
		this.fakeProgress = 1f;
	}

	// Token: 0x06002CB0 RID: 11440 RVA: 0x000FB164 File Offset: 0x000F9364
	private void TryAddConduitConsumers()
	{
		if (base.GetComponents<EntityConduitConsumer>().Length != 0)
		{
			return;
		}
		foreach (CellOffset offset in new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(2, 0),
			new CellOffset(-2, 0)
		})
		{
			EntityConduitConsumer entityConduitConsumer = base.gameObject.AddComponent<EntityConduitConsumer>();
			entityConduitConsumer.offset = offset;
			entityConduitConsumer.conduitType = ConduitType.Liquid;
		}
	}

	// Token: 0x06002CB1 RID: 11441 RVA: 0x000FB1DC File Offset: 0x000F93DC
	public float GetPressure()
	{
		GeothermalController.ProgressState progressState = this.state;
		if (progressState > GeothermalController.ProgressState.RECONNECTING_PIPES)
		{
			if (progressState - GeothermalController.ProgressState.NOTIFY_REPAIRED > 3)
			{
			}
			return this.storage.MassStored() / 12000f;
		}
		return 0f;
	}

	// Token: 0x06002CB2 RID: 11442 RVA: 0x000FB213 File Offset: 0x000F9413
	private void FakeMeterDraining(float time)
	{
		this.fakeProgress -= time / 16f;
		if (this.fakeProgress < 0f)
		{
			this.fakeProgress = 0f;
		}
		this.barometer.SetPositionPercent(this.fakeProgress);
	}

	// Token: 0x06002CB3 RID: 11443 RVA: 0x000FB254 File Offset: 0x000F9454
	private void UpdatePressure()
	{
		GeothermalController.ProgressState progressState = this.state;
		if (progressState > GeothermalController.ProgressState.RECONNECTING_PIPES)
		{
			if (progressState - GeothermalController.ProgressState.NOTIFY_REPAIRED > 3)
			{
			}
			float pressure = this.GetPressure();
			this.barometer.SetPositionPercent(pressure);
			float num = this.ComputeContentTemperature();
			if (num > 0f)
			{
				this.thermometer.SetPositionPercent((num - 50f) / 2450f);
			}
			int num2 = 0;
			for (int i = 1; i < GeothermalControllerConfig.PRESSURE_ANIM_THRESHOLDS.Length; i++)
			{
				if (pressure >= GeothermalControllerConfig.PRESSURE_ANIM_THRESHOLDS[i])
				{
					num2 = i;
				}
			}
			KAnim.Anim currentAnim = this.animController.GetCurrentAnim();
			if (((currentAnim != null) ? currentAnim.name : null) != GeothermalControllerConfig.PRESSURE_ANIM_LOOPS[num2])
			{
				this.animController.Play(GeothermalControllerConfig.PRESSURE_ANIM_LOOPS[num2], KAnim.PlayMode.Loop, 1f, 0f);
			}
			return;
		}
	}

	// Token: 0x06002CB4 RID: 11444 RVA: 0x000FB324 File Offset: 0x000F9524
	public bool IsObstructed()
	{
		if (this.IsFull())
		{
			bool flag = false;
			foreach (GeothermalVent geothermalVent in this.FindVents(false))
			{
				if (geothermalVent.IsEntombed())
				{
					return true;
				}
				if (geothermalVent.IsVentConnected())
				{
					if (!geothermalVent.CanVent())
					{
						return true;
					}
					flag = true;
				}
			}
			return !flag;
		}
		return false;
	}

	// Token: 0x06002CB5 RID: 11445 RVA: 0x000FB3A8 File Offset: 0x000F95A8
	public GeothermalVent FirstObstructedVent()
	{
		foreach (GeothermalVent geothermalVent in this.FindVents(false))
		{
			if (geothermalVent.IsEntombed())
			{
				return geothermalVent;
			}
			if (geothermalVent.IsVentConnected() && !geothermalVent.CanVent())
			{
				return geothermalVent;
			}
		}
		return null;
	}

	// Token: 0x06002CB6 RID: 11446 RVA: 0x000FB418 File Offset: 0x000F9618
	public Notification CreateFirstBatchReadyNotification()
	{
		this.dismissOnSelect = new Notification(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_FIRST_VENT_READY, NotificationType.Event, (List<Notification> _, object __) => COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_FIRST_VENT_READY_TOOLTIP, null, false, 0f, null, null, base.transform, true, false, false);
		return this.dismissOnSelect;
	}

	// Token: 0x06002CB7 RID: 11447 RVA: 0x000FB474 File Offset: 0x000F9674
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.GeothermalControllers.Add(this.GetMyWorldId(), this);
		this.operational.SetFlag(GeothermalController.allowInputFlag, false);
		base.smi.StartSM();
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.barometer = new MeterController(this.animController, "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalControllerConfig.BAROMETER_SYMBOLS);
		this.thermometer = new MeterController(this.animController, "meter_target", "meter_temp", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalControllerConfig.THERMOMETER_SYMBOLS);
		base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelected));
	}

	// Token: 0x06002CB8 RID: 11448 RVA: 0x000FB520 File Offset: 0x000F9720
	protected override void OnCleanUp()
	{
		base.Unsubscribe(-1503271301, new Action<object>(this.OnBuildingSelected));
		if (this.listener != null)
		{
			Components.GeothermalVents.Unregister(this.GetMyWorldId(), this.listener.onAdd, this.listener.onRemove);
		}
		Components.GeothermalControllers.Remove(this.GetMyWorldId(), this);
		base.OnCleanUp();
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x000FB58C File Offset: 0x000F978C
	protected void OnBuildingSelected(object clicked)
	{
		if (!(bool)clicked)
		{
			return;
		}
		if (this.dismissOnSelect != null)
		{
			if (this.dismissOnSelect.customClickCallback != null)
			{
				this.dismissOnSelect.customClickCallback(this.dismissOnSelect.customClickData);
				return;
			}
			this.dismissOnSelect.Clear();
			this.dismissOnSelect = null;
		}
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x000FB5E8 File Offset: 0x000F97E8
	public bool VentingCanFreeKeepsake()
	{
		List<GeothermalVent.ElementInfo> list = this.ComputeOutputs();
		return list.Count != 0 && list[0].temperature >= 602f;
	}

	// Token: 0x040019C6 RID: 6598
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040019C7 RID: 6599
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040019C8 RID: 6600
	private MeterController thermometer;

	// Token: 0x040019C9 RID: 6601
	private MeterController barometer;

	// Token: 0x040019CA RID: 6602
	private KBatchedAnimController animController;

	// Token: 0x040019CB RID: 6603
	public Notification dismissOnSelect;

	// Token: 0x040019CC RID: 6604
	public static Operational.Flag allowInputFlag = new Operational.Flag("allowInputFlag", Operational.Flag.Type.Requirement);

	// Token: 0x040019CD RID: 6605
	private GeothermalController.VentRegistrationListener listener;

	// Token: 0x040019CE RID: 6606
	[Serialize]
	private GeothermalController.ProgressState state;

	// Token: 0x040019CF RID: 6607
	private float fakeProgress;

	// Token: 0x020014F5 RID: 5365
	public class ReconnectPipes : Workable
	{
		// Token: 0x06008CBB RID: 36027 RVA: 0x0033B71D File Offset: 0x0033991D
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.SetWorkTime(5f);
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(GeothermalControllerConfig.RECONNECT_PUMP_ANIM_OVERRIDE)
			};
			this.synchronizeAnims = false;
			this.faceTargetWhenWorking = true;
		}

		// Token: 0x06008CBC RID: 36028 RVA: 0x0033B757 File Offset: 0x00339957
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
			if (this.storage != null)
			{
				this.storage.ConsumeAllIgnoringDisease();
			}
		}

		// Token: 0x04006B92 RID: 27538
		[MyCmpGet]
		private Storage storage;
	}

	// Token: 0x020014F6 RID: 5366
	private class VentRegistrationListener
	{
		// Token: 0x04006B93 RID: 27539
		public Action<GeothermalVent> onAdd;

		// Token: 0x04006B94 RID: 27540
		public Action<GeothermalVent> onRemove;
	}

	// Token: 0x020014F7 RID: 5367
	public enum ProgressState
	{
		// Token: 0x04006B96 RID: 27542
		NOT_STARTED,
		// Token: 0x04006B97 RID: 27543
		FETCHING_STEEL,
		// Token: 0x04006B98 RID: 27544
		RECONNECTING_PIPES,
		// Token: 0x04006B99 RID: 27545
		NOTIFY_REPAIRED,
		// Token: 0x04006B9A RID: 27546
		REPAIRED,
		// Token: 0x04006B9B RID: 27547
		AT_CAPACITY,
		// Token: 0x04006B9C RID: 27548
		COMPLETE
	}

	// Token: 0x020014F8 RID: 5368
	private class ImpuritiesHelper
	{
		// Token: 0x06008CBF RID: 36031 RVA: 0x0033B78C File Offset: 0x0033998C
		public void AddMaterial(ushort elementIdx, float mass, float temperature, byte diseaseIdx, int diseaseCount)
		{
			Element element = ElementLoader.elements[(int)elementIdx];
			if (element.lowTemp > temperature)
			{
				Element lowTempTransition = element.lowTempTransition;
				Element element2 = ElementLoader.FindElementByHash(element.lowTempTransitionOreID);
				this.AddMaterial(lowTempTransition.idx, mass * (1f - element.lowTempTransitionOreMassConversion), temperature, diseaseIdx, (int)((float)diseaseCount * (1f - element.lowTempTransitionOreMassConversion)));
				if (element2 != null)
				{
					this.AddMaterial(element2.idx, mass * element.lowTempTransitionOreMassConversion, temperature, diseaseIdx, (int)((float)diseaseCount * element.lowTempTransitionOreMassConversion));
				}
				return;
			}
			if (element.highTemp < temperature)
			{
				Element highTempTransition = element.highTempTransition;
				Element element3 = ElementLoader.FindElementByHash(element.highTempTransitionOreID);
				this.AddMaterial(highTempTransition.idx, mass * (1f - element.highTempTransitionOreMassConversion), temperature, diseaseIdx, (int)((float)diseaseCount * (1f - element.highTempTransitionOreMassConversion)));
				if (element3 != null)
				{
					this.AddMaterial(element3.idx, mass * element.highTempTransitionOreMassConversion, temperature, diseaseIdx, (int)((float)diseaseCount * element.highTempTransitionOreMassConversion));
				}
				return;
			}
			GeothermalVent.ElementInfo elementInfo = default(GeothermalVent.ElementInfo);
			for (int i = 0; i < this.results.Count; i++)
			{
				if (this.results[i].elementIdx == elementIdx)
				{
					elementInfo = this.results[i];
					elementInfo.mass += mass;
					this.results[i] = elementInfo;
					return;
				}
			}
			elementInfo.elementIdx = elementIdx;
			elementInfo.mass = mass;
			elementInfo.temperature = temperature;
			elementInfo.diseaseCount = diseaseCount;
			elementInfo.diseaseIdx = diseaseIdx;
			elementInfo.isSolid = ElementLoader.elements[(int)elementIdx].IsSolid;
			this.results.Add(elementInfo);
		}

		// Token: 0x04006B9D RID: 27549
		public List<GeothermalVent.ElementInfo> results = new List<GeothermalVent.ElementInfo>();
	}

	// Token: 0x020014F9 RID: 5369
	public class States : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController>
	{
		// Token: 0x06008CC1 RID: 36033 RVA: 0x0033B94C File Offset: 0x00339B4C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EnterTransition(this.online, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.COMPLETE).EnterTransition(this.offline, (GeothermalController.StatesInstance smi) => smi.master.State != GeothermalController.ProgressState.COMPLETE);
			this.offline.EnterTransition(this.offline.initial, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.NOT_STARTED).EnterTransition(this.offline.fetchSteel, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.FETCHING_STEEL).EnterTransition(this.offline.reconnectPipes, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.RECONNECTING_PIPES).EnterTransition(this.offline.notifyRepaired, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.NOTIFY_REPAIRED).EnterTransition(this.offline.filling, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.REPAIRED).EnterTransition(this.offline.filled, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.AT_CAPACITY).PlayAnim("off");
			this.offline.initial.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.storage.DropAll(false, false, default(Vector3), true, null);
			}).Transition(this.offline.fetchSteel, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.FETCHING_STEEL, UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null);
			this.offline.fetchSteel.ToggleChore((GeothermalController.StatesInstance smi) => this.CreateRepairFetchChore(smi, GeothermalControllerConfig.STEEL_FETCH_TAGS, 1200f - smi.master.storage.MassStored()), this.offline.checkSupplies).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForMaterials, (GeothermalController.StatesInstance smi) => smi.GetFetchListForStatusItem());
			this.offline.checkSupplies.EnterTransition(this.offline.fetchSteel, (GeothermalController.StatesInstance smi) => smi.master.storage.MassStored() < 1200f).EnterTransition(this.offline.reconnectPipes, (GeothermalController.StatesInstance smi) => smi.master.storage.MassStored() >= 1200f).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null);
			this.offline.reconnectPipes.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.state = GeothermalController.ProgressState.RECONNECTING_PIPES;
			}).ToggleChore((GeothermalController.StatesInstance smi) => this.CreateRepairChore(smi), this.offline.notifyRepaired, this.offline.reconnectPipes).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoQuestPendingReconnectPipes, null);
			this.offline.notifyRepaired.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.state = GeothermalController.ProgressState.NOTIFY_REPAIRED;
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null).ToggleNotification((GeothermalController.StatesInstance smi) => this.CreateRepairedNotification(smi)).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null);
			this.offline.repaired.Exit(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.State = GeothermalController.ProgressState.REPAIRED;
			}).PlayAnim("on_pre").OnAnimQueueComplete(this.offline.filling).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.offline.filling.PlayAnim("on").Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.TryAddConduitConsumers();
			}).ToggleOperationalFlag(GeothermalController.allowInputFlag).Transition(this.offline.filled, (GeothermalController.StatesInstance smi) => smi.master.IsFull(), UpdateRate.SIM_200ms).Update(delegate(GeothermalController.StatesInstance smi, float _)
			{
				smi.master.UpdatePressure();
			}, UpdateRate.SIM_1000ms, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.offline.filled.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.state = GeothermalController.ProgressState.AT_CAPACITY;
				smi.master.TryAddConduitConsumers();
			}).ToggleNotification((GeothermalController.StatesInstance smi) => smi.master.CreateFirstBatchReadyNotification()).EnterTransition(this.offline.filled.ready, (GeothermalController.StatesInstance smi) => !smi.master.IsObstructed()).EnterTransition(this.offline.filled.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed()).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null);
			this.offline.filled.ready.PlayAnim("on").Transition(this.offline.filled.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.offline.filled.obstructed.Transition(this.offline.filled.ready, (GeothermalController.StatesInstance smi) => !smi.master.IsObstructed(), UpdateRate.SIM_200ms).PlayAnim("on").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerCantVent, (GeothermalController.StatesInstance smi) => smi.master);
			this.online.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.TryAddConduitConsumers();
			}).defaultState = this.online.active;
			this.online.active.PlayAnim("on").Transition(this.online.venting, (GeothermalController.StatesInstance smi) => smi.master.IsFull() && !smi.master.IsObstructed(), UpdateRate.SIM_1000ms).Transition(this.online.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_1000ms).Update(delegate(GeothermalController.StatesInstance smi, float _)
			{
				smi.master.UpdatePressure();
			}, UpdateRate.SIM_1000ms, false).ToggleOperationalFlag(GeothermalController.allowInputFlag).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.online.venting.Transition(this.online.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_200ms).Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.PushToVents();
			}).PlayAnim("venting_loop", KAnim.PlayMode.Loop).Update(delegate(GeothermalController.StatesInstance smi, float f)
			{
				smi.master.FakeMeterDraining(f);
			}, UpdateRate.SIM_1000ms, false).ScheduleGoTo(16f, this.online.active).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.online.obstructed.Transition(this.online.active, (GeothermalController.StatesInstance smi) => !smi.master.IsObstructed(), UpdateRate.SIM_1000ms).PlayAnim("on").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerCantVent, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null);
		}

		// Token: 0x06008CC2 RID: 36034 RVA: 0x0033C4A0 File Offset: 0x0033A6A0
		protected Chore CreateRepairFetchChore(GeothermalController.StatesInstance smi, HashSet<Tag> tags, float mass_required)
		{
			return new FetchChore(Db.Get().ChoreTypes.RepairFetch, smi.master.storage, mass_required, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.None, 0);
		}

		// Token: 0x06008CC3 RID: 36035 RVA: 0x0033C4DC File Offset: 0x0033A6DC
		protected Chore CreateRepairChore(GeothermalController.StatesInstance smi)
		{
			return new WorkChore<GeothermalController.ReconnectPipes>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
		}

		// Token: 0x06008CC4 RID: 36036 RVA: 0x0033C514 File Offset: 0x0033A714
		protected Notification CreateRepairedNotification(GeothermalController.StatesInstance smi)
		{
			smi.master.dismissOnSelect = new Notification(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_RECONNECTED, NotificationType.Event, (List<Notification> _, object __) => COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_RECONNECTED_TOOLTIP, null, false, 0f, delegate(object _)
			{
				smi.master.dismissOnSelect = null;
				this.SetProgressionToRepaired(smi);
			}, null, null, true, true, false);
			return smi.master.dismissOnSelect;
		}

		// Token: 0x06008CC5 RID: 36037 RVA: 0x0033C59C File Offset: 0x0033A79C
		protected void SetProgressionToRepaired(GeothermalController.StatesInstance smi)
		{
			SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired = true;
			GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_PLANT_REPAIRED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_PLANT_REPAIRED_DESC, "geothermalplantonline_kanim", delegate
			{
				smi.GoTo(this.offline.repaired);
				SelectTool.Instance.Select(smi.master.GetComponent<KSelectable>(), true);
			}, smi.master.transform);
		}

		// Token: 0x04006B9E RID: 27550
		public GeothermalController.States.OfflineStates offline;

		// Token: 0x04006B9F RID: 27551
		public GeothermalController.States.OnlineStates online;

		// Token: 0x020024DD RID: 9437
		public class OfflineStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
		{
			// Token: 0x0400A3BB RID: 41915
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State initial;

			// Token: 0x0400A3BC RID: 41916
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State fetchSteel;

			// Token: 0x0400A3BD RID: 41917
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State checkSupplies;

			// Token: 0x0400A3BE RID: 41918
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State reconnectPipes;

			// Token: 0x0400A3BF RID: 41919
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State notifyRepaired;

			// Token: 0x0400A3C0 RID: 41920
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State repaired;

			// Token: 0x0400A3C1 RID: 41921
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State filling;

			// Token: 0x0400A3C2 RID: 41922
			public GeothermalController.States.OfflineStates.FilledStates filled;

			// Token: 0x02003534 RID: 13620
			public class FilledStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
			{
				// Token: 0x0400D7A5 RID: 55205
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State ready;

				// Token: 0x0400D7A6 RID: 55206
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State obstructed;
			}
		}

		// Token: 0x020024DE RID: 9438
		public class OnlineStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
		{
			// Token: 0x0400A3C3 RID: 41923
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State active;

			// Token: 0x0400A3C4 RID: 41924
			public GeothermalController.States.OnlineStates.WorkingStates venting;

			// Token: 0x0400A3C5 RID: 41925
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State obstructed;

			// Token: 0x02003535 RID: 13621
			public class WorkingStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
			{
				// Token: 0x0400D7A7 RID: 55207
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State pre;

				// Token: 0x0400D7A8 RID: 55208
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State loop;

				// Token: 0x0400D7A9 RID: 55209
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State post;
			}
		}
	}

	// Token: 0x020014FA RID: 5370
	public class StatesInstance : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x06008CCA RID: 36042 RVA: 0x0033C64A File Offset: 0x0033A84A
		public StatesInstance(GeothermalController smi) : base(smi)
		{
		}

		// Token: 0x06008CCB RID: 36043 RVA: 0x0033C654 File Offset: 0x0033A854
		public IFetchList GetFetchListForStatusItem()
		{
			GeothermalController.StatesInstance.FakeList fakeList = new GeothermalController.StatesInstance.FakeList();
			float value = 1200f - base.smi.master.storage.MassStored();
			fakeList.remaining[GameTagExtensions.Create(SimHashes.Steel)] = value;
			return fakeList;
		}

		// Token: 0x06008CCC RID: 36044 RVA: 0x0033C698 File Offset: 0x0033A898
		bool ISidescreenButtonControl.SidescreenButtonInteractable()
		{
			switch (base.smi.master.State)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				return true;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
				return false;
			case GeothermalController.ProgressState.AT_CAPACITY:
				return !base.smi.master.IsObstructed();
			case GeothermalController.ProgressState.COMPLETE:
				return false;
			default:
				return false;
			}
		}

		// Token: 0x06008CCD RID: 36045 RVA: 0x0033C6F5 File Offset: 0x0033A8F5
		bool ISidescreenButtonControl.SidescreenEnabled()
		{
			return base.smi.master.State != GeothermalController.ProgressState.COMPLETE;
		}

		// Token: 0x06008CCE RID: 36046 RVA: 0x0033C710 File Offset: 0x0033A910
		private string getSidescreenButtonText()
		{
			switch (base.smi.master.State)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.REPAIR_CONTROLLER_TITLE;
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.CANCEL_REPAIR_CONTROLLER_TITLE;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
			case GeothermalController.ProgressState.AT_CAPACITY:
			case GeothermalController.ProgressState.COMPLETE:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_TITLE;
			default:
				return "";
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06008CCF RID: 36047 RVA: 0x0033C778 File Offset: 0x0033A978
		string ISidescreenButtonControl.SidescreenButtonText
		{
			get
			{
				return this.getSidescreenButtonText();
			}
		}

		// Token: 0x06008CD0 RID: 36048 RVA: 0x0033C780 File Offset: 0x0033A980
		private string getSidescreenButtonTooltip()
		{
			switch (base.smi.master.State)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.REPAIR_CONTROLLER_TOOLTIP;
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.CANCEL_REPAIR_CONTROLLER_TOOLTIP;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_FILLING_TOOLTIP;
			case GeothermalController.ProgressState.AT_CAPACITY:
			case GeothermalController.ProgressState.COMPLETE:
				if (base.smi.master.IsObstructed())
				{
					return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_UNAVAILABLE_TOOLTIP;
				}
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_READY_TOOLTIP;
			default:
				return "";
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06008CD1 RID: 36049 RVA: 0x0033C810 File Offset: 0x0033AA10
		string ISidescreenButtonControl.SidescreenButtonTooltip
		{
			get
			{
				return this.getSidescreenButtonTooltip();
			}
		}

		// Token: 0x06008CD2 RID: 36050 RVA: 0x0033C818 File Offset: 0x0033AA18
		void ISidescreenButtonControl.OnSidescreenButtonPressed()
		{
			switch (base.smi.master.state)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
				base.smi.master.State = GeothermalController.ProgressState.FETCHING_STEEL;
				return;
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				base.smi.master.State = GeothermalController.ProgressState.NOT_STARTED;
				base.smi.GoTo(base.sm.offline.initial);
				return;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
			case GeothermalController.ProgressState.COMPLETE:
				break;
			case GeothermalController.ProgressState.AT_CAPACITY:
			{
				MusicManager.instance.PlaySong("Music_Imperative_complete_DLC2", false);
				bool flag = base.smi.master.VentingCanFreeKeepsake();
				base.smi.master.state = GeothermalController.ProgressState.COMPLETE;
				base.smi.GoTo(base.sm.online.venting);
				if (!flag)
				{
					GeothermalFirstEmissionSequence.Start(base.smi.master);
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06008CD3 RID: 36051 RVA: 0x0033C8F6 File Offset: 0x0033AAF6
		void ISidescreenButtonControl.SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06008CD4 RID: 36052 RVA: 0x0033C8FD File Offset: 0x0033AAFD
		int ISidescreenButtonControl.HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x06008CD5 RID: 36053 RVA: 0x0033C900 File Offset: 0x0033AB00
		int ISidescreenButtonControl.ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x020024E2 RID: 9442
		protected class FakeList : IFetchList
		{
			// Token: 0x17000C18 RID: 3096
			// (get) Token: 0x0600BC09 RID: 48137 RVA: 0x003D5AC2 File Offset: 0x003D3CC2
			Storage IFetchList.Destination
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x0600BC0A RID: 48138 RVA: 0x003D5AC9 File Offset: 0x003D3CC9
			float IFetchList.GetMinimumAmount(Tag tag)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600BC0B RID: 48139 RVA: 0x003D5AD0 File Offset: 0x003D3CD0
			Dictionary<Tag, float> IFetchList.GetRemaining()
			{
				return this.remaining;
			}

			// Token: 0x0600BC0C RID: 48140 RVA: 0x003D5AD8 File Offset: 0x003D3CD8
			Dictionary<Tag, float> IFetchList.GetRemainingMinimum()
			{
				throw new NotImplementedException();
			}

			// Token: 0x0400A3FD RID: 41981
			public Dictionary<Tag, float> remaining = new Dictionary<Tag, float>();
		}
	}
}
