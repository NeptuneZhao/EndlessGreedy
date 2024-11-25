using System;

// Token: 0x02000478 RID: 1144
public class GameplayEventPrecondition
{
	// Token: 0x04000DB8 RID: 3512
	public string description;

	// Token: 0x04000DB9 RID: 3513
	public GameplayEventPrecondition.PreconditionFn condition;

	// Token: 0x04000DBA RID: 3514
	public bool required;

	// Token: 0x04000DBB RID: 3515
	public int priorityModifier;

	// Token: 0x02001237 RID: 4663
	// (Invoke) Token: 0x0600827B RID: 33403
	public delegate bool PreconditionFn();
}
