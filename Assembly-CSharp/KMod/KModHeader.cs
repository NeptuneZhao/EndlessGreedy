using System;

namespace KMod
{
	// Token: 0x02000ED9 RID: 3801
	public class KModHeader
	{
		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06007654 RID: 30292 RVA: 0x002E7FA0 File Offset: 0x002E61A0
		// (set) Token: 0x06007655 RID: 30293 RVA: 0x002E7FA8 File Offset: 0x002E61A8
		public string staticID { get; set; }

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06007656 RID: 30294 RVA: 0x002E7FB1 File Offset: 0x002E61B1
		// (set) Token: 0x06007657 RID: 30295 RVA: 0x002E7FB9 File Offset: 0x002E61B9
		public string title { get; set; }

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06007658 RID: 30296 RVA: 0x002E7FC2 File Offset: 0x002E61C2
		// (set) Token: 0x06007659 RID: 30297 RVA: 0x002E7FCA File Offset: 0x002E61CA
		public string description { get; set; }
	}
}
