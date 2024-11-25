using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EC3 RID: 3779
	public class AnalyzeSeed : ColonyAchievementRequirement
	{
		// Token: 0x0600760E RID: 30222 RVA: 0x002E2714 File Offset: 0x002E0914
		public AnalyzeSeed(string seedname)
		{
			this.seedName = seedname;
		}

		// Token: 0x0600760F RID: 30223 RVA: 0x002E2723 File Offset: 0x002E0923
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ANALYZE_SEED, this.seedName.ProperName());
		}

		// Token: 0x06007610 RID: 30224 RVA: 0x002E2744 File Offset: 0x002E0944
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.analyzedSeeds.Contains(this.seedName);
		}

		// Token: 0x04005582 RID: 21890
		private string seedName;
	}
}
