using System;
using Database;

// Token: 0x02000517 RID: 1303
public class ClothingItemInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06001CFE RID: 7422 RVA: 0x00098E23 File Offset: 0x00097023
	// (set) Token: 0x06001CFF RID: 7423 RVA: 0x00098E2B File Offset: 0x0009702B
	public string id { get; set; }

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06001D00 RID: 7424 RVA: 0x00098E34 File Offset: 0x00097034
	// (set) Token: 0x06001D01 RID: 7425 RVA: 0x00098E3C File Offset: 0x0009703C
	public string name { get; set; }

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06001D02 RID: 7426 RVA: 0x00098E45 File Offset: 0x00097045
	// (set) Token: 0x06001D03 RID: 7427 RVA: 0x00098E4D File Offset: 0x0009704D
	public string desc { get; set; }

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06001D04 RID: 7428 RVA: 0x00098E56 File Offset: 0x00097056
	// (set) Token: 0x06001D05 RID: 7429 RVA: 0x00098E5E File Offset: 0x0009705E
	public PermitRarity rarity { get; set; }

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06001D06 RID: 7430 RVA: 0x00098E67 File Offset: 0x00097067
	// (set) Token: 0x06001D07 RID: 7431 RVA: 0x00098E6F File Offset: 0x0009706F
	public string animFile { get; set; }

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06001D08 RID: 7432 RVA: 0x00098E78 File Offset: 0x00097078
	// (set) Token: 0x06001D09 RID: 7433 RVA: 0x00098E80 File Offset: 0x00097080
	public string[] dlcIds { get; set; }

	// Token: 0x06001D0A RID: 7434 RVA: 0x00098E8C File Offset: 0x0009708C
	public ClothingItemInfo(string id, string name, string desc, PermitCategory category, PermitRarity rarity, string animFile)
	{
		Option<ClothingOutfitUtility.OutfitType> outfitTypeFor = PermitCategories.GetOutfitTypeFor(category);
		if (outfitTypeFor.IsNone())
		{
			throw new Exception(string.Format("Expected permit category {0} on ClothingItemResource \"{1}\" to have an {2} but none found.", category, id, "OutfitType"));
		}
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.outfitType = outfitTypeFor.Unwrap();
		this.category = category;
		this.rarity = rarity;
		this.animFile = animFile;
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x04001060 RID: 4192
	public ClothingOutfitUtility.OutfitType outfitType;

	// Token: 0x04001061 RID: 4193
	public PermitCategory category;
}
