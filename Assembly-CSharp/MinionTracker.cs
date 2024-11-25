using System;

// Token: 0x020005E5 RID: 1509
public abstract class MinionTracker : Tracker
{
	// Token: 0x06002493 RID: 9363 RVA: 0x000CBA81 File Offset: 0x000C9C81
	public MinionTracker(MinionIdentity identity)
	{
		this.identity = identity;
	}

	// Token: 0x040014B7 RID: 5303
	public MinionIdentity identity;
}
