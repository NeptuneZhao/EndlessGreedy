using System;
using UnityEngine;

// Token: 0x02000527 RID: 1319
public interface IApproachable
{
	// Token: 0x06001DAB RID: 7595
	CellOffset[] GetOffsets();

	// Token: 0x06001DAC RID: 7596
	int GetCell();

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06001DAD RID: 7597
	Transform transform { get; }
}
