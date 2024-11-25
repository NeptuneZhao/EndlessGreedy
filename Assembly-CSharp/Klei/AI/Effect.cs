using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F5B RID: 3931
	[DebuggerDisplay("{Id}")]
	public class Effect : Modifier
	{
		// Token: 0x060078B9 RID: 30905 RVA: 0x002FC6D0 File Offset: 0x002FA8D0
		public Effect(string id, string name, string description, float duration, bool show_in_ui, bool trigger_floating_text, bool is_bad, Emote emote = null, float emote_cooldown = -1f, float max_initial_delay = 0f, string stompGroup = null, string custom_icon = "") : this(id, name, description, duration, show_in_ui, trigger_floating_text, is_bad, emote, max_initial_delay, stompGroup, false, custom_icon, emote_cooldown)
		{
		}

		// Token: 0x060078BA RID: 30906 RVA: 0x002FC6FC File Offset: 0x002FA8FC
		public Effect(string id, string name, string description, float duration, bool show_in_ui, bool trigger_floating_text, bool is_bad, Emote emote, float max_initial_delay, string stompGroup, bool showStatusInWorld, string custom_icon = "", float emote_cooldown = -1f) : this(id, name, description, duration, null, show_in_ui, trigger_floating_text, is_bad, emote, max_initial_delay, stompGroup, showStatusInWorld, custom_icon, emote_cooldown)
		{
		}

		// Token: 0x060078BB RID: 30907 RVA: 0x002FC728 File Offset: 0x002FA928
		public Effect(string id, string name, string description, float duration, string[] immunityEffects, bool show_in_ui, bool trigger_floating_text, bool is_bad, Emote emote, float max_initial_delay, string stompGroup, bool showStatusInWorld, string custom_icon = "", float emote_cooldown = -1f) : base(id, name, description)
		{
			this.duration = duration;
			this.showInUI = show_in_ui;
			this.triggerFloatingText = trigger_floating_text;
			this.isBad = is_bad;
			this.emote = emote;
			this.emoteCooldown = emote_cooldown;
			this.maxInitialDelay = max_initial_delay;
			this.stompGroup = stompGroup;
			this.customIcon = custom_icon;
			this.showStatusInWorld = showStatusInWorld;
			this.immunityEffectsNames = immunityEffects;
		}

		// Token: 0x060078BC RID: 30908 RVA: 0x002FC798 File Offset: 0x002FA998
		public Effect(string id, string name, string description, float duration, bool show_in_ui, bool trigger_floating_text, bool is_bad, string emoteAnim, float emote_cooldown = -1f, string stompGroup = null, string custom_icon = "") : base(id, name, description)
		{
			this.duration = duration;
			this.showInUI = show_in_ui;
			this.triggerFloatingText = trigger_floating_text;
			this.isBad = is_bad;
			this.emoteAnim = emoteAnim;
			this.emoteCooldown = emote_cooldown;
			this.stompGroup = stompGroup;
			this.customIcon = custom_icon;
		}

		// Token: 0x060078BD RID: 30909 RVA: 0x002FC7EE File Offset: 0x002FA9EE
		public override void AddTo(Attributes attributes)
		{
			base.AddTo(attributes);
		}

		// Token: 0x060078BE RID: 30910 RVA: 0x002FC7F7 File Offset: 0x002FA9F7
		public override void RemoveFrom(Attributes attributes)
		{
			base.RemoveFrom(attributes);
		}

		// Token: 0x060078BF RID: 30911 RVA: 0x002FC800 File Offset: 0x002FAA00
		public void SetEmote(Emote emote, float emoteCooldown = -1f)
		{
			this.emote = emote;
			this.emoteCooldown = emoteCooldown;
		}

		// Token: 0x060078C0 RID: 30912 RVA: 0x002FC810 File Offset: 0x002FAA10
		public void AddEmotePrecondition(Reactable.ReactablePrecondition precon)
		{
			if (this.emotePreconditions == null)
			{
				this.emotePreconditions = new List<Reactable.ReactablePrecondition>();
			}
			this.emotePreconditions.Add(precon);
		}

		// Token: 0x060078C1 RID: 30913 RVA: 0x002FC834 File Offset: 0x002FAA34
		public static string CreateTooltip(Effect effect, bool showDuration, string linePrefix = "\n    • ", bool showHeader = true)
		{
			StringEntry stringEntry;
			Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".ADDITIONAL_EFFECTS", out stringEntry);
			string text = (showHeader && (effect.SelfModifiers.Count > 0 || stringEntry != null)) ? DUPLICANTS.MODIFIERS.EFFECT_HEADER.text : "";
			foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.TryGet(attributeModifier.AttributeId);
				if (attribute == null)
				{
					attribute = Db.Get().CritterAttributes.TryGet(attributeModifier.AttributeId);
				}
				if (attribute != null && attribute.ShowInUI != Attribute.Display.Never)
				{
					text = text + linePrefix + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, attribute.Name, attributeModifier.GetFormattedString());
				}
			}
			if (effect.immunityEffectsNames != null)
			{
				text += (string.IsNullOrEmpty(text) ? "" : (linePrefix + linePrefix));
				text += ((showHeader && effect.immunityEffectsNames != null && effect.immunityEffectsNames.Length != 0) ? DUPLICANTS.MODIFIERS.EFFECT_IMMUNITIES_HEADER.text : "");
				foreach (string id in effect.immunityEffectsNames)
				{
					Effect effect2 = Db.Get().effects.TryGet(id);
					if (effect2 != null)
					{
						text = text + linePrefix + string.Format(DUPLICANTS.MODIFIERS.IMMUNITY_FORMAT, effect2.Name);
					}
				}
			}
			if (stringEntry != null)
			{
				text = text + linePrefix + stringEntry;
			}
			if (showDuration && effect.duration > 0f)
			{
				text = text + "\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_TOTAL, GameUtil.GetFormattedCycles(effect.duration, "F1", false));
			}
			return text;
		}

		// Token: 0x060078C2 RID: 30914 RVA: 0x002FCA28 File Offset: 0x002FAC28
		public static string CreateFullTooltip(Effect effect, bool showDuration)
		{
			return string.Concat(new string[]
			{
				effect.Name,
				"\n\n",
				effect.description,
				"\n\n",
				Effect.CreateTooltip(effect, showDuration, "\n    • ", true)
			});
		}

		// Token: 0x060078C3 RID: 30915 RVA: 0x002FCA67 File Offset: 0x002FAC67
		public static void AddModifierDescriptions(GameObject parent, List<Descriptor> descs, string effect_id, bool increase_indent = false)
		{
			Effect.AddModifierDescriptions(descs, effect_id, increase_indent, "STRINGS.DUPLICANTS.ATTRIBUTES.");
		}

		// Token: 0x060078C4 RID: 30916 RVA: 0x002FCA78 File Offset: 0x002FAC78
		public static void AddModifierDescriptions(List<Descriptor> descs, string effect_id, bool increase_indent = false, string prefix = "STRINGS.DUPLICANTS.ATTRIBUTES.")
		{
			foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(effect_id).SelfModifiers)
			{
				Descriptor item = new Descriptor(Strings.Get(prefix + attributeModifier.AttributeId.ToUpper() + ".NAME") + ": " + attributeModifier.GetFormattedString(), "", Descriptor.DescriptorType.Effect, false);
				if (increase_indent)
				{
					item.IncreaseIndent();
				}
				descs.Add(item);
			}
		}

		// Token: 0x04005A2F RID: 23087
		public float duration;

		// Token: 0x04005A30 RID: 23088
		public bool showInUI;

		// Token: 0x04005A31 RID: 23089
		public bool triggerFloatingText;

		// Token: 0x04005A32 RID: 23090
		public bool isBad;

		// Token: 0x04005A33 RID: 23091
		public bool showStatusInWorld;

		// Token: 0x04005A34 RID: 23092
		public string customIcon;

		// Token: 0x04005A35 RID: 23093
		public string[] immunityEffectsNames;

		// Token: 0x04005A36 RID: 23094
		public string emoteAnim;

		// Token: 0x04005A37 RID: 23095
		public Emote emote;

		// Token: 0x04005A38 RID: 23096
		public float emoteCooldown;

		// Token: 0x04005A39 RID: 23097
		public float maxInitialDelay;

		// Token: 0x04005A3A RID: 23098
		public List<Reactable.ReactablePrecondition> emotePreconditions;

		// Token: 0x04005A3B RID: 23099
		public string stompGroup;

		// Token: 0x04005A3C RID: 23100
		public int stompPriority;
	}
}
