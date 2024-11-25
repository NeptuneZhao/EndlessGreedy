using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000684 RID: 1668
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
[AddComponentMenu("KMonoBehaviour/scripts/Battery")]
public class Battery : KMonoBehaviour, IEnergyConsumer, ICircuitConnected, IGameObjectEffectDescriptor, IEnergyProducer
{
	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06002958 RID: 10584 RVA: 0x000E9DF5 File Offset: 0x000E7FF5
	// (set) Token: 0x06002959 RID: 10585 RVA: 0x000E9DFD File Offset: 0x000E7FFD
	public float WattsUsed { get; private set; }

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x0600295A RID: 10586 RVA: 0x000E9E06 File Offset: 0x000E8006
	public float WattsNeededWhenActive
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x0600295B RID: 10587 RVA: 0x000E9E0D File Offset: 0x000E800D
	public float PercentFull
	{
		get
		{
			return this.joulesAvailable / this.capacity;
		}
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x0600295C RID: 10588 RVA: 0x000E9E1C File Offset: 0x000E801C
	public float PreviousPercentFull
	{
		get
		{
			return this.PreviousJoulesAvailable / this.capacity;
		}
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x0600295D RID: 10589 RVA: 0x000E9E2B File Offset: 0x000E802B
	public float JoulesAvailable
	{
		get
		{
			return this.joulesAvailable;
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x0600295E RID: 10590 RVA: 0x000E9E33 File Offset: 0x000E8033
	public float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x0600295F RID: 10591 RVA: 0x000E9E3B File Offset: 0x000E803B
	// (set) Token: 0x06002960 RID: 10592 RVA: 0x000E9E43 File Offset: 0x000E8043
	public float ChargeCapacity { get; private set; }

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06002961 RID: 10593 RVA: 0x000E9E4C File Offset: 0x000E804C
	public int PowerSortOrder
	{
		get
		{
			return this.powerSortOrder;
		}
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06002962 RID: 10594 RVA: 0x000E9E54 File Offset: 0x000E8054
	public string Name
	{
		get
		{
			return base.GetComponent<KSelectable>().GetName();
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06002963 RID: 10595 RVA: 0x000E9E61 File Offset: 0x000E8061
	// (set) Token: 0x06002964 RID: 10596 RVA: 0x000E9E69 File Offset: 0x000E8069
	public int PowerCell { get; private set; }

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06002965 RID: 10597 RVA: 0x000E9E72 File Offset: 0x000E8072
	public ushort CircuitID
	{
		get
		{
			return Game.Instance.circuitManager.GetCircuitID(this);
		}
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06002966 RID: 10598 RVA: 0x000E9E84 File Offset: 0x000E8084
	public bool IsConnected
	{
		get
		{
			return this.connectionStatus > CircuitManager.ConnectionStatus.NotConnected;
		}
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06002967 RID: 10599 RVA: 0x000E9E8F File Offset: 0x000E808F
	public bool IsPowered
	{
		get
		{
			return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06002968 RID: 10600 RVA: 0x000E9E9A File Offset: 0x000E809A
	// (set) Token: 0x06002969 RID: 10601 RVA: 0x000E9EA2 File Offset: 0x000E80A2
	public bool IsVirtual { get; protected set; }

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x0600296A RID: 10602 RVA: 0x000E9EAB File Offset: 0x000E80AB
	// (set) Token: 0x0600296B RID: 10603 RVA: 0x000E9EB3 File Offset: 0x000E80B3
	public object VirtualCircuitKey { get; protected set; }

	// Token: 0x0600296C RID: 10604 RVA: 0x000E9EBC File Offset: 0x000E80BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Batteries.Add(this);
		Building component = base.GetComponent<Building>();
		this.PowerCell = component.GetPowerInputCell();
		base.Subscribe<Battery>(-1582839653, Battery.OnTagsChangedDelegate);
		this.OnTagsChanged(null);
		this.meter = (base.GetComponent<PowerTransformer>() ? null : new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		}));
		Game.Instance.circuitManager.Connect(this);
		Game.Instance.energySim.AddBattery(this);
	}

	// Token: 0x0600296D RID: 10605 RVA: 0x000E9F7C File Offset: 0x000E817C
	private void OnTagsChanged(object data)
	{
		if (this.HasAllTags(this.connectedTags))
		{
			Game.Instance.circuitManager.Connect(this);
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.BatteryJoulesAvailable, this);
			return;
		}
		Game.Instance.circuitManager.Disconnect(this, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.BatteryJoulesAvailable, false);
	}

	// Token: 0x0600296E RID: 10606 RVA: 0x000EA000 File Offset: 0x000E8200
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveBattery(this);
		Game.Instance.circuitManager.Disconnect(this, true);
		Components.Batteries.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600296F RID: 10607 RVA: 0x000EA034 File Offset: 0x000E8234
	public virtual void EnergySim200ms(float dt)
	{
		this.dt = dt;
		this.joulesConsumed = 0f;
		this.WattsUsed = 0f;
		this.ChargeCapacity = this.chargeWattage * dt;
		if (this.meter != null)
		{
			float percentFull = this.PercentFull;
			this.meter.SetPositionPercent(percentFull);
		}
		this.UpdateSounds();
		this.PreviousJoulesAvailable = this.JoulesAvailable;
		this.ConsumeEnergy(this.joulesLostPerSecond * dt, true);
	}

	// Token: 0x06002970 RID: 10608 RVA: 0x000EA0A8 File Offset: 0x000E82A8
	private void UpdateSounds()
	{
		float previousPercentFull = this.PreviousPercentFull;
		float percentFull = this.PercentFull;
		if (percentFull == 0f && previousPercentFull != 0f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryDischarged);
		}
		if (percentFull > 0.999f && previousPercentFull <= 0.999f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryFull);
		}
		if (percentFull < 0.25f && previousPercentFull >= 0.25f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryWarning);
		}
	}

	// Token: 0x06002971 RID: 10609 RVA: 0x000EA124 File Offset: 0x000E8324
	public void SetConnectionStatus(CircuitManager.ConnectionStatus status)
	{
		this.connectionStatus = status;
		if (status == CircuitManager.ConnectionStatus.NotConnected)
		{
			this.operational.SetActive(false, false);
			return;
		}
		this.operational.SetActive(this.operational.IsOperational && this.JoulesAvailable > 0f, false);
	}

	// Token: 0x06002972 RID: 10610 RVA: 0x000EA174 File Offset: 0x000E8374
	public void AddEnergy(float joules)
	{
		this.joulesAvailable = Mathf.Min(this.capacity, this.JoulesAvailable + joules);
		this.joulesConsumed += joules;
		this.ChargeCapacity -= joules;
		this.WattsUsed = this.joulesConsumed / this.dt;
	}

	// Token: 0x06002973 RID: 10611 RVA: 0x000EA1CC File Offset: 0x000E83CC
	public void ConsumeEnergy(float joules, bool report = false)
	{
		if (report)
		{
			float num = Mathf.Min(this.JoulesAvailable, joules);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, -num, StringFormatter.Replace(BUILDINGS.PREFABS.BATTERY.CHARGE_LOSS, "{Battery}", this.GetProperName()), null);
		}
		this.joulesAvailable = Mathf.Max(0f, this.JoulesAvailable - joules);
	}

	// Token: 0x06002974 RID: 10612 RVA: 0x000EA22A File Offset: 0x000E842A
	public void ConsumeEnergy(float joules)
	{
		this.ConsumeEnergy(joules, false);
	}

	// Token: 0x06002975 RID: 10613 RVA: 0x000EA234 File Offset: 0x000E8434
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.powerTransformer == null)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.REQUIRESPOWERGENERATOR, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWERGENERATOR, Descriptor.DescriptorType.Requirement, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.BATTERYCAPACITY, GameUtil.GetFormattedJoules(this.capacity, "", GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYCAPACITY, GameUtil.GetFormattedJoules(this.capacity, "", GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.BATTERYLEAK, GameUtil.GetFormattedJoules(this.joulesLostPerSecond, "F1", GameUtil.TimeSlice.PerCycle)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYLEAK, GameUtil.GetFormattedJoules(this.joulesLostPerSecond, "F1", GameUtil.TimeSlice.PerCycle)), Descriptor.DescriptorType.Effect, false));
		}
		else
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.TRANSFORMER_INPUT_WIRE, UI.BUILDINGEFFECTS.TOOLTIPS.TRANSFORMER_INPUT_WIRE, Descriptor.DescriptorType.Requirement, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.TRANSFORMER_OUTPUT_WIRE, GameUtil.GetFormattedWattage(this.capacity, GameUtil.WattageFormatterUnit.Automatic, true)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.TRANSFORMER_OUTPUT_WIRE, GameUtil.GetFormattedWattage(this.capacity, GameUtil.WattageFormatterUnit.Automatic, true)), Descriptor.DescriptorType.Requirement, false));
		}
		return list;
	}

	// Token: 0x06002976 RID: 10614 RVA: 0x000EA37C File Offset: 0x000E857C
	[ContextMenu("Refill Power")]
	public void DEBUG_RefillPower()
	{
		this.joulesAvailable = this.capacity;
	}

	// Token: 0x040017D9 RID: 6105
	[SerializeField]
	public float capacity;

	// Token: 0x040017DA RID: 6106
	[SerializeField]
	public float chargeWattage = float.PositiveInfinity;

	// Token: 0x040017DB RID: 6107
	[Serialize]
	private float joulesAvailable;

	// Token: 0x040017DC RID: 6108
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x040017DD RID: 6109
	[MyCmpGet]
	public PowerTransformer powerTransformer;

	// Token: 0x040017DE RID: 6110
	protected MeterController meter;

	// Token: 0x040017E0 RID: 6112
	public float joulesLostPerSecond;

	// Token: 0x040017E2 RID: 6114
	[SerializeField]
	public int powerSortOrder;

	// Token: 0x040017E6 RID: 6118
	private float PreviousJoulesAvailable;

	// Token: 0x040017E7 RID: 6119
	private CircuitManager.ConnectionStatus connectionStatus;

	// Token: 0x040017E8 RID: 6120
	public static readonly Tag[] DEFAULT_CONNECTED_TAGS = new Tag[]
	{
		GameTags.Operational
	};

	// Token: 0x040017E9 RID: 6121
	[SerializeField]
	public Tag[] connectedTags = Battery.DEFAULT_CONNECTED_TAGS;

	// Token: 0x040017EA RID: 6122
	private static readonly EventSystem.IntraObjectHandler<Battery> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Battery>(delegate(Battery component, object data)
	{
		component.OnTagsChanged(data);
	});

	// Token: 0x040017EB RID: 6123
	private float dt;

	// Token: 0x040017EC RID: 6124
	private float joulesConsumed;
}
