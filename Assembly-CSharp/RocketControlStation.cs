using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000760 RID: 1888
public class RocketControlStation : StateMachineComponent<RocketControlStation.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x17000355 RID: 853
	// (get) Token: 0x060032B6 RID: 12982 RVA: 0x00116EF7 File Offset: 0x001150F7
	// (set) Token: 0x060032B7 RID: 12983 RVA: 0x00116EFF File Offset: 0x001150FF
	public bool RestrictWhenGrounded
	{
		get
		{
			return this.m_restrictWhenGrounded;
		}
		set
		{
			this.m_restrictWhenGrounded = value;
			base.Trigger(1861523068, null);
		}
	}

	// Token: 0x060032B8 RID: 12984 RVA: 0x00116F14 File Offset: 0x00115114
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		Components.RocketControlStations.Add(this);
		base.Subscribe<RocketControlStation>(-801688580, RocketControlStation.OnLogicValueChangedDelegate);
		base.Subscribe<RocketControlStation>(1861523068, RocketControlStation.OnRocketRestrictionChanged);
		this.UpdateRestrictionAnimSymbol(null);
	}

	// Token: 0x060032B9 RID: 12985 RVA: 0x00116F66 File Offset: 0x00115166
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RocketControlStations.Remove(this);
	}

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x060032BA RID: 12986 RVA: 0x00116F7C File Offset: 0x0011517C
	public bool BuildingRestrictionsActive
	{
		get
		{
			if (this.IsLogicInputConnected())
			{
				return this.m_logicUsageRestrictionState;
			}
			base.smi.sm.AquireClustercraft(base.smi, false);
			GameObject gameObject = base.smi.sm.clusterCraft.Get(base.smi);
			return this.RestrictWhenGrounded && gameObject != null && gameObject.gameObject.HasTag(GameTags.RocketOnGround);
		}
	}

	// Token: 0x060032BB RID: 12987 RVA: 0x00116FEE File Offset: 0x001151EE
	public bool IsLogicInputConnected()
	{
		return this.GetNetwork() != null;
	}

	// Token: 0x060032BC RID: 12988 RVA: 0x00116FFC File Offset: 0x001151FC
	public void OnLogicValueChanged(object data)
	{
		if (((LogicValueChanged)data).portID == RocketControlStation.PORT_ID)
		{
			LogicCircuitNetwork network = this.GetNetwork();
			int value = (network != null) ? network.OutputValue : 1;
			bool logicUsageRestrictionState = LogicCircuitNetwork.IsBitActive(0, value);
			this.m_logicUsageRestrictionState = logicUsageRestrictionState;
			base.Trigger(1861523068, null);
		}
	}

	// Token: 0x060032BD RID: 12989 RVA: 0x0011704F File Offset: 0x0011524F
	public void OnTagsChanged(object obj)
	{
		if (((TagChangedEventData)obj).tag == GameTags.RocketOnGround)
		{
			base.Trigger(1861523068, null);
		}
	}

	// Token: 0x060032BE RID: 12990 RVA: 0x00117074 File Offset: 0x00115274
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(RocketControlStation.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x060032BF RID: 12991 RVA: 0x001170A2 File Offset: 0x001152A2
	private void UpdateRestrictionAnimSymbol(object o = null)
	{
		base.GetComponent<KAnimControllerBase>().SetSymbolVisiblity("restriction_sign", this.BuildingRestrictionsActive);
	}

	// Token: 0x060032C0 RID: 12992 RVA: 0x001170C0 File Offset: 0x001152C0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.ROCKETRESTRICTION_HEADER, UI.BUILDINGEFFECTS.TOOLTIPS.ROCKETRESTRICTION_HEADER, Descriptor.DescriptorType.Effect, false));
		string newValue = string.Join(", ", (from t in RocketControlStation.CONTROLLED_BUILDINGS
		select Strings.Get("STRINGS.BUILDINGS.PREFABS." + t.Name.ToUpper() + ".NAME").String).ToArray<string>());
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.ROCKETRESTRICTION_BUILDINGS.text.Replace("{buildinglist}", newValue), UI.BUILDINGEFFECTS.TOOLTIPS.ROCKETRESTRICTION_BUILDINGS.text.Replace("{buildinglist}", newValue), Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x04001DF3 RID: 7667
	public static List<Tag> CONTROLLED_BUILDINGS = new List<Tag>();

	// Token: 0x04001DF4 RID: 7668
	private const int UNNETWORKED_VALUE = 1;

	// Token: 0x04001DF5 RID: 7669
	[Serialize]
	public float TimeRemaining;

	// Token: 0x04001DF6 RID: 7670
	private bool m_logicUsageRestrictionState;

	// Token: 0x04001DF7 RID: 7671
	[Serialize]
	private bool m_restrictWhenGrounded;

	// Token: 0x04001DF8 RID: 7672
	public static readonly HashedString PORT_ID = "LogicUsageRestriction";

	// Token: 0x04001DF9 RID: 7673
	private static readonly EventSystem.IntraObjectHandler<RocketControlStation> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<RocketControlStation>(delegate(RocketControlStation component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001DFA RID: 7674
	private static readonly EventSystem.IntraObjectHandler<RocketControlStation> OnRocketRestrictionChanged = new EventSystem.IntraObjectHandler<RocketControlStation>(delegate(RocketControlStation component, object data)
	{
		component.UpdateRestrictionAnimSymbol(data);
	});

	// Token: 0x020015E8 RID: 5608
	public class States : GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation>
	{
		// Token: 0x06009047 RID: 36935 RVA: 0x0034AD08 File Offset: 0x00348F08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			default_state = this.unoperational;
			this.root.Enter("SetTarget", delegate(RocketControlStation.StatesInstance smi)
			{
				this.AquireClustercraft(smi, true);
			}).Target(this.masterTarget).Exit(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 0.5f, 1f);
			});
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 1f, smi.pilotSpeedMult);
			}).PlayAnim("on").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.IsInFlight), UpdateRate.SIM_4000ms).Target(this.clusterCraft).EventTransition(GameHashes.RocketRequestLaunch, this.launch, new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.RocketReadyForLaunch)).EventTransition(GameHashes.LaunchConditionChanged, this.launch, new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.RocketReadyForLaunch)).Target(this.masterTarget).Exit(delegate(RocketControlStation.StatesInstance smi)
			{
				this.timeRemaining.Set(120f, smi, false);
			});
			this.launch.Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 1f, smi.pilotSpeedMult);
			}).ToggleChore(new Func<RocketControlStation.StatesInstance, Chore>(this.CreateLaunchChore), this.operational).Transition(this.launch.fadein, new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.IsInFlight), UpdateRate.SIM_200ms).Target(this.clusterCraft).EventTransition(GameHashes.RocketRequestLaunch, this.operational, GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Not(new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.RocketReadyForLaunch))).EventTransition(GameHashes.LaunchConditionChanged, this.operational, GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Not(new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.RocketReadyForLaunch))).Target(this.masterTarget);
			this.launch.fadein.Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				if (CameraController.Instance.cameraActiveCluster == this.clusterCraft.Get(smi).GetComponent<WorldContainer>().id)
				{
					CameraController.Instance.FadeIn(0f, 1f, null);
				}
			});
			this.running.PlayAnim("on").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.operational, GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Not(new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.IsInFlight)), UpdateRate.SIM_200ms).ParamTransition<float>(this.timeRemaining, this.ready, (RocketControlStation.StatesInstance smi, float p) => p <= 0f).Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 1f, smi.pilotSpeedMult);
			}).Update("Decrement time", new Action<RocketControlStation.StatesInstance, float>(this.DecrementTime), UpdateRate.SIM_200ms, false).Exit(delegate(RocketControlStation.StatesInstance smi)
			{
				this.timeRemaining.Set(30f, smi, false);
			});
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<RocketControlStation.StatesInstance, Chore>(this.CreateChore), this.ready.post, this.ready).Transition(this.operational, GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Not(new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.IsInFlight)), UpdateRate.SIM_200ms).OnSignal(this.pilotSuccessful, this.ready.post).Update("Decrement time", new Action<RocketControlStation.StatesInstance, float>(this.DecrementTime), UpdateRate.SIM_200ms, false);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((RocketControlStation.StatesInstance smi) => smi.master.GetComponent<RocketControlStationIdleWorkable>(), this.ready.working).ParamTransition<float>(this.timeRemaining, this.ready.warning, (RocketControlStation.StatesInstance smi, float p) => p <= 15f);
			this.ready.warning.PlayAnim("on_alert", KAnim.PlayMode.Loop).WorkableStartTransition((RocketControlStation.StatesInstance smi) => smi.master.GetComponent<RocketControlStationIdleWorkable>(), this.ready.working).ToggleMainStatusItem(Db.Get().BuildingStatusItems.PilotNeeded, null).ParamTransition<float>(this.timeRemaining, this.ready.autopilot, (RocketControlStation.StatesInstance smi, float p) => p <= 0f);
			this.ready.autopilot.PlayAnim("on_failed", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().BuildingStatusItems.AutoPilotActive, null).WorkableStartTransition((RocketControlStation.StatesInstance smi) => smi.master.GetComponent<RocketControlStationIdleWorkable>(), this.ready.working).Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 0.5f, smi.pilotSpeedMult);
			});
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 1f, smi.pilotSpeedMult);
			}).WorkableStopTransition((RocketControlStation.StatesInstance smi) => smi.master.GetComponent<RocketControlStationIdleWorkable>(), this.ready.idle);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.running).Exit(delegate(RocketControlStation.StatesInstance smi)
			{
				this.timeRemaining.Set(120f, smi, false);
			});
		}

		// Token: 0x06009048 RID: 36936 RVA: 0x0034B234 File Offset: 0x00349434
		public void AquireClustercraft(RocketControlStation.StatesInstance smi, bool force = false)
		{
			if (force || this.clusterCraft.IsNull(smi))
			{
				GameObject rocket = this.GetRocket(smi);
				this.clusterCraft.Set(rocket, smi, false);
				if (rocket != null)
				{
					rocket.Subscribe(-1582839653, new Action<object>(smi.master.OnTagsChanged));
				}
			}
		}

		// Token: 0x06009049 RID: 36937 RVA: 0x0034B28F File Offset: 0x0034948F
		private void DecrementTime(RocketControlStation.StatesInstance smi, float dt)
		{
			this.timeRemaining.Delta(-dt, smi);
		}

		// Token: 0x0600904A RID: 36938 RVA: 0x0034B2A0 File Offset: 0x003494A0
		private bool RocketReadyForLaunch(RocketControlStation.StatesInstance smi)
		{
			Clustercraft component = this.clusterCraft.Get(smi).GetComponent<Clustercraft>();
			return component.LaunchRequested && component.CheckReadyToLaunch();
		}

		// Token: 0x0600904B RID: 36939 RVA: 0x0034B2D0 File Offset: 0x003494D0
		private GameObject GetRocket(RocketControlStation.StatesInstance smi)
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(smi.GetMyWorldId());
			if (world == null)
			{
				return null;
			}
			return world.gameObject.GetComponent<Clustercraft>().gameObject;
		}

		// Token: 0x0600904C RID: 36940 RVA: 0x0034B309 File Offset: 0x00349509
		private void SetRocketSpeedModifiers(RocketControlStation.StatesInstance smi, float autoPilotSpeedMultiplier, float pilotSkillMultiplier = 1f)
		{
			this.clusterCraft.Get(smi).GetComponent<Clustercraft>().AutoPilotMultiplier = autoPilotSpeedMultiplier;
			this.clusterCraft.Get(smi).GetComponent<Clustercraft>().PilotSkillMultiplier = pilotSkillMultiplier;
		}

		// Token: 0x0600904D RID: 36941 RVA: 0x0034B33C File Offset: 0x0034953C
		private Chore CreateChore(RocketControlStation.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<RocketControlStationIdleWorkable>();
			WorkChore<RocketControlStationIdleWorkable> workChore = new WorkChore<RocketControlStationIdleWorkable>(Db.Get().ChoreTypes.RocketControl, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Work, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanUseRocketControlStation);
			workChore.AddPrecondition(ChorePreconditions.instance.IsRocketTravelling, null);
			return workChore;
		}

		// Token: 0x0600904E RID: 36942 RVA: 0x0034B3BC File Offset: 0x003495BC
		private Chore CreateLaunchChore(RocketControlStation.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<RocketControlStationLaunchWorkable>();
			WorkChore<RocketControlStationLaunchWorkable> workChore = new WorkChore<RocketControlStationLaunchWorkable>(Db.Get().ChoreTypes.RocketControl, component, null, true, null, null, null, true, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.topPriority, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanUseRocketControlStation);
			return workChore;
		}

		// Token: 0x0600904F RID: 36943 RVA: 0x0034B41A File Offset: 0x0034961A
		public void LaunchRocket(RocketControlStation.StatesInstance smi)
		{
			this.clusterCraft.Get(smi).GetComponent<Clustercraft>().Launch(false);
		}

		// Token: 0x06009050 RID: 36944 RVA: 0x0034B433 File Offset: 0x00349633
		public bool IsInFlight(RocketControlStation.StatesInstance smi)
		{
			return this.clusterCraft.Get(smi).GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.InFlight;
		}

		// Token: 0x06009051 RID: 36945 RVA: 0x0034B44E File Offset: 0x0034964E
		public bool IsLaunching(RocketControlStation.StatesInstance smi)
		{
			return this.clusterCraft.Get(smi).GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Launching;
		}

		// Token: 0x04006E24 RID: 28196
		public StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.TargetParameter clusterCraft;

		// Token: 0x04006E25 RID: 28197
		private GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State unoperational;

		// Token: 0x04006E26 RID: 28198
		private GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State operational;

		// Token: 0x04006E27 RID: 28199
		private GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State running;

		// Token: 0x04006E28 RID: 28200
		private RocketControlStation.States.ReadyStates ready;

		// Token: 0x04006E29 RID: 28201
		private RocketControlStation.States.LaunchStates launch;

		// Token: 0x04006E2A RID: 28202
		public StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Signal pilotSuccessful;

		// Token: 0x04006E2B RID: 28203
		public StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.FloatParameter timeRemaining;

		// Token: 0x02002529 RID: 9513
		public class ReadyStates : GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State
		{
			// Token: 0x0400A580 RID: 42368
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State idle;

			// Token: 0x0400A581 RID: 42369
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State working;

			// Token: 0x0400A582 RID: 42370
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State post;

			// Token: 0x0400A583 RID: 42371
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State warning;

			// Token: 0x0400A584 RID: 42372
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State autopilot;
		}

		// Token: 0x0200252A RID: 9514
		public class LaunchStates : GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State
		{
			// Token: 0x0400A585 RID: 42373
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State launch;

			// Token: 0x0400A586 RID: 42374
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State fadein;
		}
	}

	// Token: 0x020015E9 RID: 5609
	public class StatesInstance : GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.GameInstance
	{
		// Token: 0x0600905E RID: 36958 RVA: 0x0034B56A File Offset: 0x0034976A
		public StatesInstance(RocketControlStation smi) : base(smi)
		{
		}

		// Token: 0x0600905F RID: 36959 RVA: 0x0034B57E File Offset: 0x0034977E
		public void LaunchRocket()
		{
			base.sm.LaunchRocket(this);
		}

		// Token: 0x06009060 RID: 36960 RVA: 0x0034B58C File Offset: 0x0034978C
		public void SetPilotSpeedMult(WorkerBase pilot)
		{
			AttributeConverter pilotingSpeed = Db.Get().AttributeConverters.PilotingSpeed;
			AttributeConverterInstance converter = pilot.GetComponent<AttributeConverters>().GetConverter(pilotingSpeed.Id);
			float a = 1f + converter.Evaluate();
			this.pilotSpeedMult = Mathf.Max(a, 0.1f);
		}

		// Token: 0x04006E2C RID: 28204
		public float pilotSpeedMult = 1f;
	}
}
