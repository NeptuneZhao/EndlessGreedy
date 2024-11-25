using System;
using TUNING;

namespace STRINGS
{
	// Token: 0x02000F19 RID: 3865
	public class DUPLICANTS
	{
		// Token: 0x04005912 RID: 22802
		public static LocString RACE_PREFIX = "Species: {0}";

		// Token: 0x04005913 RID: 22803
		public static LocString RACE = "Duplicant";

		// Token: 0x04005914 RID: 22804
		public static LocString MODELTITLE = "Species: ";

		// Token: 0x04005915 RID: 22805
		public static LocString NAMETITLE = "Name: ";

		// Token: 0x04005916 RID: 22806
		public static LocString GENDERTITLE = "Gender: ";

		// Token: 0x04005917 RID: 22807
		public static LocString ARRIVALTIME = "Age: ";

		// Token: 0x04005918 RID: 22808
		public static LocString ARRIVALTIME_TOOLTIP = "This {1} was printed on <b>Cycle {0}</b>";

		// Token: 0x04005919 RID: 22809
		public static LocString DESC_TOOLTIP = "About {0}s";

		// Token: 0x020021AC RID: 8620
		public class MODEL
		{
			// Token: 0x02002FEC RID: 12268
			public class STANDARD
			{
				// Token: 0x0400C986 RID: 51590
				public static LocString NAME = "Standard Duplicant";
			}

			// Token: 0x02002FED RID: 12269
			public class BIONIC
			{
				// Token: 0x0400C987 RID: 51591
				public static LocString NAME = "Bionic Duplicant";

				// Token: 0x0400C988 RID: 51592
				public static LocString NAME_TOOLTIP = "This Duplicant is a curious combination of organic and inorganic parts";
			}

			// Token: 0x02002FEE RID: 12270
			public class REMOTEWORKER
			{
				// Token: 0x0400C989 RID: 51593
				public static LocString NAME = "Remote Worker";

				// Token: 0x0400C98A RID: 51594
				public static LocString DESC = "A remotely operated work robot.\n\nIt performs chores as instructed by a " + UI.FormatAsLink("Remote Controller", "REMOTEWORKTERMINAL") + " on the same planetoid.";
			}
		}

		// Token: 0x020021AD RID: 8621
		public class GENDER
		{
			// Token: 0x02002FEF RID: 12271
			public class MALE
			{
				// Token: 0x0400C98B RID: 51595
				public static LocString NAME = "M";

				// Token: 0x0200381C RID: 14364
				public class PLURALS
				{
					// Token: 0x0400DE24 RID: 56868
					public static LocString ONE = "he";

					// Token: 0x0400DE25 RID: 56869
					public static LocString TWO = "his";
				}
			}

			// Token: 0x02002FF0 RID: 12272
			public class FEMALE
			{
				// Token: 0x0400C98C RID: 51596
				public static LocString NAME = "F";

				// Token: 0x0200381D RID: 14365
				public class PLURALS
				{
					// Token: 0x0400DE26 RID: 56870
					public static LocString ONE = "she";

					// Token: 0x0400DE27 RID: 56871
					public static LocString TWO = "her";
				}
			}

			// Token: 0x02002FF1 RID: 12273
			public class NB
			{
				// Token: 0x0400C98D RID: 51597
				public static LocString NAME = "X";

				// Token: 0x0200381E RID: 14366
				public class PLURALS
				{
					// Token: 0x0400DE28 RID: 56872
					public static LocString ONE = "they";

					// Token: 0x0400DE29 RID: 56873
					public static LocString TWO = "their";
				}
			}
		}

		// Token: 0x020021AE RID: 8622
		public class STATS
		{
			// Token: 0x02002FF2 RID: 12274
			public class SUBJECTS
			{
				// Token: 0x0400C98E RID: 51598
				public static LocString DUPLICANT = "Duplicant";

				// Token: 0x0400C98F RID: 51599
				public static LocString DUPLICANT_POSSESSIVE = "Duplicant's";

				// Token: 0x0400C990 RID: 51600
				public static LocString DUPLICANT_PLURAL = "Duplicants";

				// Token: 0x0400C991 RID: 51601
				public static LocString CREATURE = "critter";

				// Token: 0x0400C992 RID: 51602
				public static LocString CREATURE_POSSESSIVE = "critter's";

				// Token: 0x0400C993 RID: 51603
				public static LocString CREATURE_PLURAL = "critters";

				// Token: 0x0400C994 RID: 51604
				public static LocString PLANT = "plant";

				// Token: 0x0400C995 RID: 51605
				public static LocString PLANT_POSESSIVE = "plant's";

				// Token: 0x0400C996 RID: 51606
				public static LocString PLANT_PLURAL = "plants";
			}

			// Token: 0x02002FF3 RID: 12275
			public class BIONICINTERNALBATTERY
			{
				// Token: 0x0400C997 RID: 51607
				public static LocString NAME = "Power Banks";

				// Token: 0x0400C998 RID: 51608
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Bionic Duplicant with zero remaining ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" will become incapacitated until replacement ",
					UI.PRE_KEYWORD,
					"Power Banks",
					UI.PST_KEYWORD,
					" are installed"
				});
			}

			// Token: 0x02002FF4 RID: 12276
			public class BIONICOXYGENTANK
			{
				// Token: 0x0400C999 RID: 51609
				public static LocString NAME = "Oxygen Tank";

				// Token: 0x0400C99A RID: 51610
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants have internal ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" tanks that enable them to work in low breathability areas"
				});
			}

			// Token: 0x02002FF5 RID: 12277
			public class BIONICOIL
			{
				// Token: 0x0400C99B RID: 51611
				public static LocString NAME = "Oil";

				// Token: 0x0400C99C RID: 51612
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants will slow down significantly when ",
					UI.PRE_KEYWORD,
					"Oil",
					UI.PST_KEYWORD,
					" levels reach zero\n\nDuplicants can obtain ",
					UI.PRE_KEYWORD,
					"Phyto Oil",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Crude Oil",
					UI.PST_KEYWORD,
					" at the ",
					UI.PRE_KEYWORD,
					"Lubrication Station",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002FF6 RID: 12278
			public class BIONICGUNK
			{
				// Token: 0x0400C99D RID: 51613
				public static LocString NAME = "Gunk";

				// Token: 0x0400C99E RID: 51614
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants become ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" when too much ",
					UI.PRE_KEYWORD,
					"Liquid Gunk",
					UI.PST_KEYWORD,
					" builds up in their bionic parts\n\nRegular visits to the ",
					UI.PRE_KEYWORD,
					"Gunk Extractor",
					UI.PST_KEYWORD,
					" are required"
				});
			}

			// Token: 0x02002FF7 RID: 12279
			public class BREATH
			{
				// Token: 0x0400C99F RID: 51615
				public static LocString NAME = "Breath";

				// Token: 0x0400C9A0 RID: 51616
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant with zero remaining ",
					UI.PRE_KEYWORD,
					"Breath",
					UI.PST_KEYWORD,
					" will begin suffocating"
				});
			}

			// Token: 0x02002FF8 RID: 12280
			public class STAMINA
			{
				// Token: 0x0400C9A1 RID: 51617
				public static LocString NAME = "Stamina";

				// Token: 0x0400C9A2 RID: 51618
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will pass out from fatigue when ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					" reaches zero"
				});
			}

			// Token: 0x02002FF9 RID: 12281
			public class CALORIES
			{
				// Token: 0x0400C9A3 RID: 51619
				public static LocString NAME = "Calories";

				// Token: 0x0400C9A4 RID: 51620
				public static LocString TOOLTIP = "This {1} can burn <b>{0}</b> before starving";
			}

			// Token: 0x02002FFA RID: 12282
			public class TEMPERATURE
			{
				// Token: 0x0400C9A5 RID: 51621
				public static LocString NAME = "Body Temperature";

				// Token: 0x0400C9A6 RID: 51622
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A healthy Duplicant's ",
					UI.PRE_KEYWORD,
					"Body Temperature",
					UI.PST_KEYWORD,
					" is <b>{1}</b>"
				});

				// Token: 0x0400C9A7 RID: 51623
				public static LocString TOOLTIP_DOMESTICATEDCRITTER = string.Concat(new string[]
				{
					"This critter's ",
					UI.PRE_KEYWORD,
					"Body Temperature",
					UI.PST_KEYWORD,
					" is <b>{1}</b>"
				});
			}

			// Token: 0x02002FFB RID: 12283
			public class EXTERNALTEMPERATURE
			{
				// Token: 0x0400C9A8 RID: 51624
				public static LocString NAME = "External Temperature";

				// Token: 0x0400C9A9 RID: 51625
				public static LocString TOOLTIP = "This Duplicant's environment is <b>{0}</b>";
			}

			// Token: 0x02002FFC RID: 12284
			public class DECOR
			{
				// Token: 0x0400C9AA RID: 51626
				public static LocString NAME = "Decor";

				// Token: 0x0400C9AB RID: 51627
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants become stressed in areas with ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" lower than their expectations\n\nOpen the ",
					UI.FormatAsOverlay("Decor Overlay", global::Action.Overlay8),
					" to view current ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values"
				});

				// Token: 0x0400C9AC RID: 51628
				public static LocString TOOLTIP_CURRENT = "\n\nCurrent Environmental Decor: <b>{0}</b>";

				// Token: 0x0400C9AD RID: 51629
				public static LocString TOOLTIP_AVERAGE_TODAY = "\nAverage Decor This Cycle: <b>{0}</b>";

				// Token: 0x0400C9AE RID: 51630
				public static LocString TOOLTIP_AVERAGE_YESTERDAY = "\nAverage Decor Last Cycle: <b>{0}</b>";
			}

			// Token: 0x02002FFD RID: 12285
			public class STRESS
			{
				// Token: 0x0400C9AF RID: 51631
				public static LocString NAME = "Stress";

				// Token: 0x0400C9B0 RID: 51632
				public static LocString TOOLTIP = "Duplicants exhibit their Stress Reactions at one hundred percent " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02002FFE RID: 12286
			public class RADIATIONBALANCE
			{
				// Token: 0x0400C9B1 RID: 51633
				public static LocString NAME = "Absorbed Rad Dose";

				// Token: 0x0400C9B2 RID: 51634
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants accumulate Rads in areas with ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" and recover when using the toilet\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay", global::Action.Overlay15),
					" to view current ",
					UI.PRE_KEYWORD,
					"Rad",
					UI.PST_KEYWORD,
					" readings"
				});

				// Token: 0x0400C9B3 RID: 51635
				public static LocString TOOLTIP_CURRENT_BALANCE = string.Concat(new string[]
				{
					"Duplicants accumulate Rads in areas with ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" and recover when using the toilet\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay", global::Action.Overlay15),
					" to view current ",
					UI.PRE_KEYWORD,
					"Rad",
					UI.PST_KEYWORD,
					" readings"
				});

				// Token: 0x0400C9B4 RID: 51636
				public static LocString CURRENT_EXPOSURE = "Current Exposure: {0}/cycle";

				// Token: 0x0400C9B5 RID: 51637
				public static LocString CURRENT_REJUVENATION = "Current Rejuvenation: {0}/cycle";
			}

			// Token: 0x02002FFF RID: 12287
			public class BLADDER
			{
				// Token: 0x0400C9B6 RID: 51638
				public static LocString NAME = "Bladder";

				// Token: 0x0400C9B7 RID: 51639
				public static LocString TOOLTIP = "Duplicants make \"messes\" if no toilets are available at one hundred percent " + UI.PRE_KEYWORD + "Bladder" + UI.PST_KEYWORD;
			}

			// Token: 0x02003000 RID: 12288
			public class HITPOINTS
			{
				// Token: 0x0400C9B8 RID: 51640
				public static LocString NAME = "Health";

				// Token: 0x0400C9B9 RID: 51641
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"When Duplicants reach zero ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					" they become incapacitated and require rescuing\n\nWhen critters reach zero ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					", they will die immediately"
				});
			}

			// Token: 0x02003001 RID: 12289
			public class SKIN_THICKNESS
			{
				// Token: 0x0400C9BA RID: 51642
				public static LocString NAME = "Skin Thickness";
			}

			// Token: 0x02003002 RID: 12290
			public class SKIN_DURABILITY
			{
				// Token: 0x0400C9BB RID: 51643
				public static LocString NAME = "Skin Durability";
			}

			// Token: 0x02003003 RID: 12291
			public class DISEASERECOVERYTIME
			{
				// Token: 0x0400C9BC RID: 51644
				public static LocString NAME = "Disease Recovery";
			}

			// Token: 0x02003004 RID: 12292
			public class TRUNKHEALTH
			{
				// Token: 0x0400C9BD RID: 51645
				public static LocString NAME = "Trunk Health";

				// Token: 0x0400C9BE RID: 51646
				public static LocString TOOLTIP = "Tree branches will die if they do not have a healthy trunk to grow from";
			}
		}

		// Token: 0x020021AF RID: 8623
		public class DEATHS
		{
			// Token: 0x02003005 RID: 12293
			public class GENERIC
			{
				// Token: 0x0400C9BF RID: 51647
				public static LocString NAME = "Death";

				// Token: 0x0400C9C0 RID: 51648
				public static LocString DESCRIPTION = "{Target} has died.";
			}

			// Token: 0x02003006 RID: 12294
			public class FROZEN
			{
				// Token: 0x0400C9C1 RID: 51649
				public static LocString NAME = "Frozen";

				// Token: 0x0400C9C2 RID: 51650
				public static LocString DESCRIPTION = "{Target} has frozen to death.";
			}

			// Token: 0x02003007 RID: 12295
			public class SUFFOCATION
			{
				// Token: 0x0400C9C3 RID: 51651
				public static LocString NAME = "Suffocation";

				// Token: 0x0400C9C4 RID: 51652
				public static LocString DESCRIPTION = "{Target} has suffocated to death.";
			}

			// Token: 0x02003008 RID: 12296
			public class STARVATION
			{
				// Token: 0x0400C9C5 RID: 51653
				public static LocString NAME = "Starvation";

				// Token: 0x0400C9C6 RID: 51654
				public static LocString DESCRIPTION = "{Target} has starved to death.";
			}

			// Token: 0x02003009 RID: 12297
			public class OVERHEATING
			{
				// Token: 0x0400C9C7 RID: 51655
				public static LocString NAME = "Overheated";

				// Token: 0x0400C9C8 RID: 51656
				public static LocString DESCRIPTION = "{Target} overheated to death.";
			}

			// Token: 0x0200300A RID: 12298
			public class DROWNED
			{
				// Token: 0x0400C9C9 RID: 51657
				public static LocString NAME = "Drowned";

				// Token: 0x0400C9CA RID: 51658
				public static LocString DESCRIPTION = "{Target} has drowned.";
			}

			// Token: 0x0200300B RID: 12299
			public class EXPLOSION
			{
				// Token: 0x0400C9CB RID: 51659
				public static LocString NAME = "Explosion";

				// Token: 0x0400C9CC RID: 51660
				public static LocString DESCRIPTION = "{Target} has died in an explosion.";
			}

			// Token: 0x0200300C RID: 12300
			public class COMBAT
			{
				// Token: 0x0400C9CD RID: 51661
				public static LocString NAME = "Slain";

				// Token: 0x0400C9CE RID: 51662
				public static LocString DESCRIPTION = "{Target} succumbed to their wounds after being incapacitated.";
			}

			// Token: 0x0200300D RID: 12301
			public class FATALDISEASE
			{
				// Token: 0x0400C9CF RID: 51663
				public static LocString NAME = "Succumbed to Disease";

				// Token: 0x0400C9D0 RID: 51664
				public static LocString DESCRIPTION = "{Target} has died of a fatal illness.";
			}

			// Token: 0x0200300E RID: 12302
			public class RADIATION
			{
				// Token: 0x0400C9D1 RID: 51665
				public static LocString NAME = "Irradiated";

				// Token: 0x0400C9D2 RID: 51666
				public static LocString DESCRIPTION = "{Target} perished from excessive radiation exposure.";
			}

			// Token: 0x0200300F RID: 12303
			public class HITBYHIGHENERGYPARTICLE
			{
				// Token: 0x0400C9D3 RID: 51667
				public static LocString NAME = "Struck by Radbolt";

				// Token: 0x0400C9D4 RID: 51668
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{Target} was struck by a radioactive ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" and perished."
				});
			}
		}

		// Token: 0x020021B0 RID: 8624
		public class CHORES
		{
			// Token: 0x0400998C RID: 39308
			public static LocString NOT_EXISTING_TASK = "Not Existing";

			// Token: 0x0400998D RID: 39309
			public static LocString IS_DEAD_TASK = "Dead";

			// Token: 0x02003010 RID: 12304
			public class THINKING
			{
				// Token: 0x0400C9D5 RID: 51669
				public static LocString NAME = "Ponder";

				// Token: 0x0400C9D6 RID: 51670
				public static LocString STATUS = "Pondering";

				// Token: 0x0400C9D7 RID: 51671
				public static LocString TOOLTIP = "This Duplicant is mulling over what they should do next";
			}

			// Token: 0x02003011 RID: 12305
			public class ASTRONAUT
			{
				// Token: 0x0400C9D8 RID: 51672
				public static LocString NAME = "Space Mission";

				// Token: 0x0400C9D9 RID: 51673
				public static LocString STATUS = "On space mission";

				// Token: 0x0400C9DA RID: 51674
				public static LocString TOOLTIP = "This Duplicant is exploring the vast universe";
			}

			// Token: 0x02003012 RID: 12306
			public class DIE
			{
				// Token: 0x0400C9DB RID: 51675
				public static LocString NAME = "Die";

				// Token: 0x0400C9DC RID: 51676
				public static LocString STATUS = "Dying";

				// Token: 0x0400C9DD RID: 51677
				public static LocString TOOLTIP = "Fare thee well, brave soul";
			}

			// Token: 0x02003013 RID: 12307
			public class ENTOMBED
			{
				// Token: 0x0400C9DE RID: 51678
				public static LocString NAME = "Entombed";

				// Token: 0x0400C9DF RID: 51679
				public static LocString STATUS = "Entombed";

				// Token: 0x0400C9E0 RID: 51680
				public static LocString TOOLTIP = "Entombed Duplicants are at risk of suffocating and must be dug out by others in the colony";
			}

			// Token: 0x02003014 RID: 12308
			public class BEINCAPACITATED
			{
				// Token: 0x0400C9E1 RID: 51681
				public static LocString NAME = "Incapacitated";

				// Token: 0x0400C9E2 RID: 51682
				public static LocString STATUS = "Dying";

				// Token: 0x0400C9E3 RID: 51683
				public static LocString TOOLTIP = "This Duplicant will die soon if they do not receive assistance";
			}

			// Token: 0x02003015 RID: 12309
			public class BEOFFLINE
			{
				// Token: 0x0400C9E4 RID: 51684
				public static LocString NAME = "Powerless";

				// Token: 0x0400C9E5 RID: 51685
				public static LocString STATUS = "Powerless";

				// Token: 0x0400C9E6 RID: 51686
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant does not have enough ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" to function"
				});
			}

			// Token: 0x02003016 RID: 12310
			public class BEBATTERYSAVEMODE
			{
				// Token: 0x0400C9E7 RID: 51687
				public static LocString NAME = "Standby Mode";

				// Token: 0x0400C9E8 RID: 51688
				public static LocString STATUS = "Standby Mode";

				// Token: 0x0400C9E9 RID: 51689
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is resting\n\nTheir ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumption is at a minimum"
				});
			}

			// Token: 0x02003017 RID: 12311
			public class GENESHUFFLE
			{
				// Token: 0x0400C9EA RID: 51690
				public static LocString NAME = "Use Neural Vacillator";

				// Token: 0x0400C9EB RID: 51691
				public static LocString STATUS = "Using Neural Vacillator";

				// Token: 0x0400C9EC RID: 51692
				public static LocString TOOLTIP = "This Duplicant is being experimented on!";
			}

			// Token: 0x02003018 RID: 12312
			public class MIGRATE
			{
				// Token: 0x0400C9ED RID: 51693
				public static LocString NAME = "Use Teleporter";

				// Token: 0x0400C9EE RID: 51694
				public static LocString STATUS = "Using Teleporter";

				// Token: 0x0400C9EF RID: 51695
				public static LocString TOOLTIP = "This Duplicant's molecules are hurtling through the air!";
			}

			// Token: 0x02003019 RID: 12313
			public class DEBUGGOTO
			{
				// Token: 0x0400C9F0 RID: 51696
				public static LocString NAME = "DebugGoTo";

				// Token: 0x0400C9F1 RID: 51697
				public static LocString STATUS = "DebugGoTo";
			}

			// Token: 0x0200301A RID: 12314
			public class DISINFECT
			{
				// Token: 0x0400C9F2 RID: 51698
				public static LocString NAME = "Disinfect";

				// Token: 0x0400C9F3 RID: 51699
				public static LocString STATUS = "Going to disinfect";

				// Token: 0x0400C9F4 RID: 51700
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Buildings can be disinfected to remove contagious ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" from their surface"
				});
			}

			// Token: 0x0200301B RID: 12315
			public class EQUIPPINGSUIT
			{
				// Token: 0x0400C9F5 RID: 51701
				public static LocString NAME = "Equip Exosuit";

				// Token: 0x0400C9F6 RID: 51702
				public static LocString STATUS = "Equipping exosuit";

				// Token: 0x0400C9F7 RID: 51703
				public static LocString TOOLTIP = "This Duplicant is putting on protective gear";
			}

			// Token: 0x0200301C RID: 12316
			public class STRESSIDLE
			{
				// Token: 0x0400C9F8 RID: 51704
				public static LocString NAME = "Antsy";

				// Token: 0x0400C9F9 RID: 51705
				public static LocString STATUS = "Antsy";

				// Token: 0x0400C9FA RID: 51706
				public static LocString TOOLTIP = "This Duplicant is a workaholic and gets stressed when they have nothing to do";
			}

			// Token: 0x0200301D RID: 12317
			public class MOVETO
			{
				// Token: 0x0400C9FB RID: 51707
				public static LocString NAME = "Move to";

				// Token: 0x0400C9FC RID: 51708
				public static LocString STATUS = "Moving to location";

				// Token: 0x0400C9FD RID: 51709
				public static LocString TOOLTIP = "This Duplicant was manually directed to move to a specific location";
			}

			// Token: 0x0200301E RID: 12318
			public class ROCKETENTEREXIT
			{
				// Token: 0x0400C9FE RID: 51710
				public static LocString NAME = "Rocket Recrewing";

				// Token: 0x0400C9FF RID: 51711
				public static LocString STATUS = "Recrewing Rocket";

				// Token: 0x0400CA00 RID: 51712
				public static LocString TOOLTIP = "This Duplicant is getting into (or out of) their assigned rocket";
			}

			// Token: 0x0200301F RID: 12319
			public class DROPUNUSEDINVENTORY
			{
				// Token: 0x0400CA01 RID: 51713
				public static LocString NAME = "Drop Inventory";

				// Token: 0x0400CA02 RID: 51714
				public static LocString STATUS = "Dropping unused inventory";

				// Token: 0x0400CA03 RID: 51715
				public static LocString TOOLTIP = "This Duplicant is dropping carried items they no longer need";
			}

			// Token: 0x02003020 RID: 12320
			public class PEE
			{
				// Token: 0x0400CA04 RID: 51716
				public static LocString NAME = "Relieve Self";

				// Token: 0x0400CA05 RID: 51717
				public static LocString STATUS = "Relieving self";

				// Token: 0x0400CA06 RID: 51718
				public static LocString TOOLTIP = "This Duplicant didn't find a toilet in time. Oops";
			}

			// Token: 0x02003021 RID: 12321
			public class EXPELLGUNK
			{
				// Token: 0x0400CA07 RID: 51719
				public static LocString NAME = "Expel Liquid Gunk";

				// Token: 0x0400CA08 RID: 51720
				public static LocString STATUS = "Expelling liquid gunk";

				// Token: 0x0400CA09 RID: 51721
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant didn't get to a ",
					UI.PRE_KEYWORD,
					"Gunk Extractor",
					UI.PST_KEYWORD,
					" in time. Urgh"
				});
			}

			// Token: 0x02003022 RID: 12322
			public class OILCHANGE
			{
				// Token: 0x0400CA0A RID: 51722
				public static LocString NAME = "Refill Oil";

				// Token: 0x0400CA0B RID: 51723
				public static LocString STATUS = "Refilling oil";

				// Token: 0x0400CA0C RID: 51724
				public static LocString TOOLTIP = "This Duplicant is making sure their internal mechanisms stay lubricated";
			}

			// Token: 0x02003023 RID: 12323
			public class BREAK_PEE
			{
				// Token: 0x0400CA0D RID: 51725
				public static LocString NAME = "Downtime: Use Toilet";

				// Token: 0x0400CA0E RID: 51726
				public static LocString STATUS = "Downtime: Going to use toilet";

				// Token: 0x0400CA0F RID: 51727
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has scheduled ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" and is using their break to go to the toilet\n\nDuplicants have to use the toilet at least once per day"
				});
			}

			// Token: 0x02003024 RID: 12324
			public class STRESSVOMIT
			{
				// Token: 0x0400CA10 RID: 51728
				public static LocString NAME = "Stress Vomit";

				// Token: 0x0400CA11 RID: 51729
				public static LocString STATUS = "Stress vomiting";

				// Token: 0x0400CA12 RID: 51730
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Some people deal with ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" better than others"
				});
			}

			// Token: 0x02003025 RID: 12325
			public class UGLY_CRY
			{
				// Token: 0x0400CA13 RID: 51731
				public static LocString NAME = "Ugly Cry";

				// Token: 0x0400CA14 RID: 51732
				public static LocString STATUS = "Ugly crying";

				// Token: 0x0400CA15 RID: 51733
				public static LocString TOOLTIP = "This Duplicant is having a healthy cry to alleviate their " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02003026 RID: 12326
			public class STRESSSHOCK
			{
				// Token: 0x0400CA16 RID: 51734
				public static LocString NAME = "Shock";

				// Token: 0x0400CA17 RID: 51735
				public static LocString STATUS = "Shocking";

				// Token: 0x0400CA18 RID: 51736
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's inability to handle ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is pretty shocking"
				});
			}

			// Token: 0x02003027 RID: 12327
			public class BINGE_EAT
			{
				// Token: 0x0400CA19 RID: 51737
				public static LocString NAME = "Binge Eat";

				// Token: 0x0400CA1A RID: 51738
				public static LocString STATUS = "Binge eating";

				// Token: 0x0400CA1B RID: 51739
				public static LocString TOOLTIP = "This Duplicant is attempting to eat their emotions due to " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02003028 RID: 12328
			public class BANSHEE_WAIL
			{
				// Token: 0x0400CA1C RID: 51740
				public static LocString NAME = "Banshee Wail";

				// Token: 0x0400CA1D RID: 51741
				public static LocString STATUS = "Wailing";

				// Token: 0x0400CA1E RID: 51742
				public static LocString TOOLTIP = "This Duplicant is emitting ear-piercing shrieks to relieve pent-up " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02003029 RID: 12329
			public class EMOTEHIGHPRIORITY
			{
				// Token: 0x0400CA1F RID: 51743
				public static LocString NAME = "Express Themselves";

				// Token: 0x0400CA20 RID: 51744
				public static LocString STATUS = "Expressing themselves";

				// Token: 0x0400CA21 RID: 51745
				public static LocString TOOLTIP = "This Duplicant needs a moment to express their feelings, then they'll be on their way";
			}

			// Token: 0x0200302A RID: 12330
			public class HUG
			{
				// Token: 0x0400CA22 RID: 51746
				public static LocString NAME = "Hug";

				// Token: 0x0400CA23 RID: 51747
				public static LocString STATUS = "Hugging";

				// Token: 0x0400CA24 RID: 51748
				public static LocString TOOLTIP = "This Duplicant is enjoying a big warm hug";
			}

			// Token: 0x0200302B RID: 12331
			public class FLEE
			{
				// Token: 0x0400CA25 RID: 51749
				public static LocString NAME = "Flee";

				// Token: 0x0400CA26 RID: 51750
				public static LocString STATUS = "Fleeing";

				// Token: 0x0400CA27 RID: 51751
				public static LocString TOOLTIP = "Run away!";
			}

			// Token: 0x0200302C RID: 12332
			public class RECOVERBREATH
			{
				// Token: 0x0400CA28 RID: 51752
				public static LocString NAME = "Recover Breath";

				// Token: 0x0400CA29 RID: 51753
				public static LocString STATUS = "Recovering breath";

				// Token: 0x0400CA2A RID: 51754
				public static LocString TOOLTIP = "";
			}

			// Token: 0x0200302D RID: 12333
			public class RECOVERFROMHEAT
			{
				// Token: 0x0400CA2B RID: 51755
				public static LocString NAME = "Recover from Heat";

				// Token: 0x0400CA2C RID: 51756
				public static LocString STATUS = "Recovering from heat";

				// Token: 0x0400CA2D RID: 51757
				public static LocString TOOLTIP = "This Duplicant's trying to cool down";
			}

			// Token: 0x0200302E RID: 12334
			public class RECOVERWARMTH
			{
				// Token: 0x0400CA2E RID: 51758
				public static LocString NAME = "Recover from Cold";

				// Token: 0x0400CA2F RID: 51759
				public static LocString STATUS = "Recovering from cold";

				// Token: 0x0400CA30 RID: 51760
				public static LocString TOOLTIP = "This Duplicant's trying to warm up";
			}

			// Token: 0x0200302F RID: 12335
			public class MOVETOQUARANTINE
			{
				// Token: 0x0400CA31 RID: 51761
				public static LocString NAME = "Move to Quarantine";

				// Token: 0x0400CA32 RID: 51762
				public static LocString STATUS = "Moving to quarantine";

				// Token: 0x0400CA33 RID: 51763
				public static LocString TOOLTIP = "This Duplicant will isolate themselves to keep their illness away from the colony";
			}

			// Token: 0x02003030 RID: 12336
			public class ATTACK
			{
				// Token: 0x0400CA34 RID: 51764
				public static LocString NAME = "Attack";

				// Token: 0x0400CA35 RID: 51765
				public static LocString STATUS = "Attacking";

				// Token: 0x0400CA36 RID: 51766
				public static LocString TOOLTIP = "Chaaaarge!";
			}

			// Token: 0x02003031 RID: 12337
			public class CAPTURE
			{
				// Token: 0x0400CA37 RID: 51767
				public static LocString NAME = "Wrangle";

				// Token: 0x0400CA38 RID: 51768
				public static LocString STATUS = "Wrangling";

				// Token: 0x0400CA39 RID: 51769
				public static LocString TOOLTIP = "Duplicants that possess the Critter Ranching Skill can wrangle most critters without traps";
			}

			// Token: 0x02003032 RID: 12338
			public class SINGTOEGG
			{
				// Token: 0x0400CA3A RID: 51770
				public static LocString NAME = "Sing To Egg";

				// Token: 0x0400CA3B RID: 51771
				public static LocString STATUS = "Singing to egg";

				// Token: 0x0400CA3C RID: 51772
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A gentle lullaby from a supportive Duplicant encourages developing ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					"\n\nIncreases ",
					UI.PRE_KEYWORD,
					"Incubation Rate",
					UI.PST_KEYWORD,
					"\n\nDuplicants must possess the ",
					DUPLICANTS.ROLES.RANCHER.NAME,
					" skill to sing to an egg"
				});
			}

			// Token: 0x02003033 RID: 12339
			public class USETOILET
			{
				// Token: 0x0400CA3D RID: 51773
				public static LocString NAME = "Use Toilet";

				// Token: 0x0400CA3E RID: 51774
				public static LocString STATUS = "Going to use toilet";

				// Token: 0x0400CA3F RID: 51775
				public static LocString TOOLTIP = "Duplicants have to use the toilet at least once per day";
			}

			// Token: 0x02003034 RID: 12340
			public class WASHHANDS
			{
				// Token: 0x0400CA40 RID: 51776
				public static LocString NAME = "Wash Hands";

				// Token: 0x0400CA41 RID: 51777
				public static LocString STATUS = "Washing hands";

				// Token: 0x0400CA42 RID: 51778
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Good hygiene removes ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" and prevents the spread of ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003035 RID: 12341
			public class SLIP
			{
				// Token: 0x0400CA43 RID: 51779
				public static LocString NAME = "Slip";

				// Token: 0x0400CA44 RID: 51780
				public static LocString STATUS = "Slipping";

				// Token: 0x0400CA45 RID: 51781
				public static LocString TOOLTIP = "Slippery surfaces can cause Duplicants to fall \"seat over tea kettle\"";
			}

			// Token: 0x02003036 RID: 12342
			public class CHECKPOINT
			{
				// Token: 0x0400CA46 RID: 51782
				public static LocString NAME = "Wait at Checkpoint";

				// Token: 0x0400CA47 RID: 51783
				public static LocString STATUS = "Waiting at Checkpoint";

				// Token: 0x0400CA48 RID: 51784
				public static LocString TOOLTIP = "This Duplicant is waiting for permission to pass";
			}

			// Token: 0x02003037 RID: 12343
			public class TRAVELTUBEENTRANCE
			{
				// Token: 0x0400CA49 RID: 51785
				public static LocString NAME = "Enter Transit Tube";

				// Token: 0x0400CA4A RID: 51786
				public static LocString STATUS = "Entering Transit Tube";

				// Token: 0x0400CA4B RID: 51787
				public static LocString TOOLTIP = "Nyoooom!";
			}

			// Token: 0x02003038 RID: 12344
			public class SCRUBORE
			{
				// Token: 0x0400CA4C RID: 51788
				public static LocString NAME = "Scrub Ore";

				// Token: 0x0400CA4D RID: 51789
				public static LocString STATUS = "Scrubbing ore";

				// Token: 0x0400CA4E RID: 51790
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material ore can be scrubbed to remove ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" present on its surface"
				});
			}

			// Token: 0x02003039 RID: 12345
			public class EAT
			{
				// Token: 0x0400CA4F RID: 51791
				public static LocString NAME = "Eat";

				// Token: 0x0400CA50 RID: 51792
				public static LocString STATUS = "Going to eat";

				// Token: 0x0400CA51 RID: 51793
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants eat to replenish their ",
					UI.PRE_KEYWORD,
					"Calorie",
					UI.PST_KEYWORD,
					" stores"
				});
			}

			// Token: 0x0200303A RID: 12346
			public class RELOADELECTROBANK
			{
				// Token: 0x0400CA52 RID: 51794
				public static LocString NAME = "Power Up";

				// Token: 0x0400CA53 RID: 51795
				public static LocString STATUS = "Looking for power banks";

				// Token: 0x0400CA54 RID: 51796
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants need ",
					UI.PRE_KEYWORD,
					"Power Banks",
					UI.PST_KEYWORD,
					" to function"
				});
			}

			// Token: 0x0200303B RID: 12347
			public class FINDOXYGENSOURCEITEM
			{
				// Token: 0x0400CA55 RID: 51797
				public static LocString NAME = "Seek Oxygen Refill";

				// Token: 0x0400CA56 RID: 51798
				public static LocString STATUS = "Looking for oxygen refills";

				// Token: 0x0400CA57 RID: 51799
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants are fitted with internal ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" tanks that must be refilled"
				});
			}

			// Token: 0x0200303C RID: 12348
			public class BIONICABSORBOXYGEN
			{
				// Token: 0x0400CA58 RID: 51800
				public static LocString NAME = "Refill Oxygen Tank";

				// Token: 0x0400CA59 RID: 51801
				public static LocString STATUS = "Refilling oxygen tank";

				// Token: 0x0400CA5A RID: 51802
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants automatically refill their internal ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" tanks when levels get too low"
				});
			}

			// Token: 0x0200303D RID: 12349
			public class UNLOADELECTROBANK
			{
				// Token: 0x0400CA5B RID: 51803
				public static LocString NAME = "Offload";

				// Token: 0x0400CA5C RID: 51804
				public static LocString STATUS = "Offloading empty power banks";

				// Token: 0x0400CA5D RID: 51805
				public static LocString TOOLTIP = "Bionic Duplicants automatically offload depleted " + UI.PRE_KEYWORD + "Power Banks" + UI.PST_KEYWORD;
			}

			// Token: 0x0200303E RID: 12350
			public class SEEKANDINSTALLUPGRADE
			{
				// Token: 0x0400CA5E RID: 51806
				public static LocString NAME = "Retrieve Booster";

				// Token: 0x0400CA5F RID: 51807
				public static LocString STATUS = "Retrieving booster";

				// Token: 0x0400CA60 RID: 51808
				public static LocString TOOLTIP = "This Duplicant is on its way to retrieve a booster that was assigned to them";
			}

			// Token: 0x0200303F RID: 12351
			public class VOMIT
			{
				// Token: 0x0400CA61 RID: 51809
				public static LocString NAME = "Vomit";

				// Token: 0x0400CA62 RID: 51810
				public static LocString STATUS = "Vomiting";

				// Token: 0x0400CA63 RID: 51811
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Vomiting produces ",
					ELEMENTS.DIRTYWATER.NAME,
					" and can spread ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003040 RID: 12352
			public class RADIATIONPAIN
			{
				// Token: 0x0400CA64 RID: 51812
				public static LocString NAME = "Radiation Aches";

				// Token: 0x0400CA65 RID: 51813
				public static LocString STATUS = "Feeling radiation aches";

				// Token: 0x0400CA66 RID: 51814
				public static LocString TOOLTIP = "Radiation Aches are a symptom of " + DUPLICANTS.DISEASES.RADIATIONSICKNESS.NAME;
			}

			// Token: 0x02003041 RID: 12353
			public class COUGH
			{
				// Token: 0x0400CA67 RID: 51815
				public static LocString NAME = "Cough";

				// Token: 0x0400CA68 RID: 51816
				public static LocString STATUS = "Coughing";

				// Token: 0x0400CA69 RID: 51817
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Coughing is a symptom of ",
					DUPLICANTS.DISEASES.SLIMESICKNESS.NAME,
					" and spreads airborne ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003042 RID: 12354
			public class WATERDAMAGEZAP
			{
				// Token: 0x0400CA6A RID: 51818
				public static LocString NAME = "Glitch";

				// Token: 0x0400CA6B RID: 51819
				public static LocString STATUS = "Glitching";

				// Token: 0x0400CA6C RID: 51820
				public static LocString TOOLTIP = "Glitching is a symptom of Bionic Duplicant systems malfunctioning due to contact with incompatible " + UI.PRE_KEYWORD + "Liquids" + UI.PST_KEYWORD;
			}

			// Token: 0x02003043 RID: 12355
			public class SLEEP
			{
				// Token: 0x0400CA6D RID: 51821
				public static LocString NAME = "Sleep";

				// Token: 0x0400CA6E RID: 51822
				public static LocString STATUS = "Sleeping";

				// Token: 0x0400CA6F RID: 51823
				public static LocString TOOLTIP = "Zzzzzz...";
			}

			// Token: 0x02003044 RID: 12356
			public class NARCOLEPSY
			{
				// Token: 0x0400CA70 RID: 51824
				public static LocString NAME = "Narcoleptic Nap";

				// Token: 0x0400CA71 RID: 51825
				public static LocString STATUS = "Narcoleptic napping";

				// Token: 0x0400CA72 RID: 51826
				public static LocString TOOLTIP = "Zzzzzz...";
			}

			// Token: 0x02003045 RID: 12357
			public class FLOORSLEEP
			{
				// Token: 0x0400CA73 RID: 51827
				public static LocString NAME = "Sleep on Floor";

				// Token: 0x0400CA74 RID: 51828
				public static LocString STATUS = "Sleeping on floor";

				// Token: 0x0400CA75 RID: 51829
				public static LocString TOOLTIP = "Zzzzzz...\n\nSleeping on the floor will give Duplicants a " + DUPLICANTS.MODIFIERS.SOREBACK.NAME;
			}

			// Token: 0x02003046 RID: 12358
			public class TAKEMEDICINE
			{
				// Token: 0x0400CA76 RID: 51830
				public static LocString NAME = "Take Medicine";

				// Token: 0x0400CA77 RID: 51831
				public static LocString STATUS = "Taking medicine";

				// Token: 0x0400CA78 RID: 51832
				public static LocString TOOLTIP = "This Duplicant is taking a dose of medicine to ward off " + UI.PRE_KEYWORD + "Disease" + UI.PST_KEYWORD;
			}

			// Token: 0x02003047 RID: 12359
			public class GETDOCTORED
			{
				// Token: 0x0400CA79 RID: 51833
				public static LocString NAME = "Visit Doctor";

				// Token: 0x0400CA7A RID: 51834
				public static LocString STATUS = "Visiting doctor";

				// Token: 0x0400CA7B RID: 51835
				public static LocString TOOLTIP = "This Duplicant is visiting a doctor to receive treatment";
			}

			// Token: 0x02003048 RID: 12360
			public class DOCTOR
			{
				// Token: 0x0400CA7C RID: 51836
				public static LocString NAME = "Treat Patient";

				// Token: 0x0400CA7D RID: 51837
				public static LocString STATUS = "Treating patient";

				// Token: 0x0400CA7E RID: 51838
				public static LocString TOOLTIP = "This Duplicant is trying to make one of their peers feel better";
			}

			// Token: 0x02003049 RID: 12361
			public class DELIVERFOOD
			{
				// Token: 0x0400CA7F RID: 51839
				public static LocString NAME = "Deliver Food";

				// Token: 0x0400CA80 RID: 51840
				public static LocString STATUS = "Delivering food";

				// Token: 0x0400CA81 RID: 51841
				public static LocString TOOLTIP = "Under thirty minutes or it's free";
			}

			// Token: 0x0200304A RID: 12362
			public class SHOWER
			{
				// Token: 0x0400CA82 RID: 51842
				public static LocString NAME = "Shower";

				// Token: 0x0400CA83 RID: 51843
				public static LocString STATUS = "Showering";

				// Token: 0x0400CA84 RID: 51844
				public static LocString TOOLTIP = "This Duplicant is having a refreshing shower";
			}

			// Token: 0x0200304B RID: 12363
			public class SIGH
			{
				// Token: 0x0400CA85 RID: 51845
				public static LocString NAME = "Sigh";

				// Token: 0x0400CA86 RID: 51846
				public static LocString STATUS = "Sighing";

				// Token: 0x0400CA87 RID: 51847
				public static LocString TOOLTIP = "Ho-hum.";
			}

			// Token: 0x0200304C RID: 12364
			public class RESTDUETODISEASE
			{
				// Token: 0x0400CA88 RID: 51848
				public static LocString NAME = "Rest";

				// Token: 0x0400CA89 RID: 51849
				public static LocString STATUS = "Resting";

				// Token: 0x0400CA8A RID: 51850
				public static LocString TOOLTIP = "This Duplicant isn't feeling well and is taking a rest";
			}

			// Token: 0x0200304D RID: 12365
			public class HEAL
			{
				// Token: 0x0400CA8B RID: 51851
				public static LocString NAME = "Heal";

				// Token: 0x0400CA8C RID: 51852
				public static LocString STATUS = "Healing";

				// Token: 0x0400CA8D RID: 51853
				public static LocString TOOLTIP = "This Duplicant is taking some time to recover from their wounds";
			}

			// Token: 0x0200304E RID: 12366
			public class STRESSACTINGOUT
			{
				// Token: 0x0400CA8E RID: 51854
				public static LocString NAME = "Lash Out";

				// Token: 0x0400CA8F RID: 51855
				public static LocString STATUS = "Lashing out";

				// Token: 0x0400CA90 RID: 51856
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is having a ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					"-induced tantrum"
				});
			}

			// Token: 0x0200304F RID: 12367
			public class RELAX
			{
				// Token: 0x0400CA91 RID: 51857
				public static LocString NAME = "Relax";

				// Token: 0x0400CA92 RID: 51858
				public static LocString STATUS = "Relaxing";

				// Token: 0x0400CA93 RID: 51859
				public static LocString TOOLTIP = "This Duplicant is taking it easy";
			}

			// Token: 0x02003050 RID: 12368
			public class STRESSHEAL
			{
				// Token: 0x0400CA94 RID: 51860
				public static LocString NAME = "De-Stress";

				// Token: 0x0400CA95 RID: 51861
				public static LocString STATUS = "De-stressing";

				// Token: 0x0400CA96 RID: 51862
				public static LocString TOOLTIP = "This Duplicant taking some time to recover from their " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02003051 RID: 12369
			public class EQUIP
			{
				// Token: 0x0400CA97 RID: 51863
				public static LocString NAME = "Equip";

				// Token: 0x0400CA98 RID: 51864
				public static LocString STATUS = "Moving to equip";

				// Token: 0x0400CA99 RID: 51865
				public static LocString TOOLTIP = "This Duplicant is putting on a piece of equipment";
			}

			// Token: 0x02003052 RID: 12370
			public class LEARNSKILL
			{
				// Token: 0x0400CA9A RID: 51866
				public static LocString NAME = "Learn Skill";

				// Token: 0x0400CA9B RID: 51867
				public static LocString STATUS = "Learning skill";

				// Token: 0x0400CA9C RID: 51868
				public static LocString TOOLTIP = "This Duplicant is learning a new " + UI.PRE_KEYWORD + "Skill" + UI.PST_KEYWORD;
			}

			// Token: 0x02003053 RID: 12371
			public class UNLEARNSKILL
			{
				// Token: 0x0400CA9D RID: 51869
				public static LocString NAME = "Unlearn Skills";

				// Token: 0x0400CA9E RID: 51870
				public static LocString STATUS = "Unlearning skills";

				// Token: 0x0400CA9F RID: 51871
				public static LocString TOOLTIP = "This Duplicant is unlearning " + UI.PRE_KEYWORD + "Skills" + UI.PST_KEYWORD;
			}

			// Token: 0x02003054 RID: 12372
			public class RECHARGE
			{
				// Token: 0x0400CAA0 RID: 51872
				public static LocString NAME = "Recharge Equipment";

				// Token: 0x0400CAA1 RID: 51873
				public static LocString STATUS = "Recharging equipment";

				// Token: 0x0400CAA2 RID: 51874
				public static LocString TOOLTIP = "This Duplicant is recharging their equipment";
			}

			// Token: 0x02003055 RID: 12373
			public class UNEQUIP
			{
				// Token: 0x0400CAA3 RID: 51875
				public static LocString NAME = "Unequip";

				// Token: 0x0400CAA4 RID: 51876
				public static LocString STATUS = "Moving to unequip";

				// Token: 0x0400CAA5 RID: 51877
				public static LocString TOOLTIP = "This Duplicant is removing a piece of their equipment";
			}

			// Token: 0x02003056 RID: 12374
			public class MOURN
			{
				// Token: 0x0400CAA6 RID: 51878
				public static LocString NAME = "Mourn";

				// Token: 0x0400CAA7 RID: 51879
				public static LocString STATUS = "Mourning";

				// Token: 0x0400CAA8 RID: 51880
				public static LocString TOOLTIP = "This Duplicant is mourning the loss of a friend";
			}

			// Token: 0x02003057 RID: 12375
			public class WARMUP
			{
				// Token: 0x0400CAA9 RID: 51881
				public static LocString NAME = "Warm Up";

				// Token: 0x0400CAAA RID: 51882
				public static LocString STATUS = "Going to warm up";

				// Token: 0x0400CAAB RID: 51883
				public static LocString TOOLTIP = "This Duplicant got too cold and is going somewhere to warm up";
			}

			// Token: 0x02003058 RID: 12376
			public class COOLDOWN
			{
				// Token: 0x0400CAAC RID: 51884
				public static LocString NAME = "Cool Off";

				// Token: 0x0400CAAD RID: 51885
				public static LocString STATUS = "Going to cool off";

				// Token: 0x0400CAAE RID: 51886
				public static LocString TOOLTIP = "This Duplicant got too hot and is going somewhere to cool off";
			}

			// Token: 0x02003059 RID: 12377
			public class EMPTYSTORAGE
			{
				// Token: 0x0400CAAF RID: 51887
				public static LocString NAME = "Empty Storage";

				// Token: 0x0400CAB0 RID: 51888
				public static LocString STATUS = "Going to empty storage";

				// Token: 0x0400CAB1 RID: 51889
				public static LocString TOOLTIP = "This Duplicant is taking items out of storage";
			}

			// Token: 0x0200305A RID: 12378
			public class ART
			{
				// Token: 0x0400CAB2 RID: 51890
				public static LocString NAME = "Decorate";

				// Token: 0x0400CAB3 RID: 51891
				public static LocString STATUS = "Going to decorate";

				// Token: 0x0400CAB4 RID: 51892
				public static LocString TOOLTIP = "This Duplicant is going to work on their art";
			}

			// Token: 0x0200305B RID: 12379
			public class MOP
			{
				// Token: 0x0400CAB5 RID: 51893
				public static LocString NAME = "Mop";

				// Token: 0x0400CAB6 RID: 51894
				public static LocString STATUS = "Going to mop";

				// Token: 0x0400CAB7 RID: 51895
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Mopping removes ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" from the floor and bottles them for transport"
				});
			}

			// Token: 0x0200305C RID: 12380
			public class RELOCATE
			{
				// Token: 0x0400CAB8 RID: 51896
				public static LocString NAME = "Relocate";

				// Token: 0x0400CAB9 RID: 51897
				public static LocString STATUS = "Going to relocate";

				// Token: 0x0400CABA RID: 51898
				public static LocString TOOLTIP = "This Duplicant is moving a building to a new location";
			}

			// Token: 0x0200305D RID: 12381
			public class TOGGLE
			{
				// Token: 0x0400CABB RID: 51899
				public static LocString NAME = "Change Setting";

				// Token: 0x0400CABC RID: 51900
				public static LocString STATUS = "Going to change setting";

				// Token: 0x0400CABD RID: 51901
				public static LocString TOOLTIP = "This Duplicant is going to change the settings on a building";
			}

			// Token: 0x0200305E RID: 12382
			public class RESCUEINCAPACITATED
			{
				// Token: 0x0400CABE RID: 51902
				public static LocString NAME = "Rescue Friend";

				// Token: 0x0400CABF RID: 51903
				public static LocString STATUS = "Rescuing friend";

				// Token: 0x0400CAC0 RID: 51904
				public static LocString TOOLTIP = "This Duplicant is rescuing another Duplicant that has been incapacitated";
			}

			// Token: 0x0200305F RID: 12383
			public class REPAIR
			{
				// Token: 0x0400CAC1 RID: 51905
				public static LocString NAME = "Repair";

				// Token: 0x0400CAC2 RID: 51906
				public static LocString STATUS = "Going to repair";

				// Token: 0x0400CAC3 RID: 51907
				public static LocString TOOLTIP = "This Duplicant is fixing a broken building";
			}

			// Token: 0x02003060 RID: 12384
			public class DECONSTRUCT
			{
				// Token: 0x0400CAC4 RID: 51908
				public static LocString NAME = "Deconstruct";

				// Token: 0x0400CAC5 RID: 51909
				public static LocString STATUS = "Going to deconstruct";

				// Token: 0x0400CAC6 RID: 51910
				public static LocString TOOLTIP = "This Duplicant is deconstructing a building";
			}

			// Token: 0x02003061 RID: 12385
			public class RESEARCH
			{
				// Token: 0x0400CAC7 RID: 51911
				public static LocString NAME = "Research";

				// Token: 0x0400CAC8 RID: 51912
				public static LocString STATUS = "Going to research";

				// Token: 0x0400CAC9 RID: 51913
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is working on the current ",
					UI.PRE_KEYWORD,
					"Research",
					UI.PST_KEYWORD,
					" focus"
				});
			}

			// Token: 0x02003062 RID: 12386
			public class ANALYZEARTIFACT
			{
				// Token: 0x0400CACA RID: 51914
				public static LocString NAME = "Artifact Analysis";

				// Token: 0x0400CACB RID: 51915
				public static LocString STATUS = "Going to analyze artifacts";

				// Token: 0x0400CACC RID: 51916
				public static LocString TOOLTIP = "This Duplicant is analyzing " + UI.PRE_KEYWORD + "Artifacts" + UI.PST_KEYWORD;
			}

			// Token: 0x02003063 RID: 12387
			public class ANALYZESEED
			{
				// Token: 0x0400CACD RID: 51917
				public static LocString NAME = "Seed Analysis";

				// Token: 0x0400CACE RID: 51918
				public static LocString STATUS = "Going to analyze seeds";

				// Token: 0x0400CACF RID: 51919
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is analyzing ",
					UI.PRE_KEYWORD,
					"Seeds",
					UI.PST_KEYWORD,
					" to find mutations"
				});
			}

			// Token: 0x02003064 RID: 12388
			public class RETURNSUIT
			{
				// Token: 0x0400CAD0 RID: 51920
				public static LocString NAME = "Dock Exosuit";

				// Token: 0x0400CAD1 RID: 51921
				public static LocString STATUS = "Docking exosuit";

				// Token: 0x0400CAD2 RID: 51922
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is plugging an ",
					UI.PRE_KEYWORD,
					"Exosuit",
					UI.PST_KEYWORD,
					" in for refilling"
				});
			}

			// Token: 0x02003065 RID: 12389
			public class GENERATEPOWER
			{
				// Token: 0x0400CAD3 RID: 51923
				public static LocString NAME = "Generate Power";

				// Token: 0x0400CAD4 RID: 51924
				public static LocString STATUS = "Going to generate power";

				// Token: 0x0400CAD5 RID: 51925
				public static LocString TOOLTIP = "This Duplicant is producing electrical " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
			}

			// Token: 0x02003066 RID: 12390
			public class HARVEST
			{
				// Token: 0x0400CAD6 RID: 51926
				public static LocString NAME = "Harvest";

				// Token: 0x0400CAD7 RID: 51927
				public static LocString STATUS = "Going to harvest";

				// Token: 0x0400CAD8 RID: 51928
				public static LocString TOOLTIP = "This Duplicant is harvesting usable materials from a mature " + UI.PRE_KEYWORD + "Plant" + UI.PST_KEYWORD;
			}

			// Token: 0x02003067 RID: 12391
			public class UPROOT
			{
				// Token: 0x0400CAD9 RID: 51929
				public static LocString NAME = "Uproot";

				// Token: 0x0400CADA RID: 51930
				public static LocString STATUS = "Going to uproot";

				// Token: 0x0400CADB RID: 51931
				public static LocString TOOLTIP = "This Duplicant is uprooting a plant to retrieve a " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;
			}

			// Token: 0x02003068 RID: 12392
			public class CLEANTOILET
			{
				// Token: 0x0400CADC RID: 51932
				public static LocString NAME = "Clean Outhouse";

				// Token: 0x0400CADD RID: 51933
				public static LocString STATUS = "Going to clean";

				// Token: 0x0400CADE RID: 51934
				public static LocString TOOLTIP = "This Duplicant is cleaning out the " + BUILDINGS.PREFABS.OUTHOUSE.NAME;
			}

			// Token: 0x02003069 RID: 12393
			public class EMPTYDESALINATOR
			{
				// Token: 0x0400CADF RID: 51935
				public static LocString NAME = "Empty Desalinator";

				// Token: 0x0400CAE0 RID: 51936
				public static LocString STATUS = "Going to clean";

				// Token: 0x0400CAE1 RID: 51937
				public static LocString TOOLTIP = "This Duplicant is emptying out the " + BUILDINGS.PREFABS.DESALINATOR.NAME;
			}

			// Token: 0x0200306A RID: 12394
			public class LIQUIDCOOLEDFAN
			{
				// Token: 0x0400CAE2 RID: 51938
				public static LocString NAME = "Use Fan";

				// Token: 0x0400CAE3 RID: 51939
				public static LocString STATUS = "Going to use fan";

				// Token: 0x0400CAE4 RID: 51940
				public static LocString TOOLTIP = "This Duplicant is attempting to cool down the area";
			}

			// Token: 0x0200306B RID: 12395
			public class ICECOOLEDFAN
			{
				// Token: 0x0400CAE5 RID: 51941
				public static LocString NAME = "Use Fan";

				// Token: 0x0400CAE6 RID: 51942
				public static LocString STATUS = "Going to use fan";

				// Token: 0x0400CAE7 RID: 51943
				public static LocString TOOLTIP = "This Duplicant is attempting to cool down the area";
			}

			// Token: 0x0200306C RID: 12396
			public class PROCESSCRITTER
			{
				// Token: 0x0400CAE8 RID: 51944
				public static LocString NAME = "Process Critter";

				// Token: 0x0400CAE9 RID: 51945
				public static LocString STATUS = "Going to process critter";

				// Token: 0x0400CAEA RID: 51946
				public static LocString TOOLTIP = "This Duplicant is processing " + UI.PRE_KEYWORD + "Critters" + UI.PST_KEYWORD;
			}

			// Token: 0x0200306D RID: 12397
			public class COOK
			{
				// Token: 0x0400CAEB RID: 51947
				public static LocString NAME = "Cook";

				// Token: 0x0400CAEC RID: 51948
				public static LocString STATUS = "Going to cook";

				// Token: 0x0400CAED RID: 51949
				public static LocString TOOLTIP = "This Duplicant is cooking " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x0200306E RID: 12398
			public class COMPOUND
			{
				// Token: 0x0400CAEE RID: 51950
				public static LocString NAME = "Compound Medicine";

				// Token: 0x0400CAEF RID: 51951
				public static LocString STATUS = "Going to compound medicine";

				// Token: 0x0400CAF0 RID: 51952
				public static LocString TOOLTIP = "This Duplicant is fabricating " + UI.PRE_KEYWORD + "Medicine" + UI.PST_KEYWORD;
			}

			// Token: 0x0200306F RID: 12399
			public class TRAIN
			{
				// Token: 0x0400CAF1 RID: 51953
				public static LocString NAME = "Train";

				// Token: 0x0400CAF2 RID: 51954
				public static LocString STATUS = "Training";

				// Token: 0x0400CAF3 RID: 51955
				public static LocString TOOLTIP = "This Duplicant is busy training";
			}

			// Token: 0x02003070 RID: 12400
			public class MUSH
			{
				// Token: 0x0400CAF4 RID: 51956
				public static LocString NAME = "Mush";

				// Token: 0x0400CAF5 RID: 51957
				public static LocString STATUS = "Going to mush";

				// Token: 0x0400CAF6 RID: 51958
				public static LocString TOOLTIP = "This Duplicant is producing " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02003071 RID: 12401
			public class COMPOSTWORKABLE
			{
				// Token: 0x0400CAF7 RID: 51959
				public static LocString NAME = "Compost";

				// Token: 0x0400CAF8 RID: 51960
				public static LocString STATUS = "Going to compost";

				// Token: 0x0400CAF9 RID: 51961
				public static LocString TOOLTIP = "This Duplicant is dropping off organic material at the " + BUILDINGS.PREFABS.COMPOST.NAME;
			}

			// Token: 0x02003072 RID: 12402
			public class FLIPCOMPOST
			{
				// Token: 0x0400CAFA RID: 51962
				public static LocString NAME = "Flip";

				// Token: 0x0400CAFB RID: 51963
				public static LocString STATUS = "Going to flip compost";

				// Token: 0x0400CAFC RID: 51964
				public static LocString TOOLTIP = BUILDINGS.PREFABS.COMPOST.NAME + "s need to be flipped in order for their contents to compost";
			}

			// Token: 0x02003073 RID: 12403
			public class DEPRESSURIZE
			{
				// Token: 0x0400CAFD RID: 51965
				public static LocString NAME = "Depressurize Well";

				// Token: 0x0400CAFE RID: 51966
				public static LocString STATUS = "Going to depressurize well";

				// Token: 0x0400CAFF RID: 51967
				public static LocString TOOLTIP = BUILDINGS.PREFABS.OILWELLCAP.NAME + "s need to be periodically depressurized to function";
			}

			// Token: 0x02003074 RID: 12404
			public class FABRICATE
			{
				// Token: 0x0400CB00 RID: 51968
				public static LocString NAME = "Fabricate";

				// Token: 0x0400CB01 RID: 51969
				public static LocString STATUS = "Going to fabricate";

				// Token: 0x0400CB02 RID: 51970
				public static LocString TOOLTIP = "This Duplicant is crafting something";
			}

			// Token: 0x02003075 RID: 12405
			public class BUILD
			{
				// Token: 0x0400CB03 RID: 51971
				public static LocString NAME = "Build";

				// Token: 0x0400CB04 RID: 51972
				public static LocString STATUS = "Going to build";

				// Token: 0x0400CB05 RID: 51973
				public static LocString TOOLTIP = "This Duplicant is constructing a new building";
			}

			// Token: 0x02003076 RID: 12406
			public class BUILDDIG
			{
				// Token: 0x0400CB06 RID: 51974
				public static LocString NAME = "Construction Dig";

				// Token: 0x0400CB07 RID: 51975
				public static LocString STATUS = "Going to construction dig";

				// Token: 0x0400CB08 RID: 51976
				public static LocString TOOLTIP = "This Duplicant is making room for a planned construction task by performing this dig";
			}

			// Token: 0x02003077 RID: 12407
			public class DIG
			{
				// Token: 0x0400CB09 RID: 51977
				public static LocString NAME = "Dig";

				// Token: 0x0400CB0A RID: 51978
				public static LocString STATUS = "Going to dig";

				// Token: 0x0400CB0B RID: 51979
				public static LocString TOOLTIP = "This Duplicant is digging out a tile";
			}

			// Token: 0x02003078 RID: 12408
			public class FETCH
			{
				// Token: 0x0400CB0C RID: 51980
				public static LocString NAME = "Deliver";

				// Token: 0x0400CB0D RID: 51981
				public static LocString STATUS = "Delivering";

				// Token: 0x0400CB0E RID: 51982
				public static LocString TOOLTIP = "This Duplicant is delivering materials where they need to go";

				// Token: 0x0400CB0F RID: 51983
				public static LocString REPORT_NAME = "Deliver to {0}";
			}

			// Token: 0x02003079 RID: 12409
			public class JOYREACTION
			{
				// Token: 0x0400CB10 RID: 51984
				public static LocString NAME = "Joy Reaction";

				// Token: 0x0400CB11 RID: 51985
				public static LocString STATUS = "Overjoyed";

				// Token: 0x0400CB12 RID: 51986
				public static LocString TOOLTIP = "This Duplicant is taking a moment to relish in their own happiness";

				// Token: 0x0400CB13 RID: 51987
				public static LocString REPORT_NAME = "Overjoyed Reaction";
			}

			// Token: 0x0200307A RID: 12410
			public class ROCKETCONTROL
			{
				// Token: 0x0400CB14 RID: 51988
				public static LocString NAME = "Rocket Control";

				// Token: 0x0400CB15 RID: 51989
				public static LocString STATUS = "Controlling rocket";

				// Token: 0x0400CB16 RID: 51990
				public static LocString TOOLTIP = "This Duplicant is keeping their spacecraft on course";

				// Token: 0x0400CB17 RID: 51991
				public static LocString REPORT_NAME = "Rocket Control";
			}

			// Token: 0x0200307B RID: 12411
			public class STORAGEFETCH
			{
				// Token: 0x0400CB18 RID: 51992
				public static LocString NAME = "Store Materials";

				// Token: 0x0400CB19 RID: 51993
				public static LocString STATUS = "Storing materials";

				// Token: 0x0400CB1A RID: 51994
				public static LocString TOOLTIP = "This Duplicant is moving materials into storage for later use";

				// Token: 0x0400CB1B RID: 51995
				public static LocString REPORT_NAME = "Store {0}";
			}

			// Token: 0x0200307C RID: 12412
			public class EQUIPMENTFETCH
			{
				// Token: 0x0400CB1C RID: 51996
				public static LocString NAME = "Store Equipment";

				// Token: 0x0400CB1D RID: 51997
				public static LocString STATUS = "Storing equipment";

				// Token: 0x0400CB1E RID: 51998
				public static LocString TOOLTIP = "This Duplicant is transporting equipment for storage";

				// Token: 0x0400CB1F RID: 51999
				public static LocString REPORT_NAME = "Store {0}";
			}

			// Token: 0x0200307D RID: 12413
			public class REPAIRFETCH
			{
				// Token: 0x0400CB20 RID: 52000
				public static LocString NAME = "Repair Supply";

				// Token: 0x0400CB21 RID: 52001
				public static LocString STATUS = "Supplying repair materials";

				// Token: 0x0400CB22 RID: 52002
				public static LocString TOOLTIP = "This Duplicant is delivering materials to where they'll be needed to repair buildings";
			}

			// Token: 0x0200307E RID: 12414
			public class RESEARCHFETCH
			{
				// Token: 0x0400CB23 RID: 52003
				public static LocString NAME = "Research Supply";

				// Token: 0x0400CB24 RID: 52004
				public static LocString STATUS = "Supplying research materials";

				// Token: 0x0400CB25 RID: 52005
				public static LocString TOOLTIP = "This Duplicant is delivering materials where they'll be needed to conduct " + UI.PRE_KEYWORD + "Research" + UI.PST_KEYWORD;
			}

			// Token: 0x0200307F RID: 12415
			public class EXCAVATEFOSSIL
			{
				// Token: 0x0400CB26 RID: 52006
				public static LocString NAME = "Excavate Fossil";

				// Token: 0x0400CB27 RID: 52007
				public static LocString STATUS = "Excavating a fossil";

				// Token: 0x0400CB28 RID: 52008
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is excavating a ",
					UI.PRE_KEYWORD,
					"Fossil",
					UI.PST_KEYWORD,
					" site"
				});
			}

			// Token: 0x02003080 RID: 12416
			public class ARMTRAP
			{
				// Token: 0x0400CB29 RID: 52009
				public static LocString NAME = "Arm Trap";

				// Token: 0x0400CB2A RID: 52010
				public static LocString STATUS = "Arming a trap";

				// Token: 0x0400CB2B RID: 52011
				public static LocString TOOLTIP = "This Duplicant is arming a trap";
			}

			// Token: 0x02003081 RID: 12417
			public class FARMFETCH
			{
				// Token: 0x0400CB2C RID: 52012
				public static LocString NAME = "Farming Supply";

				// Token: 0x0400CB2D RID: 52013
				public static LocString STATUS = "Supplying farming materials";

				// Token: 0x0400CB2E RID: 52014
				public static LocString TOOLTIP = "This Duplicant is delivering farming materials where they're needed to tend " + UI.PRE_KEYWORD + "Crops" + UI.PST_KEYWORD;
			}

			// Token: 0x02003082 RID: 12418
			public class FETCHCRITICAL
			{
				// Token: 0x0400CB2F RID: 52015
				public static LocString NAME = "Life Support Supply";

				// Token: 0x0400CB30 RID: 52016
				public static LocString STATUS = "Supplying critical materials";

				// Token: 0x0400CB31 RID: 52017
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is delivering materials required to perform ",
					UI.PRE_KEYWORD,
					"Life Support",
					UI.PST_KEYWORD,
					" Errands"
				});

				// Token: 0x0400CB32 RID: 52018
				public static LocString REPORT_NAME = "Life Support Supply to {0}";
			}

			// Token: 0x02003083 RID: 12419
			public class MACHINEFETCH
			{
				// Token: 0x0400CB33 RID: 52019
				public static LocString NAME = "Operational Supply";

				// Token: 0x0400CB34 RID: 52020
				public static LocString STATUS = "Supplying operational materials";

				// Token: 0x0400CB35 RID: 52021
				public static LocString TOOLTIP = "This Duplicant is delivering materials to where they'll be needed for machine operation";

				// Token: 0x0400CB36 RID: 52022
				public static LocString REPORT_NAME = "Operational Supply to {0}";
			}

			// Token: 0x02003084 RID: 12420
			public class COOKFETCH
			{
				// Token: 0x0400CB37 RID: 52023
				public static LocString NAME = "Cook Supply";

				// Token: 0x0400CB38 RID: 52024
				public static LocString STATUS = "Supplying cook ingredients";

				// Token: 0x0400CB39 RID: 52025
				public static LocString TOOLTIP = "This Duplicant is delivering materials required to cook " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02003085 RID: 12421
			public class DOCTORFETCH
			{
				// Token: 0x0400CB3A RID: 52026
				public static LocString NAME = "Medical Supply";

				// Token: 0x0400CB3B RID: 52027
				public static LocString STATUS = "Supplying medical resources";

				// Token: 0x0400CB3C RID: 52028
				public static LocString TOOLTIP = "This Duplicant is delivering the materials that will be needed to treat sick patients";

				// Token: 0x0400CB3D RID: 52029
				public static LocString REPORT_NAME = "Medical Supply to {0}";
			}

			// Token: 0x02003086 RID: 12422
			public class FOODFETCH
			{
				// Token: 0x0400CB3E RID: 52030
				public static LocString NAME = "Store Food";

				// Token: 0x0400CB3F RID: 52031
				public static LocString STATUS = "Storing food";

				// Token: 0x0400CB40 RID: 52032
				public static LocString TOOLTIP = "This Duplicant is moving edible resources into proper storage";

				// Token: 0x0400CB41 RID: 52033
				public static LocString REPORT_NAME = "Store {0}";
			}

			// Token: 0x02003087 RID: 12423
			public class POWERFETCH
			{
				// Token: 0x0400CB42 RID: 52034
				public static LocString NAME = "Power Supply";

				// Token: 0x0400CB43 RID: 52035
				public static LocString STATUS = "Supplying power materials";

				// Token: 0x0400CB44 RID: 52036
				public static LocString TOOLTIP = "This Duplicant is delivering materials to where they'll be needed for " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;

				// Token: 0x0400CB45 RID: 52037
				public static LocString REPORT_NAME = "Power Supply to {0}";
			}

			// Token: 0x02003088 RID: 12424
			public class FABRICATEFETCH
			{
				// Token: 0x0400CB46 RID: 52038
				public static LocString NAME = "Fabrication Supply";

				// Token: 0x0400CB47 RID: 52039
				public static LocString STATUS = "Supplying fabrication materials";

				// Token: 0x0400CB48 RID: 52040
				public static LocString TOOLTIP = "This Duplicant is delivering materials required to fabricate new objects";

				// Token: 0x0400CB49 RID: 52041
				public static LocString REPORT_NAME = "Fabrication Supply to {0}";
			}

			// Token: 0x02003089 RID: 12425
			public class BUILDFETCH
			{
				// Token: 0x0400CB4A RID: 52042
				public static LocString NAME = "Construction Supply";

				// Token: 0x0400CB4B RID: 52043
				public static LocString STATUS = "Supplying construction materials";

				// Token: 0x0400CB4C RID: 52044
				public static LocString TOOLTIP = "This delivery will provide materials to a planned construction site";
			}

			// Token: 0x0200308A RID: 12426
			public class FETCHCREATURE
			{
				// Token: 0x0400CB4D RID: 52045
				public static LocString NAME = "Relocate Critter";

				// Token: 0x0400CB4E RID: 52046
				public static LocString STATUS = "Relocating critter";

				// Token: 0x0400CB4F RID: 52047
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is moving a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" to a new location"
				});
			}

			// Token: 0x0200308B RID: 12427
			public class FETCHRANCHING
			{
				// Token: 0x0400CB50 RID: 52048
				public static LocString NAME = "Ranching Supply";

				// Token: 0x0400CB51 RID: 52049
				public static LocString STATUS = "Supplying ranching materials";

				// Token: 0x0400CB52 RID: 52050
				public static LocString TOOLTIP = "This Duplicant is delivering materials for ranching activities";
			}

			// Token: 0x0200308C RID: 12428
			public class TRANSPORT
			{
				// Token: 0x0400CB53 RID: 52051
				public static LocString NAME = "Sweep";

				// Token: 0x0400CB54 RID: 52052
				public static LocString STATUS = "Going to sweep";

				// Token: 0x0400CB55 RID: 52053
				public static LocString TOOLTIP = "Moving debris off the ground and into storage improves colony " + UI.PRE_KEYWORD + "Decor" + UI.PST_KEYWORD;
			}

			// Token: 0x0200308D RID: 12429
			public class MOVETOSAFETY
			{
				// Token: 0x0400CB56 RID: 52054
				public static LocString NAME = "Find Safe Area";

				// Token: 0x0400CB57 RID: 52055
				public static LocString STATUS = "Finding safer area";

				// Token: 0x0400CB58 RID: 52056
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is ",
					UI.PRE_KEYWORD,
					"Idle",
					UI.PST_KEYWORD,
					" and looking for somewhere safe and comfy to chill"
				});
			}

			// Token: 0x0200308E RID: 12430
			public class PARTY
			{
				// Token: 0x0400CB59 RID: 52057
				public static LocString NAME = "Party";

				// Token: 0x0400CB5A RID: 52058
				public static LocString STATUS = "Partying";

				// Token: 0x0400CB5B RID: 52059
				public static LocString TOOLTIP = "This Duplicant is partying hard";
			}

			// Token: 0x0200308F RID: 12431
			public class REMOTEWORK
			{
				// Token: 0x0400CB5C RID: 52060
				public static LocString NAME = "Remote Work";

				// Token: 0x0400CB5D RID: 52061
				public static LocString STATUS = "Working remotely";

				// Token: 0x0400CB5E RID: 52062
				public static LocString TOOLTIP = "This Duplicant's body is here, but their work is elsewhere";
			}

			// Token: 0x02003090 RID: 12432
			public class POWER_TINKER
			{
				// Token: 0x0400CB5F RID: 52063
				public static LocString NAME = "Tinker";

				// Token: 0x0400CB60 RID: 52064
				public static LocString STATUS = "Tinkering";

				// Token: 0x0400CB61 RID: 52065
				public static LocString TOOLTIP = "Tinkering with buildings improves their functionality";
			}

			// Token: 0x02003091 RID: 12433
			public class RANCH
			{
				// Token: 0x0400CB62 RID: 52066
				public static LocString NAME = "Ranch";

				// Token: 0x0400CB63 RID: 52067
				public static LocString STATUS = "Ranching";

				// Token: 0x0400CB64 RID: 52068
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is tending to a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"'s well-being"
				});

				// Token: 0x0400CB65 RID: 52069
				public static LocString REPORT_NAME = "Deliver to {0}";
			}

			// Token: 0x02003092 RID: 12434
			public class CROP_TEND
			{
				// Token: 0x0400CB66 RID: 52070
				public static LocString NAME = "Tend";

				// Token: 0x0400CB67 RID: 52071
				public static LocString STATUS = "Tending plant";

				// Token: 0x0400CB68 RID: 52072
				public static LocString TOOLTIP = "Tending to plants increases their " + UI.PRE_KEYWORD + "Growth Rate" + UI.PST_KEYWORD;
			}

			// Token: 0x02003093 RID: 12435
			public class DEMOLISH
			{
				// Token: 0x0400CB69 RID: 52073
				public static LocString NAME = "Demolish";

				// Token: 0x0400CB6A RID: 52074
				public static LocString STATUS = "Demolishing object";

				// Token: 0x0400CB6B RID: 52075
				public static LocString TOOLTIP = "Demolishing an object removes it permanently";
			}

			// Token: 0x02003094 RID: 12436
			public class IDLE
			{
				// Token: 0x0400CB6C RID: 52076
				public static LocString NAME = "Idle";

				// Token: 0x0400CB6D RID: 52077
				public static LocString STATUS = "Idle";

				// Token: 0x0400CB6E RID: 52078
				public static LocString TOOLTIP = "This Duplicant cannot reach any pending " + UI.PRE_KEYWORD + "Errands" + UI.PST_KEYWORD;
			}

			// Token: 0x02003095 RID: 12437
			public class PRECONDITIONS
			{
				// Token: 0x0400CB6F RID: 52079
				public static LocString HEADER = "The selected {Selected} could:";

				// Token: 0x0400CB70 RID: 52080
				public static LocString SUCCESS_ROW = "{Duplicant} -- {Rank}";

				// Token: 0x0400CB71 RID: 52081
				public static LocString CURRENT_ERRAND = "Current Errand";

				// Token: 0x0400CB72 RID: 52082
				public static LocString RANK_FORMAT = "#{0}";

				// Token: 0x0400CB73 RID: 52083
				public static LocString FAILURE_ROW = "{Duplicant} -- {Reason}";

				// Token: 0x0400CB74 RID: 52084
				public static LocString CONTAINS_OXYGEN = "Not enough Oxygen";

				// Token: 0x0400CB75 RID: 52085
				public static LocString IS_PREEMPTABLE = "Already assigned to {Assignee}";

				// Token: 0x0400CB76 RID: 52086
				public static LocString HAS_URGE = "No current need";

				// Token: 0x0400CB77 RID: 52087
				public static LocString IS_VALID = "Invalid";

				// Token: 0x0400CB78 RID: 52088
				public static LocString IS_PERMITTED = "Not permitted";

				// Token: 0x0400CB79 RID: 52089
				public static LocString IS_ASSIGNED_TO_ME = "Not assigned to {Selected}";

				// Token: 0x0400CB7A RID: 52090
				public static LocString IS_IN_MY_WORLD = "Outside world";

				// Token: 0x0400CB7B RID: 52091
				public static LocString IS_CELL_NOT_IN_MY_WORLD = "Already there";

				// Token: 0x0400CB7C RID: 52092
				public static LocString IS_IN_MY_ROOM = "Outside {Selected}'s room";

				// Token: 0x0400CB7D RID: 52093
				public static LocString IS_PREFERRED_ASSIGNABLE = "Not preferred assignment";

				// Token: 0x0400CB7E RID: 52094
				public static LocString IS_PREFERRED_ASSIGNABLE_OR_URGENT_BLADDER = "Not preferred assignment";

				// Token: 0x0400CB7F RID: 52095
				public static LocString HAS_SKILL_PERK = "Requires learned skill";

				// Token: 0x0400CB80 RID: 52096
				public static LocString IS_MORE_SATISFYING = "Low priority";

				// Token: 0x0400CB81 RID: 52097
				public static LocString CAN_CHAT = "Unreachable";

				// Token: 0x0400CB82 RID: 52098
				public static LocString IS_NOT_RED_ALERT = "Unavailable in Red Alert";

				// Token: 0x0400CB83 RID: 52099
				public static LocString NO_DEAD_BODIES = "Unburied Duplicant";

				// Token: 0x0400CB84 RID: 52100
				public static LocString NOT_A_ROBOT = "Unavailable to Robots";

				// Token: 0x0400CB85 RID: 52101
				public static LocString NOT_A_BIONIC = "Unavailable to Bionic Duplicants";

				// Token: 0x0400CB86 RID: 52102
				public static LocString VALID_MOURNING_SITE = "Nowhere to mourn";

				// Token: 0x0400CB87 RID: 52103
				public static LocString HAS_PLACE_TO_STAND = "Nowhere to stand";

				// Token: 0x0400CB88 RID: 52104
				public static LocString IS_SCHEDULED_TIME = "Not allowed by schedule";

				// Token: 0x0400CB89 RID: 52105
				public static LocString CAN_MOVE_TO = "Unreachable";

				// Token: 0x0400CB8A RID: 52106
				public static LocString CAN_PICKUP = "Cannot pickup";

				// Token: 0x0400CB8B RID: 52107
				public static LocString IS_AWAKE = "{Selected} is sleeping";

				// Token: 0x0400CB8C RID: 52108
				public static LocString IS_STANDING = "{Selected} must stand";

				// Token: 0x0400CB8D RID: 52109
				public static LocString IS_MOVING = "{Selected} is not moving";

				// Token: 0x0400CB8E RID: 52110
				public static LocString IS_OFF_LADDER = "{Selected} is busy climbing";

				// Token: 0x0400CB8F RID: 52111
				public static LocString NOT_IN_TUBE = "{Selected} is busy in transit";

				// Token: 0x0400CB90 RID: 52112
				public static LocString HAS_TRAIT = "Missing required trait";

				// Token: 0x0400CB91 RID: 52113
				public static LocString IS_OPERATIONAL = "Not operational";

				// Token: 0x0400CB92 RID: 52114
				public static LocString IS_MARKED_FOR_DECONSTRUCTION = "Being deconstructed";

				// Token: 0x0400CB93 RID: 52115
				public static LocString IS_NOT_BURROWED = "Is not burrowed";

				// Token: 0x0400CB94 RID: 52116
				public static LocString IS_CREATURE_AVAILABLE_FOR_RANCHING = "No Critters Available";

				// Token: 0x0400CB95 RID: 52117
				public static LocString IS_CREATURE_AVAILABLE_FOR_FIXED_CAPTURE = "Pen Status OK";

				// Token: 0x0400CB96 RID: 52118
				public static LocString IS_MARKED_FOR_DISABLE = "Building Disabled";

				// Token: 0x0400CB97 RID: 52119
				public static LocString IS_FUNCTIONAL = "Not functioning";

				// Token: 0x0400CB98 RID: 52120
				public static LocString IS_OVERRIDE_TARGET_NULL_OR_ME = "DebugIsOverrideTargetNullOrMe";

				// Token: 0x0400CB99 RID: 52121
				public static LocString NOT_CHORE_CREATOR = "DebugNotChoreCreator";

				// Token: 0x0400CB9A RID: 52122
				public static LocString IS_GETTING_MORE_STRESSED = "{Selected}'s stress is decreasing";

				// Token: 0x0400CB9B RID: 52123
				public static LocString IS_ALLOWED_BY_AUTOMATION = "Automated";

				// Token: 0x0400CB9C RID: 52124
				public static LocString CAN_DO_RECREATION = "Not Interested";

				// Token: 0x0400CB9D RID: 52125
				public static LocString DOES_SUIT_NEED_RECHARGING_IDLE = "Suit is currently charged";

				// Token: 0x0400CB9E RID: 52126
				public static LocString DOES_SUIT_NEED_RECHARGING_URGENT = "Suit is currently charged";

				// Token: 0x0400CB9F RID: 52127
				public static LocString HAS_SUIT_MARKER = "No Suit Checkpoint";

				// Token: 0x0400CBA0 RID: 52128
				public static LocString ALLOWED_TO_DEPRESSURIZE = "Not currently overpressure";

				// Token: 0x0400CBA1 RID: 52129
				public static LocString IS_STRESS_ABOVE_ACTIVATION_RANGE = "{Selected} is not stressed right now";

				// Token: 0x0400CBA2 RID: 52130
				public static LocString IS_NOT_ANGRY = "{Selected} is too angry";

				// Token: 0x0400CBA3 RID: 52131
				public static LocString IS_NOT_BEING_ATTACKED = "{Selected} is in combat";

				// Token: 0x0400CBA4 RID: 52132
				public static LocString IS_CONSUMPTION_PERMITTED = "Disallowed by consumable permissions";

				// Token: 0x0400CBA5 RID: 52133
				public static LocString CAN_CURE = "No applicable illness";

				// Token: 0x0400CBA6 RID: 52134
				public static LocString TREATMENT_AVAILABLE = "No treatable illness";

				// Token: 0x0400CBA7 RID: 52135
				public static LocString DOCTOR_AVAILABLE = "No doctors available\n(Duplicants cannot treat themselves)";

				// Token: 0x0400CBA8 RID: 52136
				public static LocString IS_OKAY_TIME_TO_SLEEP = "No current need";

				// Token: 0x0400CBA9 RID: 52137
				public static LocString IS_NARCOLEPSING = "{Selected} is currently napping";

				// Token: 0x0400CBAA RID: 52138
				public static LocString IS_FETCH_TARGET_AVAILABLE = "No pending deliveries";

				// Token: 0x0400CBAB RID: 52139
				public static LocString EDIBLE_IS_NOT_NULL = "Consumable Permission not allowed";

				// Token: 0x0400CBAC RID: 52140
				public static LocString HAS_MINGLE_CELL = "Nowhere to Mingle";

				// Token: 0x0400CBAD RID: 52141
				public static LocString EXCLUSIVELY_AVAILABLE = "Building Already Busy";

				// Token: 0x0400CBAE RID: 52142
				public static LocString BLADDER_FULL = "Bladder isn't full";

				// Token: 0x0400CBAF RID: 52143
				public static LocString BLADDER_NOT_FULL = "Bladder too full";

				// Token: 0x0400CBB0 RID: 52144
				public static LocString CURRENTLY_PEEING = "Currently Peeing";

				// Token: 0x0400CBB1 RID: 52145
				public static LocString HAS_BALLOON_STALL_CELL = "Has a location for a Balloon Stall";

				// Token: 0x0400CBB2 RID: 52146
				public static LocString IS_MINION = "Must be a Duplicant";

				// Token: 0x0400CBB3 RID: 52147
				public static LocString IS_ROCKET_TRAVELLING = "Rocket must be travelling";

				// Token: 0x0400CBB4 RID: 52148
				public static LocString REMOTE_CHORE_SUBCHORE_PRECONDITIONS = "No Eligible Remote Chores";

				// Token: 0x0400CBB5 RID: 52149
				public static LocString REMOTE_CHORE_NO_REMOTE_DOCK = "No Dock Assigned";

				// Token: 0x0400CBB6 RID: 52150
				public static LocString REMOTE_CHORE_DOCK_INOPERABLE = "Remote Worker Dock Unusable";

				// Token: 0x0400CBB7 RID: 52151
				public static LocString REMOTE_CHORE_NO_REMOTE_WORKER = "No Remote Worker at Dock";

				// Token: 0x0400CBB8 RID: 52152
				public static LocString REMOTE_CHORE_DOCK_UNAVAILABLE = "Remote Worker Already Busy";
			}
		}

		// Token: 0x020021B1 RID: 8625
		public class SKILLGROUPS
		{
			// Token: 0x02003096 RID: 12438
			public class MINING
			{
				// Token: 0x0400CBB9 RID: 52153
				public static LocString NAME = "Digger";
			}

			// Token: 0x02003097 RID: 12439
			public class BUILDING
			{
				// Token: 0x0400CBBA RID: 52154
				public static LocString NAME = "Builder";
			}

			// Token: 0x02003098 RID: 12440
			public class FARMING
			{
				// Token: 0x0400CBBB RID: 52155
				public static LocString NAME = "Farmer";
			}

			// Token: 0x02003099 RID: 12441
			public class RANCHING
			{
				// Token: 0x0400CBBC RID: 52156
				public static LocString NAME = "Rancher";
			}

			// Token: 0x0200309A RID: 12442
			public class COOKING
			{
				// Token: 0x0400CBBD RID: 52157
				public static LocString NAME = "Cooker";
			}

			// Token: 0x0200309B RID: 12443
			public class ART
			{
				// Token: 0x0400CBBE RID: 52158
				public static LocString NAME = "Decorator";
			}

			// Token: 0x0200309C RID: 12444
			public class RESEARCH
			{
				// Token: 0x0400CBBF RID: 52159
				public static LocString NAME = "Researcher";
			}

			// Token: 0x0200309D RID: 12445
			public class SUITS
			{
				// Token: 0x0400CBC0 RID: 52160
				public static LocString NAME = "Suit Wearer";
			}

			// Token: 0x0200309E RID: 12446
			public class HAULING
			{
				// Token: 0x0400CBC1 RID: 52161
				public static LocString NAME = "Supplier";
			}

			// Token: 0x0200309F RID: 12447
			public class TECHNICALS
			{
				// Token: 0x0400CBC2 RID: 52162
				public static LocString NAME = "Operator";
			}

			// Token: 0x020030A0 RID: 12448
			public class MEDICALAID
			{
				// Token: 0x0400CBC3 RID: 52163
				public static LocString NAME = "Doctor";
			}

			// Token: 0x020030A1 RID: 12449
			public class BASEKEEPING
			{
				// Token: 0x0400CBC4 RID: 52164
				public static LocString NAME = "Tidier";
			}

			// Token: 0x020030A2 RID: 12450
			public class ROCKETRY
			{
				// Token: 0x0400CBC5 RID: 52165
				public static LocString NAME = "Pilot";
			}
		}

		// Token: 0x020021B2 RID: 8626
		public class CHOREGROUPS
		{
			// Token: 0x020030A3 RID: 12451
			public class ART
			{
				// Token: 0x0400CBC6 RID: 52166
				public static LocString NAME = "Decorating";

				// Token: 0x0400CBC7 RID: 52167
				public static LocString DESC = string.Concat(new string[]
				{
					"Sculpt or paint to improve colony ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400CBC8 RID: 52168
				public static LocString ARCHETYPE_NAME = "Decorator";
			}

			// Token: 0x020030A4 RID: 12452
			public class COMBAT
			{
				// Token: 0x0400CBC9 RID: 52169
				public static LocString NAME = "Attacking";

				// Token: 0x0400CBCA RID: 52170
				public static LocString DESC = "Fight wild " + UI.FormatAsLink("Critters", "CREATURES") + ".";

				// Token: 0x0400CBCB RID: 52171
				public static LocString ARCHETYPE_NAME = "Attacker";
			}

			// Token: 0x020030A5 RID: 12453
			public class LIFESUPPORT
			{
				// Token: 0x0400CBCC RID: 52172
				public static LocString NAME = "Life Support";

				// Token: 0x0400CBCD RID: 52173
				public static LocString DESC = string.Concat(new string[]
				{
					"Maintain ",
					BUILDINGS.PREFABS.ALGAEHABITAT.NAME,
					"s, ",
					BUILDINGS.PREFABS.AIRFILTER.NAME,
					"s, and ",
					BUILDINGS.PREFABS.WATERPURIFIER.NAME,
					"s to support colony life."
				});

				// Token: 0x0400CBCE RID: 52174
				public static LocString ARCHETYPE_NAME = "Life Supporter";
			}

			// Token: 0x020030A6 RID: 12454
			public class TOGGLE
			{
				// Token: 0x0400CBCF RID: 52175
				public static LocString NAME = "Toggling";

				// Token: 0x0400CBD0 RID: 52176
				public static LocString DESC = "Enable or disable buildings, adjust building settings, and set or flip switches and sensors.";

				// Token: 0x0400CBD1 RID: 52177
				public static LocString ARCHETYPE_NAME = "Toggler";
			}

			// Token: 0x020030A7 RID: 12455
			public class COOK
			{
				// Token: 0x0400CBD2 RID: 52178
				public static LocString NAME = "Cooking";

				// Token: 0x0400CBD3 RID: 52179
				public static LocString DESC = string.Concat(new string[]
				{
					"Operate ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" preparation buildings."
				});

				// Token: 0x0400CBD4 RID: 52180
				public static LocString ARCHETYPE_NAME = "Cooker";
			}

			// Token: 0x020030A8 RID: 12456
			public class RESEARCH
			{
				// Token: 0x0400CBD5 RID: 52181
				public static LocString NAME = "Researching";

				// Token: 0x0400CBD6 RID: 52182
				public static LocString DESC = string.Concat(new string[]
				{
					"Use ",
					UI.PRE_KEYWORD,
					"Research Stations",
					UI.PST_KEYWORD,
					" to unlock new technologies."
				});

				// Token: 0x0400CBD7 RID: 52183
				public static LocString ARCHETYPE_NAME = "Researcher";
			}

			// Token: 0x020030A9 RID: 12457
			public class REPAIR
			{
				// Token: 0x0400CBD8 RID: 52184
				public static LocString NAME = "Repairing";

				// Token: 0x0400CBD9 RID: 52185
				public static LocString DESC = "Repair damaged buildings.";

				// Token: 0x0400CBDA RID: 52186
				public static LocString ARCHETYPE_NAME = "Repairer";
			}

			// Token: 0x020030AA RID: 12458
			public class FARMING
			{
				// Token: 0x0400CBDB RID: 52187
				public static LocString NAME = "Farming";

				// Token: 0x0400CBDC RID: 52188
				public static LocString DESC = string.Concat(new string[]
				{
					"Gather crops from mature ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400CBDD RID: 52189
				public static LocString ARCHETYPE_NAME = "Farmer";
			}

			// Token: 0x020030AB RID: 12459
			public class RANCHING
			{
				// Token: 0x0400CBDE RID: 52190
				public static LocString NAME = "Ranching";

				// Token: 0x0400CBDF RID: 52191
				public static LocString DESC = "Tend to domesticated " + UI.FormatAsLink("Critters", "CREATURES") + ".";

				// Token: 0x0400CBE0 RID: 52192
				public static LocString ARCHETYPE_NAME = "Rancher";
			}

			// Token: 0x020030AC RID: 12460
			public class BUILD
			{
				// Token: 0x0400CBE1 RID: 52193
				public static LocString NAME = "Building";

				// Token: 0x0400CBE2 RID: 52194
				public static LocString DESC = "Construct new buildings.";

				// Token: 0x0400CBE3 RID: 52195
				public static LocString ARCHETYPE_NAME = "Builder";
			}

			// Token: 0x020030AD RID: 12461
			public class HAULING
			{
				// Token: 0x0400CBE4 RID: 52196
				public static LocString NAME = "Supplying";

				// Token: 0x0400CBE5 RID: 52197
				public static LocString DESC = "Run resources to critical buildings and urgent storage.";

				// Token: 0x0400CBE6 RID: 52198
				public static LocString ARCHETYPE_NAME = "Supplier";
			}

			// Token: 0x020030AE RID: 12462
			public class STORAGE
			{
				// Token: 0x0400CBE7 RID: 52199
				public static LocString NAME = "Storing";

				// Token: 0x0400CBE8 RID: 52200
				public static LocString DESC = "Fill storage buildings with resources when no other errands are available.";

				// Token: 0x0400CBE9 RID: 52201
				public static LocString ARCHETYPE_NAME = "Storer";
			}

			// Token: 0x020030AF RID: 12463
			public class RECREATION
			{
				// Token: 0x0400CBEA RID: 52202
				public static LocString NAME = "Relaxing";

				// Token: 0x0400CBEB RID: 52203
				public static LocString DESC = "Use leisure facilities, chat with other Duplicants, and relieve Stress.";

				// Token: 0x0400CBEC RID: 52204
				public static LocString ARCHETYPE_NAME = "Relaxer";
			}

			// Token: 0x020030B0 RID: 12464
			public class BASEKEEPING
			{
				// Token: 0x0400CBED RID: 52205
				public static LocString NAME = "Tidying";

				// Token: 0x0400CBEE RID: 52206
				public static LocString DESC = "Sweep, mop, and disinfect objects within the colony.";

				// Token: 0x0400CBEF RID: 52207
				public static LocString ARCHETYPE_NAME = "Tidier";
			}

			// Token: 0x020030B1 RID: 12465
			public class DIG
			{
				// Token: 0x0400CBF0 RID: 52208
				public static LocString NAME = "Digging";

				// Token: 0x0400CBF1 RID: 52209
				public static LocString DESC = "Mine raw resources.";

				// Token: 0x0400CBF2 RID: 52210
				public static LocString ARCHETYPE_NAME = "Digger";
			}

			// Token: 0x020030B2 RID: 12466
			public class MEDICALAID
			{
				// Token: 0x0400CBF3 RID: 52211
				public static LocString NAME = "Doctoring";

				// Token: 0x0400CBF4 RID: 52212
				public static LocString DESC = "Treat sick and injured Duplicants.";

				// Token: 0x0400CBF5 RID: 52213
				public static LocString ARCHETYPE_NAME = "Doctor";
			}

			// Token: 0x020030B3 RID: 12467
			public class MASSAGE
			{
				// Token: 0x0400CBF6 RID: 52214
				public static LocString NAME = "Relaxing";

				// Token: 0x0400CBF7 RID: 52215
				public static LocString DESC = "Take breaks for massages.";

				// Token: 0x0400CBF8 RID: 52216
				public static LocString ARCHETYPE_NAME = "Relaxer";
			}

			// Token: 0x020030B4 RID: 12468
			public class MACHINEOPERATING
			{
				// Token: 0x0400CBF9 RID: 52217
				public static LocString NAME = "Operating";

				// Token: 0x0400CBFA RID: 52218
				public static LocString DESC = "Operating machinery for production, fabrication, and utility purposes.";

				// Token: 0x0400CBFB RID: 52219
				public static LocString ARCHETYPE_NAME = "Operator";
			}

			// Token: 0x020030B5 RID: 12469
			public class SUITS
			{
				// Token: 0x0400CBFC RID: 52220
				public static LocString ARCHETYPE_NAME = "Suit Wearer";
			}

			// Token: 0x020030B6 RID: 12470
			public class ROCKETRY
			{
				// Token: 0x0400CBFD RID: 52221
				public static LocString NAME = "Rocketry";

				// Token: 0x0400CBFE RID: 52222
				public static LocString DESC = "Pilot rockets";

				// Token: 0x0400CBFF RID: 52223
				public static LocString ARCHETYPE_NAME = "Pilot";
			}
		}

		// Token: 0x020021B3 RID: 8627
		public class STATUSITEMS
		{
			// Token: 0x020030B7 RID: 12471
			public class SLIPPERING
			{
				// Token: 0x0400CC00 RID: 52224
				public static LocString NAME = "Slipping";

				// Token: 0x0400CC01 RID: 52225
				public static LocString TOOLTIP = "This Duplicant is losing their balance on a slippery surface\n\nIt's not fun";
			}

			// Token: 0x020030B8 RID: 12472
			public class WAXEDFORTRANSITTUBE
			{
				// Token: 0x0400CC02 RID: 52226
				public static LocString NAME = "Smooth Rider";

				// Token: 0x0400CC03 RID: 52227
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slapped on some ",
					ELEMENTS.MILKFAT.NAME,
					" before starting their commute\n\nThis boosts their ",
					BUILDINGS.PREFABS.TRAVELTUBE.NAME,
					" travel speed by {0}"
				});
			}

			// Token: 0x020030B9 RID: 12473
			public class ARMINGTRAP
			{
				// Token: 0x0400CC04 RID: 52228
				public static LocString NAME = "Arming trap";

				// Token: 0x0400CC05 RID: 52229
				public static LocString TOOLTIP = "This Duplicant is arming a trap";
			}

			// Token: 0x020030BA RID: 12474
			public class GENERIC_DELIVER
			{
				// Token: 0x0400CC06 RID: 52230
				public static LocString NAME = "Delivering resources to {Target}";

				// Token: 0x0400CC07 RID: 52231
				public static LocString TOOLTIP = "This Duplicant is transporting materials to <b>{Target}</b>";
			}

			// Token: 0x020030BB RID: 12475
			public class COUGHING
			{
				// Token: 0x0400CC08 RID: 52232
				public static LocString NAME = "Yucky Lungs Coughing";

				// Token: 0x0400CC09 RID: 52233
				public static LocString TOOLTIP = "Hey! Do that into your elbow\n• Coughing fit was caused by " + DUPLICANTS.MODIFIERS.CONTAMINATEDLUNGS.NAME;
			}

			// Token: 0x020030BC RID: 12476
			public class WEARING_PAJAMAS
			{
				// Token: 0x0400CC0A RID: 52234
				public static LocString NAME = "Wearing " + UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS");

				// Token: 0x0400CC0B RID: 52235
				public static LocString TOOLTIP = "This Duplicant can now produce " + UI.FormatAsLink("Dream Journals", "DREAMJOURNAL") + " when sleeping";
			}

			// Token: 0x020030BD RID: 12477
			public class DREAMING
			{
				// Token: 0x0400CC0C RID: 52236
				public static LocString NAME = "Dreaming";

				// Token: 0x0400CC0D RID: 52237
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is adventuring through their own subconscious\n\nDreams are caused by wearing ",
					UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS"),
					"\n\n",
					UI.FormatAsLink("Dream Journal", "DREAMJOURNAL"),
					" will be ready in {time}"
				});
			}

			// Token: 0x020030BE RID: 12478
			public class FOSSILHUNT
			{
				// Token: 0x0200381F RID: 14367
				public class WORKEREXCAVATING
				{
					// Token: 0x0400DE2A RID: 56874
					public static LocString NAME = "Excavating Fossil";

					// Token: 0x0400DE2B RID: 56875
					public static LocString TOOLTIP = "This Duplicant is carefully uncovering a " + UI.FormatAsLink("Fossil", "FOSSIL");
				}
			}

			// Token: 0x020030BF RID: 12479
			public class SLEEPING
			{
				// Token: 0x0400CC0E RID: 52238
				public static LocString NAME = "Sleeping";

				// Token: 0x0400CC0F RID: 52239
				public static LocString TOOLTIP = "This Duplicant is recovering stamina";

				// Token: 0x0400CC10 RID: 52240
				public static LocString TOOLTIP_DISTURBER = "\n\nThey were sleeping peacefully until they were disturbed by <b>{Disturber}</b>";
			}

			// Token: 0x020030C0 RID: 12480
			public class SLEEPINGEXHAUSTED
			{
				// Token: 0x0400CC11 RID: 52241
				public static LocString NAME = "Unscheduled Nap";

				// Token: 0x0400CC12 RID: 52242
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Cold ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" or lack of rest depleted this Duplicant's ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					"\n\nThey didn't have enough energy to make it to bedtime"
				});
			}

			// Token: 0x020030C1 RID: 12481
			public class SLEEPINGPEACEFULLY
			{
				// Token: 0x0400CC13 RID: 52243
				public static LocString NAME = "Sleeping peacefully";

				// Token: 0x0400CC14 RID: 52244
				public static LocString TOOLTIP = "This Duplicant is getting well-deserved, quality sleep\n\nAt this rate they're sure to feel " + UI.FormatAsLink("Well Rested", "SLEEP") + " tomorrow morning";
			}

			// Token: 0x020030C2 RID: 12482
			public class SLEEPINGBADLY
			{
				// Token: 0x0400CC15 RID: 52245
				public static LocString NAME = "Sleeping badly";

				// Token: 0x0400CC16 RID: 52246
				public static LocString TOOLTIP = "This Duplicant's having trouble falling asleep due to noise from <b>{Disturber}</b>\n\nThey're going to feel a bit " + UI.FormatAsLink("Unrested", "SLEEP") + " tomorrow morning";
			}

			// Token: 0x020030C3 RID: 12483
			public class SLEEPINGTERRIBLY
			{
				// Token: 0x0400CC17 RID: 52247
				public static LocString NAME = "Can't sleep";

				// Token: 0x0400CC18 RID: 52248
				public static LocString TOOLTIP = "This Duplicant was woken up by noise from <b>{Disturber}</b> and can't get back to sleep\n\nThey're going to feel " + UI.FormatAsLink("Dead Tired", "SLEEP") + " tomorrow morning";
			}

			// Token: 0x020030C4 RID: 12484
			public class SLEEPINGINTERRUPTEDBYLIGHT
			{
				// Token: 0x0400CC19 RID: 52249
				public static LocString NAME = "Interrupted Sleep: Bright Light";

				// Token: 0x0400CC1A RID: 52250
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant can't sleep because the ",
					UI.PRE_KEYWORD,
					"Lights",
					UI.PST_KEYWORD,
					" are still on"
				});
			}

			// Token: 0x020030C5 RID: 12485
			public class SLEEPINGINTERRUPTEDBYNOISE
			{
				// Token: 0x0400CC1B RID: 52251
				public static LocString NAME = "Interrupted Sleep: Snoring Friend";

				// Token: 0x0400CC1C RID: 52252
				public static LocString TOOLTIP = "This Duplicant is having trouble sleeping thanks to a certain noisy someone";
			}

			// Token: 0x020030C6 RID: 12486
			public class SLEEPINGINTERRUPTEDBYFEAROFDARK
			{
				// Token: 0x0400CC1D RID: 52253
				public static LocString NAME = "Interrupted Sleep: Afraid of Dark";

				// Token: 0x0400CC1E RID: 52254
				public static LocString TOOLTIP = "This Duplicant is having trouble sleeping because of their fear of the dark";
			}

			// Token: 0x020030C7 RID: 12487
			public class SLEEPINGINTERRUPTEDBYMOVEMENT
			{
				// Token: 0x0400CC1F RID: 52255
				public static LocString NAME = "Interrupted Sleep: Bed Jostling";

				// Token: 0x0400CC20 RID: 52256
				public static LocString TOOLTIP = "This Duplicant was woken up because their bed was moved";
			}

			// Token: 0x020030C8 RID: 12488
			public class SLEEPINGINTERRUPTEDBYCOLD
			{
				// Token: 0x0400CC21 RID: 52257
				public static LocString NAME = "Interrupted Sleep: Cold Room";

				// Token: 0x0400CC22 RID: 52258
				public static LocString TOOLTIP = "This Duplicant is having trouble sleeping because this room is too cold";
			}

			// Token: 0x020030C9 RID: 12489
			public class REDALERT
			{
				// Token: 0x0400CC23 RID: 52259
				public static LocString NAME = "Red Alert!";

				// Token: 0x0400CC24 RID: 52260
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony is in a state of ",
					UI.PRE_KEYWORD,
					"Red Alert",
					UI.PST_KEYWORD,
					". Duplicants will not eat, sleep, use the bathroom, or engage in leisure activities while the ",
					UI.PRE_KEYWORD,
					"Red Alert",
					UI.PST_KEYWORD,
					" is active"
				});
			}

			// Token: 0x020030CA RID: 12490
			public class ROLE
			{
				// Token: 0x0400CC25 RID: 52261
				public static LocString NAME = "{Role}: {Progress} Mastery";

				// Token: 0x0400CC26 RID: 52262
				public static LocString TOOLTIP = "This Duplicant is working as a <b>{Role}</b>\n\nThey have <b>{Progress}</b> mastery of this job";
			}

			// Token: 0x020030CB RID: 12491
			public class LOWOXYGEN
			{
				// Token: 0x0400CC27 RID: 52263
				public static LocString NAME = "Oxygen low";

				// Token: 0x0400CC28 RID: 52264
				public static LocString TOOLTIP = "This Duplicant is working in a low breathability area";

				// Token: 0x0400CC29 RID: 52265
				public static LocString NOTIFICATION_NAME = "Low " + ELEMENTS.OXYGEN.NAME + " area entered";

				// Token: 0x0400CC2A RID: 52266
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are working in areas with low " + ELEMENTS.OXYGEN.NAME + ":";
			}

			// Token: 0x020030CC RID: 12492
			public class SEVEREWOUNDS
			{
				// Token: 0x0400CC2B RID: 52267
				public static LocString NAME = "Severely injured";

				// Token: 0x0400CC2C RID: 52268
				public static LocString TOOLTIP = "This Duplicant is badly hurt";

				// Token: 0x0400CC2D RID: 52269
				public static LocString NOTIFICATION_NAME = "Severely injured";

				// Token: 0x0400CC2E RID: 52270
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are badly hurt and require medical attention";
			}

			// Token: 0x020030CD RID: 12493
			public class INCAPACITATED
			{
				// Token: 0x0400CC2F RID: 52271
				public static LocString NAME = "Incapacitated: {CauseOfIncapacitation}\nTime until death: {TimeUntilDeath}\n";

				// Token: 0x0400CC30 RID: 52272
				public static LocString TOOLTIP = "This Duplicant is near death!\n\nAssign them to a Triage Cot for rescue";

				// Token: 0x0400CC31 RID: 52273
				public static LocString NOTIFICATION_NAME = "Incapacitated";

				// Token: 0x0400CC32 RID: 52274
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are near death.\nA " + BUILDINGS.PREFABS.MEDICALCOT.NAME + " is required for rescue:";
			}

			// Token: 0x020030CE RID: 12494
			public class BIONICOFFLINEINCAPACITATED
			{
				// Token: 0x0400CC33 RID: 52275
				public static LocString NAME = "Incapacitated: Powerless";

				// Token: 0x0400CC34 RID: 52276
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is non-functional!\n\nDeliver a charged ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" and reboot their systems to revive them"
				});

				// Token: 0x0400CC35 RID: 52277
				public static LocString NOTIFICATION_NAME = "Bionic Duplicant Incapacitated";

				// Token: 0x0400CC36 RID: 52278
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Bionic Duplicants are non-functional.\n\nA charged ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" and full reboot by a skilled Duplicant are required for rescue:"
				});
			}

			// Token: 0x020030CF RID: 12495
			public class BIONICWANTSOILCHANGE
			{
				// Token: 0x0400CC37 RID: 52279
				public static LocString NAME = "Low Oil";

				// Token: 0x0400CC38 RID: 52280
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is almost out of ",
					UI.PRE_KEYWORD,
					"Oil",
					UI.PST_KEYWORD,
					" and needs to visit a ",
					UI.PRE_KEYWORD,
					"Lubrication Station",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020030D0 RID: 12496
			public class BIONICWAITINGFORREBOOT
			{
				// Token: 0x0400CC39 RID: 52281
				public static LocString NAME = "Awaiting Reboot";

				// Token: 0x0400CC3A RID: 52282
				public static LocString TOOLTIP = "This Duplicant needs someone to reboot their bionic systems so they can get back to work";
			}

			// Token: 0x020030D1 RID: 12497
			public class BIONICBEINGREBOOTED
			{
				// Token: 0x0400CC3B RID: 52283
				public static LocString NAME = "Reboot in progress";

				// Token: 0x0400CC3C RID: 52284
				public static LocString TOOLTIP = "This Duplicant's bionic systems are being rebooted";
			}

			// Token: 0x020030D2 RID: 12498
			public class BIONICREQUIRESSKILLPERK
			{
				// Token: 0x0400CC3D RID: 52285
				public static LocString NAME = "Skill-Required Operation";

				// Token: 0x0400CC3E RID: 52286
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Only Duplicants with the following ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					" can reboot this Duplicant's bionic systems:\n\n{Skills}"
				});
			}

			// Token: 0x020030D3 RID: 12499
			public class BEDUNREACHABLE
			{
				// Token: 0x0400CC3F RID: 52287
				public static LocString NAME = "Cannot reach bed";

				// Token: 0x0400CC40 RID: 52288
				public static LocString TOOLTIP = "This Duplicant cannot reach their bed";

				// Token: 0x0400CC41 RID: 52289
				public static LocString NOTIFICATION_NAME = "Unreachable bed";

				// Token: 0x0400CC42 RID: 52290
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot sleep because their ",
					UI.PRE_KEYWORD,
					"Beds",
					UI.PST_KEYWORD,
					" are beyond their reach:"
				});
			}

			// Token: 0x020030D4 RID: 12500
			public class COLD
			{
				// Token: 0x0400CC43 RID: 52291
				public static LocString NAME = "Chilly surroundings";

				// Token: 0x0400CC44 RID: 52292
				public static LocString TOOLTIP = "This Duplicant cannot retain enough heat to stay warm and may be under-insulated for this area\n\nThey will begin to recover shortly after they leave this area\n\nStress: <b>{StressModification}</b>\nStamina: <b>{StaminaModification}</b>\nAthletics: <b>{AthleticsModification}</b>\n\nCurrent Environmental Exchange: <b>{currentTransferWattage}</b>\n\nInsulation Thickness: {conductivityBarrier}";
			}

			// Token: 0x020030D5 RID: 12501
			public class EXITINGCOLD
			{
				// Token: 0x0400CC45 RID: 52293
				public static LocString NAME = "Shivering";

				// Token: 0x0400CC46 RID: 52294
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was recently exposed to cold ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" and wants to warm up\n\nWithout a warming station, it will take {0} for them to recover\n\nStress: <b>{StressModification}</b>\nStamina: <b>{StaminaModification}</b>\nAthletics: <b>{AthleticsModification}</b>"
				});
			}

			// Token: 0x020030D6 RID: 12502
			public class DAILYRATIONLIMITREACHED
			{
				// Token: 0x0400CC47 RID: 52295
				public static LocString NAME = "Daily calorie limit reached";

				// Token: 0x0400CC48 RID: 52296
				public static LocString TOOLTIP = "This Duplicant has consumed their allotted " + UI.FormatAsLink("Rations", "FOOD") + " for the day";

				// Token: 0x0400CC49 RID: 52297
				public static LocString NOTIFICATION_NAME = "Daily calorie limit reached";

				// Token: 0x0400CC4A RID: 52298
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants have consumed their allotted " + UI.FormatAsLink("Rations", "FOOD") + " for the day:";
			}

			// Token: 0x020030D7 RID: 12503
			public class DOCTOR
			{
				// Token: 0x0400CC4B RID: 52299
				public static LocString NAME = "Treating Patient";

				// Token: 0x0400CC4C RID: 52300
				public static LocString STATUS = "This Duplicant is going to administer medical care to an ailing friend";
			}

			// Token: 0x020030D8 RID: 12504
			public class HOLDINGBREATH
			{
				// Token: 0x0400CC4D RID: 52301
				public static LocString NAME = "Holding breath";

				// Token: 0x0400CC4E RID: 52302
				public static LocString TOOLTIP = "This Duplicant cannot breathe in their current location";
			}

			// Token: 0x020030D9 RID: 12505
			public class RECOVERINGBREATH
			{
				// Token: 0x0400CC4F RID: 52303
				public static LocString NAME = "Recovering breath";

				// Token: 0x0400CC50 RID: 52304
				public static LocString TOOLTIP = "This Duplicant held their breath too long and needs a moment";
			}

			// Token: 0x020030DA RID: 12506
			public class HOT
			{
				// Token: 0x0400CC51 RID: 52305
				public static LocString NAME = "Toasty surroundings";

				// Token: 0x0400CC52 RID: 52306
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant cannot let off enough ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" to stay cool and may be over-insulated for this area\n\nThey will begin to recover shortly after they leave this area\n\nStress Modification: <b>{StressModification}</b>\nStamina: <b>{StaminaModification}</b>\nAthletics: <b>{AthleticsModification}</b>\n\nCurrent Environmental Exchange: <b>{currentTransferWattage}</b>\n\nInsulation Thickness: {conductivityBarrier}"
				});
			}

			// Token: 0x020030DB RID: 12507
			public class EXITINGHOT
			{
				// Token: 0x0400CC53 RID: 52307
				public static LocString NAME = "Sweaty";

				// Token: 0x0400CC54 RID: 52308
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was recently exposed to hot ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" and wants to cool down\n\nWithout a cooling station, it will take {0} for them to recover\n\nStress: <b>{StressModification}</b>\nStamina: <b>{StaminaModification}</b>\nAthletics: <b>{AthleticsModification}</b>"
				});
			}

			// Token: 0x020030DC RID: 12508
			public class HUNGRY
			{
				// Token: 0x0400CC55 RID: 52309
				public static LocString NAME = "Hungry";

				// Token: 0x0400CC56 RID: 52310
				public static LocString TOOLTIP = "This Duplicant would really like something to eat";
			}

			// Token: 0x020030DD RID: 12509
			public class POORDECOR
			{
				// Token: 0x0400CC57 RID: 52311
				public static LocString NAME = "Drab decor";

				// Token: 0x0400CC58 RID: 52312
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is depressed by the lack of ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" in this area"
				});
			}

			// Token: 0x020030DE RID: 12510
			public class POORQUALITYOFLIFE
			{
				// Token: 0x0400CC59 RID: 52313
				public static LocString NAME = "Low Morale";

				// Token: 0x0400CC5A RID: 52314
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The bad in this Duplicant's life is starting to outweigh the good\n\nImproved amenities and additional ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" would help improve their ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020030DF RID: 12511
			public class POOR_FOOD_QUALITY
			{
				// Token: 0x0400CC5B RID: 52315
				public static LocString NAME = "Lousy Meal";

				// Token: 0x0400CC5C RID: 52316
				public static LocString TOOLTIP = "The last meal this Duplicant ate didn't quite meet their expectations";
			}

			// Token: 0x020030E0 RID: 12512
			public class GOOD_FOOD_QUALITY
			{
				// Token: 0x0400CC5D RID: 52317
				public static LocString NAME = "Decadent Meal";

				// Token: 0x0400CC5E RID: 52318
				public static LocString TOOLTIP = "The last meal this Duplicant ate exceeded their expectations!";
			}

			// Token: 0x020030E1 RID: 12513
			public class NERVOUSBREAKDOWN
			{
				// Token: 0x0400CC5F RID: 52319
				public static LocString NAME = "Nervous breakdown";

				// Token: 0x0400CC60 RID: 52320
				public static LocString TOOLTIP = UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD + " has completely eroded this Duplicant's ability to function";

				// Token: 0x0400CC61 RID: 52321
				public static LocString NOTIFICATION_NAME = "Nervous breakdown";

				// Token: 0x0400CC62 RID: 52322
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have cracked under the ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" and need assistance:"
				});
			}

			// Token: 0x020030E2 RID: 12514
			public class STRESSED
			{
				// Token: 0x0400CC63 RID: 52323
				public static LocString NAME = "Stressed";

				// Token: 0x0400CC64 RID: 52324
				public static LocString TOOLTIP = "This Duplicant is feeling the pressure";

				// Token: 0x0400CC65 RID: 52325
				public static LocString NOTIFICATION_NAME = "High stress";

				// Token: 0x0400CC66 RID: 52326
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants are ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" and need to unwind:"
				});
			}

			// Token: 0x020030E3 RID: 12515
			public class NORATIONSAVAILABLE
			{
				// Token: 0x0400CC67 RID: 52327
				public static LocString NAME = "No food available";

				// Token: 0x0400CC68 RID: 52328
				public static LocString TOOLTIP = "There's nothing in the colony for this Duplicant to eat";

				// Token: 0x0400CC69 RID: 52329
				public static LocString NOTIFICATION_NAME = "No food available";

				// Token: 0x0400CC6A RID: 52330
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants have nothing to eat:";
			}

			// Token: 0x020030E4 RID: 12516
			public class QUARANTINEAREAUNREACHABLE
			{
				// Token: 0x0400CC6B RID: 52331
				public static LocString NAME = "Cannot reach quarantine";

				// Token: 0x0400CC6C RID: 52332
				public static LocString TOOLTIP = "This Duplicant cannot reach their quarantine zone";

				// Token: 0x0400CC6D RID: 52333
				public static LocString NOTIFICATION_NAME = "Unreachable quarantine";

				// Token: 0x0400CC6E RID: 52334
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants cannot reach their assigned quarantine zones:";
			}

			// Token: 0x020030E5 RID: 12517
			public class QUARANTINED
			{
				// Token: 0x0400CC6F RID: 52335
				public static LocString NAME = "Quarantined";

				// Token: 0x0400CC70 RID: 52336
				public static LocString TOOLTIP = "This Duplicant has been isolated from the colony";
			}

			// Token: 0x020030E6 RID: 12518
			public class RATIONSUNREACHABLE
			{
				// Token: 0x0400CC71 RID: 52337
				public static LocString NAME = "Cannot reach food";

				// Token: 0x0400CC72 RID: 52338
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" in the colony that this Duplicant cannot reach"
				});

				// Token: 0x0400CC73 RID: 52339
				public static LocString NOTIFICATION_NAME = "Unreachable food";

				// Token: 0x0400CC74 RID: 52340
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot access the colony's ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020030E7 RID: 12519
			public class RATIONSNOTPERMITTED
			{
				// Token: 0x0400CC75 RID: 52341
				public static LocString NAME = "Food Type Not Permitted";

				// Token: 0x0400CC76 RID: 52342
				public static LocString TOOLTIP = "This Duplicant is not allowed to eat any of the " + UI.FormatAsLink("Food", "FOOD") + " in their reach\n\nEnter the <color=#833A5FFF>CONSUMABLES</color> <color=#F44A47><b>[F]</b></color> to adjust their food permissions";

				// Token: 0x0400CC77 RID: 52343
				public static LocString NOTIFICATION_NAME = "Unpermitted food";

				// Token: 0x0400CC78 RID: 52344
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants' <color=#833A5FFF>CONSUMABLES</color> <color=#F44A47><b>[F]</b></color> permissions prevent them from eating any of the " + UI.FormatAsLink("Food", "FOOD") + " within their reach:";
			}

			// Token: 0x020030E8 RID: 12520
			public class ROTTEN
			{
				// Token: 0x0400CC79 RID: 52345
				public static LocString NAME = "Rotten";

				// Token: 0x0400CC7A RID: 52346
				public static LocString TOOLTIP = "Gross!";
			}

			// Token: 0x020030E9 RID: 12521
			public class STARVING
			{
				// Token: 0x0400CC7B RID: 52347
				public static LocString NAME = "Starving";

				// Token: 0x0400CC7C RID: 52348
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is about to die and needs ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					"!"
				});

				// Token: 0x0400CC7D RID: 52349
				public static LocString NOTIFICATION_NAME = "Starvation";

				// Token: 0x0400CC7E RID: 52350
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants are starving and will die if they can't find ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020030EA RID: 12522
			public class STRESS_SIGNAL_AGGRESIVE
			{
				// Token: 0x0400CC7F RID: 52351
				public static LocString NAME = "Frustrated";

				// Token: 0x0400CC80 RID: 52352
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is trying to keep their cool\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they destroy something to let off steam"
				});
			}

			// Token: 0x020030EB RID: 12523
			public class STRESS_SIGNAL_BINGE_EAT
			{
				// Token: 0x0400CC81 RID: 52353
				public static LocString NAME = "Stress Cravings";

				// Token: 0x0400CC82 RID: 52354
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is consumed by hunger\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they eat all the colony's ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" stores"
				});
			}

			// Token: 0x020030EC RID: 12524
			public class STRESS_SIGNAL_UGLY_CRIER
			{
				// Token: 0x0400CC83 RID: 52355
				public static LocString NAME = "Misty Eyed";

				// Token: 0x0400CC84 RID: 52356
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is trying and failing to swallow their emotions\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they have a good ugly cry"
				});
			}

			// Token: 0x020030ED RID: 12525
			public class STRESS_SIGNAL_VOMITER
			{
				// Token: 0x0400CC85 RID: 52357
				public static LocString NAME = "Stress Burp";

				// Token: 0x0400CC86 RID: 52358
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Sort of like having butterflies in your stomach, except they're burps\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they start to stress vomit"
				});
			}

			// Token: 0x020030EE RID: 12526
			public class STRESS_SIGNAL_BANSHEE
			{
				// Token: 0x0400CC87 RID: 52359
				public static LocString NAME = "Suppressed Screams";

				// Token: 0x0400CC88 RID: 52360
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is fighting the urge to scream\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they start wailing uncontrollably"
				});
			}

			// Token: 0x020030EF RID: 12527
			public class STRESS_SIGNAL_STRESS_SHOCKER
			{
				// Token: 0x0400CC89 RID: 52361
				public static LocString NAME = "Dangerously Frayed";

				// Token: 0x0400CC8A RID: 52362
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's hanging by a thread...except the thread is a live wire\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they zap someone"
				});
			}

			// Token: 0x020030F0 RID: 12528
			public class ENTOMBEDCHORE
			{
				// Token: 0x0400CC8B RID: 52363
				public static LocString NAME = "Entombed";

				// Token: 0x0400CC8C RID: 52364
				public static LocString TOOLTIP = "This Duplicant needs someone to help dig them out!";

				// Token: 0x0400CC8D RID: 52365
				public static LocString NOTIFICATION_NAME = "Entombed";

				// Token: 0x0400CC8E RID: 52366
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are trapped:";
			}

			// Token: 0x020030F1 RID: 12529
			public class EARLYMORNING
			{
				// Token: 0x0400CC8F RID: 52367
				public static LocString NAME = "Early Bird";

				// Token: 0x0400CC90 RID: 52368
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is jazzed to start the day\n• All ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" <b>+2</b> in the morning"
				});
			}

			// Token: 0x020030F2 RID: 12530
			public class NIGHTTIME
			{
				// Token: 0x0400CC91 RID: 52369
				public static LocString NAME = "Night Owl";

				// Token: 0x0400CC92 RID: 52370
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is more efficient on a nighttime ",
					UI.PRE_KEYWORD,
					"Schedule",
					UI.PST_KEYWORD,
					"\n• All ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" <b>+3</b> at night"
				});
			}

			// Token: 0x020030F3 RID: 12531
			public class METEORPHILE
			{
				// Token: 0x0400CC93 RID: 52371
				public static LocString NAME = "Rock Fan";

				// Token: 0x0400CC94 RID: 52372
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is <i>really</i> into meteor showers\n• All ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" <b>+3</b> during meteor showers"
				});
			}

			// Token: 0x020030F4 RID: 12532
			public class SUFFOCATING
			{
				// Token: 0x0400CC95 RID: 52373
				public static LocString NAME = "Suffocating";

				// Token: 0x0400CC96 RID: 52374
				public static LocString TOOLTIP = "This Duplicant cannot breathe!";

				// Token: 0x0400CC97 RID: 52375
				public static LocString NOTIFICATION_NAME = "Suffocating";

				// Token: 0x0400CC98 RID: 52376
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants cannot breathe:";
			}

			// Token: 0x020030F5 RID: 12533
			public class TIRED
			{
				// Token: 0x0400CC99 RID: 52377
				public static LocString NAME = "Tired";

				// Token: 0x0400CC9A RID: 52378
				public static LocString TOOLTIP = "This Duplicant could use a nice nap";
			}

			// Token: 0x020030F6 RID: 12534
			public class IDLE
			{
				// Token: 0x0400CC9B RID: 52379
				public static LocString NAME = "Idle";

				// Token: 0x0400CC9C RID: 52380
				public static LocString TOOLTIP = "This Duplicant cannot reach any pending errands";

				// Token: 0x0400CC9D RID: 52381
				public static LocString NOTIFICATION_NAME = "Idle";

				// Token: 0x0400CC9E RID: 52382
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot reach any pending ",
					UI.PRE_KEYWORD,
					"Errands",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020030F7 RID: 12535
			public class IDLEINROCKETS
			{
				// Token: 0x0400CC9F RID: 52383
				public static LocString NAME = "Idle";

				// Token: 0x0400CCA0 RID: 52384
				public static LocString TOOLTIP = "This Duplicant cannot reach any pending errands";

				// Token: 0x0400CCA1 RID: 52385
				public static LocString NOTIFICATION_NAME = "Idle";

				// Token: 0x0400CCA2 RID: 52386
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot reach any pending ",
					UI.PRE_KEYWORD,
					"Errands",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020030F8 RID: 12536
			public class FIGHTING
			{
				// Token: 0x0400CCA3 RID: 52387
				public static LocString NAME = "In combat";

				// Token: 0x0400CCA4 RID: 52388
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is attacking a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"!"
				});

				// Token: 0x0400CCA5 RID: 52389
				public static LocString NOTIFICATION_NAME = "Combat!";

				// Token: 0x0400CCA6 RID: 52390
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have engaged a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" in combat:"
				});
			}

			// Token: 0x020030F9 RID: 12537
			public class FLEEING
			{
				// Token: 0x0400CCA7 RID: 52391
				public static LocString NAME = "Fleeing";

				// Token: 0x0400CCA8 RID: 52392
				public static LocString TOOLTIP = "This Duplicant is trying to escape something scary!";

				// Token: 0x0400CCA9 RID: 52393
				public static LocString NOTIFICATION_NAME = "Fleeing!";

				// Token: 0x0400CCAA RID: 52394
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are trying to escape:";
			}

			// Token: 0x020030FA RID: 12538
			public class DEAD
			{
				// Token: 0x0400CCAB RID: 52395
				public static LocString NAME = "Dead: {Death}";

				// Token: 0x0400CCAC RID: 52396
				public static LocString TOOLTIP = "This Duplicant definitely isn't sleeping";
			}

			// Token: 0x020030FB RID: 12539
			public class LASHINGOUT
			{
				// Token: 0x0400CCAD RID: 52397
				public static LocString NAME = "Lashing out";

				// Token: 0x0400CCAE RID: 52398
				public static LocString TOOLTIP = "This Duplicant is breaking buildings to relieve their " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400CCAF RID: 52399
				public static LocString NOTIFICATION_NAME = "Lashing out";

				// Token: 0x0400CCB0 RID: 52400
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants broke buildings to relieve their ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020030FC RID: 12540
			public class MOVETOSUITNOTREQUIRED
			{
				// Token: 0x0400CCB1 RID: 52401
				public static LocString NAME = "Exiting Exosuit area";

				// Token: 0x0400CCB2 RID: 52402
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is leaving an area where a ",
					UI.PRE_KEYWORD,
					"Suit",
					UI.PST_KEYWORD,
					" was required"
				});
			}

			// Token: 0x020030FD RID: 12541
			public class NOROLE
			{
				// Token: 0x0400CCB3 RID: 52403
				public static LocString NAME = "No Job";

				// Token: 0x0400CCB4 RID: 52404
				public static LocString TOOLTIP = "This Duplicant does not have a Job Assignment\n\nEnter the " + UI.FormatAsManagementMenu("Jobs Panel", "[J]") + " to view all available Jobs";
			}

			// Token: 0x020030FE RID: 12542
			public class DROPPINGUNUSEDINVENTORY
			{
				// Token: 0x0400CCB5 RID: 52405
				public static LocString NAME = "Dropping objects";

				// Token: 0x0400CCB6 RID: 52406
				public static LocString TOOLTIP = "This Duplicant is dropping what they're holding";
			}

			// Token: 0x020030FF RID: 12543
			public class MOVINGTOSAFEAREA
			{
				// Token: 0x0400CCB7 RID: 52407
				public static LocString NAME = "Moving to safe area";

				// Token: 0x0400CCB8 RID: 52408
				public static LocString TOOLTIP = "This Duplicant is finding a less dangerous place";
			}

			// Token: 0x02003100 RID: 12544
			public class TOILETUNREACHABLE
			{
				// Token: 0x0400CCB9 RID: 52409
				public static LocString NAME = "Unreachable toilet";

				// Token: 0x0400CCBA RID: 52410
				public static LocString TOOLTIP = "This Duplicant cannot reach a functioning " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " or " + UI.FormatAsLink("Lavatory", "FLUSHTOILET");

				// Token: 0x0400CCBB RID: 52411
				public static LocString NOTIFICATION_NAME = "Unreachable toilet";

				// Token: 0x0400CCBC RID: 52412
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot reach a functioning ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					" or ",
					UI.FormatAsLink("Lavatory", "FLUSHTOILET"),
					":"
				});
			}

			// Token: 0x02003101 RID: 12545
			public class NOUSABLETOILETS
			{
				// Token: 0x0400CCBD RID: 52413
				public static LocString NAME = "Toilet out of order";

				// Token: 0x0400CCBE RID: 52414
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The only ",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" or ",
					UI.FormatAsLink("Lavatories", "FLUSHTOILET"),
					" in this Duplicant's reach are out of order"
				});

				// Token: 0x0400CCBF RID: 52415
				public static LocString NOTIFICATION_NAME = "Toilet out of order";

				// Token: 0x0400CCC0 RID: 52416
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants want to use an ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					" or ",
					UI.FormatAsLink("Lavatory", "FLUSHTOILET"),
					" that is out of order:"
				});
			}

			// Token: 0x02003102 RID: 12546
			public class NOTOILETS
			{
				// Token: 0x0400CCC1 RID: 52417
				public static LocString NAME = "No Outhouses";

				// Token: 0x0400CCC2 RID: 52418
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There are no ",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" available for this Duplicant\n\n",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5)
				});

				// Token: 0x0400CCC3 RID: 52419
				public static LocString NOTIFICATION_NAME = "No Outhouses built";

				// Token: 0x0400CCC4 RID: 52420
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					".\n\nThese Duplicants are in need of an ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					":"
				});
			}

			// Token: 0x02003103 RID: 12547
			public class FULLBLADDER
			{
				// Token: 0x0400CCC5 RID: 52421
				public static LocString NAME = "Full bladder";

				// Token: 0x0400CCC6 RID: 52422
				public static LocString TOOLTIP = "This Duplicant would really appreciate an " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " or " + UI.FormatAsLink("Lavatory", "FLUSHTOILET");
			}

			// Token: 0x02003104 RID: 12548
			public class STRESSFULLYEMPTYINGOIL
			{
				// Token: 0x0400CCC7 RID: 52423
				public static LocString NAME = "Expelling gunk";

				// Token: 0x0400CCC8 RID: 52424
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Bionic Duplicant couldn't get to a ",
					UI.FormatAsLink("Gunk Extractor", "GUNKEMPTIER"),
					" in time and got desperate\n\n",
					UI.FormatAsLink("Gunk Extractors", "GUNKEMPTIER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5)
				});

				// Token: 0x0400CCC9 RID: 52425
				public static LocString NOTIFICATION_NAME = "Expelled gunk";

				// Token: 0x0400CCCA RID: 52426
				public static LocString NOTIFICATION_TOOLTIP = "The " + UI.FormatAsTool("Mop Tool", global::Action.Mop) + " can be used to clean up Duplicant-related \"spills\"\n\nThese Duplicants made messes that require cleaning up:\n";
			}

			// Token: 0x02003105 RID: 12549
			public class STRESSFULLYEMPTYINGBLADDER
			{
				// Token: 0x0400CCCB RID: 52427
				public static LocString NAME = "Making a mess";

				// Token: 0x0400CCCC RID: 52428
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This poor Duplicant couldn't find an ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					" in time and is super embarrassed\n\n",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5)
				});

				// Token: 0x0400CCCD RID: 52429
				public static LocString NOTIFICATION_NAME = "Made a mess";

				// Token: 0x0400CCCE RID: 52430
				public static LocString NOTIFICATION_TOOLTIP = "The " + UI.FormatAsTool("Mop Tool", global::Action.Mop) + " can be used to clean up Duplicant-related \"spills\"\n\nThese Duplicants made messes that require cleaning up:\n";
			}

			// Token: 0x02003106 RID: 12550
			public class WASHINGHANDS
			{
				// Token: 0x0400CCCF RID: 52431
				public static LocString NAME = "Washing hands";

				// Token: 0x0400CCD0 RID: 52432
				public static LocString TOOLTIP = "This Duplicant is washing their hands";
			}

			// Token: 0x02003107 RID: 12551
			public class SHOWERING
			{
				// Token: 0x0400CCD1 RID: 52433
				public static LocString NAME = "Showering";

				// Token: 0x0400CCD2 RID: 52434
				public static LocString TOOLTIP = "This Duplicant is gonna be squeaky clean";
			}

			// Token: 0x02003108 RID: 12552
			public class RELAXING
			{
				// Token: 0x0400CCD3 RID: 52435
				public static LocString NAME = "Relaxing";

				// Token: 0x0400CCD4 RID: 52436
				public static LocString TOOLTIP = "This Duplicant's just taking it easy";
			}

			// Token: 0x02003109 RID: 12553
			public class VOMITING
			{
				// Token: 0x0400CCD5 RID: 52437
				public static LocString NAME = "Throwing up";

				// Token: 0x0400CCD6 RID: 52438
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has unceremoniously hurled as the result of a ",
					UI.FormatAsLink("Disease", "DISEASE"),
					"\n\nDuplicant-related \"spills\" can be cleaned up using the ",
					UI.PRE_KEYWORD,
					"Mop Tool",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.Mop)
				});

				// Token: 0x0400CCD7 RID: 52439
				public static LocString NOTIFICATION_NAME = "Throwing up";

				// Token: 0x0400CCD8 RID: 52440
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop),
					" can be used to clean up Duplicant-related \"spills\"\n\nA ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" has caused these Duplicants to throw up:"
				});
			}

			// Token: 0x0200310A RID: 12554
			public class STRESSVOMITING
			{
				// Token: 0x0400CCD9 RID: 52441
				public static LocString NAME = "Stress vomiting";

				// Token: 0x0400CCDA RID: 52442
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is relieving their ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" all over the floor\n\nDuplicant-related \"spills\" can be cleaned up using the ",
					UI.PRE_KEYWORD,
					"Mop Tool",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.Mop)
				});

				// Token: 0x0400CCDB RID: 52443
				public static LocString NOTIFICATION_NAME = "Stress vomiting";

				// Token: 0x0400CCDC RID: 52444
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop),
					" can used to clean up Duplicant-related \"spills\"\n\nThese Duplicants became so ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" they threw up:"
				});
			}

			// Token: 0x0200310B RID: 12555
			public class RADIATIONVOMITING
			{
				// Token: 0x0400CCDD RID: 52445
				public static LocString NAME = "Radiation vomiting";

				// Token: 0x0400CCDE RID: 52446
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is sick due to ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" poisoning.\n\nDuplicant-related \"spills\" can be cleaned up using the ",
					UI.PRE_KEYWORD,
					"Mop Tool",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.Mop)
				});

				// Token: 0x0400CCDF RID: 52447
				public static LocString NOTIFICATION_NAME = "Radiation vomiting";

				// Token: 0x0400CCE0 RID: 52448
				public static LocString NOTIFICATION_TOOLTIP = "The " + UI.FormatAsTool("Mop Tool", global::Action.Mop) + " can clean up Duplicant-related \"spills\"\n\nRadiation Sickness caused these Duplicants to throw up:";
			}

			// Token: 0x0200310C RID: 12556
			public class HASDISEASE
			{
				// Token: 0x0400CCE1 RID: 52449
				public static LocString NAME = "Feeling ill";

				// Token: 0x0400CCE2 RID: 52450
				public static LocString TOOLTIP = "This Duplicant has contracted a " + UI.FormatAsLink("Disease", "DISEASE") + " and requires recovery time at a " + UI.FormatAsLink("Sick Bay", "DOCTORSTATION");

				// Token: 0x0400CCE3 RID: 52451
				public static LocString NOTIFICATION_NAME = "Illness";

				// Token: 0x0400CCE4 RID: 52452
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have contracted a ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" and require recovery time at a ",
					UI.FormatAsLink("Sick Bay", "DOCTORSTATION"),
					":"
				});
			}

			// Token: 0x0200310D RID: 12557
			public class BODYREGULATINGHEATING
			{
				// Token: 0x0400CCE5 RID: 52453
				public static LocString NAME = "Regulating temperature at: {TempDelta}";

				// Token: 0x0400CCE6 RID: 52454
				public static LocString TOOLTIP = "This Duplicant is regulating their internal " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x0200310E RID: 12558
			public class BODYREGULATINGCOOLING
			{
				// Token: 0x0400CCE7 RID: 52455
				public static LocString NAME = "Regulating temperature at: {TempDelta}";

				// Token: 0x0400CCE8 RID: 52456
				public static LocString TOOLTIP = "This Duplicant is regulating their internal " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x0200310F RID: 12559
			public class BREATHINGO2
			{
				// Token: 0x0400CCE9 RID: 52457
				public static LocString NAME = "Inhaling {ConsumptionRate} O<sub>2</sub>";

				// Token: 0x0400CCEA RID: 52458
				public static LocString TOOLTIP = "Duplicants require " + UI.FormatAsLink("Oxygen", "OXYGEN") + " to live";
			}

			// Token: 0x02003110 RID: 12560
			public class EMITTINGCO2
			{
				// Token: 0x0400CCEB RID: 52459
				public static LocString NAME = "Exhaling {EmittingRate} CO<sub>2</sub>";

				// Token: 0x0400CCEC RID: 52460
				public static LocString TOOLTIP = "Duplicants breathe out " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE");
			}

			// Token: 0x02003111 RID: 12561
			public class PICKUPDELIVERSTATUS
			{
				// Token: 0x0400CCED RID: 52461
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CCEE RID: 52462
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003112 RID: 12562
			public class STOREDELIVERSTATUS
			{
				// Token: 0x0400CCEF RID: 52463
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CCF0 RID: 52464
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003113 RID: 12563
			public class CLEARDELIVERSTATUS
			{
				// Token: 0x0400CCF1 RID: 52465
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CCF2 RID: 52466
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003114 RID: 12564
			public class STOREFORBUILDDELIVERSTATUS
			{
				// Token: 0x0400CCF3 RID: 52467
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CCF4 RID: 52468
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003115 RID: 12565
			public class STOREFORBUILDPRIORITIZEDDELIVERSTATUS
			{
				// Token: 0x0400CCF5 RID: 52469
				public static LocString NAME = "Allocating {Item} to {Target}";

				// Token: 0x0400CCF6 RID: 52470
				public static LocString TOOLTIP = "This Duplicant is delivering materials to a <b>{Target}</b> construction errand";
			}

			// Token: 0x02003116 RID: 12566
			public class BUILDDELIVERSTATUS
			{
				// Token: 0x0400CCF7 RID: 52471
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CCF8 RID: 52472
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003117 RID: 12567
			public class BUILDPRIORITIZEDSTATUS
			{
				// Token: 0x0400CCF9 RID: 52473
				public static LocString NAME = "Building {Target}";

				// Token: 0x0400CCFA RID: 52474
				public static LocString TOOLTIP = "This Duplicant is constructing a <b>{Target}</b>";
			}

			// Token: 0x02003118 RID: 12568
			public class FABRICATEDELIVERSTATUS
			{
				// Token: 0x0400CCFB RID: 52475
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CCFC RID: 52476
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003119 RID: 12569
			public class USEITEMDELIVERSTATUS
			{
				// Token: 0x0400CCFD RID: 52477
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CCFE RID: 52478
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200311A RID: 12570
			public class STOREPRIORITYDELIVERSTATUS
			{
				// Token: 0x0400CCFF RID: 52479
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD00 RID: 52480
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200311B RID: 12571
			public class STORECRITICALDELIVERSTATUS
			{
				// Token: 0x0400CD01 RID: 52481
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD02 RID: 52482
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200311C RID: 12572
			public class COMPOSTFLIPSTATUS
			{
				// Token: 0x0400CD03 RID: 52483
				public static LocString NAME = "Going to flip compost";

				// Token: 0x0400CD04 RID: 52484
				public static LocString TOOLTIP = "This Duplicant is going to flip the " + BUILDINGS.PREFABS.COMPOST.NAME;
			}

			// Token: 0x0200311D RID: 12573
			public class DECONSTRUCTDELIVERSTATUS
			{
				// Token: 0x0400CD05 RID: 52485
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD06 RID: 52486
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200311E RID: 12574
			public class TOGGLEDELIVERSTATUS
			{
				// Token: 0x0400CD07 RID: 52487
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD08 RID: 52488
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200311F RID: 12575
			public class EMPTYSTORAGEDELIVERSTATUS
			{
				// Token: 0x0400CD09 RID: 52489
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD0A RID: 52490
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003120 RID: 12576
			public class HARVESTDELIVERSTATUS
			{
				// Token: 0x0400CD0B RID: 52491
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD0C RID: 52492
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003121 RID: 12577
			public class SLEEPDELIVERSTATUS
			{
				// Token: 0x0400CD0D RID: 52493
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD0E RID: 52494
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003122 RID: 12578
			public class EATDELIVERSTATUS
			{
				// Token: 0x0400CD0F RID: 52495
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD10 RID: 52496
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003123 RID: 12579
			public class WARMUPDELIVERSTATUS
			{
				// Token: 0x0400CD11 RID: 52497
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD12 RID: 52498
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003124 RID: 12580
			public class REPAIRDELIVERSTATUS
			{
				// Token: 0x0400CD13 RID: 52499
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD14 RID: 52500
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003125 RID: 12581
			public class REPAIRWORKSTATUS
			{
				// Token: 0x0400CD15 RID: 52501
				public static LocString NAME = "Repairing {Target}";

				// Token: 0x0400CD16 RID: 52502
				public static LocString TOOLTIP = "This Duplicant is fixing the <b>{Target}</b>";
			}

			// Token: 0x02003126 RID: 12582
			public class BREAKDELIVERSTATUS
			{
				// Token: 0x0400CD17 RID: 52503
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD18 RID: 52504
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003127 RID: 12583
			public class BREAKWORKSTATUS
			{
				// Token: 0x0400CD19 RID: 52505
				public static LocString NAME = "Breaking {Target}";

				// Token: 0x0400CD1A RID: 52506
				public static LocString TOOLTIP = "This Duplicant is going totally bananas on the <b>{Target}</b>!";
			}

			// Token: 0x02003128 RID: 12584
			public class EQUIPDELIVERSTATUS
			{
				// Token: 0x0400CD1B RID: 52507
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD1C RID: 52508
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003129 RID: 12585
			public class COOKDELIVERSTATUS
			{
				// Token: 0x0400CD1D RID: 52509
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD1E RID: 52510
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200312A RID: 12586
			public class MUSHDELIVERSTATUS
			{
				// Token: 0x0400CD1F RID: 52511
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD20 RID: 52512
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200312B RID: 12587
			public class PACIFYDELIVERSTATUS
			{
				// Token: 0x0400CD21 RID: 52513
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD22 RID: 52514
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200312C RID: 12588
			public class RESCUEDELIVERSTATUS
			{
				// Token: 0x0400CD23 RID: 52515
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD24 RID: 52516
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200312D RID: 12589
			public class RESCUEWORKSTATUS
			{
				// Token: 0x0400CD25 RID: 52517
				public static LocString NAME = "Rescuing {Target}";

				// Token: 0x0400CD26 RID: 52518
				public static LocString TOOLTIP = "This Duplicant is saving <b>{Target}</b> from certain peril!";
			}

			// Token: 0x0200312E RID: 12590
			public class MOPDELIVERSTATUS
			{
				// Token: 0x0400CD27 RID: 52519
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400CD28 RID: 52520
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200312F RID: 12591
			public class DIGGING
			{
				// Token: 0x0400CD29 RID: 52521
				public static LocString NAME = "Digging";

				// Token: 0x0400CD2A RID: 52522
				public static LocString TOOLTIP = "This Duplicant is excavating raw resources";
			}

			// Token: 0x02003130 RID: 12592
			public class EATING
			{
				// Token: 0x0400CD2B RID: 52523
				public static LocString NAME = "Eating {Target}";

				// Token: 0x0400CD2C RID: 52524
				public static LocString TOOLTIP = "This Duplicant is having a meal";
			}

			// Token: 0x02003131 RID: 12593
			public class CLEANING
			{
				// Token: 0x0400CD2D RID: 52525
				public static LocString NAME = "Cleaning {Target}";

				// Token: 0x0400CD2E RID: 52526
				public static LocString TOOLTIP = "This Duplicant is cleaning the <b>{Target}</b>";
			}

			// Token: 0x02003132 RID: 12594
			public class LIGHTWORKEFFICIENCYBONUS
			{
				// Token: 0x0400CD2F RID: 52527
				public static LocString NAME = "Lit Workspace";

				// Token: 0x0400CD30 RID: 52528
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Better visibility from the ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" is allowing this Duplicant to work faster:\n    {0}"
				});

				// Token: 0x0400CD31 RID: 52529
				public static LocString NO_BUILDING_WORK_ATTRIBUTE = "{0} Speed";
			}

			// Token: 0x02003133 RID: 12595
			public class LABORATORYWORKEFFICIENCYBONUS
			{
				// Token: 0x0400CD32 RID: 52530
				public static LocString NAME = "Lab Workspace";

				// Token: 0x0400CD33 RID: 52531
				public static LocString TOOLTIP = "Working in a Laboratory is allowing this Duplicant to work faster:\n    {0}";

				// Token: 0x0400CD34 RID: 52532
				public static LocString NO_BUILDING_WORK_ATTRIBUTE = "{0} Speed";
			}

			// Token: 0x02003134 RID: 12596
			public class PICKINGUP
			{
				// Token: 0x0400CD35 RID: 52533
				public static LocString NAME = "Picking up {Target}";

				// Token: 0x0400CD36 RID: 52534
				public static LocString TOOLTIP = "This Duplicant is retrieving <b>{Target}</b>";
			}

			// Token: 0x02003135 RID: 12597
			public class MOPPING
			{
				// Token: 0x0400CD37 RID: 52535
				public static LocString NAME = "Mopping";

				// Token: 0x0400CD38 RID: 52536
				public static LocString TOOLTIP = "This Duplicant is cleaning up a nasty spill";
			}

			// Token: 0x02003136 RID: 12598
			public class ARTING
			{
				// Token: 0x0400CD39 RID: 52537
				public static LocString NAME = "Decorating";

				// Token: 0x0400CD3A RID: 52538
				public static LocString TOOLTIP = "This Duplicant is hard at work on their art";
			}

			// Token: 0x02003137 RID: 12599
			public class MUSHING
			{
				// Token: 0x0400CD3B RID: 52539
				public static LocString NAME = "Mushing {Item}";

				// Token: 0x0400CD3C RID: 52540
				public static LocString TOOLTIP = "This Duplicant is cooking a <b>{Item}</b>";
			}

			// Token: 0x02003138 RID: 12600
			public class COOKING
			{
				// Token: 0x0400CD3D RID: 52541
				public static LocString NAME = "Cooking {Item}";

				// Token: 0x0400CD3E RID: 52542
				public static LocString TOOLTIP = "This Duplicant is cooking up a tasty <b>{Item}</b>";
			}

			// Token: 0x02003139 RID: 12601
			public class RESEARCHING
			{
				// Token: 0x0400CD3F RID: 52543
				public static LocString NAME = "Researching {Tech}";

				// Token: 0x0400CD40 RID: 52544
				public static LocString TOOLTIP = "This Duplicant is intently researching <b>{Tech}</b> technology";
			}

			// Token: 0x0200313A RID: 12602
			public class RESEARCHING_FROM_POI
			{
				// Token: 0x0400CD41 RID: 52545
				public static LocString NAME = "Unlocking Research";

				// Token: 0x0400CD42 RID: 52546
				public static LocString TOOLTIP = "This Duplicant is unlocking crucial technology";
			}

			// Token: 0x0200313B RID: 12603
			public class MISSIONCONTROLLING
			{
				// Token: 0x0400CD43 RID: 52547
				public static LocString NAME = "Mission Controlling";

				// Token: 0x0400CD44 RID: 52548
				public static LocString TOOLTIP = "This Duplicant is guiding a " + UI.PRE_KEYWORD + "Rocket" + UI.PST_KEYWORD;
			}

			// Token: 0x0200313C RID: 12604
			public class STORING
			{
				// Token: 0x0400CD45 RID: 52549
				public static LocString NAME = "Storing {Item}";

				// Token: 0x0400CD46 RID: 52550
				public static LocString TOOLTIP = "This Duplicant is putting <b>{Item}</b> away in <b>{Target}</b>";
			}

			// Token: 0x0200313D RID: 12605
			public class LOADINGELECTROBANK
			{
				// Token: 0x0400CD47 RID: 52551
				public static LocString NAME = "Loading {Item}";

				// Token: 0x0400CD48 RID: 52552
				public static LocString TOOLTIP = "This Duplicant is loading an <b>{Item}</b> into a <b>{Target}</b>";
			}

			// Token: 0x0200313E RID: 12606
			public class BUILDING
			{
				// Token: 0x0400CD49 RID: 52553
				public static LocString NAME = "Building {Target}";

				// Token: 0x0400CD4A RID: 52554
				public static LocString TOOLTIP = "This Duplicant is constructing a <b>{Target}</b>";
			}

			// Token: 0x0200313F RID: 12607
			public class EQUIPPING
			{
				// Token: 0x0400CD4B RID: 52555
				public static LocString NAME = "Equipping {Target}";

				// Token: 0x0400CD4C RID: 52556
				public static LocString TOOLTIP = "This Duplicant is equipping a <b>{Target}</b>";
			}

			// Token: 0x02003140 RID: 12608
			public class WARMINGUP
			{
				// Token: 0x0400CD4D RID: 52557
				public static LocString NAME = "Warming up";

				// Token: 0x0400CD4E RID: 52558
				public static LocString TOOLTIP = "This Duplicant got too cold and is trying to warm up";
			}

			// Token: 0x02003141 RID: 12609
			public class GENERATINGPOWER
			{
				// Token: 0x0400CD4F RID: 52559
				public static LocString NAME = "Generating power";

				// Token: 0x0400CD50 RID: 52560
				public static LocString TOOLTIP = "This Duplicant is using the <b>{Target}</b> to produce electrical " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
			}

			// Token: 0x02003142 RID: 12610
			public class HARVESTING
			{
				// Token: 0x0400CD51 RID: 52561
				public static LocString NAME = "Harvesting {Target}";

				// Token: 0x0400CD52 RID: 52562
				public static LocString TOOLTIP = "This Duplicant is gathering resources from a <b>{Target}</b>";
			}

			// Token: 0x02003143 RID: 12611
			public class UPROOTING
			{
				// Token: 0x0400CD53 RID: 52563
				public static LocString NAME = "Uprooting {Target}";

				// Token: 0x0400CD54 RID: 52564
				public static LocString TOOLTIP = "This Duplicant is digging up a <b>{Target}</b>";
			}

			// Token: 0x02003144 RID: 12612
			public class EMPTYING
			{
				// Token: 0x0400CD55 RID: 52565
				public static LocString NAME = "Emptying {Target}";

				// Token: 0x0400CD56 RID: 52566
				public static LocString TOOLTIP = "This Duplicant is removing materials from the <b>{Target}</b>";
			}

			// Token: 0x02003145 RID: 12613
			public class TOGGLING
			{
				// Token: 0x0400CD57 RID: 52567
				public static LocString NAME = "Change {Target} setting";

				// Token: 0x0400CD58 RID: 52568
				public static LocString TOOLTIP = "This Duplicant is changing the <b>{Target}</b>'s setting";
			}

			// Token: 0x02003146 RID: 12614
			public class DECONSTRUCTING
			{
				// Token: 0x0400CD59 RID: 52569
				public static LocString NAME = "Deconstructing {Target}";

				// Token: 0x0400CD5A RID: 52570
				public static LocString TOOLTIP = "This Duplicant is deconstructing the <b>{Target}</b>";
			}

			// Token: 0x02003147 RID: 12615
			public class DEMOLISHING
			{
				// Token: 0x0400CD5B RID: 52571
				public static LocString NAME = "Demolishing {Target}";

				// Token: 0x0400CD5C RID: 52572
				public static LocString TOOLTIP = "This Duplicant is demolishing the <b>{Target}</b>";
			}

			// Token: 0x02003148 RID: 12616
			public class DISINFECTING
			{
				// Token: 0x0400CD5D RID: 52573
				public static LocString NAME = "Disinfecting {Target}";

				// Token: 0x0400CD5E RID: 52574
				public static LocString TOOLTIP = "This Duplicant is disinfecting <b>{Target}</b>";
			}

			// Token: 0x02003149 RID: 12617
			public class FABRICATING
			{
				// Token: 0x0400CD5F RID: 52575
				public static LocString NAME = "Fabricating {Item}";

				// Token: 0x0400CD60 RID: 52576
				public static LocString TOOLTIP = "This Duplicant is crafting a <b>{Item}</b>";
			}

			// Token: 0x0200314A RID: 12618
			public class PROCESSING
			{
				// Token: 0x0400CD61 RID: 52577
				public static LocString NAME = "Refining {Item}";

				// Token: 0x0400CD62 RID: 52578
				public static LocString TOOLTIP = "This Duplicant is refining <b>{Item}</b>";
			}

			// Token: 0x0200314B RID: 12619
			public class SPICING
			{
				// Token: 0x0400CD63 RID: 52579
				public static LocString NAME = "Spicing Food";

				// Token: 0x0400CD64 RID: 52580
				public static LocString TOOLTIP = "This Duplicant is making a tasty meal even tastier";
			}

			// Token: 0x0200314C RID: 12620
			public class CLEARING
			{
				// Token: 0x0400CD65 RID: 52581
				public static LocString NAME = "Sweeping {Target}";

				// Token: 0x0400CD66 RID: 52582
				public static LocString TOOLTIP = "This Duplicant is sweeping away <b>{Target}</b>";
			}

			// Token: 0x0200314D RID: 12621
			public class STUDYING
			{
				// Token: 0x0400CD67 RID: 52583
				public static LocString NAME = "Analyzing";

				// Token: 0x0400CD68 RID: 52584
				public static LocString TOOLTIP = "This Duplicant is conducting a field study of a Natural Feature";
			}

			// Token: 0x0200314E RID: 12622
			public class INSTALLINGELECTROBANK
			{
				// Token: 0x0400CD69 RID: 52585
				public static LocString NAME = "Rescuing Bionic Friend";

				// Token: 0x0400CD6A RID: 52586
				public static LocString TOOLTIP = "This Duplicant is rebooting a powerless Bionic Duplicant";
			}

			// Token: 0x0200314F RID: 12623
			public class SOCIALIZING
			{
				// Token: 0x0400CD6B RID: 52587
				public static LocString NAME = "Socializing";

				// Token: 0x0400CD6C RID: 52588
				public static LocString TOOLTIP = "This Duplicant is using their break to hang out";
			}

			// Token: 0x02003150 RID: 12624
			public class BIONICEXPLORERBOOSTER
			{
				// Token: 0x0400CD6D RID: 52589
				public static LocString NOTIFICATION_NAME = "Dowsing Complete: Geyser Discovered";

				// Token: 0x0400CD6E RID: 52590
				public static LocString NOTIFICATION_TOOLTIP = "Click to see the geyser recently discovered by a Bionic Duplicant";

				// Token: 0x0400CD6F RID: 52591
				public static LocString NAME = "Dowsing {0}";

				// Token: 0x0400CD70 RID: 52592
				public static LocString TOOLTIP = "This Duplicant's always gathering geodata\n\nWhen dowsing is complete, a new geyser will be revealed in the world";
			}

			// Token: 0x02003151 RID: 12625
			public class MINGLING
			{
				// Token: 0x0400CD71 RID: 52593
				public static LocString NAME = "Mingling";

				// Token: 0x0400CD72 RID: 52594
				public static LocString TOOLTIP = "This Duplicant is using their break to chat with friends";
			}

			// Token: 0x02003152 RID: 12626
			public class NOISEPEACEFUL
			{
				// Token: 0x0400CD73 RID: 52595
				public static LocString NAME = "Peace and Quiet";

				// Token: 0x0400CD74 RID: 52596
				public static LocString TOOLTIP = "This Duplicant has found a quiet place to concentrate";
			}

			// Token: 0x02003153 RID: 12627
			public class NOISEMINOR
			{
				// Token: 0x0400CD75 RID: 52597
				public static LocString NAME = "Loud Noises";

				// Token: 0x0400CD76 RID: 52598
				public static LocString TOOLTIP = "This area is a bit too loud for comfort";
			}

			// Token: 0x02003154 RID: 12628
			public class NOISEMAJOR
			{
				// Token: 0x0400CD77 RID: 52599
				public static LocString NAME = "Cacophony!";

				// Token: 0x0400CD78 RID: 52600
				public static LocString TOOLTIP = "It's very, very loud in here!";
			}

			// Token: 0x02003155 RID: 12629
			public class LOWIMMUNITY
			{
				// Token: 0x0400CD79 RID: 52601
				public static LocString NAME = "Under the Weather";

				// Token: 0x0400CD7A RID: 52602
				public static LocString TOOLTIP = "This Duplicant has a weakened immune system and will become ill if it reaches zero";

				// Token: 0x0400CD7B RID: 52603
				public static LocString NOTIFICATION_NAME = "Low Immunity";

				// Token: 0x0400CD7C RID: 52604
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are at risk of becoming sick:";
			}

			// Token: 0x02003156 RID: 12630
			public abstract class TINKERING
			{
				// Token: 0x0400CD7D RID: 52605
				public static LocString NAME = "Tinkering";

				// Token: 0x0400CD7E RID: 52606
				public static LocString TOOLTIP = "This Duplicant is making functional improvements to a building";
			}

			// Token: 0x02003157 RID: 12631
			public class CONTACTWITHGERMS
			{
				// Token: 0x0400CD7F RID: 52607
				public static LocString NAME = "Contact with {Sickness} Germs";

				// Token: 0x0400CD80 RID: 52608
				public static LocString TOOLTIP = "This Duplicant has encountered {Sickness} Germs and is at risk of dangerous exposure if contact continues\n\n<i>" + UI.CLICK(UI.ClickType.Click) + " to jump to last contact location</i>";
			}

			// Token: 0x02003158 RID: 12632
			public class EXPOSEDTOGERMS
			{
				// Token: 0x0400CD81 RID: 52609
				public static LocString TIER1 = "Mild Exposure";

				// Token: 0x0400CD82 RID: 52610
				public static LocString TIER2 = "Medium Exposure";

				// Token: 0x0400CD83 RID: 52611
				public static LocString TIER3 = "Exposure";

				// Token: 0x0400CD84 RID: 52612
				public static readonly LocString[] EXPOSURE_TIERS = new LocString[]
				{
					DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.TIER1,
					DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.TIER2,
					DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.TIER3
				};

				// Token: 0x0400CD85 RID: 52613
				public static LocString NAME = "{Severity} to {Sickness} Germs";

				// Token: 0x0400CD86 RID: 52614
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has been exposed to a concentration of {Sickness} Germs and is at risk of waking up sick on their next shift\n\nExposed {Source}\n\nRate of Contracting {Sickness}: {Chance}\n\nResistance Rating: {Total}\n    • Base {Sickness} Resistance: {Base}\n    • ",
					DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.NAME,
					": {Dupe}\n    • {Severity} Exposure: {ExposureLevelBonus}\n\n<i>",
					UI.CLICK(UI.ClickType.Click),
					" to jump to last exposure location</i>"
				});
			}

			// Token: 0x02003159 RID: 12633
			public class GASLIQUIDEXPOSURE
			{
				// Token: 0x0400CD87 RID: 52615
				public static LocString NAME_MINOR = "Eye Irritation";

				// Token: 0x0400CD88 RID: 52616
				public static LocString NAME_MAJOR = "Major Eye Irritation";

				// Token: 0x0400CD89 RID: 52617
				public static LocString TOOLTIP = "Ah, it stings!\n\nThis poor Duplicant got a faceful of an irritating gas or liquid";

				// Token: 0x0400CD8A RID: 52618
				public static LocString TOOLTIP_EXPOSED = "Current exposure to {element} is {rate} eye irritation";

				// Token: 0x0400CD8B RID: 52619
				public static LocString TOOLTIP_RATE_INCREASE = "increasing";

				// Token: 0x0400CD8C RID: 52620
				public static LocString TOOLTIP_RATE_DECREASE = "decreasing";

				// Token: 0x0400CD8D RID: 52621
				public static LocString TOOLTIP_RATE_STAYS = "maintaining";

				// Token: 0x0400CD8E RID: 52622
				public static LocString TOOLTIP_EXPOSURE_LEVEL = "Time Remaining: {time}";
			}

			// Token: 0x0200315A RID: 12634
			public class BEINGPRODUCTIVE
			{
				// Token: 0x0400CD8F RID: 52623
				public static LocString NAME = "Super Focused";

				// Token: 0x0400CD90 RID: 52624
				public static LocString TOOLTIP = "This Duplicant is focused on being super productive right now";
			}

			// Token: 0x0200315B RID: 12635
			public class BALLOONARTISTPLANNING
			{
				// Token: 0x0400CD91 RID: 52625
				public static LocString NAME = "Balloon Artist";

				// Token: 0x0400CD92 RID: 52626
				public static LocString TOOLTIP = "This Duplicant is planning to hand out balloons in their downtime";
			}

			// Token: 0x0200315C RID: 12636
			public class BALLOONARTISTHANDINGOUT
			{
				// Token: 0x0400CD93 RID: 52627
				public static LocString NAME = "Balloon Artist";

				// Token: 0x0400CD94 RID: 52628
				public static LocString TOOLTIP = "This Duplicant is handing out balloons to other Duplicants";
			}

			// Token: 0x0200315D RID: 12637
			public class EXPELLINGRADS
			{
				// Token: 0x0400CD95 RID: 52629
				public static LocString NAME = "Cleansing Rads";

				// Token: 0x0400CD96 RID: 52630
				public static LocString TOOLTIP = "This Duplicant is, uh... \"expelling\" absorbed radiation from their system";
			}

			// Token: 0x0200315E RID: 12638
			public class ANALYZINGGENES
			{
				// Token: 0x0400CD97 RID: 52631
				public static LocString NAME = "Analyzing Plant Genes";

				// Token: 0x0400CD98 RID: 52632
				public static LocString TOOLTIP = "This Duplicant is peering deep into the genetic code of an odd seed";
			}

			// Token: 0x0200315F RID: 12639
			public class ANALYZINGARTIFACT
			{
				// Token: 0x0400CD99 RID: 52633
				public static LocString NAME = "Analyzing Artifact";

				// Token: 0x0400CD9A RID: 52634
				public static LocString TOOLTIP = "This Duplicant is studying an artifact";
			}

			// Token: 0x02003160 RID: 12640
			public class RANCHING
			{
				// Token: 0x0400CD9B RID: 52635
				public static LocString NAME = "Ranching";

				// Token: 0x0400CD9C RID: 52636
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is tending to a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"'s well-being"
				});
			}

			// Token: 0x02003161 RID: 12641
			public class CARVING
			{
				// Token: 0x0400CD9D RID: 52637
				public static LocString NAME = "Carving {Target}";

				// Token: 0x0400CD9E RID: 52638
				public static LocString TOOLTIP = "This Duplicant is carving away at a <b>{Target}</b>";
			}

			// Token: 0x02003162 RID: 12642
			public class DATARAINERPLANNING
			{
				// Token: 0x0400CD9F RID: 52639
				public static LocString NAME = "Rainmaker";

				// Token: 0x0400CDA0 RID: 52640
				public static LocString TOOLTIP = "This Duplicant is planning to dish out microchips in their downtime";
			}

			// Token: 0x02003163 RID: 12643
			public class DATARAINERRAINING
			{
				// Token: 0x0400CDA1 RID: 52641
				public static LocString NAME = "Rainmaker";

				// Token: 0x0400CDA2 RID: 52642
				public static LocString TOOLTIP = "This Duplicant is making it \"rain\" microchips";
			}

			// Token: 0x02003164 RID: 12644
			public class ROBODANCERPLANNING
			{
				// Token: 0x0400CDA3 RID: 52643
				public static LocString NAME = "Flash Mobber";

				// Token: 0x0400CDA4 RID: 52644
				public static LocString TOOLTIP = "This Duplicant is planning to show off their dance moves in their downtime";
			}

			// Token: 0x02003165 RID: 12645
			public class ROBODANCERDANCING
			{
				// Token: 0x0400CDA5 RID: 52645
				public static LocString NAME = "Flash Mobber";

				// Token: 0x0400CDA6 RID: 52646
				public static LocString TOOLTIP = "This Duplicant is showing off their dance moves to other Duplicants";
			}

			// Token: 0x02003166 RID: 12646
			public class BIONICCRITICALBATTERY
			{
				// Token: 0x0400CDA7 RID: 52647
				public static LocString NAME = "Critical Power Level";

				// Token: 0x0400CDA8 RID: 52648
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" is dangerously low\n\nThey will become incapacitated unless new ",
					UI.PRE_KEYWORD,
					"Power Banks",
					UI.PST_KEYWORD,
					" are delivered"
				});

				// Token: 0x0400CDA9 RID: 52649
				public static LocString NOTIFICATION_NAME = "Critical Power Level";

				// Token: 0x0400CDAA RID: 52650
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants will become incapacitated if they can't find ",
					UI.PRE_KEYWORD,
					"Power Banks",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02003167 RID: 12647
			public class REMOTEWORKER
			{
				// Token: 0x02003820 RID: 14368
				public class ENTERINGDOCK
				{
					// Token: 0x0400DE2C RID: 56876
					public static LocString NAME = "Docking";

					// Token: 0x0400DE2D RID: 56877
					public static LocString TOOLTIP = "This remote worker is entering its dock";
				}

				// Token: 0x02003821 RID: 14369
				public class UNREACHABLEDOCK
				{
					// Token: 0x0400DE2E RID: 56878
					public static LocString NAME = "Unreachable Dock";

					// Token: 0x0400DE2F RID: 56879
					public static LocString TOOLTIP = "This remote worker cannot reach its dock";
				}

				// Token: 0x02003822 RID: 14370
				public class NOHOMEDOCK
				{
					// Token: 0x0400DE30 RID: 56880
					public static LocString NAME = "No Dock";

					// Token: 0x0400DE31 RID: 56881
					public static LocString TOOLTIP = "This remote worker has no home dock and will self-destruct";
				}

				// Token: 0x02003823 RID: 14371
				public class POWERSTATUS
				{
					// Token: 0x0400DE32 RID: 56882
					public static LocString NAME = "[{CHARGE} Power Remaining ({RATIO})]";

					// Token: 0x0400DE33 RID: 56883
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker has {CHARGE} remaining power\n\nWhen ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						" gets low, it will return to its dock to recharge"
					});
				}

				// Token: 0x02003824 RID: 14372
				public class LOWPOWER
				{
					// Token: 0x0400DE34 RID: 56884
					public static LocString NAME = "Low Power";

					// Token: 0x0400DE35 RID: 56885
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker has low ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						"\n\nIt will recharge at its dock before accepting new chores"
					});
				}

				// Token: 0x02003825 RID: 14373
				public class OUTOFPOWER
				{
					// Token: 0x0400DE36 RID: 56886
					public static LocString NAME = "No Power";

					// Token: 0x0400DE37 RID: 56887
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker cannot function without ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						"\n\nIt must be returned to its dock"
					});
				}

				// Token: 0x02003826 RID: 14374
				public class HIGHGUNK
				{
					// Token: 0x0400DE38 RID: 56888
					public static LocString NAME = "Gunk Clogged";

					// Token: 0x0400DE39 RID: 56889
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker will dock to remove ",
						UI.PRE_KEYWORD,
						"Gunk",
						UI.PST_KEYWORD,
						" buildup before accepting new chores"
					});
				}

				// Token: 0x02003827 RID: 14375
				public class FULLGUNK
				{
					// Token: 0x0400DE3A RID: 56890
					public static LocString NAME = "Full of Gunk";

					// Token: 0x0400DE3B RID: 56891
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker cannot function due to excessive ",
						UI.PRE_KEYWORD,
						"Gunk",
						UI.PST_KEYWORD,
						" buildup\n\nIt must be returned to its dock"
					});
				}

				// Token: 0x02003828 RID: 14376
				public class LOWOIL
				{
					// Token: 0x0400DE3C RID: 56892
					public static LocString NAME = "Low Oil";

					// Token: 0x0400DE3D RID: 56893
					public static LocString TOOLTIP = "This remote worker is low on oil\n\nIt will dock to refuel before accepting new chores";
				}

				// Token: 0x02003829 RID: 14377
				public class OUTOFOIL
				{
					// Token: 0x0400DE3E RID: 56894
					public static LocString NAME = "No Oil";

					// Token: 0x0400DE3F RID: 56895
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker cannot function without ",
						UI.PRE_KEYWORD,
						"Oil",
						UI.PST_KEYWORD,
						"\n\nIt must be returned to its dock"
					});
				}

				// Token: 0x0200382A RID: 14378
				public class RECHARGING
				{
					// Token: 0x0400DE40 RID: 56896
					public static LocString NAME = "Recharging";

					// Token: 0x0400DE41 RID: 56897
					public static LocString TOOLTIP = "This remote worker is recharging its capacitor";
				}

				// Token: 0x0200382B RID: 14379
				public class OILING
				{
					// Token: 0x0400DE42 RID: 56898
					public static LocString NAME = "Refilling Oil";

					// Token: 0x0400DE43 RID: 56899
					public static LocString TOOLTIP = "This remote worker is lubricating its joints";
				}

				// Token: 0x0200382C RID: 14380
				public class DRAINING
				{
					// Token: 0x0400DE44 RID: 56900
					public static LocString NAME = "Draining Gunk";

					// Token: 0x0400DE45 RID: 56901
					public static LocString TOOLTIP = "This remote worker is unclogging its gears";
				}
			}
		}

		// Token: 0x020021B4 RID: 8628
		public class DISEASES
		{
			// Token: 0x0400998E RID: 39310
			public static LocString CURED_POPUP = "Cured of {0}";

			// Token: 0x0400998F RID: 39311
			public static LocString INFECTED_POPUP = "Became infected by {0}";

			// Token: 0x04009990 RID: 39312
			public static LocString ADDED_POPFX = "{0}: {1} Germs";

			// Token: 0x04009991 RID: 39313
			public static LocString NOTIFICATION_TOOLTIP = "{0} contracted {1} from: {2}";

			// Token: 0x04009992 RID: 39314
			public static LocString GERMS = "Germs";

			// Token: 0x04009993 RID: 39315
			public static LocString GERMS_CONSUMED_DESCRIPTION = "A count of the number of germs this Duplicant is host to";

			// Token: 0x04009994 RID: 39316
			public static LocString RECUPERATING = "Recuperating";

			// Token: 0x04009995 RID: 39317
			public static LocString INFECTION_MODIFIER = "Recently consumed {0} ({1})";

			// Token: 0x04009996 RID: 39318
			public static LocString INFECTION_MODIFIER_SOURCE = "Fighting off {0} from {1}";

			// Token: 0x04009997 RID: 39319
			public static LocString INFECTED_MODIFIER = "Suppressed immune system";

			// Token: 0x04009998 RID: 39320
			public static LocString LEGEND_POSTAMBLE = "\n•  Select an infected object for more details";

			// Token: 0x04009999 RID: 39321
			public static LocString ATTRIBUTE_MODIFIER_SYMPTOMS = "{0}: {1}";

			// Token: 0x0400999A RID: 39322
			public static LocString ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP = "Modifies {0} by {1}";

			// Token: 0x0400999B RID: 39323
			public static LocString DEATH_SYMPTOM = "Death in {0} if untreated";

			// Token: 0x0400999C RID: 39324
			public static LocString DEATH_SYMPTOM_TOOLTIP = "Without medical treatment, this Duplicant will die of their illness in {0}";

			// Token: 0x0400999D RID: 39325
			public static LocString RESISTANCES_PANEL_TOOLTIP = "{0}";

			// Token: 0x0400999E RID: 39326
			public static LocString IMMUNE_FROM_MISSING_REQUIRED_TRAIT = "Immune: Does not have {0}";

			// Token: 0x0400999F RID: 39327
			public static LocString IMMUNE_FROM_HAVING_EXLCLUDED_TRAIT = "Immune: Has {0}";

			// Token: 0x040099A0 RID: 39328
			public static LocString IMMUNE_FROM_HAVING_EXCLUDED_EFFECT = "Immunity: Has {0}";

			// Token: 0x040099A1 RID: 39329
			public static LocString CONTRACTION_PROBABILITY = "{0} of {1}'s exposures to these germs will result in {2}";

			// Token: 0x02003168 RID: 12648
			public class STATUS_ITEM_TOOLTIP
			{
				// Token: 0x0400CDAB RID: 52651
				public static LocString TEMPLATE = "{InfectionSource}{Duration}{Doctor}{Fatality}{Cures}{Bedrest}\n\n\n{Symptoms}";

				// Token: 0x0400CDAC RID: 52652
				public static LocString DESCRIPTOR = "<b>{0} {1}</b>\n";

				// Token: 0x0400CDAD RID: 52653
				public static LocString SYMPTOMS = "{0}\n";

				// Token: 0x0400CDAE RID: 52654
				public static LocString INFECTION_SOURCE = "Contracted by: {0}\n";

				// Token: 0x0400CDAF RID: 52655
				public static LocString DURATION = "Time to recovery: {0}\n";

				// Token: 0x0400CDB0 RID: 52656
				public static LocString CURES = "Remedies taken: {0}\n";

				// Token: 0x0400CDB1 RID: 52657
				public static LocString NOMEDICINETAKEN = "Remedies taken: None\n";

				// Token: 0x0400CDB2 RID: 52658
				public static LocString FATALITY = "Fatal if untreated in: {0}\n";

				// Token: 0x0400CDB3 RID: 52659
				public static LocString BEDREST = "Sick Bay assignment will allow faster recovery\n";

				// Token: 0x0400CDB4 RID: 52660
				public static LocString DOCTOR_REQUIRED = "Sick Bay assignment required for recovery\n";

				// Token: 0x0400CDB5 RID: 52661
				public static LocString DOCTORED = "Received medical treatment, recovery speed is increased\n";
			}

			// Token: 0x02003169 RID: 12649
			public class MEDICINE
			{
				// Token: 0x0400CDB6 RID: 52662
				public static LocString SELF_ADMINISTERED_BOOSTER = "Self-Administered: Anytime";

				// Token: 0x0400CDB7 RID: 52663
				public static LocString SELF_ADMINISTERED_BOOSTER_TOOLTIP = "Duplicants can give themselves this medicine, whether they are currently sick or not";

				// Token: 0x0400CDB8 RID: 52664
				public static LocString SELF_ADMINISTERED_CURE = "Self-Administered: Sick Only";

				// Token: 0x0400CDB9 RID: 52665
				public static LocString SELF_ADMINISTERED_CURE_TOOLTIP = "Duplicants can give themselves this medicine, but only while they are sick";

				// Token: 0x0400CDBA RID: 52666
				public static LocString DOCTOR_ADMINISTERED_BOOSTER = "Doctor Administered: Anytime";

				// Token: 0x0400CDBB RID: 52667
				public static LocString DOCTOR_ADMINISTERED_BOOSTER_TOOLTIP = "Duplicants can receive this medicine at a {Station}, whether they are currently sick or not\n\nThey cannot give it to themselves and must receive it from a friend with " + UI.PRE_KEYWORD + "Doctoring Skills" + UI.PST_KEYWORD;

				// Token: 0x0400CDBC RID: 52668
				public static LocString DOCTOR_ADMINISTERED_CURE = "Doctor Administered: Sick Only";

				// Token: 0x0400CDBD RID: 52669
				public static LocString DOCTOR_ADMINISTERED_CURE_TOOLTIP = "Duplicants can receive this medicine at a {Station}, but only while they are sick\n\nThey cannot give it to themselves and must receive it from a friend with " + UI.PRE_KEYWORD + "Doctoring Skills" + UI.PST_KEYWORD;

				// Token: 0x0400CDBE RID: 52670
				public static LocString BOOSTER = UI.FormatAsLink("Immune Booster", "IMMUNE SYSTEM");

				// Token: 0x0400CDBF RID: 52671
				public static LocString BOOSTER_TOOLTIP = "Boosters can be taken by both healthy and sick Duplicants to prevent potential disease";

				// Token: 0x0400CDC0 RID: 52672
				public static LocString CURES_ANY = "Alleviates " + UI.FormatAsLink("All Diseases", "DISEASE");

				// Token: 0x0400CDC1 RID: 52673
				public static LocString CURES_ANY_TOOLTIP = string.Concat(new string[]
				{
					"This is a nonspecific ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" treatment that can be taken by any sick Duplicant"
				});

				// Token: 0x0400CDC2 RID: 52674
				public static LocString CURES = "Alleviates {0}";

				// Token: 0x0400CDC3 RID: 52675
				public static LocString CURES_TOOLTIP = "This medicine is used to treat {0} and can only be taken by sick Duplicants";
			}

			// Token: 0x0200316A RID: 12650
			public class SEVERITY
			{
				// Token: 0x0400CDC4 RID: 52676
				public static LocString BENIGN = "Benign";

				// Token: 0x0400CDC5 RID: 52677
				public static LocString MINOR = "Minor";

				// Token: 0x0400CDC6 RID: 52678
				public static LocString MAJOR = "Major";

				// Token: 0x0400CDC7 RID: 52679
				public static LocString CRITICAL = "Critical";
			}

			// Token: 0x0200316B RID: 12651
			public class TYPE
			{
				// Token: 0x0400CDC8 RID: 52680
				public static LocString PATHOGEN = "Illness";

				// Token: 0x0400CDC9 RID: 52681
				public static LocString AILMENT = "Ailment";

				// Token: 0x0400CDCA RID: 52682
				public static LocString INJURY = "Injury";
			}

			// Token: 0x0200316C RID: 12652
			public class TRIGGERS
			{
				// Token: 0x0400CDCB RID: 52683
				public static LocString EATCOMPLETEEDIBLE = "May cause {Diseases}";

				// Token: 0x0200382D RID: 14381
				public class TOOLTIPS
				{
					// Token: 0x0400DE46 RID: 56902
					public static LocString EATCOMPLETEEDIBLE = "May cause {Diseases}";
				}
			}

			// Token: 0x0200316D RID: 12653
			public class INFECTIONSOURCES
			{
				// Token: 0x0400CDCC RID: 52684
				public static LocString INTERNAL_TEMPERATURE = "Extreme internal temperatures";

				// Token: 0x0400CDCD RID: 52685
				public static LocString TOXIC_AREA = "Exposure to toxic areas";

				// Token: 0x0400CDCE RID: 52686
				public static LocString FOOD = "Eating a germ-covered {0}";

				// Token: 0x0400CDCF RID: 52687
				public static LocString AIR = "Breathing germ-filled {0}";

				// Token: 0x0400CDD0 RID: 52688
				public static LocString SKIN = "Skin contamination";

				// Token: 0x0400CDD1 RID: 52689
				public static LocString UNKNOWN = "Unknown source";
			}

			// Token: 0x0200316E RID: 12654
			public class DESCRIPTORS
			{
				// Token: 0x0200382E RID: 14382
				public class INFO
				{
					// Token: 0x0400DE47 RID: 56903
					public static LocString FOODBORNE = "Contracted via ingestion\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400DE48 RID: 56904
					public static LocString FOODBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" by ingesting ",
						UI.PRE_KEYWORD,
						"Food",
						UI.PST_KEYWORD,
						" contaminated with these ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD
					});

					// Token: 0x0400DE49 RID: 56905
					public static LocString AIRBORNE = "Contracted via inhalation\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400DE4A RID: 56906
					public static LocString AIRBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" by breathing ",
						ELEMENTS.OXYGEN.NAME,
						" containing these ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD
					});

					// Token: 0x0400DE4B RID: 56907
					public static LocString SKINBORNE = "Contracted via physical contact\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400DE4C RID: 56908
					public static LocString SKINBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" by touching objects contaminated with these ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD
					});

					// Token: 0x0400DE4D RID: 56909
					public static LocString SUNBORNE = "Contracted via environmental exposure\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400DE4E RID: 56910
					public static LocString SUNBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" through exposure to hazardous environments"
					});

					// Token: 0x0400DE4F RID: 56911
					public static LocString GROWS_ON = "Multiplies in:";

					// Token: 0x0400DE50 RID: 56912
					public static LocString GROWS_ON_TOOLTIP = string.Concat(new string[]
					{
						"These substances allow ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" to spread and reproduce"
					});

					// Token: 0x0400DE51 RID: 56913
					public static LocString NEUTRAL_ON = "Survives in:";

					// Token: 0x0400DE52 RID: 56914
					public static LocString NEUTRAL_ON_TOOLTIP = UI.PRE_KEYWORD + "Germs" + UI.PST_KEYWORD + " will survive contact with these substances, but will not reproduce";

					// Token: 0x0400DE53 RID: 56915
					public static LocString DIES_SLOWLY_ON = "Inhibited by:";

					// Token: 0x0400DE54 RID: 56916
					public static LocString DIES_SLOWLY_ON_TOOLTIP = string.Concat(new string[]
					{
						"Contact with these substances will slowly reduce ",
						UI.PRE_KEYWORD,
						"Germ",
						UI.PST_KEYWORD,
						" numbers"
					});

					// Token: 0x0400DE55 RID: 56917
					public static LocString DIES_ON = "Killed by:";

					// Token: 0x0400DE56 RID: 56918
					public static LocString DIES_ON_TOOLTIP = string.Concat(new string[]
					{
						"Contact with these substances kills ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" over time"
					});

					// Token: 0x0400DE57 RID: 56919
					public static LocString DIES_QUICKLY_ON = "Disinfected by:";

					// Token: 0x0400DE58 RID: 56920
					public static LocString DIES_QUICKLY_ON_TOOLTIP = "Contact with these substances will quickly kill these " + UI.PRE_KEYWORD + "Germs" + UI.PST_KEYWORD;

					// Token: 0x0400DE59 RID: 56921
					public static LocString GROWS = "Multiplies";

					// Token: 0x0400DE5A RID: 56922
					public static LocString GROWS_TOOLTIP = "Doubles germ count every {0}";

					// Token: 0x0400DE5B RID: 56923
					public static LocString NEUTRAL = "Survives";

					// Token: 0x0400DE5C RID: 56924
					public static LocString NEUTRAL_TOOLTIP = "Germ count remains static";

					// Token: 0x0400DE5D RID: 56925
					public static LocString DIES_SLOWLY = "Inhibited";

					// Token: 0x0400DE5E RID: 56926
					public static LocString DIES_SLOWLY_TOOLTIP = "Halves germ count every {0}";

					// Token: 0x0400DE5F RID: 56927
					public static LocString DIES = "Dies";

					// Token: 0x0400DE60 RID: 56928
					public static LocString DIES_TOOLTIP = "Halves germ count every {0}";

					// Token: 0x0400DE61 RID: 56929
					public static LocString DIES_QUICKLY = "Disinfected";

					// Token: 0x0400DE62 RID: 56930
					public static LocString DIES_QUICKLY_TOOLTIP = "Halves germ count every {0}";

					// Token: 0x0400DE63 RID: 56931
					public static LocString GROWTH_FORMAT = "    • {0}";

					// Token: 0x0400DE64 RID: 56932
					public static LocString TEMPERATURE_RANGE = "Temperature range: {0} to {1}";

					// Token: 0x0400DE65 RID: 56933
					public static LocString TEMPERATURE_RANGE_TOOLTIP = string.Concat(new string[]
					{
						"These ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" can survive ",
						UI.PRE_KEYWORD,
						"Temperatures",
						UI.PST_KEYWORD,
						" between <b>{0}</b> and <b>{1}</b>\n\nThey thrive in ",
						UI.PRE_KEYWORD,
						"Temperatures",
						UI.PST_KEYWORD,
						" between <b>{2}</b> and <b>{3}</b>"
					});

					// Token: 0x0400DE66 RID: 56934
					public static LocString PRESSURE_RANGE = "Pressure range: {0} to {1}\n";

					// Token: 0x0400DE67 RID: 56935
					public static LocString PRESSURE_RANGE_TOOLTIP = string.Concat(new string[]
					{
						"These ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" can survive between <b>{0}</b> and <b>{1}</b> of pressure\n\nThey thrive in pressures between <b>{2}</b> and <b>{3}</b>"
					});
				}
			}

			// Token: 0x0200316F RID: 12655
			public class ALLDISEASES
			{
				// Token: 0x0400CDD2 RID: 52690
				public static LocString NAME = "All Diseases";
			}

			// Token: 0x02003170 RID: 12656
			public class NODISEASES
			{
				// Token: 0x0400CDD3 RID: 52691
				public static LocString NAME = "NO";
			}

			// Token: 0x02003171 RID: 12657
			public class FOODPOISONING
			{
				// Token: 0x0400CDD4 RID: 52692
				public static LocString NAME = UI.FormatAsLink("Food Poisoning", "FOODPOISONING");

				// Token: 0x0400CDD5 RID: 52693
				public static LocString LEGEND_HOVERTEXT = "Food Poisoning Germs present\n";

				// Token: 0x0400CDD6 RID: 52694
				public static LocString DESC = "Food and drinks tainted with Food Poisoning germs are unsafe to consume, as they cause vomiting and other...bodily unpleasantness.";
			}

			// Token: 0x02003172 RID: 12658
			public class SLIMELUNG
			{
				// Token: 0x0400CDD7 RID: 52695
				public static LocString NAME = UI.FormatAsLink("Slimelung", "SLIMELUNG");

				// Token: 0x0400CDD8 RID: 52696
				public static LocString LEGEND_HOVERTEXT = "Slimelung Germs present\n";

				// Token: 0x0400CDD9 RID: 52697
				public static LocString DESC = string.Concat(new string[]
				{
					"Slimelung germs are found in ",
					UI.FormatAsLink("Slime", "SLIMEMOLD"),
					" and ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					". Inhaling these germs can cause Duplicants to cough and struggle to breathe."
				});
			}

			// Token: 0x02003173 RID: 12659
			public class POLLENGERMS
			{
				// Token: 0x0400CDDA RID: 52698
				public static LocString NAME = UI.FormatAsLink("Floral Scent", "POLLENGERMS");

				// Token: 0x0400CDDB RID: 52699
				public static LocString LEGEND_HOVERTEXT = "Floral Scent allergens present\n";

				// Token: 0x0400CDDC RID: 52700
				public static LocString DESC = "Floral Scent allergens trigger excessive sneezing fits in Duplicants who possess the Allergies trait.";
			}

			// Token: 0x02003174 RID: 12660
			public class ZOMBIESPORES
			{
				// Token: 0x0400CDDD RID: 52701
				public static LocString NAME = UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES");

				// Token: 0x0400CDDE RID: 52702
				public static LocString LEGEND_HOVERTEXT = "Zombie Spores present\n";

				// Token: 0x0400CDDF RID: 52703
				public static LocString DESC = "Zombie Spores are a parasitic brain fungus released by " + UI.FormatAsLink("Sporechids", "EVIL_FLOWER") + ". Duplicants who touch or inhale the spores risk becoming infected and temporarily losing motor control.";
			}

			// Token: 0x02003175 RID: 12661
			public class RADIATIONPOISONING
			{
				// Token: 0x0400CDE0 RID: 52704
				public static LocString NAME = UI.FormatAsLink("Radioactive Contamination", "RADIATIONPOISONING");

				// Token: 0x0400CDE1 RID: 52705
				public static LocString LEGEND_HOVERTEXT = "Radioactive contamination present\n";

				// Token: 0x0400CDE2 RID: 52706
				public static LocString DESC = string.Concat(new string[]
				{
					"Items tainted with Radioactive Contaminants emit low levels of ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" that can cause ",
					UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
					". They are unaffected by pressure or temperature, but do degrade over time."
				});
			}

			// Token: 0x02003176 RID: 12662
			public class FOODSICKNESS
			{
				// Token: 0x0400CDE3 RID: 52707
				public static LocString NAME = UI.FormatAsLink("Food Poisoning", "FOODSICKNESS");

				// Token: 0x0400CDE4 RID: 52708
				public static LocString DESCRIPTION = "This Duplicant's last meal wasn't exactly food safe";

				// Token: 0x0400CDE5 RID: 52709
				public static LocString VOMIT_SYMPTOM = "Vomiting";

				// Token: 0x0400CDE6 RID: 52710
				public static LocString VOMIT_SYMPTOM_TOOLTIP = string.Concat(new string[]
				{
					"Duplicants periodically vomit throughout the day, producing additional ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" and losing ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD
				});

				// Token: 0x0400CDE7 RID: 52711
				public static LocString DESCRIPTIVE_SYMPTOMS = "Nonlethal. A Duplicant's body \"purges\" from both ends, causing extreme fatigue.";

				// Token: 0x0400CDE8 RID: 52712
				public static LocString DISEASE_SOURCE_DESCRIPTOR = "Currently infected with {2}.\n\nThis Duplicant will produce {1} when vomiting.";

				// Token: 0x0400CDE9 RID: 52713
				public static LocString DISEASE_SOURCE_DESCRIPTOR_TOOLTIP = "This Duplicant will vomit approximately every <b>{0}</b>\n\nEach time they vomit, they will release <b>{1}</b> and lose " + UI.PRE_KEYWORD + "Calories" + UI.PST_KEYWORD;
			}

			// Token: 0x02003177 RID: 12663
			public class SLIMESICKNESS
			{
				// Token: 0x0400CDEA RID: 52714
				public static LocString NAME = UI.FormatAsLink("Slimelung", "SLIMESICKNESS");

				// Token: 0x0400CDEB RID: 52715
				public static LocString DESCRIPTION = "This Duplicant's chest congestion is making it difficult to breathe";

				// Token: 0x0400CDEC RID: 52716
				public static LocString COUGH_SYMPTOM = "Coughing";

				// Token: 0x0400CDED RID: 52717
				public static LocString COUGH_SYMPTOM_TOOLTIP = string.Concat(new string[]
				{
					"Duplicants periodically cough up ",
					ELEMENTS.CONTAMINATEDOXYGEN.NAME,
					", producing additional ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD
				});

				// Token: 0x0400CDEE RID: 52718
				public static LocString DESCRIPTIVE_SYMPTOMS = "Lethal without medical treatment. Duplicants experience coughing and shortness of breath.";

				// Token: 0x0400CDEF RID: 52719
				public static LocString DISEASE_SOURCE_DESCRIPTOR = "Currently infected with {2}.\n\nThis Duplicant will produce <b>{1}</b> when coughing.";

				// Token: 0x0400CDF0 RID: 52720
				public static LocString DISEASE_SOURCE_DESCRIPTOR_TOOLTIP = "This Duplicant will cough approximately every <b>{0}</b>\n\nEach time they cough, they will release <b>{1}</b>";
			}

			// Token: 0x02003178 RID: 12664
			public class ZOMBIESICKNESS
			{
				// Token: 0x0400CDF1 RID: 52721
				public static LocString NAME = UI.FormatAsLink("Zombie Spores", "ZOMBIESICKNESS");

				// Token: 0x0400CDF2 RID: 52722
				public static LocString DESCRIPTIVE_SYMPTOMS = "Duplicants lose much of their motor control and experience extreme discomfort.";

				// Token: 0x0400CDF3 RID: 52723
				public static LocString DESCRIPTION = "Fungal spores have infiltrated the Duplicant's head and are sending unnatural electrical impulses to their brain";

				// Token: 0x0400CDF4 RID: 52724
				public static LocString LEGEND_HOVERTEXT = "Area Causes Zombie Spores\n";
			}

			// Token: 0x02003179 RID: 12665
			public class ALLERGIES
			{
				// Token: 0x0400CDF5 RID: 52725
				public static LocString NAME = UI.FormatAsLink("Allergic Reaction", "ALLERGIES");

				// Token: 0x0400CDF6 RID: 52726
				public static LocString DESCRIPTIVE_SYMPTOMS = "Allergens cause excessive sneezing fits";

				// Token: 0x0400CDF7 RID: 52727
				public static LocString DESCRIPTION = "Pollen and other irritants are causing this poor Duplicant's immune system to overreact, resulting in needless sneezing and congestion";
			}

			// Token: 0x0200317A RID: 12666
			public class SUNBURNSICKNESS
			{
				// Token: 0x0400CDF8 RID: 52728
				public static LocString NAME = UI.FormatAsLink("Sunburn", "SUNBURNSICKNESS");

				// Token: 0x0400CDF9 RID: 52729
				public static LocString DESCRIPTION = "Extreme sun exposure has given this Duplicant a nasty burn.";

				// Token: 0x0400CDFA RID: 52730
				public static LocString LEGEND_HOVERTEXT = "Area Causes Sunburn\n";

				// Token: 0x0400CDFB RID: 52731
				public static LocString SUNEXPOSURE = "Sun Exposure";

				// Token: 0x0400CDFC RID: 52732
				public static LocString DESCRIPTIVE_SYMPTOMS = "Nonlethal. Duplicants experience temporary discomfort due to dermatological damage.";
			}

			// Token: 0x0200317B RID: 12667
			public class RADIATIONSICKNESS
			{
				// Token: 0x0400CDFD RID: 52733
				public static LocString NAME = UI.FormatAsLink("Radioactive Contaminants", "RADIATIONSICKNESS");

				// Token: 0x0400CDFE RID: 52734
				public static LocString DESCRIPTIVE_SYMPTOMS = "Extremely lethal. This Duplicant is not expected to survive.";

				// Token: 0x0400CDFF RID: 52735
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"This Duplicant is leaving a trail of ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" behind them."
				});

				// Token: 0x0400CE00 RID: 52736
				public static LocString LEGEND_HOVERTEXT = "Area Causes Radiation Sickness\n";

				// Token: 0x0400CE01 RID: 52737
				public static LocString DESC = DUPLICANTS.DISEASES.RADIATIONPOISONING.DESC;
			}

			// Token: 0x0200317C RID: 12668
			public class PUTRIDODOUR
			{
				// Token: 0x0400CE02 RID: 52738
				public static LocString NAME = UI.FormatAsLink("Trench Stench", "PUTRIDODOUR");

				// Token: 0x0400CE03 RID: 52739
				public static LocString DESCRIPTION = "\nThe pungent odor wafting off this Duplicant is nauseating to their peers";

				// Token: 0x0400CE04 RID: 52740
				public static LocString CRINGE_EFFECT = "Smelled a putrid odor";

				// Token: 0x0400CE05 RID: 52741
				public static LocString LEGEND_HOVERTEXT = "Trench Stench Germs Present\n";
			}
		}

		// Token: 0x020021B5 RID: 8629
		public class MODIFIERS
		{
			// Token: 0x040099A2 RID: 39330
			public static LocString MODIFIER_FORMAT = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + ": {1}";

			// Token: 0x040099A3 RID: 39331
			public static LocString IMMUNITY_FORMAT = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

			// Token: 0x040099A4 RID: 39332
			public static LocString TIME_REMAINING = "Time Remaining: {0}";

			// Token: 0x040099A5 RID: 39333
			public static LocString TIME_TOTAL = "\nDuration: {0}";

			// Token: 0x040099A6 RID: 39334
			public static LocString EFFECT_IMMUNITIES_HEADER = UI.PRE_POS_MODIFIER + "Immune to:" + UI.PST_POS_MODIFIER;

			// Token: 0x040099A7 RID: 39335
			public static LocString EFFECT_HEADER = UI.PRE_POS_MODIFIER + "Effects:" + UI.PST_POS_MODIFIER;

			// Token: 0x0200317D RID: 12669
			public class SKILLLEVEL
			{
				// Token: 0x0400CE06 RID: 52742
				public static LocString NAME = "Skill Level";
			}

			// Token: 0x0200317E RID: 12670
			public class ROOMPARK
			{
				// Token: 0x0400CE07 RID: 52743
				public static LocString NAME = "Park";

				// Token: 0x0400CE08 RID: 52744
				public static LocString TOOLTIP = "This Duplicant recently passed through a Park\n\nWow, nature sure is neat!";
			}

			// Token: 0x0200317F RID: 12671
			public class ROOMNATURERESERVE
			{
				// Token: 0x0400CE09 RID: 52745
				public static LocString NAME = "Nature Reserve";

				// Token: 0x0400CE0A RID: 52746
				public static LocString TOOLTIP = "This Duplicant recently passed through a splendid Nature Reserve\n\nWow, nature sure is neat!";
			}

			// Token: 0x02003180 RID: 12672
			public class ROOMLATRINE
			{
				// Token: 0x0400CE0B RID: 52747
				public static LocString NAME = "Latrine";

				// Token: 0x0400CE0C RID: 52748
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant used an ",
					BUILDINGS.PREFABS.OUTHOUSE.NAME,
					" in a ",
					UI.PRE_KEYWORD,
					"Latrine",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003181 RID: 12673
			public class ROOMBATHROOM
			{
				// Token: 0x0400CE0D RID: 52749
				public static LocString NAME = "Washroom";

				// Token: 0x0400CE0E RID: 52750
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant used a ",
					BUILDINGS.PREFABS.FLUSHTOILET.NAME,
					" in a ",
					UI.PRE_KEYWORD,
					"Washroom",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003182 RID: 12674
			public class ROOMBIONICUPKEEP
			{
				// Token: 0x0400CE0F RID: 52751
				public static LocString NAME = "Workshop";

				// Token: 0x0400CE10 RID: 52752
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant used a ",
					BUILDINGS.PREFABS.GUNKEMPTIER.NAME,
					" in a ",
					UI.PRE_KEYWORD,
					"Workshop",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003183 RID: 12675
			public class FRESHOIL
			{
				// Token: 0x0400CE11 RID: 52753
				public static LocString NAME = "Fresh Oil";

				// Token: 0x0400CE12 RID: 52754
				public static LocString TOOLTIP = "This Duplicant recently used a " + BUILDINGS.PREFABS.OILCHANGER.NAME + " and feels pretty slick" + UI.PST_KEYWORD;
			}

			// Token: 0x02003184 RID: 12676
			public class ROOMBARRACKS
			{
				// Token: 0x0400CE13 RID: 52755
				public static LocString NAME = "Barracks";

				// Token: 0x0400CE14 RID: 52756
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slept in the ",
					UI.PRE_KEYWORD,
					"Barracks",
					UI.PST_KEYWORD,
					" last night and feels refreshed"
				});
			}

			// Token: 0x02003185 RID: 12677
			public class ROOMBEDROOM
			{
				// Token: 0x0400CE15 RID: 52757
				public static LocString NAME = "Luxury Barracks";

				// Token: 0x0400CE16 RID: 52758
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slept in a ",
					UI.PRE_KEYWORD,
					"Luxury Barracks",
					UI.PST_KEYWORD,
					" last night and feels extra refreshed"
				});
			}

			// Token: 0x02003186 RID: 12678
			public class ROOMPRIVATEBEDROOM
			{
				// Token: 0x0400CE17 RID: 52759
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400CE18 RID: 52760
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slept in a ",
					UI.PRE_KEYWORD,
					"Private Bedroom",
					UI.PST_KEYWORD,
					" last night and feels super refreshed"
				});
			}

			// Token: 0x02003187 RID: 12679
			public class BEDHEALTH
			{
				// Token: 0x0400CE19 RID: 52761
				public static LocString NAME = "Bed Rest";

				// Token: 0x0400CE1A RID: 52762
				public static LocString TOOLTIP = "This Duplicant will incrementally heal over while on " + UI.PRE_KEYWORD + "Bed Rest" + UI.PST_KEYWORD;
			}

			// Token: 0x02003188 RID: 12680
			public class BEDSTAMINA
			{
				// Token: 0x0400CE1B RID: 52763
				public static LocString NAME = "Sleeping in a cot";

				// Token: 0x0400CE1C RID: 52764
				public static LocString TOOLTIP = "This Duplicant's sleeping arrangements are adequate";
			}

			// Token: 0x02003189 RID: 12681
			public class LUXURYBEDSTAMINA
			{
				// Token: 0x0400CE1D RID: 52765
				public static LocString NAME = "Sleeping in a comfy bed";

				// Token: 0x0400CE1E RID: 52766
				public static LocString TOOLTIP = "This Duplicant loves their snuggly bed";
			}

			// Token: 0x0200318A RID: 12682
			public class BARRACKSSTAMINA
			{
				// Token: 0x0400CE1F RID: 52767
				public static LocString NAME = "Barracks";

				// Token: 0x0400CE20 RID: 52768
				public static LocString TOOLTIP = "This Duplicant shares sleeping quarters with others";
			}

			// Token: 0x0200318B RID: 12683
			public class LADDERBEDSTAMINA
			{
				// Token: 0x0400CE21 RID: 52769
				public static LocString NAME = "Sleeping in a ladder bed";

				// Token: 0x0400CE22 RID: 52770
				public static LocString TOOLTIP = "This Duplicant's sleeping arrangements are adequate";
			}

			// Token: 0x0200318C RID: 12684
			public class BEDROOMSTAMINA
			{
				// Token: 0x0400CE23 RID: 52771
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400CE24 RID: 52772
				public static LocString TOOLTIP = "This lucky Duplicant has their own private bedroom";
			}

			// Token: 0x0200318D RID: 12685
			public class ROOMMESSHALL
			{
				// Token: 0x0400CE25 RID: 52773
				public static LocString NAME = "Mess Hall";

				// Token: 0x0400CE26 RID: 52774
				public static LocString TOOLTIP = "This Duplicant's most recent meal was eaten in a " + UI.PRE_KEYWORD + "Mess Hall" + UI.PST_KEYWORD;
			}

			// Token: 0x0200318E RID: 12686
			public class ROOMGREATHALL
			{
				// Token: 0x0400CE27 RID: 52775
				public static LocString NAME = "Great Hall";

				// Token: 0x0400CE28 RID: 52776
				public static LocString TOOLTIP = "This Duplicant's most recent meal was eaten in a fancy " + UI.PRE_KEYWORD + "Great Hall" + UI.PST_KEYWORD;
			}

			// Token: 0x0200318F RID: 12687
			public class ENTITLEMENT
			{
				// Token: 0x0400CE29 RID: 52777
				public static LocString NAME = "Entitlement";

				// Token: 0x0400CE2A RID: 52778
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will demand better ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" and accommodations with each Expertise level they gain"
				});
			}

			// Token: 0x02003190 RID: 12688
			public class HOMEOSTASIS
			{
				// Token: 0x0400CE2B RID: 52779
				public static LocString NAME = "Homeostasis";
			}

			// Token: 0x02003191 RID: 12689
			public class WARMAIR
			{
				// Token: 0x0400CE2C RID: 52780
				public static LocString NAME = "Toasty Surroundings";
			}

			// Token: 0x02003192 RID: 12690
			public class COLDAIR
			{
				// Token: 0x0400CE2D RID: 52781
				public static LocString NAME = "Chilly Surroundings";

				// Token: 0x0400CE2E RID: 52782
				public static LocString CAUSE = "Duplicants tire quickly and lose body heat in cold environments";
			}

			// Token: 0x02003193 RID: 12691
			public class CLAUSTROPHOBIC
			{
				// Token: 0x0400CE2F RID: 52783
				public static LocString NAME = "Claustrophobic";

				// Token: 0x0400CE30 RID: 52784
				public static LocString TOOLTIP = "This Duplicant recently found themselves in an upsettingly cramped space";

				// Token: 0x0400CE31 RID: 52785
				public static LocString CAUSE = "This Duplicant got so good at their job that they became claustrophobic";
			}

			// Token: 0x02003194 RID: 12692
			public class VERTIGO
			{
				// Token: 0x0400CE32 RID: 52786
				public static LocString NAME = "Vertigo";

				// Token: 0x0400CE33 RID: 52787
				public static LocString TOOLTIP = "This Duplicant had to climb a tall ladder that left them dizzy and unsettled";

				// Token: 0x0400CE34 RID: 52788
				public static LocString CAUSE = "This Duplicant got so good at their job they became bad at ladders";
			}

			// Token: 0x02003195 RID: 12693
			public class UNCOMFORTABLEFEET
			{
				// Token: 0x0400CE35 RID: 52789
				public static LocString NAME = "Aching Feet";

				// Token: 0x0400CE36 RID: 52790
				public static LocString TOOLTIP = "This Duplicant recently walked across floor without tile, much to their chagrin";

				// Token: 0x0400CE37 RID: 52791
				public static LocString CAUSE = "This Duplicant got so good at their job that their feet became sensitive";
			}

			// Token: 0x02003196 RID: 12694
			public class PEOPLETOOCLOSEWHILESLEEPING
			{
				// Token: 0x0400CE38 RID: 52792
				public static LocString NAME = "Personal Bubble Burst";

				// Token: 0x0400CE39 RID: 52793
				public static LocString TOOLTIP = "This Duplicant had to sleep too close to others and it was awkward for them";

				// Token: 0x0400CE3A RID: 52794
				public static LocString CAUSE = "This Duplicant got so good at their job that they stopped being comfortable sleeping near other people";
			}

			// Token: 0x02003197 RID: 12695
			public class RESTLESS
			{
				// Token: 0x0400CE3B RID: 52795
				public static LocString NAME = "Restless";

				// Token: 0x0400CE3C RID: 52796
				public static LocString TOOLTIP = "This Duplicant went a few minutes without working and is now completely awash with guilt";

				// Token: 0x0400CE3D RID: 52797
				public static LocString CAUSE = "This Duplicant got so good at their job that they forgot how to be comfortable doing anything else";
			}

			// Token: 0x02003198 RID: 12696
			public class UNFASHIONABLECLOTHING
			{
				// Token: 0x0400CE3E RID: 52798
				public static LocString NAME = "Fashion Crime";

				// Token: 0x0400CE3F RID: 52799
				public static LocString TOOLTIP = "This Duplicant had to wear something that was an affront to fashion";

				// Token: 0x0400CE40 RID: 52800
				public static LocString CAUSE = "This Duplicant got so good at their job that they became incapable of tolerating unfashionable clothing";
			}

			// Token: 0x02003199 RID: 12697
			public class BURNINGCALORIES
			{
				// Token: 0x0400CE41 RID: 52801
				public static LocString NAME = "Homeostasis";
			}

			// Token: 0x0200319A RID: 12698
			public class EATINGCALORIES
			{
				// Token: 0x0400CE42 RID: 52802
				public static LocString NAME = "Eating";
			}

			// Token: 0x0200319B RID: 12699
			public class TEMPEXCHANGE
			{
				// Token: 0x0400CE43 RID: 52803
				public static LocString NAME = "Environmental Exchange";
			}

			// Token: 0x0200319C RID: 12700
			public class CLOTHING
			{
				// Token: 0x0400CE44 RID: 52804
				public static LocString NAME = "Clothing";
			}

			// Token: 0x0200319D RID: 12701
			public class CRYFACE
			{
				// Token: 0x0400CE45 RID: 52805
				public static LocString NAME = "Cry Face";

				// Token: 0x0400CE46 RID: 52806
				public static LocString TOOLTIP = "This Duplicant recently had a crying fit and it shows";

				// Token: 0x0400CE47 RID: 52807
				public static LocString CAUSE = string.Concat(new string[]
				{
					"Obtained from the ",
					UI.PRE_KEYWORD,
					"Ugly Crier",
					UI.PST_KEYWORD,
					" stress reaction"
				});
			}

			// Token: 0x0200319E RID: 12702
			public class WARMTOUCH
			{
				// Token: 0x0400CE48 RID: 52808
				public static LocString NAME = "Frost Resistant";

				// Token: 0x0400CE49 RID: 52809
				public static LocString TOOLTIP = "This Duplicant recently visited a warming station, sauna, or hot tub\n\nThey are impervious to cold as a result";

				// Token: 0x0400CE4A RID: 52810
				public static LocString PROVIDERS_NAME = "Frost Resistance";

				// Token: 0x0400CE4B RID: 52811
				public static LocString PROVIDERS_TOOLTIP = string.Concat(new string[]
				{
					"Using this building provides temporary immunity to ",
					UI.PRE_KEYWORD,
					"Chilly Surroundings",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Soggy Feet",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200319F RID: 12703
			public class REFRESHINGTOUCH
			{
				// Token: 0x0400CE4C RID: 52812
				public static LocString NAME = "Heat Resistant";

				// Token: 0x0400CE4D RID: 52813
				public static LocString TOOLTIP = "This Duplicant recently visited a cooling station and is totally unbothered by heat as a result";
			}

			// Token: 0x020031A0 RID: 12704
			public class GUNKSICK
			{
				// Token: 0x0400CE4E RID: 52814
				public static LocString NAME = "Gunk Extraction Required";

				// Token: 0x0400CE4F RID: 52815
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant needs to visit a ",
					UI.PRE_KEYWORD,
					"Gunk Extractor",
					UI.PST_KEYWORD,
					" as soon as possible\n\nThey will use a toilet as a last resort"
				});
			}

			// Token: 0x020031A1 RID: 12705
			public class EXPELLINGGUNK
			{
				// Token: 0x0400CE50 RID: 52816
				public static LocString NAME = "Making a mess";

				// Token: 0x0400CE51 RID: 52817
				public static LocString TOOLTIP = "This Duplicant just couldn't hold it all in anymore";
			}

			// Token: 0x020031A2 RID: 12706
			public class CLOGGINGTOILET
			{
				// Token: 0x0400CE52 RID: 52818
				public static LocString NAME = "Clogging a toilet";

				// Token: 0x0400CE53 RID: 52819
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is clogging a toilet with ",
					UI.PRE_KEYWORD,
					"Liquid Gunk",
					UI.PST_KEYWORD,
					"\n\nThey really should have used a ",
					UI.PRE_KEYWORD,
					"Gunk Extractor",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020031A3 RID: 12707
			public class GUNKHUNGOVER
			{
				// Token: 0x0400CE54 RID: 52820
				public static LocString NAME = "Gunk Mouth";

				// Token: 0x0400CE55 RID: 52821
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently expelled built-up ",
					UI.PRE_KEYWORD,
					"Liquid Gunk",
					UI.PST_KEYWORD,
					" and can still taste it"
				});
			}

			// Token: 0x020031A4 RID: 12708
			public class NOLUBRICATION
			{
				// Token: 0x0400CE56 RID: 52822
				public static LocString NAME = "Grinding Gears";

				// Token: 0x0400CE57 RID: 52823
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's out of ",
					UI.PRE_KEYWORD,
					"Oil",
					UI.PST_KEYWORD,
					" and cannot function properly\n\nThey need to visit a ",
					UI.PRE_KEYWORD,
					"Lubrication Station",
					UI.PST_KEYWORD,
					" as soon as possible"
				});
			}

			// Token: 0x020031A5 RID: 12709
			public class BIONICOFFLINE
			{
				// Token: 0x0400CE58 RID: 52824
				public static LocString NAME = "Powerless";

				// Token: 0x0400CE59 RID: 52825
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is non-functional!\n\nDeliver a charged ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" and reboot their systems to revive them"
				});
			}

			// Token: 0x020031A6 RID: 12710
			public class WATERDAMAGE
			{
				// Token: 0x0400CE5A RID: 52826
				public static LocString NAME = "Liquid Damage";

				// Token: 0x0400CE5B RID: 52827
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's bionic parts recently came into contact with incompatible ",
					UI.PRE_KEYWORD,
					"Liquids",
					UI.PST_KEYWORD,
					"\n\nProlonged exposure could have serious ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					" consequences"
				});
			}

			// Token: 0x020031A7 RID: 12711
			public class SLIPPED
			{
				// Token: 0x0400CE5C RID: 52828
				public static LocString NAME = "Slipped";

				// Token: 0x0400CE5D RID: 52829
				public static LocString TOOLTIP = "This Duplicant recently lost their footing on a slippery floor and feels embarrassed";
			}

			// Token: 0x020031A8 RID: 12712
			public class BIONICBATTERYSAVEMODE
			{
				// Token: 0x0400CE5E RID: 52830
				public static LocString NAME = "Peaceful Processing";

				// Token: 0x0400CE5F RID: 52831
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is enjoying a reprieve from the demands of high-wattage life\n\n",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is reduced while in Standby Mode"
				});
			}

			// Token: 0x020031A9 RID: 12713
			public class DUPLICANTGOTMILK
			{
				// Token: 0x0400CE60 RID: 52832
				public static LocString NAME = "Extra Hydrated";

				// Token: 0x0400CE61 RID: 52833
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently drank ",
					UI.PRE_KEYWORD,
					"Brackene",
					UI.PST_KEYWORD,
					". It's helping them relax"
				});
			}

			// Token: 0x020031AA RID: 12714
			public class SOILEDSUIT
			{
				// Token: 0x0400CE62 RID: 52834
				public static LocString NAME = "Soiled Suit";

				// Token: 0x0400CE63 RID: 52835
				public static LocString TOOLTIP = "This Duplicant's suit needs to be emptied of waste\n\n(Preferably soon)";

				// Token: 0x0400CE64 RID: 52836
				public static LocString CAUSE = "Obtained when a Duplicant wears a suit filled with... \"fluids\"";
			}

			// Token: 0x020031AB RID: 12715
			public class SHOWERED
			{
				// Token: 0x0400CE65 RID: 52837
				public static LocString NAME = "Showered";

				// Token: 0x0400CE66 RID: 52838
				public static LocString TOOLTIP = "This Duplicant recently had a shower and feels squeaky clean!";
			}

			// Token: 0x020031AC RID: 12716
			public class SOREBACK
			{
				// Token: 0x0400CE67 RID: 52839
				public static LocString NAME = "Sore Back";

				// Token: 0x0400CE68 RID: 52840
				public static LocString TOOLTIP = "This Duplicant feels achy from sleeping on the floor last night and would like a bed";

				// Token: 0x0400CE69 RID: 52841
				public static LocString CAUSE = "Obtained by sleeping on the ground";
			}

			// Token: 0x020031AD RID: 12717
			public class GOODEATS
			{
				// Token: 0x0400CE6A RID: 52842
				public static LocString NAME = "Soul Food";

				// Token: 0x0400CE6B RID: 52843
				public static LocString TOOLTIP = "This Duplicant had a yummy home cooked meal and is totally stuffed";

				// Token: 0x0400CE6C RID: 52844
				public static LocString CAUSE = "Obtained by eating a hearty home cooked meal";

				// Token: 0x0400CE6D RID: 52845
				public static LocString DESCRIPTION = "Duplicants find this home cooked meal is emotionally comforting";
			}

			// Token: 0x020031AE RID: 12718
			public class HOTSTUFF
			{
				// Token: 0x0400CE6E RID: 52846
				public static LocString NAME = "Hot Stuff";

				// Token: 0x0400CE6F RID: 52847
				public static LocString TOOLTIP = "This Duplicant had an extremely spicy meal and is both exhilarated and a little " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;

				// Token: 0x0400CE70 RID: 52848
				public static LocString CAUSE = "Obtained by eating a very spicy meal";

				// Token: 0x0400CE71 RID: 52849
				public static LocString DESCRIPTION = "Duplicants find this spicy meal quite invigorating";
			}

			// Token: 0x020031AF RID: 12719
			public class WARMTOUCHFOOD
			{
				// Token: 0x0400CE72 RID: 52850
				public static LocString NAME = "Frost Resistant: Spicy Diet";

				// Token: 0x0400CE73 RID: 52851
				public static LocString TOOLTIP = "This Duplicant ate spicy food and feels so warm inside that they don't even notice the cold right now";

				// Token: 0x0400CE74 RID: 52852
				public static LocString CAUSE = "Obtained by eating a very spicy meal";

				// Token: 0x0400CE75 RID: 52853
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Eating this provides temporary immunity to ",
					UI.PRE_KEYWORD,
					"Chilly Surroundings",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Soggy Feet",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020031B0 RID: 12720
			public class SEAFOODRADIATIONRESISTANCE
			{
				// Token: 0x0400CE76 RID: 52854
				public static LocString NAME = "Radiation Resistant: Aquatic Diet";

				// Token: 0x0400CE77 RID: 52855
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant ate sea-grown foods, which boost ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" resistance"
				});

				// Token: 0x0400CE78 RID: 52856
				public static LocString CAUSE = "Obtained by eating sea-grown foods like fish or lettuce";

				// Token: 0x0400CE79 RID: 52857
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Eating this improves ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" resistance"
				});
			}

			// Token: 0x020031B1 RID: 12721
			public class RECENTLYPARTIED
			{
				// Token: 0x0400CE7A RID: 52858
				public static LocString NAME = "Partied Hard";

				// Token: 0x0400CE7B RID: 52859
				public static LocString TOOLTIP = "This Duplicant recently attended a great party!";
			}

			// Token: 0x020031B2 RID: 12722
			public class NOFUNALLOWED
			{
				// Token: 0x0400CE7C RID: 52860
				public static LocString NAME = "Fun Interrupted";

				// Token: 0x0400CE7D RID: 52861
				public static LocString TOOLTIP = "This Duplicant is upset a party was rejected";
			}

			// Token: 0x020031B3 RID: 12723
			public class CONTAMINATEDLUNGS
			{
				// Token: 0x0400CE7E RID: 52862
				public static LocString NAME = "Yucky Lungs";

				// Token: 0x0400CE7F RID: 52863
				public static LocString TOOLTIP = "This Duplicant got a big nasty lungful of " + ELEMENTS.CONTAMINATEDOXYGEN.NAME;
			}

			// Token: 0x020031B4 RID: 12724
			public class MINORIRRITATION
			{
				// Token: 0x0400CE80 RID: 52864
				public static LocString NAME = "Minor Eye Irritation";

				// Token: 0x0400CE81 RID: 52865
				public static LocString TOOLTIP = "A gas or liquid made this Duplicant's eyes sting a little";

				// Token: 0x0400CE82 RID: 52866
				public static LocString CAUSE = "Obtained by exposure to a harsh liquid or gas";
			}

			// Token: 0x020031B5 RID: 12725
			public class MAJORIRRITATION
			{
				// Token: 0x0400CE83 RID: 52867
				public static LocString NAME = "Major Eye Irritation";

				// Token: 0x0400CE84 RID: 52868
				public static LocString TOOLTIP = "Woah, something really messed up this Duplicant's eyes!\n\nCaused by exposure to a harsh liquid or gas";

				// Token: 0x0400CE85 RID: 52869
				public static LocString CAUSE = "Obtained by exposure to a harsh liquid or gas";
			}

			// Token: 0x020031B6 RID: 12726
			public class FRESH_AND_CLEAN
			{
				// Token: 0x0400CE86 RID: 52870
				public static LocString NAME = "Refreshingly Clean";

				// Token: 0x0400CE87 RID: 52871
				public static LocString TOOLTIP = "This Duplicant took a warm shower and it was great!";

				// Token: 0x0400CE88 RID: 52872
				public static LocString CAUSE = "Obtained by taking a comfortably heated shower";
			}

			// Token: 0x020031B7 RID: 12727
			public class BURNED_BY_SCALDING_WATER
			{
				// Token: 0x0400CE89 RID: 52873
				public static LocString NAME = "Scalded";

				// Token: 0x0400CE8A RID: 52874
				public static LocString TOOLTIP = "Ouch! This Duplicant showered or was doused in water that was way too hot";

				// Token: 0x0400CE8B RID: 52875
				public static LocString CAUSE = "Obtained by exposure to hot water";
			}

			// Token: 0x020031B8 RID: 12728
			public class STRESSED_BY_COLD_WATER
			{
				// Token: 0x0400CE8C RID: 52876
				public static LocString NAME = "Numb";

				// Token: 0x0400CE8D RID: 52877
				public static LocString TOOLTIP = "Brr! This Duplicant was showered or doused in water that was way too cold";

				// Token: 0x0400CE8E RID: 52878
				public static LocString CAUSE = "Obtained by exposure to icy water";
			}

			// Token: 0x020031B9 RID: 12729
			public class SMELLEDSTINKY
			{
				// Token: 0x0400CE8F RID: 52879
				public static LocString NAME = "Smelled Stinky";

				// Token: 0x0400CE90 RID: 52880
				public static LocString TOOLTIP = "This Duplicant got a whiff of a certain somebody";
			}

			// Token: 0x020031BA RID: 12730
			public class STRESSREDUCTION
			{
				// Token: 0x0400CE91 RID: 52881
				public static LocString NAME = "Receiving Massage";

				// Token: 0x0400CE92 RID: 52882
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is just melting away"
				});
			}

			// Token: 0x020031BB RID: 12731
			public class STRESSREDUCTION_CLINIC
			{
				// Token: 0x0400CE93 RID: 52883
				public static LocString NAME = "Receiving Clinic Massage";

				// Token: 0x0400CE94 RID: 52884
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Clinical facilities are improving the effectiveness of this massage\n\nThis Duplicant's ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is just melting away"
				});
			}

			// Token: 0x020031BC RID: 12732
			public class UGLY_CRYING
			{
				// Token: 0x0400CE95 RID: 52885
				public static LocString NAME = "Ugly Crying";

				// Token: 0x0400CE96 RID: 52886
				public static LocString TOOLTIP = "This Duplicant is having a cathartic ugly cry as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400CE97 RID: 52887
				public static LocString NOTIFICATION_NAME = "Ugly Crying";

				// Token: 0x0400CE98 RID: 52888
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they broke down crying:";
			}

			// Token: 0x020031BD RID: 12733
			public class BINGE_EATING
			{
				// Token: 0x0400CE99 RID: 52889
				public static LocString NAME = "Insatiable Hunger";

				// Token: 0x0400CE9A RID: 52890
				public static LocString TOOLTIP = "This Duplicant is stuffing their face as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400CE9B RID: 52891
				public static LocString NOTIFICATION_NAME = "Binge Eating";

				// Token: 0x0400CE9C RID: 52892
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they began overeating:";
			}

			// Token: 0x020031BE RID: 12734
			public class BANSHEE_WAILING
			{
				// Token: 0x0400CE9D RID: 52893
				public static LocString NAME = "Deafening Shriek";

				// Token: 0x0400CE9E RID: 52894
				public static LocString TOOLTIP = "This Duplicant is wailing at the top of their lungs as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400CE9F RID: 52895
				public static LocString NOTIFICATION_NAME = "Banshee Wailing";

				// Token: 0x0400CEA0 RID: 52896
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they began wailing:";
			}

			// Token: 0x020031BF RID: 12735
			public class STRESSSHOCKER
			{
				// Token: 0x0400CEA1 RID: 52897
				public static LocString NAME = "Deafening Shriek";

				// Token: 0x0400CEA2 RID: 52898
				public static LocString TOOLTIP = "This Duplicant is wailing at the top of their lungs as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400CEA3 RID: 52899
				public static LocString NOTIFICATION_NAME = "Banshee Wailing";

				// Token: 0x0400CEA4 RID: 52900
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they began wailing:";
			}

			// Token: 0x020031C0 RID: 12736
			public class BANSHEE_WAILING_RECOVERY
			{
				// Token: 0x0400CEA5 RID: 52901
				public static LocString NAME = "Guzzling Air";

				// Token: 0x0400CEA6 RID: 52902
				public static LocString TOOLTIP = "This Duplicant needs a little extra oxygen to catch their breath";
			}

			// Token: 0x020031C1 RID: 12737
			public class METABOLISM_CALORIE_MODIFIER
			{
				// Token: 0x0400CEA7 RID: 52903
				public static LocString NAME = "Metabolism";

				// Token: 0x0400CEA8 RID: 52904
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Metabolism",
					UI.PST_KEYWORD,
					" determines how quickly a critter burns ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020031C2 RID: 12738
			public class WORKING
			{
				// Token: 0x0400CEA9 RID: 52905
				public static LocString NAME = "Working";

				// Token: 0x0400CEAA RID: 52906
				public static LocString TOOLTIP = "This Duplicant is working up a sweat";
			}

			// Token: 0x020031C3 RID: 12739
			public class UNCOMFORTABLESLEEP
			{
				// Token: 0x0400CEAB RID: 52907
				public static LocString NAME = "Sleeping Uncomfortably";

				// Token: 0x0400CEAC RID: 52908
				public static LocString TOOLTIP = "This Duplicant collapsed on the floor from sheer exhaustion";
			}

			// Token: 0x020031C4 RID: 12740
			public class MANAGERIALDUTIES
			{
				// Token: 0x0400CEAD RID: 52909
				public static LocString NAME = "Managerial Duties";

				// Token: 0x0400CEAE RID: 52910
				public static LocString TOOLTIP = "Being a manager is stressful";
			}

			// Token: 0x020031C5 RID: 12741
			public class MANAGEDCOLONY
			{
				// Token: 0x0400CEAF RID: 52911
				public static LocString NAME = "Managed Colony";

				// Token: 0x0400CEB0 RID: 52912
				public static LocString TOOLTIP = "A Duplicant is in the colony manager job";
			}

			// Token: 0x020031C6 RID: 12742
			public class BIONIC_WATTS
			{
				// Token: 0x0400CEB1 RID: 52913
				public static LocString NAME = "Wattage";

				// Token: 0x0400CEB2 RID: 52914
				public static LocString ESTIMATED_LIFE_TIME_REMAINING = string.Concat(new string[]
				{
					"Estimated ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" lifetime remaining: {0}"
				});

				// Token: 0x0400CEB3 RID: 52915
				public static LocString CURRENT_WATTAGE_TOOLTIP_LABEL = "Current Wattage: {0}";

				// Token: 0x0400CEB4 RID: 52916
				public static LocString POTENTIAL_EXTRA_WATTAGE_TOOLTIP_LABEL = "Potential Wattage: {0}";

				// Token: 0x0400CEB5 RID: 52917
				public static LocString STANDARD_ACTIVE_TEMPLATE = "{0}: {1}";

				// Token: 0x0400CEB6 RID: 52918
				public static LocString STANDARD_INACTIVE_TEMPLATE = "{0}: {1}";

				// Token: 0x0400CEB7 RID: 52919
				public static LocString SAVING_MODE_TEMPLATE = "{0} ({1}): {2}";

				// Token: 0x0400CEB8 RID: 52920
				public static LocString BASE_NAME = "Base";

				// Token: 0x0400CEB9 RID: 52921
				public static LocString SAVING_MODE_NAME = "Standby Mode";
			}

			// Token: 0x020031C7 RID: 12743
			public class FLOORSLEEP
			{
				// Token: 0x0400CEBA RID: 52922
				public static LocString NAME = "Sleeping On Floor";

				// Token: 0x0400CEBB RID: 52923
				public static LocString TOOLTIP = "This Duplicant is uncomfortably recovering " + UI.PRE_KEYWORD + "Stamina" + UI.PST_KEYWORD;
			}

			// Token: 0x020031C8 RID: 12744
			public class PASSEDOUTSLEEP
			{
				// Token: 0x0400CEBC RID: 52924
				public static LocString NAME = "Exhausted";

				// Token: 0x0400CEBD RID: 52925
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Lack of rest depleted this Duplicant's ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					"\n\nThey passed out from the fatigue"
				});
			}

			// Token: 0x020031C9 RID: 12745
			public class SLEEP
			{
				// Token: 0x0400CEBE RID: 52926
				public static LocString NAME = "Sleeping";

				// Token: 0x0400CEBF RID: 52927
				public static LocString TOOLTIP = "This Duplicant is recovering " + UI.PRE_KEYWORD + "Stamina" + UI.PST_KEYWORD;
			}

			// Token: 0x020031CA RID: 12746
			public class SLEEPCLINIC
			{
				// Token: 0x0400CEC0 RID: 52928
				public static LocString NAME = "Nodding Off";

				// Token: 0x0400CEC1 RID: 52929
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is losing ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					" because of their ",
					UI.PRE_KEYWORD,
					"Pajamas",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020031CB RID: 12747
			public class RESTFULSLEEP
			{
				// Token: 0x0400CEC2 RID: 52930
				public static LocString NAME = "Sleeping Peacefully";

				// Token: 0x0400CEC3 RID: 52931
				public static LocString TOOLTIP = "This Duplicant is getting a good night's rest";
			}

			// Token: 0x020031CC RID: 12748
			public class SLEEPY
			{
				// Token: 0x0400CEC4 RID: 52932
				public static LocString NAME = "Sleepy";

				// Token: 0x0400CEC5 RID: 52933
				public static LocString TOOLTIP = "This Duplicant is getting tired";
			}

			// Token: 0x020031CD RID: 12749
			public class HUNGRY
			{
				// Token: 0x0400CEC6 RID: 52934
				public static LocString NAME = "Hungry";

				// Token: 0x0400CEC7 RID: 52935
				public static LocString TOOLTIP = "This Duplicant is ready for lunch";
			}

			// Token: 0x020031CE RID: 12750
			public class STARVING
			{
				// Token: 0x0400CEC8 RID: 52936
				public static LocString NAME = "Starving";

				// Token: 0x0400CEC9 RID: 52937
				public static LocString TOOLTIP = "This Duplicant needs to eat something, soon";
			}

			// Token: 0x020031CF RID: 12751
			public class HOT
			{
				// Token: 0x0400CECA RID: 52938
				public static LocString NAME = "Hot";

				// Token: 0x0400CECB RID: 52939
				public static LocString TOOLTIP = "This Duplicant is uncomfortably warm";
			}

			// Token: 0x020031D0 RID: 12752
			public class COLD
			{
				// Token: 0x0400CECC RID: 52940
				public static LocString NAME = "Cold";

				// Token: 0x0400CECD RID: 52941
				public static LocString TOOLTIP = "This Duplicant is uncomfortably cold";
			}

			// Token: 0x020031D1 RID: 12753
			public class CARPETFEET
			{
				// Token: 0x0400CECE RID: 52942
				public static LocString NAME = "Tickled Tootsies";

				// Token: 0x0400CECF RID: 52943
				public static LocString TOOLTIP = "Walking on carpet has made this Duplicant's day a little more luxurious";
			}

			// Token: 0x020031D2 RID: 12754
			public class WETFEET
			{
				// Token: 0x0400CED0 RID: 52944
				public static LocString NAME = "Soggy Feet";

				// Token: 0x0400CED1 RID: 52945
				public static LocString TOOLTIP = "This Duplicant recently stepped in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

				// Token: 0x0400CED2 RID: 52946
				public static LocString CAUSE = "Obtained by walking through liquid";
			}

			// Token: 0x020031D3 RID: 12755
			public class SOAKINGWET
			{
				// Token: 0x0400CED3 RID: 52947
				public static LocString NAME = "Sopping Wet";

				// Token: 0x0400CED4 RID: 52948
				public static LocString TOOLTIP = "This Duplicant was recently submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

				// Token: 0x0400CED5 RID: 52949
				public static LocString CAUSE = "Obtained from submergence in liquid";
			}

			// Token: 0x020031D4 RID: 12756
			public class POPPEDEARDRUMS
			{
				// Token: 0x0400CED6 RID: 52950
				public static LocString NAME = "Popped Eardrums";

				// Token: 0x0400CED7 RID: 52951
				public static LocString TOOLTIP = "This Duplicant was exposed to an over-pressurized area that popped their eardrums";
			}

			// Token: 0x020031D5 RID: 12757
			public class ANEWHOPE
			{
				// Token: 0x0400CED8 RID: 52952
				public static LocString NAME = "New Hope";

				// Token: 0x0400CED9 RID: 52953
				public static LocString TOOLTIP = "This Duplicant feels pretty optimistic about their new home";
			}

			// Token: 0x020031D6 RID: 12758
			public class MEGABRAINTANKBONUS
			{
				// Token: 0x0400CEDA RID: 52954
				public static LocString NAME = "Maximum Aptitude";

				// Token: 0x0400CEDB RID: 52955
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is smarter and stronger than usual thanks to the ",
					UI.PRE_KEYWORD,
					"Somnium Synthesizer",
					UI.PST_KEYWORD,
					" "
				});
			}

			// Token: 0x020031D7 RID: 12759
			public class PRICKLEFRUITDAMAGE
			{
				// Token: 0x0400CEDC RID: 52956
				public static LocString NAME = "Ouch!";

				// Token: 0x0400CEDD RID: 52957
				public static LocString TOOLTIP = "This Duplicant ate a raw " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " and it gave their mouth ouchies";
			}

			// Token: 0x020031D8 RID: 12760
			public class NOOXYGEN
			{
				// Token: 0x0400CEDE RID: 52958
				public static LocString NAME = "No Oxygen";

				// Token: 0x0400CEDF RID: 52959
				public static LocString TOOLTIP = "There is no breathable air in this area";
			}

			// Token: 0x020031D9 RID: 12761
			public class LOWOXYGEN
			{
				// Token: 0x0400CEE0 RID: 52960
				public static LocString NAME = "Low Oxygen";

				// Token: 0x0400CEE1 RID: 52961
				public static LocString TOOLTIP = "The air is thin in this area";
			}

			// Token: 0x020031DA RID: 12762
			public class MOURNING
			{
				// Token: 0x0400CEE2 RID: 52962
				public static LocString NAME = "Mourning";

				// Token: 0x0400CEE3 RID: 52963
				public static LocString TOOLTIP = "This Duplicant is grieving the loss of a friend";
			}

			// Token: 0x020031DB RID: 12763
			public class NARCOLEPTICSLEEP
			{
				// Token: 0x0400CEE4 RID: 52964
				public static LocString NAME = "Narcoleptic Nap";

				// Token: 0x0400CEE5 RID: 52965
				public static LocString TOOLTIP = "This Duplicant just needs to rest their eyes for a second";
			}

			// Token: 0x020031DC RID: 12764
			public class BADSLEEP
			{
				// Token: 0x0400CEE6 RID: 52966
				public static LocString NAME = "Unrested: Too Bright";

				// Token: 0x0400CEE7 RID: 52967
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant tossed and turned all night because a ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" was left on where they were trying to sleep"
				});
			}

			// Token: 0x020031DD RID: 12765
			public class BADSLEEPAFRAIDOFDARK
			{
				// Token: 0x0400CEE8 RID: 52968
				public static LocString NAME = "Unrested: Afraid of Dark";

				// Token: 0x0400CEE9 RID: 52969
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant didn't get much sleep because they were too anxious about the lack of ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" to relax"
				});
			}

			// Token: 0x020031DE RID: 12766
			public class BADSLEEPMOVEMENT
			{
				// Token: 0x0400CEEA RID: 52970
				public static LocString NAME = "Unrested: Bed Jostling";

				// Token: 0x0400CEEB RID: 52971
				public static LocString TOOLTIP = "This Duplicant was woken up when a friend climbed on their ladder bed";
			}

			// Token: 0x020031DF RID: 12767
			public class BADSLEEPCOLD
			{
				// Token: 0x0400CEEC RID: 52972
				public static LocString NAME = "Unrested: Cold Bedroom";

				// Token: 0x0400CEED RID: 52973
				public static LocString TOOLTIP = "This Duplicant was shivering instead of sleeping";
			}

			// Token: 0x020031E0 RID: 12768
			public class TERRIBLESLEEP
			{
				// Token: 0x0400CEEE RID: 52974
				public static LocString NAME = "Dead Tired: Snoring Friend";

				// Token: 0x0400CEEF RID: 52975
				public static LocString TOOLTIP = "This Duplicant didn't get any shuteye last night because of all the racket from a friend's snoring";
			}

			// Token: 0x020031E1 RID: 12769
			public class PEACEFULSLEEP
			{
				// Token: 0x0400CEF0 RID: 52976
				public static LocString NAME = "Well Rested";

				// Token: 0x0400CEF1 RID: 52977
				public static LocString TOOLTIP = "This Duplicant had a blissfully quiet sleep last night";
			}

			// Token: 0x020031E2 RID: 12770
			public class CENTEROFATTENTION
			{
				// Token: 0x0400CEF2 RID: 52978
				public static LocString NAME = "Center of Attention";

				// Token: 0x0400CEF3 RID: 52979
				public static LocString TOOLTIP = "This Duplicant feels like someone's watching over them...";
			}

			// Token: 0x020031E3 RID: 12771
			public class INSPIRED
			{
				// Token: 0x0400CEF4 RID: 52980
				public static LocString NAME = "Inspired";

				// Token: 0x0400CEF5 RID: 52981
				public static LocString TOOLTIP = "This Duplicant has had a creative vision!";
			}

			// Token: 0x020031E4 RID: 12772
			public class NEWCREWARRIVAL
			{
				// Token: 0x0400CEF6 RID: 52982
				public static LocString NAME = "New Friend";

				// Token: 0x0400CEF7 RID: 52983
				public static LocString TOOLTIP = "This Duplicant is happy to see a new face in the colony";
			}

			// Token: 0x020031E5 RID: 12773
			public class UNDERWATER
			{
				// Token: 0x0400CEF8 RID: 52984
				public static LocString NAME = "Underwater";

				// Token: 0x0400CEF9 RID: 52985
				public static LocString TOOLTIP = "This Duplicant's movement is slowed";
			}

			// Token: 0x020031E6 RID: 12774
			public class NIGHTMARES
			{
				// Token: 0x0400CEFA RID: 52986
				public static LocString NAME = "Nightmares";

				// Token: 0x0400CEFB RID: 52987
				public static LocString TOOLTIP = "This Duplicant was visited by something in the night";
			}

			// Token: 0x020031E7 RID: 12775
			public class WASATTACKED
			{
				// Token: 0x0400CEFC RID: 52988
				public static LocString NAME = "Recently assailed";

				// Token: 0x0400CEFD RID: 52989
				public static LocString TOOLTIP = "This Duplicant is stressed out after having been attacked";
			}

			// Token: 0x020031E8 RID: 12776
			public class LIGHTWOUNDS
			{
				// Token: 0x0400CEFE RID: 52990
				public static LocString NAME = "Light Wounds";

				// Token: 0x0400CEFF RID: 52991
				public static LocString TOOLTIP = "This Duplicant sustained injuries that are a bit uncomfortable";
			}

			// Token: 0x020031E9 RID: 12777
			public class MODERATEWOUNDS
			{
				// Token: 0x0400CF00 RID: 52992
				public static LocString NAME = "Moderate Wounds";

				// Token: 0x0400CF01 RID: 52993
				public static LocString TOOLTIP = "This Duplicant sustained injuries that are affecting their ability to work";
			}

			// Token: 0x020031EA RID: 12778
			public class SEVEREWOUNDS
			{
				// Token: 0x0400CF02 RID: 52994
				public static LocString NAME = "Severe Wounds";

				// Token: 0x0400CF03 RID: 52995
				public static LocString TOOLTIP = "This Duplicant sustained serious injuries that are impacting their work and well-being";
			}

			// Token: 0x020031EB RID: 12779
			public class LIGHTWOUNDSCRITTER
			{
				// Token: 0x0400CF04 RID: 52996
				public static LocString NAME = "Light Wounds";

				// Token: 0x0400CF05 RID: 52997
				public static LocString TOOLTIP = "This Critter sustained injuries that are a bit uncomfortable";
			}

			// Token: 0x020031EC RID: 12780
			public class MODERATEWOUNDSCRITTER
			{
				// Token: 0x0400CF06 RID: 52998
				public static LocString NAME = "Moderate Wounds";

				// Token: 0x0400CF07 RID: 52999
				public static LocString TOOLTIP = "This Critter sustained injuries that are really affecting their health";
			}

			// Token: 0x020031ED RID: 12781
			public class SEVEREWOUNDSCRITTER
			{
				// Token: 0x0400CF08 RID: 53000
				public static LocString NAME = "Severe Wounds";

				// Token: 0x0400CF09 RID: 53001
				public static LocString TOOLTIP = "This Critter sustained serious injuries that could prove life-threatening";
			}

			// Token: 0x020031EE RID: 12782
			public class SANDBOXMORALEADJUSTMENT
			{
				// Token: 0x0400CF0A RID: 53002
				public static LocString NAME = "Sandbox Morale Adjustment";

				// Token: 0x0400CF0B RID: 53003
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had their ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" temporarily adjusted using the Sandbox tools"
				});
			}

			// Token: 0x020031EF RID: 12783
			public class ROTTEMPERATURE
			{
				// Token: 0x0400CF0C RID: 53004
				public static LocString UNREFRIGERATED = "Unrefrigerated";

				// Token: 0x0400CF0D RID: 53005
				public static LocString REFRIGERATED = "Refrigerated";

				// Token: 0x0400CF0E RID: 53006
				public static LocString FROZEN = "Frozen";
			}

			// Token: 0x020031F0 RID: 12784
			public class ROTATMOSPHERE
			{
				// Token: 0x0400CF0F RID: 53007
				public static LocString CONTAMINATED = "Contaminated Air";

				// Token: 0x0400CF10 RID: 53008
				public static LocString NORMAL = "Normal Atmosphere";

				// Token: 0x0400CF11 RID: 53009
				public static LocString STERILE = "Sterile Atmosphere";
			}

			// Token: 0x020031F1 RID: 12785
			public class BASEROT
			{
				// Token: 0x0400CF12 RID: 53010
				public static LocString NAME = "Base Decay Rate";
			}

			// Token: 0x020031F2 RID: 12786
			public class FULLBLADDER
			{
				// Token: 0x0400CF13 RID: 53011
				public static LocString NAME = "Full Bladder";

				// Token: 0x0400CF14 RID: 53012
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's ",
					UI.PRE_KEYWORD,
					"Bladder",
					UI.PST_KEYWORD,
					" is full"
				});
			}

			// Token: 0x020031F3 RID: 12787
			public class DIARRHEA
			{
				// Token: 0x0400CF15 RID: 53013
				public static LocString NAME = "Diarrhea";

				// Token: 0x0400CF16 RID: 53014
				public static LocString TOOLTIP = "This Duplicant's gut is giving them some trouble";

				// Token: 0x0400CF17 RID: 53015
				public static LocString CAUSE = "Obtained by eating a disgusting meal";

				// Token: 0x0400CF18 RID: 53016
				public static LocString DESCRIPTION = "Most Duplicants experience stomach upset from this meal";
			}

			// Token: 0x020031F4 RID: 12788
			public class STRESSFULYEMPTYINGBLADDER
			{
				// Token: 0x0400CF19 RID: 53017
				public static LocString NAME = "Making a mess";

				// Token: 0x0400CF1A RID: 53018
				public static LocString TOOLTIP = "This Duplicant had no choice but to empty their " + UI.PRE_KEYWORD + "Bladder" + UI.PST_KEYWORD;
			}

			// Token: 0x020031F5 RID: 12789
			public class REDALERT
			{
				// Token: 0x0400CF1B RID: 53019
				public static LocString NAME = "Red Alert!";

				// Token: 0x0400CF1C RID: 53020
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The ",
					UI.PRE_KEYWORD,
					"Red Alert",
					UI.PST_KEYWORD,
					" is stressing this Duplicant out"
				});
			}

			// Token: 0x020031F6 RID: 12790
			public class FUSSY
			{
				// Token: 0x0400CF1D RID: 53021
				public static LocString NAME = "Fussy";

				// Token: 0x0400CF1E RID: 53022
				public static LocString TOOLTIP = "This Duplicant is hard to please";
			}

			// Token: 0x020031F7 RID: 12791
			public class WARMINGUP
			{
				// Token: 0x0400CF1F RID: 53023
				public static LocString NAME = "Warming Up";

				// Token: 0x0400CF20 RID: 53024
				public static LocString TOOLTIP = "This Duplicant is trying to warm back up";
			}

			// Token: 0x020031F8 RID: 12792
			public class COOLINGDOWN
			{
				// Token: 0x0400CF21 RID: 53025
				public static LocString NAME = "Cooling Down";

				// Token: 0x0400CF22 RID: 53026
				public static LocString TOOLTIP = "This Duplicant is trying to cool back down";
			}

			// Token: 0x020031F9 RID: 12793
			public class DARKNESS
			{
				// Token: 0x0400CF23 RID: 53027
				public static LocString NAME = "Darkness";

				// Token: 0x0400CF24 RID: 53028
				public static LocString TOOLTIP = "Eep! This Duplicant doesn't like being in the dark!";
			}

			// Token: 0x020031FA RID: 12794
			public class STEPPEDINCONTAMINATEDWATER
			{
				// Token: 0x0400CF25 RID: 53029
				public static LocString NAME = "Stepped in polluted water";

				// Token: 0x0400CF26 RID: 53030
				public static LocString TOOLTIP = "Gross! This Duplicant stepped in something yucky";
			}

			// Token: 0x020031FB RID: 12795
			public class WELLFED
			{
				// Token: 0x0400CF27 RID: 53031
				public static LocString NAME = "Well fed";

				// Token: 0x0400CF28 RID: 53032
				public static LocString TOOLTIP = "This Duplicant feels satisfied after having a big meal";
			}

			// Token: 0x020031FC RID: 12796
			public class STALEFOOD
			{
				// Token: 0x0400CF29 RID: 53033
				public static LocString NAME = "Bad leftovers";

				// Token: 0x0400CF2A RID: 53034
				public static LocString TOOLTIP = "This Duplicant is in a bad mood from having to eat stale " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x020031FD RID: 12797
			public class ATEFROZENFOOD
			{
				// Token: 0x0400CF2B RID: 53035
				public static LocString NAME = "Ate frozen food";

				// Token: 0x0400CF2C RID: 53036
				public static LocString TOOLTIP = "This Duplicant is in a bad mood from having to eat deep-frozen " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x020031FE RID: 12798
			public class SMELLEDPUTRIDODOUR
			{
				// Token: 0x0400CF2D RID: 53037
				public static LocString NAME = "Smelled a putrid odor";

				// Token: 0x0400CF2E RID: 53038
				public static LocString TOOLTIP = "This Duplicant got a whiff of something unspeakably foul";
			}

			// Token: 0x020031FF RID: 12799
			public class VOMITING
			{
				// Token: 0x0400CF2F RID: 53039
				public static LocString NAME = "Vomiting";

				// Token: 0x0400CF30 RID: 53040
				public static LocString TOOLTIP = "Better out than in, as they say";
			}

			// Token: 0x02003200 RID: 12800
			public class BREATHING
			{
				// Token: 0x0400CF31 RID: 53041
				public static LocString NAME = "Breathing";
			}

			// Token: 0x02003201 RID: 12801
			public class HOLDINGBREATH
			{
				// Token: 0x0400CF32 RID: 53042
				public static LocString NAME = "Holding breath";
			}

			// Token: 0x02003202 RID: 12802
			public class RECOVERINGBREATH
			{
				// Token: 0x0400CF33 RID: 53043
				public static LocString NAME = "Recovering breath";
			}

			// Token: 0x02003203 RID: 12803
			public class ROTTING
			{
				// Token: 0x0400CF34 RID: 53044
				public static LocString NAME = "Rotting";
			}

			// Token: 0x02003204 RID: 12804
			public class DEAD
			{
				// Token: 0x0400CF35 RID: 53045
				public static LocString NAME = "Dead";
			}

			// Token: 0x02003205 RID: 12805
			public class TOXICENVIRONMENT
			{
				// Token: 0x0400CF36 RID: 53046
				public static LocString NAME = "Toxic environment";
			}

			// Token: 0x02003206 RID: 12806
			public class RESTING
			{
				// Token: 0x0400CF37 RID: 53047
				public static LocString NAME = "Resting";
			}

			// Token: 0x02003207 RID: 12807
			public class INTRAVENOUS_NUTRITION
			{
				// Token: 0x0400CF38 RID: 53048
				public static LocString NAME = "Intravenous Feeding";
			}

			// Token: 0x02003208 RID: 12808
			public class CATHETERIZED
			{
				// Token: 0x0400CF39 RID: 53049
				public static LocString NAME = "Catheterized";

				// Token: 0x0400CF3A RID: 53050
				public static LocString TOOLTIP = "Let's leave it at that";
			}

			// Token: 0x02003209 RID: 12809
			public class NOISEPEACEFUL
			{
				// Token: 0x0400CF3B RID: 53051
				public static LocString NAME = "Peace and Quiet";

				// Token: 0x0400CF3C RID: 53052
				public static LocString TOOLTIP = "This Duplicant has found a quiet place to concentrate";
			}

			// Token: 0x0200320A RID: 12810
			public class NOISEMINOR
			{
				// Token: 0x0400CF3D RID: 53053
				public static LocString NAME = "Loud Noises";

				// Token: 0x0400CF3E RID: 53054
				public static LocString TOOLTIP = "This area is a bit too loud for comfort";
			}

			// Token: 0x0200320B RID: 12811
			public class NOISEMAJOR
			{
				// Token: 0x0400CF3F RID: 53055
				public static LocString NAME = "Cacophony!";

				// Token: 0x0400CF40 RID: 53056
				public static LocString TOOLTIP = "It's very, very loud in here!";
			}

			// Token: 0x0200320C RID: 12812
			public class MEDICALCOT
			{
				// Token: 0x0400CF41 RID: 53057
				public static LocString NAME = "Triage Cot Rest";

				// Token: 0x0400CF42 RID: 53058
				public static LocString TOOLTIP = "Bedrest is improving this Duplicant's physical recovery time";
			}

			// Token: 0x0200320D RID: 12813
			public class MEDICALCOTDOCTORED
			{
				// Token: 0x0400CF43 RID: 53059
				public static LocString NAME = "Receiving treatment";

				// Token: 0x0400CF44 RID: 53060
				public static LocString TOOLTIP = "This Duplicant is receiving treatment for their physical injuries";
			}

			// Token: 0x0200320E RID: 12814
			public class DOCTOREDOFFCOTEFFECT
			{
				// Token: 0x0400CF45 RID: 53061
				public static LocString NAME = "Runaway Patient";

				// Token: 0x0400CF46 RID: 53062
				public static LocString TOOLTIP = "Tsk tsk!\nThis Duplicant cannot receive treatment while out of their medical bed!";
			}

			// Token: 0x0200320F RID: 12815
			public class POSTDISEASERECOVERY
			{
				// Token: 0x0400CF47 RID: 53063
				public static LocString NAME = "Feeling better";

				// Token: 0x0400CF48 RID: 53064
				public static LocString TOOLTIP = "This Duplicant is up and about, but they still have some lingering effects from their " + UI.PRE_KEYWORD + "Disease" + UI.PST_KEYWORD;

				// Token: 0x0400CF49 RID: 53065
				public static LocString ADDITIONAL_EFFECTS = "This Duplicant has temporary immunity to diseases from having beaten an infection";
			}

			// Token: 0x02003210 RID: 12816
			public class IMMUNESYSTEMOVERWHELMED
			{
				// Token: 0x0400CF4A RID: 53066
				public static LocString NAME = "Immune System Overwhelmed";

				// Token: 0x0400CF4B RID: 53067
				public static LocString TOOLTIP = "This Duplicant's immune system is slowly being overwhelmed by a high concentration of germs";
			}

			// Token: 0x02003211 RID: 12817
			public class MEDICINE_GENERICPILL
			{
				// Token: 0x0400CF4C RID: 53068
				public static LocString NAME = "Placebo";

				// Token: 0x0400CF4D RID: 53069
				public static LocString TOOLTIP = ITEMS.PILLS.PLACEBO.DESC;

				// Token: 0x0400CF4E RID: 53070
				public static LocString EFFECT_DESC = string.Concat(new string[]
				{
					"Applies the ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" effect"
				});
			}

			// Token: 0x02003212 RID: 12818
			public class MEDICINE_BASICBOOSTER
			{
				// Token: 0x0400CF4F RID: 53071
				public static LocString NAME = ITEMS.PILLS.BASICBOOSTER.NAME;

				// Token: 0x0400CF50 RID: 53072
				public static LocString TOOLTIP = ITEMS.PILLS.BASICBOOSTER.DESC;
			}

			// Token: 0x02003213 RID: 12819
			public class MEDICINE_INTERMEDIATEBOOSTER
			{
				// Token: 0x0400CF51 RID: 53073
				public static LocString NAME = ITEMS.PILLS.INTERMEDIATEBOOSTER.NAME;

				// Token: 0x0400CF52 RID: 53074
				public static LocString TOOLTIP = ITEMS.PILLS.INTERMEDIATEBOOSTER.DESC;
			}

			// Token: 0x02003214 RID: 12820
			public class MEDICINE_BASICRADPILL
			{
				// Token: 0x0400CF53 RID: 53075
				public static LocString NAME = ITEMS.PILLS.BASICRADPILL.NAME;

				// Token: 0x0400CF54 RID: 53076
				public static LocString TOOLTIP = ITEMS.PILLS.BASICRADPILL.DESC;
			}

			// Token: 0x02003215 RID: 12821
			public class MEDICINE_INTERMEDIATERADPILL
			{
				// Token: 0x0400CF55 RID: 53077
				public static LocString NAME = ITEMS.PILLS.INTERMEDIATERADPILL.NAME;

				// Token: 0x0400CF56 RID: 53078
				public static LocString TOOLTIP = ITEMS.PILLS.INTERMEDIATERADPILL.DESC;
			}

			// Token: 0x02003216 RID: 12822
			public class SUNLIGHT_PLEASANT
			{
				// Token: 0x0400CF57 RID: 53079
				public static LocString NAME = "Bright and Cheerful";

				// Token: 0x0400CF58 RID: 53080
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The strong natural ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" is making this Duplicant feel light on their feet"
				});
			}

			// Token: 0x02003217 RID: 12823
			public class SUNLIGHT_BURNING
			{
				// Token: 0x0400CF59 RID: 53081
				public static LocString NAME = "Intensely Bright";

				// Token: 0x0400CF5A RID: 53082
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The bright ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" is significantly improving this Duplicant's mood, but prolonged exposure may result in burning"
				});
			}

			// Token: 0x02003218 RID: 12824
			public class TOOKABREAK
			{
				// Token: 0x0400CF5B RID: 53083
				public static LocString NAME = "Downtime";

				// Token: 0x0400CF5C RID: 53084
				public static LocString TOOLTIP = "This Duplicant has a bit of time off from work to attend to personal matters";
			}

			// Token: 0x02003219 RID: 12825
			public class SOCIALIZED
			{
				// Token: 0x0400CF5D RID: 53085
				public static LocString NAME = "Socialized";

				// Token: 0x0400CF5E RID: 53086
				public static LocString TOOLTIP = "This Duplicant had some free time to hang out with buddies";
			}

			// Token: 0x0200321A RID: 12826
			public class GOODCONVERSATION
			{
				// Token: 0x0400CF5F RID: 53087
				public static LocString NAME = "Pleasant Chitchat";

				// Token: 0x0400CF60 RID: 53088
				public static LocString TOOLTIP = "This Duplicant recently had a chance to chat with a friend";
			}

			// Token: 0x0200321B RID: 12827
			public class WORKENCOURAGED
			{
				// Token: 0x0400CF61 RID: 53089
				public static LocString NAME = "Appreciated";

				// Token: 0x0400CF62 RID: 53090
				public static LocString TOOLTIP = "Someone saw how hard this Duplicant was working and gave them a compliment\n\nThis Duplicant feels great about themselves now!";
			}

			// Token: 0x0200321C RID: 12828
			public class ISSTICKERBOMBING
			{
				// Token: 0x0400CF63 RID: 53091
				public static LocString NAME = "Sticker Bombing";

				// Token: 0x0400CF64 RID: 53092
				public static LocString TOOLTIP = "This Duplicant is slapping stickers onto everything!\n\nEveryone's gonna love these";
			}

			// Token: 0x0200321D RID: 12829
			public class ISSPARKLESTREAKER
			{
				// Token: 0x0400CF65 RID: 53093
				public static LocString NAME = "Sparkle Streaking";

				// Token: 0x0400CF66 RID: 53094
				public static LocString TOOLTIP = "This Duplicant is currently Sparkle Streaking!\n\nBaa-ling!";
			}

			// Token: 0x0200321E RID: 12830
			public class SAWSPARKLESTREAKER
			{
				// Token: 0x0400CF67 RID: 53095
				public static LocString NAME = "Sparkle Flattered";

				// Token: 0x0400CF68 RID: 53096
				public static LocString TOOLTIP = "A Sparkle Streaker's sparkles dazzled this Duplicant\n\nThis Duplicant has a spring in their step now!";
			}

			// Token: 0x0200321F RID: 12831
			public class ISJOYSINGER
			{
				// Token: 0x0400CF69 RID: 53097
				public static LocString NAME = "Yodeling";

				// Token: 0x0400CF6A RID: 53098
				public static LocString TOOLTIP = "This Duplicant is currently Yodeling!\n\nHow melodious!";
			}

			// Token: 0x02003220 RID: 12832
			public class HEARDJOYSINGER
			{
				// Token: 0x0400CF6B RID: 53099
				public static LocString NAME = "Serenaded";

				// Token: 0x0400CF6C RID: 53100
				public static LocString TOOLTIP = "A Yodeler's singing thrilled this Duplicant\n\nThis Duplicant works at a higher tempo now!";
			}

			// Token: 0x02003221 RID: 12833
			public class ISROBODANCER
			{
				// Token: 0x0400CF6D RID: 53101
				public static LocString NAME = "Doing the Robot";

				// Token: 0x0400CF6E RID: 53102
				public static LocString TOOLTIP = "This Duplicant is dancing like everybody's watching\n\nThey're a flash mob of one!";
			}

			// Token: 0x02003222 RID: 12834
			public class SAWROBODANCER
			{
				// Token: 0x0400CF6F RID: 53103
				public static LocString NAME = "Hyped";

				// Token: 0x0400CF70 RID: 53104
				public static LocString TOOLTIP = "A Flash Mobber's dance moves wowed this Duplicant\n\nThis Duplicant feels amped up now!";
			}

			// Token: 0x02003223 RID: 12835
			public class HASBALLOON
			{
				// Token: 0x0400CF71 RID: 53105
				public static LocString NAME = "Balloon Buddy";

				// Token: 0x0400CF72 RID: 53106
				public static LocString TOOLTIP = "A Balloon Artist gave this Duplicant a balloon!\n\nThis Duplicant feels super crafty now!";
			}

			// Token: 0x02003224 RID: 12836
			public class GREETING
			{
				// Token: 0x0400CF73 RID: 53107
				public static LocString NAME = "Saw Friend";

				// Token: 0x0400CF74 RID: 53108
				public static LocString TOOLTIP = "This Duplicant recently saw a friend in the halls and got to say \"hi\"\n\nIt wasn't even awkward!";
			}

			// Token: 0x02003225 RID: 12837
			public class HUGGED
			{
				// Token: 0x0400CF75 RID: 53109
				public static LocString NAME = "Hugged";

				// Token: 0x0400CF76 RID: 53110
				public static LocString TOOLTIP = "This Duplicant recently received a hug from a friendly critter\n\nIt was so fluffy!";
			}

			// Token: 0x02003226 RID: 12838
			public class ARCADEPLAYING
			{
				// Token: 0x0400CF77 RID: 53111
				public static LocString NAME = "Gaming";

				// Token: 0x0400CF78 RID: 53112
				public static LocString TOOLTIP = "This Duplicant is playing a video game\n\nIt looks like fun!";
			}

			// Token: 0x02003227 RID: 12839
			public class PLAYEDARCADE
			{
				// Token: 0x0400CF79 RID: 53113
				public static LocString NAME = "Played Video Games";

				// Token: 0x0400CF7A RID: 53114
				public static LocString TOOLTIP = "This Duplicant recently played video games and is feeling like a champ";
			}

			// Token: 0x02003228 RID: 12840
			public class DANCING
			{
				// Token: 0x0400CF7B RID: 53115
				public static LocString NAME = "Dancing";

				// Token: 0x0400CF7C RID: 53116
				public static LocString TOOLTIP = "This Duplicant is showing off their best moves.";
			}

			// Token: 0x02003229 RID: 12841
			public class DANCED
			{
				// Token: 0x0400CF7D RID: 53117
				public static LocString NAME = "Recently Danced";

				// Token: 0x0400CF7E RID: 53118
				public static LocString TOOLTIP = "This Duplicant had a chance to cut loose!\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200322A RID: 12842
			public class JUICER
			{
				// Token: 0x0400CF7F RID: 53119
				public static LocString NAME = "Drank Juice";

				// Token: 0x0400CF80 RID: 53120
				public static LocString TOOLTIP = "This Duplicant had delicious fruity drink!\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200322B RID: 12843
			public class ESPRESSO
			{
				// Token: 0x0400CF81 RID: 53121
				public static LocString NAME = "Drank Espresso";

				// Token: 0x0400CF82 RID: 53122
				public static LocString TOOLTIP = "This Duplicant had delicious drink!\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200322C RID: 12844
			public class MECHANICALSURFBOARD
			{
				// Token: 0x0400CF83 RID: 53123
				public static LocString NAME = "Stoked";

				// Token: 0x0400CF84 RID: 53124
				public static LocString TOOLTIP = "This Duplicant had a rad experience on a surfboard.\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200322D RID: 12845
			public class MECHANICALSURFING
			{
				// Token: 0x0400CF85 RID: 53125
				public static LocString NAME = "Surfin'";

				// Token: 0x0400CF86 RID: 53126
				public static LocString TOOLTIP = "This Duplicant is surfin' some artificial waves!";
			}

			// Token: 0x0200322E RID: 12846
			public class SAUNA
			{
				// Token: 0x0400CF87 RID: 53127
				public static LocString NAME = "Steam Powered";

				// Token: 0x0400CF88 RID: 53128
				public static LocString TOOLTIP = "This Duplicant just had a relaxing time in a sauna\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200322F RID: 12847
			public class SAUNARELAXING
			{
				// Token: 0x0400CF89 RID: 53129
				public static LocString NAME = "Relaxing";

				// Token: 0x0400CF8A RID: 53130
				public static LocString TOOLTIP = "This Duplicant is relaxing in a sauna";
			}

			// Token: 0x02003230 RID: 12848
			public class HOTTUB
			{
				// Token: 0x0400CF8B RID: 53131
				public static LocString NAME = "Hot Tubbed";

				// Token: 0x0400CF8C RID: 53132
				public static LocString TOOLTIP = "This Duplicant recently unwound in a Hot Tub\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003231 RID: 12849
			public class HOTTUBRELAXING
			{
				// Token: 0x0400CF8D RID: 53133
				public static LocString NAME = "Relaxing";

				// Token: 0x0400CF8E RID: 53134
				public static LocString TOOLTIP = "This Duplicant is unwinding in a hot tub\n\nThey sure look relaxed";
			}

			// Token: 0x02003232 RID: 12850
			public class SODAFOUNTAIN
			{
				// Token: 0x0400CF8F RID: 53135
				public static LocString NAME = "Soda Filled";

				// Token: 0x0400CF90 RID: 53136
				public static LocString TOOLTIP = "This Duplicant just enjoyed a bubbly beverage\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003233 RID: 12851
			public class VERTICALWINDTUNNELFLYING
			{
				// Token: 0x0400CF91 RID: 53137
				public static LocString NAME = "Airborne";

				// Token: 0x0400CF92 RID: 53138
				public static LocString TOOLTIP = "This Duplicant is having an exhilarating time in the wind tunnel\n\nWhoosh!";
			}

			// Token: 0x02003234 RID: 12852
			public class VERTICALWINDTUNNEL
			{
				// Token: 0x0400CF93 RID: 53139
				public static LocString NAME = "Wind Swept";

				// Token: 0x0400CF94 RID: 53140
				public static LocString TOOLTIP = "This Duplicant recently had an exhilarating wind tunnel experience\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003235 RID: 12853
			public class BEACHCHAIRRELAXING
			{
				// Token: 0x0400CF95 RID: 53141
				public static LocString NAME = "Totally Chill";

				// Token: 0x0400CF96 RID: 53142
				public static LocString TOOLTIP = "This Duplicant is totally chillin' in a beach chair";
			}

			// Token: 0x02003236 RID: 12854
			public class BEACHCHAIRLIT
			{
				// Token: 0x0400CF97 RID: 53143
				public static LocString NAME = "Sun Kissed";

				// Token: 0x0400CF98 RID: 53144
				public static LocString TOOLTIP = "This Duplicant had an amazing experience at the Beach\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003237 RID: 12855
			public class BEACHCHAIRUNLIT
			{
				// Token: 0x0400CF99 RID: 53145
				public static LocString NAME = "Passably Relaxed";

				// Token: 0x0400CF9A RID: 53146
				public static LocString TOOLTIP = "This Duplicant just had a mediocre beach experience\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003238 RID: 12856
			public class TELEPHONECHAT
			{
				// Token: 0x0400CF9B RID: 53147
				public static LocString NAME = "Full of Gossip";

				// Token: 0x0400CF9C RID: 53148
				public static LocString TOOLTIP = "This Duplicant chatted on the phone with at least one other Duplicant\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003239 RID: 12857
			public class TELEPHONEBABBLE
			{
				// Token: 0x0400CF9D RID: 53149
				public static LocString NAME = "Less Anxious";

				// Token: 0x0400CF9E RID: 53150
				public static LocString TOOLTIP = "This Duplicant got some things off their chest by talking to themselves on the phone\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200323A RID: 12858
			public class TELEPHONELONGDISTANCE
			{
				// Token: 0x0400CF9F RID: 53151
				public static LocString NAME = "Sociable";

				// Token: 0x0400CFA0 RID: 53152
				public static LocString TOOLTIP = "This Duplicant is feeling sociable after chatting on the phone with someone across space\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200323B RID: 12859
			public class EDIBLEMINUS3
			{
				// Token: 0x0400CFA1 RID: 53153
				public static LocString NAME = "Grisly Meal";

				// Token: 0x0400CFA2 RID: 53154
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Grisly",
					UI.PST_KEYWORD,
					"\n\nThey hope their next meal will be better"
				});
			}

			// Token: 0x0200323C RID: 12860
			public class EDIBLEMINUS2
			{
				// Token: 0x0400CFA3 RID: 53155
				public static LocString NAME = "Terrible Meal";

				// Token: 0x0400CFA4 RID: 53156
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Terrible",
					UI.PST_KEYWORD,
					"\n\nThey hope their next meal will be better"
				});
			}

			// Token: 0x0200323D RID: 12861
			public class EDIBLEMINUS1
			{
				// Token: 0x0400CFA5 RID: 53157
				public static LocString NAME = "Poor Meal";

				// Token: 0x0400CFA6 RID: 53158
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Poor",
					UI.PST_KEYWORD,
					"\n\nThey hope their next meal will be a little better"
				});
			}

			// Token: 0x0200323E RID: 12862
			public class EDIBLE0
			{
				// Token: 0x0400CFA7 RID: 53159
				public static LocString NAME = "Standard Meal";

				// Token: 0x0400CFA8 RID: 53160
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Average",
					UI.PST_KEYWORD,
					"\n\nThey thought it was sort of okay"
				});
			}

			// Token: 0x0200323F RID: 12863
			public class EDIBLE1
			{
				// Token: 0x0400CFA9 RID: 53161
				public static LocString NAME = "Good Meal";

				// Token: 0x0400CFAA RID: 53162
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Good",
					UI.PST_KEYWORD,
					"\n\nThey thought it was pretty good!"
				});
			}

			// Token: 0x02003240 RID: 12864
			public class EDIBLE2
			{
				// Token: 0x0400CFAB RID: 53163
				public static LocString NAME = "Great Meal";

				// Token: 0x0400CFAC RID: 53164
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Great",
					UI.PST_KEYWORD,
					"\n\nThey thought it was pretty good!"
				});
			}

			// Token: 0x02003241 RID: 12865
			public class EDIBLE3
			{
				// Token: 0x0400CFAD RID: 53165
				public static LocString NAME = "Superb Meal";

				// Token: 0x0400CFAE RID: 53166
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Superb",
					UI.PST_KEYWORD,
					"\n\nThey thought it was really good!"
				});
			}

			// Token: 0x02003242 RID: 12866
			public class EDIBLE4
			{
				// Token: 0x0400CFAF RID: 53167
				public static LocString NAME = "Ambrosial Meal";

				// Token: 0x0400CFB0 RID: 53168
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Ambrosial",
					UI.PST_KEYWORD,
					"\n\nThey thought it was super tasty!"
				});
			}

			// Token: 0x02003243 RID: 12867
			public class DECORMINUS1
			{
				// Token: 0x0400CFB1 RID: 53169
				public static LocString NAME = "Last Cycle's Decor: Ugly";

				// Token: 0x0400CFB2 RID: 53170
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was downright depressing"
				});
			}

			// Token: 0x02003244 RID: 12868
			public class DECOR0
			{
				// Token: 0x0400CFB3 RID: 53171
				public static LocString NAME = "Last Cycle's Decor: Poor";

				// Token: 0x0400CFB4 RID: 53172
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was quite poor"
				});
			}

			// Token: 0x02003245 RID: 12869
			public class DECOR1
			{
				// Token: 0x0400CFB5 RID: 53173
				public static LocString NAME = "Last Cycle's Decor: Mediocre";

				// Token: 0x0400CFB6 RID: 53174
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant had no strong opinions about the colony's ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday"
				});
			}

			// Token: 0x02003246 RID: 12870
			public class DECOR2
			{
				// Token: 0x0400CFB7 RID: 53175
				public static LocString NAME = "Last Cycle's Decor: Average";

				// Token: 0x0400CFB8 RID: 53176
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was pretty alright"
				});
			}

			// Token: 0x02003247 RID: 12871
			public class DECOR3
			{
				// Token: 0x0400CFB9 RID: 53177
				public static LocString NAME = "Last Cycle's Decor: Nice";

				// Token: 0x0400CFBA RID: 53178
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was quite nice!"
				});
			}

			// Token: 0x02003248 RID: 12872
			public class DECOR4
			{
				// Token: 0x0400CFBB RID: 53179
				public static LocString NAME = "Last Cycle's Decor: Charming";

				// Token: 0x0400CFBC RID: 53180
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was downright charming!"
				});
			}

			// Token: 0x02003249 RID: 12873
			public class DECOR5
			{
				// Token: 0x0400CFBD RID: 53181
				public static LocString NAME = "Last Cycle's Decor: Gorgeous";

				// Token: 0x0400CFBE RID: 53182
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was fantastic\n\nThey love what I've done with the place!"
				});
			}

			// Token: 0x0200324A RID: 12874
			public class BREAK1
			{
				// Token: 0x0400CFBF RID: 53183
				public static LocString NAME = "One Shift Break";

				// Token: 0x0400CFC0 RID: 53184
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had one ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shift in the last cycle"
				});
			}

			// Token: 0x0200324B RID: 12875
			public class BREAK2
			{
				// Token: 0x0400CFC1 RID: 53185
				public static LocString NAME = "Two Shift Break";

				// Token: 0x0400CFC2 RID: 53186
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had two ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts in the last cycle"
				});
			}

			// Token: 0x0200324C RID: 12876
			public class BREAK3
			{
				// Token: 0x0400CFC3 RID: 53187
				public static LocString NAME = "Three Shift Break";

				// Token: 0x0400CFC4 RID: 53188
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had three ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts in the last cycle"
				});
			}

			// Token: 0x0200324D RID: 12877
			public class BREAK4
			{
				// Token: 0x0400CFC5 RID: 53189
				public static LocString NAME = "Four Shift Break";

				// Token: 0x0400CFC6 RID: 53190
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had four ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts in the last cycle"
				});
			}

			// Token: 0x0200324E RID: 12878
			public class BREAK5
			{
				// Token: 0x0400CFC7 RID: 53191
				public static LocString NAME = "Five Shift Break";

				// Token: 0x0400CFC8 RID: 53192
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had five ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts in the last cycle"
				});
			}

			// Token: 0x0200324F RID: 12879
			public class POWERTINKER
			{
				// Token: 0x0400CFC9 RID: 53193
				public static LocString NAME = "Engie's Tune-Up";

				// Token: 0x0400CFCA RID: 53194
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A skilled Duplicant has improved this generator's ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" output efficiency\n\nApplying this effect consumed one ",
					UI.PRE_KEYWORD,
					ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME,
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003250 RID: 12880
			public class FARMTINKER
			{
				// Token: 0x0400CFCB RID: 53195
				public static LocString NAME = "Farmer's Touch";

				// Token: 0x0400CFCC RID: 53196
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A skilled Duplicant has encouraged this ",
					UI.PRE_KEYWORD,
					"Plant",
					UI.PST_KEYWORD,
					" to grow a little bit faster\n\nApplying this effect consumed one dose of ",
					UI.PRE_KEYWORD,
					ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME,
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003251 RID: 12881
			public class MACHINETINKER
			{
				// Token: 0x0400CFCD RID: 53197
				public static LocString NAME = "Engie's Jerry Rig";

				// Token: 0x0400CFCE RID: 53198
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A skilled Duplicant has jerry rigged this ",
					UI.PRE_KEYWORD,
					"Generator",
					UI.PST_KEYWORD,
					" to temporarily run faster"
				});
			}

			// Token: 0x02003252 RID: 12882
			public class SPACETOURIST
			{
				// Token: 0x0400CFCF RID: 53199
				public static LocString NAME = "Visited Space";

				// Token: 0x0400CFD0 RID: 53200
				public static LocString TOOLTIP = "This Duplicant went on a trip to space and saw the wonders of the universe";
			}

			// Token: 0x02003253 RID: 12883
			public class SUDDENMORALEHELPER
			{
				// Token: 0x0400CFD1 RID: 53201
				public static LocString NAME = "Morale Upgrade Helper";

				// Token: 0x0400CFD2 RID: 53202
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant will receive a temporary ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" bonus to buffer the new ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" system introduction"
				});
			}

			// Token: 0x02003254 RID: 12884
			public class EXPOSEDTOFOODGERMS
			{
				// Token: 0x0400CFD3 RID: 53203
				public static LocString NAME = "Food Poisoning Exposure";

				// Token: 0x0400CFD4 RID: 53204
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was exposed to ",
					DUPLICANTS.DISEASES.FOODPOISONING.NAME,
					" Germs and is at risk of developing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003255 RID: 12885
			public class EXPOSEDTOSLIMEGERMS
			{
				// Token: 0x0400CFD5 RID: 53205
				public static LocString NAME = "Slimelung Exposure";

				// Token: 0x0400CFD6 RID: 53206
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was exposed to ",
					DUPLICANTS.DISEASES.SLIMELUNG.NAME,
					" and is at risk of developing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003256 RID: 12886
			public class EXPOSEDTOZOMBIESPORES
			{
				// Token: 0x0400CFD7 RID: 53207
				public static LocString NAME = "Zombie Spores Exposure";

				// Token: 0x0400CFD8 RID: 53208
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was exposed to ",
					DUPLICANTS.DISEASES.ZOMBIESPORES.NAME,
					" and is at risk of developing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003257 RID: 12887
			public class FEELINGSICKFOODGERMS
			{
				// Token: 0x0400CFD9 RID: 53209
				public static LocString NAME = "Contracted: Food Poisoning";

				// Token: 0x0400CFDA RID: 53210
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant contracted ",
					DUPLICANTS.DISEASES.FOODSICKNESS.NAME,
					" after a recent ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					" exposure and will begin exhibiting symptoms shortly"
				});
			}

			// Token: 0x02003258 RID: 12888
			public class FEELINGSICKSLIMEGERMS
			{
				// Token: 0x0400CFDB RID: 53211
				public static LocString NAME = "Contracted: Slimelung";

				// Token: 0x0400CFDC RID: 53212
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant contracted ",
					DUPLICANTS.DISEASES.SLIMESICKNESS.NAME,
					" after a recent ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					" exposure and will begin exhibiting symptoms shortly"
				});
			}

			// Token: 0x02003259 RID: 12889
			public class FEELINGSICKZOMBIESPORES
			{
				// Token: 0x0400CFDD RID: 53213
				public static LocString NAME = "Contracted: Zombie Spores";

				// Token: 0x0400CFDE RID: 53214
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant contracted ",
					DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME,
					" after a recent ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					" exposure and will begin exhibiting symptoms shortly"
				});
			}

			// Token: 0x0200325A RID: 12890
			public class SMELLEDFLOWERS
			{
				// Token: 0x0400CFDF RID: 53215
				public static LocString NAME = "Smelled Flowers";

				// Token: 0x0400CFE0 RID: 53216
				public static LocString TOOLTIP = "A pleasant " + DUPLICANTS.DISEASES.POLLENGERMS.NAME + " wafted over this Duplicant and brightened their day";
			}

			// Token: 0x0200325B RID: 12891
			public class HISTAMINESUPPRESSION
			{
				// Token: 0x0400CFE1 RID: 53217
				public static LocString NAME = "Antihistamines";

				// Token: 0x0400CFE2 RID: 53218
				public static LocString TOOLTIP = "This Duplicant's allergic reactions have been suppressed by medication";
			}

			// Token: 0x0200325C RID: 12892
			public class FOODSICKNESSRECOVERY
			{
				// Token: 0x0400CFE3 RID: 53219
				public static LocString NAME = "Food Poisoning Antibodies";

				// Token: 0x0400CFE4 RID: 53220
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently recovered from ",
					DUPLICANTS.DISEASES.FOODSICKNESS.NAME,
					" and is temporarily immune to the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200325D RID: 12893
			public class SLIMESICKNESSRECOVERY
			{
				// Token: 0x0400CFE5 RID: 53221
				public static LocString NAME = "Slimelung Antibodies";

				// Token: 0x0400CFE6 RID: 53222
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently recovered from ",
					DUPLICANTS.DISEASES.SLIMESICKNESS.NAME,
					" and is temporarily immune to the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200325E RID: 12894
			public class ZOMBIESICKNESSRECOVERY
			{
				// Token: 0x0400CFE7 RID: 53223
				public static LocString NAME = "Zombie Spores Antibodies";

				// Token: 0x0400CFE8 RID: 53224
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently recovered from ",
					DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME,
					" and is temporarily immune to the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200325F RID: 12895
			public class MESSTABLESALT
			{
				// Token: 0x0400CFE9 RID: 53225
				public static LocString NAME = "Salted Food";

				// Token: 0x0400CFEA RID: 53226
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant had the luxury of using ",
					UI.PRE_KEYWORD,
					ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME,
					UI.PST_KEYWORD,
					" with their last meal at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME
				});
			}

			// Token: 0x02003260 RID: 12896
			public class RADIATIONEXPOSUREMINOR
			{
				// Token: 0x0400CFEB RID: 53227
				public static LocString NAME = "Minor Radiation Sickness";

				// Token: 0x0400CFEC RID: 53228
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A bit of ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has made this Duplicant feel sluggish"
				});
			}

			// Token: 0x02003261 RID: 12897
			public class RADIATIONEXPOSUREMAJOR
			{
				// Token: 0x0400CFED RID: 53229
				public static LocString NAME = "Major Radiation Sickness";

				// Token: 0x0400CFEE RID: 53230
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Significant ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has left this Duplicant totally exhausted"
				});
			}

			// Token: 0x02003262 RID: 12898
			public class RADIATIONEXPOSUREEXTREME
			{
				// Token: 0x0400CFEF RID: 53231
				public static LocString NAME = "Extreme Radiation Sickness";

				// Token: 0x0400CFF0 RID: 53232
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Dangerously high ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure is making this Duplicant wish they'd never been printed"
				});
			}

			// Token: 0x02003263 RID: 12899
			public class RADIATIONEXPOSUREDEADLY
			{
				// Token: 0x0400CFF1 RID: 53233
				public static LocString NAME = "Deadly Radiation Sickness";

				// Token: 0x0400CFF2 RID: 53234
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Extreme ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has incapacitated this Duplicant"
				});
			}

			// Token: 0x02003264 RID: 12900
			public class CHARGING
			{
				// Token: 0x0400CFF3 RID: 53235
				public static LocString NAME = "Charging";

				// Token: 0x0400CFF4 RID: 53236
				public static LocString TOOLTIP = "This lil bot is charging its internal battery";
			}

			// Token: 0x02003265 RID: 12901
			public class BOTSWEEPING
			{
				// Token: 0x0400CFF5 RID: 53237
				public static LocString NAME = "Sweeping";

				// Token: 0x0400CFF6 RID: 53238
				public static LocString TOOLTIP = "This lil bot is picking up debris from the floor";
			}

			// Token: 0x02003266 RID: 12902
			public class BOTMOPPING
			{
				// Token: 0x0400CFF7 RID: 53239
				public static LocString NAME = "Mopping";

				// Token: 0x0400CFF8 RID: 53240
				public static LocString TOOLTIP = "This lil bot is clearing liquids from the ground";
			}

			// Token: 0x02003267 RID: 12903
			public class SCOUTBOTCHARGING
			{
				// Token: 0x0400CFF9 RID: 53241
				public static LocString NAME = "Charging";

				// Token: 0x0400CFFA RID: 53242
				public static LocString TOOLTIP = ROBOTS.MODELS.SCOUT.NAME + " is happily charging inside " + BUILDINGS.PREFABS.SCOUTMODULE.NAME;
			}

			// Token: 0x02003268 RID: 12904
			public class CRYOFRIEND
			{
				// Token: 0x0400CFFB RID: 53243
				public static LocString NAME = "Motivated By Friend";

				// Token: 0x0400CFFC RID: 53244
				public static LocString TOOLTIP = "This Duplicant feels motivated after meeting a long lost friend";
			}

			// Token: 0x02003269 RID: 12905
			public class BONUSDREAM1
			{
				// Token: 0x0400CFFD RID: 53245
				public static LocString NAME = "Good Dream";

				// Token: 0x0400CFFE RID: 53246
				public static LocString TOOLTIP = "This Duplicant had a good dream and is feeling psyched!";
			}

			// Token: 0x0200326A RID: 12906
			public class BONUSDREAM2
			{
				// Token: 0x0400CFFF RID: 53247
				public static LocString NAME = "Really Good Dream";

				// Token: 0x0400D000 RID: 53248
				public static LocString TOOLTIP = "This Duplicant had a really good dream and is full of possibilities!";
			}

			// Token: 0x0200326B RID: 12907
			public class BONUSDREAM3
			{
				// Token: 0x0400D001 RID: 53249
				public static LocString NAME = "Great Dream";

				// Token: 0x0400D002 RID: 53250
				public static LocString TOOLTIP = "This Duplicant had a great dream last night and periodically remembers another great moment they previously forgot";
			}

			// Token: 0x0200326C RID: 12908
			public class BONUSDREAM4
			{
				// Token: 0x0400D003 RID: 53251
				public static LocString NAME = "Dream Inspired";

				// Token: 0x0400D004 RID: 53252
				public static LocString TOOLTIP = "This Duplicant is inspired from all the unforgettable dreams they had";
			}

			// Token: 0x0200326D RID: 12909
			public class BONUSRESEARCH
			{
				// Token: 0x0400D005 RID: 53253
				public static LocString NAME = "Inspired Learner";

				// Token: 0x0400D006 RID: 53254
				public static LocString TOOLTIP = "This Duplicant is looking forward to some learning";
			}

			// Token: 0x0200326E RID: 12910
			public class BONUSTOILET1
			{
				// Token: 0x0400D007 RID: 53255
				public static LocString NAME = "Small Comforts";

				// Token: 0x0400D008 RID: 53256
				public static LocString TOOLTIP = "This Duplicant visited the {building} and appreciated the small comforts";
			}

			// Token: 0x0200326F RID: 12911
			public class BONUSTOILET2
			{
				// Token: 0x0400D009 RID: 53257
				public static LocString NAME = "Greater Comforts";

				// Token: 0x0400D00A RID: 53258
				public static LocString TOOLTIP = "This Duplicant used a " + BUILDINGS.PREFABS.OUTHOUSE.NAME + "and liked how comfortable it felt";
			}

			// Token: 0x02003270 RID: 12912
			public class BONUSTOILET3
			{
				// Token: 0x0400D00B RID: 53259
				public static LocString NAME = "Small Luxury";

				// Token: 0x0400D00C RID: 53260
				public static LocString TOOLTIP = "This Duplicant visited a " + ROOMS.TYPES.LATRINE.NAME + " and feels they could get used to this luxury";
			}

			// Token: 0x02003271 RID: 12913
			public class BONUSTOILET4
			{
				// Token: 0x0400D00D RID: 53261
				public static LocString NAME = "Luxurious";

				// Token: 0x0400D00E RID: 53262
				public static LocString TOOLTIP = "This Duplicant feels endless luxury from the " + ROOMS.TYPES.PRIVATE_BATHROOM.NAME;
			}

			// Token: 0x02003272 RID: 12914
			public class BONUSDIGGING1
			{
				// Token: 0x0400D00F RID: 53263
				public static LocString NAME = "Hot Diggity!";

				// Token: 0x0400D010 RID: 53264
				public static LocString TOOLTIP = "This Duplicant did a lot of excavating and is really digging digging";
			}

			// Token: 0x02003273 RID: 12915
			public class BONUSSTORAGE
			{
				// Token: 0x0400D011 RID: 53265
				public static LocString NAME = "Something in Store";

				// Token: 0x0400D012 RID: 53266
				public static LocString TOOLTIP = "This Duplicant stored something in a " + BUILDINGS.PREFABS.STORAGELOCKER.NAME + " and is feeling organized";
			}

			// Token: 0x02003274 RID: 12916
			public class BONUSBUILDER
			{
				// Token: 0x0400D013 RID: 53267
				public static LocString NAME = "Accomplished Builder";

				// Token: 0x0400D014 RID: 53268
				public static LocString TOOLTIP = "This Duplicant has built many buildings and has a sense of accomplishment!";
			}

			// Token: 0x02003275 RID: 12917
			public class BONUSOXYGEN
			{
				// Token: 0x0400D015 RID: 53269
				public static LocString NAME = "Fresh Air";

				// Token: 0x0400D016 RID: 53270
				public static LocString TOOLTIP = "This Duplicant breathed in some fresh air and is feeling refreshed";
			}

			// Token: 0x02003276 RID: 12918
			public class BONUSGENERATOR
			{
				// Token: 0x0400D017 RID: 53271
				public static LocString NAME = "Exercised";

				// Token: 0x0400D018 RID: 53272
				public static LocString TOOLTIP = "This Duplicant ran in a Generator and has benefited from the exercise";
			}

			// Token: 0x02003277 RID: 12919
			public class BONUSDOOR
			{
				// Token: 0x0400D019 RID: 53273
				public static LocString NAME = "Open and Shut";

				// Token: 0x0400D01A RID: 53274
				public static LocString TOOLTIP = "This Duplicant closed a door and appreciates the privacy";
			}

			// Token: 0x02003278 RID: 12920
			public class BONUSHITTHEBOOKS
			{
				// Token: 0x0400D01B RID: 53275
				public static LocString NAME = "Hit the Books";

				// Token: 0x0400D01C RID: 53276
				public static LocString TOOLTIP = "This Duplicant did some research and is feeling smarter";
			}

			// Token: 0x02003279 RID: 12921
			public class BONUSLITWORKSPACE
			{
				// Token: 0x0400D01D RID: 53277
				public static LocString NAME = "Lit";

				// Token: 0x0400D01E RID: 53278
				public static LocString TOOLTIP = "This Duplicant was in a well-lit environment and is feeling lit";
			}

			// Token: 0x0200327A RID: 12922
			public class BONUSTALKER
			{
				// Token: 0x0400D01F RID: 53279
				public static LocString NAME = "Talker";

				// Token: 0x0400D020 RID: 53280
				public static LocString TOOLTIP = "This Duplicant engaged in small talk with a coworker and is feeling connected";
			}

			// Token: 0x0200327B RID: 12923
			public class THRIVER
			{
				// Token: 0x0400D021 RID: 53281
				public static LocString NAME = "Clutchy";

				// Token: 0x0400D022 RID: 53282
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" and has kicked into hyperdrive"
				});
			}

			// Token: 0x0200327C RID: 12924
			public class LONER
			{
				// Token: 0x0400D023 RID: 53283
				public static LocString NAME = "Alone";

				// Token: 0x0400D024 RID: 53284
				public static LocString TOOLTIP = "This Duplicant is feeling more focused now that they're alone";
			}

			// Token: 0x0200327D RID: 12925
			public class STARRYEYED
			{
				// Token: 0x0400D025 RID: 53285
				public static LocString NAME = "Starry Eyed";

				// Token: 0x0400D026 RID: 53286
				public static LocString TOOLTIP = "This Duplicant loves being in space!";
			}

			// Token: 0x0200327E RID: 12926
			public class WAILEDAT
			{
				// Token: 0x0400D027 RID: 53287
				public static LocString NAME = "Disturbed by Wailing";

				// Token: 0x0400D028 RID: 53288
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is feeling ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" by someone's Banshee Wail"
				});
			}

			// Token: 0x0200327F RID: 12927
			public class BIONICPILOTINGBOOST
			{
				// Token: 0x0400D029 RID: 53289
				public static LocString NAME = "Piloting Boost";

				// Token: 0x0400D02A RID: 53290
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased rocket piloting skills thanks to the ",
					UI.PRE_KEYWORD,
					"Rocketry Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x02003280 RID: 12928
			public class BIONICCONSTRUCTIONBOOST
			{
				// Token: 0x0400D02B RID: 53291
				public static LocString NAME = "Construction Boost";

				// Token: 0x0400D02C RID: 53292
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased construction skills thanks to the ",
					UI.PRE_KEYWORD,
					"Building Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x02003281 RID: 12929
			public class BIONICEXCAVATIONBOOST
			{
				// Token: 0x0400D02D RID: 53293
				public static LocString NAME = "Excavation Boost";

				// Token: 0x0400D02E RID: 53294
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased excavation skills thanks to the ",
					UI.PRE_KEYWORD,
					"Digging Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x02003282 RID: 12930
			public class BIONICMACHINERYBOOST
			{
				// Token: 0x0400D02F RID: 53295
				public static LocString NAME = "Machinery Boost";

				// Token: 0x0400D030 RID: 53296
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased machine operating skills thanks to the ",
					UI.PRE_KEYWORD,
					"Operating Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x02003283 RID: 12931
			public class BIONICATHLETICSBOOST
			{
				// Token: 0x0400D031 RID: 53297
				public static LocString NAME = "Athletics Boost";

				// Token: 0x0400D032 RID: 53298
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has extra zip in their step thanks to the ",
					UI.PRE_KEYWORD,
					"Athletics Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x02003284 RID: 12932
			public class BIONICSCIENCEBOOST
			{
				// Token: 0x0400D033 RID: 53299
				public static LocString NAME = "Science Boost";

				// Token: 0x0400D034 RID: 53300
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased scientific research skills thanks to the ",
					UI.PRE_KEYWORD,
					"Researching Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x02003285 RID: 12933
			public class BIONICCOOKINGBOOST
			{
				// Token: 0x0400D035 RID: 53301
				public static LocString NAME = "Cuisine Boost";

				// Token: 0x0400D036 RID: 53302
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased culinary skills thanks to the ",
					UI.PRE_KEYWORD,
					"Cooking Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x02003286 RID: 12934
			public class BIONICMEDICINEBOOST
			{
				// Token: 0x0400D037 RID: 53303
				public static LocString NAME = "Medicine Boost";

				// Token: 0x0400D038 RID: 53304
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased medical skills thanks to the ",
					UI.PRE_KEYWORD,
					"Doctoring Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x02003287 RID: 12935
			public class BIONICSTRENGTHBOOST
			{
				// Token: 0x0400D039 RID: 53305
				public static LocString NAME = "Strength Boost";

				// Token: 0x0400D03A RID: 53306
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is extra strong thanks to the ",
					UI.PRE_KEYWORD,
					"Strength Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x02003288 RID: 12936
			public class BIONICCREATIVITYBOOST
			{
				// Token: 0x0400D03B RID: 53307
				public static LocString NAME = "Creativity Boost";

				// Token: 0x0400D03C RID: 53308
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased decorating skills thanks to the ",
					UI.PRE_KEYWORD,
					"Creativity Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x02003289 RID: 12937
			public class BIONICAGRICULTUREBOOST
			{
				// Token: 0x0400D03D RID: 53309
				public static LocString NAME = "Agriculture Boost";

				// Token: 0x0400D03E RID: 53310
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased agricultural skills thanks to the ",
					UI.PRE_KEYWORD,
					"Farming Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x0200328A RID: 12938
			public class BIONICHUSBANDRYBOOST
			{
				// Token: 0x0400D03F RID: 53311
				public static LocString NAME = "Husbandry Boost";

				// Token: 0x0400D040 RID: 53312
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased critter husbandry skills thanks to the ",
					UI.PRE_KEYWORD,
					"Ranching Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}
		}

		// Token: 0x020021B6 RID: 8630
		public class CONGENITALTRAITS
		{
			// Token: 0x0200328B RID: 12939
			public class NONE
			{
				// Token: 0x0400D041 RID: 53313
				public static LocString NAME = "None";

				// Token: 0x0400D042 RID: 53314
				public static LocString DESC = "This Duplicant seems pretty average overall";
			}

			// Token: 0x0200328C RID: 12940
			public class JOSHUA
			{
				// Token: 0x0400D043 RID: 53315
				public static LocString NAME = "Cheery Disposition";

				// Token: 0x0400D044 RID: 53316
				public static LocString DESC = "This Duplicant brightens others' days wherever he goes";
			}

			// Token: 0x0200328D RID: 12941
			public class ELLIE
			{
				// Token: 0x0400D045 RID: 53317
				public static LocString NAME = "Fastidious";

				// Token: 0x0400D046 RID: 53318
				public static LocString DESC = "This Duplicant needs things done in a very particular way";
			}

			// Token: 0x0200328E RID: 12942
			public class LIAM
			{
				// Token: 0x0400D047 RID: 53319
				public static LocString NAME = "Germaphobe";

				// Token: 0x0400D048 RID: 53320
				public static LocString DESC = "This Duplicant has an all-consuming fear of bacteria";
			}

			// Token: 0x0200328F RID: 12943
			public class BANHI
			{
				// Token: 0x0400D049 RID: 53321
				public static LocString NAME = "";

				// Token: 0x0400D04A RID: 53322
				public static LocString DESC = "";
			}

			// Token: 0x02003290 RID: 12944
			public class STINKY
			{
				// Token: 0x0400D04B RID: 53323
				public static LocString NAME = "Stinkiness";

				// Token: 0x0400D04C RID: 53324
				public static LocString DESC = "This Duplicant is genetically cursed by a pungent bodily odor";
			}
		}

		// Token: 0x020021B7 RID: 8631
		public class TRAITS
		{
			// Token: 0x040099A8 RID: 39336
			public static LocString TRAIT_DESCRIPTION_LIST_ENTRY = "\n• ";

			// Token: 0x040099A9 RID: 39337
			public static LocString ATTRIBUTE_MODIFIERS = "{0}: {1}";

			// Token: 0x040099AA RID: 39338
			public static LocString CANNOT_DO_TASK = "Cannot do <b>{0} Errands</b>";

			// Token: 0x040099AB RID: 39339
			public static LocString CANNOT_DO_TASK_TOOLTIP = "{0}: {1}";

			// Token: 0x040099AC RID: 39340
			public static LocString REFUSES_TO_DO_TASK = "Cannot do <b>{0} Errands</b>";

			// Token: 0x040099AD RID: 39341
			public static LocString IGNORED_EFFECTS = "Immune to <b>{0}</b>";

			// Token: 0x040099AE RID: 39342
			public static LocString IGNORED_EFFECTS_TOOLTIP = "{0}: {1}";

			// Token: 0x040099AF RID: 39343
			public static LocString GRANTED_SKILL_SHARED_NAME = "Skilled: ";

			// Token: 0x040099B0 RID: 39344
			public static LocString GRANTED_SKILL_SHARED_DESC = string.Concat(new string[]
			{
				"This Duplicant begins with a pre-learned ",
				UI.FormatAsKeyWord("Skill"),
				", but does not have increased ",
				UI.FormatAsKeyWord(DUPLICANTS.NEEDS.QUALITYOFLIFE.NAME),
				".\n\n{0}\n{1}"
			});

			// Token: 0x040099B1 RID: 39345
			public static LocString GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP = "This Duplicant receives a free " + UI.FormatAsKeyWord("Skill") + " without the drawback of increased " + UI.FormatAsKeyWord(DUPLICANTS.NEEDS.QUALITYOFLIFE.NAME);

			// Token: 0x02003291 RID: 12945
			public class CHATTY
			{
				// Token: 0x0400D04D RID: 53325
				public static LocString NAME = "Charismatic";

				// Token: 0x0400D04E RID: 53326
				public static LocString DESC = string.Concat(new string[]
				{
					"This Duplicant's so charming, chatting with them is sometimes enough to trigger an ",
					UI.PRE_KEYWORD,
					"Overjoyed",
					UI.PST_KEYWORD,
					" response"
				});
			}

			// Token: 0x02003292 RID: 12946
			public class NEEDS
			{
				// Token: 0x0200382F RID: 14383
				public class CLAUSTROPHOBIC
				{
					// Token: 0x0400DE68 RID: 56936
					public static LocString NAME = "Claustrophobic";

					// Token: 0x0400DE69 RID: 56937
					public static LocString DESC = "This Duplicant feels suffocated in spaces fewer than four tiles high or three tiles wide";
				}

				// Token: 0x02003830 RID: 14384
				public class FASHIONABLE
				{
					// Token: 0x0400DE6A RID: 56938
					public static LocString NAME = "Fashionista";

					// Token: 0x0400DE6B RID: 56939
					public static LocString DESC = "This Duplicant dies a bit inside when forced to wear unstylish clothing";
				}

				// Token: 0x02003831 RID: 14385
				public class CLIMACOPHOBIC
				{
					// Token: 0x0400DE6C RID: 56940
					public static LocString NAME = "Vertigo Prone";

					// Token: 0x0400DE6D RID: 56941
					public static LocString DESC = "Climbing ladders more than four tiles tall makes this Duplicant's stomach do flips";
				}

				// Token: 0x02003832 RID: 14386
				public class SOLITARYSLEEPER
				{
					// Token: 0x0400DE6E RID: 56942
					public static LocString NAME = "Solitary Sleeper";

					// Token: 0x0400DE6F RID: 56943
					public static LocString DESC = "This Duplicant prefers to sleep alone";
				}

				// Token: 0x02003833 RID: 14387
				public class PREFERSWARMER
				{
					// Token: 0x0400DE70 RID: 56944
					public static LocString NAME = "Skinny";

					// Token: 0x0400DE71 RID: 56945
					public static LocString DESC = string.Concat(new string[]
					{
						"This Duplicant doesn't have much ",
						UI.PRE_KEYWORD,
						"Insulation",
						UI.PST_KEYWORD,
						", so they are more ",
						UI.PRE_KEYWORD,
						"Temperature",
						UI.PST_KEYWORD,
						" sensitive than others"
					});
				}

				// Token: 0x02003834 RID: 14388
				public class PREFERSCOOLER
				{
					// Token: 0x0400DE72 RID: 56946
					public static LocString NAME = "Pudgy";

					// Token: 0x0400DE73 RID: 56947
					public static LocString DESC = string.Concat(new string[]
					{
						"This Duplicant has some extra ",
						UI.PRE_KEYWORD,
						"Insulation",
						UI.PST_KEYWORD,
						", so the room ",
						UI.PRE_KEYWORD,
						"Temperature",
						UI.PST_KEYWORD,
						" affects them a little less"
					});
				}

				// Token: 0x02003835 RID: 14389
				public class SENSITIVEFEET
				{
					// Token: 0x0400DE74 RID: 56948
					public static LocString NAME = "Delicate Feetsies";

					// Token: 0x0400DE75 RID: 56949
					public static LocString DESC = "This Duplicant is a sensitive sole and would rather walk on tile than raw bedrock";
				}

				// Token: 0x02003836 RID: 14390
				public class WORKAHOLIC
				{
					// Token: 0x0400DE76 RID: 56950
					public static LocString NAME = "Workaholic";

					// Token: 0x0400DE77 RID: 56951
					public static LocString DESC = "This Duplicant gets antsy when left idle";
				}
			}

			// Token: 0x02003293 RID: 12947
			public class ANCIENTKNOWLEDGE
			{
				// Token: 0x0400D04F RID: 53327
				public static LocString NAME = "Ancient Knowledge";

				// Token: 0x0400D050 RID: 53328
				public static LocString DESC = "This Duplicant has knowledge from the before times\n• Starts with 3 skill points";
			}

			// Token: 0x02003294 RID: 12948
			public class CANTRESEARCH
			{
				// Token: 0x0400D051 RID: 53329
				public static LocString NAME = "Yokel";

				// Token: 0x0400D052 RID: 53330
				public static LocString DESC = "This Duplicant isn't the brightest star in the solar system";
			}

			// Token: 0x02003295 RID: 12949
			public class CANTBUILD
			{
				// Token: 0x0400D053 RID: 53331
				public static LocString NAME = "Unconstructive";

				// Token: 0x0400D054 RID: 53332
				public static LocString DESC = "This Duplicant is incapable of building even the most basic of structures";
			}

			// Token: 0x02003296 RID: 12950
			public class CANTCOOK
			{
				// Token: 0x0400D055 RID: 53333
				public static LocString NAME = "Gastrophobia";

				// Token: 0x0400D056 RID: 53334
				public static LocString DESC = "This Duplicant has a deep-seated distrust of the culinary arts";
			}

			// Token: 0x02003297 RID: 12951
			public class CANTDIG
			{
				// Token: 0x0400D057 RID: 53335
				public static LocString NAME = "Trypophobia";

				// Token: 0x0400D058 RID: 53336
				public static LocString DESC = "This Duplicant's fear of holes makes it impossible for them to dig";
			}

			// Token: 0x02003298 RID: 12952
			public class HEMOPHOBIA
			{
				// Token: 0x0400D059 RID: 53337
				public static LocString NAME = "Squeamish";

				// Token: 0x0400D05A RID: 53338
				public static LocString DESC = "This Duplicant is of delicate disposition and cannot tend to the sick";
			}

			// Token: 0x02003299 RID: 12953
			public class BEDSIDEMANNER
			{
				// Token: 0x0400D05B RID: 53339
				public static LocString NAME = "Caregiver";

				// Token: 0x0400D05C RID: 53340
				public static LocString DESC = "This Duplicant has good bedside manner and a healing touch";
			}

			// Token: 0x0200329A RID: 12954
			public class MOUTHBREATHER
			{
				// Token: 0x0400D05D RID: 53341
				public static LocString NAME = "Mouth Breather";

				// Token: 0x0400D05E RID: 53342
				public static LocString DESC = "This Duplicant sucks up way more than their fair share of " + ELEMENTS.OXYGEN.NAME;
			}

			// Token: 0x0200329B RID: 12955
			public class FUSSY
			{
				// Token: 0x0400D05F RID: 53343
				public static LocString NAME = "Fussy";

				// Token: 0x0400D060 RID: 53344
				public static LocString DESC = "Nothing's ever quite good enough for this Duplicant";
			}

			// Token: 0x0200329C RID: 12956
			public class TWINKLETOES
			{
				// Token: 0x0400D061 RID: 53345
				public static LocString NAME = "Twinkletoes";

				// Token: 0x0400D062 RID: 53346
				public static LocString DESC = "This Duplicant is light as a feather on their feet";
			}

			// Token: 0x0200329D RID: 12957
			public class STRONGARM
			{
				// Token: 0x0400D063 RID: 53347
				public static LocString NAME = "Buff";

				// Token: 0x0400D064 RID: 53348
				public static LocString DESC = "This Duplicant has muscles on their muscles";
			}

			// Token: 0x0200329E RID: 12958
			public class NOODLEARMS
			{
				// Token: 0x0400D065 RID: 53349
				public static LocString NAME = "Noodle Arms";

				// Token: 0x0400D066 RID: 53350
				public static LocString DESC = "This Duplicant's arms have all the tensile strength of overcooked linguine";
			}

			// Token: 0x0200329F RID: 12959
			public class AGGRESSIVE
			{
				// Token: 0x0400D067 RID: 53351
				public static LocString NAME = "Destructive";

				// Token: 0x0400D068 RID: 53352
				public static LocString DESC = "This Duplicant handles stress by taking their frustrations out on defenseless machines";

				// Token: 0x0400D069 RID: 53353
				public static LocString NOREPAIR = "• Will not repair buildings while above 60% " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x020032A0 RID: 12960
			public class UGLYCRIER
			{
				// Token: 0x0400D06A RID: 53354
				public static LocString NAME = "Ugly Crier";

				// Token: 0x0400D06B RID: 53355
				public static LocString DESC = string.Concat(new string[]
				{
					"If this Duplicant gets too ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" it won't be pretty"
				});
			}

			// Token: 0x020032A1 RID: 12961
			public class BINGEEATER
			{
				// Token: 0x0400D06C RID: 53356
				public static LocString NAME = "Binge Eater";

				// Token: 0x0400D06D RID: 53357
				public static LocString DESC = "This Duplicant will dangerously overeat when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;
			}

			// Token: 0x020032A2 RID: 12962
			public class ANXIOUS
			{
				// Token: 0x0400D06E RID: 53358
				public static LocString NAME = "Anxious";

				// Token: 0x0400D06F RID: 53359
				public static LocString DESC = "This Duplicant collapses when put under too much pressure";
			}

			// Token: 0x020032A3 RID: 12963
			public class STRESSVOMITER
			{
				// Token: 0x0400D070 RID: 53360
				public static LocString NAME = "Vomiter";

				// Token: 0x0400D071 RID: 53361
				public static LocString DESC = "This Duplicant is liable to puke everywhere when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;
			}

			// Token: 0x020032A4 RID: 12964
			public class STRESSSHOCKER
			{
				// Token: 0x0400D072 RID: 53362
				public static LocString NAME = "Stunner";

				// Token: 0x0400D073 RID: 53363
				public static LocString DESC = "This Duplicant emits electrical shocks when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;

				// Token: 0x0400D074 RID: 53364
				public static LocString DRAIN_ATTRIBUTE = "Stress Zapping";
			}

			// Token: 0x020032A5 RID: 12965
			public class BANSHEE
			{
				// Token: 0x0400D075 RID: 53365
				public static LocString NAME = "Banshee";

				// Token: 0x0400D076 RID: 53366
				public static LocString DESC = "This Duplicant wails uncontrollably when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;
			}

			// Token: 0x020032A6 RID: 12966
			public class BALLOONARTIST
			{
				// Token: 0x0400D077 RID: 53367
				public static LocString NAME = "Balloon Artist";

				// Token: 0x0400D078 RID: 53368
				public static LocString DESC = "This Duplicant hands out balloons when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020032A7 RID: 12967
			public class SPARKLESTREAKER
			{
				// Token: 0x0400D079 RID: 53369
				public static LocString NAME = "Sparkle Streaker";

				// Token: 0x0400D07A RID: 53370
				public static LocString DESC = "This Duplicant leaves a trail of happy sparkles when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020032A8 RID: 12968
			public class STICKERBOMBER
			{
				// Token: 0x0400D07B RID: 53371
				public static LocString NAME = "Sticker Bomber";

				// Token: 0x0400D07C RID: 53372
				public static LocString DESC = "This Duplicant will spontaneously redecorate a room when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020032A9 RID: 12969
			public class SUPERPRODUCTIVE
			{
				// Token: 0x0400D07D RID: 53373
				public static LocString NAME = "Super Productive";

				// Token: 0x0400D07E RID: 53374
				public static LocString DESC = "This Duplicant is super productive when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020032AA RID: 12970
			public class HAPPYSINGER
			{
				// Token: 0x0400D07F RID: 53375
				public static LocString NAME = "Yodeler";

				// Token: 0x0400D080 RID: 53376
				public static LocString DESC = "This Duplicant belts out catchy tunes when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020032AB RID: 12971
			public class DATARAINER
			{
				// Token: 0x0400D081 RID: 53377
				public static LocString NAME = "Rainmaker";

				// Token: 0x0400D082 RID: 53378
				public static LocString DESC = "This Duplicant distributes microchips when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020032AC RID: 12972
			public class ROBODANCER
			{
				// Token: 0x0400D083 RID: 53379
				public static LocString NAME = "Flash Mobber";

				// Token: 0x0400D084 RID: 53380
				public static LocString DESC = "This Duplicant breaks into dance when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020032AD RID: 12973
			public class IRONGUT
			{
				// Token: 0x0400D085 RID: 53381
				public static LocString NAME = "Iron Gut";

				// Token: 0x0400D086 RID: 53382
				public static LocString DESC = "This Duplicant can eat just about anything without getting sick";

				// Token: 0x0400D087 RID: 53383
				public static LocString SHORT_DESC = "Immune to <b>" + DUPLICANTS.DISEASES.FOODSICKNESS.NAME + "</b>";

				// Token: 0x0400D088 RID: 53384
				public static LocString SHORT_DESC_TOOLTIP = "Eating food contaminated with " + DUPLICANTS.DISEASES.FOODSICKNESS.NAME + " Germs will not affect this Duplicant";
			}

			// Token: 0x020032AE RID: 12974
			public class STRONGIMMUNESYSTEM
			{
				// Token: 0x0400D089 RID: 53385
				public static LocString NAME = "Germ Resistant";

				// Token: 0x0400D08A RID: 53386
				public static LocString DESC = "This Duplicant's immune system bounces back faster than most";
			}

			// Token: 0x020032AF RID: 12975
			public class SCAREDYCAT
			{
				// Token: 0x0400D08B RID: 53387
				public static LocString NAME = "Pacifist";

				// Token: 0x0400D08C RID: 53388
				public static LocString DESC = "This Duplicant abhors violence";
			}

			// Token: 0x020032B0 RID: 12976
			public class ALLERGIES
			{
				// Token: 0x0400D08D RID: 53389
				public static LocString NAME = "Allergies";

				// Token: 0x0400D08E RID: 53390
				public static LocString DESC = "This Duplicant will sneeze uncontrollably when exposed to the pollen present in " + DUPLICANTS.DISEASES.POLLENGERMS.NAME;

				// Token: 0x0400D08F RID: 53391
				public static LocString SHORT_DESC = "Allergic reaction to <b>" + DUPLICANTS.DISEASES.POLLENGERMS.NAME + "</b>";

				// Token: 0x0400D090 RID: 53392
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.DISEASES.ALLERGIES.DESCRIPTIVE_SYMPTOMS;
			}

			// Token: 0x020032B1 RID: 12977
			public class WEAKIMMUNESYSTEM
			{
				// Token: 0x0400D091 RID: 53393
				public static LocString NAME = "Biohazardous";

				// Token: 0x0400D092 RID: 53394
				public static LocString DESC = "All the vitamin C in space couldn't stop this Duplicant from getting sick";
			}

			// Token: 0x020032B2 RID: 12978
			public class IRRITABLEBOWEL
			{
				// Token: 0x0400D093 RID: 53395
				public static LocString NAME = "Irritable Bowel";

				// Token: 0x0400D094 RID: 53396
				public static LocString DESC = "This Duplicant needs a little extra time to \"do their business\"";
			}

			// Token: 0x020032B3 RID: 12979
			public class CALORIEBURNER
			{
				// Token: 0x0400D095 RID: 53397
				public static LocString NAME = "Bottomless Stomach";

				// Token: 0x0400D096 RID: 53398
				public static LocString DESC = "This Duplicant might actually be several black holes in a trench coat";
			}

			// Token: 0x020032B4 RID: 12980
			public class SMALLBLADDER
			{
				// Token: 0x0400D097 RID: 53399
				public static LocString NAME = "Small Bladder";

				// Token: 0x0400D098 RID: 53400
				public static LocString DESC = string.Concat(new string[]
				{
					"This Duplicant has a tiny, pea-sized ",
					UI.PRE_KEYWORD,
					"Bladder",
					UI.PST_KEYWORD,
					". Adorable!"
				});
			}

			// Token: 0x020032B5 RID: 12981
			public class ANEMIC
			{
				// Token: 0x0400D099 RID: 53401
				public static LocString NAME = "Anemic";

				// Token: 0x0400D09A RID: 53402
				public static LocString DESC = "This Duplicant has trouble keeping up with the others";
			}

			// Token: 0x020032B6 RID: 12982
			public class GREASEMONKEY
			{
				// Token: 0x0400D09B RID: 53403
				public static LocString NAME = "Grease Monkey";

				// Token: 0x0400D09C RID: 53404
				public static LocString DESC = "This Duplicant likes to throw a wrench into the colony's plans... in a good way";
			}

			// Token: 0x020032B7 RID: 12983
			public class MOLEHANDS
			{
				// Token: 0x0400D09D RID: 53405
				public static LocString NAME = "Mole Hands";

				// Token: 0x0400D09E RID: 53406
				public static LocString DESC = "They're great for tunneling, but finding good gloves is a nightmare";
			}

			// Token: 0x020032B8 RID: 12984
			public class FASTLEARNER
			{
				// Token: 0x0400D09F RID: 53407
				public static LocString NAME = "Quick Learner";

				// Token: 0x0400D0A0 RID: 53408
				public static LocString DESC = "This Duplicant's sharp as a tack and learns new skills with amazing speed";
			}

			// Token: 0x020032B9 RID: 12985
			public class SLOWLEARNER
			{
				// Token: 0x0400D0A1 RID: 53409
				public static LocString NAME = "Slow Learner";

				// Token: 0x0400D0A2 RID: 53410
				public static LocString DESC = "This Duplicant's a little slow on the uptake, but gosh do they try";
			}

			// Token: 0x020032BA RID: 12986
			public class DIVERSLUNG
			{
				// Token: 0x0400D0A3 RID: 53411
				public static LocString NAME = "Diver's Lungs";

				// Token: 0x0400D0A4 RID: 53412
				public static LocString DESC = "This Duplicant could have been a talented opera singer in another life";
			}

			// Token: 0x020032BB RID: 12987
			public class FLATULENCE
			{
				// Token: 0x0400D0A5 RID: 53413
				public static LocString NAME = "Flatulent";

				// Token: 0x0400D0A6 RID: 53414
				public static LocString DESC = "Some Duplicants are just full of it";

				// Token: 0x0400D0A7 RID: 53415
				public static LocString SHORT_DESC = "Farts frequently";

				// Token: 0x0400D0A8 RID: 53416
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant will periodically \"output\" " + ELEMENTS.METHANE.NAME;
			}

			// Token: 0x020032BC RID: 12988
			public class SNORER
			{
				// Token: 0x0400D0A9 RID: 53417
				public static LocString NAME = "Loud Sleeper";

				// Token: 0x0400D0AA RID: 53418
				public static LocString DESC = "In space, everyone can hear you snore";

				// Token: 0x0400D0AB RID: 53419
				public static LocString SHORT_DESC = "Snores loudly";

				// Token: 0x0400D0AC RID: 53420
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant's snoring will rudely awake nearby friends";
			}

			// Token: 0x020032BD RID: 12989
			public class NARCOLEPSY
			{
				// Token: 0x0400D0AD RID: 53421
				public static LocString NAME = "Narcoleptic";

				// Token: 0x0400D0AE RID: 53422
				public static LocString DESC = "This Duplicant can and will fall asleep anytime, anyplace";

				// Token: 0x0400D0AF RID: 53423
				public static LocString SHORT_DESC = "Falls asleep periodically";

				// Token: 0x0400D0B0 RID: 53424
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant's work will be periodically interrupted by naps";
			}

			// Token: 0x020032BE RID: 12990
			public class INTERIORDECORATOR
			{
				// Token: 0x0400D0B1 RID: 53425
				public static LocString NAME = "Interior Decorator";

				// Token: 0x0400D0B2 RID: 53426
				public static LocString DESC = "\"Move it a little to the left...\"";
			}

			// Token: 0x020032BF RID: 12991
			public class UNCULTURED
			{
				// Token: 0x0400D0B3 RID: 53427
				public static LocString NAME = "Uncultured";

				// Token: 0x0400D0B4 RID: 53428
				public static LocString DESC = "This Duplicant has simply no appreciation for the arts";
			}

			// Token: 0x020032C0 RID: 12992
			public class EARLYBIRD
			{
				// Token: 0x0400D0B5 RID: 53429
				public static LocString NAME = "Early Bird";

				// Token: 0x0400D0B6 RID: 53430
				public static LocString DESC = "This Duplicant always wakes up feeling fresh and efficient!";

				// Token: 0x0400D0B7 RID: 53431
				public static LocString EXTENDED_DESC = string.Concat(new string[]
				{
					"• Morning: <b>{0}</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: 5 Schedule Blocks"
				});

				// Token: 0x0400D0B8 RID: 53432
				public static LocString SHORT_DESC = "Gains morning Attribute bonuses";

				// Token: 0x0400D0B9 RID: 53433
				public static LocString SHORT_DESC_TOOLTIP = string.Concat(new string[]
				{
					"Morning: <b>+2</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: 5 Schedule Blocks"
				});
			}

			// Token: 0x020032C1 RID: 12993
			public class NIGHTOWL
			{
				// Token: 0x0400D0BA RID: 53434
				public static LocString NAME = "Night Owl";

				// Token: 0x0400D0BB RID: 53435
				public static LocString DESC = "This Duplicant does their best work when they'd ought to be sleeping";

				// Token: 0x0400D0BC RID: 53436
				public static LocString EXTENDED_DESC = string.Concat(new string[]
				{
					"• Nighttime: <b>{0}</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: All Night"
				});

				// Token: 0x0400D0BD RID: 53437
				public static LocString SHORT_DESC = "Gains nighttime Attribute bonuses";

				// Token: 0x0400D0BE RID: 53438
				public static LocString SHORT_DESC_TOOLTIP = string.Concat(new string[]
				{
					"Nighttime: <b>+3</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: All Night"
				});
			}

			// Token: 0x020032C2 RID: 12994
			public class METEORPHILE
			{
				// Token: 0x0400D0BF RID: 53439
				public static LocString NAME = "Rock Fan";

				// Token: 0x0400D0C0 RID: 53440
				public static LocString DESC = "Meteor showers get this Duplicant really, really hyped";

				// Token: 0x0400D0C1 RID: 53441
				public static LocString EXTENDED_DESC = "• During meteor showers: <b>{0}</b> bonus to all " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD;

				// Token: 0x0400D0C2 RID: 53442
				public static LocString SHORT_DESC = "Gains Attribute bonuses during meteor showers.";

				// Token: 0x0400D0C3 RID: 53443
				public static LocString SHORT_DESC_TOOLTIP = "During meteor showers: <b>+3</b> bonus to all " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD;
			}

			// Token: 0x020032C3 RID: 12995
			public class REGENERATION
			{
				// Token: 0x0400D0C4 RID: 53444
				public static LocString NAME = "Regenerative";

				// Token: 0x0400D0C5 RID: 53445
				public static LocString DESC = "This robust Duplicant is constantly regenerating health";
			}

			// Token: 0x020032C4 RID: 12996
			public class DEEPERDIVERSLUNGS
			{
				// Token: 0x0400D0C6 RID: 53446
				public static LocString NAME = "Deep Diver's Lungs";

				// Token: 0x0400D0C7 RID: 53447
				public static LocString DESC = "This Duplicant has a frankly impressive ability to hold their breath";
			}

			// Token: 0x020032C5 RID: 12997
			public class SUNNYDISPOSITION
			{
				// Token: 0x0400D0C8 RID: 53448
				public static LocString NAME = "Sunny Disposition";

				// Token: 0x0400D0C9 RID: 53449
				public static LocString DESC = "This Duplicant has an unwaveringly positive outlook on life";
			}

			// Token: 0x020032C6 RID: 12998
			public class ROCKCRUSHER
			{
				// Token: 0x0400D0CA RID: 53450
				public static LocString NAME = "Beefsteak";

				// Token: 0x0400D0CB RID: 53451
				public static LocString DESC = "This Duplicant's got muscles on their muscles!";
			}

			// Token: 0x020032C7 RID: 12999
			public class SIMPLETASTES
			{
				// Token: 0x0400D0CC RID: 53452
				public static LocString NAME = "Shrivelled Tastebuds";

				// Token: 0x0400D0CD RID: 53453
				public static LocString DESC = "This Duplicant could lick a Puft's backside and taste nothing";
			}

			// Token: 0x020032C8 RID: 13000
			public class FOODIE
			{
				// Token: 0x0400D0CE RID: 53454
				public static LocString NAME = "Gourmet";

				// Token: 0x0400D0CF RID: 53455
				public static LocString DESC = "This Duplicant's refined palate demands only the most luxurious dishes the colony can offer";
			}

			// Token: 0x020032C9 RID: 13001
			public class ARCHAEOLOGIST
			{
				// Token: 0x0400D0D0 RID: 53456
				public static LocString NAME = "Relic Hunter";

				// Token: 0x0400D0D1 RID: 53457
				public static LocString DESC = "This Duplicant was never taught the phrase \"take only pictures, leave only footprints\"";
			}

			// Token: 0x020032CA RID: 13002
			public class DECORUP
			{
				// Token: 0x0400D0D2 RID: 53458
				public static LocString NAME = "Innately Stylish";

				// Token: 0x0400D0D3 RID: 53459
				public static LocString DESC = "This Duplicant's radiant self-confidence makes even the rattiest outfits look trendy";
			}

			// Token: 0x020032CB RID: 13003
			public class DECORDOWN
			{
				// Token: 0x0400D0D4 RID: 53460
				public static LocString NAME = "Shabby Dresser";

				// Token: 0x0400D0D5 RID: 53461
				public static LocString DESC = "This Duplicant's clearly never heard of ironing";
			}

			// Token: 0x020032CC RID: 13004
			public class THRIVER
			{
				// Token: 0x0400D0D6 RID: 53462
				public static LocString NAME = "Duress to Impress";

				// Token: 0x0400D0D7 RID: 53463
				public static LocString DESC = "This Duplicant kicks into hyperdrive when the stress is on";

				// Token: 0x0400D0D8 RID: 53464
				public static LocString SHORT_DESC = "Attribute bonuses while stressed";

				// Token: 0x0400D0D9 RID: 53465
				public static LocString SHORT_DESC_TOOLTIP = "More than 60% Stress: <b>+7</b> bonus to all " + UI.FormatAsKeyWord("Attributes");
			}

			// Token: 0x020032CD RID: 13005
			public class LONER
			{
				// Token: 0x0400D0DA RID: 53466
				public static LocString NAME = "Loner";

				// Token: 0x0400D0DB RID: 53467
				public static LocString DESC = "This Duplicant prefers solitary pursuits";

				// Token: 0x0400D0DC RID: 53468
				public static LocString SHORT_DESC = "Attribute bonuses while alone";

				// Token: 0x0400D0DD RID: 53469
				public static LocString SHORT_DESC_TOOLTIP = "Only Duplicant on a world: <b>+4</b> bonus to all " + UI.FormatAsKeyWord("Attributes");
			}

			// Token: 0x020032CE RID: 13006
			public class STARRYEYED
			{
				// Token: 0x0400D0DE RID: 53470
				public static LocString NAME = "Starry Eyed";

				// Token: 0x0400D0DF RID: 53471
				public static LocString DESC = "This Duplicant loves being in space";

				// Token: 0x0400D0E0 RID: 53472
				public static LocString SHORT_DESC = "Morale bonus while in space";

				// Token: 0x0400D0E1 RID: 53473
				public static LocString SHORT_DESC_TOOLTIP = "In outer space: <b>+10</b> " + UI.FormatAsKeyWord("Morale");
			}

			// Token: 0x020032CF RID: 13007
			public class GLOWSTICK
			{
				// Token: 0x0400D0E2 RID: 53474
				public static LocString NAME = "Glow Stick";

				// Token: 0x0400D0E3 RID: 53475
				public static LocString DESC = "This Duplicant is positively glowing";

				// Token: 0x0400D0E4 RID: 53476
				public static LocString SHORT_DESC = "Emits low amounts of rads and light";

				// Token: 0x0400D0E5 RID: 53477
				public static LocString SHORT_DESC_TOOLTIP = "Emits low amounts of rads and light";
			}

			// Token: 0x020032D0 RID: 13008
			public class RADIATIONEATER
			{
				// Token: 0x0400D0E6 RID: 53478
				public static LocString NAME = "Radiation Eater";

				// Token: 0x0400D0E7 RID: 53479
				public static LocString DESC = "This Duplicant eats radiation for breakfast (and dinner)";

				// Token: 0x0400D0E8 RID: 53480
				public static LocString SHORT_DESC = "Converts radiation exposure into calories";

				// Token: 0x0400D0E9 RID: 53481
				public static LocString SHORT_DESC_TOOLTIP = "Converts radiation exposure into calories";
			}

			// Token: 0x020032D1 RID: 13009
			public class NIGHTLIGHT
			{
				// Token: 0x0400D0EA RID: 53482
				public static LocString NAME = "Nyctophobic";

				// Token: 0x0400D0EB RID: 53483
				public static LocString DESC = "This Duplicant will imagine scary shapes in the dark all night if no one leaves a light on";

				// Token: 0x0400D0EC RID: 53484
				public static LocString SHORT_DESC = "Requires light to sleep";

				// Token: 0x0400D0ED RID: 53485
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant can't sleep in complete darkness";
			}

			// Token: 0x020032D2 RID: 13010
			public class GREENTHUMB
			{
				// Token: 0x0400D0EE RID: 53486
				public static LocString NAME = "Green Thumb";

				// Token: 0x0400D0EF RID: 53487
				public static LocString DESC = "This Duplicant regards every plant as a potential friend";
			}

			// Token: 0x020032D3 RID: 13011
			public class FROSTPROOF
			{
				// Token: 0x0400D0F0 RID: 53488
				public static LocString NAME = "Frost Proof";

				// Token: 0x0400D0F1 RID: 53489
				public static LocString DESC = "This Duplicant is too cool to be bothered by the cold";
			}

			// Token: 0x020032D4 RID: 13012
			public class CONSTRUCTIONUP
			{
				// Token: 0x0400D0F2 RID: 53490
				public static LocString NAME = "Handy";

				// Token: 0x0400D0F3 RID: 53491
				public static LocString DESC = "This Duplicant is a swift and skilled builder";
			}

			// Token: 0x020032D5 RID: 13013
			public class RANCHINGUP
			{
				// Token: 0x0400D0F4 RID: 53492
				public static LocString NAME = "Animal Lover";

				// Token: 0x0400D0F5 RID: 53493
				public static LocString DESC = "The fuzzy snoots! The little claws! The chitinous exoskeletons! This Duplicant's never met a critter they didn't like";
			}

			// Token: 0x020032D6 RID: 13014
			public class CONSTRUCTIONDOWN
			{
				// Token: 0x0400D0F6 RID: 53494
				public static LocString NAME = "Building Impaired";

				// Token: 0x0400D0F7 RID: 53495
				public static LocString DESC = "This Duplicant has trouble constructing anything besides meaningful friendships";
			}

			// Token: 0x020032D7 RID: 13015
			public class RANCHINGDOWN
			{
				// Token: 0x0400D0F8 RID: 53496
				public static LocString NAME = "Critter Aversion";

				// Token: 0x0400D0F9 RID: 53497
				public static LocString DESC = "This Duplicant just doesn't trust those beady little eyes";
			}

			// Token: 0x020032D8 RID: 13016
			public class DIGGINGDOWN
			{
				// Token: 0x0400D0FA RID: 53498
				public static LocString NAME = "Undigging";

				// Token: 0x0400D0FB RID: 53499
				public static LocString DESC = "This Duplicant couldn't dig themselves out of a paper bag";
			}

			// Token: 0x020032D9 RID: 13017
			public class MACHINERYDOWN
			{
				// Token: 0x0400D0FC RID: 53500
				public static LocString NAME = "Luddite";

				// Token: 0x0400D0FD RID: 53501
				public static LocString DESC = "This Duplicant always invites friends over just to make them hook up their electronics";
			}

			// Token: 0x020032DA RID: 13018
			public class COOKINGDOWN
			{
				// Token: 0x0400D0FE RID: 53502
				public static LocString NAME = "Kitchen Menace";

				// Token: 0x0400D0FF RID: 53503
				public static LocString DESC = "This Duplicant could probably figure out a way to burn ice cream";
			}

			// Token: 0x020032DB RID: 13019
			public class ARTDOWN
			{
				// Token: 0x0400D100 RID: 53504
				public static LocString NAME = "Unpracticed Artist";

				// Token: 0x0400D101 RID: 53505
				public static LocString DESC = "This Duplicant proudly proclaims they \"can't even draw a stick figure\"";
			}

			// Token: 0x020032DC RID: 13020
			public class CARINGDOWN
			{
				// Token: 0x0400D102 RID: 53506
				public static LocString NAME = "Unempathetic";

				// Token: 0x0400D103 RID: 53507
				public static LocString DESC = "This Duplicant's lack of bedside manner makes it difficult for them to nurse peers back to health";
			}

			// Token: 0x020032DD RID: 13021
			public class BOTANISTDOWN
			{
				// Token: 0x0400D104 RID: 53508
				public static LocString NAME = "Plant Murderer";

				// Token: 0x0400D105 RID: 53509
				public static LocString DESC = "Never ask this Duplicant to watch your ferns when you go on vacation";
			}

			// Token: 0x020032DE RID: 13022
			public class GRANTSKILL_MINING1
			{
				// Token: 0x0400D106 RID: 53510
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_MINER.NAME;

				// Token: 0x0400D107 RID: 53511
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_MINER.DESCRIPTION;

				// Token: 0x0400D108 RID: 53512
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D109 RID: 53513
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032DF RID: 13023
			public class GRANTSKILL_MINING2
			{
				// Token: 0x0400D10A RID: 53514
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MINER.NAME;

				// Token: 0x0400D10B RID: 53515
				public static LocString DESC = DUPLICANTS.ROLES.MINER.DESCRIPTION;

				// Token: 0x0400D10C RID: 53516
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D10D RID: 53517
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032E0 RID: 13024
			public class GRANTSKILL_MINING3
			{
				// Token: 0x0400D10E RID: 53518
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_MINER.NAME;

				// Token: 0x0400D10F RID: 53519
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_MINER.DESCRIPTION;

				// Token: 0x0400D110 RID: 53520
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D111 RID: 53521
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032E1 RID: 13025
			public class GRANTSKILL_MINING4
			{
				// Token: 0x0400D112 RID: 53522
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MASTER_MINER.NAME;

				// Token: 0x0400D113 RID: 53523
				public static LocString DESC = DUPLICANTS.ROLES.MASTER_MINER.DESCRIPTION;

				// Token: 0x0400D114 RID: 53524
				public static LocString SHORT_DESC = "Starts with a Tier 4 <b>Skill</b>";

				// Token: 0x0400D115 RID: 53525
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032E2 RID: 13026
			public class GRANTSKILL_BUILDING1
			{
				// Token: 0x0400D116 RID: 53526
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME;

				// Token: 0x0400D117 RID: 53527
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_BUILDER.DESCRIPTION;

				// Token: 0x0400D118 RID: 53528
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D119 RID: 53529
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032E3 RID: 13027
			public class GRANTSKILL_BUILDING2
			{
				// Token: 0x0400D11A RID: 53530
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.BUILDER.NAME;

				// Token: 0x0400D11B RID: 53531
				public static LocString DESC = DUPLICANTS.ROLES.BUILDER.DESCRIPTION;

				// Token: 0x0400D11C RID: 53532
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D11D RID: 53533
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032E4 RID: 13028
			public class GRANTSKILL_BUILDING3
			{
				// Token: 0x0400D11E RID: 53534
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_BUILDER.NAME;

				// Token: 0x0400D11F RID: 53535
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_BUILDER.DESCRIPTION;

				// Token: 0x0400D120 RID: 53536
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D121 RID: 53537
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032E5 RID: 13029
			public class GRANTSKILL_FARMING1
			{
				// Token: 0x0400D122 RID: 53538
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_FARMER.NAME;

				// Token: 0x0400D123 RID: 53539
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_FARMER.DESCRIPTION;

				// Token: 0x0400D124 RID: 53540
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D125 RID: 53541
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032E6 RID: 13030
			public class GRANTSKILL_FARMING2
			{
				// Token: 0x0400D126 RID: 53542
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.FARMER.NAME;

				// Token: 0x0400D127 RID: 53543
				public static LocString DESC = DUPLICANTS.ROLES.FARMER.DESCRIPTION;

				// Token: 0x0400D128 RID: 53544
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D129 RID: 53545
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032E7 RID: 13031
			public class GRANTSKILL_FARMING3
			{
				// Token: 0x0400D12A RID: 53546
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_FARMER.NAME;

				// Token: 0x0400D12B RID: 53547
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_FARMER.DESCRIPTION;

				// Token: 0x0400D12C RID: 53548
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D12D RID: 53549
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032E8 RID: 13032
			public class GRANTSKILL_RANCHING1
			{
				// Token: 0x0400D12E RID: 53550
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.RANCHER.NAME;

				// Token: 0x0400D12F RID: 53551
				public static LocString DESC = DUPLICANTS.ROLES.RANCHER.DESCRIPTION;

				// Token: 0x0400D130 RID: 53552
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D131 RID: 53553
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032E9 RID: 13033
			public class GRANTSKILL_RANCHING2
			{
				// Token: 0x0400D132 RID: 53554
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_RANCHER.NAME;

				// Token: 0x0400D133 RID: 53555
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_RANCHER.DESCRIPTION;

				// Token: 0x0400D134 RID: 53556
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D135 RID: 53557
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032EA RID: 13034
			public class GRANTSKILL_RESEARCHING1
			{
				// Token: 0x0400D136 RID: 53558
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME;

				// Token: 0x0400D137 RID: 53559
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_RESEARCHER.DESCRIPTION;

				// Token: 0x0400D138 RID: 53560
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D139 RID: 53561
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032EB RID: 13035
			public class GRANTSKILL_RESEARCHING2
			{
				// Token: 0x0400D13A RID: 53562
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.RESEARCHER.NAME;

				// Token: 0x0400D13B RID: 53563
				public static LocString DESC = DUPLICANTS.ROLES.RESEARCHER.DESCRIPTION;

				// Token: 0x0400D13C RID: 53564
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D13D RID: 53565
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032EC RID: 13036
			public class GRANTSKILL_RESEARCHING3
			{
				// Token: 0x0400D13E RID: 53566
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME;

				// Token: 0x0400D13F RID: 53567
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_RESEARCHER.DESCRIPTION;

				// Token: 0x0400D140 RID: 53568
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D141 RID: 53569
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032ED RID: 13037
			public class GRANTSKILL_RESEARCHING4
			{
				// Token: 0x0400D142 RID: 53570
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.NAME;

				// Token: 0x0400D143 RID: 53571
				public static LocString DESC = DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.DESCRIPTION;

				// Token: 0x0400D144 RID: 53572
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D145 RID: 53573
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032EE RID: 13038
			public class GRANTSKILL_COOKING1
			{
				// Token: 0x0400D146 RID: 53574
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_COOK.NAME;

				// Token: 0x0400D147 RID: 53575
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_COOK.DESCRIPTION;

				// Token: 0x0400D148 RID: 53576
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D149 RID: 53577
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032EF RID: 13039
			public class GRANTSKILL_COOKING2
			{
				// Token: 0x0400D14A RID: 53578
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.COOK.NAME;

				// Token: 0x0400D14B RID: 53579
				public static LocString DESC = DUPLICANTS.ROLES.COOK.DESCRIPTION;

				// Token: 0x0400D14C RID: 53580
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D14D RID: 53581
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032F0 RID: 13040
			public class GRANTSKILL_ARTING1
			{
				// Token: 0x0400D14E RID: 53582
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME;

				// Token: 0x0400D14F RID: 53583
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_ARTIST.DESCRIPTION;

				// Token: 0x0400D150 RID: 53584
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D151 RID: 53585
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032F1 RID: 13041
			public class GRANTSKILL_ARTING2
			{
				// Token: 0x0400D152 RID: 53586
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.ARTIST.NAME;

				// Token: 0x0400D153 RID: 53587
				public static LocString DESC = DUPLICANTS.ROLES.ARTIST.DESCRIPTION;

				// Token: 0x0400D154 RID: 53588
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D155 RID: 53589
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032F2 RID: 13042
			public class GRANTSKILL_ARTING3
			{
				// Token: 0x0400D156 RID: 53590
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MASTER_ARTIST.NAME;

				// Token: 0x0400D157 RID: 53591
				public static LocString DESC = DUPLICANTS.ROLES.MASTER_ARTIST.DESCRIPTION;

				// Token: 0x0400D158 RID: 53592
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D159 RID: 53593
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032F3 RID: 13043
			public class GRANTSKILL_HAULING1
			{
				// Token: 0x0400D15A RID: 53594
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.HAULER.NAME;

				// Token: 0x0400D15B RID: 53595
				public static LocString DESC = DUPLICANTS.ROLES.HAULER.DESCRIPTION;

				// Token: 0x0400D15C RID: 53596
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D15D RID: 53597
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032F4 RID: 13044
			public class GRANTSKILL_HAULING2
			{
				// Token: 0x0400D15E RID: 53598
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME;

				// Token: 0x0400D15F RID: 53599
				public static LocString DESC = DUPLICANTS.ROLES.MATERIALS_MANAGER.DESCRIPTION;

				// Token: 0x0400D160 RID: 53600
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D161 RID: 53601
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032F5 RID: 13045
			public class GRANTSKILL_SUITS1
			{
				// Token: 0x0400D162 RID: 53602
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SUIT_EXPERT.NAME;

				// Token: 0x0400D163 RID: 53603
				public static LocString DESC = DUPLICANTS.ROLES.SUIT_EXPERT.DESCRIPTION;

				// Token: 0x0400D164 RID: 53604
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D165 RID: 53605
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032F6 RID: 13046
			public class GRANTSKILL_TECHNICALS1
			{
				// Token: 0x0400D166 RID: 53606
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME;

				// Token: 0x0400D167 RID: 53607
				public static LocString DESC = DUPLICANTS.ROLES.MACHINE_TECHNICIAN.DESCRIPTION;

				// Token: 0x0400D168 RID: 53608
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D169 RID: 53609
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032F7 RID: 13047
			public class GRANTSKILL_TECHNICALS2
			{
				// Token: 0x0400D16A RID: 53610
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME;

				// Token: 0x0400D16B RID: 53611
				public static LocString DESC = DUPLICANTS.ROLES.POWER_TECHNICIAN.DESCRIPTION;

				// Token: 0x0400D16C RID: 53612
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D16D RID: 53613
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032F8 RID: 13048
			public class GRANTSKILL_ENGINEERING1
			{
				// Token: 0x0400D16E RID: 53614
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME;

				// Token: 0x0400D16F RID: 53615
				public static LocString DESC = DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.DESCRIPTION;

				// Token: 0x0400D170 RID: 53616
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D171 RID: 53617
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032F9 RID: 13049
			public class GRANTSKILL_BASEKEEPING1
			{
				// Token: 0x0400D172 RID: 53618
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.HANDYMAN.NAME;

				// Token: 0x0400D173 RID: 53619
				public static LocString DESC = DUPLICANTS.ROLES.HANDYMAN.DESCRIPTION;

				// Token: 0x0400D174 RID: 53620
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D175 RID: 53621
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032FA RID: 13050
			public class GRANTSKILL_BASEKEEPING2
			{
				// Token: 0x0400D176 RID: 53622
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.PLUMBER.NAME;

				// Token: 0x0400D177 RID: 53623
				public static LocString DESC = DUPLICANTS.ROLES.PLUMBER.DESCRIPTION;

				// Token: 0x0400D178 RID: 53624
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D179 RID: 53625
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032FB RID: 13051
			public class GRANTSKILL_ASTRONAUTING1
			{
				// Token: 0x0400D17A RID: 53626
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.ASTRONAUTTRAINEE.NAME;

				// Token: 0x0400D17B RID: 53627
				public static LocString DESC = DUPLICANTS.ROLES.ASTRONAUTTRAINEE.DESCRIPTION;

				// Token: 0x0400D17C RID: 53628
				public static LocString SHORT_DESC = "Starts with a Tier 4 <b>Skill</b>";

				// Token: 0x0400D17D RID: 53629
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032FC RID: 13052
			public class GRANTSKILL_ASTRONAUTING2
			{
				// Token: 0x0400D17E RID: 53630
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.ASTRONAUT.NAME;

				// Token: 0x0400D17F RID: 53631
				public static LocString DESC = DUPLICANTS.ROLES.ASTRONAUT.DESCRIPTION;

				// Token: 0x0400D180 RID: 53632
				public static LocString SHORT_DESC = "Starts with a Tier 5 <b>Skill</b>";

				// Token: 0x0400D181 RID: 53633
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032FD RID: 13053
			public class GRANTSKILL_MEDICINE1
			{
				// Token: 0x0400D182 RID: 53634
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME;

				// Token: 0x0400D183 RID: 53635
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_MEDIC.DESCRIPTION;

				// Token: 0x0400D184 RID: 53636
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D185 RID: 53637
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032FE RID: 13054
			public class GRANTSKILL_MEDICINE2
			{
				// Token: 0x0400D186 RID: 53638
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MEDIC.NAME;

				// Token: 0x0400D187 RID: 53639
				public static LocString DESC = DUPLICANTS.ROLES.MEDIC.DESCRIPTION;

				// Token: 0x0400D188 RID: 53640
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D189 RID: 53641
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x020032FF RID: 13055
			public class GRANTSKILL_MEDICINE3
			{
				// Token: 0x0400D18A RID: 53642
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_MEDIC.NAME;

				// Token: 0x0400D18B RID: 53643
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_MEDIC.DESCRIPTION;

				// Token: 0x0400D18C RID: 53644
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D18D RID: 53645
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003300 RID: 13056
			public class GRANTSKILL_PYROTECHNICS
			{
				// Token: 0x0400D18E RID: 53646
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.PYROTECHNIC.NAME;

				// Token: 0x0400D18F RID: 53647
				public static LocString DESC = DUPLICANTS.ROLES.PYROTECHNIC.DESCRIPTION;

				// Token: 0x0400D190 RID: 53648
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D191 RID: 53649
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003301 RID: 13057
			public class BIONICBASELINE
			{
				// Token: 0x0400D192 RID: 53650
				public static LocString NAME = "Bionic Bug: Gormless";

				// Token: 0x0400D193 RID: 53651
				public static LocString DESC = "This Duplicant has built-in limitations that cannot be changed";

				// Token: 0x0400D194 RID: 53652
				public static LocString SHORT_DESC = "";

				// Token: 0x0400D195 RID: 53653
				public static LocString SHORT_DESC_TOOLTIP = "Intelligence is one thing, instinct is another. This Duplicant struggles with both";
			}

			// Token: 0x02003302 RID: 13058
			public class DEFAULTBIONICBOOSTDIGGING
			{
				// Token: 0x0400D196 RID: 53654
				public static LocString NAME = "Bionic Booster: Excavation";

				// Token: 0x0400D197 RID: 53655
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Excavation Booster</b>";

				// Token: 0x0400D198 RID: 53656
				public static LocString SHORT_DESC = "Increased <b>Digging</b>";

				// Token: 0x0400D199 RID: 53657
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Excavation Booster gives them increased <b>Digging</b>";
			}

			// Token: 0x02003303 RID: 13059
			public class DEFAULTBIONICBOOSTBUILDING
			{
				// Token: 0x0400D19A RID: 53658
				public static LocString NAME = "Bionic Booster: Building";

				// Token: 0x0400D19B RID: 53659
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Building Booster</b>";

				// Token: 0x0400D19C RID: 53660
				public static LocString SHORT_DESC = "Increased <b>Construction</b>";

				// Token: 0x0400D19D RID: 53661
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Building Booster gives them increased <b>Construction</b>";
			}

			// Token: 0x02003304 RID: 13060
			public class DEFAULTBIONICBOOSTCOOKING
			{
				// Token: 0x0400D19E RID: 53662
				public static LocString NAME = "Bionic Booster: Cooking";

				// Token: 0x0400D19F RID: 53663
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Cooking Booster</b>";

				// Token: 0x0400D1A0 RID: 53664
				public static LocString SHORT_DESC = "Increased <b>Cuisine</b>";

				// Token: 0x0400D1A1 RID: 53665
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Cooking Booster gives them increased <b>Cuisine</b>";
			}

			// Token: 0x02003305 RID: 13061
			public class DEFAULTBIONICBOOSTART
			{
				// Token: 0x0400D1A2 RID: 53666
				public static LocString NAME = "Bionic Booster: Decorating";

				// Token: 0x0400D1A3 RID: 53667
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Decorating Booster</b>";

				// Token: 0x0400D1A4 RID: 53668
				public static LocString SHORT_DESC = "Increased <b>Creativity</b>";

				// Token: 0x0400D1A5 RID: 53669
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Decorating Booster gives them increased <b>Creativity</b>";
			}

			// Token: 0x02003306 RID: 13062
			public class DEFAULTBIONICBOOSTFARMING
			{
				// Token: 0x0400D1A6 RID: 53670
				public static LocString NAME = "Bionic Booster: Farming";

				// Token: 0x0400D1A7 RID: 53671
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Farming Booster</b>";

				// Token: 0x0400D1A8 RID: 53672
				public static LocString SHORT_DESC = "Increased <b>Agriculture</b>";

				// Token: 0x0400D1A9 RID: 53673
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Farming Booster gives them increased <b>Agriculture</b>";
			}

			// Token: 0x02003307 RID: 13063
			public class DEFAULTBIONICBOOSTRANCHING
			{
				// Token: 0x0400D1AA RID: 53674
				public static LocString NAME = "Bionic Booster: Ranching";

				// Token: 0x0400D1AB RID: 53675
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Ranching Booster</b>";

				// Token: 0x0400D1AC RID: 53676
				public static LocString SHORT_DESC = "Increased <b>Husbandry</b>";

				// Token: 0x0400D1AD RID: 53677
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Ranching Booster gives them increased <b>Husbandry</b>";
			}

			// Token: 0x02003308 RID: 13064
			public class DEFAULTBIONICBOOSTMEDICINE
			{
				// Token: 0x0400D1AE RID: 53678
				public static LocString NAME = "Bionic Booster: Doctoring";

				// Token: 0x0400D1AF RID: 53679
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Doctoring Booster</b>";

				// Token: 0x0400D1B0 RID: 53680
				public static LocString SHORT_DESC = "Increased <b>Medicine</b>";

				// Token: 0x0400D1B1 RID: 53681
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Doctoring Booster gives them increased <b>Medicine</b>";
			}

			// Token: 0x02003309 RID: 13065
			public class DEFAULTBIONICBOOSTEXPLORER
			{
				// Token: 0x0400D1B2 RID: 53682
				public static LocString NAME = "Bionic Booster: Dowsing";

				// Token: 0x0400D1B3 RID: 53683
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Dowsing Booster</b>";

				// Token: 0x0400D1B4 RID: 53684
				public static LocString SHORT_DESC = "Locates undiscovered geysers";

				// Token: 0x0400D1B5 RID: 53685
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Dowsing Booster allows them to locate undiscovered geysers";
			}
		}

		// Token: 0x020021B8 RID: 8632
		public class PERSONALITIES
		{
			// Token: 0x0200330A RID: 13066
			public class CATALINA
			{
				// Token: 0x0400D1B6 RID: 53686
				public static LocString NAME = "Catalina";

				// Token: 0x0400D1B7 RID: 53687
				public static LocString DESC = "A {0} is admired by all for her seemingly tireless work ethic. Little do people know, she's dying on the inside.";
			}

			// Token: 0x0200330B RID: 13067
			public class NISBET
			{
				// Token: 0x0400D1B8 RID: 53688
				public static LocString NAME = "Nisbet";

				// Token: 0x0400D1B9 RID: 53689
				public static LocString DESC = "This {0} likes to punch people to show her affection. Everyone's too afraid of her to tell her it hurts.";
			}

			// Token: 0x0200330C RID: 13068
			public class ELLIE
			{
				// Token: 0x0400D1BA RID: 53690
				public static LocString NAME = "Ellie";

				// Token: 0x0400D1BB RID: 53691
				public static LocString DESC = "Nothing makes an {0} happier than a big tin of glitter and a pack of unicorn stickers.";
			}

			// Token: 0x0200330D RID: 13069
			public class RUBY
			{
				// Token: 0x0400D1BC RID: 53692
				public static LocString NAME = "Ruby";

				// Token: 0x0400D1BD RID: 53693
				public static LocString DESC = "This {0} asks the pressing questions, like \"Where can I get a leather jacket in space?\"";
			}

			// Token: 0x0200330E RID: 13070
			public class LEIRA
			{
				// Token: 0x0400D1BE RID: 53694
				public static LocString NAME = "Leira";

				// Token: 0x0400D1BF RID: 53695
				public static LocString DESC = "{0}s just want everyone to be happy.";
			}

			// Token: 0x0200330F RID: 13071
			public class BUBBLES
			{
				// Token: 0x0400D1C0 RID: 53696
				public static LocString NAME = "Bubbles";

				// Token: 0x0400D1C1 RID: 53697
				public static LocString DESC = "This {0} is constantly challenging others to fight her, regardless of whether or not she can actually take them.";
			}

			// Token: 0x02003310 RID: 13072
			public class MIMA
			{
				// Token: 0x0400D1C2 RID: 53698
				public static LocString NAME = "Mi-Ma";

				// Token: 0x0400D1C3 RID: 53699
				public static LocString DESC = "Ol' {0} here can't stand lookin' at people's knees.";
			}

			// Token: 0x02003311 RID: 13073
			public class NAILS
			{
				// Token: 0x0400D1C4 RID: 53700
				public static LocString NAME = "Nails";

				// Token: 0x0400D1C5 RID: 53701
				public static LocString DESC = "People often expect a Duplicant named \"{0}\" to be tough, but they're all pretty huge wimps.";
			}

			// Token: 0x02003312 RID: 13074
			public class MAE
			{
				// Token: 0x0400D1C6 RID: 53702
				public static LocString NAME = "Mae";

				// Token: 0x0400D1C7 RID: 53703
				public static LocString DESC = "There's nothing a {0} can't do if she sets her mind to it.";
			}

			// Token: 0x02003313 RID: 13075
			public class GOSSMANN
			{
				// Token: 0x0400D1C8 RID: 53704
				public static LocString NAME = "Gossmann";

				// Token: 0x0400D1C9 RID: 53705
				public static LocString DESC = "{0}s are major goofballs who can make anyone laugh.";
			}

			// Token: 0x02003314 RID: 13076
			public class MARIE
			{
				// Token: 0x0400D1CA RID: 53706
				public static LocString NAME = "Marie";

				// Token: 0x0400D1CB RID: 53707
				public static LocString DESC = "This {0} is positively glowing! What's her secret? Radioactive isotopes, of course.";
			}

			// Token: 0x02003315 RID: 13077
			public class LINDSAY
			{
				// Token: 0x0400D1CC RID: 53708
				public static LocString NAME = "Lindsay";

				// Token: 0x0400D1CD RID: 53709
				public static LocString DESC = "A {0} is a charming woman, unless you make the mistake of messing with one of her friends.";
			}

			// Token: 0x02003316 RID: 13078
			public class DEVON
			{
				// Token: 0x0400D1CE RID: 53710
				public static LocString NAME = "Devon";

				// Token: 0x0400D1CF RID: 53711
				public static LocString DESC = "This {0} dreams of owning their own personal computer so they can start a blog full of pictures of toast.";
			}

			// Token: 0x02003317 RID: 13079
			public class REN
			{
				// Token: 0x0400D1D0 RID: 53712
				public static LocString NAME = "Ren";

				// Token: 0x0400D1D1 RID: 53713
				public static LocString DESC = "Every {0} has this unshakable feeling that his life's already happened and he's just watching it unfold from a memory.";
			}

			// Token: 0x02003318 RID: 13080
			public class FRANKIE
			{
				// Token: 0x0400D1D2 RID: 53714
				public static LocString NAME = "Frankie";

				// Token: 0x0400D1D3 RID: 53715
				public static LocString DESC = "There's nothing {0}s are more proud of than their thick, dignified eyebrows.";
			}

			// Token: 0x02003319 RID: 13081
			public class BANHI
			{
				// Token: 0x0400D1D4 RID: 53716
				public static LocString NAME = "Banhi";

				// Token: 0x0400D1D5 RID: 53717
				public static LocString DESC = "The \"cool loner\" vibes that radiate off a {0} never fail to make the colony swoon.";
			}

			// Token: 0x0200331A RID: 13082
			public class ADA
			{
				// Token: 0x0400D1D6 RID: 53718
				public static LocString NAME = "Ada";

				// Token: 0x0400D1D7 RID: 53719
				public static LocString DESC = "{0}s enjoy writing poetry in their downtime. Dark poetry.";
			}

			// Token: 0x0200331B RID: 13083
			public class HASSAN
			{
				// Token: 0x0400D1D8 RID: 53720
				public static LocString NAME = "Hassan";

				// Token: 0x0400D1D9 RID: 53721
				public static LocString DESC = "If someone says something nice to a {0} he'll think about it nonstop for no less than three weeks.";
			}

			// Token: 0x0200331C RID: 13084
			public class STINKY
			{
				// Token: 0x0400D1DA RID: 53722
				public static LocString NAME = "Stinky";

				// Token: 0x0400D1DB RID: 53723
				public static LocString DESC = "This {0} has never been invited to a party, which is a shame. His dance moves are incredible.";
			}

			// Token: 0x0200331D RID: 13085
			public class JOSHUA
			{
				// Token: 0x0400D1DC RID: 53724
				public static LocString NAME = "Joshua";

				// Token: 0x0400D1DD RID: 53725
				public static LocString DESC = "{0}s are precious goobers. Other Duplicants are strangely incapable of cursing in a {0}'s presence.";
			}

			// Token: 0x0200331E RID: 13086
			public class LIAM
			{
				// Token: 0x0400D1DE RID: 53726
				public static LocString NAME = "Liam";

				// Token: 0x0400D1DF RID: 53727
				public static LocString DESC = "No matter how much this {0} scrubs, he can never truly feel clean.";
			}

			// Token: 0x0200331F RID: 13087
			public class ABE
			{
				// Token: 0x0400D1E0 RID: 53728
				public static LocString NAME = "Abe";

				// Token: 0x0400D1E1 RID: 53729
				public static LocString DESC = "{0}s are sweet, delicate flowers. They need to be treated gingerly, with great consideration for their feelings.";
			}

			// Token: 0x02003320 RID: 13088
			public class BURT
			{
				// Token: 0x0400D1E2 RID: 53730
				public static LocString NAME = "Burt";

				// Token: 0x0400D1E3 RID: 53731
				public static LocString DESC = "This {0} always feels great after a bubble bath and a good long cry.";
			}

			// Token: 0x02003321 RID: 13089
			public class TRAVALDO
			{
				// Token: 0x0400D1E4 RID: 53732
				public static LocString NAME = "Travaldo";

				// Token: 0x0400D1E5 RID: 53733
				public static LocString DESC = "A {0}'s monotonous voice and lack of facial expression makes it impossible for others to tell when he's messing with them.";
			}

			// Token: 0x02003322 RID: 13090
			public class HAROLD
			{
				// Token: 0x0400D1E6 RID: 53734
				public static LocString NAME = "Harold";

				// Token: 0x0400D1E7 RID: 53735
				public static LocString DESC = "Get a bunch of {0}s together in a room, and you'll have... a bunch of {0}s together in a room.";
			}

			// Token: 0x02003323 RID: 13091
			public class MAX
			{
				// Token: 0x0400D1E8 RID: 53736
				public static LocString NAME = "Max";

				// Token: 0x0400D1E9 RID: 53737
				public static LocString DESC = "At any given moment a {0} is viscerally reliving ten different humiliating memories.";
			}

			// Token: 0x02003324 RID: 13092
			public class ROWAN
			{
				// Token: 0x0400D1EA RID: 53738
				public static LocString NAME = "Rowan";

				// Token: 0x0400D1EB RID: 53739
				public static LocString DESC = "{0}s have exceptionally large hearts and express their emotions most efficiently by yelling.";
			}

			// Token: 0x02003325 RID: 13093
			public class OTTO
			{
				// Token: 0x0400D1EC RID: 53740
				public static LocString NAME = "Otto";

				// Token: 0x0400D1ED RID: 53741
				public static LocString DESC = "{0}s always insult people by accident and generally exist in a perpetual state of deep regret.";
			}

			// Token: 0x02003326 RID: 13094
			public class TURNER
			{
				// Token: 0x0400D1EE RID: 53742
				public static LocString NAME = "Turner";

				// Token: 0x0400D1EF RID: 53743
				public static LocString DESC = "This {0} is paralyzed by the knowledge that others have memories and perceptions of them they can't control.";
			}

			// Token: 0x02003327 RID: 13095
			public class NIKOLA
			{
				// Token: 0x0400D1F0 RID: 53744
				public static LocString NAME = "Nikola";

				// Token: 0x0400D1F1 RID: 53745
				public static LocString DESC = "This {0} once claimed he could build a laser so powerful it would rip the colony in half. No one asked him to prove it.";
			}

			// Token: 0x02003328 RID: 13096
			public class MEEP
			{
				// Token: 0x0400D1F2 RID: 53746
				public static LocString NAME = "Meep";

				// Token: 0x0400D1F3 RID: 53747
				public static LocString DESC = "{0}s have a face only a two tonne Printing Pod could love.";
			}

			// Token: 0x02003329 RID: 13097
			public class ARI
			{
				// Token: 0x0400D1F4 RID: 53748
				public static LocString NAME = "Ari";

				// Token: 0x0400D1F5 RID: 53749
				public static LocString DESC = "{0}s tend to space out from time to time, but they always pay attention when it counts.";
			}

			// Token: 0x0200332A RID: 13098
			public class JEAN
			{
				// Token: 0x0400D1F6 RID: 53750
				public static LocString NAME = "Jean";

				// Token: 0x0400D1F7 RID: 53751
				public static LocString DESC = "Just because {0}s are a little slow doesn't mean they can't suffer from soul-crushing existential crises.";
			}

			// Token: 0x0200332B RID: 13099
			public class CAMILLE
			{
				// Token: 0x0400D1F8 RID: 53752
				public static LocString NAME = "Camille";

				// Token: 0x0400D1F9 RID: 53753
				public static LocString DESC = "This {0} loves anything that makes her feel nostalgic, including things that haven't aged well.";
			}

			// Token: 0x0200332C RID: 13100
			public class ASHKAN
			{
				// Token: 0x0400D1FA RID: 53754
				public static LocString NAME = "Ashkan";

				// Token: 0x0400D1FB RID: 53755
				public static LocString DESC = "{0}s have what can only be described as a \"seriously infectious giggle\".";
			}

			// Token: 0x0200332D RID: 13101
			public class STEVE
			{
				// Token: 0x0400D1FC RID: 53756
				public static LocString NAME = "Steve";

				// Token: 0x0400D1FD RID: 53757
				public static LocString DESC = "This {0} is convinced that he has psychic powers. And he knows exactly what his friends think about that.";
			}

			// Token: 0x0200332E RID: 13102
			public class AMARI
			{
				// Token: 0x0400D1FE RID: 53758
				public static LocString NAME = "Amari";

				// Token: 0x0400D1FF RID: 53759
				public static LocString DESC = "{0}s likes to keep the peace. Ironically, they're a riot at parties.";
			}

			// Token: 0x0200332F RID: 13103
			public class PEI
			{
				// Token: 0x0400D200 RID: 53760
				public static LocString NAME = "Pei";

				// Token: 0x0400D201 RID: 53761
				public static LocString DESC = "Every {0} spends at least half the day pretending that they remember what they came into this room for.";
			}

			// Token: 0x02003330 RID: 13104
			public class QUINN
			{
				// Token: 0x0400D202 RID: 53762
				public static LocString NAME = "Quinn";

				// Token: 0x0400D203 RID: 53763
				public static LocString DESC = "This {0}'s favorite genre of music is \"festive power ballad\".";
			}

			// Token: 0x02003331 RID: 13105
			public class JORGE
			{
				// Token: 0x0400D204 RID: 53764
				public static LocString NAME = "Jorge";

				// Token: 0x0400D205 RID: 53765
				public static LocString DESC = "{0} loves his new colony, even if their collective body odor makes his eyes water.";
			}

			// Token: 0x02003332 RID: 13106
			public class CALVIN
			{
				// Token: 0x0400D206 RID: 53766
				public static LocString NAME = "Calvin";

				// Token: 0x0400D207 RID: 53767
				public static LocString DESC = "This {0} loves the thrill of running head-first into a wall to see what it's made of.";
			}

			// Token: 0x02003333 RID: 13107
			public class FREYJA
			{
				// Token: 0x0400D208 RID: 53768
				public static LocString NAME = "Freyja";

				// Token: 0x0400D209 RID: 53769
				public static LocString DESC = "This {0} has never stopped anyone from eating yellow snow.";
			}

			// Token: 0x02003334 RID: 13108
			public class CHIP
			{
				// Token: 0x0400D20A RID: 53770
				public static LocString NAME = "Chip";

				// Token: 0x0400D20B RID: 53771
				public static LocString DESC = "This {0} is extremely good at guessing their friends' passwords.";
			}

			// Token: 0x02003335 RID: 13109
			public class EDWIREDO
			{
				// Token: 0x0400D20C RID: 53772
				public static LocString NAME = "Edwiredo";

				// Token: 0x0400D20D RID: 53773
				public static LocString DESC = "This {0} once rolled his eye so hard that he powered himself off and on again.";
			}

			// Token: 0x02003336 RID: 13110
			public class GIZMO
			{
				// Token: 0x0400D20E RID: 53774
				public static LocString NAME = "Gizmo";

				// Token: 0x0400D20F RID: 53775
				public static LocString DESC = "{0}s love nothing more than a big juicy info dump.";
			}

			// Token: 0x02003337 RID: 13111
			public class STEELA
			{
				// Token: 0x0400D210 RID: 53776
				public static LocString NAME = "Steela";

				// Token: 0x0400D211 RID: 53777
				public static LocString DESC = "{0}s aren't programmed to put up with nonsense, but they do enjoy the occasional shenanigan.";
			}

			// Token: 0x02003338 RID: 13112
			public class SONYAR
			{
				// Token: 0x0400D212 RID: 53778
				public static LocString NAME = "Sonyar";

				// Token: 0x0400D213 RID: 53779
				public static LocString DESC = "{0}s would sooner burn down the colony than read an instruction manual.";
			}

			// Token: 0x02003339 RID: 13113
			public class ULTI
			{
				// Token: 0x0400D214 RID: 53780
				public static LocString NAME = "Ulti";

				// Token: 0x0400D215 RID: 53781
				public static LocString DESC = "The only dance move this {0} knows is The Robot.";
			}
		}

		// Token: 0x020021B9 RID: 8633
		public class NEEDS
		{
			// Token: 0x0200333A RID: 13114
			public class DECOR
			{
				// Token: 0x0400D216 RID: 53782
				public static LocString NAME = "Decor Expectation";

				// Token: 0x0400D217 RID: 53783
				public static LocString PROFESSION_NAME = "Critic";

				// Token: 0x0400D218 RID: 53784
				public static LocString OBSERVED_DECOR = "Current Surroundings";

				// Token: 0x0400D219 RID: 53785
				public static LocString EXPECTATION_TOOLTIP = string.Concat(new string[]
				{
					"Most objects have ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values that alter Duplicants' opinions of their surroundings.\nThis Duplicant desires ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values of <b>{0}</b> or higher, and becomes ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" in areas with lower ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D21A RID: 53786
				public static LocString EXPECTATION_MOD_NAME = "Job Tier Request";
			}

			// Token: 0x0200333B RID: 13115
			public class FOOD_QUALITY
			{
				// Token: 0x0400D21B RID: 53787
				public static LocString NAME = "Food Quality";

				// Token: 0x0400D21C RID: 53788
				public static LocString PROFESSION_NAME = "Gourmet";

				// Token: 0x0400D21D RID: 53789
				public static LocString EXPECTATION_TOOLTIP = string.Concat(new string[]
				{
					"Each Duplicant has a minimum quality of ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" they'll tolerate eating.\nThis Duplicant desires <b>Tier {0}<b> or better ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					", and becomes ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" when they eat meals of lower quality."
				});

				// Token: 0x0400D21E RID: 53790
				public static LocString BAD_FOOD_MOD = "Food Quality";

				// Token: 0x0400D21F RID: 53791
				public static LocString NORMAL_FOOD_MOD = "Food Quality";

				// Token: 0x0400D220 RID: 53792
				public static LocString GOOD_FOOD_MOD = "Food Quality";

				// Token: 0x0400D221 RID: 53793
				public static LocString EXPECTATION_MOD_NAME = "Job Tier Request";

				// Token: 0x0400D222 RID: 53794
				public static LocString ADJECTIVE_FORMAT_POSITIVE = "{0} [{1}]";

				// Token: 0x0400D223 RID: 53795
				public static LocString ADJECTIVE_FORMAT_NEGATIVE = "{0} [{1}]";

				// Token: 0x0400D224 RID: 53796
				public static LocString FOODQUALITY = "\nFood Quality Score of {0}";

				// Token: 0x0400D225 RID: 53797
				public static LocString FOODQUALITY_EXPECTATION = string.Concat(new string[]
				{
					"\nThis Duplicant is content to eat ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" with a ",
					UI.PRE_KEYWORD,
					"Food Quality",
					UI.PST_KEYWORD,
					" of <b>{0}</b> or higher"
				});

				// Token: 0x0400D226 RID: 53798
				public static int ADJECTIVE_INDEX_OFFSET = -1;

				// Token: 0x02003837 RID: 14391
				public class ADJECTIVES
				{
					// Token: 0x0400DE78 RID: 56952
					public static LocString MINUS_1 = "Grisly";

					// Token: 0x0400DE79 RID: 56953
					public static LocString ZERO = "Terrible";

					// Token: 0x0400DE7A RID: 56954
					public static LocString PLUS_1 = "Poor";

					// Token: 0x0400DE7B RID: 56955
					public static LocString PLUS_2 = "Standard";

					// Token: 0x0400DE7C RID: 56956
					public static LocString PLUS_3 = "Good";

					// Token: 0x0400DE7D RID: 56957
					public static LocString PLUS_4 = "Great";

					// Token: 0x0400DE7E RID: 56958
					public static LocString PLUS_5 = "Superb";

					// Token: 0x0400DE7F RID: 56959
					public static LocString PLUS_6 = "Ambrosial";
				}
			}

			// Token: 0x0200333C RID: 13116
			public class QUALITYOFLIFE
			{
				// Token: 0x0400D227 RID: 53799
				public static LocString NAME = "Morale Requirements";

				// Token: 0x0400D228 RID: 53800
				public static LocString EXPECTATION_TOOLTIP = string.Concat(new string[]
				{
					"The more responsibilities and stressors a Duplicant has, the more they will desire additional leisure time and improved amenities.\n\nFailing to keep a Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" at or above their ",
					UI.PRE_KEYWORD,
					"Morale Need",
					UI.PST_KEYWORD,
					" means they will not be able to unwind, causing them ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" over time."
				});

				// Token: 0x0400D229 RID: 53801
				public static LocString EXPECTATION_MOD_NAME = "Skills Learned";

				// Token: 0x0400D22A RID: 53802
				public static LocString APTITUDE_SKILLS_MOD_NAME = "Interested Skills Learned";

				// Token: 0x0400D22B RID: 53803
				public static LocString TOTAL_SKILL_POINTS = "Total Skill Points: {0}";

				// Token: 0x0400D22C RID: 53804
				public static LocString GOOD_MODIFIER = "High Morale";

				// Token: 0x0400D22D RID: 53805
				public static LocString NEUTRAL_MODIFIER = "Sufficient Morale";

				// Token: 0x0400D22E RID: 53806
				public static LocString BAD_MODIFIER = "Low Morale";
			}

			// Token: 0x0200333D RID: 13117
			public class NOISE
			{
				// Token: 0x0400D22F RID: 53807
				public static LocString NAME = "Noise Expectation";
			}
		}

		// Token: 0x020021BA RID: 8634
		public class ATTRIBUTES
		{
			// Token: 0x040099B2 RID: 39346
			public static LocString VALUE = "{0}: {1}";

			// Token: 0x040099B3 RID: 39347
			public static LocString TOTAL_VALUE = "\n\nTotal <b>{1}</b>: {0}";

			// Token: 0x040099B4 RID: 39348
			public static LocString BASE_VALUE = "\nBase: {0}";

			// Token: 0x040099B5 RID: 39349
			public static LocString MODIFIER_ENTRY = "\n    • {0}: {1}";

			// Token: 0x040099B6 RID: 39350
			public static LocString UNPROFESSIONAL_NAME = "Lump";

			// Token: 0x040099B7 RID: 39351
			public static LocString UNPROFESSIONAL_DESC = "This Duplicant has no discernible skills";

			// Token: 0x040099B8 RID: 39352
			public static LocString PROFESSION_DESC = string.Concat(new string[]
			{
				"Expertise is determined by a Duplicant's highest ",
				UI.PRE_KEYWORD,
				"Attribute",
				UI.PST_KEYWORD,
				"\n\nDuplicants develop higher expectations as their Expertise level increases"
			});

			// Token: 0x040099B9 RID: 39353
			public static LocString STORED_VALUE = "Stored value";

			// Token: 0x0200333E RID: 13118
			public class CONSTRUCTION
			{
				// Token: 0x0400D230 RID: 53808
				public static LocString NAME = "Construction";

				// Token: 0x0400D231 RID: 53809
				public static LocString DESC = "Determines a Duplicant's building Speed.";

				// Token: 0x0400D232 RID: 53810
				public static LocString SPEEDMODIFIER = "{0} Construction Speed";
			}

			// Token: 0x0200333F RID: 13119
			public class SCALDINGTHRESHOLD
			{
				// Token: 0x0400D233 RID: 53811
				public static LocString NAME = "Scalding Threshold";

				// Token: 0x0400D234 RID: 53812
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" at which a Duplicant will get burned."
				});
			}

			// Token: 0x02003340 RID: 13120
			public class SCOLDINGTHRESHOLD
			{
				// Token: 0x0400D235 RID: 53813
				public static LocString NAME = "Frostbite Threshold";

				// Token: 0x0400D236 RID: 53814
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" at which a Duplicant will get frostbitten."
				});
			}

			// Token: 0x02003341 RID: 13121
			public class DIGGING
			{
				// Token: 0x0400D237 RID: 53815
				public static LocString NAME = "Excavation";

				// Token: 0x0400D238 RID: 53816
				public static LocString DESC = "Determines a Duplicant's mining speed.";

				// Token: 0x0400D239 RID: 53817
				public static LocString SPEEDMODIFIER = "{0} Digging Speed";

				// Token: 0x0400D23A RID: 53818
				public static LocString ATTACK_MODIFIER = "{0} Attack Damage";
			}

			// Token: 0x02003342 RID: 13122
			public class MACHINERY
			{
				// Token: 0x0400D23B RID: 53819
				public static LocString NAME = "Machinery";

				// Token: 0x0400D23C RID: 53820
				public static LocString DESC = "Determines how quickly a Duplicant uses machines.";

				// Token: 0x0400D23D RID: 53821
				public static LocString SPEEDMODIFIER = "{0} Machine Operation Speed";

				// Token: 0x0400D23E RID: 53822
				public static LocString TINKER_EFFECT_MODIFIER = "{0} Engie's Tune-Up Effect Duration";
			}

			// Token: 0x02003343 RID: 13123
			public class LIFESUPPORT
			{
				// Token: 0x0400D23F RID: 53823
				public static LocString NAME = "Life Support";

				// Token: 0x0400D240 RID: 53824
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how efficiently a Duplicant maintains ",
					BUILDINGS.PREFABS.ALGAEHABITAT.NAME,
					"s, ",
					BUILDINGS.PREFABS.AIRFILTER.NAME,
					"s, and ",
					BUILDINGS.PREFABS.WATERPURIFIER.NAME,
					"s"
				});
			}

			// Token: 0x02003344 RID: 13124
			public class TOGGLE
			{
				// Token: 0x0400D241 RID: 53825
				public static LocString NAME = "Toggle";

				// Token: 0x0400D242 RID: 53826
				public static LocString DESC = "Determines how efficiently a Duplicant tunes machinery, flips switches, and sets sensors.";
			}

			// Token: 0x02003345 RID: 13125
			public class ATHLETICS
			{
				// Token: 0x0400D243 RID: 53827
				public static LocString NAME = "Athletics";

				// Token: 0x0400D244 RID: 53828
				public static LocString DESC = "Determines a Duplicant's default runspeed.";

				// Token: 0x0400D245 RID: 53829
				public static LocString SPEEDMODIFIER = "{0} Runspeed";
			}

			// Token: 0x02003346 RID: 13126
			public class LUMINESCENCE
			{
				// Token: 0x0400D246 RID: 53830
				public static LocString NAME = "Luminescence";

				// Token: 0x0400D247 RID: 53831
				public static LocString DESC = "Determines how much light a Duplicant emits.";
			}

			// Token: 0x02003347 RID: 13127
			public class TRANSITTUBETRAVELSPEED
			{
				// Token: 0x0400D248 RID: 53832
				public static LocString NAME = "Transit Speed";

				// Token: 0x0400D249 RID: 53833
				public static LocString DESC = "Determines a Duplicant's default " + BUILDINGS.PREFABS.TRAVELTUBE.NAME + " travel speed.";

				// Token: 0x0400D24A RID: 53834
				public static LocString SPEEDMODIFIER = "{0} Transit Tube Travel Speed";
			}

			// Token: 0x02003348 RID: 13128
			public class DOCTOREDLEVEL
			{
				// Token: 0x0400D24B RID: 53835
				public static LocString NAME = UI.FormatAsLink("Treatment Received", "MEDICINE") + " Effect";

				// Token: 0x0400D24C RID: 53836
				public static LocString DESC = string.Concat(new string[]
				{
					"Duplicants who receive medical care while in a ",
					BUILDINGS.PREFABS.DOCTORSTATION.NAME,
					" or ",
					BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME,
					" will gain the ",
					UI.PRE_KEYWORD,
					"Treatment Received",
					UI.PST_KEYWORD,
					" effect\n\nThis effect reduces the severity of ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" symptoms"
				});
			}

			// Token: 0x02003349 RID: 13129
			public class SNEEZYNESS
			{
				// Token: 0x0400D24D RID: 53837
				public static LocString NAME = "Sneeziness";

				// Token: 0x0400D24E RID: 53838
				public static LocString DESC = "Determines how frequently a Duplicant sneezes.";
			}

			// Token: 0x0200334A RID: 13130
			public class GERMRESISTANCE
			{
				// Token: 0x0400D24F RID: 53839
				public static LocString NAME = "Germ Resistance";

				// Token: 0x0400D250 RID: 53840
				public static LocString DESC = string.Concat(new string[]
				{
					"Duplicants with a higher ",
					UI.PRE_KEYWORD,
					"Germ Resistance",
					UI.PST_KEYWORD,
					" rating are less likely to contract germ-based ",
					UI.PRE_KEYWORD,
					"Diseases",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x02003838 RID: 14392
				public class MODIFIER_DESCRIPTORS
				{
					// Token: 0x0400DE80 RID: 56960
					public static LocString NEGATIVE_LARGE = "{0} (Large Loss)";

					// Token: 0x0400DE81 RID: 56961
					public static LocString NEGATIVE_MEDIUM = "{0} (Medium Loss)";

					// Token: 0x0400DE82 RID: 56962
					public static LocString NEGATIVE_SMALL = "{0} (Small Loss)";

					// Token: 0x0400DE83 RID: 56963
					public static LocString NONE = "No Effect";

					// Token: 0x0400DE84 RID: 56964
					public static LocString POSITIVE_SMALL = "{0} (Small Boost)";

					// Token: 0x0400DE85 RID: 56965
					public static LocString POSITIVE_MEDIUM = "{0} (Medium Boost)";

					// Token: 0x0400DE86 RID: 56966
					public static LocString POSITIVE_LARGE = "{0} (Large Boost)";
				}
			}

			// Token: 0x0200334B RID: 13131
			public class LEARNING
			{
				// Token: 0x0400D251 RID: 53841
				public static LocString NAME = "Science";

				// Token: 0x0400D252 RID: 53842
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant conducts ",
					UI.PRE_KEYWORD,
					"Research",
					UI.PST_KEYWORD,
					" and gains ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D253 RID: 53843
				public static LocString SPEEDMODIFIER = "{0} Skill Leveling";

				// Token: 0x0400D254 RID: 53844
				public static LocString RESEARCHSPEED = "{0} Research Speed";

				// Token: 0x0400D255 RID: 53845
				public static LocString GEOTUNER_SPEED_MODIFIER = "{0} Geotuning Speed";
			}

			// Token: 0x0200334C RID: 13132
			public class COOKING
			{
				// Token: 0x0400D256 RID: 53846
				public static LocString NAME = "Cuisine";

				// Token: 0x0400D257 RID: 53847
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant prepares ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D258 RID: 53848
				public static LocString SPEEDMODIFIER = "{0} Cooking Speed";
			}

			// Token: 0x0200334D RID: 13133
			public class HAPPINESSDELTA
			{
				// Token: 0x0400D259 RID: 53849
				public static LocString NAME = "Happiness";

				// Token: 0x0400D25A RID: 53850
				public static LocString DESC = "Contented " + UI.FormatAsLink("Critters", "CREATURES") + " produce usable materials with increased frequency.";
			}

			// Token: 0x0200334E RID: 13134
			public class RADIATIONBALANCEDELTA
			{
				// Token: 0x0400D25B RID: 53851
				public static LocString NAME = "Absorbed Radiation Dose";

				// Token: 0x0400D25C RID: 53852
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants accumulate Rads in areas with ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" and recover at very slow rates\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay", global::Action.Overlay15),
					" to view current ",
					UI.PRE_KEYWORD,
					"Rad",
					UI.PST_KEYWORD,
					" readings"
				});
			}

			// Token: 0x0200334F RID: 13135
			public class INSULATION
			{
				// Token: 0x0400D25D RID: 53853
				public static LocString NAME = "Insulation";

				// Token: 0x0400D25E RID: 53854
				public static LocString DESC = string.Concat(new string[]
				{
					"Highly ",
					UI.PRE_KEYWORD,
					"Insulated",
					UI.PST_KEYWORD,
					" Duplicants retain body heat easily, while low ",
					UI.PRE_KEYWORD,
					"Insulation",
					UI.PST_KEYWORD,
					" Duplicants are easier to keep cool."
				});

				// Token: 0x0400D25F RID: 53855
				public static LocString SPEEDMODIFIER = "{0} Temperature Retention";
			}

			// Token: 0x02003350 RID: 13136
			public class STRENGTH
			{
				// Token: 0x0400D260 RID: 53856
				public static LocString NAME = "Strength";

				// Token: 0x0400D261 RID: 53857
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines a Duplicant's ",
					UI.PRE_KEYWORD,
					"Carrying Capacity",
					UI.PST_KEYWORD,
					" and cleaning speed."
				});

				// Token: 0x0400D262 RID: 53858
				public static LocString CARRYMODIFIER = "{0} " + DUPLICANTS.ATTRIBUTES.CARRYAMOUNT.NAME;

				// Token: 0x0400D263 RID: 53859
				public static LocString SPEEDMODIFIER = "{0} Tidying Speed";
			}

			// Token: 0x02003351 RID: 13137
			public class CARING
			{
				// Token: 0x0400D264 RID: 53860
				public static LocString NAME = "Medicine";

				// Token: 0x0400D265 RID: 53861
				public static LocString DESC = "Determines a Duplicant's ability to care for sick peers.";

				// Token: 0x0400D266 RID: 53862
				public static LocString SPEEDMODIFIER = "{0} Treatment Speed";

				// Token: 0x0400D267 RID: 53863
				public static LocString FABRICATE_SPEEDMODIFIER = "{0} Medicine Fabrication Speed";
			}

			// Token: 0x02003352 RID: 13138
			public class IMMUNITY
			{
				// Token: 0x0400D268 RID: 53864
				public static LocString NAME = "Immunity";

				// Token: 0x0400D269 RID: 53865
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines a Duplicant's ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" susceptibility and recovery time."
				});

				// Token: 0x0400D26A RID: 53866
				public static LocString BOOST_MODIFIER = "{0} Immunity Regen";

				// Token: 0x0400D26B RID: 53867
				public static LocString BOOST_STAT = "Immunity Attribute";
			}

			// Token: 0x02003353 RID: 13139
			public class BOTANIST
			{
				// Token: 0x0400D26C RID: 53868
				public static LocString NAME = "Agriculture";

				// Token: 0x0400D26D RID: 53869
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly and efficiently a Duplicant cultivates ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D26E RID: 53870
				public static LocString HARVEST_SPEED_MODIFIER = "{0} Harvesting Speed";

				// Token: 0x0400D26F RID: 53871
				public static LocString TINKER_MODIFIER = "{0} Tending Speed";

				// Token: 0x0400D270 RID: 53872
				public static LocString BONUS_SEEDS = "{0} Seed Chance";

				// Token: 0x0400D271 RID: 53873
				public static LocString TINKER_EFFECT_MODIFIER = "{0} Farmer's Touch Effect Duration";
			}

			// Token: 0x02003354 RID: 13140
			public class RANCHING
			{
				// Token: 0x0400D272 RID: 53874
				public static LocString NAME = "Husbandry";

				// Token: 0x0400D273 RID: 53875
				public static LocString DESC = "Determines how efficiently a Duplicant tends " + UI.FormatAsLink("Critters", "CREATURES") + ".";

				// Token: 0x0400D274 RID: 53876
				public static LocString EFFECTMODIFIER = "{0} Groom Effect Duration";

				// Token: 0x0400D275 RID: 53877
				public static LocString CAPTURABLESPEED = "{0} Wrangling Speed";
			}

			// Token: 0x02003355 RID: 13141
			public class ART
			{
				// Token: 0x0400D276 RID: 53878
				public static LocString NAME = "Creativity";

				// Token: 0x0400D277 RID: 53879
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant produces ",
					UI.PRE_KEYWORD,
					"Artwork",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D278 RID: 53880
				public static LocString SPEEDMODIFIER = "{0} Decorating Speed";
			}

			// Token: 0x02003356 RID: 13142
			public class DECOR
			{
				// Token: 0x0400D279 RID: 53881
				public static LocString NAME = "Decor";

				// Token: 0x0400D27A RID: 53882
				public static LocString DESC = string.Concat(new string[]
				{
					"Affects a Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" and their opinion of their surroundings."
				});
			}

			// Token: 0x02003357 RID: 13143
			public class THERMALCONDUCTIVITYBARRIER
			{
				// Token: 0x0400D27B RID: 53883
				public static LocString NAME = "Insulation Thickness";

				// Token: 0x0400D27C RID: 53884
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant retains or loses body ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" in any given area.\n\nIt is the sum of a Duplicant's ",
					UI.PRE_KEYWORD,
					"Equipment",
					UI.PST_KEYWORD,
					" and their natural ",
					UI.PRE_KEYWORD,
					"Insulation",
					UI.PST_KEYWORD,
					" values."
				});
			}

			// Token: 0x02003358 RID: 13144
			public class DECORRADIUS
			{
				// Token: 0x0400D27D RID: 53885
				public static LocString NAME = "Decor Radius";

				// Token: 0x0400D27E RID: 53886
				public static LocString DESC = string.Concat(new string[]
				{
					"The influence range of an object's ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" value."
				});
			}

			// Token: 0x02003359 RID: 13145
			public class DECOREXPECTATION
			{
				// Token: 0x0400D27F RID: 53887
				public static LocString NAME = "Decor Morale Bonus";

				// Token: 0x0400D280 RID: 53888
				public static LocString DESC = string.Concat(new string[]
				{
					"A Decor Morale Bonus allows Duplicants to receive ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" boosts from lower ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values.\n\nMaintaining high ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" will allow Duplicants to learn more ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x0200335A RID: 13146
			public class FOODEXPECTATION
			{
				// Token: 0x0400D281 RID: 53889
				public static LocString NAME = "Food Morale Bonus";

				// Token: 0x0400D282 RID: 53890
				public static LocString DESC = string.Concat(new string[]
				{
					"A Food Morale Bonus allows Duplicants to receive ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" boosts from lower quality ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					".\n\nMaintaining high ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" will allow Duplicants to learn more ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x0200335B RID: 13147
			public class QUALITYOFLIFEEXPECTATION
			{
				// Token: 0x0400D283 RID: 53891
				public static LocString NAME = "Morale Need";

				// Token: 0x0400D284 RID: 53892
				public static LocString DESC = string.Concat(new string[]
				{
					"Dictates how high a Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" must be kept to prevent them from gaining ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200335C RID: 13148
			public class HYGIENE
			{
				// Token: 0x0400D285 RID: 53893
				public static LocString NAME = "Hygiene";

				// Token: 0x0400D286 RID: 53894
				public static LocString DESC = "Affects a Duplicant's sense of cleanliness.";
			}

			// Token: 0x0200335D RID: 13149
			public class CARRYAMOUNT
			{
				// Token: 0x0400D287 RID: 53895
				public static LocString NAME = "Carrying Capacity";

				// Token: 0x0400D288 RID: 53896
				public static LocString DESC = "Determines the maximum weight that a Duplicant can carry.";
			}

			// Token: 0x0200335E RID: 13150
			public class SPACENAVIGATION
			{
				// Token: 0x0400D289 RID: 53897
				public static LocString NAME = "Piloting";

				// Token: 0x0400D28A RID: 53898
				public static LocString DESC = "Determines how long it takes a Duplicant to complete a space mission.";

				// Token: 0x0400D28B RID: 53899
				public static LocString DLC1_DESC = "Determines how much of a speed bonus a Duplicant provides to a rocket they are piloting.";

				// Token: 0x0400D28C RID: 53900
				public static LocString SPEED_MODIFIER = "{0} Rocket Speed";
			}

			// Token: 0x0200335F RID: 13151
			public class QUALITYOFLIFE
			{
				// Token: 0x0400D28D RID: 53901
				public static LocString NAME = "Morale";

				// Token: 0x0400D28E RID: 53902
				public static LocString DESC = string.Concat(new string[]
				{
					"A Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" must exceed their ",
					UI.PRE_KEYWORD,
					"Morale Need",
					UI.PST_KEYWORD,
					", or they'll begin to accumulate ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					".\n\n",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" can be increased by providing Duplicants higher quality ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					", allotting more ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" in\nthe colony schedule, or building better ",
					UI.PRE_KEYWORD,
					"Bathrooms",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Bedrooms",
					UI.PST_KEYWORD,
					" for them to live in."
				});

				// Token: 0x0400D28F RID: 53903
				public static LocString DESC_FORMAT = "{0} / {1}";

				// Token: 0x0400D290 RID: 53904
				public static LocString TOOLTIP_EXPECTATION = "Total <b>Morale Need</b>: {0}\n    • Skills Learned: +{0}";

				// Token: 0x0400D291 RID: 53905
				public static LocString TOOLTIP_EXPECTATION_OVER = "This Duplicant has sufficiently high " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;

				// Token: 0x0400D292 RID: 53906
				public static LocString TOOLTIP_EXPECTATION_UNDER = string.Concat(new string[]
				{
					"This Duplicant's low ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" will cause ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" over time"
				});
			}

			// Token: 0x02003360 RID: 13152
			public class AIRCONSUMPTIONRATE
			{
				// Token: 0x0400D293 RID: 53907
				public static LocString NAME = "Air Consumption Rate";

				// Token: 0x0400D294 RID: 53908
				public static LocString DESC = "Air Consumption determines how much " + ELEMENTS.OXYGEN.NAME + " a Duplicant requires per minute to live.";
			}

			// Token: 0x02003361 RID: 13153
			public class RADIATIONRESISTANCE
			{
				// Token: 0x0400D295 RID: 53909
				public static LocString NAME = "Radiation Resistance";

				// Token: 0x0400D296 RID: 53910
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how easily a Duplicant repels ",
					UI.PRE_KEYWORD,
					"Radiation Sickness",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02003362 RID: 13154
			public class RADIATIONRECOVERY
			{
				// Token: 0x0400D297 RID: 53911
				public static LocString NAME = "Radiation Absorption";

				// Token: 0x0400D298 RID: 53912
				public static LocString DESC = string.Concat(new string[]
				{
					"The rate at which ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" is neutralized within a Duplicant body."
				});
			}

			// Token: 0x02003363 RID: 13155
			public class STRESSDELTA
			{
				// Token: 0x0400D299 RID: 53913
				public static LocString NAME = "Stress";

				// Token: 0x0400D29A RID: 53914
				public static LocString DESC = "Determines how quickly a Duplicant gains or reduces " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02003364 RID: 13156
			public class BREATHDELTA
			{
				// Token: 0x0400D29B RID: 53915
				public static LocString NAME = "Breath";

				// Token: 0x0400D29C RID: 53916
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant gains or reduces ",
					UI.PRE_KEYWORD,
					"Breath",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02003365 RID: 13157
			public class BIONICOILDELTA
			{
				// Token: 0x0400D29D RID: 53917
				public static LocString NAME = "Oil";

				// Token: 0x0400D29E RID: 53918
				public static LocString DESC = "Determines how quickly a Duplicant's bionic parts gains or reduces " + UI.PRE_KEYWORD + "Oil" + UI.PST_KEYWORD;
			}

			// Token: 0x02003366 RID: 13158
			public class BLADDERDELTA
			{
				// Token: 0x0400D29F RID: 53919
				public static LocString NAME = "Bladder";

				// Token: 0x0400D2A0 RID: 53920
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant's ",
					UI.PRE_KEYWORD,
					"Bladder",
					UI.PST_KEYWORD,
					" fills or depletes."
				});
			}

			// Token: 0x02003367 RID: 13159
			public class CALORIESDELTA
			{
				// Token: 0x0400D2A1 RID: 53921
				public static LocString NAME = "Calories";

				// Token: 0x0400D2A2 RID: 53922
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant burns or stores ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02003368 RID: 13160
			public class STAMINADELTA
			{
				// Token: 0x0400D2A3 RID: 53923
				public static LocString NAME = "Stamina";

				// Token: 0x0400D2A4 RID: 53924
				public static LocString DESC = "";
			}

			// Token: 0x02003369 RID: 13161
			public class TOXICITYDELTA
			{
				// Token: 0x0400D2A5 RID: 53925
				public static LocString NAME = "Toxicity";

				// Token: 0x0400D2A6 RID: 53926
				public static LocString DESC = "";
			}

			// Token: 0x0200336A RID: 13162
			public class IMMUNELEVELDELTA
			{
				// Token: 0x0400D2A7 RID: 53927
				public static LocString NAME = "Immunity";

				// Token: 0x0400D2A8 RID: 53928
				public static LocString DESC = "";
			}

			// Token: 0x0200336B RID: 13163
			public class TOILETEFFICIENCY
			{
				// Token: 0x0400D2A9 RID: 53929
				public static LocString NAME = "Bathroom Use Speed";

				// Token: 0x0400D2AA RID: 53930
				public static LocString DESC = "Determines how long a Duplicant needs to do their \"business\".";

				// Token: 0x0400D2AB RID: 53931
				public static LocString SPEEDMODIFIER = "{0} Bathroom Use Speed";
			}

			// Token: 0x0200336C RID: 13164
			public class METABOLISM
			{
				// Token: 0x0400D2AC RID: 53932
				public static LocString NAME = "Critter Metabolism";

				// Token: 0x0400D2AD RID: 53933
				public static LocString DESC = string.Concat(new string[]
				{
					"Affects the rate at which a critter burns ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD,
					" and produces materials"
				});
			}

			// Token: 0x0200336D RID: 13165
			public class ROOMTEMPERATUREPREFERENCE
			{
				// Token: 0x0400D2AE RID: 53934
				public static LocString NAME = "Temperature Preference";

				// Token: 0x0400D2AF RID: 53935
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines the minimum body ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" a Duplicant prefers to maintain."
				});
			}

			// Token: 0x0200336E RID: 13166
			public class MAXUNDERWATERTRAVELCOST
			{
				// Token: 0x0400D2B0 RID: 53936
				public static LocString NAME = "Underwater Movement";

				// Token: 0x0400D2B1 RID: 53937
				public static LocString DESC = "Determines a Duplicant's runspeed when submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;
			}

			// Token: 0x0200336F RID: 13167
			public class OVERHEATTEMPERATURE
			{
				// Token: 0x0400D2B2 RID: 53938
				public static LocString NAME = "Overheat Temperature";

				// Token: 0x0400D2B3 RID: 53939
				public static LocString DESC = string.Concat(new string[]
				{
					"A building at Overheat ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" will take damage and break down if not cooled"
				});
			}

			// Token: 0x02003370 RID: 13168
			public class FATALTEMPERATURE
			{
				// Token: 0x0400D2B4 RID: 53940
				public static LocString NAME = "Break Down Temperature";

				// Token: 0x0400D2B5 RID: 53941
				public static LocString DESC = string.Concat(new string[]
				{
					"A building at break down ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" will lose functionality and take damage"
				});
			}

			// Token: 0x02003371 RID: 13169
			public class HITPOINTSDELTA
			{
				// Token: 0x0400D2B6 RID: 53942
				public static LocString NAME = UI.FormatAsLink("Health", "HEALTH");

				// Token: 0x0400D2B7 RID: 53943
				public static LocString DESC = "Health regeneration is increased when another Duplicant provides medical care to the patient";
			}

			// Token: 0x02003372 RID: 13170
			public class DISEASECURESPEED
			{
				// Token: 0x0400D2B8 RID: 53944
				public static LocString NAME = UI.FormatAsLink("Disease", "DISEASE") + " Recovery Speed Bonus";

				// Token: 0x0400D2B9 RID: 53945
				public static LocString DESC = "Recovery speed bonus is increased when another Duplicant provides medical care to the patient";
			}

			// Token: 0x02003373 RID: 13171
			public abstract class MACHINERYSPEED
			{
				// Token: 0x0400D2BA RID: 53946
				public static LocString NAME = "Machinery Speed";

				// Token: 0x0400D2BB RID: 53947
				public static LocString DESC = "Speed Bonus";
			}

			// Token: 0x02003374 RID: 13172
			public abstract class GENERATOROUTPUT
			{
				// Token: 0x0400D2BC RID: 53948
				public static LocString NAME = "Power Output";
			}

			// Token: 0x02003375 RID: 13173
			public abstract class ROCKETBURDEN
			{
				// Token: 0x0400D2BD RID: 53949
				public static LocString NAME = "Burden";
			}

			// Token: 0x02003376 RID: 13174
			public abstract class ROCKETENGINEPOWER
			{
				// Token: 0x0400D2BE RID: 53950
				public static LocString NAME = "Engine Power";
			}

			// Token: 0x02003377 RID: 13175
			public abstract class FUELRANGEPERKILOGRAM
			{
				// Token: 0x0400D2BF RID: 53951
				public static LocString NAME = "Range";
			}

			// Token: 0x02003378 RID: 13176
			public abstract class HEIGHT
			{
				// Token: 0x0400D2C0 RID: 53952
				public static LocString NAME = "Height";
			}

			// Token: 0x02003379 RID: 13177
			public class WILTTEMPRANGEMOD
			{
				// Token: 0x0400D2C1 RID: 53953
				public static LocString NAME = "Viable Temperature Range";

				// Token: 0x0400D2C2 RID: 53954
				public static LocString DESC = "Variance growth temperature relative to the base crop";
			}

			// Token: 0x0200337A RID: 13178
			public class YIELDAMOUNT
			{
				// Token: 0x0400D2C3 RID: 53955
				public static LocString NAME = "Yield Amount";

				// Token: 0x0400D2C4 RID: 53956
				public static LocString DESC = "Plant production relative to the base crop";
			}

			// Token: 0x0200337B RID: 13179
			public class HARVESTTIME
			{
				// Token: 0x0400D2C5 RID: 53957
				public static LocString NAME = "Harvest Duration";

				// Token: 0x0400D2C6 RID: 53958
				public static LocString DESC = "Time it takes an unskilled Duplicant to harvest this plant";
			}

			// Token: 0x0200337C RID: 13180
			public class DECORBONUS
			{
				// Token: 0x0400D2C7 RID: 53959
				public static LocString NAME = "Decor Bonus";

				// Token: 0x0400D2C8 RID: 53960
				public static LocString DESC = "Change in Decor value relative to the base crop";
			}

			// Token: 0x0200337D RID: 13181
			public class MINLIGHTLUX
			{
				// Token: 0x0400D2C9 RID: 53961
				public static LocString NAME = "Light";

				// Token: 0x0400D2CA RID: 53962
				public static LocString DESC = "Minimum lux this plant requires for growth";
			}

			// Token: 0x0200337E RID: 13182
			public class FERTILIZERUSAGEMOD
			{
				// Token: 0x0400D2CB RID: 53963
				public static LocString NAME = "Fertilizer Usage";

				// Token: 0x0400D2CC RID: 53964
				public static LocString DESC = "Fertilizer and irrigation amounts this plant requires relative to the base crop";
			}

			// Token: 0x0200337F RID: 13183
			public class MINRADIATIONTHRESHOLD
			{
				// Token: 0x0400D2CD RID: 53965
				public static LocString NAME = "Minimum Radiation";

				// Token: 0x0400D2CE RID: 53966
				public static LocString DESC = "Smallest amount of ambient Radiation required for this plant to grow";
			}

			// Token: 0x02003380 RID: 13184
			public class MAXRADIATIONTHRESHOLD
			{
				// Token: 0x0400D2CF RID: 53967
				public static LocString NAME = "Maximum Radiation";

				// Token: 0x0400D2D0 RID: 53968
				public static LocString DESC = "Largest amount of ambient Radiation this plant can tolerate";
			}
		}

		// Token: 0x020021BB RID: 8635
		public class ROLES
		{
			// Token: 0x02003381 RID: 13185
			public class GROUPS
			{
				// Token: 0x0400D2D1 RID: 53969
				public static LocString APTITUDE_DESCRIPTION = string.Concat(new string[]
				{
					"This Duplicant will gain <b>{1}</b> ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when learning ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" Skills"
				});

				// Token: 0x0400D2D2 RID: 53970
				public static LocString APTITUDE_DESCRIPTION_CHOREGROUP = string.Concat(new string[]
				{
					"{2}\n\nThis Duplicant will gain <b>+{1}</b> ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when learning ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" Skills"
				});

				// Token: 0x0400D2D3 RID: 53971
				public static LocString SUITS = "Suit Wearing";
			}

			// Token: 0x02003382 RID: 13186
			public class NO_ROLE
			{
				// Token: 0x0400D2D4 RID: 53972
				public static LocString NAME = UI.FormatAsLink("Unemployed", "NO_ROLE");

				// Token: 0x0400D2D5 RID: 53973
				public static LocString DESCRIPTION = "No job assignment";
			}

			// Token: 0x02003383 RID: 13187
			public class JUNIOR_ARTIST
			{
				// Token: 0x0400D2D6 RID: 53974
				public static LocString NAME = UI.FormatAsLink("Art Fundamentals", "ARTING1");

				// Token: 0x0400D2D7 RID: 53975
				public static LocString DESCRIPTION = "Teaches the most basic level of art skill";
			}

			// Token: 0x02003384 RID: 13188
			public class ARTIST
			{
				// Token: 0x0400D2D8 RID: 53976
				public static LocString NAME = UI.FormatAsLink("Aesthetic Design", "ARTING2");

				// Token: 0x0400D2D9 RID: 53977
				public static LocString DESCRIPTION = "Allows moderately attractive art to be created";
			}

			// Token: 0x02003385 RID: 13189
			public class MASTER_ARTIST
			{
				// Token: 0x0400D2DA RID: 53978
				public static LocString NAME = UI.FormatAsLink("Masterworks", "ARTING3");

				// Token: 0x0400D2DB RID: 53979
				public static LocString DESCRIPTION = "Enables the painting and sculpting of masterpieces";
			}

			// Token: 0x02003386 RID: 13190
			public class JUNIOR_BUILDER
			{
				// Token: 0x0400D2DC RID: 53980
				public static LocString NAME = UI.FormatAsLink("Improved Construction I", "BUILDING1");

				// Token: 0x0400D2DD RID: 53981
				public static LocString DESCRIPTION = "Marginally improves a Duplicant's construction speeds";
			}

			// Token: 0x02003387 RID: 13191
			public class BUILDER
			{
				// Token: 0x0400D2DE RID: 53982
				public static LocString NAME = UI.FormatAsLink("Improved Construction II", "BUILDING2");

				// Token: 0x0400D2DF RID: 53983
				public static LocString DESCRIPTION = "Further increases a Duplicant's construction speeds";
			}

			// Token: 0x02003388 RID: 13192
			public class SENIOR_BUILDER
			{
				// Token: 0x0400D2E0 RID: 53984
				public static LocString NAME = UI.FormatAsLink("Demolition", "BUILDING3");

				// Token: 0x0400D2E1 RID: 53985
				public static LocString DESCRIPTION = "Enables a Duplicant to deconstruct Gravitas buildings";
			}

			// Token: 0x02003389 RID: 13193
			public class JUNIOR_RESEARCHER
			{
				// Token: 0x0400D2E2 RID: 53986
				public static LocString NAME = UI.FormatAsLink("Advanced Research", "RESEARCHING1");

				// Token: 0x0400D2E3 RID: 53987
				public static LocString DESCRIPTION = "Allows Duplicants to perform research using a " + BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME;
			}

			// Token: 0x0200338A RID: 13194
			public class RESEARCHER
			{
				// Token: 0x0400D2E4 RID: 53988
				public static LocString NAME = UI.FormatAsLink("Field Research", "RESEARCHING2");

				// Token: 0x0400D2E5 RID: 53989
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Duplicants can perform studies on ",
					UI.PRE_KEYWORD,
					"Geysers",
					UI.PST_KEYWORD,
					", ",
					UI.CLUSTERMAP.PLANETOID_KEYWORD,
					", and other geographical phenomena"
				});
			}

			// Token: 0x0200338B RID: 13195
			public class SENIOR_RESEARCHER
			{
				// Token: 0x0400D2E6 RID: 53990
				public static LocString NAME = UI.FormatAsLink("Astronomy", "ASTRONOMY");

				// Token: 0x0400D2E7 RID: 53991
				public static LocString DESCRIPTION = "Enables Duplicants to study outer space using the " + BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME;
			}

			// Token: 0x0200338C RID: 13196
			public class NUCLEAR_RESEARCHER
			{
				// Token: 0x0400D2E8 RID: 53992
				public static LocString NAME = UI.FormatAsLink("Applied Sciences Research", "ATOMICRESEARCH");

				// Token: 0x0400D2E9 RID: 53993
				public static LocString DESCRIPTION = "Enables Duplicants to study matter using the " + BUILDINGS.PREFABS.NUCLEARRESEARCHCENTER.NAME;
			}

			// Token: 0x0200338D RID: 13197
			public class SPACE_RESEARCHER
			{
				// Token: 0x0400D2EA RID: 53994
				public static LocString NAME = UI.FormatAsLink("Data Analysis Researcher", "SPACERESEARCH");

				// Token: 0x0400D2EB RID: 53995
				public static LocString DESCRIPTION = "Enables Duplicants to conduct research using the " + BUILDINGS.PREFABS.DLC1COSMICRESEARCHCENTER.NAME;
			}

			// Token: 0x0200338E RID: 13198
			public class JUNIOR_COOK
			{
				// Token: 0x0400D2EC RID: 53996
				public static LocString NAME = UI.FormatAsLink("Grilling", "COOKING1");

				// Token: 0x0400D2ED RID: 53997
				public static LocString DESCRIPTION = "Allows Duplicants to cook using the " + BUILDINGS.PREFABS.COOKINGSTATION.NAME;
			}

			// Token: 0x0200338F RID: 13199
			public class COOK
			{
				// Token: 0x0400D2EE RID: 53998
				public static LocString NAME = UI.FormatAsLink("Grilling II", "COOKING2");

				// Token: 0x0400D2EF RID: 53999
				public static LocString DESCRIPTION = "Improves a Duplicant's cooking speed";
			}

			// Token: 0x02003390 RID: 13200
			public class JUNIOR_MEDIC
			{
				// Token: 0x0400D2F0 RID: 54000
				public static LocString NAME = UI.FormatAsLink("Medicine Compounding", "MEDICINE1");

				// Token: 0x0400D2F1 RID: 54001
				public static LocString DESCRIPTION = "Allows Duplicants to produce medicines at the " + BUILDINGS.PREFABS.APOTHECARY.NAME;
			}

			// Token: 0x02003391 RID: 13201
			public class MEDIC
			{
				// Token: 0x0400D2F2 RID: 54002
				public static LocString NAME = UI.FormatAsLink("Bedside Manner", "MEDICINE2");

				// Token: 0x0400D2F3 RID: 54003
				public static LocString DESCRIPTION = "Trains Duplicants to administer medicine at the " + BUILDINGS.PREFABS.DOCTORSTATION.NAME;
			}

			// Token: 0x02003392 RID: 13202
			public class SENIOR_MEDIC
			{
				// Token: 0x0400D2F4 RID: 54004
				public static LocString NAME = UI.FormatAsLink("Advanced Medical Care", "MEDICINE3");

				// Token: 0x0400D2F5 RID: 54005
				public static LocString DESCRIPTION = "Trains Duplicants to operate the " + BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME;
			}

			// Token: 0x02003393 RID: 13203
			public class MACHINE_TECHNICIAN
			{
				// Token: 0x0400D2F6 RID: 54006
				public static LocString NAME = UI.FormatAsLink("Improved Tinkering", "TECHNICALS1");

				// Token: 0x0400D2F7 RID: 54007
				public static LocString DESCRIPTION = "Marginally improves a Duplicant's tinkering speeds";
			}

			// Token: 0x02003394 RID: 13204
			public class OIL_TECHNICIAN
			{
				// Token: 0x0400D2F8 RID: 54008
				public static LocString NAME = UI.FormatAsLink("Oil Engineering", "OIL_TECHNICIAN");

				// Token: 0x0400D2F9 RID: 54009
				public static LocString DESCRIPTION = "Allows the extraction and refinement of " + ELEMENTS.CRUDEOIL.NAME;
			}

			// Token: 0x02003395 RID: 13205
			public class HAULER
			{
				// Token: 0x0400D2FA RID: 54010
				public static LocString NAME = UI.FormatAsLink("Improved Carrying I", "HAULING1");

				// Token: 0x0400D2FB RID: 54011
				public static LocString DESCRIPTION = "Minorly increase a Duplicant's strength and carrying capacity";
			}

			// Token: 0x02003396 RID: 13206
			public class MATERIALS_MANAGER
			{
				// Token: 0x0400D2FC RID: 54012
				public static LocString NAME = UI.FormatAsLink("Improved Carrying II", "HAULING2");

				// Token: 0x0400D2FD RID: 54013
				public static LocString DESCRIPTION = "Further increases a Duplicant's strength and carrying capacity for even swifter deliveries";
			}

			// Token: 0x02003397 RID: 13207
			public class JUNIOR_FARMER
			{
				// Token: 0x0400D2FE RID: 54014
				public static LocString NAME = UI.FormatAsLink("Improved Farming I", "FARMING1");

				// Token: 0x0400D2FF RID: 54015
				public static LocString DESCRIPTION = "Minorly increase a Duplicant's farming skills, increasing their chances of harvesting new plant " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;
			}

			// Token: 0x02003398 RID: 13208
			public class FARMER
			{
				// Token: 0x0400D300 RID: 54016
				public static LocString NAME = UI.FormatAsLink("Crop Tending", "FARMING2");

				// Token: 0x0400D301 RID: 54017
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Enables tending ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					", which will increase their growth speed"
				});
			}

			// Token: 0x02003399 RID: 13209
			public class SENIOR_FARMER
			{
				// Token: 0x0400D302 RID: 54018
				public static LocString NAME = UI.FormatAsLink("Improved Farming II", "FARMING3");

				// Token: 0x0400D303 RID: 54019
				public static LocString DESCRIPTION = "Further increases a Duplicant's farming skills";
			}

			// Token: 0x0200339A RID: 13210
			public class JUNIOR_MINER
			{
				// Token: 0x0400D304 RID: 54020
				public static LocString NAME = UI.FormatAsLink("Hard Digging", "MINING1");

				// Token: 0x0400D305 RID: 54021
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows the excavation of ",
					UI.PRE_KEYWORD,
					ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM,
					UI.PST_KEYWORD,
					" materials such as ",
					ELEMENTS.GRANITE.NAME
				});
			}

			// Token: 0x0200339B RID: 13211
			public class MINER
			{
				// Token: 0x0400D306 RID: 54022
				public static LocString NAME = UI.FormatAsLink("Superhard Digging", "MINING2");

				// Token: 0x0400D307 RID: 54023
				public static LocString DESCRIPTION = "Allows the excavation of the element " + ELEMENTS.KATAIRITE.NAME;
			}

			// Token: 0x0200339C RID: 13212
			public class SENIOR_MINER
			{
				// Token: 0x0400D308 RID: 54024
				public static LocString NAME = UI.FormatAsLink("Super-Duperhard Digging", "MINING3");

				// Token: 0x0400D309 RID: 54025
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows the excavation of ",
					UI.PRE_KEYWORD,
					ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.NEARLYIMPENETRABLE,
					UI.PST_KEYWORD,
					" elements, including ",
					ELEMENTS.DIAMOND.NAME,
					" and ",
					ELEMENTS.OBSIDIAN.NAME
				});
			}

			// Token: 0x0200339D RID: 13213
			public class MASTER_MINER
			{
				// Token: 0x0400D30A RID: 54026
				public static LocString NAME = UI.FormatAsLink("Hazmat Digging", "MINING4");

				// Token: 0x0400D30B RID: 54027
				public static LocString DESCRIPTION = "Allows the excavation of dangerous materials like " + ELEMENTS.CORIUM.NAME;
			}

			// Token: 0x0200339E RID: 13214
			public class SUIT_DURABILITY
			{
				// Token: 0x0400D30C RID: 54028
				public static LocString NAME = UI.FormatAsLink("Suit Sustainability Training", "SUITDURABILITY");

				// Token: 0x0400D30D RID: 54029
				public static LocString DESCRIPTION = "Suits equipped by this Duplicant lose durability " + GameUtil.GetFormattedPercent(EQUIPMENT.SUITS.SUIT_DURABILITY_SKILL_BONUS * 100f, GameUtil.TimeSlice.None) + " slower.";
			}

			// Token: 0x0200339F RID: 13215
			public class SUIT_EXPERT
			{
				// Token: 0x0400D30E RID: 54030
				public static LocString NAME = UI.FormatAsLink("Exosuit Training", "SUITS1");

				// Token: 0x0400D30F RID: 54031
				public static LocString DESCRIPTION = "Eliminates the runspeed loss experienced while wearing exosuits";
			}

			// Token: 0x020033A0 RID: 13216
			public class POWER_TECHNICIAN
			{
				// Token: 0x0400D310 RID: 54032
				public static LocString NAME = UI.FormatAsLink("Electrical Engineering", "TECHNICALS2");

				// Token: 0x0400D311 RID: 54033
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Enables generator ",
					UI.PRE_KEYWORD,
					"Tune-Up",
					UI.PST_KEYWORD,
					", which will temporarily provide improved ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" output"
				});
			}

			// Token: 0x020033A1 RID: 13217
			public class MECHATRONIC_ENGINEER
			{
				// Token: 0x0400D312 RID: 54034
				public static LocString NAME = UI.FormatAsLink("Mechatronics Engineering", "ENGINEERING1");

				// Token: 0x0400D313 RID: 54035
				public static LocString DESCRIPTION = "Allows the construction and maintenance of " + BUILDINGS.PREFABS.SOLIDCONDUIT.NAME + " systems";
			}

			// Token: 0x020033A2 RID: 13218
			public class HANDYMAN
			{
				// Token: 0x0400D314 RID: 54036
				public static LocString NAME = UI.FormatAsLink("Improved Strength", "BASEKEEPING1");

				// Token: 0x0400D315 RID: 54037
				public static LocString DESCRIPTION = "Minorly improves a Duplicant's physical strength";
			}

			// Token: 0x020033A3 RID: 13219
			public class PLUMBER
			{
				// Token: 0x0400D316 RID: 54038
				public static LocString NAME = UI.FormatAsLink("Plumbing", "BASEKEEPING2");

				// Token: 0x0400D317 RID: 54039
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows a Duplicant to empty ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" without making a mess"
				});
			}

			// Token: 0x020033A4 RID: 13220
			public class PYROTECHNIC
			{
				// Token: 0x0400D318 RID: 54040
				public static LocString NAME = UI.FormatAsLink("Pyrotechnics", "PYROTECHNICS");

				// Token: 0x0400D319 RID: 54041
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows a Duplicant to make ",
					UI.PRE_KEYWORD,
					"Blastshot",
					UI.PST_KEYWORD,
					" for the ",
					UI.PRE_KEYWORD,
					"Meteor Blaster",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020033A5 RID: 13221
			public class RANCHER
			{
				// Token: 0x0400D31A RID: 54042
				public static LocString NAME = UI.FormatAsLink("Critter Ranching I", "RANCHING1");

				// Token: 0x0400D31B RID: 54043
				public static LocString DESCRIPTION = "Allows a Duplicant to handle and care for " + UI.FormatAsLink("Critters", "CREATURES");
			}

			// Token: 0x020033A6 RID: 13222
			public class SENIOR_RANCHER
			{
				// Token: 0x0400D31C RID: 54044
				public static LocString NAME = UI.FormatAsLink("Critter Ranching II", "RANCHING2");

				// Token: 0x0400D31D RID: 54045
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Improves a Duplicant's ",
					UI.PRE_KEYWORD,
					"Ranching",
					UI.PST_KEYWORD,
					" skills"
				});
			}

			// Token: 0x020033A7 RID: 13223
			public class ASTRONAUTTRAINEE
			{
				// Token: 0x0400D31E RID: 54046
				public static LocString NAME = UI.FormatAsLink("Rocket Piloting", "ASTRONAUTING1");

				// Token: 0x0400D31F RID: 54047
				public static LocString DESCRIPTION = "Allows a Duplicant to operate a " + BUILDINGS.PREFABS.COMMANDMODULE.NAME + " to pilot a rocket ship";
			}

			// Token: 0x020033A8 RID: 13224
			public class ASTRONAUT
			{
				// Token: 0x0400D320 RID: 54048
				public static LocString NAME = UI.FormatAsLink("Rocket Navigation", "ASTRONAUTING2");

				// Token: 0x0400D321 RID: 54049
				public static LocString DESCRIPTION = "Improves the speed that space missions are completed";
			}

			// Token: 0x020033A9 RID: 13225
			public class ROCKETPILOT
			{
				// Token: 0x0400D322 RID: 54050
				public static LocString NAME = UI.FormatAsLink("Rocket Piloting", "ROCKETPILOTING1");

				// Token: 0x0400D323 RID: 54051
				public static LocString DESCRIPTION = "Allows a Duplicant to operate a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME + " and pilot rockets";
			}

			// Token: 0x020033AA RID: 13226
			public class SENIOR_ROCKETPILOT
			{
				// Token: 0x0400D324 RID: 54052
				public static LocString NAME = UI.FormatAsLink("Rocket Piloting II", "ROCKETPILOTING2");

				// Token: 0x0400D325 RID: 54053
				public static LocString DESCRIPTION = "Allows Duplicants to pilot rockets at faster speeds";
			}

			// Token: 0x020033AB RID: 13227
			public class USELESSSKILL
			{
				// Token: 0x0400D326 RID: 54054
				public static LocString NAME = "W.I.P. Skill";

				// Token: 0x0400D327 RID: 54055
				public static LocString DESCRIPTION = "This skill doesn't really do anything right now.";
			}
		}

		// Token: 0x020021BC RID: 8636
		public class THOUGHTS
		{
			// Token: 0x020033AC RID: 13228
			public class STARVING
			{
				// Token: 0x0400D328 RID: 54056
				public static LocString TOOLTIP = "Starving";
			}

			// Token: 0x020033AD RID: 13229
			public class HOT
			{
				// Token: 0x0400D329 RID: 54057
				public static LocString TOOLTIP = "Hot";
			}

			// Token: 0x020033AE RID: 13230
			public class COLD
			{
				// Token: 0x0400D32A RID: 54058
				public static LocString TOOLTIP = "Cold";
			}

			// Token: 0x020033AF RID: 13231
			public class BREAKBLADDER
			{
				// Token: 0x0400D32B RID: 54059
				public static LocString TOOLTIP = "Washroom Break";
			}

			// Token: 0x020033B0 RID: 13232
			public class FULLBLADDER
			{
				// Token: 0x0400D32C RID: 54060
				public static LocString TOOLTIP = "Full Bladder";
			}

			// Token: 0x020033B1 RID: 13233
			public class EXPELLGUNKDESIRE
			{
				// Token: 0x0400D32D RID: 54061
				public static LocString TOOLTIP = "Expel Gunk";
			}

			// Token: 0x020033B2 RID: 13234
			public class EXPELLINGSPOILEDOIL
			{
				// Token: 0x0400D32E RID: 54062
				public static LocString TOOLTIP = "Spilling Oil";
			}

			// Token: 0x020033B3 RID: 13235
			public class HAPPY
			{
				// Token: 0x0400D32F RID: 54063
				public static LocString TOOLTIP = "Happy";
			}

			// Token: 0x020033B4 RID: 13236
			public class UNHAPPY
			{
				// Token: 0x0400D330 RID: 54064
				public static LocString TOOLTIP = "Unhappy";
			}

			// Token: 0x020033B5 RID: 13237
			public class POORDECOR
			{
				// Token: 0x0400D331 RID: 54065
				public static LocString TOOLTIP = "Poor Decor";
			}

			// Token: 0x020033B6 RID: 13238
			public class POOR_FOOD_QUALITY
			{
				// Token: 0x0400D332 RID: 54066
				public static LocString TOOLTIP = "Lousy Meal";
			}

			// Token: 0x020033B7 RID: 13239
			public class GOOD_FOOD_QUALITY
			{
				// Token: 0x0400D333 RID: 54067
				public static LocString TOOLTIP = "Delicious Meal";
			}

			// Token: 0x020033B8 RID: 13240
			public class SLEEPY
			{
				// Token: 0x0400D334 RID: 54068
				public static LocString TOOLTIP = "Sleepy";
			}

			// Token: 0x020033B9 RID: 13241
			public class DREAMY
			{
				// Token: 0x0400D335 RID: 54069
				public static LocString TOOLTIP = "Dreaming";
			}

			// Token: 0x020033BA RID: 13242
			public class SUFFOCATING
			{
				// Token: 0x0400D336 RID: 54070
				public static LocString TOOLTIP = "Suffocating";
			}

			// Token: 0x020033BB RID: 13243
			public class ANGRY
			{
				// Token: 0x0400D337 RID: 54071
				public static LocString TOOLTIP = "Angry";
			}

			// Token: 0x020033BC RID: 13244
			public class RAGING
			{
				// Token: 0x0400D338 RID: 54072
				public static LocString TOOLTIP = "Raging";
			}

			// Token: 0x020033BD RID: 13245
			public class GOTINFECTED
			{
				// Token: 0x0400D339 RID: 54073
				public static LocString TOOLTIP = "Got Infected";
			}

			// Token: 0x020033BE RID: 13246
			public class PUTRIDODOUR
			{
				// Token: 0x0400D33A RID: 54074
				public static LocString TOOLTIP = "Smelled Something Putrid";
			}

			// Token: 0x020033BF RID: 13247
			public class NOISY
			{
				// Token: 0x0400D33B RID: 54075
				public static LocString TOOLTIP = "Loud Area";
			}

			// Token: 0x020033C0 RID: 13248
			public class NEWROLE
			{
				// Token: 0x0400D33C RID: 54076
				public static LocString TOOLTIP = "New Skill";
			}

			// Token: 0x020033C1 RID: 13249
			public class CHATTY
			{
				// Token: 0x0400D33D RID: 54077
				public static LocString TOOLTIP = "Greeting";
			}

			// Token: 0x020033C2 RID: 13250
			public class ENCOURAGE
			{
				// Token: 0x0400D33E RID: 54078
				public static LocString TOOLTIP = "Encouraging";
			}

			// Token: 0x020033C3 RID: 13251
			public class CONVERSATION
			{
				// Token: 0x0400D33F RID: 54079
				public static LocString TOOLTIP = "Chatting";
			}

			// Token: 0x020033C4 RID: 13252
			public class CATCHYTUNE
			{
				// Token: 0x0400D340 RID: 54080
				public static LocString TOOLTIP = "WHISTLING";
			}
		}
	}
}
