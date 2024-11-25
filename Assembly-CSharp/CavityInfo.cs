using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A71 RID: 2673
public class CavityInfo
{
	// Token: 0x06004DD2 RID: 19922 RVA: 0x001BF230 File Offset: 0x001BD430
	public CavityInfo()
	{
		this.handle = HandleVector<int>.InvalidHandle;
		this.dirty = true;
	}

	// Token: 0x06004DD3 RID: 19923 RVA: 0x001BF281 File Offset: 0x001BD481
	public void AddBuilding(KPrefabID bc)
	{
		this.buildings.Add(bc);
		this.dirty = true;
	}

	// Token: 0x06004DD4 RID: 19924 RVA: 0x001BF296 File Offset: 0x001BD496
	public void AddPlants(KPrefabID plant)
	{
		this.plants.Add(plant);
		this.dirty = true;
	}

	// Token: 0x06004DD5 RID: 19925 RVA: 0x001BF2AC File Offset: 0x001BD4AC
	public void RemoveFromCavity(KPrefabID id, List<KPrefabID> listToRemove)
	{
		int num = -1;
		for (int i = 0; i < listToRemove.Count; i++)
		{
			if (id.InstanceID == listToRemove[i].InstanceID)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			listToRemove.RemoveAt(num);
		}
	}

	// Token: 0x06004DD6 RID: 19926 RVA: 0x001BF2F0 File Offset: 0x001BD4F0
	public void OnEnter(object data)
	{
		foreach (KPrefabID kprefabID in this.buildings)
		{
			if (kprefabID != null)
			{
				kprefabID.Trigger(-832141045, data);
			}
		}
	}

	// Token: 0x06004DD7 RID: 19927 RVA: 0x001BF354 File Offset: 0x001BD554
	public Vector3 GetCenter()
	{
		return new Vector3((float)(this.minX + (this.maxX - this.minX) / 2), (float)(this.minY + (this.maxY - this.minY) / 2));
	}

	// Token: 0x040033C8 RID: 13256
	public HandleVector<int>.Handle handle;

	// Token: 0x040033C9 RID: 13257
	public bool dirty;

	// Token: 0x040033CA RID: 13258
	public int numCells;

	// Token: 0x040033CB RID: 13259
	public int maxX;

	// Token: 0x040033CC RID: 13260
	public int maxY;

	// Token: 0x040033CD RID: 13261
	public int minX;

	// Token: 0x040033CE RID: 13262
	public int minY;

	// Token: 0x040033CF RID: 13263
	public Room room;

	// Token: 0x040033D0 RID: 13264
	public List<KPrefabID> buildings = new List<KPrefabID>();

	// Token: 0x040033D1 RID: 13265
	public List<KPrefabID> plants = new List<KPrefabID>();

	// Token: 0x040033D2 RID: 13266
	public List<KPrefabID> creatures = new List<KPrefabID>();

	// Token: 0x040033D3 RID: 13267
	public List<KPrefabID> eggs = new List<KPrefabID>();
}
