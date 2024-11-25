using System;
using UnityEngine;

// Token: 0x02000430 RID: 1072
public class BionicMassOxygenAbsorbChore : Chore<BionicMassOxygenAbsorbChore.Instance>
{
	// Token: 0x060016D4 RID: 5844 RVA: 0x0007A5E8 File Offset: 0x000787E8
	public BionicMassOxygenAbsorbChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.BionicAbsorbOxygen, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BionicMassOxygenAbsorbChore.Instance(this, target.gameObject);
		Func<int> data = new Func<int>(base.smi.UpdateTargetCell);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCellUntilBegun, data);
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x0007A664 File Offset: 0x00078864
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("BionicMassAbsorbOxygenChore null context.consumer");
			return;
		}
		if (context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>() == null)
		{
			global::Debug.LogError("BionicMassAbsorbOxygenChore null BionicOxygenTankMonitor.Instance");
			return;
		}
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x060016D6 RID: 5846 RVA: 0x0007A6D9 File Offset: 0x000788D9
	public static void ResetOxygenTimer(BionicMassOxygenAbsorbChore.Instance smi)
	{
		smi.sm.SecondsPassedWithoutOxygen.Set(0f, smi, false);
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x0007A6F3 File Offset: 0x000788F3
	public static void RefreshTargetSafeCell(BionicMassOxygenAbsorbChore.Instance smi)
	{
		smi.UpdateTargetCell();
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x0007A6FC File Offset: 0x000788FC
	public static void UpdateTargetSafeCell(BionicMassOxygenAbsorbChore.Instance smi, float dt)
	{
		BionicMassOxygenAbsorbChore.RefreshTargetSafeCell(smi);
	}

	// Token: 0x060016D9 RID: 5849 RVA: 0x0007A704 File Offset: 0x00078904
	public static bool HasSpaceInOxygenTank(BionicMassOxygenAbsorbChore.Instance smi)
	{
		return smi.oxygenTankMonitor.SpaceAvailableInTank > 0f;
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x0007A718 File Offset: 0x00078918
	public static void AbsorbUpdate(BionicMassOxygenAbsorbChore.Instance smi, float dt)
	{
		float mass = Mathf.Min(dt * BionicMassOxygenAbsorbChore.ABSORB_RATE, smi.oxygenTankMonitor.SpaceAvailableInTank);
		BionicMassOxygenAbsorbChore.AbsorbUpdateData absorbUpdateData = new BionicMassOxygenAbsorbChore.AbsorbUpdateData(smi, dt);
		int gameCell;
		SimHashes nearBreathableElement = BionicMassOxygenAbsorbChore.GetNearBreathableElement(gameCell = Grid.PosToCell(smi.sm.dupe.Get(smi)), BionicMassOxygenAbsorbChore.ABSORB_RANGE, out gameCell);
		HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(BionicMassOxygenAbsorbChore.OnSimConsumeCallback), absorbUpdateData, "BionicMassOxygenAbsorbChore");
		SimMessages.ConsumeMass(gameCell, nearBreathableElement, mass, 3, handle.index);
	}

	// Token: 0x060016DB RID: 5851 RVA: 0x0007A7A4 File Offset: 0x000789A4
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		BionicMassOxygenAbsorbChore.AbsorbUpdateData absorbUpdateData = (BionicMassOxygenAbsorbChore.AbsorbUpdateData)data;
		absorbUpdateData.smi.OnSimConsume(mass_cb_info, absorbUpdateData.dt);
	}

	// Token: 0x060016DC RID: 5852 RVA: 0x0007A7CC File Offset: 0x000789CC
	public static SimHashes GetNearBreathableElement(int centralCell, CellOffset[] range, out int elementCell)
	{
		float num = 0f;
		int num2 = centralCell;
		SimHashes simHashes = SimHashes.Vacuum;
		foreach (CellOffset offset in range)
		{
			int num3 = Grid.OffsetCell(centralCell, offset);
			SimHashes simHashes2 = SimHashes.Vacuum;
			float breathableMassInCell = BionicMassOxygenAbsorbChore.GetBreathableMassInCell(num3, out simHashes2);
			if (breathableMassInCell > Mathf.Epsilon && (simHashes == SimHashes.Vacuum || breathableMassInCell > num))
			{
				simHashes = simHashes2;
				num = breathableMassInCell;
				num2 = num3;
			}
		}
		elementCell = num2;
		return simHashes;
	}

	// Token: 0x060016DD RID: 5853 RVA: 0x0007A844 File Offset: 0x00078A44
	private static float GetBreathableMassInCell(int cell, out SimHashes elementID)
	{
		if (Grid.IsValidCell(cell))
		{
			Element element = Grid.Element[cell];
			if (element.HasTag(GameTags.Breathable))
			{
				elementID = element.id;
				return Grid.Mass[cell];
			}
		}
		elementID = SimHashes.Vacuum;
		return 0f;
	}

	// Token: 0x04000CBB RID: 3259
	public static CellOffset[] ABSORB_RANGE = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(1, 1),
		new CellOffset(-1, 1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x04000CBC RID: 3260
	public const float GIVE_UP_DURATION = 2f;

	// Token: 0x04000CBD RID: 3261
	public const float ABSORB_RATE_IDEAL_CHORE_DURATION = 30f;

	// Token: 0x04000CBE RID: 3262
	public static readonly float ABSORB_RATE = BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG / 30f;

	// Token: 0x04000CBF RID: 3263
	public const string ABSORB_ANIM_FILE = "anim_banshee_kanim";

	// Token: 0x04000CC0 RID: 3264
	public const string ABSORB_PRE_ANIM_NAME = "working_pre";

	// Token: 0x04000CC1 RID: 3265
	public const string ABSORB_LOOP_ANIM_NAME = "working_loop";

	// Token: 0x04000CC2 RID: 3266
	public const string ABSORB_PST_ANIM_NAME = "working_pst";

	// Token: 0x04000CC3 RID: 3267
	public static CellOffset MouthCellOffset = new CellOffset(0, 1);

	// Token: 0x020011A6 RID: 4518
	public class States : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore>
	{
		// Token: 0x0600807A RID: 32890 RVA: 0x0031117C File Offset: 0x0030F37C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.move;
			base.Target(this.dupe);
			this.move.DefaultState(this.move.onGoing);
			this.move.onGoing.Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.RefreshTargetSafeCell)).Update(new Action<BionicMassOxygenAbsorbChore.Instance, float>(BionicMassOxygenAbsorbChore.UpdateTargetSafeCell), UpdateRate.SIM_200ms, false).MoveTo((BionicMassOxygenAbsorbChore.Instance smi) => smi.targetCell, this.absorb, this.move.fail, true);
			this.move.fail.ReturnFailure();
			this.absorb.ToggleTag(GameTags.RecoveringBreath).ToggleAnims("anim_banshee_kanim", 0f).DefaultState(this.absorb.pre);
			this.absorb.pre.PlayAnim("working_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.absorb.loop).ScheduleGoTo(3f, this.absorb.loop).Exit(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ResetOxygenTimer));
			this.absorb.loop.Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ResetOxygenTimer)).ParamTransition<float>(this.SecondsPassedWithoutOxygen, this.absorb.pst, (BionicMassOxygenAbsorbChore.Instance smi, float secondsPassed) => secondsPassed > 2f).OnSignal(this.TankFilledSignal, this.absorb.pst).PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(new Action<BionicMassOxygenAbsorbChore.Instance, float>(BionicMassOxygenAbsorbChore.AbsorbUpdate), UpdateRate.SIM_200ms, false);
			this.absorb.pst.PlayAnim("working_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x040060B2 RID: 24754
		public BionicMassOxygenAbsorbChore.States.MoveStates move;

		// Token: 0x040060B3 RID: 24755
		public BionicMassOxygenAbsorbChore.States.MassAbsorbStates absorb;

		// Token: 0x040060B4 RID: 24756
		public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State complete;

		// Token: 0x040060B5 RID: 24757
		public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.FloatParameter SecondsPassedWithoutOxygen;

		// Token: 0x040060B6 RID: 24758
		public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.TargetParameter dupe;

		// Token: 0x040060B7 RID: 24759
		public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Signal TankFilledSignal;

		// Token: 0x020023AE RID: 9134
		public class MoveStates : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
		{
			// Token: 0x04009F5B RID: 40795
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State onGoing;

			// Token: 0x04009F5C RID: 40796
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State fail;
		}

		// Token: 0x020023AF RID: 9135
		public class MassAbsorbStates : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
		{
			// Token: 0x04009F5D RID: 40797
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pre;

			// Token: 0x04009F5E RID: 40798
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State loop;

			// Token: 0x04009F5F RID: 40799
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pst;
		}
	}

	// Token: 0x020011A7 RID: 4519
	public struct AbsorbUpdateData
	{
		// Token: 0x0600807C RID: 32892 RVA: 0x00311374 File Offset: 0x0030F574
		public AbsorbUpdateData(BionicMassOxygenAbsorbChore.Instance smi, float dt)
		{
			this.smi = smi;
			this.dt = dt;
		}

		// Token: 0x040060B8 RID: 24760
		public BionicMassOxygenAbsorbChore.Instance smi;

		// Token: 0x040060B9 RID: 24761
		public float dt;
	}

	// Token: 0x020011A8 RID: 4520
	public class Instance : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.GameInstance
	{
		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x0600807D RID: 32893 RVA: 0x00311384 File Offset: 0x0030F584
		public float OXYGEN_MASS_GIVE_UP_TRESHOLD
		{
			get
			{
				return this.oxygenBreather.ConsumptionRate;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x0600807F RID: 32895 RVA: 0x0031139A File Offset: 0x0030F59A
		// (set) Token: 0x0600807E RID: 32894 RVA: 0x00311391 File Offset: 0x0030F591
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x06008080 RID: 32896 RVA: 0x003113A4 File Offset: 0x0030F5A4
		public Instance(BionicMassOxygenAbsorbChore master, GameObject duplicant) : base(master)
		{
			this.query = new SafetyQuery(Game.Instance.safetyConditions.RecoverBreathChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.navigator = duplicant.GetComponent<Navigator>();
			base.sm.dupe.Set(duplicant, base.smi, false);
			this.dupePrefabID = duplicant.GetComponent<KPrefabID>();
			this.oxygenTankMonitor = duplicant.GetSMI<BionicOxygenTankMonitor.Instance>();
			this.oxygenBreather = duplicant.GetComponent<OxygenBreather>();
		}

		// Token: 0x06008081 RID: 32897 RVA: 0x00311434 File Offset: 0x0030F634
		public int UpdateTargetCell()
		{
			this.query.Reset();
			this.navigator.RunQuery(base.smi.query);
			int cell = base.smi.query.GetResultCell();
			if (!this.oxygenBreather.IsBreathableElementAtCell(cell, null))
			{
				cell = PathFinder.InvalidCell;
			}
			this.targetCell = cell;
			return this.targetCell;
		}

		// Token: 0x06008082 RID: 32898 RVA: 0x00311498 File Offset: 0x0030F698
		public void OnSimConsume(Sim.MassConsumedCallback mass_cb_info, float dt)
		{
			if (this.dupePrefabID == null || this.oxygenBreather == null || this.oxygenTankMonitor == null || this.dupePrefabID.HasTag(GameTags.Dead))
			{
				return;
			}
			GameObject gameObject = this.dupePrefabID.gameObject;
			if (mass_cb_info.mass <= this.OXYGEN_MASS_GIVE_UP_TRESHOLD * dt)
			{
				base.sm.SecondsPassedWithoutOxygen.Set(base.sm.SecondsPassedWithoutOxygen.Get(base.smi) + dt, base.smi, false);
				return;
			}
			if (ElementLoader.elements[(int)mass_cb_info.elemIdx].id == SimHashes.ContaminatedOxygen)
			{
				this.oxygenBreather.Trigger(-935848905, mass_cb_info);
			}
			float num = this.oxygenTankMonitor.AddGas(mass_cb_info);
			if (num > Mathf.Epsilon)
			{
				SimMessages.EmitMass(Grid.PosToCell(gameObject), mass_cb_info.elemIdx, num, mass_cb_info.temperature, byte.MaxValue, 0, -1);
			}
			if (!BionicMassOxygenAbsorbChore.HasSpaceInOxygenTank(this))
			{
				base.sm.TankFilledSignal.Trigger(this);
			}
			BionicMassOxygenAbsorbChore.ResetOxygenTimer(base.smi);
		}

		// Token: 0x040060BA RID: 24762
		public Navigator navigator;

		// Token: 0x040060BB RID: 24763
		public SafetyQuery query;

		// Token: 0x040060BD RID: 24765
		public KPrefabID dupePrefabID;

		// Token: 0x040060BE RID: 24766
		public int targetCell = Grid.InvalidCell;

		// Token: 0x040060BF RID: 24767
		public BionicOxygenTankMonitor.Instance oxygenTankMonitor;
	}
}
