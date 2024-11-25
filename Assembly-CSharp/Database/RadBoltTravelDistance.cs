using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EC6 RID: 3782
	public class RadBoltTravelDistance : ColonyAchievementRequirement
	{
		// Token: 0x06007617 RID: 30231 RVA: 0x002E28C0 File Offset: 0x002E0AC0
		public RadBoltTravelDistance(int travelDistance)
		{
			this.travelDistance = travelDistance;
		}

		// Token: 0x06007618 RID: 30232 RVA: 0x002E28CF File Offset: 0x002E0ACF
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.RADBOLT_TRAVEL, SaveGame.Instance.ColonyAchievementTracker.radBoltTravelDistance, this.travelDistance);
		}

		// Token: 0x06007619 RID: 30233 RVA: 0x002E28FF File Offset: 0x002E0AFF
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.radBoltTravelDistance > (float)this.travelDistance;
		}

		// Token: 0x04005584 RID: 21892
		private int travelDistance;
	}
}
