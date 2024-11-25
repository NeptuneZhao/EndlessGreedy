using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E4D RID: 3661
	public class BalloonArtistFacadeResource : PermitResource
	{
		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06007433 RID: 29747 RVA: 0x002C7DCE File Offset: 0x002C5FCE
		// (set) Token: 0x06007434 RID: 29748 RVA: 0x002C7DD6 File Offset: 0x002C5FD6
		public string animFilename { get; private set; }

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06007435 RID: 29749 RVA: 0x002C7DDF File Offset: 0x002C5FDF
		// (set) Token: 0x06007436 RID: 29750 RVA: 0x002C7DE7 File Offset: 0x002C5FE7
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x06007437 RID: 29751 RVA: 0x002C7DF0 File Offset: 0x002C5FF0
		[Obsolete("Please use constructor with dlcIds parameter")]
		public BalloonArtistFacadeResource(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType) : this(id, name, desc, rarity, animFile, balloonFacadeType, DlcManager.AVAILABLE_ALL_VERSIONS)
		{
		}

		// Token: 0x06007438 RID: 29752 RVA: 0x002C7E08 File Offset: 0x002C6008
		public BalloonArtistFacadeResource(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] dlcIds) : base(id, name, desc, PermitCategory.JoyResponse, rarity, dlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFile);
			this.animFilename = animFile;
			this.balloonFacadeType = balloonFacadeType;
			Db.Get().Accessories.AddAccessories(id, this.AnimFile);
			this.balloonOverrideSymbolIDs = this.GetBalloonOverrideSymbolIDs();
			Debug.Assert(this.balloonOverrideSymbolIDs.Length != 0);
		}

		// Token: 0x06007439 RID: 29753 RVA: 0x002C7E78 File Offset: 0x002C6078
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.BALLOON_ARTIST_FACADE_FOR);
			return result;
		}

		// Token: 0x0600743A RID: 29754 RVA: 0x002C7EBC File Offset: 0x002C60BC
		public BalloonOverrideSymbol GetNextOverride()
		{
			int num = this.nextSymbolIndex;
			this.nextSymbolIndex = (this.nextSymbolIndex + 1) % this.balloonOverrideSymbolIDs.Length;
			return new BalloonOverrideSymbol(this.animFilename, this.balloonOverrideSymbolIDs[num]);
		}

		// Token: 0x0600743B RID: 29755 RVA: 0x002C7EFA File Offset: 0x002C60FA
		public BalloonOverrideSymbolIter GetSymbolIter()
		{
			return new BalloonOverrideSymbolIter(this);
		}

		// Token: 0x0600743C RID: 29756 RVA: 0x002C7F07 File Offset: 0x002C6107
		public BalloonOverrideSymbol GetOverrideAt(int index)
		{
			return new BalloonOverrideSymbol(this.animFilename, this.balloonOverrideSymbolIDs[index]);
		}

		// Token: 0x0600743D RID: 29757 RVA: 0x002C7F1C File Offset: 0x002C611C
		private string[] GetBalloonOverrideSymbolIDs()
		{
			KAnim.Build build = this.AnimFile.GetData().build;
			BalloonArtistFacadeType balloonArtistFacadeType = this.balloonFacadeType;
			string[] result;
			if (balloonArtistFacadeType != BalloonArtistFacadeType.Single)
			{
				if (balloonArtistFacadeType != BalloonArtistFacadeType.ThreeSet)
				{
					throw new NotImplementedException();
				}
				result = new string[]
				{
					"body1",
					"body2",
					"body3"
				};
			}
			else
			{
				result = new string[]
				{
					"body"
				};
			}
			return result;
		}

		// Token: 0x04005052 RID: 20562
		private BalloonArtistFacadeType balloonFacadeType;

		// Token: 0x04005053 RID: 20563
		public readonly string[] balloonOverrideSymbolIDs;

		// Token: 0x04005054 RID: 20564
		public int nextSymbolIndex;
	}
}
