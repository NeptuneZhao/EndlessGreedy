using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E93 RID: 3731
	public class CycleNumber : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600753B RID: 30011 RVA: 0x002DFAAE File Offset: 0x002DDCAE
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE, this.cycleNumber);
		}

		// Token: 0x0600753C RID: 30012 RVA: 0x002DFACA File Offset: 0x002DDCCA
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE_DESCRIPTION, this.cycleNumber);
		}

		// Token: 0x0600753D RID: 30013 RVA: 0x002DFAE6 File Offset: 0x002DDCE6
		public CycleNumber(int cycleNumber = 100)
		{
			this.cycleNumber = cycleNumber;
		}

		// Token: 0x0600753E RID: 30014 RVA: 0x002DFAF5 File Offset: 0x002DDCF5
		public override bool Success()
		{
			return GameClock.Instance.GetCycle() + 1 >= this.cycleNumber;
		}

		// Token: 0x0600753F RID: 30015 RVA: 0x002DFB0E File Offset: 0x002DDD0E
		public void Deserialize(IReader reader)
		{
			this.cycleNumber = reader.ReadInt32();
		}

		// Token: 0x06007540 RID: 30016 RVA: 0x002DFB1C File Offset: 0x002DDD1C
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CYCLE_NUMBER, complete ? this.cycleNumber : (GameClock.Instance.GetCycle() + 1), this.cycleNumber);
		}

		// Token: 0x04005551 RID: 21841
		private int cycleNumber;
	}
}
