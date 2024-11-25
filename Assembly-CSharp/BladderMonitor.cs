using System;
using Klei.AI;

// Token: 0x0200096D RID: 2413
public class BladderMonitor : GameStateMachine<BladderMonitor, BladderMonitor.Instance>
{
	// Token: 0x060046BD RID: 18109 RVA: 0x0019473C File Offset: 0x0019293C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.urgentwant, (BladderMonitor.Instance smi) => smi.NeedsToPee(), UpdateRate.SIM_200ms).Transition(this.breakwant, (BladderMonitor.Instance smi) => smi.WantsToPee(), UpdateRate.SIM_200ms);
		this.urgentwant.InitializeStates(this.satisfied).ToggleThought(Db.Get().Thoughts.FullBladder, null).ToggleExpression(Db.Get().Expressions.FullBladder, null).ToggleStateMachine((BladderMonitor.Instance smi) => new PeeChoreMonitor.Instance(smi.master)).ToggleEffect("FullBladder");
		this.breakwant.InitializeStates(this.satisfied);
		this.breakwant.wanting.Transition(this.urgentwant, (BladderMonitor.Instance smi) => smi.NeedsToPee(), UpdateRate.SIM_200ms).EventTransition(GameHashes.ScheduleBlocksChanged, this.satisfied, (BladderMonitor.Instance smi) => !smi.WantsToPee());
		this.breakwant.peeing.ToggleThought(Db.Get().Thoughts.BreakBladder, null);
	}

	// Token: 0x04002E16 RID: 11798
	public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002E17 RID: 11799
	public BladderMonitor.WantsToPeeStates urgentwant;

	// Token: 0x04002E18 RID: 11800
	public BladderMonitor.WantsToPeeStates breakwant;

	// Token: 0x020018F9 RID: 6393
	public class WantsToPeeStates : GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x06009ABD RID: 39613 RVA: 0x0036E180 File Offset: 0x0036C380
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State InitializeStates(GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State donePeeingState)
		{
			base.DefaultState(this.wanting).ToggleUrge(Db.Get().Urges.Pee).ToggleStateMachine((BladderMonitor.Instance smi) => new ToiletMonitor.Instance(smi.master));
			this.wanting.EventTransition(GameHashes.BeginChore, this.peeing, (BladderMonitor.Instance smi) => smi.IsPeeing());
			this.peeing.EventTransition(GameHashes.EndChore, donePeeingState, (BladderMonitor.Instance smi) => !smi.IsPeeing());
			return this;
		}

		// Token: 0x04007807 RID: 30727
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State wanting;

		// Token: 0x04007808 RID: 30728
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State peeing;
	}

	// Token: 0x020018FA RID: 6394
	public new class Instance : GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009ABF RID: 39615 RVA: 0x0036E242 File Offset: 0x0036C442
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.bladder = Db.Get().Amounts.Bladder.Lookup(master.gameObject);
			this.choreDriver = base.GetComponent<ChoreDriver>();
		}

		// Token: 0x06009AC0 RID: 39616 RVA: 0x0036E278 File Offset: 0x0036C478
		public bool NeedsToPee()
		{
			if (base.master.IsNullOrDestroyed())
			{
				return false;
			}
			if (base.master.GetComponent<KPrefabID>().HasTag(GameTags.Asleep))
			{
				return false;
			}
			DebugUtil.DevAssert(this.bladder != null, "bladder is null", null);
			return this.bladder.value >= 100f;
		}

		// Token: 0x06009AC1 RID: 39617 RVA: 0x0036E2D6 File Offset: 0x0036C4D6
		public bool WantsToPee()
		{
			return this.NeedsToPee() || (this.IsPeeTime() && this.bladder.value >= 40f);
		}

		// Token: 0x06009AC2 RID: 39618 RVA: 0x0036E301 File Offset: 0x0036C501
		public bool IsPeeing()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Pee);
		}

		// Token: 0x06009AC3 RID: 39619 RVA: 0x0036E331 File Offset: 0x0036C531
		public bool IsPeeTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Hygiene);
		}

		// Token: 0x04007809 RID: 30729
		private AmountInstance bladder;

		// Token: 0x0400780A RID: 30730
		private ChoreDriver choreDriver;
	}
}
