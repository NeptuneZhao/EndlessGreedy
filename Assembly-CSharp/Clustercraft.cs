﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000ABF RID: 2751
public class Clustercraft : ClusterGridEntity, IClusterRange, ISim4000ms, ISim1000ms
{
	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x06005132 RID: 20786 RVA: 0x001D240E File Offset: 0x001D060E
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x06005133 RID: 20787 RVA: 0x001D2416 File Offset: 0x001D0616
	// (set) Token: 0x06005134 RID: 20788 RVA: 0x001D241E File Offset: 0x001D061E
	public bool Exploding { get; protected set; }

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x06005135 RID: 20789 RVA: 0x001D2427 File Offset: 0x001D0627
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Craft;
		}
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x06005136 RID: 20790 RVA: 0x001D242C File Offset: 0x001D062C
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("rocket01_kanim"),
					initialAnim = "idle_loop"
				}
			};
		}
	}

	// Token: 0x06005137 RID: 20791 RVA: 0x001D2470 File Offset: 0x001D0670
	public override Sprite GetUISprite()
	{
		PassengerRocketModule passengerModule = this.m_moduleInterface.GetPassengerModule();
		if (passengerModule != null)
		{
			return Def.GetUISprite(passengerModule.gameObject, "ui", false).first;
		}
		return Assets.GetSprite("ic_rocket");
	}

	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x06005138 RID: 20792 RVA: 0x001D24B8 File Offset: 0x001D06B8
	public override bool IsVisible
	{
		get
		{
			return !this.Exploding;
		}
	}

	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x06005139 RID: 20793 RVA: 0x001D24C3 File Offset: 0x001D06C3
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x0600513A RID: 20794 RVA: 0x001D24C6 File Offset: 0x001D06C6
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x0600513B RID: 20795 RVA: 0x001D24C9 File Offset: 0x001D06C9
	public CraftModuleInterface ModuleInterface
	{
		get
		{
			return this.m_moduleInterface;
		}
	}

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x0600513C RID: 20796 RVA: 0x001D24D1 File Offset: 0x001D06D1
	public AxialI Destination
	{
		get
		{
			return this.m_moduleInterface.GetClusterDestinationSelector().GetDestination();
		}
	}

	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x0600513D RID: 20797 RVA: 0x001D24E4 File Offset: 0x001D06E4
	public float Speed
	{
		get
		{
			float num = this.EnginePower / this.TotalBurden;
			float num2 = num * this.AutoPilotMultiplier * this.PilotSkillMultiplier;
			float num3 = 1f;
			RoboPilotModule robotPilotModule = this.ModuleInterface.GetRobotPilotModule();
			if (robotPilotModule != null)
			{
				num3 += robotPilotModule.FlightEfficiencyModifier();
			}
			num2 *= num3;
			if (this.controlStationBuffTimeRemaining > 0f)
			{
				num2 += num * 0.20000005f;
			}
			return num2;
		}
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x0600513E RID: 20798 RVA: 0x001D2550 File Offset: 0x001D0750
	public float EnginePower
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
			{
				num += @ref.Get().performanceStats.EnginePower;
			}
			return num;
		}
	}

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x0600513F RID: 20799 RVA: 0x001D25B8 File Offset: 0x001D07B8
	public float FuelPerDistance
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
			{
				num += @ref.Get().performanceStats.FuelKilogramPerDistance;
			}
			return num;
		}
	}

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x06005140 RID: 20800 RVA: 0x001D2620 File Offset: 0x001D0820
	public float TotalBurden
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
			{
				num += @ref.Get().performanceStats.Burden;
			}
			global::Debug.Assert(num > 0f);
			return num;
		}
	}

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x06005141 RID: 20801 RVA: 0x001D2694 File Offset: 0x001D0894
	// (set) Token: 0x06005142 RID: 20802 RVA: 0x001D269C File Offset: 0x001D089C
	public bool LaunchRequested
	{
		get
		{
			return this.m_launchRequested;
		}
		private set
		{
			this.m_launchRequested = value;
			this.m_moduleInterface.TriggerEventOnCraftAndRocket(GameHashes.RocketRequestLaunch, this);
		}
	}

	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x06005143 RID: 20803 RVA: 0x001D26B6 File Offset: 0x001D08B6
	public Clustercraft.CraftStatus Status
	{
		get
		{
			return this.status;
		}
	}

	// Token: 0x06005144 RID: 20804 RVA: 0x001D26BE File Offset: 0x001D08BE
	public void SetCraftStatus(Clustercraft.CraftStatus craft_status)
	{
		this.status = craft_status;
		this.UpdateGroundTags();
		this.m_moduleInterface.TriggerEventOnCraftAndRocket(GameHashes.ClustercraftStateChanged, craft_status);
	}

	// Token: 0x06005145 RID: 20805 RVA: 0x001D26E3 File Offset: 0x001D08E3
	public void SetExploding()
	{
		this.Exploding = true;
	}

	// Token: 0x06005146 RID: 20806 RVA: 0x001D26EC File Offset: 0x001D08EC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Clustercrafts.Add(this);
	}

	// Token: 0x06005147 RID: 20807 RVA: 0x001D2700 File Offset: 0x001D0900
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.m_clusterTraveler.getSpeedCB = new Func<float>(this.GetSpeed);
		this.m_clusterTraveler.getCanTravelCB = new Func<bool, bool>(this.CanTravel);
		this.m_clusterTraveler.onTravelCB = new System.Action(this.BurnFuelForTravel);
		this.m_clusterTraveler.validateTravelCB = new Func<AxialI, bool>(this.CanTravelToCell);
		this.UpdateGroundTags();
		base.Subscribe<Clustercraft>(1512695988, Clustercraft.RocketModuleChangedHandler);
		base.Subscribe<Clustercraft>(543433792, Clustercraft.ClusterDestinationChangedHandler);
		base.Subscribe<Clustercraft>(1796608350, Clustercraft.ClusterDestinationReachedHandler);
		base.Subscribe(-688990705, delegate(object o)
		{
			this.UpdateStatusItem();
		});
		base.Subscribe<Clustercraft>(1102426921, Clustercraft.NameChangedHandler);
		this.SetRocketName(this.m_name);
		this.UpdateStatusItem();
	}

	// Token: 0x06005148 RID: 20808 RVA: 0x001D27E4 File Offset: 0x001D09E4
	public void Sim1000ms(float dt)
	{
		this.controlStationBuffTimeRemaining = Mathf.Max(this.controlStationBuffTimeRemaining - dt, 0f);
		if (this.controlStationBuffTimeRemaining > 0f)
		{
			this.missionControlStatusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.MissionControlBoosted, this);
			return;
		}
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MissionControlBoosted, false);
		this.missionControlStatusHandle = Guid.Empty;
	}

	// Token: 0x06005149 RID: 20809 RVA: 0x001D2860 File Offset: 0x001D0A60
	public void Sim4000ms(float dt)
	{
		RocketClusterDestinationSelector clusterDestinationSelector = this.m_moduleInterface.GetClusterDestinationSelector();
		if (this.Status == Clustercraft.CraftStatus.InFlight && this.m_location == clusterDestinationSelector.GetDestination())
		{
			this.OnClusterDestinationReached(null);
		}
	}

	// Token: 0x0600514A RID: 20810 RVA: 0x001D289C File Offset: 0x001D0A9C
	public void Init(AxialI location, LaunchPad pad)
	{
		this.m_location = location;
		base.GetComponent<RocketClusterDestinationSelector>().SetDestination(this.m_location);
		this.SetRocketName(GameUtil.GenerateRandomRocketName());
		if (pad != null)
		{
			this.Land(pad, true);
		}
		this.UpdateStatusItem();
	}

	// Token: 0x0600514B RID: 20811 RVA: 0x001D28D8 File Offset: 0x001D0AD8
	protected override void OnCleanUp()
	{
		Components.Clustercrafts.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600514C RID: 20812 RVA: 0x001D28EB File Offset: 0x001D0AEB
	private bool CanTravel(bool tryingToLand)
	{
		return this.HasTag(GameTags.RocketInSpace) && (tryingToLand || this.HasResourcesToMove(1, Clustercraft.CombustionResource.All));
	}

	// Token: 0x0600514D RID: 20813 RVA: 0x001D2909 File Offset: 0x001D0B09
	private bool CanTravelToCell(AxialI location)
	{
		return !(ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.Asteroid) != null) || this.CanLandAtAsteroid(location, true);
	}

	// Token: 0x0600514E RID: 20814 RVA: 0x001D2929 File Offset: 0x001D0B29
	private float GetSpeed()
	{
		return this.Speed;
	}

	// Token: 0x0600514F RID: 20815 RVA: 0x001D2934 File Offset: 0x001D0B34
	private void RocketModuleChanged(object data)
	{
		RocketModuleCluster rocketModuleCluster = (RocketModuleCluster)data;
		if (rocketModuleCluster != null)
		{
			this.UpdateGroundTags(rocketModuleCluster.gameObject);
		}
	}

	// Token: 0x06005150 RID: 20816 RVA: 0x001D295D File Offset: 0x001D0B5D
	private void OnClusterDestinationChanged(object data)
	{
		this.UpdateStatusItem();
	}

	// Token: 0x06005151 RID: 20817 RVA: 0x001D2968 File Offset: 0x001D0B68
	private void OnClusterDestinationReached(object data)
	{
		RocketClusterDestinationSelector clusterDestinationSelector = this.m_moduleInterface.GetClusterDestinationSelector();
		global::Debug.Assert(base.Location == clusterDestinationSelector.GetDestination());
		if (clusterDestinationSelector.HasAsteroidDestination())
		{
			LaunchPad destinationPad = clusterDestinationSelector.GetDestinationPad();
			this.Land(base.Location, destinationPad);
		}
		this.UpdateStatusItem();
	}

	// Token: 0x06005152 RID: 20818 RVA: 0x001D29B9 File Offset: 0x001D0BB9
	public void SetRocketName(object newName)
	{
		this.SetRocketName((string)newName);
	}

	// Token: 0x06005153 RID: 20819 RVA: 0x001D29C8 File Offset: 0x001D0BC8
	public void SetRocketName(string newName)
	{
		this.m_name = newName;
		base.name = "Clustercraft: " + newName;
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			CharacterOverlay component = @ref.Get().GetComponent<CharacterOverlay>();
			if (component != null)
			{
				NameDisplayScreen.Instance.UpdateName(component.gameObject);
				break;
			}
		}
		ClusterManager.Instance.Trigger(1943181844, newName);
	}

	// Token: 0x06005154 RID: 20820 RVA: 0x001D2A60 File Offset: 0x001D0C60
	public bool CheckPreppedForLaunch()
	{
		return this.m_moduleInterface.CheckPreppedForLaunch();
	}

	// Token: 0x06005155 RID: 20821 RVA: 0x001D2A6D File Offset: 0x001D0C6D
	public bool CheckReadyToLaunch()
	{
		return this.m_moduleInterface.CheckReadyToLaunch();
	}

	// Token: 0x06005156 RID: 20822 RVA: 0x001D2A7A File Offset: 0x001D0C7A
	public bool IsFlightInProgress()
	{
		return this.Status == Clustercraft.CraftStatus.InFlight && this.m_clusterTraveler.IsTraveling();
	}

	// Token: 0x06005157 RID: 20823 RVA: 0x001D2A92 File Offset: 0x001D0C92
	public ClusterGridEntity GetPOIAtCurrentLocation()
	{
		if (this.status != Clustercraft.CraftStatus.InFlight || this.IsFlightInProgress())
		{
			return null;
		}
		return ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_location, EntityLayer.POI);
	}

	// Token: 0x06005158 RID: 20824 RVA: 0x001D2AB8 File Offset: 0x001D0CB8
	public ClusterGridEntity GetStableOrbitAsteroid()
	{
		if (this.status != Clustercraft.CraftStatus.InFlight || this.IsFlightInProgress())
		{
			return null;
		}
		return ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(this.m_location, EntityLayer.Asteroid);
	}

	// Token: 0x06005159 RID: 20825 RVA: 0x001D2ADE File Offset: 0x001D0CDE
	public ClusterGridEntity GetOrbitAsteroid()
	{
		if (this.status != Clustercraft.CraftStatus.InFlight)
		{
			return null;
		}
		return ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(this.m_location, EntityLayer.Asteroid);
	}

	// Token: 0x0600515A RID: 20826 RVA: 0x001D2AFC File Offset: 0x001D0CFC
	public ClusterGridEntity GetAdjacentAsteroid()
	{
		return ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(this.m_location, EntityLayer.Asteroid);
	}

	// Token: 0x0600515B RID: 20827 RVA: 0x001D2B0F File Offset: 0x001D0D0F
	private bool CheckDesinationInRange()
	{
		return this.m_clusterTraveler.CurrentPath != null && this.Speed * this.m_clusterTraveler.TravelETA() <= this.ModuleInterface.Range;
	}

	// Token: 0x0600515C RID: 20828 RVA: 0x001D2B44 File Offset: 0x001D0D44
	public bool HasResourcesToMove(int hexes = 1, Clustercraft.CombustionResource combustionResource = Clustercraft.CombustionResource.All)
	{
		switch (combustionResource)
		{
		case Clustercraft.CombustionResource.Fuel:
			return this.m_moduleInterface.FuelRemaining / this.FuelPerDistance >= 600f * (float)hexes - 0.001f;
		case Clustercraft.CombustionResource.Oxidizer:
			return this.m_moduleInterface.OxidizerPowerRemaining / this.FuelPerDistance >= 600f * (float)hexes - 0.001f;
		case Clustercraft.CombustionResource.All:
			return this.m_moduleInterface.BurnableMassRemaining / this.FuelPerDistance >= 600f * (float)hexes - 0.001f;
		default:
			return false;
		}
	}

	// Token: 0x0600515D RID: 20829 RVA: 0x001D2BD8 File Offset: 0x001D0DD8
	private void BurnFuelForTravel()
	{
		float num = 600f;
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			RocketEngineCluster component = rocketModuleCluster.GetComponent<RocketEngineCluster>();
			if (component != null)
			{
				Tag fuelTag = component.fuelTag;
				float num2 = 0f;
				if (component.requireOxidizer)
				{
					num2 = this.ModuleInterface.OxidizerPowerRemaining;
				}
				if (num > 0f)
				{
					foreach (Ref<RocketModuleCluster> ref2 in this.m_moduleInterface.ClusterModules)
					{
						IFuelTank component2 = ref2.Get().GetComponent<IFuelTank>();
						if (!component2.IsNullOrDestroyed())
						{
							num -= this.BurnFromTank(num, component, fuelTag, component2.Storage, ref num2);
						}
						if (num <= 0f)
						{
							break;
						}
					}
				}
			}
			RoboPilotModule component3 = rocketModuleCluster.GetComponent<RoboPilotModule>();
			if (component3 != null)
			{
				component3.ConsumeDataBanksInFlight();
			}
		}
		this.UpdateStatusItem();
	}

	// Token: 0x0600515E RID: 20830 RVA: 0x001D2D08 File Offset: 0x001D0F08
	private float BurnFromTank(float attemptTravelAmount, RocketEngineCluster engine, Tag fuelTag, IStorage storage, ref float totalOxidizerRemaining)
	{
		float num = attemptTravelAmount * engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
		num = Mathf.Min(storage.GetAmountAvailable(fuelTag), num);
		if (engine.requireOxidizer)
		{
			num = Mathf.Min(num, totalOxidizerRemaining);
		}
		storage.ConsumeIgnoringDisease(fuelTag, num);
		if (engine.requireOxidizer)
		{
			this.BurnOxidizer(num);
			totalOxidizerRemaining -= num;
		}
		return num / engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
	}

	// Token: 0x0600515F RID: 20831 RVA: 0x001D2D7C File Offset: 0x001D0F7C
	private void BurnOxidizer(float fuelEquivalentKGs)
	{
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			OxidizerTank component = @ref.Get().GetComponent<OxidizerTank>();
			if (component != null)
			{
				foreach (KeyValuePair<Tag, float> keyValuePair in component.GetOxidizersAvailable())
				{
					float num = Clustercraft.dlc1OxidizerEfficiencies[keyValuePair.Key];
					float num2 = Mathf.Min(fuelEquivalentKGs / num, keyValuePair.Value);
					if (num2 > 0f)
					{
						component.storage.ConsumeIgnoringDisease(keyValuePair.Key, num2);
						fuelEquivalentKGs -= num2 * num;
					}
				}
			}
			if (fuelEquivalentKGs <= 0f)
			{
				break;
			}
		}
	}

	// Token: 0x06005160 RID: 20832 RVA: 0x001D2E70 File Offset: 0x001D1070
	public List<ResourceHarvestModule.StatesInstance> GetAllResourceHarvestModules()
	{
		List<ResourceHarvestModule.StatesInstance> list = new List<ResourceHarvestModule.StatesInstance>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			ResourceHarvestModule.StatesInstance smi = @ref.Get().GetSMI<ResourceHarvestModule.StatesInstance>();
			if (smi != null)
			{
				list.Add(smi);
			}
		}
		return list;
	}

	// Token: 0x06005161 RID: 20833 RVA: 0x001D2ED8 File Offset: 0x001D10D8
	public List<ArtifactHarvestModule.StatesInstance> GetAllArtifactHarvestModules()
	{
		List<ArtifactHarvestModule.StatesInstance> list = new List<ArtifactHarvestModule.StatesInstance>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			ArtifactHarvestModule.StatesInstance smi = @ref.Get().GetSMI<ArtifactHarvestModule.StatesInstance>();
			if (smi != null)
			{
				list.Add(smi);
			}
		}
		return list;
	}

	// Token: 0x06005162 RID: 20834 RVA: 0x001D2F40 File Offset: 0x001D1140
	public List<CargoBayCluster> GetAllCargoBays()
	{
		List<CargoBayCluster> list = new List<CargoBayCluster>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			CargoBayCluster component = @ref.Get().GetComponent<CargoBayCluster>();
			if (component != null)
			{
				list.Add(component);
			}
		}
		return list;
	}

	// Token: 0x06005163 RID: 20835 RVA: 0x001D2FAC File Offset: 0x001D11AC
	public List<CargoBayCluster> GetCargoBaysOfType(CargoBay.CargoType cargoType)
	{
		List<CargoBayCluster> list = new List<CargoBayCluster>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			CargoBayCluster component = @ref.Get().GetComponent<CargoBayCluster>();
			if (component != null && component.storageType == cargoType)
			{
				list.Add(component);
			}
		}
		return list;
	}

	// Token: 0x06005164 RID: 20836 RVA: 0x001D3024 File Offset: 0x001D1224
	public void DestroyCraftAndModules()
	{
		WorldContainer interiorWorld = this.m_moduleInterface.GetInteriorWorld();
		if (interiorWorld != null)
		{
			NameDisplayScreen.Instance.RemoveWorldEntries(interiorWorld.id);
		}
		List<RocketModuleCluster> list = (from x in this.m_moduleInterface.ClusterModules
		select x.Get()).ToList<RocketModuleCluster>();
		for (int i = list.Count - 1; i >= 0; i--)
		{
			RocketModuleCluster rocketModuleCluster = list[i];
			Storage component = rocketModuleCluster.GetComponent<Storage>();
			if (component != null)
			{
				component.ConsumeAllIgnoringDisease();
			}
			MinionStorage component2 = rocketModuleCluster.GetComponent<MinionStorage>();
			if (component2 != null)
			{
				List<MinionStorage.Info> storedMinionInfo = component2.GetStoredMinionInfo();
				for (int j = storedMinionInfo.Count - 1; j >= 0; j--)
				{
					component2.DeleteStoredMinion(storedMinionInfo[j].id);
				}
			}
			Util.KDestroyGameObject(rocketModuleCluster.gameObject);
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06005165 RID: 20837 RVA: 0x001D311D File Offset: 0x001D131D
	public void CancelLaunch()
	{
		if (this.LaunchRequested)
		{
			global::Debug.Log("Cancelling launch!");
			this.LaunchRequested = false;
		}
	}

	// Token: 0x06005166 RID: 20838 RVA: 0x001D3138 File Offset: 0x001D1338
	public void RequestLaunch(bool automated = false)
	{
		if (this.HasTag(GameTags.RocketNotOnGround) || this.m_moduleInterface.GetClusterDestinationSelector().IsAtDestination())
		{
			return;
		}
		if (DebugHandler.InstantBuildMode && !automated)
		{
			this.Launch(false);
		}
		if (this.LaunchRequested)
		{
			return;
		}
		if (!this.CheckPreppedForLaunch())
		{
			return;
		}
		global::Debug.Log("Triggering launch!");
		if (this.m_moduleInterface.GetRobotPilotModule() != null)
		{
			this.Launch(automated);
		}
		this.LaunchRequested = true;
	}

	// Token: 0x06005167 RID: 20839 RVA: 0x001D31B4 File Offset: 0x001D13B4
	public void Launch(bool automated = false)
	{
		if (this.HasTag(GameTags.RocketNotOnGround) || this.m_moduleInterface.GetClusterDestinationSelector().IsAtDestination())
		{
			this.LaunchRequested = false;
			return;
		}
		if ((!DebugHandler.InstantBuildMode || automated) && !this.CheckReadyToLaunch())
		{
			return;
		}
		if (automated && !this.m_moduleInterface.CheckReadyForAutomatedLaunchCommand())
		{
			this.LaunchRequested = false;
			return;
		}
		this.LaunchRequested = false;
		this.SetCraftStatus(Clustercraft.CraftStatus.Launching);
		this.m_moduleInterface.DoLaunch();
		this.BurnFuelForTravel();
		this.m_clusterTraveler.AdvancePathOneStep();
		this.UpdateStatusItem();
	}

	// Token: 0x06005168 RID: 20840 RVA: 0x001D3244 File Offset: 0x001D1444
	public void LandAtPad(LaunchPad pad)
	{
		this.m_moduleInterface.GetClusterDestinationSelector().SetDestinationPad(pad);
	}

	// Token: 0x06005169 RID: 20841 RVA: 0x001D3258 File Offset: 0x001D1458
	public Clustercraft.PadLandingStatus CanLandAtPad(LaunchPad pad, out string failReason)
	{
		if (pad == null)
		{
			failReason = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.NONEAVAILABLE;
			return Clustercraft.PadLandingStatus.CanNeverLand;
		}
		if (pad.HasRocket() && pad.LandedRocket.CraftInterface != this.m_moduleInterface)
		{
			failReason = "<TEMP>The pad already has a rocket on it!<TEMP>";
			return Clustercraft.PadLandingStatus.CanLandEventually;
		}
		if (ConditionFlightPathIsClear.PadTopEdgeDistanceToCeilingEdge(pad.gameObject) < this.ModuleInterface.RocketHeight)
		{
			failReason = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_TOO_SHORT;
			return Clustercraft.PadLandingStatus.CanNeverLand;
		}
		int num = -1;
		if (!ConditionFlightPathIsClear.CheckFlightPathClear(this.ModuleInterface, pad.gameObject, out num))
		{
			failReason = string.Format(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PATH_OBSTRUCTED, pad.GetProperName());
			return Clustercraft.PadLandingStatus.CanNeverLand;
		}
		if (!pad.GetComponent<Operational>().IsOperational)
		{
			failReason = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PAD_DISABLED;
			return Clustercraft.PadLandingStatus.CanNeverLand;
		}
		int rocketBottomPosition = pad.RocketBottomPosition;
		foreach (Ref<RocketModuleCluster> @ref in this.ModuleInterface.ClusterModules)
		{
			GameObject gameObject = @ref.Get().gameObject;
			int moduleRelativeVerticalPosition = this.ModuleInterface.GetModuleRelativeVerticalPosition(gameObject);
			Building component = gameObject.GetComponent<Building>();
			BuildingUnderConstruction component2 = gameObject.GetComponent<BuildingUnderConstruction>();
			BuildingDef buildingDef = (component != null) ? component.Def : component2.Def;
			for (int i = 0; i < buildingDef.WidthInCells; i++)
			{
				for (int j = 0; j < buildingDef.HeightInCells; j++)
				{
					int num2 = Grid.OffsetCell(rocketBottomPosition, 0, moduleRelativeVerticalPosition);
					num2 = Grid.OffsetCell(num2, -(buildingDef.WidthInCells / 2) + i, j);
					if (Grid.Solid[num2])
					{
						num = num2;
						failReason = string.Format(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_SITE_OBSTRUCTED, pad.GetProperName());
						return Clustercraft.PadLandingStatus.CanNeverLand;
					}
				}
			}
		}
		failReason = null;
		return Clustercraft.PadLandingStatus.CanLandImmediately;
	}

	// Token: 0x0600516A RID: 20842 RVA: 0x001D3428 File Offset: 0x001D1628
	private LaunchPad FindValidLandingPad(AxialI location, bool mustLandImmediately)
	{
		LaunchPad result = null;
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(location);
		LaunchPad preferredLaunchPadForWorld = this.m_moduleInterface.GetPreferredLaunchPadForWorld(asteroidWorldIdAtLocation);
		string text;
		if (preferredLaunchPadForWorld != null && this.CanLandAtPad(preferredLaunchPadForWorld, out text) == Clustercraft.PadLandingStatus.CanLandImmediately)
		{
			return preferredLaunchPadForWorld;
		}
		foreach (object obj in Components.LaunchPads)
		{
			LaunchPad launchPad = (LaunchPad)obj;
			if (launchPad.GetMyWorldLocation() == location)
			{
				string text2;
				Clustercraft.PadLandingStatus padLandingStatus = this.CanLandAtPad(launchPad, out text2);
				if (padLandingStatus == Clustercraft.PadLandingStatus.CanLandImmediately)
				{
					return launchPad;
				}
				if (!mustLandImmediately && padLandingStatus == Clustercraft.PadLandingStatus.CanLandEventually)
				{
					result = launchPad;
				}
			}
		}
		return result;
	}

	// Token: 0x0600516B RID: 20843 RVA: 0x001D34E4 File Offset: 0x001D16E4
	public bool CanLandAtAsteroid(AxialI location, bool mustLandImmediately)
	{
		LaunchPad destinationPad = this.m_moduleInterface.GetClusterDestinationSelector().GetDestinationPad();
		global::Debug.Assert(destinationPad == null || destinationPad.GetMyWorldLocation() == location, "A rocket is trying to travel to an asteroid but has selected a landing pad at a different asteroid!");
		if (destinationPad != null)
		{
			string text;
			Clustercraft.PadLandingStatus padLandingStatus = this.CanLandAtPad(destinationPad, out text);
			return padLandingStatus == Clustercraft.PadLandingStatus.CanLandImmediately || (!mustLandImmediately && padLandingStatus == Clustercraft.PadLandingStatus.CanLandEventually);
		}
		return this.FindValidLandingPad(location, mustLandImmediately) != null;
	}

	// Token: 0x0600516C RID: 20844 RVA: 0x001D3554 File Offset: 0x001D1754
	private void Land(LaunchPad pad, bool forceGrounded)
	{
		string text;
		if (this.CanLandAtPad(pad, out text) != Clustercraft.PadLandingStatus.CanLandImmediately)
		{
			return;
		}
		this.BurnFuelForTravel();
		this.m_location = pad.GetMyWorldLocation();
		this.SetCraftStatus(forceGrounded ? Clustercraft.CraftStatus.Grounded : Clustercraft.CraftStatus.Landing);
		this.m_moduleInterface.DoLand(pad);
		this.UpdateStatusItem();
	}

	// Token: 0x0600516D RID: 20845 RVA: 0x001D35A0 File Offset: 0x001D17A0
	private void Land(AxialI destination, LaunchPad chosenPad)
	{
		if (chosenPad == null)
		{
			chosenPad = this.FindValidLandingPad(destination, true);
		}
		global::Debug.Assert(chosenPad == null || chosenPad.GetMyWorldLocation() == this.m_location, "Attempting to land on a pad that isn't at our current position");
		this.Land(chosenPad, false);
	}

	// Token: 0x0600516E RID: 20846 RVA: 0x001D35F0 File Offset: 0x001D17F0
	public void UpdateStatusItem()
	{
		if (ClusterGrid.Instance == null)
		{
			return;
		}
		if (this.mainStatusHandle != Guid.Empty)
		{
			this.selectable.RemoveStatusItem(this.mainStatusHandle, false);
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_location, EntityLayer.Asteroid);
		ClusterGridEntity orbitAsteroid = this.GetOrbitAsteroid();
		bool flag = false;
		if (orbitAsteroid != null)
		{
			using (IEnumerator enumerator = Components.LaunchPads.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((LaunchPad)enumerator.Current).GetMyWorldLocation() == orbitAsteroid.Location)
					{
						flag = true;
						break;
					}
				}
			}
		}
		bool set = false;
		if (visibleEntityOfLayerAtCell != null)
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.InFlight, this.m_clusterTraveler);
		}
		else if (!this.HasResourcesToMove(1, Clustercraft.CombustionResource.All) && !flag)
		{
			set = true;
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.RocketStranded, orbitAsteroid);
		}
		else if (!this.m_moduleInterface.GetClusterDestinationSelector().IsAtDestination() && !this.CheckDesinationInRange())
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.DestinationOutOfRange, this.m_clusterTraveler);
		}
		else if (orbitAsteroid != null && this.Destination == orbitAsteroid.Location)
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.WaitingToLand, orbitAsteroid);
		}
		else if (this.IsFlightInProgress() || this.Status == Clustercraft.CraftStatus.Launching)
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.InFlight, this.m_clusterTraveler);
		}
		else if (orbitAsteroid != null)
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.InOrbit, orbitAsteroid);
		}
		else
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		}
		base.GetComponent<KPrefabID>().SetTag(GameTags.RocketStranded, set);
		float num = 0f;
		float num2 = 0f;
		foreach (CargoBayCluster cargoBayCluster in this.GetAllCargoBays())
		{
			num += cargoBayCluster.MaxCapacity;
			num2 += cargoBayCluster.RemainingCapacity;
		}
		if (this.Status == Clustercraft.CraftStatus.Grounded || num <= 0f)
		{
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightAllCargoFull, false);
			return;
		}
		if (num2 == 0f)
		{
			this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.FlightAllCargoFull, null);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, false);
			return;
		}
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightAllCargoFull, false);
		if (this.cargoStatusHandle == Guid.Empty)
		{
			this.cargoStatusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, num2);
			return;
		}
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, true);
		this.cargoStatusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, num2);
	}

	// Token: 0x0600516F RID: 20847 RVA: 0x001D3A30 File Offset: 0x001D1C30
	private void UpdateGroundTags()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.ModuleInterface.ClusterModules)
		{
			if (@ref != null && !(@ref.Get() == null))
			{
				this.UpdateGroundTags(@ref.Get().gameObject);
			}
		}
		this.UpdateGroundTags(base.gameObject);
	}

	// Token: 0x06005170 RID: 20848 RVA: 0x001D3AAC File Offset: 0x001D1CAC
	private void UpdateGroundTags(GameObject go)
	{
		this.SetTagOnGameObject(go, GameTags.RocketOnGround, this.status == Clustercraft.CraftStatus.Grounded);
		this.SetTagOnGameObject(go, GameTags.RocketNotOnGround, this.status > Clustercraft.CraftStatus.Grounded);
		this.SetTagOnGameObject(go, GameTags.RocketInSpace, this.status == Clustercraft.CraftStatus.InFlight);
		this.SetTagOnGameObject(go, GameTags.EntityInSpace, this.status == Clustercraft.CraftStatus.InFlight);
	}

	// Token: 0x06005171 RID: 20849 RVA: 0x001D3B0D File Offset: 0x001D1D0D
	private void SetTagOnGameObject(GameObject go, Tag tag, bool set)
	{
		if (set)
		{
			go.AddTag(tag);
			return;
		}
		go.RemoveTag(tag);
	}

	// Token: 0x06005172 RID: 20850 RVA: 0x001D3B21 File Offset: 0x001D1D21
	public override bool ShowName()
	{
		return this.status > Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x06005173 RID: 20851 RVA: 0x001D3B2C File Offset: 0x001D1D2C
	public override bool ShowPath()
	{
		return this.status > Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x06005174 RID: 20852 RVA: 0x001D3B37 File Offset: 0x001D1D37
	public bool IsTravellingAndFueled()
	{
		return this.HasResourcesToMove(1, Clustercraft.CombustionResource.All) && this.m_clusterTraveler.IsTraveling();
	}

	// Token: 0x06005175 RID: 20853 RVA: 0x001D3B50 File Offset: 0x001D1D50
	public override bool ShowProgressBar()
	{
		return this.IsTravellingAndFueled();
	}

	// Token: 0x06005176 RID: 20854 RVA: 0x001D3B58 File Offset: 0x001D1D58
	public override float GetProgress()
	{
		return this.m_clusterTraveler.GetMoveProgress();
	}

	// Token: 0x06005177 RID: 20855 RVA: 0x001D3B68 File Offset: 0x001D1D68
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.Status != Clustercraft.CraftStatus.Grounded && SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 27))
		{
			UIScheduler.Instance.ScheduleNextFrame("Check Fuel Costs", delegate(object o)
			{
				foreach (Ref<RocketModuleCluster> @ref in this.ModuleInterface.ClusterModules)
				{
					RocketModuleCluster rocketModuleCluster = @ref.Get();
					IFuelTank component = rocketModuleCluster.GetComponent<IFuelTank>();
					if (component != null && !component.Storage.IsEmpty())
					{
						component.DEBUG_FillTank();
					}
					OxidizerTank component2 = rocketModuleCluster.GetComponent<OxidizerTank>();
					if (component2 != null)
					{
						Dictionary<Tag, float> oxidizersAvailable = component2.GetOxidizersAvailable();
						if (oxidizersAvailable.Count > 0)
						{
							foreach (KeyValuePair<Tag, float> keyValuePair in oxidizersAvailable)
							{
								if (keyValuePair.Value > 0f)
								{
									component2.DEBUG_FillTank(ElementLoader.GetElementID(keyValuePair.Key));
									break;
								}
							}
						}
					}
				}
			}, null, null);
		}
	}

	// Token: 0x06005178 RID: 20856 RVA: 0x001D3BB2 File Offset: 0x001D1DB2
	public float GetRange()
	{
		return this.ModuleInterface.Range;
	}

	// Token: 0x06005179 RID: 20857 RVA: 0x001D3BBF File Offset: 0x001D1DBF
	public int GetRangeInTiles()
	{
		return this.ModuleInterface.RangeInTiles;
	}

	// Token: 0x040035EB RID: 13803
	[Serialize]
	private string m_name;

	// Token: 0x040035ED RID: 13805
	[MyCmpReq]
	private ClusterTraveler m_clusterTraveler;

	// Token: 0x040035EE RID: 13806
	[MyCmpReq]
	private CraftModuleInterface m_moduleInterface;

	// Token: 0x040035EF RID: 13807
	private Guid mainStatusHandle;

	// Token: 0x040035F0 RID: 13808
	private Guid cargoStatusHandle;

	// Token: 0x040035F1 RID: 13809
	private Guid missionControlStatusHandle = Guid.Empty;

	// Token: 0x040035F2 RID: 13810
	public static Dictionary<Tag, float> dlc1OxidizerEfficiencies = new Dictionary<Tag, float>
	{
		{
			SimHashes.OxyRock.CreateTag(),
			ROCKETRY.DLC1_OXIDIZER_EFFICIENCY.LOW
		},
		{
			SimHashes.LiquidOxygen.CreateTag(),
			ROCKETRY.DLC1_OXIDIZER_EFFICIENCY.HIGH
		},
		{
			SimHashes.Fertilizer.CreateTag(),
			ROCKETRY.DLC1_OXIDIZER_EFFICIENCY.VERY_LOW
		}
	};

	// Token: 0x040035F3 RID: 13811
	[Serialize]
	[Range(0f, 1f)]
	public float AutoPilotMultiplier = 1f;

	// Token: 0x040035F4 RID: 13812
	[Serialize]
	[Range(0f, 2f)]
	public float PilotSkillMultiplier = 1f;

	// Token: 0x040035F5 RID: 13813
	[Serialize]
	public float controlStationBuffTimeRemaining;

	// Token: 0x040035F6 RID: 13814
	[Serialize]
	private bool m_launchRequested;

	// Token: 0x040035F7 RID: 13815
	[Serialize]
	private Clustercraft.CraftStatus status;

	// Token: 0x040035F8 RID: 13816
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x040035F9 RID: 13817
	private static EventSystem.IntraObjectHandler<Clustercraft> RocketModuleChangedHandler = new EventSystem.IntraObjectHandler<Clustercraft>(delegate(Clustercraft cmp, object data)
	{
		cmp.RocketModuleChanged(data);
	});

	// Token: 0x040035FA RID: 13818
	private static EventSystem.IntraObjectHandler<Clustercraft> ClusterDestinationChangedHandler = new EventSystem.IntraObjectHandler<Clustercraft>(delegate(Clustercraft cmp, object data)
	{
		cmp.OnClusterDestinationChanged(data);
	});

	// Token: 0x040035FB RID: 13819
	private static EventSystem.IntraObjectHandler<Clustercraft> ClusterDestinationReachedHandler = new EventSystem.IntraObjectHandler<Clustercraft>(delegate(Clustercraft cmp, object data)
	{
		cmp.OnClusterDestinationReached(data);
	});

	// Token: 0x040035FC RID: 13820
	private static EventSystem.IntraObjectHandler<Clustercraft> NameChangedHandler = new EventSystem.IntraObjectHandler<Clustercraft>(delegate(Clustercraft cmp, object data)
	{
		cmp.SetRocketName(data);
	});

	// Token: 0x02001B02 RID: 6914
	public enum CraftStatus
	{
		// Token: 0x04007E78 RID: 32376
		Grounded,
		// Token: 0x04007E79 RID: 32377
		Launching,
		// Token: 0x04007E7A RID: 32378
		InFlight,
		// Token: 0x04007E7B RID: 32379
		Landing
	}

	// Token: 0x02001B03 RID: 6915
	public enum CombustionResource
	{
		// Token: 0x04007E7D RID: 32381
		Fuel,
		// Token: 0x04007E7E RID: 32382
		Oxidizer,
		// Token: 0x04007E7F RID: 32383
		All
	}

	// Token: 0x02001B04 RID: 6916
	public enum PadLandingStatus
	{
		// Token: 0x04007E81 RID: 32385
		CanLandImmediately,
		// Token: 0x04007E82 RID: 32386
		CanLandEventually,
		// Token: 0x04007E83 RID: 32387
		CanNeverLand
	}
}
