using System;

namespace KMod
{
	// Token: 0x02000EE2 RID: 3810
	[Flags]
	public enum Content : byte
	{
		// Token: 0x0400567B RID: 22139
		LayerableFiles = 1,
		// Token: 0x0400567C RID: 22140
		Strings = 2,
		// Token: 0x0400567D RID: 22141
		DLL = 4,
		// Token: 0x0400567E RID: 22142
		Translation = 8,
		// Token: 0x0400567F RID: 22143
		Animation = 16
	}
}
