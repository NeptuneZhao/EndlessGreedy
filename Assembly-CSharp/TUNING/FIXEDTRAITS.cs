using System;

namespace TUNING
{
	// Token: 0x02000EED RID: 3821
	public class FIXEDTRAITS
	{
		// Token: 0x02001FB5 RID: 8117
		public class NORTHERNLIGHTS
		{
			// Token: 0x04008FA8 RID: 36776
			public static int NONE = 0;

			// Token: 0x04008FA9 RID: 36777
			public static int ENABLED = 1;

			// Token: 0x04008FAA RID: 36778
			public static int DEFAULT_VALUE = FIXEDTRAITS.NORTHERNLIGHTS.NONE;

			// Token: 0x0200266E RID: 9838
			public class NAME
			{
				// Token: 0x0400AAAE RID: 43694
				public static string NONE = "northernLightsNone";

				// Token: 0x0400AAAF RID: 43695
				public static string ENABLED = "northernLightsOn";

				// Token: 0x0400AAB0 RID: 43696
				public static string DEFAULT = FIXEDTRAITS.NORTHERNLIGHTS.NAME.NONE;
			}
		}

		// Token: 0x02001FB6 RID: 8118
		public class SUNLIGHT
		{
			// Token: 0x04008FAB RID: 36779
			public static int DEFAULT_SPACED_OUT_SUNLIGHT = 40000;

			// Token: 0x04008FAC RID: 36780
			public static int NONE = 0;

			// Token: 0x04008FAD RID: 36781
			public static int VERY_VERY_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.25f);

			// Token: 0x04008FAE RID: 36782
			public static int VERY_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.5f);

			// Token: 0x04008FAF RID: 36783
			public static int LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.75f);

			// Token: 0x04008FB0 RID: 36784
			public static int MED_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.875f);

			// Token: 0x04008FB1 RID: 36785
			public static int MED = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT;

			// Token: 0x04008FB2 RID: 36786
			public static int MED_HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 1.25f);

			// Token: 0x04008FB3 RID: 36787
			public static int HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 1.5f);

			// Token: 0x04008FB4 RID: 36788
			public static int VERY_HIGH = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 2;

			// Token: 0x04008FB5 RID: 36789
			public static int VERY_VERY_HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 2.5f);

			// Token: 0x04008FB6 RID: 36790
			public static int VERY_VERY_VERY_HIGH = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 3;

			// Token: 0x04008FB7 RID: 36791
			public static int DEFAULT_VALUE = FIXEDTRAITS.SUNLIGHT.VERY_HIGH;

			// Token: 0x0200266F RID: 9839
			public class NAME
			{
				// Token: 0x0400AAB1 RID: 43697
				public static string NONE = "sunlightNone";

				// Token: 0x0400AAB2 RID: 43698
				public static string VERY_VERY_LOW = "sunlightVeryVeryLow";

				// Token: 0x0400AAB3 RID: 43699
				public static string VERY_LOW = "sunlightVeryLow";

				// Token: 0x0400AAB4 RID: 43700
				public static string LOW = "sunlightLow";

				// Token: 0x0400AAB5 RID: 43701
				public static string MED_LOW = "sunlightMedLow";

				// Token: 0x0400AAB6 RID: 43702
				public static string MED = "sunlightMed";

				// Token: 0x0400AAB7 RID: 43703
				public static string MED_HIGH = "sunlightMedHigh";

				// Token: 0x0400AAB8 RID: 43704
				public static string HIGH = "sunlightHigh";

				// Token: 0x0400AAB9 RID: 43705
				public static string VERY_HIGH = "sunlightVeryHigh";

				// Token: 0x0400AABA RID: 43706
				public static string VERY_VERY_HIGH = "sunlightVeryVeryHigh";

				// Token: 0x0400AABB RID: 43707
				public static string VERY_VERY_VERY_HIGH = "sunlightVeryVeryVeryHigh";

				// Token: 0x0400AABC RID: 43708
				public static string DEFAULT = FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH;
			}
		}

		// Token: 0x02001FB7 RID: 8119
		public class COSMICRADIATION
		{
			// Token: 0x04008FB8 RID: 36792
			public static int BASELINE = 250;

			// Token: 0x04008FB9 RID: 36793
			public static int NONE = 0;

			// Token: 0x04008FBA RID: 36794
			public static int VERY_VERY_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.25f);

			// Token: 0x04008FBB RID: 36795
			public static int VERY_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.5f);

			// Token: 0x04008FBC RID: 36796
			public static int LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.75f);

			// Token: 0x04008FBD RID: 36797
			public static int MED_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.875f);

			// Token: 0x04008FBE RID: 36798
			public static int MED = FIXEDTRAITS.COSMICRADIATION.BASELINE;

			// Token: 0x04008FBF RID: 36799
			public static int MED_HIGH = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 1.25f);

			// Token: 0x04008FC0 RID: 36800
			public static int HIGH = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 1.5f);

			// Token: 0x04008FC1 RID: 36801
			public static int VERY_HIGH = FIXEDTRAITS.COSMICRADIATION.BASELINE * 2;

			// Token: 0x04008FC2 RID: 36802
			public static int VERY_VERY_HIGH = FIXEDTRAITS.COSMICRADIATION.BASELINE * 3;

			// Token: 0x04008FC3 RID: 36803
			public static int DEFAULT_VALUE = FIXEDTRAITS.COSMICRADIATION.MED;

			// Token: 0x04008FC4 RID: 36804
			public static float TELESCOPE_RADIATION_SHIELDING = 0.5f;

			// Token: 0x02002670 RID: 9840
			public class NAME
			{
				// Token: 0x0400AABD RID: 43709
				public static string NONE = "cosmicRadiationNone";

				// Token: 0x0400AABE RID: 43710
				public static string VERY_VERY_LOW = "cosmicRadiationVeryVeryLow";

				// Token: 0x0400AABF RID: 43711
				public static string VERY_LOW = "cosmicRadiationVeryLow";

				// Token: 0x0400AAC0 RID: 43712
				public static string LOW = "cosmicRadiationLow";

				// Token: 0x0400AAC1 RID: 43713
				public static string MED_LOW = "cosmicRadiationMedLow";

				// Token: 0x0400AAC2 RID: 43714
				public static string MED = "cosmicRadiationMed";

				// Token: 0x0400AAC3 RID: 43715
				public static string MED_HIGH = "cosmicRadiationMedHigh";

				// Token: 0x0400AAC4 RID: 43716
				public static string HIGH = "cosmicRadiationHigh";

				// Token: 0x0400AAC5 RID: 43717
				public static string VERY_HIGH = "cosmicRadiationVeryHigh";

				// Token: 0x0400AAC6 RID: 43718
				public static string VERY_VERY_HIGH = "cosmicRadiationVeryVeryHigh";

				// Token: 0x0400AAC7 RID: 43719
				public static string DEFAULT = FIXEDTRAITS.COSMICRADIATION.NAME.MED;
			}
		}
	}
}
