using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Klei.CustomSettings;
using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02000A77 RID: 2679
[SerializationConfig(KSerialization.MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SaveGame")]
public class SaveGame : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x06004DF5 RID: 19957 RVA: 0x001BFC8B File Offset: 0x001BDE8B
	// (set) Token: 0x06004DF6 RID: 19958 RVA: 0x001BFC93 File Offset: 0x001BDE93
	public int AutoSaveCycleInterval
	{
		get
		{
			return this.autoSaveCycleInterval;
		}
		set
		{
			this.autoSaveCycleInterval = value;
		}
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x06004DF7 RID: 19959 RVA: 0x001BFC9C File Offset: 0x001BDE9C
	// (set) Token: 0x06004DF8 RID: 19960 RVA: 0x001BFCA4 File Offset: 0x001BDEA4
	public Vector2I TimelapseResolution
	{
		get
		{
			return this.timelapseResolution;
		}
		set
		{
			this.timelapseResolution = value;
		}
	}

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x06004DF9 RID: 19961 RVA: 0x001BFCAD File Offset: 0x001BDEAD
	public string BaseName
	{
		get
		{
			return this.baseName;
		}
	}

	// Token: 0x06004DFA RID: 19962 RVA: 0x001BFCB5 File Offset: 0x001BDEB5
	public static void DestroyInstance()
	{
		SaveGame.Instance = null;
	}

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x06004DFB RID: 19963 RVA: 0x001BFCBD File Offset: 0x001BDEBD
	public ColonyAchievementTracker ColonyAchievementTracker
	{
		get
		{
			if (this.colonyAchievementTracker == null)
			{
				this.colonyAchievementTracker = base.GetComponent<ColonyAchievementTracker>();
			}
			return this.colonyAchievementTracker;
		}
	}

	// Token: 0x06004DFC RID: 19964 RVA: 0x001BFCE0 File Offset: 0x001BDEE0
	protected override void OnPrefabInit()
	{
		SaveGame.Instance = this;
		new ColonyRationMonitor.Instance(this).StartSM();
		this.entombedItemManager = base.gameObject.AddComponent<EntombedItemManager>();
		this.worldGenSpawner = base.gameObject.AddComponent<WorldGenSpawner>();
		base.gameObject.AddOrGetDef<GameplaySeasonManager.Def>();
		base.gameObject.AddOrGetDef<ClusterFogOfWarManager.Def>();
	}

	// Token: 0x06004DFD RID: 19965 RVA: 0x001BFD38 File Offset: 0x001BDF38
	[OnSerializing]
	private void OnSerialize()
	{
		this.speed = SpeedControlScreen.Instance.GetSpeed();
	}

	// Token: 0x06004DFE RID: 19966 RVA: 0x001BFD4A File Offset: 0x001BDF4A
	[OnDeserializing]
	private void OnDeserialize()
	{
		this.baseName = SaveLoader.Instance.GameInfo.baseName;
	}

	// Token: 0x06004DFF RID: 19967 RVA: 0x001BFD61 File Offset: 0x001BDF61
	public int GetSpeed()
	{
		return this.speed;
	}

	// Token: 0x06004E00 RID: 19968 RVA: 0x001BFD6C File Offset: 0x001BDF6C
	public byte[] GetSaveHeader(bool isAutoSave, bool isCompressed, out SaveGame.Header header)
	{
		string originalSaveFileName = SaveLoader.GetOriginalSaveFileName(SaveLoader.GetActiveSaveFilePath());
		string s = JsonConvert.SerializeObject(new SaveGame.GameInfo(GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, this.baseName, isAutoSave, originalSaveFileName, SaveLoader.Instance.GameInfo.clusterId, SaveLoader.Instance.GameInfo.worldTraits, SaveLoader.Instance.GameInfo.colonyGuid, SaveLoader.Instance.GameInfo.dlcIds, this.sandboxEnabled));
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		header = default(SaveGame.Header);
		header.buildVersion = 642695U;
		header.headerSize = bytes.Length;
		header.headerVersion = 1U;
		header.compression = (isCompressed ? 1 : 0);
		return bytes;
	}

	// Token: 0x06004E01 RID: 19969 RVA: 0x001BFE30 File Offset: 0x001BE030
	public static string GetSaveUniqueID(SaveGame.GameInfo info)
	{
		if (!(info.colonyGuid != Guid.Empty))
		{
			return info.baseName + "/" + info.clusterId;
		}
		return info.colonyGuid.ToString();
	}

	// Token: 0x06004E02 RID: 19970 RVA: 0x001BFE70 File Offset: 0x001BE070
	public static global::Tuple<SaveGame.Header, SaveGame.GameInfo> GetFileInfo(string filename)
	{
		try
		{
			SaveGame.Header a;
			SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(filename, out a);
			if (gameInfo.saveMajorVersion >= 7)
			{
				return new global::Tuple<SaveGame.Header, SaveGame.GameInfo>(a, gameInfo);
			}
		}
		catch (Exception obj)
		{
			global::Debug.LogWarning("Exception while loading " + filename);
			global::Debug.LogWarning(obj);
		}
		return null;
	}

	// Token: 0x06004E03 RID: 19971 RVA: 0x001BFEC8 File Offset: 0x001BE0C8
	public static SaveGame.GameInfo GetHeader(IReader br, out SaveGame.Header header, string debugFileName)
	{
		header = default(SaveGame.Header);
		header.buildVersion = br.ReadUInt32();
		header.headerSize = br.ReadInt32();
		header.headerVersion = br.ReadUInt32();
		if (1U <= header.headerVersion)
		{
			header.compression = br.ReadInt32();
		}
		byte[] data = br.ReadBytes(header.headerSize);
		if (header.headerSize == 0 && !SaveGame.debug_SaveFileHeaderBlank_sent)
		{
			SaveGame.debug_SaveFileHeaderBlank_sent = true;
			global::Debug.LogWarning("SaveFileHeaderBlank - " + debugFileName);
		}
		SaveGame.GameInfo gameInfo = SaveGame.GetGameInfo(data);
		if (gameInfo.IsVersionOlderThan(7, 14) && gameInfo.worldTraits != null)
		{
			string[] worldTraits = gameInfo.worldTraits;
			for (int i = 0; i < worldTraits.Length; i++)
			{
				worldTraits[i] = worldTraits[i].Replace('\\', '/');
			}
		}
		if (gameInfo.IsVersionOlderThan(7, 20))
		{
			gameInfo.dlcId = "";
		}
		if (gameInfo.IsVersionOlderThan(7, 34))
		{
			gameInfo.dlcIds = new List<string>
			{
				gameInfo.dlcId
			};
		}
		return gameInfo;
	}

	// Token: 0x06004E04 RID: 19972 RVA: 0x001BFFC1 File Offset: 0x001BE1C1
	public static SaveGame.GameInfo GetGameInfo(byte[] data)
	{
		return JsonConvert.DeserializeObject<SaveGame.GameInfo>(Encoding.UTF8.GetString(data));
	}

	// Token: 0x06004E05 RID: 19973 RVA: 0x001BFFD3 File Offset: 0x001BE1D3
	public void SetBaseName(string newBaseName)
	{
		if (string.IsNullOrEmpty(newBaseName))
		{
			global::Debug.LogWarning("Cannot give the base an empty name");
			return;
		}
		this.baseName = newBaseName;
	}

	// Token: 0x06004E06 RID: 19974 RVA: 0x001BFFEF File Offset: 0x001BE1EF
	protected override void OnSpawn()
	{
		ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
		Game.Instance.Trigger(-1917495436, null);
	}

	// Token: 0x06004E07 RID: 19975 RVA: 0x001C000C File Offset: 0x001BE20C
	public List<global::Tuple<string, TextStyleSetting>> GetColonyToolTip()
	{
		List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		ClusterLayout clusterLayout;
		SettingsCache.clusterLayouts.clusterCache.TryGetValue(currentQualitySetting.id, out clusterLayout);
		list.Add(new global::Tuple<string, TextStyleSetting>(this.baseName, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		if (DlcManager.IsExpansion1Active())
		{
			StringEntry entry = Strings.Get(clusterLayout.name);
			list.Add(new global::Tuple<string, TextStyleSetting>(entry, ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		if (GameClock.Instance != null)
		{
			list.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
			list.Add(new global::Tuple<string, TextStyleSetting>(string.Format(UI.ASTEROIDCLOCK.CYCLES_OLD, GameUtil.GetCurrentCycle()), ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			list.Add(new global::Tuple<string, TextStyleSetting>(string.Format(UI.ASTEROIDCLOCK.TIME_PLAYED, (GameClock.Instance.GetTimePlayedInSeconds() / 3600f).ToString("0.00")), ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		int cameraActiveCluster = CameraController.Instance.cameraActiveCluster;
		WorldContainer world = ClusterManager.Instance.GetWorld(cameraActiveCluster);
		list.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
		if (DlcManager.IsExpansion1Active())
		{
			list.Add(new global::Tuple<string, TextStyleSetting>(world.GetComponent<ClusterGridEntity>().Name, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		}
		else
		{
			StringEntry entry2 = Strings.Get(clusterLayout.name);
			list.Add(new global::Tuple<string, TextStyleSetting>(entry2, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		}
		if (SaveLoader.Instance.GameInfo.worldTraits != null && SaveLoader.Instance.GameInfo.worldTraits.Length != 0)
		{
			string[] worldTraits = SaveLoader.Instance.GameInfo.worldTraits;
			for (int i = 0; i < worldTraits.Length; i++)
			{
				WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(worldTraits[i], false);
				if (cachedWorldTrait != null)
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(Strings.Get(cachedWorldTrait.name), ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
				else
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.MISSING_TRAIT, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
			}
		}
		else if (world.WorldTraitIds != null)
		{
			foreach (string name in world.WorldTraitIds)
			{
				WorldTrait cachedWorldTrait2 = SettingsCache.GetCachedWorldTrait(name, false);
				if (cachedWorldTrait2 != null)
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(Strings.Get(cachedWorldTrait2.name), ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
				else
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.MISSING_TRAIT, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
			}
			if (world.WorldTraitIds.Count == 0)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.NO_TRAITS.NAME_SHORTHAND, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			}
		}
		return list;
	}

	// Token: 0x040033E8 RID: 13288
	[Serialize]
	private int speed;

	// Token: 0x040033E9 RID: 13289
	[Serialize]
	public List<Tag> expandedResourceTags = new List<Tag>();

	// Token: 0x040033EA RID: 13290
	[Serialize]
	public int minGermCountForDisinfect = 10000;

	// Token: 0x040033EB RID: 13291
	[Serialize]
	public bool enableAutoDisinfect = true;

	// Token: 0x040033EC RID: 13292
	[Serialize]
	public bool sandboxEnabled;

	// Token: 0x040033ED RID: 13293
	[Serialize]
	public float relativeTemperatureOverlaySliderValue = 294.15f;

	// Token: 0x040033EE RID: 13294
	[Serialize]
	private int autoSaveCycleInterval = 1;

	// Token: 0x040033EF RID: 13295
	[Serialize]
	private Vector2I timelapseResolution = new Vector2I(512, 768);

	// Token: 0x040033F0 RID: 13296
	private string baseName;

	// Token: 0x040033F1 RID: 13297
	public static SaveGame Instance;

	// Token: 0x040033F2 RID: 13298
	private ColonyAchievementTracker colonyAchievementTracker;

	// Token: 0x040033F3 RID: 13299
	public EntombedItemManager entombedItemManager;

	// Token: 0x040033F4 RID: 13300
	public WorldGenSpawner worldGenSpawner;

	// Token: 0x040033F5 RID: 13301
	[MyCmpReq]
	public MaterialSelectorSerializer materialSelectorSerializer;

	// Token: 0x040033F6 RID: 13302
	private static bool debug_SaveFileHeaderBlank_sent;

	// Token: 0x02001A8F RID: 6799
	public struct Header
	{
		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x0600A085 RID: 41093 RVA: 0x0038002C File Offset: 0x0037E22C
		public bool IsCompressed
		{
			get
			{
				return this.compression != 0;
			}
		}

		// Token: 0x04007CED RID: 31981
		public uint buildVersion;

		// Token: 0x04007CEE RID: 31982
		public int headerSize;

		// Token: 0x04007CEF RID: 31983
		public uint headerVersion;

		// Token: 0x04007CF0 RID: 31984
		public int compression;
	}

	// Token: 0x02001A90 RID: 6800
	public struct GameInfo
	{
		// Token: 0x0600A086 RID: 41094 RVA: 0x00380038 File Offset: 0x0037E238
		public GameInfo(int numberOfCycles, int numberOfDuplicants, string baseName, bool isAutoSave, string originalSaveName, string clusterId, string[] worldTraits, Guid colonyGuid, List<string> dlcIds, bool sandboxEnabled = false)
		{
			this.numberOfCycles = numberOfCycles;
			this.numberOfDuplicants = numberOfDuplicants;
			this.baseName = baseName;
			this.isAutoSave = isAutoSave;
			this.originalSaveName = originalSaveName;
			this.clusterId = clusterId;
			this.worldTraits = worldTraits;
			this.colonyGuid = colonyGuid;
			this.sandboxEnabled = sandboxEnabled;
			this.dlcIds = dlcIds;
			this.dlcId = null;
			this.saveMajorVersion = 7;
			this.saveMinorVersion = 35;
		}

		// Token: 0x0600A087 RID: 41095 RVA: 0x003800A8 File Offset: 0x0037E2A8
		public bool IsVersionOlderThan(int major, int minor)
		{
			return this.saveMajorVersion < major || (this.saveMajorVersion == major && this.saveMinorVersion < minor);
		}

		// Token: 0x0600A088 RID: 41096 RVA: 0x003800C9 File Offset: 0x0037E2C9
		public bool IsVersionExactly(int major, int minor)
		{
			return this.saveMajorVersion == major && this.saveMinorVersion == minor;
		}

		// Token: 0x0600A089 RID: 41097 RVA: 0x003800E0 File Offset: 0x0037E2E0
		public bool IsCompatableWithCurrentDlcConfiguration(out HashSet<string> dlcIdsToEnable, out HashSet<string> dlcIdToDisable)
		{
			dlcIdsToEnable = new HashSet<string>();
			foreach (string item in this.dlcIds)
			{
				if (!DlcManager.IsContentSubscribed(item))
				{
					dlcIdsToEnable.Add(item);
				}
			}
			dlcIdToDisable = new HashSet<string>();
			if (!this.dlcIds.Contains("EXPANSION1_ID") && DlcManager.IsExpansion1Active())
			{
				dlcIdToDisable.Add("EXPANSION1_ID");
			}
			return dlcIdsToEnable.Count == 0 && dlcIdToDisable.Count == 0;
		}

		// Token: 0x04007CF1 RID: 31985
		public int numberOfCycles;

		// Token: 0x04007CF2 RID: 31986
		public int numberOfDuplicants;

		// Token: 0x04007CF3 RID: 31987
		public string baseName;

		// Token: 0x04007CF4 RID: 31988
		public bool isAutoSave;

		// Token: 0x04007CF5 RID: 31989
		public string originalSaveName;

		// Token: 0x04007CF6 RID: 31990
		public int saveMajorVersion;

		// Token: 0x04007CF7 RID: 31991
		public int saveMinorVersion;

		// Token: 0x04007CF8 RID: 31992
		public string clusterId;

		// Token: 0x04007CF9 RID: 31993
		public string[] worldTraits;

		// Token: 0x04007CFA RID: 31994
		public bool sandboxEnabled;

		// Token: 0x04007CFB RID: 31995
		public Guid colonyGuid;

		// Token: 0x04007CFC RID: 31996
		[Obsolete("Please use dlcIds instead.")]
		public string dlcId;

		// Token: 0x04007CFD RID: 31997
		public List<string> dlcIds;
	}
}
