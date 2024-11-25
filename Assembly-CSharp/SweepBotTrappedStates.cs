using System;

// Token: 0x02000385 RID: 901
public class SweepBotTrappedStates : GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>
{
	// Token: 0x060012B0 RID: 4784 RVA: 0x00066908 File Offset: 0x00064B08
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.blockedStates.evaluating;
		this.blockedStates.ToggleStatusItem(Db.Get().RobotStatusItems.CantReachStation, (SweepBotTrappedStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).TagTransition(GameTags.Robots.Behaviours.TrappedBehaviour, this.behaviourcomplete, true);
		this.blockedStates.evaluating.Enter(delegate(SweepBotTrappedStates.Instance smi)
		{
			if (smi.sm.GetSweepLocker(smi) == null)
			{
				smi.GoTo(this.blockedStates.noHome);
				return;
			}
			smi.GoTo(this.blockedStates.blocked);
		});
		this.blockedStates.blocked.ToggleChore((SweepBotTrappedStates.Instance smi) => new RescueSweepBotChore(smi.master, smi.master.gameObject, smi.sm.GetSweepLocker(smi).gameObject), this.behaviourcomplete, this.blockedStates.evaluating).PlayAnim("react_stuck", KAnim.PlayMode.Loop);
		this.blockedStates.noHome.PlayAnim("react_stuck", KAnim.PlayMode.Once).OnAnimQueueComplete(this.blockedStates.evaluating);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.TrappedBehaviour, false);
	}

	// Token: 0x060012B1 RID: 4785 RVA: 0x00066A20 File Offset: 0x00064C20
	public Storage GetSweepLocker(SweepBotTrappedStates.Instance smi)
	{
		StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
		if (smi2 == null)
		{
			return null;
		}
		return smi2.sm.sweepLocker.Get(smi2);
	}

	// Token: 0x04000AD7 RID: 2775
	public SweepBotTrappedStates.BlockedStates blockedStates;

	// Token: 0x04000AD8 RID: 2776
	public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State behaviourcomplete;

	// Token: 0x02001142 RID: 4418
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001143 RID: 4419
	public new class Instance : GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.GameInstance
	{
		// Token: 0x06007EFE RID: 32510 RVA: 0x0030BBA2 File Offset: 0x00309DA2
		public Instance(Chore<SweepBotTrappedStates.Instance> chore, SweepBotTrappedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.TrappedBehaviour);
		}
	}

	// Token: 0x02001144 RID: 4420
	public class BlockedStates : GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State
	{
		// Token: 0x04005F9D RID: 24477
		public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State evaluating;

		// Token: 0x04005F9E RID: 24478
		public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State blocked;

		// Token: 0x04005F9F RID: 24479
		public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State noHome;
	}
}
