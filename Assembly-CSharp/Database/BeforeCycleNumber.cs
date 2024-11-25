using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E94 RID: 3732
	public class BeforeCycleNumber : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007541 RID: 30017 RVA: 0x002DFB54 File Offset: 0x002DDD54
		public BeforeCycleNumber(int cycleNumber = 100)
		{
			this.cycleNumber = cycleNumber;
		}

		// Token: 0x06007542 RID: 30018 RVA: 0x002DFB63 File Offset: 0x002DDD63
		public override bool Success()
		{
			return GameClock.Instance.GetCycle() + 1 <= this.cycleNumber;
		}

		// Token: 0x06007543 RID: 30019 RVA: 0x002DFB7C File Offset: 0x002DDD7C
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x06007544 RID: 30020 RVA: 0x002DFB87 File Offset: 0x002DDD87
		public void Deserialize(IReader reader)
		{
			this.cycleNumber = reader.ReadInt32();
		}

		// Token: 0x06007545 RID: 30021 RVA: 0x002DFB95 File Offset: 0x002DDD95
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.REMAINING_CYCLES, Mathf.Max(this.cycleNumber - GameClock.Instance.GetCycle(), 0), this.cycleNumber);
		}

		// Token: 0x04005552 RID: 21842
		private int cycleNumber;
	}
}
