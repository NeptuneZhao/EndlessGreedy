using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Klei;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000EDF RID: 3807
	internal struct Directory : IFileSource
	{
		// Token: 0x0600766F RID: 30319 RVA: 0x002E8590 File Offset: 0x002E6790
		public Directory(string root)
		{
			this.root = root;
			this.file_system = new AliasDirectory(root, root, Application.streamingAssetsPath, true);
		}

		// Token: 0x06007670 RID: 30320 RVA: 0x002E85AC File Offset: 0x002E67AC
		public string GetRoot()
		{
			return this.root;
		}

		// Token: 0x06007671 RID: 30321 RVA: 0x002E85B4 File Offset: 0x002E67B4
		public bool Exists()
		{
			return Directory.Exists(this.GetRoot());
		}

		// Token: 0x06007672 RID: 30322 RVA: 0x002E85C1 File Offset: 0x002E67C1
		public bool Exists(string relative_path)
		{
			return this.Exists() && new DirectoryInfo(FileSystem.Normalize(Path.Combine(this.root, relative_path))).Exists;
		}

		// Token: 0x06007673 RID: 30323 RVA: 0x002E85E8 File Offset: 0x002E67E8
		public void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root)
		{
			relative_root = (relative_root ?? "");
			string text = FileSystem.Normalize(Path.Combine(this.root, relative_root));
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				global::Debug.LogError("Cannot iterate over $" + text + ", this directory does not exist");
				return;
			}
			foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
			{
				file_system_items.Add(new FileSystemItem
				{
					name = fileSystemInfo.Name,
					type = ((fileSystemInfo is DirectoryInfo) ? FileSystemItem.ItemType.Directory : FileSystemItem.ItemType.File)
				});
			}
		}

		// Token: 0x06007674 RID: 30324 RVA: 0x002E8684 File Offset: 0x002E6884
		public IFileDirectory GetFileSystem()
		{
			return this.file_system;
		}

		// Token: 0x06007675 RID: 30325 RVA: 0x002E868C File Offset: 0x002E688C
		public void CopyTo(string path, List<string> extensions = null)
		{
			try
			{
				Directory.CopyDirectory(this.root, path, extensions);
			}
			catch (UnauthorizedAccessException)
			{
				FileUtil.ErrorDialog(FileUtil.ErrorType.UnauthorizedAccess, path, null, null);
			}
			catch (IOException)
			{
				FileUtil.ErrorDialog(FileUtil.ErrorType.IOError, path, null, null);
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x06007676 RID: 30326 RVA: 0x002E86EC File Offset: 0x002E68EC
		public string Read(string relative_path)
		{
			string result;
			try
			{
				using (FileStream fileStream = File.OpenRead(Path.Combine(this.root, relative_path)))
				{
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, (int)fileStream.Length);
					result = Encoding.UTF8.GetString(array);
				}
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06007677 RID: 30327 RVA: 0x002E8768 File Offset: 0x002E6968
		private static int CopyDirectory(string sourceDirName, string destDirName, List<string> extensions)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			if (!directoryInfo.Exists)
			{
				return 0;
			}
			if (!FileUtil.CreateDirectory(destDirName, 0))
			{
				return 0;
			}
			FileInfo[] files = directoryInfo.GetFiles();
			int num = 0;
			foreach (FileInfo fileInfo in files)
			{
				bool flag = extensions == null || extensions.Count == 0;
				if (extensions != null)
				{
					using (List<string>.Enumerator enumerator = extensions.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == Path.GetExtension(fileInfo.Name).ToLower())
							{
								flag = true;
								break;
							}
						}
					}
				}
				if (flag)
				{
					string destFileName = Path.Combine(destDirName, fileInfo.Name);
					fileInfo.CopyTo(destFileName, false);
					num++;
				}
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
				num += Directory.CopyDirectory(directoryInfo2.FullName, destDirName2, extensions);
			}
			if (num == 0)
			{
				FileUtil.DeleteDirectory(destDirName, 0);
			}
			return num;
		}

		// Token: 0x06007678 RID: 30328 RVA: 0x002E888C File Offset: 0x002E6A8C
		public void Dispose()
		{
		}

		// Token: 0x04005671 RID: 22129
		private AliasDirectory file_system;

		// Token: 0x04005672 RID: 22130
		private string root;
	}
}
