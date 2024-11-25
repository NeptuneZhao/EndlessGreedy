using System;
using System.Collections.Generic;

// Token: 0x02000677 RID: 1655
public class BuildingInventory : KMonoBehaviour
{
	// Token: 0x060028FC RID: 10492 RVA: 0x000E7F11 File Offset: 0x000E6111
	public static void DestroyInstance()
	{
		BuildingInventory.Instance = null;
	}

	// Token: 0x060028FD RID: 10493 RVA: 0x000E7F19 File Offset: 0x000E6119
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		BuildingInventory.Instance = this;
	}

	// Token: 0x060028FE RID: 10494 RVA: 0x000E7F27 File Offset: 0x000E6127
	public HashSet<BuildingComplete> GetBuildings(Tag tag)
	{
		return this.Buildings[tag];
	}

	// Token: 0x060028FF RID: 10495 RVA: 0x000E7F35 File Offset: 0x000E6135
	public int BuildingCount(Tag tag)
	{
		if (!this.Buildings.ContainsKey(tag))
		{
			return 0;
		}
		return this.Buildings[tag].Count;
	}

	// Token: 0x06002900 RID: 10496 RVA: 0x000E7F58 File Offset: 0x000E6158
	public int BuildingCountForWorld_BAD_PERF(Tag tag, int worldId)
	{
		if (!this.Buildings.ContainsKey(tag))
		{
			return 0;
		}
		int num = 0;
		using (HashSet<BuildingComplete>.Enumerator enumerator = this.Buildings[tag].GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetMyWorldId() == worldId)
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06002901 RID: 10497 RVA: 0x000E7FC8 File Offset: 0x000E61C8
	public void RegisterBuilding(BuildingComplete building)
	{
		Tag prefabTag = building.prefabid.PrefabTag;
		HashSet<BuildingComplete> hashSet;
		if (!this.Buildings.TryGetValue(prefabTag, out hashSet))
		{
			hashSet = new HashSet<BuildingComplete>();
			this.Buildings[prefabTag] = hashSet;
		}
		hashSet.Add(building);
	}

	// Token: 0x06002902 RID: 10498 RVA: 0x000E800C File Offset: 0x000E620C
	public void UnregisterBuilding(BuildingComplete building)
	{
		Tag prefabTag = building.prefabid.PrefabTag;
		HashSet<BuildingComplete> hashSet;
		if (!this.Buildings.TryGetValue(prefabTag, out hashSet))
		{
			DebugUtil.DevLogError(string.Format("Unregistering building {0} before it was registered.", prefabTag));
			return;
		}
		DebugUtil.DevAssert(hashSet.Remove(building), string.Format("Building {0} was not found to be removed", prefabTag), null);
	}

	// Token: 0x04001789 RID: 6025
	public static BuildingInventory Instance;

	// Token: 0x0400178A RID: 6026
	private Dictionary<Tag, HashSet<BuildingComplete>> Buildings = new Dictionary<Tag, HashSet<BuildingComplete>>();
}
