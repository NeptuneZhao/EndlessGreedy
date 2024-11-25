using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000AD5 RID: 2773
[AddComponentMenu("KMonoBehaviour/scripts/OxidizerTank")]
public class OxidizerTank : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x06005258 RID: 21080 RVA: 0x001D860E File Offset: 0x001D680E
	public bool IsSuspended
	{
		get
		{
			return this.isSuspended;
		}
	}

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x06005259 RID: 21081 RVA: 0x001D8616 File Offset: 0x001D6816
	// (set) Token: 0x0600525A RID: 21082 RVA: 0x001D8620 File Offset: 0x001D6820
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
			base.Trigger(-945020481, this);
			this.OnStorageCapacityChanged(this.targetFillMass);
			if (this.filteredStorage != null)
			{
				this.filteredStorage.FilterChanged();
			}
		}
	}

	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x0600525B RID: 21083 RVA: 0x001D868C File Offset: 0x001D688C
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x0600525C RID: 21084 RVA: 0x001D8693 File Offset: 0x001D6893
	public float MaxCapacity
	{
		get
		{
			return this.maxFillMass;
		}
	}

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x0600525D RID: 21085 RVA: 0x001D869B File Offset: 0x001D689B
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x0600525E RID: 21086 RVA: 0x001D86A8 File Offset: 0x001D68A8
	public float TotalOxidizerPower
	{
		get
		{
			float num = 0f;
			foreach (GameObject gameObject in this.storage.items)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				float num2;
				if (DlcManager.FeatureClusterSpaceEnabled())
				{
					num2 = Clustercraft.dlc1OxidizerEfficiencies[component.ElementID.CreateTag()];
				}
				else
				{
					num2 = RocketStats.oxidizerEfficiencies[component.ElementID.CreateTag()];
				}
				num += component.Mass * num2;
			}
			return num;
		}
	}

	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x0600525F RID: 21087 RVA: 0x001D874C File Offset: 0x001D694C
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x06005260 RID: 21088 RVA: 0x001D874F File Offset: 0x001D694F
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x06005261 RID: 21089 RVA: 0x001D8758 File Offset: 0x001D6958
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OxidizerTank>(-905833192, OxidizerTank.OnCopySettingsDelegate);
		if (this.supportsMultipleOxidizers)
		{
			this.filteredStorage = new FilteredStorage(this, null, this, true, Db.Get().ChoreTypes.Fetch);
			this.filteredStorage.FilterChanged();
			KBatchedAnimTracker componentInChildren = base.gameObject.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
		}
	}

	// Token: 0x06005262 RID: 21090 RVA: 0x001D87C8 File Offset: 0x001D69C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.discoverResourcesOnSpawn != null)
		{
			foreach (SimHashes hash in this.discoverResourcesOnSpawn)
			{
				Element element = ElementLoader.FindElementByHash(hash);
				DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
			}
		}
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			global::Debug.Assert(DlcManager.IsExpansion1Active(), "EXP1 not active but trying to use EXP1 rockety system");
			component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionSufficientOxidizer(this));
		}
		this.UserMaxCapacity = Mathf.Min(this.UserMaxCapacity, this.maxFillMass);
		base.Subscribe<OxidizerTank>(-887025858, OxidizerTank.OnRocketLandedDelegate);
		base.Subscribe<OxidizerTank>(-1697596308, OxidizerTank.OnStorageChangeDelegate);
	}

	// Token: 0x06005263 RID: 21091 RVA: 0x001D88C4 File Offset: 0x001D6AC4
	public float GetTotalOxidizerAvailable()
	{
		float num = 0f;
		foreach (Tag tag in this.oxidizerTypes)
		{
			num += this.storage.GetAmountAvailable(tag);
		}
		return num;
	}

	// Token: 0x06005264 RID: 21092 RVA: 0x001D8904 File Offset: 0x001D6B04
	public Dictionary<Tag, float> GetOxidizersAvailable()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (Tag tag in this.oxidizerTypes)
		{
			dictionary[tag] = this.storage.GetAmountAvailable(tag);
		}
		return dictionary;
	}

	// Token: 0x06005265 RID: 21093 RVA: 0x001D8948 File Offset: 0x001D6B48
	private void OnStorageChange(object data)
	{
		this.RefreshMeter();
	}

	// Token: 0x06005266 RID: 21094 RVA: 0x001D8950 File Offset: 0x001D6B50
	private void OnStorageCapacityChanged(float newCapacity)
	{
		this.RefreshMeter();
	}

	// Token: 0x06005267 RID: 21095 RVA: 0x001D8958 File Offset: 0x001D6B58
	private void RefreshMeter()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x06005268 RID: 21096 RVA: 0x001D896D File Offset: 0x001D6B6D
	private void OnRocketLanded(object data)
	{
		if (this.consumeOnLand)
		{
			this.storage.ConsumeAllIgnoringDisease();
		}
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x06005269 RID: 21097 RVA: 0x001D8998 File Offset: 0x001D6B98
	private void OnCopySettings(object data)
	{
		OxidizerTank component = ((GameObject)data).GetComponent<OxidizerTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x0600526A RID: 21098 RVA: 0x001D89C8 File Offset: 0x001D6BC8
	[ContextMenu("Fill Tank")]
	public void DEBUG_FillTank(SimHashes element)
	{
		base.GetComponent<FlatTagFilterable>().selectedTags.Add(element.CreateTag());
		if (ElementLoader.FindElementByHash(element).IsLiquid)
		{
			this.storage.AddLiquid(element, this.targetFillMass, ElementLoader.FindElementByHash(element).defaultValues.temperature, 0, 0, false, true);
			return;
		}
		if (ElementLoader.FindElementByHash(element).IsSolid)
		{
			GameObject go = ElementLoader.FindElementByHash(element).substance.SpawnResource(base.gameObject.transform.GetPosition(), this.targetFillMass, 300f, byte.MaxValue, 0, false, false, false);
			this.storage.Store(go, false, false, true, false);
		}
	}

	// Token: 0x0600526B RID: 21099 RVA: 0x001D8A74 File Offset: 0x001D6C74
	public OxidizerTank()
	{
		Tag[] array2;
		if (!DlcManager.IsExpansion1Active())
		{
			Tag[] array = new Tag[2];
			array[0] = SimHashes.OxyRock.CreateTag();
			array2 = array;
			array[1] = SimHashes.LiquidOxygen.CreateTag();
		}
		else
		{
			Tag[] array3 = new Tag[3];
			array3[0] = SimHashes.OxyRock.CreateTag();
			array3[1] = SimHashes.LiquidOxygen.CreateTag();
			array2 = array3;
			array3[2] = SimHashes.Fertilizer.CreateTag();
		}
		this.oxidizerTypes = array2;
		base..ctor();
	}

	// Token: 0x04003650 RID: 13904
	public Storage storage;

	// Token: 0x04003651 RID: 13905
	public bool supportsMultipleOxidizers;

	// Token: 0x04003652 RID: 13906
	private MeterController meter;

	// Token: 0x04003653 RID: 13907
	private bool isSuspended;

	// Token: 0x04003654 RID: 13908
	public bool consumeOnLand = true;

	// Token: 0x04003655 RID: 13909
	[Serialize]
	public float maxFillMass;

	// Token: 0x04003656 RID: 13910
	[Serialize]
	public float targetFillMass;

	// Token: 0x04003657 RID: 13911
	public List<SimHashes> discoverResourcesOnSpawn;

	// Token: 0x04003658 RID: 13912
	[SerializeField]
	private Tag[] oxidizerTypes;

	// Token: 0x04003659 RID: 13913
	private FilteredStorage filteredStorage;

	// Token: 0x0400365A RID: 13914
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400365B RID: 13915
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x0400365C RID: 13916
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnStorageChange(data);
	});
}
