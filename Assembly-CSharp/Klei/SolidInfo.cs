using System;

namespace Klei
{
	// Token: 0x02000F26 RID: 3878
	public struct SolidInfo
	{
		// Token: 0x06007778 RID: 30584 RVA: 0x002F5CC5 File Offset: 0x002F3EC5
		public SolidInfo(int cellIdx, bool isSolid)
		{
			this.cellIdx = cellIdx;
			this.isSolid = isSolid;
		}

		// Token: 0x0400593F RID: 22847
		public int cellIdx;

		// Token: 0x04005940 RID: 22848
		public bool isSolid;
	}
}
