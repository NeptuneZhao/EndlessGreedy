using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E4F RID: 3663
	public class BalloonOverrideSymbolIter
	{
		// Token: 0x06007441 RID: 29761 RVA: 0x002C8014 File Offset: 0x002C6214
		public BalloonOverrideSymbolIter(Option<BalloonArtistFacadeResource> facade)
		{
			global::Debug.Assert(facade.IsNone() || facade.Unwrap().balloonOverrideSymbolIDs.Length != 0);
			this.facade = facade;
			if (facade.IsSome())
			{
				this.index = UnityEngine.Random.Range(0, facade.Unwrap().balloonOverrideSymbolIDs.Length);
			}
			this.Next();
		}

		// Token: 0x06007442 RID: 29762 RVA: 0x002C8079 File Offset: 0x002C6279
		public BalloonOverrideSymbol Current()
		{
			return this.current;
		}

		// Token: 0x06007443 RID: 29763 RVA: 0x002C8084 File Offset: 0x002C6284
		public BalloonOverrideSymbol Next()
		{
			if (this.facade.IsSome())
			{
				BalloonArtistFacadeResource balloonArtistFacadeResource = this.facade.Unwrap();
				this.current = new BalloonOverrideSymbol(balloonArtistFacadeResource.animFilename, balloonArtistFacadeResource.balloonOverrideSymbolIDs[this.index]);
				this.index = (this.index + 1) % balloonArtistFacadeResource.balloonOverrideSymbolIDs.Length;
				return this.current;
			}
			return default(BalloonOverrideSymbol);
		}

		// Token: 0x04005059 RID: 20569
		public readonly Option<BalloonArtistFacadeResource> facade;

		// Token: 0x0400505A RID: 20570
		private BalloonOverrideSymbol current;

		// Token: 0x0400505B RID: 20571
		private int index;
	}
}
