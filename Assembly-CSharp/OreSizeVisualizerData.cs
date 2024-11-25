using System;
using UnityEngine;

// Token: 0x020009D9 RID: 2521
public struct OreSizeVisualizerData
{
	// Token: 0x06004934 RID: 18740 RVA: 0x001A3788 File Offset: 0x001A1988
	public OreSizeVisualizerData(GameObject go)
	{
		this.primaryElement = go.GetComponent<PrimaryElement>();
		this.onMassChangedCB = null;
	}

	// Token: 0x04002FE7 RID: 12263
	public PrimaryElement primaryElement;

	// Token: 0x04002FE8 RID: 12264
	public Action<object> onMassChangedCB;
}
