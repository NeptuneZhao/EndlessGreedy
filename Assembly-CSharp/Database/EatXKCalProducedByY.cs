using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;

namespace Database
{
	// Token: 0x02000EAC RID: 3756
	public class EatXKCalProducedByY : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075B5 RID: 30133 RVA: 0x002E10BB File Offset: 0x002DF2BB
		public EatXKCalProducedByY(int numCalories, List<Tag> foodProducers)
		{
			this.numCalories = numCalories;
			this.foodProducers = foodProducers;
		}

		// Token: 0x060075B6 RID: 30134 RVA: 0x002E10D4 File Offset: 0x002DF2D4
		public override bool Success()
		{
			List<string> list = new List<string>();
			foreach (ComplexRecipe complexRecipe in ComplexRecipeManager.Get().recipes)
			{
				foreach (Tag b in this.foodProducers)
				{
					using (List<Tag>.Enumerator enumerator3 = complexRecipe.fabricators.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current == b)
							{
								list.Add(complexRecipe.FirstResult.ToString());
							}
						}
					}
				}
			}
			return WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumedForIDs(list.Distinct<string>().ToList<string>()) / 1000f > (float)this.numCalories;
		}

		// Token: 0x060075B7 RID: 30135 RVA: 0x002E11F0 File Offset: 0x002DF3F0
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.foodProducers = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.foodProducers.Add(new Tag(name));
			}
			this.numCalories = reader.ReadInt32();
		}

		// Token: 0x060075B8 RID: 30136 RVA: 0x002E1240 File Offset: 0x002DF440
		public override string GetProgress(bool complete)
		{
			string text = "";
			for (int i = 0; i < this.foodProducers.Count; i++)
			{
				if (i != 0)
				{
					text += COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.PREPARED_SEPARATOR;
				}
				BuildingDef buildingDef = Assets.GetBuildingDef(this.foodProducers[i].Name);
				if (buildingDef != null)
				{
					text += buildingDef.Name;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_ITEM, text);
		}

		// Token: 0x0400556C RID: 21868
		private int numCalories;

		// Token: 0x0400556D RID: 21869
		private List<Tag> foodProducers;
	}
}
