using System;

namespace STRINGS
{
	// Token: 0x02000F12 RID: 3858
	public class GAMEPLAY_EVENTS
	{
		// Token: 0x04005902 RID: 22786
		public static LocString CANCELED = "{0} Canceled";

		// Token: 0x04005903 RID: 22787
		public static LocString CANCELED_TOOLTIP = "The {0} event was canceled";

		// Token: 0x04005904 RID: 22788
		public static LocString DEFAULT_OPTION_NAME = "OK";

		// Token: 0x04005905 RID: 22789
		public static LocString DEFAULT_OPTION_CONSIDER_NAME = "Let me think about it";

		// Token: 0x04005906 RID: 22790
		public static LocString CHAIN_EVENT_TOOLTIP = "This event is a chain event";

		// Token: 0x04005907 RID: 22791
		public static LocString BONUS_EVENT_DESCRIPTION = "{effects} for {duration}";

		// Token: 0x0200218E RID: 8590
		public class LOCATIONS
		{
			// Token: 0x040098C8 RID: 39112
			public static LocString NONE_AVAILABLE = "No location currently available";

			// Token: 0x040098C9 RID: 39113
			public static LocString SUN = "The Sun";

			// Token: 0x040098CA RID: 39114
			public static LocString SURFACE = "Planetary Surface";

			// Token: 0x040098CB RID: 39115
			public static LocString PRINTING_POD = BUILDINGS.PREFABS.HEADQUARTERS.NAME;

			// Token: 0x040098CC RID: 39116
			public static LocString COLONY_WIDE = "Colonywide";
		}

		// Token: 0x0200218F RID: 8591
		public class TIMES
		{
			// Token: 0x040098CD RID: 39117
			public static LocString NOW = "Right now";

			// Token: 0x040098CE RID: 39118
			public static LocString IN_CYCLES = "In {0} cycles";

			// Token: 0x040098CF RID: 39119
			public static LocString UNKNOWN = "Sometime";
		}

		// Token: 0x02002190 RID: 8592
		public class EVENT_TYPES
		{
			// Token: 0x02002C3F RID: 11327
			public class PARTY
			{
				// Token: 0x0400C0DF RID: 49375
				public static LocString NAME = "Party";

				// Token: 0x0400C0E0 RID: 49376
				public static LocString DESCRIPTION = "THIS EVENT IS NOT WORKING\n{host} is throwing a birthday party for {dupe}. Make sure there is an available " + ROOMS.TYPES.REC_ROOM.NAME + " for the party.\n\nSocial events are good for Duplicant morale. Rejecting this party will hurt {host} and {dupe}'s fragile ego.";

				// Token: 0x0400C0E1 RID: 49377
				public static LocString CANCELED_NO_ROOM_TITLE = "Party Canceled";

				// Token: 0x0400C0E2 RID: 49378
				public static LocString CANCELED_NO_ROOM_DESCRIPTION = "The party was canceled because no " + ROOMS.TYPES.REC_ROOM.NAME + " was available.";

				// Token: 0x0400C0E3 RID: 49379
				public static LocString UNDERWAY = "Party Happening";

				// Token: 0x0400C0E4 RID: 49380
				public static LocString UNDERWAY_TOOLTIP = "There's a party going on";

				// Token: 0x0400C0E5 RID: 49381
				public static LocString ACCEPT_OPTION_NAME = "Allow the party to happen";

				// Token: 0x0400C0E6 RID: 49382
				public static LocString ACCEPT_OPTION_DESC = "Party goers will get {goodEffect}";

				// Token: 0x0400C0E7 RID: 49383
				public static LocString ACCEPT_OPTION_INVALID_TOOLTIP = "A cake must be built for this event to take place.";

				// Token: 0x0400C0E8 RID: 49384
				public static LocString REJECT_OPTION_NAME = "Cancel the party";

				// Token: 0x0400C0E9 RID: 49385
				public static LocString REJECT_OPTION_DESC = "{host} and {dupe} gain {badEffect}";
			}

			// Token: 0x02002C40 RID: 11328
			public class ECLIPSE
			{
				// Token: 0x0400C0EA RID: 49386
				public static LocString NAME = "Eclipse";

				// Token: 0x0400C0EB RID: 49387
				public static LocString DESCRIPTION = "A celestial object has obscured the sunlight";
			}

			// Token: 0x02002C41 RID: 11329
			public class SOLAR_FLARE
			{
				// Token: 0x0400C0EC RID: 49388
				public static LocString NAME = "Solar Storm";

				// Token: 0x0400C0ED RID: 49389
				public static LocString DESCRIPTION = "A solar flare is headed this way";
			}

			// Token: 0x02002C42 RID: 11330
			public class CREATURE_SPAWN
			{
				// Token: 0x0400C0EE RID: 49390
				public static LocString NAME = "Critter Infestation";

				// Token: 0x0400C0EF RID: 49391
				public static LocString DESCRIPTION = "There was a massive influx of destructive critters";
			}

			// Token: 0x02002C43 RID: 11331
			public class SATELLITE_CRASH
			{
				// Token: 0x0400C0F0 RID: 49392
				public static LocString NAME = "Satellite Crash";

				// Token: 0x0400C0F1 RID: 49393
				public static LocString DESCRIPTION = "Mysterious space junk has crashed into the surface.\n\nIt may contain useful resources or information, but it may also be dangerous. Approach with caution.";
			}

			// Token: 0x02002C44 RID: 11332
			public class FOOD_FIGHT
			{
				// Token: 0x0400C0F2 RID: 49394
				public static LocString NAME = "Food Fight";

				// Token: 0x0400C0F3 RID: 49395
				public static LocString DESCRIPTION = "Duplicants will throw food at each other for recreation\n\nIt may be wasteful, but everyone who participates will benefit from a major stress reduction.";

				// Token: 0x0400C0F4 RID: 49396
				public static LocString UNDERWAY = "Food Fight";

				// Token: 0x0400C0F5 RID: 49397
				public static LocString UNDERWAY_TOOLTIP = "There is a food fight happening now";

				// Token: 0x0400C0F6 RID: 49398
				public static LocString ACCEPT_OPTION_NAME = "Duplicants start preparing to fight.";

				// Token: 0x0400C0F7 RID: 49399
				public static LocString ACCEPT_OPTION_DETAILS = "(Plus morale)";

				// Token: 0x0400C0F8 RID: 49400
				public static LocString REJECT_OPTION_NAME = "No food fight today";

				// Token: 0x0400C0F9 RID: 49401
				public static LocString REJECT_OPTION_DETAILS = "Sadface";
			}

			// Token: 0x02002C45 RID: 11333
			public class PLANT_BLIGHT
			{
				// Token: 0x0400C0FA RID: 49402
				public static LocString NAME = "Plant Blight: {plant}";

				// Token: 0x0400C0FB RID: 49403
				public static LocString DESCRIPTION = "Our {plant} crops have been afflicted by a fungal sickness!\n\nI must get the Duplicants to uproot and compost the sick plants to save our farms.";

				// Token: 0x0400C0FC RID: 49404
				public static LocString SUCCESS = "Blight Managed: {plant}";

				// Token: 0x0400C0FD RID: 49405
				public static LocString SUCCESS_TOOLTIP = "All the blighted {plant} plants have been dealt with, halting the infection.";
			}

			// Token: 0x02002C46 RID: 11334
			public class CRYOFRIEND
			{
				// Token: 0x0400C0FE RID: 49406
				public static LocString NAME = "New Event: A Frozen Friend";

				// Token: 0x0400C0FF RID: 49407
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} has made an amazing discovery! A barely working ",
					BUILDINGS.PREFABS.CRYOTANK.NAME,
					" has been uncovered containing a {friend} inside in a frozen state.\n\n{dupe} was successful in thawing {friend} and this encounter has filled both Duplicants with a sense of hope, something they will desperately need to keep their ",
					UI.FormatAsLink("Morale", "MORALE"),
					" up when facing the dangers ahead."
				});

				// Token: 0x0400C100 RID: 49408
				public static LocString BUTTON = "{friend} is thawed!";
			}

			// Token: 0x02002C47 RID: 11335
			public class WARPWORLDREVEAL
			{
				// Token: 0x0400C101 RID: 49409
				public static LocString NAME = "New Event: Personnel Teleporter";

				// Token: 0x0400C102 RID: 49410
				public static LocString DESCRIPTION = "I've discovered a functioning teleportation device with a pre-programmed destination.\n\nIt appears to go to another " + UI.CLUSTERMAP.PLANETOID + ", and I'm fairly certain there's a return device on the other end.\n\nI could send a Duplicant through safely if I desired.";

				// Token: 0x0400C103 RID: 49411
				public static LocString BUTTON = "See Destination";
			}

			// Token: 0x02002C48 RID: 11336
			public class ARTIFACT_REVEAL
			{
				// Token: 0x0400C104 RID: 49412
				public static LocString NAME = "New Event: Artifact Analyzed";

				// Token: 0x0400C105 RID: 49413
				public static LocString DESCRIPTION = "An artifact from a past civilization was analyzed.\n\n{desc}";

				// Token: 0x0400C106 RID: 49414
				public static LocString BUTTON = "Close";
			}
		}

		// Token: 0x02002191 RID: 8593
		public class BONUS
		{
			// Token: 0x02002C49 RID: 11337
			public class BONUSDREAM1
			{
				// Token: 0x0400C107 RID: 49415
				public static LocString NAME = "Good Dream";

				// Token: 0x0400C108 RID: 49416
				public static LocString DESCRIPTION = "I've observed many improvements to {dupe}'s demeanor today. Analysis indicates unusually high amounts of dopamine in their system. There's a good chance this is due to an exceptionally good dream and analysis indicates that current sleeping conditions may have contributed to this occurrence.\n\nFurther improvements to sleeping conditions may have additional positive effects to the " + UI.FormatAsLink("Morale", "MORALE") + " of {dupe} and other Duplicants.";

				// Token: 0x0400C109 RID: 49417
				public static LocString CHAIN_TOOLTIP = "Improving the living conditions of {dupe} will lead to more good dreams.";
			}

			// Token: 0x02002C4A RID: 11338
			public class BONUSDREAM2
			{
				// Token: 0x0400C10A RID: 49418
				public static LocString NAME = "Really Good Dream";

				// Token: 0x0400C10B RID: 49419
				public static LocString DESCRIPTION = "{dupe} had another really good dream and the resulting release of dopamine has made this Duplicant energetic and full of possibilities! This is an encouraging byproduct of improving the living conditions of the colony.\n\nBased on these observations, building a better sleeping area for my Duplicants will have a similar effect on their " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x02002C4B RID: 11339
			public class BONUSDREAM3
			{
				// Token: 0x0400C10C RID: 49420
				public static LocString NAME = "Great Dream";

				// Token: 0x0400C10D RID: 49421
				public static LocString DESCRIPTION = "I have detected a distinct spring in {dupe}'s step today. There is a good chance that this Duplicant had another great dream last night. Such incidents are further indications that working on the care and comfort of the colony is not a waste of time.\n\nI do wonder though: What do Duplicants dream of?";
			}

			// Token: 0x02002C4C RID: 11340
			public class BONUSDREAM4
			{
				// Token: 0x0400C10E RID: 49422
				public static LocString NAME = "Amazing Dream";

				// Token: 0x0400C10F RID: 49423
				public static LocString DESCRIPTION = "{dupe}'s dream last night must have been simply amazing! Their dopamine levels are at an all time high. Based on these results, it can be safely assumed that improving the living conditions of my Duplicants will reduce " + UI.FormatAsLink("Stress", "STRESS") + " and have similar positive effects on their well-being.\n\nObservations such as this are an integral and enjoyable part of science. When I see my Duplicants happy, I can't help but share in some of their joy.";
			}

			// Token: 0x02002C4D RID: 11341
			public class BONUSTOILET1
			{
				// Token: 0x0400C110 RID: 49424
				public static LocString NAME = "Small Comforts";

				// Token: 0x0400C111 RID: 49425
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} recently visited an Outhouse and appears to have appreciated the small comforts based on the marked increase to their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nHigh ",
					UI.FormatAsLink("Morale", "MORALE"),
					" has been linked to a better work ethic and greater enthusiasm for complex jobs, which are essential in building a successful new colony."
				});
			}

			// Token: 0x02002C4E RID: 11342
			public class BONUSTOILET2
			{
				// Token: 0x0400C112 RID: 49426
				public static LocString NAME = "Greater Comforts";

				// Token: 0x0400C113 RID: 49427
				public static LocString DESCRIPTION = "{dupe} used a Lavatory and analysis shows a decided improvement to this Duplicant's " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nAs my colony grows and expands, it's important not to ignore the benefits of giving my Duplicants a pleasant place to relieve themselves.";
			}

			// Token: 0x02002C4F RID: 11343
			public class BONUSTOILET3
			{
				// Token: 0x0400C114 RID: 49428
				public static LocString NAME = "Small Luxury";

				// Token: 0x0400C115 RID: 49429
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} visited a ",
					ROOMS.TYPES.LATRINE.NAME,
					" and experienced luxury unlike they anything this Duplicant had previously experienced as analysis has revealed yet another boost to their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nIt is unclear whether this development is a result of increased hygiene or whether there is something else inherently about working plumbing which would improve ",
					UI.FormatAsLink("Morale", "MORALE"),
					" in this way. Further analysis is needed."
				});
			}

			// Token: 0x02002C50 RID: 11344
			public class BONUSTOILET4
			{
				// Token: 0x0400C116 RID: 49430
				public static LocString NAME = "Greater Luxury";

				// Token: 0x0400C117 RID: 49431
				public static LocString DESCRIPTION = "{dupe} visited a Washroom and the experience has left this Duplicant with significantly improved " + UI.FormatAsLink("Morale", "MORALE") + ". Analysis indicates this improvement should continue for many cycles.\n\nThe relationship of my Duplicants and their surroundings is an interesting aspect of colony life. I should continue to watch future developments in this department closely.";
			}

			// Token: 0x02002C51 RID: 11345
			public class BONUSRESEARCH
			{
				// Token: 0x0400C118 RID: 49432
				public static LocString NAME = "Inspired Learner";

				// Token: 0x0400C119 RID: 49433
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Analysis indicates that the appearance of a ",
					UI.PRE_KEYWORD,
					"Research Station",
					UI.PST_KEYWORD,
					" has inspired {dupe} and heightened their brain activity on a cellular level.\n\nBrain stimulation is important if my Duplicants are going to adapt and innovate in their increasingly harsh environment."
				});
			}

			// Token: 0x02002C52 RID: 11346
			public class BONUSDIGGING1
			{
				// Token: 0x0400C11A RID: 49434
				public static LocString NAME = "Hot Diggity!";

				// Token: 0x0400C11B RID: 49435
				public static LocString DESCRIPTION = "Some interesting data has revealed that {dupe} has had a marked increase in physical abilities, an increase that cannot entirely be attributed to the usual improvements that occur after regular physical activity.\n\nBased on previous observations this Duplicant's positive associations with digging appear to account for this additional physical boost.\n\nThis would mean the personal preferences of my Duplicants are directly correlated to how hard they work. How interesting...";
			}

			// Token: 0x02002C53 RID: 11347
			public class BONUSSTORAGE
			{
				// Token: 0x0400C11C RID: 49436
				public static LocString NAME = "Something in Store";

				// Token: 0x0400C11D RID: 49437
				public static LocString DESCRIPTION = "Data indicates that {dupe}'s activity in storing something in a Storage Bin has led to an increase in this Duplicant's physical strength as well as an overall improvement to their general demeanor.\n\nThere have been many studies connecting organization with an increase in well-being. It is possible this explains {dupe}'s " + UI.FormatAsLink("Morale", "MORALE") + " improvements.";
			}

			// Token: 0x02002C54 RID: 11348
			public class BONUSBUILDER
			{
				// Token: 0x0400C11E RID: 49438
				public static LocString NAME = "Accomplished Builder";

				// Token: 0x0400C11F RID: 49439
				public static LocString DESCRIPTION = "{dupe} has been hard at work building many structures crucial to the future of the colony. It seems this activity has improved this Duplicant's budding construction and mechanical skills beyond what my models predicted.\n\nWhether this increase in ability is due to them learning new skills or simply gaining self-confidence I cannot say, but this unexpected development is a welcome surprise development.";
			}

			// Token: 0x02002C55 RID: 11349
			public class BONUSOXYGEN
			{
				// Token: 0x0400C120 RID: 49440
				public static LocString NAME = "Fresh Air";

				// Token: 0x0400C121 RID: 49441
				public static LocString DESCRIPTION = "{dupe} is experiencing a sudden unexpected improvement to their physical prowess which appears to be a result of exposure to elevated levels of oxygen from passing by an Oxygen Diffuser.\n\nObservations such as this are important in documenting just how beneficial having access to oxygen is to my colony.";
			}

			// Token: 0x02002C56 RID: 11350
			public class BONUSALGAE
			{
				// Token: 0x0400C122 RID: 49442
				public static LocString NAME = "Fresh Algae Smell";

				// Token: 0x0400C123 RID: 49443
				public static LocString DESCRIPTION = "{dupe}'s recent proximity to an Algae Terrarium has left them feeling refreshed and exuberant and is correlated to an increase in their physical attributes. It is unclear whether these physical improvements came from the excess of oxygen or the invigorating smell of algae.\n\nIt's curious that I find myself nostalgic for the smell of algae growing in a lab. But how could this be...?";
			}

			// Token: 0x02002C57 RID: 11351
			public class BONUSGENERATOR
			{
				// Token: 0x0400C124 RID: 49444
				public static LocString NAME = "Exercised";

				// Token: 0x0400C125 RID: 49445
				public static LocString DESCRIPTION = "{dupe} ran in a Manual Generator and the physical activity appears to have given this Duplicant increased strength and sense of well-being.\n\nWhile not the primary reason for building Manual Generators, I am very pleased to see my Duplicants reaping the " + UI.FormatAsLink("Stress", "STRESS") + " relieving benefits to physical activity.";
			}

			// Token: 0x02002C58 RID: 11352
			public class BONUSDOOR
			{
				// Token: 0x0400C126 RID: 49446
				public static LocString NAME = "Open and Shut";

				// Token: 0x0400C127 RID: 49447
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"The act of closing a door has apparently lead to a decrease in the ",
					UI.FormatAsLink("Stress", "STRESS"),
					" levels of {dupe}, as well as decreased the exposure of this Duplicant to harmful ",
					UI.FormatAsLink("Germs", "GERMS"),
					".\n\nWhile it may be more efficient to group all my Duplicants together in common sleeping quarters, it's important to remember the mental benefits to privacy and space to express their individuality."
				});
			}

			// Token: 0x02002C59 RID: 11353
			public class BONUSHITTHEBOOKS
			{
				// Token: 0x0400C128 RID: 49448
				public static LocString NAME = "Hit the Books";

				// Token: 0x0400C129 RID: 49449
				public static LocString DESCRIPTION = "{dupe}'s recent Research errand has resulted in a significant increase to this Duplicant's brain activity. The discovery of newly found knowledge has given {dupe} an invigorating jolt of excitement.\n\nI am all too familiar with this feeling.";
			}

			// Token: 0x02002C5A RID: 11354
			public class BONUSLITWORKSPACE
			{
				// Token: 0x0400C12A RID: 49450
				public static LocString NAME = "Lit-erally Great";

				// Token: 0x0400C12B RID: 49451
				public static LocString DESCRIPTION = "{dupe}'s recent time in a well-lit area has greatly improved this Duplicant's ability to work with, and on, machinery.\n\nThis supports the prevailing theory that a well-lit workspace has many benefits beyond just improving my Duplicant's ability to see.";
			}

			// Token: 0x02002C5B RID: 11355
			public class BONUSTALKER
			{
				// Token: 0x0400C12C RID: 49452
				public static LocString NAME = "Big Small Talker";

				// Token: 0x0400C12D RID: 49453
				public static LocString DESCRIPTION = "{dupe}'s recent conversation with another Duplicant shows a correlation to improved serotonin and " + UI.FormatAsLink("Morale", "MORALE") + " levels in this Duplicant. It is very possible that small talk with a co-worker, however short and seemingly insignificant, will make my Duplicant's feel connected to the colony as a whole.\n\nAs the colony gets bigger and more sophisticated, I must ensure that the opportunity for such connections continue, for the good of my Duplicants' mental well being.";
			}
		}
	}
}
