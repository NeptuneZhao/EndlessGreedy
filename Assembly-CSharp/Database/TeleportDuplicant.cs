using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000EC0 RID: 3776
	public class TeleportDuplicant : ColonyAchievementRequirement
	{
		// Token: 0x06007605 RID: 30213 RVA: 0x002E25E8 File Offset: 0x002E07E8
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TELEPORT_DUPLICANT;
		}

		// Token: 0x06007606 RID: 30214 RVA: 0x002E25F4 File Offset: 0x002E07F4
		public override bool Success()
		{
			using (List<WarpReceiver>.Enumerator enumerator = Components.WarpReceivers.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Used)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
