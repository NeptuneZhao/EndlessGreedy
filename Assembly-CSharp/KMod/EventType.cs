using System;

namespace KMod
{
	// Token: 0x02000EE7 RID: 3815
	public enum EventType
	{
		// Token: 0x040056B5 RID: 22197
		LoadError,
		// Token: 0x040056B6 RID: 22198
		NotFound,
		// Token: 0x040056B7 RID: 22199
		InstallInfoInaccessible,
		// Token: 0x040056B8 RID: 22200
		OutOfOrder,
		// Token: 0x040056B9 RID: 22201
		ExpectedActive,
		// Token: 0x040056BA RID: 22202
		ExpectedInactive,
		// Token: 0x040056BB RID: 22203
		ActiveDuringCrash,
		// Token: 0x040056BC RID: 22204
		InstallFailed,
		// Token: 0x040056BD RID: 22205
		Installed,
		// Token: 0x040056BE RID: 22206
		Uninstalled,
		// Token: 0x040056BF RID: 22207
		VersionUpdate,
		// Token: 0x040056C0 RID: 22208
		AvailableContentChanged,
		// Token: 0x040056C1 RID: 22209
		RestartRequested,
		// Token: 0x040056C2 RID: 22210
		BadWorldGen,
		// Token: 0x040056C3 RID: 22211
		Deactivated,
		// Token: 0x040056C4 RID: 22212
		DisabledEarlyAccess,
		// Token: 0x040056C5 RID: 22213
		DownloadFailed
	}
}
