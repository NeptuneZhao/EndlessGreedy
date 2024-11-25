using System;

namespace TUNING
{
	// Token: 0x02000F09 RID: 3849
	public class METEORS
	{
		// Token: 0x02002008 RID: 8200
		public class DIFFICULTY
		{
			// Token: 0x0200268A RID: 9866
			public class PEROID_MULTIPLIER
			{
				// Token: 0x0400AB2C RID: 43820
				public const float INFREQUENT = 2f;

				// Token: 0x0400AB2D RID: 43821
				public const float INTENSE = 1f;

				// Token: 0x0400AB2E RID: 43822
				public const float DOOMED = 1f;
			}

			// Token: 0x0200268B RID: 9867
			public class SECONDS_PER_METEOR_MULTIPLIER
			{
				// Token: 0x0400AB2F RID: 43823
				public const float INFREQUENT = 1.5f;

				// Token: 0x0400AB30 RID: 43824
				public const float INTENSE = 0.8f;

				// Token: 0x0400AB31 RID: 43825
				public const float DOOMED = 0.5f;
			}

			// Token: 0x0200268C RID: 9868
			public class BOMBARD_OFF_MULTIPLIER
			{
				// Token: 0x0400AB32 RID: 43826
				public const float INFREQUENT = 1f;

				// Token: 0x0400AB33 RID: 43827
				public const float INTENSE = 1f;

				// Token: 0x0400AB34 RID: 43828
				public const float DOOMED = 0.5f;
			}

			// Token: 0x0200268D RID: 9869
			public class BOMBARD_ON_MULTIPLIER
			{
				// Token: 0x0400AB35 RID: 43829
				public const float INFREQUENT = 1f;

				// Token: 0x0400AB36 RID: 43830
				public const float INTENSE = 1f;

				// Token: 0x0400AB37 RID: 43831
				public const float DOOMED = 1f;
			}

			// Token: 0x0200268E RID: 9870
			public class MASS_MULTIPLIER
			{
				// Token: 0x0400AB38 RID: 43832
				public const float INFREQUENT = 1f;

				// Token: 0x0400AB39 RID: 43833
				public const float INTENSE = 0.8f;

				// Token: 0x0400AB3A RID: 43834
				public const float DOOMED = 0.5f;
			}
		}

		// Token: 0x02002009 RID: 8201
		public class IDENTIFY_DURATION
		{
			// Token: 0x040091E3 RID: 37347
			public const float TIER1 = 20f;
		}

		// Token: 0x0200200A RID: 8202
		public class PEROID
		{
			// Token: 0x040091E4 RID: 37348
			public const float TIER1 = 5f;

			// Token: 0x040091E5 RID: 37349
			public const float TIER2 = 10f;

			// Token: 0x040091E6 RID: 37350
			public const float TIER3 = 20f;

			// Token: 0x040091E7 RID: 37351
			public const float TIER4 = 30f;
		}

		// Token: 0x0200200B RID: 8203
		public class DURATION
		{
			// Token: 0x040091E8 RID: 37352
			public const float TIER0 = 1800f;

			// Token: 0x040091E9 RID: 37353
			public const float TIER1 = 3000f;

			// Token: 0x040091EA RID: 37354
			public const float TIER2 = 4200f;

			// Token: 0x040091EB RID: 37355
			public const float TIER3 = 6000f;
		}

		// Token: 0x0200200C RID: 8204
		public class DURATION_CLUSTER
		{
			// Token: 0x040091EC RID: 37356
			public const float TIER0 = 75f;

			// Token: 0x040091ED RID: 37357
			public const float TIER1 = 150f;

			// Token: 0x040091EE RID: 37358
			public const float TIER2 = 300f;

			// Token: 0x040091EF RID: 37359
			public const float TIER3 = 600f;

			// Token: 0x040091F0 RID: 37360
			public const float TIER4 = 1800f;

			// Token: 0x040091F1 RID: 37361
			public const float TIER5 = 3000f;
		}

		// Token: 0x0200200D RID: 8205
		public class TRAVEL_DURATION
		{
			// Token: 0x040091F2 RID: 37362
			public const float TIER0 = 600f;

			// Token: 0x040091F3 RID: 37363
			public const float TIER1 = 3000f;

			// Token: 0x040091F4 RID: 37364
			public const float TIER2 = 4500f;

			// Token: 0x040091F5 RID: 37365
			public const float TIER3 = 6000f;

			// Token: 0x040091F6 RID: 37366
			public const float TIER4 = 12000f;

			// Token: 0x040091F7 RID: 37367
			public const float TIER5 = 30000f;
		}

		// Token: 0x0200200E RID: 8206
		public class BOMBARDMENT_ON
		{
			// Token: 0x040091F8 RID: 37368
			public static MathUtil.MinMax NONE = new MathUtil.MinMax(1f, 1f);

			// Token: 0x040091F9 RID: 37369
			public static MathUtil.MinMax UNLIMITED = new MathUtil.MinMax(10000f, 10000f);

			// Token: 0x040091FA RID: 37370
			public static MathUtil.MinMax CYCLE = new MathUtil.MinMax(600f, 600f);
		}

		// Token: 0x0200200F RID: 8207
		public class BOMBARDMENT_OFF
		{
			// Token: 0x040091FB RID: 37371
			public static MathUtil.MinMax NONE = new MathUtil.MinMax(1f, 1f);
		}

		// Token: 0x02002010 RID: 8208
		public class TRAVELDURATION
		{
			// Token: 0x040091FC RID: 37372
			public static float TIER0 = 0f;

			// Token: 0x040091FD RID: 37373
			public static float TIER1 = 5f;

			// Token: 0x040091FE RID: 37374
			public static float TIER2 = 10f;

			// Token: 0x040091FF RID: 37375
			public static float TIER3 = 20f;

			// Token: 0x04009200 RID: 37376
			public static float TIER4 = 30f;
		}
	}
}
