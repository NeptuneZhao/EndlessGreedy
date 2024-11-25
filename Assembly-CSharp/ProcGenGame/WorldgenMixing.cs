﻿using System;
using System.Collections.Generic;
using System.Linq;
using Klei;
using Klei.CustomSettings;
using ObjectCloner;
using ProcGen;
using STRINGS;

namespace ProcGenGame
{
	// Token: 0x02000E13 RID: 3603
	public class WorldgenMixing
	{
		// Token: 0x060072D5 RID: 29397 RVA: 0x002C01C4 File Offset: 0x002BE3C4
		public static bool RefreshWorldMixing(MutatedClusterLayout mutatedLayout, int seed, bool isRunningWorldgenDebug, bool muteErrors)
		{
			if (mutatedLayout == null)
			{
				return false;
			}
			foreach (WorldPlacement worldPlacement in mutatedLayout.layout.worldPlacements)
			{
				worldPlacement.UndoWorldMixing();
			}
			return WorldgenMixing.DoWorldMixingInternal(mutatedLayout, seed, isRunningWorldgenDebug, muteErrors) != null;
		}

		// Token: 0x060072D6 RID: 29398 RVA: 0x002C022C File Offset: 0x002BE42C
		public static MutatedClusterLayout DoWorldMixing(ClusterLayout layout, int seed, bool isRunningWorldgenDebug, bool muteErrors)
		{
			return WorldgenMixing.DoWorldMixingInternal(new MutatedClusterLayout(layout), seed, isRunningWorldgenDebug, muteErrors);
		}

		// Token: 0x060072D7 RID: 29399 RVA: 0x002C023C File Offset: 0x002BE43C
		private static MutatedClusterLayout DoWorldMixingInternal(MutatedClusterLayout mutatedClusterLayout, int seed, bool isRunningWorldgenDebug, bool muteErrors)
		{
			List<WorldgenMixing.WorldMixingOption> list = new List<WorldgenMixing.WorldMixingOption>();
			if (CustomGameSettings.Instance != null && !GenericGameSettings.instance.devAutoWorldGen)
			{
				using (List<WorldMixingSettingConfig>.Enumerator enumerator = CustomGameSettings.Instance.GetActiveWorldMixingSettings().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						WorldMixingSettingConfig worldMixingSettingConfig = enumerator.Current;
						WorldMixingSettings worldMixingSettings = SettingsCache.TryGetCachedWorldMixingSetting(worldMixingSettingConfig.worldgenPath);
						if (!mutatedClusterLayout.layout.HasAnyTags(worldMixingSettings.forbiddenClusterTags))
						{
							int minCount = (CustomGameSettings.Instance.GetCurrentMixingSettingLevel(worldMixingSettingConfig.id).id == "GuranteeMixing") ? 1 : 0;
							ProcGen.World worldData = SettingsCache.worlds.GetWorldData(worldMixingSettings.world);
							list.Add(new WorldgenMixing.WorldMixingOption
							{
								worldgenPath = worldMixingSettings.world,
								mixingSettings = worldMixingSettings,
								minCount = minCount,
								maxCount = 1,
								cachedWorld = worldData
							});
						}
					}
					goto IL_167;
				}
			}
			string[] devWorldMixing = GenericGameSettings.instance.devWorldMixing;
			for (int i = 0; i < devWorldMixing.Length; i++)
			{
				WorldMixingSettings worldMixingSettings2 = SettingsCache.TryGetCachedWorldMixingSetting(devWorldMixing[i]);
				ProcGen.World worldData2 = SettingsCache.worlds.GetWorldData(worldMixingSettings2.world);
				list.Add(new WorldgenMixing.WorldMixingOption
				{
					worldgenPath = worldMixingSettings2.world,
					mixingSettings = worldMixingSettings2,
					minCount = 1,
					maxCount = 1,
					cachedWorld = worldData2
				});
			}
			IL_167:
			KRandom rng = new KRandom(seed);
			foreach (WorldPlacement worldPlacement in mutatedClusterLayout.layout.worldPlacements)
			{
				worldPlacement.UndoWorldMixing();
			}
			List<WorldPlacement> list2 = new List<WorldPlacement>(mutatedClusterLayout.layout.worldPlacements);
			list2.ShuffleSeeded(rng);
			foreach (WorldPlacement worldPlacement2 in list2)
			{
				if (worldPlacement2.IsMixingPlacement())
				{
					list.ShuffleSeeded(rng);
					WorldgenMixing.WorldMixingOption worldMixingOption = WorldgenMixing.FindWorldMixingOption(worldPlacement2, list);
					if (worldMixingOption != null)
					{
						Debug.Log("Mixing: Applied world substitution " + worldPlacement2.world + " -> " + worldMixingOption.worldgenPath);
						worldPlacement2.worldMixing.previousWorld = worldPlacement2.world;
						worldPlacement2.worldMixing.mixingWasApplied = true;
						worldPlacement2.world = worldMixingOption.worldgenPath;
						worldMixingOption.Consume();
						if (worldMixingOption.IsExhausted)
						{
							list.Remove(worldMixingOption);
						}
					}
				}
			}
			if (!WorldgenMixing.ValidateWorldMixingOptions(list, isRunningWorldgenDebug, muteErrors))
			{
				return null;
			}
			return mutatedClusterLayout;
		}

		// Token: 0x060072D8 RID: 29400 RVA: 0x002C04F4 File Offset: 0x002BE6F4
		private static WorldgenMixing.WorldMixingOption FindWorldMixingOption(WorldPlacement worldPlacement, List<WorldgenMixing.WorldMixingOption> options)
		{
			options = options.StableSort<WorldgenMixing.WorldMixingOption>().ToList<WorldgenMixing.WorldMixingOption>();
			foreach (WorldgenMixing.WorldMixingOption worldMixingOption in options)
			{
				if (!worldMixingOption.IsExhausted)
				{
					bool flag = true;
					foreach (string item in worldPlacement.worldMixing.requiredTags)
					{
						if (!worldMixingOption.cachedWorld.worldTags.Contains(item))
						{
							flag = false;
							break;
						}
					}
					foreach (string item2 in worldPlacement.worldMixing.forbiddenTags)
					{
						if (worldMixingOption.cachedWorld.worldTags.Contains(item2))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return worldMixingOption;
					}
				}
			}
			return null;
		}

		// Token: 0x060072D9 RID: 29401 RVA: 0x002C061C File Offset: 0x002BE81C
		private static bool ValidateWorldMixingOptions(List<WorldgenMixing.WorldMixingOption> options, bool isRunningWorldgenDebug, bool muteErrors)
		{
			List<string> list = new List<string>();
			foreach (WorldgenMixing.WorldMixingOption worldMixingOption in options)
			{
				if (!worldMixingOption.IsSatisfied)
				{
					list.Add(string.Format("{0} ({1})", worldMixingOption.worldgenPath, worldMixingOption.minCount));
				}
			}
			if (list.Count <= 0)
			{
				return true;
			}
			if (muteErrors)
			{
				return false;
			}
			string text = "WorldgenMixing: Could not guarantee these world mixings: " + string.Join("\n - ", list);
			if (!isRunningWorldgenDebug)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					text
				});
				throw new WorldgenException(text, UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE_MIXING);
			}
			DebugUtil.LogErrorArgs(new object[]
			{
				text
			});
			return false;
		}

		// Token: 0x060072DA RID: 29402 RVA: 0x002C06EC File Offset: 0x002BE8EC
		public static void DoSubworldMixing(Cluster cluster, int seed, Func<int, WorldGen, bool> ShouldSkipWorldCallback, bool isRunningWorldgenDebug)
		{
			List<WorldgenMixing.MixingOption<SubworldMixingSettings>> list = new List<WorldgenMixing.MixingOption<SubworldMixingSettings>>();
			if (CustomGameSettings.Instance != null && !GenericGameSettings.instance.devAutoWorldGen)
			{
				using (List<SubworldMixingSettingConfig>.Enumerator enumerator = CustomGameSettings.Instance.GetActiveSubworldMixingSettings().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SubworldMixingSettingConfig subworldMixingSettingConfig = enumerator.Current;
						SubworldMixingSettings mixingSettings = SettingsCache.TryGetCachedSubworldMixingSetting(subworldMixingSettingConfig.worldgenPath);
						if (!cluster.clusterLayout.HasAnyTags(subworldMixingSettingConfig.forbiddenClusterTags))
						{
							int minCount = (CustomGameSettings.Instance.GetCurrentMixingSettingLevel(subworldMixingSettingConfig.id).id == "GuranteeMixing") ? 1 : 0;
							list.Add(new WorldgenMixing.MixingOption<SubworldMixingSettings>
							{
								worldgenPath = subworldMixingSettingConfig.worldgenPath,
								mixingSettings = mixingSettings,
								minCount = minCount,
								maxCount = 3
							});
						}
					}
					goto IL_130;
				}
			}
			foreach (string text in GenericGameSettings.instance.devSubworldMixing)
			{
				SubworldMixingSettings mixingSettings2 = SettingsCache.TryGetCachedSubworldMixingSetting(text);
				list.Add(new WorldgenMixing.MixingOption<SubworldMixingSettings>
				{
					worldgenPath = text,
					mixingSettings = mixingSettings2,
					minCount = 1,
					maxCount = 3
				});
			}
			IL_130:
			KRandom rng = new KRandom(seed);
			List<WorldGen> list2 = new List<WorldGen>(cluster.worlds);
			list2.ShuffleSeeded(rng);
			list2.Sort((WorldGen a, WorldGen b) => WorldPlacement.GetSortOrder(a.Settings.worldType).CompareTo(WorldPlacement.GetSortOrder(b.Settings.worldType)));
			for (int j = 0; j < cluster.worlds.Count; j++)
			{
				WorldGen worldGen = list2[j];
				list.ShuffleSeeded(rng);
				WorldgenMixing.ApplySubworldMixingToWorld(worldGen.Settings.world, list);
			}
			WorldgenMixing.ValidateSubworldMixingOptions(list, isRunningWorldgenDebug);
		}

		// Token: 0x060072DB RID: 29403 RVA: 0x002C08B8 File Offset: 0x002BEAB8
		private static void ValidateSubworldMixingOptions(List<WorldgenMixing.MixingOption<SubworldMixingSettings>> options, bool isRunningWorldgenDebug)
		{
			List<string> list = new List<string>();
			foreach (WorldgenMixing.MixingOption<SubworldMixingSettings> mixingOption in options)
			{
				if (!mixingOption.IsSatisfied)
				{
					list.Add(string.Format("{0} ({1})", mixingOption.worldgenPath, mixingOption.minCount));
				}
			}
			if (list.Count > 0)
			{
				string text = "WorldgenMixing: Could not guarantee these subworld mixings: " + string.Join("\n - ", list);
				if (!isRunningWorldgenDebug)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						text
					});
					throw new WorldgenException(text, UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE_MIXING);
				}
				DebugUtil.LogErrorArgs(new object[]
				{
					text
				});
			}
		}

		// Token: 0x060072DC RID: 29404 RVA: 0x002C0980 File Offset: 0x002BEB80
		private static void ApplySubworldMixingToWorld(ProcGen.World world, List<WorldgenMixing.MixingOption<SubworldMixingSettings>> availableSubworldsForMixing)
		{
			List<ProcGen.World.SubworldMixingRule> list = new List<ProcGen.World.SubworldMixingRule>();
			foreach (ProcGen.World.SubworldMixingRule subworldMixingRule in world.subworldMixingRules)
			{
				if (availableSubworldsForMixing.Count == 0)
				{
					WorldgenMixing.CleanupUnusedMixing(world);
					return;
				}
				WorldgenMixing.MixingOption<SubworldMixingSettings> mixingOption = WorldgenMixing.FindSubworldMixing(subworldMixingRule, availableSubworldsForMixing);
				if (mixingOption == null)
				{
					string[] array = new string[6];
					array[0] = "WorldgenMixing: No valid mixing for '";
					array[1] = subworldMixingRule.name;
					array[2] = "' on World '";
					array[3] = world.name;
					array[4] = "' from options: ";
					array[5] = string.Join(", ", from x in availableSubworldsForMixing
					where !x.IsExhausted
					select x.mixingSettings.name);
					Debug.Log(string.Concat(array));
				}
				else
				{
					WeightedSubworldName weightedSubworldName = SerializingCloner.Copy<WeightedSubworldName>(mixingOption.mixingSettings.subworld);
					weightedSubworldName.minCount = Math.Max(subworldMixingRule.minCount, weightedSubworldName.minCount);
					weightedSubworldName.maxCount = Math.Min(subworldMixingRule.maxCount, weightedSubworldName.maxCount);
					world.subworldFiles.Add(weightedSubworldName);
					foreach (ProcGen.World.AllowedCellsFilter allowedCellsFilter in world.unknownCellsAllowedSubworlds)
					{
						for (int i = 0; i < allowedCellsFilter.subworldNames.Count; i++)
						{
							if (allowedCellsFilter.subworldNames[i] == subworldMixingRule.name)
							{
								allowedCellsFilter.subworldNames[i] = weightedSubworldName.name;
							}
						}
					}
					if (!list.Contains(subworldMixingRule))
					{
						world.worldTemplateRules.AddRange(mixingOption.mixingSettings.additionalWorldTemplateRules);
						list.Add(subworldMixingRule);
					}
					mixingOption.Consume();
					if (mixingOption.IsExhausted)
					{
						availableSubworldsForMixing.Remove(mixingOption);
					}
				}
			}
			WorldgenMixing.CleanupUnusedMixing(world);
		}

		// Token: 0x060072DD RID: 29405 RVA: 0x002C0BC4 File Offset: 0x002BEDC4
		private static WorldgenMixing.MixingOption<SubworldMixingSettings> FindSubworldMixing(ProcGen.World.SubworldMixingRule rule, List<WorldgenMixing.MixingOption<SubworldMixingSettings>> options)
		{
			options = options.StableSort<WorldgenMixing.MixingOption<SubworldMixingSettings>>().ToList<WorldgenMixing.MixingOption<SubworldMixingSettings>>();
			foreach (WorldgenMixing.MixingOption<SubworldMixingSettings> mixingOption in options)
			{
				if (!mixingOption.IsExhausted)
				{
					bool flag = true;
					foreach (string item in rule.forbiddenTags)
					{
						if (mixingOption.mixingSettings.mixingTags.Contains(item))
						{
							flag = false;
						}
					}
					foreach (string item2 in rule.requiredTags)
					{
						if (!mixingOption.mixingSettings.mixingTags.Contains(item2))
						{
							flag = false;
						}
					}
					int num = Math.Max(rule.minCount, mixingOption.mixingSettings.subworld.minCount);
					int num2 = Math.Min(rule.maxCount, mixingOption.mixingSettings.subworld.maxCount);
					if (num > num2)
					{
						flag = false;
					}
					if (flag)
					{
						return mixingOption;
					}
				}
			}
			return null;
		}

		// Token: 0x060072DE RID: 29406 RVA: 0x002C0D1C File Offset: 0x002BEF1C
		private static void CleanupUnusedMixing(ProcGen.World world)
		{
			foreach (ProcGen.World.AllowedCellsFilter allowedCellsFilter in world.unknownCellsAllowedSubworlds)
			{
				allowedCellsFilter.subworldNames.RemoveAll(new Predicate<string>(WorldgenMixing.IsMixingProxyName));
			}
		}

		// Token: 0x060072DF RID: 29407 RVA: 0x002C0D80 File Offset: 0x002BEF80
		private static bool IsMixingProxyName(string name)
		{
			return name.StartsWith("(");
		}

		// Token: 0x04004F0D RID: 20237
		private const int NUM_WORLD_TO_TRY_SUBWORLDMIXING = 3;

		// Token: 0x02001F42 RID: 8002
		public class MixingOption<T> : IComparable<WorldgenMixing.MixingOption<T>>
		{
			// Token: 0x17000BF5 RID: 3061
			// (get) Token: 0x0600ADC2 RID: 44482 RVA: 0x003A9FA1 File Offset: 0x003A81A1
			public bool IsExhausted
			{
				get
				{
					return this.maxCount <= 0;
				}
			}

			// Token: 0x17000BF6 RID: 3062
			// (get) Token: 0x0600ADC3 RID: 44483 RVA: 0x003A9FAF File Offset: 0x003A81AF
			public bool IsSatisfied
			{
				get
				{
					return this.minCount <= 0;
				}
			}

			// Token: 0x0600ADC4 RID: 44484 RVA: 0x003A9FBD File Offset: 0x003A81BD
			public void Consume()
			{
				this.minCount--;
				this.maxCount--;
			}

			// Token: 0x0600ADC5 RID: 44485 RVA: 0x003A9FDC File Offset: 0x003A81DC
			public int CompareTo(WorldgenMixing.MixingOption<T> other)
			{
				int num = other.minCount.CompareTo(this.minCount);
				if (num != 0)
				{
					return num;
				}
				return other.maxCount.CompareTo(this.maxCount);
			}

			// Token: 0x04008CFE RID: 36094
			public string worldgenPath;

			// Token: 0x04008CFF RID: 36095
			public T mixingSettings;

			// Token: 0x04008D00 RID: 36096
			public int minCount;

			// Token: 0x04008D01 RID: 36097
			public int maxCount;
		}

		// Token: 0x02001F43 RID: 8003
		public class WorldMixingOption : WorldgenMixing.MixingOption<WorldMixingSettings>
		{
			// Token: 0x04008D02 RID: 36098
			public ProcGen.World cachedWorld;
		}
	}
}
