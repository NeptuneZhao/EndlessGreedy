using System;

// Token: 0x0200097D RID: 2429
public class EmoteHighPriorityMonitor : GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance>
{
	// Token: 0x06004705 RID: 18181 RVA: 0x00196364 File Offset: 0x00194564
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.ready;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.ready.ToggleUrge(Db.Get().Urges.EmoteHighPriority).EventHandler(GameHashes.BeginChore, delegate(EmoteHighPriorityMonitor.Instance smi, object o)
		{
			smi.OnStartChore(o);
		});
		this.resetting.GoTo(this.ready);
	}

	// Token: 0x04002E48 RID: 11848
	public GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.State ready;

	// Token: 0x04002E49 RID: 11849
	public GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.State resetting;

	// Token: 0x02001927 RID: 6439
	public new class Instance : GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009B63 RID: 39779 RVA: 0x0036F740 File Offset: 0x0036D940
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009B64 RID: 39780 RVA: 0x0036F749 File Offset: 0x0036D949
		public void OnStartChore(object o)
		{
			if (((Chore)o).SatisfiesUrge(Db.Get().Urges.EmoteHighPriority))
			{
				this.GoTo(base.sm.resetting);
			}
		}
	}
}
