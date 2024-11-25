using System;
using UnityEngine;

// Token: 0x020005B1 RID: 1457
public interface IRottable
{
	// Token: 0x1700018F RID: 399
	// (get) Token: 0x060022C2 RID: 8898
	GameObject gameObject { get; }

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x060022C3 RID: 8899
	float RotTemperature { get; }

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x060022C4 RID: 8900
	float PreserveTemperature { get; }
}
