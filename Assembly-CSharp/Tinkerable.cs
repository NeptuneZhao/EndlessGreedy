using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B36 RID: 2870
[AddComponentMenu("KMonoBehaviour/Workable/Tinkerable")]
public class Tinkerable : Workable
{
	// Token: 0x0600559A RID: 21914 RVA: 0x001E9428 File Offset: 0x001E7628
	public static Tinkerable MakePowerTinkerable(GameObject prefab)
	{
		RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
		roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
		Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
		tinkerable.tinkerMaterialTag = PowerControlStationConfig.TINKER_TOOLS;
		tinkerable.tinkerMaterialAmount = 1f;
		tinkerable.requiredSkillPerk = PowerControlStationConfig.ROLE_PERK;
		tinkerable.onCompleteSFX = "Generator_Microchip_installed";
		tinkerable.boostSymbolNames = new string[]
		{
			"booster",
			"blue_light_bloom"
		};
		tinkerable.SetWorkTime(30f);
		tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		tinkerable.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		tinkerable.choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;
		tinkerable.choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;
		tinkerable.addedEffect = "PowerTinker";
		tinkerable.effectAttributeId = Db.Get().Attributes.Machinery.Id;
		tinkerable.effectMultiplier = 0.025f;
		tinkerable.multitoolContext = "powertinker";
		tinkerable.multitoolHitEffectTag = "fx_powertinker_splash";
		tinkerable.shouldShowSkillPerkStatusItem = false;
		prefab.AddOrGet<Storage>();
		prefab.AddOrGet<Effects>();
		prefab.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable);
		};
		return tinkerable;
	}

	// Token: 0x0600559B RID: 21915 RVA: 0x001E95B0 File Offset: 0x001E77B0
	public static Tinkerable MakeFarmTinkerable(GameObject prefab)
	{
		RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Farm.Id;
		roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
		Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
		tinkerable.tinkerMaterialTag = FarmStationConfig.TINKER_TOOLS;
		tinkerable.tinkerMaterialAmount = 1f;
		tinkerable.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
		tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		tinkerable.addedEffect = "FarmTinker";
		tinkerable.effectAttributeId = Db.Get().Attributes.Botanist.Id;
		tinkerable.effectMultiplier = 0.1f;
		tinkerable.SetWorkTime(15f);
		tinkerable.attributeConverter = Db.Get().AttributeConverters.PlantTendSpeed;
		tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		tinkerable.choreTypeTinker = Db.Get().ChoreTypes.CropTend.IdHash;
		tinkerable.choreTypeFetch = Db.Get().ChoreTypes.FarmFetch.IdHash;
		tinkerable.multitoolContext = "tend";
		tinkerable.multitoolHitEffectTag = "fx_tend_splash";
		tinkerable.shouldShowSkillPerkStatusItem = false;
		prefab.AddOrGet<Storage>();
		prefab.AddOrGet<Effects>();
		prefab.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable);
		};
		return tinkerable;
	}

	// Token: 0x0600559C RID: 21916 RVA: 0x001E971C File Offset: 0x001E791C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		base.Subscribe<Tinkerable>(-1157678353, Tinkerable.OnEffectRemovedDelegate);
		base.Subscribe<Tinkerable>(-1697596308, Tinkerable.OnStorageChangeDelegate);
		base.Subscribe<Tinkerable>(144050788, Tinkerable.OnUpdateRoomDelegate);
		base.Subscribe<Tinkerable>(-592767678, Tinkerable.OnOperationalChangedDelegate);
	}

	// Token: 0x0600559D RID: 21917 RVA: 0x001E97C9 File Offset: 0x001E79C9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.prioritizableAdded = true;
		base.Subscribe<Tinkerable>(493375141, Tinkerable.OnRefreshUserMenuDelegate);
		this.UpdateVisual();
	}

	// Token: 0x0600559E RID: 21918 RVA: 0x001E97FA File Offset: 0x001E79FA
	protected override void OnCleanUp()
	{
		this.UpdateMaterialReservation(false);
		if (this.updateHandle.IsValid)
		{
			this.updateHandle.ClearScheduler();
		}
		if (this.prioritizableAdded)
		{
			Prioritizable.RemoveRef(base.gameObject);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600559F RID: 21919 RVA: 0x001E9834 File Offset: 0x001E7A34
	private void OnOperationalChanged(object data)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x060055A0 RID: 21920 RVA: 0x001E983C File Offset: 0x001E7A3C
	private void OnEffectRemoved(object data)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x060055A1 RID: 21921 RVA: 0x001E9844 File Offset: 0x001E7A44
	private void OnUpdateRoom(object data)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x060055A2 RID: 21922 RVA: 0x001E984C File Offset: 0x001E7A4C
	private void OnStorageChange(object data)
	{
		if (((GameObject)data).IsPrefabID(this.tinkerMaterialTag))
		{
			this.QueueUpdateChore();
		}
	}

	// Token: 0x060055A3 RID: 21923 RVA: 0x001E9868 File Offset: 0x001E7A68
	private void QueueUpdateChore()
	{
		if (this.updateHandle.IsValid)
		{
			this.updateHandle.ClearScheduler();
		}
		this.updateHandle = GameScheduler.Instance.Schedule("UpdateTinkerChore", 1.2f, new Action<object>(this.UpdateChoreCallback), null, null);
	}

	// Token: 0x060055A4 RID: 21924 RVA: 0x001E98B5 File Offset: 0x001E7AB5
	private void UpdateChoreCallback(object obj)
	{
		this.UpdateChore();
	}

	// Token: 0x060055A5 RID: 21925 RVA: 0x001E98C0 File Offset: 0x001E7AC0
	private void UpdateChore()
	{
		Operational component = base.GetComponent<Operational>();
		bool flag = component == null || component.IsFunctional;
		bool flag2 = this.HasEffect();
		bool flag3 = this.HasCorrectRoom();
		bool flag4 = !flag2 && flag && flag3 && this.userMenuAllowed;
		bool flag5 = flag2 || !flag3 || !this.userMenuAllowed;
		if (this.chore == null && flag4)
		{
			this.UpdateMaterialReservation(true);
			if (this.HasMaterial())
			{
				this.chore = new WorkChore<Tinkerable>(Db.Get().ChoreTypes.GetByHash(this.choreTypeTinker), this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				if (component != null)
				{
					this.chore.AddPrecondition(ChorePreconditions.instance.IsFunctional, component);
				}
			}
			else
			{
				this.chore = new FetchChore(Db.Get().ChoreTypes.GetByHash(this.choreTypeFetch), this.storage, this.tinkerMaterialAmount, new HashSet<Tag>
				{
					this.tinkerMaterialTag
				}, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, new Action<Chore>(this.OnFetchComplete), null, null, Operational.State.Functional, 0);
			}
			this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, this.requiredSkillPerk);
			if (!string.IsNullOrEmpty(base.GetComponent<RoomTracker>().requiredRoomType))
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.IsInMyRoom, Grid.PosToCell(base.transform.GetPosition()));
				return;
			}
		}
		else if (this.chore != null && flag5)
		{
			this.UpdateMaterialReservation(false);
			this.chore.Cancel("No longer needed");
			this.chore = null;
		}
	}

	// Token: 0x060055A6 RID: 21926 RVA: 0x001E9A68 File Offset: 0x001E7C68
	private bool HasCorrectRoom()
	{
		return this.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x060055A7 RID: 21927 RVA: 0x001E9A78 File Offset: 0x001E7C78
	private bool RoomHasTinkerstation()
	{
		if (!this.roomTracker.IsInCorrectRoom())
		{
			return false;
		}
		if (this.roomTracker.room == null)
		{
			return false;
		}
		foreach (KPrefabID kprefabID in this.roomTracker.room.buildings)
		{
			if (!(kprefabID == null))
			{
				TinkerStation component = kprefabID.GetComponent<TinkerStation>();
				if (component != null && component.outputPrefab == this.tinkerMaterialTag)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060055A8 RID: 21928 RVA: 0x001E9B20 File Offset: 0x001E7D20
	private void UpdateMaterialReservation(bool shouldReserve)
	{
		if (shouldReserve && !this.hasReservedMaterial)
		{
			MaterialNeeds.UpdateNeed(this.tinkerMaterialTag, this.tinkerMaterialAmount, base.gameObject.GetMyWorldId());
			this.hasReservedMaterial = shouldReserve;
			return;
		}
		if (!shouldReserve && this.hasReservedMaterial)
		{
			MaterialNeeds.UpdateNeed(this.tinkerMaterialTag, -this.tinkerMaterialAmount, base.gameObject.GetMyWorldId());
			this.hasReservedMaterial = shouldReserve;
		}
	}

	// Token: 0x060055A9 RID: 21929 RVA: 0x001E9B8B File Offset: 0x001E7D8B
	private void OnFetchComplete(Chore data)
	{
		this.UpdateMaterialReservation(false);
		this.chore = null;
		this.UpdateChore();
	}

	// Token: 0x060055AA RID: 21930 RVA: 0x001E9BA4 File Offset: 0x001E7DA4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.storage.ConsumeIgnoringDisease(this.tinkerMaterialTag, this.tinkerMaterialAmount);
		float totalValue = worker.GetAttributes().Get(Db.Get().Attributes.Get(this.effectAttributeId)).GetTotalValue();
		this.effects.Add(this.addedEffect, true).timeRemaining *= 1f + totalValue * this.effectMultiplier;
		this.UpdateVisual();
		this.UpdateMaterialReservation(false);
		this.chore = null;
		this.UpdateChore();
		string sound = GlobalAssets.GetSound(this.onCompleteSFX, false);
		if (sound != null)
		{
			SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, base.transform.position, 1f, false));
		}
	}

	// Token: 0x060055AB RID: 21931 RVA: 0x001E9C68 File Offset: 0x001E7E68
	private void UpdateVisual()
	{
		if (this.boostSymbolNames == null)
		{
			return;
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		bool is_visible = this.effects.HasEffect(this.addedEffect);
		foreach (string str in this.boostSymbolNames)
		{
			component.SetSymbolVisiblity(str, is_visible);
		}
	}

	// Token: 0x060055AC RID: 21932 RVA: 0x001E9CBF File Offset: 0x001E7EBF
	private bool HasMaterial()
	{
		return this.storage.GetAmountAvailable(this.tinkerMaterialTag) >= this.tinkerMaterialAmount;
	}

	// Token: 0x060055AD RID: 21933 RVA: 0x001E9CDD File Offset: 0x001E7EDD
	private bool HasEffect()
	{
		return this.effects.HasEffect(this.addedEffect);
	}

	// Token: 0x060055AE RID: 21934 RVA: 0x001E9CF0 File Offset: 0x001E7EF0
	private void OnRefreshUserMenu(object data)
	{
		if (this.roomTracker.IsInCorrectRoom())
		{
			KIconButtonMenu.ButtonInfo button = this.userMenuAllowed ? new KIconButtonMenu.ButtonInfo("action_switch_toggle", UI.USERMENUACTIONS.TINKER.DISALLOW, new System.Action(this.OnClickToggleTinker), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.TINKER.TOOLTIP_DISALLOW, true) : new KIconButtonMenu.ButtonInfo("action_switch_toggle", UI.USERMENUACTIONS.TINKER.ALLOW, new System.Action(this.OnClickToggleTinker), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.TINKER.TOOLTIP_ALLOW, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x060055AF RID: 21935 RVA: 0x001E9D99 File Offset: 0x001E7F99
	private void OnClickToggleTinker()
	{
		this.userMenuAllowed = !this.userMenuAllowed;
		this.UpdateChore();
	}

	// Token: 0x04003816 RID: 14358
	private Chore chore;

	// Token: 0x04003817 RID: 14359
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003818 RID: 14360
	[MyCmpGet]
	private Effects effects;

	// Token: 0x04003819 RID: 14361
	[MyCmpGet]
	private RoomTracker roomTracker;

	// Token: 0x0400381A RID: 14362
	public Tag tinkerMaterialTag;

	// Token: 0x0400381B RID: 14363
	public float tinkerMaterialAmount;

	// Token: 0x0400381C RID: 14364
	public string addedEffect;

	// Token: 0x0400381D RID: 14365
	public string effectAttributeId;

	// Token: 0x0400381E RID: 14366
	public float effectMultiplier;

	// Token: 0x0400381F RID: 14367
	public string[] boostSymbolNames;

	// Token: 0x04003820 RID: 14368
	public string onCompleteSFX;

	// Token: 0x04003821 RID: 14369
	public HashedString choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;

	// Token: 0x04003822 RID: 14370
	public HashedString choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;

	// Token: 0x04003823 RID: 14371
	[Serialize]
	private bool userMenuAllowed = true;

	// Token: 0x04003824 RID: 14372
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnEffectRemovedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnEffectRemoved(data);
	});

	// Token: 0x04003825 RID: 14373
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04003826 RID: 14374
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x04003827 RID: 14375
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04003828 RID: 14376
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04003829 RID: 14377
	private bool prioritizableAdded;

	// Token: 0x0400382A RID: 14378
	private SchedulerHandle updateHandle;

	// Token: 0x0400382B RID: 14379
	private bool hasReservedMaterial;
}
