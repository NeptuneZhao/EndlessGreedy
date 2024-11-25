using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Klei;
using Newtonsoft.Json;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000EE4 RID: 3812
	[JsonObject(MemberSerialization.OptIn)]
	[DebuggerDisplay("{title}")]
	public class Mod
	{
		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06007688 RID: 30344 RVA: 0x002E8D2D File Offset: 0x002E6F2D
		// (set) Token: 0x06007689 RID: 30345 RVA: 0x002E8D35 File Offset: 0x002E6F35
		public Content available_content { get; private set; }

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x0600768A RID: 30346 RVA: 0x002E8D3E File Offset: 0x002E6F3E
		// (set) Token: 0x0600768B RID: 30347 RVA: 0x002E8D46 File Offset: 0x002E6F46
		[JsonProperty]
		public string staticID { get; private set; }

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x0600768C RID: 30348 RVA: 0x002E8D4F File Offset: 0x002E6F4F
		// (set) Token: 0x0600768D RID: 30349 RVA: 0x002E8D57 File Offset: 0x002E6F57
		public LocString manage_tooltip { get; private set; }

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x0600768E RID: 30350 RVA: 0x002E8D60 File Offset: 0x002E6F60
		// (set) Token: 0x0600768F RID: 30351 RVA: 0x002E8D68 File Offset: 0x002E6F68
		public System.Action on_managed { get; private set; }

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06007690 RID: 30352 RVA: 0x002E8D71 File Offset: 0x002E6F71
		public bool is_managed
		{
			get
			{
				return this.manage_tooltip != null;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06007691 RID: 30353 RVA: 0x002E8D7C File Offset: 0x002E6F7C
		public string title
		{
			get
			{
				return this.label.title;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06007692 RID: 30354 RVA: 0x002E8D89 File Offset: 0x002E6F89
		// (set) Token: 0x06007693 RID: 30355 RVA: 0x002E8D91 File Offset: 0x002E6F91
		public string description { get; private set; }

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06007694 RID: 30356 RVA: 0x002E8D9A File Offset: 0x002E6F9A
		// (set) Token: 0x06007695 RID: 30357 RVA: 0x002E8DA2 File Offset: 0x002E6FA2
		public Content loaded_content { get; private set; }

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06007696 RID: 30358 RVA: 0x002E8DAB File Offset: 0x002E6FAB
		// (set) Token: 0x06007697 RID: 30359 RVA: 0x002E8DB3 File Offset: 0x002E6FB3
		public IFileSource file_source
		{
			get
			{
				return this._fileSource;
			}
			set
			{
				if (this._fileSource != null)
				{
					this._fileSource.Dispose();
				}
				this._fileSource = value;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06007698 RID: 30360 RVA: 0x002E8DCF File Offset: 0x002E6FCF
		// (set) Token: 0x06007699 RID: 30361 RVA: 0x002E8DD7 File Offset: 0x002E6FD7
		public bool DevModCrashTriggered { get; private set; }

		// Token: 0x0600769A RID: 30362 RVA: 0x002E8DE0 File Offset: 0x002E6FE0
		[JsonConstructor]
		public Mod()
		{
		}

		// Token: 0x0600769B RID: 30363 RVA: 0x002E8DF4 File Offset: 0x002E6FF4
		public void CopyPersistentDataTo(Mod other_mod)
		{
			other_mod.status = this.status;
			other_mod.enabledForDlc = ((this.enabledForDlc != null) ? new List<string>(this.enabledForDlc) : new List<string>());
			other_mod.crash_count = this.crash_count;
			other_mod.loaded_content = this.loaded_content;
			other_mod.loaded_mod_data = this.loaded_mod_data;
			other_mod.reinstall_path = this.reinstall_path;
		}

		// Token: 0x0600769C RID: 30364 RVA: 0x002E8E60 File Offset: 0x002E7060
		public Mod(Label label, string staticID, string description, IFileSource file_source, LocString manage_tooltip, System.Action on_managed)
		{
			this.label = label;
			this.status = Mod.Status.NotInstalled;
			this.staticID = staticID;
			this.description = description;
			this.file_source = file_source;
			this.manage_tooltip = manage_tooltip;
			this.on_managed = on_managed;
			this.loaded_content = (Content)0;
			this.available_content = (Content)0;
			this.ScanContent();
		}

		// Token: 0x0600769D RID: 30365 RVA: 0x002E8EC6 File Offset: 0x002E70C6
		public bool IsEnabledForActiveDlc()
		{
			return this.IsEnabledForDlc(DlcManager.GetHighestActiveDlcId());
		}

		// Token: 0x0600769E RID: 30366 RVA: 0x002E8ED3 File Offset: 0x002E70D3
		public bool IsEnabledForDlc(string dlcId)
		{
			return this.enabledForDlc != null && this.enabledForDlc.Contains(dlcId);
		}

		// Token: 0x0600769F RID: 30367 RVA: 0x002E8EEB File Offset: 0x002E70EB
		public void SetEnabledForActiveDlc(bool enabled)
		{
			this.SetEnabledForDlc(DlcManager.GetHighestActiveDlcId(), enabled);
		}

		// Token: 0x060076A0 RID: 30368 RVA: 0x002E8EFC File Offset: 0x002E70FC
		public void SetEnabledForDlc(string dlcId, bool set_enabled)
		{
			if (this.enabledForDlc == null)
			{
				this.enabledForDlc = new List<string>();
			}
			bool flag = this.enabledForDlc.Contains(dlcId);
			if (set_enabled && !flag)
			{
				this.enabledForDlc.Add(dlcId);
				return;
			}
			if (!set_enabled && flag)
			{
				this.enabledForDlc.Remove(dlcId);
			}
		}

		// Token: 0x060076A1 RID: 30369 RVA: 0x002E8F54 File Offset: 0x002E7154
		public void ScanContent()
		{
			this.ModDevLog(string.Format("{0} ({1}): Setting up mod.", this.label, this.label.id));
			this.available_content = (Content)0;
			if (this.file_source == null)
			{
				if (this.label.id.EndsWith(".zip"))
				{
					DebugUtil.DevAssert(false, "Does this actually get used ever?", null);
					this.file_source = new ZipFile(this.label.install_path);
				}
				else
				{
					this.file_source = new Directory(this.label.install_path);
				}
			}
			if (!this.file_source.Exists())
			{
				global::Debug.LogWarning(string.Format("{0}: File source does not appear to be valid, skipping. ({1})", this.label, this.label.install_path));
				return;
			}
			KModHeader header = KModUtil.GetHeader(this.file_source, this.label.defaultStaticID, this.label.title, this.description, this.IsDev);
			if (this.label.title != header.title)
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"\t",
					this.label.title,
					" has a mod.yaml with the title `",
					header.title,
					"`, using that from now on."
				}));
			}
			if (this.label.defaultStaticID != header.staticID)
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"\t",
					this.label.title,
					" has a mod.yaml with a staticID `",
					header.staticID,
					"`, using that from now on."
				}));
			}
			this.label.title = header.title;
			this.staticID = header.staticID;
			this.description = header.description;
			Mod.ArchivedVersion mostSuitableArchive = this.GetMostSuitableArchive();
			if (mostSuitableArchive == null)
			{
				global::Debug.LogWarning(string.Format("{0}: No archive supports this game version, skipping content.", this.label));
				this.contentCompatability = ModContentCompatability.DoesntSupportDLCConfig;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			this.packagedModInfo = mostSuitableArchive.info;
			Content content;
			this.ScanContentFromSource(mostSuitableArchive.relativePath, out content);
			if (content == (Content)0)
			{
				global::Debug.LogWarning(string.Format("{0}: No supported content for mod, skipping content.", this.label));
				this.contentCompatability = ModContentCompatability.NoContent;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			bool flag = mostSuitableArchive.info.APIVersion == 2;
			if ((content & Content.DLL) != (Content)0 && !flag)
			{
				global::Debug.LogWarning(string.Format("{0}: DLLs found but not using the correct API version.", this.label));
				this.contentCompatability = ModContentCompatability.OldAPI;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			this.contentCompatability = ModContentCompatability.OK;
			this.available_content = content;
			this.relative_root = mostSuitableArchive.relativePath;
			global::Debug.Assert(this.content_source == null);
			this.content_source = new Directory(this.ContentPath);
			string arg = string.IsNullOrEmpty(this.relative_root) ? "root" : this.relative_root;
			global::Debug.Log(string.Format("{0}: Successfully loaded from path '{1}' with content '{2}'.", this.label, arg, this.available_content.ToString()));
		}

		// Token: 0x060076A2 RID: 30370 RVA: 0x002E9284 File Offset: 0x002E7484
		private Mod.ArchivedVersion GetMostSuitableArchive()
		{
			Mod.PackagedModInfo packagedModInfo = this.GetModInfoForFolder("");
			if (packagedModInfo == null)
			{
				packagedModInfo = new Mod.PackagedModInfo
				{
					supportedContent = "vanilla_id",
					minimumSupportedBuild = 0
				};
				if (this.ScanContentFromSourceForTranslationsOnly(""))
				{
					this.ModDevLogWarning(string.Format("{0}: No mod_info.yaml found, but since it contains a translation, default its supported content to 'ALL'", this.label));
					packagedModInfo.supportedContent = "all";
				}
				else
				{
					this.ModDevLogWarning(string.Format("{0}: No mod_info.yaml found, default its supported content to 'VANILLA_ID'", this.label));
				}
			}
			Mod.ArchivedVersion archivedVersion = new Mod.ArchivedVersion
			{
				relativePath = "",
				info = packagedModInfo
			};
			if (!this.file_source.Exists("archived_versions"))
			{
				this.ModDevLog(string.Format("\t{0}: No archived_versions for this mod, using root version directly.", this.label));
				if (!this.DoesModSupportCurrentContent(packagedModInfo))
				{
					return null;
				}
				return archivedVersion;
			}
			else
			{
				List<FileSystemItem> list = new List<FileSystemItem>();
				this.file_source.GetTopLevelItems(list, "archived_versions");
				if (list.Count == 0)
				{
					this.ModDevLog(string.Format("\t{0}: No archived_versions for this mod, using root version directly.", this.label));
					if (!this.DoesModSupportCurrentContent(packagedModInfo))
					{
						return null;
					}
					return archivedVersion;
				}
				else
				{
					List<Mod.ArchivedVersion> list2 = new List<Mod.ArchivedVersion>();
					list2.Add(archivedVersion);
					foreach (FileSystemItem fileSystemItem in list)
					{
						if (fileSystemItem.type != FileSystemItem.ItemType.File)
						{
							string relativePath = Path.Combine("archived_versions", fileSystemItem.name);
							Mod.PackagedModInfo modInfoForFolder = this.GetModInfoForFolder(relativePath);
							if (modInfoForFolder != null)
							{
								list2.Add(new Mod.ArchivedVersion
								{
									relativePath = relativePath,
									info = modInfoForFolder
								});
							}
						}
					}
					list2 = (from v in list2
					where this.DoesModSupportCurrentContent(v.info)
					select v).ToList<Mod.ArchivedVersion>();
					list2 = (from v in list2
					where v.info.APIVersion == 2 || v.info.APIVersion == 0
					select v).ToList<Mod.ArchivedVersion>();
					Mod.ArchivedVersion archivedVersion2 = (from v in list2
					where (long)v.info.minimumSupportedBuild <= 642695L
					orderby v.info.minimumSupportedBuild descending
					select v).FirstOrDefault<Mod.ArchivedVersion>();
					if (archivedVersion2 == null)
					{
						return null;
					}
					return archivedVersion2;
				}
			}
		}

		// Token: 0x060076A3 RID: 30371 RVA: 0x002E94CC File Offset: 0x002E76CC
		private Mod.PackagedModInfo GetModInfoForFolder(string relative_root)
		{
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relative_root);
			bool flag = false;
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.File && fileSystemItem.name.ToLower() == "mod_info.yaml")
				{
					flag = true;
					break;
				}
			}
			string text = string.IsNullOrEmpty(relative_root) ? "root" : relative_root;
			if (!flag)
			{
				this.ModDevLogWarning(string.Concat(new string[]
				{
					"\t",
					this.title,
					": has no mod_info.yaml in folder '",
					text,
					"'"
				}));
				return null;
			}
			string text2 = this.file_source.Read(Path.Combine(relative_root, "mod_info.yaml"));
			if (string.IsNullOrEmpty(text2))
			{
				this.ModDevLogError(string.Format("\t{0}: Failed to read {1} in folder '{2}', skipping", this.label, "mod_info.yaml", text));
				return null;
			}
			YamlIO.ErrorHandler handle_error = delegate(YamlIO.Error e, bool force_warning)
			{
				YamlIO.LogError(e, !this.IsDev);
			};
			Mod.PackagedModInfo packagedModInfo = YamlIO.Parse<Mod.PackagedModInfo>(text2, default(FileHandle), handle_error, null);
			if (packagedModInfo == null)
			{
				this.ModDevLogError(string.Format("\t{0}: Failed to parse {1} in folder '{2}', text is {3}", new object[]
				{
					this.label,
					"mod_info.yaml",
					text,
					text2
				}));
				return null;
			}
			if (packagedModInfo.supportedContent == null)
			{
				this.ModDevLogError(string.Format("\t{0}: {1} in folder '{2}' does not specify supportedContent. Make sure you spelled it correctly in your mod_info!", this.label, "mod_info.yaml", text));
				return null;
			}
			if (packagedModInfo.lastWorkingBuild != 0)
			{
				this.ModDevLogError(string.Format("\t{0}: {1} in folder '{2}' is using `{3}`, please upgrade this to `{4}`", new object[]
				{
					this.label,
					"mod_info.yaml",
					text,
					"lastWorkingBuild",
					"minimumSupportedBuild"
				}));
				if (packagedModInfo.minimumSupportedBuild == 0)
				{
					packagedModInfo.minimumSupportedBuild = packagedModInfo.lastWorkingBuild;
				}
			}
			this.ModDevLog(string.Format("\t{0}: Found valid mod_info.yaml in folder '{1}': {2} at {3}", new object[]
			{
				this.label,
				text,
				packagedModInfo.supportedContent,
				packagedModInfo.minimumSupportedBuild
			}));
			return packagedModInfo;
		}

		// Token: 0x060076A4 RID: 30372 RVA: 0x002E970C File Offset: 0x002E790C
		private bool DoesModSupportCurrentContent(Mod.PackagedModInfo mod_info)
		{
			string text = DlcManager.GetHighestActiveDlcId();
			if (text == "")
			{
				text = "vanilla_id";
			}
			text = text.ToLower();
			string text2 = mod_info.supportedContent.ToLower();
			return text2.Contains(text) || text2.Contains("all");
		}

		// Token: 0x060076A5 RID: 30373 RVA: 0x002E975C File Offset: 0x002E795C
		private bool ScanContentFromSourceForTranslationsOnly(string relativeRoot)
		{
			this.available_content = (Content)0;
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relativeRoot);
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.File && fileSystemItem.name.ToLower().EndsWith(".po"))
				{
					this.available_content |= Content.Translation;
				}
			}
			return this.available_content > (Content)0;
		}

		// Token: 0x060076A6 RID: 30374 RVA: 0x002E97F4 File Offset: 0x002E79F4
		private bool ScanContentFromSource(string relativeRoot, out Content available)
		{
			available = (Content)0;
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relativeRoot);
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.Directory)
				{
					string directory = fileSystemItem.name.ToLower();
					available |= this.AddDirectory(directory);
				}
				else
				{
					string file = fileSystemItem.name.ToLower();
					available |= this.AddFile(file);
				}
			}
			return available > (Content)0;
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x060076A7 RID: 30375 RVA: 0x002E9894 File Offset: 0x002E7A94
		public string ContentPath
		{
			get
			{
				return Path.Combine(this.label.install_path, this.relative_root);
			}
		}

		// Token: 0x060076A8 RID: 30376 RVA: 0x002E98AC File Offset: 0x002E7AAC
		public bool IsEmpty()
		{
			return this.available_content == (Content)0;
		}

		// Token: 0x060076A9 RID: 30377 RVA: 0x002E98B8 File Offset: 0x002E7AB8
		private Content AddDirectory(string directory)
		{
			Content content = (Content)0;
			string text = directory.TrimEnd('/');
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1519694028U)
			{
				if (num != 948591336U)
				{
					if (num != 1318520008U)
					{
						if (num == 1519694028U)
						{
							if (text == "elements")
							{
								content |= Content.LayerableFiles;
							}
						}
					}
					else if (text == "buildingfacades")
					{
						content |= Content.Animation;
					}
				}
				else if (text == "templates")
				{
					content |= Content.LayerableFiles;
				}
			}
			else if (num <= 3037049615U)
			{
				if (num != 2960291089U)
				{
					if (num == 3037049615U)
					{
						if (text == "worldgen")
						{
							content |= Content.LayerableFiles;
						}
					}
				}
				else if (text == "strings")
				{
					content |= Content.Strings;
				}
			}
			else if (num != 3319670096U)
			{
				if (num == 3570262116U)
				{
					if (text == "codex")
					{
						content |= Content.LayerableFiles;
					}
				}
			}
			else if (text == "anim")
			{
				content |= Content.Animation;
			}
			return content;
		}

		// Token: 0x060076AA RID: 30378 RVA: 0x002E99C8 File Offset: 0x002E7BC8
		private Content AddFile(string file)
		{
			Content content = (Content)0;
			if (file.EndsWith(".dll"))
			{
				content |= Content.DLL;
			}
			if (file.EndsWith(".po"))
			{
				content |= Content.Translation;
			}
			return content;
		}

		// Token: 0x060076AB RID: 30379 RVA: 0x002E99FA File Offset: 0x002E7BFA
		private static void AccumulateExtensions(Content content, List<string> extensions)
		{
			if ((content & Content.DLL) != (Content)0)
			{
				extensions.Add(".dll");
			}
			if ((content & (Content.Strings | Content.Translation)) != (Content)0)
			{
				extensions.Add(".po");
			}
		}

		// Token: 0x060076AC RID: 30380 RVA: 0x002E9A20 File Offset: 0x002E7C20
		[Conditional("DEBUG")]
		private void Assert(bool condition, string failure_message)
		{
			if (string.IsNullOrEmpty(this.title))
			{
				DebugUtil.Assert(condition, string.Format("{2}\n\t{0}\n\t{1}", this.title, this.label.ToString(), failure_message));
				return;
			}
			DebugUtil.Assert(condition, string.Format("{1}\n\t{0}", this.label.ToString(), failure_message));
		}

		// Token: 0x060076AD RID: 30381 RVA: 0x002E9A88 File Offset: 0x002E7C88
		public void Install()
		{
			if (this.IsLocal)
			{
				this.status = Mod.Status.Installed;
				return;
			}
			this.status = Mod.Status.ReinstallPending;
			if (this.file_source == null)
			{
				return;
			}
			if (!FileUtil.DeleteDirectory(this.label.install_path, 0))
			{
				return;
			}
			if (!FileUtil.CreateDirectory(this.label.install_path, 0))
			{
				return;
			}
			this.file_source.CopyTo(this.label.install_path, null);
			this.file_source = new Directory(this.label.install_path);
			this.status = Mod.Status.Installed;
		}

		// Token: 0x060076AE RID: 30382 RVA: 0x002E9B18 File Offset: 0x002E7D18
		public bool Uninstall()
		{
			this.SetEnabledForActiveDlc(false);
			if (this.loaded_content != (Content)0)
			{
				global::Debug.Log(string.Format("Can't uninstall {0}: still has loaded content: {1}", this.label.ToString(), this.loaded_content.ToString()));
				this.status = Mod.Status.UninstallPending;
				return false;
			}
			if (!this.IsLocal && !FileUtil.DeleteDirectory(this.label.install_path, 0))
			{
				global::Debug.Log(string.Format("Can't uninstall {0}: directory deletion failed", this.label.ToString()));
				this.status = Mod.Status.UninstallPending;
				return false;
			}
			this.status = Mod.Status.NotInstalled;
			return true;
		}

		// Token: 0x060076AF RID: 30383 RVA: 0x002E9BC0 File Offset: 0x002E7DC0
		private bool LoadStrings()
		{
			string path = FileSystem.Normalize(Path.Combine(this.ContentPath, "strings"));
			if (!Directory.Exists(path))
			{
				return false;
			}
			int num = 0;
			foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
			{
				if (!(fileInfo.Extension.ToLower() != ".po"))
				{
					num++;
					Localization.OverloadStrings(Localization.LoadStringsFile(fileInfo.FullName, false));
				}
			}
			return true;
		}

		// Token: 0x060076B0 RID: 30384 RVA: 0x002E9C3D File Offset: 0x002E7E3D
		private bool LoadTranslations()
		{
			return false;
		}

		// Token: 0x060076B1 RID: 30385 RVA: 0x002E9C40 File Offset: 0x002E7E40
		private bool LoadAnimation()
		{
			string path = FileSystem.Normalize(Path.Combine(this.ContentPath, "anim"));
			if (!Directory.Exists(path))
			{
				return false;
			}
			int num = 0;
			DirectoryInfo[] directories = new DirectoryInfo(path).GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				foreach (DirectoryInfo directoryInfo in directories[i].GetDirectories())
				{
					KAnimFile.Mod mod = new KAnimFile.Mod();
					foreach (FileInfo fileInfo in directoryInfo.GetFiles())
					{
						if (fileInfo.Extension == ".png")
						{
							byte[] data = File.ReadAllBytes(fileInfo.FullName);
							Texture2D texture2D = new Texture2D(2, 2);
							texture2D.LoadImage(data);
							mod.textures.Add(texture2D);
						}
						else if (fileInfo.Extension == ".bytes")
						{
							string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
							byte[] array = File.ReadAllBytes(fileInfo.FullName);
							if (fileNameWithoutExtension.EndsWith("_anim"))
							{
								mod.anim = array;
							}
							else if (fileNameWithoutExtension.EndsWith("_build"))
							{
								mod.build = array;
							}
							else
							{
								DebugUtil.LogWarningArgs(new object[]
								{
									string.Format("Unhandled TextAsset ({0})...ignoring", fileInfo.FullName)
								});
							}
						}
						else
						{
							DebugUtil.LogWarningArgs(new object[]
							{
								string.Format("Unhandled asset ({0})...ignoring", fileInfo.FullName)
							});
						}
					}
					string name = directoryInfo.Name + "_kanim";
					if (mod.IsValid() && ModUtil.AddKAnimMod(name, mod))
					{
						num++;
					}
				}
			}
			return true;
		}

		// Token: 0x060076B2 RID: 30386 RVA: 0x002E9E04 File Offset: 0x002E8004
		public void Load(Content content)
		{
			content &= (this.available_content & ~this.loaded_content);
			if (content > (Content)0)
			{
				global::Debug.Log(string.Format("Loading mod content {2} [{0}:{1}] (provides {3})", new object[]
				{
					this.title,
					this.label.id,
					content.ToString(),
					this.available_content.ToString()
				}));
			}
			if ((content & Content.Strings) != (Content)0 && this.LoadStrings())
			{
				this.loaded_content |= Content.Strings;
			}
			if ((content & Content.Translation) != (Content)0 && this.LoadTranslations())
			{
				this.loaded_content |= Content.Translation;
			}
			if ((content & Content.DLL) != (Content)0)
			{
				this.loaded_mod_data = DLLLoader.LoadDLLs(this, this.staticID, this.ContentPath, this.IsDev);
				if (this.loaded_mod_data != null)
				{
					this.loaded_content |= Content.DLL;
				}
			}
			if ((content & Content.LayerableFiles) != (Content)0)
			{
				global::Debug.Assert(this.content_source != null, "Attempting to Load layerable files with content_source not initialized");
				FileSystem.file_sources.Insert(0, this.content_source.GetFileSystem());
				this.loaded_content |= Content.LayerableFiles;
			}
			if ((content & Content.Animation) != (Content)0 && this.LoadAnimation())
			{
				this.loaded_content |= Content.Animation;
			}
		}

		// Token: 0x060076B3 RID: 30387 RVA: 0x002E9F43 File Offset: 0x002E8143
		public void PostLoad(IReadOnlyList<Mod> mods)
		{
			if ((this.loaded_content & Content.DLL) != (Content)0 && this.loaded_mod_data != null)
			{
				DLLLoader.PostLoadDLLs(this.staticID, this.loaded_mod_data, mods);
			}
		}

		// Token: 0x060076B4 RID: 30388 RVA: 0x002E9F69 File Offset: 0x002E8169
		public void Unload(Content content)
		{
			content &= this.loaded_content;
			if ((content & Content.LayerableFiles) != (Content)0)
			{
				FileSystem.file_sources.Remove(this.content_source.GetFileSystem());
				this.loaded_content &= ~Content.LayerableFiles;
			}
		}

		// Token: 0x060076B5 RID: 30389 RVA: 0x002E9FA2 File Offset: 0x002E81A2
		private void SetCrashCount(int new_crash_count)
		{
			this.crash_count = MathUtil.Clamp(0, 3, new_crash_count);
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x060076B6 RID: 30390 RVA: 0x002E9FB2 File Offset: 0x002E81B2
		public bool IsDev
		{
			get
			{
				return this.label.distribution_platform == Label.DistributionPlatform.Dev;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x060076B7 RID: 30391 RVA: 0x002E9FC2 File Offset: 0x002E81C2
		public bool IsLocal
		{
			get
			{
				return this.label.distribution_platform == Label.DistributionPlatform.Dev || this.label.distribution_platform == Label.DistributionPlatform.Local;
			}
		}

		// Token: 0x060076B8 RID: 30392 RVA: 0x002E9FE2 File Offset: 0x002E81E2
		public void SetCrashed()
		{
			this.SetCrashCount(this.crash_count + 1);
			if (!this.IsDev)
			{
				this.SetEnabledForActiveDlc(false);
			}
		}

		// Token: 0x060076B9 RID: 30393 RVA: 0x002EA001 File Offset: 0x002E8201
		public void Uncrash()
		{
			this.SetCrashCount(this.IsDev ? (this.crash_count - 1) : 0);
		}

		// Token: 0x060076BA RID: 30394 RVA: 0x002EA01C File Offset: 0x002E821C
		public bool IsActive()
		{
			return this.loaded_content > (Content)0;
		}

		// Token: 0x060076BB RID: 30395 RVA: 0x002EA027 File Offset: 0x002E8227
		public bool AllActive(Content content)
		{
			return (this.loaded_content & content) == content;
		}

		// Token: 0x060076BC RID: 30396 RVA: 0x002EA034 File Offset: 0x002E8234
		public bool AllActive()
		{
			return (this.loaded_content & this.available_content) == this.available_content;
		}

		// Token: 0x060076BD RID: 30397 RVA: 0x002EA04B File Offset: 0x002E824B
		public bool AnyActive(Content content)
		{
			return (this.loaded_content & content) > (Content)0;
		}

		// Token: 0x060076BE RID: 30398 RVA: 0x002EA058 File Offset: 0x002E8258
		public bool HasContent()
		{
			return this.available_content > (Content)0;
		}

		// Token: 0x060076BF RID: 30399 RVA: 0x002EA063 File Offset: 0x002E8263
		public bool HasAnyContent(Content content)
		{
			return (this.available_content & content) > (Content)0;
		}

		// Token: 0x060076C0 RID: 30400 RVA: 0x002EA070 File Offset: 0x002E8270
		public bool HasOnlyTranslationContent()
		{
			return this.available_content == Content.Translation;
		}

		// Token: 0x060076C1 RID: 30401 RVA: 0x002EA07C File Offset: 0x002E827C
		public Texture2D GetPreviewImage()
		{
			string text = null;
			foreach (string text2 in Mod.PREVIEW_FILENAMES)
			{
				if (Directory.Exists(this.ContentPath) && File.Exists(Path.Combine(this.ContentPath, text2)))
				{
					text = text2;
					break;
				}
			}
			if (text == null)
			{
				return null;
			}
			Texture2D result;
			try
			{
				byte[] data = File.ReadAllBytes(Path.Combine(this.ContentPath, text));
				Texture2D texture2D = new Texture2D(2, 2);
				texture2D.LoadImage(data);
				result = texture2D;
			}
			catch
			{
				global::Debug.LogWarning(string.Format("Mod {0} seems to have a preview.png but it didn't load correctly.", this.label));
				result = null;
			}
			return result;
		}

		// Token: 0x060076C2 RID: 30402 RVA: 0x002EA148 File Offset: 0x002E8348
		public void ModDevLog(string msg)
		{
			if (this.IsDev)
			{
				global::Debug.Log(msg);
			}
		}

		// Token: 0x060076C3 RID: 30403 RVA: 0x002EA158 File Offset: 0x002E8358
		public void ModDevLogWarning(string msg)
		{
			if (this.IsDev)
			{
				global::Debug.LogWarning(msg);
			}
		}

		// Token: 0x060076C4 RID: 30404 RVA: 0x002EA168 File Offset: 0x002E8368
		public void ModDevLogError(string msg)
		{
			if (this.IsDev)
			{
				this.DevModCrashTriggered = true;
				global::Debug.LogError(msg);
			}
		}

		// Token: 0x04005685 RID: 22149
		public const int MOD_API_VERSION_NONE = 0;

		// Token: 0x04005686 RID: 22150
		public const int MOD_API_VERSION_HARMONY1 = 1;

		// Token: 0x04005687 RID: 22151
		public const int MOD_API_VERSION_HARMONY2 = 2;

		// Token: 0x04005688 RID: 22152
		public const int MOD_API_VERSION = 2;

		// Token: 0x04005689 RID: 22153
		[JsonProperty]
		public Label label;

		// Token: 0x0400568A RID: 22154
		[JsonProperty]
		public Mod.Status status;

		// Token: 0x0400568B RID: 22155
		[JsonProperty]
		public bool enabled;

		// Token: 0x0400568C RID: 22156
		[JsonProperty]
		public List<string> enabledForDlc;

		// Token: 0x0400568E RID: 22158
		[JsonProperty]
		public int crash_count;

		// Token: 0x0400568F RID: 22159
		[JsonProperty]
		public string reinstall_path;

		// Token: 0x04005691 RID: 22161
		public bool foundInStackTrace;

		// Token: 0x04005692 RID: 22162
		public string relative_root = "";

		// Token: 0x04005693 RID: 22163
		public Mod.PackagedModInfo packagedModInfo;

		// Token: 0x04005698 RID: 22168
		public LoadedModData loaded_mod_data;

		// Token: 0x04005699 RID: 22169
		private IFileSource _fileSource;

		// Token: 0x0400569A RID: 22170
		public IFileSource content_source;

		// Token: 0x0400569B RID: 22171
		public bool is_subscribed;

		// Token: 0x0400569D RID: 22173
		private const string VANILLA_ID = "vanilla_id";

		// Token: 0x0400569E RID: 22174
		private const string ALL_ID = "all";

		// Token: 0x0400569F RID: 22175
		private const string ARCHIVED_VERSIONS_FOLDER = "archived_versions";

		// Token: 0x040056A0 RID: 22176
		private const string MOD_INFO_FILENAME = "mod_info.yaml";

		// Token: 0x040056A1 RID: 22177
		public ModContentCompatability contentCompatability;

		// Token: 0x040056A2 RID: 22178
		public const int MAX_CRASH_COUNT = 3;

		// Token: 0x040056A3 RID: 22179
		private static readonly List<string> PREVIEW_FILENAMES = new List<string>
		{
			"preview.png",
			"Preview.png",
			"PREVIEW.PNG"
		};

		// Token: 0x02001F8D RID: 8077
		public enum Status
		{
			// Token: 0x04008EF4 RID: 36596
			NotInstalled,
			// Token: 0x04008EF5 RID: 36597
			Installed,
			// Token: 0x04008EF6 RID: 36598
			UninstallPending,
			// Token: 0x04008EF7 RID: 36599
			ReinstallPending
		}

		// Token: 0x02001F8E RID: 8078
		public class ArchivedVersion
		{
			// Token: 0x04008EF8 RID: 36600
			public string relativePath;

			// Token: 0x04008EF9 RID: 36601
			public Mod.PackagedModInfo info;
		}

		// Token: 0x02001F8F RID: 8079
		public class PackagedModInfo
		{
			// Token: 0x17000BF9 RID: 3065
			// (get) Token: 0x0600AF68 RID: 44904 RVA: 0x003B12A2 File Offset: 0x003AF4A2
			// (set) Token: 0x0600AF69 RID: 44905 RVA: 0x003B12AA File Offset: 0x003AF4AA
			public string supportedContent { get; set; }

			// Token: 0x17000BFA RID: 3066
			// (get) Token: 0x0600AF6A RID: 44906 RVA: 0x003B12B3 File Offset: 0x003AF4B3
			// (set) Token: 0x0600AF6B RID: 44907 RVA: 0x003B12BB File Offset: 0x003AF4BB
			[Obsolete("Use minimumSupportedBuild instead!")]
			public int lastWorkingBuild { get; set; }

			// Token: 0x17000BFB RID: 3067
			// (get) Token: 0x0600AF6C RID: 44908 RVA: 0x003B12C4 File Offset: 0x003AF4C4
			// (set) Token: 0x0600AF6D RID: 44909 RVA: 0x003B12CC File Offset: 0x003AF4CC
			public int minimumSupportedBuild { get; set; }

			// Token: 0x17000BFC RID: 3068
			// (get) Token: 0x0600AF6E RID: 44910 RVA: 0x003B12D5 File Offset: 0x003AF4D5
			// (set) Token: 0x0600AF6F RID: 44911 RVA: 0x003B12DD File Offset: 0x003AF4DD
			public int APIVersion { get; set; }

			// Token: 0x17000BFD RID: 3069
			// (get) Token: 0x0600AF70 RID: 44912 RVA: 0x003B12E6 File Offset: 0x003AF4E6
			// (set) Token: 0x0600AF71 RID: 44913 RVA: 0x003B12EE File Offset: 0x003AF4EE
			public string version { get; set; }
		}
	}
}
