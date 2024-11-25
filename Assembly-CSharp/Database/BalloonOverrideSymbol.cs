using System;

namespace Database
{
	// Token: 0x02000E4E RID: 3662
	public readonly struct BalloonOverrideSymbol
	{
		// Token: 0x0600743E RID: 29758 RVA: 0x002C7F84 File Offset: 0x002C6184
		public BalloonOverrideSymbol(string animFileID, string animFileSymbolID)
		{
			if (string.IsNullOrEmpty(animFileID) || string.IsNullOrEmpty(animFileSymbolID))
			{
				this = default(BalloonOverrideSymbol);
				return;
			}
			this.animFileID = animFileID;
			this.animFileSymbolID = animFileSymbolID;
			this.animFile = Assets.GetAnim(animFileID);
			this.symbol = this.animFile.Value.GetData().build.GetSymbol(animFileSymbolID);
		}

		// Token: 0x0600743F RID: 29759 RVA: 0x002C7FF8 File Offset: 0x002C61F8
		public void ApplyTo(BalloonArtist.Instance artist)
		{
			artist.SetBalloonSymbolOverride(this);
		}

		// Token: 0x06007440 RID: 29760 RVA: 0x002C8006 File Offset: 0x002C6206
		public void ApplyTo(BalloonFX.Instance balloon)
		{
			balloon.SetBalloonSymbolOverride(this);
		}

		// Token: 0x04005055 RID: 20565
		public readonly Option<KAnim.Build.Symbol> symbol;

		// Token: 0x04005056 RID: 20566
		public readonly Option<KAnimFile> animFile;

		// Token: 0x04005057 RID: 20567
		public readonly string animFileID;

		// Token: 0x04005058 RID: 20568
		public readonly string animFileSymbolID;
	}
}
