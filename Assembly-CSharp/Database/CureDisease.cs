using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EB7 RID: 3767
	public class CureDisease : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075E2 RID: 30178 RVA: 0x002E1E50 File Offset: 0x002E0050
		public override bool Success()
		{
			return Game.Instance.savedInfo.curedDisease;
		}

		// Token: 0x060075E3 RID: 30179 RVA: 0x002E1E61 File Offset: 0x002E0061
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x060075E4 RID: 30180 RVA: 0x002E1E63 File Offset: 0x002E0063
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CURED_DISEASE;
		}
	}
}
