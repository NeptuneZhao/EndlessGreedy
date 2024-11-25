using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E59 RID: 3673
	public class ClothingOutfitResource : Resource, IBlueprintDlcInfo
	{
		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06007467 RID: 29799 RVA: 0x002D0A74 File Offset: 0x002CEC74
		// (set) Token: 0x06007468 RID: 29800 RVA: 0x002D0A7C File Offset: 0x002CEC7C
		public string[] itemsInOutfit { get; private set; }

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06007469 RID: 29801 RVA: 0x002D0A85 File Offset: 0x002CEC85
		// (set) Token: 0x0600746A RID: 29802 RVA: 0x002D0A8D File Offset: 0x002CEC8D
		public string[] dlcIds { get; set; } = DlcManager.AVAILABLE_ALL_VERSIONS;

		// Token: 0x0600746B RID: 29803 RVA: 0x002D0A96 File Offset: 0x002CEC96
		public ClothingOutfitResource(string id, string[] items_in_outfit, string name, ClothingOutfitUtility.OutfitType outfitType) : base(id, name)
		{
			this.itemsInOutfit = items_in_outfit;
			this.outfitType = outfitType;
		}

		// Token: 0x0600746C RID: 29804 RVA: 0x002D0ABA File Offset: 0x002CECBA
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			Sprite sprite = Assets.GetSprite("unknown");
			return new global::Tuple<Sprite, Color>(sprite, (sprite != null) ? Color.white : Color.clear);
		}

		// Token: 0x0600746D RID: 29805 RVA: 0x002D0AE5 File Offset: 0x002CECE5
		public string GetDlcIdFrom()
		{
			if (this.dlcIds == DlcManager.AVAILABLE_ALL_VERSIONS || this.dlcIds == DlcManager.AVAILABLE_VANILLA_ONLY)
			{
				return null;
			}
			if (this.dlcIds.Length == 0)
			{
				return null;
			}
			return this.dlcIds[0];
		}

		// Token: 0x04005241 RID: 21057
		public ClothingOutfitUtility.OutfitType outfitType;
	}
}
