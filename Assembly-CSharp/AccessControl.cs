using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A14 RID: 2580
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/AccessControl")]
public class AccessControl : KMonoBehaviour, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x06004ACD RID: 19149 RVA: 0x001AB95B File Offset: 0x001A9B5B
	// (set) Token: 0x06004ACE RID: 19150 RVA: 0x001AB963 File Offset: 0x001A9B63
	public AccessControl.Permission DefaultPermission
	{
		get
		{
			return this._defaultPermission;
		}
		set
		{
			this._defaultPermission = value;
			this.SetStatusItem();
			this.SetGridRestrictions(null, this._defaultPermission);
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x06004ACF RID: 19151 RVA: 0x001AB97F File Offset: 0x001A9B7F
	public bool Online
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004AD0 RID: 19152 RVA: 0x001AB984 File Offset: 0x001A9B84
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (AccessControl.accessControlActive == null)
		{
			AccessControl.accessControlActive = new StatusItem("accessControlActive", BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.NAME, BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
		}
		base.Subscribe<AccessControl>(279163026, AccessControl.OnControlStateChangedDelegate);
		base.Subscribe<AccessControl>(-905833192, AccessControl.OnCopySettingsDelegate);
	}

	// Token: 0x06004AD1 RID: 19153 RVA: 0x001AB9F8 File Offset: 0x001A9BF8
	private void CheckForBadData()
	{
		List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> list = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> item in this.savedPermissions)
		{
			if (item.Key.Get() == null)
			{
				list.Add(item);
			}
		}
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> item2 in list)
		{
			this.savedPermissions.Remove(item2);
		}
	}

	// Token: 0x06004AD2 RID: 19154 RVA: 0x001ABAA8 File Offset: 0x001A9CA8
	protected override void OnSpawn()
	{
		this.isTeleporter = (base.GetComponent<NavTeleporter>() != null);
		base.OnSpawn();
		if (this.savedPermissions.Count > 0)
		{
			this.CheckForBadData();
		}
		if (this.registered)
		{
			this.RegisterInGrid(true);
			this.RestorePermissions();
		}
		ListPool<global::Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.PooledList pooledList = ListPool<global::Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.Allocate();
		for (int i = this.savedPermissions.Count - 1; i >= 0; i--)
		{
			KPrefabID kprefabID = this.savedPermissions[i].Key.Get();
			if (kprefabID != null)
			{
				MinionIdentity component = kprefabID.GetComponent<MinionIdentity>();
				if (component != null)
				{
					pooledList.Add(new global::Tuple<MinionAssignablesProxy, AccessControl.Permission>(component.assignableProxy.Get(), this.savedPermissions[i].Value));
					this.savedPermissions.RemoveAt(i);
					this.ClearGridRestrictions(kprefabID);
				}
			}
		}
		foreach (global::Tuple<MinionAssignablesProxy, AccessControl.Permission> tuple in pooledList)
		{
			this.SetPermission(tuple.first, tuple.second);
		}
		pooledList.Recycle();
		this.SetStatusItem();
	}

	// Token: 0x06004AD3 RID: 19155 RVA: 0x001ABBE4 File Offset: 0x001A9DE4
	protected override void OnCleanUp()
	{
		this.RegisterInGrid(false);
		base.OnCleanUp();
	}

	// Token: 0x06004AD4 RID: 19156 RVA: 0x001ABBF3 File Offset: 0x001A9DF3
	private void OnControlStateChanged(object data)
	{
		this.overrideAccess = (Door.ControlState)data;
	}

	// Token: 0x06004AD5 RID: 19157 RVA: 0x001ABC04 File Offset: 0x001A9E04
	private void OnCopySettings(object data)
	{
		AccessControl component = ((GameObject)data).GetComponent<AccessControl>();
		if (component != null)
		{
			this.savedPermissions.Clear();
			foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair in component.savedPermissions)
			{
				if (keyValuePair.Key.Get() != null)
				{
					this.SetPermission(keyValuePair.Key.Get().GetComponent<MinionAssignablesProxy>(), keyValuePair.Value);
				}
			}
			this._defaultPermission = component._defaultPermission;
			this.SetGridRestrictions(null, this.DefaultPermission);
		}
	}

	// Token: 0x06004AD6 RID: 19158 RVA: 0x001ABCC0 File Offset: 0x001A9EC0
	public void SetRegistered(bool newRegistered)
	{
		if (newRegistered && !this.registered)
		{
			this.RegisterInGrid(true);
			this.RestorePermissions();
			return;
		}
		if (!newRegistered && this.registered)
		{
			this.RegisterInGrid(false);
		}
	}

	// Token: 0x06004AD7 RID: 19159 RVA: 0x001ABCF0 File Offset: 0x001A9EF0
	public void SetPermission(MinionAssignablesProxy key, AccessControl.Permission permission)
	{
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.savedPermissions.Count; i++)
		{
			if (this.savedPermissions[i].Key.GetId() == component.InstanceID)
			{
				flag = true;
				KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair = this.savedPermissions[i];
				this.savedPermissions[i] = new KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>(keyValuePair.Key, permission);
				break;
			}
		}
		if (!flag)
		{
			this.savedPermissions.Add(new KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>(new Ref<KPrefabID>(component), permission));
		}
		this.SetStatusItem();
		this.SetGridRestrictions(component, permission);
	}

	// Token: 0x06004AD8 RID: 19160 RVA: 0x001ABD9C File Offset: 0x001A9F9C
	private void RestorePermissions()
	{
		this.SetGridRestrictions(null, this.DefaultPermission);
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair in this.savedPermissions)
		{
			KPrefabID x = keyValuePair.Key.Get();
			if (x == null)
			{
				DebugUtil.Assert(x == null, "Tried to set a duplicant-specific access restriction with a null key! This will result in an invisible default permission!");
			}
			this.SetGridRestrictions(keyValuePair.Key.Get(), keyValuePair.Value);
		}
	}

	// Token: 0x06004AD9 RID: 19161 RVA: 0x001ABE38 File Offset: 0x001AA038
	private void RegisterInGrid(bool register)
	{
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		if (register)
		{
			Rotatable component3 = base.GetComponent<Rotatable>();
			Grid.Restriction.Orientation orientation;
			if (!this.isTeleporter)
			{
				orientation = ((component3 == null || component3.GetOrientation() == Orientation.Neutral) ? Grid.Restriction.Orientation.Vertical : Grid.Restriction.Orientation.Horizontal);
			}
			else
			{
				orientation = Grid.Restriction.Orientation.SingleCell;
			}
			if (component != null)
			{
				this.registeredBuildingCells = component.PlacementCells;
				int[] array = this.registeredBuildingCells;
				for (int i = 0; i < array.Length; i++)
				{
					Grid.RegisterRestriction(array[i], orientation);
				}
			}
			else
			{
				foreach (CellOffset offset in component2.OccupiedCellsOffsets)
				{
					Grid.RegisterRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), orientation);
				}
			}
			if (this.isTeleporter)
			{
				Grid.RegisterRestriction(base.GetComponent<NavTeleporter>().GetCell(), orientation);
			}
		}
		else
		{
			if (component != null)
			{
				if (component.GetMyWorldId() != 255 && this.registeredBuildingCells != null)
				{
					int[] array = this.registeredBuildingCells;
					for (int i = 0; i < array.Length; i++)
					{
						Grid.UnregisterRestriction(array[i]);
					}
					this.registeredBuildingCells = null;
				}
			}
			else
			{
				foreach (CellOffset offset2 in component2.OccupiedCellsOffsets)
				{
					Grid.UnregisterRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset2));
				}
			}
			if (this.isTeleporter)
			{
				int cell = base.GetComponent<NavTeleporter>().GetCell();
				if (cell != Grid.InvalidCell)
				{
					Grid.UnregisterRestriction(cell);
				}
			}
		}
		this.registered = register;
	}

	// Token: 0x06004ADA RID: 19162 RVA: 0x001ABFDC File Offset: 0x001AA1DC
	private void SetGridRestrictions(KPrefabID kpid, AccessControl.Permission permission)
	{
		if (!this.registered || !base.isSpawned)
		{
			return;
		}
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		int minionInstanceID = (kpid != null) ? kpid.InstanceID : -1;
		Grid.Restriction.Directions directions = (Grid.Restriction.Directions)0;
		switch (permission)
		{
		case AccessControl.Permission.Both:
			directions = (Grid.Restriction.Directions)0;
			break;
		case AccessControl.Permission.GoLeft:
			directions = Grid.Restriction.Directions.Right;
			break;
		case AccessControl.Permission.GoRight:
			directions = Grid.Restriction.Directions.Left;
			break;
		case AccessControl.Permission.Neither:
			directions = (Grid.Restriction.Directions.Left | Grid.Restriction.Directions.Right);
			break;
		}
		if (this.isTeleporter)
		{
			if (directions != (Grid.Restriction.Directions)0)
			{
				directions = Grid.Restriction.Directions.Teleport;
			}
			else
			{
				directions = (Grid.Restriction.Directions)0;
			}
		}
		if (component != null)
		{
			int[] array = this.registeredBuildingCells;
			for (int i = 0; i < array.Length; i++)
			{
				Grid.SetRestriction(array[i], minionInstanceID, directions);
			}
		}
		else
		{
			foreach (CellOffset offset in component2.OccupiedCellsOffsets)
			{
				Grid.SetRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), minionInstanceID, directions);
			}
		}
		if (this.isTeleporter)
		{
			Grid.SetRestriction(base.GetComponent<NavTeleporter>().GetCell(), minionInstanceID, directions);
		}
	}

	// Token: 0x06004ADB RID: 19163 RVA: 0x001AC0F0 File Offset: 0x001AA2F0
	private void ClearGridRestrictions(KPrefabID kpid)
	{
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		int minionInstanceID = (kpid != null) ? kpid.InstanceID : -1;
		if (component != null)
		{
			int[] array = this.registeredBuildingCells;
			for (int i = 0; i < array.Length; i++)
			{
				Grid.ClearRestriction(array[i], minionInstanceID);
			}
			return;
		}
		foreach (CellOffset offset in component2.OccupiedCellsOffsets)
		{
			Grid.ClearRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), minionInstanceID);
		}
	}

	// Token: 0x06004ADC RID: 19164 RVA: 0x001AC198 File Offset: 0x001AA398
	public AccessControl.Permission GetPermission(Navigator minion)
	{
		Door.ControlState controlState = this.overrideAccess;
		if (controlState == Door.ControlState.Opened)
		{
			return AccessControl.Permission.Both;
		}
		if (controlState == Door.ControlState.Locked)
		{
			return AccessControl.Permission.Neither;
		}
		return this.GetSetPermission(this.GetKeyForNavigator(minion));
	}

	// Token: 0x06004ADD RID: 19165 RVA: 0x001AC1C5 File Offset: 0x001AA3C5
	private MinionAssignablesProxy GetKeyForNavigator(Navigator minion)
	{
		return minion.GetComponent<MinionIdentity>().assignableProxy.Get();
	}

	// Token: 0x06004ADE RID: 19166 RVA: 0x001AC1D7 File Offset: 0x001AA3D7
	public AccessControl.Permission GetSetPermission(MinionAssignablesProxy key)
	{
		return this.GetSetPermission(key.GetComponent<KPrefabID>());
	}

	// Token: 0x06004ADF RID: 19167 RVA: 0x001AC1E8 File Offset: 0x001AA3E8
	private AccessControl.Permission GetSetPermission(KPrefabID kpid)
	{
		AccessControl.Permission result = this.DefaultPermission;
		if (kpid != null)
		{
			for (int i = 0; i < this.savedPermissions.Count; i++)
			{
				if (this.savedPermissions[i].Key.GetId() == kpid.InstanceID)
				{
					result = this.savedPermissions[i].Value;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06004AE0 RID: 19168 RVA: 0x001AC254 File Offset: 0x001AA454
	public void ClearPermission(MinionAssignablesProxy key)
	{
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component != null)
		{
			for (int i = 0; i < this.savedPermissions.Count; i++)
			{
				if (this.savedPermissions[i].Key.GetId() == component.InstanceID)
				{
					this.savedPermissions.RemoveAt(i);
					break;
				}
			}
		}
		this.SetStatusItem();
		this.ClearGridRestrictions(component);
	}

	// Token: 0x06004AE1 RID: 19169 RVA: 0x001AC2C4 File Offset: 0x001AA4C4
	public bool IsDefaultPermission(MinionAssignablesProxy key)
	{
		bool flag = false;
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component != null)
		{
			for (int i = 0; i < this.savedPermissions.Count; i++)
			{
				if (this.savedPermissions[i].Key.GetId() == component.InstanceID)
				{
					flag = true;
					break;
				}
			}
		}
		return !flag;
	}

	// Token: 0x06004AE2 RID: 19170 RVA: 0x001AC324 File Offset: 0x001AA524
	private void SetStatusItem()
	{
		if (this._defaultPermission != AccessControl.Permission.Both || this.savedPermissions.Count > 0)
		{
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, AccessControl.accessControlActive, null);
			return;
		}
		this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, null, null);
	}

	// Token: 0x06004AE3 RID: 19171 RVA: 0x001AC388 File Offset: 0x001AA588
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.ACCESS_CONTROL, UI.BUILDINGEFFECTS.TOOLTIPS.ACCESS_CONTROL, Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04003105 RID: 12549
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003106 RID: 12550
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003107 RID: 12551
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003108 RID: 12552
	private bool isTeleporter;

	// Token: 0x04003109 RID: 12553
	private int[] registeredBuildingCells;

	// Token: 0x0400310A RID: 12554
	[Serialize]
	private List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> savedPermissions = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();

	// Token: 0x0400310B RID: 12555
	[Serialize]
	private AccessControl.Permission _defaultPermission;

	// Token: 0x0400310C RID: 12556
	[Serialize]
	public bool registered = true;

	// Token: 0x0400310D RID: 12557
	[Serialize]
	public bool controlEnabled;

	// Token: 0x0400310E RID: 12558
	public Door.ControlState overrideAccess;

	// Token: 0x0400310F RID: 12559
	private static StatusItem accessControlActive;

	// Token: 0x04003110 RID: 12560
	private static readonly EventSystem.IntraObjectHandler<AccessControl> OnControlStateChangedDelegate = new EventSystem.IntraObjectHandler<AccessControl>(delegate(AccessControl component, object data)
	{
		component.OnControlStateChanged(data);
	});

	// Token: 0x04003111 RID: 12561
	private static readonly EventSystem.IntraObjectHandler<AccessControl> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<AccessControl>(delegate(AccessControl component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02001A31 RID: 6705
	public enum Permission
	{
		// Token: 0x04007BB3 RID: 31667
		Both,
		// Token: 0x04007BB4 RID: 31668
		GoLeft,
		// Token: 0x04007BB5 RID: 31669
		GoRight,
		// Token: 0x04007BB6 RID: 31670
		Neither
	}
}
