using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EBE RID: 3774
	public class SentCraftIntoTemporalTear : VictoryColonyAchievementRequirement
	{
		// Token: 0x060075FD RID: 30205 RVA: 0x002E2520 File Offset: 0x002E0720
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION, UI.SPACEDESTINATIONS.WORMHOLE.NAME);
		}

		// Token: 0x060075FE RID: 30206 RVA: 0x002E2536 File Offset: 0x002E0736
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION_DESCRIPTION, UI.SPACEDESTINATIONS.WORMHOLE.NAME);
		}

		// Token: 0x060075FF RID: 30207 RVA: 0x002E254C File Offset: 0x002E074C
		public override string GetProgress(bool completed)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET_TO_WORMHOLE;
		}

		// Token: 0x06007600 RID: 30208 RVA: 0x002E2558 File Offset: 0x002E0758
		public override bool Success()
		{
			return ClusterManager.Instance.GetClusterPOIManager().HasTemporalTearConsumedCraft();
		}
	}
}
