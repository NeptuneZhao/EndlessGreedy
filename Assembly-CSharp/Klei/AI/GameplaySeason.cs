using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Klei.AI
{
	// Token: 0x02000F62 RID: 3938
	[DebuggerDisplay("{base.Id}")]
	public class GameplaySeason : Resource
	{
		// Token: 0x060078FB RID: 30971 RVA: 0x002FE11C File Offset: 0x002FC31C
		public GameplaySeason(string id, GameplaySeason.Type type, string dlcId, float period, bool synchronizedToPeriod, float randomizedEventStartTime = -1f, bool startActive = false, int finishAfterNumEvents = -1, float minCycle = 0f, float maxCycle = float.PositiveInfinity, int numEventsToStartEachPeriod = 1) : base(id, null, null)
		{
			this.type = type;
			this.dlcId = dlcId;
			this.period = period;
			this.synchronizedToPeriod = synchronizedToPeriod;
			global::Debug.Assert(period > 0f, "Season " + id + "'s Period cannot be 0 or negative");
			if (randomizedEventStartTime == -1f)
			{
				this.randomizedEventStartTime = new MathUtil.MinMax(--0f * period, 0f * period);
			}
			else
			{
				this.randomizedEventStartTime = new MathUtil.MinMax(-randomizedEventStartTime, randomizedEventStartTime);
				DebugUtil.DevAssert((this.randomizedEventStartTime.max - this.randomizedEventStartTime.min) * 0.4f < period, string.Format("Season {0} randomizedEventStartTime is greater than {1}% of its period.", id, 0.4f), null);
			}
			this.startActive = startActive;
			this.finishAfterNumEvents = finishAfterNumEvents;
			this.minCycle = minCycle;
			this.maxCycle = maxCycle;
			this.events = new List<GameplayEvent>();
			this.numEventsToStartEachPeriod = numEventsToStartEachPeriod;
		}

		// Token: 0x060078FC RID: 30972 RVA: 0x002FE21A File Offset: 0x002FC41A
		public virtual void AdditionalEventInstanceSetup(StateMachine.Instance generic_smi)
		{
		}

		// Token: 0x060078FD RID: 30973 RVA: 0x002FE21C File Offset: 0x002FC41C
		public virtual float GetSeasonPeriod()
		{
			return this.period;
		}

		// Token: 0x060078FE RID: 30974 RVA: 0x002FE224 File Offset: 0x002FC424
		public GameplaySeason AddEvent(GameplayEvent evt)
		{
			this.events.Add(evt);
			return this;
		}

		// Token: 0x060078FF RID: 30975 RVA: 0x002FE233 File Offset: 0x002FC433
		public virtual GameplaySeasonInstance Instantiate(int worldId)
		{
			return new GameplaySeasonInstance(this, worldId);
		}

		// Token: 0x04005A5B RID: 23131
		public const float DEFAULT_PERCENTAGE_RANDOMIZED_EVENT_START = 0f;

		// Token: 0x04005A5C RID: 23132
		public const float PERCENTAGE_WARNING = 0.4f;

		// Token: 0x04005A5D RID: 23133
		public const float USE_DEFAULT = -1f;

		// Token: 0x04005A5E RID: 23134
		public const int INFINITE = -1;

		// Token: 0x04005A5F RID: 23135
		public float period;

		// Token: 0x04005A60 RID: 23136
		public bool synchronizedToPeriod;

		// Token: 0x04005A61 RID: 23137
		public MathUtil.MinMax randomizedEventStartTime;

		// Token: 0x04005A62 RID: 23138
		public int finishAfterNumEvents = -1;

		// Token: 0x04005A63 RID: 23139
		public bool startActive;

		// Token: 0x04005A64 RID: 23140
		public int numEventsToStartEachPeriod;

		// Token: 0x04005A65 RID: 23141
		public float minCycle;

		// Token: 0x04005A66 RID: 23142
		public float maxCycle;

		// Token: 0x04005A67 RID: 23143
		public List<GameplayEvent> events;

		// Token: 0x04005A68 RID: 23144
		public GameplaySeason.Type type;

		// Token: 0x04005A69 RID: 23145
		public string dlcId;

		// Token: 0x02002351 RID: 9041
		public enum Type
		{
			// Token: 0x04009E5C RID: 40540
			World,
			// Token: 0x04009E5D RID: 40541
			Cluster
		}
	}
}
