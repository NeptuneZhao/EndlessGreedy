using System;
using System.Collections.Generic;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02000F53 RID: 3923
	public class Sunburn : Sickness
	{
		// Token: 0x060078A4 RID: 30884 RVA: 0x002FBB04 File Offset: 0x002F9D04
		public Sunburn() : base("SunburnSickness", Sickness.SicknessType.Ailment, Sickness.Severity.Minor, 0.005f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Exposure
		}, 1020f, null)
		{
			base.AddSicknessComponent(new CommonSickEffectSickness());
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.033333335f, DUPLICANTS.DISEASES.SUNBURNSICKNESS.NAME, false, false, true)
			}));
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_hot_kanim",
				"anim_loco_run_hot_kanim",
				"anim_loco_walk_hot_kanim"
			}, Db.Get().Expressions.SickFierySkin));
			base.AddSicknessComponent(new PeriodicEmoteSickness(Db.Get().Emotes.Minion.Hot, 5f));
		}

		// Token: 0x04005A23 RID: 23075
		public const string ID = "SunburnSickness";
	}
}
