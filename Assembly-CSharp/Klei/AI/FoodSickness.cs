using System;
using System.Collections.Generic;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02000F4A RID: 3914
	public class FoodSickness : Sickness
	{
		// Token: 0x06007879 RID: 30841 RVA: 0x002FA36C File Offset: 0x002F856C
		public FoodSickness() : base("FoodSickness", Sickness.SicknessType.Pathogen, Sickness.Severity.Minor, 0.005f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Digestion
		}, 1020f, "FoodSicknessRecovery")
		{
			base.AddSicknessComponent(new CommonSickEffectSickness());
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier("BladderDelta", 0.33333334f, DUPLICANTS.DISEASES.FOODSICKNESS.NAME, false, false, true),
				new AttributeModifier("ToiletEfficiency", -0.2f, DUPLICANTS.DISEASES.FOODSICKNESS.NAME, false, false, true),
				new AttributeModifier("StaminaDelta", -0.05f, DUPLICANTS.DISEASES.FOODSICKNESS.NAME, false, false, true)
			}));
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_sick_kanim"
			}, Db.Get().Expressions.Sick));
			base.AddSicknessComponent(new PeriodicEmoteSickness(Db.Get().Emotes.Minion.Sick, 10f));
		}

		// Token: 0x04005A02 RID: 23042
		public const string ID = "FoodSickness";

		// Token: 0x04005A03 RID: 23043
		public const string RECOVERY_ID = "FoodSicknessRecovery";

		// Token: 0x04005A04 RID: 23044
		private const float VOMIT_FREQUENCY = 200f;
	}
}
