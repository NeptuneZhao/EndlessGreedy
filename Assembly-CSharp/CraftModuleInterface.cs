using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000AC4 RID: 2756
[SerializationConfig(MemberSerialization.OptIn)]
public class CraftModuleInterface : KMonoBehaviour, ISim4000ms
{
	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x060051AF RID: 20911 RVA: 0x001D4C08 File Offset: 0x001D2E08
	public IList<Ref<RocketModuleCluster>> ClusterModules
	{
		get
		{
			return this.clusterModules.AsReadOnly();
		}
	}

	// Token: 0x060051B0 RID: 20912 RVA: 0x001D4C15 File Offset: 0x001D2E15
	public LaunchPad GetPreferredLaunchPadForWorld(int world_id)
	{
		if (this.preferredLaunchPad.ContainsKey(world_id))
		{
			return this.preferredLaunchPad[world_id].Get();
		}
		return null;
	}

	// Token: 0x060051B1 RID: 20913 RVA: 0x001D4C38 File Offset: 0x001D2E38
	private void SetPreferredLaunchPadForWorld(LaunchPad pad)
	{
		if (!this.preferredLaunchPad.ContainsKey(pad.GetMyWorldId()))
		{
			this.preferredLaunchPad.Add(this.CurrentPad.GetMyWorldId(), new Ref<LaunchPad>());
		}
		this.preferredLaunchPad[this.CurrentPad.GetMyWorldId()].Set(this.CurrentPad);
	}

	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x060051B2 RID: 20914 RVA: 0x001D4C94 File Offset: 0x001D2E94
	public LaunchPad CurrentPad
	{
		get
		{
			if (this.m_clustercraft != null && this.m_clustercraft.Status != Clustercraft.CraftStatus.InFlight && this.clusterModules.Count > 0)
			{
				if (this.bottomModule == null)
				{
					this.SetBottomModule();
				}
				global::Debug.Assert(this.bottomModule != null && this.bottomModule.Get() != null, "More than one cluster module but no bottom module found.");
				int num = Grid.CellBelow(Grid.PosToCell(this.bottomModule.Get().transform.position));
				if (Grid.IsValidCell(num))
				{
					GameObject gameObject = null;
					Grid.ObjectLayers[1].TryGetValue(num, out gameObject);
					if (gameObject != null)
					{
						return gameObject.GetComponent<LaunchPad>();
					}
				}
			}
			return null;
		}
	}

	// Token: 0x170005FF RID: 1535
	// (get) Token: 0x060051B3 RID: 20915 RVA: 0x001D4D50 File Offset: 0x001D2F50
	public float Speed
	{
		get
		{
			return this.m_clustercraft.Speed;
		}
	}

	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x060051B4 RID: 20916 RVA: 0x001D4D60 File Offset: 0x001D2F60
	public float Range
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (engine != null)
			{
				return this.BurnableMassRemaining / engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
			}
			return 0f;
		}
	}

	// Token: 0x17000601 RID: 1537
	// (get) Token: 0x060051B5 RID: 20917 RVA: 0x001D4D9A File Offset: 0x001D2F9A
	public int RangeInTiles
	{
		get
		{
			return (int)Mathf.Floor((this.Range + 0.001f) / 600f);
		}
	}

	// Token: 0x17000602 RID: 1538
	// (get) Token: 0x060051B6 RID: 20918 RVA: 0x001D4DB4 File Offset: 0x001D2FB4
	public float FuelPerHex
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (engine != null)
			{
				return engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance * 600f;
			}
			return float.PositiveInfinity;
		}
	}

	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x060051B7 RID: 20919 RVA: 0x001D4DF0 File Offset: 0x001D2FF0
	public float BurnableMassRemaining
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (!(engine != null))
			{
				return 0f;
			}
			if (!engine.requireOxidizer)
			{
				return this.FuelRemaining;
			}
			return Mathf.Min(this.FuelRemaining, this.OxidizerPowerRemaining);
		}
	}

	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x060051B8 RID: 20920 RVA: 0x001D4E34 File Offset: 0x001D3034
	public float FuelRemaining
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (engine == null)
			{
				return 0f;
			}
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				IFuelTank component = @ref.Get().GetComponent<IFuelTank>();
				if (!component.IsNullOrDestroyed())
				{
					num += component.Storage.GetAmountAvailable(engine.fuelTag);
				}
			}
			return (float)Mathf.CeilToInt(num);
		}
	}

	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x060051B9 RID: 20921 RVA: 0x001D4ECC File Offset: 0x001D30CC
	public float OxidizerPowerRemaining
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				OxidizerTank component = @ref.Get().GetComponent<OxidizerTank>();
				if (component != null)
				{
					num += component.TotalOxidizerPower;
				}
			}
			return (float)Mathf.CeilToInt(num);
		}
	}

	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x060051BA RID: 20922 RVA: 0x001D4F44 File Offset: 0x001D3144
	public int MaxHeight
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (engine != null)
			{
				return engine.maxHeight;
			}
			return -1;
		}
	}

	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x060051BB RID: 20923 RVA: 0x001D4F69 File Offset: 0x001D3169
	public float TotalBurden
	{
		get
		{
			return this.m_clustercraft.TotalBurden;
		}
	}

	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x060051BC RID: 20924 RVA: 0x001D4F76 File Offset: 0x001D3176
	public float EnginePower
	{
		get
		{
			return this.m_clustercraft.EnginePower;
		}
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x060051BD RID: 20925 RVA: 0x001D4F84 File Offset: 0x001D3184
	public int RocketHeight
	{
		get
		{
			int num = 0;
			foreach (Ref<RocketModuleCluster> @ref in this.ClusterModules)
			{
				num += @ref.Get().GetComponent<Building>().Def.HeightInCells;
			}
			return num;
		}
	}

	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x060051BE RID: 20926 RVA: 0x001D4FE8 File Offset: 0x001D31E8
	public bool HasCargoModule
	{
		get
		{
			using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.ClusterModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Get().GetComponent<CargoBayCluster>() != null)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x060051BF RID: 20927 RVA: 0x001D5040 File Offset: 0x001D3240
	protected override void OnPrefabInit()
	{
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Combine(instance.OnLoad, new Action<Game.GameSaveData>(this.OnLoad));
	}

	// Token: 0x060051C0 RID: 20928 RVA: 0x001D5068 File Offset: 0x001D3268
	protected override void OnSpawn()
	{
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Remove(instance.OnLoad, new Action<Game.GameSaveData>(this.OnLoad));
		if (this.m_clustercraft.Status != Clustercraft.CraftStatus.Grounded)
		{
			this.ForceAttachmentNetwork();
		}
		this.SetBottomModule();
		base.Subscribe(-1311384361, new Action<object>(this.CompleteSelfDestruct));
	}

	// Token: 0x060051C1 RID: 20929 RVA: 0x001D50CC File Offset: 0x001D32CC
	private void OnLoad(Game.GameSaveData data)
	{
		foreach (Ref<RocketModule> @ref in this.modules)
		{
			this.clusterModules.Add(new Ref<RocketModuleCluster>(@ref.Get().GetComponent<RocketModuleCluster>()));
		}
		this.modules.Clear();
		foreach (Ref<RocketModuleCluster> ref2 in this.clusterModules)
		{
			if (!(ref2.Get() == null))
			{
				ref2.Get().CraftInterface = this;
			}
		}
		bool flag = false;
		for (int i = this.clusterModules.Count - 1; i >= 0; i--)
		{
			if (this.clusterModules[i] == null || this.clusterModules[i].Get() == null)
			{
				global::Debug.LogWarning(string.Format("Rocket {0} had a null module at index {1} on load! Why????", base.name, i), this);
				this.clusterModules.RemoveAt(i);
				flag = true;
			}
		}
		this.SetBottomModule();
		if (flag && this.m_clustercraft.Status == Clustercraft.CraftStatus.Grounded)
		{
			global::Debug.LogWarning("The module stack was broken. Collapsing " + base.name + "...", this);
			this.SortModuleListByPosition();
			LaunchPad currentPad = this.CurrentPad;
			if (currentPad != null)
			{
				int num = currentPad.RocketBottomPosition;
				for (int j = 0; j < this.clusterModules.Count; j++)
				{
					RocketModuleCluster rocketModuleCluster = this.clusterModules[j].Get();
					if (num != Grid.PosToCell(rocketModuleCluster.transform.GetPosition()))
					{
						global::Debug.LogWarning(string.Format("Collapsing space under module {0}:{1}", j, rocketModuleCluster.name));
						rocketModuleCluster.transform.SetPosition(Grid.CellToPos(num, CellAlignment.Bottom, Grid.SceneLayer.Building));
					}
					num = Grid.OffsetCell(num, 0, this.clusterModules[j].Get().GetComponent<Building>().Def.HeightInCells);
				}
			}
			for (int k = 0; k < this.clusterModules.Count - 1; k++)
			{
				BuildingAttachPoint component = this.clusterModules[k].Get().GetComponent<BuildingAttachPoint>();
				if (component != null)
				{
					AttachableBuilding component2 = this.clusterModules[k + 1].Get().GetComponent<AttachableBuilding>();
					if (component.points[0].attachedBuilding != component2)
					{
						global::Debug.LogWarning("Reattaching " + component.name + " & " + component2.name);
						component.points[0].attachedBuilding = component2;
					}
				}
			}
		}
	}

	// Token: 0x060051C2 RID: 20930 RVA: 0x001D53C0 File Offset: 0x001D35C0
	public void AddModule(RocketModuleCluster newModule)
	{
		for (int i = 0; i < this.clusterModules.Count; i++)
		{
			if (this.clusterModules[i].Get() == newModule)
			{
				global::Debug.LogError(string.Concat(new string[]
				{
					"Adding module ",
					(newModule != null) ? newModule.ToString() : null,
					" to the same rocket (",
					this.m_clustercraft.Name,
					") twice"
				}));
			}
		}
		this.clusterModules.Add(new Ref<RocketModuleCluster>(newModule));
		newModule.CraftInterface = this;
		base.Trigger(1512695988, newModule);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			if (rocketModuleCluster != null && rocketModuleCluster != newModule)
			{
				rocketModuleCluster.Trigger(1512695988, newModule);
			}
		}
		newModule.Trigger(1512695988, newModule);
		this.SetBottomModule();
	}

	// Token: 0x060051C3 RID: 20931 RVA: 0x001D54DC File Offset: 0x001D36DC
	public void RemoveModule(RocketModuleCluster module)
	{
		for (int i = this.clusterModules.Count - 1; i >= 0; i--)
		{
			if (this.clusterModules[i].Get() == module)
			{
				this.clusterModules.RemoveAt(i);
				break;
			}
		}
		base.Trigger(1512695988, null);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger(1512695988, null);
		}
		this.SetBottomModule();
		if (this.clusterModules.Count == 0)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x060051C4 RID: 20932 RVA: 0x001D55A0 File Offset: 0x001D37A0
	private void SortModuleListByPosition()
	{
		this.clusterModules.Sort(delegate(Ref<RocketModuleCluster> a, Ref<RocketModuleCluster> b)
		{
			if (Grid.CellToPos(Grid.PosToCell(a.Get())).y >= Grid.CellToPos(Grid.PosToCell(b.Get())).y)
			{
				return 1;
			}
			return -1;
		});
	}

	// Token: 0x060051C5 RID: 20933 RVA: 0x001D55CC File Offset: 0x001D37CC
	private void SetBottomModule()
	{
		if (this.clusterModules.Count > 0)
		{
			this.bottomModule = this.clusterModules[0];
			Vector3 vector = this.bottomModule.Get().transform.position;
			using (List<Ref<RocketModuleCluster>>.Enumerator enumerator = this.clusterModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ref<RocketModuleCluster> @ref = enumerator.Current;
					Vector3 position = @ref.Get().transform.position;
					if (position.y < vector.y)
					{
						this.bottomModule = @ref;
						vector = position;
					}
				}
				return;
			}
		}
		this.bottomModule = null;
	}

	// Token: 0x060051C6 RID: 20934 RVA: 0x001D5680 File Offset: 0x001D3880
	public int GetHeightOfModuleTop(GameObject module)
	{
		int num = 0;
		for (int i = 0; i < this.ClusterModules.Count; i++)
		{
			num += this.clusterModules[i].Get().GetComponent<Building>().Def.HeightInCells;
			if (this.clusterModules[i].Get().gameObject == module)
			{
				return num;
			}
		}
		global::Debug.LogError("Could not find module " + module.GetProperName() + " in CraftModuleInterface craft " + this.m_clustercraft.Name);
		return 0;
	}

	// Token: 0x060051C7 RID: 20935 RVA: 0x001D5710 File Offset: 0x001D3910
	public int GetModuleRelativeVerticalPosition(GameObject module)
	{
		int num = 0;
		for (int i = 0; i < this.ClusterModules.Count; i++)
		{
			if (this.clusterModules[i].Get().gameObject == module)
			{
				return num;
			}
			num += this.clusterModules[i].Get().GetComponent<Building>().Def.HeightInCells;
		}
		global::Debug.LogError("Could not find module " + module.GetProperName() + " in CraftModuleInterface craft " + this.m_clustercraft.Name);
		return 0;
	}

	// Token: 0x060051C8 RID: 20936 RVA: 0x001D57A0 File Offset: 0x001D39A0
	public void Sim4000ms(float dt)
	{
		int num = 0;
		foreach (ProcessCondition.ProcessConditionType conditionType in this.conditionsToCheck)
		{
			if (this.EvaluateConditionSet(conditionType) != ProcessCondition.Status.Failure)
			{
				num++;
			}
		}
		if (num != this.lastConditionTypeSucceeded)
		{
			this.lastConditionTypeSucceeded = num;
			this.TriggerEventOnCraftAndRocket(GameHashes.LaunchConditionChanged, null);
		}
	}

	// Token: 0x060051C9 RID: 20937 RVA: 0x001D5818 File Offset: 0x001D3A18
	public bool IsLaunchRequested()
	{
		return this.m_clustercraft.LaunchRequested;
	}

	// Token: 0x060051CA RID: 20938 RVA: 0x001D5825 File Offset: 0x001D3A25
	public bool CheckPreppedForLaunch()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight) > ProcessCondition.Status.Failure;
	}

	// Token: 0x060051CB RID: 20939 RVA: 0x001D5845 File Offset: 0x001D3A45
	public bool CheckReadyToLaunch()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) > ProcessCondition.Status.Failure;
	}

	// Token: 0x060051CC RID: 20940 RVA: 0x001D586E File Offset: 0x001D3A6E
	public bool HasLaunchWarnings()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Warning || this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Warning || this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) == ProcessCondition.Status.Warning;
	}

	// Token: 0x060051CD RID: 20941 RVA: 0x001D5890 File Offset: 0x001D3A90
	public bool CheckReadyForAutomatedLaunchCommand()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Ready;
	}

	// Token: 0x060051CE RID: 20942 RVA: 0x001D58A8 File Offset: 0x001D3AA8
	public bool CheckReadyForAutomatedLaunch()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) == ProcessCondition.Status.Ready;
	}

	// Token: 0x060051CF RID: 20943 RVA: 0x001D58CC File Offset: 0x001D3ACC
	public void TriggerEventOnCraftAndRocket(GameHashes evt, object data)
	{
		base.Trigger((int)evt, data);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger((int)evt, data);
		}
	}

	// Token: 0x060051D0 RID: 20944 RVA: 0x001D592C File Offset: 0x001D3B2C
	public void CancelLaunch()
	{
		this.m_clustercraft.CancelLaunch();
	}

	// Token: 0x060051D1 RID: 20945 RVA: 0x001D5939 File Offset: 0x001D3B39
	public void TriggerLaunch(bool automated = false)
	{
		this.m_clustercraft.RequestLaunch(automated);
	}

	// Token: 0x060051D2 RID: 20946 RVA: 0x001D5948 File Offset: 0x001D3B48
	public void DoLaunch()
	{
		this.SortModuleListByPosition();
		this.CurrentPad.Trigger(705820818, this);
		this.SetPreferredLaunchPadForWorld(this.CurrentPad);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger(705820818, this);
		}
	}

	// Token: 0x060051D3 RID: 20947 RVA: 0x001D59C8 File Offset: 0x001D3BC8
	public void DoLand(LaunchPad pad)
	{
		int num = pad.RocketBottomPosition;
		for (int i = 0; i < this.clusterModules.Count; i++)
		{
			this.clusterModules[i].Get().MoveToPad(num);
			num = Grid.OffsetCell(num, 0, this.clusterModules[i].Get().GetComponent<Building>().Def.HeightInCells);
		}
		this.SetBottomModule();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger(-1165815793, pad);
		}
		pad.Trigger(-1165815793, this);
	}

	// Token: 0x060051D4 RID: 20948 RVA: 0x001D5A94 File Offset: 0x001D3C94
	public LaunchConditionManager FindLaunchConditionManager()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			LaunchConditionManager component = @ref.Get().GetComponent<LaunchConditionManager>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x060051D5 RID: 20949 RVA: 0x001D5AFC File Offset: 0x001D3CFC
	public LaunchableRocketCluster FindLaunchableRocket()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			LaunchableRocketCluster component = rocketModuleCluster.GetComponent<LaunchableRocketCluster>();
			if (component != null && rocketModuleCluster.CraftInterface != null && rocketModuleCluster.CraftInterface.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Grounded)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x060051D6 RID: 20950 RVA: 0x001D5B84 File Offset: 0x001D3D84
	public List<GameObject> GetParts()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			list.Add(@ref.Get().gameObject);
		}
		return list;
	}

	// Token: 0x060051D7 RID: 20951 RVA: 0x001D5BE8 File Offset: 0x001D3DE8
	public RocketEngineCluster GetEngine()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketEngineCluster component = @ref.Get().GetComponent<RocketEngineCluster>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x060051D8 RID: 20952 RVA: 0x001D5C50 File Offset: 0x001D3E50
	public PassengerRocketModule GetPassengerModule()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			PassengerRocketModule component = @ref.Get().GetComponent<PassengerRocketModule>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x060051D9 RID: 20953 RVA: 0x001D5CB8 File Offset: 0x001D3EB8
	public WorldContainer GetInteriorWorld()
	{
		PassengerRocketModule passengerModule = this.GetPassengerModule();
		if (passengerModule == null)
		{
			return null;
		}
		ClustercraftInteriorDoor interiorDoor = passengerModule.GetComponent<ClustercraftExteriorDoor>().GetInteriorDoor();
		if (interiorDoor == null)
		{
			return null;
		}
		return interiorDoor.GetMyWorld();
	}

	// Token: 0x060051DA RID: 20954 RVA: 0x001D5CF4 File Offset: 0x001D3EF4
	public RoboPilotModule GetRobotPilotModule()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RoboPilotModule component = @ref.Get().GetComponent<RoboPilotModule>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x060051DB RID: 20955 RVA: 0x001D5D5C File Offset: 0x001D3F5C
	public RocketClusterDestinationSelector GetClusterDestinationSelector()
	{
		return base.GetComponent<RocketClusterDestinationSelector>();
	}

	// Token: 0x060051DC RID: 20956 RVA: 0x001D5D64 File Offset: 0x001D3F64
	public bool HasClusterDestinationSelector()
	{
		return base.GetComponent<RocketClusterDestinationSelector>() != null;
	}

	// Token: 0x060051DD RID: 20957 RVA: 0x001D5D74 File Offset: 0x001D3F74
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		this.returnConditions.Clear();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			List<ProcessCondition> conditionSet = @ref.Get().GetConditionSet(conditionType);
			if (conditionSet != null)
			{
				this.returnConditions.AddRange(conditionSet);
			}
		}
		if (this.CurrentPad != null)
		{
			List<ProcessCondition> conditionSet2 = this.CurrentPad.GetComponent<LaunchPadConditions>().GetConditionSet(conditionType);
			if (conditionSet2 != null)
			{
				this.returnConditions.AddRange(conditionSet2);
			}
		}
		return this.returnConditions;
	}

	// Token: 0x060051DE RID: 20958 RVA: 0x001D5E1C File Offset: 0x001D401C
	private ProcessCondition.Status EvaluateConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		ProcessCondition.Status status = ProcessCondition.Status.Ready;
		foreach (ProcessCondition processCondition in this.GetConditionSet(conditionType))
		{
			ProcessCondition.Status status2 = processCondition.EvaluateCondition();
			if (status2 < status)
			{
				status = status2;
			}
			if (status == ProcessCondition.Status.Failure)
			{
				break;
			}
		}
		return status;
	}

	// Token: 0x060051DF RID: 20959 RVA: 0x001D5E7C File Offset: 0x001D407C
	private void ForceAttachmentNetwork()
	{
		RocketModuleCluster rocketModuleCluster = null;
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster2 = @ref.Get();
			if (rocketModuleCluster != null)
			{
				BuildingAttachPoint component = rocketModuleCluster.GetComponent<BuildingAttachPoint>();
				AttachableBuilding component2 = rocketModuleCluster2.GetComponent<AttachableBuilding>();
				component.points[0].attachedBuilding = component2;
			}
			rocketModuleCluster = rocketModuleCluster2;
		}
	}

	// Token: 0x060051E0 RID: 20960 RVA: 0x001D5EF8 File Offset: 0x001D40F8
	public static Storage SpawnRocketDebris(string nameSuffix, SimHashes element)
	{
		Vector3 position = new Vector3(-1f, -1f, 0f);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("DebrisPayload"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(element, true);
		gameObject.name += nameSuffix;
		gameObject.SetActive(true);
		return gameObject.GetComponent<Storage>();
	}

	// Token: 0x060051E1 RID: 20961 RVA: 0x001D5F5C File Offset: 0x001D415C
	public void CompleteSelfDestruct(object data = null)
	{
		global::Debug.Assert(this.HasTag(GameTags.RocketInSpace), "Self Destruct is only valid for in-space rockets!");
		SimHashes elementID = this.GetPassengerModule().GetComponent<PrimaryElement>().ElementID;
		List<RocketModule> list = new List<RocketModule>();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			list.Add(@ref.Get());
		}
		List<GameObject> list2 = new List<GameObject>();
		List<GameObject> list3 = new List<GameObject>();
		foreach (RocketModule rocketModule in list)
		{
			foreach (Storage storage in rocketModule.GetComponents<Storage>())
			{
				bool vent_gas = false;
				bool dump_liquid = false;
				List<GameObject> collect_dropped_items = list3;
				storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
				foreach (GameObject gameObject in list3)
				{
					if (gameObject.HasTag(GameTags.Creature))
					{
						Butcherable component = gameObject.GetComponent<Butcherable>();
						if (component != null && component.drops != null && component.drops.Length != 0)
						{
							GameObject[] collection = component.CreateDrops();
							list2.AddRange(collection);
						}
						gameObject.DeleteObject();
					}
					else
					{
						list2.Add(gameObject);
					}
				}
				list3.Clear();
			}
			Deconstructable component2 = rocketModule.GetComponent<Deconstructable>();
			list2.AddRange(component2.ForceDestroyAndGetMaterials());
		}
		List<Storage> list4 = new List<Storage>();
		foreach (GameObject gameObject2 in list2)
		{
			Pickupable component3 = gameObject2.GetComponent<Pickupable>();
			if (component3 != null)
			{
				component3.PrimaryElement.Units = (float)Mathf.Max(1, Mathf.RoundToInt(component3.PrimaryElement.Units * 0.5f));
				if ((list4.Count == 0 || list4[list4.Count - 1].RemainingCapacity() == 0f) && component3.PrimaryElement.Mass > 0f)
				{
					list4.Add(CraftModuleInterface.SpawnRocketDebris(" from CMI", elementID));
				}
				Storage storage2 = list4[list4.Count - 1];
				while (component3.PrimaryElement.Mass > storage2.RemainingCapacity())
				{
					Pickupable pickupable = component3.Take(storage2.RemainingCapacity());
					storage2.Store(pickupable.gameObject, false, false, true, false);
					storage2 = CraftModuleInterface.SpawnRocketDebris(" from CMI", elementID);
					list4.Add(storage2);
				}
				if (component3.PrimaryElement.Mass > 0f)
				{
					storage2.Store(component3.gameObject, false, false, true, false);
				}
			}
		}
		foreach (Storage cmp in list4)
		{
			RailGunPayload.StatesInstance smi = cmp.GetSMI<RailGunPayload.StatesInstance>();
			smi.StartSM();
			smi.Travel(this.m_clustercraft.Location, ClusterUtil.ClosestVisibleAsteroidToLocation(this.m_clustercraft.Location).Location);
		}
		this.m_clustercraft.SetExploding();
	}

	// Token: 0x0400360C RID: 13836
	[Serialize]
	private List<Ref<RocketModule>> modules = new List<Ref<RocketModule>>();

	// Token: 0x0400360D RID: 13837
	[Serialize]
	private List<Ref<RocketModuleCluster>> clusterModules = new List<Ref<RocketModuleCluster>>();

	// Token: 0x0400360E RID: 13838
	private Ref<RocketModuleCluster> bottomModule;

	// Token: 0x0400360F RID: 13839
	[Serialize]
	private Dictionary<int, Ref<LaunchPad>> preferredLaunchPad = new Dictionary<int, Ref<LaunchPad>>();

	// Token: 0x04003610 RID: 13840
	[MyCmpReq]
	private Clustercraft m_clustercraft;

	// Token: 0x04003611 RID: 13841
	private List<ProcessCondition.ProcessConditionType> conditionsToCheck = new List<ProcessCondition.ProcessConditionType>
	{
		ProcessCondition.ProcessConditionType.RocketPrep,
		ProcessCondition.ProcessConditionType.RocketStorage,
		ProcessCondition.ProcessConditionType.RocketBoard,
		ProcessCondition.ProcessConditionType.RocketFlight
	};

	// Token: 0x04003612 RID: 13842
	private int lastConditionTypeSucceeded = -1;

	// Token: 0x04003613 RID: 13843
	private List<ProcessCondition> returnConditions = new List<ProcessCondition>();
}
