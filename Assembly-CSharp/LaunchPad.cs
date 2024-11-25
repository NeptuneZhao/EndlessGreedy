using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000ACC RID: 2764
public class LaunchPad : KMonoBehaviour, ISim1000ms, IListableOption, IProcessConditionSet
{
	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x0600521B RID: 21019 RVA: 0x001D7510 File Offset: 0x001D5710
	public RocketModuleCluster LandedRocket
	{
		get
		{
			GameObject gameObject = null;
			Grid.ObjectLayers[1].TryGetValue(this.RocketBottomPosition, out gameObject);
			if (gameObject != null)
			{
				RocketModuleCluster component = gameObject.GetComponent<RocketModuleCluster>();
				Clustercraft clustercraft = (component != null && component.CraftInterface != null) ? component.CraftInterface.GetComponent<Clustercraft>() : null;
				if (clustercraft != null && (clustercraft.Status == Clustercraft.CraftStatus.Grounded || clustercraft.Status == Clustercraft.CraftStatus.Landing))
				{
					return component;
				}
			}
			return null;
		}
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x0600521C RID: 21020 RVA: 0x001D7587 File Offset: 0x001D5787
	public int RocketBottomPosition
	{
		get
		{
			return Grid.OffsetCell(Grid.PosToCell(this), this.baseModulePosition);
		}
	}

	// Token: 0x0600521D RID: 21021 RVA: 0x001D759C File Offset: 0x001D579C
	[OnDeserialized]
	private void OnDeserialzed()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 24))
		{
			Building component = base.GetComponent<Building>();
			if (component != null)
			{
				component.RunOnArea(delegate(int cell)
				{
					if (Grid.IsValidCell(cell) && Grid.Solid[cell])
					{
						SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.LaunchpadDesolidify, 0f, -1f, byte.MaxValue, 0, -1);
					}
				});
			}
		}
	}

	// Token: 0x0600521E RID: 21022 RVA: 0x001D75F8 File Offset: 0x001D57F8
	protected override void OnPrefabInit()
	{
		UserNameable component = base.GetComponent<UserNameable>();
		if (component != null)
		{
			component.SetName(GameUtil.GenerateRandomLaunchPadName());
		}
	}

	// Token: 0x0600521F RID: 21023 RVA: 0x001D7620 File Offset: 0x001D5820
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tower = new LaunchPad.LaunchPadTower(this, this.maxTowerHeight);
		this.OnRocketBuildingChanged(this.GetRocketBaseModule());
		this.partitionerEntry = GameScenePartitioner.Instance.Add("LaunchPad.OnSpawn", base.gameObject, Extents.OneCell(this.RocketBottomPosition), GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnRocketBuildingChanged));
		Components.LaunchPads.Add(this);
		this.CheckLandedRocketPassengerModuleStatus();
		int num = ConditionFlightPathIsClear.PadTopEdgeDistanceToCeilingEdge(base.gameObject);
		if (num < 35)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.RocketPlatformCloseToCeiling, num);
		}
	}

	// Token: 0x06005220 RID: 21024 RVA: 0x001D76D4 File Offset: 0x001D58D4
	protected override void OnCleanUp()
	{
		Components.LaunchPads.Remove(this);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.lastBaseAttachable != null)
		{
			AttachableBuilding attachableBuilding = this.lastBaseAttachable;
			attachableBuilding.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachableBuilding.onAttachmentNetworkChanged, new Action<object>(this.OnRocketLayoutChanged));
			this.lastBaseAttachable = null;
		}
		this.RebuildLaunchTowerHeightHandler.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06005221 RID: 21025 RVA: 0x001D774C File Offset: 0x001D594C
	private void CheckLandedRocketPassengerModuleStatus()
	{
		if (this.LandedRocket == null)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.landedRocketPassengerModuleStatusItem, false);
			this.landedRocketPassengerModuleStatusItem = Guid.Empty;
			return;
		}
		if (this.LandedRocket.CraftInterface.GetPassengerModule() == null)
		{
			if (this.landedRocketPassengerModuleStatusItem == Guid.Empty)
			{
				this.landedRocketPassengerModuleStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.LandedRocketLacksPassengerModule, null);
				return;
			}
		}
		else if (this.landedRocketPassengerModuleStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.landedRocketPassengerModuleStatusItem, false);
			this.landedRocketPassengerModuleStatusItem = Guid.Empty;
		}
	}

	// Token: 0x06005222 RID: 21026 RVA: 0x001D7804 File Offset: 0x001D5A04
	public bool IsLogicInputConnected()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(this.triggerPort);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell) != null;
	}

	// Token: 0x06005223 RID: 21027 RVA: 0x001D7838 File Offset: 0x001D5A38
	public void Sim1000ms(float dt)
	{
		LogicPorts component = base.gameObject.GetComponent<LogicPorts>();
		RocketModuleCluster landedRocket = this.LandedRocket;
		if (landedRocket != null && this.IsLogicInputConnected())
		{
			if (component.GetInputValue(this.triggerPort) == 1)
			{
				if (landedRocket.CraftInterface.CheckReadyForAutomatedLaunchCommand())
				{
					landedRocket.CraftInterface.TriggerLaunch(true);
				}
				else
				{
					landedRocket.CraftInterface.CancelLaunch();
				}
			}
			else
			{
				landedRocket.CraftInterface.CancelLaunch();
			}
		}
		this.CheckLandedRocketPassengerModuleStatus();
		component.SendSignal(this.landedRocketPort, (landedRocket != null) ? 1 : 0);
		if (landedRocket != null)
		{
			component.SendSignal(this.statusPort, (landedRocket.CraftInterface.CheckReadyForAutomatedLaunch() || landedRocket.CraftInterface.HasTag(GameTags.RocketNotOnGround)) ? 1 : 0);
			return;
		}
		component.SendSignal(this.statusPort, 0);
	}

	// Token: 0x06005224 RID: 21028 RVA: 0x001D7910 File Offset: 0x001D5B10
	public GameObject AddBaseModule(BuildingDef moduleDefID, IList<Tag> elements)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.baseModulePosition);
		GameObject gameObject;
		if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			gameObject = moduleDefID.Build(cell, Orientation.Neutral, null, elements, 293.15f, true, GameClock.Instance.GetTime());
		}
		else
		{
			gameObject = moduleDefID.TryPlace(null, Grid.CellToPosCBC(cell, moduleDefID.SceneLayer), Orientation.Neutral, elements, 0);
		}
		GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab("Clustercraft"), null, null);
		gameObject2.SetActive(true);
		Clustercraft component = gameObject2.GetComponent<Clustercraft>();
		component.GetComponent<CraftModuleInterface>().AddModule(gameObject.GetComponent<RocketModuleCluster>());
		component.Init(this.GetMyWorldLocation(), this);
		if (gameObject.GetComponent<BuildingUnderConstruction>() != null)
		{
			this.OnRocketBuildingChanged(gameObject);
		}
		base.Trigger(374403796, null);
		return gameObject;
	}

	// Token: 0x06005225 RID: 21029 RVA: 0x001D79E0 File Offset: 0x001D5BE0
	private void OnRocketBuildingChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		RocketModuleCluster landedRocket = this.LandedRocket;
		global::Debug.Assert(gameObject == null || landedRocket == null || landedRocket.gameObject == gameObject, "Launch Pad had a rocket land or take off on it twice??");
		Clustercraft clustercraft = (landedRocket != null && landedRocket.CraftInterface != null) ? landedRocket.CraftInterface.GetComponent<Clustercraft>() : null;
		if (clustercraft != null)
		{
			if (clustercraft.Status == Clustercraft.CraftStatus.Landing)
			{
				base.Trigger(-887025858, landedRocket);
			}
			else if (clustercraft.Status == Clustercraft.CraftStatus.Launching)
			{
				base.Trigger(-1277991738, landedRocket);
				AttachableBuilding component = landedRocket.CraftInterface.ClusterModules[0].Get().GetComponent<AttachableBuilding>();
				component.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(component.onAttachmentNetworkChanged, new Action<object>(this.OnRocketLayoutChanged));
			}
		}
		this.OnRocketLayoutChanged(null);
	}

	// Token: 0x06005226 RID: 21030 RVA: 0x001D7AC4 File Offset: 0x001D5CC4
	private void OnRocketLayoutChanged(object data)
	{
		if (this.lastBaseAttachable != null)
		{
			AttachableBuilding attachableBuilding = this.lastBaseAttachable;
			attachableBuilding.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachableBuilding.onAttachmentNetworkChanged, new Action<object>(this.OnRocketLayoutChanged));
			this.lastBaseAttachable = null;
		}
		GameObject rocketBaseModule = this.GetRocketBaseModule();
		if (rocketBaseModule != null)
		{
			this.lastBaseAttachable = rocketBaseModule.GetComponent<AttachableBuilding>();
			AttachableBuilding attachableBuilding2 = this.lastBaseAttachable;
			attachableBuilding2.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(attachableBuilding2.onAttachmentNetworkChanged, new Action<object>(this.OnRocketLayoutChanged));
		}
		this.DirtyTowerHeight();
	}

	// Token: 0x06005227 RID: 21031 RVA: 0x001D7B56 File Offset: 0x001D5D56
	public bool HasRocket()
	{
		return this.LandedRocket != null;
	}

	// Token: 0x06005228 RID: 21032 RVA: 0x001D7B64 File Offset: 0x001D5D64
	public bool HasRocketWithCommandModule()
	{
		return this.HasRocket() && this.LandedRocket.CraftInterface.FindLaunchableRocket() != null;
	}

	// Token: 0x06005229 RID: 21033 RVA: 0x001D7B88 File Offset: 0x001D5D88
	private GameObject GetRocketBaseModule()
	{
		GameObject gameObject = Grid.Objects[Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.baseModulePosition), 1];
		if (!(gameObject != null) || !(gameObject.GetComponent<RocketModule>() != null))
		{
			return null;
		}
		return gameObject;
	}

	// Token: 0x0600522A RID: 21034 RVA: 0x001D7BD4 File Offset: 0x001D5DD4
	public void DirtyTowerHeight()
	{
		if (!this.dirtyTowerHeight)
		{
			this.dirtyTowerHeight = true;
			if (!this.RebuildLaunchTowerHeightHandler.IsValid)
			{
				this.RebuildLaunchTowerHeightHandler = GameScheduler.Instance.ScheduleNextFrame("RebuildLaunchTowerHeight", new Action<object>(this.RebuildLaunchTowerHeight), null, null);
			}
		}
	}

	// Token: 0x0600522B RID: 21035 RVA: 0x001D7C20 File Offset: 0x001D5E20
	private void RebuildLaunchTowerHeight(object obj)
	{
		RocketModuleCluster landedRocket = this.LandedRocket;
		if (landedRocket != null)
		{
			this.tower.SetTowerHeight(landedRocket.CraftInterface.MaxHeight);
		}
		this.dirtyTowerHeight = false;
		this.RebuildLaunchTowerHeightHandler.ClearScheduler();
	}

	// Token: 0x0600522C RID: 21036 RVA: 0x001D7C65 File Offset: 0x001D5E65
	public string GetProperName()
	{
		return base.gameObject.GetProperName();
	}

	// Token: 0x0600522D RID: 21037 RVA: 0x001D7C74 File Offset: 0x001D5E74
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		RocketProcessConditionDisplayTarget rocketProcessConditionDisplayTarget = null;
		RocketModuleCluster landedRocket = this.LandedRocket;
		if (landedRocket != null)
		{
			for (int i = 0; i < landedRocket.CraftInterface.ClusterModules.Count; i++)
			{
				RocketProcessConditionDisplayTarget component = landedRocket.CraftInterface.ClusterModules[i].Get().GetComponent<RocketProcessConditionDisplayTarget>();
				if (component != null)
				{
					rocketProcessConditionDisplayTarget = component;
					break;
				}
			}
		}
		if (rocketProcessConditionDisplayTarget != null)
		{
			return ((IProcessConditionSet)rocketProcessConditionDisplayTarget).GetConditionSet(conditionType);
		}
		return new List<ProcessCondition>();
	}

	// Token: 0x0600522E RID: 21038 RVA: 0x001D7CF0 File Offset: 0x001D5EF0
	public static List<LaunchPad> GetLaunchPadsForDestination(AxialI destination)
	{
		List<LaunchPad> list = new List<LaunchPad>();
		foreach (object obj in Components.LaunchPads)
		{
			LaunchPad launchPad = (LaunchPad)obj;
			if (launchPad.GetMyWorldLocation() == destination)
			{
				list.Add(launchPad);
			}
		}
		return list;
	}

	// Token: 0x04003630 RID: 13872
	public HashedString triggerPort;

	// Token: 0x04003631 RID: 13873
	public HashedString statusPort;

	// Token: 0x04003632 RID: 13874
	public HashedString landedRocketPort;

	// Token: 0x04003633 RID: 13875
	private CellOffset baseModulePosition = new CellOffset(0, 2);

	// Token: 0x04003634 RID: 13876
	private SchedulerHandle RebuildLaunchTowerHeightHandler;

	// Token: 0x04003635 RID: 13877
	private AttachableBuilding lastBaseAttachable;

	// Token: 0x04003636 RID: 13878
	private LaunchPad.LaunchPadTower tower;

	// Token: 0x04003637 RID: 13879
	[Serialize]
	public int maxTowerHeight;

	// Token: 0x04003638 RID: 13880
	private bool dirtyTowerHeight;

	// Token: 0x04003639 RID: 13881
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400363A RID: 13882
	private Guid landedRocketPassengerModuleStatusItem = Guid.Empty;

	// Token: 0x02001B17 RID: 6935
	public class LaunchPadTower
	{
		// Token: 0x0600A251 RID: 41553 RVA: 0x00385674 File Offset: 0x00383874
		public LaunchPadTower(LaunchPad pad, int startHeight)
		{
			this.pad = pad;
			this.SetTowerHeight(startHeight);
		}

		// Token: 0x0600A252 RID: 41554 RVA: 0x0038572C File Offset: 0x0038392C
		public void AddTowerRow()
		{
			GameObject gameObject = new GameObject("LaunchPadTowerRow");
			gameObject.SetActive(false);
			gameObject.transform.SetParent(this.pad.transform);
			gameObject.transform.SetLocalPosition(Grid.CellSizeInMeters * Vector3.up * (float)(this.towerAnimControllers.Count + this.pad.baseModulePosition.y));
			gameObject.transform.SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Backwall)));
			KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
			kbatchedAnimController.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("rocket_launchpad_tower_kanim")
			};
			gameObject.SetActive(true);
			this.towerAnimControllers.Add(kbatchedAnimController);
			kbatchedAnimController.initialAnim = this.towerBGAnimNames[this.towerAnimControllers.Count % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_off;
			this.animLink = new KAnimLink(this.pad.GetComponent<KAnimControllerBase>(), kbatchedAnimController);
		}

		// Token: 0x0600A253 RID: 41555 RVA: 0x00385850 File Offset: 0x00383A50
		public void RemoveTowerRow()
		{
		}

		// Token: 0x0600A254 RID: 41556 RVA: 0x00385854 File Offset: 0x00383A54
		public void SetTowerHeight(int height)
		{
			if (height < 8)
			{
				height = 0;
			}
			this.targetHeight = height;
			this.pad.maxTowerHeight = height;
			while (this.targetHeight > this.towerAnimControllers.Count)
			{
				this.AddTowerRow();
			}
			if (this.activeAnimationRoutine != null)
			{
				this.pad.StopCoroutine(this.activeAnimationRoutine);
			}
			this.activeAnimationRoutine = this.pad.StartCoroutine(this.TowerRoutine());
		}

		// Token: 0x0600A255 RID: 41557 RVA: 0x003858C6 File Offset: 0x00383AC6
		private IEnumerator TowerRoutine()
		{
			while (this.currentHeight < this.targetHeight)
			{
				LaunchPad.LaunchPadTower.<>c__DisplayClass15_0 CS$<>8__locals1 = new LaunchPad.LaunchPadTower.<>c__DisplayClass15_0();
				CS$<>8__locals1.animComplete = false;
				this.towerAnimControllers[this.currentHeight].Queue(this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_on_pre, KAnim.PlayMode.Once, 1f, 0f);
				this.towerAnimControllers[this.currentHeight].Queue(this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_on, KAnim.PlayMode.Once, 1f, 0f);
				this.towerAnimControllers[this.currentHeight].onAnimComplete += delegate(HashedString arg)
				{
					CS$<>8__locals1.animComplete = true;
				};
				float delay = 0.25f;
				while (!CS$<>8__locals1.animComplete && delay > 0f)
				{
					delay -= Time.deltaTime;
					yield return 0;
				}
				this.currentHeight++;
				CS$<>8__locals1 = null;
			}
			while (this.currentHeight > this.targetHeight)
			{
				LaunchPad.LaunchPadTower.<>c__DisplayClass15_1 CS$<>8__locals2 = new LaunchPad.LaunchPadTower.<>c__DisplayClass15_1();
				this.currentHeight--;
				CS$<>8__locals2.animComplete = false;
				this.towerAnimControllers[this.currentHeight].Queue(this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_off_pre, KAnim.PlayMode.Once, 1f, 0f);
				this.towerAnimControllers[this.currentHeight].Queue(this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_off, KAnim.PlayMode.Once, 1f, 0f);
				this.towerAnimControllers[this.currentHeight].onAnimComplete += delegate(HashedString arg)
				{
					CS$<>8__locals2.animComplete = true;
				};
				float delay = 0.25f;
				while (!CS$<>8__locals2.animComplete && delay > 0f)
				{
					delay -= Time.deltaTime;
					yield return 0;
				}
				CS$<>8__locals2 = null;
			}
			yield return 0;
			yield break;
		}

		// Token: 0x04007EC0 RID: 32448
		private LaunchPad pad;

		// Token: 0x04007EC1 RID: 32449
		private KAnimLink animLink;

		// Token: 0x04007EC2 RID: 32450
		private Coroutine activeAnimationRoutine;

		// Token: 0x04007EC3 RID: 32451
		private string[] towerBGAnimNames = new string[]
		{
			"A1",
			"A2",
			"A3",
			"B",
			"C",
			"D",
			"E1",
			"E2",
			"F1",
			"F2"
		};

		// Token: 0x04007EC4 RID: 32452
		private string towerBGAnimSuffix_on = "_on";

		// Token: 0x04007EC5 RID: 32453
		private string towerBGAnimSuffix_on_pre = "_on_pre";

		// Token: 0x04007EC6 RID: 32454
		private string towerBGAnimSuffix_off_pre = "_off_pre";

		// Token: 0x04007EC7 RID: 32455
		private string towerBGAnimSuffix_off = "_off";

		// Token: 0x04007EC8 RID: 32456
		private List<KBatchedAnimController> towerAnimControllers = new List<KBatchedAnimController>();

		// Token: 0x04007EC9 RID: 32457
		private int targetHeight;

		// Token: 0x04007ECA RID: 32458
		private int currentHeight;
	}
}
