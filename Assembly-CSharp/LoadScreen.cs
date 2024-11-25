﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ProcGen;
using ProcGenGame;
using Steamworks;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B9E RID: 2974
public class LoadScreen : KModalScreen
{
	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x060059BB RID: 22971 RVA: 0x002070EC File Offset: 0x002052EC
	// (set) Token: 0x060059BC RID: 22972 RVA: 0x002070F3 File Offset: 0x002052F3
	public static LoadScreen Instance { get; private set; }

	// Token: 0x060059BD RID: 22973 RVA: 0x002070FB File Offset: 0x002052FB
	public static void DestroyInstance()
	{
		LoadScreen.Instance = null;
	}

	// Token: 0x060059BE RID: 22974 RVA: 0x00207104 File Offset: 0x00205304
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(LoadScreen.Instance == null);
		LoadScreen.Instance = this;
		base.OnPrefabInit();
		this.saveButtonPrefab.gameObject.SetActive(false);
		this.colonyListPool = new UIPool<HierarchyReferences>(this.saveButtonPrefab);
		if (SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		if (this.closeButton != null)
		{
			this.closeButton.onClick += delegate()
			{
				this.Deactivate();
			};
		}
		if (this.colonyCloudButton != null)
		{
			this.colonyCloudButton.onClick += delegate()
			{
				this.ConvertAllToCloud();
			};
		}
		if (this.colonyLocalButton != null)
		{
			this.colonyLocalButton.onClick += delegate()
			{
				this.ConvertAllToLocal();
			};
		}
		if (this.colonyInfoButton != null)
		{
			this.colonyInfoButton.onClick += delegate()
			{
				this.ShowSaveInfo();
			};
		}
		if (this.loadMoreButton != null)
		{
			this.loadMoreButton.onClick += delegate()
			{
				this.displayedPageCount++;
				this.RefreshColonyList();
				this.ShowColonyList();
			};
		}
	}

	// Token: 0x060059BF RID: 22975 RVA: 0x00207221 File Offset: 0x00205421
	private bool IsInMenu()
	{
		return App.GetCurrentSceneName() == "frontend";
	}

	// Token: 0x060059C0 RID: 22976 RVA: 0x00207232 File Offset: 0x00205432
	private bool CloudSavesVisible()
	{
		return SaveLoader.GetCloudSavesAvailable() && this.IsInMenu();
	}

	// Token: 0x060059C1 RID: 22977 RVA: 0x00207244 File Offset: 0x00205444
	protected override void OnActivate()
	{
		base.OnActivate();
		WorldGen.LoadSettings(false);
		this.SetCloudSaveInfoActive(this.CloudSavesVisible());
		this.displayedPageCount = 1;
		this.RefreshColonyList();
		this.ShowColonyList();
		bool cloudSavesAvailable = SaveLoader.GetCloudSavesAvailable();
		this.cloudTutorialBouncer.gameObject.SetActive(cloudSavesAvailable);
		if (cloudSavesAvailable && !this.cloudTutorialBouncer.IsBouncing())
		{
			int @int = KPlayerPrefs.GetInt("LoadScreenCloudTutorialTimes", 0);
			if (@int < 5)
			{
				this.cloudTutorialBouncer.Bounce();
				KPlayerPrefs.SetInt("LoadScreenCloudTutorialTimes", @int + 1);
				KPlayerPrefs.GetInt("LoadScreenCloudTutorialTimes", 0);
			}
			else
			{
				this.cloudTutorialBouncer.gameObject.SetActive(false);
			}
		}
		if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.colonyInfoButton.gameObject.SetActive(false);
		}
	}

	// Token: 0x060059C2 RID: 22978 RVA: 0x0020730C File Offset: 0x0020550C
	private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetColoniesDetails(List<SaveLoader.SaveFileEntry> files)
	{
		Dictionary<string, List<LoadScreen.SaveGameFileDetails>> dictionary = new Dictionary<string, List<LoadScreen.SaveGameFileDetails>>();
		if (files.Count <= 0)
		{
			return dictionary;
		}
		for (int i = 0; i < files.Count; i++)
		{
			if (this.IsFileValid(files[i].path))
			{
				global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(files[i].path);
				SaveGame.Header first = fileInfo.first;
				SaveGame.GameInfo second = fileInfo.second;
				System.DateTime timeStamp = files[i].timeStamp;
				long size = 0L;
				try
				{
					size = new FileInfo(files[i].path).Length;
				}
				catch (Exception ex)
				{
					global::Debug.LogWarning("Failed to get size for file: " + files[i].ToString() + "\n" + ex.ToString());
				}
				LoadScreen.SaveGameFileDetails saveGameFileDetails = new LoadScreen.SaveGameFileDetails
				{
					BaseName = second.baseName,
					FileName = files[i].path,
					FileDate = timeStamp,
					FileHeader = first,
					FileInfo = second,
					Size = size,
					UniqueID = SaveGame.GetSaveUniqueID(second)
				};
				if (!dictionary.ContainsKey(saveGameFileDetails.UniqueID))
				{
					dictionary.Add(saveGameFileDetails.UniqueID, new List<LoadScreen.SaveGameFileDetails>());
				}
				dictionary[saveGameFileDetails.UniqueID].Add(saveGameFileDetails);
			}
		}
		return dictionary;
	}

	// Token: 0x060059C3 RID: 22979 RVA: 0x00207474 File Offset: 0x00205674
	private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetColonies(bool sort)
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(sort, SaveLoader.SaveType.both);
		return this.GetColoniesDetails(allFiles);
	}

	// Token: 0x060059C4 RID: 22980 RVA: 0x00207490 File Offset: 0x00205690
	private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetLocalColonies(bool sort)
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(sort, SaveLoader.SaveType.local);
		return this.GetColoniesDetails(allFiles);
	}

	// Token: 0x060059C5 RID: 22981 RVA: 0x002074AC File Offset: 0x002056AC
	private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetCloudColonies(bool sort)
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(sort, SaveLoader.SaveType.cloud);
		return this.GetColoniesDetails(allFiles);
	}

	// Token: 0x060059C6 RID: 22982 RVA: 0x002074C8 File Offset: 0x002056C8
	private bool IsFileValid(string filename)
	{
		bool result = false;
		try
		{
			SaveGame.Header header;
			result = (SaveLoader.LoadHeader(filename, out header).saveMajorVersion >= 7);
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("Corrupted save file: " + filename + "\n" + ex.ToString());
		}
		return result;
	}

	// Token: 0x060059C7 RID: 22983 RVA: 0x0020751C File Offset: 0x0020571C
	private void CheckCloudLocalOverlap()
	{
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (cloudSavePrefix == null)
		{
			return;
		}
		foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in this.GetColonies(false))
		{
			bool flag = false;
			List<LoadScreen.SaveGameFileDetails> list = new List<LoadScreen.SaveGameFileDetails>();
			foreach (LoadScreen.SaveGameFileDetails saveGameFileDetails in keyValuePair.Value)
			{
				if (SaveLoader.IsSaveCloud(saveGameFileDetails.FileName))
				{
					flag = true;
				}
				else
				{
					list.Add(saveGameFileDetails);
				}
			}
			if (flag && list.Count != 0)
			{
				string baseName = list[0].BaseName;
				string path = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), baseName);
				string text = System.IO.Path.Combine(cloudSavePrefix, baseName);
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				global::Debug.Log("Saves / Found overlapped cloud/local saves for colony '" + baseName + "', moving to cloud...");
				foreach (LoadScreen.SaveGameFileDetails saveGameFileDetails2 in list)
				{
					string fileName = saveGameFileDetails2.FileName;
					string source = System.IO.Path.ChangeExtension(fileName, "png");
					string path2 = text;
					if (SaveLoader.IsSaveAuto(fileName))
					{
						string text2 = System.IO.Path.Combine(path2, "auto_save");
						if (!Directory.Exists(text2))
						{
							Directory.CreateDirectory(text2);
						}
						path2 = text2;
					}
					string text3 = System.IO.Path.Combine(path2, System.IO.Path.GetFileName(fileName));
					global::Tuple<bool, bool> tuple;
					if (this.FileMatch(fileName, text3, out tuple))
					{
						global::Debug.Log("Saves / file match found for `" + fileName + "`...");
						this.MigrateFile(fileName, text3, false);
						string dest = System.IO.Path.ChangeExtension(text3, "png");
						this.MigrateFile(source, dest, true);
					}
					else
					{
						global::Debug.Log("Saves / no file match found for `" + fileName + "`... move as copy");
						string nextUsableSavePath = SaveLoader.GetNextUsableSavePath(text3);
						this.MigrateFile(fileName, nextUsableSavePath, false);
						string dest2 = System.IO.Path.ChangeExtension(nextUsableSavePath, "png");
						this.MigrateFile(source, dest2, true);
					}
				}
				this.RemoveEmptyFolder(path);
			}
		}
	}

	// Token: 0x060059C8 RID: 22984 RVA: 0x00207794 File Offset: 0x00205994
	private void DeleteFileAndEmptyFolder(string file)
	{
		if (File.Exists(file))
		{
			File.Delete(file);
		}
		this.RemoveEmptyFolder(System.IO.Path.GetDirectoryName(file));
	}

	// Token: 0x060059C9 RID: 22985 RVA: 0x002077B0 File Offset: 0x002059B0
	private void RemoveEmptyFolder(string path)
	{
		if (!Directory.Exists(path))
		{
			return;
		}
		if (!File.GetAttributes(path).HasFlag(FileAttributes.Directory))
		{
			return;
		}
		if (Directory.EnumerateFileSystemEntries(path).Any<string>())
		{
			return;
		}
		try
		{
			Directory.Delete(path);
		}
		catch (Exception obj)
		{
			global::Debug.LogWarning("Failed to remove empty directory `" + path + "`...");
			global::Debug.LogWarning(obj);
		}
	}

	// Token: 0x060059CA RID: 22986 RVA: 0x00207824 File Offset: 0x00205A24
	private void RefreshColonyList()
	{
		if (this.colonyListPool != null)
		{
			this.colonyListPool.ClearAll();
		}
		this.CheckCloudLocalOverlap();
		Dictionary<string, List<LoadScreen.SaveGameFileDetails>> colonies = this.GetColonies(true);
		if (colonies.Count > 0)
		{
			int num = 0;
			foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in colonies)
			{
				if (num >= this.displayedPageCount * 20)
				{
					break;
				}
				this.AddColonyToList(keyValuePair.Value);
				num++;
			}
			this.loadMoreButton.gameObject.SetActive(colonies.Count != num);
			this.loadMoreButton.gameObject.transform.SetAsLastSibling();
		}
	}

	// Token: 0x060059CB RID: 22987 RVA: 0x002078E8 File Offset: 0x00205AE8
	private string GetFileHash(string path)
	{
		string result;
		using (MD5 md = MD5.Create())
		{
			using (FileStream fileStream = File.OpenRead(path))
			{
				result = BitConverter.ToString(md.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
			}
		}
		return result;
	}

	// Token: 0x060059CC RID: 22988 RVA: 0x00207958 File Offset: 0x00205B58
	private bool FileMatch(string file, string other_file, out global::Tuple<bool, bool> matches)
	{
		matches = new global::Tuple<bool, bool>(false, false);
		if (!File.Exists(file))
		{
			return false;
		}
		if (!File.Exists(other_file))
		{
			return false;
		}
		bool flag = false;
		bool flag2 = false;
		try
		{
			string fileHash = this.GetFileHash(file);
			string fileHash2 = this.GetFileHash(other_file);
			FileInfo fileInfo = new FileInfo(file);
			FileInfo fileInfo2 = new FileInfo(other_file);
			flag = (fileInfo.Length == fileInfo2.Length);
			flag2 = (fileHash == fileHash2);
		}
		catch (Exception obj)
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"FileMatch / file match failed for `",
				file,
				"` vs `",
				other_file,
				"`!"
			}));
			global::Debug.LogWarning(obj);
			return false;
		}
		matches.first = flag;
		matches.second = flag2;
		return flag && flag2;
	}

	// Token: 0x060059CD RID: 22989 RVA: 0x00207A20 File Offset: 0x00205C20
	private bool MigrateFile(string source, string dest, bool ignoreMissing = false)
	{
		global::Debug.Log(string.Concat(new string[]
		{
			"Migration / moving `",
			source,
			"` to `",
			dest,
			"` ..."
		}));
		if (dest == source)
		{
			global::Debug.Log(string.Concat(new string[]
			{
				"Migration / ignored `",
				source,
				"` to `",
				dest,
				"` ... same location"
			}));
			return true;
		}
		global::Tuple<bool, bool> tuple;
		if (this.FileMatch(source, dest, out tuple))
		{
			global::Debug.Log("Migration / dest and source are identical size + hash ... removing original");
			try
			{
				this.DeleteFileAndEmptyFolder(source);
			}
			catch (Exception ex)
			{
				global::Debug.LogWarning("Migration / removing original failed for `" + source + "`!");
				global::Debug.LogWarning(ex);
				throw ex;
			}
			return true;
		}
		try
		{
			global::Debug.Log("Migration / copying...");
			File.Copy(source, dest, false);
		}
		catch (FileNotFoundException obj) when (ignoreMissing)
		{
			global::Debug.Log("Migration / File `" + source + "` wasn't found but we're ignoring that.");
			return true;
		}
		catch (Exception ex2)
		{
			global::Debug.LogWarning("Migration / copy failed for `" + source + "`! Leaving it alone");
			global::Debug.LogWarning(ex2);
			global::Debug.LogWarning("failed to convert colony: " + ex2.ToString());
			throw ex2;
		}
		global::Debug.Log("Migration / copy ok ...");
		global::Tuple<bool, bool> tuple2;
		if (!this.FileMatch(source, dest, out tuple2))
		{
			global::Debug.LogWarning("Migration / failed to match dest file for `" + source + "`!");
			global::Debug.LogWarning(string.Format("Migration / did hash match? {0} did size match? {1}", tuple2.second, tuple2.first));
			throw new Exception("Hash/Size didn't match for source and destination");
		}
		global::Debug.Log("Migration / hash validation ok ... removing original");
		try
		{
			this.DeleteFileAndEmptyFolder(source);
		}
		catch (Exception ex3)
		{
			global::Debug.LogWarning("Migration / removing original failed for `" + source + "`!");
			global::Debug.LogWarning(ex3);
			throw ex3;
		}
		global::Debug.Log("Migration / moved ok for `" + source + "`!");
		return true;
	}

	// Token: 0x060059CE RID: 22990 RVA: 0x00207C24 File Offset: 0x00205E24
	private bool MigrateSave(string dest_root, string file, bool is_auto_save, out string saveError)
	{
		saveError = null;
		global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(file);
		SaveGame.Header first = fileInfo.first;
		string path = fileInfo.second.baseName.TrimEnd(' ');
		string fileName = System.IO.Path.GetFileName(file);
		string text = System.IO.Path.Combine(dest_root, path);
		if (!Directory.Exists(text))
		{
			text = Directory.CreateDirectory(text).FullName;
		}
		string path2 = text;
		if (is_auto_save)
		{
			string text2 = System.IO.Path.Combine(text, "auto_save");
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			path2 = text2;
		}
		string text3 = System.IO.Path.Combine(path2, fileName);
		string source = System.IO.Path.ChangeExtension(file, "png");
		string dest = System.IO.Path.ChangeExtension(text3, "png");
		try
		{
			this.MigrateFile(file, text3, false);
			this.MigrateFile(source, dest, true);
		}
		catch (Exception ex)
		{
			saveError = ex.Message;
			return false;
		}
		return true;
	}

	// Token: 0x060059CF RID: 22991 RVA: 0x00207D00 File Offset: 0x00205F00
	private ValueTuple<int, int, ulong> GetSavesSizeAndCounts(List<LoadScreen.SaveGameFileDetails> list)
	{
		ulong num = 0UL;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			LoadScreen.SaveGameFileDetails saveGameFileDetails = list[i];
			num += (ulong)saveGameFileDetails.Size;
			if (saveGameFileDetails.FileInfo.isAutoSave)
			{
				num3++;
			}
			else
			{
				num2++;
			}
		}
		return new ValueTuple<int, int, ulong>(num2, num3, num);
	}

	// Token: 0x060059D0 RID: 22992 RVA: 0x00207D58 File Offset: 0x00205F58
	private int CountValidSaves(string path, SearchOption searchType = SearchOption.AllDirectories)
	{
		int num = 0;
		List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(path, false, searchType);
		for (int i = 0; i < saveFiles.Count; i++)
		{
			if (this.IsFileValid(saveFiles[i].path))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060059D1 RID: 22993 RVA: 0x00207D9C File Offset: 0x00205F9C
	private ValueTuple<int, int> GetMigrationSaveCounts()
	{
		int item = this.CountValidSaves(SaveLoader.GetSavePrefixAndCreateFolder(), SearchOption.TopDirectoryOnly);
		int item2 = this.CountValidSaves(SaveLoader.GetAutoSavePrefix(), SearchOption.AllDirectories);
		return new ValueTuple<int, int>(item, item2);
	}

	// Token: 0x060059D2 RID: 22994 RVA: 0x00207DC8 File Offset: 0x00205FC8
	private ValueTuple<int, int> MigrateSaves(out string errorColony, out string errorMessage)
	{
		errorColony = null;
		errorMessage = null;
		int num = 0;
		string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
		List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(savePrefixAndCreateFolder, false, SearchOption.TopDirectoryOnly);
		for (int i = 0; i < saveFiles.Count; i++)
		{
			SaveLoader.SaveFileEntry saveFileEntry = saveFiles[i];
			if (this.IsFileValid(saveFileEntry.path))
			{
				string text;
				if (this.MigrateSave(savePrefixAndCreateFolder, saveFileEntry.path, false, out text))
				{
					num++;
				}
				else if (errorColony == null)
				{
					errorColony = saveFileEntry.path;
					errorMessage = text;
				}
			}
		}
		int num2 = 0;
		List<SaveLoader.SaveFileEntry> saveFiles2 = SaveLoader.GetSaveFiles(SaveLoader.GetAutoSavePrefix(), false, SearchOption.AllDirectories);
		for (int j = 0; j < saveFiles2.Count; j++)
		{
			SaveLoader.SaveFileEntry saveFileEntry2 = saveFiles2[j];
			if (this.IsFileValid(saveFileEntry2.path))
			{
				string text2;
				if (this.MigrateSave(savePrefixAndCreateFolder, saveFileEntry2.path, true, out text2))
				{
					num2++;
				}
				else if (errorColony == null)
				{
					errorColony = saveFileEntry2.path;
					errorMessage = text2;
				}
			}
		}
		return new ValueTuple<int, int>(num, num2);
	}

	// Token: 0x060059D3 RID: 22995 RVA: 0x00207EB8 File Offset: 0x002060B8
	public void ShowMigrationIfNecessary(bool fromMainMenu)
	{
		ValueTuple<int, int> migrationSaveCounts = this.GetMigrationSaveCounts();
		int saveCount = migrationSaveCounts.Item1;
		int autoCount = migrationSaveCounts.Item2;
		if (saveCount == 0 && autoCount == 0)
		{
			if (fromMainMenu)
			{
				this.Deactivate();
			}
			return;
		}
		base.Activate();
		this.migrationPanelRefs.gameObject.SetActive(true);
		KButton migrateButton = this.migrationPanelRefs.GetReference<RectTransform>("MigrateSaves").GetComponent<KButton>();
		KButton continueButton = this.migrationPanelRefs.GetReference<RectTransform>("Continue").GetComponent<KButton>();
		KButton moreInfoButton = this.migrationPanelRefs.GetReference<RectTransform>("MoreInfo").GetComponent<KButton>();
		KButton component = this.migrationPanelRefs.GetReference<RectTransform>("OpenSaves").GetComponent<KButton>();
		LocText statsText = this.migrationPanelRefs.GetReference<RectTransform>("CountText").GetComponent<LocText>();
		LocText infoText = this.migrationPanelRefs.GetReference<RectTransform>("InfoText").GetComponent<LocText>();
		migrateButton.gameObject.SetActive(true);
		continueButton.gameObject.SetActive(false);
		moreInfoButton.gameObject.SetActive(false);
		statsText.text = string.Format(UI.FRONTEND.LOADSCREEN.MIGRATE_COUNT, saveCount, autoCount);
		component.ClearOnClick();
		component.onClick += delegate()
		{
			App.OpenWebURL(SaveLoader.GetSavePrefixAndCreateFolder());
		};
		migrateButton.ClearOnClick();
		migrateButton.onClick += delegate()
		{
			migrateButton.gameObject.SetActive(false);
			string text;
			string newValue;
			ValueTuple<int, int> valueTuple = this.MigrateSaves(out text, out newValue);
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			bool flag = text == null;
			string format = flag ? UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT.text : UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES.Replace("{ErrorColony}", text).Replace("{ErrorMessage}", newValue);
			statsText.text = string.Format(format, new object[]
			{
				item,
				saveCount,
				item2,
				autoCount
			});
			infoText.gameObject.SetActive(false);
			if (flag)
			{
				continueButton.gameObject.SetActive(true);
			}
			else
			{
				moreInfoButton.gameObject.SetActive(true);
			}
			MainMenu.Instance.RefreshResumeButton(false);
			this.RefreshColonyList();
		};
		continueButton.ClearOnClick();
		continueButton.onClick += delegate()
		{
			this.migrationPanelRefs.gameObject.SetActive(false);
			this.cloudTutorialBouncer.Bounce();
		};
		moreInfoButton.ClearOnClick();
		Action<InfoDialogScreen> <>9__4;
		Action<InfoDialogScreen> <>9__6;
		moreInfoButton.onClick += delegate()
		{
			if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
			{
				InfoDialogScreen infoDialogScreen = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE).AddPlainText(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_PRE).AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1, "").AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2, "").AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3, "").AddPlainText(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_POST);
				string text = UI.CONFIRMDIALOG.OK;
				Action<InfoDialogScreen> action;
				if ((action = <>9__4) == null)
				{
					action = (<>9__4 = delegate(InfoDialogScreen d)
					{
						this.migrationPanelRefs.gameObject.SetActive(false);
						this.cloudTutorialBouncer.Bounce();
						d.Deactivate();
					});
				}
				infoDialogScreen.AddOption(text, action, true).Activate();
				return;
			}
			InfoDialogScreen infoDialogScreen2 = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE).AddPlainText(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_PRE).AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1, "").AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2, "").AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3, "").AddPlainText(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_POST).AddOption(UI.FRONTEND.LOADSCREEN.MIGRATE_FAILURES_FORUM_BUTTON, delegate(InfoDialogScreen d)
			{
				App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
			}, false);
			string text2 = UI.CONFIRMDIALOG.OK;
			Action<InfoDialogScreen> action2;
			if ((action2 = <>9__6) == null)
			{
				action2 = (<>9__6 = delegate(InfoDialogScreen d)
				{
					this.migrationPanelRefs.gameObject.SetActive(false);
					this.cloudTutorialBouncer.Bounce();
					d.Deactivate();
				});
			}
			infoDialogScreen2.AddOption(text2, action2, true).Activate();
		};
	}

	// Token: 0x060059D4 RID: 22996 RVA: 0x002080B1 File Offset: 0x002062B1
	private void SetCloudSaveInfoActive(bool active)
	{
		this.colonyCloudButton.gameObject.SetActive(active);
		this.colonyLocalButton.gameObject.SetActive(active);
	}

	// Token: 0x060059D5 RID: 22997 RVA: 0x002080D8 File Offset: 0x002062D8
	private bool ConvertToLocalOrCloud(string fromRoot, string destRoot, string colonyName)
	{
		string text = System.IO.Path.Combine(fromRoot, colonyName);
		string text2 = System.IO.Path.Combine(destRoot, colonyName);
		global::Debug.Log(string.Concat(new string[]
		{
			"Convert / Colony '",
			colonyName,
			"' from `",
			text,
			"` => `",
			text2,
			"`"
		}));
		try
		{
			Directory.Move(text, text2);
			return true;
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("failed to convert colony: " + ex.ToString());
			string message = UI.FRONTEND.LOADSCREEN.CONVERT_ERROR.Replace("{Colony}", colonyName).Replace("{Error}", ex.Message);
			this.ShowConvertError(message);
		}
		return false;
	}

	// Token: 0x060059D6 RID: 22998 RVA: 0x00208194 File Offset: 0x00206394
	private bool ConvertColonyToCloud(string colonyName)
	{
		string savePrefix = SaveLoader.GetSavePrefix();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (cloudSavePrefix == null)
		{
			global::Debug.LogWarning("Failed to move colony to cloud, no cloud save prefix found (usually a userID is missing, not logged in?)");
			return false;
		}
		return this.ConvertToLocalOrCloud(savePrefix, cloudSavePrefix, colonyName);
	}

	// Token: 0x060059D7 RID: 22999 RVA: 0x002081C8 File Offset: 0x002063C8
	private bool ConvertColonyToLocal(string colonyName)
	{
		string savePrefix = SaveLoader.GetSavePrefix();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (cloudSavePrefix == null)
		{
			global::Debug.LogWarning("Failed to move colony from cloud, no cloud save prefix found (usually a userID is missing, not logged in?)");
			return false;
		}
		return this.ConvertToLocalOrCloud(cloudSavePrefix, savePrefix, colonyName);
	}

	// Token: 0x060059D8 RID: 23000 RVA: 0x002081FC File Offset: 0x002063FC
	private void DoConvertAllToLocal()
	{
		Dictionary<string, List<LoadScreen.SaveGameFileDetails>> cloudColonies = this.GetCloudColonies(false);
		if (cloudColonies.Count == 0)
		{
			return;
		}
		bool flag = true;
		foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in cloudColonies)
		{
			flag &= this.ConvertColonyToLocal(keyValuePair.Value[0].BaseName);
		}
		if (flag)
		{
			string replacement = UI.PLATFORMS.STEAM;
			this.ShowSimpleDialog(UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_TO_LOCAL_SUCCESS.Replace("{Client}", replacement));
		}
		this.RefreshColonyList();
		MainMenu.Instance.RefreshResumeButton(false);
		SaveLoader.SetCloudSavesDefault(false);
	}

	// Token: 0x060059D9 RID: 23001 RVA: 0x002082B8 File Offset: 0x002064B8
	private void DoConvertAllToCloud()
	{
		Dictionary<string, List<LoadScreen.SaveGameFileDetails>> localColonies = this.GetLocalColonies(false);
		if (localColonies.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in localColonies)
		{
			string baseName = keyValuePair.Value[0].BaseName;
			if (!list.Contains(baseName))
			{
				list.Add(baseName);
			}
		}
		bool flag = true;
		foreach (string colonyName in list)
		{
			flag &= this.ConvertColonyToCloud(colonyName);
		}
		if (flag)
		{
			string replacement = UI.PLATFORMS.STEAM;
			this.ShowSimpleDialog(UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_TO_CLOUD_SUCCESS.Replace("{Client}", replacement));
		}
		this.RefreshColonyList();
		MainMenu.Instance.RefreshResumeButton(false);
		SaveLoader.SetCloudSavesDefault(true);
	}

	// Token: 0x060059DA RID: 23002 RVA: 0x002083CC File Offset: 0x002065CC
	private void ConvertAllToCloud()
	{
		string message = string.Format("{0}\n{1}\n", UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD_DETAILS, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_WARNING);
		KPlayerPrefs.SetInt("LoadScreenCloudTutorialTimes", 5);
		this.ConfirmCloudSaveMigrations(message, UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_COLONIES, UI.FRONTEND.LOADSCREEN.OPEN_SAVE_FOLDER, delegate
		{
			this.DoConvertAllToCloud();
		}, delegate
		{
			App.OpenWebURL(SaveLoader.GetSavePrefix());
		}, this.localToCloudSprite);
	}

	// Token: 0x060059DB RID: 23003 RVA: 0x00208450 File Offset: 0x00206650
	private void ConvertAllToLocal()
	{
		string message = string.Format("{0}\n{1}\n", UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL_DETAILS, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_WARNING);
		KPlayerPrefs.SetInt("LoadScreenCloudTutorialTimes", 5);
		this.ConfirmCloudSaveMigrations(message, UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_COLONIES, UI.FRONTEND.LOADSCREEN.OPEN_SAVE_FOLDER, delegate
		{
			this.DoConvertAllToLocal();
		}, delegate
		{
			App.OpenWebURL(SaveLoader.GetCloudSavePrefix());
		}, this.cloudToLocalSprite);
	}

	// Token: 0x060059DC RID: 23004 RVA: 0x002084D4 File Offset: 0x002066D4
	private void ShowSaveInfo()
	{
		if (this.infoScreen == null)
		{
			this.infoScreen = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.SAVE_INFO_DIALOG_TITLE).AddSprite(this.infoSprite).AddPlainText(UI.FRONTEND.LOADSCREEN.SAVE_INFO_DIALOG_TEXT).AddOption(UI.FRONTEND.LOADSCREEN.OPEN_SAVE_FOLDER, delegate(InfoDialogScreen d)
			{
				App.OpenWebURL(SaveLoader.GetSavePrefix());
			}, true).AddDefaultCancel();
			string cloudRoot = SaveLoader.GetCloudSavePrefix();
			if (cloudRoot != null && this.CloudSavesVisible())
			{
				this.infoScreen.AddOption(UI.FRONTEND.LOADSCREEN.OPEN_CLOUDSAVE_FOLDER, delegate(InfoDialogScreen d)
				{
					App.OpenWebURL(cloudRoot);
				}, true);
			}
			this.infoScreen.gameObject.SetActive(true);
		}
	}

	// Token: 0x060059DD RID: 23005 RVA: 0x002085C5 File Offset: 0x002067C5
	protected override void OnDeactivate()
	{
		if (SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		this.selectedSave = null;
		base.OnDeactivate();
	}

	// Token: 0x060059DE RID: 23006 RVA: 0x002085EC File Offset: 0x002067EC
	private void ShowColonyList()
	{
		this.colonyListRoot.SetActive(true);
		this.colonyViewRoot.SetActive(false);
		this.currentColony = null;
		this.selectedSave = null;
	}

	// Token: 0x060059DF RID: 23007 RVA: 0x00208614 File Offset: 0x00206814
	private bool CheckSaveVersion(LoadScreen.SaveGameFileDetails save, LocText display)
	{
		if (LoadScreen.IsSaveFileFromUnsupportedFutureBuild(save.FileHeader, save.FileInfo))
		{
			if (display != null)
			{
				display.text = string.Format(UI.FRONTEND.LOADSCREEN.SAVE_TOO_NEW, new object[]
				{
					save.FileName,
					save.FileHeader.buildVersion,
					save.FileInfo.saveMinorVersion,
					642695U,
					35
				});
			}
			return false;
		}
		if (save.FileInfo.saveMajorVersion < 7)
		{
			if (display != null)
			{
				display.text = string.Format(UI.FRONTEND.LOADSCREEN.UNSUPPORTED_SAVE_VERSION, new object[]
				{
					save.FileName,
					save.FileInfo.saveMajorVersion,
					save.FileInfo.saveMinorVersion,
					7,
					35
				});
			}
			return false;
		}
		return true;
	}

	// Token: 0x060059E0 RID: 23008 RVA: 0x00208718 File Offset: 0x00206918
	private bool CheckSaveDLCsCompatable(LoadScreen.SaveGameFileDetails save)
	{
		HashSet<string> hashSet;
		HashSet<string> hashSet2;
		return save.FileInfo.IsCompatableWithCurrentDlcConfiguration(out hashSet, out hashSet2);
	}

	// Token: 0x060059E1 RID: 23009 RVA: 0x00208738 File Offset: 0x00206938
	private string GetSaveDLCIncompatabilityTooltip(LoadScreen.SaveGameFileDetails save)
	{
		string text = "";
		HashSet<string> hashSet;
		HashSet<string> hashSet2;
		if (save.FileInfo.IsCompatableWithCurrentDlcConfiguration(out hashSet, out hashSet2))
		{
			text = null;
		}
		else
		{
			text = UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION;
			foreach (string dlcId in hashSet)
			{
				text = text + "\n" + string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_ENABLE, DlcManager.GetDlcTitle(dlcId));
			}
			foreach (string dlcId2 in hashSet2)
			{
				text = text + "\n" + string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_DISABLE, DlcManager.GetDlcTitle(dlcId2));
			}
		}
		return text;
	}

	// Token: 0x060059E2 RID: 23010 RVA: 0x00208828 File Offset: 0x00206A28
	private void ShowColonySave(LoadScreen.SaveGameFileDetails save)
	{
		HierarchyReferences component = this.colonyViewRoot.GetComponent<HierarchyReferences>();
		component.GetReference<RectTransform>("Title").GetComponent<LocText>().text = save.BaseName;
		component.GetReference<RectTransform>("Date").GetComponent<LocText>().text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), save.FileDate.ToLocalTime());
		string text = save.FileInfo.clusterId;
		if (text != null && !SettingsCache.clusterLayouts.clusterCache.ContainsKey(text))
		{
			string text2 = SettingsCache.GetScope("EXPANSION1_ID") + text;
			if (SettingsCache.clusterLayouts.clusterCache.ContainsKey(text2))
			{
				text = text2;
			}
			else
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Failed to find cluster " + text + " including the scoped path, setting to default cluster name."
				});
				global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
				text = WorldGenSettings.ClusterDefaultName;
			}
		}
		ProcGen.World world = (text != null) ? SettingsCache.clusterLayouts.GetWorldData(text, 0) : null;
		string arg = (world != null) ? Strings.Get(world.name) : " - ";
		component.GetReference<LocText>("InfoWorld").text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_INFO_FMT, UI.FRONTEND.LOADSCREEN.WORLD_NAME, arg);
		component.GetReference<LocText>("InfoCycles").text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_INFO_FMT, UI.FRONTEND.LOADSCREEN.CYCLES_SURVIVED, save.FileInfo.numberOfCycles);
		component.GetReference<LocText>("InfoDupes").text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_INFO_FMT, UI.FRONTEND.LOADSCREEN.DUPLICANTS_ALIVE, save.FileInfo.numberOfDuplicants);
		TMP_Text component2 = component.GetReference<RectTransform>("FileSize").GetComponent<LocText>();
		string formattedBytes = GameUtil.GetFormattedBytes((ulong)save.Size);
		component2.text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_FILE_SIZE, formattedBytes);
		component.GetReference<RectTransform>("Filename").GetComponent<LocText>().text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_FILE_NAME, System.IO.Path.GetFileName(save.FileName));
		LocText component3 = component.GetReference<RectTransform>("AutoInfo").GetComponent<LocText>();
		component3.gameObject.SetActive(!this.CheckSaveVersion(save, component3));
		Image component4 = component.GetReference<RectTransform>("Preview").GetComponent<Image>();
		this.SetPreview(save.FileName, save.BaseName, component4, false);
		KButton component5 = component.GetReference<RectTransform>("DeleteButton").GetComponent<KButton>();
		component5.ClearOnClick();
		System.Action <>9__1;
		component5.onClick += delegate()
		{
			LoadScreen <>4__this = this;
			System.Action onDelete;
			if ((onDelete = <>9__1) == null)
			{
				onDelete = (<>9__1 = delegate()
				{
					int num = this.currentColony.IndexOf(save);
					this.currentColony.Remove(save);
					this.ShowColony(this.currentColony, num - 1);
				});
			}
			<>4__this.Delete(onDelete);
		};
	}

	// Token: 0x060059E3 RID: 23011 RVA: 0x00208B10 File Offset: 0x00206D10
	private void ShowColony(List<LoadScreen.SaveGameFileDetails> saves, int selectIndex = -1)
	{
		if (saves.Count <= 0)
		{
			this.RefreshColonyList();
			this.ShowColonyList();
			return;
		}
		this.currentColony = saves;
		this.colonyListRoot.SetActive(false);
		this.colonyViewRoot.SetActive(true);
		string baseName = saves[0].BaseName;
		HierarchyReferences component = this.colonyViewRoot.GetComponent<HierarchyReferences>();
		KButton component2 = component.GetReference<RectTransform>("Back").GetComponent<KButton>();
		component2.ClearOnClick();
		component2.onClick += delegate()
		{
			this.ShowColonyList();
		};
		component.GetReference<RectTransform>("ColonyTitle").GetComponent<LocText>().text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_TITLE, baseName);
		GameObject gameObject = component.GetReference<RectTransform>("Content").gameObject;
		RectTransform reference = component.GetReference<RectTransform>("SaveTemplate");
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
			if (gameObject2 != null && gameObject2.name.Contains("Clone"))
			{
				UnityEngine.Object.Destroy(gameObject2);
			}
		}
		if (selectIndex < 0)
		{
			selectIndex = 0;
		}
		if (selectIndex > saves.Count - 1)
		{
			selectIndex = saves.Count - 1;
		}
		for (int j = 0; j < saves.Count; j++)
		{
			LoadScreen.SaveGameFileDetails save = saves[j];
			RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(reference, gameObject.transform);
			HierarchyReferences component3 = rectTransform.GetComponent<HierarchyReferences>();
			rectTransform.gameObject.SetActive(true);
			component3.GetReference<RectTransform>("AutoLabel").gameObject.SetActive(save.FileInfo.isAutoSave);
			component3.GetReference<RectTransform>("SaveText").GetComponent<LocText>().text = System.IO.Path.GetFileNameWithoutExtension(save.FileName);
			component3.GetReference<RectTransform>("DateText").GetComponent<LocText>().text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), save.FileDate.ToLocalTime());
			component3.GetReference<RectTransform>("NewestLabel").gameObject.SetActive(j == 0);
			RectTransform reference2 = component3.GetReference<RectTransform>("DLCIconPrefab");
			foreach (string dlcId in save.FileInfo.dlcIds)
			{
				GameObject gameObject3 = global::Util.KInstantiateUI(reference2.gameObject, reference2.transform.parent.gameObject, true);
				gameObject3.GetComponent<Image>().sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(dlcId));
				gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, DlcManager.GetDlcTitle(dlcId)));
			}
			object obj = this.CheckSaveVersion(save, null) && this.CheckSaveDLCsCompatable(save);
			KButton button = rectTransform.GetComponent<KButton>();
			button.ClearOnClick();
			button.onClick += delegate()
			{
				this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
				this.ShowColonySave(save);
			};
			object obj2 = obj;
			if (obj2 != null)
			{
				button.onDoubleClick += delegate()
				{
					this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
					this.Load();
				};
			}
			KButton component4 = component3.GetReference<RectTransform>("LoadButton").GetComponent<KButton>();
			component4.ClearOnClick();
			if (obj2 == null)
			{
				component4.isInteractable = false;
				component4.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Disabled);
				component4.GetComponent<ToolTip>().SetSimpleTooltip(this.GetSaveDLCIncompatabilityTooltip(save));
			}
			else
			{
				component4.onClick += delegate()
				{
					this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
					this.Load();
				};
			}
			if (j == selectIndex)
			{
				this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
				this.ShowColonySave(save);
			}
		}
	}

	// Token: 0x060059E4 RID: 23012 RVA: 0x00208F04 File Offset: 0x00207104
	private void AddColonyToList(List<LoadScreen.SaveGameFileDetails> saves)
	{
		if (saves.Count == 0)
		{
			return;
		}
		HierarchyReferences freeElement = this.colonyListPool.GetFreeElement(this.saveButtonRoot, true);
		saves.Sort((LoadScreen.SaveGameFileDetails x, LoadScreen.SaveGameFileDetails y) => y.FileDate.CompareTo(x.FileDate));
		LoadScreen.SaveGameFileDetails saveGameFileDetails = saves[0];
		string colonyName = saveGameFileDetails.BaseName;
		ValueTuple<int, int, ulong> savesSizeAndCounts = this.GetSavesSizeAndCounts(saves);
		int item = savesSizeAndCounts.Item1;
		int item2 = savesSizeAndCounts.Item2;
		string formattedBytes = GameUtil.GetFormattedBytes(savesSizeAndCounts.Item3);
		freeElement.GetReference<RectTransform>("HeaderTitle").GetComponent<LocText>().text = colonyName;
		freeElement.GetReference<RectTransform>("HeaderDate").GetComponent<LocText>().text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), saveGameFileDetails.FileDate.ToLocalTime());
		freeElement.GetReference<RectTransform>("SaveTitle").GetComponent<LocText>().text = string.Format(UI.FRONTEND.LOADSCREEN.SAVE_INFO, item, item2, formattedBytes);
		Image component = freeElement.GetReference<RectTransform>("Preview").GetComponent<Image>();
		this.SetPreview(saveGameFileDetails.FileName, colonyName, component, true);
		List<ValueTuple<Sprite, string>> list = new List<ValueTuple<Sprite, string>>();
		if (saveGameFileDetails.FileInfo.dlcIds.Contains("EXPANSION1_ID"))
		{
			list.Add(new ValueTuple<Sprite, string>(Assets.GetSprite(DlcManager.GetDlcSmallLogo("EXPANSION1_ID")), string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, DlcManager.GetDlcTitle("EXPANSION1_ID"))));
		}
		foreach (string text in saveGameFileDetails.FileInfo.dlcIds)
		{
			if (DlcManager.IsDlcId(text) && !(text == "EXPANSION1_ID"))
			{
				list.Add(new ValueTuple<Sprite, string>(Assets.GetSprite(DlcManager.GetDlcSmallLogo(text)), string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, DlcManager.GetDlcTitle(text))));
			}
		}
		GameObject gameObject = freeElement.transform.Find("Header").Find("DlcIcons").Find("Prefab_DlcIcon").gameObject;
		gameObject.SetActive(false);
		for (int i = 0; i < gameObject.transform.parent.childCount; i++)
		{
			GameObject gameObject2 = gameObject.transform.parent.GetChild(i).gameObject;
			if (gameObject2 != gameObject)
			{
				UnityEngine.Object.Destroy(gameObject2);
			}
		}
		foreach (ValueTuple<Sprite, string> valueTuple in list)
		{
			GameObject gameObject3 = global::Util.KInstantiateUI(gameObject, gameObject.transform.parent.gameObject, true);
			Image component2 = gameObject3.GetComponent<Image>();
			ToolTip component3 = gameObject3.GetComponent<ToolTip>();
			component2.sprite = valueTuple.Item1;
			component3.SetSimpleTooltip(valueTuple.Item2);
		}
		Component reference = freeElement.GetReference<RectTransform>("LocationIcons");
		bool flag = this.CloudSavesVisible();
		reference.gameObject.SetActive(flag);
		if (flag)
		{
			LocText locationText = freeElement.GetReference<RectTransform>("LocationText").GetComponent<LocText>();
			bool isLocal = SaveLoader.IsSaveLocal(saveGameFileDetails.FileName);
			locationText.text = (isLocal ? UI.FRONTEND.LOADSCREEN.LOCAL_SAVE : UI.FRONTEND.LOADSCREEN.CLOUD_SAVE);
			KButton cloudButton = freeElement.GetReference<RectTransform>("CloudButton").GetComponent<KButton>();
			KButton localButton = freeElement.GetReference<RectTransform>("LocalButton").GetComponent<KButton>();
			cloudButton.gameObject.SetActive(!isLocal);
			cloudButton.ClearOnClick();
			System.Action <>9__4;
			cloudButton.onClick += delegate()
			{
				string text2 = string.Format("{0}\n", UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL_DETAILS);
				LoadScreen <>4__this = this;
				string message = text2;
				string title = UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL;
				string confirmText = UI.FRONTEND.LOADSCREEN.CONVERT_COLONY;
				string backupText = null;
				System.Action commitAction;
				if ((commitAction = <>9__4) == null)
				{
					commitAction = (<>9__4 = delegate()
					{
						cloudButton.gameObject.SetActive(false);
						isLocal = true;
						locationText.text = (isLocal ? UI.FRONTEND.LOADSCREEN.LOCAL_SAVE : UI.FRONTEND.LOADSCREEN.CLOUD_SAVE);
						this.ConvertColonyToLocal(colonyName);
						this.RefreshColonyList();
						MainMenu.Instance.RefreshResumeButton(false);
					});
				}
				<>4__this.ConfirmCloudSaveMigrations(message, title, confirmText, backupText, commitAction, null, this.cloudToLocalSprite);
			};
			localButton.gameObject.SetActive(isLocal);
			localButton.ClearOnClick();
			System.Action <>9__5;
			localButton.onClick += delegate()
			{
				string text2 = string.Format("{0}\n", UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD_DETAILS);
				LoadScreen <>4__this = this;
				string message = text2;
				string title = UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD;
				string confirmText = UI.FRONTEND.LOADSCREEN.CONVERT_COLONY;
				string backupText = null;
				System.Action commitAction;
				if ((commitAction = <>9__5) == null)
				{
					commitAction = (<>9__5 = delegate()
					{
						localButton.gameObject.SetActive(false);
						isLocal = false;
						locationText.text = (isLocal ? UI.FRONTEND.LOADSCREEN.LOCAL_SAVE : UI.FRONTEND.LOADSCREEN.CLOUD_SAVE);
						this.ConvertColonyToCloud(colonyName);
						this.RefreshColonyList();
						MainMenu.Instance.RefreshResumeButton(false);
					});
				}
				<>4__this.ConfirmCloudSaveMigrations(message, title, confirmText, backupText, commitAction, null, this.localToCloudSprite);
			};
		}
		this.GetSaveDLCIncompatabilityTooltip(saveGameFileDetails);
		freeElement.GetReference<RectTransform>("Button").GetComponent<KButton>().onClick += delegate()
		{
			this.ShowColony(saves, -1);
		};
		freeElement.transform.SetAsLastSibling();
	}

	// Token: 0x060059E5 RID: 23013 RVA: 0x002093A4 File Offset: 0x002075A4
	private void SetPreview(string filename, string basename, Image preview, bool fallbackToTimelapse = false)
	{
		preview.color = Color.black;
		preview.gameObject.SetActive(false);
		try
		{
			Sprite sprite = RetireColonyUtility.LoadColonyPreview(filename, basename, fallbackToTimelapse);
			if (!(sprite == null))
			{
				Rect rect = preview.rectTransform.parent.rectTransform().rect;
				preview.sprite = sprite;
				preview.color = (sprite ? Color.white : Color.black);
				float num = sprite.bounds.size.x / sprite.bounds.size.y;
				if ((double)num >= 1.77777777777778)
				{
					preview.rectTransform.sizeDelta = new Vector2(rect.height * num, rect.height);
				}
				else
				{
					preview.rectTransform.sizeDelta = new Vector2(rect.width, rect.width / num);
				}
				preview.gameObject.SetActive(true);
			}
		}
		catch (Exception obj)
		{
			global::Debug.Log(obj);
		}
	}

	// Token: 0x060059E6 RID: 23014 RVA: 0x002094B4 File Offset: 0x002076B4
	public static void ForceStopGame()
	{
		ThreadedHttps<KleiMetrics>.Instance.ClearGameFields();
		ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
		Game.Instance.SetIsLoading();
		Grid.CellCount = 0;
		Sim.Shutdown();
	}

	// Token: 0x060059E7 RID: 23015 RVA: 0x002094E0 File Offset: 0x002076E0
	private static bool IsSaveFileFromUnsupportedFutureBuild(SaveGame.Header header, SaveGame.GameInfo gameInfo)
	{
		return gameInfo.saveMajorVersion > 7 || (gameInfo.saveMajorVersion == 7 && gameInfo.saveMinorVersion > 35) || header.buildVersion > 642695U;
	}

	// Token: 0x060059E8 RID: 23016 RVA: 0x00209510 File Offset: 0x00207710
	private void UpdateSelected(KButton button, string filename, List<string> dlcIds)
	{
		if (this.selectedSave != null && this.selectedSave.button != null)
		{
			this.selectedSave.button.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
		}
		if (this.selectedSave == null)
		{
			this.selectedSave = new LoadScreen.SelectedSave();
		}
		this.selectedSave.button = button;
		this.selectedSave.filename = filename;
		this.selectedSave.dlcIds = dlcIds;
		if (this.selectedSave.button != null)
		{
			this.selectedSave.button.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
		}
	}

	// Token: 0x060059E9 RID: 23017 RVA: 0x002095B0 File Offset: 0x002077B0
	private void Load()
	{
		if (!DlcManager.IsAllContentSubscribed(this.selectedSave.dlcIds))
		{
			string message = this.selectedSave.dlcIds.Contains("") ? UI.FRONTEND.LOADSCREEN.VANILLA_RESTART : UI.FRONTEND.LOADSCREEN.EXPANSION1_RESTART;
			this.ConfirmDoAction(message, delegate
			{
				KPlayerPrefs.SetString("AutoResumeSaveFile", this.selectedSave.filename);
				DlcManager.ToggleDLC("EXPANSION1_ID");
			});
			return;
		}
		LoadingOverlay.Load(new System.Action(this.DoLoad));
	}

	// Token: 0x060059EA RID: 23018 RVA: 0x0020961D File Offset: 0x0020781D
	private void DoLoad()
	{
		if (this.selectedSave == null)
		{
			return;
		}
		LoadScreen.DoLoad(this.selectedSave.filename);
		this.Deactivate();
	}

	// Token: 0x060059EB RID: 23019 RVA: 0x00209640 File Offset: 0x00207840
	public static void DoLoad(string filename)
	{
		KCrashReporter.MOST_RECENT_SAVEFILE = filename;
		bool flag = true;
		SaveGame.Header header;
		SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(filename, out header);
		string arg = null;
		string arg2 = null;
		if (header.buildVersion > 642695U)
		{
			arg = header.buildVersion.ToString();
			arg2 = 642695U.ToString();
		}
		else if (gameInfo.saveMajorVersion < 7)
		{
			arg = string.Format("v{0}.{1}", gameInfo.saveMajorVersion, gameInfo.saveMinorVersion);
			arg2 = string.Format("v{0}.{1}", 7, 35);
		}
		if (!flag)
		{
			GameObject parent = (FrontEndManager.Instance == null) ? GameScreenManager.Instance.ssOverlayCanvas : FrontEndManager.Instance.gameObject;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format(UI.CRASHSCREEN.LOADFAILED, "Version Mismatch", arg, arg2), null, null, null, null, null, null, null, null);
			return;
		}
		if (Game.Instance != null)
		{
			LoadScreen.ForceStopGame();
		}
		SaveLoader.SetActiveSaveFilePath(filename);
		Time.timeScale = 0f;
		App.LoadScene("backend");
	}

	// Token: 0x060059EC RID: 23020 RVA: 0x00209761 File Offset: 0x00207961
	private void MoreInfo()
	{
		App.OpenWebURL("http://support.kleientertainment.com/customer/portal/articles/2776550");
	}

	// Token: 0x060059ED RID: 23021 RVA: 0x00209770 File Offset: 0x00207970
	private void Delete(System.Action onDelete)
	{
		if (this.selectedSave == null || string.IsNullOrEmpty(this.selectedSave.filename))
		{
			global::Debug.LogError("The path provided is not valid and cannot be deleted.");
			return;
		}
		this.ConfirmDoAction(string.Format(UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, System.IO.Path.GetFileName(this.selectedSave.filename)), delegate
		{
			try
			{
				this.DeleteFileAndEmptyFolder(this.selectedSave.filename);
				string file = System.IO.Path.ChangeExtension(this.selectedSave.filename, "png");
				this.DeleteFileAndEmptyFolder(file);
				if (onDelete != null)
				{
					onDelete();
				}
				MainMenu.Instance.RefreshResumeButton(false);
			}
			catch (SystemException ex)
			{
				global::Debug.LogError(ex.ToString());
			}
		});
	}

	// Token: 0x060059EE RID: 23022 RVA: 0x002097E7 File Offset: 0x002079E7
	private void ShowSimpleDialog(string title, string message)
	{
		global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(title).AddPlainText(message).AddDefaultOK(false).Activate();
	}

	// Token: 0x060059EF RID: 23023 RVA: 0x0020981C File Offset: 0x00207A1C
	private void ConfirmCloudSaveMigrations(string message, string title, string confirmText, string backupText, System.Action commitAction, System.Action backupAction, Sprite sprite)
	{
		global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(title).AddSprite(sprite).AddPlainText(message).AddDefaultCancel().AddOption(confirmText, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
			commitAction();
		}, true).Activate();
	}

	// Token: 0x060059F0 RID: 23024 RVA: 0x00209884 File Offset: 0x00207A84
	private void ShowConvertError(string message)
	{
		if (this.errorInfoScreen == null)
		{
			if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
			{
				this.errorInfoScreen = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.CONVERT_ERROR_TITLE).AddSprite(this.errorSprite).AddPlainText(message).AddDefaultOK(false);
				this.errorInfoScreen.Activate();
				return;
			}
			this.errorInfoScreen = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.CONVERT_ERROR_TITLE).AddSprite(this.errorSprite).AddPlainText(message).AddOption(UI.FRONTEND.LOADSCREEN.MIGRATE_FAILURES_FORUM_BUTTON, delegate(InfoDialogScreen d)
			{
				App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
			}, false).AddDefaultOK(false);
			this.errorInfoScreen.Activate();
		}
	}

	// Token: 0x060059F1 RID: 23025 RVA: 0x00209984 File Offset: 0x00207B84
	private void ConfirmDoAction(string message, System.Action action)
	{
		if (this.confirmScreen == null)
		{
			this.confirmScreen = global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, false);
			this.confirmScreen.PopupConfirmDialog(message, action, delegate
			{
			}, null, null, null, null, null, null);
			this.confirmScreen.gameObject.SetActive(true);
		}
	}

	// Token: 0x060059F2 RID: 23026 RVA: 0x00209A03 File Offset: 0x00207C03
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.currentColony != null && e.TryConsume(global::Action.Escape))
		{
			this.ShowColonyList();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04003B02 RID: 15106
	private const int MAX_CLOUD_TUTORIALS = 5;

	// Token: 0x04003B03 RID: 15107
	private const string CLOUD_TUTORIAL_KEY = "LoadScreenCloudTutorialTimes";

	// Token: 0x04003B04 RID: 15108
	private const int ITEMS_PER_PAGE = 20;

	// Token: 0x04003B05 RID: 15109
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003B06 RID: 15110
	[SerializeField]
	private GameObject saveButtonRoot;

	// Token: 0x04003B07 RID: 15111
	[SerializeField]
	private GameObject colonyListRoot;

	// Token: 0x04003B08 RID: 15112
	[SerializeField]
	private GameObject colonyViewRoot;

	// Token: 0x04003B09 RID: 15113
	[SerializeField]
	private HierarchyReferences migrationPanelRefs;

	// Token: 0x04003B0A RID: 15114
	[SerializeField]
	private HierarchyReferences saveButtonPrefab;

	// Token: 0x04003B0B RID: 15115
	[SerializeField]
	private KButton loadMoreButton;

	// Token: 0x04003B0C RID: 15116
	[Space]
	[SerializeField]
	private KButton colonyCloudButton;

	// Token: 0x04003B0D RID: 15117
	[SerializeField]
	private KButton colonyLocalButton;

	// Token: 0x04003B0E RID: 15118
	[SerializeField]
	private KButton colonyInfoButton;

	// Token: 0x04003B0F RID: 15119
	[SerializeField]
	private Sprite localToCloudSprite;

	// Token: 0x04003B10 RID: 15120
	[SerializeField]
	private Sprite cloudToLocalSprite;

	// Token: 0x04003B11 RID: 15121
	[SerializeField]
	private Sprite errorSprite;

	// Token: 0x04003B12 RID: 15122
	[SerializeField]
	private Sprite infoSprite;

	// Token: 0x04003B13 RID: 15123
	[SerializeField]
	private Bouncer cloudTutorialBouncer;

	// Token: 0x04003B14 RID: 15124
	public bool requireConfirmation = true;

	// Token: 0x04003B15 RID: 15125
	private LoadScreen.SelectedSave selectedSave;

	// Token: 0x04003B16 RID: 15126
	private List<LoadScreen.SaveGameFileDetails> currentColony;

	// Token: 0x04003B17 RID: 15127
	private UIPool<HierarchyReferences> colonyListPool;

	// Token: 0x04003B18 RID: 15128
	private ConfirmDialogScreen confirmScreen;

	// Token: 0x04003B19 RID: 15129
	private InfoDialogScreen infoScreen;

	// Token: 0x04003B1A RID: 15130
	private InfoDialogScreen errorInfoScreen;

	// Token: 0x04003B1B RID: 15131
	private ConfirmDialogScreen errorScreen;

	// Token: 0x04003B1C RID: 15132
	private InspectSaveScreen inspectScreenInstance;

	// Token: 0x04003B1D RID: 15133
	private int displayedPageCount = 1;

	// Token: 0x02001C02 RID: 7170
	private struct SaveGameFileDetails
	{
		// Token: 0x04008183 RID: 33155
		public string BaseName;

		// Token: 0x04008184 RID: 33156
		public string FileName;

		// Token: 0x04008185 RID: 33157
		public string UniqueID;

		// Token: 0x04008186 RID: 33158
		public System.DateTime FileDate;

		// Token: 0x04008187 RID: 33159
		public SaveGame.Header FileHeader;

		// Token: 0x04008188 RID: 33160
		public SaveGame.GameInfo FileInfo;

		// Token: 0x04008189 RID: 33161
		public long Size;
	}

	// Token: 0x02001C03 RID: 7171
	private class SelectedSave
	{
		// Token: 0x0400818A RID: 33162
		public string filename;

		// Token: 0x0400818B RID: 33163
		public List<string> dlcIds;

		// Token: 0x0400818C RID: 33164
		public KButton button;
	}
}
