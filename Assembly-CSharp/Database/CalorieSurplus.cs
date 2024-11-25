using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EA2 RID: 3746
	public class CalorieSurplus : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600758A RID: 30090 RVA: 0x002E04B0 File Offset: 0x002DE6B0
		public CalorieSurplus(float surplusAmount)
		{
			this.surplusAmount = (double)surplusAmount;
		}

		// Token: 0x0600758B RID: 30091 RVA: 0x002E04C0 File Offset: 0x002DE6C0
		public override bool Success()
		{
			return (double)(ClusterManager.Instance.CountAllRations() / 1000f) >= this.surplusAmount;
		}

		// Token: 0x0600758C RID: 30092 RVA: 0x002E04DE File Offset: 0x002DE6DE
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x0600758D RID: 30093 RVA: 0x002E04E9 File Offset: 0x002DE6E9
		public void Deserialize(IReader reader)
		{
			this.surplusAmount = reader.ReadDouble();
		}

		// Token: 0x0600758E RID: 30094 RVA: 0x002E04F7 File Offset: 0x002DE6F7
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIE_SURPLUS, GameUtil.GetFormattedCalories(complete ? ((float)this.surplusAmount) : ClusterManager.Instance.CountAllRations(), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.surplusAmount, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x0400555C RID: 21852
		private double surplusAmount;
	}
}
