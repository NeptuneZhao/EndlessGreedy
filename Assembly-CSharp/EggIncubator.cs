using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020006C2 RID: 1730
[SerializationConfig(MemberSerialization.OptIn)]
public class EggIncubator : SingleEntityReceptacle, ISaveLoadable, ISim1000ms
{
	// Token: 0x06002BB8 RID: 11192 RVA: 0x000F597C File Offset: 0x000F3B7C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoReplaceEntity = true;
		this.choreType = Db.Get().ChoreTypes.RanchingFetch;
		this.statusItemNeed = Db.Get().BuildingStatusItems.NeedEgg;
		this.statusItemNoneAvailable = Db.Get().BuildingStatusItems.NoAvailableEgg;
		this.statusItemAwaitingDelivery = Db.Get().BuildingStatusItems.AwaitingEggDelivery;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.occupyingObjectRelativePosition = new Vector3(0.5f, 1f, -1f);
		this.synchronizeAnims = false;
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("egg_target", false);
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.Subscribe<EggIncubator>(-905833192, EggIncubator.OnCopySettingsDelegate);
	}

	// Token: 0x06002BB9 RID: 11193 RVA: 0x000F5A60 File Offset: 0x000F3C60
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.occupyingObject)
		{
			if (base.occupyingObject.HasTag(GameTags.Creature))
			{
				this.storage.allowItemRemoval = true;
			}
			this.storage.RenotifyAll();
			this.PositionOccupyingObject();
		}
		base.Subscribe<EggIncubator>(-592767678, EggIncubator.OnOperationalChangedDelegate);
		base.Subscribe<EggIncubator>(-731304873, EggIncubator.OnOccupantChangedDelegate);
		base.Subscribe<EggIncubator>(-1697596308, EggIncubator.OnStorageChangeDelegate);
		this.smi = new EggIncubatorStates.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x06002BBA RID: 11194 RVA: 0x000F5AFC File Offset: 0x000F3CFC
	private void OnCopySettings(object data)
	{
		EggIncubator component = ((GameObject)data).GetComponent<EggIncubator>();
		if (component != null)
		{
			this.autoReplaceEntity = component.autoReplaceEntity;
			if (base.occupyingObject == null)
			{
				if (!(this.requestedEntityTag == component.requestedEntityTag) || !(this.requestedEntityAdditionalFilterTag == component.requestedEntityAdditionalFilterTag))
				{
					base.CancelActiveRequest();
				}
				if (this.fetchChore == null)
				{
					Tag requestedEntityTag = component.requestedEntityTag;
					this.CreateOrder(requestedEntityTag, component.requestedEntityAdditionalFilterTag);
				}
			}
			if (base.occupyingObject != null)
			{
				Prioritizable component2 = base.GetComponent<Prioritizable>();
				if (component2 != null)
				{
					Prioritizable component3 = base.occupyingObject.GetComponent<Prioritizable>();
					if (component3 != null)
					{
						component3.SetMasterPriority(component2.GetMasterPriority());
					}
				}
			}
		}
	}

	// Token: 0x06002BBB RID: 11195 RVA: 0x000F5BC5 File Offset: 0x000F3DC5
	protected override void OnCleanUp()
	{
		this.smi.StopSM("cleanup");
		base.OnCleanUp();
	}

	// Token: 0x06002BBC RID: 11196 RVA: 0x000F5BE0 File Offset: 0x000F3DE0
	protected override void SubscribeToOccupant()
	{
		base.SubscribeToOccupant();
		if (base.occupyingObject != null)
		{
			this.tracker = base.occupyingObject.AddComponent<KBatchedAnimTracker>();
			this.tracker.symbol = "egg_target";
			this.tracker.forceAlwaysVisible = true;
		}
		this.UpdateProgress();
	}

	// Token: 0x06002BBD RID: 11197 RVA: 0x000F5C39 File Offset: 0x000F3E39
	protected override void UnsubscribeFromOccupant()
	{
		base.UnsubscribeFromOccupant();
		UnityEngine.Object.Destroy(this.tracker);
		this.tracker = null;
		this.UpdateProgress();
	}

	// Token: 0x06002BBE RID: 11198 RVA: 0x000F5C5C File Offset: 0x000F3E5C
	private void OnOperationalChanged(object data = null)
	{
		if (!base.occupyingObject)
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x06002BBF RID: 11199 RVA: 0x000F5C8E File Offset: 0x000F3E8E
	private void OnOccupantChanged(object data = null)
	{
		if (!base.occupyingObject)
		{
			this.storage.allowItemRemoval = false;
		}
	}

	// Token: 0x06002BC0 RID: 11200 RVA: 0x000F5CA9 File Offset: 0x000F3EA9
	private void OnStorageChange(object data = null)
	{
		if (base.occupyingObject && !this.storage.items.Contains(base.occupyingObject))
		{
			this.UnsubscribeFromOccupant();
			this.ClearOccupant();
		}
	}

	// Token: 0x06002BC1 RID: 11201 RVA: 0x000F5CDC File Offset: 0x000F3EDC
	protected override void ClearOccupant()
	{
		bool flag = false;
		if (base.occupyingObject != null)
		{
			flag = !base.occupyingObject.HasTag(GameTags.Egg);
		}
		base.ClearOccupant();
		if (this.autoReplaceEntity && flag && this.requestedEntityTag.IsValid)
		{
			this.CreateOrder(this.requestedEntityTag, Tag.Invalid);
		}
	}

	// Token: 0x06002BC2 RID: 11202 RVA: 0x000F5D3C File Offset: 0x000F3F3C
	protected override void PositionOccupyingObject()
	{
		base.PositionOccupyingObject();
		base.occupyingObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
		KSelectable component = base.occupyingObject.GetComponent<KSelectable>();
		if (component != null)
		{
			component.IsSelectable = true;
		}
	}

	// Token: 0x06002BC3 RID: 11203 RVA: 0x000F5D80 File Offset: 0x000F3F80
	public override void OrderRemoveOccupant()
	{
		UnityEngine.Object.Destroy(this.tracker);
		this.tracker = null;
		this.storage.DropAll(false, false, default(Vector3), true, null);
		base.occupyingObject = null;
		this.ClearOccupant();
	}

	// Token: 0x06002BC4 RID: 11204 RVA: 0x000F5DC4 File Offset: 0x000F3FC4
	public float GetProgress()
	{
		float result = 0f;
		if (base.occupyingObject)
		{
			AmountInstance amountInstance = base.occupyingObject.GetAmounts().Get(Db.Get().Amounts.Incubation);
			if (amountInstance != null)
			{
				result = amountInstance.value / amountInstance.GetMax();
			}
			else
			{
				result = 1f;
			}
		}
		return result;
	}

	// Token: 0x06002BC5 RID: 11205 RVA: 0x000F5E1E File Offset: 0x000F401E
	private void UpdateProgress()
	{
		this.meter.SetPositionPercent(this.GetProgress());
	}

	// Token: 0x06002BC6 RID: 11206 RVA: 0x000F5E31 File Offset: 0x000F4031
	public void Sim1000ms(float dt)
	{
		this.UpdateProgress();
		this.UpdateChore();
	}

	// Token: 0x06002BC7 RID: 11207 RVA: 0x000F5E40 File Offset: 0x000F4040
	public void StoreBaby(GameObject baby)
	{
		this.UnsubscribeFromOccupant();
		this.storage.DropAll(false, false, default(Vector3), true, null);
		this.storage.allowItemRemoval = true;
		this.storage.Store(baby, false, false, true, false);
		base.occupyingObject = baby;
		this.SubscribeToOccupant();
		base.Trigger(-731304873, base.occupyingObject);
	}

	// Token: 0x06002BC8 RID: 11208 RVA: 0x000F5EA8 File Offset: 0x000F40A8
	private void UpdateChore()
	{
		if (this.operational.IsOperational && this.EggNeedsAttention())
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<EggIncubatorWorkable>(Db.Get().ChoreTypes.EggSing, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("now is not the time for song");
			this.chore = null;
		}
	}

	// Token: 0x06002BC9 RID: 11209 RVA: 0x000F5F24 File Offset: 0x000F4124
	private bool EggNeedsAttention()
	{
		if (!base.Occupant)
		{
			return false;
		}
		IncubationMonitor.Instance instance = base.Occupant.GetSMI<IncubationMonitor.Instance>();
		return instance != null && !instance.HasSongBuff();
	}

	// Token: 0x04001925 RID: 6437
	[MyCmpAdd]
	private EggIncubatorWorkable workable;

	// Token: 0x04001926 RID: 6438
	[MyCmpAdd]
	private CopyBuildingSettings copySettings;

	// Token: 0x04001927 RID: 6439
	private Chore chore;

	// Token: 0x04001928 RID: 6440
	private EggIncubatorStates.Instance smi;

	// Token: 0x04001929 RID: 6441
	private KBatchedAnimTracker tracker;

	// Token: 0x0400192A RID: 6442
	private MeterController meter;

	// Token: 0x0400192B RID: 6443
	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x0400192C RID: 6444
	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnOccupantChanged(data);
	});

	// Token: 0x0400192D RID: 6445
	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x0400192E RID: 6446
	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnCopySettings(data);
	});
}
