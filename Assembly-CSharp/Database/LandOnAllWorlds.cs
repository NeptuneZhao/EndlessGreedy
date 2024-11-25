using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EC5 RID: 3781
	public class LandOnAllWorlds : ColonyAchievementRequirement
	{
		// Token: 0x06007614 RID: 30228 RVA: 0x002E27C0 File Offset: 0x002E09C0
		public override string GetProgress(bool complete)
		{
			int num = 0;
			int num2 = 0;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior)
				{
					num++;
					if (worldContainer.IsDupeVisited || worldContainer.IsRoverVisted)
					{
						num2++;
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAND_DUPES_ON_ALL_WORLDS, num2, num);
		}

		// Token: 0x06007615 RID: 30229 RVA: 0x002E284C File Offset: 0x002E0A4C
		public override bool Success()
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior && !worldContainer.IsDupeVisited && !worldContainer.IsRoverVisted)
				{
					return false;
				}
			}
			return true;
		}
	}
}
