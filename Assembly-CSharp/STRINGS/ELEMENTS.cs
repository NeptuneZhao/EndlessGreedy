using System;

namespace STRINGS
{
	// Token: 0x02000F1C RID: 3868
	public class ELEMENTS
	{
		// Token: 0x0400591B RID: 22811
		public static LocString ELEMENTDESCSOLID = "Resource Type: {0}\nMelting point: {1}\nHardness: {2}";

		// Token: 0x0400591C RID: 22812
		public static LocString ELEMENTDESCLIQUID = "Resource Type: {0}\nFreezing point: {1}\nEvaporation point: {2}";

		// Token: 0x0400591D RID: 22813
		public static LocString ELEMENTDESCGAS = "Resource Type: {0}\nCondensation point: {1}";

		// Token: 0x0400591E RID: 22814
		public static LocString ELEMENTDESCVACUUM = "Resource Type: {0}";

		// Token: 0x0400591F RID: 22815
		public static LocString BREATHABLEDESC = "<color=#{0}>({1})</color>";

		// Token: 0x04005920 RID: 22816
		public static LocString THERMALPROPERTIES = "\nSpecific Heat Capacity: {SPECIFIC_HEAT_CAPACITY}\nThermal Conductivity: {THERMAL_CONDUCTIVITY}";

		// Token: 0x04005921 RID: 22817
		public static LocString RADIATIONPROPERTIES = "Radiation Absorption Factor: {0}\nRadiation Emission/1000kg: {1}";

		// Token: 0x04005922 RID: 22818
		public static LocString ELEMENTPROPERTIES = "Properties: {0}";

		// Token: 0x020021CE RID: 8654
		public class STATE
		{
			// Token: 0x040099C9 RID: 39369
			public static LocString SOLID = "Solid";

			// Token: 0x040099CA RID: 39370
			public static LocString LIQUID = "Liquid";

			// Token: 0x040099CB RID: 39371
			public static LocString GAS = "Gas";

			// Token: 0x040099CC RID: 39372
			public static LocString VACUUM = "None";
		}

		// Token: 0x020021CF RID: 8655
		public class MATERIAL_MODIFIERS
		{
			// Token: 0x040099CD RID: 39373
			public static LocString EFFECTS_HEADER = "<b>Resource Effects:</b>";

			// Token: 0x040099CE RID: 39374
			public static LocString DECOR = UI.FormatAsLink("Decor", "DECOR") + ": {0}";

			// Token: 0x040099CF RID: 39375
			public static LocString OVERHEATTEMPERATURE = UI.FormatAsLink("Overheat Temperature", "HEAT") + ": {0}";

			// Token: 0x040099D0 RID: 39376
			public static LocString HIGH_THERMAL_CONDUCTIVITY = UI.FormatAsLink("High Thermal Conductivity", "HEAT");

			// Token: 0x040099D1 RID: 39377
			public static LocString LOW_THERMAL_CONDUCTIVITY = UI.FormatAsLink("Insulator", "HEAT");

			// Token: 0x040099D2 RID: 39378
			public static LocString LOW_SPECIFIC_HEAT_CAPACITY = UI.FormatAsLink("Thermally Reactive", "HEAT");

			// Token: 0x040099D3 RID: 39379
			public static LocString HIGH_SPECIFIC_HEAT_CAPACITY = UI.FormatAsLink("Slow Heating", "HEAT");

			// Token: 0x040099D4 RID: 39380
			public static LocString EXCELLENT_RADIATION_SHIELD = UI.FormatAsLink("Excellent Radiation Shield", "RADIATION");

			// Token: 0x02003455 RID: 13397
			public class TOOLTIP
			{
				// Token: 0x0400D4AD RID: 54445
				public static LocString EFFECTS_HEADER = "Buildings constructed from this material will have these properties";

				// Token: 0x0400D4AE RID: 54446
				public static LocString DECOR = "This material will add <b>{0}</b> to the finished building's " + UI.PRE_KEYWORD + "Decor" + UI.PST_KEYWORD;

				// Token: 0x0400D4AF RID: 54447
				public static LocString OVERHEATTEMPERATURE = "This material will add <b>{0}</b> to the finished building's " + UI.PRE_KEYWORD + "Overheat Temperature" + UI.PST_KEYWORD;

				// Token: 0x0400D4B0 RID: 54448
				public static LocString HIGH_THERMAL_CONDUCTIVITY = string.Concat(new string[]
				{
					"This material disperses ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" because energy transfers quickly through materials with high ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nBetween two objects, the rate of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" transfer will be determined by the object with the <i>lowest</i> ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nThermal Conductivity: {1} W per degree K difference (Oxygen: 0.024 W)"
				});

				// Token: 0x0400D4B1 RID: 54449
				public static LocString LOW_THERMAL_CONDUCTIVITY = string.Concat(new string[]
				{
					"This material retains ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" because energy transfers slowly through materials with low ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nBetween two objects, the rate of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" transfer will be determined by the object with the <i>lowest</i> ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nThermal Conductivity: {1} W per degree K difference (Oxygen: 0.024 W)"
				});

				// Token: 0x0400D4B2 RID: 54450
				public static LocString LOW_SPECIFIC_HEAT_CAPACITY = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Thermally Reactive",
					UI.PST_KEYWORD,
					" materials require little energy to raise in ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					", and therefore heat and cool quickly\n\nSpecific Heat Capacity: {1} DTU to raise 1g by 1K"
				});

				// Token: 0x0400D4B3 RID: 54451
				public static LocString HIGH_SPECIFIC_HEAT_CAPACITY = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Slow Heating",
					UI.PST_KEYWORD,
					" materials require a large amount of energy to raise in ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					", and therefore heat and cool slowly\n\nSpecific Heat Capacity: {1} DTU to raise 1g by 1K"
				});

				// Token: 0x0400D4B4 RID: 54452
				public static LocString EXCELLENT_RADIATION_SHIELD = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Excellent Radiation Shield",
					UI.PST_KEYWORD,
					" radiation has a hard time passing through materials with a high ",
					UI.PRE_KEYWORD,
					"Radiation Absorption Factor",
					UI.PST_KEYWORD,
					" value. \n\nRadiation Absorption Factor: {1}"
				});
			}
		}

		// Token: 0x020021D0 RID: 8656
		public class HARDNESS
		{
			// Token: 0x040099D5 RID: 39381
			public static LocString NA = "N/A";

			// Token: 0x040099D6 RID: 39382
			public static LocString SOFT = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.SOFT + ")";

			// Token: 0x040099D7 RID: 39383
			public static LocString VERYSOFT = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYSOFT + ")";

			// Token: 0x040099D8 RID: 39384
			public static LocString FIRM = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.FIRM + ")";

			// Token: 0x040099D9 RID: 39385
			public static LocString VERYFIRM = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM + ")";

			// Token: 0x040099DA RID: 39386
			public static LocString NEARLYIMPENETRABLE = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.NEARLYIMPENETRABLE + ")";

			// Token: 0x040099DB RID: 39387
			public static LocString IMPENETRABLE = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.IMPENETRABLE + ")";

			// Token: 0x02003456 RID: 13398
			public class HARDNESS_DESCRIPTOR
			{
				// Token: 0x0400D4B5 RID: 54453
				public static LocString SOFT = "Soft";

				// Token: 0x0400D4B6 RID: 54454
				public static LocString VERYSOFT = "Very Soft";

				// Token: 0x0400D4B7 RID: 54455
				public static LocString FIRM = "Firm";

				// Token: 0x0400D4B8 RID: 54456
				public static LocString VERYFIRM = "Very Firm";

				// Token: 0x0400D4B9 RID: 54457
				public static LocString NEARLYIMPENETRABLE = "Nearly Impenetrable";

				// Token: 0x0400D4BA RID: 54458
				public static LocString IMPENETRABLE = "Impenetrable";
			}
		}

		// Token: 0x020021D1 RID: 8657
		public class AEROGEL
		{
			// Token: 0x040099DC RID: 39388
			public static LocString NAME = UI.FormatAsLink("Aerogel", "AEROGEL");

			// Token: 0x040099DD RID: 39389
			public static LocString DESC = "";
		}

		// Token: 0x020021D2 RID: 8658
		public class ALGAE
		{
			// Token: 0x040099DE RID: 39390
			public static LocString NAME = UI.FormatAsLink("Algae", "ALGAE");

			// Token: 0x040099DF RID: 39391
			public static LocString DESC = string.Concat(new string[]
			{
				"Algae is a cluster of non-motile, single-celled lifeforms.\n\nIt can be used to produce ",
				ELEMENTS.OXYGEN.NAME,
				" when used in an ",
				BUILDINGS.PREFABS.MINERALDEOXIDIZER.NAME,
				"."
			});
		}

		// Token: 0x020021D3 RID: 8659
		public class ALUMINUMORE
		{
			// Token: 0x040099E0 RID: 39392
			public static LocString NAME = UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE");

			// Token: 0x040099E1 RID: 39393
			public static LocString DESC = "Aluminum ore, also known as Bauxite, is a sedimentary rock high in metal content.\n\nIt can be refined into " + UI.FormatAsLink("Aluminum", "ALUMINUM") + ".";
		}

		// Token: 0x020021D4 RID: 8660
		public class ALUMINUM
		{
			// Token: 0x040099E2 RID: 39394
			public static LocString NAME = UI.FormatAsLink("Aluminum", "ALUMINUM");

			// Token: 0x040099E3 RID: 39395
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Aluminum is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				".\n\nIt has high Thermal Conductivity and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020021D5 RID: 8661
		public class MOLTENALUMINUM
		{
			// Token: 0x040099E4 RID: 39396
			public static LocString NAME = UI.FormatAsLink("Molten Aluminum", "MOLTENALUMINUM");

			// Token: 0x040099E5 RID: 39397
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Molten Aluminum is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x020021D6 RID: 8662
		public class ALUMINUMGAS
		{
			// Token: 0x040099E6 RID: 39398
			public static LocString NAME = UI.FormatAsLink("Aluminum Gas", "ALUMINUMGAS");

			// Token: 0x040099E7 RID: 39399
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Aluminum Gas is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020021D7 RID: 8663
		public class BLEACHSTONE
		{
			// Token: 0x040099E8 RID: 39400
			public static LocString NAME = UI.FormatAsLink("Bleach Stone", "BLEACHSTONE");

			// Token: 0x040099E9 RID: 39401
			public static LocString DESC = string.Concat(new string[]
			{
				"Bleach stone is an unstable compound that emits unbreathable ",
				UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
				".\n\nIt is useful in ",
				UI.FormatAsLink("Hygienic", "HYGIENE"),
				" processes."
			});
		}

		// Token: 0x020021D8 RID: 8664
		public class BITUMEN
		{
			// Token: 0x040099EA RID: 39402
			public static LocString NAME = UI.FormatAsLink("Bitumen", "BITUMEN");

			// Token: 0x040099EB RID: 39403
			public static LocString DESC = "Bitumen is a sticky viscous residue left behind from " + ELEMENTS.PETROLEUM.NAME + " production.";
		}

		// Token: 0x020021D9 RID: 8665
		public class BOTTLEDWATER
		{
			// Token: 0x040099EC RID: 39404
			public static LocString NAME = UI.FormatAsLink("Water", "BOTTLEDWATER");

			// Token: 0x040099ED RID: 39405
			public static LocString DESC = "(H<sub>2</sub>O) Clean " + ELEMENTS.WATER.NAME + ", prepped for transport.";
		}

		// Token: 0x020021DA RID: 8666
		public class BRINEICE
		{
			// Token: 0x040099EE RID: 39406
			public static LocString NAME = UI.FormatAsLink("Brine Ice", "BRINEICE");

			// Token: 0x040099EF RID: 39407
			public static LocString DESC = string.Concat(new string[]
			{
				"Brine Ice is a natural, highly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				" and frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x020021DB RID: 8667
		public class MILKICE
		{
			// Token: 0x040099F0 RID: 39408
			public static LocString NAME = UI.FormatAsLink("Frozen Brackene", "MILKICE");

			// Token: 0x040099F1 RID: 39409
			public static LocString DESC = string.Concat(new string[]
			{
				"Frozen Brackene is ",
				UI.FormatAsLink("Brackene", "MILK"),
				" frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x020021DC RID: 8668
		public class BRINE
		{
			// Token: 0x040099F2 RID: 39410
			public static LocString NAME = UI.FormatAsLink("Brine", "BRINE");

			// Token: 0x040099F3 RID: 39411
			public static LocString DESC = string.Concat(new string[]
			{
				"Brine is a natural, highly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x020021DD RID: 8669
		public class CARBON
		{
			// Token: 0x040099F4 RID: 39412
			public static LocString NAME = UI.FormatAsLink("Coal", "CARBON");

			// Token: 0x040099F5 RID: 39413
			public static LocString DESC = "(C) Coal is a combustible fossil fuel composed of carbon.\n\nIt is useful in " + UI.FormatAsLink("Power", "POWER") + " production.";
		}

		// Token: 0x020021DE RID: 8670
		public class REFINEDCARBON
		{
			// Token: 0x040099F6 RID: 39414
			public static LocString NAME = UI.FormatAsLink("Refined Carbon", "REFINEDCARBON");

			// Token: 0x040099F7 RID: 39415
			public static LocString DESC = "(C) Refined carbon is solid element purified from raw " + ELEMENTS.CARBON.NAME + ".";
		}

		// Token: 0x020021DF RID: 8671
		public class ETHANOLGAS
		{
			// Token: 0x040099F8 RID: 39416
			public static LocString NAME = UI.FormatAsLink("Ethanol Gas", "ETHANOLGAS");

			// Token: 0x040099F9 RID: 39417
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Ethanol Gas is an advanced chemical compound heated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x020021E0 RID: 8672
		public class ETHANOL
		{
			// Token: 0x040099FA RID: 39418
			public static LocString NAME = UI.FormatAsLink("Ethanol", "ETHANOL");

			// Token: 0x040099FB RID: 39419
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Ethanol is an advanced chemical compound in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.\n\nIt can be used as a highly effective fuel source when burned.";
		}

		// Token: 0x020021E1 RID: 8673
		public class SOLIDETHANOL
		{
			// Token: 0x040099FC RID: 39420
			public static LocString NAME = UI.FormatAsLink("Solid Ethanol", "SOLIDETHANOL");

			// Token: 0x040099FD RID: 39421
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Solid Ethanol is an advanced chemical compound.\n\nIt can be used as a highly effective fuel source when burned.";
		}

		// Token: 0x020021E2 RID: 8674
		public class CARBONDIOXIDE
		{
			// Token: 0x040099FE RID: 39422
			public static LocString NAME = UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE");

			// Token: 0x040099FF RID: 39423
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an atomically heavy chemical compound in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.\n\nIt tends to sink below other gases.";
		}

		// Token: 0x020021E3 RID: 8675
		public class CARBONFIBRE
		{
			// Token: 0x04009A00 RID: 39424
			public static LocString NAME = UI.FormatAsLink("Carbon Fiber", "CARBONFIBRE");

			// Token: 0x04009A01 RID: 39425
			public static LocString DESC = "Carbon Fiber is a " + UI.FormatAsLink("Manufactured Material", "REFINEDMINERAL") + " with high tensile strength.";
		}

		// Token: 0x020021E4 RID: 8676
		public class CARBONGAS
		{
			// Token: 0x04009A02 RID: 39426
			public static LocString NAME = UI.FormatAsLink("Carbon Gas", "CARBONGAS");

			// Token: 0x04009A03 RID: 39427
			public static LocString DESC = "(C) Carbon is an abundant, versatile element heated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x020021E5 RID: 8677
		public class CHLORINE
		{
			// Token: 0x04009A04 RID: 39428
			public static LocString NAME = UI.FormatAsLink("Liquid Chlorine", "CHLORINE");

			// Token: 0x04009A05 RID: 39429
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x020021E6 RID: 8678
		public class CHLORINEGAS
		{
			// Token: 0x04009A06 RID: 39430
			public static LocString NAME = UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS");

			// Token: 0x04009A07 RID: 39431
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020021E7 RID: 8679
		public class CLAY
		{
			// Token: 0x04009A08 RID: 39432
			public static LocString NAME = UI.FormatAsLink("Clay", "CLAY");

			// Token: 0x04009A09 RID: 39433
			public static LocString DESC = "Clay is a soft, naturally occurring composite of stone and soil that hardens at high " + UI.FormatAsLink("Temperatures", "HEAT") + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x020021E8 RID: 8680
		public class BRICK
		{
			// Token: 0x04009A0A RID: 39434
			public static LocString NAME = UI.FormatAsLink("Brick", "BRICK");

			// Token: 0x04009A0B RID: 39435
			public static LocString DESC = "Brick is a hard, brittle material formed from heated " + ELEMENTS.CLAY.NAME + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x020021E9 RID: 8681
		public class CERAMIC
		{
			// Token: 0x04009A0C RID: 39436
			public static LocString NAME = UI.FormatAsLink("Ceramic", "CERAMIC");

			// Token: 0x04009A0D RID: 39437
			public static LocString DESC = "Ceramic is a hard, brittle material formed from heated " + ELEMENTS.CLAY.NAME + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x020021EA RID: 8682
		public class CEMENT
		{
			// Token: 0x04009A0E RID: 39438
			public static LocString NAME = UI.FormatAsLink("Cement", "CEMENT");

			// Token: 0x04009A0F RID: 39439
			public static LocString DESC = "Cement is a refined building material used for assembling advanced buildings.";
		}

		// Token: 0x020021EB RID: 8683
		public class CEMENTMIX
		{
			// Token: 0x04009A10 RID: 39440
			public static LocString NAME = UI.FormatAsLink("Cement Mix", "CEMENTMIX");

			// Token: 0x04009A11 RID: 39441
			public static LocString DESC = "Cement Mix can be used to create " + ELEMENTS.CEMENT.NAME + " for advanced building assembly.";
		}

		// Token: 0x020021EC RID: 8684
		public class CONTAMINATEDOXYGEN
		{
			// Token: 0x04009A12 RID: 39442
			public static LocString NAME = UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN");

			// Token: 0x04009A13 RID: 39443
			public static LocString DESC = "(O<sub>2</sub>) Polluted Oxygen is dirty, unfiltered air.\n\nIt is breathable.";
		}

		// Token: 0x020021ED RID: 8685
		public class COPPER
		{
			// Token: 0x04009A14 RID: 39444
			public static LocString NAME = UI.FormatAsLink("Copper", "COPPER");

			// Token: 0x04009A15 RID: 39445
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Copper is a conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020021EE RID: 8686
		public class COPPERGAS
		{
			// Token: 0x04009A16 RID: 39446
			public static LocString NAME = UI.FormatAsLink("Copper Gas", "COPPERGAS");

			// Token: 0x04009A17 RID: 39447
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Copper Gas is a conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				" heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020021EF RID: 8687
		public class CREATURE
		{
			// Token: 0x04009A18 RID: 39448
			public static LocString NAME = UI.FormatAsLink("Genetic Ooze", "CREATURE");

			// Token: 0x04009A19 RID: 39449
			public static LocString DESC = "(DuPe) Ooze is a slurry of water, carbon, and dozens and dozens of trace elements.\n\nDuplicants are printed from pure Ooze.";
		}

		// Token: 0x020021F0 RID: 8688
		public class PHYTOOIL
		{
			// Token: 0x04009A1A RID: 39450
			public static LocString NAME = UI.FormatAsLink("Phyto Oil", "PHYTOOIL");

			// Token: 0x04009A1B RID: 39451
			public static LocString DESC = string.Concat(new string[]
			{
				"Phyto Oil is a thick, slippery ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" extracted from pureed ",
				UI.FormatAsLink("Slime", "SLIME"),
				"."
			});
		}

		// Token: 0x020021F1 RID: 8689
		public class FROZENPHYTOOIL
		{
			// Token: 0x04009A1C RID: 39452
			public static LocString NAME = UI.FormatAsLink("Frozen Phyto Oil", "FROZENPHYTOOIL");

			// Token: 0x04009A1D RID: 39453
			public static LocString DESC = string.Concat(new string[]
			{
				"Frozen Phyto Oil is thick, slippery ",
				UI.FormatAsLink("Slime", "SLIME"),
				" puree extract frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x020021F2 RID: 8690
		public class CRUDEOIL
		{
			// Token: 0x04009A1E RID: 39454
			public static LocString NAME = UI.FormatAsLink("Crude Oil", "CRUDEOIL");

			// Token: 0x04009A1F RID: 39455
			public static LocString DESC = "Crude Oil is a raw potential " + UI.FormatAsLink("Power", "POWER") + " source composed of billions of dead, primordial organisms.\n\nIt is also a useful lubricant for certain types of machinery.";
		}

		// Token: 0x020021F3 RID: 8691
		public class PETROLEUM
		{
			// Token: 0x04009A20 RID: 39456
			public static LocString NAME = UI.FormatAsLink("Petroleum", "PETROLEUM");

			// Token: 0x04009A21 RID: 39457
			public static LocString NAME_TWO = UI.FormatAsLink("Petroleum", "PETROLEUM");

			// Token: 0x04009A22 RID: 39458
			public static LocString DESC = string.Concat(new string[]
			{
				"Petroleum is a ",
				UI.FormatAsLink("Power", "POWER"),
				" source refined from ",
				UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
				".\n\nIt is also an essential ingredient in the production of ",
				UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
				"."
			});
		}

		// Token: 0x020021F4 RID: 8692
		public class SOURGAS
		{
			// Token: 0x04009A23 RID: 39459
			public static LocString NAME = UI.FormatAsLink("Sour Gas", "SOURGAS");

			// Token: 0x04009A24 RID: 39460
			public static LocString NAME_TWO = UI.FormatAsLink("Sour Gas", "SOURGAS");

			// Token: 0x04009A25 RID: 39461
			public static LocString DESC = string.Concat(new string[]
			{
				"Sour Gas is a hydrocarbon ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" containing high concentrations of hydrogen sulfide.\n\nIt is a byproduct of highly heated ",
				UI.FormatAsLink("Petroleum", "PETROLEUM"),
				"."
			});
		}

		// Token: 0x020021F5 RID: 8693
		public class CRUSHEDICE
		{
			// Token: 0x04009A26 RID: 39462
			public static LocString NAME = UI.FormatAsLink("Crushed Ice", "CRUSHEDICE");

			// Token: 0x04009A27 RID: 39463
			public static LocString DESC = "(H<sub>2</sub>O) A slush of crushed, semi-solid ice.";
		}

		// Token: 0x020021F6 RID: 8694
		public class CRUSHEDROCK
		{
			// Token: 0x04009A28 RID: 39464
			public static LocString NAME = UI.FormatAsLink("Crushed Rock", "CRUSHEDROCK");

			// Token: 0x04009A29 RID: 39465
			public static LocString DESC = "Crushed Rock is " + UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK") + " crushed into a mechanical mixture.";
		}

		// Token: 0x020021F7 RID: 8695
		public class CUPRITE
		{
			// Token: 0x04009A2A RID: 39466
			public static LocString NAME = UI.FormatAsLink("Copper Ore", "CUPRITE");

			// Token: 0x04009A2B RID: 39467
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu<sub>2</sub>O) Copper Ore is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020021F8 RID: 8696
		public class DEPLETEDURANIUM
		{
			// Token: 0x04009A2C RID: 39468
			public static LocString NAME = UI.FormatAsLink("Depleted Uranium", "DEPLETEDURANIUM");

			// Token: 0x04009A2D RID: 39469
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Depleted Uranium is ",
				UI.FormatAsLink("Uranium", "URANIUMORE"),
				" with a low U-235 content.\n\nIt is created as a byproduct of ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				" and is no longer suitable as fuel."
			});
		}

		// Token: 0x020021F9 RID: 8697
		public class DIAMOND
		{
			// Token: 0x04009A2E RID: 39470
			public static LocString NAME = UI.FormatAsLink("Diamond", "DIAMOND");

			// Token: 0x04009A2F RID: 39471
			public static LocString DESC = "(C) Diamond is industrial-grade, high density carbon.\n\nIt is very difficult to excavate.";
		}

		// Token: 0x020021FA RID: 8698
		public class DIRT
		{
			// Token: 0x04009A30 RID: 39472
			public static LocString NAME = UI.FormatAsLink("Dirt", "DIRT");

			// Token: 0x04009A31 RID: 39473
			public static LocString DESC = "Dirt is a soft, nutrient-rich substance capable of supporting life.\n\nIt is necessary in some forms of " + UI.FormatAsLink("Food", "FOOD") + " production.";
		}

		// Token: 0x020021FB RID: 8699
		public class DIRTYICE
		{
			// Token: 0x04009A32 RID: 39474
			public static LocString NAME = UI.FormatAsLink("Polluted Ice", "DIRTYICE");

			// Token: 0x04009A33 RID: 39475
			public static LocString DESC = "Polluted Ice is dirty, unfiltered water frozen into a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x020021FC RID: 8700
		public class DIRTYWATER
		{
			// Token: 0x04009A34 RID: 39476
			public static LocString NAME = UI.FormatAsLink("Polluted Water", "DIRTYWATER");

			// Token: 0x04009A35 RID: 39477
			public static LocString DESC = "Polluted Water is dirty, unfiltered " + UI.FormatAsLink("Water", "WATER") + ".\n\nIt is not fit for consumption.";
		}

		// Token: 0x020021FD RID: 8701
		public class ELECTRUM
		{
			// Token: 0x04009A36 RID: 39478
			public static LocString NAME = UI.FormatAsLink("Electrum", "ELECTRUM");

			// Token: 0x04009A37 RID: 39479
			public static LocString DESC = string.Concat(new string[]
			{
				"Electrum is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" alloy composed of gold and silver.\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020021FE RID: 8702
		public class ENRICHEDURANIUM
		{
			// Token: 0x04009A38 RID: 39480
			public static LocString NAME = UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM");

			// Token: 0x04009A39 RID: 39481
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Enriched Uranium is a highly ",
				UI.FormatAsLink("Radioactive", "RADIATION"),
				", refined substance.\n\nIt is primarily used to ",
				UI.FormatAsLink("Power", "POWER"),
				" potent research reactors."
			});
		}

		// Token: 0x020021FF RID: 8703
		public class FERTILIZER
		{
			// Token: 0x04009A3A RID: 39482
			public static LocString NAME = UI.FormatAsLink("Fertilizer", "FERTILIZER");

			// Token: 0x04009A3B RID: 39483
			public static LocString DESC = "Fertilizer is a processed mixture of biological nutrients.\n\nIt aids in the growth of certain " + UI.FormatAsLink("Plants", "PLANTS") + ".";
		}

		// Token: 0x02002200 RID: 8704
		public class PONDSCUM
		{
			// Token: 0x04009A3C RID: 39484
			public static LocString NAME = UI.FormatAsLink("Pondscum", "PONDSCUM");

			// Token: 0x04009A3D RID: 39485
			public static LocString DESC = string.Concat(new string[]
			{
				"Pondscum is a soft, naturally occurring composite of biological nutrients.\n\nIt may be processed into ",
				UI.FormatAsLink("Fertilizer", "FERTILIZER"),
				" and aids in the growth of certain ",
				UI.FormatAsLink("Plants", "PLANTS"),
				"."
			});
		}

		// Token: 0x02002201 RID: 8705
		public class FALLOUT
		{
			// Token: 0x04009A3E RID: 39486
			public static LocString NAME = UI.FormatAsLink("Nuclear Fallout", "FALLOUT");

			// Token: 0x04009A3F RID: 39487
			public static LocString DESC = string.Concat(new string[]
			{
				"Nuclear Fallout is a highly toxic gas full of ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATION"),
				". Condenses into ",
				UI.FormatAsLink("Liquid Nuclear Waste", "NUCLEARWASTE"),
				"."
			});
		}

		// Token: 0x02002202 RID: 8706
		public class FOOLSGOLD
		{
			// Token: 0x04009A40 RID: 39488
			public static LocString NAME = UI.FormatAsLink("Pyrite", "FOOLSGOLD");

			// Token: 0x04009A41 RID: 39489
			public static LocString DESC = string.Concat(new string[]
			{
				"(FeS<sub>2</sub>) Pyrite is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nAlso known as \"Fool's Gold\", is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02002203 RID: 8707
		public class FULLERENE
		{
			// Token: 0x04009A42 RID: 39490
			public static LocString NAME = UI.FormatAsLink("Fullerene", "FULLERENE");

			// Token: 0x04009A43 RID: 39491
			public static LocString DESC = "(C<sub>60</sub>) Fullerene is a form of " + UI.FormatAsLink("Coal", "CARBON") + " consisting of spherical molecules.";
		}

		// Token: 0x02002204 RID: 8708
		public class GLASS
		{
			// Token: 0x04009A44 RID: 39492
			public static LocString NAME = UI.FormatAsLink("Glass", "GLASS");

			// Token: 0x04009A45 RID: 39493
			public static LocString DESC = "Glass is a brittle, transparent substance formed from " + UI.FormatAsLink("Sand", "SAND") + " fired at high temperatures.";
		}

		// Token: 0x02002205 RID: 8709
		public class GOLD
		{
			// Token: 0x04009A46 RID: 39494
			public static LocString NAME = UI.FormatAsLink("Gold", "GOLD");

			// Token: 0x04009A47 RID: 39495
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold is a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02002206 RID: 8710
		public class GOLDAMALGAM
		{
			// Token: 0x04009A48 RID: 39496
			public static LocString NAME = UI.FormatAsLink("Gold Amalgam", "GOLDAMALGAM");

			// Token: 0x04009A49 RID: 39497
			public static LocString DESC = "Gold Amalgam is a conductive amalgam of gold and mercury.\n\nIt is suitable for building " + UI.FormatAsLink("Power", "POWER") + " systems.";
		}

		// Token: 0x02002207 RID: 8711
		public class GOLDGAS
		{
			// Token: 0x04009A4A RID: 39498
			public static LocString NAME = UI.FormatAsLink("Gold Gas", "GOLDGAS");

			// Token: 0x04009A4B RID: 39499
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold Gas is a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02002208 RID: 8712
		public class GRANITE
		{
			// Token: 0x04009A4C RID: 39500
			public static LocString NAME = UI.FormatAsLink("Granite", "GRANITE");

			// Token: 0x04009A4D RID: 39501
			public static LocString DESC = "Granite is a dense composite of " + UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK") + ".\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02002209 RID: 8713
		public class GRAPHITE
		{
			// Token: 0x04009A4E RID: 39502
			public static LocString NAME = UI.FormatAsLink("Graphite", "GRAPHITE");

			// Token: 0x04009A4F RID: 39503
			public static LocString DESC = "(C) Graphite is the most stable form of carbon.\n\nIt has high thermal conductivity and is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x0200220A RID: 8714
		public class LIQUIDGUNK
		{
			// Token: 0x04009A50 RID: 39504
			public static LocString NAME = UI.FormatAsLink("Liquid Gunk", "LIQUIDGUNK");

			// Token: 0x04009A51 RID: 39505
			public static LocString DESC = "Liquid Gunk is the built-up grime and grit produced by Duplicants' bionic mechanisms.\n\nIt is unpleasantly viscous.";
		}

		// Token: 0x0200220B RID: 8715
		public class GUNK
		{
			// Token: 0x04009A52 RID: 39506
			public static LocString NAME = UI.FormatAsLink("Gunk", "GUNK");

			// Token: 0x04009A53 RID: 39507
			public static LocString DESC = "Gunk is the built-up grime and grit produced by Duplicants' bionic mechanisms that has been frozen into a a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x0200220C RID: 8716
		public class SOLIDNUCLEARWASTE
		{
			// Token: 0x04009A54 RID: 39508
			public static LocString NAME = UI.FormatAsLink("Solid Nuclear Waste", "SOLIDNUCLEARWASTE");

			// Token: 0x04009A55 RID: 39509
			public static LocString DESC = "Highly toxic solid full of " + UI.FormatAsLink("Radioactive Contaminants", "RADIATION") + ".";
		}

		// Token: 0x0200220D RID: 8717
		public class HELIUM
		{
			// Token: 0x04009A56 RID: 39510
			public static LocString NAME = UI.FormatAsLink("Helium", "HELIUM");

			// Token: 0x04009A57 RID: 39511
			public static LocString DESC = "(He) Helium is an atomically lightweight, chemical " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".";
		}

		// Token: 0x0200220E RID: 8718
		public class HYDROGEN
		{
			// Token: 0x04009A58 RID: 39512
			public static LocString NAME = UI.FormatAsLink("Hydrogen Gas", "HYDROGEN");

			// Token: 0x04009A59 RID: 39513
			public static LocString DESC = "(H) Hydrogen Gas is the universe's most common and atomically light element in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x0200220F RID: 8719
		public class ICE
		{
			// Token: 0x04009A5A RID: 39514
			public static LocString NAME = UI.FormatAsLink("Ice", "ICE");

			// Token: 0x04009A5B RID: 39515
			public static LocString DESC = "(H<sub>2</sub>O) Ice is clean water frozen into a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02002210 RID: 8720
		public class IGNEOUSROCK
		{
			// Token: 0x04009A5C RID: 39516
			public static LocString NAME = UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK");

			// Token: 0x04009A5D RID: 39517
			public static LocString DESC = "Igneous Rock is a composite of solidified volcanic rock.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02002211 RID: 8721
		public class ISORESIN
		{
			// Token: 0x04009A5E RID: 39518
			public static LocString NAME = UI.FormatAsLink("Isoresin", "ISORESIN");

			// Token: 0x04009A5F RID: 39519
			public static LocString DESC = "Isoresin is a crystallized sap composed of long-chain polymers.\n\nIt is used in the production of rare, high grade materials.";
		}

		// Token: 0x02002212 RID: 8722
		public class RESIN
		{
			// Token: 0x04009A60 RID: 39520
			public static LocString NAME = UI.FormatAsLink("Liquid Resin", "RESIN");

			// Token: 0x04009A61 RID: 39521
			public static LocString DESC = "Sticky goo harvested from a grumpy tree.\n\nIt can be polymerized into " + UI.FormatAsLink("Isoresin", "ISORESIN") + " by boiling away its excess moisture.";
		}

		// Token: 0x02002213 RID: 8723
		public class SOLIDRESIN
		{
			// Token: 0x04009A62 RID: 39522
			public static LocString NAME = UI.FormatAsLink("Solid Resin", "SOLIDRESIN");

			// Token: 0x04009A63 RID: 39523
			public static LocString DESC = "Solidified goo harvested from a grumpy tree.\n\nIt is used in the production of " + UI.FormatAsLink("Isoresin", "ISORESIN") + ".";
		}

		// Token: 0x02002214 RID: 8724
		public class IRON
		{
			// Token: 0x04009A64 RID: 39524
			public static LocString NAME = UI.FormatAsLink("Iron", "IRON");

			// Token: 0x04009A65 RID: 39525
			public static LocString DESC = "(Fe) Iron is a common industrial " + UI.FormatAsLink("Metal", "RAWMETAL") + ".";
		}

		// Token: 0x02002215 RID: 8725
		public class IRONGAS
		{
			// Token: 0x04009A66 RID: 39526
			public static LocString NAME = UI.FormatAsLink("Iron Gas", "IRONGAS");

			// Token: 0x04009A67 RID: 39527
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Iron Gas is a common industrial ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x02002216 RID: 8726
		public class IRONORE
		{
			// Token: 0x04009A68 RID: 39528
			public static LocString NAME = UI.FormatAsLink("Iron Ore", "IRONORE");

			// Token: 0x04009A69 RID: 39529
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Iron Ore is a soft ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02002217 RID: 8727
		public class COBALTGAS
		{
			// Token: 0x04009A6A RID: 39530
			public static LocString NAME = UI.FormatAsLink("Cobalt Gas", "COBALTGAS");

			// Token: 0x04009A6B RID: 39531
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x02002218 RID: 8728
		public class COBALT
		{
			// Token: 0x04009A6C RID: 39532
			public static LocString NAME = UI.FormatAsLink("Cobalt", "COBALT");

			// Token: 0x04009A6D RID: 39533
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" made from ",
				UI.FormatAsLink("Cobalt Ore", "COBALTITE"),
				"."
			});
		}

		// Token: 0x02002219 RID: 8729
		public class COBALTITE
		{
			// Token: 0x04009A6E RID: 39534
			public static LocString NAME = UI.FormatAsLink("Cobalt Ore", "COBALTITE");

			// Token: 0x04009A6F RID: 39535
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt Ore is a blue-hued ",
				UI.FormatAsLink("Metal", "BUILDINGMATERIALCLASSES"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x0200221A RID: 8730
		public class KATAIRITE
		{
			// Token: 0x04009A70 RID: 39536
			public static LocString NAME = UI.FormatAsLink("Abyssalite", "KATAIRITE");

			// Token: 0x04009A71 RID: 39537
			public static LocString DESC = "(Ab) Abyssalite is a resilient, crystalline element.";
		}

		// Token: 0x0200221B RID: 8731
		public class LIME
		{
			// Token: 0x04009A72 RID: 39538
			public static LocString NAME = UI.FormatAsLink("Lime", "LIME");

			// Token: 0x04009A73 RID: 39539
			public static LocString DESC = "(CaCO<sub>3</sub>) Lime is a mineral commonly found in " + UI.FormatAsLink("Critter", "CREATURES") + " egg shells.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x0200221C RID: 8732
		public class FOSSIL
		{
			// Token: 0x04009A74 RID: 39540
			public static LocString NAME = UI.FormatAsLink("Fossil", "FOSSIL");

			// Token: 0x04009A75 RID: 39541
			public static LocString DESC = "Fossil is organic matter, highly compressed and hardened into a mineral state.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x0200221D RID: 8733
		public class LEADGAS
		{
			// Token: 0x04009A76 RID: 39542
			public static LocString NAME = UI.FormatAsLink("Lead Gas", "LEADGAS");

			// Token: 0x04009A77 RID: 39543
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead Gas is a soft yet extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x0200221E RID: 8734
		public class LEAD
		{
			// Token: 0x04009A78 RID: 39544
			public static LocString NAME = UI.FormatAsLink("Lead", "LEAD");

			// Token: 0x04009A79 RID: 39545
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead is a soft yet extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				".\n\nIt has a low Overheat Temperature and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x0200221F RID: 8735
		public class LIQUIDCARBONDIOXIDE
		{
			// Token: 0x04009A7A RID: 39546
			public static LocString NAME = UI.FormatAsLink("Liquid Carbon Dioxide", "LIQUIDCARBONDIOXIDE");

			// Token: 0x04009A7B RID: 39547
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an unbreathable chemical compound.\n\nThis selection is currently in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002220 RID: 8736
		public class LIQUIDHELIUM
		{
			// Token: 0x04009A7C RID: 39548
			public static LocString NAME = UI.FormatAsLink("Helium", "LIQUIDHELIUM");

			// Token: 0x04009A7D RID: 39549
			public static LocString DESC = "(He) Helium is an atomically lightweight chemical element cooled into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002221 RID: 8737
		public class LIQUIDHYDROGEN
		{
			// Token: 0x04009A7E RID: 39550
			public static LocString NAME = UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN");

			// Token: 0x04009A7F RID: 39551
			public static LocString DESC = "(H) Hydrogen in its " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.\n\nIt freezes most substances that come into contact with it.";
		}

		// Token: 0x02002222 RID: 8738
		public class LIQUIDOXYGEN
		{
			// Token: 0x04009A80 RID: 39552
			public static LocString NAME = UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN");

			// Token: 0x04009A81 RID: 39553
			public static LocString DESC = "(O<sub>2</sub>) Oxygen is a breathable chemical.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002223 RID: 8739
		public class LIQUIDMETHANE
		{
			// Token: 0x04009A82 RID: 39554
			public static LocString NAME = UI.FormatAsLink("Liquid Methane", "LIQUIDMETHANE");

			// Token: 0x04009A83 RID: 39555
			public static LocString DESC = "(CH<sub>4</sub>) Methane is an alkane.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002224 RID: 8740
		public class LIQUIDPHOSPHORUS
		{
			// Token: 0x04009A84 RID: 39556
			public static LocString NAME = UI.FormatAsLink("Liquid Phosphorus", "LIQUIDPHOSPHORUS");

			// Token: 0x04009A85 RID: 39557
			public static LocString DESC = "(P) Phosphorus is a chemical element.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002225 RID: 8741
		public class LIQUIDPROPANE
		{
			// Token: 0x04009A86 RID: 39558
			public static LocString NAME = UI.FormatAsLink("Liquid Propane", "LIQUIDPROPANE");

			// Token: 0x04009A87 RID: 39559
			public static LocString DESC = string.Concat(new string[]
			{
				"(C<sub>3</sub>H<sub>8</sub>) Propane is an alkane.\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02002226 RID: 8742
		public class LIQUIDSULFUR
		{
			// Token: 0x04009A88 RID: 39560
			public static LocString NAME = UI.FormatAsLink("Liquid Sulfur", "LIQUIDSULFUR");

			// Token: 0x04009A89 RID: 39561
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002227 RID: 8743
		public class MAFICROCK
		{
			// Token: 0x04009A8A RID: 39562
			public static LocString NAME = UI.FormatAsLink("Mafic Rock", "MAFICROCK");

			// Token: 0x04009A8B RID: 39563
			public static LocString DESC = string.Concat(new string[]
			{
				"Mafic Rock is a variation of ",
				UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
				" that is rich in ",
				UI.FormatAsLink("Iron", "IRON"),
				".\n\nIt is useful as a <b>Construction Material</b>."
			});
		}

		// Token: 0x02002228 RID: 8744
		public class MAGMA
		{
			// Token: 0x04009A8C RID: 39564
			public static LocString NAME = UI.FormatAsLink("Magma", "MAGMA");

			// Token: 0x04009A8D RID: 39565
			public static LocString DESC = string.Concat(new string[]
			{
				"Magma is a composite of ",
				UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
				" heated into a molten, ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002229 RID: 8745
		public class WOODLOG
		{
			// Token: 0x04009A8E RID: 39566
			public static LocString NAME = UI.FormatAsLink("Wood", "WOOD");

			// Token: 0x04009A8F RID: 39567
			public static LocString DESC = string.Concat(new string[]
			{
				"Wood is a good source of ",
				UI.FormatAsLink("Heat", "HEAT"),
				" and ",
				UI.FormatAsLink("Power", "POWER"),
				".\n\nIts insulation properties and positive ",
				UI.FormatAsLink("Decor", "DECOR"),
				" also make it a useful <b>Construction Material</b>."
			});
		}

		// Token: 0x0200222A RID: 8746
		public class CINNABAR
		{
			// Token: 0x04009A90 RID: 39568
			public static LocString NAME = UI.FormatAsLink("Cinnabar Ore", "CINNABAR");

			// Token: 0x04009A91 RID: 39569
			public static LocString DESC = string.Concat(new string[]
			{
				"(HgS) Cinnabar Ore, also known as mercury sulfide, is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" that can be refined into ",
				UI.FormatAsLink("Mercury", "MERCURY"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x0200222B RID: 8747
		public class TALLOW
		{
			// Token: 0x04009A92 RID: 39570
			public static LocString NAME = UI.FormatAsLink("Tallow", "TALLOW");

			// Token: 0x04009A93 RID: 39571
			public static LocString DESC = "A chunk of uncooked grease from a deceased " + CREATURES.SPECIES.SEAL.NAME + ".";
		}

		// Token: 0x0200222C RID: 8748
		public class MERCURY
		{
			// Token: 0x04009A94 RID: 39572
			public static LocString NAME = UI.FormatAsLink("Mercury", "MERCURY");

			// Token: 0x04009A95 RID: 39573
			public static LocString DESC = "(Hg) Mercury is a metallic " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".";
		}

		// Token: 0x0200222D RID: 8749
		public class MERCURYGAS
		{
			// Token: 0x04009A96 RID: 39574
			public static LocString NAME = UI.FormatAsLink("Mercury Gas", "MERCURYGAS");

			// Token: 0x04009A97 RID: 39575
			public static LocString DESC = string.Concat(new string[]
			{
				"(Hg) Mercury Gas is a ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x0200222E RID: 8750
		public class METHANE
		{
			// Token: 0x04009A98 RID: 39576
			public static LocString NAME = UI.FormatAsLink("Natural Gas", "METHANE");

			// Token: 0x04009A99 RID: 39577
			public static LocString DESC = string.Concat(new string[]
			{
				"Natural Gas is a mixture of various alkanes in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x0200222F RID: 8751
		public class MILK
		{
			// Token: 0x04009A9A RID: 39578
			public static LocString NAME = UI.FormatAsLink("Brackene", "MILK");

			// Token: 0x04009A9B RID: 39579
			public static LocString DESC = string.Concat(new string[]
			{
				"Brackene is a sodium-rich ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				".\n\nIt is useful in ",
				UI.FormatAsLink("Ranching", "RANCHING"),
				"."
			});
		}

		// Token: 0x02002230 RID: 8752
		public class MILKFAT
		{
			// Token: 0x04009A9C RID: 39580
			public static LocString NAME = UI.FormatAsLink("Brackwax", "MILKFAT");

			// Token: 0x04009A9D RID: 39581
			public static LocString DESC = string.Concat(new string[]
			{
				"Brackwax is a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" byproduct of ",
				UI.FormatAsLink("Brackene", "MILK"),
				"."
			});
		}

		// Token: 0x02002231 RID: 8753
		public class MOLTENCARBON
		{
			// Token: 0x04009A9E RID: 39582
			public static LocString NAME = UI.FormatAsLink("Liquid Carbon", "MOLTENCARBON");

			// Token: 0x04009A9F RID: 39583
			public static LocString DESC = "(C) Liquid Carbon is an abundant, versatile element heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002232 RID: 8754
		public class MOLTENCOPPER
		{
			// Token: 0x04009AA0 RID: 39584
			public static LocString NAME = UI.FormatAsLink("Molten Copper", "MOLTENCOPPER");

			// Token: 0x04009AA1 RID: 39585
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Molten Copper is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002233 RID: 8755
		public class MOLTENGLASS
		{
			// Token: 0x04009AA2 RID: 39586
			public static LocString NAME = UI.FormatAsLink("Molten Glass", "MOLTENGLASS");

			// Token: 0x04009AA3 RID: 39587
			public static LocString DESC = "Molten Glass is a composite of granular rock, heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002234 RID: 8756
		public class MOLTENGOLD
		{
			// Token: 0x04009AA4 RID: 39588
			public static LocString NAME = UI.FormatAsLink("Molten Gold", "MOLTENGOLD");

			// Token: 0x04009AA5 RID: 39589
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold, a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002235 RID: 8757
		public class MOLTENIRON
		{
			// Token: 0x04009AA6 RID: 39590
			public static LocString NAME = UI.FormatAsLink("Molten Iron", "MOLTENIRON");

			// Token: 0x04009AA7 RID: 39591
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Molten Iron is a common industrial ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002236 RID: 8758
		public class MOLTENCOBALT
		{
			// Token: 0x04009AA8 RID: 39592
			public static LocString NAME = UI.FormatAsLink("Molten Cobalt", "MOLTENCOBALT");

			// Token: 0x04009AA9 RID: 39593
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Molten Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002237 RID: 8759
		public class MOLTENLEAD
		{
			// Token: 0x04009AAA RID: 39594
			public static LocString NAME = UI.FormatAsLink("Molten Lead", "MOLTENLEAD");

			// Token: 0x04009AAB RID: 39595
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead is an extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02002238 RID: 8760
		public class MOLTENNIOBIUM
		{
			// Token: 0x04009AAC RID: 39596
			public static LocString NAME = UI.FormatAsLink("Molten Niobium", "MOLTENNIOBIUM");

			// Token: 0x04009AAD RID: 39597
			public static LocString DESC = "(Nb) Molten Niobium is a rare metal heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02002239 RID: 8761
		public class MOLTENTUNGSTEN
		{
			// Token: 0x04009AAE RID: 39598
			public static LocString NAME = UI.FormatAsLink("Molten Tungsten", "MOLTENTUNGSTEN");

			// Token: 0x04009AAF RID: 39599
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Molten Tungsten is a crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x0200223A RID: 8762
		public class MOLTENTUNGSTENDISELENIDE
		{
			// Token: 0x04009AB0 RID: 39600
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide", "MOLTENTUNGSTENDISELENIDE");

			// Token: 0x04009AB1 RID: 39601
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide is an inorganic ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x0200223B RID: 8763
		public class MOLTENSTEEL
		{
			// Token: 0x04009AB2 RID: 39602
			public static LocString NAME = UI.FormatAsLink("Molten Steel", "MOLTENSTEEL");

			// Token: 0x04009AB3 RID: 39603
			public static LocString DESC = string.Concat(new string[]
			{
				"Molten Steel is a ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" alloy of iron and carbon, heated into a hazardous ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x0200223C RID: 8764
		public class MOLTENURANIUM
		{
			// Token: 0x04009AB4 RID: 39604
			public static LocString NAME = UI.FormatAsLink("Liquid Uranium", "MOLTENURANIUM");

			// Token: 0x04009AB5 RID: 39605
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Liquid Uranium is a highly ",
				UI.FormatAsLink("Radioactive", "RADIATION"),
				" substance, heated into a hazardous ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state.\n\nIt is a byproduct of ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				"."
			});
		}

		// Token: 0x0200223D RID: 8765
		public class NIOBIUM
		{
			// Token: 0x04009AB6 RID: 39606
			public static LocString NAME = UI.FormatAsLink("Niobium", "NIOBIUM");

			// Token: 0x04009AB7 RID: 39607
			public static LocString DESC = "(Nb) Niobium is a rare metal with many practical applications in metallurgy and superconductor " + UI.FormatAsLink("Research", "RESEARCH") + ".";
		}

		// Token: 0x0200223E RID: 8766
		public class NIOBIUMGAS
		{
			// Token: 0x04009AB8 RID: 39608
			public static LocString NAME = UI.FormatAsLink("Niobium Gas", "NIOBIUMGAS");

			// Token: 0x04009AB9 RID: 39609
			public static LocString DESC = "(Nb) Niobium Gas is a rare metal.\n\nThis selection is in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x0200223F RID: 8767
		public class NUCLEARWASTE
		{
			// Token: 0x04009ABA RID: 39610
			public static LocString NAME = UI.FormatAsLink("Liquid Nuclear Waste", "NUCLEARWASTE");

			// Token: 0x04009ABB RID: 39611
			public static LocString DESC = string.Concat(new string[]
			{
				"Highly toxic liquid full of ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATION"),
				" which emit ",
				UI.FormatAsLink("Radiation", "RADIATION"),
				" that can be absorbed by ",
				UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER"),
				"."
			});
		}

		// Token: 0x02002240 RID: 8768
		public class OBSIDIAN
		{
			// Token: 0x04009ABC RID: 39612
			public static LocString NAME = UI.FormatAsLink("Obsidian", "OBSIDIAN");

			// Token: 0x04009ABD RID: 39613
			public static LocString DESC = "Obsidian is a brittle composite of volcanic " + UI.FormatAsLink("Glass", "GLASS") + ".";
		}

		// Token: 0x02002241 RID: 8769
		public class OXYGEN
		{
			// Token: 0x04009ABE RID: 39614
			public static LocString NAME = UI.FormatAsLink("Oxygen", "OXYGEN");

			// Token: 0x04009ABF RID: 39615
			public static LocString DESC = "(O<sub>2</sub>) Oxygen is an atomically lightweight and breathable " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ", necessary for sustaining life.\n\nIt tends to rise above other gases.";
		}

		// Token: 0x02002242 RID: 8770
		public class OXYROCK
		{
			// Token: 0x04009AC0 RID: 39616
			public static LocString NAME = UI.FormatAsLink("Oxylite", "OXYROCK");

			// Token: 0x04009AC1 RID: 39617
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ir<sub>3</sub>O<sub>2</sub>) Oxylite is a chemical compound that slowly emits breathable ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				".\n\nExcavating ",
				ELEMENTS.OXYROCK.NAME,
				" increases its emission rate, but depletes the ore more rapidly."
			});
		}

		// Token: 0x02002243 RID: 8771
		public class PHOSPHATENODULES
		{
			// Token: 0x04009AC2 RID: 39618
			public static LocString NAME = UI.FormatAsLink("Phosphate Nodules", "PHOSPHATENODULES");

			// Token: 0x04009AC3 RID: 39619
			public static LocString DESC = "(PO<sup>3-</sup><sub>4</sub>) Nodules of sedimentary rock containing high concentrations of phosphate.";
		}

		// Token: 0x02002244 RID: 8772
		public class PHOSPHORITE
		{
			// Token: 0x04009AC4 RID: 39620
			public static LocString NAME = UI.FormatAsLink("Phosphorite", "PHOSPHORITE");

			// Token: 0x04009AC5 RID: 39621
			public static LocString DESC = "Phosphorite is a composite of sedimentary rock, saturated with phosphate.";
		}

		// Token: 0x02002245 RID: 8773
		public class PHOSPHORUS
		{
			// Token: 0x04009AC6 RID: 39622
			public static LocString NAME = UI.FormatAsLink("Refined Phosphorus", "PHOSPHORUS");

			// Token: 0x04009AC7 RID: 39623
			public static LocString DESC = "(P) Refined Phosphorus is a chemical element in its " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02002246 RID: 8774
		public class PHOSPHORUSGAS
		{
			// Token: 0x04009AC8 RID: 39624
			public static LocString NAME = UI.FormatAsLink("Phosphorus Gas", "PHOSPHORUSGAS");

			// Token: 0x04009AC9 RID: 39625
			public static LocString DESC = string.Concat(new string[]
			{
				"(P) Phosphorus Gas is the ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state of ",
				UI.FormatAsLink("Refined Phosphorus", "PHOSPHORUS"),
				"."
			});
		}

		// Token: 0x02002247 RID: 8775
		public class PROPANE
		{
			// Token: 0x04009ACA RID: 39626
			public static LocString NAME = UI.FormatAsLink("Propane Gas", "PROPANE");

			// Token: 0x04009ACB RID: 39627
			public static LocString DESC = string.Concat(new string[]
			{
				"(C<sub>3</sub>H<sub>8</sub>) Propane Gas is a natural alkane.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02002248 RID: 8776
		public class RADIUM
		{
			// Token: 0x04009ACC RID: 39628
			public static LocString NAME = UI.FormatAsLink("Radium", "RADIUM");

			// Token: 0x04009ACD RID: 39629
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ra) Radium is a ",
				UI.FormatAsLink("Light", "LIGHT"),
				" emitting radioactive substance.\n\nIt is useful as a ",
				UI.FormatAsLink("Power", "POWER"),
				" source."
			});
		}

		// Token: 0x02002249 RID: 8777
		public class YELLOWCAKE
		{
			// Token: 0x04009ACE RID: 39630
			public static LocString NAME = UI.FormatAsLink("Yellowcake", "YELLOWCAKE");

			// Token: 0x04009ACF RID: 39631
			public static LocString DESC = string.Concat(new string[]
			{
				"(U<sub>3</sub>O<sub>8</sub>) Yellowcake is a byproduct of ",
				UI.FormatAsLink("Uranium", "URANIUM"),
				" mining.\n\nIt is useful in preparing fuel for ",
				UI.FormatAsLink("Research Reactors", "NUCLEARREACTOR"),
				".\n\nNote: Do not eat."
			});
		}

		// Token: 0x0200224A RID: 8778
		public class ROCKGAS
		{
			// Token: 0x04009AD0 RID: 39632
			public static LocString NAME = UI.FormatAsLink("Rock Gas", "ROCKGAS");

			// Token: 0x04009AD1 RID: 39633
			public static LocString DESC = "Rock Gas is rock that has been superheated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x0200224B RID: 8779
		public class RUST
		{
			// Token: 0x04009AD2 RID: 39634
			public static LocString NAME = UI.FormatAsLink("Rust", "RUST");

			// Token: 0x04009AD3 RID: 39635
			public static LocString DESC = string.Concat(new string[]
			{
				"Rust is an iron oxide that forms from the breakdown of ",
				UI.FormatAsLink("Iron", "IRON"),
				".\n\nIt is useful in some ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				" production processes."
			});
		}

		// Token: 0x0200224C RID: 8780
		public class REGOLITH
		{
			// Token: 0x04009AD4 RID: 39636
			public static LocString NAME = UI.FormatAsLink("Regolith", "REGOLITH");

			// Token: 0x04009AD5 RID: 39637
			public static LocString DESC = "Regolith is a sandy substance composed of the various particles that collect atop terrestrial objects.\n\nIt is useful as a " + UI.FormatAsLink("Filtration Medium", "REGOLITH") + ".";
		}

		// Token: 0x0200224D RID: 8781
		public class SALTGAS
		{
			// Token: 0x04009AD6 RID: 39638
			public static LocString NAME = UI.FormatAsLink("Salt Gas", "SALTGAS");

			// Token: 0x04009AD7 RID: 39639
			public static LocString DESC = "(NaCl) Salt Gas is an edible chemical compound that has been superheated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x0200224E RID: 8782
		public class MOLTENSALT
		{
			// Token: 0x04009AD8 RID: 39640
			public static LocString NAME = UI.FormatAsLink("Molten Salt", "MOLTENSALT");

			// Token: 0x04009AD9 RID: 39641
			public static LocString DESC = "(NaCl) Molten Salt is an edible chemical compound that has been heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x0200224F RID: 8783
		public class SALT
		{
			// Token: 0x04009ADA RID: 39642
			public static LocString NAME = UI.FormatAsLink("Salt", "SALT");

			// Token: 0x04009ADB RID: 39643
			public static LocString DESC = "(NaCl) Salt, also known as sodium chloride, is an edible chemical compound.\n\nWhen refined, it can be eaten with meals to increase Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
		}

		// Token: 0x02002250 RID: 8784
		public class SALTWATER
		{
			// Token: 0x04009ADC RID: 39644
			public static LocString NAME = UI.FormatAsLink("Salt Water", "SALTWATER");

			// Token: 0x04009ADD RID: 39645
			public static LocString DESC = string.Concat(new string[]
			{
				"Salt Water is a natural, lightly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x02002251 RID: 8785
		public class SAND
		{
			// Token: 0x04009ADE RID: 39646
			public static LocString NAME = UI.FormatAsLink("Sand", "SAND");

			// Token: 0x04009ADF RID: 39647
			public static LocString DESC = "Sand is a composite of granular rock.\n\nIt is useful as a " + UI.FormatAsLink("Filtration Medium", "FILTER") + ".";
		}

		// Token: 0x02002252 RID: 8786
		public class SANDCEMENT
		{
			// Token: 0x04009AE0 RID: 39648
			public static LocString NAME = UI.FormatAsLink("Sand Cement", "SANDCEMENT");

			// Token: 0x04009AE1 RID: 39649
			public static LocString DESC = "";
		}

		// Token: 0x02002253 RID: 8787
		public class SANDSTONE
		{
			// Token: 0x04009AE2 RID: 39650
			public static LocString NAME = UI.FormatAsLink("Sandstone", "SANDSTONE");

			// Token: 0x04009AE3 RID: 39651
			public static LocString DESC = "Sandstone is a composite of relatively soft sedimentary rock.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02002254 RID: 8788
		public class SEDIMENTARYROCK
		{
			// Token: 0x04009AE4 RID: 39652
			public static LocString NAME = UI.FormatAsLink("Sedimentary Rock", "SEDIMENTARYROCK");

			// Token: 0x04009AE5 RID: 39653
			public static LocString DESC = "Sedimentary Rock is a hardened composite of sediment layers.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02002255 RID: 8789
		public class SLIMEMOLD
		{
			// Token: 0x04009AE6 RID: 39654
			public static LocString NAME = UI.FormatAsLink("Slime", "SLIMEMOLD");

			// Token: 0x04009AE7 RID: 39655
			public static LocString DESC = string.Concat(new string[]
			{
				"Slime is a thick biomixture of algae, fungi, and mucopolysaccharides.\n\nIt can be distilled into ",
				UI.FormatAsLink("Algae", "ALGAE"),
				" and emits ",
				ELEMENTS.CONTAMINATEDOXYGEN.NAME,
				" once dug up."
			});
		}

		// Token: 0x02002256 RID: 8790
		public class SNOW
		{
			// Token: 0x04009AE8 RID: 39656
			public static LocString NAME = UI.FormatAsLink("Snow", "SNOW");

			// Token: 0x04009AE9 RID: 39657
			public static LocString DESC = "(H<sub>2</sub>O) Snow is a mass of loose, crystalline ice particles.\n\nIt becomes " + UI.FormatAsLink("Water", "WATER") + " when melted.";
		}

		// Token: 0x02002257 RID: 8791
		public class STABLESNOW
		{
			// Token: 0x04009AEA RID: 39658
			public static LocString NAME = "Packed " + ELEMENTS.SNOW.NAME;

			// Token: 0x04009AEB RID: 39659
			public static LocString DESC = ELEMENTS.SNOW.DESC;
		}

		// Token: 0x02002258 RID: 8792
		public class SOLIDCARBONDIOXIDE
		{
			// Token: 0x04009AEC RID: 39660
			public static LocString NAME = UI.FormatAsLink("Solid Carbon Dioxide", "SOLIDCARBONDIOXIDE");

			// Token: 0x04009AED RID: 39661
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an unbreathable compound in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02002259 RID: 8793
		public class SOLIDCHLORINE
		{
			// Token: 0x04009AEE RID: 39662
			public static LocString NAME = UI.FormatAsLink("Solid Chlorine", "SOLIDCHLORINE");

			// Token: 0x04009AEF RID: 39663
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x0200225A RID: 8794
		public class SOLIDCRUDEOIL
		{
			// Token: 0x04009AF0 RID: 39664
			public static LocString NAME = UI.FormatAsLink("Solid Crude Oil", "SOLIDCRUDEOIL");

			// Token: 0x04009AF1 RID: 39665
			public static LocString DESC = "";
		}

		// Token: 0x0200225B RID: 8795
		public class SOLIDHYDROGEN
		{
			// Token: 0x04009AF2 RID: 39666
			public static LocString NAME = UI.FormatAsLink("Solid Hydrogen", "SOLIDHYDROGEN");

			// Token: 0x04009AF3 RID: 39667
			public static LocString DESC = "(H) Solid Hydrogen is the universe's most common element in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x0200225C RID: 8796
		public class SOLIDMERCURY
		{
			// Token: 0x04009AF4 RID: 39668
			public static LocString NAME = UI.FormatAsLink("Mercury", "SOLIDMERCURY");

			// Token: 0x04009AF5 RID: 39669
			public static LocString DESC = string.Concat(new string[]
			{
				"(Hg) Mercury is a rare ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x0200225D RID: 8797
		public class SOLIDOXYGEN
		{
			// Token: 0x04009AF6 RID: 39670
			public static LocString NAME = UI.FormatAsLink("Solid Oxygen", "SOLIDOXYGEN");

			// Token: 0x04009AF7 RID: 39671
			public static LocString DESC = "(O<sub>2</sub>) Solid Oxygen is a breathable element in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x0200225E RID: 8798
		public class SOLIDMETHANE
		{
			// Token: 0x04009AF8 RID: 39672
			public static LocString NAME = UI.FormatAsLink("Solid Methane", "SOLIDMETHANE");

			// Token: 0x04009AF9 RID: 39673
			public static LocString DESC = "(CH<sub>4</sub>) Methane is an alkane in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x0200225F RID: 8799
		public class SOLIDNAPHTHA
		{
			// Token: 0x04009AFA RID: 39674
			public static LocString NAME = UI.FormatAsLink("Solid Naphtha", "SOLIDNAPHTHA");

			// Token: 0x04009AFB RID: 39675
			public static LocString DESC = "Naphtha is a distilled hydrocarbon mixture in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02002260 RID: 8800
		public class CORIUM
		{
			// Token: 0x04009AFC RID: 39676
			public static LocString NAME = UI.FormatAsLink("Corium", "CORIUM");

			// Token: 0x04009AFD RID: 39677
			public static LocString DESC = "A radioactive mixture of nuclear waste and melted reactor materials.\n\nReleases " + UI.FormatAsLink("Nuclear Fallout", "FALLOUT") + " gas.";
		}

		// Token: 0x02002261 RID: 8801
		public class SOLIDPETROLEUM
		{
			// Token: 0x04009AFE RID: 39678
			public static LocString NAME = UI.FormatAsLink("Solid Petroleum", "SOLIDPETROLEUM");

			// Token: 0x04009AFF RID: 39679
			public static LocString DESC = string.Concat(new string[]
			{
				"Petroleum is a ",
				UI.FormatAsLink("Power", "POWER"),
				" source.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02002262 RID: 8802
		public class SOLIDPROPANE
		{
			// Token: 0x04009B00 RID: 39680
			public static LocString NAME = UI.FormatAsLink("Solid Propane", "SOLIDPROPANE");

			// Token: 0x04009B01 RID: 39681
			public static LocString DESC = "(C<sub>3</sub>H<sub>8</sub>) Solid Propane is a natural gas in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02002263 RID: 8803
		public class SOLIDSUPERCOOLANT
		{
			// Token: 0x04009B02 RID: 39682
			public static LocString NAME = UI.FormatAsLink("Solid Super Coolant", "SOLIDSUPERCOOLANT");

			// Token: 0x04009B03 RID: 39683
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				" coolant.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02002264 RID: 8804
		public class SOLIDVISCOGEL
		{
			// Token: 0x04009B04 RID: 39684
			public static LocString NAME = UI.FormatAsLink("Solid Visco-Gel", "SOLIDVISCOGEL");

			// Token: 0x04009B05 RID: 39685
			public static LocString DESC = string.Concat(new string[]
			{
				"Visco-Gel is a polymer that has high surface tension when in ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" form.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02002265 RID: 8805
		public class SYNGAS
		{
			// Token: 0x04009B06 RID: 39686
			public static LocString NAME = UI.FormatAsLink("Synthesis Gas", "SYNGAS");

			// Token: 0x04009B07 RID: 39687
			public static LocString DESC = "Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02002266 RID: 8806
		public class MOLTENSYNGAS
		{
			// Token: 0x04009B08 RID: 39688
			public static LocString NAME = UI.FormatAsLink("Molten Synthesis Gas", "SYNGAS");

			// Token: 0x04009B09 RID: 39689
			public static LocString DESC = "Molten Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02002267 RID: 8807
		public class SOLIDSYNGAS
		{
			// Token: 0x04009B0A RID: 39690
			public static LocString NAME = UI.FormatAsLink("Solid Synthesis Gas", "SYNGAS");

			// Token: 0x04009B0B RID: 39691
			public static LocString DESC = "Solid Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02002268 RID: 8808
		public class STEAM
		{
			// Token: 0x04009B0C RID: 39692
			public static LocString NAME = UI.FormatAsLink("Steam", "STEAM");

			// Token: 0x04009B0D RID: 39693
			public static LocString DESC = string.Concat(new string[]
			{
				"(H<sub>2</sub>O) Steam is ",
				ELEMENTS.WATER.NAME,
				" that has been heated into a scalding ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x02002269 RID: 8809
		public class STEEL
		{
			// Token: 0x04009B0E RID: 39694
			public static LocString NAME = UI.FormatAsLink("Steel", "STEEL");

			// Token: 0x04009B0F RID: 39695
			public static LocString DESC = "Steel is a " + UI.FormatAsLink("Metal Alloy", "REFINEDMETAL") + " composed of iron and carbon.";
		}

		// Token: 0x0200226A RID: 8810
		public class STEELGAS
		{
			// Token: 0x04009B10 RID: 39696
			public static LocString NAME = UI.FormatAsLink("Steel Gas", "STEELGAS");

			// Token: 0x04009B11 RID: 39697
			public static LocString DESC = string.Concat(new string[]
			{
				"Steel Gas is a superheated ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" composed of iron and carbon."
			});
		}

		// Token: 0x0200226B RID: 8811
		public class SUGARWATER
		{
			// Token: 0x04009B12 RID: 39698
			public static LocString NAME = UI.FormatAsLink("Nectar", "SUGARWATER");

			// Token: 0x04009B13 RID: 39699
			public static LocString DESC = string.Concat(new string[]
			{
				"Nectar is a natural, lightly concentrated solution of ",
				UI.FormatAsLink("Sucrose", "SUCROSE"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				"."
			});
		}

		// Token: 0x0200226C RID: 8812
		public class SULFUR
		{
			// Token: 0x04009B14 RID: 39700
			public static LocString NAME = UI.FormatAsLink("Sulfur", "SULFUR");

			// Token: 0x04009B15 RID: 39701
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x0200226D RID: 8813
		public class SULFURGAS
		{
			// Token: 0x04009B16 RID: 39702
			public static LocString NAME = UI.FormatAsLink("Sulfur Gas", "SULFURGAS");

			// Token: 0x04009B17 RID: 39703
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x0200226E RID: 8814
		public class SUPERCOOLANT
		{
			// Token: 0x04009B18 RID: 39704
			public static LocString NAME = UI.FormatAsLink("Super Coolant", "SUPERCOOLANT");

			// Token: 0x04009B19 RID: 39705
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade coolant that utilizes the unusual energy states of ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				".\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x0200226F RID: 8815
		public class SUPERCOOLANTGAS
		{
			// Token: 0x04009B1A RID: 39706
			public static LocString NAME = UI.FormatAsLink("Super Coolant Gas", "SUPERCOOLANTGAS");

			// Token: 0x04009B1B RID: 39707
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				" coolant.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02002270 RID: 8816
		public class SUPERINSULATOR
		{
			// Token: 0x04009B1C RID: 39708
			public static LocString NAME = UI.FormatAsLink("Insulite", "SUPERINSULATOR");

			// Token: 0x04009B1D RID: 39709
			public static LocString DESC = string.Concat(new string[]
			{
				"Insulite reduces ",
				UI.FormatAsLink("Heat Transfer", "HEAT"),
				" and is composed of recrystallized ",
				UI.FormatAsLink("Abyssalite", "KATAIRITE"),
				"."
			});
		}

		// Token: 0x02002271 RID: 8817
		public class TEMPCONDUCTORSOLID
		{
			// Token: 0x04009B1E RID: 39710
			public static LocString NAME = UI.FormatAsLink("Thermium", "TEMPCONDUCTORSOLID");

			// Token: 0x04009B1F RID: 39711
			public static LocString DESC = "Thermium is an industrial metal alloy formulated to maximize " + UI.FormatAsLink("Heat Transfer", "HEAT") + " and thermal dispersion.";
		}

		// Token: 0x02002272 RID: 8818
		public class TUNGSTEN
		{
			// Token: 0x04009B20 RID: 39712
			public static LocString NAME = UI.FormatAsLink("Tungsten", "TUNGSTEN");

			// Token: 0x04009B21 RID: 39713
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Tungsten is an extremely tough crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02002273 RID: 8819
		public class TUNGSTENGAS
		{
			// Token: 0x04009B22 RID: 39714
			public static LocString NAME = UI.FormatAsLink("Tungsten Gas", "TUNGSTENGAS");

			// Token: 0x04009B23 RID: 39715
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Tungsten is a superheated crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02002274 RID: 8820
		public class TUNGSTENDISELENIDE
		{
			// Token: 0x04009B24 RID: 39716
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide", "TUNGSTENDISELENIDE");

			// Token: 0x04009B25 RID: 39717
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide is an inorganic ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound with a crystalline structure.\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02002275 RID: 8821
		public class TUNGSTENDISELENIDEGAS
		{
			// Token: 0x04009B26 RID: 39718
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide Gas", "TUNGSTENDISELENIDEGAS");

			// Token: 0x04009B27 RID: 39719
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide Gasis a superheated ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02002276 RID: 8822
		public class TOXICSAND
		{
			// Token: 0x04009B28 RID: 39720
			public static LocString NAME = UI.FormatAsLink("Polluted Dirt", "TOXICSAND");

			// Token: 0x04009B29 RID: 39721
			public static LocString DESC = "Polluted Dirt is unprocessed biological waste.\n\nIt emits " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " over time.";
		}

		// Token: 0x02002277 RID: 8823
		public class UNOBTANIUM
		{
			// Token: 0x04009B2A RID: 39722
			public static LocString NAME = UI.FormatAsLink("Neutronium", "UNOBTANIUM");

			// Token: 0x04009B2B RID: 39723
			public static LocString DESC = "(Nt) Neutronium is a mysterious and extremely resilient element.\n\nIt cannot be excavated by any Duplicant mining tool.";
		}

		// Token: 0x02002278 RID: 8824
		public class URANIUMORE
		{
			// Token: 0x04009B2C RID: 39724
			public static LocString NAME = UI.FormatAsLink("Uranium Ore", "URANIUMORE");

			// Token: 0x04009B2D RID: 39725
			public static LocString DESC = "(U) Uranium Ore is a highly " + UI.FormatAsLink("Radioactive", "RADIATION") + " substance.\n\nIt can be refined into fuel for research reactors.";
		}

		// Token: 0x02002279 RID: 8825
		public class VACUUM
		{
			// Token: 0x04009B2E RID: 39726
			public static LocString NAME = UI.FormatAsLink("Vacuum", "VACUUM");

			// Token: 0x04009B2F RID: 39727
			public static LocString DESC = "A vacuum is a space devoid of all matter.";
		}

		// Token: 0x0200227A RID: 8826
		public class VISCOGEL
		{
			// Token: 0x04009B30 RID: 39728
			public static LocString NAME = UI.FormatAsLink("Visco-Gel Fluid", "VISCOGEL");

			// Token: 0x04009B31 RID: 39729
			public static LocString DESC = "Visco-Gel Fluid is a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " polymer with high surface tension, preventing typical liquid flow and allowing for unusual configurations.";
		}

		// Token: 0x0200227B RID: 8827
		public class VOID
		{
			// Token: 0x04009B32 RID: 39730
			public static LocString NAME = UI.FormatAsLink("Void", "VOID");

			// Token: 0x04009B33 RID: 39731
			public static LocString DESC = "Cold, infinite nothingness.";
		}

		// Token: 0x0200227C RID: 8828
		public class COMPOSITION
		{
			// Token: 0x04009B34 RID: 39732
			public static LocString NAME = UI.FormatAsLink("Composition", "COMPOSITION");

			// Token: 0x04009B35 RID: 39733
			public static LocString DESC = "A mixture of two or more elements.";
		}

		// Token: 0x0200227D RID: 8829
		public class WATER
		{
			// Token: 0x04009B36 RID: 39734
			public static LocString NAME = UI.FormatAsLink("Water", "WATER");

			// Token: 0x04009B37 RID: 39735
			public static LocString DESC = "(H<sub>2</sub>O) Clean " + UI.FormatAsLink("Water", "WATER") + ", suitable for consumption.";
		}

		// Token: 0x0200227E RID: 8830
		public class WOLFRAMITE
		{
			// Token: 0x04009B38 RID: 39736
			public static LocString NAME = UI.FormatAsLink("Wolframite", "WOLFRAMITE");

			// Token: 0x04009B39 RID: 39737
			public static LocString DESC = string.Concat(new string[]
			{
				"((Fe,Mn)WO<sub>4</sub>) Wolframite is a dense Metallic element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt is a source of ",
				UI.FormatAsLink("Tungsten", "TUNGSTEN"),
				" and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x0200227F RID: 8831
		public class TESTELEMENT
		{
			// Token: 0x04009B3A RID: 39738
			public static LocString NAME = UI.FormatAsLink("Test Element", "TESTELEMENT");

			// Token: 0x04009B3B RID: 39739
			public static LocString DESC = string.Concat(new string[]
			{
				"((Fe,Mn)WO<sub>4</sub>) Wolframite is a dense Metallic element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt is a source of ",
				UI.FormatAsLink("Tungsten", "TUNGSTEN"),
				" and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02002280 RID: 8832
		public class POLYPROPYLENE
		{
			// Token: 0x04009B3C RID: 39740
			public static LocString NAME = UI.FormatAsLink("Plastic", "POLYPROPYLENE");

			// Token: 0x04009B3D RID: 39741
			public static LocString DESC = "(C<sub>3</sub>H<sub>6</sub>)<sub>n</sub> " + ELEMENTS.POLYPROPYLENE.NAME + " is a thermoplastic polymer.\n\nIt is useful for constructing a variety of advanced buildings and equipment.";

			// Token: 0x04009B3E RID: 39742
			public static LocString BUILD_DESC = "Buildings made of this " + ELEMENTS.POLYPROPYLENE.NAME + " have antiseptic properties";
		}

		// Token: 0x02002281 RID: 8833
		public class HARDPOLYPROPYLENE
		{
			// Token: 0x04009B3F RID: 39743
			public static LocString NAME = UI.FormatAsLink("Plastium", "HARDPOLYPROPYLENE");

			// Token: 0x04009B40 RID: 39744
			public static LocString DESC = string.Concat(new string[]
			{
				ELEMENTS.HARDPOLYPROPYLENE.NAME,
				" is an advanced thermoplastic polymer made from ",
				UI.FormatAsLink("Thermium", "TEMPCONDUCTORSOLID"),
				", ",
				UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
				" and ",
				UI.FormatAsLink("Brackwax", "MILKFAT"),
				".\n\nIt is highly heat-resistant and suitable for use in space buildings."
			});
		}

		// Token: 0x02002282 RID: 8834
		public class NAPHTHA
		{
			// Token: 0x04009B41 RID: 39745
			public static LocString NAME = UI.FormatAsLink("Liquid Naphtha", "NAPHTHA");

			// Token: 0x04009B42 RID: 39746
			public static LocString DESC = "Naphtha a distilled hydrocarbon mixture produced from the burning of " + UI.FormatAsLink("Plastic", "POLYPROPYLENE") + ".";
		}

		// Token: 0x02002283 RID: 8835
		public class SLABS
		{
			// Token: 0x04009B43 RID: 39747
			public static LocString NAME = UI.FormatAsLink("Building Slab", "SLABS");

			// Token: 0x04009B44 RID: 39748
			public static LocString DESC = "Slabs are a refined mineral building block used for assembling advanced buildings.";
		}

		// Token: 0x02002284 RID: 8836
		public class TOXICMUD
		{
			// Token: 0x04009B45 RID: 39749
			public static LocString NAME = UI.FormatAsLink("Polluted Mud", "TOXICMUD");

			// Token: 0x04009B46 RID: 39750
			public static LocString DESC = string.Concat(new string[]
			{
				"A mixture of ",
				UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
				" and ",
				UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
				".\n\nCan be separated into its base elements using a ",
				UI.FormatAsLink("Sludge Press", "SLUDGEPRESS"),
				"."
			});
		}

		// Token: 0x02002285 RID: 8837
		public class MUD
		{
			// Token: 0x04009B47 RID: 39751
			public static LocString NAME = UI.FormatAsLink("Mud", "MUD");

			// Token: 0x04009B48 RID: 39752
			public static LocString DESC = string.Concat(new string[]
			{
				"A mixture of ",
				UI.FormatAsLink("Dirt", "DIRT"),
				" and ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nCan be separated into its base elements using a ",
				UI.FormatAsLink("Sludge Press", "SLUDGEPRESS"),
				"."
			});
		}

		// Token: 0x02002286 RID: 8838
		public class SUCROSE
		{
			// Token: 0x04009B49 RID: 39753
			public static LocString NAME = UI.FormatAsLink("Sucrose", "SUCROSE");

			// Token: 0x04009B4A RID: 39754
			public static LocString DESC = "(C<sub>12</sub>H<sub>22</sub>O<sub>11</sub>) Sucrose is the raw form of sugar.\n\nIt can be used for cooking higher-quality " + UI.FormatAsLink("Food", "FOOD") + ".";
		}

		// Token: 0x02002287 RID: 8839
		public class MOLTENSUCROSE
		{
			// Token: 0x04009B4B RID: 39755
			public static LocString NAME = UI.FormatAsLink("Liquid Sucrose", "MOLTENSUCROSE");

			// Token: 0x04009B4C RID: 39756
			public static LocString DESC = "(C<sub>12</sub>H<sub>22</sub>O<sub>11</sub>) Liquid Sucrose is the raw form of sugar, heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}
	}
}
