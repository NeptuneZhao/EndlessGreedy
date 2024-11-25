using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000EBC RID: 3772
	public class DupesVsSolidTransferArmFetch : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075F6 RID: 30198 RVA: 0x002E223E File Offset: 0x002E043E
		public DupesVsSolidTransferArmFetch(float percentage, int numCycles)
		{
			this.percentage = percentage;
			this.numCycles = numCycles;
		}

		// Token: 0x060075F7 RID: 30199 RVA: 0x002E2254 File Offset: 0x002E0454
		public override bool Success()
		{
			Dictionary<int, int> fetchDupeChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchDupeChoreDeliveries;
			Dictionary<int, int> fetchAutomatedChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchAutomatedChoreDeliveries;
			int num = 0;
			this.currentCycleCount = 0;
			for (int i = GameClock.Instance.GetCycle() - 1; i >= GameClock.Instance.GetCycle() - this.numCycles; i--)
			{
				if (fetchAutomatedChoreDeliveries.ContainsKey(i))
				{
					if (fetchDupeChoreDeliveries.ContainsKey(i) && (float)fetchDupeChoreDeliveries[i] >= (float)fetchAutomatedChoreDeliveries[i] * this.percentage)
					{
						break;
					}
					num++;
				}
				else if (fetchDupeChoreDeliveries.ContainsKey(i))
				{
					num = 0;
					break;
				}
			}
			this.currentCycleCount = Math.Max(this.currentCycleCount, num);
			return num >= this.numCycles;
		}

		// Token: 0x060075F8 RID: 30200 RVA: 0x002E230D File Offset: 0x002E050D
		public void Deserialize(IReader reader)
		{
			this.numCycles = reader.ReadInt32();
			this.percentage = reader.ReadSingle();
		}

		// Token: 0x0400557C RID: 21884
		public float percentage;

		// Token: 0x0400557D RID: 21885
		public int numCycles;

		// Token: 0x0400557E RID: 21886
		public int currentCycleCount;

		// Token: 0x0400557F RID: 21887
		public bool armsOutPerformingDupesThisCycle;
	}
}
