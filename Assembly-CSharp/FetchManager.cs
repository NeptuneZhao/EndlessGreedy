using System;
using System.Collections.Generic;
using System.Diagnostics;
using FoodRehydrator;
using UnityEngine;

// Token: 0x020008B5 RID: 2229
[AddComponentMenu("KMonoBehaviour/scripts/FetchManager")]
public class FetchManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06003E54 RID: 15956 RVA: 0x001594C3 File Offset: 0x001576C3
	private static int QuantizeRotValue(float rot_value)
	{
		return (int)(4f * rot_value);
	}

	// Token: 0x06003E55 RID: 15957 RVA: 0x001594CD File Offset: 0x001576CD
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x06003E56 RID: 15958 RVA: 0x001594CF File Offset: 0x001576CF
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x06003E57 RID: 15959 RVA: 0x001594D1 File Offset: 0x001576D1
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x06003E58 RID: 15960 RVA: 0x001594D3 File Offset: 0x001576D3
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x06003E59 RID: 15961 RVA: 0x001594D8 File Offset: 0x001576D8
	public HandleVector<int>.Handle Add(Pickupable pickupable)
	{
		Tag tag = pickupable.KPrefabID.PrefabID();
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId = null;
		if (!this.prefabIdToFetchables.TryGetValue(tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId = new FetchManager.FetchablesByPrefabId(tag);
			this.prefabIdToFetchables[tag] = fetchablesByPrefabId;
		}
		return fetchablesByPrefabId.AddPickupable(pickupable);
	}

	// Token: 0x06003E5A RID: 15962 RVA: 0x00159520 File Offset: 0x00157720
	public void Remove(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
	{
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
		if (this.prefabIdToFetchables.TryGetValue(prefab_tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId.RemovePickupable(fetchable_handle);
		}
	}

	// Token: 0x06003E5B RID: 15963 RVA: 0x00159544 File Offset: 0x00157744
	public void UpdateStorage(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle, Storage storage)
	{
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
		if (this.prefabIdToFetchables.TryGetValue(prefab_tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId.UpdateStorage(fetchable_handle, storage);
		}
	}

	// Token: 0x06003E5C RID: 15964 RVA: 0x00159569 File Offset: 0x00157769
	public void UpdateTags(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
	{
		this.prefabIdToFetchables[prefab_tag].UpdateTags(fetchable_handle);
	}

	// Token: 0x06003E5D RID: 15965 RVA: 0x00159580 File Offset: 0x00157780
	public void Sim1000ms(float dt)
	{
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair in this.prefabIdToFetchables)
		{
			keyValuePair.Value.Sim1000ms(dt);
		}
	}

	// Token: 0x06003E5E RID: 15966 RVA: 0x001595DC File Offset: 0x001577DC
	public void UpdatePickups(PathProber path_prober, WorkerBase worker)
	{
		Navigator component = worker.GetComponent<Navigator>();
		this.updateOffsetTables.Reset(null);
		this.updatePickupsWorkItems.Reset(null);
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair in this.prefabIdToFetchables)
		{
			FetchManager.FetchablesByPrefabId value = keyValuePair.Value;
			this.updateOffsetTables.Add(new FetchManager.UpdateOffsetTables(value));
			this.updatePickupsWorkItems.Add(new FetchManager.UpdatePickupWorkItem
			{
				fetchablesByPrefabId = value,
				pathProber = path_prober,
				navigator = component,
				worker = worker.gameObject
			});
		}
		GlobalJobManager.Run(this.updateOffsetTables);
		for (int i = 0; i < this.updateOffsetTables.Count; i++)
		{
			this.updateOffsetTables.GetWorkItem(i).Finish();
		}
		OffsetTracker.isExecutingWithinJob = true;
		GlobalJobManager.Run(this.updatePickupsWorkItems);
		OffsetTracker.isExecutingWithinJob = false;
		this.pickups.Clear();
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair2 in this.prefabIdToFetchables)
		{
			this.pickups.AddRange(keyValuePair2.Value.finalPickups);
		}
		this.pickups.Sort(FetchManager.ComparerNoPriority);
	}

	// Token: 0x06003E5F RID: 15967 RVA: 0x0015975C File Offset: 0x0015795C
	public static bool IsFetchablePickup(Pickupable pickup, FetchChore chore, Storage destination)
	{
		KPrefabID kprefabID = pickup.KPrefabID;
		Storage storage = pickup.storage;
		if (pickup.UnreservedAmount <= 0f)
		{
			return false;
		}
		if (kprefabID == null)
		{
			return false;
		}
		if (!pickup.isChoreAllowedToPickup(chore.choreType))
		{
			return false;
		}
		if (chore.criteria == FetchChore.MatchCriteria.MatchID && !chore.tags.Contains(kprefabID.PrefabTag))
		{
			return false;
		}
		if (chore.criteria == FetchChore.MatchCriteria.MatchTags && !kprefabID.HasTag(chore.tagsFirst))
		{
			return false;
		}
		if (chore.requiredTag.IsValid && !kprefabID.HasTag(chore.requiredTag))
		{
			return false;
		}
		if (kprefabID.HasAnyTags(chore.forbiddenTags))
		{
			return false;
		}
		if (kprefabID.HasTag(GameTags.MarkedForMove))
		{
			return false;
		}
		if (storage != null)
		{
			if (!storage.ignoreSourcePriority && destination.ShouldOnlyTransferFromLowerPriority && destination.masterPriority <= storage.masterPriority)
			{
				return false;
			}
			if (destination.storageNetworkID != -1 && destination.storageNetworkID == storage.storageNetworkID)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003E60 RID: 15968 RVA: 0x0015985C File Offset: 0x00157A5C
	public static Pickupable FindFetchTarget(List<Pickupable> pickupables, Storage destination, FetchChore chore)
	{
		foreach (Pickupable pickupable in pickupables)
		{
			if (FetchManager.IsFetchablePickup(pickupable, chore, destination))
			{
				return pickupable;
			}
		}
		return null;
	}

	// Token: 0x06003E61 RID: 15969 RVA: 0x001598B4 File Offset: 0x00157AB4
	public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
	{
		foreach (FetchManager.Pickup pickup in this.pickups)
		{
			if (FetchManager.IsFetchablePickup(pickup.pickupable, chore, destination))
			{
				return pickup.pickupable;
			}
		}
		return null;
	}

	// Token: 0x06003E62 RID: 15970 RVA: 0x0015991C File Offset: 0x00157B1C
	public static bool IsFetchablePickup_Exclude(KPrefabID pickup_id, Storage source, float pickup_unreserved_amount, HashSet<Tag> exclude_tags, Tag required_tag, Storage destination)
	{
		return FetchManager.IsFetchablePickup_Exclude(pickup_id, source, pickup_unreserved_amount, exclude_tags, new Tag[]
		{
			required_tag
		}, destination);
	}

	// Token: 0x06003E63 RID: 15971 RVA: 0x00159938 File Offset: 0x00157B38
	public static bool IsFetchablePickup_Exclude(KPrefabID pickup_id, Storage source, float pickup_unreserved_amount, HashSet<Tag> exclude_tags, Tag[] required_tags, Storage destination)
	{
		if (pickup_unreserved_amount <= 0f)
		{
			return false;
		}
		if (pickup_id == null)
		{
			return false;
		}
		if (exclude_tags.Contains(pickup_id.PrefabTag))
		{
			return false;
		}
		if (!pickup_id.HasAllTags(required_tags))
		{
			return false;
		}
		if (source != null)
		{
			if (!source.ignoreSourcePriority && destination.ShouldOnlyTransferFromLowerPriority && destination.masterPriority <= source.masterPriority)
			{
				return false;
			}
			if (destination.storageNetworkID != -1 && destination.storageNetworkID == source.storageNetworkID)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003E64 RID: 15972 RVA: 0x001599C2 File Offset: 0x00157BC2
	public Pickupable FindEdibleFetchTarget(Storage destination, HashSet<Tag> exclude_tags, Tag required_tag)
	{
		return this.FindEdibleFetchTarget(destination, exclude_tags, new Tag[]
		{
			required_tag
		});
	}

	// Token: 0x06003E65 RID: 15973 RVA: 0x001599DC File Offset: 0x00157BDC
	public Pickupable FindEdibleFetchTarget(Storage destination, HashSet<Tag> exclude_tags, Tag[] required_tags)
	{
		FetchManager.Pickup pickup = new FetchManager.Pickup
		{
			PathCost = ushort.MaxValue,
			foodQuality = int.MinValue
		};
		int num = int.MaxValue;
		foreach (FetchManager.Pickup pickup2 in this.pickups)
		{
			Pickupable pickupable = pickup2.pickupable;
			if (FetchManager.IsFetchablePickup_Exclude(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedAmount, exclude_tags, required_tags, destination))
			{
				int num2 = (int)pickup2.PathCost + (5 - pickup2.foodQuality) * 50;
				if (num2 < num)
				{
					pickup = pickup2;
					num = num2;
				}
			}
		}
		Navigator component = destination.GetComponent<Navigator>();
		if (component != null)
		{
			foreach (object obj in Components.FoodRehydrators)
			{
				GameObject gameObject = (GameObject)obj;
				int cell = Grid.PosToCell(gameObject);
				int cost = component.PathProber.GetCost(cell);
				if (cost != -1 && num > cost + 50 + 5)
				{
					AccessabilityManager accessabilityManager = (gameObject != null) ? gameObject.GetComponent<AccessabilityManager>() : null;
					if (accessabilityManager != null && accessabilityManager.CanAccess(destination.gameObject))
					{
						foreach (GameObject gameObject2 in gameObject.GetComponent<Storage>().items)
						{
							Storage storage = (gameObject2 != null) ? gameObject2.GetComponent<Storage>() : null;
							if (storage != null && !storage.IsEmpty())
							{
								Edible component2 = storage.items[0].GetComponent<Edible>();
								Pickupable component3 = component2.GetComponent<Pickupable>();
								if (FetchManager.IsFetchablePickup_Exclude(component3.KPrefabID, component3.storage, component3.UnreservedAmount, exclude_tags, required_tags, destination))
								{
									int num3 = cost + (5 - component2.FoodInfo.Quality + 1) * 50 + 5;
									if (num3 < num)
									{
										pickup.pickupable = component3;
										pickup.foodQuality = component2.FoodInfo.Quality;
										pickup.tagBitsHash = component2.PrefabID().GetHashCode();
										num = num3;
									}
								}
							}
						}
					}
				}
			}
		}
		return pickup.pickupable;
	}

	// Token: 0x0400265D RID: 9821
	private static readonly FetchManager.PickupComparerIncludingPriority ComparerIncludingPriority = new FetchManager.PickupComparerIncludingPriority();

	// Token: 0x0400265E RID: 9822
	private static readonly FetchManager.PickupComparerNoPriority ComparerNoPriority = new FetchManager.PickupComparerNoPriority();

	// Token: 0x0400265F RID: 9823
	private List<FetchManager.Pickup> pickups = new List<FetchManager.Pickup>();

	// Token: 0x04002660 RID: 9824
	public Dictionary<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchables = new Dictionary<Tag, FetchManager.FetchablesByPrefabId>();

	// Token: 0x04002661 RID: 9825
	private WorkItemCollection<FetchManager.UpdateOffsetTables, object> updateOffsetTables = new WorkItemCollection<FetchManager.UpdateOffsetTables, object>();

	// Token: 0x04002662 RID: 9826
	private WorkItemCollection<FetchManager.UpdatePickupWorkItem, object> updatePickupsWorkItems = new WorkItemCollection<FetchManager.UpdatePickupWorkItem, object>();

	// Token: 0x020017A7 RID: 6055
	public struct Fetchable
	{
		// Token: 0x0400734A RID: 29514
		public Pickupable pickupable;

		// Token: 0x0400734B RID: 29515
		public int tagBitsHash;

		// Token: 0x0400734C RID: 29516
		public int masterPriority;

		// Token: 0x0400734D RID: 29517
		public int freshness;

		// Token: 0x0400734E RID: 29518
		public int foodQuality;
	}

	// Token: 0x020017A8 RID: 6056
	[DebuggerDisplay("{pickupable.name}")]
	public struct Pickup
	{
		// Token: 0x0400734F RID: 29519
		public Pickupable pickupable;

		// Token: 0x04007350 RID: 29520
		public int tagBitsHash;

		// Token: 0x04007351 RID: 29521
		public ushort PathCost;

		// Token: 0x04007352 RID: 29522
		public int masterPriority;

		// Token: 0x04007353 RID: 29523
		public int freshness;

		// Token: 0x04007354 RID: 29524
		public int foodQuality;
	}

	// Token: 0x020017A9 RID: 6057
	private class PickupComparerIncludingPriority : IComparer<FetchManager.Pickup>
	{
		// Token: 0x0600964A RID: 38474 RVA: 0x00361550 File Offset: 0x0035F750
		public int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
		{
			int num = a.tagBitsHash.CompareTo(b.tagBitsHash);
			if (num != 0)
			{
				return num;
			}
			num = b.masterPriority.CompareTo(a.masterPriority);
			if (num != 0)
			{
				return num;
			}
			num = a.PathCost.CompareTo(b.PathCost);
			if (num != 0)
			{
				return num;
			}
			num = b.foodQuality.CompareTo(a.foodQuality);
			if (num != 0)
			{
				return num;
			}
			return b.freshness.CompareTo(a.freshness);
		}
	}

	// Token: 0x020017AA RID: 6058
	private class PickupComparerNoPriority : IComparer<FetchManager.Pickup>
	{
		// Token: 0x0600964C RID: 38476 RVA: 0x003615DC File Offset: 0x0035F7DC
		public int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
		{
			int num = a.PathCost.CompareTo(b.PathCost);
			if (num != 0)
			{
				return num;
			}
			num = b.foodQuality.CompareTo(a.foodQuality);
			if (num != 0)
			{
				return num;
			}
			return b.freshness.CompareTo(a.freshness);
		}
	}

	// Token: 0x020017AB RID: 6059
	public class FetchablesByPrefabId
	{
		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x0600964E RID: 38478 RVA: 0x00361635 File Offset: 0x0035F835
		// (set) Token: 0x0600964F RID: 38479 RVA: 0x0036163D File Offset: 0x0035F83D
		public Tag prefabId { get; private set; }

		// Token: 0x06009650 RID: 38480 RVA: 0x00361648 File Offset: 0x0035F848
		public FetchablesByPrefabId(Tag prefab_id)
		{
			this.prefabId = prefab_id;
			this.fetchables = new KCompactedVector<FetchManager.Fetchable>(0);
			this.rotUpdaters = new Dictionary<HandleVector<int>.Handle, Rottable.Instance>();
			this.finalPickups = new List<FetchManager.Pickup>();
		}

		// Token: 0x06009651 RID: 38481 RVA: 0x003616A8 File Offset: 0x0035F8A8
		public HandleVector<int>.Handle AddPickupable(Pickupable pickupable)
		{
			int foodQuality = 5;
			Edible component = pickupable.GetComponent<Edible>();
			if (component != null)
			{
				foodQuality = component.GetQuality();
			}
			int masterPriority = 0;
			if (pickupable.storage != null)
			{
				Prioritizable prioritizable = pickupable.storage.prioritizable;
				if (prioritizable != null)
				{
					masterPriority = prioritizable.GetMasterPriority().priority_value;
				}
			}
			Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
			int freshness = 0;
			if (!smi.IsNullOrStopped())
			{
				freshness = FetchManager.QuantizeRotValue(smi.RotValue);
			}
			KPrefabID kprefabID = pickupable.KPrefabID;
			HandleVector<int>.Handle handle = this.fetchables.Allocate(new FetchManager.Fetchable
			{
				pickupable = pickupable,
				foodQuality = foodQuality,
				freshness = freshness,
				masterPriority = masterPriority,
				tagBitsHash = kprefabID.GetTagsHash()
			});
			if (!smi.IsNullOrStopped())
			{
				this.rotUpdaters[handle] = smi;
			}
			return handle;
		}

		// Token: 0x06009652 RID: 38482 RVA: 0x0036178B File Offset: 0x0035F98B
		public void RemovePickupable(HandleVector<int>.Handle fetchable_handle)
		{
			this.fetchables.Free(fetchable_handle);
			this.rotUpdaters.Remove(fetchable_handle);
		}

		// Token: 0x06009653 RID: 38483 RVA: 0x003617A8 File Offset: 0x0035F9A8
		public void UpdatePickups(PathProber path_prober, Navigator worker_navigator, GameObject worker_go)
		{
			this.GatherPickupablesWhichCanBePickedUp(worker_go);
			this.GatherReachablePickups(worker_navigator);
			this.finalPickups.Sort(FetchManager.ComparerIncludingPriority);
			if (this.finalPickups.Count > 0)
			{
				FetchManager.Pickup pickup = this.finalPickups[0];
				int num = pickup.tagBitsHash;
				int num2 = this.finalPickups.Count;
				int num3 = 0;
				for (int i = 1; i < this.finalPickups.Count; i++)
				{
					bool flag = false;
					FetchManager.Pickup pickup2 = this.finalPickups[i];
					int tagBitsHash = pickup2.tagBitsHash;
					if (pickup.masterPriority == pickup2.masterPriority && tagBitsHash == num)
					{
						flag = true;
					}
					if (flag)
					{
						num2--;
					}
					else
					{
						num3++;
						pickup = pickup2;
						num = tagBitsHash;
						if (i > num3)
						{
							this.finalPickups[num3] = pickup2;
						}
					}
				}
				this.finalPickups.RemoveRange(num2, this.finalPickups.Count - num2);
			}
		}

		// Token: 0x06009654 RID: 38484 RVA: 0x00361894 File Offset: 0x0035FA94
		private void GatherPickupablesWhichCanBePickedUp(GameObject worker_go)
		{
			this.pickupsWhichCanBePickedUp.Clear();
			foreach (FetchManager.Fetchable fetchable in this.fetchables.GetDataList())
			{
				Pickupable pickupable = fetchable.pickupable;
				if (pickupable.CouldBePickedUpByMinion(worker_go))
				{
					this.pickupsWhichCanBePickedUp.Add(new FetchManager.Pickup
					{
						pickupable = pickupable,
						tagBitsHash = fetchable.tagBitsHash,
						PathCost = ushort.MaxValue,
						masterPriority = fetchable.masterPriority,
						freshness = fetchable.freshness,
						foodQuality = fetchable.foodQuality
					});
				}
			}
		}

		// Token: 0x06009655 RID: 38485 RVA: 0x0036195C File Offset: 0x0035FB5C
		public void UpdateOffsetTables()
		{
			foreach (FetchManager.Fetchable fetchable in this.fetchables.GetDataList())
			{
				fetchable.pickupable.GetOffsets(fetchable.pickupable.cachedCell);
			}
		}

		// Token: 0x06009656 RID: 38486 RVA: 0x003619C4 File Offset: 0x0035FBC4
		private void GatherReachablePickups(Navigator navigator)
		{
			this.cellCosts.Clear();
			this.finalPickups.Clear();
			foreach (FetchManager.Pickup pickup in this.pickupsWhichCanBePickedUp)
			{
				Pickupable pickupable = pickup.pickupable;
				int num = -1;
				if (!this.cellCosts.TryGetValue(pickupable.cachedCell, out num))
				{
					num = pickupable.GetNavigationCost(navigator, pickupable.cachedCell);
					this.cellCosts[pickupable.cachedCell] = num;
				}
				if (num != -1)
				{
					this.finalPickups.Add(new FetchManager.Pickup
					{
						pickupable = pickupable,
						tagBitsHash = pickup.tagBitsHash,
						PathCost = (ushort)num,
						masterPriority = pickup.masterPriority,
						freshness = pickup.freshness,
						foodQuality = pickup.foodQuality
					});
				}
			}
		}

		// Token: 0x06009657 RID: 38487 RVA: 0x00361AC8 File Offset: 0x0035FCC8
		public void UpdateStorage(HandleVector<int>.Handle fetchable_handle, Storage storage)
		{
			FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
			int masterPriority = 0;
			Pickupable pickupable = data.pickupable;
			if (pickupable.storage != null)
			{
				Prioritizable prioritizable = pickupable.storage.prioritizable;
				if (prioritizable != null)
				{
					masterPriority = prioritizable.GetMasterPriority().priority_value;
				}
			}
			data.masterPriority = masterPriority;
			this.fetchables.SetData(fetchable_handle, data);
		}

		// Token: 0x06009658 RID: 38488 RVA: 0x00361B34 File Offset: 0x0035FD34
		public void UpdateTags(HandleVector<int>.Handle fetchable_handle)
		{
			FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
			data.tagBitsHash = data.pickupable.KPrefabID.GetTagsHash();
			this.fetchables.SetData(fetchable_handle, data);
		}

		// Token: 0x06009659 RID: 38489 RVA: 0x00361B74 File Offset: 0x0035FD74
		public void Sim1000ms(float dt)
		{
			foreach (KeyValuePair<HandleVector<int>.Handle, Rottable.Instance> keyValuePair in this.rotUpdaters)
			{
				HandleVector<int>.Handle key = keyValuePair.Key;
				Rottable.Instance value = keyValuePair.Value;
				FetchManager.Fetchable data = this.fetchables.GetData(key);
				data.freshness = FetchManager.QuantizeRotValue(value.RotValue);
				this.fetchables.SetData(key, data);
			}
		}

		// Token: 0x04007355 RID: 29525
		public KCompactedVector<FetchManager.Fetchable> fetchables;

		// Token: 0x04007356 RID: 29526
		public List<FetchManager.Pickup> finalPickups = new List<FetchManager.Pickup>();

		// Token: 0x04007357 RID: 29527
		private Dictionary<HandleVector<int>.Handle, Rottable.Instance> rotUpdaters;

		// Token: 0x04007358 RID: 29528
		private List<FetchManager.Pickup> pickupsWhichCanBePickedUp = new List<FetchManager.Pickup>();

		// Token: 0x04007359 RID: 29529
		private Dictionary<int, int> cellCosts = new Dictionary<int, int>();
	}

	// Token: 0x020017AC RID: 6060
	private struct UpdateOffsetTables : IWorkItem<object>
	{
		// Token: 0x0600965A RID: 38490 RVA: 0x00361C00 File Offset: 0x0035FE00
		public UpdateOffsetTables(FetchManager.FetchablesByPrefabId fetchables)
		{
			this.data = fetchables;
			this.failed = ListPool<Pickupable, FetchManager.UpdateOffsetTables>.Allocate();
		}

		// Token: 0x0600965B RID: 38491 RVA: 0x00361C14 File Offset: 0x0035FE14
		public void Run(object _)
		{
			if (Game.IsOnMainThread())
			{
				this.data.UpdateOffsetTables();
				return;
			}
			foreach (FetchManager.Fetchable fetchable in this.data.fetchables.GetDataList())
			{
				if (!fetchable.pickupable.ValidateOffsets(fetchable.pickupable.cachedCell))
				{
					this.failed.Add(fetchable.pickupable);
				}
			}
		}

		// Token: 0x0600965C RID: 38492 RVA: 0x00361CA8 File Offset: 0x0035FEA8
		public void Finish()
		{
			foreach (Pickupable pickupable in this.failed)
			{
				pickupable.GetOffsets(pickupable.cachedCell);
			}
			this.failed.Recycle();
		}

		// Token: 0x0400735B RID: 29531
		public FetchManager.FetchablesByPrefabId data;

		// Token: 0x0400735C RID: 29532
		private ListPool<Pickupable, FetchManager.UpdateOffsetTables>.PooledList failed;
	}

	// Token: 0x020017AD RID: 6061
	private struct UpdatePickupWorkItem : IWorkItem<object>
	{
		// Token: 0x0600965D RID: 38493 RVA: 0x00361D0C File Offset: 0x0035FF0C
		public void Run(object shared_data)
		{
			this.fetchablesByPrefabId.UpdatePickups(this.pathProber, this.navigator, this.worker);
		}

		// Token: 0x0400735D RID: 29533
		public FetchManager.FetchablesByPrefabId fetchablesByPrefabId;

		// Token: 0x0400735E RID: 29534
		public PathProber pathProber;

		// Token: 0x0400735F RID: 29535
		public Navigator navigator;

		// Token: 0x04007360 RID: 29536
		public GameObject worker;
	}
}
