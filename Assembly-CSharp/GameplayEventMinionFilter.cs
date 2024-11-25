using System;

// Token: 0x02000476 RID: 1142
public class GameplayEventMinionFilter
{
	// Token: 0x04000DB5 RID: 3509
	public string id;

	// Token: 0x04000DB6 RID: 3510
	public GameplayEventMinionFilter.FilterFn filter;

	// Token: 0x0200122F RID: 4655
	// (Invoke) Token: 0x06008268 RID: 33384
	public delegate bool FilterFn(MinionIdentity minion);
}
