using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200075A RID: 1882
[AddComponentMenu("KMonoBehaviour/scripts/Refrigerator")]
public class Refrigerator : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x06003267 RID: 12903 RVA: 0x00114E59 File Offset: 0x00113059
	protected override void OnPrefabInit()
	{
		this.filteredStorage = new FilteredStorage(this, new Tag[]
		{
			GameTags.Compostable
		}, this, true, Db.Get().ChoreTypes.FoodFetch);
	}

	// Token: 0x06003268 RID: 12904 RVA: 0x00114E8C File Offset: 0x0011308C
	protected override void OnSpawn()
	{
		base.GetComponent<KAnimControllerBase>().Play("off", KAnim.PlayMode.Once, 1f, 0f);
		FoodStorage component = base.GetComponent<FoodStorage>();
		component.FilteredStorage = this.filteredStorage;
		component.SpicedFoodOnly = component.SpicedFoodOnly;
		this.filteredStorage.FilterChanged();
		this.UpdateLogicCircuit();
		base.Subscribe<Refrigerator>(-905833192, Refrigerator.OnCopySettingsDelegate);
		base.Subscribe<Refrigerator>(-1697596308, Refrigerator.UpdateLogicCircuitCBDelegate);
		base.Subscribe<Refrigerator>(-592767678, Refrigerator.UpdateLogicCircuitCBDelegate);
	}

	// Token: 0x06003269 RID: 12905 RVA: 0x00114F1A File Offset: 0x0011311A
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x0600326A RID: 12906 RVA: 0x00114F27 File Offset: 0x00113127
	public bool IsActive()
	{
		return this.operational.IsActive;
	}

	// Token: 0x0600326B RID: 12907 RVA: 0x00114F34 File Offset: 0x00113134
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		Refrigerator component = gameObject.GetComponent<Refrigerator>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x0600326C RID: 12908 RVA: 0x00114F6F File Offset: 0x0011316F
	// (set) Token: 0x0600326D RID: 12909 RVA: 0x00114F87 File Offset: 0x00113187
	public float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
			this.UpdateLogicCircuit();
		}
	}

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x0600326E RID: 12910 RVA: 0x00114FA1 File Offset: 0x001131A1
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x0600326F RID: 12911 RVA: 0x00114FAE File Offset: 0x001131AE
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x06003270 RID: 12912 RVA: 0x00114FB5 File Offset: 0x001131B5
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x17000353 RID: 851
	// (get) Token: 0x06003271 RID: 12913 RVA: 0x00114FC2 File Offset: 0x001131C2
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06003272 RID: 12914 RVA: 0x00114FC5 File Offset: 0x001131C5
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x06003273 RID: 12915 RVA: 0x00114FCD File Offset: 0x001131CD
	private void UpdateLogicCircuitCB(object data)
	{
		this.UpdateLogicCircuit();
	}

	// Token: 0x06003274 RID: 12916 RVA: 0x00114FD8 File Offset: 0x001131D8
	private void UpdateLogicCircuit()
	{
		bool flag = this.filteredStorage.IsFull();
		bool isOperational = this.operational.IsOperational;
		bool flag2 = flag && isOperational;
		this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, flag2 ? 1 : 0);
		this.filteredStorage.SetLogicMeter(flag2);
	}

	// Token: 0x04001DD1 RID: 7633
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001DD2 RID: 7634
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001DD3 RID: 7635
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x04001DD4 RID: 7636
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04001DD5 RID: 7637
	private FilteredStorage filteredStorage;

	// Token: 0x04001DD6 RID: 7638
	private static readonly EventSystem.IntraObjectHandler<Refrigerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Refrigerator>(delegate(Refrigerator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001DD7 RID: 7639
	private static readonly EventSystem.IntraObjectHandler<Refrigerator> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<Refrigerator>(delegate(Refrigerator component, object data)
	{
		component.UpdateLogicCircuitCB(data);
	});
}
