using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000757 RID: 1879
public class Reactor : StateMachineComponent<Reactor.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x1700034B RID: 843
	// (get) Token: 0x0600323F RID: 12863 RVA: 0x00113E8E File Offset: 0x0011208E
	// (set) Token: 0x06003240 RID: 12864 RVA: 0x00113E96 File Offset: 0x00112096
	private float ReactionMassTarget
	{
		get
		{
			return this.reactionMassTarget;
		}
		set
		{
			this.fuelDelivery.capacity = value * 2f;
			this.fuelDelivery.refillMass = value * 0.2f;
			this.fuelDelivery.MinimumMass = value * 0.2f;
			this.reactionMassTarget = value;
		}
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06003241 RID: 12865 RVA: 0x00113ED5 File Offset: 0x001120D5
	public float FuelTemperature
	{
		get
		{
			if (this.reactionStorage.items.Count > 0)
			{
				return this.reactionStorage.items[0].GetComponent<PrimaryElement>().Temperature;
			}
			return -1f;
		}
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06003242 RID: 12866 RVA: 0x00113F0C File Offset: 0x0011210C
	public float ReserveCoolantMass
	{
		get
		{
			PrimaryElement storedCoolant = this.GetStoredCoolant();
			if (!(storedCoolant == null))
			{
				return storedCoolant.Mass;
			}
			return 0f;
		}
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x06003243 RID: 12867 RVA: 0x00113F35 File Offset: 0x00112135
	public bool On
	{
		get
		{
			return base.smi.IsInsideState(base.smi.sm.on);
		}
	}

	// Token: 0x06003244 RID: 12868 RVA: 0x00113F54 File Offset: 0x00112154
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.NuclearReactors.Add(this);
		Storage[] components = base.GetComponents<Storage>();
		this.supplyStorage = components[0];
		this.reactionStorage = components[1];
		this.wasteStorage = components[2];
		this.CreateMeters();
		base.smi.StartSM();
		this.fuelDelivery = base.GetComponent<ManualDeliveryKG>();
		this.CheckLogicInputValueChanged(true);
	}

	// Token: 0x06003245 RID: 12869 RVA: 0x00113FB8 File Offset: 0x001121B8
	protected override void OnCleanUp()
	{
		Components.NuclearReactors.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x00113FCB File Offset: 0x001121CB
	private void Update()
	{
		this.CheckLogicInputValueChanged(false);
	}

	// Token: 0x06003247 RID: 12871 RVA: 0x00113FD4 File Offset: 0x001121D4
	public Notification CreateMeltdownNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(MISC.NOTIFICATIONS.REACTORMELTDOWN.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.REACTORMELTDOWN.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06003248 RID: 12872 RVA: 0x00114033 File Offset: 0x00112233
	public void SetStorages(Storage supply, Storage reaction, Storage waste)
	{
		this.supplyStorage = supply;
		this.reactionStorage = reaction;
		this.wasteStorage = waste;
	}

	// Token: 0x06003249 RID: 12873 RVA: 0x0011404C File Offset: 0x0011224C
	private void CheckLogicInputValueChanged(bool onLoad = false)
	{
		int num = 1;
		if (this.logicPorts.IsPortConnected("CONTROL_FUEL_DELIVERY"))
		{
			num = this.logicPorts.GetInputValue("CONTROL_FUEL_DELIVERY");
		}
		if (num == 0 && (this.fuelDeliveryEnabled || onLoad))
		{
			this.fuelDelivery.refillMass = -1f;
			this.fuelDeliveryEnabled = false;
			this.fuelDelivery.AbortDelivery("AutomationDisabled");
			return;
		}
		if (num == 1 && (!this.fuelDeliveryEnabled || onLoad))
		{
			this.fuelDelivery.refillMass = this.reactionMassTarget * 0.2f;
			this.fuelDeliveryEnabled = true;
		}
	}

	// Token: 0x0600324A RID: 12874 RVA: 0x001140EC File Offset: 0x001122EC
	private void OnLogicConnectionChanged(int value, bool connection)
	{
	}

	// Token: 0x0600324B RID: 12875 RVA: 0x001140F0 File Offset: 0x001122F0
	private void CreateMeters()
	{
		this.temperatureMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "temperature_meter_target", "meter_temperature", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"temperature_meter_target"
		});
		this.waterMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "water_meter_target", "meter_water", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"water_meter_target"
		});
	}

	// Token: 0x0600324C RID: 12876 RVA: 0x00114158 File Offset: 0x00112358
	private void TransferFuel()
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		PrimaryElement storedFuel = this.GetStoredFuel();
		float num = (activeFuel != null) ? activeFuel.Mass : 0f;
		float num2 = (storedFuel != null) ? storedFuel.Mass : 0f;
		float num3 = this.ReactionMassTarget - num;
		num3 = Mathf.Min(num2, num3);
		if (num3 > 0.5f || num3 == num2)
		{
			this.supplyStorage.Transfer(this.reactionStorage, this.fuelTag, num3, false, true);
		}
	}

	// Token: 0x0600324D RID: 12877 RVA: 0x001141E0 File Offset: 0x001123E0
	private void TransferCoolant()
	{
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		PrimaryElement storedCoolant = this.GetStoredCoolant();
		float num = (activeCoolant != null) ? activeCoolant.Mass : 0f;
		float a = (storedCoolant != null) ? storedCoolant.Mass : 0f;
		float num2 = 30f - num;
		num2 = Mathf.Min(a, num2);
		if (num2 > 0f)
		{
			this.supplyStorage.Transfer(this.reactionStorage, this.coolantTag, num2, false, true);
		}
	}

	// Token: 0x0600324E RID: 12878 RVA: 0x0011425C File Offset: 0x0011245C
	private PrimaryElement GetStoredFuel()
	{
		GameObject gameObject = this.supplyStorage.FindFirst(this.fuelTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x0600324F RID: 12879 RVA: 0x00114298 File Offset: 0x00112498
	private PrimaryElement GetActiveFuel()
	{
		GameObject gameObject = this.reactionStorage.FindFirst(this.fuelTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06003250 RID: 12880 RVA: 0x001142D4 File Offset: 0x001124D4
	private PrimaryElement GetStoredCoolant()
	{
		GameObject gameObject = this.supplyStorage.FindFirst(this.coolantTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06003251 RID: 12881 RVA: 0x00114310 File Offset: 0x00112510
	private PrimaryElement GetActiveCoolant()
	{
		GameObject gameObject = this.reactionStorage.FindFirst(this.coolantTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06003252 RID: 12882 RVA: 0x0011434C File Offset: 0x0011254C
	private bool CanStartReaction()
	{
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		PrimaryElement activeFuel = this.GetActiveFuel();
		return activeCoolant && activeFuel && activeCoolant.Mass >= 30f && activeFuel.Mass >= 0.5f;
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x00114394 File Offset: 0x00112594
	private void Cool(float dt)
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		if (activeFuel == null)
		{
			return;
		}
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		if (activeCoolant == null)
		{
			return;
		}
		GameUtil.ForceConduction(activeFuel, activeCoolant, dt * 5f);
		if (activeCoolant.Temperature > 673.15f)
		{
			base.smi.sm.doVent.Trigger(base.smi);
		}
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x001143FC File Offset: 0x001125FC
	private void React(float dt)
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		if (activeFuel != null && activeFuel.Mass >= 0.25f)
		{
			float num = GameUtil.EnergyToTemperatureDelta(-100f * dt * activeFuel.Mass, activeFuel);
			activeFuel.Temperature += num;
			this.spentFuel += dt * 0.016666668f;
		}
	}

	// Token: 0x06003255 RID: 12885 RVA: 0x0011445D File Offset: 0x0011265D
	private void SetEmitRads(float rads)
	{
		base.smi.master.radEmitter.emitRads = rads;
		base.smi.master.radEmitter.Refresh();
	}

	// Token: 0x06003256 RID: 12886 RVA: 0x0011448C File Offset: 0x0011268C
	private bool ReadyToCool()
	{
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		return activeCoolant != null && activeCoolant.Mass > 0f;
	}

	// Token: 0x06003257 RID: 12887 RVA: 0x001144B8 File Offset: 0x001126B8
	private void DumpSpentFuel()
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		if (activeFuel != null)
		{
			if (this.spentFuel <= 0f)
			{
				return;
			}
			float num = this.spentFuel * 100f;
			if (num > 0f)
			{
				this.wasteStorage.AddLiquid(SimHashes.NuclearWaste, num, activeFuel.Temperature, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id), Mathf.RoundToInt(num * 50f), false, true);
			}
			if (this.wasteStorage.MassStored() >= 100f)
			{
				this.wasteStorage.DropAll(true, true, default(Vector3), true, null);
			}
			if (this.spentFuel >= activeFuel.Mass)
			{
				Util.KDestroyGameObject(activeFuel.gameObject);
				this.spentFuel = 0f;
				return;
			}
			activeFuel.Mass -= this.spentFuel;
			this.spentFuel = 0f;
		}
	}

	// Token: 0x06003258 RID: 12888 RVA: 0x001145B4 File Offset: 0x001127B4
	private void UpdateVentStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.ClearToVent())
		{
			if (component.HasStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure))
			{
				base.smi.sm.canVent.Set(true, base.smi, false);
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, false);
				return;
			}
		}
		else if (!component.HasStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure))
		{
			base.smi.sm.canVent.Set(false, base.smi, false);
			component.AddStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, null);
		}
	}

	// Token: 0x06003259 RID: 12889 RVA: 0x0011466C File Offset: 0x0011286C
	private void UpdateCoolantStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.GetStoredCoolant() != null || base.smi.GetCurrentState() == base.smi.sm.meltdown || base.smi.GetCurrentState() == base.smi.sm.dead)
		{
			if (component.HasStatusItem(Db.Get().BuildingStatusItems.NoCoolant))
			{
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.NoCoolant, false);
				return;
			}
		}
		else if (!component.HasStatusItem(Db.Get().BuildingStatusItems.NoCoolant))
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.NoCoolant, null);
		}
	}

	// Token: 0x0600325A RID: 12890 RVA: 0x00114728 File Offset: 0x00112928
	private void InitVentCells()
	{
		if (this.ventCells == null)
		{
			this.ventCells = new int[]
			{
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.zero),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.left),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.right + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.left + Vector3.left),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.left),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.right + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.left + Vector3.left)
			};
		}
	}

	// Token: 0x0600325B RID: 12891 RVA: 0x00114994 File Offset: 0x00112B94
	public int GetVentCell()
	{
		this.InitVentCells();
		for (int i = 0; i < this.ventCells.Length; i++)
		{
			if (Grid.Mass[this.ventCells[i]] < 150f && !Grid.Solid[this.ventCells[i]])
			{
				return this.ventCells[i];
			}
		}
		return -1;
	}

	// Token: 0x0600325C RID: 12892 RVA: 0x001149F4 File Offset: 0x00112BF4
	private bool ClearToVent()
	{
		this.InitVentCells();
		for (int i = 0; i < this.ventCells.Length; i++)
		{
			if (Grid.Mass[this.ventCells[i]] < 150f && !Grid.Solid[this.ventCells[i]])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600325D RID: 12893 RVA: 0x00114A4A File Offset: 0x00112C4A
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x04001DB8 RID: 7608
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001DB9 RID: 7609
	[MyCmpGet]
	private RadiationEmitter radEmitter;

	// Token: 0x04001DBA RID: 7610
	[MyCmpGet]
	private ManualDeliveryKG fuelDelivery;

	// Token: 0x04001DBB RID: 7611
	private MeterController temperatureMeter;

	// Token: 0x04001DBC RID: 7612
	private MeterController waterMeter;

	// Token: 0x04001DBD RID: 7613
	private Storage supplyStorage;

	// Token: 0x04001DBE RID: 7614
	private Storage reactionStorage;

	// Token: 0x04001DBF RID: 7615
	private Storage wasteStorage;

	// Token: 0x04001DC0 RID: 7616
	private Tag fuelTag = SimHashes.EnrichedUranium.CreateTag();

	// Token: 0x04001DC1 RID: 7617
	private Tag coolantTag = GameTags.AnyWater;

	// Token: 0x04001DC2 RID: 7618
	private Vector3 dumpOffset = new Vector3(0f, 5f, 0f);

	// Token: 0x04001DC3 RID: 7619
	public static string MELTDOWN_STINGER = "Stinger_Loop_NuclearMeltdown";

	// Token: 0x04001DC4 RID: 7620
	private static float meterFrameScaleHack = 3f;

	// Token: 0x04001DC5 RID: 7621
	[Serialize]
	private float spentFuel;

	// Token: 0x04001DC6 RID: 7622
	private float timeSinceMeltdownEmit;

	// Token: 0x04001DC7 RID: 7623
	private const float reactorMeltDownBonusMassAmount = 10f;

	// Token: 0x04001DC8 RID: 7624
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x04001DC9 RID: 7625
	private LogicEventHandler fuelControlPort;

	// Token: 0x04001DCA RID: 7626
	private bool fuelDeliveryEnabled = true;

	// Token: 0x04001DCB RID: 7627
	public Guid refuelStausHandle;

	// Token: 0x04001DCC RID: 7628
	[Serialize]
	public int numCyclesRunning;

	// Token: 0x04001DCD RID: 7629
	private float reactionMassTarget = 60f;

	// Token: 0x04001DCE RID: 7630
	private int[] ventCells;

	// Token: 0x020015D3 RID: 5587
	public class StatesInstance : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.GameInstance
	{
		// Token: 0x06009003 RID: 36867 RVA: 0x00349D45 File Offset: 0x00347F45
		public StatesInstance(Reactor smi) : base(smi)
		{
		}
	}

	// Token: 0x020015D4 RID: 5588
	public class States : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor>
	{
		// Token: 0x06009004 RID: 36868 RVA: 0x00349D50 File Offset: 0x00347F50
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			default_state = this.off;
			this.root.EventHandler(GameHashes.OnStorageChange, delegate(Reactor.StatesInstance smi)
			{
				PrimaryElement storedCoolant = smi.master.GetStoredCoolant();
				if (!storedCoolant)
				{
					smi.master.waterMeter.SetPositionPercent(0f);
					return;
				}
				smi.master.waterMeter.SetPositionPercent(storedCoolant.Mass / 90f);
			});
			this.off_pre.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.off);
			this.off.PlayAnim("off").Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.master.radEmitter.SetEmitting(false);
				smi.master.SetEmitRads(0f);
			}).ParamTransition<bool>(this.reactionUnderway, this.on, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>(this.melted, this.dead, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>(this.meltingDown, this.meltdown, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.master.TransferFuel();
				smi.master.TransferCoolant();
				if (smi.master.CanStartReaction())
				{
					smi.GoTo(this.on);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.on.Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.sm.reactionUnderway.Set(true, smi, false);
				smi.master.operational.SetActive(true, false);
				smi.master.SetEmitRads(2400f);
				smi.master.radEmitter.SetEmitting(true);
			}).EventHandler(GameHashes.NewDay, (Reactor.StatesInstance smi) => GameClock.Instance, delegate(Reactor.StatesInstance smi)
			{
				smi.master.numCyclesRunning++;
			}).Exit(delegate(Reactor.StatesInstance smi)
			{
				smi.sm.reactionUnderway.Set(false, smi, false);
				smi.master.numCyclesRunning = 0;
			}).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.master.TransferFuel();
				smi.master.TransferCoolant();
				smi.master.React(dt);
				smi.master.UpdateCoolantStatus();
				smi.master.UpdateVentStatus();
				smi.master.DumpSpentFuel();
				if (!smi.master.fuelDeliveryEnabled)
				{
					smi.master.refuelStausHandle = smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ReactorRefuelDisabled, null);
				}
				else
				{
					smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ReactorRefuelDisabled, false);
					smi.master.refuelStausHandle = Guid.Empty;
				}
				if (smi.master.GetActiveCoolant() != null)
				{
					smi.master.Cool(dt);
				}
				PrimaryElement activeFuel = smi.master.GetActiveFuel();
				if (activeFuel != null)
				{
					smi.master.temperatureMeter.SetPositionPercent(Mathf.Clamp01(activeFuel.Temperature / 3000f) / Reactor.meterFrameScaleHack);
					if (activeFuel.Temperature >= 3000f)
					{
						smi.sm.meltdownMassRemaining.Set(10f + smi.master.supplyStorage.MassStored() + smi.master.reactionStorage.MassStored() + smi.master.wasteStorage.MassStored(), smi, false);
						smi.master.supplyStorage.ConsumeAllIgnoringDisease();
						smi.master.reactionStorage.ConsumeAllIgnoringDisease();
						smi.master.wasteStorage.ConsumeAllIgnoringDisease();
						smi.GoTo(this.meltdown.pre);
						return;
					}
					if (activeFuel.Mass <= 0.25f)
					{
						smi.GoTo(this.off_pre);
						smi.master.temperatureMeter.SetPositionPercent(0f);
						return;
					}
				}
				else
				{
					smi.GoTo(this.off_pre);
					smi.master.temperatureMeter.SetPositionPercent(0f);
				}
			}, UpdateRate.SIM_200ms, false).DefaultState(this.on.pre);
			this.on.pre.PlayAnim("working_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.on.reacting).OnSignal(this.doVent, this.on.venting);
			this.on.reacting.PlayAnim("working_loop", KAnim.PlayMode.Loop).OnSignal(this.doVent, this.on.venting);
			this.on.venting.ParamTransition<bool>(this.canVent, this.on.venting.vent, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>(this.canVent, this.on.venting.ventIssue, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsFalse);
			this.on.venting.ventIssue.PlayAnim("venting_issue", KAnim.PlayMode.Loop).ParamTransition<bool>(this.canVent, this.on.venting.vent, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue);
			this.on.venting.vent.PlayAnim("venting").Enter(delegate(Reactor.StatesInstance smi)
			{
				PrimaryElement activeCoolant = smi.master.GetActiveCoolant();
				if (activeCoolant != null)
				{
					activeCoolant.GetComponent<Dumpable>().Dump(Grid.CellToPos(smi.master.GetVentCell()));
				}
			}).OnAnimQueueComplete(this.on.reacting);
			this.meltdown.ToggleStatusItem(Db.Get().BuildingStatusItems.ReactorMeltdown, null).ToggleNotification((Reactor.StatesInstance smi) => smi.master.CreateMeltdownNotification()).ParamTransition<float>(this.meltdownMassRemaining, this.dead, (Reactor.StatesInstance smi, float p) => p <= 0f).ToggleTag(GameTags.DeadReactor).DefaultState(this.meltdown.loop);
			this.meltdown.pre.PlayAnim("almost_meltdown_pre", KAnim.PlayMode.Once).QueueAnim("almost_meltdown_loop", false, null).QueueAnim("meltdown_pre", false, null).OnAnimQueueComplete(this.meltdown.loop);
			this.meltdown.loop.PlayAnim("meltdown_loop", KAnim.PlayMode.Loop).Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.master.radEmitter.SetEmitting(true);
				smi.master.SetEmitRads(4800f);
				smi.master.temperatureMeter.SetPositionPercent(1f / Reactor.meterFrameScaleHack);
				smi.master.UpdateCoolantStatus();
				if (this.meltingDown.Get(smi))
				{
					MusicManager.instance.PlaySong(Reactor.MELTDOWN_STINGER, false);
					MusicManager.instance.StopDynamicMusic(false);
				}
				else
				{
					MusicManager.instance.PlaySong(Reactor.MELTDOWN_STINGER, false);
					MusicManager.instance.SetSongParameter(Reactor.MELTDOWN_STINGER, "Music_PlayStinger", 1f, true);
					MusicManager.instance.StopDynamicMusic(false);
				}
				this.meltingDown.Set(true, smi, false);
			}).Exit(delegate(Reactor.StatesInstance smi)
			{
				this.meltingDown.Set(false, smi, false);
				MusicManager.instance.SetSongParameter(Reactor.MELTDOWN_STINGER, "Music_NuclearMeltdownActive", 0f, true);
			}).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.master.timeSinceMeltdownEmit += dt;
				float num = 0.5f;
				float b = 5f;
				if (smi.master.timeSinceMeltdownEmit > num && smi.sm.meltdownMassRemaining.Get(smi) > 0f)
				{
					smi.master.timeSinceMeltdownEmit -= num;
					float num2 = Mathf.Min(smi.sm.meltdownMassRemaining.Get(smi), b);
					smi.sm.meltdownMassRemaining.Delta(-num2, smi);
					for (int i = 0; i < 3; i++)
					{
						if (num2 >= NuclearWasteCometConfig.MASS)
						{
							GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(NuclearWasteCometConfig.ID), smi.master.transform.position + Vector3.up * 2f, Quaternion.identity, null, null, true, 0);
							gameObject.SetActive(true);
							Comet component = gameObject.GetComponent<Comet>();
							component.ignoreObstacleForDamage.Set(smi.master.gameObject.GetComponent<KPrefabID>());
							component.addTiles = 1;
							int num3 = 270;
							while (num3 > 225 && num3 < 335)
							{
								num3 = UnityEngine.Random.Range(0, 360);
							}
							float f = (float)num3 * 3.1415927f / 180f;
							component.Velocity = new Vector2(-Mathf.Cos(f) * 20f, Mathf.Sin(f) * 20f);
							component.GetComponent<KBatchedAnimController>().Rotation = (float)(-(float)num3) - 90f;
							num2 -= NuclearWasteCometConfig.MASS;
						}
					}
					for (int j = 0; j < 3; j++)
					{
						if (num2 >= 0.001f)
						{
							SimMessages.AddRemoveSubstance(Grid.PosToCell(smi.master.transform.position + Vector3.up * 3f + Vector3.right * (float)j * 2f), SimHashes.NuclearWaste, CellEventLogger.Instance.ElementEmitted, num2 / 3f, 3000f, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.RoundToInt(50f * (num2 / 3f)), true, -1);
						}
					}
				}
			}, UpdateRate.SIM_200ms, false);
			this.dead.PlayAnim("dead").ToggleTag(GameTags.DeadReactor).Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.master.temperatureMeter.SetPositionPercent(1f / Reactor.meterFrameScaleHack);
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DeadReactorCoolingOff, smi);
				this.melted.Set(true, smi, false);
			}).Exit(delegate(Reactor.StatesInstance smi)
			{
				smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DeadReactorCoolingOff, false);
			}).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.sm.timeSinceMeltdown.Delta(dt, smi);
				smi.master.radEmitter.emitRads = Mathf.Lerp(4800f, 0f, smi.sm.timeSinceMeltdown.Get(smi) / 3000f);
				smi.master.radEmitter.Refresh();
			}, UpdateRate.SIM_200ms, false);
		}

		// Token: 0x04006DE5 RID: 28133
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Signal doVent;

		// Token: 0x04006DE6 RID: 28134
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter canVent = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(true);

		// Token: 0x04006DE7 RID: 28135
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter reactionUnderway = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter();

		// Token: 0x04006DE8 RID: 28136
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter meltdownMassRemaining = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter(0f);

		// Token: 0x04006DE9 RID: 28137
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter timeSinceMeltdown = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter(0f);

		// Token: 0x04006DEA RID: 28138
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter meltingDown = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(false);

		// Token: 0x04006DEB RID: 28139
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter melted = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(false);

		// Token: 0x04006DEC RID: 28140
		public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State off;

		// Token: 0x04006DED RID: 28141
		public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State off_pre;

		// Token: 0x04006DEE RID: 28142
		public Reactor.States.ReactingStates on;

		// Token: 0x04006DEF RID: 28143
		public Reactor.States.MeltdownStates meltdown;

		// Token: 0x04006DF0 RID: 28144
		public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State dead;

		// Token: 0x02002521 RID: 9505
		public class ReactingStates : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
		{
			// Token: 0x0400A55B RID: 42331
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pre;

			// Token: 0x0400A55C RID: 42332
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State reacting;

			// Token: 0x0400A55D RID: 42333
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pst;

			// Token: 0x0400A55E RID: 42334
			public Reactor.States.ReactingStates.VentingStates venting;

			// Token: 0x02003538 RID: 13624
			public class VentingStates : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
			{
				// Token: 0x0400D7AD RID: 55213
				public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State ventIssue;

				// Token: 0x0400D7AE RID: 55214
				public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State vent;
			}
		}

		// Token: 0x02002522 RID: 9506
		public class MeltdownStates : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
		{
			// Token: 0x0400A55F RID: 42335
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State almost_pre;

			// Token: 0x0400A560 RID: 42336
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State almost_loop;

			// Token: 0x0400A561 RID: 42337
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pre;

			// Token: 0x0400A562 RID: 42338
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State loop;
		}
	}
}
