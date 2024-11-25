using System;
using Database;

// Token: 0x02000516 RID: 1302
public class ArtableInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x00098D47 File Offset: 0x00096F47
	// (set) Token: 0x06001CF2 RID: 7410 RVA: 0x00098D4F File Offset: 0x00096F4F
	public string id { get; set; }

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x00098D58 File Offset: 0x00096F58
	// (set) Token: 0x06001CF4 RID: 7412 RVA: 0x00098D60 File Offset: 0x00096F60
	public string name { get; set; }

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x00098D69 File Offset: 0x00096F69
	// (set) Token: 0x06001CF6 RID: 7414 RVA: 0x00098D71 File Offset: 0x00096F71
	public string desc { get; set; }

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x00098D7A File Offset: 0x00096F7A
	// (set) Token: 0x06001CF8 RID: 7416 RVA: 0x00098D82 File Offset: 0x00096F82
	public PermitRarity rarity { get; set; }

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x00098D8B File Offset: 0x00096F8B
	// (set) Token: 0x06001CFA RID: 7418 RVA: 0x00098D93 File Offset: 0x00096F93
	public string animFile { get; set; }

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06001CFB RID: 7419 RVA: 0x00098D9C File Offset: 0x00096F9C
	// (set) Token: 0x06001CFC RID: 7420 RVA: 0x00098DA4 File Offset: 0x00096FA4
	public string[] dlcIds { get; set; }

	// Token: 0x06001CFD RID: 7421 RVA: 0x00098DB0 File Offset: 0x00096FB0
	public ArtableInfo(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, string status_id, string prefabId, string symbolname = "")
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFile;
		this.anim = anim;
		this.decor_value = decor_value;
		this.cheer_on_complete = cheer_on_complete;
		this.status_id = status_id;
		this.prefabId = prefabId;
		this.symbolname = symbolname;
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x04001056 RID: 4182
	public string anim;

	// Token: 0x04001057 RID: 4183
	public int decor_value;

	// Token: 0x04001058 RID: 4184
	public bool cheer_on_complete;

	// Token: 0x04001059 RID: 4185
	public string status_id;

	// Token: 0x0400105A RID: 4186
	public string prefabId;

	// Token: 0x0400105B RID: 4187
	public string symbolname;
}
