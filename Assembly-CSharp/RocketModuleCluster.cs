using System;
using UnityEngine;

// Token: 0x02000AE2 RID: 2786
public class RocketModuleCluster : RocketModule
{
	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x060052CC RID: 21196 RVA: 0x001DB0DD File Offset: 0x001D92DD
	// (set) Token: 0x060052CD RID: 21197 RVA: 0x001DB0E5 File Offset: 0x001D92E5
	public CraftModuleInterface CraftInterface
	{
		get
		{
			return this._craftInterface;
		}
		set
		{
			this._craftInterface = value;
			if (this._craftInterface != null)
			{
				base.name = base.name + ": " + this.GetParentRocketName();
			}
		}
	}

	// Token: 0x060052CE RID: 21198 RVA: 0x001DB118 File Offset: 0x001D9318
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<RocketModuleCluster>(2121280625, RocketModuleCluster.OnNewConstructionDelegate);
	}

	// Token: 0x060052CF RID: 21199 RVA: 0x001DB134 File Offset: 0x001D9334
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.CraftInterface == null && DlcManager.FeatureClusterSpaceEnabled())
		{
			this.RegisterWithCraftModuleInterface();
		}
		if (base.GetComponent<RocketEngine>() == null && base.GetComponent<RocketEngineCluster>() == null && base.GetComponent<BuildingUnderConstruction>() == null)
		{
			base.Subscribe<RocketModuleCluster>(1655598572, RocketModuleCluster.OnLaunchConditionChangedDelegate);
			base.Subscribe<RocketModuleCluster>(-887025858, RocketModuleCluster.OnLandDelegate);
		}
	}

	// Token: 0x060052D0 RID: 21200 RVA: 0x001DB1B0 File Offset: 0x001D93B0
	protected void OnNewConstruction(object data)
	{
		Constructable constructable = (Constructable)data;
		if (constructable == null)
		{
			return;
		}
		RocketModuleCluster component = constructable.GetComponent<RocketModuleCluster>();
		if (component == null)
		{
			return;
		}
		if (component.CraftInterface != null)
		{
			component.CraftInterface.AddModule(this);
		}
	}

	// Token: 0x060052D1 RID: 21201 RVA: 0x001DB1FC File Offset: 0x001D93FC
	private void RegisterWithCraftModuleInterface()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			if (!(gameObject == base.gameObject))
			{
				RocketModuleCluster component = gameObject.GetComponent<RocketModuleCluster>();
				if (component != null)
				{
					component.CraftInterface.AddModule(this);
					break;
				}
			}
		}
	}

	// Token: 0x060052D2 RID: 21202 RVA: 0x001DB27C File Offset: 0x001D947C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.CraftInterface.RemoveModule(this);
	}

	// Token: 0x060052D3 RID: 21203 RVA: 0x001DB290 File Offset: 0x001D9490
	public override LaunchConditionManager FindLaunchConditionManager()
	{
		return this.CraftInterface.FindLaunchConditionManager();
	}

	// Token: 0x060052D4 RID: 21204 RVA: 0x001DB29D File Offset: 0x001D949D
	public override string GetParentRocketName()
	{
		if (this.CraftInterface != null)
		{
			return this.CraftInterface.GetComponent<Clustercraft>().Name;
		}
		return this.parentRocketName;
	}

	// Token: 0x060052D5 RID: 21205 RVA: 0x001DB2C4 File Offset: 0x001D94C4
	private void OnLaunchConditionChanged(object data)
	{
		this.UpdateAnimations();
	}

	// Token: 0x060052D6 RID: 21206 RVA: 0x001DB2CC File Offset: 0x001D94CC
	private void OnLand(object data)
	{
		this.UpdateAnimations();
	}

	// Token: 0x060052D7 RID: 21207 RVA: 0x001DB2D4 File Offset: 0x001D94D4
	protected void UpdateAnimations()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Clustercraft clustercraft = (this.CraftInterface == null) ? null : this.CraftInterface.GetComponent<Clustercraft>();
		if (clustercraft != null && clustercraft.Status == Clustercraft.CraftStatus.Launching && component.HasAnimation("launch"))
		{
			component.ClearQueue();
			if (component.HasAnimation("launch_pre"))
			{
				component.Play("launch_pre", KAnim.PlayMode.Once, 1f, 0f);
			}
			component.Queue("launch", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (this.CraftInterface != null && this.CraftInterface.CheckPreppedForLaunch())
		{
			component.initialAnim = "ready_to_launch";
			component.Play("pre_ready_to_launch", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("ready_to_launch", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		component.initialAnim = "grounded";
		component.Play("pst_ready_to_launch", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("grounded", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x040036B2 RID: 14002
	public RocketModulePerformance performanceStats;

	// Token: 0x040036B3 RID: 14003
	private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnNewConstructionDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>(delegate(RocketModuleCluster component, object data)
	{
		component.OnNewConstruction(data);
	});

	// Token: 0x040036B4 RID: 14004
	private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnLaunchConditionChangedDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>(delegate(RocketModuleCluster component, object data)
	{
		component.OnLaunchConditionChanged(data);
	});

	// Token: 0x040036B5 RID: 14005
	private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnLandDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>(delegate(RocketModuleCluster component, object data)
	{
		component.OnLand(data);
	});

	// Token: 0x040036B6 RID: 14006
	private CraftModuleInterface _craftInterface;
}
