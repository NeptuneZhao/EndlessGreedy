using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000696 RID: 1686
public class ClusterTelescope : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>
{
	// Token: 0x06002A1F RID: 10783 RVA: 0x000ED554 File Offset: 0x000EB754
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.ready.no_visibility;
		this.root.Update(delegate(ClusterTelescope.Instance smi, float dt)
		{
			KSelectable component = smi.GetComponent<KSelectable>();
			bool flag = Mathf.Approximately(0f, smi.PercentClear) || smi.PercentClear < 0f;
			bool flag2 = Mathf.Approximately(1f, smi.PercentClear) || smi.PercentClear > 1f;
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, flag, smi);
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, !flag && !flag2, smi);
		}, UpdateRate.SIM_200ms, false);
		this.ready.DoNothing();
		this.ready.no_visibility.UpdateTransition(this.ready.ready_to_work, (ClusterTelescope.Instance smi, float dt) => smi.HasSkyVisibility(), UpdateRate.SIM_200ms, false);
		this.ready.ready_to_work.UpdateTransition(this.ready.no_visibility, (ClusterTelescope.Instance smi, float dt) => !smi.HasSkyVisibility(), UpdateRate.SIM_200ms, false).DefaultState(this.ready.ready_to_work.decide);
		this.ready.ready_to_work.decide.EnterTransition(this.ready.ready_to_work.identifyMeteorShower, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification()).EnterTransition(this.ready.ready_to_work.revealTile, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnRevealingTile()).EnterTransition(this.all_work_complete, (ClusterTelescope.Instance smi) => !smi.IsAnyAvailableWorkToBeDone());
		this.ready.ready_to_work.identifyMeteorShower.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnMeteorIdentification()).ParamTransition<GameObject>(this.meteorShowerTarget, this.ready.ready_to_work.decide, GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.IsNull).EventTransition(GameHashes.ClusterMapMeteorShowerIdentified, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnMeteorIdentification()).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnMeteorIdentification()).ToggleChore((ClusterTelescope.Instance smi) => smi.CreateIdentifyMeteorChore(), this.ready.no_visibility);
		this.ready.ready_to_work.revealTile.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification()).EventTransition(GameHashes.ClusterFogOfWarRevealed, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnRevealingTile()).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification()).ToggleChore((ClusterTelescope.Instance smi) => smi.CreateRevealTileChore(), this.ready.no_visibility);
		this.all_work_complete.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.no_visibility, (ClusterTelescope.Instance smi) => smi.IsAnyAvailableWorkToBeDone()).ToggleMainStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeAllWorkComplete, null).EventTransition(GameHashes.ClusterLocationChanged, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.no_visibility, (ClusterTelescope.Instance smi) => smi.IsAnyAvailableWorkToBeDone()).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.no_visibility, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification());
	}

	// Token: 0x0400183F RID: 6207
	public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State all_work_complete;

	// Token: 0x04001840 RID: 6208
	public ClusterTelescope.ReadyStates ready;

	// Token: 0x04001841 RID: 6209
	public StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.TargetParameter meteorShowerTarget;

	// Token: 0x04001842 RID: 6210
	public StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Signal MeteorIdenificationPriorityChangeSignal;

	// Token: 0x02001488 RID: 5256
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006A16 RID: 27158
		public int clearScanCellRadius = 15;

		// Token: 0x04006A17 RID: 27159
		public int analyzeClusterRadius = 3;

		// Token: 0x04006A18 RID: 27160
		public KAnimFile[] workableOverrideAnims;

		// Token: 0x04006A19 RID: 27161
		public bool providesOxygen;

		// Token: 0x04006A1A RID: 27162
		public SkyVisibilityInfo skyVisibilityInfo;
	}

	// Token: 0x02001489 RID: 5257
	public class WorkStates : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State
	{
		// Token: 0x04006A1B RID: 27163
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State decide;

		// Token: 0x04006A1C RID: 27164
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State identifyMeteorShower;

		// Token: 0x04006A1D RID: 27165
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State revealTile;
	}

	// Token: 0x0200148A RID: 5258
	public class ReadyStates : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State
	{
		// Token: 0x04006A1E RID: 27166
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State no_visibility;

		// Token: 0x04006A1F RID: 27167
		public ClusterTelescope.WorkStates ready_to_work;
	}

	// Token: 0x0200148B RID: 5259
	public new class Instance : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.GameInstance, ICheckboxControl, BuildingStatusItems.ISkyVisInfo
	{
		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06008AFC RID: 35580 RVA: 0x003353B0 File Offset: 0x003335B0
		public float PercentClear
		{
			get
			{
				return this.m_percentClear;
			}
		}

		// Token: 0x06008AFD RID: 35581 RVA: 0x003353B8 File Offset: 0x003335B8
		float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
		{
			return this.m_percentClear;
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06008AFE RID: 35582 RVA: 0x003353C0 File Offset: 0x003335C0
		private bool hasMeteorShowerTarget
		{
			get
			{
				return this.meteorShowerTarget != null;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06008AFF RID: 35583 RVA: 0x003353CB File Offset: 0x003335CB
		private ClusterMapMeteorShower.Instance meteorShowerTarget
		{
			get
			{
				GameObject gameObject = base.sm.meteorShowerTarget.Get(this);
				if (gameObject == null)
				{
					return null;
				}
				return gameObject.GetSMI<ClusterMapMeteorShower.Instance>();
			}
		}

		// Token: 0x06008B00 RID: 35584 RVA: 0x003353E9 File Offset: 0x003335E9
		public Instance(IStateMachineTarget smi, ClusterTelescope.Def def) : base(smi, def)
		{
			this.workableOverrideAnims = def.workableOverrideAnims;
			this.providesOxygen = def.providesOxygen;
		}

		// Token: 0x06008B01 RID: 35585 RVA: 0x00335412 File Offset: 0x00333612
		public bool ShouldBeWorkingOnRevealingTile()
		{
			return this.CheckHasAnalyzeTarget() && (!this.allowMeteorIdentification || !this.CheckHasValidMeteorTarget());
		}

		// Token: 0x06008B02 RID: 35586 RVA: 0x00335431 File Offset: 0x00333631
		public bool ShouldBeWorkingOnMeteorIdentification()
		{
			return this.allowMeteorIdentification && this.CheckHasValidMeteorTarget();
		}

		// Token: 0x06008B03 RID: 35587 RVA: 0x00335443 File Offset: 0x00333643
		public bool IsAnyAvailableWorkToBeDone()
		{
			return this.CheckHasAnalyzeTarget() || this.ShouldBeWorkingOnMeteorIdentification();
		}

		// Token: 0x06008B04 RID: 35588 RVA: 0x00335458 File Offset: 0x00333658
		public bool CheckHasValidMeteorTarget()
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			if (this.HasValidMeteor())
			{
				return true;
			}
			ClusterMapMeteorShower.Instance instance = null;
			AxialI myWorldLocation = this.GetMyWorldLocation();
			ClusterGrid.Instance.GetVisibleUnidentifiedMeteorShowerWithinRadius(myWorldLocation, base.def.analyzeClusterRadius, out instance);
			base.sm.meteorShowerTarget.Set((instance == null) ? null : instance.gameObject, this, false);
			return instance != null;
		}

		// Token: 0x06008B05 RID: 35589 RVA: 0x003354C0 File Offset: 0x003336C0
		public bool CheckHasAnalyzeTarget()
		{
			ClusterFogOfWarManager.Instance smi = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			if (this.m_hasAnalyzeTarget && !smi.IsLocationRevealed(this.m_analyzeTarget))
			{
				return true;
			}
			AxialI myWorldLocation = this.GetMyWorldLocation();
			this.m_hasAnalyzeTarget = smi.GetUnrevealedLocationWithinRadius(myWorldLocation, base.def.analyzeClusterRadius, out this.m_analyzeTarget);
			return this.m_hasAnalyzeTarget;
		}

		// Token: 0x06008B06 RID: 35590 RVA: 0x0033551C File Offset: 0x0033371C
		private bool HasValidMeteor()
		{
			if (!this.hasMeteorShowerTarget)
			{
				return false;
			}
			AxialI myWorldLocation = this.GetMyWorldLocation();
			bool flag = ClusterGrid.Instance.IsInRange(this.meteorShowerTarget.ClusterGridPosition(), myWorldLocation, base.def.analyzeClusterRadius);
			bool flag2 = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().IsLocationRevealed(this.meteorShowerTarget.ClusterGridPosition());
			bool hasBeenIdentified = this.meteorShowerTarget.HasBeenIdentified;
			return flag && flag2 && !hasBeenIdentified;
		}

		// Token: 0x06008B07 RID: 35591 RVA: 0x00335594 File Offset: 0x00333794
		public Chore CreateRevealTileChore()
		{
			WorkChore<ClusterTelescope.ClusterTelescopeWorkable> workChore = new WorkChore<ClusterTelescope.ClusterTelescopeWorkable>(Db.Get().ChoreTypes.Research, this.m_workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			if (this.providesOxygen)
			{
				workChore.AddPrecondition(Telescope.ContainsOxygen, null);
			}
			return workChore;
		}

		// Token: 0x06008B08 RID: 35592 RVA: 0x003355E4 File Offset: 0x003337E4
		public Chore CreateIdentifyMeteorChore()
		{
			WorkChore<ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable> workChore = new WorkChore<ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable>(Db.Get().ChoreTypes.Research, this.m_identifyMeteorWorkable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			if (this.providesOxygen)
			{
				workChore.AddPrecondition(Telescope.ContainsOxygen, null);
			}
			return workChore;
		}

		// Token: 0x06008B09 RID: 35593 RVA: 0x00335632 File Offset: 0x00333832
		public ClusterMapMeteorShower.Instance GetMeteorTarget()
		{
			return this.meteorShowerTarget;
		}

		// Token: 0x06008B0A RID: 35594 RVA: 0x0033563A File Offset: 0x0033383A
		public AxialI GetAnalyzeTarget()
		{
			global::Debug.Assert(this.m_hasAnalyzeTarget, "GetAnalyzeTarget called but this telescope has no target assigned.");
			return this.m_analyzeTarget;
		}

		// Token: 0x06008B0B RID: 35595 RVA: 0x00335654 File Offset: 0x00333854
		public bool HasSkyVisibility()
		{
			ValueTuple<bool, float> visibilityOf = base.def.skyVisibilityInfo.GetVisibilityOf(base.gameObject);
			bool item = visibilityOf.Item1;
			float item2 = visibilityOf.Item2;
			this.m_percentClear = item2;
			return item;
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06008B0C RID: 35596 RVA: 0x0033568C File Offset: 0x0033388C
		public string CheckboxTitleKey
		{
			get
			{
				return "STRINGS.UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.TITLE";
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06008B0D RID: 35597 RVA: 0x00335693 File Offset: 0x00333893
		public string CheckboxLabel
		{
			get
			{
				return UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.CHECKBOX_METEORS;
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06008B0E RID: 35598 RVA: 0x0033569F File Offset: 0x0033389F
		public string CheckboxTooltip
		{
			get
			{
				return UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.CHECKBOX_TOOLTIP_METEORS;
			}
		}

		// Token: 0x06008B0F RID: 35599 RVA: 0x003356AB File Offset: 0x003338AB
		public bool GetCheckboxValue()
		{
			return this.allowMeteorIdentification;
		}

		// Token: 0x06008B10 RID: 35600 RVA: 0x003356B3 File Offset: 0x003338B3
		public void SetCheckboxValue(bool value)
		{
			this.allowMeteorIdentification = value;
			base.sm.MeteorIdenificationPriorityChangeSignal.Trigger(this);
		}

		// Token: 0x04006A20 RID: 27168
		private float m_percentClear;

		// Token: 0x04006A21 RID: 27169
		[Serialize]
		public bool allowMeteorIdentification = true;

		// Token: 0x04006A22 RID: 27170
		[Serialize]
		private bool m_hasAnalyzeTarget;

		// Token: 0x04006A23 RID: 27171
		[Serialize]
		private AxialI m_analyzeTarget;

		// Token: 0x04006A24 RID: 27172
		[MyCmpAdd]
		private ClusterTelescope.ClusterTelescopeWorkable m_workable;

		// Token: 0x04006A25 RID: 27173
		[MyCmpAdd]
		private ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable m_identifyMeteorWorkable;

		// Token: 0x04006A26 RID: 27174
		public KAnimFile[] workableOverrideAnims;

		// Token: 0x04006A27 RID: 27175
		public bool providesOxygen;
	}

	// Token: 0x0200148C RID: 5260
	public class ClusterTelescopeWorkable : Workable, OxygenBreather.IGasProvider
	{
		// Token: 0x06008B11 RID: 35601 RVA: 0x003356D0 File Offset: 0x003338D0
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
			this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
			this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
			this.requiredSkillPerk = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.radiationShielding = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING, STRINGS.BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME, false, false, true);
		}

		// Token: 0x06008B12 RID: 35602 RVA: 0x0033577B File Offset: 0x0033397B
		protected override void OnCleanUp()
		{
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.OnCleanUp();
		}

		// Token: 0x06008B13 RID: 35603 RVA: 0x0033579C File Offset: 0x0033399C
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			base.SetWorkTime(float.PositiveInfinity);
			this.overrideAnims = this.m_telescope.workableOverrideAnims;
		}

		// Token: 0x06008B14 RID: 35604 RVA: 0x00335800 File Offset: 0x00333A00
		private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
		{
			WorkerBase worker = base.worker;
			if (worker == null)
			{
				return;
			}
			KPrefabID component = worker.GetComponent<KPrefabID>();
			OxygenBreather component2 = worker.GetComponent<OxygenBreather>();
			Klei.AI.Attributes attributes = worker.GetAttributes();
			KSelectable component3 = base.GetComponent<KSelectable>();
			if (ev == Workable.WorkableEvent.WorkStarted)
			{
				base.ShowProgressBar(true);
				this.progressBar.SetUpdateFunc(() => this.m_fowManager.GetRevealCompleteFraction(this.currentTarget));
				this.currentTarget = this.m_telescope.GetAnalyzeTarget();
				if (!ClusterGrid.Instance.GetEntityOfLayerAtCell(this.currentTarget, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(this.currentTarget);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().SetTargetMeteorShower(null);
				}
				if (this.m_telescope.providesOxygen)
				{
					attributes.Add(this.radiationShielding);
					if (component2 != null)
					{
						this.workerGasProvider = component2.GetGasProvider();
						component2.SetGasProvider(this);
					}
					worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
					component.AddTag(GameTags.Shaded, false);
				}
				base.GetComponent<Operational>().SetActive(true, false);
				this.checkMarkerFrequency = UnityEngine.Random.Range(2f, 5f);
				component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
				return;
			}
			if (ev != Workable.WorkableEvent.WorkStopped)
			{
				return;
			}
			if (this.m_telescope.providesOxygen)
			{
				attributes.Remove(this.radiationShielding);
				if (component2 != null)
				{
					component2.SetGasProvider(this.workerGasProvider);
				}
				worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
				component.RemoveTag(GameTags.Shaded);
			}
			base.GetComponent<Operational>().SetActive(false, false);
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.ShowProgressBar(false);
			component3.RemoveStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
		}

		// Token: 0x06008B15 RID: 35605 RVA: 0x003359F4 File Offset: 0x00333BF4
		public override List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> descriptors = base.GetDescriptors(go);
			Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
			descriptors.Add(item);
			return descriptors;
		}

		// Token: 0x06008B16 RID: 35606 RVA: 0x00335A4F File Offset: 0x00333C4F
		public override float GetEfficiencyMultiplier(WorkerBase worker)
		{
			return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.m_telescope.PercentClear);
		}

		// Token: 0x06008B17 RID: 35607 RVA: 0x00335A6C File Offset: 0x00333C6C
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			AxialI analyzeTarget = this.m_telescope.GetAnalyzeTarget();
			bool flag = false;
			if (analyzeTarget != this.currentTarget)
			{
				if (this.telescopeTargetMarker)
				{
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(analyzeTarget);
				}
				this.currentTarget = analyzeTarget;
				flag = true;
			}
			if (!flag && this.checkMarkerTimer > this.checkMarkerFrequency)
			{
				this.checkMarkerTimer = 0f;
				if (!this.telescopeTargetMarker && !ClusterGrid.Instance.GetEntityOfLayerAtCell(this.currentTarget, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(this.currentTarget);
				}
			}
			this.checkMarkerTimer += dt;
			float num = ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL / ROCKETRY.CLUSTER_FOW.DEFAULT_CYCLES_PER_REVEAL / 600f;
			float points = dt * num;
			this.m_fowManager.EarnRevealPointsForLocation(this.currentTarget, points);
			return base.OnWorkTick(worker, dt);
		}

		// Token: 0x06008B18 RID: 35608 RVA: 0x00335B7A File Offset: 0x00333D7A
		public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06008B19 RID: 35609 RVA: 0x00335B7C File Offset: 0x00333D7C
		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06008B1A RID: 35610 RVA: 0x00335B7E File Offset: 0x00333D7E
		public bool ShouldEmitCO2()
		{
			return false;
		}

		// Token: 0x06008B1B RID: 35611 RVA: 0x00335B81 File Offset: 0x00333D81
		public bool ShouldStoreCO2()
		{
			return false;
		}

		// Token: 0x06008B1C RID: 35612 RVA: 0x00335B84 File Offset: 0x00333D84
		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			if (this.storage.items.Count <= 0)
			{
				return false;
			}
			GameObject gameObject = this.storage.items[0];
			if (gameObject == null)
			{
				return false;
			}
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			bool result = component.Mass >= amount;
			component.Mass = Mathf.Max(0f, component.Mass - amount);
			return result;
		}

		// Token: 0x06008B1D RID: 35613 RVA: 0x00335BF0 File Offset: 0x00333DF0
		public bool IsLowOxygen()
		{
			if (this.storage.items.Count <= 0)
			{
				return true;
			}
			PrimaryElement primaryElement = this.storage.FindFirstWithMass(GameTags.Breathable, 0f);
			return primaryElement == null || primaryElement.Mass == 0f;
		}

		// Token: 0x04006A28 RID: 27176
		[MySmiReq]
		private ClusterTelescope.Instance m_telescope;

		// Token: 0x04006A29 RID: 27177
		private ClusterFogOfWarManager.Instance m_fowManager;

		// Token: 0x04006A2A RID: 27178
		private GameObject telescopeTargetMarker;

		// Token: 0x04006A2B RID: 27179
		private AxialI currentTarget;

		// Token: 0x04006A2C RID: 27180
		private OxygenBreather.IGasProvider workerGasProvider;

		// Token: 0x04006A2D RID: 27181
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04006A2E RID: 27182
		private AttributeModifier radiationShielding;

		// Token: 0x04006A2F RID: 27183
		private float checkMarkerTimer;

		// Token: 0x04006A30 RID: 27184
		private float checkMarkerFrequency = 1f;
	}

	// Token: 0x0200148D RID: 5261
	public class ClusterTelescopeIdentifyMeteorWorkable : Workable, OxygenBreather.IGasProvider
	{
		// Token: 0x06008B20 RID: 35616 RVA: 0x00335C68 File Offset: 0x00333E68
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
			this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
			this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
			this.requiredSkillPerk = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.radiationShielding = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING, STRINGS.BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME, false, false, true);
		}

		// Token: 0x06008B21 RID: 35617 RVA: 0x00335D13 File Offset: 0x00333F13
		protected override void OnCleanUp()
		{
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.OnCleanUp();
		}

		// Token: 0x06008B22 RID: 35618 RVA: 0x00335D34 File Offset: 0x00333F34
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			base.SetWorkTime(float.PositiveInfinity);
			this.overrideAnims = this.m_telescope.workableOverrideAnims;
		}

		// Token: 0x06008B23 RID: 35619 RVA: 0x00335D98 File Offset: 0x00333F98
		private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
		{
			WorkerBase worker = base.worker;
			if (worker == null)
			{
				return;
			}
			KPrefabID component = worker.GetComponent<KPrefabID>();
			OxygenBreather component2 = worker.GetComponent<OxygenBreather>();
			Klei.AI.Attributes attributes = worker.GetAttributes();
			KSelectable component3 = base.GetComponent<KSelectable>();
			if (ev == Workable.WorkableEvent.WorkStarted)
			{
				base.ShowProgressBar(true);
				this.progressBar.SetUpdateFunc(delegate
				{
					if (this.currentTarget == null)
					{
						return 0f;
					}
					return this.currentTarget.IdentifyingProgress;
				});
				this.currentTarget = this.m_telescope.GetMeteorTarget();
				AxialI axialI = this.currentTarget.ClusterGridPosition();
				if (!ClusterGrid.Instance.GetEntityOfLayerAtCell(axialI, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					TelescopeTarget component4 = this.telescopeTargetMarker.GetComponent<TelescopeTarget>();
					component4.Init(axialI);
					component4.SetTargetMeteorShower(this.currentTarget);
				}
				if (this.m_telescope.providesOxygen)
				{
					attributes.Add(this.radiationShielding);
					this.workerGasProvider = component2.GetGasProvider();
					component2.SetGasProvider(this);
					component2.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
					component.AddTag(GameTags.Shaded, false);
				}
				base.GetComponent<Operational>().SetActive(true, false);
				this.checkMarkerFrequency = UnityEngine.Random.Range(2f, 5f);
				component3.AddStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeMeteorWorking, this);
				return;
			}
			if (ev != Workable.WorkableEvent.WorkStopped)
			{
				return;
			}
			if (this.m_telescope.providesOxygen)
			{
				attributes.Remove(this.radiationShielding);
				component2.SetGasProvider(this.workerGasProvider);
				component2.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
				component.RemoveTag(GameTags.Shaded);
			}
			base.GetComponent<Operational>().SetActive(false, false);
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.ShowProgressBar(false);
			component3.RemoveStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeMeteorWorking, this);
		}

		// Token: 0x06008B24 RID: 35620 RVA: 0x00335F7C File Offset: 0x0033417C
		public override List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> descriptors = base.GetDescriptors(go);
			Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
			descriptors.Add(item);
			return descriptors;
		}

		// Token: 0x06008B25 RID: 35621 RVA: 0x00335FD8 File Offset: 0x003341D8
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			ClusterMapMeteorShower.Instance meteorTarget = this.m_telescope.GetMeteorTarget();
			AxialI axialI = meteorTarget.ClusterGridPosition();
			bool flag = false;
			if (meteorTarget != this.currentTarget)
			{
				if (this.telescopeTargetMarker)
				{
					TelescopeTarget component = this.telescopeTargetMarker.GetComponent<TelescopeTarget>();
					component.Init(axialI);
					component.SetTargetMeteorShower(meteorTarget);
				}
				this.currentTarget = meteorTarget;
				flag = true;
			}
			if (!flag && this.checkMarkerTimer > this.checkMarkerFrequency)
			{
				this.checkMarkerTimer = 0f;
				if (!this.telescopeTargetMarker && !ClusterGrid.Instance.GetEntityOfLayerAtCell(axialI, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(axialI);
				}
			}
			this.checkMarkerTimer += dt;
			float num = 20f;
			float points = dt / num;
			this.currentTarget.ProgressIdentifiction(points);
			return base.OnWorkTick(worker, dt);
		}

		// Token: 0x06008B26 RID: 35622 RVA: 0x003360D5 File Offset: 0x003342D5
		public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06008B27 RID: 35623 RVA: 0x003360D7 File Offset: 0x003342D7
		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06008B28 RID: 35624 RVA: 0x003360D9 File Offset: 0x003342D9
		public bool ShouldEmitCO2()
		{
			return false;
		}

		// Token: 0x06008B29 RID: 35625 RVA: 0x003360DC File Offset: 0x003342DC
		public bool ShouldStoreCO2()
		{
			return false;
		}

		// Token: 0x06008B2A RID: 35626 RVA: 0x003360E0 File Offset: 0x003342E0
		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			if (this.storage.items.Count <= 0)
			{
				return false;
			}
			GameObject gameObject = this.storage.items[0];
			if (gameObject == null)
			{
				return false;
			}
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			bool result = component.Mass >= amount;
			component.Mass = Mathf.Max(0f, component.Mass - amount);
			return result;
		}

		// Token: 0x06008B2B RID: 35627 RVA: 0x0033614C File Offset: 0x0033434C
		public bool IsLowOxygen()
		{
			if (this.storage.items.Count <= 0)
			{
				return true;
			}
			GameObject gameObject = this.storage.items[0];
			return !(gameObject == null) && gameObject.GetComponent<PrimaryElement>().Mass > 0f;
		}

		// Token: 0x04006A31 RID: 27185
		[MySmiReq]
		private ClusterTelescope.Instance m_telescope;

		// Token: 0x04006A32 RID: 27186
		private ClusterFogOfWarManager.Instance m_fowManager;

		// Token: 0x04006A33 RID: 27187
		private GameObject telescopeTargetMarker;

		// Token: 0x04006A34 RID: 27188
		private ClusterMapMeteorShower.Instance currentTarget;

		// Token: 0x04006A35 RID: 27189
		private OxygenBreather.IGasProvider workerGasProvider;

		// Token: 0x04006A36 RID: 27190
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04006A37 RID: 27191
		private AttributeModifier radiationShielding;

		// Token: 0x04006A38 RID: 27192
		private float checkMarkerTimer;

		// Token: 0x04006A39 RID: 27193
		private float checkMarkerFrequency = 1f;
	}
}
