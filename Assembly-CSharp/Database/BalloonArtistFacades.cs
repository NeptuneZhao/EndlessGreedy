using System;

namespace Database
{
	// Token: 0x02000E4B RID: 3659
	public class BalloonArtistFacades : ResourceSet<BalloonArtistFacadeResource>
	{
		// Token: 0x06007430 RID: 29744 RVA: 0x002C7CF8 File Offset: 0x002C5EF8
		public BalloonArtistFacades(ResourceSet parent) : base("BalloonArtistFacades", parent)
		{
			foreach (BalloonArtistFacadeInfo balloonArtistFacadeInfo in Blueprints.Get().all.balloonArtistFacades)
			{
				this.Add(balloonArtistFacadeInfo.id, balloonArtistFacadeInfo.name, balloonArtistFacadeInfo.desc, balloonArtistFacadeInfo.rarity, balloonArtistFacadeInfo.animFile, balloonArtistFacadeInfo.balloonFacadeType, balloonArtistFacadeInfo.dlcIds);
			}
		}

		// Token: 0x06007431 RID: 29745 RVA: 0x002C7D8C File Offset: 0x002C5F8C
		[Obsolete("Please use Add(...) with dlcIds parameter")]
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType)
		{
			this.Add(id, name, desc, rarity, animFile, balloonFacadeType, DlcManager.AVAILABLE_ALL_VERSIONS);
		}

		// Token: 0x06007432 RID: 29746 RVA: 0x002C7DA4 File Offset: 0x002C5FA4
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] dlcIds)
		{
			BalloonArtistFacadeResource item = new BalloonArtistFacadeResource(id, name, desc, rarity, animFile, balloonFacadeType, dlcIds);
			this.resources.Add(item);
		}
	}
}
