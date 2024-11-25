using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000ADC RID: 2780
public class RocketClusterDestinationSelector : ClusterDestinationSelector
{
	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x0600529B RID: 21147 RVA: 0x001D9C9D File Offset: 0x001D7E9D
	// (set) Token: 0x0600529C RID: 21148 RVA: 0x001D9CA5 File Offset: 0x001D7EA5
	public bool Repeat
	{
		get
		{
			return this.m_repeat;
		}
		set
		{
			this.m_repeat = value;
		}
	}

	// Token: 0x0600529D RID: 21149 RVA: 0x001D9CAE File Offset: 0x001D7EAE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<RocketClusterDestinationSelector>(-1277991738, this.OnLaunchDelegate);
	}

	// Token: 0x0600529E RID: 21150 RVA: 0x001D9CC8 File Offset: 0x001D7EC8
	protected override void OnSpawn()
	{
		if (this.isHarvesting)
		{
			this.WaitForPOIHarvest();
		}
	}

	// Token: 0x0600529F RID: 21151 RVA: 0x001D9CD8 File Offset: 0x001D7ED8
	public LaunchPad GetDestinationPad(AxialI destination)
	{
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
		if (this.m_launchPad.ContainsKey(asteroidWorldIdAtLocation))
		{
			return this.m_launchPad[asteroidWorldIdAtLocation].Get();
		}
		return null;
	}

	// Token: 0x060052A0 RID: 21152 RVA: 0x001D9D0D File Offset: 0x001D7F0D
	public LaunchPad GetDestinationPad()
	{
		return this.GetDestinationPad(this.m_destination);
	}

	// Token: 0x060052A1 RID: 21153 RVA: 0x001D9D1B File Offset: 0x001D7F1B
	public override void SetDestination(AxialI location)
	{
		base.SetDestination(location);
	}

	// Token: 0x060052A2 RID: 21154 RVA: 0x001D9D24 File Offset: 0x001D7F24
	public void SetDestinationPad(LaunchPad pad)
	{
		Debug.Assert(pad == null || ClusterGrid.Instance.IsInRange(pad.GetMyWorldLocation(), this.m_destination, 1), "Tried sending a rocket to a launchpad that wasn't its destination world.");
		if (pad != null)
		{
			this.AddDestinationPad(pad.GetMyWorldLocation(), pad);
			base.SetDestination(pad.GetMyWorldLocation());
		}
		base.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.ClusterDestinationChanged, null);
	}

	// Token: 0x060052A3 RID: 21155 RVA: 0x001D9D94 File Offset: 0x001D7F94
	private void AddDestinationPad(AxialI location, LaunchPad pad)
	{
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(location);
		if (asteroidWorldIdAtLocation < 0)
		{
			return;
		}
		if (!this.m_launchPad.ContainsKey(asteroidWorldIdAtLocation))
		{
			this.m_launchPad.Add(asteroidWorldIdAtLocation, new Ref<LaunchPad>());
		}
		this.m_launchPad[asteroidWorldIdAtLocation].Set(pad);
	}

	// Token: 0x060052A4 RID: 21156 RVA: 0x001D9DE0 File Offset: 0x001D7FE0
	protected override void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		if (clusterLocationChangedEvent.newLocation == this.m_destination)
		{
			base.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.ClusterDestinationReached, null);
			if (this.m_repeat)
			{
				if (ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(clusterLocationChangedEvent.newLocation, EntityLayer.POI) != null && this.CanRocketHarvest())
				{
					this.WaitForPOIHarvest();
					return;
				}
				this.SetUpReturnTrip();
			}
		}
	}

	// Token: 0x060052A5 RID: 21157 RVA: 0x001D9E54 File Offset: 0x001D8054
	private void SetUpReturnTrip()
	{
		this.AddDestinationPad(this.m_prevDestination, this.m_prevLaunchPad.Get());
		this.m_destination = this.m_prevDestination;
		this.m_prevDestination = base.GetComponent<Clustercraft>().Location;
		this.m_prevLaunchPad.Set(base.GetComponent<CraftModuleInterface>().CurrentPad);
	}

	// Token: 0x060052A6 RID: 21158 RVA: 0x001D9EAC File Offset: 0x001D80AC
	private bool CanRocketHarvest()
	{
		bool flag = false;
		List<ResourceHarvestModule.StatesInstance> allResourceHarvestModules = base.GetComponent<Clustercraft>().GetAllResourceHarvestModules();
		if (allResourceHarvestModules.Count > 0)
		{
			using (List<ResourceHarvestModule.StatesInstance>.Enumerator enumerator = allResourceHarvestModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CheckIfCanHarvest())
					{
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			List<ArtifactHarvestModule.StatesInstance> allArtifactHarvestModules = base.GetComponent<Clustercraft>().GetAllArtifactHarvestModules();
			if (allArtifactHarvestModules.Count > 0)
			{
				using (List<ArtifactHarvestModule.StatesInstance>.Enumerator enumerator2 = allArtifactHarvestModules.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.CheckIfCanHarvest())
						{
							flag = true;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x060052A7 RID: 21159 RVA: 0x001D9F6C File Offset: 0x001D816C
	private void OnStorageChange(object data)
	{
		if (!this.CanRocketHarvest())
		{
			this.isHarvesting = false;
			foreach (Ref<RocketModuleCluster> @ref in base.GetComponent<Clustercraft>().ModuleInterface.ClusterModules)
			{
				if (@ref.Get().GetComponent<Storage>())
				{
					base.Unsubscribe(@ref.Get().gameObject, -1697596308, new Action<object>(this.OnStorageChange));
				}
			}
			this.SetUpReturnTrip();
		}
	}

	// Token: 0x060052A8 RID: 21160 RVA: 0x001DA008 File Offset: 0x001D8208
	private void WaitForPOIHarvest()
	{
		this.isHarvesting = true;
		foreach (Ref<RocketModuleCluster> @ref in base.GetComponent<Clustercraft>().ModuleInterface.ClusterModules)
		{
			if (@ref.Get().GetComponent<Storage>())
			{
				base.Subscribe(@ref.Get().gameObject, -1697596308, new Action<object>(this.OnStorageChange));
			}
		}
	}

	// Token: 0x060052A9 RID: 21161 RVA: 0x001DA094 File Offset: 0x001D8294
	private void OnLaunch(object data)
	{
		CraftModuleInterface component = base.GetComponent<CraftModuleInterface>();
		this.m_prevLaunchPad.Set(component.CurrentPad);
		Clustercraft component2 = base.GetComponent<Clustercraft>();
		this.m_prevDestination = component2.Location;
	}

	// Token: 0x04003678 RID: 13944
	[Serialize]
	private Dictionary<int, Ref<LaunchPad>> m_launchPad = new Dictionary<int, Ref<LaunchPad>>();

	// Token: 0x04003679 RID: 13945
	[Serialize]
	private bool m_repeat;

	// Token: 0x0400367A RID: 13946
	[Serialize]
	private AxialI m_prevDestination;

	// Token: 0x0400367B RID: 13947
	[Serialize]
	private Ref<LaunchPad> m_prevLaunchPad = new Ref<LaunchPad>();

	// Token: 0x0400367C RID: 13948
	[Serialize]
	private bool isHarvesting;

	// Token: 0x0400367D RID: 13949
	private EventSystem.IntraObjectHandler<RocketClusterDestinationSelector> OnLaunchDelegate = new EventSystem.IntraObjectHandler<RocketClusterDestinationSelector>(delegate(RocketClusterDestinationSelector cmp, object data)
	{
		cmp.OnLaunch(data);
	});
}
