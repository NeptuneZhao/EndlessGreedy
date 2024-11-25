using System;

namespace Database
{
	// Token: 0x02000E7C RID: 3708
	public class Shirts : ResourceSet<Shirt>
	{
		// Token: 0x060074DB RID: 29915 RVA: 0x002DAC08 File Offset: 0x002D8E08
		public Shirts()
		{
			this.Hot00 = base.Add(new Shirt("body_shirt_hot_shearling"));
			this.Decor00 = base.Add(new Shirt("body_shirt_decor01"));
		}

		// Token: 0x04005496 RID: 21654
		public Shirt Hot00;

		// Token: 0x04005497 RID: 21655
		public Shirt Decor00;
	}
}
