using System;
using System.Collections.Generic;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02000F47 RID: 3911
	public class Allergies : Sickness
	{
		// Token: 0x06007863 RID: 30819 RVA: 0x002F9020 File Offset: 0x002F7220
		public Allergies() : base("Allergies", Sickness.SicknessType.Pathogen, Sickness.Severity.Minor, 0.00025f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Inhalation
		}, 60f, null)
		{
			float value = 0.025f;
			base.AddSicknessComponent(new CommonSickEffectSickness());
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_allergies_kanim"
			}, Db.Get().Expressions.Pollen));
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, value, DUPLICANTS.DISEASES.ALLERGIES.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Sneezyness.Id, 10f, DUPLICANTS.DISEASES.ALLERGIES.NAME, false, false, true)
			}));
		}

		// Token: 0x040059EB RID: 23019
		public const string ID = "Allergies";

		// Token: 0x040059EC RID: 23020
		public const float STRESS_PER_CYCLE = 15f;
	}
}
