using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Database;
using Klei.CustomSettings;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x0200082D RID: 2093
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CustomGameSettings")]
public class CustomGameSettings : KMonoBehaviour
{
	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x060039F5 RID: 14837 RVA: 0x0013BE1D File Offset: 0x0013A01D
	public static CustomGameSettings Instance
	{
		get
		{
			return CustomGameSettings.instance;
		}
	}

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x060039F6 RID: 14838 RVA: 0x0013BE24 File Offset: 0x0013A024
	public IReadOnlyDictionary<string, string> CurrentStoryLevelsBySetting
	{
		get
		{
			return this.currentStoryLevelsBySetting;
		}
	}

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x060039F7 RID: 14839 RVA: 0x0013BE2C File Offset: 0x0013A02C
	// (remove) Token: 0x060039F8 RID: 14840 RVA: 0x0013BE64 File Offset: 0x0013A064
	public event Action<SettingConfig, SettingLevel> OnQualitySettingChanged;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x060039F9 RID: 14841 RVA: 0x0013BE9C File Offset: 0x0013A09C
	// (remove) Token: 0x060039FA RID: 14842 RVA: 0x0013BED4 File Offset: 0x0013A0D4
	public event Action<SettingConfig, SettingLevel> OnStorySettingChanged;

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x060039FB RID: 14843 RVA: 0x0013BF0C File Offset: 0x0013A10C
	// (remove) Token: 0x060039FC RID: 14844 RVA: 0x0013BF44 File Offset: 0x0013A144
	public event Action<SettingConfig, SettingLevel> OnMixingSettingChanged;

	// Token: 0x060039FD RID: 14845 RVA: 0x0013BF7C File Offset: 0x0013A17C
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 6))
		{
			this.customGameMode = (this.is_custom_game ? CustomGameSettings.CustomGameMode.Custom : CustomGameSettings.CustomGameMode.Survival);
		}
		if (this.CurrentQualityLevelsBySetting.ContainsKey("CarePackages "))
		{
			if (!this.CurrentQualityLevelsBySetting.ContainsKey(CustomGameSettingConfigs.CarePackages.id))
			{
				this.CurrentQualityLevelsBySetting.Add(CustomGameSettingConfigs.CarePackages.id, this.CurrentQualityLevelsBySetting["CarePackages "]);
			}
			this.CurrentQualityLevelsBySetting.Remove("CarePackages ");
		}
		this.CurrentQualityLevelsBySetting.Remove("Expansion1Active");
		string clusterDefaultName;
		this.CurrentQualityLevelsBySetting.TryGetValue(CustomGameSettingConfigs.ClusterLayout.id, out clusterDefaultName);
		if (clusterDefaultName.IsNullOrWhiteSpace())
		{
			if (!DlcManager.IsExpansion1Active())
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Deserializing CustomGameSettings.ClusterLayout: ClusterLayout is blank, using default cluster instead"
				});
			}
			clusterDefaultName = WorldGenSettings.ClusterDefaultName;
			this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, clusterDefaultName);
		}
		if (!SettingsCache.clusterLayouts.clusterCache.ContainsKey(clusterDefaultName))
		{
			global::Debug.Log("Deserializing CustomGameSettings.ClusterLayout: '" + clusterDefaultName + "' doesn't exist in the clusterCache, trying to rewrite path to scoped path.");
			string text = SettingsCache.GetScope("EXPANSION1_ID") + clusterDefaultName;
			if (SettingsCache.clusterLayouts.clusterCache.ContainsKey(text))
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"Deserializing CustomGameSettings.ClusterLayout: Success in rewriting ClusterLayout '",
					clusterDefaultName,
					"' to '",
					text,
					"'"
				}));
				this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, text);
			}
			else
			{
				global::Debug.LogWarning("Deserializing CustomGameSettings.ClusterLayout: Failed to find cluster '" + clusterDefaultName + "' including the scoped path, setting to default cluster name.");
				global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
				this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, WorldGenSettings.ClusterDefaultName);
			}
		}
		this.CheckCustomGameMode();
	}

	// Token: 0x060039FE RID: 14846 RVA: 0x0013C150 File Offset: 0x0013A350
	private void AddMissingQualitySettings()
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			SettingConfig value = keyValuePair.Value;
			if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(value.required_content) && !this.CurrentQualityLevelsBySetting.ContainsKey(value.id))
			{
				if (value.missing_content_default != "")
				{
					DebugUtil.LogArgs(new object[]
					{
						string.Concat(new string[]
						{
							"QualitySetting '",
							value.id,
							"' is missing, setting it to missing_content_default '",
							value.missing_content_default,
							"'."
						})
					});
					this.SetQualitySetting(value, value.missing_content_default);
				}
				else
				{
					DebugUtil.DevLogError("QualitySetting '" + value.id + "' is missing in this save. Either provide a missing_content_default or handle it in OnDeserialized.");
				}
			}
		}
	}

	// Token: 0x060039FF RID: 14847 RVA: 0x0013C250 File Offset: 0x0013A450
	protected override void OnPrefabInit()
	{
		DlcManager.IsExpansion1Active();
		Action<SettingConfig> action = delegate(SettingConfig setting)
		{
			this.AddQualitySettingConfig(setting);
			if (setting.coordinate_range >= 0L)
			{
				this.CoordinatedQualitySettings.Add(setting.id);
			}
		};
		Action<SettingConfig> action2 = delegate(SettingConfig setting)
		{
			this.AddStorySettingConfig(setting);
			if (setting.coordinate_range >= 0L)
			{
				this.CoordinatedStorySettings.Add(setting.id);
			}
		};
		Action<SettingConfig> action3 = delegate(SettingConfig setting)
		{
			this.AddMixingSettingsConfig(setting);
			if (setting.coordinate_range >= 0L)
			{
				this.CoordinatedMixingSettings.Add(setting.id);
			}
		};
		CustomGameSettings.instance = this;
		action(CustomGameSettingConfigs.ClusterLayout);
		action(CustomGameSettingConfigs.WorldgenSeed);
		action(CustomGameSettingConfigs.ImmuneSystem);
		action(CustomGameSettingConfigs.CalorieBurn);
		action(CustomGameSettingConfigs.Morale);
		action(CustomGameSettingConfigs.Durability);
		action(CustomGameSettingConfigs.MeteorShowers);
		action(CustomGameSettingConfigs.Radiation);
		action(CustomGameSettingConfigs.Stress);
		action(CustomGameSettingConfigs.StressBreaks);
		action(CustomGameSettingConfigs.CarePackages);
		action(CustomGameSettingConfigs.SandboxMode);
		action(CustomGameSettingConfigs.FastWorkersMode);
		action(CustomGameSettingConfigs.SaveToCloud);
		action(CustomGameSettingConfigs.Teleporters);
		action3(CustomMixingSettingsConfigs.DLC2Mixing);
		action3(CustomMixingSettingsConfigs.IceCavesMixing);
		action3(CustomMixingSettingsConfigs.CarrotQuarryMixing);
		action3(CustomMixingSettingsConfigs.SugarWoodsMixing);
		action3(CustomMixingSettingsConfigs.CeresAsteroidMixing);
		action3(CustomMixingSettingsConfigs.DLC3Mixing);
		foreach (Story story in Db.Get().Stories.GetStoriesSortedByCoordinateOrder())
		{
			int num = (story.kleiUseOnlyCoordinateOrder == -1) ? -1 : 3;
			SettingConfig obj = new ListSettingConfig(story.Id, "", "", new List<SettingLevel>
			{
				new SettingLevel("Disabled", "", "", 0L, null),
				new SettingLevel("Guaranteed", "", "", 1L, null)
			}, "Disabled", "Disabled", (long)num, false, false, null, "", false);
			action2(obj);
		}
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
			if (dlcMixingSettingConfig != null && DlcManager.IsContentSubscribed(dlcMixingSettingConfig.id))
			{
				this.SetMixingSetting(dlcMixingSettingConfig, "Enabled");
			}
		}
		this.VerifySettingCoordinates();
	}

	// Token: 0x06003A00 RID: 14848 RVA: 0x0013C4B4 File Offset: 0x0013A6B4
	public void DisableAllStories()
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.StorySettings)
		{
			this.SetStorySetting(keyValuePair.Value, false);
		}
	}

	// Token: 0x06003A01 RID: 14849 RVA: 0x0013C510 File Offset: 0x0013A710
	public void SetSurvivalDefaults()
	{
		this.customGameMode = CustomGameSettings.CustomGameMode.Survival;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			this.SetQualitySetting(keyValuePair.Value, keyValuePair.Value.GetDefaultLevelId());
		}
	}

	// Token: 0x06003A02 RID: 14850 RVA: 0x0013C57C File Offset: 0x0013A77C
	public void SetNosweatDefaults()
	{
		this.customGameMode = CustomGameSettings.CustomGameMode.Nosweat;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			this.SetQualitySetting(keyValuePair.Value, keyValuePair.Value.GetNoSweatDefaultLevelId());
		}
	}

	// Token: 0x06003A03 RID: 14851 RVA: 0x0013C5E8 File Offset: 0x0013A7E8
	public SettingLevel CycleQualitySettingLevel(ListSettingConfig config, int direction)
	{
		this.SetQualitySetting(config, config.CycleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id], direction));
		return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
	}

	// Token: 0x06003A04 RID: 14852 RVA: 0x0013C620 File Offset: 0x0013A820
	public SettingLevel ToggleQualitySettingLevel(ToggleSettingConfig config)
	{
		this.SetQualitySetting(config, config.ToggleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id]));
		return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
	}

	// Token: 0x06003A05 RID: 14853 RVA: 0x0013C658 File Offset: 0x0013A858
	private void CheckCustomGameMode()
	{
		bool flag = true;
		bool flag2 = true;
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			if (!this.QualitySettings.ContainsKey(keyValuePair.Key))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Quality settings missing " + keyValuePair.Key
				});
			}
			else if (this.QualitySettings[keyValuePair.Key].triggers_custom_game)
			{
				if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].GetDefaultLevelId())
				{
					flag = false;
				}
				if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].GetNoSweatDefaultLevelId())
				{
					flag2 = false;
				}
				if (!flag && !flag2)
				{
					break;
				}
			}
		}
		CustomGameSettings.CustomGameMode customGameMode;
		if (flag)
		{
			customGameMode = CustomGameSettings.CustomGameMode.Survival;
		}
		else if (flag2)
		{
			customGameMode = CustomGameSettings.CustomGameMode.Nosweat;
		}
		else
		{
			customGameMode = CustomGameSettings.CustomGameMode.Custom;
		}
		if (customGameMode != this.customGameMode)
		{
			DebugUtil.LogArgs(new object[]
			{
				"Game mode changed from",
				this.customGameMode,
				"to",
				customGameMode
			});
			this.customGameMode = customGameMode;
		}
	}

	// Token: 0x06003A06 RID: 14854 RVA: 0x0013C7AC File Offset: 0x0013A9AC
	public void SetQualitySetting(SettingConfig config, string value)
	{
		this.SetQualitySetting(config, value, true);
	}

	// Token: 0x06003A07 RID: 14855 RVA: 0x0013C7B7 File Offset: 0x0013A9B7
	public void SetQualitySetting(SettingConfig config, string value, bool notify)
	{
		this.CurrentQualityLevelsBySetting[config.id] = value;
		this.CheckCustomGameMode();
		if (notify && this.OnQualitySettingChanged != null)
		{
			this.OnQualitySettingChanged(config, this.GetCurrentQualitySetting(config));
		}
	}

	// Token: 0x06003A08 RID: 14856 RVA: 0x0013C7EF File Offset: 0x0013A9EF
	public SettingLevel GetCurrentQualitySetting(SettingConfig setting)
	{
		return this.GetCurrentQualitySetting(setting.id);
	}

	// Token: 0x06003A09 RID: 14857 RVA: 0x0013C800 File Offset: 0x0013AA00
	public SettingLevel GetCurrentQualitySetting(string setting_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetDefaultLevelId());
		}
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetNoSweatDefaultLevelId());
		}
		if (!this.CurrentQualityLevelsBySetting.ContainsKey(setting_id))
		{
			this.CurrentQualityLevelsBySetting[setting_id] = this.QualitySettings[setting_id].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsAllContentSubscribed(settingConfig.required_content) ? this.CurrentQualityLevelsBySetting[setting_id] : settingConfig.GetDefaultLevelId();
		return this.QualitySettings[setting_id].GetLevel(level_id);
	}

	// Token: 0x06003A0A RID: 14858 RVA: 0x0013C8B4 File Offset: 0x0013AAB4
	public string GetCurrentQualitySettingLevelId(SettingConfig config)
	{
		return this.CurrentQualityLevelsBySetting[config.id];
	}

	// Token: 0x06003A0B RID: 14859 RVA: 0x0013C8C8 File Offset: 0x0013AAC8
	public string GetSettingLevelLabel(string setting_id, string level_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (settingConfig != null)
		{
			SettingLevel level = settingConfig.GetLevel(level_id);
			if (level != null)
			{
				return level.label;
			}
		}
		global::Debug.LogWarning("No label string for setting: " + setting_id + " level: " + level_id);
		return "";
	}

	// Token: 0x06003A0C RID: 14860 RVA: 0x0013C914 File Offset: 0x0013AB14
	public string GetQualitySettingLevelTooltip(string setting_id, string level_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (settingConfig != null)
		{
			SettingLevel level = settingConfig.GetLevel(level_id);
			if (level != null)
			{
				return level.tooltip;
			}
		}
		global::Debug.LogWarning("No tooltip string for setting: " + setting_id + " level: " + level_id);
		return "";
	}

	// Token: 0x06003A0D RID: 14861 RVA: 0x0013C960 File Offset: 0x0013AB60
	public void AddQualitySettingConfig(SettingConfig config)
	{
		this.QualitySettings.Add(config.id, config);
		if (!this.CurrentQualityLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.CurrentQualityLevelsBySetting[config.id]))
		{
			this.CurrentQualityLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x06003A0E RID: 14862 RVA: 0x0013C9C4 File Offset: 0x0013ABC4
	public void AddStorySettingConfig(SettingConfig config)
	{
		this.StorySettings.Add(config.id, config);
		if (!this.currentStoryLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.currentStoryLevelsBySetting[config.id]))
		{
			this.currentStoryLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x06003A0F RID: 14863 RVA: 0x0013CA25 File Offset: 0x0013AC25
	public void SetStorySetting(SettingConfig config, string value)
	{
		this.SetStorySetting(config, value == "Guaranteed");
	}

	// Token: 0x06003A10 RID: 14864 RVA: 0x0013CA39 File Offset: 0x0013AC39
	public void SetStorySetting(SettingConfig config, bool value)
	{
		this.currentStoryLevelsBySetting[config.id] = (value ? "Guaranteed" : "Disabled");
		if (this.OnStorySettingChanged != null)
		{
			this.OnStorySettingChanged(config, this.GetCurrentStoryTraitSetting(config));
		}
	}

	// Token: 0x06003A11 RID: 14865 RVA: 0x0013CA78 File Offset: 0x0013AC78
	public void ParseAndApplyStoryTraitSettingsCode(string code)
	{
		BigInteger dividend = this.Base36toBinary(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (object obj in global::Util.Reverse(this.CoordinatedStorySettings))
		{
			string key = (string)obj;
			SettingConfig settingConfig = this.StorySettings[key];
			if (settingConfig.coordinate_range != -1L)
			{
				long num = (long)(dividend % settingConfig.coordinate_range);
				dividend /= settingConfig.coordinate_range;
				foreach (SettingLevel settingLevel in settingConfig.GetLevels())
				{
					if (settingLevel.coordinate_value == num)
					{
						dictionary[settingConfig] = settingLevel.id;
						break;
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
		{
			this.SetStorySetting(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06003A12 RID: 14866 RVA: 0x0013CBD4 File Offset: 0x0013ADD4
	private string GetStoryTraitSettingsCode()
	{
		BigInteger bigInteger = 0;
		foreach (string key in this.CoordinatedStorySettings)
		{
			SettingConfig settingConfig = this.StorySettings[key];
			bigInteger *= settingConfig.coordinate_range;
			bigInteger += settingConfig.GetLevel(this.currentStoryLevelsBySetting[key]).coordinate_value;
		}
		return this.BinarytoBase36(bigInteger);
	}

	// Token: 0x06003A13 RID: 14867 RVA: 0x0013CC70 File Offset: 0x0013AE70
	public SettingLevel GetCurrentStoryTraitSetting(SettingConfig setting)
	{
		return this.GetCurrentStoryTraitSetting(setting.id);
	}

	// Token: 0x06003A14 RID: 14868 RVA: 0x0013CC80 File Offset: 0x0013AE80
	public SettingLevel GetCurrentStoryTraitSetting(string settingId)
	{
		SettingConfig settingConfig = this.StorySettings[settingId];
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetDefaultLevelId());
		}
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetNoSweatDefaultLevelId());
		}
		if (!this.currentStoryLevelsBySetting.ContainsKey(settingId))
		{
			this.currentStoryLevelsBySetting[settingId] = this.StorySettings[settingId].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsAllContentSubscribed(settingConfig.required_content) ? this.currentStoryLevelsBySetting[settingId] : settingConfig.GetDefaultLevelId();
		return this.StorySettings[settingId].GetLevel(level_id);
	}

	// Token: 0x06003A15 RID: 14869 RVA: 0x0013CD34 File Offset: 0x0013AF34
	public List<string> GetCurrentStories()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> keyValuePair in this.currentStoryLevelsBySetting)
		{
			if (this.IsStoryActive(keyValuePair.Key, keyValuePair.Value))
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x06003A16 RID: 14870 RVA: 0x0013CDAC File Offset: 0x0013AFAC
	public bool IsStoryActive(string id, string level)
	{
		SettingConfig settingConfig;
		return this.StorySettings.TryGetValue(id, out settingConfig) && settingConfig != null && level == "Guaranteed";
	}

	// Token: 0x06003A17 RID: 14871 RVA: 0x0013CDDB File Offset: 0x0013AFDB
	public void SetMixingSetting(SettingConfig config, string value)
	{
		this.SetMixingSetting(config, value, true);
	}

	// Token: 0x06003A18 RID: 14872 RVA: 0x0013CDE6 File Offset: 0x0013AFE6
	public void SetMixingSetting(SettingConfig config, string value, bool notify)
	{
		this.CurrentMixingLevelsBySetting[config.id] = value;
		if (notify && this.OnMixingSettingChanged != null)
		{
			this.OnMixingSettingChanged(config, this.GetCurrentMixingSettingLevel(config));
		}
	}

	// Token: 0x06003A19 RID: 14873 RVA: 0x0013CE18 File Offset: 0x0013B018
	public void AddMixingSettingsConfig(SettingConfig config)
	{
		this.MixingSettings.Add(config.id, config);
		if (!this.CurrentMixingLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.CurrentMixingLevelsBySetting[config.id]))
		{
			this.CurrentMixingLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x06003A1A RID: 14874 RVA: 0x0013CE79 File Offset: 0x0013B079
	public SettingLevel GetCurrentMixingSettingLevel(SettingConfig setting)
	{
		return this.GetCurrentMixingSettingLevel(setting.id);
	}

	// Token: 0x06003A1B RID: 14875 RVA: 0x0013CE88 File Offset: 0x0013B088
	public SettingConfig GetWorldMixingSettingForWorldgenFile(string file)
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			WorldMixingSettingConfig worldMixingSettingConfig = keyValuePair.Value as WorldMixingSettingConfig;
			if (worldMixingSettingConfig != null && worldMixingSettingConfig.worldgenPath == file)
			{
				return keyValuePair.Value;
			}
		}
		return null;
	}

	// Token: 0x06003A1C RID: 14876 RVA: 0x0013CF00 File Offset: 0x0013B100
	public SettingConfig GetSubworldMixingSettingForWorldgenFile(string file)
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			SubworldMixingSettingConfig subworldMixingSettingConfig = keyValuePair.Value as SubworldMixingSettingConfig;
			if (subworldMixingSettingConfig != null && subworldMixingSettingConfig.worldgenPath == file)
			{
				return keyValuePair.Value;
			}
		}
		return null;
	}

	// Token: 0x06003A1D RID: 14877 RVA: 0x0013CF78 File Offset: 0x0013B178
	public void DisableAllMixing()
	{
		foreach (SettingConfig settingConfig in this.MixingSettings.Values)
		{
			this.SetMixingSetting(settingConfig, settingConfig.GetDefaultLevelId());
		}
	}

	// Token: 0x06003A1E RID: 14878 RVA: 0x0013CFD8 File Offset: 0x0013B1D8
	public List<SubworldMixingSettingConfig> GetActiveSubworldMixingSettings()
	{
		List<SubworldMixingSettingConfig> list = new List<SubworldMixingSettingConfig>();
		foreach (SettingConfig settingConfig in this.MixingSettings.Values)
		{
			SubworldMixingSettingConfig subworldMixingSettingConfig = settingConfig as SubworldMixingSettingConfig;
			if (subworldMixingSettingConfig != null && this.GetCurrentMixingSettingLevel(settingConfig).id != "Disabled")
			{
				list.Add(subworldMixingSettingConfig);
			}
		}
		return list;
	}

	// Token: 0x06003A1F RID: 14879 RVA: 0x0013D05C File Offset: 0x0013B25C
	public List<WorldMixingSettingConfig> GetActiveWorldMixingSettings()
	{
		List<WorldMixingSettingConfig> list = new List<WorldMixingSettingConfig>();
		foreach (SettingConfig settingConfig in this.MixingSettings.Values)
		{
			WorldMixingSettingConfig worldMixingSettingConfig = settingConfig as WorldMixingSettingConfig;
			if (worldMixingSettingConfig != null && this.GetCurrentMixingSettingLevel(settingConfig).id != "Disabled")
			{
				list.Add(worldMixingSettingConfig);
			}
		}
		return list;
	}

	// Token: 0x06003A20 RID: 14880 RVA: 0x0013D0E0 File Offset: 0x0013B2E0
	public SettingLevel CycleMixingSettingLevel(ListSettingConfig config, int direction)
	{
		this.SetMixingSetting(config, config.CycleSettingLevelID(this.CurrentMixingLevelsBySetting[config.id], direction));
		return config.GetLevel(this.CurrentMixingLevelsBySetting[config.id]);
	}

	// Token: 0x06003A21 RID: 14881 RVA: 0x0013D118 File Offset: 0x0013B318
	public SettingLevel ToggleMixingSettingLevel(ToggleSettingConfig config)
	{
		this.SetMixingSetting(config, config.ToggleSettingLevelID(this.CurrentMixingLevelsBySetting[config.id]));
		return config.GetLevel(this.CurrentMixingLevelsBySetting[config.id]);
	}

	// Token: 0x06003A22 RID: 14882 RVA: 0x0013D150 File Offset: 0x0013B350
	public SettingLevel GetCurrentMixingSettingLevel(string settingId)
	{
		SettingConfig settingConfig = this.MixingSettings[settingId];
		if (!this.CurrentMixingLevelsBySetting.ContainsKey(settingId))
		{
			this.CurrentMixingLevelsBySetting[settingId] = this.MixingSettings[settingId].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsAllContentSubscribed(settingConfig.required_content) ? this.CurrentMixingLevelsBySetting[settingId] : settingConfig.GetDefaultLevelId();
		return this.MixingSettings[settingId].GetLevel(level_id);
	}

	// Token: 0x06003A23 RID: 14883 RVA: 0x0013D1CC File Offset: 0x0013B3CC
	public List<string> GetCurrentDlcMixingIds()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
			if (dlcMixingSettingConfig != null && dlcMixingSettingConfig.IsOnLevel(this.GetCurrentMixingSettingLevel(dlcMixingSettingConfig.id).id))
			{
				list.Add(dlcMixingSettingConfig.id);
			}
		}
		return list;
	}

	// Token: 0x06003A24 RID: 14884 RVA: 0x0013D254 File Offset: 0x0013B454
	public void ParseAndApplyMixingSettingsCode(string code)
	{
		BigInteger dividend = this.Base36toBinary(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (object obj in global::Util.Reverse(this.CoordinatedMixingSettings))
		{
			string key = (string)obj;
			SettingConfig settingConfig = this.MixingSettings[key];
			if (settingConfig.coordinate_range != -1L)
			{
				long num = (long)(dividend % settingConfig.coordinate_range);
				dividend /= settingConfig.coordinate_range;
				foreach (SettingLevel settingLevel in settingConfig.GetLevels())
				{
					if (settingLevel.coordinate_value == num)
					{
						dictionary[settingConfig] = settingLevel.id;
						break;
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
		{
			this.SetMixingSetting(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06003A25 RID: 14885 RVA: 0x0013D3B0 File Offset: 0x0013B5B0
	private string GetMixingSettingsCode()
	{
		BigInteger bigInteger = 0;
		foreach (string key in this.CoordinatedMixingSettings)
		{
			SettingConfig settingConfig = this.MixingSettings[key];
			bigInteger *= settingConfig.coordinate_range;
			bigInteger += settingConfig.GetLevel(this.GetCurrentMixingSettingLevel(settingConfig).id).coordinate_value;
		}
		return this.BinarytoBase36(bigInteger);
	}

	// Token: 0x06003A26 RID: 14886 RVA: 0x0013D44C File Offset: 0x0013B64C
	public void RemoveInvalidMixingSettings()
	{
		ClusterLayout currentClusterLayout = this.GetCurrentClusterLayout();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
			if (dlcMixingSettingConfig != null && currentClusterLayout.requiredDlcIds.Contains(dlcMixingSettingConfig.id))
			{
				this.SetMixingSetting(keyValuePair.Value, "Disabled");
			}
		}
		CustomGameSettings.<>c__DisplayClass71_0 CS$<>8__locals1;
		CS$<>8__locals1.availableDlcs = this.GetCurrentDlcMixingIds();
		CS$<>8__locals1.availableDlcs.AddRange(currentClusterLayout.requiredDlcIds);
		foreach (KeyValuePair<string, SettingConfig> keyValuePair2 in this.MixingSettings)
		{
			SettingConfig value = keyValuePair2.Value;
			WorldMixingSettingConfig worldMixingSettingConfig = value as WorldMixingSettingConfig;
			if (worldMixingSettingConfig == null)
			{
				SubworldMixingSettingConfig subworldMixingSettingConfig = value as SubworldMixingSettingConfig;
				if (subworldMixingSettingConfig != null)
				{
					if (!CustomGameSettings.<RemoveInvalidMixingSettings>g__HasRequiredContent|71_0(subworldMixingSettingConfig.required_content, ref CS$<>8__locals1) || currentClusterLayout.HasAnyTags(subworldMixingSettingConfig.forbiddenClusterTags))
					{
						this.SetMixingSetting(keyValuePair2.Value, "Disabled");
					}
				}
			}
			else if (!CustomGameSettings.<RemoveInvalidMixingSettings>g__HasRequiredContent|71_0(worldMixingSettingConfig.required_content, ref CS$<>8__locals1) || currentClusterLayout.HasAnyTags(worldMixingSettingConfig.forbiddenClusterTags))
			{
				this.SetMixingSetting(keyValuePair2.Value, "Disabled");
			}
		}
	}

	// Token: 0x06003A27 RID: 14887 RVA: 0x0013D5C0 File Offset: 0x0013B7C0
	public ClusterLayout GetCurrentClusterLayout()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (currentQualitySetting == null)
		{
			return null;
		}
		return SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
	}

	// Token: 0x06003A28 RID: 14888 RVA: 0x0013D5F4 File Offset: 0x0013B7F4
	public int GetCurrentWorldgenSeed()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
		if (currentQualitySetting == null)
		{
			return 0;
		}
		return int.Parse(currentQualitySetting.id);
	}

	// Token: 0x06003A29 RID: 14889 RVA: 0x0013D624 File Offset: 0x0013B824
	public void LoadClusters()
	{
		Dictionary<string, ClusterLayout> clusterCache = SettingsCache.clusterLayouts.clusterCache;
		List<SettingLevel> list = new List<SettingLevel>(clusterCache.Count);
		foreach (KeyValuePair<string, ClusterLayout> keyValuePair in clusterCache)
		{
			StringEntry stringEntry;
			string label = Strings.TryGet(new StringKey(keyValuePair.Value.name), out stringEntry) ? stringEntry.ToString() : keyValuePair.Value.name;
			string tooltip = Strings.TryGet(new StringKey(keyValuePair.Value.description), out stringEntry) ? stringEntry.ToString() : keyValuePair.Value.description;
			list.Add(new SettingLevel(keyValuePair.Key, label, tooltip, 0L, null));
		}
		CustomGameSettingConfigs.ClusterLayout.StompLevels(list, WorldGenSettings.ClusterDefaultName, WorldGenSettings.ClusterDefaultName);
	}

	// Token: 0x06003A2A RID: 14890 RVA: 0x0013D714 File Offset: 0x0013B914
	public void Print()
	{
		string text = "Custom Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair.Key,
				"=",
				keyValuePair.Value,
				","
			});
		}
		global::Debug.Log(text);
		text = "Story Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair2 in this.currentStoryLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair2.Key,
				"=",
				keyValuePair2.Value,
				","
			});
		}
		global::Debug.Log(text);
		text = "Mixing Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair3 in this.CurrentMixingLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair3.Key,
				"=",
				keyValuePair3.Value,
				","
			});
		}
		global::Debug.Log(text);
	}

	// Token: 0x06003A2B RID: 14891 RVA: 0x0013D898 File Offset: 0x0013BA98
	private bool AllValuesMatch(Dictionary<string, string> data, CustomGameSettings.CustomGameMode mode)
	{
		bool result = true;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			if (!(keyValuePair.Key == CustomGameSettingConfigs.WorldgenSeed.id))
			{
				string b = null;
				if (mode != CustomGameSettings.CustomGameMode.Survival)
				{
					if (mode == CustomGameSettings.CustomGameMode.Nosweat)
					{
						b = keyValuePair.Value.GetNoSweatDefaultLevelId();
					}
				}
				else
				{
					b = keyValuePair.Value.GetDefaultLevelId();
				}
				if (data.ContainsKey(keyValuePair.Key) && data[keyValuePair.Key] != b)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003A2C RID: 14892 RVA: 0x0013D94C File Offset: 0x0013BB4C
	public List<CustomGameSettings.MetricSettingsData> GetSettingsForMetrics()
	{
		List<CustomGameSettings.MetricSettingsData> list = new List<CustomGameSettings.MetricSettingsData>();
		list.Add(new CustomGameSettings.MetricSettingsData
		{
			Name = "CustomGameMode",
			Value = this.customGameMode.ToString()
		});
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			list.Add(new CustomGameSettings.MetricSettingsData
			{
				Name = keyValuePair.Key,
				Value = keyValuePair.Value
			});
		}
		CustomGameSettings.MetricSettingsData item = new CustomGameSettings.MetricSettingsData
		{
			Name = "CustomGameModeActual",
			Value = CustomGameSettings.CustomGameMode.Custom.ToString()
		};
		foreach (object obj in Enum.GetValues(typeof(CustomGameSettings.CustomGameMode)))
		{
			CustomGameSettings.CustomGameMode customGameMode = (CustomGameSettings.CustomGameMode)obj;
			if (customGameMode != CustomGameSettings.CustomGameMode.Custom && this.AllValuesMatch(this.CurrentQualityLevelsBySetting, customGameMode))
			{
				item.Value = customGameMode.ToString();
				break;
			}
		}
		list.Add(item);
		return list;
	}

	// Token: 0x06003A2D RID: 14893 RVA: 0x0013DAB8 File Offset: 0x0013BCB8
	public List<CustomGameSettings.MetricSettingsData> GetSettingsForMixingMetrics()
	{
		List<CustomGameSettings.MetricSettingsData> list = new List<CustomGameSettings.MetricSettingsData>();
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentMixingLevelsBySetting)
		{
			if (DlcManager.IsAllContentSubscribed(this.MixingSettings[keyValuePair.Key].required_content))
			{
				list.Add(new CustomGameSettings.MetricSettingsData
				{
					Name = keyValuePair.Key,
					Value = keyValuePair.Value
				});
			}
		}
		return list;
	}

	// Token: 0x06003A2E RID: 14894 RVA: 0x0013DB54 File Offset: 0x0013BD54
	public bool VerifySettingCoordinates()
	{
		bool flag = this.VerifySettingsDictionary(this.QualitySettings);
		bool flag2 = this.VerifySettingsDictionary(this.StorySettings);
		return flag || flag2;
	}

	// Token: 0x06003A2F RID: 14895 RVA: 0x0013DB7C File Offset: 0x0013BD7C
	private bool VerifySettingsDictionary(Dictionary<string, SettingConfig> configs)
	{
		bool result = false;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in configs)
		{
			if (keyValuePair.Value.coordinate_range >= 0L)
			{
				List<SettingLevel> levels = keyValuePair.Value.GetLevels();
				if (keyValuePair.Value.coordinate_range < (long)levels.Count)
				{
					result = true;
					global::Debug.Assert(false, string.Concat(new string[]
					{
						keyValuePair.Value.id,
						": Range between coordinate min and max insufficient for all levels (",
						keyValuePair.Value.coordinate_range.ToString(),
						"<",
						levels.Count.ToString(),
						")"
					}));
				}
				foreach (SettingLevel settingLevel in levels)
				{
					Dictionary<long, string> dictionary = new Dictionary<long, string>();
					string text = keyValuePair.Value.id + " > " + settingLevel.id;
					if (keyValuePair.Value.coordinate_range <= settingLevel.coordinate_value)
					{
						result = true;
						global::Debug.Assert(false, string.Format("%s: Level coordinate value (%u) exceedes range (%u)", text, settingLevel.coordinate_value, keyValuePair.Value.coordinate_range));
					}
					if (settingLevel.coordinate_value < 0L)
					{
						result = true;
						global::Debug.Assert(false, text + ": Level coordinate value must be >= 0");
					}
					else if (settingLevel.coordinate_value == 0L)
					{
						if (settingLevel.id != keyValuePair.Value.GetDefaultLevelId())
						{
							result = true;
							global::Debug.Assert(false, text + ": Only the default level should have a coordinate value of 0");
						}
					}
					else
					{
						string str;
						bool flag = !dictionary.TryGetValue(settingLevel.coordinate_value, out str);
						dictionary[settingLevel.coordinate_value] = text;
						if (settingLevel.id == keyValuePair.Value.GetDefaultLevelId())
						{
							result = true;
							global::Debug.Assert(false, text + ": Default level must be a coordinate value of 0");
						}
						if (!flag)
						{
							result = true;
							global::Debug.Assert(false, text + ": Combined coordinate conflicts with another coordinate (" + str + "). Ensure this SettingConfig's min and max don't overlap with another SettingConfig's");
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06003A30 RID: 14896 RVA: 0x0013DDFC File Offset: 0x0013BFFC
	public static string[] ParseSettingCoordinate(string coord)
	{
		Match match = new Regex("(.*)-(\\d*)-(.*)-(.*)-(.*)").Match(coord);
		for (int i = 1; i <= 2; i++)
		{
			if (match.Groups.Count == 1)
			{
				match = new Regex("(.*)-(\\d*)-(.*)-(.*)-(.*)".Remove("(.*)-(\\d*)-(.*)-(.*)-(.*)".Length - i * 5)).Match(coord);
			}
		}
		string[] array = new string[match.Groups.Count];
		for (int j = 0; j < match.Groups.Count; j++)
		{
			array[j] = match.Groups[j].Value;
		}
		return array;
	}

	// Token: 0x06003A31 RID: 14897 RVA: 0x0013DE94 File Offset: 0x0013C094
	public string GetSettingsCoordinate()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (currentQualitySetting == null)
		{
			DebugUtil.DevLogError("GetSettingsCoordinate: clusterLayoutSetting is null, returning '0' coordinate");
			CustomGameSettings.Instance.Print();
			global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
			return "0-0-0-0-0";
		}
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
		SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
		string otherSettingsCode = this.GetOtherSettingsCode();
		string storyTraitSettingsCode = this.GetStoryTraitSettingsCode();
		string mixingSettingsCode = this.GetMixingSettingsCode();
		return string.Format("{0}-{1}-{2}-{3}-{4}", new object[]
		{
			clusterData.GetCoordinatePrefix(),
			currentQualitySetting2.id,
			otherSettingsCode,
			storyTraitSettingsCode,
			mixingSettingsCode
		});
	}

	// Token: 0x06003A32 RID: 14898 RVA: 0x0013DF60 File Offset: 0x0013C160
	public void ParseAndApplySettingsCode(string code)
	{
		BigInteger dividend = this.Base36toBinary(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (object obj in global::Util.Reverse(this.CoordinatedQualitySettings))
		{
			string key = (string)obj;
			if (this.QualitySettings.ContainsKey(key))
			{
				SettingConfig settingConfig = this.QualitySettings[key];
				if (settingConfig.coordinate_range != -1L)
				{
					long num = (long)(dividend % settingConfig.coordinate_range);
					dividend /= settingConfig.coordinate_range;
					foreach (SettingLevel settingLevel in settingConfig.GetLevels())
					{
						if (settingLevel.coordinate_value == num)
						{
							dictionary[settingConfig] = settingLevel.id;
							break;
						}
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
		{
			this.SetQualitySetting(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06003A33 RID: 14899 RVA: 0x0013E0CC File Offset: 0x0013C2CC
	private string GetOtherSettingsCode()
	{
		BigInteger bigInteger = 0;
		foreach (string text in this.CoordinatedQualitySettings)
		{
			SettingConfig settingConfig = this.QualitySettings[text];
			bigInteger *= settingConfig.coordinate_range;
			bigInteger += settingConfig.GetLevel(this.GetCurrentQualitySetting(text).id).coordinate_value;
		}
		return this.BinarytoBase36(bigInteger);
	}

	// Token: 0x06003A34 RID: 14900 RVA: 0x0013E168 File Offset: 0x0013C368
	private BigInteger Base36toBinary(string input)
	{
		if (input == "0")
		{
			return 0;
		}
		BigInteger bigInteger = 0;
		for (int i = input.Length - 1; i >= 0; i--)
		{
			bigInteger *= 36;
			long value = (long)this.hexChars.IndexOf(input[i]);
			bigInteger += value;
		}
		DebugUtil.LogArgs(new object[]
		{
			"tried converting",
			input,
			", got",
			bigInteger,
			"and returns to",
			this.BinarytoBase36(bigInteger)
		});
		return bigInteger;
	}

	// Token: 0x06003A35 RID: 14901 RVA: 0x0013E210 File Offset: 0x0013C410
	private string BinarytoBase36(BigInteger input)
	{
		if (input == 0L)
		{
			return "0";
		}
		BigInteger bigInteger = input;
		string text = "";
		while (bigInteger > 0L)
		{
			text += this.hexChars[(int)(bigInteger % 36)].ToString();
			bigInteger /= 36;
		}
		return text;
	}

	// Token: 0x06003A3A RID: 14906 RVA: 0x0013E36C File Offset: 0x0013C56C
	[CompilerGenerated]
	internal static bool <RemoveInvalidMixingSettings>g__HasRequiredContent|71_0(string[] requiredContent, ref CustomGameSettings.<>c__DisplayClass71_0 A_1)
	{
		foreach (string text in requiredContent)
		{
			if (!(text == "") && !A_1.availableDlcs.Contains(text))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040022EA RID: 8938
	private static CustomGameSettings instance;

	// Token: 0x040022EB RID: 8939
	public const long NO_COORDINATE_RANGE = -1L;

	// Token: 0x040022EC RID: 8940
	private const int NUM_STORY_LEVELS = 3;

	// Token: 0x040022ED RID: 8941
	public const string STORY_DISABLED_LEVEL = "Disabled";

	// Token: 0x040022EE RID: 8942
	public const string STORY_GUARANTEED_LEVEL = "Guaranteed";

	// Token: 0x040022EF RID: 8943
	[Serialize]
	public bool is_custom_game;

	// Token: 0x040022F0 RID: 8944
	[Serialize]
	public CustomGameSettings.CustomGameMode customGameMode;

	// Token: 0x040022F1 RID: 8945
	[Serialize]
	private Dictionary<string, string> CurrentQualityLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x040022F2 RID: 8946
	[Serialize]
	private Dictionary<string, string> CurrentMixingLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x040022F3 RID: 8947
	private Dictionary<string, string> currentStoryLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x040022F4 RID: 8948
	public List<string> CoordinatedQualitySettings = new List<string>();

	// Token: 0x040022F5 RID: 8949
	public Dictionary<string, SettingConfig> QualitySettings = new Dictionary<string, SettingConfig>();

	// Token: 0x040022F6 RID: 8950
	public List<string> CoordinatedStorySettings = new List<string>();

	// Token: 0x040022F7 RID: 8951
	public Dictionary<string, SettingConfig> StorySettings = new Dictionary<string, SettingConfig>();

	// Token: 0x040022F8 RID: 8952
	public List<string> CoordinatedMixingSettings = new List<string>();

	// Token: 0x040022F9 RID: 8953
	public Dictionary<string, SettingConfig> MixingSettings = new Dictionary<string, SettingConfig>();

	// Token: 0x040022FD RID: 8957
	private const string coordinatePatern = "(.*)-(\\d*)-(.*)-(.*)-(.*)";

	// Token: 0x040022FE RID: 8958
	private string hexChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	// Token: 0x0200174E RID: 5966
	public enum CustomGameMode
	{
		// Token: 0x04007269 RID: 29289
		Survival,
		// Token: 0x0400726A RID: 29290
		Nosweat,
		// Token: 0x0400726B RID: 29291
		Custom = 255
	}

	// Token: 0x0200174F RID: 5967
	public struct MetricSettingsData
	{
		// Token: 0x0400726C RID: 29292
		public string Name;

		// Token: 0x0400726D RID: 29293
		public string Value;
	}
}
