using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AB7 RID: 2743
public class CargoBayCluster : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x060050DD RID: 20701 RVA: 0x001D0C75 File Offset: 0x001CEE75
	// (set) Token: 0x060050DE RID: 20702 RVA: 0x001D0C7D File Offset: 0x001CEE7D
	public float UserMaxCapacity
	{
		get
		{
			return this.userMaxCapacity;
		}
		set
		{
			this.userMaxCapacity = value;
			base.Trigger(-945020481, this);
		}
	}

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x060050DF RID: 20703 RVA: 0x001D0C92 File Offset: 0x001CEE92
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x060050E0 RID: 20704 RVA: 0x001D0C99 File Offset: 0x001CEE99
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x060050E1 RID: 20705 RVA: 0x001D0CA6 File Offset: 0x001CEEA6
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x060050E2 RID: 20706 RVA: 0x001D0CB3 File Offset: 0x001CEEB3
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x060050E3 RID: 20707 RVA: 0x001D0CB6 File Offset: 0x001CEEB6
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x060050E4 RID: 20708 RVA: 0x001D0CBE File Offset: 0x001CEEBE
	public float RemainingCapacity
	{
		get
		{
			return this.userMaxCapacity - this.storage.MassStored();
		}
	}

	// Token: 0x060050E5 RID: 20709 RVA: 0x001D0CD2 File Offset: 0x001CEED2
	protected override void OnPrefabInit()
	{
		this.userMaxCapacity = this.storage.capacityKg;
	}

	// Token: 0x060050E6 RID: 20710 RVA: 0x001D0CE8 File Offset: 0x001CEEE8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		base.Subscribe<CargoBayCluster>(493375141, CargoBayCluster.OnRefreshUserMenuDelegate);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		KBatchedAnimTracker component = this.meter.gameObject.GetComponent<KBatchedAnimTracker>();
		component.matchParentOffset = true;
		component.forceAlwaysAlive = true;
		this.OnStorageChange(null);
		base.Subscribe<CargoBayCluster>(-1697596308, CargoBayCluster.OnStorageChangeDelegate);
	}

	// Token: 0x060050E7 RID: 20711 RVA: 0x001D0DA8 File Offset: 0x001CEFA8
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, delegate()
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x060050E8 RID: 20712 RVA: 0x001D0E04 File Offset: 0x001CF004
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
		this.UpdateCargoStatusItem();
	}

	// Token: 0x060050E9 RID: 20713 RVA: 0x001D0E30 File Offset: 0x001CF030
	private void UpdateCargoStatusItem()
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component == null)
		{
			return;
		}
		CraftModuleInterface craftInterface = component.CraftInterface;
		if (craftInterface == null)
		{
			return;
		}
		Clustercraft component2 = craftInterface.GetComponent<Clustercraft>();
		if (component2 == null)
		{
			return;
		}
		component2.UpdateStatusItem();
	}

	// Token: 0x040035BA RID: 13754
	private MeterController meter;

	// Token: 0x040035BB RID: 13755
	[SerializeField]
	public Storage storage;

	// Token: 0x040035BC RID: 13756
	[SerializeField]
	public CargoBay.CargoType storageType;

	// Token: 0x040035BD RID: 13757
	[Serialize]
	private float userMaxCapacity;

	// Token: 0x040035BE RID: 13758
	private static readonly EventSystem.IntraObjectHandler<CargoBayCluster> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CargoBayCluster>(delegate(CargoBayCluster component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040035BF RID: 13759
	private static readonly EventSystem.IntraObjectHandler<CargoBayCluster> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CargoBayCluster>(delegate(CargoBayCluster component, object data)
	{
		component.OnStorageChange(data);
	});
}
