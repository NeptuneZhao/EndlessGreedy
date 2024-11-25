using System;

// Token: 0x020008CC RID: 2252
public class SafeCellMonitor : GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance>
{
	// Token: 0x06003FFE RID: 16382 RVA: 0x0016AB48 File Offset: 0x00168D48
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.ToggleUrge(Db.Get().Urges.MoveToSafety);
		this.satisfied.EventTransition(GameHashes.SafeCellDetected, this.danger, (SafeCellMonitor.Instance smi) => smi.IsAreaUnsafe());
		this.danger.EventTransition(GameHashes.SafeCellLost, this.satisfied, (SafeCellMonitor.Instance smi) => !smi.IsAreaUnsafe()).ToggleChore((SafeCellMonitor.Instance smi) => new MoveToSafetyChore(smi.master), this.satisfied);
	}

	// Token: 0x04002A48 RID: 10824
	public GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002A49 RID: 10825
	public GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, object>.State danger;

	// Token: 0x02001802 RID: 6146
	public new class Instance : GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600972D RID: 38701 RVA: 0x0036464A File Offset: 0x0036284A
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.safeCellSensor = base.GetComponent<Sensors>().GetSensor<SafeCellSensor>();
		}

		// Token: 0x0600972E RID: 38702 RVA: 0x00364664 File Offset: 0x00362864
		public bool IsAreaUnsafe()
		{
			return this.safeCellSensor.HasSafeCell();
		}

		// Token: 0x040074B3 RID: 29875
		private SafeCellSensor safeCellSensor;
	}
}
