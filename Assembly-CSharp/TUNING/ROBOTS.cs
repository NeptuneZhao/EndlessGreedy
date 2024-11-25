using System;

namespace TUNING
{
	// Token: 0x02000EF4 RID: 3828
	public class ROBOTS
	{
		// Token: 0x02001FC8 RID: 8136
		public class SCOUTBOT
		{
			// Token: 0x04009015 RID: 36885
			public static float CARRY_CAPACITY = DUPLICANTSTATS.STANDARD.BaseStats.CARRY_CAPACITY;

			// Token: 0x04009016 RID: 36886
			public static readonly float DIGGING = 1f;

			// Token: 0x04009017 RID: 36887
			public static readonly float CONSTRUCTION = 1f;

			// Token: 0x04009018 RID: 36888
			public static readonly float ATHLETICS = 1f;

			// Token: 0x04009019 RID: 36889
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x0400901A RID: 36890
			public static readonly float BATTERY_DEPLETION_RATE = 30f;

			// Token: 0x0400901B RID: 36891
			public static readonly float BATTERY_CAPACITY = ROBOTS.SCOUTBOT.BATTERY_DEPLETION_RATE * 10f * 600f;
		}

		// Token: 0x02001FC9 RID: 8137
		public class MORBBOT
		{
			// Token: 0x0400901C RID: 36892
			public static float CARRY_CAPACITY = DUPLICANTSTATS.STANDARD.BaseStats.CARRY_CAPACITY * 2f;

			// Token: 0x0400901D RID: 36893
			public const float DIGGING = 1f;

			// Token: 0x0400901E RID: 36894
			public const float CONSTRUCTION = 1f;

			// Token: 0x0400901F RID: 36895
			public const float ATHLETICS = 3f;

			// Token: 0x04009020 RID: 36896
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x04009021 RID: 36897
			public const float LIFETIME = 6000f;

			// Token: 0x04009022 RID: 36898
			public const float BATTERY_DEPLETION_RATE = 30f;

			// Token: 0x04009023 RID: 36899
			public const float BATTERY_CAPACITY = 180000f;

			// Token: 0x04009024 RID: 36900
			public const float DECONSTRUCTION_WORK_TIME = 10f;
		}

		// Token: 0x02001FCA RID: 8138
		public class FETCHDRONE
		{
			// Token: 0x04009025 RID: 36901
			public static float CARRY_CAPACITY = DUPLICANTSTATS.STANDARD.BaseStats.CARRY_CAPACITY * 2f;

			// Token: 0x04009026 RID: 36902
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x04009027 RID: 36903
			public const float BATTERY_DEPLETION_RATE = 30f;
		}
	}
}
