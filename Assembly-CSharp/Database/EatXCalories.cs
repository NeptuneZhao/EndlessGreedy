using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EAB RID: 3755
	public class EatXCalories : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075B1 RID: 30129 RVA: 0x002E102D File Offset: 0x002DF22D
		public EatXCalories(int numCalories)
		{
			this.numCalories = numCalories;
		}

		// Token: 0x060075B2 RID: 30130 RVA: 0x002E103C File Offset: 0x002DF23C
		public override bool Success()
		{
			return WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumed() / 1000f > (float)this.numCalories;
		}

		// Token: 0x060075B3 RID: 30131 RVA: 0x002E1057 File Offset: 0x002DF257
		public void Deserialize(IReader reader)
		{
			this.numCalories = reader.ReadInt32();
		}

		// Token: 0x060075B4 RID: 30132 RVA: 0x002E1068 File Offset: 0x002DF268
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_CALORIES, GameUtil.GetFormattedCalories(complete ? ((float)this.numCalories * 1000f) : WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumed(), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.numCalories * 1000f, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x0400556B RID: 21867
		private int numCalories;
	}
}
