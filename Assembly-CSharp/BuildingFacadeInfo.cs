using System;
using System.Collections.Generic;
using Database;

// Token: 0x02000518 RID: 1304
public class BuildingFacadeInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06001D0B RID: 7435 RVA: 0x00098F12 File Offset: 0x00097112
	// (set) Token: 0x06001D0C RID: 7436 RVA: 0x00098F1A File Offset: 0x0009711A
	public string id { get; set; }

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06001D0D RID: 7437 RVA: 0x00098F23 File Offset: 0x00097123
	// (set) Token: 0x06001D0E RID: 7438 RVA: 0x00098F2B File Offset: 0x0009712B
	public string name { get; set; }

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06001D0F RID: 7439 RVA: 0x00098F34 File Offset: 0x00097134
	// (set) Token: 0x06001D10 RID: 7440 RVA: 0x00098F3C File Offset: 0x0009713C
	public string desc { get; set; }

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06001D11 RID: 7441 RVA: 0x00098F45 File Offset: 0x00097145
	// (set) Token: 0x06001D12 RID: 7442 RVA: 0x00098F4D File Offset: 0x0009714D
	public PermitRarity rarity { get; set; }

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06001D13 RID: 7443 RVA: 0x00098F56 File Offset: 0x00097156
	// (set) Token: 0x06001D14 RID: 7444 RVA: 0x00098F5E File Offset: 0x0009715E
	public string animFile { get; set; }

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06001D15 RID: 7445 RVA: 0x00098F67 File Offset: 0x00097167
	// (set) Token: 0x06001D16 RID: 7446 RVA: 0x00098F6F File Offset: 0x0009716F
	public string[] dlcIds { get; set; }

	// Token: 0x06001D17 RID: 7447 RVA: 0x00098F78 File Offset: 0x00097178
	public BuildingFacadeInfo(string id, string name, string desc, PermitRarity rarity, string prefabId, string animFile, string[] dlcIds, Dictionary<string, string> workables = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.prefabId = prefabId;
		this.animFile = animFile;
		this.workables = workables;
		this.dlcIds = dlcIds;
	}

	// Token: 0x04001069 RID: 4201
	public string prefabId;

	// Token: 0x0400106B RID: 4203
	public Dictionary<string, string> workables;
}
