using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace KMod
{
	// Token: 0x02000ED6 RID: 3798
	public class LoadedModData
	{
		// Token: 0x04005665 RID: 22117
		public Harmony harmony;

		// Token: 0x04005666 RID: 22118
		public Dictionary<Assembly, UserMod2> userMod2Instances;

		// Token: 0x04005667 RID: 22119
		public ICollection<Assembly> dlls;

		// Token: 0x04005668 RID: 22120
		public ICollection<MethodBase> patched_methods;
	}
}
