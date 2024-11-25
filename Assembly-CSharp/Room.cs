using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A6D RID: 2669
public class Room : IAssignableIdentity
{
	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x06004DAB RID: 19883 RVA: 0x001BD3BB File Offset: 0x001BB5BB
	public List<KPrefabID> buildings
	{
		get
		{
			return this.cavity.buildings;
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x06004DAC RID: 19884 RVA: 0x001BD3C8 File Offset: 0x001BB5C8
	public List<KPrefabID> plants
	{
		get
		{
			return this.cavity.plants;
		}
	}

	// Token: 0x06004DAD RID: 19885 RVA: 0x001BD3D5 File Offset: 0x001BB5D5
	public string GetProperName()
	{
		return this.roomType.Name;
	}

	// Token: 0x06004DAE RID: 19886 RVA: 0x001BD3E4 File Offset: 0x001BB5E4
	public List<Ownables> GetOwners()
	{
		this.current_owners.Clear();
		foreach (KPrefabID kprefabID in this.GetPrimaryEntities())
		{
			if (kprefabID != null)
			{
				Ownable component = kprefabID.GetComponent<Ownable>();
				if (component != null && component.assignee != null && component.assignee != this)
				{
					foreach (Ownables item in component.assignee.GetOwners())
					{
						if (!this.current_owners.Contains(item))
						{
							this.current_owners.Add(item);
						}
					}
				}
			}
		}
		return this.current_owners;
	}

	// Token: 0x06004DAF RID: 19887 RVA: 0x001BD4D0 File Offset: 0x001BB6D0
	public List<GameObject> GetBuildingsOnFloor()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.buildings.Count; i++)
		{
			if (!Grid.Solid[Grid.PosToCell(this.buildings[i])] && Grid.Solid[Grid.CellBelow(Grid.PosToCell(this.buildings[i]))])
			{
				list.Add(this.buildings[i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06004DB0 RID: 19888 RVA: 0x001BD550 File Offset: 0x001BB750
	public Ownables GetSoleOwner()
	{
		List<Ownables> owners = this.GetOwners();
		if (owners.Count <= 0)
		{
			return null;
		}
		return owners[0];
	}

	// Token: 0x06004DB1 RID: 19889 RVA: 0x001BD578 File Offset: 0x001BB778
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Find((Ownables x) => x == owner) != null;
	}

	// Token: 0x06004DB2 RID: 19890 RVA: 0x001BD5AF File Offset: 0x001BB7AF
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x06004DB3 RID: 19891 RVA: 0x001BD5BC File Offset: 0x001BB7BC
	public List<KPrefabID> GetPrimaryEntities()
	{
		this.primary_buildings.Clear();
		RoomType roomType = this.roomType;
		if (roomType.primary_constraint != null)
		{
			foreach (KPrefabID kprefabID in this.buildings)
			{
				if (kprefabID != null && roomType.primary_constraint.building_criteria(kprefabID))
				{
					this.primary_buildings.Add(kprefabID);
				}
			}
			foreach (KPrefabID kprefabID2 in this.plants)
			{
				if (kprefabID2 != null && roomType.primary_constraint.building_criteria(kprefabID2))
				{
					this.primary_buildings.Add(kprefabID2);
				}
			}
		}
		return this.primary_buildings;
	}

	// Token: 0x06004DB4 RID: 19892 RVA: 0x001BD6B8 File Offset: 0x001BB8B8
	public void RetriggerBuildings()
	{
		foreach (KPrefabID kprefabID in this.buildings)
		{
			if (!(kprefabID == null))
			{
				kprefabID.Trigger(144050788, this);
			}
		}
		foreach (KPrefabID kprefabID2 in this.plants)
		{
			if (!(kprefabID2 == null))
			{
				kprefabID2.Trigger(144050788, this);
			}
		}
	}

	// Token: 0x06004DB5 RID: 19893 RVA: 0x001BD76C File Offset: 0x001BB96C
	public bool IsNull()
	{
		return false;
	}

	// Token: 0x06004DB6 RID: 19894 RVA: 0x001BD76F File Offset: 0x001BB96F
	public void CleanUp()
	{
		Game.Instance.assignmentManager.RemoveFromAllGroups(this);
	}

	// Token: 0x04003380 RID: 13184
	public CavityInfo cavity;

	// Token: 0x04003381 RID: 13185
	public RoomType roomType;

	// Token: 0x04003382 RID: 13186
	private List<KPrefabID> primary_buildings = new List<KPrefabID>();

	// Token: 0x04003383 RID: 13187
	private List<Ownables> current_owners = new List<Ownables>();
}
