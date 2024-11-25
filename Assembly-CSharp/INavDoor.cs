using System;

// Token: 0x020006BF RID: 1727
public interface INavDoor
{
	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06002B8C RID: 11148
	bool isSpawned { get; }

	// Token: 0x06002B8D RID: 11149
	bool IsOpen();

	// Token: 0x06002B8E RID: 11150
	void Open();

	// Token: 0x06002B8F RID: 11151
	void Close();
}
