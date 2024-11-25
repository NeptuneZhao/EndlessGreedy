using System;

namespace STRINGS
{
	// Token: 0x02000F0D RID: 3853
	public class EQUIPMENT
	{
		// Token: 0x020020D8 RID: 8408
		public class PREFABS
		{
			// Token: 0x02002990 RID: 10640
			public class OXYGEN_MASK
			{
				// Token: 0x0400B4CF RID: 46287
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK");

				// Token: 0x0400B4D0 RID: 46288
				public static LocString DESC = "Ensures my Duplicants can breathe easy... for a little while, anyways.";

				// Token: 0x0400B4D1 RID: 46289
				public static LocString EFFECT = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.\n\nMust be refilled with oxygen at an " + UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER") + " when depleted.";

				// Token: 0x0400B4D2 RID: 46290
				public static LocString RECIPE_DESC = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.";

				// Token: 0x0400B4D3 RID: 46291
				public static LocString GENERICNAME = "Suit";

				// Token: 0x0400B4D4 RID: 46292
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Oxygen Mask", "OXYGEN_MASK");

				// Token: 0x0400B4D5 RID: 46293
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK"),
					".\n\nMasks can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02002991 RID: 10641
			public class ATMO_SUIT
			{
				// Token: 0x0400B4D6 RID: 46294
				public static LocString NAME = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT");

				// Token: 0x0400B4D7 RID: 46295
				public static LocString DESC = "Ensures my Duplicants can breathe easy, anytime, anywhere.";

				// Token: 0x0400B4D8 RID: 46296
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments, and protects against extreme temperatures.\n\nMust be refilled with oxygen at an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400B4D9 RID: 46297
				public static LocString RECIPE_DESC = "Supplies Duplicants with " + UI.FormatAsLink("Oxygen", "OXYGEN") + "  in toxic and low breathability environments.";

				// Token: 0x0400B4DA RID: 46298
				public static LocString GENERICNAME = "Suit";

				// Token: 0x0400B4DB RID: 46299
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Atmo Suit", "ATMO_SUIT");

				// Token: 0x0400B4DC RID: 46300
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Atmo Suit", "ATMO_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});

				// Token: 0x0400B4DD RID: 46301
				public static LocString REPAIR_WORN_RECIPE_NAME = "Repair " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400B4DE RID: 46302
				public static LocString REPAIR_WORN_DESC = "Restore a " + UI.FormatAsLink("Worn Atmo Suit", "ATMO_SUIT") + " to working order.";
			}

			// Token: 0x02002992 RID: 10642
			public class ATMO_SUIT_SET
			{
				// Token: 0x020035E3 RID: 13795
				public class PUFT
				{
					// Token: 0x0400D91A RID: 55578
					public static LocString NAME = "Puft Atmo Suit";

					// Token: 0x0400D91B RID: 55579
					public static LocString DESC = "Critter-forward protective gear for the intrepid explorer!\nReleased for Klei Fest 2023.";
				}
			}

			// Token: 0x02002993 RID: 10643
			public class HOLIDAY_2023_CRATE
			{
				// Token: 0x0400B4DF RID: 46303
				public static LocString NAME = "Holiday Gift Crate";

				// Token: 0x0400B4E0 RID: 46304
				public static LocString DESC = "An unaddressed package has been discovered near the Printing Pod. It exudes seasonal cheer, and trace amounts of Neutronium have been detected.";
			}

			// Token: 0x02002994 RID: 10644
			public class ATMO_SUIT_HELMET
			{
				// Token: 0x0400B4E1 RID: 46305
				public static LocString NAME = "Default Atmo Helmet";

				// Token: 0x0400B4E2 RID: 46306
				public static LocString DESC = "Default helmet for atmo suits.";

				// Token: 0x020035E4 RID: 13796
				public class FACADES
				{
					// Token: 0x020039B6 RID: 14774
					public class SPARKLE_RED
					{
						// Token: 0x0400E261 RID: 57953
						public static LocString NAME = "Red Glitter Atmo Helmet";

						// Token: 0x0400E262 RID: 57954
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x020039B7 RID: 14775
					public class SPARKLE_GREEN
					{
						// Token: 0x0400E263 RID: 57955
						public static LocString NAME = "Green Glitter Atmo Helmet";

						// Token: 0x0400E264 RID: 57956
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x020039B8 RID: 14776
					public class SPARKLE_BLUE
					{
						// Token: 0x0400E265 RID: 57957
						public static LocString NAME = "Blue Glitter Atmo Helmet";

						// Token: 0x0400E266 RID: 57958
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x020039B9 RID: 14777
					public class SPARKLE_PURPLE
					{
						// Token: 0x0400E267 RID: 57959
						public static LocString NAME = "Violet Glitter Atmo Helmet";

						// Token: 0x0400E268 RID: 57960
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x020039BA RID: 14778
					public class LIMONE
					{
						// Token: 0x0400E269 RID: 57961
						public static LocString NAME = "Citrus Atmo Helmet";

						// Token: 0x0400E26A RID: 57962
						public static LocString DESC = "Fresh, fruity and full of breathable air.";
					}

					// Token: 0x020039BB RID: 14779
					public class PUFT
					{
						// Token: 0x0400E26B RID: 57963
						public static LocString NAME = "Puft Atmo Helmet";

						// Token: 0x0400E26C RID: 57964
						public static LocString DESC = "Convincing enough to fool most Pufts and even a few Duplicants.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x020039BC RID: 14780
					public class CLUBSHIRT_PURPLE
					{
						// Token: 0x0400E26D RID: 57965
						public static LocString NAME = "Eggplant Atmo Helmet";

						// Token: 0x0400E26E RID: 57966
						public static LocString DESC = "It is neither an egg, nor a plant. But it <i>is</i> a functional helmet.";
					}

					// Token: 0x020039BD RID: 14781
					public class TRIANGLES_TURQ
					{
						// Token: 0x0400E26F RID: 57967
						public static LocString NAME = "Confetti Atmo Helmet";

						// Token: 0x0400E270 RID: 57968
						public static LocString DESC = "Doubles as a party hat.";
					}

					// Token: 0x020039BE RID: 14782
					public class CUMMERBUND_RED
					{
						// Token: 0x0400E271 RID: 57969
						public static LocString NAME = "Blastoff Atmo Helmet";

						// Token: 0x0400E272 RID: 57970
						public static LocString DESC = "Red means go!";
					}

					// Token: 0x020039BF RID: 14783
					public class WORKOUT_LAVENDER
					{
						// Token: 0x0400E273 RID: 57971
						public static LocString NAME = "Pink Punch Atmo Helmet";

						// Token: 0x0400E274 RID: 57972
						public static LocString DESC = "Unapologetically ostentatious.";
					}

					// Token: 0x020039C0 RID: 14784
					public class CANTALOUPE
					{
						// Token: 0x0400E275 RID: 57973
						public static LocString NAME = "Rocketmelon Atmo Helmet";

						// Token: 0x0400E276 RID: 57974
						public static LocString DESC = "A melon for your melon.";
					}

					// Token: 0x020039C1 RID: 14785
					public class MONDRIAN_BLUE_RED_YELLOW
					{
						// Token: 0x0400E277 RID: 57975
						public static LocString NAME = "Cubist Atmo Helmet";

						// Token: 0x0400E278 RID: 57976
						public static LocString DESC = "Abstract geometrics are both hip <i>and</i> square.";
					}

					// Token: 0x020039C2 RID: 14786
					public class OVERALLS_RED
					{
						// Token: 0x0400E279 RID: 57977
						public static LocString NAME = "Spiffy Atmo Helmet";

						// Token: 0x0400E27A RID: 57978
						public static LocString DESC = "The twin antennae serve as an early warning system for low ceilings.";
					}
				}
			}

			// Token: 0x02002995 RID: 10645
			public class ATMO_SUIT_BODY
			{
				// Token: 0x0400B4E3 RID: 46307
				public static LocString NAME = "Default Atmo Uniform";

				// Token: 0x0400B4E4 RID: 46308
				public static LocString DESC = "Default top and bottom of an atmo suit.";

				// Token: 0x020035E5 RID: 13797
				public class FACADES
				{
					// Token: 0x020039C3 RID: 14787
					public class SPARKLE_RED
					{
						// Token: 0x0400E27B RID: 57979
						public static LocString NAME = "Red Glitter Atmo Suit";

						// Token: 0x0400E27C RID: 57980
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x020039C4 RID: 14788
					public class SPARKLE_GREEN
					{
						// Token: 0x0400E27D RID: 57981
						public static LocString NAME = "Green Glitter Atmo Suit";

						// Token: 0x0400E27E RID: 57982
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x020039C5 RID: 14789
					public class SPARKLE_BLUE
					{
						// Token: 0x0400E27F RID: 57983
						public static LocString NAME = "Blue Glitter Atmo Suit";

						// Token: 0x0400E280 RID: 57984
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x020039C6 RID: 14790
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400E281 RID: 57985
						public static LocString NAME = "Violet Glitter Atmo Suit";

						// Token: 0x0400E282 RID: 57986
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x020039C7 RID: 14791
					public class LIMONE
					{
						// Token: 0x0400E283 RID: 57987
						public static LocString NAME = "Citrus Atmo Suit";

						// Token: 0x0400E284 RID: 57988
						public static LocString DESC = "Perfect for summery, atmospheric excursions.";
					}

					// Token: 0x020039C8 RID: 14792
					public class PUFT
					{
						// Token: 0x0400E285 RID: 57989
						public static LocString NAME = "Puft Atmo Suit";

						// Token: 0x0400E286 RID: 57990
						public static LocString DESC = "Warning: prolonged wear may result in feelings of Puft-up pride.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x020039C9 RID: 14793
					public class BASIC_PURPLE
					{
						// Token: 0x0400E287 RID: 57991
						public static LocString NAME = "Crisp Eggplant Atmo Suit";

						// Token: 0x0400E288 RID: 57992
						public static LocString DESC = "It really emphasizes wide shoulders.";
					}

					// Token: 0x020039CA RID: 14794
					public class PRINT_TRIANGLES_TURQ
					{
						// Token: 0x0400E289 RID: 57993
						public static LocString NAME = "Confetti Atmo Suit";

						// Token: 0x0400E28A RID: 57994
						public static LocString DESC = "It puts the \"fun\" in \"perfunctory nods to personnel individuality\"!";
					}

					// Token: 0x020039CB RID: 14795
					public class BASIC_NEON_PINK
					{
						// Token: 0x0400E28B RID: 57995
						public static LocString NAME = "Crisp Neon Pink Atmo Suit";

						// Token: 0x0400E28C RID: 57996
						public static LocString DESC = "The neck is a little snug.";
					}

					// Token: 0x020039CC RID: 14796
					public class MULTI_RED_BLACK
					{
						// Token: 0x0400E28D RID: 57997
						public static LocString NAME = "Red-bellied Atmo Suit";

						// Token: 0x0400E28E RID: 57998
						public static LocString DESC = "It really highlights the midsection.";
					}

					// Token: 0x020039CD RID: 14797
					public class CANTALOUPE
					{
						// Token: 0x0400E28F RID: 57999
						public static LocString NAME = "Rocketmelon Atmo Suit";

						// Token: 0x0400E290 RID: 58000
						public static LocString DESC = "It starts to smell ripe pretty quickly.";
					}

					// Token: 0x020039CE RID: 14798
					public class MULTI_BLUE_GREY_BLACK
					{
						// Token: 0x0400E291 RID: 58001
						public static LocString NAME = "Swagger Atmo Suit";

						// Token: 0x0400E292 RID: 58002
						public static LocString DESC = "Engineered to resemble stonewashed denim and black leather.";
					}

					// Token: 0x020039CF RID: 14799
					public class MULTI_BLUE_YELLOW_RED
					{
						// Token: 0x0400E293 RID: 58003
						public static LocString NAME = "Fundamental Stripe Atmo Suit";

						// Token: 0x0400E294 RID: 58004
						public static LocString DESC = "Designed by the Primary Colors Appreciation Society.";
					}
				}
			}

			// Token: 0x02002996 RID: 10646
			public class ATMO_SUIT_GLOVES
			{
				// Token: 0x0400B4E5 RID: 46309
				public static LocString NAME = "Default Atmo Gloves";

				// Token: 0x0400B4E6 RID: 46310
				public static LocString DESC = "Default atmo suit gloves.";

				// Token: 0x020035E6 RID: 13798
				public class FACADES
				{
					// Token: 0x020039D0 RID: 14800
					public class SPARKLE_RED
					{
						// Token: 0x0400E295 RID: 58005
						public static LocString NAME = "Red Glitter Atmo Gloves";

						// Token: 0x0400E296 RID: 58006
						public static LocString DESC = "Sparkly red gloves for hostile environments.";
					}

					// Token: 0x020039D1 RID: 14801
					public class SPARKLE_GREEN
					{
						// Token: 0x0400E297 RID: 58007
						public static LocString NAME = "Green Glitter Atmo Gloves";

						// Token: 0x0400E298 RID: 58008
						public static LocString DESC = "Sparkly green gloves for hostile environments.";
					}

					// Token: 0x020039D2 RID: 14802
					public class SPARKLE_BLUE
					{
						// Token: 0x0400E299 RID: 58009
						public static LocString NAME = "Blue Glitter Atmo Gloves";

						// Token: 0x0400E29A RID: 58010
						public static LocString DESC = "Sparkly blue gloves for hostile environments.";
					}

					// Token: 0x020039D3 RID: 14803
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400E29B RID: 58011
						public static LocString NAME = "Violet Glitter Atmo Gloves";

						// Token: 0x0400E29C RID: 58012
						public static LocString DESC = "Sparkly violet gloves for hostile environments.";
					}

					// Token: 0x020039D4 RID: 14804
					public class LIMONE
					{
						// Token: 0x0400E29D RID: 58013
						public static LocString NAME = "Citrus Atmo Gloves";

						// Token: 0x0400E29E RID: 58014
						public static LocString DESC = "Lime-inspired gloves brighten up hostile environments.";
					}

					// Token: 0x020039D5 RID: 14805
					public class PUFT
					{
						// Token: 0x0400E29F RID: 58015
						public static LocString NAME = "Puft Atmo Gloves";

						// Token: 0x0400E2A0 RID: 58016
						public static LocString DESC = "A little Puft-love for delicate extremities.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x020039D6 RID: 14806
					public class GOLD
					{
						// Token: 0x0400E2A1 RID: 58017
						public static LocString NAME = "Gold Atmo Gloves";

						// Token: 0x0400E2A2 RID: 58018
						public static LocString DESC = "A golden touch! Without all the Midas-type baggage.";
					}

					// Token: 0x020039D7 RID: 14807
					public class PURPLE
					{
						// Token: 0x0400E2A3 RID: 58019
						public static LocString NAME = "Eggplant Atmo Gloves";

						// Token: 0x0400E2A4 RID: 58020
						public static LocString DESC = "Fab purple gloves for hostile environments.";
					}

					// Token: 0x020039D8 RID: 14808
					public class WHITE
					{
						// Token: 0x0400E2A5 RID: 58021
						public static LocString NAME = "White Atmo Gloves";

						// Token: 0x0400E2A6 RID: 58022
						public static LocString DESC = "For the Duplicant who never gets their hands dirty.";
					}

					// Token: 0x020039D9 RID: 14809
					public class STRIPES_LAVENDER
					{
						// Token: 0x0400E2A7 RID: 58023
						public static LocString NAME = "Wildberry Atmo Gloves";

						// Token: 0x0400E2A8 RID: 58024
						public static LocString DESC = "Functional finger-protectors with fruity flair.";
					}

					// Token: 0x020039DA RID: 14810
					public class CANTALOUPE
					{
						// Token: 0x0400E2A9 RID: 58025
						public static LocString NAME = "Rocketmelon Atmo Gloves";

						// Token: 0x0400E2AA RID: 58026
						public static LocString DESC = "It takes eighteen melon rinds to make a single glove.";
					}

					// Token: 0x020039DB RID: 14811
					public class BROWN
					{
						// Token: 0x0400E2AB RID: 58027
						public static LocString NAME = "Leather Atmo Gloves";

						// Token: 0x0400E2AC RID: 58028
						public static LocString DESC = "They creak rather loudly during the break-in period.";
					}
				}
			}

			// Token: 0x02002997 RID: 10647
			public class ATMO_SUIT_BELT
			{
				// Token: 0x0400B4E7 RID: 46311
				public static LocString NAME = "Default Atmo Belt";

				// Token: 0x0400B4E8 RID: 46312
				public static LocString DESC = "Default belt for atmo suits.";

				// Token: 0x020035E7 RID: 13799
				public class FACADES
				{
					// Token: 0x020039DC RID: 14812
					public class SPARKLE_RED
					{
						// Token: 0x0400E2AD RID: 58029
						public static LocString NAME = "Red Glitter Atmo Belt";

						// Token: 0x0400E2AE RID: 58030
						public static LocString DESC = "It's red! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x020039DD RID: 14813
					public class SPARKLE_GREEN
					{
						// Token: 0x0400E2AF RID: 58031
						public static LocString NAME = "Green Glitter Atmo Belt";

						// Token: 0x0400E2B0 RID: 58032
						public static LocString DESC = "It's green! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x020039DE RID: 14814
					public class SPARKLE_BLUE
					{
						// Token: 0x0400E2B1 RID: 58033
						public static LocString NAME = "Blue Glitter Atmo Belt";

						// Token: 0x0400E2B2 RID: 58034
						public static LocString DESC = "It's blue! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x020039DF RID: 14815
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400E2B3 RID: 58035
						public static LocString NAME = "Violet Glitter Atmo Belt";

						// Token: 0x0400E2B4 RID: 58036
						public static LocString DESC = "It's violet! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x020039E0 RID: 14816
					public class LIMONE
					{
						// Token: 0x0400E2B5 RID: 58037
						public static LocString NAME = "Citrus Atmo Belt";

						// Token: 0x0400E2B6 RID: 58038
						public static LocString DESC = "This lime-hued belt really pulls an atmo suit together.";
					}

					// Token: 0x020039E1 RID: 14817
					public class PUFT
					{
						// Token: 0x0400E2B7 RID: 58039
						public static LocString NAME = "Puft Atmo Belt";

						// Token: 0x0400E2B8 RID: 58040
						public static LocString DESC = "If critters wore belts...\nReleased for Klei Fest 2023.";
					}

					// Token: 0x020039E2 RID: 14818
					public class TWOTONE_PURPLE
					{
						// Token: 0x0400E2B9 RID: 58041
						public static LocString NAME = "Eggplant Atmo Belt";

						// Token: 0x0400E2BA RID: 58042
						public static LocString DESC = "In the more pretentious space-fashion circles, it's known as \"aubergine.\"";
					}

					// Token: 0x020039E3 RID: 14819
					public class BASIC_GOLD
					{
						// Token: 0x0400E2BB RID: 58043
						public static LocString NAME = "Gold Atmo Belt";

						// Token: 0x0400E2BC RID: 58044
						public static LocString DESC = "Better to be overdressed than underdressed.";
					}

					// Token: 0x020039E4 RID: 14820
					public class BASIC_GREY
					{
						// Token: 0x0400E2BD RID: 58045
						public static LocString NAME = "Slate Atmo Belt";

						// Token: 0x0400E2BE RID: 58046
						public static LocString DESC = "Slick and understated space style.";
					}

					// Token: 0x020039E5 RID: 14821
					public class BASIC_NEON_PINK
					{
						// Token: 0x0400E2BF RID: 58047
						public static LocString NAME = "Neon Pink Atmo Belt";

						// Token: 0x0400E2C0 RID: 58048
						public static LocString DESC = "Visible from several planetoids away.";
					}

					// Token: 0x020039E6 RID: 14822
					public class CANTALOUPE
					{
						// Token: 0x0400E2C1 RID: 58049
						public static LocString NAME = "Rocketmelon Atmo Belt";

						// Token: 0x0400E2C2 RID: 58050
						public static LocString DESC = "A tribute to the <i>cucumis melo cantalupensis</i>.";
					}

					// Token: 0x020039E7 RID: 14823
					public class TWOTONE_BROWN
					{
						// Token: 0x0400E2C3 RID: 58051
						public static LocString NAME = "Leather Atmo Belt";

						// Token: 0x0400E2C4 RID: 58052
						public static LocString DESC = "Crafted from the tanned hide of a thick-skinned critter.";
					}
				}
			}

			// Token: 0x02002998 RID: 10648
			public class ATMO_SUIT_SHOES
			{
				// Token: 0x0400B4E9 RID: 46313
				public static LocString NAME = "Default Atmo Boots";

				// Token: 0x0400B4EA RID: 46314
				public static LocString DESC = "Default footwear for atmo suits.";

				// Token: 0x020035E8 RID: 13800
				public class FACADES
				{
					// Token: 0x020039E8 RID: 14824
					public class LIMONE
					{
						// Token: 0x0400E2C5 RID: 58053
						public static LocString NAME = "Citrus Atmo Boots";

						// Token: 0x0400E2C6 RID: 58054
						public static LocString DESC = "Cheery boots for stomping around in hostile environments.";
					}

					// Token: 0x020039E9 RID: 14825
					public class PUFT
					{
						// Token: 0x0400E2C7 RID: 58055
						public static LocString NAME = "Puft Atmo Boots";

						// Token: 0x0400E2C8 RID: 58056
						public static LocString DESC = "These boots were made for puft-ing.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x020039EA RID: 14826
					public class SPARKLE_BLACK
					{
						// Token: 0x0400E2C9 RID: 58057
						public static LocString NAME = "Black Glitter Atmo Boots";

						// Token: 0x0400E2CA RID: 58058
						public static LocString DESC = "A timeless color, with a little pizzazz.";
					}

					// Token: 0x020039EB RID: 14827
					public class BASIC_BLACK
					{
						// Token: 0x0400E2CB RID: 58059
						public static LocString NAME = "Stealth Atmo Boots";

						// Token: 0x0400E2CC RID: 58060
						public static LocString DESC = "They attract no attention at all.";
					}

					// Token: 0x020039EC RID: 14828
					public class BASIC_PURPLE
					{
						// Token: 0x0400E2CD RID: 58061
						public static LocString NAME = "Eggplant Atmo Boots";

						// Token: 0x0400E2CE RID: 58062
						public static LocString DESC = "Purple boots for stomping around in hostile environments.";
					}

					// Token: 0x020039ED RID: 14829
					public class BASIC_LAVENDER
					{
						// Token: 0x0400E2CF RID: 58063
						public static LocString NAME = "Lavender Atmo Boots";

						// Token: 0x0400E2D0 RID: 58064
						public static LocString DESC = "Soothing space booties for tired feet.";
					}

					// Token: 0x020039EE RID: 14830
					public class CANTALOUPE
					{
						// Token: 0x0400E2D1 RID: 58065
						public static LocString NAME = "Rocketmelon Atmo Boots";

						// Token: 0x0400E2D2 RID: 58066
						public static LocString DESC = "Keeps feet safe (and juicy) in hostile environments.";
					}
				}
			}

			// Token: 0x02002999 RID: 10649
			public class AQUA_SUIT
			{
				// Token: 0x0400B4EB RID: 46315
				public static LocString NAME = UI.FormatAsLink("Aqua Suit", "AQUA_SUIT");

				// Token: 0x0400B4EC RID: 46316
				public static LocString DESC = "Because breathing underwater is better than... not.";

				// Token: 0x0400B4ED RID: 46317
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" at an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400B4EE RID: 46318
				public static LocString RECIPE_DESC = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.";

				// Token: 0x0400B4EF RID: 46319
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "AQUA_SUIT");

				// Token: 0x0400B4F0 RID: 46320
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Aqua Suit", "AQUA_SUIT"),
					".\n\nSuits can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x0200299A RID: 10650
			public class TEMPERATURE_SUIT
			{
				// Token: 0x0400B4F1 RID: 46321
				public static LocString NAME = UI.FormatAsLink("Thermo Suit", "TEMPERATURE_SUIT");

				// Token: 0x0400B4F2 RID: 46322
				public static LocString DESC = "Keeps my Duplicants cool in case things heat up.";

				// Token: 0x0400B4F3 RID: 46323
				public static LocString EFFECT = "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.\n\nMust be powered at a Thermo Suit Dock when depleted.";

				// Token: 0x0400B4F4 RID: 46324
				public static LocString RECIPE_DESC = "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.";

				// Token: 0x0400B4F5 RID: 46325
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "TEMPERATURE_SUIT");

				// Token: 0x0400B4F6 RID: 46326
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Thermo Suit", "TEMPERATURE_SUIT"),
					".\n\nSuits can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x0200299B RID: 10651
			public class JET_SUIT
			{
				// Token: 0x0400B4F7 RID: 46327
				public static LocString NAME = UI.FormatAsLink("Jet Suit", "JET_SUIT");

				// Token: 0x0400B4F8 RID: 46328
				public static LocString DESC = "Allows my Duplicants to take to the skies, for a time.";

				// Token: 0x0400B4F9 RID: 46329
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments.\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" at a ",
					UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400B4FA RID: 46330
				public static LocString RECIPE_DESC = "Supplies Duplicants with " + UI.FormatAsLink("Oxygen", "OXYGEN") + " in toxic and low breathability environments.\n\nAllows Duplicant flight.";

				// Token: 0x0400B4FB RID: 46331
				public static LocString GENERICNAME = "Jet Suit";

				// Token: 0x0400B4FC RID: 46332
				public static LocString TANK_EFFECT_NAME = "Fuel Tank";

				// Token: 0x0400B4FD RID: 46333
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Jet Suit", "JET_SUIT");

				// Token: 0x0400B4FE RID: 46334
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Jet Suit", "JET_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});
			}

			// Token: 0x0200299C RID: 10652
			public class LEAD_SUIT
			{
				// Token: 0x0400B4FF RID: 46335
				public static LocString NAME = UI.FormatAsLink("Lead Suit", "LEAD_SUIT");

				// Token: 0x0400B500 RID: 46336
				public static LocString DESC = "Because exposure to radiation doesn't grant Duplicants superpowers.";

				// Token: 0x0400B501 RID: 46337
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and protection in areas with ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					".\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" at a ",
					UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400B502 RID: 46338
				public static LocString RECIPE_DESC = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments.\n\nProtects Duplicants from ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					"."
				});

				// Token: 0x0400B503 RID: 46339
				public static LocString GENERICNAME = "Lead Suit";

				// Token: 0x0400B504 RID: 46340
				public static LocString BATTERY_EFFECT_NAME = "Suit Battery";

				// Token: 0x0400B505 RID: 46341
				public static LocString SUIT_OUT_OF_BATTERIES = "Suit Batteries Empty";

				// Token: 0x0400B506 RID: 46342
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "LEAD_SUIT");

				// Token: 0x0400B507 RID: 46343
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Lead Suit", "LEAD_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});
			}

			// Token: 0x0200299D RID: 10653
			public class COOL_VEST
			{
				// Token: 0x0400B508 RID: 46344
				public static LocString NAME = UI.FormatAsLink("Cool Vest", "COOL_VEST");

				// Token: 0x0400B509 RID: 46345
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400B50A RID: 46346
				public static LocString DESC = "Don't sweat it!";

				// Token: 0x0400B50B RID: 46347
				public static LocString EFFECT = "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation.";

				// Token: 0x0400B50C RID: 46348
				public static LocString RECIPE_DESC = "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation";
			}

			// Token: 0x0200299E RID: 10654
			public class WARM_VEST
			{
				// Token: 0x0400B50D RID: 46349
				public static LocString NAME = UI.FormatAsLink("Warm Coat", "WARM_VEST");

				// Token: 0x0400B50E RID: 46350
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400B50F RID: 46351
				public static LocString DESC = "Happiness is a warm Duplicant.";

				// Token: 0x0400B510 RID: 46352
				public static LocString EFFECT = "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation.";

				// Token: 0x0400B511 RID: 46353
				public static LocString RECIPE_DESC = "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation";
			}

			// Token: 0x0200299F RID: 10655
			public class FUNKY_VEST
			{
				// Token: 0x0400B512 RID: 46354
				public static LocString NAME = UI.FormatAsLink("Snazzy Suit", "FUNKY_VEST");

				// Token: 0x0400B513 RID: 46355
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400B514 RID: 46356
				public static LocString DESC = "This transforms my Duplicant into a walking beacon of charm and style.";

				// Token: 0x0400B515 RID: 46357
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases Decor in a small area effect around the wearer. Can be upgraded to ",
					UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING"),
					" at the ",
					UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION"),
					"."
				});

				// Token: 0x0400B516 RID: 46358
				public static LocString RECIPE_DESC = "Increases Decor in a small area effect around the wearer. Can be upgraded to " + UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING") + " at the " + UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION");
			}

			// Token: 0x020029A0 RID: 10656
			public class CUSTOMCLOTHING
			{
				// Token: 0x0400B517 RID: 46359
				public static LocString NAME = UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING");

				// Token: 0x0400B518 RID: 46360
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400B519 RID: 46361
				public static LocString DESC = "This transforms my Duplicant into a colony-inspiring fashion icon.";

				// Token: 0x0400B51A RID: 46362
				public static LocString EFFECT = "Increases Decor in a small area effect around the wearer.";

				// Token: 0x0400B51B RID: 46363
				public static LocString RECIPE_DESC = "Increases Decor in a small area effect around the wearer";

				// Token: 0x020035E9 RID: 13801
				public class FACADES
				{
					// Token: 0x0400D91C RID: 55580
					public static LocString CLUBSHIRT = UI.FormatAsLink("Purple Polyester Suit", "CUSTOMCLOTHING");

					// Token: 0x0400D91D RID: 55581
					public static LocString CUMMERBUND = UI.FormatAsLink("Classic Cummerbund", "CUSTOMCLOTHING");

					// Token: 0x0400D91E RID: 55582
					public static LocString DECOR_02 = UI.FormatAsLink("Snazzier Red Suit", "CUSTOMCLOTHING");

					// Token: 0x0400D91F RID: 55583
					public static LocString DECOR_03 = UI.FormatAsLink("Snazzier Blue Suit", "CUSTOMCLOTHING");

					// Token: 0x0400D920 RID: 55584
					public static LocString DECOR_04 = UI.FormatAsLink("Snazzier Green Suit", "CUSTOMCLOTHING");

					// Token: 0x0400D921 RID: 55585
					public static LocString DECOR_05 = UI.FormatAsLink("Snazzier Violet Suit", "CUSTOMCLOTHING");

					// Token: 0x0400D922 RID: 55586
					public static LocString GAUDYSWEATER = UI.FormatAsLink("Pompom Knit Suit", "CUSTOMCLOTHING");

					// Token: 0x0400D923 RID: 55587
					public static LocString LIMONE = UI.FormatAsLink("Citrus Spandex Suit", "CUSTOMCLOTHING");

					// Token: 0x0400D924 RID: 55588
					public static LocString MONDRIAN = UI.FormatAsLink("Cubist Knit Suit", "CUSTOMCLOTHING");

					// Token: 0x0400D925 RID: 55589
					public static LocString OVERALLS = UI.FormatAsLink("Spiffy Overalls", "CUSTOMCLOTHING");

					// Token: 0x0400D926 RID: 55590
					public static LocString TRIANGLES = UI.FormatAsLink("Confetti Suit", "CUSTOMCLOTHING");

					// Token: 0x0400D927 RID: 55591
					public static LocString WORKOUT = UI.FormatAsLink("Pink Unitard", "CUSTOMCLOTHING");
				}
			}

			// Token: 0x020029A1 RID: 10657
			public class CLOTHING_GLOVES
			{
				// Token: 0x0400B51C RID: 46364
				public static LocString NAME = "Default Gloves";

				// Token: 0x0400B51D RID: 46365
				public static LocString DESC = "The default gloves.";

				// Token: 0x020035EA RID: 13802
				public class FACADES
				{
					// Token: 0x020039EF RID: 14831
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400E2D3 RID: 58067
						public static LocString NAME = "Basic Aqua Gloves";

						// Token: 0x0400E2D4 RID: 58068
						public static LocString DESC = "A good, solid pair of aqua-blue gloves that go with everything.";
					}

					// Token: 0x020039F0 RID: 14832
					public class BASIC_YELLOW
					{
						// Token: 0x0400E2D5 RID: 58069
						public static LocString NAME = "Basic Yellow Gloves";

						// Token: 0x0400E2D6 RID: 58070
						public static LocString DESC = "A good, solid pair of yellow gloves that go with everything.";
					}

					// Token: 0x020039F1 RID: 14833
					public class BASIC_BLACK
					{
						// Token: 0x0400E2D7 RID: 58071
						public static LocString NAME = "Basic Black Gloves";

						// Token: 0x0400E2D8 RID: 58072
						public static LocString DESC = "A good, solid pair of black gloves that go with everything.";
					}

					// Token: 0x020039F2 RID: 14834
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400E2D9 RID: 58073
						public static LocString NAME = "Basic Bubblegum Gloves";

						// Token: 0x0400E2DA RID: 58074
						public static LocString DESC = "A good, solid pair of bubblegum-pink gloves that go with everything.";
					}

					// Token: 0x020039F3 RID: 14835
					public class BASIC_GREEN
					{
						// Token: 0x0400E2DB RID: 58075
						public static LocString NAME = "Basic Green Gloves";

						// Token: 0x0400E2DC RID: 58076
						public static LocString DESC = "A good, solid pair of green gloves that go with everything.";
					}

					// Token: 0x020039F4 RID: 14836
					public class BASIC_ORANGE
					{
						// Token: 0x0400E2DD RID: 58077
						public static LocString NAME = "Basic Orange Gloves";

						// Token: 0x0400E2DE RID: 58078
						public static LocString DESC = "A good, solid pair of orange gloves that go with everything.";
					}

					// Token: 0x020039F5 RID: 14837
					public class BASIC_PURPLE
					{
						// Token: 0x0400E2DF RID: 58079
						public static LocString NAME = "Basic Purple Gloves";

						// Token: 0x0400E2E0 RID: 58080
						public static LocString DESC = "A good, solid pair of purple gloves that go with everything.";
					}

					// Token: 0x020039F6 RID: 14838
					public class BASIC_RED
					{
						// Token: 0x0400E2E1 RID: 58081
						public static LocString NAME = "Basic Red Gloves";

						// Token: 0x0400E2E2 RID: 58082
						public static LocString DESC = "A good, solid pair of red gloves that go with everything.";
					}

					// Token: 0x020039F7 RID: 14839
					public class BASIC_WHITE
					{
						// Token: 0x0400E2E3 RID: 58083
						public static LocString NAME = "Basic White Gloves";

						// Token: 0x0400E2E4 RID: 58084
						public static LocString DESC = "A good, solid pair of white gloves that go with everything.";
					}

					// Token: 0x020039F8 RID: 14840
					public class GLOVES_ATHLETIC_DEEPRED
					{
						// Token: 0x0400E2E5 RID: 58085
						public static LocString NAME = "Team Captain Sports Gloves";

						// Token: 0x0400E2E6 RID: 58086
						public static LocString DESC = "Red-striped gloves for winning at any activity.";
					}

					// Token: 0x020039F9 RID: 14841
					public class GLOVES_ATHLETIC_SATSUMA
					{
						// Token: 0x0400E2E7 RID: 58087
						public static LocString NAME = "Superfan Sports Gloves";

						// Token: 0x0400E2E8 RID: 58088
						public static LocString DESC = "Orange-striped gloves for enthusiastic athletes.";
					}

					// Token: 0x020039FA RID: 14842
					public class GLOVES_ATHLETIC_LEMON
					{
						// Token: 0x0400E2E9 RID: 58089
						public static LocString NAME = "Hype Sports Gloves";

						// Token: 0x0400E2EA RID: 58090
						public static LocString DESC = "Yellow-striped gloves for athletes who seek to raise the bar.";
					}

					// Token: 0x020039FB RID: 14843
					public class GLOVES_ATHLETIC_KELLYGREEN
					{
						// Token: 0x0400E2EB RID: 58091
						public static LocString NAME = "Go Team Sports Gloves";

						// Token: 0x0400E2EC RID: 58092
						public static LocString DESC = "Green-striped gloves for the perenially good sport.";
					}

					// Token: 0x020039FC RID: 14844
					public class GLOVES_ATHLETIC_COBALT
					{
						// Token: 0x0400E2ED RID: 58093
						public static LocString NAME = "True Blue Sports Gloves";

						// Token: 0x0400E2EE RID: 58094
						public static LocString DESC = "Blue-striped gloves perfect for shaking hands after the game.";
					}

					// Token: 0x020039FD RID: 14845
					public class GLOVES_ATHLETIC_FLAMINGO
					{
						// Token: 0x0400E2EF RID: 58095
						public static LocString NAME = "Pep Rally Sports Gloves";

						// Token: 0x0400E2F0 RID: 58096
						public static LocString DESC = "Pink-striped glove designed to withstand countless high-fives.";
					}

					// Token: 0x020039FE RID: 14846
					public class GLOVES_ATHLETIC_CHARCOAL
					{
						// Token: 0x0400E2F1 RID: 58097
						public static LocString NAME = "Underdog Sports Gloves";

						// Token: 0x0400E2F2 RID: 58098
						public static LocString DESC = "The muted stripe minimizes distractions so its wearer can focus on trying very, very hard.";
					}

					// Token: 0x020039FF RID: 14847
					public class CUFFLESS_BLUEBERRY
					{
						// Token: 0x0400E2F3 RID: 58099
						public static LocString NAME = "Blueberry Glovelets";

						// Token: 0x0400E2F4 RID: 58100
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003A00 RID: 14848
					public class CUFFLESS_GRAPE
					{
						// Token: 0x0400E2F5 RID: 58101
						public static LocString NAME = "Grape Glovelets";

						// Token: 0x0400E2F6 RID: 58102
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003A01 RID: 14849
					public class CUFFLESS_LEMON
					{
						// Token: 0x0400E2F7 RID: 58103
						public static LocString NAME = "Lemon Glovelets";

						// Token: 0x0400E2F8 RID: 58104
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003A02 RID: 14850
					public class CUFFLESS_LIME
					{
						// Token: 0x0400E2F9 RID: 58105
						public static LocString NAME = "Lime Glovelets";

						// Token: 0x0400E2FA RID: 58106
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003A03 RID: 14851
					public class CUFFLESS_SATSUMA
					{
						// Token: 0x0400E2FB RID: 58107
						public static LocString NAME = "Satsuma Glovelets";

						// Token: 0x0400E2FC RID: 58108
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003A04 RID: 14852
					public class CUFFLESS_STRAWBERRY
					{
						// Token: 0x0400E2FD RID: 58109
						public static LocString NAME = "Strawberry Glovelets";

						// Token: 0x0400E2FE RID: 58110
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003A05 RID: 14853
					public class CUFFLESS_WATERMELON
					{
						// Token: 0x0400E2FF RID: 58111
						public static LocString NAME = "Watermelon Glovelets";

						// Token: 0x0400E300 RID: 58112
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003A06 RID: 14854
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400E301 RID: 58113
						public static LocString NAME = "LED Gloves";

						// Token: 0x0400E302 RID: 58114
						public static LocString DESC = "Great for gesticulating at parties.";
					}

					// Token: 0x02003A07 RID: 14855
					public class ATHLETE
					{
						// Token: 0x0400E303 RID: 58115
						public static LocString NAME = "Racing Gloves";

						// Token: 0x0400E304 RID: 58116
						public static LocString DESC = "Crafted for high-speed handshakes.";
					}

					// Token: 0x02003A08 RID: 14856
					public class BASIC_BROWN_KHAKI
					{
						// Token: 0x0400E305 RID: 58117
						public static LocString NAME = "Basic Khaki Gloves";

						// Token: 0x0400E306 RID: 58118
						public static LocString DESC = "They don't show dirt.";
					}

					// Token: 0x02003A09 RID: 14857
					public class BASIC_BLUEGREY
					{
						// Token: 0x0400E307 RID: 58119
						public static LocString NAME = "Basic Gunmetal Gloves";

						// Token: 0x0400E308 RID: 58120
						public static LocString DESC = "A tough name for soft gloves.";
					}

					// Token: 0x02003A0A RID: 14858
					public class CUFFLESS_BLACK
					{
						// Token: 0x0400E309 RID: 58121
						public static LocString NAME = "Stealth Glovelets";

						// Token: 0x0400E30A RID: 58122
						public static LocString DESC = "It's easy to forget they're even on.";
					}

					// Token: 0x02003A0B RID: 14859
					public class DENIM_BLUE
					{
						// Token: 0x0400E30B RID: 58123
						public static LocString NAME = "Denim Gloves";

						// Token: 0x0400E30C RID: 58124
						public static LocString DESC = "They're not great for dexterity.";
					}

					// Token: 0x02003A0C RID: 14860
					public class BASIC_GREY
					{
						// Token: 0x0400E30D RID: 58125
						public static LocString NAME = "Basic Gray Gloves";

						// Token: 0x0400E30E RID: 58126
						public static LocString DESC = "A good, solid pair of gray gloves that go with everything.";
					}

					// Token: 0x02003A0D RID: 14861
					public class BASIC_PINKSALMON
					{
						// Token: 0x0400E30F RID: 58127
						public static LocString NAME = "Basic Coral Gloves";

						// Token: 0x0400E310 RID: 58128
						public static LocString DESC = "A good, solid pair of bright pink gloves that go with everything.";
					}

					// Token: 0x02003A0E RID: 14862
					public class BASIC_TAN
					{
						// Token: 0x0400E311 RID: 58129
						public static LocString NAME = "Basic Tan Gloves";

						// Token: 0x0400E312 RID: 58130
						public static LocString DESC = "A good, solid pair of tan gloves that go with everything.";
					}

					// Token: 0x02003A0F RID: 14863
					public class BALLERINA_PINK
					{
						// Token: 0x0400E313 RID: 58131
						public static LocString NAME = "Ballet Gloves";

						// Token: 0x0400E314 RID: 58132
						public static LocString DESC = "Wrist ruffles highlight the poetic movements of the phalanges.";
					}

					// Token: 0x02003A10 RID: 14864
					public class FORMAL_WHITE
					{
						// Token: 0x0400E315 RID: 58133
						public static LocString NAME = "White Silk Gloves";

						// Token: 0x0400E316 RID: 58134
						public static LocString DESC = "They're as soft as...well, silk.";
					}

					// Token: 0x02003A11 RID: 14865
					public class LONG_WHITE
					{
						// Token: 0x0400E317 RID: 58135
						public static LocString NAME = "White Evening Gloves";

						// Token: 0x0400E318 RID: 58136
						public static LocString DESC = "Super-long gloves for super-formal occasions.";
					}

					// Token: 0x02003A12 RID: 14866
					public class TWOTONE_CREAM_CHARCOAL
					{
						// Token: 0x0400E319 RID: 58137
						public static LocString NAME = "Contrast Cuff Gloves";

						// Token: 0x0400E31A RID: 58138
						public static LocString DESC = "For elegance so understated, it may go completely unnoticed.";
					}

					// Token: 0x02003A13 RID: 14867
					public class SOCKSUIT_BEIGE
					{
						// Token: 0x0400E31B RID: 58139
						public static LocString NAME = "Vintage Handsock";

						// Token: 0x0400E31C RID: 58140
						public static LocString DESC = "Designed by someone with cold hands and an excess of old socks.";
					}

					// Token: 0x02003A14 RID: 14868
					public class BASIC_SLATE
					{
						// Token: 0x0400E31D RID: 58141
						public static LocString NAME = "Basic Slate Gloves";

						// Token: 0x0400E31E RID: 58142
						public static LocString DESC = "A good, solid pair of slate gloves that go with everything.";
					}

					// Token: 0x02003A15 RID: 14869
					public class KNIT_GOLD
					{
						// Token: 0x0400E31F RID: 58143
						public static LocString NAME = "Gold Knit Gloves";

						// Token: 0x0400E320 RID: 58144
						public static LocString DESC = "Produces a pleasantly muffled \"whump\" when high-fiving.";
					}

					// Token: 0x02003A16 RID: 14870
					public class KNIT_MAGENTA
					{
						// Token: 0x0400E321 RID: 58145
						public static LocString NAME = "Magenta Knit Gloves";

						// Token: 0x0400E322 RID: 58146
						public static LocString DESC = "Produces a pleasantly muffled \"whump\" when high-fiving.";
					}

					// Token: 0x02003A17 RID: 14871
					public class SPARKLE_WHITE
					{
						// Token: 0x0400E323 RID: 58147
						public static LocString NAME = "White Glitter Gloves";

						// Token: 0x0400E324 RID: 58148
						public static LocString DESC = "Each sequin was attached using sealant borrowed from the rocketry department.";
					}

					// Token: 0x02003A18 RID: 14872
					public class GINCH_PINK_SALTROCK
					{
						// Token: 0x0400E325 RID: 58149
						public static LocString NAME = "Frilly Saltrock Gloves";

						// Token: 0x0400E326 RID: 58150
						public static LocString DESC = "Thick, soft pink gloves with added flounce.";
					}

					// Token: 0x02003A19 RID: 14873
					public class GINCH_PURPLE_DUSKY
					{
						// Token: 0x0400E327 RID: 58151
						public static LocString NAME = "Frilly Dusk Gloves";

						// Token: 0x0400E328 RID: 58152
						public static LocString DESC = "Thick, soft purple gloves with added flounce.";
					}

					// Token: 0x02003A1A RID: 14874
					public class GINCH_BLUE_BASIN
					{
						// Token: 0x0400E329 RID: 58153
						public static LocString NAME = "Frilly Basin Gloves";

						// Token: 0x0400E32A RID: 58154
						public static LocString DESC = "Thick, soft blue gloves with added flounce.";
					}

					// Token: 0x02003A1B RID: 14875
					public class GINCH_TEAL_BALMY
					{
						// Token: 0x0400E32B RID: 58155
						public static LocString NAME = "Frilly Balm Gloves";

						// Token: 0x0400E32C RID: 58156
						public static LocString DESC = "The soft teal fabric soothes hard-working hands.";
					}

					// Token: 0x02003A1C RID: 14876
					public class GINCH_GREEN_LIME
					{
						// Token: 0x0400E32D RID: 58157
						public static LocString NAME = "Frilly Leach Gloves";

						// Token: 0x0400E32E RID: 58158
						public static LocString DESC = "Thick, soft green gloves with added flounce.";
					}

					// Token: 0x02003A1D RID: 14877
					public class GINCH_YELLOW_YELLOWCAKE
					{
						// Token: 0x0400E32F RID: 58159
						public static LocString NAME = "Frilly Yellowcake Gloves";

						// Token: 0x0400E330 RID: 58160
						public static LocString DESC = "Thick, soft yellow gloves with added flounce.";
					}

					// Token: 0x02003A1E RID: 14878
					public class GINCH_ORANGE_ATOMIC
					{
						// Token: 0x0400E331 RID: 58161
						public static LocString NAME = "Frilly Atomic Gloves";

						// Token: 0x0400E332 RID: 58162
						public static LocString DESC = "Thick, bright orange gloves with added flounce.";
					}

					// Token: 0x02003A1F RID: 14879
					public class GINCH_RED_MAGMA
					{
						// Token: 0x0400E333 RID: 58163
						public static LocString NAME = "Frilly Magma Gloves";

						// Token: 0x0400E334 RID: 58164
						public static LocString DESC = "Thick, soft red gloves with added flounce.";
					}

					// Token: 0x02003A20 RID: 14880
					public class GINCH_GREY_GREY
					{
						// Token: 0x0400E335 RID: 58165
						public static LocString NAME = "Frilly Slate Gloves";

						// Token: 0x0400E336 RID: 58166
						public static LocString DESC = "Thick, soft grey gloves with added flounce.";
					}

					// Token: 0x02003A21 RID: 14881
					public class GINCH_GREY_CHARCOAL
					{
						// Token: 0x0400E337 RID: 58167
						public static LocString NAME = "Frilly Charcoal Gloves";

						// Token: 0x0400E338 RID: 58168
						public static LocString DESC = "Thick, soft dark grey gloves with added flounce.";
					}
				}
			}

			// Token: 0x020029A2 RID: 10658
			public class CLOTHING_TOPS
			{
				// Token: 0x0400B51E RID: 46366
				public static LocString NAME = "Default Top";

				// Token: 0x0400B51F RID: 46367
				public static LocString DESC = "The default shirt.";

				// Token: 0x020035EB RID: 13803
				public class FACADES
				{
					// Token: 0x02003A22 RID: 14882
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400E339 RID: 58169
						public static LocString NAME = "Basic Aqua Shirt";

						// Token: 0x0400E33A RID: 58170
						public static LocString DESC = "A nice aqua-blue shirt that goes with everything.";
					}

					// Token: 0x02003A23 RID: 14883
					public class BASIC_BLACK
					{
						// Token: 0x0400E33B RID: 58171
						public static LocString NAME = "Basic Black Shirt";

						// Token: 0x0400E33C RID: 58172
						public static LocString DESC = "A nice black shirt that goes with everything.";
					}

					// Token: 0x02003A24 RID: 14884
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400E33D RID: 58173
						public static LocString NAME = "Basic Bubblegum Shirt";

						// Token: 0x0400E33E RID: 58174
						public static LocString DESC = "A nice bubblegum-pink shirt that goes with everything.";
					}

					// Token: 0x02003A25 RID: 14885
					public class BASIC_GREEN
					{
						// Token: 0x0400E33F RID: 58175
						public static LocString NAME = "Basic Green Shirt";

						// Token: 0x0400E340 RID: 58176
						public static LocString DESC = "A nice green shirt that goes with everything.";
					}

					// Token: 0x02003A26 RID: 14886
					public class BASIC_ORANGE
					{
						// Token: 0x0400E341 RID: 58177
						public static LocString NAME = "Basic Orange Shirt";

						// Token: 0x0400E342 RID: 58178
						public static LocString DESC = "A nice orange shirt that goes with everything.";
					}

					// Token: 0x02003A27 RID: 14887
					public class BASIC_PURPLE
					{
						// Token: 0x0400E343 RID: 58179
						public static LocString NAME = "Basic Purple Shirt";

						// Token: 0x0400E344 RID: 58180
						public static LocString DESC = "A nice purple shirt that goes with everything.";
					}

					// Token: 0x02003A28 RID: 14888
					public class BASIC_RED_BURNT
					{
						// Token: 0x0400E345 RID: 58181
						public static LocString NAME = "Basic Red Shirt";

						// Token: 0x0400E346 RID: 58182
						public static LocString DESC = "A nice red shirt that goes with everything.";
					}

					// Token: 0x02003A29 RID: 14889
					public class BASIC_WHITE
					{
						// Token: 0x0400E347 RID: 58183
						public static LocString NAME = "Basic White Shirt";

						// Token: 0x0400E348 RID: 58184
						public static LocString DESC = "A nice white shirt that goes with everything.";
					}

					// Token: 0x02003A2A RID: 14890
					public class BASIC_YELLOW
					{
						// Token: 0x0400E349 RID: 58185
						public static LocString NAME = "Basic Yellow Shirt";

						// Token: 0x0400E34A RID: 58186
						public static LocString DESC = "A nice yellow shirt that goes with everything.";
					}

					// Token: 0x02003A2B RID: 14891
					public class RAGLANTOP_DEEPRED
					{
						// Token: 0x0400E34B RID: 58187
						public static LocString NAME = "Team Captain T-shirt";

						// Token: 0x0400E34C RID: 58188
						public static LocString DESC = "A slightly sweat-stained tee for natural leaders.";
					}

					// Token: 0x02003A2C RID: 14892
					public class RAGLANTOP_COBALT
					{
						// Token: 0x0400E34D RID: 58189
						public static LocString NAME = "True Blue T-shirt";

						// Token: 0x0400E34E RID: 58190
						public static LocString DESC = "A slightly sweat-stained tee for the real team players.";
					}

					// Token: 0x02003A2D RID: 14893
					public class RAGLANTOP_FLAMINGO
					{
						// Token: 0x0400E34F RID: 58191
						public static LocString NAME = "Pep Rally T-shirt";

						// Token: 0x0400E350 RID: 58192
						public static LocString DESC = "A slightly sweat-stained tee to boost team spirits.";
					}

					// Token: 0x02003A2E RID: 14894
					public class RAGLANTOP_KELLYGREEN
					{
						// Token: 0x0400E351 RID: 58193
						public static LocString NAME = "Go Team T-shirt";

						// Token: 0x0400E352 RID: 58194
						public static LocString DESC = "A slightly sweat-stained tee for cheering from the sidelines.";
					}

					// Token: 0x02003A2F RID: 14895
					public class RAGLANTOP_CHARCOAL
					{
						// Token: 0x0400E353 RID: 58195
						public static LocString NAME = "Underdog T-shirt";

						// Token: 0x0400E354 RID: 58196
						public static LocString DESC = "For those who don't win a lot.";
					}

					// Token: 0x02003A30 RID: 14896
					public class RAGLANTOP_LEMON
					{
						// Token: 0x0400E355 RID: 58197
						public static LocString NAME = "Hype T-shirt";

						// Token: 0x0400E356 RID: 58198
						public static LocString DESC = "A slightly sweat-stained tee to wear when talking a big game.";
					}

					// Token: 0x02003A31 RID: 14897
					public class RAGLANTOP_SATSUMA
					{
						// Token: 0x0400E357 RID: 58199
						public static LocString NAME = "Superfan T-shirt";

						// Token: 0x0400E358 RID: 58200
						public static LocString DESC = "A slightly sweat-stained tee for the long-time supporter.";
					}

					// Token: 0x02003A32 RID: 14898
					public class JELLYPUFFJACKET_BLUEBERRY
					{
						// Token: 0x0400E359 RID: 58201
						public static LocString NAME = "Blueberry Jelly Jacket";

						// Token: 0x0400E35A RID: 58202
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003A33 RID: 14899
					public class JELLYPUFFJACKET_GRAPE
					{
						// Token: 0x0400E35B RID: 58203
						public static LocString NAME = "Grape Jelly Jacket";

						// Token: 0x0400E35C RID: 58204
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003A34 RID: 14900
					public class JELLYPUFFJACKET_LEMON
					{
						// Token: 0x0400E35D RID: 58205
						public static LocString NAME = "Lemon Jelly Jacket";

						// Token: 0x0400E35E RID: 58206
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003A35 RID: 14901
					public class JELLYPUFFJACKET_LIME
					{
						// Token: 0x0400E35F RID: 58207
						public static LocString NAME = "Lime Jelly Jacket";

						// Token: 0x0400E360 RID: 58208
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003A36 RID: 14902
					public class JELLYPUFFJACKET_SATSUMA
					{
						// Token: 0x0400E361 RID: 58209
						public static LocString NAME = "Satsuma Jelly Jacket";

						// Token: 0x0400E362 RID: 58210
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003A37 RID: 14903
					public class JELLYPUFFJACKET_STRAWBERRY
					{
						// Token: 0x0400E363 RID: 58211
						public static LocString NAME = "Strawberry Jelly Jacket";

						// Token: 0x0400E364 RID: 58212
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003A38 RID: 14904
					public class JELLYPUFFJACKET_WATERMELON
					{
						// Token: 0x0400E365 RID: 58213
						public static LocString NAME = "Watermelon Jelly Jacket";

						// Token: 0x0400E366 RID: 58214
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003A39 RID: 14905
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400E367 RID: 58215
						public static LocString NAME = "LED Jacket";

						// Token: 0x0400E368 RID: 58216
						public static LocString DESC = "For dancing in the dark.";
					}

					// Token: 0x02003A3A RID: 14906
					public class TSHIRT_WHITE
					{
						// Token: 0x0400E369 RID: 58217
						public static LocString NAME = "Classic White Tee";

						// Token: 0x0400E36A RID: 58218
						public static LocString DESC = "It's practically begging for a big Bog Jelly stain down the front.";
					}

					// Token: 0x02003A3B RID: 14907
					public class TSHIRT_MAGENTA
					{
						// Token: 0x0400E36B RID: 58219
						public static LocString NAME = "Classic Magenta Tee";

						// Token: 0x0400E36C RID: 58220
						public static LocString DESC = "It will never chafe against delicate inner-elbow skin.";
					}

					// Token: 0x02003A3C RID: 14908
					public class ATHLETE
					{
						// Token: 0x0400E36D RID: 58221
						public static LocString NAME = "Racing Jacket";

						// Token: 0x0400E36E RID: 58222
						public static LocString DESC = "The epitome of fast fashion.";
					}

					// Token: 0x02003A3D RID: 14909
					public class DENIM_BLUE
					{
						// Token: 0x0400E36F RID: 58223
						public static LocString NAME = "Denim Jacket";

						// Token: 0x0400E370 RID: 58224
						public static LocString DESC = "The top half of a Canadian tuxedo.";
					}

					// Token: 0x02003A3E RID: 14910
					public class GONCH_STRAWBERRY
					{
						// Token: 0x0400E371 RID: 58225
						public static LocString NAME = "Executive Undershirt";

						// Token: 0x0400E372 RID: 58226
						public static LocString DESC = "The breathable base layer every power suit needs.";
					}

					// Token: 0x02003A3F RID: 14911
					public class GONCH_SATSUMA
					{
						// Token: 0x0400E373 RID: 58227
						public static LocString NAME = "Underling Undershirt";

						// Token: 0x0400E374 RID: 58228
						public static LocString DESC = "Extra-absorbent fabric in the underarms to mop up nervous sweat.";
					}

					// Token: 0x02003A40 RID: 14912
					public class GONCH_LEMON
					{
						// Token: 0x0400E375 RID: 58229
						public static LocString NAME = "Groupthink Undershirt";

						// Token: 0x0400E376 RID: 58230
						public static LocString DESC = "Because the most popular choice is always the right choice.";
					}

					// Token: 0x02003A41 RID: 14913
					public class GONCH_LIME
					{
						// Token: 0x0400E377 RID: 58231
						public static LocString NAME = "Stakeholder Undershirt";

						// Token: 0x0400E378 RID: 58232
						public static LocString DESC = "Soft against the skin, for those who have skin in the game.";
					}

					// Token: 0x02003A42 RID: 14914
					public class GONCH_BLUEBERRY
					{
						// Token: 0x0400E379 RID: 58233
						public static LocString NAME = "Admin Undershirt";

						// Token: 0x0400E37A RID: 58234
						public static LocString DESC = "Criminally underappreciated.";
					}

					// Token: 0x02003A43 RID: 14915
					public class GONCH_GRAPE
					{
						// Token: 0x0400E37B RID: 58235
						public static LocString NAME = "Buzzword Undershirt";

						// Token: 0x0400E37C RID: 58236
						public static LocString DESC = "A value-added vest for touching base and thinking outside the box using best practices ASAP.";
					}

					// Token: 0x02003A44 RID: 14916
					public class GONCH_WATERMELON
					{
						// Token: 0x0400E37D RID: 58237
						public static LocString NAME = "Synergy Undershirt";

						// Token: 0x0400E37E RID: 58238
						public static LocString DESC = "Asking for it by name often triggers dramatic eye-rolls from bystanders.";
					}

					// Token: 0x02003A45 RID: 14917
					public class NERD_BROWN
					{
						// Token: 0x0400E37F RID: 58239
						public static LocString NAME = "Research Shirt";

						// Token: 0x0400E380 RID: 58240
						public static LocString DESC = "Comes with a thoughtfully chewed-up ballpoint pen.";
					}

					// Token: 0x02003A46 RID: 14918
					public class GI_WHITE
					{
						// Token: 0x0400E381 RID: 58241
						public static LocString NAME = "Rebel Gi Jacket";

						// Token: 0x0400E382 RID: 58242
						public static LocString DESC = "The contrasting trim hides stains from messy post-sparring snacks.";
					}

					// Token: 0x02003A47 RID: 14919
					public class JACKET_SMOKING_BURGUNDY
					{
						// Token: 0x0400E383 RID: 58243
						public static LocString NAME = "Donor Jacket";

						// Token: 0x0400E384 RID: 58244
						public static LocString DESC = "Crafted from the softest, most philanthropic fibers.";
					}

					// Token: 0x02003A48 RID: 14920
					public class MECHANIC
					{
						// Token: 0x0400E385 RID: 58245
						public static LocString NAME = "Engineer Jacket";

						// Token: 0x0400E386 RID: 58246
						public static LocString DESC = "Designed to withstand the rigors of applied science.";
					}

					// Token: 0x02003A49 RID: 14921
					public class VELOUR_BLACK
					{
						// Token: 0x0400E387 RID: 58247
						public static LocString NAME = "PhD Velour Jacket";

						// Token: 0x0400E388 RID: 58248
						public static LocString DESC = "A formal jacket for those who are \"not that kind of doctor.\"";
					}

					// Token: 0x02003A4A RID: 14922
					public class VELOUR_BLUE
					{
						// Token: 0x0400E389 RID: 58249
						public static LocString NAME = "Shortwave Velour Jacket";

						// Token: 0x0400E38A RID: 58250
						public static LocString DESC = "A luxe, pettable jacket paired with a clip-on tie.";
					}

					// Token: 0x02003A4B RID: 14923
					public class VELOUR_PINK
					{
						// Token: 0x0400E38B RID: 58251
						public static LocString NAME = "Gamma Velour Jacket";

						// Token: 0x0400E38C RID: 58252
						public static LocString DESC = "Some scientists are less shy than others.";
					}

					// Token: 0x02003A4C RID: 14924
					public class WAISTCOAT_PINSTRIPE_SLATE
					{
						// Token: 0x0400E38D RID: 58253
						public static LocString NAME = "Nobel Pinstripe Waistcoat";

						// Token: 0x0400E38E RID: 58254
						public static LocString DESC = "One must dress for the prize that one wishes to win.";
					}

					// Token: 0x02003A4D RID: 14925
					public class WATER
					{
						// Token: 0x0400E38F RID: 58255
						public static LocString NAME = "HVAC Khaki Shirt";

						// Token: 0x0400E390 RID: 58256
						public static LocString DESC = "Designed to regulate temperature and humidity.";
					}

					// Token: 0x02003A4E RID: 14926
					public class TWEED_PINK_ORCHID
					{
						// Token: 0x0400E391 RID: 58257
						public static LocString NAME = "Power Brunch Blazer";

						// Token: 0x0400E392 RID: 58258
						public static LocString DESC = "Winners never quit, quitters never win.";
					}

					// Token: 0x02003A4F RID: 14927
					public class DRESS_SLEEVELESS_BOW_BW
					{
						// Token: 0x0400E393 RID: 58259
						public static LocString NAME = "PhD Dress";

						// Token: 0x0400E394 RID: 58260
						public static LocString DESC = "Ready for a post-thesis-defense party.";
					}

					// Token: 0x02003A50 RID: 14928
					public class BODYSUIT_BALLERINA_PINK
					{
						// Token: 0x0400E395 RID: 58261
						public static LocString NAME = "Ballet Leotard";

						// Token: 0x0400E396 RID: 58262
						public static LocString DESC = "Lab-crafted fabric with a level of stretchiness that defies the laws of physics.";
					}

					// Token: 0x02003A51 RID: 14929
					public class SOCKSUIT_BEIGE
					{
						// Token: 0x0400E397 RID: 58263
						public static LocString NAME = "Vintage Sockshirt";

						// Token: 0x0400E398 RID: 58264
						public static LocString DESC = "Like a sock for the torso. With sleeves.";
					}

					// Token: 0x02003A52 RID: 14930
					public class X_SPORCHID
					{
						// Token: 0x0400E399 RID: 58265
						public static LocString NAME = "Sporefest Sweater";

						// Token: 0x0400E39A RID: 58266
						public static LocString DESC = "This soft knit can be worn anytime, not just during Zombie Spore season.";
					}

					// Token: 0x02003A53 RID: 14931
					public class X1_PINCHAPEPPERNUTBELLS
					{
						// Token: 0x0400E39B RID: 58267
						public static LocString NAME = "Pinchabell Jacket";

						// Token: 0x0400E39C RID: 58268
						public static LocString DESC = "The peppernuts jingle just loudly enough to be distracting.";
					}

					// Token: 0x02003A54 RID: 14932
					public class POMPOM_SHINEBUGS_PINK_PEPPERNUT
					{
						// Token: 0x0400E39D RID: 58269
						public static LocString NAME = "Pom Bug Sweater";

						// Token: 0x0400E39E RID: 58270
						public static LocString DESC = "No Shine Bugs were harmed in the making of this sweater.";
					}

					// Token: 0x02003A55 RID: 14933
					public class SNOWFLAKE_BLUE
					{
						// Token: 0x0400E39F RID: 58271
						public static LocString NAME = "Crystal-Iced Sweater";

						// Token: 0x0400E3A0 RID: 58272
						public static LocString DESC = "Tiny imperfections in the front pattern ensure that no two are truly identical.";
					}

					// Token: 0x02003A56 RID: 14934
					public class PJ_CLOVERS_GLITCH_KELLY
					{
						// Token: 0x0400E3A1 RID: 58273
						public static LocString NAME = "Lucky Jammies";

						// Token: 0x0400E3A2 RID: 58274
						public static LocString DESC = "Even the most brilliant minds need a little extra luck sometimes.";
					}

					// Token: 0x02003A57 RID: 14935
					public class PJ_HEARTS_CHILLI_STRAWBERRY
					{
						// Token: 0x0400E3A3 RID: 58275
						public static LocString NAME = "Sweetheart Jammies";

						// Token: 0x0400E3A4 RID: 58276
						public static LocString DESC = "Plush chenille fabric and a drool-absorbent collar? This sleepsuit really <i>is</i> \"The One.\"";
					}

					// Token: 0x02003A58 RID: 14936
					public class BUILDER
					{
						// Token: 0x0400E3A5 RID: 58277
						public static LocString NAME = "Hi-Vis Jacket";

						// Token: 0x0400E3A6 RID: 58278
						public static LocString DESC = "Unmissable style for the safety-minded.";
					}

					// Token: 0x02003A59 RID: 14937
					public class FLORAL_PINK
					{
						// Token: 0x0400E3A7 RID: 58279
						public static LocString NAME = "Downtime Shirt";

						// Token: 0x0400E3A8 RID: 58280
						public static LocString DESC = "For maxing and relaxing when errands are too taxing.";
					}

					// Token: 0x02003A5A RID: 14938
					public class GINCH_PINK_SALTROCK
					{
						// Token: 0x0400E3A9 RID: 58281
						public static LocString NAME = "Frilly Saltrock Undershirt";

						// Token: 0x0400E3AA RID: 58282
						public static LocString DESC = "A seamless pink undershirt with laser-cut ruffles.";
					}

					// Token: 0x02003A5B RID: 14939
					public class GINCH_PURPLE_DUSKY
					{
						// Token: 0x0400E3AB RID: 58283
						public static LocString NAME = "Frilly Dusk Undershirt";

						// Token: 0x0400E3AC RID: 58284
						public static LocString DESC = "A seamless purple undershirt with laser-cut ruffles.";
					}

					// Token: 0x02003A5C RID: 14940
					public class GINCH_BLUE_BASIN
					{
						// Token: 0x0400E3AD RID: 58285
						public static LocString NAME = "Frilly Basin Undershirt";

						// Token: 0x0400E3AE RID: 58286
						public static LocString DESC = "A seamless blue undershirt with laser-cut ruffles.";
					}

					// Token: 0x02003A5D RID: 14941
					public class GINCH_TEAL_BALMY
					{
						// Token: 0x0400E3AF RID: 58287
						public static LocString NAME = "Frilly Balm Undershirt";

						// Token: 0x0400E3B0 RID: 58288
						public static LocString DESC = "A seamless teal undershirt with laser-cut ruffles.";
					}

					// Token: 0x02003A5E RID: 14942
					public class GINCH_GREEN_LIME
					{
						// Token: 0x0400E3B1 RID: 58289
						public static LocString NAME = "Frilly Leach Undershirt";

						// Token: 0x0400E3B2 RID: 58290
						public static LocString DESC = "A seamless green undershirt with laser-cut ruffles.";
					}

					// Token: 0x02003A5F RID: 14943
					public class GINCH_YELLOW_YELLOWCAKE
					{
						// Token: 0x0400E3B3 RID: 58291
						public static LocString NAME = "Frilly Yellowcake Undershirt";

						// Token: 0x0400E3B4 RID: 58292
						public static LocString DESC = "A seamless yellow undershirt with laser-cut ruffles.";
					}

					// Token: 0x02003A60 RID: 14944
					public class GINCH_ORANGE_ATOMIC
					{
						// Token: 0x0400E3B5 RID: 58293
						public static LocString NAME = "Frilly Atomic Undershirt";

						// Token: 0x0400E3B6 RID: 58294
						public static LocString DESC = "A seamless orange undershirt with laser-cut ruffles.";
					}

					// Token: 0x02003A61 RID: 14945
					public class GINCH_RED_MAGMA
					{
						// Token: 0x0400E3B7 RID: 58295
						public static LocString NAME = "Frilly Magma Undershirt";

						// Token: 0x0400E3B8 RID: 58296
						public static LocString DESC = "A seamless red undershirt with laser-cut ruffles.";
					}

					// Token: 0x02003A62 RID: 14946
					public class GINCH_GREY_GREY
					{
						// Token: 0x0400E3B9 RID: 58297
						public static LocString NAME = "Frilly Slate Undershirt";

						// Token: 0x0400E3BA RID: 58298
						public static LocString DESC = "A seamless grey undershirt with laser-cut ruffles.";
					}

					// Token: 0x02003A63 RID: 14947
					public class GINCH_GREY_CHARCOAL
					{
						// Token: 0x0400E3BB RID: 58299
						public static LocString NAME = "Frilly Charcoal Undershirt";

						// Token: 0x0400E3BC RID: 58300
						public static LocString DESC = "A seamless dark grey undershirt with laser-cut ruffles.";
					}

					// Token: 0x02003A64 RID: 14948
					public class KNIT_POLKADOT_TURQ
					{
						// Token: 0x0400E3BD RID: 58301
						public static LocString NAME = "Polka Dot Track Jacket";

						// Token: 0x0400E3BE RID: 58302
						public static LocString DESC = "The dots are infused with odor-neutralizing enzymes!";
					}

					// Token: 0x02003A65 RID: 14949
					public class FLASHY
					{
						// Token: 0x0400E3BF RID: 58303
						public static LocString NAME = "Superstar Jacket";

						// Token: 0x0400E3C0 RID: 58304
						public static LocString DESC = "Some of us were not made to be subtle.";
					}
				}
			}

			// Token: 0x020029A3 RID: 10659
			public class CLOTHING_BOTTOMS
			{
				// Token: 0x0400B520 RID: 46368
				public static LocString NAME = "Default Bottom";

				// Token: 0x0400B521 RID: 46369
				public static LocString DESC = "The default bottoms.";

				// Token: 0x020035EC RID: 13804
				public class FACADES
				{
					// Token: 0x02003A66 RID: 14950
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400E3C1 RID: 58305
						public static LocString NAME = "Basic Aqua Pants";

						// Token: 0x0400E3C2 RID: 58306
						public static LocString DESC = "A clean pair of aqua-blue pants that go with everything.";
					}

					// Token: 0x02003A67 RID: 14951
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400E3C3 RID: 58307
						public static LocString NAME = "Basic Bubblegum Pants";

						// Token: 0x0400E3C4 RID: 58308
						public static LocString DESC = "A clean pair of bubblegum-pink pants that go with everything.";
					}

					// Token: 0x02003A68 RID: 14952
					public class BASIC_GREEN
					{
						// Token: 0x0400E3C5 RID: 58309
						public static LocString NAME = "Basic Green Pants";

						// Token: 0x0400E3C6 RID: 58310
						public static LocString DESC = "A clean pair of green pants that go with everything.";
					}

					// Token: 0x02003A69 RID: 14953
					public class BASIC_ORANGE
					{
						// Token: 0x0400E3C7 RID: 58311
						public static LocString NAME = "Basic Orange Pants";

						// Token: 0x0400E3C8 RID: 58312
						public static LocString DESC = "A clean pair of orange pants that go with everything.";
					}

					// Token: 0x02003A6A RID: 14954
					public class BASIC_PURPLE
					{
						// Token: 0x0400E3C9 RID: 58313
						public static LocString NAME = "Basic Purple Pants";

						// Token: 0x0400E3CA RID: 58314
						public static LocString DESC = "A clean pair of purple pants that go with everything.";
					}

					// Token: 0x02003A6B RID: 14955
					public class BASIC_RED
					{
						// Token: 0x0400E3CB RID: 58315
						public static LocString NAME = "Basic Red Pants";

						// Token: 0x0400E3CC RID: 58316
						public static LocString DESC = "A clean pair of red pants that go with everything.";
					}

					// Token: 0x02003A6C RID: 14956
					public class BASIC_WHITE
					{
						// Token: 0x0400E3CD RID: 58317
						public static LocString NAME = "Basic White Pants";

						// Token: 0x0400E3CE RID: 58318
						public static LocString DESC = "A clean pair of white pants that go with everything.";
					}

					// Token: 0x02003A6D RID: 14957
					public class BASIC_YELLOW
					{
						// Token: 0x0400E3CF RID: 58319
						public static LocString NAME = "Basic Yellow Pants";

						// Token: 0x0400E3D0 RID: 58320
						public static LocString DESC = "A clean pair of yellow pants that go with everything.";
					}

					// Token: 0x02003A6E RID: 14958
					public class BASIC_BLACK
					{
						// Token: 0x0400E3D1 RID: 58321
						public static LocString NAME = "Basic Black Pants";

						// Token: 0x0400E3D2 RID: 58322
						public static LocString DESC = "A clean pair of black pants that go with everything.";
					}

					// Token: 0x02003A6F RID: 14959
					public class SHORTS_BASIC_DEEPRED
					{
						// Token: 0x0400E3D3 RID: 58323
						public static LocString NAME = "Team Captain Shorts";

						// Token: 0x0400E3D4 RID: 58324
						public static LocString DESC = "A fresh pair of shorts for natural leaders.";
					}

					// Token: 0x02003A70 RID: 14960
					public class SHORTS_BASIC_SATSUMA
					{
						// Token: 0x0400E3D5 RID: 58325
						public static LocString NAME = "Superfan Shorts";

						// Token: 0x0400E3D6 RID: 58326
						public static LocString DESC = "A fresh pair of shorts for long-time supporters of...shorts.";
					}

					// Token: 0x02003A71 RID: 14961
					public class SHORTS_BASIC_YELLOWCAKE
					{
						// Token: 0x0400E3D7 RID: 58327
						public static LocString NAME = "Yellowcake Shorts";

						// Token: 0x0400E3D8 RID: 58328
						public static LocString DESC = "A fresh pair of uranium-powder-colored shorts that are definitely not radioactive. Probably.";
					}

					// Token: 0x02003A72 RID: 14962
					public class SHORTS_BASIC_KELLYGREEN
					{
						// Token: 0x0400E3D9 RID: 58329
						public static LocString NAME = "Go Team Shorts";

						// Token: 0x0400E3DA RID: 58330
						public static LocString DESC = "A fresh pair of shorts for cheering from the sidelines.";
					}

					// Token: 0x02003A73 RID: 14963
					public class SHORTS_BASIC_BLUE_COBALT
					{
						// Token: 0x0400E3DB RID: 58331
						public static LocString NAME = "True Blue Shorts";

						// Token: 0x0400E3DC RID: 58332
						public static LocString DESC = "A fresh pair of shorts for the real team players.";
					}

					// Token: 0x02003A74 RID: 14964
					public class SHORTS_BASIC_PINK_FLAMINGO
					{
						// Token: 0x0400E3DD RID: 58333
						public static LocString NAME = "Pep Rally Shorts";

						// Token: 0x0400E3DE RID: 58334
						public static LocString DESC = "The peppiest pair of shorts this side of the asteroid.";
					}

					// Token: 0x02003A75 RID: 14965
					public class SHORTS_BASIC_CHARCOAL
					{
						// Token: 0x0400E3DF RID: 58335
						public static LocString NAME = "Underdog Shorts";

						// Token: 0x0400E3E0 RID: 58336
						public static LocString DESC = "A fresh pair of shorts. They're cleaner than they look.";
					}

					// Token: 0x02003A76 RID: 14966
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400E3E1 RID: 58337
						public static LocString NAME = "LED Pants";

						// Token: 0x0400E3E2 RID: 58338
						public static LocString DESC = "These legs are lit.";
					}

					// Token: 0x02003A77 RID: 14967
					public class ATHLETE
					{
						// Token: 0x0400E3E3 RID: 58339
						public static LocString NAME = "Racing Pants";

						// Token: 0x0400E3E4 RID: 58340
						public static LocString DESC = "Fast, furious fashion.";
					}

					// Token: 0x02003A78 RID: 14968
					public class BASIC_LIGHTBROWN
					{
						// Token: 0x0400E3E5 RID: 58341
						public static LocString NAME = "Basic Khaki Pants";

						// Token: 0x0400E3E6 RID: 58342
						public static LocString DESC = "Transition effortlessly from subterranean day to subterranean night.";
					}

					// Token: 0x02003A79 RID: 14969
					public class BASIC_REDORANGE
					{
						// Token: 0x0400E3E7 RID: 58343
						public static LocString NAME = "Basic Crimson Pants";

						// Token: 0x0400E3E8 RID: 58344
						public static LocString DESC = "Like red pants, but slightly fancier-sounding.";
					}

					// Token: 0x02003A7A RID: 14970
					public class GONCH_STRAWBERRY
					{
						// Token: 0x0400E3E9 RID: 58345
						public static LocString NAME = "Executive Briefs";

						// Token: 0x0400E3EA RID: 58346
						public static LocString DESC = "Bossy (under)pants.";
					}

					// Token: 0x02003A7B RID: 14971
					public class GONCH_SATSUMA
					{
						// Token: 0x0400E3EB RID: 58347
						public static LocString NAME = "Underling Briefs";

						// Token: 0x0400E3EC RID: 58348
						public static LocString DESC = "The seams are already unraveling.";
					}

					// Token: 0x02003A7C RID: 14972
					public class GONCH_LEMON
					{
						// Token: 0x0400E3ED RID: 58349
						public static LocString NAME = "Groupthink Briefs";

						// Token: 0x0400E3EE RID: 58350
						public static LocString DESC = "All the cool people are wearing them.";
					}

					// Token: 0x02003A7D RID: 14973
					public class GONCH_LIME
					{
						// Token: 0x0400E3EF RID: 58351
						public static LocString NAME = "Stakeholder Briefs";

						// Token: 0x0400E3F0 RID: 58352
						public static LocString DESC = "They're really invested in keeping the wearer comfortable.";
					}

					// Token: 0x02003A7E RID: 14974
					public class GONCH_BLUEBERRY
					{
						// Token: 0x0400E3F1 RID: 58353
						public static LocString NAME = "Admin Briefs";

						// Token: 0x0400E3F2 RID: 58354
						public static LocString DESC = "The workhorse of the underwear world.";
					}

					// Token: 0x02003A7F RID: 14975
					public class GONCH_GRAPE
					{
						// Token: 0x0400E3F3 RID: 58355
						public static LocString NAME = "Buzzword Briefs";

						// Token: 0x0400E3F4 RID: 58356
						public static LocString DESC = "Underwear that works hard, plays hard, and gives 110% to maximize the \"bottom\" line.";
					}

					// Token: 0x02003A80 RID: 14976
					public class GONCH_WATERMELON
					{
						// Token: 0x0400E3F5 RID: 58357
						public static LocString NAME = "Synergy Briefs";

						// Token: 0x0400E3F6 RID: 58358
						public static LocString DESC = "Teamwork makes the dream work.";
					}

					// Token: 0x02003A81 RID: 14977
					public class DENIM_BLUE
					{
						// Token: 0x0400E3F7 RID: 58359
						public static LocString NAME = "Jeans";

						// Token: 0x0400E3F8 RID: 58360
						public static LocString DESC = "The bottom half of a Canadian tuxedo.";
					}

					// Token: 0x02003A82 RID: 14978
					public class GI_WHITE
					{
						// Token: 0x0400E3F9 RID: 58361
						public static LocString NAME = "White Capris";

						// Token: 0x0400E3FA RID: 58362
						public static LocString DESC = "The cropped length is ideal for wading through flooded hallways.";
					}

					// Token: 0x02003A83 RID: 14979
					public class NERD_BROWN
					{
						// Token: 0x0400E3FB RID: 58363
						public static LocString NAME = "Research Pants";

						// Token: 0x0400E3FC RID: 58364
						public static LocString DESC = "The pockets are full of illegible notes that didn't quite survive the wash.";
					}

					// Token: 0x02003A84 RID: 14980
					public class SKIRT_BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400E3FD RID: 58365
						public static LocString NAME = "Aqua Rayon Skirt";

						// Token: 0x0400E3FE RID: 58366
						public static LocString DESC = "The tag says \"Dry Clean Only.\" There are no dry cleaners in space.";
					}

					// Token: 0x02003A85 RID: 14981
					public class SKIRT_BASIC_PURPLE
					{
						// Token: 0x0400E3FF RID: 58367
						public static LocString NAME = "Purple Rayon Skirt";

						// Token: 0x0400E400 RID: 58368
						public static LocString DESC = "It's not the most breathable fabric, but it <i>is</i> a lovely shade of purple.";
					}

					// Token: 0x02003A86 RID: 14982
					public class SKIRT_BASIC_GREEN
					{
						// Token: 0x0400E401 RID: 58369
						public static LocString NAME = "Olive Rayon Skirt";

						// Token: 0x0400E402 RID: 58370
						public static LocString DESC = "Designed not to get snagged on ladders.";
					}

					// Token: 0x02003A87 RID: 14983
					public class SKIRT_BASIC_ORANGE
					{
						// Token: 0x0400E403 RID: 58371
						public static LocString NAME = "Apricot Rayon Skirt";

						// Token: 0x0400E404 RID: 58372
						public static LocString DESC = "Ready for spontaneous workplace twirling.";
					}

					// Token: 0x02003A88 RID: 14984
					public class SKIRT_BASIC_PINK_ORCHID
					{
						// Token: 0x0400E405 RID: 58373
						public static LocString NAME = "Bubblegum Rayon Skirt";

						// Token: 0x0400E406 RID: 58374
						public static LocString DESC = "The bubblegum scent lasts 100 washes!";
					}

					// Token: 0x02003A89 RID: 14985
					public class SKIRT_BASIC_RED
					{
						// Token: 0x0400E407 RID: 58375
						public static LocString NAME = "Garnet Rayon Skirt";

						// Token: 0x0400E408 RID: 58376
						public static LocString DESC = "It's business time.";
					}

					// Token: 0x02003A8A RID: 14986
					public class SKIRT_BASIC_YELLOW
					{
						// Token: 0x0400E409 RID: 58377
						public static LocString NAME = "Yellow Rayon Skirt";

						// Token: 0x0400E40A RID: 58378
						public static LocString DESC = "A formerly white skirt that has not aged well.";
					}

					// Token: 0x02003A8B RID: 14987
					public class SKIRT_BASIC_POLKADOT
					{
						// Token: 0x0400E40B RID: 58379
						public static LocString NAME = "Polka Dot Skirt";

						// Token: 0x0400E40C RID: 58380
						public static LocString DESC = "Polka dots are a way to infinity.";
					}

					// Token: 0x02003A8C RID: 14988
					public class SKIRT_BASIC_WATERMELON
					{
						// Token: 0x0400E40D RID: 58381
						public static LocString NAME = "Picnic Skirt";

						// Token: 0x0400E40E RID: 58382
						public static LocString DESC = "The seeds are spittable, but will bear no fruit.";
					}

					// Token: 0x02003A8D RID: 14989
					public class SKIRT_DENIM_BLUE
					{
						// Token: 0x0400E40F RID: 58383
						public static LocString NAME = "Denim Tux Skirt";

						// Token: 0x0400E410 RID: 58384
						public static LocString DESC = "Designed for the casual red carpet.";
					}

					// Token: 0x02003A8E RID: 14990
					public class SKIRT_LEOPARD_PRINT_BLUE_PINK
					{
						// Token: 0x0400E411 RID: 58385
						public static LocString NAME = "Disco Leopard Skirt";

						// Token: 0x0400E412 RID: 58386
						public static LocString DESC = "A faux-fur party staple.";
					}

					// Token: 0x02003A8F RID: 14991
					public class SKIRT_SPARKLE_BLUE
					{
						// Token: 0x0400E413 RID: 58387
						public static LocString NAME = "Blue Tinsel Skirt";

						// Token: 0x0400E414 RID: 58388
						public static LocString DESC = "The tinsel is scratchy, but look how shiny!";
					}

					// Token: 0x02003A90 RID: 14992
					public class BASIC_ORANGE_SATSUMA
					{
						// Token: 0x0400E415 RID: 58389
						public static LocString NAME = "Hi-Vis Pants";

						// Token: 0x0400E416 RID: 58390
						public static LocString DESC = "They make the wearer feel truly seen.";
					}

					// Token: 0x02003A91 RID: 14993
					public class PINSTRIPE_SLATE
					{
						// Token: 0x0400E417 RID: 58391
						public static LocString NAME = "Nobel Pinstripe Trousers";

						// Token: 0x0400E418 RID: 58392
						public static LocString DESC = "There's a waterproof pocket to keep acceptance speeches smudge-free.";
					}

					// Token: 0x02003A92 RID: 14994
					public class VELOUR_BLACK
					{
						// Token: 0x0400E419 RID: 58393
						public static LocString NAME = "Black Velour Trousers";

						// Token: 0x0400E41A RID: 58394
						public static LocString DESC = "Fuzzy, formal and finely cut.";
					}

					// Token: 0x02003A93 RID: 14995
					public class VELOUR_BLUE
					{
						// Token: 0x0400E41B RID: 58395
						public static LocString NAME = "Shortwave Velour Pants";

						// Token: 0x0400E41C RID: 58396
						public static LocString DESC = "Formal wear with a sensory side.";
					}

					// Token: 0x02003A94 RID: 14996
					public class VELOUR_PINK
					{
						// Token: 0x0400E41D RID: 58397
						public static LocString NAME = "Gamma Velour Pants";

						// Token: 0x0400E41E RID: 58398
						public static LocString DESC = "They're stretchy <i>and</i> flame retardant.";
					}

					// Token: 0x02003A95 RID: 14997
					public class SKIRT_BALLERINA_PINK
					{
						// Token: 0x0400E41F RID: 58399
						public static LocString NAME = "Ballet Tutu";

						// Token: 0x0400E420 RID: 58400
						public static LocString DESC = "A tulle skirt spun and assembled by an army of patent-pending nanobots.";
					}

					// Token: 0x02003A96 RID: 14998
					public class SKIRT_TWEED_PINK_ORCHID
					{
						// Token: 0x0400E421 RID: 58401
						public static LocString NAME = "Power Brunch Skirt";

						// Token: 0x0400E422 RID: 58402
						public static LocString DESC = "It has pockets!";
					}

					// Token: 0x02003A97 RID: 14999
					public class GINCH_PINK_GLUON
					{
						// Token: 0x0400E423 RID: 58403
						public static LocString NAME = "Gluon Shorties";

						// Token: 0x0400E424 RID: 58404
						public static LocString DESC = "Comfy pink short-shorts with a ruffled hem.";
					}

					// Token: 0x02003A98 RID: 15000
					public class GINCH_PURPLE_CORTEX
					{
						// Token: 0x0400E425 RID: 58405
						public static LocString NAME = "Cortex Shorties";

						// Token: 0x0400E426 RID: 58406
						public static LocString DESC = "Comfy purple short-shorts with a ruffled hem.";
					}

					// Token: 0x02003A99 RID: 15001
					public class GINCH_BLUE_FROSTY
					{
						// Token: 0x0400E427 RID: 58407
						public static LocString NAME = "Frosty Shorties";

						// Token: 0x0400E428 RID: 58408
						public static LocString DESC = "Icy blue short-shorts with a ruffled hem.";
					}

					// Token: 0x02003A9A RID: 15002
					public class GINCH_TEAL_LOCUS
					{
						// Token: 0x0400E429 RID: 58409
						public static LocString NAME = "Locus Shorties";

						// Token: 0x0400E42A RID: 58410
						public static LocString DESC = "Comfy teal short-shorts with a ruffled hem.";
					}

					// Token: 0x02003A9B RID: 15003
					public class GINCH_GREEN_GOOP
					{
						// Token: 0x0400E42B RID: 58411
						public static LocString NAME = "Goop Shorties";

						// Token: 0x0400E42C RID: 58412
						public static LocString DESC = "Short-shorts with a ruffled hem and one pocket full of melted snacks.";
					}

					// Token: 0x02003A9C RID: 15004
					public class GINCH_YELLOW_BILE
					{
						// Token: 0x0400E42D RID: 58413
						public static LocString NAME = "Bile Shorties";

						// Token: 0x0400E42E RID: 58414
						public static LocString DESC = "Ruffled short-shorts in a stomach-turning shade of yellow.";
					}

					// Token: 0x02003A9D RID: 15005
					public class GINCH_ORANGE_NYBBLE
					{
						// Token: 0x0400E42F RID: 58415
						public static LocString NAME = "Nybble Shorties";

						// Token: 0x0400E430 RID: 58416
						public static LocString DESC = "Comfy orange ruffled short-shorts for computer scientists.";
					}

					// Token: 0x02003A9E RID: 15006
					public class GINCH_RED_IRONBOW
					{
						// Token: 0x0400E431 RID: 58417
						public static LocString NAME = "Ironbow Shorties";

						// Token: 0x0400E432 RID: 58418
						public static LocString DESC = "Comfy red short-shorts with a ruffled hem.";
					}

					// Token: 0x02003A9F RID: 15007
					public class GINCH_GREY_PHLEGM
					{
						// Token: 0x0400E433 RID: 58419
						public static LocString NAME = "Phlegmy Shorties";

						// Token: 0x0400E434 RID: 58420
						public static LocString DESC = "Ruffled short-shorts in a rather sticky shade of light grey.";
					}

					// Token: 0x02003AA0 RID: 15008
					public class GINCH_GREY_OBELUS
					{
						// Token: 0x0400E435 RID: 58421
						public static LocString NAME = "Obelus Shorties";

						// Token: 0x0400E436 RID: 58422
						public static LocString DESC = "Comfy grey short-shorts with a ruffled hem.";
					}

					// Token: 0x02003AA1 RID: 15009
					public class KNIT_POLKADOT_TURQ
					{
						// Token: 0x0400E437 RID: 58423
						public static LocString NAME = "Polka Dot Track Pants";

						// Token: 0x0400E438 RID: 58424
						public static LocString DESC = "For clowning around during mandatory physical fitness week.";
					}

					// Token: 0x02003AA2 RID: 15010
					public class GI_BELT_WHITE_BLACK
					{
						// Token: 0x0400E439 RID: 58425
						public static LocString NAME = "Rebel Gi Pants";

						// Token: 0x0400E43A RID: 58426
						public static LocString DESC = "Relaxed-fit pants designed for roundhouse kicks.";
					}

					// Token: 0x02003AA3 RID: 15011
					public class BELT_KHAKI_TAN
					{
						// Token: 0x0400E43B RID: 58427
						public static LocString NAME = "HVAC Khaki Pants";

						// Token: 0x0400E43C RID: 58428
						public static LocString DESC = "Rip-resistant fabric makes crawling through ducts a breeze.";
					}
				}
			}

			// Token: 0x020029A4 RID: 10660
			public class CLOTHING_SHOES
			{
				// Token: 0x0400B522 RID: 46370
				public static LocString NAME = "Default Footwear";

				// Token: 0x0400B523 RID: 46371
				public static LocString DESC = "The default style of footwear.";

				// Token: 0x020035ED RID: 13805
				public class FACADES
				{
					// Token: 0x02003AA4 RID: 15012
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400E43D RID: 58429
						public static LocString NAME = "Basic Aqua Shoes";

						// Token: 0x0400E43E RID: 58430
						public static LocString DESC = "A fresh pair of aqua-blue shoes that go with everything.";
					}

					// Token: 0x02003AA5 RID: 15013
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400E43F RID: 58431
						public static LocString NAME = "Basic Bubblegum Shoes";

						// Token: 0x0400E440 RID: 58432
						public static LocString DESC = "A fresh pair of bubblegum-pink shoes that go with everything.";
					}

					// Token: 0x02003AA6 RID: 15014
					public class BASIC_GREEN
					{
						// Token: 0x0400E441 RID: 58433
						public static LocString NAME = "Basic Green Shoes";

						// Token: 0x0400E442 RID: 58434
						public static LocString DESC = "A fresh pair of green shoes that go with everything.";
					}

					// Token: 0x02003AA7 RID: 15015
					public class BASIC_ORANGE
					{
						// Token: 0x0400E443 RID: 58435
						public static LocString NAME = "Basic Orange Shoes";

						// Token: 0x0400E444 RID: 58436
						public static LocString DESC = "A fresh pair of orange shoes that go with everything.";
					}

					// Token: 0x02003AA8 RID: 15016
					public class BASIC_PURPLE
					{
						// Token: 0x0400E445 RID: 58437
						public static LocString NAME = "Basic Purple Shoes";

						// Token: 0x0400E446 RID: 58438
						public static LocString DESC = "A fresh pair of purple shoes that go with everything.";
					}

					// Token: 0x02003AA9 RID: 15017
					public class BASIC_RED
					{
						// Token: 0x0400E447 RID: 58439
						public static LocString NAME = "Basic Red Shoes";

						// Token: 0x0400E448 RID: 58440
						public static LocString DESC = "A fresh pair of red shoes that go with everything.";
					}

					// Token: 0x02003AAA RID: 15018
					public class BASIC_WHITE
					{
						// Token: 0x0400E449 RID: 58441
						public static LocString NAME = "Basic White Shoes";

						// Token: 0x0400E44A RID: 58442
						public static LocString DESC = "A fresh pair of white shoes that go with everything.";
					}

					// Token: 0x02003AAB RID: 15019
					public class BASIC_YELLOW
					{
						// Token: 0x0400E44B RID: 58443
						public static LocString NAME = "Basic Yellow Shoes";

						// Token: 0x0400E44C RID: 58444
						public static LocString DESC = "A fresh pair of yellow shoes that go with everything.";
					}

					// Token: 0x02003AAC RID: 15020
					public class BASIC_BLACK
					{
						// Token: 0x0400E44D RID: 58445
						public static LocString NAME = "Basic Black Shoes";

						// Token: 0x0400E44E RID: 58446
						public static LocString DESC = "A fresh pair of black shoes that go with everything.";
					}

					// Token: 0x02003AAD RID: 15021
					public class BASIC_BLUEGREY
					{
						// Token: 0x0400E44F RID: 58447
						public static LocString NAME = "Basic Gunmetal Shoes";

						// Token: 0x0400E450 RID: 58448
						public static LocString DESC = "A fresh pair of pastel shoes that go with everything.";
					}

					// Token: 0x02003AAE RID: 15022
					public class BASIC_TAN
					{
						// Token: 0x0400E451 RID: 58449
						public static LocString NAME = "Basic Tan Shoes";

						// Token: 0x0400E452 RID: 58450
						public static LocString DESC = "They're remarkably unremarkable.";
					}

					// Token: 0x02003AAF RID: 15023
					public class SOCKS_ATHLETIC_DEEPRED
					{
						// Token: 0x0400E453 RID: 58451
						public static LocString NAME = "Team Captain Gym Socks";

						// Token: 0x0400E454 RID: 58452
						public static LocString DESC = "Breathable socks with sporty red stripes.";
					}

					// Token: 0x02003AB0 RID: 15024
					public class SOCKS_ATHLETIC_SATSUMA
					{
						// Token: 0x0400E455 RID: 58453
						public static LocString NAME = "Superfan Gym Socks";

						// Token: 0x0400E456 RID: 58454
						public static LocString DESC = "Breathable socks with sporty orange stripes.";
					}

					// Token: 0x02003AB1 RID: 15025
					public class SOCKS_ATHLETIC_LEMON
					{
						// Token: 0x0400E457 RID: 58455
						public static LocString NAME = "Hype Gym Socks";

						// Token: 0x0400E458 RID: 58456
						public static LocString DESC = "Breathable socks with sporty yellow stripes.";
					}

					// Token: 0x02003AB2 RID: 15026
					public class SOCKS_ATHLETIC_KELLYGREEN
					{
						// Token: 0x0400E459 RID: 58457
						public static LocString NAME = "Go Team Gym Socks";

						// Token: 0x0400E45A RID: 58458
						public static LocString DESC = "Breathable socks with sporty green stripes.";
					}

					// Token: 0x02003AB3 RID: 15027
					public class SOCKS_ATHLETIC_COBALT
					{
						// Token: 0x0400E45B RID: 58459
						public static LocString NAME = "True Blue Gym Socks";

						// Token: 0x0400E45C RID: 58460
						public static LocString DESC = "Breathable socks with sporty blue stripes.";
					}

					// Token: 0x02003AB4 RID: 15028
					public class SOCKS_ATHLETIC_FLAMINGO
					{
						// Token: 0x0400E45D RID: 58461
						public static LocString NAME = "Pep Rally Gym Socks";

						// Token: 0x0400E45E RID: 58462
						public static LocString DESC = "Breathable socks with sporty pink stripes.";
					}

					// Token: 0x02003AB5 RID: 15029
					public class SOCKS_ATHLETIC_CHARCOAL
					{
						// Token: 0x0400E45F RID: 58463
						public static LocString NAME = "Underdog Gym Socks";

						// Token: 0x0400E460 RID: 58464
						public static LocString DESC = "Breathable socks that do nothing whatsoever to eliminate foot odor.";
					}

					// Token: 0x02003AB6 RID: 15030
					public class BASIC_GREY
					{
						// Token: 0x0400E461 RID: 58465
						public static LocString NAME = "Basic Gray Shoes";

						// Token: 0x0400E462 RID: 58466
						public static LocString DESC = "A fresh pair of gray shoes that go with everything.";
					}

					// Token: 0x02003AB7 RID: 15031
					public class DENIM_BLUE
					{
						// Token: 0x0400E463 RID: 58467
						public static LocString NAME = "Denim Shoes";

						// Token: 0x0400E464 RID: 58468
						public static LocString DESC = "Not technically essential for a Canadian tuxedo, but why not?";
					}

					// Token: 0x02003AB8 RID: 15032
					public class LEGWARMERS_STRAWBERRY
					{
						// Token: 0x0400E465 RID: 58469
						public static LocString NAME = "Slouchy Strawberry Socks";

						// Token: 0x0400E466 RID: 58470
						public static LocString DESC = "Freckly knitted socks that don't stay up.";
					}

					// Token: 0x02003AB9 RID: 15033
					public class LEGWARMERS_SATSUMA
					{
						// Token: 0x0400E467 RID: 58471
						public static LocString NAME = "Slouchy Satsuma Socks";

						// Token: 0x0400E468 RID: 58472
						public static LocString DESC = "Sweet knitted socks for spontaneous dance segments.";
					}

					// Token: 0x02003ABA RID: 15034
					public class LEGWARMERS_LEMON
					{
						// Token: 0x0400E469 RID: 58473
						public static LocString NAME = "Slouchy Lemon Socks";

						// Token: 0x0400E46A RID: 58474
						public static LocString DESC = "Zesty knitted socks that don't stay up.";
					}

					// Token: 0x02003ABB RID: 15035
					public class LEGWARMERS_LIME
					{
						// Token: 0x0400E46B RID: 58475
						public static LocString NAME = "Slouchy Lime Socks";

						// Token: 0x0400E46C RID: 58476
						public static LocString DESC = "Juicy knitted socks that don't stay up.";
					}

					// Token: 0x02003ABC RID: 15036
					public class LEGWARMERS_BLUEBERRY
					{
						// Token: 0x0400E46D RID: 58477
						public static LocString NAME = "Slouchy Blueberry Socks";

						// Token: 0x0400E46E RID: 58478
						public static LocString DESC = "Knitted socks with a fun bobble-stitch texture.";
					}

					// Token: 0x02003ABD RID: 15037
					public class LEGWARMERS_GRAPE
					{
						// Token: 0x0400E46F RID: 58479
						public static LocString NAME = "Slouchy Grape Socks";

						// Token: 0x0400E470 RID: 58480
						public static LocString DESC = "These fabulous knitted socks that don't stay up are really raisin the bar.";
					}

					// Token: 0x02003ABE RID: 15038
					public class LEGWARMERS_WATERMELON
					{
						// Token: 0x0400E471 RID: 58481
						public static LocString NAME = "Slouchy Watermelon Socks";

						// Token: 0x0400E472 RID: 58482
						public static LocString DESC = "Summery knitted socks that don't stay up.";
					}

					// Token: 0x02003ABF RID: 15039
					public class BALLERINA_PINK
					{
						// Token: 0x0400E473 RID: 58483
						public static LocString NAME = "Ballet Shoes";

						// Token: 0x0400E474 RID: 58484
						public static LocString DESC = "There's no \"pointe\" in aiming for anything less than perfection.";
					}

					// Token: 0x02003AC0 RID: 15040
					public class MARYJANE_SOCKS_BW
					{
						// Token: 0x0400E475 RID: 58485
						public static LocString NAME = "Frilly Sock Shoes";

						// Token: 0x0400E476 RID: 58486
						public static LocString DESC = "They add a little <i>je ne sais quoi</i> to everyday lab wear.";
					}

					// Token: 0x02003AC1 RID: 15041
					public class CLASSICFLATS_CREAM_CHARCOAL
					{
						// Token: 0x0400E477 RID: 58487
						public static LocString NAME = "Dressy Shoes";

						// Token: 0x0400E478 RID: 58488
						public static LocString DESC = "An enduring style, for enduring endless small talk.";
					}

					// Token: 0x02003AC2 RID: 15042
					public class VELOUR_BLUE
					{
						// Token: 0x0400E479 RID: 58489
						public static LocString NAME = "Shortwave Velour Shoes";

						// Token: 0x0400E47A RID: 58490
						public static LocString DESC = "Not the easiest to keep clean.";
					}

					// Token: 0x02003AC3 RID: 15043
					public class VELOUR_PINK
					{
						// Token: 0x0400E47B RID: 58491
						public static LocString NAME = "Gamma Velour Shoes";

						// Token: 0x0400E47C RID: 58492
						public static LocString DESC = "Finally, a pair of work-appropriate fuzzy shoes.";
					}

					// Token: 0x02003AC4 RID: 15044
					public class VELOUR_BLACK
					{
						// Token: 0x0400E47D RID: 58493
						public static LocString NAME = "Black Velour Shoes";

						// Token: 0x0400E47E RID: 58494
						public static LocString DESC = "Matching velour lining gently tickles feet with every step.";
					}

					// Token: 0x02003AC5 RID: 15045
					public class FLASHY
					{
						// Token: 0x0400E47F RID: 58495
						public static LocString NAME = "Superstar Shoes";

						// Token: 0x0400E480 RID: 58496
						public static LocString DESC = "Why walk when you can <i>moon</i>walk?";
					}

					// Token: 0x02003AC6 RID: 15046
					public class GINCH_PINK_SALTROCK
					{
						// Token: 0x0400E481 RID: 58497
						public static LocString NAME = "Frilly Saltrock Socks";

						// Token: 0x0400E482 RID: 58498
						public static LocString DESC = "Thick, soft pink socks with extra flounce.";
					}

					// Token: 0x02003AC7 RID: 15047
					public class GINCH_PURPLE_DUSKY
					{
						// Token: 0x0400E483 RID: 58499
						public static LocString NAME = "Frilly Dusk Socks";

						// Token: 0x0400E484 RID: 58500
						public static LocString DESC = "Thick, soft purple socks with extra flounce.";
					}

					// Token: 0x02003AC8 RID: 15048
					public class GINCH_BLUE_BASIN
					{
						// Token: 0x0400E485 RID: 58501
						public static LocString NAME = "Frilly Basin Socks";

						// Token: 0x0400E486 RID: 58502
						public static LocString DESC = "Thick, soft blue socks with extra flounce.";
					}

					// Token: 0x02003AC9 RID: 15049
					public class GINCH_TEAL_BALMY
					{
						// Token: 0x0400E487 RID: 58503
						public static LocString NAME = "Frilly Balm Socks";

						// Token: 0x0400E488 RID: 58504
						public static LocString DESC = "Thick, soothing teal socks with extra flounce.";
					}

					// Token: 0x02003ACA RID: 15050
					public class GINCH_GREEN_LIME
					{
						// Token: 0x0400E489 RID: 58505
						public static LocString NAME = "Frilly Leach Socks";

						// Token: 0x0400E48A RID: 58506
						public static LocString DESC = "Thick, soft green socks with extra flounce.";
					}

					// Token: 0x02003ACB RID: 15051
					public class GINCH_YELLOW_YELLOWCAKE
					{
						// Token: 0x0400E48B RID: 58507
						public static LocString NAME = "Frilly Yellowcake Socks";

						// Token: 0x0400E48C RID: 58508
						public static LocString DESC = "Dangerously soft yellow socks with extra flounce.";
					}

					// Token: 0x02003ACC RID: 15052
					public class GINCH_ORANGE_ATOMIC
					{
						// Token: 0x0400E48D RID: 58509
						public static LocString NAME = "Frilly Atomic Socks";

						// Token: 0x0400E48E RID: 58510
						public static LocString DESC = "Thick, soft orange socks with extra flounce.";
					}

					// Token: 0x02003ACD RID: 15053
					public class GINCH_RED_MAGMA
					{
						// Token: 0x0400E48F RID: 58511
						public static LocString NAME = "Frilly Magma Socks";

						// Token: 0x0400E490 RID: 58512
						public static LocString DESC = "Thick, toasty red socks with extra flounce.";
					}

					// Token: 0x02003ACE RID: 15054
					public class GINCH_GREY_GREY
					{
						// Token: 0x0400E491 RID: 58513
						public static LocString NAME = "Frilly Slate Socks";

						// Token: 0x0400E492 RID: 58514
						public static LocString DESC = "Thick, soft grey socks with extra flounce.";
					}

					// Token: 0x02003ACF RID: 15055
					public class GINCH_GREY_CHARCOAL
					{
						// Token: 0x0400E493 RID: 58515
						public static LocString NAME = "Frilly Charcoal Socks";

						// Token: 0x0400E494 RID: 58516
						public static LocString DESC = "Thick, soft dark grey socks with extra flounce.";
					}
				}
			}

			// Token: 0x020029A5 RID: 10661
			public class CLOTHING_HATS
			{
				// Token: 0x0400B524 RID: 46372
				public static LocString NAME = "Default Headgear";

				// Token: 0x0400B525 RID: 46373
				public static LocString DESC = "<DESC>";

				// Token: 0x020035EE RID: 13806
				public class FACADES
				{
				}
			}

			// Token: 0x020029A6 RID: 10662
			public class CLOTHING_ACCESORIES
			{
				// Token: 0x0400B526 RID: 46374
				public static LocString NAME = "Default Accessory";

				// Token: 0x0400B527 RID: 46375
				public static LocString DESC = "<DESC>";

				// Token: 0x020035EF RID: 13807
				public class FACADES
				{
				}
			}

			// Token: 0x020029A7 RID: 10663
			public class OXYGEN_TANK
			{
				// Token: 0x0400B528 RID: 46376
				public static LocString NAME = UI.FormatAsLink("Oxygen Tank", "OXYGEN_TANK");

				// Token: 0x0400B529 RID: 46377
				public static LocString GENERICNAME = "Equipment";

				// Token: 0x0400B52A RID: 46378
				public static LocString DESC = "It's like a to-go bag for your lungs.";

				// Token: 0x0400B52B RID: 46379
				public static LocString EFFECT = "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>.";

				// Token: 0x0400B52C RID: 46380
				public static LocString RECIPE_DESC = "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>";
			}

			// Token: 0x020029A8 RID: 10664
			public class OXYGEN_TANK_UNDERWATER
			{
				// Token: 0x0400B52D RID: 46381
				public static LocString NAME = "Oxygen Rebreather";

				// Token: 0x0400B52E RID: 46382
				public static LocString GENERICNAME = "Equipment";

				// Token: 0x0400B52F RID: 46383
				public static LocString DESC = "";

				// Token: 0x0400B530 RID: 46384
				public static LocString EFFECT = "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid.";

				// Token: 0x0400B531 RID: 46385
				public static LocString RECIPE_DESC = "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid";
			}

			// Token: 0x020029A9 RID: 10665
			public class EQUIPPABLEBALLOON
			{
				// Token: 0x0400B532 RID: 46386
				public static LocString NAME = UI.FormatAsLink("Balloon Friend", "EQUIPPABLEBALLOON");

				// Token: 0x0400B533 RID: 46387
				public static LocString DESC = "A floating friend to reassure my Duplicants they are so very, very clever.";

				// Token: 0x0400B534 RID: 46388
				public static LocString EFFECT = "Gives Duplicants a boost in brain function.\n\nSupplied by Duplicants with the Balloon Artist " + UI.FormatAsLink("Overjoyed", "MORALE") + " response.";

				// Token: 0x0400B535 RID: 46389
				public static LocString RECIPE_DESC = "Gives Duplicants a boost in brain function.\n\nSupplied by Duplicants with the Balloon Artist " + UI.FormatAsLink("Overjoyed", "MORALE") + " response";

				// Token: 0x0400B536 RID: 46390
				public static LocString GENERICNAME = "Balloon Friend";

				// Token: 0x020035F0 RID: 13808
				public class FACADES
				{
					// Token: 0x02003AD0 RID: 15056
					public class DEFAULT_BALLOON
					{
						// Token: 0x0400E495 RID: 58517
						public static LocString NAME = UI.FormatAsLink("Balloon Friend", "EQUIPPABLEBALLOON");

						// Token: 0x0400E496 RID: 58518
						public static LocString DESC = "A floating friend to reassure my Duplicants that they are so very, very clever.";
					}

					// Token: 0x02003AD1 RID: 15057
					public class BALLOON_FIREENGINE_LONG_SPARKLES
					{
						// Token: 0x0400E497 RID: 58519
						public static LocString NAME = UI.FormatAsLink("Magma Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400E498 RID: 58520
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x02003AD2 RID: 15058
					public class BALLOON_YELLOW_LONG_SPARKLES
					{
						// Token: 0x0400E499 RID: 58521
						public static LocString NAME = UI.FormatAsLink("Lavatory Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400E49A RID: 58522
						public static LocString DESC = "Sparkly balloons in an all-too-familiar hue.";
					}

					// Token: 0x02003AD3 RID: 15059
					public class BALLOON_BLUE_LONG_SPARKLES
					{
						// Token: 0x0400E49B RID: 58523
						public static LocString NAME = UI.FormatAsLink("Wheezewort Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400E49C RID: 58524
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x02003AD4 RID: 15060
					public class BALLOON_GREEN_LONG_SPARKLES
					{
						// Token: 0x0400E49D RID: 58525
						public static LocString NAME = UI.FormatAsLink("Mush Bar Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400E49E RID: 58526
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x02003AD5 RID: 15061
					public class BALLOON_PINK_LONG_SPARKLES
					{
						// Token: 0x0400E49F RID: 58527
						public static LocString NAME = UI.FormatAsLink("Petal Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4A0 RID: 58528
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x02003AD6 RID: 15062
					public class BALLOON_PURPLE_LONG_SPARKLES
					{
						// Token: 0x0400E4A1 RID: 58529
						public static LocString NAME = UI.FormatAsLink("Dusky Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4A2 RID: 58530
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x02003AD7 RID: 15063
					public class BALLOON_BABY_PACU_EGG
					{
						// Token: 0x0400E4A3 RID: 58531
						public static LocString NAME = UI.FormatAsLink("Floatie Fish", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4A4 RID: 58532
						public static LocString DESC = "They do not taste as good as the real thing.";
					}

					// Token: 0x02003AD8 RID: 15064
					public class BALLOON_BABY_GLOSSY_DRECKO_EGG
					{
						// Token: 0x0400E4A5 RID: 58533
						public static LocString NAME = UI.FormatAsLink("Glossy Glee", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4A6 RID: 58534
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02003AD9 RID: 15065
					public class BALLOON_BABY_HATCH_EGG
					{
						// Token: 0x0400E4A7 RID: 58535
						public static LocString NAME = UI.FormatAsLink("Helium Hatches", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4A8 RID: 58536
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02003ADA RID: 15066
					public class BALLOON_BABY_POKESHELL_EGG
					{
						// Token: 0x0400E4A9 RID: 58537
						public static LocString NAME = UI.FormatAsLink("Peppy Pokeshells", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4AA RID: 58538
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02003ADB RID: 15067
					public class BALLOON_BABY_PUFT_EGG
					{
						// Token: 0x0400E4AB RID: 58539
						public static LocString NAME = UI.FormatAsLink("Puffed-Up Pufts", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4AC RID: 58540
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02003ADC RID: 15068
					public class BALLOON_BABY_SHOVOLE_EGG
					{
						// Token: 0x0400E4AD RID: 58541
						public static LocString NAME = UI.FormatAsLink("Voley Voley Voles", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4AE RID: 58542
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02003ADD RID: 15069
					public class BALLOON_BABY_PIP_EGG
					{
						// Token: 0x0400E4AF RID: 58543
						public static LocString NAME = UI.FormatAsLink("Pip Pip Hooray", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4B0 RID: 58544
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x02003ADE RID: 15070
					public class CANDY_BLUEBERRY
					{
						// Token: 0x0400E4B1 RID: 58545
						public static LocString NAME = UI.FormatAsLink("Candied Blueberry", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4B2 RID: 58546
						public static LocString DESC = "A juicy bunch of blueberry-scented balloons.";
					}

					// Token: 0x02003ADF RID: 15071
					public class CANDY_GRAPE
					{
						// Token: 0x0400E4B3 RID: 58547
						public static LocString NAME = UI.FormatAsLink("Candied Grape", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4B4 RID: 58548
						public static LocString DESC = "A juicy bunch of grape-scented balloons.";
					}

					// Token: 0x02003AE0 RID: 15072
					public class CANDY_LEMON
					{
						// Token: 0x0400E4B5 RID: 58549
						public static LocString NAME = UI.FormatAsLink("Candied Lemon", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4B6 RID: 58550
						public static LocString DESC = "A juicy lemon-scented bunch of balloons.";
					}

					// Token: 0x02003AE1 RID: 15073
					public class CANDY_LIME
					{
						// Token: 0x0400E4B7 RID: 58551
						public static LocString NAME = UI.FormatAsLink("Candied Lime", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4B8 RID: 58552
						public static LocString DESC = "A juicy lime-scented bunch of balloons.";
					}

					// Token: 0x02003AE2 RID: 15074
					public class CANDY_ORANGE
					{
						// Token: 0x0400E4B9 RID: 58553
						public static LocString NAME = UI.FormatAsLink("Candied Satsuma", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4BA RID: 58554
						public static LocString DESC = "A juicy satsuma-scented bunch of balloons.";
					}

					// Token: 0x02003AE3 RID: 15075
					public class CANDY_STRAWBERRY
					{
						// Token: 0x0400E4BB RID: 58555
						public static LocString NAME = UI.FormatAsLink("Candied Strawberry", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4BC RID: 58556
						public static LocString DESC = "A juicy strawberry-scented bunch of balloons.";
					}

					// Token: 0x02003AE4 RID: 15076
					public class CANDY_WATERMELON
					{
						// Token: 0x0400E4BD RID: 58557
						public static LocString NAME = UI.FormatAsLink("Candied Watermelon", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4BE RID: 58558
						public static LocString DESC = "A juicy watermelon-scented bunch of balloons.";
					}

					// Token: 0x02003AE5 RID: 15077
					public class HAND_GOLD
					{
						// Token: 0x0400E4BF RID: 58559
						public static LocString NAME = UI.FormatAsLink("Gold Fingers", "EQUIPPABLEBALLOON");

						// Token: 0x0400E4C0 RID: 58560
						public static LocString DESC = "Inflatable gestures of encouragement.";
					}
				}
			}

			// Token: 0x020029AA RID: 10666
			public class SLEEPCLINICPAJAMAS
			{
				// Token: 0x0400B537 RID: 46391
				public static LocString NAME = UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS");

				// Token: 0x0400B538 RID: 46392
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400B539 RID: 46393
				public static LocString DESC = "A soft, fleecy ticket to dreamland.";

				// Token: 0x0400B53A RID: 46394
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Helps Duplicants fall asleep by reducing ",
					UI.FormatAsLink("Stamina", "STAMINA"),
					".\n\nEnables the wearer to dream and produce ",
					UI.FormatAsLink("Dream Journals", "DREAMJOURNAL"),
					"."
				});

				// Token: 0x0400B53B RID: 46395
				public static LocString DESTROY_TOAST = "Ripped Pajamas";
			}
		}
	}
}
