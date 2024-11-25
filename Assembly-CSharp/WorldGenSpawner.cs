using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using ProcGen;
using ProcGenGame;
using TemplateClasses;
using UnityEngine;

// Token: 0x02000B65 RID: 2917
[AddComponentMenu("KMonoBehaviour/scripts/WorldGenSpawner")]
public class WorldGenSpawner : KMonoBehaviour
{
	// Token: 0x06005789 RID: 22409 RVA: 0x001F44FF File Offset: 0x001F26FF
	public bool SpawnsRemain()
	{
		return this.spawnables.Count > 0;
	}

	// Token: 0x0600578A RID: 22410 RVA: 0x001F4510 File Offset: 0x001F2710
	public void SpawnEverything()
	{
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			this.spawnables[i].TrySpawn();
		}
	}

	// Token: 0x0600578B RID: 22411 RVA: 0x001F4544 File Offset: 0x001F2744
	public void SpawnTag(string id)
	{
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			if (this.spawnables[i].spawnInfo.id == id)
			{
				this.spawnables[i].TrySpawn();
			}
		}
	}

	// Token: 0x0600578C RID: 22412 RVA: 0x001F4598 File Offset: 0x001F2798
	public void ClearSpawnersInArea(Vector2 root_position, CellOffset[] area)
	{
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			if (Grid.IsCellOffsetOf(Grid.PosToCell(root_position), this.spawnables[i].cell, area))
			{
				this.spawnables[i].FreeResources();
			}
		}
	}

	// Token: 0x0600578D RID: 22413 RVA: 0x001F45EB File Offset: 0x001F27EB
	public IReadOnlyList<WorldGenSpawner.Spawnable> GetSpawnables()
	{
		return this.spawnables;
	}

	// Token: 0x0600578E RID: 22414 RVA: 0x001F45F4 File Offset: 0x001F27F4
	protected override void OnSpawn()
	{
		if (!this.hasPlacedTemplates)
		{
			global::Debug.Assert(SaveLoader.Instance.Cluster != null, "Trying to place templates for an already-loaded save, no worldgen data available");
			this.DoReveal(SaveLoader.Instance.Cluster);
			this.PlaceTemplates(SaveLoader.Instance.Cluster);
			this.hasPlacedTemplates = true;
		}
		if (this.spawnInfos == null)
		{
			return;
		}
		for (int i = 0; i < this.spawnInfos.Length; i++)
		{
			this.AddSpawnable(this.spawnInfos[i]);
		}
	}

	// Token: 0x0600578F RID: 22415 RVA: 0x001F4674 File Offset: 0x001F2874
	[OnSerializing]
	private void OnSerializing()
	{
		List<Prefab> list = new List<Prefab>();
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			WorldGenSpawner.Spawnable spawnable = this.spawnables[i];
			if (!spawnable.isSpawned)
			{
				list.Add(spawnable.spawnInfo);
			}
		}
		this.spawnInfos = list.ToArray();
	}

	// Token: 0x06005790 RID: 22416 RVA: 0x001F46CA File Offset: 0x001F28CA
	private void AddSpawnable(Prefab prefab)
	{
		this.spawnables.Add(new WorldGenSpawner.Spawnable(prefab));
	}

	// Token: 0x06005791 RID: 22417 RVA: 0x001F46E0 File Offset: 0x001F28E0
	public void AddLegacySpawner(Tag tag, int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		this.AddSpawnable(new Prefab(tag.Name, Prefab.Type.Other, vector2I.x, vector2I.y, SimHashes.Carbon, -1f, 1f, null, 0, Orientation.Neutral, null, null, 0, null));
	}

	// Token: 0x06005792 RID: 22418 RVA: 0x001F472C File Offset: 0x001F292C
	public List<Tag> GetUnspawnedWithType<T>(int worldID) where T : KMonoBehaviour
	{
		List<Tag> list = new List<Tag>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => !match.isSpawned && (int)Grid.WorldIdx[match.cell] == worldID && Assets.GetPrefab(match.spawnInfo.id) != null && Assets.GetPrefab(match.spawnInfo.id).GetComponent<T>() != null));
		}
		foreach (WorldGenSpawner.Spawnable spawnable in list2.FindAll(match2))
		{
			list.Add(spawnable.spawnInfo.id);
		}
		return list;
	}

	// Token: 0x06005793 RID: 22419 RVA: 0x001F47C8 File Offset: 0x001F29C8
	public List<WorldGenSpawner.Spawnable> GeInfoOfUnspawnedWithType<T>(int worldID) where T : KMonoBehaviour
	{
		List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => !match.isSpawned && (int)Grid.WorldIdx[match.cell] == worldID && Assets.GetPrefab(match.spawnInfo.id) != null && Assets.GetPrefab(match.spawnInfo.id).GetComponent<T>() != null));
		}
		foreach (WorldGenSpawner.Spawnable item in list2.FindAll(match2))
		{
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06005794 RID: 22420 RVA: 0x001F4858 File Offset: 0x001F2A58
	public List<Tag> GetSpawnersWithTag(Tag tag, int worldID, bool includeSpawned = false)
	{
		List<Tag> list = new List<Tag>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => (includeSpawned || !match.isSpawned) && (int)Grid.WorldIdx[match.cell] == worldID && match.spawnInfo.id == tag));
		}
		foreach (WorldGenSpawner.Spawnable spawnable in list2.FindAll(match2))
		{
			list.Add(spawnable.spawnInfo.id);
		}
		return list;
	}

	// Token: 0x06005795 RID: 22421 RVA: 0x001F4904 File Offset: 0x001F2B04
	public List<WorldGenSpawner.Spawnable> GetSpawnablesWithTag(Tag tag, int worldID, bool includeSpawned = false)
	{
		List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => (includeSpawned || !match.isSpawned) && (int)Grid.WorldIdx[match.cell] == worldID && match.spawnInfo.id == tag));
		}
		foreach (WorldGenSpawner.Spawnable item in list2.FindAll(match2))
		{
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06005796 RID: 22422 RVA: 0x001F49A0 File Offset: 0x001F2BA0
	public List<WorldGenSpawner.Spawnable> GetSpawnablesWithTag(bool includeSpawned = false, params Tag[] tags)
	{
		List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => includeSpawned || !match.isSpawned));
		}
		foreach (WorldGenSpawner.Spawnable spawnable in list2.FindAll(match2))
		{
			foreach (Tag b in tags)
			{
				if (spawnable.spawnInfo.id == b)
				{
					list.Add(spawnable);
					break;
				}
			}
		}
		return list;
	}

	// Token: 0x06005797 RID: 22423 RVA: 0x001F4A6C File Offset: 0x001F2C6C
	private void PlaceTemplates(Cluster clusterLayout)
	{
		this.spawnables = new List<WorldGenSpawner.Spawnable>();
		foreach (WorldGen worldGen in clusterLayout.worlds)
		{
			foreach (Prefab prefab in worldGen.SpawnData.buildings)
			{
				prefab.location_x += worldGen.data.world.offset.x;
				prefab.location_y += worldGen.data.world.offset.y;
				prefab.type = Prefab.Type.Building;
				this.AddSpawnable(prefab);
			}
			foreach (Prefab prefab2 in worldGen.SpawnData.elementalOres)
			{
				prefab2.location_x += worldGen.data.world.offset.x;
				prefab2.location_y += worldGen.data.world.offset.y;
				prefab2.type = Prefab.Type.Ore;
				this.AddSpawnable(prefab2);
			}
			foreach (Prefab prefab3 in worldGen.SpawnData.otherEntities)
			{
				prefab3.location_x += worldGen.data.world.offset.x;
				prefab3.location_y += worldGen.data.world.offset.y;
				prefab3.type = Prefab.Type.Other;
				this.AddSpawnable(prefab3);
			}
			foreach (Prefab prefab4 in worldGen.SpawnData.pickupables)
			{
				prefab4.location_x += worldGen.data.world.offset.x;
				prefab4.location_y += worldGen.data.world.offset.y;
				prefab4.type = Prefab.Type.Pickupable;
				this.AddSpawnable(prefab4);
			}
			foreach (Tag tag in worldGen.SpawnData.discoveredResources)
			{
				DiscoveredResources.Instance.Discover(tag);
			}
			worldGen.SpawnData.buildings.Clear();
			worldGen.SpawnData.elementalOres.Clear();
			worldGen.SpawnData.otherEntities.Clear();
			worldGen.SpawnData.pickupables.Clear();
			worldGen.SpawnData.discoveredResources.Clear();
		}
	}

	// Token: 0x06005798 RID: 22424 RVA: 0x001F4E08 File Offset: 0x001F3008
	private void DoReveal(Cluster clusterLayout)
	{
		foreach (WorldGen worldGen in clusterLayout.worlds)
		{
			Game.Instance.Reset(worldGen.SpawnData, worldGen.WorldOffset);
		}
		for (int i = 0; i < Grid.CellCount; i++)
		{
			Grid.Revealed[i] = false;
			Grid.Spawnable[i] = 0;
		}
		float innerRadius = 16.5f;
		int radius = 18;
		Vector2I vector2I = clusterLayout.currentWorld.SpawnData.baseStartPos;
		vector2I += clusterLayout.currentWorld.WorldOffset;
		GridVisibility.Reveal(vector2I.x, vector2I.y, radius, innerRadius);
	}

	// Token: 0x04003941 RID: 14657
	[Serialize]
	private Prefab[] spawnInfos;

	// Token: 0x04003942 RID: 14658
	[Serialize]
	private bool hasPlacedTemplates;

	// Token: 0x04003943 RID: 14659
	private List<WorldGenSpawner.Spawnable> spawnables = new List<WorldGenSpawner.Spawnable>();

	// Token: 0x02001BB9 RID: 7097
	public class Spawnable
	{
		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x0600A44C RID: 42060 RVA: 0x0038C2D9 File Offset: 0x0038A4D9
		// (set) Token: 0x0600A44D RID: 42061 RVA: 0x0038C2E1 File Offset: 0x0038A4E1
		public Prefab spawnInfo { get; private set; }

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x0600A44E RID: 42062 RVA: 0x0038C2EA File Offset: 0x0038A4EA
		// (set) Token: 0x0600A44F RID: 42063 RVA: 0x0038C2F2 File Offset: 0x0038A4F2
		public bool isSpawned { get; private set; }

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x0600A450 RID: 42064 RVA: 0x0038C2FB File Offset: 0x0038A4FB
		// (set) Token: 0x0600A451 RID: 42065 RVA: 0x0038C303 File Offset: 0x0038A503
		public int cell { get; private set; }

		// Token: 0x0600A452 RID: 42066 RVA: 0x0038C30C File Offset: 0x0038A50C
		public Spawnable(Prefab spawn_info)
		{
			this.spawnInfo = spawn_info;
			int num = Grid.XYToCell(this.spawnInfo.location_x, this.spawnInfo.location_y);
			GameObject prefab = Assets.GetPrefab(spawn_info.id);
			if (prefab != null)
			{
				WorldSpawnableMonitor.Def def = prefab.GetDef<WorldSpawnableMonitor.Def>();
				if (def != null && def.adjustSpawnLocationCb != null)
				{
					num = def.adjustSpawnLocationCb(num);
				}
			}
			this.cell = num;
			global::Debug.Assert(Grid.IsValidCell(this.cell));
			if (Grid.Spawnable[this.cell] > 0)
			{
				this.TrySpawn();
				return;
			}
			this.fogOfWarPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnReveal", this, this.cell, GameScenePartitioner.Instance.fogOfWarChangedLayer, new Action<object>(this.OnReveal));
		}

		// Token: 0x0600A453 RID: 42067 RVA: 0x0038C3DA File Offset: 0x0038A5DA
		private void OnReveal(object data)
		{
			if (Grid.Spawnable[this.cell] > 0)
			{
				this.TrySpawn();
			}
		}

		// Token: 0x0600A454 RID: 42068 RVA: 0x0038C3F1 File Offset: 0x0038A5F1
		private void OnSolidChanged(object data)
		{
			if (!Grid.Solid[this.cell])
			{
				GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
				Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
				this.Spawn();
			}
		}

		// Token: 0x0600A455 RID: 42069 RVA: 0x0038C430 File Offset: 0x0038A630
		public void FreeResources()
		{
			if (this.solidChangedPartitionerEntry.IsValid())
			{
				GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
				if (Game.Instance != null)
				{
					Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
				}
			}
			GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
			this.isSpawned = true;
		}

		// Token: 0x0600A456 RID: 42070 RVA: 0x0038C494 File Offset: 0x0038A694
		public void TrySpawn()
		{
			if (this.isSpawned)
			{
				return;
			}
			if (this.solidChangedPartitionerEntry.IsValid())
			{
				return;
			}
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[this.cell]);
			bool flag = world != null && world.IsDiscovered;
			GameObject prefab = Assets.GetPrefab(this.GetPrefabTag());
			if (!(prefab != null))
			{
				if (flag)
				{
					GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
					this.Spawn();
				}
				return;
			}
			if (!(flag | prefab.HasTag(GameTags.WarpTech)))
			{
				return;
			}
			GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
			bool flag2 = false;
			if (prefab.GetComponent<Pickupable>() != null && !prefab.HasTag(GameTags.Creatures.Digger))
			{
				flag2 = true;
			}
			else if (prefab.GetDef<BurrowMonitor.Def>() != null)
			{
				flag2 = true;
			}
			if (flag2 && Grid.Solid[this.cell])
			{
				this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnSolidChanged", this, this.cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
				Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(this.cell);
				return;
			}
			this.Spawn();
		}

		// Token: 0x0600A457 RID: 42071 RVA: 0x0038C5C8 File Offset: 0x0038A7C8
		private Tag GetPrefabTag()
		{
			Mob mob = SettingsCache.mobs.GetMob(this.spawnInfo.id);
			if (mob != null && mob.prefabName != null)
			{
				return new Tag(mob.prefabName);
			}
			return new Tag(this.spawnInfo.id);
		}

		// Token: 0x0600A458 RID: 42072 RVA: 0x0038C614 File Offset: 0x0038A814
		private void Spawn()
		{
			this.isSpawned = true;
			GameObject gameObject = WorldGenSpawner.Spawnable.GetSpawnableCallback(this.spawnInfo.type)(this.spawnInfo, 0);
			if (gameObject != null && gameObject)
			{
				gameObject.SetActive(true);
				gameObject.Trigger(1119167081, this.spawnInfo);
			}
			this.FreeResources();
		}

		// Token: 0x0600A459 RID: 42073 RVA: 0x0038C674 File Offset: 0x0038A874
		public static WorldGenSpawner.Spawnable.PlaceEntityFn GetSpawnableCallback(Prefab.Type type)
		{
			switch (type)
			{
			case Prefab.Type.Building:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceBuilding);
			case Prefab.Type.Ore:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceElementalOres);
			case Prefab.Type.Pickupable:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlacePickupables);
			case Prefab.Type.Other:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
			default:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
			}
		}

		// Token: 0x0400808C RID: 32908
		private HandleVector<int>.Handle fogOfWarPartitionerEntry;

		// Token: 0x0400808D RID: 32909
		private HandleVector<int>.Handle solidChangedPartitionerEntry;

		// Token: 0x0200262F RID: 9775
		// (Invoke) Token: 0x0600C19C RID: 49564
		public delegate GameObject PlaceEntityFn(Prefab prefab, int root_cell);
	}
}
