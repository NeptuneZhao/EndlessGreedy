using System;

namespace KMod
{
	// Token: 0x02000ED4 RID: 3796
	public static class Testing
	{
		// Token: 0x0400565E RID: 22110
		public static Testing.DLLLoading dll_loading;

		// Token: 0x0400565F RID: 22111
		public const Testing.SaveLoad SAVE_LOAD = Testing.SaveLoad.NoTesting;

		// Token: 0x04005660 RID: 22112
		public const Testing.Install INSTALL = Testing.Install.NoTesting;

		// Token: 0x04005661 RID: 22113
		public const Testing.Boot BOOT = Testing.Boot.NoTesting;

		// Token: 0x02001F82 RID: 8066
		public enum DLLLoading
		{
			// Token: 0x04008ED5 RID: 36565
			NoTesting,
			// Token: 0x04008ED6 RID: 36566
			Fail,
			// Token: 0x04008ED7 RID: 36567
			UseModLoaderDLLExclusively
		}

		// Token: 0x02001F83 RID: 8067
		public enum SaveLoad
		{
			// Token: 0x04008ED9 RID: 36569
			NoTesting,
			// Token: 0x04008EDA RID: 36570
			FailSave,
			// Token: 0x04008EDB RID: 36571
			FailLoad
		}

		// Token: 0x02001F84 RID: 8068
		public enum Install
		{
			// Token: 0x04008EDD RID: 36573
			NoTesting,
			// Token: 0x04008EDE RID: 36574
			ForceUninstall,
			// Token: 0x04008EDF RID: 36575
			ForceReinstall,
			// Token: 0x04008EE0 RID: 36576
			ForceUpdate
		}

		// Token: 0x02001F85 RID: 8069
		public enum Boot
		{
			// Token: 0x04008EE2 RID: 36578
			NoTesting,
			// Token: 0x04008EE3 RID: 36579
			Crash
		}
	}
}
