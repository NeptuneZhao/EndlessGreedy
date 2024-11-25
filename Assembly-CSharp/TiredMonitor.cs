using System;

// Token: 0x020009AA RID: 2474
public class TiredMonitor : GameStateMachine<TiredMonitor, TiredMonitor.Instance>
{
	// Token: 0x060047FC RID: 18428 RVA: 0x0019C5F0 File Offset: 0x0019A7F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventTransition(GameHashes.SleepFail, this.tired, null);
		this.tired.Enter(delegate(TiredMonitor.Instance smi)
		{
			smi.SetInterruptDay();
		}).EventTransition(GameHashes.NewDay, (TiredMonitor.Instance smi) => GameClock.Instance, this.root, (TiredMonitor.Instance smi) => smi.AllowInterruptClear()).ToggleExpression(Db.Get().Expressions.Tired, null).ToggleAnims("anim_loco_walk_slouch_kanim", 0f).ToggleAnims("anim_idle_slouch_kanim", 0f);
	}

	// Token: 0x04002F20 RID: 12064
	public GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.State tired;

	// Token: 0x020019A0 RID: 6560
	public new class Instance : GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009D71 RID: 40305 RVA: 0x00375192 File Offset: 0x00373392
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009D72 RID: 40306 RVA: 0x003751A9 File Offset: 0x003733A9
		public void SetInterruptDay()
		{
			this.interruptedDay = GameClock.Instance.GetCycle();
		}

		// Token: 0x06009D73 RID: 40307 RVA: 0x003751BB File Offset: 0x003733BB
		public bool AllowInterruptClear()
		{
			bool flag = GameClock.Instance.GetCycle() > this.interruptedDay + 1;
			if (flag)
			{
				this.interruptedDay = -1;
			}
			return flag;
		}

		// Token: 0x04007A21 RID: 31265
		public int disturbedDay = -1;

		// Token: 0x04007A22 RID: 31266
		public int interruptedDay = -1;
	}
}
