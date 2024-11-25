using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EC7 RID: 3783
	public class HarvestAHiveWithoutBeingStung : ColonyAchievementRequirement
	{
		// Token: 0x0600761A RID: 30234 RVA: 0x002E2919 File Offset: 0x002E0B19
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HARVEST_HIVE;
		}

		// Token: 0x0600761B RID: 30235 RVA: 0x002E2925 File Offset: 0x002E0B25
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.harvestAHiveWithoutGettingStung;
		}
	}
}
