using System;
using System.Collections.Generic;
using Klei;

namespace KMod
{
	// Token: 0x02000EDE RID: 3806
	public interface IFileSource
	{
		// Token: 0x06007667 RID: 30311
		string GetRoot();

		// Token: 0x06007668 RID: 30312
		bool Exists();

		// Token: 0x06007669 RID: 30313
		bool Exists(string relative_path);

		// Token: 0x0600766A RID: 30314
		void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root = "");

		// Token: 0x0600766B RID: 30315
		IFileDirectory GetFileSystem();

		// Token: 0x0600766C RID: 30316
		void CopyTo(string path, List<string> extensions = null);

		// Token: 0x0600766D RID: 30317
		string Read(string relative_path);

		// Token: 0x0600766E RID: 30318
		void Dispose();
	}
}
