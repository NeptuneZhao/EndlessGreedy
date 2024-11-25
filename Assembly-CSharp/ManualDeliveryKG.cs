using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000955 RID: 2389
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ManualDeliveryKG")]
public class ManualDeliveryKG : KMonoBehaviour, ISim1000ms
{
	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x060045C1 RID: 17857 RVA: 0x0018D0AD File Offset: 0x0018B2AD
	public bool IsPaused
	{
		get
		{
			return this.paused;
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x060045C2 RID: 17858 RVA: 0x0018D0B5 File Offset: 0x0018B2B5
	public float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x060045C3 RID: 17859 RVA: 0x0018D0BD File Offset: 0x0018B2BD
	// (set) Token: 0x060045C4 RID: 17860 RVA: 0x0018D0C5 File Offset: 0x0018B2C5
	public Tag RequestedItemTag
	{
		get
		{
			return this.requestedItemTag;
		}
		set
		{
			this.requestedItemTag = value;
			this.AbortDelivery("Requested Item Tag Changed");
		}
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x060045C5 RID: 17861 RVA: 0x0018D0D9 File Offset: 0x0018B2D9
	// (set) Token: 0x060045C6 RID: 17862 RVA: 0x0018D0E1 File Offset: 0x0018B2E1
	public Tag[] ForbiddenTags
	{
		get
		{
			return this.forbiddenTags;
		}
		set
		{
			this.forbiddenTags = value;
			this.AbortDelivery("Forbidden Tags Changed");
		}
	}

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x060045C7 RID: 17863 RVA: 0x0018D0F5 File Offset: 0x0018B2F5
	public Storage DebugStorage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x060045C8 RID: 17864 RVA: 0x0018D0FD File Offset: 0x0018B2FD
	public FetchList2 DebugFetchList
	{
		get
		{
			return this.fetchList;
		}
	}

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x060045C9 RID: 17865 RVA: 0x0018D105 File Offset: 0x0018B305
	private float MassStoredPerUnit
	{
		get
		{
			return this.storage.GetMassAvailable(this.requestedItemTag) / this.MassPerUnit;
		}
	}

	// Token: 0x060045CA RID: 17866 RVA: 0x0018D120 File Offset: 0x0018B320
	protected override void OnSpawn()
	{
		base.OnSpawn();
		DebugUtil.Assert(this.choreTypeIDHash.IsValid, "ManualDeliveryKG Must have a valid chore type specified!", base.name);
		if (this.allowPause)
		{
			base.Subscribe<ManualDeliveryKG>(493375141, ManualDeliveryKG.OnRefreshUserMenuDelegate);
			base.Subscribe<ManualDeliveryKG>(-111137758, ManualDeliveryKG.OnRefreshUserMenuDelegate);
		}
		base.Subscribe<ManualDeliveryKG>(-592767678, ManualDeliveryKG.OnOperationalChangedDelegate);
		if (this.storage != null)
		{
			this.SetStorage(this.storage);
		}
		Prioritizable.AddRef(base.gameObject);
		if (this.userPaused && this.allowPause)
		{
			this.OnPause();
		}
	}

	// Token: 0x060045CB RID: 17867 RVA: 0x0018D1C4 File Offset: 0x0018B3C4
	protected override void OnCleanUp()
	{
		this.AbortDelivery("ManualDeliverKG destroyed");
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x060045CC RID: 17868 RVA: 0x0018D1E4 File Offset: 0x0018B3E4
	public void SetStorage(Storage storage)
	{
		if (this.storage != null)
		{
			this.storage.Unsubscribe(this.onStorageChangeSubscription);
			this.onStorageChangeSubscription = -1;
		}
		this.AbortDelivery("storage pointer changed");
		this.storage = storage;
		if (this.storage != null && base.isSpawned)
		{
			global::Debug.Assert(this.onStorageChangeSubscription == -1);
			this.onStorageChangeSubscription = this.storage.Subscribe<ManualDeliveryKG>(-1697596308, ManualDeliveryKG.OnStorageChangedDelegate);
		}
	}

	// Token: 0x060045CD RID: 17869 RVA: 0x0018D268 File Offset: 0x0018B468
	public void Pause(bool pause, string reason)
	{
		if (this.paused != pause)
		{
			this.paused = pause;
			if (pause)
			{
				this.AbortDelivery(reason);
			}
		}
	}

	// Token: 0x060045CE RID: 17870 RVA: 0x0018D284 File Offset: 0x0018B484
	public void Sim1000ms(float dt)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x060045CF RID: 17871 RVA: 0x0018D28C File Offset: 0x0018B48C
	[ContextMenu("UpdateDeliveryState")]
	public void UpdateDeliveryState()
	{
		if (!this.requestedItemTag.IsValid)
		{
			return;
		}
		if (this.storage == null)
		{
			return;
		}
		this.UpdateFetchList();
	}

	// Token: 0x060045D0 RID: 17872 RVA: 0x0018D2B4 File Offset: 0x0018B4B4
	public void RequestDelivery()
	{
		if (this.fetchList != null)
		{
			return;
		}
		float massStoredPerUnit = this.MassStoredPerUnit;
		if (massStoredPerUnit < this.capacity)
		{
			this.CreateFetchChore(massStoredPerUnit);
		}
	}

	// Token: 0x060045D1 RID: 17873 RVA: 0x0018D2E4 File Offset: 0x0018B4E4
	private void CreateFetchChore(float stored_mass)
	{
		float num = this.capacity - stored_mass;
		num = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, num);
		if (this.RoundFetchAmountToInt)
		{
			num = (float)((int)num);
			if (num < 0.1f)
			{
				return;
			}
		}
		ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.choreTypeIDHash);
		this.fetchList = new FetchList2(this.storage, byHash);
		this.fetchList.ShowStatusItem = this.ShowStatusItem;
		this.fetchList.MinimumAmount[this.requestedItemTag] = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, this.MinimumMass);
		FetchList2 fetchList = this.fetchList;
		Tag tag = this.requestedItemTag;
		float amount = num;
		fetchList.Add(tag, this.forbiddenTags, amount, Operational.State.None);
		this.fetchList.Submit(new System.Action(this.OnFetchComplete), false);
	}

	// Token: 0x060045D2 RID: 17874 RVA: 0x0018D3B0 File Offset: 0x0018B5B0
	private void OnFetchComplete()
	{
		if (this.FillToCapacity && this.storage != null)
		{
			float amountAvailable = this.storage.GetAmountAvailable(this.requestedItemTag);
			if (amountAvailable < this.capacity)
			{
				this.CreateFetchChore(amountAvailable);
			}
		}
	}

	// Token: 0x060045D3 RID: 17875 RVA: 0x0018D3F8 File Offset: 0x0018B5F8
	private void UpdateFetchList()
	{
		if (this.paused)
		{
			return;
		}
		if (this.fetchList != null && this.fetchList.IsComplete)
		{
			this.fetchList = null;
		}
		if (!(this.operational == null) && !this.operational.MeetsRequirements(this.operationalRequirement))
		{
			if (this.fetchList != null)
			{
				this.fetchList.Cancel("Operational requirements");
				this.fetchList = null;
				return;
			}
		}
		else if (this.fetchList == null && this.MassStoredPerUnit < this.refillMass)
		{
			this.RequestDelivery();
		}
	}

	// Token: 0x060045D4 RID: 17876 RVA: 0x0018D487 File Offset: 0x0018B687
	public void AbortDelivery(string reason)
	{
		if (this.fetchList != null)
		{
			FetchList2 fetchList = this.fetchList;
			this.fetchList = null;
			fetchList.Cancel(reason);
		}
	}

	// Token: 0x060045D5 RID: 17877 RVA: 0x0018D4A4 File Offset: 0x0018B6A4
	protected void OnStorageChanged(object data)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x060045D6 RID: 17878 RVA: 0x0018D4AC File Offset: 0x0018B6AC
	private void OnPause()
	{
		this.userPaused = true;
		this.Pause(true, "Forbid manual delivery");
	}

	// Token: 0x060045D7 RID: 17879 RVA: 0x0018D4C1 File Offset: 0x0018B6C1
	private void OnResume()
	{
		this.userPaused = false;
		this.Pause(false, "Allow manual delivery");
	}

	// Token: 0x060045D8 RID: 17880 RVA: 0x0018D4D8 File Offset: 0x0018B6D8
	private void OnRefreshUserMenu(object data)
	{
		if (!this.allowPause)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.paused) ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME, new System.Action(this.OnPause), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME_OFF, new System.Action(this.OnResume), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x060045D9 RID: 17881 RVA: 0x0018D57A File Offset: 0x0018B77A
	private void OnOperationalChanged(object data)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x04002D5D RID: 11613
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002D5E RID: 11614
	[SerializeField]
	private Storage storage;

	// Token: 0x04002D5F RID: 11615
	[SerializeField]
	public Tag requestedItemTag;

	// Token: 0x04002D60 RID: 11616
	private Tag[] forbiddenTags;

	// Token: 0x04002D61 RID: 11617
	[SerializeField]
	public float capacity = 100f;

	// Token: 0x04002D62 RID: 11618
	[SerializeField]
	public float refillMass = 10f;

	// Token: 0x04002D63 RID: 11619
	[SerializeField]
	public float MinimumMass = 10f;

	// Token: 0x04002D64 RID: 11620
	[SerializeField]
	public bool RoundFetchAmountToInt;

	// Token: 0x04002D65 RID: 11621
	[SerializeField]
	public float MassPerUnit = 1f;

	// Token: 0x04002D66 RID: 11622
	[SerializeField]
	public bool FillToCapacity;

	// Token: 0x04002D67 RID: 11623
	[SerializeField]
	public Operational.State operationalRequirement;

	// Token: 0x04002D68 RID: 11624
	[SerializeField]
	public bool allowPause;

	// Token: 0x04002D69 RID: 11625
	[SerializeField]
	private bool paused;

	// Token: 0x04002D6A RID: 11626
	[SerializeField]
	public HashedString choreTypeIDHash;

	// Token: 0x04002D6B RID: 11627
	[Serialize]
	private bool userPaused;

	// Token: 0x04002D6C RID: 11628
	public bool ShowStatusItem = true;

	// Token: 0x04002D6D RID: 11629
	private FetchList2 fetchList;

	// Token: 0x04002D6E RID: 11630
	private int onStorageChangeSubscription = -1;

	// Token: 0x04002D6F RID: 11631
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04002D70 RID: 11632
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04002D71 RID: 11633
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnStorageChanged(data);
	});
}
