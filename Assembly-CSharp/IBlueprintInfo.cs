using System;
using Database;

// Token: 0x02000515 RID: 1301
public interface IBlueprintInfo : IBlueprintDlcInfo
{
	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06001CE7 RID: 7399
	// (set) Token: 0x06001CE8 RID: 7400
	string id { get; set; }

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06001CE9 RID: 7401
	// (set) Token: 0x06001CEA RID: 7402
	string name { get; set; }

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06001CEB RID: 7403
	// (set) Token: 0x06001CEC RID: 7404
	string desc { get; set; }

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06001CED RID: 7405
	// (set) Token: 0x06001CEE RID: 7406
	PermitRarity rarity { get; set; }

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06001CEF RID: 7407
	// (set) Token: 0x06001CF0 RID: 7408
	string animFile { get; set; }
}
