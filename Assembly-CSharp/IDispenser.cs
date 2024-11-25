using System;
using System.Collections.Generic;

// Token: 0x02000D64 RID: 3428
public interface IDispenser
{
	// Token: 0x06006BF9 RID: 27641
	List<Tag> DispensedItems();

	// Token: 0x06006BFA RID: 27642
	Tag SelectedItem();

	// Token: 0x06006BFB RID: 27643
	void SelectItem(Tag tag);

	// Token: 0x06006BFC RID: 27644
	void OnOrderDispense();

	// Token: 0x06006BFD RID: 27645
	void OnCancelDispense();

	// Token: 0x06006BFE RID: 27646
	bool HasOpenChore();

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x06006BFF RID: 27647
	// (remove) Token: 0x06006C00 RID: 27648
	event System.Action OnStopWorkEvent;
}
