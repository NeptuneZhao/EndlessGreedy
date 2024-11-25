using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E57 RID: 3671
	public class ClothingItemResource : PermitResource
	{
		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x0600745D RID: 29789 RVA: 0x002D0800 File Offset: 0x002CEA00
		// (set) Token: 0x0600745E RID: 29790 RVA: 0x002D0808 File Offset: 0x002CEA08
		public string animFilename { get; private set; }

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x0600745F RID: 29791 RVA: 0x002D0811 File Offset: 0x002CEA11
		// (set) Token: 0x06007460 RID: 29792 RVA: 0x002D0819 File Offset: 0x002CEA19
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06007461 RID: 29793 RVA: 0x002D0822 File Offset: 0x002CEA22
		// (set) Token: 0x06007462 RID: 29794 RVA: 0x002D082A File Offset: 0x002CEA2A
		public ClothingOutfitUtility.OutfitType outfitType { get; private set; }

		// Token: 0x06007463 RID: 29795 RVA: 0x002D0834 File Offset: 0x002CEA34
		[Obsolete("Please use constructor with dlcIds parameter")]
		public ClothingItemResource(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile) : this(id, name, desc, outfitType, category, rarity, animFile, DlcManager.AVAILABLE_ALL_VERSIONS)
		{
		}

		// Token: 0x06007464 RID: 29796 RVA: 0x002D0857 File Offset: 0x002CEA57
		public ClothingItemResource(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile, string[] dlcIds) : base(id, name, desc, category, rarity, dlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFile);
			this.animFilename = animFile;
			this.outfitType = outfitType;
		}

		// Token: 0x06007465 RID: 29797 RVA: 0x002D088C File Offset: 0x002CEA8C
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			if (this.AnimFile == null)
			{
				Debug.LogError("Clothing kanim is missing from bundle: " + this.animFilename);
			}
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.CLOTHING_ITEM_FACADE_FOR);
			return result;
		}
	}
}
