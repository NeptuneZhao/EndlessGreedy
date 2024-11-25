using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B24 RID: 2852
public class StructureToStructureTemperature : KMonoBehaviour
{
	// Token: 0x06005501 RID: 21761 RVA: 0x001E6028 File Offset: 0x001E4228
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<StructureToStructureTemperature>(-1555603773, StructureToStructureTemperature.OnStructureTemperatureRegisteredDelegate);
	}

	// Token: 0x06005502 RID: 21762 RVA: 0x001E6041 File Offset: 0x001E4241
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.DefineConductiveCells();
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
	}

	// Token: 0x06005503 RID: 21763 RVA: 0x001E606F File Offset: 0x001E426F
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
		this.UnregisterToSIM();
		base.OnCleanUp();
	}

	// Token: 0x06005504 RID: 21764 RVA: 0x001E60A0 File Offset: 0x001E42A0
	private void OnStructureTemperatureRegistered(object _sim_handle)
	{
		int sim_handle = (int)_sim_handle;
		this.RegisterToSIM(sim_handle);
	}

	// Token: 0x06005505 RID: 21765 RVA: 0x001E60BC File Offset: 0x001E42BC
	private void RegisterToSIM(int sim_handle)
	{
		string name = this.building.Def.Name;
		SimMessages.RegisterBuildingToBuildingHeatExchange(sim_handle2, Game.Instance.simComponentCallbackManager.Add(delegate(int sim_handle, object callback_data)
		{
			this.OnSimRegistered(sim_handle);
		}, null, "StructureToStructureTemperature.SimRegister").index);
	}

	// Token: 0x06005506 RID: 21766 RVA: 0x001E6109 File Offset: 0x001E4309
	private void OnSimRegistered(int sim_handle)
	{
		if (sim_handle != -1)
		{
			this.selfHandle = sim_handle;
			this.hasBeenRegister = true;
			if (this.buildingDestroyed)
			{
				this.UnregisterToSIM();
				return;
			}
			this.Refresh_InContactBuildings();
		}
	}

	// Token: 0x06005507 RID: 21767 RVA: 0x001E6132 File Offset: 0x001E4332
	private void UnregisterToSIM()
	{
		if (this.hasBeenRegister)
		{
			SimMessages.RemoveBuildingToBuildingHeatExchange(this.selfHandle, -1);
		}
		this.buildingDestroyed = true;
	}

	// Token: 0x06005508 RID: 21768 RVA: 0x001E6150 File Offset: 0x001E4350
	private void DefineConductiveCells()
	{
		this.conductiveCells = new List<int>(this.building.PlacementCells);
		this.conductiveCells.Remove(this.building.GetUtilityInputCell());
		this.conductiveCells.Remove(this.building.GetUtilityOutputCell());
	}

	// Token: 0x06005509 RID: 21769 RVA: 0x001E61A1 File Offset: 0x001E43A1
	private void Add(StructureToStructureTemperature.InContactBuildingData buildingData)
	{
		if (this.inContactBuildings.Add(buildingData.buildingInContact))
		{
			SimMessages.AddBuildingToBuildingHeatExchange(this.selfHandle, buildingData.buildingInContact, buildingData.cellsInContact);
		}
	}

	// Token: 0x0600550A RID: 21770 RVA: 0x001E61CD File Offset: 0x001E43CD
	private void Remove(int building)
	{
		if (this.inContactBuildings.Contains(building))
		{
			this.inContactBuildings.Remove(building);
			SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchange(this.selfHandle, building);
		}
	}

	// Token: 0x0600550B RID: 21771 RVA: 0x001E61F8 File Offset: 0x001E43F8
	private void OnAnyBuildingChanged(int _cell, object _data)
	{
		if (this.hasBeenRegister)
		{
			StructureToStructureTemperature.BuildingChangedObj buildingChangedObj = (StructureToStructureTemperature.BuildingChangedObj)_data;
			bool flag = false;
			int num = 0;
			for (int i = 0; i < buildingChangedObj.building.PlacementCells.Length; i++)
			{
				int item = buildingChangedObj.building.PlacementCells[i];
				if (this.conductiveCells.Contains(item))
				{
					flag = true;
					num++;
				}
			}
			if (flag)
			{
				int simHandler = buildingChangedObj.simHandler;
				StructureToStructureTemperature.BuildingChangeType changeType = buildingChangedObj.changeType;
				if (changeType == StructureToStructureTemperature.BuildingChangeType.Created)
				{
					StructureToStructureTemperature.InContactBuildingData buildingData = new StructureToStructureTemperature.InContactBuildingData
					{
						buildingInContact = simHandler,
						cellsInContact = num
					};
					this.Add(buildingData);
					return;
				}
				if (changeType != StructureToStructureTemperature.BuildingChangeType.Destroyed)
				{
					return;
				}
				this.Remove(simHandler);
			}
		}
	}

	// Token: 0x0600550C RID: 21772 RVA: 0x001E62A4 File Offset: 0x001E44A4
	private void Refresh_InContactBuildings()
	{
		foreach (StructureToStructureTemperature.InContactBuildingData buildingData in this.GetAll_InContact_Buildings())
		{
			this.Add(buildingData);
		}
	}

	// Token: 0x0600550D RID: 21773 RVA: 0x001E62F8 File Offset: 0x001E44F8
	private List<StructureToStructureTemperature.InContactBuildingData> GetAll_InContact_Buildings()
	{
		Dictionary<Building, int> dictionary = new Dictionary<Building, int>();
		List<StructureToStructureTemperature.InContactBuildingData> list = new List<StructureToStructureTemperature.InContactBuildingData>();
		List<GameObject> buildingsInCell = new List<GameObject>();
		using (List<int>.Enumerator enumerator = this.conductiveCells.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int cell = enumerator.Current;
				buildingsInCell.Clear();
				Action<int> action = delegate(int layer)
				{
					GameObject gameObject = Grid.Objects[cell, layer];
					if (gameObject != null && !buildingsInCell.Contains(gameObject))
					{
						buildingsInCell.Add(gameObject);
					}
				};
				action(1);
				action(26);
				action(27);
				action(31);
				action(32);
				action(30);
				action(12);
				action(13);
				action(16);
				action(17);
				action(24);
				action(2);
				for (int i = 0; i < buildingsInCell.Count; i++)
				{
					Building building = (buildingsInCell[i] == null) ? null : buildingsInCell[i].GetComponent<Building>();
					if (building != null && building.Def.UseStructureTemperature && building.PlacementCellsContainCell(cell))
					{
						if (!dictionary.ContainsKey(building))
						{
							dictionary.Add(building, 0);
						}
						Dictionary<Building, int> dictionary2 = dictionary;
						Building key = building;
						int num = dictionary2[key];
						dictionary2[key] = num + 1;
					}
				}
			}
		}
		foreach (Building building2 in dictionary.Keys)
		{
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(building2);
			if (handle != HandleVector<int>.InvalidHandle)
			{
				int simHandleCopy = GameComps.StructureTemperatures.GetPayload(handle).simHandleCopy;
				StructureToStructureTemperature.InContactBuildingData item = new StructureToStructureTemperature.InContactBuildingData
				{
					buildingInContact = simHandleCopy,
					cellsInContact = dictionary[building2]
				};
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x040037AC RID: 14252
	[MyCmpGet]
	private Building building;

	// Token: 0x040037AD RID: 14253
	private List<int> conductiveCells;

	// Token: 0x040037AE RID: 14254
	private HashSet<int> inContactBuildings = new HashSet<int>();

	// Token: 0x040037AF RID: 14255
	private bool hasBeenRegister;

	// Token: 0x040037B0 RID: 14256
	private bool buildingDestroyed;

	// Token: 0x040037B1 RID: 14257
	private int selfHandle;

	// Token: 0x040037B2 RID: 14258
	protected static readonly EventSystem.IntraObjectHandler<StructureToStructureTemperature> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<StructureToStructureTemperature>(delegate(StructureToStructureTemperature component, object data)
	{
		component.OnStructureTemperatureRegistered(data);
	});

	// Token: 0x02001B73 RID: 7027
	public enum BuildingChangeType
	{
		// Token: 0x04007FCA RID: 32714
		Created,
		// Token: 0x04007FCB RID: 32715
		Destroyed,
		// Token: 0x04007FCC RID: 32716
		Moved
	}

	// Token: 0x02001B74 RID: 7028
	public struct InContactBuildingData
	{
		// Token: 0x04007FCD RID: 32717
		public int buildingInContact;

		// Token: 0x04007FCE RID: 32718
		public int cellsInContact;
	}

	// Token: 0x02001B75 RID: 7029
	public struct BuildingChangedObj
	{
		// Token: 0x0600A372 RID: 41842 RVA: 0x00389D94 File Offset: 0x00387F94
		public BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType _changeType, Building _building, int sim_handler)
		{
			this.changeType = _changeType;
			this.building = _building;
			this.simHandler = sim_handler;
		}

		// Token: 0x04007FCF RID: 32719
		public StructureToStructureTemperature.BuildingChangeType changeType;

		// Token: 0x04007FD0 RID: 32720
		public int simHandler;

		// Token: 0x04007FD1 RID: 32721
		public Building building;
	}
}
