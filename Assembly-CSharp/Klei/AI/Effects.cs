using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F5D RID: 3933
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Effects")]
	public class Effects : KMonoBehaviour, ISaveLoadable, ISim1000ms
	{
		// Token: 0x060078D2 RID: 30930 RVA: 0x002FD093 File Offset: 0x002FB293
		protected override void OnPrefabInit()
		{
			this.autoRegisterSimRender = false;
		}

		// Token: 0x060078D3 RID: 30931 RVA: 0x002FD09C File Offset: 0x002FB29C
		protected override void OnSpawn()
		{
			if (this.saveLoadImmunities != null)
			{
				foreach (Effects.SaveLoadImmunities saveLoadImmunities in this.saveLoadImmunities)
				{
					if (Db.Get().effects.Exists(saveLoadImmunities.effectID))
					{
						Effect effect = Db.Get().effects.Get(saveLoadImmunities.effectID);
						this.AddImmunity(effect, saveLoadImmunities.giverID, true);
					}
				}
			}
			if (this.saveLoadEffects != null)
			{
				foreach (Effects.SaveLoadEffect saveLoadEffect in this.saveLoadEffects)
				{
					if (Db.Get().effects.Exists(saveLoadEffect.id))
					{
						Effect newEffect = Db.Get().effects.Get(saveLoadEffect.id);
						EffectInstance effectInstance = this.Add(newEffect, true);
						if (effectInstance != null)
						{
							effectInstance.timeRemaining = saveLoadEffect.timeRemaining;
						}
					}
				}
			}
			if (this.effectsThatExpire.Count > 0)
			{
				SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
			}
		}

		// Token: 0x060078D4 RID: 30932 RVA: 0x002FD1A0 File Offset: 0x002FB3A0
		public EffectInstance Get(string effect_id)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect.Id == effect_id)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x060078D5 RID: 30933 RVA: 0x002FD208 File Offset: 0x002FB408
		public EffectInstance Get(HashedString effect_id)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect.IdHash == effect_id)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x060078D6 RID: 30934 RVA: 0x002FD270 File Offset: 0x002FB470
		public EffectInstance Get(Effect effect)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect == effect)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x060078D7 RID: 30935 RVA: 0x002FD2CC File Offset: 0x002FB4CC
		public bool HasImmunityTo(Effect effect)
		{
			using (List<Effects.EffectImmunity>.Enumerator enumerator = this.effectImmunites.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect == effect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060078D8 RID: 30936 RVA: 0x002FD328 File Offset: 0x002FB528
		public EffectInstance Add(string effect_id, bool should_save)
		{
			Effect newEffect = Db.Get().effects.Get(effect_id);
			return this.Add(newEffect, should_save);
		}

		// Token: 0x060078D9 RID: 30937 RVA: 0x002FD350 File Offset: 0x002FB550
		public EffectInstance Add(HashedString effect_id, bool should_save)
		{
			Effect newEffect = Db.Get().effects.Get(effect_id);
			return this.Add(newEffect, should_save);
		}

		// Token: 0x060078DA RID: 30938 RVA: 0x002FD378 File Offset: 0x002FB578
		public EffectInstance Add(Effect newEffect, bool should_save)
		{
			if (this.HasImmunityTo(newEffect))
			{
				return null;
			}
			Traits component = base.GetComponent<Traits>();
			if (component != null && component.IsEffectIgnored(newEffect))
			{
				return null;
			}
			Attributes attributes = this.GetAttributes();
			EffectInstance effectInstance = this.Get(newEffect);
			if (!string.IsNullOrEmpty(newEffect.stompGroup))
			{
				for (int i = this.effects.Count - 1; i >= 0; i--)
				{
					if (this.effects[i] != effectInstance && !(this.effects[i].effect.stompGroup != newEffect.stompGroup) && this.effects[i].effect.stompPriority > newEffect.stompPriority)
					{
						return null;
					}
				}
				for (int j = this.effects.Count - 1; j >= 0; j--)
				{
					if (this.effects[j] != effectInstance && !(this.effects[j].effect.stompGroup != newEffect.stompGroup) && this.effects[j].effect.stompPriority <= newEffect.stompPriority)
					{
						this.Remove(this.effects[j].effect);
					}
				}
			}
			if (effectInstance == null)
			{
				effectInstance = new EffectInstance(base.gameObject, newEffect, should_save);
				newEffect.AddTo(attributes);
				this.effects.Add(effectInstance);
				if (newEffect.duration > 0f)
				{
					this.effectsThatExpire.Add(effectInstance);
					if (this.effectsThatExpire.Count == 1)
					{
						SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
					}
				}
				base.Trigger(-1901442097, newEffect);
			}
			effectInstance.timeRemaining = newEffect.duration;
			return effectInstance;
		}

		// Token: 0x060078DB RID: 30939 RVA: 0x002FD530 File Offset: 0x002FB730
		public void Remove(Effect effect)
		{
			this.Remove(effect.IdHash);
		}

		// Token: 0x060078DC RID: 30940 RVA: 0x002FD540 File Offset: 0x002FB740
		public void Remove(HashedString effect_id)
		{
			int i = 0;
			while (i < this.effectsThatExpire.Count)
			{
				if (this.effectsThatExpire[i].effect.IdHash == effect_id)
				{
					int index = this.effectsThatExpire.Count - 1;
					this.effectsThatExpire[i] = this.effectsThatExpire[index];
					this.effectsThatExpire.RemoveAt(index);
					if (this.effectsThatExpire.Count == 0)
					{
						SimAndRenderScheduler.instance.Remove(this);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < this.effects.Count; j++)
			{
				if (this.effects[j].effect.IdHash == effect_id)
				{
					Attributes attributes = this.GetAttributes();
					EffectInstance effectInstance = this.effects[j];
					effectInstance.OnCleanUp();
					Effect effect = effectInstance.effect;
					effect.RemoveFrom(attributes);
					int index2 = this.effects.Count - 1;
					this.effects[j] = this.effects[index2];
					this.effects.RemoveAt(index2);
					base.Trigger(-1157678353, effect);
					return;
				}
			}
		}

		// Token: 0x060078DD RID: 30941 RVA: 0x002FD674 File Offset: 0x002FB874
		public void Remove(string effect_id)
		{
			int i = 0;
			while (i < this.effectsThatExpire.Count)
			{
				if (this.effectsThatExpire[i].effect.Id == effect_id)
				{
					int index = this.effectsThatExpire.Count - 1;
					this.effectsThatExpire[i] = this.effectsThatExpire[index];
					this.effectsThatExpire.RemoveAt(index);
					if (this.effectsThatExpire.Count == 0)
					{
						SimAndRenderScheduler.instance.Remove(this);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < this.effects.Count; j++)
			{
				if (this.effects[j].effect.Id == effect_id)
				{
					Attributes attributes = this.GetAttributes();
					EffectInstance effectInstance = this.effects[j];
					effectInstance.OnCleanUp();
					Effect effect = effectInstance.effect;
					effect.RemoveFrom(attributes);
					int index2 = this.effects.Count - 1;
					this.effects[j] = this.effects[index2];
					this.effects.RemoveAt(index2);
					base.Trigger(-1157678353, effect);
					return;
				}
			}
		}

		// Token: 0x060078DE RID: 30942 RVA: 0x002FD7A8 File Offset: 0x002FB9A8
		public bool HasEffect(HashedString effect_id)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect.IdHash == effect_id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060078DF RID: 30943 RVA: 0x002FD80C File Offset: 0x002FBA0C
		public bool HasEffect(string effect_id)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect.Id == effect_id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060078E0 RID: 30944 RVA: 0x002FD870 File Offset: 0x002FBA70
		public bool HasEffect(Effect effect)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect == effect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060078E1 RID: 30945 RVA: 0x002FD8CC File Offset: 0x002FBACC
		public void Sim1000ms(float dt)
		{
			for (int i = 0; i < this.effectsThatExpire.Count; i++)
			{
				EffectInstance effectInstance = this.effectsThatExpire[i];
				if (effectInstance.IsExpired())
				{
					this.Remove(effectInstance.effect);
				}
				effectInstance.timeRemaining -= dt;
			}
		}

		// Token: 0x060078E2 RID: 30946 RVA: 0x002FD920 File Offset: 0x002FBB20
		public void AddImmunity(Effect effect, string giverID, bool shouldSave = true)
		{
			if (giverID != null)
			{
				foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
				{
					if (effectImmunity.giverID == giverID && effectImmunity.effect == effect)
					{
						return;
					}
				}
			}
			Effects.EffectImmunity effectImmunity2 = new Effects.EffectImmunity(effect, giverID, shouldSave);
			this.effectImmunites.Add(effectImmunity2);
			base.Trigger(1152870979, effectImmunity2);
		}

		// Token: 0x060078E3 RID: 30947 RVA: 0x002FD9B0 File Offset: 0x002FBBB0
		public void RemoveImmunity(Effect effect, string ID)
		{
			Effects.EffectImmunity effectImmunity = default(Effects.EffectImmunity);
			bool flag = false;
			foreach (Effects.EffectImmunity effectImmunity2 in this.effectImmunites)
			{
				if (effectImmunity2.effect == effect && (ID == null || ID == effectImmunity2.giverID))
				{
					effectImmunity = effectImmunity2;
					flag = true;
				}
			}
			if (flag)
			{
				this.effectImmunites.Remove(effectImmunity);
				base.Trigger(964452195, effectImmunity);
			}
		}

		// Token: 0x060078E4 RID: 30948 RVA: 0x002FDA48 File Offset: 0x002FBC48
		[OnSerializing]
		internal void OnSerializing()
		{
			List<Effects.SaveLoadEffect> list = new List<Effects.SaveLoadEffect>();
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.shouldSave)
				{
					Effects.SaveLoadEffect item = new Effects.SaveLoadEffect
					{
						id = effectInstance.effect.Id,
						timeRemaining = effectInstance.timeRemaining,
						saved = true
					};
					list.Add(item);
				}
			}
			this.saveLoadEffects = list.ToArray();
			List<Effects.SaveLoadImmunities> list2 = new List<Effects.SaveLoadImmunities>();
			foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
			{
				if (effectImmunity.shouldSave)
				{
					Effect effect = effectImmunity.effect;
					Effects.SaveLoadImmunities item2 = new Effects.SaveLoadImmunities
					{
						effectID = effect.Id,
						giverID = effectImmunity.giverID,
						saved = true
					};
					list2.Add(item2);
				}
			}
			this.saveLoadImmunities = list2.ToArray();
		}

		// Token: 0x060078E5 RID: 30949 RVA: 0x002FDB84 File Offset: 0x002FBD84
		public List<Effects.SaveLoadImmunities> GetAllImmunitiesForSerialization()
		{
			List<Effects.SaveLoadImmunities> list = new List<Effects.SaveLoadImmunities>();
			foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
			{
				Effect effect = effectImmunity.effect;
				Effects.SaveLoadImmunities item = new Effects.SaveLoadImmunities
				{
					effectID = effect.Id,
					giverID = effectImmunity.giverID,
					saved = effectImmunity.shouldSave
				};
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060078E6 RID: 30950 RVA: 0x002FDC1C File Offset: 0x002FBE1C
		public List<Effects.SaveLoadEffect> GetAllEffectsForSerialization()
		{
			List<Effects.SaveLoadEffect> list = new List<Effects.SaveLoadEffect>();
			foreach (EffectInstance effectInstance in this.effects)
			{
				Effects.SaveLoadEffect item = new Effects.SaveLoadEffect
				{
					id = effectInstance.effect.Id,
					timeRemaining = effectInstance.timeRemaining,
					saved = effectInstance.shouldSave
				};
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060078E7 RID: 30951 RVA: 0x002FDCB0 File Offset: 0x002FBEB0
		public List<EffectInstance> GetTimeLimitedEffects()
		{
			return this.effectsThatExpire;
		}

		// Token: 0x060078E8 RID: 30952 RVA: 0x002FDCB8 File Offset: 0x002FBEB8
		public void CopyEffects(Effects source)
		{
			foreach (EffectInstance effectInstance in source.effects)
			{
				this.Add(effectInstance.effect, effectInstance.shouldSave).timeRemaining = effectInstance.timeRemaining;
			}
			foreach (EffectInstance effectInstance2 in source.effectsThatExpire)
			{
				this.Add(effectInstance2.effect, effectInstance2.shouldSave).timeRemaining = effectInstance2.timeRemaining;
			}
		}

		// Token: 0x04005A43 RID: 23107
		[Serialize]
		private Effects.SaveLoadEffect[] saveLoadEffects;

		// Token: 0x04005A44 RID: 23108
		[Serialize]
		private Effects.SaveLoadImmunities[] saveLoadImmunities;

		// Token: 0x04005A45 RID: 23109
		private List<EffectInstance> effects = new List<EffectInstance>();

		// Token: 0x04005A46 RID: 23110
		private List<EffectInstance> effectsThatExpire = new List<EffectInstance>();

		// Token: 0x04005A47 RID: 23111
		private List<Effects.EffectImmunity> effectImmunites = new List<Effects.EffectImmunity>();

		// Token: 0x02002343 RID: 9027
		[Serializable]
		public struct EffectImmunity
		{
			// Token: 0x0600B610 RID: 46608 RVA: 0x003C92F7 File Offset: 0x003C74F7
			public EffectImmunity(Effect e, string id, bool save = true)
			{
				this.giverID = id;
				this.effect = e;
				this.shouldSave = save;
			}

			// Token: 0x04009E2E RID: 40494
			public string giverID;

			// Token: 0x04009E2F RID: 40495
			public Effect effect;

			// Token: 0x04009E30 RID: 40496
			public bool shouldSave;
		}

		// Token: 0x02002344 RID: 9028
		[Serializable]
		public struct SaveLoadImmunities
		{
			// Token: 0x04009E31 RID: 40497
			public string giverID;

			// Token: 0x04009E32 RID: 40498
			public string effectID;

			// Token: 0x04009E33 RID: 40499
			public bool saved;
		}

		// Token: 0x02002345 RID: 9029
		[Serializable]
		public struct SaveLoadEffect
		{
			// Token: 0x04009E34 RID: 40500
			public string id;

			// Token: 0x04009E35 RID: 40501
			public float timeRemaining;

			// Token: 0x04009E36 RID: 40502
			public bool saved;
		}
	}
}
