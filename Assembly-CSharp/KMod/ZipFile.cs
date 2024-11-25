using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Klei;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000EE0 RID: 3808
	internal struct ZipFile : IFileSource
	{
		// Token: 0x06007679 RID: 30329 RVA: 0x002E888E File Offset: 0x002E6A8E
		public ZipFile(string filename)
		{
			this.filename = filename;
			this.zipfile = ZipFile.Read(filename);
			this.file_system = new ZipFileDirectory(this.zipfile.Name, this.zipfile, Application.streamingAssetsPath, true);
		}

		// Token: 0x0600767A RID: 30330 RVA: 0x002E88C5 File Offset: 0x002E6AC5
		public string GetRoot()
		{
			return this.filename;
		}

		// Token: 0x0600767B RID: 30331 RVA: 0x002E88CD File Offset: 0x002E6ACD
		public bool Exists()
		{
			return File.Exists(this.GetRoot());
		}

		// Token: 0x0600767C RID: 30332 RVA: 0x002E88DC File Offset: 0x002E6ADC
		public bool Exists(string relative_path)
		{
			if (!this.Exists())
			{
				return false;
			}
			using (IEnumerator<ZipEntry> enumerator = this.zipfile.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (FileSystem.Normalize(enumerator.Current.FileName).StartsWith(relative_path))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600767D RID: 30333 RVA: 0x002E8944 File Offset: 0x002E6B44
		public void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root)
		{
			HashSetPool<string, ZipFile>.PooledHashSet pooledHashSet = HashSetPool<string, ZipFile>.Allocate();
			string[] array;
			if (!string.IsNullOrEmpty(relative_root))
			{
				relative_root = (relative_root ?? "");
				relative_root = FileSystem.Normalize(relative_root);
				array = relative_root.Split('/', StringSplitOptions.None);
			}
			else
			{
				array = new string[0];
			}
			foreach (ZipEntry zipEntry in this.zipfile)
			{
				List<string> list = (from part in FileSystem.Normalize(zipEntry.FileName).Split('/', StringSplitOptions.None)
				where !string.IsNullOrEmpty(part)
				select part).ToList<string>();
				if (this.IsSharedRoot(array, list))
				{
					list = list.GetRange(array.Length, list.Count - array.Length);
					if (list.Count != 0)
					{
						string text = list[0];
						if (pooledHashSet.Add(text))
						{
							file_system_items.Add(new FileSystemItem
							{
								name = text,
								type = ((1 < list.Count) ? FileSystemItem.ItemType.Directory : FileSystemItem.ItemType.File)
							});
						}
					}
				}
			}
			pooledHashSet.Recycle();
		}

		// Token: 0x0600767E RID: 30334 RVA: 0x002E8A6C File Offset: 0x002E6C6C
		private bool IsSharedRoot(string[] root_path, List<string> check_path)
		{
			for (int i = 0; i < root_path.Length; i++)
			{
				if (i >= check_path.Count || root_path[i] != check_path[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600767F RID: 30335 RVA: 0x002E8AA4 File Offset: 0x002E6CA4
		public IFileDirectory GetFileSystem()
		{
			return this.file_system;
		}

		// Token: 0x06007680 RID: 30336 RVA: 0x002E8AAC File Offset: 0x002E6CAC
		public void CopyTo(string path, List<string> extensions = null)
		{
			foreach (ZipEntry zipEntry in this.zipfile.Entries)
			{
				bool flag = extensions == null || extensions.Count == 0;
				if (extensions != null)
				{
					foreach (string value in extensions)
					{
						if (zipEntry.FileName.ToLower().EndsWith(value))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					string path2 = FileSystem.Normalize(Path.Combine(path, zipEntry.FileName));
					string directoryName = Path.GetDirectoryName(path2);
					if (string.IsNullOrEmpty(directoryName) || FileUtil.CreateDirectory(directoryName, 0))
					{
						using (MemoryStream memoryStream = new MemoryStream((int)zipEntry.UncompressedSize))
						{
							zipEntry.Extract(memoryStream);
							using (FileStream fileStream = FileUtil.Create(path2, 0))
							{
								fileStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
							}
						}
					}
				}
			}
		}

		// Token: 0x06007681 RID: 30337 RVA: 0x002E8BFC File Offset: 0x002E6DFC
		public string Read(string relative_path)
		{
			ICollection<ZipEntry> collection = this.zipfile.SelectEntries(relative_path);
			if (collection.Count == 0)
			{
				return string.Empty;
			}
			foreach (ZipEntry zipEntry in collection)
			{
				using (MemoryStream memoryStream = new MemoryStream((int)zipEntry.UncompressedSize))
				{
					zipEntry.Extract(memoryStream);
					return Encoding.UTF8.GetString(memoryStream.GetBuffer());
				}
			}
			return string.Empty;
		}

		// Token: 0x06007682 RID: 30338 RVA: 0x002E8CA0 File Offset: 0x002E6EA0
		public void Dispose()
		{
			this.zipfile.Dispose();
		}

		// Token: 0x04005673 RID: 22131
		private string filename;

		// Token: 0x04005674 RID: 22132
		private ZipFile zipfile;

		// Token: 0x04005675 RID: 22133
		private ZipFileDirectory file_system;
	}
}
