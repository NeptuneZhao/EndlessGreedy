using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000AC5 RID: 2757
public class FuelTank : KMonoBehaviour, IUserControlledCapacity, IFuelTank
{
	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x060051E3 RID: 20963 RVA: 0x001D6385 File Offset: 0x001D4585
	public IStorage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x060051E4 RID: 20964 RVA: 0x001D638D File Offset: 0x001D458D
	public bool ConsumeFuelOnLand
	{
		get
		{
			return this.consumeFuelOnLand;
		}
	}

	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x060051E5 RID: 20965 RVA: 0x001D6395 File Offset: 0x001D4595
	// (set) Token: 0x060051E6 RID: 20966 RVA: 0x001D63A0 File Offset: 0x001D45A0
	public float UserMaxCapacity
	{
		get
		{
			return this.targetFillMass;
		}
		set
		{
			this.targetFillMass = value;
			this.storage.capacityKg = this.targetFillMass;
			ConduitConsumer component = base.GetComponent<ConduitConsumer>();
			if (component != null)
			{
				component.capacityKG = this.targetFillMass;
			}
			ManualDeliveryKG component2 = base.GetComponent<ManualDeliveryKG>();
			if (component2 != null)
			{
				component2.capacity = (component2.refillMass = this.targetFillMass);
			}
			base.Trigger(-945020481, this);
		}
	}

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x060051E7 RID: 20967 RVA: 0x001D6412 File Offset: 0x001D4612
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x060051E8 RID: 20968 RVA: 0x001D6419 File Offset: 0x001D4619
	public float MaxCapacity
	{
		get
		{
			return this.physicalFuelCapacity;
		}
	}

	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x060051E9 RID: 20969 RVA: 0x001D6421 File Offset: 0x001D4621
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000611 RID: 1553
	// (get) Token: 0x060051EA RID: 20970 RVA: 0x001D642E File Offset: 0x001D462E
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000612 RID: 1554
	// (get) Token: 0x060051EB RID: 20971 RVA: 0x001D6431 File Offset: 0x001D4631
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x060051EC RID: 20972 RVA: 0x001D6439 File Offset: 0x001D4639
	// (set) Token: 0x060051ED RID: 20973 RVA: 0x001D6444 File Offset: 0x001D4644
	public Tag FuelType
	{
		get
		{
			return this.fuelType;
		}
		set
		{
			this.fuelType = value;
			if (this.storage.storageFilters == null)
			{
				this.storage.storageFilters = new List<Tag>();
			}
			this.storage.storageFilters.Add(this.fuelType);
			ManualDeliveryKG component = base.GetComponent<ManualDeliveryKG>();
			if (component != null)
			{
				component.RequestedItemTag = this.fuelType;
			}
		}
	}

	// Token: 0x060051EE RID: 20974 RVA: 0x001D64A7 File Offset: 0x001D46A7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<FuelTank>(-905833192, FuelTank.OnCopySettingsDelegate);
	}

	// Token: 0x060051EF RID: 20975 RVA: 0x001D64C0 File Offset: 0x001D46C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.targetFillMass == -1f)
		{
			this.targetFillMass = this.physicalFuelCapacity;
		}
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionProperlyFueled(this));
		}
		base.Subscribe<FuelTank>(-887025858, FuelTank.OnRocketLandedDelegate);
		this.UserMaxCapacity = this.UserMaxCapacity;
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.OnStorageChange(null);
		base.Subscribe<FuelTank>(-1697596308, FuelTank.OnStorageChangedDelegate);
	}

	// Token: 0x060051F0 RID: 20976 RVA: 0x001D65B5 File Offset: 0x001D47B5
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.capacityKg);
	}

	// Token: 0x060051F1 RID: 20977 RVA: 0x001D65D9 File Offset: 0x001D47D9
	private void OnRocketLanded(object data)
	{
		if (this.ConsumeFuelOnLand)
		{
			this.storage.ConsumeAllIgnoringDisease();
		}
	}

	// Token: 0x060051F2 RID: 20978 RVA: 0x001D65F0 File Offset: 0x001D47F0
	private void OnCopySettings(object data)
	{
		FuelTank component = ((GameObject)data).GetComponent<FuelTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x060051F3 RID: 20979 RVA: 0x001D6620 File Offset: 0x001D4820
	public void DEBUG_FillTank()
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			RocketEngine rocketEngine = null;
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				rocketEngine = gameObject.GetComponent<RocketEngine>();
				if (rocketEngine != null && rocketEngine.mainEngine)
				{
					break;
				}
			}
			if (rocketEngine != null)
			{
				Element element = ElementLoader.GetElement(rocketEngine.fuelTag);
				if (element.IsLiquid)
				{
					this.storage.AddLiquid(element.id, this.targetFillMass - this.storage.MassStored(), element.defaultValues.temperature, 0, 0, false, true);
					return;
				}
				if (element.IsGas)
				{
					this.storage.AddGasChunk(element.id, this.targetFillMass - this.storage.MassStored(), element.defaultValues.temperature, 0, 0, false, true);
					return;
				}
				if (element.IsSolid)
				{
					this.storage.AddOre(element.id, this.targetFillMass - this.storage.MassStored(), element.defaultValues.temperature, 0, 0, false, true);
					return;
				}
			}
			else
			{
				global::Debug.LogWarning("Fuel tank couldn't find rocket engine");
			}
			return;
		}
		RocketEngineCluster rocketEngineCluster = null;
		foreach (GameObject gameObject2 in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			rocketEngineCluster = gameObject2.GetComponent<RocketEngineCluster>();
			if (rocketEngineCluster != null && rocketEngineCluster.mainEngine)
			{
				break;
			}
		}
		if (rocketEngineCluster != null)
		{
			Element element2 = ElementLoader.GetElement(rocketEngineCluster.fuelTag);
			if (element2.IsLiquid)
			{
				this.storage.AddLiquid(element2.id, this.targetFillMass - this.storage.MassStored(), element2.defaultValues.temperature, 0, 0, false, true);
			}
			else if (element2.IsGas)
			{
				this.storage.AddGasChunk(element2.id, this.targetFillMass - this.storage.MassStored(), element2.defaultValues.temperature, 0, 0, false, true);
			}
			else if (element2.IsSolid)
			{
				this.storage.AddOre(element2.id, this.targetFillMass - this.storage.MassStored(), element2.defaultValues.temperature, 0, 0, false, true);
			}
			rocketEngineCluster.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().UpdateStatusItem();
			return;
		}
		global::Debug.LogWarning("Fuel tank couldn't find rocket engine");
	}

	// Token: 0x04003614 RID: 13844
	public Storage storage;

	// Token: 0x04003615 RID: 13845
	private MeterController meter;

	// Token: 0x04003616 RID: 13846
	[Serialize]
	public float targetFillMass = -1f;

	// Token: 0x04003617 RID: 13847
	[SerializeField]
	public float physicalFuelCapacity;

	// Token: 0x04003618 RID: 13848
	public bool consumeFuelOnLand;

	// Token: 0x04003619 RID: 13849
	[SerializeField]
	private Tag fuelType;

	// Token: 0x0400361A RID: 13850
	private static readonly EventSystem.IntraObjectHandler<FuelTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<FuelTank>(delegate(FuelTank component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400361B RID: 13851
	private static readonly EventSystem.IntraObjectHandler<FuelTank> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<FuelTank>(delegate(FuelTank component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x0400361C RID: 13852
	private static readonly EventSystem.IntraObjectHandler<FuelTank> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<FuelTank>(delegate(FuelTank component, object data)
	{
		component.OnStorageChange(data);
	});
}
