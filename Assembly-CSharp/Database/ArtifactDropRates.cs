using System;
using TUNING;

namespace Database
{
	// Token: 0x02000E48 RID: 3656
	public class ArtifactDropRates : ResourceSet<ArtifactDropRate>
	{
		// Token: 0x0600742A RID: 29738 RVA: 0x002C6A5F File Offset: 0x002C4C5F
		public ArtifactDropRates(ResourceSet parent) : base("ArtifactDropRates", parent)
		{
			this.CreateDropRates();
		}

		// Token: 0x0600742B RID: 29739 RVA: 0x002C6A74 File Offset: 0x002C4C74
		private void CreateDropRates()
		{
			this.None = new ArtifactDropRate();
			this.None.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 1f);
			base.Add(this.None);
			this.Bad = new ArtifactDropRate();
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER0, 5f);
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER1, 3f);
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER2, 2f);
			base.Add(this.Bad);
			this.Mediocre = new ArtifactDropRate();
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER1, 5f);
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER2, 3f);
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER3, 2f);
			base.Add(this.Mediocre);
			this.Good = new ArtifactDropRate();
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER2, 5f);
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER3, 3f);
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER4, 2f);
			base.Add(this.Good);
			this.Great = new ArtifactDropRate();
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER3, 5f);
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER4, 3f);
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER5, 2f);
			base.Add(this.Great);
			this.Amazing = new ArtifactDropRate();
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER3, 3f);
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER4, 5f);
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER5, 2f);
			base.Add(this.Amazing);
			this.Perfect = new ArtifactDropRate();
			this.Perfect.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Perfect.AddItem(DECOR.SPACEARTIFACT.TIER4, 6f);
			this.Perfect.AddItem(DECOR.SPACEARTIFACT.TIER5, 4f);
			base.Add(this.Perfect);
		}

		// Token: 0x04005003 RID: 20483
		public ArtifactDropRate None;

		// Token: 0x04005004 RID: 20484
		public ArtifactDropRate Bad;

		// Token: 0x04005005 RID: 20485
		public ArtifactDropRate Mediocre;

		// Token: 0x04005006 RID: 20486
		public ArtifactDropRate Good;

		// Token: 0x04005007 RID: 20487
		public ArtifactDropRate Great;

		// Token: 0x04005008 RID: 20488
		public ArtifactDropRate Amazing;

		// Token: 0x04005009 RID: 20489
		public ArtifactDropRate Perfect;
	}
}
