using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F76 RID: 3958
	public class Trait : Modifier
	{
		// Token: 0x06007969 RID: 31081 RVA: 0x002FF79E File Offset: 0x002FD99E
		public Trait(string id, string name, string description, float rating, bool should_save, ChoreGroup[] disallowed_chore_groups, bool positive_trait, bool is_valid_starter_trait) : base(id, name, description)
		{
			this.Rating = rating;
			this.ShouldSave = should_save;
			this.disabledChoreGroups = disallowed_chore_groups;
			this.PositiveTrait = positive_trait;
			this.ValidStarterTrait = is_valid_starter_trait;
			this.ignoredEffects = new string[0];
		}

		// Token: 0x0600796A RID: 31082 RVA: 0x002FF7E0 File Offset: 0x002FD9E0
		public void AddIgnoredEffects(string[] effects)
		{
			List<string> list = new List<string>(this.ignoredEffects);
			list.AddRange(effects);
			this.ignoredEffects = list.ToArray();
		}

		// Token: 0x0600796B RID: 31083 RVA: 0x002FF80C File Offset: 0x002FDA0C
		public string GetTooltip()
		{
			string text;
			if (this.TooltipCB != null)
			{
				text = this.TooltipCB();
			}
			else
			{
				text = this.description;
				text += this.GetAttributeModifiersString(true);
				text += this.GetDisabledChoresString(true);
				text += this.GetIgnoredEffectsString(true);
				text += this.GetExtendedTooltipStr();
			}
			return text;
		}

		// Token: 0x0600796C RID: 31084 RVA: 0x002FF870 File Offset: 0x002FDA70
		public string GetAttributeModifiersString(bool list_entry)
		{
			string text = "";
			foreach (AttributeModifier attributeModifier in this.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
				if (list_entry)
				{
					text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
				}
				text += string.Format(DUPLICANTS.TRAITS.ATTRIBUTE_MODIFIERS, attribute.Name, attributeModifier.GetFormattedString());
			}
			return text;
		}

		// Token: 0x0600796D RID: 31085 RVA: 0x002FF910 File Offset: 0x002FDB10
		public string GetDisabledChoresString(bool list_entry)
		{
			string text = "";
			if (this.disabledChoreGroups != null)
			{
				string format = DUPLICANTS.TRAITS.CANNOT_DO_TASK;
				if (this.isTaskBeingRefused)
				{
					format = DUPLICANTS.TRAITS.REFUSES_TO_DO_TASK;
				}
				foreach (ChoreGroup choreGroup in this.disabledChoreGroups)
				{
					if (list_entry)
					{
						text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
					}
					text += string.Format(format, choreGroup.Name);
				}
			}
			return text;
		}

		// Token: 0x0600796E RID: 31086 RVA: 0x002FF98C File Offset: 0x002FDB8C
		public string GetIgnoredEffectsString(bool list_entry)
		{
			string text = "";
			if (this.ignoredEffects != null && this.ignoredEffects.Length != 0)
			{
				for (int i = 0; i < this.ignoredEffects.Length; i++)
				{
					string text2 = this.ignoredEffects[i];
					if (list_entry)
					{
						text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
					}
					string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text2.ToUpper() + ".NAME");
					text += string.Format(DUPLICANTS.TRAITS.IGNORED_EFFECTS, arg);
					if (!list_entry && i < this.ignoredEffects.Length - 1)
					{
						text += "\n";
					}
				}
			}
			return text;
		}

		// Token: 0x0600796F RID: 31087 RVA: 0x002FFA3C File Offset: 0x002FDC3C
		public string GetExtendedTooltipStr()
		{
			string text = "";
			if (this.ExtendedTooltip != null)
			{
				foreach (Func<string> func in this.ExtendedTooltip.GetInvocationList())
				{
					text = text + "\n" + func();
				}
			}
			return text;
		}

		// Token: 0x06007970 RID: 31088 RVA: 0x002FFA90 File Offset: 0x002FDC90
		public override void AddTo(Attributes attributes)
		{
			base.AddTo(attributes);
			ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
			if (component != null && this.disabledChoreGroups != null)
			{
				foreach (ChoreGroup chore_group in this.disabledChoreGroups)
				{
					component.SetPermittedByTraits(chore_group, false);
				}
			}
		}

		// Token: 0x06007971 RID: 31089 RVA: 0x002FFAE4 File Offset: 0x002FDCE4
		public override void RemoveFrom(Attributes attributes)
		{
			base.RemoveFrom(attributes);
			ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
			if (component != null && this.disabledChoreGroups != null)
			{
				foreach (ChoreGroup chore_group in this.disabledChoreGroups)
				{
					component.SetPermittedByTraits(chore_group, true);
				}
			}
		}

		// Token: 0x04005AA0 RID: 23200
		public float Rating;

		// Token: 0x04005AA1 RID: 23201
		public bool ShouldSave;

		// Token: 0x04005AA2 RID: 23202
		public bool PositiveTrait;

		// Token: 0x04005AA3 RID: 23203
		public bool ValidStarterTrait;

		// Token: 0x04005AA4 RID: 23204
		public Action<GameObject> OnAddTrait;

		// Token: 0x04005AA5 RID: 23205
		public Func<string> TooltipCB;

		// Token: 0x04005AA6 RID: 23206
		public Func<string> ExtendedTooltip;

		// Token: 0x04005AA7 RID: 23207
		public ChoreGroup[] disabledChoreGroups;

		// Token: 0x04005AA8 RID: 23208
		public bool isTaskBeingRefused;

		// Token: 0x04005AA9 RID: 23209
		public string[] ignoredEffects;
	}
}
