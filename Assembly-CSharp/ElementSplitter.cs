using System;
using UnityEngine;

// Token: 0x0200055F RID: 1375
public struct ElementSplitter
{
	// Token: 0x06001FD2 RID: 8146 RVA: 0x000B2EE8 File Offset: 0x000B10E8
	public ElementSplitter(GameObject go)
	{
		this.primaryElement = go.GetComponent<PrimaryElement>();
		this.kPrefabID = go.GetComponent<KPrefabID>();
		this.onTakeCB = null;
		this.canAbsorbCB = null;
	}

	// Token: 0x040011F1 RID: 4593
	public PrimaryElement primaryElement;

	// Token: 0x040011F2 RID: 4594
	public Func<Pickupable, float, Pickupable> onTakeCB;

	// Token: 0x040011F3 RID: 4595
	public Func<Pickupable, bool> canAbsorbCB;

	// Token: 0x040011F4 RID: 4596
	public KPrefabID kPrefabID;
}
