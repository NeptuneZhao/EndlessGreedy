using System;

namespace STRINGS
{
	// Token: 0x02000F14 RID: 3860
	public class BUILDING
	{
		// Token: 0x02002197 RID: 8599
		public class STATUSITEMS
		{
			// Token: 0x02002CBE RID: 11454
			public class GUNKEMPTIERFULL
			{
				// Token: 0x0400C244 RID: 49732
				public static LocString NAME = "Storage Full";

				// Token: 0x0400C245 RID: 49733
				public static LocString TOOLTIP = "This building's internal storage is at maximum capacity\n\nIt must be emptied before its next use";
			}

			// Token: 0x02002CBF RID: 11455
			public class MERCURYLIGHT_CHARGING
			{
				// Token: 0x0400C246 RID: 49734
				public static LocString NAME = "Powering Up: {0}";

				// Token: 0x0400C247 RID: 49735
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" levels are gradually increasing\n\nIf its ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" requirements continue to be met, it will reach maximum brightness in {0}"
				});
			}

			// Token: 0x02002CC0 RID: 11456
			public class MERCURYLIGHT_DEPLEATING
			{
				// Token: 0x0400C248 RID: 49736
				public static LocString NAME = "Brightness: {0}";

				// Token: 0x0400C249 RID: 49737
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" output is decreasing because its ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" requirements are not being met\n\nIt will power off once its stores are depleted"
				});
			}

			// Token: 0x02002CC1 RID: 11457
			public class MERCURYLIGHT_DEPLEATED
			{
				// Token: 0x0400C24A RID: 49738
				public static LocString NAME = "Powered Off";

				// Token: 0x0400C24B RID: 49739
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is non-operational due to a lack of resources\n\nIt will begin to power up when its ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" requirements are met"
				});
			}

			// Token: 0x02002CC2 RID: 11458
			public class MERCURYLIGHT_CHARGED
			{
				// Token: 0x0400C24C RID: 49740
				public static LocString NAME = "Fully Charged";

				// Token: 0x0400C24D RID: 49741
				public static LocString TOOLTIP = "This building is functioning at maximum capacity";
			}

			// Token: 0x02002CC3 RID: 11459
			public class SPECIALCARGOBAYCLUSTERCRITTERSTORED
			{
				// Token: 0x0400C24E RID: 49742
				public static LocString NAME = "Contents: {0}";

				// Token: 0x0400C24F RID: 49743
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002CC4 RID: 11460
			public class GEOTUNER_NEEDGEYSER
			{
				// Token: 0x0400C250 RID: 49744
				public static LocString NAME = "No Geyser Selected";

				// Token: 0x0400C251 RID: 49745
				public static LocString TOOLTIP = "Select an analyzed geyser to increase its output";
			}

			// Token: 0x02002CC5 RID: 11461
			public class GEOTUNER_CHARGE_REQUIRED
			{
				// Token: 0x0400C252 RID: 49746
				public static LocString NAME = "Experimentation Needed";

				// Token: 0x0400C253 RID: 49747
				public static LocString TOOLTIP = "This building requires a Duplicant to produce amplification data through experimentation";
			}

			// Token: 0x02002CC6 RID: 11462
			public class GEOTUNER_CHARGING
			{
				// Token: 0x0400C254 RID: 49748
				public static LocString NAME = "Compiling Data";

				// Token: 0x0400C255 RID: 49749
				public static LocString TOOLTIP = "Compiling amplification data through experimentation";
			}

			// Token: 0x02002CC7 RID: 11463
			public class GEOTUNER_CHARGED
			{
				// Token: 0x0400C256 RID: 49750
				public static LocString NAME = "Data Remaining: {0}";

				// Token: 0x0400C257 RID: 49751
				public static LocString TOOLTIP = "This building consumes amplification data while boosting a geyser\n\nTime remaining: {0} ({1} data per second)";
			}

			// Token: 0x02002CC8 RID: 11464
			public class GEOTUNER_GEYSER_STATUS
			{
				// Token: 0x0400C258 RID: 49752
				public static LocString NAME = "";

				// Token: 0x0400C259 RID: 49753
				public static LocString NAME_ERUPTING = "Target is Erupting";

				// Token: 0x0400C25A RID: 49754
				public static LocString NAME_DORMANT = "Target is Not Erupting";

				// Token: 0x0400C25B RID: 49755
				public static LocString NAME_IDLE = "Target is Not Erupting";

				// Token: 0x0400C25C RID: 49756
				public static LocString TOOLTIP = "";

				// Token: 0x0400C25D RID: 49757
				public static LocString TOOLTIP_ERUPTING = "The selected geyser is erupting and will receive stored amplification data";

				// Token: 0x0400C25E RID: 49758
				public static LocString TOOLTIP_DORMANT = "The selected geyser is not erupting\n\nIt will not receive stored amplification data in this state";

				// Token: 0x0400C25F RID: 49759
				public static LocString TOOLTIP_IDLE = "The selected geyser is not erupting\n\nIt will not receive stored amplification data in this state";
			}

			// Token: 0x02002CC9 RID: 11465
			public class GEYSER_GEOTUNED
			{
				// Token: 0x0400C260 RID: 49760
				public static LocString NAME = "Geotuned ({0}/{1})";

				// Token: 0x0400C261 RID: 49761
				public static LocString TOOLTIP = "This geyser is being boosted by {0} out {1} of " + UI.PRE_KEYWORD + "Geotuners" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CCA RID: 11466
			public class RADIATOR_ENERGY_CURRENT_EMISSION_RATE
			{
				// Token: 0x0400C262 RID: 49762
				public static LocString NAME = "Currently Emitting: {ENERGY_RATE}";

				// Token: 0x0400C263 RID: 49763
				public static LocString TOOLTIP = "Currently Emitting: {ENERGY_RATE}";
			}

			// Token: 0x02002CCB RID: 11467
			public class NOTLINKEDTOHEAD
			{
				// Token: 0x0400C264 RID: 49764
				public static LocString NAME = "Not Linked";

				// Token: 0x0400C265 RID: 49765
				public static LocString TOOLTIP = "This building must be built adjacent to a {headBuilding} or another {linkBuilding} in order to function";
			}

			// Token: 0x02002CCC RID: 11468
			public class BAITED
			{
				// Token: 0x0400C266 RID: 49766
				public static LocString NAME = "{0} Bait";

				// Token: 0x0400C267 RID: 49767
				public static LocString TOOLTIP = "This lure is baited with {0}\n\nBait material is set during the construction of the building";
			}

			// Token: 0x02002CCD RID: 11469
			public class NOCOOLANT
			{
				// Token: 0x0400C268 RID: 49768
				public static LocString NAME = "No Coolant";

				// Token: 0x0400C269 RID: 49769
				public static LocString TOOLTIP = "This building needs coolant";
			}

			// Token: 0x02002CCE RID: 11470
			public class ANGERDAMAGE
			{
				// Token: 0x0400C26A RID: 49770
				public static LocString NAME = "Damage: Duplicant Tantrum";

				// Token: 0x0400C26B RID: 49771
				public static LocString TOOLTIP = "A stressed Duplicant is damaging this building";

				// Token: 0x0400C26C RID: 49772
				public static LocString NOTIFICATION = "Building Damage: Duplicant Tantrum";

				// Token: 0x0400C26D RID: 49773
				public static LocString NOTIFICATION_TOOLTIP = "Stressed Duplicants are damaging these buildings:\n\n{0}";
			}

			// Token: 0x02002CCF RID: 11471
			public class PIPECONTENTS
			{
				// Token: 0x0400C26E RID: 49774
				public static LocString EMPTY = "Empty";

				// Token: 0x0400C26F RID: 49775
				public static LocString CONTENTS = "{0} of {1} at {2}";

				// Token: 0x0400C270 RID: 49776
				public static LocString CONTENTS_WITH_DISEASE = "\n  {0}";
			}

			// Token: 0x02002CD0 RID: 11472
			public class CONVEYOR_CONTENTS
			{
				// Token: 0x0400C271 RID: 49777
				public static LocString EMPTY = "Empty";

				// Token: 0x0400C272 RID: 49778
				public static LocString CONTENTS = "{0} of {1} at {2}";

				// Token: 0x0400C273 RID: 49779
				public static LocString CONTENTS_WITH_DISEASE = "\n  {0}";
			}

			// Token: 0x02002CD1 RID: 11473
			public class ASSIGNEDTO
			{
				// Token: 0x0400C274 RID: 49780
				public static LocString NAME = "Assigned to: {Assignee}";

				// Token: 0x0400C275 RID: 49781
				public static LocString TOOLTIP = "Only {Assignee} can use this amenity";
			}

			// Token: 0x02002CD2 RID: 11474
			public class ASSIGNEDPUBLIC
			{
				// Token: 0x0400C276 RID: 49782
				public static LocString NAME = "Assigned to: Public";

				// Token: 0x0400C277 RID: 49783
				public static LocString TOOLTIP = "Any Duplicant can use this amenity";
			}

			// Token: 0x02002CD3 RID: 11475
			public class ASSIGNEDTOROOM
			{
				// Token: 0x0400C278 RID: 49784
				public static LocString NAME = "Assigned to: {0}";

				// Token: 0x0400C279 RID: 49785
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Any Duplicant assigned to this ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" can use this amenity"
				});
			}

			// Token: 0x02002CD4 RID: 11476
			public class AWAITINGSEEDDELIVERY
			{
				// Token: 0x0400C27A RID: 49786
				public static LocString NAME = "Awaiting Delivery";

				// Token: 0x0400C27B RID: 49787
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CD5 RID: 11477
			public class AWAITINGBAITDELIVERY
			{
				// Token: 0x0400C27C RID: 49788
				public static LocString NAME = "Awaiting Bait";

				// Token: 0x0400C27D RID: 49789
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Bait" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CD6 RID: 11478
			public class CLINICOUTSIDEHOSPITAL
			{
				// Token: 0x0400C27E RID: 49790
				public static LocString NAME = "Medical building outside Hospital";

				// Token: 0x0400C27F RID: 49791
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Rebuild this medical equipment in a ",
					UI.PRE_KEYWORD,
					"Hospital",
					UI.PST_KEYWORD,
					" to more effectively quarantine sick Duplicants"
				});
			}

			// Token: 0x02002CD7 RID: 11479
			public class BOTTLE_EMPTIER
			{
				// Token: 0x02003781 RID: 14209
				public static class ALLOWED
				{
					// Token: 0x0400DCBB RID: 56507
					public static LocString NAME = "Auto-Bottle: On";

					// Token: 0x0400DCBC RID: 56508
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may specifically fetch ",
						UI.PRE_KEYWORD,
						"Liquid",
						UI.PST_KEYWORD,
						" from a bottling station to bring to this location"
					});
				}

				// Token: 0x02003782 RID: 14210
				public static class DENIED
				{
					// Token: 0x0400DCBD RID: 56509
					public static LocString NAME = "Auto-Bottle: Off";

					// Token: 0x0400DCBE RID: 56510
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may not specifically fetch ",
						UI.PRE_KEYWORD,
						"Liquid",
						UI.PST_KEYWORD,
						" from a bottling station to bring to this location"
					});
				}
			}

			// Token: 0x02002CD8 RID: 11480
			public class CANISTER_EMPTIER
			{
				// Token: 0x02003783 RID: 14211
				public static class ALLOWED
				{
					// Token: 0x0400DCBF RID: 56511
					public static LocString NAME = "Auto-Bottle: On";

					// Token: 0x0400DCC0 RID: 56512
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may specifically fetch ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" from a canister filling station to bring to this location"
					});
				}

				// Token: 0x02003784 RID: 14212
				public static class DENIED
				{
					// Token: 0x0400DCC1 RID: 56513
					public static LocString NAME = "Auto-Bottle: Off";

					// Token: 0x0400DCC2 RID: 56514
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may not specifically fetch ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" from a canister filling station to bring to this location"
					});
				}
			}

			// Token: 0x02002CD9 RID: 11481
			public class BROKEN
			{
				// Token: 0x0400C280 RID: 49792
				public static LocString NAME = "Broken";

				// Token: 0x0400C281 RID: 49793
				public static LocString TOOLTIP = "This building received damage from <b>{DamageInfo}</b>\n\nIt will not function until it receives repairs";
			}

			// Token: 0x02002CDA RID: 11482
			public class CHANGESTORAGETILETARGET
			{
				// Token: 0x0400C282 RID: 49794
				public static LocString NAME = "Set Storage: {TargetName}";

				// Token: 0x0400C283 RID: 49795
				public static LocString TOOLTIP = "Waiting for a Duplicant to reassign this storage to {TargetName}";

				// Token: 0x0400C284 RID: 49796
				public static LocString EMPTY = "Empty";
			}

			// Token: 0x02002CDB RID: 11483
			public class CHANGEDOORCONTROLSTATE
			{
				// Token: 0x0400C285 RID: 49797
				public static LocString NAME = "Pending Door State Change: {ControlState}";

				// Token: 0x0400C286 RID: 49798
				public static LocString TOOLTIP = "Waiting for a Duplicant to change control state";
			}

			// Token: 0x02002CDC RID: 11484
			public class DISPENSEREQUESTED
			{
				// Token: 0x0400C287 RID: 49799
				public static LocString NAME = "Dispense Requested";

				// Token: 0x0400C288 RID: 49800
				public static LocString TOOLTIP = "Waiting for a Duplicant to dispense the item";
			}

			// Token: 0x02002CDD RID: 11485
			public class SUIT_LOCKER
			{
				// Token: 0x02003785 RID: 14213
				public class NEED_CONFIGURATION
				{
					// Token: 0x0400DCC3 RID: 56515
					public static LocString NAME = "Current Status: Needs Configuration";

					// Token: 0x0400DCC4 RID: 56516
					public static LocString TOOLTIP = "Set this dock to store a suit or leave it empty";
				}

				// Token: 0x02003786 RID: 14214
				public class READY
				{
					// Token: 0x0400DCC5 RID: 56517
					public static LocString NAME = "Current Status: Empty";

					// Token: 0x0400DCC6 RID: 56518
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock is ready to receive a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD,
						", either by manual delivery or from a Duplicant returning the suit they're wearing"
					});
				}

				// Token: 0x02003787 RID: 14215
				public class SUIT_REQUESTED
				{
					// Token: 0x0400DCC7 RID: 56519
					public static LocString NAME = "Current Status: Awaiting Delivery";

					// Token: 0x0400DCC8 RID: 56520
					public static LocString TOOLTIP = "Waiting for a Duplicant to deliver a " + UI.PRE_KEYWORD + "Suit" + UI.PST_KEYWORD;
				}

				// Token: 0x02003788 RID: 14216
				public class CHARGING
				{
					// Token: 0x0400DCC9 RID: 56521
					public static LocString NAME = "Current Status: Charging Suit";

					// Token: 0x0400DCCA RID: 56522
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD,
						" is docked and refueling"
					});
				}

				// Token: 0x02003789 RID: 14217
				public class NO_OXYGEN
				{
					// Token: 0x0400DCCB RID: 56523
					public static LocString NAME = "Current Status: No Oxygen";

					// Token: 0x0400DCCC RID: 56524
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.OXYGEN.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x0200378A RID: 14218
				public class NO_FUEL
				{
					// Token: 0x0400DCCD RID: 56525
					public static LocString NAME = "Current Status: No Fuel";

					// Token: 0x0400DCCE RID: 56526
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.PETROLEUM.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x0200378B RID: 14219
				public class NO_COOLANT
				{
					// Token: 0x0400DCCF RID: 56527
					public static LocString NAME = "Current Status: No Coolant";

					// Token: 0x0400DCD0 RID: 56528
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.WATER.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x0200378C RID: 14220
				public class NOT_OPERATIONAL
				{
					// Token: 0x0400DCD1 RID: 56529
					public static LocString NAME = "Current Status: Offline";

					// Token: 0x0400DCD2 RID: 56530
					public static LocString TOOLTIP = "This dock requires " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
				}

				// Token: 0x0200378D RID: 14221
				public class FULLY_CHARGED
				{
					// Token: 0x0400DCD3 RID: 56531
					public static LocString NAME = "Current Status: Full Fueled";

					// Token: 0x0400DCD4 RID: 56532
					public static LocString TOOLTIP = "This suit is fully refueled and ready for use";
				}
			}

			// Token: 0x02002CDE RID: 11486
			public class SUITMARKERTRAVERSALONLYWHENROOMAVAILABLE
			{
				// Token: 0x0400C289 RID: 49801
				public static LocString NAME = "Clearance: Vacancy Only";

				// Token: 0x0400C28A RID: 49802
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Suited Duplicants may pass only if there is room in a ",
					UI.PRE_KEYWORD,
					"Dock",
					UI.PST_KEYWORD,
					" to store their ",
					UI.PRE_KEYWORD,
					"Suit",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002CDF RID: 11487
			public class SUITMARKERTRAVERSALANYTIME
			{
				// Token: 0x0400C28B RID: 49803
				public static LocString NAME = "Clearance: Always Permitted";

				// Token: 0x0400C28C RID: 49804
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Suited Duplicants may pass even if there is no room to store their ",
					UI.PRE_KEYWORD,
					"Suits",
					UI.PST_KEYWORD,
					"\n\nWhen all available docks are full, Duplicants will unequip their ",
					UI.PRE_KEYWORD,
					"Suits",
					UI.PST_KEYWORD,
					" and drop them on the floor"
				});
			}

			// Token: 0x02002CE0 RID: 11488
			public class SUIT_LOCKER_NEEDS_CONFIGURATION
			{
				// Token: 0x0400C28D RID: 49805
				public static LocString NAME = "Not Configured";

				// Token: 0x0400C28E RID: 49806
				public static LocString TOOLTIP = "Dock settings not configured";
			}

			// Token: 0x02002CE1 RID: 11489
			public class CURRENTDOORCONTROLSTATE
			{
				// Token: 0x0400C28F RID: 49807
				public static LocString NAME = "Current State: {ControlState}";

				// Token: 0x0400C290 RID: 49808
				public static LocString TOOLTIP = "Current State: {ControlState}\n\nAuto: Duplicants open and close this door as needed\nLocked: Nothing may pass through\nOpen: This door will remain open";

				// Token: 0x0400C291 RID: 49809
				public static LocString OPENED = "Opened";

				// Token: 0x0400C292 RID: 49810
				public static LocString AUTO = "Auto";

				// Token: 0x0400C293 RID: 49811
				public static LocString LOCKED = "Locked";
			}

			// Token: 0x02002CE2 RID: 11490
			public class CONDUITBLOCKED
			{
				// Token: 0x0400C294 RID: 49812
				public static LocString NAME = "Pipe Blocked";

				// Token: 0x0400C295 RID: 49813
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x02002CE3 RID: 11491
			public class OUTPUTTILEBLOCKED
			{
				// Token: 0x0400C296 RID: 49814
				public static LocString NAME = "Output Blocked";

				// Token: 0x0400C297 RID: 49815
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x02002CE4 RID: 11492
			public class CONDUITBLOCKEDMULTIPLES
			{
				// Token: 0x0400C298 RID: 49816
				public static LocString NAME = "Pipe Blocked";

				// Token: 0x0400C299 RID: 49817
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x02002CE5 RID: 11493
			public class SOLIDCONDUITBLOCKEDMULTIPLES
			{
				// Token: 0x0400C29A RID: 49818
				public static LocString NAME = "Conveyor Rail Blocked";

				// Token: 0x0400C29B RID: 49819
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Conveyor Rail",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x02002CE6 RID: 11494
			public class OUTPUTPIPEFULL
			{
				// Token: 0x0400C29C RID: 49820
				public static LocString NAME = "Output Pipe Full";

				// Token: 0x0400C29D RID: 49821
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Unable to flush contents, output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x02002CE7 RID: 11495
			public class CONSTRUCTIONUNREACHABLE
			{
				// Token: 0x0400C29E RID: 49822
				public static LocString NAME = "Unreachable Build";

				// Token: 0x0400C29F RID: 49823
				public static LocString TOOLTIP = "Duplicants cannot reach this construction site";
			}

			// Token: 0x02002CE8 RID: 11496
			public class MOPUNREACHABLE
			{
				// Token: 0x0400C2A0 RID: 49824
				public static LocString NAME = "Unreachable Mop";

				// Token: 0x0400C2A1 RID: 49825
				public static LocString TOOLTIP = "Duplicants cannot reach this area";
			}

			// Token: 0x02002CE9 RID: 11497
			public class DEADREACTORCOOLINGOFF
			{
				// Token: 0x0400C2A2 RID: 49826
				public static LocString NAME = "Cooling ({CyclesRemaining} cycles remaining)";

				// Token: 0x0400C2A3 RID: 49827
				public static LocString TOOLTIP = "The radiation coming from this reactor is diminishing";
			}

			// Token: 0x02002CEA RID: 11498
			public class DIGUNREACHABLE
			{
				// Token: 0x0400C2A4 RID: 49828
				public static LocString NAME = "Unreachable Dig";

				// Token: 0x0400C2A5 RID: 49829
				public static LocString TOOLTIP = "Duplicants cannot reach this area";
			}

			// Token: 0x02002CEB RID: 11499
			public class STORAGEUNREACHABLE
			{
				// Token: 0x0400C2A6 RID: 49830
				public static LocString NAME = "Unreachable Storage";

				// Token: 0x0400C2A7 RID: 49831
				public static LocString TOOLTIP = "Duplicants cannot reach this storage unit";
			}

			// Token: 0x02002CEC RID: 11500
			public class PASSENGERMODULEUNREACHABLE
			{
				// Token: 0x0400C2A8 RID: 49832
				public static LocString NAME = "Unreachable Module";

				// Token: 0x0400C2A9 RID: 49833
				public static LocString TOOLTIP = "Duplicants cannot reach this rocket module";
			}

			// Token: 0x02002CED RID: 11501
			public class CONSTRUCTABLEDIGUNREACHABLE
			{
				// Token: 0x0400C2AA RID: 49834
				public static LocString NAME = "Unreachable Dig";

				// Token: 0x0400C2AB RID: 49835
				public static LocString TOOLTIP = "This construction site contains cells that cannot be dug out";
			}

			// Token: 0x02002CEE RID: 11502
			public class EMPTYPUMPINGSTATION
			{
				// Token: 0x0400C2AC RID: 49836
				public static LocString NAME = "Empty";

				// Token: 0x0400C2AD RID: 49837
				public static LocString TOOLTIP = "This pumping station cannot access any " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CEF RID: 11503
			public class ENTOMBED
			{
				// Token: 0x0400C2AE RID: 49838
				public static LocString NAME = "Entombed";

				// Token: 0x0400C2AF RID: 49839
				public static LocString TOOLTIP = "Must be dug out by a Duplicant";

				// Token: 0x0400C2B0 RID: 49840
				public static LocString NOTIFICATION_NAME = "Building entombment";

				// Token: 0x0400C2B1 RID: 49841
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are entombed and need to be dug out:";
			}

			// Token: 0x02002CF0 RID: 11504
			public class ELECTROBANKJOULESAVAILABLE
			{
				// Token: 0x0400C2B2 RID: 49842
				public static LocString NAME = "Power Remaining: {JoulesAvailable} / {JoulesCapacity}";

				// Token: 0x0400C2B3 RID: 49843
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"<b>{JoulesAvailable}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" available for use\n\nMaximum capacity: {JoulesCapacity}"
				});
			}

			// Token: 0x02002CF1 RID: 11505
			public class FABRICATORACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400C2B4 RID: 49844
				public static LocString NAME = "Fabricator accepts mutant seeds";

				// Token: 0x0400C2B5 RID: 49845
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This fabricator is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as recipe ingredients"
				});
			}

			// Token: 0x02002CF2 RID: 11506
			public class FISHFEEDERACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400C2B6 RID: 49846
				public static LocString NAME = "Fish Feeder accepts mutant seeds";

				// Token: 0x0400C2B7 RID: 49847
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This fish feeder is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as fish food"
				});
			}

			// Token: 0x02002CF3 RID: 11507
			public class INVALIDPORTOVERLAP
			{
				// Token: 0x0400C2B8 RID: 49848
				public static LocString NAME = "Invalid Port Overlap";

				// Token: 0x0400C2B9 RID: 49849
				public static LocString TOOLTIP = "Ports on this building overlap those on another building\n\nThis building must be rebuilt in a valid location";

				// Token: 0x0400C2BA RID: 49850
				public static LocString NOTIFICATION_NAME = "Building has overlapping ports";

				// Token: 0x0400C2BB RID: 49851
				public static LocString NOTIFICATION_TOOLTIP = "These buildings must be rebuilt with non-overlapping ports:";
			}

			// Token: 0x02002CF4 RID: 11508
			public class GENESHUFFLECOMPLETED
			{
				// Token: 0x0400C2BC RID: 49852
				public static LocString NAME = "Vacillation Complete";

				// Token: 0x0400C2BD RID: 49853
				public static LocString TOOLTIP = "The Duplicant has completed the neural vacillation process and is ready to be released";
			}

			// Token: 0x02002CF5 RID: 11509
			public class OVERHEATED
			{
				// Token: 0x0400C2BE RID: 49854
				public static LocString NAME = "Damage: Overheating";

				// Token: 0x0400C2BF RID: 49855
				public static LocString TOOLTIP = "This building is taking damage and will break down if not cooled";
			}

			// Token: 0x02002CF6 RID: 11510
			public class OVERLOADED
			{
				// Token: 0x0400C2C0 RID: 49856
				public static LocString NAME = "Damage: Overloading";

				// Token: 0x0400C2C1 RID: 49857
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Wire",
					UI.PST_KEYWORD,
					" is taking damage because there are too many buildings pulling ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from this circuit\n\nSplit this ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" circuit into multiple circuits, or use higher quality ",
					UI.PRE_KEYWORD,
					"Wires",
					UI.PST_KEYWORD,
					" to prevent overloading"
				});
			}

			// Token: 0x02002CF7 RID: 11511
			public class LOGICOVERLOADED
			{
				// Token: 0x0400C2C2 RID: 49858
				public static LocString NAME = "Damage: Overloading";

				// Token: 0x0400C2C3 RID: 49859
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Logic Wire",
					UI.PST_KEYWORD,
					" is taking damage\n\nLimit the output to one Bit, or replace it with ",
					UI.PRE_KEYWORD,
					"Logic Ribbon",
					UI.PST_KEYWORD,
					" to prevent further damage"
				});
			}

			// Token: 0x02002CF8 RID: 11512
			public class OPERATINGENERGY
			{
				// Token: 0x0400C2C4 RID: 49860
				public static LocString NAME = "Heat Production: {0}/s";

				// Token: 0x0400C2C5 RID: 49861
				public static LocString TOOLTIP = "This building is producing <b>{0}</b> per second\n\nSources:\n{1}";

				// Token: 0x0400C2C6 RID: 49862
				public static LocString LINEITEM = "    • {0}: {1}\n";

				// Token: 0x0400C2C7 RID: 49863
				public static LocString OPERATING = "Normal operation";

				// Token: 0x0400C2C8 RID: 49864
				public static LocString EXHAUSTING = "Excess produced";

				// Token: 0x0400C2C9 RID: 49865
				public static LocString PIPECONTENTS_TRANSFER = "Transferred from pipes";

				// Token: 0x0400C2CA RID: 49866
				public static LocString FOOD_TRANSFER = "Internal Cooling";
			}

			// Token: 0x02002CF9 RID: 11513
			public class FLOODED
			{
				// Token: 0x0400C2CB RID: 49867
				public static LocString NAME = "Building Flooded";

				// Token: 0x0400C2CC RID: 49868
				public static LocString TOOLTIP = "Building cannot function at current saturation";

				// Token: 0x0400C2CD RID: 49869
				public static LocString NOTIFICATION_NAME = "Flooding";

				// Token: 0x0400C2CE RID: 49870
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are flooded:";
			}

			// Token: 0x02002CFA RID: 11514
			public class NOTSUBMERGED
			{
				// Token: 0x0400C2CF RID: 49871
				public static LocString NAME = "Building Not Submerged";

				// Token: 0x0400C2D0 RID: 49872
				public static LocString TOOLTIP = "Building cannot function unless submerged in liquid";
			}

			// Token: 0x02002CFB RID: 11515
			public class GASVENTOBSTRUCTED
			{
				// Token: 0x0400C2D1 RID: 49873
				public static LocString NAME = "Gas Vent Obstructed";

				// Token: 0x0400C2D2 RID: 49874
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" has been obstructed and is preventing ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" flow to this vent"
				});
			}

			// Token: 0x02002CFC RID: 11516
			public class GASVENTOVERPRESSURE
			{
				// Token: 0x0400C2D3 RID: 49875
				public static LocString NAME = "Gas Vent Overpressure";

				// Token: 0x0400C2D4 RID: 49876
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" pressure in this area is preventing further ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" emission\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x02002CFD RID: 11517
			public class DIRECTION_CONTROL
			{
				// Token: 0x0400C2D5 RID: 49877
				public static LocString NAME = "Use Direction: {Direction}";

				// Token: 0x0400C2D6 RID: 49878
				public static LocString TOOLTIP = "Duplicants will only use this building when walking by it\n\nCurrently allowed direction: <b>{Direction}</b>";

				// Token: 0x0200378E RID: 14222
				public static class DIRECTIONS
				{
					// Token: 0x0400DCD5 RID: 56533
					public static LocString LEFT = "Left";

					// Token: 0x0400DCD6 RID: 56534
					public static LocString RIGHT = "Right";

					// Token: 0x0400DCD7 RID: 56535
					public static LocString BOTH = "Both";
				}
			}

			// Token: 0x02002CFE RID: 11518
			public class WATTSONGAMEOVER
			{
				// Token: 0x0400C2D7 RID: 49879
				public static LocString NAME = "Colony Lost";

				// Token: 0x0400C2D8 RID: 49880
				public static LocString TOOLTIP = "All Duplicants are dead or incapacitated";
			}

			// Token: 0x02002CFF RID: 11519
			public class INVALIDBUILDINGLOCATION
			{
				// Token: 0x0400C2D9 RID: 49881
				public static LocString NAME = "Invalid Building Location";

				// Token: 0x0400C2DA RID: 49882
				public static LocString TOOLTIP = "Cannot construct a building in this location";
			}

			// Token: 0x02002D00 RID: 11520
			public class LIQUIDVENTOBSTRUCTED
			{
				// Token: 0x0400C2DB RID: 49883
				public static LocString NAME = "Liquid Vent Obstructed";

				// Token: 0x0400C2DC RID: 49884
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" has been obstructed and is preventing ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" flow to this vent"
				});
			}

			// Token: 0x02002D01 RID: 11521
			public class LIQUIDVENTOVERPRESSURE
			{
				// Token: 0x0400C2DD RID: 49885
				public static LocString NAME = "Liquid Vent Overpressure";

				// Token: 0x0400C2DE RID: 49886
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" pressure in this area is preventing further ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" emission\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x02002D02 RID: 11522
			public class MANUALLYCONTROLLED
			{
				// Token: 0x0400C2DF RID: 49887
				public static LocString NAME = "Manually Controlled";

				// Token: 0x0400C2E0 RID: 49888
				public static LocString TOOLTIP = "This Duplicant is under my control";
			}

			// Token: 0x02002D03 RID: 11523
			public class LIMITVALVELIMITREACHED
			{
				// Token: 0x0400C2E1 RID: 49889
				public static LocString NAME = "Limit Reached";

				// Token: 0x0400C2E2 RID: 49890
				public static LocString TOOLTIP = "No more Mass can be transferred";
			}

			// Token: 0x02002D04 RID: 11524
			public class LIMITVALVELIMITNOTREACHED
			{
				// Token: 0x0400C2E3 RID: 49891
				public static LocString NAME = "Amount remaining: {0}";

				// Token: 0x0400C2E4 RID: 49892
				public static LocString TOOLTIP = "This building will stop transferring Mass when the amount remaining reaches 0";
			}

			// Token: 0x02002D05 RID: 11525
			public class MATERIALSUNAVAILABLE
			{
				// Token: 0x0400C2E5 RID: 49893
				public static LocString NAME = "Insufficient Resources\n{ItemsRemaining}";

				// Token: 0x0400C2E6 RID: 49894
				public static LocString TOOLTIP = "Crucial materials for this building are beyond reach or unavailable";

				// Token: 0x0400C2E7 RID: 49895
				public static LocString NOTIFICATION_NAME = "Building lacks resources";

				// Token: 0x0400C2E8 RID: 49896
				public static LocString NOTIFICATION_TOOLTIP = "Crucial materials are unavailable or beyond reach for these buildings:";

				// Token: 0x0400C2E9 RID: 49897
				public static LocString LINE_ITEM_MASS = "• {0}: {1}";

				// Token: 0x0400C2EA RID: 49898
				public static LocString LINE_ITEM_UNITS = "• {0}";
			}

			// Token: 0x02002D06 RID: 11526
			public class MATERIALSUNAVAILABLEFORREFILL
			{
				// Token: 0x0400C2EB RID: 49899
				public static LocString NAME = "Resources Low\n{ItemsRemaining}";

				// Token: 0x0400C2EC RID: 49900
				public static LocString TOOLTIP = "This building will soon require materials that are unavailable";

				// Token: 0x0400C2ED RID: 49901
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x02002D07 RID: 11527
			public class MELTINGDOWN
			{
				// Token: 0x0400C2EE RID: 49902
				public static LocString NAME = "Breaking Down";

				// Token: 0x0400C2EF RID: 49903
				public static LocString TOOLTIP = "This building is collapsing";

				// Token: 0x0400C2F0 RID: 49904
				public static LocString NOTIFICATION_NAME = "Building breakdown";

				// Token: 0x0400C2F1 RID: 49905
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are collapsing:";
			}

			// Token: 0x02002D08 RID: 11528
			public class MISSINGFOUNDATION
			{
				// Token: 0x0400C2F2 RID: 49906
				public static LocString NAME = "Missing Tile";

				// Token: 0x0400C2F3 RID: 49907
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Build ",
					UI.PRE_KEYWORD,
					"Tile",
					UI.PST_KEYWORD,
					" beneath this building to regain function\n\nTile can be found in the ",
					UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
					" of the Build Menu"
				});
			}

			// Token: 0x02002D09 RID: 11529
			public class NEUTRONIUMUNMINABLE
			{
				// Token: 0x0400C2F4 RID: 49908
				public static LocString NAME = "Cannot Mine";

				// Token: 0x0400C2F5 RID: 49909
				public static LocString TOOLTIP = "This resource cannot be mined by Duplicant tools";
			}

			// Token: 0x02002D0A RID: 11530
			public class NEEDGASIN
			{
				// Token: 0x0400C2F6 RID: 49910
				public static LocString NAME = "No Gas Intake\n{GasRequired}";

				// Token: 0x0400C2F7 RID: 49911
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Gas Intake",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" connected"
				});

				// Token: 0x0400C2F8 RID: 49912
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x02002D0B RID: 11531
			public class NEEDGASOUT
			{
				// Token: 0x0400C2F9 RID: 49913
				public static LocString NAME = "No Gas Output";

				// Token: 0x0400C2FA RID: 49914
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Gas Output",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" connected"
				});
			}

			// Token: 0x02002D0C RID: 11532
			public class NEEDLIQUIDIN
			{
				// Token: 0x0400C2FB RID: 49915
				public static LocString NAME = "No Liquid Intake\n{LiquidRequired}";

				// Token: 0x0400C2FC RID: 49916
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Liquid Intake",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" connected"
				});

				// Token: 0x0400C2FD RID: 49917
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x02002D0D RID: 11533
			public class NEEDLIQUIDOUT
			{
				// Token: 0x0400C2FE RID: 49918
				public static LocString NAME = "No Liquid Output";

				// Token: 0x0400C2FF RID: 49919
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Liquid Output",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" connected"
				});
			}

			// Token: 0x02002D0E RID: 11534
			public class LIQUIDPIPEEMPTY
			{
				// Token: 0x0400C300 RID: 49920
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400C301 RID: 49921
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is no ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" in this pipe"
				});
			}

			// Token: 0x02002D0F RID: 11535
			public class LIQUIDPIPEOBSTRUCTED
			{
				// Token: 0x0400C302 RID: 49922
				public static LocString NAME = "Not Pumping";

				// Token: 0x0400C303 RID: 49923
				public static LocString TOOLTIP = "This pump is not active";
			}

			// Token: 0x02002D10 RID: 11536
			public class GASPIPEEMPTY
			{
				// Token: 0x0400C304 RID: 49924
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400C305 RID: 49925
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is no ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" in this pipe"
				});
			}

			// Token: 0x02002D11 RID: 11537
			public class GASPIPEOBSTRUCTED
			{
				// Token: 0x0400C306 RID: 49926
				public static LocString NAME = "Not Pumping";

				// Token: 0x0400C307 RID: 49927
				public static LocString TOOLTIP = "This pump is not active";
			}

			// Token: 0x02002D12 RID: 11538
			public class NEEDSOLIDIN
			{
				// Token: 0x0400C308 RID: 49928
				public static LocString NAME = "No Conveyor Loader";

				// Token: 0x0400C309 RID: 49929
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material cannot be fed onto this Conveyor system for transport\n\nEnter the ",
					UI.FormatAsBuildMenuTab("Shipping Tab", global::Action.Plan13),
					" of the Build Menu to build and connect a ",
					UI.PRE_KEYWORD,
					"Conveyor Loader",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002D13 RID: 11539
			public class NEEDSOLIDOUT
			{
				// Token: 0x0400C30A RID: 49930
				public static LocString NAME = "No Conveyor Receptacle";

				// Token: 0x0400C30B RID: 49931
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material cannot be offloaded from this Conveyor system and will backup the rails\n\nEnter the ",
					UI.FormatAsBuildMenuTab("Shipping Tab", global::Action.Plan13),
					" of the Build Menu to build and connect a ",
					UI.PRE_KEYWORD,
					"Conveyor Receptacle",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002D14 RID: 11540
			public class SOLIDPIPEOBSTRUCTED
			{
				// Token: 0x0400C30C RID: 49932
				public static LocString NAME = "Conveyor Rail Backup";

				// Token: 0x0400C30D RID: 49933
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Conveyor Rail",
					UI.PST_KEYWORD,
					" cannot carry anymore material\n\nRemove material from the ",
					UI.PRE_KEYWORD,
					"Conveyor Receptacle",
					UI.PST_KEYWORD,
					" to free space for more objects"
				});
			}

			// Token: 0x02002D15 RID: 11541
			public class NEEDPLANT
			{
				// Token: 0x0400C30E RID: 49934
				public static LocString NAME = "No Seeds";

				// Token: 0x0400C30F RID: 49935
				public static LocString TOOLTIP = "Uproot wild plants to obtain seeds";
			}

			// Token: 0x02002D16 RID: 11542
			public class NEEDSEED
			{
				// Token: 0x0400C310 RID: 49936
				public static LocString NAME = "No Seed Selected";

				// Token: 0x0400C311 RID: 49937
				public static LocString TOOLTIP = "Uproot wild plants to obtain seeds";
			}

			// Token: 0x02002D17 RID: 11543
			public class NEEDPOWER
			{
				// Token: 0x0400C312 RID: 49938
				public static LocString NAME = "No Power";

				// Token: 0x0400C313 RID: 49939
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"All connected ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" sources have lost charge"
				});
			}

			// Token: 0x02002D18 RID: 11544
			public class NOTENOUGHPOWER
			{
				// Token: 0x0400C314 RID: 49940
				public static LocString NAME = "Insufficient Power";

				// Token: 0x0400C315 RID: 49941
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building does not have enough stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" to run"
				});
			}

			// Token: 0x02002D19 RID: 11545
			public class POWERLOOPDETECTED
			{
				// Token: 0x0400C316 RID: 49942
				public static LocString NAME = "Power Loop Detected";

				// Token: 0x0400C317 RID: 49943
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Transformer's",
					UI.PST_KEYWORD,
					" ",
					UI.PRE_KEYWORD,
					"Power Output",
					UI.PST_KEYWORD,
					" has been connected back to its own ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002D1A RID: 11546
			public class NEEDRESOURCE
			{
				// Token: 0x0400C318 RID: 49944
				public static LocString NAME = "Resource Required";

				// Token: 0x0400C319 RID: 49945
				public static LocString TOOLTIP = "This building is missing required materials";
			}

			// Token: 0x02002D1B RID: 11547
			public class NEWDUPLICANTSAVAILABLE
			{
				// Token: 0x0400C31A RID: 49946
				public static LocString NAME = "Printables Available";

				// Token: 0x0400C31B RID: 49947
				public static LocString TOOLTIP = "I am ready to print a new colony member or care package";

				// Token: 0x0400C31C RID: 49948
				public static LocString NOTIFICATION_NAME = "New Printables are available";

				// Token: 0x0400C31D RID: 49949
				public static LocString NOTIFICATION_TOOLTIP = "The Printing Pod " + UI.FormatAsHotKey(global::Action.Plan1) + " is ready to print a new Duplicant or care package.\nI'll need to select a blueprint:";
			}

			// Token: 0x02002D1C RID: 11548
			public class NOAPPLICABLERESEARCHSELECTED
			{
				// Token: 0x0400C31E RID: 49950
				public static LocString NAME = "Inapplicable Research";

				// Token: 0x0400C31F RID: 49951
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building cannot produce the correct ",
					UI.PRE_KEYWORD,
					"Research Type",
					UI.PST_KEYWORD,
					" for the current ",
					UI.FormatAsLink("Research Focus", "TECH")
				});

				// Token: 0x0400C320 RID: 49952
				public static LocString NOTIFICATION_NAME = UI.FormatAsLink("Research Center", "ADVANCEDRESEARCHCENTER") + " idle";

				// Token: 0x0400C321 RID: 49953
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These buildings cannot produce the correct ",
					UI.PRE_KEYWORD,
					"Research Type",
					UI.PST_KEYWORD,
					" for the selected ",
					UI.FormatAsLink("Research Focus", "TECH"),
					":"
				});
			}

			// Token: 0x02002D1D RID: 11549
			public class NOAPPLICABLEANALYSISSELECTED
			{
				// Token: 0x0400C322 RID: 49954
				public static LocString NAME = "No Analysis Focus Selected";

				// Token: 0x0400C323 RID: 49955
				public static LocString TOOLTIP = "Select an unknown destination from the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to begin analysis";

				// Token: 0x0400C324 RID: 49956
				public static LocString NOTIFICATION_NAME = UI.FormatAsLink("Telescope", "TELESCOPE") + " idle";

				// Token: 0x0400C325 RID: 49957
				public static LocString NOTIFICATION_TOOLTIP = "These buildings require an analysis focus:";
			}

			// Token: 0x02002D1E RID: 11550
			public class NOAVAILABLESEED
			{
				// Token: 0x0400C326 RID: 49958
				public static LocString NAME = "No Seed Available";

				// Token: 0x0400C327 RID: 49959
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The selected ",
					UI.PRE_KEYWORD,
					"Seed",
					UI.PST_KEYWORD,
					" is not available"
				});
			}

			// Token: 0x02002D1F RID: 11551
			public class NOSTORAGEFILTERSET
			{
				// Token: 0x0400C328 RID: 49960
				public static LocString NAME = "Filters Not Designated";

				// Token: 0x0400C329 RID: 49961
				public static LocString TOOLTIP = "No resources types are marked for storage in this building";
			}

			// Token: 0x02002D20 RID: 11552
			public class NOSUITMARKER
			{
				// Token: 0x0400C32A RID: 49962
				public static LocString NAME = "No Checkpoint";

				// Token: 0x0400C32B RID: 49963
				public static LocString TOOLTIP = "Docks must be placed beside a " + BUILDINGS.PREFABS.CHECKPOINT.NAME + ", opposite the side the checkpoint faces";
			}

			// Token: 0x02002D21 RID: 11553
			public class SUITMARKERWRONGSIDE
			{
				// Token: 0x0400C32C RID: 49964
				public static LocString NAME = "Invalid Checkpoint";

				// Token: 0x0400C32D RID: 49965
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has been built on the wrong side of a ",
					BUILDINGS.PREFABS.CHECKPOINT.NAME,
					"\n\nDocks must be placed beside a ",
					BUILDINGS.PREFABS.CHECKPOINT.NAME,
					", opposite the side the checkpoint faces"
				});
			}

			// Token: 0x02002D22 RID: 11554
			public class NOFILTERELEMENTSELECTED
			{
				// Token: 0x0400C32E RID: 49966
				public static LocString NAME = "No Filter Selected";

				// Token: 0x0400C32F RID: 49967
				public static LocString TOOLTIP = "Select a resource to filter";
			}

			// Token: 0x02002D23 RID: 11555
			public class NOLUREELEMENTSELECTED
			{
				// Token: 0x0400C330 RID: 49968
				public static LocString NAME = "No Bait Selected";

				// Token: 0x0400C331 RID: 49969
				public static LocString TOOLTIP = "Select a resource to use as bait";
			}

			// Token: 0x02002D24 RID: 11556
			public class NOFISHABLEWATERBELOW
			{
				// Token: 0x0400C332 RID: 49970
				public static LocString NAME = "No Fishable Water";

				// Token: 0x0400C333 RID: 49971
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There are no edible ",
					UI.PRE_KEYWORD,
					"Fish",
					UI.PST_KEYWORD,
					" beneath this structure"
				});
			}

			// Token: 0x02002D25 RID: 11557
			public class NOPOWERCONSUMERS
			{
				// Token: 0x0400C334 RID: 49972
				public static LocString NAME = "No Power Consumers";

				// Token: 0x0400C335 RID: 49973
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"No buildings are connected to this ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" source"
				});
			}

			// Token: 0x02002D26 RID: 11558
			public class NOWIRECONNECTED
			{
				// Token: 0x0400C336 RID: 49974
				public static LocString NAME = "No Power Wire Connected";

				// Token: 0x0400C337 RID: 49975
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has not been connected to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" grid"
				});
			}

			// Token: 0x02002D27 RID: 11559
			public class PENDINGDECONSTRUCTION
			{
				// Token: 0x0400C338 RID: 49976
				public static LocString NAME = "Deconstruction Errand";

				// Token: 0x0400C339 RID: 49977
				public static LocString TOOLTIP = "Building will be deconstructed once a Duplicant is available";
			}

			// Token: 0x02002D28 RID: 11560
			public class PENDINGDEMOLITION
			{
				// Token: 0x0400C33A RID: 49978
				public static LocString NAME = "Demolition Errand";

				// Token: 0x0400C33B RID: 49979
				public static LocString TOOLTIP = "Object will be permanently demolished once a Duplicant is available";
			}

			// Token: 0x02002D29 RID: 11561
			public class PENDINGFISH
			{
				// Token: 0x0400C33C RID: 49980
				public static LocString NAME = "Fishing Errand";

				// Token: 0x0400C33D RID: 49981
				public static LocString TOOLTIP = "Spot will be fished once a Duplicant is available";
			}

			// Token: 0x02002D2A RID: 11562
			public class PENDINGHARVEST
			{
				// Token: 0x0400C33E RID: 49982
				public static LocString NAME = "Harvest Errand";

				// Token: 0x0400C33F RID: 49983
				public static LocString TOOLTIP = "Plant will be harvested once a Duplicant is available";
			}

			// Token: 0x02002D2B RID: 11563
			public class PENDINGUPROOT
			{
				// Token: 0x0400C340 RID: 49984
				public static LocString NAME = "Uproot Errand";

				// Token: 0x0400C341 RID: 49985
				public static LocString TOOLTIP = "Plant will be uprooted once a Duplicant is available";
			}

			// Token: 0x02002D2C RID: 11564
			public class PENDINGREPAIR
			{
				// Token: 0x0400C342 RID: 49986
				public static LocString NAME = "Repair Errand";

				// Token: 0x0400C343 RID: 49987
				public static LocString TOOLTIP = "Building will be repaired once a Duplicant is available\nReceived damage from {DamageInfo}";
			}

			// Token: 0x02002D2D RID: 11565
			public class PENDINGSWITCHTOGGLE
			{
				// Token: 0x0400C344 RID: 49988
				public static LocString NAME = "Settings Errand";

				// Token: 0x0400C345 RID: 49989
				public static LocString TOOLTIP = "Settings will be changed once a Duplicant is available";
			}

			// Token: 0x02002D2E RID: 11566
			public class PENDINGWORK
			{
				// Token: 0x0400C346 RID: 49990
				public static LocString NAME = "Work Errand";

				// Token: 0x0400C347 RID: 49991
				public static LocString TOOLTIP = "Building will be operated once a Duplicant is available";
			}

			// Token: 0x02002D2F RID: 11567
			public class POWERBUTTONOFF
			{
				// Token: 0x0400C348 RID: 49992
				public static LocString NAME = "Function Suspended";

				// Token: 0x0400C349 RID: 49993
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has been toggled off\nPress ",
					UI.PRE_KEYWORD,
					"Enable Building",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ToggleEnabled),
					" to resume its use"
				});
			}

			// Token: 0x02002D30 RID: 11568
			public class PUMPINGSTATION
			{
				// Token: 0x0400C34A RID: 49994
				public static LocString NAME = "Liquid Available: {Liquids}";

				// Token: 0x0400C34B RID: 49995
				public static LocString TOOLTIP = "This pumping station has access to: {Liquids}";
			}

			// Token: 0x02002D31 RID: 11569
			public class PRESSUREOK
			{
				// Token: 0x0400C34C RID: 49996
				public static LocString NAME = "Max Gas Pressure";

				// Token: 0x0400C34D RID: 49997
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ambient ",
					UI.PRE_KEYWORD,
					"Gas Pressure",
					UI.PST_KEYWORD,
					" is preventing this building from emitting gas\n\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x02002D32 RID: 11570
			public class UNDERPRESSURE
			{
				// Token: 0x0400C34E RID: 49998
				public static LocString NAME = "Low Air Pressure";

				// Token: 0x0400C34F RID: 49999
				public static LocString TOOLTIP = "A minimum atmospheric pressure of <b>{TargetPressure}</b> is needed for this building to operate";
			}

			// Token: 0x02002D33 RID: 11571
			public class STORAGELOCKER
			{
				// Token: 0x0400C350 RID: 50000
				public static LocString NAME = "Storing: {Stored} / {Capacity} {Units}";

				// Token: 0x0400C351 RID: 50001
				public static LocString TOOLTIP = "This container is storing <b>{Stored}{Units}</b> of a maximum <b>{Capacity}{Units}</b>";
			}

			// Token: 0x02002D34 RID: 11572
			public class CRITTERCAPACITY
			{
				// Token: 0x0400C352 RID: 50002
				public static LocString NAME = "Storing: {Stored} / {Capacity} Critters";

				// Token: 0x0400C353 RID: 50003
				public static LocString TOOLTIP = "This container is storing <b>{Stored} {StoredUnits}</b> of a maximum <b>{Capacity} {CapacityUnits}</b>";

				// Token: 0x0400C354 RID: 50004
				public static LocString UNITS = "Critters";

				// Token: 0x0400C355 RID: 50005
				public static LocString UNIT = "Critter";
			}

			// Token: 0x02002D35 RID: 11573
			public class SKILL_POINTS_AVAILABLE
			{
				// Token: 0x0400C356 RID: 50006
				public static LocString NAME = "Skill Points Available";

				// Token: 0x0400C357 RID: 50007
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant has ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					" available"
				});
			}

			// Token: 0x02002D36 RID: 11574
			public class TANNINGLIGHTSUFFICIENT
			{
				// Token: 0x0400C358 RID: 50008
				public static LocString NAME = "Tanning Light Available";

				// Token: 0x0400C359 RID: 50009
				public static LocString TOOLTIP = "There is sufficient " + UI.FormatAsLink("Light", "LIGHT") + " here to create pleasing skin crisping";
			}

			// Token: 0x02002D37 RID: 11575
			public class TANNINGLIGHTINSUFFICIENT
			{
				// Token: 0x0400C35A RID: 50010
				public static LocString NAME = "Insufficient Tanning Light";

				// Token: 0x0400C35B RID: 50011
				public static LocString TOOLTIP = "The " + UI.FormatAsLink("Light", "LIGHT") + " here is not bright enough for that Sunny Day feeling";
			}

			// Token: 0x02002D38 RID: 11576
			public class UNASSIGNED
			{
				// Token: 0x0400C35C RID: 50012
				public static LocString NAME = "Unassigned";

				// Token: 0x0400C35D RID: 50013
				public static LocString TOOLTIP = "Assign a Duplicant to use this amenity";
			}

			// Token: 0x02002D39 RID: 11577
			public class UNDERCONSTRUCTION
			{
				// Token: 0x0400C35E RID: 50014
				public static LocString NAME = "Under Construction";

				// Token: 0x0400C35F RID: 50015
				public static LocString TOOLTIP = "This building is currently being built";
			}

			// Token: 0x02002D3A RID: 11578
			public class UNDERCONSTRUCTIONNOWORKER
			{
				// Token: 0x0400C360 RID: 50016
				public static LocString NAME = "Construction Errand";

				// Token: 0x0400C361 RID: 50017
				public static LocString TOOLTIP = "Building will be constructed once a Duplicant is available";
			}

			// Token: 0x02002D3B RID: 11579
			public class WAITINGFORMATERIALS
			{
				// Token: 0x0400C362 RID: 50018
				public static LocString NAME = "Awaiting Delivery\n{ItemsRemaining}";

				// Token: 0x0400C363 RID: 50019
				public static LocString TOOLTIP = "These materials will be delivered once a Duplicant is available";

				// Token: 0x0400C364 RID: 50020
				public static LocString LINE_ITEM_MASS = "• {0}: {1}";

				// Token: 0x0400C365 RID: 50021
				public static LocString LINE_ITEM_UNITS = "• {0}";
			}

			// Token: 0x02002D3C RID: 11580
			public class WAITINGFORRADIATION
			{
				// Token: 0x0400C366 RID: 50022
				public static LocString NAME = "Awaiting Radbolts";

				// Token: 0x0400C367 RID: 50023
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires Radbolts to function\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay"),
					" ",
					UI.FormatAsHotKey(global::Action.Overlay15),
					" to view this building's radiation port"
				});
			}

			// Token: 0x02002D3D RID: 11581
			public class WAITINGFORREPAIRMATERIALS
			{
				// Token: 0x0400C368 RID: 50024
				public static LocString NAME = "Awaiting Repair Delivery\n{ItemsRemaining}\n";

				// Token: 0x0400C369 RID: 50025
				public static LocString TOOLTIP = "These materials must be delivered before this building can be repaired";

				// Token: 0x0400C36A RID: 50026
				public static LocString LINE_ITEM = string.Concat(new string[]
				{
					"• ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					": <b>{1}</b>"
				});
			}

			// Token: 0x02002D3E RID: 11582
			public class MISSINGGANTRY
			{
				// Token: 0x0400C36B RID: 50027
				public static LocString NAME = "Missing Gantry";

				// Token: 0x0400C36C RID: 50028
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.FormatAsLink("Gantry", "GANTRY"),
					" must be built below ",
					UI.FormatAsLink("Command Capsules", "COMMANDMODULE"),
					" and ",
					UI.FormatAsLink("Sight-Seeing Modules", "TOURISTMODULE"),
					" for Duplicant access"
				});
			}

			// Token: 0x02002D3F RID: 11583
			public class DISEMBARKINGDUPLICANT
			{
				// Token: 0x0400C36D RID: 50029
				public static LocString NAME = "Waiting To Disembark";

				// Token: 0x0400C36E RID: 50030
				public static LocString TOOLTIP = "The Duplicant inside this rocket can't come out until the " + UI.FormatAsLink("Gantry", "GANTRY") + " is extended";
			}

			// Token: 0x02002D40 RID: 11584
			public class REACTORMELTDOWN
			{
				// Token: 0x0400C36F RID: 50031
				public static LocString NAME = "Reactor Meltdown";

				// Token: 0x0400C370 RID: 50032
				public static LocString TOOLTIP = "This reactor is spilling dangerous radioactive waste and cannot be stopped";
			}

			// Token: 0x02002D41 RID: 11585
			public class ROCKETNAME
			{
				// Token: 0x0400C371 RID: 50033
				public static LocString NAME = "Parent Rocket: {0}";

				// Token: 0x0400C372 RID: 50034
				public static LocString TOOLTIP = "This module belongs to the rocket: " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D42 RID: 11586
			public class HASGANTRY
			{
				// Token: 0x0400C373 RID: 50035
				public static LocString NAME = "Has Gantry";

				// Token: 0x0400C374 RID: 50036
				public static LocString TOOLTIP = "Duplicants may now enter this section of the rocket";
			}

			// Token: 0x02002D43 RID: 11587
			public class NORMAL
			{
				// Token: 0x0400C375 RID: 50037
				public static LocString NAME = "Normal";

				// Token: 0x0400C376 RID: 50038
				public static LocString TOOLTIP = "Nothing out of the ordinary here";
			}

			// Token: 0x02002D44 RID: 11588
			public class MANUALGENERATORCHARGINGUP
			{
				// Token: 0x0400C377 RID: 50039
				public static LocString NAME = "Charging Up";

				// Token: 0x0400C378 RID: 50040
				public static LocString TOOLTIP = "This power source is being charged";
			}

			// Token: 0x02002D45 RID: 11589
			public class MANUALGENERATORRELEASINGENERGY
			{
				// Token: 0x0400C379 RID: 50041
				public static LocString NAME = "Powering";

				// Token: 0x0400C37A RID: 50042
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This generator is supplying energy to ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumers"
				});
			}

			// Token: 0x02002D46 RID: 11590
			public class GENERATOROFFLINE
			{
				// Token: 0x0400C37B RID: 50043
				public static LocString NAME = "Generator Idle";

				// Token: 0x0400C37C RID: 50044
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" source is idle"
				});
			}

			// Token: 0x02002D47 RID: 11591
			public class PIPE
			{
				// Token: 0x0400C37D RID: 50045
				public static LocString NAME = "Contents: {Contents}";

				// Token: 0x0400C37E RID: 50046
				public static LocString TOOLTIP = "This pipe is delivering {Contents}";
			}

			// Token: 0x02002D48 RID: 11592
			public class CONVEYOR
			{
				// Token: 0x0400C37F RID: 50047
				public static LocString NAME = "Contents: {Contents}";

				// Token: 0x0400C380 RID: 50048
				public static LocString TOOLTIP = "This conveyor is delivering {Contents}";
			}

			// Token: 0x02002D49 RID: 11593
			public class FABRICATORIDLE
			{
				// Token: 0x0400C381 RID: 50049
				public static LocString NAME = "No Fabrications Queued";

				// Token: 0x0400C382 RID: 50050
				public static LocString TOOLTIP = "Select a recipe to begin fabrication";
			}

			// Token: 0x02002D4A RID: 11594
			public class FABRICATOREMPTY
			{
				// Token: 0x0400C383 RID: 50051
				public static LocString NAME = "Waiting For Materials";

				// Token: 0x0400C384 RID: 50052
				public static LocString TOOLTIP = "Fabrication will begin once materials have been delivered";
			}

			// Token: 0x02002D4B RID: 11595
			public class FABRICATORLACKSHEP
			{
				// Token: 0x0400C385 RID: 50053
				public static LocString NAME = "Waiting For Radbolts ({CurrentHEP}/{HEPRequired})";

				// Token: 0x0400C386 RID: 50054
				public static LocString TOOLTIP = "A queued recipe requires more Radbolts than are currently stored.\n\nCurrently stored: {CurrentHEP}\nRequired for recipe: {HEPRequired}";
			}

			// Token: 0x02002D4C RID: 11596
			public class TOILET
			{
				// Token: 0x0400C387 RID: 50055
				public static LocString NAME = "{FlushesRemaining} \"Visits\" Remaining";

				// Token: 0x0400C388 RID: 50056
				public static LocString TOOLTIP = "{FlushesRemaining} more Duplicants can use this amenity before it requires maintenance";
			}

			// Token: 0x02002D4D RID: 11597
			public class TOILETNEEDSEMPTYING
			{
				// Token: 0x0400C389 RID: 50057
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400C38A RID: 50058
				public static LocString TOOLTIP = "This amenity cannot be used while full\n\nEmptying it will produce " + UI.FormatAsLink("Polluted Dirt", "TOXICSAND");
			}

			// Token: 0x02002D4E RID: 11598
			public class DESALINATORNEEDSEMPTYING
			{
				// Token: 0x0400C38B RID: 50059
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400C38C RID: 50060
				public static LocString TOOLTIP = "This building needs to be emptied of " + UI.FormatAsLink("Salt", "SALT") + " to resume function";
			}

			// Token: 0x02002D4F RID: 11599
			public class MILKSEPARATORNEEDSEMPTYING
			{
				// Token: 0x0400C38D RID: 50061
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400C38E RID: 50062
				public static LocString TOOLTIP = "This building needs to be emptied of " + UI.FormatAsLink("Brackwax", "MILKFAT") + " to resume function";
			}

			// Token: 0x02002D50 RID: 11600
			public class HABITATNEEDSEMPTYING
			{
				// Token: 0x0400C38F RID: 50063
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400C390 RID: 50064
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.FormatAsLink("Algae Terrarium", "ALGAEHABITAT"),
					" needs to be emptied of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"\n\n",
					UI.FormatAsLink("Bottle Emptiers", "BOTTLEEMPTIER"),
					" can be used to transport and dispose of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" in designated areas"
				});
			}

			// Token: 0x02002D51 RID: 11601
			public class UNUSABLE
			{
				// Token: 0x0400C391 RID: 50065
				public static LocString NAME = "Out of Order";

				// Token: 0x0400C392 RID: 50066
				public static LocString TOOLTIP = "This amenity requires maintenance";
			}

			// Token: 0x02002D52 RID: 11602
			public class NORESEARCHSELECTED
			{
				// Token: 0x0400C393 RID: 50067
				public static LocString NAME = "No Research Focus Selected";

				// Token: 0x0400C394 RID: 50068
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Open the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" to select a new ",
					UI.FormatAsLink("Research", "TECH"),
					" project"
				});

				// Token: 0x0400C395 RID: 50069
				public static LocString NOTIFICATION_NAME = "No " + UI.FormatAsLink("Research Focus", "TECH") + " selected";

				// Token: 0x0400C396 RID: 50070
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"Open the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" to select a new ",
					UI.FormatAsLink("Research", "TECH"),
					" project"
				});
			}

			// Token: 0x02002D53 RID: 11603
			public class NORESEARCHORDESTINATIONSELECTED
			{
				// Token: 0x0400C397 RID: 50071
				public static LocString NAME = "No Research Focus or Starmap Destination Selected";

				// Token: 0x0400C398 RID: 50072
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Select a ",
					UI.FormatAsLink("Research", "TECH"),
					" project in the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" or a Destination in the ",
					UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap)
				});

				// Token: 0x0400C399 RID: 50073
				public static LocString NOTIFICATION_NAME = "No " + UI.FormatAsLink("Research Focus", "TECH") + " or Starmap destination selected";

				// Token: 0x0400C39A RID: 50074
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"Select a ",
					UI.FormatAsLink("Research", "TECH"),
					" project in the ",
					UI.FormatAsManagementMenu("Research Tree", "[R]"),
					" or a Destination in the ",
					UI.FormatAsManagementMenu("Starmap", "[Z]")
				});
			}

			// Token: 0x02002D54 RID: 11604
			public class RESEARCHING
			{
				// Token: 0x0400C39B RID: 50075
				public static LocString NAME = "Current " + UI.FormatAsLink("Research", "TECH") + ": {Tech}";

				// Token: 0x0400C39C RID: 50076
				public static LocString TOOLTIP = "Research produced at this station will be invested in {Tech}";
			}

			// Token: 0x02002D55 RID: 11605
			public class TINKERING
			{
				// Token: 0x0400C39D RID: 50077
				public static LocString NAME = "Tinkering: {0}";

				// Token: 0x0400C39E RID: 50078
				public static LocString TOOLTIP = "This Duplicant is creating {0} to use somewhere else";
			}

			// Token: 0x02002D56 RID: 11606
			public class VALVE
			{
				// Token: 0x0400C39F RID: 50079
				public static LocString NAME = "Max Flow Rate: {MaxFlow}";

				// Token: 0x0400C3A0 RID: 50080
				public static LocString TOOLTIP = "This valve is allowing flow at a volume of <b>{MaxFlow}</b>";
			}

			// Token: 0x02002D57 RID: 11607
			public class VALVEREQUEST
			{
				// Token: 0x0400C3A1 RID: 50081
				public static LocString NAME = "Requested Flow Rate: {QueuedMaxFlow}";

				// Token: 0x0400C3A2 RID: 50082
				public static LocString TOOLTIP = "Waiting for a Duplicant to adjust flow rate";
			}

			// Token: 0x02002D58 RID: 11608
			public class EMITTINGLIGHT
			{
				// Token: 0x0400C3A3 RID: 50083
				public static LocString NAME = "Emitting Light";

				// Token: 0x0400C3A4 RID: 50084
				public static LocString TOOLTIP = "Open the " + UI.FormatAsOverlay("Light Overlay", global::Action.Overlay5) + " to view this light's visibility radius";
			}

			// Token: 0x02002D59 RID: 11609
			public class KETTLEINSUFICIENTSOLIDS
			{
				// Token: 0x0400C3A5 RID: 50085
				public static LocString NAME = "Insufficient " + UI.FormatAsLink("Ice", "ICE");

				// Token: 0x0400C3A6 RID: 50086
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires a minimum of {0} ",
					UI.FormatAsLink("Ice", "ICE"),
					" in order to function\n\nDeliver more ",
					UI.FormatAsLink("Ice", "ICE"),
					" to operate this building"
				});
			}

			// Token: 0x02002D5A RID: 11610
			public class KETTLEINSUFICIENTFUEL
			{
				// Token: 0x0400C3A7 RID: 50087
				public static LocString NAME = "Insufficient " + UI.FormatAsLink("Wood", "WOODLOG");

				// Token: 0x0400C3A8 RID: 50088
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Colder ",
					UI.FormatAsLink("Ice", "ICE"),
					" increases the amount of ",
					UI.FormatAsLink("Wood", "WOODLOG"),
					" required for melting\n\nCurrent requirement: minimum {0} ",
					UI.FormatAsLink("Wood", "WOODLOG")
				});
			}

			// Token: 0x02002D5B RID: 11611
			public class KETTLEINSUFICIENTLIQUIDSPACE
			{
				// Token: 0x0400C3A9 RID: 50089
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400C3AA RID: 50090
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.FormatAsLink("Ice Liquefier", "ICEKETTLE"),
					" needs to be emptied of ",
					UI.FormatAsLink("Water", "WATER"),
					" in order to resume function\n\nIt requires at least {2} of storage space in order to function properly\n\nCurrently storing {0} of a maximum {1} ",
					UI.FormatAsLink("Water", "WATER")
				});
			}

			// Token: 0x02002D5C RID: 11612
			public class KETTLEMELTING
			{
				// Token: 0x0400C3AB RID: 50091
				public static LocString NAME = "Melting Ice";

				// Token: 0x0400C3AC RID: 50092
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is currently melting stored ",
					UI.FormatAsLink("Ice", "ICE"),
					" to produce ",
					UI.FormatAsLink("Water", "WATER"),
					"\n\n",
					UI.FormatAsLink("Water", "WATER"),
					" output temperature: {0}"
				});
			}

			// Token: 0x02002D5D RID: 11613
			public class RATIONBOXCONTENTS
			{
				// Token: 0x0400C3AD RID: 50093
				public static LocString NAME = "Storing: {Stored}";

				// Token: 0x0400C3AE RID: 50094
				public static LocString TOOLTIP = "This box contains <b>{Stored}</b> of " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D5E RID: 11614
			public class EMITTINGELEMENT
			{
				// Token: 0x0400C3AF RID: 50095
				public static LocString NAME = "Emitting {ElementType}: {FlowRate}";

				// Token: 0x0400C3B0 RID: 50096
				public static LocString TOOLTIP = "Producing {ElementType} at " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002D5F RID: 11615
			public class EMITTINGCO2
			{
				// Token: 0x0400C3B1 RID: 50097
				public static LocString NAME = "Emitting CO<sub>2</sub>: {FlowRate}";

				// Token: 0x0400C3B2 RID: 50098
				public static LocString TOOLTIP = "Producing " + ELEMENTS.CARBONDIOXIDE.NAME + " at " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002D60 RID: 11616
			public class EMITTINGOXYGENAVG
			{
				// Token: 0x0400C3B3 RID: 50099
				public static LocString NAME = "Emitting " + UI.FormatAsLink("Oxygen", "OXYGEN") + ": {FlowRate}";

				// Token: 0x0400C3B4 RID: 50100
				public static LocString TOOLTIP = "Producing " + ELEMENTS.OXYGEN.NAME + " at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002D61 RID: 11617
			public class EMITTINGGASAVG
			{
				// Token: 0x0400C3B5 RID: 50101
				public static LocString NAME = "Emitting {Element}: {FlowRate}";

				// Token: 0x0400C3B6 RID: 50102
				public static LocString TOOLTIP = "Producing {Element} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002D62 RID: 11618
			public class EMITTINGBLOCKEDHIGHPRESSURE
			{
				// Token: 0x0400C3B7 RID: 50103
				public static LocString NAME = "Not Emitting: Overpressure";

				// Token: 0x0400C3B8 RID: 50104
				public static LocString TOOLTIP = "Ambient pressure is too high for {Element} to be released";
			}

			// Token: 0x02002D63 RID: 11619
			public class EMITTINGBLOCKEDLOWTEMPERATURE
			{
				// Token: 0x0400C3B9 RID: 50105
				public static LocString NAME = "Not Emitting: Too Cold";

				// Token: 0x0400C3BA RID: 50106
				public static LocString TOOLTIP = "Temperature is too low for {Element} to be released";
			}

			// Token: 0x02002D64 RID: 11620
			public class PUMPINGLIQUIDORGAS
			{
				// Token: 0x0400C3BB RID: 50107
				public static LocString NAME = "Average Flow Rate: {FlowRate}";

				// Token: 0x0400C3BC RID: 50108
				public static LocString TOOLTIP = "This building is pumping an average volume of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002D65 RID: 11621
			public class WIRECIRCUITSTATUS
			{
				// Token: 0x0400C3BD RID: 50109
				public static LocString NAME = "Current Load: {CurrentLoadAndColor} / {MaxLoad}";

				// Token: 0x0400C3BE RID: 50110
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The current ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" load on this wire\n\nOverloading a wire will cause damage to the wire over time and cause it to break"
				});
			}

			// Token: 0x02002D66 RID: 11622
			public class WIREMAXWATTAGESTATUS
			{
				// Token: 0x0400C3BF RID: 50111
				public static LocString NAME = "Potential Load: {TotalPotentialLoadAndColor} / {MaxLoad}";

				// Token: 0x0400C3C0 RID: 50112
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"How much wattage this network will draw if all ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumers on the network become active at once"
				});
			}

			// Token: 0x02002D67 RID: 11623
			public class NOLIQUIDELEMENTTOPUMP
			{
				// Token: 0x0400C3C1 RID: 50113
				public static LocString NAME = "Pump Not In Liquid";

				// Token: 0x0400C3C2 RID: 50114
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This pump must be submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x02002D68 RID: 11624
			public class NOGASELEMENTTOPUMP
			{
				// Token: 0x0400C3C3 RID: 50115
				public static LocString NAME = "Pump Not In Gas";

				// Token: 0x0400C3C4 RID: 50116
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This pump must be submerged in ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x02002D69 RID: 11625
			public class INVALIDMASKSTATIONCONSUMPTIONSTATE
			{
				// Token: 0x0400C3C5 RID: 50117
				public static LocString NAME = "Station Not In Oxygen";

				// Token: 0x0400C3C6 RID: 50118
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This station must be submerged in ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x02002D6A RID: 11626
			public class PIPEMAYMELT
			{
				// Token: 0x0400C3C7 RID: 50119
				public static LocString NAME = "High Melt Risk";

				// Token: 0x0400C3C8 RID: 50120
				public static LocString TOOLTIP = "This pipe is in danger of melting at the current " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D6B RID: 11627
			public class ELEMENTEMITTEROUTPUT
			{
				// Token: 0x0400C3C9 RID: 50121
				public static LocString NAME = "Emitting {ElementTypes}: {FlowRate}";

				// Token: 0x0400C3CA RID: 50122
				public static LocString TOOLTIP = "This object is releasing {ElementTypes} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002D6C RID: 11628
			public class ELEMENTCONSUMER
			{
				// Token: 0x0400C3CB RID: 50123
				public static LocString NAME = "Consuming {ElementTypes}: {FlowRate}";

				// Token: 0x0400C3CC RID: 50124
				public static LocString TOOLTIP = "This object is utilizing ambient {ElementTypes} from the environment";
			}

			// Token: 0x02002D6D RID: 11629
			public class SPACECRAFTREADYTOLAND
			{
				// Token: 0x0400C3CD RID: 50125
				public static LocString NAME = "Spacecraft ready to land";

				// Token: 0x0400C3CE RID: 50126
				public static LocString TOOLTIP = "A spacecraft is ready to land";

				// Token: 0x0400C3CF RID: 50127
				public static LocString NOTIFICATION = "Space mission complete";

				// Token: 0x0400C3D0 RID: 50128
				public static LocString NOTIFICATION_TOOLTIP = "Spacecrafts have completed their missions";
			}

			// Token: 0x02002D6E RID: 11630
			public class CONSUMINGFROMSTORAGE
			{
				// Token: 0x0400C3D1 RID: 50129
				public static LocString NAME = "Consuming {ElementTypes}: {FlowRate}";

				// Token: 0x0400C3D2 RID: 50130
				public static LocString TOOLTIP = "This building is consuming {ElementTypes} from storage";
			}

			// Token: 0x02002D6F RID: 11631
			public class ELEMENTCONVERTEROUTPUT
			{
				// Token: 0x0400C3D3 RID: 50131
				public static LocString NAME = "Emitting {ElementTypes}: {FlowRate}";

				// Token: 0x0400C3D4 RID: 50132
				public static LocString TOOLTIP = "This building is releasing {ElementTypes} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002D70 RID: 11632
			public class ELEMENTCONVERTERINPUT
			{
				// Token: 0x0400C3D5 RID: 50133
				public static LocString NAME = "Using {ElementTypes}: {FlowRate}";

				// Token: 0x0400C3D6 RID: 50134
				public static LocString TOOLTIP = "This building is using {ElementTypes} from storage at a rate of " + UI.FormatAsNegativeRate("{FlowRate}");
			}

			// Token: 0x02002D71 RID: 11633
			public class AWAITINGCOMPOSTFLIP
			{
				// Token: 0x0400C3D7 RID: 50135
				public static LocString NAME = "Requires Flipping";

				// Token: 0x0400C3D8 RID: 50136
				public static LocString TOOLTIP = "Compost must be flipped periodically to produce " + UI.FormatAsLink("Dirt", "DIRT");
			}

			// Token: 0x02002D72 RID: 11634
			public class AWAITINGWASTE
			{
				// Token: 0x0400C3D9 RID: 50137
				public static LocString NAME = "Awaiting Compostables";

				// Token: 0x0400C3DA RID: 50138
				public static LocString TOOLTIP = "More waste material is required to begin the composting process";
			}

			// Token: 0x02002D73 RID: 11635
			public class BATTERIESSUFFICIENTLYFULL
			{
				// Token: 0x0400C3DB RID: 50139
				public static LocString NAME = "Batteries Sufficiently Full";

				// Token: 0x0400C3DC RID: 50140
				public static LocString TOOLTIP = "All batteries are above the refill threshold";
			}

			// Token: 0x02002D74 RID: 11636
			public class NEEDRESOURCEMASS
			{
				// Token: 0x0400C3DD RID: 50141
				public static LocString NAME = "Insufficient Resources\n{ResourcesRequired}";

				// Token: 0x0400C3DE RID: 50142
				public static LocString TOOLTIP = "The mass of material that was delivered to this building was too low\n\nDeliver more material to run this building";

				// Token: 0x0400C3DF RID: 50143
				public static LocString LINE_ITEM = "• <b>{0}</b>";
			}

			// Token: 0x02002D75 RID: 11637
			public class JOULESAVAILABLE
			{
				// Token: 0x0400C3E0 RID: 50144
				public static LocString NAME = "Power Available: {JoulesAvailable} / {JoulesCapacity}";

				// Token: 0x0400C3E1 RID: 50145
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"<b>{JoulesAvailable}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" available for use"
				});
			}

			// Token: 0x02002D76 RID: 11638
			public class WATTAGE
			{
				// Token: 0x0400C3E2 RID: 50146
				public static LocString NAME = "Wattage: {Wattage}";

				// Token: 0x0400C3E3 RID: 50147
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002D77 RID: 11639
			public class SOLARPANELWATTAGE
			{
				// Token: 0x0400C3E4 RID: 50148
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400C3E5 RID: 50149
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This panel is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002D78 RID: 11640
			public class MODULESOLARPANELWATTAGE
			{
				// Token: 0x0400C3E6 RID: 50150
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400C3E7 RID: 50151
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This panel is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002D79 RID: 11641
			public class WATTSON
			{
				// Token: 0x0400C3E8 RID: 50152
				public static LocString NAME = "Next Print: {TimeRemaining}";

				// Token: 0x0400C3E9 RID: 50153
				public static LocString TOOLTIP = "The Printing Pod can print out new Duplicants and useful resources over time.\nThe next print will be ready in <b>{TimeRemaining}</b>";

				// Token: 0x0400C3EA RID: 50154
				public static LocString UNAVAILABLE = "UNAVAILABLE";
			}

			// Token: 0x02002D7A RID: 11642
			public class FLUSHTOILET
			{
				// Token: 0x0400C3EB RID: 50155
				public static LocString NAME = "{toilet} Ready";

				// Token: 0x0400C3EC RID: 50156
				public static LocString TOOLTIP = "This bathroom is ready to receive visitors";
			}

			// Token: 0x02002D7B RID: 11643
			public class FLUSHTOILETINUSE
			{
				// Token: 0x0400C3ED RID: 50157
				public static LocString NAME = "{toilet} In Use";

				// Token: 0x0400C3EE RID: 50158
				public static LocString TOOLTIP = "This bathroom is occupied";
			}

			// Token: 0x02002D7C RID: 11644
			public class WIRECONNECTED
			{
				// Token: 0x0400C3EF RID: 50159
				public static LocString NAME = "Wire Connected";

				// Token: 0x0400C3F0 RID: 50160
				public static LocString TOOLTIP = "This wire is connected to a network";
			}

			// Token: 0x02002D7D RID: 11645
			public class WIRENOMINAL
			{
				// Token: 0x0400C3F1 RID: 50161
				public static LocString NAME = "Wire Nominal";

				// Token: 0x0400C3F2 RID: 50162
				public static LocString TOOLTIP = "This wire is able to handle the wattage it is receiving";
			}

			// Token: 0x02002D7E RID: 11646
			public class WIREDISCONNECTED
			{
				// Token: 0x0400C3F3 RID: 50163
				public static LocString NAME = "Wire Disconnected";

				// Token: 0x0400C3F4 RID: 50164
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This wire is not connecting a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumer to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" generator"
				});
			}

			// Token: 0x02002D7F RID: 11647
			public class COOLING
			{
				// Token: 0x0400C3F5 RID: 50165
				public static LocString NAME = "Cooling";

				// Token: 0x0400C3F6 RID: 50166
				public static LocString TOOLTIP = "This building is cooling the surrounding area";
			}

			// Token: 0x02002D80 RID: 11648
			public class COOLINGSTALLEDHOTENV
			{
				// Token: 0x0400C3F7 RID: 50167
				public static LocString NAME = "Gas Too Hot";

				// Token: 0x0400C3F8 RID: 50168
				public static LocString TOOLTIP = "Incoming pipe contents cannot be cooled more than <b>{2}</b> below the surrounding environment\n\nEnvironment: {0}\nCurrent Pipe Contents: {1}";
			}

			// Token: 0x02002D81 RID: 11649
			public class COOLINGSTALLEDCOLDGAS
			{
				// Token: 0x0400C3F9 RID: 50169
				public static LocString NAME = "Gas Too Cold";

				// Token: 0x0400C3FA RID: 50170
				public static LocString TOOLTIP = "This building cannot cool incoming pipe contents below <b>{0}</b>\n\nCurrent Pipe Contents: {0}";
			}

			// Token: 0x02002D82 RID: 11650
			public class COOLINGSTALLEDHOTLIQUID
			{
				// Token: 0x0400C3FB RID: 50171
				public static LocString NAME = "Liquid Too Hot";

				// Token: 0x0400C3FC RID: 50172
				public static LocString TOOLTIP = "Incoming pipe contents cannot be cooled more than <b>{2}</b> below the surrounding environment\n\nEnvironment: {0}\nCurrent Pipe Contents: {1}";
			}

			// Token: 0x02002D83 RID: 11651
			public class COOLINGSTALLEDCOLDLIQUID
			{
				// Token: 0x0400C3FD RID: 50173
				public static LocString NAME = "Liquid Too Cold";

				// Token: 0x0400C3FE RID: 50174
				public static LocString TOOLTIP = "This building cannot cool incoming pipe contents below <b>{0}</b>\n\nCurrent Pipe Contents: {0}";
			}

			// Token: 0x02002D84 RID: 11652
			public class CANNOTCOOLFURTHER
			{
				// Token: 0x0400C3FF RID: 50175
				public static LocString NAME = "Minimum Temperature Reached";

				// Token: 0x0400C400 RID: 50176
				public static LocString TOOLTIP = "This building cannot cool the surrounding environment below <b>{0}</b>";
			}

			// Token: 0x02002D85 RID: 11653
			public class HEATINGSTALLEDHOTENV
			{
				// Token: 0x0400C401 RID: 50177
				public static LocString NAME = "Target Temperature Reached";

				// Token: 0x0400C402 RID: 50178
				public static LocString TOOLTIP = "This building cannot heat the surrounding environment beyond <b>{0}</b>";
			}

			// Token: 0x02002D86 RID: 11654
			public class HEATINGSTALLEDLOWMASS_GAS
			{
				// Token: 0x0400C403 RID: 50179
				public static LocString NAME = "Insufficient Atmosphere";

				// Token: 0x0400C404 RID: 50180
				public static LocString TOOLTIP = "This building cannot operate in a vacuum";
			}

			// Token: 0x02002D87 RID: 11655
			public class HEATINGSTALLEDLOWMASS_LIQUID
			{
				// Token: 0x0400C405 RID: 50181
				public static LocString NAME = "Not Submerged In Liquid";

				// Token: 0x0400C406 RID: 50182
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to function"
				});
			}

			// Token: 0x02002D88 RID: 11656
			public class BUILDINGDISABLED
			{
				// Token: 0x0400C407 RID: 50183
				public static LocString NAME = "Building Disabled";

				// Token: 0x0400C408 RID: 50184
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Press ",
					UI.PRE_KEYWORD,
					"Enable Building",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ToggleEnabled),
					" to resume use"
				});
			}

			// Token: 0x02002D89 RID: 11657
			public class MISSINGREQUIREMENTS
			{
				// Token: 0x0400C409 RID: 50185
				public static LocString NAME = "Missing Requirements";

				// Token: 0x0400C40A RID: 50186
				public static LocString TOOLTIP = "There are some problems that need to be fixed before this building is operational";
			}

			// Token: 0x02002D8A RID: 11658
			public class GETTINGREADY
			{
				// Token: 0x0400C40B RID: 50187
				public static LocString NAME = "Getting Ready";

				// Token: 0x0400C40C RID: 50188
				public static LocString TOOLTIP = "This building will soon be ready to use";
			}

			// Token: 0x02002D8B RID: 11659
			public class WORKING
			{
				// Token: 0x0400C40D RID: 50189
				public static LocString NAME = "Nominal";

				// Token: 0x0400C40E RID: 50190
				public static LocString TOOLTIP = "This building is working as intended";
			}

			// Token: 0x02002D8C RID: 11660
			public class GRAVEEMPTY
			{
				// Token: 0x0400C40F RID: 50191
				public static LocString NAME = "Empty";

				// Token: 0x0400C410 RID: 50192
				public static LocString TOOLTIP = "This memorial honors no one.";
			}

			// Token: 0x02002D8D RID: 11661
			public class GRAVE
			{
				// Token: 0x0400C411 RID: 50193
				public static LocString NAME = "RIP {DeadDupe}";

				// Token: 0x0400C412 RID: 50194
				public static LocString TOOLTIP = "{Epitaph}";
			}

			// Token: 0x02002D8E RID: 11662
			public class AWAITINGARTING
			{
				// Token: 0x0400C413 RID: 50195
				public static LocString NAME = "Incomplete Artwork";

				// Token: 0x0400C414 RID: 50196
				public static LocString TOOLTIP = "This building requires a Duplicant's artistic touch";
			}

			// Token: 0x02002D8F RID: 11663
			public class LOOKINGUGLY
			{
				// Token: 0x0400C415 RID: 50197
				public static LocString NAME = "Crude";

				// Token: 0x0400C416 RID: 50198
				public static LocString TOOLTIP = "Honestly, Morbs could've done better";
			}

			// Token: 0x02002D90 RID: 11664
			public class LOOKINGOKAY
			{
				// Token: 0x0400C417 RID: 50199
				public static LocString NAME = "Quaint";

				// Token: 0x0400C418 RID: 50200
				public static LocString TOOLTIP = "Duplicants find this art piece quite charming";
			}

			// Token: 0x02002D91 RID: 11665
			public class LOOKINGGREAT
			{
				// Token: 0x0400C419 RID: 50201
				public static LocString NAME = "Masterpiece";

				// Token: 0x0400C41A RID: 50202
				public static LocString TOOLTIP = "This poignant piece stirs something deep within each Duplicant's soul";
			}

			// Token: 0x02002D92 RID: 11666
			public class EXPIRED
			{
				// Token: 0x0400C41B RID: 50203
				public static LocString NAME = "Depleted";

				// Token: 0x0400C41C RID: 50204
				public static LocString TOOLTIP = "This building has no more use";
			}

			// Token: 0x02002D93 RID: 11667
			public class COOLINGWATER
			{
				// Token: 0x0400C41D RID: 50205
				public static LocString NAME = "Cooling Water";

				// Token: 0x0400C41E RID: 50206
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is cooling ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" down to its freezing point"
				});
			}

			// Token: 0x02002D94 RID: 11668
			public class EXCAVATOR_BOMB
			{
				// Token: 0x0200378F RID: 14223
				public class UNARMED
				{
					// Token: 0x0400DCD8 RID: 56536
					public static LocString NAME = "Unarmed";

					// Token: 0x0400DCD9 RID: 56537
					public static LocString TOOLTIP = "This explosive is currently inactive";
				}

				// Token: 0x02003790 RID: 14224
				public class ARMED
				{
					// Token: 0x0400DCDA RID: 56538
					public static LocString NAME = "Armed";

					// Token: 0x0400DCDB RID: 56539
					public static LocString TOOLTIP = "Stand back, this baby's ready to blow!";
				}

				// Token: 0x02003791 RID: 14225
				public class COUNTDOWN
				{
					// Token: 0x0400DCDC RID: 56540
					public static LocString NAME = "Countdown: {0}";

					// Token: 0x0400DCDD RID: 56541
					public static LocString TOOLTIP = "<b>{0}</b> seconds until detonation";
				}

				// Token: 0x02003792 RID: 14226
				public class DUPE_DANGER
				{
					// Token: 0x0400DCDE RID: 56542
					public static LocString NAME = "Duplicant Preservation Override";

					// Token: 0x0400DCDF RID: 56543
					public static LocString TOOLTIP = "Explosive disabled due to close Duplicant proximity";
				}

				// Token: 0x02003793 RID: 14227
				public class EXPLODING
				{
					// Token: 0x0400DCE0 RID: 56544
					public static LocString NAME = "Exploding";

					// Token: 0x0400DCE1 RID: 56545
					public static LocString TOOLTIP = "Kaboom!";
				}
			}

			// Token: 0x02002D95 RID: 11669
			public class BURNER
			{
				// Token: 0x02003794 RID: 14228
				public class BURNING_FUEL
				{
					// Token: 0x0400DCE2 RID: 56546
					public static LocString NAME = "Consuming Fuel: {0}";

					// Token: 0x0400DCE3 RID: 56547
					public static LocString TOOLTIP = "<b>{0}</b> fuel remaining";
				}

				// Token: 0x02003795 RID: 14229
				public class HAS_FUEL
				{
					// Token: 0x0400DCE4 RID: 56548
					public static LocString NAME = "Fueled: {0}";

					// Token: 0x0400DCE5 RID: 56549
					public static LocString TOOLTIP = "<b>{0}</b> fuel remaining";
				}
			}

			// Token: 0x02002D96 RID: 11670
			public class CREATURE_REUSABLE_TRAP
			{
				// Token: 0x02003796 RID: 14230
				public class NEEDS_ARMING
				{
					// Token: 0x0400DCE6 RID: 56550
					public static LocString NAME = "Waiting to be Armed";

					// Token: 0x0400DCE7 RID: 56551
					public static LocString TOOLTIP = "Waiting for a Duplicant to arm this trap\n\nOnly Duplicants with the " + DUPLICANTS.ROLES.RANCHER.NAME + " skill can arm traps";
				}

				// Token: 0x02003797 RID: 14231
				public class READY
				{
					// Token: 0x0400DCE8 RID: 56552
					public static LocString NAME = "Armed";

					// Token: 0x0400DCE9 RID: 56553
					public static LocString TOOLTIP = "This trap has been armed and is ready to catch a " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
				}

				// Token: 0x02003798 RID: 14232
				public class SPRUNG
				{
					// Token: 0x0400DCEA RID: 56554
					public static LocString NAME = "Sprung";

					// Token: 0x0400DCEB RID: 56555
					public static LocString TOOLTIP = "This trap has caught a {0}!";
				}
			}

			// Token: 0x02002D97 RID: 11671
			public class CREATURE_TRAP
			{
				// Token: 0x02003799 RID: 14233
				public class NEEDSBAIT
				{
					// Token: 0x0400DCEC RID: 56556
					public static LocString NAME = "Needs Bait";

					// Token: 0x0400DCED RID: 56557
					public static LocString TOOLTIP = "This trap needs to be baited before it can be set";
				}

				// Token: 0x0200379A RID: 14234
				public class READY
				{
					// Token: 0x0400DCEE RID: 56558
					public static LocString NAME = "Set";

					// Token: 0x0400DCEF RID: 56559
					public static LocString TOOLTIP = "This trap has been set and is ready to catch a " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
				}

				// Token: 0x0200379B RID: 14235
				public class SPRUNG
				{
					// Token: 0x0400DCF0 RID: 56560
					public static LocString NAME = "Sprung";

					// Token: 0x0400DCF1 RID: 56561
					public static LocString TOOLTIP = "This trap has caught a {0}!";
				}
			}

			// Token: 0x02002D98 RID: 11672
			public class ACCESS_CONTROL
			{
				// Token: 0x0200379C RID: 14236
				public class ACTIVE
				{
					// Token: 0x0400DCF2 RID: 56562
					public static LocString NAME = "Access Restrictions";

					// Token: 0x0400DCF3 RID: 56563
					public static LocString TOOLTIP = "Some Duplicants are prohibited from passing through this door by the current " + UI.PRE_KEYWORD + "Access Permissions" + UI.PST_KEYWORD;
				}

				// Token: 0x0200379D RID: 14237
				public class OFFLINE
				{
					// Token: 0x0400DCF4 RID: 56564
					public static LocString NAME = "Access Control Offline";

					// Token: 0x0400DCF5 RID: 56565
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This door has granted Emergency ",
						UI.PRE_KEYWORD,
						"Access Permissions",
						UI.PST_KEYWORD,
						"\n\nAll Duplicants are permitted to pass through it until ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						" is restored"
					});
				}
			}

			// Token: 0x02002D99 RID: 11673
			public class REQUIRESSKILLPERK
			{
				// Token: 0x0400C41F RID: 50207
				public static LocString NAME = "Skill-Required Operation";

				// Token: 0x0400C420 RID: 50208
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Only Duplicants with one of the following ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					" can operate this building:\n{Skills}"
				});
			}

			// Token: 0x02002D9A RID: 11674
			public class DIGREQUIRESSKILLPERK
			{
				// Token: 0x0400C421 RID: 50209
				public static LocString NAME = "Skill-Required Dig";

				// Token: 0x0400C422 RID: 50210
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Only Duplicants with one of the following ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					" can mine this material:\n{Skills}"
				});
			}

			// Token: 0x02002D9B RID: 11675
			public class COLONYLACKSREQUIREDSKILLPERK
			{
				// Token: 0x0400C423 RID: 50211
				public static LocString NAME = "Colony Lacks {Skills} Skill";

				// Token: 0x0400C424 RID: 50212
				public static LocString TOOLTIP = "{Skills} Skill required to operate\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", global::Action.ManageSkills) + " to teach {Skills} to a Duplicant";
			}

			// Token: 0x02002D9C RID: 11676
			public class CLUSTERCOLONYLACKSREQUIREDSKILLPERK
			{
				// Token: 0x0400C425 RID: 50213
				public static LocString NAME = "Local Colony Lacks {Skills} Skill";

				// Token: 0x0400C426 RID: 50214
				public static LocString TOOLTIP = BUILDING.STATUSITEMS.COLONYLACKSREQUIREDSKILLPERK.TOOLTIP + ", or bring a Duplicant with the skill from another " + UI.CLUSTERMAP.PLANETOID;
			}

			// Token: 0x02002D9D RID: 11677
			public class WORKREQUIRESMINION
			{
				// Token: 0x0400C427 RID: 50215
				public static LocString NAME = "Duplicant Operation Required";

				// Token: 0x0400C428 RID: 50216
				public static LocString TOOLTIP = "A Duplicant must be present to complete this operation";
			}

			// Token: 0x02002D9E RID: 11678
			public class SWITCHSTATUSACTIVE
			{
				// Token: 0x0400C429 RID: 50217
				public static LocString NAME = "Active";

				// Token: 0x0400C42A RID: 50218
				public static LocString TOOLTIP = "This switch is currently toggled <b>On</b>";
			}

			// Token: 0x02002D9F RID: 11679
			public class SWITCHSTATUSINACTIVE
			{
				// Token: 0x0400C42B RID: 50219
				public static LocString NAME = "Inactive";

				// Token: 0x0400C42C RID: 50220
				public static LocString TOOLTIP = "This switch is currently toggled <b>Off</b>";
			}

			// Token: 0x02002DA0 RID: 11680
			public class LOGICSWITCHSTATUSACTIVE
			{
				// Token: 0x0400C42D RID: 50221
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400C42E RID: 50222
				public static LocString TOOLTIP = "This switch is currently sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);
			}

			// Token: 0x02002DA1 RID: 11681
			public class LOGICSWITCHSTATUSINACTIVE
			{
				// Token: 0x0400C42F RID: 50223
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400C430 RID: 50224
				public static LocString TOOLTIP = "This switch is currently sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002DA2 RID: 11682
			public class LOGICSENSORSTATUSACTIVE
			{
				// Token: 0x0400C431 RID: 50225
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400C432 RID: 50226
				public static LocString TOOLTIP = "This sensor is currently sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);
			}

			// Token: 0x02002DA3 RID: 11683
			public class LOGICSENSORSTATUSINACTIVE
			{
				// Token: 0x0400C433 RID: 50227
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400C434 RID: 50228
				public static LocString TOOLTIP = "This sensor is currently sending " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002DA4 RID: 11684
			public class PLAYERCONTROLLEDTOGGLESIDESCREEN
			{
				// Token: 0x0400C435 RID: 50229
				public static LocString NAME = "Pending Toggle on Unpause";

				// Token: 0x0400C436 RID: 50230
				public static LocString TOOLTIP = "This will be toggled when time is unpaused";
			}

			// Token: 0x02002DA5 RID: 11685
			public class FOOD_CONTAINERS_OUTSIDE_RANGE
			{
				// Token: 0x0400C437 RID: 50231
				public static LocString NAME = "Unreachable food";

				// Token: 0x0400C438 RID: 50232
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Recuperating Duplicants must have ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" available within <b>{0}</b> cells"
				});
			}

			// Token: 0x02002DA6 RID: 11686
			public class TOILETS_OUTSIDE_RANGE
			{
				// Token: 0x0400C439 RID: 50233
				public static LocString NAME = "Unreachable restroom";

				// Token: 0x0400C43A RID: 50234
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Recuperating Duplicants must have ",
					UI.PRE_KEYWORD,
					"Toilets",
					UI.PST_KEYWORD,
					" available within <b>{0}</b> cells"
				});
			}

			// Token: 0x02002DA7 RID: 11687
			public class BUILDING_DEPRECATED
			{
				// Token: 0x0400C43B RID: 50235
				public static LocString NAME = "Building Deprecated";

				// Token: 0x0400C43C RID: 50236
				public static LocString TOOLTIP = "This building is from an older version of the game and its use is not intended";
			}

			// Token: 0x02002DA8 RID: 11688
			public class TURBINE_BLOCKED_INPUT
			{
				// Token: 0x0400C43D RID: 50237
				public static LocString NAME = "All Inputs Blocked";

				// Token: 0x0400C43E RID: 50238
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This turbine's ",
					UI.PRE_KEYWORD,
					"Input Vents",
					UI.PST_KEYWORD,
					" are blocked, so it can't intake any ",
					ELEMENTS.STEAM.NAME,
					".\n\nThe ",
					UI.PRE_KEYWORD,
					"Input Vents",
					UI.PST_KEYWORD,
					" are located directly below the foundation ",
					UI.PRE_KEYWORD,
					"Tile",
					UI.PST_KEYWORD,
					" this building is resting on."
				});
			}

			// Token: 0x02002DA9 RID: 11689
			public class TURBINE_PARTIALLY_BLOCKED_INPUT
			{
				// Token: 0x0400C43F RID: 50239
				public static LocString NAME = "{Blocked}/{Total} Inputs Blocked";

				// Token: 0x0400C440 RID: 50240
				public static LocString TOOLTIP = "<b>{Blocked}</b> of this turbine's <b>{Total}</b> inputs have been blocked, resulting in reduced throughput";
			}

			// Token: 0x02002DAA RID: 11690
			public class TURBINE_TOO_HOT
			{
				// Token: 0x0400C441 RID: 50241
				public static LocString NAME = "Turbine Too Hot";

				// Token: 0x0400C442 RID: 50242
				public static LocString TOOLTIP = "This turbine must be below <b>{Overheat_Temperature}</b> to properly process {Src_Element} into {Dest_Element}";
			}

			// Token: 0x02002DAB RID: 11691
			public class TURBINE_BLOCKED_OUTPUT
			{
				// Token: 0x0400C443 RID: 50243
				public static LocString NAME = "Output Blocked";

				// Token: 0x0400C444 RID: 50244
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A blocked ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" has stopped this turbine from functioning"
				});
			}

			// Token: 0x02002DAC RID: 11692
			public class TURBINE_INSUFFICIENT_MASS
			{
				// Token: 0x0400C445 RID: 50245
				public static LocString NAME = "Not Enough {Src_Element}";

				// Token: 0x0400C446 RID: 50246
				public static LocString TOOLTIP = "The {Src_Element} present below this turbine must be at least <b>{Min_Mass}</b> in order to turn the turbine";
			}

			// Token: 0x02002DAD RID: 11693
			public class TURBINE_INSUFFICIENT_TEMPERATURE
			{
				// Token: 0x0400C447 RID: 50247
				public static LocString NAME = "{Src_Element} Temperature Below {Active_Temperature}";

				// Token: 0x0400C448 RID: 50248
				public static LocString TOOLTIP = "This turbine requires {Src_Element} that is a minimum of <b>{Active_Temperature}</b> in order to produce power";
			}

			// Token: 0x02002DAE RID: 11694
			public class TURBINE_ACTIVE_WATTAGE
			{
				// Token: 0x0400C449 RID: 50249
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400C44A RID: 50250
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This turbine is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nIt is running at <b>{Efficiency}</b> of full capacity\n\nIncrease {Src_Element} ",
					UI.PRE_KEYWORD,
					"Mass",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" to improve output"
				});
			}

			// Token: 0x02002DAF RID: 11695
			public class TURBINE_SPINNING_UP
			{
				// Token: 0x0400C44B RID: 50251
				public static LocString NAME = "Spinning Up";

				// Token: 0x0400C44C RID: 50252
				public static LocString TOOLTIP = "This turbine is currently spinning up\n\nSpinning up allows a turbine to continue running for a short period if the pressure it needs to run becomes unavailable";
			}

			// Token: 0x02002DB0 RID: 11696
			public class TURBINE_ACTIVE
			{
				// Token: 0x0400C44D RID: 50253
				public static LocString NAME = "Active";

				// Token: 0x0400C44E RID: 50254
				public static LocString TOOLTIP = "This turbine is running at <b>{0}RPM</b>";
			}

			// Token: 0x02002DB1 RID: 11697
			public class WELL_PRESSURIZING
			{
				// Token: 0x0400C44F RID: 50255
				public static LocString NAME = "Backpressure: {0}";

				// Token: 0x0400C450 RID: 50256
				public static LocString TOOLTIP = "Well pressure increases with each use and must be periodically relieved to prevent shutdown";
			}

			// Token: 0x02002DB2 RID: 11698
			public class WELL_OVERPRESSURE
			{
				// Token: 0x0400C451 RID: 50257
				public static LocString NAME = "Overpressure";

				// Token: 0x0400C452 RID: 50258
				public static LocString TOOLTIP = "This well can no longer function due to excessive backpressure";
			}

			// Token: 0x02002DB3 RID: 11699
			public class NOTINANYROOM
			{
				// Token: 0x0400C453 RID: 50259
				public static LocString NAME = "Outside of room";

				// Token: 0x0400C454 RID: 50260
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be built inside a ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" for full functionality\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x02002DB4 RID: 11700
			public class NOTINREQUIREDROOM
			{
				// Token: 0x0400C455 RID: 50261
				public static LocString NAME = "Outside of {0}";

				// Token: 0x0400C456 RID: 50262
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be built inside a {0} for full functionality\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x02002DB5 RID: 11701
			public class NOTINRECOMMENDEDROOM
			{
				// Token: 0x0400C457 RID: 50263
				public static LocString NAME = "Outside of {0}";

				// Token: 0x0400C458 RID: 50264
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"It is recommended to build this building inside a {0}\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x02002DB6 RID: 11702
			public class RELEASING_PRESSURE
			{
				// Token: 0x0400C459 RID: 50265
				public static LocString NAME = "Releasing Pressure";

				// Token: 0x0400C45A RID: 50266
				public static LocString TOOLTIP = "Pressure buildup is being safely released";
			}

			// Token: 0x02002DB7 RID: 11703
			public class LOGIC_FEEDBACK_LOOP
			{
				// Token: 0x0400C45B RID: 50267
				public static LocString NAME = "Feedback Loop";

				// Token: 0x0400C45C RID: 50268
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Feedback loops prevent automation grids from functioning\n\nFeedback loops occur when the ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" of an automated building connects back to its own ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD,
					" through the Automation grid"
				});
			}

			// Token: 0x02002DB8 RID: 11704
			public class ENOUGH_COOLANT
			{
				// Token: 0x0400C45D RID: 50269
				public static LocString NAME = "Awaiting Coolant";

				// Token: 0x0400C45E RID: 50270
				public static LocString TOOLTIP = "<b>{1}</b> of {0} must be present in storage to begin production";
			}

			// Token: 0x02002DB9 RID: 11705
			public class ENOUGH_FUEL
			{
				// Token: 0x0400C45F RID: 50271
				public static LocString NAME = "Awaiting Fuel";

				// Token: 0x0400C460 RID: 50272
				public static LocString TOOLTIP = "<b>{1}</b> of {0} must be present in storage to begin production";
			}

			// Token: 0x02002DBA RID: 11706
			public class LOGIC
			{
				// Token: 0x0400C461 RID: 50273
				public static LocString LOGIC_CONTROLLED_ENABLED = "Enabled by Automation Grid";

				// Token: 0x0400C462 RID: 50274
				public static LocString LOGIC_CONTROLLED_DISABLED = "Disabled by Automation Grid";
			}

			// Token: 0x02002DBB RID: 11707
			public class GANTRY
			{
				// Token: 0x0400C463 RID: 50275
				public static LocString AUTOMATION_CONTROL = "Automation Control: {0}";

				// Token: 0x0400C464 RID: 50276
				public static LocString MANUAL_CONTROL = "Manual Control: {0}";

				// Token: 0x0400C465 RID: 50277
				public static LocString EXTENDED = "Extended";

				// Token: 0x0400C466 RID: 50278
				public static LocString RETRACTED = "Retracted";
			}

			// Token: 0x02002DBC RID: 11708
			public class OBJECTDISPENSER
			{
				// Token: 0x0400C467 RID: 50279
				public static LocString AUTOMATION_CONTROL = "Automation Control: {0}";

				// Token: 0x0400C468 RID: 50280
				public static LocString MANUAL_CONTROL = "Manual Control: {0}";

				// Token: 0x0400C469 RID: 50281
				public static LocString OPENED = "Opened";

				// Token: 0x0400C46A RID: 50282
				public static LocString CLOSED = "Closed";
			}

			// Token: 0x02002DBD RID: 11709
			public class TOO_COLD
			{
				// Token: 0x0400C46B RID: 50283
				public static LocString NAME = "Too Cold";

				// Token: 0x0400C46C RID: 50284
				public static LocString TOOLTIP = "Either this building or its surrounding environment is too cold to operate";
			}

			// Token: 0x02002DBE RID: 11710
			public class CHECKPOINT
			{
				// Token: 0x0400C46D RID: 50285
				public static LocString LOGIC_CONTROLLED_OPEN = "Clearance: Permitted";

				// Token: 0x0400C46E RID: 50286
				public static LocString LOGIC_CONTROLLED_CLOSED = "Clearance: Not Permitted";

				// Token: 0x0400C46F RID: 50287
				public static LocString LOGIC_CONTROLLED_DISCONNECTED = "No Automation";

				// Token: 0x0200379E RID: 14238
				public class TOOLTIPS
				{
					// Token: 0x0400DCF6 RID: 56566
					public static LocString LOGIC_CONTROLLED_OPEN = "Automated Checkpoint is receiving a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ", preventing Duplicants from passing";

					// Token: 0x0400DCF7 RID: 56567
					public static LocString LOGIC_CONTROLLED_CLOSED = "Automated Checkpoint is receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ", allowing Duplicants to pass";

					// Token: 0x0400DCF8 RID: 56568
					public static LocString LOGIC_CONTROLLED_DISCONNECTED = string.Concat(new string[]
					{
						"This Checkpoint has not been connected to an ",
						UI.PRE_KEYWORD,
						"Automation",
						UI.PST_KEYWORD,
						" grid"
					});
				}
			}

			// Token: 0x02002DBF RID: 11711
			public class HIGHENERGYPARTICLEREDIRECTOR
			{
				// Token: 0x0400C470 RID: 50288
				public static LocString LOGIC_CONTROLLED_STANDBY = "Incoming Radbolts: Ignore";

				// Token: 0x0400C471 RID: 50289
				public static LocString LOGIC_CONTROLLED_ACTIVE = "Incoming Radbolts: Redirect";

				// Token: 0x0400C472 RID: 50290
				public static LocString NORMAL = "Normal";

				// Token: 0x0200379F RID: 14239
				public class TOOLTIPS
				{
					// Token: 0x0400DCF9 RID: 56569
					public static LocString LOGIC_CONTROLLED_STANDBY = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Reflector"),
						" is receiving a ",
						UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
						", ignoring incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400DCFA RID: 56570
					public static LocString LOGIC_CONTROLLED_ACTIVE = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Reflector"),
						" is receiving a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						", accepting incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400DCFB RID: 56571
					public static LocString NORMAL = "Incoming Radbolts will be accepted and redirected";
				}
			}

			// Token: 0x02002DC0 RID: 11712
			public class HIGHENERGYPARTICLESPAWNER
			{
				// Token: 0x0400C473 RID: 50291
				public static LocString LOGIC_CONTROLLED_STANDBY = "Launch Radbolt: Off";

				// Token: 0x0400C474 RID: 50292
				public static LocString LOGIC_CONTROLLED_ACTIVE = "Launch Radbolt: On";

				// Token: 0x0400C475 RID: 50293
				public static LocString NORMAL = "Normal";

				// Token: 0x020037A0 RID: 14240
				public class TOOLTIPS
				{
					// Token: 0x0400DCFC RID: 56572
					public static LocString LOGIC_CONTROLLED_STANDBY = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Generator"),
						" is receiving a ",
						UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
						", ignoring incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400DCFD RID: 56573
					public static LocString LOGIC_CONTROLLED_ACTIVE = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Generator"),
						" is receiving a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						", accepting incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400DCFE RID: 56574
					public static LocString NORMAL = string.Concat(new string[]
					{
						"Incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD,
						" will be accepted and redirected"
					});
				}
			}

			// Token: 0x02002DC1 RID: 11713
			public class AWAITINGFUEL
			{
				// Token: 0x0400C476 RID: 50294
				public static LocString NAME = "Awaiting Fuel: {0}";

				// Token: 0x0400C477 RID: 50295
				public static LocString TOOLTIP = "This building requires <b>{1}</b> of {0} to operate";
			}

			// Token: 0x02002DC2 RID: 11714
			public class FOSSILHUNT
			{
				// Token: 0x020037A1 RID: 14241
				public class PENDING_EXCAVATION
				{
					// Token: 0x0400DCFF RID: 56575
					public static LocString NAME = "Awaiting Excavation";

					// Token: 0x0400DD00 RID: 56576
					public static LocString TOOLTIP = "Currently awaiting excavation by a Duplicant";
				}

				// Token: 0x020037A2 RID: 14242
				public class EXCAVATING
				{
					// Token: 0x0400DD01 RID: 56577
					public static LocString NAME = "Excavation In Progress";

					// Token: 0x0400DD02 RID: 56578
					public static LocString TOOLTIP = "Currently being excavated by a Duplicant";
				}
			}

			// Token: 0x02002DC3 RID: 11715
			public class MEGABRAINTANK
			{
				// Token: 0x020037A3 RID: 14243
				public class PROGRESS
				{
					// Token: 0x02003B4C RID: 15180
					public class PROGRESSIONRATE
					{
						// Token: 0x0400E5C0 RID: 58816
						public static LocString NAME = "Dream Journals: {ActivationProgress}";

						// Token: 0x0400E5C1 RID: 58817
						public static LocString TOOLTIP = "Currently awaiting the Dream Journals necessary to restore this building to full functionality";
					}

					// Token: 0x02003B4D RID: 15181
					public class DREAMANALYSIS
					{
						// Token: 0x0400E5C2 RID: 58818
						public static LocString NAME = "Analyzing Dreams: {TimeToComplete}s";

						// Token: 0x0400E5C3 RID: 58819
						public static LocString TOOLTIP = "Maximum Aptitude effect sustained while dream analysis continues";
					}
				}

				// Token: 0x020037A4 RID: 14244
				public class COMPLETE
				{
					// Token: 0x0400DD03 RID: 56579
					public static LocString NAME = "Fully Restored";

					// Token: 0x0400DD04 RID: 56580
					public static LocString TOOLTIP = "This building is functioning at full capacity";
				}
			}

			// Token: 0x02002DC4 RID: 11716
			public class MEGABRAINNOTENOUGHOXYGEN
			{
				// Token: 0x0400C478 RID: 50296
				public static LocString NAME = "Lacks Oxygen";

				// Token: 0x0400C479 RID: 50297
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building needs ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" in order to function"
				});
			}

			// Token: 0x02002DC5 RID: 11717
			public class NOLOGICWIRECONNECTED
			{
				// Token: 0x0400C47A RID: 50298
				public static LocString NAME = "No Automation Wire Connected";

				// Token: 0x0400C47B RID: 50299
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has not been connected to an ",
					UI.PRE_KEYWORD,
					"Automation",
					UI.PST_KEYWORD,
					" grid"
				});
			}

			// Token: 0x02002DC6 RID: 11718
			public class NOTUBECONNECTED
			{
				// Token: 0x0400C47C RID: 50300
				public static LocString NAME = "No Tube Connected";

				// Token: 0x0400C47D RID: 50301
				public static LocString TOOLTIP = "The first section of tube extending from a " + BUILDINGS.PREFABS.TRAVELTUBEENTRANCE.NAME + " must connect directly upward";
			}

			// Token: 0x02002DC7 RID: 11719
			public class NOTUBEEXITS
			{
				// Token: 0x0400C47E RID: 50302
				public static LocString NAME = "No Landing Available";

				// Token: 0x0400C47F RID: 50303
				public static LocString TOOLTIP = "Duplicants can only exit a tube when there is somewhere for them to land within <b>two tiles</b>";
			}

			// Token: 0x02002DC8 RID: 11720
			public class STOREDCHARGE
			{
				// Token: 0x0400C480 RID: 50304
				public static LocString NAME = "Charge Available: {0}/{1}";

				// Token: 0x0400C481 RID: 50305
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has <b>{0}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nIt consumes ",
					UI.FormatAsNegativeRate("{2}"),
					" per use"
				});
			}

			// Token: 0x02002DC9 RID: 11721
			public class NEEDEGG
			{
				// Token: 0x0400C482 RID: 50306
				public static LocString NAME = "No Egg Selected";

				// Token: 0x0400C483 RID: 50307
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Collect ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" from ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" to incubate"
				});
			}

			// Token: 0x02002DCA RID: 11722
			public class NOAVAILABLEEGG
			{
				// Token: 0x0400C484 RID: 50308
				public static LocString NAME = "No Egg Available";

				// Token: 0x0400C485 RID: 50309
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The selected ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" is not currently available"
				});
			}

			// Token: 0x02002DCB RID: 11723
			public class AWAITINGEGGDELIVERY
			{
				// Token: 0x0400C486 RID: 50310
				public static LocString NAME = "Awaiting Delivery";

				// Token: 0x0400C487 RID: 50311
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Egg" + UI.PST_KEYWORD;
			}

			// Token: 0x02002DCC RID: 11724
			public class INCUBATORPROGRESS
			{
				// Token: 0x0400C488 RID: 50312
				public static LocString NAME = "Incubating: {Percent}";

				// Token: 0x0400C489 RID: 50313
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" incubating cozily\n\nIt will hatch when ",
					UI.PRE_KEYWORD,
					"Incubation",
					UI.PST_KEYWORD,
					" reaches <b>100%</b>"
				});
			}

			// Token: 0x02002DCD RID: 11725
			public class NETWORKQUALITY
			{
				// Token: 0x0400C48A RID: 50314
				public static LocString NAME = "Scan Network Quality: {TotalQuality}";

				// Token: 0x0400C48B RID: 50315
				public static LocString TOOLTIP = "This scanner network is scanning at <b>{TotalQuality}</b> effectiveness\n\nIt will detect incoming objects <b>{WorstTime}</b> to <b>{BestTime}</b> before they arrive\n\nBuild multiple " + BUILDINGS.PREFABS.COMETDETECTOR.NAME + "s to increase surface coverage and improve network quality\n\n    • Surface Coverage: <b>{Coverage}</b>";
			}

			// Token: 0x02002DCE RID: 11726
			public class DETECTORSCANNING
			{
				// Token: 0x0400C48C RID: 50316
				public static LocString NAME = "Scanning";

				// Token: 0x0400C48D RID: 50317
				public static LocString TOOLTIP = "This scanner is currently scouring space for anything of interest";
			}

			// Token: 0x02002DCF RID: 11727
			public class INCOMINGMETEORS
			{
				// Token: 0x0400C48E RID: 50318
				public static LocString NAME = "Incoming Object Detected";

				// Token: 0x0400C48F RID: 50319
				public static LocString TOOLTIP = "Warning!\n\nHigh velocity objects on approach!";
			}

			// Token: 0x02002DD0 RID: 11728
			public class SPACE_VISIBILITY_NONE
			{
				// Token: 0x0400C490 RID: 50320
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400C491 RID: 50321
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space\n    • Efficiency: <b>{VISIBILITY}</b>";
			}

			// Token: 0x02002DD1 RID: 11729
			public class SPACE_VISIBILITY_REDUCED
			{
				// Token: 0x0400C492 RID: 50322
				public static LocString NAME = "Reduced Visibility";

				// Token: 0x0400C493 RID: 50323
				public static LocString TOOLTIP = "This building has a partially obstructed view of space\n\nTo operate at maximum speed, this building requires an unblocked view of space\n    • Efficiency: <b>{VISIBILITY}</b>";
			}

			// Token: 0x02002DD2 RID: 11730
			public class LANDEDROCKETLACKSPASSENGERMODULE
			{
				// Token: 0x0400C494 RID: 50324
				public static LocString NAME = "Rocket lacks spacefarer module";

				// Token: 0x0400C495 RID: 50325
				public static LocString TOOLTIP = "A rocket must have a spacefarer module";
			}

			// Token: 0x02002DD3 RID: 11731
			public class PATH_NOT_CLEAR
			{
				// Token: 0x0400C496 RID: 50326
				public static LocString NAME = "Launch Path Blocked";

				// Token: 0x0400C497 RID: 50327
				public static LocString TOOLTIP = "There are obstructions in the launch trajectory of this rocket:\n    • {0}\n\nThis rocket requires a clear flight path for launch";

				// Token: 0x0400C498 RID: 50328
				public static LocString TILE_FORMAT = "Solid {0}";
			}

			// Token: 0x02002DD4 RID: 11732
			public class RAILGUN_PATH_NOT_CLEAR
			{
				// Token: 0x0400C499 RID: 50329
				public static LocString NAME = "Launch Path Blocked";

				// Token: 0x0400C49A RID: 50330
				public static LocString TOOLTIP = "There are obstructions in the launch trajectory of this " + UI.FormatAsLink("Interplanetary Launcher", "RAILGUN") + "\n\nThis launcher requires a clear path to launch payloads";
			}

			// Token: 0x02002DD5 RID: 11733
			public class RAILGUN_NO_DESTINATION
			{
				// Token: 0x0400C49B RID: 50331
				public static LocString NAME = "No Delivery Destination";

				// Token: 0x0400C49C RID: 50332
				public static LocString TOOLTIP = "A delivery destination has not been set";
			}

			// Token: 0x02002DD6 RID: 11734
			public class NOSURFACESIGHT
			{
				// Token: 0x0400C49D RID: 50333
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400C49E RID: 50334
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x02002DD7 RID: 11735
			public class ROCKETRESTRICTIONACTIVE
			{
				// Token: 0x0400C49F RID: 50335
				public static LocString NAME = "Access: Restricted";

				// Token: 0x0400C4A0 RID: 50336
				public static LocString TOOLTIP = "This building cannot be operated while restricted, though it can be filled\n\nControlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02002DD8 RID: 11736
			public class ROCKETRESTRICTIONINACTIVE
			{
				// Token: 0x0400C4A1 RID: 50337
				public static LocString NAME = "Access: Not Restricted";

				// Token: 0x0400C4A2 RID: 50338
				public static LocString TOOLTIP = "This building's operation is not restricted\n\nControlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02002DD9 RID: 11737
			public class NOROCKETRESTRICTION
			{
				// Token: 0x0400C4A3 RID: 50339
				public static LocString NAME = "Not Controlled";

				// Token: 0x0400C4A4 RID: 50340
				public static LocString TOOLTIP = "This building is not controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02002DDA RID: 11738
			public class BROADCASTEROUTOFRANGE
			{
				// Token: 0x0400C4A5 RID: 50341
				public static LocString NAME = "Broadcaster Out of Range";

				// Token: 0x0400C4A6 RID: 50342
				public static LocString TOOLTIP = "This receiver is too far from the selected broadcaster to get signal updates";
			}

			// Token: 0x02002DDB RID: 11739
			public class LOSINGRADBOLTS
			{
				// Token: 0x0400C4A7 RID: 50343
				public static LocString NAME = "Radbolt Decay";

				// Token: 0x0400C4A8 RID: 50344
				public static LocString TOOLTIP = "This building is unable to maintain the integrity of the radbolts it is storing";
			}

			// Token: 0x02002DDC RID: 11740
			public class TOP_PRIORITY_CHORE
			{
				// Token: 0x0400C4A9 RID: 50345
				public static LocString NAME = "Top Priority";

				// Token: 0x0400C4AA RID: 50346
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This errand has been set to ",
					UI.PRE_KEYWORD,
					"Top Priority",
					UI.PST_KEYWORD,
					"\n\nThe colony will be in ",
					UI.PRE_KEYWORD,
					"Yellow Alert",
					UI.PST_KEYWORD,
					" until this task is completed"
				});

				// Token: 0x0400C4AB RID: 50347
				public static LocString NOTIFICATION_NAME = "Yellow Alert";

				// Token: 0x0400C4AC RID: 50348
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The following errands have been set to ",
					UI.PRE_KEYWORD,
					"Top Priority",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02002DDD RID: 11741
			public class HOTTUBWATERTOOCOLD
			{
				// Token: 0x0400C4AD RID: 50349
				public static LocString NAME = "Water Too Cold";

				// Token: 0x0400C4AE RID: 50350
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub's ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" is below <b>{temperature}</b>\n\nIt is draining so it can be refilled with warmer ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002DDE RID: 11742
			public class HOTTUBTOOHOT
			{
				// Token: 0x0400C4AF RID: 50351
				public static LocString NAME = "Building Too Hot";

				// Token: 0x0400C4B0 RID: 50352
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub's ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{temperature}</b>\n\nIt needs to cool before it can safely be used"
				});
			}

			// Token: 0x02002DDF RID: 11743
			public class HOTTUBFILLING
			{
				// Token: 0x0400C4B1 RID: 50353
				public static LocString NAME = "Filling Up: ({fullness})";

				// Token: 0x0400C4B2 RID: 50354
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub is currently filling with ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					"\n\nIt will be available to use when the ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" level reaches <b>100%</b>"
				});
			}

			// Token: 0x02002DE0 RID: 11744
			public class WINDTUNNELINTAKE
			{
				// Token: 0x0400C4B3 RID: 50355
				public static LocString NAME = "Intake Requires Gas";

				// Token: 0x0400C4B4 RID: 50356
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A wind tunnel requires ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" at the top and bottom intakes in order to operate\n\nThe intakes for this wind tunnel don't have enough gas to operate"
				});
			}

			// Token: 0x02002DE1 RID: 11745
			public class TEMPORAL_TEAR_OPENER_NO_TARGET
			{
				// Token: 0x0400C4B5 RID: 50357
				public static LocString NAME = "Temporal Tear not revealed";

				// Token: 0x0400C4B6 RID: 50358
				public static LocString TOOLTIP = "This machine is meant to target something in space, but the target has not yet been revealed";
			}

			// Token: 0x02002DE2 RID: 11746
			public class TEMPORAL_TEAR_OPENER_NO_LOS
			{
				// Token: 0x0400C4B7 RID: 50359
				public static LocString NAME = "Line of Sight: Obstructed";

				// Token: 0x0400C4B8 RID: 50360
				public static LocString TOOLTIP = "This device needs a clear view of space to operate";
			}

			// Token: 0x02002DE3 RID: 11747
			public class TEMPORAL_TEAR_OPENER_INSUFFICIENT_COLONIES
			{
				// Token: 0x0400C4B9 RID: 50361
				public static LocString NAME = "Too few Printing Pods {progress}";

				// Token: 0x0400C4BA RID: 50362
				public static LocString TOOLTIP = "To open the Temporal Tear, this device relies on a network of activated Printing Pods {progress}";
			}

			// Token: 0x02002DE4 RID: 11748
			public class TEMPORAL_TEAR_OPENER_PROGRESS
			{
				// Token: 0x0400C4BB RID: 50363
				public static LocString NAME = "Charging Progress: {progress}";

				// Token: 0x0400C4BC RID: 50364
				public static LocString TOOLTIP = "This device must be charged with a high number of Radbolts\n\nOperation can commence once this device is fully charged";
			}

			// Token: 0x02002DE5 RID: 11749
			public class TEMPORAL_TEAR_OPENER_READY
			{
				// Token: 0x0400C4BD RID: 50365
				public static LocString NOTIFICATION = "Temporal Tear Opener fully charged";

				// Token: 0x0400C4BE RID: 50366
				public static LocString NOTIFICATION_TOOLTIP = "Push the red button to activate";
			}

			// Token: 0x02002DE6 RID: 11750
			public class WARPPORTALCHARGING
			{
				// Token: 0x0400C4BF RID: 50367
				public static LocString NAME = "Recharging: {charge}";

				// Token: 0x0400C4C0 RID: 50368
				public static LocString TOOLTIP = "This teleporter will be ready for use in {cycles} cycles";
			}

			// Token: 0x02002DE7 RID: 11751
			public class WARPCONDUITPARTNERDISABLED
			{
				// Token: 0x0400C4C1 RID: 50369
				public static LocString NAME = "Teleporter Disabled ({x}/2)";

				// Token: 0x0400C4C2 RID: 50370
				public static LocString TOOLTIP = "This teleporter cannot be used until both the transmitting and receiving sides have been activated";
			}

			// Token: 0x02002DE8 RID: 11752
			public class COLLECTINGHEP
			{
				// Token: 0x0400C4C3 RID: 50371
				public static LocString NAME = "Collecting Radbolts ({x}/cycle)";

				// Token: 0x0400C4C4 RID: 50372
				public static LocString TOOLTIP = "Collecting Radbolts from ambient radiation";
			}

			// Token: 0x02002DE9 RID: 11753
			public class INORBIT
			{
				// Token: 0x0400C4C5 RID: 50373
				public static LocString NAME = "In Orbit: {Destination}";

				// Token: 0x0400C4C6 RID: 50374
				public static LocString TOOLTIP = "This rocket is currently in orbit around {Destination}";
			}

			// Token: 0x02002DEA RID: 11754
			public class WAITINGTOLAND
			{
				// Token: 0x0400C4C7 RID: 50375
				public static LocString NAME = "Waiting to land on {Destination}";

				// Token: 0x0400C4C8 RID: 50376
				public static LocString TOOLTIP = "This rocket is waiting for an available Rcoket Platform on {Destination}";
			}

			// Token: 0x02002DEB RID: 11755
			public class INFLIGHT
			{
				// Token: 0x0400C4C9 RID: 50377
				public static LocString NAME = "In Flight To {Destination_Asteroid}: {ETA}";

				// Token: 0x0400C4CA RID: 50378
				public static LocString TOOLTIP = "This rocket is currently traveling to {Destination_Pad} on {Destination_Asteroid}\n\nIt will arrive in {ETA}";

				// Token: 0x0400C4CB RID: 50379
				public static LocString TOOLTIP_NO_PAD = "This rocket is currently traveling to {Destination_Asteroid}\n\nIt will arrive in {ETA}";
			}

			// Token: 0x02002DEC RID: 11756
			public class DESTINATIONOUTOFRANGE
			{
				// Token: 0x0400C4CC RID: 50380
				public static LocString NAME = "Destination Out Of Range";

				// Token: 0x0400C4CD RID: 50381
				public static LocString TOOLTIP = "This rocket lacks the range to reach its destination\n\nRocket Range: {Range}\nDestination Distance: {Distance}";
			}

			// Token: 0x02002DED RID: 11757
			public class ROCKETSTRANDED
			{
				// Token: 0x0400C4CE RID: 50382
				public static LocString NAME = "Stranded";

				// Token: 0x0400C4CF RID: 50383
				public static LocString TOOLTIP = "This rocket has run out of fuel and cannot move";
			}

			// Token: 0x02002DEE RID: 11758
			public class SPACEPOIHARVESTING
			{
				// Token: 0x0400C4D0 RID: 50384
				public static LocString NAME = "Extracting Resources: {0}";

				// Token: 0x0400C4D1 RID: 50385
				public static LocString TOOLTIP = "Resources are being mined from this space debris";
			}

			// Token: 0x02002DEF RID: 11759
			public class SPACEPOIWASTING
			{
				// Token: 0x0400C4D2 RID: 50386
				public static LocString NAME = "Cannot store resources: {0}";

				// Token: 0x0400C4D3 RID: 50387
				public static LocString TOOLTIP = "Some resources being mined from this space debris cannot be stored in this rocket";
			}

			// Token: 0x02002DF0 RID: 11760
			public class RAILGUNPAYLOADNEEDSEMPTYING
			{
				// Token: 0x0400C4D4 RID: 50388
				public static LocString NAME = "Ready To Unpack";

				// Token: 0x0400C4D5 RID: 50389
				public static LocString TOOLTIP = "This payload has reached its destination and is ready to be unloaded\n\nIt can be marked for unpacking manually, or automatically unpacked on arrival using a " + BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME;
			}

			// Token: 0x02002DF1 RID: 11761
			public class MISSIONCONTROLASSISTINGROCKET
			{
				// Token: 0x0400C4D6 RID: 50390
				public static LocString NAME = "Guidance Signal: {0}";

				// Token: 0x0400C4D7 RID: 50391
				public static LocString TOOLTIP = "Once transmission is complete, Mission Control will boost targeted rocket's speed";
			}

			// Token: 0x02002DF2 RID: 11762
			public class MISSIONCONTROLBOOSTED
			{
				// Token: 0x0400C4D8 RID: 50392
				public static LocString NAME = "Mission Control Speed Boost: {0}";

				// Token: 0x0400C4D9 RID: 50393
				public static LocString TOOLTIP = "Mission Control has given this rocket a {0} speed boost\n\n{1} remaining";
			}

			// Token: 0x02002DF3 RID: 11763
			public class TRANSITTUBEENTRANCEWAXREADY
			{
				// Token: 0x0400C4DA RID: 50394
				public static LocString NAME = "Smooth Ride Ready";

				// Token: 0x0400C4DB RID: 50395
				public static LocString TOOLTIP = "This building is stocked with speed-boosting " + ELEMENTS.MILKFAT.NAME + "\n\n{0} per use ({1} remaining)";
			}

			// Token: 0x02002DF4 RID: 11764
			public class NOROCKETSTOMISSIONCONTROLBOOST
			{
				// Token: 0x0400C4DC RID: 50396
				public static LocString NAME = "No Eligible Rockets in Range";

				// Token: 0x0400C4DD RID: 50397
				public static LocString TOOLTIP = "Rockets must be mid-flight and not targeted by another Mission Control Station, or already boosted";
			}

			// Token: 0x02002DF5 RID: 11765
			public class NOROCKETSTOMISSIONCONTROLCLUSTERBOOST
			{
				// Token: 0x0400C4DE RID: 50398
				public static LocString NAME = "No Eligible Rockets in Range";

				// Token: 0x0400C4DF RID: 50399
				public static LocString TOOLTIP = "Rockets must be mid-flight, within {0} tiles, and not targeted by another Mission Control Station or already boosted";
			}

			// Token: 0x02002DF6 RID: 11766
			public class AWAITINGEMPTYBUILDING
			{
				// Token: 0x0400C4E0 RID: 50400
				public static LocString NAME = "Empty Errand";

				// Token: 0x0400C4E1 RID: 50401
				public static LocString TOOLTIP = "Building will be emptied once a Duplicant is available";
			}

			// Token: 0x02002DF7 RID: 11767
			public class DUPLICANTACTIVATIONREQUIRED
			{
				// Token: 0x0400C4E2 RID: 50402
				public static LocString NAME = "Activation Required";

				// Token: 0x0400C4E3 RID: 50403
				public static LocString TOOLTIP = "A Duplicant is required to bring this building online";
			}

			// Token: 0x02002DF8 RID: 11768
			public class PILOTNEEDED
			{
				// Token: 0x0400C4E4 RID: 50404
				public static LocString NAME = "Switching to Autopilot";

				// Token: 0x0400C4E5 RID: 50405
				public static LocString TOOLTIP = "Autopilot will engage in {timeRemaining} if a Duplicant pilot does not assume control";
			}

			// Token: 0x02002DF9 RID: 11769
			public class AUTOPILOTACTIVE
			{
				// Token: 0x0400C4E6 RID: 50406
				public static LocString NAME = "Autopilot Engaged";

				// Token: 0x0400C4E7 RID: 50407
				public static LocString TOOLTIP = "This rocket has entered autopilot mode and will fly at reduced speed\n\nIt can resume full speed once a Duplicant pilot takes over";
			}

			// Token: 0x02002DFA RID: 11770
			public class ROCKETCHECKLISTINCOMPLETE
			{
				// Token: 0x0400C4E8 RID: 50408
				public static LocString NAME = "Launch Checklist Incomplete";

				// Token: 0x0400C4E9 RID: 50409
				public static LocString TOOLTIP = "Critical launch tasks uncompleted\n\nRefer to the Launch Checklist in the status panel";
			}

			// Token: 0x02002DFB RID: 11771
			public class ROCKETCARGOEMPTYING
			{
				// Token: 0x0400C4EA RID: 50410
				public static LocString NAME = "Unloading Cargo";

				// Token: 0x0400C4EB RID: 50411
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Rocket cargo is being unloaded into the ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					"\n\nLoading of new cargo will begin once unloading is complete"
				});
			}

			// Token: 0x02002DFC RID: 11772
			public class ROCKETCARGOFILLING
			{
				// Token: 0x0400C4EC RID: 50412
				public static LocString NAME = "Loading Cargo";

				// Token: 0x0400C4ED RID: 50413
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Cargo is being loaded onto the rocket from the ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					"\n\nRocket cargo will be ready for launch once loading is complete"
				});
			}

			// Token: 0x02002DFD RID: 11773
			public class ROCKETCARGOFULL
			{
				// Token: 0x0400C4EE RID: 50414
				public static LocString NAME = "Platform Ready";

				// Token: 0x0400C4EF RID: 50415
				public static LocString TOOLTIP = "All cargo operations are complete";
			}

			// Token: 0x02002DFE RID: 11774
			public class FLIGHTALLCARGOFULL
			{
				// Token: 0x0400C4F0 RID: 50416
				public static LocString NAME = "All cargo bays are full";

				// Token: 0x0400C4F1 RID: 50417
				public static LocString TOOLTIP = "Rocket cannot store any more materials";
			}

			// Token: 0x02002DFF RID: 11775
			public class FLIGHTCARGOREMAINING
			{
				// Token: 0x0400C4F2 RID: 50418
				public static LocString NAME = "Cargo capacity remaining: {0}";

				// Token: 0x0400C4F3 RID: 50419
				public static LocString TOOLTIP = "Rocket can store up to {0} more materials";
			}

			// Token: 0x02002E00 RID: 11776
			public class ROCKET_PORT_IDLE
			{
				// Token: 0x0400C4F4 RID: 50420
				public static LocString NAME = "Idle";

				// Token: 0x0400C4F5 RID: 50421
				public static LocString TOOLTIP = "This port is idle because there is no rocket on the connected " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;
			}

			// Token: 0x02002E01 RID: 11777
			public class ROCKET_PORT_UNLOADING
			{
				// Token: 0x0400C4F6 RID: 50422
				public static LocString NAME = "Unloading Rocket";

				// Token: 0x0400C4F7 RID: 50423
				public static LocString TOOLTIP = "Resources are being unloaded from the rocket into the local network";
			}

			// Token: 0x02002E02 RID: 11778
			public class ROCKET_PORT_LOADING
			{
				// Token: 0x0400C4F8 RID: 50424
				public static LocString NAME = "Loading Rocket";

				// Token: 0x0400C4F9 RID: 50425
				public static LocString TOOLTIP = "Resources are being loaded from the local network into the rocket's storage";
			}

			// Token: 0x02002E03 RID: 11779
			public class ROCKET_PORT_LOADED
			{
				// Token: 0x0400C4FA RID: 50426
				public static LocString NAME = "Cargo Transfer Complete";

				// Token: 0x0400C4FB RID: 50427
				public static LocString TOOLTIP = "The connected rocket has either reached max capacity for this resource type, or lacks appropriate storage modules";
			}

			// Token: 0x02002E04 RID: 11780
			public class CONNECTED_ROCKET_PORT
			{
				// Token: 0x0400C4FC RID: 50428
				public static LocString NAME = "Port Network Attached";

				// Token: 0x0400C4FD RID: 50429
				public static LocString TOOLTIP = "This module has been connected to a " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + " and can now load and unload cargo";
			}

			// Token: 0x02002E05 RID: 11781
			public class CONNECTED_ROCKET_WRONG_PORT
			{
				// Token: 0x0400C4FE RID: 50430
				public static LocString NAME = "Incorrect Port Network";

				// Token: 0x0400C4FF RID: 50431
				public static LocString TOOLTIP = "The attached " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + " is not the correct type for this cargo module";
			}

			// Token: 0x02002E06 RID: 11782
			public class CONNECTED_ROCKET_NO_PORT
			{
				// Token: 0x0400C500 RID: 50432
				public static LocString NAME = "No Rocket Ports";

				// Token: 0x0400C501 RID: 50433
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					" has no ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					" attached\n\n",
					UI.PRE_KEYWORD,
					"Solid",
					UI.PST_KEYWORD,
					", ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					", and ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME_PLURAL,
					" can be attached to load and unload cargo from a landed rocket's modules"
				});
			}

			// Token: 0x02002E07 RID: 11783
			public class CLUSTERTELESCOPEALLWORKCOMPLETE
			{
				// Token: 0x0400C502 RID: 50434
				public static LocString NAME = "Area Complete";

				// Token: 0x0400C503 RID: 50435
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Telescope",
					UI.PST_KEYWORD,
					" has analyzed all the space visible from its current location"
				});
			}

			// Token: 0x02002E08 RID: 11784
			public class ROCKETPLATFORMCLOSETOCEILING
			{
				// Token: 0x0400C504 RID: 50436
				public static LocString NAME = "Low Clearance: {distance} Tiles";

				// Token: 0x0400C505 RID: 50437
				public static LocString TOOLTIP = "Tall rockets may not be able to land on this " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;
			}

			// Token: 0x02002E09 RID: 11785
			public class MODULEGENERATORNOTPOWERED
			{
				// Token: 0x0400C506 RID: 50438
				public static LocString NAME = "Thrust Generation: {ActiveWattage}/{MaxWattage}";

				// Token: 0x0400C507 RID: 50439
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Engine will generate ",
					UI.FormatAsPositiveRate("{MaxWattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" once traveling through space\n\nRight now, it's not doing much of anything"
				});
			}

			// Token: 0x02002E0A RID: 11786
			public class MODULEGENERATORPOWERED
			{
				// Token: 0x0400C508 RID: 50440
				public static LocString NAME = "Thrust Generation: {ActiveWattage}/{MaxWattage}";

				// Token: 0x0400C509 RID: 50441
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Engine is extracting ",
					UI.FormatAsPositiveRate("{MaxWattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from the thruster\n\nIt will continue generating power as long as it travels through space"
				});
			}

			// Token: 0x02002E0B RID: 11787
			public class INORBITREQUIRED
			{
				// Token: 0x0400C50A RID: 50442
				public static LocString NAME = "Grounded";

				// Token: 0x0400C50B RID: 50443
				public static LocString TOOLTIP = "This building cannot operate from the surface of a " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " and must be in space to function";
			}

			// Token: 0x02002E0C RID: 11788
			public class REACTORREFUELDISABLED
			{
				// Token: 0x0400C50C RID: 50444
				public static LocString NAME = "Refuel Disabled";

				// Token: 0x0400C50D RID: 50445
				public static LocString TOOLTIP = "This building will not be refueled once its active fuel has been consumed";
			}

			// Token: 0x02002E0D RID: 11789
			public class RAILGUNCOOLDOWN
			{
				// Token: 0x0400C50E RID: 50446
				public static LocString NAME = "Cleaning Rails: {timeleft}";

				// Token: 0x0400C50F RID: 50447
				public static LocString TOOLTIP = "This building automatically performs routine maintenance every {x} launches";
			}

			// Token: 0x02002E0E RID: 11790
			public class FRIDGECOOLING
			{
				// Token: 0x0400C510 RID: 50448
				public static LocString NAME = "Cooling Contents: {UsedPower}";

				// Token: 0x0400C511 RID: 50449
				public static LocString TOOLTIP = "{UsedPower} of {MaxPower} are being used to cool the contents of this food storage";
			}

			// Token: 0x02002E0F RID: 11791
			public class FRIDGESTEADY
			{
				// Token: 0x0400C512 RID: 50450
				public static LocString NAME = "Energy Saver: {UsedPower}";

				// Token: 0x0400C513 RID: 50451
				public static LocString TOOLTIP = "The contents of this food storage are at refrigeration temperatures\n\nEnergy Saver mode has been automatically activated using only {UsedPower} of {MaxPower}";
			}

			// Token: 0x02002E10 RID: 11792
			public class TELEPHONE
			{
				// Token: 0x020037A5 RID: 14245
				public class BABBLE
				{
					// Token: 0x0400DD05 RID: 56581
					public static LocString NAME = "Babbling to no one.";

					// Token: 0x0400DD06 RID: 56582
					public static LocString TOOLTIP = "{Duplicant} just needed to vent to into the void.";
				}

				// Token: 0x020037A6 RID: 14246
				public class CONVERSATION
				{
					// Token: 0x0400DD07 RID: 56583
					public static LocString TALKING_TO = "Talking to {Duplicant} on {Asteroid}";

					// Token: 0x0400DD08 RID: 56584
					public static LocString TALKING_TO_NUM = "Talking to {0} friends.";
				}
			}

			// Token: 0x02002E11 RID: 11793
			public class CREATUREMANIPULATORPROGRESS
			{
				// Token: 0x0400C514 RID: 50452
				public static LocString NAME = "Collected Species Data {0}/{1}";

				// Token: 0x0400C515 RID: 50453
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires data from multiple ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" species to unlock its genetic manipulator\n\nSpecies scanned:"
				});

				// Token: 0x0400C516 RID: 50454
				public static LocString NO_DATA = "No species scanned";
			}

			// Token: 0x02002E12 RID: 11794
			public class CREATUREMANIPULATORMORPHMODELOCKED
			{
				// Token: 0x0400C517 RID: 50455
				public static LocString NAME = "Current Status: Offline";

				// Token: 0x0400C518 RID: 50456
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building cannot operate until it collects more ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" DNA"
				});
			}

			// Token: 0x02002E13 RID: 11795
			public class CREATUREMANIPULATORMORPHMODE
			{
				// Token: 0x0400C519 RID: 50457
				public static LocString NAME = "Current Status: Online";

				// Token: 0x0400C51A RID: 50458
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is ready to manipulate ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" DNA"
				});
			}

			// Token: 0x02002E14 RID: 11796
			public class CREATUREMANIPULATORWAITING
			{
				// Token: 0x0400C51B RID: 50459
				public static LocString NAME = "Waiting for a Critter";

				// Token: 0x0400C51C RID: 50460
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is waiting for a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" to get sucked into its scanning area"
				});
			}

			// Token: 0x02002E15 RID: 11797
			public class CREATUREMANIPULATORWORKING
			{
				// Token: 0x0400C51D RID: 50461
				public static LocString NAME = "Poking and Prodding Critter";

				// Token: 0x0400C51E RID: 50462
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is extracting genetic information from a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" "
				});
			}

			// Token: 0x02002E16 RID: 11798
			public class SPICEGRINDERNOSPICE
			{
				// Token: 0x0400C51F RID: 50463
				public static LocString NAME = "No Spice Selected";

				// Token: 0x0400C520 RID: 50464
				public static LocString TOOLTIP = "Select a recipe to begin fabrication";
			}

			// Token: 0x02002E17 RID: 11799
			public class SPICEGRINDERACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400C521 RID: 50465
				public static LocString NAME = "Spice Grinder accepts mutant seeds";

				// Token: 0x0400C522 RID: 50466
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This spice grinder is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as recipe ingredients"
				});
			}

			// Token: 0x02002E18 RID: 11800
			public class MISSILELAUNCHER_NOSURFACESIGHT
			{
				// Token: 0x0400C523 RID: 50467
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400C524 RID: 50468
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x02002E19 RID: 11801
			public class MISSILELAUNCHER_PARTIALLYBLOCKED
			{
				// Token: 0x0400C525 RID: 50469
				public static LocString NAME = "Limited Line of Sight";

				// Token: 0x0400C526 RID: 50470
				public static LocString TOOLTIP = "This building has a partially obstructed view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x02002E1A RID: 11802
			public class COMPLEXFABRICATOR
			{
				// Token: 0x020037A7 RID: 14247
				public class COOKING
				{
					// Token: 0x0400DD09 RID: 56585
					public static LocString NAME = "Cooking {Item}";

					// Token: 0x0400DD0A RID: 56586
					public static LocString TOOLTIP = "This building is currently whipping up a batch of {Item}";
				}

				// Token: 0x020037A8 RID: 14248
				public class PRODUCING
				{
					// Token: 0x0400DD0B RID: 56587
					public static LocString NAME = "Producing {Item}";

					// Token: 0x0400DD0C RID: 56588
					public static LocString TOOLTIP = "This building is carrying out its current production orders";
				}

				// Token: 0x020037A9 RID: 14249
				public class RESEARCHING
				{
					// Token: 0x0400DD0D RID: 56589
					public static LocString NAME = "Researching {Item}";

					// Token: 0x0400DD0E RID: 56590
					public static LocString TOOLTIP = "This building is currently conducting important research";
				}

				// Token: 0x020037AA RID: 14250
				public class ANALYZING
				{
					// Token: 0x0400DD0F RID: 56591
					public static LocString NAME = "Analyzing {Item}";

					// Token: 0x0400DD10 RID: 56592
					public static LocString TOOLTIP = "This building is currently analyzing a fascinating artifact";
				}

				// Token: 0x020037AB RID: 14251
				public class UNTRAINING
				{
					// Token: 0x0400DD11 RID: 56593
					public static LocString NAME = "Untraining {Duplicant}";

					// Token: 0x0400DD12 RID: 56594
					public static LocString TOOLTIP = "Restoring {Duplicant} to a blissfully ignorant state";
				}

				// Token: 0x020037AC RID: 14252
				public class TELESCOPE
				{
					// Token: 0x0400DD13 RID: 56595
					public static LocString NAME = "Studying Space";

					// Token: 0x0400DD14 RID: 56596
					public static LocString TOOLTIP = "This building is currently investigating the mysteries of space";
				}

				// Token: 0x020037AD RID: 14253
				public class CLUSTERTELESCOPEMETEOR
				{
					// Token: 0x0400DD15 RID: 56597
					public static LocString NAME = "Studying Meteor";

					// Token: 0x0400DD16 RID: 56598
					public static LocString TOOLTIP = "This building is currently studying a meteor";
				}
			}

			// Token: 0x02002E1B RID: 11803
			public class REMOTEWORKERDEPOT
			{
				// Token: 0x020037AE RID: 14254
				public class MAKINGWORKER
				{
					// Token: 0x0400DD17 RID: 56599
					public static LocString NAME = "Assembling Remote Worker";

					// Token: 0x0400DD18 RID: 56600
					public static LocString TOOLTIP = "This building is currently assembling a remote worker drone";
				}
			}

			// Token: 0x02002E1C RID: 11804
			public class REMOTEWORKTERMINAL
			{
				// Token: 0x020037AF RID: 14255
				public class NODOCK
				{
					// Token: 0x0400DD19 RID: 56601
					public static LocString NAME = "No Dock Assigned";

					// Token: 0x0400DD1A RID: 56602
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This building must be assigned a ",
						UI.PRE_KEYWORD,
						"Remote Worker Dock",
						UI.PST_KEYWORD,
						" in order to function"
					});
				}
			}

			// Token: 0x02002E1D RID: 11805
			public class DATAMINER
			{
				// Token: 0x020037B0 RID: 14256
				public class PRODUCTIONRATE
				{
					// Token: 0x0400DD1B RID: 56603
					public static LocString NAME = "Production Rate: {RATE}";

					// Token: 0x0400DD1C RID: 56604
					public static LocString TOOLTIP = "This building is operating at {RATE} of its maximum speed\n\nProduction rate decreases at higher temperatures\n\nCurrent ambient temperature: {TEMP}";
				}
			}
		}

		// Token: 0x02002198 RID: 8600
		public class DETAILS
		{
			// Token: 0x040098D8 RID: 39128
			public static LocString USE_COUNT = "Uses: {0}";

			// Token: 0x040098D9 RID: 39129
			public static LocString USE_COUNT_TOOLTIP = "This building has been used {0} times";
		}
	}
}
