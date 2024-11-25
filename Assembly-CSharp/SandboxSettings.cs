using System;
using System.Collections.Generic;

// Token: 0x02000D2B RID: 3371
public class SandboxSettings
{
	// Token: 0x060069C7 RID: 27079 RVA: 0x0027CE7D File Offset: 0x0027B07D
	public void AddIntSetting(string prefsKey, Action<int> setAction, int defaultValue)
	{
		this.intSettings.Add(new SandboxSettings.Setting<int>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x060069C8 RID: 27080 RVA: 0x0027CE92 File Offset: 0x0027B092
	public int GetIntSetting(string prefsKey)
	{
		return KPlayerPrefs.GetInt(prefsKey);
	}

	// Token: 0x060069C9 RID: 27081 RVA: 0x0027CE9C File Offset: 0x0027B09C
	public void SetIntSetting(string prefsKey, int value)
	{
		SandboxSettings.Setting<int> setting = this.intSettings.Find((SandboxSettings.Setting<int> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No intSetting named: ",
				prefsKey,
				" could be found amongst ",
				this.intSettings.Count.ToString(),
				" int settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x060069CA RID: 27082 RVA: 0x0027CF1F File Offset: 0x0027B11F
	public void RestoreIntSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetIntSetting(prefsKey, this.GetIntSetting(prefsKey));
			return;
		}
		this.ForceDefaultIntSetting(prefsKey);
	}

	// Token: 0x060069CB RID: 27083 RVA: 0x0027CF40 File Offset: 0x0027B140
	public void ForceDefaultIntSetting(string prefsKey)
	{
		this.SetIntSetting(prefsKey, this.intSettings.Find((SandboxSettings.Setting<int> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x060069CC RID: 27084 RVA: 0x0027CF82 File Offset: 0x0027B182
	public void AddFloatSetting(string prefsKey, Action<float> setAction, float defaultValue)
	{
		this.floatSettings.Add(new SandboxSettings.Setting<float>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x060069CD RID: 27085 RVA: 0x0027CF97 File Offset: 0x0027B197
	public float GetFloatSetting(string prefsKey)
	{
		return KPlayerPrefs.GetFloat(prefsKey);
	}

	// Token: 0x060069CE RID: 27086 RVA: 0x0027CFA0 File Offset: 0x0027B1A0
	public void SetFloatSetting(string prefsKey, float value)
	{
		SandboxSettings.Setting<float> setting = this.floatSettings.Find((SandboxSettings.Setting<float> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No KPlayerPrefs float setting named: ",
				prefsKey,
				" could be found amongst ",
				this.floatSettings.Count.ToString(),
				" float settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x060069CF RID: 27087 RVA: 0x0027D023 File Offset: 0x0027B223
	public void RestoreFloatSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetFloatSetting(prefsKey, this.GetFloatSetting(prefsKey));
			return;
		}
		this.ForceDefaultFloatSetting(prefsKey);
	}

	// Token: 0x060069D0 RID: 27088 RVA: 0x0027D044 File Offset: 0x0027B244
	public void ForceDefaultFloatSetting(string prefsKey)
	{
		this.SetFloatSetting(prefsKey, this.floatSettings.Find((SandboxSettings.Setting<float> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x060069D1 RID: 27089 RVA: 0x0027D086 File Offset: 0x0027B286
	public void AddStringSetting(string prefsKey, Action<string> setAction, string defaultValue)
	{
		this.stringSettings.Add(new SandboxSettings.Setting<string>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x060069D2 RID: 27090 RVA: 0x0027D09B File Offset: 0x0027B29B
	public string GetStringSetting(string prefsKey)
	{
		return KPlayerPrefs.GetString(prefsKey);
	}

	// Token: 0x060069D3 RID: 27091 RVA: 0x0027D0A4 File Offset: 0x0027B2A4
	public void SetStringSetting(string prefsKey, string value)
	{
		SandboxSettings.Setting<string> setting = this.stringSettings.Find((SandboxSettings.Setting<string> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No KPlayerPrefs string setting named: ",
				prefsKey,
				" could be found amongst ",
				this.stringSettings.Count.ToString(),
				" settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x060069D4 RID: 27092 RVA: 0x0027D127 File Offset: 0x0027B327
	public void RestoreStringSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetStringSetting(prefsKey, this.GetStringSetting(prefsKey));
			return;
		}
		this.ForceDefaultStringSetting(prefsKey);
	}

	// Token: 0x060069D5 RID: 27093 RVA: 0x0027D148 File Offset: 0x0027B348
	public void ForceDefaultStringSetting(string prefsKey)
	{
		this.SetStringSetting(prefsKey, this.stringSettings.Find((SandboxSettings.Setting<string> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x060069D6 RID: 27094 RVA: 0x0027D18C File Offset: 0x0027B38C
	public SandboxSettings()
	{
		this.AddStringSetting("SandboxTools.SelectedEntity", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedEntity", data);
			this.OnChangeEntity();
		}, "MushBar");
		this.AddIntSetting("SandboxTools.SelectedElement", delegate(int data)
		{
			KPlayerPrefs.SetInt("SandboxTools.SelectedElement", data);
			this.OnChangeElement(this.hasRestoredElement);
			this.hasRestoredElement = true;
		}, (int)ElementLoader.GetElementIndex(SimHashes.Oxygen));
		this.AddStringSetting("SandboxTools.SelectedDisease", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedDisease", data);
			this.OnChangeDisease();
		}, Db.Get().Diseases.FoodGerms.Id);
		this.AddIntSetting("SandboxTools.DiseaseCount", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandboxTools.DiseaseCount", val);
			this.OnChangeDiseaseCount();
		}, 0);
		this.AddStringSetting("SandboxTools.SelectedStory", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedStory", data);
			this.OnChangeStory();
		}, Db.Get().Stories.resources[Db.Get().Stories.resources.Count - 1].Id);
		this.AddIntSetting("SandboxTools.BrushSize", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandboxTools.BrushSize", val);
			this.OnChangeBrushSize();
		}, 1);
		this.AddFloatSetting("SandboxTools.NoiseScale", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.NoiseScale", val);
			this.OnChangeNoiseScale();
		}, 1f);
		this.AddFloatSetting("SandboxTools.NoiseDensity", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.NoiseDensity", val);
			this.OnChangeNoiseDensity();
		}, 1f);
		this.AddFloatSetting("SandboxTools.Mass", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.Mass", val);
			this.OnChangeMass();
		}, 1f);
		this.AddFloatSetting("SandbosTools.Temperature", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.Temperature", val);
			this.OnChangeTemperature();
		}, 300f);
		this.AddFloatSetting("SandbosTools.TemperatureAdditive", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.TemperatureAdditive", val);
			this.OnChangeAdditiveTemperature();
		}, 5f);
		this.AddFloatSetting("SandbosTools.StressAdditive", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.StressAdditive", val);
			this.OnChangeAdditiveStress();
		}, 50f);
		this.AddIntSetting("SandbosTools.MoraleAdjustment", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandbosTools.MoraleAdjustment", val);
			this.OnChangeMoraleAdjustment();
		}, 50);
	}

	// Token: 0x060069D7 RID: 27095 RVA: 0x0027D368 File Offset: 0x0027B568
	public void RestorePrefs()
	{
		foreach (SandboxSettings.Setting<int> setting in this.intSettings)
		{
			this.RestoreIntSetting(setting.PrefsKey);
		}
		foreach (SandboxSettings.Setting<float> setting2 in this.floatSettings)
		{
			this.RestoreFloatSetting(setting2.PrefsKey);
		}
		foreach (SandboxSettings.Setting<string> setting3 in this.stringSettings)
		{
			this.RestoreStringSetting(setting3.PrefsKey);
		}
	}

	// Token: 0x04004810 RID: 18448
	private List<SandboxSettings.Setting<int>> intSettings = new List<SandboxSettings.Setting<int>>();

	// Token: 0x04004811 RID: 18449
	private List<SandboxSettings.Setting<float>> floatSettings = new List<SandboxSettings.Setting<float>>();

	// Token: 0x04004812 RID: 18450
	private List<SandboxSettings.Setting<string>> stringSettings = new List<SandboxSettings.Setting<string>>();

	// Token: 0x04004813 RID: 18451
	public bool InstantBuild = true;

	// Token: 0x04004814 RID: 18452
	private bool hasRestoredElement;

	// Token: 0x04004815 RID: 18453
	public Action<bool> OnChangeElement;

	// Token: 0x04004816 RID: 18454
	public System.Action OnChangeMass;

	// Token: 0x04004817 RID: 18455
	public System.Action OnChangeDisease;

	// Token: 0x04004818 RID: 18456
	public System.Action OnChangeDiseaseCount;

	// Token: 0x04004819 RID: 18457
	public System.Action OnChangeStory;

	// Token: 0x0400481A RID: 18458
	public System.Action OnChangeEntity;

	// Token: 0x0400481B RID: 18459
	public System.Action OnChangeBrushSize;

	// Token: 0x0400481C RID: 18460
	public System.Action OnChangeNoiseScale;

	// Token: 0x0400481D RID: 18461
	public System.Action OnChangeNoiseDensity;

	// Token: 0x0400481E RID: 18462
	public System.Action OnChangeTemperature;

	// Token: 0x0400481F RID: 18463
	public System.Action OnChangeAdditiveTemperature;

	// Token: 0x04004820 RID: 18464
	public System.Action OnChangeAdditiveStress;

	// Token: 0x04004821 RID: 18465
	public System.Action OnChangeMoraleAdjustment;

	// Token: 0x04004822 RID: 18466
	public const string KEY_SELECTED_ENTITY = "SandboxTools.SelectedEntity";

	// Token: 0x04004823 RID: 18467
	public const string KEY_SELECTED_ELEMENT = "SandboxTools.SelectedElement";

	// Token: 0x04004824 RID: 18468
	public const string KEY_SELECTED_DISEASE = "SandboxTools.SelectedDisease";

	// Token: 0x04004825 RID: 18469
	public const string KEY_DISEASE_COUNT = "SandboxTools.DiseaseCount";

	// Token: 0x04004826 RID: 18470
	public const string KEY_SELECTED_STORY = "SandboxTools.SelectedStory";

	// Token: 0x04004827 RID: 18471
	public const string KEY_BRUSH_SIZE = "SandboxTools.BrushSize";

	// Token: 0x04004828 RID: 18472
	public const string KEY_NOISE_SCALE = "SandboxTools.NoiseScale";

	// Token: 0x04004829 RID: 18473
	public const string KEY_NOISE_DENSITY = "SandboxTools.NoiseDensity";

	// Token: 0x0400482A RID: 18474
	public const string KEY_MASS = "SandboxTools.Mass";

	// Token: 0x0400482B RID: 18475
	public const string KEY_TEMPERATURE = "SandbosTools.Temperature";

	// Token: 0x0400482C RID: 18476
	public const string KEY_TEMPERATURE_ADDITIVE = "SandbosTools.TemperatureAdditive";

	// Token: 0x0400482D RID: 18477
	public const string KEY_STRESS_ADDITIVE = "SandbosTools.StressAdditive";

	// Token: 0x0400482E RID: 18478
	public const string KEY_MORALE_ADJUSTMENT = "SandbosTools.MoraleAdjustment";

	// Token: 0x02001E5C RID: 7772
	public class Setting<T>
	{
		// Token: 0x0600AB37 RID: 43831 RVA: 0x003A42C1 File Offset: 0x003A24C1
		public Setting(string prefsKey, Action<T> setAction, T defaultValue)
		{
			this.prefsKey = prefsKey;
			this.SetAction = setAction;
			this.defaultValue = defaultValue;
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x0600AB38 RID: 43832 RVA: 0x003A42DE File Offset: 0x003A24DE
		public string PrefsKey
		{
			get
			{
				return this.prefsKey;
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (set) Token: 0x0600AB39 RID: 43833 RVA: 0x003A42E6 File Offset: 0x003A24E6
		public T Value
		{
			set
			{
				this.SetAction(value);
			}
		}

		// Token: 0x04008A3B RID: 35387
		private string prefsKey;

		// Token: 0x04008A3C RID: 35388
		private Action<T> SetAction;

		// Token: 0x04008A3D RID: 35389
		public T defaultValue;
	}
}
