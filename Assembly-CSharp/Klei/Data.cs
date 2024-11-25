using System;
using System.Collections.Generic;
using ProcGen;
using ProcGenGame;
using VoronoiTree;

namespace Klei
{
	// Token: 0x02000F29 RID: 3881
	public class Data
	{
		// Token: 0x0600777C RID: 30588 RVA: 0x002F5D4C File Offset: 0x002F3F4C
		public Data()
		{
			this.worldLayout = new WorldLayout(null, 0);
			this.terrainCells = new List<TerrainCell>();
			this.overworldCells = new List<TerrainCell>();
			this.rivers = new List<ProcGen.River>();
			this.gameSpawnData = new GameSpawnData();
			this.world = new Chunk();
			this.voronoiTree = new Tree(0);
		}

		// Token: 0x04005948 RID: 22856
		public int globalWorldSeed;

		// Token: 0x04005949 RID: 22857
		public int globalWorldLayoutSeed;

		// Token: 0x0400594A RID: 22858
		public int globalTerrainSeed;

		// Token: 0x0400594B RID: 22859
		public int globalNoiseSeed;

		// Token: 0x0400594C RID: 22860
		public int chunkEdgeSize = 32;

		// Token: 0x0400594D RID: 22861
		public WorldLayout worldLayout;

		// Token: 0x0400594E RID: 22862
		public List<TerrainCell> terrainCells;

		// Token: 0x0400594F RID: 22863
		public List<TerrainCell> overworldCells;

		// Token: 0x04005950 RID: 22864
		public List<ProcGen.River> rivers;

		// Token: 0x04005951 RID: 22865
		public GameSpawnData gameSpawnData;

		// Token: 0x04005952 RID: 22866
		public Chunk world;

		// Token: 0x04005953 RID: 22867
		public Tree voronoiTree;

		// Token: 0x04005954 RID: 22868
		public AxialI clusterLocation;
	}
}
