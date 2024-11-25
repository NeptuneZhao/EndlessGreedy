using System;
using System.Collections.Generic;
using KSerialization;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F79 RID: 3961
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Traits")]
	public class Traits : KMonoBehaviour, ISaveLoadable
	{
		// Token: 0x0600797F RID: 31103 RVA: 0x002FFE22 File Offset: 0x002FE022
		public List<string> GetTraitIds()
		{
			return this.TraitIds;
		}

		// Token: 0x06007980 RID: 31104 RVA: 0x002FFE2A File Offset: 0x002FE02A
		public void SetTraitIds(List<string> traits)
		{
			this.TraitIds = traits;
		}

		// Token: 0x06007981 RID: 31105 RVA: 0x002FFE34 File Offset: 0x002FE034
		protected override void OnSpawn()
		{
			foreach (string id in this.TraitIds)
			{
				if (Db.Get().traits.Exists(id))
				{
					Trait trait = Db.Get().traits.Get(id);
					this.AddInternal(trait);
				}
			}
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 15))
			{
				List<DUPLICANTSTATS.TraitVal> joytraits = DUPLICANTSTATS.JOYTRAITS;
				if (base.GetComponent<MinionIdentity>())
				{
					bool flag = true;
					foreach (DUPLICANTSTATS.TraitVal traitVal in joytraits)
					{
						if (this.HasTrait(traitVal.id))
						{
							flag = false;
						}
					}
					if (flag)
					{
						DUPLICANTSTATS.TraitVal random = joytraits.GetRandom<DUPLICANTSTATS.TraitVal>();
						Trait trait2 = Db.Get().traits.Get(random.id);
						this.Add(trait2);
					}
				}
			}
		}

		// Token: 0x06007982 RID: 31106 RVA: 0x002FFF54 File Offset: 0x002FE154
		private void AddInternal(Trait trait)
		{
			if (!this.HasTrait(trait))
			{
				this.TraitList.Add(trait);
				trait.AddTo(this.GetAttributes());
				if (trait.OnAddTrait != null)
				{
					trait.OnAddTrait(base.gameObject);
				}
			}
		}

		// Token: 0x06007983 RID: 31107 RVA: 0x002FFF90 File Offset: 0x002FE190
		public void Add(Trait trait)
		{
			DebugUtil.Assert(base.IsInitialized() || base.GetComponent<Modifiers>().IsInitialized(), "Tried adding a trait on a prefab, use Modifiers.initialTraits instead!", trait.Name, base.gameObject.name);
			if (trait.ShouldSave)
			{
				this.TraitIds.Add(trait.Id);
			}
			this.AddInternal(trait);
		}

		// Token: 0x06007984 RID: 31108 RVA: 0x002FFFF0 File Offset: 0x002FE1F0
		public bool HasTrait(string trait_id)
		{
			bool result = false;
			using (List<Trait>.Enumerator enumerator = this.TraitList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == trait_id)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06007985 RID: 31109 RVA: 0x00300050 File Offset: 0x002FE250
		public bool HasTrait(Trait trait)
		{
			using (List<Trait>.Enumerator enumerator = this.TraitList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == trait)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007986 RID: 31110 RVA: 0x003000A8 File Offset: 0x002FE2A8
		public void Clear()
		{
			while (this.TraitList.Count > 0)
			{
				this.Remove(this.TraitList[0]);
			}
		}

		// Token: 0x06007987 RID: 31111 RVA: 0x003000CC File Offset: 0x002FE2CC
		public void Remove(Trait trait)
		{
			for (int i = 0; i < this.TraitList.Count; i++)
			{
				if (this.TraitList[i] == trait)
				{
					this.TraitList.RemoveAt(i);
					this.TraitIds.Remove(trait.Id);
					trait.RemoveFrom(this.GetAttributes());
					return;
				}
			}
		}

		// Token: 0x06007988 RID: 31112 RVA: 0x0030012C File Offset: 0x002FE32C
		public bool IsEffectIgnored(Effect effect)
		{
			foreach (Trait trait in this.TraitList)
			{
				if (trait.ignoredEffects != null && Array.IndexOf<string>(trait.ignoredEffects, effect.Id) != -1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007989 RID: 31113 RVA: 0x0030019C File Offset: 0x002FE39C
		public bool IsChoreGroupDisabled(ChoreGroup choreGroup)
		{
			Trait trait;
			return this.IsChoreGroupDisabled(choreGroup, out trait);
		}

		// Token: 0x0600798A RID: 31114 RVA: 0x003001B2 File Offset: 0x002FE3B2
		public bool IsChoreGroupDisabled(ChoreGroup choreGroup, out Trait disablingTrait)
		{
			return this.IsChoreGroupDisabled(choreGroup.IdHash, out disablingTrait);
		}

		// Token: 0x0600798B RID: 31115 RVA: 0x003001C4 File Offset: 0x002FE3C4
		public bool IsChoreGroupDisabled(HashedString choreGroupId)
		{
			Trait trait;
			return this.IsChoreGroupDisabled(choreGroupId, out trait);
		}

		// Token: 0x0600798C RID: 31116 RVA: 0x003001DC File Offset: 0x002FE3DC
		public bool IsChoreGroupDisabled(HashedString choreGroupId, out Trait disablingTrait)
		{
			foreach (Trait trait in this.TraitList)
			{
				if (trait.disabledChoreGroups != null)
				{
					ChoreGroup[] disabledChoreGroups = trait.disabledChoreGroups;
					for (int i = 0; i < disabledChoreGroups.Length; i++)
					{
						if (disabledChoreGroups[i].IdHash == choreGroupId)
						{
							disablingTrait = trait;
							return true;
						}
					}
				}
			}
			disablingTrait = null;
			return false;
		}

		// Token: 0x04005AAB RID: 23211
		public List<Trait> TraitList = new List<Trait>();

		// Token: 0x04005AAC RID: 23212
		[Serialize]
		private List<string> TraitIds = new List<string>();
	}
}
