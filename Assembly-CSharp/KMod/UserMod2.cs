using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace KMod
{
	// Token: 0x02000ED5 RID: 3797
	public class UserMod2
	{
		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06007647 RID: 30279 RVA: 0x002E7B00 File Offset: 0x002E5D00
		// (set) Token: 0x06007648 RID: 30280 RVA: 0x002E7B08 File Offset: 0x002E5D08
		public Assembly assembly { get; set; }

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06007649 RID: 30281 RVA: 0x002E7B11 File Offset: 0x002E5D11
		// (set) Token: 0x0600764A RID: 30282 RVA: 0x002E7B19 File Offset: 0x002E5D19
		public string path { get; set; }

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x0600764B RID: 30283 RVA: 0x002E7B22 File Offset: 0x002E5D22
		// (set) Token: 0x0600764C RID: 30284 RVA: 0x002E7B2A File Offset: 0x002E5D2A
		public Mod mod { get; set; }

		// Token: 0x0600764D RID: 30285 RVA: 0x002E7B33 File Offset: 0x002E5D33
		public virtual void OnLoad(Harmony harmony)
		{
			harmony.PatchAll(this.assembly);
		}

		// Token: 0x0600764E RID: 30286 RVA: 0x002E7B41 File Offset: 0x002E5D41
		public virtual void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
		{
		}
	}
}
