using System;

// Token: 0x02000555 RID: 1365
public class WorldSpawnableMonitor : GameStateMachine<WorldSpawnableMonitor, WorldSpawnableMonitor.Instance, IStateMachineTarget, WorldSpawnableMonitor.Def>
{
	// Token: 0x06001F4F RID: 8015 RVA: 0x000AFBBB File Offset: 0x000ADDBB
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
	}

	// Token: 0x0200134C RID: 4940
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400662E RID: 26158
		public Func<int, int> adjustSpawnLocationCb;
	}

	// Token: 0x0200134D RID: 4941
	public new class Instance : GameStateMachine<WorldSpawnableMonitor, WorldSpawnableMonitor.Instance, IStateMachineTarget, WorldSpawnableMonitor.Def>.GameInstance
	{
		// Token: 0x0600868C RID: 34444 RVA: 0x003296E6 File Offset: 0x003278E6
		public Instance(IStateMachineTarget master, WorldSpawnableMonitor.Def def) : base(master, def)
		{
		}
	}
}
