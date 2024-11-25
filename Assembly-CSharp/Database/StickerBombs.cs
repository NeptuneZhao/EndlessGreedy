using System;

namespace Database
{
	// Token: 0x02000E86 RID: 3718
	public class StickerBombs : ResourceSet<DbStickerBomb>
	{
		// Token: 0x060074FD RID: 29949 RVA: 0x002DC480 File Offset: 0x002DA680
		public StickerBombs(ResourceSet parent) : base("StickerBombs", parent)
		{
			foreach (StickerBombFacadeInfo stickerBombFacadeInfo in Blueprints.Get().all.stickerBombFacades)
			{
				this.Add(stickerBombFacadeInfo.id, stickerBombFacadeInfo.name, stickerBombFacadeInfo.desc, stickerBombFacadeInfo.rarity, stickerBombFacadeInfo.animFile, stickerBombFacadeInfo.sticker, stickerBombFacadeInfo.dlcIds);
			}
		}

		// Token: 0x060074FE RID: 29950 RVA: 0x002DC514 File Offset: 0x002DA714
		[Obsolete("Please use Add(...) with dlcIds parameter")]
		private DbStickerBomb Add(string id, string name, string desc, PermitRarity rarity, string animfilename, string symbolName)
		{
			return this.Add(id, name, desc, rarity, animfilename, symbolName, DlcManager.AVAILABLE_ALL_VERSIONS);
		}

		// Token: 0x060074FF RID: 29951 RVA: 0x002DC52C File Offset: 0x002DA72C
		private DbStickerBomb Add(string id, string name, string desc, PermitRarity rarity, string animfilename, string symbolName, string[] dlcIds)
		{
			DbStickerBomb dbStickerBomb = new DbStickerBomb(id, name, desc, rarity, animfilename, symbolName, dlcIds);
			this.resources.Add(dbStickerBomb);
			return dbStickerBomb;
		}

		// Token: 0x06007500 RID: 29952 RVA: 0x002DC557 File Offset: 0x002DA757
		public DbStickerBomb GetRandomSticker()
		{
			return this.resources.GetRandom<DbStickerBomb>();
		}
	}
}
