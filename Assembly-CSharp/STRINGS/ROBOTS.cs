using System;

namespace STRINGS
{
	// Token: 0x02000F1B RID: 3867
	public class ROBOTS
	{
		// Token: 0x0400591A RID: 22810
		public static LocString CATEGORY_NAME = "Robots";

		// Token: 0x020021CA RID: 8650
		public class STATS
		{
			// Token: 0x02003440 RID: 13376
			public class INTERNALBATTERY
			{
				// Token: 0x0400D476 RID: 54390
				public static LocString NAME = "Rechargeable Battery";

				// Token: 0x0400D477 RID: 54391
				public static LocString TOOLTIP = "When this bot's battery runs out it must temporarily stop working to go recharge";
			}

			// Token: 0x02003441 RID: 13377
			public class INTERNALCHEMICALBATTERY
			{
				// Token: 0x0400D478 RID: 54392
				public static LocString NAME = "Chemical Battery";

				// Token: 0x0400D479 RID: 54393
				public static LocString TOOLTIP = "This bot will shut down permanently when its battery runs out";
			}

			// Token: 0x02003442 RID: 13378
			public class INTERNALBIOBATTERY
			{
				// Token: 0x0400D47A RID: 54394
				public static LocString NAME = "Biofuel";

				// Token: 0x0400D47B RID: 54395
				public static LocString TOOLTIP = "This bot will shut down permanently when its biofuel runs out";
			}

			// Token: 0x02003443 RID: 13379
			public class INTERNALELECTROBANK
			{
				// Token: 0x0400D47C RID: 54396
				public static LocString NAME = "Power Bank";

				// Token: 0x0400D47D RID: 54397
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"When this bot's ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" runs out, it will stop working until a fully charged one is delivered"
				});
			}
		}

		// Token: 0x020021CB RID: 8651
		public class ATTRIBUTES
		{
			// Token: 0x02003444 RID: 13380
			public class INTERNALBATTERYDELTA
			{
				// Token: 0x0400D47E RID: 54398
				public static LocString NAME = "Rechargeable Battery Drain";

				// Token: 0x0400D47F RID: 54399
				public static LocString TOOLTIP = "The rate at which battery life is depleted";
			}
		}

		// Token: 0x020021CC RID: 8652
		public class STATUSITEMS
		{
			// Token: 0x02003445 RID: 13381
			public class CANTREACHSTATION
			{
				// Token: 0x0400D480 RID: 54400
				public static LocString NAME = "Unreachable Dock";

				// Token: 0x0400D481 RID: 54401
				public static LocString DESC = "Obstacles are preventing {0} from heading home";

				// Token: 0x0400D482 RID: 54402
				public static LocString TOOLTIP = "Obstacles are preventing {0} from heading home";
			}

			// Token: 0x02003446 RID: 13382
			public class MOVINGTOCHARGESTATION
			{
				// Token: 0x0400D483 RID: 54403
				public static LocString NAME = "Traveling to Dock";

				// Token: 0x0400D484 RID: 54404
				public static LocString DESC = "{0} is on its way home to recharge";

				// Token: 0x0400D485 RID: 54405
				public static LocString TOOLTIP = "{0} is on its way home to recharge";
			}

			// Token: 0x02003447 RID: 13383
			public class LOWBATTERY
			{
				// Token: 0x0400D486 RID: 54406
				public static LocString NAME = "Low Battery";

				// Token: 0x0400D487 RID: 54407
				public static LocString DESC = "{0}'s battery is low and needs to recharge";

				// Token: 0x0400D488 RID: 54408
				public static LocString TOOLTIP = "{0}'s battery is low and needs to recharge";
			}

			// Token: 0x02003448 RID: 13384
			public class LOWBATTERYNOCHARGE
			{
				// Token: 0x0400D489 RID: 54409
				public static LocString NAME = "Low Battery";

				// Token: 0x0400D48A RID: 54410
				public static LocString DESC = "{0}'s battery is low\n\nThe internal battery cannot be recharged and this robot will cease functioning after it is depleted.";

				// Token: 0x0400D48B RID: 54411
				public static LocString TOOLTIP = "{0}'s battery is low\n\nThe internal battery cannot be recharged and this robot will cease functioning after it is depleted.";
			}

			// Token: 0x02003449 RID: 13385
			public class DEADBATTERY
			{
				// Token: 0x0400D48C RID: 54412
				public static LocString NAME = "Shut Down";

				// Token: 0x0400D48D RID: 54413
				public static LocString DESC = "RIP {0}\n\n{0}'s battery has been depleted and cannot be recharged";

				// Token: 0x0400D48E RID: 54414
				public static LocString TOOLTIP = "RIP {0}\n\n{0}'s battery has been depleted and cannot be recharged";
			}

			// Token: 0x0200344A RID: 13386
			public class DEADBATTERYFLYDO
			{
				// Token: 0x0400D48F RID: 54415
				public static LocString NAME = "Shut Down";

				// Token: 0x0400D490 RID: 54416
				public static LocString DESC = "{0}'s battery has been depleted\n\n{0} will resume function when a new battery has been delivered";

				// Token: 0x0400D491 RID: 54417
				public static LocString TOOLTIP = "{0}'s battery has been depleted\n\n{0} will resume function when a new battery has been delivered";
			}

			// Token: 0x0200344B RID: 13387
			public class DUSTBINFULL
			{
				// Token: 0x0400D492 RID: 54418
				public static LocString NAME = "Dust Bin Full";

				// Token: 0x0400D493 RID: 54419
				public static LocString DESC = "{0} must return to its dock to unload";

				// Token: 0x0400D494 RID: 54420
				public static LocString TOOLTIP = "{0} must return to its dock to unload";
			}

			// Token: 0x0200344C RID: 13388
			public class WORKING
			{
				// Token: 0x0400D495 RID: 54421
				public static LocString NAME = "Working";

				// Token: 0x0400D496 RID: 54422
				public static LocString DESC = "{0} is working diligently. Great job, {0}!";

				// Token: 0x0400D497 RID: 54423
				public static LocString TOOLTIP = "{0} is working diligently. Great job, {0}!";
			}

			// Token: 0x0200344D RID: 13389
			public class UNLOADINGSTORAGE
			{
				// Token: 0x0400D498 RID: 54424
				public static LocString NAME = "Unloading";

				// Token: 0x0400D499 RID: 54425
				public static LocString DESC = "{0} is emptying out its dust bin";

				// Token: 0x0400D49A RID: 54426
				public static LocString TOOLTIP = "{0} is emptying out its dust bin";
			}

			// Token: 0x0200344E RID: 13390
			public class CHARGING
			{
				// Token: 0x0400D49B RID: 54427
				public static LocString NAME = "Charging";

				// Token: 0x0400D49C RID: 54428
				public static LocString DESC = "{0} is recharging its battery";

				// Token: 0x0400D49D RID: 54429
				public static LocString TOOLTIP = "{0} is recharging its battery";
			}

			// Token: 0x0200344F RID: 13391
			public class REACTPOSITIVE
			{
				// Token: 0x0400D49E RID: 54430
				public static LocString NAME = "Happy Reaction";

				// Token: 0x0400D49F RID: 54431
				public static LocString DESC = "This bot saw something nice!";

				// Token: 0x0400D4A0 RID: 54432
				public static LocString TOOLTIP = "This bot saw something nice!";
			}

			// Token: 0x02003450 RID: 13392
			public class REACTNEGATIVE
			{
				// Token: 0x0400D4A1 RID: 54433
				public static LocString NAME = "Bothered Reaction";

				// Token: 0x0400D4A2 RID: 54434
				public static LocString DESC = "This bot saw something upsetting";

				// Token: 0x0400D4A3 RID: 54435
				public static LocString TOOLTIP = "This bot saw something upsetting";
			}
		}

		// Token: 0x020021CD RID: 8653
		public class MODELS
		{
			// Token: 0x02003451 RID: 13393
			public class MORB
			{
				// Token: 0x0400D4A4 RID: 54436
				public static LocString NAME = UI.FormatAsLink("Biobot", "STORYTRAITMORBROVER");

				// Token: 0x0400D4A5 RID: 54437
				public static LocString DESC = "A Pathogen-Fueled Extravehicular Geo-Exploratory Guidebot (model Y), aka \"P.E.G.G.Y.\"\n\nIt can be assigned basic building tasks and digging duties in hazardous environments.";

				// Token: 0x0400D4A6 RID: 54438
				public static LocString CODEX_DESC = "The pathogen-fueled guidebot is designed to maximize a colony's chances of surviving in hostile environments by meeting three core outcomes:\n\n1. Filtration and removal of toxins from environment;\n2. Safe disposal of filtered toxins through conversion into usable biofuel;\n3. Creation of geo-exploration equipment for colony expansion with minimal colonist endangerment.\n\nThe elements aggregated during this process may result in the unintentional spread of contaminants. Specialized training required for safe handling.";
			}

			// Token: 0x02003452 RID: 13394
			public class SCOUT
			{
				// Token: 0x0400D4A7 RID: 54439
				public static LocString NAME = "Rover";

				// Token: 0x0400D4A8 RID: 54440
				public static LocString DESC = "A curious bot that can remotely explore new " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " locations.";
			}

			// Token: 0x02003453 RID: 13395
			public class SWEEPBOT
			{
				// Token: 0x0400D4A9 RID: 54441
				public static LocString NAME = "Sweepy";

				// Token: 0x0400D4AA RID: 54442
				public static LocString DESC = string.Concat(new string[]
				{
					"An automated sweeping robot.\n\nSweeps up ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					" debris and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" spills and stores the material back in its ",
					UI.FormatAsLink("Sweepy Dock", "SWEEPBOTSTATION"),
					"."
				});
			}

			// Token: 0x02003454 RID: 13396
			public class FLYDO
			{
				// Token: 0x0400D4AB RID: 54443
				public static LocString NAME = "Flydo";

				// Token: 0x0400D4AC RID: 54444
				public static LocString DESC = "A programmable delivery robot.\n\nPicks up " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " objects for delivery to selected destinations.";
			}
		}
	}
}
