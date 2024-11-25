﻿using System;
using System.Collections.Generic;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02000F4D RID: 3917
	public class RadiationSickness : Sickness
	{
		// Token: 0x0600787E RID: 30846 RVA: 0x002FA82C File Offset: 0x002F8A2C
		public RadiationSickness() : base("RadiationSickness", Sickness.SicknessType.Pathogen, Sickness.Severity.Major, 0.00025f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Inhalation,
			Sickness.InfectionVector.Contact
		}, 10800f, "RadiationSicknessRecovery")
		{
			base.AddSicknessComponent(new CustomSickEffectSickness("spore_fx_kanim", "working_loop"));
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_spores_kanim",
				"anim_loco_spore_kanim"
			}, Db.Get().Expressions.Zombie));
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier(Db.Get().Attributes.Athletics.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Strength.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Digging.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Construction.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Art.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Caring.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Learning.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Machinery.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Cooking.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Botanist.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Ranching.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true)
			}));
		}

		// Token: 0x04005A07 RID: 23047
		public const string ID = "RadiationSickness";

		// Token: 0x04005A08 RID: 23048
		public const string RECOVERY_ID = "RadiationSicknessRecovery";

		// Token: 0x04005A09 RID: 23049
		public const int ATTRIBUTE_PENALTY = -10;
	}
}
