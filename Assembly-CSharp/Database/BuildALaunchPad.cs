using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EC2 RID: 3778
	public class BuildALaunchPad : ColonyAchievementRequirement
	{
		// Token: 0x0600760B RID: 30219 RVA: 0x002E2681 File Offset: 0x002E0881
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILD_A_LAUNCHPAD;
		}

		// Token: 0x0600760C RID: 30220 RVA: 0x002E2690 File Offset: 0x002E0890
		public override bool Success()
		{
			foreach (LaunchPad component in Components.LaunchPads.Items)
			{
				WorldContainer myWorld = component.GetMyWorld();
				if (!myWorld.IsStartWorld && Components.WarpReceivers.GetWorldItems(myWorld.id, false).Count == 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
