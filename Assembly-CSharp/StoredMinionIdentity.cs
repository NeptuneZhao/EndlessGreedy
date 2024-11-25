using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005C7 RID: 1479
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/StoredMinionIdentity")]
public class StoredMinionIdentity : KMonoBehaviour, ISaveLoadable, IAssignableIdentity, IListableOption, IPersonalPriorityManager
{
	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000C8B2F File Offset: 0x000C6D2F
	// (set) Token: 0x060023E7 RID: 9191 RVA: 0x000C8B37 File Offset: 0x000C6D37
	[Serialize]
	public string genderStringKey { get; set; }

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x060023E8 RID: 9192 RVA: 0x000C8B40 File Offset: 0x000C6D40
	// (set) Token: 0x060023E9 RID: 9193 RVA: 0x000C8B48 File Offset: 0x000C6D48
	[Serialize]
	public string nameStringKey { get; set; }

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x060023EA RID: 9194 RVA: 0x000C8B51 File Offset: 0x000C6D51
	// (set) Token: 0x060023EB RID: 9195 RVA: 0x000C8B59 File Offset: 0x000C6D59
	[Serialize]
	public HashedString personalityResourceId { get; set; }

	// Token: 0x060023EC RID: 9196 RVA: 0x000C8B64 File Offset: 0x000C6D64
	[OnDeserialized]
	[Obsolete]
	private void OnDeserializedMethod()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 7))
		{
			int num = 0;
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryByRoleID)
			{
				if (keyValuePair.Value && keyValuePair.Key != "NoRole")
				{
					num++;
				}
			}
			this.TotalExperienceGained = MinionResume.CalculatePreviousExperienceBar(num);
			foreach (KeyValuePair<HashedString, float> keyValuePair2 in this.AptitudeByRoleGroup)
			{
				this.AptitudeBySkillGroup[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.forbiddenTagSet = new HashSet<Tag>(this.forbiddenTags);
			this.forbiddenTags = null;
		}
		if (!this.model.IsValid)
		{
			this.model = MinionConfig.MODEL;
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 30))
		{
			this.bodyData = Accessorizer.UpdateAccessorySlots(this.nameStringKey, ref this.accessories);
		}
		if (this.clothingItems.Count > 0)
		{
			this.customClothingItems[ClothingOutfitUtility.OutfitType.Clothing] = new List<ResourceRef<ClothingItemResource>>(this.clothingItems);
			this.clothingItems.Clear();
		}
		List<ResourceRef<Accessory>> list = this.accessories.FindAll((ResourceRef<Accessory> acc) => acc.Get() == null);
		if (list.Count > 0)
		{
			List<ClothingItemResource> list2 = new List<ClothingItemResource>();
			foreach (ResourceRef<Accessory> resourceRef in list)
			{
				ClothingItemResource clothingItemResource = Db.Get().Permits.ClothingItems.TryResolveAccessoryResource(resourceRef.Guid);
				if (clothingItemResource != null && !list2.Contains(clothingItemResource))
				{
					list2.Add(clothingItemResource);
					this.customClothingItems[ClothingOutfitUtility.OutfitType.Clothing].Add(new ResourceRef<ClothingItemResource>(clothingItemResource));
				}
			}
			this.bodyData = Accessorizer.UpdateAccessorySlots(this.nameStringKey, ref this.accessories);
		}
		this.OnDeserializeModifiers();
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x000C8DD4 File Offset: 0x000C6FD4
	public bool HasPerk(SkillPerk perk)
	{
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).perks.Contains(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x000C8E54 File Offset: 0x000C7054
	public bool HasMasteredSkill(string skillId)
	{
		return this.MasteryBySkillID.ContainsKey(skillId) && this.MasteryBySkillID[skillId];
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x000C8E72 File Offset: 0x000C7072
	protected override void OnPrefabInit()
	{
		this.assignableProxy = new Ref<MinionAssignablesProxy>();
		this.minionModifiers = base.GetComponent<MinionModifiers>();
		this.savedAttributeValues = new Dictionary<string, float>();
	}

	// Token: 0x060023F0 RID: 9200 RVA: 0x000C8E98 File Offset: 0x000C7098
	[OnSerializing]
	private void OnSerialize()
	{
		this.savedAttributeValues.Clear();
		foreach (AttributeInstance attributeInstance in this.minionModifiers.attributes)
		{
			this.savedAttributeValues.Add(attributeInstance.Attribute.Id, attributeInstance.GetTotalValue());
		}
	}

	// Token: 0x060023F1 RID: 9201 RVA: 0x000C8F0C File Offset: 0x000C710C
	protected override void OnSpawn()
	{
		string[] attributes = MinionConfig.GetAttributes();
		string[] amounts = MinionConfig.GetAmounts();
		AttributeModifier[] traits = MinionConfig.GetTraits();
		if (this.model == BionicMinionConfig.MODEL)
		{
			attributes = BionicMinionConfig.GetAttributes();
			amounts = BionicMinionConfig.GetAmounts();
			traits = BionicMinionConfig.GetTraits();
		}
		BaseMinionConfig.AddMinionAttributes(this.minionModifiers, attributes);
		BaseMinionConfig.AddMinionAmounts(this.minionModifiers, amounts);
		BaseMinionConfig.AddMinionTraits(BaseMinionConfig.GetMinionNameForModel(this.model), BaseMinionConfig.GetMinionBaseTraitIDForModel(this.model), this.minionModifiers, traits);
		this.ValidateProxy();
		this.CleanupLimboMinions();
	}

	// Token: 0x060023F2 RID: 9202 RVA: 0x000C8F95 File Offset: 0x000C7195
	public void OnHardDelete()
	{
		if (this.assignableProxy.Get() != null)
		{
			Util.KDestroyGameObject(this.assignableProxy.Get().gameObject);
		}
		ScheduleManager.Instance.OnStoredDupeDestroyed(this);
		Components.StoredMinionIdentities.Remove(this);
	}

	// Token: 0x060023F3 RID: 9203 RVA: 0x000C8FD8 File Offset: 0x000C71D8
	private void OnDeserializeModifiers()
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.savedAttributeValues)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.TryGet(keyValuePair.Key);
			if (attribute == null)
			{
				attribute = Db.Get().BuildingAttributes.TryGet(keyValuePair.Key);
			}
			if (attribute != null)
			{
				if (this.minionModifiers.attributes.Get(attribute.Id) != null)
				{
					this.minionModifiers.attributes.Get(attribute.Id).Modifiers.Clear();
					this.minionModifiers.attributes.Get(attribute.Id).ClearModifiers();
				}
				else
				{
					this.minionModifiers.attributes.Add(attribute);
				}
				this.minionModifiers.attributes.Add(new AttributeModifier(attribute.Id, keyValuePair.Value, () => DUPLICANTS.ATTRIBUTES.STORED_VALUE, false, false));
			}
		}
	}

	// Token: 0x060023F4 RID: 9204 RVA: 0x000C910C File Offset: 0x000C730C
	public void ValidateProxy()
	{
		this.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(this.assignableProxy, this);
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x000C9120 File Offset: 0x000C7320
	public string[] GetClothingItemIds(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.customClothingItems.ContainsKey(outfitType))
		{
			string[] array = new string[this.customClothingItems[outfitType].Count];
			for (int i = 0; i < this.customClothingItems[outfitType].Count; i++)
			{
				array[i] = this.customClothingItems[outfitType][i].Get().Id;
			}
			return array;
		}
		return null;
	}

	// Token: 0x060023F6 RID: 9206 RVA: 0x000C9190 File Offset: 0x000C7390
	private void CleanupLimboMinions()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		bool flag = false;
		if (component.InstanceID == -1)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Stored minion with an invalid kpid! Attempting to recover...",
				this.storedName
			});
			flag = true;
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		if (component.conflicted)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with a conflicted kpid! Attempting to recover... ",
				component.InstanceID,
				this.storedName
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		this.assignableProxy.Get().SetTarget(this, base.gameObject);
		bool flag2 = false;
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			List<MinionStorage.Info> storedMinionInfo = minionStorage.GetStoredMinionInfo();
			for (int i = 0; i < storedMinionInfo.Count; i++)
			{
				MinionStorage.Info info = storedMinionInfo[i];
				if (flag && info.serializedMinion != null && info.serializedMinion.GetId() == -1 && info.name == this.storedName)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Found a minion storage with an invalid ref, rebinding.",
						component.InstanceID,
						this.storedName,
						minionStorage.gameObject.name
					});
					info = new MinionStorage.Info(this.storedName, new Ref<KPrefabID>(component));
					storedMinionInfo[i] = info;
					minionStorage.GetComponent<Assignable>().Assign(this);
					flag2 = true;
					break;
				}
				if (info.serializedMinion != null && info.serializedMinion.Get() == component)
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				break;
			}
		}
		if (!flag2)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Found a stored minion that wasn't in any minion storage. Respawning them at the portal.",
				component.InstanceID,
				this.storedName
			});
			GameObject activeTelepad = GameUtil.GetActiveTelepad();
			if (activeTelepad != null)
			{
				MinionStorage.DeserializeMinion(component.gameObject, activeTelepad.transform.GetPosition());
			}
		}
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000C9470 File Offset: 0x000C7670
	public string GetProperName()
	{
		return this.storedName;
	}

	// Token: 0x060023F8 RID: 9208 RVA: 0x000C9478 File Offset: 0x000C7678
	public List<Ownables> GetOwners()
	{
		return this.assignableProxy.Get().ownables;
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x000C948A File Offset: 0x000C768A
	public Ownables GetSoleOwner()
	{
		return this.assignableProxy.Get().GetComponent<Ownables>();
	}

	// Token: 0x060023FA RID: 9210 RVA: 0x000C949C File Offset: 0x000C769C
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Contains(owner as Ownables);
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x000C94AF File Offset: 0x000C76AF
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x000C94BC File Offset: 0x000C76BC
	public Accessory GetAccessory(AccessorySlot slot)
	{
		for (int i = 0; i < this.accessories.Count; i++)
		{
			if (this.accessories[i].Get() != null && this.accessories[i].Get().slot == slot)
			{
				return this.accessories[i].Get();
			}
		}
		return null;
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x000C951E File Offset: 0x000C771E
	public bool IsNull()
	{
		return this == null;
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000C9528 File Offset: 0x000C7728
	public string GetStorageReason()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			using (List<MinionStorage.Info>.Enumerator enumerator2 = minionStorage.GetStoredMinionInfo().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.serializedMinion.Get() == component)
					{
						return minionStorage.GetProperName();
					}
				}
			}
		}
		return "";
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x000C95E0 File Offset: 0x000C77E0
	public bool IsPermittedToConsume(string consumable)
	{
		return !this.forbiddenTagSet.Contains(consumable);
	}

	// Token: 0x06002400 RID: 9216 RVA: 0x000C95F8 File Offset: 0x000C77F8
	public bool IsChoreGroupDisabled(ChoreGroup chore_group)
	{
		foreach (string id in this.traitIDs)
		{
			if (Db.Get().traits.Exists(id))
			{
				Trait trait = Db.Get().traits.Get(id);
				if (trait.disabledChoreGroups != null)
				{
					ChoreGroup[] disabledChoreGroups = trait.disabledChoreGroups;
					for (int i = 0; i < disabledChoreGroups.Length; i++)
					{
						if (disabledChoreGroups[i].IdHash == chore_group.IdHash)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06002401 RID: 9217 RVA: 0x000C96A8 File Offset: 0x000C78A8
	public int GetPersonalPriority(ChoreGroup chore_group)
	{
		ChoreConsumer.PriorityInfo priorityInfo;
		if (this.choreGroupPriorities.TryGetValue(chore_group.IdHash, out priorityInfo))
		{
			return priorityInfo.priority;
		}
		return 0;
	}

	// Token: 0x06002402 RID: 9218 RVA: 0x000C96D2 File Offset: 0x000C78D2
	public int GetAssociatedSkillLevel(ChoreGroup group)
	{
		return 0;
	}

	// Token: 0x06002403 RID: 9219 RVA: 0x000C96D5 File Offset: 0x000C78D5
	public void SetPersonalPriority(ChoreGroup group, int value)
	{
	}

	// Token: 0x06002404 RID: 9220 RVA: 0x000C96D7 File Offset: 0x000C78D7
	public void ResetPersonalPriorities()
	{
	}

	// Token: 0x04001468 RID: 5224
	[Serialize]
	public string storedName;

	// Token: 0x04001469 RID: 5225
	[Serialize]
	public Tag model;

	// Token: 0x0400146A RID: 5226
	[Serialize]
	public string gender;

	// Token: 0x0400146E RID: 5230
	[Serialize]
	[ReadOnly]
	public float arrivalTime;

	// Token: 0x0400146F RID: 5231
	[Serialize]
	public int voiceIdx;

	// Token: 0x04001470 RID: 5232
	[Serialize]
	public KCompBuilder.BodyData bodyData;

	// Token: 0x04001471 RID: 5233
	[Serialize]
	public List<Ref<KPrefabID>> assignedItems;

	// Token: 0x04001472 RID: 5234
	[Serialize]
	public List<Ref<KPrefabID>> equippedItems;

	// Token: 0x04001473 RID: 5235
	[Serialize]
	public List<string> traitIDs;

	// Token: 0x04001474 RID: 5236
	[Serialize]
	public List<ResourceRef<Accessory>> accessories;

	// Token: 0x04001475 RID: 5237
	[Obsolete("Deprecated, use customClothingItems")]
	[Serialize]
	public List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();

	// Token: 0x04001476 RID: 5238
	[Serialize]
	public Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customClothingItems = new Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>>();

	// Token: 0x04001477 RID: 5239
	[Serialize]
	public Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> wearables = new Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable>();

	// Token: 0x04001478 RID: 5240
	[Obsolete("Deprecated, use forbiddenTagSet")]
	[Serialize]
	public List<Tag> forbiddenTags;

	// Token: 0x04001479 RID: 5241
	[Serialize]
	public HashSet<Tag> forbiddenTagSet;

	// Token: 0x0400147A RID: 5242
	[Serialize]
	public Ref<MinionAssignablesProxy> assignableProxy;

	// Token: 0x0400147B RID: 5243
	[Serialize]
	public List<Effects.SaveLoadEffect> saveLoadEffects;

	// Token: 0x0400147C RID: 5244
	[Serialize]
	public List<Effects.SaveLoadImmunities> saveLoadImmunities;

	// Token: 0x0400147D RID: 5245
	[Serialize]
	public Dictionary<string, bool> MasteryByRoleID = new Dictionary<string, bool>();

	// Token: 0x0400147E RID: 5246
	[Serialize]
	public Dictionary<string, bool> MasteryBySkillID = new Dictionary<string, bool>();

	// Token: 0x0400147F RID: 5247
	[Serialize]
	public List<string> grantedSkillIDs = new List<string>();

	// Token: 0x04001480 RID: 5248
	[Serialize]
	public Dictionary<HashedString, float> AptitudeByRoleGroup = new Dictionary<HashedString, float>();

	// Token: 0x04001481 RID: 5249
	[Serialize]
	public Dictionary<HashedString, float> AptitudeBySkillGroup = new Dictionary<HashedString, float>();

	// Token: 0x04001482 RID: 5250
	[Serialize]
	public float TotalExperienceGained;

	// Token: 0x04001483 RID: 5251
	[Serialize]
	public string currentHat;

	// Token: 0x04001484 RID: 5252
	[Serialize]
	public string targetHat;

	// Token: 0x04001485 RID: 5253
	[Serialize]
	public Dictionary<HashedString, ChoreConsumer.PriorityInfo> choreGroupPriorities = new Dictionary<HashedString, ChoreConsumer.PriorityInfo>();

	// Token: 0x04001486 RID: 5254
	[Serialize]
	public List<AttributeLevels.LevelSaveLoad> attributeLevels;

	// Token: 0x04001487 RID: 5255
	[Serialize]
	public Dictionary<string, float> savedAttributeValues;

	// Token: 0x04001488 RID: 5256
	public MinionModifiers minionModifiers;
}
