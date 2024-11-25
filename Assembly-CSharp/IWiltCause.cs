using System;

// Token: 0x0200082B RID: 2091
public interface IWiltCause
{
	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x060039DF RID: 14815
	string WiltStateString { get; }

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x060039E0 RID: 14816
	WiltCondition.Condition[] Conditions { get; }
}
