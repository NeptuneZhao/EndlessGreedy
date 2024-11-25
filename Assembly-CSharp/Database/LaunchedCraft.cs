using System;
using System.Collections;
using STRINGS;

namespace Database
{
	// Token: 0x02000EBF RID: 3775
	public class LaunchedCraft : ColonyAchievementRequirement
	{
		// Token: 0x06007602 RID: 30210 RVA: 0x002E2571 File Offset: 0x002E0771
		public override string GetProgress(bool completed)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET;
		}

		// Token: 0x06007603 RID: 30211 RVA: 0x002E2580 File Offset: 0x002E0780
		public override bool Success()
		{
			using (IEnumerator enumerator = Components.Clustercrafts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((Clustercraft)enumerator.Current).Status == Clustercraft.CraftStatus.InFlight)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
