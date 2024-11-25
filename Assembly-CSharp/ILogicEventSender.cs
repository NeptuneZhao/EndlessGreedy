using System;

// Token: 0x02000948 RID: 2376
public interface ILogicEventSender : ILogicNetworkConnection
{
	// Token: 0x0600453A RID: 17722
	void LogicTick();

	// Token: 0x0600453B RID: 17723
	int GetLogicCell();

	// Token: 0x0600453C RID: 17724
	int GetLogicValue();
}
