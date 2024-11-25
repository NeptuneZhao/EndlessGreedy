using System;
using System.Collections.Generic;

// Token: 0x0200046C RID: 1132
public class GlobalChoreProvider : ChoreProvider, IRender200ms
{
	// Token: 0x0600185B RID: 6235 RVA: 0x00082327 File Offset: 0x00080527
	public static void DestroyInstance()
	{
		GlobalChoreProvider.Instance = null;
	}

	// Token: 0x0600185C RID: 6236 RVA: 0x0008232F File Offset: 0x0008052F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GlobalChoreProvider.Instance = this;
		this.clearableManager = new ClearableManager();
	}

	// Token: 0x0600185D RID: 6237 RVA: 0x00082348 File Offset: 0x00080548
	protected override void OnWorldRemoved(object data)
	{
		int num = (int)data;
		int parentWorldId = ClusterManager.Instance.GetWorld(num).ParentWorldId;
		List<FetchChore> chores;
		if (this.fetchMap.TryGetValue(parentWorldId, out chores))
		{
			base.ClearWorldChores<FetchChore>(chores, num);
		}
		base.OnWorldRemoved(data);
	}

	// Token: 0x0600185E RID: 6238 RVA: 0x0008238C File Offset: 0x0008058C
	protected override void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		if (worldParentChangedEventArgs == null || worldParentChangedEventArgs.lastParentId == 255)
		{
			return;
		}
		base.OnWorldParentChanged(data);
		List<FetchChore> oldChores;
		if (!this.fetchMap.TryGetValue(worldParentChangedEventArgs.lastParentId, out oldChores))
		{
			return;
		}
		List<FetchChore> newChores;
		if (!this.fetchMap.TryGetValue(worldParentChangedEventArgs.world.ParentWorldId, out newChores))
		{
			newChores = (this.fetchMap[worldParentChangedEventArgs.world.ParentWorldId] = new List<FetchChore>());
		}
		base.TransferChores<FetchChore>(oldChores, newChores, worldParentChangedEventArgs.world.ParentWorldId);
	}

	// Token: 0x0600185F RID: 6239 RVA: 0x00082418 File Offset: 0x00080618
	public override void AddChore(Chore chore)
	{
		FetchChore fetchChore = chore as FetchChore;
		if (fetchChore != null)
		{
			int myParentWorldId = fetchChore.gameObject.GetMyParentWorldId();
			List<FetchChore> list;
			if (!this.fetchMap.TryGetValue(myParentWorldId, out list))
			{
				list = (this.fetchMap[myParentWorldId] = new List<FetchChore>());
			}
			chore.provider = this;
			list.Add(fetchChore);
			return;
		}
		base.AddChore(chore);
	}

	// Token: 0x06001860 RID: 6240 RVA: 0x00082474 File Offset: 0x00080674
	public override void RemoveChore(Chore chore)
	{
		FetchChore fetchChore = chore as FetchChore;
		if (fetchChore != null)
		{
			int myParentWorldId = fetchChore.gameObject.GetMyParentWorldId();
			List<FetchChore> list;
			if (this.fetchMap.TryGetValue(myParentWorldId, out list))
			{
				list.Remove(fetchChore);
			}
			chore.provider = null;
			return;
		}
		base.RemoveChore(chore);
	}

	// Token: 0x06001861 RID: 6241 RVA: 0x000824C0 File Offset: 0x000806C0
	public void UpdateFetches(PathProber path_prober)
	{
		List<FetchChore> list = null;
		int myParentWorldId = path_prober.gameObject.GetMyParentWorldId();
		if (!this.fetchMap.TryGetValue(myParentWorldId, out list))
		{
			return;
		}
		this.fetches.Clear();
		Navigator component = path_prober.GetComponent<Navigator>();
		for (int i = list.Count - 1; i >= 0; i--)
		{
			FetchChore fetchChore = list[i];
			if (!(fetchChore.driver != null) && (!(fetchChore.automatable != null) || !fetchChore.automatable.GetAutomationOnly()))
			{
				if (fetchChore.provider == null)
				{
					fetchChore.Cancel("no provider");
					list[i] = list[list.Count - 1];
					list.RemoveAt(list.Count - 1);
				}
				else
				{
					Storage destination = fetchChore.destination;
					if (!(destination == null))
					{
						int navigationCost = component.GetNavigationCost(destination);
						if (navigationCost != -1)
						{
							this.fetches.Add(new GlobalChoreProvider.Fetch
							{
								chore = fetchChore,
								idsHash = fetchChore.tagsHash,
								cost = navigationCost,
								priority = fetchChore.masterPriority,
								category = destination.fetchCategory
							});
						}
					}
				}
			}
		}
		if (this.fetches.Count > 0)
		{
			this.fetches.Sort(GlobalChoreProvider.Comparer);
			int j = 1;
			int num = 0;
			while (j < this.fetches.Count)
			{
				if (!this.fetches[num].IsBetterThan(this.fetches[j]))
				{
					num++;
					this.fetches[num] = this.fetches[j];
				}
				j++;
			}
			this.fetches.RemoveRange(num + 1, this.fetches.Count - num - 1);
		}
		this.clearableManager.CollectAndSortClearables(component);
	}

	// Token: 0x06001862 RID: 6242 RVA: 0x000826B4 File Offset: 0x000808B4
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		base.CollectChores(consumer_state, succeeded, failed_contexts);
		this.clearableManager.CollectChores(this.fetches, consumer_state, succeeded, failed_contexts);
		int num = CPUBudget.coreCount * 4;
		if (this.fetches.Count > num)
		{
			GlobalChoreProvider.batch_fetch_collector.Reset(this);
			int coreCount = CPUBudget.coreCount;
			int num2 = Math.Min(16, this.fetches.Count / coreCount);
			for (int i = 0; i < this.fetches.Count; i += num2)
			{
				GlobalChoreProvider.batch_fetch_collector.Add(new GlobalChoreProvider.FetchChoreCollectTask(i, Math.Min(i + num2, this.fetches.Count), consumer_state));
			}
			GlobalJobManager.Run(GlobalChoreProvider.batch_fetch_collector);
			for (int j = 0; j < coreCount; j++)
			{
				GlobalChoreProvider.batch_fetch_collector.GetWorkItem(j).Finish(succeeded, failed_contexts);
			}
			return;
		}
		for (int k = 0; k < this.fetches.Count; k++)
		{
			this.fetches[k].chore.CollectChoresFromGlobalChoreProvider(consumer_state, succeeded, failed_contexts, false);
		}
	}

	// Token: 0x06001863 RID: 6243 RVA: 0x000827BE File Offset: 0x000809BE
	public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
	{
		return this.clearableManager.RegisterClearable(clearable);
	}

	// Token: 0x06001864 RID: 6244 RVA: 0x000827CC File Offset: 0x000809CC
	public void UnregisterClearable(HandleVector<int>.Handle handle)
	{
		this.clearableManager.UnregisterClearable(handle);
	}

	// Token: 0x06001865 RID: 6245 RVA: 0x000827DA File Offset: 0x000809DA
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
		GlobalChoreProvider.Instance = null;
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x000827E8 File Offset: 0x000809E8
	public void Render200ms(float dt)
	{
		this.UpdateStorageFetchableBits();
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x000827F0 File Offset: 0x000809F0
	private void UpdateStorageFetchableBits()
	{
		ChoreType storageFetch = Db.Get().ChoreTypes.StorageFetch;
		ChoreType foodFetch = Db.Get().ChoreTypes.FoodFetch;
		this.storageFetchableTags.Clear();
		List<int> worldIDsSorted = ClusterManager.Instance.GetWorldIDsSorted();
		for (int i = 0; i < worldIDsSorted.Count; i++)
		{
			List<FetchChore> list;
			if (this.fetchMap.TryGetValue(worldIDsSorted[i], out list))
			{
				for (int j = 0; j < list.Count; j++)
				{
					FetchChore fetchChore = list[j];
					if ((fetchChore.choreType == storageFetch || fetchChore.choreType == foodFetch) && fetchChore.destination)
					{
						int cell = Grid.PosToCell(fetchChore.destination);
						if (MinionGroupProber.Get().IsReachable(cell, fetchChore.destination.GetOffsets(cell)))
						{
							this.storageFetchableTags.UnionWith(fetchChore.tags);
						}
					}
				}
			}
		}
	}

	// Token: 0x06001868 RID: 6248 RVA: 0x000828E0 File Offset: 0x00080AE0
	public bool ClearableHasDestination(Pickupable pickupable)
	{
		KPrefabID kprefabID = pickupable.KPrefabID;
		return this.storageFetchableTags.Contains(kprefabID.PrefabTag);
	}

	// Token: 0x04000D86 RID: 3462
	public static GlobalChoreProvider Instance;

	// Token: 0x04000D87 RID: 3463
	public Dictionary<int, List<FetchChore>> fetchMap = new Dictionary<int, List<FetchChore>>();

	// Token: 0x04000D88 RID: 3464
	public List<GlobalChoreProvider.Fetch> fetches = new List<GlobalChoreProvider.Fetch>();

	// Token: 0x04000D89 RID: 3465
	private static readonly GlobalChoreProvider.FetchComparer Comparer = new GlobalChoreProvider.FetchComparer();

	// Token: 0x04000D8A RID: 3466
	private ClearableManager clearableManager;

	// Token: 0x04000D8B RID: 3467
	private HashSet<Tag> storageFetchableTags = new HashSet<Tag>();

	// Token: 0x04000D8C RID: 3468
	private static WorkItemCollection<GlobalChoreProvider.FetchChoreCollectTask, GlobalChoreProvider> batch_fetch_collector = new WorkItemCollection<GlobalChoreProvider.FetchChoreCollectTask, GlobalChoreProvider>();

	// Token: 0x02001225 RID: 4645
	public struct Fetch
	{
		// Token: 0x06008252 RID: 33362 RVA: 0x0031C940 File Offset: 0x0031AB40
		public bool IsBetterThan(GlobalChoreProvider.Fetch fetch)
		{
			if (this.category != fetch.category)
			{
				return false;
			}
			if (this.idsHash != fetch.idsHash)
			{
				return false;
			}
			if (this.chore.choreType != fetch.chore.choreType)
			{
				return false;
			}
			if (this.priority.priority_class > fetch.priority.priority_class)
			{
				return true;
			}
			if (this.priority.priority_class == fetch.priority.priority_class)
			{
				if (this.priority.priority_value > fetch.priority.priority_value)
				{
					return true;
				}
				if (this.priority.priority_value == fetch.priority.priority_value)
				{
					return this.cost <= fetch.cost;
				}
			}
			return false;
		}

		// Token: 0x04006285 RID: 25221
		public FetchChore chore;

		// Token: 0x04006286 RID: 25222
		public int idsHash;

		// Token: 0x04006287 RID: 25223
		public int cost;

		// Token: 0x04006288 RID: 25224
		public PrioritySetting priority;

		// Token: 0x04006289 RID: 25225
		public Storage.FetchCategory category;
	}

	// Token: 0x02001226 RID: 4646
	private struct FetchChoreCollectTask : IWorkItem<GlobalChoreProvider>
	{
		// Token: 0x06008253 RID: 33363 RVA: 0x0031C9FE File Offset: 0x0031ABFE
		public FetchChoreCollectTask(int start, int end, ChoreConsumerState consumer_state)
		{
			this.start = start;
			this.end = end;
			this.consumer_state = consumer_state;
			this.succeeded = ListPool<Chore.Precondition.Context, GlobalChoreProvider.FetchChoreCollectTask>.Allocate();
			this.failed = ListPool<Chore.Precondition.Context, GlobalChoreProvider.FetchChoreCollectTask>.Allocate();
			this.incomplete = ListPool<Chore.Precondition.Context, GlobalChoreProvider.FetchChoreCollectTask>.Allocate();
		}

		// Token: 0x06008254 RID: 33364 RVA: 0x0031CA38 File Offset: 0x0031AC38
		public void Run(GlobalChoreProvider context)
		{
			for (int i = this.start; i < this.end; i++)
			{
				context.fetches[i].chore.CollectChoresFromGlobalChoreProvider(this.consumer_state, this.succeeded, this.incomplete, this.failed, false);
			}
		}

		// Token: 0x06008255 RID: 33365 RVA: 0x0031CA8C File Offset: 0x0031AC8C
		public void Finish(List<Chore.Precondition.Context> combined_succeeded, List<Chore.Precondition.Context> combined_failed)
		{
			combined_succeeded.AddRange(this.succeeded);
			this.succeeded.Clear();
			this.succeeded.Recycle();
			combined_failed.AddRange(this.failed);
			this.failed.Clear();
			this.failed.Recycle();
			foreach (Chore.Precondition.Context item in this.incomplete)
			{
				item.FinishPreconditions();
				if (item.IsSuccess())
				{
					combined_succeeded.Add(item);
				}
				else
				{
					combined_failed.Add(item);
				}
			}
			this.incomplete.Clear();
			this.incomplete.Recycle();
		}

		// Token: 0x0400628A RID: 25226
		private int start;

		// Token: 0x0400628B RID: 25227
		private int end;

		// Token: 0x0400628C RID: 25228
		private ChoreConsumerState consumer_state;

		// Token: 0x0400628D RID: 25229
		public ListPool<Chore.Precondition.Context, GlobalChoreProvider.FetchChoreCollectTask>.PooledList succeeded;

		// Token: 0x0400628E RID: 25230
		public ListPool<Chore.Precondition.Context, GlobalChoreProvider.FetchChoreCollectTask>.PooledList failed;

		// Token: 0x0400628F RID: 25231
		public ListPool<Chore.Precondition.Context, GlobalChoreProvider.FetchChoreCollectTask>.PooledList incomplete;
	}

	// Token: 0x02001227 RID: 4647
	private class FetchComparer : IComparer<GlobalChoreProvider.Fetch>
	{
		// Token: 0x06008256 RID: 33366 RVA: 0x0031CB54 File Offset: 0x0031AD54
		public int Compare(GlobalChoreProvider.Fetch a, GlobalChoreProvider.Fetch b)
		{
			int num = b.priority.priority_class - a.priority.priority_class;
			if (num != 0)
			{
				return num;
			}
			int num2 = b.priority.priority_value - a.priority.priority_value;
			if (num2 != 0)
			{
				return num2;
			}
			return a.cost - b.cost;
		}
	}

	// Token: 0x02001228 RID: 4648
	private struct FindTopPriorityTask : IWorkItem<object>
	{
		// Token: 0x06008258 RID: 33368 RVA: 0x0031CBB0 File Offset: 0x0031ADB0
		public FindTopPriorityTask(int start, int end, List<Prioritizable> worldCollection)
		{
			this.start = start;
			this.end = end;
			this.worldCollection = worldCollection;
			this.found = false;
		}

		// Token: 0x06008259 RID: 33369 RVA: 0x0031CBD0 File Offset: 0x0031ADD0
		public void Run(object context)
		{
			if (GlobalChoreProvider.FindTopPriorityTask.abort)
			{
				return;
			}
			int num = this.start;
			while (num != this.end && this.worldCollection.Count > num)
			{
				if (!(this.worldCollection[num] == null) && this.worldCollection[num].IsTopPriority())
				{
					this.found = true;
					break;
				}
				num++;
			}
			if (this.found)
			{
				GlobalChoreProvider.FindTopPriorityTask.abort = true;
			}
		}

		// Token: 0x04006290 RID: 25232
		private int start;

		// Token: 0x04006291 RID: 25233
		private int end;

		// Token: 0x04006292 RID: 25234
		private List<Prioritizable> worldCollection;

		// Token: 0x04006293 RID: 25235
		public bool found;

		// Token: 0x04006294 RID: 25236
		public static bool abort;
	}
}
