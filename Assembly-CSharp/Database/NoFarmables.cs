using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000EA9 RID: 3753
	public class NoFarmables : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075A8 RID: 30120 RVA: 0x002E0E30 File Offset: 0x002DF030
		public override bool Success()
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				foreach (PlantablePlot plantablePlot in Components.PlantablePlots.GetItems(worldContainer.id))
				{
					if (plantablePlot.Occupant != null)
					{
						using (IEnumerator<Tag> enumerator3 = plantablePlot.possibleDepositObjectTags.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								if (enumerator3.Current != GameTags.DecorSeed)
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060075A9 RID: 30121 RVA: 0x002E0F24 File Offset: 0x002DF124
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x060075AA RID: 30122 RVA: 0x002E0F2F File Offset: 0x002DF12F
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x060075AB RID: 30123 RVA: 0x002E0F31 File Offset: 0x002DF131
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.NO_FARM_TILES;
		}
	}
}
