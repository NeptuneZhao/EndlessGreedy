using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A6B RID: 2667
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionResume")]
public class MinionResume : IExperienceRecipient, ISaveLoadable, ISim200ms
{
	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x06004D5F RID: 19807 RVA: 0x001BBC0E File Offset: 0x001B9E0E
	public MinionIdentity GetIdentity
	{
		get
		{
			return this.identity;
		}
	}

	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x06004D60 RID: 19808 RVA: 0x001BBC16 File Offset: 0x001B9E16
	public float TotalExperienceGained
	{
		get
		{
			return this.totalExperienceGained;
		}
	}

	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x06004D61 RID: 19809 RVA: 0x001BBC1E File Offset: 0x001B9E1E
	public int TotalSkillPointsGained
	{
		get
		{
			return MinionResume.CalculateTotalSkillPointsGained(this.TotalExperienceGained);
		}
	}

	// Token: 0x06004D62 RID: 19810 RVA: 0x001BBC2B File Offset: 0x001B9E2B
	public static int CalculateTotalSkillPointsGained(float experience)
	{
		return Mathf.FloorToInt(Mathf.Pow(experience / (float)SKILLS.TARGET_SKILLS_CYCLE / 600f, 1f / SKILLS.EXPERIENCE_LEVEL_POWER) * (float)SKILLS.TARGET_SKILLS_EARNED);
	}

	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x06004D63 RID: 19811 RVA: 0x001BBC58 File Offset: 0x001B9E58
	public int SkillsMastered
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
			{
				if (keyValuePair.Value)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x06004D64 RID: 19812 RVA: 0x001BBCB4 File Offset: 0x001B9EB4
	public int AvailableSkillpoints
	{
		get
		{
			return this.TotalSkillPointsGained - this.SkillsMastered + ((this.GrantedSkillIDs == null) ? 0 : this.GrantedSkillIDs.Count);
		}
	}

	// Token: 0x06004D65 RID: 19813 RVA: 0x001BBCDC File Offset: 0x001B9EDC
	[OnDeserialized]
	private void OnDeserializedMethod()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 7))
		{
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryByRoleID)
			{
				if (keyValuePair.Value && keyValuePair.Key != "NoRole")
				{
					this.ForceAddSkillPoint();
				}
			}
			foreach (KeyValuePair<HashedString, float> keyValuePair2 in this.AptitudeByRoleGroup)
			{
				this.AptitudeBySkillGroup[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}
		if (this.TotalSkillPointsGained > 1000 || this.TotalSkillPointsGained < 0)
		{
			this.ForceSetSkillPoints(100);
		}
	}

	// Token: 0x06004D66 RID: 19814 RVA: 0x001BBDD8 File Offset: 0x001B9FD8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.MinionResumes.Add(this);
	}

	// Token: 0x06004D67 RID: 19815 RVA: 0x001BBDEC File Offset: 0x001B9FEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.GrantedSkillIDs.RemoveAll((string x) => Db.Get().Skills.TryGet(x) == null);
		List<string> list = new List<string>();
		foreach (string text in this.MasteryBySkillID.Keys)
		{
			if (Db.Get().Skills.TryGet(text) == null)
			{
				list.Add(text);
			}
		}
		foreach (string key in list)
		{
			this.MasteryBySkillID.Remove(key);
		}
		if (this.GrantedSkillIDs == null)
		{
			this.GrantedSkillIDs = new List<string>();
		}
		List<string> list2 = new List<string>();
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).deprecated)
			{
				list2.Add(keyValuePair.Key);
			}
		}
		foreach (string skillId in list2)
		{
			this.UnmasterSkill(skillId);
		}
		foreach (KeyValuePair<string, bool> keyValuePair2 in this.MasteryBySkillID)
		{
			if (keyValuePair2.Value)
			{
				Skill skill = Db.Get().Skills.Get(keyValuePair2.Key);
				foreach (SkillPerk skillPerk in skill.perks)
				{
					if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(skillPerk.requiredDlcIds))
					{
						if (skillPerk.OnRemove != null)
						{
							skillPerk.OnRemove(this);
						}
						if (skillPerk.OnApply != null)
						{
							skillPerk.OnApply(this);
						}
					}
				}
				if (!this.ownedHats.ContainsKey(skill.hat))
				{
					this.ownedHats.Add(skill.hat, true);
				}
			}
		}
		this.UpdateExpectations();
		this.UpdateMorale();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		MinionResume.ApplyHat(this.currentHat, component);
		this.ShowNewSkillPointNotification();
	}

	// Token: 0x06004D68 RID: 19816 RVA: 0x001BC0D0 File Offset: 0x001BA2D0
	public void RestoreResume(Dictionary<string, bool> MasteryBySkillID, Dictionary<HashedString, float> AptitudeBySkillGroup, List<string> GrantedSkillIDs, float totalExperienceGained)
	{
		this.MasteryBySkillID = MasteryBySkillID;
		this.GrantedSkillIDs = ((GrantedSkillIDs != null) ? GrantedSkillIDs : new List<string>());
		this.AptitudeBySkillGroup = AptitudeBySkillGroup;
		this.totalExperienceGained = totalExperienceGained;
	}

	// Token: 0x06004D69 RID: 19817 RVA: 0x001BC0F9 File Offset: 0x001BA2F9
	protected override void OnCleanUp()
	{
		Components.MinionResumes.Remove(this);
		if (this.lastSkillNotification != null)
		{
			Game.Instance.GetComponent<Notifier>().Remove(this.lastSkillNotification);
			this.lastSkillNotification = null;
		}
		base.OnCleanUp();
	}

	// Token: 0x06004D6A RID: 19818 RVA: 0x001BC130 File Offset: 0x001BA330
	public bool HasMasteredSkill(string skillId)
	{
		return this.MasteryBySkillID.ContainsKey(skillId) && this.MasteryBySkillID[skillId];
	}

	// Token: 0x06004D6B RID: 19819 RVA: 0x001BC150 File Offset: 0x001BA350
	public void UpdateUrge()
	{
		if (this.targetHat != this.currentHat)
		{
			if (!base.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
			{
				base.gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
				return;
			}
		}
		else
		{
			base.gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
		}
	}

	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x06004D6C RID: 19820 RVA: 0x001BC1D0 File Offset: 0x001BA3D0
	public string CurrentRole
	{
		get
		{
			return this.currentRole;
		}
	}

	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x06004D6D RID: 19821 RVA: 0x001BC1D8 File Offset: 0x001BA3D8
	public string CurrentHat
	{
		get
		{
			return this.currentHat;
		}
	}

	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x06004D6E RID: 19822 RVA: 0x001BC1E0 File Offset: 0x001BA3E0
	public string TargetHat
	{
		get
		{
			return this.targetHat;
		}
	}

	// Token: 0x06004D6F RID: 19823 RVA: 0x001BC1E8 File Offset: 0x001BA3E8
	public void SetHats(string current, string target)
	{
		this.currentHat = current;
		this.targetHat = target;
	}

	// Token: 0x06004D70 RID: 19824 RVA: 0x001BC1F8 File Offset: 0x001BA3F8
	public void SetCurrentRole(string role_id)
	{
		this.currentRole = role_id;
	}

	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x06004D71 RID: 19825 RVA: 0x001BC201 File Offset: 0x001BA401
	public string TargetRole
	{
		get
		{
			return this.targetRole;
		}
	}

	// Token: 0x06004D72 RID: 19826 RVA: 0x001BC20C File Offset: 0x001BA40C
	private void ApplySkillPerks(string skillId)
	{
		foreach (SkillPerk skillPerk in Db.Get().Skills.Get(skillId).perks)
		{
			if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(skillPerk.requiredDlcIds) && skillPerk.OnApply != null)
			{
				skillPerk.OnApply(this);
			}
		}
	}

	// Token: 0x06004D73 RID: 19827 RVA: 0x001BC290 File Offset: 0x001BA490
	private void RemoveSkillPerks(string skillId)
	{
		foreach (SkillPerk skillPerk in Db.Get().Skills.Get(skillId).perks)
		{
			if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(skillPerk.requiredDlcIds) && skillPerk.OnRemove != null)
			{
				skillPerk.OnRemove(this);
			}
		}
	}

	// Token: 0x06004D74 RID: 19828 RVA: 0x001BC314 File Offset: 0x001BA514
	public void Sim200ms(float dt)
	{
		this.DEBUG_SecondsAlive += dt;
		if (!base.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
		{
			this.DEBUG_PassiveExperienceGained += dt * SKILLS.PASSIVE_EXPERIENCE_PORTION;
			this.AddExperience(dt * SKILLS.PASSIVE_EXPERIENCE_PORTION);
		}
	}

	// Token: 0x06004D75 RID: 19829 RVA: 0x001BC364 File Offset: 0x001BA564
	public bool IsAbleToLearnSkill(string skillId)
	{
		Skill skill = Db.Get().Skills.Get(skillId);
		string choreGroupID = Db.Get().SkillGroups.Get(skill.skillGroup).choreGroupID;
		if (!string.IsNullOrEmpty(choreGroupID))
		{
			Traits component = base.GetComponent<Traits>();
			if (component != null && component.IsChoreGroupDisabled(choreGroupID))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004D76 RID: 19830 RVA: 0x001BC3C8 File Offset: 0x001BA5C8
	public bool BelowMoraleExpectation(Skill skill)
	{
		float num = Db.Get().Attributes.QualityOfLife.Lookup(this).GetTotalValue();
		float totalValue = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(this).GetTotalValue();
		int moraleExpectation = skill.GetMoraleExpectation();
		if (this.AptitudeBySkillGroup.ContainsKey(skill.skillGroup) && this.AptitudeBySkillGroup[skill.skillGroup] > 0f)
		{
			num += 1f;
		}
		return totalValue + (float)moraleExpectation <= num;
	}

	// Token: 0x06004D77 RID: 19831 RVA: 0x001BC458 File Offset: 0x001BA658
	public bool HasMasteredDirectlyRequiredSkillsForSkill(Skill skill)
	{
		for (int i = 0; i < skill.priorSkills.Count; i++)
		{
			if (!this.HasMasteredSkill(skill.priorSkills[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004D78 RID: 19832 RVA: 0x001BC492 File Offset: 0x001BA692
	public bool HasSkillPointsRequiredForSkill(Skill skill)
	{
		return this.AvailableSkillpoints >= 1;
	}

	// Token: 0x06004D79 RID: 19833 RVA: 0x001BC4A0 File Offset: 0x001BA6A0
	public bool HasSkillAptitude(Skill skill)
	{
		return this.AptitudeBySkillGroup.ContainsKey(skill.skillGroup) && this.AptitudeBySkillGroup[skill.skillGroup] > 0f;
	}

	// Token: 0x06004D7A RID: 19834 RVA: 0x001BC4DA File Offset: 0x001BA6DA
	public bool HasBeenGrantedSkill(Skill skill)
	{
		return this.GrantedSkillIDs != null && this.GrantedSkillIDs.Contains(skill.Id);
	}

	// Token: 0x06004D7B RID: 19835 RVA: 0x001BC4FC File Offset: 0x001BA6FC
	public bool HasBeenGrantedSkill(string id)
	{
		return this.GrantedSkillIDs != null && this.GrantedSkillIDs.Contains(id);
	}

	// Token: 0x06004D7C RID: 19836 RVA: 0x001BC51C File Offset: 0x001BA71C
	public MinionResume.SkillMasteryConditions[] GetSkillMasteryConditions(string skillId)
	{
		List<MinionResume.SkillMasteryConditions> list = new List<MinionResume.SkillMasteryConditions>();
		Skill skill = Db.Get().Skills.Get(skillId);
		if (this.HasSkillAptitude(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.SkillAptitude);
		}
		if (!this.BelowMoraleExpectation(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.StressWarning);
		}
		if (!this.IsAbleToLearnSkill(skillId))
		{
			list.Add(MinionResume.SkillMasteryConditions.UnableToLearn);
		}
		if (!this.HasSkillPointsRequiredForSkill(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.NeedsSkillPoints);
		}
		if (!this.HasMasteredDirectlyRequiredSkillsForSkill(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.MissingPreviousSkill);
		}
		return list.ToArray();
	}

	// Token: 0x06004D7D RID: 19837 RVA: 0x001BC596 File Offset: 0x001BA796
	public bool CanMasterSkill(MinionResume.SkillMasteryConditions[] masteryConditions)
	{
		return !Array.Exists<MinionResume.SkillMasteryConditions>(masteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.UnableToLearn || element == MinionResume.SkillMasteryConditions.NeedsSkillPoints || element == MinionResume.SkillMasteryConditions.MissingPreviousSkill);
	}

	// Token: 0x06004D7E RID: 19838 RVA: 0x001BC5C2 File Offset: 0x001BA7C2
	public bool OwnsHat(string hatId)
	{
		return this.ownedHats.ContainsKey(hatId) && this.ownedHats[hatId];
	}

	// Token: 0x06004D7F RID: 19839 RVA: 0x001BC5E0 File Offset: 0x001BA7E0
	public void SkillLearned()
	{
		if (base.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
		{
			base.gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
		}
		foreach (string key in this.ownedHats.Keys.ToList<string>())
		{
			this.ownedHats[key] = true;
		}
		if (this.targetHat != null && this.currentHat != this.targetHat)
		{
			new PutOnHatChore(this, Db.Get().ChoreTypes.SwitchHat);
		}
	}

	// Token: 0x06004D80 RID: 19840 RVA: 0x001BC6B4 File Offset: 0x001BA8B4
	public void MasterSkill(string skillId)
	{
		if (!base.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
		{
			base.gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
		}
		this.MasteryBySkillID[skillId] = true;
		this.ApplySkillPerks(skillId);
		this.UpdateExpectations();
		this.UpdateMorale();
		this.TriggerMasterSkillEvents();
		GameScheduler.Instance.Schedule("Morale Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Morale, true);
		}, null, null);
		if (!this.ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
		{
			this.ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);
		}
		if (this.AvailableSkillpoints == 0 && this.lastSkillNotification != null)
		{
			Game.Instance.GetComponent<Notifier>().Remove(this.lastSkillNotification);
			this.lastSkillNotification = null;
		}
	}

	// Token: 0x06004D81 RID: 19841 RVA: 0x001BC7CC File Offset: 0x001BA9CC
	public void UnmasterSkill(string skillId)
	{
		if (this.MasteryBySkillID.ContainsKey(skillId))
		{
			this.MasteryBySkillID.Remove(skillId);
			this.RemoveSkillPerks(skillId);
			this.UpdateExpectations();
			this.UpdateMorale();
			this.TriggerMasterSkillEvents();
		}
	}

	// Token: 0x06004D82 RID: 19842 RVA: 0x001BC804 File Offset: 0x001BAA04
	public void GrantSkill(string skillId)
	{
		if (this.GrantedSkillIDs == null)
		{
			this.GrantedSkillIDs = new List<string>();
		}
		if (!this.HasBeenGrantedSkill(skillId))
		{
			this.MasteryBySkillID[skillId] = true;
			this.ApplySkillPerks(skillId);
			this.GrantedSkillIDs.Add(skillId);
			this.UpdateExpectations();
			this.UpdateMorale();
			this.TriggerMasterSkillEvents();
			if (!this.ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
			{
				this.ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);
			}
		}
	}

	// Token: 0x06004D83 RID: 19843 RVA: 0x001BC8A4 File Offset: 0x001BAAA4
	public void UngrantSkill(string skillId)
	{
		if (this.GrantedSkillIDs != null)
		{
			this.GrantedSkillIDs.RemoveAll((string match) => match == skillId);
		}
		this.UnmasterSkill(skillId);
	}

	// Token: 0x06004D84 RID: 19844 RVA: 0x001BC8EC File Offset: 0x001BAAEC
	public Sprite GetSkillGrantSourceIcon(string skillID)
	{
		if (!this.GrantedSkillIDs.Contains(skillID))
		{
			return null;
		}
		BionicUpgradesMonitor.Instance smi = base.gameObject.GetSMI<BionicUpgradesMonitor.Instance>();
		if (smi != null)
		{
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
			{
				if (upgradeComponentSlot.HasUpgradeInstalled)
				{
					return Def.GetUISprite(upgradeComponentSlot.installedUpgradeComponent.gameObject, "ui", false).first;
				}
			}
		}
		return Assets.GetSprite("skill_granted_trait");
	}

	// Token: 0x06004D85 RID: 19845 RVA: 0x001BC964 File Offset: 0x001BAB64
	private void TriggerMasterSkillEvents()
	{
		base.Trigger(540773776, null);
		Game.Instance.Trigger(-1523247426, this);
	}

	// Token: 0x06004D86 RID: 19846 RVA: 0x001BC982 File Offset: 0x001BAB82
	public void ForceSetSkillPoints(int points)
	{
		this.totalExperienceGained = MinionResume.CalculatePreviousExperienceBar(points);
	}

	// Token: 0x06004D87 RID: 19847 RVA: 0x001BC990 File Offset: 0x001BAB90
	public void ForceAddSkillPoint()
	{
		this.AddExperience(MinionResume.CalculateNextExperienceBar(this.TotalSkillPointsGained) - this.totalExperienceGained);
	}

	// Token: 0x06004D88 RID: 19848 RVA: 0x001BC9AA File Offset: 0x001BABAA
	public static float CalculateNextExperienceBar(int current_skill_points)
	{
		return Mathf.Pow((float)(current_skill_points + 1) / (float)SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) * (float)SKILLS.TARGET_SKILLS_CYCLE * 600f;
	}

	// Token: 0x06004D89 RID: 19849 RVA: 0x001BC9CE File Offset: 0x001BABCE
	public static float CalculatePreviousExperienceBar(int current_skill_points)
	{
		return Mathf.Pow((float)current_skill_points / (float)SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) * (float)SKILLS.TARGET_SKILLS_CYCLE * 600f;
	}

	// Token: 0x06004D8A RID: 19850 RVA: 0x001BC9F0 File Offset: 0x001BABF0
	private void UpdateExpectations()
	{
		int num = 0;
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && !this.HasBeenGrantedSkill(keyValuePair.Key))
			{
				Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
				num += skill.tier + 1;
			}
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(this);
		if (this.skillsMoraleExpectationModifier != null)
		{
			attributeInstance.Remove(this.skillsMoraleExpectationModifier);
			this.skillsMoraleExpectationModifier = null;
		}
		if (num > 0)
		{
			this.skillsMoraleExpectationModifier = new AttributeModifier(attributeInstance.Id, (float)num, DUPLICANTS.NEEDS.QUALITYOFLIFE.EXPECTATION_MOD_NAME, false, false, true);
			attributeInstance.Add(this.skillsMoraleExpectationModifier);
		}
	}

	// Token: 0x06004D8B RID: 19851 RVA: 0x001BCADC File Offset: 0x001BACDC
	private void UpdateMorale()
	{
		int num = 0;
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && !this.HasBeenGrantedSkill(keyValuePair.Key))
			{
				Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
				float num2 = 0f;
				if (this.AptitudeBySkillGroup.TryGetValue(new HashedString(skill.skillGroup), out num2))
				{
					num += (int)num2;
				}
			}
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(this);
		if (this.skillsMoraleModifier != null)
		{
			attributeInstance.Remove(this.skillsMoraleModifier);
			this.skillsMoraleModifier = null;
		}
		if (num > 0)
		{
			this.skillsMoraleModifier = new AttributeModifier(attributeInstance.Id, (float)num, DUPLICANTS.NEEDS.QUALITYOFLIFE.APTITUDE_SKILLS_MOD_NAME, false, false, true);
			attributeInstance.Add(this.skillsMoraleModifier);
		}
	}

	// Token: 0x06004D8C RID: 19852 RVA: 0x001BCBE4 File Offset: 0x001BADE4
	private void OnSkillPointGained()
	{
		Game.Instance.Trigger(1505456302, this);
		this.ShowNewSkillPointNotification();
		if (PopFXManager.Instance != null)
		{
			string text = MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.identity.GetProperName());
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, text, base.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
		}
		new UpgradeFX.Instance(base.gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, -0.1f)).StartSM();
	}

	// Token: 0x06004D8D RID: 19853 RVA: 0x001BCC90 File Offset: 0x001BAE90
	private void ShowNewSkillPointNotification()
	{
		if (this.AvailableSkillpoints == 1)
		{
			this.lastSkillNotification = new ManagementMenuNotification(global::Action.ManageSkills, NotificationValence.Good, this.identity.GetSoleOwner().gameObject.GetInstanceID().ToString(), MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.identity.GetProperName()), NotificationType.Good, new Func<List<Notification>, object, string>(this.GetSkillPointGainedTooltip), this.identity, true, 0f, delegate(object d)
			{
				ManagementMenu.Instance.OpenSkills(this.identity);
			}, null, null, true);
			base.GetComponent<Notifier>().Add(this.lastSkillNotification, "");
		}
	}

	// Token: 0x06004D8E RID: 19854 RVA: 0x001BCD2C File Offset: 0x001BAF2C
	private string GetSkillPointGainedTooltip(List<Notification> notifications, object data)
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP.Replace("{Duplicant}", ((MinionIdentity)data).GetProperName());
	}

	// Token: 0x06004D8F RID: 19855 RVA: 0x001BCD48 File Offset: 0x001BAF48
	public void SetAptitude(HashedString skillGroupID, float amount)
	{
		this.AptitudeBySkillGroup[skillGroupID] = amount;
	}

	// Token: 0x06004D90 RID: 19856 RVA: 0x001BCD58 File Offset: 0x001BAF58
	public float GetAptitudeExperienceMultiplier(HashedString skillGroupId, float buildingFrequencyMultiplier)
	{
		float num = 0f;
		this.AptitudeBySkillGroup.TryGetValue(skillGroupId, out num);
		return 1f + num * SKILLS.APTITUDE_EXPERIENCE_MULTIPLIER * buildingFrequencyMultiplier;
	}

	// Token: 0x06004D91 RID: 19857 RVA: 0x001BCD8C File Offset: 0x001BAF8C
	public void AddExperience(float amount)
	{
		float num = this.totalExperienceGained;
		float num2 = MinionResume.CalculateNextExperienceBar(this.TotalSkillPointsGained);
		this.totalExperienceGained += amount;
		if (base.isSpawned && this.totalExperienceGained >= num2 && num < num2)
		{
			this.OnSkillPointGained();
		}
	}

	// Token: 0x06004D92 RID: 19858 RVA: 0x001BCDD8 File Offset: 0x001BAFD8
	public override void AddExperienceWithAptitude(string skillGroupId, float amount, float buildingMultiplier)
	{
		float num = amount * this.GetAptitudeExperienceMultiplier(skillGroupId, buildingMultiplier) * SKILLS.ACTIVE_EXPERIENCE_PORTION;
		this.DEBUG_ActiveExperienceGained += num;
		this.AddExperience(num);
	}

	// Token: 0x06004D93 RID: 19859 RVA: 0x001BCE10 File Offset: 0x001BB010
	public bool HasPerk(HashedString perkId)
	{
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perkId))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004D94 RID: 19860 RVA: 0x001BCE8C File Offset: 0x001BB08C
	public bool HasPerk(SkillPerk perk)
	{
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004D95 RID: 19861 RVA: 0x001BCF08 File Offset: 0x001BB108
	public void RemoveHat()
	{
		MinionResume.RemoveHat(base.GetComponent<KBatchedAnimController>());
	}

	// Token: 0x06004D96 RID: 19862 RVA: 0x001BCF18 File Offset: 0x001BB118
	public static void RemoveHat(KBatchedAnimController controller)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		Accessorizer component = controller.GetComponent<Accessorizer>();
		if (component != null)
		{
			Accessory accessory = component.GetAccessory(hat);
			if (accessory != null)
			{
				component.RemoveAccessory(accessory);
			}
		}
		else
		{
			controller.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride(hat.targetSymbolId, 4);
		}
		controller.SetSymbolVisiblity(hat.targetSymbolId, false);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
	}

	// Token: 0x06004D97 RID: 19863 RVA: 0x001BCFB4 File Offset: 0x001BB1B4
	public static void AddHat(string hat_id, KBatchedAnimController controller)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		Accessory accessory = hat.Lookup(hat_id);
		if (accessory == null)
		{
			global::Debug.LogWarning("Missing hat: " + hat_id);
		}
		Accessorizer component = controller.GetComponent<Accessorizer>();
		if (component != null)
		{
			Accessory accessory2 = component.GetAccessory(Db.Get().AccessorySlots.Hat);
			if (accessory2 != null)
			{
				component.RemoveAccessory(accessory2);
			}
			if (accessory != null)
			{
				component.AddAccessory(accessory);
			}
		}
		else
		{
			SymbolOverrideController component2 = controller.GetComponent<SymbolOverrideController>();
			component2.TryRemoveSymbolOverride(hat.targetSymbolId, 4);
			component2.AddSymbolOverride(hat.targetSymbolId, accessory.symbol, 4);
		}
		controller.SetSymbolVisiblity(hat.targetSymbolId, true);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
	}

	// Token: 0x06004D98 RID: 19864 RVA: 0x001BD09C File Offset: 0x001BB29C
	public void ApplyTargetHat()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		MinionResume.ApplyHat(this.targetHat, component);
		this.currentHat = this.targetHat;
		this.targetHat = null;
	}

	// Token: 0x06004D99 RID: 19865 RVA: 0x001BD0CF File Offset: 0x001BB2CF
	public static void ApplyHat(string hat_id, KBatchedAnimController controller)
	{
		if (hat_id.IsNullOrWhiteSpace())
		{
			MinionResume.RemoveHat(controller);
			return;
		}
		MinionResume.AddHat(hat_id, controller);
	}

	// Token: 0x06004D9A RID: 19866 RVA: 0x001BD0E7 File Offset: 0x001BB2E7
	public string GetSkillsSubtitle()
	{
		return string.Format(DUPLICANTS.NEEDS.QUALITYOFLIFE.TOTAL_SKILL_POINTS, this.TotalSkillPointsGained);
	}

	// Token: 0x06004D9B RID: 19867 RVA: 0x001BD104 File Offset: 0x001BB304
	public static bool AnyMinionHasPerk(string perk, int worldId = -1)
	{
		using (List<MinionResume>.Enumerator enumerator = (from minion in (worldId >= 0) ? Components.MinionResumes.GetWorldItems(worldId, true) : Components.MinionResumes.Items
		where !minion.HasTag(GameTags.Dead)
		select minion).ToList<MinionResume>().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasPerk(perk))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004D9C RID: 19868 RVA: 0x001BD1A4 File Offset: 0x001BB3A4
	public static bool AnyOtherMinionHasPerk(string perk, MinionResume me)
	{
		foreach (MinionResume minionResume in Components.MinionResumes.Items)
		{
			if (!(minionResume == me) && minionResume.HasPerk(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004D9D RID: 19869 RVA: 0x001BD214 File Offset: 0x001BB414
	public void ResetSkillLevels(bool returnSkillPoints = true)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (string skillId in list)
		{
			this.UnmasterSkill(skillId);
		}
	}

	// Token: 0x04003369 RID: 13161
	[MyCmpReq]
	private MinionIdentity identity;

	// Token: 0x0400336A RID: 13162
	[Serialize]
	public Dictionary<string, bool> MasteryByRoleID = new Dictionary<string, bool>();

	// Token: 0x0400336B RID: 13163
	[Serialize]
	public Dictionary<string, bool> MasteryBySkillID = new Dictionary<string, bool>();

	// Token: 0x0400336C RID: 13164
	[Serialize]
	public List<string> GrantedSkillIDs = new List<string>();

	// Token: 0x0400336D RID: 13165
	[Serialize]
	public Dictionary<HashedString, float> AptitudeByRoleGroup = new Dictionary<HashedString, float>();

	// Token: 0x0400336E RID: 13166
	[Serialize]
	public Dictionary<HashedString, float> AptitudeBySkillGroup = new Dictionary<HashedString, float>();

	// Token: 0x0400336F RID: 13167
	[Serialize]
	private string currentRole = "NoRole";

	// Token: 0x04003370 RID: 13168
	[Serialize]
	private string targetRole = "NoRole";

	// Token: 0x04003371 RID: 13169
	[Serialize]
	private string currentHat;

	// Token: 0x04003372 RID: 13170
	[Serialize]
	private string targetHat;

	// Token: 0x04003373 RID: 13171
	private Dictionary<string, bool> ownedHats = new Dictionary<string, bool>();

	// Token: 0x04003374 RID: 13172
	[Serialize]
	private float totalExperienceGained;

	// Token: 0x04003375 RID: 13173
	private Notification lastSkillNotification;

	// Token: 0x04003376 RID: 13174
	private AttributeModifier skillsMoraleExpectationModifier;

	// Token: 0x04003377 RID: 13175
	private AttributeModifier skillsMoraleModifier;

	// Token: 0x04003378 RID: 13176
	public float DEBUG_PassiveExperienceGained;

	// Token: 0x04003379 RID: 13177
	public float DEBUG_ActiveExperienceGained;

	// Token: 0x0400337A RID: 13178
	public float DEBUG_SecondsAlive;

	// Token: 0x02001A80 RID: 6784
	public enum SkillMasteryConditions
	{
		// Token: 0x04007CA7 RID: 31911
		SkillAptitude,
		// Token: 0x04007CA8 RID: 31912
		StressWarning,
		// Token: 0x04007CA9 RID: 31913
		UnableToLearn,
		// Token: 0x04007CAA RID: 31914
		NeedsSkillPoints,
		// Token: 0x04007CAB RID: 31915
		MissingPreviousSkill
	}
}
