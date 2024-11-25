using System;

// Token: 0x0200079C RID: 1948
public interface IDisconnectable
{
	// Token: 0x0600354C RID: 13644
	bool Connect();

	// Token: 0x0600354D RID: 13645
	void Disconnect();

	// Token: 0x0600354E RID: 13646
	bool IsDisconnected();
}
