using System;
using System.Collections.Generic;

// Token: 0x02000D83 RID: 3459
public interface INToggleSideScreenControl
{
	// Token: 0x170007A0 RID: 1952
	// (get) Token: 0x06006CD3 RID: 27859
	string SidescreenTitleKey { get; }

	// Token: 0x170007A1 RID: 1953
	// (get) Token: 0x06006CD4 RID: 27860
	List<LocString> Options { get; }

	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x06006CD5 RID: 27861
	List<LocString> Tooltips { get; }

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x06006CD6 RID: 27862
	string Description { get; }

	// Token: 0x170007A4 RID: 1956
	// (get) Token: 0x06006CD7 RID: 27863
	int SelectedOption { get; }

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x06006CD8 RID: 27864
	int QueuedOption { get; }

	// Token: 0x06006CD9 RID: 27865
	void QueueSelectedOption(int option);
}
