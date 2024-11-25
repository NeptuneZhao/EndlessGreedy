using System;

namespace Database
{
	// Token: 0x02000E64 RID: 3684
	public class Faces : ResourceSet<Face>
	{
		// Token: 0x0600748B RID: 29835 RVA: 0x002D4BA0 File Offset: 0x002D2DA0
		public Faces()
		{
			this.Neutral = base.Add(new Face("Neutral", null));
			this.Happy = base.Add(new Face("Happy", null));
			this.Uncomfortable = base.Add(new Face("Uncomfortable", null));
			this.Cold = base.Add(new Face("Cold", null));
			this.Hot = base.Add(new Face("Hot", "headfx_sweat"));
			this.Tired = base.Add(new Face("Tired", null));
			this.Sleep = base.Add(new Face("Sleep", null));
			this.Hungry = base.Add(new Face("Hungry", null));
			this.Angry = base.Add(new Face("Angry", null));
			this.Suffocate = base.Add(new Face("Suffocate", null));
			this.Sick = base.Add(new Face("Sick", "headfx_sick"));
			this.SickSpores = base.Add(new Face("Spores", "headfx_spores"));
			this.Zombie = base.Add(new Face("Zombie", null));
			this.SickFierySkin = base.Add(new Face("Fiery", "headfx_fiery"));
			this.SickCold = base.Add(new Face("SickCold", "headfx_sickcold"));
			this.Pollen = base.Add(new Face("Pollen", "headfx_pollen"));
			this.Dead = base.Add(new Face("Death", null));
			this.Productive = base.Add(new Face("Productive", null));
			this.Determined = base.Add(new Face("Determined", null));
			this.Sticker = base.Add(new Face("Sticker", null));
			this.Sparkle = base.Add(new Face("Sparkle", null));
			this.Balloon = base.Add(new Face("Balloon", null));
			this.Tickled = base.Add(new Face("Tickled", null));
			this.Music = base.Add(new Face("Music", null));
			this.Radiation1 = base.Add(new Face("Radiation1", "headfx_radiation1"));
			this.Radiation2 = base.Add(new Face("Radiation2", "headfx_radiation2"));
			this.Radiation3 = base.Add(new Face("Radiation3", "headfx_radiation3"));
			this.Radiation4 = base.Add(new Face("Radiation4", "headfx_radiation4"));
		}

		// Token: 0x04005365 RID: 21349
		public Face Neutral;

		// Token: 0x04005366 RID: 21350
		public Face Happy;

		// Token: 0x04005367 RID: 21351
		public Face Uncomfortable;

		// Token: 0x04005368 RID: 21352
		public Face Cold;

		// Token: 0x04005369 RID: 21353
		public Face Hot;

		// Token: 0x0400536A RID: 21354
		public Face Tired;

		// Token: 0x0400536B RID: 21355
		public Face Sleep;

		// Token: 0x0400536C RID: 21356
		public Face Hungry;

		// Token: 0x0400536D RID: 21357
		public Face Angry;

		// Token: 0x0400536E RID: 21358
		public Face Suffocate;

		// Token: 0x0400536F RID: 21359
		public Face Dead;

		// Token: 0x04005370 RID: 21360
		public Face Sick;

		// Token: 0x04005371 RID: 21361
		public Face SickSpores;

		// Token: 0x04005372 RID: 21362
		public Face Zombie;

		// Token: 0x04005373 RID: 21363
		public Face SickFierySkin;

		// Token: 0x04005374 RID: 21364
		public Face SickCold;

		// Token: 0x04005375 RID: 21365
		public Face Pollen;

		// Token: 0x04005376 RID: 21366
		public Face Productive;

		// Token: 0x04005377 RID: 21367
		public Face Determined;

		// Token: 0x04005378 RID: 21368
		public Face Sticker;

		// Token: 0x04005379 RID: 21369
		public Face Balloon;

		// Token: 0x0400537A RID: 21370
		public Face Sparkle;

		// Token: 0x0400537B RID: 21371
		public Face Tickled;

		// Token: 0x0400537C RID: 21372
		public Face Music;

		// Token: 0x0400537D RID: 21373
		public Face Radiation1;

		// Token: 0x0400537E RID: 21374
		public Face Radiation2;

		// Token: 0x0400537F RID: 21375
		public Face Radiation3;

		// Token: 0x04005380 RID: 21376
		public Face Radiation4;
	}
}
