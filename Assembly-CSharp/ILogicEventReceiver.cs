using System;

// Token: 0x02000949 RID: 2377
public interface ILogicEventReceiver : ILogicNetworkConnection
{
	// Token: 0x0600453D RID: 17725
	void ReceiveLogicEvent(int value);

	// Token: 0x0600453E RID: 17726
	int GetLogicCell();
}
