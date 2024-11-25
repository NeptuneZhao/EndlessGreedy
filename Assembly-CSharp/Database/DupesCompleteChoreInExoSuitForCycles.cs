using System;
using System.Collections.Generic;
using System.Linq;

namespace Database
{
	// Token: 0x02000EBD RID: 3773
	public class DupesCompleteChoreInExoSuitForCycles : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075F9 RID: 30201 RVA: 0x002E2327 File Offset: 0x002E0527
		public DupesCompleteChoreInExoSuitForCycles(int numCycles)
		{
			this.numCycles = numCycles;
		}

		// Token: 0x060075FA RID: 30202 RVA: 0x002E2338 File Offset: 0x002E0538
		public override bool Success()
		{
			Dictionary<int, List<int>> dupesCompleteChoresInSuits = SaveGame.Instance.ColonyAchievementTracker.dupesCompleteChoresInSuits;
			Dictionary<int, float> dictionary = new Dictionary<int, float>();
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				KPrefabID component = minionIdentity.GetComponent<KPrefabID>();
				if (!component.HasTag(GameTags.Dead))
				{
					dictionary.Add(component.InstanceID, minionIdentity.arrivalTime);
				}
			}
			int num = 0;
			int num2 = Math.Min(dupesCompleteChoresInSuits.Count, this.numCycles);
			for (int i = GameClock.Instance.GetCycle() - num2; i <= GameClock.Instance.GetCycle(); i++)
			{
				if (dupesCompleteChoresInSuits.ContainsKey(i))
				{
					List<int> list = dictionary.Keys.Except(dupesCompleteChoresInSuits[i]).ToList<int>();
					bool flag = true;
					foreach (int key in list)
					{
						if (dictionary[key] < (float)i)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						num++;
					}
					else if (i != GameClock.Instance.GetCycle())
					{
						num = 0;
					}
					this.currentCycleStreak = num;
					if (num >= this.numCycles)
					{
						this.currentCycleStreak = this.numCycles;
						return true;
					}
				}
				else
				{
					this.currentCycleStreak = Math.Max(this.currentCycleStreak, num);
					num = 0;
				}
			}
			return false;
		}

		// Token: 0x060075FB RID: 30203 RVA: 0x002E24C8 File Offset: 0x002E06C8
		public void Deserialize(IReader reader)
		{
			this.numCycles = reader.ReadInt32();
		}

		// Token: 0x060075FC RID: 30204 RVA: 0x002E24D8 File Offset: 0x002E06D8
		public int GetNumberOfDupesForCycle(int cycle)
		{
			int result = 0;
			Dictionary<int, List<int>> dupesCompleteChoresInSuits = SaveGame.Instance.ColonyAchievementTracker.dupesCompleteChoresInSuits;
			if (dupesCompleteChoresInSuits.ContainsKey(GameClock.Instance.GetCycle()))
			{
				result = dupesCompleteChoresInSuits[GameClock.Instance.GetCycle()].Count;
			}
			return result;
		}

		// Token: 0x04005580 RID: 21888
		public int currentCycleStreak;

		// Token: 0x04005581 RID: 21889
		public int numCycles;
	}
}
