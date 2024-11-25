using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E8B RID: 3723
	public class Thoughts : ResourceSet<Thought>
	{
		// Token: 0x0600751C RID: 29980 RVA: 0x002DF084 File Offset: 0x002DD284
		public Thoughts(ResourceSet parent) : base("Thoughts", parent)
		{
			this.GotInfected = new Thought("GotInfected", this, "crew_state_sick", null, "crew_state_sick", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.GOTINFECTED.TOOLTIP, false, 4f);
			this.Starving = new Thought("Starving", this, "crew_state_hungry", null, "crew_state_hungry", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.STARVING.TOOLTIP, false, 4f);
			this.Hot = new Thought("Hot", this, "crew_state_temp_up", null, "crew_state_temp_up", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.HOT.TOOLTIP, false, 4f);
			this.Cold = new Thought("Cold", this, "crew_state_temp_down", null, "crew_state_temp_down", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.COLD.TOOLTIP, false, 4f);
			this.BreakBladder = new Thought("BreakBladder", this, "crew_state_full_bladder", null, "crew_state_full_bladder", "bubble_conversation", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.BREAKBLADDER.TOOLTIP, false, 4f);
			this.FullBladder = new Thought("FullBladder", this, "crew_state_full_bladder", null, "crew_state_full_bladder", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.FULLBLADDER.TOOLTIP, false, 4f);
			this.ExpellingGunk = new Thought("ExpellingGunk", this, "crew_state_oil_change_desire", null, "crew_state_oil_change_desire", "bubble_conversation", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.EXPELLINGSPOILEDOIL.TOOLTIP, false, 4f);
			this.ExpellGunkDesire = new Thought("ExpellGunkDesire", this, "crew_state_oil_change_desire", null, "crew_state_oil_change_desire", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.EXPELLGUNKDESIRE.TOOLTIP, false, 4f);
			this.PoorDecor = new Thought("PoorDecor", this, "crew_state_decor", null, "crew_state_decor", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.POORDECOR.TOOLTIP, false, 4f);
			this.PoorFoodQuality = new Thought("PoorFoodQuality", this, "crew_state_yuck", null, "crew_state_yuck", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.POOR_FOOD_QUALITY.TOOLTIP, false, 4f);
			this.GoodFoodQuality = new Thought("GoodFoodQuality", this, "crew_state_happy", null, "crew_state_happy", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.GOOD_FOOD_QUALITY.TOOLTIP, false, 4f);
			this.Happy = new Thought("Happy", this, "crew_state_happy", null, "crew_state_happy", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.HAPPY.TOOLTIP, false, 4f);
			this.Unhappy = new Thought("Unhappy", this, "crew_state_unhappy", null, "crew_state_unhappy", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.UNHAPPY.TOOLTIP, false, 4f);
			this.Sleepy = new Thought("Sleepy", this, "crew_state_sleepy", null, "crew_state_sleepy", "bubble_conversation", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.SLEEPY.TOOLTIP, false, 4f);
			this.Suffocating = new Thought("Suffocating", this, "crew_state_cantbreathe", null, "crew_state_cantbreathe", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.SUFFOCATING.TOOLTIP, false, 4f);
			this.Angry = new Thought("Angry", this, "crew_state_angry", null, "crew_state_angry", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.ANGRY.TOOLTIP, false, 4f);
			this.Raging = new Thought("Enraged", this, "crew_state_enraged", null, "crew_state_enraged", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.RAGING.TOOLTIP, false, 4f);
			this.PutridOdour = new Thought("PutridOdour", this, "crew_state_smelled_putrid_odour", null, "crew_state_smelled_putrid_odour", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.PUTRIDODOUR.TOOLTIP, true, 4f);
			this.Noisy = new Thought("Noisy", this, "crew_state_noisey", null, "crew_state_noisey", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.NOISY.TOOLTIP, true, 4f);
			this.NewRole = new Thought("NewRole", this, "crew_state_role", null, "crew_state_role", "bubble_alert", SpeechMonitor.PREFIX_SAD, DUPLICANTS.THOUGHTS.NEWROLE.TOOLTIP, false, 4f);
			this.Encourage = new Thought("Encourage", this, "crew_state_encourage", null, "crew_state_happy", "bubble_conversation", SpeechMonitor.PREFIX_HAPPY, DUPLICANTS.THOUGHTS.ENCOURAGE.TOOLTIP, false, 4f);
			this.Chatty = new Thought("Chatty", this, "crew_state_chatty", null, "conversation_short", "bubble_conversation", SpeechMonitor.PREFIX_HAPPY, DUPLICANTS.THOUGHTS.CHATTY.TOOLTIP, false, 4f);
			this.CatchyTune = new Thought("CatchyTune", this, "crew_state_music", null, "crew_state_music", "bubble_conversation", SpeechMonitor.PREFIX_SINGER, DUPLICANTS.THOUGHTS.CATCHYTUNE.TOOLTIP, true, 4f);
			this.Dreaming = new Thought("Dreaming", this, "pajamas", null, null, "bubble_dream", null, DUPLICANTS.THOUGHTS.DREAMY.TOOLTIP, false, 4f);
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.resources[i].priority = 100 * (this.Count - i);
			}
		}

		// Token: 0x0400550C RID: 21772
		public Thought Starving;

		// Token: 0x0400550D RID: 21773
		public Thought Hot;

		// Token: 0x0400550E RID: 21774
		public Thought Cold;

		// Token: 0x0400550F RID: 21775
		public Thought BreakBladder;

		// Token: 0x04005510 RID: 21776
		public Thought FullBladder;

		// Token: 0x04005511 RID: 21777
		public Thought ExpellGunkDesire;

		// Token: 0x04005512 RID: 21778
		public Thought ExpellingGunk;

		// Token: 0x04005513 RID: 21779
		public Thought Happy;

		// Token: 0x04005514 RID: 21780
		public Thought Unhappy;

		// Token: 0x04005515 RID: 21781
		public Thought PoorDecor;

		// Token: 0x04005516 RID: 21782
		public Thought PoorFoodQuality;

		// Token: 0x04005517 RID: 21783
		public Thought GoodFoodQuality;

		// Token: 0x04005518 RID: 21784
		public Thought Sleepy;

		// Token: 0x04005519 RID: 21785
		public Thought Suffocating;

		// Token: 0x0400551A RID: 21786
		public Thought Angry;

		// Token: 0x0400551B RID: 21787
		public Thought Raging;

		// Token: 0x0400551C RID: 21788
		public Thought GotInfected;

		// Token: 0x0400551D RID: 21789
		public Thought PutridOdour;

		// Token: 0x0400551E RID: 21790
		public Thought Noisy;

		// Token: 0x0400551F RID: 21791
		public Thought NewRole;

		// Token: 0x04005520 RID: 21792
		public Thought Chatty;

		// Token: 0x04005521 RID: 21793
		public Thought Encourage;

		// Token: 0x04005522 RID: 21794
		public Thought CatchyTune;

		// Token: 0x04005523 RID: 21795
		public Thought Dreaming;
	}
}
