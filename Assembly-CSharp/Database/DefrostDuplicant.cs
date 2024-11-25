using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EC1 RID: 3777
	public class DefrostDuplicant : ColonyAchievementRequirement
	{
		// Token: 0x06007608 RID: 30216 RVA: 0x002E265C File Offset: 0x002E085C
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.DEFROST_DUPLICANT;
		}

		// Token: 0x06007609 RID: 30217 RVA: 0x002E2668 File Offset: 0x002E0868
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.defrostedDuplicant;
		}
	}
}
