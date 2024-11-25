using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x020006DB RID: 1755
public class GeneticAnalysisStation : GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>
{
	// Token: 0x06002C81 RID: 11393 RVA: 0x000F9AD0 File Offset: 0x000F7CD0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational));
		this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Not(new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.HasSeedToStudy));
		this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Not(new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Not(new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.HasSeedToStudy))).ToggleChore(new Func<GeneticAnalysisStation.StatesInstance, Chore>(this.CreateChore), this.operational);
	}

	// Token: 0x06002C82 RID: 11394 RVA: 0x000F9BAC File Offset: 0x000F7DAC
	private bool HasSeedToStudy(GeneticAnalysisStation.StatesInstance smi)
	{
		return smi.storage.GetMassAvailable(GameTags.UnidentifiedSeed) >= 1f;
	}

	// Token: 0x06002C83 RID: 11395 RVA: 0x000F9BC8 File Offset: 0x000F7DC8
	private bool IsOperational(GeneticAnalysisStation.StatesInstance smi)
	{
		return smi.GetComponent<Operational>().IsOperational;
	}

	// Token: 0x06002C84 RID: 11396 RVA: 0x000F9BD8 File Offset: 0x000F7DD8
	private Chore CreateChore(GeneticAnalysisStation.StatesInstance smi)
	{
		return new WorkChore<GeneticAnalysisStationWorkable>(Db.Get().ChoreTypes.AnalyzeSeed, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x040019AD RID: 6573
	public GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.State inoperational;

	// Token: 0x040019AE RID: 6574
	public GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.State operational;

	// Token: 0x040019AF RID: 6575
	public GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.State ready;

	// Token: 0x020014E7 RID: 5351
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020014E8 RID: 5352
	public class StatesInstance : GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.GameInstance
	{
		// Token: 0x06008C8B RID: 35979 RVA: 0x0033ADE6 File Offset: 0x00338FE6
		public StatesInstance(IStateMachineTarget master, GeneticAnalysisStation.Def def) : base(master, def)
		{
			this.workable.statesInstance = this;
		}

		// Token: 0x06008C8C RID: 35980 RVA: 0x0033ADFC File Offset: 0x00338FFC
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshFetchTags();
		}

		// Token: 0x06008C8D RID: 35981 RVA: 0x0033AE0C File Offset: 0x0033900C
		public void SetSeedForbidden(Tag seedID, bool forbidden)
		{
			if (this.forbiddenSeeds == null)
			{
				this.forbiddenSeeds = new HashSet<Tag>();
			}
			bool flag;
			if (forbidden)
			{
				flag = this.forbiddenSeeds.Add(seedID);
			}
			else
			{
				flag = this.forbiddenSeeds.Remove(seedID);
			}
			if (flag)
			{
				this.RefreshFetchTags();
			}
		}

		// Token: 0x06008C8E RID: 35982 RVA: 0x0033AE54 File Offset: 0x00339054
		public bool GetSeedForbidden(Tag seedID)
		{
			if (this.forbiddenSeeds == null)
			{
				this.forbiddenSeeds = new HashSet<Tag>();
			}
			return this.forbiddenSeeds.Contains(seedID);
		}

		// Token: 0x06008C8F RID: 35983 RVA: 0x0033AE78 File Offset: 0x00339078
		private void RefreshFetchTags()
		{
			if (this.forbiddenSeeds == null)
			{
				this.manualDelivery.ForbiddenTags = null;
				return;
			}
			Tag[] array = new Tag[this.forbiddenSeeds.Count];
			int num = 0;
			foreach (Tag tag in this.forbiddenSeeds)
			{
				array[num++] = tag;
				this.storage.Drop(tag);
			}
			this.manualDelivery.ForbiddenTags = array;
		}

		// Token: 0x04006B56 RID: 27478
		[MyCmpReq]
		public Storage storage;

		// Token: 0x04006B57 RID: 27479
		[MyCmpReq]
		public ManualDeliveryKG manualDelivery;

		// Token: 0x04006B58 RID: 27480
		[MyCmpReq]
		public GeneticAnalysisStationWorkable workable;

		// Token: 0x04006B59 RID: 27481
		[Serialize]
		private HashSet<Tag> forbiddenSeeds;
	}
}
