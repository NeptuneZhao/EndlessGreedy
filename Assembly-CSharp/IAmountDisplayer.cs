using System;
using Klei.AI;

// Token: 0x02000BCA RID: 3018
public interface IAmountDisplayer
{
	// Token: 0x06005C1C RID: 23580
	string GetValueString(Amount master, AmountInstance instance);

	// Token: 0x06005C1D RID: 23581
	string GetDescription(Amount master, AmountInstance instance);

	// Token: 0x06005C1E RID: 23582
	string GetTooltip(Amount master, AmountInstance instance);

	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x06005C1F RID: 23583
	IAttributeFormatter Formatter { get; }
}
