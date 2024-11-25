using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020002FE RID: 766
public class MorbRoverMaker : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>
{
	// Token: 0x06001016 RID: 4118 RVA: 0x0005B038 File Offset: 0x00059238
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.no_operational;
		this.root.Update(new Action<MorbRoverMaker.Instance, float>(MorbRoverMaker.GermsRequiredFeedbackUpdate), UpdateRate.SIM_1000ms, false);
		this.no_operational.Enter(delegate(MorbRoverMaker.Instance smi)
		{
			MorbRoverMaker.DisableManualDelivery(smi, "Disable manual delivery while no operational. in case players disabled the machine on purpose for this reason");
		}).TagTransition(GameTags.Operational, this.operational, false);
		this.operational.TagTransition(GameTags.Operational, this.no_operational, true).DefaultState(this.operational.covered);
		this.operational.covered.ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerDusty, null).ParamTransition<bool>(this.WasUncoverByDuplicant, this.operational.idle, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsTrue).Enter(delegate(MorbRoverMaker.Instance smi)
		{
			MorbRoverMaker.DisableManualDelivery(smi, "Machine can't ask for materials if it has not been investigated by a dupe");
		}).DefaultState(this.operational.covered.idle);
		this.operational.covered.idle.PlayAnim("dusty").ParamTransition<bool>(this.UncoverOrderRequested, this.operational.covered.careOrderGiven, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsTrue);
		this.operational.covered.careOrderGiven.PlayAnim("dusty").Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.StartWorkChore_RevealMachine)).Exit(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.CancelWorkChore_RevealMachine)).WorkableCompleteTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_RevealMachine(), this.operational.covered.complete).ParamTransition<bool>(this.UncoverOrderRequested, this.operational.covered.idle, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsFalse);
		this.operational.covered.complete.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.SetUncovered));
		this.operational.idle.Enter(delegate(MorbRoverMaker.Instance smi)
		{
			MorbRoverMaker.EnableManualDelivery(smi, "Operational and discovered");
		}).EnterTransition(this.operational.crafting, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting)).EnterTransition(this.operational.waitingForMorb, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.IsCraftingCompleted)).EventTransition(GameHashes.OnStorageChange, this.operational.crafting, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting)).PlayAnim("idle").ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null);
		this.operational.crafting.DefaultState(this.operational.crafting.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerCraftingBody, null);
		this.operational.crafting.conflict.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.ResetRoverBodyCraftingProgress)).GoTo(this.operational.idle);
		this.operational.crafting.pre.EventTransition(GameHashes.OnStorageChange, this.operational.crafting.conflict, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Not(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting))).PlayAnim("crafting_pre").OnAnimQueueComplete(this.operational.crafting.loop);
		this.operational.crafting.loop.EventTransition(GameHashes.OnStorageChange, this.operational.crafting.conflict, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Not(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting))).Update(new Action<MorbRoverMaker.Instance, float>(MorbRoverMaker.CraftingUpdate), UpdateRate.SIM_200ms, false).PlayAnim("crafting_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.CraftProgress, this.operational.crafting.pst, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsOne);
		this.operational.crafting.pst.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.ConsumeRoverBodyCraftingMaterials)).PlayAnim("crafting_pst").OnAnimQueueComplete(this.operational.waitingForMorb);
		this.operational.waitingForMorb.PlayAnim("crafting_complete").ParamTransition<long>(this.Germs, this.operational.doctor, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Parameter<long>.Callback(MorbRoverMaker.HasEnoughGerms)).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null);
		this.operational.doctor.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.StartWorkChore_ReleaseRover)).Exit(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.CancelWorkChore_ReleaseRover)).WorkableCompleteTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_ReleaseRover(), this.operational.finish).DefaultState(this.operational.doctor.needed);
		this.operational.doctor.needed.PlayAnim("waiting", KAnim.PlayMode.Loop).WorkableStartTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_ReleaseRover(), this.operational.doctor.working).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerReadyForDoctor, null);
		this.operational.doctor.working.WorkableStopTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_ReleaseRover(), this.operational.doctor.needed);
		this.operational.finish.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.SpawnRover)).GoTo(this.operational.idle);
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x0005B601 File Offset: 0x00059801
	public static bool ShouldBeCrafting(MorbRoverMaker.Instance smi)
	{
		return smi.HasMaterialsForRover && smi.RoverDevelopment_Progress < 1f;
	}

	// Token: 0x06001018 RID: 4120 RVA: 0x0005B61A File Offset: 0x0005981A
	public static bool IsCraftingCompleted(MorbRoverMaker.Instance smi)
	{
		return smi.RoverDevelopment_Progress == 1f;
	}

	// Token: 0x06001019 RID: 4121 RVA: 0x0005B629 File Offset: 0x00059829
	public static bool HasEnoughGerms(MorbRoverMaker.Instance smi, long germCount)
	{
		return germCount >= smi.def.GERMS_PER_ROVER;
	}

	// Token: 0x0600101A RID: 4122 RVA: 0x0005B63C File Offset: 0x0005983C
	public static void StartWorkChore_ReleaseRover(MorbRoverMaker.Instance smi)
	{
		smi.CreateWorkChore_ReleaseRover();
	}

	// Token: 0x0600101B RID: 4123 RVA: 0x0005B644 File Offset: 0x00059844
	public static void CancelWorkChore_ReleaseRover(MorbRoverMaker.Instance smi)
	{
		smi.CancelWorkChore_ReleaseRover();
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x0005B64C File Offset: 0x0005984C
	public static void StartWorkChore_RevealMachine(MorbRoverMaker.Instance smi)
	{
		smi.CreateWorkChore_RevealMachine();
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x0005B654 File Offset: 0x00059854
	public static void CancelWorkChore_RevealMachine(MorbRoverMaker.Instance smi)
	{
		smi.CancelWorkChore_RevealMachine();
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x0005B65C File Offset: 0x0005985C
	public static void SetUncovered(MorbRoverMaker.Instance smi)
	{
		smi.Uncover();
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x0005B664 File Offset: 0x00059864
	public static void SpawnRover(MorbRoverMaker.Instance smi)
	{
		smi.SpawnRover();
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x0005B66C File Offset: 0x0005986C
	public static void EnableManualDelivery(MorbRoverMaker.Instance smi, string reason)
	{
		smi.EnableManualDelivery(reason);
	}

	// Token: 0x06001021 RID: 4129 RVA: 0x0005B675 File Offset: 0x00059875
	public static void DisableManualDelivery(MorbRoverMaker.Instance smi, string reason)
	{
		smi.DisableManualDelivery(reason);
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x0005B67E File Offset: 0x0005987E
	public static void ConsumeRoverBodyCraftingMaterials(MorbRoverMaker.Instance smi)
	{
		smi.ConsumeRoverBodyCraftingMaterials();
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x0005B686 File Offset: 0x00059886
	public static void ResetRoverBodyCraftingProgress(MorbRoverMaker.Instance smi)
	{
		smi.SetRoverDevelopmentProgress(0f);
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x0005B694 File Offset: 0x00059894
	public static void CraftingUpdate(MorbRoverMaker.Instance smi, float dt)
	{
		float roverDevelopmentProgress = Mathf.Clamp((smi.RoverDevelopment_Progress * smi.def.ROVER_CRAFTING_DURATION + dt) / smi.def.ROVER_CRAFTING_DURATION, 0f, 1f);
		smi.SetRoverDevelopmentProgress(roverDevelopmentProgress);
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x0005B6D8 File Offset: 0x000598D8
	public static void GermsRequiredFeedbackUpdate(MorbRoverMaker.Instance smi, float dt)
	{
		if (GameClock.Instance.GetTime() - smi.lastTimeGermsAdded > smi.def.FEEDBACK_NO_GERMS_DETECTED_TIMEOUT & smi.MorbDevelopment_Progress < 1f & !smi.IsInsideState(smi.sm.operational.doctor) & smi.HasBeenRevealed)
		{
			smi.ShowGermRequiredStatusItemAlert();
			return;
		}
		smi.HideGermRequiredStatusItemAlert();
	}

	// Token: 0x040009B4 RID: 2484
	private const string ROBOT_PROGRESS_METER_TARGET_NAME = "meter_robot_target";

	// Token: 0x040009B5 RID: 2485
	private const string ROBOT_PROGRESS_METER_ANIMATION_NAME = "meter_robot";

	// Token: 0x040009B6 RID: 2486
	private const string COVERED_IDLE_ANIM_NAME = "dusty";

	// Token: 0x040009B7 RID: 2487
	private const string IDLE_ANIM_NAME = "idle";

	// Token: 0x040009B8 RID: 2488
	private const string CRAFT_PRE_ANIM_NAME = "crafting_pre";

	// Token: 0x040009B9 RID: 2489
	private const string CRAFT_LOOP_ANIM_NAME = "crafting_loop";

	// Token: 0x040009BA RID: 2490
	private const string CRAFT_PST_ANIM_NAME = "crafting_pst";

	// Token: 0x040009BB RID: 2491
	private const string CRAFT_COMPLETED_ANIM_NAME = "crafting_complete";

	// Token: 0x040009BC RID: 2492
	private const string WAITING_FOR_DOCTOR_ANIM_NAME = "waiting";

	// Token: 0x040009BD RID: 2493
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.BoolParameter UncoverOrderRequested;

	// Token: 0x040009BE RID: 2494
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.BoolParameter WasUncoverByDuplicant;

	// Token: 0x040009BF RID: 2495
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.LongParameter Germs;

	// Token: 0x040009C0 RID: 2496
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.FloatParameter CraftProgress;

	// Token: 0x040009C1 RID: 2497
	public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State no_operational;

	// Token: 0x040009C2 RID: 2498
	public MorbRoverMaker.OperationalStates operational;

	// Token: 0x02001122 RID: 4386
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007E87 RID: 32391 RVA: 0x0030A6B8 File Offset: 0x003088B8
		public float GetConduitMaxPackageMass()
		{
			ConduitType germ_INTAKE_CONDUIT_TYPE = this.GERM_INTAKE_CONDUIT_TYPE;
			if (germ_INTAKE_CONDUIT_TYPE == ConduitType.Gas)
			{
				return 1f;
			}
			if (germ_INTAKE_CONDUIT_TYPE != ConduitType.Liquid)
			{
				return 1f;
			}
			return 10f;
		}

		// Token: 0x04005F32 RID: 24370
		public float FEEDBACK_NO_GERMS_DETECTED_TIMEOUT = 2f;

		// Token: 0x04005F33 RID: 24371
		public Tag ROVER_PREFAB_ID;

		// Token: 0x04005F34 RID: 24372
		public float INITIAL_MORB_DEVELOPMENT_PERCENTAGE;

		// Token: 0x04005F35 RID: 24373
		public float ROVER_CRAFTING_DURATION;

		// Token: 0x04005F36 RID: 24374
		public float METAL_PER_ROVER;

		// Token: 0x04005F37 RID: 24375
		public long GERMS_PER_ROVER;

		// Token: 0x04005F38 RID: 24376
		public int MAX_GERMS_TAKEN_PER_PACKAGE;

		// Token: 0x04005F39 RID: 24377
		public int GERM_TYPE;

		// Token: 0x04005F3A RID: 24378
		public SimHashes ROVER_MATERIAL;

		// Token: 0x04005F3B RID: 24379
		public ConduitType GERM_INTAKE_CONDUIT_TYPE;
	}

	// Token: 0x02001123 RID: 4387
	public class CoverStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04005F3C RID: 24380
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State idle;

		// Token: 0x04005F3D RID: 24381
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State careOrderGiven;

		// Token: 0x04005F3E RID: 24382
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State complete;
	}

	// Token: 0x02001124 RID: 4388
	public class OperationalStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04005F3F RID: 24383
		public MorbRoverMaker.CoverStates covered;

		// Token: 0x04005F40 RID: 24384
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State idle;

		// Token: 0x04005F41 RID: 24385
		public MorbRoverMaker.CraftingStates crafting;

		// Token: 0x04005F42 RID: 24386
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State waitingForMorb;

		// Token: 0x04005F43 RID: 24387
		public MorbRoverMaker.DoctorStates doctor;

		// Token: 0x04005F44 RID: 24388
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State finish;
	}

	// Token: 0x02001125 RID: 4389
	public class DoctorStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04005F45 RID: 24389
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State needed;

		// Token: 0x04005F46 RID: 24390
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State working;
	}

	// Token: 0x02001126 RID: 4390
	public class CraftingStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04005F47 RID: 24391
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State conflict;

		// Token: 0x04005F48 RID: 24392
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State pre;

		// Token: 0x04005F49 RID: 24393
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State loop;

		// Token: 0x04005F4A RID: 24394
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State pst;
	}

	// Token: 0x02001127 RID: 4391
	public new class Instance : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06007E8D RID: 32397 RVA: 0x0030A71A File Offset: 0x0030891A
		public long MorbDevelopment_GermsCollected
		{
			get
			{
				return base.sm.Germs.Get(base.smi);
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06007E8E RID: 32398 RVA: 0x0030A732 File Offset: 0x00308932
		public long MorbDevelopment_RemainingGerms
		{
			get
			{
				return base.def.GERMS_PER_ROVER - this.MorbDevelopment_GermsCollected;
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06007E8F RID: 32399 RVA: 0x0030A746 File Offset: 0x00308946
		public float MorbDevelopment_Progress
		{
			get
			{
				return Mathf.Clamp((float)this.MorbDevelopment_GermsCollected / (float)base.def.GERMS_PER_ROVER, 0f, 1f);
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06007E90 RID: 32400 RVA: 0x0030A76B File Offset: 0x0030896B
		public bool HasMaterialsForRover
		{
			get
			{
				return this.storage.GetMassAvailable(base.def.ROVER_MATERIAL) >= base.def.METAL_PER_ROVER;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06007E91 RID: 32401 RVA: 0x0030A793 File Offset: 0x00308993
		public float RoverDevelopment_Progress
		{
			get
			{
				return base.sm.CraftProgress.Get(base.smi);
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06007E92 RID: 32402 RVA: 0x0030A7AB File Offset: 0x003089AB
		public bool HasBeenRevealed
		{
			get
			{
				return base.sm.WasUncoverByDuplicant.Get(base.smi);
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06007E93 RID: 32403 RVA: 0x0030A7C3 File Offset: 0x003089C3
		public bool CanPumpGerms
		{
			get
			{
				return this.operational && this.MorbDevelopment_Progress < 1f && this.HasBeenRevealed;
			}
		}

		// Token: 0x06007E94 RID: 32404 RVA: 0x0030A7E7 File Offset: 0x003089E7
		public Workable GetWorkable_RevealMachine()
		{
			return this.workable_reveal;
		}

		// Token: 0x06007E95 RID: 32405 RVA: 0x0030A7EF File Offset: 0x003089EF
		public Workable GetWorkable_ReleaseRover()
		{
			return this.workable_release;
		}

		// Token: 0x06007E96 RID: 32406 RVA: 0x0030A7F8 File Offset: 0x003089F8
		public void ShowGermRequiredStatusItemAlert()
		{
			if (this.germsRequiredAlertStatusItemHandle == default(Guid))
			{
				this.germsRequiredAlertStatusItemHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerNoGermsConsumedAlert, base.smi);
			}
		}

		// Token: 0x06007E97 RID: 32407 RVA: 0x0030A844 File Offset: 0x00308A44
		public void HideGermRequiredStatusItemAlert()
		{
			if (this.germsRequiredAlertStatusItemHandle != default(Guid))
			{
				this.selectable.RemoveStatusItem(this.germsRequiredAlertStatusItemHandle, false);
				this.germsRequiredAlertStatusItemHandle = default(Guid);
			}
		}

		// Token: 0x06007E98 RID: 32408 RVA: 0x0030A888 File Offset: 0x00308A88
		public Instance(IStateMachineTarget master, MorbRoverMaker.Def def) : base(master, def)
		{
			this.RobotProgressMeter = new MeterController(this.buildingAnimCtr, "meter_robot_target", "meter_robot", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
		}

		// Token: 0x06007E99 RID: 32409 RVA: 0x0030A8F0 File Offset: 0x00308AF0
		public override void StartSM()
		{
			Building component = base.GetComponent<Building>();
			this.inputCell = component.GetUtilityInputCell();
			this.outputCell = component.GetUtilityOutputCell();
			base.StartSM();
			if (!this.HasBeenRevealed)
			{
				base.sm.Germs.Set(0L, base.smi, false);
				this.AddGerms((long)((float)base.def.GERMS_PER_ROVER * base.def.INITIAL_MORB_DEVELOPMENT_PERCENTAGE), false);
			}
			Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE).AddConduitUpdater(new Action<float>(this.Flow), ConduitFlowPriority.Default);
			this.UpdateMeters();
		}

		// Token: 0x06007E9A RID: 32410 RVA: 0x0030A98C File Offset: 0x00308B8C
		public void AddGerms(long amount, bool playAnimations = true)
		{
			long value = this.MorbDevelopment_GermsCollected + amount;
			base.sm.Germs.Set(value, base.smi, false);
			this.UpdateMeters();
			if (amount > 0L)
			{
				if (playAnimations)
				{
					this.capsule.PlayPumpGermsAnimation();
				}
				Action<long> germsAdded = this.GermsAdded;
				if (germsAdded != null)
				{
					germsAdded(amount);
				}
				this.lastTimeGermsAdded = GameClock.Instance.GetTime();
			}
		}

		// Token: 0x06007E9B RID: 32411 RVA: 0x0030A9F8 File Offset: 0x00308BF8
		public long RemoveGerms(long amount)
		{
			long num = amount.Min(this.MorbDevelopment_GermsCollected);
			long value = this.MorbDevelopment_GermsCollected - num;
			base.sm.Germs.Set(value, base.smi, false);
			this.UpdateMeters();
			return num;
		}

		// Token: 0x06007E9C RID: 32412 RVA: 0x0030AA3B File Offset: 0x00308C3B
		public void EnableManualDelivery(string reason)
		{
			this.manualDelivery.Pause(false, reason);
		}

		// Token: 0x06007E9D RID: 32413 RVA: 0x0030AA4A File Offset: 0x00308C4A
		public void DisableManualDelivery(string reason)
		{
			this.manualDelivery.Pause(true, reason);
		}

		// Token: 0x06007E9E RID: 32414 RVA: 0x0030AA59 File Offset: 0x00308C59
		public void SetRoverDevelopmentProgress(float value)
		{
			base.sm.CraftProgress.Set(value, base.smi, false);
			this.UpdateMeters();
		}

		// Token: 0x06007E9F RID: 32415 RVA: 0x0030AA7C File Offset: 0x00308C7C
		public void UpdateMeters()
		{
			this.RobotProgressMeter.SetPositionPercent(this.RoverDevelopment_Progress);
			this.capsule.SetMorbDevelopmentProgress(this.MorbDevelopment_Progress);
			this.capsule.SetGermMeterProgress(this.HasBeenRevealed ? this.MorbDevelopment_Progress : 0f);
		}

		// Token: 0x06007EA0 RID: 32416 RVA: 0x0030AACB File Offset: 0x00308CCB
		public void Uncover()
		{
			base.sm.WasUncoverByDuplicant.Set(true, base.smi, false);
			System.Action onUncovered = this.OnUncovered;
			if (onUncovered == null)
			{
				return;
			}
			onUncovered();
		}

		// Token: 0x06007EA1 RID: 32417 RVA: 0x0030AAF8 File Offset: 0x00308CF8
		public void CreateWorkChore_ReleaseRover()
		{
			if (this.workChore_releaseRover == null)
			{
				this.workChore_releaseRover = new WorkChore<MorbRoverMakerWorkable>(Db.Get().ChoreTypes.Doctor, this.workable_release, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06007EA2 RID: 32418 RVA: 0x0030AB3E File Offset: 0x00308D3E
		public void CancelWorkChore_ReleaseRover()
		{
			if (this.workChore_releaseRover != null)
			{
				this.workChore_releaseRover.Cancel("MorbRoverMaker.CancelWorkChore_ReleaseRover");
				this.workChore_releaseRover = null;
			}
		}

		// Token: 0x06007EA3 RID: 32419 RVA: 0x0030AB60 File Offset: 0x00308D60
		public void CreateWorkChore_RevealMachine()
		{
			if (this.workChore_revealMachine == null)
			{
				this.workChore_revealMachine = new WorkChore<MorbRoverMakerRevealWorkable>(Db.Get().ChoreTypes.Repair, this.workable_reveal, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06007EA4 RID: 32420 RVA: 0x0030ABA6 File Offset: 0x00308DA6
		public void CancelWorkChore_RevealMachine()
		{
			if (this.workChore_revealMachine != null)
			{
				this.workChore_revealMachine.Cancel("MorbRoverMaker.CancelWorkChore_RevealMachine");
				this.workChore_revealMachine = null;
			}
		}

		// Token: 0x06007EA5 RID: 32421 RVA: 0x0030ABC8 File Offset: 0x00308DC8
		public void ConsumeRoverBodyCraftingMaterials()
		{
			float num = 0f;
			this.storage.ConsumeAndGetDisease(base.def.ROVER_MATERIAL.CreateTag(), base.def.METAL_PER_ROVER, out num, out this.lastastMaterialsConsumedDiseases, out this.lastastMaterialsConsumedTemp);
		}

		// Token: 0x06007EA6 RID: 32422 RVA: 0x0030AC10 File Offset: 0x00308E10
		public void SpawnRover()
		{
			if (this.RoverDevelopment_Progress == 1f)
			{
				this.RemoveGerms(base.def.GERMS_PER_ROVER);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(base.def.ROVER_PREFAB_ID), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0);
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (this.lastastMaterialsConsumedDiseases.idx != 255)
				{
					component.AddDisease(this.lastastMaterialsConsumedDiseases.idx, this.lastastMaterialsConsumedDiseases.count, "From the materials provided for its creation");
				}
				if (this.lastastMaterialsConsumedTemp > 0f)
				{
					component.SetMassTemperature(component.Mass, this.lastastMaterialsConsumedTemp);
				}
				gameObject.SetActive(true);
				this.SetRoverDevelopmentProgress(0f);
				Action<GameObject> onRoverSpawned = this.OnRoverSpawned;
				if (onRoverSpawned == null)
				{
					return;
				}
				onRoverSpawned(gameObject);
			}
		}

		// Token: 0x06007EA7 RID: 32423 RVA: 0x0030ACE8 File Offset: 0x00308EE8
		private void Flow(float dt)
		{
			if (this.CanPumpGerms)
			{
				ConduitFlow flowManager = Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE);
				int num = 0;
				if (flowManager.HasConduit(this.inputCell) && flowManager.HasConduit(this.outputCell))
				{
					ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
					ConduitFlow.ConduitContents contents2 = flowManager.GetContents(this.outputCell);
					float num2 = Mathf.Min(contents.mass, base.def.GetConduitMaxPackageMass() * dt);
					if (flowManager.CanMergeContents(contents, contents2, num2))
					{
						float amountAllowedForMerging = flowManager.GetAmountAllowedForMerging(contents, contents2, num2);
						if (amountAllowedForMerging > 0f)
						{
							ConduitFlow conduitFlow = (base.def.GERM_INTAKE_CONDUIT_TYPE == ConduitType.Liquid) ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow;
							int num3 = contents.diseaseCount;
							if (contents.diseaseIdx != 255 && (int)contents.diseaseIdx == base.def.GERM_TYPE)
							{
								num = (int)this.MorbDevelopment_RemainingGerms.Min((long)base.def.MAX_GERMS_TAKEN_PER_PACKAGE).Min((long)contents.diseaseCount);
								num3 -= num;
							}
							float num4 = conduitFlow.AddElement(this.outputCell, contents.element, amountAllowedForMerging, contents.temperature, contents.diseaseIdx, num3);
							if (amountAllowedForMerging != num4)
							{
								global::Debug.Log("[Morb Rover Maker] Mass Differs By: " + (amountAllowedForMerging - num4).ToString());
							}
							flowManager.RemoveElement(this.inputCell, num4);
						}
					}
				}
				if (num > 0)
				{
					this.AddGerms((long)num, true);
				}
			}
		}

		// Token: 0x06007EA8 RID: 32424 RVA: 0x0030AE6A File Offset: 0x0030906A
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE).RemoveConduitUpdater(new Action<float>(this.Flow));
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06007EA9 RID: 32425 RVA: 0x0030AE93 File Offset: 0x00309093
		public string SidescreenButtonText
		{
			get
			{
				return this.HasBeenRevealed ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY : (base.sm.UncoverOrderRequested.Get(base.smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN);
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06007EAA RID: 32426 RVA: 0x0030AECD File Offset: 0x003090CD
		public string SidescreenButtonTooltip
		{
			get
			{
				return this.HasBeenRevealed ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY_TOOLTIP : (base.sm.UncoverOrderRequested.Get(base.smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN_TOOLTIP : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN_TOOLTIP);
			}
		}

		// Token: 0x06007EAB RID: 32427 RVA: 0x0030AF07 File Offset: 0x00309107
		public bool SidescreenEnabled()
		{
			return true;
		}

		// Token: 0x06007EAC RID: 32428 RVA: 0x0030AF0A File Offset: 0x0030910A
		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		// Token: 0x06007EAD RID: 32429 RVA: 0x0030AF0D File Offset: 0x0030910D
		public int HorizontalGroupID()
		{
			return 0;
		}

		// Token: 0x06007EAE RID: 32430 RVA: 0x0030AF10 File Offset: 0x00309110
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06007EAF RID: 32431 RVA: 0x0030AF14 File Offset: 0x00309114
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007EB0 RID: 32432 RVA: 0x0030AF1C File Offset: 0x0030911C
		public void OnSidescreenButtonPressed()
		{
			if (this.HasBeenRevealed)
			{
				this.storage.DropAll(false, false, default(Vector3), true, null);
				return;
			}
			bool flag = base.smi.sm.UncoverOrderRequested.Get(base.smi);
			base.smi.sm.UncoverOrderRequested.Set(!flag, base.smi, false);
		}

		// Token: 0x04005F4B RID: 24395
		public Action<long> GermsAdded;

		// Token: 0x04005F4C RID: 24396
		public System.Action OnUncovered;

		// Token: 0x04005F4D RID: 24397
		public Action<GameObject> OnRoverSpawned;

		// Token: 0x04005F4E RID: 24398
		[MyCmpGet]
		private MorbRoverMakerRevealWorkable workable_reveal;

		// Token: 0x04005F4F RID: 24399
		[MyCmpGet]
		private MorbRoverMakerWorkable workable_release;

		// Token: 0x04005F50 RID: 24400
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04005F51 RID: 24401
		[MyCmpGet]
		private KBatchedAnimController buildingAnimCtr;

		// Token: 0x04005F52 RID: 24402
		[MyCmpGet]
		private ManualDeliveryKG manualDelivery;

		// Token: 0x04005F53 RID: 24403
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04005F54 RID: 24404
		[MyCmpGet]
		private MorbRoverMaker_Capsule capsule;

		// Token: 0x04005F55 RID: 24405
		[MyCmpGet]
		private KSelectable selectable;

		// Token: 0x04005F56 RID: 24406
		private MeterController RobotProgressMeter;

		// Token: 0x04005F57 RID: 24407
		private int inputCell = -1;

		// Token: 0x04005F58 RID: 24408
		private int outputCell = -1;

		// Token: 0x04005F59 RID: 24409
		private Chore workChore_revealMachine;

		// Token: 0x04005F5A RID: 24410
		private Chore workChore_releaseRover;

		// Token: 0x04005F5B RID: 24411
		[Serialize]
		private float lastastMaterialsConsumedTemp = -1f;

		// Token: 0x04005F5C RID: 24412
		[Serialize]
		private SimUtil.DiseaseInfo lastastMaterialsConsumedDiseases = SimUtil.DiseaseInfo.Invalid;

		// Token: 0x04005F5D RID: 24413
		public float lastTimeGermsAdded = -1f;

		// Token: 0x04005F5E RID: 24414
		private Guid germsRequiredAlertStatusItemHandle;
	}
}
