using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zlib;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using KMod;
using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02000A78 RID: 2680
[AddComponentMenu("KMonoBehaviour/scripts/SaveLoader")]
public class SaveLoader : KMonoBehaviour
{
	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x06004E09 RID: 19977 RVA: 0x001C036F File Offset: 0x001BE56F
	// (set) Token: 0x06004E0A RID: 19978 RVA: 0x001C0377 File Offset: 0x001BE577
	public bool loadedFromSave { get; private set; }

	// Token: 0x06004E0B RID: 19979 RVA: 0x001C0380 File Offset: 0x001BE580
	public static void DestroyInstance()
	{
		SaveLoader.Instance = null;
	}

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x06004E0C RID: 19980 RVA: 0x001C0388 File Offset: 0x001BE588
	// (set) Token: 0x06004E0D RID: 19981 RVA: 0x001C038F File Offset: 0x001BE58F
	public static SaveLoader Instance { get; private set; }

	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x06004E0E RID: 19982 RVA: 0x001C0397 File Offset: 0x001BE597
	// (set) Token: 0x06004E0F RID: 19983 RVA: 0x001C039F File Offset: 0x001BE59F
	public Action<Cluster> OnWorldGenComplete { get; set; }

	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x06004E10 RID: 19984 RVA: 0x001C03A8 File Offset: 0x001BE5A8
	public Cluster Cluster
	{
		get
		{
			return this.m_cluster;
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x06004E11 RID: 19985 RVA: 0x001C03B0 File Offset: 0x001BE5B0
	public ClusterLayout ClusterLayout
	{
		get
		{
			if (this.m_clusterLayout == null)
			{
				this.m_clusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
			}
			return this.m_clusterLayout;
		}
	}

	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x06004E12 RID: 19986 RVA: 0x001C03D0 File Offset: 0x001BE5D0
	// (set) Token: 0x06004E13 RID: 19987 RVA: 0x001C03D8 File Offset: 0x001BE5D8
	public SaveGame.GameInfo GameInfo { get; private set; }

	// Token: 0x06004E14 RID: 19988 RVA: 0x001C03E1 File Offset: 0x001BE5E1
	protected override void OnPrefabInit()
	{
		SaveLoader.Instance = this;
		this.saveManager = base.GetComponent<SaveManager>();
	}

	// Token: 0x06004E15 RID: 19989 RVA: 0x001C03F5 File Offset: 0x001BE5F5
	private void MoveCorruptFile(string filename)
	{
	}

	// Token: 0x06004E16 RID: 19990 RVA: 0x001C03F8 File Offset: 0x001BE5F8
	protected override void OnSpawn()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (WorldGen.CanLoad(activeSaveFilePath))
		{
			Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
			SimMessages.CreateSimElementsTable(ElementLoader.elements);
			SimMessages.CreateDiseaseTable(Db.Get().Diseases);
			this.loadedFromSave = true;
			this.loadedFromSave = this.Load(activeSaveFilePath);
			this.saveFileCorrupt = !this.loadedFromSave;
			if (!this.loadedFromSave)
			{
				SaveLoader.SetActiveSaveFilePath(null);
				if (this.mustRestartOnFail)
				{
					this.MoveCorruptFile(activeSaveFilePath);
					Sim.Shutdown();
					App.LoadScene("frontend");
					return;
				}
			}
		}
		if (!this.loadedFromSave)
		{
			Sim.Shutdown();
			if (!string.IsNullOrEmpty(activeSaveFilePath))
			{
				DebugUtil.LogArgs(new object[]
				{
					"Couldn't load [" + activeSaveFilePath + "]"
				});
			}
			if (this.saveFileCorrupt)
			{
				this.MoveCorruptFile(activeSaveFilePath);
			}
			if (!this.LoadFromWorldGen())
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Couldn't start new game with current world gen, moving file"
				});
				KMonoBehaviour.isLoadingScene = true;
				this.MoveCorruptFile(WorldGen.WORLDGEN_SAVE_FILENAME);
				App.LoadScene("frontend");
			}
		}
	}

	// Token: 0x06004E17 RID: 19991 RVA: 0x001C0508 File Offset: 0x001BE708
	private static void CompressContents(BinaryWriter fileWriter, byte[] uncompressed, int length)
	{
		using (ZlibStream zlibStream = new ZlibStream(fileWriter.BaseStream, CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestSpeed))
		{
			zlibStream.Write(uncompressed, 0, length);
			zlibStream.Flush();
		}
	}

	// Token: 0x06004E18 RID: 19992 RVA: 0x001C0550 File Offset: 0x001BE750
	private byte[] FloatToBytes(float[] floats)
	{
		byte[] array = new byte[floats.Length * 4];
		Buffer.BlockCopy(floats, 0, array, 0, array.Length);
		return array;
	}

	// Token: 0x06004E19 RID: 19993 RVA: 0x001C0575 File Offset: 0x001BE775
	private static byte[] DecompressContents(byte[] compressed)
	{
		return ZlibStream.UncompressBuffer(compressed);
	}

	// Token: 0x06004E1A RID: 19994 RVA: 0x001C0580 File Offset: 0x001BE780
	private float[] BytesToFloat(byte[] bytes)
	{
		float[] array = new float[bytes.Length / 4];
		Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
		return array;
	}

	// Token: 0x06004E1B RID: 19995 RVA: 0x001C05A8 File Offset: 0x001BE7A8
	private SaveFileRoot PrepSaveFile()
	{
		SaveFileRoot saveFileRoot = new SaveFileRoot();
		saveFileRoot.WidthInCells = Grid.WidthInCells;
		saveFileRoot.HeightInCells = Grid.HeightInCells;
		saveFileRoot.streamed["GridVisible"] = Grid.Visible;
		saveFileRoot.streamed["GridSpawnable"] = Grid.Spawnable;
		saveFileRoot.streamed["GridDamage"] = this.FloatToBytes(Grid.Damage);
		Global.Instance.modManager.SendMetricsEvent();
		saveFileRoot.active_mods = new List<Label>();
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.IsEnabledForActiveDlc())
			{
				saveFileRoot.active_mods.Add(mod.label);
			}
		}
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				Camera.main.transform.parent.GetComponent<CameraController>().Save(binaryWriter);
			}
			saveFileRoot.streamed["Camera"] = memoryStream.ToArray();
		}
		return saveFileRoot;
	}

	// Token: 0x06004E1C RID: 19996 RVA: 0x001C0704 File Offset: 0x001BE904
	private void Save(BinaryWriter writer)
	{
		writer.WriteKleiString("world");
		Serializer.Serialize(this.PrepSaveFile(), writer);
		Game.SaveSettings(writer);
		Sim.Save(writer, 0, 0);
		this.saveManager.Save(writer);
		Game.Instance.Save(writer);
	}

	// Token: 0x06004E1D RID: 19997 RVA: 0x001C0744 File Offset: 0x001BE944
	private bool Load(IReader reader)
	{
		global::Debug.Assert(reader.ReadKleiString() == "world");
		Deserializer deserializer = new Deserializer(reader);
		SaveFileRoot saveFileRoot = new SaveFileRoot();
		deserializer.Deserialize(saveFileRoot);
		if ((this.GameInfo.saveMajorVersion == 7 || this.GameInfo.saveMinorVersion < 8) && saveFileRoot.requiredMods != null)
		{
			saveFileRoot.active_mods = new List<Label>();
			foreach (ModInfo modInfo in saveFileRoot.requiredMods)
			{
				saveFileRoot.active_mods.Add(new Label
				{
					id = modInfo.assetID,
					version = (long)modInfo.lastModifiedTime,
					distribution_platform = Label.DistributionPlatform.Steam,
					title = modInfo.description
				});
			}
			saveFileRoot.requiredMods.Clear();
		}
		KMod.Manager modManager = Global.Instance.modManager;
		modManager.Load(Content.LayerableFiles);
		if (!modManager.MatchFootprint(saveFileRoot.active_mods, Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Mod footprint of save file doesn't match current mod configuration"
			});
		}
		string text = string.Format("Mod Footprint ({0}):", saveFileRoot.active_mods.Count);
		foreach (Label label in saveFileRoot.active_mods)
		{
			text = text + "\n  - " + label.title;
		}
		global::Debug.Log(text);
		this.LogActiveMods();
		Global.Instance.modManager.SendMetricsEvent();
		WorldGen.LoadSettings(false);
		CustomGameSettings.Instance.LoadClusters();
		if (this.GameInfo.clusterId == null)
		{
			SaveGame.GameInfo gameInfo = this.GameInfo;
			if (!string.IsNullOrEmpty(saveFileRoot.clusterID))
			{
				gameInfo.clusterId = saveFileRoot.clusterID;
			}
			else
			{
				try
				{
					gameInfo.clusterId = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;
				}
				catch
				{
					gameInfo.clusterId = WorldGenSettings.ClusterDefaultName;
					CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, gameInfo.clusterId);
				}
			}
			this.GameInfo = gameInfo;
		}
		Game.clusterId = this.GameInfo.clusterId;
		Game.LoadSettings(deserializer);
		GridSettings.Reset(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells);
		if (Application.isPlaying)
		{
			Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
		}
		Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
		SimMessages.CreateSimElementsTable(ElementLoader.elements);
		Sim.AllocateCells(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells, false);
		SimMessages.CreateDiseaseTable(Db.Get().Diseases);
		Sim.HandleMessage(SimMessageHashes.ClearUnoccupiedCells, 0, null);
		IReader reader2;
		if (saveFileRoot.streamed.ContainsKey("Sim"))
		{
			reader2 = new FastReader(saveFileRoot.streamed["Sim"]);
		}
		else
		{
			reader2 = reader;
		}
		if (Sim.LoadWorld(reader2) != 0)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\nSimDLL found bad data\n"
			});
			Sim.Shutdown();
			return false;
		}
		Sim.Start();
		SceneInitializer.Instance.PostLoadPrefabs();
		this.mustRestartOnFail = true;
		if (!this.saveManager.Load(reader))
		{
			Sim.Shutdown();
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\n"
			});
			SaveLoader.SetActiveSaveFilePath(null);
			return false;
		}
		Grid.Visible = saveFileRoot.streamed["GridVisible"];
		if (saveFileRoot.streamed.ContainsKey("GridSpawnable"))
		{
			Grid.Spawnable = saveFileRoot.streamed["GridSpawnable"];
		}
		Grid.Damage = this.BytesToFloat(saveFileRoot.streamed["GridDamage"]);
		Game.Instance.Load(deserializer);
		CameraSaveData.Load(new FastReader(saveFileRoot.streamed["Camera"]));
		ClusterManager.Instance.InitializeWorldGrid();
		SimMessages.DefineWorldOffsets((from container in ClusterManager.Instance.WorldContainers
		select new SimMessages.WorldOffsetData
		{
			worldOffsetX = container.WorldOffset.x,
			worldOffsetY = container.WorldOffset.y,
			worldSizeX = container.WorldSize.x,
			worldSizeY = container.WorldSize.y
		}).ToList<SimMessages.WorldOffsetData>());
		return true;
	}

	// Token: 0x06004E1E RID: 19998 RVA: 0x001C0B68 File Offset: 0x001BED68
	private void LogActiveMods()
	{
		string text = string.Format("Active Mods ({0}):", Global.Instance.modManager.mods.Count((Mod x) => x.IsEnabledForActiveDlc()));
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.IsEnabledForActiveDlc())
			{
				text = text + "\n  - " + mod.title;
			}
		}
		global::Debug.Log(text);
	}

	// Token: 0x06004E1F RID: 19999 RVA: 0x001C0C20 File Offset: 0x001BEE20
	public static string GetSavePrefix()
	{
		return System.IO.Path.Combine(global::Util.RootFolder(), string.Format("{0}{1}", "save_files", System.IO.Path.DirectorySeparatorChar));
	}

	// Token: 0x06004E20 RID: 20000 RVA: 0x001C0C48 File Offset: 0x001BEE48
	public static string GetCloudSavePrefix()
	{
		string text = System.IO.Path.Combine(global::Util.RootFolder(), string.Format("{0}{1}", "cloud_save_files", System.IO.Path.DirectorySeparatorChar));
		string userID = SaveLoader.GetUserID();
		if (string.IsNullOrEmpty(userID))
		{
			return null;
		}
		text = System.IO.Path.Combine(text, userID);
		if (!System.IO.Directory.Exists(text))
		{
			System.IO.Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x06004E21 RID: 20001 RVA: 0x001C0CA4 File Offset: 0x001BEEA4
	public static string GetSavePrefixAndCreateFolder()
	{
		string savePrefix = SaveLoader.GetSavePrefix();
		if (!System.IO.Directory.Exists(savePrefix))
		{
			System.IO.Directory.CreateDirectory(savePrefix);
		}
		return savePrefix;
	}

	// Token: 0x06004E22 RID: 20002 RVA: 0x001C0CC8 File Offset: 0x001BEEC8
	public static string GetUserID()
	{
		DistributionPlatform.User localUser = DistributionPlatform.Inst.LocalUser;
		if (localUser == null)
		{
			return null;
		}
		return localUser.Id.ToString();
	}

	// Token: 0x06004E23 RID: 20003 RVA: 0x001C0CF0 File Offset: 0x001BEEF0
	public static string GetNextUsableSavePath(string filename)
	{
		int num = 0;
		string arg = System.IO.Path.ChangeExtension(filename, null);
		while (File.Exists(filename))
		{
			filename = SaveScreen.GetValidSaveFilename(string.Format("{0} ({1})", arg, num));
			num++;
		}
		return filename;
	}

	// Token: 0x06004E24 RID: 20004 RVA: 0x001C0D2E File Offset: 0x001BEF2E
	public static string GetOriginalSaveFileName(string filename)
	{
		if (!filename.Contains("/") && !filename.Contains("\\"))
		{
			return filename;
		}
		filename.Replace('\\', '/');
		return System.IO.Path.GetFileName(filename);
	}

	// Token: 0x06004E25 RID: 20005 RVA: 0x001C0D5D File Offset: 0x001BEF5D
	public static bool IsSaveAuto(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/auto_save/");
	}

	// Token: 0x06004E26 RID: 20006 RVA: 0x001C0D76 File Offset: 0x001BEF76
	public static bool IsSaveLocal(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/save_files/");
	}

	// Token: 0x06004E27 RID: 20007 RVA: 0x001C0D8F File Offset: 0x001BEF8F
	public static bool IsSaveCloud(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/cloud_save_files/");
	}

	// Token: 0x06004E28 RID: 20008 RVA: 0x001C0DA8 File Offset: 0x001BEFA8
	public static string GetAutoSavePrefix()
	{
		string text = System.IO.Path.Combine(SaveLoader.GetSavePrefixAndCreateFolder(), string.Format("{0}{1}", "auto_save", System.IO.Path.DirectorySeparatorChar));
		if (!System.IO.Directory.Exists(text))
		{
			System.IO.Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x06004E29 RID: 20009 RVA: 0x001C0DE9 File Offset: 0x001BEFE9
	public static void SetActiveSaveFilePath(string path)
	{
		KPlayerPrefs.SetString("SaveFilenameKey/", path);
	}

	// Token: 0x06004E2A RID: 20010 RVA: 0x001C0DF6 File Offset: 0x001BEFF6
	public static string GetActiveSaveFilePath()
	{
		return KPlayerPrefs.GetString("SaveFilenameKey/");
	}

	// Token: 0x06004E2B RID: 20011 RVA: 0x001C0E04 File Offset: 0x001BF004
	public static string GetActiveAutoSavePath()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (activeSaveFilePath == null)
		{
			return SaveLoader.GetAutoSavePrefix();
		}
		return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(activeSaveFilePath), "auto_save");
	}

	// Token: 0x06004E2C RID: 20012 RVA: 0x001C0E30 File Offset: 0x001BF030
	public static string GetAutosaveFilePath()
	{
		return SaveLoader.GetAutoSavePrefix() + "AutoSave Cycle 1.sav";
	}

	// Token: 0x06004E2D RID: 20013 RVA: 0x001C0E44 File Offset: 0x001BF044
	public static string GetActiveSaveColonyFolder()
	{
		string text = SaveLoader.GetActiveSaveFolder();
		if (text == null)
		{
			text = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), SaveLoader.Instance.GameInfo.baseName);
		}
		return text;
	}

	// Token: 0x06004E2E RID: 20014 RVA: 0x001C0E78 File Offset: 0x001BF078
	public static string GetActiveSaveFolder()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (!string.IsNullOrEmpty(activeSaveFilePath))
		{
			return System.IO.Path.GetDirectoryName(activeSaveFilePath);
		}
		return null;
	}

	// Token: 0x06004E2F RID: 20015 RVA: 0x001C0E9C File Offset: 0x001BF09C
	public static List<SaveLoader.SaveFileEntry> GetSaveFiles(string save_dir, bool sort, SearchOption search = SearchOption.AllDirectories)
	{
		List<SaveLoader.SaveFileEntry> list = new List<SaveLoader.SaveFileEntry>();
		if (string.IsNullOrEmpty(save_dir))
		{
			return list;
		}
		try
		{
			if (!System.IO.Directory.Exists(save_dir))
			{
				System.IO.Directory.CreateDirectory(save_dir);
			}
			foreach (string text in System.IO.Directory.GetFiles(save_dir, "*.sav", search))
			{
				try
				{
					System.DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(text);
					SaveLoader.SaveFileEntry item = new SaveLoader.SaveFileEntry
					{
						path = text,
						timeStamp = lastWriteTimeUtc
					};
					list.Add(item);
				}
				catch (Exception ex)
				{
					global::Debug.LogWarning("Problem reading file: " + text + "\n" + ex.ToString());
				}
			}
			if (sort)
			{
				list.Sort((SaveLoader.SaveFileEntry x, SaveLoader.SaveFileEntry y) => y.timeStamp.CompareTo(x.timeStamp));
			}
		}
		catch (Exception ex2)
		{
			string text2 = null;
			if (ex2 is UnauthorizedAccessException)
			{
				text2 = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, save_dir);
			}
			else if (ex2 is IOException)
			{
				text2 = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, save_dir);
			}
			if (text2 == null)
			{
				throw ex2;
			}
			GameObject parent = (FrontEndManager.Instance == null) ? GameScreenManager.Instance.ssOverlayCanvas : FrontEndManager.Instance.gameObject;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(text2, null, null, null, null, null, null, null, null);
		}
		return list;
	}

	// Token: 0x06004E30 RID: 20016 RVA: 0x001C101C File Offset: 0x001BF21C
	public static List<SaveLoader.SaveFileEntry> GetAllFiles(bool sort, SaveLoader.SaveType type = SaveLoader.SaveType.both)
	{
		switch (type)
		{
		case SaveLoader.SaveType.local:
			return SaveLoader.GetSaveFiles(SaveLoader.GetSavePrefixAndCreateFolder(), sort, SearchOption.AllDirectories);
		case SaveLoader.SaveType.cloud:
			return SaveLoader.GetSaveFiles(SaveLoader.GetCloudSavePrefix(), sort, SearchOption.AllDirectories);
		case SaveLoader.SaveType.both:
		{
			List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(SaveLoader.GetSavePrefixAndCreateFolder(), false, SearchOption.AllDirectories);
			List<SaveLoader.SaveFileEntry> saveFiles2 = SaveLoader.GetSaveFiles(SaveLoader.GetCloudSavePrefix(), false, SearchOption.AllDirectories);
			saveFiles.AddRange(saveFiles2);
			if (sort)
			{
				saveFiles.Sort((SaveLoader.SaveFileEntry x, SaveLoader.SaveFileEntry y) => y.timeStamp.CompareTo(x.timeStamp));
			}
			return saveFiles;
		}
		default:
			return new List<SaveLoader.SaveFileEntry>();
		}
	}

	// Token: 0x06004E31 RID: 20017 RVA: 0x001C10A7 File Offset: 0x001BF2A7
	public static List<SaveLoader.SaveFileEntry> GetAllColonyFiles(bool sort, SearchOption search = SearchOption.TopDirectoryOnly)
	{
		return SaveLoader.GetSaveFiles(SaveLoader.GetActiveSaveColonyFolder(), sort, search);
	}

	// Token: 0x06004E32 RID: 20018 RVA: 0x001C10B5 File Offset: 0x001BF2B5
	public static bool GetCloudSavesDefault()
	{
		return !(SaveLoader.GetCloudSavesDefaultPref() == "Disabled");
	}

	// Token: 0x06004E33 RID: 20019 RVA: 0x001C10CC File Offset: 0x001BF2CC
	public static string GetCloudSavesDefaultPref()
	{
		string text = KPlayerPrefs.GetString("SavesDefaultToCloud", "Enabled");
		if (text != "Enabled" && text != "Disabled")
		{
			text = "Enabled";
		}
		return text;
	}

	// Token: 0x06004E34 RID: 20020 RVA: 0x001C110A File Offset: 0x001BF30A
	public static void SetCloudSavesDefault(bool value)
	{
		SaveLoader.SetCloudSavesDefaultPref(value ? "Enabled" : "Disabled");
	}

	// Token: 0x06004E35 RID: 20021 RVA: 0x001C1120 File Offset: 0x001BF320
	public static void SetCloudSavesDefaultPref(string pref)
	{
		if (pref != "Enabled" && pref != "Disabled")
		{
			global::Debug.LogWarning("Ignoring cloud saves default pref `" + pref + "` as it's not valid, expected `Enabled` or `Disabled`");
			return;
		}
		KPlayerPrefs.SetString("SavesDefaultToCloud", pref);
	}

	// Token: 0x06004E36 RID: 20022 RVA: 0x001C115D File Offset: 0x001BF35D
	public static bool GetCloudSavesAvailable()
	{
		return !string.IsNullOrEmpty(SaveLoader.GetUserID()) && SaveLoader.GetCloudSavePrefix() != null;
	}

	// Token: 0x06004E37 RID: 20023 RVA: 0x001C1178 File Offset: 0x001BF378
	public static string GetLatestSaveForCurrentDLC()
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(true, SaveLoader.SaveType.both);
		for (int i = 0; i < allFiles.Count; i++)
		{
			global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(allFiles[i].path);
			if (fileInfo != null)
			{
				SaveGame.Header first = fileInfo.first;
				SaveGame.GameInfo second = fileInfo.second;
				HashSet<string> hashSet;
				HashSet<string> hashSet2;
				if (second.saveMajorVersion >= 7 && second.IsCompatableWithCurrentDlcConfiguration(out hashSet, out hashSet2))
				{
					return allFiles[i].path;
				}
			}
		}
		return null;
	}

	// Token: 0x06004E38 RID: 20024 RVA: 0x001C11EC File Offset: 0x001BF3EC
	public void InitialSave()
	{
		string text = SaveLoader.GetActiveSaveFilePath();
		if (string.IsNullOrEmpty(text))
		{
			text = SaveLoader.GetAutosaveFilePath();
		}
		else if (!text.Contains(".sav"))
		{
			text += ".sav";
		}
		this.LogActiveMods();
		this.Save(text, false, true);
	}

	// Token: 0x06004E39 RID: 20025 RVA: 0x001C1238 File Offset: 0x001BF438
	public string Save(string filename, bool isAutoSave = false, bool updateSavePointer = true)
	{
		KSerialization.Manager.Clear();
		string directoryName = System.IO.Path.GetDirectoryName(filename);
		try
		{
			if (directoryName != null && !System.IO.Directory.Exists(directoryName))
			{
				System.IO.Directory.CreateDirectory(directoryName);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("Problem creating save folder for " + filename + "!\n" + ex.ToString());
		}
		this.ReportSaveMetrics(isAutoSave);
		RetireColonyUtility.SaveColonySummaryData();
		if (isAutoSave && !GenericGameSettings.instance.keepAllAutosaves)
		{
			List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(SaveLoader.GetActiveAutoSavePath(), true, SearchOption.AllDirectories);
			List<string> list = new List<string>();
			foreach (SaveLoader.SaveFileEntry saveFileEntry in saveFiles)
			{
				global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(saveFileEntry.path);
				if (fileInfo != null && SaveGame.GetSaveUniqueID(fileInfo.second) == SaveLoader.Instance.GameInfo.colonyGuid.ToString())
				{
					list.Add(saveFileEntry.path);
				}
			}
			for (int i = list.Count - 1; i >= 9; i--)
			{
				string text = list[i];
				try
				{
					global::Debug.Log("Deleting old autosave: " + text);
					File.Delete(text);
				}
				catch (Exception ex2)
				{
					global::Debug.LogWarning("Problem deleting autosave: " + text + "\n" + ex2.ToString());
				}
				string text2 = System.IO.Path.ChangeExtension(text, ".png");
				try
				{
					if (File.Exists(text2))
					{
						File.Delete(text2);
					}
				}
				catch (Exception ex3)
				{
					global::Debug.LogWarning("Problem deleting autosave screenshot: " + text2 + "\n" + ex3.ToString());
				}
			}
		}
		using (MemoryStream memoryStream = new MemoryStream((int)((float)this.lastUncompressedSize * 1.1f)))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				this.Save(binaryWriter);
				this.lastUncompressedSize = (int)memoryStream.Length;
				try
				{
					using (BinaryWriter binaryWriter2 = new BinaryWriter(File.Open(filename, FileMode.Create)))
					{
						SaveGame.Header header;
						byte[] saveHeader = SaveGame.Instance.GetSaveHeader(isAutoSave, this.compressSaveData, out header);
						binaryWriter2.Write(header.buildVersion);
						binaryWriter2.Write(header.headerSize);
						binaryWriter2.Write(header.headerVersion);
						binaryWriter2.Write(header.compression);
						binaryWriter2.Write(saveHeader);
						KSerialization.Manager.SerializeDirectory(binaryWriter2);
						if (this.compressSaveData)
						{
							SaveLoader.CompressContents(binaryWriter2, memoryStream.GetBuffer(), (int)memoryStream.Length);
						}
						else
						{
							binaryWriter2.Write(memoryStream.ToArray());
						}
						KCrashReporter.MOST_RECENT_SAVEFILE = filename;
						Stats.Print();
					}
				}
				catch (Exception ex4)
				{
					if (ex4 is UnauthorizedAccessException)
					{
						DebugUtil.LogArgs(new object[]
						{
							"UnauthorizedAccessException for " + filename
						});
						((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.CRASHSCREEN.SAVEFAILED, "Unauthorized Access Exception"), null, null, null, null, null, null, null, null);
						return SaveLoader.GetActiveSaveFilePath();
					}
					if (ex4 is IOException)
					{
						DebugUtil.LogArgs(new object[]
						{
							"IOException (probably out of disk space) for " + filename
						});
						((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.CRASHSCREEN.SAVEFAILED, "IOException. You may not have enough free space!"), null, null, null, null, null, null, null, null);
						return SaveLoader.GetActiveSaveFilePath();
					}
					throw ex4;
				}
			}
		}
		if (updateSavePointer)
		{
			SaveLoader.SetActiveSaveFilePath(filename);
		}
		Game.Instance.timelapser.SaveColonyPreview(filename);
		DebugUtil.LogArgs(new object[]
		{
			"Saved to",
			"[" + filename + "]"
		});
		GC.Collect();
		return filename;
	}

	// Token: 0x06004E3A RID: 20026 RVA: 0x001C16E0 File Offset: 0x001BF8E0
	public static SaveGame.GameInfo LoadHeader(string filename, out SaveGame.Header header)
	{
		byte[] array = new byte[512];
		SaveGame.GameInfo header2;
		using (FileStream fileStream = File.OpenRead(filename))
		{
			fileStream.Read(array, 0, 512);
			header2 = SaveGame.GetHeader(new FastReader(array), out header, filename);
		}
		return header2;
	}

	// Token: 0x06004E3B RID: 20027 RVA: 0x001C1738 File Offset: 0x001BF938
	public bool Load(string filename)
	{
		SaveLoader.SetActiveSaveFilePath(filename);
		try
		{
			KSerialization.Manager.Clear();
			byte[] array = File.ReadAllBytes(filename);
			IReader reader = new FastReader(array);
			SaveGame.Header header;
			this.GameInfo = SaveGame.GetHeader(reader, out header, filename);
			ThreadedHttps<KleiMetrics>.Instance.SetExpansionsActive(this.GameInfo.dlcIds);
			DebugUtil.LogArgs(new object[]
			{
				string.Format("Loading save file: {4}\n headerVersion:{0}, buildVersion:{1}, headerSize:{2}, IsCompressed:{3}", new object[]
				{
					header.headerVersion,
					header.buildVersion,
					header.headerSize,
					header.IsCompressed,
					filename
				})
			});
			DebugUtil.LogArgs(new object[]
			{
				string.Format("GameInfo loaded from save header:\n  numberOfCycles:{0},\n  numberOfDuplicants:{1},\n  baseName:{2},\n  isAutoSave:{3},\n  originalSaveName:{4},\n  clusterId:{5},\n  worldTraits:{6},\n  colonyGuid:{7},\n  saveVersion:{8}.{9}", new object[]
				{
					this.GameInfo.numberOfCycles,
					this.GameInfo.numberOfDuplicants,
					this.GameInfo.baseName,
					this.GameInfo.isAutoSave,
					this.GameInfo.originalSaveName,
					this.GameInfo.clusterId,
					(this.GameInfo.worldTraits != null && this.GameInfo.worldTraits.Length != 0) ? string.Join(", ", this.GameInfo.worldTraits) : "<i>none</i>",
					this.GameInfo.colonyGuid,
					this.GameInfo.saveMajorVersion,
					this.GameInfo.saveMinorVersion
				})
			});
			string originalSaveName = this.GameInfo.originalSaveName;
			if (originalSaveName.Contains("/") || originalSaveName.Contains("\\"))
			{
				string originalSaveFileName = SaveLoader.GetOriginalSaveFileName(originalSaveName);
				SaveGame.GameInfo gameInfo = this.GameInfo;
				gameInfo.originalSaveName = originalSaveFileName;
				this.GameInfo = gameInfo;
				global::Debug.Log(string.Concat(new string[]
				{
					"Migration / Save originalSaveName updated from: `",
					originalSaveName,
					"` => `",
					this.GameInfo.originalSaveName,
					"`"
				}));
			}
			if (this.GameInfo.saveMajorVersion == 7 && this.GameInfo.saveMinorVersion < 4)
			{
				Helper.SetTypeInfoMask((SerializationTypeInfo)191);
			}
			KSerialization.Manager.DeserializeDirectory(reader);
			if (header.IsCompressed)
			{
				int num = array.Length - reader.Position;
				byte[] array2 = new byte[num];
				Array.Copy(array, reader.Position, array2, 0, num);
				byte[] array3 = SaveLoader.DecompressContents(array2);
				this.lastUncompressedSize = array3.Length;
				IReader reader2 = new FastReader(array3);
				this.Load(reader2);
			}
			else
			{
				this.lastUncompressedSize = array.Length;
				this.Load(reader);
			}
			KCrashReporter.MOST_RECENT_SAVEFILE = filename;
			if (this.GameInfo.isAutoSave && !string.IsNullOrEmpty(this.GameInfo.originalSaveName))
			{
				string originalSaveFileName2 = SaveLoader.GetOriginalSaveFileName(this.GameInfo.originalSaveName);
				string text;
				if (SaveLoader.IsSaveCloud(filename))
				{
					string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
					if (cloudSavePrefix != null)
					{
						text = System.IO.Path.Combine(cloudSavePrefix, this.GameInfo.baseName, originalSaveFileName2);
					}
					else
					{
						text = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filename).Replace("auto_save", ""), this.GameInfo.baseName, originalSaveFileName2);
					}
				}
				else
				{
					text = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), this.GameInfo.baseName, originalSaveFileName2);
				}
				if (text != null)
				{
					SaveLoader.SetActiveSaveFilePath(text);
				}
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\n" + ex.Message + "\n" + ex.StackTrace
			});
			Sim.Shutdown();
			SaveLoader.SetActiveSaveFilePath(null);
			return false;
		}
		Stats.Print();
		DebugUtil.LogArgs(new object[]
		{
			"Loaded",
			"[" + filename + "]"
		});
		DebugUtil.LogArgs(new object[]
		{
			"World Seeds",
			string.Concat(new string[]
			{
				"[",
				this.clusterDetailSave.globalWorldSeed.ToString(),
				"/",
				this.clusterDetailSave.globalWorldLayoutSeed.ToString(),
				"/",
				this.clusterDetailSave.globalTerrainSeed.ToString(),
				"/",
				this.clusterDetailSave.globalNoiseSeed.ToString(),
				"]"
			})
		});
		GC.Collect();
		return true;
	}

	// Token: 0x06004E3C RID: 20028 RVA: 0x001C1BCC File Offset: 0x001BFDCC
	public bool LoadFromWorldGen()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Attempting to start a new game with current world gen"
		});
		WorldGen.LoadSettings(false);
		FastReader reader = new FastReader(File.ReadAllBytes(WorldGen.WORLDGEN_SAVE_FILENAME));
		this.m_cluster = Cluster.Load(reader);
		ListPool<SimSaveFileStructure, SaveLoader>.PooledList pooledList = ListPool<SimSaveFileStructure, SaveLoader>.Allocate();
		this.m_cluster.LoadClusterSim(pooledList, reader);
		SaveGame.GameInfo gameInfo = this.GameInfo;
		gameInfo.clusterId = this.m_cluster.Id;
		gameInfo.colonyGuid = Guid.NewGuid();
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		gameInfo.dlcIds = new List<string>(currentClusterLayout.requiredDlcIds);
		foreach (string item in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
		{
			if (!gameInfo.dlcIds.Contains(item))
			{
				gameInfo.dlcIds.Add(item);
			}
		}
		this.GameInfo = gameInfo;
		ThreadedHttps<KleiMetrics>.Instance.SetExpansionsActive(this.GameInfo.dlcIds);
		if (pooledList.Count != this.m_cluster.worlds.Count)
		{
			global::Debug.LogError("Attempt failed. Failed to load all worlds.");
			pooledList.Recycle();
			return false;
		}
		GridSettings.Reset(this.m_cluster.size.x, this.m_cluster.size.y);
		if (Application.isPlaying)
		{
			Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
		}
		this.clusterDetailSave = new WorldDetailSave();
		foreach (SimSaveFileStructure simSaveFileStructure in pooledList)
		{
			this.clusterDetailSave.globalNoiseSeed = simSaveFileStructure.worldDetail.globalNoiseSeed;
			this.clusterDetailSave.globalTerrainSeed = simSaveFileStructure.worldDetail.globalTerrainSeed;
			this.clusterDetailSave.globalWorldLayoutSeed = simSaveFileStructure.worldDetail.globalWorldLayoutSeed;
			this.clusterDetailSave.globalWorldSeed = simSaveFileStructure.worldDetail.globalWorldSeed;
			Vector2 b = Grid.CellToPos2D(Grid.PosToCell(new Vector2I(simSaveFileStructure.x, simSaveFileStructure.y)));
			foreach (WorldDetailSave.OverworldCell overworldCell in simSaveFileStructure.worldDetail.overworldCells)
			{
				for (int num = 0; num != overworldCell.poly.Vertices.Count; num++)
				{
					List<Vector2> vertices = overworldCell.poly.Vertices;
					int index = num;
					vertices[index] += b;
				}
				overworldCell.poly.RefreshBounds();
			}
			this.clusterDetailSave.overworldCells.AddRange(simSaveFileStructure.worldDetail.overworldCells);
		}
		Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
		SimMessages.CreateSimElementsTable(ElementLoader.elements);
		Sim.AllocateCells(this.m_cluster.size.x, this.m_cluster.size.y, false);
		SimMessages.DefineWorldOffsets((from world in this.m_cluster.worlds
		select new SimMessages.WorldOffsetData
		{
			worldOffsetX = world.WorldOffset.x,
			worldOffsetY = world.WorldOffset.y,
			worldSizeX = world.WorldSize.x,
			worldSizeY = world.WorldSize.y
		}).ToList<SimMessages.WorldOffsetData>());
		SimMessages.CreateDiseaseTable(Db.Get().Diseases);
		Sim.HandleMessage(SimMessageHashes.ClearUnoccupiedCells, 0, null);
		try
		{
			foreach (SimSaveFileStructure simSaveFileStructure2 in pooledList)
			{
				FastReader reader2 = new FastReader(simSaveFileStructure2.Sim);
				if (Sim.Load(reader2) != 0)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"\n--- Error loading save ---\nSimDLL found bad data\n"
					});
					Sim.Shutdown();
					pooledList.Recycle();
					return false;
				}
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("--- Error loading Sim FROM NEW WORLDGEN ---" + ex.Message + "\n" + ex.StackTrace);
			Sim.Shutdown();
			pooledList.Recycle();
			return false;
		}
		global::Debug.Log("Attempt success");
		Sim.Start();
		SceneInitializer.Instance.PostLoadPrefabs();
		SceneInitializer.Instance.NewSaveGamePrefab();
		this.cachedGSD = this.m_cluster.currentWorld.SpawnData;
		this.OnWorldGenComplete.Signal(this.m_cluster);
		OniMetrics.LogEvent(OniMetrics.Event.NewSave, "NewGame", true);
		StoryManager.Instance.InitialSaveSetup();
		ThreadedHttps<KleiMetrics>.Instance.IncrementGameCount();
		OniMetrics.SendEvent(OniMetrics.Event.NewSave, "New Save");
		pooledList.Recycle();
		return true;
	}

	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x06004E3D RID: 20029 RVA: 0x001C20D0 File Offset: 0x001C02D0
	// (set) Token: 0x06004E3E RID: 20030 RVA: 0x001C20D8 File Offset: 0x001C02D8
	public GameSpawnData cachedGSD { get; private set; }

	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x06004E3F RID: 20031 RVA: 0x001C20E1 File Offset: 0x001C02E1
	// (set) Token: 0x06004E40 RID: 20032 RVA: 0x001C20E9 File Offset: 0x001C02E9
	public WorldDetailSave clusterDetailSave { get; private set; }

	// Token: 0x06004E41 RID: 20033 RVA: 0x001C20F2 File Offset: 0x001C02F2
	public void SetWorldDetail(WorldDetailSave worldDetail)
	{
		this.clusterDetailSave = worldDetail;
	}

	// Token: 0x06004E42 RID: 20034 RVA: 0x001C20FC File Offset: 0x001C02FC
	private void ReportSaveMetrics(bool is_auto_save)
	{
		if (ThreadedHttps<KleiMetrics>.Instance == null || !ThreadedHttps<KleiMetrics>.Instance.enabled || this.saveManager == null)
		{
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary[GameClock.NewCycleKey] = GameClock.Instance.GetCycle() + 1;
		dictionary["IsAutoSave"] = is_auto_save;
		dictionary["SavedPrefabs"] = this.GetSavedPrefabMetrics();
		dictionary["ResourcesAccessible"] = this.GetWorldInventoryMetrics();
		dictionary["MinionMetrics"] = this.GetMinionMetrics();
		dictionary["WorldMetrics"] = this.GetWorldMetrics();
		if (is_auto_save)
		{
			dictionary["DailyReport"] = this.GetDailyReportMetrics();
			dictionary["PerformanceMeasurements"] = this.GetPerformanceMeasurements();
			dictionary["AverageFrameTime"] = this.GetFrameTime();
		}
		dictionary["CustomGameSettings"] = CustomGameSettings.Instance.GetSettingsForMetrics();
		dictionary["CustomMixingSettings"] = CustomGameSettings.Instance.GetSettingsForMixingMetrics();
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(dictionary, "ReportSaveMetrics");
	}

	// Token: 0x06004E43 RID: 20035 RVA: 0x001C2218 File Offset: 0x001C0418
	private List<SaveLoader.MinionMetricsData> GetMinionMetrics()
	{
		List<SaveLoader.MinionMetricsData> list = new List<SaveLoader.MinionMetricsData>();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if (!(minionIdentity == null))
			{
				Amounts amounts = minionIdentity.gameObject.GetComponent<Modifiers>().amounts;
				List<SaveLoader.MinionAttrFloatData> list2 = new List<SaveLoader.MinionAttrFloatData>(amounts.Count);
				foreach (AmountInstance amountInstance in amounts)
				{
					float value = amountInstance.value;
					if (!float.IsNaN(value) && !float.IsInfinity(value))
					{
						list2.Add(new SaveLoader.MinionAttrFloatData
						{
							Name = amountInstance.modifier.Id,
							Value = amountInstance.value
						});
					}
				}
				MinionResume component = minionIdentity.gameObject.GetComponent<MinionResume>();
				float totalExperienceGained = component.TotalExperienceGained;
				List<string> list3 = new List<string>();
				foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
				{
					if (keyValuePair.Value)
					{
						list3.Add(keyValuePair.Key);
					}
				}
				list.Add(new SaveLoader.MinionMetricsData
				{
					Name = minionIdentity.name,
					Modifiers = list2,
					TotalExperienceGained = totalExperienceGained,
					Skills = list3
				});
			}
		}
		return list;
	}

	// Token: 0x06004E44 RID: 20036 RVA: 0x001C23E8 File Offset: 0x001C05E8
	private List<SaveLoader.SavedPrefabMetricsData> GetSavedPrefabMetrics()
	{
		Dictionary<Tag, List<SaveLoadRoot>> lists = this.saveManager.GetLists();
		List<SaveLoader.SavedPrefabMetricsData> list = new List<SaveLoader.SavedPrefabMetricsData>(lists.Count);
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in lists)
		{
			Tag key = keyValuePair.Key;
			List<SaveLoadRoot> value = keyValuePair.Value;
			if (value.Count > 0)
			{
				list.Add(new SaveLoader.SavedPrefabMetricsData
				{
					PrefabName = key.ToString(),
					Count = value.Count
				});
			}
		}
		return list;
	}

	// Token: 0x06004E45 RID: 20037 RVA: 0x001C2494 File Offset: 0x001C0694
	private List<SaveLoader.WorldInventoryMetricsData> GetWorldInventoryMetrics()
	{
		Dictionary<Tag, float> allWorldsAccessibleAmounts = ClusterManager.Instance.GetAllWorldsAccessibleAmounts();
		List<SaveLoader.WorldInventoryMetricsData> list = new List<SaveLoader.WorldInventoryMetricsData>(allWorldsAccessibleAmounts.Count);
		foreach (KeyValuePair<Tag, float> keyValuePair in allWorldsAccessibleAmounts)
		{
			float value = keyValuePair.Value;
			if (!float.IsInfinity(value) && !float.IsNaN(value))
			{
				list.Add(new SaveLoader.WorldInventoryMetricsData
				{
					Name = keyValuePair.Key.ToString(),
					Amount = value
				});
			}
		}
		return list;
	}

	// Token: 0x06004E46 RID: 20038 RVA: 0x001C2540 File Offset: 0x001C0740
	private List<SaveLoader.DailyReportMetricsData> GetDailyReportMetrics()
	{
		List<SaveLoader.DailyReportMetricsData> list = new List<SaveLoader.DailyReportMetricsData>();
		int cycle = GameClock.Instance.GetCycle();
		ReportManager.DailyReport dailyReport = ReportManager.Instance.FindReport(cycle);
		if (dailyReport != null)
		{
			foreach (ReportManager.ReportEntry reportEntry in dailyReport.reportEntries)
			{
				SaveLoader.DailyReportMetricsData item = default(SaveLoader.DailyReportMetricsData);
				item.Name = reportEntry.reportType.ToString();
				if (!float.IsInfinity(reportEntry.Net) && !float.IsNaN(reportEntry.Net))
				{
					item.Net = new float?(reportEntry.Net);
				}
				if (SaveLoader.force_infinity)
				{
					item.Net = null;
				}
				if (!float.IsInfinity(reportEntry.Positive) && !float.IsNaN(reportEntry.Positive))
				{
					item.Positive = new float?(reportEntry.Positive);
				}
				if (!float.IsInfinity(reportEntry.Negative) && !float.IsNaN(reportEntry.Negative))
				{
					item.Negative = new float?(reportEntry.Negative);
				}
				list.Add(item);
			}
			list.Add(new SaveLoader.DailyReportMetricsData
			{
				Name = "MinionCount",
				Net = new float?((float)Components.LiveMinionIdentities.Count),
				Positive = new float?(0f),
				Negative = new float?(0f)
			});
		}
		return list;
	}

	// Token: 0x06004E47 RID: 20039 RVA: 0x001C26D8 File Offset: 0x001C08D8
	private List<SaveLoader.PerformanceMeasurement> GetPerformanceMeasurements()
	{
		List<SaveLoader.PerformanceMeasurement> list = new List<SaveLoader.PerformanceMeasurement>();
		if (Global.Instance != null)
		{
			PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
			list.Add(new SaveLoader.PerformanceMeasurement
			{
				name = "FramesAbove30",
				value = component.NumFramesAbove30
			});
			list.Add(new SaveLoader.PerformanceMeasurement
			{
				name = "FramesBelow30",
				value = component.NumFramesBelow30
			});
			component.Reset();
		}
		return list;
	}

	// Token: 0x06004E48 RID: 20040 RVA: 0x001C2760 File Offset: 0x001C0960
	private float GetFrameTime()
	{
		PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
		DebugUtil.LogArgs(new object[]
		{
			"Average frame time:",
			1f / component.FPS
		});
		return 1f / component.FPS;
	}

	// Token: 0x06004E49 RID: 20041 RVA: 0x001C27AC File Offset: 0x001C09AC
	private List<SaveLoader.WorldMetricsData> GetWorldMetrics()
	{
		List<SaveLoader.WorldMetricsData> list = new List<SaveLoader.WorldMetricsData>();
		if (Global.Instance != null)
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior)
				{
					float discoveryTimestamp = worldContainer.IsDiscovered ? worldContainer.DiscoveryTimestamp : -1f;
					float dupeVisitedTimestamp = worldContainer.IsDupeVisited ? worldContainer.DupeVisitedTimestamp : -1f;
					list.Add(new SaveLoader.WorldMetricsData
					{
						Name = worldContainer.worldName,
						DiscoveryTimestamp = discoveryTimestamp,
						DupeVisitedTimestamp = dupeVisitedTimestamp
					});
				}
			}
		}
		return list;
	}

	// Token: 0x06004E4A RID: 20042 RVA: 0x001C2870 File Offset: 0x001C0A70
	public bool IsDLCActiveForCurrentSave(string dlcid)
	{
		return DlcManager.IsContentSubscribed(dlcid) && (dlcid == "" || dlcid == "" || this.GameInfo.dlcIds.Contains(dlcid));
	}

	// Token: 0x06004E4B RID: 20043 RVA: 0x001C28AC File Offset: 0x001C0AAC
	public bool IsDlcListActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return true;
		}
		foreach (string text in dlcIds)
		{
			if (text == "")
			{
				return true;
			}
			if (this.IsDLCActiveForCurrentSave(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004E4C RID: 20044 RVA: 0x001C28F4 File Offset: 0x001C0AF4
	public bool IsAllDlcActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return true;
		}
		foreach (string text in dlcIds)
		{
			if (!(text == "") && !this.IsDLCActiveForCurrentSave(text))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004E4D RID: 20045 RVA: 0x001C2938 File Offset: 0x001C0B38
	public bool IsAnyDlcActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return false;
		}
		foreach (string text in dlcIds)
		{
			if (!(text == "") && this.IsDLCActiveForCurrentSave(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004E4E RID: 20046 RVA: 0x001C297B File Offset: 0x001C0B7B
	public bool IsCorrectDlcActiveForCurrentSave(string[] required, string[] forbidden)
	{
		return this.IsAllDlcActiveForCurrentSave(required) && !this.IsAnyDlcActiveForCurrentSave(forbidden);
	}

	// Token: 0x06004E4F RID: 20047 RVA: 0x001C2994 File Offset: 0x001C0B94
	public string GetSaveLoadContentLetters()
	{
		if (this.GameInfo.dlcIds.Count <= 0)
		{
			return "V";
		}
		string text = "";
		foreach (string dlcId in this.GameInfo.dlcIds)
		{
			text += DlcManager.GetContentLetter(dlcId);
		}
		return text;
	}

	// Token: 0x06004E50 RID: 20048 RVA: 0x001C2A14 File Offset: 0x001C0C14
	public void UpgradeActiveSaveDLCInfo(string dlcId, bool trigger_load = false)
	{
		string activeSaveFolder = SaveLoader.GetActiveSaveFolder();
		string path = SaveGame.Instance.BaseName + UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND + ".sav";
		string filename = System.IO.Path.Combine(activeSaveFolder, path);
		this.Save(filename, false, false);
		if (!this.GameInfo.dlcIds.Contains(dlcId))
		{
			this.GameInfo.dlcIds.Add(dlcId);
		}
		string current_save = SaveLoader.GetActiveSaveFilePath();
		this.Save(SaveLoader.GetActiveSaveFilePath(), false, false);
		if (trigger_load)
		{
			LoadingOverlay.Load(delegate
			{
				LoadScreen.DoLoad(current_save);
			});
		}
	}

	// Token: 0x040033F7 RID: 13303
	[MyCmpGet]
	private GridSettings gridSettings;

	// Token: 0x040033F9 RID: 13305
	private bool saveFileCorrupt;

	// Token: 0x040033FA RID: 13306
	private bool compressSaveData = true;

	// Token: 0x040033FB RID: 13307
	private int lastUncompressedSize;

	// Token: 0x040033FC RID: 13308
	public bool saveAsText;

	// Token: 0x040033FD RID: 13309
	public const string MAINMENU_LEVELNAME = "launchscene";

	// Token: 0x040033FE RID: 13310
	public const string FRONTEND_LEVELNAME = "frontend";

	// Token: 0x040033FF RID: 13311
	public const string BACKEND_LEVELNAME = "backend";

	// Token: 0x04003400 RID: 13312
	public const string SAVE_EXTENSION = ".sav";

	// Token: 0x04003401 RID: 13313
	public const string AUTOSAVE_FOLDER = "auto_save";

	// Token: 0x04003402 RID: 13314
	public const string CLOUDSAVE_FOLDER = "cloud_save_files";

	// Token: 0x04003403 RID: 13315
	public const string SAVE_FOLDER = "save_files";

	// Token: 0x04003404 RID: 13316
	public const int MAX_AUTOSAVE_FILES = 10;

	// Token: 0x04003406 RID: 13318
	[NonSerialized]
	public SaveManager saveManager;

	// Token: 0x04003408 RID: 13320
	private Cluster m_cluster;

	// Token: 0x04003409 RID: 13321
	private ClusterLayout m_clusterLayout;

	// Token: 0x0400340B RID: 13323
	private const string CorruptFileSuffix = "_";

	// Token: 0x0400340C RID: 13324
	private const float SAVE_BUFFER_HEAD_ROOM = 0.1f;

	// Token: 0x0400340D RID: 13325
	private bool mustRestartOnFail;

	// Token: 0x04003410 RID: 13328
	public const string METRIC_SAVED_PREFAB_KEY = "SavedPrefabs";

	// Token: 0x04003411 RID: 13329
	public const string METRIC_IS_AUTO_SAVE_KEY = "IsAutoSave";

	// Token: 0x04003412 RID: 13330
	public const string METRIC_WAS_DEBUG_EVER_USED = "WasDebugEverUsed";

	// Token: 0x04003413 RID: 13331
	public const string METRIC_IS_SANDBOX_ENABLED = "IsSandboxEnabled";

	// Token: 0x04003414 RID: 13332
	public const string METRIC_RESOURCES_ACCESSIBLE_KEY = "ResourcesAccessible";

	// Token: 0x04003415 RID: 13333
	public const string METRIC_DAILY_REPORT_KEY = "DailyReport";

	// Token: 0x04003416 RID: 13334
	public const string METRIC_WORLD_METRICS_KEY = "WorldMetrics";

	// Token: 0x04003417 RID: 13335
	public const string METRIC_MINION_METRICS_KEY = "MinionMetrics";

	// Token: 0x04003418 RID: 13336
	public const string METRIC_CUSTOM_GAME_SETTINGS = "CustomGameSettings";

	// Token: 0x04003419 RID: 13337
	public const string METRIC_CUSTOM_MIXING_SETTINGS = "CustomMixingSettings";

	// Token: 0x0400341A RID: 13338
	public const string METRIC_PERFORMANCE_MEASUREMENTS = "PerformanceMeasurements";

	// Token: 0x0400341B RID: 13339
	public const string METRIC_FRAME_TIME = "AverageFrameTime";

	// Token: 0x0400341C RID: 13340
	private static bool force_infinity;

	// Token: 0x02001A91 RID: 6801
	public class FlowUtilityNetworkInstance
	{
		// Token: 0x04007CFE RID: 31998
		public int id = -1;

		// Token: 0x04007CFF RID: 31999
		public SimHashes containedElement = SimHashes.Vacuum;

		// Token: 0x04007D00 RID: 32000
		public float containedMass;

		// Token: 0x04007D01 RID: 32001
		public float containedTemperature;
	}

	// Token: 0x02001A92 RID: 6802
	[SerializationConfig(KSerialization.MemberSerialization.OptOut)]
	public class FlowUtilityNetworkSaver : ISaveLoadable
	{
		// Token: 0x0600A08B RID: 41099 RVA: 0x003801A6 File Offset: 0x0037E3A6
		public FlowUtilityNetworkSaver()
		{
			this.gas = new List<SaveLoader.FlowUtilityNetworkInstance>();
			this.liquid = new List<SaveLoader.FlowUtilityNetworkInstance>();
		}

		// Token: 0x04007D02 RID: 32002
		public List<SaveLoader.FlowUtilityNetworkInstance> gas;

		// Token: 0x04007D03 RID: 32003
		public List<SaveLoader.FlowUtilityNetworkInstance> liquid;
	}

	// Token: 0x02001A93 RID: 6803
	public struct SaveFileEntry
	{
		// Token: 0x04007D04 RID: 32004
		public string path;

		// Token: 0x04007D05 RID: 32005
		public System.DateTime timeStamp;
	}

	// Token: 0x02001A94 RID: 6804
	public enum SaveType
	{
		// Token: 0x04007D07 RID: 32007
		local,
		// Token: 0x04007D08 RID: 32008
		cloud,
		// Token: 0x04007D09 RID: 32009
		both
	}

	// Token: 0x02001A95 RID: 6805
	private struct MinionAttrFloatData
	{
		// Token: 0x04007D0A RID: 32010
		public string Name;

		// Token: 0x04007D0B RID: 32011
		public float Value;
	}

	// Token: 0x02001A96 RID: 6806
	private struct MinionMetricsData
	{
		// Token: 0x04007D0C RID: 32012
		public string Name;

		// Token: 0x04007D0D RID: 32013
		public List<SaveLoader.MinionAttrFloatData> Modifiers;

		// Token: 0x04007D0E RID: 32014
		public float TotalExperienceGained;

		// Token: 0x04007D0F RID: 32015
		public List<string> Skills;
	}

	// Token: 0x02001A97 RID: 6807
	private struct SavedPrefabMetricsData
	{
		// Token: 0x04007D10 RID: 32016
		public string PrefabName;

		// Token: 0x04007D11 RID: 32017
		public int Count;
	}

	// Token: 0x02001A98 RID: 6808
	private struct WorldInventoryMetricsData
	{
		// Token: 0x04007D12 RID: 32018
		public string Name;

		// Token: 0x04007D13 RID: 32019
		public float Amount;
	}

	// Token: 0x02001A99 RID: 6809
	private struct DailyReportMetricsData
	{
		// Token: 0x04007D14 RID: 32020
		public string Name;

		// Token: 0x04007D15 RID: 32021
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Net;

		// Token: 0x04007D16 RID: 32022
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Positive;

		// Token: 0x04007D17 RID: 32023
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Negative;
	}

	// Token: 0x02001A9A RID: 6810
	private struct PerformanceMeasurement
	{
		// Token: 0x04007D18 RID: 32024
		public string name;

		// Token: 0x04007D19 RID: 32025
		public float value;
	}

	// Token: 0x02001A9B RID: 6811
	private struct WorldMetricsData
	{
		// Token: 0x04007D1A RID: 32026
		public string Name;

		// Token: 0x04007D1B RID: 32027
		public float DiscoveryTimestamp;

		// Token: 0x04007D1C RID: 32028
		public float DupeVisitedTimestamp;
	}
}
