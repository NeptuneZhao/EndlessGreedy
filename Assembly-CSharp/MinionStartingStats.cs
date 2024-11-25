﻿using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000965 RID: 2405
public class MinionStartingStats : ITelepadDeliverable
{
	// Token: 0x06004655 RID: 18005 RVA: 0x00190FB0 File Offset: 0x0018F1B0
	public MinionStartingStats(Personality personality, string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false)
	{
		this.personality = personality;
		this.GenerateStats(guaranteedAptitudeID, guaranteedTraitID, isDebugMinion, false);
	}

	// Token: 0x06004656 RID: 18006 RVA: 0x00191004 File Offset: 0x0018F204
	public MinionStartingStats(bool is_starter_minion, string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false)
	{
		this.personality = Db.Get().Personalities.GetRandom(true, is_starter_minion);
		this.GenerateStats(guaranteedAptitudeID, guaranteedTraitID, isDebugMinion, is_starter_minion);
	}

	// Token: 0x06004657 RID: 18007 RVA: 0x00191068 File Offset: 0x0018F268
	public MinionStartingStats(Tag model, bool is_starter_minion, string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false)
	{
		this.personality = Db.Get().Personalities.GetRandom(model, true, is_starter_minion);
		this.GenerateStats(guaranteedAptitudeID, guaranteedTraitID, isDebugMinion, is_starter_minion);
	}

	// Token: 0x06004658 RID: 18008 RVA: 0x001910CC File Offset: 0x0018F2CC
	public MinionStartingStats(List<Tag> models, bool is_starter_minion, string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false)
	{
		this.personality = Db.Get().Personalities.GetRandom(models, true, is_starter_minion);
		this.GenerateStats(guaranteedAptitudeID, guaranteedTraitID, isDebugMinion, is_starter_minion);
	}

	// Token: 0x06004659 RID: 18009 RVA: 0x00191130 File Offset: 0x0018F330
	private void GenerateStats(string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false, bool is_starter_minion = false)
	{
		this.voiceIdx = UnityEngine.Random.Range(0, 4);
		this.Name = this.personality.Name;
		this.NameStringKey = this.personality.nameStringKey;
		this.GenderStringKey = this.personality.genderStringKey;
		this.Traits.Add(Db.Get().traits.Get(BaseMinionConfig.GetMinionBaseTraitIDForModel(this.personality.model)));
		List<ChoreGroup> disabled_chore_groups = new List<ChoreGroup>();
		this.GenerateAptitudes(guaranteedAptitudeID);
		int pointsDelta = this.GenerateTraits(is_starter_minion, disabled_chore_groups, guaranteedAptitudeID, guaranteedTraitID, isDebugMinion);
		this.GenerateAttributes(pointsDelta, disabled_chore_groups);
		KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(this.personality);
		foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
		{
			if (accessorySlot.accessories.Count != 0)
			{
				Accessory accessory = null;
				if (accessorySlot == Db.Get().AccessorySlots.HeadShape)
				{
					accessory = accessorySlot.Lookup(bodyData.headShape);
					if (accessory == null)
					{
						this.personality.headShape = 0;
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Mouth)
				{
					accessory = accessorySlot.Lookup(bodyData.mouth);
					if (accessory == null)
					{
						this.personality.mouth = 0;
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Eyes)
				{
					accessory = accessorySlot.Lookup(bodyData.eyes);
					if (accessory == null)
					{
						this.personality.eyes = 0;
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Hair)
				{
					accessory = accessorySlot.Lookup(bodyData.hair);
					if (accessory == null)
					{
						this.personality.hair = 0;
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.HatHair)
				{
					accessory = accessorySlot.accessories[0];
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Body)
				{
					accessory = accessorySlot.Lookup(bodyData.body);
					if (accessory == null)
					{
						this.personality.body = 0;
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Arm)
				{
					accessory = accessorySlot.Lookup(bodyData.arms);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.ArmLower)
				{
					accessory = accessorySlot.Lookup(bodyData.armslower);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.ArmLowerSkin)
				{
					accessory = accessorySlot.Lookup(bodyData.armLowerSkin);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.ArmUpperSkin)
				{
					accessory = accessorySlot.Lookup(bodyData.armUpperSkin);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.LegSkin)
				{
					accessory = accessorySlot.Lookup(bodyData.legSkin);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Leg)
				{
					accessory = accessorySlot.Lookup(bodyData.legs);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Belt)
				{
					accessory = accessorySlot.Lookup(bodyData.belt);
					if (accessory == null)
					{
						accessory = accessorySlot.accessories[0];
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Neck)
				{
					accessory = accessorySlot.Lookup(bodyData.neck);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Pelvis)
				{
					accessory = accessorySlot.Lookup(bodyData.pelvis);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Foot)
				{
					accessory = accessorySlot.Lookup(bodyData.foot);
					if (accessory == null)
					{
						accessory = accessorySlot.accessories[0];
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Skirt)
				{
					accessory = accessorySlot.Lookup(bodyData.skirt);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Necklace)
				{
					accessory = accessorySlot.Lookup(bodyData.necklace);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Cuff)
				{
					accessory = accessorySlot.Lookup(bodyData.cuff);
					if (accessory == null)
					{
						accessory = accessorySlot.accessories[0];
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Hand)
				{
					accessory = accessorySlot.Lookup(bodyData.hand);
					if (accessory == null)
					{
						accessory = accessorySlot.accessories[0];
					}
				}
				this.accessories.Add(accessory);
			}
		}
	}

	// Token: 0x0600465A RID: 18010 RVA: 0x00191604 File Offset: 0x0018F804
	private int GenerateTraits(bool is_starter_minion, List<ChoreGroup> disabled_chore_groups, string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false)
	{
		MinionStartingStats.<>c__DisplayClass19_0 CS$<>8__locals1 = new MinionStartingStats.<>c__DisplayClass19_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.is_starter_minion = is_starter_minion;
		CS$<>8__locals1.isDebugMinion = isDebugMinion;
		CS$<>8__locals1.guaranteedAptitudeID = guaranteedAptitudeID;
		CS$<>8__locals1.disabled_chore_groups = disabled_chore_groups;
		CS$<>8__locals1.statDelta = 0;
		CS$<>8__locals1.selectedTraits = new List<string>();
		CS$<>8__locals1.randSeed = new KRandom();
		Trait trait = Db.Get().traits.Get(this.personality.stresstrait);
		this.stressTrait = trait;
		Trait trait2 = Db.Get().traits.Get(this.personality.joyTrait);
		this.joyTrait = trait2;
		this.stickerType = this.personality.stickerType;
		Trait trait3 = Db.Get().traits.TryGet(this.personality.congenitaltrait);
		if (trait3 == null || trait3.Name == "None")
		{
			this.congenitaltrait = null;
		}
		else
		{
			this.congenitaltrait = trait3;
		}
		if (this.personality.model == GameTags.Minions.Models.Bionic)
		{
			string[] default_BIONIC_TRAITS = BionicMinionConfig.DEFAULT_BIONIC_TRAITS;
			for (int i = 0; i < default_BIONIC_TRAITS.Length; i++)
			{
				string id = default_BIONIC_TRAITS[i];
				DUPLICANTSTATS.TraitVal traitVal = DUPLICANTSTATS.BIONICTRAITS.Find((DUPLICANTSTATS.TraitVal match) => match.id == id);
				CS$<>8__locals1.<GenerateTraits>g__SelectTrait|1(traitVal, Db.Get().traits.Get(id), true);
			}
			DUPLICANTSTATS.TraitVal random = DUPLICANTSTATS.BIONICUPGRADETRAITS.GetRandom<DUPLICANTSTATS.TraitVal>();
			CS$<>8__locals1.<GenerateTraits>g__SelectTrait|1(random, Db.Get().traits.Get(random.id), true);
			this.IsValid = true;
			return CS$<>8__locals1.statDelta;
		}
		Func<List<DUPLICANTSTATS.TraitVal>, bool, bool> func = delegate(List<DUPLICANTSTATS.TraitVal> traitPossibilities, bool positiveTrait)
		{
			if (CS$<>8__locals1.<>4__this.Traits.Count > DUPLICANTSTATS.MAX_TRAITS)
			{
				return false;
			}
			Mathf.Abs(Util.GaussianRandom(0f, 1f));
			int num6 = traitPossibilities.Count;
			int num7;
			if (!positiveTrait)
			{
				if (DUPLICANTSTATS.rarityDeckActive.Count < 1)
				{
					DUPLICANTSTATS.rarityDeckActive.AddRange(DUPLICANTSTATS.RARITY_DECK);
				}
				if (DUPLICANTSTATS.rarityDeckActive.Count == DUPLICANTSTATS.RARITY_DECK.Count)
				{
					DUPLICANTSTATS.rarityDeckActive.ShuffleSeeded(CS$<>8__locals1.randSeed);
				}
				num7 = DUPLICANTSTATS.rarityDeckActive[DUPLICANTSTATS.rarityDeckActive.Count - 1];
				DUPLICANTSTATS.rarityDeckActive.RemoveAt(DUPLICANTSTATS.rarityDeckActive.Count - 1);
			}
			else
			{
				List<int> list = new List<int>();
				if (CS$<>8__locals1.is_starter_minion)
				{
					list.Add(CS$<>8__locals1.<>4__this.rarityBalance - 1);
					list.Add(CS$<>8__locals1.<>4__this.rarityBalance);
					list.Add(CS$<>8__locals1.<>4__this.rarityBalance);
					list.Add(CS$<>8__locals1.<>4__this.rarityBalance + 1);
				}
				else
				{
					list.Add(CS$<>8__locals1.<>4__this.rarityBalance - 2);
					list.Add(CS$<>8__locals1.<>4__this.rarityBalance - 1);
					list.Add(CS$<>8__locals1.<>4__this.rarityBalance);
					list.Add(CS$<>8__locals1.<>4__this.rarityBalance + 1);
					list.Add(CS$<>8__locals1.<>4__this.rarityBalance + 2);
				}
				list.ShuffleSeeded(CS$<>8__locals1.randSeed);
				num7 = list[0];
				num7 = Mathf.Max(DUPLICANTSTATS.RARITY_COMMON, num7);
				num7 = Mathf.Min(DUPLICANTSTATS.RARITY_LEGENDARY, num7);
			}
			List<DUPLICANTSTATS.TraitVal> list2 = new List<DUPLICANTSTATS.TraitVal>(traitPossibilities);
			for (int k = list2.Count - 1; k > -1; k--)
			{
				if (list2[k].rarity != num7)
				{
					list2.RemoveAt(k);
					num6--;
				}
			}
			list2.ShuffleSeeded(CS$<>8__locals1.randSeed);
			foreach (DUPLICANTSTATS.TraitVal traitVal4 in list2)
			{
				global::Debug.Assert(SaveLoader.Instance != null, "IsDLCActiveForCurrentSave should not be called from the front end");
				if (!SaveLoader.Instance.IsDLCActiveForCurrentSave(traitVal4.dlcId))
				{
					num6--;
				}
				else if (CS$<>8__locals1.selectedTraits.Contains(traitVal4.id))
				{
					num6--;
				}
				else
				{
					Trait trait5 = Db.Get().traits.TryGet(traitVal4.id);
					if (trait5 == null)
					{
						global::Debug.LogWarning("Trying to add nonexistent trait: " + traitVal4.id);
						num6--;
					}
					else if (!CS$<>8__locals1.isDebugMinion || trait5.disabledChoreGroups == null || trait5.disabledChoreGroups.Length == 0)
					{
						if (CS$<>8__locals1.is_starter_minion && !trait5.ValidStarterTrait)
						{
							num6--;
						}
						else if (traitVal4.doNotGenerateTrait)
						{
							num6--;
						}
						else if (CS$<>8__locals1.<>4__this.AreTraitAndAptitudesExclusive(traitVal4, CS$<>8__locals1.<>4__this.skillAptitudes))
						{
							num6--;
						}
						else if (CS$<>8__locals1.is_starter_minion && CS$<>8__locals1.guaranteedAptitudeID != null && CS$<>8__locals1.<>4__this.AreTraitAndArchetypeExclusive(traitVal4, CS$<>8__locals1.guaranteedAptitudeID))
						{
							num6--;
						}
						else
						{
							if (!CS$<>8__locals1.<>4__this.AreTraitsMutuallyExclusive(traitVal4, CS$<>8__locals1.selectedTraits))
							{
								base.<GenerateTraits>g__SelectTrait|1(traitVal4, trait5, positiveTrait);
								return true;
							}
							num6--;
						}
					}
				}
			}
			return false;
		};
		int num;
		int num2;
		if (CS$<>8__locals1.is_starter_minion)
		{
			num = 1;
			num2 = 1;
		}
		else
		{
			if (DUPLICANTSTATS.podTraitConfigurationsActive.Count < 1)
			{
				DUPLICANTSTATS.podTraitConfigurationsActive.AddRange(DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK);
			}
			if (DUPLICANTSTATS.podTraitConfigurationsActive.Count == DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK.Count)
			{
				DUPLICANTSTATS.podTraitConfigurationsActive.ShuffleSeeded(CS$<>8__locals1.randSeed);
			}
			num = DUPLICANTSTATS.podTraitConfigurationsActive[DUPLICANTSTATS.podTraitConfigurationsActive.Count - 1].first;
			num2 = DUPLICANTSTATS.podTraitConfigurationsActive[DUPLICANTSTATS.podTraitConfigurationsActive.Count - 1].second;
			DUPLICANTSTATS.podTraitConfigurationsActive.RemoveAt(DUPLICANTSTATS.podTraitConfigurationsActive.Count - 1);
		}
		bool flag = false;
		int num3 = 0;
		int num4 = 0;
		int num5 = (num2 + num) * 4;
		if (!string.IsNullOrEmpty(guaranteedTraitID))
		{
			DUPLICANTSTATS.TraitVal traitVal2 = DUPLICANTSTATS.GetTraitVal(guaranteedTraitID);
			if (traitVal2.id == guaranteedTraitID)
			{
				Trait trait4 = Db.Get().traits.TryGet(traitVal2.id);
				bool positiveTrait2 = trait4.PositiveTrait;
				CS$<>8__locals1.selectedTraits.Add(traitVal2.id);
				CS$<>8__locals1.statDelta += traitVal2.statBonus;
				this.rarityBalance += (positiveTrait2 ? (-traitVal2.rarity) : traitVal2.rarity);
				this.Traits.Add(trait4);
				if (trait4.disabledChoreGroups != null)
				{
					for (int j = 0; j < trait4.disabledChoreGroups.Length; j++)
					{
						CS$<>8__locals1.disabled_chore_groups.Add(trait4.disabledChoreGroups[j]);
					}
				}
				if (positiveTrait2)
				{
					num3++;
				}
				else
				{
					num4++;
				}
			}
		}
		if (!flag)
		{
			if (this.congenitaltrait != null)
			{
				DUPLICANTSTATS.TraitVal traitVal3;
				if (this.congenitaltrait.PositiveTrait)
				{
					num3++;
					traitVal3 = DUPLICANTSTATS.GOODTRAITS.Find((DUPLICANTSTATS.TraitVal match) => match.id == CS$<>8__locals1.<>4__this.congenitaltrait.Id);
				}
				else
				{
					num4++;
					traitVal3 = DUPLICANTSTATS.BADTRAITS.Find((DUPLICANTSTATS.TraitVal match) => match.id == CS$<>8__locals1.<>4__this.congenitaltrait.Id);
				}
				CS$<>8__locals1.<GenerateTraits>g__SelectTrait|1(traitVal3, this.congenitaltrait, this.congenitaltrait.PositiveTrait);
			}
		}
		while (num5 > 0 && (num4 < num2 || num3 < num))
		{
			if (num4 < num2 && func(DUPLICANTSTATS.BADTRAITS, false))
			{
				num4++;
			}
			if (num3 < num && func(DUPLICANTSTATS.GOODTRAITS, true))
			{
				num3++;
			}
			num5--;
		}
		if (num5 > 0)
		{
			this.IsValid = true;
		}
		return CS$<>8__locals1.statDelta;
	}

	// Token: 0x0600465B RID: 18011 RVA: 0x00191A30 File Offset: 0x0018FC30
	private void GenerateAptitudes(string guaranteedAptitudeID = null)
	{
		int num = UnityEngine.Random.Range(1, 4);
		List<SkillGroup> list = new List<SkillGroup>(Db.Get().SkillGroups.resources);
		list.Shuffle<SkillGroup>();
		if (guaranteedAptitudeID != null)
		{
			this.skillAptitudes.Add(Db.Get().SkillGroups.Get(guaranteedAptitudeID), (float)DUPLICANTSTATS.APTITUDE_BONUS);
			list.Remove(Db.Get().SkillGroups.Get(guaranteedAptitudeID));
			num--;
		}
		for (int i = 0; i < num; i++)
		{
			this.skillAptitudes.Add(list[i], (float)DUPLICANTSTATS.APTITUDE_BONUS);
		}
	}

	// Token: 0x0600465C RID: 18012 RVA: 0x00191AC4 File Offset: 0x0018FCC4
	private void GenerateAttributes(int pointsDelta, List<ChoreGroup> disabled_chore_groups)
	{
		List<string> list = new List<string>(DUPLICANTSTATS.ALL_ATTRIBUTES);
		for (int i = 0; i < list.Count; i++)
		{
			if (!this.StartingLevels.ContainsKey(list[i]))
			{
				this.StartingLevels[list[i]] = 0;
			}
		}
		foreach (KeyValuePair<SkillGroup, float> keyValuePair in this.skillAptitudes)
		{
			if (keyValuePair.Key.relevantAttributes.Count > 0)
			{
				for (int j = 0; j < keyValuePair.Key.relevantAttributes.Count; j++)
				{
					if (!this.StartingLevels.ContainsKey(keyValuePair.Key.relevantAttributes[j].Id))
					{
						global::Debug.LogError("Need to add " + keyValuePair.Key.relevantAttributes[j].Id + " to TUNING.DUPLICANTSTATS.ALL_ATTRIBUTES");
					}
					Dictionary<string, int> startingLevels = this.StartingLevels;
					string id = keyValuePair.Key.relevantAttributes[j].Id;
					startingLevels[id] += DUPLICANTSTATS.APTITUDE_ATTRIBUTE_BONUSES[this.skillAptitudes.Count - 1];
				}
			}
		}
		List<SkillGroup> list2 = new List<SkillGroup>(this.skillAptitudes.Keys);
		if (pointsDelta > 0)
		{
			for (int k = pointsDelta; k > 0; k--)
			{
				list2.Shuffle<SkillGroup>();
				for (int l = 0; l < list2[0].relevantAttributes.Count; l++)
				{
					Dictionary<string, int> startingLevels = this.StartingLevels;
					string id = list2[0].relevantAttributes[l].Id;
					startingLevels[id]++;
				}
			}
		}
		if (disabled_chore_groups.Count > 0)
		{
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<string, int> keyValuePair2 in this.StartingLevels)
			{
				if (keyValuePair2.Value > num)
				{
					num = keyValuePair2.Value;
				}
				if (keyValuePair2.Key == disabled_chore_groups[0].attribute.Id)
				{
					num2 = keyValuePair2.Value;
				}
			}
			if (num == num2)
			{
				foreach (string text in list)
				{
					if (text != disabled_chore_groups[0].attribute.Id)
					{
						int num3 = 0;
						this.StartingLevels.TryGetValue(text, out num3);
						int num4 = 0;
						if (num3 > 0)
						{
							num4 = 1;
						}
						this.StartingLevels[disabled_chore_groups[0].attribute.Id] = num3 - num4;
						this.StartingLevels[text] = num + num4;
						break;
					}
				}
			}
		}
	}

	// Token: 0x0600465D RID: 18013 RVA: 0x00191DF0 File Offset: 0x0018FFF0
	public void Apply(GameObject go)
	{
		MinionIdentity component = go.GetComponent<MinionIdentity>();
		component.SetName(this.Name);
		component.nameStringKey = this.NameStringKey;
		component.genderStringKey = this.GenderStringKey;
		component.personalityResourceId = this.personality.IdHash;
		component.model = this.personality.model;
		this.ApplyTraits(go);
		this.ApplyRace(go);
		this.ApplyAptitudes(go);
		this.ApplyAccessories(go);
		this.ApplyExperience(go);
		this.ApplyOutfit(this.personality, go);
		this.ApplyJoyResponseOutfit(this.personality, go);
	}

	// Token: 0x0600465E RID: 18014 RVA: 0x00191E88 File Offset: 0x00190088
	public void ApplyExperience(GameObject go)
	{
		foreach (KeyValuePair<string, int> keyValuePair in this.StartingLevels)
		{
			go.GetComponent<AttributeLevels>().SetLevel(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x0600465F RID: 18015 RVA: 0x00191EF0 File Offset: 0x001900F0
	public void ApplyAccessories(GameObject go)
	{
		Accessorizer component = go.GetComponent<Accessorizer>();
		component.ApplyMinionPersonality(this.personality);
		component.UpdateHairBasedOnHat();
	}

	// Token: 0x06004660 RID: 18016 RVA: 0x00191F0C File Offset: 0x0019010C
	public void ApplyOutfit(Personality personality, GameObject go)
	{
		WearableAccessorizer component = go.GetComponent<WearableAccessorizer>();
		Option<ClothingOutfitTarget> option = ClothingOutfitTarget.TryFromTemplateId(personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.Clothing));
		if (option.IsSome())
		{
			component.ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, option.Unwrap().ReadItemValues());
		}
	}

	// Token: 0x06004661 RID: 18017 RVA: 0x00191F4C File Offset: 0x0019014C
	public void ApplyJoyResponseOutfit(Personality personality, GameObject go)
	{
		JoyResponseOutfitTarget joyResponseOutfitTarget = JoyResponseOutfitTarget.FromPersonality(personality);
		JoyResponseOutfitTarget.FromMinion(go).WriteFacadeId(joyResponseOutfitTarget.ReadFacadeId());
	}

	// Token: 0x06004662 RID: 18018 RVA: 0x00191F75 File Offset: 0x00190175
	public void ApplyRace(GameObject go)
	{
		go.GetComponent<MinionIdentity>().voiceIdx = this.voiceIdx;
	}

	// Token: 0x06004663 RID: 18019 RVA: 0x00191F88 File Offset: 0x00190188
	public static KCompBuilder.BodyData CreateBodyData(Personality p)
	{
		return new KCompBuilder.BodyData
		{
			eyes = HashCache.Get().Add(string.Format("eyes_{0:000}", p.eyes)),
			hair = HashCache.Get().Add(string.Format("hair_{0:000}", p.hair)),
			headShape = HashCache.Get().Add(string.Format("headshape_{0:000}", p.headShape)),
			mouth = HashCache.Get().Add(string.Format("mouth_{0:000}", p.mouth)),
			neck = HashCache.Get().Add("neck"),
			arms = HashCache.Get().Add(string.Format("arm_sleeve_{0:000}", p.body)),
			armslower = HashCache.Get().Add(string.Format("arm_lower_sleeve_{0:000}", p.body)),
			body = HashCache.Get().Add(string.Format("torso_{0:000}", p.body)),
			hat = HashedString.Invalid,
			faceFX = HashedString.Invalid,
			armLowerSkin = HashCache.Get().Add(string.Format("arm_lower_{0:000}", p.headShape)),
			armUpperSkin = HashCache.Get().Add(string.Format("arm_upper_{0:000}", p.headShape)),
			legSkin = HashCache.Get().Add(string.Format("leg_skin_{0:000}", p.headShape)),
			neck = HashCache.Get().Add((p.neck != 0) ? string.Format("neck_{0:000}", p.neck) : "neck"),
			legs = HashCache.Get().Add((p.leg != 0) ? string.Format("leg_{0:000}", p.leg) : "leg"),
			belt = HashCache.Get().Add((p.belt != 0) ? string.Format("belt_{0:000}", p.belt) : "belt"),
			pelvis = HashCache.Get().Add((p.pelvis != 0) ? string.Format("pelvis_{0:000}", p.pelvis) : "pelvis"),
			foot = HashCache.Get().Add((p.foot != 0) ? string.Format("foot_{0:000}", p.foot) : "foot"),
			hand = HashCache.Get().Add((p.hand != 0) ? string.Format("hand_paint_{0:000}", p.hand) : "hand_paint"),
			cuff = HashCache.Get().Add((p.cuff != 0) ? string.Format("cuff_{0:000}", p.cuff) : "cuff")
		};
	}

	// Token: 0x06004664 RID: 18020 RVA: 0x001922BC File Offset: 0x001904BC
	public void ApplyAptitudes(GameObject go)
	{
		MinionResume component = go.GetComponent<MinionResume>();
		foreach (KeyValuePair<SkillGroup, float> keyValuePair in this.skillAptitudes)
		{
			component.SetAptitude(keyValuePair.Key.Id, keyValuePair.Value);
		}
	}

	// Token: 0x06004665 RID: 18021 RVA: 0x00192330 File Offset: 0x00190530
	public void ApplyTraits(GameObject go)
	{
		Traits component = go.GetComponent<Traits>();
		component.Clear();
		foreach (Trait trait in this.Traits)
		{
			component.Add(trait);
		}
		component.Add(this.stressTrait);
		component.Add(this.joyTrait);
		go.GetComponent<MinionIdentity>().SetStickerType(this.stickerType);
		MinionIdentity component2 = go.GetComponent<MinionIdentity>();
		component2.SetName(this.Name);
		component2.nameStringKey = this.NameStringKey;
		go.GetComponent<MinionIdentity>().SetGender(this.GenderStringKey);
	}

	// Token: 0x06004666 RID: 18022 RVA: 0x001923E8 File Offset: 0x001905E8
	public GameObject Deliver(Vector3 location)
	{
		GameObject prefab = Assets.GetPrefab(this.personality.model);
		GameObject gameObject = Util.KInstantiate(prefab, null, null);
		gameObject.name = prefab.name;
		gameObject.SetActive(true);
		gameObject.transform.SetLocalPosition(location);
		this.Apply(gameObject);
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		new EmoteChore(gameObject.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_portal_kanim", Telepad.PortalBirthAnim, null);
		return gameObject;
	}

	// Token: 0x06004667 RID: 18023 RVA: 0x00192470 File Offset: 0x00190670
	private bool AreTraitAndAptitudesExclusive(DUPLICANTSTATS.TraitVal traitVal, Dictionary<SkillGroup, float> aptitudes)
	{
		if (traitVal.mutuallyExclusiveAptitudes == null)
		{
			return false;
		}
		foreach (KeyValuePair<SkillGroup, float> keyValuePair in this.skillAptitudes)
		{
			using (List<HashedString>.Enumerator enumerator2 = traitVal.mutuallyExclusiveAptitudes.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == keyValuePair.Key.IdHash && keyValuePair.Value > 0f)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06004668 RID: 18024 RVA: 0x00192528 File Offset: 0x00190728
	private bool AreTraitAndArchetypeExclusive(DUPLICANTSTATS.TraitVal traitVal, string guaranteedAptitudeID)
	{
		if (!DUPLICANTSTATS.ARCHETYPE_TRAIT_EXCLUSIONS.ContainsKey(guaranteedAptitudeID))
		{
			global::Debug.LogError("Need to add attribute " + guaranteedAptitudeID + " to ARCHETYPE_TRAIT_EXCLUSIONS");
		}
		using (List<string>.Enumerator enumerator = DUPLICANTSTATS.ARCHETYPE_TRAIT_EXCLUSIONS[guaranteedAptitudeID].GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == traitVal.id)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004669 RID: 18025 RVA: 0x001925B0 File Offset: 0x001907B0
	private bool AreTraitsMutuallyExclusive(DUPLICANTSTATS.TraitVal traitVal, List<string> selectedTraits)
	{
		foreach (string text in selectedTraits)
		{
			foreach (DUPLICANTSTATS.TraitVal traitVal2 in DUPLICANTSTATS.GOODTRAITS)
			{
				if (text == traitVal2.id && traitVal2.mutuallyExclusiveTraits != null && traitVal2.mutuallyExclusiveTraits.Contains(traitVal.id))
				{
					return true;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal3 in DUPLICANTSTATS.BADTRAITS)
			{
				if (text == traitVal3.id && traitVal3.mutuallyExclusiveTraits != null && traitVal3.mutuallyExclusiveTraits.Contains(traitVal.id))
				{
					return true;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal4 in DUPLICANTSTATS.CONGENITALTRAITS)
			{
				if (text == traitVal4.id && traitVal4.mutuallyExclusiveTraits != null && traitVal4.mutuallyExclusiveTraits.Contains(traitVal.id))
				{
					return true;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal5 in DUPLICANTSTATS.SPECIALTRAITS)
			{
				if (text == traitVal5.id && traitVal5.mutuallyExclusiveTraits != null && traitVal5.mutuallyExclusiveTraits.Contains(traitVal.id))
				{
					return true;
				}
			}
			if (traitVal.mutuallyExclusiveTraits != null && traitVal.mutuallyExclusiveTraits.Contains(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04002DC3 RID: 11715
	public string Name;

	// Token: 0x04002DC4 RID: 11716
	public string NameStringKey;

	// Token: 0x04002DC5 RID: 11717
	public string GenderStringKey;

	// Token: 0x04002DC6 RID: 11718
	public List<Trait> Traits = new List<Trait>();

	// Token: 0x04002DC7 RID: 11719
	public int rarityBalance;

	// Token: 0x04002DC8 RID: 11720
	public Trait stressTrait;

	// Token: 0x04002DC9 RID: 11721
	public Trait joyTrait;

	// Token: 0x04002DCA RID: 11722
	public Trait congenitaltrait;

	// Token: 0x04002DCB RID: 11723
	public string stickerType;

	// Token: 0x04002DCC RID: 11724
	public int voiceIdx;

	// Token: 0x04002DCD RID: 11725
	public Dictionary<string, int> StartingLevels = new Dictionary<string, int>();

	// Token: 0x04002DCE RID: 11726
	public Personality personality;

	// Token: 0x04002DCF RID: 11727
	public List<Accessory> accessories = new List<Accessory>();

	// Token: 0x04002DD0 RID: 11728
	public bool IsValid;

	// Token: 0x04002DD1 RID: 11729
	public Dictionary<SkillGroup, float> skillAptitudes = new Dictionary<SkillGroup, float>();
}
