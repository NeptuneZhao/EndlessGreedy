using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000680 RID: 1664
public class ArtifactAnalysisStation : GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>
{
	// Token: 0x06002929 RID: 10537 RVA: 0x000E8DD4 File Offset: 0x000E6FD4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational));
		this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Not(new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.HasArtifactToStudy));
		this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Not(new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Not(new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.HasArtifactToStudy))).ToggleChore(new Func<ArtifactAnalysisStation.StatesInstance, Chore>(this.CreateChore), this.operational);
	}

	// Token: 0x0600292A RID: 10538 RVA: 0x000E8EB0 File Offset: 0x000E70B0
	private bool HasArtifactToStudy(ArtifactAnalysisStation.StatesInstance smi)
	{
		return smi.storage.GetMassAvailable(GameTags.CharmedArtifact) >= 1f;
	}

	// Token: 0x0600292B RID: 10539 RVA: 0x000E8ECC File Offset: 0x000E70CC
	private bool IsOperational(ArtifactAnalysisStation.StatesInstance smi)
	{
		return smi.GetComponent<Operational>().IsOperational;
	}

	// Token: 0x0600292C RID: 10540 RVA: 0x000E8EDC File Offset: 0x000E70DC
	private Chore CreateChore(ArtifactAnalysisStation.StatesInstance smi)
	{
		return new WorkChore<ArtifactAnalysisStationWorkable>(Db.Get().ChoreTypes.AnalyzeArtifact, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x040017B3 RID: 6067
	public GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.State inoperational;

	// Token: 0x040017B4 RID: 6068
	public GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.State operational;

	// Token: 0x040017B5 RID: 6069
	public GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.State ready;

	// Token: 0x02001464 RID: 5220
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001465 RID: 5221
	public class StatesInstance : GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.GameInstance
	{
		// Token: 0x06008A7B RID: 35451 RVA: 0x00334095 File Offset: 0x00332295
		public StatesInstance(IStateMachineTarget master, ArtifactAnalysisStation.Def def) : base(master, def)
		{
			this.workable.statesInstance = this;
		}

		// Token: 0x06008A7C RID: 35452 RVA: 0x003340AB File Offset: 0x003322AB
		public override void StartSM()
		{
			base.StartSM();
		}

		// Token: 0x040069A5 RID: 27045
		[MyCmpReq]
		public Storage storage;

		// Token: 0x040069A6 RID: 27046
		[MyCmpReq]
		public ManualDeliveryKG manualDelivery;

		// Token: 0x040069A7 RID: 27047
		[MyCmpReq]
		public ArtifactAnalysisStationWorkable workable;

		// Token: 0x040069A8 RID: 27048
		[Serialize]
		private HashSet<Tag> forbiddenSeeds;
	}
}
