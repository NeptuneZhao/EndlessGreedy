using System;
using System.Collections.Generic;

// Token: 0x02000BF7 RID: 3063
public class CategoryEntry : CodexEntry
{
	// Token: 0x170006E0 RID: 1760
	// (get) Token: 0x06005D90 RID: 23952 RVA: 0x00226FDA File Offset: 0x002251DA
	// (set) Token: 0x06005D91 RID: 23953 RVA: 0x00226FE2 File Offset: 0x002251E2
	public bool largeFormat { get; set; }

	// Token: 0x170006E1 RID: 1761
	// (get) Token: 0x06005D92 RID: 23954 RVA: 0x00226FEB File Offset: 0x002251EB
	// (set) Token: 0x06005D93 RID: 23955 RVA: 0x00226FF3 File Offset: 0x002251F3
	public bool sort { get; set; }

	// Token: 0x06005D94 RID: 23956 RVA: 0x00226FFC File Offset: 0x002251FC
	public CategoryEntry(string category, List<ContentContainer> contentContainers, string name, List<CodexEntry> entriesInCategory, bool largeFormat, bool sort) : base(category, contentContainers, name)
	{
		this.entriesInCategory = entriesInCategory;
		this.largeFormat = largeFormat;
		this.sort = sort;
	}

	// Token: 0x04003EAF RID: 16047
	public List<CodexEntry> entriesInCategory = new List<CodexEntry>();
}
