using System;

// Token: 0x020007B0 RID: 1968
public interface ICircuitConnected
{
	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x060035CE RID: 13774
	bool IsVirtual { get; }

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x060035CF RID: 13775
	int PowerCell { get; }

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x060035D0 RID: 13776
	object VirtualCircuitKey { get; }
}
