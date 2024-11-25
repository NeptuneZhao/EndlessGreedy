using System;

// Token: 0x02000994 RID: 2452
public class QuarantineFeedableMonitor : GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance>
{
	// Token: 0x0600478A RID: 18314 RVA: 0x001990F4 File Offset: 0x001972F4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.EventTransition(GameHashes.AddUrge, this.hungry, (QuarantineFeedableMonitor.Instance smi) => smi.IsHungry());
		this.hungry.EventTransition(GameHashes.RemoveUrge, this.satisfied, (QuarantineFeedableMonitor.Instance smi) => !smi.IsHungry());
	}

	// Token: 0x04002EAF RID: 11951
	public GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002EB0 RID: 11952
	public GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.State hungry;

	// Token: 0x02001962 RID: 6498
	public new class Instance : GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009C55 RID: 40021 RVA: 0x00372136 File Offset: 0x00370336
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009C56 RID: 40022 RVA: 0x0037213F File Offset: 0x0037033F
		public bool IsHungry()
		{
			return base.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Eat);
		}
	}
}
