using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F77 RID: 3959
	public class TraitUtil
	{
		// Token: 0x06007972 RID: 31090 RVA: 0x002FFB36 File Offset: 0x002FDD36
		public static System.Action CreateDisabledTaskTrait(string id, string name, string desc, string disabled_chore_group, bool is_valid_starter_trait)
		{
			return delegate()
			{
				ChoreGroup[] disabled_chore_groups = new ChoreGroup[]
				{
					Db.Get().ChoreGroups.Get(disabled_chore_group)
				};
				Db.Get().CreateTrait(id, name, desc, null, true, disabled_chore_groups, false, is_valid_starter_trait);
			};
		}

		// Token: 0x06007973 RID: 31091 RVA: 0x002FFB6C File Offset: 0x002FDD6C
		public static System.Action CreateTrait(string id, string name, string desc, string attributeId, float delta, string[] chore_groups, bool positiveTrait = false)
		{
			return delegate()
			{
				List<ChoreGroup> list = new List<ChoreGroup>();
				foreach (string id2 in chore_groups)
				{
					list.Add(Db.Get().ChoreGroups.Get(id2));
				}
				Db.Get().CreateTrait(id, name, desc, null, true, list.ToArray(), positiveTrait, true).Add(new AttributeModifier(attributeId, delta, name, false, false, true));
			};
		}

		// Token: 0x06007974 RID: 31092 RVA: 0x002FFBC0 File Offset: 0x002FDDC0
		public static System.Action CreateAttributeEffectTrait(string id, string name, string desc, string attributeId, float delta, string attributeId2, float delta2, bool positiveTrait = false)
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true);
				trait.Add(new AttributeModifier(attributeId, delta, name, false, false, true));
				trait.Add(new AttributeModifier(attributeId2, delta2, name, false, false, true));
			};
		}

		// Token: 0x06007975 RID: 31093 RVA: 0x002FFC19 File Offset: 0x002FDE19
		public static System.Action CreateAttributeEffectTrait(string id, string name, string desc, string[] attributeIds, float[] deltas, bool positiveTrait = false)
		{
			return delegate()
			{
				global::Debug.Assert(attributeIds.Length == deltas.Length, "CreateAttributeEffectTrait must have an equal number of attributeIds and deltas");
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true);
				for (int i = 0; i < attributeIds.Length; i++)
				{
					trait.Add(new AttributeModifier(attributeIds[i], deltas[i], name, false, false, true));
				}
			};
		}

		// Token: 0x06007976 RID: 31094 RVA: 0x002FFC58 File Offset: 0x002FDE58
		public static System.Action CreateAttributeEffectTrait(string id, string name, string desc, string attributeId, float delta, bool positiveTrait = false, Action<GameObject> on_add = null, bool is_valid_starter_trait = true)
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, is_valid_starter_trait);
				trait.Add(new AttributeModifier(attributeId, delta, name, false, false, true));
				trait.OnAddTrait = on_add;
			};
		}

		// Token: 0x06007977 RID: 31095 RVA: 0x002FFCB1 File Offset: 0x002FDEB1
		public static System.Action CreateEffectModifierTrait(string id, string name, string desc, string[] ignoredEffects, bool positiveTrait = false)
		{
			return delegate()
			{
				Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true).AddIgnoredEffects(ignoredEffects);
			};
		}

		// Token: 0x06007978 RID: 31096 RVA: 0x002FFCE7 File Offset: 0x002FDEE7
		public static System.Action CreateNamedTrait(string id, string name, string desc, bool positiveTrait = false)
		{
			return delegate()
			{
				Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true);
			};
		}

		// Token: 0x06007979 RID: 31097 RVA: 0x002FFD18 File Offset: 0x002FDF18
		public static System.Action CreateTrait(string id, string name, string desc, Action<GameObject> on_add, ChoreGroup[] disabled_chore_groups = null, bool positiveTrait = false, Func<string> extendedDescFn = null)
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, disabled_chore_groups, positiveTrait, true);
				trait.OnAddTrait = on_add;
				if (extendedDescFn != null)
				{
					Trait trait2 = trait;
					trait2.ExtendedTooltip = (Func<string>)Delegate.Combine(trait2.ExtendedTooltip, extendedDescFn);
				}
			};
		}

		// Token: 0x0600797A RID: 31098 RVA: 0x002FFD69 File Offset: 0x002FDF69
		public static System.Action CreateComponentTrait<T>(string id, string name, string desc, bool positiveTrait = false, Func<string> extendedDescFn = null) where T : KMonoBehaviour
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true);
				trait.OnAddTrait = delegate(GameObject go)
				{
					go.FindOrAddUnityComponent<T>();
				};
				if (extendedDescFn != null)
				{
					Trait trait2 = trait;
					trait2.ExtendedTooltip = (Func<string>)Delegate.Combine(trait2.ExtendedTooltip, extendedDescFn);
				}
			};
		}

		// Token: 0x0600797B RID: 31099 RVA: 0x002FFD9F File Offset: 0x002FDF9F
		public static System.Action CreateSkillGrantingTrait(string id, string name, string desc, string skillId)
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, true, true);
				trait.TooltipCB = (() => string.Format(DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_DESC, desc, SkillWidget.SkillPerksString(Db.Get().Skills.Get(skillId))));
				trait.OnAddTrait = delegate(GameObject go)
				{
					MinionResume component = go.GetComponent<MinionResume>();
					if (component != null)
					{
						component.GrantSkill(skillId);
					}
				};
			};
		}

		// Token: 0x0600797C RID: 31100 RVA: 0x002FFDD0 File Offset: 0x002FDFD0
		public static string GetSkillGrantingTraitNameById(string id)
		{
			string result = "";
			StringEntry stringEntry;
			if (Strings.TryGet("STRINGS.DUPLICANTS.TRAITS.GRANTSKILL_" + id.ToUpper() + ".NAME", out stringEntry))
			{
				result = stringEntry.String;
			}
			return result;
		}
	}
}
