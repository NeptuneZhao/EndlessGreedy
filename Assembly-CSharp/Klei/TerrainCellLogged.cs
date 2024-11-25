using System;
using System.Collections.Generic;
using ProcGen.Map;
using ProcGenGame;
using VoronoiTree;

namespace Klei
{
	// Token: 0x02000F2F RID: 3887
	public class TerrainCellLogged : TerrainCell
	{
		// Token: 0x06007790 RID: 30608 RVA: 0x002F61EE File Offset: 0x002F43EE
		public TerrainCellLogged()
		{
		}

		// Token: 0x06007791 RID: 30609 RVA: 0x002F61F6 File Offset: 0x002F43F6
		public TerrainCellLogged(Cell node, Diagram.Site site, Dictionary<Tag, int> distancesToTags) : base(node, site, distancesToTags)
		{
		}

		// Token: 0x06007792 RID: 30610 RVA: 0x002F6201 File Offset: 0x002F4401
		public override void LogInfo(string evt, string param, float value)
		{
		}
	}
}
