using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000E72 RID: 3698
	public class PermitResources : ResourceSet<PermitResource>
	{
		// Token: 0x060074BC RID: 29884 RVA: 0x002D824C File Offset: 0x002D644C
		public PermitResources(ResourceSet parent) : base("PermitResources", parent)
		{
			this.Root = new ResourceSet<Resource>("Root", null);
			this.Permits = new Dictionary<string, IEnumerable<PermitResource>>();
			this.BuildingFacades = new BuildingFacades(this.Root);
			this.Permits.Add(this.BuildingFacades.Id, this.BuildingFacades.resources);
			this.EquippableFacades = new EquippableFacades(this.Root);
			this.Permits.Add(this.EquippableFacades.Id, this.EquippableFacades.resources);
			this.ArtableStages = new ArtableStages(this.Root);
			this.Permits.Add(this.ArtableStages.Id, this.ArtableStages.resources);
			this.StickerBombs = new StickerBombs(this.Root);
			this.Permits.Add(this.StickerBombs.Id, this.StickerBombs.resources);
			this.ClothingItems = new ClothingItems(this.Root);
			this.ClothingOutfits = new ClothingOutfits(this.Root, this.ClothingItems);
			this.Permits.Add(this.ClothingItems.Id, this.ClothingItems.resources);
			this.BalloonArtistFacades = new BalloonArtistFacades(this.Root);
			this.Permits.Add(this.BalloonArtistFacades.Id, this.BalloonArtistFacades.resources);
			this.MonumentParts = new MonumentParts(this.Root);
			foreach (IEnumerable<PermitResource> collection in this.Permits.Values)
			{
				this.resources.AddRange(collection);
			}
		}

		// Token: 0x060074BD RID: 29885 RVA: 0x002D8428 File Offset: 0x002D6628
		public void PostProcess()
		{
			this.BuildingFacades.PostProcess();
		}

		// Token: 0x04005441 RID: 21569
		public ResourceSet Root;

		// Token: 0x04005442 RID: 21570
		public BuildingFacades BuildingFacades;

		// Token: 0x04005443 RID: 21571
		public EquippableFacades EquippableFacades;

		// Token: 0x04005444 RID: 21572
		public ArtableStages ArtableStages;

		// Token: 0x04005445 RID: 21573
		public StickerBombs StickerBombs;

		// Token: 0x04005446 RID: 21574
		public ClothingItems ClothingItems;

		// Token: 0x04005447 RID: 21575
		public ClothingOutfits ClothingOutfits;

		// Token: 0x04005448 RID: 21576
		public MonumentParts MonumentParts;

		// Token: 0x04005449 RID: 21577
		public BalloonArtistFacades BalloonArtistFacades;

		// Token: 0x0400544A RID: 21578
		public Dictionary<string, IEnumerable<PermitResource>> Permits;
	}
}
