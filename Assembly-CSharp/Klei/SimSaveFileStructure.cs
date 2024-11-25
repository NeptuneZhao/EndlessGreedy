using System;

namespace Klei
{
	// Token: 0x02000F2C RID: 3884
	public class SimSaveFileStructure
	{
		// Token: 0x0600777F RID: 30591 RVA: 0x002F5DDD File Offset: 0x002F3FDD
		public SimSaveFileStructure()
		{
			this.worldDetail = new WorldDetailSave();
		}

		// Token: 0x0400595F RID: 22879
		public int WidthInCells;

		// Token: 0x04005960 RID: 22880
		public int HeightInCells;

		// Token: 0x04005961 RID: 22881
		public int x;

		// Token: 0x04005962 RID: 22882
		public int y;

		// Token: 0x04005963 RID: 22883
		public byte[] Sim;

		// Token: 0x04005964 RID: 22884
		public WorldDetailSave worldDetail;
	}
}
