using System;

namespace Database
{
	// Token: 0x02000E63 RID: 3683
	public class Expressions : ResourceSet<Expression>
	{
		// Token: 0x0600748A RID: 29834 RVA: 0x002D4884 File Offset: 0x002D2A84
		public Expressions(ResourceSet parent) : base("Expressions", parent)
		{
			Faces faces = Db.Get().Faces;
			this.Angry = new Expression("Angry", this, faces.Angry);
			this.Suffocate = new Expression("Suffocate", this, faces.Suffocate);
			this.RecoverBreath = new Expression("RecoverBreath", this, faces.Uncomfortable);
			this.RedAlert = new Expression("RedAlert", this, faces.Hot);
			this.Hungry = new Expression("Hungry", this, faces.Hungry);
			this.Radiation1 = new Expression("Radiation1", this, faces.Radiation1);
			this.Radiation2 = new Expression("Radiation2", this, faces.Radiation2);
			this.Radiation3 = new Expression("Radiation3", this, faces.Radiation3);
			this.Radiation4 = new Expression("Radiation4", this, faces.Radiation4);
			this.SickSpores = new Expression("SickSpores", this, faces.SickSpores);
			this.Zombie = new Expression("Zombie", this, faces.Zombie);
			this.SickFierySkin = new Expression("SickFierySkin", this, faces.SickFierySkin);
			this.SickCold = new Expression("SickCold", this, faces.SickCold);
			this.Pollen = new Expression("Pollen", this, faces.Pollen);
			this.Sick = new Expression("Sick", this, faces.Sick);
			this.Cold = new Expression("Cold", this, faces.Cold);
			this.Hot = new Expression("Hot", this, faces.Hot);
			this.FullBladder = new Expression("FullBladder", this, faces.Uncomfortable);
			this.Tired = new Expression("Tired", this, faces.Tired);
			this.Unhappy = new Expression("Unhappy", this, faces.Uncomfortable);
			this.Uncomfortable = new Expression("Uncomfortable", this, faces.Uncomfortable);
			this.Productive = new Expression("Productive", this, faces.Productive);
			this.Determined = new Expression("Determined", this, faces.Determined);
			this.Sticker = new Expression("Sticker", this, faces.Sticker);
			this.Balloon = new Expression("Sticker", this, faces.Balloon);
			this.Sparkle = new Expression("Sticker", this, faces.Sparkle);
			this.Music = new Expression("Music", this, faces.Music);
			this.Tickled = new Expression("Tickled", this, faces.Tickled);
			this.Happy = new Expression("Happy", this, faces.Happy);
			this.Relief = new Expression("Relief", this, faces.Happy);
			this.Neutral = new Expression("Neutral", this, faces.Neutral);
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.resources[i].priority = 100 * (this.Count - i);
			}
		}

		// Token: 0x04005346 RID: 21318
		public Expression Neutral;

		// Token: 0x04005347 RID: 21319
		public Expression Happy;

		// Token: 0x04005348 RID: 21320
		public Expression Uncomfortable;

		// Token: 0x04005349 RID: 21321
		public Expression Cold;

		// Token: 0x0400534A RID: 21322
		public Expression Hot;

		// Token: 0x0400534B RID: 21323
		public Expression FullBladder;

		// Token: 0x0400534C RID: 21324
		public Expression Tired;

		// Token: 0x0400534D RID: 21325
		public Expression Hungry;

		// Token: 0x0400534E RID: 21326
		public Expression Angry;

		// Token: 0x0400534F RID: 21327
		public Expression Unhappy;

		// Token: 0x04005350 RID: 21328
		public Expression RedAlert;

		// Token: 0x04005351 RID: 21329
		public Expression Suffocate;

		// Token: 0x04005352 RID: 21330
		public Expression RecoverBreath;

		// Token: 0x04005353 RID: 21331
		public Expression Sick;

		// Token: 0x04005354 RID: 21332
		public Expression SickSpores;

		// Token: 0x04005355 RID: 21333
		public Expression Zombie;

		// Token: 0x04005356 RID: 21334
		public Expression SickFierySkin;

		// Token: 0x04005357 RID: 21335
		public Expression SickCold;

		// Token: 0x04005358 RID: 21336
		public Expression Pollen;

		// Token: 0x04005359 RID: 21337
		public Expression Relief;

		// Token: 0x0400535A RID: 21338
		public Expression Productive;

		// Token: 0x0400535B RID: 21339
		public Expression Determined;

		// Token: 0x0400535C RID: 21340
		public Expression Sticker;

		// Token: 0x0400535D RID: 21341
		public Expression Balloon;

		// Token: 0x0400535E RID: 21342
		public Expression Sparkle;

		// Token: 0x0400535F RID: 21343
		public Expression Music;

		// Token: 0x04005360 RID: 21344
		public Expression Tickled;

		// Token: 0x04005361 RID: 21345
		public Expression Radiation1;

		// Token: 0x04005362 RID: 21346
		public Expression Radiation2;

		// Token: 0x04005363 RID: 21347
		public Expression Radiation3;

		// Token: 0x04005364 RID: 21348
		public Expression Radiation4;
	}
}
