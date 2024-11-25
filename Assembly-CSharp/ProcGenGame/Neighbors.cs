using System;
using KSerialization;

namespace ProcGenGame
{
	// Token: 0x02000E0A RID: 3594
	[SerializationConfig(MemberSerialization.OptOut)]
	public struct Neighbors
	{
		// Token: 0x0600721B RID: 29211 RVA: 0x002B658A File Offset: 0x002B478A
		public Neighbors(TerrainCell a, TerrainCell b)
		{
			Debug.Assert(a != null && b != null, "NULL Neighbor");
			this.n0 = a;
			this.n1 = b;
		}

		// Token: 0x04004EB2 RID: 20146
		public TerrainCell n0;

		// Token: 0x04004EB3 RID: 20147
		public TerrainCell n1;
	}
}
