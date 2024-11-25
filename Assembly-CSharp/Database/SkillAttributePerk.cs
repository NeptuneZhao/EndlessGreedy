using System;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x02000ECD RID: 3789
	public class SkillAttributePerk : SkillPerk
	{
		// Token: 0x06007633 RID: 30259 RVA: 0x002E489C File Offset: 0x002E2A9C
		public SkillAttributePerk(string id, string attributeId, float modifierBonus, string modifierDesc) : base(id, "", null, null, delegate(MinionResume identity)
		{
		}, null, false)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.Get(attributeId);
			this.modifier = new AttributeModifier(attributeId, modifierBonus, modifierDesc, false, false, true);
			this.Name = string.Format(UI.ROLES_SCREEN.PERKS.ATTRIBUTE_EFFECT_FMT, this.modifier.GetFormattedString(), attribute.Name);
			base.OnApply = delegate(MinionResume identity)
			{
				if (identity.GetAttributes().Get(this.modifier.AttributeId).Modifiers.FindIndex((AttributeModifier mod) => mod == this.modifier) == -1)
				{
					identity.GetAttributes().Add(this.modifier);
				}
			};
			base.OnRemove = delegate(MinionResume identity)
			{
				identity.GetAttributes().Remove(this.modifier);
			};
		}

		// Token: 0x040055C9 RID: 21961
		public AttributeModifier modifier;
	}
}
