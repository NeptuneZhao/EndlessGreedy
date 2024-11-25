using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E95 RID: 3733
	public class FractionalCycleNumber : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007546 RID: 30022 RVA: 0x002DFBCD File Offset: 0x002DDDCD
		public FractionalCycleNumber(float fractionalCycleNumber)
		{
			this.fractionalCycleNumber = fractionalCycleNumber;
		}

		// Token: 0x06007547 RID: 30023 RVA: 0x002DFBDC File Offset: 0x002DDDDC
		public override bool Success()
		{
			int num = (int)this.fractionalCycleNumber;
			float num2 = this.fractionalCycleNumber - (float)num;
			return (float)(GameClock.Instance.GetCycle() + 1) > this.fractionalCycleNumber || (GameClock.Instance.GetCycle() + 1 == num && GameClock.Instance.GetCurrentCycleAsPercentage() >= num2);
		}

		// Token: 0x06007548 RID: 30024 RVA: 0x002DFC33 File Offset: 0x002DDE33
		public void Deserialize(IReader reader)
		{
			this.fractionalCycleNumber = reader.ReadSingle();
		}

		// Token: 0x06007549 RID: 30025 RVA: 0x002DFC44 File Offset: 0x002DDE44
		public override string GetProgress(bool complete)
		{
			float num = (float)GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage();
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.FRACTIONAL_CYCLE, complete ? this.fractionalCycleNumber : num, this.fractionalCycleNumber);
		}

		// Token: 0x04005553 RID: 21843
		private float fractionalCycleNumber;
	}
}
