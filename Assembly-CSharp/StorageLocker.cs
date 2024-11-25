using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000774 RID: 1908
[AddComponentMenu("KMonoBehaviour/scripts/StorageLocker")]
public class StorageLocker : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x0600338D RID: 13197 RVA: 0x0011AB7F File Offset: 0x00118D7F
	protected override void OnPrefabInit()
	{
		this.Initialize(false);
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x0011AB88 File Offset: 0x00118D88
	protected void Initialize(bool use_logic_meter)
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("StorageLocker", 35);
		ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
		this.filteredStorage = new FilteredStorage(this, null, this, use_logic_meter, fetch_chore_type);
		base.Subscribe<StorageLocker>(-905833192, StorageLocker.OnCopySettingsDelegate);
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x0011ABE4 File Offset: 0x00118DE4
	protected override void OnSpawn()
	{
		this.filteredStorage.FilterChanged();
		if (this.nameable != null && !this.lockerName.IsNullOrWhiteSpace())
		{
			this.nameable.SetName(this.lockerName);
		}
		base.Trigger(-1683615038, null);
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x0011AC34 File Offset: 0x00118E34
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x06003391 RID: 13201 RVA: 0x0011AC44 File Offset: 0x00118E44
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		StorageLocker component = gameObject.GetComponent<StorageLocker>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06003392 RID: 13202 RVA: 0x0011AC7F File Offset: 0x00118E7F
	public void UpdateForbiddenTag(Tag game_tag, bool forbidden)
	{
		if (forbidden)
		{
			this.filteredStorage.RemoveForbiddenTag(game_tag);
			return;
		}
		this.filteredStorage.AddForbiddenTag(game_tag);
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06003393 RID: 13203 RVA: 0x0011AC9D File Offset: 0x00118E9D
	// (set) Token: 0x06003394 RID: 13204 RVA: 0x0011ACB5 File Offset: 0x00118EB5
	public virtual float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, base.GetComponent<Storage>().capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06003395 RID: 13205 RVA: 0x0011ACC9 File Offset: 0x00118EC9
	public float AmountStored
	{
		get
		{
			return base.GetComponent<Storage>().MassStored();
		}
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06003396 RID: 13206 RVA: 0x0011ACD6 File Offset: 0x00118ED6
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06003397 RID: 13207 RVA: 0x0011ACDD File Offset: 0x00118EDD
	public float MaxCapacity
	{
		get
		{
			return base.GetComponent<Storage>().capacityKg;
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06003398 RID: 13208 RVA: 0x0011ACEA File Offset: 0x00118EEA
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06003399 RID: 13209 RVA: 0x0011ACED File Offset: 0x00118EED
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x04001E8C RID: 7820
	private LoggerFS log;

	// Token: 0x04001E8D RID: 7821
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04001E8E RID: 7822
	[Serialize]
	public string lockerName = "";

	// Token: 0x04001E8F RID: 7823
	protected FilteredStorage filteredStorage;

	// Token: 0x04001E90 RID: 7824
	[MyCmpGet]
	private UserNameable nameable;

	// Token: 0x04001E91 RID: 7825
	public string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;

	// Token: 0x04001E92 RID: 7826
	private static readonly EventSystem.IntraObjectHandler<StorageLocker> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<StorageLocker>(delegate(StorageLocker component, object data)
	{
		component.OnCopySettings(data);
	});
}
