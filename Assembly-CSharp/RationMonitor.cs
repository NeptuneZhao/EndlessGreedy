using System;

// Token: 0x02000996 RID: 2454
public class RationMonitor : GameStateMachine<RationMonitor, RationMonitor.Instance>
{
	// Token: 0x06004792 RID: 18322 RVA: 0x00199AE4 File Offset: 0x00197CE4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.rationsavailable;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.EventHandler(GameHashes.EatCompleteEater, delegate(RationMonitor.Instance smi, object d)
		{
			smi.OnEatComplete(d);
		}).EventHandler(GameHashes.NewDay, (RationMonitor.Instance smi) => GameClock.Instance, delegate(RationMonitor.Instance smi)
		{
			smi.OnNewDay();
		}).ParamTransition<float>(this.rationsAteToday, this.rationsavailable, (RationMonitor.Instance smi, float p) => smi.HasRationsAvailable()).ParamTransition<float>(this.rationsAteToday, this.outofrations, (RationMonitor.Instance smi, float p) => !smi.HasRationsAvailable());
		this.rationsavailable.DefaultState(this.rationsavailable.noediblesavailable);
		this.rationsavailable.noediblesavailable.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.NoRationsAvailable).EventTransition(GameHashes.ColonyHasRationsChanged, new Func<RationMonitor.Instance, KMonoBehaviour>(RationMonitor.GetSaveGame), this.rationsavailable.ediblesunreachable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.AreThereAnyEdibles));
		this.rationsavailable.ediblereachablebutnotpermitted.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.RationsNotPermitted).EventTransition(GameHashes.ColonyHasRationsChanged, new Func<RationMonitor.Instance, KMonoBehaviour>(RationMonitor.GetSaveGame), this.rationsavailable.noediblesavailable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.AreThereNoEdibles)).EventTransition(GameHashes.ClosestEdibleChanged, this.rationsavailable.ediblesunreachable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.NotIsEdibleInReachButNotPermitted));
		this.rationsavailable.ediblesunreachable.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.RationsUnreachable).EventTransition(GameHashes.ColonyHasRationsChanged, new Func<RationMonitor.Instance, KMonoBehaviour>(RationMonitor.GetSaveGame), this.rationsavailable.noediblesavailable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.AreThereNoEdibles)).EventTransition(GameHashes.ClosestEdibleChanged, this.rationsavailable.edibleavailable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.IsEdibleAvailable)).EventTransition(GameHashes.ClosestEdibleChanged, this.rationsavailable.ediblereachablebutnotpermitted, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.IsEdibleInReachButNotPermitted));
		this.rationsavailable.edibleavailable.ToggleChore((RationMonitor.Instance smi) => new EatChore(smi.master), this.rationsavailable.noediblesavailable).DefaultState(this.rationsavailable.edibleavailable.readytoeat);
		this.rationsavailable.edibleavailable.readytoeat.EventTransition(GameHashes.ClosestEdibleChanged, this.rationsavailable.noediblesavailable, null).EventTransition(GameHashes.BeginChore, this.rationsavailable.edibleavailable.eating, (RationMonitor.Instance smi) => smi.IsEating());
		this.rationsavailable.edibleavailable.eating.DoNothing();
		this.outofrations.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.DailyRationLimitReached);
	}

	// Token: 0x06004793 RID: 18323 RVA: 0x00199E32 File Offset: 0x00198032
	private static bool AreThereNoEdibles(RationMonitor.Instance smi)
	{
		return !RationMonitor.AreThereAnyEdibles(smi);
	}

	// Token: 0x06004794 RID: 18324 RVA: 0x00199E40 File Offset: 0x00198040
	private static bool AreThereAnyEdibles(RationMonitor.Instance smi)
	{
		if (SaveGame.Instance != null)
		{
			ColonyRationMonitor.Instance smi2 = SaveGame.Instance.GetSMI<ColonyRationMonitor.Instance>();
			if (smi2 != null)
			{
				return !smi2.IsOutOfRations();
			}
		}
		return false;
	}

	// Token: 0x06004795 RID: 18325 RVA: 0x00199E73 File Offset: 0x00198073
	private static KMonoBehaviour GetSaveGame(RationMonitor.Instance smi)
	{
		return SaveGame.Instance;
	}

	// Token: 0x06004796 RID: 18326 RVA: 0x00199E7A File Offset: 0x0019807A
	private static bool IsEdibleAvailable(RationMonitor.Instance smi)
	{
		return smi.GetEdible() != null;
	}

	// Token: 0x06004797 RID: 18327 RVA: 0x00199E88 File Offset: 0x00198088
	private static bool NotIsEdibleInReachButNotPermitted(RationMonitor.Instance smi)
	{
		return !RationMonitor.IsEdibleInReachButNotPermitted(smi);
	}

	// Token: 0x06004798 RID: 18328 RVA: 0x00199E93 File Offset: 0x00198093
	private static bool IsEdibleInReachButNotPermitted(RationMonitor.Instance smi)
	{
		return smi.GetComponent<Sensors>().GetSensor<ClosestEdibleSensor>().edibleInReachButNotPermitted;
	}

	// Token: 0x04002EC7 RID: 11975
	public StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.FloatParameter rationsAteToday;

	// Token: 0x04002EC8 RID: 11976
	public RationMonitor.RationsAvailableState rationsavailable;

	// Token: 0x04002EC9 RID: 11977
	public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState outofrations;

	// Token: 0x02001968 RID: 6504
	public class EdibleAvailablestate : GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007954 RID: 31060
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State readytoeat;

		// Token: 0x04007955 RID: 31061
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State eating;
	}

	// Token: 0x02001969 RID: 6505
	public class RationsAvailableState : GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007956 RID: 31062
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState noediblesavailable;

		// Token: 0x04007957 RID: 31063
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState ediblereachablebutnotpermitted;

		// Token: 0x04007958 RID: 31064
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState ediblesunreachable;

		// Token: 0x04007959 RID: 31065
		public RationMonitor.EdibleAvailablestate edibleavailable;
	}

	// Token: 0x0200196A RID: 6506
	public new class Instance : GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009C71 RID: 40049 RVA: 0x00372492 File Offset: 0x00370692
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.choreDriver = master.GetComponent<ChoreDriver>();
		}

		// Token: 0x06009C72 RID: 40050 RVA: 0x003724A7 File Offset: 0x003706A7
		public Edible GetEdible()
		{
			return base.GetComponent<Sensors>().GetSensor<ClosestEdibleSensor>().GetEdible();
		}

		// Token: 0x06009C73 RID: 40051 RVA: 0x003724B9 File Offset: 0x003706B9
		public bool HasRationsAvailable()
		{
			return true;
		}

		// Token: 0x06009C74 RID: 40052 RVA: 0x003724BC File Offset: 0x003706BC
		public float GetRationsAteToday()
		{
			return base.sm.rationsAteToday.Get(base.smi);
		}

		// Token: 0x06009C75 RID: 40053 RVA: 0x003724D4 File Offset: 0x003706D4
		public float GetRationsRemaining()
		{
			return 1f;
		}

		// Token: 0x06009C76 RID: 40054 RVA: 0x003724DB File Offset: 0x003706DB
		public bool IsEating()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
		}

		// Token: 0x06009C77 RID: 40055 RVA: 0x00372512 File Offset: 0x00370712
		public void OnNewDay()
		{
			base.smi.sm.rationsAteToday.Set(0f, base.smi, false);
		}

		// Token: 0x06009C78 RID: 40056 RVA: 0x00372538 File Offset: 0x00370738
		public void OnEatComplete(object data)
		{
			Edible edible = (Edible)data;
			base.sm.rationsAteToday.Delta(edible.caloriesConsumed, base.smi);
			WorldResourceAmountTracker<RationTracker>.Get().RegisterAmountConsumed(edible.FoodInfo.Id, edible.caloriesConsumed);
		}

		// Token: 0x0400795A RID: 31066
		private ChoreDriver choreDriver;
	}
}
