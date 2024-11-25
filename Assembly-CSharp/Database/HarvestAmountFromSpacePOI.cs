using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EC4 RID: 3780
	public class HarvestAmountFromSpacePOI : ColonyAchievementRequirement
	{
		// Token: 0x06007611 RID: 30225 RVA: 0x002E2765 File Offset: 0x002E0965
		public HarvestAmountFromSpacePOI(float amountToHarvest)
		{
			this.amountToHarvest = amountToHarvest;
		}

		// Token: 0x06007612 RID: 30226 RVA: 0x002E2774 File Offset: 0x002E0974
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.MINE_SPACE_POI, SaveGame.Instance.ColonyAchievementTracker.totalMaterialsHarvestFromPOI, this.amountToHarvest);
		}

		// Token: 0x06007613 RID: 30227 RVA: 0x002E27A4 File Offset: 0x002E09A4
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.totalMaterialsHarvestFromPOI > this.amountToHarvest;
		}

		// Token: 0x04005583 RID: 21891
		private float amountToHarvest;
	}
}
