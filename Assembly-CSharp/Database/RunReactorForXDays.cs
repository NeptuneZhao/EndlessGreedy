using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000EC9 RID: 3785
	public class RunReactorForXDays : ColonyAchievementRequirement
	{
		// Token: 0x06007620 RID: 30240 RVA: 0x002E29F4 File Offset: 0x002E0BF4
		public RunReactorForXDays(int numCycles)
		{
			this.numCycles = numCycles;
		}

		// Token: 0x06007621 RID: 30241 RVA: 0x002E2A04 File Offset: 0x002E0C04
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (Reactor reactor in Components.NuclearReactors.Items)
			{
				if (reactor.numCyclesRunning > num)
				{
					num = reactor.numCyclesRunning;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.RUN_A_REACTOR, complete ? this.numCycles : num, this.numCycles);
		}

		// Token: 0x06007622 RID: 30242 RVA: 0x002E2A94 File Offset: 0x002E0C94
		public override bool Success()
		{
			using (List<Reactor>.Enumerator enumerator = Components.NuclearReactors.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.numCyclesRunning >= this.numCycles)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04005587 RID: 21895
		private int numCycles;
	}
}
