using System;

namespace KMod
{
	// Token: 0x02000EDD RID: 3805
	public struct FileSystemItem
	{
		// Token: 0x0400566F RID: 22127
		public string name;

		// Token: 0x04005670 RID: 22128
		public FileSystemItem.ItemType type;

		// Token: 0x02001F8A RID: 8074
		public enum ItemType
		{
			// Token: 0x04008EE9 RID: 36585
			Directory,
			// Token: 0x04008EEA RID: 36586
			File
		}
	}
}
