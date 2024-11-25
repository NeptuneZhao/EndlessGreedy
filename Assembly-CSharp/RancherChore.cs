using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x0200044B RID: 1099
public class RancherChore : Chore<RancherChore.RancherChoreStates.Instance>
{
	// Token: 0x06001747 RID: 5959 RVA: 0x0007DFFC File Offset: 0x0007C1FC
	public RancherChore(KPrefabID rancher_station)
	{
		Chore.Precondition isOpenForRanching = default(Chore.Precondition);
		isOpenForRanching.id = "IsCreatureAvailableForRanching";
		isOpenForRanching.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_RANCHING;
		isOpenForRanching.sortOrder = -3;
		isOpenForRanching.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			RanchStation.Instance instance = data as RanchStation.Instance;
			return !instance.HasRancher && instance.IsCritterAvailableForRanching;
		};
		this.IsOpenForRanching = isOpenForRanching;
		base..ctor(Db.Get().ChoreTypes.Ranch, rancher_station, null, false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime);
		this.AddPrecondition(this.IsOpenForRanching, rancher_station.GetSMI<RanchStation.Instance>());
		SkillPerkMissingComplainer component = base.GetComponent<SkillPerkMissingComplainer>();
		this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, component.requiredSkillPerk);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, rancher_station.GetComponent<Building>());
		Operational component2 = rancher_station.GetComponent<Operational>();
		this.AddPrecondition(ChorePreconditions.instance.IsOperational, component2);
		Deconstructable component3 = rancher_station.GetComponent<Deconstructable>();
		this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component3);
		BuildingEnabledButton component4 = rancher_station.GetComponent<BuildingEnabledButton>();
		this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component4);
		base.smi = new RancherChore.RancherChoreStates.Instance(rancher_station);
		base.SetPrioritizable(rancher_station.GetComponent<Prioritizable>());
	}

	// Token: 0x06001748 RID: 5960 RVA: 0x0007E14D File Offset: 0x0007C34D
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.rancher.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x06001749 RID: 5961 RVA: 0x0007E17E File Offset: 0x0007C37E
	protected override void End(string reason)
	{
		base.End(reason);
		base.smi.sm.rancher.Set(null, base.smi);
	}

	// Token: 0x04000D12 RID: 3346
	public Chore.Precondition IsOpenForRanching;

	// Token: 0x020011E0 RID: 4576
	public class RancherChoreStates : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance>
	{
		// Token: 0x0600815D RID: 33117 RVA: 0x00316EE0 File Offset: 0x003150E0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.moveToRanch;
			base.Target(this.rancher);
			this.root.Exit("TriggerRanchStationNoLongerAvailable", delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				smi.ranchStation.TriggerRanchStationNoLongerAvailable();
			});
			this.moveToRanch.MoveTo((RancherChore.RancherChoreStates.Instance smi) => Grid.PosToCell(smi.transform.GetPosition()), this.waitForAvailableRanchable, null, false);
			this.waitForAvailableRanchable.Enter("FindRanchable", delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				smi.WaitForAvailableRanchable(0f);
			}).Update("FindRanchable", delegate(RancherChore.RancherChoreStates.Instance smi, float dt)
			{
				smi.WaitForAvailableRanchable(dt);
			}, UpdateRate.SIM_200ms, false);
			this.ranchCritter.ScheduleGoTo(0.5f, this.ranchCritter.callForCritter).EventTransition(GameHashes.CreatureAbandonedRanchStation, this.waitForAvailableRanchable, null);
			this.ranchCritter.callForCritter.ToggleAnims("anim_interacts_rancherstation_kanim", 0f).PlayAnim("calling_loop", KAnim.PlayMode.Loop).ScheduleActionNextFrame("TellCreatureRancherIsReady", delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				smi.ranchStation.MessageRancherReady();
			}).Target(this.masterTarget).EventTransition(GameHashes.CreatureArrivedAtRanchStation, this.ranchCritter.working, null);
			this.ranchCritter.working.ToggleWork<RancherChore.RancherWorkable>(this.masterTarget, this.ranchCritter.pst, this.waitForAvailableRanchable, null);
			this.ranchCritter.pst.ToggleAnims(new Func<RancherChore.RancherChoreStates.Instance, HashedString>(RancherChore.RancherChoreStates.GetRancherInteractAnim)).QueueAnim("wipe_brow", false, null).OnAnimQueueComplete(this.waitForAvailableRanchable);
		}

		// Token: 0x0600815E RID: 33118 RVA: 0x003170B9 File Offset: 0x003152B9
		private static HashedString GetRancherInteractAnim(RancherChore.RancherChoreStates.Instance smi)
		{
			return smi.ranchStation.def.RancherInteractAnim;
		}

		// Token: 0x0600815F RID: 33119 RVA: 0x003170CC File Offset: 0x003152CC
		public static bool TryRanchCreature(RancherChore.RancherChoreStates.Instance smi)
		{
			Debug.Assert(smi.ranchStation != null, "smi.ranchStation was null");
			RanchedStates.Instance activeRanchable = smi.ranchStation.ActiveRanchable;
			if (activeRanchable.IsNullOrStopped())
			{
				return false;
			}
			KPrefabID component = activeRanchable.GetComponent<KPrefabID>();
			smi.sm.rancher.Get(smi).Trigger(937885943, component.PrefabTag.Name);
			smi.ranchStation.RanchCreature();
			return true;
		}

		// Token: 0x040061AB RID: 25003
		public StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher;

		// Token: 0x040061AC RID: 25004
		private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State moveToRanch;

		// Token: 0x040061AD RID: 25005
		private RancherChore.RancherChoreStates.RanchState ranchCritter;

		// Token: 0x040061AE RID: 25006
		private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State waitForAvailableRanchable;

		// Token: 0x020023D5 RID: 9173
		private class RanchState : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x04009FF6 RID: 40950
			public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State callForCritter;

			// Token: 0x04009FF7 RID: 40951
			public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State working;

			// Token: 0x04009FF8 RID: 40952
			public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State pst;
		}

		// Token: 0x020023D6 RID: 9174
		public new class Instance : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.GameInstance
		{
			// Token: 0x0600B7E6 RID: 47078 RVA: 0x003CE2CA File Offset: 0x003CC4CA
			public Instance(KPrefabID rancher_station) : base(rancher_station)
			{
				this.ranchStation = rancher_station.GetSMI<RanchStation.Instance>();
			}

			// Token: 0x0600B7E7 RID: 47079 RVA: 0x003CE2E0 File Offset: 0x003CC4E0
			public void WaitForAvailableRanchable(float dt)
			{
				this.waitTime += dt;
				GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state = this.ranchStation.IsCritterAvailableForRanching ? base.sm.ranchCritter : null;
				if (state != null || this.waitTime >= 2f)
				{
					this.waitTime = 0f;
					this.GoTo(state);
				}
			}

			// Token: 0x04009FF9 RID: 40953
			private const float WAIT_FOR_RANCHABLE_TIMEOUT = 2f;

			// Token: 0x04009FFA RID: 40954
			public RanchStation.Instance ranchStation;

			// Token: 0x04009FFB RID: 40955
			private float waitTime;
		}
	}

	// Token: 0x020011E1 RID: 4577
	public class RancherWorkable : Workable
	{
		// Token: 0x06008161 RID: 33121 RVA: 0x00317144 File Offset: 0x00315344
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.ranch = base.gameObject.GetSMI<RanchStation.Instance>();
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.ranch.def.RancherInteractAnim)
			};
			base.SetWorkTime(this.ranch.def.WorkTime);
			base.SetWorkerStatusItem(this.ranch.def.RanchingStatusItem);
			this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
			this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			this.lightEfficiencyBonus = false;
		}

		// Token: 0x06008162 RID: 33122 RVA: 0x003171EF File Offset: 0x003153EF
		public override Klei.AI.Attribute GetWorkAttribute()
		{
			return Db.Get().Attributes.Ranching;
		}

		// Token: 0x06008163 RID: 33123 RVA: 0x00317200 File Offset: 0x00315400
		protected override void OnStartWork(WorkerBase worker)
		{
			if (this.ranch == null)
			{
				return;
			}
			this.critterAnimController = this.ranch.ActiveRanchable.AnimController;
			this.critterAnimController.Play(this.ranch.def.RanchedPreAnim, KAnim.PlayMode.Once, 1f, 0f);
			this.critterAnimController.Queue(this.ranch.def.RanchedLoopAnim, KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x06008164 RID: 33124 RVA: 0x00317278 File Offset: 0x00315478
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			if (this.ranch.def.OnRanchWorkTick != null)
			{
				this.ranch.def.OnRanchWorkTick(this.ranch.ActiveRanchable.gameObject, dt, this);
			}
			return base.OnWorkTick(worker, dt);
		}

		// Token: 0x06008165 RID: 33125 RVA: 0x003172C8 File Offset: 0x003154C8
		public override void OnPendingCompleteWork(WorkerBase work)
		{
			RancherChore.RancherChoreStates.Instance smi = base.gameObject.GetSMI<RancherChore.RancherChoreStates.Instance>();
			if (this.ranch == null || smi == null)
			{
				return;
			}
			if (RancherChore.RancherChoreStates.TryRanchCreature(smi))
			{
				this.critterAnimController.Play(this.ranch.def.RanchedPstAnim, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06008166 RID: 33126 RVA: 0x0031731B File Offset: 0x0031551B
		protected override void OnAbortWork(WorkerBase worker)
		{
			if (this.ranch == null || this.critterAnimController == null)
			{
				return;
			}
			this.critterAnimController.Play(this.ranch.def.RanchedAbortAnim, KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x040061AF RID: 25007
		private RanchStation.Instance ranch;

		// Token: 0x040061B0 RID: 25008
		private KBatchedAnimController critterAnimController;
	}
}
