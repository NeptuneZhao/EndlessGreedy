using System;
using System.Collections.Generic;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02000EF5 RID: 3829
	public class DUPLICANTSTATS
	{
		// Token: 0x06007717 RID: 30487 RVA: 0x002F18E8 File Offset: 0x002EFAE8
		public static DUPLICANTSTATS.TraitVal GetTraitVal(string id)
		{
			foreach (DUPLICANTSTATS.TraitVal traitVal in DUPLICANTSTATS.SPECIALTRAITS)
			{
				if (id == traitVal.id)
				{
					return traitVal;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal2 in DUPLICANTSTATS.GOODTRAITS)
			{
				if (id == traitVal2.id)
				{
					return traitVal2;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal3 in DUPLICANTSTATS.BADTRAITS)
			{
				if (id == traitVal3.id)
				{
					return traitVal3;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal4 in DUPLICANTSTATS.CONGENITALTRAITS)
			{
				if (id == traitVal4.id)
				{
					return traitVal4;
				}
			}
			DebugUtil.Assert(true, "Could not find TraitVal with ID: " + id);
			return DUPLICANTSTATS.INVALID_TRAIT_VAL;
		}

		// Token: 0x06007718 RID: 30488 RVA: 0x002F1A50 File Offset: 0x002EFC50
		public static DUPLICANTSTATS GetStatsFor(GameObject gameObject)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null)
			{
				return DUPLICANTSTATS.GetStatsFor(component);
			}
			return null;
		}

		// Token: 0x06007719 RID: 30489 RVA: 0x002F1A78 File Offset: 0x002EFC78
		public static DUPLICANTSTATS GetStatsFor(KPrefabID prefabID)
		{
			if (!prefabID.HasTag(GameTags.BaseMinion))
			{
				return null;
			}
			foreach (Tag tag in GameTags.Minions.Models.AllModels)
			{
				if (prefabID.HasTag(tag))
				{
					return DUPLICANTSTATS.GetStatsFor(tag);
				}
			}
			return null;
		}

		// Token: 0x0600771A RID: 30490 RVA: 0x002F1AC1 File Offset: 0x002EFCC1
		public static DUPLICANTSTATS GetStatsFor(Tag type)
		{
			if (DUPLICANTSTATS.DUPLICANT_TYPES.ContainsKey(type))
			{
				return DUPLICANTSTATS.DUPLICANT_TYPES[type];
			}
			return null;
		}

		// Token: 0x0400573C RID: 22332
		public const float RANCHING_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.1f;

		// Token: 0x0400573D RID: 22333
		public const float FARMING_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.1f;

		// Token: 0x0400573E RID: 22334
		public const float POWER_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.025f;

		// Token: 0x0400573F RID: 22335
		public const float RANCHING_CAPTURABLE_MULTIPLIER_BONUS_PER_POINT = 0.05f;

		// Token: 0x04005740 RID: 22336
		public const float STANDARD_STRESS_PENALTY = 0.016666668f;

		// Token: 0x04005741 RID: 22337
		public const float STANDARD_STRESS_BONUS = -0.033333335f;

		// Token: 0x04005742 RID: 22338
		public const float STRESS_BELOW_EXPECTATIONS_FOOD = 0.25f;

		// Token: 0x04005743 RID: 22339
		public const float STRESS_ABOVE_EXPECTATIONS_FOOD = -0.5f;

		// Token: 0x04005744 RID: 22340
		public const float STANDARD_STRESS_PENALTY_SECOND = 0.25f;

		// Token: 0x04005745 RID: 22341
		public const float STANDARD_STRESS_BONUS_SECOND = -0.5f;

		// Token: 0x04005746 RID: 22342
		public const float TRAVEL_TIME_WARNING_THRESHOLD = 0.4f;

		// Token: 0x04005747 RID: 22343
		public static readonly string[] ALL_ATTRIBUTES = new string[]
		{
			"Strength",
			"Caring",
			"Construction",
			"Digging",
			"Machinery",
			"Learning",
			"Cooking",
			"Botanist",
			"Art",
			"Ranching",
			"Athletics",
			"SpaceNavigation"
		};

		// Token: 0x04005748 RID: 22344
		public static readonly string[] DISTRIBUTED_ATTRIBUTES = new string[]
		{
			"Strength",
			"Caring",
			"Construction",
			"Digging",
			"Machinery",
			"Learning",
			"Cooking",
			"Botanist",
			"Art",
			"Ranching"
		};

		// Token: 0x04005749 RID: 22345
		public static readonly string[] ROLLED_ATTRIBUTES = new string[]
		{
			"Athletics"
		};

		// Token: 0x0400574A RID: 22346
		public static readonly int[] APTITUDE_ATTRIBUTE_BONUSES = new int[]
		{
			7,
			3,
			1
		};

		// Token: 0x0400574B RID: 22347
		public static int ROLLED_ATTRIBUTE_MAX = 5;

		// Token: 0x0400574C RID: 22348
		public static float ROLLED_ATTRIBUTE_POWER = 4f;

		// Token: 0x0400574D RID: 22349
		public static Dictionary<string, List<string>> ARCHETYPE_TRAIT_EXCLUSIONS = new Dictionary<string, List<string>>
		{
			{
				"Mining",
				new List<string>
				{
					"Anemic",
					"DiggingDown",
					"Narcolepsy"
				}
			},
			{
				"Building",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"ConstructionDown",
					"DiggingDown",
					"Narcolepsy"
				}
			},
			{
				"Farming",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"BotanistDown",
					"RanchingDown",
					"Narcolepsy"
				}
			},
			{
				"Ranching",
				new List<string>
				{
					"RanchingDown",
					"BotanistDown",
					"Narcolepsy"
				}
			},
			{
				"Cooking",
				new List<string>
				{
					"NoodleArms",
					"CookingDown"
				}
			},
			{
				"Art",
				new List<string>
				{
					"ArtDown",
					"DecorDown"
				}
			},
			{
				"Research",
				new List<string>
				{
					"SlowLearner"
				}
			},
			{
				"Suits",
				new List<string>
				{
					"Anemic",
					"NoodleArms"
				}
			},
			{
				"Hauling",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"Narcolepsy"
				}
			},
			{
				"Technicals",
				new List<string>
				{
					"MachineryDown"
				}
			},
			{
				"MedicalAid",
				new List<string>
				{
					"CaringDown",
					"WeakImmuneSystem"
				}
			},
			{
				"Basekeeping",
				new List<string>
				{
					"Anemic",
					"NoodleArms"
				}
			},
			{
				"Rocketry",
				new List<string>()
			}
		};

		// Token: 0x0400574E RID: 22350
		public static int RARITY_LEGENDARY = 5;

		// Token: 0x0400574F RID: 22351
		public static int RARITY_EPIC = 4;

		// Token: 0x04005750 RID: 22352
		public static int RARITY_RARE = 3;

		// Token: 0x04005751 RID: 22353
		public static int RARITY_UNCOMMON = 2;

		// Token: 0x04005752 RID: 22354
		public static int RARITY_COMMON = 1;

		// Token: 0x04005753 RID: 22355
		public static int NO_STATPOINT_BONUS = 0;

		// Token: 0x04005754 RID: 22356
		public static int TINY_STATPOINT_BONUS = 1;

		// Token: 0x04005755 RID: 22357
		public static int SMALL_STATPOINT_BONUS = 2;

		// Token: 0x04005756 RID: 22358
		public static int MEDIUM_STATPOINT_BONUS = 3;

		// Token: 0x04005757 RID: 22359
		public static int LARGE_STATPOINT_BONUS = 4;

		// Token: 0x04005758 RID: 22360
		public static int HUGE_STATPOINT_BONUS = 5;

		// Token: 0x04005759 RID: 22361
		public static int COMMON = 1;

		// Token: 0x0400575A RID: 22362
		public static int UNCOMMON = 2;

		// Token: 0x0400575B RID: 22363
		public static int RARE = 3;

		// Token: 0x0400575C RID: 22364
		public static int EPIC = 4;

		// Token: 0x0400575D RID: 22365
		public static int LEGENDARY = 5;

		// Token: 0x0400575E RID: 22366
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(1, 1);

		// Token: 0x0400575F RID: 22367
		public static global::Tuple<int, int> TRAITS_TWO_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(2, 1);

		// Token: 0x04005760 RID: 22368
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_TWO_NEGATIVE = new global::Tuple<int, int>(1, 2);

		// Token: 0x04005761 RID: 22369
		public static global::Tuple<int, int> TRAITS_TWO_POSITIVE_TWO_NEGATIVE = new global::Tuple<int, int>(2, 2);

		// Token: 0x04005762 RID: 22370
		public static global::Tuple<int, int> TRAITS_THREE_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(3, 1);

		// Token: 0x04005763 RID: 22371
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_THREE_NEGATIVE = new global::Tuple<int, int>(1, 3);

		// Token: 0x04005764 RID: 22372
		public static int MIN_STAT_POINTS = 0;

		// Token: 0x04005765 RID: 22373
		public static int MAX_STAT_POINTS = 0;

		// Token: 0x04005766 RID: 22374
		public static int MAX_TRAITS = 4;

		// Token: 0x04005767 RID: 22375
		public static int APTITUDE_BONUS = 1;

		// Token: 0x04005768 RID: 22376
		public static List<int> RARITY_DECK = new List<int>
		{
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_EPIC,
			DUPLICANTSTATS.RARITY_EPIC,
			DUPLICANTSTATS.RARITY_LEGENDARY
		};

		// Token: 0x04005769 RID: 22377
		public static List<int> rarityDeckActive = new List<int>(DUPLICANTSTATS.RARITY_DECK);

		// Token: 0x0400576A RID: 22378
		public static List<global::Tuple<int, int>> POD_TRAIT_CONFIGURATIONS_DECK = new List<global::Tuple<int, int>>
		{
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_THREE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_THREE_NEGATIVE
		};

		// Token: 0x0400576B RID: 22379
		public static List<global::Tuple<int, int>> podTraitConfigurationsActive = new List<global::Tuple<int, int>>(DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK);

		// Token: 0x0400576C RID: 22380
		public static readonly List<string> CONTRACTEDTRAITS_HEALING = new List<string>
		{
			"IrritableBowel",
			"Aggressive",
			"SlowLearner",
			"WeakImmuneSystem",
			"Snorer",
			"CantDig"
		};

		// Token: 0x0400576D RID: 22381
		public static readonly List<DUPLICANTSTATS.TraitVal> CONGENITALTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "None"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Joshua",
				mutuallyExclusiveTraits = new List<string>
				{
					"ScaredyCat",
					"Aggressive"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Ellie",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				mutuallyExclusiveTraits = new List<string>
				{
					"InteriorDecorator",
					"MouthBreather",
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Stinky",
				mutuallyExclusiveTraits = new List<string>
				{
					"Flatulence",
					"InteriorDecorator"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Liam",
				mutuallyExclusiveTraits = new List<string>
				{
					"Flatulence",
					"InteriorDecorator"
				}
			}
		};

		// Token: 0x0400576E RID: 22382
		public static readonly DUPLICANTSTATS.TraitVal INVALID_TRAIT_VAL = new DUPLICANTSTATS.TraitVal
		{
			id = "INVALID"
		};

		// Token: 0x0400576F RID: 22383
		public static readonly List<DUPLICANTSTATS.TraitVal> BADTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantResearch",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Research"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantDig",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Mining"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantCook",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Cooking"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantBuild",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Building"
				},
				mutuallyExclusiveTraits = new List<string>
				{
					"GrantSkill_Engineering1"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Hemophobia",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"MedicalAid"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ScaredyCat",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Mining"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ConstructionDown",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"ConstructionUp",
					"CantBuild"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RanchingDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"RanchingUp"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CaringDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BotanistDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ArtDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CookingDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Foodie",
					"CantCook"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MachineryDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DiggingDown",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"MoleHands",
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SlowLearner",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"FastLearner",
					"CantResearch"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NoodleArms",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DecorDown",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Anemic",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Flatulence",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "IrritableBowel",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Snorer",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MouthBreather",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SmallBladder",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CalorieBurner",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "WeakImmuneSystem",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Allergies",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NightLight",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Narcolepsy",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			}
		};

		// Token: 0x04005770 RID: 22384
		public static readonly List<DUPLICANTSTATS.TraitVal> STRESSTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Aggressive",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StressVomiter",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "UglyCrier",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BingeEater",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Banshee",
				dlcId = ""
			}
		};

		// Token: 0x04005771 RID: 22385
		public static readonly List<DUPLICANTSTATS.TraitVal> JOYTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "BalloonArtist",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SparkleStreaker",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StickerBomber",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SuperProductive",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "HappySinger",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DataRainer",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RoboDancer",
				dlcId = "DLC3_ID"
			}
		};

		// Token: 0x04005772 RID: 22386
		public static readonly List<DUPLICANTSTATS.TraitVal> GENESHUFFLERTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Regeneration",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DeeperDiversLungs",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SunnyDisposition",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RockCrusher",
				dlcId = ""
			}
		};

		// Token: 0x04005773 RID: 22387
		public static readonly List<DUPLICANTSTATS.TraitVal> BIONICTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBaseline",
				dlcId = "DLC3_ID"
			}
		};

		// Token: 0x04005774 RID: 22388
		public static readonly List<DUPLICANTSTATS.TraitVal> BIONICUPGRADETRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostDigging",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostBuilding",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostCooking",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostArt",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostFarming",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostRanching",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostMedicine",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostExplorer",
				dlcId = "DLC3_ID"
			}
		};

		// Token: 0x04005775 RID: 22389
		public static readonly List<DUPLICANTSTATS.TraitVal> SPECIALTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "AncientKnowledge",
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "EXPANSION1_ID",
				doNotGenerateTrait = true,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantResearch",
					"CantBuild",
					"CantCook",
					"CantDig",
					"Hemophobia",
					"ScaredyCat",
					"Anemic",
					"SlowLearner",
					"NoodleArms",
					"ConstructionDown",
					"RanchingDown",
					"DiggingDown",
					"MachineryDown",
					"CookingDown",
					"ArtDown",
					"CaringDown",
					"BotanistDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Chatty",
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "",
				doNotGenerateTrait = true
			}
		};

		// Token: 0x04005776 RID: 22390
		public static readonly List<DUPLICANTSTATS.TraitVal> GOODTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Twinkletoes",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Anemic"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StrongArm",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"NoodleArms"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Greasemonkey",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"MachineryDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DiversLung",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"MouthBreather"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "IronGut",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StrongImmuneSystem",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"WeakImmuneSystem"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "EarlyBird",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"NightOwl"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NightOwl",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"EarlyBird"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Meteorphile",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MoleHands",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig",
					"DiggingDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "FastLearner",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"SlowLearner",
					"CantResearch"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "InteriorDecorator",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured",
					"ArtDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Uncultured",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"InteriorDecorator"
				},
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Art"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SimpleTastes",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Foodie"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Foodie",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"SimpleTastes",
					"CantCook",
					"CookingDown"
				},
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Cooking"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BedsideManner",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia",
					"CaringDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DecorUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"DecorDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Thriver",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GreenThumb",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"BotanistDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ConstructionUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"ConstructionDown",
					"CantBuild"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RanchingUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"RanchingDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Loner",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "EXPANSION1_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StarryEyed",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "EXPANSION1_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GlowStick",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "EXPANSION1_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RadiationEater",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "EXPANSION1_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "FrostProof",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "DLC2_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining3",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Farming2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Ranching1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Cooking1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantCook"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting3",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Suits1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Technicals2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Engineering1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Basekeeping2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Anemic"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Medicine2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia"
				}
			}
		};

		// Token: 0x04005777 RID: 22391
		public static readonly List<DUPLICANTSTATS.TraitVal> NEEDTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Claustrophobic",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "PrefersWarmer",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"PrefersColder"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "PrefersColder",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"PrefersWarmer"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SensitiveFeet",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Fashionable",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Climacophobic",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SolitarySleeper",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			}
		};

		// Token: 0x04005778 RID: 22392
		public static DUPLICANTSTATS STANDARD = new DUPLICANTSTATS();

		// Token: 0x04005779 RID: 22393
		public static DUPLICANTSTATS BIONICS = new DUPLICANTSTATS
		{
			BaseStats = new DUPLICANTSTATS.BASESTATS
			{
				NO_OXYGEN_THRESHOLD = 0.5f,
				MAX_CALORIES = 0f
			}
		};

		// Token: 0x0400577A RID: 22394
		private static readonly Dictionary<Tag, DUPLICANTSTATS> DUPLICANT_TYPES = new Dictionary<Tag, DUPLICANTSTATS>
		{
			{
				GameTags.Minions.Models.Standard,
				DUPLICANTSTATS.STANDARD
			},
			{
				GameTags.Minions.Models.Bionic,
				DUPLICANTSTATS.BIONICS
			}
		};

		// Token: 0x0400577B RID: 22395
		public DUPLICANTSTATS.BASESTATS BaseStats = new DUPLICANTSTATS.BASESTATS();

		// Token: 0x0400577C RID: 22396
		public DUPLICANTSTATS.TEMPERATURE Temperature = new DUPLICANTSTATS.TEMPERATURE();

		// Token: 0x0400577D RID: 22397
		public DUPLICANTSTATS.BREATH Breath = new DUPLICANTSTATS.BREATH();

		// Token: 0x0400577E RID: 22398
		public DUPLICANTSTATS.LIGHT Light = new DUPLICANTSTATS.LIGHT();

		// Token: 0x0400577F RID: 22399
		public DUPLICANTSTATS.COMBAT Combat = new DUPLICANTSTATS.COMBAT();

		// Token: 0x04005780 RID: 22400
		public DUPLICANTSTATS.SECRETIONS Secretions = new DUPLICANTSTATS.SECRETIONS();

		// Token: 0x02001FCB RID: 8139
		public static class RADIATION_DIFFICULTY_MODIFIERS
		{
			// Token: 0x04009028 RID: 36904
			public static float HARDEST = 0.33f;

			// Token: 0x04009029 RID: 36905
			public static float HARDER = 0.66f;

			// Token: 0x0400902A RID: 36906
			public static float DEFAULT = 1f;

			// Token: 0x0400902B RID: 36907
			public static float EASIER = 2f;

			// Token: 0x0400902C RID: 36908
			public static float EASIEST = 100f;
		}

		// Token: 0x02001FCC RID: 8140
		public static class RADIATION_EXPOSURE_LEVELS
		{
			// Token: 0x0400902D RID: 36909
			public const float LOW = 100f;

			// Token: 0x0400902E RID: 36910
			public const float MODERATE = 300f;

			// Token: 0x0400902F RID: 36911
			public const float HIGH = 600f;

			// Token: 0x04009030 RID: 36912
			public const float DEADLY = 900f;
		}

		// Token: 0x02001FCD RID: 8141
		public static class MOVEMENT_MODIFIERS
		{
			// Token: 0x04009031 RID: 36913
			public static float NEUTRAL = 1f;

			// Token: 0x04009032 RID: 36914
			public static float BONUS_1 = 1.1f;

			// Token: 0x04009033 RID: 36915
			public static float BONUS_2 = 1.25f;

			// Token: 0x04009034 RID: 36916
			public static float BONUS_3 = 1.5f;

			// Token: 0x04009035 RID: 36917
			public static float BONUS_4 = 1.75f;

			// Token: 0x04009036 RID: 36918
			public static float PENALTY_1 = 0.9f;

			// Token: 0x04009037 RID: 36919
			public static float PENALTY_2 = 0.75f;

			// Token: 0x04009038 RID: 36920
			public static float PENALTY_3 = 0.5f;

			// Token: 0x04009039 RID: 36921
			public static float PENALTY_4 = 0.25f;
		}

		// Token: 0x02001FCE RID: 8142
		public static class QOL_STRESS
		{
			// Token: 0x0400903A RID: 36922
			public const float ABOVE_EXPECTATIONS = -0.016666668f;

			// Token: 0x0400903B RID: 36923
			public const float AT_EXPECTATIONS = -0.008333334f;

			// Token: 0x0400903C RID: 36924
			public const float MIN_STRESS = -0.033333335f;

			// Token: 0x02002676 RID: 9846
			public static class BELOW_EXPECTATIONS
			{
				// Token: 0x0400AACF RID: 43727
				public const float EASY = 0.0033333334f;

				// Token: 0x0400AAD0 RID: 43728
				public const float NEUTRAL = 0.004166667f;

				// Token: 0x0400AAD1 RID: 43729
				public const float HARD = 0.008333334f;

				// Token: 0x0400AAD2 RID: 43730
				public const float VERYHARD = 0.016666668f;
			}

			// Token: 0x02002677 RID: 9847
			public static class MAX_STRESS
			{
				// Token: 0x0400AAD3 RID: 43731
				public const float EASY = 0.016666668f;

				// Token: 0x0400AAD4 RID: 43732
				public const float NEUTRAL = 0.041666668f;

				// Token: 0x0400AAD5 RID: 43733
				public const float HARD = 0.05f;

				// Token: 0x0400AAD6 RID: 43734
				public const float VERYHARD = 0.083333336f;
			}
		}

		// Token: 0x02001FCF RID: 8143
		public static class CLOTHING
		{
			// Token: 0x02002678 RID: 9848
			public class DECOR_MODIFICATION
			{
				// Token: 0x0400AAD7 RID: 43735
				public const int NEGATIVE_SIGNIFICANT = -30;

				// Token: 0x0400AAD8 RID: 43736
				public const int NEGATIVE_MILD = -10;

				// Token: 0x0400AAD9 RID: 43737
				public const int BASIC = -5;

				// Token: 0x0400AADA RID: 43738
				public const int POSITIVE_MILD = 10;

				// Token: 0x0400AADB RID: 43739
				public const int POSITIVE_SIGNIFICANT = 30;

				// Token: 0x0400AADC RID: 43740
				public const int POSITIVE_MAJOR = 40;
			}

			// Token: 0x02002679 RID: 9849
			public class CONDUCTIVITY_BARRIER_MODIFICATION
			{
				// Token: 0x0400AADD RID: 43741
				public const float THIN = 0.0005f;

				// Token: 0x0400AADE RID: 43742
				public const float BASIC = 0.0025f;

				// Token: 0x0400AADF RID: 43743
				public const float THICK = 0.008f;
			}

			// Token: 0x0200267A RID: 9850
			public class SWEAT_EFFICIENCY_MULTIPLIER
			{
				// Token: 0x0400AAE0 RID: 43744
				public const float DIMINISH_SIGNIFICANT = -2.5f;

				// Token: 0x0400AAE1 RID: 43745
				public const float DIMINISH_MILD = -1.25f;

				// Token: 0x0400AAE2 RID: 43746
				public const float NEUTRAL = 0f;

				// Token: 0x0400AAE3 RID: 43747
				public const float IMPROVE = 2f;
			}
		}

		// Token: 0x02001FD0 RID: 8144
		public static class NOISE
		{
			// Token: 0x0400903D RID: 36925
			public const int THRESHOLD_PEACEFUL = 0;

			// Token: 0x0400903E RID: 36926
			public const int THRESHOLD_QUIET = 36;

			// Token: 0x0400903F RID: 36927
			public const int THRESHOLD_TOSS_AND_TURN = 45;

			// Token: 0x04009040 RID: 36928
			public const int THRESHOLD_WAKE_UP = 60;

			// Token: 0x04009041 RID: 36929
			public const int THRESHOLD_MINOR_REACTION = 80;

			// Token: 0x04009042 RID: 36930
			public const int THRESHOLD_MAJOR_REACTION = 106;

			// Token: 0x04009043 RID: 36931
			public const int THRESHOLD_EXTREME_REACTION = 125;
		}

		// Token: 0x02001FD1 RID: 8145
		public static class ROOM
		{
			// Token: 0x04009044 RID: 36932
			public const float LABORATORY_RESEARCH_EFFICIENCY_BONUS = 0.1f;
		}

		// Token: 0x02001FD2 RID: 8146
		public class DISTRIBUTIONS
		{
			// Token: 0x0600AFF4 RID: 45044 RVA: 0x003B2823 File Offset: 0x003B0A23
			public static int[] GetRandomDistribution()
			{
				return DUPLICANTSTATS.DISTRIBUTIONS.TYPES[UnityEngine.Random.Range(0, DUPLICANTSTATS.DISTRIBUTIONS.TYPES.Count)];
			}

			// Token: 0x04009045 RID: 36933
			public static readonly List<int[]> TYPES = new List<int[]>
			{
				new int[]
				{
					5,
					4,
					4,
					3,
					3,
					2,
					1
				},
				new int[]
				{
					5,
					3,
					2,
					1
				},
				new int[]
				{
					5,
					2,
					2,
					1
				},
				new int[]
				{
					5,
					1
				},
				new int[]
				{
					5,
					3,
					1
				},
				new int[]
				{
					3,
					3,
					3,
					3,
					1
				},
				new int[]
				{
					4
				},
				new int[]
				{
					3
				},
				new int[]
				{
					2
				},
				new int[]
				{
					1
				}
			};
		}

		// Token: 0x02001FD3 RID: 8147
		public struct TraitVal
		{
			// Token: 0x04009046 RID: 36934
			public string id;

			// Token: 0x04009047 RID: 36935
			public int statBonus;

			// Token: 0x04009048 RID: 36936
			public int impact;

			// Token: 0x04009049 RID: 36937
			public int rarity;

			// Token: 0x0400904A RID: 36938
			public string dlcId;

			// Token: 0x0400904B RID: 36939
			public List<string> mutuallyExclusiveTraits;

			// Token: 0x0400904C RID: 36940
			public List<HashedString> mutuallyExclusiveAptitudes;

			// Token: 0x0400904D RID: 36941
			public bool doNotGenerateTrait;
		}

		// Token: 0x02001FD4 RID: 8148
		public class ATTRIBUTE_LEVELING
		{
			// Token: 0x0400904E RID: 36942
			public static int MAX_GAINED_ATTRIBUTE_LEVEL = 20;

			// Token: 0x0400904F RID: 36943
			public static int TARGET_MAX_LEVEL_CYCLE = 400;

			// Token: 0x04009050 RID: 36944
			public static float EXPERIENCE_LEVEL_POWER = 1.7f;

			// Token: 0x04009051 RID: 36945
			public static float FULL_EXPERIENCE = 1f;

			// Token: 0x04009052 RID: 36946
			public static float ALL_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.8f;

			// Token: 0x04009053 RID: 36947
			public static float MOST_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.5f;

			// Token: 0x04009054 RID: 36948
			public static float PART_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.25f;

			// Token: 0x04009055 RID: 36949
			public static float BARELY_EVER_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.1f;
		}

		// Token: 0x02001FD5 RID: 8149
		public class BASESTATS
		{
			// Token: 0x17000BFE RID: 3070
			// (get) Token: 0x0600AFF9 RID: 45049 RVA: 0x003B29A2 File Offset: 0x003B0BA2
			public float CALORIES_BURNED_PER_SECOND
			{
				get
				{
					return this.CALORIES_BURNED_PER_CYCLE / 600f;
				}
			}

			// Token: 0x17000BFF RID: 3071
			// (get) Token: 0x0600AFFA RID: 45050 RVA: 0x003B29B0 File Offset: 0x003B0BB0
			public float HUNGRY_THRESHOLD
			{
				get
				{
					return this.SATISFIED_THRESHOLD - -this.CALORIES_BURNED_PER_CYCLE * 0.5f / this.MAX_CALORIES;
				}
			}

			// Token: 0x17000C00 RID: 3072
			// (get) Token: 0x0600AFFB RID: 45051 RVA: 0x003B29CD File Offset: 0x003B0BCD
			public float STARVING_THRESHOLD
			{
				get
				{
					return -this.CALORIES_BURNED_PER_CYCLE / this.MAX_CALORIES;
				}
			}

			// Token: 0x17000C01 RID: 3073
			// (get) Token: 0x0600AFFC RID: 45052 RVA: 0x003B29DD File Offset: 0x003B0BDD
			public float DUPLICANT_COOLING_KILOWATTS
			{
				get
				{
					return this.COOLING_EFFICIENCY * -this.CALORIES_BURNED_PER_SECOND * 0.001f * this.KCAL2JOULES / 1000f;
				}
			}

			// Token: 0x17000C02 RID: 3074
			// (get) Token: 0x0600AFFD RID: 45053 RVA: 0x003B2A00 File Offset: 0x003B0C00
			public float DUPLICANT_WARMING_KILOWATTS
			{
				get
				{
					return this.WARMING_EFFICIENCY * -this.CALORIES_BURNED_PER_SECOND * 0.001f * this.KCAL2JOULES / 1000f;
				}
			}

			// Token: 0x17000C03 RID: 3075
			// (get) Token: 0x0600AFFE RID: 45054 RVA: 0x003B2A23 File Offset: 0x003B0C23
			public float DUPLICANT_BASE_GENERATION_KILOWATTS
			{
				get
				{
					return this.HEAT_GENERATION_EFFICIENCY * -this.CALORIES_BURNED_PER_SECOND * 0.001f * this.KCAL2JOULES / 1000f;
				}
			}

			// Token: 0x17000C04 RID: 3076
			// (get) Token: 0x0600AFFF RID: 45055 RVA: 0x003B2A46 File Offset: 0x003B0C46
			public float GUESSTIMATE_CALORIES_BURNED_PER_SECOND
			{
				get
				{
					return this.CALORIES_BURNED_PER_CYCLE / 600f;
				}
			}

			// Token: 0x04009056 RID: 36950
			public float DEFAULT_MASS = 30f;

			// Token: 0x04009057 RID: 36951
			public float STAMINA_USED_PER_SECOND = -0.11666667f;

			// Token: 0x04009058 RID: 36952
			public float TRANSIT_TUBE_TRAVEL_SPEED = 18f;

			// Token: 0x04009059 RID: 36953
			public float OXYGEN_USED_PER_SECOND = 0.1f;

			// Token: 0x0400905A RID: 36954
			public float OXYGEN_TO_CO2_CONVERSION = 0.02f;

			// Token: 0x0400905B RID: 36955
			public float LOW_OXYGEN_THRESHOLD = 0.52f;

			// Token: 0x0400905C RID: 36956
			public float NO_OXYGEN_THRESHOLD = 0.05f;

			// Token: 0x0400905D RID: 36957
			public float RECOVER_BREATH_DELTA = 3f;

			// Token: 0x0400905E RID: 36958
			public float MIN_CO2_TO_EMIT = 0.02f;

			// Token: 0x0400905F RID: 36959
			public float BLADDER_INCREASE_PER_SECOND = 0.16666667f;

			// Token: 0x04009060 RID: 36960
			public float DECOR_EXPECTATION;

			// Token: 0x04009061 RID: 36961
			public float FOOD_QUALITY_EXPECTATION;

			// Token: 0x04009062 RID: 36962
			public float RECREATION_EXPECTATION = 2f;

			// Token: 0x04009063 RID: 36963
			public float MAX_PROFESSION_DECOR_EXPECTATION = 75f;

			// Token: 0x04009064 RID: 36964
			public float MAX_PROFESSION_FOOD_EXPECTATION;

			// Token: 0x04009065 RID: 36965
			public int MAX_UNDERWATER_TRAVEL_COST = 8;

			// Token: 0x04009066 RID: 36966
			public float TOILET_EFFICIENCY = 1f;

			// Token: 0x04009067 RID: 36967
			public float ROOM_TEMPERATURE_PREFERENCE;

			// Token: 0x04009068 RID: 36968
			public int BUILDING_DAMAGE_ACTING_OUT = 100;

			// Token: 0x04009069 RID: 36969
			public float IMMUNE_LEVEL_MAX = 100f;

			// Token: 0x0400906A RID: 36970
			public float IMMUNE_LEVEL_RECOVERY = 0.025f;

			// Token: 0x0400906B RID: 36971
			public float CARRY_CAPACITY = 200f;

			// Token: 0x0400906C RID: 36972
			public float HIT_POINTS = 100f;

			// Token: 0x0400906D RID: 36973
			public float RADIATION_RESISTANCE;

			// Token: 0x0400906E RID: 36974
			public string NAV_GRID_NAME = "MinionNavGrid";

			// Token: 0x0400906F RID: 36975
			public float KCAL2JOULES = 4184f;

			// Token: 0x04009070 RID: 36976
			public float MAX_CALORIES = 4000000f;

			// Token: 0x04009071 RID: 36977
			public float CALORIES_BURNED_PER_CYCLE = -1000000f;

			// Token: 0x04009072 RID: 36978
			public float SATISFIED_THRESHOLD = 0.95f;

			// Token: 0x04009073 RID: 36979
			public float COOLING_EFFICIENCY = 0.08f;

			// Token: 0x04009074 RID: 36980
			public float WARMING_EFFICIENCY = 0.08f;

			// Token: 0x04009075 RID: 36981
			public float HEAT_GENERATION_EFFICIENCY = 0.012f;

			// Token: 0x04009076 RID: 36982
			public float GUESSTIMATE_CALORIES_PER_CYCLE = -1600000f;
		}

		// Token: 0x02001FD6 RID: 8150
		public class TEMPERATURE
		{
			// Token: 0x04009077 RID: 36983
			public DUPLICANTSTATS.TEMPERATURE.EXTERNAL External = new DUPLICANTSTATS.TEMPERATURE.EXTERNAL();

			// Token: 0x04009078 RID: 36984
			public DUPLICANTSTATS.TEMPERATURE.INTERNAL Internal = new DUPLICANTSTATS.TEMPERATURE.INTERNAL();

			// Token: 0x04009079 RID: 36985
			public DUPLICANTSTATS.TEMPERATURE.CONDUCTIVITY_BARRIER_MODIFICATION Conductivity_Barrier_Modification = new DUPLICANTSTATS.TEMPERATURE.CONDUCTIVITY_BARRIER_MODIFICATION();

			// Token: 0x0400907A RID: 36986
			public float SKIN_THICKNESS = 0.002f;

			// Token: 0x0400907B RID: 36987
			public float SURFACE_AREA = 1f;

			// Token: 0x0400907C RID: 36988
			public float GROUND_TRANSFER_SCALE;

			// Token: 0x0200267B RID: 9851
			public class EXTERNAL
			{
				// Token: 0x0400AAE4 RID: 43748
				public float THRESHOLD_COLD = 283.15f;

				// Token: 0x0400AAE5 RID: 43749
				public float THRESHOLD_HOT = 306.15f;

				// Token: 0x0400AAE6 RID: 43750
				public float THRESHOLD_SCALDING = 345f;
			}

			// Token: 0x0200267C RID: 9852
			public class INTERNAL
			{
				// Token: 0x0400AAE7 RID: 43751
				public float IDEAL = 310.15f;

				// Token: 0x0400AAE8 RID: 43752
				public float THRESHOLD_HYPOTHERMIA = 308.15f;

				// Token: 0x0400AAE9 RID: 43753
				public float THRESHOLD_HYPERTHERMIA = 312.15f;

				// Token: 0x0400AAEA RID: 43754
				public float THRESHOLD_FATAL_HOT = 320.15f;

				// Token: 0x0400AAEB RID: 43755
				public float THRESHOLD_FATAL_COLD = 300.15f;
			}

			// Token: 0x0200267D RID: 9853
			public class CONDUCTIVITY_BARRIER_MODIFICATION
			{
				// Token: 0x0400AAEC RID: 43756
				public float SKINNY = -0.005f;

				// Token: 0x0400AAED RID: 43757
				public float PUDGY = 0.005f;
			}
		}

		// Token: 0x02001FD7 RID: 8151
		public class BREATH
		{
			// Token: 0x17000C05 RID: 3077
			// (get) Token: 0x0600B002 RID: 45058 RVA: 0x003B2BD3 File Offset: 0x003B0DD3
			public float RETREAT_AMOUNT
			{
				get
				{
					return this.RETREAT_AT_SECONDS / this.BREATH_BAR_TOTAL_SECONDS * this.BREATH_BAR_TOTAL_AMOUNT;
				}
			}

			// Token: 0x17000C06 RID: 3078
			// (get) Token: 0x0600B003 RID: 45059 RVA: 0x003B2BE9 File Offset: 0x003B0DE9
			public float SUFFOCATE_AMOUNT
			{
				get
				{
					return this.SUFFOCATION_WARN_AT_SECONDS / this.BREATH_BAR_TOTAL_SECONDS * this.BREATH_BAR_TOTAL_AMOUNT;
				}
			}

			// Token: 0x17000C07 RID: 3079
			// (get) Token: 0x0600B004 RID: 45060 RVA: 0x003B2BFF File Offset: 0x003B0DFF
			public float BREATH_RATE
			{
				get
				{
					return this.BREATH_BAR_TOTAL_AMOUNT / this.BREATH_BAR_TOTAL_SECONDS;
				}
			}

			// Token: 0x0400907D RID: 36989
			private float BREATH_BAR_TOTAL_SECONDS = 110f;

			// Token: 0x0400907E RID: 36990
			private float RETREAT_AT_SECONDS = 80f;

			// Token: 0x0400907F RID: 36991
			private float SUFFOCATION_WARN_AT_SECONDS = 50f;

			// Token: 0x04009080 RID: 36992
			public float BREATH_BAR_TOTAL_AMOUNT = 100f;
		}

		// Token: 0x02001FD8 RID: 8152
		public class LIGHT
		{
			// Token: 0x04009081 RID: 36993
			public int LUX_SUNBURN = 72000;

			// Token: 0x04009082 RID: 36994
			public float SUNBURN_DELAY_TIME = 120f;

			// Token: 0x04009083 RID: 36995
			public int LUX_PLEASANT_LIGHT = 40000;

			// Token: 0x04009084 RID: 36996
			public float LIGHT_WORK_EFFICIENCY_BONUS = 0.15f;

			// Token: 0x04009085 RID: 36997
			public int NO_LIGHT;

			// Token: 0x04009086 RID: 36998
			public int VERY_LOW_LIGHT = 1;

			// Token: 0x04009087 RID: 36999
			public int LOW_LIGHT = 500;

			// Token: 0x04009088 RID: 37000
			public int MEDIUM_LIGHT = 1000;

			// Token: 0x04009089 RID: 37001
			public int HIGH_LIGHT = 10000;

			// Token: 0x0400908A RID: 37002
			public int VERY_HIGH_LIGHT = 50000;

			// Token: 0x0400908B RID: 37003
			public int MAX_LIGHT = 100000;
		}

		// Token: 0x02001FD9 RID: 8153
		public class COMBAT
		{
			// Token: 0x0400908C RID: 37004
			public DUPLICANTSTATS.COMBAT.BASICWEAPON BasicWeapon = new DUPLICANTSTATS.COMBAT.BASICWEAPON();

			// Token: 0x0400908D RID: 37005
			public Health.HealthState FLEE_THRESHOLD = Health.HealthState.Critical;

			// Token: 0x0200267E RID: 9854
			public class BASICWEAPON
			{
				// Token: 0x0400AAEE RID: 43758
				public float ATTACKS_PER_SECOND = 2f;

				// Token: 0x0400AAEF RID: 43759
				public float MIN_DAMAGE_PER_HIT = 1f;

				// Token: 0x0400AAF0 RID: 43760
				public float MAX_DAMAGE_PER_HIT = 1f;

				// Token: 0x0400AAF1 RID: 43761
				public AttackProperties.TargetType TARGET_TYPE;

				// Token: 0x0400AAF2 RID: 43762
				public AttackProperties.DamageType DAMAGE_TYPE;

				// Token: 0x0400AAF3 RID: 43763
				public int MAX_HITS = 1;

				// Token: 0x0400AAF4 RID: 43764
				public float AREA_OF_EFFECT_RADIUS;
			}
		}

		// Token: 0x02001FDA RID: 8154
		public class SECRETIONS
		{
			// Token: 0x0400908E RID: 37006
			public float PEE_FUSE_TIME = 120f;

			// Token: 0x0400908F RID: 37007
			public float PEE_PER_FLOOR_PEE = 2f;

			// Token: 0x04009090 RID: 37008
			public float PEE_PER_TOILET_PEE = 6.7f;

			// Token: 0x04009091 RID: 37009
			public string PEE_DISEASE = "FoodPoisoning";

			// Token: 0x04009092 RID: 37010
			public int DISEASE_PER_PEE = 100000;

			// Token: 0x04009093 RID: 37011
			public int DISEASE_PER_VOMIT = 100000;
		}
	}
}
