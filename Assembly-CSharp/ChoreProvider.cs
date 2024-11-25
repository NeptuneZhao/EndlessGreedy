using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000469 RID: 1129
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreProvider")]
public class ChoreProvider : KMonoBehaviour
{
	// Token: 0x17000091 RID: 145
	// (get) Token: 0x06001841 RID: 6209 RVA: 0x00081A2F File Offset: 0x0007FC2F
	// (set) Token: 0x06001842 RID: 6210 RVA: 0x00081A37 File Offset: 0x0007FC37
	public string Name { get; private set; }

	// Token: 0x06001843 RID: 6211 RVA: 0x00081A40 File Offset: 0x0007FC40
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Game.Instance.Subscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		Game.Instance.Subscribe(586301400, new Action<object>(this.OnMinionMigrated));
		Game.Instance.Subscribe(1142724171, new Action<object>(this.OnEntityMigrated));
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x00081AAA File Offset: 0x0007FCAA
	protected override void OnSpawn()
	{
		if (ClusterManager.Instance != null)
		{
			ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		}
		base.OnSpawn();
		this.Name = base.name;
	}

	// Token: 0x06001845 RID: 6213 RVA: 0x00081AE8 File Offset: 0x0007FCE8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		Game.Instance.Unsubscribe(586301400, new Action<object>(this.OnMinionMigrated));
		Game.Instance.Unsubscribe(1142724171, new Action<object>(this.OnEntityMigrated));
		if (ClusterManager.Instance != null)
		{
			ClusterManager.Instance.Unsubscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		}
	}

	// Token: 0x06001846 RID: 6214 RVA: 0x00081B78 File Offset: 0x0007FD78
	protected virtual void OnWorldRemoved(object data)
	{
		int num = (int)data;
		int parentWorldId = ClusterManager.Instance.GetWorld(num).ParentWorldId;
		List<Chore> chores;
		if (this.choreWorldMap.TryGetValue(parentWorldId, out chores))
		{
			this.ClearWorldChores<Chore>(chores, num);
		}
	}

	// Token: 0x06001847 RID: 6215 RVA: 0x00081BB8 File Offset: 0x0007FDB8
	protected virtual void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		List<Chore> oldChores;
		if (worldParentChangedEventArgs == null || worldParentChangedEventArgs.lastParentId == 255 || worldParentChangedEventArgs.lastParentId == worldParentChangedEventArgs.world.ParentWorldId || !this.choreWorldMap.TryGetValue(worldParentChangedEventArgs.lastParentId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(worldParentChangedEventArgs.world.ParentWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[worldParentChangedEventArgs.world.ParentWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, worldParentChangedEventArgs.world.ParentWorldId);
	}

	// Token: 0x06001848 RID: 6216 RVA: 0x00081C60 File Offset: 0x0007FE60
	protected virtual void OnEntityMigrated(object data)
	{
		MigrationEventArgs migrationEventArgs = data as MigrationEventArgs;
		List<Chore> oldChores;
		if (migrationEventArgs == null || !(migrationEventArgs.entity == base.gameObject) || migrationEventArgs.prevWorldId == migrationEventArgs.targetWorldId || !this.choreWorldMap.TryGetValue(migrationEventArgs.prevWorldId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(migrationEventArgs.targetWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[migrationEventArgs.targetWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, migrationEventArgs.targetWorldId);
	}

	// Token: 0x06001849 RID: 6217 RVA: 0x00081CF4 File Offset: 0x0007FEF4
	protected virtual void OnMinionMigrated(object data)
	{
		MinionMigrationEventArgs minionMigrationEventArgs = data as MinionMigrationEventArgs;
		List<Chore> oldChores;
		if (minionMigrationEventArgs == null || !(minionMigrationEventArgs.minionId.gameObject == base.gameObject) || minionMigrationEventArgs.prevWorldId == minionMigrationEventArgs.targetWorldId || !this.choreWorldMap.TryGetValue(minionMigrationEventArgs.prevWorldId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(minionMigrationEventArgs.targetWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[minionMigrationEventArgs.targetWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, minionMigrationEventArgs.targetWorldId);
	}

	// Token: 0x0600184A RID: 6218 RVA: 0x00081D90 File Offset: 0x0007FF90
	protected void TransferChores<T>(List<T> oldChores, List<T> newChores, int transferId) where T : Chore
	{
		int num = oldChores.Count - 1;
		for (int i = num; i >= 0; i--)
		{
			T t = oldChores[i];
			if (t.isNull)
			{
				DebugUtil.DevLogError(string.Concat(new string[]
				{
					"[",
					t.GetType().Name,
					"] ",
					t.GetReportName(null),
					" has no target"
				}));
			}
			else if (t.gameObject.GetMyParentWorldId() == transferId)
			{
				newChores.Add(t);
				oldChores[i] = oldChores[num];
				oldChores.RemoveAt(num--);
			}
		}
	}

	// Token: 0x0600184B RID: 6219 RVA: 0x00081E4C File Offset: 0x0008004C
	protected void ClearWorldChores<T>(List<T> chores, int worldId) where T : Chore
	{
		int num = chores.Count - 1;
		for (int i = num; i >= 0; i--)
		{
			if (chores[i].gameObject.GetMyWorldId() == worldId)
			{
				chores[i] = chores[num];
				chores.RemoveAt(num--);
			}
		}
	}

	// Token: 0x0600184C RID: 6220 RVA: 0x00081EA0 File Offset: 0x000800A0
	public virtual void AddChore(Chore chore)
	{
		chore.provider = this;
		List<Chore> list = null;
		int myParentWorldId = chore.gameObject.GetMyParentWorldId();
		if (!this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			list = (this.choreWorldMap[myParentWorldId] = new List<Chore>());
		}
		list.Add(chore);
	}

	// Token: 0x0600184D RID: 6221 RVA: 0x00081EEC File Offset: 0x000800EC
	public virtual void RemoveChore(Chore chore)
	{
		if (chore == null)
		{
			return;
		}
		chore.provider = null;
		List<Chore> list = null;
		int myParentWorldId = chore.gameObject.GetMyParentWorldId();
		if (this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			list.Remove(chore);
		}
	}

	// Token: 0x0600184E RID: 6222 RVA: 0x00081F2C File Offset: 0x0008012C
	public virtual void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		List<Chore> list = null;
		int myParentWorldId = consumer_state.gameObject.GetMyParentWorldId();
		if (!this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			return;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (list[i].provider == null)
			{
				list[i].Cancel("no provider");
				list[i] = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
			}
		}
		int num = CPUBudget.coreCount * 4;
		if (list.Count < num)
		{
			using (List<Chore>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Chore chore = enumerator.Current;
					chore.CollectChores(consumer_state, succeeded, failed_contexts, false);
				}
				return;
			}
		}
		ChoreProvider.batch_chore_collector.Reset(list);
		int coreCount = CPUBudget.coreCount;
		int num2 = Math.Min(24, list.Count / coreCount);
		for (int j = 0; j < list.Count; j += num2)
		{
			ChoreProvider.batch_chore_collector.Add(new ChoreProvider.ChoreProviderCollectTask(j, Math.Min(j + num2, list.Count), consumer_state));
		}
		GlobalJobManager.Run(ChoreProvider.batch_chore_collector);
		for (int k = 0; k < ChoreProvider.batch_chore_collector.Count; k++)
		{
			ChoreProvider.batch_chore_collector.GetWorkItem(k).Finish(succeeded, failed_contexts);
		}
	}

	// Token: 0x04000D7E RID: 3454
	public Dictionary<int, List<Chore>> choreWorldMap = new Dictionary<int, List<Chore>>();

	// Token: 0x04000D7F RID: 3455
	private static WorkItemCollection<ChoreProvider.ChoreProviderCollectTask, List<Chore>> batch_chore_collector = new WorkItemCollection<ChoreProvider.ChoreProviderCollectTask, List<Chore>>();

	// Token: 0x02001220 RID: 4640
	private struct ChoreProviderCollectTask : IWorkItem<List<Chore>>
	{
		// Token: 0x06008241 RID: 33345 RVA: 0x0031C3EB File Offset: 0x0031A5EB
		public ChoreProviderCollectTask(int start, int end, ChoreConsumerState consumer_state)
		{
			this.start = start;
			this.end = end;
			this.consumer_state = consumer_state;
			this.succeeded = ListPool<Chore.Precondition.Context, ChoreProvider.ChoreProviderCollectTask>.Allocate();
			this.failed = ListPool<Chore.Precondition.Context, ChoreProvider.ChoreProviderCollectTask>.Allocate();
			this.incomplete = ListPool<Chore.Precondition.Context, ChoreProvider.ChoreProviderCollectTask>.Allocate();
		}

		// Token: 0x06008242 RID: 33346 RVA: 0x0031C424 File Offset: 0x0031A624
		public void Run(List<Chore> chores)
		{
			for (int i = this.start; i < this.end; i++)
			{
				chores[i].CollectChores(this.consumer_state, this.succeeded, this.incomplete, this.failed, false);
			}
		}

		// Token: 0x06008243 RID: 33347 RVA: 0x0031C46C File Offset: 0x0031A66C
		public void Finish(List<Chore.Precondition.Context> combined_succeeded, List<Chore.Precondition.Context> combined_failed)
		{
			combined_succeeded.AddRange(this.succeeded);
			this.succeeded.Recycle();
			combined_failed.AddRange(this.failed);
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
			this.incomplete.Recycle();
		}

		// Token: 0x04006276 RID: 25206
		private int start;

		// Token: 0x04006277 RID: 25207
		private int end;

		// Token: 0x04006278 RID: 25208
		private ChoreConsumerState consumer_state;

		// Token: 0x04006279 RID: 25209
		public ListPool<Chore.Precondition.Context, ChoreProvider.ChoreProviderCollectTask>.PooledList succeeded;

		// Token: 0x0400627A RID: 25210
		public ListPool<Chore.Precondition.Context, ChoreProvider.ChoreProviderCollectTask>.PooledList failed;

		// Token: 0x0400627B RID: 25211
		public ListPool<Chore.Precondition.Context, ChoreProvider.ChoreProviderCollectTask>.PooledList incomplete;
	}
}
