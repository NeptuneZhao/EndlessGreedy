using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000EC8 RID: 3784
	public class SurviveARocketWithMinimumMorale : ColonyAchievementRequirement
	{
		// Token: 0x0600761D RID: 30237 RVA: 0x002E293E File Offset: 0x002E0B3E
		public SurviveARocketWithMinimumMorale(float minimumMorale, int numberOfCycles)
		{
			this.minimumMorale = minimumMorale;
			this.numberOfCycles = numberOfCycles;
		}

		// Token: 0x0600761E RID: 30238 RVA: 0x002E2954 File Offset: 0x002E0B54
		public override string GetProgress(bool complete)
		{
			if (complete)
			{
				return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SURVIVE_SPACE_COMPLETE, this.minimumMorale, this.numberOfCycles);
			}
			return base.GetProgress(complete);
		}

		// Token: 0x0600761F RID: 30239 RVA: 0x002E2988 File Offset: 0x002E0B88
		public override bool Success()
		{
			foreach (KeyValuePair<int, int> keyValuePair in SaveGame.Instance.ColonyAchievementTracker.cyclesRocketDupeMoraleAboveRequirement)
			{
				if (keyValuePair.Value >= this.numberOfCycles)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005585 RID: 21893
		public float minimumMorale;

		// Token: 0x04005586 RID: 21894
		public int numberOfCycles;
	}
}
