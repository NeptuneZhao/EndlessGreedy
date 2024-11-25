using System;

namespace STRINGS
{
	// Token: 0x02000F18 RID: 3864
	public class RESEARCH
	{
		// Token: 0x020021A7 RID: 8615
		public class MESSAGING
		{
			// Token: 0x04009978 RID: 39288
			public static LocString NORESEARCHSELECTED = "No research selected";

			// Token: 0x04009979 RID: 39289
			public static LocString RESEARCHTYPEREQUIRED = "{0} required";

			// Token: 0x0400997A RID: 39290
			public static LocString RESEARCHTYPEALSOREQUIRED = "{0} also required";

			// Token: 0x0400997B RID: 39291
			public static LocString NO_RESEARCHER_SKILL = "No Researchers assigned";

			// Token: 0x0400997C RID: 39292
			public static LocString NO_RESEARCHER_SKILL_TOOLTIP = "The selected research focus requires {ResearchType} to complete\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", global::Action.ManageSkills) + " and teach a Duplicant the {ResearchType} Skill to use this building";

			// Token: 0x0400997D RID: 39293
			public static LocString MISSING_RESEARCH_STATION = "Missing Research Station";

			// Token: 0x0400997E RID: 39294
			public static LocString MISSING_RESEARCH_STATION_TOOLTIP = "The selected research focus requires a {0} to perform\n\nOpen the " + UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10) + " of the Build Menu to construct one";

			// Token: 0x02002F5D RID: 12125
			public static class DLC
			{
				// Token: 0x0400C863 RID: 51299
				public static LocString EXPANSION1 = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"\n\n<i>",
					UI.DLC1.NAME,
					"</i>",
					UI.PST_KEYWORD,
					" DLC Content"
				});

				// Token: 0x0400C864 RID: 51300
				public static LocString DLC_CONTENT = "\n<i>{0}</i> DLC Content";
			}
		}

		// Token: 0x020021A8 RID: 8616
		public class TYPES
		{
			// Token: 0x0400997F RID: 39295
			public static LocString MISSINGRECIPEDESC = "Missing Recipe Description";

			// Token: 0x02002F5E RID: 12126
			public class ALPHA
			{
				// Token: 0x0400C865 RID: 51301
				public static LocString NAME = "Novice Research";

				// Token: 0x0400C866 RID: 51302
				public static LocString DESC = UI.FormatAsLink("Novice Research", "RESEARCH") + " is required to unlock basic technologies.\nIt can be conducted at a " + UI.FormatAsLink("Research Station", "RESEARCHCENTER") + ".";

				// Token: 0x0400C867 RID: 51303
				public static LocString RECIPEDESC = "Unlocks rudimentary technologies.";
			}

			// Token: 0x02002F5F RID: 12127
			public class BETA
			{
				// Token: 0x0400C868 RID: 51304
				public static LocString NAME = "Advanced Research";

				// Token: 0x0400C869 RID: 51305
				public static LocString DESC = UI.FormatAsLink("Advanced Research", "RESEARCH") + " is required to unlock improved technologies.\nIt can be conducted at a " + UI.FormatAsLink("Super Computer", "ADVANCEDRESEARCHCENTER") + ".";

				// Token: 0x0400C86A RID: 51306
				public static LocString RECIPEDESC = "Unlocks improved technologies.";
			}

			// Token: 0x02002F60 RID: 12128
			public class GAMMA
			{
				// Token: 0x0400C86B RID: 51307
				public static LocString NAME = "Interstellar Research";

				// Token: 0x0400C86C RID: 51308
				public static LocString DESC = UI.FormatAsLink("Interstellar Research", "RESEARCH") + " is required to unlock space technologies.\nIt can be conducted at a " + UI.FormatAsLink("Virtual Planetarium", "COSMICRESEARCHCENTER") + ".";

				// Token: 0x0400C86D RID: 51309
				public static LocString RECIPEDESC = "Unlocks cutting-edge technologies.";
			}

			// Token: 0x02002F61 RID: 12129
			public class DELTA
			{
				// Token: 0x0400C86E RID: 51310
				public static LocString NAME = "Applied Sciences Research";

				// Token: 0x0400C86F RID: 51311
				public static LocString DESC = UI.FormatAsLink("Applied Sciences Research", "RESEARCH") + " is required to unlock materials science technologies.\nIt can be conducted at a " + UI.FormatAsLink("Materials Study Terminal", "NUCLEARRESEARCHCENTER") + ".";

				// Token: 0x0400C870 RID: 51312
				public static LocString RECIPEDESC = "Unlocks next wave technologies.";
			}

			// Token: 0x02002F62 RID: 12130
			public class ORBITAL
			{
				// Token: 0x0400C871 RID: 51313
				public static LocString NAME = "Data Analysis Research";

				// Token: 0x0400C872 RID: 51314
				public static LocString DESC = UI.FormatAsLink("Data Analysis Research", "RESEARCH") + " is required to unlock Data Analysis technologies.\nIt can be conducted at a " + UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER") + ".";

				// Token: 0x0400C873 RID: 51315
				public static LocString RECIPEDESC = "Unlocks out-of-this-world technologies.";
			}
		}

		// Token: 0x020021A9 RID: 8617
		public class OTHER_TECH_ITEMS
		{
			// Token: 0x02002F63 RID: 12131
			public class AUTOMATION_OVERLAY
			{
				// Token: 0x0400C874 RID: 51316
				public static LocString NAME = UI.FormatAsOverlay("Automation Overlay");

				// Token: 0x0400C875 RID: 51317
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Automation Overlay") + ".";
			}

			// Token: 0x02002F64 RID: 12132
			public class SUITS_OVERLAY
			{
				// Token: 0x0400C876 RID: 51318
				public static LocString NAME = UI.FormatAsOverlay("Exosuit Overlay");

				// Token: 0x0400C877 RID: 51319
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Exosuit Overlay") + ".";
			}

			// Token: 0x02002F65 RID: 12133
			public class JET_SUIT
			{
				// Token: 0x0400C878 RID: 51320
				public static LocString NAME = UI.PRE_KEYWORD + "Jet Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C879 RID: 51321
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Jet Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x02002F66 RID: 12134
			public class OXYGEN_MASK
			{
				// Token: 0x0400C87A RID: 51322
				public static LocString NAME = UI.PRE_KEYWORD + "Oxygen Mask" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C87B RID: 51323
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Oxygen Masks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F67 RID: 12135
			public class LEAD_SUIT
			{
				// Token: 0x0400C87C RID: 51324
				public static LocString NAME = UI.PRE_KEYWORD + "Lead Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C87D RID: 51325
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Lead Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x02002F68 RID: 12136
			public class ATMO_SUIT
			{
				// Token: 0x0400C87E RID: 51326
				public static LocString NAME = UI.PRE_KEYWORD + "Atmo Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C87F RID: 51327
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Atmo Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x02002F69 RID: 12137
			public class BETA_RESEARCH_POINT
			{
				// Token: 0x0400C880 RID: 51328
				public static LocString NAME = UI.PRE_KEYWORD + "Advanced Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400C881 RID: 51329
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Advanced Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02002F6A RID: 12138
			public class GAMMA_RESEARCH_POINT
			{
				// Token: 0x0400C882 RID: 51330
				public static LocString NAME = UI.PRE_KEYWORD + "Interstellar Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400C883 RID: 51331
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Interstellar Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02002F6B RID: 12139
			public class DELTA_RESEARCH_POINT
			{
				// Token: 0x0400C884 RID: 51332
				public static LocString NAME = UI.PRE_KEYWORD + "Materials Science Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400C885 RID: 51333
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Materials Science Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02002F6C RID: 12140
			public class ORBITAL_RESEARCH_POINT
			{
				// Token: 0x0400C886 RID: 51334
				public static LocString NAME = UI.PRE_KEYWORD + "Data Analysis Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400C887 RID: 51335
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Data Analysis Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02002F6D RID: 12141
			public class CONVEYOR_OVERLAY
			{
				// Token: 0x0400C888 RID: 51336
				public static LocString NAME = UI.FormatAsOverlay("Conveyor Overlay");

				// Token: 0x0400C889 RID: 51337
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Conveyor Overlay") + ".";
			}

			// Token: 0x02002F6E RID: 12142
			public class DISPOSABLE_ELECTROBANK_ORGANIC
			{
				// Token: 0x0400C88A RID: 51338
				public static LocString NAME = UI.PRE_KEYWORD + "Organic Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C88B RID: 51339
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Organic Power Banks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F6F RID: 12143
			public class DISPOSABLE_ELECTROBANK_URANIUM_ORE
			{
				// Token: 0x0400C88C RID: 51340
				public static LocString NAME = UI.PRE_KEYWORD + "Nuclear Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C88D RID: 51341
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Nuclear Power Banks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F70 RID: 12144
			public class ELECTROBANK
			{
				// Token: 0x0400C88E RID: 51342
				public static LocString NAME = UI.PRE_KEYWORD + "Eco Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C88F RID: 51343
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Eco Power Banks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F71 RID: 12145
			public class PILOTINGBOOSTER
			{
				// Token: 0x0400C890 RID: 51344
				public static LocString NAME = UI.PRE_KEYWORD + "Rocketry Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C891 RID: 51345
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Rocketry Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F72 RID: 12146
			public class CONSTRUCTIONBOOSTER
			{
				// Token: 0x0400C892 RID: 51346
				public static LocString NAME = UI.PRE_KEYWORD + "Building Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C893 RID: 51347
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Building Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F73 RID: 12147
			public class EXCAVATIONBOOSTER
			{
				// Token: 0x0400C894 RID: 51348
				public static LocString NAME = UI.PRE_KEYWORD + "Digging Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C895 RID: 51349
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Digging Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F74 RID: 12148
			public class EXPLORERBOOSTER
			{
				// Token: 0x0400C896 RID: 51350
				public static LocString NAME = UI.PRE_KEYWORD + "Dowsing Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C897 RID: 51351
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Dowsing Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F75 RID: 12149
			public class MACHINERYBOOSTER
			{
				// Token: 0x0400C898 RID: 51352
				public static LocString NAME = UI.PRE_KEYWORD + "Operating Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C899 RID: 51353
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Operating Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F76 RID: 12150
			public class ATHLETICSBOOSTER
			{
				// Token: 0x0400C89A RID: 51354
				public static LocString NAME = UI.PRE_KEYWORD + "Athletics Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C89B RID: 51355
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Athletics Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F77 RID: 12151
			public class SCIENCEBOOSTER
			{
				// Token: 0x0400C89C RID: 51356
				public static LocString NAME = UI.PRE_KEYWORD + "Researching Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C89D RID: 51357
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Researching Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F78 RID: 12152
			public class COOKINGBOOSTER
			{
				// Token: 0x0400C89E RID: 51358
				public static LocString NAME = UI.PRE_KEYWORD + "Cooking Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C89F RID: 51359
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Cooking Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F79 RID: 12153
			public class MEDICINEBOOSTER
			{
				// Token: 0x0400C8A0 RID: 51360
				public static LocString NAME = UI.PRE_KEYWORD + "Doctoring Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C8A1 RID: 51361
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Doctoring Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F7A RID: 12154
			public class STRENGTHBOOSTER
			{
				// Token: 0x0400C8A2 RID: 51362
				public static LocString NAME = UI.PRE_KEYWORD + "Strength Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C8A3 RID: 51363
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Strength Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F7B RID: 12155
			public class CREATIVITYBOOSTER
			{
				// Token: 0x0400C8A4 RID: 51364
				public static LocString NAME = UI.PRE_KEYWORD + "Decorating Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C8A5 RID: 51365
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Decorating Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F7C RID: 12156
			public class AGRICULTUREBOOSTER
			{
				// Token: 0x0400C8A6 RID: 51366
				public static LocString NAME = UI.PRE_KEYWORD + "Farming Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C8A7 RID: 51367
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Farming Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002F7D RID: 12157
			public class HUSBANDRYBOOSTER
			{
				// Token: 0x0400C8A8 RID: 51368
				public static LocString NAME = UI.PRE_KEYWORD + "Ranching Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400C8A9 RID: 51369
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Ranching Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}
		}

		// Token: 0x020021AA RID: 8618
		public class TREES
		{
			// Token: 0x04009980 RID: 39296
			public static LocString TITLE_FOOD = "Food";

			// Token: 0x04009981 RID: 39297
			public static LocString TITLE_POWER = "Power";

			// Token: 0x04009982 RID: 39298
			public static LocString TITLE_SOLIDS = "Solid Material";

			// Token: 0x04009983 RID: 39299
			public static LocString TITLE_COLONYDEVELOPMENT = "Colony Development";

			// Token: 0x04009984 RID: 39300
			public static LocString TITLE_RADIATIONTECH = "Radiation Technologies";

			// Token: 0x04009985 RID: 39301
			public static LocString TITLE_MEDICINE = "Medicine";

			// Token: 0x04009986 RID: 39302
			public static LocString TITLE_LIQUIDS = "Liquids";

			// Token: 0x04009987 RID: 39303
			public static LocString TITLE_GASES = "Gases";

			// Token: 0x04009988 RID: 39304
			public static LocString TITLE_SUITS = "Exosuits";

			// Token: 0x04009989 RID: 39305
			public static LocString TITLE_DECOR = "Decor";

			// Token: 0x0400998A RID: 39306
			public static LocString TITLE_COMPUTERS = "Computers";

			// Token: 0x0400998B RID: 39307
			public static LocString TITLE_ROCKETS = "Rocketry";
		}

		// Token: 0x020021AB RID: 8619
		public class TECHS
		{
			// Token: 0x02002F7E RID: 12158
			public class JOBS
			{
				// Token: 0x0400C8AA RID: 51370
				public static LocString NAME = UI.FormatAsLink("Employment", "JOBS");

				// Token: 0x0400C8AB RID: 51371
				public static LocString DESC = "Exchange the skill points earned by Duplicants for new traits and abilities.";
			}

			// Token: 0x02002F7F RID: 12159
			public class IMPROVEDOXYGEN
			{
				// Token: 0x0400C8AC RID: 51372
				public static LocString NAME = UI.FormatAsLink("Air Systems", "IMPROVEDOXYGEN");

				// Token: 0x0400C8AD RID: 51373
				public static LocString DESC = "Maintain clean, breathable air in the colony.";
			}

			// Token: 0x02002F80 RID: 12160
			public class FARMINGTECH
			{
				// Token: 0x0400C8AE RID: 51374
				public static LocString NAME = UI.FormatAsLink("Basic Farming", "FARMINGTECH");

				// Token: 0x0400C8AF RID: 51375
				public static LocString DESC = "Learn the introductory principles of " + UI.FormatAsLink("Plant", "PLANTS") + " domestication.";
			}

			// Token: 0x02002F81 RID: 12161
			public class AGRICULTURE
			{
				// Token: 0x0400C8B0 RID: 51376
				public static LocString NAME = UI.FormatAsLink("Agriculture", "AGRICULTURE");

				// Token: 0x0400C8B1 RID: 51377
				public static LocString DESC = "Master the agricultural art of crop raising.";
			}

			// Token: 0x02002F82 RID: 12162
			public class RANCHING
			{
				// Token: 0x0400C8B2 RID: 51378
				public static LocString NAME = UI.FormatAsLink("Ranching", "RANCHING");

				// Token: 0x0400C8B3 RID: 51379
				public static LocString DESC = "Tame and care for wild critters.";
			}

			// Token: 0x02002F83 RID: 12163
			public class ANIMALCONTROL
			{
				// Token: 0x0400C8B4 RID: 51380
				public static LocString NAME = UI.FormatAsLink("Animal Control", "ANIMALCONTROL");

				// Token: 0x0400C8B5 RID: 51381
				public static LocString DESC = "Useful techniques to manage critter populations in the colony.";
			}

			// Token: 0x02002F84 RID: 12164
			public class ANIMALCOMFORT
			{
				// Token: 0x0400C8B6 RID: 51382
				public static LocString NAME = UI.FormatAsLink("Creature Comforts", "ANIMALCOMFORT");

				// Token: 0x0400C8B7 RID: 51383
				public static LocString DESC = "Strategies for maximizing critters' quality of life.";
			}

			// Token: 0x02002F85 RID: 12165
			public class DAIRYOPERATION
			{
				// Token: 0x0400C8B8 RID: 51384
				public static LocString NAME = UI.FormatAsLink("Brackene Flow", "DAIRYOPERATION");

				// Token: 0x0400C8B9 RID: 51385
				public static LocString DESC = "Advanced production, processing and distribution of this fluid resource.";
			}

			// Token: 0x02002F86 RID: 12166
			public class FOODREPURPOSING
			{
				// Token: 0x0400C8BA RID: 51386
				public static LocString NAME = UI.FormatAsLink("Food Repurposing", "FOODREPURPOSING");

				// Token: 0x0400C8BB RID: 51387
				public static LocString DESC = string.Concat(new string[]
				{
					"Blend that leftover ",
					UI.FormatAsLink("Food", "FOOD"),
					" into a ",
					UI.FormatAsLink("Morale", "MORALE"),
					"-boosting slurry."
				});
			}

			// Token: 0x02002F87 RID: 12167
			public class FINEDINING
			{
				// Token: 0x0400C8BC RID: 51388
				public static LocString NAME = UI.FormatAsLink("Meal Preparation", "FINEDINING");

				// Token: 0x0400C8BD RID: 51389
				public static LocString DESC = "Prepare more nutritious " + UI.FormatAsLink("Food", "FOOD") + " and store it longer before spoiling.";
			}

			// Token: 0x02002F88 RID: 12168
			public class FINERDINING
			{
				// Token: 0x0400C8BE RID: 51390
				public static LocString NAME = UI.FormatAsLink("Gourmet Meal Preparation", "FINERDINING");

				// Token: 0x0400C8BF RID: 51391
				public static LocString DESC = "Raise colony Morale by cooking the most delicious, high-quality " + UI.FormatAsLink("Foods", "FOOD") + ".";
			}

			// Token: 0x02002F89 RID: 12169
			public class GASPIPING
			{
				// Token: 0x0400C8C0 RID: 51392
				public static LocString NAME = UI.FormatAsLink("Ventilation", "GASPIPING");

				// Token: 0x0400C8C1 RID: 51393
				public static LocString DESC = "Rudimentary technologies for installing " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " infrastructure.";
			}

			// Token: 0x02002F8A RID: 12170
			public class IMPROVEDGASPIPING
			{
				// Token: 0x0400C8C2 RID: 51394
				public static LocString NAME = UI.FormatAsLink("Improved Ventilation", "IMPROVEDGASPIPING");

				// Token: 0x0400C8C3 RID: 51395
				public static LocString DESC = UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " infrastructure capable of withstanding more intense conditions, such as " + UI.FormatAsLink("Heat", "Heat") + " and pressure.";
			}

			// Token: 0x02002F8B RID: 12171
			public class FLOWREDIRECTION
			{
				// Token: 0x0400C8C4 RID: 51396
				public static LocString NAME = UI.FormatAsLink("Flow Redirection", "FLOWREDIRECTION");

				// Token: 0x0400C8C5 RID: 51397
				public static LocString DESC = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " management for " + UI.FormatAsLink("Morale", "MORALE") + " and industry.";
			}

			// Token: 0x02002F8C RID: 12172
			public class LIQUIDDISTRIBUTION
			{
				// Token: 0x0400C8C6 RID: 51398
				public static LocString NAME = UI.FormatAsLink("Liquid Distribution", "LIQUIDDISTRIBUTION");

				// Token: 0x0400C8C7 RID: 51399
				public static LocString DESC = "Advanced fittings ensure that " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources get where they need to go.";
			}

			// Token: 0x02002F8D RID: 12173
			public class TEMPERATUREMODULATION
			{
				// Token: 0x0400C8C8 RID: 51400
				public static LocString NAME = UI.FormatAsLink("Temperature Modulation", "TEMPERATUREMODULATION");

				// Token: 0x0400C8C9 RID: 51401
				public static LocString DESC = "Precise " + UI.FormatAsLink("Temperature", "HEAT") + " altering technologies to keep my colony at the perfect Kelvin.";
			}

			// Token: 0x02002F8E RID: 12174
			public class HVAC
			{
				// Token: 0x0400C8CA RID: 51402
				public static LocString NAME = UI.FormatAsLink("HVAC", "HVAC");

				// Token: 0x0400C8CB RID: 51403
				public static LocString DESC = string.Concat(new string[]
				{
					"Regulate ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" in the colony for ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" cultivation and Duplicant comfort."
				});
			}

			// Token: 0x02002F8F RID: 12175
			public class GASDISTRIBUTION
			{
				// Token: 0x0400C8CC RID: 51404
				public static LocString NAME = UI.FormatAsLink("Gas Distribution", "GASDISTRIBUTION");

				// Token: 0x0400C8CD RID: 51405
				public static LocString DESC = "Design building hookups to get " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources circulating properly.";
			}

			// Token: 0x02002F90 RID: 12176
			public class LIQUIDTEMPERATURE
			{
				// Token: 0x0400C8CE RID: 51406
				public static LocString NAME = UI.FormatAsLink("Liquid Tuning", "LIQUIDTEMPERATURE");

				// Token: 0x0400C8CF RID: 51407
				public static LocString DESC = string.Concat(new string[]
				{
					"Easily manipulate ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" ",
					UI.FormatAsLink("Heat", "Temperatures"),
					" with these temperature regulating technologies."
				});
			}

			// Token: 0x02002F91 RID: 12177
			public class INSULATION
			{
				// Token: 0x0400C8D0 RID: 51408
				public static LocString NAME = UI.FormatAsLink("Insulation", "INSULATION");

				// Token: 0x0400C8D1 RID: 51409
				public static LocString DESC = "Improve " + UI.FormatAsLink("Heat", "Heat") + " distribution within the colony and guard buildings from extreme temperatures.";
			}

			// Token: 0x02002F92 RID: 12178
			public class PRESSUREMANAGEMENT
			{
				// Token: 0x0400C8D2 RID: 51410
				public static LocString NAME = UI.FormatAsLink("Pressure Management", "PRESSUREMANAGEMENT");

				// Token: 0x0400C8D3 RID: 51411
				public static LocString DESC = "Unlock technologies to manage colony pressure and atmosphere.";
			}

			// Token: 0x02002F93 RID: 12179
			public class PORTABLEGASSES
			{
				// Token: 0x0400C8D4 RID: 51412
				public static LocString NAME = UI.FormatAsLink("Portable Gases", "PORTABLEGASSES");

				// Token: 0x0400C8D5 RID: 51413
				public static LocString DESC = "Unlock technologies to easily move gases around your colony.";
			}

			// Token: 0x02002F94 RID: 12180
			public class DIRECTEDAIRSTREAMS
			{
				// Token: 0x0400C8D6 RID: 51414
				public static LocString NAME = UI.FormatAsLink("Decontamination", "DIRECTEDAIRSTREAMS");

				// Token: 0x0400C8D7 RID: 51415
				public static LocString DESC = "Instruments to help reduce " + UI.FormatAsLink("Germ", "DISEASE") + " spread within the base.";
			}

			// Token: 0x02002F95 RID: 12181
			public class LIQUIDFILTERING
			{
				// Token: 0x0400C8D8 RID: 51416
				public static LocString NAME = UI.FormatAsLink("Liquid-Based Refinement Processes", "LIQUIDFILTERING");

				// Token: 0x0400C8D9 RID: 51417
				public static LocString DESC = "Use pumped " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " to filter out unwanted elements.";
			}

			// Token: 0x02002F96 RID: 12182
			public class LIQUIDPIPING
			{
				// Token: 0x0400C8DA RID: 51418
				public static LocString NAME = UI.FormatAsLink("Plumbing", "LIQUIDPIPING");

				// Token: 0x0400C8DB RID: 51419
				public static LocString DESC = "Rudimentary technologies for installing " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " infrastructure.";
			}

			// Token: 0x02002F97 RID: 12183
			public class IMPROVEDLIQUIDPIPING
			{
				// Token: 0x0400C8DC RID: 51420
				public static LocString NAME = UI.FormatAsLink("Improved Plumbing", "IMPROVEDLIQUIDPIPING");

				// Token: 0x0400C8DD RID: 51421
				public static LocString DESC = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " infrastructure capable of withstanding more intense conditions, such as " + UI.FormatAsLink("Heat", "Heat") + " and pressure.";
			}

			// Token: 0x02002F98 RID: 12184
			public class PRECISIONPLUMBING
			{
				// Token: 0x0400C8DE RID: 51422
				public static LocString NAME = UI.FormatAsLink("Advanced Caffeination", "PRECISIONPLUMBING");

				// Token: 0x0400C8DF RID: 51423
				public static LocString DESC = "Let Duplicants relax after a long day of subterranean digging with a shot of warm beanjuice.";
			}

			// Token: 0x02002F99 RID: 12185
			public class SANITATIONSCIENCES
			{
				// Token: 0x0400C8E0 RID: 51424
				public static LocString NAME = UI.FormatAsLink("Sanitation", "SANITATIONSCIENCES");

				// Token: 0x0400C8E1 RID: 51425
				public static LocString DESC = "Make daily ablutions less of a hassle.";
			}

			// Token: 0x02002F9A RID: 12186
			public class ADVANCEDSANITATION
			{
				// Token: 0x0400C8E2 RID: 51426
				public static LocString NAME = UI.FormatAsLink("Advanced Sanitation", "ADVANCEDSANITATION");

				// Token: 0x0400C8E3 RID: 51427
				public static LocString DESC = "Clean up dirty Duplicants.";
			}

			// Token: 0x02002F9B RID: 12187
			public class MEDICINEI
			{
				// Token: 0x0400C8E4 RID: 51428
				public static LocString NAME = UI.FormatAsLink("Pharmacology", "MEDICINEI");

				// Token: 0x0400C8E5 RID: 51429
				public static LocString DESC = "Compound natural cures to fight the most common " + UI.FormatAsLink("Sicknesses", "SICKNESSES") + " that plague Duplicants.";
			}

			// Token: 0x02002F9C RID: 12188
			public class MEDICINEII
			{
				// Token: 0x0400C8E6 RID: 51430
				public static LocString NAME = UI.FormatAsLink("Medical Equipment", "MEDICINEII");

				// Token: 0x0400C8E7 RID: 51431
				public static LocString DESC = "The basic necessities doctors need to facilitate patient care.";
			}

			// Token: 0x02002F9D RID: 12189
			public class MEDICINEIII
			{
				// Token: 0x0400C8E8 RID: 51432
				public static LocString NAME = UI.FormatAsLink("Pathogen Diagnostics", "MEDICINEIII");

				// Token: 0x0400C8E9 RID: 51433
				public static LocString DESC = "Stop Germs at the source using special medical automation technology.";
			}

			// Token: 0x02002F9E RID: 12190
			public class MEDICINEIV
			{
				// Token: 0x0400C8EA RID: 51434
				public static LocString NAME = UI.FormatAsLink("Micro-Targeted Medicine", "MEDICINEIV");

				// Token: 0x0400C8EB RID: 51435
				public static LocString DESC = "State of the art equipment to conquer the most stubborn of illnesses.";
			}

			// Token: 0x02002F9F RID: 12191
			public class ADVANCEDFILTRATION
			{
				// Token: 0x0400C8EC RID: 51436
				public static LocString NAME = UI.FormatAsLink("Filtration", "ADVANCEDFILTRATION");

				// Token: 0x0400C8ED RID: 51437
				public static LocString DESC = string.Concat(new string[]
				{
					"Basic technologies for filtering ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x02002FA0 RID: 12192
			public class POWERREGULATION
			{
				// Token: 0x0400C8EE RID: 51438
				public static LocString NAME = UI.FormatAsLink("Power Regulation", "POWERREGULATION");

				// Token: 0x0400C8EF RID: 51439
				public static LocString DESC = "Prevent wasted " + UI.FormatAsLink("Power", "POWER") + " with improved electrical tools.";
			}

			// Token: 0x02002FA1 RID: 12193
			public class COMBUSTION
			{
				// Token: 0x0400C8F0 RID: 51440
				public static LocString NAME = UI.FormatAsLink("Internal Combustion", "COMBUSTION");

				// Token: 0x0400C8F1 RID: 51441
				public static LocString DESC = "Fuel-powered generators for crude yet powerful " + UI.FormatAsLink("Power", "POWER") + " production.";
			}

			// Token: 0x02002FA2 RID: 12194
			public class IMPROVEDCOMBUSTION
			{
				// Token: 0x0400C8F2 RID: 51442
				public static LocString NAME = UI.FormatAsLink("Fossil Fuels", "IMPROVEDCOMBUSTION");

				// Token: 0x0400C8F3 RID: 51443
				public static LocString DESC = "Burn dirty fuels for exceptional " + UI.FormatAsLink("Power", "POWER") + " production.";
			}

			// Token: 0x02002FA3 RID: 12195
			public class INTERIORDECOR
			{
				// Token: 0x0400C8F4 RID: 51444
				public static LocString NAME = UI.FormatAsLink("Interior Decor", "INTERIORDECOR");

				// Token: 0x0400C8F5 RID: 51445
				public static LocString DESC = UI.FormatAsLink("Decor", "DECOR") + " boosting items to counteract the gloom of underground living.";
			}

			// Token: 0x02002FA4 RID: 12196
			public class ARTISTRY
			{
				// Token: 0x0400C8F6 RID: 51446
				public static LocString NAME = UI.FormatAsLink("Artistic Expression", "ARTISTRY");

				// Token: 0x0400C8F7 RID: 51447
				public static LocString DESC = "Majorly improve " + UI.FormatAsLink("Decor", "DECOR") + " by giving Duplicants the tools of artistic and emotional expression.";
			}

			// Token: 0x02002FA5 RID: 12197
			public class CLOTHING
			{
				// Token: 0x0400C8F8 RID: 51448
				public static LocString NAME = UI.FormatAsLink("Textile Production", "CLOTHING");

				// Token: 0x0400C8F9 RID: 51449
				public static LocString DESC = "Bring Duplicants the " + UI.FormatAsLink("Morale", "MORALE") + " boosting benefits of soft, cushy fabrics.";
			}

			// Token: 0x02002FA6 RID: 12198
			public class ACOUSTICS
			{
				// Token: 0x0400C8FA RID: 51450
				public static LocString NAME = UI.FormatAsLink("Sound Amplifiers", "ACOUSTICS");

				// Token: 0x0400C8FB RID: 51451
				public static LocString DESC = "Precise control of the audio spectrum allows Duplicants to get funky.";
			}

			// Token: 0x02002FA7 RID: 12199
			public class SPACEPOWER
			{
				// Token: 0x0400C8FC RID: 51452
				public static LocString NAME = UI.FormatAsLink("Space Power", "SPACEPOWER");

				// Token: 0x0400C8FD RID: 51453
				public static LocString DESC = "It's like power... in space!";
			}

			// Token: 0x02002FA8 RID: 12200
			public class AMPLIFIERS
			{
				// Token: 0x0400C8FE RID: 51454
				public static LocString NAME = UI.FormatAsLink("Power Amplifiers", "AMPLIFIERS");

				// Token: 0x0400C8FF RID: 51455
				public static LocString DESC = "Further increased efficacy of " + UI.FormatAsLink("Power", "POWER") + " management to prevent those wasted joules.";
			}

			// Token: 0x02002FA9 RID: 12201
			public class LUXURY
			{
				// Token: 0x0400C900 RID: 51456
				public static LocString NAME = UI.FormatAsLink("Home Luxuries", "LUXURY");

				// Token: 0x0400C901 RID: 51457
				public static LocString DESC = "Luxury amenities for advanced " + UI.FormatAsLink("Stress", "STRESS") + " reduction.";
			}

			// Token: 0x02002FAA RID: 12202
			public class ENVIRONMENTALAPPRECIATION
			{
				// Token: 0x0400C902 RID: 51458
				public static LocString NAME = UI.FormatAsLink("Environmental Appreciation", "ENVIRONMENTALAPPRECIATION");

				// Token: 0x0400C903 RID: 51459
				public static LocString DESC = string.Concat(new string[]
				{
					"Improve ",
					UI.FormatAsLink("Morale", "MORALE"),
					" by lazing around in ",
					UI.FormatAsLink("Light", "LIGHT"),
					" with a high Lux value."
				});
			}

			// Token: 0x02002FAB RID: 12203
			public class FINEART
			{
				// Token: 0x0400C904 RID: 51460
				public static LocString NAME = UI.FormatAsLink("Fine Art", "FINEART");

				// Token: 0x0400C905 RID: 51461
				public static LocString DESC = "Broader options for artistic " + UI.FormatAsLink("Decor", "DECOR") + " improvements.";
			}

			// Token: 0x02002FAC RID: 12204
			public class REFRACTIVEDECOR
			{
				// Token: 0x0400C906 RID: 51462
				public static LocString NAME = UI.FormatAsLink("High Culture", "REFRACTIVEDECOR");

				// Token: 0x0400C907 RID: 51463
				public static LocString DESC = "New methods for working with extremely high quality art materials.";
			}

			// Token: 0x02002FAD RID: 12205
			public class RENAISSANCEART
			{
				// Token: 0x0400C908 RID: 51464
				public static LocString NAME = UI.FormatAsLink("Renaissance Art", "RENAISSANCEART");

				// Token: 0x0400C909 RID: 51465
				public static LocString DESC = "The kind of art that culture legacies are made of.";
			}

			// Token: 0x02002FAE RID: 12206
			public class GLASSFURNISHINGS
			{
				// Token: 0x0400C90A RID: 51466
				public static LocString NAME = UI.FormatAsLink("Glass Blowing", "GLASSFURNISHINGS");

				// Token: 0x0400C90B RID: 51467
				public static LocString DESC = "The decorative benefits of glass are both apparent and transparent.";
			}

			// Token: 0x02002FAF RID: 12207
			public class SCREENS
			{
				// Token: 0x0400C90C RID: 51468
				public static LocString NAME = UI.FormatAsLink("New Media", "SCREENS");

				// Token: 0x0400C90D RID: 51469
				public static LocString DESC = "High tech displays with lots of pretty colors.";
			}

			// Token: 0x02002FB0 RID: 12208
			public class ADVANCEDPOWERREGULATION
			{
				// Token: 0x0400C90E RID: 51470
				public static LocString NAME = UI.FormatAsLink("Advanced Power Regulation", "ADVANCEDPOWERREGULATION");

				// Token: 0x0400C90F RID: 51471
				public static LocString DESC = "Circuit components required for large scale " + UI.FormatAsLink("Power", "POWER") + " management.";
			}

			// Token: 0x02002FB1 RID: 12209
			public class PLASTICS
			{
				// Token: 0x0400C910 RID: 51472
				public static LocString NAME = UI.FormatAsLink("Plastic Manufacturing", "PLASTICS");

				// Token: 0x0400C911 RID: 51473
				public static LocString DESC = "Stable, lightweight, durable. Plastics are useful for a wide array of applications.";
			}

			// Token: 0x02002FB2 RID: 12210
			public class SUITS
			{
				// Token: 0x0400C912 RID: 51474
				public static LocString NAME = UI.FormatAsLink("Hazard Protection", "SUITS");

				// Token: 0x0400C913 RID: 51475
				public static LocString DESC = "Vital gear for surviving in extreme conditions and environments.";
			}

			// Token: 0x02002FB3 RID: 12211
			public class DISTILLATION
			{
				// Token: 0x0400C914 RID: 51476
				public static LocString NAME = UI.FormatAsLink("Distillation", "DISTILLATION");

				// Token: 0x0400C915 RID: 51477
				public static LocString DESC = "Distill difficult mixtures down to their most useful parts.";
			}

			// Token: 0x02002FB4 RID: 12212
			public class CATALYTICS
			{
				// Token: 0x0400C916 RID: 51478
				public static LocString NAME = UI.FormatAsLink("Catalytics", "CATALYTICS");

				// Token: 0x0400C917 RID: 51479
				public static LocString DESC = "Advanced gas manipulation using unique catalysts.";
			}

			// Token: 0x02002FB5 RID: 12213
			public class ADVANCEDRESEARCH
			{
				// Token: 0x0400C918 RID: 51480
				public static LocString NAME = UI.FormatAsLink("Advanced Research", "ADVANCEDRESEARCH");

				// Token: 0x0400C919 RID: 51481
				public static LocString DESC = "The tools my colony needs to conduct more advanced, in-depth research.";
			}

			// Token: 0x02002FB6 RID: 12214
			public class SPACEPROGRAM
			{
				// Token: 0x0400C91A RID: 51482
				public static LocString NAME = UI.FormatAsLink("Space Program", "SPACEPROGRAM");

				// Token: 0x0400C91B RID: 51483
				public static LocString DESC = "The first steps in getting a Duplicant to space.";
			}

			// Token: 0x02002FB7 RID: 12215
			public class CRASHPLAN
			{
				// Token: 0x0400C91C RID: 51484
				public static LocString NAME = UI.FormatAsLink("Crash Plan", "CRASHPLAN");

				// Token: 0x0400C91D RID: 51485
				public static LocString DESC = "What goes up, must come down.";
			}

			// Token: 0x02002FB8 RID: 12216
			public class DURABLELIFESUPPORT
			{
				// Token: 0x0400C91E RID: 51486
				public static LocString NAME = UI.FormatAsLink("Durable Life Support", "DURABLELIFESUPPORT");

				// Token: 0x0400C91F RID: 51487
				public static LocString DESC = "Improved devices for extended missions into space.";
			}

			// Token: 0x02002FB9 RID: 12217
			public class ARTIFICIALFRIENDS
			{
				// Token: 0x0400C920 RID: 51488
				public static LocString NAME = UI.FormatAsLink("Artificial Friends", "ARTIFICIALFRIENDS");

				// Token: 0x0400C921 RID: 51489
				public static LocString DESC = "Sweeping advances in companion technology.";
			}

			// Token: 0x02002FBA RID: 12218
			public class ROBOTICTOOLS
			{
				// Token: 0x0400C922 RID: 51490
				public static LocString NAME = UI.FormatAsLink("Robotic Tools", "ROBOTICTOOLS");

				// Token: 0x0400C923 RID: 51491
				public static LocString DESC = "The goal of every great civilization is to one day make itself obsolete.";
			}

			// Token: 0x02002FBB RID: 12219
			public class LOGICCONTROL
			{
				// Token: 0x0400C924 RID: 51492
				public static LocString NAME = UI.FormatAsLink("Smart Home", "LOGICCONTROL");

				// Token: 0x0400C925 RID: 51493
				public static LocString DESC = "Switches that grant full control of building operations within the colony.";
			}

			// Token: 0x02002FBC RID: 12220
			public class LOGICCIRCUITS
			{
				// Token: 0x0400C926 RID: 51494
				public static LocString NAME = UI.FormatAsLink("Advanced Automation", "LOGICCIRCUITS");

				// Token: 0x0400C927 RID: 51495
				public static LocString DESC = "The only limit to colony automation is my own imagination.";
			}

			// Token: 0x02002FBD RID: 12221
			public class PARALLELAUTOMATION
			{
				// Token: 0x0400C928 RID: 51496
				public static LocString NAME = UI.FormatAsLink("Parallel Automation", "PARALLELAUTOMATION");

				// Token: 0x0400C929 RID: 51497
				public static LocString DESC = "Multi-wire automation at a fraction of the space.";
			}

			// Token: 0x02002FBE RID: 12222
			public class MULTIPLEXING
			{
				// Token: 0x0400C92A RID: 51498
				public static LocString NAME = UI.FormatAsLink("Multiplexing", "MULTIPLEXING");

				// Token: 0x0400C92B RID: 51499
				public static LocString DESC = "More choices for Automation signal distribution.";
			}

			// Token: 0x02002FBF RID: 12223
			public class VALVEMINIATURIZATION
			{
				// Token: 0x0400C92C RID: 51500
				public static LocString NAME = UI.FormatAsLink("Valve Miniaturization", "VALVEMINIATURIZATION");

				// Token: 0x0400C92D RID: 51501
				public static LocString DESC = "Smaller, more efficient pumps for those low-throughput situations.";
			}

			// Token: 0x02002FC0 RID: 12224
			public class HYDROCARBONPROPULSION
			{
				// Token: 0x0400C92E RID: 51502
				public static LocString NAME = UI.FormatAsLink("Hydrocarbon Propulsion", "HYDROCARBONPROPULSION");

				// Token: 0x0400C92F RID: 51503
				public static LocString DESC = "Low-range rocket engines with lots of smoke.";
			}

			// Token: 0x02002FC1 RID: 12225
			public class BETTERHYDROCARBONPROPULSION
			{
				// Token: 0x0400C930 RID: 51504
				public static LocString NAME = UI.FormatAsLink("Improved Hydrocarbon Propulsion", "BETTERHYDROCARBONPROPULSION");

				// Token: 0x0400C931 RID: 51505
				public static LocString DESC = "Mid-range rocket engines with lots of smoke.";
			}

			// Token: 0x02002FC2 RID: 12226
			public class PRETTYGOODCONDUCTORS
			{
				// Token: 0x0400C932 RID: 51506
				public static LocString NAME = UI.FormatAsLink("Low-Resistance Conductors", "PRETTYGOODCONDUCTORS");

				// Token: 0x0400C933 RID: 51507
				public static LocString DESC = "Pure-core wires that can handle more " + UI.FormatAsLink("Electrical", "POWER") + " current without overloading.";
			}

			// Token: 0x02002FC3 RID: 12227
			public class RENEWABLEENERGY
			{
				// Token: 0x0400C934 RID: 51508
				public static LocString NAME = UI.FormatAsLink("Renewable Energy", "RENEWABLEENERGY");

				// Token: 0x0400C935 RID: 51509
				public static LocString DESC = "Clean, sustainable " + UI.FormatAsLink("Power", "POWER") + " production that produces little to no waste.";
			}

			// Token: 0x02002FC4 RID: 12228
			public class BASICREFINEMENT
			{
				// Token: 0x0400C936 RID: 51510
				public static LocString NAME = UI.FormatAsLink("Brute-Force Refinement", "BASICREFINEMENT");

				// Token: 0x0400C937 RID: 51511
				public static LocString DESC = "Low-tech refinement methods for producing clay and renewable sources of sand.";
			}

			// Token: 0x02002FC5 RID: 12229
			public class REFINEDOBJECTS
			{
				// Token: 0x0400C938 RID: 51512
				public static LocString NAME = UI.FormatAsLink("Refined Renovations", "REFINEDOBJECTS");

				// Token: 0x0400C939 RID: 51513
				public static LocString DESC = "Improve base infrastructure with new objects crafted from " + UI.FormatAsLink("Refined Metals", "REFINEDMETAL") + ".";
			}

			// Token: 0x02002FC6 RID: 12230
			public class GENERICSENSORS
			{
				// Token: 0x0400C93A RID: 51514
				public static LocString NAME = UI.FormatAsLink("Generic Sensors", "GENERICSENSORS");

				// Token: 0x0400C93B RID: 51515
				public static LocString DESC = "Drive automation in a variety of new, inventive ways.";
			}

			// Token: 0x02002FC7 RID: 12231
			public class DUPETRAFFICCONTROL
			{
				// Token: 0x0400C93C RID: 51516
				public static LocString NAME = UI.FormatAsLink("Computing", "DUPETRAFFICCONTROL");

				// Token: 0x0400C93D RID: 51517
				public static LocString DESC = "Virtually extend the boundaries of Duplicant imagination.";
			}

			// Token: 0x02002FC8 RID: 12232
			public class ADVANCEDSCANNERS
			{
				// Token: 0x0400C93E RID: 51518
				public static LocString NAME = UI.FormatAsLink("Sensitive Microimaging", "ADVANCEDSCANNERS");

				// Token: 0x0400C93F RID: 51519
				public static LocString DESC = "Computerized systems do the looking, so Duplicants don't have to.";
			}

			// Token: 0x02002FC9 RID: 12233
			public class SMELTING
			{
				// Token: 0x0400C940 RID: 51520
				public static LocString NAME = UI.FormatAsLink("Smelting", "SMELTING");

				// Token: 0x0400C941 RID: 51521
				public static LocString DESC = "High temperatures facilitate the production of purer, special use metal resources.";
			}

			// Token: 0x02002FCA RID: 12234
			public class TRAVELTUBES
			{
				// Token: 0x0400C942 RID: 51522
				public static LocString NAME = UI.FormatAsLink("Transit Tubes", "TRAVELTUBES");

				// Token: 0x0400C943 RID: 51523
				public static LocString DESC = "A wholly futuristic way to move Duplicants around the base.";
			}

			// Token: 0x02002FCB RID: 12235
			public class SMARTSTORAGE
			{
				// Token: 0x0400C944 RID: 51524
				public static LocString NAME = UI.FormatAsLink("Smart Storage", "SMARTSTORAGE");

				// Token: 0x0400C945 RID: 51525
				public static LocString DESC = "Completely automate the storage of solid resources.";
			}

			// Token: 0x02002FCC RID: 12236
			public class SOLIDTRANSPORT
			{
				// Token: 0x0400C946 RID: 51526
				public static LocString NAME = UI.FormatAsLink("Solid Transport", "SOLIDTRANSPORT");

				// Token: 0x0400C947 RID: 51527
				public static LocString DESC = "Free Duplicants from the drudgery of day-to-day material deliveries with new methods of automation.";
			}

			// Token: 0x02002FCD RID: 12237
			public class SOLIDMANAGEMENT
			{
				// Token: 0x0400C948 RID: 51528
				public static LocString NAME = UI.FormatAsLink("Solid Management", "SOLIDMANAGEMENT");

				// Token: 0x0400C949 RID: 51529
				public static LocString DESC = "Make solid decisions in " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " sorting.";
			}

			// Token: 0x02002FCE RID: 12238
			public class SOLIDDISTRIBUTION
			{
				// Token: 0x0400C94A RID: 51530
				public static LocString NAME = UI.FormatAsLink("Solid Distribution", "SOLIDDISTRIBUTION");

				// Token: 0x0400C94B RID: 51531
				public static LocString DESC = "Internal rocket hookups for " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " resources.";
			}

			// Token: 0x02002FCF RID: 12239
			public class HIGHTEMPFORGING
			{
				// Token: 0x0400C94C RID: 51532
				public static LocString NAME = UI.FormatAsLink("Superheated Forging", "HIGHTEMPFORGING");

				// Token: 0x0400C94D RID: 51533
				public static LocString DESC = "Craft entirely new materials by harnessing the most extreme temperatures.";
			}

			// Token: 0x02002FD0 RID: 12240
			public class HIGHPRESSUREFORGING
			{
				// Token: 0x0400C94E RID: 51534
				public static LocString NAME = UI.FormatAsLink("Pressurized Forging", "HIGHPRESSUREFORGING");

				// Token: 0x0400C94F RID: 51535
				public static LocString DESC = "High pressure diamond forging.";
			}

			// Token: 0x02002FD1 RID: 12241
			public class RADIATIONPROTECTION
			{
				// Token: 0x0400C950 RID: 51536
				public static LocString NAME = UI.FormatAsLink("Radiation Protection", "RADIATIONPROTECTION");

				// Token: 0x0400C951 RID: 51537
				public static LocString DESC = "Shield Duplicants from dangerous amounts of radiation.";
			}

			// Token: 0x02002FD2 RID: 12242
			public class SKYDETECTORS
			{
				// Token: 0x0400C952 RID: 51538
				public static LocString NAME = UI.FormatAsLink("Celestial Detection", "SKYDETECTORS");

				// Token: 0x0400C953 RID: 51539
				public static LocString DESC = "Turn Duplicants' eyes to the skies and discover what undiscovered wonders await out there.";
			}

			// Token: 0x02002FD3 RID: 12243
			public class JETPACKS
			{
				// Token: 0x0400C954 RID: 51540
				public static LocString NAME = UI.FormatAsLink("Jetpacks", "JETPACKS");

				// Token: 0x0400C955 RID: 51541
				public static LocString DESC = "Objectively the most stylish way for Duplicants to get around.";
			}

			// Token: 0x02002FD4 RID: 12244
			public class BASICROCKETRY
			{
				// Token: 0x0400C956 RID: 51542
				public static LocString NAME = UI.FormatAsLink("Introductory Rocketry", "BASICROCKETRY");

				// Token: 0x0400C957 RID: 51543
				public static LocString DESC = "Everything required for launching the colony's very first space program.";
			}

			// Token: 0x02002FD5 RID: 12245
			public class ENGINESI
			{
				// Token: 0x0400C958 RID: 51544
				public static LocString NAME = UI.FormatAsLink("Solid Fuel Combustion", "ENGINESI");

				// Token: 0x0400C959 RID: 51545
				public static LocString DESC = "Rockets that fly further, longer.";
			}

			// Token: 0x02002FD6 RID: 12246
			public class ENGINESII
			{
				// Token: 0x0400C95A RID: 51546
				public static LocString NAME = UI.FormatAsLink("Hydrocarbon Combustion", "ENGINESII");

				// Token: 0x0400C95B RID: 51547
				public static LocString DESC = "Delve deeper into the vastness of space than ever before.";
			}

			// Token: 0x02002FD7 RID: 12247
			public class ENGINESIII
			{
				// Token: 0x0400C95C RID: 51548
				public static LocString NAME = UI.FormatAsLink("Cryofuel Combustion", "ENGINESIII");

				// Token: 0x0400C95D RID: 51549
				public static LocString DESC = "With this technology, the sky is your oyster. Go exploring!";
			}

			// Token: 0x02002FD8 RID: 12248
			public class CRYOFUELPROPULSION
			{
				// Token: 0x0400C95E RID: 51550
				public static LocString NAME = UI.FormatAsLink("Cryofuel Propulsion", "CRYOFUELPROPULSION");

				// Token: 0x0400C95F RID: 51551
				public static LocString DESC = "A semi-powerful engine to propel you further into the galaxy.";
			}

			// Token: 0x02002FD9 RID: 12249
			public class NUCLEARPROPULSION
			{
				// Token: 0x0400C960 RID: 51552
				public static LocString NAME = UI.FormatAsLink("Radbolt Propulsion", "NUCLEARPROPULSION");

				// Token: 0x0400C961 RID: 51553
				public static LocString DESC = "Radical technology to get you to the stars.";
			}

			// Token: 0x02002FDA RID: 12250
			public class ADVANCEDRESOURCEEXTRACTION
			{
				// Token: 0x0400C962 RID: 51554
				public static LocString NAME = UI.FormatAsLink("Advanced Resource Extraction", "ADVANCEDRESOURCEEXTRACTION");

				// Token: 0x0400C963 RID: 51555
				public static LocString DESC = "Bring back souvieners from the stars.";
			}

			// Token: 0x02002FDB RID: 12251
			public class CARGOI
			{
				// Token: 0x0400C964 RID: 51556
				public static LocString NAME = UI.FormatAsLink("Solid Cargo", "CARGOI");

				// Token: 0x0400C965 RID: 51557
				public static LocString DESC = "Make extra use of journeys into space by mining and storing useful resources.";
			}

			// Token: 0x02002FDC RID: 12252
			public class CARGOII
			{
				// Token: 0x0400C966 RID: 51558
				public static LocString NAME = UI.FormatAsLink("Liquid and Gas Cargo", "CARGOII");

				// Token: 0x0400C967 RID: 51559
				public static LocString DESC = "Extract precious liquids and gases from the far reaches of space, and return with them to the colony.";
			}

			// Token: 0x02002FDD RID: 12253
			public class CARGOIII
			{
				// Token: 0x0400C968 RID: 51560
				public static LocString NAME = UI.FormatAsLink("Unique Cargo", "CARGOIII");

				// Token: 0x0400C969 RID: 51561
				public static LocString DESC = "Allow Duplicants to take their friends to see the stars... or simply bring souvenirs back from their travels.";
			}

			// Token: 0x02002FDE RID: 12254
			public class NOTIFICATIONSYSTEMS
			{
				// Token: 0x0400C96A RID: 51562
				public static LocString NAME = UI.FormatAsLink("Notification Systems", "NOTIFICATIONSYSTEMS");

				// Token: 0x0400C96B RID: 51563
				public static LocString DESC = "Get all the news you need to know about your complex colony.";
			}

			// Token: 0x02002FDF RID: 12255
			public class NUCLEARREFINEMENT
			{
				// Token: 0x0400C96C RID: 51564
				public static LocString NAME = UI.FormatAsLink("Radiation Refinement", "NUCLEAR");

				// Token: 0x0400C96D RID: 51565
				public static LocString DESC = "Refine uranium and generate radiation.";
			}

			// Token: 0x02002FE0 RID: 12256
			public class NUCLEARRESEARCH
			{
				// Token: 0x0400C96E RID: 51566
				public static LocString NAME = UI.FormatAsLink("Materials Science Research", "NUCLEARRESEARCH");

				// Token: 0x0400C96F RID: 51567
				public static LocString DESC = "Harness sub-atomic particles to study the properties of matter.";
			}

			// Token: 0x02002FE1 RID: 12257
			public class ADVANCEDNUCLEARRESEARCH
			{
				// Token: 0x0400C970 RID: 51568
				public static LocString NAME = UI.FormatAsLink("More Materials Science Research", "ADVANCEDNUCLEARRESEARCH");

				// Token: 0x0400C971 RID: 51569
				public static LocString DESC = "Harness sub-atomic particles to study the properties of matter even more.";
			}

			// Token: 0x02002FE2 RID: 12258
			public class NUCLEARSTORAGE
			{
				// Token: 0x0400C972 RID: 51570
				public static LocString NAME = UI.FormatAsLink("Radbolt Containment", "NUCLEARSTORAGE");

				// Token: 0x0400C973 RID: 51571
				public static LocString DESC = "Build a quality cache of radbolts.";
			}

			// Token: 0x02002FE3 RID: 12259
			public class SOLIDSPACE
			{
				// Token: 0x0400C974 RID: 51572
				public static LocString NAME = UI.FormatAsLink("Solid Control", "SOLIDSPACE");

				// Token: 0x0400C975 RID: 51573
				public static LocString DESC = "Transport and sort " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " resources.";
			}

			// Token: 0x02002FE4 RID: 12260
			public class HIGHVELOCITYTRANSPORT
			{
				// Token: 0x0400C976 RID: 51574
				public static LocString NAME = UI.FormatAsLink("High Velocity Transport", "HIGHVELOCITY");

				// Token: 0x0400C977 RID: 51575
				public static LocString DESC = "Hurl things through space.";
			}

			// Token: 0x02002FE5 RID: 12261
			public class MONUMENTS
			{
				// Token: 0x0400C978 RID: 51576
				public static LocString NAME = UI.FormatAsLink("Monuments", "MONUMENTS");

				// Token: 0x0400C979 RID: 51577
				public static LocString DESC = "Monumental art projects.";
			}

			// Token: 0x02002FE6 RID: 12262
			public class BIOENGINEERING
			{
				// Token: 0x0400C97A RID: 51578
				public static LocString NAME = UI.FormatAsLink("Bioengineering", "BIOENGINEERING");

				// Token: 0x0400C97B RID: 51579
				public static LocString DESC = "Mutation station.";
			}

			// Token: 0x02002FE7 RID: 12263
			public class SPACECOMBUSTION
			{
				// Token: 0x0400C97C RID: 51580
				public static LocString NAME = UI.FormatAsLink("Advanced Combustion", "SPACECOMBUSTION");

				// Token: 0x0400C97D RID: 51581
				public static LocString DESC = "Sweet advancements in rocket engines.";
			}

			// Token: 0x02002FE8 RID: 12264
			public class HIGHVELOCITYDESTRUCTION
			{
				// Token: 0x0400C97E RID: 51582
				public static LocString NAME = UI.FormatAsLink("High Velocity Destruction", "HIGHVELOCITYDESTRUCTION");

				// Token: 0x0400C97F RID: 51583
				public static LocString DESC = "Mine the skies.";
			}

			// Token: 0x02002FE9 RID: 12265
			public class SPACEGAS
			{
				// Token: 0x0400C980 RID: 51584
				public static LocString NAME = UI.FormatAsLink("Advanced Gas Flow", "SPACEGAS");

				// Token: 0x0400C981 RID: 51585
				public static LocString DESC = UI.FormatAsLink("Gas", "ELEMENTS_GASSES") + " engines and transportation for rockets.";
			}

			// Token: 0x02002FEA RID: 12266
			public class DATASCIENCE
			{
				// Token: 0x0400C982 RID: 51586
				public static LocString NAME = UI.FormatAsLink("Data Science", "DATASCIENCE");

				// Token: 0x0400C983 RID: 51587
				public static LocString DESC = "The science of making the data work for my Duplicants, instead of the other way around.";
			}

			// Token: 0x02002FEB RID: 12267
			public class DATASCIENCEBASEGAME
			{
				// Token: 0x0400C984 RID: 51588
				public static LocString NAME = UI.FormatAsLink("Data Science", "DATASCIENCEBASEGAME");

				// Token: 0x0400C985 RID: 51589
				public static LocString DESC = "The science of making the data work for my Duplicants, instead of the other way around.";
			}
		}
	}
}
