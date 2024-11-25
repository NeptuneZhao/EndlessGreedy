using System;

namespace Database
{
	// Token: 0x02000E85 RID: 3717
	public class DbStickerBomb : PermitResource
	{
		// Token: 0x060074FA RID: 29946 RVA: 0x002DC3E3 File Offset: 0x002DA5E3
		[Obsolete("Please use constructor with dlcIds parameter")]
		public DbStickerBomb(string id, string name, string desc, PermitRarity rarity, string animfilename, string sticker) : this(id, name, desc, rarity, animfilename, sticker, DlcManager.AVAILABLE_ALL_VERSIONS)
		{
		}

		// Token: 0x060074FB RID: 29947 RVA: 0x002DC3F9 File Offset: 0x002DA5F9
		public DbStickerBomb(string id, string name, string desc, PermitRarity rarity, string animfilename, string sticker, string[] dlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, dlcIds)
		{
			this.id = id;
			this.sticker = sticker;
			this.animFile = Assets.GetAnim(animfilename);
		}

		// Token: 0x060074FC RID: 29948 RVA: 0x002DC42C File Offset: 0x002DA62C
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			return new PermitPresentationInfo
			{
				sprite = Def.GetUISpriteFromMultiObjectAnim(this.animFile, string.Format("{0}_{1}", "idle_sticker", this.sticker), false, string.Format("{0}_{1}", "sticker", this.sticker))
			};
		}

		// Token: 0x040054E8 RID: 21736
		public string id;

		// Token: 0x040054E9 RID: 21737
		public string sticker;

		// Token: 0x040054EA RID: 21738
		public KAnimFile animFile;

		// Token: 0x040054EB RID: 21739
		private const string stickerAnimPrefix = "idle_sticker";

		// Token: 0x040054EC RID: 21740
		private const string stickerSymbolPrefix = "sticker";
	}
}
