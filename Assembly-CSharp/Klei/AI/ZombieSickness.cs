using System;
using System.Collections.Generic;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02000F54 RID: 3924
	public class ZombieSickness : Sickness
	{
		// Token: 0x060078A5 RID: 30885 RVA: 0x002FBBFC File Offset: 0x002F9DFC
		public ZombieSickness() : base("ZombieSickness", Sickness.SicknessType.Pathogen, Sickness.Severity.Major, 0.00025f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Inhalation,
			Sickness.InfectionVector.Contact
		}, 10800f, "ZombieSicknessRecovery")
		{
			base.AddSicknessComponent(new CustomSickEffectSickness("spore_fx_kanim", "working_loop"));
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_spores_kanim",
				"anim_loco_spore_kanim"
			}, Db.Get().Expressions.SickSpores));
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

		// Token: 0x04005A24 RID: 23076
		public const string ID = "ZombieSickness";

		// Token: 0x04005A25 RID: 23077
		public const string RECOVERY_ID = "ZombieSicknessRecovery";

		// Token: 0x04005A26 RID: 23078
		public const int ATTRIBUTE_PENALTY = -10;
	}
}
