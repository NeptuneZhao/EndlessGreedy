using System;

// Token: 0x020006E9 RID: 1769
public class GunkEmptier : GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>
{
	// Token: 0x06002D07 RID: 11527 RVA: 0x000FD1D4 File Offset: 0x000FB3D4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.noOperational.EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.IsOperational));
		this.operational.EventTransition(GameHashes.OperationalChanged, this.noOperational, GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Not(new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.IsOperational))).DefaultState(this.operational.noStorageSpace);
		this.operational.noStorageSpace.ToggleStatusItem(Db.Get().BuildingStatusItems.GunkEmptierFull, null).EventTransition(GameHashes.OnStorageChange, this.operational.ready, new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.HasSpaceToEmptyABionicGunkTank));
		this.operational.ready.EventTransition(GameHashes.OnStorageChange, this.operational.noStorageSpace, GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Not(new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.HasSpaceToEmptyABionicGunkTank))).ToggleChore(new Func<GunkEmptier.Instance, Chore>(GunkEmptier.CreateChore), this.operational.noStorageSpace);
	}

	// Token: 0x06002D08 RID: 11528 RVA: 0x000FD2DB File Offset: 0x000FB4DB
	public static bool HasSpaceToEmptyABionicGunkTank(GunkEmptier.Instance smi)
	{
		return smi.RemainingStorageCapacity >= GunkMonitor.GUNK_CAPACITY;
	}

	// Token: 0x06002D09 RID: 11529 RVA: 0x000FD2ED File Offset: 0x000FB4ED
	public static bool IsOperational(GunkEmptier.Instance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x06002D0A RID: 11530 RVA: 0x000FD2F8 File Offset: 0x000FB4F8
	private static WorkChore<GunkEmptierWorkable> CreateChore(GunkEmptier.Instance smi)
	{
		return new WorkChore<GunkEmptierWorkable>(Db.Get().ChoreTypes.ExpellGunk, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
	}

	// Token: 0x04001A00 RID: 6656
	public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State noOperational;

	// Token: 0x04001A01 RID: 6657
	public GunkEmptier.OperationalStates operational;

	// Token: 0x0200150E RID: 5390
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200150F RID: 5391
	public class OperationalStates : GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State
	{
		// Token: 0x04006BE0 RID: 27616
		public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State noStorageSpace;

		// Token: 0x04006BE1 RID: 27617
		public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State ready;
	}

	// Token: 0x02001510 RID: 5392
	public new class Instance : GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.GameInstance
	{
		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06008D1D RID: 36125 RVA: 0x0033E0AE File Offset: 0x0033C2AE
		public float RemainingStorageCapacity
		{
			get
			{
				return this.storage.RemainingCapacity();
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06008D1E RID: 36126 RVA: 0x0033E0BB File Offset: 0x0033C2BB
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x06008D1F RID: 36127 RVA: 0x0033E0C8 File Offset: 0x0033C2C8
		public Instance(IStateMachineTarget master, GunkEmptier.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<Storage>();
			this.operational = base.GetComponent<Operational>();
		}

		// Token: 0x04006BE2 RID: 27618
		private Operational operational;

		// Token: 0x04006BE3 RID: 27619
		private Storage storage;
	}
}
