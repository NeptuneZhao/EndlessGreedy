using System;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F5C RID: 3932
	[DebuggerDisplay("{effect.Id}")]
	public class EffectInstance : ModifierInstance<Effect>
	{
		// Token: 0x060078C5 RID: 30917 RVA: 0x002FCB24 File Offset: 0x002FAD24
		public EffectInstance(GameObject game_object, Effect effect, bool should_save) : base(game_object, effect)
		{
			this.effect = effect;
			this.shouldSave = should_save;
			this.DefineEffectImmunities();
			this.ApplyImmunities();
			this.ConfigureStatusItem();
			if (effect.showInUI)
			{
				KSelectable component = base.gameObject.GetComponent<KSelectable>();
				if (!component.GetStatusItemGroup().HasStatusItem(this.statusItem))
				{
					component.AddStatusItem(this.statusItem, this);
				}
			}
			if (effect.triggerFloatingText && PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, effect.Name, game_object.transform, 1.5f, false);
			}
			if (effect.emote != null)
			{
				this.RegisterEmote(effect.emote, effect.emoteCooldown);
			}
			if (!string.IsNullOrEmpty(effect.emoteAnim))
			{
				this.RegisterEmote(effect.emoteAnim, effect.emoteCooldown);
			}
		}

		// Token: 0x060078C6 RID: 30918 RVA: 0x002FCC04 File Offset: 0x002FAE04
		protected void DefineEffectImmunities()
		{
			if (this.immunityEffects == null && this.effect.immunityEffectsNames != null)
			{
				this.immunityEffects = new Effect[this.effect.immunityEffectsNames.Length];
				for (int i = 0; i < this.immunityEffects.Length; i++)
				{
					this.immunityEffects[i] = Db.Get().effects.Get(this.effect.immunityEffectsNames[i]);
				}
			}
		}

		// Token: 0x060078C7 RID: 30919 RVA: 0x002FCC78 File Offset: 0x002FAE78
		protected void ApplyImmunities()
		{
			if (base.gameObject != null && this.immunityEffects != null)
			{
				Effects component = base.gameObject.GetComponent<Effects>();
				for (int i = 0; i < this.immunityEffects.Length; i++)
				{
					component.Remove(this.immunityEffects[i]);
					component.AddImmunity(this.immunityEffects[i], this.effect.IdHash.ToString(), false);
				}
			}
		}

		// Token: 0x060078C8 RID: 30920 RVA: 0x002FCCF0 File Offset: 0x002FAEF0
		protected void RemoveImmunities()
		{
			if (base.gameObject != null && this.immunityEffects != null)
			{
				Effects component = base.gameObject.GetComponent<Effects>();
				for (int i = 0; i < this.immunityEffects.Length; i++)
				{
					component.RemoveImmunity(this.immunityEffects[i], this.effect.IdHash.ToString());
				}
			}
		}

		// Token: 0x060078C9 RID: 30921 RVA: 0x002FCD58 File Offset: 0x002FAF58
		public void RegisterEmote(string emoteAnim, float cooldown = -1f)
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi == null)
			{
				return;
			}
			bool flag = cooldown < 0f;
			float globalCooldown = flag ? 100000f : cooldown;
			EmoteReactable emoteReactable = smi.AddSelfEmoteReactable(base.gameObject, this.effect.Name + "_Emote", emoteAnim, flag, Db.Get().ChoreTypes.Emote, globalCooldown, 20f, float.NegativeInfinity, this.effect.maxInitialDelay, this.effect.emotePreconditions);
			if (emoteReactable == null)
			{
				return;
			}
			emoteReactable.InsertPrecondition(0, new Reactable.ReactablePrecondition(this.NotInATube));
			if (!flag)
			{
				this.reactable = emoteReactable;
			}
		}

		// Token: 0x060078CA RID: 30922 RVA: 0x002FCE00 File Offset: 0x002FB000
		public void RegisterEmote(Emote emote, float cooldown = -1f)
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi == null)
			{
				return;
			}
			bool flag = cooldown < 0f;
			float globalCooldown = flag ? 100000f : cooldown;
			EmoteReactable emoteReactable = smi.AddSelfEmoteReactable(base.gameObject, this.effect.Name + "_Emote", emote, flag, Db.Get().ChoreTypes.Emote, globalCooldown, 20f, float.NegativeInfinity, this.effect.maxInitialDelay, this.effect.emotePreconditions);
			if (emoteReactable == null)
			{
				return;
			}
			emoteReactable.InsertPrecondition(0, new Reactable.ReactablePrecondition(this.NotInATube));
			if (!flag)
			{
				this.reactable = emoteReactable;
			}
		}

		// Token: 0x060078CB RID: 30923 RVA: 0x002FCEAC File Offset: 0x002FB0AC
		private bool NotInATube(GameObject go, Navigator.ActiveTransition transition)
		{
			return transition.navGridTransition.start != NavType.Tube && transition.navGridTransition.end != NavType.Tube;
		}

		// Token: 0x060078CC RID: 30924 RVA: 0x002FCED0 File Offset: 0x002FB0D0
		public override void OnCleanUp()
		{
			if (this.statusItem != null)
			{
				base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.statusItem, false);
				this.statusItem = null;
			}
			if (this.reactable != null)
			{
				this.reactable.Cleanup();
				this.reactable = null;
			}
			this.RemoveImmunities();
		}

		// Token: 0x060078CD RID: 30925 RVA: 0x002FCF24 File Offset: 0x002FB124
		public float GetTimeRemaining()
		{
			return this.timeRemaining;
		}

		// Token: 0x060078CE RID: 30926 RVA: 0x002FCF2C File Offset: 0x002FB12C
		public bool IsExpired()
		{
			return this.effect.duration > 0f && this.timeRemaining <= 0f;
		}

		// Token: 0x060078CF RID: 30927 RVA: 0x002FCF54 File Offset: 0x002FB154
		private void ConfigureStatusItem()
		{
			StatusItem.IconType iconType = this.effect.isBad ? StatusItem.IconType.Exclamation : StatusItem.IconType.Info;
			if (!this.effect.customIcon.IsNullOrWhiteSpace())
			{
				iconType = StatusItem.IconType.Custom;
			}
			string id = this.effect.Id;
			string name = this.effect.Name;
			string description = this.effect.description;
			string customIcon = this.effect.customIcon;
			StatusItem.IconType icon_type = iconType;
			NotificationType notification_type = this.effect.isBad ? NotificationType.Bad : NotificationType.Neutral;
			bool allow_multiples = false;
			bool showStatusInWorld = this.effect.showStatusInWorld;
			this.statusItem = new StatusItem(id, name, description, customIcon, icon_type, notification_type, allow_multiples, OverlayModes.None.ID, 2, showStatusInWorld, null);
			this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveString);
			this.statusItem.resolveTooltipCallback = new Func<string, object, string>(this.ResolveTooltip);
		}

		// Token: 0x060078D0 RID: 30928 RVA: 0x002FD013 File Offset: 0x002FB213
		private string ResolveString(string str, object data)
		{
			return str;
		}

		// Token: 0x060078D1 RID: 30929 RVA: 0x002FD018 File Offset: 0x002FB218
		private string ResolveTooltip(string str, object data)
		{
			string text = str;
			EffectInstance effectInstance = (EffectInstance)data;
			string text2 = Effect.CreateTooltip(effectInstance.effect, false, "\n    • ", true);
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + "\n\n" + text2;
			}
			if (effectInstance.effect.duration > 0f)
			{
				text = text + "\n\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_REMAINING, GameUtil.GetFormattedCycles(this.GetTimeRemaining(), "F1", false));
			}
			return text;
		}

		// Token: 0x04005A3D RID: 23101
		public Effect effect;

		// Token: 0x04005A3E RID: 23102
		public bool shouldSave;

		// Token: 0x04005A3F RID: 23103
		public StatusItem statusItem;

		// Token: 0x04005A40 RID: 23104
		public float timeRemaining;

		// Token: 0x04005A41 RID: 23105
		public EmoteReactable reactable;

		// Token: 0x04005A42 RID: 23106
		protected Effect[] immunityEffects;
	}
}
