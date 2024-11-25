using System;

// Token: 0x020007D7 RID: 2007
public interface IConduitConsumer
{
	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x06003753 RID: 14163
	Storage Storage { get; }

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06003754 RID: 14164
	ConduitType ConduitType { get; }
}
