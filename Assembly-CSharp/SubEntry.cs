using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000BF6 RID: 3062
public class SubEntry
{
	// Token: 0x06005D70 RID: 23920 RVA: 0x00226DFE File Offset: 0x00224FFE
	public SubEntry()
	{
	}

	// Token: 0x06005D71 RID: 23921 RVA: 0x00226E14 File Offset: 0x00225014
	public SubEntry(string id, string parentEntryID, List<ContentContainer> contentContainers, string name)
	{
		this.id = id;
		this.parentEntryID = parentEntryID;
		this.name = name;
		this.contentContainers = contentContainers;
		if (!string.IsNullOrEmpty(this.lockID))
		{
			foreach (ContentContainer contentContainer in contentContainers)
			{
				contentContainer.lockID = this.lockID;
			}
		}
		if (string.IsNullOrEmpty(this.sortString))
		{
			if (!string.IsNullOrEmpty(this.title))
			{
				this.sortString = UI.StripLinkFormatting(this.title);
				return;
			}
			this.sortString = UI.StripLinkFormatting(name);
		}
	}

	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x06005D72 RID: 23922 RVA: 0x00226EDC File Offset: 0x002250DC
	// (set) Token: 0x06005D73 RID: 23923 RVA: 0x00226EE4 File Offset: 0x002250E4
	public List<ContentContainer> contentContainers { get; set; }

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x06005D74 RID: 23924 RVA: 0x00226EED File Offset: 0x002250ED
	// (set) Token: 0x06005D75 RID: 23925 RVA: 0x00226EF5 File Offset: 0x002250F5
	public string parentEntryID { get; set; }

	// Token: 0x170006D4 RID: 1748
	// (get) Token: 0x06005D76 RID: 23926 RVA: 0x00226EFE File Offset: 0x002250FE
	// (set) Token: 0x06005D77 RID: 23927 RVA: 0x00226F06 File Offset: 0x00225106
	public string id { get; set; }

	// Token: 0x170006D5 RID: 1749
	// (get) Token: 0x06005D78 RID: 23928 RVA: 0x00226F0F File Offset: 0x0022510F
	// (set) Token: 0x06005D79 RID: 23929 RVA: 0x00226F17 File Offset: 0x00225117
	public string name { get; set; }

	// Token: 0x170006D6 RID: 1750
	// (get) Token: 0x06005D7A RID: 23930 RVA: 0x00226F20 File Offset: 0x00225120
	// (set) Token: 0x06005D7B RID: 23931 RVA: 0x00226F28 File Offset: 0x00225128
	public string title { get; set; }

	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x06005D7C RID: 23932 RVA: 0x00226F31 File Offset: 0x00225131
	// (set) Token: 0x06005D7D RID: 23933 RVA: 0x00226F39 File Offset: 0x00225139
	public string subtitle { get; set; }

	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x06005D7E RID: 23934 RVA: 0x00226F42 File Offset: 0x00225142
	// (set) Token: 0x06005D7F RID: 23935 RVA: 0x00226F4A File Offset: 0x0022514A
	public Sprite icon { get; set; }

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x06005D80 RID: 23936 RVA: 0x00226F53 File Offset: 0x00225153
	// (set) Token: 0x06005D81 RID: 23937 RVA: 0x00226F5B File Offset: 0x0022515B
	public int layoutPriority { get; set; }

	// Token: 0x170006DA RID: 1754
	// (get) Token: 0x06005D82 RID: 23938 RVA: 0x00226F64 File Offset: 0x00225164
	// (set) Token: 0x06005D83 RID: 23939 RVA: 0x00226F6C File Offset: 0x0022516C
	public bool disabled { get; set; }

	// Token: 0x170006DB RID: 1755
	// (get) Token: 0x06005D84 RID: 23940 RVA: 0x00226F75 File Offset: 0x00225175
	// (set) Token: 0x06005D85 RID: 23941 RVA: 0x00226F7D File Offset: 0x0022517D
	public string lockID { get; set; }

	// Token: 0x170006DC RID: 1756
	// (get) Token: 0x06005D86 RID: 23942 RVA: 0x00226F86 File Offset: 0x00225186
	// (set) Token: 0x06005D87 RID: 23943 RVA: 0x00226F8E File Offset: 0x0022518E
	public string[] dlcIds { get; set; }

	// Token: 0x170006DD RID: 1757
	// (get) Token: 0x06005D88 RID: 23944 RVA: 0x00226F97 File Offset: 0x00225197
	// (set) Token: 0x06005D89 RID: 23945 RVA: 0x00226F9F File Offset: 0x0022519F
	public string[] forbiddenDLCIds { get; set; }

	// Token: 0x06005D8A RID: 23946 RVA: 0x00226FA8 File Offset: 0x002251A8
	public string[] GetDlcIds()
	{
		return this.dlcIds;
	}

	// Token: 0x06005D8B RID: 23947 RVA: 0x00226FB0 File Offset: 0x002251B0
	public string[] GetForbiddenDlCIds()
	{
		return this.forbiddenDLCIds;
	}

	// Token: 0x170006DE RID: 1758
	// (get) Token: 0x06005D8C RID: 23948 RVA: 0x00226FB8 File Offset: 0x002251B8
	// (set) Token: 0x06005D8D RID: 23949 RVA: 0x00226FC0 File Offset: 0x002251C0
	public string sortString { get; set; }

	// Token: 0x170006DF RID: 1759
	// (get) Token: 0x06005D8E RID: 23950 RVA: 0x00226FC9 File Offset: 0x002251C9
	// (set) Token: 0x06005D8F RID: 23951 RVA: 0x00226FD1 File Offset: 0x002251D1
	public bool showBeforeGeneratedCategoryLinks { get; set; }

	// Token: 0x04003E9E RID: 16030
	public ContentContainer lockedContentContainer;

	// Token: 0x04003EA5 RID: 16037
	public Color iconColor = Color.white;
}
