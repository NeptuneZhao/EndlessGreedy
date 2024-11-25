using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;

namespace TUNING
{
	// Token: 0x02000EFD RID: 3837
	public class CREATURES
	{
		// Token: 0x040057C6 RID: 22470
		public const float WILD_GROWTH_RATE_MODIFIER = 0.25f;

		// Token: 0x040057C7 RID: 22471
		public const int DEFAULT_PROBING_RADIUS = 32;

		// Token: 0x040057C8 RID: 22472
		public const float CREATURES_BASE_GENERATION_KILOWATTS = 10f;

		// Token: 0x040057C9 RID: 22473
		public const float FERTILITY_TIME_BY_LIFESPAN = 0.6f;

		// Token: 0x040057CA RID: 22474
		public const float INCUBATION_TIME_BY_LIFESPAN = 0.2f;

		// Token: 0x040057CB RID: 22475
		public const float INCUBATOR_INCUBATION_MULTIPLIER = 4f;

		// Token: 0x040057CC RID: 22476
		public const float WILD_CALORIE_BURN_RATIO = 0.25f;

		// Token: 0x040057CD RID: 22477
		public const float HUG_INCUBATION_MULTIPLIER = 1f;

		// Token: 0x040057CE RID: 22478
		public const float VIABILITY_LOSS_RATE = -0.016666668f;

		// Token: 0x040057CF RID: 22479
		public const float STATERPILLAR_POWER_CHARGE_LOSS_RATE = -0.055555556f;

		// Token: 0x02001FE6 RID: 8166
		public class HITPOINTS
		{
			// Token: 0x04009107 RID: 37127
			public const float TIER0 = 5f;

			// Token: 0x04009108 RID: 37128
			public const float TIER1 = 25f;

			// Token: 0x04009109 RID: 37129
			public const float TIER2 = 50f;

			// Token: 0x0400910A RID: 37130
			public const float TIER3 = 100f;

			// Token: 0x0400910B RID: 37131
			public const float TIER4 = 150f;

			// Token: 0x0400910C RID: 37132
			public const float TIER5 = 200f;

			// Token: 0x0400910D RID: 37133
			public const float TIER6 = 400f;
		}

		// Token: 0x02001FE7 RID: 8167
		public class MASS_KG
		{
			// Token: 0x0400910E RID: 37134
			public const float TIER0 = 5f;

			// Token: 0x0400910F RID: 37135
			public const float TIER1 = 25f;

			// Token: 0x04009110 RID: 37136
			public const float TIER2 = 50f;

			// Token: 0x04009111 RID: 37137
			public const float TIER3 = 100f;

			// Token: 0x04009112 RID: 37138
			public const float TIER4 = 200f;

			// Token: 0x04009113 RID: 37139
			public const float TIER5 = 400f;
		}

		// Token: 0x02001FE8 RID: 8168
		public class TEMPERATURE
		{
			// Token: 0x04009114 RID: 37140
			public const float SKIN_THICKNESS = 0.025f;

			// Token: 0x04009115 RID: 37141
			public const float SURFACE_AREA = 17.5f;

			// Token: 0x04009116 RID: 37142
			public const float GROUND_TRANSFER_SCALE = 0f;

			// Token: 0x04009117 RID: 37143
			public static float FREEZING_10 = 173f;

			// Token: 0x04009118 RID: 37144
			public static float FREEZING_9 = 183f;

			// Token: 0x04009119 RID: 37145
			public static float FREEZING_3 = 243f;

			// Token: 0x0400911A RID: 37146
			public static float FREEZING_2 = 253f;

			// Token: 0x0400911B RID: 37147
			public static float FREEZING_1 = 263f;

			// Token: 0x0400911C RID: 37148
			public static float FREEZING = 273f;

			// Token: 0x0400911D RID: 37149
			public static float COOL = 283f;

			// Token: 0x0400911E RID: 37150
			public static float MODERATE = 293f;

			// Token: 0x0400911F RID: 37151
			public static float HOT = 303f;

			// Token: 0x04009120 RID: 37152
			public static float HOT_1 = 313f;

			// Token: 0x04009121 RID: 37153
			public static float HOT_2 = 323f;

			// Token: 0x04009122 RID: 37154
			public static float HOT_3 = 333f;

			// Token: 0x04009123 RID: 37155
			public static float HOT_7 = 373f;
		}

		// Token: 0x02001FE9 RID: 8169
		public class LIFESPAN
		{
			// Token: 0x04009124 RID: 37156
			public const float TIER0 = 5f;

			// Token: 0x04009125 RID: 37157
			public const float TIER1 = 25f;

			// Token: 0x04009126 RID: 37158
			public const float TIER2 = 75f;

			// Token: 0x04009127 RID: 37159
			public const float TIER3 = 100f;

			// Token: 0x04009128 RID: 37160
			public const float TIER4 = 150f;

			// Token: 0x04009129 RID: 37161
			public const float TIER5 = 200f;

			// Token: 0x0400912A RID: 37162
			public const float TIER6 = 400f;
		}

		// Token: 0x02001FEA RID: 8170
		public class CONVERSION_EFFICIENCY
		{
			// Token: 0x0400912B RID: 37163
			public static float BAD_2 = 0.1f;

			// Token: 0x0400912C RID: 37164
			public static float BAD_1 = 0.25f;

			// Token: 0x0400912D RID: 37165
			public static float NORMAL = 0.5f;

			// Token: 0x0400912E RID: 37166
			public static float GOOD_1 = 0.75f;

			// Token: 0x0400912F RID: 37167
			public static float GOOD_2 = 0.95f;

			// Token: 0x04009130 RID: 37168
			public static float GOOD_3 = 1f;
		}

		// Token: 0x02001FEB RID: 8171
		public class SPACE_REQUIREMENTS
		{
			// Token: 0x04009131 RID: 37169
			public static int TIER1 = 4;

			// Token: 0x04009132 RID: 37170
			public static int TIER2 = 8;

			// Token: 0x04009133 RID: 37171
			public static int TIER3 = 12;

			// Token: 0x04009134 RID: 37172
			public static int TIER4 = 16;
		}

		// Token: 0x02001FEC RID: 8172
		public class EGG_CHANCE_MODIFIERS
		{
			// Token: 0x0600B01C RID: 45084 RVA: 0x003B3925 File Offset: 0x003B1B25
			private static System.Action CreateDietaryModifier(string id, Tag eggTag, HashSet<Tag> foodTags, float modifierPerCal)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.DIET.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.DIET.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							string arg = string.Join(", ", (from t in foodTags
							select t.ProperName()).ToArray<string>());
							descStr = string.Format(descStr, arg);
							return descStr;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							inst.gameObject.Subscribe(-2038961714, delegate(object data)
							{
								CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
								if (foodTags.Contains(caloriesConsumedEvent.tag))
								{
									inst.AddBreedingChance(eggType, caloriesConsumedEvent.calories * modifierPerCal);
								}
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600B01D RID: 45085 RVA: 0x003B3953 File Offset: 0x003B1B53
			private static System.Action CreateDietaryModifier(string id, Tag eggTag, Tag foodTag, float modifierPerCal)
			{
				return CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier(id, eggTag, new HashSet<Tag>
				{
					foodTag
				}, modifierPerCal);
			}

			// Token: 0x0600B01E RID: 45086 RVA: 0x003B396A File Offset: 0x003B1B6A
			private static System.Action CreateNearbyCreatureModifier(string id, Tag eggTag, Tag nearbyCreatureBaby, Tag nearbyCreatureAdult, float modifierPerSecond, bool alsoInvert)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = (modifierPerSecond < 0f) ? CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.NAME : CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.NAME;
					string text2 = (modifierPerSecond < 0f) ? CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.DESC : CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = ((string descStr) => string.Format(descStr, nearbyCreatureAdult.ProperName())));
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							NearbyCreatureMonitor.Instance instance = inst.gameObject.GetSMI<NearbyCreatureMonitor.Instance>();
							if (instance == null)
							{
								instance = new NearbyCreatureMonitor.Instance(inst.master);
								instance.StartSM();
							}
							instance.OnUpdateNearbyCreatures += delegate(float dt, List<KPrefabID> creatures, List<KPrefabID> eggs)
							{
								bool flag = false;
								foreach (KPrefabID kprefabID in creatures)
								{
									if (kprefabID.PrefabTag == nearbyCreatureBaby || kprefabID.PrefabTag == nearbyCreatureAdult)
									{
										flag = true;
										break;
									}
								}
								if (flag)
								{
									inst.AddBreedingChance(eggType, dt * modifierPerSecond);
									return;
								}
								if (alsoInvert)
								{
									inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
								}
							};
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600B01F RID: 45087 RVA: 0x003B39A8 File Offset: 0x003B1BA8
			private static System.Action CreateElementCreatureModifier(string id, Tag eggTag, Tag element, float modifierPerSecond, bool alsoInvert, bool checkSubstantialLiquid, string tooltipOverride = null)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							if (tooltipOverride == null)
							{
								return string.Format(descStr, ElementLoader.GetElement(element).name);
							}
							return tooltipOverride;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							CritterElementMonitor.Instance instance = inst.gameObject.GetSMI<CritterElementMonitor.Instance>();
							if (instance == null)
							{
								instance = new CritterElementMonitor.Instance(inst.master);
								instance.StartSM();
							}
							instance.OnUpdateEggChances += delegate(float dt)
							{
								int num = Grid.PosToCell(inst);
								if (!Grid.IsValidCell(num))
								{
									return;
								}
								if (Grid.Element[num].HasTag(element) && (!checkSubstantialLiquid || Grid.IsSubstantialLiquid(num, 0.35f)))
								{
									inst.AddBreedingChance(eggType, dt * modifierPerSecond);
									return;
								}
								if (alsoInvert)
								{
									inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
								}
							};
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600B020 RID: 45088 RVA: 0x003B39F9 File Offset: 0x003B1BF9
			private static System.Action CreateCropTendedModifier(string id, Tag eggTag, HashSet<Tag> cropTags, float modifierPerEvent)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.CROPTENDING.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.CROPTENDING.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							string arg = string.Join(", ", (from t in cropTags
							select t.ProperName()).ToArray<string>());
							descStr = string.Format(descStr, arg);
							return descStr;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							inst.gameObject.Subscribe(90606262, delegate(object data)
							{
								CropTendingStates.CropTendingEventData cropTendingEventData = (CropTendingStates.CropTendingEventData)data;
								if (cropTags.Contains(cropTendingEventData.cropId))
								{
									inst.AddBreedingChance(eggType, modifierPerEvent);
								}
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600B021 RID: 45089 RVA: 0x003B3A27 File Offset: 0x003B1C27
			private static System.Action CreateTemperatureModifier(string id, Tag eggTag, float minTemp, float maxTemp, float modifierPerSecond, bool alsoInvert)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.NAME;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = null;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = ((string src) => string.Format(CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.DESC, GameUtil.GetFormattedTemperature(minTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(maxTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))));
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							CritterTemperatureMonitor.Instance smi = inst.gameObject.GetSMI<CritterTemperatureMonitor.Instance>();
							if (smi != null)
							{
								CritterTemperatureMonitor.Instance instance = smi;
								instance.OnUpdate_GetTemperatureInternal = (Action<float, float>)Delegate.Combine(instance.OnUpdate_GetTemperatureInternal, new Action<float, float>(delegate(float dt, float newTemp)
								{
									if (newTemp > minTemp && newTemp < maxTemp)
									{
										inst.AddBreedingChance(eggType, dt * modifierPerSecond);
										return;
									}
									if (alsoInvert)
									{
										inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
									}
								}));
								return;
							}
							DebugUtil.LogErrorArgs(new object[]
							{
								"Ack! Trying to add temperature modifier",
								id,
								"to",
								inst.master.name,
								"but it doesn't have a CritterTemperatureMonitor.Instance"
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x04009135 RID: 37173
			public static List<System.Action> MODIFIER_CREATORS = new List<System.Action>
			{
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchHard", "HatchHardEgg".ToTag(), SimHashes.SedimentaryRock.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchVeggie", "HatchVeggieEgg".ToTag(), SimHashes.Dirt.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchMetal", "HatchMetalEgg".ToTag(), HatchMetalConfig.METAL_ORE_TAGS, 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaBalance", "PuftAlphaEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), -0.00025f, true),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyOxylite", "PuftOxyliteEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyBleachstone", "PuftBleachstoneEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterHighTemp", "OilfloaterHighTempEgg".ToTag(), 373.15f, 523.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterDecor", "OilfloaterDecorEgg".ToTag(), 293.15f, 333.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugOrange", "LightBugOrangeEgg".ToTag(), "GrilledPrickleFruit".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPurple", "LightBugPurpleEgg".ToTag(), "FriedMushroom".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPink", "LightBugPinkEgg".ToTag(), "SpiceBread".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlue", "LightBugBlueEgg".ToTag(), "Salsa".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlack", "LightBugBlackEgg".ToTag(), SimHashes.Phosphorus.CreateTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugCrystal", "LightBugCrystalEgg".ToTag(), "CookedMeat".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuTropical", "PacuTropicalEgg".ToTag(), 308.15f, 353.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuCleaner", "PacuCleanerEgg".ToTag(), 243.15f, 278.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("DreckoPlastic", "DreckoPlasticEgg".ToTag(), "BasicSingleHarvestPlant".ToTag(), 0.025f / DreckoTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("SquirrelHug", "SquirrelHugEgg".ToTag(), BasicFabricMaterialPlantConfig.ID.ToTag(), 0.025f / SquirrelTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateCropTendedModifier("DivergentWorm", "DivergentWormEgg".ToTag(), new HashSet<Tag>
				{
					"WormPlant".ToTag(),
					"SuperWormPlant".ToTag()
				}, 0.05f / (float)DivergentTuning.TIMES_TENDED_PER_CYCLE_FOR_EVOLUTION),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("PokeLumber", "CrabWoodEgg".ToTag(), SimHashes.Ethanol.CreateTag(), 0.00025f, true, true, null),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("PokeFreshWater", "CrabFreshWaterEgg".ToTag(), SimHashes.Water.CreateTag(), 0.00025f, true, true, null),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("MoleDelicacy", "MoleDelicacyEgg".ToTag(), MoleDelicacyConfig.EGG_CHANCES_TEMPERATURE_MIN, MoleDelicacyConfig.EGG_CHANCES_TEMPERATURE_MAX, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("StaterpillarGas", "StaterpillarGasEgg".ToTag(), GameTags.Unbreathable, 0.00025f, true, false, CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.UNBREATHABLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("StaterpillarLiquid", "StaterpillarLiquidEgg".ToTag(), GameTags.Liquid, 0.00025f, true, false, CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.LIQUID),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("BellyGold", "GoldBellyEgg".ToTag(), "FriesCarrot".ToTag(), 0.05f / BellyTuning.STANDARD_CALORIES_PER_CYCLE)
			};
		}

		// Token: 0x02001FED RID: 8173
		public class SORTING
		{
			// Token: 0x04009136 RID: 37174
			public static Dictionary<string, int> CRITTER_ORDER = new Dictionary<string, int>
			{
				{
					"Hatch",
					10
				},
				{
					"Puft",
					20
				},
				{
					"Drecko",
					30
				},
				{
					"Squirrel",
					40
				},
				{
					"Pacu",
					50
				},
				{
					"Oilfloater",
					60
				},
				{
					"LightBug",
					70
				},
				{
					"Crab",
					80
				},
				{
					"DivergentBeetle",
					90
				},
				{
					"Staterpillar",
					100
				},
				{
					"Mole",
					110
				},
				{
					"Bee",
					120
				},
				{
					"Moo",
					130
				},
				{
					"Glom",
					140
				},
				{
					"WoodDeer",
					140
				},
				{
					"Seal",
					150
				},
				{
					"IceBelly",
					160
				}
			};
		}
	}
}
