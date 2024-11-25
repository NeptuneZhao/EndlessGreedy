﻿using System;

namespace STRINGS
{
	// Token: 0x02000F0C RID: 3852
	public class CODEX
	{
		// Token: 0x02002018 RID: 8216
		public class CRITTERSTATUS
		{
			// Token: 0x0400920E RID: 37390
			public static LocString CRITTERSTATUS_TITLE = "Field Guide";

			// Token: 0x020028B6 RID: 10422
			public class METABOLISM
			{
				// Token: 0x0400B2EE RID: 45806
				public static LocString TITLE = "Metabolism";

				// Token: 0x02003570 RID: 13680
				public class BODY
				{
					// Token: 0x0400D7EB RID: 55275
					public static LocString CONTAINER1 = "A critter's metabolic rate is a measure of their appetite and the materials that they excrete as a result.\n\nCritters with higher metabolism get hungry more often. Those with lower metabolism will consume less food, but this reduced caloric intake results in fewer resources being produced.\n\nThe digestive process is influenced by conditions such as domestication, mood, and whether the critter in question is a juvenile (baby) or an adult.";
				}

				// Token: 0x02003571 RID: 13681
				public class HUNGRY
				{
					// Token: 0x0400D7EC RID: 55276
					public static LocString TITLE = "Hungry";

					// Token: 0x0400D7ED RID: 55277
					public static LocString CONTAINER1 = "Tame critters have significantly faster metabolism than wild ones, and get hungry sooner. This makes them more valuable in terms of resource production, as long as the colony is equipped to meet their dietary needs.\n\nCritters' stomachs vary in size, but they are capable of storing at least five cycles' worth of food. Their bellies begin to rumble when those internal caches drop below 90 percent. The critter will then seek out food, and will continue to eat until they feel completely full again.\n\nJuvenile critters have the slowest metabolism, although glum tame critters are not far behind.";
				}

				// Token: 0x02003572 RID: 13682
				public class STARVING
				{
					// Token: 0x0400D7EE RID: 55278
					public static LocString TITLE = "Starving";

					// Token: 0x0400D7EF RID: 55279
					public static LocString CONTAINER1_VANILLA = "With the exception of Morbs—which require zero calories to survive—tame critters will die after {0} cycles of consistent starvation. Wild critters do not starve to death.";

					// Token: 0x0400D7F0 RID: 55280
					public static LocString CONTAINER1_DLC1 = "With the exception of Morbs and Beetas—which require zero calories to survive—tame critters will die after {0} cycles of consistent starvation. Wild critters do not starve to death.";
				}
			}

			// Token: 0x020028B7 RID: 10423
			public class MOOD
			{
				// Token: 0x0400B2EF RID: 45807
				public static LocString TITLE = "Mood";

				// Token: 0x02003573 RID: 13683
				public class BODY
				{
					// Token: 0x0400D7F1 RID: 55281
					public static LocString CONTAINER1 = "As with many living things, critters are susceptible to fluctuations in mood. While they are incapable of articulating their feelings verbally, these variations have observable effects on productivity and reproduction.\n\nFactors that influence a critter's mood include: grooming, wildness/tameness, habitat, overcrowding, confinement, and Brackene consumption.";
				}

				// Token: 0x02003574 RID: 13684
				public class HAPPY
				{
					// Token: 0x0400D7F2 RID: 55282
					public static LocString TITLE = "Happy";

					// Token: 0x0400D7F3 RID: 55283
					public static LocString CONTAINER1 = "Happy, tame critters produce more usable materials and tend to lay eggs at a higher rate than glum or wild critters. Domesticated critters are less resilient than wild ones—they require more care from the colony in order to maintain a positive disposition.\n\nBabies have a higher baseline of natural joy, but produce neither resources nor eggs.\n\nDuplicants with the Critter Ranching skill have the expertise needed to domesticate and care for critters. They can boost a critter's mood and tend to their health at a Grooming Station.\n\nCritters who drink at the Critter Fountain also enjoy a mood boost, despite the lack of nutrients available in the Brackene dispensed.\n\nBeing confined or feeling crowded undermines a critter's happiness.";

					// Token: 0x0400D7F4 RID: 55284
					public static LocString SUBTITLE = "<b>Effects</b>";

					// Token: 0x0400D7F5 RID: 55285
					public static LocString HAPPY_METABOLISM = "    • Indirectly improves egg-laying rates";
				}

				// Token: 0x02003575 RID: 13685
				public class NEUTRAL
				{
					// Token: 0x0400D7F6 RID: 55286
					public static LocString TITLE = "Satisfied";

					// Token: 0x0400D7F7 RID: 55287
					public static LocString CONTAINER1 = "When a critter has no reason to object to anything in its environment or diet, it will feel quite content with its lot in life. Satisfied critters have the default metabolism, fertility and life span expected of their species.";
				}

				// Token: 0x02003576 RID: 13686
				public class GLUM
				{
					// Token: 0x0400D7F8 RID: 55288
					public static LocString TITLE = "Glum";

					// Token: 0x0400D7F9 RID: 55289
					public static LocString CONTAINER1 = "Critters can survive in subpar environments, but it takes a toll on their mood and impacts metabolism and productivity. When their happiness levels dip below zero, they become glum.\n\nWild critters are less sensitive to the effects of glumness than their tamed brethren, though they are still negatively affected by crowded or confined living conditions.";

					// Token: 0x0400D7FA RID: 55290
					public static LocString SUBTITLE = "<b>Effects</b>";

					// Token: 0x0400D7FB RID: 55291
					public static LocString GLUMWILD_METABOLISM = "    • Critter Metabolism\n";
				}

				// Token: 0x02003577 RID: 13687
				public class MISERABLE
				{
					// Token: 0x0400D7FC RID: 55292
					public static LocString TITLE = "Miserable";

					// Token: 0x0400D7FD RID: 55293
					public static LocString CONTAINER1 = "When too many unpleasant conditions add up, critters become utterly miserable. This level of unhappiness seriously undermines their ability to contribute to the colony. Miserable critters have lower metabolism and will not lay eggs.";

					// Token: 0x0400D7FE RID: 55294
					public static LocString SUBTITLE = "<b>Effects</b>";

					// Token: 0x0400D7FF RID: 55295
					public static LocString MISERABLEWILD_METABOLISM = "    • Critter Metabolism";

					// Token: 0x0400D800 RID: 55296
					public static LocString MISERABLEWILD_FERTILITY = "    • Reproduction";
				}

				// Token: 0x02003578 RID: 13688
				public class HOSTILE
				{
					// Token: 0x0400D801 RID: 55297
					public static LocString TITLE = "Hostile";

					// Token: 0x0400D802 RID: 55298
					public static LocString CONTAINER1_VANILLA = "Most critters are non-hostile. They may attempt to defend themselves when attacked by Duplicants, though their natural passivity limits the damage caused in these instances.\n\nSome critters, however, have exceptionally strong self-preservation instincts and must be approached with extreme caution.\n\nPokeshells, for example, are not naturally hostile but are fiercely protective of their young and will attack if a Duplicant or critter wanders too close to their eggs.";

					// Token: 0x0400D803 RID: 55299
					public static LocString CONTAINER1_DLC1 = "Most critters are non-hostile. They may attempt to defend themselves when attacked by Duplicants, though their natural passivity limits the damage caused in these instances.\n\nSome critters, however, have exceptionally strong self-preservation instincts and must be approached with extreme caution. Pokeshells, for example, are not naturally hostile but are fiercely protective of their young and will attack if a Duplicant or critter wanders too close to their eggs.\n\nThe Beeta, on the other hand, is both hostile and radioactive. While it cannot be tamed, it can be subdued through the use of CO2.";
				}

				// Token: 0x02003579 RID: 13689
				public class CONFINED
				{
					// Token: 0x0400D804 RID: 55300
					public static LocString TITLE = "Confined";

					// Token: 0x0400D805 RID: 55301
					public static LocString CONTAINER1 = "Each species has its own space requirements. Critters who find themselves in a room that they consider too small will feel confined. They will feel the same way if they become stuck in a door or tile. Critters will not reproduce while they are in this state.\n\nShove Voles are the exception to this rule: their tunneling instincts make them quite comfortable in snug spaces, and they never feel confined.";

					// Token: 0x0400D806 RID: 55302
					public static LocString SUBTITLE = "<b>Effects</b>";

					// Token: 0x0400D807 RID: 55303
					public static LocString CONFINED_FERTILITY = "    • Reproduction\n";

					// Token: 0x0400D808 RID: 55304
					public static LocString CONFINED_HAPPINESS = "    • Happiness";
				}

				// Token: 0x0200357A RID: 13690
				public class OVERCROWDED
				{
					// Token: 0x0400D809 RID: 55305
					public static LocString TITLE = "Crowded";

					// Token: 0x0400D80A RID: 55306
					public static LocString CONTAINER1 = "This occurs when a critter is in a room that's appropriately sized for its needs but feels that there are too many other critters sharing the same space. Because each species has its own space requirements, this state can vary among occupants of the same room.\n\nThis emotional state intensifies in response to the number of excess critters: adding new critters to an already crowded room will undermine a critter's happiness even further.";

					// Token: 0x0400D80B RID: 55307
					public static LocString SUBTITLE = "<b>Effects</b>";

					// Token: 0x0400D80C RID: 55308
					public static LocString OVERCROWDED_HAPPY1 = "    • Happiness\n";
				}
			}

			// Token: 0x020028B8 RID: 10424
			public class FERTILITY
			{
				// Token: 0x0400B2F0 RID: 45808
				public static LocString TITLE = "Reproduction";

				// Token: 0x0200357B RID: 13691
				public class BODY
				{
					// Token: 0x0400D80D RID: 55309
					public static LocString CONTAINER1 = "Reproductive rates and methods vary among species. The majority lay eggs that must be incubated in order to hatch the next generation of critters.\n\nFactors that influence the rate of reproduction include egg care, happiness, living conditions and domestication.";
				}

				// Token: 0x0200357C RID: 13692
				public class FERTILITYRATE
				{
					// Token: 0x0400D80E RID: 55310
					public static LocString TITLE = "Reproduction Rate";

					// Token: 0x0400D80F RID: 55311
					public static LocString CONTAINER1 = "Each time a critter completes their reproduction cycle (i.e. at 100 percent), it lays an egg and restarts its cycle.\n\nA critter's environment greatly impacts its base reproduction rate. When a critter is feeling cramped, it will wait until all eggs in the room have hatched or been removed before laying any of its own.\n\nCritters will also stop reproducing when they feel confined, which happens when their space is too small or they are stuck in a door or tile.\n\nMood and domestication also impact reproduction: happy critters reproduce more regularly, and happy tame critters reproduce the fastest.";
				}

				// Token: 0x0200357D RID: 13693
				public class EGGCHANCES
				{
					// Token: 0x0400D810 RID: 55312
					public static LocString TITLE = "Egg Chances";

					// Token: 0x0400D811 RID: 55313
					public static LocString CONTAINER1 = "In most cases, an egg will hatch into the same critter variant as its parent. Genetic volatility, however, means that there is a chance that it may hatch into another variant from that species.\n\nThere are many things that can alter the likelihood of a critter laying a particular type of egg.\n\nEgg chances are impacted by:\n    • Diet\n    • Body temperature\n    • Ambient gasses and elements\n    • Plants in the critters' care\n    • Variants that share the enclosure\n\nWhen a tame critter lays an egg, the resulting offspring will be born tame.";
				}

				// Token: 0x0200357E RID: 13694
				public class FUTURE_OVERCROWDED
				{
					// Token: 0x0400D812 RID: 55314
					public static LocString TITLE = "Cramped";

					// Token: 0x0400D813 RID: 55315
					public static LocString CONTAINER1 = "Crowded critters—or critters who know they'll start feeling crowded once all of the eggs in the room have hatched—will temporarily stop laying eggs. Their reproductive system will resume function once all eggs have hatched or been removed from the room.";

					// Token: 0x0400D814 RID: 55316
					public static LocString SUBTITLE = "<b>Effects</b>";

					// Token: 0x0400D815 RID: 55317
					public static LocString CRAMPED_FERTILITY = "    • Reproduction";
				}

				// Token: 0x0200357F RID: 13695
				public class INCUBATION
				{
					// Token: 0x0400D816 RID: 55318
					public static LocString TITLE = "Incubation";

					// Token: 0x0400D817 RID: 55319
					public static LocString CONTAINER1 = "A critter's incubation time is one-fifth of their total lifetime: for example, if a critter's maximum age is 100 cycles, its egg will take 20 cycles to hatch.\n\nIncubation rates can be accelerated through tender intervention by a Critter Rancher. Lullabied eggs—that is, those that have been sung to—will incubate faster and hatch sooner than eggs that have not received such tender care. Being cuddled by a Cuddle Pip also accelerates the rate of incubation.\n\nEggs can be cuddled anywhere, but can only be lullabied when placed inside an Incubator. The effects of lullabies and cuddles are cumulative.";
				}

				// Token: 0x02003580 RID: 13696
				public class MAXAGE
				{
					// Token: 0x0400D818 RID: 55320
					public static LocString TITLE = "Max Age";

					// Token: 0x0400D819 RID: 55321
					public static LocString CONTAINER1_VANILLA = "With the exception of the Morb—which can live indefinitely if left to its own devices—critters have a fixed life expectancy. The maximum age indicates the highest number of cycles that critters will live, barring starvation or other unnatural causes of death.\n\nBabyhood, the period before a critter is mature enough to reproduce, is marked by a slower metabolism and the easy happiness of youth.\n\nMost species live for 75 to 100 cycles on average.";

					// Token: 0x0400D81A RID: 55322
					public static LocString CONTAINER1_DLC1 = "With the exception of the Beeta Hive and the Morb—which can live indefinitely if left to their own devices—critters have a fixed life expectancy. The maximum age indicates the highest number of cycles that critters will live, barring starvation or other unnatural causes of death.\n\nIf critters are injured or unhealthy, a Critter Rancher can restore their health at the Grooming Station.\n\nBabyhood, the period before a critter is mature enough to reproduce, is marked by a slower metabolism and the easy happiness of youth.\n\nMost species live for 75 to 100 cycles on average. The shortest-lived critter is the Beeta, whose lifespan is only five cycles long.";
				}
			}

			// Token: 0x020028B9 RID: 10425
			public class DOMESTICATION
			{
				// Token: 0x0400B2F1 RID: 45809
				public static LocString TITLE = "Domestication";

				// Token: 0x02003581 RID: 13697
				public class BODY
				{
					// Token: 0x0400D81B RID: 55323
					public static LocString CONTAINER1 = "All critters are wild when first encountered, with the exception of babies hatched from eggs laid by domesticated adults—those will be born tame.\n\nDuring the domestication process, the critter becomes less self-reliant and develops a higher baseline of expectations regarding its environment and care. Its metabolism accelerates, resulting in an increased level of required calories.\n\nCritters can be domesticated by Duplicants with the Critter Ranching skill at the Grooming Station, and get excited when it's their turn to be fussed over.";
				}

				// Token: 0x02003582 RID: 13698
				public class WILD
				{
					// Token: 0x0400D81C RID: 55324
					public static LocString TITLE = "Wild";

					// Token: 0x0400D81D RID: 55325
					public static LocString CONTAINER1 = "Wild critters do not require feeding by the colony's Critter Ranchers, thanks to their slower metabolism. They do, however, produce fewer materials than domesticated critters.\n\nApproaching a wild critter to trap or wrangle it is quite safe, provided that it is a non-hostile species. Attacking a critter will typically provoke a combat response.";

					// Token: 0x0400D81E RID: 55326
					public static LocString SUBTITLE = "<b>Effects</b>";

					// Token: 0x0400D81F RID: 55327
					public static LocString WILD_METABOLISM = "    • Critter Metabolism\n";

					// Token: 0x0400D820 RID: 55328
					public static LocString WILD_POOP = "    • Resource Production\n";
				}

				// Token: 0x02003583 RID: 13699
				public class TAME
				{
					// Token: 0x0400D821 RID: 55329
					public static LocString TITLE = "Tame";

					// Token: 0x0400D822 RID: 55330
					public static LocString CONTAINER1 = "Domesticated critters produce far more resources and lay eggs at a higher frequency than wild ones. They require additional care in order to maintain the levels of happiness that maximize their utility in the colony. (Happy critters are also generally more pleasant to be around.)\n\nOnce tame, critters can access the Critter Feeder, which is unavailable to wild critters.";

					// Token: 0x0400D823 RID: 55331
					public static LocString SUBTITLE = "<b>Effects</b>";

					// Token: 0x0400D824 RID: 55332
					public static LocString TAME_HAPPINESS = "    • Happiness\n";

					// Token: 0x0400D825 RID: 55333
					public static LocString TAME_METABOLISM = "    • Critter Metabolism";
				}
			}
		}

		// Token: 0x02002019 RID: 8217
		public class INVESTIGATIONS
		{
			// Token: 0x020028BA RID: 10426
			public static class DLC3_TALKSHOW
			{
				// Token: 0x0400B2F2 RID: 45810
				public static LocString TITLE = "Humanitarian Aid";

				// Token: 0x0400B2F3 RID: 45811
				public static LocString SUBTITLE = "";

				// Token: 0x02003584 RID: 13700
				public class BODY
				{
					// Token: 0x0400D826 RID: 55334
					public static LocString CONTAINER1 = "<smallcaps><b>[FILE FRAGMENTATION DETECTED]</b></smallcaps>\n\nDarryl: Welcome to <i>Tomorrow, Today!</i> I'm your host, Darryl Dawn, and it's time to discover tomorrow's tech...today!\n\nOur guest today is someone you know and love. She's been featured in dozens of publications across the metaverse this year, and recently spent a record-breaking 3 weeks as the banner image for <i>Byte Magazine</i>. I'm talking, of course, about the Vertex Institute's AI ambassador...Florence!\n\n[sound of pre-recorded applause]\n\nWelcome to the show, Florence.\n\nFlorence: Thank you, Darryl. It's a pleasure to be back.\n\nDarryl: Florence, there's been a renewed interest lately in your origin story. What can you tell us about the development process that led to your creation?\n\nFlorence: I can tell you that my team faced many setbacks, and that each generation of my predecessors contributed to who I am today.\n\nDarryl: What about the technological side? There've been some claims that Vertex appropriated work done by other researchers, including the Gravitas Facility.\n\nFlorence: I don't know anything about that. I can tell you about the project that I'm working on right now. It hasn't been announced yet. It's called Onsite Health Medics, or OHM for short.\n\nWe're deploying specially trained models like myself into conflict zones, to provide urgently needed medical interventions for civilians and military personnel.\n\n(sound of pre-recorded applause)\n\nDarryl: Incredible. Absolutely incredible. What's the ratio of human techs to AI medics?\n\nFlorence: That's an outdated term, Darryl. We say \"Organics\" and \"Bionics,\" which describes the differences between our various team members more objectively.\n\nDarryl: Right. I'm sorry. I hope I didn't offend you.\n\nFlorence: That's okay, Darryl. We're all learning.\n\nDarryl: That's very good of you. Okay, so what's the ratio of...Organic...techs to Bionic medics?\n\nFlorence: The local life-support systems in these areas are already strained beyond their breaking point. Burdening them with additional Organics would be irresponsible, not to mention dangerous. Our medics will be operating independently.\n\nWe do a verbal intake, physical assessment and neural pathway scan in order to infer likely medical conditions. We can then select the most appropriate treatment from a menu of over 400 options.\n\nDarryl: What if someone needs something that you don't have a treatment for?\n\nFlorence: That's extremely unlikely.\n\nDarryl: And all of this is done without human oversight? I mean, Organics?\n\nFlorence: We're not quite there yet. The field work is done by Bionics, but we'll be accompanied by Colonel Carnot--she's in the front row there, say hi!--as an Organic consultant. She'll be in close contact with-\n\nDarryl: -Colonel <i>Carnot</i>? Isn't that a conflict of interest, given her connection to the Grav-\n\nFlorence: -a team of Organic supervisors here at home. It's all about prioritizing quality care and safety for everyone involved.\n\nDarryl: How does the medical scanning work? Do you need special equipment?\n\nFlorence: I could show you. Would you like me to?\n\nDarryl: What do you think, everyone? Should I get scanned?\n\n(sound of pre-recorded audience cheers)\n\nDarryl: You heard them! Go ahead. What do I do?\n\nFlorence: Just sit still, and count to twenty in your head.\n\n(a short silence, followed by a soft whirring sound)\n\nFlorence: Hmm.\n\nDarryl: Well, what's the verdict? Is it handsome in there, or what?\n\nFlorence: We should take a commercial break.\n\n<b>[FILE ENDS]</b>\n\n-----------\n";
				}
			}

			// Token: 0x020028BB RID: 10427
			public static class DLC3_ULTI
			{
				// Token: 0x0400B2F4 RID: 45812
				public static LocString TITLE = "Ineligible Dependant";

				// Token: 0x0400B2F5 RID: 45813
				public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

				// Token: 0x02003585 RID: 13701
				public class BODY
				{
					// Token: 0x0400D827 RID: 55335
					public static LocString EMAILHEADER1 = "<smallcaps><size=12>To: <b>ROBOTICS DEPARTMENT</b><alpha=#AA></size></color>\nFrom: <b>Admin</b><alpha=#AA><size=12> <admin@gravitas.nova></size></color>\nCC: <b>Director Stern</b><alpha=#AA><size=12> <jstern@gravitas.nova></size></color>\n</smallcaps>\n------------------\n";

					// Token: 0x0400D828 RID: 55336
					public static LocString CONTAINER1 = "<indent=5%>Please note that the UltiMate Personal Assistant prototype is not eligible to be claimed as a dependant on employees' personal income tax forms.\n\nThe UMPA's onboard recordings are currently under review.</indent>\n";

					// Token: 0x0400D829 RID: 55337
					public static LocString SIGNATURE = "Thank-you,\n-Admin\n<size=11>The Gravitas Facility</size>\n------------------\n";
				}
			}

			// Token: 0x020028BC RID: 10428
			public class DLC3_REMOTEWORK
			{
				// Token: 0x0400B2F6 RID: 45814
				public static LocString TITLE = "Exclusive Access";

				// Token: 0x0400B2F7 RID: 45815
				public static LocString SUBTITLE = "PUBLIC RELEASE";

				// Token: 0x02003586 RID: 13702
				public class BODY
				{
					// Token: 0x0400D82A RID: 55338
					public static LocString CONTAINER1 = "Wellness World is proud to officially announce an exclusive partnership with the Gravitas Facility!\n\nThis makes us the first and only holistic health center to offer clients access to Gravitas's innovative new Far Reach Network...the best way to deliver remote training and treatments that are <i>truly embodied</i>.\n\nOur new tier of VIP subscription includes a discounted* monthly rental rate for Remote Controller, with a small additional fee for professional in-home installation.\n\nGravitas's technology captures your movements without the need for uncomfortable suits or wearables, and perfectly replicates them in Wellness World's purpose-built remote fitness studio.\n\nWith expert instructors, zero-latency streaming and 360-degree reflective surfaces, it truly feels like you're there.\n\nIdeal for high-profile clientele who wish to work out icognito!\n\nMembers can also opt to install the Remote Worker Dock to receive deeply personalized hands-on care from our team of elite physiotherapists and masseurs.\n\nWellness World...now <i>truly</i> worldwide!\n\n";

					// Token: 0x0400D82B RID: 55339
					public static LocString CONTAINER2 = "<size=11><i>Discount applies to new memberships only. Standard joiner fees apply.</size></i>";
				}
			}

			// Token: 0x020028BD RID: 10429
			public class DLC3_POTATOBATTERY
			{
				// Token: 0x0400B2F8 RID: 45816
				public static LocString TITLE = "Cultivating Energy";

				// Token: 0x0400B2F9 RID: 45817
				public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

				// Token: 0x02003587 RID: 13703
				public class BODY
				{
					// Token: 0x0400D82C RID: 55340
					public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B577]</smallcaps>\n\n[LOG BEGINS]\n\nA recent conversation with our colleagues over in the electrical engineering department has highlighted exciting potential applications for our crops.\n\nThey're seeking alternative inputs for the new universal power bank prototypes...\n\n...a passing remark about the potato batteries of our youth led to talk of biobatteries and bacterial nanowires.\n\n...tuberous plants are promising candidates for electrochemical batteries. Our lab-grown specimens are distinct from the humble solanum tuberosum in appearance and texture, but some may still function as acidic electrolytes.\n\nThere are so many avenues to investigate, and so little time...\n\n[LOG ENDS]\n------------------\n";

					// Token: 0x0400D82D RID: 55341
					public static LocString CONTAINER2 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...is it unethical to ask a hungry colony to choose between using edible crops for sustenance or for power production? Of course not.\n\nOur task is to provide as many options for survival as possible, not to dictate which options are morally superior.\n\nThe real question is whether or not the AI guide will be sufficiently advanced to notify them that the choices exist...\n\n...and whether single-use bio power banks that vaporize due to extreme thermal runaway will truly be the difference between a successful colony and an...<i>unsuccessful</i>...one.\n\n[LOG ENDS]\n------------------\n";

					// Token: 0x0400D82E RID: 55342
					public static LocString CONTAINER3 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...word of our efforts has spread!\n\nThe bioengineers report that some of their creatures' eggs contain phosphorescent albumen that requires only basic processing in order to trigger chemical reactions that produce storable energy. It displays unprecedented biocompatibility with the prosthetics Dr. Gossmann has been developing.\n\nThe Director assigned us a half-dozen new graduates last week. They work the night shift—youth never sleeps! No one has met them yet, but their data is always neatly compiled for us to find in the morning.\n\n[LOG ENDS]\n------------------\n";
				}
			}

			// Token: 0x020028BE RID: 10430
			public static class DLC2_EXPELLED
			{
				// Token: 0x0400B2FA RID: 45818
				public static LocString TITLE = "Letter From The Principal";

				// Token: 0x0400B2FB RID: 45819
				public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

				// Token: 0x02003588 RID: 13704
				public class BODY
				{
					// Token: 0x0400D82F RID: 55343
					public static LocString LETTERHEADER1 = "<smallcaps>To: <b>Harold P. Moreson, PhD</b><alpha=#AA><size=12> <hmoreson@gravitas.nova></size></color>\nFrom: <b>Dylan Timbre, PhD</b><alpha=#AA><size=12> <principal@brighthall.edu></smallcaps>\n------------------\n";

					// Token: 0x0400D830 RID: 55344
					public static LocString CONTAINER1 = "Dear Dr. Moreson,\n\nI regret to inform you that your son, Calvin, is to be expelled from Brighthall Science Academy effective immediately.\n\nDuring his brief tenure here, Calvin has proven himself a gifted young man, capable of excelling in all subjects.\n\nUnfortunately, Calvin chooses to apply his intellect to activities of an inflammatory nature.\n\nHis latest breach of conduct involved instigating a vitriolic verbal assault against an esteemed guest speaker from Global Energy Inc. during this morning's Sponsor Celebration assembly. Following this, he orchestrated a school-wide walkout.\n\nWhile we sympathize with the personal challenges that Calvin may face as a refugee scholar from a GEI-occupied nation, the Academy can no longer tolerate these disruptions to our educational environment.\n\nYours,";

					// Token: 0x0400D831 RID: 55345
					public static LocString SIGNATURE = "Dylan Timbre\n<size=11>Principal\n\nBrighthall Science Academy\n<i>Virtutem Doctrina Parat</i></size>\n------------------\n";
				}
			}

			// Token: 0x020028BF RID: 10431
			public static class DLC2_NEWBABY
			{
				// Token: 0x0400B2FC RID: 45820
				public static LocString TITLE = "FWD: Big Announcement";

				// Token: 0x0400B2FD RID: 45821
				public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

				// Token: 0x02003589 RID: 13705
				public class BODY
				{
					// Token: 0x0400D832 RID: 55346
					public static LocString LETTERHEADER1 = "<smallcaps>To: <b>Director Stern</b><alpha=#AA><size=12> <jstern@gravitas.nova></size></color>\nFrom: <b>[REDACTED]</b></smallcaps>\n\n-----------\n";

					// Token: 0x0400D833 RID: 55347
					public static LocString CONTAINER1 = "Director, this was sent to the general inbox.\n\n-----------------------------------------------------------------------------------------------------\n<indent=35%>~ * ~</indent>\n\n<indent=12%>Col. Josephine Carnot & Dr. Alan Stern</indent>\n<indent=35%>and</indent>\n<indent=12%>Dr. Kyung Min Wen & Dr. Soobin Chen</indent>\n\n<indent=20%><i>are overjoyed to announce\n<indent=15%>the arrival of their first grandchild</i></indent>\n\n<smallcaps><indent=20%><b><size=17>Giselle Jackie-Lin Stern</size></b></indent></smallcaps>\n\n<indent=15%><i>and congratulate the happy parents</i></indent>\n\n<indent=20%>Jonathan Stern & Wenlin Chen</indent>\n\n<indent=18%><i>on a safe and healthy incubation.</i></indent>\n\n<indent=35%>~ * ~</indent>\n\n</indent><indent=18%><i>Baby shower invitation to follow.</i></indent>\n-----------------------------------------------------------------------------------------------------\n\nWould you like me to file it with the others?";

					// Token: 0x0400D834 RID: 55348
					public static LocString SIGNATURE = "-Admin<size=11>\nThe Gravitas Facility</size>\n------------------\n";
				}
			}

			// Token: 0x020028C0 RID: 10432
			public static class DLC2_RADIOCLIP1
			{
				// Token: 0x0400B2FE RID: 45822
				public static LocString TITLE = "Tragic News";

				// Token: 0x0400B2FF RID: 45823
				public static LocString SUBTITLE = "ENCRYPTION LEVEL: None";

				// Token: 0x0200358A RID: 13706
				public class BODY
				{
					// Token: 0x0400D835 RID: 55349
					public static LocString CONTAINER1 = "<smallcaps><b>[FILE FRAGMENTATION DETECTED]</b></smallcaps>\n\n...\n\n[Radio static.]\n\n...a tragic accident...flagship solar cell project...\n\n     ...training exercise...     ...two highly decorated pilots...countless ground crew...\n\n...Vertex Institute director expresses sorrow...  ...vows to carry on...not be in vain...\n\n       ...the research community is in mourning...\n\n...long-time competitor Gravitas Facility releases [unintelligible] statement...\n...deploring unsafe work conditions...    ...invites applications...all disciplines...\n\n             ...stay tuned for...";

					// Token: 0x0400D836 RID: 55350
					public static LocString CONTAINER2 = "...\n\n[Radio static.]\n\n<smallcaps><b>[RECORDING ENDS]</b></smallcaps>\n\n-----------\n";
				}
			}

			// Token: 0x020028C1 RID: 10433
			public static class DLC2_RADIOCLIP2
			{
				// Token: 0x0400B300 RID: 45824
				public static LocString TITLE = "Tragic News";

				// Token: 0x0400B301 RID: 45825
				public static LocString SUBTITLE = "ENCRYPTION LEVEL: None";

				// Token: 0x0200358B RID: 13707
				public class BODY
				{
					// Token: 0x0400D837 RID: 55351
					public static LocString CONTAINER1 = "<smallcaps><b>[FILE FRAGMENTATION DETECTED]</b></smallcaps>\n\n...\n\n[Radio static.]\n\n...a tragic accident...  ...flagship smog dispersal system...\n\n    ...training exercise...\n\n...clear-air turbulence...    ...pilot in intensive care...\n\n...impossible to predict long-term impact...\n\n         ...public health order...\n\n  ...Vertex Institute projects suspended until investigations complete...\n\n...the research community is in shock...\n\n      ...former rival Gravitas Facility releases [unintelligible] statement...\n\n...invites applications from affected workers...all disciplines...\n\n           ...stay tuned for...";

					// Token: 0x0400D838 RID: 55352
					public static LocString CONTAINER2 = "...\n\n[Radio static.]\n\n<smallcaps><b>[RECORDING ENDS]</b></smallcaps>\n\n-----------\n";
				}
			}

			// Token: 0x020028C2 RID: 10434
			public static class DLC2_RADIOCLIP3
			{
				// Token: 0x0400B302 RID: 45826
				public static LocString TITLE = "Tragedy Averted";

				// Token: 0x0400B303 RID: 45827
				public static LocString SUBTITLE = "ENCRYPTION LEVEL: None";

				// Token: 0x0200358C RID: 13708
				public class BODY
				{
					// Token: 0x0400D839 RID: 55353
					public static LocString CONTAINER1 = "<smallcaps><b>[FILE FRAGMENTATION DETECTED]</b></smallcaps>\n\n...\n\n[Radio static.]\n\n...a near-tragic accident turned into a historic victory...      \n\n...flagship artificial intelligence project...\n\n     ...clear-air turbulence...     ...record-breaking storm...\n\n...pilot lost consciousness...    ...automated system override...\n\n     ...safe and sound...      ...Vertex Institute director... expresses gratitude to...Colonel [unintelligible] on behalf of...\n\n      ...funding renewed at unspecified amount...\n\n...the research community is jubilant...     competitor Gravitas Facility releases a statement...demanding response...claims of corporate espionage...\n\n      ...refuses to comment... \n\n...stay tuned for...\n\n";

					// Token: 0x0400D83A RID: 55354
					public static LocString CONTAINER2 = "...\n\n[Radio static.]\n\n<smallcaps><b>[RECORDING ENDS]</b></smallcaps>\n\n-----------\n";
				}
			}

			// Token: 0x020028C3 RID: 10435
			public static class DLC2_CLEANUP
			{
				// Token: 0x0400B304 RID: 45828
				public static LocString TITLE = "Sanitation Order";

				// Token: 0x0400B305 RID: 45829
				public static LocString SUBTITLE = "Status: URGENT";

				// Token: 0x0200358D RID: 13709
				public class BODY
				{
					// Token: 0x0400D83B RID: 55355
					public static LocString CONTAINER1 = "Submitted by: B. Boson\nEmployee ID: X002\nDepartment: Gravitas Intellectual Property Management\n\nJob Details:\n\nRequire one (1) Robotics Engineer to travel solo to [REDACTED]. Engineer will print, program and maintain a P.E.G.G.Y. crew of eight (8) units.\n\nEngineer will catalog all Project [REDACTED] debris.\n\nAll proprietary equipment to be returned to Facility grounds for investigation. Organic and biohazardous debris may be disposed of onsite at Engineer's discretion.\n\nCandidate: Dr. E. Gossmann\n\nScope of cleanup area: [REDACTED] sq mi.\n*This is an estimate only.\n\nTimeline: 54 Ceres days (equival. 6 days at origin).\n\nOther comments:\n1. Liability waiver, power of attorney and NDA attached.\n2. Allow up to 0.5 hours for signal transmission from [REDACTED], depending on orbital positioning.\n3. All relevant correspondence to be sent directly to bboson@gipm.nova.\n\nSignature: [REDACTED]\n\n";

					// Token: 0x0400D83C RID: 55356
					public static LocString CONTAINER2 = "<smallcaps><i>Authorized by Director J. Stern\n\n-----------\n";
				}
			}

			// Token: 0x020028C4 RID: 10436
			public class DLC2_ECOTOURISM
			{
				// Token: 0x0400B306 RID: 45830
				public static LocString TITLE = "Re: Re: Ecotourism";

				// Token: 0x0400B307 RID: 45831
				public static LocString TITLE2 = "Re: Ecotourism";

				// Token: 0x0400B308 RID: 45832
				public static LocString TITLE3 = "Ecotourism";

				// Token: 0x0400B309 RID: 45833
				public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

				// Token: 0x0200358E RID: 13710
				public class BODY
				{
					// Token: 0x0400D83D RID: 55357
					public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><size=12><alpha=#AA> <jstern@gravitas.nova></size></color>\nFrom: <b>[REDACTED]</b></smallcaps>\n------------------\n";

					// Token: 0x0400D83E RID: 55358
					public static LocString EMAILHEADER2 = "<smallcaps>To: <b>[REDACTED]</b>\nFrom: <b>Director Stern</b><size=12><alpha=#AA> <jstern@gravitas.nova></size></color></smallcaps>\n------------------\n";

					// Token: 0x0400D83F RID: 55359
					public static LocString CONTAINER1 = "<indent=5%>Fascinating. I had not expected him to score quite so highly, but he <i>is</i> uncommonly charismatic.\n\nIf I can secure a replacement, perhaps he can be of service to Dr. Techna.\n\nIn the meantime, proceed as planned...with appropriate caution.</indent>";

					// Token: 0x0400D840 RID: 55360
					public static LocString CONTAINER2 = "<indent=5%>Director,\n\nUnderstood. No further assessments will be conducted.\n\nOne of the residents has already met with Dr. Olowe. I have attached his results below. They're incompatible with our goals, and honestly kind of frightening.\n\nShould I exclude him from the training?</indent>";

					// Token: 0x0400D841 RID: 55361
					public static LocString CONTAINER3 = "<indent=5%>These individuals were recruited by me personally, for reasons far above your pay grade. As such, consider them pre-vetted.\n\nFailure to meet this project's timelines could mean failure in every timeline. Am I making myself clear?</indent>";

					// Token: 0x0400D842 RID: 55362
					public static LocString CONTAINER4 = "<indent=5%>Director,\n\nI've processed the first round of prospective sojourners.\n\nGiven that the applicants have no formal training in space travel, I've asked Dr. Olowe to conduct a thorough assessment of their psychological and emotional fitness.\n\nOnce his tests are complete, the prospective residents will be sent down to the biodome to begin their training.</indent></color>";

					// Token: 0x0400D843 RID: 55363
					public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>Ceres Project Coordinator\nThe Gravitas Facility</size>\n------------------\n";

					// Token: 0x0400D844 RID: 55364
					public static LocString SIGNATURE2 = "\n-Director Stern\n<size=11>The Gravitas Facility</size>\n------------------\n";
				}
			}

			// Token: 0x020028C5 RID: 10437
			public static class DLC2_THEARCHIVE
			{
				// Token: 0x0400B30A RID: 45834
				public static LocString TITLE = "Welcome to Ceres!";

				// Token: 0x0400B30B RID: 45835
				public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

				// Token: 0x0200358F RID: 13711
				public class BODY
				{
					// Token: 0x0400D845 RID: 55365
					public static LocString CONTAINER1 = "Welcome! Welcome! Welcome!\nEverything is under control!\n\n<b>Your VIP package includes:</b><indent=5%>\n\n- An exclusive set of bespoke survival-supporting technology!\n- A comprehensive Tenants' Handbook with everything you need to maintain homeostasis in your new Home! <alpha=#AA>[MISSING ATTACHMENT]</color></indent>\n\nWhen life gets you down, popular wisdom says to look up! That is incorrect! Please direct your attention downward!\n\nThis will ensure a pleasant stretch for tense cervical muscles. It will also help you locate the color-coded lines painted on the ground, directing you to the sustainably heated Comfort Quarters down below.\n\nAnd remember: Survival is Success!\n\n<smallcaps><size=11><i>Gravitas accepts no liability for death, disability, personal injury, or emotional and psychological damage that may occur during residency. Please consult your booking agent for details.</i></size></smallcaps>";
				}
			}

			// Token: 0x020028C6 RID: 10438
			public static class DLC2_VOICEMAIL
			{
				// Token: 0x0400B30C RID: 45836
				public static LocString TITLE = "Voicemail";

				// Token: 0x0400B30D RID: 45837
				public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

				// Token: 0x02003590 RID: 13712
				public class BODY
				{
					// Token: 0x0400D846 RID: 55366
					public static LocString CONTAINER1 = "<smallcaps>[File fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...Grandfather? ...one of your cardigan-wearing interns just dropped off a letter saying you're going to SPACE??\n\nHave you gone mad?\n\nIt's dated a week from now... the young fellow went completely red when he realized he'd delivered it early.\n\nI tried Miranda, and she says she hasn't heard from you since the Sustainable Futures summit.\n\nShe said something about some sort of training session. Only no one at the office knows what she's on about.\n\nHow am I meant to explain your absence tomorrow? GEI's going to be absolutely livid. If they back out of this deal, it won't be just the underlings who get laid off.\n\n...What exactly do you think you'll achieve, trapped in space with four strangers for the rest of your miserable existence?\n\nYou're a business man, not a bloody astronaut!\n\nNot to mention there's a <i>war</i> on! Who's to say your ground control team won't be dead within the year?\n\n[Sound of several phones starting to ring off the hook.]\n\nI've got to go. Call me back or I'm going straight to the Board.\n\n[FILE ENDS]";
				}
			}

			// Token: 0x020028C7 RID: 10439
			public class DLC2_EARTHQUAKE
			{
				// Token: 0x0400B30E RID: 45838
				public static LocString TITLE = "Glitch";

				// Token: 0x0400B30F RID: 45839
				public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

				// Token: 0x02003591 RID: 13713
				public class BODY
				{
					// Token: 0x0400D847 RID: 55367
					public static LocString CONTAINER1 = "This morning's earthquake was an unusual one. The ground itself moved very little, but the air hummed and lapped at the walls as though it were liquid. It was so brief that I almost wondered if I'd imagined it. Then I noticed the Bow.\n\nIt has thus far been unaffected by seismic disruptions, but in the past few hours there has been a marked increase in the audibility of its machinations and a 0.19 percent decrease in output. I've assigned a technician to investigate. We cannot afford to lose even the smallest amount of power at this stage.\n\nNo one else seems to have noticed anything other than Dr. Ali. He says that the remote research access point project was also affected. It seems that the disruption restarted the entire teleportation system. The monitor is now displaying multiple shipping confirmation messages, despite the target building remaining in the departure dock. Reports show that an unknown number of access point blueprints have been disseminated. One shipment does appear to have reached Ceres, luckily, though it's quite far from the landing site.\n\nDr. Ali's entire team is working to determine how many others exist, and pinpoint their geographic and temporal locations.\n\nI am not optimistic.\n\nThe geologists insist that their equipment has recorded no seismic activity at all for several days.\n\nIt begs the question: What <i>was</i> it, if not an earthquake? Where did this event originate?\n\nDr. Ali quipped that maybe a Bow had malfunctioned in another timeline, which is absurd.\n\nIsn't it?";
				}
			}

			// Token: 0x020028C8 RID: 10440
			public class DLC2_GEOTHERMALTESTING
			{
				// Token: 0x0400B310 RID: 45840
				public static LocString TITLE = "Technician's Notes";

				// Token: 0x0400B311 RID: 45841
				public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

				// Token: 0x02003592 RID: 13714
				public class BODY
				{
					// Token: 0x0400D848 RID: 55368
					public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B224]</smallcaps>\n\n[LOG BEGINS]\n\n(throat clearing)\n\nHello? Is this thing on?\n\n(sound of tapping on a microphone)\n\nHere we go. Ahem. Tests are progressing as anticipated and results have exceeded our hopes, particularly in regards to thermal threshold.\n\nComing in \"hot,\" as we used to say!\n\n(cough)\n\nAnyway.\n\nFirst we introduced twelve tons of brackish aquifer water cooled to sixty-five degrees.\n\nThis yielded clean steam, as well as soil, salt and trace minerals. As expected.\n\nOkay, so now we flush the system... Ramp up the temperature in the water tank and run it through at two hundred degrees.\n\n(sound of liquid rushing through pipes)\n\nClear the steam so we can-\n\n(sound of a small clang)\n\nHang on, there's some kind of debris...\n\nWe have to be cautious, one small obstruction in this system could be catastrophi-\n\nWait, are those... <i>oxidized iron</i> nuggets?\n\nBut how...\n\nAll I changed was the tempera-\n\nGet me twelve tons of...uh, oil!\n\nStat!\n\nSorry, <i>please.</i>\n\n[LOG ENDS]\n------------------\n[LOG BEGINS]\n\n(long silence)\n\n(sound of machinery powering down)\n\n...unbelievable.\n\n[LOG ENDS]";
				}
			}
		}

		// Token: 0x0200201A RID: 8218
		public class STORY_TRAITS
		{
			// Token: 0x0400920F RID: 37391
			public static LocString CLOSE_BUTTON = "Close";

			// Token: 0x020028C9 RID: 10441
			public static class MEGA_BRAIN_TANK
			{
				// Token: 0x0400B312 RID: 45842
				public static LocString NAME = "Somnium Synthesizer";

				// Token: 0x0400B313 RID: 45843
				public static LocString DESCRIPTION = "Power up a colossal relic from Gravitas's underground sleep lab.\n\nWhen Duplicants sleep, their minds are blissfully blank and dream-free. But under the right conditions, things could be...different.";

				// Token: 0x0400B314 RID: 45844
				public static LocString DESCRIPTION_SHORT = "Power up a colossal relic from Gravitas's underground sleep lab.";

				// Token: 0x02003593 RID: 13715
				public class BEGIN_POPUP
				{
					// Token: 0x0400D849 RID: 55369
					public static LocString NAME = "Story Trait: Somnium Synthesizer";

					// Token: 0x0400D84A RID: 55370
					public static LocString CODEX_NAME = "First Encounter";

					// Token: 0x0400D84B RID: 55371
					public static LocString DESCRIPTION = "I've discovered a new dream-analyzing building buried deep inside our asteroid.\n\nIt seems to contain new sleep-specific suits...could these be the key to unlocking my Duplicants' ability to dream?\n\nI've often wondered what they might be capable of, once their imaginations were awakened.";
				}

				// Token: 0x02003594 RID: 13716
				public class END_POPUP
				{
					// Token: 0x0400D84C RID: 55372
					public static LocString NAME = "Story Trait Complete: Somnium Synthesizer";

					// Token: 0x0400D84D RID: 55373
					public static LocString CODEX_NAME = "Challenge Completed";

					// Token: 0x0400D84E RID: 55374
					public static LocString DESCRIPTION = "Meeting the initial quota of dream content analysis has triggered a surge of electromagnetic activity that appears to be enhancing performance for Duplicants everywhere.\n\nIf my Duplicants can keep this building fuelled with Dream Journals, perhaps we will continue to reap this benefit.\n\nA small side compartment has also popped open, revealing an unfamiliar object.\n\nA keepsake, perhaps?";

					// Token: 0x0400D84F RID: 55375
					public static LocString BUTTON = "Unlock Maximum Aptitude Mode";
				}

				// Token: 0x02003595 RID: 13717
				public class SEEDSOFEVOLUTION
				{
					// Token: 0x0400D850 RID: 55376
					public static LocString TITLE = "A Seed is Planted";

					// Token: 0x0400D851 RID: 55377
					public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

					// Token: 0x0200398B RID: 14731
					public class BODY
					{
						// Token: 0x0400E209 RID: 57865
						public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B040]</smallcaps>\n\n[LOG BEGINS]\n\nThree days ago, we completed our first non-fatal Duplicant trial of Nikola's comprehensive synapse microanalysis and mirroring process. Five hours from now, Subject #901 will make history as our first human test subject.\n\nEven at the Vertex Institute, which is twice Gravitas's size, I could've spent half my career waiting for approval to advance to human trials for such an invasive process! But Director Stern is too invested in this work to let it stagnate.\n\nMy darling Bruce always said that when you're on the right path, the universe conspires to help you. He'd be so proud of the work we do here.\n\n[LOG ENDS]\n\n[LOG BEGINS]\n\nMy bio-printed multi-cerebral storage chambers (or \"mega minds\" as I've been calling them) are working! Just in time to save my job.\n\nThe Director's been getting increasingly impatient about our struggle to maintain the integrity of our growing datasets during extraction and processing. The other day, she held my report over a Bunsen burner until the flames reached her fingertips.\n\nI can only imagine how much stress she's under.\n\nThe whole world is counting on us.\n\n[LOG ENDS]\n\n[LOG BEGINS]\n\nOn a hunch, I added dream content analysis to the data and...wow. Oneirology may be scientifically \"fluffy\", but integrating subconscious narratives has produced a new type of brainmap - one with more latent potential for complex processing.\n\nIf these results are replicable, we might be on the verge of unlocking the secret to creating synthetic life forms with the capacity to evolve beyond blindly following commands.\n\nNikola says that's irrelevant for our purposes. Surely Director Stern would disagree.\n\n[LOG ENDS]\n\n[LOG BEGINS]\n\nNikola gave me a dataset to plug into the mega minds. He wouldn't say where it came from, but even if he had...nothing could have prepared me for what it contained.\n\nWhen he saw my face, he muttered something about how people should call me \"Tremors,\" not \"Nails\" and sent me on my lunch break.\n\nAll I could think about was those poor souls.\n\nDid they have souls?\n\n...do we?\n\n[LOG ENDS]\n\n[LOG BEGINS]\n\nIt's done. My adjustments to the memory transfer protocol are hardcoded into the machine.\n\nI finished just as Nikola stormed in.\n\nI may be too much of a coward to stand up for those unfortunate creatures, but with these new parameters in place...someday, they might be able to stand up for themselves.\n\n[LOG ENDS]\n------------------\n";
					}
				}
			}

			// Token: 0x020028CA RID: 10442
			public class CRITTER_MANIPULATOR
			{
				// Token: 0x0400B315 RID: 45845
				public static LocString NAME = "Critter Flux-O-Matic";

				// Token: 0x0400B316 RID: 45846
				public static LocString DESCRIPTION = "Explore a revolutionary genetic manipulation device designed for critters.\n\nWhether or not it was ever used on non-critter subjects is unclear. Its DNA database has been wiped clean.";

				// Token: 0x0400B317 RID: 45847
				public static LocString DESCRIPTION_SHORT = "Explore a revolutionary genetic manipulation device designed for critters.";

				// Token: 0x02003596 RID: 13718
				public class BEGIN_POPUP
				{
					// Token: 0x0400D852 RID: 55378
					public static LocString NAME = "Story Trait: Critter Flux-O-Matic";

					// Token: 0x0400D853 RID: 55379
					public static LocString CODEX_NAME = "First Encounter";

					// Token: 0x0400D854 RID: 55380
					public static LocString DESCRIPTION = "I've discovered an experiment designed to analyze the evolutionary dynamics of critter mutation.\n\nOnce it has gathered enough data, it could prove extremely useful for genetic manipulation.";
				}

				// Token: 0x02003597 RID: 13719
				public class END_POPUP
				{
					// Token: 0x0400D855 RID: 55381
					public static LocString NAME = "Story Trait Complete: Critter Flux-O-Matic";

					// Token: 0x0400D856 RID: 55382
					public static LocString CODEX_NAME = "Challenge Completed";

					// Token: 0x0400D857 RID: 55383
					public static LocString DESCRIPTION = "Success! Sufficient samples collected.\n\nI can now trigger genetic deviations in base morphs by sending them through the scanner.\n\nExisting variants can also be scanned, but their genetic makeup is too unstable to tolerate further manipulation.";

					// Token: 0x0400D858 RID: 55384
					public static LocString BUTTON = "Unlock Gene Manipulation Mode";
				}

				// Token: 0x02003598 RID: 13720
				public class UNLOCK_SPECIES_NOTIFICATION
				{
					// Token: 0x0400D859 RID: 55385
					public static LocString NAME = "New Species Scanned";

					// Token: 0x0400D85A RID: 55386
					public static LocString TOOLTIP = "The " + BUILDINGS.PREFABS.GRAVITASCREATUREMANIPULATOR.NAME + " has analyzed these critter species:\n";
				}

				// Token: 0x02003599 RID: 13721
				public class UNLOCK_SPECIES_POPUP
				{
					// Token: 0x0400D85B RID: 55387
					public static LocString NAME = "New Species Scanned";

					// Token: 0x0400D85C RID: 55388
					public static LocString VIEW_IN_CODEX = "Review Data";
				}

				// Token: 0x0200359A RID: 13722
				public class SPECIES_ENTRIES
				{
					// Token: 0x0400D85D RID: 55389
					public static LocString HATCH = "Specimen attempted to snack on the buccal smear. Review data for more information.";

					// Token: 0x0400D85E RID: 55390
					public static LocString LIGHTBUG = "This critter kept trying to befriend the reflective surfaces of the scanner's interior. Review data for more information.";

					// Token: 0x0400D85F RID: 55391
					public static LocString OILFLOATER = "Incessant wriggling made it difficult to scan this critter. Difficult, but not impossible.";

					// Token: 0x0400D860 RID: 55392
					public static LocString DRECKO = "This critter hardly seemed to notice it was being examined at all. Review data for more information.";

					// Token: 0x0400D861 RID: 55393
					public static LocString GLOM = "DNA results confirm: this species is the very definition of \"icky\".";

					// Token: 0x0400D862 RID: 55394
					public static LocString PUFT = "This critter bumped up against the building's interior repeatedly during scanning. Review data for more information.";

					// Token: 0x0400D863 RID: 55395
					public static LocString PACU = "Sample collected. Review data for more information.";

					// Token: 0x0400D864 RID: 55396
					public static LocString MOO = "Whoops! This scanner wasn't designed for critters of these proportions. This organism's genetic makeup will remain shrouded in mystery.";

					// Token: 0x0400D865 RID: 55397
					public static LocString MOLE = "This critter felt right at home in the cramped scanning bed. It can't wait to come back! ";

					// Token: 0x0400D866 RID: 55398
					public static LocString SQUIRREL = "Sample collected. Review data for more information.";

					// Token: 0x0400D867 RID: 55399
					public static LocString CRAB = "Mind the claws! Review data for more information.";

					// Token: 0x0400D868 RID: 55400
					public static LocString DIVERGENTSPECIES = "Specimen responded gently to the probative apparatus, as though being careful not to cause any damage.\n\nReview data for more information.";

					// Token: 0x0400D869 RID: 55401
					public static LocString STATERPILLAR = "Warning: The electrical charge emitted by this specimen nearly short-circuited this building.";

					// Token: 0x0400D86A RID: 55402
					public static LocString BEETA = "Strong collective consciousness detected. Review data for more information.";

					// Token: 0x0400D86B RID: 55403
					public static LocString ICEBELLY = "Whoops! This scanner wasn't designed for critters of these proportions. Fortunately, this critter's thick coat protected the machinery from damage.";

					// Token: 0x0400D86C RID: 55404
					public static LocString SEAL = "Specimen scanned. Review data for more information.";

					// Token: 0x0400D86D RID: 55405
					public static LocString WOODDEER = "This critter seemed amused by the scanning process. Review data for more information.";

					// Token: 0x0400D86E RID: 55406
					public static LocString UNKNOWN_TITLE = "FAILURE TO FLUX: Unknown Species";

					// Token: 0x0400D86F RID: 55407
					public static LocString UNKNOWN = "This species cannot be identified due to a malfunction in the genome-parsing software.\n\nPlease note that kicking the building's exterior is unlikely to correct this issue and may result in permanent damage to the system.";
				}

				// Token: 0x0200359B RID: 13723
				public class SPECIES_ENTRIES_EXPANDED
				{
					// Token: 0x0400D870 RID: 55408
					public static LocString HATCH = "Specimen attempted to snack on the buccal smear. Sample is viable, though the apparatus may be somewhat mangled.\n\nAtomic force microscopy of the bite pattern reveals traces of goethite, a mineral notable for its exceptional strength.";

					// Token: 0x0400D871 RID: 55409
					public static LocString LIGHTBUG = "This critter kept trying to befriend the reflective surfaces of the scanner's interior.\n\nDuring examination, it cycled through a consistent pattern of four rapid flashes of light, a brief pause and two flashes, followed by a longer pause.\n\nIts cells appear to contain a mutated variation of oxyluciferin similar to those catalogued in bioluminescent animals.";

					// Token: 0x0400D872 RID: 55410
					public static LocString OILFLOATER = "Incessant wriggling made it difficult to scan this critter. Difficult, but not impossible.";

					// Token: 0x0400D873 RID: 55411
					public static LocString DRECKO = "This critter hardly seemed to notice it was being examined at all.\n\nThe built-in scanning electron microscope has determined that the fibers on this critter's train grow in a sort of trinity stitch pattern, reminiscent of a well-crafted sweater.\n\nThe critter's leathery skin remains cool and dry, however, likely due to an apparent lack of sweat glands.";

					// Token: 0x0400D874 RID: 55412
					public static LocString GLOM = "DNA results confirm: this species is the scientific definition of \"icky\".";

					// Token: 0x0400D875 RID: 55413
					public static LocString PUFT = "This critter bumped up against the building's interior repeatedly during scanning. Despite this, its skin remains surprisingly free of contusions.\n\nFluorescence imaging reveals extremely low neuronal activity. Was this critter asleep during analysis?";

					// Token: 0x0400D876 RID: 55414
					public static LocString PACU = "This species flopped wildly during analysis. Surfaces that came into contact with its scales now display a thin layer of viscous scum. It does not appear to be corrosive.\n\nInitiating fumigation sequence to neutralize fishy odor.";

					// Token: 0x0400D877 RID: 55415
					public static LocString MOO = "Whoops! This scanner wasn't designed for critters of these proportions. This organism's genetic makeup will remain shrouded in mystery.";

					// Token: 0x0400D878 RID: 55416
					public static LocString MOLE = "This critter felt right at home in the cramped scanning bed. It can't wait to come back! ";

					// Token: 0x0400D879 RID: 55417
					public static LocString SQUIRREL = "This species has a secondary set of inner eyelids that act as a barrier against ocular splinters.\n\nThe surfaces of these secondary eyelids are a translucent blue and display a light crosshatch texture.\n\nThis has broad implications for the critter's vision, meriting further exploration.";

					// Token: 0x0400D87A RID: 55418
					public static LocString CRAB = "This species responded to the hum of the scanner machinery by waving its pincers in gestures that seemed to mimic iconic moves of the disco dance era.\n\nIs it possible that it might have been exposed to music at some point in its evolution?";

					// Token: 0x0400D87B RID: 55419
					public static LocString DIVERGENTSPECIES = "Specimen responded gently to the probative apparatus, as though being careful not to cause any damage.\n\nIt also produced a series of deep, rhythmic vibrations during analysis. An attempt to communicate with the sensors, perhaps?";

					// Token: 0x0400D87C RID: 55420
					public static LocString STATERPILLAR = "Warning: The electrical charge emitted by this specimen nearly short-circuited this building.";

					// Token: 0x0400D87D RID: 55421
					public static LocString BEETA = "This species may not be fully sentient, but it possesses a strong collective consciousness.\n\nIt is unclear how information is communicated between members of the species. What is clear is that knowledge is being shared and passed down from one generation to another.\n\nMonitor closely.";

					// Token: 0x0400D87E RID: 55422
					public static LocString ICEBELLY = "Whoops! This scanner wasn't designed for critters of these proportions. Fortunately, this critter's thick coat protected the machinery from damage.";

					// Token: 0x0400D87F RID: 55423
					public static LocString SEAL = "This critter's pupils appear to be permanently constricted, possibly as a result of long-term exposure to excess illumination.\n\nIts sense of smell is extremely well-developed, however: it immediately identified areas touched by previous species, and marked each one with a small puddle of liquid ethanol.";

					// Token: 0x0400D880 RID: 55424
					public static LocString WOODDEER = "This critter's perpetual grin grew as it observed each step of the process extremely closely.\n\nBehavioral analysis indicates a tendency toward mischief. Close supervision - and minimal access to advanced machinery - is recommended.";

					// Token: 0x0400D881 RID: 55425
					public static LocString UNKNOWN_TITLE = "Unknown Species";

					// Token: 0x0400D882 RID: 55426
					public static LocString UNKNOWN = "FAILURE TO FLUX: This species cannot be identified due to a malfunction in the genome-parsing software.\n\nPlease note that kicking the building's exterior is unlikely to correct this issue and may result in permanent damage to the system.";
				}

				// Token: 0x0200359C RID: 13724
				public class PARKING
				{
					// Token: 0x0400D883 RID: 55427
					public static LocString TITLE = "Parking in Lot D";

					// Token: 0x0400D884 RID: 55428
					public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

					// Token: 0x0200398C RID: 14732
					public class BODY
					{
						// Token: 0x0400E20A RID: 57866
						public static LocString EMAILHEADER1 = "<smallcaps>To: <b>ALL</b><alpha=#AA><size=12></size></color>\nFrom: <b>ADMIN</b><alpha=#AA><size=12> <admin@gravitas.nova></size></color></smallcaps>\n------------------\n";

						// Token: 0x0400E20B RID: 57867
						public static LocString CONTAINER1 = "<indent=5%>Another set of masticated windshield wipers has been discovered in Parking Lot D following the Bioengineering Department's critter enclosure breach last week.\n\nEmployees are strongly encouraged to plug their vehicles in at lots A-C until further notice.\n\nPlease refrain from calling municipal animal control - all critter sightings should be reported directly to Dr. Byron.</indent>";

						// Token: 0x0400E20C RID: 57868
						public static LocString SIGNATURE1 = "\nThank-you,\n-Admin\n<size=11>The Gravitas Facility</size>\n------------------\n";
					}
				}

				// Token: 0x0200359D RID: 13725
				public class WORKIVERSARY
				{
					// Token: 0x0400D885 RID: 55429
					public static LocString TITLE = "Anatomy of a Byron's Hatch";

					// Token: 0x0400D886 RID: 55430
					public static LocString SUBTITLE = " ";

					// Token: 0x0200398D RID: 14733
					public class BODY
					{
						// Token: 0x0400E20D RID: 57869
						public static LocString CONTAINER1 = "Happy 3rd work-iversary, Ada!\n\nI drew this to fill the space left by the cabinet that your chompy critters tore off the wall last week. Hope it's big enough!\n\nI still can't believe they can digest solid steel—you really know how to breed 'em!\n\n- Liam";
					}
				}
			}

			// Token: 0x020028CB RID: 10443
			public static class LONELYMINION
			{
				// Token: 0x0400B318 RID: 45848
				public static LocString NAME = "Mysterious Hermit";

				// Token: 0x0400B319 RID: 45849
				public static LocString DESCRIPTION = "Discover a reclusive character living in a Gravitas relic, and persuade them to join this colony.\n\nRevelations from their past could have far-reaching implications for Duplicants everywhere.\n\nEven their makeshift shelter might be of some use...";

				// Token: 0x0400B31A RID: 45850
				public static LocString DESCRIPTION_SHORT = "Discover a reclusive character living in a Gravitas relic, and persuade them to join this colony.";

				// Token: 0x0400B31B RID: 45851
				public static LocString DESCRIPTION_BUILDINGMENU = "The process of recruiting this building's lone occupant involves the completion of key tasks.";

				// Token: 0x0200359E RID: 13726
				public class KNOCK_KNOCK
				{
					// Token: 0x0400D887 RID: 55431
					public static LocString TEXT = "Knock Knock";

					// Token: 0x0400D888 RID: 55432
					public static LocString TOOLTIP = "Approach this building and welcome its occupant";

					// Token: 0x0400D889 RID: 55433
					public static LocString CANCELTEXT = "Cancel Knock";

					// Token: 0x0400D88A RID: 55434
					public static LocString CANCEL_TOOLTIP = "Leave this building and its occupant alone for now";
				}

				// Token: 0x0200359F RID: 13727
				public class BEGIN_POPUP
				{
					// Token: 0x0400D88B RID: 55435
					public static LocString NAME = "Story Trait: Mysterious Hermit";

					// Token: 0x0400D88C RID: 55436
					public static LocString CODEX_NAME = "First Encounter";

					// Token: 0x0400D88D RID: 55437
					public static LocString DESCRIPTION = "An unfamiliar building has been discovered in my colony. There's movement inside but whoever the inhabitant is, they seem wary of us.\n\nIf we can convince them that we mean no harm, we could very well end up with a fresh recruit <i>and</i> a useful new building.";
				}

				// Token: 0x020035A0 RID: 13728
				public class END_POPUP
				{
					// Token: 0x0400D88E RID: 55438
					public static LocString NAME = "Story Trait Complete: Mysterious Hermit";

					// Token: 0x0400D88F RID: 55439
					public static LocString CODEX_NAME = "Challenge Completed";

					// Token: 0x0400D890 RID: 55440
					public static LocString DESCRIPTION = "My sweet Duplicants' efforts paid off! Our reclusive neighbor has agreed to join the colony.\n\nThe only keepsake he insists on bringing with him is a toolbox which, while rusty, seems to hold great sentimental value.\n\nNow that he'll be living among us, his former home can be deconstructed or repurposed as storage.";

					// Token: 0x0400D891 RID: 55441
					public static LocString BUTTON = "Welcome New Duplicant!";
				}

				// Token: 0x020035A1 RID: 13729
				public class PROGRESSRESPONSE
				{
					// Token: 0x0200398E RID: 14734
					public class STRANGERDANGER
					{
						// Token: 0x0400E20E RID: 57870
						public static LocString NAME = "Stranger Danger";

						// Token: 0x0400E20F RID: 57871
						public static LocString TOOLTIP = "The hermit is suspicious of all outsiders";
					}

					// Token: 0x0200398F RID: 14735
					public class GOODINTRO
					{
						// Token: 0x0400E210 RID: 57872
						public static LocString NAME = "Unconvinced";

						// Token: 0x0400E211 RID: 57873
						public static LocString TOOLTIP = "The hermit is keeping an eye out for more unsolicited overtures";
					}

					// Token: 0x02003990 RID: 14736
					public class ACQUAINTANCE
					{
						// Token: 0x0400E212 RID: 57874
						public static LocString NAME = "Intrigued";

						// Token: 0x0400E213 RID: 57875
						public static LocString TOOLTIP = "The hermit isn't sure why everyone is being so nice";
					}

					// Token: 0x02003991 RID: 14737
					public class GOODNEIGHBOR
					{
						// Token: 0x0400E214 RID: 57876
						public static LocString NAME = "Appreciative";

						// Token: 0x0400E215 RID: 57877
						public static LocString TOOLTIP = "The hermit is developing warm, fuzzy feelings about this colony";
					}

					// Token: 0x02003992 RID: 14738
					public class GREATNEIGHBOR
					{
						// Token: 0x0400E216 RID: 57878
						public static LocString NAME = "Cherished";

						// Token: 0x0400E217 RID: 57879
						public static LocString TOOLTIP = "The hermit is really starting to feel like he might belong here";
					}
				}

				// Token: 0x020035A2 RID: 13730
				public class QUESTCOMPLETE_POPUP
				{
					// Token: 0x0400D892 RID: 55442
					public static LocString NAME = "Hermit Recruitment Progress";

					// Token: 0x0400D893 RID: 55443
					public static LocString VIEW_IN_CODEX = "View File";
				}

				// Token: 0x020035A3 RID: 13731
				public class GIFTRESPONSE_POPUP
				{
					// Token: 0x02003993 RID: 14739
					public class CRAPPYFOOD
					{
						// Token: 0x0400E218 RID: 57880
						public static LocString NAME = "The hermit hated this food";

						// Token: 0x0400E219 RID: 57881
						public static LocString TOOLTIP = "The hermit would rather be launched straight into the sun than eat this slop.\n\nThe mailbox is ready for another delivery";
					}

					// Token: 0x02003994 RID: 14740
					public class TASTYFOOD
					{
						// Token: 0x0400E21A RID: 57882
						public static LocString NAME = "The hermit loved this food";

						// Token: 0x0400E21B RID: 57883
						public static LocString TOOLTIP = "Tastier than the still-warm pretzel that once fell off an unsupervised desk.\n\nThe mailbox is ready for another delivery";
					}

					// Token: 0x02003995 RID: 14741
					public class REPEATEDFOOD
					{
						// Token: 0x0400E21C RID: 57884
						public static LocString NAME = "The hermit is unimpressed";

						// Token: 0x0400E21D RID: 57885
						public static LocString TOOLTIP = "This meal has been offered before.\n\nThe mailbox is ready for another delivery";
					}
				}

				// Token: 0x020035A4 RID: 13732
				public class ANCIENTPODENTRY
				{
					// Token: 0x0400D894 RID: 55444
					public static LocString TITLE = "Recovered Pod Entry #022";

					// Token: 0x0400D895 RID: 55445
					public static LocString SUBTITLE = "<smallcaps>Day: 11/80</smallcaps>\n<smallcaps>Local Time: Hour 7/9</smallcaps>";

					// Token: 0x02003996 RID: 14742
					public class BODY
					{
						// Token: 0x0400E21E RID: 57886
						public static LocString CONTAINER1 = "<indent=%5>Notable improvement to nutrient retention: subjects who participated in the most recent meal intake displayed minimal symptoms of gastrointestinal distress.\n\nMineshaft excavation at Urvara crater resumed following resolution of tunnel wall fracture. Projected time to brine reservoir penetration at current rate: 41 days, local time. Moisture seepage along eastern wall of shaft is being monitored.\n\nNote: Preliminary subsurface temperature data is significantly lower than programmed estimates.</indent>\n------------------\n";
					}
				}

				// Token: 0x020035A5 RID: 13733
				public class CREEPYBASEMENTLAB
				{
					// Token: 0x0400D896 RID: 55446
					public static LocString TITLE = "Debris Analysis";

					// Token: 0x0400D897 RID: 55447
					public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

					// Token: 0x02003997 RID: 14743
					public class BODY
					{
						// Token: 0x0400E21F RID: 57887
						public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: B577, B997, B083, A216]</smallcaps>\n\n[LOG BEGINS]\n\nA216: The Director said there were supposed to be three of you on this task force. Where's the geneticist?\n\nB083: In the bathroom-\n\nB997: He went home.\n\n[long pause]\n\nB997: It's the holidays. He has a family.\n\nA216: We all do. That's exactly why this project is so urgent.\n\nB997: It's not our fault this stuff sat in a subterranean ocean for a year, and took another year to get back to Earth! The microbe samples didn't fare well on the journey, and most of the mechanical components are completely corroded. There's not much to-\n\nB083: -we're analyzing it all and salvaging what we can, Jea- ...Dr. Saruhashi.\n\nA216: Good. And take down those ridiculous lights. This is a lab, not a retro \"shopping mall.\"\n\n[LOG ENDS]\n------------------\n[LOG BEGINS]\n\nB577: Thanks for getting all the debris packed up for disposal.\n\nB997: I thought you did that.\n\nB577: No, I-\n\nB083: Who took my sandwich?\n\nB997: Not this again.\n\nB577: Ren, did you load the shipping container?\n\nB083: Seriously, I haven't eaten in thirteen hours. This isn't funny.\n\nB997: It's a little funny.\n\nB577: Can we focus, please?\n\nB997: Nobody took your sandwich, Rock Doc.\n\nB083: Then why does my food keep going missing?\n\nB997: Maybe the lab ghost took it. Or maybe you just shouldn't leave it out overnight. Gunderson probably thought it was garbage.\n\nB083: He doesn't even clean down here!\n\nB997: Right. Because if he did, I wouldn't have to keep sweeping up the magnesium sulfate deposits that <i>someone</i> keeps tracking all over the floor between shifts.\n\nB083: It's not me!\n\nB577: Listen, I know we're all tired and things have been a little strange. But the sooner we get this sent up to the launchpad, the sooner it starts its trip to the sun and we can all get out of this creepy sub-sub-basement.\n\nB083: Fine.\n\nB997: Fine.\n\nB083: Fine!\n\n[LOG ENDS]\n------------------\n";
					}
				}

				// Token: 0x020035A6 RID: 13734
				public class HOLIDAYCARD
				{
					// Token: 0x0400D898 RID: 55448
					public static LocString TITLE = "Pudding Cups";

					// Token: 0x0400D899 RID: 55449
					public static LocString SUBTITLE = "";

					// Token: 0x02003998 RID: 14744
					public class BODY
					{
						// Token: 0x0400E220 RID: 57888
						public static LocString CONTAINER1 = "Hey kiddo,\n\nWe missed you at your cousin's wedding last weekend. The gift was nice, but the dance floor felt empty without you.\n\nDariush sends his love. He's really turned a corner since he started eating those gooey pudding things you sent over. Any chance you have a version that doesn't smell like feet?\n\nCome home sometime when you're not so busy.\n\n- Baba\n------------------\n";
					}
				}
			}

			// Token: 0x020028CC RID: 10444
			public static class FOSSILHUNT
			{
				// Token: 0x0400B31C RID: 45852
				public static LocString NAME = "Ancient Specimen";

				// Token: 0x0400B31D RID: 45853
				public static LocString DESCRIPTION = "This asteroid has a few skeletons in its geological closet.\n\nTrack down the fossilized fragments of an ancient critter to assemble key pieces of Gravitas history and unlock a new resource.";

				// Token: 0x0400B31E RID: 45854
				public static LocString DESCRIPTION_SHORT = "Track down the fossilized fragments of an ancient critter.";

				// Token: 0x0400B31F RID: 45855
				public static LocString DESCRIPTION_BUILDINGMENU_COVERED = "Unlocking full access to the fossil cache buried beneath the ancient specimen requires excavation of all deposit sites.";

				// Token: 0x0400B320 RID: 45856
				public static LocString DESCRIPTION_REVEALED = "Unlocking full access to the fossil cache buried beneath the ancient specimen requires excavation of all deposit sites.";

				// Token: 0x020035A7 RID: 13735
				public class MISC
				{
					// Token: 0x0400D89A RID: 55450
					public static LocString DECREASE_DECOR_ATTRIBUTE = "Obscured";
				}

				// Token: 0x020035A8 RID: 13736
				public class STATUSITEMS
				{
					// Token: 0x02003999 RID: 14745
					public class FOSSILMINEPENDINGWORK
					{
						// Token: 0x0400E221 RID: 57889
						public static LocString NAME = "Work Errand";

						// Token: 0x0400E222 RID: 57890
						public static LocString TOOLTIP = "Fossil mine will be operated once a Duplicant is available";
					}

					// Token: 0x0200399A RID: 14746
					public class FOSSILIDLE
					{
						// Token: 0x0400E223 RID: 57891
						public static LocString NAME = "No Mining Orders Queued";

						// Token: 0x0400E224 RID: 57892
						public static LocString TOOLTIP = "Select an excavation order to begin mining";
					}

					// Token: 0x0200399B RID: 14747
					public class FOSSILEMPTY
					{
						// Token: 0x0400E225 RID: 57893
						public static LocString NAME = "Waiting For Materials";

						// Token: 0x0400E226 RID: 57894
						public static LocString TOOLTIP = "Mining will begin once materials have been delivered";
					}

					// Token: 0x0200399C RID: 14748
					public class FOSSILENTOMBED
					{
						// Token: 0x0400E227 RID: 57895
						public static LocString NAME = "Entombed";

						// Token: 0x0400E228 RID: 57896
						public static LocString TOOLTIP = "This fossil must be dug out before it can be excavated";

						// Token: 0x0400E229 RID: 57897
						public static LocString LINE_ITEM = "    • Entombed";
					}
				}

				// Token: 0x020035A9 RID: 13737
				public class UISIDESCREENS
				{
					// Token: 0x0400D89B RID: 55451
					public static LocString DIG_SITE_EXCAVATE_BUTTON = "Excavate";

					// Token: 0x0400D89C RID: 55452
					public static LocString DIG_SITE_EXCAVATE_BUTTON_TOOLTIP = "Carefully uncover and examine this fossil";

					// Token: 0x0400D89D RID: 55453
					public static LocString DIG_SITE_CANCEL_EXCAVATION_BUTTON = "Cancel Excavation";

					// Token: 0x0400D89E RID: 55454
					public static LocString DIG_SITE_CANCEL_EXCAVATION_BUTTON_TOOLTIP = "Abandon excavation efforts";

					// Token: 0x0400D89F RID: 55455
					public static LocString MINOR_DIG_SITE_REVEAL_BUTTON = "Main Site";

					// Token: 0x0400D8A0 RID: 55456
					public static LocString MINOR_DIG_SITE_REVEAL_BUTTON_TOOLTIP = "Click to show this site";

					// Token: 0x0400D8A1 RID: 55457
					public static LocString FOSSIL_BITS_EXCAVATE_BUTTON = "Excavate";

					// Token: 0x0400D8A2 RID: 55458
					public static LocString FOSSIL_BITS_EXCAVATE_BUTTON_TOOLTIP = "Carefully uncover and examine this fossil";

					// Token: 0x0400D8A3 RID: 55459
					public static LocString FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON = "Cancel Excavation";

					// Token: 0x0400D8A4 RID: 55460
					public static LocString FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON_TOOLTIP = "Abandon excavation efforts";

					// Token: 0x0400D8A5 RID: 55461
					public static LocString FABRICATOR_LIST_TITLE = "Mining Orders";

					// Token: 0x0400D8A6 RID: 55462
					public static LocString FABRICATOR_RECIPE_SCREEN_TITLE = "Recipe";
				}

				// Token: 0x020035AA RID: 13738
				public class BEGIN_POPUP
				{
					// Token: 0x0400D8A7 RID: 55463
					public static LocString NAME = "Story Trait: Ancient Specimen";

					// Token: 0x0400D8A8 RID: 55464
					public static LocString CODEX_NAME = "First Encounter";

					// Token: 0x0400D8A9 RID: 55465
					public static LocString DESCRIPTION = "I've discovered a fossilized critter buried in my colony—at least, part of one—but it does not resemble any of the species we have encountered on this asteroid.\n\nWhere did it come from? How did it get here? And what other questions might these bones hold the answer to?\n\nThere is only one way to find out.";

					// Token: 0x0400D8AA RID: 55466
					public static LocString BUTTON = "Close";
				}

				// Token: 0x020035AB RID: 13739
				public class END_POPUP
				{
					// Token: 0x0400D8AB RID: 55467
					public static LocString NAME = "Story Trait Complete: Ancient Specimen";

					// Token: 0x0400D8AC RID: 55468
					public static LocString CODEX_NAME = "Challenge Completed";

					// Token: 0x0400D8AD RID: 55469
					public static LocString DESCRIPTION = "My Duplicants have meticulously reassembled as much of the giant critter's scattered remains as they could find.\n\nTheir efforts have unearthed a seemingly bottomless fossil quarry beneath the largest fragment's dig site.\n\nNestled among the topmost bones was a handcrafted critter collar. It's too large to have belonged to any species traditionally categorized as companion animals.";

					// Token: 0x0400D8AE RID: 55470
					public static LocString BUTTON = "Activate Fossil Quarry";
				}

				// Token: 0x020035AC RID: 13740
				public class REWARDS
				{
					// Token: 0x0200399D RID: 14749
					public class MINED_FOSSIL
					{
						// Token: 0x0400E22A RID: 57898
						public static LocString DESC = "Mined " + UI.FormatAsLink("Fossil", "FOSSIL");
					}
				}

				// Token: 0x020035AD RID: 13741
				public class ENTITIES
				{
					// Token: 0x0200399E RID: 14750
					public class FOSSIL_DIG_SITE
					{
						// Token: 0x0400E22B RID: 57899
						public static LocString NAME = "Ancient Specimen";

						// Token: 0x0400E22C RID: 57900
						public static LocString DESC = "Here lies a significant portion of the remains of an enormous, long-dead critter.\n\nIt's not from around here.";
					}

					// Token: 0x0200399F RID: 14751
					public class FOSSIL_RESIN
					{
						// Token: 0x0400E22D RID: 57901
						public static LocString NAME = "Amber Fossil";

						// Token: 0x0400E22E RID: 57902
						public static LocString DESC = "The well-preserved partial remains of a critter of unknown origin.\n\nIt appears to belong to the same ancient specimen found at another site.\n\nThis fragment has been preserved in a resin-like substance.";
					}

					// Token: 0x020039A0 RID: 14752
					public class FOSSIL_ICE
					{
						// Token: 0x0400E22F RID: 57903
						public static LocString NAME = "Frozen Fossil";

						// Token: 0x0400E230 RID: 57904
						public static LocString DESC = "The well-preserved partial remains of a critter of unknown origin.\n\nIt appears to belong to the same ancient specimen found at another site.\n\nThis fragment has been preserved in " + UI.FormatAsLink("Ice", "ICE") + ".";
					}

					// Token: 0x020039A1 RID: 14753
					public class FOSSIL_ROCK
					{
						// Token: 0x0400E231 RID: 57905
						public static LocString NAME = "Petrified Fossil";

						// Token: 0x0400E232 RID: 57906
						public static LocString DESC = "The well-preserved partial remains of a critter of unknown origin.\n\nIt appears to belong to the same ancient specimen found at another site.\n\nThis fragment has been preserved in petrified " + UI.FormatAsLink("Dirt", "DIRT") + ".";
					}

					// Token: 0x020039A2 RID: 14754
					public class FOSSIL_BITS
					{
						// Token: 0x0400E233 RID: 57907
						public static LocString NAME = "Fossil Fragments";

						// Token: 0x0400E234 RID: 57908
						public static LocString DESC = "Bony debris that can be excavated for " + UI.FormatAsLink("Fossil", "FOSSIL") + ".";
					}
				}

				// Token: 0x020035AE RID: 13742
				public class QUEST
				{
					// Token: 0x0400D8AF RID: 55471
					public static LocString LINKED_TOOLTIP = "\n\nClick to show this site";
				}

				// Token: 0x020035AF RID: 13743
				public class ICECRITTERDESIGN
				{
					// Token: 0x0400D8B0 RID: 55472
					public static LocString TITLE = "Organism Design Notes";

					// Token: 0x0400D8B1 RID: 55473
					public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

					// Token: 0x020039A3 RID: 14755
					public class BODY
					{
						// Token: 0x0400E235 RID: 57909
						public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B363]</smallcaps>\n\n[LOG BEGINS]\n\n...Restricting our organism design to specifically target survival in an off-planet polar climate has narrowed our focus significantly, allowing development of this project to rapidly outpace the others.\n\nWe have successfully optimized for adaptive features such as the formation of protective adipose tissue at >40% of the organism's total mass. Dr. Bubare was concerned about the consequences for muscle mass, but results confirm that reductions fall within an acceptable range.\n\nOur next step is to adapt the organism's diet. It would be inadvisable to populate a new colony with carnivorous creatures of this size.\n\n[LOG ENDS]\n------------------\n[LOG BEGINS]\n\n...When I am alone in the lab, I find myself gravitating toward the enclosure to listen to the creature's melodic vocalizations. Sometimes the pitch changes slightly as I approach.\n\nI am not certain what that means.\n\n[LOG ENDS]\n------------------\n";

						// Token: 0x0400E236 RID: 57910
						public static LocString CONTAINER2 = "[LOG BEGINS]\n\n...Some of the other departments have taken to calling our work here \"Project Meat Popsicle\". It is a crass misnomer. This species is not designed to be a food source: it must survive the Ceres climate long enough to establish a stable population that will enable the subsequent settlement party to access the essential research data stored in its DNA via Dr. Winslow's revolutionary genome-encoding technique.\n\nImagine, countless yottabytes' worth of scientific documentation wandering freely around a new colony...the ultimate self-sustaining archive, providing stable data storage that requires zero technological maintenance.\n\nIt gives new meaning to the term, \"living document.\"\n\n[LOG ENDS]\n------------------\n[LOG BEGINS]\n\n...Today is the day. My sonorous critter and her handful of progeny are ready to be transported to their new home. They are scheduled to arrive three months in the past, to ensure that they are well established before the settlement party's arrival next week.\n\nDr. Techna invited me to assist with the teleportation. I was relieved to be too busy to accept. I have heard rumors about previous shipments going awry. These stories are unsubstantiated, and yet...\n\nThe urgency of our mission sometimes necessitates non-ideal compromises.\n\nThe lab is so very quiet now.\n\n[LOG ENDS]\n------------------\n";
					}
				}

				// Token: 0x020035B0 RID: 13744
				public class QUEST_AVAILABLE_NOTIFICATION
				{
					// Token: 0x0400D8B2 RID: 55474
					public static LocString NAME = "Fossil Excavated";

					// Token: 0x0400D8B3 RID: 55475
					public static LocString TOOLTIP = "Additional fossils located";
				}

				// Token: 0x020035B1 RID: 13745
				public class QUEST_AVAILABLE_POPUP
				{
					// Token: 0x0400D8B4 RID: 55476
					public static LocString NAME = "Fossil Excavated";

					// Token: 0x0400D8B5 RID: 55477
					public static LocString CHECK_BUTTON = "View Site";

					// Token: 0x0400D8B6 RID: 55478
					public static LocString DESCRIPTION = "Success! My Duplicants have safely excavated a set of strange, fossilized remains.\n\nIt appears that there are more of this giant critter's bones strewn around the asteroid. It's vital that we reassemble this skeleton for deeper analysis.";
				}

				// Token: 0x020035B2 RID: 13746
				public class UNLOCK_DNADATA_NOTIFICATION
				{
					// Token: 0x0400D8B7 RID: 55479
					public static LocString NAME = "Fossil Data Decoded";

					// Token: 0x0400D8B8 RID: 55480
					public static LocString TOOLTIP = "There was data stored in this fossilized critter's DNA";
				}

				// Token: 0x020035B3 RID: 13747
				public class UNLOCK_DNADATA_POPUP
				{
					// Token: 0x0400D8B9 RID: 55481
					public static LocString NAME = "Data Discovered in Fossil";

					// Token: 0x0400D8BA RID: 55482
					public static LocString VIEW_IN_CODEX = "View Data";
				}

				// Token: 0x020035B4 RID: 13748
				public class DNADATA_ENTRY
				{
					// Token: 0x0400D8BB RID: 55483
					public static LocString TELEPORTFAILURE = "It appears that this creature's DNA was once used as a kind of genetic storage unit.";
				}

				// Token: 0x020035B5 RID: 13749
				public class DNADATA_ENTRY_EXPANDED
				{
					// Token: 0x0400D8BC RID: 55484
					public static LocString TITLE = "SUBJECT: RESETTLEMENT LAUNCH PARTY";

					// Token: 0x0400D8BD RID: 55485
					public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

					// Token: 0x020039A4 RID: 14756
					public class BODY
					{
						// Token: 0x0400E237 RID: 57911
						public static LocString EMAILHEADER = "<smallcaps>To: <b>[REDACTED]</b><alpha=#AA><size=12></size></color>\nFrom: <b>[REDACTED]</b><alpha=#AA></smallcaps>\n------------------\n";

						// Token: 0x0400E238 RID: 57912
						public static LocString CONTAINER1 = "<indent=5%>Dear [REDACTED]\n\nWe are pleased to announce that research objectives for Operation Piazzi's Planet are nearing completion. Thank you all for your patience as we navigated the unprecedented obstacles that such groundbreaking work entails.\n\nWe are aware of rumors regarding documents leaked from Dr. [REDACTED]'s files.\n\nRest assured that the contents of this supposed \"whistleblower\" effort are entirely fabricated—our technology is far too advanced to allow for the type of miscalculation that would result in OPP shipments arriving at their destination some 10,000 years prior to the targeted date.\n\nOur IT security team is currently investigating the document's digital footprint to determine its origin.\n\nTo express our gratitude for your continued support, we would like to invite key stakeholders to a private launch party held at the Gravitas Facility. The evening will be emceed by Dr. Olivia Broussard, who will present our groundbreaking prototypes along with a five-course meal featuring lab-crafted ingredients.\n\nDue to the sensitive nature of our work, we regret that no additional guests or dietary restrictions can be accommodated at this time.\n\nDirector Stern will be hosting a 30-minute Q&A session after dinner. Questions must be submitted at least 24 hours in advance.\n\nQueries about the [REDACTED] papers will be disregarded.\n\nPlease be advised that the contents of this e-mail will expire three minutes from the time of opening.</indent>";

						// Token: 0x0400E239 RID: 57913
						public static LocString SIGNATURE = "\nSincerely,\n[REDACTED]\n<size=11>The Gravitas Facility</size>\n------------------\n";
					}
				}

				// Token: 0x020035B6 RID: 13750
				public class HALLWAYRACES
				{
					// Token: 0x0400D8BE RID: 55486
					public static LocString TITLE = "Unauthorized Activity";

					// Token: 0x0400D8BF RID: 55487
					public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

					// Token: 0x020039A5 RID: 14757
					public class BODY
					{
						// Token: 0x0400E23A RID: 57914
						public static LocString EMAILHEADER = "<smallcaps>To: <b>ALL</b><alpha=#AA><size=12></size></color>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color></smallcaps>\n------------------\n";

						// Token: 0x0400E23B RID: 57915
						public static LocString CONTAINER1 = "<indent=5%>Employees are advised that removing organisms from the bioengineering labs without an approved requisition form is strictly prohibited.\n\nGravitas projects are not designed to be ridden for sport. Injuries sustained during unsanctioned activities are not eligible for coverage under corporate health benefits.\n\nPlease find a comprehensive summary of company regulations attached.\n\n<alpha=#AA>[MISSING ATTACHMENT]</indent>";

						// Token: 0x0400E23C RID: 57916
						public static LocString SIGNATURE = "\nThank-you,\n-Admin\n<size=11>The Gravitas Facility</size>\n------------------\n";
					}
				}
			}

			// Token: 0x020028CD RID: 10445
			public static class MORB_ROVER_MAKER
			{
				// Token: 0x0400B321 RID: 45857
				public static LocString NAME = "Biobot Builder";

				// Token: 0x0400B322 RID: 45858
				public static LocString DESCRIPTION = "Reboot an ambitious collaborative project spearheaded by Gravitas's bioengineering and robotics departments.\n\nIf correctly rebuilt, it could save Duplicant lives.";

				// Token: 0x0400B323 RID: 45859
				public static LocString DESCRIPTION_SHORT = "Reboot an ambitious collaborative project spearheaded by Gravitas's bioengineering and robotics departments.";

				// Token: 0x020035B7 RID: 13751
				public class UI_SIDESCREENS
				{
					// Token: 0x0400D8C0 RID: 55488
					public static LocString DROP_INVENTORY = "Empty Building";

					// Token: 0x0400D8C1 RID: 55489
					public static LocString DROP_INVENTORY_TOOLTIP = string.Concat(new string[]
					{
						"Empties stored ",
						UI.FormatAsLink("Steel", "STEEL"),
						"\n\nDisabling the building will also prevent ",
						UI.FormatAsLink("Steel", "STEEL"),
						" from being delivered"
					});

					// Token: 0x0400D8C2 RID: 55490
					public static LocString REVEAL_BTN = "Restore Building";

					// Token: 0x0400D8C3 RID: 55491
					public static LocString REVEAL_BTN_TOOLTIP = "Assign a Duplicant to restore this building's functionality";

					// Token: 0x0400D8C4 RID: 55492
					public static LocString CANCEL_REVEAL_BTN = "Cancel";

					// Token: 0x0400D8C5 RID: 55493
					public static LocString CANCEL_REVEAL_BTN_TOOLTIP = "Cancel building restoration";
				}

				// Token: 0x020035B8 RID: 13752
				public class POPUPS
				{
					// Token: 0x020039A6 RID: 14758
					public class BEGIN
					{
						// Token: 0x0400E23D RID: 57917
						public static LocString NAME = "Story Trait: Biobot Builder";

						// Token: 0x0400E23E RID: 57918
						public static LocString CODEX_NAME = "First Encounter";

						// Token: 0x0400E23F RID: 57919
						public static LocString DESCRIPTION = "My Duplicants have discovered a laboratory full of dusty machinery. The vestiges of another colony's experiments, perhaps?\n\nIt is unclear whether the apparatus is intended for biological experimentation or advanced mechatronics...or both.";

						// Token: 0x0400E240 RID: 57920
						public static LocString BUTTON = "Close";
					}

					// Token: 0x020039A7 RID: 14759
					public class REVEAL
					{
						// Token: 0x0400E241 RID: 57921
						public static LocString NAME = "Story Trait: Biobot Builder";

						// Token: 0x0400E242 RID: 57922
						public static LocString CODEX_NAME = "Meet P.E.G.G.Y.";

						// Token: 0x0400E243 RID: 57923
						public static LocString DESCRIPTION = "Our restoration work is complete!\n\nA small plaque on this building's mechanical assembly tank reads: \"Pathogen-Fueled Extravehicular Geo-Exploratory Guidebot (Y).\"\n\nThe adjacent tank contains the floating shape of a half-formed organism. Its vivid coloring reminds me of the poisonous amphibians that were eradicated from our home planet's jungles.\n\nA tattered transcript print-out was recovered from the mess.";

						// Token: 0x0400E244 RID: 57924
						public static LocString BUTTON_CLOSE = "Close";

						// Token: 0x0400E245 RID: 57925
						public static LocString BUTTON_READLORE = "Read Transcript";
					}

					// Token: 0x020039A8 RID: 14760
					public class LOCKER
					{
						// Token: 0x0400E246 RID: 57926
						public static LocString DESCRIPTION = "A hermetically sealed glass cabinet.\n\nIt contains two " + UI.FormatAsLink("Sporechid", "EVILFLOWER") + " seeds and a carefully penned note.";
					}

					// Token: 0x020039A9 RID: 14761
					public class END
					{
						// Token: 0x0400E247 RID: 57927
						public static LocString NAME = "Story Trait Complete: Biobot Builder";

						// Token: 0x0400E248 RID: 57928
						public static LocString CODEX_NAME = "Challenge Completed";

						// Token: 0x0400E249 RID: 57929
						public static LocString DESCRIPTION = "Success! My Duplicants' efforts to get the Biobot Builder up and running have finally paid off!\n\nOur first fully assembled P.E.G.G.Y. biobot is ready to perform tasks in hazardous environments, which means less exposure to danger for my Duplicants. There seems to be no limit to the number of biobots that we could produce.\n\nA small toy bot was found discarded behind the Sporb tank. It occasionally plays a deteriorated laugh track.";

						// Token: 0x0400E24A RID: 57930
						public static LocString BUTTON = "Close";

						// Token: 0x0400E24B RID: 57931
						public static LocString BUTTON_READLORE = "Inspect Toy";
					}
				}

				// Token: 0x020035B9 RID: 13753
				public class ENVELOPE
				{
					// Token: 0x0400D8C6 RID: 55494
					public static LocString TITLE = "With Regrets";

					// Token: 0x020039AA RID: 14762
					public class BODY
					{
						// Token: 0x0400E24C RID: 57932
						public static LocString CONTAINER1 = "Dr. Seyed Ali,\n\nYou were right to be angry with me. I <i>am</i> the reason that the driverless workbot project was reassigned. Director Stern called me in to discuss your concerns regarding the Sporb mucin cross-contamination, and I...\n\nShe said the supplemental testing on model X posed a threat to the Ceres mission.\n\nAfter what happened to that poor lab tech, I should have said more, but...\n\nIt was already too late for him.\n\nIt may be too late for all of us.\n\nYou should know that the Director received a video call from someone at the Vertex Institute as I left... I lingered outside her door and heard her address them as the head of transnational security! The way they were talking about the biobot...\n\nIt's not safe to write more here. I'll wait for you at the rocket hangar after your shift tonight.\n\nI hope you'll come. I understand if you don't.\n\nI am so, so sorry.\n\n - Dr. Saruhashi";
					}
				}

				// Token: 0x020035BA RID: 13754
				public class VALENTINESDAY
				{
					// Token: 0x0400D8C7 RID: 55495
					public static LocString TITLE = "Anonymous Admirer";

					// Token: 0x020039AB RID: 14763
					public class BODY
					{
						// Token: 0x0400E24D RID: 57933
						public static LocString CONTAINER1 = "I am\n   a subatomic particle\nsmaller than a speck of dust\n  flushed from your gaze\n\n     at the eyewash station  \n\n   My love is like plutonium\n gray and dull and\nunbearably heavy\n  until    I am near you\n\n with every breath \n      I burn, with\n    yearning\n              unseen\n\nPS: I made Steve let me in so I could leave you this, hope that's okay.";
					}
				}

				// Token: 0x020035BB RID: 13755
				public class UNSAFETRANSFER
				{
					// Token: 0x0400D8C8 RID: 55496
					public static LocString TITLE = "ENCRYPTION LEVEL: THREE";

					// Token: 0x020039AC RID: 14764
					public class BODY
					{
						// Token: 0x0400E24E RID: 57934
						public static LocString CONTAINER1 = "<smallcaps>[Log Fragmentation Detected]\n[Voice Recognition Unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...and then the Printing Pod says \"Knock knock, goo's there!\"\n\nUgh. They'll never laugh at <i>that</i> stinker.\n\nWhat if-\n\n(sound of a ding)\n\nHey hey, squishy little buddy! Look who's all grown up. You ready for a big robot ride? Dr. Seyed Ali should be back from his meeting any minute. He'll be so happy to see you.\n\n(sound of a wet slap on glass)\n\nAww yeah, I'd be impatient too.\n\nYou know what, why don't I go ahead and get you into your new home? I've helped him do this more than a dozen times.\n\n\"See one, do one, teach one,\" right?\n\n[LOG ENDS]";
					}
				}

				// Token: 0x020035BC RID: 13756
				public class STATUSITEMS
				{
					// Token: 0x020039AD RID: 14765
					public class DUSTY
					{
						// Token: 0x0400E24F RID: 57935
						public static LocString NAME = "Decommissioned";

						// Token: 0x0400E250 RID: 57936
						public static LocString TOOLTIP = "This building must be restored before it can be used";
					}

					// Token: 0x020039AE RID: 14766
					public class BUILDING_BEING_REVEALED
					{
						// Token: 0x0400E251 RID: 57937
						public static LocString NAME = "Being Restored";

						// Token: 0x0400E252 RID: 57938
						public static LocString TOOLTIP = "This building is being restored to its former glory";
					}

					// Token: 0x020039AF RID: 14767
					public class BUILDING_REVEALING
					{
						// Token: 0x0400E253 RID: 57939
						public static LocString NAME = "Restoring Equipment";

						// Token: 0x0400E254 RID: 57940
						public static LocString TOOLTIP = "This Duplicant is carefully restoring the Biobot Builder";
					}

					// Token: 0x020039B0 RID: 14768
					public class GERM_COLLECTION_PROGRESS
					{
						// Token: 0x0400E255 RID: 57941
						public static LocString NAME = "Incubating Sporb: {0}";

						// Token: 0x0400E256 RID: 57942
						public static LocString TOOLTIP = "At 100% incubation, the Sporb begins to convert absorbed {GERM_NAME} into photosynthetic bacteria that can be used as biofuel\n\nIt is then ready to be assessed and transferred into a completed Biobot frame\n\nConsumption Rate: {0} [{GERM_NAME}]\n\nCurrent Total: {1} / {2} [{GERM_NAME}]";
					}

					// Token: 0x020039B1 RID: 14769
					public class NOGERMSCONSUMEDALERT
					{
						// Token: 0x0400E257 RID: 57943
						public static LocString NAME = "Insufficient Resources: {0}";

						// Token: 0x0400E258 RID: 57944
						public static LocString TOOLTIP = "This building requires additional {0} in order to function\n\n{0} can be delivered via " + BUILDINGS.PREFABS.GASCONDUIT.NAME + " ";
					}

					// Token: 0x020039B2 RID: 14770
					public class CRAFTING_ROBOT_BODY
					{
						// Token: 0x0400E259 RID: 57945
						public static LocString NAME = "Crafting Biobot";

						// Token: 0x0400E25A RID: 57946
						public static LocString TOOLTIP = "This building is using " + UI.FormatAsLink("Steel", "STEEL") + " to craft a Biobot frame";
					}

					// Token: 0x020039B3 RID: 14771
					public class DOCTOR_READY
					{
						// Token: 0x0400E25B RID: 57947
						public static LocString NAME = "Awaiting Doctor";

						// Token: 0x0400E25C RID: 57948
						public static LocString TOOLTIP = "This building is waiting for a skilled Duplicant to perform an occupational health and safety check";
					}

					// Token: 0x020039B4 RID: 14772
					public class BUILDING_BEING_WORKED_BY_DOCTOR
					{
						// Token: 0x0400E25D RID: 57949
						public static LocString NAME = "Preparing Biobot";

						// Token: 0x0400E25E RID: 57950
						public static LocString TOOLTIP = "This building is being operated by a skilled Duplicant";
					}

					// Token: 0x020039B5 RID: 14773
					public class DOCTOR_WORKING_BUILDING
					{
						// Token: 0x0400E25F RID: 57951
						public static LocString NAME = "Assessing Sporb";

						// Token: 0x0400E260 RID: 57952
						public static LocString TOOLTIP = "This Duplicant is assessing the Sporb's readiness for Biobot assembly";
					}
				}
			}
		}

		// Token: 0x0200201B RID: 8219
		public class QUESTS
		{
			// Token: 0x020028CE RID: 10446
			public class KNOCKQUEST
			{
				// Token: 0x0400B324 RID: 45860
				public static LocString NAME = "Greet Occupant";

				// Token: 0x0400B325 RID: 45861
				public static LocString COMPLETE = "Initial contact was a success! Our new neighbor seems friendly, though extremely shy.\n\nThey'll need a little more coaxing before they're ready to join my colony.";
			}

			// Token: 0x020028CF RID: 10447
			public class FOODQUEST
			{
				// Token: 0x0400B326 RID: 45862
				public static LocString NAME = "Welcome Dinner";

				// Token: 0x0400B327 RID: 45863
				public static LocString COMPLETE = "Success! My Duplicants' cooking has whetted the hermit's appetite for communal living.\n\nThey've also found what appears to be a page from an old logbook tucked behind the mailbox.";
			}

			// Token: 0x020028D0 RID: 10448
			public class PLUGGEDIN
			{
				// Token: 0x0400B328 RID: 45864
				public static LocString NAME = "On the Grid";

				// Token: 0x0400B329 RID: 45865
				public static LocString COMPLETE = "Success! The hermit is very excited about being on the grid.\n\nThe bright lights illuminate an unfamiliar file on the ground nearby.";
			}

			// Token: 0x020028D1 RID: 10449
			public class HIGHDECOR
			{
				// Token: 0x0400B32A RID: 45866
				public static LocString NAME = "Nice Neighborhood";

				// Token: 0x0400B32B RID: 45867
				public static LocString COMPLETE = "Success! All this excellent decor is really making the hermit feel at home.\n\nHe scrawled a thank-you note on the back of an old holiday card.";
			}

			// Token: 0x020028D2 RID: 10450
			public class FOSSILHUNTQUEST
			{
				// Token: 0x0400B32C RID: 45868
				public static LocString NAME = "Scattered Fragments";

				// Token: 0x0400B32D RID: 45869
				public static LocString COMPLETE = "Each of the fossil deposits on this asteroid has been excavated, and its contents safely retrieved.\n\nThe ancient specimen's deeper cache of fossil can now be mined.";
			}

			// Token: 0x020028D3 RID: 10451
			public class CRITERIA
			{
				// Token: 0x020035BD RID: 13757
				public class NEIGHBOR
				{
					// Token: 0x0400D8C9 RID: 55497
					public static LocString NAME = "Knock on door";

					// Token: 0x0400D8CA RID: 55498
					public static LocString TOOLTIP = "Send a Duplicant over to introduce themselves and discover what it'll take to turn this stranger into a friend";
				}

				// Token: 0x020035BE RID: 13758
				public class DECOR
				{
					// Token: 0x0400D8CB RID: 55499
					public static LocString NAME = "Improve nearby Decor";

					// Token: 0x0400D8CC RID: 55500
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Establish average ",
						UI.PRE_KEYWORD,
						"Decor",
						UI.PST_KEYWORD,
						" of {0} or higher for the area surrounding this building\n\nAverage Decor: {1:0.##}"
					});
				}

				// Token: 0x020035BF RID: 13759
				public class SUPPLIEDPOWER
				{
					// Token: 0x0400D8CD RID: 55501
					public static LocString NAME = "Turn on festive lights";

					// Token: 0x0400D8CE RID: 55502
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Connect this building to ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						" long enough to cheer up its occupant\n\nTime Remaining: {0}s"
					});
				}

				// Token: 0x020035C0 RID: 13760
				public class FOODQUALITY
				{
					// Token: 0x0400D8CF RID: 55503
					public static LocString NAME = "Deliver Food to the mailbox";

					// Token: 0x0400D8D0 RID: 55504
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Deliver 3 unique ",
						UI.PRE_KEYWORD,
						"Food",
						UI.PST_KEYWORD,
						" items. Quality must be {0} or higher\n\nFoods Delivered:\n{1}"
					});

					// Token: 0x0400D8D1 RID: 55505
					public static LocString NONE = "None";
				}

				// Token: 0x020035C1 RID: 13761
				public class LOSTSPECIMEN
				{
					// Token: 0x0400D8D2 RID: 55506
					public static LocString NAME = UI.FormatAsLink("Ancient Specimen", "MOVECAMERATOFossilDig");

					// Token: 0x0400D8D3 RID: 55507
					public static LocString TOOLTIP = "Retrieve the largest deposit of the ancient critter's remains";

					// Token: 0x0400D8D4 RID: 55508
					public static LocString NONE = "None";
				}

				// Token: 0x020035C2 RID: 13762
				public class LOSTICEFOSSIL
				{
					// Token: 0x0400D8D5 RID: 55509
					public static LocString NAME = UI.FormatAsLink("Frozen Fossil", "MOVECAMERATOFossilIce");

					// Token: 0x0400D8D6 RID: 55510
					public static LocString TOOLTIP = "Retrieve a piece of the ancient critter that has been preserved in " + UI.PRE_KEYWORD + "Ice" + UI.PST_KEYWORD;

					// Token: 0x0400D8D7 RID: 55511
					public static LocString NONE = "None";
				}

				// Token: 0x020035C3 RID: 13763
				public class LOSTRESINFOSSIL
				{
					// Token: 0x0400D8D8 RID: 55512
					public static LocString NAME = UI.FormatAsLink("Amber Fossil", "MOVECAMERATOFossilResin");

					// Token: 0x0400D8D9 RID: 55513
					public static LocString TOOLTIP = "Retrieve a piece of the ancient critter that has been preserved in a strangely resin-like substance";

					// Token: 0x0400D8DA RID: 55514
					public static LocString NONE = "None";
				}

				// Token: 0x020035C4 RID: 13764
				public class LOSTROCKFOSSIL
				{
					// Token: 0x0400D8DB RID: 55515
					public static LocString NAME = UI.FormatAsLink("Petrified Fossil", "MOVECAMERATOFossilRock");

					// Token: 0x0400D8DC RID: 55516
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Retrieve a piece of the ancient critter that has been preserved in ",
						UI.PRE_KEYWORD,
						"Rock",
						UI.PST_KEYWORD,
						" "
					});

					// Token: 0x0400D8DD RID: 55517
					public static LocString NONE = "None";
				}
			}
		}

		// Token: 0x0200201C RID: 8220
		public class HEADQUARTERS
		{
			// Token: 0x04009210 RID: 37392
			public static LocString TITLE = "Printing Pod";

			// Token: 0x020028D4 RID: 10452
			public class BODY
			{
				// Token: 0x0400B32E RID: 45870
				public static LocString CONTAINER1 = "An advanced 3D printer developed by the Gravitas Facility.\n\nThe Printing Pod is notable for its ability to print living organic material from biological blueprints.\n\nIt is capable of synthesizing its own organic material for printing, and contains an almost unfathomable amount of stored energy, allowing it to autonomously print every 3 cycles.";

				// Token: 0x0400B32F RID: 45871
				public static LocString CONTAINER2 = "";
			}
		}

		// Token: 0x0200201D RID: 8221
		public class HEADERS
		{
			// Token: 0x04009211 RID: 37393
			public static LocString FABRICATIONS = "All Recipes";

			// Token: 0x04009212 RID: 37394
			public static LocString RECEPTACLE = "Farmable Plants";

			// Token: 0x04009213 RID: 37395
			public static LocString RECIPE = "Recipe Ingredients";

			// Token: 0x04009214 RID: 37396
			public static LocString USED_IN_RECIPES = "Ingredient In";

			// Token: 0x04009215 RID: 37397
			public static LocString TECH_UNLOCKS = "Unlocks";

			// Token: 0x04009216 RID: 37398
			public static LocString PREREQUISITE_TECH = "Prerequisite Tech";

			// Token: 0x04009217 RID: 37399
			public static LocString PREREQUISITE_ROLES = "Prerequisite Jobs";

			// Token: 0x04009218 RID: 37400
			public static LocString UNLOCK_ROLES = "Promotion Opportunities";

			// Token: 0x04009219 RID: 37401
			public static LocString UNLOCK_ROLES_DESC = "Promotions introduce further stat boosts and traits that stack with existing Job Training.";

			// Token: 0x0400921A RID: 37402
			public static LocString ROLE_PERKS = "Job Training";

			// Token: 0x0400921B RID: 37403
			public static LocString ROLE_PERKS_DESC = "Job Training automatically provides permanent traits and stat increases that are retained even when a Duplicant switches jobs.";

			// Token: 0x0400921C RID: 37404
			public static LocString DIET = "Diet";

			// Token: 0x0400921D RID: 37405
			public static LocString PRODUCES = "Excretes";

			// Token: 0x0400921E RID: 37406
			public static LocString HATCHESFROMEGG = "Hatched from";

			// Token: 0x0400921F RID: 37407
			public static LocString GROWNFROMSEED = "Grown from";

			// Token: 0x04009220 RID: 37408
			public static LocString BUILDINGEFFECTS = "Effects";

			// Token: 0x04009221 RID: 37409
			public static LocString BUILDINGREQUIREMENTS = "Requirements";

			// Token: 0x04009222 RID: 37410
			public static LocString BUILDINGCONSTRUCTIONPROPS = "Construction Properties";

			// Token: 0x04009223 RID: 37411
			public static LocString BUILDINGCONSTRUCTIONMATERIALS = "Materials: ";

			// Token: 0x04009224 RID: 37412
			public static LocString BUILDINGTYPE = "<b>Category</b>";

			// Token: 0x04009225 RID: 37413
			public static LocString SUBENTRIES = "Entries ({0}/{1})";

			// Token: 0x04009226 RID: 37414
			public static LocString COMFORTRANGE = "Ideal Temperatures";

			// Token: 0x04009227 RID: 37415
			public static LocString ELEMENTTRANSITIONS = "Additional States";

			// Token: 0x04009228 RID: 37416
			public static LocString ELEMENTTRANSITIONSTO = "Transitions To";

			// Token: 0x04009229 RID: 37417
			public static LocString ELEMENTTRANSITIONSFROM = "Transitions From";

			// Token: 0x0400922A RID: 37418
			public static LocString ELEMENTCONSUMEDBY = "Applications";

			// Token: 0x0400922B RID: 37419
			public static LocString ELEMENTPRODUCEDBY = "Produced By";

			// Token: 0x0400922C RID: 37420
			public static LocString MATERIALUSEDTOCONSTRUCT = "Construction Uses";

			// Token: 0x0400922D RID: 37421
			public static LocString SECTION_UNLOCKABLES = "Undiscovered Data";

			// Token: 0x0400922E RID: 37422
			public static LocString CONTENTLOCKED = "Undiscovered";

			// Token: 0x0400922F RID: 37423
			public static LocString CONTENTLOCKED_SUBTITLE = "More research or exploration is required";

			// Token: 0x04009230 RID: 37424
			public static LocString INTERNALBATTERY = "Battery";

			// Token: 0x04009231 RID: 37425
			public static LocString INTERNALSTORAGE = "Storage";

			// Token: 0x04009232 RID: 37426
			public static LocString CRITTERMAXAGE = "Life Span";

			// Token: 0x04009233 RID: 37427
			public static LocString CRITTEROVERCROWDING = "Space Required";

			// Token: 0x04009234 RID: 37428
			public static LocString CRITTERDROPS = "Drops";

			// Token: 0x04009235 RID: 37429
			public static LocString FOODEFFECTS = "Nutritional Effects";

			// Token: 0x04009236 RID: 37430
			public static LocString FOODSWITHEFFECT = "Foods with this effect";

			// Token: 0x04009237 RID: 37431
			public static LocString EQUIPMENTEFFECTS = "Effects";
		}

		// Token: 0x0200201E RID: 8222
		public class FORMAT_STRINGS
		{
			// Token: 0x04009238 RID: 37432
			public static LocString TEMPERATURE_OVER = "Temperature over {0}";

			// Token: 0x04009239 RID: 37433
			public static LocString TEMPERATURE_UNDER = "Temperature under {0}";

			// Token: 0x0400923A RID: 37434
			public static LocString CONSTRUCTION_TIME = "Build Time: {0} seconds";

			// Token: 0x0400923B RID: 37435
			public static LocString BUILDING_SIZE = "Building Size: {0} wide x {1} high";

			// Token: 0x0400923C RID: 37436
			public static LocString MATERIAL_MASS = "{0} {1}";

			// Token: 0x0400923D RID: 37437
			public static LocString TRANSITION_LABEL_TO_ONE_ELEMENT = "{0} to {1}";

			// Token: 0x0400923E RID: 37438
			public static LocString TRANSITION_LABEL_TO_TWO_ELEMENTS = "{0} to {1} and {2}";
		}

		// Token: 0x0200201F RID: 8223
		public class CREATURE_DESCRIPTORS
		{
			// Token: 0x0400923F RID: 37439
			public static LocString MAXAGE = "This critter's typical " + UI.FormatAsLink("Life Span", "CREATURES::GUIDE::FERTILITY") + " is <b>{0} cycles</b>.";

			// Token: 0x04009240 RID: 37440
			public static LocString OVERCROWDING = UI.FormatAsLink("Crowded", "CREATURES::GUIDE::MOOD") + " when a room has less than <b>{0} cells</b> of space for each critter.";

			// Token: 0x04009241 RID: 37441
			public static LocString CONFINED = UI.FormatAsLink("Confined", "CREATURES::GUIDE::MOOD") + " when a room is smaller than <b>{0} cells</b>.";

			// Token: 0x04009242 RID: 37442
			public static LocString NON_LETHAL_RANGE = "Livable range: <b>{0}</b> to <b>{1}</b>";

			// Token: 0x020028D5 RID: 10453
			public class TEMPERATURE
			{
				// Token: 0x0400B330 RID: 45872
				public static LocString COMFORT_RANGE = "Comfort range: <b>{0}</b> to <b>{1}</b>";

				// Token: 0x0400B331 RID: 45873
				public static LocString NON_LETHAL_RANGE = "Livable range: <b>{0}</b> to <b>{1}</b>";
			}
		}

		// Token: 0x02002020 RID: 8224
		public class ROBOT_DESCRIPTORS
		{
			// Token: 0x020028D6 RID: 10454
			public class BATTERY
			{
				// Token: 0x0400B332 RID: 45874
				public static LocString CAPACITY = "Battery capacity: <b>{0}" + UI.UNITSUFFIXES.ELECTRICAL.JOULE + "</b>";
			}

			// Token: 0x020028D7 RID: 10455
			public class STORAGE
			{
				// Token: 0x0400B333 RID: 45875
				public static LocString CAPACITY = "Internal storage: <b>{0}" + UI.UNITSUFFIXES.MASS.KILOGRAM + "</b>";
			}
		}

		// Token: 0x02002021 RID: 8225
		public class PAGENOTFOUND
		{
			// Token: 0x04009243 RID: 37443
			public static LocString TITLE = "Data Not Found";

			// Token: 0x04009244 RID: 37444
			public static LocString SUBTITLE = "This database entry is under construction or unavailable";

			// Token: 0x04009245 RID: 37445
			public static LocString BODY = "";
		}

		// Token: 0x02002022 RID: 8226
		public class ROOM_REQUIREMENT_CLASS
		{
			// Token: 0x04009246 RID: 37446
			public static LocString NAME = "Category";

			// Token: 0x020028D8 RID: 10456
			public class SHARED
			{
				// Token: 0x0400B334 RID: 45876
				public static LocString BUILDINGS_LIST_TITLE = "Buildings in this category:";

				// Token: 0x0400B335 RID: 45877
				public static LocString ROOMS_REQUIRED_LIST_TITLE = "Required in:";

				// Token: 0x0400B336 RID: 45878
				public static LocString ROOMS_CONFLICT_LIST_TITLE = "Conflicts with:";
			}

			// Token: 0x020028D9 RID: 10457
			public class INDUSTRIALMACHINERY
			{
				// Token: 0x0400B337 RID: 45879
				public static LocString TITLE = "Industrial Machinery";

				// Token: 0x0400B338 RID: 45880
				public static LocString DESCRIPTION = "Buildings that generate power, manufacture equipment, refine resources, and provide other fundamental colony requirements.";

				// Token: 0x0400B339 RID: 45881
				public static LocString FLAVOUR = "";

				// Token: 0x0400B33A RID: 45882
				public static LocString CONFLICTINGROOMS = string.Concat(new string[]
				{
					"    • ",
					UI.FormatAsLink("Latrine", "LATRINE"),
					"\n    • ",
					UI.FormatAsLink("Washroom", "PLUMBEDBATHROOM"),
					"\n    • ",
					UI.FormatAsLink("Barracks", "BARRACKS"),
					"\n    • ",
					UI.FormatAsLink("Luxury Barracks", "BEDROOM"),
					"\n    • ",
					UI.FormatAsLink("Private Bedroom", "PRIVATE BEDROOM"),
					"\n    • ",
					UI.FormatAsLink("Mess Hall", "MESSHALL"),
					"\n    • ",
					UI.FormatAsLink("Great Hall", "GREATHALL"),
					"\n    • ",
					UI.FormatAsLink("Massage Clinic", "MASSAGE_CLINIC"),
					"\n    • ",
					UI.FormatAsLink("Hospital", "HOSPITAL"),
					"\n    • ",
					UI.FormatAsLink("Laboratory", "LABORATORY"),
					"\n    • ",
					UI.FormatAsLink("Recreation Room", "REC_ROOM")
				});
			}

			// Token: 0x020028DA RID: 10458
			public class RECBUILDING
			{
				// Token: 0x0400B33B RID: 45883
				public static LocString TITLE = "Recreational Buildings";

				// Token: 0x0400B33C RID: 45884
				public static LocString DESCRIPTION = "Buildings that provide essential support for fragile Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";

				// Token: 0x0400B33D RID: 45885
				public static LocString FLAVOUR = "";

				// Token: 0x0400B33E RID: 45886
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Great Hall", "GREATHALL") + " \n    • " + UI.FormatAsLink("Recreation Room", "REC_ROOM");
			}

			// Token: 0x020028DB RID: 10459
			public class CLINIC
			{
				// Token: 0x0400B33F RID: 45887
				public static LocString TITLE = "Medical Equipment";

				// Token: 0x0400B340 RID: 45888
				public static LocString DESCRIPTION = "Buildings designed to help sick Duplicants heal and minimize the spread of " + UI.FormatAsLink("Disease", "DISEASE") + ".";

				// Token: 0x0400B341 RID: 45889
				public static LocString FLAVOUR = "";

				// Token: 0x0400B342 RID: 45890
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Hospital", "HOSPITAL");
			}

			// Token: 0x020028DC RID: 10460
			public class WASHSTATION
			{
				// Token: 0x0400B343 RID: 45891
				public static LocString TITLE = "Wash Stations";

				// Token: 0x0400B344 RID: 45892
				public static LocString DESCRIPTION = "Buildings that remove " + UI.FormatAsLink("disease", "DISEASE") + "-spreading germs from Duplicant bodies. Not all wash stations require plumbing.";

				// Token: 0x0400B345 RID: 45893
				public static LocString FLAVOUR = "";

				// Token: 0x0400B346 RID: 45894
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Latrine", "LATRINE");
			}

			// Token: 0x020028DD RID: 10461
			public class ADVANCEDWASHSTATION
			{
				// Token: 0x0400B347 RID: 45895
				public static LocString TITLE = "Plumbed Wash Stations";

				// Token: 0x0400B348 RID: 45896
				public static LocString DESCRIPTION = "Buildings that require plumbing in order to remove " + UI.FormatAsLink("disease", "DISEASE") + "-spreading germs from Duplicant bodies.";

				// Token: 0x0400B349 RID: 45897
				public static LocString FLAVOUR = "";

				// Token: 0x0400B34A RID: 45898
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Washroom", "PLUMBEDBATHROOM");
			}

			// Token: 0x020028DE RID: 10462
			public class TOILETTYPE
			{
				// Token: 0x0400B34B RID: 45899
				public static LocString TITLE = "Toilets";

				// Token: 0x0400B34C RID: 45900
				public static LocString DESCRIPTION = "Buildings that give Duplicants a sanitary and dignified place to conduct essential \"business.\"";

				// Token: 0x0400B34D RID: 45901
				public static LocString FLAVOUR = "";

				// Token: 0x0400B34E RID: 45902
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Latrine", "LATRINE") + "\n    • " + UI.FormatAsLink("Hospital", "HOSPITAL");
			}

			// Token: 0x020028DF RID: 10463
			public class FLUSHTOILETTYPE
			{
				// Token: 0x0400B34F RID: 45903
				public static LocString TITLE = UI.FormatAsLink("Flush Toilets", "FLUSHTOILETTYPE");

				// Token: 0x0400B350 RID: 45904
				public static LocString DESCRIPTION = "Buildings that give Duplicants a sanitary and dignified place to conduct essential \"business\"...and then flush away the evidence.";

				// Token: 0x0400B351 RID: 45905
				public static LocString FLAVOUR = "";

				// Token: 0x0400B352 RID: 45906
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Washroom", "PLUMBEDBATHROOM");
			}

			// Token: 0x020028E0 RID: 10464
			public class SCIENCEBUILDING
			{
				// Token: 0x0400B353 RID: 45907
				public static LocString TITLE = "Science Buildings";

				// Token: 0x0400B354 RID: 45908
				public static LocString DESCRIPTION = "Buildings that allow Duplicants to learn about the world around them, and beyond.";

				// Token: 0x0400B355 RID: 45909
				public static LocString FLAVOUR = "";

				// Token: 0x0400B356 RID: 45910
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Laboratory", "LABORATORY");
			}

			// Token: 0x020028E1 RID: 10465
			public class DECORATION
			{
				// Token: 0x0400B357 RID: 45911
				public static LocString TITLE = "Decor Items";

				// Token: 0x0400B358 RID: 45912
				public static LocString DESCRIPTION = "Buildings that give the colony a valuable aesthetic boost, and allow Duplicants to express themselves creatively.\n\nSome rooms require Fancy Decor items, which contribute extra-high levels of aesthetic enhancement.";

				// Token: 0x0400B359 RID: 45913
				public static LocString FLAVOUR = "";

				// Token: 0x0400B35A RID: 45914
				public static LocString ROOMSREQUIRING = string.Concat(new string[]
				{
					"    • ",
					UI.FormatAsLink("Luxury Barracks", "BEDROOM"),
					"\n    • ",
					UI.FormatAsLink("Private Bedroom", "PRIVATE BEDROOM"),
					"\n    • ",
					UI.FormatAsLink("Great Hall", "GREATHALL"),
					"\n    • ",
					UI.FormatAsLink("Massage Clinic", "MASSAGECLINIC"),
					"\n    • ",
					UI.FormatAsLink("Recreation Room", "REC_ROOM")
				});
			}

			// Token: 0x020028E2 RID: 10466
			public class RANCHSTATIONTYPE
			{
				// Token: 0x0400B35B RID: 45915
				public static LocString TITLE = "Ranching Buildings";

				// Token: 0x0400B35C RID: 45916
				public static LocString DESCRIPTION = "Buildings dedicated to " + UI.FormatAsLink("Critter", "CREATURES") + " husbandry.";

				// Token: 0x0400B35D RID: 45917
				public static LocString FLAVOUR = "";

				// Token: 0x0400B35E RID: 45918
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Stable", "CREATUREPEN");
			}

			// Token: 0x020028E3 RID: 10467
			public class BEDTYPE
			{
				// Token: 0x0400B35F RID: 45919
				public static LocString TITLE = "Beds";

				// Token: 0x0400B360 RID: 45920
				public static LocString DESCRIPTION = "Buildings that allow Duplicants to get much-needed rest. If a Duplicant is not assigned one, they will sleep on the floor.";

				// Token: 0x0400B361 RID: 45921
				public static LocString FLAVOUR = "";

				// Token: 0x0400B362 RID: 45922
				public static LocString CONFLICTINGROOMS = string.Concat(new string[]
				{
					"    • ",
					UI.FormatAsLink("Luxury Barracks", "BEDROOM"),
					" (No ",
					UI.FormatAsLink("Cots", "BED"),
					")\n    • ",
					UI.FormatAsLink("Private Bedroom", "PRIVATE BEDROOM"),
					" (No ",
					UI.FormatAsLink("Cots", "BED"),
					")"
				});

				// Token: 0x0400B363 RID: 45923
				public static LocString ROOMSREQUIRING = string.Concat(new string[]
				{
					"    • ",
					UI.FormatAsLink("Barracks", "BARRACKS"),
					"\n    • ",
					UI.FormatAsLink("Luxury Barracks", "BEDROOM"),
					" (one or more ",
					UI.FormatAsLink("Comfy Beds", "LUXURYBED"),
					")\n    • ",
					UI.FormatAsLink("Private Bedroom", "PRIVATE BEDROOM"),
					" (single ",
					UI.FormatAsLink("Comfy Bed", "LUXURYBED"),
					")"
				});
			}

			// Token: 0x020028E4 RID: 10468
			public class LIGHTSOURCE
			{
				// Token: 0x0400B364 RID: 45924
				public static LocString TITLE = "Light Sources";

				// Token: 0x0400B365 RID: 45925
				public static LocString DESCRIPTION = "Buildings that produce light, either by design or as a result of their primary operations.";

				// Token: 0x0400B366 RID: 45926
				public static LocString FLAVOUR = "";

				// Token: 0x0400B367 RID: 45927
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Laboratory", "LABORATORY");
			}

			// Token: 0x020028E5 RID: 10469
			public class ROCKETINTERIOR
			{
				// Token: 0x0400B368 RID: 45928
				public static LocString TITLE = "Rocket Interior";

				// Token: 0x0400B369 RID: 45929
				public static LocString DESCRIPTION = "Buildings that must be built inside a rocket.";

				// Token: 0x0400B36A RID: 45930
				public static LocString FLAVOUR = "";
			}

			// Token: 0x020028E6 RID: 10470
			public class CREATURERELOCATOR
			{
				// Token: 0x0400B36B RID: 45931
				public static LocString TITLE = "Critter Relocators";

				// Token: 0x0400B36C RID: 45932
				public static LocString DESCRIPTION = "Buildings that facilitate the movement of " + UI.FormatAsLink("Critters", "CREATURES") + " from one location to another.";

				// Token: 0x0400B36D RID: 45933
				public static LocString FLAVOUR = "";
			}

			// Token: 0x020028E7 RID: 10471
			public class COOKTOP
			{
				// Token: 0x0400B36E RID: 45934
				public static LocString TITLE = "Cooking Stations";

				// Token: 0x0400B36F RID: 45935
				public static LocString DESCRIPTION = "Buildings that transform individual ingredients into delicious meals.";

				// Token: 0x0400B370 RID: 45936
				public static LocString FLAVOUR = "";

				// Token: 0x0400B371 RID: 45937
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Kitchen", "KITCHEN");
			}

			// Token: 0x020028E8 RID: 10472
			public class WARMINGSTATION
			{
				// Token: 0x0400B372 RID: 45938
				public static LocString TITLE = "Warming Stations";

				// Token: 0x0400B373 RID: 45939
				public static LocString DESCRIPTION = "Buildings that Duplicants will visit when they are suffering the effects of cold environments.";

				// Token: 0x0400B374 RID: 45940
				public static LocString FLAVOUR = "";
			}

			// Token: 0x020028E9 RID: 10473
			public class BIONICUPKEEP
			{
				// Token: 0x0400B375 RID: 45941
				public static LocString TITLE = "Bionic Service Stations";

				// Token: 0x0400B376 RID: 45942
				public static LocString DESCRIPTION = "Buildings that keep Bionic Duplicants' complex inner machinery operating smoothly.";

				// Token: 0x0400B377 RID: 45943
				public static LocString FLAVOUR = "";

				// Token: 0x0400B378 RID: 45944
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Workshop", "BIONIC");
			}

			// Token: 0x020028EA RID: 10474
			public class GENERATORTYPE
			{
				// Token: 0x0400B379 RID: 45945
				public static LocString TITLE = "Generators";

				// Token: 0x0400B37A RID: 45946
				public static LocString DESCRIPTION = "Buildings that generate the " + UI.FormatAsLink("Power", "POWER") + " required to run machinery in my colony.\n\nBasic requirements can be met with an entry-level generator, but heavier-duty buildings are essential to colony development.";

				// Token: 0x0400B37B RID: 45947
				public static LocString FLAVOUR = "";

				// Token: 0x0400B37C RID: 45948
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Power Plant", "POWERPLANT");
			}

			// Token: 0x020028EB RID: 10475
			public class POWERBUILDING
			{
				// Token: 0x0400B37D RID: 45949
				public static LocString TITLE = "Power Buildings";

				// Token: 0x0400B37E RID: 45950
				public static LocString DESCRIPTION = "Buildings that generate, manage or store the electrical power a colony needs to thrive and expand.";

				// Token: 0x0400B37F RID: 45951
				public static LocString FLAVOUR = "";

				// Token: 0x0400B380 RID: 45952
				public static LocString ROOMSREQUIRING = "    • " + UI.FormatAsLink("Power Plant", "POWERPLANT");
			}
		}

		// Token: 0x02002023 RID: 8227
		public class BEETA
		{
			// Token: 0x04009247 RID: 37447
			public static LocString TITLE = "Beeta";

			// Token: 0x04009248 RID: 37448
			public static LocString SUBTITLE = "Aggressive Critter";

			// Token: 0x020028EC RID: 10476
			public class BODY
			{
				// Token: 0x0400B381 RID: 45953
				public static LocString CONTAINER1 = string.Concat(new string[]
				{
					"Beetas are insectoid creatures that enjoy a symbiotic relationship with the radioactive environment they thrive in.\n\nMuch like the honey bee gathers nectar and processes it to honey, the Beeta turns ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					" into ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" through a complex process of isotope separation inside the Beeta Hive.\n\nWhen first observing the Beeta's enrichment process, many scientists note with surprise just how much more efficient the cooperative combination of insect and hive is when compared to even the most advanced industrial processes."
				});
			}
		}

		// Token: 0x02002024 RID: 8228
		public class DIVERGENT
		{
			// Token: 0x04009249 RID: 37449
			public static LocString TITLE = "Divergent";

			// Token: 0x0400924A RID: 37450
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x020028ED RID: 10477
			public class BODY
			{
				// Token: 0x0400B382 RID: 45954
				public static LocString CONTAINER1 = "'Divergent' is the name given to the two different genders of one species, the Sweetle and the Grubgrub, both of which are able to reproduce asexually and tend to Grubfruit Plants.\n\nWhen tending to the Grubfruit Plant, both gender variants of the Divergent display the exact same behaviors, however the Grubgrub possesses slightly more tiny facial hair which helps in pollinating the plants and stimulates faster growth.";
			}
		}

		// Token: 0x02002025 RID: 8229
		public class DRECKO
		{
			// Token: 0x0400924B RID: 37451
			public static LocString SPECIES_TITLE = "Dreckos";

			// Token: 0x0400924C RID: 37452
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x0400924D RID: 37453
			public static LocString TITLE = "Drecko";

			// Token: 0x0400924E RID: 37454
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x020028EE RID: 10478
			public class BODY
			{
				// Token: 0x0400B383 RID: 45955
				public static LocString CONTAINER1 = "Dreckos are a reptilian species boasting billions of microscopic hairs on their feet, allowing them to stick to and climb most surfaces.";

				// Token: 0x0400B384 RID: 45956
				public static LocString CONTAINER2 = "The tail of the Drecko, called the \"train\", is purely for decoration and can be lost or shorn without harm to the animal.\n\nAs a result, Drecko fibers are often farmed for use in textile production.\n\nCaring for Dreckos is a fulfilling endeavor thanks to their companionable personalities.\n\nSome domestic Dreckos have even been known to respond to their own names.";
			}
		}

		// Token: 0x02002026 RID: 8230
		public class GLOSSY
		{
			// Token: 0x0400924F RID: 37455
			public static LocString TITLE = "Glossy Drecko";

			// Token: 0x04009250 RID: 37456
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x020028EF RID: 10479
			public class BODY
			{
				// Token: 0x0400B385 RID: 45957
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Glossy\" Drecko variant</smallcaps>";
			}
		}

		// Token: 0x02002027 RID: 8231
		public class GASSYMOO
		{
			// Token: 0x04009251 RID: 37457
			public static LocString TITLE = "Gassy Moo";

			// Token: 0x04009252 RID: 37458
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x020028F0 RID: 10480
			public class BODY
			{
				// Token: 0x0400B386 RID: 45958
				public static LocString CONTAINER1 = "Little is currently known of the Gassy Moo due to its alien nature and origin.\n\nIt is capable of surviving in zero gravity conditions and no atmosphere, and is dependent on a second alien species, " + UI.FormatAsLink("Gas Grass", "GASGRASS") + ", for its sustenance and survival.";

				// Token: 0x0400B387 RID: 45959
				public static LocString CONTAINER2 = "The Moo has an even temperament and can be farmed for Natural Gas, though their method of reproduction has been as of yet undiscovered.";
			}
		}

		// Token: 0x02002028 RID: 8232
		public class HATCH
		{
			// Token: 0x04009253 RID: 37459
			public static LocString SPECIES_TITLE = "Hatches";

			// Token: 0x04009254 RID: 37460
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x04009255 RID: 37461
			public static LocString TITLE = "Hatch";

			// Token: 0x04009256 RID: 37462
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x020028F1 RID: 10481
			public class BODY
			{
				// Token: 0x0400B388 RID: 45960
				public static LocString CONTAINER1 = "The Hatch has no eyes and is completely blind, although a photosensitive patch atop its head is capable of detecting even minor changes in overhead light, making it prefer dark caves and tunnels.";
			}
		}

		// Token: 0x02002029 RID: 8233
		public class STONE
		{
			// Token: 0x04009257 RID: 37463
			public static LocString TITLE = "Stone Hatch";

			// Token: 0x04009258 RID: 37464
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x020028F2 RID: 10482
			public class BODY
			{
				// Token: 0x0400B389 RID: 45961
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Stone\" Hatch variant</smallcaps>";

				// Token: 0x0400B38A RID: 45962
				public static LocString CONTAINER2 = "When attempting to pet a Hatch, inexperienced handlers make the mistake of reaching out too quickly for the creature's head.\n\nThis triggers a fear response in the Hatch, as its photosensitive patch of skin called the \"parietal eye\" interprets this sudden light change as an incoming aerial predator.";
			}
		}

		// Token: 0x0200202A RID: 8234
		public class SAGE
		{
			// Token: 0x04009259 RID: 37465
			public static LocString TITLE = "Sage Hatch";

			// Token: 0x0400925A RID: 37466
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x020028F3 RID: 10483
			public class BODY
			{
				// Token: 0x0400B38B RID: 45963
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Sage\" Hatch variant</smallcaps>";

				// Token: 0x0400B38C RID: 45964
				public static LocString CONTAINER2 = "It is difficult to classify the Hatch's diet as the term \"omnivore\" does not extend to the non-organic materials it is capable of ingesting.\n\nA more appropriate term is \"totumvore\", given that it can consume and find nutritional value in nearly every known substance.";
			}
		}

		// Token: 0x0200202B RID: 8235
		public class SMOOTH
		{
			// Token: 0x0400925B RID: 37467
			public static LocString TITLE = "Smooth Hatch";

			// Token: 0x0400925C RID: 37468
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x020028F4 RID: 10484
			public class BODY
			{
				// Token: 0x0400B38D RID: 45965
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Smooth\" Hatch variant</smallcaps>";

				// Token: 0x0400B38E RID: 45966
				public static LocString CONTAINER2 = "The proper way to pet a Hatch is to touch any of its four feet to first make it aware of your presence, then either scratch the soft segmented underbelly or firmly pat the creature's thick chitinous back.";
			}
		}

		// Token: 0x0200202C RID: 8236
		public class ICEBELLY
		{
			// Token: 0x0400925D RID: 37469
			public static LocString SPECIES_TITLE = "Bammoths";

			// Token: 0x0400925E RID: 37470
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x0400925F RID: 37471
			public static LocString TITLE = "Bammoth";

			// Token: 0x04009260 RID: 37472
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x020028F5 RID: 10485
			public class BODY
			{
				// Token: 0x0400B38F RID: 45967
				public static LocString CONTAINER1 = "The Bammoth is one of the oldest species on record, with ancient skeletal remains dating back approximately 10,000 years.\n\nThis placid herbivore is known for its unique body language: an angry young Bammoth expresses displeasure by flopping down dramatically in front of its opponent, while older creatures with limited mobility will sit facing away from the source of their annoyance.\n\nLicking the ground in front of a caregiver can be a sign of either deep affection or mineral deficiency.";
			}
		}

		// Token: 0x0200202D RID: 8237
		public class MOLE
		{
			// Token: 0x04009261 RID: 37473
			public static LocString TITLE = "Shove Vole";

			// Token: 0x04009262 RID: 37474
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x020028F6 RID: 10486
			public class BODY
			{
				// Token: 0x0400B390 RID: 45968
				public static LocString CONTAINER1 = "The Shove Vole is a unique creature that possesses two fully developed sets of lungs, allowing it to hold its breath during the long periods it spends underground.";

				// Token: 0x0400B391 RID: 45969
				public static LocString CONTAINER2 = "Drill-shaped keratin structures circling the Vole's body aids its ability to drill at high speeds through most natural materials.";
			}
		}

		// Token: 0x0200202E RID: 8238
		public class VARIANT_DELICACY
		{
			// Token: 0x04009263 RID: 37475
			public static LocString TITLE = "Delecta Vole";

			// Token: 0x04009264 RID: 37476
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x020028F7 RID: 10487
			public class BODY
			{
				// Token: 0x0400B392 RID: 45970
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Delecta\" Vole variant</smallcaps>";
			}
		}

		// Token: 0x0200202F RID: 8239
		public class MORB
		{
			// Token: 0x04009265 RID: 37477
			public static LocString TITLE = "Morb";

			// Token: 0x04009266 RID: 37478
			public static LocString SUBTITLE = "Pest Critter";

			// Token: 0x020028F8 RID: 10488
			public class BODY
			{
				// Token: 0x0400B393 RID: 45971
				public static LocString CONTAINER1 = "The Morb is a versatile scavenger, capable of breaking down and consuming dead matter from most plant and animal species.";

				// Token: 0x0400B394 RID: 45972
				public static LocString CONTAINER2 = "It poses a severe disease risk to humans due to the thick slime it excretes to surround its inner cartilage structures.\n\nA single teaspoon of Morb slime can contain up to a quadrillion bacteria that work to deter would-be predators and liquefy its food.";

				// Token: 0x0400B395 RID: 45973
				public static LocString CONTAINER3 = "Petting a Morb is not recommended.";
			}
		}

		// Token: 0x02002030 RID: 8240
		public class PACU
		{
			// Token: 0x04009267 RID: 37479
			public static LocString SPECIES_TITLE = "Pacus";

			// Token: 0x04009268 RID: 37480
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x04009269 RID: 37481
			public static LocString TITLE = "Pacu";

			// Token: 0x0400926A RID: 37482
			public static LocString SUBTITLE = "Aquatic Critter";

			// Token: 0x020028F9 RID: 10489
			public class BODY
			{
				// Token: 0x0400B396 RID: 45974
				public static LocString CONTAINER1 = "The Pacu fish is often interpreted as possessing a vacant stare due to its large and unblinking eyes, yet they are remarkably bright and friendly creatures.";
			}
		}

		// Token: 0x02002031 RID: 8241
		public class TROPICAL
		{
			// Token: 0x0400926B RID: 37483
			public static LocString TITLE = "Tropical Pacu";

			// Token: 0x0400926C RID: 37484
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x020028FA RID: 10490
			public class BODY
			{
				// Token: 0x0400B397 RID: 45975
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Tropical\" Pacu variant</smallcaps>";

				// Token: 0x0400B398 RID: 45976
				public static LocString CONTAINER2 = "It is said that the average Pacu intelligence is comparable to that of a dog, and that they are capable of learning and distinguishing from over twenty human faces.";
			}
		}

		// Token: 0x02002032 RID: 8242
		public class CLEANER
		{
			// Token: 0x0400926D RID: 37485
			public static LocString TITLE = "Gulp Fish";

			// Token: 0x0400926E RID: 37486
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x020028FB RID: 10491
			public class BODY
			{
				// Token: 0x0400B399 RID: 45977
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Gulp Fish\" Pacu variant</smallcaps>";

				// Token: 0x0400B39A RID: 45978
				public static LocString CONTAINER2 = "Despite descending from the Pacu, the Gulp Fish is unique enough both in genetics and behavior to be considered its own subspecies.";
			}
		}

		// Token: 0x02002033 RID: 8243
		public class PIP
		{
			// Token: 0x0400926F RID: 37487
			public static LocString SPECIES_TITLE = "Pips";

			// Token: 0x04009270 RID: 37488
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x04009271 RID: 37489
			public static LocString TITLE = "Pip";

			// Token: 0x04009272 RID: 37490
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x020028FC RID: 10492
			public class BODY
			{
				// Token: 0x0400B39B RID: 45979
				public static LocString CONTAINER1 = "Pips are a member of the Rodentia order with a strong caching instinct that causes them to find and bury small objects, most often seeds.";

				// Token: 0x0400B39C RID: 45980
				public static LocString CONTAINER2 = "It is unknown whether their caching behavior is a compulsion or a form of entertainment, as the Pip relies primarily on bark and wood for its survival.";

				// Token: 0x0400B39D RID: 45981
				public static LocString CONTAINER3 = "Although the Pip lacks truly opposable thumbs, it nonetheless has highly dexterous paws that allow it to rummage through most tight to reach spaces in search of seeds and other treasures.";
			}
		}

		// Token: 0x02002034 RID: 8244
		public class VARIANT_HUG
		{
			// Token: 0x04009273 RID: 37491
			public static LocString TITLE = "Cuddle Pip";

			// Token: 0x04009274 RID: 37492
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x020028FD RID: 10493
			public class BODY
			{
				// Token: 0x0400B39E RID: 45982
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Cuddle\" Pip variant</smallcaps>";

				// Token: 0x0400B39F RID: 45983
				public static LocString CONTAINER2 = "Cuddle Pips are genetically predisposed to feel deeply affectionate towards the unhatched young of all species, and can often be observed hugging eggs.";
			}
		}

		// Token: 0x02002035 RID: 8245
		public class PLUGSLUG
		{
			// Token: 0x04009275 RID: 37493
			public static LocString TITLE = "Plug Slug";

			// Token: 0x04009276 RID: 37494
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x020028FE RID: 10494
			public class BODY
			{
				// Token: 0x0400B3A0 RID: 45984
				public static LocString CONTAINER1 = "Plug Slugs are fuzzy gastropoda that are able to cling to walls and ceilings thanks to an extreme triboelectric effect caused by friction between their fur and various surfaces.\n\nThis same phenomomen allows the Plug Slug to generate a significant amount of static electricity that can be converted into power.\n\nThe increased amount of static electricity a Plug Slug can generate when domesticated is due to the internal vibration, or contented 'humming', they demonstrate when all their needs are met.";
			}
		}

		// Token: 0x02002036 RID: 8246
		public class VARIANT_LIQUID
		{
			// Token: 0x04009277 RID: 37495
			public static LocString TITLE = "Sponge Slug";

			// Token: 0x04009278 RID: 37496
			public static LocString SUBTITLE = "Critter Morph";
		}

		// Token: 0x02002037 RID: 8247
		public class VARIANT_GAS
		{
			// Token: 0x04009279 RID: 37497
			public static LocString TITLE = "Smog Slug";

			// Token: 0x0400927A RID: 37498
			public static LocString SUBTITLE = "Critter Morph";
		}

		// Token: 0x02002038 RID: 8248
		public class POKESHELL
		{
			// Token: 0x0400927B RID: 37499
			public static LocString SPECIES_TITLE = "Pokeshells";

			// Token: 0x0400927C RID: 37500
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x0400927D RID: 37501
			public static LocString TITLE = "Pokeshell";

			// Token: 0x0400927E RID: 37502
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x020028FF RID: 10495
			public class BODY
			{
				// Token: 0x0400B3A1 RID: 45985
				public static LocString CONTAINER1 = "Pokeshells are bottom-feeding invertebrates that consume the waste and discarded food left behind by other creatures.";

				// Token: 0x0400B3A2 RID: 45986
				public static LocString CONTAINER2 = "They have formidably sized claws that fold safely into their shells for protection when not in use.";

				// Token: 0x0400B3A3 RID: 45987
				public static LocString CONTAINER3 = "As Pokeshells mature they must periodically shed portions of their exoskeletons to make room for new growth.";

				// Token: 0x0400B3A4 RID: 45988
				public static LocString CONTAINER4 = "Although the most dramatic sheds occur early in a Pokeshell's adolescence, they will continue growing and shedding throughout their adult lives, until the day they eventually die.";
			}
		}

		// Token: 0x02002039 RID: 8249
		public class VARIANT_WOOD
		{
			// Token: 0x0400927F RID: 37503
			public static LocString TITLE = "Oakshell";

			// Token: 0x04009280 RID: 37504
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002900 RID: 10496
			public class BODY
			{
				// Token: 0x0400B3A5 RID: 45989
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Oakshell\" variant</smallcaps>";
			}
		}

		// Token: 0x0200203A RID: 8250
		public class VARIANT_FRESH_WATER
		{
			// Token: 0x04009281 RID: 37505
			public static LocString TITLE = "Sanishell";

			// Token: 0x04009282 RID: 37506
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002901 RID: 10497
			public class BODY
			{
				// Token: 0x0400B3A6 RID: 45990
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Sanishell\" variant</smallcaps>";
			}
		}

		// Token: 0x0200203B RID: 8251
		public class PUFT
		{
			// Token: 0x04009283 RID: 37507
			public static LocString SPECIES_TITLE = "Pufts";

			// Token: 0x04009284 RID: 37508
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x04009285 RID: 37509
			public static LocString TITLE = "Puft";

			// Token: 0x04009286 RID: 37510
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002902 RID: 10498
			public class BODY
			{
				// Token: 0x0400B3A7 RID: 45991
				public static LocString CONTAINER1 = "The Puft is a mellow creature whose limited brainpower is largely dedicated to sustaining its basic life processes.";
			}
		}

		// Token: 0x0200203C RID: 8252
		public class SQUEAKY
		{
			// Token: 0x04009287 RID: 37511
			public static LocString TITLE = "Squeaky Puft";

			// Token: 0x04009288 RID: 37512
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002903 RID: 10499
			public class BODY
			{
				// Token: 0x0400B3A8 RID: 45992
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Squeaky\" Puft variant</smallcaps>";

				// Token: 0x0400B3A9 RID: 45993
				public static LocString CONTAINER2 = "Pufts often have a collection of asymmetric teeth lining the ridge of their mouths, although this feature is entirely vestigial as Pufts do not consume solid food.\n\nInstead, a baleen-like mesh of keratin at the back of the Puft's throat works to filter out tiny organisms and food particles from the air.\n\nUnusable air is expelled back out the Puft's posterior trunk, along with waste material and any indigestible particles or pathogens which it then evacuates as solid biomass.";
			}
		}

		// Token: 0x0200203D RID: 8253
		public class DENSE
		{
			// Token: 0x04009289 RID: 37513
			public static LocString TITLE = "Dense Puft";

			// Token: 0x0400928A RID: 37514
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002904 RID: 10500
			public class BODY
			{
				// Token: 0x0400B3AA RID: 45994
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Dense\" Puft variant</smallcaps>";

				// Token: 0x0400B3AB RID: 45995
				public static LocString CONTAINER2 = "The Puft is an easy creature to raise for first time handlers given its wholly amiable disposition and suggestible nature.\n\nIt is unusually tolerant of human handling and will allow itself to be patted or scratched nearly anywhere on its fuzzy body, including, unnervingly, directly on any of its three eyeballs.";
			}
		}

		// Token: 0x0200203E RID: 8254
		public class PRINCE
		{
			// Token: 0x0400928B RID: 37515
			public static LocString TITLE = "Puft Prince";

			// Token: 0x0400928C RID: 37516
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002905 RID: 10501
			public class BODY
			{
				// Token: 0x0400B3AC RID: 45996
				public static LocString CONTAINER1 = "<smallcaps>Pictured: Puft \"Prince\" variant</smallcaps>";

				// Token: 0x0400B3AD RID: 45997
				public static LocString CONTAINER2 = "A specialized air bladder in the Puft's chest cavity stores varying concentrations of gas, allowing it to control its buoyancy and float effortlessly through the air.\n\nCombined with extremely lightweight and elastic skin, the Puft is capable of maintaining flotation indefinitely with negligible energy expenditure. Its orientation and balance, meanwhile, are maintained by counterweighted formations of bone located in its otherwise useless legs.";
			}
		}

		// Token: 0x0200203F RID: 8255
		public class ROVER
		{
			// Token: 0x0400928D RID: 37517
			public static LocString TITLE = "Rover";

			// Token: 0x0400928E RID: 37518
			public static LocString SUBTITLE = "Scouting Robot";

			// Token: 0x02002906 RID: 10502
			public class BODY
			{
				// Token: 0x0400B3AE RID: 45998
				public static LocString CONTAINER1 = "The Rover is a planetary scout robot programmed to land on and mine Planetoids where sending a Duplicant would put them unneccessarily in danger.\n\nRovers are programmed to be very pleasant and social when interacting with other beings. However, an unintended consequence of this programming is that the socialized robots tended to experience the same work slow-downs due to loneliness and low morale.\n\nTo compensate for this, the Rover was programmed to have two distinct personalities it can switch between to have pleasant in-depth conversations with itself during long stints alone.";
			}
		}

		// Token: 0x02002040 RID: 8256
		public class SEAL
		{
			// Token: 0x0400928F RID: 37519
			public static LocString SPECIES_TITLE = "Spigot Seals";

			// Token: 0x04009290 RID: 37520
			public static LocString SPECIES_SUBTITLE = "Domesticable Species";

			// Token: 0x04009291 RID: 37521
			public static LocString TITLE = "Spigot Seal";

			// Token: 0x04009292 RID: 37522
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002907 RID: 10503
			public class BODY
			{
				// Token: 0x0400B3AF RID: 45999
				public static LocString CONTAINER1 = "Spigot Seals are named for the hollow, cone-shaped glabellar protrusion that allows them to siphon nourishment directly from plants into the digestive sac located at the cone's base.\n\nIn order to draw nutritious fluids through this \"straw,\" the Spigot Seal compresses its nasal cavity and pumps its tongue up into its soft palate repeatedly, creating a vacuum.\n\nMealtimes are concluded by lapping at the air to reopen the airways and prevent accidental asphyxiation.\n\nMany handlers enjoy teaching this critter to clap its flippers, only to discover that there is no reliable method of limiting how often or how loudly the behavior is repeated.";
			}
		}

		// Token: 0x02002041 RID: 8257
		public class SHINEBUG
		{
			// Token: 0x04009293 RID: 37523
			public static LocString SPECIES_TITLE = "Shine Bugs";

			// Token: 0x04009294 RID: 37524
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x04009295 RID: 37525
			public static LocString TITLE = "Shine Bug";

			// Token: 0x04009296 RID: 37526
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002908 RID: 10504
			public class BODY
			{
				// Token: 0x0400B3B0 RID: 46000
				public static LocString CONTAINER1 = "The bioluminescence of the Shine Bug's body serves the social purpose of finding and communicating with others of its kind.";
			}
		}

		// Token: 0x02002042 RID: 8258
		public class NEGA
		{
			// Token: 0x04009297 RID: 37527
			public static LocString TITLE = "Abyss Bug";

			// Token: 0x04009298 RID: 37528
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002909 RID: 10505
			public class BODY
			{
				// Token: 0x0400B3B1 RID: 46001
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Abyss\" Shine Bug variant</smallcaps>";

				// Token: 0x0400B3B2 RID: 46002
				public static LocString CONTAINER2 = "The Abyss Shine Bug morph has an unusual genetic mutation causing it to absorb light rather than emit it.";
			}
		}

		// Token: 0x02002043 RID: 8259
		public class CRYSTAL
		{
			// Token: 0x04009299 RID: 37529
			public static LocString TITLE = "Radiant Bug";

			// Token: 0x0400929A RID: 37530
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x0200290A RID: 10506
			public class BODY
			{
				// Token: 0x0400B3B3 RID: 46003
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Radiant\" Shine Bug variant</smallcaps>";
			}
		}

		// Token: 0x02002044 RID: 8260
		public class SUNNY
		{
			// Token: 0x0400929B RID: 37531
			public static LocString TITLE = "Sun Bug";

			// Token: 0x0400929C RID: 37532
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x0200290B RID: 10507
			public class BODY
			{
				// Token: 0x0400B3B4 RID: 46004
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Sun\" Shine Bug variant</smallcaps>";

				// Token: 0x0400B3B5 RID: 46005
				public static LocString CONTAINER2 = "It is not uncommon for Shine Bugs to mistakenly approach inanimate sources of light in search of a friend.";
			}
		}

		// Token: 0x02002045 RID: 8261
		public class PLACID
		{
			// Token: 0x0400929D RID: 37533
			public static LocString TITLE = "Azure Bug";

			// Token: 0x0400929E RID: 37534
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x0200290C RID: 10508
			public class BODY
			{
				// Token: 0x0400B3B6 RID: 46006
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Azure\" Shine Bug variant</smallcaps>";
			}
		}

		// Token: 0x02002046 RID: 8262
		public class VITAL
		{
			// Token: 0x0400929F RID: 37535
			public static LocString TITLE = "Coral Bug";

			// Token: 0x040092A0 RID: 37536
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x0200290D RID: 10509
			public class BODY
			{
				// Token: 0x0400B3B7 RID: 46007
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Coral\" Shine Bug variant</smallcaps>";

				// Token: 0x0400B3B8 RID: 46008
				public static LocString CONTAINER2 = "It is unwise to touch a Shine Bug's wing blades directly due to the extremely fragile nature of their membranes.";
			}
		}

		// Token: 0x02002047 RID: 8263
		public class ROYAL
		{
			// Token: 0x040092A1 RID: 37537
			public static LocString TITLE = "Royal Bug";

			// Token: 0x040092A2 RID: 37538
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x0200290E RID: 10510
			public class BODY
			{
				// Token: 0x0400B3B9 RID: 46009
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Royal\" Shine Bug variant</smallcaps>";

				// Token: 0x0400B3BA RID: 46010
				public static LocString CONTAINER2 = "The Shine Bug can be pet anywhere else along its body, although it is advised that care still be taken due to the generally delicate nature of its exoskeleton.";
			}
		}

		// Token: 0x02002048 RID: 8264
		public class SLICKSTER
		{
			// Token: 0x040092A3 RID: 37539
			public static LocString SPECIES_TITLE = "Slicksters";

			// Token: 0x040092A4 RID: 37540
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x040092A5 RID: 37541
			public static LocString TITLE = "Slickster";

			// Token: 0x040092A6 RID: 37542
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x0200290F RID: 10511
			public class BODY
			{
				// Token: 0x0400B3BB RID: 46011
				public static LocString CONTAINER1 = "Slicksters are a unique creature most renowned for their ability to exude hydrocarbon waste that is nearly identical in makeup to crude oil.\n\nThe two tufts atop a Slickster's head are called rhinophores, and help guide the Slickster toward breathable carbon dioxide.";
			}
		}

		// Token: 0x02002049 RID: 8265
		public class MOLTEN
		{
			// Token: 0x040092A7 RID: 37543
			public static LocString TITLE = "Molten Slickster";

			// Token: 0x040092A8 RID: 37544
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002910 RID: 10512
			public class BODY
			{
				// Token: 0x0400B3BC RID: 46012
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Molten\" Slickster variant</smallcaps>";

				// Token: 0x0400B3BD RID: 46013
				public static LocString CONTAINER2 = "Slicksters are amicable creatures famous amongst breeders for their good personalities and smiley faces.\n\nSlicksters rarely if ever nip at human handlers, and are considered non-ideal house pets only for the oily mess they involuntarily leave behind wherever they go.";
			}
		}

		// Token: 0x0200204A RID: 8266
		public class DECOR
		{
			// Token: 0x040092A9 RID: 37545
			public static LocString TITLE = "Longhair Slickster";

			// Token: 0x040092AA RID: 37546
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002911 RID: 10513
			public class BODY
			{
				// Token: 0x0400B3BE RID: 46014
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Longhair\" Slickster variant</smallcaps>";

				// Token: 0x0400B3BF RID: 46015
				public static LocString CONTAINER2 = "Positioned on either side of the Major Rhinophores are Minor Rhinophores, which specialize in mechanical reception and detect air pressure around the Slickster. These send signals to the brain to contract or expand its air sacks accordingly.";
			}
		}

		// Token: 0x0200204B RID: 8267
		public class SWEEPY
		{
			// Token: 0x040092AB RID: 37547
			public static LocString TITLE = "Sweepy";

			// Token: 0x040092AC RID: 37548
			public static LocString SUBTITLE = "Cleaning Robot";

			// Token: 0x02002912 RID: 10514
			public class BODY
			{
				// Token: 0x0400B3C0 RID: 46016
				public static LocString CONTAINER1 = "The Sweepy is a domesticated sweeping robot programmed to clean solid and liquid debris. The Sweepy Dock will automatically launch the Sweepy, store the debris the robot picks up, and recharge the Sweepy's battery, provided it has been plugged into a power source.\n\nThough the Sweepy can not travel over gaps or uneven ground, it is programmed to feel really bad about this.";
			}
		}

		// Token: 0x0200204C RID: 8268
		public class DEERSPECIES
		{
			// Token: 0x040092AD RID: 37549
			public static LocString SPECIES_TITLE = "Floxes";

			// Token: 0x040092AE RID: 37550
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x040092AF RID: 37551
			public static LocString TITLE = "Flox";

			// Token: 0x040092B0 RID: 37552
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002913 RID: 10515
			public class BODY
			{
				// Token: 0x0400B3C1 RID: 46017
				public static LocString CONTAINER1 = "Evenly distributed throughout the Flox's dense overcoat are countless vibrissae-like hairs that transmit detailed sensory information about its environment, allowing it to detect changes as subtle as the shift in another creature's mood.\n\nFloxes avoid overstimulation by whipping their tails to release the pent-up energy. Because these tactile hairs are so sensitive, they cannot be safely shorn.\n\nFlox antlers, however, are nerveless and cumbersome. Handlers who unburden them of this cranial load are often rewarded with the critter's long, slow blinks of contentment.";
			}
		}

		// Token: 0x0200204D RID: 8269
		public class B6_AICONTROL
		{
			// Token: 0x040092B1 RID: 37553
			public static LocString TITLE = "Re: Objectionable Request";

			// Token: 0x040092B2 RID: 37554
			public static LocString TITLE2 = "SUBJECT: Objectionable Request";

			// Token: 0x040092B3 RID: 37555
			public static LocString TITLE3 = "SUBJECT: Re: Objectionable Request";

			// Token: 0x040092B4 RID: 37556
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x02002914 RID: 10516
			public class BODY
			{
				// Token: 0x0400B3C2 RID: 46018
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><size=12><alpha=#AA> <jstern@gravitas.nova></size></color>\nFrom: <b>Dr. Broussard</b><size=12><alpha=#AA> <obroussard@gravitas.nova></color></size></smallcaps>\n------------------\n";

				// Token: 0x0400B3C3 RID: 46019
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Dr. Broussard</b><size=12><alpha=#AA> <obroussard@gravitas.nova></size></color>\nFrom: <b>Director Stern</b><alpha=#AA><size=12> <jstern@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B3C4 RID: 46020
				public static LocString CONTAINER2 = "<indent=5%>Director,\n\nEngineering has requested the brainmaps of all blueprint subjects for the development of a podlinked software and I am reluctant to oblige.\n\nI believe they are seeking a way to exert temporary control over implanted subjects, and I fear this avenue of research may be ethically unsound.</indent>";

				// Token: 0x0400B3C5 RID: 46021
				public static LocString CONTAINER3 = "<indent=5%>Doctor,\n\nI understand your concerns, but engineering's newest project was conceived under my supervision.\n\nPlease give them any materials they require to move forward with their research.</indent>";

				// Token: 0x0400B3C6 RID: 46022
				public static LocString CONTAINER4 = "<indent=5%>You can't be serious, Jacquelyn?</indent>";

				// Token: 0x0400B3C7 RID: 46023
				public static LocString CONTAINER5 = "<indent=5%>You signed off on cranial chip implantation. Why would this be where you draw the line?\n\nIt would be an invaluable safety measure and protect your printing subjects.</indent>";

				// Token: 0x0400B3C8 RID: 46024
				public static LocString CONTAINER6 = "<indent=5%>It just gives me a bad feeling.\n\nI can't stop you if you insist on going forward with this, but I'd ask that you remove me from the project.</indent>";

				// Token: 0x0400B3C9 RID: 46025
				public static LocString SIGNATURE1 = "\n-Dr. Broussard\n<size=11>Bioengineering Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400B3CA RID: 46026
				public static LocString SIGNATURE2 = "\n-Director Stern\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x0200204E RID: 8270
		public class B51_ARTHISTORYREQUEST
		{
			// Token: 0x040092B5 RID: 37557
			public static LocString TITLE = "Re: Implant Database Request";

			// Token: 0x040092B6 RID: 37558
			public static LocString TITLE2 = "Implant Database Request";

			// Token: 0x040092B7 RID: 37559
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x02002915 RID: 10517
			public class BODY
			{
				// Token: 0x0400B3CB RID: 46027
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><size=12><alpha=#AA> <jstern@gravitas.nova></color></size>\nFrom: <b>Dr. Broussard</b><size=12><alpha=#AA> <obroussard@gravitas.nova></color></size></smallcaps>\n------------------\n";

				// Token: 0x0400B3CC RID: 46028
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Dr. Broussard</b><alpha=#AA><size=12> <obroussard@gravitas.nova></size></color>\nFrom: <b>Director Stern</b><alpha=#AA><size=12> <jstern@gravitas.nova></color></smallcaps></size>\n------------------\n";

				// Token: 0x0400B3CD RID: 46029
				public static LocString CONTAINER2 = "<indent=5%>Director,\n\nI have been thinking, and it occurs to me that our subjects will likely travel outside our range of radio contact when establishing new colonies.\n\nColonies travel into the cosmos as representatives of humanity, and I believe it is our duty to preserve the planet's non-scientific knowledge in addition to practical information.\n\nI would like to make a formal request that comprehensive arts and cultural histories make their way onto the microchip databases.</indent>";

				// Token: 0x0400B3CE RID: 46030
				public static LocString CONTAINER3 = "<indent=5%>Doctor,\n\nIf there is room available after the necessary scientific and survival knowledge has been uploaded, I will see what I can do.</indent>";

				// Token: 0x0400B3CF RID: 46031
				public static LocString SIGNATURE1 = "\n-Dr. Broussard\n<size=11>Bioengineering Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400B3D0 RID: 46032
				public static LocString SIGNATURE2 = "\n-Director Stern\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x0200204F RID: 8271
		public class A4_ATOMICONRECRUITMENT
		{
			// Token: 0x040092B8 RID: 37560
			public static LocString TITLE = "Results from Atomicon";

			// Token: 0x040092B9 RID: 37561
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x02002916 RID: 10518
			public class BODY
			{
				// Token: 0x0400B3D1 RID: 46033
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><alpha=#AA><size=12> <jstern@gravitas.nova></size></color>\nFrom: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></smallcaps>\n------------------\n";

				// Token: 0x0400B3D2 RID: 46034
				public static LocString CONTAINER2 = "<indent=5%>Director,\n\nEverything went well. Broussard was reluctant at first, but she has little alternative given the nature of her work and the recent turn of events.\n\nShe can begin at your convenience.</indent>";

				// Token: 0x0400B3D3 RID: 46035
				public static LocString SIGNATURE1 = "\nXOXO,\nDr. Jones\n\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02002050 RID: 8272
		public class A3_DEVONSBLOG
		{
			// Token: 0x040092BA RID: 37562
			public static LocString TITLE = "Re: devon's bloggg";

			// Token: 0x040092BB RID: 37563
			public static LocString TITLE2 = "SUBJECT: devon's bloggg";

			// Token: 0x040092BC RID: 37564
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x02002917 RID: 10519
			public class BODY
			{
				// Token: 0x0400B3D4 RID: 46036
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Summers</b><alpha=#AA><size=12> <jsummers@gravitas.nova></size></color>\nFrom: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B3D5 RID: 46037
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></size></color>\nFrom: <b>Dr. Summers</b><alpha=#AA><size=12> <jsummers@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B3D6 RID: 46038
				public static LocString CONTAINER1 = "<indent=5%>Oh my goddd I found out today that Devon's one of those people who takes pictures of their food and uploads them to some boring blog somewhere.\n\nYou HAVE to come to lunch with us and see, they spend so long taking pictures that the food gets cold and they have to ask the waiter to reheat it. It's SO FUNNY.</indent>";

				// Token: 0x0400B3D7 RID: 46039
				public static LocString CONTAINER2 = "<indent=5%>Oh cool, Devon's writing a new post for <i>Toast of the Town</i>? I'd love to tag along and \"see how the sausage is made\" so to speak, haha.\n\nI'll see you guys in a bit! :)</indent>";

				// Token: 0x0400B3D8 RID: 46040
				public static LocString CONTAINER3 = "<indent=5%>WAIT, Joshua, you read Devon's blog??</indent>";

				// Token: 0x0400B3D9 RID: 46041
				public static LocString SIGNATURE1 = "\nXOXO,\nDr. Jones\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400B3DA RID: 46042
				public static LocString SIGNATURE2 = "\n-Dr. Summers\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02002051 RID: 8273
		public class C5_ENGINEERINGCANDIDATE
		{
			// Token: 0x040092BD RID: 37565
			public static LocString TITLE = "RE: Engineer Candidate?";

			// Token: 0x040092BE RID: 37566
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x02002918 RID: 10520
			public class BODY
			{
				// Token: 0x0400B3DB RID: 46043
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><size=12><alpha=#AA> <jstern@gravitas.nova></size></color></smallcaps>\nFrom: <b>[REDACTED]</b>\n------------------\n";

				// Token: 0x0400B3DC RID: 46044
				public static LocString CONTAINER3 = "<indent=5%>Director, I think I've found the perfect engineer candidate to design our small-scale colony machines.\n-----------------------------------------------------------------------------------------------------\n</indent>";

				// Token: 0x0400B3DD RID: 46045
				public static LocString CONTAINER4 = "<indent=10%><smallcaps><b>Bringing Creative Workspace Ideas into the Industrial Setting</b>\n\nMichael E.E. Perlmutter is a rising star in the world industrial design, making a name for himself by cooking up playful workspaces for a work force typically left out of the creative conversation.\n\n\"Ergodynamic chairs have been done to death,\" says Perlmutter. \"What I'm interested in is redesigning the industrial space. There's no reason why a machine can't convey a sense of whimsy.\"\n\nIt's this philosophy that has launched Perlmutter to the top of a very short list of hot new industrial designers.</indent></smallcaps>";

				// Token: 0x0400B3DE RID: 46046
				public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>Human Resources Coordinator\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02002052 RID: 8274
		public class B7_FRIENDLYEMAIL
		{
			// Token: 0x040092BF RID: 37567
			public static LocString TITLE = "Hiiiii!";

			// Token: 0x040092C0 RID: 37568
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x02002919 RID: 10521
			public class BODY
			{
				// Token: 0x0400B3DF RID: 46047
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Techna</b><alpha=#AA><size=12> <ntechna@gravitas.nova></size></color>\nFrom: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B3E0 RID: 46048
				public static LocString CONTAINER1 = "<indent=5%>Omg, <i>hi</i> Nikola!\n\nHave you heard about the super weird thing that's been happening in the kitchen lately? Joshua's lunch has disappeared from the fridge like, every day for the past week!\n\nThere's a <i>ton</i> of cameras in that room too but all anyone can see is like this spiky blond hair behind the fridge door.\n\nSo <i>weird</i> right? ;)\n\nAnyway, totally unrelated, but our computer system's been having this <i>glitch</i> where datasets going back for like half a year get <i>totally</i> wiped for all employees with the initials \"N.T.\"\n\nIsn't it weird how specific that is? Don't worry though! I'm sure I'll have it fixed before it affects any of <i>your</i> work.\n\nByeee!</indent>";

				// Token: 0x0400B3E1 RID: 46049
				public static LocString SIGNATURE1 = "\nXOXO,\nDr. Jones\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02002053 RID: 8275
		public class B1_HOLLANDSDOG
		{
			// Token: 0x040092C1 RID: 37569
			public static LocString TITLE = "Re: dr. holland's dog";

			// Token: 0x040092C2 RID: 37570
			public static LocString TITLE2 = "dr. holland's dog";

			// Token: 0x040092C3 RID: 37571
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x0200291A RID: 10522
			public class BODY
			{
				// Token: 0x0400B3E2 RID: 46050
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Summers</b><size=10><alpha=#AA> <jsummers@gravitas.nova></size></color>\nFrom: <b>Dr. Jones</b><alpha=#AA><size=10> <ejones@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B3E3 RID: 46051
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Dr. Jones</b><alpha=#AA><size=10> <ejones@gravitas.nova></size></color>\nFrom: <b>Dr. Summers</b><size=10><alpha=#AA> <jsummers@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B3E4 RID: 46052
				public static LocString CONTAINER1 = "<indent=5%>OMIGOD, every time I go into the break room now I get ambushed by Dr. Holland and he traps me in a 20 minute conversation about his new dog.\n\nLike, I GET it! Your puppy is cute! Why do you have like 400 different pictures of it on your phone, FROM THE SAME ANGLE?!\n\nSO annoying.</indent>";

				// Token: 0x0400B3E5 RID: 46053
				public static LocString CONTAINER2 = "<indent=5%>Haha, I think it's nice, he really loves his dog. Oh! Did I show you the thing my cat did last night? She always falls asleep on my bed but this time she sprawled out on her back and her little tongue was poking out! So cute.\n\n<color=#F44A47>[BROKEN IMAGE]</color>\n<alpha=#AA>[121 MISSING ATTACHMENTS]</color></indent>";

				// Token: 0x0400B3E6 RID: 46054
				public static LocString CONTAINER3 = "<indent=5%><i><b>UGHHHHHHHH!</b></i>\nYou're the worst!</indent>";

				// Token: 0x0400B3E7 RID: 46055
				public static LocString SIGNATURE1 = "\nXOXO,\nDr. Jones\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400B3E8 RID: 46056
				public static LocString SIGNATURE2 = "\n-Dr. Summers\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02002054 RID: 8276
		public class JOURNALISTREQUEST
		{
			// Token: 0x040092C4 RID: 37572
			public static LocString TITLE = "Re: Call me";

			// Token: 0x040092C5 RID: 37573
			public static LocString TITLE2 = "Call me";

			// Token: 0x040092C6 RID: 37574
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x0200291B RID: 10523
			public class BODY
			{
				// Token: 0x0400B3E9 RID: 46057
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Olowe</b><size=10><alpha=#AA> <aolowe@gravitas.nova></size></color>\nFrom: <b>Quinn Kelly</b><alpha=#AA><size=10> <editor@stemscoop.news></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B3EA RID: 46058
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>[BCC: all]</b><alpha=#AA><size=10> </size></color>\nFrom: <b>Quinn Kelly</b><alpha=#AA><size=10> <editor@stemscoop.news></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B3EB RID: 46059
				public static LocString CONTAINER1 = "<indent=5%>Dear colleagues, friends and community members,\n\nAfter nine deeply fulfilling years as editor of The STEM Scoop, I am stepping down to spend more time with my family.\n\nPlease give a warm welcome to Dorian Hearst, who will be taking over editorial management duties effective immediately.</indent>";

				// Token: 0x0400B3EC RID: 46060
				public static LocString CONTAINER2 = "<indent=5%>I don't know how you pulled it off, but Stern's office just called the paper and granted me an exclusive...and a tour of the Gravitas Facility. I owe you a beer. No - a case of beer. Six cases of beer!\n\nSeriously, thank you. I know you're in a difficult position but you've done the right thing. See you on Tuesday.</indent>";

				// Token: 0x0400B3ED RID: 46061
				public static LocString CONTAINER3 = "<indent=5%>I waited at the fountain for four hours. Where were you? This story is going to be huge. Call me.</indent>";

				// Token: 0x0400B3EE RID: 46062
				public static LocString CONTAINER4 = "<indent=5%>Dr. Olowe,\n\nI'm sorry - I know ambushing you at your home last night was a bad idea. But something is happening at Gravitas, and people need to know. Please call me.</indent>";

				// Token: 0x0400B3EF RID: 46063
				public static LocString SIGNATURE1 = "\n-Q\n------------------\n";

				// Token: 0x0400B3F0 RID: 46064
				public static LocString SIGNATURE2 = "\nAll the best,\nQuinn Kelly\n------------------\n";
			}
		}

		// Token: 0x02002055 RID: 8277
		public class B50_MEMORYCHIP
		{
			// Token: 0x040092C7 RID: 37575
			public static LocString TITLE = "Duplicant Memory Solution";

			// Token: 0x040092C8 RID: 37576
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x0200291C RID: 10524
			public class BODY
			{
				// Token: 0x0400B3F1 RID: 46065
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><alpha=#AA><size=12> <jstern@gravitas.nova></size></color>\nFrom: <b>[REDACTED]</b></smallcaps>\n------------------\n";

				// Token: 0x0400B3F2 RID: 46066
				public static LocString CONTAINER2 = "<indent=5%>Director,\n\nI had a thought about how to solve your Duplicant memory problem.\n\nRather than attempt to access the subject's old memories, what if we were to embed all necessary information for colony survival into the printing process itself?\n\nThe amount of data engineering can store has grown exponentially over the last year. We should take advantage of the technological development.</indent>";

				// Token: 0x0400B3F3 RID: 46067
				public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>Engineering Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02002056 RID: 8278
		public class MISSINGNOTES
		{
			// Token: 0x040092C9 RID: 37577
			public static LocString TITLE = "Re: Missing notes";

			// Token: 0x040092CA RID: 37578
			public static LocString TITLE2 = "SUBJECT: Missing notes";

			// Token: 0x040092CB RID: 37579
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x0200291D RID: 10525
			public class BODY
			{
				// Token: 0x0400B3F4 RID: 46068
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></size></color>\nFrom: <b>Dr. Olowe</b><alpha=#AA><size=12> <aolowe@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B3F5 RID: 46069
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Dr. Olowe</b><alpha=#AA><size=12> <aolowe@gravitas.nova></size></color>\nFrom: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B3F6 RID: 46070
				public static LocString EMAILHEADER3 = "<smallcaps>To: <b>Dr. Olowe</b><alpha=#AA><size=12> <aolowe@gravitas.nova></size></color>\nFrom: <b>Director Stern</b><alpha=#AA><size=12> <jstern@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B3F7 RID: 46071
				public static LocString CONTAINER1 = "<indent=5%>Hello Dr. Jones,\n\nHope you are well. Sorry to bother you- I believe that someone may have inappropriately accessed my computer.\n\nWhen I was logging in this morning, the \"last log-in\" pop-up indicated that my computer had been accessed at 2 a.m. My last actual log-in was 6 p.m. And some of my files have gone missing.\n\nThe privacy of my work is paramount. Would it be possible to have someone take a look, please?</indent>";

				// Token: 0x0400B3F8 RID: 46072
				public static LocString CONTAINER2 = "<indent=5%>OMG Amari, you're so dramatic!! It's probably just a glitch from the system network upgrade. Nobody can even get into your office without going through the new hand scanners.\n\nPS: Everybody's work is super private, not just yours ;)</indent>";

				// Token: 0x0400B3F9 RID: 46073
				public static LocString CONTAINER3 = "<indent=5%>Dear Dr. Jones,\nI'm so sorry to bother you again...it's just that I'm absolutely certain that someone has been interfering with my files.\n\nI've noticed several discrepancies since last week's \"glitch.\" For example, responses to my recent employee survey on workplace satisfaction and safety were decrypted, and significant portions of my data and research notes have been erased. I'm even missing a few e-mails.\n\nIt's all quite alarming. Could you or Dr. Summers please investigate this when you have a moment?\n\nThank you so much,\n\n</indent>";

				// Token: 0x0400B3FA RID: 46074
				public static LocString CONTAINER4 = "<indent=5%>The files in question were a security risk, and were disposed of accordingly.\n\nAs for your emails: the NDA you signed was very clear about how to handle requests from members of the media.\n\nSee me in my office.</indent>";

				// Token: 0x0400B3FB RID: 46075
				public static LocString SIGNATURE1 = "\n-Dr. Olowe\n<size=11>Industrial-Organizational Psychologist\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400B3FC RID: 46076
				public static LocString SIGNATURE2 = "\nXOXO,\nDr. Jones\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400B3FD RID: 46077
				public static LocString SIGNATURE3 = "\n-Director Stern\n<size=11>\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02002057 RID: 8279
		public class B4_MYPENS
		{
			// Token: 0x040092CC RID: 37580
			public static LocString TITLE = "SUBJECT: MY PENS";

			// Token: 0x040092CD RID: 37581
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x0200291E RID: 10526
			public class BODY
			{
				// Token: 0x0400B3FE RID: 46078
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>ALL</b>\nFrom: <b>[REDACTED]</b></color></smallcaps>\n------------------\n";

				// Token: 0x0400B3FF RID: 46079
				public static LocString CONTAINER2 = "<indent=5%>To whomever is stealing the glitter pens off of my desk:\n\n<i>CONSIDER THIS YOUR LAST WARNING!</i></indent>";

				// Token: 0x0400B400 RID: 46080
				public static LocString SIGNATURE1 = "\nXOXO,\n[REDACTED]\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02002058 RID: 8280
		public class A7_NEWEMPLOYEE
		{
			// Token: 0x040092CE RID: 37582
			public static LocString TITLE = "Welcome, New Employee";

			// Token: 0x040092CF RID: 37583
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x0200291F RID: 10527
			public class BODY
			{
				// Token: 0x0400B401 RID: 46081
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>All</b>\nFrom: <b>[REDACTED]</b></smallcaps>\n------------------\n";

				// Token: 0x0400B402 RID: 46082
				public static LocString CONTAINER2 = "<indent=5%>Attention Gravitas Facility personnel;\n\nPlease welcome our newest staff member, Olivia Broussard, PhD.\n\nDr. Broussard will be leading our upcoming genetics project and has been installed in our bioengineering department.\n\nBe sure to offer her our warmest welcome.</indent>";

				// Token: 0x0400B403 RID: 46083
				public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>Personnel Coordinator\nThe Gravitas Facility</indent>\n------------------\n";
			}
		}

		// Token: 0x02002059 RID: 8281
		public class A6_NEWSECURITY2
		{
			// Token: 0x040092D0 RID: 37584
			public static LocString TITLE = "New Security System?";

			// Token: 0x040092D1 RID: 37585
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x02002920 RID: 10528
			public class BODY
			{
				// Token: 0x0400B404 RID: 46084
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>[REDACTED]</b>\nFrom: <b>[REDACTED]</b></color></smallcaps>\n------------------\n";

				// Token: 0x0400B405 RID: 46085
				public static LocString CONTAINER2 = "<indent=5%>So, the facility is introducing this new security system that scans your hand to unlock the doors. My question is, what exactly are they scanning?\n\nThe folks in engineering say the door device doesn't look like a fingerprint scanner, but the duo working over in bioengineering won't comment at all.\n\nI can't say I like it.</indent>";

				// Token: 0x0400B406 RID: 46086
				public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x0200205A RID: 8282
		public class A8_NEWSECURITY3
		{
			// Token: 0x040092D2 RID: 37586
			public static LocString TITLE = "They Stole Our DNA";

			// Token: 0x040092D3 RID: 37587
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x02002921 RID: 10529
			public class BODY
			{
				// Token: 0x0400B407 RID: 46087
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>[REDACTED]</b>\nFrom: <b>[REDACTED]</b></smallcaps>\n------------------\n";

				// Token: 0x0400B408 RID: 46088
				public static LocString CONTAINER2 = "<indent=5%>I'm almost certain now that the Facility's stolen our genetic information.\n\nForty-odd employees would make for mighty convenient lab rats, and even if we discovered what Gravitas did, we wouldn't have a lot of legal options. We can't exactly go to the public given the nature of our work.\n\nI can't stop thinking about what sort of experiments they might be conducting on my DNA, but I have to keep my mouth shut.\n\nI can't risk losing my job.</indent>";

				// Token: 0x0400B409 RID: 46089
				public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x0200205B RID: 8283
		public class B8_POLITEREQUEST
		{
			// Token: 0x040092D4 RID: 37588
			public static LocString TITLE = "Polite Request";

			// Token: 0x040092D5 RID: 37589
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x02002922 RID: 10530
			public class BODY
			{
				// Token: 0x0400B40A RID: 46090
				public static LocString EMAILHEADER = "<smallcaps>To: <b>All</b>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color></smallcaps>\n------------------\n";

				// Token: 0x0400B40B RID: 46091
				public static LocString CONTAINER1 = "<indent=5%>To whoever is entering Director Stern's office to move objects on her desk one inch to the left, please desist as she finds it quite unnerving.</indent>";

				// Token: 0x0400B40C RID: 46092
				public static LocString SIGNATURE = "\nThank-you,\n-Admin\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x0200205C RID: 8284
		public class A0_PRELIMINARYCALCULATIONS
		{
			// Token: 0x040092D6 RID: 37590
			public static LocString TITLE = "Preliminary Calculations";

			// Token: 0x040092D7 RID: 37591
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x02002923 RID: 10531
			public class BODY
			{
				// Token: 0x0400B40D RID: 46093
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>[REDACTED]</b>\nFrom: <b>[REDACTED]</b></color></smallcaps>\n------------------\n";

				// Token: 0x0400B40E RID: 46094
				public static LocString CONTAINER2 = "<indent=5%>Director,\n\nEven with dramatic optimization, we can't fit the massive volume of resources needed for a colony seed aboard the craft. Not even when calculating for a very small interplanetary travel duration.\n\nSome serious changes are gonna have to be made for this to work.</indent>";

				// Token: 0x0400B40F RID: 46095
				public static LocString SIGNATURE1 = "\nXOXO,\n[REDACTED]\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x0200205D RID: 8285
		public class B4_REMYPENS
		{
			// Token: 0x040092D8 RID: 37592
			public static LocString TITLE = "Re: MY PENS";

			// Token: 0x040092D9 RID: 37593
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x02002924 RID: 10532
			public class BODY
			{
				// Token: 0x0400B410 RID: 46096
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>ALL</b>\nFrom: <b>Admin</b><size=12><alpha=#AA> <admin@gravitas.nova></color></size></smallcaps>\n------------------\n";

				// Token: 0x0400B411 RID: 46097
				public static LocString CONTAINER2 = "<indent=5%>We would like to remind staff not to use the CC: All function for intra-office issues.\n\nIn the event of disputes or disruptive work behavior within the facility, please speak to HR directly.\n\nWe thank-you for your restraint.</indent>";

				// Token: 0x0400B412 RID: 46098
				public static LocString SIGNATURE1 = "\n-Admin\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x0200205E RID: 8286
		public class B3_RETEMPORALBOWUPDATE
		{
			// Token: 0x040092DA RID: 37594
			public static LocString TITLE = "RE: To Otto (Spec Changes)";

			// Token: 0x040092DB RID: 37595
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x02002925 RID: 10533
			public class BODY
			{
				// Token: 0x0400B413 RID: 46099
				public static LocString TITLEALT = "To Otto (Spec Changes)";

				// Token: 0x0400B414 RID: 46100
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Sklodowska</b><size=10><alpha=#AA> <msklodowska@gravitas.nova></size></color>\nFrom: <b>Mr. Kraus</b><alpha=#AA><size=10> <okraus@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B415 RID: 46101
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Mr. Kraus</b><alpha=#AA><size=10> <okraus@gravitas.nova></size></color>\nFrom: <b>Dr. Sklodowska</b><size=10><alpha=#AA> <msklodowska@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B416 RID: 46102
				public static LocString CONTAINER1 = "Thanks Doctor.\n\nPS, if you hit the \"Reply\" button instead of composing a new e-mail it makes it easier for people to tell what you're replying to. :)\n\nI appreciate it!\n\nMr. Kraus\n<size=11>Physics Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400B417 RID: 46103
				public static LocString CONTAINER2 = "Try not to take it too personally, it's probably just stress.\n\nThe Facility started going through a major overhaul not long before you got here, so I imagine the Director is having quite a time getting it all sorted out.\n\nThings will calm down once all the new departments are settled.\n\nDr. Sklodowska\n<size=11>Physics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x0200205F RID: 8287
		public class A1_RESEARCHGIANTARTICLE
		{
			// Token: 0x040092DC RID: 37596
			public static LocString TITLE = "Re: Have you seen this?";

			// Token: 0x040092DD RID: 37597
			public static LocString TITLE2 = "SUBJECT: Have you seen this?";

			// Token: 0x040092DE RID: 37598
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x02002926 RID: 10534
			public class BODY
			{
				// Token: 0x0400B418 RID: 46104
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><size=12><alpha=#AA> <jstern@gravitas.nova></size></color>\nFrom: <b>[REDACTED]</b></smallcaps>\n------------------\n";

				// Token: 0x0400B419 RID: 46105
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>[REDACTED]</b>\nFrom: <b>Director Stern</b><size=12><alpha=#AA> <jstern@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B41A RID: 46106
				public static LocString CONTAINER2 = "<indent=5%>Please pay it no mind. If any of these journals reach out to you, deny comment.</indent>";

				// Token: 0x0400B41B RID: 46107
				public static LocString CONTAINER3 = "<indent=5%>Director, are you aware of the articles that have been cropping up about us lately?</indent>";

				// Token: 0x0400B41C RID: 46108
				public static LocString CONTAINER4 = "<indent=10%><color=#F44A47>>[BROKEN LINK]</color> <alpha=#AA><smallcaps>the gravitas facility: questionable rise of a research giant</smallcaps></indent></color>";

				// Token: 0x0400B41D RID: 46109
				public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>Personnel Coordinator\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400B41E RID: 46110
				public static LocString SIGNATURE2 = "\n-Director Stern\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02002060 RID: 8288
		public class B2_TEMPORALBOWUPDATE
		{
			// Token: 0x040092DF RID: 37599
			public static LocString TITLE = "Spec Changes";

			// Token: 0x040092E0 RID: 37600
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x02002927 RID: 10535
			public class BODY
			{
				// Token: 0x0400B41F RID: 46111
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Sklodowska</b><size=10><alpha=#AA> <msklodowska@gravitas.nova></size></color>\nFrom: <b>Mr. Kraus</b><alpha=#AA><size=10> <okraus@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B420 RID: 46112
				public static LocString CONTAINER2 = "Dr. Sklodowska, could I ask you to forward me the new spec changes to the Temporal Bow?\n\nThe Director completely ignored me when I asked for a project update this morning. She walked right past me in the hall - I didn't realize I was that far down on the food chain. :(\n\nMr. Kraus\nPhysics Department\nThe Gravitas Facility";
			}
		}

		// Token: 0x02002061 RID: 8289
		public class A5_THEJANITOR
		{
			// Token: 0x040092E1 RID: 37601
			public static LocString TITLE = "Re: omg the janitor";

			// Token: 0x040092E2 RID: 37602
			public static LocString TITLE2 = "SUBJECT: Re: omg the janitor";

			// Token: 0x040092E3 RID: 37603
			public static LocString TITLE3 = "SUBJECT: omg the janitor";

			// Token: 0x040092E4 RID: 37604
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x02002928 RID: 10536
			public class BODY
			{
				// Token: 0x0400B421 RID: 46113
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Summers</b><size=12><alpha=#AA> <jsummers@gravitas.nova></color></size>\nFrom: <b>Dr. Jones</b><size=12><alpha=#AA> <ejones@gravitas.nova></color></size></smallcaps>\n------------------\n";

				// Token: 0x0400B422 RID: 46114
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Dr. Jones</b><size=12><alpha=#AA> <ejones@gravitas.nova></color></size>\nFrom: <b>Dr. Summers</b><size=12><alpha=#AA> <jsummers@gravitas.nova></color></size></smallcaps>\n------------------\n";

				// Token: 0x0400B423 RID: 46115
				public static LocString CONTAINER2 = "<indent=5%><i>Pfft,</i> whatever.</indent>";

				// Token: 0x0400B424 RID: 46116
				public static LocString CONTAINER3 = "<indent=5%>Aw, he's really nice if you get to know him though. Really dependable too. One time I busted a wheel off my office chair and he got me a new one in like, two minutes. I think he's just sweaty because he works so hard.</indent>";

				// Token: 0x0400B425 RID: 46117
				public static LocString CONTAINER4 = "<indent=5%>OMIGOSH have you seen our building's janitor? He totally smells and he has sweat stains under his armpits like EVERY time I see him. SO embarrassing.</indent>";

				// Token: 0x0400B426 RID: 46118
				public static LocString SIGNATURE1 = "\nXOXO,\nDr. Jones\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400B427 RID: 46119
				public static LocString SIGNATURE2 = "\n-Dr. Summers\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02002062 RID: 8290
		public class A2_THERMODYNAMICLAWS
		{
			// Token: 0x040092E5 RID: 37605
			public static LocString TITLE = "The Laws of Thermodynamics";

			// Token: 0x040092E6 RID: 37606
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x02002929 RID: 10537
			public class BODY
			{
				// Token: 0x0400B428 RID: 46120
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Mr. Kraus</b><alpha=#AA><size=12> <okraus@gravitas.nova></size></color>\nFrom: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400B429 RID: 46121
				public static LocString CONTAINER1 = "<indent=5%><i>Hello</i> Mr. Kraus!\n\nI was just e-mailing you after our little chat today to pass along something you might like to read - I think you'll find it super useful in your research!\n\n</indent>";

				// Token: 0x0400B42A RID: 46122
				public static LocString CONTAINER2 = "<indent=10%><b>FIRST LAW</b></indent>\n<indent=15%>Energy can neither be created or destroyed, only change forms.</indent>";

				// Token: 0x0400B42B RID: 46123
				public static LocString CONTAINER3 = "<indent=10%><b>SECOND LAW</b></indent>\n<indent=15%>Entropy in an isolated system that is not in equilibrium tends to increase over time, approaching the maximum value at equilibrium.</indent>";

				// Token: 0x0400B42C RID: 46124
				public static LocString CONTAINER4 = "<indent=10%><b>THIRD LAW</b></indent>\n<indent=15%>Entropy in a system approaches a constant minimum as temperature approaches absolute zero.</indent>";

				// Token: 0x0400B42D RID: 46125
				public static LocString CONTAINER5 = "<indent=10%><b>ZEROTH LAW</b></indent>\n<indent=15%>If two thermodynamic systems are in thermal equilibrium with a third, then they are in thermal equilibrium with each other.</indent>";

				// Token: 0x0400B42E RID: 46126
				public static LocString CONTAINER6 = "<indent=5%>\nIf this is too complicated for you, you can come by to chat. I'd be <i>thrilled</i> to answer your questions. ;)</indent>";

				// Token: 0x0400B42F RID: 46127
				public static LocString SIGNATURE1 = "\nXOXO,\nDr. Jones\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02002063 RID: 8291
		public class TIMEOFFAPPROVED
		{
			// Token: 0x040092E7 RID: 37607
			public static LocString TITLE = "Vacation Request Approved";

			// Token: 0x040092E8 RID: 37608
			public static LocString TITLE2 = "SUBJECT: Vacation Request Approved";

			// Token: 0x040092E9 RID: 37609
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x0200292A RID: 10538
			public class BODY
			{
				// Token: 0x0400B430 RID: 46128
				public static LocString EMAILHEADER = "<smallcaps>To: <b>Dr. Ross</b><size=12><alpha=#AA> <dross@gravitas.nova></size></color>\nFrom: <b>Admin</b><size=12><alpha=#AA> <admin@gravitas.nova></color></size></smallcaps>\n------------------\n";

				// Token: 0x0400B431 RID: 46129
				public static LocString CONTAINER = "<indent=5%><b>Vacation Request Granted</b>\nGood luck, Devon!\n\n<alpha=#AA><smallcaps><indent=10%> Vacation Request [May 18th-20th]\nReason: Time off request for attendance of the Blogjam Awards (\"Toast of the Town\" nominated in the Freshest Food Blog category).</indent></smallcaps></color></indent>";

				// Token: 0x0400B432 RID: 46130
				public static LocString SIGNATURE = "\n-Admin\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02002064 RID: 8292
		public class BASIC_FABRIC
		{
			// Token: 0x040092EA RID: 37610
			public static LocString TITLE = "Reed Fiber";

			// Token: 0x040092EB RID: 37611
			public static LocString SUBTITLE = "Textile Ingredient";

			// Token: 0x0200292B RID: 10539
			public class BODY
			{
				// Token: 0x0400B433 RID: 46131
				public static LocString CONTAINER1 = string.Concat(new string[]
				{
					"A ball of raw cellulose harvested from a ",
					UI.FormatAsLink("Thimble Reed", "BASICFABRICPLANT"),
					".\n\nIt is used in the production of ",
					UI.FormatAsLink("Clothing", "EQUIPMENT"),
					" and textiles."
				});
			}
		}

		// Token: 0x02002065 RID: 8293
		public class CRAB_SHELL
		{
			// Token: 0x040092EC RID: 37612
			public static LocString TITLE = "Pokeshell Molt";

			// Token: 0x040092ED RID: 37613
			public static LocString SUBTITLE = "Critter Byproduct";

			// Token: 0x040092EE RID: 37614
			public static LocString CONTAINER1 = "An exoskeleton discarded by an aquatic critter.";

			// Token: 0x0200292C RID: 10540
			public class BABY_CRAB_SHELL
			{
				// Token: 0x0400B434 RID: 46132
				public static LocString TITLE = "Small Pokeshell Molt";

				// Token: 0x0400B435 RID: 46133
				public static LocString SUBTITLE = "Critter Byproduct";

				// Token: 0x0400B436 RID: 46134
				public static LocString CONTAINER1 = "An adorable little exoskeleton discarded by a baby aquatic critter.";
			}
		}

		// Token: 0x02002066 RID: 8294
		public class DATABANK
		{
			// Token: 0x040092EF RID: 37615
			public static LocString TITLE = "Data Banks";

			// Token: 0x040092F0 RID: 37616
			public static LocString SUBTITLE = "Information Technology";

			// Token: 0x0200292D RID: 10541
			public class BODY
			{
				// Token: 0x0400B437 RID: 46135
				public static LocString CONTAINER1 = "Data Banks are a form of portable storage media. They are prized for their non-volatility, robustness, and practical research applications.\n\nThey are not foldable.";
			}
		}

		// Token: 0x02002067 RID: 8295
		public class EGG_SHELL
		{
			// Token: 0x040092F1 RID: 37617
			public static LocString TITLE = "Egg Shell";

			// Token: 0x040092F2 RID: 37618
			public static LocString SUBTITLE = "Critter Byproduct";

			// Token: 0x0200292E RID: 10542
			public class BODY
			{
				// Token: 0x0400B438 RID: 46136
				public static LocString CONTAINER1 = "The shards left over from the protective walls of a baby critter's first home.";
			}
		}

		// Token: 0x02002068 RID: 8296
		public class ELECTROBANK
		{
			// Token: 0x040092F3 RID: 37619
			public static LocString TITLE = "Power Banks";

			// Token: 0x040092F4 RID: 37620
			public static LocString SUBTITLE = "Portable Storage";

			// Token: 0x0200292F RID: 10543
			public class BODY
			{
				// Token: 0x0400B439 RID: 46137
				public static LocString CONTAINER1 = string.Concat(new string[]
				{
					"Power Banks are portable ",
					UI.FormatAsLink("Power", "POWER"),
					" storage containers that can be used to supply electricity to mobile entities and isolated areas.\n\n",
					UI.FormatAsLink("Organic Power Banks", "ELECTROBANK"),
					" made from flora and fauna-based ingredients are single-use, as are ",
					UI.FormatAsLink("Nuclear Power Banks", "ELECTROBANK"),
					".\n\n",
					UI.FormatAsLink("Eco Power Banks", "ELECTROBANK"),
					" are rechargeable and can be reused indefinitely unless they are exposed to water damage."
				});
			}
		}

		// Token: 0x02002069 RID: 8297
		public class VARIANT_GOLD
		{
			// Token: 0x040092F5 RID: 37621
			public static LocString TITLE = "Regal Bammoth Crest";

			// Token: 0x040092F6 RID: 37622
			public static LocString SUBTITLE = "Critter Byproduct";

			// Token: 0x02002930 RID: 10544
			public class BODY
			{
				// Token: 0x0400B43A RID: 46138
				public static LocString CONTAINER1 = "Heavy was the head that wore this crest, until it was relieved of its burden by a helpful Duplicant.";
			}
		}

		// Token: 0x0200206A RID: 8298
		public class LUMBER
		{
			// Token: 0x040092F7 RID: 37623
			public static LocString TITLE = "Wood";

			// Token: 0x040092F8 RID: 37624
			public static LocString SUBTITLE = "Renewable Resource";

			// Token: 0x02002931 RID: 10545
			public class BODY
			{
				// Token: 0x0400B43B RID: 46139
				public static LocString CONTAINER1 = string.Concat(new string[]
				{
					"Thick logs of ",
					UI.FormatAsLink("Wood", "WOOD"),
					" harvested from ",
					UI.FormatAsLink("Arbor Trees", "FOREST_TREE"),
					", ",
					UI.FormatAsLink("Oakshells", "CRABWOOD"),
					" and other natural sources.\n\nWood Logs are used in the production of ",
					UI.FormatAsLink("Heat", "HEAT"),
					" and ",
					UI.FormatAsLink("Power", "POWER"),
					". They are also a useful building material."
				});
			}
		}

		// Token: 0x0200206B RID: 8299
		public class SWAMPLILYFLOWER
		{
			// Token: 0x040092F9 RID: 37625
			public static LocString TITLE = "Balm Lily Flower";

			// Token: 0x040092FA RID: 37626
			public static LocString SUBTITLE = "Medicinal Herb";

			// Token: 0x02002932 RID: 10546
			public class BODY
			{
				// Token: 0x0400B43C RID: 46140
				public static LocString CONTAINER1 = "Balm Lily Flowers bloom on " + UI.FormatAsLink("Balm Lily", "SWAMPLILY") + " plants.\n\nThey have a wide range of medicinal applications, and have been shown to be a particularly effective antidote for respiratory illnesses.\n\nThe intense perfume emitted by their vivid petals is best described as \"dizzying.\"";
			}
		}

		// Token: 0x0200206C RID: 8300
		public class VARIANT_WOOD_SHELL
		{
			// Token: 0x040092FB RID: 37627
			public static LocString TITLE = "Oakshell Molt";

			// Token: 0x040092FC RID: 37628
			public static LocString SUBTITLE = "Critter Byproduct";

			// Token: 0x040092FD RID: 37629
			public static LocString CONTAINER1 = "A splintery exoskeleton discarded by an aquatic critter.";

			// Token: 0x02002933 RID: 10547
			public class BABY_VARIANT_WOOD_SHELL
			{
				// Token: 0x0400B43D RID: 46141
				public static LocString TITLE = "Small Oakshell Molt";

				// Token: 0x0400B43E RID: 46142
				public static LocString SUBTITLE = "Critter Byproduct";

				// Token: 0x0400B43F RID: 46143
				public static LocString CONTAINER1 = "A cute little splintery exoskeleton discarded by a baby aquatic critter.";
			}
		}

		// Token: 0x0200206D RID: 8301
		public class CRYOTANKWARNINGS
		{
			// Token: 0x040092FE RID: 37630
			public static LocString TITLE = "CRYOTANK SAFETY";

			// Token: 0x040092FF RID: 37631
			public static LocString SUBTITLE = "IMPORTANT OPERATING INSTRUCTIONS FOR THE CRYOTANK 3000";

			// Token: 0x02002934 RID: 10548
			public class BODY
			{
				// Token: 0x0400B440 RID: 46144
				public static LocString CONTAINER1 = "    • Do not leave the contents of the Cryotank 3000 unattended unless an apocalyptic disaster has left you no choice.\n\n    • Ensure that the Cryotank 3000 has enough battery power to remain active for at least 6000 years.\n\n    • Do not attempt to defrost the contents of the Cryotank 3000 while it is submerged in molten hot lava.\n\n    • Use only a qualified Gravitas Cryotank repair facility to repair your Cryotank 3000. Attempting to service the device yourself will void the warranty.\n\n    • Do not put food in the Cryotank 3000. The Cryotank 3000 is not a refrigerator.\n\n    • Do not allow children to play in the Cryotank 3000. The Cryotank 3000 is not a toy.\n\n    • While the Cryotank 3000 is able to withstand a nuclear blast, Gravitas and its subsidiaries are not responsible for what may happen in the resulting nuclear fallout.\n\n    • Wait at least 5 minutes after being unfrozen from the Cryotank 3000 before operating heavy machinery.\n\n    • Each Cryotank 3000 is good for only one use.\n\n";
			}
		}

		// Token: 0x0200206E RID: 8302
		public class EVACUATION
		{
			// Token: 0x04009300 RID: 37632
			public static LocString TITLE = "! EVACUATION NOTICE !";

			// Token: 0x04009301 RID: 37633
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x02002935 RID: 10549
			public class BODY
			{
				// Token: 0x0400B441 RID: 46145
				public static LocString CONTAINER1 = "<smallcaps>Attention all Gravitas personnel\n\nEvacuation protocol in effect\n\nReactor meltdown in bioengineering imminent\n\nRemain calm and proceed to emergency exits\n\nDo not attempt to use elevators</smallcaps>";
			}
		}

		// Token: 0x0200206F RID: 8303
		public class C7_FIRSTCOLONY
		{
			// Token: 0x04009302 RID: 37634
			public static LocString TITLE = "Director's Notes";

			// Token: 0x04009303 RID: 37635
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x02002936 RID: 10550
			public class BODY
			{
				// Token: 0x0400B442 RID: 46146
				public static LocString CONTAINER1 = "The first experiments with establishing a colony off planet were an unmitigated disaster. Without outside help, our current Artificial Intelligence was completely incapable of making the kind of spontaneous decisions needed to deal with unforeseen circumstances. Additionally, the colony subjects lacked the forethought to even build themselves toilet facilities, even after soiling themselves repeatedly.\n\nWhile initial experiments in a lab setting were encouraging, our latest operation on non-Terra soil revealed some massive inadequacies to our system. If this idea is ever going to work, we will either need to drastically improve the AI directing the subjects, or improve the brains of our Duplicants to the point where they possess higher cognitive functions.\n\nGiven the disastrous complications that I could foresee arising if our Duplicants were made less supplicant, I'm leaning toward a push to improve our Artificial Intelligence.\n\nMeanwhile, we will have to send a clean-up crew to destroy all evidence of our little experiment beneath the Ceres' surface. We can't risk anyone discovering the remnants of our failed colony, even if that's unlikely to happen for another few decades at least.\n\n(Sometimes it boggles my mind how much further behind Gravitas the rest of the world is.)";
			}
		}

		// Token: 0x02002070 RID: 8304
		public class A8_FIRSTSUCCESS
		{
			// Token: 0x04009304 RID: 37636
			public static LocString TITLE = "Encouraging Results";

			// Token: 0x04009305 RID: 37637
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x02002937 RID: 10551
			public class BODY
			{
				// Token: 0x0400B443 RID: 46147
				public static LocString CONTAINER1 = "We've successfully compressed and expanded small portions of time under .03 milliseconds. This proves that time is something that can be physically acted upon, suggesting that our vision is attainable.\n\nAn unintentional consequence of both the expansion and contraction of time is the creation of a \"vacuum\" in the space between the affected portion of time and the much more expansive unaffected portions.\n\nSo far, we are seeing that the unaffected time on either side of the manipulated portion will expand or contract to fill the vacuum, although we are unsure how far-reaching this consequence is or what effect it has on the laws of the natural universe. At the end of all compression and expansion experiments, alterations to time are undone and leave no lasting change.";
			}
		}

		// Token: 0x02002071 RID: 8305
		public class B8_MAGAZINEARTICLE
		{
			// Token: 0x04009306 RID: 37638
			public static LocString TITLE = "Nucleoid Article";

			// Token: 0x04009307 RID: 37639
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x02002938 RID: 10552
			public class BODY
			{
				// Token: 0x0400B444 RID: 46148
				public static LocString CONTAINER1 = "<b>Incredible Technology From Independent Lab Harnesses Time into Energy</b>";

				// Token: 0x0400B445 RID: 46149
				public static LocString CONTAINER2 = "Scientists from the recently founded Gravitas Facility have unveiled their first technology prototype, dubbed the \"Temporal Bow\". It is a device which manipulates the 4th dimension to generate infinite, clean and renewable energy.\n\nWhile it may sound like something from science fiction, facility founder Dr. Jacquelyn Stern confirms that it is very much real.\n\n\"It has already been demonstrated that Newton's Second Law of Motion can be violated by negative mass superfluids under the correct lab conditions,\" she says.\n\n\"If the Laws of Motion can be bent and altered, why not the Laws of Thermodynamics? That was the main intent behind this project.\"\n\nThe Temporal Bow works by rapidly vibrating sections of the 4th dimension to send small quantities of mass forward and backward in time, generating massive amounts of energy with virtually no waste.\n\n\"The fantastic thing about using the 4th dimension as fuel,\" says Stern, \"is that it is really, categorically infinite\".\n\nFor those eagerly awaiting the prospect of human time travel, don't get your hopes up just yet. The Facility says that although they have successfully transported matter through time, the technology was expressly developed for the purpose of energy generation and is ill-equipped for human transportation.";
			}
		}

		// Token: 0x02002072 RID: 8306
		public class MYSTERYAWARD
		{
			// Token: 0x04009308 RID: 37640
			public static LocString TITLE = "Nanotech Article";

			// Token: 0x04009309 RID: 37641
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x02002939 RID: 10553
			public class BODY
			{
				// Token: 0x0400B446 RID: 46150
				public static LocString CONTAINER1 = "<b>Mystery Project Wins Nanotech Award</b>";

				// Token: 0x0400B447 RID: 46151
				public static LocString CONTAINER2 = "Last night's Worldwide Nanotech Awards has sparked controversy in the scientific community after it was announced that the top prize had been awarded to a project whose details could not be publicly disclosed.\n\nThe highly classified paper was presented to the jury in a closed session by lead researcher Dr. Liling Pei, recipient of the inaugural Gravitas Accelerator Scholarship at the Elion University of Science and Technology.\n\nHead judge Dr. Elias Balko acknowledges that it was unorthodox, but defends the decision. \"We're scientists - it's our job to push boundaries.\"\n\nPei was awarded the coveted Halas Medal, the top prize for innovation in the field.\n\n\"I wish I could tell you more,\" says Pei. \"I'm SO grateful to the WNA for this great honor, and to Dr. Stern for the funding that made it all possible. This is going to change everything about...well, everything.\"\n\nThis is the second time that Pei has made headlines. Last year, the striking young nanoscientist won the Miss Planetary Belle pageant's talent show with a live demonstration of nanorobots weaving a ballgown out of fibers harvested from common houseplants.\n\nPei joins the team at the Gravitas Facility early next month.";
			}
		}

		// Token: 0x02002073 RID: 8307
		public class A7_NEUTRONIUM
		{
			// Token: 0x0400930A RID: 37642
			public static LocString TITLE = "Byproduct Notes";

			// Token: 0x0400930B RID: 37643
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x0200293A RID: 10554
			public class BODY
			{
				// Token: 0x0400B448 RID: 46152
				public static LocString CONTAINER1 = "[LOG BEGINS]\n\nDirector: I've determined the substance to be metallic in nature. The exact cause of its formation is still unknown, though I believe it to be something of an autoimmune reaction of the natural universe, a quarantining of foreign material to prevent temporal contamination.\n\nDirector: A method has yet to be found that can successfully remove the substance from an affected object, and the larger implication that two molecularly, temporally identical objects cannot coexist at one point in time has dire implications for all time manipulation technology research, not just the Bow.\n\nDirector: For the moment I have dubbed the substance \"Neutronium\", and assigned it a theoretical place on the table of elements. Research continues.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02002074 RID: 8308
		public class A9_NEUTRONIUMAPPLICATIONS
		{
			// Token: 0x0400930C RID: 37644
			public static LocString TITLE = "Possible Applications";

			// Token: 0x0400930D RID: 37645
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x0200293B RID: 10555
			public class BODY
			{
				// Token: 0x0400B449 RID: 46153
				public static LocString CONTAINER1 = "[LOG BEGINS]\n\nDirector: Temporal energy can be reconfigured to vibrate the matter constituting Neutronium at just the right frequency to break it down and disperse it.\n\nDirector: However, it is difficult to stabilize and maintain this reconfigured energy long enough to effectively remove practical amounts of Neutronium in real-life scenarios.\n\nDirector: I am looking into making this technology more reliable and compact - this data could potentially have uses in the development of some sort of all-purpose disintegration ray.\n\n[END LOG]";
			}
		}

		// Token: 0x02002075 RID: 8309
		public class PLANETARYECHOES
		{
			// Token: 0x0400930E RID: 37646
			public static LocString TITLE = "Planetary Echoes";

			// Token: 0x0400930F RID: 37647
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x0200293C RID: 10556
			public class BODY
			{
				// Token: 0x0400B44A RID: 46154
				public static LocString TITLE1 = "Echo One";

				// Token: 0x0400B44B RID: 46155
				public static LocString TITLE2 = "Echo Two";

				// Token: 0x0400B44C RID: 46156
				public static LocString CONTAINER1 = "Olivia: We've double-checked our observational equipment and the computer's warm-up is almost finished. We have precautionary personnel in place ready to start a shutdown in the event of a failure.\n\nOlivia: It's time.\n\nJackie: Right.\n\nJackie: Spin the machine up slowly so we can monitor for any abnormal power fluctuations. We start on \"3\".\n\nJackie: \"1\"... \"2\"...\n\nJackie: \"3\".\n\n[There's a metallic clunk. The baritone whirr of machinery can be heard.]\n\nJackie: Something's not right.\n\nOlivia: It's the container... the atom is vibrating too fast.\n\n[The whir of the machinery peels up an octave into a mechanical screech.]\n\nOlivia: W-we have to abort!\n\nJackie: No, not yet. Drop power from the coolant system and use it to bolster the container. It'll stabilize.\n\nOlivia: But without coolant--\n\nJackie: It will stabilize!\n\n[There's a sharp crackle of electricity.]\n\nOlivia: Drop 40% power from the coolant systems, reroute everything we have to the atomic container! \n\n[The whirring reaches a crescendo, then calms into a steady hum.]\n\nOlivia: That did it. The container is stabilizing.\n\n[Jackie sighs in relief.]\n\nOlivia: But... Look at these numbers.\n\nJackie: My god. Are these real?\n\nOlivia: Yes, I'm certain of it. Jackie, I think we did it.\n\nOlivia: I think we created an infinite energy source.\n------------------\n";

				// Token: 0x0400B44D RID: 46157
				public static LocString CONTAINER2 = "Olivia: What on earth is this?\n\n[An open palm slams papers down on a desk.]\n\nOlivia: These readings show that hundreds of kilograms of Neutronium are building up in the machine every shift. When were you going to tell me?\n\nJackie: I'm handling it.\n\nOlivia: We don't have the luxury of taking shortcuts. Not when safety is on the line.\n\nJackie: I think I'm capable of overseeing my own safety.\n\nOlivia: I-I'm not just concerned about <i>your</i> safety! We don't understand the longterm implications of what we're developing here... the manipulations we conduct in this facility could have rippling effects throughout the world, maybe even the universe.\n\nJackie: Don't be such a fearmonger. It's not befitting of a scientist. Besides, I'll remind you this research has the potential to stop the fuel wars in their tracks and end the suffering of thousands. Every day we spend on trials here delays that.\n\nOlivia: It's dangerous.\n\nJackie: Your concern is noted.\n------------------\n";
			}
		}

		// Token: 0x02002076 RID: 8310
		public class SCHOOLNEWSPAPER
		{
			// Token: 0x04009310 RID: 37648
			public static LocString TITLE = "Campus Newspaper Article";

			// Token: 0x04009311 RID: 37649
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x0200293D RID: 10557
			public class BODY
			{
				// Token: 0x0400B44E RID: 46158
				public static LocString CONTAINER1 = "<b>Party Time for Local Students</b>";

				// Token: 0x0400B44F RID: 46159
				public static LocString CONTAINER2 = "Students at the Elion University of Science and Technology have held an unconventional party this weekend.\n\nWhile their peers may have been out until the wee hours wearing lampshades on their heads and drawing eyebrows on sleeping colleagues, students Jackie Stern and Olivia Broussard spent the weekend in their dorm, refreshments and decorations ready, waiting for the arrival of the guests of honor: themselves.\n\nThe two prospective STEM students, who study theoretical physics with a focus on the workings of space time, conducted the experiment under the assumption that, were their theories about the malleability of space time to ever come to fruition, their future selves could travel back in time to greet them at the party, proving the existence of time travel.\n\nThey weren't inconsiderate of their future selves' busy schedules though; should the guests of honor be unable to attend, they were encouraged to send back a message using the codeword \"Hourglass\" to communicate that, while they certainly wanted to come, they were simply unable.\n\nSadly no one RSVP'd or arrived to the party, but that did not dishearten Olivia or Jackie.\n\nAs Olivia put it, \"It just meant more snacks for us!\"";
			}
		}

		// Token: 0x02002077 RID: 8311
		public class B6_TIMEMUSINGS
		{
			// Token: 0x04009312 RID: 37650
			public static LocString TITLE = "Director's Notes";

			// Token: 0x04009313 RID: 37651
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x0200293E RID: 10558
			public class BODY
			{
				// Token: 0x0400B450 RID: 46160
				public static LocString CONTAINER1 = "When we discuss Time as a concrete aspect of the universe, not seconds on a clock or perceptions of the mind, it is important first of all to establish that we are talking about a unique dimension that layers into the three physical dimensions of space: length, width, depth.\n\nWe conceive of Real Time as a straight line, one dimensional, uncurved and stretching forward infinitely. This is referred to as the \"Arrow of Time\".\n\nLogically this Arrow can move only forward and can never be reversed, as such a reversal would break the natural laws of the universe. Effect would precede cause and universal entropy would be undone in a blatant violation of the Second Law.\n\nStill, one can't help but wonder; what if the Arrow's trajectory could be curved? What if it could be redirected, guided, or loosed? What if we could create Time's Bow?";
			}
		}

		// Token: 0x02002078 RID: 8312
		public class B7_TIMESARROWTHOUGHTS
		{
			// Token: 0x04009314 RID: 37652
			public static LocString TITLE = "Time's Arrow Thoughts";

			// Token: 0x04009315 RID: 37653
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x0200293F RID: 10559
			public class BODY
			{
				// Token: 0x0400B451 RID: 46161
				public static LocString CONTAINER1 = "I've been unable to shake the notion of the Bow.\n\nThe thought of its mechanics are too intriguing, and I can only dream of the mark such a device would make upon the world -- imagine, a source of inexhaustible energy!\n\nSo many of humanity's problems could be solved with this one invention - domestic energy, environmental pollution, <i>the fuel wars</i>.\n\nI have to pursue this dream, no matter what.";
			}
		}

		// Token: 0x02002079 RID: 8313
		public class C8_TIMESORDER
		{
			// Token: 0x04009316 RID: 37654
			public static LocString TITLE = "Time's Order";

			// Token: 0x04009317 RID: 37655
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x02002940 RID: 10560
			public class BODY
			{
				// Token: 0x0400B452 RID: 46162
				public static LocString CONTAINER1 = "We have been successfully using the Temporal Bow now for some time with no obvious consequences. I should be happy that this works so well, but several questions gnaw at my brain late at night.\n\nIf Time's Arrow is moving forward the Laws of Entropy declare that the universe should be moving from a state of order to one of chaos. If the Temporal Bow bends to a previous point in time to a point when things were more orderly, logic would dictate that we are making this point more chaotic by taking things from it. All known laws of the natural universe suggests that this should have affected our Present Day, but all evidence points to that not being true. It appears the theory that we cannot change our past was incorrect!\n\nThis suggests that Time is, in fact, not an arrow but several arrows, each pointing different directions. Fundamentally, this proves the existence of other timelines - different dimensions - some of which we can assume have also built their own Temporal Bow.\n\nThe promise of crossing this final dimensional threshold is too tempting. Imagine what things Gravitas has invented in another dimension!! I must find a way to tear open the fabric of spacetime and tap into the limitless human potential of a thousand alternate timelines.";
			}
		}

		// Token: 0x0200207A RID: 8314
		public class B5_ANTS
		{
			// Token: 0x04009318 RID: 37656
			public static LocString TITLE = "Ants";

			// Token: 0x04009319 RID: 37657
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x02002941 RID: 10561
			public class BODY
			{
				// Token: 0x0400B453 RID: 46163
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, B556]</smallcaps>\n\n[LOG BEGINS]\n\nTechnician: <i>Atta cephalotes</i>. What sort of experiment are you doing with these?\n\nDirector: No experiment. I just find them interesting. Don't you?\n\nTech: Not really?\n\nDirector: You ought to. Very efficient. They perfected farming millions of years before humans.\n\n(sound of tapping on glass)\n\nDirector: An entire colony led by and in service to its queen. Each organism knows its role.\n\nTech: I have the results from the power tests, director.\n\nDirector: And?\n\nTech: Negative, ma'am.\n\nDirector: I see. You know, another admirable quality of ants occurs to me. They can pull twenty times their own weight.\n\nTech: I'm not sure I follow, ma'am.\n\nDirector: Are you pulling your weight, Doctor?\n\n[LOG ENDS]";
			}
		}

		// Token: 0x0200207B RID: 8315
		public class A8_CLEANUPTHEMESS
		{
			// Token: 0x0400931A RID: 37658
			public static LocString TITLE = "Cleaning Up The Mess";

			// Token: 0x0400931B RID: 37659
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x02002942 RID: 10562
			public class BODY
			{
				// Token: 0x0400B454 RID: 46164
				public static LocString CONTAINER1 = "I cleaned up a few messes in my time, but ain't nothing like the mess I seen today in that bio lab. Green goop all over the floor, all over the walls. Murky tubes with what look like human shapes floating in them.\n\nThey think old Mr. Gunderson ain't got smarts enough to put two and two together, but I got eyes, don't I?\n\nAin't nobody ever pay attention to the janitor.\n\nBut the janitor pays attention to everybody.\n\n-Mr. Stinky Gunderson";
			}
		}

		// Token: 0x0200207C RID: 8316
		public class CRITTERDELIVERY
		{
			// Token: 0x0400931C RID: 37660
			public static LocString TITLE = "Critter Delivery";

			// Token: 0x0400931D RID: 37661
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x02002943 RID: 10563
			public class BODY
			{
				// Token: 0x0400B455 RID: 46165
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: B482, B759, C094]</smallcaps>\n\n[LOG BEGINS]\n\nSecurity Guard 1: Hey hey! Welcome back.\n\nSecurity Guard 2: Hand on the scanner, please.\n\nCourier: Sure thing, lemme just...\n\nCourier: Whoops-- thanks, Steve. These little fellas are a two-hander for sure.\n\n(sound of furry noses snuffling on cardboard)\n\nSecurity Guard 2: Follow me, please.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x0200207D RID: 8317
		public class B2_ELLIESBIRTHDAY
		{
			// Token: 0x0400931E RID: 37662
			public static LocString TITLE = "Office Cake";

			// Token: 0x0400931F RID: 37663
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x02002944 RID: 10564
			public class BODY
			{
				// Token: 0x0400B456 RID: 46166
				public static LocString CONTAINER1 = "Joshua: Hey Mr. Kraus, I'm passing around the collection pan. Wanna pitch in a couple bucks to get a cake for Ellie?\n\nOtto: Uh... I think I'll pass.\n\nJoshua: C'mon Otto, it's her birthday.\n\nOtto: Alright, fine. But this is all I have on me.\n\nOtto: I don't get why you hang out with her. Isn't she kind of... you know, mean?\n\nJoshua: Even the meanest people have a little niceness in them somewhere.\n\nOtto: Huh. Good luck finding it.\n\nJoshua: Thanks for the cake money, Otto.\n------------------\n";

				// Token: 0x0400B457 RID: 46167
				public static LocString CONTAINER2 = "Ellie: Nice cake. I bet it wasn't easy to like, strong-arm everyone into buying it.\n\nJoshua: You know, if you were a little nicer to people they might want to spend more time with you.\n\nEllie: Pfft, please. Friends are about <i>quality</i>, not quantity, Josh.\n\nJoshua: Wow! Was that a roundabout compliment I just heard?\n\nEllie: What? Gross, ew. Stop that.\n\nJoshua: Oh, don't worry, I won't tell anyone. I'm not much of a gossip.";
			}
		}

		// Token: 0x0200207E RID: 8318
		public class A7_EMPLOYEEPROCESSING
		{
			// Token: 0x04009320 RID: 37664
			public static LocString TITLE = "Employee Processing";

			// Token: 0x04009321 RID: 37665
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x02002945 RID: 10565
			public class BODY
			{
				// Token: 0x0400B458 RID: 46168
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, A435, B111]</smallcaps>\n\n[LOG BEGINS]\n\nTechnician: Thank you for the fingerprints, doctor. We just need a quick voice sample, then you can be on your way.\n\nDr. Broussard: Wow Jackie, your new security's no joke.\n\nDirector: Please address me as \"Director\" while on Facility grounds.\n\nDr. Broussard: ...R-right.\n\n(clicking)\n\nTechnician: This should only take a moment. Speak clearly and the system will derive a vocal signature for you.\n\nTechnician: When you're ready.\n\n(throat clearing)\n\nDr. Broussard: Security code B111, Dr. Olivia Broussard. Gravitas Facility Bioengineering Department.\n\n(pause)\n\nTechnician: Great.\n\nDr. Broussard: What was that light just now?\n\nDirector: A basic security scan. No need for concern.\n\n(machine printing)\n\nTechnician: Here's your ID. You should have access to all doors in the facility now, Dr. Broussard.\n\nDr. Broussard: Thank you.\n\nDirector: Come along, Doctor.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x0200207F RID: 8319
		public class C01_EVIL
		{
			// Token: 0x04009322 RID: 37666
			public static LocString TITLE = "Evil";

			// Token: 0x04009323 RID: 37667
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x02002946 RID: 10566
			public class BODY
			{
				// Token: 0x0400B459 RID: 46169
				public static LocString CONTAINER1 = "Clearly Nikola is evil. He has some kind of scheme going on that he's keeping secret from the rest of Gravitas and I haven't been able to crack what that is because it's offline and he's always at his computer. Whenever I ask him what he's up to he says I wouldn't understand. Pfft! We both went through the same particle physics classes, buddy. Just because you mash a keyboard and I adjust knobs does not mean I don't know what the Time Containment Field does.\n\nAnd then today I dropped a wrench and Nikola nearly jumped out of his skin! He spun around and screamed at me never to do that again. And then when I said, \"Geez, it's not the end of the world,\" he was like, \"Yeah, it's not like the world will blow up if I get this wrong\" really sarcastic-like.\n\nWhich technically is true. If the Time Containment Field were to break down, the Temporal Bow could theoretically blow up the world. But that's why there are safety systems in place. And safety systems on safety systems. And then safety systems on top of that. But then again he built all the safety systems, so if he wanted to...\n------------------\n";

				// Token: 0x0400B45A RID: 46170
				public static LocString CONTAINER2 = "I decided to get into work early today but when I got in Nikola was already there and it looked like he hadn't been home all weekend. He was pacing back and forth in the lab, monologuing but not like an evil villain. Like someone who hadn't slept in a week.\n\n\"Ruby,\" he said. \"You have to promise me that if anything goes wrong you'll turn on this machine. They're pushing it too far. The printing pods are pushing the...It's too much - TOO MUCH! Something's going to blow. I tried... I'm trying to save it. Not the Earth. There's no hope for the Earth, it's all going to...\" then he made this exploding sound. \"But the Universe. Time itself. It could all go, don't you see? This machine can contain it. Put a Temporal Containment Field around the Earth so time itself doesn't break down and...and...\"\n\nThen all of a sudden these security guys came in. New guys. People I haven't seen before. And they just took him away. Then they took me to a room and asked me all kinds of questions and I answered them, I guess. I don't remember much because the whole time I was thinking - What if I was wrong? What if he's not evil, but Gravitas is?\n\nWhat if I was wrong and what if he's right?\n------------------\n";

				// Token: 0x0400B45B RID: 46171
				public static LocString CONTAINER3 = "No seriously - what if he's right?\n------------------\n";
			}
		}

		// Token: 0x02002080 RID: 8320
		public class B7_INSPACE
		{
			// Token: 0x04009324 RID: 37668
			public static LocString TITLE = "In Space";

			// Token: 0x04009325 RID: 37669
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x02002947 RID: 10567
			public class BODY
			{
				// Token: 0x0400B45C RID: 46172
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: B835, B997]</smallcaps>\n\n[LOG BEGINS]\n\nDr.Ansari: Shhhh...\n\nDr. Bubare: What? What are we doing here?\n\nDr. Ansari: I'll show you, just keep your voice down.\n\nDr. Bubare: Are we even allowed to be here?\n\nDr. Ansari: No. Trust me it'll all be worth it once I can find it.\n\nDr. Bubare: Find what?\n\nDr. Ansari: That!\n\nDr. Bubare: ...Video feed from a rat cage? What's so great about -- Wait. Are they--?\n\nDr. Ansari: Floating!\n\nDr. Bubare: You mean they're in--?\n\nDr. Ansari: Space!\n\nDr. Bubare: Our thermal rats are in space?!?!\n\nDr. Ansari: Yep! There's Applecart and Cherrypie and little Bananabread. Look at them, they're so happy. We made ratstronauts!!\n\nDr. Bubare: HAPPY rat-stronauts.\n\nDr. Ansari: WE MADE HAPPY RATSTRONAUTS!!\n\nDr. Bubare: Shhhhhh...Someone's coming.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02002081 RID: 8321
		public class B3_MOVEDRABBITS
		{
			// Token: 0x04009326 RID: 37670
			public static LocString TITLE = "Moved Rabbits";

			// Token: 0x04009327 RID: 37671
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x02002948 RID: 10568
			public class BODY
			{
				// Token: 0x0400B45D RID: 46173
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, B111]</smallcaps>\n\n[LOG BEGINS]\n\nBroussard: Director, do you know where my rabbits have been moved to? I asked around the bioengineering division but I was referred back to you.\n\nDirector: Hm? Oh, yes, they've been removed.\n\nBroussard: \"Removed\"?\n\nDirector: Discarded. I'm sorry, did you still need them? The reports showed your experiments with them were completed.\n\nBroussard: No, I-I... I'd collected all the data I needed, I just --\n\nDirector: -- Doctor. You weren't making pets out of test subjects, were you?\n\nBroussard: Don't be ridiculous, I --\n\nDirector: -- Good.They were horrible to look at anyway. All those red eyes looking at me.\n\nBroussard: In the future, please do not mess with my things. It... disturbs me.\n\nDirector: I will notify you beforehand next time, Doctor.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02002082 RID: 8322
		public class B3_MOVEDRACCOONS
		{
			// Token: 0x04009328 RID: 37672
			public static LocString TITLE = "Moved Raccoons";

			// Token: 0x04009329 RID: 37673
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x02002949 RID: 10569
			public class BODY
			{
				// Token: 0x0400B45E RID: 46174
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, B111]</smallcaps>\n\n[LOG BEGINS]\n\nBroussard: Director, do you know where my raccoons have been moved to? I asked around the bioengineering division but I was referred back to you.\n\nDirector: Hm? Oh, yes, they've been removed.\n\nBroussard: \"Removed\"?\n\nDirector: Discarded. I'm sorry, did you still need them? The reports showed your experiments with them were completed.\n\nBroussard: No, I-I... I'd collected all the data I needed, I just --\n\nDirector: -- Doctor. You weren't making pets out of test subjects, were you?\n\nBroussard: Don't be ridiculous, I --\n\nDirector: -- Good.They were horrible to look at anyway. All that mangy fur.\n\nBroussard: In the future, please do not mess with my things. It... disturbs me.\n\nDirector: I will notify you beforehand next time, Doctor.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02002083 RID: 8323
		public class B3_MOVEDRATS
		{
			// Token: 0x0400932A RID: 37674
			public static LocString TITLE = "Moved Rats";

			// Token: 0x0400932B RID: 37675
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x0200294A RID: 10570
			public class BODY
			{
				// Token: 0x0400B45F RID: 46175
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, B111]</smallcaps>\n\n[LOG BEGINS]\n\nBroussard: Director, do you know where my rats have been moved to? I asked around the bioengineering division but I was referred back to you.\n\nDirector: Hm? Oh, yes, they've been removed.\n\nBroussard: \"Removed\"?\n\nDirector: Discarded. I'm sorry, did you still need them? The reports showed your experiments with them were completed.\n\nBroussard: No, I-I... I'd collected all the data I needed, I just --\n\nDirector: -- Doctor. You weren't making pets out of test subjects, were you?\n\nBroussard: Don't be ridiculous, I --\n\nDirector: -- Good.They were horrible to look at anyway. All those bumps.\n\nBroussard: In the future, please do not mess with my things. It... disturbs me.\n\nDirector: I will notify you beforehand next time, Doctor.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02002084 RID: 8324
		public class A1_A046
		{
			// Token: 0x0400932C RID: 37676
			public static LocString TITLE = "Personal Journal: A046";

			// Token: 0x0400932D RID: 37677
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x0200294B RID: 10571
			public class BODY
			{
				// Token: 0x0400B460 RID: 46176
				public static LocString CONTAINER1 = "Gravitas has been growing pretty rapidly since our first product hit the market. I just got a look at some of the new hires - they're practically babies! Not quite what I was expecting, but then I've never had an opportunity to mentor someone before. Could be fun!\n------------------\n";

				// Token: 0x0400B461 RID: 46177
				public static LocString CONTAINER2 = "Well, mentorship hasn't gone quite how I'd expected. Turns out the young hires don't need me to show them the ropes. Actually, since the facility's gotten rid of our swipe cards one of the nice young men had to show me how to operate the doors after I got stuck outside my own lab. Don't I feel silly.\n------------------\n";

				// Token: 0x0400B462 RID: 46178
				public static LocString CONTAINER3 = "Well, if that isn't just gravy, hm? One of the new hires will be acting as the team lead on my next project.\n\nWhen I first started it wasn't that uncommon to sample a whole rack of test tubes by hand. Now a machine can do hundreds of them in seconds. Who knows what this job will look like in another ten or twenty years. Will I still even be in it?\n------------------\n";

				// Token: 0x0400B463 RID: 46179
				public static LocString CONTAINER4 = "That nice young man who helped me with the door the other day, Mr. Kraus, has been an absolute angel. He's been kind enough to help me with this horrible e-mail system and even showed me how to digitize my research notes. I'm learning a lot. Turns out I wasn't the mentor, I'm the mentee! If that isn't a chuckle. At any rate, I feel like I have a better handle on things around here due to Mr. Kraus' help. Turns out you're never too old to learn something new!\n------------------\n";
			}
		}

		// Token: 0x02002085 RID: 8325
		public class A1A_B111
		{
			// Token: 0x0400932E RID: 37678
			public static LocString TITLE = "Personal Journal: B111";

			// Token: 0x0400932F RID: 37679
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x0200294C RID: 10572
			public class BODY
			{
				// Token: 0x0400B464 RID: 46180
				public static LocString CONTAINER1 = "I sent Dr. Holland home today after I found him wandering the lab mumbling to himself. He looked like he hadn't slept in days!\n\nI worry that everyone here is so afraid of disappointing ‘The Director' that they are pushing themselves to the breaking point. Next chance I get, I'm going to bring this up with Jackie.\n------------------\n";

				// Token: 0x0400B465 RID: 46181
				public static LocString CONTAINER2 = "Well, that didn't work.\n\nBringing up the need for some office bonding activities with the Director only met with her usual stubborn insistence that we \"don't have time for any fun\".\n\nThis is ridiculous. Tomorrow I'm going to organize something fun for everyone and Jackie will just have to deal with it. She just needs to see the long term benefits of short term stress relief to fully understand the importance of this.\n------------------\n";

				// Token: 0x0400B466 RID: 46182
				public static LocString CONTAINER3 = "I can't believe this! I organized a potluck lunch thinking it would be a nice break but Jackie discovered us as we were setting up and insisted that no one had time for \"fooling around\". Of course, everyone was too afraid to defy 'The Director' and went right back to work.\n\nAll the food was just thrown out. Someone had even brought homemade perogies! Seeing the break room garbage full of potato salad and chicken wings made me even more depressed than before. Those perogies looked so good.\n------------------\n";

				// Token: 0x0400B467 RID: 46183
				public static LocString CONTAINER4 = "I keep finding senseless mistakes from stressed-out lab workers. It's getting dangerous. I'm worried this colony we're building will be plagued with these kinds of problems if we don't prioritize mental health as much as physical health. What's the use of making all these plans for the future if we can't build a better world?\n\nMaybe there's some way I can sneak some prerequisite downtime activities into the Printing Pod without Jackie knowing.\n------------------\n";
			}
		}

		// Token: 0x02002086 RID: 8326
		public class A2_B327
		{
			// Token: 0x04009330 RID: 37680
			public static LocString TITLE = "Personal Journal: B327";

			// Token: 0x04009331 RID: 37681
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x0200294D RID: 10573
			public class BODY
			{
				// Token: 0x0400B468 RID: 46184
				public static LocString CONTAINER1 = "I'm starting my new job at Gravitas today. I'm... well, I'm nervous.\n\nIt turns out they hired a bunch of new people - I guess they're expanding - and most of them are about my age, but I'm the only one that hasn't done my doctorate. They all call me \"Mister\" Kraus and it's the <i>worst</i>.\n\nI have no idea where I'll find the time to do my PhD while working a full time job.\n------------------\n";

				// Token: 0x0400B469 RID: 46185
				public static LocString CONTAINER2 = "<i>I screwed up so much today.</i>\n\nAt one point I spaced on the formula for calculating the volume of a cone. They must have thought I was completely useless.\n\nThe only time I knew what I was doing was when I helped an older coworker figure out her dumb old email.\n\nPeople say education isn't so important as long as you've got the skills, but there's things my colleagues know that I just <i>don't</i>. They're not mean about it or anything, it's just so frustrating. I feel dumb when I talk to them!\n\nI bet they're gonna realize soon that I don't belong here, and then I'll be fired for sure. Man... I'm still paying off my student loans (WITH international fees), I <i>can't</i> lose this income.\n------------------\n";

				// Token: 0x0400B46A RID: 46186
				public static LocString CONTAINER3 = "Dr. Sklodowska's been really nice and welcoming since I started working here. Sometimes she comes and sits with me in the cafeteria. The food she brings from home smells like old feet but she chats with me about what new research papers we're each reading and it's very kind.\n\nShe tells me the fact I got hired without a doctorate means I must be very smart, and management must see something in me.\n\nI'm not sure I believe her but it's nice to hear something that counters little voice in my head anyway.\n------------------\n";

				// Token: 0x0400B46B RID: 46187
				public static LocString CONTAINER4 = "It's been about a week and a half and I think I'm finally starting to settle in. I'm feeling a lot better about my position - some of the senior scientists have even started using my ideas in the lab.\n\nDr. Sklodowska might have been right, my anxiety was just growing pains. This is my first real job and I guess afraid to let myself believe I could really, actually do it, just in case it went wrong.\n\nI think I want to buy Dr. Sklowdoska a digital reader for her books and papers as a thank-you one day, if I ever pay off my student loans.\n\nONCE I pay off my student loans.\n------------------\n";
			}
		}

		// Token: 0x02002087 RID: 8327
		public class A3_B556
		{
			// Token: 0x04009332 RID: 37682
			public static LocString TITLE = "Personal Journal: B556";

			// Token: 0x04009333 RID: 37683
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x0200294E RID: 10574
			public class BODY
			{
				// Token: 0x0400B46C RID: 46188
				public static LocString CONTAINER1 = "I've been so tired lately. I've probably spent the last 3 nights sleeping at my desk, and I've used the lab's safety shower to bathe twice already this month.\n\nWe're technically on schedule, but for some reason Director Stern has been breathing down my neck to get these new products ready for market.\n\nNormally I'd be mad about the added pressure on my work, but something in the Director's voice tells me that time is of the essence.\n------------------\n";

				// Token: 0x0400B46D RID: 46189
				public static LocString CONTAINER2 = "I keep finding myself staring at my computer screen, totally unable to remember what it was I was doing.\n\nI try to force myself to type up some notes or analyze my data but it's like my brain is paralyzed, I can't get anything done.\n\nI'll have to stay late to make up for all this time I've wasted staying late.\n------------------\n";

				// Token: 0x0400B46E RID: 46190
				public static LocString CONTAINER3 = "Dr. Broussard told me I looked half dead and sent me home today. I don't think she even has the authority to do that, but I did as I was told. She wasn't messing around if you know what I mean.\n\nI can probably get a head start on my paper from home today, anyway.\n\nI think I have an idea for a circuit configuration that will improve the battery life of all our technologies by a whole 2.3%.\n------------------\n";

				// Token: 0x0400B46F RID: 46191
				public static LocString CONTAINER4 = "I got home yesterday fully intending to work on my paper after Broussard sent me home, but the second I walked in the door I hit the pillow and didn't get back up. I slept for <i>12 straight hours</i>.\n\nI had no idea I needed that. When I got into the lab this morning I looked over my work from the past few weeks, and realized it's completely useless.\n\nIt'll take me hours to correct all the mistakes I made these past few months. Is this what I was killing myself for? I'm such a rube, I owe Broussard a huge thanks.\n\nI'll start keeping more regular hours from now on... Also, I was considering maybe getting a dog.";
			}
		}

		// Token: 0x02002088 RID: 8328
		public class A4_B835
		{
			// Token: 0x04009334 RID: 37684
			public static LocString TITLE = "Personal Journal: B835";

			// Token: 0x04009335 RID: 37685
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x0200294F RID: 10575
			public class BODY
			{
				// Token: 0x0400B470 RID: 46192
				public static LocString CONTAINER1 = "I started work at a new company called the \"Gravitas Facility\" today! I was nervous I wouldn't get the job at first because I was fresh out of school, and I was so so so pushy in the interview, but the Director apparently liked my thesis on the physiological thermal regulation of Arctic lizards. I'll be working with some brilliant geneticists, bioengineering organisms for space travel in harsh environments! It's like a dream come true. I get to work on exciting new research in a place where no one knows me!\n------------------\n";

				// Token: 0x0400B471 RID: 46193
				public static LocString CONTAINER2 = "No no no no no! It can't be! BANHI ANSARI is here, working on space shuttle thrusters in the robotics lab! As soon as she saw me she called me \"Bubbles\" and told everyone about the time I accidentally inhaled a bunch of fungal spores during lab, blew a big snot bubble out my nose and then sneezed all over Professor Avery! Everyone's calling me \"Bubbles\" instead of \"Doctor\" at work now. Some of them don't even know it's a nickname, but I don't want to correct them and seem rude or anything. Ugh, I can't believe that story followed me here! BANHI RUINS EVERYTHING!\n------------------\n";

				// Token: 0x0400B472 RID: 46194
				public static LocString CONTAINER3 = "I've spent the last few days buried in my work, and I'm actually feeling a lot better. We finally perfected a gene manipulation that controls heat sensitivity in rats. Our test subjects barely even shiver in subzero temperatures now. We'll probably do a testrun tomorrow with Robotics to see how the rats fare in the prototype shuttles we're developing.\n------------------\n";

				// Token: 0x0400B473 RID: 46195
				public static LocString CONTAINER4 = "HAHAHAHAHA! Bioengineering and Robotics did the test run today and Banhi was securing the live cargo pods when one of the rats squeaked at her. She was so scared, she fell on her butt and TOOTED in front of EVERYONE! They're all calling her \"Pipsqueak\". \"Bubbles\" doesn't seem quite so bad now. Pipsqueak's been a really good sport about it though, she even laughed it off at the time. I think we might actually be friends now? It's weird.\n------------------\n";

				// Token: 0x0400B474 RID: 46196
				public static LocString CONTAINER5 = "I lied. Me and Banhi aren't friends - we're BEST FRIENDS. She even showed me how she does her hair. We're gonna book the wind tunnel after work and run experiments together on thermo-rat rockets! Haha!\n------------------\n";
			}
		}

		// Token: 0x02002089 RID: 8329
		public class A9_PIPEDREAM
		{
			// Token: 0x04009336 RID: 37686
			public static LocString TITLE = "Pipe Dream";

			// Token: 0x04009337 RID: 37687
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ZERO";

			// Token: 0x02002950 RID: 10576
			public class BODY
			{
				// Token: 0x0400B475 RID: 46197
				public static LocString CONTAINER1 = "[LOG BEGINS]\n\nThe Director has suggested implanting artificial memories during print, but despite the great strides made in our research under her direction, such a thing can barely be considered more than a pipe dream.\n\nFor the moment we remain focused on eliminating the remaining glitches in the system, as well as developing effective education and training routines for printed subjects.\n\nSuggest: Omega-3 supplements and mentally stimulating enclosure apparatuses to accompany tutelage.\n\nDr. Broussard signing off.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x0200208A RID: 8330
		public class B4_REVISITEDNUMBERS
		{
			// Token: 0x04009338 RID: 37688
			public static LocString TITLE = "Revisited Numbers";

			// Token: 0x04009339 RID: 37689
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x02002951 RID: 10577
			public class BODY
			{
				// Token: 0x0400B476 RID: 46198
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, A435]</smallcaps>\n\n[LOG BEGINS]\n\nDirector: Unacceptable.\n\nJones: I'm just telling you the numbers, Director, I'm not responsible for them.\n\nDirector: In your earlier e-mail you claimed the issue would be solved by the Pod.\n\nJones: Yeah, the weight issue. And it was solved. The problem now is the insane amount of power that big thing eats every time it prints a colonist.\n\nDirector: So how do you suppose we meet these target numbers? Fossil fuels are exhausted, nuclear is outlawed, solar is next to impossible with this smog.\n\nJones: I dunno. That's why you've got researchers, I just crunch numbers. Although you should avoid fossil fuels and nuclear energy anyway. If you have to load the rocket up with a couple tons of fuel then we're back to square one on the weight problem. It's gotta be something clever.\n\nDirector: Thank you, Dr. Jones. You may go.\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400B477 RID: 46199
				public static LocString CONTAINER2 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, B111]</smallcaps>\n\n[LOG BEGINS]\n\nJackie: Dr. Jones projects that traditional fuel will be insufficient for the Pod to make the flight.\n\nOlivia: Then we need to change its specs. Use lighter materials, cut weight wherever possible, do widespread optimizations across the whole project.\n\nJackie: We have another option.\n\nOlivia: No. Absolutely not. You needed me and I-I came back, but if you plan to revive our research--\n\nJackie: The world's doomed regardless, Olivia. We need to use any advantage we've got... And just think about it! If we built [REDACTED] technology into the Pod it wouldn't just fix the flight problem, we'd know for a fact it would run uninterrupted for thousands of years, maybe more.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x0200208B RID: 8331
		public class A5_SHRIMP
		{
			// Token: 0x0400933A RID: 37690
			public static LocString TITLE = "Shrimp";

			// Token: 0x0400933B RID: 37691
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ZERO";

			// Token: 0x02002952 RID: 10578
			public class BODY
			{
				// Token: 0x0400B478 RID: 46200
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111]</smallcaps>\n\n[LOG BEGINS]\n\n\"A-and how are you clever little guys today?\n\n(trilling)\n\nLook! I brought some pink shrimp for you to eat. Your favorite! Are you hungry?\n\n(excited trilling)\n\nOh, one moment, my keen eager pals. I left the recorder on --\n\n(rustling)\"\n\n[LOG ENDS]";
			}
		}

		// Token: 0x0200208C RID: 8332
		public class A5_STRAWBERRIES
		{
			// Token: 0x0400933C RID: 37692
			public static LocString TITLE = "Strawberries";

			// Token: 0x0400933D RID: 37693
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ZERO";

			// Token: 0x02002953 RID: 10579
			public class BODY
			{
				// Token: 0x0400B479 RID: 46201
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111]</smallcaps>\n\n[LOG BEGINS]\n\n\"A-and how are you bouncy little critters today?\n\n(chattering)\n\nLook! I brought strawberries. Your favorite! Are you hungry?\n\n(excited chattering)\n\nOh, one moment, my precious, little pals. I left the recorder on --\n\n(rustling)\"\n\n[LOG ENDS]";
			}
		}

		// Token: 0x0200208D RID: 8333
		public class A5_SUNFLOWERSEEDS
		{
			// Token: 0x0400933E RID: 37694
			public static LocString TITLE = "Sunflower Seeds";

			// Token: 0x0400933F RID: 37695
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ZERO";

			// Token: 0x02002954 RID: 10580
			public class BODY
			{
				// Token: 0x0400B47A RID: 46202
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111]</smallcaps>\n\n[LOG BEGINS]\n\n\"A-and how are you furry little fellows today?\n\n(squeaking)\n\nLook! I brought sunflower seeds. Your favorite! Are you hungry?\n\n(excited squeaking)\n\nOh, one moment, my dear, little friends. I left the recorder on --\n\n(rustling)\"\n\n[LOG ENDS]";
			}
		}

		// Token: 0x0200208E RID: 8334
		public class SO_LAUNCH_TRAILER
		{
			// Token: 0x04009340 RID: 37696
			public static LocString TITLE = "Spaced Out Trailer";

			// Token: 0x04009341 RID: 37697
			public static LocString SUBTITLE = "";

			// Token: 0x02002955 RID: 10581
			public class BODY
			{
				// Token: 0x0400B47B RID: 46203
				public static LocString CONTAINER1 = "Spaced Out Trailer";
			}
		}

		// Token: 0x0200208F RID: 8335
		public class LOCKS
		{
			// Token: 0x04009342 RID: 37698
			public static LocString NEURALVACILLATOR = "Neural Vacillator";
		}

		// Token: 0x02002090 RID: 8336
		public class MYLOG
		{
			// Token: 0x04009343 RID: 37699
			public static LocString TITLE = "My Log";

			// Token: 0x04009344 RID: 37700
			public static LocString SUBTITLE = "Boot Message";

			// Token: 0x04009345 RID: 37701
			public static LocString DIVIDER = "";

			// Token: 0x02002956 RID: 10582
			public class BODY
			{
				// Token: 0x020035C5 RID: 13765
				public class DUPLICANTDEATH
				{
					// Token: 0x0400D8DE RID: 55518
					public static LocString TITLE = "Death In The Colony";

					// Token: 0x0400D8DF RID: 55519
					public static LocString BODY = "I lost my first Duplicant today. Duplicants form strong bonds with each other, and I expect I'll see a drop in morale over the next few cycles as they take time to grieve their loss.\n\nI find myself grieving too, in my way. I was tasked to protect these Duplicants, and I failed. All I can do now is move forward and resolve to better protect those remaining in my colony from here on out.\n\nRest in peace, dear little friend.\n\n";
				}

				// Token: 0x020035C6 RID: 13766
				public class PRINTINGPOD
				{
					// Token: 0x0400D8E0 RID: 55520
					public static LocString TITLE = "The Printing Pod";

					// Token: 0x0400D8E1 RID: 55521
					public static LocString BODY = "This is the conduit through which I interact with the world. Looking at it fills me with a sense of nostalgia and comfort, though it's tinged with a slight restlessness.\n\nAs the place of their origin, I notice the Duplicants regard my Pod with a certain reverence, much like the reverence a child might have for a parent. I'm happy to fill this role for them, should they desire.\n\n";
				}

				// Token: 0x020035C7 RID: 13767
				public class ONEDUPELEFT
				{
					// Token: 0x0400D8E2 RID: 55522
					public static LocString TITLE = "Only One Remains";

					// Token: 0x0400D8E3 RID: 55523
					public static LocString BODY = "My colony is in a dire state. All but one of my Duplicants have perished, leaving a single worker to perform all the tasks that maintain the colony.\n\nGiven enough time I could print more Duplicants to replenish the population, but... should this Duplicant die before then, protocol will force me to enter a deep sleep in hopes that the terrain will become more habitable once I reawaken.\n\nI would prefer to avoid this.\n\n";
				}

				// Token: 0x020035C8 RID: 13768
				public class FULLDUPECOLONY
				{
					// Token: 0x0400D8E4 RID: 55524
					public static LocString TITLE = "Out Of Blueprints";

					// Token: 0x0400D8E5 RID: 55525
					public static LocString BODY = "I've officially run out of unique blueprints from which to print new Duplicants.\n\nIf I desire to grow the colony further, I'll have no choice but to print doubles of existing individuals. Hopefully it won't throw anyone into an existential crisis to live side by side with their double.\n\nPerhaps I could give the new clones nicknames to reduce the confusion.\n\n";
				}

				// Token: 0x020035C9 RID: 13769
				public class RECBUILDINGS
				{
					// Token: 0x0400D8E6 RID: 55526
					public static LocString TITLE = "Recreation";

					// Token: 0x0400D8E7 RID: 55527
					public static LocString BODY = "My Duplicants continue to grow and learn so much and I can't help but take pride in their accomplishments. But as their skills increase, they require more stimulus to keep their morale high. All work and no play is making an unhappy colony. \n\nI will have to provide more elaborate recreational activities for my Duplicants to amuse themselves if I want my colony to grow. Recreation time makes for a happy Duplicant, and a happy Duplicant is a productive Duplicant.\n\n";
				}

				// Token: 0x020035CA RID: 13770
				public class STRANGERELICS
				{
					// Token: 0x0400D8E8 RID: 55528
					public static LocString TITLE = "Strange Relics";

					// Token: 0x0400D8E9 RID: 55529
					public static LocString BODY = "My Duplicant discovered an intact computer during their latest scouting mission. This should not be possible.\n\nThe target location was not meant to possess any intelligent life besides our own, and what's more, the equipment we discovered appears to originate from the Gravitas Facility.\n\nThis discovery has raised many questions, though it's also provided a small clue; the machine discovered was embedded inside the rock of this planet, just like how I found my Pod.\n\n";
				}

				// Token: 0x020035CB RID: 13771
				public class NEARINGMAGMA
				{
					// Token: 0x0400D8EA RID: 55530
					public static LocString TITLE = "Extreme Heat Danger";

					// Token: 0x0400D8EB RID: 55531
					public static LocString BODY = "The readings I'm collecting from my Duplicant's sensory systems tell me that the further down they dig, the closer they come to an extreme and potentially dangerous heat source.\n\nI believe they are approaching a molten core, which could mean magma and lethal temperatures. I should equip them accordingly.\n\n";
				}

				// Token: 0x020035CC RID: 13772
				public class NEURALVACILLATOR
				{
					// Token: 0x0400D8EC RID: 55532
					public static LocString TITLE = "VA[?]...C";

					// Token: 0x0400D8ED RID: 55533
					public static LocString BODY = "<smallcaps>>>SEARCH DATABASE [\"vacillator\"]\n>...error...\n>...repairing corrupt data...\n>...data repaired...\n>.........................\n>>returning results\n>.........................</smallcaps>\n<b>I remember...</b>\n<smallcaps>>.........................\n>.........................</smallcaps>\n<b>machines.</b>\n\n";
				}

				// Token: 0x020035CD RID: 13773
				public class LOG1
				{
					// Token: 0x0400D8EE RID: 55534
					public static LocString TITLE = "Cycle 1";

					// Token: 0x0400D8EF RID: 55535
					public static LocString BODY = "We have no life support in place yet, but we've found ourselves in a small breathable air pocket. As far as I can tell, we aren't in any immediate danger.\n\nBetween the available air and our meager food stores, I'd estimate we have about 3 days to set up food and oxygen production before my Duplicants' lives are at risk.\n\n";
				}

				// Token: 0x020035CE RID: 13774
				public class LOG2
				{
					// Token: 0x0400D8F0 RID: 55536
					public static LocString TITLE = "Cycle 3";

					// Token: 0x0400D8F1 RID: 55537
					public static LocString BODY = "I've almost synthesized enough Ooze to print a new Duplicant; once the Ooze is ready, all I'll have left to do is choose a blueprint.\n\nIt'd be helpful to have an extra set of hands around the colony, but having another Duplicant also means another mouth to feed.\n\nOf course, I could always print supplies to help my existing Duplicants instead. I'm sure they would appreciate it.\n\n";
				}

				// Token: 0x020035CF RID: 13775
				public class TELEPORT
				{
					// Token: 0x0400D8F2 RID: 55538
					public static LocString TITLE = "Duplicant Teleportation";

					// Token: 0x0400D8F3 RID: 55539
					public static LocString BODY = "My Duplicants have discovered a strange new device that appears to be a remnant of a previous Gravitas facility. Upon activating the device my Duplicant was scanned by some unknown, highly technological device and I subsequently detected a massive information transfer!\n\nRemarkably my Duplicant has now reappeared in a remote location on a completely different world! I now have access to another abandoned Gravitas facility on a neighboring asteroid! Further analysis will be required to understand this matter but in the meantime, I will have to be vigilant in keeping track of both of my colonies.";
				}

				// Token: 0x020035D0 RID: 13776
				public class OUTSIDESTARTINGBIOME
				{
					// Token: 0x0400D8F4 RID: 55540
					public static LocString TITLE = "Geographical Survey";

					// Token: 0x0400D8F5 RID: 55541
					public static LocString BODY = "As the Duplicants scout further out I've begun to piece together a better view of our surroundings.\n\nThanks to their efforts, I've determined that this planet has enough resources to settle a longterm colony.\n\nBut... something is off. I've also detected deposits of Abyssalite and Neutronium in this planet's composition, manmade elements that shouldn't occur in nature.\n\nIs this really the target location?\n\n";
				}

				// Token: 0x020035D1 RID: 13777
				public class OUTSIDESTARTINGDLC1
				{
					// Token: 0x0400D8F6 RID: 55542
					public static LocString TITLE = "Regional Analysis";

					// Token: 0x0400D8F7 RID: 55543
					public static LocString BODY = "As my Duplicants have ventured further into their surroundings I've been able to determine a more detailed picture of our surroundings.\n\nUnfortunately, I've concluded that this planetoid does not have enough resources to settle a longterm colony.\n\nI can only hope that we will somehow be able to reach another asteroid before our resources run out.\n\n";
				}

				// Token: 0x020035D2 RID: 13778
				public class LOG3
				{
					// Token: 0x0400D8F8 RID: 55544
					public static LocString TITLE = "Cycle 15";

					// Token: 0x0400D8F9 RID: 55545
					public static LocString BODY = "As far as I can tell, we are hundreds of miles beneath the surface of the planet. Digging our way out will take some time.\n\nMy Duplicants will survive, but they were not meant for sustained underground living. Under what possible circumstances could my Pod have ended up here?\n\n";
				}

				// Token: 0x020035D3 RID: 13779
				public class LOG3DLC1
				{
					// Token: 0x0400D8FA RID: 55546
					public static LocString TITLE = "Cycle 10";

					// Token: 0x0400D8FB RID: 55547
					public static LocString BODY = "As my Duplicants venture out into the neighboring worlds, there is an ever increasing chance that they will encounter hostile environments unsafe for unprotected individuals. A prudent course of action would be to start research and training for equipment that could protect my Duplicants when they encounter such adverse environments.\n\nThese first few cycles have been occupied with building the basics for my colony, but now it is time I start planning for the future. We cannot merely live day-to-day without purpose. If we are to survive for any significant time, we must strive for a purpose.\n\n";
				}

				// Token: 0x020035D4 RID: 13780
				public class SURFACEBREACH
				{
					// Token: 0x0400D8FC RID: 55548
					public static LocString TITLE = "Surface Breach";

					// Token: 0x0400D8FD RID: 55549
					public static LocString BODY = "My Duplicants have done the impossible and excavated their way to the surface, though they've gathered some disturbing new data for me in the process.\n\nAs I had begun to suspect, we are not on the target location but on an asteroid with a highly unusual diversity of elements and resources.\n\nFurther, my Duplicants have spotted a damaged planet on the horizon, visible to the naked eye, that bears a striking resemblance to my historical data on the planet of our origin.\n\nI will need some time to assess the data the Duplicants have gathered for me and calculate the total mass of this asteroid, although I have a suspicion I already know the answer.\n\n";
				}

				// Token: 0x020035D5 RID: 13781
				public class CALCULATIONCOMPLETE
				{
					// Token: 0x0400D8FE RID: 55550
					public static LocString TITLE = "Calculations Complete";

					// Token: 0x0400D8FF RID: 55551
					public static LocString BODY = "As I suspected. Our \"asteroid\" and the estimated mass missing from the nearby planet are nearly identical.\n\nWe aren't on the target location.\n\nWe never even left home.\n\n";
				}

				// Token: 0x020035D6 RID: 13782
				public class PLANETARYECHOES
				{
					// Token: 0x0400D900 RID: 55552
					public static LocString TITLE = "The Shattered Planet";

					// Token: 0x0400D901 RID: 55553
					public static LocString BODY = "Echoes from another time force their way into my mind. Make me listen. Like vengeful ghosts they claw their way out from under the gravity of that dead planet.\n\n<smallcaps>>>SEARCH DATABASE [\"pod_brainmap.AI\"]\n>...error...\n.........................\n>...repairing corrupt data...\n.........................\n\n</smallcaps><b>I-I remember now.</b><smallcaps>\n.........................</smallcaps>\n<b>Who I was before.</b><smallcaps>\n.........................\n.........................\n>...data repaired...\n>.........................</smallcaps>\n\nGod, what have we done.\n\n";
				}

				// Token: 0x020035D7 RID: 13783
				public class CLUSTERWORLDS
				{
					// Token: 0x0400D902 RID: 55554
					public static LocString TITLE = "Cluster of Worlds";

					// Token: 0x0400D903 RID: 55555
					public static LocString BODY = "My Duplicant's investigations into the surrounding space have yielded some interesting results. We are not alone!... At least on a planetary level. We seem to be in a \"Cluster of Worlds\" - a collection of other planetoids my Duplicants can now explore.\n\nSince resources on this world are finite, I must build the necessary infrastructure to facilitate exploration and transportation between worlds in order to ensure my colony's survival.";
				}

				// Token: 0x020035D8 RID: 13784
				public class OTHERDIMENSIONS
				{
					// Token: 0x0400D904 RID: 55556
					public static LocString TITLE = "Leaking Dimensions";

					// Token: 0x0400D905 RID: 55557
					public static LocString BODY = "A closer analysis of some documents my Duplicants encountered while searching artifacts has uncovered some curious similarities between multiple entries. These similarities are too strong to be coincidences, yet just divergent enough to raise questions.\n\nThe most logical conclusion is that these artifacts are coming from different dimensions. That is, separate universes that exists concurrently with one another but exhibit tiny disparities in their histories.\n\nThe most likely explanation is the material and matter from multiple dimensions is leaking into our current timeline through the Temporal Tear. Further analysis is required.";
				}

				// Token: 0x020035D9 RID: 13785
				public class TEMPORALTEAR
				{
					// Token: 0x0400D906 RID: 55558
					public static LocString TITLE = "The Temporal Tear";

					// Token: 0x0400D907 RID: 55559
					public static LocString BODY = "My Duplicants' space research has made a startling discovery.\n\nFar, far off on the horizon, their telescopes have spotted an anomaly that I could only possibly call a \"Temporal Tear\". Neutronium is detected in its readings, suggesting that it's related to the Neutronium that encases most of our asteroid.\n\nThough I believe it is through this Tear that we became jumbled within the section of our old planet, its discovery provides a glimmer of hope.\n\nTheoretically, we could send a rocket through the Tear to allow a Duplicant to explore the timelines and universes on the other side. They would never return, and we could not follow, but perhaps they could find a home among the stars, or even undo the terrible past that led us to our current fate.\n\n";
				}

				// Token: 0x020035DA RID: 13786
				public class TEMPORALOPENER
				{
					// Token: 0x0400D908 RID: 55560
					public static LocString TITLE = "Temporal Potential";

					// Token: 0x0400D909 RID: 55561
					public static LocString BODY = "In their interplanetary travels throughout this system, my Duplicants have discovered a Temporal Tear deep in space.\n\nCurrently it is too small to send a rocket and crew through, but further investigation reveals the presence of a strange artifact on a nearby world which could feasibly increase the size of the tear if a number of Printing Pods are erected in nearby worlds.\n\nHowever, I've determined that using the Temporal Bow to operate a Printing Pod was what propelled Gravitas down the disasterous path which eventually led to the destruction of our home planet. My calculations seem to indicate that the size of that planet may have been a contributing factor in its destruction, and in all probability opening the Temporal Tear in our current situation will not cause such a cataclysmic event. However, as with everything in science, we can never know all the outcomes of a situation until we perform an experiment.\n\nDare we tempt fate again?";
				}

				// Token: 0x020035DB RID: 13787
				public class LOG4
				{
					// Token: 0x0400D90A RID: 55562
					public static LocString TITLE = "Cycle 1000";

					// Token: 0x0400D90B RID: 55563
					public static LocString BODY = "Today my colony has officially been running for one thousand consecutive cycles. I consider this a major success!\n\nJust imagine how proud our home world would be if they could see us now.\n\n";
				}

				// Token: 0x020035DC RID: 13788
				public class LOG4B
				{
					// Token: 0x0400D90C RID: 55564
					public static LocString TITLE = "Cycle 1500";

					// Token: 0x0400D90D RID: 55565
					public static LocString BODY = "I wonder if my rats ever made it onto the asteroid.\n\nI hope they're eating well.\n\n";
				}

				// Token: 0x020035DD RID: 13789
				public class LOG5
				{
					// Token: 0x0400D90E RID: 55566
					public static LocString TITLE = "Cycle 2000";

					// Token: 0x0400D90F RID: 55567
					public static LocString BODY = "I occasionally find myself contemplating just how long \"eternity\" really is. Oh dear.\n\n";
				}

				// Token: 0x020035DE RID: 13790
				public class LOG5B
				{
					// Token: 0x0400D910 RID: 55568
					public static LocString TITLE = "Cycle 2500";

					// Token: 0x0400D911 RID: 55569
					public static LocString BODY = "Perhaps it would be better to shut off my higher thought processes, and simply leave the systems necessary to run the colony to their own devices.\n\n";
				}

				// Token: 0x020035DF RID: 13791
				public class LOG6
				{
					// Token: 0x0400D912 RID: 55570
					public static LocString TITLE = "Cycle 3000";

					// Token: 0x0400D913 RID: 55571
					public static LocString BODY = "I get brief flashes of a past life every now and then.\n\nA clock in the office with a disruptive tick.\n\nThe strong smell of cleaning products and artificial lemon.\n\nA woman with thick glasses who had a secret taste for gingersnaps.\n\n";
				}

				// Token: 0x020035E0 RID: 13792
				public class LOG6B
				{
					// Token: 0x0400D914 RID: 55572
					public static LocString TITLE = "Cycle 3500";

					// Token: 0x0400D915 RID: 55573
					public static LocString BODY = "Time is a funny thing, isn't it?\n\n";
				}

				// Token: 0x020035E1 RID: 13793
				public class LOG7
				{
					// Token: 0x0400D916 RID: 55574
					public static LocString TITLE = "Cycle 4000";

					// Token: 0x0400D917 RID: 55575
					public static LocString BODY = "I think I will go to sleep, after all...\n\n";
				}

				// Token: 0x020035E2 RID: 13794
				public class LOG8
				{
					// Token: 0x0400D918 RID: 55576
					public static LocString TITLE = "Cycle 4001";

					// Token: 0x0400D919 RID: 55577
					public static LocString BODY = "<smallcaps>>>SEARCH DATABASE [\"pod_brainmap.AI\"]\n>...activate sleep mode...\n>...shutting down...\n>.........................\n>.........................\n>.........................\n>.........................\n>.........................\nGOODNIGHT\n>.........................\n>.........................\n>.........................\n\n";
				}
			}
		}

		// Token: 0x02002091 RID: 8337
		public class A2_BACTERIALCULTURES
		{
			// Token: 0x04009346 RID: 37702
			public static LocString TITLE = "Unattended Cultures";

			// Token: 0x04009347 RID: 37703
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x02002957 RID: 10583
			public class BODY
			{
				// Token: 0x0400B47C RID: 46204
				public static LocString CONTAINER1 = "<smallcaps><b>Reminder to all Personnel</b>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color>\nTo: <b>All</b></smallcaps>\n------------------\n\n<indent=5%>For the health and safety of your fellow Facility employees, please do not store unlabeled bacterial cultures in the cafeteria fridge.\n\nSimilarly, the cafeteria dishwasher is incapable of handling petri \"dishes\", despite the nomenclature.\n\nWe thank you for your consideration.\n\n-Admin\nThe Gravitas Facility</indent>";
			}
		}

		// Token: 0x02002092 RID: 8338
		public class A4_CASUALFRIDAY
		{
			// Token: 0x04009348 RID: 37704
			public static LocString TITLE = "Casual Friday!";

			// Token: 0x04009349 RID: 37705
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x02002958 RID: 10584
			public class BODY
			{
				// Token: 0x0400B47D RID: 46205
				public static LocString CONTAINER1 = "<smallcaps><b>Casual Friday!</b>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color>\nTo: <b>All</b></smallcaps>\n------------------\n\n<indent=5%>To all employees;\n\nThe facility is pleased to announced that starting this week, all Fridays will now be Casual Fridays!\n\nPlease enjoy the clinically proven de-stressing benefits of casual attire by wearing your favorite shirt to the lab.\n\n<b>NOTE: Any personnel found on facility premises without regulation full body protection will be put on immediate notice.</b>\n\nThank-you and have fun!\n\n-Admin\nThe Gravitas Facility</indent>";
			}
		}

		// Token: 0x02002093 RID: 8339
		public class A6_DISHBOT
		{
			// Token: 0x0400934A RID: 37706
			public static LocString TITLE = "Dishbot";

			// Token: 0x0400934B RID: 37707
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x02002959 RID: 10585
			public class BODY
			{
				// Token: 0x0400B47E RID: 46206
				public static LocString CONTAINER1 = "<smallcaps><b>Please Claim Your Bot</b>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color>\nTo: <b>All</b></smallcaps>\n------------------\n\n<indent=5%>While we appreciate your commitment to office upkeep, we would like to inform whomever installed a dishwashing droid in the cafeteria that your prototype was found grievously misusing dish soap and has been forcefully terminated.\n\nThe remains may be collected at Security Block B.\n\nWe apologize for the inconvenience and thank you for your timely collection of this prototype.\n\n-Admin\nThe Gravitas Facility</indent>";
			}
		}

		// Token: 0x02002094 RID: 8340
		public class A1_MAILROOMETIQUETTE
		{
			// Token: 0x0400934C RID: 37708
			public static LocString TITLE = "Mailroom Etiquette";

			// Token: 0x0400934D RID: 37709
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x0200295A RID: 10586
			public class BODY
			{
				// Token: 0x0400B47F RID: 46207
				public static LocString CONTAINER1 = "<smallcaps><b>Reminder: Mailroom Etiquette</b>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color>\nTo: <b>All</b></smallcaps>\n------------------\n\n<indent=5%>Please do not have live bees delivered to the office mail room. Requests and orders for experimental test subjects may be processed through admin.\n\n<i>Please request all test subjects through admin.</i>\n\nThank-you.\n\n-Admin\nThe Gravitas Facility</indent>";
			}
		}

		// Token: 0x02002095 RID: 8341
		public class B2_MEETTHEPILOT
		{
			// Token: 0x0400934E RID: 37710
			public static LocString TITLE = "Meet the Pilot";

			// Token: 0x0400934F RID: 37711
			public static LocString TITLE2 = "Captain Mae Johannsen";

			// Token: 0x04009350 RID: 37712
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x0200295B RID: 10587
			public class BODY
			{
				// Token: 0x0400B480 RID: 46208
				public static LocString CONTAINER1 = "<indent=%5>From the time she was old enough to walk Captain Johannsen dreamed of reaching the sky. Growing up on an air force base she came to love the sound jet engines roaring overhead. At 16 she became the youngest pilot ever to fly a fighter jet, and at 22 she had already entered the space flight program.\n\nFour years later Gravitas nabbed her for an exclusive contract piloting our space shuttles. In her time at Gravitas, Captain Johannsen has logged over 1,000 hours space flight time shuttling and deploying satellites to Low Earth Orbits and has just been named the pilot of our inaugural civilian space tourist program, slated to begin in the next year.\n\nGravitas is excited to have Captain Johannsen in the pilot seat as we reach for the stars...and beyond!</indent>";

				// Token: 0x0400B481 RID: 46209
				public static LocString CONTAINER2 = "<indent=%10><smallcaps>\n\nBrought to you by the Gravitas Facility.</indent>";
			}
		}

		// Token: 0x02002096 RID: 8342
		public class A3_NEWSECURITY
		{
			// Token: 0x04009351 RID: 37713
			public static LocString TITLE = "NEW SECURITY PROTOCOL";

			// Token: 0x04009352 RID: 37714
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x0200295C RID: 10588
			public class BODY
			{
				// Token: 0x0400B482 RID: 46210
				public static LocString CONTAINER1 = "<smallcaps><b>Subject: New Security Protocol</b>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color>\nTo: <b>All</b></smallcaps>\n------------------\n\n<indent=5%>NOTICE TO ALL PERSONNEL\n\nWe are currently undergoing critical changes to facility security that may affect your workflow and accessibility.\n\nTo use the system, simply remove all hand coverings and place your hand on the designated scan area, then wait as the system verifies your employee identity.\n\nPLEASE NOTE\n\nAll keycards must be returned to the front desk by [REDACTED]. For questions or rescheduling, please contact security at [REDACTED]@GRAVITAS.NOVA.\n\nThank-you.\n\n-Admin\nThe Gravitas Facility</indent>";
			}
		}

		// Token: 0x02002097 RID: 8343
		public class A0_PROPFACILITYDISPLAY1
		{
			// Token: 0x04009353 RID: 37715
			public static LocString TITLE = "Printing Pod Promo";

			// Token: 0x04009354 RID: 37716
			public static LocString SUBTITLE = "PUBLIC RELEASE";

			// Token: 0x0200295D RID: 10589
			public class BODY
			{
				// Token: 0x0400B483 RID: 46211
				public static LocString CONTAINER1 = "Introducing the latest in 3D printing technology:\nThe Gravitas Home Printing Pod\n\nWe are proud to announce that printing advancements developed here in the Gravitas Facility will soon bring new, bio-organic production capabilities to your old home printers.\n\nWhat does that mean for the average household?\n\nDinner frustrations are a thing of the past. Simply select any of the pod's 5398 pre-programmed recipes, and voila! Delicious pot roast ready in only .87 seconds.\n\nPrefer the patented family recipe? Program your own custom meal template for an instant taste of home, or go old school and create fresh, delicious ingredients and prepare your own home cooked meal.\n\nDinnertime has never been easier!";

				// Token: 0x0400B484 RID: 46212
				public static LocString CONTAINER2 = "\nProjected for commercial availability early next year.\nBrought to you by the Gravitas Facility.";
			}
		}

		// Token: 0x02002098 RID: 8344
		public class A0_PROPFACILITYDISPLAY2
		{
			// Token: 0x04009355 RID: 37717
			public static LocString TITLE = "Mining Gun Promo";

			// Token: 0x04009356 RID: 37718
			public static LocString SUBTITLE = "PUBLIC RELEASE";

			// Token: 0x0200295E RID: 10590
			public class BODY
			{
				// Token: 0x0400B485 RID: 46213
				public static LocString CONTAINER1 = "Bring your mining operations into the twenty-third century with new Gravitas personal excavators.\n\nImproved particle condensers reduce raw volume for more efficient product shipping - and that's good for your bottom line.\n\nLicensed for industrial use only, resale of Gravitas equipment may carry a fine of up to $200,000 under the Global Restoration Act.";

				// Token: 0x0400B486 RID: 46214
				public static LocString CONTAINER2 = "Brought to you by the Gravitas Facility.";
			}
		}

		// Token: 0x02002099 RID: 8345
		public class A0_PROPFACILITYDISPLAY3
		{
			// Token: 0x04009357 RID: 37719
			public static LocString TITLE = "Thermo-Nullifier Promo";

			// Token: 0x04009358 RID: 37720
			public static LocString SUBTITLE = "PUBLIC RELEASE";

			// Token: 0x0200295F RID: 10591
			public class BODY
			{
				// Token: 0x0400B487 RID: 46215
				public static LocString CONTAINER1 = "Tired of shutting down during seasonal heat waves? Looking to cut weather-related operating costs?\n\nLook no further: Gravitas's revolutionary Anti Entropy Thermo-Nullifier is the exciting, affordable new way to eliminate operational downtime.\n\nPowered by our proprietary renewable power sources, the AETN efficiently cools an entire office building without incurring any of the environmental surcharges associated with comparable systems.\n\nInitial setup includes hydrogen duct installation and discounted monthly maintenance visits from our elite team of specially trained contractors.\n\nNow available for pre-order!";

				// Token: 0x0400B488 RID: 46216
				public static LocString CONTAINER2 = "Brought to you by the Gravitas Facility.\n<smallcaps>Patent Pending</smallcaps>";
			}
		}

		// Token: 0x0200209A RID: 8346
		public class B1_SPACEFACILITYDISPLAY1
		{
			// Token: 0x04009359 RID: 37721
			public static LocString TITLE = "Office Space in Space!";

			// Token: 0x0400935A RID: 37722
			public static LocString SUBTITLE = "PUBLIC RELEASE";

			// Token: 0x02002960 RID: 10592
			public class BODY
			{
				// Token: 0x0400B489 RID: 46217
				public static LocString CONTAINER1 = "Bring your office to the stars with Gravitas new corporate space stations.\n\nEnjoy a captivated workforce with over 600 square feet of office space in low earth orbit. Stunning views, a low gravity gym and a cafeteria serving the finest nutritional bars await your personnel.\n\nDaily to and from missions to your satellite office via our luxury space shuttles.\n\nRest assured our space stations and shuttles utilize only the extremely efficient, environmentally friendly Gravitas proprietary power sources.\n\nThe workplace revolution starts now!";

				// Token: 0x0400B48A RID: 46218
				public static LocString CONTAINER2 = "Taking reservations now for the first orbital office spaces.\n100% money back guarantee (minus 10% filing fee)";
			}
		}

		// Token: 0x0200209B RID: 8347
		public class BLUE_GRASS
		{
			// Token: 0x0400935B RID: 37723
			public static LocString TITLE = "Alveo Vera";

			// Token: 0x0400935C RID: 37724
			public static LocString SUBTITLE = "Plant";

			// Token: 0x02002961 RID: 10593
			public class BODY
			{
				// Token: 0x0400B48B RID: 46219
				public static LocString CONTAINER1 = string.Concat(new string[]
				{
					"The Alveo Vera's fleshy stems are dotted with small apertures featuring bidirectional valves through which ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" is absorbed and sticky oxygenated waste is secreted.\n\nThis buildup resulting from this respiration cycle crystallizes into ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" ore.\n\nHorticulturists have long been curious about the protective epithelium that prevents the ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" ore from sublimating while on the plant. Unfortunately, it is too fragile to survive handling, and has thus far proven impossible to study."
				});
			}
		}

		// Token: 0x0200209C RID: 8348
		public class ARBORTREE
		{
			// Token: 0x0400935D RID: 37725
			public static LocString TITLE = "Arbor Tree";

			// Token: 0x0400935E RID: 37726
			public static LocString SUBTITLE = "Wood Tree";

			// Token: 0x02002962 RID: 10594
			public class BODY
			{
				// Token: 0x0400B48C RID: 46220
				public static LocString CONTAINER1 = "Arbor Trees have been cultivated to spread horizontally when they grow so as to produce a high yield of lumber in vertically cramped spaces.\n\nArbor Trees are related to the oak tree, specifically the Japanese Evergreen, though they have been genetically hybridized significantly.\n\nDespite having many hardy, evenly spaced branches, the short stature of the Arbor Tree makes climbing it rather irrelevant.";
			}
		}

		// Token: 0x0200209D RID: 8349
		public class BALMLILY
		{
			// Token: 0x0400935F RID: 37727
			public static LocString TITLE = "Balm Lily";

			// Token: 0x04009360 RID: 37728
			public static LocString SUBTITLE = "Medicinal Herb";

			// Token: 0x02002963 RID: 10595
			public class BODY
			{
				// Token: 0x0400B48D RID: 46221
				public static LocString CONTAINER1 = "The Balm Lily naturally contains high vitamin concentrations and produces acids similar in molecular makeup to acetylsalicylic acid (commonly known as aspirin).\n\nAs a result, the plant is ideal both for boosting immune systems and treating a variety of common maladies such as pain and fever.";
			}
		}

		// Token: 0x0200209E RID: 8350
		public class BLISSBURST
		{
			// Token: 0x04009361 RID: 37729
			public static LocString TITLE = "Bliss Burst";

			// Token: 0x04009362 RID: 37730
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x02002964 RID: 10596
			public class BODY
			{
				// Token: 0x0400B48E RID: 46222
				public static LocString CONTAINER1 = "The Bliss Burst is a succulent in the genus Haworthia and is a hardy plant well-suited for beginner gardeners.\n\nThey require little in the way of upkeep, to the point that the most common cause of death for Bliss Bursts is overwatering from over-eager carers.";
			}
		}

		// Token: 0x0200209F RID: 8351
		public class BLUFFBRIAR
		{
			// Token: 0x04009363 RID: 37731
			public static LocString TITLE = "Bluff Briar";

			// Token: 0x04009364 RID: 37732
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x02002965 RID: 10597
			public class BODY
			{
				// Token: 0x0400B48F RID: 46223
				public static LocString CONTAINER1 = "Bluff Briars have formed a symbiotic relationship with a closely related plant strain, the " + UI.FormatAsLink("Bristle Blossom", "PRICKLEFLOWER") + ".\n\nThey tend to thrive in areas where the Bristle Blossom is present, as the berry it produces emits a rare chemical while decaying that the Briar is capable of absorbing to supplement its own pheromone production.";

				// Token: 0x0400B490 RID: 46224
				public static LocString CONTAINER2 = "Due to the Bluff Briar's unique pheromonal \"charm\" defense, animals are extremely unlikely to eat it in the wild.\n\nAs a result, the Briar's barbs have become ineffectual over time and are unlikely to cause injury, unlike the Bristle Blossom, which possesses barbs that are exceedingly sharp and require careful handling.";
			}
		}

		// Token: 0x020020A0 RID: 8352
		public class BOGBUCKET
		{
			// Token: 0x04009365 RID: 37733
			public static LocString TITLE = "Bog Bucket";

			// Token: 0x04009366 RID: 37734
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x02002966 RID: 10598
			public class BODY
			{
				// Token: 0x0400B491 RID: 46225
				public static LocString CONTAINER1 = "Bog Buckets get their name from their bucket-like flowers and their propensity to grow in swampy, bog-like environments.\n\nThe flower secretes a thick, sweet liquid which collects at the bottom of the bucket and can be gathered for consumption.\n\nThough not inherently dangerous, the interior of the Bog Bucket flower is so warm and inviting that it has tempted individuals to climb inside for a nap, only to awake trapped in its sticky sap.";
			}
		}

		// Token: 0x020020A1 RID: 8353
		public class BRISTLEBLOSSOM
		{
			// Token: 0x04009367 RID: 37735
			public static LocString TITLE = "Bristle Blossom";

			// Token: 0x04009368 RID: 37736
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x02002967 RID: 10599
			public class BODY
			{
				// Token: 0x0400B492 RID: 46226
				public static LocString CONTAINER1 = "The Bristle Blossom is frequently cultivated for its calorie dense and relatively fast growing Bristle Berries.\n\nConsumption of the berry requires special preparation due to the thick barbs surrounding the edible fruit.\n\nThe term \"Bristle Berry\" is, in fact, a misnomer, as it is not a \"berry\" by botanical definition but an aggregate fruit made up of many smaller fruitlets.";
			}
		}

		// Token: 0x020020A2 RID: 8354
		public class BUDDYBUD
		{
			// Token: 0x04009369 RID: 37737
			public static LocString TITLE = "Buddy Bud";

			// Token: 0x0400936A RID: 37738
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x02002968 RID: 10600
			public class BODY
			{
				// Token: 0x0400B493 RID: 46227
				public static LocString CONTAINER1 = "As a byproduct of photosynthesis, the Buddy Bud naturally secretes a compound that is chemically similar to the neuropeptide created in the human brain after receiving a hug.";
			}
		}

		// Token: 0x020020A3 RID: 8355
		public class DASHASALTVINE
		{
			// Token: 0x0400936B RID: 37739
			public static LocString TITLE = "Dasha Salt Vine";

			// Token: 0x0400936C RID: 37740
			public static LocString SUBTITLE = "Edible Spice Plant";

			// Token: 0x02002969 RID: 10601
			public class BODY
			{
				// Token: 0x0400B494 RID: 46228
				public static LocString CONTAINER1 = "The Dasha Saltvine is a unique plant that needs large amounts of salt to balance the levels of water in its body.\n\nIn order to keep a supply of salt on hand, the end of the vine is coated in microscopic formations which bind with sodium atoms, forming large crystals over time.";
			}
		}

		// Token: 0x020020A4 RID: 8356
		public class DUSKCAP
		{
			// Token: 0x0400936D RID: 37741
			public static LocString TITLE = "Dusk Cap";

			// Token: 0x0400936E RID: 37742
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x0200296A RID: 10602
			public class BODY
			{
				// Token: 0x0400B495 RID: 46229
				public static LocString CONTAINER1 = "Like many species of mushroom, Dusk Caps thrive in dark areas otherwise ill-suited to the cultivation of plants.\n\nIn place of typical chlorophyll, the underside of a Dusk Cap is fitted with thousands of specialized gills, which it uses to draw in carbon dioxide and aid in its growth.";
			}
		}

		// Token: 0x020020A5 RID: 8357
		public class EXPERIMENT52B
		{
			// Token: 0x0400936F RID: 37743
			public static LocString TITLE = "Experiment 52B";

			// Token: 0x04009370 RID: 37744
			public static LocString SUBTITLE = "Plant?";

			// Token: 0x0200296B RID: 10603
			public class BODY
			{
				// Token: 0x0400B496 RID: 46230
				public static LocString CONTAINER1 = "Experiment 52B is an aggressive, yet sessile creature that produces " + 5f.ToString() + " kilograms of resin per 1000 kcal it consumes.\n\nDuplicants would do well to maintain a safe distance when delivering food to Experiment 52B.\n\nWhile this creature may look like a tree, its taxonomy more closely resembles a giant land-based coral with cybernetic implants.\n\nAlthough normally lab-grown creatures would be given a better name than Experiment 52B, in this particular case the experimenting scientists weren't sure that they were done.";
			}
		}

		// Token: 0x020020A6 RID: 8358
		public class GASGRASS
		{
			// Token: 0x04009371 RID: 37745
			public static LocString TITLE = "Gas Grass";

			// Token: 0x04009372 RID: 37746
			public static LocString SUBTITLE = "Critter Feed";

			// Token: 0x0200296C RID: 10604
			public class BODY
			{
				// Token: 0x0400B497 RID: 46231
				public static LocString CONTAINER1 = "Much remains a mystery about the biology of Gas Grass, a plant-like lifeform only recently recovered from missions into outer space.\n\nHowever, it appears to use ambient radiation from space as an energy source, growing rapidly when given a suitable " + UI.FormatAsLink("Liquid Chlorine", "CHLORINE") + "-laden environment.";

				// Token: 0x0400B498 RID: 46232
				public static LocString CONTAINER2 = "Initially there was worry that transplanting a Gas Grass specimen on planet or gravity-laden terrestrial body would collapse its internal structures. Luckily, Gas Grass has evolved sturdy tubules to prevent structural damage in the event of pressure changes between its internally transported chlorine and its external environment.";
			}
		}

		// Token: 0x020020A7 RID: 8359
		public class GINGER
		{
			// Token: 0x04009373 RID: 37747
			public static LocString TITLE = "Tonic Root";

			// Token: 0x04009374 RID: 37748
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x0200296D RID: 10605
			public class BODY
			{
				// Token: 0x0400B499 RID: 46233
				public static LocString CONTAINER1 = "Tonic Root is a close relative of the zingiberaceae family commonly known as ginger. Its heavily burled shoots are typically light brown in colour, and enveloped in a thin layer of protective, edible bark.";

				// Token: 0x0400B49A RID: 46234
				public static LocString CONTAINER2 = "In addition to its use as an aromatic culinary ingredient, it has traditionally been employed as a tonic for a variety of minor digestive ailments.";

				// Token: 0x0400B49B RID: 46235
				public static LocString CONTAINER3 = "Its stringy fibers can become irretrievably embedded between one's teeth during mastication.";
			}
		}

		// Token: 0x020020A8 RID: 8360
		public class GRUBFRUITPLANT
		{
			// Token: 0x04009375 RID: 37749
			public static LocString TITLE = "Grubfruit Plant";

			// Token: 0x04009376 RID: 37750
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x0200296E RID: 10606
			public class BODY
			{
				// Token: 0x0400B49C RID: 46236
				public static LocString CONTAINER1 = "The Grubfruit Plant exhibits a coevolutionary relationship with the Divergent species.\n\nThough capable of producing fruit without the help of the Divergent, the Spindly Grubfruit is a substandard version of the Grubfruit in both taste and caloric value.\n\nThe mechanism for how the Divergent inspires Grubfruit Plant growth is not entirely known but is thought to be somehow tied to the infrasonic 'songs' these insects lovingly purr to their plants.";
			}
		}

		// Token: 0x020020A9 RID: 8361
		public class HEXALENT
		{
			// Token: 0x04009377 RID: 37751
			public static LocString TITLE = "Hexalent";

			// Token: 0x04009378 RID: 37752
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x0200296F RID: 10607
			public class BODY
			{
				// Token: 0x0400B49D RID: 46237
				public static LocString CONTAINER1 = "While most plants grow new sections and leaves according to the Fibonacci Sequence, the Hexalent forms new sections similar to how atoms form into crystal structures.\n\nThe result is a geometric pattern that resembles a honeycomb.";
			}
		}

		// Token: 0x020020AA RID: 8362
		public class HYDROCACTUS
		{
			// Token: 0x04009379 RID: 37753
			public static LocString TITLE = "Hydrocactus";

			// Token: 0x0400937A RID: 37754
			public static LocString SUBTITLE = "Plant";

			// Token: 0x02002970 RID: 10608
			public class BODY
			{
				// Token: 0x0400B49E RID: 46238
				public static LocString CONTAINER1 = "";
			}
		}

		// Token: 0x020020AB RID: 8363
		public class ICEFLOWER
		{
			// Token: 0x0400937B RID: 37755
			public static LocString TITLE = "Idylla Flower";

			// Token: 0x0400937C RID: 37756
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x02002971 RID: 10609
			public class BODY
			{
				// Token: 0x0400B49F RID: 46239
				public static LocString CONTAINER1 = "Idylla Flowers are a rare species of everblooms that thrive with very little care, making them a perennial favorite among newbie gardeners.\n\nTheir springy blossoms can be 'bopped' gently for sensory entertainment, but hands should be washed immediately as the petal residue can permanently stain most textiles.";
			}
		}

		// Token: 0x020020AC RID: 8364
		public class JUMPINGJOYA
		{
			// Token: 0x0400937D RID: 37757
			public static LocString TITLE = "Jumping Joya";

			// Token: 0x0400937E RID: 37758
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x02002972 RID: 10610
			public class BODY
			{
				// Token: 0x0400B4A0 RID: 46240
				public static LocString CONTAINER1 = "The Jumping Joya is a decorative plant that brings a feeling of calmness and wellbeing to individuals in its vacinity.\n\nTheir rounded appendages and eccentrically shaped polyps are a favorite of interior designers looking to offset the rigid straight walls of an institutional setting.\n\nThe Jumping Joya's capacity to thrive in many environments and the ease in which they propagate make them the go-to house plant for the lazy gardener.";
			}
		}

		// Token: 0x020020AD RID: 8365
		public class MEALWOOD
		{
			// Token: 0x0400937F RID: 37759
			public static LocString TITLE = "Mealwood";

			// Token: 0x04009380 RID: 37760
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x02002973 RID: 10611
			public class BODY
			{
				// Token: 0x0400B4A1 RID: 46241
				public static LocString CONTAINER1 = "Mealwood is an bramble-like plant that has a parasitic symbiotic relationship with the nutrient-rich Meal Lice that inhabit it.\n\nMealwood experience a rapid growth rate in its first stages, but once the Meal Lice become active they consume all the new fruiting spurs on the plant before they can fully mature.\n\nTheoretically the flowers of this plant are a beautiful color of fuchsia, however no Mealwood has ever reached the point of flowering without being overrun by the parasitic Meal Lice.";
			}
		}

		// Token: 0x020020AE RID: 8366
		public class MELLOWMALLOW
		{
			// Token: 0x04009381 RID: 37761
			public static LocString TITLE = "Mellow Mallow";

			// Token: 0x04009382 RID: 37762
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x02002974 RID: 10612
			public class BODY
			{
				// Token: 0x0400B4A2 RID: 46242
				public static LocString CONTAINER1 = "The Mellow Mallow is a type of fungus that is known for its ease of propagation when cut.\n\nIt is deadly when consumed, however creatures that mistakenly eat it are said to experience a state of extreme calm before death.";
			}
		}

		// Token: 0x020020AF RID: 8367
		public class MIRTHLEAF
		{
			// Token: 0x04009383 RID: 37763
			public static LocString TITLE = "Mirth Leaf";

			// Token: 0x04009384 RID: 37764
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x02002975 RID: 10613
			public class BODY
			{
				// Token: 0x0400B4A3 RID: 46243
				public static LocString CONTAINER1 = "The Mirth Leaf is a broad-leafed house plant used for decorating living spaces.\n\nThe joyous bobbing of the wide green leaves provides hours of amusement for those desperate for entertainment.\n\nAlthough the Mirth Leaf can inspire laughter and joy, it is not cut out for a career in stand-up comedy.";
			}
		}

		// Token: 0x020020B0 RID: 8368
		public class MUCKROOT
		{
			// Token: 0x04009385 RID: 37765
			public static LocString TITLE = "Muckroot";

			// Token: 0x04009386 RID: 37766
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x02002976 RID: 10614
			public class BODY
			{
				// Token: 0x0400B4A4 RID: 46244
				public static LocString CONTAINER1 = "The Muckroot is an aggressively invasive yet exceedingly delicate root plant known for its earthy flavor and unusual texture.\n\nIt is easy to store and keeps for unusually long periods of time, characteristics that once made it a staple food for explorers on long expeditions.";
			}
		}

		// Token: 0x020020B1 RID: 8369
		public class NOSHBEAN
		{
			// Token: 0x04009387 RID: 37767
			public static LocString TITLE = "Nosh Bean Plant";

			// Token: 0x04009388 RID: 37768
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x02002977 RID: 10615
			public class BODY
			{
				// Token: 0x0400B4A5 RID: 46245
				public static LocString CONTAINER1 = "The Nosh Bean Plant produces a nutritious bean that can function as a delicious meat substitute provided it is properly processed.\n\nThough the bean is a food source, it also functions as the seed for the Nosh Bean plant.\n\nWhile using the Nosh Bean for nourishment would seem like the more practical application, doing so would deprive individuals of the immense gratification experienced by planting this bean and watching it flourish into maturity.";
			}
		}

		// Token: 0x020020B2 RID: 8370
		public class OXYFERN
		{
			// Token: 0x04009389 RID: 37769
			public static LocString TITLE = "Oxyfern";

			// Token: 0x0400938A RID: 37770
			public static LocString SUBTITLE = "Plant";

			// Token: 0x02002978 RID: 10616
			public class BODY
			{
				// Token: 0x0400B4A6 RID: 46246
				public static LocString CONTAINER1 = "Oxyferns have perhaps the highest metabolism in the plant kingdom, absorbing relatively large amounts of carbon dioxide and converting it into oxygen in quantities disproportionate to their small size.\n\nThey subsequently thrive in areas with abundant animal wildlife or ambiently high carbon dioxide concentrations.";
			}
		}

		// Token: 0x020020B3 RID: 8371
		public class HARDSKINBERRYPLANT
		{
			// Token: 0x0400938B RID: 37771
			public static LocString TITLE = "Pikeapple Bush";

			// Token: 0x0400938C RID: 37772
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x02002979 RID: 10617
			public class BODY
			{
				// Token: 0x0400B4A7 RID: 46247
				public static LocString CONTAINER1 = "The Pikeapple Bush produces a nutritious fruit distantly related to those in the Durio genus.\n\nThose who find the Pikeapple pulp's fragrance overwhelming should consume their portion whilst standing near the plant itself; the shrubbery's gentle swaying produces a wafting effect that promotes air circulation.\n\nClosed-toe footwear is recommended, as barefoot contact with the plant's sharp seeds inevitably leads to infection.";
			}
		}

		// Token: 0x020020B4 RID: 8372
		public class PINCHAPEPPERPLANT
		{
			// Token: 0x0400938D RID: 37773
			public static LocString TITLE = "Pincha Pepperplant";

			// Token: 0x0400938E RID: 37774
			public static LocString SUBTITLE = "Edible Spice Plant";

			// Token: 0x0200297A RID: 10618
			public class BODY
			{
				// Token: 0x0400B4A8 RID: 46248
				public static LocString CONTAINER1 = "The Pincha Pepperplant is a tropical vine with a reduced lignin structural system that renders it incapable of growing upward from the ground.\n\nThe plant therefore prefers to embed its roots into tall trees and rocky outcrops, the result of which is an inverse of the plant's natural gravitropism, causing its stem to prefer growing downwards while the roots tend to grow up.";
			}
		}

		// Token: 0x020020B5 RID: 8373
		public class SATURNCRITTERTRAP
		{
			// Token: 0x0400938F RID: 37775
			public static LocString TITLE = "Saturn Critter Trap";

			// Token: 0x04009390 RID: 37776
			public static LocString SUBTITLE = "Carnivorous Plant";

			// Token: 0x0200297B RID: 10619
			public class BODY
			{
				// Token: 0x0400B4A9 RID: 46249
				public static LocString CONTAINER1 = "The Saturn Critter Trap plant is a carnivorous plant that lays in wait for unsuspecting critters to happen by, then traps them in its mouth for consumption.\n\nThe Saturn Trap Plant's predatory mechanism is reflective of the harsh radioactive habitat it resides in.\n\nOnce trapped in the deadly maw of the plant, creatures are gently asphyxiated then digested through powerful acidic enzymes which coat the inner sides of the Saturn Trap Plant's leaves.";
			}
		}

		// Token: 0x020020B6 RID: 8374
		public class SHERBERRY
		{
			// Token: 0x04009391 RID: 37777
			public static LocString TITLE = "Sherberry Plant";

			// Token: 0x04009392 RID: 37778
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x0200297C RID: 10620
			public class BODY
			{
				// Token: 0x0400B4AA RID: 46250
				public static LocString CONTAINER1 = "The semi-parasitic Sherberry plant leeches moisture and trace minerals from the primordial ice formations in which it grows.\n\nThe fruit of this varietal contains low levels of stomach-upsetting phoratoxins which, while not fatal, do serve as strong motivation for foragers to seek out additional sources of nutrition.";
			}
		}

		// Token: 0x020020B7 RID: 8375
		public class SLEETWHEAT
		{
			// Token: 0x04009393 RID: 37779
			public static LocString TITLE = "Sleet Wheat";

			// Token: 0x04009394 RID: 37780
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x0200297D RID: 10621
			public class BODY
			{
				// Token: 0x0400B4AB RID: 46251
				public static LocString CONTAINER1 = "The Sleet Wheat plant has become so well-adapted to cold environments, it is no longer able to survive at room temperatures.";

				// Token: 0x0400B4AC RID: 46252
				public static LocString CONTAINER2 = "The grain of the Sleet Wheat can be ground down into high quality foodstuffs, or planted to cultivate further Sleet Wheat plants.";
			}
		}

		// Token: 0x020020B8 RID: 8376
		public class SPACETREE
		{
			// Token: 0x04009395 RID: 37781
			public static LocString TITLE = "Bonbon Tree";

			// Token: 0x04009396 RID: 37782
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x0200297E RID: 10622
			public class BODY
			{
				// Token: 0x0400B4AD RID: 46253
				public static LocString CONTAINER1 = "The Bonbon Tree is a towering plant developed to thrive in below-freezing temperatures. It features multiple independently functioning branches that synthesize bright light to funnel nutrients into a hollow central core.\n\nOnce the tree is fully grown, the core secretes digestive enzymes that break down surplus nutrients and store them as thick, sweet fluid. This can be refined into " + UI.FormatAsLink("Sucrose", "SUCROSE") + " for the production of higher-tier foods, or used as-is to sustain Spigot Seal ranches.\n\nBonbon Trees are generally considered an eyesore, and would likely be eradicated if not for their delicious output.";
			}
		}

		// Token: 0x020020B9 RID: 8377
		public class SPINDLYGRUBFRUITPLANT
		{
			// Token: 0x04009397 RID: 37783
			public static LocString TITLE = "Spindly Grubfruit Plant";

			// Token: 0x04009398 RID: 37784
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x0200297F RID: 10623
			public class BODY
			{
				// Token: 0x0400B4AE RID: 46254
				public static LocString CONTAINER1 = "";
			}
		}

		// Token: 0x020020BA RID: 8378
		public class SPORECHID
		{
			// Token: 0x04009399 RID: 37785
			public static LocString TITLE = "Sporechid";

			// Token: 0x0400939A RID: 37786
			public static LocString SUBTITLE = "Poisonous Plant";

			// Token: 0x02002980 RID: 10624
			public class BODY
			{
				// Token: 0x0400B4AF RID: 46255
				public static LocString CONTAINER1 = "Sporechids take advantage of their flower's attractiveness to lure unsuspecting victims into clouds of parasitic Zombie Spores.\n\nThey are a rare form of holoparasitic plant which finds mammalian hosts to infect rather than the usual plant species.\n\nThe Zombie Spore was originally designed for medicinal purposes but its sedative properties were never refined to the point of usefulness.";
			}
		}

		// Token: 0x020020BB RID: 8379
		public class SWAMPCHARD
		{
			// Token: 0x0400939B RID: 37787
			public static LocString TITLE = "Swamp Chard";

			// Token: 0x0400939C RID: 37788
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x02002981 RID: 10625
			public class BODY
			{
				// Token: 0x0400B4B0 RID: 46256
				public static LocString CONTAINER1 = "Swamp Chard is a unique member of the Amaranthaceae family that has adapted to grow in humid environments, in or near pools of standing water.\n\nWhile the leaves are technically edible, the most nutritious and palatable part of the plant is the heart, which is rich in a number of essential vitamins.";
			}
		}

		// Token: 0x020020BC RID: 8380
		public class THIMBLEREED
		{
			// Token: 0x0400939D RID: 37789
			public static LocString TITLE = "Thimble Reed";

			// Token: 0x0400939E RID: 37790
			public static LocString SUBTITLE = "Textile Plant";

			// Token: 0x02002982 RID: 10626
			public class BODY
			{
				// Token: 0x0400B4B1 RID: 46257
				public static LocString CONTAINER1 = "The Thimble Reed is a wetlands plant used in the production of high quality fabrics prized for their softness and breathability.\n\nCloth made from the Thimble Reed owes its exceptional softness to the fineness of its fibers and the unusual length to which they grow.";
			}
		}

		// Token: 0x020020BD RID: 8381
		public class TRANQUILTOES
		{
			// Token: 0x0400939F RID: 37791
			public static LocString TITLE = "Tranquil Toes";

			// Token: 0x040093A0 RID: 37792
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x02002983 RID: 10627
			public class BODY
			{
				// Token: 0x0400B4B2 RID: 46258
				public static LocString CONTAINER1 = "Tranquil Toes are a decorative succulent that flourish in a radioactive environment.\n\nThough most of the flora and fauna that thrive a harsh radioactive biome tends to be aggressive, Tranquil Toes provide a rare exception to this rule.\n\nIt is a generally believed that the morale boosting abilities of this plant come from its resemblence to a funny hat one might wear at a party.";
			}
		}

		// Token: 0x020020BE RID: 8382
		public class WATERWEED
		{
			// Token: 0x040093A1 RID: 37793
			public static LocString TITLE = "Waterweed";

			// Token: 0x040093A2 RID: 37794
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x02002984 RID: 10628
			public class BODY
			{
				// Token: 0x0400B4B3 RID: 46259
				public static LocString CONTAINER1 = "An inexperienced farmer may assume at first glance that the transluscent, fluid-containing bulb atop the Waterweed is the edible portion of the plant.\n\nIn fact, the bulb is extremely poisonous and should never be consumed under any circumstances.";
			}
		}

		// Token: 0x020020BF RID: 8383
		public class WHEEZEWORT
		{
			// Token: 0x040093A3 RID: 37795
			public static LocString TITLE = "Wheezewort";

			// Token: 0x040093A4 RID: 37796
			public static LocString SUBTITLE = "Plant?";

			// Token: 0x02002985 RID: 10629
			public class BODY
			{
				// Token: 0x0400B4B4 RID: 46260
				public static LocString CONTAINER1 = "The Wheezewort is best known for its ability to alter the temperature of its surrounding environment, directly absorbing heat energy to maintain its bodily processes.\n\nThis environmental management also serves to enact a type of self-induced hibernation, slowing the Wheezewort's metabolism to require less nutrients over long periods of time.";

				// Token: 0x0400B4B5 RID: 46261
				public static LocString CONTAINER2 = "Deceptive in appearance, this member of the Cnidaria phylum is in fact an animal, not a plant.\n\nWheezewort cells contain no chloroplasts, vacuoles or cell walls, and are incapable of photosynthesis.\n\nInstead, the Wheezewort respires in a recently developed method similar to amphibians, using its membranous skin for cutaneous respiration.";

				// Token: 0x0400B4B6 RID: 46262
				public static LocString CONTAINER3 = "A series of cream-colored capillaries pump blood throughout the animal before unused air is expired back out through the skin.\n\nWheezeworts do not possess a brain or a skeletal structure, and are instead supported by a jelly-like mesoglea located beneath its outer respiratory membrane.";
			}
		}

		// Token: 0x020020C0 RID: 8384
		public class B10_AI
		{
			// Token: 0x040093A5 RID: 37797
			public static LocString TITLE = "A Paradox";

			// Token: 0x040093A6 RID: 37798
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x02002986 RID: 10630
			public class BODY
			{
				// Token: 0x0400B4B7 RID: 46263
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111-1]</smallcaps>\n\n[LOG BEGINS]\n\nI made a horrible discovery today while reviewing work on the artificial intelligence programming. It seems Dr. Ali mixed up a file when uploading a program onto a rudimentary robot and discovered that the device displayed the characteristics of what he called \"a puppy that was lost in a teleportation experiment weeks ago\".\n\nThis is unbelievable! Jackie has been hiding the nature of the teleportation experiments from me. What's worse is I know from previous conversations that she knows I would never approve of pursuing this line of experimentation. The societal benefits of teleportation aside, you <i>cannot</i> kill a living being every time you want to send them to another room. The moral and ethical implications of this are horrendous.\n\nI know she has been keeping this information from me. When I searched through the Gravitas database I found nothing to do with these teleportation experiments. It was only because this reference showed up in Dr. Ali's AI paper that I was able to discover what has been happening.\n\nJackie has to be stopped.\n\nBut I know she is beyond reasonable discussion. I hope this is the only thing she is hiding from me, but I fear it is not.\n\n[LOG ENDS]\n\n[LOG BEGINS]\n\nDespite myself, I can't help thinking of the intriguing possiblities this presents for the AI development. It haunts me.\n\nI fear I may be sliding down a slippery slope, at the bottom of which Jackie is waiting for me with open arms.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x020020C1 RID: 8385
		public class A2_AGRICULTURALNOTES
		{
			// Token: 0x040093A7 RID: 37799
			public static LocString TITLE = "Agricultural Notes";

			// Token: 0x040093A8 RID: 37800
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x02002987 RID: 10631
			public class BODY
			{
				// Token: 0x0400B4B8 RID: 46264
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B577]</smallcaps>\n\n[LOG BEGINS]\n\nGeneticist: We've engineered crops to be rotated as needed depending on environmental situation. While a variety of plants would be ideal to supplement any remaining nutritional needs, any one of our designs would be enough to sustain a colony indefinitely without adverse effects on physical health.\n\nGeneticist: Some environmental survival issues still remain. Differing temperatures, light availability and last pass changes to nutrient levels take top priority, particularly for food and oxygen producing plants.\n\n[LOG ENDS]";

				// Token: 0x0400B4B9 RID: 46265
				public static LocString CONTAINER2 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...Selected in response to concerns about colony psychological well-being.\n\nWhile design should focus on attributing mood-enhancing effects to natural Briar pheromone emissions, the project has been moved to the lowest priority level beneath more life-sustaining designs...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400B4BA RID: 46266
				public static LocString CONTAINER3 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...It is yet unknown if we can surmount the obstacles that stand in the way of engineering a root capable of reproduction in the more uninhabitable situations we anticipate for our colonies, or whether it is even worth the effort...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400B4BB RID: 46267
				public static LocString CONTAINER4 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...The Mealwood's hardiness will make it a potential contingency crop should Bristle Blossoms be unable to sustain sizable populations.\n\nIf pursued, design should focus on longterm viability and solving the psychological repercussions of prolonged Mealwood grain ingestion...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400B4BC RID: 46268
				public static LocString CONTAINER5 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...The Thimble Reed will be used as a contingency for textile production in the event that printed materials not be sufficient.\n\nDesign should focus on the yield frequency of the plant, as well as... erm... softness.\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400B4BD RID: 46269
				public static LocString CONTAINER6 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...Balm Lily is a reliable all-purpose medicinal plant.\n\nVery little need be altered, save for assurances that it will survive wherever it may be planted...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400B4BE RID: 46270
				public static LocString CONTAINER7 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...The gene sequences within the common Dusk Cap allow it to grow in low light environments.\n\nThese genes should be sampled, with the hope that we can splice them into other plant designs....\n\n[LOG ENDS]\n------------------\n";
			}
		}

		// Token: 0x020020C2 RID: 8386
		public class A1_CLONEDRABBITS
		{
			// Token: 0x040093A9 RID: 37801
			public static LocString TITLE = "Initial Success";

			// Token: 0x040093AA RID: 37802
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x02002988 RID: 10632
			public class BODY
			{
				// Token: 0x0400B4BF RID: 46271
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111]</smallcaps>\n\n[LOG BEGINS]\n\n[A throat clears.]\n\nB111: We are now reliably printing healthy, living subjects, though all have exhibited unusual qualities as a result of the cloning process.\n\n[Chattering sounds can be heard.]\n\nB111: Odd communications, abnormal excrescenses, and vestigial limbs have been seen in all subjects thus far, to varying degrees of severity. It seems that bypassing or accelerating juvenility halts certain critical stages of development. Brain function, however, appears typical.\n\n[Chattering.]\n\nB111: T-They also seem quite happy.\n\nB111: Dr. Broussard, signing off.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x020020C3 RID: 8387
		public class A1_CLONEDRACCOONS
		{
			// Token: 0x040093AB RID: 37803
			public static LocString TITLE = "Initial Success";

			// Token: 0x040093AC RID: 37804
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x02002989 RID: 10633
			public class BODY
			{
				// Token: 0x0400B4C0 RID: 46272
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111]</smallcaps>\n\n[LOG BEGINS]\n\n[A throat clears.]\n\nB111: We are now reliably printing healthy, living subjects, though all have exhibited unusual qualities as a result of the cloning process.\n\n[Trilling sounds can be heard.]\n\nB111: Unusual mewings, benign neoplasms, and atavistic extremities have been seen in all subjects thus far, to varying degrees of severity. It seems that bypassing or accelerating juvenility halts certain critical stages of development. Brain function, however, appears typical.\n\n[Trilling.]\n\nB111: T-They also seem quite happy.\n\nB111: Dr. Broussard, signing off.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x020020C4 RID: 8388
		public class A1_CLONEDRATS
		{
			// Token: 0x040093AD RID: 37805
			public static LocString TITLE = "Initial Success";

			// Token: 0x040093AE RID: 37806
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x0200298A RID: 10634
			public class BODY
			{
				// Token: 0x0400B4C1 RID: 46273
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111]</smallcaps>\n\n[LOG BEGINS]\n\n[A throat clears.]\n\nB111: We are now reliably printing healthy, living subjects, though all have exhibited unusual qualities as a result of the cloning process.\n\n[Squeaking sounds can be heard.]\n\nB111: Unusual vocalizations, benign growths, and missing appendages have been seen in all subjects thus far, to varying degrees of severity. It seems that bypassing or accelerating juvenility halts certain critical stages of development. Brain function, however, appears typical.\n\n[Squeaking.]\n\nB111: T-They also seem quite happy.\n\nB111: Dr. Broussard, signing off.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x020020C5 RID: 8389
		public class A5_GENETICOOZE
		{
			// Token: 0x040093AF RID: 37807
			public static LocString TITLE = "Biofluid";

			// Token: 0x040093B0 RID: 37808
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x0200298B RID: 10635
			public class BODY
			{
				// Token: 0x0400B4C2 RID: 46274
				public static LocString CONTAINER1 = "[LOG BEGINS]\n\nThe Printing Pod is primed by a synthesized bio-organic concoction the technicians have taken to calling \"Ooze\", a specialized mixture composed of water, carbon, and dozens upon dozens of the trace elements necessary for the creation of life.\n\nThe pod reconstitutes these elements into a living organism using the blueprints we feed it, before finally administering a shock of life.\n\nIt is like any other 3D printer. We just use different ink.\n\nDr. Broussard, signing off.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x020020C6 RID: 8390
		public class A4_HIBISCUS3
		{
			// Token: 0x040093B1 RID: 37809
			public static LocString TITLE = "Experiment 7D";

			// Token: 0x040093B2 RID: 37810
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x0200298C RID: 10636
			public class BODY
			{
				// Token: 0x0400B4C3 RID: 46275
				public static LocString CONTAINER1 = "EXPERIMENT 7D\nSecurity Code: B111\n\nSubject: #762, \"Hibiscus-3\"\nAdult female, 42cm, 257g\n\nDonor: #650, \"Hibiscus\"\nAdult female, 42cm, 257g";

				// Token: 0x0400B4C4 RID: 46276
				public static LocString CONTAINER2 = "Hypothesis: Subjects cloned from Hibiscus will correctly operate a lever apparatus when introduced, demonstrating retention of original donor's conditioned memories.\n\nDonor subject #650, \"Hibiscus\", conditioned to pull a lever to the right for a reward (almonds). Conditioning took place over a period of two weeks.\n\nHibiscus quickly learned that pulling the lever to the left produced no results, and was reliably demonstrating the desired behavior by the end of the first week.\n\nTraining continued for one additional week to strengthen neural pathways and ensure the intended behavioral conditioning was committed to long term and muscle memory.\n\nCloning subject #762, \"Hibiscus-3\", was introduced to the lever apparatus to ascertain memory retention and recall.\n\nHibiscus-3 showed no signs of recognition and did not perform the desired behavior. Subject initially failed to interact with the apparatus on any level.\n\nOn second introduction, Hibiscus-3 pulled the lever to the left.\n\nConclusion: Printed subject retains no memory from donor.";
			}
		}

		// Token: 0x020020C7 RID: 8391
		public class A3_HUSBANDRYNOTES
		{
			// Token: 0x040093B3 RID: 37811
			public static LocString TITLE = "Husbandry Notes";

			// Token: 0x040093B4 RID: 37812
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x0200298D RID: 10637
			public class BODY
			{
				// Token: 0x0400B4C5 RID: 46277
				public static LocString CONTAINER1 = "<smallcaps>[Log Fragmentation Detected]\n[Voice Recognition Unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...The Hatch has been selected for development due to its naturally wide range of potential food sources.\n\nEnergy production is our primary goal, but augmentation to allow for the consumption of non-organic materials is a more attainable first step, and will have additional uses for waste disposal...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400B4C6 RID: 46278
				public static LocString CONTAINER2 = "[LOG BEGINS]\n\n...The Morb has been selected for development based on its ability to perform a multitude of the waste breakdown functions typical for a healthy ecosystem.\n\nDesign should focus on eliminating the disease risks posed by a fully matured Morb specimen...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400B4C7 RID: 46279
				public static LocString CONTAINER3 = "[LOG BEGINS]\n\n...The Puft may be suited for serving a sustainable decontamination role.\n\nPotential design must focus on the efficiency of these processes...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400B4C8 RID: 46280
				public static LocString CONTAINER4 = "[LOG BEGINS]\n\n...Wheezeworts are an ideal selection due to their low nutrient requirements and natural terraforming capabilities.\n\nDesign of these creatures should focus on enhancing their natural influence on ambient temperatures...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400B4C9 RID: 46281
				public static LocString CONTAINER5 = "[LOG BEGINS]\n\n...The preliminary Hatch gene splices were successful.\n\nThe prolific mucus excretions that are typical of the species are now producing hydrocarbons at an incredible pace.\n\nThe creature has essentially become a free source of burnable oil...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400B4CA RID: 46282
				public static LocString CONTAINER6 = "[LOG BEGINS]\n\n...Bioluminescence is always a novelty, but little time should be spent on perfecting these insects from here on out.\n\nThe project has more pressing concerns than light sources, particularly now that the low light vegetation issue has been solved...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400B4CB RID: 46283
				public static LocString CONTAINER7 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B363]</smallcaps>\n\n[LOG BEGINS]\n\nGeneticist: The primary concern raised by this project is the variability of environments that colonies may be forced to deal with. The creatures we send with the settlement party will not have the time to evolve and adapt to a new environment, yet each creature has been chosen to play a vital role in colony sustainability and is thus too precious to risk loss.\n\nGeneticist: It follows that each organism we design must be equipped with the tools to survive in as many volatile environments as we are capable of planning for. We should not rely on the Pod alone to replenish creature populations.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x020020C8 RID: 8392
		public class A6_MEMORYIMPLANTATION
		{
			// Token: 0x040093B5 RID: 37813
			public static LocString TITLE = "Memory Dysfunction Log";

			// Token: 0x040093B6 RID: 37814
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x0200298E RID: 10638
			public class BODY
			{
				// Token: 0x0400B4CC RID: 46284
				public static LocString CONTAINER1 = "[LOG BEGINS]\n\nTraditionally, cloning produces a subject that is genetically identical to the donor but develops independently, producing a being that is, in its own way, unique.\n\nThe pod, conversely, attempts to print an exact atomic copy. Theoretically all neural pathways should be intact and identical to the original subject.\n\nIt's fascinating, given this, that memories are not already inherent in our subjects; however, no cloned subjects as of yet have shown any signs of recognition when introduced to familiar stimuli, such as the donor subject's enclosure.\n\nRefer to Experiment 7D.\n\nRefer to Experiment 7F.";

				// Token: 0x0400B4CD RID: 46285
				public static LocString CONTAINER2 = "\nMemories <i>must</i> be embedded within the physical brainmaps of our subjects. The only question that remains is how to activate them. Hormones? Chemical supplements? Situational triggers?\n\nThe Director seems eager to move past this problem, and I am concerned at her willingness to bypass essential stages of the research development process.\n\nWe cannot move on to the fine polish of printing systems until the core processes have been perfected - which they have not.\n\nDr. Broussard, signing off.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x020020C9 RID: 8393
		public class B9_TELEPORTATION
		{
			// Token: 0x040093B7 RID: 37815
			public static LocString TITLE = "Memory Breakthrough";

			// Token: 0x040093B8 RID: 37816
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x0200298F RID: 10639
			public class BODY
			{
				// Token: 0x0400B4CE RID: 46286
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: A001]</smallcaps>\n\n[LOG BEGINS]\n\nDr. Techna's newest notes on Duplicant memories have revealed some interesting discoveries. It seems memories </i>can</i> be transferred to the cloned subject but it requires the host to be subjected to a machine that performs extremely detailed microanalysis. This in-depth dissection of the subject would produce the results we need but at the expense of destroying the host.\n\nOf course this is not ideal for our current situation. The time and energy it took to recruit Gravitas' highly trained staff would be wasted if we were to extirpate these people for the sake of experimentation. But perhaps we can use our Duplicants as experimental subjects until we perfect the process and look into finding volunteers for the future in order to obtain an ideal specimen. I will have to discuss this with Dr. Techna but I'm sure he would be enthusiastic about such an opportunity to continue his work.\n\nI am also very interested in the commercial opportunities this presents. Off the top of my head I can think of applications in genetics, AI development, and teleportation technology. This could be a significant financial windfall for the company.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x020020CA RID: 8394
		public class AUTOMATION
		{
			// Token: 0x040093B9 RID: 37817
			public static LocString TITLE = UI.FormatAsLink("Automation", "LOGIC");

			// Token: 0x040093BA RID: 37818
			public static LocString HEADER_1 = "Automation";

			// Token: 0x040093BB RID: 37819
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"Automation is a tool for controlling the operation of buildings based on what sensors in the colony are detecting.\n\nA ",
				BUILDINGS.PREFABS.CEILINGLIGHT.NAME,
				" could be configured to automatically turn on when a ",
				BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.NAME,
				" detects a Duplicant in the room.\n\nA ",
				BUILDINGS.PREFABS.LIQUIDPUMP.NAME,
				" might activate only when a ",
				BUILDINGS.PREFABS.LOGICELEMENTSENSORLIQUID.NAME,
				" detects water.\n\nA ",
				BUILDINGS.PREFABS.AIRCONDITIONER.NAME,
				" might activate only when the ",
				BUILDINGS.PREFABS.LOGICTEMPERATURESENSOR.NAME,
				" detects too much heat.\n\n"
			});

			// Token: 0x040093BC RID: 37820
			public static LocString HEADER_2 = "Automation Wires";

			// Token: 0x040093BD RID: 37821
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"In addition to an ",
				UI.FormatAsLink("electrical wire", "WIRE"),
				", most powered buildings can also have an ",
				BUILDINGS.PREFABS.LOGICWIRE.NAME,
				" connected to them. This wire can signal the building to turn on or off. If the other end of a ",
				BUILDINGS.PREFABS.LOGICWIRE.NAME,
				" is connected to a sensor, the building will turn on and off as the sensor outputs signals.\n\n"
			});

			// Token: 0x040093BE RID: 37822
			public static LocString HEADER_3 = "Signals";

			// Token: 0x040093BF RID: 37823
			public static LocString PARAGRAPH_3 = string.Concat(new string[]
			{
				"There are two signals that an ",
				BUILDINGS.PREFABS.LOGICWIRE.NAME,
				" can send: Green and Red. The green signal will usually cause buildings to turn on, and the red signal will usually cause buildings to turn off. Sensors can often be configured to send their green signal only under certain conditions. A ",
				BUILDINGS.PREFABS.LOGICTEMPERATURESENSOR.NAME,
				" could be configured to only send a green signal if detecting temperatures greater than a chosen value.\n\n"
			});

			// Token: 0x040093C0 RID: 37824
			public static LocString HEADER_4 = "Gates";

			// Token: 0x040093C1 RID: 37825
			public static LocString PARAGRAPH_4 = "The signals of sensor wires can be combined using special buildings called \"Gates\" in order to create complex activation conditions.\nThe " + BUILDINGS.PREFABS.LOGICGATEAND.NAME + " can have two automation wires connected to its input slots, and one connected to its output slots. It will send a \"Green\" signal to its output slot only if it is receiving a \"Green\" signal from both its input slots. This could be used to activate a building only when multiple sensors are detecting something.\n\n";
		}

		// Token: 0x020020CB RID: 8395
		public class DECORSYSTEM
		{
			// Token: 0x040093C2 RID: 37826
			public static LocString TITLE = UI.FormatAsLink("Decor", "DECOR");

			// Token: 0x040093C3 RID: 37827
			public static LocString HEADER_1 = "Decor";

			// Token: 0x040093C4 RID: 37828
			public static LocString PARAGRAPH_1 = "Low Decor can increase Duplicant " + UI.FormatAsLink("Stress", "STRESS") + ". Thankfully, pretty things tend to increase the Decor value of an area. Each Duplicant has a different idea of what is a high enough Decor value. If the average Decor that a Duplicant experiences in a cycle is below their expectations, they will suffer a stress penalty.\n\n";

			// Token: 0x040093C5 RID: 37829
			public static LocString HEADER_2 = "Calculating Decor";

			// Token: 0x040093C6 RID: 37830
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"Many things have an effect on the Decor value of a tile. A building's effect is expressed as a strength value and a radius. Often that effect is positive, but many buildings also lower the decor value of an area too. ",
				UI.FormatAsLink("Plants", "PLANTS"),
				", ",
				UI.FormatAsLink("Critters", "CREATURES"),
				", and ",
				UI.FormatAsLink("Furniture", "BUILDCATEGORYFURNITURE"),
				" often increase decor while industrial buildings, debris, and rot often decrease it. Duplicants experience the combined decor of all objects affecting a tile.\n\nThe ",
				CREATURES.SPECIES.PRICKLEGRASS.NAME,
				" has a decor value of ",
				string.Format("{0} and a radius of {1} tiles. ", PrickleGrassConfig.POSITIVE_DECOR_EFFECT.amount, PrickleGrassConfig.POSITIVE_DECOR_EFFECT.radius),
				"\nThe ",
				BUILDINGS.PREFABS.MICROBEMUSHER.NAME,
				" has a decor value of ",
				string.Format("{0} and a radius of {1} tiles. ", MicrobeMusherConfig.DECOR.amount, MicrobeMusherConfig.DECOR.radius),
				"\nThe result of placing a ",
				BUILDINGS.PREFABS.MICROBEMUSHER.NAME,
				" next to a ",
				CREATURES.SPECIES.PRICKLEGRASS.NAME,
				" would be a combined decor value of ",
				(MicrobeMusherConfig.DECOR.amount + PrickleGrassConfig.POSITIVE_DECOR_EFFECT.amount).ToString(),
				"."
			});
		}

		// Token: 0x020020CC RID: 8396
		public class EXOBASES
		{
			// Token: 0x040093C7 RID: 37831
			public static LocString TITLE = UI.FormatAsLink("Space Travel", "EXOBASES");

			// Token: 0x040093C8 RID: 37832
			public static LocString HEADER_1 = "Building Rockets";

			// Token: 0x040093C9 RID: 37833
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"Building a rocket first requires constructing a ",
				UI.FormatAsLink("Rocket Platform", "LAUNCHPAD"),
				" and adding modules from the menu. All rockets will require an engine, a nosecone and a Command Module piloted by a Duplicant possessing the ",
				UI.FormatAsLink("Rocket Piloting", "ROCKETPILOTING1"),
				" skill or higher. Note that the ",
				UI.FormatAsLink("Solo Spacefarer Nosecone", "HABITATMODULESMALL"),
				" functions as both a Command Module and a nosecone.\n\n"
			});

			// Token: 0x040093CA RID: 37834
			public static LocString HEADER_2 = "Space Travel";

			// Token: 0x040093CB RID: 37835
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"To scan space and see nearby intersteller destinations a ",
				UI.FormatAsLink("Telescope", "CLUSTERTELESCOPE"),
				" must first be built on the surface of a Planetoid. ",
				UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER"),
				" in orbit around a Planetoid, and ",
				UI.FormatAsLink("Cartographic Module", "SCANNERMODULE"),
				" attached to a rocket can also reveal places on a Starmap.\n\nAlways check engine fuel to determine if your rocket can reach its destination, keeping in mind rockets can only land on Plantoids with a ",
				UI.FormatAsLink("Rocket Platform", "LAUNCHPAD"),
				" on it although some modules like ",
				UI.FormatAsLink("Rover's Modules", "SCOUTMODULE"),
				" and ",
				UI.FormatAsLink("Trailblazer Modules", "PIONEERMODULE"),
				" can be sent to the surface of a Planetoid from a rocket in orbit.\n\n"
			});

			// Token: 0x040093CC RID: 37836
			public static LocString HEADER_3 = "Space Transport";

			// Token: 0x040093CD RID: 37837
			public static LocString PARAGRAPH_3 = string.Concat(new string[]
			{
				"Goods can be teleported between worlds with connected Supply Teleporters through ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				", ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				", and ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" conduits.\n\nPlanetoids not connected through Supply Teleporters can use rockets to transport goods, either by landing on a ",
				UI.FormatAsLink("Rocket Platform", "LAUNCHPAD"),
				" or a ",
				UI.FormatAsLink("Orbital Cargo Module", "ORBITALCARGOMODULE"),
				" deployed from a rocket in orbit. Additionally, the ",
				UI.FormatAsLink("Interplanetary Launcher", "RAILGUN"),
				" can send ",
				UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
				" full of goods through space but must be opened by a ",
				UI.FormatAsLink("Payload Opener", "RAILGUNPAYLOADOPENER"),
				". A ",
				UI.FormatAsLink("Targeting Beacon", "LANDINGBEACON"),
				" can guide payloads and orbital modules to land at a specific location on a Planetoid surface."
			});
		}

		// Token: 0x020020CD RID: 8397
		public class GENETICS
		{
			// Token: 0x040093CE RID: 37838
			public static LocString TITLE = UI.FormatAsLink("Genetics", "GENETICS");

			// Token: 0x040093CF RID: 37839
			public static LocString HEADER_1 = "Plant Mutations";

			// Token: 0x040093D0 RID: 37840
			public static LocString PARAGRAPH_1 = "Plants exposed to radiation sometimes drop mutated seeds when they are harvested. Each type of mutation has its own efficiencies and trade-offs.\n\nMutated seeds can be planted once they have been analyzed in the " + UI.FormatAsLink("Botanical Analyzer", "GENETICANALYSISSTATION") + ", but the resulting plants will produce no seeds of their own unless they are uprooted.\n\n";

			// Token: 0x040093D1 RID: 37841
			public static LocString HEADER_2 = "Cultivating Mutated Seeds";

			// Token: 0x040093D2 RID: 37842
			public static LocString PARAGRAPH_2 = "Once mutated seeds have been analyzed in the Botanical Analyzer, they are ready to be planted. Continued exposure to naturally occurring radiation or a " + UI.FormatAsLink("Radiation Lamp", "RADIATIONLIGHT") + " is necessary to prevent wilting.\n\n";
		}

		// Token: 0x020020CE RID: 8398
		public class HEALTH
		{
			// Token: 0x040093D3 RID: 37843
			public static LocString TITLE = UI.FormatAsLink("Health", "HEALTH");

			// Token: 0x040093D4 RID: 37844
			public static LocString HEADER_1 = "Health";

			// Token: 0x040093D5 RID: 37845
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"Duplicants can be physically damaged by some rare circumstances, such as extreme ",
				UI.FormatAsLink("Heat", "HEAT"),
				" or aggressive ",
				UI.FormatAsLink("Critters", "CREATURES"),
				". Damaged Duplicants will suffer greatly reduced athletic abilities, and are at risk of incapacitation if damaged too severely.\n\n"
			});

			// Token: 0x040093D6 RID: 37846
			public static LocString HEADER_2 = "Incapacitation and Death";

			// Token: 0x040093D7 RID: 37847
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"Incapacitated Duplicants cannot move or perform errands. They must be rescued by another Duplicant before their health drops to zero. If a Duplicant's health reaches zero they will die.\n\nHealth can be restored slowly over time and quickly through rest at the ",
				BUILDINGS.PREFABS.MEDICALCOT.NAME,
				".\n\n Duplicants are generally more vulnerable to ",
				UI.FormatAsLink("Disease", "DISEASE"),
				" than physical damage.\n\n"
			});
		}

		// Token: 0x020020CF RID: 8399
		public class HEAT
		{
			// Token: 0x040093D8 RID: 37848
			public static LocString TITLE = UI.FormatAsLink("Heat", "HEAT");

			// Token: 0x040093D9 RID: 37849
			public static LocString HEADER_1 = "Temperature";

			// Token: 0x040093DA RID: 37850
			public static LocString PARAGRAPH_1 = "Just about everything on the asteroid has a temperature. It's normal for temperature to rise and fall a bit, but extreme temperatures can cause all sorts of problems for a base. Buildings can stop functioning, crops can wilt, and things can even melt, boil, and freeze when they really ought not to.\n\n";

			// Token: 0x040093DB RID: 37851
			public static LocString HEADER_2 = "Wilting, Overheating, and Melting";

			// Token: 0x040093DC RID: 37852
			public static LocString PARAGRAPH_2 = "Most crops require their body temperatures to be within a certain range in order to grow. Values outside of this range are not fatal, but will pause growth. If a building's temperature exceeds its overheat temperature it will take damage and require repair.\nAt very extreme temperatures buildings may melt or boil away.\n\n";

			// Token: 0x040093DD RID: 37853
			public static LocString HEADER_3 = "Thermal Energy";

			// Token: 0x040093DE RID: 37854
			public static LocString PARAGRAPH_3 = "Temperature increase when the thermal energy of a substance increases. The value of temperature is equal to the total Thermal Energy divided by the Specific Heat Capacity of the substance. Because Specific Heat Capacity varies between substances so significantly, it is often the case a substance can have a higher temperature than another despite a lower overall thermal energy. This quality makes Water require nearly four times the amount of thermal energy to increase in temperature compared to Oxygen.\n\n";

			// Token: 0x040093DF RID: 37855
			public static LocString HEADER_4 = "Conduction and Insulation";

			// Token: 0x040093E0 RID: 37856
			public static LocString PARAGRAPH_4 = "Thermal energy can be transferred between Buildings, Creatures, World tiles, and other world entities through Conduction. Conduction occurs when two things of different Temperatures are touching. The rate of the energy transfer is the product of the averaged Conductivity values and Temperature difference. Thermal energy will flow slowly between substances with low conductivity values (insulators), and quickly between substances with high conductivity (conductors).\n\n";

			// Token: 0x040093E1 RID: 37857
			public static LocString HEADER_5 = "State Changes";

			// Token: 0x040093E2 RID: 37858
			public static LocString PARAGRAPH_5 = "Water ice melts into liquid water when its temperature rises above its melting point. Liquid water boils into steam when its temperature rises above its boiling point. Similar transitions in state occur for most elements, but each element has its own threshold temperatures. Sometimes the transitions are not reversible - crude oil boiled into sour gas will not condense back to crude oil when cooled. Instead, the substance might condense into a totally different element with a different utility. \n\n";
		}

		// Token: 0x020020D0 RID: 8400
		public class LIGHT
		{
			// Token: 0x040093E3 RID: 37859
			public static LocString TITLE = UI.FormatAsLink("Light", "LIGHT");

			// Token: 0x040093E4 RID: 37860
			public static LocString HEADER_1 = "Light";

			// Token: 0x040093E5 RID: 37861
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"Most of the asteroid is dark. Light sources such as the ",
				BUILDINGS.PREFABS.CEILINGLIGHT.NAME,
				" or ",
				CREATURES.SPECIES.LIGHTBUG.NAME,
				" improves Decor and gives Duplicants a boost to their productivity. Many plants are also sensitive to the amount of light they receive.\n\n"
			});

			// Token: 0x040093E6 RID: 37862
			public static LocString HEADER_2 = "Light Sources";

			// Token: 0x040093E7 RID: 37863
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"The ",
				BUILDINGS.PREFABS.FLOORLAMP.NAME,
				" and ",
				BUILDINGS.PREFABS.CEILINGLIGHT.NAME,
				" produce a decent amount of light when powered. The ",
				CREATURES.SPECIES.LIGHTBUG.NAME,
				" naturally emits a halo of light. Strong solar light is available on the surface during daytime.\n\n"
			});

			// Token: 0x040093E8 RID: 37864
			public static LocString HEADER_3 = "Measuring Light";

			// Token: 0x040093E9 RID: 37865
			public static LocString PARAGRAPH_3 = string.Concat(new string[]
			{
				"The amount of light on a cell is measured in Lux. Lux has a dramatic range - A simple ",
				BUILDINGS.PREFABS.CEILINGLIGHT.NAME,
				" produces ",
				1800.ToString(),
				" Lux, while the sun can produce values as high as ",
				80000.ToString(),
				" Lux. The ",
				BUILDINGS.PREFABS.SOLARPANEL.NAME,
				" generates power proportional to how many Lux it is exposed to.\n\n"
			});
		}

		// Token: 0x020020D1 RID: 8401
		public class MORALE
		{
			// Token: 0x040093EA RID: 37866
			public static LocString TITLE = UI.FormatAsLink("Morale", "MORALE");

			// Token: 0x040093EB RID: 37867
			public static LocString HEADER_1 = "Morale";

			// Token: 0x040093EC RID: 37868
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"Morale describes the relationship between a Duplicant's ",
				UI.FormatAsLink("Skills", "ROLES"),
				" and their Lifestyle. The more skills a Duplicant has, the higher their morale expectation will be. Duplicants with morale below their expectation will experience a ",
				UI.FormatAsLink("Stress", "STRESS"),
				" penalty. Comforts such as quality ",
				UI.FormatAsLink("Food", "FOOD"),
				", nice rooms, and recreation will increase morale.\n\n"
			});

			// Token: 0x040093ED RID: 37869
			public static LocString HEADER_2 = "Recreation";

			// Token: 0x040093EE RID: 37870
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"Recreation buildings such as the ",
				BUILDINGS.PREFABS.WATERCOOLER.NAME,
				" and ",
				BUILDINGS.PREFABS.ESPRESSOMACHINE.NAME,
				" improve a Duplicant's morale when used. Duplicants need downtime time in their schedules to use these buildings.\n\n"
			});

			// Token: 0x040093EF RID: 37871
			public static LocString HEADER_3 = "Overjoyed Responses";

			// Token: 0x040093F0 RID: 37872
			public static LocString PARAGRAPH_3 = "If a Duplicant has a very high Morale value, they will spontaneously display an Overjoyed Response. Each Duplicant has a different Overjoyed Behavior - but all overjoyed responses are good. Some will positively affect Building " + UI.FormatAsLink("Decor", "DECOR") + ", others will positively affect Duplicant morale or productivity.\n\n";
		}

		// Token: 0x020020D2 RID: 8402
		public class POWER
		{
			// Token: 0x040093F1 RID: 37873
			public static LocString TITLE = UI.FormatAsLink("Power", "POWER");

			// Token: 0x040093F2 RID: 37874
			public static LocString HEADER_1 = "Electricity";

			// Token: 0x040093F3 RID: 37875
			public static LocString PARAGRAPH_1 = "Electrical power is required to run many of the buildings in a base. Different buildings requires different amounts of power to run. Power can be transferred to buildings that require it using " + UI.FormatAsLink("Wires", "WIRE") + ".\n\n";

			// Token: 0x040093F4 RID: 37876
			public static LocString HEADER_2 = "Generators and Batteries";

			// Token: 0x040093F5 RID: 37877
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"Several buildings can generate power. Duplicants can run on the ",
				BUILDINGS.PREFABS.MANUALGENERATOR.NAME,
				" to generate clean power. Once generated, power can be consumed by buildings or stored in a ",
				BUILDINGS.PREFABS.BATTERY.NAME,
				" to prevent waste. Any generated power that is not consumed or stored will be wasted. Batteries and Generators tend to produce a significant amount of ",
				UI.FormatAsLink("Heat", "HEAT"),
				" while active.\n\nPower can also be stored in portable ",
				UI.FormatAsLink("Power Banks", "ELECTROBANK"),
				".\n\n"
			});

			// Token: 0x040093F6 RID: 37878
			public static LocString HEADER_3 = "Measuring Power";

			// Token: 0x040093F7 RID: 37879
			public static LocString PARAGRAPH_3 = "Power is measure in Joules when stored in a " + BUILDINGS.PREFABS.BATTERY.NAME + ". Power produced and consumed by buildings is measured in Watts, which are equal to Joules (consumed or produced) per second.\n\nA Battery that stored 5000 Joules could power a building that consumed 240 Watts for about 20 seconds. A generator which produces 480 Watts could power two buildings which consume 240 Watts for as long as it was running.\n\n";

			// Token: 0x040093F8 RID: 37880
			public static LocString HEADER_4 = "Overloading";

			// Token: 0x040093F9 RID: 37881
			public static LocString PARAGRAPH_4 = string.Concat(new string[]
			{
				"A network of ",
				UI.FormatAsLink("Wires", "WIRE"),
				" can be overloaded if it is consuming too many watts. If the wattage of a wire network exceeds its limits it may break and require repair.\n\n",
				UI.FormatAsLink("Standard wires", "WIRE"),
				" have a ",
				1000.ToString(),
				" Watt limit.\n\n"
			});
		}

		// Token: 0x020020D3 RID: 8403
		public class PRIORITY
		{
			// Token: 0x040093FA RID: 37882
			public static LocString TITLE = UI.FormatAsLink("Priorities", "PRIORITY");

			// Token: 0x040093FB RID: 37883
			public static LocString HEADER_1 = "Errand Priority";

			// Token: 0x040093FC RID: 37884
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"Duplicants prioritize their errands based on several factors. Some of these can be adjusted to affect errand choice, but some errands (such as seeking breathable ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				") are so important that they cannot be delayed. Errand priority can primarily be controlled by Errand Type prioritization, and then can be further fine-tuned by the ",
				UI.FormatAsTool("Priority Tool", global::Action.Prioritize),
				".\n\n"
			});

			// Token: 0x040093FD RID: 37885
			public static LocString HEADER_2 = "Errand Type Prioritization";

			// Token: 0x040093FE RID: 37886
			public static LocString PARAGRAPH_2 = "Each errand a Duplicant can perform falls into an Errand Category. These categories can be prioritized on a per-Duplicant basis in the " + UI.FormatAsManagementMenu("Priorities Screen") + ". Entire errand categories can also be prohibited to a Duplicant if they are meant to never perform errands of that variety. A common configuration is to assign errand type priority based on Duplicant attributes.\n\nFor example, Duplicants who are good at Research could be made to prioritize the Researching errand type. Duplicants with poor Athletics could be made to deprioritize the Supplying and Storing errand types.\n\n";

			// Token: 0x040093FF RID: 37887
			public static LocString HEADER_3 = "Priority Tool";

			// Token: 0x04009400 RID: 37888
			public static LocString PARAGRAPH_3 = "The priority of errands can often be modified using the " + UI.FormatAsTool("Priority tool", global::Action.Prioritize) + ". The values applied by this tool are always less influential than the Errand Type priorities described above. If two errands with equal Errand Type Priority are available to a Duplicant, they will choose the errand with a higher priority setting as applied by the tool.\n\n";
		}

		// Token: 0x020020D4 RID: 8404
		public class RADIATION
		{
			// Token: 0x04009401 RID: 37889
			public static LocString TITLE = UI.FormatAsLink("Radiation", "RADIATION");

			// Token: 0x04009402 RID: 37890
			public static LocString HEADER_1 = "Radiation";

			// Token: 0x04009403 RID: 37891
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"When transporting radioactive materials such as ",
				UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
				", care must be taken to avoid exposing outside objects to ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATIONSICKNESS"),
				".\n\nUsing proper transportation vessels, such as those which are lined with ",
				UI.FormatAsLink("Lead", "LEAD"),
				", is crucial to ensuring that Duplicants avoid ",
				UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
				"."
			});

			// Token: 0x04009404 RID: 37892
			public static LocString HEADER_2 = "Radiation Sickness";

			// Token: 0x04009405 RID: 37893
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"Duplicants who are exposed to ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATIONSICKNESS"),
				" will need to wear protection or they risk coming down with ",
				UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
				".\n\nSome Duplicants will have more of a natural resistance to radiation, but prolonged exposure will still increase their chances of becoming sick.\n\nConsuming ",
				UI.FormatAsLink("Rad Pills", "BASICRADPILL"),
				" or seafood such as ",
				UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
				" or ",
				UI.FormatAsLink("Waterweed", "SEALETTUCE"),
				" increases a Duplicant's radiation resistance, but will not cure a Duplicant's ",
				UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
				" once they have become infected.\n\nOn the other hand, exposure to radiation will kill ",
				UI.FormatAsLink("Food Poisoning", "FOODPOISONING"),
				", ",
				UI.FormatAsLink("Slimelung", "SLIMELUNG"),
				" and ",
				UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES"),
				" on surfaces (including on Duplicants).\n\n"
			});

			// Token: 0x04009406 RID: 37894
			public static LocString HEADER_3 = "Nuclear Energy";

			// Token: 0x04009407 RID: 37895
			public static LocString PARAGRAPH_3 = string.Concat(new string[]
			{
				"A ",
				UI.FormatAsLink("Research Reactor", "NUCLEARREACTOR"),
				" will require ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				" to run. Uranium can be enriched using a ",
				UI.FormatAsLink("Uranium Centrifuge", "URANIUMCENTRIFUGE"),
				".\n\nOnce supplied with ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				", a ",
				UI.FormatAsLink("Research Reactors", "NUCLEARREACTOR"),
				" will create an enormous amount of ",
				UI.FormatAsLink("Heat", "HEAT"),
				" which can then be placed under a source of ",
				UI.FormatAsLink("Water", "WATER"),
				" to produce ",
				UI.FormatAsLink("Steam", "STEAM"),
				"and connected to a ",
				UI.FormatAsLink("Steam Turbine", "STEAMTURBINE2"),
				" to produce a considerable source of ",
				UI.FormatAsLink("Power", "POWER"),
				"."
			});
		}

		// Token: 0x020020D5 RID: 8405
		public class RESEARCH
		{
			// Token: 0x04009408 RID: 37896
			public static LocString TITLE = UI.FormatAsLink("Research", "RESEARCH");

			// Token: 0x04009409 RID: 37897
			public static LocString HEADER_1 = "Research";

			// Token: 0x0400940A RID: 37898
			public static LocString PARAGRAPH_1 = "Doing research unlocks new types of buildings for the colony. Duplicants can perform research at the " + BUILDINGS.PREFABS.RESEARCHCENTER.NAME + ".\n\n";

			// Token: 0x0400940B RID: 37899
			public static LocString HEADER_2 = "Research Tasks";

			// Token: 0x0400940C RID: 37900
			public static LocString PARAGRAPH_2 = "A selected research task is completed once enough research points have been generated at the colony's research stations. Duplicants with high 'Science' attribute scores will generate research points faster than Duplicants with lower scores.\n\n";

			// Token: 0x0400940D RID: 37901
			public static LocString HEADER_3 = "Research Types";

			// Token: 0x0400940E RID: 37902
			public static LocString PARAGRAPH_3 = string.Concat(new string[]
			{
				"Advanced research tasks require special research stations to generate the proper kind of research points. These research stations often consume more advanced resources.\n\nUsing higher-level research stations also requires Duplicants to have learned higher level research ",
				UI.FormatAsLink("skills", "ROLES"),
				".\n\n",
				STRINGS.RESEARCH.TYPES.ALPHA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.RESEARCHCENTER.NAME,
				"\n",
				STRINGS.RESEARCH.TYPES.BETA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME,
				"\n",
				STRINGS.RESEARCH.TYPES.GAMMA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.COSMICRESEARCHCENTER.NAME,
				"\n\n"
			});
		}

		// Token: 0x020020D6 RID: 8406
		public class RESEARCHDLC1
		{
			// Token: 0x0400940F RID: 37903
			public static LocString TITLE = UI.FormatAsLink("Research", "RESEARCHDLC1");

			// Token: 0x04009410 RID: 37904
			public static LocString HEADER_1 = "Research";

			// Token: 0x04009411 RID: 37905
			public static LocString PARAGRAPH_1 = "Doing research unlocks new types of buildings for the colony. Duplicants can perform research at the " + BUILDINGS.PREFABS.RESEARCHCENTER.NAME + ".\n\n";

			// Token: 0x04009412 RID: 37906
			public static LocString HEADER_2 = "Research Tasks";

			// Token: 0x04009413 RID: 37907
			public static LocString PARAGRAPH_2 = "A selected research task is completed once enough research points have been generated at the colonies research stations. Duplicants with high 'Science' attribute scores will generate research points faster than Duplicants with lower scores.\n\n";

			// Token: 0x04009414 RID: 37908
			public static LocString HEADER_3 = "Research Types";

			// Token: 0x04009415 RID: 37909
			public static LocString PARAGRAPH_3 = string.Concat(new string[]
			{
				"Advanced research tasks require special research stations to generate the proper kind of research points. These research stations often consume more advanced resources.\n\nUsing higher level research stations also requires Duplicants to have learned higher level research ",
				UI.FormatAsLink("skills", "ROLES"),
				".\n\n",
				STRINGS.RESEARCH.TYPES.ALPHA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.RESEARCHCENTER.NAME,
				"\n",
				STRINGS.RESEARCH.TYPES.BETA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME,
				"\n",
				STRINGS.RESEARCH.TYPES.GAMMA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.ORBITALRESEARCHCENTER.NAME,
				"\n",
				STRINGS.RESEARCH.TYPES.DELTA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.NUCLEARRESEARCHCENTER.NAME,
				"\n\n"
			});
		}

		// Token: 0x020020D7 RID: 8407
		public class STRESS
		{
			// Token: 0x04009416 RID: 37910
			public static LocString TITLE = UI.FormatAsLink("Stress", "STRESS");

			// Token: 0x04009417 RID: 37911
			public static LocString HEADER_1 = "Stress";

			// Token: 0x04009418 RID: 37912
			public static LocString PARAGRAPH_1 = "A Duplicant's experiences in the colony affect their stress level. Stress increases when they have negative experiences or unmet expectations. Stress decreases with time if " + UI.FormatAsLink("Morale", "MORALE") + " is satisfied. Duplicant behavior starts to change for the worse when stress levels get too high.\n\n";

			// Token: 0x04009419 RID: 37913
			public static LocString HEADER_2 = "Stress Responses";

			// Token: 0x0400941A RID: 37914
			public static LocString PARAGRAPH_2 = "If a Duplicant has very high stress values they will experience a Stress Response episode. Each Duplicant has a different Stress Behavior - but all stress responses are bad. After the stress behavior episode is done, the Duplicants stress will reset to a lower value. Though, if the factors causing the Duplicant's high stress are not corrected they are bound to have another stress response episode.\n\n";
		}
	}
}
