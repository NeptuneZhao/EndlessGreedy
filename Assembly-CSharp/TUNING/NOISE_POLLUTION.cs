using System;

namespace TUNING
{
	// Token: 0x02000EF3 RID: 3827
	public class NOISE_POLLUTION
	{
		// Token: 0x04005739 RID: 22329
		public static readonly EffectorValues NONE = new EffectorValues
		{
			amount = 0,
			radius = 0
		};

		// Token: 0x0400573A RID: 22330
		public static readonly EffectorValues CONE_OF_SILENCE = new EffectorValues
		{
			amount = -120,
			radius = 5
		};

		// Token: 0x0400573B RID: 22331
		public static float DUPLICANT_TIME_THRESHOLD = 3f;

		// Token: 0x02001FC4 RID: 8132
		public class LENGTHS
		{
			// Token: 0x04008FFB RID: 36859
			public static float VERYSHORT = 0.25f;

			// Token: 0x04008FFC RID: 36860
			public static float SHORT = 0.5f;

			// Token: 0x04008FFD RID: 36861
			public static float NORMAL = 1f;

			// Token: 0x04008FFE RID: 36862
			public static float LONG = 1.5f;

			// Token: 0x04008FFF RID: 36863
			public static float VERYLONG = 2f;
		}

		// Token: 0x02001FC5 RID: 8133
		public class NOISY
		{
			// Token: 0x04009000 RID: 36864
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 45,
				radius = 10
			};

			// Token: 0x04009001 RID: 36865
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 55,
				radius = 10
			};

			// Token: 0x04009002 RID: 36866
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 65,
				radius = 10
			};

			// Token: 0x04009003 RID: 36867
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 75,
				radius = 15
			};

			// Token: 0x04009004 RID: 36868
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 90,
				radius = 15
			};

			// Token: 0x04009005 RID: 36869
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 105,
				radius = 20
			};

			// Token: 0x04009006 RID: 36870
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 125,
				radius = 20
			};
		}

		// Token: 0x02001FC6 RID: 8134
		public class CREATURES
		{
			// Token: 0x04009007 RID: 36871
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 30,
				radius = 5
			};

			// Token: 0x04009008 RID: 36872
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 35,
				radius = 5
			};

			// Token: 0x04009009 RID: 36873
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 45,
				radius = 5
			};

			// Token: 0x0400900A RID: 36874
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 55,
				radius = 5
			};

			// Token: 0x0400900B RID: 36875
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 65,
				radius = 5
			};

			// Token: 0x0400900C RID: 36876
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 75,
				radius = 5
			};

			// Token: 0x0400900D RID: 36877
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 90,
				radius = 10
			};

			// Token: 0x0400900E RID: 36878
			public static readonly EffectorValues TIER7 = new EffectorValues
			{
				amount = 105,
				radius = 10
			};
		}

		// Token: 0x02001FC7 RID: 8135
		public class DAMPEN
		{
			// Token: 0x0400900F RID: 36879
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = -5,
				radius = 1
			};

			// Token: 0x04009010 RID: 36880
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = -10,
				radius = 2
			};

			// Token: 0x04009011 RID: 36881
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = -15,
				radius = 3
			};

			// Token: 0x04009012 RID: 36882
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = -20,
				radius = 4
			};

			// Token: 0x04009013 RID: 36883
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = -20,
				radius = 5
			};

			// Token: 0x04009014 RID: 36884
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = -25,
				radius = 6
			};
		}
	}
}
