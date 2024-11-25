using System;

namespace STRINGS
{
	// Token: 0x02000F23 RID: 3875
	public static class WORLD_TRAITS
	{
		// Token: 0x0400592A RID: 22826
		public static LocString MISSING_TRAIT = "<missing traits>";

		// Token: 0x0200230C RID: 8972
		public static class NO_TRAITS
		{
			// Token: 0x04009D04 RID: 40196
			public static LocString NAME = "<i>This world is stable and has no unusual features.</i>";

			// Token: 0x04009D05 RID: 40197
			public static LocString NAME_SHORTHAND = "No unusual features";

			// Token: 0x04009D06 RID: 40198
			public static LocString DESCRIPTION = "This world exists in a particularly stable configuration each time it is encountered";
		}

		// Token: 0x0200230D RID: 8973
		public static class BOULDERS_LARGE
		{
			// Token: 0x04009D07 RID: 40199
			public static LocString NAME = "Large Boulders";

			// Token: 0x04009D08 RID: 40200
			public static LocString DESCRIPTION = "Huge boulders make digging through this world more difficult";
		}

		// Token: 0x0200230E RID: 8974
		public static class BOULDERS_MEDIUM
		{
			// Token: 0x04009D09 RID: 40201
			public static LocString NAME = "Medium Boulders";

			// Token: 0x04009D0A RID: 40202
			public static LocString DESCRIPTION = "Mid-sized boulders make digging through this world more difficult";
		}

		// Token: 0x0200230F RID: 8975
		public static class BOULDERS_MIXED
		{
			// Token: 0x04009D0B RID: 40203
			public static LocString NAME = "Mixed Boulders";

			// Token: 0x04009D0C RID: 40204
			public static LocString DESCRIPTION = "Boulders of various sizes make digging through this world more difficult";
		}

		// Token: 0x02002310 RID: 8976
		public static class BOULDERS_SMALL
		{
			// Token: 0x04009D0D RID: 40205
			public static LocString NAME = "Small Boulders";

			// Token: 0x04009D0E RID: 40206
			public static LocString DESCRIPTION = "Tiny boulders make digging through this world more difficult";
		}

		// Token: 0x02002311 RID: 8977
		public static class DEEP_OIL
		{
			// Token: 0x04009D0F RID: 40207
			public static LocString NAME = "Trapped Oil";

			// Token: 0x04009D10 RID: 40208
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"Most of the ",
				UI.PRE_KEYWORD,
				"Oil",
				UI.PST_KEYWORD,
				" in this world will need to be extracted with ",
				BUILDINGS.PREFABS.OILWELLCAP.NAME,
				"s"
			});
		}

		// Token: 0x02002312 RID: 8978
		public static class FROZEN_CORE
		{
			// Token: 0x04009D11 RID: 40209
			public static LocString NAME = "Frozen Core";

			// Token: 0x04009D12 RID: 40210
			public static LocString DESCRIPTION = "This world has a chilly core of solid " + ELEMENTS.ICE.NAME;
		}

		// Token: 0x02002313 RID: 8979
		public static class GEOACTIVE
		{
			// Token: 0x04009D13 RID: 40211
			public static LocString NAME = "Geoactive";

			// Token: 0x04009D14 RID: 40212
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"This world has more ",
				UI.PRE_KEYWORD,
				"Geysers",
				UI.PST_KEYWORD,
				" and ",
				UI.PRE_KEYWORD,
				"Vents",
				UI.PST_KEYWORD,
				" than usual"
			});
		}

		// Token: 0x02002314 RID: 8980
		public static class GEODES
		{
			// Token: 0x04009D15 RID: 40213
			public static LocString NAME = "Geodes";

			// Token: 0x04009D16 RID: 40214
			public static LocString DESCRIPTION = "Large geodes containing rare material caches are deposited across this world";
		}

		// Token: 0x02002315 RID: 8981
		public static class GEODORMANT
		{
			// Token: 0x04009D17 RID: 40215
			public static LocString NAME = "Geodormant";

			// Token: 0x04009D18 RID: 40216
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"This world has fewer ",
				UI.PRE_KEYWORD,
				"Geysers",
				UI.PST_KEYWORD,
				" and ",
				UI.PRE_KEYWORD,
				"Vents",
				UI.PST_KEYWORD,
				" than usual"
			});
		}

		// Token: 0x02002316 RID: 8982
		public static class GLACIERS_LARGE
		{
			// Token: 0x04009D19 RID: 40217
			public static LocString NAME = "Large Glaciers";

			// Token: 0x04009D1A RID: 40218
			public static LocString DESCRIPTION = "Huge chunks of primordial " + ELEMENTS.ICE.NAME + " are scattered across this world";
		}

		// Token: 0x02002317 RID: 8983
		public static class IRREGULAR_OIL
		{
			// Token: 0x04009D1B RID: 40219
			public static LocString NAME = "Irregular Oil";

			// Token: 0x04009D1C RID: 40220
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"The ",
				UI.PRE_KEYWORD,
				"Oil",
				UI.PST_KEYWORD,
				" on this asteroid is anything but regular!"
			});
		}

		// Token: 0x02002318 RID: 8984
		public static class MAGMA_VENTS
		{
			// Token: 0x04009D1D RID: 40221
			public static LocString NAME = "Magma Channels";

			// Token: 0x04009D1E RID: 40222
			public static LocString DESCRIPTION = "The " + ELEMENTS.MAGMA.NAME + " from this world's core has leaked into the mantle and crust";
		}

		// Token: 0x02002319 RID: 8985
		public static class METAL_POOR
		{
			// Token: 0x04009D1F RID: 40223
			public static LocString NAME = "Metal Poor";

			// Token: 0x04009D20 RID: 40224
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"There is a reduced amount of ",
				UI.PRE_KEYWORD,
				"Metal Ore",
				UI.PST_KEYWORD,
				" on this world, proceed with caution!"
			});
		}

		// Token: 0x0200231A RID: 8986
		public static class METAL_RICH
		{
			// Token: 0x04009D21 RID: 40225
			public static LocString NAME = "Metal Rich";

			// Token: 0x04009D22 RID: 40226
			public static LocString DESCRIPTION = "This asteroid is an abundant source of " + UI.PRE_KEYWORD + "Metal Ore" + UI.PST_KEYWORD;
		}

		// Token: 0x0200231B RID: 8987
		public static class MISALIGNED_START
		{
			// Token: 0x04009D23 RID: 40227
			public static LocString NAME = "Alternate Pod Location";

			// Token: 0x04009D24 RID: 40228
			public static LocString DESCRIPTION = "The " + BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME + " didn't end up in the asteroid's exact center this time... but it's still nowhere near the surface";
		}

		// Token: 0x0200231C RID: 8988
		public static class SLIME_SPLATS
		{
			// Token: 0x04009D25 RID: 40229
			public static LocString NAME = "Slime Molds";

			// Token: 0x04009D26 RID: 40230
			public static LocString DESCRIPTION = "Sickly " + ELEMENTS.SLIMEMOLD.NAME + " growths have crept all over this world";
		}

		// Token: 0x0200231D RID: 8989
		public static class SUBSURFACE_OCEAN
		{
			// Token: 0x04009D27 RID: 40231
			public static LocString NAME = "Subsurface Ocean";

			// Token: 0x04009D28 RID: 40232
			public static LocString DESCRIPTION = "Below the crust of this world is a " + ELEMENTS.SALTWATER.NAME + " sea";
		}

		// Token: 0x0200231E RID: 8990
		public static class VOLCANOES
		{
			// Token: 0x04009D29 RID: 40233
			public static LocString NAME = "Volcanic Activity";

			// Token: 0x04009D2A RID: 40234
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"Several active ",
				UI.PRE_KEYWORD,
				"Volcanoes",
				UI.PST_KEYWORD,
				" have been detected in this world"
			});
		}

		// Token: 0x0200231F RID: 8991
		public static class RADIOACTIVE_CRUST
		{
			// Token: 0x04009D2B RID: 40235
			public static LocString NAME = "Radioactive Crust";

			// Token: 0x04009D2C RID: 40236
			public static LocString DESCRIPTION = "Deposits of " + ELEMENTS.URANIUMORE.NAME + " are found in this world's crust";
		}

		// Token: 0x02002320 RID: 8992
		public static class LUSH_CORE
		{
			// Token: 0x04009D2D RID: 40237
			public static LocString NAME = "Lush Core";

			// Token: 0x04009D2E RID: 40238
			public static LocString DESCRIPTION = "This world has a lush forest core";
		}

		// Token: 0x02002321 RID: 8993
		public static class METAL_CAVES
		{
			// Token: 0x04009D2F RID: 40239
			public static LocString NAME = "Metallic Caves";

			// Token: 0x04009D30 RID: 40240
			public static LocString DESCRIPTION = "This world has caves of metal ore";
		}

		// Token: 0x02002322 RID: 8994
		public static class DISTRESS_SIGNAL
		{
			// Token: 0x04009D31 RID: 40241
			public static LocString NAME = "Frozen Friend";

			// Token: 0x04009D32 RID: 40242
			public static LocString DESCRIPTION = "This world contains a frozen friend from a long time ago";
		}

		// Token: 0x02002323 RID: 8995
		public static class CRASHED_SATELLITES
		{
			// Token: 0x04009D33 RID: 40243
			public static LocString NAME = "Crashed Satellites";

			// Token: 0x04009D34 RID: 40244
			public static LocString DESCRIPTION = "This world contains crashed radioactive satellites";
		}
	}
}
