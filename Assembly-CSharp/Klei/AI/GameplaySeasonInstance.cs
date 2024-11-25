using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F63 RID: 3939
	[SerializationConfig(MemberSerialization.OptIn)]
	public class GameplaySeasonInstance : ISaveLoadable
	{
		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06007900 RID: 30976 RVA: 0x002FE23C File Offset: 0x002FC43C
		public float NextEventTime
		{
			get
			{
				return this.nextPeriodTime + this.randomizedNextTime;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06007901 RID: 30977 RVA: 0x002FE24B File Offset: 0x002FC44B
		public GameplaySeason Season
		{
			get
			{
				if (this._season == null)
				{
					this._season = Db.Get().GameplaySeasons.TryGet(this.seasonId);
				}
				return this._season;
			}
		}

		// Token: 0x06007902 RID: 30978 RVA: 0x002FE278 File Offset: 0x002FC478
		public GameplaySeasonInstance(GameplaySeason season, int worldId)
		{
			this.seasonId = season.Id;
			this.worldId = worldId;
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			if (season.synchronizedToPeriod)
			{
				float seasonPeriod = this.Season.GetSeasonPeriod();
				this.nextPeriodTime = (Mathf.Floor(currentTimeInCycles / seasonPeriod) + 1f) * seasonPeriod;
			}
			else
			{
				this.nextPeriodTime = currentTimeInCycles;
			}
			this.CalculateNextEventTime();
		}

		// Token: 0x06007903 RID: 30979 RVA: 0x002FE2E0 File Offset: 0x002FC4E0
		private void CalculateNextEventTime()
		{
			float seasonPeriod = this.Season.GetSeasonPeriod();
			this.randomizedNextTime = UnityEngine.Random.Range(this.Season.randomizedEventStartTime.min, this.Season.randomizedEventStartTime.max);
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			float num = this.nextPeriodTime + this.randomizedNextTime;
			while (num < currentTimeInCycles || num < this.Season.minCycle)
			{
				this.nextPeriodTime += seasonPeriod;
				num = this.nextPeriodTime + this.randomizedNextTime;
			}
		}

		// Token: 0x06007904 RID: 30980 RVA: 0x002FE368 File Offset: 0x002FC568
		public bool StartEvent(bool ignorePreconditions = false)
		{
			bool result = false;
			this.CalculateNextEventTime();
			this.numStartEvents++;
			List<GameplayEvent> list;
			if (!ignorePreconditions)
			{
				list = (from x in this.Season.events
				where x.IsAllowed()
				select x).ToList<GameplayEvent>();
			}
			else
			{
				list = this.Season.events;
			}
			List<GameplayEvent> list2 = list;
			if (list2.Count > 0)
			{
				list2.ForEach(delegate(GameplayEvent x)
				{
					x.CalculatePriority();
				});
				list2.Sort();
				int maxExclusive = Mathf.Min(list2.Count, 5);
				GameplayEvent eventType = list2[UnityEngine.Random.Range(0, maxExclusive)];
				GameplayEventManager.Instance.StartNewEvent(eventType, this.worldId, new Action<StateMachine.Instance>(this.Season.AdditionalEventInstanceSetup));
				result = true;
			}
			this.allEventWillNotRunAgain = true;
			using (List<GameplayEvent>.Enumerator enumerator = this.Season.events.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.WillNeverRunAgain())
					{
						this.allEventWillNotRunAgain = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06007905 RID: 30981 RVA: 0x002FE4A4 File Offset: 0x002FC6A4
		public bool ShouldGenerateEvents()
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(this.worldId);
			if (!world.IsDupeVisited && !world.IsRoverVisted)
			{
				return false;
			}
			if ((this.Season.finishAfterNumEvents != -1 && this.numStartEvents >= this.Season.finishAfterNumEvents) || this.allEventWillNotRunAgain)
			{
				return false;
			}
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			return currentTimeInCycles > this.Season.minCycle && currentTimeInCycles < this.Season.maxCycle;
		}

		// Token: 0x04005A6A RID: 23146
		public const int LIMIT_SELECTION = 5;

		// Token: 0x04005A6B RID: 23147
		[Serialize]
		public int numStartEvents;

		// Token: 0x04005A6C RID: 23148
		[Serialize]
		public int worldId;

		// Token: 0x04005A6D RID: 23149
		[Serialize]
		private readonly string seasonId;

		// Token: 0x04005A6E RID: 23150
		[Serialize]
		private float nextPeriodTime;

		// Token: 0x04005A6F RID: 23151
		[Serialize]
		private float randomizedNextTime;

		// Token: 0x04005A70 RID: 23152
		private bool allEventWillNotRunAgain;

		// Token: 0x04005A71 RID: 23153
		private GameplaySeason _season;
	}
}
