using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200058C RID: 1420
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionIdentity")]
public class MinionIdentity : KMonoBehaviour, ISaveLoadable, IAssignableIdentity, IListableOption, ISim1000ms
{
	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06002102 RID: 8450 RVA: 0x000B8C50 File Offset: 0x000B6E50
	// (set) Token: 0x06002103 RID: 8451 RVA: 0x000B8C58 File Offset: 0x000B6E58
	[Serialize]
	public string genderStringKey { get; set; }

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06002104 RID: 8452 RVA: 0x000B8C61 File Offset: 0x000B6E61
	// (set) Token: 0x06002105 RID: 8453 RVA: 0x000B8C69 File Offset: 0x000B6E69
	[Serialize]
	public string nameStringKey { get; set; }

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06002106 RID: 8454 RVA: 0x000B8C72 File Offset: 0x000B6E72
	// (set) Token: 0x06002107 RID: 8455 RVA: 0x000B8C7A File Offset: 0x000B6E7A
	[Serialize]
	public HashedString personalityResourceId { get; set; }

	// Token: 0x06002108 RID: 8456 RVA: 0x000B8C83 File Offset: 0x000B6E83
	public static void DestroyStatics()
	{
		MinionIdentity.maleNameList = null;
		MinionIdentity.femaleNameList = null;
	}

	// Token: 0x06002109 RID: 8457 RVA: 0x000B8C94 File Offset: 0x000B6E94
	protected override void OnPrefabInit()
	{
		if (this.name == null)
		{
			this.name = MinionIdentity.ChooseRandomName();
		}
		if (GameClock.Instance != null)
		{
			this.arrivalTime = (float)GameClock.Instance.GetCycle();
		}
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if (component != null)
		{
			KAnimControllerBase kanimControllerBase = component;
			kanimControllerBase.OnUpdateBounds = (Action<Bounds>)Delegate.Combine(kanimControllerBase.OnUpdateBounds, new Action<Bounds>(this.OnUpdateBounds));
		}
		GameUtil.SubscribeToTags<MinionIdentity>(this, MinionIdentity.OnDeadTagAddedDelegate, true);
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x000B8D10 File Offset: 0x000B6F10
	protected override void OnSpawn()
	{
		if (this.addToIdentityList)
		{
			this.ValidateProxy();
			this.CleanupLimboMinions();
		}
		PathProber component = base.GetComponent<PathProber>();
		if (component != null)
		{
			component.SetGroupProber(MinionGroupProber.Get());
		}
		this.SetName(this.name);
		if (this.nameStringKey == null)
		{
			this.nameStringKey = this.name;
		}
		this.SetGender(this.gender);
		if (this.genderStringKey == null)
		{
			this.genderStringKey = "NB";
		}
		if (this.personalityResourceId == HashedString.Invalid)
		{
			Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(this.nameStringKey);
			if (personalityFromNameStringKey != null)
			{
				this.personalityResourceId = personalityFromNameStringKey.Id;
			}
		}
		if (!this.model.IsValid)
		{
			Personality personalityFromNameStringKey2 = Db.Get().Personalities.GetPersonalityFromNameStringKey(this.nameStringKey);
			if (personalityFromNameStringKey2 != null)
			{
				this.model = personalityFromNameStringKey2.model;
			}
		}
		if (this.addToIdentityList)
		{
			Components.MinionIdentities.Add(this);
			if (!Components.MinionIdentitiesByModel.ContainsKey(this.model))
			{
				Components.MinionIdentitiesByModel[this.model] = new Components.Cmps<MinionIdentity>();
			}
			Components.MinionIdentitiesByModel[this.model].Add(this);
			if (!base.gameObject.HasTag(GameTags.Dead))
			{
				Components.LiveMinionIdentities.Add(this);
				if (!Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
				{
					Components.LiveMinionIdentitiesByModel[this.model] = new Components.Cmps<MinionIdentity>();
				}
				Components.LiveMinionIdentitiesByModel[this.model].Add(this);
				Game.Instance.Trigger(2144209314, this);
			}
		}
		SymbolOverrideController component2 = base.GetComponent<SymbolOverrideController>();
		if (component2 != null)
		{
			Accessorizer component3 = base.gameObject.GetComponent<Accessorizer>();
			if (component3 != null)
			{
				string str = HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.Mouth).symbol.hash).Replace("mouth", "cheek");
				component2.AddSymbolOverride("snapto_cheek", Assets.GetAnim("head_swap_kanim").GetData().build.GetSymbol(str), 1);
				component2.AddSymbolOverride("snapto_hair_always", component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol, 1);
				component2.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			}
		}
		this.voiceId = (this.voiceIdx + 1).ToString("D2");
		Prioritizable component4 = base.GetComponent<Prioritizable>();
		if (component4 != null)
		{
			component4.showIcon = false;
		}
		Pickupable component5 = base.GetComponent<Pickupable>();
		if (component5 != null)
		{
			component5.carryAnimOverride = Assets.GetAnim("anim_incapacitated_carrier_kanim");
		}
		this.ApplyCustomGameSettings();
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x000B9054 File Offset: 0x000B7254
	public void ValidateProxy()
	{
		this.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(this.assignableProxy, this);
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x000B9068 File Offset: 0x000B7268
	private void CleanupLimboMinions()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (component.InstanceID == -1)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with an invalid kpid! Attempting to recover...",
				this.name
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
		if (component.conflicted)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with a conflicted kpid! Attempting to recover... ",
				component.InstanceID,
				this.name
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
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x000B91A1 File Offset: 0x000B73A1
	public string GetProperName()
	{
		return base.gameObject.GetProperName();
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x000B91AE File Offset: 0x000B73AE
	public string GetVoiceId()
	{
		return this.voiceId;
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x000B91B6 File Offset: 0x000B73B6
	public void SetName(string name)
	{
		this.name = name;
		if (this.selectable != null)
		{
			this.selectable.SetName(name);
		}
		base.gameObject.name = name;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x000B91F5 File Offset: 0x000B73F5
	public void SetStickerType(string stickerType)
	{
		this.stickerType = stickerType;
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x000B91FE File Offset: 0x000B73FE
	public bool IsNull()
	{
		return this == null;
	}

	// Token: 0x06002112 RID: 8466 RVA: 0x000B9207 File Offset: 0x000B7407
	public void SetGender(string gender)
	{
		this.gender = gender;
		this.selectable.SetGender(gender);
	}

	// Token: 0x06002113 RID: 8467 RVA: 0x000B921C File Offset: 0x000B741C
	public static string ChooseRandomName()
	{
		if (MinionIdentity.femaleNameList == null)
		{
			MinionIdentity.maleNameList = new MinionIdentity.NameList(Game.Instance.maleNamesFile);
			MinionIdentity.femaleNameList = new MinionIdentity.NameList(Game.Instance.femaleNamesFile);
		}
		if (UnityEngine.Random.value > 0.5f)
		{
			return MinionIdentity.maleNameList.Next();
		}
		return MinionIdentity.femaleNameList.Next();
	}

	// Token: 0x06002114 RID: 8468 RVA: 0x000B927C File Offset: 0x000B747C
	protected override void OnCleanUp()
	{
		if (this.assignableProxy != null)
		{
			MinionAssignablesProxy minionAssignablesProxy = this.assignableProxy.Get();
			if (minionAssignablesProxy && minionAssignablesProxy.target == this)
			{
				Util.KDestroyGameObject(minionAssignablesProxy.gameObject);
			}
		}
		Components.MinionIdentities.Remove(this);
		if (Components.MinionIdentitiesByModel.ContainsKey(this.model))
		{
			Components.MinionIdentitiesByModel[this.model].Remove(this);
		}
		Components.LiveMinionIdentities.Remove(this);
		if (Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
		{
			Components.LiveMinionIdentitiesByModel[this.model].Remove(this);
		}
		Game.Instance.Trigger(2144209314, this);
	}

	// Token: 0x06002115 RID: 8469 RVA: 0x000B932F File Offset: 0x000B752F
	private void OnUpdateBounds(Bounds bounds)
	{
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		component.offset = bounds.center;
		component.size = bounds.extents;
	}

	// Token: 0x06002116 RID: 8470 RVA: 0x000B935C File Offset: 0x000B755C
	private void OnDied(object data)
	{
		this.GetSoleOwner().UnassignAll();
		this.GetEquipment().UnequipAll();
		Components.LiveMinionIdentities.Remove(this);
		if (Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
		{
			Components.LiveMinionIdentitiesByModel[this.model].Remove(this);
		}
		Game.Instance.Trigger(-1523247426, this);
		Game.Instance.Trigger(2144209314, this);
	}

	// Token: 0x06002117 RID: 8471 RVA: 0x000B93D2 File Offset: 0x000B75D2
	public List<Ownables> GetOwners()
	{
		return this.assignableProxy.Get().ownables;
	}

	// Token: 0x06002118 RID: 8472 RVA: 0x000B93E4 File Offset: 0x000B75E4
	public Ownables GetSoleOwner()
	{
		return this.assignableProxy.Get().GetComponent<Ownables>();
	}

	// Token: 0x06002119 RID: 8473 RVA: 0x000B93F6 File Offset: 0x000B75F6
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Contains(owner as Ownables);
	}

	// Token: 0x0600211A RID: 8474 RVA: 0x000B9409 File Offset: 0x000B7609
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x0600211B RID: 8475 RVA: 0x000B9416 File Offset: 0x000B7616
	public Equipment GetEquipment()
	{
		return this.assignableProxy.Get().GetComponent<Equipment>();
	}

	// Token: 0x0600211C RID: 8476 RVA: 0x000B9428 File Offset: 0x000B7628
	public void Sim1000ms(float dt)
	{
		if (this == null)
		{
			return;
		}
		if (this.navigator == null)
		{
			this.navigator = base.GetComponent<Navigator>();
		}
		if (this.navigator != null && !this.navigator.IsMoving())
		{
			return;
		}
		if (this.choreDriver == null)
		{
			this.choreDriver = base.GetComponent<ChoreDriver>();
		}
		if (this.choreDriver != null)
		{
			Chore currentChore = this.choreDriver.GetCurrentChore();
			if (currentChore != null && currentChore is FetchAreaChore)
			{
				MinionResume component = base.GetComponent<MinionResume>();
				if (component != null)
				{
					component.AddExperienceWithAptitude(Db.Get().SkillGroups.Hauling.Id, dt, SKILLS.ALL_DAY_EXPERIENCE);
				}
			}
		}
	}

	// Token: 0x0600211D RID: 8477 RVA: 0x000B94E4 File Offset: 0x000B76E4
	private void ApplyCustomGameSettings()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ImmuneSystem);
		if (currentQualitySetting.id == "Compromised")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, -0.3333f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Weak")
		{
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -1f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.WEAK.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Strong")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Invincible")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 100000000f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 200f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Stress);
		if (currentQualitySetting2.id == "Doomed")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.033333335f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.DOOMED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Pessimistic")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.016666668f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.PESSIMISTIC.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Optimistic")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.016666668f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.OPTIMISTIC.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Indomitable")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, float.NegativeInfinity, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.INDOMITABLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		SettingLevel currentQualitySetting3 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CalorieBurn);
		if (currentQualitySetting3.id == "VeryHard")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * 1f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.VERYHARD.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Hard")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * 0.5f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.HARD.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Easy")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * -0.5f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.EASY.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Disabled")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, float.PositiveInfinity, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.DISABLED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
	}

	// Token: 0x0600211E RID: 8478 RVA: 0x000B9AAC File Offset: 0x000B7CAC
	public static float GetCalorieBurnMultiplier()
	{
		float result = 1f;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CalorieBurn);
		if (currentQualitySetting.id == "VeryHard")
		{
			result = 2f;
		}
		else if (currentQualitySetting.id == "Hard")
		{
			result = 1.5f;
		}
		else if (currentQualitySetting.id == "Easy")
		{
			result = 0.5f;
		}
		else if (currentQualitySetting.id == "Disabled")
		{
			result = 0f;
		}
		return result;
	}

	// Token: 0x04001280 RID: 4736
	public const string HairAlwaysSymbol = "snapto_hair_always";

	// Token: 0x04001281 RID: 4737
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001282 RID: 4738
	[MyCmpReq]
	public Modifiers modifiers;

	// Token: 0x04001283 RID: 4739
	public int femaleVoiceCount;

	// Token: 0x04001284 RID: 4740
	public int maleVoiceCount;

	// Token: 0x04001285 RID: 4741
	[Serialize]
	public Tag model;

	// Token: 0x04001286 RID: 4742
	[Serialize]
	private new string name;

	// Token: 0x04001287 RID: 4743
	[Serialize]
	public string gender;

	// Token: 0x0400128B RID: 4747
	[Serialize]
	public string stickerType;

	// Token: 0x0400128C RID: 4748
	[Serialize]
	[ReadOnly]
	public float arrivalTime;

	// Token: 0x0400128D RID: 4749
	[Serialize]
	public int voiceIdx;

	// Token: 0x0400128E RID: 4750
	[Serialize]
	public Ref<MinionAssignablesProxy> assignableProxy;

	// Token: 0x0400128F RID: 4751
	private Navigator navigator;

	// Token: 0x04001290 RID: 4752
	private ChoreDriver choreDriver;

	// Token: 0x04001291 RID: 4753
	public float timeLastSpoke;

	// Token: 0x04001292 RID: 4754
	private string voiceId;

	// Token: 0x04001293 RID: 4755
	private KAnimHashedString overrideExpression;

	// Token: 0x04001294 RID: 4756
	private KAnimHashedString expression;

	// Token: 0x04001295 RID: 4757
	public bool addToIdentityList = true;

	// Token: 0x04001296 RID: 4758
	private static MinionIdentity.NameList maleNameList;

	// Token: 0x04001297 RID: 4759
	private static MinionIdentity.NameList femaleNameList;

	// Token: 0x04001298 RID: 4760
	private static readonly EventSystem.IntraObjectHandler<MinionIdentity> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<MinionIdentity>(GameTags.Dead, delegate(MinionIdentity component, object data)
	{
		component.OnDied(data);
	});

	// Token: 0x0200137F RID: 4991
	private class NameList
	{
		// Token: 0x0600875B RID: 34651 RVA: 0x0032B784 File Offset: 0x00329984
		public NameList(TextAsset file)
		{
			string[] array = file.text.Replace("  ", " ").Replace("\r\n", "\n").Split('\n', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(' ', StringSplitOptions.None);
				if (array2[array2.Length - 1] != "" && array2[array2.Length - 1] != null)
				{
					this.names.Add(array2[array2.Length - 1]);
				}
			}
			this.names.Shuffle<string>();
		}

		// Token: 0x0600875C RID: 34652 RVA: 0x0032B824 File Offset: 0x00329A24
		public string Next()
		{
			List<string> list = this.names;
			int num = this.idx;
			this.idx = num + 1;
			return list[num % this.names.Count];
		}

		// Token: 0x040066D8 RID: 26328
		private List<string> names = new List<string>();

		// Token: 0x040066D9 RID: 26329
		private int idx;
	}
}
