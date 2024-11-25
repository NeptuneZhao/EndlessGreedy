using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007D4 RID: 2004
public class Components
{
	// Token: 0x040020DB RID: 8411
	public static Components.Cmps<RobotAi.Instance> LiveRobotsIdentities = new Components.Cmps<RobotAi.Instance>();

	// Token: 0x040020DC RID: 8412
	public static Components.Cmps<MinionIdentity> LiveMinionIdentities = new Components.Cmps<MinionIdentity>();

	// Token: 0x040020DD RID: 8413
	public static Components.Cmps<MinionIdentity> MinionIdentities = new Components.Cmps<MinionIdentity>();

	// Token: 0x040020DE RID: 8414
	public static Components.Cmps<StoredMinionIdentity> StoredMinionIdentities = new Components.Cmps<StoredMinionIdentity>();

	// Token: 0x040020DF RID: 8415
	public static Components.Cmps<MinionStorage> MinionStorages = new Components.Cmps<MinionStorage>();

	// Token: 0x040020E0 RID: 8416
	public static Components.Cmps<MinionResume> MinionResumes = new Components.Cmps<MinionResume>();

	// Token: 0x040020E1 RID: 8417
	public static Dictionary<Tag, Components.Cmps<MinionIdentity>> MinionIdentitiesByModel = new Dictionary<Tag, Components.Cmps<MinionIdentity>>();

	// Token: 0x040020E2 RID: 8418
	public static Dictionary<Tag, Components.Cmps<MinionIdentity>> LiveMinionIdentitiesByModel = new Dictionary<Tag, Components.Cmps<MinionIdentity>>();

	// Token: 0x040020E3 RID: 8419
	public static Components.CmpsByWorld<Sleepable> NormalBeds = new Components.CmpsByWorld<Sleepable>();

	// Token: 0x040020E4 RID: 8420
	public static Components.Cmps<IUsable> Toilets = new Components.Cmps<IUsable>();

	// Token: 0x040020E5 RID: 8421
	public static Components.Cmps<Pickupable> Pickupables = new Components.Cmps<Pickupable>();

	// Token: 0x040020E6 RID: 8422
	public static Components.Cmps<Brain> Brains = new Components.Cmps<Brain>();

	// Token: 0x040020E7 RID: 8423
	public static Components.Cmps<BuildingComplete> BuildingCompletes = new Components.Cmps<BuildingComplete>();

	// Token: 0x040020E8 RID: 8424
	public static Components.Cmps<Notifier> Notifiers = new Components.Cmps<Notifier>();

	// Token: 0x040020E9 RID: 8425
	public static Components.Cmps<Fabricator> Fabricators = new Components.Cmps<Fabricator>();

	// Token: 0x040020EA RID: 8426
	public static Components.Cmps<Refinery> Refineries = new Components.Cmps<Refinery>();

	// Token: 0x040020EB RID: 8427
	public static Components.CmpsByWorld<PlantablePlot> PlantablePlots = new Components.CmpsByWorld<PlantablePlot>();

	// Token: 0x040020EC RID: 8428
	public static Components.Cmps<Ladder> Ladders = new Components.Cmps<Ladder>();

	// Token: 0x040020ED RID: 8429
	public static Components.Cmps<NavTeleporter> NavTeleporters = new Components.Cmps<NavTeleporter>();

	// Token: 0x040020EE RID: 8430
	public static Components.Cmps<ITravelTubePiece> ITravelTubePieces = new Components.Cmps<ITravelTubePiece>();

	// Token: 0x040020EF RID: 8431
	public static Components.CmpsByWorld<CreatureFeeder> CreatureFeeders = new Components.CmpsByWorld<CreatureFeeder>();

	// Token: 0x040020F0 RID: 8432
	public static Components.CmpsByWorld<MilkFeeder.Instance> MilkFeeders = new Components.CmpsByWorld<MilkFeeder.Instance>();

	// Token: 0x040020F1 RID: 8433
	public static Components.Cmps<Light2D> Light2Ds = new Components.Cmps<Light2D>();

	// Token: 0x040020F2 RID: 8434
	public static Components.Cmps<Radiator> Radiators = new Components.Cmps<Radiator>();

	// Token: 0x040020F3 RID: 8435
	public static Components.Cmps<Edible> Edibles = new Components.Cmps<Edible>();

	// Token: 0x040020F4 RID: 8436
	public static Components.Cmps<Diggable> Diggables = new Components.Cmps<Diggable>();

	// Token: 0x040020F5 RID: 8437
	public static Components.Cmps<IResearchCenter> ResearchCenters = new Components.Cmps<IResearchCenter>();

	// Token: 0x040020F6 RID: 8438
	public static Components.Cmps<Harvestable> Harvestables = new Components.Cmps<Harvestable>();

	// Token: 0x040020F7 RID: 8439
	public static Components.Cmps<HarvestDesignatable> HarvestDesignatables = new Components.Cmps<HarvestDesignatable>();

	// Token: 0x040020F8 RID: 8440
	public static Components.Cmps<Uprootable> Uprootables = new Components.Cmps<Uprootable>();

	// Token: 0x040020F9 RID: 8441
	public static Components.Cmps<Health> Health = new Components.Cmps<Health>();

	// Token: 0x040020FA RID: 8442
	public static Components.Cmps<Equipment> Equipment = new Components.Cmps<Equipment>();

	// Token: 0x040020FB RID: 8443
	public static Components.Cmps<FactionAlignment> FactionAlignments = new Components.Cmps<FactionAlignment>();

	// Token: 0x040020FC RID: 8444
	public static Components.Cmps<FactionAlignment> PlayerTargeted = new Components.Cmps<FactionAlignment>();

	// Token: 0x040020FD RID: 8445
	public static Components.Cmps<Telepad> Telepads = new Components.Cmps<Telepad>();

	// Token: 0x040020FE RID: 8446
	public static Components.Cmps<Generator> Generators = new Components.Cmps<Generator>();

	// Token: 0x040020FF RID: 8447
	public static Components.Cmps<EnergyConsumer> EnergyConsumers = new Components.Cmps<EnergyConsumer>();

	// Token: 0x04002100 RID: 8448
	public static Components.Cmps<Battery> Batteries = new Components.Cmps<Battery>();

	// Token: 0x04002101 RID: 8449
	public static Components.Cmps<Breakable> Breakables = new Components.Cmps<Breakable>();

	// Token: 0x04002102 RID: 8450
	public static Components.Cmps<Crop> Crops = new Components.Cmps<Crop>();

	// Token: 0x04002103 RID: 8451
	public static Components.Cmps<Prioritizable> Prioritizables = new Components.Cmps<Prioritizable>();

	// Token: 0x04002104 RID: 8452
	public static Components.Cmps<Clinic> Clinics = new Components.Cmps<Clinic>();

	// Token: 0x04002105 RID: 8453
	public static Components.Cmps<HandSanitizer> HandSanitizers = new Components.Cmps<HandSanitizer>();

	// Token: 0x04002106 RID: 8454
	public static Components.Cmps<EntityCellVisualizer> EntityCellVisualizers = new Components.Cmps<EntityCellVisualizer>();

	// Token: 0x04002107 RID: 8455
	public static Components.Cmps<RoleStation> RoleStations = new Components.Cmps<RoleStation>();

	// Token: 0x04002108 RID: 8456
	public static Components.Cmps<Telescope> Telescopes = new Components.Cmps<Telescope>();

	// Token: 0x04002109 RID: 8457
	public static Components.Cmps<Capturable> Capturables = new Components.Cmps<Capturable>();

	// Token: 0x0400210A RID: 8458
	public static Components.Cmps<NotCapturable> NotCapturables = new Components.Cmps<NotCapturable>();

	// Token: 0x0400210B RID: 8459
	public static Components.Cmps<DiseaseSourceVisualizer> DiseaseSourceVisualizers = new Components.Cmps<DiseaseSourceVisualizer>();

	// Token: 0x0400210C RID: 8460
	public static Components.Cmps<Grave> Graves = new Components.Cmps<Grave>();

	// Token: 0x0400210D RID: 8461
	public static Components.Cmps<AttachableBuilding> AttachableBuildings = new Components.Cmps<AttachableBuilding>();

	// Token: 0x0400210E RID: 8462
	public static Components.Cmps<BuildingAttachPoint> BuildingAttachPoints = new Components.Cmps<BuildingAttachPoint>();

	// Token: 0x0400210F RID: 8463
	public static Components.Cmps<MinionAssignablesProxy> MinionAssignablesProxy = new Components.Cmps<MinionAssignablesProxy>();

	// Token: 0x04002110 RID: 8464
	public static Components.Cmps<ComplexFabricator> ComplexFabricators = new Components.Cmps<ComplexFabricator>();

	// Token: 0x04002111 RID: 8465
	public static Components.Cmps<MonumentPart> MonumentParts = new Components.Cmps<MonumentPart>();

	// Token: 0x04002112 RID: 8466
	public static Components.Cmps<PlantableSeed> PlantableSeeds = new Components.Cmps<PlantableSeed>();

	// Token: 0x04002113 RID: 8467
	public static Components.Cmps<IBasicBuilding> BasicBuildings = new Components.Cmps<IBasicBuilding>();

	// Token: 0x04002114 RID: 8468
	public static Components.Cmps<Painting> Paintings = new Components.Cmps<Painting>();

	// Token: 0x04002115 RID: 8469
	public static Components.Cmps<BuildingComplete> TemplateBuildings = new Components.Cmps<BuildingComplete>();

	// Token: 0x04002116 RID: 8470
	public static Components.Cmps<Teleporter> Teleporters = new Components.Cmps<Teleporter>();

	// Token: 0x04002117 RID: 8471
	public static Components.Cmps<MutantPlant> MutantPlants = new Components.Cmps<MutantPlant>();

	// Token: 0x04002118 RID: 8472
	public static Components.Cmps<LandingBeacon.Instance> LandingBeacons = new Components.Cmps<LandingBeacon.Instance>();

	// Token: 0x04002119 RID: 8473
	public static Components.Cmps<HighEnergyParticle> HighEnergyParticles = new Components.Cmps<HighEnergyParticle>();

	// Token: 0x0400211A RID: 8474
	public static Components.Cmps<HighEnergyParticlePort> HighEnergyParticlePorts = new Components.Cmps<HighEnergyParticlePort>();

	// Token: 0x0400211B RID: 8475
	public static Components.Cmps<Clustercraft> Clustercrafts = new Components.Cmps<Clustercraft>();

	// Token: 0x0400211C RID: 8476
	public static Components.Cmps<ClustercraftInteriorDoor> ClusterCraftInteriorDoors = new Components.Cmps<ClustercraftInteriorDoor>();

	// Token: 0x0400211D RID: 8477
	public static Components.Cmps<PassengerRocketModule> PassengerRocketModules = new Components.Cmps<PassengerRocketModule>();

	// Token: 0x0400211E RID: 8478
	public static Components.Cmps<ClusterTraveler> ClusterTravelers = new Components.Cmps<ClusterTraveler>();

	// Token: 0x0400211F RID: 8479
	public static Components.Cmps<LaunchPad> LaunchPads = new Components.Cmps<LaunchPad>();

	// Token: 0x04002120 RID: 8480
	public static Components.Cmps<WarpReceiver> WarpReceivers = new Components.Cmps<WarpReceiver>();

	// Token: 0x04002121 RID: 8481
	public static Components.Cmps<RocketControlStation> RocketControlStations = new Components.Cmps<RocketControlStation>();

	// Token: 0x04002122 RID: 8482
	public static Components.Cmps<Reactor> NuclearReactors = new Components.Cmps<Reactor>();

	// Token: 0x04002123 RID: 8483
	public static Components.Cmps<BuildingComplete> EntombedBuildings = new Components.Cmps<BuildingComplete>();

	// Token: 0x04002124 RID: 8484
	public static Components.Cmps<SpaceArtifact> SpaceArtifacts = new Components.Cmps<SpaceArtifact>();

	// Token: 0x04002125 RID: 8485
	public static Components.Cmps<ArtifactAnalysisStationWorkable> ArtifactAnalysisStations = new Components.Cmps<ArtifactAnalysisStationWorkable>();

	// Token: 0x04002126 RID: 8486
	public static Components.Cmps<RocketConduitReceiver> RocketConduitReceivers = new Components.Cmps<RocketConduitReceiver>();

	// Token: 0x04002127 RID: 8487
	public static Components.Cmps<RocketConduitSender> RocketConduitSenders = new Components.Cmps<RocketConduitSender>();

	// Token: 0x04002128 RID: 8488
	public static Components.Cmps<LogicBroadcaster> LogicBroadcasters = new Components.Cmps<LogicBroadcaster>();

	// Token: 0x04002129 RID: 8489
	public static Components.Cmps<Telephone> Telephones = new Components.Cmps<Telephone>();

	// Token: 0x0400212A RID: 8490
	public static Components.Cmps<MissionControlWorkable> MissionControlWorkables = new Components.Cmps<MissionControlWorkable>();

	// Token: 0x0400212B RID: 8491
	public static Components.Cmps<MissionControlClusterWorkable> MissionControlClusterWorkables = new Components.Cmps<MissionControlClusterWorkable>();

	// Token: 0x0400212C RID: 8492
	public static Components.Cmps<MinorFossilDigSite.Instance> MinorFossilDigSites = new Components.Cmps<MinorFossilDigSite.Instance>();

	// Token: 0x0400212D RID: 8493
	public static Components.Cmps<MajorFossilDigSite.Instance> MajorFossilDigSites = new Components.Cmps<MajorFossilDigSite.Instance>();

	// Token: 0x0400212E RID: 8494
	public static Components.Cmps<GameObject> FoodRehydrators = new Components.Cmps<GameObject>();

	// Token: 0x0400212F RID: 8495
	public static Components.CmpsByWorld<SocialGatheringPoint> SocialGatheringPoints = new Components.CmpsByWorld<SocialGatheringPoint>();

	// Token: 0x04002130 RID: 8496
	public static Components.CmpsByWorld<Geyser> Geysers = new Components.CmpsByWorld<Geyser>();

	// Token: 0x04002131 RID: 8497
	public static Components.CmpsByWorld<GeoTuner.Instance> GeoTuners = new Components.CmpsByWorld<GeoTuner.Instance>();

	// Token: 0x04002132 RID: 8498
	public static Components.CmpsByWorld<CritterCondo.Instance> CritterCondos = new Components.CmpsByWorld<CritterCondo.Instance>();

	// Token: 0x04002133 RID: 8499
	public static Components.CmpsByWorld<GeothermalController> GeothermalControllers = new Components.CmpsByWorld<GeothermalController>();

	// Token: 0x04002134 RID: 8500
	public static Components.CmpsByWorld<GeothermalVent> GeothermalVents = new Components.CmpsByWorld<GeothermalVent>();

	// Token: 0x04002135 RID: 8501
	public static Components.CmpsByWorld<RemoteWorkerDock> RemoteWorkerDocks = new Components.CmpsByWorld<RemoteWorkerDock>();

	// Token: 0x04002136 RID: 8502
	public static Components.CmpsByWorld<IRemoteDockWorkTarget> RemoteDockWorkTargets = new Components.CmpsByWorld<IRemoteDockWorkTarget>();

	// Token: 0x04002137 RID: 8503
	public static Components.Cmps<Assignable> AssignableItems = new Components.Cmps<Assignable>();

	// Token: 0x04002138 RID: 8504
	public static Components.CmpsByWorld<Comet> Meteors = new Components.CmpsByWorld<Comet>();

	// Token: 0x04002139 RID: 8505
	public static Components.CmpsByWorld<DetectorNetwork.Instance> DetectorNetworks = new Components.CmpsByWorld<DetectorNetwork.Instance>();

	// Token: 0x0400213A RID: 8506
	public static Components.CmpsByWorld<ScannerNetworkVisualizer> ScannerVisualizers = new Components.CmpsByWorld<ScannerNetworkVisualizer>();

	// Token: 0x0400213B RID: 8507
	public static Components.Cmps<IncubationMonitor.Instance> IncubationMonitors = new Components.Cmps<IncubationMonitor.Instance>();

	// Token: 0x0400213C RID: 8508
	public static Components.Cmps<FixedCapturableMonitor.Instance> FixedCapturableMonitors = new Components.Cmps<FixedCapturableMonitor.Instance>();

	// Token: 0x0400213D RID: 8509
	public static Components.Cmps<BeeHive.StatesInstance> BeeHives = new Components.Cmps<BeeHive.StatesInstance>();

	// Token: 0x0400213E RID: 8510
	public static Components.Cmps<StateMachine.Instance> EffectImmunityProviderStations = new Components.Cmps<StateMachine.Instance>();

	// Token: 0x0400213F RID: 8511
	public static Components.Cmps<PeeChoreMonitor.Instance> CriticalBladders = new Components.Cmps<PeeChoreMonitor.Instance>();

	// Token: 0x0200169A RID: 5786
	public class Cmps<T> : ICollection, IEnumerable, IEnumerable<T>
	{
		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060092DD RID: 37597 RVA: 0x0035708B File Offset: 0x0035528B
		public List<T> Items
		{
			get
			{
				return this.items.GetDataList();
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060092DE RID: 37598 RVA: 0x00357098 File Offset: 0x00355298
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x060092DF RID: 37599 RVA: 0x003570A5 File Offset: 0x003552A5
		public Cmps()
		{
			App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.Clear));
			this.items = new KCompactedVector<T>(0);
			this.table = new Dictionary<T, HandleVector<int>.Handle>();
		}

		// Token: 0x17000A0A RID: 2570
		public T this[int idx]
		{
			get
			{
				return this.Items[idx];
			}
		}

		// Token: 0x060092E1 RID: 37601 RVA: 0x003570F2 File Offset: 0x003552F2
		private void Clear()
		{
			this.items.Clear();
			this.table.Clear();
			this.OnAdd = null;
			this.OnRemove = null;
		}

		// Token: 0x060092E2 RID: 37602 RVA: 0x00357118 File Offset: 0x00355318
		public void Add(T cmp)
		{
			HandleVector<int>.Handle value = this.items.Allocate(cmp);
			this.table[cmp] = value;
			if (this.OnAdd != null)
			{
				this.OnAdd(cmp);
			}
		}

		// Token: 0x060092E3 RID: 37603 RVA: 0x00357154 File Offset: 0x00355354
		public void Remove(T cmp)
		{
			HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
			if (this.table.TryGetValue(cmp, out invalidHandle))
			{
				this.table.Remove(cmp);
				this.items.Free(invalidHandle);
				if (this.OnRemove != null)
				{
					this.OnRemove(cmp);
				}
			}
		}

		// Token: 0x060092E4 RID: 37604 RVA: 0x003571A8 File Offset: 0x003553A8
		public void Register(Action<T> on_add, Action<T> on_remove)
		{
			this.OnAdd += on_add;
			this.OnRemove += on_remove;
			foreach (T obj in this.Items)
			{
				this.OnAdd(obj);
			}
		}

		// Token: 0x060092E5 RID: 37605 RVA: 0x00357210 File Offset: 0x00355410
		public void Unregister(Action<T> on_add, Action<T> on_remove)
		{
			this.OnAdd -= on_add;
			this.OnRemove -= on_remove;
		}

		// Token: 0x060092E6 RID: 37606 RVA: 0x00357220 File Offset: 0x00355420
		public List<T> GetWorldItems(int worldId, bool checkChildWorlds = false)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				otherWorldIds = ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds();
			}
			return this.GetWorldItems(worldId, otherWorldIds, null);
		}

		// Token: 0x060092E7 RID: 37607 RVA: 0x0035724C File Offset: 0x0035544C
		public List<T> GetWorldItems(int worldId, bool checkChildWorlds, Func<T, bool> filter)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				otherWorldIds = ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds();
			}
			return this.GetWorldItems(worldId, otherWorldIds, filter);
		}

		// Token: 0x060092E8 RID: 37608 RVA: 0x00357278 File Offset: 0x00355478
		public List<T> GetWorldItems(int worldId, ICollection<int> otherWorldIds, Func<T, bool> filter)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < this.Items.Count; i++)
			{
				T t = this.Items[i];
				int myWorldId = (t as KMonoBehaviour).GetMyWorldId();
				bool flag = worldId == myWorldId;
				if (!flag && otherWorldIds != null && otherWorldIds.Contains(myWorldId))
				{
					flag = true;
				}
				if (flag && filter != null)
				{
					flag = filter(t);
				}
				if (flag)
				{
					list.Add(t);
				}
			}
			return list;
		}

		// Token: 0x060092E9 RID: 37609 RVA: 0x003572F4 File Offset: 0x003554F4
		public IEnumerable<T> WorldItemsEnumerate(int worldId, bool checkChildWorlds = false)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				otherWorldIds = ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds();
			}
			return this.WorldItemsEnumerate(worldId, otherWorldIds);
		}

		// Token: 0x060092EA RID: 37610 RVA: 0x0035731F File Offset: 0x0035551F
		public IEnumerable<T> WorldItemsEnumerate(int worldId, ICollection<int> otherWorldIds = null)
		{
			int num;
			for (int index = 0; index < this.Items.Count; index = num + 1)
			{
				T t = this.Items[index];
				int myWorldId = (t as KMonoBehaviour).GetMyWorldId();
				if (myWorldId == worldId || (otherWorldIds != null && otherWorldIds.Contains(myWorldId)))
				{
					yield return t;
				}
				num = index;
			}
			yield break;
		}

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x060092EB RID: 37611 RVA: 0x00357340 File Offset: 0x00355540
		// (remove) Token: 0x060092EC RID: 37612 RVA: 0x00357378 File Offset: 0x00355578
		public event Action<T> OnAdd;

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x060092ED RID: 37613 RVA: 0x003573B0 File Offset: 0x003555B0
		// (remove) Token: 0x060092EE RID: 37614 RVA: 0x003573E8 File Offset: 0x003555E8
		public event Action<T> OnRemove;

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060092EF RID: 37615 RVA: 0x0035741D File Offset: 0x0035561D
		public bool IsSynchronized
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060092F0 RID: 37616 RVA: 0x00357424 File Offset: 0x00355624
		public object SyncRoot
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060092F1 RID: 37617 RVA: 0x0035742B File Offset: 0x0035562B
		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060092F2 RID: 37618 RVA: 0x00357432 File Offset: 0x00355632
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x060092F3 RID: 37619 RVA: 0x00357444 File Offset: 0x00355644
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x060092F4 RID: 37620 RVA: 0x00357456 File Offset: 0x00355656
		public IEnumerator GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x04007026 RID: 28710
		private Dictionary<T, HandleVector<int>.Handle> table;

		// Token: 0x04007027 RID: 28711
		private KCompactedVector<T> items;
	}

	// Token: 0x0200169B RID: 5787
	public class CmpsByWorld<T>
	{
		// Token: 0x060092F5 RID: 37621 RVA: 0x00357468 File Offset: 0x00355668
		public CmpsByWorld()
		{
			App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.Clear));
			this.m_CmpsByWorld = new Dictionary<int, Components.Cmps<T>>();
		}

		// Token: 0x060092F6 RID: 37622 RVA: 0x0035749B File Offset: 0x0035569B
		public void Clear()
		{
			this.m_CmpsByWorld.Clear();
		}

		// Token: 0x060092F7 RID: 37623 RVA: 0x003574A8 File Offset: 0x003556A8
		public Components.Cmps<T> CreateOrGetCmps(int worldId)
		{
			Components.Cmps<T> cmps;
			if (!this.m_CmpsByWorld.TryGetValue(worldId, out cmps))
			{
				cmps = new Components.Cmps<T>();
				this.m_CmpsByWorld[worldId] = cmps;
			}
			return cmps;
		}

		// Token: 0x060092F8 RID: 37624 RVA: 0x003574D9 File Offset: 0x003556D9
		public void Add(int worldId, T cmp)
		{
			DebugUtil.DevAssertArgs(worldId != -1, new object[]
			{
				"CmpsByWorld tried to add a component to an invalid world. Did you call this during a state machine's constructor instead of StartSM? ",
				cmp
			});
			this.CreateOrGetCmps(worldId).Add(cmp);
		}

		// Token: 0x060092F9 RID: 37625 RVA: 0x0035750B File Offset: 0x0035570B
		public void Remove(int worldId, T cmp)
		{
			this.CreateOrGetCmps(worldId).Remove(cmp);
		}

		// Token: 0x060092FA RID: 37626 RVA: 0x0035751A File Offset: 0x0035571A
		public void Register(int worldId, Action<T> on_add, Action<T> on_remove)
		{
			this.CreateOrGetCmps(worldId).Register(on_add, on_remove);
		}

		// Token: 0x060092FB RID: 37627 RVA: 0x0035752A File Offset: 0x0035572A
		public void Unregister(int worldId, Action<T> on_add, Action<T> on_remove)
		{
			this.CreateOrGetCmps(worldId).Unregister(on_add, on_remove);
		}

		// Token: 0x060092FC RID: 37628 RVA: 0x0035753A File Offset: 0x0035573A
		public List<T> GetItems(int worldId)
		{
			return this.CreateOrGetCmps(worldId).Items;
		}

		// Token: 0x060092FD RID: 37629 RVA: 0x00357548 File Offset: 0x00355748
		public Dictionary<int, Components.Cmps<T>>.KeyCollection GetWorldsIds()
		{
			return this.m_CmpsByWorld.Keys;
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060092FE RID: 37630 RVA: 0x00357558 File Offset: 0x00355758
		public int GlobalCount
		{
			get
			{
				int num = 0;
				foreach (KeyValuePair<int, Components.Cmps<T>> keyValuePair in this.m_CmpsByWorld)
				{
					num += keyValuePair.Value.Count;
				}
				return num;
			}
		}

		// Token: 0x060092FF RID: 37631 RVA: 0x003575B8 File Offset: 0x003557B8
		public int CountWorldItems(int worldId, bool includeChildren = false)
		{
			int num = this.GetItems(worldId).Count;
			if (includeChildren)
			{
				foreach (int worldId2 in ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds())
				{
					num += this.GetItems(worldId2).Count;
				}
			}
			return num;
		}

		// Token: 0x06009300 RID: 37632 RVA: 0x00357628 File Offset: 0x00355828
		public IEnumerable<T> WorldItemsEnumerate(int worldId, bool checkChildWorlds = false)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				otherWorldIds = ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds();
			}
			return this.WorldItemsEnumerate(worldId, otherWorldIds);
		}

		// Token: 0x06009301 RID: 37633 RVA: 0x00357653 File Offset: 0x00355853
		public IEnumerable<T> WorldItemsEnumerate(int worldId, ICollection<int> otherWorldIds = null)
		{
			List<T> items = this.GetItems(worldId);
			int num;
			for (int index = 0; index < items.Count; index = num + 1)
			{
				yield return items[index];
				num = index;
			}
			if (otherWorldIds != null)
			{
				foreach (int worldId2 in otherWorldIds)
				{
					items = this.GetItems(worldId2);
					for (int index = 0; index < items.Count; index = num + 1)
					{
						yield return items[index];
						num = index;
					}
				}
				IEnumerator<int> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400702A RID: 28714
		private Dictionary<int, Components.Cmps<T>> m_CmpsByWorld;
	}
}
