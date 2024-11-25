using System;
using TUNING;

namespace STRINGS
{
	// Token: 0x02000F13 RID: 3859
	public class ROOMS
	{
		// Token: 0x02002192 RID: 8594
		public class CATEGORY
		{
			// Token: 0x02002C5C RID: 11356
			public class NONE
			{
				// Token: 0x0400C12E RID: 49454
				public static LocString NAME = "None";
			}

			// Token: 0x02002C5D RID: 11357
			public class FOOD
			{
				// Token: 0x0400C12F RID: 49455
				public static LocString NAME = "Dining";
			}

			// Token: 0x02002C5E RID: 11358
			public class SLEEP
			{
				// Token: 0x0400C130 RID: 49456
				public static LocString NAME = "Sleep";
			}

			// Token: 0x02002C5F RID: 11359
			public class RECREATION
			{
				// Token: 0x0400C131 RID: 49457
				public static LocString NAME = "Recreation";
			}

			// Token: 0x02002C60 RID: 11360
			public class BATHROOM
			{
				// Token: 0x0400C132 RID: 49458
				public static LocString NAME = "Washroom";
			}

			// Token: 0x02002C61 RID: 11361
			public class BIONIC
			{
				// Token: 0x0400C133 RID: 49459
				public static LocString NAME = "Workshop";
			}

			// Token: 0x02002C62 RID: 11362
			public class HOSPITAL
			{
				// Token: 0x0400C134 RID: 49460
				public static LocString NAME = "Medical";
			}

			// Token: 0x02002C63 RID: 11363
			public class INDUSTRIAL
			{
				// Token: 0x0400C135 RID: 49461
				public static LocString NAME = "Industrial";
			}

			// Token: 0x02002C64 RID: 11364
			public class AGRICULTURAL
			{
				// Token: 0x0400C136 RID: 49462
				public static LocString NAME = "Agriculture";
			}

			// Token: 0x02002C65 RID: 11365
			public class PARK
			{
				// Token: 0x0400C137 RID: 49463
				public static LocString NAME = "Parks";
			}

			// Token: 0x02002C66 RID: 11366
			public class SCIENCE
			{
				// Token: 0x0400C138 RID: 49464
				public static LocString NAME = "Science";
			}
		}

		// Token: 0x02002193 RID: 8595
		public class TYPES
		{
			// Token: 0x040098D0 RID: 39120
			public static LocString CONFLICTED = "Conflicted Room";

			// Token: 0x02002C67 RID: 11367
			public class NEUTRAL
			{
				// Token: 0x0400C139 RID: 49465
				public static LocString NAME = "Miscellaneous Room";

				// Token: 0x0400C13A RID: 49466
				public static LocString DESCRIPTION = "An enclosed space with plenty of potential and no dedicated use.";

				// Token: 0x0400C13B RID: 49467
				public static LocString EFFECT = "- No effect";

				// Token: 0x0400C13C RID: 49468
				public static LocString TOOLTIP = "This area has walls and doors but no dedicated use";
			}

			// Token: 0x02002C68 RID: 11368
			public class LATRINE
			{
				// Token: 0x0400C13D RID: 49469
				public static LocString NAME = "Latrine";

				// Token: 0x0400C13E RID: 49470
				public static LocString DESCRIPTION = "It's a step up from doing one's business in full view of the rest of the colony.\n\nUsing a toilet in an enclosed room will improve Duplicants' Morale.";

				// Token: 0x0400C13F RID: 49471
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C140 RID: 49472
				public static LocString TOOLTIP = "Using a toilet in an enclosed room will improve Duplicants' Morale";
			}

			// Token: 0x02002C69 RID: 11369
			public class BIONICUPKEEP
			{
				// Token: 0x0400C141 RID: 49473
				public static LocString NAME = "Workshop";

				// Token: 0x0400C142 RID: 49474
				public static LocString DESCRIPTION = "Where Bionic Duplicants can get the specialized care they need.\n\nUsing a " + BUILDINGS.PREFABS.GUNKEMPTIER.NAME + " in a Workshop will improve Bionic Duplicants' Morale.";

				// Token: 0x0400C143 RID: 49475
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C144 RID: 49476
				public static LocString TOOLTIP = "Using a gunk extractor in a Workshop will improve Bionic Duplicants' Morale";
			}

			// Token: 0x02002C6A RID: 11370
			public class PLUMBEDBATHROOM
			{
				// Token: 0x0400C145 RID: 49477
				public static LocString NAME = "Washroom";

				// Token: 0x0400C146 RID: 49478
				public static LocString DESCRIPTION = "A sanctuary of personal hygiene.\n\nUsing a fully plumbed Washroom will improve Duplicants' Morale.";

				// Token: 0x0400C147 RID: 49479
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C148 RID: 49480
				public static LocString TOOLTIP = "Using a fully plumbed Washroom will improve Duplicants' Morale";
			}

			// Token: 0x02002C6B RID: 11371
			public class BARRACKS
			{
				// Token: 0x0400C149 RID: 49481
				public static LocString NAME = "Barracks";

				// Token: 0x0400C14A RID: 49482
				public static LocString DESCRIPTION = "A basic communal sleeping area for up-and-coming colonies.\n\nSleeping in Barracks will improve Duplicants' Morale.";

				// Token: 0x0400C14B RID: 49483
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C14C RID: 49484
				public static LocString TOOLTIP = "Sleeping in Barracks will improve Duplicants' Morale";
			}

			// Token: 0x02002C6C RID: 11372
			public class BEDROOM
			{
				// Token: 0x0400C14D RID: 49485
				public static LocString NAME = "Luxury Barracks";

				// Token: 0x0400C14E RID: 49486
				public static LocString DESCRIPTION = "An upscale communal sleeping area full of things that greatly enhance quality of rest for occupants.\n\nSleeping in a Luxury Barracks will improve Duplicants' Morale.";

				// Token: 0x0400C14F RID: 49487
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C150 RID: 49488
				public static LocString TOOLTIP = "Sleeping in a Luxury Barracks will improve Duplicants' Morale";
			}

			// Token: 0x02002C6D RID: 11373
			public class PRIVATE_BEDROOM
			{
				// Token: 0x0400C151 RID: 49489
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400C152 RID: 49490
				public static LocString DESCRIPTION = "A comfortable, roommate-free retreat where tired Duplicants can get uninterrupted rest.\n\nSleeping in a Private Bedroom will greatly improve Duplicants' Morale.";

				// Token: 0x0400C153 RID: 49491
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C154 RID: 49492
				public static LocString TOOLTIP = "Sleeping in a Private Bedroom will greatly improve Duplicants' Morale";
			}

			// Token: 0x02002C6E RID: 11374
			public class MESSHALL
			{
				// Token: 0x0400C155 RID: 49493
				public static LocString NAME = "Mess Hall";

				// Token: 0x0400C156 RID: 49494
				public static LocString DESCRIPTION = "A simple dining room setup that's easy to improve upon.\n\nEating at a mess table in a Mess Hall will increase Duplicants' Morale.";

				// Token: 0x0400C157 RID: 49495
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C158 RID: 49496
				public static LocString TOOLTIP = "Eating at a Mess Table in a Mess Hall will improve Duplicants' Morale";
			}

			// Token: 0x02002C6F RID: 11375
			public class KITCHEN
			{
				// Token: 0x0400C159 RID: 49497
				public static LocString NAME = "Kitchen";

				// Token: 0x0400C15A RID: 49498
				public static LocString DESCRIPTION = "A cooking area equipped to take meals to the next level.\n\nAdding ingredients from a Spice Grinder to foods cooked on an Electric Grill or Gas Range provides a variety of positive benefits.";

				// Token: 0x0400C15B RID: 49499
				public static LocString EFFECT = "- Enables Spice Grinder use";

				// Token: 0x0400C15C RID: 49500
				public static LocString TOOLTIP = "Using a Spice Grinder in a Kitchen adds benefits to foods cooked on Electric Grill or Gas Range";
			}

			// Token: 0x02002C70 RID: 11376
			public class GREATHALL
			{
				// Token: 0x0400C15D RID: 49501
				public static LocString NAME = "Great Hall";

				// Token: 0x0400C15E RID: 49502
				public static LocString DESCRIPTION = "A great place to eat, with great decor and great company. Great!\n\nEating in a Great Hall will significantly improve Duplicants' Morale.";

				// Token: 0x0400C15F RID: 49503
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C160 RID: 49504
				public static LocString TOOLTIP = "Eating in a Great Hall will significantly improve Duplicants' Morale";
			}

			// Token: 0x02002C71 RID: 11377
			public class HOSPITAL
			{
				// Token: 0x0400C161 RID: 49505
				public static LocString NAME = "Hospital";

				// Token: 0x0400C162 RID: 49506
				public static LocString DESCRIPTION = "A dedicated medical facility that helps minimize recovery time.\n\nSick Duplicants assigned to medical buildings located within a Hospital are also less likely to spread Disease.";

				// Token: 0x0400C163 RID: 49507
				public static LocString EFFECT = "- Quarantine sick Duplicants";

				// Token: 0x0400C164 RID: 49508
				public static LocString TOOLTIP = "Sick Duplicants assigned to medical buildings located within a Hospital are less likely to spread Disease";
			}

			// Token: 0x02002C72 RID: 11378
			public class MASSAGE_CLINIC
			{
				// Token: 0x0400C165 RID: 49509
				public static LocString NAME = "Massage Clinic";

				// Token: 0x0400C166 RID: 49510
				public static LocString DESCRIPTION = "A soothing space with a very relaxing ambience, especially when well-decorated.\n\nReceiving massages at a Massage Clinic will significantly improve Stress reduction.";

				// Token: 0x0400C167 RID: 49511
				public static LocString EFFECT = "- Massage stress relief bonus";

				// Token: 0x0400C168 RID: 49512
				public static LocString TOOLTIP = "Receiving massages at a Massage Clinic will significantly improve Stress reduction";
			}

			// Token: 0x02002C73 RID: 11379
			public class POWER_PLANT
			{
				// Token: 0x0400C169 RID: 49513
				public static LocString NAME = "Power Plant";

				// Token: 0x0400C16A RID: 49514
				public static LocString DESCRIPTION = "The perfect place for Duplicants to flex their Electrical Engineering skills.\n\nHeavy-duty generators built within a Power Plant can be tuned up using microchips from power control stations to improve their " + UI.FormatAsLink("Power", "POWER") + " power production.";

				// Token: 0x0400C16B RID: 49515
				public static LocString EFFECT = "- Enables " + ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME + " tune-ups on heavy-duty generators";

				// Token: 0x0400C16C RID: 49516
				public static LocString TOOLTIP = "Heavy-duty generators built in a Power Plant can be tuned up using microchips from Power Control Stations to improve their Power production";
			}

			// Token: 0x02002C74 RID: 11380
			public class MACHINE_SHOP
			{
				// Token: 0x0400C16D RID: 49517
				public static LocString NAME = "Machine Shop";

				// Token: 0x0400C16E RID: 49518
				public static LocString DESCRIPTION = "It smells like elbow grease.\n\nDuplicants working in a Machine Shop can maintain buildings and increase their production speed.";

				// Token: 0x0400C16F RID: 49519
				public static LocString EFFECT = "- Increased fabrication efficiency";

				// Token: 0x0400C170 RID: 49520
				public static LocString TOOLTIP = "Duplicants working in a Machine Shop can maintain buildings and increase their production speed";
			}

			// Token: 0x02002C75 RID: 11381
			public class FARM
			{
				// Token: 0x0400C171 RID: 49521
				public static LocString NAME = "Greenhouse";

				// Token: 0x0400C172 RID: 49522
				public static LocString DESCRIPTION = "An enclosed agricultural space best utilized by Duplicants with Crop Tending skills.\n\nCrops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed.";

				// Token: 0x0400C173 RID: 49523
				public static LocString EFFECT = "- Enables Farm Station use";

				// Token: 0x0400C174 RID: 49524
				public static LocString TOOLTIP = "Crops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed";
			}

			// Token: 0x02002C76 RID: 11382
			public class CREATUREPEN
			{
				// Token: 0x0400C175 RID: 49525
				public static LocString NAME = "Stable";

				// Token: 0x0400C176 RID: 49526
				public static LocString DESCRIPTION = "Critters don't mind it here, as long as things don't get too crowded.\n\nStabled critters can be tended to in order to improve their happiness, hasten their domestication and increase their production.\n\nEnables the use of Grooming Stations, Shearing Stations, Critter Condos, Critter Fountains and Milking Stations.";

				// Token: 0x0400C177 RID: 49527
				public static LocString EFFECT = "- Critter taming and mood bonus";

				// Token: 0x0400C178 RID: 49528
				public static LocString TOOLTIP = "A stable enables Grooming Station, Critter Condo, Critter Fountain, Shearing Station and Milking Station use";
			}

			// Token: 0x02002C77 RID: 11383
			public class REC_ROOM
			{
				// Token: 0x0400C179 RID: 49529
				public static LocString NAME = "Recreation Room";

				// Token: 0x0400C17A RID: 49530
				public static LocString DESCRIPTION = "Where Duplicants go to mingle with off-duty peers and indulge in a little R&R.\n\nScheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room.";

				// Token: 0x0400C17B RID: 49531
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C17C RID: 49532
				public static LocString TOOLTIP = "Scheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room";
			}

			// Token: 0x02002C78 RID: 11384
			public class PARK
			{
				// Token: 0x0400C17D RID: 49533
				public static LocString NAME = "Park";

				// Token: 0x0400C17E RID: 49534
				public static LocString DESCRIPTION = "A little greenery goes a long way.\n\nPassing through natural spaces throughout the day will raise the Morale of Duplicants.";

				// Token: 0x0400C17F RID: 49535
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C180 RID: 49536
				public static LocString TOOLTIP = "Passing through natural spaces throughout the day will raise the Morale of Duplicants";
			}

			// Token: 0x02002C79 RID: 11385
			public class NATURERESERVE
			{
				// Token: 0x0400C181 RID: 49537
				public static LocString NAME = "Nature Reserve";

				// Token: 0x0400C182 RID: 49538
				public static LocString DESCRIPTION = "A lot of greenery goes an even longer way.\n\nPassing through a Nature Reserve will grant higher Morale bonuses to Duplicants than a Park.";

				// Token: 0x0400C183 RID: 49539
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C184 RID: 49540
				public static LocString TOOLTIP = "A Nature Reserve will grant higher Morale bonuses to Duplicants than a Park";
			}

			// Token: 0x02002C7A RID: 11386
			public class LABORATORY
			{
				// Token: 0x0400C185 RID: 49541
				public static LocString NAME = "Laboratory";

				// Token: 0x0400C186 RID: 49542
				public static LocString DESCRIPTION = "Where wild hypotheses meet rigorous scientific experimentation.\n\nScience stations built in a Laboratory function more efficiently.\n\nA Laboratory enables the use of the Geotuner and the Mission Control Station.";

				// Token: 0x0400C187 RID: 49543
				public static LocString EFFECT = "- Efficiency bonus";

				// Token: 0x0400C188 RID: 49544
				public static LocString TOOLTIP = "Science buildings built in a Laboratory function more efficiently\n\nA Laboratory enables Geotuner and Mission Control Station use";
			}

			// Token: 0x02002C7B RID: 11387
			public class PRIVATE_BATHROOM
			{
				// Token: 0x0400C189 RID: 49545
				public static LocString NAME = "Private Bathroom";

				// Token: 0x0400C18A RID: 49546
				public static LocString DESCRIPTION = "Finally, a place to truly be alone with one's thoughts.\n\nDuplicants relieve even more Stress when using the toilet in a Private Bathroom than in a Latrine.";

				// Token: 0x0400C18B RID: 49547
				public static LocString EFFECT = "- Stress relief bonus";

				// Token: 0x0400C18C RID: 49548
				public static LocString TOOLTIP = "Duplicants relieve even more stress when using the toilet in a Private Bathroom than in a Latrine";
			}

			// Token: 0x02002C7C RID: 11388
			public class BIONIC_UPKEEP
			{
				// Token: 0x0400C18D RID: 49549
				public static LocString NAME = "Workshop";

				// Token: 0x0400C18E RID: 49550
				public static LocString DESCRIPTION = "A spa of sorts, for Duplicants who were built different.\n\nBionic Duplicants who access bionic service stations in a Workshop will get a nice little Morale boost.";

				// Token: 0x0400C18F RID: 49551
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C190 RID: 49552
				public static LocString TOOLTIP = "Bionic Duplicants get a Morale boost when using bionic service stations in a Workshop";
			}
		}

		// Token: 0x02002194 RID: 8596
		public class CRITERIA
		{
			// Token: 0x040098D1 RID: 39121
			public static LocString HEADER = "<b>Requirements:</b>";

			// Token: 0x040098D2 RID: 39122
			public static LocString NEUTRAL_TYPE = "Enclosed by wall tile";

			// Token: 0x040098D3 RID: 39123
			public static LocString POSSIBLE_TYPES_HEADER = "Possible Room Types";

			// Token: 0x040098D4 RID: 39124
			public static LocString NO_TYPE_CONFLICTS = "Remove conflicting buildings";

			// Token: 0x040098D5 RID: 39125
			public static LocString IN_CODE_ERROR = "String Key Not Found: {0}";

			// Token: 0x02002C7D RID: 11389
			public class CRITERIA_FAILED
			{
				// Token: 0x0400C191 RID: 49553
				public static LocString MISSING_BUILDING = "Missing {0}";

				// Token: 0x0400C192 RID: 49554
				public static LocString FAILED = "{0}";
			}

			// Token: 0x02002C7E RID: 11390
			public static class DECORATION
			{
				// Token: 0x0400C193 RID: 49555
				public static LocString NAME = UI.FormatAsLink("Decor item", "BUILDCATEGORYREQUIREMENTCLASSDECORATION");

				// Token: 0x0400C194 RID: 49556
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DECORATION.NAME;
			}

			// Token: 0x02002C7F RID: 11391
			public class CEILING_HEIGHT
			{
				// Token: 0x0400C195 RID: 49557
				public static LocString NAME = "Minimum height: {0} tiles";

				// Token: 0x0400C196 RID: 49558
				public static LocString DESCRIPTION = "Must have a ceiling height of at least {0} tiles";

				// Token: 0x0400C197 RID: 49559
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CEILING_HEIGHT.NAME;
			}

			// Token: 0x02002C80 RID: 11392
			public class MINIMUM_SIZE
			{
				// Token: 0x0400C198 RID: 49560
				public static LocString NAME = "Minimum size: {0} tiles";

				// Token: 0x0400C199 RID: 49561
				public static LocString DESCRIPTION = "Must have an area of at least {0} tiles";

				// Token: 0x0400C19A RID: 49562
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MINIMUM_SIZE.NAME;
			}

			// Token: 0x02002C81 RID: 11393
			public class MAXIMUM_SIZE
			{
				// Token: 0x0400C19B RID: 49563
				public static LocString NAME = "Maximum size: {0} tiles";

				// Token: 0x0400C19C RID: 49564
				public static LocString DESCRIPTION = "Must have an area no larger than {0} tiles";

				// Token: 0x0400C19D RID: 49565
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MAXIMUM_SIZE.NAME;
			}

			// Token: 0x02002C82 RID: 11394
			public class INDUSTRIALMACHINERY
			{
				// Token: 0x0400C19E RID: 49566
				public static LocString NAME = UI.FormatAsLink("Industrial machinery", "BUILDCATEGORYREQUIREMENTCLASSINDUSTRIALMACHINERY");

				// Token: 0x0400C19F RID: 49567
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.INDUSTRIALMACHINERY.NAME;
			}

			// Token: 0x02002C83 RID: 11395
			public class HAS_BED
			{
				// Token: 0x0400C1A0 RID: 49568
				public static LocString NAME = "One or more " + UI.FormatAsLink("beds", "BUILDCATEGORYREQUIREMENTCLASSBEDTYPE");

				// Token: 0x0400C1A1 RID: 49569
				public static LocString DESCRIPTION = "Requires at least one Cot or Comfy Bed";

				// Token: 0x0400C1A2 RID: 49570
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.HAS_BED.NAME;
			}

			// Token: 0x02002C84 RID: 11396
			public class HAS_LUXURY_BED
			{
				// Token: 0x0400C1A3 RID: 49571
				public static LocString NAME = "One or more " + UI.FormatAsLink("Comfy Beds", "LUXURYBED");

				// Token: 0x0400C1A4 RID: 49572
				public static LocString DESCRIPTION = "Requires at least one Comfy Bed";

				// Token: 0x0400C1A5 RID: 49573
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.HAS_LUXURY_BED.NAME;
			}

			// Token: 0x02002C85 RID: 11397
			public class LUXURYBEDTYPE
			{
				// Token: 0x0400C1A6 RID: 49574
				public static LocString NAME = "Single " + UI.FormatAsLink("Comfy Bed", "LUXURYBED");

				// Token: 0x0400C1A7 RID: 49575
				public static LocString DESCRIPTION = "Must have no more than one Comfy Bed";

				// Token: 0x0400C1A8 RID: 49576
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.LUXURYBEDTYPE.NAME;
			}

			// Token: 0x02002C86 RID: 11398
			public class BED_SINGLE
			{
				// Token: 0x0400C1A9 RID: 49577
				public static LocString NAME = "Single " + UI.FormatAsLink("beds", "BUILDCATEGORYREQUIREMENTCLASSBEDTYPE");

				// Token: 0x0400C1AA RID: 49578
				public static LocString DESCRIPTION = "Must have no more than one Cot or Comfy Bed";

				// Token: 0x0400C1AB RID: 49579
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BED_SINGLE.NAME;
			}

			// Token: 0x02002C87 RID: 11399
			public class IS_BACKWALLED
			{
				// Token: 0x0400C1AC RID: 49580
				public static LocString NAME = "Has backwall tiles";

				// Token: 0x0400C1AD RID: 49581
				public static LocString DESCRIPTION = "Must be covered in backwall tiles";

				// Token: 0x0400C1AE RID: 49582
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.IS_BACKWALLED.NAME;
			}

			// Token: 0x02002C88 RID: 11400
			public class NO_COTS
			{
				// Token: 0x0400C1AF RID: 49583
				public static LocString NAME = "No " + UI.FormatAsLink("Cots", "BED");

				// Token: 0x0400C1B0 RID: 49584
				public static LocString DESCRIPTION = "Room cannot contain a Cot";

				// Token: 0x0400C1B1 RID: 49585
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_COTS.NAME;
			}

			// Token: 0x02002C89 RID: 11401
			public class NO_LUXURY_BEDS
			{
				// Token: 0x0400C1B2 RID: 49586
				public static LocString NAME = "No " + UI.FormatAsLink("Comfy Beds", "LUXURYBED");

				// Token: 0x0400C1B3 RID: 49587
				public static LocString DESCRIPTION = "Room cannot contain a Comfy Bed";

				// Token: 0x0400C1B4 RID: 49588
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_LUXURY_BEDS.NAME;
			}

			// Token: 0x02002C8A RID: 11402
			public class BEDTYPE
			{
				// Token: 0x0400C1B5 RID: 49589
				public static LocString NAME = UI.FormatAsLink("Beds", "BUILDCATEGORYREQUIREMENTCLASSBEDTYPE");

				// Token: 0x0400C1B6 RID: 49590
				public static LocString DESCRIPTION = "Requires two or more Cots or Comfy Beds";

				// Token: 0x0400C1B7 RID: 49591
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BEDTYPE.NAME;
			}

			// Token: 0x02002C8B RID: 11403
			public class BUILDING_DECOR_POSITIVE
			{
				// Token: 0x0400C1B8 RID: 49592
				public static LocString NAME = "Positive " + UI.FormatAsLink("decor", "BUILDCATEGORYREQUIREMENTCLASSDECORATION");

				// Token: 0x0400C1B9 RID: 49593
				public static LocString DESCRIPTION = "Requires at least one building with positive decor";

				// Token: 0x0400C1BA RID: 49594
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.NAME;
			}

			// Token: 0x02002C8C RID: 11404
			public class DECORATIVE_ITEM
			{
				// Token: 0x0400C1BB RID: 49595
				public static LocString NAME = UI.FormatAsLink("Decor item", "BUILDCATEGORYREQUIREMENTCLASSDECORATION") + " ({0})";

				// Token: 0x0400C1BC RID: 49596
				public static LocString DESCRIPTION = "Requires {0} or more Decor items";

				// Token: 0x0400C1BD RID: 49597
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DECORATIVE_ITEM.NAME;
			}

			// Token: 0x02002C8D RID: 11405
			public class DECOR20
			{
				// Token: 0x0600CDE8 RID: 52712 RVA: 0x0040831C File Offset: 0x0040651C
				// Note: this type is marked as 'beforefieldinit'.
				static DECOR20()
				{
					string str = "Requires a decorative item with a minimum Decor value of ";
					int amount = BUILDINGS.DECOR.BONUS.TIER3.amount;
					ROOMS.CRITERIA.DECOR20.DESCRIPTION = str + amount.ToString();
					ROOMS.CRITERIA.DECOR20.CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DECOR20.NAME;
				}

				// Token: 0x0400C1BE RID: 49598
				public static LocString NAME = UI.FormatAsLink("Fancy decor item", "BUILDCATEGORYREQUIREMENTCLASSDECORATION");

				// Token: 0x0400C1BF RID: 49599
				public static LocString DESCRIPTION;

				// Token: 0x0400C1C0 RID: 49600
				public static LocString CONFLICT_DESCRIPTION;
			}

			// Token: 0x02002C8E RID: 11406
			public class CLINIC
			{
				// Token: 0x0400C1C1 RID: 49601
				public static LocString NAME = UI.FormatAsLink("Medical equipment", "BUILDCATEGORYREQUIREMENTCLASSCLINIC");

				// Token: 0x0400C1C2 RID: 49602
				public static LocString DESCRIPTION = "Requires one or more Sick Bays or Disease Clinics";

				// Token: 0x0400C1C3 RID: 49603
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CLINIC.NAME;
			}

			// Token: 0x02002C8F RID: 11407
			public class POWERPLANT
			{
				// Token: 0x0400C1C4 RID: 49604
				public static LocString NAME = UI.FormatAsLink("Heavy-Duty Generator", "BUILDCATEGORYREQUIREMENTCLASSGENERATORTYPE") + "\n    • Two or more " + UI.FormatAsLink("Power Buildings", "BUILDCATEGORYREQUIREMENTCLASSPOWERBUILDING");

				// Token: 0x0400C1C5 RID: 49605
				public static LocString DESCRIPTION = "Requires a Heavy-Duty Generator and two or more Power Buildings";

				// Token: 0x0400C1C6 RID: 49606
				public static LocString CONFLICT_DESCRIPTION = "Heavy-Duty Generator and two or more Power buildings";
			}

			// Token: 0x02002C90 RID: 11408
			public class FARMSTATIONTYPE
			{
				// Token: 0x0400C1C7 RID: 49607
				public static LocString NAME = UI.FormatAsLink("Farm Station", "FARMSTATION");

				// Token: 0x0400C1C8 RID: 49608
				public static LocString DESCRIPTION = "Requires a single Farm Station";

				// Token: 0x0400C1C9 RID: 49609
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FARMSTATIONTYPE.NAME;
			}

			// Token: 0x02002C91 RID: 11409
			public class CREATURERELOCATOR
			{
				// Token: 0x0400C1CA RID: 49610
				public static LocString NAME = UI.FormatAsLink("Critter relocator", "BUILDCATEGORYREQUIREMENTCLASSCREATURERELOCATOR");

				// Token: 0x0400C1CB RID: 49611
				public static LocString DESCRIPTION = "Requires a single Critter Drop-Off or Fish Release";

				// Token: 0x0400C1CC RID: 49612
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CREATURERELOCATOR.NAME;
			}

			// Token: 0x02002C92 RID: 11410
			public class CREATURE_FEEDER
			{
				// Token: 0x0400C1CD RID: 49613
				public static LocString NAME = UI.FormatAsLink("Critter Feeder", "CREATUREFEEDER");

				// Token: 0x0400C1CE RID: 49614
				public static LocString DESCRIPTION = "Requires a single Critter Feeder";

				// Token: 0x0400C1CF RID: 49615
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CREATURE_FEEDER.NAME;
			}

			// Token: 0x02002C93 RID: 11411
			public class RANCHSTATIONTYPE
			{
				// Token: 0x0400C1D0 RID: 49616
				public static LocString NAME = UI.FormatAsLink("Ranching building", "BUILDCATEGORYREQUIREMENTCLASSRANCHSTATIONTYPE");

				// Token: 0x0400C1D1 RID: 49617
				public static LocString DESCRIPTION = "Requires a single Grooming Station, Critter Condo, Critter Fountain, Shearing Station or Milking Station";

				// Token: 0x0400C1D2 RID: 49618
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.RANCHSTATIONTYPE.NAME;
			}

			// Token: 0x02002C94 RID: 11412
			public class SPICESTATION
			{
				// Token: 0x0400C1D3 RID: 49619
				public static LocString NAME = UI.FormatAsLink("Spice Grinder", "SPICEGRINDER");

				// Token: 0x0400C1D4 RID: 49620
				public static LocString DESCRIPTION = "Requires a single Spice Grinder";

				// Token: 0x0400C1D5 RID: 49621
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.SPICESTATION.NAME;
			}

			// Token: 0x02002C95 RID: 11413
			public class COOKTOP
			{
				// Token: 0x0400C1D6 RID: 49622
				public static LocString NAME = UI.FormatAsLink("Cooking station", "BUILDCATEGORYREQUIREMENTCLASSCOOKTOP");

				// Token: 0x0400C1D7 RID: 49623
				public static LocString DESCRIPTION = "Requires a single Electric Grill or Gas Range";

				// Token: 0x0400C1D8 RID: 49624
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.COOKTOP.NAME;
			}

			// Token: 0x02002C96 RID: 11414
			public class REFRIGERATOR
			{
				// Token: 0x0400C1D9 RID: 49625
				public static LocString NAME = UI.FormatAsLink("Refrigerator", "REFRIGERATOR");

				// Token: 0x0400C1DA RID: 49626
				public static LocString DESCRIPTION = "Requires a single Refrigerator";

				// Token: 0x0400C1DB RID: 49627
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.REFRIGERATOR.NAME;
			}

			// Token: 0x02002C97 RID: 11415
			public class RECBUILDING
			{
				// Token: 0x0400C1DC RID: 49628
				public static LocString NAME = UI.FormatAsLink("Recreational building", "BUILDCATEGORYREQUIREMENTCLASSRECBUILDING");

				// Token: 0x0400C1DD RID: 49629
				public static LocString DESCRIPTION = "Requires one or more recreational buildings";

				// Token: 0x0400C1DE RID: 49630
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.RECBUILDING.NAME;
			}

			// Token: 0x02002C98 RID: 11416
			public class PARK
			{
				// Token: 0x0400C1DF RID: 49631
				public static LocString NAME = UI.FormatAsLink("Park Sign", "PARKSIGN");

				// Token: 0x0400C1E0 RID: 49632
				public static LocString DESCRIPTION = "Requires one or more Park Signs";

				// Token: 0x0400C1E1 RID: 49633
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.PARK.NAME;
			}

			// Token: 0x02002C99 RID: 11417
			public class MACHINESHOPTYPE
			{
				// Token: 0x0400C1E2 RID: 49634
				public static LocString NAME = "Mechanics Station";

				// Token: 0x0400C1E3 RID: 49635
				public static LocString DESCRIPTION = "Requires requires one or more Mechanics Stations";

				// Token: 0x0400C1E4 RID: 49636
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MACHINESHOPTYPE.NAME;
			}

			// Token: 0x02002C9A RID: 11418
			public class FOOD_BOX
			{
				// Token: 0x0400C1E5 RID: 49637
				public static LocString NAME = "Food storage";

				// Token: 0x0400C1E6 RID: 49638
				public static LocString DESCRIPTION = "Requires one or more Ration Boxes or Refrigerators";

				// Token: 0x0400C1E7 RID: 49639
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FOOD_BOX.NAME;
			}

			// Token: 0x02002C9B RID: 11419
			public class LIGHTSOURCE
			{
				// Token: 0x0400C1E8 RID: 49640
				public static LocString NAME = UI.FormatAsLink("Light source", "BUILDCATEGORYREQUIREMENTCLASSLIGHTSOURCE");

				// Token: 0x0400C1E9 RID: 49641
				public static LocString DESCRIPTION = "Requires one or more light sources";

				// Token: 0x0400C1EA RID: 49642
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.LIGHTSOURCE.NAME;
			}

			// Token: 0x02002C9C RID: 11420
			public class DESTRESSINGBUILDING
			{
				// Token: 0x0400C1EB RID: 49643
				public static LocString NAME = UI.FormatAsLink("De-Stressing Building", "MASSAGETABLE");

				// Token: 0x0400C1EC RID: 49644
				public static LocString DESCRIPTION = "Requires one or more De-Stressing buildings";

				// Token: 0x0400C1ED RID: 49645
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DESTRESSINGBUILDING.NAME;
			}

			// Token: 0x02002C9D RID: 11421
			public class MASSAGE_TABLE
			{
				// Token: 0x0400C1EE RID: 49646
				public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

				// Token: 0x0400C1EF RID: 49647
				public static LocString DESCRIPTION = "Requires one or more Massage Tables";

				// Token: 0x0400C1F0 RID: 49648
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MASSAGE_TABLE.NAME;
			}

			// Token: 0x02002C9E RID: 11422
			public class MESSTABLE
			{
				// Token: 0x0400C1F1 RID: 49649
				public static LocString NAME = UI.FormatAsLink("Mess Table", "DININGTABLE");

				// Token: 0x0400C1F2 RID: 49650
				public static LocString DESCRIPTION = "Requires a single Mess Table";

				// Token: 0x0400C1F3 RID: 49651
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MESSTABLE.NAME;
			}

			// Token: 0x02002C9F RID: 11423
			public class NO_MESS_STATION
			{
				// Token: 0x0400C1F4 RID: 49652
				public static LocString NAME = "No " + UI.FormatAsLink("Mess Table", "DININGTABLE");

				// Token: 0x0400C1F5 RID: 49653
				public static LocString DESCRIPTION = "Cannot contain a Mess Table";

				// Token: 0x0400C1F6 RID: 49654
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_MESS_STATION.NAME;
			}

			// Token: 0x02002CA0 RID: 11424
			public class MESS_STATION_MULTIPLE
			{
				// Token: 0x0400C1F7 RID: 49655
				public static LocString NAME = UI.FormatAsLink("Mess Tables", "DININGTABLE");

				// Token: 0x0400C1F8 RID: 49656
				public static LocString DESCRIPTION = "Requires two or more Mess Tables";

				// Token: 0x0400C1F9 RID: 49657
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MESS_STATION_MULTIPLE.NAME;
			}

			// Token: 0x02002CA1 RID: 11425
			public class RESEARCH_STATION
			{
				// Token: 0x0400C1FA RID: 49658
				public static LocString NAME = UI.FormatAsLink("Research station", "BUILDCATEGORYREQUIREMENTCLASSRESEARCH_STATION");

				// Token: 0x0400C1FB RID: 49659
				public static LocString DESCRIPTION = "Requires one or more Research Stations or Super Computers";

				// Token: 0x0400C1FC RID: 49660
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.RESEARCH_STATION.NAME;
			}

			// Token: 0x02002CA2 RID: 11426
			public class BIONICUPKEEP
			{
				// Token: 0x0400C1FD RID: 49661
				public static LocString NAME = UI.FormatAsLink("Bionic service station", "BUILDCATEGORYREQUIREMENTCLASSBIONICUPKEEP");

				// Token: 0x0400C1FE RID: 49662
				public static LocString DESCRIPTION = "Requires at least one Lubrication Station and one Gunk Extractor";

				// Token: 0x0400C1FF RID: 49663
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BIONICUPKEEP.NAME;
			}

			// Token: 0x02002CA3 RID: 11427
			public class BIONIC_GUNKEMPTIER
			{
				// Token: 0x0400C200 RID: 49664
				public static LocString NAME = UI.FormatAsLink("Gunk Extractor", "BUILDCATEGORYREQUIREMENTCLASSBIONIC_GUNKEMPTIER");

				// Token: 0x0400C201 RID: 49665
				public static LocString DESCRIPTION = "Requires one or more Gunk Extractors";

				// Token: 0x0400C202 RID: 49666
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BIONIC_GUNKEMPTIER.NAME;
			}

			// Token: 0x02002CA4 RID: 11428
			public class BIONIC_LUBRICATION
			{
				// Token: 0x0400C203 RID: 49667
				public static LocString NAME = UI.FormatAsLink("Lubrication Station", "BUILDCATEGORYREQUIREMENTCLASSBIONIC_LUBRICATION");

				// Token: 0x0400C204 RID: 49668
				public static LocString DESCRIPTION = "Requires one or more Lubrication Stations";

				// Token: 0x0400C205 RID: 49669
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BIONIC_LUBRICATION.NAME;
			}

			// Token: 0x02002CA5 RID: 11429
			public class TOILETTYPE
			{
				// Token: 0x0400C206 RID: 49670
				public static LocString NAME = UI.FormatAsLink("Toilet", "BUILDCATEGORYREQUIREMENTCLASSTOILETTYPE");

				// Token: 0x0400C207 RID: 49671
				public static LocString DESCRIPTION = "Requires one or more Outhouses or Lavatories";

				// Token: 0x0400C208 RID: 49672
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.TOILETTYPE.NAME;
			}

			// Token: 0x02002CA6 RID: 11430
			public class FLUSHTOILETTYPE
			{
				// Token: 0x0400C209 RID: 49673
				public static LocString NAME = UI.FormatAsLink("Flush Toilet", "BUILDCATEGORYREQUIREMENTCLASSFLUSHTOILETTYPE");

				// Token: 0x0400C20A RID: 49674
				public static LocString DESCRIPTION = "Requires one or more Lavatories";

				// Token: 0x0400C20B RID: 49675
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FLUSHTOILETTYPE.NAME;
			}

			// Token: 0x02002CA7 RID: 11431
			public class NO_OUTHOUSES
			{
				// Token: 0x0400C20C RID: 49676
				public static LocString NAME = "No " + UI.FormatAsLink("Outhouses", "OUTHOUSE");

				// Token: 0x0400C20D RID: 49677
				public static LocString DESCRIPTION = "Cannot contain basic Outhouses";

				// Token: 0x0400C20E RID: 49678
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_OUTHOUSES.NAME;
			}

			// Token: 0x02002CA8 RID: 11432
			public class WASHSTATION
			{
				// Token: 0x0400C20F RID: 49679
				public static LocString NAME = UI.FormatAsLink("Wash station", "BUILDCATEGORYREQUIREMENTCLASSWASHSTATION");

				// Token: 0x0400C210 RID: 49680
				public static LocString DESCRIPTION = "Requires one or more Wash Basins, Sinks, Hand Sanitizers, or Showers";

				// Token: 0x0400C211 RID: 49681
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WASHSTATION.NAME;
			}

			// Token: 0x02002CA9 RID: 11433
			public class ADVANCEDWASHSTATION
			{
				// Token: 0x0400C212 RID: 49682
				public static LocString NAME = UI.FormatAsLink("Plumbed wash station", "BUILDCATEGORYREQUIREMENTCLASSWASHSTATION");

				// Token: 0x0400C213 RID: 49683
				public static LocString DESCRIPTION = "Requires one or more Sinks, Hand Sanitizers, or Showers";

				// Token: 0x0400C214 RID: 49684
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.ADVANCEDWASHSTATION.NAME;
			}

			// Token: 0x02002CAA RID: 11434
			public class NO_INDUSTRIAL_MACHINERY
			{
				// Token: 0x0400C215 RID: 49685
				public static LocString NAME = "No " + UI.FormatAsLink("industrial machinery", "BUILDCATEGORYREQUIREMENTCLASSINDUSTRIALMACHINERY");

				// Token: 0x0400C216 RID: 49686
				public static LocString DESCRIPTION = "Cannot contain any building labeled Industrial Machinery";

				// Token: 0x0400C217 RID: 49687
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.NAME;
			}

			// Token: 0x02002CAB RID: 11435
			public class WILDANIMAL
			{
				// Token: 0x0400C218 RID: 49688
				public static LocString NAME = "Wildlife";

				// Token: 0x0400C219 RID: 49689
				public static LocString DESCRIPTION = "Requires at least one wild critter";

				// Token: 0x0400C21A RID: 49690
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDANIMAL.NAME;
			}

			// Token: 0x02002CAC RID: 11436
			public class WILDANIMALS
			{
				// Token: 0x0400C21B RID: 49691
				public static LocString NAME = "More wildlife";

				// Token: 0x0400C21C RID: 49692
				public static LocString DESCRIPTION = "Requires two or more wild critters";

				// Token: 0x0400C21D RID: 49693
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDANIMALS.NAME;
			}

			// Token: 0x02002CAD RID: 11437
			public class WILDPLANT
			{
				// Token: 0x0400C21E RID: 49694
				public static LocString NAME = "Two wild plants";

				// Token: 0x0400C21F RID: 49695
				public static LocString DESCRIPTION = "Requires two or more wild plants";

				// Token: 0x0400C220 RID: 49696
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDPLANT.NAME;
			}

			// Token: 0x02002CAE RID: 11438
			public class WILDPLANTS
			{
				// Token: 0x0400C221 RID: 49697
				public static LocString NAME = "Four wild plants";

				// Token: 0x0400C222 RID: 49698
				public static LocString DESCRIPTION = "Requires four or more wild plants";

				// Token: 0x0400C223 RID: 49699
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDPLANTS.NAME;
			}

			// Token: 0x02002CAF RID: 11439
			public class SCIENCEBUILDING
			{
				// Token: 0x0400C224 RID: 49700
				public static LocString NAME = UI.FormatAsLink("Science building", "BUILDCATEGORYREQUIREMENTCLASSSCIENCEBUILDING");

				// Token: 0x0400C225 RID: 49701
				public static LocString DESCRIPTION = "Requires one or more science buildings";

				// Token: 0x0400C226 RID: 49702
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.SCIENCEBUILDING.NAME;
			}

			// Token: 0x02002CB0 RID: 11440
			public class SCIENCE_BUILDINGS
			{
				// Token: 0x0400C227 RID: 49703
				public static LocString NAME = "Two " + UI.FormatAsLink("science buildings", "BUILDCATEGORYREQUIREMENTCLASSSCIENCEBUILDING");

				// Token: 0x0400C228 RID: 49704
				public static LocString DESCRIPTION = "Requires two or more science buildings";

				// Token: 0x0400C229 RID: 49705
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.SCIENCE_BUILDINGS.NAME;
			}

			// Token: 0x02002CB1 RID: 11441
			public class ROCKETINTERIOR
			{
				// Token: 0x0400C22A RID: 49706
				public static LocString NAME = UI.FormatAsLink("Rocket interior", "BUILDCATEGORYREQUIREMENTCLASSROCKETINTERIOR");

				// Token: 0x0400C22B RID: 49707
				public static LocString DESCRIPTION = "Must be built inside a rocket";

				// Token: 0x0400C22C RID: 49708
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.ROCKETINTERIOR.NAME;
			}

			// Token: 0x02002CB2 RID: 11442
			public class WARMINGSTATION
			{
				// Token: 0x0400C22D RID: 49709
				public static LocString NAME = UI.FormatAsLink("Warming station", "BUILDCATEGORYREQUIREMENTCLASSWARMINGSTATION");

				// Token: 0x0400C22E RID: 49710
				public static LocString DESCRIPTION = "Raises the ambient temperature";

				// Token: 0x0400C22F RID: 49711
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WARMINGSTATION.NAME;
			}

			// Token: 0x02002CB3 RID: 11443
			public class GENERATORTYPE
			{
				// Token: 0x0400C230 RID: 49712
				public static LocString NAME = UI.FormatAsLink("Generator", "BUILDCATEGORYREQUIREMENTCLASSGENERATORTYPE");

				// Token: 0x0400C231 RID: 49713
				public static LocString DESCRIPTION = "Generates electrical power";

				// Token: 0x0400C232 RID: 49714
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.GENERATORTYPE.NAME;
			}

			// Token: 0x02002CB4 RID: 11444
			public class HEAVYDUTYGENERATORTYPE
			{
				// Token: 0x0400C233 RID: 49715
				public static LocString NAME = UI.FormatAsLink("Heavy-duty generator", "BUILDCATEGORYREQUIREMENTCLASSGENERATORTYPE");

				// Token: 0x0400C234 RID: 49716
				public static LocString DESCRIPTION = "For big power needs";

				// Token: 0x0400C235 RID: 49717
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.HEAVYDUTYGENERATORTYPE.NAME;
			}

			// Token: 0x02002CB5 RID: 11445
			public class LIGHTDUTYGENERATORTYPE
			{
				// Token: 0x0400C236 RID: 49718
				public static LocString NAME = UI.FormatAsLink("Basic generator", "BUILDCATEGORYREQUIREMENTCLASSGENERATORTYPE");

				// Token: 0x0400C237 RID: 49719
				public static LocString DESCRIPTION = "For regular power needs";

				// Token: 0x0400C238 RID: 49720
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.LIGHTDUTYGENERATORTYPE.NAME;
			}

			// Token: 0x02002CB6 RID: 11446
			public class POWERBUILDING
			{
				// Token: 0x0400C239 RID: 49721
				public static LocString NAME = UI.FormatAsLink("Power building", "BUILDCATEGORYREQUIREMENTCLASSPOWERBUILDING");

				// Token: 0x0400C23A RID: 49722
				public static LocString DESCRIPTION = "Power buildings";

				// Token: 0x0400C23B RID: 49723
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.POWERBUILDING.NAME;
			}
		}

		// Token: 0x02002195 RID: 8597
		public class DETAILS
		{
			// Token: 0x040098D6 RID: 39126
			public static LocString HEADER = "Room Details";

			// Token: 0x02002CB7 RID: 11447
			public class ASSIGNED_TO
			{
				// Token: 0x0400C23C RID: 49724
				public static LocString NAME = "<b>Assignments:</b>\n{0}";

				// Token: 0x0400C23D RID: 49725
				public static LocString UNASSIGNED = "Unassigned";
			}

			// Token: 0x02002CB8 RID: 11448
			public class AVERAGE_TEMPERATURE
			{
				// Token: 0x0400C23E RID: 49726
				public static LocString NAME = "Average temperature: {0}";
			}

			// Token: 0x02002CB9 RID: 11449
			public class AVERAGE_ATMO_MASS
			{
				// Token: 0x0400C23F RID: 49727
				public static LocString NAME = "Average air pressure: {0}";
			}

			// Token: 0x02002CBA RID: 11450
			public class SIZE
			{
				// Token: 0x0400C240 RID: 49728
				public static LocString NAME = "Room size: {0} Tiles";
			}

			// Token: 0x02002CBB RID: 11451
			public class BUILDING_COUNT
			{
				// Token: 0x0400C241 RID: 49729
				public static LocString NAME = "Buildings: {0}";
			}

			// Token: 0x02002CBC RID: 11452
			public class CREATURE_COUNT
			{
				// Token: 0x0400C242 RID: 49730
				public static LocString NAME = "Critters: {0}";
			}

			// Token: 0x02002CBD RID: 11453
			public class PLANT_COUNT
			{
				// Token: 0x0400C243 RID: 49731
				public static LocString NAME = "Plants: {0}";
			}
		}

		// Token: 0x02002196 RID: 8598
		public class EFFECTS
		{
			// Token: 0x040098D7 RID: 39127
			public static LocString HEADER = "<b>Effects:</b>";
		}
	}
}
