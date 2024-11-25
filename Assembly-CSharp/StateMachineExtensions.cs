using System;

// Token: 0x020004CB RID: 1227
public static class StateMachineExtensions
{
	// Token: 0x06001A69 RID: 6761 RVA: 0x0008B824 File Offset: 0x00089A24
	public static bool IsNullOrStopped(this StateMachine.Instance smi)
	{
		return smi == null || !smi.IsRunning();
	}
}
