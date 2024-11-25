using System;

// Token: 0x0200097B RID: 2427
public class DoctorMonitor : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance>
{
	// Token: 0x06004701 RID: 18177 RVA: 0x0019627B File Offset: 0x0019447B
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.ToggleUrge(Db.Get().Urges.Doctor);
	}

	// Token: 0x02001924 RID: 6436
	public new class Instance : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009B5C RID: 39772 RVA: 0x0036F6DE File Offset: 0x0036D8DE
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
