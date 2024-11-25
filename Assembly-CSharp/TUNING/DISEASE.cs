using System;

namespace TUNING
{
	// Token: 0x02000EF7 RID: 3831
	public class DISEASE
	{
		// Token: 0x04005781 RID: 22401
		public const int COUNT_SCALER = 1000;

		// Token: 0x04005782 RID: 22402
		public const int GENERIC_EMIT_COUNT = 100000;

		// Token: 0x04005783 RID: 22403
		public const float GENERIC_EMIT_INTERVAL = 5f;

		// Token: 0x04005784 RID: 22404
		public const float GENERIC_INFECTION_RADIUS = 1.5f;

		// Token: 0x04005785 RID: 22405
		public const float GENERIC_INFECTION_INTERVAL = 5f;

		// Token: 0x04005786 RID: 22406
		public const float STINKY_EMIT_MASS = 0.0025000002f;

		// Token: 0x04005787 RID: 22407
		public const float STINKY_EMIT_INTERVAL = 2.5f;

		// Token: 0x04005788 RID: 22408
		public const float STORAGE_TRANSFER_RATE = 0.05f;

		// Token: 0x04005789 RID: 22409
		public const float WORKABLE_TRANSFER_RATE = 0.33f;

		// Token: 0x0400578A RID: 22410
		public const float LADDER_TRANSFER_RATE = 0.005f;

		// Token: 0x0400578B RID: 22411
		public const float INTERNAL_GERM_DEATH_MULTIPLIER = -0.00066666666f;

		// Token: 0x0400578C RID: 22412
		public const float INTERNAL_GERM_DEATH_ADDEND = -0.8333333f;

		// Token: 0x0400578D RID: 22413
		public const float MINIMUM_IMMUNE_DAMAGE = 0.00016666666f;

		// Token: 0x02001FDC RID: 8156
		public class DURATION
		{
			// Token: 0x04009098 RID: 37016
			public const float LONG = 10800f;

			// Token: 0x04009099 RID: 37017
			public const float LONGISH = 4620f;

			// Token: 0x0400909A RID: 37018
			public const float NORMAL = 2220f;

			// Token: 0x0400909B RID: 37019
			public const float SHORT = 1020f;

			// Token: 0x0400909C RID: 37020
			public const float TEMPORARY = 180f;

			// Token: 0x0400909D RID: 37021
			public const float VERY_BRIEF = 60f;
		}

		// Token: 0x02001FDD RID: 8157
		public class IMMUNE_ATTACK_STRENGTH_PERCENT
		{
			// Token: 0x0400909E RID: 37022
			public const float SLOW_3 = 0.00025f;

			// Token: 0x0400909F RID: 37023
			public const float SLOW_2 = 0.0005f;

			// Token: 0x040090A0 RID: 37024
			public const float SLOW_1 = 0.00125f;

			// Token: 0x040090A1 RID: 37025
			public const float NORMAL = 0.005f;

			// Token: 0x040090A2 RID: 37026
			public const float FAST_1 = 0.0125f;

			// Token: 0x040090A3 RID: 37027
			public const float FAST_2 = 0.05f;

			// Token: 0x040090A4 RID: 37028
			public const float FAST_3 = 0.125f;
		}

		// Token: 0x02001FDE RID: 8158
		public class RADIATION_KILL_RATE
		{
			// Token: 0x040090A5 RID: 37029
			public const float NO_EFFECT = 0f;

			// Token: 0x040090A6 RID: 37030
			public const float SLOW = 1f;

			// Token: 0x040090A7 RID: 37031
			public const float NORMAL = 2.5f;

			// Token: 0x040090A8 RID: 37032
			public const float FAST = 5f;
		}

		// Token: 0x02001FDF RID: 8159
		public static class GROWTH_FACTOR
		{
			// Token: 0x040090A9 RID: 37033
			public const float NONE = float.PositiveInfinity;

			// Token: 0x040090AA RID: 37034
			public const float DEATH_1 = 12000f;

			// Token: 0x040090AB RID: 37035
			public const float DEATH_2 = 6000f;

			// Token: 0x040090AC RID: 37036
			public const float DEATH_3 = 3000f;

			// Token: 0x040090AD RID: 37037
			public const float DEATH_4 = 1200f;

			// Token: 0x040090AE RID: 37038
			public const float DEATH_5 = 300f;

			// Token: 0x040090AF RID: 37039
			public const float DEATH_MAX = 10f;

			// Token: 0x040090B0 RID: 37040
			public const float DEATH_INSTANT = 0f;

			// Token: 0x040090B1 RID: 37041
			public const float GROWTH_1 = -12000f;

			// Token: 0x040090B2 RID: 37042
			public const float GROWTH_2 = -6000f;

			// Token: 0x040090B3 RID: 37043
			public const float GROWTH_3 = -3000f;

			// Token: 0x040090B4 RID: 37044
			public const float GROWTH_4 = -1200f;

			// Token: 0x040090B5 RID: 37045
			public const float GROWTH_5 = -600f;

			// Token: 0x040090B6 RID: 37046
			public const float GROWTH_6 = -300f;

			// Token: 0x040090B7 RID: 37047
			public const float GROWTH_7 = -150f;
		}

		// Token: 0x02001FE0 RID: 8160
		public static class UNDERPOPULATION_DEATH_RATE
		{
			// Token: 0x040090B8 RID: 37048
			public const float NONE = 0f;

			// Token: 0x040090B9 RID: 37049
			private const float BASE_NUM_TO_KILL = 400f;

			// Token: 0x040090BA RID: 37050
			public const float SLOW = 0.6666667f;

			// Token: 0x040090BB RID: 37051
			public const float FAST = 2.6666667f;
		}
	}
}
