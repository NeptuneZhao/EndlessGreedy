using System;
using Database;

// Token: 0x02000519 RID: 1305
public class BalloonArtistFacadeInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06001D18 RID: 7448 RVA: 0x00098FC8 File Offset: 0x000971C8
	// (set) Token: 0x06001D19 RID: 7449 RVA: 0x00098FD0 File Offset: 0x000971D0
	public string id { get; set; }

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06001D1A RID: 7450 RVA: 0x00098FD9 File Offset: 0x000971D9
	// (set) Token: 0x06001D1B RID: 7451 RVA: 0x00098FE1 File Offset: 0x000971E1
	public string name { get; set; }

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06001D1C RID: 7452 RVA: 0x00098FEA File Offset: 0x000971EA
	// (set) Token: 0x06001D1D RID: 7453 RVA: 0x00098FF2 File Offset: 0x000971F2
	public string desc { get; set; }

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06001D1E RID: 7454 RVA: 0x00098FFB File Offset: 0x000971FB
	// (set) Token: 0x06001D1F RID: 7455 RVA: 0x00099003 File Offset: 0x00097203
	public PermitRarity rarity { get; set; }

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06001D20 RID: 7456 RVA: 0x0009900C File Offset: 0x0009720C
	// (set) Token: 0x06001D21 RID: 7457 RVA: 0x00099014 File Offset: 0x00097214
	public string animFile { get; set; }

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06001D22 RID: 7458 RVA: 0x0009901D File Offset: 0x0009721D
	// (set) Token: 0x06001D23 RID: 7459 RVA: 0x00099025 File Offset: 0x00097225
	public string[] dlcIds { get; set; }

	// Token: 0x06001D24 RID: 7460 RVA: 0x0009902E File Offset: 0x0009722E
	public BalloonArtistFacadeInfo(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFile;
		this.balloonFacadeType = balloonFacadeType;
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x04001072 RID: 4210
	public BalloonArtistFacadeType balloonFacadeType;
}
