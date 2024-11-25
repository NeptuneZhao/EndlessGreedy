using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E9E RID: 3742
	public class OpenTemporalTear : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007576 RID: 30070 RVA: 0x002E0171 File Offset: 0x002DE371
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.OPEN_TEMPORAL_TEAR;
		}

		// Token: 0x06007577 RID: 30071 RVA: 0x002E017D File Offset: 0x002DE37D
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007578 RID: 30072 RVA: 0x002E018B File Offset: 0x002DE38B
		public override bool Success()
		{
			return ClusterManager.Instance.GetComponent<ClusterPOIManager>().IsTemporalTearOpen();
		}

		// Token: 0x06007579 RID: 30073 RVA: 0x002E019C File Offset: 0x002DE39C
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.OPEN_TEMPORAL_TEAR;
		}
	}
}
