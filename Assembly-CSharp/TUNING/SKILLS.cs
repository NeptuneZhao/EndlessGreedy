using System;

namespace TUNING
{
	// Token: 0x02000EF0 RID: 3824
	public class SKILLS
	{
		// Token: 0x04005727 RID: 22311
		public static int TARGET_SKILLS_EARNED = 15;

		// Token: 0x04005728 RID: 22312
		public static int TARGET_SKILLS_CYCLE = 250;

		// Token: 0x04005729 RID: 22313
		public static float EXPERIENCE_LEVEL_POWER = 1.44f;

		// Token: 0x0400572A RID: 22314
		public static float PASSIVE_EXPERIENCE_PORTION = 0.5f;

		// Token: 0x0400572B RID: 22315
		public static float ACTIVE_EXPERIENCE_PORTION = 0.6f;

		// Token: 0x0400572C RID: 22316
		public static float FULL_EXPERIENCE = 1f;

		// Token: 0x0400572D RID: 22317
		public static float ALL_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.9f;

		// Token: 0x0400572E RID: 22318
		public static float MOST_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.75f;

		// Token: 0x0400572F RID: 22319
		public static float PART_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.5f;

		// Token: 0x04005730 RID: 22320
		public static float BARELY_EVER_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.25f;

		// Token: 0x04005731 RID: 22321
		public static float APTITUDE_EXPERIENCE_MULTIPLIER = 0.5f;

		// Token: 0x04005732 RID: 22322
		public static int[] SKILL_TIER_MORALE_COST = new int[]
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7
		};
	}
}
