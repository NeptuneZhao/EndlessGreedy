using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000876 RID: 2166
[SkipSaveFileSerialization]
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConsumer : SimComponent, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06003C65 RID: 15461 RVA: 0x0014EC74 File Offset: 0x0014CE74
	// (remove) Token: 0x06003C66 RID: 15462 RVA: 0x0014ECAC File Offset: 0x0014CEAC
	public event Action<Sim.ConsumedMassInfo> OnElementConsumed;

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06003C67 RID: 15463 RVA: 0x0014ECE1 File Offset: 0x0014CEE1
	public float AverageConsumeRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x06003C68 RID: 15464 RVA: 0x0014ECF8 File Offset: 0x0014CEF8
	public static void ClearInstanceMap()
	{
		ElementConsumer.handleInstanceMap.Clear();
	}

	// Token: 0x06003C69 RID: 15465 RVA: 0x0014ED04 File Offset: 0x0014CF04
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		if (this.elementToConsume == SimHashes.Void)
		{
			throw new ArgumentException("No consumable elements specified");
		}
		if (!this.ignoreActiveChanged)
		{
			base.Subscribe<ElementConsumer>(824508782, ElementConsumer.OnActiveChangedDelegate);
		}
		if (this.capacityKG != float.PositiveInfinity)
		{
			this.hasAvailableCapacity = !this.IsStorageFull();
			base.Subscribe<ElementConsumer>(-1697596308, ElementConsumer.OnStorageChangeDelegate);
		}
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x0014ED90 File Offset: 0x0014CF90
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x06003C6B RID: 15467 RVA: 0x0014EDAE File Offset: 0x0014CFAE
	protected virtual bool IsActive()
	{
		return this.operational == null || this.operational.IsActive;
	}

	// Token: 0x06003C6C RID: 15468 RVA: 0x0014EDCC File Offset: 0x0014CFCC
	public void EnableConsumption(bool enabled)
	{
		bool flag = this.consumptionEnabled;
		this.consumptionEnabled = enabled;
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		if (enabled != flag)
		{
			this.UpdateSimData();
		}
	}

	// Token: 0x06003C6D RID: 15469 RVA: 0x0014EE00 File Offset: 0x0014D000
	private bool IsStorageFull()
	{
		PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.elementToConsume);
		return primaryElement != null && primaryElement.Mass >= this.capacityKG;
	}

	// Token: 0x06003C6E RID: 15470 RVA: 0x0014EE3B File Offset: 0x0014D03B
	public void RefreshConsumptionRate()
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimData();
	}

	// Token: 0x06003C6F RID: 15471 RVA: 0x0014EE54 File Offset: 0x0014D054
	private void UpdateSimData()
	{
		global::Debug.Assert(Sim.IsValidHandle(this.simHandle));
		int sampleCell = this.GetSampleCell();
		float num = (this.consumptionEnabled && this.hasAvailableCapacity) ? this.consumptionRate : 0f;
		SimMessages.SetElementConsumerData(this.simHandle, sampleCell, num);
		this.UpdateStatusItem();
	}

	// Token: 0x06003C70 RID: 15472 RVA: 0x0014EEAC File Offset: 0x0014D0AC
	public static void AddMass(Sim.ConsumedMassInfo consumed_info)
	{
		if (!Sim.IsValidHandle(consumed_info.simHandle))
		{
			return;
		}
		ElementConsumer elementConsumer;
		if (ElementConsumer.handleInstanceMap.TryGetValue(consumed_info.simHandle, out elementConsumer))
		{
			elementConsumer.AddMassInternal(consumed_info);
		}
	}

	// Token: 0x06003C71 RID: 15473 RVA: 0x0014EEE2 File Offset: 0x0014D0E2
	private int GetSampleCell()
	{
		return Grid.PosToCell(base.transform.GetPosition() + this.sampleCellOffset);
	}

	// Token: 0x06003C72 RID: 15474 RVA: 0x0014EF00 File Offset: 0x0014D100
	private void AddMassInternal(Sim.ConsumedMassInfo consumed_info)
	{
		if (consumed_info.mass > 0f)
		{
			if (this.storeOnConsume)
			{
				Element element = ElementLoader.elements[(int)consumed_info.removedElemIdx];
				if (this.elementToConsume == SimHashes.Vacuum || this.elementToConsume == element.id)
				{
					if (element.IsLiquid)
					{
						this.storage.AddLiquid(element.id, consumed_info.mass, consumed_info.temperature, consumed_info.diseaseIdx, consumed_info.diseaseCount, true, true);
					}
					else if (element.IsGas)
					{
						this.storage.AddGasChunk(element.id, consumed_info.mass, consumed_info.temperature, consumed_info.diseaseIdx, consumed_info.diseaseCount, true, true);
					}
				}
			}
			else
			{
				this.consumedTemperature = GameUtil.GetFinalTemperature(consumed_info.temperature, consumed_info.mass, this.consumedTemperature, this.consumedMass);
				this.consumedMass += consumed_info.mass;
				if (this.OnElementConsumed != null)
				{
					this.OnElementConsumed(consumed_info);
				}
			}
		}
		Game.Instance.accumulators.Accumulate(this.accumulator, consumed_info.mass);
	}

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06003C73 RID: 15475 RVA: 0x0014F02C File Offset: 0x0014D22C
	public bool IsElementAvailable
	{
		get
		{
			int sampleCell = this.GetSampleCell();
			SimHashes id = Grid.Element[sampleCell].id;
			return this.elementToConsume == id && Grid.Mass[sampleCell] >= this.minimumMass;
		}
	}

	// Token: 0x06003C74 RID: 15476 RVA: 0x0014F070 File Offset: 0x0014D270
	private void UpdateStatusItem()
	{
		if (this.showInStatusPanel)
		{
			if (this.statusHandle == Guid.Empty && this.IsActive() && this.consumptionEnabled)
			{
				this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.ElementConsumer, this);
				return;
			}
			if (this.statusHandle != Guid.Empty && !this.consumptionEnabled)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
				this.statusHandle = Guid.Empty;
				return;
			}
		}
		else if (this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
			this.statusHandle = Guid.Empty;
		}
	}

	// Token: 0x06003C75 RID: 15477 RVA: 0x0014F134 File Offset: 0x0014D334
	private void OnStorageChange(object data)
	{
		bool flag = !this.IsStorageFull();
		if (flag != this.hasAvailableCapacity)
		{
			this.hasAvailableCapacity = flag;
			this.RefreshConsumptionRate();
		}
	}

	// Token: 0x06003C76 RID: 15478 RVA: 0x0014F161 File Offset: 0x0014D361
	protected override void OnCmpEnable()
	{
		if (!base.isSpawned)
		{
			return;
		}
		if (!this.IsActive())
		{
			return;
		}
		this.UpdateStatusItem();
	}

	// Token: 0x06003C77 RID: 15479 RVA: 0x0014F17B File Offset: 0x0014D37B
	protected override void OnCmpDisable()
	{
		this.UpdateStatusItem();
	}

	// Token: 0x06003C78 RID: 15480 RVA: 0x0014F184 File Offset: 0x0014D384
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.isRequired && this.showDescriptor)
		{
			Element element = ElementLoader.FindElementByHash(this.elementToConsume);
			string arg = element.tag.ProperName();
			if (element.IsVacuum)
			{
				if (this.configuration == ElementConsumer.Configuration.AllGas)
				{
					arg = ELEMENTS.STATE.GAS;
				}
				else if (this.configuration == ElementConsumer.Configuration.AllLiquid)
				{
					arg = ELEMENTS.STATE.LIQUID;
				}
				else
				{
					arg = UI.BUILDINGEFFECTS.CONSUMESANYELEMENT;
				}
			}
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESELEMENT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESELEMENT, arg), Descriptor.DescriptorType.Requirement);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06003C79 RID: 15481 RVA: 0x0014F23C File Offset: 0x0014D43C
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.showDescriptor)
		{
			Element element = ElementLoader.FindElementByHash(this.elementToConsume);
			string arg = element.tag.ProperName();
			if (element.IsVacuum)
			{
				if (this.configuration == ElementConsumer.Configuration.AllGas)
				{
					arg = ELEMENTS.STATE.GAS;
				}
				else if (this.configuration == ElementConsumer.Configuration.AllLiquid)
				{
					arg = ELEMENTS.STATE.LIQUID;
				}
				else
				{
					arg = UI.BUILDINGEFFECTS.CONSUMESANYELEMENT;
				}
			}
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.consumptionRate / 100f * 100f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.consumptionRate / 100f * 100f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06003C7A RID: 15482 RVA: 0x0014F328 File Offset: 0x0014D528
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors())
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.EffectDescriptors())
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06003C7B RID: 15483 RVA: 0x0014F3C4 File Offset: 0x0014D5C4
	private void OnActiveChanged(object data)
	{
		bool isActive = this.operational.IsActive;
		this.EnableConsumption(isActive);
	}

	// Token: 0x06003C7C RID: 15484 RVA: 0x0014F3E4 File Offset: 0x0014D5E4
	protected override void OnSimUnregister()
	{
		global::Debug.Assert(Sim.IsValidHandle(this.simHandle));
		ElementConsumer.handleInstanceMap.Remove(this.simHandle);
		ElementConsumer.StaticUnregister(this.simHandle);
	}

	// Token: 0x06003C7D RID: 15485 RVA: 0x0014F412 File Offset: 0x0014D612
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		SimMessages.AddElementConsumer(this.GetSampleCell(), this.configuration, this.elementToConsume, this.consumptionRadius, cb_handle.index);
	}

	// Token: 0x06003C7E RID: 15486 RVA: 0x0014F438 File Offset: 0x0014D638
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(ElementConsumer.StaticUnregister);
	}

	// Token: 0x06003C7F RID: 15487 RVA: 0x0014F446 File Offset: 0x0014D646
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveElementConsumer(-1, sim_handle);
	}

	// Token: 0x06003C80 RID: 15488 RVA: 0x0014F45A File Offset: 0x0014D65A
	protected override void OnSimRegistered()
	{
		if (this.consumptionEnabled)
		{
			this.UpdateSimData();
		}
		ElementConsumer.handleInstanceMap[this.simHandle] = this;
	}

	// Token: 0x040024DC RID: 9436
	[HashedEnum]
	[SerializeField]
	public SimHashes elementToConsume = SimHashes.Vacuum;

	// Token: 0x040024DD RID: 9437
	[SerializeField]
	public float consumptionRate;

	// Token: 0x040024DE RID: 9438
	[SerializeField]
	public byte consumptionRadius = 1;

	// Token: 0x040024DF RID: 9439
	[SerializeField]
	public float minimumMass;

	// Token: 0x040024E0 RID: 9440
	[SerializeField]
	public bool showInStatusPanel = true;

	// Token: 0x040024E1 RID: 9441
	[SerializeField]
	public Vector3 sampleCellOffset;

	// Token: 0x040024E2 RID: 9442
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x040024E3 RID: 9443
	[SerializeField]
	public ElementConsumer.Configuration configuration;

	// Token: 0x040024E4 RID: 9444
	[Serialize]
	[NonSerialized]
	public float consumedMass;

	// Token: 0x040024E5 RID: 9445
	[Serialize]
	[NonSerialized]
	public float consumedTemperature;

	// Token: 0x040024E6 RID: 9446
	[SerializeField]
	public bool storeOnConsume;

	// Token: 0x040024E7 RID: 9447
	[MyCmpGet]
	public Storage storage;

	// Token: 0x040024E8 RID: 9448
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040024E9 RID: 9449
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x040024EB RID: 9451
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x040024EC RID: 9452
	public bool ignoreActiveChanged;

	// Token: 0x040024ED RID: 9453
	private Guid statusHandle;

	// Token: 0x040024EE RID: 9454
	public bool showDescriptor = true;

	// Token: 0x040024EF RID: 9455
	public bool isRequired = true;

	// Token: 0x040024F0 RID: 9456
	private bool consumptionEnabled;

	// Token: 0x040024F1 RID: 9457
	private bool hasAvailableCapacity = true;

	// Token: 0x040024F2 RID: 9458
	private static Dictionary<int, ElementConsumer> handleInstanceMap = new Dictionary<int, ElementConsumer>();

	// Token: 0x040024F3 RID: 9459
	private static readonly EventSystem.IntraObjectHandler<ElementConsumer> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ElementConsumer>(delegate(ElementConsumer component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x040024F4 RID: 9460
	private static readonly EventSystem.IntraObjectHandler<ElementConsumer> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<ElementConsumer>(delegate(ElementConsumer component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x0200177C RID: 6012
	public enum Configuration
	{
		// Token: 0x040072DE RID: 29406
		Element,
		// Token: 0x040072DF RID: 29407
		AllLiquid,
		// Token: 0x040072E0 RID: 29408
		AllGas
	}
}
