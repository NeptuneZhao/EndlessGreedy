using System;
using System.Collections.Generic;

// Token: 0x020004D1 RID: 1233
internal class StateMachineManagerAsyncLoader : GlobalAsyncLoader<StateMachineManagerAsyncLoader>
{
	// Token: 0x06001A97 RID: 6807 RVA: 0x0008C0AA File Offset: 0x0008A2AA
	public override void Run()
	{
	}

	// Token: 0x04000F14 RID: 3860
	public List<StateMachine> stateMachines = new List<StateMachine>();
}
