using System;
using TUNING;

namespace STRINGS
{
	// Token: 0x02000F0B RID: 3851
	public class BUILDINGS
	{
		// Token: 0x02002012 RID: 8210
		public class PREFABS
		{
			// Token: 0x02002690 RID: 9872
			public class HEADQUARTERSCOMPLETE
			{
				// Token: 0x0400AB3F RID: 43839
				public static LocString NAME = UI.FormatAsLink("Printing Pod", "HEADQUARTERS");

				// Token: 0x0400AB40 RID: 43840
				public static LocString UNIQUE_POPTEXT = "A clone of the cloning machine? What a novel thought.\n\nAlas, it won't work.";
			}

			// Token: 0x02002691 RID: 9873
			public class EXOBASEHEADQUARTERS
			{
				// Token: 0x0400AB41 RID: 43841
				public static LocString NAME = UI.FormatAsLink("Mini-Pod", "EXOBASEHEADQUARTERS");

				// Token: 0x0400AB42 RID: 43842
				public static LocString DESC = "A quick and easy substitute, though it'll never live up to the original.";

				// Token: 0x0400AB43 RID: 43843
				public static LocString EFFECT = "A portable bioprinter that produces new Duplicants or care packages containing resources.\n\nOnly one Printing Pod or Mini-Pod is permitted per Planetoid.";
			}

			// Token: 0x02002692 RID: 9874
			public class AIRCONDITIONER
			{
				// Token: 0x0400AB44 RID: 43844
				public static LocString NAME = UI.FormatAsLink("Thermo Regulator", "AIRCONDITIONER");

				// Token: 0x0400AB45 RID: 43845
				public static LocString DESC = "A thermo regulator doesn't remove heat, but relocates it to a new area.";

				// Token: 0x0400AB46 RID: 43846
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Cools the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" piped through it, but outputs ",
					UI.FormatAsLink("Heat", "HEAT"),
					" in its immediate vicinity."
				});
			}

			// Token: 0x02002693 RID: 9875
			public class STATERPILLAREGG
			{
				// Token: 0x0400AB47 RID: 43847
				public static LocString NAME = UI.FormatAsLink("Slug Egg", "STATERPILLAREGG");

				// Token: 0x0400AB48 RID: 43848
				public static LocString DESC = "The electrifying egg of the " + UI.FormatAsLink("Plug Slug", "STATERPILLAR") + ".";

				// Token: 0x0400AB49 RID: 43849
				public static LocString EFFECT = "Slug Eggs can be connected to a " + UI.FormatAsLink("Power", "POWER") + " circuit as an energy source.";
			}

			// Token: 0x02002694 RID: 9876
			public class STATERPILLARGENERATOR
			{
				// Token: 0x0400AB4A RID: 43850
				public static LocString NAME = UI.FormatAsLink("Plug Slug", "STATERPILLAR");

				// Token: 0x02003547 RID: 13639
				public class MODIFIERS
				{
					// Token: 0x0400D7DA RID: 55258
					public static LocString WILD = "Wild!";

					// Token: 0x0400D7DB RID: 55259
					public static LocString HUNGRY = "Hungry!";
				}
			}

			// Token: 0x02002695 RID: 9877
			public class BEEHIVE
			{
				// Token: 0x0400AB4B RID: 43851
				public static LocString NAME = UI.FormatAsLink("Beeta Hive", "BEEHIVE");

				// Token: 0x0400AB4C RID: 43852
				public static LocString DESC = string.Concat(new string[]
				{
					"A moderately ",
					UI.FormatAsLink("Radioactive", "RADIATION"),
					" nest made by ",
					UI.FormatAsLink("Beetas", "BEE"),
					".\n\nConverts ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					" into ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" when worked by a Beeta.\nWill not function if ground below has been destroyed."
				});

				// Token: 0x0400AB4D RID: 43853
				public static LocString EFFECT = "The cozy home of a Beeta.";
			}

			// Token: 0x02002696 RID: 9878
			public class ETHANOLDISTILLERY
			{
				// Token: 0x0400AB4E RID: 43854
				public static LocString NAME = UI.FormatAsLink("Ethanol Distiller", "ETHANOLDISTILLERY");

				// Token: 0x0400AB4F RID: 43855
				public static LocString DESC = string.Concat(new string[]
				{
					"Ethanol distillers convert ",
					ITEMS.INDUSTRIAL_PRODUCTS.WOOD.NAME,
					" into burnable ",
					ELEMENTS.ETHANOL.NAME,
					" fuel."
				});

				// Token: 0x0400AB50 RID: 43856
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Refines ",
					UI.FormatAsLink("Wood", "WOOD"),
					" into ",
					UI.FormatAsLink("Ethanol", "ETHANOL"),
					"."
				});
			}

			// Token: 0x02002697 RID: 9879
			public class ALGAEDISTILLERY
			{
				// Token: 0x0400AB51 RID: 43857
				public static LocString NAME = UI.FormatAsLink("Algae Distiller", "ALGAEDISTILLERY");

				// Token: 0x0400AB52 RID: 43858
				public static LocString DESC = "Algae distillers convert disease-causing slime into algae for oxygen production.";

				// Token: 0x0400AB53 RID: 43859
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Refines ",
					UI.FormatAsLink("Slime", "SLIMEMOLD"),
					" into ",
					UI.FormatAsLink("Algae", "ALGAE"),
					"."
				});
			}

			// Token: 0x02002698 RID: 9880
			public class GUNKEMPTIER
			{
				// Token: 0x0400AB54 RID: 43860
				public static LocString NAME = UI.FormatAsLink("Gunk Extractor", "GUNKEMPTIER");

				// Token: 0x0400AB55 RID: 43861
				public static LocString DESC = "Bionic Duplicants are much more relaxed after a visit to the gunk extractor.";

				// Token: 0x0400AB56 RID: 43862
				public static LocString EFFECT = "Cleanses stale " + UI.FormatAsLink("Liquid Gunk", "LIQUIDGUNK") + " build-up from Duplicants' bionic parts.";
			}

			// Token: 0x02002699 RID: 9881
			public class OILCHANGER
			{
				// Token: 0x0400AB57 RID: 43863
				public static LocString NAME = UI.FormatAsLink("Lubrication Station", "OILCHANGER");

				// Token: 0x0400AB58 RID: 43864
				public static LocString DESC = "A fresh supply of oil keeps the ol' joints from getting too creaky.";

				// Token: 0x0400AB59 RID: 43865
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Phyto Oil", "PHYTOOIL"),
					" or ",
					UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
					" to keep Duplicants' bionic parts running smoothly."
				});
			}

			// Token: 0x0200269A RID: 9882
			public class OXYLITEREFINERY
			{
				// Token: 0x0400AB5A RID: 43866
				public static LocString NAME = UI.FormatAsLink("Oxylite Refinery", "OXYLITEREFINERY");

				// Token: 0x0400AB5B RID: 43867
				public static LocString DESC = "Oxylite is a solid and easily transportable source of consumable oxygen.";

				// Token: 0x0400AB5C RID: 43868
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Synthesizes ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" using ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and a small amount of ",
					UI.FormatAsLink("Gold", "GOLD"),
					"."
				});
			}

			// Token: 0x0200269B RID: 9883
			public class OXYSCONCE
			{
				// Token: 0x0400AB5D RID: 43869
				public static LocString NAME = UI.FormatAsLink("Oxylite Sconce", "OXYSCONCE");

				// Token: 0x0400AB5E RID: 43870
				public static LocString DESC = "Sconces prevent diffused oxygen from being wasted inside storage bins.";

				// Token: 0x0400AB5F RID: 43871
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores a small chunk of ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" which gradually releases ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" into the environment."
				});
			}

			// Token: 0x0200269C RID: 9884
			public class FERTILIZERMAKER
			{
				// Token: 0x0400AB60 RID: 43872
				public static LocString NAME = UI.FormatAsLink("Fertilizer Synthesizer", "FERTILIZERMAKER");

				// Token: 0x0400AB61 RID: 43873
				public static LocString DESC = "Fertilizer synthesizers convert polluted dirt into fertilizer for domestic plants.";

				// Token: 0x0400AB62 RID: 43874
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" and ",
					UI.FormatAsLink("Phosphorite", "PHOSPHORITE"),
					" to produce ",
					UI.FormatAsLink("Fertilizer", "FERTILIZER"),
					"."
				});
			}

			// Token: 0x0200269D RID: 9885
			public class ALGAEHABITAT
			{
				// Token: 0x0400AB63 RID: 43875
				public static LocString NAME = UI.FormatAsLink("Algae Terrarium", "ALGAEHABITAT");

				// Token: 0x0400AB64 RID: 43876
				public static LocString DESC = "Algae colony, Duplicant colony... we're more alike than we are different.";

				// Token: 0x0400AB65 RID: 43877
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsLink("Algae", "ALGAE"),
					" to produce ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and remove some ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					".\n\nGains a 10% efficiency boost in direct ",
					UI.FormatAsLink("Light", "LIGHT"),
					"."
				});

				// Token: 0x0400AB66 RID: 43878
				public static LocString SIDESCREEN_TITLE = "Empty " + UI.FormatAsLink("Polluted Water", "DIRTYWATER") + " Threshold";
			}

			// Token: 0x0200269E RID: 9886
			public class BATTERY
			{
				// Token: 0x0400AB67 RID: 43879
				public static LocString NAME = UI.FormatAsLink("Battery", "BATTERY");

				// Token: 0x0400AB68 RID: 43880
				public static LocString DESC = "Batteries allow power from generators to be stored for later.";

				// Token: 0x0400AB69 RID: 43881
				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Power", "POWER") + " from generators, then provides that power to buildings.\n\nLoses charge over time.";

				// Token: 0x0400AB6A RID: 43882
				public static LocString CHARGE_LOSS = "{Battery} charge loss";
			}

			// Token: 0x0200269F RID: 9887
			public class FLYINGCREATUREBAIT
			{
				// Token: 0x0400AB6B RID: 43883
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Bait", "FLYINGCREATUREBAIT");

				// Token: 0x0400AB6C RID: 43884
				public static LocString DESC = "The type of critter attracted by critter bait depends on the construction material.";

				// Token: 0x0400AB6D RID: 43885
				public static LocString EFFECT = "Attracts one type of airborne critter.\n\nSingle use.";
			}

			// Token: 0x020026A0 RID: 9888
			public class WATERTRAP
			{
				// Token: 0x0400AB6E RID: 43886
				public static LocString NAME = UI.FormatAsLink("Fish Trap", "WATERTRAP");

				// Token: 0x0400AB6F RID: 43887
				public static LocString DESC = "Trapped fish will automatically be bagged for transport.";

				// Token: 0x0400AB70 RID: 43888
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Attracts and traps swimming ",
					UI.FormatAsLink("Pacu", "PACU"),
					".\n\nOnly Duplicants with the ",
					UI.FormatAsLink("Critter Ranching I", "RANCHING1"),
					" skill can arm this trap. It's reusable!"
				});
			}

			// Token: 0x020026A1 RID: 9889
			public class REUSABLETRAP
			{
				// Token: 0x0400AB71 RID: 43889
				public static LocString LOGIC_PORT = "Trap Occupied";

				// Token: 0x0400AB72 RID: 43890
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when a critter has been trapped";

				// Token: 0x0400AB73 RID: 43891
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400AB74 RID: 43892
				public static LocString INPUT_LOGIC_PORT = "Trap Setter";

				// Token: 0x0400AB75 RID: 43893
				public static LocString INPUT_LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set trap";

				// Token: 0x0400AB76 RID: 43894
				public static LocString INPUT_LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disarm and empty trap";
			}

			// Token: 0x020026A2 RID: 9890
			public class CREATUREAIRTRAP
			{
				// Token: 0x0400AB77 RID: 43895
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Trap", "FLYINGCREATUREBAIT");

				// Token: 0x0400AB78 RID: 43896
				public static LocString DESC = "It needs to be armed prior to use.";

				// Token: 0x0400AB79 RID: 43897
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Attracts and captures airborne ",
					UI.FormatAsLink("Critters", "CREATURES"),
					".\n\nOnly Duplicants with the ",
					UI.FormatAsLink("Critter Ranching I", "RANCHING1"),
					" skill can arm this trap. It's reusable!"
				});
			}

			// Token: 0x020026A3 RID: 9891
			public class AIRBORNECREATURELURE
			{
				// Token: 0x0400AB7A RID: 43898
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Lure", "AIRBORNECREATURELURE");

				// Token: 0x0400AB7B RID: 43899
				public static LocString DESC = "Lures can relocate Pufts or Shine Bugs to specific locations in my colony.";

				// Token: 0x0400AB7C RID: 43900
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Attracts one type of airborne critter at a time.\n\nMust be baited with ",
					UI.FormatAsLink("Slime", "SLIMEMOLD"),
					" or ",
					UI.FormatAsLink("Phosphorite", "PHOSPHORITE"),
					"."
				});
			}

			// Token: 0x020026A4 RID: 9892
			public class BATTERYMEDIUM
			{
				// Token: 0x0400AB7D RID: 43901
				public static LocString NAME = UI.FormatAsLink("Jumbo Battery", "BATTERYMEDIUM");

				// Token: 0x0400AB7E RID: 43902
				public static LocString DESC = "Larger batteries hold more power and keep systems running longer before recharging.";

				// Token: 0x0400AB7F RID: 43903
				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Power", "POWER") + " from generators, then provides that power to buildings.\n\nSlightly loses charge over time.";
			}

			// Token: 0x020026A5 RID: 9893
			public class BATTERYSMART
			{
				// Token: 0x0400AB80 RID: 43904
				public static LocString NAME = UI.FormatAsLink("Smart Battery", "BATTERYSMART");

				// Token: 0x0400AB81 RID: 43905
				public static LocString DESC = "Smart batteries send a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when they require charging.";

				// Token: 0x0400AB82 RID: 43906
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Power", "POWER"),
					" from generators, then provides that power to buildings.\n\nSends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the configuration of the Logic Activation Parameters.\n\nVery slightly loses charge over time."
				});

				// Token: 0x0400AB83 RID: 43907
				public static LocString LOGIC_PORT = "Charge Parameters";

				// Token: 0x0400AB84 RID: 43908
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when battery is less than <b>Low Threshold</b> charged, until <b>High Threshold</b> is reached again";

				// Token: 0x0400AB85 RID: 43909
				public static LocString LOGIC_PORT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when the battery is more than <b>High Threshold</b> charged, until <b>Low Threshold</b> is reached again";

				// Token: 0x0400AB86 RID: 43910
				public static LocString ACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when battery is less than <b>{0}%</b> charged, until it is <b>{1}% (High Threshold)</b> charged";

				// Token: 0x0400AB87 RID: 43911
				public static LocString DEACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when battery is <b>{0}%</b> charged, until it is less than <b>{1}% (Low Threshold)</b> charged";

				// Token: 0x0400AB88 RID: 43912
				public static LocString SIDESCREEN_TITLE = "Logic Activation Parameters";

				// Token: 0x0400AB89 RID: 43913
				public static LocString SIDESCREEN_ACTIVATE = "Low Threshold:";

				// Token: 0x0400AB8A RID: 43914
				public static LocString SIDESCREEN_DEACTIVATE = "High Threshold:";
			}

			// Token: 0x020026A6 RID: 9894
			public class BED
			{
				// Token: 0x0400AB8B RID: 43915
				public static LocString NAME = UI.FormatAsLink("Cot", "BED");

				// Token: 0x0400AB8C RID: 43916
				public static LocString DESC = "Duplicants without a bed will develop sore backs from sleeping on the floor.";

				// Token: 0x0400AB8D RID: 43917
				public static LocString EFFECT = "Gives one Duplicant a place to sleep.\n\nDuplicants will automatically return to their cots to sleep at night.";

				// Token: 0x02003548 RID: 13640
				public class FACADES
				{
					// Token: 0x02003857 RID: 14423
					public class DEFAULT_BED
					{
						// Token: 0x0400DFA1 RID: 57249
						public static LocString NAME = UI.FormatAsLink("Cot", "BED");

						// Token: 0x0400DFA2 RID: 57250
						public static LocString DESC = "A safe place to sleep.";
					}

					// Token: 0x02003858 RID: 14424
					public class STARCURTAIN
					{
						// Token: 0x0400DFA3 RID: 57251
						public static LocString NAME = UI.FormatAsLink("Stargazer Cot", "BED");

						// Token: 0x0400DFA4 RID: 57252
						public static LocString DESC = "Now Duplicants can sleep beneath the stars without wearing an Atmo Suit to bed.";
					}

					// Token: 0x02003859 RID: 14425
					public class SCIENCELAB
					{
						// Token: 0x0400DFA5 RID: 57253
						public static LocString NAME = UI.FormatAsLink("Lab Cot", "BED");

						// Token: 0x0400DFA6 RID: 57254
						public static LocString DESC = "For the Duplicant who dreams of scientific discoveries.";
					}

					// Token: 0x0200385A RID: 14426
					public class STAYCATION
					{
						// Token: 0x0400DFA7 RID: 57255
						public static LocString NAME = UI.FormatAsLink("Staycation Cot", "BED");

						// Token: 0x0400DFA8 RID: 57256
						public static LocString DESC = "Like a weekend away, except... not.";
					}

					// Token: 0x0200385B RID: 14427
					public class CREAKY
					{
						// Token: 0x0400DFA9 RID: 57257
						public static LocString NAME = UI.FormatAsLink("Camping Cot", "BED");

						// Token: 0x0400DFAA RID: 57258
						public static LocString DESC = "It's sturdier than it looks.";
					}

					// Token: 0x0200385C RID: 14428
					public class STRINGLIGHTS
					{
						// Token: 0x0400DFAB RID: 57259
						public static LocString NAME = "Good Job Cot";

						// Token: 0x0400DFAC RID: 57260
						public static LocString DESC = "Wrapped in shiny gold stars, to help sleepy Duplicants feel accomplished.";
					}
				}
			}

			// Token: 0x020026A7 RID: 9895
			public class BOTTLEEMPTIER
			{
				// Token: 0x0400AB8E RID: 43918
				public static LocString NAME = UI.FormatAsLink("Bottle Emptier", "BOTTLEEMPTIER");

				// Token: 0x0400AB8F RID: 43919
				public static LocString DESC = "A bottle emptier's Element Filter can be used to designate areas for specific liquid storage.";

				// Token: 0x0400AB90 RID: 43920
				public static LocString EFFECT = "Empties bottled " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " back into the world.";
			}

			// Token: 0x020026A8 RID: 9896
			public class BOTTLEEMPTIERGAS
			{
				// Token: 0x0400AB91 RID: 43921
				public static LocString NAME = UI.FormatAsLink("Canister Emptier", "BOTTLEEMPTIERGAS");

				// Token: 0x0400AB92 RID: 43922
				public static LocString DESC = "A canister emptier's Element Filter can designate areas for specific gas storage.";

				// Token: 0x0400AB93 RID: 43923
				public static LocString EFFECT = "Empties " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " canisters back into the world.";
			}

			// Token: 0x020026A9 RID: 9897
			public class BOTTLEEMPTIERCONDUITLIQUID
			{
				// Token: 0x0400AB94 RID: 43924
				public static LocString NAME = UI.FormatAsLink("Bottle Drainer", "BOTTLEEMPTIERCONDUITLIQUID");

				// Token: 0x0400AB95 RID: 43925
				public static LocString DESC = "A bottle drainer's Element Filter can be used to designate areas for specific liquid storage.";

				// Token: 0x0400AB96 RID: 43926
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Drains bottled ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" into ",
					UI.FormatAsLink("Liquid Pipes", "LIQUIDCONDUIT"),
					"."
				});
			}

			// Token: 0x020026AA RID: 9898
			public class BOTTLEEMPTIERCONDUITGAS
			{
				// Token: 0x0400AB97 RID: 43927
				public static LocString NAME = UI.FormatAsLink("Canister Drainer", "BOTTLEEMPTIERCONDUITGAS");

				// Token: 0x0400AB98 RID: 43928
				public static LocString DESC = "A canister drainer's Element Filter can designate areas for specific gas storage.";

				// Token: 0x0400AB99 RID: 43929
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Drains ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" canisters into ",
					UI.FormatAsLink("Gas Pipes", "GASCONDUIT"),
					"."
				});
			}

			// Token: 0x020026AB RID: 9899
			public class ARTIFACTCARGOBAY
			{
				// Token: 0x0400AB9A RID: 43930
				public static LocString NAME = UI.FormatAsLink("Artifact Transport Module", "ARTIFACTCARGOBAY");

				// Token: 0x0400AB9B RID: 43931
				public static LocString DESC = "Holds artifacts found in space.";

				// Token: 0x0400AB9C RID: 43932
				public static LocString EFFECT = "Allows Duplicants to store any artifacts they uncover during space missions.\n\nArtifacts become available to the colony upon the rocket's return. \n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".";
			}

			// Token: 0x020026AC RID: 9900
			public class CARGOBAY
			{
				// Token: 0x0400AB9D RID: 43933
				public static LocString NAME = UI.FormatAsLink("Cargo Bay", "CARGOBAY");

				// Token: 0x0400AB9E RID: 43934
				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				// Token: 0x0400AB9F RID: 43935
				public static LocString EFFECT = "Allows Duplicants to store any " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			// Token: 0x020026AD RID: 9901
			public class CARGOBAYCLUSTER
			{
				// Token: 0x0400ABA0 RID: 43936
				public static LocString NAME = UI.FormatAsLink("Large Cargo Bay", "CARGOBAY");

				// Token: 0x0400ABA1 RID: 43937
				public static LocString DESC = "Holds more than a regular cargo bay.";

				// Token: 0x0400ABA2 RID: 43938
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store most of the ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020026AE RID: 9902
			public class SOLIDCARGOBAYSMALL
			{
				// Token: 0x0400ABA3 RID: 43939
				public static LocString NAME = UI.FormatAsLink("Cargo Bay", "SOLIDCARGOBAYSMALL");

				// Token: 0x0400ABA4 RID: 43940
				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				// Token: 0x0400ABA5 RID: 43941
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store some of the ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020026AF RID: 9903
			public class SPECIALCARGOBAY
			{
				// Token: 0x0400ABA6 RID: 43942
				public static LocString NAME = UI.FormatAsLink("Biological Cargo Bay", "SPECIALCARGOBAY");

				// Token: 0x0400ABA7 RID: 43943
				public static LocString DESC = "Biological cargo bays allow Duplicants to retrieve alien plants and wildlife from space.";

				// Token: 0x0400ABA8 RID: 43944
				public static LocString EFFECT = "Allows Duplicants to store unusual or organic resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			// Token: 0x020026B0 RID: 9904
			public class SPECIALCARGOBAYCLUSTER
			{
				// Token: 0x0400ABA9 RID: 43945
				public static LocString NAME = UI.FormatAsLink("Critter Cargo Bay", "SPECIALCARGOBAY");

				// Token: 0x0400ABAA RID: 43946
				public static LocString DESC = "Critters do not require feeding during transit.";

				// Token: 0x0400ABAB RID: 43947
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to transport ",
					UI.CODEX.CATEGORYNAMES.CREATURES,
					" through space.\n\nSpecimens can be released into the colony upon the rocket's return.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});

				// Token: 0x0400ABAC RID: 43948
				public static LocString RELEASE_BTN = "Release Critter";

				// Token: 0x0400ABAD RID: 43949
				public static LocString RELEASE_BTN_TOOLTIP = "Release the critter stored inside";
			}

			// Token: 0x020026B1 RID: 9905
			public class COMMANDMODULE
			{
				// Token: 0x0400ABAE RID: 43950
				public static LocString NAME = UI.FormatAsLink("Command Capsule", "COMMANDMODULE");

				// Token: 0x0400ABAF RID: 43951
				public static LocString DESC = "At least one astronaut must be assigned to the command module to pilot a rocket.";

				// Token: 0x0400ABB0 RID: 43952
				public static LocString EFFECT = "Contains passenger seating for Duplicant " + UI.FormatAsLink("Astronauts", "ASTRONAUTING1") + ".\n\nA Command Capsule must be the last module installed at the top of a rocket.";

				// Token: 0x0400ABB1 RID: 43953
				public static LocString LOGIC_PORT_READY = "Rocket Checklist";

				// Token: 0x0400ABB2 RID: 43954
				public static LocString LOGIC_PORT_READY_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket launch checklist is complete";

				// Token: 0x0400ABB3 RID: 43955
				public static LocString LOGIC_PORT_READY_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400ABB4 RID: 43956
				public static LocString LOGIC_PORT_LAUNCH = "Launch Rocket";

				// Token: 0x0400ABB5 RID: 43957
				public static LocString LOGIC_PORT_LAUNCH_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket";

				// Token: 0x0400ABB6 RID: 43958
				public static LocString LOGIC_PORT_LAUNCH_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Awaits launch command";
			}

			// Token: 0x020026B2 RID: 9906
			public class CLUSTERCOMMANDMODULE
			{
				// Token: 0x0400ABB7 RID: 43959
				public static LocString NAME = UI.FormatAsLink("Command Capsule", "CLUSTERCOMMANDMODULE");

				// Token: 0x0400ABB8 RID: 43960
				public static LocString DESC = "";

				// Token: 0x0400ABB9 RID: 43961
				public static LocString EFFECT = "";

				// Token: 0x0400ABBA RID: 43962
				public static LocString LOGIC_PORT_READY = "Rocket Checklist";

				// Token: 0x0400ABBB RID: 43963
				public static LocString LOGIC_PORT_READY_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket launch checklist is complete";

				// Token: 0x0400ABBC RID: 43964
				public static LocString LOGIC_PORT_READY_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400ABBD RID: 43965
				public static LocString LOGIC_PORT_LAUNCH = "Launch Rocket";

				// Token: 0x0400ABBE RID: 43966
				public static LocString LOGIC_PORT_LAUNCH_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket";

				// Token: 0x0400ABBF RID: 43967
				public static LocString LOGIC_PORT_LAUNCH_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Awaits launch command";
			}

			// Token: 0x020026B3 RID: 9907
			public class CLUSTERCRAFTINTERIORDOOR
			{
				// Token: 0x0400ABC0 RID: 43968
				public static LocString NAME = UI.FormatAsLink("Interior Hatch", "CLUSTERCRAFTINTERIORDOOR");

				// Token: 0x0400ABC1 RID: 43969
				public static LocString DESC = "A hatch for getting in and out of the rocket.";

				// Token: 0x0400ABC2 RID: 43970
				public static LocString EFFECT = "Warning: Do not open mid-flight.";
			}

			// Token: 0x020026B4 RID: 9908
			public class ROBOPILOTMODULE
			{
				// Token: 0x0400ABC3 RID: 43971
				public static LocString NAME = UI.FormatAsLink("Robo-Pilot Module", "ROBOPILOTMODULE");

				// Token: 0x0400ABC4 RID: 43972
				public static LocString DESC = "Robo-pilot modules do not require a Duplicant astronaut.";

				// Token: 0x0400ABC5 RID: 43973
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Enables rockets to travel swfitly without a ",
					UI.FormatAsLink("Rocket Control Station", "ROCKETCONTROLSTATION"),
					".\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020026B5 RID: 9909
			public class ROBOPILOTCOMMANDMODULE
			{
				// Token: 0x0400ABC6 RID: 43974
				public static LocString NAME = UI.FormatAsLink("Robo-Pilot Capsule", "ROBOPILOTCOMMANDMODULE");

				// Token: 0x0400ABC7 RID: 43975
				public static LocString DESC = "Robo-pilot modules do not require a Duplicant astronaut.";

				// Token: 0x0400ABC8 RID: 43976
				public static LocString EFFECT = "Enables rockets to travel swiftly and safely without a " + UI.FormatAsLink("Command Capsule", "COMMANDMODULE") + ".\n\nA Robo-Pilot Capsule must be the last module installed at the top of a rocket.";
			}

			// Token: 0x020026B6 RID: 9910
			public class ROCKETCONTROLSTATION
			{
				// Token: 0x0400ABC9 RID: 43977
				public static LocString NAME = UI.FormatAsLink("Rocket Control Station", "ROCKETCONTROLSTATION");

				// Token: 0x0400ABCA RID: 43978
				public static LocString DESC = "Someone needs to be around to jiggle the controls when the screensaver comes on.";

				// Token: 0x0400ABCB RID: 43979
				public static LocString EFFECT = "Allows Duplicants to use pilot-operated rockets and control access to interior buildings.\n\nAssigned Duplicants must have the " + UI.FormatAsLink("Rocket Piloting", "ROCKETPILOTING1") + " skill.";

				// Token: 0x0400ABCC RID: 43980
				public static LocString LOGIC_PORT = "Restrict Building Usage";

				// Token: 0x0400ABCD RID: 43981
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Restrict access to interior buildings";

				// Token: 0x0400ABCE RID: 43982
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Unrestrict access to interior buildings";
			}

			// Token: 0x020026B7 RID: 9911
			public class RESEARCHMODULE
			{
				// Token: 0x0400ABCF RID: 43983
				public static LocString NAME = UI.FormatAsLink("Research Module", "RESEARCHMODULE");

				// Token: 0x0400ABD0 RID: 43984
				public static LocString DESC = "Data banks can be used at virtual planetariums to produce additional research.";

				// Token: 0x0400ABD1 RID: 43985
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Completes one ",
					UI.FormatAsLink("Research Task", "RESEARCH"),
					" per space mission.\n\nProduces a small Data Bank regardless of mission destination.\n\nGenerated ",
					UI.FormatAsLink("Research Points", "RESEARCH"),
					" become available upon the rocket's return."
				});
			}

			// Token: 0x020026B8 RID: 9912
			public class TOURISTMODULE
			{
				// Token: 0x0400ABD2 RID: 43986
				public static LocString NAME = UI.FormatAsLink("Sight-Seeing Module", "TOURISTMODULE");

				// Token: 0x0400ABD3 RID: 43987
				public static LocString DESC = "An astronaut must accompany sight seeing Duplicants on rocket flights.";

				// Token: 0x0400ABD4 RID: 43988
				public static LocString EFFECT = "Allows one non-Astronaut Duplicant to visit space.\n\nSight-Seeing Rocket flights decrease " + UI.FormatAsLink("Stress", "STRESS") + ".";
			}

			// Token: 0x020026B9 RID: 9913
			public class SCANNERMODULE
			{
				// Token: 0x0400ABD5 RID: 43989
				public static LocString NAME = UI.FormatAsLink("Cartographic Module", "SCANNERMODULE");

				// Token: 0x0400ABD6 RID: 43990
				public static LocString DESC = "Allows Duplicants to boldly go where other Duplicants haven't been yet.";

				// Token: 0x0400ABD7 RID: 43991
				public static LocString EFFECT = "Automatically analyzes adjacent space while on a voyage. \n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".";
			}

			// Token: 0x020026BA RID: 9914
			public class HABITATMODULESMALL
			{
				// Token: 0x0400ABD8 RID: 43992
				public static LocString NAME = UI.FormatAsLink("Solo Spacefarer Nosecone", "HABITATMODULESMALL");

				// Token: 0x0400ABD9 RID: 43993
				public static LocString DESC = "One lucky Duplicant gets the best view from the whole rocket.";

				// Token: 0x0400ABDA RID: 43994
				public static LocString EFFECT = "Functions as a Command Module and a Nosecone.\n\nHolds one Duplicant traveller.\n\nOne Command Module may be installed per rocket.\n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ". \n\nMust be built at the top of a rocket.";
			}

			// Token: 0x020026BB RID: 9915
			public class HABITATMODULEMEDIUM
			{
				// Token: 0x0400ABDB RID: 43995
				public static LocString NAME = UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM");

				// Token: 0x0400ABDC RID: 43996
				public static LocString DESC = "Allows Duplicants to survive space travel... Hopefully.";

				// Token: 0x0400ABDD RID: 43997
				public static LocString EFFECT = "Functions as a Command Module.\n\nHolds up to ten Duplicant travellers.\n\nOne Command Module may be installed per rocket. \n\nEngine must be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".";
			}

			// Token: 0x020026BC RID: 9916
			public class NOSECONEBASIC
			{
				// Token: 0x0400ABDE RID: 43998
				public static LocString NAME = UI.FormatAsLink("Basic Nosecone", "NOSECONEBASIC");

				// Token: 0x0400ABDF RID: 43999
				public static LocString DESC = "Every rocket requires a nosecone to fly.";

				// Token: 0x0400ABE0 RID: 44000
				public static LocString EFFECT = "Protects a rocket during takeoff and entry, enabling space travel.\n\nEngine must be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ". \n\nMust be built at the top of a rocket.";
			}

			// Token: 0x020026BD RID: 9917
			public class NOSECONEHARVEST
			{
				// Token: 0x0400ABE1 RID: 44001
				public static LocString NAME = UI.FormatAsLink("Drillcone", "NOSECONEHARVEST");

				// Token: 0x0400ABE2 RID: 44002
				public static LocString DESC = "Harvests resources from the universe.";

				// Token: 0x0400ABE3 RID: 44003
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Enables a rocket to drill into interstellar debris and collect ",
					UI.FormatAsLink("gas", "ELEMENTS_GAS"),
					", ",
					UI.FormatAsLink("liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("solid", "ELEMENTS_SOLID"),
					" resources from space.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nMust be built at the top of a rocket with ",
					UI.FormatAsLink("gas", "ELEMENTS_GAS"),
					", ",
					UI.FormatAsLink("liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("solid", "ELEMENTS_SOLID"),
					" Cargo Module attached to store the appropriate resources."
				});
			}

			// Token: 0x020026BE RID: 9918
			public class CO2ENGINE
			{
				// Token: 0x0400ABE4 RID: 44004
				public static LocString NAME = UI.FormatAsLink("Carbon Dioxide Engine", "CO2ENGINE");

				// Token: 0x0400ABE5 RID: 44005
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x0400ABE6 RID: 44006
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses pressurized ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" to propel rockets for short range space exploration.\n\nCarbon Dioxide Engines are relatively fast engine for their size but with limited height restrictions.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x020026BF RID: 9919
			public class KEROSENEENGINE
			{
				// Token: 0x0400ABE7 RID: 44007
				public static LocString NAME = UI.FormatAsLink("Petroleum Engine", "KEROSENEENGINE");

				// Token: 0x0400ABE8 RID: 44008
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x0400ABE9 RID: 44009
				public static LocString EFFECT = "Burns " + UI.FormatAsLink("Petroleum", "PETROLEUM") + " to propel rockets for mid-range space exploration.\n\nPetroleum Engines have generous height restrictions, ideal for hauling many modules.\n\nThe engine must be built first before more rocket modules can be added.";
			}

			// Token: 0x020026C0 RID: 9920
			public class KEROSENEENGINECLUSTER
			{
				// Token: 0x0400ABEA RID: 44010
				public static LocString NAME = UI.FormatAsLink("Petroleum Engine", "KEROSENEENGINECLUSTER");

				// Token: 0x0400ABEB RID: 44011
				public static LocString DESC = "More powerful rocket engines can propel heavier burdens.";

				// Token: 0x0400ABEC RID: 44012
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" to propel rockets for mid-range space exploration.\n\nPetroleum Engines have generous height restrictions, ideal for hauling many modules.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x020026C1 RID: 9921
			public class KEROSENEENGINECLUSTERSMALL
			{
				// Token: 0x0400ABED RID: 44013
				public static LocString NAME = UI.FormatAsLink("Small Petroleum Engine", "KEROSENEENGINECLUSTERSMALL");

				// Token: 0x0400ABEE RID: 44014
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x0400ABEF RID: 44015
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" to propel rockets for mid-range space exploration.\n\nSmall Petroleum Engines possess the same speed as a ",
					UI.FormatAsLink("Petroleum Engines", "KEROSENEENGINE"),
					" but have smaller height restrictions.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x020026C2 RID: 9922
			public class HYDROGENENGINE
			{
				// Token: 0x0400ABF0 RID: 44016
				public static LocString NAME = UI.FormatAsLink("Hydrogen Engine", "HYDROGENENGINE");

				// Token: 0x0400ABF1 RID: 44017
				public static LocString DESC = "Hydrogen engines can propel rockets further than steam or petroleum engines.";

				// Token: 0x0400ABF2 RID: 44018
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN"),
					" to propel rockets for long-range space exploration.\n\nHydrogen Engines have the same generous height restrictions as ",
					UI.FormatAsLink("Petroleum Engines", "KEROSENEENGINE"),
					" but are slightly faster.\n\nThe engine must be built first before more rocket modules can be added."
				});
			}

			// Token: 0x020026C3 RID: 9923
			public class HYDROGENENGINECLUSTER
			{
				// Token: 0x0400ABF3 RID: 44019
				public static LocString NAME = UI.FormatAsLink("Hydrogen Engine", "HYDROGENENGINECLUSTER");

				// Token: 0x0400ABF4 RID: 44020
				public static LocString DESC = "Hydrogen engines can propel rockets further than steam or petroleum engines.";

				// Token: 0x0400ABF5 RID: 44021
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN"),
					" to propel rockets for long-range space exploration.\n\nHydrogen Engines have the same generous height restrictions as ",
					UI.FormatAsLink("Petroleum Engines", "KEROSENEENGINE"),
					" but are slightly faster.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					".\n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x020026C4 RID: 9924
			public class SUGARENGINE
			{
				// Token: 0x0400ABF6 RID: 44022
				public static LocString NAME = UI.FormatAsLink("Sugar Engine", "SUGARENGINE");

				// Token: 0x0400ABF7 RID: 44023
				public static LocString DESC = "Not the most stylish way to travel space, but certainly the tastiest.";

				// Token: 0x0400ABF8 RID: 44024
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					" to propel rockets for short range space exploration.\n\nSugar Engines have higher height restrictions than ",
					UI.FormatAsLink("Carbon Dioxide Engines", "CO2ENGINE"),
					", but move slower.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x020026C5 RID: 9925
			public class HEPENGINE
			{
				// Token: 0x0400ABF9 RID: 44025
				public static LocString NAME = UI.FormatAsLink("Radbolt Engine", "HEPENGINE");

				// Token: 0x0400ABFA RID: 44026
				public static LocString DESC = "Radbolt-fueled rockets support few modules, but travel exceptionally far.";

				// Token: 0x0400ABFB RID: 44027
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Injects ",
					UI.FormatAsLink("Radbolts", "RADIATION"),
					" into a reaction chamber to propel rockets for long-range space exploration.\n\nRadbolt Engines are faster than ",
					UI.FormatAsLink("Hydrogen Engines", "HYDROGENENGINE"),
					" but with a more restrictive height allowance.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});

				// Token: 0x0400ABFC RID: 44028
				public static LocString LOGIC_PORT_STORAGE = "Radbolt Storage";

				// Token: 0x0400ABFD RID: 44029
				public static LocString LOGIC_PORT_STORAGE_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its Radbolt Storage is full";

				// Token: 0x0400ABFE RID: 44030
				public static LocString LOGIC_PORT_STORAGE_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020026C6 RID: 9926
			public class ORBITALCARGOMODULE
			{
				// Token: 0x0400ABFF RID: 44031
				public static LocString NAME = UI.FormatAsLink("Orbital Cargo Module", "ORBITALCARGOMODULE");

				// Token: 0x0400AC00 RID: 44032
				public static LocString DESC = "It's a generally good idea to pack some supplies when exploring unknown worlds.";

				// Token: 0x0400AC01 RID: 44033
				public static LocString EFFECT = "Delivers cargo to the surface of Planetoids that do not yet have a " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ". \n\nMust be built via Rocket Platform.";
			}

			// Token: 0x020026C7 RID: 9927
			public class BATTERYMODULE
			{
				// Token: 0x0400AC02 RID: 44034
				public static LocString NAME = UI.FormatAsLink("Battery Module", "BATTERYMODULE");

				// Token: 0x0400AC03 RID: 44035
				public static LocString DESC = "Charging a battery module before takeoff makes it easier to power buildings during flight.";

				// Token: 0x0400AC04 RID: 44036
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores the excess ",
					UI.FormatAsLink("Power", "POWER"),
					" generated by a Rocket Engine or ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					".\n\nProvides stored power to ",
					UI.FormatAsLink("Interior Rocket Outlets", "ROCKETINTERIORPOWERPLUG"),
					".\n\nLoses charge over time. \n\nMust be built via Rocket Platform."
				});
			}

			// Token: 0x020026C8 RID: 9928
			public class PIONEERMODULE
			{
				// Token: 0x0400AC05 RID: 44037
				public static LocString NAME = UI.FormatAsLink("Trailblazer Module", "PIONEERMODULE");

				// Token: 0x0400AC06 RID: 44038
				public static LocString DESC = "That's one small step for Dupekind.";

				// Token: 0x0400AC07 RID: 44039
				public static LocString EFFECT = "Enables travel to Planetoids that do not yet have a " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".\n\nCan hold one Duplicant traveller.\n\nDeployment is available while in a Starmap hex adjacent to a Planetoid. \n\nMust be built via Rocket Platform.";
			}

			// Token: 0x020026C9 RID: 9929
			public class SOLARPANELMODULE
			{
				// Token: 0x0400AC08 RID: 44040
				public static LocString NAME = UI.FormatAsLink("Solar Panel Module", "SOLARPANELMODULE");

				// Token: 0x0400AC09 RID: 44041
				public static LocString DESC = "Collect solar energy before takeoff and during flight.";

				// Token: 0x0400AC0A RID: 44042
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					" for use on rockets.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nMust be exposed to space."
				});
			}

			// Token: 0x020026CA RID: 9930
			public class SCOUTMODULE
			{
				// Token: 0x0400AC0B RID: 44043
				public static LocString NAME = UI.FormatAsLink("Rover's Module", "SCOUTMODULE");

				// Token: 0x0400AC0C RID: 44044
				public static LocString DESC = "Rover can conduct explorations of planetoids that don't have rocket platforms built.";

				// Token: 0x0400AC0D RID: 44045
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Deploys one ",
					UI.FormatAsLink("Rover Bot", "SCOUT"),
					" for remote Planetoid exploration.\n\nDeployment is available while in a Starmap hex adjacent to a Planetoid. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020026CB RID: 9931
			public class PIONEERLANDER
			{
				// Token: 0x0400AC0E RID: 44046
				public static LocString NAME = UI.FormatAsLink("Trailblazer Lander", "PIONEERLANDER");

				// Token: 0x0400AC0F RID: 44047
				public static LocString DESC = "Lands a Duplicant on a Planetoid from an orbiting " + BUILDINGS.PREFABS.PIONEERMODULE.NAME + ".";
			}

			// Token: 0x020026CC RID: 9932
			public class SCOUTLANDER
			{
				// Token: 0x0400AC10 RID: 44048
				public static LocString NAME = UI.FormatAsLink("Rover's Lander", "SCOUTLANDER");

				// Token: 0x0400AC11 RID: 44049
				public static LocString DESC = string.Concat(new string[]
				{
					"Lands ",
					UI.FormatAsLink("Rover", "SCOUT"),
					" on a Planetoid when ",
					BUILDINGS.PREFABS.SCOUTMODULE.NAME,
					" is in orbit."
				});
			}

			// Token: 0x020026CD RID: 9933
			public class GANTRY
			{
				// Token: 0x0400AC12 RID: 44050
				public static LocString NAME = UI.FormatAsLink("Gantry", "GANTRY");

				// Token: 0x0400AC13 RID: 44051
				public static LocString DESC = "A gantry can be built over rocket pieces where ladders and tile cannot.";

				// Token: 0x0400AC14 RID: 44052
				public static LocString EFFECT = "Provides scaffolding across rocket modules to allow Duplicant access.";

				// Token: 0x0400AC15 RID: 44053
				public static LocString LOGIC_PORT = "Extend/Retract";

				// Token: 0x0400AC16 RID: 44054
				public static LocString LOGIC_PORT_ACTIVE = "<b>Extends gantry</b> when a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " signal is received";

				// Token: 0x0400AC17 RID: 44055
				public static LocString LOGIC_PORT_INACTIVE = "<b>Retracts gantry</b> when a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " signal is received";
			}

			// Token: 0x020026CE RID: 9934
			public class ROCKETINTERIORPOWERPLUG
			{
				// Token: 0x0400AC18 RID: 44056
				public static LocString NAME = UI.FormatAsLink("Power Outlet Fitting", "ROCKETINTERIORPOWERPLUG");

				// Token: 0x0400AC19 RID: 44057
				public static LocString DESC = "Outlets conveniently power buildings inside a cockpit using their rocket's power stores.";

				// Token: 0x0400AC1A RID: 44058
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Provides ",
					UI.FormatAsLink("Power", "POWER"),
					" to connected buildings.\n\nPulls power from ",
					UI.FormatAsLink("Battery Modules", "BATTERYMODULE"),
					" and Rocket Engines.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x020026CF RID: 9935
			public class ROCKETINTERIORLIQUIDINPUT
			{
				// Token: 0x0400AC1B RID: 44059
				public static LocString NAME = UI.FormatAsLink("Liquid Intake Fitting", "ROCKETINTERIORLIQUIDINPUT");

				// Token: 0x0400AC1C RID: 44060
				public static LocString DESC = "Begone, foul waters!";

				// Token: 0x0400AC1D RID: 44061
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to be pumped into rocket storage via ",
					UI.FormatAsLink("Pipes", "LIQUIDCONDUIT"),
					".\n\nSends liquid to the first Rocket Module with available space.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x020026D0 RID: 9936
			public class ROCKETINTERIORLIQUIDOUTPUT
			{
				// Token: 0x0400AC1E RID: 44062
				public static LocString NAME = UI.FormatAsLink("Liquid Output Fitting", "ROCKETINTERIORLIQUIDOUTPUT");

				// Token: 0x0400AC1F RID: 44063
				public static LocString DESC = "Now if only we had some water balloons...";

				// Token: 0x0400AC20 RID: 44064
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to be drawn from rocket storage via ",
					UI.FormatAsLink("Pipes", "LIQUIDCONDUIT"),
					".\n\nDraws liquid from the first Rocket Module with the requested material.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x020026D1 RID: 9937
			public class ROCKETINTERIORGASINPUT
			{
				// Token: 0x0400AC21 RID: 44065
				public static LocString NAME = UI.FormatAsLink("Gas Intake Fitting", "ROCKETINTERIORGASINPUT");

				// Token: 0x0400AC22 RID: 44066
				public static LocString DESC = "It's basically central-vac.";

				// Token: 0x0400AC23 RID: 44067
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" to be pumped into rocket storage via ",
					UI.FormatAsLink("Pipes", "GASCONDUIT"),
					".\n\nSends gas to the first Rocket Module with available space.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x020026D2 RID: 9938
			public class ROCKETINTERIORGASOUTPUT
			{
				// Token: 0x0400AC24 RID: 44068
				public static LocString NAME = UI.FormatAsLink("Gas Output Fitting", "ROCKETINTERIORGASOUTPUT");

				// Token: 0x0400AC25 RID: 44069
				public static LocString DESC = "Refreshing breezes, on-demand.";

				// Token: 0x0400AC26 RID: 44070
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" to be drawn from rocket storage via ",
					UI.FormatAsLink("Pipes", "GASCONDUIT"),
					".\n\nDraws gas from the first Rocket Module with the requested material.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x020026D3 RID: 9939
			public class ROCKETINTERIORSOLIDINPUT
			{
				// Token: 0x0400AC27 RID: 44071
				public static LocString NAME = UI.FormatAsLink("Conveyor Receptacle Fitting", "ROCKETINTERIORSOLIDINPUT");

				// Token: 0x0400AC28 RID: 44072
				public static LocString DESC = "Why organize your shelves when you can just shove everything in here?";

				// Token: 0x0400AC29 RID: 44073
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" to be moved into rocket storage via ",
					UI.FormatAsLink("Conveyor Rails", "SOLIDCONDUIT"),
					".\n\nSends solid material to the first Rocket Module with available space.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x020026D4 RID: 9940
			public class ROCKETINTERIORSOLIDOUTPUT
			{
				// Token: 0x0400AC2A RID: 44074
				public static LocString NAME = UI.FormatAsLink("Conveyor Loader Fitting", "ROCKETINTERIORSOLIDOUTPUT");

				// Token: 0x0400AC2B RID: 44075
				public static LocString DESC = "For accessing your stored luggage mid-flight.";

				// Token: 0x0400AC2C RID: 44076
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" to be moved out of rocket storage via ",
					UI.FormatAsLink("Conveyor Rails", "SOLIDCONDUIT"),
					".\n\nDraws solid material from the first Rocket Module with the requested material.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x020026D5 RID: 9941
			public class WATERCOOLER
			{
				// Token: 0x0400AC2D RID: 44077
				public static LocString NAME = UI.FormatAsLink("Water Cooler", "WATERCOOLER");

				// Token: 0x0400AC2E RID: 44078
				public static LocString DESC = "Chatting with friends improves Duplicants' moods and reduces their stress.";

				// Token: 0x0400AC2F RID: 44079
				public static LocString EFFECT = "Provides a gathering place for Duplicants during Downtime.\n\nImproves Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";

				// Token: 0x02003549 RID: 13641
				public class OPTION_TOOLTIPS
				{
					// Token: 0x0400D7DC RID: 55260
					public static LocString WATER = ELEMENTS.WATER.NAME + "\nPlain potable water";

					// Token: 0x0400D7DD RID: 55261
					public static LocString MILK = ELEMENTS.MILK.NAME + "\nA salty, green-hued beverage";
				}

				// Token: 0x0200354A RID: 13642
				public class FACADES
				{
					// Token: 0x0200385D RID: 14429
					public class DEFAULT_WATERCOOLER
					{
						// Token: 0x0400DFAD RID: 57261
						public static LocString NAME = UI.FormatAsLink("Water Cooler", "WATERCOOLER");

						// Token: 0x0400DFAE RID: 57262
						public static LocString DESC = "Where Duplicants sip and socialize.";
					}

					// Token: 0x0200385E RID: 14430
					public class ROUND_BODY
					{
						// Token: 0x0400DFAF RID: 57263
						public static LocString NAME = UI.FormatAsLink("Elegant Water Cooler", "WATERCOOLER");

						// Token: 0x0400DFB0 RID: 57264
						public static LocString DESC = "It really classes up a breakroom.";
					}

					// Token: 0x0200385F RID: 14431
					public class BALLOON
					{
						// Token: 0x0400DFB1 RID: 57265
						public static LocString NAME = UI.FormatAsLink("Inflatable Water Cooler", "WATERCOOLER");

						// Token: 0x0400DFB2 RID: 57266
						public static LocString DESC = "There's a funny aftertaste.";
					}

					// Token: 0x02003860 RID: 14432
					public class YELLOW_TARTAR
					{
						// Token: 0x0400DFB3 RID: 57267
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Water Cooler", "WATERCOOLER");

						// Token: 0x0400DFB4 RID: 57268
						public static LocString DESC = "Did someone boil eggs in this water?";
					}

					// Token: 0x02003861 RID: 14433
					public class RED_ROSE
					{
						// Token: 0x0400DFB5 RID: 57269
						public static LocString NAME = UI.FormatAsLink("Puce Pink Water Cooler", "WATERCOOLER");

						// Token: 0x0400DFB6 RID: 57270
						public static LocString DESC = "Rose-colored paper cups: the shatter-proof alternative to rose-colored glasses.";
					}

					// Token: 0x02003862 RID: 14434
					public class GREEN_MUSH
					{
						// Token: 0x0400DFB7 RID: 57271
						public static LocString NAME = UI.FormatAsLink("Mush Green Water Cooler", "WATERCOOLER");

						// Token: 0x0400DFB8 RID: 57272
						public static LocString DESC = "Ideal for post-Mush Bar palate cleansing.";
					}

					// Token: 0x02003863 RID: 14435
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400DFB9 RID: 57273
						public static LocString NAME = UI.FormatAsLink("Faint Purple Water Cooler", "WATERCOOLER");

						// Token: 0x0400DFBA RID: 57274
						public static LocString DESC = "Most Duplicants agree that it really should dispense juice.";
					}

					// Token: 0x02003864 RID: 14436
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400DFBB RID: 57275
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Water Cooler", "WATERCOOLER");

						// Token: 0x0400DFBC RID: 57276
						public static LocString DESC = "Lightly salted with Duplicants' tears.";
					}
				}
			}

			// Token: 0x020026D6 RID: 9942
			public class ARCADEMACHINE
			{
				// Token: 0x0400AC30 RID: 44080
				public static LocString NAME = UI.FormatAsLink("Arcade Cabinet", "ARCADEMACHINE");

				// Token: 0x0400AC31 RID: 44081
				public static LocString DESC = "Komet Kablam-O!\nFor up to two players.";

				// Token: 0x0400AC32 RID: 44082
				public static LocString EFFECT = "Allows Duplicants to play video games on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x020026D7 RID: 9943
			public class SINGLEPLAYERARCADE
			{
				// Token: 0x0400AC33 RID: 44083
				public static LocString NAME = UI.FormatAsLink("Single Player Arcade", "SINGLEPLAYERARCADE");

				// Token: 0x0400AC34 RID: 44084
				public static LocString DESC = "Space Brawler IV! For one player.";

				// Token: 0x0400AC35 RID: 44085
				public static LocString EFFECT = "Allows a Duplicant to play video games solo on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x020026D8 RID: 9944
			public class PHONOBOX
			{
				// Token: 0x0400AC36 RID: 44086
				public static LocString NAME = UI.FormatAsLink("Jukebot", "PHONOBOX");

				// Token: 0x0400AC37 RID: 44087
				public static LocString DESC = "Dancing helps Duplicants get their innermost feelings out.";

				// Token: 0x0400AC38 RID: 44088
				public static LocString EFFECT = "Plays music for Duplicants to dance to on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x020026D9 RID: 9945
			public class JUICER
			{
				// Token: 0x0400AC39 RID: 44089
				public static LocString NAME = UI.FormatAsLink("Juicer", "JUICER");

				// Token: 0x0400AC3A RID: 44090
				public static LocString DESC = "Fruity juice can really brighten a Duplicant's breaktime";

				// Token: 0x0400AC3B RID: 44091
				public static LocString EFFECT = "Provides refreshment for Duplicants on their breaks.\n\nDrinking juice increases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x020026DA RID: 9946
			public class ESPRESSOMACHINE
			{
				// Token: 0x0400AC3C RID: 44092
				public static LocString NAME = UI.FormatAsLink("Espresso Machine", "ESPRESSOMACHINE");

				// Token: 0x0400AC3D RID: 44093
				public static LocString DESC = "A shot of espresso helps Duplicants relax after a long day.";

				// Token: 0x0400AC3E RID: 44094
				public static LocString EFFECT = "Provides refreshment for Duplicants on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x020026DB RID: 9947
			public class TELEPHONE
			{
				// Token: 0x0400AC3F RID: 44095
				public static LocString NAME = UI.FormatAsLink("Party Line Phone", "TELEPHONE");

				// Token: 0x0400AC40 RID: 44096
				public static LocString DESC = "You never know who you'll meet on the other line.";

				// Token: 0x0400AC41 RID: 44097
				public static LocString EFFECT = "Can be used by one Duplicant to chat with themselves or with other Duplicants in different locations.\n\nChatting increases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";

				// Token: 0x0400AC42 RID: 44098
				public static LocString EFFECT_BABBLE = "{attrib}: {amount} (No One)";

				// Token: 0x0400AC43 RID: 44099
				public static LocString EFFECT_BABBLE_TOOLTIP = "Duplicants will gain {amount} {attrib} if they chat only with themselves.";

				// Token: 0x0400AC44 RID: 44100
				public static LocString EFFECT_CHAT = "{attrib}: {amount} (At least one Duplicant)";

				// Token: 0x0400AC45 RID: 44101
				public static LocString EFFECT_CHAT_TOOLTIP = "Duplicants will gain {amount} {attrib} if they chat with at least one other Duplicant.";

				// Token: 0x0400AC46 RID: 44102
				public static LocString EFFECT_LONG_DISTANCE = "{attrib}: {amount} (At least one Duplicant across space)";

				// Token: 0x0400AC47 RID: 44103
				public static LocString EFFECT_LONG_DISTANCE_TOOLTIP = "Duplicants will gain {amount} {attrib} if they chat with at least one other Duplicant across space.";
			}

			// Token: 0x020026DC RID: 9948
			public class MODULARLIQUIDINPUT
			{
				// Token: 0x0400AC48 RID: 44104
				public static LocString NAME = UI.FormatAsLink("Liquid Input Hub", "MODULARLIQUIDINPUT");

				// Token: 0x0400AC49 RID: 44105
				public static LocString DESC = "A hub from which to input " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + ".";
			}

			// Token: 0x020026DD RID: 9949
			public class MODULARSOLIDINPUT
			{
				// Token: 0x0400AC4A RID: 44106
				public static LocString NAME = UI.FormatAsLink("Solid Input Hub", "MODULARSOLIDINPUT");

				// Token: 0x0400AC4B RID: 44107
				public static LocString DESC = "A hub from which to input " + UI.FormatAsLink("Solids", "ELEMENTS_SOLID") + ".";
			}

			// Token: 0x020026DE RID: 9950
			public class MODULARGASINPUT
			{
				// Token: 0x0400AC4C RID: 44108
				public static LocString NAME = UI.FormatAsLink("Gas Input Hub", "MODULARGASINPUT");

				// Token: 0x0400AC4D RID: 44109
				public static LocString DESC = "A hub from which to input " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".";
			}

			// Token: 0x020026DF RID: 9951
			public class MECHANICALSURFBOARD
			{
				// Token: 0x0400AC4E RID: 44110
				public static LocString NAME = UI.FormatAsLink("Mechanical Surfboard", "MECHANICALSURFBOARD");

				// Token: 0x0400AC4F RID: 44111
				public static LocString DESC = "Mechanical waves make for radical relaxation time.";

				// Token: 0x0400AC50 RID: 44112
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nSome ",
					UI.FormatAsLink("Water", "WATER"),
					" gets splashed on the floor during use."
				});

				// Token: 0x0400AC51 RID: 44113
				public static LocString WATER_REQUIREMENT = "{element}: {amount}";

				// Token: 0x0400AC52 RID: 44114
				public static LocString WATER_REQUIREMENT_TOOLTIP = "This building must be filled with {amount} {element} in order to function.";

				// Token: 0x0400AC53 RID: 44115
				public static LocString LEAK_REQUIREMENT = "Spillage: {amount}";

				// Token: 0x0400AC54 RID: 44116
				public static LocString LEAK_REQUIREMENT_TOOLTIP = "This building will spill {amount} of its contents on to the floor during use, which must be replenished.";
			}

			// Token: 0x020026E0 RID: 9952
			public class SAUNA
			{
				// Token: 0x0400AC55 RID: 44117
				public static LocString NAME = UI.FormatAsLink("Sauna", "SAUNA");

				// Token: 0x0400AC56 RID: 44118
				public static LocString DESC = "A steamy sauna soothes away all the aches and pains.";

				// Token: 0x0400AC57 RID: 44119
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Steam", "STEAM"),
					" to create a relaxing atmosphere.\n\nIncreases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					" and provides a lingering sense of warmth."
				});
			}

			// Token: 0x020026E1 RID: 9953
			public class BEACHCHAIR
			{
				// Token: 0x0400AC58 RID: 44120
				public static LocString NAME = UI.FormatAsLink("Beach Chair", "BEACHCHAIR");

				// Token: 0x0400AC59 RID: 44121
				public static LocString DESC = "Soak up some relaxing sun rays.";

				// Token: 0x0400AC5A RID: 44122
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Duplicants can relax by lounging in ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					".\n\nIncreases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x0400AC5B RID: 44123
				public static LocString LIGHTEFFECT_LOW = "{attrib}: {amount} (Dim Light)";

				// Token: 0x0400AC5C RID: 44124
				public static LocString LIGHTEFFECT_LOW_TOOLTIP = "Duplicants will gain {amount} {attrib} if this building is in light dimmer than {lux}.";

				// Token: 0x0400AC5D RID: 44125
				public static LocString LIGHTEFFECT_HIGH = "{attrib}: {amount} (Bright Light)";

				// Token: 0x0400AC5E RID: 44126
				public static LocString LIGHTEFFECT_HIGH_TOOLTIP = "Duplicants will gain {amount} {attrib} if this building is in at least {lux} light.";
			}

			// Token: 0x020026E2 RID: 9954
			public class SUNLAMP
			{
				// Token: 0x0400AC5F RID: 44127
				public static LocString NAME = UI.FormatAsLink("Sun Lamp", "SUNLAMP");

				// Token: 0x0400AC60 RID: 44128
				public static LocString DESC = "An artificial ray of sunshine.";

				// Token: 0x0400AC61 RID: 44129
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Gives off ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					" level Lux.\n\nCan be paired with ",
					UI.FormatAsLink("Beach Chairs", "BEACHCHAIR"),
					"."
				});
			}

			// Token: 0x020026E3 RID: 9955
			public class VERTICALWINDTUNNEL
			{
				// Token: 0x0400AC62 RID: 44130
				public static LocString NAME = UI.FormatAsLink("Vertical Wind Tunnel", "VERTICALWINDTUNNEL");

				// Token: 0x0400AC63 RID: 44131
				public static LocString DESC = "Duplicants love the feeling of high-powered wind through their hair.";

				// Token: 0x0400AC64 RID: 44132
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Must be connected to a ",
					UI.FormatAsLink("Power Source", "POWER"),
					". To properly function, the area under this building must be left vacant.\n\nIncreases Duplicants ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x0400AC65 RID: 44133
				public static LocString DISPLACEMENTEFFECT = "Gas Displacement: {amount}";

				// Token: 0x0400AC66 RID: 44134
				public static LocString DISPLACEMENTEFFECT_TOOLTIP = "This building will displace {amount} Gas while in use.";
			}

			// Token: 0x020026E4 RID: 9956
			public class TELEPORTALPAD
			{
				// Token: 0x0400AC67 RID: 44135
				public static LocString NAME = "Teleporter Pad";

				// Token: 0x0400AC68 RID: 44136
				public static LocString DESC = "Duplicants are just atoms as far as the pad's concerned.";

				// Token: 0x0400AC69 RID: 44137
				public static LocString EFFECT = "Instantly transports Duplicants and items to another portal with the same portal code.";

				// Token: 0x0400AC6A RID: 44138
				public static LocString LOGIC_PORT = "Portal Code Input";

				// Token: 0x0400AC6B RID: 44139
				public static LocString LOGIC_PORT_ACTIVE = "1";

				// Token: 0x0400AC6C RID: 44140
				public static LocString LOGIC_PORT_INACTIVE = "0";
			}

			// Token: 0x020026E5 RID: 9957
			public class CHECKPOINT
			{
				// Token: 0x0400AC6D RID: 44141
				public static LocString NAME = UI.FormatAsLink("Duplicant Checkpoint", "CHECKPOINT");

				// Token: 0x0400AC6E RID: 44142
				public static LocString DESC = "Checkpoints can be connected to automated sensors to determine when it's safe to enter.";

				// Token: 0x0400AC6F RID: 44143
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to pass when receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					".\n\nPrevents Duplicants from passing when receiving a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					"."
				});

				// Token: 0x0400AC70 RID: 44144
				public static LocString LOGIC_PORT = "Duplicant Stop/Go";

				// Token: 0x0400AC71 RID: 44145
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow Duplicant passage";

				// Token: 0x0400AC72 RID: 44146
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent Duplicant passage";
			}

			// Token: 0x020026E6 RID: 9958
			public class FIREPOLE
			{
				// Token: 0x0400AC73 RID: 44147
				public static LocString NAME = UI.FormatAsLink("Fire Pole", "FIREPOLE");

				// Token: 0x0400AC74 RID: 44148
				public static LocString DESC = "Build these in addition to ladders for efficient upward and downward movement.";

				// Token: 0x0400AC75 RID: 44149
				public static LocString EFFECT = "Allows rapid Duplicant descent.\n\nSignificantly slows upward climbing.";
			}

			// Token: 0x020026E7 RID: 9959
			public class FLOORSWITCH
			{
				// Token: 0x0400AC76 RID: 44150
				public static LocString NAME = UI.FormatAsLink("Weight Plate", "FLOORSWITCH");

				// Token: 0x0400AC77 RID: 44151
				public static LocString DESC = "Weight plates can be used to turn on amenities only when Duplicants pass by.";

				// Token: 0x0400AC78 RID: 44152
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when an object or Duplicant is placed atop of it.\n\nCannot be triggered by ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" or ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					"."
				});

				// Token: 0x0400AC79 RID: 44153
				public static LocString LOGIC_PORT_DESC = UI.FormatAsLink("Active", "LOGIC") + "/" + UI.FormatAsLink("Inactive", "LOGIC");
			}

			// Token: 0x020026E8 RID: 9960
			public class KILN
			{
				// Token: 0x0400AC7A RID: 44154
				public static LocString NAME = UI.FormatAsLink("Kiln", "KILN");

				// Token: 0x0400AC7B RID: 44155
				public static LocString DESC = "Kilns can also be used to refine coal into pure carbon.";

				// Token: 0x0400AC7C RID: 44156
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Fires ",
					UI.FormatAsLink("Clay", "CLAY"),
					" to produce ",
					UI.FormatAsLink("Ceramic", "CERAMIC"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			// Token: 0x020026E9 RID: 9961
			public class LIQUIDFUELTANK
			{
				// Token: 0x0400AC7D RID: 44157
				public static LocString NAME = UI.FormatAsLink("Liquid Fuel Tank", "LIQUIDFUELTANK");

				// Token: 0x0400AC7E RID: 44158
				public static LocString DESC = "Storing additional fuel increases the distance a rocket can travel before returning.";

				// Token: 0x0400AC7F RID: 44159
				public static LocString EFFECT = "Stores the " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " fuel piped into it to supply rocket engines.\n\nThe stored fuel type is determined by the rocket engine it is built upon.";
			}

			// Token: 0x020026EA RID: 9962
			public class LIQUIDFUELTANKCLUSTER
			{
				// Token: 0x0400AC80 RID: 44160
				public static LocString NAME = UI.FormatAsLink("Large Liquid Fuel Tank", "LIQUIDFUELTANKCLUSTER");

				// Token: 0x0400AC81 RID: 44161
				public static LocString DESC = "Storing additional fuel increases the distance a rocket can travel before returning.";

				// Token: 0x0400AC82 RID: 44162
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" fuel piped into it to supply rocket engines.\n\nThe stored fuel type is determined by the rocket engine it is built upon. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020026EB RID: 9963
			public class LANDING_POD
			{
				// Token: 0x0400AC83 RID: 44163
				public static LocString NAME = "Spacefarer Deploy Pod";

				// Token: 0x0400AC84 RID: 44164
				public static LocString DESC = "Geronimo!";

				// Token: 0x0400AC85 RID: 44165
				public static LocString EFFECT = "Contains a Duplicant deployed from orbit.\n\nPod will disintegrate on arrival.";
			}

			// Token: 0x020026EC RID: 9964
			public class ROCKETPOD
			{
				// Token: 0x0400AC86 RID: 44166
				public static LocString NAME = UI.FormatAsLink("Trailblazer Deploy Pod", "ROCKETPOD");

				// Token: 0x0400AC87 RID: 44167
				public static LocString DESC = "The Duplicant inside is equal parts nervous and excited.";

				// Token: 0x0400AC88 RID: 44168
				public static LocString EFFECT = "Contains a Duplicant deployed from orbit by a " + BUILDINGS.PREFABS.PIONEERMODULE.NAME + ".\n\nPod will disintegrate on arrival.";
			}

			// Token: 0x020026ED RID: 9965
			public class SCOUTROCKETPOD
			{
				// Token: 0x0400AC89 RID: 44169
				public static LocString NAME = UI.FormatAsLink("Rover's Doghouse", "SCOUTROCKETPOD");

				// Token: 0x0400AC8A RID: 44170
				public static LocString DESC = "Good luck out there, boy!";

				// Token: 0x0400AC8B RID: 44171
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Contains a ",
					UI.FormatAsLink("Rover", "SCOUT"),
					" deployed from an orbiting ",
					BUILDINGS.PREFABS.SCOUTMODULE.NAME,
					".\n\nPod will disintegrate on arrival."
				});
			}

			// Token: 0x020026EE RID: 9966
			public class ROCKETCOMMANDCONSOLE
			{
				// Token: 0x0400AC8C RID: 44172
				public static LocString NAME = UI.FormatAsLink("Rocket Cockpit", "ROCKETCOMMANDCONSOLE");

				// Token: 0x0400AC8D RID: 44173
				public static LocString DESC = "Looks kinda fun.";

				// Token: 0x0400AC8E RID: 44174
				public static LocString EFFECT = "Allows a Duplicant to pilot a rocket.\n\nCargo rockets must possess a Rocket Cockpit in order to function.";
			}

			// Token: 0x020026EF RID: 9967
			public class ROCKETENVELOPETILE
			{
				// Token: 0x0400AC8F RID: 44175
				public static LocString NAME = UI.FormatAsLink("Rocket", "ROCKETENVELOPETILE");

				// Token: 0x0400AC90 RID: 44176
				public static LocString DESC = "Keeps the space out.";

				// Token: 0x0400AC91 RID: 44177
				public static LocString EFFECT = "The walls of a rocket.";
			}

			// Token: 0x020026F0 RID: 9968
			public class ROCKETENVELOPEWINDOWTILE
			{
				// Token: 0x0400AC92 RID: 44178
				public static LocString NAME = UI.FormatAsLink("Rocket Window", "ROCKETENVELOPEWINDOWTILE");

				// Token: 0x0400AC93 RID: 44179
				public static LocString DESC = "I can see my asteroid from here!";

				// Token: 0x0400AC94 RID: 44180
				public static LocString EFFECT = "The window of a rocket.";
			}

			// Token: 0x020026F1 RID: 9969
			public class ROCKETWALLTILE
			{
				// Token: 0x0400AC95 RID: 44181
				public static LocString NAME = UI.FormatAsLink("Rocket Wall", "ROCKETENVELOPETILE");

				// Token: 0x0400AC96 RID: 44182
				public static LocString DESC = "Keeps the space out.";

				// Token: 0x0400AC97 RID: 44183
				public static LocString EFFECT = "The walls of a rocket.";
			}

			// Token: 0x020026F2 RID: 9970
			public class SMALLOXIDIZERTANK
			{
				// Token: 0x0400AC98 RID: 44184
				public static LocString NAME = UI.FormatAsLink("Small Solid Oxidizer Tank", "SMALLOXIDIZERTANK");

				// Token: 0x0400AC99 RID: 44185
				public static LocString DESC = "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";

				// Token: 0x0400AC9A RID: 44186
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Fertilizer", "Fertilizer"),
					" and ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" for burning rocket fuels. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});

				// Token: 0x0400AC9B RID: 44187
				public static LocString UI_FILTER_CATEGORY = "Accepted Oxidizers";
			}

			// Token: 0x020026F3 RID: 9971
			public class OXIDIZERTANK
			{
				// Token: 0x0400AC9C RID: 44188
				public static LocString NAME = UI.FormatAsLink("Solid Oxidizer Tank", "OXIDIZERTANK");

				// Token: 0x0400AC9D RID: 44189
				public static LocString DESC = "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";

				// Token: 0x0400AC9E RID: 44190
				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Oxylite", "OXYROCK") + " and other oxidizers for burning rocket fuels.";

				// Token: 0x0400AC9F RID: 44191
				public static LocString UI_FILTER_CATEGORY = "Accepted Oxidizers";
			}

			// Token: 0x020026F4 RID: 9972
			public class OXIDIZERTANKCLUSTER
			{
				// Token: 0x0400ACA0 RID: 44192
				public static LocString NAME = UI.FormatAsLink("Large Solid Oxidizer Tank", "OXIDIZERTANKCLUSTER");

				// Token: 0x0400ACA1 RID: 44193
				public static LocString DESC = "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";

				// Token: 0x0400ACA2 RID: 44194
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" and other oxidizers for burning rocket fuels.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});

				// Token: 0x0400ACA3 RID: 44195
				public static LocString UI_FILTER_CATEGORY = "Accepted Oxidizers";
			}

			// Token: 0x020026F5 RID: 9973
			public class OXIDIZERTANKLIQUID
			{
				// Token: 0x0400ACA4 RID: 44196
				public static LocString NAME = UI.FormatAsLink("Liquid Oxidizer Tank", "OXIDIZERTANKLIQUID");

				// Token: 0x0400ACA5 RID: 44197
				public static LocString DESC = "Liquid oxygen improves the thrust-to-mass ratio of rocket fuels.";

				// Token: 0x0400ACA6 RID: 44198
				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN") + " for burning rocket fuels.";
			}

			// Token: 0x020026F6 RID: 9974
			public class OXIDIZERTANKLIQUIDCLUSTER
			{
				// Token: 0x0400ACA7 RID: 44199
				public static LocString NAME = UI.FormatAsLink("Liquid Oxidizer Tank", "OXIDIZERTANKLIQUIDCLUSTER");

				// Token: 0x0400ACA8 RID: 44200
				public static LocString DESC = "Liquid oxygen improves the thrust-to-mass ratio of rocket fuels.";

				// Token: 0x0400ACA9 RID: 44201
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN"),
					" for burning rocket fuels. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020026F7 RID: 9975
			public class LIQUIDCONDITIONER
			{
				// Token: 0x0400ACAA RID: 44202
				public static LocString NAME = UI.FormatAsLink("Thermo Aquatuner", "LIQUIDCONDITIONER");

				// Token: 0x0400ACAB RID: 44203
				public static LocString DESC = "A thermo aquatuner cools liquid and outputs the heat elsewhere.";

				// Token: 0x0400ACAC RID: 44204
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Cools the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" piped through it, but outputs ",
					UI.FormatAsLink("Heat", "HEAT"),
					" in its immediate vicinity."
				});
			}

			// Token: 0x020026F8 RID: 9976
			public class LIQUIDCARGOBAY
			{
				// Token: 0x0400ACAD RID: 44205
				public static LocString NAME = UI.FormatAsLink("Liquid Cargo Tank", "LIQUIDCARGOBAY");

				// Token: 0x0400ACAE RID: 44206
				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				// Token: 0x0400ACAF RID: 44207
				public static LocString EFFECT = "Allows Duplicants to store any " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			// Token: 0x020026F9 RID: 9977
			public class LIQUIDCARGOBAYCLUSTER
			{
				// Token: 0x0400ACB0 RID: 44208
				public static LocString NAME = UI.FormatAsLink("Large Liquid Cargo Tank", "LIQUIDCARGOBAY");

				// Token: 0x0400ACB1 RID: 44209
				public static LocString DESC = "Holds more than a regular cargo tank.";

				// Token: 0x0400ACB2 RID: 44210
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store most of the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020026FA RID: 9978
			public class LIQUIDCARGOBAYSMALL
			{
				// Token: 0x0400ACB3 RID: 44211
				public static LocString NAME = UI.FormatAsLink("Liquid Cargo Tank", "LIQUIDCARGOBAYSMALL");

				// Token: 0x0400ACB4 RID: 44212
				public static LocString DESC = "Duplicants will fill cargo tanks with whatever resources they find during space missions.";

				// Token: 0x0400ACB5 RID: 44213
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store some of the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020026FB RID: 9979
			public class LUXURYBED
			{
				// Token: 0x0400ACB6 RID: 44214
				public static LocString NAME = UI.FormatAsLink("Comfy Bed", "LUXURYBED");

				// Token: 0x0400ACB7 RID: 44215
				public static LocString DESC = "Duplicants prefer comfy beds to cots and wake up more rested after sleeping in them.";

				// Token: 0x0400ACB8 RID: 44216
				public static LocString EFFECT = "Provides a sleeping area for one Duplicant and restores additional stamina.\n\nDuplicants will automatically sleep in their assigned beds at night.";

				// Token: 0x0200354B RID: 13643
				public class FACADES
				{
					// Token: 0x02003865 RID: 14437
					public class DEFAULT_LUXURYBED
					{
						// Token: 0x0400DFBD RID: 57277
						public static LocString NAME = UI.FormatAsLink("Comfy Bed", "LUXURYBED");

						// Token: 0x0400DFBE RID: 57278
						public static LocString DESC = "Much comfier than a cot.";
					}

					// Token: 0x02003866 RID: 14438
					public class GRANDPRIX
					{
						// Token: 0x0400DFBF RID: 57279
						public static LocString NAME = UI.FormatAsLink("Grand Prix Bed", "LUXURYBED");

						// Token: 0x0400DFC0 RID: 57280
						public static LocString DESC = "Where every Duplicant wakes up a winner.";
					}

					// Token: 0x02003867 RID: 14439
					public class BOAT
					{
						// Token: 0x0400DFC1 RID: 57281
						public static LocString NAME = UI.FormatAsLink("Dreamboat Bed", "LUXURYBED");

						// Token: 0x0400DFC2 RID: 57282
						public static LocString DESC = "Ahoy! Set sail for zzzzz's.";
					}

					// Token: 0x02003868 RID: 14440
					public class ROCKET_BED
					{
						// Token: 0x0400DFC3 RID: 57283
						public static LocString NAME = UI.FormatAsLink("S.S. Napmaster Bed", "LUXURYBED");

						// Token: 0x0400DFC4 RID: 57284
						public static LocString DESC = "Launches sleepy Duplicants into a deep-space slumber.";
					}

					// Token: 0x02003869 RID: 14441
					public class BOUNCY_BED
					{
						// Token: 0x0400DFC5 RID: 57285
						public static LocString NAME = UI.FormatAsLink("Bouncy Castle Bed", "LUXURYBED");

						// Token: 0x0400DFC6 RID: 57286
						public static LocString DESC = "An inflatable party prop makes a surprisingly good bed.";
					}

					// Token: 0x0200386A RID: 14442
					public class PUFT_BED
					{
						// Token: 0x0400DFC7 RID: 57287
						public static LocString NAME = UI.FormatAsLink("Puft Bed", "LUXURYBED");

						// Token: 0x0400DFC8 RID: 57288
						public static LocString DESC = "A comfy, if somewhat 'fragrant', place to sleep.";
					}

					// Token: 0x0200386B RID: 14443
					public class HAND
					{
						// Token: 0x0400DFC9 RID: 57289
						public static LocString NAME = UI.FormatAsLink("Cradled Bed", "LUXURYBED");

						// Token: 0x0400DFCA RID: 57290
						public static LocString DESC = "It's so nice to be held.";
					}

					// Token: 0x0200386C RID: 14444
					public class RUBIKS
					{
						// Token: 0x0400DFCB RID: 57291
						public static LocString NAME = UI.FormatAsLink("Puzzle Cube Bed", "LUXURYBED");

						// Token: 0x0400DFCC RID: 57292
						public static LocString DESC = "A little pattern recognition at bedtime soothes the mind.";
					}

					// Token: 0x0200386D RID: 14445
					public class RED_ROSE
					{
						// Token: 0x0400DFCD RID: 57293
						public static LocString NAME = UI.FormatAsLink("Comfy Puce Bed", "LUXURYBED");

						// Token: 0x0400DFCE RID: 57294
						public static LocString DESC = "A pink-hued bed for rosy dreams.";
					}

					// Token: 0x0200386E RID: 14446
					public class GREEN_MUSH
					{
						// Token: 0x0400DFCF RID: 57295
						public static LocString NAME = UI.FormatAsLink("Comfy Mush Bed", "LUXURYBED");

						// Token: 0x0400DFD0 RID: 57296
						public static LocString DESC = "The mattress is so soft, it's almost impossible to climb out of.";
					}

					// Token: 0x0200386F RID: 14447
					public class YELLOW_TARTAR
					{
						// Token: 0x0400DFD1 RID: 57297
						public static LocString NAME = UI.FormatAsLink("Comfy Ick Bed", "LUXURYBED");

						// Token: 0x0400DFD2 RID: 57298
						public static LocString DESC = "When life is icky, bed rest is the only answer.";
					}

					// Token: 0x02003870 RID: 14448
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400DFD3 RID: 57299
						public static LocString NAME = UI.FormatAsLink("Comfy Fainting Bed", "LUXURYBED");

						// Token: 0x0400DFD4 RID: 57300
						public static LocString DESC = "A soft landing spot for swooners.";
					}
				}
			}

			// Token: 0x020026FC RID: 9980
			public class LADDERBED
			{
				// Token: 0x0400ACB9 RID: 44217
				public static LocString NAME = UI.FormatAsLink("Ladder Bed", "LADDERBED");

				// Token: 0x0400ACBA RID: 44218
				public static LocString DESC = "Duplicant's sleep will be interrupted if another Duplicant uses the ladder.";

				// Token: 0x0400ACBB RID: 44219
				public static LocString EFFECT = "Provides a sleeping area for one Duplicant and also functions as a ladder.\n\nDuplicants will automatically sleep in their assigned beds at night.";
			}

			// Token: 0x020026FD RID: 9981
			public class MEDICALCOT
			{
				// Token: 0x0400ACBC RID: 44220
				public static LocString NAME = UI.FormatAsLink("Triage Cot", "MEDICALCOT");

				// Token: 0x0400ACBD RID: 44221
				public static LocString DESC = "Duplicants use triage cots to recover from physical injuries and receive aid from peers.";

				// Token: 0x0400ACBE RID: 44222
				public static LocString EFFECT = "Accelerates " + UI.FormatAsLink("Health", "HEALTH") + " restoration and the healing of physical injuries.\n\nRevives incapacitated Duplicants.";
			}

			// Token: 0x020026FE RID: 9982
			public class DOCTORSTATION
			{
				// Token: 0x0400ACBF RID: 44223
				public static LocString NAME = UI.FormatAsLink("Sick Bay", "DOCTORSTATION");

				// Token: 0x0400ACC0 RID: 44224
				public static LocString DESC = "Sick bays can be placed in hospital rooms to decrease the likelihood of disease spreading.";

				// Token: 0x0400ACC1 RID: 44225
				public static LocString EFFECT = "Allows Duplicants to administer basic treatments to sick Duplicants.\n\nDuplicants must possess the Bedside Manner " + UI.FormatAsLink("Skill", "ROLES") + " to treat peers.";
			}

			// Token: 0x020026FF RID: 9983
			public class ADVANCEDDOCTORSTATION
			{
				// Token: 0x0400ACC2 RID: 44226
				public static LocString NAME = UI.FormatAsLink("Disease Clinic", "ADVANCEDDOCTORSTATION");

				// Token: 0x0400ACC3 RID: 44227
				public static LocString DESC = "Disease clinics require power, but treat more serious illnesses than sick bays alone.";

				// Token: 0x0400ACC4 RID: 44228
				public static LocString EFFECT = "Allows Duplicants to administer powerful treatments to sick Duplicants.\n\nDuplicants must possess the Advanced Medical Care " + UI.FormatAsLink("Skill", "ROLES") + " to treat peers.";
			}

			// Token: 0x02002700 RID: 9984
			public class MASSAGETABLE
			{
				// Token: 0x0400ACC5 RID: 44229
				public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

				// Token: 0x0400ACC6 RID: 44230
				public static LocString DESC = "Massage tables quickly reduce extreme stress, at the cost of power production.";

				// Token: 0x0400ACC7 RID: 44231
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Rapidly reduces ",
					UI.FormatAsLink("Stress", "STRESS"),
					" for the Duplicant user.\n\nDuplicants will automatically seek a massage table when ",
					UI.FormatAsLink("Stress", "STRESS"),
					" exceeds breaktime range."
				});

				// Token: 0x0400ACC8 RID: 44232
				public static LocString ACTIVATE_TOOLTIP = "Duplicants must take a massage break when their " + UI.FormatAsKeyWord("Stress") + " reaches {0}%";

				// Token: 0x0400ACC9 RID: 44233
				public static LocString DEACTIVATE_TOOLTIP = "Breaktime ends when " + UI.FormatAsKeyWord("Stress") + " is reduced to {0}%";

				// Token: 0x0200354C RID: 13644
				public class FACADES
				{
					// Token: 0x02003871 RID: 14449
					public class DEFAULT_MASSAGETABLE
					{
						// Token: 0x0400DFD5 RID: 57301
						public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

						// Token: 0x0400DFD6 RID: 57302
						public static LocString DESC = "Massage tables quickly reduce extreme stress, at the cost of power production.";
					}

					// Token: 0x02003872 RID: 14450
					public class SHIATSU
					{
						// Token: 0x0400DFD7 RID: 57303
						public static LocString NAME = UI.FormatAsLink("Shiatsu Table", "MASSAGETABLE");

						// Token: 0x0400DFD8 RID: 57304
						public static LocString DESC = "Deep pressure for deep-seated stress.";
					}

					// Token: 0x02003873 RID: 14451
					public class MASSEUR_BALLOON
					{
						// Token: 0x0400DFD9 RID: 57305
						public static LocString NAME = UI.FormatAsLink("Inflatable Massage Table", "MASSAGETABLE");

						// Token: 0x0400DFDA RID: 57306
						public static LocString DESC = "Inflates well-being, deflates stress.";
					}
				}
			}

			// Token: 0x02002701 RID: 9985
			public class CEILINGLIGHT
			{
				// Token: 0x0400ACCA RID: 44234
				public static LocString NAME = UI.FormatAsLink("Ceiling Light", "CEILINGLIGHT");

				// Token: 0x0400ACCB RID: 44235
				public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";

				// Token: 0x0400ACCC RID: 44236
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Provides ",
					UI.FormatAsLink("Light", "LIGHT"),
					" when ",
					UI.FormatAsLink("Powered", "POWER"),
					".\n\nIncreases Duplicant workspeed within light radius."
				});

				// Token: 0x0200354D RID: 13645
				public class FACADES
				{
					// Token: 0x02003874 RID: 14452
					public class DEFAULT_CEILINGLIGHT
					{
						// Token: 0x0400DFDB RID: 57307
						public static LocString NAME = UI.FormatAsLink("Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400DFDC RID: 57308
						public static LocString DESC = "It does not go on the floor.";
					}

					// Token: 0x02003875 RID: 14453
					public class LABFLASK
					{
						// Token: 0x0400DFDD RID: 57309
						public static LocString NAME = UI.FormatAsLink("Lab Flask Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400DFDE RID: 57310
						public static LocString DESC = "For best results, do not fill with liquids.";
					}

					// Token: 0x02003876 RID: 14454
					public class FAUXPIPE
					{
						// Token: 0x0400DFDF RID: 57311
						public static LocString NAME = UI.FormatAsLink("Faux Pipe Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400DFE0 RID: 57312
						public static LocString DESC = "The height of plumbing-inspired interior design.";
					}

					// Token: 0x02003877 RID: 14455
					public class MINING
					{
						// Token: 0x0400DFE1 RID: 57313
						public static LocString NAME = UI.FormatAsLink("Mining Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400DFE2 RID: 57314
						public static LocString DESC = "The protective cage makes it the safest choice for underground parties.";
					}

					// Token: 0x02003878 RID: 14456
					public class BLOSSOM
					{
						// Token: 0x0400DFE3 RID: 57315
						public static LocString NAME = UI.FormatAsLink("Blossom Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400DFE4 RID: 57316
						public static LocString DESC = "For Duplicants who can't keep real plants alive.";
					}

					// Token: 0x02003879 RID: 14457
					public class POLKADOT
					{
						// Token: 0x0400DFE5 RID: 57317
						public static LocString NAME = UI.FormatAsLink("Polka Dot Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400DFE6 RID: 57318
						public static LocString DESC = "A fun lampshade for fun spaces.";
					}

					// Token: 0x0200387A RID: 14458
					public class RUBIKS
					{
						// Token: 0x0400DFE7 RID: 57319
						public static LocString NAME = UI.FormatAsLink("Puzzle Cube Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400DFE8 RID: 57320
						public static LocString DESC = "The initials E.R. are sewn into the lampshade.";
					}
				}
			}

			// Token: 0x02002702 RID: 9986
			public class MERCURYCEILINGLIGHT
			{
				// Token: 0x0400ACCD RID: 44237
				public static LocString NAME = UI.FormatAsLink("Mercury Ceiling Light", "MERCURYCEILINGLIGHT");

				// Token: 0x0400ACCE RID: 44238
				public static LocString DESC = "Mercury ceiling lights take a while to reach full brightness, but once they do...zowie!";

				// Token: 0x0400ACCF RID: 44239
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Mercury", "MERCURY"),
					" and ",
					UI.FormatAsLink("Power", "POWER"),
					" to produce ",
					UI.FormatAsLink("Light", "LIGHT"),
					".\n\nLight reduces Duplicant stress and is required to grow certain plants."
				});
			}

			// Token: 0x02002703 RID: 9987
			public class AIRFILTER
			{
				// Token: 0x0400ACD0 RID: 44240
				public static LocString NAME = UI.FormatAsLink("Deodorizer", "AIRFILTER");

				// Token: 0x0400ACD1 RID: 44241
				public static LocString DESC = "Oh! Citrus scented!";

				// Token: 0x0400ACD2 RID: 44242
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Sand", "SAND"),
					" to filter ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					" from the air, reducing ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" spread."
				});
			}

			// Token: 0x02002704 RID: 9988
			public class ARTIFACTANALYSISSTATION
			{
				// Token: 0x0400ACD3 RID: 44243
				public static LocString NAME = UI.FormatAsLink("Artifact Analysis Station", "ARTIFACTANALYSISSTATION");

				// Token: 0x0400ACD4 RID: 44244
				public static LocString DESC = "Discover the mysteries of the past.";

				// Token: 0x0400ACD5 RID: 44245
				public static LocString EFFECT = "Analyses and extracts " + UI.FormatAsLink("Neutronium", "UNOBTANIUM") + " from artifacts of interest.";

				// Token: 0x0400ACD6 RID: 44246
				public static LocString PAYLOAD_DROP_RATE = ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME + " drop chance: {chance}";

				// Token: 0x0400ACD7 RID: 44247
				public static LocString PAYLOAD_DROP_RATE_TOOLTIP = "This artifact has a {chance} to drop a " + ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME + " when analyzed at the " + BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.NAME;
			}

			// Token: 0x02002705 RID: 9989
			public class CANVAS
			{
				// Token: 0x0400ACD8 RID: 44248
				public static LocString NAME = UI.FormatAsLink("Blank Canvas", "CANVAS");

				// Token: 0x0400ACD9 RID: 44249
				public static LocString DESC = "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";

				// Token: 0x0400ACDA RID: 44250
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be painted by a Duplicant."
				});

				// Token: 0x0400ACDB RID: 44251
				public static LocString POORQUALITYNAME = "Crude Painting";

				// Token: 0x0400ACDC RID: 44252
				public static LocString AVERAGEQUALITYNAME = "Mediocre Painting";

				// Token: 0x0400ACDD RID: 44253
				public static LocString EXCELLENTQUALITYNAME = "Masterpiece";

				// Token: 0x0200354E RID: 13646
				public class FACADES
				{
					// Token: 0x0200387B RID: 14459
					public class ART_A
					{
						// Token: 0x0400DFE9 RID: 57321
						public static LocString NAME = UI.FormatAsLink("Doodle Dee Duplicant", "ART_A");

						// Token: 0x0400DFEA RID: 57322
						public static LocString DESC = "A sweet, amateurish interpretation of the Duplicant form.";
					}

					// Token: 0x0200387C RID: 14460
					public class ART_B
					{
						// Token: 0x0400DFEB RID: 57323
						public static LocString NAME = UI.FormatAsLink("Midnight Meal", "ART_B");

						// Token: 0x0400DFEC RID: 57324
						public static LocString DESC = "The fast-food equivalent of high art.";
					}

					// Token: 0x0200387D RID: 14461
					public class ART_C
					{
						// Token: 0x0400DFED RID: 57325
						public static LocString NAME = UI.FormatAsLink("Dupa Leesa", "ART_C");

						// Token: 0x0400DFEE RID: 57326
						public static LocString DESC = "Some viewers swear they've seen it blink.";
					}

					// Token: 0x0200387E RID: 14462
					public class ART_D
					{
						// Token: 0x0400DFEF RID: 57327
						public static LocString NAME = UI.FormatAsLink("The Screech", "ART_D");

						// Token: 0x0400DFF0 RID: 57328
						public static LocString DESC = "If art could speak, this piece would be far less popular.";
					}

					// Token: 0x0200387F RID: 14463
					public class ART_E
					{
						// Token: 0x0400DFF1 RID: 57329
						public static LocString NAME = UI.FormatAsLink("Fridup Kallo", "ART_E");

						// Token: 0x0400DFF2 RID: 57330
						public static LocString DESC = "Scratching and sniffing the flower yields no scent.";
					}

					// Token: 0x02003880 RID: 14464
					public class ART_F
					{
						// Token: 0x0400DFF3 RID: 57331
						public static LocString NAME = UI.FormatAsLink("Moopoleon Bonafarte", "ART_F");

						// Token: 0x0400DFF4 RID: 57332
						public static LocString DESC = "Portrait of a leader astride their mighty steed.";
					}

					// Token: 0x02003881 RID: 14465
					public class ART_G
					{
						// Token: 0x0400DFF5 RID: 57333
						public static LocString NAME = UI.FormatAsLink("Expressive Genius", "ART_G");

						// Token: 0x0400DFF6 RID: 57334
						public static LocString DESC = "The raw emotion conveyed here often renders viewers speechless.";
					}

					// Token: 0x02003882 RID: 14466
					public class ART_H
					{
						// Token: 0x0400DFF7 RID: 57335
						public static LocString NAME = UI.FormatAsLink("The Smooch", "ART_H");

						// Token: 0x0400DFF8 RID: 57336
						public static LocString DESC = "A candid moment of affection between two organisms.";
					}

					// Token: 0x02003883 RID: 14467
					public class ART_I
					{
						// Token: 0x0400DFF9 RID: 57337
						public static LocString NAME = UI.FormatAsLink("Self-Self-Self Portrait", "ART_I");

						// Token: 0x0400DFFA RID: 57338
						public static LocString DESC = "A multi-layered exploration of the artist as a subject.";
					}

					// Token: 0x02003884 RID: 14468
					public class ART_J
					{
						// Token: 0x0400DFFB RID: 57339
						public static LocString NAME = UI.FormatAsLink("Nikola Devouring His Mush Bar", "ART_J");

						// Token: 0x0400DFFC RID: 57340
						public static LocString DESC = "A painting that captures the true nature of hunger.";
					}

					// Token: 0x02003885 RID: 14469
					public class ART_K
					{
						// Token: 0x0400DFFD RID: 57341
						public static LocString NAME = UI.FormatAsLink("Sketchy Fungi", "ART_K");

						// Token: 0x0400DFFE RID: 57342
						public static LocString DESC = "The perfect painting for dark, dank spaces.";
					}

					// Token: 0x02003886 RID: 14470
					public class ART_L
					{
						// Token: 0x0400DFFF RID: 57343
						public static LocString NAME = UI.FormatAsLink("Post-Ear Era", "ART_L");

						// Token: 0x0400E000 RID: 57344
						public static LocString DESC = "The furry hat helped keep the artist's bandage on.";
					}

					// Token: 0x02003887 RID: 14471
					public class ART_M
					{
						// Token: 0x0400E001 RID: 57345
						public static LocString NAME = UI.FormatAsLink("Maternal Gaze", "ART_M");

						// Token: 0x0400E002 RID: 57346
						public static LocString DESC = "She's not angry, just disappointed.";
					}

					// Token: 0x02003888 RID: 14472
					public class ART_O
					{
						// Token: 0x0400E003 RID: 57347
						public static LocString NAME = UI.FormatAsLink("Hands-On", "ART_O");

						// Token: 0x0400E004 RID: 57348
						public static LocString DESC = "It's all about cooperation, really.";
					}

					// Token: 0x02003889 RID: 14473
					public class ART_N
					{
						// Token: 0x0400E005 RID: 57349
						public static LocString NAME = UI.FormatAsLink("Always Hope", "ART_N");

						// Token: 0x0400E006 RID: 57350
						public static LocString DESC = "Most Duplicants believe that the balloon in this image is about to be caught.";
					}

					// Token: 0x0200388A RID: 14474
					public class ART_P
					{
						// Token: 0x0400E007 RID: 57351
						public static LocString NAME = UI.FormatAsLink("Pour Soul", "ART_P");

						// Token: 0x0400E008 RID: 57352
						public static LocString DESC = "It is a cruel guest who does not RSVP.";
					}

					// Token: 0x0200388B RID: 14475
					public class ART_Q
					{
						// Token: 0x0400E009 RID: 57353
						public static LocString NAME = UI.FormatAsLink("Ore Else", "ART_Q");

						// Token: 0x0400E00A RID: 57354
						public static LocString DESC = "The only kind of gift that poorly behaved Duplicants can expect to receive.";
					}

					// Token: 0x0200388C RID: 14476
					public class ART_R
					{
						// Token: 0x0400E00B RID: 57355
						public static LocString NAME = UI.FormatAsLink("Lazer Pipz", "ART_R");

						// Token: 0x0400E00C RID: 57356
						public static LocString DESC = "It combines two things that everyone loves: pips and lasers.";
					}
				}
			}

			// Token: 0x02002706 RID: 9990
			public class CANVASWIDE
			{
				// Token: 0x0400ACDE RID: 44254
				public static LocString NAME = UI.FormatAsLink("Landscape Canvas", "CANVASWIDE");

				// Token: 0x0400ACDF RID: 44255
				public static LocString DESC = "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";

				// Token: 0x0400ACE0 RID: 44256
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be painted by a Duplicant."
				});

				// Token: 0x0400ACE1 RID: 44257
				public static LocString POORQUALITYNAME = "Crude Painting";

				// Token: 0x0400ACE2 RID: 44258
				public static LocString AVERAGEQUALITYNAME = "Mediocre Painting";

				// Token: 0x0400ACE3 RID: 44259
				public static LocString EXCELLENTQUALITYNAME = "Masterpiece";

				// Token: 0x0200354F RID: 13647
				public class FACADES
				{
					// Token: 0x0200388D RID: 14477
					public class ART_WIDE_A
					{
						// Token: 0x0400E00D RID: 57357
						public static LocString NAME = UI.FormatAsLink("The Twins", "ART_WIDE_A");

						// Token: 0x0400E00E RID: 57358
						public static LocString DESC = "The effort is admirable, though the execution is not.";
					}

					// Token: 0x0200388E RID: 14478
					public class ART_WIDE_B
					{
						// Token: 0x0400E00F RID: 57359
						public static LocString NAME = UI.FormatAsLink("Ground Zero", "ART_WIDE_B");

						// Token: 0x0400E010 RID: 57360
						public static LocString DESC = "Every story has its origin.";
					}

					// Token: 0x0200388F RID: 14479
					public class ART_WIDE_C
					{
						// Token: 0x0400E011 RID: 57361
						public static LocString NAME = UI.FormatAsLink("Still Life with Barbeque and Frost Bun", "ART_WIDE_C");

						// Token: 0x0400E012 RID: 57362
						public static LocString DESC = "Food this good deserves to be immortalized.";
					}

					// Token: 0x02003890 RID: 14480
					public class ART_WIDE_D
					{
						// Token: 0x0400E013 RID: 57363
						public static LocString NAME = UI.FormatAsLink("Composition with Three Colors", "ART_WIDE_D");

						// Token: 0x0400E014 RID: 57364
						public static LocString DESC = "All the other colors in the artist's palette had dried up.";
					}

					// Token: 0x02003891 RID: 14481
					public class ART_WIDE_E
					{
						// Token: 0x0400E015 RID: 57365
						public static LocString NAME = UI.FormatAsLink("Behold, A Fork", "ART_WIDE_E");

						// Token: 0x0400E016 RID: 57366
						public static LocString DESC = "Each tine represents a branch of science.";
					}

					// Token: 0x02003892 RID: 14482
					public class ART_WIDE_F
					{
						// Token: 0x0400E017 RID: 57367
						public static LocString NAME = UI.FormatAsLink("The Astronomer at Home", "ART_WIDE_F");

						// Token: 0x0400E018 RID: 57368
						public static LocString DESC = "Its companion piece, \"The Astronomer at Work\" was lost in a meteor shower.";
					}

					// Token: 0x02003893 RID: 14483
					public class ART_WIDE_G
					{
						// Token: 0x0400E019 RID: 57369
						public static LocString NAME = UI.FormatAsLink("Iconic Iteration", "ART_WIDE_G");

						// Token: 0x0400E01A RID: 57370
						public static LocString DESC = "For the art collector who doesn't mind a bit of repetition.";
					}

					// Token: 0x02003894 RID: 14484
					public class ART_WIDE_H
					{
						// Token: 0x0400E01B RID: 57371
						public static LocString NAME = UI.FormatAsLink("La Belle Meep", "ART_WIDE_H");

						// Token: 0x0400E01C RID: 57372
						public static LocString DESC = "A daring piece, guaranteed to cause a stir.";
					}

					// Token: 0x02003895 RID: 14485
					public class ART_WIDE_I
					{
						// Token: 0x0400E01D RID: 57373
						public static LocString NAME = UI.FormatAsLink("Glorious Vole", "ART_WIDE_I");

						// Token: 0x0400E01E RID: 57374
						public static LocString DESC = "A moody study of the renowned tunneler.";
					}

					// Token: 0x02003896 RID: 14486
					public class ART_WIDE_J
					{
						// Token: 0x0400E01F RID: 57375
						public static LocString NAME = UI.FormatAsLink("The Swell Swell", "ART_WIDE_J");

						// Token: 0x0400E020 RID: 57376
						public static LocString DESC = "As far as wave-themed art goes, it's great.";
					}

					// Token: 0x02003897 RID: 14487
					public class ART_WIDE_K
					{
						// Token: 0x0400E021 RID: 57377
						public static LocString NAME = UI.FormatAsLink("Flight of the Slicksters", "ART_WIDE_K");

						// Token: 0x0400E022 RID: 57378
						public static LocString DESC = "The delight on the subjects' faces is contagious.";
					}

					// Token: 0x02003898 RID: 14488
					public class ART_WIDE_L
					{
						// Token: 0x0400E023 RID: 57379
						public static LocString NAME = UI.FormatAsLink("The Shiny Night", "ART_WIDE_L");

						// Token: 0x0400E024 RID: 57380
						public static LocString DESC = "A dreamy abundance of swirls, whirls and whorls.";
					}

					// Token: 0x02003899 RID: 14489
					public class ART_WIDE_M
					{
						// Token: 0x0400E025 RID: 57381
						public static LocString NAME = UI.FormatAsLink("Hot Afternoon", "ART_WIDE_M");

						// Token: 0x0400E026 RID: 57382
						public static LocString DESC = "Things get a bit melty if they're forgotten in the sun.";
					}

					// Token: 0x0200389A RID: 14490
					public class ART_WIDE_O
					{
						// Token: 0x0400E027 RID: 57383
						public static LocString NAME = UI.FormatAsLink("Super Old Mural", "ART_WIDE_O");

						// Token: 0x0400E028 RID: 57384
						public static LocString DESC = "Even just exhaling nearby could damage this historical work.";
					}
				}
			}

			// Token: 0x02002707 RID: 9991
			public class CANVASTALL
			{
				// Token: 0x0400ACE4 RID: 44260
				public static LocString NAME = UI.FormatAsLink("Portrait Canvas", "CANVASTALL");

				// Token: 0x0400ACE5 RID: 44261
				public static LocString DESC = "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";

				// Token: 0x0400ACE6 RID: 44262
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be painted by a Duplicant."
				});

				// Token: 0x0400ACE7 RID: 44263
				public static LocString POORQUALITYNAME = "Crude Painting";

				// Token: 0x0400ACE8 RID: 44264
				public static LocString AVERAGEQUALITYNAME = "Mediocre Painting";

				// Token: 0x0400ACE9 RID: 44265
				public static LocString EXCELLENTQUALITYNAME = "Masterpiece";

				// Token: 0x02003550 RID: 13648
				public class FACADES
				{
					// Token: 0x0200389B RID: 14491
					public class ART_TALL_A
					{
						// Token: 0x0400E029 RID: 57385
						public static LocString NAME = UI.FormatAsLink("Ode to O2", "ART_TALL_A");

						// Token: 0x0400E02A RID: 57386
						public static LocString DESC = "Even amateur art is essential to life.";
					}

					// Token: 0x0200389C RID: 14492
					public class ART_TALL_B
					{
						// Token: 0x0400E02B RID: 57387
						public static LocString NAME = UI.FormatAsLink("A Cool Wheeze", "ART_TALL_B");

						// Token: 0x0400E02C RID: 57388
						public static LocString DESC = "It certainly is colorful.";
					}

					// Token: 0x0200389D RID: 14493
					public class ART_TALL_C
					{
						// Token: 0x0400E02D RID: 57389
						public static LocString NAME = UI.FormatAsLink("Luxe Splatter", "ART_TALL_C");

						// Token: 0x0400E02E RID: 57390
						public static LocString DESC = "Chaotic, yet compelling.";
					}

					// Token: 0x0200389E RID: 14494
					public class ART_TALL_D
					{
						// Token: 0x0400E02F RID: 57391
						public static LocString NAME = UI.FormatAsLink("Pickled Meal Lice II", "ART_TALL_D");

						// Token: 0x0400E030 RID: 57392
						public static LocString DESC = "It doesn't have to taste good, it's art.";
					}

					// Token: 0x0200389F RID: 14495
					public class ART_TALL_E
					{
						// Token: 0x0400E031 RID: 57393
						public static LocString NAME = UI.FormatAsLink("Fruit Face", "ART_TALL_E");

						// Token: 0x0400E032 RID: 57394
						public static LocString DESC = "Rumor has it that the model was self-conscious about their uneven eyebrows.";
					}

					// Token: 0x020038A0 RID: 14496
					public class ART_TALL_F
					{
						// Token: 0x0400E033 RID: 57395
						public static LocString NAME = UI.FormatAsLink("Girl with the Blue Scarf", "ART_TALL_F");

						// Token: 0x0400E034 RID: 57396
						public static LocString DESC = "The earring is nice too.";
					}

					// Token: 0x020038A1 RID: 14497
					public class ART_TALL_G
					{
						// Token: 0x0400E035 RID: 57397
						public static LocString NAME = UI.FormatAsLink("A Farewell at Sunrise", "ART_TALL_G");

						// Token: 0x0400E036 RID: 57398
						public static LocString DESC = "A poetic ink painting depicting the beginning of an end.";
					}

					// Token: 0x020038A2 RID: 14498
					public class ART_TALL_H
					{
						// Token: 0x0400E037 RID: 57399
						public static LocString NAME = UI.FormatAsLink("Conqueror of Clusters", "ART_TALL_H");

						// Token: 0x0400E038 RID: 57400
						public static LocString DESC = "The type of painting that ambitious Duplicants gravitate to.";
					}

					// Token: 0x020038A3 RID: 14499
					public class ART_TALL_I
					{
						// Token: 0x0400E039 RID: 57401
						public static LocString NAME = UI.FormatAsLink("Pei Phone", "ART_TALL_I");

						// Token: 0x0400E03A RID: 57402
						public static LocString DESC = "When the future calls, Duplicants answer.";
					}

					// Token: 0x020038A4 RID: 14500
					public class ART_TALL_J
					{
						// Token: 0x0400E03B RID: 57403
						public static LocString NAME = UI.FormatAsLink("Duplicants of the Galaxy", "ART_TALL_J");

						// Token: 0x0400E03C RID: 57404
						public static LocString DESC = "A poster for a blockbuster film that was never made.";
					}

					// Token: 0x020038A5 RID: 14501
					public class ART_TALL_K
					{
						// Token: 0x0400E03D RID: 57405
						public static LocString NAME = UI.FormatAsLink("Cubist Loo", "ART_TALL_K");

						// Token: 0x0400E03E RID: 57406
						public static LocString DESC = "The glass and frame are hydrophobic, for easy cleaning.";
					}

					// Token: 0x020038A6 RID: 14502
					public class ART_TALL_M
					{
						// Token: 0x0400E03F RID: 57407
						public static LocString NAME = UI.FormatAsLink("Do Not Disturb", "ART_TALL_M");

						// Token: 0x0400E040 RID: 57408
						public static LocString DESC = "No one likes being interrupted when they're waiting for inspiration to strike.";
					}

					// Token: 0x020038A7 RID: 14503
					public class ART_TALL_L
					{
						// Token: 0x0400E041 RID: 57409
						public static LocString NAME = UI.FormatAsLink("Mirror Ball", "ART_TALL_L");

						// Token: 0x0400E042 RID: 57410
						public static LocString DESC = "Nearby, a companion animal waited for the object to be thrown.";
					}

					// Token: 0x020038A8 RID: 14504
					public class ART_TALL_P
					{
						// Token: 0x0400E043 RID: 57411
						public static LocString NAME = "The Feast";

						// Token: 0x0400E044 RID: 57412
						public static LocString DESC = "There were greasy fingerprints on the canvas even before the paint had dried.";
					}
				}
			}

			// Token: 0x02002708 RID: 9992
			public class CO2SCRUBBER
			{
				// Token: 0x0400ACEA RID: 44266
				public static LocString NAME = UI.FormatAsLink("Carbon Skimmer", "CO2SCRUBBER");

				// Token: 0x0400ACEB RID: 44267
				public static LocString DESC = "Skimmers remove large amounts of carbon dioxide, but produce no breathable air.";

				// Token: 0x0400ACEC RID: 44268
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Water", "WATER"),
					" to filter ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" from the air."
				});
			}

			// Token: 0x02002709 RID: 9993
			public class COMPOST
			{
				// Token: 0x0400ACED RID: 44269
				public static LocString NAME = UI.FormatAsLink("Compost", "COMPOST");

				// Token: 0x0400ACEE RID: 44270
				public static LocString DESC = "Composts safely deal with biological waste, producing fresh dirt.";

				// Token: 0x0400ACEF RID: 44271
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reduces ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" and other compostables down into ",
					UI.FormatAsLink("Dirt", "DIRT"),
					"."
				});
			}

			// Token: 0x0200270A RID: 9994
			public class COOKINGSTATION
			{
				// Token: 0x0400ACF0 RID: 44272
				public static LocString NAME = UI.FormatAsLink("Electric Grill", "COOKINGSTATION");

				// Token: 0x0400ACF1 RID: 44273
				public static LocString DESC = "Proper cooking eliminates foodborne disease and produces tasty, stress-relieving meals.";

				// Token: 0x0400ACF2 RID: 44274
				public static LocString EFFECT = "Cooks a wide variety of improved " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x0200270B RID: 9995
			public class CRYOTANK
			{
				// Token: 0x0400ACF3 RID: 44275
				public static LocString NAME = UI.FormatAsLink("Cryotank 3000", "CRYOTANK");

				// Token: 0x0400ACF4 RID: 44276
				public static LocString DESC = "The tank appears impossibly old, but smells crisp and brand new.\n\nA silhouette just barely visible through the frost of the glass.";

				// Token: 0x0400ACF5 RID: 44277
				public static LocString DEFROSTBUTTON = "Defrost Friend";

				// Token: 0x0400ACF6 RID: 44278
				public static LocString DEFROSTBUTTONTOOLTIP = "A new pal is just an icebreaker away";
			}

			// Token: 0x0200270C RID: 9996
			public class GOURMETCOOKINGSTATION
			{
				// Token: 0x0400ACF7 RID: 44279
				public static LocString NAME = UI.FormatAsLink("Gas Range", "GOURMETCOOKINGSTATION");

				// Token: 0x0400ACF8 RID: 44280
				public static LocString DESC = "Luxury meals increase Duplicants' morale and prevents them from becoming stressed.";

				// Token: 0x0400ACF9 RID: 44281
				public static LocString EFFECT = "Cooks a wide variety of quality " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x0200270D RID: 9997
			public class DININGTABLE
			{
				// Token: 0x0400ACFA RID: 44282
				public static LocString NAME = UI.FormatAsLink("Mess Table", "DININGTABLE");

				// Token: 0x0400ACFB RID: 44283
				public static LocString DESC = "Duplicants prefer to dine at a table, rather than eat off the floor.";

				// Token: 0x0400ACFC RID: 44284
				public static LocString EFFECT = "Gives one Duplicant a place to eat.\n\nDuplicants will automatically eat at their assigned table when hungry.";
			}

			// Token: 0x0200270E RID: 9998
			public class DOOR
			{
				// Token: 0x0400ACFD RID: 44285
				public static LocString NAME = UI.FormatAsLink("Pneumatic Door", "DOOR");

				// Token: 0x0400ACFE RID: 44286
				public static LocString DESC = "Door controls can be used to prevent Duplicants from entering restricted areas.";

				// Token: 0x0400ACFF RID: 44287
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Encloses areas without blocking ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});

				// Token: 0x0400AD00 RID: 44288
				public static LocString PRESSURE_SUIT_REQUIRED = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT") + " required {0}";

				// Token: 0x0400AD01 RID: 44289
				public static LocString PRESSURE_SUIT_NOT_REQUIRED = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT") + " not required {0}";

				// Token: 0x0400AD02 RID: 44290
				public static LocString ABOVE = "above";

				// Token: 0x0400AD03 RID: 44291
				public static LocString BELOW = "below";

				// Token: 0x0400AD04 RID: 44292
				public static LocString LEFT = "on left";

				// Token: 0x0400AD05 RID: 44293
				public static LocString RIGHT = "on right";

				// Token: 0x0400AD06 RID: 44294
				public static LocString LOGIC_OPEN = "Open/Close";

				// Token: 0x0400AD07 RID: 44295
				public static LocString LOGIC_OPEN_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Open";

				// Token: 0x0400AD08 RID: 44296
				public static LocString LOGIC_OPEN_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Close and lock";

				// Token: 0x02003551 RID: 13649
				public static class CONTROL_STATE
				{
					// Token: 0x020038A9 RID: 14505
					public class OPEN
					{
						// Token: 0x0400E045 RID: 57413
						public static LocString NAME = "Open";

						// Token: 0x0400E046 RID: 57414
						public static LocString TOOLTIP = "This door will remain open";
					}

					// Token: 0x020038AA RID: 14506
					public class CLOSE
					{
						// Token: 0x0400E047 RID: 57415
						public static LocString NAME = "Lock";

						// Token: 0x0400E048 RID: 57416
						public static LocString TOOLTIP = "Nothing may pass through";
					}

					// Token: 0x020038AB RID: 14507
					public class AUTO
					{
						// Token: 0x0400E049 RID: 57417
						public static LocString NAME = "Auto";

						// Token: 0x0400E04A RID: 57418
						public static LocString TOOLTIP = "Duplicants open and close this door as needed";
					}
				}
			}

			// Token: 0x0200270F RID: 9999
			public class ELECTROBANKCHARGER
			{
				// Token: 0x0400AD09 RID: 44297
				public static LocString NAME = UI.FormatAsLink("Power Bank Charger", "ELECTROBANKCHARGER");

				// Token: 0x0400AD0A RID: 44298
				public static LocString DESC = "Bionic Duplicants rely on a steady supply of power to function.";

				// Token: 0x0400AD0B RID: 44299
				public static LocString EFFECT = "Converts empty " + UI.FormatAsLink("Eco Power Banks", "ELECTROBANK") + " into fully charged units ready for reuse.";
			}

			// Token: 0x02002710 RID: 10000
			public class SMALLELECTROBANKDISCHARGER
			{
				// Token: 0x0400AD0C RID: 44300
				public static LocString NAME = UI.FormatAsLink("Wall Socket", "SMALLELECTROBANKDISCHARGER");

				// Token: 0x0400AD0D RID: 44301
				public static LocString DESC = "It can also be placed on the ceiling.";

				// Token: 0x0400AD0E RID: 44302
				public static LocString EFFECT = "Converts stored energy from " + UI.FormatAsLink("Eco Power Banks", "ELECTROBANK") + " into power for connected buildings.";
			}

			// Token: 0x02002711 RID: 10001
			public class LARGEELECTROBANKDISCHARGER
			{
				// Token: 0x0400AD0F RID: 44303
				public static LocString NAME = UI.FormatAsLink("Socket Station", "LARGEELECTROBANKDISCHARGER");

				// Token: 0x0400AD10 RID: 44304
				public static LocString DESC = "It's a real powerhouse.";

				// Token: 0x0400AD11 RID: 44305
				public static LocString EFFECT = "Efficiently converts stored energy from " + UI.FormatAsLink("Power Banks", "ELECTROBANK") + " into power for connected buildings.";
			}

			// Token: 0x02002712 RID: 10002
			public class ELECTROLYZER
			{
				// Token: 0x0400AD12 RID: 44306
				public static LocString NAME = UI.FormatAsLink("Electrolyzer", "ELECTROLYZER");

				// Token: 0x0400AD13 RID: 44307
				public static LocString DESC = "Water goes in one end, life sustaining oxygen comes out the other.";

				// Token: 0x0400AD14 RID: 44308
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Water", "WATER"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			// Token: 0x02002713 RID: 10003
			public class RUSTDEOXIDIZER
			{
				// Token: 0x0400AD15 RID: 44309
				public static LocString NAME = UI.FormatAsLink("Rust Deoxidizer", "RUSTDEOXIDIZER");

				// Token: 0x0400AD16 RID: 44310
				public static LocString DESC = "Rust and salt goes in, oxygen comes out.";

				// Token: 0x0400AD17 RID: 44311
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Rust", "RUST"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Chlorine Gas", "CHLORINE"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			// Token: 0x02002714 RID: 10004
			public class DESALINATOR
			{
				// Token: 0x0400AD18 RID: 44312
				public static LocString NAME = UI.FormatAsLink("Desalinator", "DESALINATOR");

				// Token: 0x0400AD19 RID: 44313
				public static LocString DESC = "Salt can be refined into table salt for a mealtime morale boost.";

				// Token: 0x0400AD1A RID: 44314
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Removes ",
					UI.FormatAsLink("Salt", "SALT"),
					" from ",
					UI.FormatAsLink("Brine", "BRINE"),
					" or ",
					UI.FormatAsLink("Salt Water", "SALTWATER"),
					", producing ",
					UI.FormatAsLink("Water", "WATER"),
					"."
				});
			}

			// Token: 0x02002715 RID: 10005
			public class POWERTRANSFORMERSMALL
			{
				// Token: 0x0400AD1B RID: 44315
				public static LocString NAME = UI.FormatAsLink("Power Transformer", "POWERTRANSFORMERSMALL");

				// Token: 0x0400AD1C RID: 44316
				public static LocString DESC = "Limiting the power drawn by wires prevents them from incurring overload damage.";

				// Token: 0x0400AD1D RID: 44317
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Limits ",
					UI.FormatAsLink("Power", "POWER"),
					" flowing through the Transformer to 1000 W.\n\nConnect ",
					UI.FormatAsLink("Batteries", "BATTERY"),
					" on the large side to act as a valve and prevent ",
					UI.FormatAsLink("Wires", "WIRE"),
					" from drawing more than 1000 W.\n\nCan be rotated before construction."
				});
			}

			// Token: 0x02002716 RID: 10006
			public class POWERTRANSFORMER
			{
				// Token: 0x0400AD1E RID: 44318
				public static LocString NAME = UI.FormatAsLink("Large Power Transformer", "POWERTRANSFORMER");

				// Token: 0x0400AD1F RID: 44319
				public static LocString DESC = "It's a power transformer, but larger.";

				// Token: 0x0400AD20 RID: 44320
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Limits ",
					UI.FormatAsLink("Power", "POWER"),
					" flowing through the Transformer to 4 kW.\n\nConnect ",
					UI.FormatAsLink("Batteries", "BATTERY"),
					" on the large side to act as a valve and prevent ",
					UI.FormatAsLink("Wires", "WIRE"),
					" from drawing more than 4 kW.\n\nCan be rotated before construction."
				});
			}

			// Token: 0x02002717 RID: 10007
			public class FLOORLAMP
			{
				// Token: 0x0400AD21 RID: 44321
				public static LocString NAME = UI.FormatAsLink("Lamp", "FLOORLAMP");

				// Token: 0x0400AD22 RID: 44322
				public static LocString DESC = "Any building's light emitting radius can be viewed in the light overlay.";

				// Token: 0x0400AD23 RID: 44323
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Provides ",
					UI.FormatAsLink("Light", "LIGHT"),
					" when ",
					UI.FormatAsLink("Powered", "POWER"),
					".\n\nIncreases Duplicant workspeed within light radius."
				});

				// Token: 0x02003552 RID: 13650
				public class FACADES
				{
					// Token: 0x020038AC RID: 14508
					public class DEFAULT_FLOORLAMP
					{
						// Token: 0x0400E04B RID: 57419
						public static LocString NAME = UI.FormatAsLink("Lamp", "FLOORLAMP");

						// Token: 0x0400E04C RID: 57420
						public static LocString DESC = "Any building's light emitting radius can be viewed in the light overlay.";
					}

					// Token: 0x020038AD RID: 14509
					public class LEG
					{
						// Token: 0x0400E04D RID: 57421
						public static LocString NAME = UI.FormatAsLink("Fragile Leg Lamp", "FLOORLAMP");

						// Token: 0x0400E04E RID: 57422
						public static LocString DESC = "This lamp blazes forth in unparalleled glory.";
					}

					// Token: 0x020038AE RID: 14510
					public class BRISTLEBLOSSOM
					{
						// Token: 0x0400E04F RID: 57423
						public static LocString NAME = UI.FormatAsLink("Holiday Lamp", "FLOORLAMP");

						// Token: 0x0400E050 RID: 57424
						public static LocString DESC = "It's a bit prickly, but it casts a festive glow.";
					}
				}
			}

			// Token: 0x02002718 RID: 10008
			public class FLOWERVASE
			{
				// Token: 0x0400AD24 RID: 44324
				public static LocString NAME = UI.FormatAsLink("Flower Pot", "FLOWERVASE");

				// Token: 0x0400AD25 RID: 44325
				public static LocString DESC = "Flower pots allow decorative plants to be moved to new locations.";

				// Token: 0x0400AD26 RID: 44326
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x02003553 RID: 13651
				public class FACADES
				{
					// Token: 0x020038AF RID: 14511
					public class DEFAULT_FLOWERVASE
					{
						// Token: 0x0400E051 RID: 57425
						public static LocString NAME = UI.FormatAsLink("Flower Pot", "FLOWERVASE");

						// Token: 0x0400E052 RID: 57426
						public static LocString DESC = "The original container for plants on the move.";
					}

					// Token: 0x020038B0 RID: 14512
					public class RETRO_SUNNY
					{
						// Token: 0x0400E053 RID: 57427
						public static LocString NAME = UI.FormatAsLink("Sunny Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400E054 RID: 57428
						public static LocString DESC = "A funky yellow flower pot for plants on the move.";
					}

					// Token: 0x020038B1 RID: 14513
					public class RETRO_BOLD
					{
						// Token: 0x0400E055 RID: 57429
						public static LocString NAME = UI.FormatAsLink("Bold Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400E056 RID: 57430
						public static LocString DESC = "A funky red flower pot for plants on the move.";
					}

					// Token: 0x020038B2 RID: 14514
					public class RETRO_BRIGHT
					{
						// Token: 0x0400E057 RID: 57431
						public static LocString NAME = UI.FormatAsLink("Bright Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400E058 RID: 57432
						public static LocString DESC = "A funky green flower pot for plants on the move.";
					}

					// Token: 0x020038B3 RID: 14515
					public class RETRO_DREAMY
					{
						// Token: 0x0400E059 RID: 57433
						public static LocString NAME = UI.FormatAsLink("Dreamy Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400E05A RID: 57434
						public static LocString DESC = "A funky blue flower pot for plants on the move.";
					}

					// Token: 0x020038B4 RID: 14516
					public class RETRO_ELEGANT
					{
						// Token: 0x0400E05B RID: 57435
						public static LocString NAME = UI.FormatAsLink("Elegant Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400E05C RID: 57436
						public static LocString DESC = "A funky white flower pot for plants on the move.";
					}
				}
			}

			// Token: 0x02002719 RID: 10009
			public class FLOWERVASEWALL
			{
				// Token: 0x0400AD27 RID: 44327
				public static LocString NAME = UI.FormatAsLink("Wall Pot", "FLOWERVASEWALL");

				// Token: 0x0400AD28 RID: 44328
				public static LocString DESC = "Placing a plant in a wall pot can add a spot of Decor to otherwise bare walls.";

				// Token: 0x0400AD29 RID: 44329
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be hung from a wall."
				});

				// Token: 0x02003554 RID: 13652
				public class FACADES
				{
					// Token: 0x020038B5 RID: 14517
					public class DEFAULT_FLOWERVASEWALL
					{
						// Token: 0x0400E05D RID: 57437
						public static LocString NAME = UI.FormatAsLink("Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400E05E RID: 57438
						public static LocString DESC = "Facilitates vertical plant displays.";
					}

					// Token: 0x020038B6 RID: 14518
					public class RETRO_GREEN
					{
						// Token: 0x0400E05F RID: 57439
						public static LocString NAME = UI.FormatAsLink("Bright Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400E060 RID: 57440
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					// Token: 0x020038B7 RID: 14519
					public class RETRO_YELLOW
					{
						// Token: 0x0400E061 RID: 57441
						public static LocString NAME = UI.FormatAsLink("Sunny Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400E062 RID: 57442
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					// Token: 0x020038B8 RID: 14520
					public class RETRO_RED
					{
						// Token: 0x0400E063 RID: 57443
						public static LocString NAME = UI.FormatAsLink("Bold Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400E064 RID: 57444
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					// Token: 0x020038B9 RID: 14521
					public class RETRO_BLUE
					{
						// Token: 0x0400E065 RID: 57445
						public static LocString NAME = UI.FormatAsLink("Dreamy Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400E066 RID: 57446
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					// Token: 0x020038BA RID: 14522
					public class RETRO_WHITE
					{
						// Token: 0x0400E067 RID: 57447
						public static LocString NAME = UI.FormatAsLink("Elegant Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400E068 RID: 57448
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}
				}
			}

			// Token: 0x0200271A RID: 10010
			public class FLOWERVASEHANGING
			{
				// Token: 0x0400AD2A RID: 44330
				public static LocString NAME = UI.FormatAsLink("Hanging Pot", "FLOWERVASEHANGING");

				// Token: 0x0400AD2B RID: 44331
				public static LocString DESC = "Hanging pots can add some Decor to a room, without blocking buildings on the floor.";

				// Token: 0x0400AD2C RID: 44332
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be hung from a ceiling."
				});

				// Token: 0x02003555 RID: 13653
				public class FACADES
				{
					// Token: 0x020038BB RID: 14523
					public class RETRO_RED
					{
						// Token: 0x0400E069 RID: 57449
						public static LocString NAME = UI.FormatAsLink("Bold Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400E06A RID: 57450
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x020038BC RID: 14524
					public class RETRO_GREEN
					{
						// Token: 0x0400E06B RID: 57451
						public static LocString NAME = UI.FormatAsLink("Bright Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400E06C RID: 57452
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x020038BD RID: 14525
					public class RETRO_BLUE
					{
						// Token: 0x0400E06D RID: 57453
						public static LocString NAME = UI.FormatAsLink("Dreamy Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400E06E RID: 57454
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x020038BE RID: 14526
					public class RETRO_YELLOW
					{
						// Token: 0x0400E06F RID: 57455
						public static LocString NAME = UI.FormatAsLink("Sunny Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400E070 RID: 57456
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x020038BF RID: 14527
					public class RETRO_WHITE
					{
						// Token: 0x0400E071 RID: 57457
						public static LocString NAME = UI.FormatAsLink("Elegant Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400E072 RID: 57458
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x020038C0 RID: 14528
					public class BEAKER
					{
						// Token: 0x0400E073 RID: 57459
						public static LocString NAME = UI.FormatAsLink("Beaker Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400E074 RID: 57460
						public static LocString DESC = "A measured approach to indoor plant decor.";
					}

					// Token: 0x020038C1 RID: 14529
					public class RUBIKS
					{
						// Token: 0x0400E075 RID: 57461
						public static LocString NAME = UI.FormatAsLink("Puzzle Cube Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400E076 RID: 57462
						public static LocString DESC = "The real puzzle is how to keep indoor plants alive.";
					}
				}
			}

			// Token: 0x0200271B RID: 10011
			public class FLOWERVASEHANGINGFANCY
			{
				// Token: 0x0400AD2D RID: 44333
				public static LocString NAME = UI.FormatAsLink("Aero Pot", "FLOWERVASEHANGINGFANCY");

				// Token: 0x0400AD2E RID: 44334
				public static LocString DESC = "Aero pots can be hung from the ceiling and have extremely high Decor.";

				// Token: 0x0400AD2F RID: 44335
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be hung from a ceiling."
				});

				// Token: 0x02003556 RID: 13654
				public class FACADES
				{
				}
			}

			// Token: 0x0200271C RID: 10012
			public class FLUSHTOILET
			{
				// Token: 0x0400AD30 RID: 44336
				public static LocString NAME = UI.FormatAsLink("Lavatory", "FLUSHTOILET");

				// Token: 0x0400AD31 RID: 44337
				public static LocString DESC = "Lavatories transmit fewer germs to Duplicants' skin and require no emptying.";

				// Token: 0x0400AD32 RID: 44338
				public static LocString EFFECT = "Gives Duplicants a place to relieve themselves.\n\nSpreads very few " + UI.FormatAsLink("Germs", "DISEASE") + ".";

				// Token: 0x02003557 RID: 13655
				public class FACADES
				{
					// Token: 0x020038C2 RID: 14530
					public class DEFAULT_FLUSHTOILET
					{
						// Token: 0x0400E077 RID: 57463
						public static LocString NAME = UI.FormatAsLink("Lavatory", "FLUSHTOILET");

						// Token: 0x0400E078 RID: 57464
						public static LocString DESC = "Lavatories transmit fewer germs to Duplicants' skin and require no emptying.";
					}

					// Token: 0x020038C3 RID: 14531
					public class POLKA_DARKPURPLERESIN
					{
						// Token: 0x0400E079 RID: 57465
						public static LocString NAME = UI.FormatAsLink("Mod Dot Lavatory", "FLUSHTOILET");

						// Token: 0x0400E07A RID: 57466
						public static LocString DESC = "For those who've really got to a-go-go.";
					}

					// Token: 0x020038C4 RID: 14532
					public class POLKA_DARKNAVYNOOKGREEN
					{
						// Token: 0x0400E07B RID: 57467
						public static LocString NAME = UI.FormatAsLink("Party Dot Lavatory", "FLUSHTOILET");

						// Token: 0x0400E07C RID: 57468
						public static LocString DESC = "Smooth moves happen here.";
					}

					// Token: 0x020038C5 RID: 14533
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400E07D RID: 57469
						public static LocString NAME = UI.FormatAsLink("Faint Purple Lavatory", "FLUSHTOILET");

						// Token: 0x0400E07E RID: 57470
						public static LocString DESC = "It's like pooping inside Hexalent fruit!";
					}

					// Token: 0x020038C6 RID: 14534
					public class YELLOW_TARTAR
					{
						// Token: 0x0400E07F RID: 57471
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Lavatory", "FLUSHTOILET");

						// Token: 0x0400E080 RID: 57472
						public static LocString DESC = "Someone thought it'd be a good idea to have the outside match the inside.";
					}

					// Token: 0x020038C7 RID: 14535
					public class RED_ROSE
					{
						// Token: 0x0400E081 RID: 57473
						public static LocString NAME = UI.FormatAsLink("Puce Pink Lavatory", "FLUSHTOILET");

						// Token: 0x0400E082 RID: 57474
						public static LocString DESC = "The scented pink toilet paper smells like a rosebush in a sewage plant.";
					}

					// Token: 0x020038C8 RID: 14536
					public class GREEN_MUSH
					{
						// Token: 0x0400E083 RID: 57475
						public static LocString NAME = UI.FormatAsLink("Mush Green Lavatory", "FLUSHTOILET");

						// Token: 0x0400E084 RID: 57476
						public static LocString DESC = "Mush in, mush out.";
					}

					// Token: 0x020038C9 RID: 14537
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400E085 RID: 57477
						public static LocString NAME = UI.FormatAsLink("Weepy Lavatory", "FLUSHTOILET");

						// Token: 0x0400E086 RID: 57478
						public static LocString DESC = "A private place to feel big feelings.";
					}
				}
			}

			// Token: 0x0200271D RID: 10013
			public class SHOWER
			{
				// Token: 0x0400AD33 RID: 44339
				public static LocString NAME = UI.FormatAsLink("Shower", "SHOWER");

				// Token: 0x0400AD34 RID: 44340
				public static LocString DESC = "Regularly showering will prevent Duplicants spreading germs to the things they touch.";

				// Token: 0x0400AD35 RID: 44341
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Improves Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					" and removes surface ",
					UI.FormatAsLink("Germs", "DISEASE"),
					"."
				});
			}

			// Token: 0x0200271E RID: 10014
			public class CONDUIT
			{
				// Token: 0x02003558 RID: 13656
				public class STATUS_ITEM
				{
					// Token: 0x0400D7DE RID: 55262
					public static LocString NAME = "Marked for Emptying";

					// Token: 0x0400D7DF RID: 55263
					public static LocString TOOLTIP = "Awaiting a " + UI.FormatAsLink("Plumber", "PLUMBER") + " to clear this pipe";
				}
			}

			// Token: 0x0200271F RID: 10015
			public class MORBROVERMAKER
			{
				// Token: 0x0400AD36 RID: 44342
				public static LocString NAME = UI.FormatAsLink("Biobot Builder", "STORYTRAITMORBROVER");

				// Token: 0x0400AD37 RID: 44343
				public static LocString DESC = "Allows a skilled Duplicant to manufacture a steady supply of icky yet effective bots.";

				// Token: 0x0400AD38 RID: 44344
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES"),
					" and ",
					UI.FormatAsLink("Steel", "STEEL"),
					" to craft biofueled machines that can be sent into hostile environments.\n\nDefunct ",
					UI.FormatAsLink("Biobots", "STORYTRAITMORBROVER"),
					" drop harvestable ",
					UI.FormatAsLink("Steel", "STEEL"),
					"."
				});
			}

			// Token: 0x02002720 RID: 10016
			public class FOSSILDIG
			{
				// Token: 0x0400AD39 RID: 44345
				public static LocString NAME = "Ancient Specimen";

				// Token: 0x0400AD3A RID: 44346
				public static LocString DESC = "It's not from around here.";

				// Token: 0x0400AD3B RID: 44347
				public static LocString EFFECT = "Contains a partial " + UI.FormatAsLink("Fossil", "FOSSIL") + " left behind by a giant critter.\n\nStudying the full skeleton could yield the information required to access a valuable new resource.";
			}

			// Token: 0x02002721 RID: 10017
			public class FOSSILDIG_COMPLETED
			{
				// Token: 0x0400AD3C RID: 44348
				public static LocString NAME = "Fossil Quarry";

				// Token: 0x0400AD3D RID: 44349
				public static LocString DESC = "There sure are a lot of old bones in this area.";

				// Token: 0x0400AD3E RID: 44350
				public static LocString EFFECT = "Contains a deep cache of harvestable " + UI.FormatAsLink("Fossils", "FOSSIL") + ".";
			}

			// Token: 0x02002722 RID: 10018
			public class GAMMARAYOVEN
			{
				// Token: 0x0400AD3F RID: 44351
				public static LocString NAME = UI.FormatAsLink("Gamma Ray Oven", "GAMMARAYOVEN");

				// Token: 0x0400AD40 RID: 44352
				public static LocString DESC = "Nuke your food.";

				// Token: 0x0400AD41 RID: 44353
				public static LocString EFFECT = "Cooks a variety of " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x02002723 RID: 10019
			public class GASCARGOBAY
			{
				// Token: 0x0400AD42 RID: 44354
				public static LocString NAME = UI.FormatAsLink("Gas Cargo Canister", "GASCARGOBAY");

				// Token: 0x0400AD43 RID: 44355
				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				// Token: 0x0400AD44 RID: 44356
				public static LocString EFFECT = "Allows Duplicants to store any " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			// Token: 0x02002724 RID: 10020
			public class GASCARGOBAYCLUSTER
			{
				// Token: 0x0400AD45 RID: 44357
				public static LocString NAME = UI.FormatAsLink("Large Gas Cargo Canister", "GASCARGOBAY");

				// Token: 0x0400AD46 RID: 44358
				public static LocString DESC = "Holds more than a typical gas cargo canister.";

				// Token: 0x0400AD47 RID: 44359
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store most of the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002725 RID: 10021
			public class GASCARGOBAYSMALL
			{
				// Token: 0x0400AD48 RID: 44360
				public static LocString NAME = UI.FormatAsLink("Gas Cargo Canister", "GASCARGOBAYSMALL");

				// Token: 0x0400AD49 RID: 44361
				public static LocString DESC = "Duplicants fill cargo canisters with any resources they find during space missions.";

				// Token: 0x0400AD4A RID: 44362
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store some of the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002726 RID: 10022
			public class GASCONDUIT
			{
				// Token: 0x0400AD4B RID: 44363
				public static LocString NAME = UI.FormatAsLink("Gas Pipe", "GASCONDUIT");

				// Token: 0x0400AD4C RID: 44364
				public static LocString DESC = "Gas pipes are used to connect the inputs and outputs of ventilated buildings.";

				// Token: 0x0400AD4D RID: 44365
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" between ",
					UI.FormatAsLink("Outputs", "GASPIPING"),
					" and ",
					UI.FormatAsLink("Intakes", "GASPIPING"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002727 RID: 10023
			public class GASCONDUITBRIDGE
			{
				// Token: 0x0400AD4E RID: 44366
				public static LocString NAME = UI.FormatAsLink("Gas Bridge", "GASCONDUITBRIDGE");

				// Token: 0x0400AD4F RID: 44367
				public static LocString DESC = "Separate pipe systems prevent mingled contents from causing building damage.";

				// Token: 0x0400AD50 RID: 44368
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Gas Pipe", "GASPIPING") + " section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x02002728 RID: 10024
			public class GASCONDUITPREFERENTIALFLOW
			{
				// Token: 0x0400AD51 RID: 44369
				public static LocString NAME = UI.FormatAsLink("Priority Gas Flow", "GASCONDUITPREFERENTIALFLOW");

				// Token: 0x0400AD52 RID: 44370
				public static LocString DESC = "Priority flows ensure important buildings are filled first when on a system with other buildings.";

				// Token: 0x0400AD53 RID: 44371
				public static LocString EFFECT = "Diverts " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " to a secondary input when its primary input overflows.";
			}

			// Token: 0x02002729 RID: 10025
			public class LIQUIDCONDUITPREFERENTIALFLOW
			{
				// Token: 0x0400AD54 RID: 44372
				public static LocString NAME = UI.FormatAsLink("Priority Liquid Flow", "LIQUIDCONDUITPREFERENTIALFLOW");

				// Token: 0x0400AD55 RID: 44373
				public static LocString DESC = "Priority flows ensure important buildings are filled first when on a system with other buildings.";

				// Token: 0x0400AD56 RID: 44374
				public static LocString EFFECT = "Diverts " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " to a secondary input when its primary input overflows.";
			}

			// Token: 0x0200272A RID: 10026
			public class GASCONDUITOVERFLOW
			{
				// Token: 0x0400AD57 RID: 44375
				public static LocString NAME = UI.FormatAsLink("Gas Overflow Valve", "GASCONDUITOVERFLOW");

				// Token: 0x0400AD58 RID: 44376
				public static LocString DESC = "Overflow valves can be used to prioritize which buildings should receive precious resources first.";

				// Token: 0x0400AD59 RID: 44377
				public static LocString EFFECT = "Fills a secondary" + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " output only when its primary output is blocked.";
			}

			// Token: 0x0200272B RID: 10027
			public class LIQUIDCONDUITOVERFLOW
			{
				// Token: 0x0400AD5A RID: 44378
				public static LocString NAME = UI.FormatAsLink("Liquid Overflow Valve", "LIQUIDCONDUITOVERFLOW");

				// Token: 0x0400AD5B RID: 44379
				public static LocString DESC = "Overflow valves can be used to prioritize which buildings should receive precious resources first.";

				// Token: 0x0400AD5C RID: 44380
				public static LocString EFFECT = "Fills a secondary" + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " output only when its primary output is blocked.";
			}

			// Token: 0x0200272C RID: 10028
			public class LAUNCHPAD
			{
				// Token: 0x0400AD5D RID: 44381
				public static LocString NAME = UI.FormatAsLink("Rocket Platform", "LAUNCHPAD");

				// Token: 0x0400AD5E RID: 44382
				public static LocString DESC = "A platform from which rockets can be launched and on which they can land.";

				// Token: 0x0400AD5F RID: 44383
				public static LocString EFFECT = "Precursor to construction of all other Rocket modules.\n\nAllows Rockets to launch from or land on the host Planetoid.\n\nAutomatically links up to " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + UI.FormatAsLink("s", "MODULARLAUNCHPADPORTSOLID") + " built to either side of the platform.";

				// Token: 0x0400AD60 RID: 44384
				public static LocString LOGIC_PORT_READY = "Rocket Checklist";

				// Token: 0x0400AD61 RID: 44385
				public static LocString LOGIC_PORT_READY_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket is ready for flight";

				// Token: 0x0400AD62 RID: 44386
				public static LocString LOGIC_PORT_READY_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400AD63 RID: 44387
				public static LocString LOGIC_PORT_LANDED_ROCKET = "Landed Rocket";

				// Token: 0x0400AD64 RID: 44388
				public static LocString LOGIC_PORT_LANDED_ROCKET_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket is on the " + BUILDINGS.PREFABS.LAUNCHPAD.NAME;

				// Token: 0x0400AD65 RID: 44389
				public static LocString LOGIC_PORT_LANDED_ROCKET_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400AD66 RID: 44390
				public static LocString LOGIC_PORT_LAUNCH = "Launch Rocket";

				// Token: 0x0400AD67 RID: 44391
				public static LocString LOGIC_PORT_LAUNCH_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket";

				// Token: 0x0400AD68 RID: 44392
				public static LocString LOGIC_PORT_LAUNCH_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Cancel launch";
			}

			// Token: 0x0200272D RID: 10029
			public class GASFILTER
			{
				// Token: 0x0400AD69 RID: 44393
				public static LocString NAME = UI.FormatAsLink("Gas Filter", "GASFILTER");

				// Token: 0x0400AD6A RID: 44394
				public static LocString DESC = "All gases are sent into the building's output pipe, except the gas chosen for filtering.";

				// Token: 0x0400AD6B RID: 44395
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sieves one ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" from the air, sending it into a dedicated ",
					UI.FormatAsLink("Pipe", "GASPIPING"),
					"."
				});

				// Token: 0x0400AD6C RID: 44396
				public static LocString STATUS_ITEM = "Filters: {0}";

				// Token: 0x0400AD6D RID: 44397
				public static LocString ELEMENT_NOT_SPECIFIED = "Not Specified";
			}

			// Token: 0x0200272E RID: 10030
			public class SOLIDFILTER
			{
				// Token: 0x0400AD6E RID: 44398
				public static LocString NAME = UI.FormatAsLink("Solid Filter", "SOLIDFILTER");

				// Token: 0x0400AD6F RID: 44399
				public static LocString DESC = "All solids are sent into the building's output conveyor, except the solid chosen for filtering.";

				// Token: 0x0400AD70 RID: 44400
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Separates one ",
					UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID"),
					" from the conveyor, sending it into a dedicated ",
					BUILDINGS.PREFABS.SOLIDCONDUIT.NAME,
					"."
				});

				// Token: 0x0400AD71 RID: 44401
				public static LocString STATUS_ITEM = "Filters: {0}";

				// Token: 0x0400AD72 RID: 44402
				public static LocString ELEMENT_NOT_SPECIFIED = "Not Specified";
			}

			// Token: 0x0200272F RID: 10031
			public class GASPERMEABLEMEMBRANE
			{
				// Token: 0x0400AD73 RID: 44403
				public static LocString NAME = UI.FormatAsLink("Airflow Tile", "GASPERMEABLEMEMBRANE");

				// Token: 0x0400AD74 RID: 44404
				public static LocString DESC = "Building with airflow tile promotes better gas circulation within a colony.";

				// Token: 0x0400AD75 RID: 44405
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nBlocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" flow without obstructing ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x02002730 RID: 10032
			public class DEVPUMPGAS
			{
				// Token: 0x0400AD76 RID: 44406
				public static LocString NAME = "Dev Pump Gas";

				// Token: 0x0400AD77 RID: 44407
				public static LocString DESC = "Piping a pump's output to a building's intake will send gas to that building.";

				// Token: 0x0400AD78 RID: 44408
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					".\n\nMust be immersed in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x02002731 RID: 10033
			public class GASPUMP
			{
				// Token: 0x0400AD79 RID: 44409
				public static LocString NAME = UI.FormatAsLink("Gas Pump", "GASPUMP");

				// Token: 0x0400AD7A RID: 44410
				public static LocString DESC = "Piping a pump's output to a building's intake will send gas to that building.";

				// Token: 0x0400AD7B RID: 44411
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					".\n\nMust be immersed in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x02002732 RID: 10034
			public class GASMINIPUMP
			{
				// Token: 0x0400AD7C RID: 44412
				public static LocString NAME = UI.FormatAsLink("Mini Gas Pump", "GASMINIPUMP");

				// Token: 0x0400AD7D RID: 44413
				public static LocString DESC = "Mini pumps are useful for moving small quantities of gas with minimum power.";

				// Token: 0x0400AD7E RID: 44414
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in a small amount of ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					".\n\nMust be immersed in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x02002733 RID: 10035
			public class GASVALVE
			{
				// Token: 0x0400AD7F RID: 44415
				public static LocString NAME = UI.FormatAsLink("Gas Valve", "GASVALVE");

				// Token: 0x0400AD80 RID: 44416
				public static LocString DESC = "Valves control the amount of gas that moves through pipes, preventing waste.";

				// Token: 0x0400AD81 RID: 44417
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Controls the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" volume permitted through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					"."
				});
			}

			// Token: 0x02002734 RID: 10036
			public class GASLOGICVALVE
			{
				// Token: 0x0400AD82 RID: 44418
				public static LocString NAME = UI.FormatAsLink("Gas Shutoff", "GASLOGICVALVE");

				// Token: 0x0400AD83 RID: 44419
				public static LocString DESC = "Automated piping saves power and time by removing the need for Duplicant input.";

				// Token: 0x0400AD84 RID: 44420
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow on or off."
				});

				// Token: 0x0400AD85 RID: 44421
				public static LocString LOGIC_PORT = "Open/Close";

				// Token: 0x0400AD86 RID: 44422
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow gas flow";

				// Token: 0x0400AD87 RID: 44423
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent gas flow";
			}

			// Token: 0x02002735 RID: 10037
			public class GASLIMITVALVE
			{
				// Token: 0x0400AD88 RID: 44424
				public static LocString NAME = UI.FormatAsLink("Gas Meter Valve", "GASLIMITVALVE");

				// Token: 0x0400AD89 RID: 44425
				public static LocString DESC = "Meter Valves let an exact amount of gas pass through before shutting off.";

				// Token: 0x0400AD8A RID: 44426
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow off when the specified amount has passed through it."
				});

				// Token: 0x0400AD8B RID: 44427
				public static LocString LOGIC_PORT_OUTPUT = "Limit Reached";

				// Token: 0x0400AD8C RID: 44428
				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if limit has been reached";

				// Token: 0x0400AD8D RID: 44429
				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400AD8E RID: 44430
				public static LocString LOGIC_PORT_RESET = "Reset Meter";

				// Token: 0x0400AD8F RID: 44431
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the amount";

				// Token: 0x0400AD90 RID: 44432
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x02002736 RID: 10038
			public class GASVENT
			{
				// Token: 0x0400AD91 RID: 44433
				public static LocString NAME = UI.FormatAsLink("Gas Vent", "GASVENT");

				// Token: 0x0400AD92 RID: 44434
				public static LocString DESC = "Vents are an exit point for gases from ventilation systems.";

				// Token: 0x0400AD93 RID: 44435
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Releases ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" from ",
					UI.FormatAsLink("Gas Pipes", "GASPIPING"),
					"."
				});
			}

			// Token: 0x02002737 RID: 10039
			public class GASVENTHIGHPRESSURE
			{
				// Token: 0x0400AD94 RID: 44436
				public static LocString NAME = UI.FormatAsLink("High Pressure Gas Vent", "GASVENTHIGHPRESSURE");

				// Token: 0x0400AD95 RID: 44437
				public static LocString DESC = "High pressure vents can expel gas into more highly pressurized environments.";

				// Token: 0x0400AD96 RID: 44438
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Releases ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" from ",
					UI.FormatAsLink("Gas Pipes", "GASPIPING"),
					" into high pressure locations."
				});
			}

			// Token: 0x02002738 RID: 10040
			public class GASBOTTLER
			{
				// Token: 0x0400AD97 RID: 44439
				public static LocString NAME = UI.FormatAsLink("Canister Filler", "GASBOTTLER");

				// Token: 0x0400AD98 RID: 44440
				public static LocString DESC = "Canisters allow Duplicants to manually deliver gases from place to place.";

				// Token: 0x0400AD99 RID: 44441
				public static LocString EFFECT = "Automatically stores piped " + UI.FormatAsLink("Gases", "ELEMENTS_GAS") + " into canisters for manual transport.";
			}

			// Token: 0x02002739 RID: 10041
			public class LIQUIDBOTTLER
			{
				// Token: 0x0400AD9A RID: 44442
				public static LocString NAME = UI.FormatAsLink("Bottle Filler", "LIQUIDBOTTLER");

				// Token: 0x0400AD9B RID: 44443
				public static LocString DESC = "Bottle fillers allow Duplicants to manually deliver liquids from place to place.";

				// Token: 0x0400AD9C RID: 44444
				public static LocString EFFECT = "Automatically stores piped " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " into bottles for manual transport.";
			}

			// Token: 0x0200273A RID: 10042
			public class GENERATOR
			{
				// Token: 0x0400AD9D RID: 44445
				public static LocString NAME = UI.FormatAsLink("Coal Generator", "GENERATOR");

				// Token: 0x0400AD9E RID: 44446
				public static LocString DESC = "Burning coal produces more energy than manual power, but emits heat and exhaust.";

				// Token: 0x0400AD9F RID: 44447
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Coal", "CARBON"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					"."
				});

				// Token: 0x0400ADA0 RID: 44448
				public static LocString OVERPRODUCTION = "{Generator} overproduction";
			}

			// Token: 0x0200273B RID: 10043
			public class GENETICANALYSISSTATION
			{
				// Token: 0x0400ADA1 RID: 44449
				public static LocString NAME = UI.FormatAsLink("Botanical Analyzer", "GENETICANALYSISSTATION");

				// Token: 0x0400ADA2 RID: 44450
				public static LocString DESC = "Would a mutated rose still smell as sweet?";

				// Token: 0x0400ADA3 RID: 44451
				public static LocString EFFECT = "Identifies new " + UI.FormatAsLink("Seed", "PLANTS") + " subspecies.";
			}

			// Token: 0x0200273C RID: 10044
			public class DEVGENERATOR
			{
				// Token: 0x0400ADA4 RID: 44452
				public static LocString NAME = "Dev Generator";

				// Token: 0x0400ADA5 RID: 44453
				public static LocString DESC = "Runs on coffee.";

				// Token: 0x0400ADA6 RID: 44454
				public static LocString EFFECT = "Generates testing power for late nights.";
			}

			// Token: 0x0200273D RID: 10045
			public class DEVLIFESUPPORT
			{
				// Token: 0x0400ADA7 RID: 44455
				public static LocString NAME = "Dev Life Support";

				// Token: 0x0400ADA8 RID: 44456
				public static LocString DESC = "Keeps Duplicants cozy and breathing.";

				// Token: 0x0400ADA9 RID: 44457
				public static LocString EFFECT = "Generates warm, oxygen-rich air.";
			}

			// Token: 0x0200273E RID: 10046
			public class DEVLIGHTGENERATOR
			{
				// Token: 0x0400ADAA RID: 44458
				public static LocString NAME = "Dev Light Source";

				// Token: 0x0400ADAB RID: 44459
				public static LocString DESC = "Brightens up a dev's darkest hours.";

				// Token: 0x0400ADAC RID: 44460
				public static LocString EFFECT = "Generates dimmable light on demand.";

				// Token: 0x0400ADAD RID: 44461
				public static LocString FALLOFF_LABEL = "Falloff Rate";

				// Token: 0x0400ADAE RID: 44462
				public static LocString BRIGHTNESS_LABEL = "Brightness";

				// Token: 0x0400ADAF RID: 44463
				public static LocString RANGE_LABEL = "Range";
			}

			// Token: 0x0200273F RID: 10047
			public class DEVRADIATIONGENERATOR
			{
				// Token: 0x0400ADB0 RID: 44464
				public static LocString NAME = "Dev Radiation Emitter";

				// Token: 0x0400ADB1 RID: 44465
				public static LocString DESC = "That's some <i>strong</i> coffee.";

				// Token: 0x0400ADB2 RID: 44466
				public static LocString EFFECT = "Generates on-demand radiation to keep things clear. <i>Nu-</i>clear.";
			}

			// Token: 0x02002740 RID: 10048
			public class DEVHEATER
			{
				// Token: 0x0400ADB3 RID: 44467
				public static LocString NAME = "Dev Heater";

				// Token: 0x0400ADB4 RID: 44468
				public static LocString DESC = "Did someone touch the thermostat?";

				// Token: 0x0400ADB5 RID: 44469
				public static LocString EFFECT = "Generates on-demand heat for testing toastiness.";
			}

			// Token: 0x02002741 RID: 10049
			public class GENERICFABRICATOR
			{
				// Token: 0x0400ADB6 RID: 44470
				public static LocString NAME = UI.FormatAsLink("Omniprinter", "GENERICFABRICATOR");

				// Token: 0x0400ADB7 RID: 44471
				public static LocString DESC = "Omniprinters are incapable of printing organic matter.";

				// Token: 0x0400ADB8 RID: 44472
				public static LocString EFFECT = "Converts " + UI.FormatAsLink("Raw Mineral", "RAWMINERAL") + " into unique materials and objects.";
			}

			// Token: 0x02002742 RID: 10050
			public class GEOTUNER
			{
				// Token: 0x0400ADB9 RID: 44473
				public static LocString NAME = UI.FormatAsLink("Geotuner", "GEOTUNER");

				// Token: 0x0400ADBA RID: 44474
				public static LocString DESC = "The targeted geyser receives stored amplification data when it is erupting.";

				// Token: 0x0400ADBB RID: 44475
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases the ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" and output of an analyzed ",
					UI.FormatAsLink("Geyser", "GEYSERS"),
					".\n\nMultiple Geotuners can be directed at a single ",
					UI.FormatAsLink("Geyser", "GEYSERS"),
					" anywhere on an asteroid."
				});

				// Token: 0x0400ADBC RID: 44476
				public static LocString LOGIC_PORT = "Geyser Eruption Monitor";

				// Token: 0x0400ADBD RID: 44477
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when geyser is erupting";

				// Token: 0x0400ADBE RID: 44478
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002743 RID: 10051
			public class GRAVE
			{
				// Token: 0x0400ADBF RID: 44479
				public static LocString NAME = UI.FormatAsLink("Tasteful Memorial", "GRAVE");

				// Token: 0x0400ADC0 RID: 44480
				public static LocString DESC = "Burying dead Duplicants reduces health hazards and stress on the colony.";

				// Token: 0x0400ADC1 RID: 44481
				public static LocString EFFECT = "Provides a final resting place for deceased Duplicants.\n\nLiving Duplicants will automatically place an unburied corpse inside.";
			}

			// Token: 0x02002744 RID: 10052
			public class HEADQUARTERS
			{
				// Token: 0x0400ADC2 RID: 44482
				public static LocString NAME = UI.FormatAsLink("Printing Pod", "HEADQUARTERS");

				// Token: 0x0400ADC3 RID: 44483
				public static LocString DESC = "New Duplicants come out here, but thank goodness, they never go back in.";

				// Token: 0x0400ADC4 RID: 44484
				public static LocString EFFECT = "An exceptionally advanced bioprinter of unknown origin.\n\nIt periodically produces new Duplicants or care packages containing resources.";
			}

			// Token: 0x02002745 RID: 10053
			public class HYDROGENGENERATOR
			{
				// Token: 0x0400ADC5 RID: 44485
				public static LocString NAME = UI.FormatAsLink("Hydrogen Generator", "HYDROGENGENERATOR");

				// Token: 0x0400ADC6 RID: 44486
				public static LocString DESC = "Hydrogen generators are extremely efficient, emitting next to no waste.";

				// Token: 0x0400ADC7 RID: 44487
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					"."
				});
			}

			// Token: 0x02002746 RID: 10054
			public class METHANEGENERATOR
			{
				// Token: 0x0400ADC8 RID: 44488
				public static LocString NAME = UI.FormatAsLink("Natural Gas Generator", "METHANEGENERATOR");

				// Token: 0x0400ADC9 RID: 44489
				public static LocString DESC = "Natural gas generators leak polluted water and are best built above a waste reservoir.";

				// Token: 0x0400ADCA RID: 44490
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Natural Gas", "METHANE"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"."
				});
			}

			// Token: 0x02002747 RID: 10055
			public class NUCLEARREACTOR
			{
				// Token: 0x0400ADCB RID: 44491
				public static LocString NAME = UI.FormatAsLink("Research Reactor", "NUCLEARREACTOR");

				// Token: 0x0400ADCC RID: 44492
				public static LocString DESC = "Radbolt generators and reflectors make radiation useable by other buildings.";

				// Token: 0x0400ADCD RID: 44493
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" to produce ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" for Radbolt production.\n\nGenerates a massive amount of ",
					UI.FormatAsLink("Heat", "HEAT"),
					". Overheating will result in an explosive meltdown."
				});

				// Token: 0x0400ADCE RID: 44494
				public static LocString LOGIC_PORT = "Fuel Delivery Control";

				// Token: 0x0400ADCF RID: 44495
				public static LocString INPUT_PORT_ACTIVE = "Fuel Delivery Enabled";

				// Token: 0x0400ADD0 RID: 44496
				public static LocString INPUT_PORT_INACTIVE = "Fuel Delivery Disabled";
			}

			// Token: 0x02002748 RID: 10056
			public class WOODGASGENERATOR
			{
				// Token: 0x0400ADD1 RID: 44497
				public static LocString NAME = UI.FormatAsLink("Wood Burner", "WOODGASGENERATOR");

				// Token: 0x0400ADD2 RID: 44498
				public static LocString DESC = "Wood burners are small and easy to maintain, but produce a fair amount of heat.";

				// Token: 0x0400ADD3 RID: 44499
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Wood", "WOOD"),
					" to produce electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					UI.FormatAsLink("Heat", "HEAT"),
					"."
				});
			}

			// Token: 0x02002749 RID: 10057
			public class PETROLEUMGENERATOR
			{
				// Token: 0x0400ADD4 RID: 44500
				public static LocString NAME = UI.FormatAsLink("Petroleum Generator", "PETROLEUMGENERATOR");

				// Token: 0x0400ADD5 RID: 44501
				public static LocString DESC = "Petroleum generators have a high energy output but produce a great deal of waste.";

				// Token: 0x0400ADD6 RID: 44502
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts either ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" or ",
					UI.FormatAsLink("Ethanol", "ETHANOL"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"."
				});
			}

			// Token: 0x0200274A RID: 10058
			public class HYDROPONICFARM
			{
				// Token: 0x0400ADD7 RID: 44503
				public static LocString NAME = UI.FormatAsLink("Hydroponic Farm", "HYDROPONICFARM");

				// Token: 0x0400ADD8 RID: 44504
				public static LocString DESC = "Hydroponic farms reduce Duplicant traffic by automating irrigating crops.";

				// Token: 0x0400ADD9 RID: 44505
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Grows one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" from a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nCan be used as floor tile and rotated before construction.\n\nMust be irrigated through ",
					UI.FormatAsLink("Liquid Piping", "LIQUIDPIPING"),
					"."
				});
			}

			// Token: 0x0200274B RID: 10059
			public class INSULATEDGASCONDUIT
			{
				// Token: 0x0400ADDA RID: 44506
				public static LocString NAME = UI.FormatAsLink("Insulated Gas Pipe", "INSULATEDGASCONDUIT");

				// Token: 0x0400ADDB RID: 44507
				public static LocString DESC = "Pipe insulation prevents gas contents from significantly changing temperature in transit.";

				// Token: 0x0400ADDC RID: 44508
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" with minimal change in ",
					UI.FormatAsLink("Temperature", "HEAT"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x0200274C RID: 10060
			public class GASCONDUITRADIANT
			{
				// Token: 0x0400ADDD RID: 44509
				public static LocString NAME = UI.FormatAsLink("Radiant Gas Pipe", "GASCONDUITRADIANT");

				// Token: 0x0400ADDE RID: 44510
				public static LocString DESC = "Radiant pipes pumping cold gas can be run through hot areas to help cool them down.";

				// Token: 0x0400ADDF RID: 44511
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					", allowing extreme ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" exchange with the surrounding environment.\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x0200274D RID: 10061
			public class INSULATEDLIQUIDCONDUIT
			{
				// Token: 0x0400ADE0 RID: 44512
				public static LocString NAME = UI.FormatAsLink("Insulated Liquid Pipe", "INSULATEDLIQUIDCONDUIT");

				// Token: 0x0400ADE1 RID: 44513
				public static LocString DESC = "Pipe insulation prevents liquid contents from significantly changing temperature in transit.";

				// Token: 0x0400ADE2 RID: 44514
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" with minimal change in ",
					UI.FormatAsLink("Temperature", "HEAT"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x0200274E RID: 10062
			public class LIQUIDCONDUITRADIANT
			{
				// Token: 0x0400ADE3 RID: 44515
				public static LocString NAME = UI.FormatAsLink("Radiant Liquid Pipe", "LIQUIDCONDUITRADIANT");

				// Token: 0x0400ADE4 RID: 44516
				public static LocString DESC = "Radiant pipes pumping cold liquid can be run through hot areas to help cool them down.";

				// Token: 0x0400ADE5 RID: 44517
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", allowing extreme ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" exchange with the surrounding environment.\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x0200274F RID: 10063
			public class CONTACTCONDUCTIVEPIPEBRIDGE
			{
				// Token: 0x0400ADE6 RID: 44518
				public static LocString NAME = UI.FormatAsLink("Conduction Panel", "CONTACTCONDUCTIVEPIPEBRIDGE");

				// Token: 0x0400ADE7 RID: 44519
				public static LocString DESC = "It can transfer heat effectively even if no liquid is passing through.";

				// Token: 0x0400ADE8 RID: 44520
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", allowing extreme ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" exchange with overlapping buildings.\n\nCan function in a vacuum.\n\nCan be run through wall and floor tiles."
				});
			}

			// Token: 0x02002750 RID: 10064
			public class INSULATEDWIRE
			{
				// Token: 0x0400ADE9 RID: 44521
				public static LocString NAME = UI.FormatAsLink("Insulated Wire", "INSULATEDWIRE");

				// Token: 0x0400ADEA RID: 44522
				public static LocString DESC = "This stuff won't go melting if things get heated.";

				// Token: 0x0400ADEB RID: 44523
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects buildings to ",
					UI.FormatAsLink("Power", "POWER"),
					" sources in extreme ",
					UI.FormatAsLink("Heat", "HEAT"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002751 RID: 10065
			public class INSULATIONTILE
			{
				// Token: 0x0400ADEC RID: 44524
				public static LocString NAME = UI.FormatAsLink("Insulated Tile", "INSULATIONTILE");

				// Token: 0x0400ADED RID: 44525
				public static LocString DESC = "The low thermal conductivity of insulated tiles slows any heat passing through them.";

				// Token: 0x0400ADEE RID: 44526
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nReduces " + UI.FormatAsLink("Heat", "HEAT") + " transfer between walls, retaining ambient heat in an area.";
			}

			// Token: 0x02002752 RID: 10066
			public class EXTERIORWALL
			{
				// Token: 0x0400ADEF RID: 44527
				public static LocString NAME = UI.FormatAsLink("Drywall", "EXTERIORWALL");

				// Token: 0x0400ADF0 RID: 44528
				public static LocString DESC = "Drywall can be used in conjunction with tiles to build airtight rooms on the surface.";

				// Token: 0x0400ADF1 RID: 44529
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Prevents ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" loss in space.\n\nBuilds an insulating backwall behind buildings."
				});

				// Token: 0x02003559 RID: 13657
				public class FACADES
				{
					// Token: 0x020038CA RID: 14538
					public class DEFAULT_EXTERIORWALL
					{
						// Token: 0x0400E087 RID: 57479
						public static LocString NAME = UI.FormatAsLink("Drywall", "EXTERIORWALL");

						// Token: 0x0400E088 RID: 57480
						public static LocString DESC = "It gets the job done.";
					}

					// Token: 0x020038CB RID: 14539
					public class BALM_LILY
					{
						// Token: 0x0400E089 RID: 57481
						public static LocString NAME = UI.FormatAsLink("Balm Lily Print", "EXTERIORWALL");

						// Token: 0x0400E08A RID: 57482
						public static LocString DESC = "A mellow floral wallpaper.";
					}

					// Token: 0x020038CC RID: 14540
					public class CLOUDS
					{
						// Token: 0x0400E08B RID: 57483
						public static LocString NAME = UI.FormatAsLink("Cloud Print", "EXTERIORWALL");

						// Token: 0x0400E08C RID: 57484
						public static LocString DESC = "A soft, fluffy wallpaper.";
					}

					// Token: 0x020038CD RID: 14541
					public class MUSHBAR
					{
						// Token: 0x0400E08D RID: 57485
						public static LocString NAME = UI.FormatAsLink("Mush Bar Print", "EXTERIORWALL");

						// Token: 0x0400E08E RID: 57486
						public static LocString DESC = "A gag-inducing wallpaper.";
					}

					// Token: 0x020038CE RID: 14542
					public class PLAID
					{
						// Token: 0x0400E08F RID: 57487
						public static LocString NAME = UI.FormatAsLink("Aqua Plaid Print", "EXTERIORWALL");

						// Token: 0x0400E090 RID: 57488
						public static LocString DESC = "A cozy flannel wallpaper.";
					}

					// Token: 0x020038CF RID: 14543
					public class RAIN
					{
						// Token: 0x0400E091 RID: 57489
						public static LocString NAME = UI.FormatAsLink("Rainy Print", "EXTERIORWALL");

						// Token: 0x0400E092 RID: 57490
						public static LocString DESC = "A precipitation-themed wallpaper.";
					}

					// Token: 0x020038D0 RID: 14544
					public class AQUATICMOSAIC
					{
						// Token: 0x0400E093 RID: 57491
						public static LocString NAME = UI.FormatAsLink("Aquatic Mosaic", "EXTERIORWALL");

						// Token: 0x0400E094 RID: 57492
						public static LocString DESC = "A multi-hued blue wallpaper.";
					}

					// Token: 0x020038D1 RID: 14545
					public class RAINBOW
					{
						// Token: 0x0400E095 RID: 57493
						public static LocString NAME = UI.FormatAsLink("Rainbow Stripe", "EXTERIORWALL");

						// Token: 0x0400E096 RID: 57494
						public static LocString DESC = "A wallpaper with <i>all</i> the colors.";
					}

					// Token: 0x020038D2 RID: 14546
					public class SNOW
					{
						// Token: 0x0400E097 RID: 57495
						public static LocString NAME = UI.FormatAsLink("Snowflake Print", "EXTERIORWALL");

						// Token: 0x0400E098 RID: 57496
						public static LocString DESC = "A wallpaper as unique as my colony.";
					}

					// Token: 0x020038D3 RID: 14547
					public class SUN
					{
						// Token: 0x0400E099 RID: 57497
						public static LocString NAME = UI.FormatAsLink("Sunshine Print", "EXTERIORWALL");

						// Token: 0x0400E09A RID: 57498
						public static LocString DESC = "A UV-free wallpaper.";
					}

					// Token: 0x020038D4 RID: 14548
					public class COFFEE
					{
						// Token: 0x0400E09B RID: 57499
						public static LocString NAME = UI.FormatAsLink("Cafe Print", "EXTERIORWALL");

						// Token: 0x0400E09C RID: 57500
						public static LocString DESC = "A caffeine-themed wallpaper.";
					}

					// Token: 0x020038D5 RID: 14549
					public class PASTELPOLKA
					{
						// Token: 0x0400E09D RID: 57501
						public static LocString NAME = UI.FormatAsLink("Pastel Polka Print", "EXTERIORWALL");

						// Token: 0x0400E09E RID: 57502
						public static LocString DESC = "A soothing, dotted wallpaper.";
					}

					// Token: 0x020038D6 RID: 14550
					public class PASTELBLUE
					{
						// Token: 0x0400E09F RID: 57503
						public static LocString NAME = UI.FormatAsLink("Pastel Blue", "EXTERIORWALL");

						// Token: 0x0400E0A0 RID: 57504
						public static LocString DESC = "A soothing blue wallpaper.";
					}

					// Token: 0x020038D7 RID: 14551
					public class PASTELGREEN
					{
						// Token: 0x0400E0A1 RID: 57505
						public static LocString NAME = UI.FormatAsLink("Pastel Green", "EXTERIORWALL");

						// Token: 0x0400E0A2 RID: 57506
						public static LocString DESC = "A soothing green wallpaper.";
					}

					// Token: 0x020038D8 RID: 14552
					public class PASTELPINK
					{
						// Token: 0x0400E0A3 RID: 57507
						public static LocString NAME = UI.FormatAsLink("Pastel Pink", "EXTERIORWALL");

						// Token: 0x0400E0A4 RID: 57508
						public static LocString DESC = "A soothing pink wallpaper.";
					}

					// Token: 0x020038D9 RID: 14553
					public class PASTELPURPLE
					{
						// Token: 0x0400E0A5 RID: 57509
						public static LocString NAME = UI.FormatAsLink("Pastel Purple", "EXTERIORWALL");

						// Token: 0x0400E0A6 RID: 57510
						public static LocString DESC = "A soothing purple wallpaper.";
					}

					// Token: 0x020038DA RID: 14554
					public class PASTELYELLOW
					{
						// Token: 0x0400E0A7 RID: 57511
						public static LocString NAME = UI.FormatAsLink("Pastel Yellow", "EXTERIORWALL");

						// Token: 0x0400E0A8 RID: 57512
						public static LocString DESC = "A soothing yellow wallpaper.";
					}

					// Token: 0x020038DB RID: 14555
					public class BASIC_WHITE
					{
						// Token: 0x0400E0A9 RID: 57513
						public static LocString NAME = UI.FormatAsLink("Fresh White", "EXTERIORWALL");

						// Token: 0x0400E0AA RID: 57514
						public static LocString DESC = "It's just so fresh and so clean.";
					}

					// Token: 0x020038DC RID: 14556
					public class DIAGONAL_RED_DEEP_WHITE
					{
						// Token: 0x0400E0AB RID: 57515
						public static LocString NAME = UI.FormatAsLink("Magma Diagonal", "EXTERIORWALL");

						// Token: 0x0400E0AC RID: 57516
						public static LocString DESC = "A red wallpaper with a diagonal stripe.";
					}

					// Token: 0x020038DD RID: 14557
					public class DIAGONAL_ORANGE_SATSUMA_WHITE
					{
						// Token: 0x0400E0AD RID: 57517
						public static LocString NAME = UI.FormatAsLink("Bright Diagonal", "EXTERIORWALL");

						// Token: 0x0400E0AE RID: 57518
						public static LocString DESC = "An orange wallpaper with a diagonal stripe.";
					}

					// Token: 0x020038DE RID: 14558
					public class DIAGONAL_YELLOW_LEMON_WHITE
					{
						// Token: 0x0400E0AF RID: 57519
						public static LocString NAME = UI.FormatAsLink("Yellowcake Diagonal", "EXTERIORWALL");

						// Token: 0x0400E0B0 RID: 57520
						public static LocString DESC = "A radiation-free wallpaper with a diagonal stripe.";
					}

					// Token: 0x020038DF RID: 14559
					public class DIAGONAL_GREEN_KELLY_WHITE
					{
						// Token: 0x0400E0B1 RID: 57521
						public static LocString NAME = UI.FormatAsLink("Algae Diagonal", "EXTERIORWALL");

						// Token: 0x0400E0B2 RID: 57522
						public static LocString DESC = "A slippery wallpaper with a diagonal stripe.";
					}

					// Token: 0x020038E0 RID: 14560
					public class DIAGONAL_BLUE_COBALT_WHITE
					{
						// Token: 0x0400E0B3 RID: 57523
						public static LocString NAME = UI.FormatAsLink("H2O Diagonal", "EXTERIORWALL");

						// Token: 0x0400E0B4 RID: 57524
						public static LocString DESC = "A damp wallpaper with a diagonal stripe.";
					}

					// Token: 0x020038E1 RID: 14561
					public class DIAGONAL_PINK_FLAMINGO_WHITE
					{
						// Token: 0x0400E0B5 RID: 57525
						public static LocString NAME = UI.FormatAsLink("Petal Diagonal", "EXTERIORWALL");

						// Token: 0x0400E0B6 RID: 57526
						public static LocString DESC = "A pink wallpaper with a diagonal stripe.";
					}

					// Token: 0x020038E2 RID: 14562
					public class DIAGONAL_GREY_CHARCOAL_WHITE
					{
						// Token: 0x0400E0B7 RID: 57527
						public static LocString NAME = UI.FormatAsLink("Charcoal Diagonal", "EXTERIORWALL");

						// Token: 0x0400E0B8 RID: 57528
						public static LocString DESC = "A sleek wallpaper with a diagonal stripe.";
					}

					// Token: 0x020038E3 RID: 14563
					public class CIRCLE_RED_DEEP_WHITE
					{
						// Token: 0x0400E0B9 RID: 57529
						public static LocString NAME = UI.FormatAsLink("Magma Wedge", "EXTERIORWALL");

						// Token: 0x0400E0BA RID: 57530
						public static LocString DESC = "It can be arranged into giant red polka dots.";
					}

					// Token: 0x020038E4 RID: 14564
					public class CIRCLE_ORANGE_SATSUMA_WHITE
					{
						// Token: 0x0400E0BB RID: 57531
						public static LocString NAME = UI.FormatAsLink("Bright Wedge", "EXTERIORWALL");

						// Token: 0x0400E0BC RID: 57532
						public static LocString DESC = "It can be arranged into giant orange polka dots.";
					}

					// Token: 0x020038E5 RID: 14565
					public class CIRCLE_YELLOW_LEMON_WHITE
					{
						// Token: 0x0400E0BD RID: 57533
						public static LocString NAME = UI.FormatAsLink("Yellowcake Wedge", "EXTERIORWALL");

						// Token: 0x0400E0BE RID: 57534
						public static LocString DESC = "A radiation-free pattern that can be arranged into giant polka dots.";
					}

					// Token: 0x020038E6 RID: 14566
					public class CIRCLE_GREEN_KELLY_WHITE
					{
						// Token: 0x0400E0BF RID: 57535
						public static LocString NAME = UI.FormatAsLink("Algae Wedge", "EXTERIORWALL");

						// Token: 0x0400E0C0 RID: 57536
						public static LocString DESC = "It can be arranged into giant green polka dots.";
					}

					// Token: 0x020038E7 RID: 14567
					public class CIRCLE_BLUE_COBALT_WHITE
					{
						// Token: 0x0400E0C1 RID: 57537
						public static LocString NAME = UI.FormatAsLink("H2O Wedge", "EXTERIORWALL");

						// Token: 0x0400E0C2 RID: 57538
						public static LocString DESC = "It can be arranged into giant blue polka dots.";
					}

					// Token: 0x020038E8 RID: 14568
					public class CIRCLE_PINK_FLAMINGO_WHITE
					{
						// Token: 0x0400E0C3 RID: 57539
						public static LocString NAME = UI.FormatAsLink("Petal Wedge", "EXTERIORWALL");

						// Token: 0x0400E0C4 RID: 57540
						public static LocString DESC = "It can be arranged into giant pink polka dots.";
					}

					// Token: 0x020038E9 RID: 14569
					public class CIRCLE_GREY_CHARCOAL_WHITE
					{
						// Token: 0x0400E0C5 RID: 57541
						public static LocString NAME = UI.FormatAsLink("Charcoal Wedge", "EXTERIORWALL");

						// Token: 0x0400E0C6 RID: 57542
						public static LocString DESC = "It can be arranged into giant shadowy polka dots.";
					}

					// Token: 0x020038EA RID: 14570
					public class BASIC_BLUE_COBALT
					{
						// Token: 0x0400E0C7 RID: 57543
						public static LocString NAME = UI.FormatAsLink("Solid Cobalt", "EXTERIORWALL");

						// Token: 0x0400E0C8 RID: 57544
						public static LocString DESC = "It doesn't cure the blues, so much as emphasize them.";
					}

					// Token: 0x020038EB RID: 14571
					public class BASIC_GREEN_KELLY
					{
						// Token: 0x0400E0C9 RID: 57545
						public static LocString NAME = UI.FormatAsLink("Spring Green", "EXTERIORWALL");

						// Token: 0x0400E0CA RID: 57546
						public static LocString DESC = "It's cheaper than having a garden.";
					}

					// Token: 0x020038EC RID: 14572
					public class BASIC_GREY_CHARCOAL
					{
						// Token: 0x0400E0CB RID: 57547
						public static LocString NAME = UI.FormatAsLink("Solid Charcoal", "EXTERIORWALL");

						// Token: 0x0400E0CC RID: 57548
						public static LocString DESC = "An elevated take on \"gray\".";
					}

					// Token: 0x020038ED RID: 14573
					public class BASIC_ORANGE_SATSUMA
					{
						// Token: 0x0400E0CD RID: 57549
						public static LocString NAME = UI.FormatAsLink("Solid Satsuma", "EXTERIORWALL");

						// Token: 0x0400E0CE RID: 57550
						public static LocString DESC = "Less fruit-forward, but just as fresh.";
					}

					// Token: 0x020038EE RID: 14574
					public class BASIC_PINK_FLAMINGO
					{
						// Token: 0x0400E0CF RID: 57551
						public static LocString NAME = UI.FormatAsLink("Solid Pink", "EXTERIORWALL");

						// Token: 0x0400E0D0 RID: 57552
						public static LocString DESC = "A bold statement, for bold Duplicants.";
					}

					// Token: 0x020038EF RID: 14575
					public class BASIC_RED_DEEP
					{
						// Token: 0x0400E0D1 RID: 57553
						public static LocString NAME = UI.FormatAsLink("Chili Red", "EXTERIORWALL");

						// Token: 0x0400E0D2 RID: 57554
						public static LocString DESC = "It really spices up dull walls.";
					}

					// Token: 0x020038F0 RID: 14576
					public class BASIC_YELLOW_LEMON
					{
						// Token: 0x0400E0D3 RID: 57555
						public static LocString NAME = UI.FormatAsLink("Canary Yellow", "EXTERIORWALL");

						// Token: 0x0400E0D4 RID: 57556
						public static LocString DESC = "The original coal-mine chic.";
					}

					// Token: 0x020038F1 RID: 14577
					public class BLUEBERRIES
					{
						// Token: 0x0400E0D5 RID: 57557
						public static LocString NAME = UI.FormatAsLink("Juicy Blueberry", "EXTERIORWALL");

						// Token: 0x0400E0D6 RID: 57558
						public static LocString DESC = "It stains the fingers.";
					}

					// Token: 0x020038F2 RID: 14578
					public class GRAPES
					{
						// Token: 0x0400E0D7 RID: 57559
						public static LocString NAME = UI.FormatAsLink("Grape Escape", "EXTERIORWALL");

						// Token: 0x0400E0D8 RID: 57560
						public static LocString DESC = "It's seedless, if that matters.";
					}

					// Token: 0x020038F3 RID: 14579
					public class LEMON
					{
						// Token: 0x0400E0D9 RID: 57561
						public static LocString NAME = UI.FormatAsLink("Sour Lemon", "EXTERIORWALL");

						// Token: 0x0400E0DA RID: 57562
						public static LocString DESC = "A bitter yet refreshing style.";
					}

					// Token: 0x020038F4 RID: 14580
					public class LIME
					{
						// Token: 0x0400E0DB RID: 57563
						public static LocString NAME = UI.FormatAsLink("Juicy Lime", "EXTERIORWALL");

						// Token: 0x0400E0DC RID: 57564
						public static LocString DESC = "Contains no actual vitamin C.";
					}

					// Token: 0x020038F5 RID: 14581
					public class SATSUMA
					{
						// Token: 0x0400E0DD RID: 57565
						public static LocString NAME = UI.FormatAsLink("Satsuma Slice", "EXTERIORWALL");

						// Token: 0x0400E0DE RID: 57566
						public static LocString DESC = "Adds some much-needed zest to the room.";
					}

					// Token: 0x020038F6 RID: 14582
					public class STRAWBERRY
					{
						// Token: 0x0400E0DF RID: 57567
						public static LocString NAME = UI.FormatAsLink("Strawberry Speckle", "EXTERIORWALL");

						// Token: 0x0400E0E0 RID: 57568
						public static LocString DESC = "Fruity freckles for naturally sweet spaces.";
					}

					// Token: 0x020038F7 RID: 14583
					public class WATERMELON
					{
						// Token: 0x0400E0E1 RID: 57569
						public static LocString NAME = UI.FormatAsLink("Juicy Watermelon", "EXTERIORWALL");

						// Token: 0x0400E0E2 RID: 57570
						public static LocString DESC = "Far more practical than gluing real fruit on a wall.";
					}

					// Token: 0x020038F8 RID: 14584
					public class TROPICAL
					{
						// Token: 0x0400E0E3 RID: 57571
						public static LocString NAME = UI.FormatAsLink("Sporechid Print", "EXTERIORWALL");

						// Token: 0x0400E0E4 RID: 57572
						public static LocString DESC = "The original scratch-and-sniff version was immediately recalled.";
					}

					// Token: 0x020038F9 RID: 14585
					public class TOILETPAPER
					{
						// Token: 0x0400E0E5 RID: 57573
						public static LocString NAME = UI.FormatAsLink("De-loo-xe", "EXTERIORWALL");

						// Token: 0x0400E0E6 RID: 57574
						public static LocString DESC = "Softly undulating lines create an undeniable air of loo-xury.";
					}

					// Token: 0x020038FA RID: 14586
					public class PLUNGER
					{
						// Token: 0x0400E0E7 RID: 57575
						public static LocString NAME = UI.FormatAsLink("Plunger Print", "EXTERIORWALL");

						// Token: 0x0400E0E8 RID: 57576
						public static LocString DESC = "Unclogs one's creative impulses.";
					}

					// Token: 0x020038FB RID: 14587
					public class STRIPES_BLUE
					{
						// Token: 0x0400E0E9 RID: 57577
						public static LocString NAME = UI.FormatAsLink("Blue Awning Stripe", "EXTERIORWALL");

						// Token: 0x0400E0EA RID: 57578
						public static LocString DESC = "Thick stripes in alternating shades of blue.";
					}

					// Token: 0x020038FC RID: 14588
					public class STRIPES_DIAGONAL_BLUE
					{
						// Token: 0x0400E0EB RID: 57579
						public static LocString NAME = UI.FormatAsLink("Blue Regimental Stripe", "EXTERIORWALL");

						// Token: 0x0400E0EC RID: 57580
						public static LocString DESC = "Inspired by the ties worn during intraoffice sports.";
					}

					// Token: 0x020038FD RID: 14589
					public class STRIPES_CIRCLE_BLUE
					{
						// Token: 0x0400E0ED RID: 57581
						public static LocString NAME = UI.FormatAsLink("Blue Circle Stripe", "EXTERIORWALL");

						// Token: 0x0400E0EE RID: 57582
						public static LocString DESC = "A stripe that curves to the right.";
					}

					// Token: 0x020038FE RID: 14590
					public class SQUARES_RED_DEEP_WHITE
					{
						// Token: 0x0400E0EF RID: 57583
						public static LocString NAME = UI.FormatAsLink("Magma Checkers", "EXTERIORWALL");

						// Token: 0x0400E0F0 RID: 57584
						public static LocString DESC = "They're so hot right now!";
					}

					// Token: 0x020038FF RID: 14591
					public class SQUARES_ORANGE_SATSUMA_WHITE
					{
						// Token: 0x0400E0F1 RID: 57585
						public static LocString NAME = UI.FormatAsLink("Bright Checkers", "EXTERIORWALL");

						// Token: 0x0400E0F2 RID: 57586
						public static LocString DESC = "Every tile feels like four tiles!";
					}

					// Token: 0x02003900 RID: 14592
					public class SQUARES_YELLOW_LEMON_WHITE
					{
						// Token: 0x0400E0F3 RID: 57587
						public static LocString NAME = UI.FormatAsLink("Yellowcake Checkers", "EXTERIORWALL");

						// Token: 0x0400E0F4 RID: 57588
						public static LocString DESC = "Any brighter, and they'd be radioactive!";
					}

					// Token: 0x02003901 RID: 14593
					public class SQUARES_GREEN_KELLY_WHITE
					{
						// Token: 0x0400E0F5 RID: 57589
						public static LocString NAME = UI.FormatAsLink("Algae Checkers", "EXTERIORWALL");

						// Token: 0x0400E0F6 RID: 57590
						public static LocString DESC = "Now with real simulated algae color!";
					}

					// Token: 0x02003902 RID: 14594
					public class SQUARES_BLUE_COBALT_WHITE
					{
						// Token: 0x0400E0F7 RID: 57591
						public static LocString NAME = UI.FormatAsLink("H2O Checkers", "EXTERIORWALL");

						// Token: 0x0400E0F8 RID: 57592
						public static LocString DESC = "Drink it all in!";
					}

					// Token: 0x02003903 RID: 14595
					public class SQUARES_PINK_FLAMINGO_WHITE
					{
						// Token: 0x0400E0F9 RID: 57593
						public static LocString NAME = UI.FormatAsLink("Petal Checkers", "EXTERIORWALL");

						// Token: 0x0400E0FA RID: 57594
						public static LocString DESC = "Fiercely fluorescent floral-inspired pink!";
					}

					// Token: 0x02003904 RID: 14596
					public class SQUARES_GREY_CHARCOAL_WHITE
					{
						// Token: 0x0400E0FB RID: 57595
						public static LocString NAME = UI.FormatAsLink("Charcoal Checkers", "EXTERIORWALL");

						// Token: 0x0400E0FC RID: 57596
						public static LocString DESC = "So retro!";
					}

					// Token: 0x02003905 RID: 14597
					public class KITCHEN_RETRO1
					{
						// Token: 0x0400E0FD RID: 57597
						public static LocString NAME = UI.FormatAsLink("Cafeteria Kitsch", "EXTERIORWALL");

						// Token: 0x0400E0FE RID: 57598
						public static LocString DESC = "Some diners find it nostalgic.";
					}

					// Token: 0x02003906 RID: 14598
					public class PLUS_RED_DEEP_WHITE
					{
						// Token: 0x0400E0FF RID: 57599
						public static LocString NAME = UI.FormatAsLink("Digital Chili", "EXTERIORWALL");

						// Token: 0x0400E100 RID: 57600
						public static LocString DESC = "A pixelated red-and-white print.";
					}

					// Token: 0x02003907 RID: 14599
					public class PLUS_ORANGE_SATSUMA_WHITE
					{
						// Token: 0x0400E101 RID: 57601
						public static LocString NAME = UI.FormatAsLink("Digital Satsuma", "EXTERIORWALL");

						// Token: 0x0400E102 RID: 57602
						public static LocString DESC = "A pixelated orange-and-white print.";
					}

					// Token: 0x02003908 RID: 14600
					public class PLUS_YELLOW_LEMON_WHITE
					{
						// Token: 0x0400E103 RID: 57603
						public static LocString NAME = UI.FormatAsLink("Digital Lemon", "EXTERIORWALL");

						// Token: 0x0400E104 RID: 57604
						public static LocString DESC = "A pixelated yellow-and-white print.";
					}

					// Token: 0x02003909 RID: 14601
					public class PLUS_GREEN_KELLY_WHITE
					{
						// Token: 0x0400E105 RID: 57605
						public static LocString NAME = UI.FormatAsLink("Digital Lawn", "EXTERIORWALL");

						// Token: 0x0400E106 RID: 57606
						public static LocString DESC = "A pixelated green-and-white print.";
					}

					// Token: 0x0200390A RID: 14602
					public class PLUS_BLUE_COBALT_WHITE
					{
						// Token: 0x0400E107 RID: 57607
						public static LocString NAME = UI.FormatAsLink("Digital Cobalt", "EXTERIORWALL");

						// Token: 0x0400E108 RID: 57608
						public static LocString DESC = "A pixelated blue-and-white print.";
					}

					// Token: 0x0200390B RID: 14603
					public class PLUS_PINK_FLAMINGO_WHITE
					{
						// Token: 0x0400E109 RID: 57609
						public static LocString NAME = UI.FormatAsLink("Digital Pink", "EXTERIORWALL");

						// Token: 0x0400E10A RID: 57610
						public static LocString DESC = "A pixelated pink-and-white print.";
					}

					// Token: 0x0200390C RID: 14604
					public class PLUS_GREY_CHARCOAL_WHITE
					{
						// Token: 0x0400E10B RID: 57611
						public static LocString NAME = UI.FormatAsLink("Digital Charcoal", "EXTERIORWALL");

						// Token: 0x0400E10C RID: 57612
						public static LocString DESC = "It's futuristic, so it must be good.";
					}

					// Token: 0x0200390D RID: 14605
					public class STRIPES_ROSE
					{
						// Token: 0x0400E10D RID: 57613
						public static LocString NAME = UI.FormatAsLink("Puce Stripe", "EXTERIORWALL");

						// Token: 0x0400E10E RID: 57614
						public static LocString DESC = "Vertical stripes make it quite obvious when nearby objects are askew.";
					}

					// Token: 0x0200390E RID: 14606
					public class STRIPES_DIAGONAL_ROSE
					{
						// Token: 0x0400E10F RID: 57615
						public static LocString NAME = UI.FormatAsLink("Puce Diagonal", "EXTERIORWALL");

						// Token: 0x0400E110 RID: 57616
						public static LocString DESC = "Some describe this color as \"squashed bug.\"";
					}

					// Token: 0x0200390F RID: 14607
					public class STRIPES_CIRCLE_ROSE
					{
						// Token: 0x0400E111 RID: 57617
						public static LocString NAME = UI.FormatAsLink("Puce Curves", "EXTERIORWALL");

						// Token: 0x0400E112 RID: 57618
						public static LocString DESC = "It's pronounced \"peeyoo-ss,\" a sound that Duplicants just can't seem to reproduce.";
					}

					// Token: 0x02003910 RID: 14608
					public class STRIPES_MUSH
					{
						// Token: 0x0400E113 RID: 57619
						public static LocString NAME = UI.FormatAsLink("Mush Stripe", "EXTERIORWALL");

						// Token: 0x0400E114 RID: 57620
						public static LocString DESC = "The kind of green that makes one feel slightly nauseated.";
					}

					// Token: 0x02003911 RID: 14609
					public class STRIPES_DIAGONAL_MUSH
					{
						// Token: 0x0400E115 RID: 57621
						public static LocString NAME = UI.FormatAsLink("Mush Diagonal", "EXTERIORWALL");

						// Token: 0x0400E116 RID: 57622
						public static LocString DESC = "Diagonal stripes in alternating shades of mush bar.";
					}

					// Token: 0x02003912 RID: 14610
					public class STRIPES_CIRCLE_MUSH
					{
						// Token: 0x0400E117 RID: 57623
						public static LocString NAME = UI.FormatAsLink("Mush Curves", "EXTERIORWALL");

						// Token: 0x0400E118 RID: 57624
						public static LocString DESC = "This wallpaper, like this colony's journey, is full of twists and turns.";
					}

					// Token: 0x02003913 RID: 14611
					public class STRIPES_YELLOW_TARTAR
					{
						// Token: 0x0400E119 RID: 57625
						public static LocString NAME = UI.FormatAsLink("Ick Stripe", "EXTERIORWALL");

						// Token: 0x0400E11A RID: 57626
						public static LocString DESC = "Vertical stripes make it quite obvious when nearby objects are askew.";
					}

					// Token: 0x02003914 RID: 14612
					public class STRIPES_DIAGONAL_YELLOW_TARTAR
					{
						// Token: 0x0400E11B RID: 57627
						public static LocString NAME = UI.FormatAsLink("Ick Diagonal", "EXTERIORWALL");

						// Token: 0x0400E11C RID: 57628
						public static LocString DESC = "Diagonal stripes in alternating shades of yellow.";
					}

					// Token: 0x02003915 RID: 14613
					public class STRIPES_CIRCLE_YELLOW_TARTAR
					{
						// Token: 0x0400E11D RID: 57629
						public static LocString NAME = UI.FormatAsLink("Ick Curves", "EXTERIORWALL");

						// Token: 0x0400E11E RID: 57630
						public static LocString DESC = "This wallpaper, like this colony's journey, is full of twists and turns.";
					}

					// Token: 0x02003916 RID: 14614
					public class STRIPES_PURPLE_BRAINFAT
					{
						// Token: 0x0400E11F RID: 57631
						public static LocString NAME = UI.FormatAsLink("Fainting Stripe", "EXTERIORWALL");

						// Token: 0x0400E120 RID: 57632
						public static LocString DESC = "Vertical stripes make it quite obvious when nearby objects are askew.";
					}

					// Token: 0x02003917 RID: 14615
					public class STRIPES_DIAGONAL_PURPLE_BRAINFAT
					{
						// Token: 0x0400E121 RID: 57633
						public static LocString NAME = UI.FormatAsLink("Fainting Diagonal", "EXTERIORWALL");

						// Token: 0x0400E122 RID: 57634
						public static LocString DESC = "Diagonal stripes in alternating shades of purple.";
					}

					// Token: 0x02003918 RID: 14616
					public class STRIPES_CIRCLE_PURPLE_BRAINFAT
					{
						// Token: 0x0400E123 RID: 57635
						public static LocString NAME = UI.FormatAsLink("Fainting Curves", "EXTERIORWALL");

						// Token: 0x0400E124 RID: 57636
						public static LocString DESC = "This wallpaper, like this colony's journey, is full of twists and turns.";
					}

					// Token: 0x02003919 RID: 14617
					public class FLOPPY_AZULENE_VITRO
					{
						// Token: 0x0400E125 RID: 57637
						public static LocString NAME = UI.FormatAsLink("Waterlogged Databank", "EXTERIORWALL");

						// Token: 0x0400E126 RID: 57638
						public static LocString DESC = "A fun blue print in honor of information storage.";
					}

					// Token: 0x0200391A RID: 14618
					public class FLOPPY_BLACK_WHITE
					{
						// Token: 0x0400E127 RID: 57639
						public static LocString NAME = UI.FormatAsLink("Monochrome Databank", "EXTERIORWALL");

						// Token: 0x0400E128 RID: 57640
						public static LocString DESC = "A chic black-and-white print in honor of information storage.";
					}

					// Token: 0x0200391B RID: 14619
					public class FLOPPY_PEAGREEN_BALMY
					{
						// Token: 0x0400E129 RID: 57641
						public static LocString NAME = UI.FormatAsLink("Lush Databank", "EXTERIORWALL");

						// Token: 0x0400E12A RID: 57642
						public static LocString DESC = "A fun green print in honor of information storage.";
					}

					// Token: 0x0200391C RID: 14620
					public class FLOPPY_SATSUMA_YELLOWCAKE
					{
						// Token: 0x0400E12B RID: 57643
						public static LocString NAME = UI.FormatAsLink("Hi-Vis Databank", "EXTERIORWALL");

						// Token: 0x0400E12C RID: 57644
						public static LocString DESC = "A fun orange print in honor of information storage.";
					}

					// Token: 0x0200391D RID: 14621
					public class FLOPPY_MAGMA_AMINO
					{
						// Token: 0x0400E12D RID: 57645
						public static LocString NAME = UI.FormatAsLink("Flashy Databank", "EXTERIORWALL");

						// Token: 0x0400E12E RID: 57646
						public static LocString DESC = "A fun red print in honor of information storage.";
					}

					// Token: 0x0200391E RID: 14622
					public class ORANGE_JUICE
					{
						// Token: 0x0400E12F RID: 57647
						public static LocString NAME = UI.FormatAsLink("Infinite Spill", "EXTERIORWALL");

						// Token: 0x0400E130 RID: 57648
						public static LocString DESC = "If the liquids never hit the floor, is it really a spill?";
					}

					// Token: 0x0200391F RID: 14623
					public class PAINT_BLOTS
					{
						// Token: 0x0400E131 RID: 57649
						public static LocString NAME = UI.FormatAsLink("Happy Accidents", "EXTERIORWALL");

						// Token: 0x0400E132 RID: 57650
						public static LocString DESC = "There are no mistakes, only cheerful little splotches.";
					}

					// Token: 0x02003920 RID: 14624
					public class TELESCOPE
					{
						// Token: 0x0400E133 RID: 57651
						public static LocString NAME = UI.FormatAsLink("Telescope Print", "EXTERIORWALL");

						// Token: 0x0400E134 RID: 57652
						public static LocString DESC = "The perfect wallpaper for skygazers.";
					}

					// Token: 0x02003921 RID: 14625
					public class TICTACTOE_O
					{
						// Token: 0x0400E135 RID: 57653
						public static LocString NAME = UI.FormatAsLink("TicTacToe O", "EXTERIORWALL");

						// Token: 0x0400E136 RID: 57654
						public static LocString DESC = "A crisp black 'O' on a clean white background. Ideal for monochromatic games rooms.";
					}

					// Token: 0x02003922 RID: 14626
					public class TICTACTOE_X
					{
						// Token: 0x0400E137 RID: 57655
						public static LocString NAME = UI.FormatAsLink("TicTacToe X", "EXTERIORWALL");

						// Token: 0x0400E138 RID: 57656
						public static LocString DESC = "A crisp black 'X' on a clean white background. Ideal for monochromatic games rooms.";
					}

					// Token: 0x02003923 RID: 14627
					public class DICE_1
					{
						// Token: 0x0400E139 RID: 57657
						public static LocString NAME = UI.FormatAsLink("Roll One", "EXTERIORWALL");

						// Token: 0x0400E13A RID: 57658
						public static LocString DESC = "Inspired by classic dice.";
					}

					// Token: 0x02003924 RID: 14628
					public class DICE_2
					{
						// Token: 0x0400E13B RID: 57659
						public static LocString NAME = UI.FormatAsLink("Roll Two", "EXTERIORWALL");

						// Token: 0x0400E13C RID: 57660
						public static LocString DESC = "Inspired by classic dice.";
					}

					// Token: 0x02003925 RID: 14629
					public class DICE_3
					{
						// Token: 0x0400E13D RID: 57661
						public static LocString NAME = UI.FormatAsLink("Roll Three", "EXTERIORWALL");

						// Token: 0x0400E13E RID: 57662
						public static LocString DESC = "Inspired by classic dice.";
					}

					// Token: 0x02003926 RID: 14630
					public class DICE_4
					{
						// Token: 0x0400E13F RID: 57663
						public static LocString NAME = UI.FormatAsLink("Roll Four", "EXTERIORWALL");

						// Token: 0x0400E140 RID: 57664
						public static LocString DESC = "Inspired by classic dice.";
					}

					// Token: 0x02003927 RID: 14631
					public class DICE_5
					{
						// Token: 0x0400E141 RID: 57665
						public static LocString NAME = UI.FormatAsLink("Roll Five", "EXTERIORWALL");

						// Token: 0x0400E142 RID: 57666
						public static LocString DESC = "Inspired by classic dice.";
					}

					// Token: 0x02003928 RID: 14632
					public class DICE_6
					{
						// Token: 0x0400E143 RID: 57667
						public static LocString NAME = UI.FormatAsLink("High Roller", "EXTERIORWALL");

						// Token: 0x0400E144 RID: 57668
						public static LocString DESC = "Inspired by classic dice.";
					}
				}
			}

			// Token: 0x02002753 RID: 10067
			public class FARMTILE
			{
				// Token: 0x0400ADF2 RID: 44530
				public static LocString NAME = UI.FormatAsLink("Farm Tile", "FARMTILE");

				// Token: 0x0400ADF3 RID: 44531
				public static LocString DESC = "Duplicants can deliver fertilizer and liquids to farm tiles, accelerating plant growth.";

				// Token: 0x0400ADF4 RID: 44532
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Grows one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" from a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nCan be used as floor tile and rotated before construction."
				});
			}

			// Token: 0x02002754 RID: 10068
			public class LADDER
			{
				// Token: 0x0400ADF5 RID: 44533
				public static LocString NAME = UI.FormatAsLink("Ladder", "LADDER");

				// Token: 0x0400ADF6 RID: 44534
				public static LocString DESC = "(That means they climb it.)";

				// Token: 0x0400ADF7 RID: 44535
				public static LocString EFFECT = "Enables vertical mobility for Duplicants.";
			}

			// Token: 0x02002755 RID: 10069
			public class LADDERFAST
			{
				// Token: 0x0400ADF8 RID: 44536
				public static LocString NAME = UI.FormatAsLink("Plastic Ladder", "LADDERFAST");

				// Token: 0x0400ADF9 RID: 44537
				public static LocString DESC = "Plastic ladders are mildly antiseptic and can help limit the spread of germs in a colony.";

				// Token: 0x0400ADFA RID: 44538
				public static LocString EFFECT = "Increases Duplicant climbing speed.";
			}

			// Token: 0x02002756 RID: 10070
			public class LIQUIDCONDUIT
			{
				// Token: 0x0400ADFB RID: 44539
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT");

				// Token: 0x0400ADFC RID: 44540
				public static LocString DESC = "Liquid pipes are used to connect the inputs and outputs of plumbed buildings.";

				// Token: 0x0400ADFD RID: 44541
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" between ",
					UI.FormatAsLink("Outputs", "LIQUIDPIPING"),
					" and ",
					UI.FormatAsLink("Intakes", "LIQUIDPIPING"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002757 RID: 10071
			public class LIQUIDCONDUITBRIDGE
			{
				// Token: 0x0400ADFE RID: 44542
				public static LocString NAME = UI.FormatAsLink("Liquid Bridge", "LIQUIDCONDUITBRIDGE");

				// Token: 0x0400ADFF RID: 44543
				public static LocString DESC = "Separate pipe systems help prevent building damage caused by mingled pipe contents.";

				// Token: 0x0400AE00 RID: 44544
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Liquid Pipe", "LIQUIDPIPING") + " section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x02002758 RID: 10072
			public class ICECOOLEDFAN
			{
				// Token: 0x0400AE01 RID: 44545
				public static LocString NAME = UI.FormatAsLink("Ice-E Fan", "ICECOOLEDFAN");

				// Token: 0x0400AE02 RID: 44546
				public static LocString DESC = "A Duplicant can work an Ice-E fan to temporarily cool small areas as needed.";

				// Token: 0x0400AE03 RID: 44547
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Ice", "ICEORE"),
					" to dissipate a small amount of the ",
					UI.FormatAsLink("Heat", "HEAT"),
					"."
				});
			}

			// Token: 0x02002759 RID: 10073
			public class ICEMACHINE
			{
				// Token: 0x0400AE04 RID: 44548
				public static LocString NAME = UI.FormatAsLink("Ice Maker", "ICEMACHINE");

				// Token: 0x0400AE05 RID: 44549
				public static LocString DESC = "Ice makers can be used as a small renewable source of ice and snow.";

				// Token: 0x0400AE06 RID: 44550
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Water", "WATER"),
					" into ",
					UI.FormatAsLink("Ice", "ICE"),
					" or ",
					UI.FormatAsLink("Snow", "SNOW"),
					"."
				});

				// Token: 0x0200355A RID: 13658
				public class OPTION_TOOLTIPS
				{
					// Token: 0x0400D7E0 RID: 55264
					public static LocString ICE = "Convert " + UI.FormatAsLink("Water", "WATER") + " into " + UI.FormatAsLink("Ice", "ICE");

					// Token: 0x0400D7E1 RID: 55265
					public static LocString SNOW = "Convert " + UI.FormatAsLink("Water", "WATER") + " into " + UI.FormatAsLink("Snow", "SNOW");
				}
			}

			// Token: 0x0200275A RID: 10074
			public class LIQUIDCOOLEDFAN
			{
				// Token: 0x0400AE07 RID: 44551
				public static LocString NAME = UI.FormatAsLink("Hydrofan", "LIQUIDCOOLEDFAN");

				// Token: 0x0400AE08 RID: 44552
				public static LocString DESC = "A Duplicant can work a hydrofan to temporarily cool small areas as needed.";

				// Token: 0x0400AE09 RID: 44553
				public static LocString EFFECT = "Dissipates a small amount of the " + UI.FormatAsLink("Heat", "HEAT") + ".";
			}

			// Token: 0x0200275B RID: 10075
			public class CREATURETRAP
			{
				// Token: 0x0400AE0A RID: 44554
				public static LocString NAME = UI.FormatAsLink("Critter Trap", "CREATURETRAP");

				// Token: 0x0400AE0B RID: 44555
				public static LocString DESC = "Critter traps cannot catch swimming or flying critters.";

				// Token: 0x0400AE0C RID: 44556
				public static LocString EFFECT = "Captures a living " + UI.FormatAsLink("Critter", "CREATURES") + " for transport.\n\nSingle use.";
			}

			// Token: 0x0200275C RID: 10076
			public class CREATUREGROUNDTRAP
			{
				// Token: 0x0400AE0D RID: 44557
				public static LocString NAME = UI.FormatAsLink("Critter Trap", "CREATURETRAP");

				// Token: 0x0400AE0E RID: 44558
				public static LocString DESC = "It's designed for land critters, but flopping fish sometimes find their way in too.";

				// Token: 0x0400AE0F RID: 44559
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Captures a living ",
					UI.FormatAsLink("Critter", "CREATURES"),
					" for transport.\n\nOnly Duplicants with the ",
					UI.FormatAsLink("Critter Ranching I", "RANCHING1"),
					" skill can arm this trap. It's reusable!"
				});
			}

			// Token: 0x0200275D RID: 10077
			public class CREATUREDELIVERYPOINT
			{
				// Token: 0x0400AE10 RID: 44560
				public static LocString NAME = UI.FormatAsLink("Critter Drop-Off", "CREATUREDELIVERYPOINT");

				// Token: 0x0400AE11 RID: 44561
				public static LocString DESC = "Duplicants automatically bring captured critters to these relocation points for release.";

				// Token: 0x0400AE12 RID: 44562
				public static LocString EFFECT = "Releases trapped " + UI.FormatAsLink("Critters", "CREATURES") + " back into the world.\n\nCan be used multiple times.";
			}

			// Token: 0x0200275E RID: 10078
			public class CRITTERPICKUP
			{
				// Token: 0x0400AE13 RID: 44563
				public static LocString NAME = UI.FormatAsLink("Critter Pick-Up", "CRITTERPICKUP");

				// Token: 0x0400AE14 RID: 44564
				public static LocString DESC = "Duplicants will automatically wrangle excess critters.";

				// Token: 0x0400AE15 RID: 44565
				public static LocString EFFECT = "Ensures the prompt relocation of " + UI.FormatAsLink("Critters", "CREATURES") + " that exceed the maximum amount set.\n\nMonitoring and pick-up are limited to the specified species.";

				// Token: 0x0200355B RID: 13659
				public class LOGIC_INPUT
				{
					// Token: 0x0400D7E2 RID: 55266
					public static LocString DESC = "Enable/Disable";

					// Token: 0x0400D7E3 RID: 55267
					public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Wrangle excess critters";

					// Token: 0x0400D7E4 RID: 55268
					public static LocString LOGIC_PORT_INACTIVE = BUILDINGS.PREFABS.CRITTERPICKUP.LOGIC_INPUT.LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Ignore excess critters";
				}
			}

			// Token: 0x0200275F RID: 10079
			public class CRITTERDROPOFF
			{
				// Token: 0x0400AE16 RID: 44566
				public static LocString NAME = UI.FormatAsLink("Critter Drop-Off", "CRITTERDROPOFF");

				// Token: 0x0400AE17 RID: 44567
				public static LocString DESC = "Duplicants automatically bring captured critters to these relocation points for release.";

				// Token: 0x0400AE18 RID: 44568
				public static LocString EFFECT = "Releases trapped " + UI.FormatAsLink("Critters", "CREATURES") + " back into the world.\n\nMonitoring and drop-off are limited to the specified species.";

				// Token: 0x0200355C RID: 13660
				public class LOGIC_INPUT
				{
					// Token: 0x0400D7E5 RID: 55269
					public static LocString DESC = "Enable/Disable";

					// Token: 0x0400D7E6 RID: 55270
					public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Enable critter drop-off";

					// Token: 0x0400D7E7 RID: 55271
					public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disable critter drop-off";
				}
			}

			// Token: 0x02002760 RID: 10080
			public class LIQUIDFILTER
			{
				// Token: 0x0400AE19 RID: 44569
				public static LocString NAME = UI.FormatAsLink("Liquid Filter", "LIQUIDFILTER");

				// Token: 0x0400AE1A RID: 44570
				public static LocString DESC = "All liquids are sent into the building's output pipe, except the liquid chosen for filtering.";

				// Token: 0x0400AE1B RID: 44571
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sieves one ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" out of a mix, sending it into a dedicated ",
					UI.FormatAsLink("Filtered Output Pipe", "LIQUIDPIPING"),
					".\n\nCan only filter one liquid type at a time."
				});
			}

			// Token: 0x02002761 RID: 10081
			public class DEVPUMPLIQUID
			{
				// Token: 0x0400AE1C RID: 44572
				public static LocString NAME = "Dev Pump Liquid";

				// Token: 0x0400AE1D RID: 44573
				public static LocString DESC = "Piping a pump's output to a building's intake will send liquid to that building.";

				// Token: 0x0400AE1E RID: 44574
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					".\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			// Token: 0x02002762 RID: 10082
			public class LIQUIDPUMP
			{
				// Token: 0x0400AE1F RID: 44575
				public static LocString NAME = UI.FormatAsLink("Liquid Pump", "LIQUIDPUMP");

				// Token: 0x0400AE20 RID: 44576
				public static LocString DESC = "Piping a pump's output to a building's intake will send liquid to that building.";

				// Token: 0x0400AE21 RID: 44577
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					".\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			// Token: 0x02002763 RID: 10083
			public class LIQUIDMINIPUMP
			{
				// Token: 0x0400AE22 RID: 44578
				public static LocString NAME = UI.FormatAsLink("Mini Liquid Pump", "LIQUIDMINIPUMP");

				// Token: 0x0400AE23 RID: 44579
				public static LocString DESC = "Mini pumps are useful for moving small quantities of liquid with minimum power.";

				// Token: 0x0400AE24 RID: 44580
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in a small amount of ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					".\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			// Token: 0x02002764 RID: 10084
			public class LIQUIDPUMPINGSTATION
			{
				// Token: 0x0400AE25 RID: 44581
				public static LocString NAME = UI.FormatAsLink("Pitcher Pump", "LIQUIDPUMPINGSTATION");

				// Token: 0x0400AE26 RID: 44582
				public static LocString DESC = "Pitcher pumps allow Duplicants to bottle and deliver liquids from place to place.";

				// Token: 0x0400AE27 RID: 44583
				public static LocString EFFECT = "Manually pumps " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " into bottles for transport.\n\nDuplicants can only carry liquids that are bottled.";
			}

			// Token: 0x02002765 RID: 10085
			public class LIQUIDVALVE
			{
				// Token: 0x0400AE28 RID: 44584
				public static LocString NAME = UI.FormatAsLink("Liquid Valve", "LIQUIDVALVE");

				// Token: 0x0400AE29 RID: 44585
				public static LocString DESC = "Valves control the amount of liquid that moves through pipes, preventing waste.";

				// Token: 0x0400AE2A RID: 44586
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Controls the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" volume permitted through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					"."
				});
			}

			// Token: 0x02002766 RID: 10086
			public class LIQUIDLOGICVALVE
			{
				// Token: 0x0400AE2B RID: 44587
				public static LocString NAME = UI.FormatAsLink("Liquid Shutoff", "LIQUIDLOGICVALVE");

				// Token: 0x0400AE2C RID: 44588
				public static LocString DESC = "Automated piping saves power and time by removing the need for Duplicant input.";

				// Token: 0x0400AE2D RID: 44589
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" flow on or off."
				});

				// Token: 0x0400AE2E RID: 44590
				public static LocString LOGIC_PORT = "Open/Close";

				// Token: 0x0400AE2F RID: 44591
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow Liquid flow";

				// Token: 0x0400AE30 RID: 44592
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent Liquid flow";
			}

			// Token: 0x02002767 RID: 10087
			public class LIQUIDLIMITVALVE
			{
				// Token: 0x0400AE31 RID: 44593
				public static LocString NAME = UI.FormatAsLink("Liquid Meter Valve", "LIQUIDLIMITVALVE");

				// Token: 0x0400AE32 RID: 44594
				public static LocString DESC = "Meter Valves let an exact amount of liquid pass through before shutting off.";

				// Token: 0x0400AE33 RID: 44595
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" flow off when the specified amount has passed through it."
				});

				// Token: 0x0400AE34 RID: 44596
				public static LocString LOGIC_PORT_OUTPUT = "Limit Reached";

				// Token: 0x0400AE35 RID: 44597
				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if limit has been reached";

				// Token: 0x0400AE36 RID: 44598
				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400AE37 RID: 44599
				public static LocString LOGIC_PORT_RESET = "Reset Meter";

				// Token: 0x0400AE38 RID: 44600
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the amount";

				// Token: 0x0400AE39 RID: 44601
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x02002768 RID: 10088
			public class LIQUIDVENT
			{
				// Token: 0x0400AE3A RID: 44602
				public static LocString NAME = UI.FormatAsLink("Liquid Vent", "LIQUIDVENT");

				// Token: 0x0400AE3B RID: 44603
				public static LocString DESC = "Vents are an exit point for liquids from plumbing systems.";

				// Token: 0x0400AE3C RID: 44604
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Releases ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" from ",
					UI.FormatAsLink("Liquid Pipes", "LIQUIDPIPING"),
					"."
				});
			}

			// Token: 0x02002769 RID: 10089
			public class MANUALGENERATOR
			{
				// Token: 0x0400AE3D RID: 44605
				public static LocString NAME = UI.FormatAsLink("Manual Generator", "MANUALGENERATOR");

				// Token: 0x0400AE3E RID: 44606
				public static LocString DESC = "Watching Duplicants run on it is adorable... the electrical power is just an added bonus.";

				// Token: 0x0400AE3F RID: 44607
				public static LocString EFFECT = "Converts manual labor into electrical " + UI.FormatAsLink("Power", "POWER") + ".";
			}

			// Token: 0x0200276A RID: 10090
			public class MANUALPRESSUREDOOR
			{
				// Token: 0x0400AE40 RID: 44608
				public static LocString NAME = UI.FormatAsLink("Manual Airlock", "MANUALPRESSUREDOOR");

				// Token: 0x0400AE41 RID: 44609
				public static LocString DESC = "Airlocks can quarter off dangerous areas and prevent gases from seeping into the colony.";

				// Token: 0x0400AE42 RID: 44610
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Blocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow, maintaining pressure between areas.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});
			}

			// Token: 0x0200276B RID: 10091
			public class MESHTILE
			{
				// Token: 0x0400AE43 RID: 44611
				public static LocString NAME = UI.FormatAsLink("Mesh Tile", "MESHTILE");

				// Token: 0x0400AE44 RID: 44612
				public static LocString DESC = "Mesh tile can be used to make Duplicant pathways in areas where liquid flows.";

				// Token: 0x0400AE45 RID: 44613
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nDoes not obstruct ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow."
				});
			}

			// Token: 0x0200276C RID: 10092
			public class PLASTICTILE
			{
				// Token: 0x0400AE46 RID: 44614
				public static LocString NAME = UI.FormatAsLink("Plastic Tile", "PLASTICTILE");

				// Token: 0x0400AE47 RID: 44615
				public static LocString DESC = "Plastic tile is mildly antiseptic and can help limit the spread of germs in a colony.";

				// Token: 0x0400AE48 RID: 44616
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nSignificantly increases Duplicant runspeed.";
			}

			// Token: 0x0200276D RID: 10093
			public class GLASSTILE
			{
				// Token: 0x0400AE49 RID: 44617
				public static LocString NAME = UI.FormatAsLink("Window Tile", "GLASSTILE");

				// Token: 0x0400AE4A RID: 44618
				public static LocString DESC = "Window tiles provide a barrier against liquid and gas and are completely transparent.";

				// Token: 0x0400AE4B RID: 44619
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nAllows ",
					UI.FormatAsLink("Light", "LIGHT"),
					" and ",
					UI.FormatAsLink("Decor", "DECOR"),
					" to pass through."
				});
			}

			// Token: 0x0200276E RID: 10094
			public class METALTILE
			{
				// Token: 0x0400AE4C RID: 44620
				public static LocString NAME = UI.FormatAsLink("Metal Tile", "METALTILE");

				// Token: 0x0400AE4D RID: 44621
				public static LocString DESC = "Heat travels much more quickly through metal tile than other types of flooring.";

				// Token: 0x0400AE4E RID: 44622
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nSignificantly increases Duplicant runspeed.";
			}

			// Token: 0x0200276F RID: 10095
			public class BUNKERTILE
			{
				// Token: 0x0400AE4F RID: 44623
				public static LocString NAME = UI.FormatAsLink("Bunker Tile", "BUNKERTILE");

				// Token: 0x0400AE50 RID: 44624
				public static LocString DESC = "Bunker tile can build strong shelters in otherwise dangerous environments.";

				// Token: 0x0400AE51 RID: 44625
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nCan withstand extreme pressures and impacts.";
			}

			// Token: 0x02002770 RID: 10096
			public class STORAGETILE
			{
				// Token: 0x0400AE52 RID: 44626
				public static LocString NAME = UI.FormatAsLink("Storage Tile", "STORAGETILE");

				// Token: 0x0400AE53 RID: 44627
				public static LocString DESC = "Storage tiles keep selected non-edible solids out of the way.";

				// Token: 0x0400AE54 RID: 44628
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nProvides built-in storage for small spaces.";
			}

			// Token: 0x02002771 RID: 10097
			public class CARPETTILE
			{
				// Token: 0x0400AE55 RID: 44629
				public static LocString NAME = UI.FormatAsLink("Carpeted Tile", "CARPETTILE");

				// Token: 0x0400AE56 RID: 44630
				public static LocString DESC = "Soft on little Duplicant toesies.";

				// Token: 0x0400AE57 RID: 44631
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			// Token: 0x02002772 RID: 10098
			public class MOULDINGTILE
			{
				// Token: 0x0400AE58 RID: 44632
				public static LocString NAME = UI.FormatAsLink("Trimming Tile", "MOUDLINGTILE");

				// Token: 0x0400AE59 RID: 44633
				public static LocString DESC = "Trimming is used as purely decorative lining for walls and structures.";

				// Token: 0x0400AE5A RID: 44634
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			// Token: 0x02002773 RID: 10099
			public class MONUMENTBOTTOM
			{
				// Token: 0x0400AE5B RID: 44635
				public static LocString NAME = UI.FormatAsLink("Monument Base", "MONUMENTBOTTOM");

				// Token: 0x0400AE5C RID: 44636
				public static LocString DESC = "The base of a monument must be constructed first.";

				// Token: 0x0400AE5D RID: 44637
				public static LocString EFFECT = "Builds the bottom section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";
			}

			// Token: 0x02002774 RID: 10100
			public class MONUMENTMIDDLE
			{
				// Token: 0x0400AE5E RID: 44638
				public static LocString NAME = UI.FormatAsLink("Monument Midsection", "MONUMENTMIDDLE");

				// Token: 0x0400AE5F RID: 44639
				public static LocString DESC = "Customized sections of a Great Monument can be mixed and matched.";

				// Token: 0x0400AE60 RID: 44640
				public static LocString EFFECT = "Builds the middle section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";
			}

			// Token: 0x02002775 RID: 10101
			public class MONUMENTTOP
			{
				// Token: 0x0400AE61 RID: 44641
				public static LocString NAME = UI.FormatAsLink("Monument Top", "MONUMENTTOP");

				// Token: 0x0400AE62 RID: 44642
				public static LocString DESC = "Building a Great Monument will declare to the universe that this hunk of rock is your own.";

				// Token: 0x0400AE63 RID: 44643
				public static LocString EFFECT = "Builds the top section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";
			}

			// Token: 0x02002776 RID: 10102
			public class MICROBEMUSHER
			{
				// Token: 0x0400AE64 RID: 44644
				public static LocString NAME = UI.FormatAsLink("Microbe Musher", "MICROBEMUSHER");

				// Token: 0x0400AE65 RID: 44645
				public static LocString DESC = "Musher recipes will keep Duplicants fed, but may impact health and morale over time.";

				// Token: 0x0400AE66 RID: 44646
				public static LocString EFFECT = "Produces low quality " + UI.FormatAsLink("Food", "FOOD") + " using common ingredients.\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x0200355D RID: 13661
				public class FACADES
				{
					// Token: 0x02003929 RID: 14633
					public class DEFAULT_MICROBEMUSHER
					{
						// Token: 0x0400E145 RID: 57669
						public static LocString NAME = UI.FormatAsLink("Microbe Musher", "MICROBEMUSHER");

						// Token: 0x0400E146 RID: 57670
						public static LocString DESC = "Musher recipes will keep Duplicants fed, but may impact health and morale over time.";
					}

					// Token: 0x0200392A RID: 14634
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400E147 RID: 57671
						public static LocString NAME = UI.FormatAsLink("Faint Purple Microbe Musher", "MICROBEMUSHER");

						// Token: 0x0400E148 RID: 57672
						public static LocString DESC = "A colorful distraction from the actual quality of the food.";
					}

					// Token: 0x0200392B RID: 14635
					public class YELLOW_TARTAR
					{
						// Token: 0x0400E149 RID: 57673
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Microbe Musher", "MICROBEMUSHER");

						// Token: 0x0400E14A RID: 57674
						public static LocString DESC = "Makes meals that are memorable for all the wrong reasons.";
					}

					// Token: 0x0200392C RID: 14636
					public class RED_ROSE
					{
						// Token: 0x0400E14B RID: 57675
						public static LocString NAME = UI.FormatAsLink("Puce Pink Microbe Musher", "MICROBEMUSHER");

						// Token: 0x0400E14C RID: 57676
						public static LocString DESC = "Hunger strikes are not an option, but color-coordination is.";
					}

					// Token: 0x0200392D RID: 14637
					public class GREEN_MUSH
					{
						// Token: 0x0400E14D RID: 57677
						public static LocString NAME = UI.FormatAsLink("Mush Green Microbe Musher", "MICROBEMUSHER");

						// Token: 0x0400E14E RID: 57678
						public static LocString DESC = "Edible colloids for dinner <i>again</i>?";
					}

					// Token: 0x0200392E RID: 14638
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400E14F RID: 57679
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Microbe Musher", "MICROBEMUSHER");

						// Token: 0x0400E150 RID: 57680
						public static LocString DESC = "Prioritizes nutritional value over flavor.";
					}
				}
			}

			// Token: 0x02002777 RID: 10103
			public class MINERALDEOXIDIZER
			{
				// Token: 0x0400AE67 RID: 44647
				public static LocString NAME = UI.FormatAsLink("Oxygen Diffuser", "MINERALDEOXIDIZER");

				// Token: 0x0400AE68 RID: 44648
				public static LocString DESC = "Oxygen diffusers are inefficient, but output enough oxygen to keep a colony breathing.";

				// Token: 0x0400AE69 RID: 44649
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts large amounts of ",
					UI.FormatAsLink("Algae", "ALGAE"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			// Token: 0x02002778 RID: 10104
			public class SUBLIMATIONSTATION
			{
				// Token: 0x0400AE6A RID: 44650
				public static LocString NAME = UI.FormatAsLink("Sublimation Station", "SUBLIMATIONSTATION");

				// Token: 0x0400AE6B RID: 44651
				public static LocString DESC = "Sublimation is the sublime process by which solids convert directly into gas.";

				// Token: 0x0400AE6C RID: 44652
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Speeds up the conversion of ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" into ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			// Token: 0x02002779 RID: 10105
			public class WOODTILE
			{
				// Token: 0x0400AE6D RID: 44653
				public static LocString NAME = "Wood Tile";

				// Token: 0x0400AE6E RID: 44654
				public static LocString DESC = "Rooms built with wood tile are cozy and pleasant.";

				// Token: 0x0400AE6F RID: 44655
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nProvides good insulation and boosts ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			// Token: 0x0200277A RID: 10106
			public class SNOWTILE
			{
				// Token: 0x0400AE70 RID: 44656
				public static LocString NAME = "Snow Tile";

				// Token: 0x0400AE71 RID: 44657
				public static LocString DESC = "Snow tiles have low thermal conductivity, but will melt if temperatures get too high.";

				// Token: 0x0400AE72 RID: 44658
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nInsulates rooms to reduce " + UI.FormatAsLink("Heat", "HEAT") + " loss in cold climates.";
			}

			// Token: 0x0200277B RID: 10107
			public class CAMPFIRE
			{
				// Token: 0x0400AE73 RID: 44659
				public static LocString NAME = UI.FormatAsLink("Wood Heater", "CAMPFIRE");

				// Token: 0x0400AE74 RID: 44660
				public static LocString DESC = "Wood heaters dry out soggy feet and help Duplicants forget how cold they are.";

				// Token: 0x0400AE75 RID: 44661
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsLink("Wood", "WOOD"),
					" in order to ",
					UI.FormatAsLink("Heat", "HEAT"),
					" chilly surroundings."
				});
			}

			// Token: 0x0200277C RID: 10108
			public class ICEKETTLE
			{
				// Token: 0x0400AE76 RID: 44662
				public static LocString NAME = UI.FormatAsLink("Ice Liquefier", "ICEKETTLE");

				// Token: 0x0400AE77 RID: 44663
				public static LocString DESC = "The water never gets hot enough to burn the tongue.";

				// Token: 0x0400AE78 RID: 44664
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsLink("Wood", "WOOD"),
					" to melt ",
					UI.FormatAsLink("Ice", "ICE"),
					" into ",
					UI.FormatAsLink("Water", "WATER"),
					", which can be bottled for transport."
				});
			}

			// Token: 0x0200277D RID: 10109
			public class WOODSTORAGE
			{
				// Token: 0x0400AE79 RID: 44665
				public static LocString NAME = "Wood Pile";

				// Token: 0x0400AE7A RID: 44666
				public static LocString DESC = "Once it's empty, there's no use pining for more.";

				// Token: 0x0400AE7B RID: 44667
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores a finite supply of ",
					UI.FormatAsLink("Wood", "WOOD"),
					", which can be used for construction or to produce ",
					UI.FormatAsLink("Heat", "HEAT"),
					"."
				});
			}

			// Token: 0x0200277E RID: 10110
			public class DLC2POITECHUNLOCKS
			{
				// Token: 0x0400AE7C RID: 44668
				public static LocString NAME = "Research Portal";

				// Token: 0x0400AE7D RID: 44669
				public static LocString DESC = "A functional research decrypter with one transmission remaining.\n\nIt was designed to support colony survival.";
			}

			// Token: 0x0200277F RID: 10111
			public class DEEPFRYER
			{
				// Token: 0x0400AE7E RID: 44670
				public static LocString NAME = UI.FormatAsLink("Deep Fryer", "DEEPFRYER");

				// Token: 0x0400AE7F RID: 44671
				public static LocString DESC = "Everything tastes better when it's deep-fried.";

				// Token: 0x0400AE80 RID: 44672
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					" to cook a wide variety of improved ",
					UI.FormatAsLink("Foods", "FOOD"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x0200355E RID: 13662
				public class STATUSITEMS
				{
					// Token: 0x0200392F RID: 14639
					public class OUTSIDE_KITCHEN
					{
						// Token: 0x0400E151 RID: 57681
						public static LocString NAME = "Outside of Kitchen";

						// Token: 0x0400E152 RID: 57682
						public static LocString TOOLTIP = "This building must be in a Kitchen before it can be used";
					}
				}
			}

			// Token: 0x02002780 RID: 10112
			public class ORESCRUBBER
			{
				// Token: 0x0400AE81 RID: 44673
				public static LocString NAME = UI.FormatAsLink("Ore Scrubber", "ORESCRUBBER");

				// Token: 0x0400AE82 RID: 44674
				public static LocString DESC = "Scrubbers sanitize freshly mined materials before they're brought into the colony.";

				// Token: 0x0400AE83 RID: 44675
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Kills a significant amount of ",
					UI.FormatAsLink("Germs", "DISEASE"),
					" present on ",
					UI.FormatAsLink("Raw Ore", "RAWMINERAL"),
					"."
				});
			}

			// Token: 0x02002781 RID: 10113
			public class OUTHOUSE
			{
				// Token: 0x0400AE84 RID: 44676
				public static LocString NAME = UI.FormatAsLink("Outhouse", "OUTHOUSE");

				// Token: 0x0400AE85 RID: 44677
				public static LocString DESC = "The colony that eats together, excretes together.";

				// Token: 0x0400AE86 RID: 44678
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Gives Duplicants a place to relieve themselves.\n\nRequires no ",
					UI.FormatAsLink("Piping", "LIQUIDPIPING"),
					".\n\nMust be periodically emptied of ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					"."
				});
			}

			// Token: 0x02002782 RID: 10114
			public class APOTHECARY
			{
				// Token: 0x0400AE87 RID: 44679
				public static LocString NAME = UI.FormatAsLink("Apothecary", "APOTHECARY");

				// Token: 0x0400AE88 RID: 44680
				public static LocString DESC = "Some medications help prevent diseases, while others aim to alleviate existing illness.";

				// Token: 0x0400AE89 RID: 44681
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Medicine", "MEDICINE"),
					" to cure most basic ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nDuplicants must possess the Medicine Compounding ",
					UI.FormatAsLink("Skill", "ROLES"),
					" to fabricate medicines.\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			// Token: 0x02002783 RID: 10115
			public class ADVANCEDAPOTHECARY
			{
				// Token: 0x0400AE8A RID: 44682
				public static LocString NAME = UI.FormatAsLink("Nuclear Apothecary", "ADVANCEDAPOTHECARY");

				// Token: 0x0400AE8B RID: 44683
				public static LocString DESC = "Some medications help prevent diseases, while others aim to alleviate existing illness.";

				// Token: 0x0400AE8C RID: 44684
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Medicine", "MEDICINE"),
					" to cure most basic ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nDuplicants must possess the Medicine Compounding ",
					UI.FormatAsLink("Skill", "ROLES"),
					" to fabricate medicines.\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			// Token: 0x02002784 RID: 10116
			public class PLANTERBOX
			{
				// Token: 0x0400AE8D RID: 44685
				public static LocString NAME = UI.FormatAsLink("Planter Box", "PLANTERBOX");

				// Token: 0x0400AE8E RID: 44686
				public static LocString DESC = "Domestically grown seeds mature more quickly than wild plants.";

				// Token: 0x0400AE8F RID: 44687
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Grows one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" from a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					"."
				});

				// Token: 0x0200355F RID: 13663
				public class FACADES
				{
					// Token: 0x02003930 RID: 14640
					public class DEFAULT_PLANTERBOX
					{
						// Token: 0x0400E153 RID: 57683
						public static LocString NAME = UI.FormatAsLink("Planter Box", "PLANTERBOX");

						// Token: 0x0400E154 RID: 57684
						public static LocString DESC = "Domestically grown seeds mature more quickly than wild plants.";
					}

					// Token: 0x02003931 RID: 14641
					public class MEALWOOD
					{
						// Token: 0x0400E155 RID: 57685
						public static LocString NAME = UI.FormatAsLink("Mealy Teal Planter Box", "PLANTERBOX");

						// Token: 0x0400E156 RID: 57686
						public static LocString DESC = "Inspired by genetically modified nature.";
					}

					// Token: 0x02003932 RID: 14642
					public class BRISTLEBLOSSOM
					{
						// Token: 0x0400E157 RID: 57687
						public static LocString NAME = UI.FormatAsLink("Bristly Green Planter Box", "PLANTERBOX");

						// Token: 0x0400E158 RID: 57688
						public static LocString DESC = "The interior is lined with tiny barbs.";
					}

					// Token: 0x02003933 RID: 14643
					public class WHEEZEWORT
					{
						// Token: 0x0400E159 RID: 57689
						public static LocString NAME = UI.FormatAsLink("Wheezy Whorl Planter Box", "PLANTERBOX");

						// Token: 0x0400E15A RID: 57690
						public static LocString DESC = "For the dreamy agriculturalist.";
					}

					// Token: 0x02003934 RID: 14644
					public class SLEETWHEAT
					{
						// Token: 0x0400E15B RID: 57691
						public static LocString NAME = UI.FormatAsLink("Sleet Blue Planter Box", "PLANTERBOX");

						// Token: 0x0400E15C RID: 57692
						public static LocString DESC = "The thick paint drips are invisible from a distance.";
					}

					// Token: 0x02003935 RID: 14645
					public class SALMON_PINK
					{
						// Token: 0x0400E15D RID: 57693
						public static LocString NAME = UI.FormatAsLink("Flashy Planter Box", "PLANTERBOX");

						// Token: 0x0400E15E RID: 57694
						public static LocString DESC = "It's not exactly a subtle color.";
					}
				}
			}

			// Token: 0x02002785 RID: 10117
			public class PRESSUREDOOR
			{
				// Token: 0x0400AE90 RID: 44688
				public static LocString NAME = UI.FormatAsLink("Mechanized Airlock", "PRESSUREDOOR");

				// Token: 0x0400AE91 RID: 44689
				public static LocString DESC = "Mechanized airlocks open and close more quickly than other types of door.";

				// Token: 0x0400AE92 RID: 44690
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Blocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow, maintaining pressure between areas.\n\nFunctions as a ",
					UI.FormatAsLink("Manual Airlock", "MANUALPRESSUREDOOR"),
					" when no ",
					UI.FormatAsLink("Power", "POWER"),
					" is available.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});
			}

			// Token: 0x02002786 RID: 10118
			public class BUNKERDOOR
			{
				// Token: 0x0400AE93 RID: 44691
				public static LocString NAME = UI.FormatAsLink("Bunker Door", "BUNKERDOOR");

				// Token: 0x0400AE94 RID: 44692
				public static LocString DESC = "A massive, slow-moving door which is nearly indestructible.";

				// Token: 0x0400AE95 RID: 44693
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Blocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow, maintaining pressure between areas.\n\nCan withstand extremely high pressures and impacts."
				});
			}

			// Token: 0x02002787 RID: 10119
			public class RATIONBOX
			{
				// Token: 0x0400AE96 RID: 44694
				public static LocString NAME = UI.FormatAsLink("Ration Box", "RATIONBOX");

				// Token: 0x0400AE97 RID: 44695
				public static LocString DESC = "Ration boxes keep food safe from hungry critters, but don't slow food spoilage.";

				// Token: 0x0400AE98 RID: 44696
				public static LocString EFFECT = "Stores a small amount of " + UI.FormatAsLink("Food", "FOOD") + ".\n\nFood must be delivered to boxes by Duplicants.";
			}

			// Token: 0x02002788 RID: 10120
			public class PARKSIGN
			{
				// Token: 0x0400AE99 RID: 44697
				public static LocString NAME = UI.FormatAsLink("Park Sign", "PARKSIGN");

				// Token: 0x0400AE9A RID: 44698
				public static LocString DESC = "Passing through parks will increase Duplicant Morale.";

				// Token: 0x0400AE9B RID: 44699
				public static LocString EFFECT = "Classifies an area as a Park or Nature Reserve.";
			}

			// Token: 0x02002789 RID: 10121
			public class RADIATIONLIGHT
			{
				// Token: 0x0400AE9C RID: 44700
				public static LocString NAME = UI.FormatAsLink("Radiation Lamp", "RADIATIONLIGHT");

				// Token: 0x0400AE9D RID: 44701
				public static LocString DESC = "Duplicants can become sick if exposed to radiation without protection.";

				// Token: 0x0400AE9E RID: 44702
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Emits ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" when ",
					UI.FormatAsLink("Powered", "POWER"),
					" that can be collected by a ",
					UI.FormatAsLink("Radbolt Generator", "HIGHENERGYPARTICLESPAWNER"),
					"."
				});
			}

			// Token: 0x0200278A RID: 10122
			public class REFRIGERATOR
			{
				// Token: 0x0400AE9F RID: 44703
				public static LocString NAME = UI.FormatAsLink("Refrigerator", "REFRIGERATOR");

				// Token: 0x0400AEA0 RID: 44704
				public static LocString DESC = "Food spoilage can be slowed by ambient conditions as well as by refrigerators.";

				// Token: 0x0400AEA1 RID: 44705
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Food", "FOOD"),
					" at an ideal ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" to prevent spoilage."
				});

				// Token: 0x0400AEA2 RID: 44706
				public static LocString LOGIC_PORT = "Full/Not Full";

				// Token: 0x0400AEA3 RID: 44707
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when full";

				// Token: 0x0400AEA4 RID: 44708
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x02003560 RID: 13664
				public class FACADES
				{
					// Token: 0x02003936 RID: 14646
					public class DEFAULT_REFRIGERATOR
					{
						// Token: 0x0400E15F RID: 57695
						public static LocString NAME = UI.FormatAsLink("Refrigerator", "REFRIGERATOR");

						// Token: 0x0400E160 RID: 57696
						public static LocString DESC = "Food spoilage can be slowed by ambient conditions as well as by refrigerators.";
					}

					// Token: 0x02003937 RID: 14647
					public class STRIPES_RED_WHITE
					{
						// Token: 0x0400E161 RID: 57697
						public static LocString NAME = UI.FormatAsLink("Bold Stripe Refrigerator", "REFRIGERATOR");

						// Token: 0x0400E162 RID: 57698
						public static LocString DESC = "Bold on the outside, cold on the inside!";
					}

					// Token: 0x02003938 RID: 14648
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400E163 RID: 57699
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Refrigerator", "REFRIGERATOR");

						// Token: 0x0400E164 RID: 57700
						public static LocString DESC = "For food so cold, it brings a tear to the eye.";
					}

					// Token: 0x02003939 RID: 14649
					public class GREEN_MUSH
					{
						// Token: 0x0400E165 RID: 57701
						public static LocString NAME = UI.FormatAsLink("Mush Green Refrigerator", "REFRIGERATOR");

						// Token: 0x0400E166 RID: 57702
						public static LocString DESC = "Honestly, this hue is particularly chilling.";
					}

					// Token: 0x0200393A RID: 14650
					public class RED_ROSE
					{
						// Token: 0x0400E167 RID: 57703
						public static LocString NAME = UI.FormatAsLink("Puce Pink Refrigerator", "REFRIGERATOR");

						// Token: 0x0400E168 RID: 57704
						public static LocString DESC = "Inspired by the Duplicant poem, \"Pretty in Puce.\"";
					}

					// Token: 0x0200393B RID: 14651
					public class YELLOW_TARTAR
					{
						// Token: 0x0400E169 RID: 57705
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Refrigerator", "REFRIGERATOR");

						// Token: 0x0400E16A RID: 57706
						public static LocString DESC = "Some Duplicants call it \"sunny\" yellow, but only because they've never seen the sun.";
					}

					// Token: 0x0200393C RID: 14652
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400E16B RID: 57707
						public static LocString NAME = UI.FormatAsLink("Faint Purple Refrigerator", "REFRIGERATOR");

						// Token: 0x0400E16C RID: 57708
						public static LocString DESC = "This fridge makes color-coordination a (cold) snap.";
					}
				}
			}

			// Token: 0x0200278B RID: 10123
			public class ROLESTATION
			{
				// Token: 0x0400AEA5 RID: 44709
				public static LocString NAME = UI.FormatAsLink("Skills Board", "ROLESTATION");

				// Token: 0x0400AEA6 RID: 44710
				public static LocString DESC = "A skills board can teach special skills to Duplicants they can't learn on their own.";

				// Token: 0x0400AEA7 RID: 44711
				public static LocString EFFECT = "Allows Duplicants to spend Skill Points to learn new " + UI.FormatAsLink("Skills", "JOBS") + ".";
			}

			// Token: 0x0200278C RID: 10124
			public class RESETSKILLSSTATION
			{
				// Token: 0x0400AEA8 RID: 44712
				public static LocString NAME = UI.FormatAsLink("Skill Scrubber", "RESETSKILLSSTATION");

				// Token: 0x0400AEA9 RID: 44713
				public static LocString DESC = "Erase skills from a Duplicant's mind, returning them to their default abilities.";

				// Token: 0x0400AEAA RID: 44714
				public static LocString EFFECT = "Refunds a Duplicant's Skill Points for reassignment.\n\nDuplicants will lose all assigned skills in the process.";
			}

			// Token: 0x0200278D RID: 10125
			public class RESEARCHCENTER
			{
				// Token: 0x0400AEAB RID: 44715
				public static LocString NAME = UI.FormatAsLink("Research Station", "RESEARCHCENTER");

				// Token: 0x0400AEAC RID: 44716
				public static LocString DESC = "Research stations are necessary for unlocking all research tiers.";

				// Token: 0x0400AEAD RID: 44717
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Novice Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Dirt", "DIRT"),
					"."
				});
			}

			// Token: 0x0200278E RID: 10126
			public class ADVANCEDRESEARCHCENTER
			{
				// Token: 0x0400AEAE RID: 44718
				public static LocString NAME = UI.FormatAsLink("Super Computer", "ADVANCEDRESEARCHCENTER");

				// Token: 0x0400AEAF RID: 44719
				public static LocString DESC = "Super computers unlock higher technology tiers than research stations alone.";

				// Token: 0x0400AEB0 RID: 44720
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Advanced Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Water", "WATER"),
					".\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Advanced Research", "RESEARCHING1"),
					" skill."
				});
			}

			// Token: 0x0200278F RID: 10127
			public class NUCLEARRESEARCHCENTER
			{
				// Token: 0x0400AEB1 RID: 44721
				public static LocString NAME = UI.FormatAsLink("Materials Study Terminal", "NUCLEARRESEARCHCENTER");

				// Token: 0x0400AEB2 RID: 44722
				public static LocString DESC = "Comes with a few ions thrown in, free of charge.";

				// Token: 0x0400AEB3 RID: 44723
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Materials Science Research", "RESEARCHDLC1"),
					" to unlock new technologies.\n\nConsumes Radbolts.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Applied Sciences Research", "ATOMICRESEARCH"),
					" skill."
				});
			}

			// Token: 0x02002790 RID: 10128
			public class ORBITALRESEARCHCENTER
			{
				// Token: 0x0400AEB4 RID: 44724
				public static LocString NAME = UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER");

				// Token: 0x0400AEB5 RID: 44725
				public static LocString DESC = "Orbital Data Collection Labs record data while orbiting a Planetoid and write it to a " + UI.FormatAsLink("Data Bank", "ORBITALRESEARCHDATABANK") + ". ";

				// Token: 0x0400AEB6 RID: 44726
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Creates ",
					UI.FormatAsLink("Data Banks", "ORBITALRESEARCHDATABANK"),
					" that can be consumed at a ",
					UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
					" and ",
					UI.FormatAsLink("Power", "POWER"),
					"."
				});
			}

			// Token: 0x02002791 RID: 10129
			public class COSMICRESEARCHCENTER
			{
				// Token: 0x0400AEB7 RID: 44727
				public static LocString NAME = UI.FormatAsLink("Virtual Planetarium", "COSMICRESEARCHCENTER");

				// Token: 0x0400AEB8 RID: 44728
				public static LocString DESC = "Planetariums allow the simulated exploration of locations discovered with a telescope.";

				// Token: 0x0400AEB9 RID: 44729
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Interstellar Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes data from ",
					UI.FormatAsLink("Research Modules", "RESEARCHMODULE"),
					".\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Astronomy", "ASTRONOMY"),
					" skill."
				});
			}

			// Token: 0x02002792 RID: 10130
			public class DLC1COSMICRESEARCHCENTER
			{
				// Token: 0x0400AEBA RID: 44730
				public static LocString NAME = UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER");

				// Token: 0x0400AEBB RID: 44731
				public static LocString DESC = "Planetariums allow the simulated exploration of locations recorded in " + UI.FormatAsLink("Data Banks", "ORBITALRESEARCHDATABANK") + ".";

				// Token: 0x0400AEBC RID: 44732
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Data Analysis Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Data Banks", "ORBITALRESEARCHDATABANK"),
					" generated by exploration."
				});
			}

			// Token: 0x02002793 RID: 10131
			public class TELESCOPE
			{
				// Token: 0x0400AEBD RID: 44733
				public static LocString NAME = UI.FormatAsLink("Telescope", "TELESCOPE");

				// Token: 0x0400AEBE RID: 44734
				public static LocString DESC = "Telescopes are necessary for learning starmaps and conducting rocket missions.";

				// Token: 0x0400AEBF RID: 44735
				public static LocString EFFECT = "Maps Starmap destinations.\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Field Research", "RESEARCHING2") + " skill.\n\nBuilding must be exposed to space to function.";

				// Token: 0x0400AEC0 RID: 44736
				public static LocString REQUIREMENT_TOOLTIP = "A steady {0} supply is required to sustain working Duplicants.";
			}

			// Token: 0x02002794 RID: 10132
			public class CLUSTERTELESCOPE
			{
				// Token: 0x0400AEC1 RID: 44737
				public static LocString NAME = UI.FormatAsLink("Telescope", "CLUSTERTELESCOPE");

				// Token: 0x0400AEC2 RID: 44738
				public static LocString DESC = "Telescopes are necessary for studying space, allowing rocket travel to other worlds.";

				// Token: 0x0400AEC3 RID: 44739
				public static LocString EFFECT = "Reveals visitable Planetoids in space.\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Astronomy", "ASTRONOMY") + " skill.\n\nBuilding must be exposed to space to function.";

				// Token: 0x0400AEC4 RID: 44740
				public static LocString REQUIREMENT_TOOLTIP = "A steady {0} supply is required to sustain working Duplicants.";
			}

			// Token: 0x02002795 RID: 10133
			public class CLUSTERTELESCOPEENCLOSED
			{
				// Token: 0x0400AEC5 RID: 44741
				public static LocString NAME = UI.FormatAsLink("Enclosed Telescope", "CLUSTERTELESCOPEENCLOSED");

				// Token: 0x0400AEC6 RID: 44742
				public static LocString DESC = "Telescopes are necessary for studying space, allowing rocket travel to other worlds.";

				// Token: 0x0400AEC7 RID: 44743
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reveals visitable Planetoids in space... in comfort!\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Astronomy", "ASTRONOMY"),
					" skill.\n\nExcellent sunburn protection (100%), partial ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" protection (",
					GameUtil.GetFormattedPercent(FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING * 100f, GameUtil.TimeSlice.None),
					") .\n\nBuilding must be exposed to space to function."
				});

				// Token: 0x0400AEC8 RID: 44744
				public static LocString REQUIREMENT_TOOLTIP = "A steady {0} supply is required to sustain working Duplicants.";
			}

			// Token: 0x02002796 RID: 10134
			public class MISSIONCONTROL
			{
				// Token: 0x0400AEC9 RID: 44745
				public static LocString NAME = UI.FormatAsLink("Mission Control Station", "MISSIONCONTROL");

				// Token: 0x0400AECA RID: 44746
				public static LocString DESC = "Like a backseat driver who actually does know better.";

				// Token: 0x0400AECB RID: 44747
				public static LocString EFFECT = "Provides guidance data to rocket pilots, to improve rocket speed.\n\nMust be operated by a Duplicant with the " + UI.FormatAsLink("Astronomy", "ASTRONOMY") + " skill.\n\nRequires a clear line of sight to space in order to function.";
			}

			// Token: 0x02002797 RID: 10135
			public class MISSIONCONTROLCLUSTER
			{
				// Token: 0x0400AECC RID: 44748
				public static LocString NAME = UI.FormatAsLink("Mission Control Station", "MISSIONCONTROLCLUSTER");

				// Token: 0x0400AECD RID: 44749
				public static LocString DESC = "Like a backseat driver who actually does know better.";

				// Token: 0x0400AECE RID: 44750
				public static LocString EFFECT = "Provides guidance data to rocket pilots within range, to improve rocket speed.\n\nMust be operated by a Duplicant with the " + UI.FormatAsLink("Astronomy", "ASTRONOMY") + " skill.\n\nRequires a clear line of sight to space in order to function.";
			}

			// Token: 0x02002798 RID: 10136
			public class SCULPTURE
			{
				// Token: 0x0400AECF RID: 44751
				public static LocString NAME = UI.FormatAsLink("Large Sculpting Block", "SCULPTURE");

				// Token: 0x0400AED0 RID: 44752
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x0400AED1 RID: 44753
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x0400AED2 RID: 44754
				public static LocString POORQUALITYNAME = "\"Abstract\" Sculpture";

				// Token: 0x0400AED3 RID: 44755
				public static LocString AVERAGEQUALITYNAME = "Mediocre Sculpture";

				// Token: 0x0400AED4 RID: 44756
				public static LocString EXCELLENTQUALITYNAME = "Genius Sculpture";

				// Token: 0x02003561 RID: 13665
				public class FACADES
				{
					// Token: 0x0200393D RID: 14653
					public class SCULPTURE_GOOD_1
					{
						// Token: 0x0400E16D RID: 57709
						public static LocString NAME = UI.FormatAsLink("O Cupid, My Cupid", "SCULPTURE_GOOD_1");

						// Token: 0x0400E16E RID: 57710
						public static LocString DESC = "Ode to the bow and arrow, love's equivalent to a mining gun...but for hearts.";
					}

					// Token: 0x0200393E RID: 14654
					public class SCULPTURE_CRAP_1
					{
						// Token: 0x0400E16F RID: 57711
						public static LocString NAME = UI.FormatAsLink("Inexplicable", "SCULPTURE_CRAP_1");

						// Token: 0x0400E170 RID: 57712
						public static LocString DESC = "A valiant attempt at art.";
					}

					// Token: 0x0200393F RID: 14655
					public class SCULPTURE_AMAZING_2
					{
						// Token: 0x0400E171 RID: 57713
						public static LocString NAME = UI.FormatAsLink("Plate Chucker", "SCULPTURE_AMAZING_2");

						// Token: 0x0400E172 RID: 57714
						public static LocString DESC = "A masterful portrayal of an athlete who's been banned from the communal kitchen.";
					}

					// Token: 0x02003940 RID: 14656
					public class SCULPTURE_AMAZING_3
					{
						// Token: 0x0400E173 RID: 57715
						public static LocString NAME = UI.FormatAsLink("Before Battle", "SCULPTURE_AMAZING_3");

						// Token: 0x0400E174 RID: 57716
						public static LocString DESC = "A masterful portrayal of a slingshot-wielding hero.";
					}

					// Token: 0x02003941 RID: 14657
					public class SCULPTURE_AMAZING_4
					{
						// Token: 0x0400E175 RID: 57717
						public static LocString NAME = UI.FormatAsLink("Grandiose Grub-Grub", "SCULPTURE_AMAZING_4");

						// Token: 0x0400E176 RID: 57718
						public static LocString DESC = "A masterful portrayal of a gentle, plant-tending critter.";
					}

					// Token: 0x02003942 RID: 14658
					public class SCULPTURE_AMAZING_1
					{
						// Token: 0x0400E177 RID: 57719
						public static LocString NAME = UI.FormatAsLink("The Hypothesizer", "SCULPTURE_AMAZING_1");

						// Token: 0x0400E178 RID: 57720
						public static LocString DESC = "A masterful portrayal of a scientist lost in thought.";
					}

					// Token: 0x02003943 RID: 14659
					public class SCULPTURE_AMAZING_5
					{
						// Token: 0x0400E179 RID: 57721
						public static LocString NAME = UI.FormatAsLink("Vertical Cosmos", "SCULPTURE_AMAZING_5");

						// Token: 0x0400E17A RID: 57722
						public static LocString DESC = "It contains multitudes.";
					}

					// Token: 0x02003944 RID: 14660
					public class SCULPTURE_AMAZING_6
					{
						// Token: 0x0400E17B RID: 57723
						public static LocString NAME = UI.FormatAsLink("Into the Voids", "SCULPTURE_AMAZING_6");

						// Token: 0x0400E17C RID: 57724
						public static LocString DESC = "No amount of material success will ever fill the void within.";
					}
				}
			}

			// Token: 0x02002799 RID: 10137
			public class ICESCULPTURE
			{
				// Token: 0x0400AED5 RID: 44757
				public static LocString NAME = UI.FormatAsLink("Ice Block", "ICESCULPTURE");

				// Token: 0x0400AED6 RID: 44758
				public static LocString DESC = "Prone to melting.";

				// Token: 0x0400AED7 RID: 44759
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x0400AED8 RID: 44760
				public static LocString POORQUALITYNAME = "\"Abstract\" Ice Sculpture";

				// Token: 0x0400AED9 RID: 44761
				public static LocString AVERAGEQUALITYNAME = "Mediocre Ice Sculpture";

				// Token: 0x0400AEDA RID: 44762
				public static LocString EXCELLENTQUALITYNAME = "Genius Ice Sculpture";

				// Token: 0x02003562 RID: 13666
				public class FACADES
				{
					// Token: 0x02003945 RID: 14661
					public class ICESCULPTURE_CRAP
					{
						// Token: 0x0400E17D RID: 57725
						public static LocString NAME = UI.FormatAsLink("Cubi I", "ICESCULPTURE_CRAP");

						// Token: 0x0400E17E RID: 57726
						public static LocString DESC = "It's structurally unsound, but otherwise not entirely terrible.";
					}

					// Token: 0x02003946 RID: 14662
					public class ICESCULPTURE_AMAZING_1
					{
						// Token: 0x0400E17F RID: 57727
						public static LocString NAME = UI.FormatAsLink("Exquisite Chompers", "ICESCULPTURE_AMAZING_1");

						// Token: 0x0400E180 RID: 57728
						public static LocString DESC = "These incisors are the stuff of dental legend.";
					}

					// Token: 0x02003947 RID: 14663
					public class ICESCULPTURE_AMAZING_2
					{
						// Token: 0x0400E181 RID: 57729
						public static LocString NAME = UI.FormatAsLink("Frosty Crustacean", "ICESCULPTURE_AMAZING_2");

						// Token: 0x0400E182 RID: 57730
						public static LocString DESC = "A charming depiction of the mighty Pokeshell in mid-rampage.";
					}

					// Token: 0x02003948 RID: 14664
					public class ICESCULPTURE_AMAZING_3
					{
						// Token: 0x0400E183 RID: 57731
						public static LocString NAME = UI.FormatAsLink("The Chase", "ICESCULPTURE_AMAZING_3");

						// Token: 0x0400E184 RID: 57732
						public static LocString DESC = "Some aquarists posit that Pacus are the original creators of the game now known as \"Tag.\"";
					}
				}
			}

			// Token: 0x0200279A RID: 10138
			public class MARBLESCULPTURE
			{
				// Token: 0x0400AEDB RID: 44763
				public static LocString NAME = UI.FormatAsLink("Marble Block", "MARBLESCULPTURE");

				// Token: 0x0400AEDC RID: 44764
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x0400AEDD RID: 44765
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x0400AEDE RID: 44766
				public static LocString POORQUALITYNAME = "\"Abstract\" Marble Sculpture";

				// Token: 0x0400AEDF RID: 44767
				public static LocString AVERAGEQUALITYNAME = "Mediocre Marble Sculpture";

				// Token: 0x0400AEE0 RID: 44768
				public static LocString EXCELLENTQUALITYNAME = "Genius Marble Sculpture";

				// Token: 0x02003563 RID: 13667
				public class FACADES
				{
					// Token: 0x02003949 RID: 14665
					public class SCULPTURE_MARBLE_CRAP_1
					{
						// Token: 0x0400E185 RID: 57733
						public static LocString NAME = UI.FormatAsLink("Lumpy Fungus", "SCULPTURE_MARBLE_CRAP_1");

						// Token: 0x0400E186 RID: 57734
						public static LocString DESC = "The artist was a very fungi.";
					}

					// Token: 0x0200394A RID: 14666
					public class SCULPTURE_MARBLE_GOOD_1
					{
						// Token: 0x0400E187 RID: 57735
						public static LocString NAME = UI.FormatAsLink("Unicorn Bust", "SCULPTURE_MARBLE_GOOD_1");

						// Token: 0x0400E188 RID: 57736
						public static LocString DESC = "It has real \"mane\" character energy.";
					}

					// Token: 0x0200394B RID: 14667
					public class SCULPTURE_MARBLE_AMAZING_1
					{
						// Token: 0x0400E189 RID: 57737
						public static LocString NAME = UI.FormatAsLink("The Large-ish Mermaid", "SCULPTURE_MARBLE_AMAZING_1");

						// Token: 0x0400E18A RID: 57738
						public static LocString DESC = "She's not afraid to take up space.";
					}

					// Token: 0x0200394C RID: 14668
					public class SCULPTURE_MARBLE_AMAZING_2
					{
						// Token: 0x0400E18B RID: 57739
						public static LocString NAME = UI.FormatAsLink("Grouchy Beast", "SCULPTURE_MARBLE_AMAZING_2");

						// Token: 0x0400E18C RID: 57740
						public static LocString DESC = "The artist took great pleasure in conveying their displeasure.";
					}

					// Token: 0x0200394D RID: 14669
					public class SCULPTURE_MARBLE_AMAZING_3
					{
						// Token: 0x0400E18D RID: 57741
						public static LocString NAME = UI.FormatAsLink("The Guardian", "SCULPTURE_MARBLE_AMAZING_3");

						// Token: 0x0400E18E RID: 57742
						public static LocString DESC = "Will not play fetch.";
					}

					// Token: 0x0200394E RID: 14670
					public class SCULPTURE_MARBLE_AMAZING_4
					{
						// Token: 0x0400E18F RID: 57743
						public static LocString NAME = UI.FormatAsLink("Truly A-Moo-Zing", "SCULPTURE_MARBLE_AMAZING_4");

						// Token: 0x0400E190 RID: 57744
						public static LocString DESC = "A masterful celebration of one of the universe's most mysterious - and flatulent - organisms.";
					}

					// Token: 0x0200394F RID: 14671
					public class SCULPTURE_MARBLE_AMAZING_5
					{
						// Token: 0x0400E191 RID: 57745
						public static LocString NAME = UI.FormatAsLink("Green Goddess", "SCULPTURE_MARBLE_AMAZING_5");

						// Token: 0x0400E192 RID: 57746
						public static LocString DESC = "A masterful celebration of the deep bond between a horticulturalist and her prize Bristle Blossom.";
					}
				}
			}

			// Token: 0x0200279B RID: 10139
			public class METALSCULPTURE
			{
				// Token: 0x0400AEE1 RID: 44769
				public static LocString NAME = UI.FormatAsLink("Metal Block", "METALSCULPTURE");

				// Token: 0x0400AEE2 RID: 44770
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x0400AEE3 RID: 44771
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x0400AEE4 RID: 44772
				public static LocString POORQUALITYNAME = "\"Abstract\" Metal Sculpture";

				// Token: 0x0400AEE5 RID: 44773
				public static LocString AVERAGEQUALITYNAME = "Mediocre Metal Sculpture";

				// Token: 0x0400AEE6 RID: 44774
				public static LocString EXCELLENTQUALITYNAME = "Genius Metal Sculpture";

				// Token: 0x02003564 RID: 13668
				public class FACADES
				{
					// Token: 0x02003950 RID: 14672
					public class SCULPTURE_METAL_CRAP_1
					{
						// Token: 0x0400E193 RID: 57747
						public static LocString NAME = UI.FormatAsLink("Unnatural Beauty", "SCULPTURE_METAL_CRAP_1");

						// Token: 0x0400E194 RID: 57748
						public static LocString DESC = "Actually, it's a very good likeness.";
					}

					// Token: 0x02003951 RID: 14673
					public class SCULPTURE_METAL_GOOD_1
					{
						// Token: 0x0400E195 RID: 57749
						public static LocString NAME = UI.FormatAsLink("Beautiful Biohazard", "SCULPTURE_METAL_GOOD_1");

						// Token: 0x0400E196 RID: 57750
						public static LocString DESC = "The Morb's eye is mounted on a swivel that activates at random intervals.";
					}

					// Token: 0x02003952 RID: 14674
					public class SCULPTURE_METAL_AMAZING_1
					{
						// Token: 0x0400E197 RID: 57751
						public static LocString NAME = UI.FormatAsLink("Insatiable Appetite", "SCULPTURE_METAL_AMAZING_1");

						// Token: 0x0400E198 RID: 57752
						public static LocString DESC = "It's quite lovely, until someone stubs their toe on it in the dark.";
					}

					// Token: 0x02003953 RID: 14675
					public class SCULPTURE_METAL_AMAZING_2
					{
						// Token: 0x0400E199 RID: 57753
						public static LocString NAME = UI.FormatAsLink("Agape", "SCULPTURE_METAL_AMAZING_2");

						// Token: 0x0400E19A RID: 57754
						public static LocString DESC = "Not quite expressionist, but undeniably expressive.";
					}

					// Token: 0x02003954 RID: 14676
					public class SCULPTURE_METAL_AMAZING_3
					{
						// Token: 0x0400E19B RID: 57755
						public static LocString NAME = UI.FormatAsLink("Friendly Flier", "SCULPTURE_METAL_AMAZING_3");

						// Token: 0x0400E19C RID: 57756
						public static LocString DESC = "It emits no light, but it sure does brighten up a room.";
					}

					// Token: 0x02003955 RID: 14677
					public class SCULPTURE_METAL_AMAZING_4
					{
						// Token: 0x0400E19D RID: 57757
						public static LocString NAME = UI.FormatAsLink("Whatta Pip", "SCULPTURE_METAL_AMAZING_4");

						// Token: 0x0400E19E RID: 57758
						public static LocString DESC = "A masterful likeness of the mischievous critter that Duplicants love to love.";
					}

					// Token: 0x02003956 RID: 14678
					public class SCULPTURE_METAL_AMAZING_5
					{
						// Token: 0x0400E19F RID: 57759
						public static LocString NAME = UI.FormatAsLink("Phrenologist's Dream", "SCULPTURE_METAL_AMAZING_5");

						// Token: 0x0400E1A0 RID: 57760
						public static LocString DESC = "What if the entire head is one big bump?";
					}
				}
			}

			// Token: 0x0200279C RID: 10140
			public class SMALLSCULPTURE
			{
				// Token: 0x0400AEE7 RID: 44775
				public static LocString NAME = UI.FormatAsLink("Sculpting Block", "SMALLSCULPTURE");

				// Token: 0x0400AEE8 RID: 44776
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x0400AEE9 RID: 44777
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Minorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x0400AEEA RID: 44778
				public static LocString POORQUALITYNAME = "\"Abstract\" Sculpture";

				// Token: 0x0400AEEB RID: 44779
				public static LocString AVERAGEQUALITYNAME = "Mediocre Sculpture";

				// Token: 0x0400AEEC RID: 44780
				public static LocString EXCELLENTQUALITYNAME = "Genius Sculpture";

				// Token: 0x02003565 RID: 13669
				public class FACADES
				{
					// Token: 0x02003957 RID: 14679
					public class SCULPTURE_1x2_GOOD
					{
						// Token: 0x0400E1A1 RID: 57761
						public static LocString NAME = UI.FormatAsLink("Lunar Slice", "SCULPTURE_1x2_GOOD");

						// Token: 0x0400E1A2 RID: 57762
						public static LocString DESC = "It must be a moon, because there are no bananas in space.";
					}

					// Token: 0x02003958 RID: 14680
					public class SCULPTURE_1x2_CRAP
					{
						// Token: 0x0400E1A3 RID: 57763
						public static LocString NAME = UI.FormatAsLink("Unrequited", "SCULPTURE_1x2_CRAP");

						// Token: 0x0400E1A4 RID: 57764
						public static LocString DESC = "It's a heavy heart.";
					}

					// Token: 0x02003959 RID: 14681
					public class SCULPTURE_1x2_AMAZING_1
					{
						// Token: 0x0400E1A5 RID: 57765
						public static LocString NAME = UI.FormatAsLink("Not a Funnel", "SCULPTURE_1x2_AMAZING_1");

						// Token: 0x0400E1A6 RID: 57766
						public static LocString DESC = "<i>Ceci n'est pas un entonnoir.</i>";
					}

					// Token: 0x0200395A RID: 14682
					public class SCULPTURE_1x2_AMAZING_2
					{
						// Token: 0x0400E1A7 RID: 57767
						public static LocString NAME = UI.FormatAsLink("Equilibrium", "SCULPTURE_1x2_AMAZING_2");

						// Token: 0x0400E1A8 RID: 57768
						public static LocString DESC = "Part of a well-balanced exhibit.";
					}

					// Token: 0x0200395B RID: 14683
					public class SCULPTURE_1x2_AMAZING_3
					{
						// Token: 0x0400E1A9 RID: 57769
						public static LocString NAME = UI.FormatAsLink("Opaque Orb", "SCULPTURE_1x2_AMAZING_3");

						// Token: 0x0400E1AA RID: 57770
						public static LocString DESC = "It lacks transparency.";
					}

					// Token: 0x0200395C RID: 14684
					public class SCULPTURE_1x2_AMAZING_4
					{
						// Token: 0x0400E1AB RID: 57771
						public static LocString NAME = UI.FormatAsLink("Employee of the Month", "SCULPTURE_1x2_AMAZING_4");

						// Token: 0x0400E1AC RID: 57772
						public static LocString DESC = "A masterful celebration of the Sweepy's unbeatable work ethic and cheerful, can-clean attitude.";
					}

					// Token: 0x0200395D RID: 14685
					public class SCULPTURE_1x2_AMAZING_5
					{
						// Token: 0x0400E1AD RID: 57773
						public static LocString NAME = UI.FormatAsLink("Pointy Impossibility", "SCULPTURE_1x2_AMAZING_5");

						// Token: 0x0400E1AE RID: 57774
						public static LocString DESC = "A three-dimensional rebellion against the rules of Euclidean space.";
					}

					// Token: 0x0200395E RID: 14686
					public class SCULPTURE_1x2_AMAZING_6
					{
						// Token: 0x0400E1AF RID: 57775
						public static LocString NAME = UI.FormatAsLink("Fireball", "SCULPTURE_1x2_AMAZING_6");

						// Token: 0x0400E1B0 RID: 57776
						public static LocString DESC = "Tribute to the artist's friend, who once attempted to catch a meteor with their bare hands.";
					}
				}
			}

			// Token: 0x0200279D RID: 10141
			public class WOODSCULPTURE
			{
				// Token: 0x0400AEED RID: 44781
				public static LocString NAME = UI.FormatAsLink("Wood Block", "WOODSCULPTURE");

				// Token: 0x0400AEEE RID: 44782
				public static LocString DESC = "A great fit for smaller spaces.";

				// Token: 0x0400AEEF RID: 44783
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x0400AEF0 RID: 44784
				public static LocString POORQUALITYNAME = "\"Abstract\" Wood Sculpture";

				// Token: 0x0400AEF1 RID: 44785
				public static LocString AVERAGEQUALITYNAME = "Mediocre Wood Sculpture";

				// Token: 0x0400AEF2 RID: 44786
				public static LocString EXCELLENTQUALITYNAME = "Genius Wood Sculpture";
			}

			// Token: 0x0200279E RID: 10142
			public class SHEARINGSTATION
			{
				// Token: 0x0400AEF3 RID: 44787
				public static LocString NAME = UI.FormatAsLink("Shearing Station", "SHEARINGSTATION");

				// Token: 0x0400AEF4 RID: 44788
				public static LocString DESC = "Those critters aren't gonna shear themselves.";

				// Token: 0x0400AEF5 RID: 44789
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Shearing stations allow eligible ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" to be safely sheared for useful raw materials.\n\nVisiting this building restores ",
					UI.FormatAsLink("Critters'", "CREATURES"),
					" physical and emotional well-being."
				});
			}

			// Token: 0x0200279F RID: 10143
			public class OXYGENMASKSTATION
			{
				// Token: 0x0400AEF6 RID: 44790
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask Station", "OXYGENMASKSTATION");

				// Token: 0x0400AEF7 RID: 44791
				public static LocString DESC = "Duplicants can't pass by a station if it lacks enough oxygen to fill a mask.";

				// Token: 0x0400AEF8 RID: 44792
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses designated ",
					UI.FormatAsLink("Metal Ores", "METAL"),
					" from filter settings to create ",
					UI.FormatAsLink("Oxygen Masks", "OXYGENMASK"),
					".\n\nAutomatically draws in ambient ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" to fill masks.\n\nMarks a threshold where Duplicants must put on or take off a mask.\n\nCan be rotated before construction."
				});
			}

			// Token: 0x020027A0 RID: 10144
			public class SWEEPBOTSTATION
			{
				// Token: 0x0400AEF9 RID: 44793
				public static LocString NAME = UI.FormatAsLink("Sweepy's Dock", "SWEEPBOTSTATION");

				// Token: 0x0400AEFA RID: 44794
				public static LocString NAMEDSTATION = "{0}'s Dock";

				// Token: 0x0400AEFB RID: 44795
				public static LocString DESC = "The cute little face comes pre-installed.";

				// Token: 0x0400AEFC RID: 44796
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Deploys an automated ",
					UI.FormatAsLink("Sweepy Bot", "SWEEPBOT"),
					" to sweep up ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					" debris and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" spills.\n\nDock stores ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Solids", "ELEMENTS_SOLID"),
					" gathered by the Sweepy.\n\nUses ",
					UI.FormatAsLink("Power", "POWER"),
					" to recharge the Sweepy.\n\nDuplicants will empty Dock storage into available storage bins."
				});
			}

			// Token: 0x020027A1 RID: 10145
			public class OXYGENMASKMARKER
			{
				// Token: 0x0400AEFD RID: 44797
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask Checkpoint", "OXYGENMASKMARKER");

				// Token: 0x0400AEFE RID: 44798
				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				// Token: 0x0400AEFF RID: 44799
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must put on or take off an ",
					UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK"),
					".\n\nMust be built next to an ",
					UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER"),
					".\n\nCan be rotated before construction."
				});
			}

			// Token: 0x020027A2 RID: 10146
			public class OXYGENMASKLOCKER
			{
				// Token: 0x0400AF00 RID: 44800
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER");

				// Token: 0x0400AF01 RID: 44801
				public static LocString DESC = "An oxygen mask dock will store and refill masks while they're not in use.";

				// Token: 0x0400AF02 RID: 44802
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Oxygen Masks", "OXYGEN_MASK"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nBuild next to an ",
					UI.FormatAsLink("Oxygen Mask Checkpoint", "OXYGENMASKMARKER"),
					" to make Duplicants put on masks when passing by."
				});
			}

			// Token: 0x020027A3 RID: 10147
			public class SUITMARKER
			{
				// Token: 0x0400AF03 RID: 44803
				public static LocString NAME = UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER");

				// Token: 0x0400AF04 RID: 44804
				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				// Token: 0x0400AF05 RID: 44805
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must change into or out of ",
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					".\n\nMust be built next to an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					".\n\nCan be rotated before construction."
				});
			}

			// Token: 0x020027A4 RID: 10148
			public class SUITLOCKER
			{
				// Token: 0x0400AF06 RID: 44806
				public static LocString NAME = UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER");

				// Token: 0x0400AF07 RID: 44807
				public static LocString DESC = "An atmo suit dock will empty atmo suits of waste, but only one suit can charge at a time.";

				// Token: 0x0400AF08 RID: 44808
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nEmpties suits of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					".\n\nBuild next to an ",
					UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER"),
					" to make Duplicants change into suits when passing by."
				});
			}

			// Token: 0x020027A5 RID: 10149
			public class JETSUITMARKER
			{
				// Token: 0x0400AF09 RID: 44809
				public static LocString NAME = UI.FormatAsLink("Jet Suit Checkpoint", "JETSUITMARKER");

				// Token: 0x0400AF0A RID: 44810
				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				// Token: 0x0400AF0B RID: 44811
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must change into or out of ",
					UI.FormatAsLink("Jet Suits", "JET_SUIT"),
					".\n\nMust be built next to a ",
					UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER"),
					".\n\nCan be rotated before construction."
				});
			}

			// Token: 0x020027A6 RID: 10150
			public class JETSUITLOCKER
			{
				// Token: 0x0400AF0C RID: 44812
				public static LocString NAME = UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER");

				// Token: 0x0400AF0D RID: 44813
				public static LocString DESC = "Jet suit docks can refill jet suits with air and fuel, or empty them of waste.";

				// Token: 0x0400AF0E RID: 44814
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Jet Suits", "JET_SUIT"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					".\n\nEmpties suits of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					".\n\nBuild next to a ",
					UI.FormatAsLink("Jet Suit Checkpoint", "JETSUITMARKER"),
					" to make Duplicants change into suits when passing by."
				});
			}

			// Token: 0x020027A7 RID: 10151
			public class LEADSUITMARKER
			{
				// Token: 0x0400AF0F RID: 44815
				public static LocString NAME = UI.FormatAsLink("Lead Suit Checkpoint", "LEADSUITMARKER");

				// Token: 0x0400AF10 RID: 44816
				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				// Token: 0x0400AF11 RID: 44817
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must change into or out of ",
					UI.FormatAsLink("Lead Suits", "LEAD_SUIT"),
					".\n\nMust be built next to a ",
					UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER"),
					"\n\nCan be rotated before construction."
				});
			}

			// Token: 0x020027A8 RID: 10152
			public class LEADSUITLOCKER
			{
				// Token: 0x0400AF12 RID: 44818
				public static LocString NAME = UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER");

				// Token: 0x0400AF13 RID: 44819
				public static LocString DESC = "Lead suit docks can refill lead suits with air and empty them of waste.";

				// Token: 0x0400AF14 RID: 44820
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Lead Suits", "LEAD_SUIT"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nEmpties suits of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					".\n\nBuild next to a ",
					UI.FormatAsLink("Lead Suit Checkpoint", "LEADSUITMARKER"),
					" to make Duplicants change into suits when passing by."
				});
			}

			// Token: 0x020027A9 RID: 10153
			public class CRAFTINGTABLE
			{
				// Token: 0x0400AF15 RID: 44821
				public static LocString NAME = UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE");

				// Token: 0x0400AF16 RID: 44822
				public static LocString DESC = "Crafting stations allow Duplicants to make oxygen masks to wear in low breathability areas.";

				// Token: 0x0400AF17 RID: 44823
				public static LocString EFFECT = "Produces items and equipment for Duplicant use.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x020027AA RID: 10154
			public class ADVANCEDCRAFTINGTABLE
			{
				// Token: 0x0400AF18 RID: 44824
				public static LocString NAME = UI.FormatAsLink("Soldering Station", "ADVANCEDCRAFTINGTABLE");

				// Token: 0x0400AF19 RID: 44825
				public static LocString DESC = "Soldering stations allow Duplicants to build helpful Flydo retriever bots.";

				// Token: 0x0400AF1A RID: 44826
				public static LocString EFFECT = "Produces advanced electronics and bionic " + UI.FormatAsLink("Boosters", "BIONIC_UPGRADE") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x0400AF1B RID: 44827
				public static LocString BIONIC_COMPONENT_RECIPE_DESC = "Converts {0} to {1}";
			}

			// Token: 0x020027AB RID: 10155
			public class DATAMINER
			{
				// Token: 0x0400AF1C RID: 44828
				public static LocString NAME = UI.FormatAsLink("Data Miner", "DATAMINER");

				// Token: 0x0400AF1D RID: 44829
				public static LocString DESC = "Data banks can also be used to program robo-pilots and bionic boosters.";

				// Token: 0x0400AF1E RID: 44830
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Mass-produces ",
					UI.FormatAsLink("Data Banks", "ORBITAL_RESESARCH_DATABANK"),
					" that can be processed into ",
					UI.FormatAsLink("Data Analysis Research", "RESEARCH"),
					" points.\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x0400AF1F RID: 44831
				public static LocString RECIPE_DESCRIPTION = "Turns {0} into {1}.";
			}

			// Token: 0x020027AC RID: 10156
			public class REMOTEWORKTERMINAL
			{
				// Token: 0x0400AF20 RID: 44832
				public static LocString NAME = UI.FormatAsLink("Remote Controller", "REMOTEWORKTERMINAL");

				// Token: 0x0400AF21 RID: 44833
				public static LocString DESC = "Remote controllers cut down on colony commute times.";

				// Token: 0x0400AF22 RID: 44834
				public static LocString EFFECT = "Enables Duplicants to operate machinery remotely via a connected " + UI.FormatAsLink("Remote Worker Dock", "REMOTEWORKERDOCK") + ".";
			}

			// Token: 0x020027AD RID: 10157
			public class REMOTEWORKERDOCK
			{
				// Token: 0x0400AF23 RID: 44835
				public static LocString NAME = UI.FormatAsLink("Remote Worker Dock", "REMOTEWORKERDOCK");

				// Token: 0x0400AF24 RID: 44836
				public static LocString DESC = "It's a Duplicant's duplicate.";

				// Token: 0x0400AF25 RID: 44837
				public static LocString EFFECT = "Carries out machine operation instructions received from a connected " + UI.FormatAsLink("Remote Controller", "REMOTEWORKTERMINAL") + ".\n\nMust be placed within range of its target building.";
			}

			// Token: 0x020027AE RID: 10158
			public class SUITFABRICATOR
			{
				// Token: 0x0400AF26 RID: 44838
				public static LocString NAME = UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR");

				// Token: 0x0400AF27 RID: 44839
				public static LocString DESC = "Exosuits can be filled with oxygen to allow Duplicants to safely enter hazardous areas.";

				// Token: 0x0400AF28 RID: 44840
				public static LocString EFFECT = "Forges protective " + UI.FormatAsLink("Exosuits", "EXOSUIT") + " for Duplicants to wear.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x020027AF RID: 10159
			public class CLOTHINGALTERATIONSTATION
			{
				// Token: 0x0400AF29 RID: 44841
				public static LocString NAME = UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION");

				// Token: 0x0400AF2A RID: 44842
				public static LocString DESC = "Allows skilled Duplicants to add extra personal pizzazz to their wardrobe.";

				// Token: 0x0400AF2B RID: 44843
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Upgrades ",
					UI.FormatAsLink("Snazzy Suits", "FUNKY_VEST"),
					" into ",
					UI.FormatAsLink("Primo Garb", "CUSTOM_CLOTHING"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			// Token: 0x020027B0 RID: 10160
			public class CLOTHINGFABRICATOR
			{
				// Token: 0x0400AF2C RID: 44844
				public static LocString NAME = UI.FormatAsLink("Textile Loom", "CLOTHINGFABRICATOR");

				// Token: 0x0400AF2D RID: 44845
				public static LocString DESC = "A textile loom can be used to spin Reed Fiber into wearable Duplicant clothing.";

				// Token: 0x0400AF2E RID: 44846
				public static LocString EFFECT = "Tailors Duplicant " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " items.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x020027B1 RID: 10161
			public class SOLIDBOOSTER
			{
				// Token: 0x0400AF2F RID: 44847
				public static LocString NAME = UI.FormatAsLink("Solid Fuel Thruster", "SOLIDBOOSTER");

				// Token: 0x0400AF30 RID: 44848
				public static LocString DESC = "Additional thrusters allow rockets to reach far away space destinations.";

				// Token: 0x0400AF31 RID: 44849
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Iron", "IRON"),
					" and ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" to increase rocket exploration distance."
				});
			}

			// Token: 0x020027B2 RID: 10162
			public class SPACEHEATER
			{
				// Token: 0x0400AF32 RID: 44850
				public static LocString NAME = UI.FormatAsLink("Space Heater", "SPACEHEATER");

				// Token: 0x0400AF33 RID: 44851
				public static LocString DESC = "Space heaters are a welcome cure for cold, soggy feet.";

				// Token: 0x0400AF34 RID: 44852
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Radiates a moderate amount of ",
					UI.FormatAsLink("Heat", "HEAT"),
					".\n\nRequires ",
					UI.FormatAsLink("Power", "POWER"),
					" in order to function."
				});
			}

			// Token: 0x020027B3 RID: 10163
			public class SPICEGRINDER
			{
				// Token: 0x0400AF35 RID: 44853
				public static LocString NAME = UI.FormatAsLink("Spice Grinder", "SPICEGRINDER");

				// Token: 0x0400AF36 RID: 44854
				public static LocString DESC = "Crushed seeds and other edibles make excellent meal-enhancing additives.";

				// Token: 0x0400AF37 RID: 44855
				public static LocString EFFECT = "Produces ingredients that add benefits to " + UI.FormatAsLink("foods", "FOOD") + " prepared at skilled cooking stations.";

				// Token: 0x0400AF38 RID: 44856
				public static LocString INGREDIENTHEADER = "Ingredients per 1000kcal:";
			}

			// Token: 0x020027B4 RID: 10164
			public class STORAGELOCKER
			{
				// Token: 0x0400AF39 RID: 44857
				public static LocString NAME = UI.FormatAsLink("Storage Bin", "STORAGELOCKER");

				// Token: 0x0400AF3A RID: 44858
				public static LocString DESC = "Resources left on the floor become \"debris\" and lower decor when not put away.";

				// Token: 0x0400AF3B RID: 44859
				public static LocString EFFECT = "Stores the " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " of your choosing.";

				// Token: 0x02003566 RID: 13670
				public class FACADES
				{
					// Token: 0x0200395F RID: 14687
					public class DEFAULT_STORAGELOCKER
					{
						// Token: 0x0400E1B1 RID: 57777
						public static LocString NAME = UI.FormatAsLink("Storage Bin", "STORAGELOCKER");

						// Token: 0x0400E1B2 RID: 57778
						public static LocString DESC = "Resources left on the floor become \"debris\" and lower decor when not put away.";
					}

					// Token: 0x02003960 RID: 14688
					public class GREEN_MUSH
					{
						// Token: 0x0400E1B3 RID: 57779
						public static LocString NAME = UI.FormatAsLink("Mush Green Storage Bin", "STORAGELOCKER");

						// Token: 0x0400E1B4 RID: 57780
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x02003961 RID: 14689
					public class RED_ROSE
					{
						// Token: 0x0400E1B5 RID: 57781
						public static LocString NAME = UI.FormatAsLink("Puce Pink Storage Bin", "STORAGELOCKER");

						// Token: 0x0400E1B6 RID: 57782
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x02003962 RID: 14690
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400E1B7 RID: 57783
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Storage Bin", "STORAGELOCKER");

						// Token: 0x0400E1B8 RID: 57784
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x02003963 RID: 14691
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400E1B9 RID: 57785
						public static LocString NAME = UI.FormatAsLink("Faint Purple Storage Bin", "STORAGELOCKER");

						// Token: 0x0400E1BA RID: 57786
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x02003964 RID: 14692
					public class YELLOW_TARTAR
					{
						// Token: 0x0400E1BB RID: 57787
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Storage Bin", "STORAGELOCKER");

						// Token: 0x0400E1BC RID: 57788
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x02003965 RID: 14693
					public class POLKA_DARKNAVYNOOKGREEN
					{
						// Token: 0x0400E1BD RID: 57789
						public static LocString NAME = UI.FormatAsLink("Party Dot Storage Bin", "STORAGELOCKER");

						// Token: 0x0400E1BE RID: 57790
						public static LocString DESC = "A fun storage solution for fun-damental materials.";
					}

					// Token: 0x02003966 RID: 14694
					public class POLKA_DARKPURPLERESIN
					{
						// Token: 0x0400E1BF RID: 57791
						public static LocString NAME = UI.FormatAsLink("Mod Dot Storage Bin", "STORAGELOCKER");

						// Token: 0x0400E1C0 RID: 57792
						public static LocString DESC = "Groovy storage, because messy colonies are such a drag.";
					}

					// Token: 0x02003967 RID: 14695
					public class STRIPES_RED_WHITE
					{
						// Token: 0x0400E1C1 RID: 57793
						public static LocString NAME = "Bold Stripe Storage Bin";

						// Token: 0x0400E1C2 RID: 57794
						public static LocString DESC = "It's the merriest storage bin of all.";
					}
				}
			}

			// Token: 0x020027B5 RID: 10165
			public class STORAGELOCKERSMART
			{
				// Token: 0x0400AF3C RID: 44860
				public static LocString NAME = UI.FormatAsLink("Smart Storage Bin", "STORAGELOCKERSMART");

				// Token: 0x0400AF3D RID: 44861
				public static LocString DESC = "Smart storage bins can automate resource organization based on type and mass.";

				// Token: 0x0400AF3E RID: 44862
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores the ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" of your choosing.\n\nSends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when bin is full."
				});

				// Token: 0x0400AF3F RID: 44863
				public static LocString LOGIC_PORT = "Full/Not Full";

				// Token: 0x0400AF40 RID: 44864
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when full";

				// Token: 0x0400AF41 RID: 44865
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020027B6 RID: 10166
			public class OBJECTDISPENSER
			{
				// Token: 0x0400AF42 RID: 44866
				public static LocString NAME = UI.FormatAsLink("Automatic Dispenser", "OBJECTDISPENSER");

				// Token: 0x0400AF43 RID: 44867
				public static LocString DESC = "Automatic dispensers will store and drop resources in small quantities.";

				// Token: 0x0400AF44 RID: 44868
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores any ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" delivered to it by Duplicants.\n\nDumps stored materials back into the world when it receives a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"."
				});

				// Token: 0x0400AF45 RID: 44869
				public static LocString LOGIC_PORT = "Dump Trigger";

				// Token: 0x0400AF46 RID: 44870
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Dump all stored materials";

				// Token: 0x0400AF47 RID: 44871
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Store materials";
			}

			// Token: 0x020027B7 RID: 10167
			public class LIQUIDRESERVOIR
			{
				// Token: 0x0400AF48 RID: 44872
				public static LocString NAME = UI.FormatAsLink("Liquid Reservoir", "LIQUIDRESERVOIR");

				// Token: 0x0400AF49 RID: 44873
				public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";

				// Token: 0x0400AF4A RID: 44874
				public static LocString EFFECT = "Stores any " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources piped into it.";
			}

			// Token: 0x020027B8 RID: 10168
			public class GASRESERVOIR
			{
				// Token: 0x0400AF4B RID: 44875
				public static LocString NAME = UI.FormatAsLink("Gas Reservoir", "GASRESERVOIR");

				// Token: 0x0400AF4C RID: 44876
				public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";

				// Token: 0x0400AF4D RID: 44877
				public static LocString EFFECT = "Stores any " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources piped into it.";

				// Token: 0x02003567 RID: 13671
				public class FACADES
				{
					// Token: 0x02003968 RID: 14696
					public class DEFAULT_GASRESERVOIR
					{
						// Token: 0x0400E1C3 RID: 57795
						public static LocString NAME = UI.FormatAsLink("Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400E1C4 RID: 57796
						public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";
					}

					// Token: 0x02003969 RID: 14697
					public class LIGHTGOLD
					{
						// Token: 0x0400E1C5 RID: 57797
						public static LocString NAME = UI.FormatAsLink("Golden Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400E1C6 RID: 57798
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x0200396A RID: 14698
					public class PEAGREEN
					{
						// Token: 0x0400E1C7 RID: 57799
						public static LocString NAME = UI.FormatAsLink("Greenpea Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400E1C8 RID: 57800
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x0200396B RID: 14699
					public class LIGHTCOBALT
					{
						// Token: 0x0400E1C9 RID: 57801
						public static LocString NAME = UI.FormatAsLink("Bluemoon Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400E1CA RID: 57802
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x0200396C RID: 14700
					public class POLKA_DARKPURPLERESIN
					{
						// Token: 0x0400E1CB RID: 57803
						public static LocString NAME = UI.FormatAsLink("Mod Dot Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400E1CC RID: 57804
						public static LocString DESC = "It sports the cheeriest of paint jobs. What a gas!";
					}

					// Token: 0x0200396D RID: 14701
					public class POLKA_DARKNAVYNOOKGREEN
					{
						// Token: 0x0400E1CD RID: 57805
						public static LocString NAME = UI.FormatAsLink("Party Dot Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400E1CE RID: 57806
						public static LocString DESC = "Safe gas storage doesn't have to be dull.";
					}

					// Token: 0x0200396E RID: 14702
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400E1CF RID: 57807
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400E1D0 RID: 57808
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x0200396F RID: 14703
					public class YELLOW_TARTAR
					{
						// Token: 0x0400E1D1 RID: 57809
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400E1D2 RID: 57810
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x02003970 RID: 14704
					public class GREEN_MUSH
					{
						// Token: 0x0400E1D3 RID: 57811
						public static LocString NAME = UI.FormatAsLink("Mush Green Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400E1D4 RID: 57812
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x02003971 RID: 14705
					public class RED_ROSE
					{
						// Token: 0x0400E1D5 RID: 57813
						public static LocString NAME = UI.FormatAsLink("Puce Pink Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400E1D6 RID: 57814
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x02003972 RID: 14706
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400E1D7 RID: 57815
						public static LocString NAME = UI.FormatAsLink("Faint Purple Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400E1D8 RID: 57816
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}
				}
			}

			// Token: 0x020027B9 RID: 10169
			public class SMARTRESERVOIR
			{
				// Token: 0x0400AF4E RID: 44878
				public static LocString LOGIC_PORT = "Refill Parameters";

				// Token: 0x0400AF4F RID: 44879
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when reservoir is less than <b>Low Threshold</b> full, until <b>High Threshold</b> is reached again";

				// Token: 0x0400AF50 RID: 44880
				public static LocString LOGIC_PORT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when reservoir is <b>High Threshold</b> full, until <b>Low Threshold</b> is reached again";

				// Token: 0x0400AF51 RID: 44881
				public static LocString ACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when reservoir is less than <b>{0}%</b> full, until it is <b>{1}% (High Threshold)</b> full";

				// Token: 0x0400AF52 RID: 44882
				public static LocString DEACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when reservoir is <b>{0}%</b> full, until it is less than <b>{1}% (Low Threshold)</b> full";

				// Token: 0x0400AF53 RID: 44883
				public static LocString SIDESCREEN_TITLE = "Logic Activation Parameters";

				// Token: 0x0400AF54 RID: 44884
				public static LocString SIDESCREEN_ACTIVATE = "Low Threshold:";

				// Token: 0x0400AF55 RID: 44885
				public static LocString SIDESCREEN_DEACTIVATE = "High Threshold:";
			}

			// Token: 0x020027BA RID: 10170
			public class LIQUIDHEATER
			{
				// Token: 0x0400AF56 RID: 44886
				public static LocString NAME = UI.FormatAsLink("Liquid Tepidizer", "LIQUIDHEATER");

				// Token: 0x0400AF57 RID: 44887
				public static LocString DESC = "Tepidizers heat liquid which can kill waterborne germs.";

				// Token: 0x0400AF58 RID: 44888
				public static LocString EFFECT = "Warms large bodies of " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".\n\nMust be fully submerged.";
			}

			// Token: 0x020027BB RID: 10171
			public class SWITCH
			{
				// Token: 0x0400AF59 RID: 44889
				public static LocString NAME = UI.FormatAsLink("Switch", "SWITCH");

				// Token: 0x0400AF5A RID: 44890
				public static LocString DESC = "Switches can only affect buildings that come after them on a circuit.";

				// Token: 0x0400AF5B RID: 44891
				public static LocString EFFECT = "Turns " + UI.FormatAsLink("Power", "POWER") + " on or off.\n\nDoes not affect circuitry preceding the switch.";

				// Token: 0x0400AF5C RID: 44892
				public static LocString SIDESCREEN_TITLE = "Switch";

				// Token: 0x0400AF5D RID: 44893
				public static LocString TURN_ON = "Turn On";

				// Token: 0x0400AF5E RID: 44894
				public static LocString TURN_ON_TOOLTIP = "Turn On {Hotkey}";

				// Token: 0x0400AF5F RID: 44895
				public static LocString TURN_OFF = "Turn Off";

				// Token: 0x0400AF60 RID: 44896
				public static LocString TURN_OFF_TOOLTIP = "Turn Off {Hotkey}";
			}

			// Token: 0x020027BC RID: 10172
			public class LOGICPOWERRELAY
			{
				// Token: 0x0400AF61 RID: 44897
				public static LocString NAME = UI.FormatAsLink("Power Shutoff", "LOGICPOWERRELAY");

				// Token: 0x0400AF62 RID: 44898
				public static LocString DESC = "Automated systems save power and time by removing the need for Duplicant input.";

				// Token: 0x0400AF63 RID: 44899
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off.\n\nDoes not affect circuitry preceding the switch."
				});

				// Token: 0x0400AF64 RID: 44900
				public static LocString LOGIC_PORT = "Kill Power";

				// Token: 0x0400AF65 RID: 44901
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					": Allow ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" through connected circuits"
				});

				// Token: 0x0400AF66 RID: 44902
				public static LocString LOGIC_PORT_INACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					": Prevent ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from flowing through connected circuits"
				});
			}

			// Token: 0x020027BD RID: 10173
			public class LOGICINTERASTEROIDSENDER
			{
				// Token: 0x0400AF67 RID: 44903
				public static LocString NAME = UI.FormatAsLink("Automation Broadcaster", "LOGICINTERASTEROIDSENDER");

				// Token: 0x0400AF68 RID: 44904
				public static LocString DESC = "Sends automation signals into space.";

				// Token: 0x0400AF69 RID: 44905
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to an ",
					UI.FormatAsLink("Automation Receiver", "LOGICINTERASTEROIDRECEIVER"),
					" over vast distances in space.\n\nBoth the Automation Broadcaster and the Automation Receiver must be exposed to space to function."
				});

				// Token: 0x0400AF6A RID: 44906
				public static LocString DEFAULTNAME = "Unnamed Broadcaster";

				// Token: 0x0400AF6B RID: 44907
				public static LocString LOGIC_PORT = "Broadcasting Signal";

				// Token: 0x0400AF6C RID: 44908
				public static LocString LOGIC_PORT_ACTIVE = "Broadcasting: " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400AF6D RID: 44909
				public static LocString LOGIC_PORT_INACTIVE = "Broadcasting: " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020027BE RID: 10174
			public class LOGICINTERASTEROIDRECEIVER
			{
				// Token: 0x0400AF6E RID: 44910
				public static LocString NAME = UI.FormatAsLink("Automation Receiver", "LOGICINTERASTEROIDRECEIVER");

				// Token: 0x0400AF6F RID: 44911
				public static LocString DESC = "Receives automation signals from space.";

				// Token: 0x0400AF70 RID: 44912
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Receives a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" from an ",
					UI.FormatAsLink("Automation Broadcaster", "LOGICINTERASTEROIDSENDER"),
					" over vast distances in space.\n\nBoth the Automation Receiver and the Automation Broadcaster must be exposed to space to function."
				});

				// Token: 0x0400AF71 RID: 44913
				public static LocString LOGIC_PORT = "Receiving Signal";

				// Token: 0x0400AF72 RID: 44914
				public static LocString LOGIC_PORT_ACTIVE = "Receiving: " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400AF73 RID: 44915
				public static LocString LOGIC_PORT_INACTIVE = "Receiving: " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020027BF RID: 10175
			public class TEMPERATURECONTROLLEDSWITCH
			{
				// Token: 0x0400AF74 RID: 44916
				public static LocString NAME = UI.FormatAsLink("Thermo Switch", "TEMPERATURECONTROLLEDSWITCH");

				// Token: 0x0400AF75 RID: 44917
				public static LocString DESC = "Automated switches can be used to manage circuits in areas where Duplicants cannot enter.";

				// Token: 0x0400AF76 RID: 44918
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically turns ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off using ambient ",
					UI.FormatAsLink("Temperature", "HEAT"),
					".\n\nDoes not affect circuitry preceding the switch."
				});
			}

			// Token: 0x020027C0 RID: 10176
			public class PRESSURESWITCHLIQUID
			{
				// Token: 0x0400AF77 RID: 44919
				public static LocString NAME = UI.FormatAsLink("Hydro Switch", "PRESSURESWITCHLIQUID");

				// Token: 0x0400AF78 RID: 44920
				public static LocString DESC = "A hydro switch shuts off power when the liquid pressure surrounding it surpasses the set threshold.";

				// Token: 0x0400AF79 RID: 44921
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically turns ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off using ambient ",
					UI.FormatAsLink("Liquid Pressure", "PRESSURE"),
					".\n\nDoes not affect circuitry preceding the switch.\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			// Token: 0x020027C1 RID: 10177
			public class PRESSURESWITCHGAS
			{
				// Token: 0x0400AF7A RID: 44922
				public static LocString NAME = UI.FormatAsLink("Atmo Switch", "PRESSURESWITCHGAS");

				// Token: 0x0400AF7B RID: 44923
				public static LocString DESC = "An atmo switch shuts off power when the air pressure surrounding it surpasses the set threshold.";

				// Token: 0x0400AF7C RID: 44924
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically turns ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off using ambient ",
					UI.FormatAsLink("Gas Pressure", "PRESSURE"),
					" .\n\nDoes not affect circuitry preceding the switch."
				});
			}

			// Token: 0x020027C2 RID: 10178
			public class TILE
			{
				// Token: 0x0400AF7D RID: 44925
				public static LocString NAME = UI.FormatAsLink("Tile", "TILE");

				// Token: 0x0400AF7E RID: 44926
				public static LocString DESC = "Tile can be used to bridge gaps and get to unreachable areas.";

				// Token: 0x0400AF7F RID: 44927
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nIncreases Duplicant runspeed.";
			}

			// Token: 0x020027C3 RID: 10179
			public class WALLTOILET
			{
				// Token: 0x0400AF80 RID: 44928
				public static LocString NAME = UI.FormatAsLink("Wall Toilet", "WALLTOILET");

				// Token: 0x0400AF81 RID: 44929
				public static LocString DESC = "Wall Toilets transmit fewer germs to Duplicants and require no emptying.";

				// Token: 0x0400AF82 RID: 44930
				public static LocString EFFECT = "Gives Duplicants a place to relieve themselves. Empties directly on the other side of the wall.\n\nSpreads very few " + UI.FormatAsLink("Germs", "DISEASE") + ".";
			}

			// Token: 0x020027C4 RID: 10180
			public class WATERPURIFIER
			{
				// Token: 0x0400AF83 RID: 44931
				public static LocString NAME = UI.FormatAsLink("Water Sieve", "WATERPURIFIER");

				// Token: 0x0400AF84 RID: 44932
				public static LocString DESC = "Sieves cannot kill germs and pass any they receive into their waste and water output.";

				// Token: 0x0400AF85 RID: 44933
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces clean ",
					UI.FormatAsLink("Water", "WATER"),
					" from ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" using ",
					UI.FormatAsLink("Sand", "SAND"),
					".\n\nProduces ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					"."
				});
			}

			// Token: 0x020027C5 RID: 10181
			public class DISTILLATIONCOLUMN
			{
				// Token: 0x0400AF86 RID: 44934
				public static LocString NAME = UI.FormatAsLink("Distillation Column", "DISTILLATIONCOLUMN");

				// Token: 0x0400AF87 RID: 44935
				public static LocString DESC = "Gets hot and steamy.";

				// Token: 0x0400AF88 RID: 44936
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Separates any ",
					UI.FormatAsLink("Contaminated Water", "DIRTYWATER"),
					" piped through it into ",
					UI.FormatAsLink("Steam", "STEAM"),
					" and ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					"."
				});
			}

			// Token: 0x020027C6 RID: 10182
			public class WIRE
			{
				// Token: 0x0400AF89 RID: 44937
				public static LocString NAME = UI.FormatAsLink("Wire", "WIRE");

				// Token: 0x0400AF8A RID: 44938
				public static LocString DESC = "Electrical wire is used to connect generators, batteries, and buildings.";

				// Token: 0x0400AF8B RID: 44939
				public static LocString EFFECT = "Connects buildings to " + UI.FormatAsLink("Power", "POWER") + " sources.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x020027C7 RID: 10183
			public class WIREBRIDGE
			{
				// Token: 0x0400AF8C RID: 44940
				public static LocString NAME = UI.FormatAsLink("Wire Bridge", "WIREBRIDGE");

				// Token: 0x0400AF8D RID: 44941
				public static LocString DESC = "Splitting generators onto separate grids can prevent overloads and wasted electricity.";

				// Token: 0x0400AF8E RID: 44942
				public static LocString EFFECT = "Runs one wire section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x020027C8 RID: 10184
			public class HIGHWATTAGEWIRE
			{
				// Token: 0x0400AF8F RID: 44943
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE");

				// Token: 0x0400AF90 RID: 44944
				public static LocString DESC = "Higher wattage wire is used to avoid power overloads, particularly for strong generators.";

				// Token: 0x0400AF91 RID: 44945
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than regular ",
					UI.FormatAsLink("Wire", "WIRE"),
					" without overloading.\n\nCannot be run through wall and floor tile."
				});
			}

			// Token: 0x020027C9 RID: 10185
			public class WIREBRIDGEHIGHWATTAGE
			{
				// Token: 0x0400AF92 RID: 44946
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Joint Plate", "WIREBRIDGEHIGHWATTAGE");

				// Token: 0x0400AF93 RID: 44947
				public static LocString DESC = "Joint plates can run Heavi-Watt wires through walls without leaking gas or liquid.";

				// Token: 0x0400AF94 RID: 44948
				public static LocString EFFECT = "Allows " + UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE") + " to be run through wall and floor tile.\n\nFunctions as regular tile.";
			}

			// Token: 0x020027CA RID: 10186
			public class WIREREFINED
			{
				// Token: 0x0400AF95 RID: 44949
				public static LocString NAME = UI.FormatAsLink("Conductive Wire", "WIREREFINED");

				// Token: 0x0400AF96 RID: 44950
				public static LocString DESC = "My Duplicants prefer the look of conductive wire to the regular raggedy stuff.";

				// Token: 0x0400AF97 RID: 44951
				public static LocString EFFECT = "Connects buildings to " + UI.FormatAsLink("Power", "POWER") + " sources.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x020027CB RID: 10187
			public class WIREREFINEDBRIDGE
			{
				// Token: 0x0400AF98 RID: 44952
				public static LocString NAME = UI.FormatAsLink("Conductive Wire Bridge", "WIREREFINEDBRIDGE");

				// Token: 0x0400AF99 RID: 44953
				public static LocString DESC = "Splitting generators onto separate systems can prevent overloads and wasted electricity.";

				// Token: 0x0400AF9A RID: 44954
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than a regular ",
					UI.FormatAsLink("Wire Bridge", "WIREBRIDGE"),
					" without overloading.\n\nRuns one wire section over another without joining them.\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x020027CC RID: 10188
			public class WIREREFINEDHIGHWATTAGE
			{
				// Token: 0x0400AF9B RID: 44955
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Conductive Wire", "WIREREFINEDHIGHWATTAGE");

				// Token: 0x0400AF9C RID: 44956
				public static LocString DESC = "Higher wattage wire is used to avoid power overloads, particularly for strong generators.";

				// Token: 0x0400AF9D RID: 44957
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than regular ",
					UI.FormatAsLink("Wire", "WIRE"),
					" without overloading.\n\nCannot be run through wall and floor tile."
				});
			}

			// Token: 0x020027CD RID: 10189
			public class WIREREFINEDBRIDGEHIGHWATTAGE
			{
				// Token: 0x0400AF9E RID: 44958
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Conductive Joint Plate", "WIREREFINEDBRIDGEHIGHWATTAGE");

				// Token: 0x0400AF9F RID: 44959
				public static LocString DESC = "Joint plates can run Heavi-Watt wires through walls without leaking gas or liquid.";

				// Token: 0x0400AFA0 RID: 44960
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than a regular ",
					UI.FormatAsLink("Heavi-Watt Joint Plate", "WIREBRIDGEHIGHWATTAGE"),
					" without overloading.\n\nAllows ",
					UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE"),
					" to be run through wall and floor tile."
				});
			}

			// Token: 0x020027CE RID: 10190
			public class HANDSANITIZER
			{
				// Token: 0x0400AFA1 RID: 44961
				public static LocString NAME = UI.FormatAsLink("Hand Sanitizer", "HANDSANITIZER");

				// Token: 0x0400AFA2 RID: 44962
				public static LocString DESC = "Hand sanitizers kill germs more effectively than wash basins.";

				// Token: 0x0400AFA3 RID: 44963
				public static LocString EFFECT = "Removes most " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Hand Sanitizers when passing by in the selected direction.";
			}

			// Token: 0x020027CF RID: 10191
			public class WASHBASIN
			{
				// Token: 0x0400AFA4 RID: 44964
				public static LocString NAME = UI.FormatAsLink("Wash Basin", "WASHBASIN");

				// Token: 0x0400AFA5 RID: 44965
				public static LocString DESC = "Germ spread can be reduced by building these where Duplicants often get dirty.";

				// Token: 0x0400AFA6 RID: 44966
				public static LocString EFFECT = "Removes some " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Wash Basins when passing by in the selected direction.";
			}

			// Token: 0x020027D0 RID: 10192
			public class WASHSINK
			{
				// Token: 0x0400AFA7 RID: 44967
				public static LocString NAME = UI.FormatAsLink("Sink", "WASHSINK");

				// Token: 0x0400AFA8 RID: 44968
				public static LocString DESC = "Sinks are plumbed and do not need to be manually emptied or refilled.";

				// Token: 0x0400AFA9 RID: 44969
				public static LocString EFFECT = "Removes " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Sinks when passing by in the selected direction.";

				// Token: 0x02003568 RID: 13672
				public class FACADES
				{
					// Token: 0x02003973 RID: 14707
					public class DEFAULT_WASHSINK
					{
						// Token: 0x0400E1D9 RID: 57817
						public static LocString NAME = UI.FormatAsLink("Sink", "WASHSINK");

						// Token: 0x0400E1DA RID: 57818
						public static LocString DESC = "Sinks are plumbed and do not need to be manually emptied or refilled.";
					}

					// Token: 0x02003974 RID: 14708
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400E1DB RID: 57819
						public static LocString NAME = UI.FormatAsLink("Faint Purple Sink", "WASHSINK");

						// Token: 0x0400E1DC RID: 57820
						public static LocString DESC = "A refreshing splash of color for the light-headed.";
					}

					// Token: 0x02003975 RID: 14709
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400E1DD RID: 57821
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Sink", "WASHSINK");

						// Token: 0x0400E1DE RID: 57822
						public static LocString DESC = "A calm, colorful sink for heavy-hearted Duplicants.";
					}

					// Token: 0x02003976 RID: 14710
					public class GREEN_MUSH
					{
						// Token: 0x0400E1DF RID: 57823
						public static LocString NAME = UI.FormatAsLink("Mush Green Sink", "WASHSINK");

						// Token: 0x0400E1E0 RID: 57824
						public static LocString DESC = "Even the soap is mush-colored.";
					}

					// Token: 0x02003977 RID: 14711
					public class YELLOW_TARTAR
					{
						// Token: 0x0400E1E1 RID: 57825
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Sink", "WASHSINK");

						// Token: 0x0400E1E2 RID: 57826
						public static LocString DESC = "The juxtaposition of 'ick' and 'clean' can be very satisfying.";
					}

					// Token: 0x02003978 RID: 14712
					public class RED_ROSE
					{
						// Token: 0x0400E1E3 RID: 57827
						public static LocString NAME = UI.FormatAsLink("Puce Pink Sink", "WASHSINK");

						// Token: 0x0400E1E4 RID: 57828
						public static LocString DESC = "Some Duplicants say it looks like a germ-devouring mouth.";
					}
				}
			}

			// Token: 0x020027D1 RID: 10193
			public class DECONTAMINATIONSHOWER
			{
				// Token: 0x0400AFAA RID: 44970
				public static LocString NAME = UI.FormatAsLink("Decontamination Shower", "DECONTAMINATIONSHOWER");

				// Token: 0x0400AFAB RID: 44971
				public static LocString DESC = "Don't forget to decontaminate behind your ears.";

				// Token: 0x0400AFAC RID: 44972
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Water", "WATER"),
					" to remove ",
					UI.FormatAsLink("Germs", "DISEASE"),
					" and ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					".\n\nDecontaminates both Duplicants and their ",
					UI.FormatAsLink("Clothing", "EQUIPMENT"),
					"."
				});
			}

			// Token: 0x020027D2 RID: 10194
			public class TILEPOI
			{
				// Token: 0x0400AFAD RID: 44973
				public static LocString NAME = UI.FormatAsLink("Tile", "TILEPOI");

				// Token: 0x0400AFAE RID: 44974
				public static LocString DESC = "";

				// Token: 0x0400AFAF RID: 44975
				public static LocString EFFECT = "Used to build the walls and floor of rooms.";
			}

			// Token: 0x020027D3 RID: 10195
			public class POLYMERIZER
			{
				// Token: 0x0400AFB0 RID: 44976
				public static LocString NAME = UI.FormatAsLink("Polymer Press", "POLYMERIZER");

				// Token: 0x0400AFB1 RID: 44977
				public static LocString DESC = "Plastic can be used to craft unique buildings and goods.";

				// Token: 0x0400AFB2 RID: 44978
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" or ",
					UI.FormatAsLink("Nectar", "SUGARWATER"),
					" into raw ",
					UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
					"."
				});
			}

			// Token: 0x020027D4 RID: 10196
			public class DIRECTIONALWORLDPUMPLIQUID
			{
				// Token: 0x0400AFB3 RID: 44979
				public static LocString NAME = UI.FormatAsLink("Liquid Channel", "DIRECTIONALWORLDPUMPLIQUID");

				// Token: 0x0400AFB4 RID: 44980
				public static LocString DESC = "Channels move more volume than pumps and require no power, but need sufficient pressure to function.";

				// Token: 0x0400AFB5 RID: 44981
				public static LocString EFFECT = "Directionally moves large volumes of " + UI.FormatAsLink("LIQUID", "ELEMENTS_LIQUID") + " through a channel.\n\nCan be used as floor tile and rotated before construction.";
			}

			// Token: 0x020027D5 RID: 10197
			public class STEAMTURBINE
			{
				// Token: 0x0400AFB6 RID: 44982
				public static LocString NAME = UI.FormatAsLink("[DEPRECATED] Steam Turbine", "STEAMTURBINE");

				// Token: 0x0400AFB7 RID: 44983
				public static LocString DESC = "Useful for converting the geothermal energy of magma into usable power.";

				// Token: 0x0400AFB8 RID: 44984
				public static LocString EFFECT = string.Concat(new string[]
				{
					"THIS BUILDING HAS BEEN DEPRECATED AND CANNOT BE BUILT.\n\nGenerates exceptional electrical ",
					UI.FormatAsLink("Power", "POWER"),
					" using pressurized, ",
					UI.FormatAsLink("Scalding", "HEAT"),
					" ",
					UI.FormatAsLink("Steam", "STEAM"),
					".\n\nOutputs significantly cooler ",
					UI.FormatAsLink("Steam", "STEAM"),
					" than it receives.\n\nAir pressure beneath this building must be higher than pressure above for air to flow."
				});
			}

			// Token: 0x020027D6 RID: 10198
			public class STEAMTURBINE2
			{
				// Token: 0x0400AFB9 RID: 44985
				public static LocString NAME = UI.FormatAsLink("Steam Turbine", "STEAMTURBINE2");

				// Token: 0x0400AFBA RID: 44986
				public static LocString DESC = "Useful for converting the geothermal energy into usable power.";

				// Token: 0x0400AFBB RID: 44987
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Steam", "STEAM"),
					" from the tiles directly below the machine's foundation and uses it to generate electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nOutputs ",
					UI.FormatAsLink("Water", "WATER"),
					"."
				});

				// Token: 0x0400AFBC RID: 44988
				public static LocString HEAT_SOURCE = "Power Generation Waste";
			}

			// Token: 0x020027D7 RID: 10199
			public class STEAMENGINE
			{
				// Token: 0x0400AFBD RID: 44989
				public static LocString NAME = UI.FormatAsLink("Steam Engine", "STEAMENGINE");

				// Token: 0x0400AFBE RID: 44990
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x0400AFBF RID: 44991
				public static LocString EFFECT = "Utilizes " + UI.FormatAsLink("Steam", "STEAM") + " to propel rockets for space exploration.\n\nThe engine of a rocket must be built first before more rocket modules may be added.";
			}

			// Token: 0x020027D8 RID: 10200
			public class STEAMENGINECLUSTER
			{
				// Token: 0x0400AFC0 RID: 44992
				public static LocString NAME = UI.FormatAsLink("Steam Engine", "STEAMENGINECLUSTER");

				// Token: 0x0400AFC1 RID: 44993
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x0400AFC2 RID: 44994
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Utilizes ",
					UI.FormatAsLink("Steam", "STEAM"),
					" to propel rockets for space exploration.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x020027D9 RID: 10201
			public class SOLARPANEL
			{
				// Token: 0x0400AFC3 RID: 44995
				public static LocString NAME = UI.FormatAsLink("Solar Panel", "SOLARPANEL");

				// Token: 0x0400AFC4 RID: 44996
				public static LocString DESC = "Solar panels convert high intensity sunlight into power and produce zero waste.";

				// Token: 0x0400AFC5 RID: 44997
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nMust be exposed to space."
				});
			}

			// Token: 0x020027DA RID: 10202
			public class COMETDETECTOR
			{
				// Token: 0x0400AFC6 RID: 44998
				public static LocString NAME = UI.FormatAsLink("Space Scanner", "COMETDETECTOR");

				// Token: 0x0400AFC7 RID: 44999
				public static LocString DESC = "Networks of many scanners will scan more efficiently than one on its own.";

				// Token: 0x0400AFC8 RID: 45000
				public static LocString EFFECT = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to its automation circuit when it detects incoming objects from space.\n\nCan be configured to detect incoming meteor showers or returning space rockets.";
			}

			// Token: 0x020027DB RID: 10203
			public class OILREFINERY
			{
				// Token: 0x0400AFC9 RID: 45001
				public static LocString NAME = UI.FormatAsLink("Oil Refinery", "OILREFINERY");

				// Token: 0x0400AFCA RID: 45002
				public static LocString DESC = "Petroleum can only be produced from the refinement of crude oil.";

				// Token: 0x0400AFCB RID: 45003
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
					" into ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" and ",
					UI.FormatAsLink("Natural Gas", "METHANE"),
					"."
				});
			}

			// Token: 0x020027DC RID: 10204
			public class OILWELLCAP
			{
				// Token: 0x0400AFCC RID: 45004
				public static LocString NAME = UI.FormatAsLink("Oil Well", "OILWELLCAP");

				// Token: 0x0400AFCD RID: 45005
				public static LocString DESC = "Water pumped into an oil reservoir cannot be recovered.";

				// Token: 0x0400AFCE RID: 45006
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Extracts ",
					UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
					" using clean ",
					UI.FormatAsLink("Water", "WATER"),
					".\n\nMust be built atop an ",
					UI.FormatAsLink("Oil Reservoir", "OIL_WELL"),
					"."
				});
			}

			// Token: 0x020027DD RID: 10205
			public class METALREFINERY
			{
				// Token: 0x0400AFCF RID: 45007
				public static LocString NAME = UI.FormatAsLink("Metal Refinery", "METALREFINERY");

				// Token: 0x0400AFD0 RID: 45008
				public static LocString DESC = "Refined metals are necessary to build advanced electronics and technologies.";

				// Token: 0x0400AFD1 RID: 45009
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Refined Metals", "REFINEDMETAL"),
					" from raw ",
					UI.FormatAsLink("Metal Ore", "RAWMETAL"),
					".\n\nSignificantly ",
					UI.FormatAsLink("Heats", "HEAT"),
					" and outputs the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" piped into it.\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x0400AFD2 RID: 45010
				public static LocString RECIPE_DESCRIPTION = "Extracts pure {0} from {1}.";
			}

			// Token: 0x020027DE RID: 10206
			public class MISSILEFABRICATOR
			{
				// Token: 0x0400AFD3 RID: 45011
				public static LocString NAME = UI.FormatAsLink("Blastshot Maker", "MISSILEFABRICATOR");

				// Token: 0x0400AFD4 RID: 45012
				public static LocString DESC = "Blastshot shells are an effective defense against incoming meteor showers.";

				// Token: 0x0400AFD5 RID: 45013
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Blastshot", "MISSILELAUNCHER"),
					" from ",
					UI.FormatAsLink("Refined Metals", "REFINEDMETAL"),
					" combined with ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x0400AFD6 RID: 45014
				public static LocString RECIPE_DESCRIPTION = "Produces {0} from {1} and {2}.";
			}

			// Token: 0x020027DF RID: 10207
			public class GLASSFORGE
			{
				// Token: 0x0400AFD7 RID: 45015
				public static LocString NAME = UI.FormatAsLink("Glass Forge", "GLASSFORGE");

				// Token: 0x0400AFD8 RID: 45016
				public static LocString DESC = "Glass can be used to construct window tile.";

				// Token: 0x0400AFD9 RID: 45017
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Molten Glass", "MOLTENGLASS"),
					" from raw ",
					UI.FormatAsLink("Sand", "SAND"),
					".\n\nOutputs ",
					UI.FormatAsLink("High Temperature", "HEAT"),
					" ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x0400AFDA RID: 45018
				public static LocString RECIPE_DESCRIPTION = "Extracts pure {0} from {1}.";
			}

			// Token: 0x020027E0 RID: 10208
			public class ROCKCRUSHER
			{
				// Token: 0x0400AFDB RID: 45019
				public static LocString NAME = UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER");

				// Token: 0x0400AFDC RID: 45020
				public static LocString DESC = "Rock Crushers loosen nuggets from raw ore and can process many different resources.";

				// Token: 0x0400AFDD RID: 45021
				public static LocString EFFECT = "Inefficiently produces refined materials from raw resources.\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x0400AFDE RID: 45022
				public static LocString RECIPE_DESCRIPTION = "Crushes {0} into {1}";

				// Token: 0x0400AFDF RID: 45023
				public static LocString RECIPE_DESCRIPTION_TWO_OUTPUT = "Crushes {0} into {1} and {2}";

				// Token: 0x0400AFE0 RID: 45024
				public static LocString METAL_RECIPE_DESCRIPTION = "Crushes {1} into " + UI.FormatAsLink("Sand", "SAND") + " and pure {0}";

				// Token: 0x0400AFE1 RID: 45025
				public static LocString LIME_RECIPE_DESCRIPTION = "Crushes {1} into {0}";

				// Token: 0x0400AFE2 RID: 45026
				public static LocString LIME_FROM_LIMESTONE_RECIPE_DESCRIPTION = "Crushes {0} into {1} and a small amount of pure {2}";

				// Token: 0x02003569 RID: 13673
				public class FACADES
				{
					// Token: 0x02003979 RID: 14713
					public class DEFAULT_ROCKCRUSHER
					{
						// Token: 0x0400E1E5 RID: 57829
						public static LocString NAME = UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400E1E6 RID: 57830
						public static LocString DESC = "Rock Crushers loosen nuggets from raw ore and can process many different resources.";
					}

					// Token: 0x0200397A RID: 14714
					public class HANDS
					{
						// Token: 0x0400E1E7 RID: 57831
						public static LocString NAME = UI.FormatAsLink("Punchy Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400E1E8 RID: 57832
						public static LocString DESC = "Smashy smashy!";
					}

					// Token: 0x0200397B RID: 14715
					public class TEETH
					{
						// Token: 0x0400E1E9 RID: 57833
						public static LocString NAME = UI.FormatAsLink("Toothy Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400E1EA RID: 57834
						public static LocString DESC = "Not designed to handle overcooked food waste.";
					}

					// Token: 0x0200397C RID: 14716
					public class ROUNDSTAMP
					{
						// Token: 0x0400E1EB RID: 57835
						public static LocString NAME = UI.FormatAsLink("Smooth Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400E1EC RID: 57836
						public static LocString DESC = "Inspired by the traditional mortar and pestle.";
					}

					// Token: 0x0200397D RID: 14717
					public class SPIKEBEDS
					{
						// Token: 0x0400E1ED RID: 57837
						public static LocString NAME = UI.FormatAsLink("Spiked Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400E1EE RID: 57838
						public static LocString DESC = "Mashes rocks into oblivion.";
					}

					// Token: 0x0200397E RID: 14718
					public class CHOMP
					{
						// Token: 0x0400E1EF RID: 57839
						public static LocString NAME = UI.FormatAsLink("Mani Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400E1F0 RID: 57840
						public static LocString DESC = "Buffs rough ore into smooth little nuggets.";
					}

					// Token: 0x0200397F RID: 14719
					public class GEARS
					{
						// Token: 0x0400E1F1 RID: 57841
						public static LocString NAME = UI.FormatAsLink("Super-Mech Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400E1F2 RID: 57842
						public static LocString DESC = "Uncrushed ore really grinds its gears.";
					}

					// Token: 0x02003980 RID: 14720
					public class BALLOON
					{
						// Token: 0x0400E1F3 RID: 57843
						public static LocString NAME = UI.FormatAsLink("Pop-A-Rocks-E", "ROCKCRUSHER");

						// Token: 0x0400E1F4 RID: 57844
						public static LocString DESC = "Wherever there's raw ore, there's a rock crusher lurking nearby.";
					}
				}
			}

			// Token: 0x020027E1 RID: 10209
			public class SLUDGEPRESS
			{
				// Token: 0x0400AFE3 RID: 45027
				public static LocString NAME = UI.FormatAsLink("Sludge Press", "SLUDGEPRESS");

				// Token: 0x0400AFE4 RID: 45028
				public static LocString DESC = "What Duplicant doesn't love playing with mud?";

				// Token: 0x0400AFE5 RID: 45029
				public static LocString EFFECT = "Separates " + UI.FormatAsLink("Mud", "MUD") + " and other sludges into their base elements.\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x0400AFE6 RID: 45030
				public static LocString RECIPE_DESCRIPTION = "Separates {0} into its base elements.";
			}

			// Token: 0x020027E2 RID: 10210
			public class SUPERMATERIALREFINERY
			{
				// Token: 0x0400AFE7 RID: 45031
				public static LocString NAME = UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY");

				// Token: 0x0400AFE8 RID: 45032
				public static LocString DESC = "Rare materials can be procured through rocket missions into space.";

				// Token: 0x0400AFE9 RID: 45033
				public static LocString EFFECT = "Processes " + UI.FormatAsLink("Rare Materials", "RAREMATERIALS") + " into advanced industrial goods.\n\nRare materials can be retrieved from space missions.\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x0400AFEA RID: 45034
				public static LocString SUPERCOOLANT_RECIPE_DESCRIPTION = "Super Coolant is an industrial-grade " + UI.FormatAsLink("Fullerene", "FULLERENE") + " coolant.";

				// Token: 0x0400AFEB RID: 45035
				public static LocString SUPERINSULATOR_RECIPE_DESCRIPTION = string.Concat(new string[]
				{
					"Insulite reduces ",
					UI.FormatAsLink("Heat Transfer", "HEAT"),
					" and is composed of recrystallized ",
					UI.FormatAsLink("Abyssalite", "KATAIRITE"),
					"."
				});

				// Token: 0x0400AFEC RID: 45036
				public static LocString TEMPCONDUCTORSOLID_RECIPE_DESCRIPTION = "Thermium is an industrial metal alloy formulated to maximize " + UI.FormatAsLink("Heat Transfer", "HEAT") + " and thermal dispersion.";

				// Token: 0x0400AFED RID: 45037
				public static LocString VISCOGEL_RECIPE_DESCRIPTION = "Visco-Gel Fluid is a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " polymer with high surface tension.";

				// Token: 0x0400AFEE RID: 45038
				public static LocString YELLOWCAKE_RECIPE_DESCRIPTION = "Yellowcake is a " + UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID") + " used in uranium enrichment.";

				// Token: 0x0400AFEF RID: 45039
				public static LocString FULLERENE_RECIPE_DESCRIPTION = string.Concat(new string[]
				{
					"Fullerene is a ",
					UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID"),
					" used in the production of ",
					UI.FormatAsLink("Super Coolant", "SUPERCOOLANT"),
					"."
				});

				// Token: 0x0400AFF0 RID: 45040
				public static LocString HARDPLASTIC_RECIPE_DESCRIPTION = "Plastium is a highly heat-resistant, plastic-like " + UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID") + " used for space buildings.";
			}

			// Token: 0x020027E3 RID: 10211
			public class THERMALBLOCK
			{
				// Token: 0x0400AFF1 RID: 45041
				public static LocString NAME = UI.FormatAsLink("Tempshift Plate", "THERMALBLOCK");

				// Token: 0x0400AFF2 RID: 45042
				public static LocString DESC = "The thermal properties of construction materials determine their heat retention.";

				// Token: 0x0400AFF3 RID: 45043
				public static LocString EFFECT = "Accelerates or buffers " + UI.FormatAsLink("Heat", "HEAT") + " dispersal based on the construction material used.\n\nHas a small area of effect.";
			}

			// Token: 0x020027E4 RID: 10212
			public class POWERCONTROLSTATION
			{
				// Token: 0x0400AFF4 RID: 45044
				public static LocString NAME = UI.FormatAsLink("Power Control Station", "POWERCONTROLSTATION");

				// Token: 0x0400AFF5 RID: 45045
				public static LocString DESC = "Only one Duplicant may be assigned to a station at a time.";

				// Token: 0x0400AFF6 RID: 45046
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME,
					" to increase the ",
					UI.FormatAsLink("Power", "POWER"),
					" output of generators.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Tune Up", "TECHNICALS2"),
					" trait.\n\nThis building is a necessary component of the Power Plant room."
				});
			}

			// Token: 0x020027E5 RID: 10213
			public class FARMSTATION
			{
				// Token: 0x0400AFF7 RID: 45047
				public static LocString NAME = UI.FormatAsLink("Farm Station", "FARMSTATION");

				// Token: 0x0400AFF8 RID: 45048
				public static LocString DESC = "This station only has an effect on crops grown within the same room.";

				// Token: 0x0400AFF9 RID: 45049
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Micronutrient Fertilizer", "FARM_STATION_TOOLS"),
					" to increase ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" growth rates.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Crop Tending", "FARMING2"),
					" trait.\n\nThis building is a necessary component of the Greenhouse room."
				});
			}

			// Token: 0x020027E6 RID: 10214
			public class FISHDELIVERYPOINT
			{
				// Token: 0x0400AFFA RID: 45050
				public static LocString NAME = UI.FormatAsLink("Fish Release", "FISHDELIVERYPOINT");

				// Token: 0x0400AFFB RID: 45051
				public static LocString DESC = "A fish release must be built above liquid to prevent released fish from suffocating.";

				// Token: 0x0400AFFC RID: 45052
				public static LocString EFFECT = "Releases trapped " + UI.FormatAsLink("Pacu", "PACU") + " back into the world.\n\nCan be used multiple times.";
			}

			// Token: 0x020027E7 RID: 10215
			public class FISHFEEDER
			{
				// Token: 0x0400AFFD RID: 45053
				public static LocString NAME = UI.FormatAsLink("Fish Feeder", "FISHFEEDER");

				// Token: 0x0400AFFE RID: 45054
				public static LocString DESC = "Build this feeder above a body of water to feed the fish within.";

				// Token: 0x0400AFFF RID: 45055
				public static LocString EFFECT = "Automatically dispenses stored " + UI.FormatAsLink("Critter", "CREATURES") + " food into the area below.\n\nDispenses continuously as food is consumed.";
			}

			// Token: 0x020027E8 RID: 10216
			public class FISHTRAP
			{
				// Token: 0x0400B000 RID: 45056
				public static LocString NAME = UI.FormatAsLink("Fish Trap", "FISHTRAP");

				// Token: 0x0400B001 RID: 45057
				public static LocString DESC = "Trapped fish will automatically be bagged for transport.";

				// Token: 0x0400B002 RID: 45058
				public static LocString EFFECT = "Attracts and traps swimming " + UI.FormatAsLink("Pacu", "PACU") + ".\n\nSingle use.";
			}

			// Token: 0x020027E9 RID: 10217
			public class RANCHSTATION
			{
				// Token: 0x0400B003 RID: 45059
				public static LocString NAME = UI.FormatAsLink("Grooming Station", "RANCHSTATION");

				// Token: 0x0400B004 RID: 45060
				public static LocString DESC = "A groomed critter is a happy, healthy, productive critter.";

				// Token: 0x0400B005 RID: 45061
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows the assigned ",
					UI.FormatAsLink("Rancher", "RANCHER"),
					" to care for ",
					UI.FormatAsLink("Critters", "CREATURES"),
					".\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Critter Ranching", "RANCHING1"),
					" skill."
				});
			}

			// Token: 0x020027EA RID: 10218
			public class MACHINESHOP
			{
				// Token: 0x0400B006 RID: 45062
				public static LocString NAME = UI.FormatAsLink("Mechanics Station", "MACHINESHOP");

				// Token: 0x0400B007 RID: 45063
				public static LocString DESC = "Duplicants will only improve the efficiency of buildings in the same room as this station.";

				// Token: 0x0400B008 RID: 45064
				public static LocString EFFECT = "Allows the assigned " + UI.FormatAsLink("Engineer", "MACHINE_TECHNICIAN") + " to improve building production efficiency.\n\nThis building is a necessary component of the Machine Shop room.";
			}

			// Token: 0x020027EB RID: 10219
			public class LOGICWIRE
			{
				// Token: 0x0400B009 RID: 45065
				public static LocString NAME = UI.FormatAsLink("Automation Wire", "LOGICWIRE");

				// Token: 0x0400B00A RID: 45066
				public static LocString DESC = "Automation wire is used to connect building ports to automation gates.";

				// Token: 0x0400B00B RID: 45067
				public static LocString EFFECT = "Connects buildings to " + UI.FormatAsLink("Sensors", "LOGIC") + ".\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x020027EC RID: 10220
			public class LOGICRIBBON
			{
				// Token: 0x0400B00C RID: 45068
				public static LocString NAME = UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON");

				// Token: 0x0400B00D RID: 45069
				public static LocString DESC = "Logic ribbons use significantly less space to carry multiple automation signals.";

				// Token: 0x0400B00E RID: 45070
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A 4-Bit ",
					BUILDINGS.PREFABS.LOGICWIRE.NAME,
					" which can carry up to four automation signals.\n\nUse a ",
					UI.FormatAsLink("Ribbon Writer", "LOGICRIBBONWRITER"),
					" to output to multiple Bits, and a ",
					UI.FormatAsLink("Ribbon Reader", "LOGICRIBBONREADER"),
					" to input from multiple Bits."
				});
			}

			// Token: 0x020027ED RID: 10221
			public class LOGICWIREBRIDGE
			{
				// Token: 0x0400B00F RID: 45071
				public static LocString NAME = UI.FormatAsLink("Automation Wire Bridge", "LOGICWIREBRIDGE");

				// Token: 0x0400B010 RID: 45072
				public static LocString DESC = "Wire bridges allow multiple automation grids to exist in a small area without connecting.";

				// Token: 0x0400B011 RID: 45073
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Automation Wire", "LOGICWIRE") + " section over another without joining them.\n\nCan be run through wall and floor tile.";

				// Token: 0x0400B012 RID: 45074
				public static LocString LOGIC_PORT = "Transmit Signal";

				// Token: 0x0400B013 RID: 45075
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Pass through the " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400B014 RID: 45076
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Pass through the " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020027EE RID: 10222
			public class LOGICRIBBONBRIDGE
			{
				// Token: 0x0400B015 RID: 45077
				public static LocString NAME = UI.FormatAsLink("Automation Ribbon Bridge", "LOGICRIBBONBRIDGE");

				// Token: 0x0400B016 RID: 45078
				public static LocString DESC = "Wire bridges allow multiple automation grids to exist in a small area without connecting.";

				// Token: 0x0400B017 RID: 45079
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON") + " section over another without joining them.\n\nCan be run through wall and floor tile.";

				// Token: 0x0400B018 RID: 45080
				public static LocString LOGIC_PORT = "Transmit Signal";

				// Token: 0x0400B019 RID: 45081
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Pass through the " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400B01A RID: 45082
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Pass through the " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020027EF RID: 10223
			public class LOGICGATEAND
			{
				// Token: 0x0400B01B RID: 45083
				public static LocString NAME = UI.FormatAsLink("AND Gate", "LOGICGATEAND");

				// Token: 0x0400B01C RID: 45084
				public static LocString DESC = "This gate outputs a Green Signal when both its inputs are receiving Green Signals at the same time.";

				// Token: 0x0400B01D RID: 45085
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when both Input A <b>AND</b> Input B are receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when even one Input is receiving ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					"."
				});

				// Token: 0x0400B01E RID: 45086
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400B01F RID: 45087
				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if both Inputs are receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x0400B020 RID: 45088
				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if any Input is receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);
			}

			// Token: 0x020027F0 RID: 10224
			public class LOGICGATEOR
			{
				// Token: 0x0400B021 RID: 45089
				public static LocString NAME = UI.FormatAsLink("OR Gate", "LOGICGATEOR");

				// Token: 0x0400B022 RID: 45090
				public static LocString DESC = "This gate outputs a Green Signal if receiving one or more Green Signals.";

				// Token: 0x0400B023 RID: 45091
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if at least one of Input A <b>OR</b> Input B is receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when neither Input A or Input B are receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					"."
				});

				// Token: 0x0400B024 RID: 45092
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400B025 RID: 45093
				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if any Input is receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x0400B026 RID: 45094
				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if both Inputs are receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);
			}

			// Token: 0x020027F1 RID: 10225
			public class LOGICGATENOT
			{
				// Token: 0x0400B027 RID: 45095
				public static LocString NAME = UI.FormatAsLink("NOT Gate", "LOGICGATENOT");

				// Token: 0x0400B028 RID: 45096
				public static LocString DESC = "This gate reverses automation signals, turning a Green Signal into a Red Signal and vice versa.";

				// Token: 0x0400B029 RID: 45097
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the Input is receiving a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when its Input is receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"."
				});

				// Token: 0x0400B02A RID: 45098
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400B02B RID: 45099
				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

				// Token: 0x0400B02C RID: 45100
				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);
			}

			// Token: 0x020027F2 RID: 10226
			public class LOGICGATEXOR
			{
				// Token: 0x0400B02D RID: 45101
				public static LocString NAME = UI.FormatAsLink("XOR Gate", "LOGICGATEXOR");

				// Token: 0x0400B02E RID: 45102
				public static LocString DESC = "This gate outputs a Green Signal if exactly one of its Inputs is receiving a Green Signal.";

				// Token: 0x0400B02F RID: 45103
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if exactly one of its Inputs is receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" if both or neither Inputs are receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"."
				});

				// Token: 0x0400B030 RID: 45104
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400B031 RID: 45105
				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if exactly one of its Inputs is receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x0400B032 RID: 45106
				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if both Input signals match (any color)";
			}

			// Token: 0x020027F3 RID: 10227
			public class LOGICGATEBUFFER
			{
				// Token: 0x0400B033 RID: 45107
				public static LocString NAME = UI.FormatAsLink("BUFFER Gate", "LOGICGATEBUFFER");

				// Token: 0x0400B034 RID: 45108
				public static LocString DESC = "This gate continues outputting a Green Signal for a short time after the gate stops receiving a Green Signal.";

				// Token: 0x0400B035 RID: 45109
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the Input is receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					".\n\nContinues sending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for an amount of buffer time after the Input receives a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					"."
				});

				// Token: 0x0400B036 RID: 45110
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400B037 RID: 45111
				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" while receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					". After receiving ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					", will continue sending ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" until the timer has expired"
				});

				// Token: 0x0400B038 RID: 45112
				public static LocString OUTPUT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".";
			}

			// Token: 0x020027F4 RID: 10228
			public class LOGICGATEFILTER
			{
				// Token: 0x0400B039 RID: 45113
				public static LocString NAME = UI.FormatAsLink("FILTER Gate", "LOGICGATEFILTER");

				// Token: 0x0400B03A RID: 45114
				public static LocString DESC = "This gate only lets a Green Signal through if its Input has received a Green Signal that lasted longer than the selected filter time.";

				// Token: 0x0400B03B RID: 45115
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Only lets a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" through if the Input has received a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for longer than the selected filter time.\n\nWill continue outputting a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" if the ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" did not last long enough."
				});

				// Token: 0x0400B03C RID: 45116
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400B03D RID: 45117
				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" after receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" for longer than the selected filter timer"
				});

				// Token: 0x0400B03E RID: 45118
				public static LocString OUTPUT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".";
			}

			// Token: 0x020027F5 RID: 10229
			public class LOGICMEMORY
			{
				// Token: 0x0400B03F RID: 45119
				public static LocString NAME = UI.FormatAsLink("Memory Toggle", "LOGICMEMORY");

				// Token: 0x0400B040 RID: 45120
				public static LocString DESC = "A Memory stores a Green Signal received in the Set Port (S) until the Reset Port (R) receives a Green Signal.";

				// Token: 0x0400B041 RID: 45121
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Contains an internal Memory, and will output whatever signal is stored in that Memory.\n\nSignals sent to the Inputs <i>only</i> affect the Memory, and do not pass through to the Output. \n\nSending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to the Set Port (S) will set the memory to ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					". \n\nSending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to the Reset Port (R) will reset the memory back to ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					"."
				});

				// Token: 0x0400B042 RID: 45122
				public static LocString STATUS_ITEM_VALUE = "Current Value: {0}";

				// Token: 0x0400B043 RID: 45123
				public static LocString READ_PORT = "MEMORY OUTPUT";

				// Token: 0x0400B044 RID: 45124
				public static LocString READ_PORT_ACTIVE = "Outputs a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the internal Memory is set to " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x0400B045 RID: 45125
				public static LocString READ_PORT_INACTIVE = "Outputs a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if the internal Memory is set to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

				// Token: 0x0400B046 RID: 45126
				public static LocString SET_PORT = "SET PORT (S)";

				// Token: 0x0400B047 RID: 45127
				public static LocString SET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set the internal Memory to " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x0400B048 RID: 45128
				public static LocString SET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": No effect";

				// Token: 0x0400B049 RID: 45129
				public static LocString RESET_PORT = "RESET PORT (R)";

				// Token: 0x0400B04A RID: 45130
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the internal Memory to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

				// Token: 0x0400B04B RID: 45131
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": No effect";
			}

			// Token: 0x020027F6 RID: 10230
			public class LOGICGATEMULTIPLEXER
			{
				// Token: 0x0400B04C RID: 45132
				public static LocString NAME = UI.FormatAsLink("Signal Selector", "LOGICGATEMULTIPLEXER");

				// Token: 0x0400B04D RID: 45133
				public static LocString DESC = "Signal Selectors can be used to select which automation signal is relevant to pass through to a given circuit";

				// Token: 0x0400B04E RID: 45134
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Select which one of four Input signals should be sent out the Output, using Control Inputs.\n\nSend a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to the two Control Inputs to determine which Input is selected."
				});

				// Token: 0x0400B04F RID: 45135
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400B050 RID: 45136
				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Receives a ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" or ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					" signal from the selected input"
				});

				// Token: 0x0400B051 RID: 45137
				public static LocString OUTPUT_INACTIVE = "Nothing";
			}

			// Token: 0x020027F7 RID: 10231
			public class LOGICGATEDEMULTIPLEXER
			{
				// Token: 0x0400B052 RID: 45138
				public static LocString NAME = UI.FormatAsLink("Signal Distributor", "LOGICGATEDEMULTIPLEXER");

				// Token: 0x0400B053 RID: 45139
				public static LocString DESC = "Signal Distributors can be used to choose which circuit should receive a given automation signal.";

				// Token: 0x0400B054 RID: 45140
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Route a single Input signal out one of four possible Outputs, based on the selection made by the Control Inputs.\n\nSend a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to the two Control Inputs to determine which Output is selected."
				});

				// Token: 0x0400B055 RID: 45141
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400B056 RID: 45142
				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" or ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					" signal to the selected output"
				});

				// Token: 0x0400B057 RID: 45143
				public static LocString OUTPUT_INACTIVE = "Nothing";
			}

			// Token: 0x020027F8 RID: 10232
			public class LOGICSWITCH
			{
				// Token: 0x0400B058 RID: 45144
				public static LocString NAME = UI.FormatAsLink("Signal Switch", "LOGICSWITCH");

				// Token: 0x0400B059 RID: 45145
				public static LocString DESC = "Signal switches don't turn grids on and off like power switches, but add an extra signal.";

				// Token: 0x0400B05A RID: 45146
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" on an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid."
				});

				// Token: 0x0400B05B RID: 45147
				public static LocString SIDESCREEN_TITLE = "Signal Switch";

				// Token: 0x0400B05C RID: 45148
				public static LocString LOGIC_PORT = "Signal Toggle";

				// Token: 0x0400B05D RID: 45149
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if toggled ON";

				// Token: 0x0400B05E RID: 45150
				public static LocString LOGIC_PORT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if toggled OFF";
			}

			// Token: 0x020027F9 RID: 10233
			public class LOGICPRESSURESENSORGAS
			{
				// Token: 0x0400B05F RID: 45151
				public static LocString NAME = UI.FormatAsLink("Atmo Sensor", "LOGICPRESSURESENSORGAS");

				// Token: 0x0400B060 RID: 45152
				public static LocString DESC = "Atmo sensors can be used to prevent excess oxygen production and overpressurization.";

				// Token: 0x0400B061 RID: 45153
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" pressure enters the chosen range."
				});

				// Token: 0x0400B062 RID: 45154
				public static LocString LOGIC_PORT = UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Pressure";

				// Token: 0x0400B063 RID: 45155
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if Gas pressure is within the selected range";

				// Token: 0x0400B064 RID: 45156
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020027FA RID: 10234
			public class LOGICPRESSURESENSORLIQUID
			{
				// Token: 0x0400B065 RID: 45157
				public static LocString NAME = UI.FormatAsLink("Hydro Sensor", "LOGICPRESSURESENSORLIQUID");

				// Token: 0x0400B066 RID: 45158
				public static LocString DESC = "A hydro sensor can tell a pump to refill its basin as soon as it contains too little liquid.";

				// Token: 0x0400B067 RID: 45159
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" pressure enters the chosen range.\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});

				// Token: 0x0400B068 RID: 45160
				public static LocString LOGIC_PORT = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Pressure";

				// Token: 0x0400B069 RID: 45161
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if Liquid pressure is within the selected range";

				// Token: 0x0400B06A RID: 45162
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020027FB RID: 10235
			public class LOGICTEMPERATURESENSOR
			{
				// Token: 0x0400B06B RID: 45163
				public static LocString NAME = UI.FormatAsLink("Thermo Sensor", "LOGICTEMPERATURESENSOR");

				// Token: 0x0400B06C RID: 45164
				public static LocString DESC = "Thermo sensors can disable buildings when they approach dangerous temperatures.";

				// Token: 0x0400B06D RID: 45165
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ambient ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" enters the chosen range."
				});

				// Token: 0x0400B06E RID: 45166
				public static LocString LOGIC_PORT = "Ambient " + UI.FormatAsLink("Temperature", "HEAT");

				// Token: 0x0400B06F RID: 45167
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if ambient ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" is within the selected range"
				});

				// Token: 0x0400B070 RID: 45168
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020027FC RID: 10236
			public class LOGICLIGHTSENSOR
			{
				// Token: 0x0400B071 RID: 45169
				public static LocString NAME = UI.FormatAsLink("Light Sensor", "LOGICLIGHTSENSOR");

				// Token: 0x0400B072 RID: 45170
				public static LocString DESC = "Light sensors can tell surface bunker doors above solar panels to open or close based on solar light levels.";

				// Token: 0x0400B073 RID: 45171
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ambient ",
					UI.FormatAsLink("Brightness", "LIGHT"),
					" enters the chosen range."
				});

				// Token: 0x0400B074 RID: 45172
				public static LocString LOGIC_PORT = "Ambient " + UI.FormatAsLink("Brightness", "LIGHT");

				// Token: 0x0400B075 RID: 45173
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if ambient ",
					UI.FormatAsLink("Brightness", "LIGHT"),
					" is within the selected range"
				});

				// Token: 0x0400B076 RID: 45174
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020027FD RID: 10237
			public class LOGICWATTAGESENSOR
			{
				// Token: 0x0400B077 RID: 45175
				public static LocString NAME = UI.FormatAsLink("Wattage Sensor", "LOGICWATTSENSOR");

				// Token: 0x0400B078 RID: 45176
				public static LocString DESC = "Wattage sensors can send a signal when a building has switched on or off.";

				// Token: 0x0400B079 RID: 45177
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ",
					UI.FormatAsLink("Wattage", "POWER"),
					" consumed enters the chosen range."
				});

				// Token: 0x0400B07A RID: 45178
				public static LocString LOGIC_PORT = "Consumed " + UI.FormatAsLink("Wattage", "POWER");

				// Token: 0x0400B07B RID: 45179
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if current ",
					UI.FormatAsLink("Wattage", "POWER"),
					" is within the selected range"
				});

				// Token: 0x0400B07C RID: 45180
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020027FE RID: 10238
			public class LOGICHEPSENSOR
			{
				// Token: 0x0400B07D RID: 45181
				public static LocString NAME = UI.FormatAsLink("Radbolt Sensor", "LOGICHEPSENSOR");

				// Token: 0x0400B07E RID: 45182
				public static LocString DESC = "Radbolt sensors can send a signal when a Radbolt passes over them.";

				// Token: 0x0400B07F RID: 45183
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when Radbolts detected enters the chosen range."
				});

				// Token: 0x0400B080 RID: 45184
				public static LocString LOGIC_PORT = "Detected Radbolts";

				// Token: 0x0400B081 RID: 45185
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if detected Radbolts are within the selected range";

				// Token: 0x0400B082 RID: 45186
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020027FF RID: 10239
			public class LOGICTIMEOFDAYSENSOR
			{
				// Token: 0x0400B083 RID: 45187
				public static LocString NAME = UI.FormatAsLink("Cycle Sensor", "LOGICTIMEOFDAYSENSOR");

				// Token: 0x0400B084 RID: 45188
				public static LocString DESC = "Cycle sensors ensure systems always turn on at the same time, day or night, every cycle.";

				// Token: 0x0400B085 RID: 45189
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sets an automatic ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" and ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" schedule within one day-night cycle."
				});

				// Token: 0x0400B086 RID: 45190
				public static LocString LOGIC_PORT = "Cycle Time";

				// Token: 0x0400B087 RID: 45191
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if current time is within the selected ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" range"
				});

				// Token: 0x0400B088 RID: 45192
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002800 RID: 10240
			public class LOGICTIMERSENSOR
			{
				// Token: 0x0400B089 RID: 45193
				public static LocString NAME = UI.FormatAsLink("Timer Sensor", "LOGICTIMERSENSOR");

				// Token: 0x0400B08A RID: 45194
				public static LocString DESC = "Timer sensors create automation schedules for very short or very long periods of time.";

				// Token: 0x0400B08B RID: 45195
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Creates a timer to send ",
					UI.FormatAsAutomationState("Green Signals", UI.AutomationState.Active),
					" and ",
					UI.FormatAsAutomationState("Red Signals", UI.AutomationState.Standby),
					" for specific amounts of time."
				});

				// Token: 0x0400B08C RID: 45196
				public static LocString LOGIC_PORT = "Timer Schedule";

				// Token: 0x0400B08D RID: 45197
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " for the selected amount of Green time";

				// Token: 0x0400B08E RID: 45198
				public static LocString LOGIC_PORT_INACTIVE = "Then, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " for the selected amount of Red time";
			}

			// Token: 0x02002801 RID: 10241
			public class LOGICCRITTERCOUNTSENSOR
			{
				// Token: 0x0400B08F RID: 45199
				public static LocString NAME = UI.FormatAsLink("Critter Sensor", "LOGICCRITTERCOUNTSENSOR");

				// Token: 0x0400B090 RID: 45200
				public static LocString DESC = "Detecting critter populations can help adjust their automated feeding and care regimens.";

				// Token: 0x0400B091 RID: 45201
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the number of eggs and critters in a room."
				});

				// Token: 0x0400B092 RID: 45202
				public static LocString LOGIC_PORT = "Critter Count";

				// Token: 0x0400B093 RID: 45203
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Critters and Eggs in the Room is greater than the selected threshold.";

				// Token: 0x0400B094 RID: 45204
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400B095 RID: 45205
				public static LocString SIDESCREEN_TITLE = "Critter Sensor";

				// Token: 0x0400B096 RID: 45206
				public static LocString COUNT_CRITTER_LABEL = "Count Critters";

				// Token: 0x0400B097 RID: 45207
				public static LocString COUNT_EGG_LABEL = "Count Eggs";
			}

			// Token: 0x02002802 RID: 10242
			public class LOGICCLUSTERLOCATIONSENSOR
			{
				// Token: 0x0400B098 RID: 45208
				public static LocString NAME = UI.FormatAsLink("Starmap Location Sensor", "LOGICCLUSTERLOCATIONSENSOR");

				// Token: 0x0400B099 RID: 45209
				public static LocString DESC = "Starmap Location sensors can signal when a spacecraft is at a certain location";

				// Token: 0x0400B09A RID: 45210
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Send ",
					UI.FormatAsAutomationState("Green Signals", UI.AutomationState.Active),
					" at the chosen Starmap locations and ",
					UI.FormatAsAutomationState("Red Signals", UI.AutomationState.Standby),
					" everywhere else."
				});

				// Token: 0x0400B09B RID: 45211
				public static LocString LOGIC_PORT = "Starmap Location Sensor";

				// Token: 0x0400B09C RID: 45212
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + "when a spacecraft is at the chosen Starmap locations";

				// Token: 0x0400B09D RID: 45213
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002803 RID: 10243
			public class LOGICDUPLICANTSENSOR
			{
				// Token: 0x0400B09E RID: 45214
				public static LocString NAME = UI.FormatAsLink("Duplicant Motion Sensor", "DUPLICANTSENSOR");

				// Token: 0x0400B09F RID: 45215
				public static LocString DESC = "Motion sensors save power by only enabling buildings when Duplicants are nearby.";

				// Token: 0x0400B0A0 RID: 45216
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on whether a Duplicant is in the sensor's range."
				});

				// Token: 0x0400B0A1 RID: 45217
				public static LocString LOGIC_PORT = "Duplicant Motion Sensor";

				// Token: 0x0400B0A2 RID: 45218
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " while a Duplicant is in the sensor's tile range";

				// Token: 0x0400B0A3 RID: 45219
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002804 RID: 10244
			public class LOGICDISEASESENSOR
			{
				// Token: 0x0400B0A4 RID: 45220
				public static LocString NAME = UI.FormatAsLink("Germ Sensor", "LOGICDISEASESENSOR");

				// Token: 0x0400B0A5 RID: 45221
				public static LocString DESC = "Detecting germ populations can help block off or clean up dangerous areas.";

				// Token: 0x0400B0A6 RID: 45222
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on quantity of surrounding ",
					UI.FormatAsLink("Germs", "DISEASE"),
					"."
				});

				// Token: 0x0400B0A7 RID: 45223
				public static LocString LOGIC_PORT = UI.FormatAsLink("Germ", "DISEASE") + " Count";

				// Token: 0x0400B0A8 RID: 45224
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs is within the selected range";

				// Token: 0x0400B0A9 RID: 45225
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002805 RID: 10245
			public class LOGICELEMENTSENSORGAS
			{
				// Token: 0x0400B0AA RID: 45226
				public static LocString NAME = UI.FormatAsLink("Gas Element Sensor", "LOGICELEMENTSENSORGAS");

				// Token: 0x0400B0AB RID: 45227
				public static LocString DESC = "These sensors can detect the presence of a specific gas and alter systems accordingly.";

				// Token: 0x0400B0AC RID: 45228
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" is detected on this sensor's tile.\n\nSends a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when the selected ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" is not present."
				});

				// Token: 0x0400B0AD RID: 45229
				public static LocString LOGIC_PORT = "Specific " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Presence";

				// Token: 0x0400B0AE RID: 45230
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the selected Gas is detected";

				// Token: 0x0400B0AF RID: 45231
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002806 RID: 10246
			public class LOGICELEMENTSENSORLIQUID
			{
				// Token: 0x0400B0B0 RID: 45232
				public static LocString NAME = UI.FormatAsLink("Liquid Element Sensor", "LOGICELEMENTSENSORLIQUID");

				// Token: 0x0400B0B1 RID: 45233
				public static LocString DESC = "These sensors can detect the presence of a specific liquid and alter systems accordingly.";

				// Token: 0x0400B0B2 RID: 45234
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" is detected on this sensor's tile.\n\nSends a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when the selected ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" is not present."
				});

				// Token: 0x0400B0B3 RID: 45235
				public static LocString LOGIC_PORT = "Specific " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Presence";

				// Token: 0x0400B0B4 RID: 45236
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the selected Liquid is detected";

				// Token: 0x0400B0B5 RID: 45237
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002807 RID: 10247
			public class LOGICRADIATIONSENSOR
			{
				// Token: 0x0400B0B6 RID: 45238
				public static LocString NAME = UI.FormatAsLink("Radiation Sensor", "LOGICRADIATIONSENSOR");

				// Token: 0x0400B0B7 RID: 45239
				public static LocString DESC = "Radiation sensors can disable buildings when they detect dangerous levels of radiation.";

				// Token: 0x0400B0B8 RID: 45240
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ambient ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" enters the chosen range."
				});

				// Token: 0x0400B0B9 RID: 45241
				public static LocString LOGIC_PORT = "Ambient " + UI.FormatAsLink("Radiation", "RADIATION");

				// Token: 0x0400B0BA RID: 45242
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if ambient ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" is within the selected range"
				});

				// Token: 0x0400B0BB RID: 45243
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002808 RID: 10248
			public class GASCONDUITDISEASESENSOR
			{
				// Token: 0x0400B0BC RID: 45244
				public static LocString NAME = UI.FormatAsLink("Gas Pipe Germ Sensor", "GASCONDUITDISEASESENSOR");

				// Token: 0x0400B0BD RID: 45245
				public static LocString DESC = "Germ sensors can help control automation behavior in the presence of germs.";

				// Token: 0x0400B0BE RID: 45246
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the internal ",
					UI.FormatAsLink("Germ", "DISEASE"),
					" count of the pipe."
				});

				// Token: 0x0400B0BF RID: 45247
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count";

				// Token: 0x0400B0C0 RID: 45248
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs in the pipe is within the selected range";

				// Token: 0x0400B0C1 RID: 45249
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002809 RID: 10249
			public class LIQUIDCONDUITDISEASESENSOR
			{
				// Token: 0x0400B0C2 RID: 45250
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe Germ Sensor", "LIQUIDCONDUITDISEASESENSOR");

				// Token: 0x0400B0C3 RID: 45251
				public static LocString DESC = "Germ sensors can help control automation behavior in the presence of germs.";

				// Token: 0x0400B0C4 RID: 45252
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the internal ",
					UI.FormatAsLink("Germ", "DISEASE"),
					" count of the pipe."
				});

				// Token: 0x0400B0C5 RID: 45253
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count";

				// Token: 0x0400B0C6 RID: 45254
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs in the pipe is within the selected range";

				// Token: 0x0400B0C7 RID: 45255
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x0200280A RID: 10250
			public class SOLIDCONDUITDISEASESENSOR
			{
				// Token: 0x0400B0C8 RID: 45256
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail Germ Sensor", "SOLIDCONDUITDISEASESENSOR");

				// Token: 0x0400B0C9 RID: 45257
				public static LocString DESC = "Germ sensors can help control automation behavior in the presence of germs.";

				// Token: 0x0400B0CA RID: 45258
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the internal ",
					UI.FormatAsLink("Germ", "DISEASE"),
					" count of the object on the rail."
				});

				// Token: 0x0400B0CB RID: 45259
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count";

				// Token: 0x0400B0CC RID: 45260
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs on the object on the rail is within the selected range";

				// Token: 0x0400B0CD RID: 45261
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x0200280B RID: 10251
			public class GASCONDUITELEMENTSENSOR
			{
				// Token: 0x0400B0CE RID: 45262
				public static LocString NAME = UI.FormatAsLink("Gas Pipe Element Sensor", "GASCONDUITELEMENTSENSOR");

				// Token: 0x0400B0CF RID: 45263
				public static LocString DESC = "Element sensors can be used to detect the presence of a specific gas in a pipe.";

				// Token: 0x0400B0D0 RID: 45264
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" is detected within a pipe."
				});

				// Token: 0x0400B0D1 RID: 45265
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Presence";

				// Token: 0x0400B0D2 RID: 45266
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured Gas is detected";

				// Token: 0x0400B0D3 RID: 45267
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x0200280C RID: 10252
			public class LIQUIDCONDUITELEMENTSENSOR
			{
				// Token: 0x0400B0D4 RID: 45268
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe Element Sensor", "LIQUIDCONDUITELEMENTSENSOR");

				// Token: 0x0400B0D5 RID: 45269
				public static LocString DESC = "Element sensors can be used to detect the presence of a specific liquid in a pipe.";

				// Token: 0x0400B0D6 RID: 45270
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" is detected within a pipe."
				});

				// Token: 0x0400B0D7 RID: 45271
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Presence";

				// Token: 0x0400B0D8 RID: 45272
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured Liquid is detected within the pipe";

				// Token: 0x0400B0D9 RID: 45273
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x0200280D RID: 10253
			public class SOLIDCONDUITELEMENTSENSOR
			{
				// Token: 0x0400B0DA RID: 45274
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail Element Sensor", "SOLIDCONDUITELEMENTSENSOR");

				// Token: 0x0400B0DB RID: 45275
				public static LocString DESC = "Element sensors can be used to detect the presence of a specific item on a rail.";

				// Token: 0x0400B0DC RID: 45276
				public static LocString EFFECT = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the selected item is detected on a rail.";

				// Token: 0x0400B0DD RID: 45277
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Item", "ELEMENTS_LIQUID") + " Presence";

				// Token: 0x0400B0DE RID: 45278
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured item is detected on the rail";

				// Token: 0x0400B0DF RID: 45279
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x0200280E RID: 10254
			public class GASCONDUITTEMPERATURESENSOR
			{
				// Token: 0x0400B0E0 RID: 45280
				public static LocString NAME = UI.FormatAsLink("Gas Pipe Thermo Sensor", "GASCONDUITTEMPERATURESENSOR");

				// Token: 0x0400B0E1 RID: 45281
				public static LocString DESC = "Thermo sensors disable buildings when their pipe contents reach a certain temperature.";

				// Token: 0x0400B0E2 RID: 45282
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when pipe contents enter the chosen ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" range."
				});

				// Token: 0x0400B0E3 RID: 45283
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " " + UI.FormatAsLink("Temperature", "HEAT");

				// Token: 0x0400B0E4 RID: 45284
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained Gas is within the selected Temperature range";

				// Token: 0x0400B0E5 RID: 45285
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x0200280F RID: 10255
			public class LIQUIDCONDUITTEMPERATURESENSOR
			{
				// Token: 0x0400B0E6 RID: 45286
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe Thermo Sensor", "LIQUIDCONDUITTEMPERATURESENSOR");

				// Token: 0x0400B0E7 RID: 45287
				public static LocString DESC = "Thermo sensors disable buildings when their pipe contents reach a certain temperature.";

				// Token: 0x0400B0E8 RID: 45288
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when pipe contents enter the chosen ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" range."
				});

				// Token: 0x0400B0E9 RID: 45289
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " " + UI.FormatAsLink("Temperature", "HEAT");

				// Token: 0x0400B0EA RID: 45290
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained Liquid is within the selected Temperature range";

				// Token: 0x0400B0EB RID: 45291
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002810 RID: 10256
			public class SOLIDCONDUITTEMPERATURESENSOR
			{
				// Token: 0x0400B0EC RID: 45292
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail Thermo Sensor", "SOLIDCONDUITTEMPERATURESENSOR");

				// Token: 0x0400B0ED RID: 45293
				public static LocString DESC = "Thermo sensors disable buildings when their rail contents reach a certain temperature.";

				// Token: 0x0400B0EE RID: 45294
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when rail contents enter the chosen ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" range."
				});

				// Token: 0x0400B0EF RID: 45295
				public static LocString LOGIC_PORT = "Internal Item " + UI.FormatAsLink("Temperature", "HEAT");

				// Token: 0x0400B0F0 RID: 45296
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained item is within the selected Temperature range";

				// Token: 0x0400B0F1 RID: 45297
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002811 RID: 10257
			public class LOGICCOUNTER
			{
				// Token: 0x0400B0F2 RID: 45298
				public static LocString NAME = UI.FormatAsLink("Signal Counter", "LOGICCOUNTER");

				// Token: 0x0400B0F3 RID: 45299
				public static LocString DESC = "For numbers higher than ten connect multiple counters together.";

				// Token: 0x0400B0F4 RID: 45300
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Counts how many times a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" has been received up to a chosen number.\n\nWhen the chosen number is reached it sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" until it receives another ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					", when it resets automatically and begins counting again."
				});

				// Token: 0x0400B0F5 RID: 45301
				public static LocString LOGIC_PORT = "Internal Counter Value";

				// Token: 0x0400B0F6 RID: 45302
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Increase counter by one";

				// Token: 0x0400B0F7 RID: 45303
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";

				// Token: 0x0400B0F8 RID: 45304
				public static LocString LOGIC_PORT_RESET = "Reset Counter";

				// Token: 0x0400B0F9 RID: 45305
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset counter";

				// Token: 0x0400B0FA RID: 45306
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";

				// Token: 0x0400B0FB RID: 45307
				public static LocString LOGIC_PORT_OUTPUT = "Number Reached";

				// Token: 0x0400B0FC RID: 45308
				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the counter matches the selected value";

				// Token: 0x0400B0FD RID: 45309
				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002812 RID: 10258
			public class LOGICALARM
			{
				// Token: 0x0400B0FE RID: 45310
				public static LocString NAME = UI.FormatAsLink("Automated Notifier", "LOGICALARM");

				// Token: 0x0400B0FF RID: 45311
				public static LocString DESC = "Sends a notification when it receives a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".";

				// Token: 0x0400B100 RID: 45312
				public static LocString EFFECT = "Attach to sensors to send a notification when certain conditions are met.\n\nNotifications can be customized.";

				// Token: 0x0400B101 RID: 45313
				public static LocString LOGIC_PORT = "Notification";

				// Token: 0x0400B102 RID: 45314
				public static LocString INPUT_NAME = "INPUT";

				// Token: 0x0400B103 RID: 45315
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Push notification";

				// Token: 0x0400B104 RID: 45316
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x02002813 RID: 10259
			public class PIXELPACK
			{
				// Token: 0x0400B105 RID: 45317
				public static LocString NAME = UI.FormatAsLink("Pixel Pack", "PIXELPACK");

				// Token: 0x0400B106 RID: 45318
				public static LocString DESC = "Four pixels which can be individually designated different colors.";

				// Token: 0x0400B107 RID: 45319
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Pixels can be designated a color when it receives a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" and a different color when it receives a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					".\n\nInput from an ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE"),
					" controls the whole strip. Input from an ",
					UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON"),
					" can control individual pixels on the strip."
				});

				// Token: 0x0400B108 RID: 45320
				public static LocString LOGIC_PORT = "Color Selection";

				// Token: 0x0400B109 RID: 45321
				public static LocString INPUT_NAME = "RIBBON INPUT";

				// Token: 0x0400B10A RID: 45322
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Display the configured " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " pixels";

				// Token: 0x0400B10B RID: 45323
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Display the configured " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " pixels";

				// Token: 0x0400B10C RID: 45324
				public static LocString SIDESCREEN_TITLE = "Pixel Pack";
			}

			// Token: 0x02002814 RID: 10260
			public class LOGICHAMMER
			{
				// Token: 0x0400B10D RID: 45325
				public static LocString NAME = UI.FormatAsLink("Hammer", "LOGICHAMMER");

				// Token: 0x0400B10E RID: 45326
				public static LocString DESC = "The hammer makes neat sounds when it strikes buildings.";

				// Token: 0x0400B10F RID: 45327
				public static LocString EFFECT = "In its default orientation, the hammer strikes the building to the left when it receives a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".\n\nEach building has a unique sound when struck by the hammer.\n\nThe hammer does no damage when it strikes.";

				// Token: 0x0400B110 RID: 45328
				public static LocString LOGIC_PORT = "Resonating Buildings";

				// Token: 0x0400B111 RID: 45329
				public static LocString INPUT_NAME = "INPUT";

				// Token: 0x0400B112 RID: 45330
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Hammer strikes once";

				// Token: 0x0400B113 RID: 45331
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x02002815 RID: 10261
			public class LOGICRIBBONWRITER
			{
				// Token: 0x0400B114 RID: 45332
				public static LocString NAME = UI.FormatAsLink("Ribbon Writer", "LOGICRIBBONWRITER");

				// Token: 0x0400B115 RID: 45333
				public static LocString DESC = "Translates the signal from an " + UI.FormatAsLink("Automation Wire", "LOGICWIRE") + " to a single Bit in an " + UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON");

				// Token: 0x0400B116 RID: 45334
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Writes a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to the specified Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME,
					"\n\n",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME,
					" must be used as the output wire to avoid overloading."
				});

				// Token: 0x0400B117 RID: 45335
				public static LocString LOGIC_PORT = "1-Bit Input";

				// Token: 0x0400B118 RID: 45336
				public static LocString INPUT_NAME = "INPUT";

				// Token: 0x0400B119 RID: 45337
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Receives " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to be written to selected Bit";

				// Token: 0x0400B11A RID: 45338
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Receives " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " to to be written selected Bit";

				// Token: 0x0400B11B RID: 45339
				public static LocString LOGIC_PORT_OUTPUT = "Bit Writing";

				// Token: 0x0400B11C RID: 45340
				public static LocString OUTPUT_NAME = "RIBBON OUTPUT";

				// Token: 0x0400B11D RID: 45341
				public static LocString OUTPUT_PORT_ACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					": Writes a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to selected Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME
				});

				// Token: 0x0400B11E RID: 45342
				public static LocString OUTPUT_PORT_INACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					": Writes a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to selected Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME
				});
			}

			// Token: 0x02002816 RID: 10262
			public class LOGICRIBBONREADER
			{
				// Token: 0x0400B11F RID: 45343
				public static LocString NAME = UI.FormatAsLink("Ribbon Reader", "LOGICRIBBONREADER");

				// Token: 0x0400B120 RID: 45344
				public static LocString DESC = string.Concat(new string[]
				{
					"Inputs the signal from a single Bit in an ",
					UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON"),
					" into an ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE"),
					"."
				});

				// Token: 0x0400B121 RID: 45345
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reads a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" from the specified Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME,
					" onto an ",
					BUILDINGS.PREFABS.LOGICWIRE.NAME,
					"."
				});

				// Token: 0x0400B122 RID: 45346
				public static LocString LOGIC_PORT = "4-Bit Input";

				// Token: 0x0400B123 RID: 45347
				public static LocString INPUT_NAME = "RIBBON INPUT";

				// Token: 0x0400B124 RID: 45348
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reads a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " from selected Bit";

				// Token: 0x0400B125 RID: 45349
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Reads a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " from selected Bit";

				// Token: 0x0400B126 RID: 45350
				public static LocString LOGIC_PORT_OUTPUT = "Bit Reading";

				// Token: 0x0400B127 RID: 45351
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x0400B128 RID: 45352
				public static LocString OUTPUT_PORT_ACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					": Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to attached ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE")
				});

				// Token: 0x0400B129 RID: 45353
				public static LocString OUTPUT_PORT_INACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					": Sends a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to attached ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE")
				});
			}

			// Token: 0x02002817 RID: 10263
			public class TRAVELTUBEENTRANCE
			{
				// Token: 0x0400B12A RID: 45354
				public static LocString NAME = UI.FormatAsLink("Transit Tube Access", "TRAVELTUBEENTRANCE");

				// Token: 0x0400B12B RID: 45355
				public static LocString DESC = "Duplicants require access points to enter tubes, but not to exit them.";

				// Token: 0x0400B12C RID: 45356
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to enter the connected ",
					UI.FormatAsLink("Transit Tube", "TRAVELTUBE"),
					" system.\n\nStops drawing ",
					UI.FormatAsLink("Power", "POWER"),
					" once fully charged."
				});
			}

			// Token: 0x02002818 RID: 10264
			public class TRAVELTUBE
			{
				// Token: 0x0400B12D RID: 45357
				public static LocString NAME = UI.FormatAsLink("Transit Tube", "TRAVELTUBE");

				// Token: 0x0400B12E RID: 45358
				public static LocString DESC = "Duplicants will only exit a transit tube when a safe landing area is available beneath it.";

				// Token: 0x0400B12F RID: 45359
				public static LocString EFFECT = "Quickly transports Duplicants from a " + UI.FormatAsLink("Transit Tube Access", "TRAVELTUBEENTRANCE") + " to the tube's end.\n\nOnly transports Duplicants.";
			}

			// Token: 0x02002819 RID: 10265
			public class TRAVELTUBEWALLBRIDGE
			{
				// Token: 0x0400B130 RID: 45360
				public static LocString NAME = UI.FormatAsLink("Transit Tube Crossing", "TRAVELTUBEWALLBRIDGE");

				// Token: 0x0400B131 RID: 45361
				public static LocString DESC = "Tube crossings can run transit tubes through walls without leaking gas or liquid.";

				// Token: 0x0400B132 RID: 45362
				public static LocString EFFECT = "Allows " + UI.FormatAsLink("Transit Tubes", "TRAVELTUBE") + " to be run through wall and floor tile.\n\nFunctions as regular tile.";
			}

			// Token: 0x0200281A RID: 10266
			public class SOLIDCONDUIT
			{
				// Token: 0x0400B133 RID: 45363
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT");

				// Token: 0x0400B134 RID: 45364
				public static LocString DESC = "Rails move materials where they'll be needed most, saving Duplicants the walk.";

				// Token: 0x0400B135 RID: 45365
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Transports ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" on a track between ",
					UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX"),
					" and ",
					UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x0200281B RID: 10267
			public class SOLIDCONDUITINBOX
			{
				// Token: 0x0400B136 RID: 45366
				public static LocString NAME = UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX");

				// Token: 0x0400B137 RID: 45367
				public static LocString DESC = "Material filters can be used to determine what resources are sent down the rail.";

				// Token: 0x0400B138 RID: 45368
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" onto ",
					UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT"),
					" for transport.\n\nOnly loads the resources of your choosing."
				});
			}

			// Token: 0x0200281C RID: 10268
			public class SOLIDCONDUITOUTBOX
			{
				// Token: 0x0400B139 RID: 45369
				public static LocString NAME = UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX");

				// Token: 0x0400B13A RID: 45370
				public static LocString DESC = "When materials reach the end of a rail they enter a receptacle to be used by Duplicants.";

				// Token: 0x0400B13B RID: 45371
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" from a ",
					UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT"),
					" into storage."
				});
			}

			// Token: 0x0200281D RID: 10269
			public class SOLIDTRANSFERARM
			{
				// Token: 0x0400B13C RID: 45372
				public static LocString NAME = UI.FormatAsLink("Auto-Sweeper", "SOLIDTRANSFERARM");

				// Token: 0x0400B13D RID: 45373
				public static LocString DESC = "An auto-sweeper's range can be viewed at any time by " + UI.CLICK(UI.ClickType.clicking) + " on the building.";

				// Token: 0x0400B13E RID: 45374
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automates ",
					UI.FormatAsLink("Sweeping", "CHORES"),
					" and ",
					UI.FormatAsLink("Supplying", "CHORES"),
					" errands by sucking up all nearby ",
					UI.FormatAsLink("Debris", "DECOR"),
					".\n\nMaterials are automatically delivered to any ",
					UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX"),
					", ",
					UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX"),
					", storage, or buildings within range."
				});
			}

			// Token: 0x0200281E RID: 10270
			public class SOLIDCONDUITBRIDGE
			{
				// Token: 0x0400B13F RID: 45375
				public static LocString NAME = UI.FormatAsLink("Conveyor Bridge", "SOLIDCONDUITBRIDGE");

				// Token: 0x0400B140 RID: 45376
				public static LocString DESC = "Separating rail systems helps ensure materials go to the intended destinations.";

				// Token: 0x0400B141 RID: 45377
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT") + " section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x0200281F RID: 10271
			public class SOLIDVENT
			{
				// Token: 0x0400B142 RID: 45378
				public static LocString NAME = UI.FormatAsLink("Conveyor Chute", "SOLIDVENT");

				// Token: 0x0400B143 RID: 45379
				public static LocString DESC = "When materials reach the end of a rail they are dropped back into the world.";

				// Token: 0x0400B144 RID: 45380
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" from a ",
					UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT"),
					" onto the floor."
				});
			}

			// Token: 0x02002820 RID: 10272
			public class SOLIDLOGICVALVE
			{
				// Token: 0x0400B145 RID: 45381
				public static LocString NAME = UI.FormatAsLink("Conveyor Shutoff", "SOLIDLOGICVALVE");

				// Token: 0x0400B146 RID: 45382
				public static LocString DESC = "Automated conveyors save power and time by removing the need for Duplicant input.";

				// Token: 0x0400B147 RID: 45383
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID"),
					" transport on or off."
				});

				// Token: 0x0400B148 RID: 45384
				public static LocString LOGIC_PORT = "Open/Close";

				// Token: 0x0400B149 RID: 45385
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow material transport";

				// Token: 0x0400B14A RID: 45386
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent material transport";
			}

			// Token: 0x02002821 RID: 10273
			public class SOLIDLIMITVALVE
			{
				// Token: 0x0400B14B RID: 45387
				public static LocString NAME = UI.FormatAsLink("Conveyor Meter", "SOLIDLIMITVALVE");

				// Token: 0x0400B14C RID: 45388
				public static LocString DESC = "Conveyor Meters let an exact amount of materials pass through before shutting off.";

				// Token: 0x0400B14D RID: 45389
				public static LocString EFFECT = "Connects to an " + UI.FormatAsLink("Automation", "LOGIC") + " grid to automatically turn material transfer off when the specified amount has passed through it.";

				// Token: 0x0400B14E RID: 45390
				public static LocString LOGIC_PORT_OUTPUT = "Limit Reached";

				// Token: 0x0400B14F RID: 45391
				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if limit has been reached";

				// Token: 0x0400B150 RID: 45392
				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400B151 RID: 45393
				public static LocString LOGIC_PORT_RESET = "Reset Meter";

				// Token: 0x0400B152 RID: 45394
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the amount";

				// Token: 0x0400B153 RID: 45395
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x02002822 RID: 10274
			public class DEVPUMPSOLID
			{
				// Token: 0x0400B154 RID: 45396
				public static LocString NAME = "Dev Pump Solid";

				// Token: 0x0400B155 RID: 45397
				public static LocString DESC = "Piping a pump's output to a building's intake will send solids to that building.";

				// Token: 0x0400B156 RID: 45398
				public static LocString EFFECT = "Generates chosen " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " and runs it through " + UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT");
			}

			// Token: 0x02002823 RID: 10275
			public class AUTOMINER
			{
				// Token: 0x0400B157 RID: 45399
				public static LocString NAME = UI.FormatAsLink("Robo-Miner", "AUTOMINER");

				// Token: 0x0400B158 RID: 45400
				public static LocString DESC = "A robo-miner's range can be viewed at any time by selecting the building.";

				// Token: 0x0400B159 RID: 45401
				public static LocString EFFECT = "Automatically digs out all materials in a set range.";
			}

			// Token: 0x02002824 RID: 10276
			public class CREATUREFEEDER
			{
				// Token: 0x0400B15A RID: 45402
				public static LocString NAME = UI.FormatAsLink("Critter Feeder", "CREATUREFEEDER");

				// Token: 0x0400B15B RID: 45403
				public static LocString DESC = "Critters tend to stay close to their food source and wander less when given a feeder.";

				// Token: 0x0400B15C RID: 45404
				public static LocString EFFECT = "Automatically dispenses food for hungry " + UI.FormatAsLink("Critters", "CREATURES") + ".";
			}

			// Token: 0x02002825 RID: 10277
			public class GRAVITASPEDESTAL
			{
				// Token: 0x0400B15D RID: 45405
				public static LocString NAME = UI.FormatAsLink("Pedestal", "ITEMPEDESTAL");

				// Token: 0x0400B15E RID: 45406
				public static LocString DESC = "Perception can be drastically changed by a bit of thoughtful presentation.";

				// Token: 0x0400B15F RID: 45407
				public static LocString EFFECT = "Displays a single object, doubling its " + UI.FormatAsLink("Decor", "DECOR") + " value.\n\nObjects with negative Decor will gain some positive Decor when displayed.";

				// Token: 0x0400B160 RID: 45408
				public static LocString DISPLAYED_ITEM_FMT = "Displayed {0}";
			}

			// Token: 0x02002826 RID: 10278
			public class ITEMPEDESTAL
			{
				// Token: 0x0400B161 RID: 45409
				public static LocString NAME = UI.FormatAsLink("Pedestal", "ITEMPEDESTAL");

				// Token: 0x0400B162 RID: 45410
				public static LocString DESC = "Perception can be drastically changed by a bit of thoughtful presentation.";

				// Token: 0x0400B163 RID: 45411
				public static LocString EFFECT = "Displays a single object, doubling its " + UI.FormatAsLink("Decor", "DECOR") + " value.\n\nObjects with negative Decor will gain some positive Decor when displayed.";

				// Token: 0x0400B164 RID: 45412
				public static LocString DISPLAYED_ITEM_FMT = "Displayed {0}";

				// Token: 0x0200356A RID: 13674
				public class FACADES
				{
					// Token: 0x02003981 RID: 14721
					public class DEFAULT_ITEMPEDESTAL
					{
						// Token: 0x0400E1F5 RID: 57845
						public static LocString NAME = UI.FormatAsLink("Pedestal", "ITEMPEDESTAL");

						// Token: 0x0400E1F6 RID: 57846
						public static LocString DESC = "Perception can be drastically changed by a bit of thoughtful presentation.";
					}

					// Token: 0x02003982 RID: 14722
					public class HAND
					{
						// Token: 0x0400E1F7 RID: 57847
						public static LocString NAME = UI.FormatAsLink("Hand of Dupe Pedestal", "ITEMPEDESTAL");

						// Token: 0x0400E1F8 RID: 57848
						public static LocString DESC = "This pedestal cradles precious objects in the palm of its hand.";
					}
				}
			}

			// Token: 0x02002827 RID: 10279
			public class CROWNMOULDING
			{
				// Token: 0x0400B165 RID: 45413
				public static LocString NAME = UI.FormatAsLink("Ceiling Trim", "CROWNMOULDING");

				// Token: 0x0400B166 RID: 45414
				public static LocString DESC = "Ceiling trim is a purely decorative addition to one's overhead area.";

				// Token: 0x0400B167 RID: 45415
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to decorate the ceilings of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x0200356B RID: 13675
				public class FACADES
				{
					// Token: 0x02003983 RID: 14723
					public class DEFAULT_CROWNMOULDING
					{
						// Token: 0x0400E1F9 RID: 57849
						public static LocString NAME = UI.FormatAsLink("Ceiling Trim", "CROWNMOULDING");

						// Token: 0x0400E1FA RID: 57850
						public static LocString DESC = "Ceiling trim is a purely decorative addition to one's overhead area.";
					}

					// Token: 0x02003984 RID: 14724
					public class SHINEORNAMENTS
					{
						// Token: 0x0400E1FB RID: 57851
						public static LocString NAME = UI.FormatAsLink("Fancy Bug Ceiling Garland", "CROWNMOULDING");

						// Token: 0x0400E1FC RID: 57852
						public static LocString DESC = "Someone spent their entire weekend gluing ribbons to paper Shine Bug cut-outs, and it shows.";
					}
				}
			}

			// Token: 0x02002828 RID: 10280
			public class CORNERMOULDING
			{
				// Token: 0x0400B168 RID: 45416
				public static LocString NAME = UI.FormatAsLink("Corner Trim", "CORNERMOULDING");

				// Token: 0x0400B169 RID: 45417
				public static LocString DESC = "Corner trim is a purely decorative addition for ceiling corners.";

				// Token: 0x0400B16A RID: 45418
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to decorate the ceiling corners of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x0200356C RID: 13676
				public class FACADES
				{
					// Token: 0x02003985 RID: 14725
					public class DEFAULT_CORNERMOULDING
					{
						// Token: 0x0400E1FD RID: 57853
						public static LocString NAME = UI.FormatAsLink("Corner Trim", "CORNERMOULDING");

						// Token: 0x0400E1FE RID: 57854
						public static LocString DESC = "It really dresses up a ceiling corner.";
					}

					// Token: 0x02003986 RID: 14726
					public class SHINEORNAMENTS
					{
						// Token: 0x0400E1FF RID: 57855
						public static LocString NAME = UI.FormatAsLink("Fancy Bug Corner Garland", "CORNERMOULDING");

						// Token: 0x0400E200 RID: 57856
						public static LocString DESC = "Why deck the halls, when you could <i>festoon</i> them?";
					}
				}
			}

			// Token: 0x02002829 RID: 10281
			public class EGGINCUBATOR
			{
				// Token: 0x0400B16B RID: 45419
				public static LocString NAME = UI.FormatAsLink("Incubator", "EGGINCUBATOR");

				// Token: 0x0400B16C RID: 45420
				public static LocString DESC = "Incubators can maintain the ideal internal conditions for several species of critter egg.";

				// Token: 0x0400B16D RID: 45421
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Incubates ",
					UI.FormatAsLink("Critter", "CREATURES"),
					" eggs until ready to hatch.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Critter Ranching", "RANCHING1"),
					" skill."
				});
			}

			// Token: 0x0200282A RID: 10282
			public class EGGCRACKER
			{
				// Token: 0x0400B16E RID: 45422
				public static LocString NAME = UI.FormatAsLink("Egg Cracker", "EGGCRACKER");

				// Token: 0x0400B16F RID: 45423
				public static LocString DESC = "Raw eggs are an ingredient in certain high quality food recipes.";

				// Token: 0x0400B170 RID: 45424
				public static LocString EFFECT = "Converts viable " + UI.FormatAsLink("Critter", "CREATURES") + " eggs into cooking ingredients.\n\nCracked Eggs cannot hatch.\n\nDuplicants will not crack eggs unless tasks are queued.";

				// Token: 0x0400B171 RID: 45425
				public static LocString RECIPE_DESCRIPTION = "Turns {0} into {1}.";

				// Token: 0x0400B172 RID: 45426
				public static LocString RESULT_DESCRIPTION = "Cracked {0}";

				// Token: 0x0200356D RID: 13677
				public class FACADES
				{
					// Token: 0x02003987 RID: 14727
					public class DEFAULT_EGGCRACKER
					{
						// Token: 0x0400E201 RID: 57857
						public static LocString NAME = UI.FormatAsLink("Egg Cracker", "EGGCRACKER");

						// Token: 0x0400E202 RID: 57858
						public static LocString DESC = "It cracks eggs.";
					}

					// Token: 0x02003988 RID: 14728
					public class BEAKER
					{
						// Token: 0x0400E203 RID: 57859
						public static LocString NAME = UI.FormatAsLink("Beaker Cracker", "EGGCRACKER");

						// Token: 0x0400E204 RID: 57860
						public static LocString DESC = "A practical exercise in physics.";
					}

					// Token: 0x02003989 RID: 14729
					public class FLOWER
					{
						// Token: 0x0400E205 RID: 57861
						public static LocString NAME = UI.FormatAsLink("Blossom Cracker", "EGGCRACKER");

						// Token: 0x0400E206 RID: 57862
						public static LocString DESC = "Now with EZ-clean petals.";
					}

					// Token: 0x0200398A RID: 14730
					public class HANDS
					{
						// Token: 0x0400E207 RID: 57863
						public static LocString NAME = UI.FormatAsLink("Handy Cracker", "EGGCRACKER");

						// Token: 0x0400E208 RID: 57864
						public static LocString DESC = "Just like Mi-Ma used to have.";
					}
				}
			}

			// Token: 0x0200282B RID: 10283
			public class URANIUMCENTRIFUGE
			{
				// Token: 0x0400B173 RID: 45427
				public static LocString NAME = UI.FormatAsLink("Uranium Centrifuge", "URANIUMCENTRIFUGE");

				// Token: 0x0400B174 RID: 45428
				public static LocString DESC = "Enriched uranium is a specialized substance that can be used to fuel powerful research reactors.";

				// Token: 0x0400B175 RID: 45429
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Extracts ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" from ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					".\n\nOutputs ",
					UI.FormatAsLink("Depleted Uranium", "DEPLETEDURANIUM"),
					" in molten form."
				});

				// Token: 0x0400B176 RID: 45430
				public static LocString RECIPE_DESCRIPTION = "Convert Uranium ore to Molten Uranium and Enriched Uranium";
			}

			// Token: 0x0200282C RID: 10284
			public class HIGHENERGYPARTICLEREDIRECTOR
			{
				// Token: 0x0400B177 RID: 45431
				public static LocString NAME = UI.FormatAsLink("Radbolt Reflector", "HIGHENERGYPARTICLEREDIRECTOR");

				// Token: 0x0400B178 RID: 45432
				public static LocString DESC = "We were all out of mirrors.";

				// Token: 0x0400B179 RID: 45433
				public static LocString EFFECT = "Receives and redirects Radbolts from " + UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER") + ".";

				// Token: 0x0400B17A RID: 45434
				public static LocString LOGIC_PORT = "Ignore incoming Radbolts";

				// Token: 0x0400B17B RID: 45435
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow incoming Radbolts";

				// Token: 0x0400B17C RID: 45436
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Ignore incoming Radbolts";
			}

			// Token: 0x0200282D RID: 10285
			public class MANUALHIGHENERGYPARTICLESPAWNER
			{
				// Token: 0x0400B17D RID: 45437
				public static LocString NAME = UI.FormatAsLink("Manual Radbolt Generator", "MANUALHIGHENERGYPARTICLESPAWNER");

				// Token: 0x0400B17E RID: 45438
				public static LocString DESC = "Radbolts are necessary for producing Materials Science research.";

				// Token: 0x0400B17F RID: 45439
				public static LocString EFFECT = "Refines radioactive ores to generate Radbolts.\n\nEmits generated Radbolts in the direction of your choosing.";

				// Token: 0x0400B180 RID: 45440
				public static LocString RECIPE_DESCRIPTION = "Creates " + UI.FormatAsLink("Radbolts", "RADIATION") + " by processing {0}. Also creates {1} as a byproduct.";
			}

			// Token: 0x0200282E RID: 10286
			public class HIGHENERGYPARTICLESPAWNER
			{
				// Token: 0x0400B181 RID: 45441
				public static LocString NAME = UI.FormatAsLink("Radbolt Generator", "HIGHENERGYPARTICLESPAWNER");

				// Token: 0x0400B182 RID: 45442
				public static LocString DESC = "Radbolts are necessary for producing Materials Science research.";

				// Token: 0x0400B183 RID: 45443
				public static LocString EFFECT = "Attracts nearby " + UI.FormatAsLink("Radiation", "RADIATION") + " to generate Radbolts.\n\nEmits generated Radbolts in the direction of your choosing when the set Radbolt threshold is reached.\n\nRadbolts collected will rapidly decay while this building is disabled.";

				// Token: 0x0400B184 RID: 45444
				public static LocString LOGIC_PORT = "Do not emit Radbolts";

				// Token: 0x0400B185 RID: 45445
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Emit Radbolts";

				// Token: 0x0400B186 RID: 45446
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Do not emit Radbolts";
			}

			// Token: 0x0200282F RID: 10287
			public class DEVHEPSPAWNER
			{
				// Token: 0x0400B187 RID: 45447
				public static LocString NAME = "Dev Radbolt Generator";

				// Token: 0x0400B188 RID: 45448
				public static LocString DESC = "Radbolts are necessary for producing Materials Science research.";

				// Token: 0x0400B189 RID: 45449
				public static LocString EFFECT = "Generates Radbolts.";

				// Token: 0x0400B18A RID: 45450
				public static LocString LOGIC_PORT = "Do not emit Radbolts";

				// Token: 0x0400B18B RID: 45451
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Emit Radbolts";

				// Token: 0x0400B18C RID: 45452
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Do not emit Radbolts";
			}

			// Token: 0x02002830 RID: 10288
			public class HEPBATTERY
			{
				// Token: 0x0400B18D RID: 45453
				public static LocString NAME = UI.FormatAsLink("Radbolt Chamber", "HEPBATTERY");

				// Token: 0x0400B18E RID: 45454
				public static LocString DESC = "Particles packed up and ready to go.";

				// Token: 0x0400B18F RID: 45455
				public static LocString EFFECT = "Stores Radbolts in a high-energy state, ready for transport.\n\nRequires a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to release radbolts from storage when the Radbolt threshold is reached.\n\nRadbolts in storage will rapidly decay while this building is disabled.";

				// Token: 0x0400B190 RID: 45456
				public static LocString LOGIC_PORT = "Do not emit Radbolts";

				// Token: 0x0400B191 RID: 45457
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Emit Radbolts";

				// Token: 0x0400B192 RID: 45458
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Do not emit Radbolts";

				// Token: 0x0400B193 RID: 45459
				public static LocString LOGIC_PORT_STORAGE = "Radbolt Storage";

				// Token: 0x0400B194 RID: 45460
				public static LocString LOGIC_PORT_STORAGE_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its Radbolt Storage is full";

				// Token: 0x0400B195 RID: 45461
				public static LocString LOGIC_PORT_STORAGE_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002831 RID: 10289
			public class HEPBRIDGETILE
			{
				// Token: 0x0400B196 RID: 45462
				public static LocString NAME = UI.FormatAsLink("Radbolt Joint Plate", "HEPBRIDGETILE");

				// Token: 0x0400B197 RID: 45463
				public static LocString DESC = "Allows Radbolts to pass through walls.";

				// Token: 0x0400B198 RID: 45464
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Receives ",
					UI.FormatAsLink("Radbolts", "RADIATION"),
					" from ",
					UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER"),
					" and directs them through walls. All other materials and elements will be blocked from passage."
				});
			}

			// Token: 0x02002832 RID: 10290
			public class ASTRONAUTTRAININGCENTER
			{
				// Token: 0x0400B199 RID: 45465
				public static LocString NAME = UI.FormatAsLink("Space Cadet Centrifuge", "ASTRONAUTTRAININGCENTER");

				// Token: 0x0400B19A RID: 45466
				public static LocString DESC = "Duplicants must complete astronaut training in order to pilot space rockets.";

				// Token: 0x0400B19B RID: 45467
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Trains Duplicants to become ",
					UI.FormatAsLink("Astronaut", "ROCKETPILOTING1"),
					".\n\nDuplicants must possess the ",
					UI.FormatAsLink("Astronaut", "ROCKETPILOTING1"),
					" trait to receive training."
				});
			}

			// Token: 0x02002833 RID: 10291
			public class HOTTUB
			{
				// Token: 0x0400B19C RID: 45468
				public static LocString NAME = UI.FormatAsLink("Hot Tub", "HOTTUB");

				// Token: 0x0400B19D RID: 45469
				public static LocString DESC = "Relaxes Duplicants with massaging jets of heated liquid.";

				// Token: 0x0400B19E RID: 45470
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Requires ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					" to and from tub and ",
					UI.FormatAsLink("Power", "POWER"),
					" to run the jets.\n\nWater must be a comfortable temperature and will cool rapidly.\n\nIncreases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					" and leaves them feeling deliciously warm."
				});

				// Token: 0x0400B19F RID: 45471
				public static LocString WATER_REQUIREMENT = "{element}: {amount}";

				// Token: 0x0400B1A0 RID: 45472
				public static LocString WATER_REQUIREMENT_TOOLTIP = "This building must be filled with {amount} {element} in order to function.";

				// Token: 0x0400B1A1 RID: 45473
				public static LocString TEMPERATURE_REQUIREMENT = "Minimum {element} Temperature: {temperature}";

				// Token: 0x0400B1A2 RID: 45474
				public static LocString TEMPERATURE_REQUIREMENT_TOOLTIP = "The Hot Tub will only be usable if supplied with {temperature} {element}. If the {element} gets too cold, the Hot Tub will drain and require refilling with {element}.";
			}

			// Token: 0x02002834 RID: 10292
			public class SODAFOUNTAIN
			{
				// Token: 0x0400B1A3 RID: 45475
				public static LocString NAME = UI.FormatAsLink("Soda Fountain", "SODAFOUNTAIN");

				// Token: 0x0400B1A4 RID: 45476
				public static LocString DESC = "Sparkling water puts a twinkle in a Duplicant's eye.";

				// Token: 0x0400B1A5 RID: 45477
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Creates soda from ",
					UI.FormatAsLink("Water", "WATER"),
					" and ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					".\n\nConsuming soda water increases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			// Token: 0x02002835 RID: 10293
			public class UNCONSTRUCTEDROCKETMODULE
			{
				// Token: 0x0400B1A6 RID: 45478
				public static LocString NAME = "Empty Rocket Module";

				// Token: 0x0400B1A7 RID: 45479
				public static LocString DESC = "Something useful could be put here someday";

				// Token: 0x0400B1A8 RID: 45480
				public static LocString EFFECT = "Can be changed into a different rocket module";
			}

			// Token: 0x02002836 RID: 10294
			public class MILKFATSEPARATOR
			{
				// Token: 0x0400B1A9 RID: 45481
				public static LocString NAME = UI.FormatAsLink("Brackwax Gleaner", "MILKFATSEPARATOR");

				// Token: 0x0400B1AA RID: 45482
				public static LocString DESC = "Duplicants can slather up with brackwax to increase their travel speed in transit tubes.";

				// Token: 0x0400B1AB RID: 45483
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Refines ",
					ELEMENTS.MILK.NAME,
					" into ",
					ELEMENTS.BRINE.NAME,
					" and ",
					ELEMENTS.MILKFAT.NAME,
					", and emits ",
					ELEMENTS.CARBONDIOXIDE.NAME,
					"."
				});
			}

			// Token: 0x02002837 RID: 10295
			public class MILKFEEDER
			{
				// Token: 0x0400B1AC RID: 45484
				public static LocString NAME = UI.FormatAsLink("Critter Fountain", "MILKFEEDER");

				// Token: 0x0400B1AD RID: 45485
				public static LocString DESC = "It's easier to tolerate overcrowding when you're all hopped up on brackene.";

				// Token: 0x0400B1AE RID: 45486
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Dispenses ",
					ELEMENTS.MILK.NAME,
					" to a wide variety of ",
					UI.CODEX.CATEGORYNAMES.CREATURES,
					".\n\nAccessing the fountain significantly improves ",
					UI.CODEX.CATEGORYNAMES.CREATURES,
					"' moods."
				});
			}

			// Token: 0x02002838 RID: 10296
			public class MILKINGSTATION
			{
				// Token: 0x0400B1AF RID: 45487
				public static LocString NAME = UI.FormatAsLink("Milking Station", "MILKINGSTATION");

				// Token: 0x0400B1B0 RID: 45488
				public static LocString DESC = "The harvested liquid is basically the equivalent of soda for critters.";

				// Token: 0x0400B1B1 RID: 45489
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants with the ",
					UI.FormatAsLink("Critter Ranching II", "RANCHING2"),
					" skill to milk ",
					UI.FormatAsLink("Gassy Moos", "MOO"),
					" for ",
					ELEMENTS.MILK.NAME,
					".\n\n",
					ELEMENTS.MILK.NAME,
					" can be used to refill the ",
					BUILDINGS.PREFABS.MILKFEEDER.NAME,
					"."
				});
			}

			// Token: 0x02002839 RID: 10297
			public class MODULARLAUNCHPADPORT
			{
				// Token: 0x0400B1B2 RID: 45490
				public static LocString NAME = UI.FormatAsLink("Rocket Port", "MODULARLAUNCHPADPORTSOLID");

				// Token: 0x0400B1B3 RID: 45491
				public static LocString NAME_PLURAL = UI.FormatAsLink("Rocket Ports", "MODULARLAUNCHPADPORTSOLID");
			}

			// Token: 0x0200283A RID: 10298
			public class MODULARLAUNCHPADPORTGAS
			{
				// Token: 0x0400B1B4 RID: 45492
				public static LocString NAME = UI.FormatAsLink("Gas Rocket Port Loader", "MODULARLAUNCHPADPORTGAS");

				// Token: 0x0400B1B5 RID: 45493
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x0400B1B6 RID: 45494
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" to the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the gas filters set on the rocket's cargo bays."
				});
			}

			// Token: 0x0200283B RID: 10299
			public class MODULARLAUNCHPADPORTBRIDGE
			{
				// Token: 0x0400B1B7 RID: 45495
				public static LocString NAME = UI.FormatAsLink("Rocket Port Extension", "MODULARLAUNCHPADPORTBRIDGE");

				// Token: 0x0400B1B8 RID: 45496
				public static LocString DESC = "Allows rocket platforms to be built farther apart.";

				// Token: 0x0400B1B9 RID: 45497
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or any ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					"."
				});
			}

			// Token: 0x0200283C RID: 10300
			public class MODULARLAUNCHPADPORTLIQUID
			{
				// Token: 0x0400B1BA RID: 45498
				public static LocString NAME = UI.FormatAsLink("Liquid Rocket Port Loader", "MODULARLAUNCHPADPORTLIQUID");

				// Token: 0x0400B1BB RID: 45499
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x0400B1BC RID: 45500
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the liquid filters set on the rocket's cargo bays."
				});
			}

			// Token: 0x0200283D RID: 10301
			public class MODULARLAUNCHPADPORTSOLID
			{
				// Token: 0x0400B1BD RID: 45501
				public static LocString NAME = UI.FormatAsLink("Solid Rocket Port Loader", "MODULARLAUNCHPADPORTSOLID");

				// Token: 0x0400B1BE RID: 45502
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x0400B1BF RID: 45503
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Solids", "ELEMENTS_SOLID"),
					" to the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the solid material filters set on the rocket's cargo bays."
				});
			}

			// Token: 0x0200283E RID: 10302
			public class MODULARLAUNCHPADPORTGASUNLOADER
			{
				// Token: 0x0400B1C0 RID: 45504
				public static LocString NAME = UI.FormatAsLink("Gas Rocket Port Unloader", "MODULARLAUNCHPADPORTGASUNLOADER");

				// Token: 0x0400B1C1 RID: 45505
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x0400B1C2 RID: 45506
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" from the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the gas filters set on this unloader."
				});
			}

			// Token: 0x0200283F RID: 10303
			public class MODULARLAUNCHPADPORTLIQUIDUNLOADER
			{
				// Token: 0x0400B1C3 RID: 45507
				public static LocString NAME = UI.FormatAsLink("Liquid Rocket Port Unloader", "MODULARLAUNCHPADPORTLIQUIDUNLOADER");

				// Token: 0x0400B1C4 RID: 45508
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x0400B1C5 RID: 45509
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" from the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the liquid filters set on this unloader."
				});
			}

			// Token: 0x02002840 RID: 10304
			public class MODULARLAUNCHPADPORTSOLIDUNLOADER
			{
				// Token: 0x0400B1C6 RID: 45510
				public static LocString NAME = UI.FormatAsLink("Solid Rocket Port Unloader", "MODULARLAUNCHPADPORTSOLIDUNLOADER");

				// Token: 0x0400B1C7 RID: 45511
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x0400B1C8 RID: 45512
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Solids", "ELEMENTS_SOLID"),
					" from the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the solid material filters set on this unloader."
				});
			}

			// Token: 0x02002841 RID: 10305
			public class STICKERBOMB
			{
				// Token: 0x0400B1C9 RID: 45513
				public static LocString NAME = UI.FormatAsLink("Sticker Bomb", "STICKERBOMB");

				// Token: 0x0400B1CA RID: 45514
				public static LocString DESC = "Surprise decor sneak attacks a Duplicant's gloomy day.";
			}

			// Token: 0x02002842 RID: 10306
			public class HEATCOMPRESSOR
			{
				// Token: 0x0400B1CB RID: 45515
				public static LocString NAME = UI.FormatAsLink("Liquid Heatquilizer", "HEATCOMPRESSOR");

				// Token: 0x0400B1CC RID: 45516
				public static LocString DESC = "\"Room temperature\" is relative, really.";

				// Token: 0x0400B1CD RID: 45517
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Heats or cools ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" to match ambient ",
					UI.FormatAsLink("Air Temperature", "HEAT"),
					"."
				});
			}

			// Token: 0x02002843 RID: 10307
			public class PARTYCAKE
			{
				// Token: 0x0400B1CE RID: 45518
				public static LocString NAME = UI.FormatAsLink("Triple Decker Cake", "PARTYCAKE");

				// Token: 0x0400B1CF RID: 45519
				public static LocString DESC = "Any way you slice it, that's a good looking cake.";

				// Token: 0x0400B1D0 RID: 45520
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nAdds a ",
					UI.FormatAsLink("Morale", "MORALE"),
					" bonus to Duplicants' parties."
				});
			}

			// Token: 0x02002844 RID: 10308
			public class RAILGUN
			{
				// Token: 0x0400B1D1 RID: 45521
				public static LocString NAME = UI.FormatAsLink("Interplanetary Launcher", "RAILGUN");

				// Token: 0x0400B1D2 RID: 45522
				public static LocString DESC = "It's tempting to climb inside but trust me... don't.";

				// Token: 0x0400B1D3 RID: 45523
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Launches ",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" between Planetoids.\n\nPayloads can contain ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" materials.\n\nCannot transport Duplicants."
				});

				// Token: 0x0400B1D4 RID: 45524
				public static LocString SIDESCREEN_HEP_REQUIRED = "Launch cost: {current} / {required} radbolts";

				// Token: 0x0400B1D5 RID: 45525
				public static LocString LOGIC_PORT = "Launch Toggle";

				// Token: 0x0400B1D6 RID: 45526
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Enable payload launching.";

				// Token: 0x0400B1D7 RID: 45527
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disable payload launching.";
			}

			// Token: 0x02002845 RID: 10309
			public class RAILGUNPAYLOADOPENER
			{
				// Token: 0x0400B1D8 RID: 45528
				public static LocString NAME = UI.FormatAsLink("Payload Opener", "RAILGUNPAYLOADOPENER");

				// Token: 0x0400B1D9 RID: 45529
				public static LocString DESC = "Payload openers can be hooked up to conveyors, plumbing and ventilation for improved sorting.";

				// Token: 0x0400B1DA RID: 45530
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unpacks ",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" delivered by Duplicants.\n\nAutomatically separates ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" materials and distributes them to the appropriate systems."
				});
			}

			// Token: 0x02002846 RID: 10310
			public class LANDINGBEACON
			{
				// Token: 0x0400B1DB RID: 45531
				public static LocString NAME = UI.FormatAsLink("Targeting Beacon", "LANDINGBEACON");

				// Token: 0x0400B1DC RID: 45532
				public static LocString DESC = "Microtarget where your " + UI.FormatAsLink("Interplanetary Payload", "RAILGUNPAYLOAD") + " lands on a Planetoid surface.";

				// Token: 0x0400B1DD RID: 45533
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Guides ",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" and ",
					UI.FormatAsLink("Orbital Cargo Modules", "ORBITALCARGOMODULE"),
					" to land nearby.\n\n",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" must be launched from a ",
					UI.FormatAsLink("Interplanetary Launcher", "RAILGUN"),
					"."
				});
			}

			// Token: 0x02002847 RID: 10311
			public class DIAMONDPRESS
			{
				// Token: 0x0400B1DE RID: 45534
				public static LocString NAME = UI.FormatAsLink("Diamond Press", "DIAMONDPRESS");

				// Token: 0x0400B1DF RID: 45535
				public static LocString DESC = "Crushes refined carbon into diamond.";

				// Token: 0x0400B1E0 RID: 45536
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Power", "POWER"),
					" and ",
					UI.FormatAsLink("Radbolts", "RADIATION"),
					" to crush ",
					UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
					" into ",
					UI.FormatAsLink("Diamond", "DIAMOND"),
					".\n\nDuplicants will not fabricate items unless recipes are queued and ",
					UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
					" has been discovered."
				});

				// Token: 0x0400B1E1 RID: 45537
				public static LocString REFINED_CARBON_RECIPE_DESCRIPTION = "Converts {1} to {0}";
			}

			// Token: 0x02002848 RID: 10312
			public class ESCAPEPOD
			{
				// Token: 0x0400B1E2 RID: 45538
				public static LocString NAME = UI.FormatAsLink("Escape Pod", "ESCAPEPOD");

				// Token: 0x0400B1E3 RID: 45539
				public static LocString DESC = "Delivers a Duplicant from a stranded rocket to the nearest Planetoid.";
			}

			// Token: 0x02002849 RID: 10313
			public class ROCKETINTERIORLIQUIDOUTPUTPORT
			{
				// Token: 0x0400B1E4 RID: 45540
				public static LocString NAME = UI.FormatAsLink("Liquid Spacefarer Output Port", "ROCKETINTERIORLIQUIDOUTPUTPORT");

				// Token: 0x0400B1E5 RID: 45541
				public static LocString DESC = "A direct attachment to the input port on the exterior of a rocket.";

				// Token: 0x0400B1E6 RID: 45542
				public static LocString EFFECT = "Allows a direct conduit connection into the " + UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM") + " of a rocket.";
			}

			// Token: 0x0200284A RID: 10314
			public class ROCKETINTERIORLIQUIDINPUTPORT
			{
				// Token: 0x0400B1E7 RID: 45543
				public static LocString NAME = UI.FormatAsLink("Liquid Spacefarer Input Port", "ROCKETINTERIORLIQUIDINPUTPORT");

				// Token: 0x0400B1E8 RID: 45544
				public static LocString DESC = "A direct attachment to the output port on the exterior of a rocket.";

				// Token: 0x0400B1E9 RID: 45545
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows a direct conduit connection out of the ",
					UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM"),
					" of a rocket.\nCan be used to vent ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to space during flight."
				});
			}

			// Token: 0x0200284B RID: 10315
			public class ROCKETINTERIORGASOUTPUTPORT
			{
				// Token: 0x0400B1EA RID: 45546
				public static LocString NAME = UI.FormatAsLink("Gas Spacefarer Output Port", "ROCKETINTERIORGASOUTPUTPORT");

				// Token: 0x0400B1EB RID: 45547
				public static LocString DESC = "A direct attachment to the input port on the exterior of a rocket.";

				// Token: 0x0400B1EC RID: 45548
				public static LocString EFFECT = "Allows a direct conduit connection into the " + UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM") + " of a rocket.";
			}

			// Token: 0x0200284C RID: 10316
			public class ROCKETINTERIORGASINPUTPORT
			{
				// Token: 0x0400B1ED RID: 45549
				public static LocString NAME = UI.FormatAsLink("Gas Spacefarer Input Port", "ROCKETINTERIORGASINPUTPORT");

				// Token: 0x0400B1EE RID: 45550
				public static LocString DESC = "A direct attachment leading to the output port on the exterior of the rocket.";

				// Token: 0x0400B1EF RID: 45551
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows a direct conduit connection out of the ",
					UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM"),
					" of the rocket.\nCan be used to vent ",
					UI.FormatAsLink("Gasses", "ELEMENTS_GAS"),
					" to space during flight."
				});
			}

			// Token: 0x0200284D RID: 10317
			public class MISSILELAUNCHER
			{
				// Token: 0x0400B1F0 RID: 45552
				public static LocString NAME = UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER");

				// Token: 0x0400B1F1 RID: 45553
				public static LocString DESC = "Some meteors drop harvestable resources when they're blown to smithereens.";

				// Token: 0x0400B1F2 RID: 45554
				public static LocString EFFECT = "Fires " + UI.FormatAsLink("Blastshot", "MISSILELAUNCHER") + " shells at meteor showers to defend the colony from impact-related damage.\n\nRange: 16 tiles horizontally, 32 tiles vertically.";

				// Token: 0x0400B1F3 RID: 45555
				public static LocString TARGET_SELECTION_HEADER = "Target Selection";

				// Token: 0x0200356E RID: 13678
				public class BODY
				{
					// Token: 0x0400D7E8 RID: 55272
					public static LocString CONTAINER1 = "Fires " + UI.FormatAsLink("Blastshot", "MISSILELAUNCHER") + " shells at meteor showers to defend the colony from impact-related damage.\n\nRange: 16 tiles horizontally, 32 tiles vertically.\n\nMeteors that have been blown to smithereens leave behind no harvestable resources.";
				}
			}

			// Token: 0x0200284E RID: 10318
			public class CRITTERCONDO
			{
				// Token: 0x0400B1F4 RID: 45556
				public static LocString NAME = UI.FormatAsLink("Critter Condo", "CRITTERCONDO");

				// Token: 0x0400B1F5 RID: 45557
				public static LocString DESC = "It's nice to have nice things.";

				// Token: 0x0400B1F6 RID: 45558
				public static LocString EFFECT = "Provides a comfortable lounge area that boosts " + UI.FormatAsLink("Critter", "CREATURES") + " happiness.";
			}

			// Token: 0x0200284F RID: 10319
			public class UNDERWATERCRITTERCONDO
			{
				// Token: 0x0400B1F7 RID: 45559
				public static LocString NAME = UI.FormatAsLink("Water Fort", "UNDERWATERCRITTERCONDO");

				// Token: 0x0400B1F8 RID: 45560
				public static LocString DESC = "Even wild critters are happier after they've had a little R&R.";

				// Token: 0x0400B1F9 RID: 45561
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A fancy respite area for adult ",
					UI.FormatAsLink("Pokeshells", "CRABSPECIES"),
					" and ",
					UI.FormatAsLink("Pacu", "PACUSPECIES"),
					"."
				});
			}

			// Token: 0x02002850 RID: 10320
			public class AIRBORNECRITTERCONDO
			{
				// Token: 0x0400B1FA RID: 45562
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Condo", "AIRBORNECRITTERCONDO");

				// Token: 0x0400B1FB RID: 45563
				public static LocString DESC = "Triggers natural nesting instincts and improves critters' moods.";

				// Token: 0x0400B1FC RID: 45564
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A hanging respite area for adult ",
					UI.FormatAsLink("Pufts", "PUFT"),
					", ",
					UI.FormatAsLink("Gassy Moos", "MOOSPECIES"),
					" and ",
					UI.FormatAsLink("Shine Bugs", "LIGHTBUG"),
					"."
				});
			}

			// Token: 0x02002851 RID: 10321
			public class MASSIVEHEATSINK
			{
				// Token: 0x0400B1FD RID: 45565
				public static LocString NAME = UI.FormatAsLink("Anti Entropy Thermo-Nullifier", "MASSIVEHEATSINK");

				// Token: 0x0400B1FE RID: 45566
				public static LocString DESC = "";

				// Token: 0x0400B1FF RID: 45567
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A self-sustaining machine powered by what appears to be refined ",
					UI.FormatAsLink("Neutronium", "UNOBTANIUM"),
					".\n\nAbsorbs and neutralizes ",
					UI.FormatAsLink("Heat", "HEAT"),
					" energy when provided with piped ",
					UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
					"."
				});
			}

			// Token: 0x02002852 RID: 10322
			public class MEGABRAINTANK
			{
				// Token: 0x0400B200 RID: 45568
				public static LocString NAME = UI.FormatAsLink("Somnium Synthesizer", "MEGABRAINTANK");

				// Token: 0x0400B201 RID: 45569
				public static LocString DESC = "";

				// Token: 0x0400B202 RID: 45570
				public static LocString EFFECT = string.Concat(new string[]
				{
					"An organic multi-cortex repository and processing system fuelled by ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nAnalyzes ",
					UI.FormatAsLink("Dream Journals", "DREAMJOURNAL"),
					" produced by Duplicants wearing ",
					UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS"),
					".\n\nProvides a sustainable boost to Duplicant skills and abilities throughout the colony."
				});
			}

			// Token: 0x02002853 RID: 10323
			public class GRAVITASCREATUREMANIPULATOR
			{
				// Token: 0x0400B203 RID: 45571
				public static LocString NAME = UI.FormatAsLink("Critter Flux-O-Matic", "GRAVITASCREATUREMANIPULATOR");

				// Token: 0x0400B204 RID: 45572
				public static LocString DESC = "";

				// Token: 0x0400B205 RID: 45573
				public static LocString EFFECT = "An experimental DNA manipulator.\n\nAnalyzes " + UI.FormatAsLink("Critters", "CREATURES") + " to transform base morphs into random variants of their species.";
			}

			// Token: 0x02002854 RID: 10324
			public class FACILITYBACKWALLWINDOW
			{
				// Token: 0x0400B206 RID: 45574
				public static LocString NAME = "Window";

				// Token: 0x0400B207 RID: 45575
				public static LocString DESC = "";

				// Token: 0x0400B208 RID: 45576
				public static LocString EFFECT = "A tall, thin window.";
			}

			// Token: 0x02002855 RID: 10325
			public class POIBUNKEREXTERIORDOOR
			{
				// Token: 0x0400B209 RID: 45577
				public static LocString NAME = "Security Door";

				// Token: 0x0400B20A RID: 45578
				public static LocString EFFECT = "A strong door with a sophisticated genetic lock.";

				// Token: 0x0400B20B RID: 45579
				public static LocString DESC = "";
			}

			// Token: 0x02002856 RID: 10326
			public class POIDOORINTERNAL
			{
				// Token: 0x0400B20C RID: 45580
				public static LocString NAME = "Security Door";

				// Token: 0x0400B20D RID: 45581
				public static LocString EFFECT = "A strong door with a sophisticated genetic lock.";

				// Token: 0x0400B20E RID: 45582
				public static LocString DESC = "";
			}

			// Token: 0x02002857 RID: 10327
			public class POIFACILITYDOOR
			{
				// Token: 0x0400B20F RID: 45583
				public static LocString NAME = "Lobby Doors";

				// Token: 0x0400B210 RID: 45584
				public static LocString EFFECT = "Large double doors that were once the main entrance to a large facility.";

				// Token: 0x0400B211 RID: 45585
				public static LocString DESC = "";
			}

			// Token: 0x02002858 RID: 10328
			public class POIDLC2SHOWROOMDOOR
			{
				// Token: 0x0400B212 RID: 45586
				public static LocString NAME = "Showroom Doors";

				// Token: 0x0400B213 RID: 45587
				public static LocString EFFECT = "Large double doors identical to those you might find at the main entrance to a large facility.";

				// Token: 0x0400B214 RID: 45588
				public static LocString DESC = "";
			}

			// Token: 0x02002859 RID: 10329
			public class VENDINGMACHINE
			{
				// Token: 0x0400B215 RID: 45589
				public static LocString NAME = "Vending Machine";

				// Token: 0x0400B216 RID: 45590
				public static LocString DESC = "A pristine " + UI.FormatAsLink("Nutrient Bar", "FIELDRATION") + " dispenser.";
			}

			// Token: 0x0200285A RID: 10330
			public class GENESHUFFLER
			{
				// Token: 0x0400B217 RID: 45591
				public static LocString NAME = "Neural Vacillator";

				// Token: 0x0400B218 RID: 45592
				public static LocString DESC = "A massive synthetic brain, suspended in saline solution.\n\nThere is a chair attached to the device with room for one person.";
			}

			// Token: 0x0200285B RID: 10331
			public class PROPTALLPLANT
			{
				// Token: 0x0400B219 RID: 45593
				public static LocString NAME = "Potted Plant";

				// Token: 0x0400B21A RID: 45594
				public static LocString DESC = "Looking closely, it appears to be fake.";
			}

			// Token: 0x0200285C RID: 10332
			public class PROPTABLE
			{
				// Token: 0x0400B21B RID: 45595
				public static LocString NAME = "Table";

				// Token: 0x0400B21C RID: 45596
				public static LocString DESC = "A table and some chairs.";
			}

			// Token: 0x0200285D RID: 10333
			public class PROPDESK
			{
				// Token: 0x0400B21D RID: 45597
				public static LocString NAME = "Computer Desk";

				// Token: 0x0400B21E RID: 45598
				public static LocString DESC = "An intact office desk, decorated with several personal belongings and a barely functioning computer.";
			}

			// Token: 0x0200285E RID: 10334
			public class PROPFACILITYCHAIR
			{
				// Token: 0x0400B21F RID: 45599
				public static LocString NAME = "Lobby Chair";

				// Token: 0x0400B220 RID: 45600
				public static LocString DESC = "A chair where visitors can comfortably wait before their appointments.";
			}

			// Token: 0x0200285F RID: 10335
			public class PROPFACILITYCOUCH
			{
				// Token: 0x0400B221 RID: 45601
				public static LocString NAME = "Lobby Couch";

				// Token: 0x0400B222 RID: 45602
				public static LocString DESC = "A couch where visitors can comfortably wait before their appointments.";
			}

			// Token: 0x02002860 RID: 10336
			public class PROPFACILITYDESK
			{
				// Token: 0x0400B223 RID: 45603
				public static LocString NAME = "Director's Desk";

				// Token: 0x0400B224 RID: 45604
				public static LocString DESC = "A spotless desk filled with impeccably organized office supplies.\n\nA photo peeks out from beneath the desk pad, depicting two beaming young women in caps and gowns.\n\nThe photo is quite old.";
			}

			// Token: 0x02002861 RID: 10337
			public class PROPFACILITYTABLE
			{
				// Token: 0x0400B225 RID: 45605
				public static LocString NAME = "Coffee Table";

				// Token: 0x0400B226 RID: 45606
				public static LocString DESC = "A low coffee table that may have once held old science magazines.";
			}

			// Token: 0x02002862 RID: 10338
			public class PROPFACILITYSTATUE
			{
				// Token: 0x0400B227 RID: 45607
				public static LocString NAME = "Gravitas Monument";

				// Token: 0x0400B228 RID: 45608
				public static LocString DESC = "A large, modern sculpture that sits in the center of the lobby.\n\nIt's an artistic cross between an hourglass shape and a double helix.";
			}

			// Token: 0x02002863 RID: 10339
			public class PROPFACILITYCHANDELIER
			{
				// Token: 0x0400B229 RID: 45609
				public static LocString NAME = "Chandelier";

				// Token: 0x0400B22A RID: 45610
				public static LocString DESC = "A large chandelier that hangs from the ceiling.\n\nIt does not appear to function.";
			}

			// Token: 0x02002864 RID: 10340
			public class PROPFACILITYGLOBEDROORS
			{
				// Token: 0x0400B22B RID: 45611
				public static LocString NAME = "Filing Cabinet";

				// Token: 0x0400B22C RID: 45612
				public static LocString DESC = "A filing cabinet for storing hard copy employee records.\n\nThe contents have been shredded.";
			}

			// Token: 0x02002865 RID: 10341
			public class PROPFACILITYDISPLAY1
			{
				// Token: 0x0400B22D RID: 45613
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400B22E RID: 45614
				public static LocString DESC = "An electronic display projecting the blueprint of a familiar device.\n\nIt looks like a Printing Pod.";
			}

			// Token: 0x02002866 RID: 10342
			public class PROPFACILITYDISPLAY2
			{
				// Token: 0x0400B22F RID: 45615
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400B230 RID: 45616
				public static LocString DESC = "An electronic display projecting the blueprint of a familiar device.\n\nIt looks like a Mining Gun.";
			}

			// Token: 0x02002867 RID: 10343
			public class PROPFACILITYDISPLAY3
			{
				// Token: 0x0400B231 RID: 45617
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400B232 RID: 45618
				public static LocString DESC = "An electronic display projecting the blueprint of a strange device.\n\nPerhaps these displays were used to entice visitors.";
			}

			// Token: 0x02002868 RID: 10344
			public class PROPFACILITYTALLPLANT
			{
				// Token: 0x0400B233 RID: 45619
				public static LocString NAME = "Office Plant";

				// Token: 0x0400B234 RID: 45620
				public static LocString DESC = "It's survived the vacuum of space by virtue of being plastic.";
			}

			// Token: 0x02002869 RID: 10345
			public class PROPFACILITYLAMP
			{
				// Token: 0x0400B235 RID: 45621
				public static LocString NAME = "Light Fixture";

				// Token: 0x0400B236 RID: 45622
				public static LocString DESC = "A long light fixture that hangs from the ceiling.\n\nIt does not appear to function.";
			}

			// Token: 0x0200286A RID: 10346
			public class PROPFACILITYWALLDEGREE
			{
				// Token: 0x0400B237 RID: 45623
				public static LocString NAME = "Doctorate Degree";

				// Token: 0x0400B238 RID: 45624
				public static LocString DESC = "Certification in Applied Physics, awarded in recognition of one \"Jacquelyn A. Stern\".";
			}

			// Token: 0x0200286B RID: 10347
			public class PROPFACILITYPAINTING
			{
				// Token: 0x0400B239 RID: 45625
				public static LocString NAME = "Landscape Portrait";

				// Token: 0x0400B23A RID: 45626
				public static LocString DESC = "A painting featuring a copse of fir trees and a magnificent mountain range on the horizon.\n\nThe air in the room prickles with the sensation that I'm not meant to be here.";
			}

			// Token: 0x0200286C RID: 10348
			public class PROPRECEPTIONDESK
			{
				// Token: 0x0400B23B RID: 45627
				public static LocString NAME = "Reception Desk";

				// Token: 0x0400B23C RID: 45628
				public static LocString DESC = "A full coffee cup and a note abandoned mid sentence sit behind the desk.\n\nIt gives me an eerie feeling, as if the receptionist has stepped out and will return any moment.";
			}

			// Token: 0x0200286D RID: 10349
			public class PROPELEVATOR
			{
				// Token: 0x0400B23D RID: 45629
				public static LocString NAME = "Broken Elevator";

				// Token: 0x0400B23E RID: 45630
				public static LocString DESC = "Out of service.\n\nThe buttons inside indicate it went down more than a dozen floors at one point in time.";
			}

			// Token: 0x0200286E RID: 10350
			public class SETLOCKER
			{
				// Token: 0x0400B23F RID: 45631
				public static LocString NAME = "Locker";

				// Token: 0x0400B240 RID: 45632
				public static LocString DESC = "A basic metal locker.\n\nIt contains an assortment of personal effects.";
			}

			// Token: 0x0200286F RID: 10351
			public class PROPEXOSETLOCKER
			{
				// Token: 0x0400B241 RID: 45633
				public static LocString NAME = "Off-site Locker";

				// Token: 0x0400B242 RID: 45634
				public static LocString DESC = "A locker made with ultra-lightweight textiles.\n\nIt contains an assortment of personal effects.";
			}

			// Token: 0x02002870 RID: 10352
			public class PROPGRAVITASSMALLSEEDLOCKER
			{
				// Token: 0x0400B243 RID: 45635
				public static LocString NAME = "Wall Cabinet";

				// Token: 0x0400B244 RID: 45636
				public static LocString DESC = "A small glass cabinet.\n\nThere's a biohazard symbol on it.";
			}

			// Token: 0x02002871 RID: 10353
			public class PROPLIGHT
			{
				// Token: 0x0400B245 RID: 45637
				public static LocString NAME = "Light Fixture";

				// Token: 0x0400B246 RID: 45638
				public static LocString DESC = "An elegant ceiling lamp, slightly worse for wear.";
			}

			// Token: 0x02002872 RID: 10354
			public class PROPLADDER
			{
				// Token: 0x0400B247 RID: 45639
				public static LocString NAME = "Ladder";

				// Token: 0x0400B248 RID: 45640
				public static LocString DESC = "A hard plastic ladder.";
			}

			// Token: 0x02002873 RID: 10355
			public class PROPSKELETON
			{
				// Token: 0x0400B249 RID: 45641
				public static LocString NAME = "Model Skeleton";

				// Token: 0x0400B24A RID: 45642
				public static LocString DESC = "A detailed anatomical model.\n\nIt appears to be made of resin.";
			}

			// Token: 0x02002874 RID: 10356
			public class PROPSURFACESATELLITE1
			{
				// Token: 0x0400B24B RID: 45643
				public static LocString NAME = "Crashed Satellite";

				// Token: 0x0400B24C RID: 45644
				public static LocString DESC = "All that remains of a once peacefully orbiting satellite.";
			}

			// Token: 0x02002875 RID: 10357
			public class PROPSURFACESATELLITE2
			{
				// Token: 0x0400B24D RID: 45645
				public static LocString NAME = "Wrecked Satellite";

				// Token: 0x0400B24E RID: 45646
				public static LocString DESC = "All that remains of a once peacefully orbiting satellite.";
			}

			// Token: 0x02002876 RID: 10358
			public class PROPSURFACESATELLITE3
			{
				// Token: 0x0400B24F RID: 45647
				public static LocString NAME = "Crushed Satellite";

				// Token: 0x0400B250 RID: 45648
				public static LocString DESC = "All that remains of a once peacefully orbiting satellite.";
			}

			// Token: 0x02002877 RID: 10359
			public class PROPCLOCK
			{
				// Token: 0x0400B251 RID: 45649
				public static LocString NAME = "Clock";

				// Token: 0x0400B252 RID: 45650
				public static LocString DESC = "A simple wall clock.\n\nIt is no longer ticking.";
			}

			// Token: 0x02002878 RID: 10360
			public class PROPGRAVITASDECORATIVEWINDOW
			{
				// Token: 0x0400B253 RID: 45651
				public static LocString NAME = "Window";

				// Token: 0x0400B254 RID: 45652
				public static LocString DESC = "A tall, thin window which once pointed to a courtyard.";
			}

			// Token: 0x02002879 RID: 10361
			public class PROPGRAVITASLABWINDOW
			{
				// Token: 0x0400B255 RID: 45653
				public static LocString NAME = "Lab Window";

				// Token: 0x0400B256 RID: 45654
				public static LocString DESC = "";

				// Token: 0x0400B257 RID: 45655
				public static LocString EFFECT = "A lab window. Formerly a portal to the outside world.";
			}

			// Token: 0x0200287A RID: 10362
			public class PROPGRAVITASLABWINDOWHORIZONTAL
			{
				// Token: 0x0400B258 RID: 45656
				public static LocString NAME = "Lab Window";

				// Token: 0x0400B259 RID: 45657
				public static LocString DESC = "";

				// Token: 0x0400B25A RID: 45658
				public static LocString EFFECT = "A lab window.\n\nSomeone once stared out of this, contemplating the results of an experiment.";
			}

			// Token: 0x0200287B RID: 10363
			public class PROPGRAVITASLABWALL
			{
				// Token: 0x0400B25B RID: 45659
				public static LocString NAME = "Lab Wall";

				// Token: 0x0400B25C RID: 45660
				public static LocString DESC = "";

				// Token: 0x0400B25D RID: 45661
				public static LocString EFFECT = "A regular wall that once existed in a working lab.";
			}

			// Token: 0x0200287C RID: 10364
			public class GRAVITASCONTAINER
			{
				// Token: 0x0400B25E RID: 45662
				public static LocString NAME = "Pajama Cubby";

				// Token: 0x0400B25F RID: 45663
				public static LocString DESC = "";

				// Token: 0x0400B260 RID: 45664
				public static LocString EFFECT = "A clothing storage unit.\n\nIt contains ultra-soft sleepwear.";
			}

			// Token: 0x0200287D RID: 10365
			public class GRAVITASLABLIGHT
			{
				// Token: 0x0400B261 RID: 45665
				public static LocString NAME = "LED Light";

				// Token: 0x0400B262 RID: 45666
				public static LocString DESC = "";

				// Token: 0x0400B263 RID: 45667
				public static LocString EFFECT = "An overhead light therapy lamp designed to soothe the minds.";
			}

			// Token: 0x0200287E RID: 10366
			public class GRAVITASDOOR
			{
				// Token: 0x0400B264 RID: 45668
				public static LocString NAME = "Gravitas Door";

				// Token: 0x0400B265 RID: 45669
				public static LocString DESC = "";

				// Token: 0x0400B266 RID: 45670
				public static LocString EFFECT = "An office door to an office that no longer exists.";
			}

			// Token: 0x0200287F RID: 10367
			public class PROPGRAVITASWALL
			{
				// Token: 0x0400B267 RID: 45671
				public static LocString NAME = "Wall";

				// Token: 0x0400B268 RID: 45672
				public static LocString DESC = "";

				// Token: 0x0400B269 RID: 45673
				public static LocString EFFECT = "The wall of a once-great scientific facility.";
			}

			// Token: 0x02002880 RID: 10368
			public class PROPGRAVITASWALLPURPLE
			{
				// Token: 0x0400B26A RID: 45674
				public static LocString NAME = "Wall";

				// Token: 0x0400B26B RID: 45675
				public static LocString DESC = "";

				// Token: 0x0400B26C RID: 45676
				public static LocString EFFECT = "The wall of an ambitious research and development department.";
			}

			// Token: 0x02002881 RID: 10369
			public class PROPGRAVITASWALLPURPLEWHITEDIAGONAL
			{
				// Token: 0x0400B26D RID: 45677
				public static LocString NAME = "Wall";

				// Token: 0x0400B26E RID: 45678
				public static LocString DESC = "";

				// Token: 0x0400B26F RID: 45679
				public static LocString EFFECT = "The wall of an ambitious research and development department.";
			}

			// Token: 0x02002882 RID: 10370
			public class PROPGRAVITASDISPLAY4
			{
				// Token: 0x0400B270 RID: 45680
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400B271 RID: 45681
				public static LocString DESC = "An electronic display projecting the blueprint of a robotic device.\n\nIt looks like a ceiling robot.";
			}

			// Token: 0x02002883 RID: 10371
			public class PROPDLC2DISPLAY1
			{
				// Token: 0x0400B272 RID: 45682
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400B273 RID: 45683
				public static LocString DESC = "An electronic display projecting the blueprint of an engineering project.\n\nIt looks like a pump of some kind.";
			}

			// Token: 0x02002884 RID: 10372
			public class PROPGRAVITASCEILINGROBOT
			{
				// Token: 0x0400B274 RID: 45684
				public static LocString NAME = "Ceiling Robot";

				// Token: 0x0400B275 RID: 45685
				public static LocString DESC = "Non-functioning robotic arms that once assisted lab technicians.";
			}

			// Token: 0x02002885 RID: 10373
			public class PROPGRAVITASFLOORROBOT
			{
				// Token: 0x0400B276 RID: 45686
				public static LocString NAME = "Robotic Arm";

				// Token: 0x0400B277 RID: 45687
				public static LocString DESC = "The grasping robotic claw designed to assist technicians in a lab.";
			}

			// Token: 0x02002886 RID: 10374
			public class PROPGRAVITASJAR1
			{
				// Token: 0x0400B278 RID: 45688
				public static LocString NAME = "Big Brain Jar";

				// Token: 0x0400B279 RID: 45689
				public static LocString DESC = "An abnormally large brain floating in embalming liquid to prevent decomposition.";
			}

			// Token: 0x02002887 RID: 10375
			public class PROPGRAVITASCREATUREPOSTER
			{
				// Token: 0x0400B27A RID: 45690
				public static LocString NAME = "Anatomy Poster";

				// Token: 0x0400B27B RID: 45691
				public static LocString DESC = "An anatomical illustration of the very first " + UI.FormatAsLink("Hatch", "HATCH") + " ever produced.\n\nWhile the ratio of egg sac to brain may appear outlandish, it is in fact to scale.";
			}

			// Token: 0x02002888 RID: 10376
			public class PROPGRAVITASDESKPODIUM
			{
				// Token: 0x0400B27C RID: 45692
				public static LocString NAME = "Computer Podium";

				// Token: 0x0400B27D RID: 45693
				public static LocString DESC = "A clutter-proof desk to minimize distractions.\n\nThere appears to be something stored in the computer.";
			}

			// Token: 0x02002889 RID: 10377
			public class PROPGRAVITASFIRSTAIDKIT
			{
				// Token: 0x0400B27E RID: 45694
				public static LocString NAME = "First Aid Kit";

				// Token: 0x0400B27F RID: 45695
				public static LocString DESC = "It looks like it's been used a lot.";
			}

			// Token: 0x0200288A RID: 10378
			public class PROPGRAVITASHANDSCANNER
			{
				// Token: 0x0400B280 RID: 45696
				public static LocString NAME = "Hand Scanner";

				// Token: 0x0400B281 RID: 45697
				public static LocString DESC = "A sophisticated security device.\n\nIt appears to use a method other than fingerprints to verify an individual's identity.";
			}

			// Token: 0x0200288B RID: 10379
			public class PROPGRAVITASLABTABLE
			{
				// Token: 0x0400B282 RID: 45698
				public static LocString NAME = "Lab Desk";

				// Token: 0x0400B283 RID: 45699
				public static LocString DESC = "The quaint research desk of a departed lab technician.\n\nPerhaps the computer stores something of interest.";
			}

			// Token: 0x0200288C RID: 10380
			public class PROPGRAVITASROBTICTABLE
			{
				// Token: 0x0400B284 RID: 45700
				public static LocString NAME = "Robotics Research Desk";

				// Token: 0x0400B285 RID: 45701
				public static LocString DESC = "The work space of an extinct robotics technician who left behind some unfinished prototypes.";
			}

			// Token: 0x0200288D RID: 10381
			public class PROPDLC2GEOTHERMALCART
			{
				// Token: 0x0400B286 RID: 45702
				public static LocString NAME = "Service Cart";

				// Token: 0x0400B287 RID: 45703
				public static LocString DESC = "Maintenance equipment that once flushed debris out of complex mechanisms.\n\nOne of the wheels is squeaky.";
			}

			// Token: 0x0200288E RID: 10382
			public class PROPGRAVITASSHELF
			{
				// Token: 0x0400B288 RID: 45704
				public static LocString NAME = "Shelf";

				// Token: 0x0400B289 RID: 45705
				public static LocString DESC = "A shelf holding jars just out of reach for a short person.";
			}

			// Token: 0x0200288F RID: 10383
			public class PROPGRAVITASTOOLSHELF
			{
				// Token: 0x0400B28A RID: 45706
				public static LocString NAME = "Tool Rack";

				// Token: 0x0400B28B RID: 45707
				public static LocString DESC = "A wall-mounted rack for storing and displaying useful tools at a not-so-useful height.";
			}

			// Token: 0x02002890 RID: 10384
			public class PROPGRAVITASTOOLCRATE
			{
				// Token: 0x0400B28C RID: 45708
				public static LocString NAME = "Tool Crate";

				// Token: 0x0400B28D RID: 45709
				public static LocString DESC = "A packing crate intended for safety equipment.\n\nIt has been repurposed for tool storage.";
			}

			// Token: 0x02002891 RID: 10385
			public class PROPGRAVITASFIREEXTINGUISHER
			{
				// Token: 0x0400B28E RID: 45710
				public static LocString NAME = "Broken Fire Extinguisher";

				// Token: 0x0400B28F RID: 45711
				public static LocString DESC = "Essential lab equipment.\n\nThe inspection tag indicates it has long expired.";
			}

			// Token: 0x02002892 RID: 10386
			public class PROPGRAVITASJAR2
			{
				// Token: 0x0400B290 RID: 45712
				public static LocString NAME = "Sample Jar";

				// Token: 0x0400B291 RID: 45713
				public static LocString DESC = "The corpse of a proto-hatch creature meticulously preserved in a jar.";
			}

			// Token: 0x02002893 RID: 10387
			public class PROPEXOSHELFLONG
			{
				// Token: 0x0400B292 RID: 45714
				public static LocString NAME = "Long Prefab Shelf";

				// Token: 0x0400B293 RID: 45715
				public static LocString DESC = "A shelf made out of flat-packed pieces that can be assembled in various ways.\n\nThis is the long way.";
			}

			// Token: 0x02002894 RID: 10388
			public class PROPEXOSHELSHORT
			{
				// Token: 0x0400B294 RID: 45716
				public static LocString NAME = "Prefab Shelf";

				// Token: 0x0400B295 RID: 45717
				public static LocString DESC = "A shelf made out of flat-packed pieces that can be assembled in various ways.\n\nIt looks nice, actually.";
			}

			// Token: 0x02002895 RID: 10389
			public class PROPHUMANMURPHYBED
			{
				// Token: 0x0400B296 RID: 45718
				public static LocString NAME = "Murphy Bed";

				// Token: 0x0400B297 RID: 45719
				public static LocString DESC = "A bed that folds into the wall, for small live/work spaces.\n\nThis is the display model.";
			}

			// Token: 0x02002896 RID: 10390
			public class PROPHUMANCHESTERFIELDSOFA
			{
				// Token: 0x0400B298 RID: 45720
				public static LocString NAME = "Showroom Couch";

				// Token: 0x0400B299 RID: 45721
				public static LocString DESC = "A luxurious couch where potential residents can comfortably nap and dream of home.";
			}

			// Token: 0x02002897 RID: 10391
			public class PROPHUMANCHESTERFIELDCHAIR
			{
				// Token: 0x0400B29A RID: 45722
				public static LocString NAME = "Showroom Chair";

				// Token: 0x0400B29B RID: 45723
				public static LocString DESC = "A luxurious chair where future generations can comfortably sit and dream of home.";
			}

			// Token: 0x02002898 RID: 10392
			public class WARPCONDUITRECEIVER
			{
				// Token: 0x0400B29C RID: 45724
				public static LocString NAME = "Supply Teleporter Output";

				// Token: 0x0400B29D RID: 45725
				public static LocString DESC = "The tubes at the back disappear into nowhere.";

				// Token: 0x0400B29E RID: 45726
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A machine capable of teleporting ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources to another asteroid.\n\nIt can be activated by a Duplicant with the ",
					UI.FormatAsLink("Field Research", "RESEARCHING2"),
					" skill.\n\nThis is the receiving side."
				});
			}

			// Token: 0x02002899 RID: 10393
			public class WARPCONDUITSENDER
			{
				// Token: 0x0400B29F RID: 45727
				public static LocString NAME = "Supply Teleporter Input";

				// Token: 0x0400B2A0 RID: 45728
				public static LocString DESC = "The tubes at the back disappear into nowhere.";

				// Token: 0x0400B2A1 RID: 45729
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A machine capable of teleporting ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources to another asteroid.\n\nIt can be activated by a Duplicant with the ",
					UI.FormatAsLink("Field Research", "RESEARCHING2"),
					" skill.\n\nThis is the transmitting side."
				});
			}

			// Token: 0x0200289A RID: 10394
			public class WARPPORTAL
			{
				// Token: 0x0400B2A2 RID: 45730
				public static LocString NAME = "Teleporter Transmitter";

				// Token: 0x0400B2A3 RID: 45731
				public static LocString DESC = "The functional remnants of an intricate teleportation system.\n\nThis is the outgoing side, and has one pre-programmed destination.";
			}

			// Token: 0x0200289B RID: 10395
			public class WARPRECEIVER
			{
				// Token: 0x0400B2A4 RID: 45732
				public static LocString NAME = "Teleporter Receiver";

				// Token: 0x0400B2A5 RID: 45733
				public static LocString DESC = "The functional remnants of an intricate teleportation system.\n\nThis is the incoming side.";
			}

			// Token: 0x0200289C RID: 10396
			public class TEMPORALTEAROPENER
			{
				// Token: 0x0400B2A6 RID: 45734
				public static LocString NAME = "Temporal Tear Opener";

				// Token: 0x0400B2A7 RID: 45735
				public static LocString DESC = "Infinite possibilities, with a complimentary side of meteor showers.";

				// Token: 0x0400B2A8 RID: 45736
				public static LocString EFFECT = "A powerful mechanism capable of tearing through the fabric of reality.";

				// Token: 0x0200356F RID: 13679
				public class SIDESCREEN
				{
					// Token: 0x0400D7E9 RID: 55273
					public static LocString TEXT = "Fire!";

					// Token: 0x0400D7EA RID: 55274
					public static LocString TOOLTIP = "The big red button.";
				}
			}

			// Token: 0x0200289D RID: 10397
			public class LONELYMINIONHOUSE
			{
				// Token: 0x0400B2A9 RID: 45737
				public static LocString NAME = UI.FormatAsLink("Gravitas Shipping Container", "LONELYMINIONHOUSE");

				// Token: 0x0400B2AA RID: 45738
				public static LocString DESC = "Its occupant has been alone for so long, he's forgotten what friendship feels like.";

				// Token: 0x0400B2AB RID: 45739
				public static LocString EFFECT = "A large transport unit from the facility's sub-sub-basement.\n\nIt has been modified into a crude yet functional temporary shelter.";
			}

			// Token: 0x0200289E RID: 10398
			public class LONELYMINIONHOUSE_COMPLETE
			{
				// Token: 0x0400B2AC RID: 45740
				public static LocString NAME = UI.FormatAsLink("Gravitas Shipping Container", "LONELYMINIONHOUSE_COMPLETE");

				// Token: 0x0400B2AD RID: 45741
				public static LocString DESC = "Someone lived inside it for a while.";

				// Token: 0x0400B2AE RID: 45742
				public static LocString EFFECT = "A super-spacious container for the " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " of your choosing.";
			}

			// Token: 0x0200289F RID: 10399
			public class LONELYMAILBOX
			{
				// Token: 0x0400B2AF RID: 45743
				public static LocString NAME = "Mailbox";

				// Token: 0x0400B2B0 RID: 45744
				public static LocString DESC = "There's nothing quite like receiving homemade gifts in the mail.";

				// Token: 0x0400B2B1 RID: 45745
				public static LocString EFFECT = "Displays a single edible object.";
			}

			// Token: 0x020028A0 RID: 10400
			public class PLASTICFLOWERS
			{
				// Token: 0x0400B2B2 RID: 45746
				public static LocString NAME = "Plastic Flowers";

				// Token: 0x0400B2B3 RID: 45747
				public static LocString DESCRIPTION = "Maintenance-free blooms that will outlive us all.";

				// Token: 0x0400B2B4 RID: 45748
				public static LocString LORE_DLC2 = "Manufactured by Home Staging Heroes Ltd. as commissioned by the Gravitas Facility, to <i>\"Make Space Feel More Like Home.\"</i>\n\nThis bouquet is designed to smell like freshly baked cookies.";
			}

			// Token: 0x020028A1 RID: 10401
			public class FOUNTAINPEN
			{
				// Token: 0x0400B2B5 RID: 45749
				public static LocString NAME = "Fountain Pen";

				// Token: 0x0400B2B6 RID: 45750
				public static LocString DESCRIPTION = "Cuts through red tape better than a sword ever could.";

				// Token: 0x0400B2B7 RID: 45751
				public static LocString LORE_DLC2 = "The handcrafted gold nib features a triangular logo with the letters V and I inside.\n\nIts owner was too proud to report it stolen, and would be shocked to learn of its whereabouts.";
			}

			// Token: 0x020028A2 RID: 10402
			public class PROPCLOTHESHANGER
			{
				// Token: 0x0400B2B8 RID: 45752
				public static LocString NAME = "Coat Rack";

				// Token: 0x0400B2B9 RID: 45753
				public static LocString DESC = "Holds one " + EQUIPMENT.PREFABS.WARM_VEST.NAME + ".\n\nIt'd be silly not to use it.";
			}

			// Token: 0x020028A3 RID: 10403
			public class PROPCERESPOSTERA
			{
				// Token: 0x0400B2BA RID: 45754
				public static LocString NAME = "Travel Poster";

				// Token: 0x0400B2BB RID: 45755
				public static LocString DESC = "A poster promoting a local tourist attraction.\n\nActual scenery may vary.";
			}

			// Token: 0x020028A4 RID: 10404
			public class PROPCERESPOSTERB
			{
				// Token: 0x0400B2BC RID: 45756
				public static LocString NAME = "Travel Poster";

				// Token: 0x0400B2BD RID: 45757
				public static LocString DESC = "A poster promoting local wildlife.\n\nThe first in an unfinished series.";
			}

			// Token: 0x020028A5 RID: 10405
			public class PROPCERESPOSTERLARGE
			{
				// Token: 0x0400B2BE RID: 45758
				public static LocString NAME = "Acoustic Art Panel";

				// Token: 0x0400B2BF RID: 45759
				public static LocString DESC = "A sound-absorbing panel that makes small-space living more bearable.\n\nThe artwork features a  power source.";
			}

			// Token: 0x020028A6 RID: 10406
			public class CHLORINATOR
			{
				// Token: 0x0400B2C0 RID: 45760
				public static LocString NAME = UI.FormatAsLink("Bleach Stone Hopper", "CHLORINATOR");

				// Token: 0x0400B2C1 RID: 45761
				public static LocString DESC = "Bleach stone is useful for sanitation and geotuning.";

				// Token: 0x0400B2C2 RID: 45762
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					ELEMENTS.SALT.NAME,
					" and ",
					ELEMENTS.GOLD.NAME,
					" to produce ",
					ELEMENTS.BLEACHSTONE.NAME,
					"."
				});
			}

			// Token: 0x020028A7 RID: 10407
			public class MILKPRESS
			{
				// Token: 0x0400B2C3 RID: 45763
				public static LocString NAME = UI.FormatAsLink("Plant Pulverizer", "MILKPRESS");

				// Token: 0x0400B2C4 RID: 45764
				public static LocString DESC = "For Duplicants who are too squeamish to milk critters.";

				// Token: 0x0400B2C5 RID: 45765
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Crushes ",
					CREATURES.SPECIES.SEEDS.COLDWHEAT.NAME,
					" or ",
					ITEMS.FOOD.SPICENUT.NAME,
					" to extract ",
					ELEMENTS.MILK.NAME,
					".\n\n",
					ELEMENTS.MILK.NAME,
					" can be used to refill the ",
					BUILDINGS.PREFABS.MILKFEEDER.NAME,
					"."
				});

				// Token: 0x0400B2C6 RID: 45766
				public static LocString WHEAT_MILK_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				// Token: 0x0400B2C7 RID: 45767
				public static LocString NUT_MILK_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				// Token: 0x0400B2C8 RID: 45768
				public static LocString PHYTO_OIL_RECIPE_DESCRIPTION = "Converts {0} to {1} and {2}";
			}

			// Token: 0x020028A8 RID: 10408
			public class FOODDEHYDRATOR
			{
				// Token: 0x0400B2C9 RID: 45769
				public static LocString NAME = UI.FormatAsLink("Dehydrator", "FOODDEHYDRATOR");

				// Token: 0x0400B2CA RID: 45770
				public static LocString DESC = "Some of the eliminated liquid inevitably ends up on the floor.";

				// Token: 0x0400B2CB RID: 45771
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses low, even heat to eliminate moisture from eligible ",
					UI.FormatAsLink("Foods", "FOOD"),
					" and render them shelf-stable.\n\nDehydrated meals must be processed at the ",
					UI.FormatAsLink("Rehydrator", "FOODREHYDRATOR"),
					" before they can be eaten."
				});

				// Token: 0x0400B2CC RID: 45772
				public static LocString RECIPE_NAME = "Dried {0}";

				// Token: 0x0400B2CD RID: 45773
				public static LocString RESULT_DESCRIPTION = "Dehydrated portions of {0} do not require refrigeration.";
			}

			// Token: 0x020028A9 RID: 10409
			public class FOODREHYDRATOR
			{
				// Token: 0x0400B2CE RID: 45774
				public static LocString NAME = UI.FormatAsLink("Rehydrator", "FOODREHYDRATOR");

				// Token: 0x0400B2CF RID: 45775
				public static LocString DESC = "Rehydrated food is nutritious and only slightly less delicious.";

				// Token: 0x0400B2D0 RID: 45776
				public static LocString EFFECT = "Restores moisture to convert shelf-stable packaged meals into edible " + UI.FormatAsLink("Food", "FOOD") + ".";
			}

			// Token: 0x020028AA RID: 10410
			public class GEOTHERMALCONTROLLER
			{
				// Token: 0x0400B2D1 RID: 45777
				public static LocString NAME = UI.FormatAsLink("Geothermal Heat Pump", "GEOTHERMALCONTROLLER");

				// Token: 0x0400B2D2 RID: 45778
				public static LocString DESC = "What comes out depends very much on the initial temperature of what goes in.";

				// Token: 0x0400B2D3 RID: 45779
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Heat", "HEAT"),
					" from the planet's core to dramatically increase the temperature of ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" inputs.\n\nMaterials will be emitted at connected Geo Vents."
				});
			}

			// Token: 0x020028AB RID: 10411
			public class GEOTHERMALVENT
			{
				// Token: 0x0400B2D4 RID: 45780
				public static LocString NAME = UI.FormatAsLink("Geo Vent", "GEOTHERMALVENT");

				// Token: 0x0400B2D5 RID: 45781
				public static LocString NAME_FMT = UI.FormatAsLink("Geo Vent C-{ID}", "GEOTHERMALVENT");

				// Token: 0x0400B2D6 RID: 45782
				public static LocString DESC = "Geo vents must finish their current emission before accepting new materials.";

				// Token: 0x0400B2D7 RID: 45783
				public static LocString EFFECT = "Emits high-" + UI.FormatAsLink("temperature", "HEAT") + " materials received from the Geothermal Heat Pump.";

				// Token: 0x0400B2D8 RID: 45784
				public static LocString BLOCKED_DESC = string.Concat(new string[]
				{
					"Blocked geo vents can be cleared by pumping in ",
					UI.FormatAsLink("liquids", "ELEMENTS_LIQUID"),
					" that are hot enough to melt ",
					UI.FormatAsLink("Lead", "LEAD"),
					"."
				});

				// Token: 0x0400B2D9 RID: 45785
				public static LocString LOGIC_PORT = "Material Content Monitor";

				// Token: 0x0400B2DA RID: 45786
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when geo vent has materials to emit";

				// Token: 0x0400B2DB RID: 45787
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}
		}

		// Token: 0x02002013 RID: 8211
		public static class DAMAGESOURCES
		{
			// Token: 0x04009201 RID: 37377
			public static LocString NOTIFICATION_TOOLTIP = "A {0} sustained damage from {1}";

			// Token: 0x04009202 RID: 37378
			public static LocString CONDUIT_CONTENTS_FROZE = "pipe contents becoming too cold";

			// Token: 0x04009203 RID: 37379
			public static LocString CONDUIT_CONTENTS_BOILED = "pipe contents becoming too hot";

			// Token: 0x04009204 RID: 37380
			public static LocString BUILDING_OVERHEATED = "overheating";

			// Token: 0x04009205 RID: 37381
			public static LocString CORROSIVE_ELEMENT = "corrosive element";

			// Token: 0x04009206 RID: 37382
			public static LocString BAD_INPUT_ELEMENT = "receiving an incorrect substance";

			// Token: 0x04009207 RID: 37383
			public static LocString MINION_DESTRUCTION = "an angry Duplicant. Rude!";

			// Token: 0x04009208 RID: 37384
			public static LocString LIQUID_PRESSURE = "neighboring liquid pressure";

			// Token: 0x04009209 RID: 37385
			public static LocString CIRCUIT_OVERLOADED = "an overloaded circuit";

			// Token: 0x0400920A RID: 37386
			public static LocString LOGIC_CIRCUIT_OVERLOADED = "an overloaded logic circuit";

			// Token: 0x0400920B RID: 37387
			public static LocString MICROMETEORITE = "micrometeorite";

			// Token: 0x0400920C RID: 37388
			public static LocString COMET = "falling space rocks";

			// Token: 0x0400920D RID: 37389
			public static LocString ROCKET = "rocket engine";
		}

		// Token: 0x02002014 RID: 8212
		public static class AUTODISINFECTABLE
		{
			// Token: 0x020028AC RID: 10412
			public static class ENABLE_AUTODISINFECT
			{
				// Token: 0x0400B2DC RID: 45788
				public static LocString NAME = "Enable Disinfect";

				// Token: 0x0400B2DD RID: 45789
				public static LocString TOOLTIP = "Automatically disinfect this building when it becomes contaminated";
			}

			// Token: 0x020028AD RID: 10413
			public static class DISABLE_AUTODISINFECT
			{
				// Token: 0x0400B2DE RID: 45790
				public static LocString NAME = "Disable Disinfect";

				// Token: 0x0400B2DF RID: 45791
				public static LocString TOOLTIP = "Do not automatically disinfect this building";
			}

			// Token: 0x020028AE RID: 10414
			public static class NO_DISEASE
			{
				// Token: 0x0400B2E0 RID: 45792
				public static LocString TOOLTIP = "This building is clean";
			}
		}

		// Token: 0x02002015 RID: 8213
		public static class DISINFECTABLE
		{
			// Token: 0x020028AF RID: 10415
			public static class ENABLE_DISINFECT
			{
				// Token: 0x0400B2E1 RID: 45793
				public static LocString NAME = "Disinfect";

				// Token: 0x0400B2E2 RID: 45794
				public static LocString TOOLTIP = "Mark this building for disinfection";
			}

			// Token: 0x020028B0 RID: 10416
			public static class DISABLE_DISINFECT
			{
				// Token: 0x0400B2E3 RID: 45795
				public static LocString NAME = "Cancel Disinfect";

				// Token: 0x0400B2E4 RID: 45796
				public static LocString TOOLTIP = "Cancel this disinfect order";
			}

			// Token: 0x020028B1 RID: 10417
			public static class NO_DISEASE
			{
				// Token: 0x0400B2E5 RID: 45797
				public static LocString TOOLTIP = "This building is already clean";
			}
		}

		// Token: 0x02002016 RID: 8214
		public static class REPAIRABLE
		{
			// Token: 0x020028B2 RID: 10418
			public static class ENABLE_AUTOREPAIR
			{
				// Token: 0x0400B2E6 RID: 45798
				public static LocString NAME = "Enable Autorepair";

				// Token: 0x0400B2E7 RID: 45799
				public static LocString TOOLTIP = "Automatically repair this building when damaged";
			}

			// Token: 0x020028B3 RID: 10419
			public static class DISABLE_AUTOREPAIR
			{
				// Token: 0x0400B2E8 RID: 45800
				public static LocString NAME = "Disable Autorepair";

				// Token: 0x0400B2E9 RID: 45801
				public static LocString TOOLTIP = "Only repair this building when ordered";
			}
		}

		// Token: 0x02002017 RID: 8215
		public static class AUTOMATABLE
		{
			// Token: 0x020028B4 RID: 10420
			public static class ENABLE_AUTOMATIONONLY
			{
				// Token: 0x0400B2EA RID: 45802
				public static LocString NAME = "Disable Manual";

				// Token: 0x0400B2EB RID: 45803
				public static LocString TOOLTIP = "This building's storage may be accessed by Auto-Sweepers only\n\nDuplicants will not be permitted to add or remove materials from this building";
			}

			// Token: 0x020028B5 RID: 10421
			public static class DISABLE_AUTOMATIONONLY
			{
				// Token: 0x0400B2EC RID: 45804
				public static LocString NAME = "Enable Manual";

				// Token: 0x0400B2ED RID: 45805
				public static LocString TOOLTIP = "This building's storage may be accessed by both Duplicants and Auto-Sweeper buildings";
			}
		}
	}
}
