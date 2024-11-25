using System;

// Token: 0x02000A1C RID: 2588
public interface IRemoteDockWorkTarget
{
	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x06004B1C RID: 19228
	Chore RemoteDockChore { get; }

	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x06004B1D RID: 19229
	IApproachable Approachable { get; }
}
