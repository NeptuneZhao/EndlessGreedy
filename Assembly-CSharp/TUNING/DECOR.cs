using System;
using STRINGS;

namespace TUNING
{
	// Token: 0x02000EF2 RID: 3826
	public class DECOR
	{
		// Token: 0x04005737 RID: 22327
		public static int LIT_BONUS = 15;

		// Token: 0x04005738 RID: 22328
		public static readonly EffectorValues NONE = new EffectorValues
		{
			amount = 0,
			radius = 0
		};

		// Token: 0x02001FC1 RID: 8129
		public class BONUS
		{
			// Token: 0x04008FE5 RID: 36837
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 10,
				radius = 1
			};

			// Token: 0x04008FE6 RID: 36838
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 15,
				radius = 2
			};

			// Token: 0x04008FE7 RID: 36839
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 20,
				radius = 3
			};

			// Token: 0x04008FE8 RID: 36840
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 25,
				radius = 4
			};

			// Token: 0x04008FE9 RID: 36841
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 30,
				radius = 5
			};

			// Token: 0x04008FEA RID: 36842
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 35,
				radius = 6
			};

			// Token: 0x04008FEB RID: 36843
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 50,
				radius = 7
			};

			// Token: 0x04008FEC RID: 36844
			public static readonly EffectorValues TIER7 = new EffectorValues
			{
				amount = 80,
				radius = 7
			};

			// Token: 0x04008FED RID: 36845
			public static readonly EffectorValues TIER8 = new EffectorValues
			{
				amount = 200,
				radius = 8
			};
		}

		// Token: 0x02001FC2 RID: 8130
		public class PENALTY
		{
			// Token: 0x04008FEE RID: 36846
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = -5,
				radius = 1
			};

			// Token: 0x04008FEF RID: 36847
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = -10,
				radius = 2
			};

			// Token: 0x04008FF0 RID: 36848
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = -15,
				radius = 3
			};

			// Token: 0x04008FF1 RID: 36849
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = -20,
				radius = 4
			};

			// Token: 0x04008FF2 RID: 36850
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = -20,
				radius = 5
			};

			// Token: 0x04008FF3 RID: 36851
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = -25,
				radius = 6
			};
		}

		// Token: 0x02001FC3 RID: 8131
		public class SPACEARTIFACT
		{
			// Token: 0x04008FF4 RID: 36852
			public static readonly ArtifactTier TIER_NONE = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER_NONE.key, DECOR.NONE, 0f);

			// Token: 0x04008FF5 RID: 36853
			public static readonly ArtifactTier TIER0 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER0.key, DECOR.BONUS.TIER0, 0.25f);

			// Token: 0x04008FF6 RID: 36854
			public static readonly ArtifactTier TIER1 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER1.key, DECOR.BONUS.TIER2, 0.4f);

			// Token: 0x04008FF7 RID: 36855
			public static readonly ArtifactTier TIER2 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER2.key, DECOR.BONUS.TIER4, 0.55f);

			// Token: 0x04008FF8 RID: 36856
			public static readonly ArtifactTier TIER3 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER3.key, DECOR.BONUS.TIER5, 0.7f);

			// Token: 0x04008FF9 RID: 36857
			public static readonly ArtifactTier TIER4 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER4.key, DECOR.BONUS.TIER6, 0.85f);

			// Token: 0x04008FFA RID: 36858
			public static readonly ArtifactTier TIER5 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER5.key, DECOR.BONUS.TIER7, 1f);
		}
	}
}
