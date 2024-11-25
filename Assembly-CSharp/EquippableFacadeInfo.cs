using System;
using Database;

// Token: 0x0200051B RID: 1307
public class EquippableFacadeInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06001D32 RID: 7474 RVA: 0x00099114 File Offset: 0x00097314
	// (set) Token: 0x06001D33 RID: 7475 RVA: 0x0009911C File Offset: 0x0009731C
	public string id { get; set; }

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06001D34 RID: 7476 RVA: 0x00099125 File Offset: 0x00097325
	// (set) Token: 0x06001D35 RID: 7477 RVA: 0x0009912D File Offset: 0x0009732D
	public string name { get; set; }

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06001D36 RID: 7478 RVA: 0x00099136 File Offset: 0x00097336
	// (set) Token: 0x06001D37 RID: 7479 RVA: 0x0009913E File Offset: 0x0009733E
	public string desc { get; set; }

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06001D38 RID: 7480 RVA: 0x00099147 File Offset: 0x00097347
	// (set) Token: 0x06001D39 RID: 7481 RVA: 0x0009914F File Offset: 0x0009734F
	public PermitRarity rarity { get; set; }

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06001D3A RID: 7482 RVA: 0x00099158 File Offset: 0x00097358
	// (set) Token: 0x06001D3B RID: 7483 RVA: 0x00099160 File Offset: 0x00097360
	public string animFile { get; set; }

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06001D3C RID: 7484 RVA: 0x00099169 File Offset: 0x00097369
	// (set) Token: 0x06001D3D RID: 7485 RVA: 0x00099171 File Offset: 0x00097371
	public string[] dlcIds { get; set; }

	// Token: 0x06001D3E RID: 7486 RVA: 0x0009917C File Offset: 0x0009737C
	public EquippableFacadeInfo(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.defID = defID;
		this.buildOverride = buildOverride;
		this.animFile = animFile;
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0400107F RID: 4223
	public string buildOverride;

	// Token: 0x04001080 RID: 4224
	public string defID;
}
