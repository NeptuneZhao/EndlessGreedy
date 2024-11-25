using System;

namespace STRINGS
{
	// Token: 0x02000F1A RID: 3866
	public class ITEMS
	{
		// Token: 0x020021BD RID: 8637
		public class PILLS
		{
			// Token: 0x020033C5 RID: 13253
			public class PLACEBO
			{
				// Token: 0x0400D341 RID: 54081
				public static LocString NAME = "Placebo";

				// Token: 0x0400D342 RID: 54082
				public static LocString DESC = "A general, all-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".\n\nThe less one knows about it, the better it works.";

				// Token: 0x0400D343 RID: 54083
				public static LocString RECIPEDESC = "All-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".";
			}

			// Token: 0x020033C6 RID: 13254
			public class BASICBOOSTER
			{
				// Token: 0x0400D344 RID: 54084
				public static LocString NAME = "Vitamin Chews";

				// Token: 0x0400D345 RID: 54085
				public static LocString DESC = "Minorly reduces the chance of becoming sick.";

				// Token: 0x0400D346 RID: 54086
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A supplement that minorly reduces the chance of contracting a ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nMust be taken daily."
				});
			}

			// Token: 0x020033C7 RID: 13255
			public class INTERMEDIATEBOOSTER
			{
				// Token: 0x0400D347 RID: 54087
				public static LocString NAME = "Immuno Booster";

				// Token: 0x0400D348 RID: 54088
				public static LocString DESC = "Significantly reduces the chance of becoming sick.";

				// Token: 0x0400D349 RID: 54089
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A supplement that significantly reduces the chance of contracting a ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nMust be taken daily."
				});
			}

			// Token: 0x020033C8 RID: 13256
			public class ANTIHISTAMINE
			{
				// Token: 0x0400D34A RID: 54090
				public static LocString NAME = "Allergy Medication";

				// Token: 0x0400D34B RID: 54091
				public static LocString DESC = "Suppresses and prevents allergic reactions.";

				// Token: 0x0400D34C RID: 54092
				public static LocString RECIPEDESC = "A strong antihistamine Duplicants can take to halt an allergic reaction. " + ITEMS.PILLS.ANTIHISTAMINE.NAME + " will also prevent further reactions from occurring for a short time after ingestion.";
			}

			// Token: 0x020033C9 RID: 13257
			public class BASICCURE
			{
				// Token: 0x0400D34D RID: 54093
				public static LocString NAME = "Curative Tablet";

				// Token: 0x0400D34E RID: 54094
				public static LocString DESC = "A simple, easy-to-take remedy for minor germ-based diseases.";

				// Token: 0x0400D34F RID: 54095
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Duplicants can take this to cure themselves of minor ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nCurative Tablets are very effective against ",
					UI.FormatAsLink("Food Poisoning", "FOODSICKNESS"),
					"."
				});
			}

			// Token: 0x020033CA RID: 13258
			public class INTERMEDIATECURE
			{
				// Token: 0x0400D350 RID: 54096
				public static LocString NAME = "Medical Pack";

				// Token: 0x0400D351 RID: 54097
				public static LocString DESC = "A doctor-administered cure for moderate ailments.";

				// Token: 0x0400D352 RID: 54098
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A doctor-administered cure for moderate ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					". ",
					ITEMS.PILLS.INTERMEDIATECURE.NAME,
					"s are very effective against ",
					UI.FormatAsLink("Slimelung", "SLIMESICKNESS"),
					".\n\nMust be administered by a Duplicant with the ",
					DUPLICANTS.ROLES.MEDIC.NAME,
					" Skill."
				});
			}

			// Token: 0x020033CB RID: 13259
			public class ADVANCEDCURE
			{
				// Token: 0x0400D353 RID: 54099
				public static LocString NAME = "Serum Vial";

				// Token: 0x0400D354 RID: 54100
				public static LocString DESC = "A doctor-administered cure for severe ailments.";

				// Token: 0x0400D355 RID: 54101
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"An extremely powerful medication created to treat severe ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					". ",
					ITEMS.PILLS.ADVANCEDCURE.NAME,
					" is very effective against ",
					UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES"),
					".\n\nMust be administered by a Duplicant with the ",
					DUPLICANTS.ROLES.SENIOR_MEDIC.NAME,
					" Skill."
				});
			}

			// Token: 0x020033CC RID: 13260
			public class BASICRADPILL
			{
				// Token: 0x0400D356 RID: 54102
				public static LocString NAME = "Basic Rad Pill";

				// Token: 0x0400D357 RID: 54103
				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";

				// Token: 0x0400D358 RID: 54104
				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}

			// Token: 0x020033CD RID: 13261
			public class INTERMEDIATERADPILL
			{
				// Token: 0x0400D359 RID: 54105
				public static LocString NAME = "Intermediate Rad Pill";

				// Token: 0x0400D35A RID: 54106
				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";

				// Token: 0x0400D35B RID: 54107
				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}
		}

		// Token: 0x020021BE RID: 8638
		public class BIONIC_BOOSTERS
		{
			// Token: 0x020033CE RID: 13262
			public class EXPLORER_BOOSTER
			{
				// Token: 0x0400D35C RID: 54108
				public static LocString NAME = "Dowsing Booster";

				// Token: 0x0400D35D RID: 54109
				public static LocString DESC = "Enables a Bionic Duplicant to regularly uncover hidden geysers.";
			}

			// Token: 0x020033CF RID: 13263
			public class PILOTING_BOOSTER
			{
				// Token: 0x0400D35E RID: 54110
				public static LocString NAME = "Rocketry Booster";

				// Token: 0x0400D35F RID: 54111
				public static LocString DESC = "Increases a Bionic Duplicant's rocket piloting skills.";
			}

			// Token: 0x020033D0 RID: 13264
			public class CONSTRUCTION_BOOSTER
			{
				// Token: 0x0400D360 RID: 54112
				public static LocString NAME = "Building Booster";

				// Token: 0x0400D361 RID: 54113
				public static LocString DESC = "Increases a Bionic Duplicant's construction skills.";
			}

			// Token: 0x020033D1 RID: 13265
			public class EXCAVATION_BOOSTER
			{
				// Token: 0x0400D362 RID: 54114
				public static LocString NAME = "Excavation Booster";

				// Token: 0x0400D363 RID: 54115
				public static LocString DESC = "Increases a Bionic Duplicant's digging skills.";
			}

			// Token: 0x020033D2 RID: 13266
			public class MACHINERY_BOOSTER
			{
				// Token: 0x0400D364 RID: 54116
				public static LocString NAME = "Operating Booster";

				// Token: 0x0400D365 RID: 54117
				public static LocString DESC = "Increases a Bionic Duplicant's machinery skills.";
			}

			// Token: 0x020033D3 RID: 13267
			public class ATHLETICS_BOOSTER
			{
				// Token: 0x0400D366 RID: 54118
				public static LocString NAME = "Athletics Booster";

				// Token: 0x0400D367 RID: 54119
				public static LocString DESC = "Increases a Bionic Duplicant's runspeed.";
			}

			// Token: 0x020033D4 RID: 13268
			public class SCIENCE_BOOSTER
			{
				// Token: 0x0400D368 RID: 54120
				public static LocString NAME = "Researching Booster";

				// Token: 0x0400D369 RID: 54121
				public static LocString DESC = "Increases a Bionic Duplicant's science researching skills.";
			}

			// Token: 0x020033D5 RID: 13269
			public class COOKING_BOOSTER
			{
				// Token: 0x0400D36A RID: 54122
				public static LocString NAME = "Cooking Booster";

				// Token: 0x0400D36B RID: 54123
				public static LocString DESC = "Increases a Bionic Duplicant's culinary skills.";
			}

			// Token: 0x020033D6 RID: 13270
			public class MEDICINE_BOOSTER
			{
				// Token: 0x0400D36C RID: 54124
				public static LocString NAME = "Doctoring Booster";

				// Token: 0x0400D36D RID: 54125
				public static LocString DESC = "Increases a Bionic Duplicant's doctoring skills.";
			}

			// Token: 0x020033D7 RID: 13271
			public class STRENGTH_BOOSTER
			{
				// Token: 0x0400D36E RID: 54126
				public static LocString NAME = "Strength Booster";

				// Token: 0x0400D36F RID: 54127
				public static LocString DESC = "Increases a Bionic Duplicant's carrying capacity and tidying speed.";
			}

			// Token: 0x020033D8 RID: 13272
			public class CREATIVITY_BOOSTER
			{
				// Token: 0x0400D370 RID: 54128
				public static LocString NAME = "Decorating Booster";

				// Token: 0x0400D371 RID: 54129
				public static LocString DESC = "Increases a Bionic Duplicant's creativity.";
			}

			// Token: 0x020033D9 RID: 13273
			public class AGRICULTURE_BOOSTER
			{
				// Token: 0x0400D372 RID: 54130
				public static LocString NAME = "Farming Booster";

				// Token: 0x0400D373 RID: 54131
				public static LocString DESC = "Increases a Bionic Duplicant's agricultural skills.";
			}

			// Token: 0x020033DA RID: 13274
			public class HUSBANDRY_BOOSTER
			{
				// Token: 0x0400D374 RID: 54132
				public static LocString NAME = "Ranching Booster";

				// Token: 0x0400D375 RID: 54133
				public static LocString DESC = "Increases a Bionic Duplicant's husbandry skills.";
			}
		}

		// Token: 0x020021BF RID: 8639
		public class FOOD
		{
			// Token: 0x040099BA RID: 39354
			public static LocString COMPOST = "Compost";

			// Token: 0x020033DB RID: 13275
			public class FOODSPLAT
			{
				// Token: 0x0400D376 RID: 54134
				public static LocString NAME = "Food Splatter";

				// Token: 0x0400D377 RID: 54135
				public static LocString DESC = "Food smeared on the wall from a recent Food Fight";
			}

			// Token: 0x020033DC RID: 13276
			public class BURGER
			{
				// Token: 0x0400D378 RID: 54136
				public static LocString NAME = UI.FormatAsLink("Frost Burger", "BURGER");

				// Token: 0x0400D379 RID: 54137
				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					".\n\nIt's the only burger best served cold."
				});

				// Token: 0x0400D37A RID: 54138
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					"."
				});

				// Token: 0x02003839 RID: 14393
				public class DEHYDRATED
				{
					// Token: 0x0400DE87 RID: 56967
					public static LocString NAME = "Dried Frost Burger";

					// Token: 0x0400DE88 RID: 56968
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Frost Burger", "BURGER"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x020033DD RID: 13277
			public class FIELDRATION
			{
				// Token: 0x0400D37B RID: 54139
				public static LocString NAME = UI.FormatAsLink("Nutrient Bar", "FIELDRATION");

				// Token: 0x0400D37C RID: 54140
				public static LocString DESC = "A nourishing nutrient paste, sandwiched between thin wafer layers.";
			}

			// Token: 0x020033DE RID: 13278
			public class MUSHBAR
			{
				// Token: 0x0400D37D RID: 54141
				public static LocString NAME = UI.FormatAsLink("Mush Bar", "MUSHBAR");

				// Token: 0x0400D37E RID: 54142
				public static LocString DESC = "An edible, putrefied mudslop.\n\nMush Bars are preferable to starvation, but only just barely.";

				// Token: 0x0400D37F RID: 54143
				public static LocString RECIPEDESC = "An edible, putrefied mudslop.\n\n" + ITEMS.FOOD.MUSHBAR.NAME + "s are preferable to starvation, but only just barely.";
			}

			// Token: 0x020033DF RID: 13279
			public class MUSHROOMWRAP
			{
				// Token: 0x0400D380 RID: 54144
				public static LocString NAME = UI.FormatAsLink("Mushroom Wrap", "MUSHROOMWRAP");

				// Token: 0x0400D381 RID: 54145
				public static LocString DESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nIt has an earthy flavor punctuated by a refreshing crunch."
				});

				// Token: 0x0400D382 RID: 54146
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});

				// Token: 0x0200383A RID: 14394
				public class DEHYDRATED
				{
					// Token: 0x0400DE89 RID: 56969
					public static LocString NAME = "Dried Mushroom Wrap";

					// Token: 0x0400DE8A RID: 56970
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Mushroom Wrap", "MUSHROOMWRAP"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x020033E0 RID: 13280
			public class MICROWAVEDLETTUCE
			{
				// Token: 0x0400D383 RID: 54147
				public static LocString NAME = UI.FormatAsLink("Microwaved Lettuce", "MICROWAVEDLETTUCE");

				// Token: 0x0400D384 RID: 54148
				public static LocString DESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";

				// Token: 0x0400D385 RID: 54149
				public static LocString RECIPEDESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}

			// Token: 0x020033E1 RID: 13281
			public class GAMMAMUSH
			{
				// Token: 0x0400D386 RID: 54150
				public static LocString NAME = UI.FormatAsLink("Gamma Mush", "GAMMAMUSH");

				// Token: 0x0400D387 RID: 54151
				public static LocString DESC = "A disturbingly delicious mixture of irradiated dirt and water.";

				// Token: 0x0400D388 RID: 54152
				public static LocString RECIPEDESC = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR") + " reheated in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}

			// Token: 0x020033E2 RID: 13282
			public class FRUITCAKE
			{
				// Token: 0x0400D389 RID: 54153
				public static LocString NAME = UI.FormatAsLink("Berry Sludge", "FRUITCAKE");

				// Token: 0x0400D38A RID: 54154
				public static LocString DESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.\n\nIts aggressive, overbearing sweetness can leave the tongue feeling temporarily numb.";

				// Token: 0x0400D38B RID: 54155
				public static LocString RECIPEDESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.";
			}

			// Token: 0x020033E3 RID: 13283
			public class POPCORN
			{
				// Token: 0x0400D38C RID: 54156
				public static LocString NAME = UI.FormatAsLink("Popcorn", "POPCORN");

				// Token: 0x0400D38D RID: 54157
				public static LocString DESC = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " popped in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".\n\nCompletely devoid of any fancy flavorings.";

				// Token: 0x0400D38E RID: 54158
				public static LocString RECIPEDESC = "Gamma-radiated " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + ".";
			}

			// Token: 0x020033E4 RID: 13284
			public class SUSHI
			{
				// Token: 0x0400D38F RID: 54159
				public static LocString NAME = UI.FormatAsLink("Sushi", "SUSHI");

				// Token: 0x0400D390 RID: 54160
				public static LocString DESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nWhile the salt of the lettuce may initially overpower the flavor, a keen palate can discern the subtle sweetness of the fillet beneath."
				});

				// Token: 0x0400D391 RID: 54161
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});
			}

			// Token: 0x020033E5 RID: 13285
			public class HATCHEGG
			{
				// Token: 0x0400D392 RID: 54162
				public static LocString NAME = CREATURES.SPECIES.HATCH.EGG_NAME;

				// Token: 0x0400D393 RID: 54163
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Hatch", "HATCH"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Hatchling", "HATCH"),
					"."
				});

				// Token: 0x0400D394 RID: 54164
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Hatch", "HATCH") + ".";
			}

			// Token: 0x020033E6 RID: 13286
			public class DRECKOEGG
			{
				// Token: 0x0400D395 RID: 54165
				public static LocString NAME = CREATURES.SPECIES.DRECKO.EGG_NAME;

				// Token: 0x0400D396 RID: 54166
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Drecko", "DRECKO"),
					".\n\nIf incubated, it will hatch into a new ",
					UI.FormatAsLink("Drecklet", "DRECKO"),
					"."
				});

				// Token: 0x0400D397 RID: 54167
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Drecko", "DRECKO") + ".";
			}

			// Token: 0x020033E7 RID: 13287
			public class LIGHTBUGEGG
			{
				// Token: 0x0400D398 RID: 54168
				public static LocString NAME = CREATURES.SPECIES.LIGHTBUG.EGG_NAME;

				// Token: 0x0400D399 RID: 54169
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Shine Bug", "LIGHTBUG"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Shine Nymph", "LIGHTBUG"),
					"."
				});

				// Token: 0x0400D39A RID: 54170
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Shine Bug", "LIGHTBUG") + ".";
			}

			// Token: 0x020033E8 RID: 13288
			public class LETTUCE
			{
				// Token: 0x0400D39B RID: 54171
				public static LocString NAME = UI.FormatAsLink("Lettuce", "LETTUCE");

				// Token: 0x0400D39C RID: 54172
				public static LocString DESC = "Crunchy, slightly salty leaves from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + " plant.";

				// Token: 0x0400D39D RID: 54173
				public static LocString RECIPEDESC = "Edible roughage from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + ".";
			}

			// Token: 0x020033E9 RID: 13289
			public class PASTA
			{
				// Token: 0x0400D39E RID: 54174
				public static LocString NAME = UI.FormatAsLink("Pasta", "PASTA");

				// Token: 0x0400D39F RID: 54175
				public static LocString DESC = "pasta made from egg and wheat";

				// Token: 0x0400D3A0 RID: 54176
				public static LocString RECIPEDESC = "pasta made from egg and wheat";
			}

			// Token: 0x020033EA RID: 13290
			public class PANCAKES
			{
				// Token: 0x0400D3A1 RID: 54177
				public static LocString NAME = UI.FormatAsLink("Soufflé Pancakes", "PANCAKES");

				// Token: 0x0400D3A2 RID: 54178
				public static LocString DESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED"),
					".\n\nThey're so thick!"
				});

				// Token: 0x0400D3A3 RID: 54179
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED"),
					"."
				});
			}

			// Token: 0x020033EB RID: 13291
			public class OILFLOATEREGG
			{
				// Token: 0x0400D3A4 RID: 54180
				public static LocString NAME = CREATURES.SPECIES.OILFLOATER.EGG_NAME;

				// Token: 0x0400D3A5 RID: 54181
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Slickster", "OILFLOATER"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Slickster Larva", "OILFLOATER"),
					"."
				});

				// Token: 0x0400D3A6 RID: 54182
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Slickster", "OILFLOATER") + ".";
			}

			// Token: 0x020033EC RID: 13292
			public class PUFTEGG
			{
				// Token: 0x0400D3A7 RID: 54183
				public static LocString NAME = CREATURES.SPECIES.PUFT.EGG_NAME;

				// Token: 0x0400D3A8 RID: 54184
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Puft", "PUFT"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Puftlet", "PUFT"),
					"."
				});

				// Token: 0x0400D3A9 RID: 54185
				public static LocString RECIPEDESC = "An egg laid by a " + CREATURES.SPECIES.PUFT.NAME + ".";
			}

			// Token: 0x020033ED RID: 13293
			public class FISHMEAT
			{
				// Token: 0x0400D3AA RID: 54186
				public static LocString NAME = UI.FormatAsLink("Pacu Fillet", "FISHMEAT");

				// Token: 0x0400D3AB RID: 54187
				public static LocString DESC = "An uncooked fillet from a very dead " + CREATURES.SPECIES.PACU.NAME + ". Yum!";
			}

			// Token: 0x020033EE RID: 13294
			public class MEAT
			{
				// Token: 0x0400D3AC RID: 54188
				public static LocString NAME = UI.FormatAsLink("Meat", "MEAT");

				// Token: 0x0400D3AD RID: 54189
				public static LocString DESC = "Uncooked meat from a very dead critter. Yum!";
			}

			// Token: 0x020033EF RID: 13295
			public class PLANTMEAT
			{
				// Token: 0x0400D3AE RID: 54190
				public static LocString NAME = UI.FormatAsLink("Plant Meat", "PLANTMEAT");

				// Token: 0x0400D3AF RID: 54191
				public static LocString DESC = "Planty plant meat from a plant. How nice!";
			}

			// Token: 0x020033F0 RID: 13296
			public class SHELLFISHMEAT
			{
				// Token: 0x0400D3B0 RID: 54192
				public static LocString NAME = UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT");

				// Token: 0x0400D3B1 RID: 54193
				public static LocString DESC = "An uncooked chunk of very dead " + CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.NAME + ". Yum!";
			}

			// Token: 0x020033F1 RID: 13297
			public class MUSHROOM
			{
				// Token: 0x0400D3B2 RID: 54194
				public static LocString NAME = UI.FormatAsLink("Mushroom", "MUSHROOM");

				// Token: 0x0400D3B3 RID: 54195
				public static LocString DESC = "An edible, flavorless fungus that grew in the dark.";
			}

			// Token: 0x020033F2 RID: 13298
			public class COOKEDFISH
			{
				// Token: 0x0400D3B4 RID: 54196
				public static LocString NAME = UI.FormatAsLink("Cooked Seafood", "COOKEDFISH");

				// Token: 0x0400D3B5 RID: 54197
				public static LocString DESC = "A cooked piece of freshly caught aquatic critter.\n\nUnsurprisingly, it tastes a bit fishy.";

				// Token: 0x0400D3B6 RID: 54198
				public static LocString RECIPEDESC = "A cooked piece of freshly caught aquatic critter.";
			}

			// Token: 0x020033F3 RID: 13299
			public class COOKEDMEAT
			{
				// Token: 0x0400D3B7 RID: 54199
				public static LocString NAME = UI.FormatAsLink("Barbeque", "COOKEDMEAT");

				// Token: 0x0400D3B8 RID: 54200
				public static LocString DESC = "The cooked meat of a defeated critter.\n\nIt has a delightful smoky aftertaste.";

				// Token: 0x0400D3B9 RID: 54201
				public static LocString RECIPEDESC = "The cooked meat of a defeated critter.";
			}

			// Token: 0x020033F4 RID: 13300
			public class FRIESCARROT
			{
				// Token: 0x0400D3BA RID: 54202
				public static LocString NAME = UI.FormatAsLink("Squash Fries", "FRIESCARROT");

				// Token: 0x0400D3BB RID: 54203
				public static LocString DESC = "Irresistibly crunchy.\n\nBest eaten hot.";

				// Token: 0x0400D3BC RID: 54204
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Crunchy sticks of ",
					UI.FormatAsLink("Plume Squash", "CARROT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x020033F5 RID: 13301
			public class DEEPFRIEDFISH
			{
				// Token: 0x0400D3BD RID: 54205
				public static LocString NAME = UI.FormatAsLink("Fish Taco", "DEEPFRIEDFISH");

				// Token: 0x0400D3BE RID: 54206
				public static LocString DESC = "Deep-fried fish cradled in a crunchy fin.";

				// Token: 0x0400D3BF RID: 54207
				public static LocString RECIPEDESC = UI.FormatAsLink("Pacu Fillet", "FISHMEAT") + " lightly battered and deep-fried in " + UI.FormatAsLink("Tallow", "TALLOW") + ".";
			}

			// Token: 0x020033F6 RID: 13302
			public class DEEPFRIEDSHELLFISH
			{
				// Token: 0x0400D3C0 RID: 54208
				public static LocString NAME = UI.FormatAsLink("Shellfish Tempura", "DEEPFRIEDSHELLFISH");

				// Token: 0x0400D3C1 RID: 54209
				public static LocString DESC = "A crispy deep-fried critter claw.";

				// Token: 0x0400D3C2 RID: 54210
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A tender chunk of battered ",
					UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x020033F7 RID: 13303
			public class DEEPFRIEDMEAT
			{
				// Token: 0x0400D3C3 RID: 54211
				public static LocString NAME = UI.FormatAsLink("Deep Fried Steak", "DEEPFRIEDMEAT");

				// Token: 0x0400D3C4 RID: 54212
				public static LocString DESC = "A juicy slab of meat with a crunchy deep-fried upper layer.";

				// Token: 0x0400D3C5 RID: 54213
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A juicy slab of ",
					UI.FormatAsLink("Raw Meat", "MEAT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x020033F8 RID: 13304
			public class DEEPFRIEDNOSH
			{
				// Token: 0x0400D3C6 RID: 54214
				public static LocString NAME = UI.FormatAsLink("Nosh Noms", "DEEPFRIEDNOSH");

				// Token: 0x0400D3C7 RID: 54215
				public static LocString DESC = "A snackable handful of crunchy beans.";

				// Token: 0x0400D3C8 RID: 54216
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A crunchy stack of ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x020033F9 RID: 13305
			public class PICKLEDMEAL
			{
				// Token: 0x0400D3C9 RID: 54217
				public static LocString NAME = UI.FormatAsLink("Pickled Meal", "PICKLEDMEAL");

				// Token: 0x0400D3CA RID: 54218
				public static LocString DESC = "Meal Lice preserved in vinegar.\n\nIt's a rarely acquired taste.";

				// Token: 0x0400D3CB RID: 54219
				public static LocString RECIPEDESC = ITEMS.FOOD.BASICPLANTFOOD.NAME + " regrettably preserved in vinegar.";
			}

			// Token: 0x020033FA RID: 13306
			public class FRIEDMUSHBAR
			{
				// Token: 0x0400D3CC RID: 54220
				public static LocString NAME = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR");

				// Token: 0x0400D3CD RID: 54221
				public static LocString DESC = "Pan-fried, solidified mudslop.\n\nThe inside is almost completely uncooked, despite the crunch on the outside.";

				// Token: 0x0400D3CE RID: 54222
				public static LocString RECIPEDESC = "Pan-fried, solidified mudslop.";
			}

			// Token: 0x020033FB RID: 13307
			public class RAWEGG
			{
				// Token: 0x0400D3CF RID: 54223
				public static LocString NAME = UI.FormatAsLink("Raw Egg", "RAWEGG");

				// Token: 0x0400D3D0 RID: 54224
				public static LocString DESC = "A raw Egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.\n\nIt will never hatch.";

				// Token: 0x0400D3D1 RID: 54225
				public static LocString RECIPEDESC = "A raw egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.";
			}

			// Token: 0x020033FC RID: 13308
			public class COOKEDEGG
			{
				// Token: 0x0400D3D2 RID: 54226
				public static LocString NAME = UI.FormatAsLink("Omelette", "COOKEDEGG");

				// Token: 0x0400D3D3 RID: 54227
				public static LocString DESC = "Fluffed and folded Egg innards.\n\nIt turns out you do, in fact, have to break a few eggs to make it.";

				// Token: 0x0400D3D4 RID: 54228
				public static LocString RECIPEDESC = "Fluffed and folded egg innards.";
			}

			// Token: 0x020033FD RID: 13309
			public class FRIEDMUSHROOM
			{
				// Token: 0x0400D3D5 RID: 54229
				public static LocString NAME = UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM");

				// Token: 0x0400D3D6 RID: 54230
				public static LocString DESC = "A pan-fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".\n\nIt has a thick, savory flavor with subtle earthy undertones.";

				// Token: 0x0400D3D7 RID: 54231
				public static LocString RECIPEDESC = "A pan-fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".";
			}

			// Token: 0x020033FE RID: 13310
			public class COOKEDPIKEAPPLE
			{
				// Token: 0x0400D3D8 RID: 54232
				public static LocString NAME = UI.FormatAsLink("Pikeapple Skewer", "COOKEDPIKEAPPLE");

				// Token: 0x0400D3D9 RID: 54233
				public static LocString DESC = "Grilling a " + UI.FormatAsLink("Pikeapple", "HARDSKINBERRY") + " softens its spikes, making it slighly less awkward to eat.\n\nIt does not diminish the smell.";

				// Token: 0x0400D3DA RID: 54234
				public static LocString RECIPEDESC = "A grilled dish made with a fruiting " + UI.FormatAsLink("Pikeapple", "HARDSKINBERRY") + ".";
			}

			// Token: 0x020033FF RID: 13311
			public class PRICKLEFRUIT
			{
				// Token: 0x0400D3DB RID: 54235
				public static LocString NAME = UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT");

				// Token: 0x0400D3DC RID: 54236
				public static LocString DESC = "A sweet, mostly pleasant-tasting fruit covered in prickly barbs.";
			}

			// Token: 0x02003400 RID: 13312
			public class GRILLEDPRICKLEFRUIT
			{
				// Token: 0x0400D3DD RID: 54237
				public static LocString NAME = UI.FormatAsLink("Gristle Berry", "GRILLEDPRICKLEFRUIT");

				// Token: 0x0400D3DE RID: 54238
				public static LocString DESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".\n\nHeat unlocked an exquisite taste in the fruit, though the burnt spines leave something to be desired.";

				// Token: 0x0400D3DF RID: 54239
				public static LocString RECIPEDESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".";
			}

			// Token: 0x02003401 RID: 13313
			public class SWAMPFRUIT
			{
				// Token: 0x0400D3E0 RID: 54240
				public static LocString NAME = UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT");

				// Token: 0x0400D3E1 RID: 54241
				public static LocString DESC = "A fruit with an outer film that contains chewy gelatinous cubes.";
			}

			// Token: 0x02003402 RID: 13314
			public class SWAMPDELIGHTS
			{
				// Token: 0x0400D3E2 RID: 54242
				public static LocString NAME = UI.FormatAsLink("Swampy Delights", "SWAMPDELIGHTS");

				// Token: 0x0400D3E3 RID: 54243
				public static LocString DESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".\n\nEach cube has a wonderfully chewy texture and is lightly coated in a delicate powder.";

				// Token: 0x0400D3E4 RID: 54244
				public static LocString RECIPEDESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".";
			}

			// Token: 0x02003403 RID: 13315
			public class WORMBASICFRUIT
			{
				// Token: 0x0400D3E5 RID: 54245
				public static LocString NAME = UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT");

				// Token: 0x0400D3E6 RID: 54246
				public static LocString DESC = "A " + UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT") + " that failed to develop properly.\n\nIt is nonetheless edible, and vaguely tasty.";
			}

			// Token: 0x02003404 RID: 13316
			public class WORMBASICFOOD
			{
				// Token: 0x0400D3E7 RID: 54247
				public static LocString NAME = UI.FormatAsLink("Roast Grubfruit Nut", "WORMBASICFOOD");

				// Token: 0x0400D3E8 RID: 54248
				public static LocString DESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".\n\nIt has a smoky aroma and tastes of coziness.";

				// Token: 0x0400D3E9 RID: 54249
				public static LocString RECIPEDESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".";
			}

			// Token: 0x02003405 RID: 13317
			public class WORMSUPERFRUIT
			{
				// Token: 0x0400D3EA RID: 54250
				public static LocString NAME = UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT");

				// Token: 0x0400D3EB RID: 54251
				public static LocString DESC = "A plump, healthy fruit with a honey-like taste.";
			}

			// Token: 0x02003406 RID: 13318
			public class WORMSUPERFOOD
			{
				// Token: 0x0400D3EC RID: 54252
				public static LocString NAME = UI.FormatAsLink("Grubfruit Preserve", "WORMSUPERFOOD");

				// Token: 0x0400D3ED RID: 54253
				public static LocString DESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					".\n\nThe thick, goopy jam retains the shape of the jar when poured out, but the sweet taste can't be matched."
				});

				// Token: 0x0400D3EE RID: 54254
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					"."
				});
			}

			// Token: 0x02003407 RID: 13319
			public class BERRYPIE
			{
				// Token: 0x0400D3EF RID: 54255
				public static LocString NAME = UI.FormatAsLink("Mixed Berry Pie", "BERRYPIE");

				// Token: 0x0400D3F0 RID: 54256
				public static LocString DESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					".\n\nThe mixture of berries creates a fragrant, colorful filling that packs a sweet punch."
				});

				// Token: 0x0400D3F1 RID: 54257
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					"."
				});

				// Token: 0x0200383B RID: 14395
				public class DEHYDRATED
				{
					// Token: 0x0400DE8B RID: 56971
					public static LocString NAME = "Dried Berry Pie";

					// Token: 0x0400DE8C RID: 56972
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Mixed Berry Pie", "BERRYPIE"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x02003408 RID: 13320
			public class COLDWHEATBREAD
			{
				// Token: 0x0400D3F2 RID: 54258
				public static LocString NAME = UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD");

				// Token: 0x0400D3F3 RID: 54259
				public static LocString DESC = "A simple bun baked from " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " grain.\n\nEach bite leaves a mild cooling sensation in one's mouth, even when the bun itself is warm.";

				// Token: 0x0400D3F4 RID: 54260
				public static LocString RECIPEDESC = "A simple bun baked from " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " grain.";
			}

			// Token: 0x02003409 RID: 13321
			public class BEAN
			{
				// Token: 0x0400D3F5 RID: 54261
				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEAN");

				// Token: 0x0400D3F6 RID: 54262
				public static LocString DESC = "The crisp bean of a " + UI.FormatAsLink("Nosh Sprout", "BEAN_PLANT") + ".\n\nEach bite tastes refreshingly natural and wholesome.";
			}

			// Token: 0x0200340A RID: 13322
			public class SPICENUT
			{
				// Token: 0x0400D3F7 RID: 54263
				public static LocString NAME = UI.FormatAsLink("Pincha Peppernut", "SPICENUT");

				// Token: 0x0400D3F8 RID: 54264
				public static LocString DESC = "The flavorful nut of a " + UI.FormatAsLink("Pincha Pepperplant", "SPICE_VINE") + ".\n\nThe bitter outer rind hides a rich, peppery core that is useful in cooking.";
			}

			// Token: 0x0200340B RID: 13323
			public class SPICEBREAD
			{
				// Token: 0x0400D3F9 RID: 54265
				public static LocString NAME = UI.FormatAsLink("Pepper Bread", "SPICEBREAD");

				// Token: 0x0400D3FA RID: 54266
				public static LocString DESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.\n\nThere's a simple joy to be had in pulling it apart in one's fingers.";

				// Token: 0x0400D3FB RID: 54267
				public static LocString RECIPEDESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.";

				// Token: 0x0200383C RID: 14396
				public class DEHYDRATED
				{
					// Token: 0x0400DE8D RID: 56973
					public static LocString NAME = "Dried Pepper Bread";

					// Token: 0x0400DE8E RID: 56974
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Pepper Bread", "SPICEBREAD"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200340C RID: 13324
			public class SURFANDTURF
			{
				// Token: 0x0400D3FC RID: 54268
				public static LocString NAME = UI.FormatAsLink("Surf'n'Turf", "SURFANDTURF");

				// Token: 0x0400D3FD RID: 54269
				public static LocString DESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea.\n\nIt's hearty and satisfying."
				});

				// Token: 0x0400D3FE RID: 54270
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea."
				});

				// Token: 0x0200383D RID: 14397
				public class DEHYDRATED
				{
					// Token: 0x0400DE8F RID: 56975
					public static LocString NAME = "Dried Surf'n'Turf";

					// Token: 0x0400DE90 RID: 56976
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Surf'n'Turf", "SURFANDTURF"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200340D RID: 13325
			public class TOFU
			{
				// Token: 0x0400D3FF RID: 54271
				public static LocString NAME = UI.FormatAsLink("Tofu", "TOFU");

				// Token: 0x0400D400 RID: 54272
				public static LocString DESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED") + ".\n\nIt has an unusual but pleasant consistency.";

				// Token: 0x0400D401 RID: 54273
				public static LocString RECIPEDESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED") + ".";
			}

			// Token: 0x0200340E RID: 13326
			public class SPICYTOFU
			{
				// Token: 0x0400D402 RID: 54274
				public static LocString NAME = UI.FormatAsLink("Spicy Tofu", "SPICYTOFU");

				// Token: 0x0400D403 RID: 54275
				public static LocString DESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.\n\nIt packs a delightful punch.";

				// Token: 0x0400D404 RID: 54276
				public static LocString RECIPEDESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.";

				// Token: 0x0200383E RID: 14398
				public class DEHYDRATED
				{
					// Token: 0x0400DE91 RID: 56977
					public static LocString NAME = "Dried Spicy Tofu";

					// Token: 0x0400DE92 RID: 56978
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Spicy Tofu", "SPICYTOFU"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200340F RID: 13327
			public class CURRY
			{
				// Token: 0x0400D405 RID: 54277
				public static LocString NAME = UI.FormatAsLink("Curried Beans", "CURRY");

				// Token: 0x0400D406 RID: 54278
				public static LocString DESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					".\n\nIt's so spicy!"
				});

				// Token: 0x0400D407 RID: 54279
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					"."
				});

				// Token: 0x0200383F RID: 14399
				public class DEHYDRATED
				{
					// Token: 0x0400DE93 RID: 56979
					public static LocString NAME = "Dried Curried Beans";

					// Token: 0x0400DE94 RID: 56980
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Curried Beans", "CURRY"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x02003410 RID: 13328
			public class SALSA
			{
				// Token: 0x0400D408 RID: 54280
				public static LocString NAME = UI.FormatAsLink("Stuffed Berry", "SALSA");

				// Token: 0x0400D409 RID: 54281
				public static LocString DESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";

				// Token: 0x0400D40A RID: 54282
				public static LocString RECIPEDESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";

				// Token: 0x02003840 RID: 14400
				public class DEHYDRATED
				{
					// Token: 0x0400DE95 RID: 56981
					public static LocString NAME = "Dried Stuffed Berry";

					// Token: 0x0400DE96 RID: 56982
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Stuffed Berry", "SALSA"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x02003411 RID: 13329
			public class HARDSKINBERRY
			{
				// Token: 0x0400D40B RID: 54283
				public static LocString NAME = UI.FormatAsLink("Pikeapple", "HARDSKINBERRY");

				// Token: 0x0400D40C RID: 54284
				public static LocString DESC = "An edible fruit encased in a thorny husk.";
			}

			// Token: 0x02003412 RID: 13330
			public class CARROT
			{
				// Token: 0x0400D40D RID: 54285
				public static LocString NAME = UI.FormatAsLink("Plume Squash", "CARROT");

				// Token: 0x0400D40E RID: 54286
				public static LocString DESC = "An edible tuber with an earthy, elegant flavor.";
			}

			// Token: 0x02003413 RID: 13331
			public class PEMMICAN
			{
				// Token: 0x0400D40F RID: 54287
				public static LocString NAME = UI.FormatAsLink("Pemmican", "PEMMICAN");

				// Token: 0x0400D410 RID: 54288
				public static LocString DESC = UI.FormatAsLink("Meat", "MEAT") + " and " + UI.FormatAsLink("Tallow", "TALLOW") + " pounded into a calorie-dense brick with an exceptionally long shelf life.\n\nSurvival never tasted so good.";

				// Token: 0x0400D411 RID: 54289
				public static LocString RECIPEDESC = UI.FormatAsLink("Meat", "MEAT") + " and " + UI.FormatAsLink("Tallow", "TALLOW") + " pounded into a nutrient-dense brick with an exceptionally long shelf life.";
			}

			// Token: 0x02003414 RID: 13332
			public class BASICPLANTFOOD
			{
				// Token: 0x0400D412 RID: 54290
				public static LocString NAME = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD");

				// Token: 0x0400D413 RID: 54291
				public static LocString DESC = "A flavorless grain that almost never wiggles on its own.";
			}

			// Token: 0x02003415 RID: 13333
			public class BASICPLANTBAR
			{
				// Token: 0x0400D414 RID: 54292
				public static LocString NAME = UI.FormatAsLink("Liceloaf", "BASICPLANTBAR");

				// Token: 0x0400D415 RID: 54293
				public static LocString DESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";

				// Token: 0x0400D416 RID: 54294
				public static LocString RECIPEDESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";
			}

			// Token: 0x02003416 RID: 13334
			public class BASICFORAGEPLANT
			{
				// Token: 0x0400D417 RID: 54295
				public static LocString NAME = UI.FormatAsLink("Muckroot", "BASICFORAGEPLANT");

				// Token: 0x0400D418 RID: 54296
				public static LocString DESC = "A seedless fruit with an upsettingly bland aftertaste.\n\nIt cannot be replanted.\n\nDigging up Buried Objects may uncover a " + ITEMS.FOOD.BASICFORAGEPLANT.NAME + ".";
			}

			// Token: 0x02003417 RID: 13335
			public class FORESTFORAGEPLANT
			{
				// Token: 0x0400D419 RID: 54297
				public static LocString NAME = UI.FormatAsLink("Hexalent Fruit", "FORESTFORAGEPLANT");

				// Token: 0x0400D41A RID: 54298
				public static LocString DESC = "A seedless fruit with an unusual rubbery texture.\n\nIt cannot be replanted.\n\nHexalent fruit is much more calorie dense than Muckroot fruit.";
			}

			// Token: 0x02003418 RID: 13336
			public class SWAMPFORAGEPLANT
			{
				// Token: 0x0400D41B RID: 54299
				public static LocString NAME = UI.FormatAsLink("Swamp Chard Heart", "SWAMPFORAGEPLANT");

				// Token: 0x0400D41C RID: 54300
				public static LocString DESC = "A seedless plant with a squishy, juicy center and an awful smell.\n\nIt cannot be replanted.";
			}

			// Token: 0x02003419 RID: 13337
			public class ICECAVESFORAGEPLANT
			{
				// Token: 0x0400D41D RID: 54301
				public static LocString NAME = UI.FormatAsLink("Sherberry", "ICECAVESFORAGEPLANT");

				// Token: 0x0400D41E RID: 54302
				public static LocString DESC = "A cold seedless fruit that triggers mild brain freeze.\n\nIt cannot be replanted.";
			}

			// Token: 0x0200341A RID: 13338
			public class ROTPILE
			{
				// Token: 0x0400D41F RID: 54303
				public static LocString NAME = UI.FormatAsLink("Rot Pile", "COMPOST");

				// Token: 0x0400D420 RID: 54304
				public static LocString DESC = string.Concat(new string[]
				{
					"An inedible glop of former foodstuff.\n\n",
					ITEMS.FOOD.ROTPILE.NAME,
					"s break down into ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" over time."
				});
			}

			// Token: 0x0200341B RID: 13339
			public class COLDWHEATSEED
			{
				// Token: 0x0400D421 RID: 54305
				public static LocString NAME = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED");

				// Token: 0x0400D422 RID: 54306
				public static LocString DESC = "An edible grain that leaves a cool taste on the tongue.";
			}

			// Token: 0x0200341C RID: 13340
			public class BEANPLANTSEED
			{
				// Token: 0x0400D423 RID: 54307
				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED");

				// Token: 0x0400D424 RID: 54308
				public static LocString DESC = "An inedible bean that can be processed into delicious foods.";
			}

			// Token: 0x0200341D RID: 13341
			public class QUICHE
			{
				// Token: 0x0400D425 RID: 54309
				public static LocString NAME = UI.FormatAsLink("Mushroom Quiche", "QUICHE");

				// Token: 0x0400D426 RID: 54310
				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust.\n\nSomehow, it's both soggy <i>and</i> crispy."
				});

				// Token: 0x0400D427 RID: 54311
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust."
				});

				// Token: 0x02003841 RID: 14401
				public class DEHYDRATED
				{
					// Token: 0x0400DE97 RID: 56983
					public static LocString NAME = "Dried Mushroom Quiche";

					// Token: 0x0400DE98 RID: 56984
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Mushroom Quiche", "QUICHE"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}
		}

		// Token: 0x020021C0 RID: 8640
		public class INGREDIENTS
		{
			// Token: 0x0200341E RID: 13342
			public class SWAMPLILYFLOWER
			{
				// Token: 0x0400D428 RID: 54312
				public static LocString NAME = UI.FormatAsLink("Balm Lily Flower", "SWAMPLILYFLOWER");

				// Token: 0x0400D429 RID: 54313
				public static LocString DESC = "A medicinal flower that soothes most minor maladies.\n\nIt is exceptionally fragrant.";
			}

			// Token: 0x0200341F RID: 13343
			public class GINGER
			{
				// Token: 0x0400D42A RID: 54314
				public static LocString NAME = UI.FormatAsLink("Tonic Root", "GINGERCONFIG");

				// Token: 0x0400D42B RID: 54315
				public static LocString DESC = "A chewy, fibrous rhizome with a fiery aftertaste.";
			}
		}

		// Token: 0x020021C1 RID: 8641
		public class INDUSTRIAL_PRODUCTS
		{
			// Token: 0x02003420 RID: 13344
			public class ELECTROBANK_MUCKROOT
			{
				// Token: 0x0400D42C RID: 54316
				public static LocString NAME = UI.FormatAsLink("Muckroot Power Bank", "ELECTROBANK");

				// Token: 0x0400D42D RID: 54317
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable organic ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Muckroot", "BASICFORAGEPLANT"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Muckroot Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02003421 RID: 13345
			public class ELECTROBANK_CARROT
			{
				// Token: 0x0400D42E RID: 54318
				public static LocString NAME = UI.FormatAsLink("Squash Power Bank", "ELECTROBANK");

				// Token: 0x0400D42F RID: 54319
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable organic ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Plume Squash", "CARROT"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Squash Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02003422 RID: 13346
			public class ELECTROBANK_LIGHTBUGEGG
			{
				// Token: 0x0400D430 RID: 54320
				public static LocString NAME = UI.FormatAsLink("Shine Egg Power Bank", "ELECTROBANK");

				// Token: 0x0400D431 RID: 54321
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable organic ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Shine Nymph Egg", "LIGHTBUG"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Shine Egg Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02003423 RID: 13347
			public class ELECTROBANK_SUCROSE
			{
				// Token: 0x0400D432 RID: 54322
				public static LocString NAME = UI.FormatAsLink("Sucrose Power Bank", "ELECTROBANK");

				// Token: 0x0400D433 RID: 54323
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable organic ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Sucrose Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02003424 RID: 13348
			public class ELECTROBANK_STATERPILLAR
			{
				// Token: 0x0400D434 RID: 54324
				public static LocString NAME = UI.FormatAsLink("Slug Egg Power Bank", "ELECTROBANK");

				// Token: 0x0400D435 RID: 54325
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable organic ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Plug Slug Egg", "STATERPILLAR"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Slug Egg Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02003425 RID: 13349
			public class ELECTROBANK_URANIUM_ORE
			{
				// Token: 0x0400D436 RID: 54326
				public static LocString NAME = UI.FormatAsLink("Nuclear Power Bank", "ELECTROBANK");

				// Token: 0x0400D437 RID: 54327
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Nuclear Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02003426 RID: 13350
			public class ELECTROBANK_METAL_ORE
			{
				// Token: 0x0400D438 RID: 54328
				public static LocString NAME = UI.FormatAsLink("Metal Power Bank", "ELECTROBANK");

				// Token: 0x0400D439 RID: 54329
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Metal Ore", "METAL"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Metal Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02003427 RID: 13351
			public class ELECTROBANK
			{
				// Token: 0x0400D43A RID: 54330
				public static LocString NAME = UI.FormatAsLink("Eco Power Bank", "ELECTROBANK");

				// Token: 0x0400D43B RID: 54331
				public static LocString DESC = string.Concat(new string[]
				{
					"A rechargeable ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Eco Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Soldering Station", "ADVANCEDCRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02003428 RID: 13352
			public class ELECTROBANK_EMPTY
			{
				// Token: 0x0400D43C RID: 54332
				public static LocString NAME = UI.FormatAsLink("Empty Eco Power Bank", "ELECTROBANK");

				// Token: 0x0400D43D RID: 54333
				public static LocString DESC = string.Concat(new string[]
				{
					"A depleted ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					".\n\nIt must be recharged at a ",
					UI.FormatAsLink("Power Bank Charger", "ELECTROBANKCHARGER"),
					" before it can be reused."
				});
			}

			// Token: 0x02003429 RID: 13353
			public class ELECTROBANK_GARBAGE
			{
				// Token: 0x0400D43E RID: 54334
				public static LocString NAME = UI.FormatAsLink("Power Bank Scrap", "ELECTROBANK");

				// Token: 0x0400D43F RID: 54335
				public static LocString DESC = "A " + UI.FormatAsLink("Power Bank", "ELECTROBANK") + " that has reached the end of its life cycle.\n\nIt can be salvaged for metal ore.";
			}

			// Token: 0x0200342A RID: 13354
			public class FUEL_BRICK
			{
				// Token: 0x0400D440 RID: 54336
				public static LocString NAME = "Fuel Brick";

				// Token: 0x0400D441 RID: 54337
				public static LocString DESC = "A densely compressed brick of combustible material.\n\nIt can be burned to produce a one-time burst of " + UI.FormatAsLink("Power", "POWER") + ".";
			}

			// Token: 0x0200342B RID: 13355
			public class BASIC_FABRIC
			{
				// Token: 0x0400D442 RID: 54338
				public static LocString NAME = UI.FormatAsLink("Reed Fiber", "BASIC_FABRIC");

				// Token: 0x0400D443 RID: 54339
				public static LocString DESC = "A ball of raw cellulose used in the production of " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " and textiles.";
			}

			// Token: 0x0200342C RID: 13356
			public class TRAP_PARTS
			{
				// Token: 0x0400D444 RID: 54340
				public static LocString NAME = "Trap Components";

				// Token: 0x0400D445 RID: 54341
				public static LocString DESC = string.Concat(new string[]
				{
					"These components can be assembled into a ",
					BUILDINGS.PREFABS.CREATURETRAP.NAME,
					" and used to catch ",
					UI.FormatAsLink("Critters", "CREATURES"),
					"."
				});
			}

			// Token: 0x0200342D RID: 13357
			public class POWER_STATION_TOOLS
			{
				// Token: 0x0400D446 RID: 54342
				public static LocString NAME = "Microchip";

				// Token: 0x0400D447 RID: 54343
				public static LocString DESC = string.Concat(new string[]
				{
					"A specialized ",
					ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME,
					" created by a professional engineer.\n\nTunes up ",
					UI.PRE_KEYWORD,
					"Generators",
					UI.PST_KEYWORD,
					" to increase their ",
					UI.FormatAsLink("Power", "POWER"),
					" output."
				});

				// Token: 0x0400D448 RID: 54344
				public static LocString TINKER_REQUIREMENT_NAME = "Skill: " + DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME;

				// Token: 0x0400D449 RID: 54345
				public static LocString TINKER_REQUIREMENT_TOOLTIP = string.Concat(new string[]
				{
					"Can only be used by a Duplicant with ",
					DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME,
					" to apply a ",
					UI.PRE_KEYWORD,
					"Tune Up",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D44A RID: 54346
				public static LocString TINKER_EFFECT_NAME = "Engie's Tune-Up: {0} {1}";

				// Token: 0x0400D44B RID: 54347
				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Tune Up",
					UI.PST_KEYWORD,
					" a generator, increasing its {0} by <b>{1}</b>."
				});

				// Token: 0x0400D44C RID: 54348
				public static LocString RECIPE_DESCRIPTION = "Make " + ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME + " from {0}";
			}

			// Token: 0x0200342E RID: 13358
			public class FARM_STATION_TOOLS
			{
				// Token: 0x0400D44D RID: 54349
				public static LocString NAME = "Micronutrient Fertilizer";

				// Token: 0x0400D44E RID: 54350
				public static LocString DESC = string.Concat(new string[]
				{
					"Specialized ",
					UI.FormatAsLink("Fertilizer", "FERTILIZER"),
					" mixed by a Duplicant with the ",
					DUPLICANTS.ROLES.FARMER.NAME,
					" Skill.\n\nIncreases the ",
					UI.PRE_KEYWORD,
					"Growth Rate",
					UI.PST_KEYWORD,
					" of one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					"."
				});
			}

			// Token: 0x0200342F RID: 13359
			public class MACHINE_PARTS
			{
				// Token: 0x0400D44F RID: 54351
				public static LocString NAME = "Custom Parts";

				// Token: 0x0400D450 RID: 54352
				public static LocString DESC = string.Concat(new string[]
				{
					"Specialized Parts crafted by a professional engineer.\n\n",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" machine buildings to increase their efficiency."
				});

				// Token: 0x0400D451 RID: 54353
				public static LocString TINKER_REQUIREMENT_NAME = "Job: " + DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME;

				// Token: 0x0400D452 RID: 54354
				public static LocString TINKER_REQUIREMENT_TOOLTIP = string.Concat(new string[]
				{
					"Can only be used by a Duplicant with ",
					DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME,
					" to apply a ",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D453 RID: 54355
				public static LocString TINKER_EFFECT_NAME = "Engineer's Jerry Rig: {0} {1}";

				// Token: 0x0400D454 RID: 54356
				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" upgrades to a machine building, increasing its {0} by <b>{1}</b>."
				});
			}

			// Token: 0x02003430 RID: 13360
			public class RESEARCH_DATABANK
			{
				// Token: 0x0400D455 RID: 54357
				public static LocString NAME = UI.FormatAsLink("Data Bank", "RESEARCH_DATABANK");

				// Token: 0x0400D456 RID: 54358
				public static LocString DESC = "Raw data that can be processed into " + UI.FormatAsLink("Interstellar Research", "RESEARCH") + " points.";
			}

			// Token: 0x02003431 RID: 13361
			public class ORBITAL_RESEARCH_DATABANK
			{
				// Token: 0x0400D457 RID: 54359
				public static LocString NAME = UI.FormatAsLink("Data Bank", "ORBITAL_RESEARCH_DATABANK");

				// Token: 0x0400D458 RID: 54360
				public static LocString DESC = "Raw Data that can be processed into " + UI.FormatAsLink("Data Analysis Research", "RESEARCH") + " points.";

				// Token: 0x0400D459 RID: 54361
				public static LocString RECIPE_DESC = string.Concat(new string[]
				{
					"Data Banks of raw data generated from exploring, either by exploring new areas with Duplicants, or by using an ",
					UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER"),
					".\n\nUsed by the ",
					UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER"),
					" to conduct research."
				});
			}

			// Token: 0x02003432 RID: 13362
			public class EGG_SHELL
			{
				// Token: 0x0400D45A RID: 54362
				public static LocString NAME = UI.FormatAsLink("Egg Shell", "EGG_SHELL");

				// Token: 0x0400D45B RID: 54363
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";
			}

			// Token: 0x02003433 RID: 13363
			public class GOLD_BELLY_CROWN
			{
				// Token: 0x0400D45C RID: 54364
				public static LocString NAME = UI.FormatAsLink("Regal Bammoth Crest", "GOLD_BELLY_CROWN");

				// Token: 0x0400D45D RID: 54365
				public static LocString DESC = "Can be crushed to produce " + ELEMENTS.GOLDAMALGAM.NAME + ".";
			}

			// Token: 0x02003434 RID: 13364
			public class CRAB_SHELL
			{
				// Token: 0x0400D45E RID: 54366
				public static LocString NAME = UI.FormatAsLink("Pokeshell Molt", "CRAB_SHELL");

				// Token: 0x0400D45F RID: 54367
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";

				// Token: 0x02003842 RID: 14402
				public class VARIANT_WOOD
				{
					// Token: 0x0400DE99 RID: 56985
					public static LocString NAME = UI.FormatAsLink("Oakshell Molt", "VARIANT_WOOD_SHELL");

					// Token: 0x0400DE9A RID: 56986
					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Wood", "WOOD") + ".";
				}
			}

			// Token: 0x02003435 RID: 13365
			public class BABY_CRAB_SHELL
			{
				// Token: 0x0400D460 RID: 54368
				public static LocString NAME = UI.FormatAsLink("Small Pokeshell Molt", "CRAB_SHELL");

				// Token: 0x0400D461 RID: 54369
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";

				// Token: 0x02003843 RID: 14403
				public class VARIANT_WOOD
				{
					// Token: 0x0400DE9B RID: 56987
					public static LocString NAME = UI.FormatAsLink("Small Oakshell Molt", "VARIANT_WOOD_SHELL");

					// Token: 0x0400DE9C RID: 56988
					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Wood", "WOOD") + ".";
				}
			}

			// Token: 0x02003436 RID: 13366
			public class WOOD
			{
				// Token: 0x0400D462 RID: 54370
				public static LocString NAME = UI.FormatAsLink("Wood", "WOOD");

				// Token: 0x0400D463 RID: 54371
				public static LocString DESC = string.Concat(new string[]
				{
					"Natural resource harvested from certain ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" and ",
					UI.FormatAsLink("Plants", "PLANTS"),
					".\n\nUsed in construction or ",
					UI.FormatAsLink("Heat", "HEAT"),
					" production."
				});
			}

			// Token: 0x02003437 RID: 13367
			public class GENE_SHUFFLER_RECHARGE
			{
				// Token: 0x0400D464 RID: 54372
				public static LocString NAME = "Vacillator Recharge";

				// Token: 0x0400D465 RID: 54373
				public static LocString DESC = "Replenishes one charge to a depleted " + BUILDINGS.PREFABS.GENESHUFFLER.NAME + ".";
			}

			// Token: 0x02003438 RID: 13368
			public class TABLE_SALT
			{
				// Token: 0x0400D466 RID: 54374
				public static LocString NAME = "Table Salt";

				// Token: 0x0400D467 RID: 54375
				public static LocString DESC = string.Concat(new string[]
				{
					"A seasoning that Duplicants can add to their ",
					UI.FormatAsLink("Food", "FOOD"),
					" to boost ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nDuplicants will automatically use Table Salt while sitting at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME,
					" during mealtime.\n\n<i>Only the finest grains are chosen.</i>"
				});
			}

			// Token: 0x02003439 RID: 13369
			public class REFINED_SUGAR
			{
				// Token: 0x0400D468 RID: 54376
				public static LocString NAME = "Refined Sugar";

				// Token: 0x0400D469 RID: 54377
				public static LocString DESC = string.Concat(new string[]
				{
					"A seasoning that Duplicants can add to their ",
					UI.FormatAsLink("Food", "FOOD"),
					" to boost ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nDuplicants will automatically use Refined Sugar while sitting at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME,
					" during mealtime.\n\n<i>Only the finest grains are chosen.</i>"
				});
			}

			// Token: 0x0200343A RID: 13370
			public class ICE_BELLY_POOP
			{
				// Token: 0x0400D46A RID: 54378
				public static LocString NAME = UI.FormatAsLink("Bammoth Patty", "ICE_BELLY_POOP");

				// Token: 0x0400D46B RID: 54379
				public static LocString DESC = string.Concat(new string[]
				{
					"A little treat left behind by a very large critter.\n\nIt can be crushed to extract ",
					UI.FormatAsLink("Phosphorite", "PHOSPHORITE"),
					" and ",
					UI.FormatAsLink("Clay", "CLAY"),
					"."
				});
			}
		}

		// Token: 0x020021C2 RID: 8642
		public class CARGO_CAPSULE
		{
			// Token: 0x040099BB RID: 39355
			public static LocString NAME = "Care Package";

			// Token: 0x040099BC RID: 39356
			public static LocString DESC = "A delivery system for recently printed resources.\n\nIt will dematerialize shortly.";
		}

		// Token: 0x020021C3 RID: 8643
		public class RAILGUNPAYLOAD
		{
			// Token: 0x040099BD RID: 39357
			public static LocString NAME = UI.FormatAsLink("Interplanetary Payload", "RAILGUNPAYLOAD");

			// Token: 0x040099BE RID: 39358
			public static LocString DESC = string.Concat(new string[]
			{
				"Contains resources packed for interstellar shipping.\n\nCan be launched by a ",
				BUILDINGS.PREFABS.RAILGUN.NAME,
				" or unpacked with a ",
				BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME,
				"."
			});
		}

		// Token: 0x020021C4 RID: 8644
		public class MISSILE_BASIC
		{
			// Token: 0x040099BF RID: 39359
			public static LocString NAME = UI.FormatAsLink("Blastshot", "MISSILELAUNCHER");

			// Token: 0x040099C0 RID: 39360
			public static LocString DESC = "An explosive projectile designed to defend against meteor showers.\n\nMust be launched by a " + UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER") + ".";
		}

		// Token: 0x020021C5 RID: 8645
		public class DEBRISPAYLOAD
		{
			// Token: 0x040099C1 RID: 39361
			public static LocString NAME = "Rocket Debris";

			// Token: 0x040099C2 RID: 39362
			public static LocString DESC = "Whatever is left over from a Rocket Self-Destruct can be recovered once it has crash-landed.";
		}

		// Token: 0x020021C6 RID: 8646
		public class RADIATION
		{
			// Token: 0x0200343B RID: 13371
			public class HIGHENERGYPARITCLE
			{
				// Token: 0x0400D46C RID: 54380
				public static LocString NAME = "Radbolts";

				// Token: 0x0400D46D RID: 54381
				public static LocString DESC = string.Concat(new string[]
				{
					"A concentrated field of ",
					UI.FormatAsKeyWord("Radbolts"),
					" that can be largely redirected using a ",
					UI.FormatAsLink("Radbolt Reflector", "HIGHENERGYPARTICLEREDIRECTOR"),
					"."
				});
			}
		}

		// Token: 0x020021C7 RID: 8647
		public class DREAMJOURNAL
		{
			// Token: 0x040099C3 RID: 39363
			public static LocString NAME = "Dream Journal";

			// Token: 0x040099C4 RID: 39364
			public static LocString DESC = string.Concat(new string[]
			{
				"A hand-scrawled account of ",
				UI.FormatAsLink("Pajama", "SLEEP_CLINIC_PAJAMAS"),
				"-induced dreams.\n\nCan be analyzed using a ",
				UI.FormatAsLink("Somnium Synthesizer", "MEGABRAINTANK"),
				"."
			});
		}

		// Token: 0x020021C8 RID: 8648
		public class DEHYDRATEDFOODPACKAGE
		{
			// Token: 0x040099C5 RID: 39365
			public static LocString NAME = "Dry Ration";

			// Token: 0x040099C6 RID: 39366
			public static LocString DESC = "A package of non-perishable dehydrated food.\n\nIt requires no refrigeration, but must be rehydrated before consumption.";

			// Token: 0x040099C7 RID: 39367
			public static LocString CONSUMED = "Ate Rehydrated Food";

			// Token: 0x040099C8 RID: 39368
			public static LocString CONTENTS = "Dried {0}";
		}

		// Token: 0x020021C9 RID: 8649
		public class SPICES
		{
			// Token: 0x0200343C RID: 13372
			public class MACHINERY_SPICE
			{
				// Token: 0x0400D46E RID: 54382
				public static LocString NAME = UI.FormatAsLink("Machinist Spice", "MACHINERY_SPICE");

				// Token: 0x0400D46F RID: 54383
				public static LocString DESC = "Improves operating skills when ingested.";
			}

			// Token: 0x0200343D RID: 13373
			public class PILOTING_SPICE
			{
				// Token: 0x0400D470 RID: 54384
				public static LocString NAME = UI.FormatAsLink("Rocketeer Spice", "PILOTING_SPICE");

				// Token: 0x0400D471 RID: 54385
				public static LocString DESC = "Provides a boost to piloting abilities.";
			}

			// Token: 0x0200343E RID: 13374
			public class PRESERVING_SPICE
			{
				// Token: 0x0400D472 RID: 54386
				public static LocString NAME = UI.FormatAsLink("Freshener Spice", "PRESERVING_SPICE");

				// Token: 0x0400D473 RID: 54387
				public static LocString DESC = "Slows the decomposition of perishable foods.";
			}

			// Token: 0x0200343F RID: 13375
			public class STRENGTH_SPICE
			{
				// Token: 0x0400D474 RID: 54388
				public static LocString NAME = UI.FormatAsLink("Brawny Spice", "STRENGTH_SPICE");

				// Token: 0x0400D475 RID: 54389
				public static LocString DESC = "Strengthens even the weakest of muscles.";
			}
		}
	}
}
