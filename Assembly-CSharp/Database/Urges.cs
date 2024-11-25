using System;

namespace Database
{
	// Token: 0x02000E8C RID: 3724
	public class Urges : ResourceSet<Urge>
	{
		// Token: 0x0600751D RID: 29981 RVA: 0x002DF55C File Offset: 0x002DD75C
		public Urges()
		{
			this.HealCritical = base.Add(new Urge("HealCritical"));
			this.BeOffline = base.Add(new Urge("BeOffline"));
			this.BeIncapacitated = base.Add(new Urge("BeIncapacitated"));
			this.PacifyEat = base.Add(new Urge("PacifyEat"));
			this.PacifySleep = base.Add(new Urge("PacifySleep"));
			this.PacifyIdle = base.Add(new Urge("PacifyIdle"));
			this.EmoteHighPriority = base.Add(new Urge("EmoteHighPriority"));
			this.RecoverBreath = base.Add(new Urge("RecoverBreath"));
			this.RecoverWarmth = base.Add(new Urge("RecoverWarmth"));
			this.Aggression = base.Add(new Urge("Aggression"));
			this.MoveToQuarantine = base.Add(new Urge("MoveToQuarantine"));
			this.WashHands = base.Add(new Urge("WashHands"));
			this.Shower = base.Add(new Urge("Shower"));
			this.Eat = base.Add(new Urge("Eat"));
			this.ReloadElectrobank = base.Add(new Urge("ReloadElectrobank"));
			this.RemoveDischargedElectrobank = base.Add(new Urge("RemoveDischargedElectrobank"));
			this.Pee = base.Add(new Urge("Pee"));
			this.RestDueToDisease = base.Add(new Urge("RestDueToDisease"));
			this.Sleep = base.Add(new Urge("Sleep"));
			this.Narcolepsy = base.Add(new Urge("Narcolepsy"));
			this.Doctor = base.Add(new Urge("Doctor"));
			this.Heal = base.Add(new Urge("Heal"));
			this.Feed = base.Add(new Urge("Feed"));
			this.PacifyRelocate = base.Add(new Urge("PacifyRelocate"));
			this.Emote = base.Add(new Urge("Emote"));
			this.MoveToSafety = base.Add(new Urge("MoveToSafety"));
			this.WarmUp = base.Add(new Urge("WarmUp"));
			this.CoolDown = base.Add(new Urge("CoolDown"));
			this.LearnSkill = base.Add(new Urge("LearnSkill"));
			this.EmoteIdle = base.Add(new Urge("EmoteIdle"));
			this.OilRefill = base.Add(new Urge("OilRefill"));
			this.GunkPee = base.Add(new Urge("GunkPee"));
			this.FindOxygenRefill = base.Add(new Urge("FindOxygenRefill"));
		}

		// Token: 0x04005524 RID: 21796
		public Urge BeIncapacitated;

		// Token: 0x04005525 RID: 21797
		public Urge BeOffline;

		// Token: 0x04005526 RID: 21798
		public Urge Sleep;

		// Token: 0x04005527 RID: 21799
		public Urge Narcolepsy;

		// Token: 0x04005528 RID: 21800
		public Urge Eat;

		// Token: 0x04005529 RID: 21801
		public Urge RemoveDischargedElectrobank;

		// Token: 0x0400552A RID: 21802
		public Urge ReloadElectrobank;

		// Token: 0x0400552B RID: 21803
		public Urge WashHands;

		// Token: 0x0400552C RID: 21804
		public Urge Shower;

		// Token: 0x0400552D RID: 21805
		public Urge Pee;

		// Token: 0x0400552E RID: 21806
		public Urge MoveToQuarantine;

		// Token: 0x0400552F RID: 21807
		public Urge HealCritical;

		// Token: 0x04005530 RID: 21808
		public Urge RecoverBreath;

		// Token: 0x04005531 RID: 21809
		public Urge FindOxygenRefill;

		// Token: 0x04005532 RID: 21810
		public Urge RecoverWarmth;

		// Token: 0x04005533 RID: 21811
		public Urge Emote;

		// Token: 0x04005534 RID: 21812
		public Urge Feed;

		// Token: 0x04005535 RID: 21813
		public Urge Doctor;

		// Token: 0x04005536 RID: 21814
		public Urge Flee;

		// Token: 0x04005537 RID: 21815
		public Urge Heal;

		// Token: 0x04005538 RID: 21816
		public Urge PacifyIdle;

		// Token: 0x04005539 RID: 21817
		public Urge PacifyEat;

		// Token: 0x0400553A RID: 21818
		public Urge PacifySleep;

		// Token: 0x0400553B RID: 21819
		public Urge PacifyRelocate;

		// Token: 0x0400553C RID: 21820
		public Urge RestDueToDisease;

		// Token: 0x0400553D RID: 21821
		public Urge EmoteHighPriority;

		// Token: 0x0400553E RID: 21822
		public Urge Aggression;

		// Token: 0x0400553F RID: 21823
		public Urge MoveToSafety;

		// Token: 0x04005540 RID: 21824
		public Urge WarmUp;

		// Token: 0x04005541 RID: 21825
		public Urge CoolDown;

		// Token: 0x04005542 RID: 21826
		public Urge LearnSkill;

		// Token: 0x04005543 RID: 21827
		public Urge EmoteIdle;

		// Token: 0x04005544 RID: 21828
		public Urge OilRefill;

		// Token: 0x04005545 RID: 21829
		public Urge GunkPee;
	}
}
