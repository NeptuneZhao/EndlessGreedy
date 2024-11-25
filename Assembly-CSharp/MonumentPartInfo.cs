using System;
using Database;

// Token: 0x0200051C RID: 1308
public class MonumentPartInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06001D3F RID: 7487 RVA: 0x000991CF File Offset: 0x000973CF
	// (set) Token: 0x06001D40 RID: 7488 RVA: 0x000991D7 File Offset: 0x000973D7
	public string id { get; set; }

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06001D41 RID: 7489 RVA: 0x000991E0 File Offset: 0x000973E0
	// (set) Token: 0x06001D42 RID: 7490 RVA: 0x000991E8 File Offset: 0x000973E8
	public string name { get; set; }

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06001D43 RID: 7491 RVA: 0x000991F1 File Offset: 0x000973F1
	// (set) Token: 0x06001D44 RID: 7492 RVA: 0x000991F9 File Offset: 0x000973F9
	public string desc { get; set; }

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06001D45 RID: 7493 RVA: 0x00099202 File Offset: 0x00097402
	// (set) Token: 0x06001D46 RID: 7494 RVA: 0x0009920A File Offset: 0x0009740A
	public PermitRarity rarity { get; set; }

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06001D47 RID: 7495 RVA: 0x00099213 File Offset: 0x00097413
	// (set) Token: 0x06001D48 RID: 7496 RVA: 0x0009921B File Offset: 0x0009741B
	public string animFile { get; set; }

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06001D49 RID: 7497 RVA: 0x00099224 File Offset: 0x00097424
	// (set) Token: 0x06001D4A RID: 7498 RVA: 0x0009922C File Offset: 0x0009742C
	public string[] dlcIds { get; set; }

	// Token: 0x06001D4B RID: 7499 RVA: 0x00099238 File Offset: 0x00097438
	public MonumentPartInfo(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] dlcIds)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFilename;
		this.state = state;
		this.symbolName = symbolName;
		this.part = part;
		this.dlcIds = dlcIds;
	}

	// Token: 0x04001088 RID: 4232
	public string state;

	// Token: 0x04001089 RID: 4233
	public string symbolName;

	// Token: 0x0400108A RID: 4234
	public MonumentPartResource.Part part;
}
