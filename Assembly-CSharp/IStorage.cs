using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008F5 RID: 2293
public interface IStorage
{
	// Token: 0x060041E4 RID: 16868
	bool ShouldShowInUI();

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x060041E5 RID: 16869
	// (set) Token: 0x060041E6 RID: 16870
	bool allowUIItemRemoval { get; set; }

	// Token: 0x060041E7 RID: 16871
	GameObject Drop(GameObject go, bool do_disease_transfer = true);

	// Token: 0x060041E8 RID: 16872
	List<GameObject> GetItems();

	// Token: 0x060041E9 RID: 16873
	bool IsFull();

	// Token: 0x060041EA RID: 16874
	bool IsEmpty();

	// Token: 0x060041EB RID: 16875
	float Capacity();

	// Token: 0x060041EC RID: 16876
	float RemainingCapacity();

	// Token: 0x060041ED RID: 16877
	float GetAmountAvailable(Tag tag);

	// Token: 0x060041EE RID: 16878
	void ConsumeIgnoringDisease(Tag tag, float amount);
}
