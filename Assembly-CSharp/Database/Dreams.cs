using System;

namespace Database
{
	// Token: 0x02000E5E RID: 3678
	public class Dreams : ResourceSet<Dream>
	{
		// Token: 0x06007476 RID: 29814 RVA: 0x002D2721 File Offset: 0x002D0921
		public Dreams(ResourceSet parent) : base("Dreams", parent)
		{
			this.CommonDream = new Dream("CommonDream", this, "dream_tear_swirly_kanim", new string[]
			{
				"dreamIcon_journal"
			});
		}

		// Token: 0x040052AC RID: 21164
		public Dream CommonDream;
	}
}
