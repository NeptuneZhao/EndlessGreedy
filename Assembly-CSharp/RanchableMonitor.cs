using System;

// Token: 0x02000553 RID: 1363
public class RanchableMonitor : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>
{
	// Token: 0x06001F47 RID: 8007 RVA: 0x000AF55F File Offset: 0x000AD75F
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetRanched, (RanchableMonitor.Instance smi) => smi.ShouldGoGetRanched(), null);
	}

	// Token: 0x02001346 RID: 4934
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001347 RID: 4935
	public new class Instance : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>.GameInstance
	{
		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06008672 RID: 34418 RVA: 0x003292A7 File Offset: 0x003274A7
		// (set) Token: 0x06008673 RID: 34419 RVA: 0x003292AF File Offset: 0x003274AF
		public ChoreConsumer ChoreConsumer { get; private set; }

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06008674 RID: 34420 RVA: 0x003292B8 File Offset: 0x003274B8
		public Navigator NavComponent
		{
			get
			{
				return this.navComponent;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06008675 RID: 34421 RVA: 0x003292C0 File Offset: 0x003274C0
		public RanchedStates.Instance States
		{
			get
			{
				if (this.states == null)
				{
					this.states = this.controller.GetSMI<RanchedStates.Instance>();
				}
				return this.states;
			}
		}

		// Token: 0x06008676 RID: 34422 RVA: 0x003292E1 File Offset: 0x003274E1
		public Instance(IStateMachineTarget master, RanchableMonitor.Def def) : base(master, def)
		{
			this.ChoreConsumer = base.GetComponent<ChoreConsumer>();
			this.navComponent = base.GetComponent<Navigator>();
		}

		// Token: 0x06008677 RID: 34423 RVA: 0x00329303 File Offset: 0x00327503
		public bool ShouldGoGetRanched()
		{
			return this.TargetRanchStation != null && this.TargetRanchStation.IsRunning() && this.TargetRanchStation.IsRancherReady;
		}

		// Token: 0x0400661E RID: 26142
		public RanchStation.Instance TargetRanchStation;

		// Token: 0x0400661F RID: 26143
		private Navigator navComponent;

		// Token: 0x04006620 RID: 26144
		private RanchedStates.Instance states;
	}
}
