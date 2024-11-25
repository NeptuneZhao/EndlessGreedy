using System;
using System.Collections.Generic;

namespace Klei
{
	// Token: 0x02000F2D RID: 3885
	public class ClusterLayoutSave
	{
		// Token: 0x06007780 RID: 30592 RVA: 0x002F5DF0 File Offset: 0x002F3FF0
		public ClusterLayoutSave()
		{
			this.worlds = new List<ClusterLayoutSave.World>();
		}

		// Token: 0x04005965 RID: 22885
		public string ID;

		// Token: 0x04005966 RID: 22886
		public Vector2I version;

		// Token: 0x04005967 RID: 22887
		public List<ClusterLayoutSave.World> worlds;

		// Token: 0x04005968 RID: 22888
		public Vector2I size;

		// Token: 0x04005969 RID: 22889
		public int currentWorldIdx;

		// Token: 0x0400596A RID: 22890
		public int numRings;

		// Token: 0x0400596B RID: 22891
		public Dictionary<ClusterLayoutSave.POIType, List<AxialI>> poiLocations = new Dictionary<ClusterLayoutSave.POIType, List<AxialI>>();

		// Token: 0x0400596C RID: 22892
		public Dictionary<AxialI, string> poiPlacements = new Dictionary<AxialI, string>();

		// Token: 0x0200232E RID: 9006
		public class World
		{
			// Token: 0x04009DF3 RID: 40435
			public Data data = new Data();

			// Token: 0x04009DF4 RID: 40436
			public string name = string.Empty;

			// Token: 0x04009DF5 RID: 40437
			public bool isDiscovered;

			// Token: 0x04009DF6 RID: 40438
			public List<string> traits = new List<string>();

			// Token: 0x04009DF7 RID: 40439
			public List<string> storyTraits = new List<string>();

			// Token: 0x04009DF8 RID: 40440
			public List<string> seasons = new List<string>();

			// Token: 0x04009DF9 RID: 40441
			public List<string> generatedSubworlds = new List<string>();
		}

		// Token: 0x0200232F RID: 9007
		public enum POIType
		{
			// Token: 0x04009DFB RID: 40443
			TemporalTear,
			// Token: 0x04009DFC RID: 40444
			ResearchDestination
		}
	}
}
