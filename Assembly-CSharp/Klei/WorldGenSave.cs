using System;
using System.Collections.Generic;

namespace Klei
{
	// Token: 0x02000F2A RID: 3882
	public class WorldGenSave
	{
		// Token: 0x0600777D RID: 30589 RVA: 0x002F5DB7 File Offset: 0x002F3FB7
		public WorldGenSave()
		{
			this.data = new Data();
		}

		// Token: 0x04005955 RID: 22869
		public Vector2I version;

		// Token: 0x04005956 RID: 22870
		public Data data;

		// Token: 0x04005957 RID: 22871
		public string worldID;

		// Token: 0x04005958 RID: 22872
		public List<string> traitIDs;

		// Token: 0x04005959 RID: 22873
		public List<string> storyTraitIDs;
	}
}
