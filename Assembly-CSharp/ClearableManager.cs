using System;
using System.Collections.Generic;

// Token: 0x0200046D RID: 1133
internal class ClearableManager
{
	// Token: 0x0600186B RID: 6251 RVA: 0x00082944 File Offset: 0x00080B44
	public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
	{
		return this.markedClearables.Allocate(new ClearableManager.MarkedClearable
		{
			clearable = clearable,
			pickupable = clearable.GetComponent<Pickupable>(),
			prioritizable = clearable.GetComponent<Prioritizable>()
		});
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x00082987 File Offset: 0x00080B87
	public void UnregisterClearable(HandleVector<int>.Handle handle)
	{
		this.markedClearables.Free(handle);
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x00082998 File Offset: 0x00080B98
	public void CollectAndSortClearables(Navigator navigator)
	{
		this.sortedClearables.Clear();
		foreach (ClearableManager.MarkedClearable markedClearable in this.markedClearables.GetDataList())
		{
			int navigationCost = markedClearable.pickupable.GetNavigationCost(navigator, markedClearable.pickupable.cachedCell);
			if (navigationCost != -1)
			{
				this.sortedClearables.Add(new ClearableManager.SortedClearable
				{
					pickupable = markedClearable.pickupable,
					masterPriority = markedClearable.prioritizable.GetMasterPriority(),
					cost = navigationCost
				});
			}
		}
		this.sortedClearables.Sort(ClearableManager.SortedClearable.comparer);
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x00082A5C File Offset: 0x00080C5C
	public void CollectChores(List<GlobalChoreProvider.Fetch> fetches, ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		ChoreType transport = Db.Get().ChoreTypes.Transport;
		int personalPriority = consumer_state.consumer.GetPersonalPriority(transport);
		int priority = Game.Instance.advancedPersonalPriorities ? transport.explicitPriority : transport.priority;
		bool flag = false;
		for (int i = 0; i < this.sortedClearables.Count; i++)
		{
			ClearableManager.SortedClearable sortedClearable = this.sortedClearables[i];
			Pickupable pickupable = sortedClearable.pickupable;
			PrioritySetting masterPriority = sortedClearable.masterPriority;
			Chore.Precondition.Context item = default(Chore.Precondition.Context);
			item.personalPriority = personalPriority;
			KPrefabID kprefabID = pickupable.KPrefabID;
			int num = 0;
			while (fetches != null && num < fetches.Count)
			{
				GlobalChoreProvider.Fetch fetch = fetches[num];
				if ((fetch.chore.criteria == FetchChore.MatchCriteria.MatchID && fetch.chore.tags.Contains(kprefabID.PrefabTag)) || (fetch.chore.criteria == FetchChore.MatchCriteria.MatchTags && kprefabID.HasTag(fetch.chore.tagsFirst)))
				{
					item.Set(fetch.chore, consumer_state, false, pickupable);
					item.choreTypeForPermission = transport;
					item.RunPreconditions();
					if (item.IsSuccess())
					{
						item.masterPriority = masterPriority;
						item.priority = priority;
						item.interruptPriority = transport.interruptPriority;
						succeeded.Add(item);
						flag = true;
						break;
					}
				}
				num++;
			}
			if (flag)
			{
				break;
			}
		}
	}

	// Token: 0x04000D8D RID: 3469
	private KCompactedVector<ClearableManager.MarkedClearable> markedClearables = new KCompactedVector<ClearableManager.MarkedClearable>(0);

	// Token: 0x04000D8E RID: 3470
	private List<ClearableManager.SortedClearable> sortedClearables = new List<ClearableManager.SortedClearable>();

	// Token: 0x02001229 RID: 4649
	private struct MarkedClearable
	{
		// Token: 0x04006295 RID: 25237
		public Clearable clearable;

		// Token: 0x04006296 RID: 25238
		public Pickupable pickupable;

		// Token: 0x04006297 RID: 25239
		public Prioritizable prioritizable;
	}

	// Token: 0x0200122A RID: 4650
	private struct SortedClearable
	{
		// Token: 0x04006298 RID: 25240
		public Pickupable pickupable;

		// Token: 0x04006299 RID: 25241
		public PrioritySetting masterPriority;

		// Token: 0x0400629A RID: 25242
		public int cost;

		// Token: 0x0400629B RID: 25243
		public static ClearableManager.SortedClearable.Comparer comparer = new ClearableManager.SortedClearable.Comparer();

		// Token: 0x020023FC RID: 9212
		public class Comparer : IComparer<ClearableManager.SortedClearable>
		{
			// Token: 0x0600B888 RID: 47240 RVA: 0x003CF1F4 File Offset: 0x003CD3F4
			public int Compare(ClearableManager.SortedClearable a, ClearableManager.SortedClearable b)
			{
				int num = b.masterPriority.priority_value - a.masterPriority.priority_value;
				if (num == 0)
				{
					return a.cost - b.cost;
				}
				return num;
			}
		}
	}
}
