using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EAF RID: 3759
	public class ExploreOilFieldSubZone : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075C2 RID: 30146 RVA: 0x002E1554 File Offset: 0x002DF754
		public override bool Success()
		{
			return Game.Instance.savedInfo.discoveredOilField;
		}

		// Token: 0x060075C3 RID: 30147 RVA: 0x002E1565 File Offset: 0x002DF765
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x060075C4 RID: 30148 RVA: 0x002E1567 File Offset: 0x002DF767
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ENTER_OIL_BIOME;
		}
	}
}
