using System;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x02000A24 RID: 2596
public class RemoteWorkerSM : StateMachineComponent<RemoteWorkerSM.StatesInstance>
{
	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x06004B3D RID: 19261 RVA: 0x001AD69C File Offset: 0x001AB89C
	// (set) Token: 0x06004B3E RID: 19262 RVA: 0x001AD6A4 File Offset: 0x001AB8A4
	public bool Docked
	{
		get
		{
			return this.docked;
		}
		set
		{
			this.docked = value;
		}
	}

	// Token: 0x06004B3F RID: 19263 RVA: 0x001AD6AD File Offset: 0x001AB8AD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004B40 RID: 19264 RVA: 0x001AD6C0 File Offset: 0x001AB8C0
	public void SetNextChore(Chore.Precondition.Context next)
	{
		this.nextChore = new Chore.Precondition.Context?(next);
	}

	// Token: 0x06004B41 RID: 19265 RVA: 0x001AD6CE File Offset: 0x001AB8CE
	public void StartNextChore()
	{
		if (this.nextChore != null)
		{
			this.driver.SetChore(this.nextChore.Value);
			this.nextChore = null;
		}
	}

	// Token: 0x06004B42 RID: 19266 RVA: 0x001AD6FF File Offset: 0x001AB8FF
	public bool HasChoreQueued()
	{
		return this.nextChore != null;
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x06004B43 RID: 19267 RVA: 0x001AD70C File Offset: 0x001AB90C
	// (set) Token: 0x06004B44 RID: 19268 RVA: 0x001AD71F File Offset: 0x001AB91F
	public RemoteWorkerDock HomeDepot
	{
		get
		{
			Ref<RemoteWorkerDock> @ref = this.homeDepot;
			if (@ref == null)
			{
				return null;
			}
			return @ref.Get();
		}
		set
		{
			this.homeDepot = new Ref<RemoteWorkerDock>(value);
		}
	}

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x06004B45 RID: 19269 RVA: 0x001AD72D File Offset: 0x001AB92D
	// (set) Token: 0x06004B46 RID: 19270 RVA: 0x001AD735 File Offset: 0x001AB935
	public bool ActivelyControlled { get; set; }

	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x06004B47 RID: 19271 RVA: 0x001AD73E File Offset: 0x001AB93E
	// (set) Token: 0x06004B48 RID: 19272 RVA: 0x001AD746 File Offset: 0x001AB946
	public bool ActivelyWorking { get; set; }

	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x06004B49 RID: 19273 RVA: 0x001AD74F File Offset: 0x001AB94F
	// (set) Token: 0x06004B4A RID: 19274 RVA: 0x001AD757 File Offset: 0x001AB957
	public bool Available { get; set; }

	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x06004B4B RID: 19275 RVA: 0x001AD760 File Offset: 0x001AB960
	public bool RequiresMaintnence
	{
		get
		{
			return this.power.IsLowPower;
		}
	}

	// Token: 0x06004B4C RID: 19276 RVA: 0x001AD770 File Offset: 0x001AB970
	public void TickResources(float dt)
	{
		this.power.ApplyDeltaEnergy(-0.1f * dt);
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float temperature;
		this.storage.ConsumeAndGetDisease(GameTags.LubricatingOil, 0.1f * dt, out num, out diseaseInfo, out temperature);
		if (num > 0f)
		{
			this.storage.AddElement(SimHashes.LiquidGunk, num, temperature, diseaseInfo.idx, diseaseInfo.count, true, true);
		}
	}

	// Token: 0x06004B4D RID: 19277 RVA: 0x001AD7D6 File Offset: 0x001AB9D6
	public GameObject FindStation()
	{
		if (Components.ComplexFabricators.Count == 0)
		{
			return null;
		}
		return Components.ComplexFabricators[0].gameObject;
	}

	// Token: 0x06004B4E RID: 19278 RVA: 0x001AD7F6 File Offset: 0x001AB9F6
	public bool HasHomeDepot()
	{
		return !this.HomeDepot.IsNullOrDestroyed();
	}

	// Token: 0x04003147 RID: 12615
	[MyCmpAdd]
	private RemoteWorkerCapacitor power;

	// Token: 0x04003148 RID: 12616
	[MyCmpAdd]
	private RemoteWorkerGunkMonitor gunk;

	// Token: 0x04003149 RID: 12617
	[MyCmpAdd]
	private RemoteWorkerOilMonitor oil;

	// Token: 0x0400314A RID: 12618
	[MyCmpAdd]
	private ChoreDriver driver;

	// Token: 0x0400314B RID: 12619
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400314C RID: 12620
	public bool playNewWorker;

	// Token: 0x0400314D RID: 12621
	[Serialize]
	private bool docked = true;

	// Token: 0x0400314E RID: 12622
	private Chore.Precondition.Context? nextChore;

	// Token: 0x0400314F RID: 12623
	private const string LostAnim_pre = "incapacitate_pre";

	// Token: 0x04003150 RID: 12624
	private const string LostAnim_loop = "incapacitate_loop";

	// Token: 0x04003151 RID: 12625
	private const string DeathAnim = "incapacitate_death";

	// Token: 0x04003152 RID: 12626
	[Serialize]
	private Ref<RemoteWorkerDock> homeDepot;

	// Token: 0x04003153 RID: 12627
	private Chore.Precondition.Context enterDockContext;

	// Token: 0x04003154 RID: 12628
	private Chore.Precondition.Context exitDockContext;

	// Token: 0x02001A3E RID: 6718
	public class StatesInstance : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.GameInstance
	{
		// Token: 0x06009F75 RID: 40821 RVA: 0x0037C765 File Offset: 0x0037A965
		public StatesInstance(RemoteWorkerSM master) : base(master)
		{
			base.sm.homedock.Set(base.smi.master.HomeDepot, base.smi);
		}
	}

	// Token: 0x02001A3F RID: 6719
	public class States : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM>
	{
		// Token: 0x06009F76 RID: 40822 RVA: 0x0037C794 File Offset: 0x0037A994
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.uncontrolled;
			this.controlled.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = false;
			}).EnterTransition(this.controlled.exit_dock, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock)).EnterTransition(this.controlled.working, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock))).Transition(this.uncontrolled, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasRemoteOperator)), UpdateRate.SIM_200ms).Transition(this.incapacitated.lost, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot)), UpdateRate.SIM_200ms).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)), UpdateRate.SIM_200ms).Update(new Action<RemoteWorkerSM.StatesInstance, float>(RemoteWorkerSM.States.TickResources), UpdateRate.RENDER_200ms, false);
			this.controlled.exit_dock.ToggleWork<ExitableDock>(this.homedock, this.controlled.working, this.controlled.working, (RemoteWorkerSM.StatesInstance _) => true);
			this.controlled.working.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.ActivelyWorking = true;
			}).Exit(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.ActivelyWorking = false;
			}).DefaultState(this.controlled.working.find_work);
			this.controlled.working.find_work.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				if (RemoteWorkerSM.States.HasChore(smi))
				{
					smi.GoTo(this.controlled.working.do_work);
					return;
				}
				RemoteWorkerSM.States.SetNextChore(smi);
				smi.GoTo(RemoteWorkerSM.States.HasChore(smi) ? this.controlled.working.do_work : this.controlled.no_work);
			});
			this.controlled.working.do_work.Exit(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.ClearChore)).Transition(this.controlled.working.find_work, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChore)), UpdateRate.SIM_200ms);
			this.controlled.no_work.Transition(this.controlled.working.do_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChore), UpdateRate.SIM_200ms).Transition(this.controlled.working.find_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChoreQueued), UpdateRate.SIM_200ms);
			this.uncontrolled.EnterTransition(this.uncontrolled.working.new_worker, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsNewWorker)).EnterTransition(this.uncontrolled.idle, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock)).EnterTransition(this.uncontrolled.approach_dock, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock))).Transition(this.controlled.working.find_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasRemoteOperator), UpdateRate.SIM_200ms).Transition(this.incapacitated.lost, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot)), UpdateRate.SIM_200ms).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)), UpdateRate.SIM_200ms);
			this.uncontrolled.approach_dock.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = true;
			}).MoveTo<IApproachable>(this.homedock, this.uncontrolled.working.enter, this.uncontrolled.idle, null, null);
			this.uncontrolled.working.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = false;
			});
			this.uncontrolled.working.new_worker.ToggleWork<NewWorker>(this.homedock, this.uncontrolled.working.recharge, this.uncontrolled.working.recharge, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.enter.ToggleWork<EnterableDock>(this.homedock, this.uncontrolled.working.recharge, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.recharge.ToggleWork<WorkerRecharger>(this.homedock, this.uncontrolled.working.recharge_pst, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.recharge_pst.OnAnimQueueComplete(this.uncontrolled.working.drain_gunk).ScheduleGoTo(1f, this.uncontrolled.working.drain_gunk);
			this.uncontrolled.working.drain_gunk.ToggleWork<WorkerGunkRemover>(this.homedock, this.uncontrolled.working.drain_gunk_pst, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.drain_gunk_pst.OnAnimQueueComplete(this.uncontrolled.working.fill_oil).ScheduleGoTo(1f, this.uncontrolled.working.fill_oil);
			this.uncontrolled.working.fill_oil.ToggleWork<WorkerOilRefiller>(this.homedock, this.uncontrolled.working.fill_oil_pst, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.fill_oil_pst.OnAnimQueueComplete(this.uncontrolled.idle).ScheduleGoTo(1f, this.uncontrolled.idle);
			this.uncontrolled.idle.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = true;
			}).PlayAnim(RemoteWorkerConfig.IDLE_IN_DOCK_ANIM, KAnim.PlayMode.Loop).Transition(this.uncontrolled.working.recharge, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.And(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.RequiresMaintnence), new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.DockIsOperational)), UpdateRate.SIM_1000ms);
			this.incapacitated.ToggleAnims("anim_incapacitated_kanim", 0f);
			this.incapacitated.lost.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.Play("incapacitate_pre", KAnim.PlayMode.Once);
				smi.Queue("incapacitate_loop", KAnim.PlayMode.Loop);
				RemoteWorkerSM.States.ClearChore(smi);
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.UnreachableDock, null).Transition(this.controlled, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot), UpdateRate.SIM_200ms).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)), UpdateRate.SIM_200ms);
			this.incapacitated.die.Enter(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.ClearChore)).PlayAnim("incapacitate_death").OnAnimQueueComplete(this.incapacitated.explode).ToggleStatusItem(Db.Get().DuplicantStatusItems.NoHomeDock, null);
			this.incapacitated.explode.Enter(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.Explode));
		}

		// Token: 0x06009F77 RID: 40823 RVA: 0x0037CF1D File Offset: 0x0037B11D
		public static bool IsNewWorker(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.playNewWorker;
		}

		// Token: 0x06009F78 RID: 40824 RVA: 0x0037CF2A File Offset: 0x0037B12A
		public static void SetNextChore(RemoteWorkerSM.StatesInstance smi)
		{
			smi.master.StartNextChore();
		}

		// Token: 0x06009F79 RID: 40825 RVA: 0x0037CF37 File Offset: 0x0037B137
		public static void ClearChore(RemoteWorkerSM.StatesInstance smi)
		{
			smi.master.driver.StopChore();
		}

		// Token: 0x06009F7A RID: 40826 RVA: 0x0037CF49 File Offset: 0x0037B149
		public static bool HasChore(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.driver.HasChore();
		}

		// Token: 0x06009F7B RID: 40827 RVA: 0x0037CF5B File Offset: 0x0037B15B
		public static bool HasChoreQueued(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.HasChoreQueued();
		}

		// Token: 0x06009F7C RID: 40828 RVA: 0x0037CF68 File Offset: 0x0037B168
		public static bool CanReachDepot(RemoteWorkerSM.StatesInstance smi)
		{
			int depotCell = RemoteWorkerSM.States.GetDepotCell(smi);
			return depotCell != Grid.InvalidCell && smi.master.GetComponent<Navigator>().CanReach(depotCell);
		}

		// Token: 0x06009F7D RID: 40829 RVA: 0x0037CF98 File Offset: 0x0037B198
		public static int GetDepotCell(RemoteWorkerSM.StatesInstance smi)
		{
			RemoteWorkerDock homeDepot = smi.master.HomeDepot;
			if (homeDepot == null)
			{
				return Grid.InvalidCell;
			}
			return Grid.PosToCell(homeDepot);
		}

		// Token: 0x06009F7E RID: 40830 RVA: 0x0037CFC6 File Offset: 0x0037B1C6
		public static bool HasRemoteOperator(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.ActivelyControlled;
		}

		// Token: 0x06009F7F RID: 40831 RVA: 0x0037CFD3 File Offset: 0x0037B1D3
		public static bool RequiresMaintnence(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.RequiresMaintnence;
		}

		// Token: 0x06009F80 RID: 40832 RVA: 0x0037CFE0 File Offset: 0x0037B1E0
		public static bool DockIsOperational(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.HomeDepot != null && smi.master.HomeDepot.IsOperational;
		}

		// Token: 0x06009F81 RID: 40833 RVA: 0x0037D007 File Offset: 0x0037B207
		public static bool HasHomeDepot(RemoteWorkerSM.StatesInstance smi)
		{
			return RemoteWorkerSM.States.GetDepotCell(smi) != Grid.InvalidCell;
		}

		// Token: 0x06009F82 RID: 40834 RVA: 0x0037D019 File Offset: 0x0037B219
		public static void StopWork(RemoteWorkerSM.StatesInstance smi)
		{
			if (smi.master.driver.HasChore())
			{
				smi.master.driver.StopChore();
			}
		}

		// Token: 0x06009F83 RID: 40835 RVA: 0x0037D03D File Offset: 0x0037B23D
		public static bool IsInsideDock(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.Docked;
		}

		// Token: 0x06009F84 RID: 40836 RVA: 0x0037D04C File Offset: 0x0037B24C
		public static void Explode(RemoteWorkerSM.StatesInstance smi)
		{
			Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, smi.master.transform.position, 0f);
			PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
			component.Element.substance.SpawnResource(Grid.CellToPosCCC(Grid.PosToCell(smi.master.gameObject), Grid.SceneLayer.Ore), 42f, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, false, false);
			Util.KDestroyGameObject(smi.master.gameObject);
		}

		// Token: 0x06009F85 RID: 40837 RVA: 0x0037D0DB File Offset: 0x0037B2DB
		public static void TickResources(RemoteWorkerSM.StatesInstance smi, float dt)
		{
			if (dt > 0f)
			{
				smi.master.TickResources(dt);
			}
		}

		// Token: 0x04007BC8 RID: 31688
		public RemoteWorkerSM.States.ControlledStates controlled;

		// Token: 0x04007BC9 RID: 31689
		public RemoteWorkerSM.States.UncontrolledStates uncontrolled;

		// Token: 0x04007BCA RID: 31690
		public RemoteWorkerSM.States.IncapacitatedStates incapacitated;

		// Token: 0x04007BCB RID: 31691
		public StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.TargetParameter homedock;

		// Token: 0x020025F4 RID: 9716
		public class ControlledStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
		{
			// Token: 0x0400A8F3 RID: 43251
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State exit_dock;

			// Token: 0x0400A8F4 RID: 43252
			public RemoteWorkerSM.States.ControlledStates.WorkingStates working;

			// Token: 0x0400A8F5 RID: 43253
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State no_work;

			// Token: 0x02003540 RID: 13632
			public class WorkingStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
			{
				// Token: 0x0400D7BC RID: 55228
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State find_work;

				// Token: 0x0400D7BD RID: 55229
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State do_work;
			}
		}

		// Token: 0x020025F5 RID: 9717
		public class UncontrolledStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
		{
			// Token: 0x0400A8F6 RID: 43254
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State approach_dock;

			// Token: 0x0400A8F7 RID: 43255
			public RemoteWorkerSM.States.UncontrolledStates.WorkingDockStates working;

			// Token: 0x0400A8F8 RID: 43256
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State idle;

			// Token: 0x02003541 RID: 13633
			public class WorkingDockStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
			{
				// Token: 0x0400D7BE RID: 55230
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State new_worker;

				// Token: 0x0400D7BF RID: 55231
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State enter;

				// Token: 0x0400D7C0 RID: 55232
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State recharge;

				// Token: 0x0400D7C1 RID: 55233
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State recharge_pst;

				// Token: 0x0400D7C2 RID: 55234
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State drain_gunk;

				// Token: 0x0400D7C3 RID: 55235
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State drain_gunk_pst;

				// Token: 0x0400D7C4 RID: 55236
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State fill_oil;

				// Token: 0x0400D7C5 RID: 55237
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State fill_oil_pst;
			}
		}

		// Token: 0x020025F6 RID: 9718
		public class IncapacitatedStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
		{
			// Token: 0x0400A8F9 RID: 43257
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State lost;

			// Token: 0x0400A8FA RID: 43258
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State die;

			// Token: 0x0400A8FB RID: 43259
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State explode;
		}
	}
}
