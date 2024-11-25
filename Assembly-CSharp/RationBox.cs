using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000756 RID: 1878
[AddComponentMenu("KMonoBehaviour/scripts/RationBox")]
public class RationBox : KMonoBehaviour, IUserControlledCapacity, IRender1000ms, IRottable
{
	// Token: 0x0600322D RID: 12845 RVA: 0x00113CDC File Offset: 0x00111EDC
	protected override void OnPrefabInit()
	{
		this.filteredStorage = new FilteredStorage(this, new Tag[]
		{
			GameTags.Compostable
		}, this, false, Db.Get().ChoreTypes.FoodFetch);
		base.Subscribe<RationBox>(-592767678, RationBox.OnOperationalChangedDelegate);
		base.Subscribe<RationBox>(-905833192, RationBox.OnCopySettingsDelegate);
		DiscoveredResources.Instance.Discover("FieldRation".ToTag(), GameTags.Edible);
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x00113D53 File Offset: 0x00111F53
	protected override void OnSpawn()
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(component.IsOperational, false);
		this.filteredStorage.FilterChanged();
	}

	// Token: 0x0600322F RID: 12847 RVA: 0x00113D72 File Offset: 0x00111F72
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x06003230 RID: 12848 RVA: 0x00113D7F File Offset: 0x00111F7F
	private void OnOperationalChanged(object data)
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(component.IsOperational, false);
	}

	// Token: 0x06003231 RID: 12849 RVA: 0x00113D94 File Offset: 0x00111F94
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		RationBox component = gameObject.GetComponent<RationBox>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06003232 RID: 12850 RVA: 0x00113DCF File Offset: 0x00111FCF
	public void Render1000ms(float dt)
	{
		Rottable.SetStatusItems(this);
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06003233 RID: 12851 RVA: 0x00113DD7 File Offset: 0x00111FD7
	// (set) Token: 0x06003234 RID: 12852 RVA: 0x00113DEF File Offset: 0x00111FEF
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
		}
	}

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x06003235 RID: 12853 RVA: 0x00113E03 File Offset: 0x00112003
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06003236 RID: 12854 RVA: 0x00113E10 File Offset: 0x00112010
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06003237 RID: 12855 RVA: 0x00113E17 File Offset: 0x00112017
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06003238 RID: 12856 RVA: 0x00113E24 File Offset: 0x00112024
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x06003239 RID: 12857 RVA: 0x00113E27 File Offset: 0x00112027
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x0600323A RID: 12858 RVA: 0x00113E2F File Offset: 0x0011202F
	public float RotTemperature
	{
		get
		{
			return 277.15f;
		}
	}

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x0600323B RID: 12859 RVA: 0x00113E36 File Offset: 0x00112036
	public float PreserveTemperature
	{
		get
		{
			return 255.15f;
		}
	}

	// Token: 0x0600323E RID: 12862 RVA: 0x00113E86 File Offset: 0x00112086
	GameObject IRottable.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001DB3 RID: 7603
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001DB4 RID: 7604
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04001DB5 RID: 7605
	private FilteredStorage filteredStorage;

	// Token: 0x04001DB6 RID: 7606
	private static readonly EventSystem.IntraObjectHandler<RationBox> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<RationBox>(delegate(RationBox component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001DB7 RID: 7607
	private static readonly EventSystem.IntraObjectHandler<RationBox> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<RationBox>(delegate(RationBox component, object data)
	{
		component.OnCopySettings(data);
	});
}
