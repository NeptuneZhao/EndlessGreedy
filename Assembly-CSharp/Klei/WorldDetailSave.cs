using System;
using System.Collections.Generic;
using Delaunay.Geo;
using KSerialization;
using ProcGen;
using ProcGenGame;

namespace Klei
{
	// Token: 0x02000F2B RID: 3883
	public class WorldDetailSave
	{
		// Token: 0x0600777E RID: 30590 RVA: 0x002F5DCA File Offset: 0x002F3FCA
		public WorldDetailSave()
		{
			this.overworldCells = new List<WorldDetailSave.OverworldCell>();
		}

		// Token: 0x0400595A RID: 22874
		public List<WorldDetailSave.OverworldCell> overworldCells;

		// Token: 0x0400595B RID: 22875
		public int globalWorldSeed;

		// Token: 0x0400595C RID: 22876
		public int globalWorldLayoutSeed;

		// Token: 0x0400595D RID: 22877
		public int globalTerrainSeed;

		// Token: 0x0400595E RID: 22878
		public int globalNoiseSeed;

		// Token: 0x0200232D RID: 9005
		[SerializationConfig(MemberSerialization.OptOut)]
		public class OverworldCell
		{
			// Token: 0x0600B5EA RID: 46570 RVA: 0x003C8A8A File Offset: 0x003C6C8A
			public OverworldCell()
			{
			}

			// Token: 0x0600B5EB RID: 46571 RVA: 0x003C8A92 File Offset: 0x003C6C92
			public OverworldCell(SubWorld.ZoneType zoneType, TerrainCell tc)
			{
				this.poly = tc.poly;
				this.tags = tc.node.tags;
				this.zoneType = zoneType;
			}

			// Token: 0x04009DF0 RID: 40432
			public Polygon poly;

			// Token: 0x04009DF1 RID: 40433
			public TagSet tags;

			// Token: 0x04009DF2 RID: 40434
			public SubWorld.ZoneType zoneType;
		}
	}
}
