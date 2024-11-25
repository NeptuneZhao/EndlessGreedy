using System;
using UnityEngine;

// Token: 0x020008F7 RID: 2295
public interface ITelepadDeliverableContainer
{
	// Token: 0x060041F0 RID: 16880
	void SelectDeliverable();

	// Token: 0x060041F1 RID: 16881
	void DeselectDeliverable();

	// Token: 0x060041F2 RID: 16882
	GameObject GetGameObject();
}
