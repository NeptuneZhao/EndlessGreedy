using System;
using Database;

// Token: 0x0200051A RID: 1306
public class StickerBombFacadeInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06001D25 RID: 7461 RVA: 0x0009906E File Offset: 0x0009726E
	// (set) Token: 0x06001D26 RID: 7462 RVA: 0x00099076 File Offset: 0x00097276
	public string id { get; set; }

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06001D27 RID: 7463 RVA: 0x0009907F File Offset: 0x0009727F
	// (set) Token: 0x06001D28 RID: 7464 RVA: 0x00099087 File Offset: 0x00097287
	public string name { get; set; }

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06001D29 RID: 7465 RVA: 0x00099090 File Offset: 0x00097290
	// (set) Token: 0x06001D2A RID: 7466 RVA: 0x00099098 File Offset: 0x00097298
	public string desc { get; set; }

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06001D2B RID: 7467 RVA: 0x000990A1 File Offset: 0x000972A1
	// (set) Token: 0x06001D2C RID: 7468 RVA: 0x000990A9 File Offset: 0x000972A9
	public PermitRarity rarity { get; set; }

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06001D2D RID: 7469 RVA: 0x000990B2 File Offset: 0x000972B2
	// (set) Token: 0x06001D2E RID: 7470 RVA: 0x000990BA File Offset: 0x000972BA
	public string animFile { get; set; }

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06001D2F RID: 7471 RVA: 0x000990C3 File Offset: 0x000972C3
	// (set) Token: 0x06001D30 RID: 7472 RVA: 0x000990CB File Offset: 0x000972CB
	public string[] dlcIds { get; set; }

	// Token: 0x06001D31 RID: 7473 RVA: 0x000990D4 File Offset: 0x000972D4
	public StickerBombFacadeInfo(string id, string name, string desc, PermitRarity rarity, string animFile, string sticker)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFile;
		this.sticker = sticker;
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x04001079 RID: 4217
	public string sticker;
}
