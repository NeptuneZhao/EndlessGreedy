using System;
using System.Collections.Generic;
using KMod;

namespace Klei
{
	// Token: 0x02000F28 RID: 3880
	internal class SaveFileRoot
	{
		// Token: 0x0600777B RID: 30587 RVA: 0x002F5D39 File Offset: 0x002F3F39
		public SaveFileRoot()
		{
			this.streamed = new Dictionary<string, byte[]>();
		}

		// Token: 0x04005942 RID: 22850
		public int WidthInCells;

		// Token: 0x04005943 RID: 22851
		public int HeightInCells;

		// Token: 0x04005944 RID: 22852
		public Dictionary<string, byte[]> streamed;

		// Token: 0x04005945 RID: 22853
		public string clusterID;

		// Token: 0x04005946 RID: 22854
		public List<ModInfo> requiredMods;

		// Token: 0x04005947 RID: 22855
		public List<Label> active_mods;
	}
}
