using System;
using System.Collections.Generic;
using FMOD.Studio;
using KSerialization;
using ProcGenGame;
using TUNING;
using UnityEngine;

// Token: 0x020007BB RID: 1979
public class ClusterManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x0600365A RID: 13914 RVA: 0x00127B28 File Offset: 0x00125D28
	public static void DestroyInstance()
	{
		ClusterManager.Instance = null;
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x0600365B RID: 13915 RVA: 0x00127B30 File Offset: 0x00125D30
	public int worldCount
	{
		get
		{
			return this.m_worldContainers.Count;
		}
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x0600365C RID: 13916 RVA: 0x00127B3D File Offset: 0x00125D3D
	public int activeWorldId
	{
		get
		{
			return this.activeWorldIdx;
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x0600365D RID: 13917 RVA: 0x00127B45 File Offset: 0x00125D45
	public IList<WorldContainer> WorldContainers
	{
		get
		{
			return this.m_worldContainers.AsReadOnly();
		}
	}

	// Token: 0x0600365E RID: 13918 RVA: 0x00127B52 File Offset: 0x00125D52
	public ClusterPOIManager GetClusterPOIManager()
	{
		return this.m_clusterPOIsManager;
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x0600365F RID: 13919 RVA: 0x00127B5C File Offset: 0x00125D5C
	public Dictionary<int, List<IAssignableIdentity>> MinionsByWorld
	{
		get
		{
			this.minionsByWorld.Clear();
			for (int i = 0; i < Components.MinionAssignablesProxy.Count; i++)
			{
				if (!Components.MinionAssignablesProxy[i].GetTargetGameObject().HasTag(GameTags.Dead))
				{
					int id = Components.MinionAssignablesProxy[i].GetTargetGameObject().GetComponent<KMonoBehaviour>().GetMyWorld().id;
					if (!this.minionsByWorld.ContainsKey(id))
					{
						this.minionsByWorld.Add(id, new List<IAssignableIdentity>());
					}
					this.minionsByWorld[id].Add(Components.MinionAssignablesProxy[i]);
				}
			}
			return this.minionsByWorld;
		}
	}

	// Token: 0x06003660 RID: 13920 RVA: 0x00127C09 File Offset: 0x00125E09
	public void RegisterWorldContainer(WorldContainer worldContainer)
	{
		this.m_worldContainers.Add(worldContainer);
	}

	// Token: 0x06003661 RID: 13921 RVA: 0x00127C17 File Offset: 0x00125E17
	public void UnregisterWorldContainer(WorldContainer worldContainer)
	{
		base.Trigger(-1078710002, worldContainer.id);
		this.m_worldContainers.Remove(worldContainer);
	}

	// Token: 0x06003662 RID: 13922 RVA: 0x00127C3C File Offset: 0x00125E3C
	public List<int> GetWorldIDsSorted()
	{
		ListPool<WorldContainer, ClusterManager>.PooledList pooledList = ListPool<WorldContainer, ClusterManager>.Allocate(this.m_worldContainers);
		pooledList.Sort((WorldContainer a, WorldContainer b) => a.DiscoveryTimestamp.CompareTo(b.DiscoveryTimestamp));
		this._worldIDs.Clear();
		foreach (WorldContainer worldContainer in pooledList)
		{
			this._worldIDs.Add(worldContainer.id);
		}
		pooledList.Recycle();
		return this._worldIDs;
	}

	// Token: 0x06003663 RID: 13923 RVA: 0x00127CDC File Offset: 0x00125EDC
	public List<int> GetDiscoveredAsteroidIDsSorted()
	{
		ListPool<WorldContainer, ClusterManager>.PooledList pooledList = ListPool<WorldContainer, ClusterManager>.Allocate(this.m_worldContainers);
		pooledList.Sort((WorldContainer a, WorldContainer b) => a.DiscoveryTimestamp.CompareTo(b.DiscoveryTimestamp));
		this._discoveredAsteroidIds.Clear();
		for (int i = 0; i < pooledList.Count; i++)
		{
			if (pooledList[i].IsDiscovered && !pooledList[i].IsModuleInterior)
			{
				this._discoveredAsteroidIds.Add(pooledList[i].id);
			}
		}
		pooledList.Recycle();
		return this._discoveredAsteroidIds;
	}

	// Token: 0x06003664 RID: 13924 RVA: 0x00127D78 File Offset: 0x00125F78
	public WorldContainer GetStartWorld()
	{
		foreach (WorldContainer worldContainer in this.WorldContainers)
		{
			if (worldContainer.IsStartWorld)
			{
				return worldContainer;
			}
		}
		return this.WorldContainers[0];
	}

	// Token: 0x06003665 RID: 13925 RVA: 0x00127DD8 File Offset: 0x00125FD8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClusterManager.Instance = this;
		SaveLoader instance = SaveLoader.Instance;
		instance.OnWorldGenComplete = (Action<Cluster>)Delegate.Combine(instance.OnWorldGenComplete, new Action<Cluster>(this.OnWorldGenComplete));
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x00127E0C File Offset: 0x0012600C
	protected override void OnSpawn()
	{
		if (this.m_grid == null)
		{
			this.m_grid = new ClusterGrid(this.m_numRings);
		}
		this.UpdateWorldReverbSnapshot(this.activeWorldId);
		base.OnSpawn();
	}

	// Token: 0x06003667 RID: 13927 RVA: 0x00127E39 File Offset: 0x00126039
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x06003668 RID: 13928 RVA: 0x00127E41 File Offset: 0x00126041
	public WorldContainer activeWorld
	{
		get
		{
			return this.GetWorld(this.activeWorldId);
		}
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x00127E50 File Offset: 0x00126050
	private void OnWorldGenComplete(Cluster clusterLayout)
	{
		this.m_numRings = clusterLayout.numRings;
		this.m_grid = new ClusterGrid(this.m_numRings);
		AxialI location = AxialI.ZERO;
		foreach (WorldGen worldGen in clusterLayout.worlds)
		{
			int id = this.CreateAsteroidWorldContainer(worldGen).id;
			Vector2I position = worldGen.GetPosition();
			Vector2I vector2I = position + worldGen.GetSize();
			if (worldGen.isStartingWorld)
			{
				location = worldGen.GetClusterLocation();
			}
			for (int i = position.y; i < vector2I.y; i++)
			{
				for (int j = position.x; j < vector2I.x; j++)
				{
					int num = Grid.XYToCell(j, i);
					Grid.WorldIdx[num] = (byte)id;
					Pathfinding.Instance.AddDirtyNavGridCell(num);
				}
			}
			if (worldGen.isStartingWorld)
			{
				this.activeWorldIdx = id;
			}
		}
		this.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(location, 1);
		this.m_clusterPOIsManager.PopulatePOIsFromWorldGen(clusterLayout);
	}

	// Token: 0x0600366A RID: 13930 RVA: 0x00127F7C File Offset: 0x0012617C
	private int GetNextWorldId()
	{
		HashSetPool<int, ClusterManager>.PooledHashSet pooledHashSet = HashSetPool<int, ClusterManager>.Allocate();
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			pooledHashSet.Add(worldContainer.id);
		}
		global::Debug.Assert(this.m_worldContainers.Count < 255, "Oh no! We're trying to generate our 255th world in this save, things are going to start going badly...");
		for (int i = 0; i < 255; i++)
		{
			if (!pooledHashSet.Contains(i))
			{
				pooledHashSet.Recycle();
				return i;
			}
		}
		pooledHashSet.Recycle();
		return 255;
	}

	// Token: 0x0600366B RID: 13931 RVA: 0x00128024 File Offset: 0x00126224
	private WorldContainer CreateAsteroidWorldContainer(WorldGen world)
	{
		int nextWorldId = this.GetNextWorldId();
		GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab("Asteroid"), null, null);
		WorldContainer component = gameObject.GetComponent<WorldContainer>();
		component.SetID(nextWorldId);
		component.SetWorldDetails(world);
		AsteroidGridEntity component2 = gameObject.GetComponent<AsteroidGridEntity>();
		if (world != null)
		{
			AxialI clusterLocation = world.GetClusterLocation();
			component2.Init(component.GetRandomName(), clusterLocation, world.Settings.world.asteroidIcon);
		}
		else
		{
			component2.Init("", AxialI.ZERO, "");
		}
		if (component.IsStartWorld)
		{
			OrbitalMechanics component3 = gameObject.GetComponent<OrbitalMechanics>();
			if (component3 != null)
			{
				component3.CreateOrbitalObject(Db.Get().OrbitalTypeCategories.backgroundEarth.Id);
			}
		}
		gameObject.SetActive(true);
		return component;
	}

	// Token: 0x0600366C RID: 13932 RVA: 0x001280E8 File Offset: 0x001262E8
	private void CreateDefaultAsteroidWorldContainer()
	{
		if (this.m_worldContainers.Count == 0)
		{
			global::Debug.LogWarning("Cluster manager has no world containers, create a default using Grid settings.");
			WorldContainer worldContainer = this.CreateAsteroidWorldContainer(null);
			int id = worldContainer.id;
			int num = (int)worldContainer.minimumBounds.y;
			while ((float)num <= worldContainer.maximumBounds.y)
			{
				int num2 = (int)worldContainer.minimumBounds.x;
				while ((float)num2 <= worldContainer.maximumBounds.x)
				{
					int num3 = Grid.XYToCell(num2, num);
					Grid.WorldIdx[num3] = (byte)id;
					Pathfinding.Instance.AddDirtyNavGridCell(num3);
					num2++;
				}
				num++;
			}
		}
	}

	// Token: 0x0600366D RID: 13933 RVA: 0x00128180 File Offset: 0x00126380
	public void InitializeWorldGrid()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
		{
			this.CreateDefaultAsteroidWorldContainer();
		}
		bool flag = false;
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			Vector2I worldOffset = worldContainer.WorldOffset;
			Vector2I vector2I = worldOffset + worldContainer.WorldSize;
			for (int i = worldOffset.y; i < vector2I.y; i++)
			{
				for (int j = worldOffset.x; j < vector2I.x; j++)
				{
					int num = Grid.XYToCell(j, i);
					Grid.WorldIdx[num] = (byte)worldContainer.id;
					Pathfinding.Instance.AddDirtyNavGridCell(num);
				}
			}
			flag |= worldContainer.IsDiscovered;
		}
		if (!flag)
		{
			global::Debug.LogWarning("No worlds have been discovered. Setting the active world to discovered");
			this.activeWorld.SetDiscovered(false);
		}
	}

	// Token: 0x0600366E RID: 13934 RVA: 0x00128288 File Offset: 0x00126488
	public void SetActiveWorld(int worldIdx)
	{
		int num = this.activeWorldIdx;
		if (num != worldIdx)
		{
			this.activeWorldIdx = worldIdx;
			Game.Instance.Trigger(1983128072, new global::Tuple<int, int>(this.activeWorldIdx, num));
			this.UpdateRocketInteriorAudio();
		}
	}

	// Token: 0x0600366F RID: 13935 RVA: 0x001282C8 File Offset: 0x001264C8
	public void TimelapseModeOverrideActiveWorld(int overrideValue)
	{
		this.activeWorldIdx = overrideValue;
	}

	// Token: 0x06003670 RID: 13936 RVA: 0x001282D4 File Offset: 0x001264D4
	public WorldContainer GetWorld(int id)
	{
		for (int i = 0; i < this.m_worldContainers.Count; i++)
		{
			if (this.m_worldContainers[i].id == id)
			{
				return this.m_worldContainers[i];
			}
		}
		return null;
	}

	// Token: 0x06003671 RID: 13937 RVA: 0x0012831C File Offset: 0x0012651C
	public WorldContainer GetWorldFromPosition(Vector3 position)
	{
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			if (worldContainer.ContainsPoint(position))
			{
				return worldContainer;
			}
		}
		return null;
	}

	// Token: 0x06003672 RID: 13938 RVA: 0x00128380 File Offset: 0x00126580
	public float CountAllRations()
	{
		float result = 0f;
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, worldContainer.worldInventory, true);
		}
		return result;
	}

	// Token: 0x06003673 RID: 13939 RVA: 0x001283E8 File Offset: 0x001265E8
	public Dictionary<Tag, float> GetAllWorldsAccessibleAmounts()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			foreach (KeyValuePair<Tag, float> keyValuePair in worldContainer.worldInventory.GetAccessibleAmounts())
			{
				if (dictionary.ContainsKey(keyValuePair.Key))
				{
					Dictionary<Tag, float> dictionary2 = dictionary;
					Tag key = keyValuePair.Key;
					dictionary2[key] += keyValuePair.Value;
				}
				else
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06003674 RID: 13940 RVA: 0x001284CC File Offset: 0x001266CC
	public void MigrateMinion(MinionIdentity minion, int targetID)
	{
		this.MigrateMinion(minion, targetID, minion.GetMyWorldId());
	}

	// Token: 0x06003675 RID: 13941 RVA: 0x001284DC File Offset: 0x001266DC
	public void MigrateCritter(GameObject critter, int targetID)
	{
		this.MigrateCritter(critter, targetID, critter.GetMyWorldId());
	}

	// Token: 0x06003676 RID: 13942 RVA: 0x001284EC File Offset: 0x001266EC
	public void MigrateCritter(GameObject critter, int targetID, int prevID)
	{
		this.critterMigrationEvArg.entity = critter;
		this.critterMigrationEvArg.prevWorldId = prevID;
		this.critterMigrationEvArg.targetWorldId = targetID;
		Game.Instance.Trigger(1142724171, this.critterMigrationEvArg);
	}

	// Token: 0x06003677 RID: 13943 RVA: 0x00128528 File Offset: 0x00126728
	public void MigrateMinion(MinionIdentity minion, int targetID, int prevID)
	{
		if (!ClusterManager.Instance.GetWorld(targetID).IsDiscovered)
		{
			ClusterManager.Instance.GetWorld(targetID).SetDiscovered(false);
		}
		if (!ClusterManager.Instance.GetWorld(targetID).IsDupeVisited)
		{
			ClusterManager.Instance.GetWorld(targetID).SetDupeVisited();
		}
		this.migrationEvArg.minionId = minion;
		this.migrationEvArg.prevWorldId = prevID;
		this.migrationEvArg.targetWorldId = targetID;
		Game.Instance.assignmentManager.RemoveFromWorld(minion, this.migrationEvArg.prevWorldId);
		Game.Instance.Trigger(586301400, this.migrationEvArg);
	}

	// Token: 0x06003678 RID: 13944 RVA: 0x001285D0 File Offset: 0x001267D0
	public int GetLandingBeaconLocation(int worldId)
	{
		foreach (object obj in Components.LandingBeacons)
		{
			LandingBeacon.Instance instance = (LandingBeacon.Instance)obj;
			if (instance.GetMyWorldId() == worldId && instance.CanBeTargeted())
			{
				return Grid.PosToCell(instance);
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x06003679 RID: 13945 RVA: 0x00128644 File Offset: 0x00126844
	public int GetRandomClearCell(int worldId)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < 1000)
		{
			num++;
			int num2 = UnityEngine.Random.Range(0, Grid.CellCount);
			if (!Grid.Solid[num2] && !Grid.IsLiquid(num2) && (int)Grid.WorldIdx[num2] == worldId)
			{
				return num2;
			}
		}
		num = 0;
		while (!flag && num < 1000)
		{
			num++;
			int num3 = UnityEngine.Random.Range(0, Grid.CellCount);
			if (!Grid.Solid[num3] && (int)Grid.WorldIdx[num3] == worldId)
			{
				return num3;
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x0600367A RID: 13946 RVA: 0x001286D0 File Offset: 0x001268D0
	private bool NotObstructedCell(int x, int y)
	{
		int cell = Grid.XYToCell(x, y);
		return Grid.IsValidCell(cell) && Grid.Objects[cell, 1] == null;
	}

	// Token: 0x0600367B RID: 13947 RVA: 0x00128704 File Offset: 0x00126904
	private int LowestYThatSeesSky(int topCellYPos, int x)
	{
		int num = topCellYPos;
		while (!this.ValidSurfaceCell(x, num))
		{
			num--;
		}
		return num;
	}

	// Token: 0x0600367C RID: 13948 RVA: 0x00128724 File Offset: 0x00126924
	private bool ValidSurfaceCell(int x, int y)
	{
		int i = Grid.XYToCell(x, y - 1);
		return Grid.Solid[i] || Grid.Foundation[i];
	}

	// Token: 0x0600367D RID: 13949 RVA: 0x00128758 File Offset: 0x00126958
	public int GetRandomSurfaceCell(int worldID, int width = 1, bool excludeTopBorderHeight = true)
	{
		WorldContainer worldContainer = this.m_worldContainers.Find((WorldContainer match) => match.id == worldID);
		int num = Mathf.RoundToInt(UnityEngine.Random.Range(worldContainer.minimumBounds.x + (float)(worldContainer.Width / 10), worldContainer.maximumBounds.x - (float)(worldContainer.Width / 10)));
		int num2 = Mathf.RoundToInt(worldContainer.maximumBounds.y);
		if (excludeTopBorderHeight)
		{
			num2 -= Grid.TopBorderHeight;
		}
		int num3 = num;
		int num4 = this.LowestYThatSeesSky(num2, num3);
		int num5;
		if (this.NotObstructedCell(num3, num4))
		{
			num5 = 1;
		}
		else
		{
			num5 = 0;
		}
		while (num3 + 1 != num && num5 < width)
		{
			num3++;
			if ((float)num3 > worldContainer.maximumBounds.x)
			{
				num5 = 0;
				num3 = (int)worldContainer.minimumBounds.x;
			}
			int num6 = this.LowestYThatSeesSky(num2, num3);
			bool flag = this.NotObstructedCell(num3, num6);
			if (num6 == num4 && flag)
			{
				num5++;
			}
			else if (flag)
			{
				num5 = 1;
			}
			else
			{
				num5 = 0;
			}
			num4 = num6;
		}
		if (num5 < width)
		{
			return -1;
		}
		return Grid.XYToCell(num3, num4);
	}

	// Token: 0x0600367E RID: 13950 RVA: 0x00128884 File Offset: 0x00126A84
	public bool IsPositionInActiveWorld(Vector3 pos)
	{
		if (this.activeWorld != null && !CameraController.Instance.ignoreClusterFX)
		{
			Vector2 vector = this.activeWorld.maximumBounds * Grid.CellSizeInMeters + new Vector2(1f, 1f);
			Vector2 vector2 = this.activeWorld.minimumBounds * Grid.CellSizeInMeters;
			if (pos.x < vector2.x || pos.x > vector.x || pos.y < vector2.y || pos.y > vector.y)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600367F RID: 13951 RVA: 0x0012892C File Offset: 0x00126B2C
	public WorldContainer CreateRocketInteriorWorld(GameObject craft_go, string interiorTemplateName, System.Action callback)
	{
		Vector2I rocket_INTERIOR_SIZE = ROCKETRY.ROCKET_INTERIOR_SIZE;
		Vector2I vector2I;
		if (Grid.GetFreeGridSpace(rocket_INTERIOR_SIZE, out vector2I))
		{
			int nextWorldId = this.GetNextWorldId();
			craft_go.AddComponent<WorldInventory>();
			WorldContainer worldContainer = craft_go.AddComponent<WorldContainer>();
			worldContainer.SetRocketInteriorWorldDetails(nextWorldId, rocket_INTERIOR_SIZE, vector2I);
			Vector2I vector2I2 = vector2I + rocket_INTERIOR_SIZE;
			for (int i = vector2I.y; i < vector2I2.y; i++)
			{
				for (int j = vector2I.x; j < vector2I2.x; j++)
				{
					int num = Grid.XYToCell(j, i);
					Grid.WorldIdx[num] = (byte)nextWorldId;
					Pathfinding.Instance.AddDirtyNavGridCell(num);
				}
			}
			global::Debug.Log(string.Format("Created new rocket interior id: {0}, at {1} with size {2}", nextWorldId, vector2I, rocket_INTERIOR_SIZE));
			worldContainer.PlaceInteriorTemplate(interiorTemplateName, delegate
			{
				if (callback != null)
				{
					callback();
				}
				craft_go.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.RocketInteriorComplete, null);
			});
			craft_go.AddOrGet<OrbitalMechanics>().CreateOrbitalObject(Db.Get().OrbitalTypeCategories.landed.Id);
			base.Trigger(-1280433810, worldContainer.id);
			return worldContainer;
		}
		global::Debug.LogError("Failed to create rocket interior.");
		return null;
	}

	// Token: 0x06003680 RID: 13952 RVA: 0x00128A68 File Offset: 0x00126C68
	public void DestoryRocketInteriorWorld(int world_id, ClustercraftExteriorDoor door)
	{
		WorldContainer world = this.GetWorld(world_id);
		if (world == null || !world.IsModuleInterior)
		{
			global::Debug.LogError(string.Format("Attempting to destroy world id {0}. The world is not a valid rocket interior", world_id));
			return;
		}
		GameObject gameObject = door.GetComponent<RocketModuleCluster>().CraftInterface.gameObject;
		if (this.activeWorldId == world_id)
		{
			if (gameObject.GetComponent<WorldContainer>().ParentWorldId == world_id)
			{
				this.SetActiveWorld(ClusterManager.Instance.GetStartWorld().id);
			}
			else
			{
				this.SetActiveWorld(gameObject.GetComponent<WorldContainer>().ParentWorldId);
			}
		}
		OrbitalMechanics component = gameObject.GetComponent<OrbitalMechanics>();
		if (!component.IsNullOrDestroyed())
		{
			UnityEngine.Object.Destroy(component);
		}
		bool flag = gameObject.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.InFlight;
		PrimaryElement moduleElemet = door.GetComponent<PrimaryElement>();
		AxialI clusterLocation = world.GetComponent<ClusterGridEntity>().Location;
		Vector3 rocketModuleWorldPos = door.transform.position;
		if (!flag)
		{
			world.EjectAllDupes(rocketModuleWorldPos);
		}
		else
		{
			world.SpacePodAllDupes(clusterLocation, moduleElemet.ElementID);
		}
		world.CancelChores();
		HashSet<int> noRefundTiles;
		world.DestroyWorldBuildings(out noRefundTiles);
		this.UnregisterWorldContainer(world);
		if (!flag)
		{
			GameScheduler.Instance.ScheduleNextFrame("ClusterManager.world.TransferResourcesToParentWorld", delegate(object obj)
			{
				world.TransferResourcesToParentWorld(rocketModuleWorldPos + new Vector3(0f, 0.5f, 0f), noRefundTiles);
			}, null, null);
			GameScheduler.Instance.ScheduleNextFrame("ClusterManager.DeleteWorldObjects", delegate(object obj)
			{
				this.DeleteWorldObjects(world);
			}, null, null);
			return;
		}
		GameScheduler.Instance.ScheduleNextFrame("ClusterManager.world.TransferResourcesToDebris", delegate(object obj)
		{
			world.TransferResourcesToDebris(clusterLocation, noRefundTiles, moduleElemet.ElementID);
		}, null, null);
		GameScheduler.Instance.ScheduleNextFrame("ClusterManager.DeleteWorldObjects", delegate(object obj)
		{
			this.DeleteWorldObjects(world);
		}, null, null);
	}

	// Token: 0x06003681 RID: 13953 RVA: 0x00128C3C File Offset: 0x00126E3C
	public void UpdateWorldReverbSnapshot(int worldId)
	{
		if (!DlcManager.IsPureVanilla())
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().SmallRocketInteriorReverbSnapshot, STOP_MODE.ALLOWFADEOUT);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MediumRocketInteriorReverbSnapshot, STOP_MODE.ALLOWFADEOUT);
		}
		AudioMixer.instance.PauseSpaceVisibleSnapshot(false);
		WorldContainer world = this.GetWorld(worldId);
		if (world.IsModuleInterior)
		{
			PassengerRocketModule passengerModule = world.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule();
			AudioMixer.instance.Start(passengerModule.interiorReverbSnapshot);
			AudioMixer.instance.PauseSpaceVisibleSnapshot(true);
			this.UpdateRocketInteriorAudio();
		}
	}

	// Token: 0x06003682 RID: 13954 RVA: 0x00128CCC File Offset: 0x00126ECC
	public void UpdateRocketInteriorAudio()
	{
		WorldContainer activeWorld = this.activeWorld;
		if (activeWorld != null && activeWorld.IsModuleInterior)
		{
			activeWorld.minimumBounds + new Vector2((float)activeWorld.Width * Grid.CellSizeInMeters, (float)activeWorld.Height * Grid.CellSizeInMeters) / 2f;
			Clustercraft component = activeWorld.GetComponent<Clustercraft>();
			ClusterManager.RocketStatesForAudio rocketInteriorState = ClusterManager.RocketStatesForAudio.Grounded;
			switch (component.Status)
			{
			case Clustercraft.CraftStatus.Grounded:
				rocketInteriorState = (component.LaunchRequested ? ClusterManager.RocketStatesForAudio.ReadyForLaunch : ClusterManager.RocketStatesForAudio.Grounded);
				break;
			case Clustercraft.CraftStatus.Launching:
				rocketInteriorState = ClusterManager.RocketStatesForAudio.Launching;
				break;
			case Clustercraft.CraftStatus.InFlight:
				rocketInteriorState = ClusterManager.RocketStatesForAudio.InSpace;
				break;
			case Clustercraft.CraftStatus.Landing:
				rocketInteriorState = ClusterManager.RocketStatesForAudio.Landing;
				break;
			}
			ClusterManager.RocketInteriorState = rocketInteriorState;
		}
	}

	// Token: 0x06003683 RID: 13955 RVA: 0x00128D78 File Offset: 0x00126F78
	private void DeleteWorldObjects(WorldContainer world)
	{
		Grid.FreeGridSpace(world.WorldSize, world.WorldOffset);
		WorldInventory worldInventory = null;
		if (world != null)
		{
			worldInventory = world.GetComponent<WorldInventory>();
		}
		if (worldInventory != null)
		{
			UnityEngine.Object.Destroy(worldInventory);
		}
		if (world != null)
		{
			UnityEngine.Object.Destroy(world);
		}
	}

	// Token: 0x04002042 RID: 8258
	public static int MAX_ROCKET_INTERIOR_COUNT = 16;

	// Token: 0x04002043 RID: 8259
	public static ClusterManager.RocketStatesForAudio RocketInteriorState = ClusterManager.RocketStatesForAudio.Grounded;

	// Token: 0x04002044 RID: 8260
	public static ClusterManager Instance;

	// Token: 0x04002045 RID: 8261
	private ClusterGrid m_grid;

	// Token: 0x04002046 RID: 8262
	[Serialize]
	private int m_numRings = 9;

	// Token: 0x04002047 RID: 8263
	[Serialize]
	private int activeWorldIdx;

	// Token: 0x04002048 RID: 8264
	public const byte INVALID_WORLD_IDX = 255;

	// Token: 0x04002049 RID: 8265
	public static Color[] worldColors = new Color[]
	{
		Color.HSVToRGB(0.15f, 0.3f, 0.5f),
		Color.HSVToRGB(0.3f, 0.3f, 0.5f),
		Color.HSVToRGB(0.45f, 0.3f, 0.5f),
		Color.HSVToRGB(0.6f, 0.3f, 0.5f),
		Color.HSVToRGB(0.75f, 0.3f, 0.5f),
		Color.HSVToRGB(0.9f, 0.3f, 0.5f)
	};

	// Token: 0x0400204A RID: 8266
	private List<WorldContainer> m_worldContainers = new List<WorldContainer>();

	// Token: 0x0400204B RID: 8267
	[MyCmpGet]
	private ClusterPOIManager m_clusterPOIsManager;

	// Token: 0x0400204C RID: 8268
	private Dictionary<int, List<IAssignableIdentity>> minionsByWorld = new Dictionary<int, List<IAssignableIdentity>>();

	// Token: 0x0400204D RID: 8269
	private MinionMigrationEventArgs migrationEvArg = new MinionMigrationEventArgs();

	// Token: 0x0400204E RID: 8270
	private MigrationEventArgs critterMigrationEvArg = new MigrationEventArgs();

	// Token: 0x0400204F RID: 8271
	private List<int> _worldIDs = new List<int>();

	// Token: 0x04002050 RID: 8272
	private List<int> _discoveredAsteroidIds = new List<int>();

	// Token: 0x0200167B RID: 5755
	public enum RocketStatesForAudio
	{
		// Token: 0x04006FCF RID: 28623
		Grounded,
		// Token: 0x04006FD0 RID: 28624
		ReadyForLaunch,
		// Token: 0x04006FD1 RID: 28625
		Launching,
		// Token: 0x04006FD2 RID: 28626
		InSpace,
		// Token: 0x04006FD3 RID: 28627
		Landing
	}
}
