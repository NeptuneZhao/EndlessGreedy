using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000EAA RID: 3754
	public class EatXCaloriesFromY : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075AD RID: 30125 RVA: 0x002E0F45 File Offset: 0x002DF145
		public EatXCaloriesFromY(int numCalories, List<string> fromFoodType)
		{
			this.numCalories = numCalories;
			this.fromFoodType = fromFoodType;
		}

		// Token: 0x060075AE RID: 30126 RVA: 0x002E0F66 File Offset: 0x002DF166
		public override bool Success()
		{
			return WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumedForIDs(this.fromFoodType) / 1000f > (float)this.numCalories;
		}

		// Token: 0x060075AF RID: 30127 RVA: 0x002E0F88 File Offset: 0x002DF188
		public void Deserialize(IReader reader)
		{
			this.numCalories = reader.ReadInt32();
			int num = reader.ReadInt32();
			this.fromFoodType = new List<string>(num);
			for (int i = 0; i < num; i++)
			{
				string item = reader.ReadKleiString();
				this.fromFoodType.Add(item);
			}
		}

		// Token: 0x060075B0 RID: 30128 RVA: 0x002E0FD4 File Offset: 0x002DF1D4
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIES_FROM_MEAT, GameUtil.GetFormattedCalories(complete ? ((float)this.numCalories * 1000f) : WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumedForIDs(this.fromFoodType), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.numCalories * 1000f, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x04005569 RID: 21865
		private int numCalories;

		// Token: 0x0400556A RID: 21866
		private List<string> fromFoodType = new List<string>();
	}
}
